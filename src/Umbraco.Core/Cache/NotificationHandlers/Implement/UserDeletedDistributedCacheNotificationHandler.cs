using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.Membership;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Extensions;

namespace Umbraco.Cms.Core.Cache;

/// <inheritdoc />
public sealed class UserDeletedDistributedCacheNotificationHandler : DeletedDistributedCacheNotificationHandlerBase<IUser, UserDeletedNotification>
{
    private readonly DistributedCache _distributedCache;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserDeletedDistributedCacheNotificationHandler" /> class.
    /// </summary>
    /// <param name="distributedCache">The distributed cache.</param>
    public UserDeletedDistributedCacheNotificationHandler(DistributedCache distributedCache)
        => _distributedCache = distributedCache;

    /// <inheritdoc />
    [Obsolete("Scheduled for removal in Umbraco 18.")]
    protected override void Handle(IEnumerable<IUser> entities)
        => Handle(entities, new Dictionary<string, object?>());

    /// <inheritdoc />
    protected override void Handle(IEnumerable<IUser> entities, IDictionary<string, object?> state)
        => _distributedCache.RemoveUserCache(entities);
}
