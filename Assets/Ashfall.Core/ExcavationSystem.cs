using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class ExcavationState
    {
        public string systemId = ExcavationSystem.SystemId;
        public List<ExcavationSite> sites = new List<ExcavationSite>();
    }

    [Serializable]
    public sealed class ExcavationSite
    {
        public string siteId = string.Empty;
        public string roomBlueprintId = string.Empty;
        public float progress;
        public float requiredProgress = 100f;
        public int assignedWorkerCount;
        public float structuralRisk; // 0-1, risk of cave-in
        public bool hasCavedIn;
        public bool isComplete;
        public List<string> requiredTools = new List<string>();
        public bool shoringApplied;
        public List<string> discoveredCaches = new List<string>();
    }

    public sealed class ExcavationSystem
    {
        public const string SystemId = "excavation";
        private ExcavationState _state = new ExcavationState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;

        public ExcavationState State => _state;
        public event Action OnExcavationChanged;

        public ExcavationSystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public ActionResult AddSite(string siteId, string roomBlueprintId, float requiredProgress, float risk)
        {
            if (_state.sites.Exists(s => s.siteId == siteId))
                return ActionResult.Blocked("site_exists", "excavation.site_exists");
            _state.sites.Add(new ExcavationSite
            {
                siteId = siteId, roomBlueprintId = roomBlueprintId,
                requiredProgress = requiredProgress, structuralRisk = risk
            });
            OnExcavationChanged?.Invoke();
            return ActionResult.Success("excavation.site_added");
        }

        public ActionResult AssignWorkers(string siteId, int count)
        {
            var site = _state.sites.Find(s => s.siteId == siteId);
            if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
            if (site.isComplete) return ActionResult.Blocked("already_complete", "excavation.already_complete");
            if (site.hasCavedIn) return ActionResult.Blocked("caved_in", "excavation.caved_in");
            site.assignedWorkerCount = Math.Max(0, count);
            OnExcavationChanged?.Invoke();
            return ActionResult.Success("excavation.workers_assigned",
                new Dictionary<string, double> { { "workers", count } });
        }

        public ActionResult ApplyShoring(string siteId)
        {
            var site = _state.sites.Find(s => s.siteId == siteId);
            if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
            if (site.shoringApplied) return ActionResult.Blocked("already_shored", "excavation.already_shored");
            site.shoringApplied = true;
            site.structuralRisk *= 0.5f;
            OnExcavationChanged?.Invoke();
            return ActionResult.Success("excavation.shoring_applied",
                new Dictionary<string, double> { { "risk", site.structuralRisk } });
        }

        public void TickDay()
        {
            foreach (var site in _state.sites)
            {
                if (site.isComplete || site.hasCavedIn || site.assignedWorkerCount <= 0) continue;

                float dailyProgress = site.assignedWorkerCount * 5f;
                if (site.shoringApplied) dailyProgress *= 1.2f;
                site.progress += dailyProgress;

                if (_rng.NextDouble() < site.structuralRisk * 0.1f)
                {
                    site.hasCavedIn = true;
                    site.progress = Math.Max(0, site.progress - 20f);
                    _log.Warn($"[Excavation] cave-in at {site.siteId}!");
                }

                if (site.progress >= site.requiredProgress)
                {
                    site.isComplete = true;
                    _log.Info($"[Excavation] completed {site.siteId} -> {site.roomBlueprintId}");
                }
            }
            OnExcavationChanged?.Invoke();
        }

        public ExcavationState CaptureState() => _state;
        public void RestoreState(ExcavationState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnExcavationChanged?.Invoke();
        }
    }
}
