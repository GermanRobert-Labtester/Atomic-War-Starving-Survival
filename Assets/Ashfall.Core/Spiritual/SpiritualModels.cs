using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Spiritual
{
    // ── Ritual & Superstition Data Models ─────────────────────────

    [Serializable]
    public sealed class SpiritualRitualDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty; // "ritual" | "superstition" | "folklore_comfort"

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("context_trigger")]
        public string ContextTrigger { get; set; } = string.Empty; // "expedition_departure" | "mealtime" | "birthday" | "blackout" | "return_muster" | "machine_maintenance"

        [JsonPropertyName("morale_delta")]
        public float MoraleDelta { get; set; } // capped, small positive or negative (e.g. +1.5f to +4f)

        [JsonPropertyName("friction_flag")]
        public string FrictionFlag { get; set; } = string.Empty; // set when superstition collides with operational needs

        [JsonPropertyName("is_optional")]
        public bool IsOptional { get; set; } = true;

        [JsonPropertyName("cooldown_days")]
        public int CooldownDays { get; set; } = 3;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    // ── Memorial & Funeral Rite Models ────────────────────────────

    [Serializable]
    public sealed class MemorialRiteDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("rite_type")]
        public string RiteType { get; set; } = string.Empty; // "roll_call" | "empty_bunk" | "division_of_effects" | "work_gang_farewell" | "wall_engraving" | "last_wish_committal"

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;

        [JsonPropertyName("grief_reduction_multiplier")]
        public float GriefReductionMultiplier { get; set; } = 0.8f; // soft partial mitigation (e.g. 0.8x grief scale)

        [JsonPropertyName("guilt_relief_amount")]
        public float GuiltReliefAmount { get; set; } = 0.15f; // small relief for participating survivors

        [JsonPropertyName("requires_recovered_body")]
        public bool RequiresRecoveredBody { get; set; } = false;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    // ── Belief Movement Models ───────────────────────────────────

    [Serializable]
    public sealed class BeliefMovementDefinition
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("latin_name")]
        public string LatinName { get; set; } = string.Empty;

        [JsonPropertyName("creed")]
        public string Creed { get; set; } = string.Empty;

        [JsonPropertyName("comfort_themes")]
        public List<string> ComfortThemes { get; set; } = new List<string>();

        [JsonPropertyName("blind_spot_themes")]
        public List<string> BlindSpotThemes { get; set; } = new List<string>();

        [JsonPropertyName("key_practices")]
        public List<string> KeyPractices { get; set; } = new List<string>();

        [JsonPropertyName("conflict_profiles")]
        public List<string> ConflictProfiles { get; set; } = new List<string>();

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();
    }

    // ── Staged Mourning Record ───────────────────────────────────

    [Serializable]
    public sealed class MourningArcRecord
    {
        public string DeceasedId { get; set; } = string.Empty;
        public int DeathDay { get; set; }
        public int CurrentStage { get; set; } // 1: Acute Shock, 2: Empty Shift, 3: Return of Ordinary, 4: Memorial Observance, 5: Long-Tail Echo
        public string PerformedRiteId { get; set; } = string.Empty;
        public bool RiteCompleted { get; set; }
        public bool RiteSkipped { get; set; }
        public int LastUpdateDay { get; set; }
    }
}
