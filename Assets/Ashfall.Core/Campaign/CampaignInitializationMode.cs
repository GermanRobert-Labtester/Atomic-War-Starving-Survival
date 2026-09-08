namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Explicit boundary between creating a campaign and restoring one.
    /// Starting defaults are legal only during FreshInitialize.
    /// </summary>
    public enum CampaignInitializationMode
    {
        Restore = 0,
        FreshInitialize = 1
    }
}
