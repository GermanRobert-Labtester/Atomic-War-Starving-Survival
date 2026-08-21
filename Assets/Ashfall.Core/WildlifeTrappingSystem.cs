using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    [Serializable]
    public sealed class WildlifeTrappingState
    {
        public string systemId = WildlifeTrappingSystem.SystemId;
        public List<TrapSite> trapSites = new List<TrapSite>();
        public int totalCatch;
        public int totalToxicRemoved;
    }

    [Serializable]
    public sealed class TrapSite
    {
        public string siteId = string.Empty;
        public string assignedHunterId = string.Empty;
        public string baitType = string.Empty;
        public int setDay = -1;
        public int checkDay = -1;
        public int checkIntervalDays = 2;
        public bool hasCatch;
        public string catchSpecies = string.Empty;
        public float carcassYield;
        public bool isToxic;
        public bool toxinRemoved;
        public bool isMeatProcessed;
    }

    public sealed class WildlifeTrappingSystem
    {
        public const string SystemId = "wildlife_trapping";
        private WildlifeTrappingState _state = new WildlifeTrappingState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public WildlifeTrappingState State => _state;
        public event Action OnTrappingChanged;

        public WildlifeTrappingSystem(ISeededRng rng, ILog log = null!)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult SetTrap(string siteId, string baitType, string hunterId)
        {
            var existing = _state.trapSites.Find(s => s.siteId == siteId);
            if (existing != null)
            {
                if (!existing.hasCatch && existing.setDay > 0)
                    return ActionResult.Blocked("trap_active", "trapping.trap_active");
                existing.setDay = _currentDay;
                existing.checkDay = _currentDay + existing.checkIntervalDays;
                existing.baitType = baitType;
                existing.assignedHunterId = hunterId ?? string.Empty;
                existing.hasCatch = false;
            }
            else
            {
                _state.trapSites.Add(new TrapSite
                {
                    siteId = siteId, baitType = baitType,
                    assignedHunterId = hunterId ?? string.Empty,
                    setDay = _currentDay, checkDay = _currentDay + 2
                });
            }
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.trap_set");
        }

        public ActionResult CheckTraps()
        {
            int caught = 0;
            foreach (var site in _state.trapSites)
            {
                if (site.hasCatch || site.setDay <= 0) continue;
                if (_currentDay < site.checkDay) continue;

                if (_rng.NextDouble() < 0.5f) // 50% catch rate
                {
                    site.hasCatch = true;
                    site.catchSpecies = _rng.NextDouble() < 0.3f ? "rabbit" : "rat";
                    site.carcassYield = 1f + (float)_rng.NextDouble() * 2f;
                    site.isToxic = _rng.NextDouble() < 0.2f; // 20% toxic
                    site.toxinRemoved = false;
                    site.isMeatProcessed = false;
                    caught++;
                    _state.totalCatch++;
                }
            }
            OnTrappingChanged?.Invoke();
            return caught > 0
                ? ActionResult.Success("trapping.catch_found", new Dictionary<string, double> { { "caught", caught } })
                : ActionResult.Success("trapping.no_catch");
        }

        public ActionResult Butcher(string siteId)
        {
            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");
            if (site.isMeatProcessed)
                return ActionResult.Blocked("already_butchered", "trapping.already_butchered");

            site.isMeatProcessed = true;
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.butchered",
                new Dictionary<string, double> { { "yield", site.carcassYield }, { "toxic", site.isToxic ? 1 : 0 } });
        }

        public ActionResult RemoveToxin(string siteId)
        {
            var site = _state.trapSites.Find(s => s.siteId == siteId);
            if (site == null || !site.hasCatch)
                return ActionResult.Blocked("no_catch", "trapping.no_catch");
            if (!site.isToxic)
                return ActionResult.Blocked("not_toxic", "trapping.not_toxic");
            if (site.toxinRemoved)
                return ActionResult.Blocked("already_clean", "trapping.already_clean");

            site.toxinRemoved = true;
            _state.totalToxicRemoved++;
            OnTrappingChanged?.Invoke();
            return ActionResult.Success("trapping.toxin_removed");
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            CheckTraps();
        }

        public WildlifeTrappingState CaptureState() => _state;
        public void RestoreState(WildlifeTrappingState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnTrappingChanged?.Invoke();
        }
    }
}
