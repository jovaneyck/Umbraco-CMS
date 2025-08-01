using System.Net;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Umbraco.Cms.Core.Configuration.Models;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Cms.Web.Common.Routing;
using Umbraco.Cms.Web.Common.Security;
using Umbraco.Cms.Web.Website.Controllers;
using Umbraco.Extensions;
using static Umbraco.Cms.Core.Constants.Web.Routing;
using RouteDirection = Umbraco.Cms.Core.Routing.RouteDirection;

namespace Umbraco.Cms.Web.Website.Routing;

/// <summary>
///     The route value transformer for Umbraco front-end routes
/// </summary>
/// <remarks>
///     NOTE: In aspnet 5 DynamicRouteValueTransformer has been improved, see
///     https://github.com/dotnet/aspnetcore/issues/21471
///     It seems as though with the "State" parameter we could more easily assign the IPublishedRequest or
///     IPublishedContent
///     or UmbracoContext more easily that way. In the meantime we will rely on assigning the IPublishedRequest to the
///     route values along with the IPublishedContent to the umbraco context
///     have created a GH discussion here https://github.com/dotnet/aspnetcore/discussions/28562 we'll see if anyone
///     responds
/// </remarks>
public class UmbracoRouteValueTransformer : DynamicRouteValueTransformer
{
    private readonly IControllerActionSearcher _controllerActionSearcher;
    private readonly IDataProtectionProvider _dataProtectionProvider;
    private readonly ILogger<UmbracoRouteValueTransformer> _logger;
    private readonly IPublicAccessRequestHandler _publicAccessRequestHandler;
    private readonly IPublishedRouter _publishedRouter;
    private readonly IRoutableDocumentFilter _routableDocumentFilter;
    private readonly IUmbracoRouteValuesFactory _routeValuesFactory;
    private readonly IRuntimeState _runtime;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly IUmbracoVirtualPageRoute _umbracoVirtualPageRoute;
    private GlobalSettings _globalSettings;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UmbracoRouteValueTransformer" /> class.
    /// </summary>
    public UmbracoRouteValueTransformer(
        ILogger<UmbracoRouteValueTransformer> logger,
        IUmbracoContextAccessor umbracoContextAccessor,
        IPublishedRouter publishedRouter,
        IRuntimeState runtime,
        IUmbracoRouteValuesFactory routeValuesFactory,
        IRoutableDocumentFilter routableDocumentFilter,
        IDataProtectionProvider dataProtectionProvider,
        IControllerActionSearcher controllerActionSearcher,
        IPublicAccessRequestHandler publicAccessRequestHandler,
        IUmbracoVirtualPageRoute umbracoVirtualPageRoute,
        IOptionsMonitor<GlobalSettings> globalSettings)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _umbracoContextAccessor =
            umbracoContextAccessor ?? throw new ArgumentNullException(nameof(umbracoContextAccessor));
        _publishedRouter = publishedRouter ?? throw new ArgumentNullException(nameof(publishedRouter));
        _runtime = runtime ?? throw new ArgumentNullException(nameof(runtime));
        _routeValuesFactory = routeValuesFactory ?? throw new ArgumentNullException(nameof(routeValuesFactory));
        _routableDocumentFilter =
            routableDocumentFilter ?? throw new ArgumentNullException(nameof(routableDocumentFilter));
        _dataProtectionProvider = dataProtectionProvider;
        _controllerActionSearcher = controllerActionSearcher;
        _publicAccessRequestHandler = publicAccessRequestHandler;
        _umbracoVirtualPageRoute = umbracoVirtualPageRoute;
        _globalSettings = globalSettings.CurrentValue;
        globalSettings.OnChange(x => _globalSettings = x);
    }

    /// <inheritdoc />
    public override async ValueTask<RouteValueDictionary> TransformAsync(
        HttpContext httpContext, RouteValueDictionary values)
    {
        // will be null for any client side requests like JS, etc...
        if (!_umbracoContextAccessor.TryGetUmbracoContext(out IUmbracoContext? umbracoContext))
        {
            return null!;
        }

        if (!_routableDocumentFilter.IsDocumentRequest(httpContext.Request.Path))
        {
            return null!;
        }

        if (CheckActiveDynamicRoutingAndNoException(httpContext))
        {
            return null!;
        }

        // Check if there is no existing content and return the no content controller
        // FIXME: This should be changed to route cache, so instead, if there are any routes, we know there is content.
        if (!umbracoContext.Content?.HasContent() ?? false)
        {
            return new RouteValueDictionary
            {
                [ControllerToken] = ControllerExtensions.GetControllerName<RenderNoContentController>(),
                [ActionToken] = nameof(RenderNoContentController.Index),
            };
        }

        IPublishedRequest publishedRequest = await RouteRequestAsync(umbracoContext);

        UmbracoRouteValues? umbracoRouteValues = await _routeValuesFactory.CreateAsync(httpContext, publishedRequest);

        // now we need to do some public access checks
        umbracoRouteValues =
            await _publicAccessRequestHandler.RewriteForPublishedContentAccessAsync(httpContext, umbracoRouteValues);

        // Store the route values as a httpcontext feature
        httpContext.Features.Set(umbracoRouteValues);

        // Need to check if there is form data being posted back to an Umbraco URL
        PostedDataProxyInfo? postedInfo = GetFormInfo(httpContext, values);
        if (postedInfo != null)
        {
            // Ensure the virtual page content and route values are setup when submitting to a surface controller
            // If we don't do this, the virtual page controller never gets called after the surface controller completes
            await _umbracoVirtualPageRoute.SetupVirtualPageRoute(httpContext);

            return HandlePostedValues(postedInfo, httpContext);
        }

        UmbracoRouteResult? routeResult = umbracoRouteValues?.PublishedRequest.GetRouteResult();

        if (!routeResult.HasValue || routeResult == UmbracoRouteResult.NotFound)
        {
            // No content was found, not by any registered 404 handlers and
            // not by the IContentLastChanceFinder. In this case we want to return
            // our default 404 page but we cannot return route values now because
            // it's possible that a developer is handling dynamic routes too.
            // Our 404 page will be handled with the NotFoundSelectorPolicy

            return null!;
        }

        // See https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.mvc.routing.dynamicroutevaluetransformer.transformasync?view=aspnetcore-5.0#Microsoft_AspNetCore_Mvc_Routing_DynamicRouteValueTransformer_TransformAsync_Microsoft_AspNetCore_Http_HttpContext_Microsoft_AspNetCore_Routing_RouteValueDictionary_
        // We should apparenlty not be modified these values.
        // So we create new ones.
        var newValues = new RouteValueDictionary { [ControllerToken] = umbracoRouteValues?.ControllerName };
        if (string.IsNullOrWhiteSpace(umbracoRouteValues?.ActionName) == false)
        {
            newValues[ActionToken] = umbracoRouteValues.ActionName;
        }

        return newValues;
    }

    /// <summary>
    ///     Check whether dynamic routing is currently active in an request where no exception has occured.
    /// </summary>
    /// <returns>[true] if dynamic routing is active, [false] if inactive or an exception has occured.</returns>
    private static bool CheckActiveDynamicRoutingAndNoException(HttpContext httpContext)
    {
        // Don't execute if there are already UmbracoRouteValues assigned.
        // This can occur if someone else is dynamically routing and in which case we don't want to overwrite
        // the routing work being done there.
        UmbracoRouteValues? umbracoRouteValues = httpContext.Features.Get<UmbracoRouteValues>();

        // No dynamic routing is active currently.
        if (umbracoRouteValues == null)
        {
            return false;
        }

        // There is dynamic routing active so we have to check whether an exception occured in the current request.
        // If this is the case we do want dynamic routing since it might be an Umbraco content page which is used as an error page.
        IExceptionHandlerFeature? exceptionHandlerFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

        return exceptionHandlerFeature == null;
    }

    private async Task<IPublishedRequest> RouteRequestAsync(IUmbracoContext umbracoContext)
    {
        // ok, process

        // instantiate, prepare and process the published content request
        // important to use CleanedUmbracoUrl - lowercase path-only version of the current url
        IPublishedRequestBuilder requestBuilder =
            await _publishedRouter.CreateRequestAsync(umbracoContext.CleanedUmbracoUrl);

        // TODO: This is ugly with the re-assignment to umbraco context but at least its now
        // an immutable object. The only way to make this better would be to have a RouteRequest
        // as part of UmbracoContext but then it will require a PublishedRouter dependency so not sure that's worth it.
        // Maybe could be a one-time Set method instead?
        IPublishedRequest routedRequest =
            await _publishedRouter.RouteRequestAsync(requestBuilder, new RouteRequestOptions(RouteDirection.Inbound));
        umbracoContext.PublishedRequest = routedRequest;

        return routedRequest;
    }

    /// <summary>
    ///     Checks the request and query strings to see if it matches the definition of having a Surface controller
    ///     posted/get value, if so, then we return a PostedDataProxyInfo object with the correct information.
    /// </summary>
    private PostedDataProxyInfo? GetFormInfo(HttpContext httpContext, RouteValueDictionary values)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        // if it is a POST/GET then a `ufprt` value must be in the request
        var ufprt = httpContext.Request.GetUfprt();
        if (string.IsNullOrWhiteSpace(ufprt))
        {
            return null;
        }

        if (!EncryptionHelper.DecryptAndValidateEncryptedRouteString(
                _dataProtectionProvider,
                ufprt,
                out IDictionary<string, string?>? decodedUfprt))
        {
            return null;
        }

        // Get all route values that are not the default ones and add them separately so they eventually get to action parameters
        foreach (KeyValuePair<string, string?> item in decodedUfprt.Where(x =>
                     ReservedAdditionalKeys.AllKeys.Contains(x.Key) == false))
        {
            values[item.Key] = item.Value;
        }

        // return the proxy info without the surface id... could be a local controller.
        return new PostedDataProxyInfo
        {
            ControllerName =
                WebUtility.UrlDecode(decodedUfprt.First(x => x.Key == ReservedAdditionalKeys.Controller).Value),
            ActionName =
                WebUtility.UrlDecode(decodedUfprt.First(x => x.Key == ReservedAdditionalKeys.Action).Value),
            Area = WebUtility.UrlDecode(decodedUfprt.First(x => x.Key == ReservedAdditionalKeys.Area).Value),
        };
    }

    private RouteValueDictionary HandlePostedValues(PostedDataProxyInfo postedInfo, HttpContext httpContext)
    {
        // set the standard route values/tokens
        var values = new RouteValueDictionary
        {
            [ControllerToken] = postedInfo.ControllerName, [ActionToken] = postedInfo.ActionName,
        };

        ControllerActionDescriptor? surfaceControllerDescriptor =
            _controllerActionSearcher.Find<SurfaceController>(httpContext, postedInfo.ControllerName, postedInfo.ActionName, postedInfo.Area);

        if (surfaceControllerDescriptor == null)
        {
            throw new InvalidOperationException(
                "Could not find a Surface controller route in the RouteTable for controller name " +
                postedInfo.ControllerName);
        }

        // set the area if one is there.
        if (!postedInfo.Area.IsNullOrWhiteSpace())
        {
            values["area"] = postedInfo.Area;
        }

        return values;
    }

    private sealed class PostedDataProxyInfo
    {
        public string? ControllerName { get; set; }

        public string? ActionName { get; set; }

        public string? Area { get; set; }
    }

    // Define reserved dictionary keys for controller, action and area specified in route additional values data
    private static class ReservedAdditionalKeys
    {
        internal const string Controller = "c";
        internal const string Action = "a";
        internal const string Area = "ar";

        internal static readonly string[] AllKeys = { Controller, Action, Area };
    }
}
