using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;

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

        /// <summary>Plan 36: catalog for trap definitions. Set after construction.</summary>
        public WildlifeTrappingCatalog? Catalog { get; set; }

        /// <summary>Plan 36: inventory for material payment. Set after construction.</summary>
        public InventoryHostSession? Inventory { get; set; }

        /// <summary>Plan 36: delegate for applying disease. Set by host to route to DiseaseSystem.</summary>
        public Action<string, string, int>? ApplyDisease { get; set; }

        /// <summary>Plan 36: delegate for applying contamination dose. Set by host to route to RadiationSystem.</summary>
        public Action<string, float>? ApplyContamination { get; set; }

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
        /// Plan 36: Catalog-aware trap deployment with atomic material payment.
        /// Consumes the trap item from inventory, then deploys with catalog parameters.
        /// The trap item is crafted separately via recipes; deployment does not double-charge.
        /// </summary>
        public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId)
        {
            if (Catalog == null)
                return ActionResult.Blocked("no_catalog", "trapping.no_catalog");
            if (Inventory == null)
                return ActionResult.Blocked("no_inventory", "trapping.no_inventory");

            if (!Catalog.Traps.TryGetValue(trapId, out var trapDef))
                return ActionResult.Blocked("unknown_trap", "trapping.unknown_trap");

            // Consume the trap item from inventory (crafted via recipes)
            var bill = new InventoryBill();
            bill.AddCost(trapId, 1); // trap item ID matches trap definition ID

            using var tx = Inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                return ActionResult.Blocked("no_trap_item", "trapping.no_trap_item");
            }

            // Deploy trap with catalog parameters
            var setResult = System.SetTrap(siteId, baitType, hunterId, trapDef.trapType,
                trapId, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            if (!setResult.IsSuccess)
            {
                tx.Cancel();
                return setResult;
            }

            tx.TryCommit();
            LastEvent = $"Set {trapDef.displayName} at {siteId} (Hunter: {hunterId})";
            RaiseStateChanged();
            return setResult;
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

        /// <summary>Fallback contamination dose when prey has positive risk but no explicit dose.</summary>
        private const float FallbackContaminationDose = 2f;

        /// <summary>Fallback disease ID for medium-risk prey without explicit mapping.</summary>
        private const string FallbackDiseaseId = "disease_zoonotic_flu";

        public ActionResult Butcher(string siteId, string butcherId = "")
        {
            var res = System.Butcher(siteId, butcherId);
            if (res.IsSuccess)
            {
                // Plan 36 Closure II / Tasks 5-8: apply disease/contamination from site state
                var site = System.State.trapSites.Find(s => s.siteId == siteId);
                if (site != null && Catalog != null && Catalog.Prey.TryGetValue(site.catchSpecies, out var preyDef))
                {
                    string survivor = string.IsNullOrEmpty(butcherId) ? "unknown" : butcherId;
                    int day = _currentDay > 0 ? _currentDay : site.setDay;

                    // Disease application (deterministic from site state, with catalog fallback if unauthored)
                    string diseaseId = !string.IsNullOrEmpty(site.diseaseId)
                        ? site.diseaseId
                        : (site.contaminationDose <= 0f && System.RollDiseaseRisk(preyDef.diseaseRisk) ? ResolveDiseaseId(preyDef) : string.Empty);

                    if (ApplyDisease != null && !string.IsNullOrEmpty(diseaseId))
                    {
                        ApplyDisease(survivor, diseaseId, day);
                    }

                    // Contamination application (deterministic from site state, with catalog fallback if unauthored)
                    float dose = site.contaminationDose > 0f
                        ? site.contaminationDose
                        : (string.IsNullOrEmpty(site.diseaseId) && System.RollContaminationRisk(preyDef.contaminationRisk)
                            ? (preyDef.contaminationDose > 0f ? preyDef.contaminationDose : FallbackContaminationDose)
                            : 0f);

                    if (ApplyContamination != null && dose > 0f)
                    {
                        ApplyContamination(survivor, dose);
                    }
                }

                LastEvent = string.IsNullOrEmpty(butcherId)
                    ? $"Butchered game catch at site {siteId}"
                    : $"Butchered game catch at site {siteId} (butcher: {butcherId})";
                RaiseStateChanged();
            }
            return res;
        }

        /// <summary>
        /// Resolve disease ID: explicit per-species mapping wins, otherwise tier fallback.
        /// Low risk (≤0.1) → no disease; medium/high → fallback wildlife disease.
        /// </summary>
        public static string ResolveDiseaseId(PreyDefinition prey) => PreyDefinition.ResolveDiseaseId(prey);

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

        /// <summary>
        /// Plan 36 Closure II: Repair a broken trap with atomic material payment.
        /// Repair cost = ceil(setup cost × 0.5) per item, aggregated by ID.
        /// </summary>
        public ActionResult TryRepairTrap(string siteId)
        {
            if (Catalog == null)
                return ActionResult.Blocked("no_catalog", "trapping.no_catalog");
            if (Inventory == null)
                return ActionResult.Blocked("no_inventory", "trapping.no_inventory");

            var site = System.State.trapSites.Find(s => s.siteId == siteId);
            if (site == null)
                return ActionResult.Blocked("no_trap", "trapping.no_trap");
            if (string.IsNullOrEmpty(site.trapId))
                return ActionResult.Blocked("legacy_trap", "trapping.legacy_trap_unrepairable");
            if (!site.isBroken && site.remainingDurability > 0)
                return ActionResult.Blocked("not_damaged", "trapping.not_damaged");

            if (!Catalog.Traps.TryGetValue(site.trapId, out var trapDef))
                return ActionResult.Blocked("unknown_trap", "trapping.unknown_trap");

            // Build repair bill: ceil(setup cost × 0.5) per item
            var bill = new InventoryBill();
            var aggregated = new Dictionary<string, int>(StringComparer.Ordinal);
            foreach (var cost in trapDef.setupCosts)
            {
                if (string.IsNullOrEmpty(cost.itemId) || cost.amount <= 0) continue;
                aggregated.TryGetValue(cost.itemId, out int existing);
                aggregated[cost.itemId] = existing + cost.amount;
            }
            foreach (var kv in aggregated)
            {
                int repairQty = (int)Math.Ceiling(kv.Value * 0.5);
                if (repairQty > 0)
                    bill.AddCost(kv.Key, repairQty);
            }

            // Execute atomic transaction
            using var tx = Inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
                return ActionResult.Blocked("insufficient_materials", "trapping.insufficient_materials");

            // Repair the trap
            var repairResult = System.RepairTrap(siteId, trapDef.durabilityChecks);
            if (!repairResult.IsSuccess)
            {
                tx.Cancel();
                return repairResult;
            }

            tx.TryCommit();
            LastEvent = $"Repaired {trapDef.displayName} at {siteId}";
            RaiseStateChanged();
            return repairResult;
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            System.TickDay(day);
            int caughtDelta = System.State.totalCatch - _lastSeenCatchTotal;
            _lastSeenCatchTotal = System.State.totalCatch;
            if (caughtDelta > 0) OnCatchPressure?.Invoke(caughtDelta);
            RaiseStateChanged();
        }

        /// <summary>
        /// Plan 28 Phase 3 (overhunt): raised after the daily auto-check when
        /// snares produced catches. The host forwards the count into the
        /// migration system's <c>ApplyHarvestPressure</c> — heavy exploitation
        /// of a migration window thins the local packs, feeding the existing
        /// density and scarcity consumers. Bounded, reversible (the existing
        /// birth recovery), no hidden tracking system.
        /// </summary>
        public event Action<int>? OnCatchPressure;
        private int _lastSeenCatchTotal;
        private int _currentDay;

        public override void Save()
        {
            if (!IsDirty) return;
            WildlifeTrappingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }
    }
}
