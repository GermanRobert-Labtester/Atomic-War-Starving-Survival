using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
#pragma warning disable CS8618

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
        public int reinforcedBeams; // Plans 90-93: cast beams set through the foundry loop
        public List<string> discoveredCaches = new List<string>();
    }

    public sealed class ExcavationSystem
    {
        public const string SystemId = "excavation";
        // Plans 90-93: the canonical structural beam is the foundry's cast
        // T-beam; reinforcement is an item-flow consumer of the foundry loop.
        public const string StructuralBeamItemId = "item_foundry_t_beam";
        public const int StructuralBeamCost = 2;
        private ExcavationState _state = new ExcavationState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private readonly Inventory.Inventory? _inventory;

        public ExcavationState State => _state;
        public event Action OnExcavationChanged;

        public ExcavationSystem(ISeededRng rng, ILog? log = null, Inventory.Inventory? inventory = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
            _inventory = inventory;
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

        /// <summary>
        /// Plans 90-93 — set cast structural beams (foundry output) into the
        /// working face. Atomic inventory billing: a failed reinforcement
        /// consumes nothing and mutates nothing. Each set halves structural
        /// risk again, with diminishing returns at low risk.
        /// </summary>
        public ActionResult TryApplyStructuralReinforcement(string siteId)
        {
            var site = _state.sites.Find(s => s.siteId == siteId);
            if (site == null) return ActionResult.Failed("unknown_site", "excavation.unknown_site");
            if (site.isComplete) return ActionResult.Blocked("already_complete", "excavation.already_complete");
            if (site.hasCavedIn) return ActionResult.Blocked("caved_in", "excavation.caved_in");
            if (site.structuralRisk <= 0.05f) return ActionResult.Blocked("risk_already_low", "excavation.risk_already_low");
            if (_inventory == null) return ActionResult.Failed("no_inventory", "excavation.no_inventory");

            var bill = new InventoryBill();
            bill.AddCost(StructuralBeamItemId, StructuralBeamCost);

            bool committed = _inventory.TryExecuteTransaction(bill, () =>
            {
                site.reinforcedBeams++;
                site.structuralRisk = Math.Max(0.05f, site.structuralRisk * 0.5f);
                OnExcavationChanged?.Invoke();
            });
            if (!committed) return ActionResult.Failed("missing_beams", "excavation.missing_beams");

            return ActionResult.Success("excavation.reinforced",
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

        public ExcavationState CaptureState() => CloneState(_state);

        public void RestoreState(ExcavationState saved)
        {
            if (saved == null) return;
            _state = CloneState(saved);
        }

        private static ExcavationState CloneState(ExcavationState src)
        {
            if (src == null) return new ExcavationState();
            var s = new SystemTextJsonSerializer();
            var json = s.Serialize(src);
            return s.Deserialize<ExcavationState>(json) ?? new ExcavationState();
        }
    }
}
