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
}
