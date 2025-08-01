using Moq;
using NUnit.Framework;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.DeliveryApi;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Services.Navigation;
using Umbraco.Cms.Tests.Common;

namespace Umbraco.Cms.Tests.UnitTests.Umbraco.Core.DeliveryApi;

[TestFixture]
public class ContentBuilderTests : DeliveryApiTests
{
    [Test]
    public void ContentBuilder_MapsContentDataAndPropertiesCorrectly()
    {
        var content = new Mock<IPublishedContent>();

        var prop1 = new PublishedElementPropertyBase(DeliveryApiPropertyType, content.Object, false, PropertyCacheLevel.None, new VariationContext(), Mock.Of<ICacheManager>());
        var prop2 = new PublishedElementPropertyBase(DefaultPropertyType, content.Object, false, PropertyCacheLevel.None, new VariationContext(), Mock.Of<ICacheManager>());

        var contentType = new Mock<IPublishedContentType>();
        contentType.SetupGet(c => c.Alias).Returns("thePageType");
        contentType.SetupGet(c => c.ItemType).Returns(PublishedItemType.Content);

        var key = Guid.NewGuid();
        var urlSegment = "url-segment";
        var name = "The page";
        ConfigurePublishedContentMock(content, key, name, urlSegment, contentType.Object, new[] { prop1, prop2 });
        content.SetupGet(c => c.CreateDate).Returns(new DateTime(2023, 06, 01));
        content.SetupGet(c => c.UpdateDate).Returns(new DateTime(2023, 07, 12));

        var apiContentRouteProvider = new Mock<IApiContentPathProvider>();
        apiContentRouteProvider
            .Setup(p => p.GetContentPath(It.IsAny<IPublishedContent>(), It.IsAny<string?>()))
            .Returns((IPublishedContent c, string? culture) => $"url:{c.UrlSegment}");

        var navigationQueryServiceMock = new Mock<IDocumentNavigationQueryService>();
        IEnumerable<Guid> ancestorsKeys = [];
        navigationQueryServiceMock.Setup(x => x.TryGetAncestorsKeys(key, out ancestorsKeys)).Returns(true);

        var routeBuilder = CreateContentRouteBuilder(apiContentRouteProvider.Object, CreateGlobalSettings(), navigationQueryService: navigationQueryServiceMock.Object);

        var builder = new ApiContentBuilder(new ApiContentNameProvider(), routeBuilder, CreateOutputExpansionStrategyAccessor(), CreateVariationContextAccessor());
        var result = builder.Build(content.Object);

        Assert.NotNull(result);
        Assert.AreEqual("The page", result.Name);
        Assert.AreEqual("thePageType", result.ContentType);
        Assert.AreEqual("/url:url-segment/", result.Route.Path);
        Assert.AreEqual(key, result.Id);
        Assert.AreEqual(2, result.Properties.Count);
        Assert.AreEqual("Delivery API value", result.Properties["deliveryApi"]);
        Assert.AreEqual("Default value", result.Properties["default"]);
        Assert.AreEqual(new DateTime(2023, 06, 01), result.CreateDate);
        Assert.AreEqual(new DateTime(2023, 07, 12), result.UpdateDate);
    }

    [TestCase("en-US", "2023-08-04")]
    [TestCase("da-DK", "2023-09-08")]
    public void ContentBuilder_MapsContentDatesCorrectlyForCultureVariance(string culture, string expectedUpdateDate)
    {
        var content = new Mock<IPublishedContent>();

        var contentType = new Mock<IPublishedContentType>();
        contentType.SetupGet(c => c.Alias).Returns("thePageType");
        contentType.SetupGet(c => c.ItemType).Returns(PublishedItemType.Content);
        contentType.SetupGet(c => c.Variations).Returns(ContentVariation.Culture);

        var key = Guid.NewGuid();
        var urlSegment = "url-segment";
        var name = "The page";
        ConfigurePublishedContentMock(content, key, name, urlSegment, contentType.Object, []);
        content.SetupGet(c => c.CreateDate).Returns(new DateTime(2023, 07, 02));
        content
            .SetupGet(c => c.Cultures)
            .Returns(new Dictionary<string, PublishedCultureInfo>
            {
                { "en-US", new PublishedCultureInfo("en-US", "EN Name", "en-url-segment", new DateTime(2023, 08, 04)) },
                { "da-DK", new PublishedCultureInfo("da-DK", "DA Name", "da-url-segment", new DateTime(2023, 09, 08)) },
            });

        var routeBuilder = new Mock<IApiContentRouteBuilder>();
        routeBuilder
            .Setup(r => r.Build(content.Object, It.IsAny<string?>()))
            .Returns(new ApiContentRoute(content.Object.UrlSegment!, new ApiContentStartItem(Guid.NewGuid(), "/")));

        var variationContextAccessor = new TestVariationContextAccessor { VariationContext = new VariationContext(culture) };

        var builder = new ApiContentBuilder(new ApiContentNameProvider(), routeBuilder.Object, CreateOutputExpansionStrategyAccessor(), variationContextAccessor);
        var result = builder.Build(content.Object);

        Assert.NotNull(result);
        Assert.AreEqual(new DateTime(2023, 07, 02), result.CreateDate);
        Assert.AreEqual(DateTime.Parse(expectedUpdateDate), result.UpdateDate);
    }

    [Test]
    public void ContentBuilder_CanCustomizeContentNameInDeliveryApiOutput()
    {
        var content = new Mock<IPublishedContent>();

        var contentType = new Mock<IPublishedContentType>();
        contentType.SetupGet(c => c.Alias).Returns("thePageType");

        ConfigurePublishedContentMock(content, Guid.NewGuid(), "The page", "the-page", contentType.Object, Array.Empty<PublishedElementPropertyBase>());

        var customNameProvider = new Mock<IApiContentNameProvider>();
        customNameProvider.Setup(n => n.GetName(content.Object)).Returns($"Custom name for: {content.Object.Name}");

        var routeBuilder = new Mock<IApiContentRouteBuilder>();
        routeBuilder
            .Setup(r => r.Build(content.Object, It.IsAny<string?>()))
            .Returns(new ApiContentRoute(content.Object.UrlSegment!, new ApiContentStartItem(Guid.NewGuid(), "/")));

        var builder = new ApiContentBuilder(customNameProvider.Object, routeBuilder.Object, CreateOutputExpansionStrategyAccessor(), CreateVariationContextAccessor());
        var result = builder.Build(content.Object);

        Assert.NotNull(result);
        Assert.AreEqual("Custom name for: The page", result.Name);
    }

    [Test]
    public void ContentBuilder_ReturnsNullForUnRoutableContent()
    {
        var content = new Mock<IPublishedContent>();

        var contentType = new Mock<IPublishedContentType>();
        contentType.SetupGet(c => c.Alias).Returns("thePageType");

        ConfigurePublishedContentMock(content, Guid.NewGuid(), "The page", "the-page", contentType.Object, Array.Empty<PublishedElementPropertyBase>());

        var routeBuilder = new Mock<IApiContentRouteBuilder>();
        routeBuilder
            .Setup(r => r.Build(content.Object, It.IsAny<string?>()))
            .Returns((ApiContentRoute)null);

        var builder = new ApiContentBuilder(new ApiContentNameProvider(), routeBuilder.Object, CreateOutputExpansionStrategyAccessor(), CreateVariationContextAccessor());
        var result = builder.Build(content.Object);

        Assert.Null(result);
    }

    [TestCase("Shared value", "Segmented value", false)]
    [TestCase(null, "Segmented value", false)]
    [TestCase("Shared value", null, true)]
    [TestCase(null, null, true)]
    public void ContentBuilder_ReturnsNullForRequestedSegmentThatIsNotCreated(object? sharedValue, object? segmentedValue, bool expectNull)
    {
        var content = new Mock<IPublishedContent>();

        var sharedPropertyValueConverter = new Mock<IDeliveryApiPropertyValueConverter>();
        sharedPropertyValueConverter.Setup(p => p.IsValue(It.IsAny<object?>(), It.IsAny<PropertyValueLevel>())).Returns(sharedValue is not null);
        sharedPropertyValueConverter.Setup(p => p.ConvertIntermediateToDeliveryApiObject(
            It.IsAny<IPublishedElement>(),
            It.IsAny<IPublishedPropertyType>(),
            It.IsAny<PropertyCacheLevel>(),
            It.IsAny<object?>(),
            It.IsAny<bool>(),
            It.IsAny<bool>())).Returns(sharedValue);
        sharedPropertyValueConverter.Setup(p => p.IsConverter(It.IsAny<IPublishedPropertyType>())).Returns(true);
        sharedPropertyValueConverter.Setup(p => p.GetPropertyCacheLevel(It.IsAny<IPublishedPropertyType>())).Returns(PropertyCacheLevel.None);
        sharedPropertyValueConverter.Setup(p => p.GetDeliveryApiPropertyCacheLevel(It.IsAny<IPublishedPropertyType>())).Returns(PropertyCacheLevel.None);

        var sharedPropertyType = SetupPublishedPropertyType(sharedPropertyValueConverter.Object, "sharedMessage", "Umbraco.Textstring");
        var sharedProperty = new PublishedElementPropertyBase(sharedPropertyType, content.Object, false, PropertyCacheLevel.None, new VariationContext(), Mock.Of<ICacheManager>());

        var segmentedPropertyValueConverter = new Mock<IDeliveryApiPropertyValueConverter>();
        segmentedPropertyValueConverter.Setup(p => p.IsValue(It.IsAny<object?>(), It.IsAny<PropertyValueLevel>())).Returns(segmentedValue is not null);
        segmentedPropertyValueConverter.Setup(p => p.ConvertIntermediateToDeliveryApiObject(
            It.IsAny<IPublishedElement>(),
            It.IsAny<IPublishedPropertyType>(),
            It.IsAny<PropertyCacheLevel>(),
            It.IsAny<object?>(),
            It.IsAny<bool>(),
            It.IsAny<bool>())).Returns(segmentedValue);
        segmentedPropertyValueConverter.Setup(p => p.IsConverter(It.IsAny<IPublishedPropertyType>())).Returns(true);
        segmentedPropertyValueConverter.Setup(p => p.GetPropertyCacheLevel(It.IsAny<IPublishedPropertyType>())).Returns(PropertyCacheLevel.None);
        segmentedPropertyValueConverter.Setup(p => p.GetDeliveryApiPropertyCacheLevel(It.IsAny<IPublishedPropertyType>())).Returns(PropertyCacheLevel.None);

        var segmentedPropertyType = SetupPublishedPropertyType(segmentedPropertyValueConverter.Object, "segmentedMessage", "Umbraco.Textstring", contentVariation: ContentVariation.Segment);
        var segmentedProperty = new PublishedElementPropertyBase(segmentedPropertyType, content.Object, false, PropertyCacheLevel.None, new VariationContext(), Mock.Of<ICacheManager>());

        var contentType = new Mock<IPublishedContentType>();
        contentType.SetupGet(c => c.Alias).Returns("thePageType");
        contentType.SetupGet(c => c.ItemType).Returns(PublishedItemType.Content);
        contentType.SetupGet(c => c.Variations).Returns(ContentVariation.Segment);

        var key = Guid.NewGuid();
        var urlSegment = "url-segment";
        var name = "The page";
        ConfigurePublishedContentMock(content, key, name, urlSegment, contentType.Object, [sharedProperty, segmentedProperty]);

        var routeBuilderMock = new Mock<IApiContentRouteBuilder>();
        routeBuilderMock
            .Setup(r => r.Build(content.Object, It.IsAny<string?>()))
            .Returns(new ApiContentRoute(content.Object.UrlSegment!, new ApiContentStartItem(Guid.NewGuid(), "/")));

        var variationContextAccessorMock = new Mock<IVariationContextAccessor>();
        variationContextAccessorMock.Setup(v => v.VariationContext).Returns(new VariationContext(segment: "missingSegment"));

        var builder = new ApiContentBuilder(new ApiContentNameProvider(), routeBuilderMock.Object, CreateOutputExpansionStrategyAccessor(), variationContextAccessorMock.Object);
        var result = builder.Build(content.Object);

        if (expectNull)
        {
            Assert.IsNull(result);
        }
        else
        {
            Assert.IsNotNull(result);
        }
    }
}
