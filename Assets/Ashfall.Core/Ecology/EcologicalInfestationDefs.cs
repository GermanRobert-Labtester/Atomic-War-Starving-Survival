using System;
using System.Collections.Generic;

namespace Ashfall.Core.Ecology
{
    /// <summary>Infestation lifecycle statuses. Terminal states never re-trigger.</summary>
    public enum EcologicalInfestationStatus
    {
        Inactive = 0,
        Active = 1,
        /// <summary>Tolerated: kept for its bounded resource at an ongoing cost.</summary>
        ToleratedHarvesting = 2,
        /// <summary>Terminal state after a successful clear.</summary>
        ResolvedCleared = 3
    }

    /// <summary>One authored clear option (grounded item cost + seeded success).</summary>
    [Serializable]
    public sealed class InfestationClearOption
    {
        public string option_id = string.Empty;
        public string display_name = string.Empty;
        public string required_item_id = string.Empty;
        public int required_item_count;
        /// <summary>0..1 — deterministic roll on the caller's day fork.</summary>
        public float success_chance = 0.5f;
        public string outcome_summary = string.Empty;
        /// <summary>0..1 — on failure, chance the attempt costs one extra of the required item.</summary>
        public float failure_backlash_chance;
    }

    /// <summary>
    /// One authored infestation (data authority: ecological_infestations.json).
    /// Effects route through owning systems via host callbacks — this class
    /// never touches inventory, disease, or market state directly.
    /// </summary>
    [Serializable]
    public sealed class EcologicalInfestationDefinition
    {
        public string id = string.Empty;
        public string name = string.Empty;
        /// <summary>"location" (loc_* target) or "shelter" (room_ target).</summary>
        public string scope = "location";
        public string target_id = string.Empty;
        /// <summary>Empty = eligible any season; otherwise Plan 19 window ids.</summary>
        public List<string> eligible_seasons = new List<string>();
        /// <summary>Daily trigger chance while the host finds the site eligible (0 = host-triggered only).</summary>
        public float trigger_chance_per_day;
        /// <summary>Optional precondition the host evaluates (e.g. "grain_stores", "low_filtration").</summary>
        public string requires_state = string.Empty;
        public string trigger_summary = string.Empty;
        public string hazard_summary = string.Empty;
        /// <summary>Bounded shelter food loss per day while active (inventory authority converts).</summary>
        public int food_loss_per_day;
        /// <summary>Daily disease-risk roll while tolerated (spore exposure).</summary>
        public float tolerated_hazard_risk;
        /// <summary>Plan 09 pathogen seeded through the disease port when the hazard rolls true.</summary>
        public string linked_disease_id = string.Empty;
        /// <summary>Optional non-combat benefit while tolerated (bounded).</summary>
        public string leave_resource_item_id = string.Empty;
        public int leave_resource_amount;
        public string leave_benefit_summary = string.Empty;
        /// <summary>Maximum tolerated harvests before the colony is exhausted (bounded benefit).</summary>
        public int max_harvests = 1;
        public List<InfestationClearOption> clear_options = new List<InfestationClearOption>();
    }

    [Serializable]
    public sealed class EcologicalInfestationRecord
    {
        public string infestation_id = string.Empty;
        public int status = (int)EcologicalInfestationStatus.Inactive;
        public int triggered_day = -1;
        public int last_action_day = -1;
        public int harvests_taken;
        public int failed_clear_count;
        public int trigger_roll_count;
    }

    [Serializable]
    public sealed class EcologicalInfestationState
    {
        public int schema_version = 1;
        public string systemId = EcologicalInfestationSystem.SystemId;
        public List<EcologicalInfestationRecord> records = new List<EcologicalInfestationRecord>();
        public long rollCount;
    }
}
