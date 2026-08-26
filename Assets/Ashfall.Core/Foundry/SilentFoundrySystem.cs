using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Foundry
{
    // ---------------------------------------------------------------------

    /// <summary>
    /// THE SILENT FOUNDRY (Expansion 10) — smelter-bay production, repair and
    /// maintenance system. Owns the mutable simulation state; the static
    /// production catalog and the blueprint remain authored catalogs and are
    /// never mutated here.
    ///
    /// Identity is material continuity, not a weapon factory: agricultural
    /// tools, structural beams, railway spikes and wheels, acid-resistant
    /// pipes, repair plates, brackets, water components, heavy tools and
    /// heavy-alloy parts. Every heat has a source (charge), a rule (catalog
    /// product), labour/time/fuel/water costs, a quality outcome, inventory,
    /// trade, treaty and persistence consequences.
    ///
    /// Determinism: all randomness flows through the injected ISeededRng
    /// (default xorshift64*). Same seed ⇒ same outcomes. No System.Random.
    /// </summary>
    public sealed partial class SilentFoundrySystem
    {
        public const int DefaultSeed = 1009;
        public const int MaxWorkers = 8;            // room_bp_11 max_dweller_capacity
        public const float BlueprintBasePowerKw = 45f;
        public const float BlueprintWaterFlowLpm = 40f;

        // Event-id strings for bus forwarding (typed events are the primary surface).
        public const string EventUnlocked = "silent_foundry_unlocked";
        public const string EventRepairStarted = "silent_foundry_repair_started";
        public const string EventRepaired = "silent_foundry_repaired";
        public const string EventMaintenanceDue = "silent_foundry_maintenance_due";
        public const string EventHeatPrepared = "silent_foundry_heat_prepared";
        public const string EventHeatStarted = "silent_foundry_heat_started";
        public const string EventHeatCompleted = "silent_foundry_heat_completed";
        public const string EventCastCompleted = "silent_foundry_cast_completed";
        public const string EventCastFailed = "silent_foundry_cast_failed";
        public const string EventSafetyWarning = "silent_foundry_safety_warning";
        public const string EventIncident = "silent_foundry_incident";
        public const string EventTreatyQuotaMet = "silent_foundry_treaty_quota_met";
        public const string EventTreatyQuotaMissed = "silent_foundry_treaty_quota_missed";
        public const string EventConsequenceApplied = "silent_foundry_treaty_consequence_applied";
        public const string EventLaborDispute = "silent_foundry_labor_dispute";
        public const string EventStrikeStarted = "silent_foundry_strike_started";
        public const string EventStrikeResolved = "silent_foundry_strike_resolved";
        public const string EventBlueprintReferenced = "silent_foundry_blueprint_referenced";
        public const string EventJournalTriggered = "silent_foundry_journal_triggered";

        // Typed events (established convention — no third bus).
        public event Action<SilentFoundryState> OnStateChanged;
        public event Action<FoundryProductionRecord> OnProductionCompleted;
        public event Action<FoundryFailedCastRecord> OnCastFailed;
        public event Action<string> OnSafetyWarning;
        public event Action<FoundryIncidentRecord> OnIncident;
        public event Action<FoundryTreatyCompliance> OnTreatyQuotaMet;
        public event Action<FoundryTreatyCompliance> OnTreatyQuotaMissed;
        /// <summary>Fired once per assessment cycle when a policy consequence is applied.</summary>
        public event Action<FoundryConsequenceRecord> OnConsequenceApplied;
        public event Action<FoundryLaborDispute, int> OnLaborDisputeChanged;
        public event Action<FoundryStrikeResolution, int> OnStrikeResolved;
        public event Action<FoundryJournalTrigger> OnJournalTriggered;
        /// <summary>Forwarder for the string event bus (optional).</summary>
        public event Action<string> OnEventRaised;

        private readonly SilentFoundryState _state;
        private ISeededRng _rng;
        private readonly Func<int, ISeededRng> _rngFactory;
        private readonly ILog _log;
        private SilentFoundryCatalog _catalog = new SilentFoundryCatalog();
        private readonly Dictionary<string, int> _treatyRatificationDays = new Dictionary<string, int>(StringComparer.Ordinal);
        private SilentFoundryConsequencePolicyCatalog _consequencePolicy = new SilentFoundryConsequencePolicyCatalog();
        private readonly SilentFoundryConsequenceState _consequenceState = new SilentFoundryConsequenceState();

        // Inventory ports (wired by host; deterministic, no host objects here).
        private Func<string, int> _getCount = _ => 0;
        private Func<string, int, bool> _canAdd = (_, _) => false;
        private Action<string, int> _addItem = (_, _) => { };
        private Action<string, int> _consume = (_, _) => { };

        /// <summary>Standing range mirrors the existing FactionStanceConstants [-100, 100].</summary>
        public const float StandingMin = -100f;
        public const float StandingMax = 100f;
        public const float StandingNeutral = 0f;

        public SilentFoundrySystem(
SilentFoundryState? state = null,
ISeededRng? rng = null,
Func<int, ISeededRng>? rngFactory = null,
ILog? log = null)
        {
            _rngFactory = rngFactory ?? (seed => new SeededRng(seed));
            _state = state ?? new SilentFoundryState();
            _rng = rng ?? _rngFactory(_state.rngSeed == 0 ? DefaultSeed : _state.rngSeed);
            _state.rngSeed = _rng.Seed;
            _log = log ?? NullLog.Instance;
            NormalizeState();
        }

        private void NormalizeState()
        {
            if (_state.stateVersion < 1 || _state.stateVersion > SilentFoundryState.CurrentVersion)
                _state.stateVersion = SilentFoundryState.CurrentVersion;
            if (_state.completed == null) _state.completed = new List<FoundryProductionRecord>();
            if (_state.failed == null) _state.failed = new List<FoundryFailedCastRecord>();
            if (_state.incidents == null) _state.incidents = new List<FoundryIncidentRecord>();
            if (_state.repairs == null) _state.repairs = new List<FoundryRepairRecord>();
            if (_state.treatyCompliance == null) _state.treatyCompliance = new List<FoundryTreatyCompliance>();
            if (_state.triggeredJournals == null) _state.triggeredJournals = new List<string>();
        }

        // -----------------------------------------------------------------
        // Binding
        // -----------------------------------------------------------------

        /// <summary>
        /// Bind the static production catalog and blueprint-derived constants.
        /// The blueprint is static authored data — only its typed values are read.
        /// </summary>
        public void BindCatalog(SilentFoundryCatalog catalog, int maintenanceCycleDaysFromBlueprint)
        {
            if (catalog != null) _catalog = catalog;
            _state.maintenanceCycleDays = maintenanceCycleDaysFromBlueprint > 0
                ? maintenanceCycleDaysFromBlueprint
                : 4;
            Raise(EventBlueprintReferenced, "blueprint room_bp_11_the_silent_foundry_smelter_bay referenced; maintenance cycle "
                + _state.maintenanceCycleDays + "d");
        }

        /// <summary>Bind treaty ratification-day anchors (exact ids from RegionalTreatyCatalog).</summary>
        public void BindTreaties(IReadOnlyDictionary<string, int> ratificationDaysById)
        {
            if (ratificationDaysById == null) return;
            _treatyRatificationDays.Clear();
            foreach (var kvp in ratificationDaysById)
            {
                if (!string.IsNullOrEmpty(kvp.Key) && kvp.Value > 0)
                    _treatyRatificationDays[kvp.Key] = kvp.Value;
            }
            EnsureTreatyComplianceRows();
        }

        /// <summary>
        /// Bind the authored consequence policy catalog. Without a bound policy
        /// no consequences are applied (treaty assessment still records met/missed).
        /// </summary>
        public void BindConsequencePolicy(SilentFoundryConsequencePolicyCatalog policy)
        {
            if (policy == null) return;
            _consequencePolicy = policy;
            if (policy.HasErrors)
            {
                for (int i = 0; i < policy.Errors.Count; i++)
                    _log.Warn("[SilentFoundry] consequence policy: " + policy.Errors[i]);
            }
        }

        /// <summary>Bind inventory ports. Defaults to a sealed (no-op) inventory.</summary>
        public void BindInventory(
            Func<string, int> getCount,
            Func<string, int, bool> canAdd,
            Action<string, int> addItem,
            Action<string, int> consume)
        {
            if (getCount != null) _getCount = getCount;
            if (canAdd != null) _canAdd = canAdd;
            if (addItem != null) _addItem = addItem;
            if (consume != null) _consume = consume;
        }

        // -----------------------------------------------------------------
        // State queries
        // -----------------------------------------------------------------

        public SilentFoundryState State => _state;
        public SilentFoundryCatalog Catalog => _catalog;
        public bool IsUnlocked => _state.unlocked;
        public FoundryHeatStage HeatStage => _state.heatStage;
        public FoundryLaborDispute LaborDispute => _state.laborDispute;
        public bool IsMaintenanceOverdue => _state.daysSinceMaintenance > _state.maintenanceCycleDays;
        public int DaysOverdue => Math.Max(0, _state.daysSinceMaintenance - _state.maintenanceCycleDays);
        public int OverdueCycles => _state.maintenanceCycleDays > 0
            ? DaysOverdue / _state.maintenanceCycleDays : 0;

        public float GetComponentCondition(FoundryFacilityComponent component)
        {
            switch (component)
            {
                case FoundryFacilityComponent.RefractoryLining: return _state.refractoryLining;
                case FoundryFacilityComponent.HearthTuyeres: return _state.hearthTuyeres;
                case FoundryFacilityComponent.SandBeds: return _state.sandBeds;
                case FoundryFacilityComponent.StructuralSupports: return _state.structuralSupports;
                case FoundryFacilityComponent.SafetyExhaust: return _state.safetyExhaust;
                default: return 0f;
            }
        }

        public float AverageFacilityCondition()
        {
            return (GetComponentCondition(FoundryFacilityComponent.RefractoryLining)
                + GetComponentCondition(FoundryFacilityComponent.HearthTuyeres)
                + GetComponentCondition(FoundryFacilityComponent.SandBeds)
                + GetComponentCondition(FoundryFacilityComponent.StructuralSupports)
                + GetComponentCondition(FoundryFacilityComponent.SafetyExhaust)) / 5f;
        }

        public bool IsJournalTriggered(string templateId) =>
            _state.triggeredJournals != null && _state.triggeredJournals.Contains(templateId);

        public FoundryTreatyCompliance? GetTreatyCompliance(string treatyId)
        {
            if (_state.treatyCompliance == null) return null;
            for (int i = 0; i < _state.treatyCompliance.Count; i++)
                if (_state.treatyCompliance[i] != null
                    && string.Equals(_state.treatyCompliance[i].treatyId, treatyId, StringComparison.Ordinal))
                    return _state.treatyCompliance[i];
            return null;
        }

        public IReadOnlyList<FoundryProductionRecord> CompletedProduction => _state.completed;
        public IReadOnlyList<FoundryFailedCastRecord> FailedCasts => _state.failed;
        public IReadOnlyList<FoundryIncidentRecord> Incidents => _state.incidents;
        public int TotalProductionCount => _state.completed.Count;
        public int TotalFailedCount => _state.failed.Count;
        public float CumulativeStress => _state.cumulativeStress;
        public float CumulativeHope => _state.cumulativeHope;

        public bool IsHeatActive => HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete;

        /// <summary>Electrical power demand in kW for the active heat stage.</summary>
        public float CurrentPowerDemandKw => HeatStage switch
        {
            FoundryHeatStage.ChargeLoaded => 3.0f,
            FoundryHeatStage.Preheat => 15.0f,
            FoundryHeatStage.AtHeat => 22.0f,
            FoundryHeatStage.Tapped => 18.0f,
            FoundryHeatStage.Casting => 16.0f,
            FoundryHeatStage.Cooling => 4.0f,
            _ => 0f
        };

        /// <summary>Useful waste heat in kW emitted into adjacent shelter facilities.</summary>
        public float CurrentWasteHeatKw => HeatStage switch
        {
            FoundryHeatStage.ChargeLoaded => 2.0f,
            FoundryHeatStage.Preheat => 12.0f,
            FoundryHeatStage.AtHeat => 25.0f,
            FoundryHeatStage.Tapped => 22.0f,
            FoundryHeatStage.Casting => 18.0f,
            FoundryHeatStage.Cooling => 6.0f,
            _ => 0f
        };

        /// <summary>
        /// Suspend or abort active heat due to electrical grid brownout or lack of power.
        /// </summary>
        public void SuspendHeat(string reason, int day)
        {
            if (!IsHeatActive) return;
            var product = _catalog.GetProduct(_state.activeProductId);
            _state.failed.Add(new FoundryFailedCastRecord
            {
                productId = _state.activeProductId,
                displayName = product?.display_name ?? _state.activeProductId,
                reason = "Heat aborted due to grid power failure: " + reason,
                failedDay = day,
                materialsLost = _state.materialsConsumed
            });
            _state.heatStage = FoundryHeatStage.Idle;
            _state.activeProductId = string.Empty;
            _state.assignedWorkers = 0;
            _state.laborAccumulated = 0f;
            Raise(EventCastFailed, "heat aborted: power grid failure (day " + day + ")");
            OnSafetyWarning?.Invoke("Foundry heat aborted: electrical power failure.");
            RaiseStateChanged();
        }

        // ── Treaty consequence queries ─────────────────────────────────

        /// <summary>Authoritative net standing of the Foundry Guild from its treaties.</summary>
        public float GuildStanding => _consequenceState.guildStanding;

        /// <summary>Idempotency + audit ledger of applied consequences.</summary>
        public IReadOnlyList<FoundryConsequenceRecord> AppliedConsequences => _consequenceState.applied;

        public bool IsConsequenceApplied(string treatyId, int cycleMarker)
            => _consequenceState.IsApplied(treatyId, cycleMarker);

        /// <summary>
        /// Derive the current outcome state for a treaty. NotRatified and Pending
        /// are neutral; the last applied consequence (if any) carries the outcome.
        /// Pure derivation — no mutable state.
        /// </summary>
        public FoundryTreatyOutcome GetTreatyOutcome(string treatyId, int day)
        {
            if (string.IsNullOrEmpty(treatyId)) return FoundryTreatyOutcome.NotRatified;
            int ratificationDay = _treatyRatificationDays.TryGetValue(treatyId, out int rDay) ? rDay : 0;
            if (ratificationDay <= 0 || day < ratificationDay) return FoundryTreatyOutcome.NotRatified;

            var c = GetTreatyCompliance(treatyId);
            if (c == null || c.lastAssessmentDay == 0) return FoundryTreatyOutcome.Pending;

            // The most recent applied consequence for this treaty is the outcome.
            FoundryConsequenceRecord? latest = null;
            for (int i = 0; i < _consequenceState.applied.Count; i++)
            {
                var r = _consequenceState.applied[i];
                if (r != null && string.Equals(r.treatyId, treatyId, StringComparison.Ordinal)
                    && (latest == null || r.appliedDay > latest.appliedDay))
                    latest = r;
            }
            if (latest != null) return latest.outcome;

            // Assessed but no policy consequence exists (e.g. treaty_16) — fall
            // back to the compliance row's coarse signal.
            return c.missedCount > c.metCount ? FoundryTreatyOutcome.Missed
                : c.metCount > 0 ? FoundryTreatyOutcome.Met
                : FoundryTreatyOutcome.Pending;
        }

        // -----------------------------------------------------------------
        // Unlock & facilities
        // -----------------------------------------------------------------

        /// <summary>Unlock the Foundry. Idempotent; raises the unlock event once.</summary>
        public bool Unlock(int day)
        {
            if (_state.unlocked) return false;
            _state.unlocked = true;
            _state.unlockDay = day;
            EnsureTreatyComplianceRows();
            Raise(EventUnlocked, "The Silent Foundry is open (day " + day + ").");
            RaiseStateChanged();
            return true;
        }

        /// <summary>
        /// Repair one facility component. Consumes firebrick + labour + time
        /// (the repair happens over a full day). Returns a human-readable reason
        /// when it cannot start.
        /// </summary>
        public string StartRepair(FoundryFacilityComponent component, int day)
        {
            if (!_state.unlocked) return "The Foundry is not unlocked.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "A heat is in progress; repairs cannot start on the active furnace floor.";

            int firebrickCost = ComponentRepairCost(component);
            if (_getCount(SilentFoundryIds.ItemFirebrick) < firebrickCost)
                return "Not enough firebrick (" + firebrickCost + " required, " + _getCount(SilentFoundryIds.ItemFirebrick) + " held).";

            _consume(SilentFoundryIds.ItemFirebrick, firebrickCost);
            float before = GetComponentCondition(component);
            float restored = 100f;
            switch (component)
            {
                case FoundryFacilityComponent.RefractoryLining: _state.refractoryLining = restored; break;
                case FoundryFacilityComponent.HearthTuyeres: _state.hearthTuyeres = restored; break;
                case FoundryFacilityComponent.SandBeds: _state.sandBeds = restored; break;
                case FoundryFacilityComponent.StructuralSupports: _state.structuralSupports = restored; break;
                case FoundryFacilityComponent.SafetyExhaust: _state.safetyExhaust = restored; break;
            }
            _state.repairs.Add(new FoundryRepairRecord
            {
                component = component.ToString(),
                day = day,
                conditionBefore = before,
                conditionAfter = restored
            });
            Raise(EventRepairStarted, "repair started on " + component + " (day " + day + ")");
            Raise(EventRepaired, component.ToString() + " restored to " + restored.ToString("F0"));
            RaiseStateChanged();
            return "Repair complete: " + component + " restored to 100.";
        }

        /// <summary>Perform full maintenance. Resets the 4-day cycle.</summary>
        public string PerformMaintenance(int day)
        {
            if (!_state.unlocked) return "The Foundry is not unlocked.";
            _state.maintenanceDueDay = day + _state.maintenanceCycleDays;
            _state.daysSinceMaintenance = 0;
            _state.maintenancePerformed++;
            // Service restores a little wear without a full rebuild.
            _state.refractoryLining = Math.Min(100f, _state.refractoryLining + 6f);
            _state.hearthTuyeres = Math.Min(100f, _state.hearthTuyeres + 6f);
            _state.safetyExhaust = Math.Min(100f, _state.safetyExhaust + 6f);
            Raise(EventRepaired, "full maintenance performed; next due day " + _state.maintenanceDueDay);
            RaiseStateChanged();
            return "Maintenance performed. Next service due day " + _state.maintenanceDueDay + ".";
        }

        private static int ComponentRepairCost(FoundryFacilityComponent component)
        {
            switch (component)
            {
                case FoundryFacilityComponent.RefractoryLining: return 8;
                case FoundryFacilityComponent.HearthTuyeres: return 10;
                case FoundryFacilityComponent.SandBeds: return 4;
                case FoundryFacilityComponent.StructuralSupports: return 12;
                case FoundryFacilityComponent.SafetyExhaust: return 6;
                default: return 6;
            }
        }

        // -----------------------------------------------------------------
        // Green-sand casting bed
        // -----------------------------------------------------------------

        /// <summary>
        /// Replenish/refresh the sand bed: consumes green sand (and optionally
        /// water) and resets the reuse counter. Player choice: preserve scarce
        /// high-quality sand for a critical cast or refresh now.
        /// </summary>
        public string PrepareSand(int waterLitres)
        {
            if (!_state.unlocked) return "The Foundry is not unlocked.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "Cannot prepare sand while a heat is active.";

            int sandNeeded = 2;
            if (_getCount(SilentFoundryIds.ItemGreenSand) < sandNeeded)
                return "Not enough green sand (" + sandNeeded + " required).";

            int waterAvailable = _getCount(SilentFoundryIds.ItemCleanWater);
            if (waterAvailable < waterLitres)
                return "Not enough clean water (" + waterLitres + " required, " + waterAvailable + " held).";

            _consume(SilentFoundryIds.ItemGreenSand, sandNeeded);
            if (waterLitres > 0) _consume(SilentFoundryIds.ItemCleanWater, waterLitres);

            _state.sandQuality = Math.Min(100f, _state.sandQuality + 12f);
            _state.sandMoisture = MathfCompat.Clamp(60f + waterLitres * 0.1f, 0f, 100f);
            _state.binderQuality = Math.Min(100f, _state.binderQuality + 6f);
            _state.contamination = Math.Max(0f, _state.contamination - 15f);
            _state.moldReuseCount = 0;
            _state.compaction = 70f;
            RaiseStateChanged();
            return "Sand bed refreshed: quality " + _state.sandQuality.ToString("F0")
                + ", moisture " + _state.sandMoisture.ToString("F0") + ".";
        }

        /// <summary>Compact the mold with the available skill; raises compaction.</summary>
        public string CompactMold(float skill)
        {
            if (!_state.unlocked) return "The Foundry is not unlocked.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "Cannot work the mold while a heat is active.";
            _state.compaction = MathfCompat.Clamp(_state.compaction + 20f * MathfCompat.Clamp(skill, 0f, 1f), 0f, 100f);
            _state.patternQuality = MathfCompat.Clamp(_state.patternQuality + 4f * skill, 0f, 100f);
            RaiseStateChanged();
            return "Mold compacted. Compaction " + _state.compaction.ToString("F0") + ".";
        }

        // -----------------------------------------------------------------

        // Treaty compliance
        // -----------------------------------------------------------------

        private void EnsureTreatyComplianceRows()
        {
            if (_state.treatyCompliance == null) _state.treatyCompliance = new List<FoundryTreatyCompliance>();

            void Ensure(string treatyId, string obligation)
            {
                if (GetTreatyCompliance(treatyId) == null)
                {
                    _state.treatyCompliance.Add(new FoundryTreatyCompliance
                    {
                        treatyId = treatyId,
                        obligation = obligation,
                        quotaDeadlineDay = _treatyRatificationDays.TryGetValue(treatyId, out int day) ? day : 0
                    });
                }
            }

            Ensure(SilentFoundryIds.TreatyBrinePipe, "brine_pipe_quota");
            Ensure(SilentFoundryIds.TreatyLabourSchedule, "labor_shifts");
            Ensure(SilentFoundryIds.TreatyRoadIron, "road_iron_quota");
            Ensure(SilentFoundryIds.TreatyClusterCharter, "charter_eligibility");
        }

        private void ApplyQuotaFulfilment(FoundryProductEntry product, int amount)
        {
            if (string.IsNullOrEmpty(product.treaty_id) || product.quota_amount <= 0) return;
            var c = GetTreatyCompliance(product.treaty_id);
            if (c == null) return;
            c.quotaFulfilled += amount;
            c.currentCycleMet = c.quotaFulfilled >= c.quotaTotal;
        }

        /// <summary>
        /// Evaluate every tracked treaty. Assessment days are derived from each
        /// treaty's authored ratification day plus its assessment cycle (30 days).
        /// Day-agnostic — works for synthetic days and long campaigns alike.
        /// </summary>
        public void AssessTreatyCompliance(int day)
        {
            if (_state.treatyCompliance == null) return;

            // Initialize quota totals from the catalog each assessment.
            for (int i = 0; i < _state.treatyCompliance.Count; i++)
            {
                var c = _state.treatyCompliance[i];
                if (c == null) continue;
                if (c.quotaTotal == 0 && !string.IsNullOrEmpty(c.treatyId))
                {
                    int total = 0;
                    for (int p = 0; p < _catalog.AllProducts.Count; p++)
                    {
                        var prod = _catalog.AllProducts[p];
                        if (prod != null && !string.IsNullOrEmpty(prod.treaty_id)
                            && string.Equals(prod.treaty_id, c.treatyId, StringComparison.Ordinal)
                            && prod.quota_amount > 0)
                        {
                            total += prod.quota_amount;
                        }
                    }
                    c.quotaTotal = total;
                }
            }

            for (int i = 0; i < _state.treatyCompliance.Count; i++)
            {
                var c = _state.treatyCompliance[i];
                if (c == null || string.IsNullOrEmpty(c.treatyId)) continue;

                int ratificationDay = _treatyRatificationDays.TryGetValue(c.treatyId, out int rDay) ? rDay : 0;
                if (ratificationDay <= 0) continue;
                if (day < ratificationDay) continue;

                const int assessmentCycle = 30;
                int assessmentDay = ratificationDay + ((day - ratificationDay) / assessmentCycle) * assessmentCycle;
                if (assessmentDay == day && day != c.lastAssessmentDay)
                {
                    AssessOne(c, day);
                    c.lastAssessmentDay = day;
                }

                // Cluster-charter eligibility: late-campaign marker, derived not asserted.
                // (Serialized field name `constitutionEligible` kept for save compatibility.)
                if (string.Equals(c.treatyId, SilentFoundryIds.TreatyClusterCharter, StringComparison.Ordinal))
                {
                    c.constitutionEligible = _state.incidents.Count == 0 || day < ratificationDay;
                }
            }
        }

        private void AssessOne(FoundryTreatyCompliance c, int day)
        {
            switch (c.obligation)
            {
                case "road_iron_quota":
                case "brine_pipe_quota":
                    if (c.quotaTotal > 0)
                    {
                        c.currentCycleMet = c.quotaFulfilled >= c.quotaTotal;
                        if (c.currentCycleMet)
                        {
                            c.metCount++;
                            Raise(EventTreatyQuotaMet, c.treatyId + " quota met day " + day);
                            OnTreatyQuotaMet?.Invoke(c);
                            ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Met, day);
                        }
                        else
                        {
                            c.missedCount++;
                            Raise(EventTreatyQuotaMissed, c.treatyId + " quota missed day " + day
                                + " (" + c.quotaFulfilled + "/" + c.quotaTotal + ")");
                            OnTreatyQuotaMissed?.Invoke(c);
                            ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Missed, day);
                        }
                        c.quotaFulfilled = 0; // next cycle starts fresh
                        c.currentCycleMet = false;
                    }
                    break;

                case "labor_shifts":
                    // The labour schedule accord: shifts capped at 8h during
                    // liquid pours; workers hold a water ration; lockouts and
                    // strikes close the coal window. A strike or open overtime
                    // is a violation.
                    // Fatigue alone is never a violation — only the policy-level
                    // labor semantics (strike / overtime / child labor) count.
                    bool violation = _state.laborDispute == FoundryLaborDispute.StrikeActive
                        || _state.overtimeFlag || _state.childLaborUsed;
                    if (violation)
                    {
                        c.missedCount++;
                        Raise(EventTreatyQuotaMissed, c.treatyId + " labor-shift violation day " + day);
                        OnTreatyQuotaMissed?.Invoke(c);
                        ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Violated, day);
                    }
                    else
                    {
                        c.metCount++;
                        Raise(EventTreatyQuotaMet, c.treatyId + " labor accord upheld day " + day);
                        OnTreatyQuotaMet?.Invoke(c);
                        ApplyConsequence(c.treatyId, FoundryTreatyOutcome.Met, day);
                    }
                    break;

                case "charter_eligibility":
                    // The Cluster Charter is a finale marker, not a quota.
                    // Eligibility is derived; no economy or standing consequence
                    // is defined for it by policy (regression-guarded in tests).
                    break;
            }
        }

        /// <summary>
        /// Apply the authored policy consequence for a treaty outcome — once per
        /// assessment cycle. Idempotency is keyed by (treatyId, cycleMarker) and
        /// the cycleMarker is the assessment day, so reloading a save or calling
        /// AssessTreatyCompliance again on the same day never re-applies.
        /// Pre-ratification and pending states are neutral by construction: this
        /// is only reached from AssessOne, which the ratification gate already
        /// guards.
        /// </summary>
        private void ApplyConsequence(string treatyId, FoundryTreatyOutcome outcome, int day)
        {
            if (string.IsNullOrEmpty(treatyId)) return;
            if (outcome != FoundryTreatyOutcome.Met
                && outcome != FoundryTreatyOutcome.Missed
                && outcome != FoundryTreatyOutcome.Violated) return;

            int cycleMarker = day;
            if (_consequenceState.IsApplied(treatyId, cycleMarker)) return;

            var policy = _consequencePolicy.Find(treatyId, outcome);
            if (policy == null)
            {
                // No authored policy → the outcome is recorded but carries no
                // consequence (e.g. treaty_16 has no policy by design).
                return;
            }

            float nextStanding = MathfCompat.Clamp(
                _consequenceState.guildStanding + policy.standing_delta, StandingMin, StandingMax);
            _consequenceState.guildStanding = nextStanding;

            var record = new FoundryConsequenceRecord
            {
                treatyId = treatyId,
                outcome = outcome,
                appliedDay = day,
                cycleMarker = cycleMarker,
                standingDelta = policy.standing_delta,
                reason = policy.reason,
                modifiers = new List<FoundryGoodModifier>()
            };
            if (policy.market_modifiers != null)
            {
                for (int i = 0; i < policy.market_modifiers.Count; i++)
                {
                    if (policy.market_modifiers[i] == null) continue;
                    record.modifiers.Add(new FoundryGoodModifier
                    {
                        good_id = policy.market_modifiers[i].good_id,
                        demand_delta = policy.market_modifiers[i].demand_delta,
                        reason = policy.market_modifiers[i].reason
                    });
                }
            }
            _consequenceState.applied.Add(record);

            Raise(EventConsequenceApplied, treatyId + " " + SilentFoundryConsequencePolicyCatalog.OutcomeName(outcome)
                + " day " + day + " standing " + policy.standing_delta.ToString("F0"));
            OnConsequenceApplied?.Invoke(record);
        }

        // -----------------------------------------------------------------

        // Internals
        // -----------------------------------------------------------------

        private void Raise(string eventId, string message)
        {
            OnEventRaised?.Invoke(eventId);
            _log.Info("[SilentFoundry] " + eventId + " — " + message);
        }

        private void RaiseStateChanged() => OnStateChanged?.Invoke(_state);
    }
}

