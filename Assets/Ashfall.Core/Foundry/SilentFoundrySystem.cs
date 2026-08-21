using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Foundry
{
    // ---------------------------------------------------------------------
    // Identity (exact ids from the authored data — never aliased)
    // ---------------------------------------------------------------------

    public static class SilentFoundryIds
    {
        public const string ExpansionId = "exp_10_the_silent_foundry";
        public const string FactionId = "faction_silent_foundry";
        public const string BlueprintRoomId = "room_bp_11_the_silent_foundry_smelter_bay";

        public const string JournalFirstHeat = "jrnl_foundry_first_heat";
        public const string JournalStrike = "jrnl_foundry_strike";

        // District 8 accords (data authority: foundry_accords.json).
        public const string TreatyBrinePipe = "treaty_brine_pipe_and_iodine_exchange";
        public const string TreatyLabourSchedule = "treaty_cluster_labour_schedule";
        public const string TreatyRoadIron = "treaty_road_iron_charter";
        public const string TreatyClusterCharter = "treaty_the_cluster_charter";

        // Charge/consumable material ids (items.json / foundry_items.json).
        public const string ItemScrapMetal = "scrap_metal";
        public const string ItemCoal = "coal";
        public const string ItemCharcoal = "charcoal";
        public const string ItemCleanWater = "clean_water";
        public const string ItemFirebrick = "item_foundry_firebrick";
        public const string ItemGreenSand = "item_foundry_green_sand";
        public const string ItemFlux = "item_foundry_flux";
        public const string ItemAlloyAdditive = "item_foundry_alloy_additive";
    }

    // ---------------------------------------------------------------------
    // Domain enums
    // ---------------------------------------------------------------------

    /// <summary>Heat lifecycle stages. Deterministic, prerequisite-gated.</summary>
    public enum FoundryHeatStage
    {
        Idle = 0,
        ChargeLoaded = 1,
        Preheat = 2,
        AtHeat = 3,
        Tapped = 4,
        Casting = 5,
        Cooling = 6,
        Complete = 7
    }

    /// <summary>Labor dispute ladder. Escalates only from real conflicts.</summary>
    public enum FoundryLaborDispute
    {
        None = 0,
        Tensions = 1,
        StrikeActive = 2,
        Resolved = 3
    }

    public enum FoundryQualityTier
    {
        Scrap = 0,
        Usable = 1,
        Good = 2,
        Fine = 3
    }

    public enum FoundryIncidentSeverity
    {
        None = 0,
        Contained = 1,
        Severe = 2
    }

    public enum FoundryFacilityComponent
    {
        RefractoryLining = 0,
        HearthTuyeres = 1,
        SandBeds = 2,
        StructuralSupports = 3,
        SafetyExhaust = 4
    }

    public enum FoundryStrikeResolution
    {
        ConcedeShiftLimits = 0,
        UpholdQuota = 1,
        Mediation = 2
    }

    // ---------------------------------------------------------------------
    // History records (mutable, serialized)
    // ---------------------------------------------------------------------

    [Serializable]
    public sealed class FoundryProductionRecord
    {
        public string productId = string.Empty;
        public string displayName = string.Empty;
        public int amount = 0;
        public FoundryQualityTier tier = FoundryQualityTier.Usable;
        public int completedDay = 0;
        public int workers = 0;
    }

    [Serializable]
    public sealed class FoundryFailedCastRecord
    {
        public string productId = string.Empty;
        public string displayName = string.Empty;
        public string reason = string.Empty;
        public int failedDay = 0;
        public int materialsLost = 0;
    }

    [Serializable]
    public sealed class FoundryIncidentRecord
    {
        public FoundryIncidentSeverity severity = FoundryIncidentSeverity.None;
        public int day = 0;
        public string summary = string.Empty;
        public int workersInjured = 0;
        public int downtimeDays = 0;
    }

    [Serializable]
    public sealed class FoundryRepairRecord
    {
        public string component = string.Empty;
        public int day = 0;
        public float conditionBefore = 0f;
        public float conditionAfter = 0f;
    }

    [Serializable]
    public sealed class FoundryTreatyCompliance
    {
        public string treatyId = string.Empty;
        public string obligation = string.Empty;   // brine_pipe_quota | labor_shifts | road_iron_quota | charter_eligibility
        public int quotaTotal = 0;
        public int quotaFulfilled = 0;
        public int quotaDeadlineDay = 0;           // next assessment day
        public int lastAssessmentDay = 0;
        public int metCount = 0;
        public int missedCount = 0;
        public bool currentCycleMet = false;
        /// <summary>Sum of standing consequences from missed obligations.</summary>
        /// <summary>
        /// Legacy per-treaty penalty counter (pre-policy). Kept for old-save
        /// compat; the policy-driven <c>GuildStanding</c> is the standing authority.
        /// </summary>
        public float standingPenalty = 0f;
        public bool constitutionEligible = false;
    }

    // ---------------------------------------------------------------------
    // Save DTO (versioned, plain public fields, no host objects)
    // ---------------------------------------------------------------------

    [Serializable]
    public sealed class SilentFoundryState
    {
        public const int CurrentVersion = 1;

        public int stateVersion = CurrentVersion;

        public bool unlocked = false;
        public int unlockDay = 0;

        // Facility condition (0..100, authored baseline 100).
        public float refractoryLining = 100f;
        public float hearthTuyeres = 100f;
        public float sandBeds = 100f;
        public float structuralSupports = 100f;
        public float safetyExhaust = 100f;

        // Maintenance.
        public int maintenanceCycleDays = 4;       // authored anchor from room_bp_11
        public int maintenanceDueDay = 0;
        public int daysSinceMaintenance = 0;
        public int maintenancePerformed = 0;

        // Green-sand casting bed.
        public float sandQuality = 65f;
        public float sandMoisture = 65f;           // target band ~55..75
        public float binderQuality = 60f;
        public float patternQuality = 70f;
        public float contamination = 5f;           // grows with low-grade charge
        public int moldReuseCount = 0;
        public float compaction = 70f;

        // Heat lifecycle.
        public FoundryHeatStage heatStage = FoundryHeatStage.Idle;
        public int heatStartedDay = 0;
        public int stageElapsedDays = 0;

        // Active production.
        public string activeProductId = string.Empty;
        public int assignedWorkers = 0;
        public float workerSkill = 0.5f;
        public float laborAccumulated = 0f;
        public float workerExposure = 0f;          // fatigue/exposure units accrued
        public int materialsConsumed = 0;
        public bool childLaborUsed = false;

        // Output quality of the cast currently completing.
        public float pendingQuality = 0f;

        // History.
        public List<FoundryProductionRecord> completed = new List<FoundryProductionRecord>();
        public List<FoundryFailedCastRecord> failed = new List<FoundryFailedCastRecord>();
        public List<FoundryIncidentRecord> incidents = new List<FoundryIncidentRecord>();
        public List<FoundryRepairRecord> repairs = new List<FoundryRepairRecord>();

        // Labor.
        public FoundryLaborDispute laborDispute = FoundryLaborDispute.None;
        public int laborDisputeStartedDay = 0;
        public int strikeStartedDay = 0;
        public bool overtimeFlag = false;
        public bool educationConflictFlag = false;

        // Treaty compliance.
        public List<FoundryTreatyCompliance> treatyCompliance = new List<FoundryTreatyCompliance>();

        // Journal + morale.
        public List<string> triggeredJournals = new List<string>();
        public float cumulativeStress = 0f;
        public float cumulativeHope = 0f;
        public int firstHeatDay = 0;
        public int strikeDay = 0;

        // Determinism.
        public int rngSeed = 0;
    }

    // ---------------------------------------------------------------------
    // Journal trigger payload
    // ---------------------------------------------------------------------

    public sealed class FoundryJournalTrigger
    {
        public string TemplateId = string.Empty;
        public float StressDelta = 0f;
        public float HopeEarned = 0f;
        public int Day = 0;
    }

    // ---------------------------------------------------------------------
    // The system
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
    public sealed class SilentFoundrySystem
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
            SilentFoundryState state = null!,
            ISeededRng rng = null!,
            Func<int, ISeededRng> rngFactory = null!,
            ILog log = null!)
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
        // Production
        // -----------------------------------------------------------------

        /// <summary>
        /// Start a heat for the given product. Consumes the full charge
        /// (ingredients + fuel + water) immediately; failure to hold the charge
        /// later wastes it. Validates every prerequisite with a visible reason.
        /// </summary>
        public string StartProduction(string productId, int workers, float workerSkill, int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.laborDispute == FoundryLaborDispute.StrikeActive)
                return "The strike has shut the charging floor; no heat can start.";
            if (HeatStage != FoundryHeatStage.Idle && HeatStage != FoundryHeatStage.Complete)
                return "A heat is already in progress (" + HeatStage + ").";

            var product = _catalog.GetProduct(productId);
            if (product == null) return "Unknown product: " + productId;

            workers = MathfCompat.Clamp(workers, 1, MaxWorkers);
            if (workers < 1) return "At least one worker is required.";

            // Charge check (ingredients).
            for (int i = 0; i < product.ingredients.Count; i++)
            {
                var ing = product.ingredients[i];
                if (ing == null || string.IsNullOrEmpty(ing.item_id)) continue;
                int held = _getCount(ing.item_id);
                if (held < ing.amount)
                    return "Missing charge material " + ing.item_id + " (need " + ing.amount + ", have " + held + ").";
            }

            // Fuel check — coal or charcoal.
            int coal = _getCount(SilentFoundryIds.ItemCoal);
            int charcoal = _getCount(SilentFoundryIds.ItemCharcoal);
            if (coal + charcoal < product.fuel_units)
                return "Not enough fuel (" + product.fuel_units + " units of coal/charcoal required).";

            // Water check.
            int water = _getCount(SilentFoundryIds.ItemCleanWater);
            if (water < product.water_litres)
                return "Not enough clean water for the heat (" + product.water_litres + " required, " + water + " held).";

            // Consume charge deterministically: coal first, then charcoal.
            ConsumeFuel(product.fuel_units, coal, charcoal);
            _consume(SilentFoundryIds.ItemCleanWater, product.water_litres);
            for (int i = 0; i < product.ingredients.Count; i++)
            {
                var ing = product.ingredients[i];
                if (ing == null || string.IsNullOrEmpty(ing.item_id)) continue;
                _consume(ing.item_id, ing.amount);
            }

            int materialsConsumed = product.fuel_units + product.water_litres;
            for (int i = 0; i < product.ingredients.Count; i++)
                if (product.ingredients[i] != null)
                    materialsConsumed += product.ingredients[i].amount;

            _state.activeProductId = product.product_id;
            _state.assignedWorkers = workers;
            _state.workerSkill = MathfCompat.Clamp(workerSkill, 0f, 1f);
            _state.laborAccumulated = 0f;
            _state.materialsConsumed = materialsConsumed;
            _state.heatStartedDay = day;
            _state.stageElapsedDays = 0;
            _state.heatStage = FoundryHeatStage.ChargeLoaded;
            // A hot charge with low-grade ingredients sours the sand.
            _state.contamination = Math.Min(100f, _state.contamination + 2f);

            Raise(EventHeatPrepared, product.display_name + " charge loaded (" + materialsConsumed + " units of material)");
            Raise(EventHeatStarted, "heat started day " + day + " · " + workers + " workers");
            RaiseStateChanged();
            return "Heat started: " + product.display_name + " · " + workers + " workers · fuel " + product.fuel_units
                + " · water " + product.water_litres + "L.";
        }

        private void ConsumeFuel(int units, int coalHeld, int charcoalHeld)
        {
            int fromCoal = Math.Min(units, coalHeld);
            if (fromCoal > 0) _consume(SilentFoundryIds.ItemCoal, fromCoal);
            int fromCharcoal = units - fromCoal;
            if (fromCharcoal > 0) _consume(SilentFoundryIds.ItemCharcoal, fromCharcoal);
        }

        /// <summary>
        /// Tap and cast the furnace. This is the risk window: molten iron
        /// breakout through hearth brick / water-slag steam vapor explosion.
        /// Safety warnings surface first; a cast here can be lost or cause an
        /// incident. Deterministic given the seeded RNG.
        /// </summary>
        public string TapAndCast(int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (HeatStage != FoundryHeatStage.AtHeat)
                return "The furnace is not at heat. Stage: " + HeatStage + ".";

            var product = _catalog.GetProduct(_state.activeProductId);
            if (product == null)
            {
                _state.heatStage = FoundryHeatStage.Idle;
                RaiseStateChanged();
                return "No product bound to the current heat; furnace dumped.";
            }

            var warnings = GetSafetyWarnings();
            if (warnings.Count > 0)
            {
                for (int i = 0; i < warnings.Count; i++)
                {
                    Raise(EventSafetyWarning, warnings[i]);
                    _log.Warn("[SilentFoundry] " + warnings[i]);
                }
            }

            // Incident roll — only when the furnace is genuinely unsafe, never hidden.
            int incidentChance = ComputeIncidentChance();
            bool incident = incidentChance > 0 && _rng.Next(0, 100) < incidentChance;
            if (incident)
            {
                return ResolveIncident(product, day);
            }

            _state.heatStage = FoundryHeatStage.Tapped;
            _state.stageElapsedDays = 0;
            RaiseStateChanged();
            return "Tap successful. Molten " + product.display_name + " is in the ladle.";
        }

        /// <summary>Player-facing safety readout before the irreversible tap.</summary>
        public List<string> GetSafetyWarnings()
        {
            var warnings = new List<string>();
            if (_state.hearthTuyeres < 35f)
                warnings.Add("Hearth brick and tuyeres are badly worn (" + _state.hearthTuyeres.ToString("F0")
                    + "/100). Molten iron breakout risk.");
            if (_state.refractoryLining < 25f)
                warnings.Add("Refractory lining is critically spalled. The shell could fail under heat.");
            if (OverdueCycles >= 1)
                warnings.Add("Maintenance is overdue (" + DaysOverdue + " days). Furnace controls drift and fuel cost rises.");
            if (_state.safetyExhaust < 30f)
                warnings.Add("Exhaust/heat management is degraded. Fumes will concentrate on the charging floor.");
            if (_state.sandBeds < 25f)
                warnings.Add("Sand beds are damaged; castings will come out cracked or slagged.");
            return warnings;
        }

        /// <summary>Chance (0..100) of a catastrophic incident at the tap.</summary>
        public int ComputeIncidentChance()
        {
            int chance = 0;
            if (_state.hearthTuyeres < 20f && _state.refractoryLining < 25f) chance += 20;
            else if (_state.hearthTuyeres < 25f) chance += 8;
            if (OverdueCycles >= 2) chance += 12;
            if (OverdueCycles >= 1) chance += 6;
            if (_state.safetyExhaust < 25f) chance += 8;
            if (_state.sandMoisture > 90f && _state.hearthTuyeres < 40f) chance += 6; // steam pocket risk
            return Math.Min(60, chance);
        }

        private string ResolveIncident(FoundryProductEntry product, int day)
        {
            bool severe = _state.hearthTuyeres < 10f || OverdueCycles >= 3;
            var severity = severe ? FoundryIncidentSeverity.Severe : FoundryIncidentSeverity.Contained;
            int downtime = severe ? 7 : 3;
            int injured = severe ? _rng.Next(1, 3) : (_rng.Next(0, 100) < 40 ? 1 : 0);

            // Damage: the furnace takes the hit, not the whole shelter.
            _state.hearthTuyeres = Math.Max(5f, _state.hearthTuyeres - (severe ? 30f : 15f));
            _state.refractoryLining = Math.Max(5f, _state.refractoryLining - (severe ? 35f : 15f));
            _state.safetyExhaust = Math.Max(5f, _state.safetyExhaust - (severe ? 25f : 10f));

            var record = new FoundryIncidentRecord
            {
                severity = severity,
                day = day,
                summary = severe
                    ? "Molten iron broke through the hearth brick; a water-slag steam vapor explosion followed. "
                      + "The floor is shut for " + downtime + " days."
                    : "A splash and steam event on the charging floor. " + downtime + " days of lost heat.",
                workersInjured = injured,
                downtimeDays = downtime
            };
            _state.incidents.Add(record);

            // Worker exposure/fatigue consequence.
            _state.workerExposure += (severe ? 40f : 18f);

            _state.heatStage = FoundryHeatStage.Idle;
            _state.activeProductId = string.Empty;
            _state.assignedWorkers = 0;
            _state.laborAccumulated = 0f;
            _state.materialsConsumed = 0;

            Raise(EventIncident, severity + " incident day " + day + ": " + record.summary);
            OnIncident?.Invoke(record);
            RaiseStateChanged();
            return "INCIDENT: " + record.summary + " (downtime " + downtime + "d, injured " + injured + ").";
        }

        /// <summary>Player labor decisions that shape the strike conflict.</summary>
        public void SetOvertime(bool overtime) { _state.overtimeFlag = overtime; RaiseStateChanged(); }
        public void SetChildLaborUsed(bool used) { _state.childLaborUsed = used; RaiseStateChanged(); }

        /// <summary>
        /// Open a labor dispute. Requires a real conflict: production pressure
        /// (quota missed or an active heat under overtime/child labour) combined
        /// with education or shift grievances. Fatigue alone never triggers it.
        /// </summary>
        public string BeginLaborDispute(int day)
        {
            if (!_state.unlocked) return "The Silent Foundry is not unlocked.";
            if (_state.laborDispute != FoundryLaborDispute.None) return "A dispute is already open.";

            bool productionPressure = QuotaMissedRecently() || (_state.heatStage != FoundryHeatStage.Idle && _state.overtimeFlag);
            bool shiftGrievance = _state.overtimeFlag || _state.childLaborUsed;
            bool educationConflict = _state.educationConflictFlag || _state.childLaborUsed;

            if (!productionPressure || !(shiftGrievance || educationConflict))
                return "No genuine dispute conditions: production pressure=" + productionPressure
                    + " shiftGrievance=" + shiftGrievance + " educationConflict=" + educationConflict + ".";

            _state.laborDispute = FoundryLaborDispute.Tensions;
            _state.laborDisputeStartedDay = day;
            _state.educationConflictFlag = _state.educationConflictFlag || _state.childLaborUsed;
            Raise(EventLaborDispute, "labor tensions opened day " + day);
            RaiseStateChanged();
            return "Labor tensions opened. The charging floor is restless.";
        }

        /// <summary>Escalate to a full strike after unresolved tensions.</summary>
        public bool EscalateToStrike(int day)
        {
            if (_state.laborDispute != FoundryLaborDispute.Tensions) return false;
            _state.laborDispute = FoundryLaborDispute.StrikeActive;
            _state.strikeStartedDay = day;
            Raise(EventStrikeStarted, "strike active day " + day);
            OnLaborDisputeChanged?.Invoke(_state.laborDispute, day);
            // The strike is the journaled event state (jrnl_foundry_strike).
            MaybeTriggerJournal(SilentFoundryIds.JournalStrike, day);
            RaiseStateChanged();
            return true;
        }

        /// <summary>Resolve the strike with a player decision.</summary>
        public string ResolveStrike(FoundryStrikeResolution resolution, int day)
        {
            if (_state.laborDispute != FoundryLaborDispute.StrikeActive)
                return "No active strike to resolve.";

            switch (resolution)
            {
                case FoundryStrikeResolution.ConcedeShiftLimits:
                    _state.overtimeFlag = false;
                    _state.childLaborUsed = false;
                    _state.educationConflictFlag = true;
                    break;
                case FoundryStrikeResolution.UpholdQuota:
                    _state.overtimeFlag = true;
                    break;
                case FoundryStrikeResolution.Mediation:
                    _state.overtimeFlag = false;
                    _state.childLaborUsed = false;
                    break;
            }
            _state.laborDispute = FoundryLaborDispute.Resolved;
            Raise(EventStrikeResolved, resolution + " day " + day);
            OnStrikeResolved?.Invoke(resolution, day);
            RaiseStateChanged();
            return "Strike resolved via " + resolution + ".";
        }

        private bool QuotaMissedRecently()
        {
            for (int i = 0; i < _state.treatyCompliance.Count; i++)
            {
                var c = _state.treatyCompliance[i];
                if (c != null && c.missedCount > 0 && !c.currentCycleMet) return true;
            }
            return false;
        }

        // -----------------------------------------------------------------
        // Daily simulation
        // -----------------------------------------------------------------

        /// <summary>
        /// Advance one simulation day. Drives maintenance accounting, heat
        /// stage progression, labour escalation and treaty assessment.
        /// Deterministic; uses no wall-clock time.
        /// </summary>
        public void TickDaily(int day)
        {
            if (!_state.unlocked) return;

            // Maintenance accounting.
            if (_state.maintenanceDueDay > 0 && day > _state.maintenanceDueDay)
            {
                _state.daysSinceMaintenance++;
                if (_state.daysSinceMaintenance == _state.maintenanceCycleDays + 1)
                {
                    Raise(EventMaintenanceDue, "maintenance overdue since day " + _state.maintenanceDueDay);
                    OnSafetyWarning?.Invoke("Maintenance is overdue. Fuel cost and cast risk climb every day.");
                }
            }
            else if (_state.maintenanceDueDay == 0)
            {
                // Not yet commissioned — count from unlock.
                if (_state.unlockDay > 0)
                {
                    _state.daysSinceMaintenance = Math.Max(0, day - _state.unlockDay);
                    if (_state.daysSinceMaintenance == _state.maintenanceCycleDays + 1)
                        Raise(EventMaintenanceDue, "first maintenance window passed day " + day);
                }
            }

            // Gradual wear while the facility is in use (any heat or repair activity).
            float wear = HeatStage == FoundryHeatStage.Idle ? 0.15f : 0.9f;
            _state.refractoryLining = Math.Max(0f, _state.refractoryLining - wear * 0.5f);
            _state.hearthTuyeres = Math.Max(0f, _state.hearthTuyeres - wear * 0.7f);
            _state.safetyExhaust = Math.Max(0f, _state.safetyExhaust - wear * 0.4f);

            // Heat stage machine.
            AdvanceHeatStage(day);

            // Labor escalation: unresolved tensions escalate after one day.
            if (_state.laborDispute == FoundryLaborDispute.Tensions
                && day - _state.laborDisputeStartedDay >= 1)
            {
                EscalateToStrike(day);
            }

            // Treaty assessment at ratification/deadline days.
            AssessTreatyCompliance(day);

            RaiseStateChanged();
        }

        private void AdvanceHeatStage(int day)
        {
            if (HeatStage == FoundryHeatStage.Idle || HeatStage == FoundryHeatStage.Complete) return;

            _state.stageElapsedDays++;

            switch (HeatStage)
            {
                case FoundryHeatStage.ChargeLoaded:
                    // Wait a full day with the charge in the cupola.
                    if (_state.stageElapsedDays >= 1) SetStage(FoundryHeatStage.Preheat, day);
                    break;

                case FoundryHeatStage.Preheat:
                    // Overdue maintenance lengthens preheat (more fuel, slower climb).
                    int preheatDays = 1 + Math.Min(2, OverdueCycles);
                    if (_state.stageElapsedDays >= preheatDays) SetStage(FoundryHeatStage.AtHeat, day);
                    break;

                case FoundryHeatStage.AtHeat:
                    // The furnace holds. The player must tap; an un-tapped heat
                    // burns out after 3 days and wastes the charge (visible cost).
                    if (_state.stageElapsedDays >= 3)
                    {
                        _state.heatStage = FoundryHeatStage.Idle;
                        var product = _catalog.GetProduct(_state.activeProductId);
                        _state.failed.Add(new FoundryFailedCastRecord
                        {
                            productId = _state.activeProductId,
                            displayName = product?.display_name ?? _state.activeProductId,
                            reason = "Heat burned out untapped (furnace held too long).",
                            failedDay = day,
                            materialsLost = _state.materialsConsumed
                        });
                        _state.activeProductId = string.Empty;
                        _state.assignedWorkers = 0;
                        _state.laborAccumulated = 0f;
                        Raise(EventCastFailed, "heat burned out untapped day " + day);
                        OnCastFailed?.Invoke(_state.failed[_state.failed.Count - 1]);
                    }
                    break;

                case FoundryHeatStage.Tapped:
                    SetStage(FoundryHeatStage.Casting, day);
                    break;

                case FoundryHeatStage.Casting:
                    {
                        var product = _catalog.GetProduct(_state.activeProductId);
                        int castingDays = Math.Max(1, (int)Math.Ceiling((product?.cast_hours ?? 4f) / 24f));
                        if (_state.stageElapsedDays >= castingDays)
                        {
                            SetStage(FoundryHeatStage.Cooling, day);
                        }
                        else
                        {
                            // Labour accrues per day across assigned workers.
                            float labourPerDay = _state.assignedWorkers * 8f * (0.75f + 0.25f * _state.workerSkill);
                            _state.laborAccumulated += labourPerDay;
                            // Heat and fumes exact a cost on the crew.
                            _state.workerExposure += 4f;
                        }
                    }
                    break;

                case FoundryHeatStage.Cooling:
                    if (_state.stageElapsedDays >= 1) CompleteCast(day);
                    break;
            }
        }

        private void SetStage(FoundryHeatStage stage, int day)
        {
            _state.heatStage = stage;
            _state.stageElapsedDays = 0;
        }

        private void CompleteCast(int day)
        {
            var product = _catalog.GetProduct(_state.activeProductId);
            if (product == null)
            {
                _state.heatStage = FoundryHeatStage.Idle;
                _state.activeProductId = string.Empty;
                return;
            }

            float quality = RollQuality(product);
            _state.pendingQuality = quality;
            var tier = QualityTier(quality);

            if (tier == FoundryQualityTier.Scrap || quality <= 0f)
            {
                _state.failed.Add(new FoundryFailedCastRecord
                {
                    productId = product.product_id,
                    displayName = product.display_name,
                    reason = "Cast cracked or slagged (quality " + quality.ToString("F0") + ").",
                    failedDay = day,
                    materialsLost = _state.materialsConsumed
                });
                _state.heatStage = FoundryHeatStage.Idle;
                _state.activeProductId = string.Empty;
                _state.assignedWorkers = 0;
                _state.laborAccumulated = 0f;
                Raise(EventCastFailed, product.display_name + " cast failed day " + day + " (quality " + quality.ToString("F0") + ")");
                OnCastFailed?.Invoke(_state.failed[_state.failed.Count - 1]);
                RaiseStateChanged();
                return;
            }

            // Output lands in inventory when the host wired an inventory.
            if (_canAdd(product.result_item_id, product.result_amount))
            {
                _addItem(product.result_item_id, product.result_amount);
            }

            var record = new FoundryProductionRecord
            {
                productId = product.product_id,
                displayName = product.display_name,
                amount = product.result_amount,
                tier = tier,
                completedDay = day,
                workers = _state.assignedWorkers
            };
            _state.completed.Add(record);

            // Quota fulfilment.
            ApplyQuotaFulfilment(product, record.amount);

            _state.heatStage = FoundryHeatStage.Complete;
            _state.activeProductId = string.Empty;
            _state.assignedWorkers = 0;
            _state.laborAccumulated = 0f;

            Raise(EventCastCompleted, product.display_name + " ×" + record.amount + " (" + tier + ", quality " + quality.ToString("F0") + ") day " + day);
            OnProductionCompleted?.Invoke(record);

            // First successful heat → jrnl_foundry_first_heat (once).
            MaybeTriggerJournal(SilentFoundryIds.JournalFirstHeat, day);

            Raise(EventHeatCompleted, product.display_name + " cast completed day " + day);
            RaiseStateChanged();
        }

        private float RollQuality(FoundryProductEntry product)
        {
            float q = product.quality_target;
            q += ((_state.sandQuality - 60f) / 10f) * 5f;      // sand quality ±5
            q -= Math.Abs(_state.sandMoisture - 65f) * 0.35f;  // moisture deviation (target 65)
            q += ((_state.binderQuality - 50f) / 10f) * 4f;    // binder ±4
            q -= _state.contamination / 10f;                   // contamination 0..-10
            q += (_state.patternQuality / 100f) * 4f;          // pattern +0..4
            q += ((_state.compaction - 60f) / 10f) * 3f;       // compaction ±3
            q += ((_state.hearthTuyeres - 60f) / 10f) * 3f;    // furnace condition ±3
            q += ((_state.refractoryLining - 60f) / 10f) * 3f; // lining condition ±3
            q -= Math.Min(15f, DaysOverdue * 2.5f);            // maintenance neglect
            q += (_state.workerSkill - product.skill_target) * 12f; // skill ±6
            q += _rng.Next(-5, 6);                             // seeded jitter only

            // Mold reuse degrades the bed.
            _state.moldReuseCount++;
            _state.sandQuality = Math.Max(5f, _state.sandQuality - 2.5f);
            _state.binderQuality = Math.Max(5f, _state.binderQuality - 2f);
            _state.contamination = Math.Min(100f, _state.contamination + 3f);

            return MathfCompat.Clamp(q, 0f, 100f);
        }

        public static FoundryQualityTier QualityTier(float quality)
        {
            if (quality >= 90f) return FoundryQualityTier.Fine;
            if (quality >= 75f) return FoundryQualityTier.Good;
            if (quality >= 55f) return FoundryQualityTier.Usable;
            return FoundryQualityTier.Scrap;
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
        // Journal triggers (once-only, deterministic)
        // -----------------------------------------------------------------

        /// <summary>
        /// Trigger a journal template once. The narrative text itself stays in
        /// the authored template (NarrativeBatchCatalog); Core only owns the
        /// once-only guard, the typed deltas, and the event the host bridges.
        /// </summary>
        private void MaybeTriggerJournal(string templateId, int day)
        {
            if (IsJournalTriggered(templateId)) return;
            if (!TryGetJournalDeltas(templateId, out float stressDelta, out float hopeEarned)) return;

            _state.triggeredJournals.Add(templateId);
            if (templateId == SilentFoundryIds.JournalFirstHeat && _state.firstHeatDay == 0)
                _state.firstHeatDay = day;
            if (templateId == SilentFoundryIds.JournalStrike && _state.strikeDay == 0)
                _state.strikeDay = day;

            _state.cumulativeStress += stressDelta;
            _state.cumulativeHope += hopeEarned;

            var trigger = new FoundryJournalTrigger
            {
                TemplateId = templateId,
                StressDelta = stressDelta,
                HopeEarned = hopeEarned,
                Day = day
            };
            Raise(EventJournalTriggered, templateId + " triggered day " + day + " (stress " + stressDelta + ", hope " + hopeEarned + ")");
            OnJournalTriggered?.Invoke(trigger);
        }

        /// <summary>
        /// Typed journal moral deltas. These mirror the authored stress_delta /
        /// hope_earned values in jrnl_templates_cycle_d.json; the tests pin them
        /// to the JSON so drift is caught at build time, not in play.
        /// </summary>
        public static bool TryGetJournalDeltas(string templateId, out float stressDelta, out float hopeEarned)
        {
            switch (templateId)
            {
                case SilentFoundryIds.JournalFirstHeat:
                    stressDelta = -5f;
                    hopeEarned = 5f;
                    return true;
                case SilentFoundryIds.JournalStrike:
                    stressDelta = 7f;
                    hopeEarned = 2f;
                    return true;
                default:
                    stressDelta = 0f;
                    hopeEarned = 0f;
                    return false;
            }
        }

        // -----------------------------------------------------------------
        // Save / restore
        // -----------------------------------------------------------------

        public SilentFoundryState CaptureState()
        {
            NormalizeState();
            _state.stateVersion = SilentFoundryState.CurrentVersion;
            _state.rngSeed = _rng.Seed;
            return _state;
        }

        /// <summary>Capture the durable consequence ledger (rides the hub save envelope).</summary>
        public SilentFoundryConsequenceState CaptureConsequenceState()
        {
            if (_consequenceState.applied == null) _consequenceState.applied = new List<FoundryConsequenceRecord>();
            _consequenceState.stateVersion = SilentFoundryConsequenceState.CurrentVersion;
            return _consequenceState;
        }

        /// <summary>
        /// Restore the consequence ledger. Missing state (older saves) defaults to
        /// an empty ledger and neutral standing — nothing is re-applied because
        /// the ledger is the idempotency authority.
        /// </summary>
        public void RestoreConsequenceState(SilentFoundryConsequenceState save)
        {
            if (save == null) return;
            _consequenceState.stateVersion = Math.Max(1, save.stateVersion);
            _consequenceState.applied = save.applied ?? new List<FoundryConsequenceRecord>();
            _consequenceState.guildStanding = MathfCompat.Clamp(save.guildStanding, StandingMin, StandingMax);
        }

        public void RestoreState(SilentFoundryState save)
        {
            if (save == null) return;
            _state.stateVersion = save.stateVersion;
            _state.unlocked = save.unlocked;
            _state.unlockDay = save.unlockDay;
            _state.refractoryLining = save.refractoryLining;
            _state.hearthTuyeres = save.hearthTuyeres;
            _state.sandBeds = save.sandBeds;
            _state.structuralSupports = save.structuralSupports;
            _state.safetyExhaust = save.safetyExhaust;
            _state.maintenanceCycleDays = save.maintenanceCycleDays > 0 ? save.maintenanceCycleDays : 4;
            _state.maintenanceDueDay = save.maintenanceDueDay;
            _state.daysSinceMaintenance = save.daysSinceMaintenance;
            _state.maintenancePerformed = save.maintenancePerformed;
            _state.sandQuality = save.sandQuality;
            _state.sandMoisture = save.sandMoisture;
            _state.binderQuality = save.binderQuality;
            _state.patternQuality = save.patternQuality;
            _state.contamination = save.contamination;
            _state.moldReuseCount = save.moldReuseCount;
            _state.compaction = save.compaction;
            _state.heatStage = save.heatStage;
            _state.heatStartedDay = save.heatStartedDay;
            _state.stageElapsedDays = save.stageElapsedDays;
            _state.activeProductId = save.activeProductId;
            _state.assignedWorkers = save.assignedWorkers;
            _state.workerSkill = save.workerSkill;
            _state.laborAccumulated = save.laborAccumulated;
            _state.workerExposure = save.workerExposure;
            _state.materialsConsumed = save.materialsConsumed;
            _state.childLaborUsed = save.childLaborUsed;
            _state.pendingQuality = save.pendingQuality;
            _state.completed = save.completed ?? new List<FoundryProductionRecord>();
            _state.failed = save.failed ?? new List<FoundryFailedCastRecord>();
            _state.incidents = save.incidents ?? new List<FoundryIncidentRecord>();
            _state.repairs = save.repairs ?? new List<FoundryRepairRecord>();
            _state.laborDispute = save.laborDispute;
            _state.laborDisputeStartedDay = save.laborDisputeStartedDay;
            _state.strikeStartedDay = save.strikeStartedDay;
            _state.overtimeFlag = save.overtimeFlag;
            _state.educationConflictFlag = save.educationConflictFlag;
            _state.treatyCompliance = save.treatyCompliance ?? new List<FoundryTreatyCompliance>();
            _state.triggeredJournals = save.triggeredJournals ?? new List<string>();
            _state.cumulativeStress = save.cumulativeStress;
            _state.cumulativeHope = save.cumulativeHope;
            _state.firstHeatDay = save.firstHeatDay;
            _state.strikeDay = save.strikeDay;
            if (save.rngSeed != 0 && save.rngSeed != _rng.Seed)
                _rng = _rngFactory(save.rngSeed);
            _state.rngSeed = _rng.Seed;
            NormalizeState();
            EnsureTreatyComplianceRows();
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
