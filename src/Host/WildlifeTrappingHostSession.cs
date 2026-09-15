// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.World;

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

        // ── Plan IV: destination-authority adapters ──
        // Trapping emits domain facts; these adapters hand each pending fact
        // to its destination authority. An adapter returns true ONLY after
        // the destination accepted the fact; then the outbox marks it
        // delivered. Rejected/unknown destinations leave the fact pending.

        /// <summary>(questId, speciesId, survivorId) → accepted. The moral authority acks at resolution.</summary>
        public Func<string, string, string, bool>? DeliverMoralConsequence { get; set; }

        /// <summary>(encounterId, siteId, day) → accepted by the encounter authority.</summary>
        public Func<string, string, int, bool>? DeliverTrapEncounter { get; set; }

        /// <summary>(message) → accepted by the radio authority.</summary>
        public Func<string, bool>? DeliverTrappingBroadcast { get; set; }

        /// <summary>(eventId, siteId, day, sourceId) → accepted by the canonical narrative authority.</summary>
        public Func<string, string, int, string, bool>? DeliverNarrativeIncident { get; set; }

        /// <summary>Optional host-facing bycatch projection for narrative/telemetry adapters.</summary>
        public event Action<BycatchOccurredEvent>? OnBycatchOccurred;

        /// <summary>Optional food sink. The default host path adds raw meat to Inventory.</summary>
        public Func<int, bool>? DeliverButcheryFood { get; set; }

        /// <summary>Cross-system morale adapter. The host resolves authored
        /// prey data; the callback routes the mutation to the canonical survivor
        /// authority.</summary>
        public Action<string, float, string>? ApplyMorale { get; set; }
        private readonly HashSet<string> _moraleAppliedButcheryIds = new(StringComparer.Ordinal);

        /// <summary>Tasks 5-8: optional custom disease resolver hook (defaults to ResolveDiseaseId/PreyDefinition.ResolveDiseaseId).</summary>
        public Func<PreyDefinition, string>? DiseaseResolver { get; set; }
        public event Action<string>? OnTrapCrafted;

        private WastelandMapSystem? _map;
        public WastelandMapSystem? Map
        {
            get => _map;
            set
            {
                _map = value;
                ReconcileMapMarkers();
            }
        }

        public WildlifeTrappingHostSession(WildlifeTrappingSystem system)
        {
            System = system ?? new WildlifeTrappingSystem(new SeededRng(1986), new GodotLog());

            System.OnTrappingChanged += () =>
            {
                RaiseStateChanged();
            };
            System.OnTrapDeployed += SyncTrapMarker;
            System.OnTrapBroken += SyncTrapMarker;
            System.OnTrapRepaired += SyncTrapMarker;
            System.OnTrapRemoved += e => _map?.RemoveTrapMarker(e.siteId);
            System.OnBycatchResolved += HandleBycatchResolved;
        }

        private void HandleBycatchResolved(BycatchOccurredEvent occurrence)
        {
            if (occurrence == null) return;
            LastEvent = $"Bycatch at {occurrence.siteId}: primary={occurrence.primarySpeciesId}, secondary={occurrence.bycatchSpeciesId}";
            OnBycatchOccurred?.Invoke(occurrence);
            RaiseStateChanged();
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

        /// <summary>Shared site replaceability query for host/UI preflight.</summary>
        public bool CanSetTrapAtSite(string siteId, out string failureCode)
            => System.CanSetTrapAtSite(siteId, out failureCode);

        /// <summary>Forwards the live ecology snapshot into Core's selector.</summary>
        public void SetSelectionContext(WildlifeSelectionContext context)
            => System.SetSelectionContext(context);

        /// <summary>
        /// Plan 36 / Flagship Trapping Task 1: Catalog-aware trap deployment with atomic material payment.
        /// Enforces TrapDefinition.setupCosts (or crafted trap item if held) atomically.
        /// Preflight validates site active state to prevent accidental charges.
        /// </summary>
        public ActionResult TrySetTrap(string siteId, string trapId, string baitType, string hunterId)
        {
            if (Catalog == null)
                return ActionResult.Blocked("no_catalog", "trapping.no_catalog");
            if (Inventory == null)
                return ActionResult.Blocked("no_inventory", "trapping.no_inventory");

            if (!Catalog.Traps.TryGetValue(trapId, out var trapDef))
                return ActionResult.Blocked("unknown_trap", "trapping.unknown_trap");

            // Preflight check: active trap cannot be replaced while active and operational
            if (!CanSetTrapAtSite(siteId, out string failureCode))
                return ActionResult.Blocked(failureCode, "trapping." + failureCode);

            // Determine billing: prefer finished trap item if held; otherwise consume setup materials
            var bill = new InventoryBill();
            bool consumedFinishedTrap = Inventory.Inventory.CountById(trapId) >= 1;
            if (consumedFinishedTrap)
            {
                bill.AddCost(trapId, 1);
            }
            else
            {
                bill = trapDef.CalculateSetupBill();
            }

            using var tx = Inventory.Inventory.BeginTransaction(bill);
            if (!tx.Validation.IsValid)
            {
                return ActionResult.Blocked("insufficient_materials", "trapping.insufficient_materials");
            }

            // Deploy trap with catalog parameters
            var setResult = System.SetTrap(siteId, baitType, hunterId, trapDef.trapType,
                trapId, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            if (!setResult.IsSuccess)
            {
                tx.Cancel();
                return setResult;
            }

            if (!tx.TryCommit())
            {
                // Domain already mutated; materials must not silently vanish or
                // report success when the inventory commit failed.
                System.RemoveTrap(siteId);
                return ActionResult.Blocked("commit_failed", "trapping.commit_failed");
            }
            if (!consumedFinishedTrap)
                OnTrapCrafted?.Invoke(trapId);
            LastEvent = $"Set {trapDef.displayName} at {siteId} (Hunter: {hunterId})";
            RaiseStateChanged();
            return setResult;
        }

        /// <summary>
        /// Task 1: Query setup bill for a trap definition. Prefers finished trap item if held,
        /// else evaluates authored setupCosts.
        /// </summary>
        public bool TryGetSetupBill(string trapId, out InventoryBill bill, out string reason)
        {
            bill = new InventoryBill();
            if (Catalog == null)
            {
                reason = "trapping.no_catalog";
                return false;
            }
            if (!Catalog.Traps.TryGetValue(trapId, out var trapDef))
            {
                reason = "trapping.unknown_trap";
                return false;
            }
            if (Inventory != null && Inventory.Inventory.CountById(trapId) >= 1)
            {
                bill.AddCost(trapId, 1);
            }
            else
            {
                bill = trapDef.CalculateSetupBill();
            }
            reason = string.Empty;
            return true;
        }

        /// <summary>
        /// Task 1: Preflight check for trap deployment affordability.
        /// </summary>
        public bool CanAffordSetup(string trapId, out InventoryBill bill, out string failureReason)
        {
            if (!TryGetSetupBill(trapId, out bill, out failureReason))
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
            DeliverPendingEvents();
            return res;
        }

        // ── Plan IV: shared event-delivery discipline ──

        /// <summary>
        /// Deliver every pending external fact to its destination authority in
        /// persisted sequence order. Delivery failure (destination missing,
        /// unknown content, rejection) leaves the fact pending — nothing is
        /// silently dropped. Safe to call repeatedly; delivered facts are
        /// skipped. Called after each state-mutating entry point and after
        /// composition completes so restored pending facts recover.
        /// </summary>
        public void DeliverPendingEvents()
        {
            var pending = System.GetPendingEvents();
            for (int i = 0; i < pending.Count; i++)
            {
                var ev = pending[i];
                bool accepted = ev.kind switch
                {
                    WildlifeTrappingEventKinds.MoralConsequence
                        => DeliverMoralConsequence?.Invoke(ev.payloadId, ev.speciesId, ev.survivorId) ?? false,
                    WildlifeTrappingEventKinds.TrapEncounter
                        => DeliverTrapEncounter?.Invoke(ev.payloadId, ev.sourceTrapSiteId, ev.day) ?? false,
                    WildlifeTrappingEventKinds.TrappingBroadcast
                        => DeliverTrappingBroadcast?.Invoke(ComposeBroadcastMessage(ev)) ?? false,
                    WildlifeTrappingEventKinds.NarrativeIncident
                        => DeliverNarrativeIncident?.Invoke(
                            ev.payloadId,
                            ev.sourceTrapSiteId,
                            ev.day,
                            WildlifeTrappingSystem.BuildNarrativeIncidentSourceId(ev.sourceTrapSiteId, ev.payloadId)) ?? false,
                    _ => false
                };
                if (accepted)
                    System.MarkEventDelivered(ev.eventId);
            }
        }

        /// <summary>
        /// Player-readable radio text for one trapping broadcast fact.
        /// Species render through their display names — never raw IDs.
        /// </summary>
        public string ComposeBroadcastMessage(WildlifeTrappingPendingEvent ev)
        {
            string speciesName = ResolveSpeciesDisplayName(ev.speciesId);
            return ev.payloadId switch
            {
                TrappingBroadcastIds.FirstCatch
                    => $"Wildlife net, day {ev.day}: first {speciesName} taken at the line.",
                TrappingBroadcastIds.TrapBroken
                    => $"Wildlife net, day {ev.day}: a trap came up wrecked at {ev.sourceTrapSiteId} — sprung and torn.",
                TrappingBroadcastIds.RareBycatch
                    => $"Wildlife net, day {ev.day}: odd bycatch at the line today — a {speciesName}, of all things.",
                _ => $"Wildlife net, day {ev.day}: report from the trap line."
            };
        }

        private string ResolveSpeciesDisplayName(string speciesId)
        {
            if (string.IsNullOrEmpty(speciesId)) return "animal";
            var quarry = System.GetQuarryCatalog();
            if (quarry.TryGetValue(speciesId, out var q) && !string.IsNullOrEmpty(q.displayName))
                return q.displayName;
            return speciesId.Replace('_', ' ');
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
                if (site != null && !string.IsNullOrEmpty(butcherId) && Catalog != null && Catalog.Prey.TryGetValue(site.catchSpecies, out var preyDef))
                {
                    int day = _currentDay > 0 ? _currentDay : site.setDay;
                    string butcheryId = $"butchery:{site.siteId}:{site.deploymentSequence}:{site.catchSpecies}:{butcherId}";
                    if (_moraleAppliedButcheryIds.Add(butcheryId)
                        && float.IsFinite(preyDef.moraleEffect)
                        && preyDef.moraleEffect != 0f)
                    {
                        ApplyMorale?.Invoke(butcherId, preyDef.moraleEffect,
                            $"wildlife_butchery:{site.catchSpecies}");
                    }

                    // Disease application (deterministic from site state, with catalog fallback if unauthored)
                    var resolver = DiseaseResolver ?? ResolveDiseaseId;
                    string diseaseId = !string.IsNullOrEmpty(site.diseaseId)
                        ? site.diseaseId
                        : (System.RollDiseaseRisk(preyDef.diseaseRisk) ? resolver(preyDef) : string.Empty);

                    if (ApplyDisease != null && !string.IsNullOrEmpty(diseaseId))
                    {
                        ApplyDisease(butcherId, diseaseId, day);
                    }

                    // Contamination application (deterministic from site state, with catalog fallback if unauthored)
                    float dose = site.contaminationDose > 0f
                        ? site.contaminationDose
                        : (System.RollContaminationRisk(preyDef.contaminationRisk)
                            ? (preyDef.contaminationDose > 0f ? preyDef.contaminationDose : FallbackContaminationDose)
                            : 0f);

                    if (ApplyContamination != null && dose > 0f)
                    {
                        ApplyContamination(butcherId, dose);
                    }

                    // Plan VI: the secondary carcass has its own persisted
                    // disease and contamination results. It never reuses the
                    // primary definition or contributes a second morale delta.
                    if (!string.IsNullOrEmpty(site.bycatchSpecies)
                        && Catalog.Prey.ContainsKey(site.bycatchSpecies))
                    {
                        string bycatchDiseaseId = site.bycatchDiseaseId;
                        if (ApplyDisease != null && !string.IsNullOrEmpty(bycatchDiseaseId))
                            ApplyDisease(butcherId, bycatchDiseaseId, day);

                        if (ApplyContamination != null && site.bycatchContaminationDose > 0f)
                            ApplyContamination(butcherId, site.bycatchContaminationDose);
                    }

                }

                // The Core transaction is committed once. Food output is a
                // separate inventory projection and therefore happens only
                // on that successful transition, never on a repeated button
                // press or restore.
                var foodSite = System.State.trapSites.Find(s => s.siteId == siteId);
                if (foodSite != null)
                {
                    int foodUnits = Math.Max(0, (int)Math.Round(
                        foodSite.carcassYield + foodSite.bycatchYield,
                        MidpointRounding.AwayFromZero));
                    if (foodUnits > 0)
                    {
                        bool accepted = DeliverButcheryFood?.Invoke(foodUnits)
                            ?? (Inventory != null && Inventory.TryAdd("raw_meat", foodUnits));
                        if (!accepted)
                            GD.PushWarning($"[WildlifeTrapping] Butchery food output of {foodUnits} units was not accepted by inventory.");
                    }
                }

                LastEvent = string.IsNullOrEmpty(butcherId)
                    ? $"Butchered game catch at site {siteId}"
                    : $"Butchered game catch at site {siteId} (butcher: {butcherId})";
                RaiseStateChanged();
            }
            DeliverPendingEvents();
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
        /// Preserve hide after butchery. Core marks the site once; inventory
        /// projection is best-effort (same pattern as <see cref="Butcher"/>).
        /// </summary>
        public ActionResult PreserveHide(string siteId)
        {
            var res = System.PreserveHide(siteId, out string hideItemId, out float hideQuantity);
            if (!res.IsSuccess)
                return res;

            int qty = Math.Max(0, (int)Math.Round(hideQuantity, MidpointRounding.AwayFromZero));
            if (!string.IsNullOrEmpty(hideItemId) && qty > 0)
            {
                bool accepted = Inventory != null
                    && (Inventory.TryAdd(hideItemId, qty)
                        || Inventory.Inventory.TryProduce(hideItemId, qty));
                if (!accepted)
                    GD.PushWarning(
                        $"[WildlifeTrapping] Hide output {hideItemId} x{qty} was not accepted by inventory.");
            }

            LastEvent = string.IsNullOrEmpty(hideItemId) || qty <= 0
                ? $"No usable hide at {siteId}"
                : $"Preserved hide at {siteId}: {hideItemId} x{qty}";
            RaiseStateChanged();
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

            ActionResult repairResult = ActionResult.Blocked("repair_unavailable", "trapping.repair_unavailable");
            try
            {
                if (!tx.TryCommit(() =>
                    {
                        repairResult = System.RepairTrap(siteId, trapDef.durabilityChecks);
                        if (!repairResult.IsSuccess)
                            throw new InvalidOperationException(repairResult.FailureCode ?? "trapping.repair_failed");
                    }))
                {
                    return ActionResult.Blocked("commit_failed", "trapping.commit_failed");
                }
            }
            catch (InvalidOperationException)
            {
                return repairResult;
            }
            LastEvent = $"Repaired {trapDef.displayName} at {siteId}";
            RaiseStateChanged();
            return repairResult;
        }

        public ActionResult RemoveTrap(string siteId)
        {
            var result = System.RemoveTrap(siteId);
            if (result.IsSuccess)
            {
                LastEvent = $"Removed trap at {siteId}";
                RaiseStateChanged();
            }
            return result;
        }

        /// <summary>Rebuilds the canonical map projection from restored trap
        /// state. The map owns markers; trap state owns only lifecycle facts.</summary>
        public void ReconcileMapMarkers()
        {
            if (_map == null) return;
            var sources = new List<TrapMapMarkerSource>();
            if (System.State.trapSites != null)
            {
                foreach (var site in System.State.trapSites)
                {
                    if (site == null || string.IsNullOrEmpty(site.siteId)) continue;
                    if (!_map.TryResolveTrapSitePosition(site.siteId, out float x, out float y))
                    {
                        GD.PushWarning($"[WildlifeTrapping] No canonical map coordinate for trap site '{site.siteId}'.");
                        continue;
                    }
                    sources.Add(new TrapMapMarkerSource
                    {
                        SiteId = site.siteId,
                        TrapId = site.trapId,
                        TrapType = site.trapType,
                        PositionX = x,
                        PositionY = y,
                        IsBroken = site.isBroken
                    });
                }
            }
            _map.ReconcileTrapMarkers(sources);
        }

        private void SyncTrapMarker(TrapLifecycleEvent lifecycle)
        {
            if (_map == null || lifecycle == null) return;
            _map.EnsureTrapMarker(lifecycle.siteId, lifecycle.trapId, lifecycle.trapType, lifecycle.isBroken);
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            System.TickDay(day);
            int caughtDelta = System.State.totalCatch - _lastSeenCatchTotal;
            _lastSeenCatchTotal = System.State.totalCatch;
            if (caughtDelta > 0) OnCatchPressure?.Invoke(caughtDelta);
            RaiseStateChanged();
            DeliverPendingEvents();
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
