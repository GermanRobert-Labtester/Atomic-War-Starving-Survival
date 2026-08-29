using System;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for WildlifeTrappingSystem.
    /// Manages perimeter snare lines, bait consumption, game butchery, toxin removal, and food reserves.
    /// </summary>
    public sealed class WildlifeTrappingHostSession
    : HostSessionBase{
        public WildlifeTrappingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;
        public WildlifeTrappingHostSession(WildlifeTrappingSystem system)
        {
            System = system ?? new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());

            System.OnTrappingChanged += () =>
            {
                RaiseStateChanged();
            };
        }

        public ActionResult SetTrap(string siteId, string baitType, string hunterId)
        {
            var res = System.SetTrap(siteId, baitType, hunterId);
            if (res.IsSuccess)
            {
                LastEvent = $"Set {baitType} snare at {siteId} (Hunter: {hunterId})";
                RaiseStateChanged();
            }
            return res;
        }

        /// <summary>
        /// Live wildlife pressure for the trapped sector (1.0 = authored rate),
        /// refreshed daily by the evolving-world day owner from the migration
        /// system's sector density.
        /// </summary>
        public float WildlifeDensityMultiplier { get; set; } = 1f;

        public ActionResult CheckTraps(float? densityMultiplier = null)
        {
            var res = System.CheckTraps(densityMultiplier ?? WildlifeDensityMultiplier);
            if (res.IsSuccess)
            {
                LastEvent = (densityMultiplier ?? WildlifeDensityMultiplier) == 1f
                    ? "Inspected all perimeter snares."
                    : $"Inspected all perimeter snares (wildlife pressure x{densityMultiplier:0.00}).";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult Butcher(string siteId, string butcherId = "")
        {
            var res = System.Butcher(siteId, butcherId);
            if (res.IsSuccess)
            {
                LastEvent = string.IsNullOrEmpty(butcherId)
                    ? $"Butchered game catch at site {siteId}"
                    : $"Butchered game catch at site {siteId} (butcher: {butcherId})";
                RaiseStateChanged();
            }
            return res;
        }

        public ActionResult RemoveToxin(string siteId)
        {
            var res = System.RemoveToxin(siteId);
            if (res.IsSuccess)
            {
                LastEvent = $"Purged radiation glands and toxins from catch at {siteId}";
                RaiseStateChanged();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            RaiseStateChanged();
        }

        public override void Save()
        {
            if (!IsDirty) return;
            WildlifeTrappingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
