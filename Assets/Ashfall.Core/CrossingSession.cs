using System;

namespace Ashfall.Core
{
    /// <summary>
    /// Nobody's Charter pack minimum: vouch gate actually gates Crossing travel.
    /// Godot selftest / Core tests drive this. Does not port GameBootstrap.
    /// </summary>
    public sealed class CrossingSession
    {
        public VouchAccessSystem Vouch { get; }
        public CrossingCatalog Catalog { get; }

        public CrossingSession(VouchAccessSystem vouch, CrossingCatalog catalog)
        {
            Vouch = vouch ?? new VouchAccessSystem();
            Catalog = catalog ?? new CrossingCatalog();
        }

        public static CrossingSession Load(string dataDirectory, ILog log = null)
        {
            var loader = new CrossingCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer(), log);
            return new CrossingSession(new VouchAccessSystem(), loader.Load(dataDirectory));
        }

        /// <summary>True when the player may currently pass the viaduct.</summary>
        public bool GateAllowsCrossing() => Vouch != null && Vouch.HasAccess;

        public static bool IsCrossingNode(string nodeId) =>
            !string.IsNullOrEmpty(nodeId) && nodeId.StartsWith("loc_crossing_", StringComparison.Ordinal);

        /// <summary>Social gate: Crossing nodes are blocked until a name is staked.</summary>
        public bool IsTravelBlocked(string nodeId)
        {
            if (!IsCrossingNode(nodeId)) return false;
            return !GateAllowsCrossing();
        }

        public bool TryVouch(string npcId, bool lastResort = false) =>
            Vouch.GrantVouch(npcId, lastResort);

        public bool BurnVouch() => Vouch.BurnVouch();

        public void SoftenAccess() => Vouch.SoftenAccess();
    }
}
