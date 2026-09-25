// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Radiation
{
    public readonly struct RouteDoseCheck
    {
        public RouteDoseCheck(HotspotRow? worst, HotspotRow? safest, int hotspotCount, double totalUsv)
        {
            Worst = worst;
            Safest = safest;
            HotspotCount = hotspotCount;
            TotalUsv = totalUsv;
        }

        /// <summary>Worst-banded place on the route; null when the route is clean.</summary>
        public HotspotRow? Worst { get; }
        /// <summary>Lowest-radiation place checked — the safer alternative.</summary>
        public HotspotRow? Safest { get; }
        public int HotspotCount { get; }
        /// <summary>Sum of the route's baseline readings (µSv/h) — a transit cost, not a dose.</summary>
        public double TotalUsv { get; }

        public bool Clear => Worst == null;
    }

    /// <summary>
    /// Alpha feature G4 — pre-departure route check.
    ///
    /// Read-only projection: given a planned set of places, name the worst band
    /// on it, the safest alternative, and the summed baseline reading. Owns no
    /// state, writes nothing, and cannot be mistaken for the dose ledger, which
    /// remains the only place exposure is booked.
    /// </summary>
    public static class RouteDoseCheckService
    {
        public static RouteDoseCheck Evaluate(
            RadiationHotspotPolicy policy,
            IEnumerable<DoseLocationDef>? plannedPlaces)
        {
            if (plannedPlaces == null) return new RouteDoseCheck(null, null, 0, 0);

            HotspotRow? worst = null;
            HotspotRow? safest = null;
            double total = 0;
            int hotspots = 0;

            foreach (var place in plannedPlaces)
            {
                if (place == null) continue;
                double usv = place.radiationUsv;
                total += usv;

                if (safest == null || usv < safest.Value.RadiationUsv)
                    safest = new HotspotRow(place.id,
                        string.IsNullOrWhiteSpace(place.displayName) ? place.id : place.displayName,
                        place.sector, usv, string.Empty, string.Empty);

                var band = policy?.BandFor(usv);
                if (band == null) continue;
                hotspots++;
                if (worst == null || usv > worst.Value.RadiationUsv)
                    worst = new HotspotRow(place.id,
                        string.IsNullOrWhiteSpace(place.displayName) ? place.id : place.displayName,
                        place.sector, usv, band.Severity, band.Label);
            }

            if (safest != null && worst != null && safest.Value.LocationId == worst.Value.LocationId)
                safest = null;

            return new RouteDoseCheck(worst, safest, hotspots, total);
        }
    }
}
