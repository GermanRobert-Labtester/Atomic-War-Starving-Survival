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

        /// <summary>Tasks 5-8: optional custom disease resolver hook (defaults to ResolveDiseaseId/PreyDefinition.ResolveDiseaseId).</summary>
        public Func<PreyDefinition, string>? DiseaseResolver { get; set; }

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
        public const float FallbackContaminationDose = PreyDefinition.FallbackContaminationDose;

        /// <summary>Fallback disease ID for medium-risk prey without explicit mapping.</summary>
        public const string FallbackDiseaseId = PreyDefinition.FallbackDiseaseId;

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
                    var resolver = DiseaseResolver ?? ResolveDiseaseId;
                    string diseaseId = !string.IsNullOrEmpty(site.diseaseId)
                        ? site.diseaseId
                        : (System.RollDiseaseRisk(preyDef.diseaseRisk) ? resolver(preyDef) : string.Empty);

                    if (ApplyDisease != null && !string.IsNullOrEmpty(diseaseId))
                    {
                        ApplyDisease(survivor, diseaseId, day);
                    }

                    // Contamination application (deterministic from site state, with catalog fallback if unauthored)
                    float dose = site.contaminationDose > 0f
                        ? site.contaminationDose
                        : (System.RollContaminationRisk(preyDef.contaminationRisk)
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
        /// Workstream D: Shared repair bill calculation authority.
        /// Resolves the site, checks broken/durability, and calculates repair bill from trap definition.
        /// </summary>
        public bool TryGetRepairBill(string siteId, out InventoryBill bill, out string reason)
        {
            bill = new InventoryBill();
            if (Catalog == null)
            {
                reason = "trapping.no_catalog";
                return false;
            }

            var site = System.State.trapSites.Find(s => s.siteId == siteId);
            if (site == null)
            {
                reason = "trapping.no_trap";
                return false;
            }
            if (string.IsNullOrEmpty(site.trapId))
            {
                reason = "trapping.legacy_trap_unrepairable";
                return false;
            }
            if (!site.isBroken && site.remainingDurability > 0)
            {
                reason = "trapping.not_damaged";
                return false;
            }

            if (!Catalog.Traps.TryGetValue(site.trapId, out var trapDef))
            {
                reason = "trapping.unknown_trap";
                return false;
            }

            bill = trapDef.CalculateRepairBill();
            reason = string.Empty;
            return true;
        }

        /// <summary>
        /// Workstream D: Preflight check for repair affordability.
        /// </summary>
        public bool CanAffordRepair(string siteId, out InventoryBill bill, out string failureReason)
        {
            if (!TryGetRepairBill(siteId, out bill, out failureReason))
                return false;

            if (Inventory == null)
            {
                failureReason = "trapping.no_inventory";
                return false;
            }

            var quote = Inventory.Inventory.QuoteTransaction(bill);
            if (!quote.CanExecute)
            {
                failureReason = !string.IsNullOrEmpty(quote.Validation.FailureReason)
                    ? quote.Validation.FailureReason
                    : "trapping.insufficient_materials";
                return false;
            }

            failureReason = string.Empty;
            return true;
        }

        /// <summary>
        /// Plan 36 Closure II / Workstream D: Repair a broken trap with atomic material payment.
        /// Delegates to TryGetRepairBill for cost calculation and validation.
        /// </summary>
        public ActionResult TryRepairTrap(string siteId)
        {
            if (Inventory == null)
                return ActionResult.Blocked("no_inventory", "trapping.no_inventory");

            if (!TryGetRepairBill(siteId, out var bill, out var reason))
                return ActionResult.Blocked("repair_unavailable", reason);

            var site = System.State.trapSites.Find(s => s.siteId == siteId)!;
            var trapDef = Catalog!.Traps[site.trapId];

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
