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
    {
        public WildlifeTrappingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public event Action? StateChanged;

        public WildlifeTrappingHostSession(WildlifeTrappingSystem system)
        {
            System = system ?? new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());

            System.OnTrappingChanged += () =>
            {
                StateChanged?.Invoke();
            };
        }

        public ActionResult SetTrap(string siteId, string baitType, string hunterId)
        {
            var res = System.SetTrap(siteId, baitType, hunterId);
            if (res.IsSuccess)
            {
                LastEvent = $"Set {baitType} snare at {siteId} (Hunter: {hunterId})";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult CheckTraps()
        {
            var res = System.CheckTraps();
            if (res.IsSuccess)
            {
                LastEvent = "Inspected all perimeter snares.";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult Butcher(string siteId)
        {
            var res = System.Butcher(siteId);
            if (res.IsSuccess)
            {
                LastEvent = $"Butchered game catch at site {siteId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public ActionResult RemoveToxin(string siteId)
        {
            var res = System.RemoveToxin(siteId);
            if (res.IsSuccess)
            {
                LastEvent = $"Purged radiation glands and toxins from catch at {siteId}";
                StateChanged?.Invoke();
            }
            return res;
        }

        public void TickDay(int day)
        {
            System.TickDay(day);
            StateChanged?.Invoke();
        }
    }
}
