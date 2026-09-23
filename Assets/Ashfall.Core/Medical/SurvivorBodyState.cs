// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Medical
{
    public sealed class LimbRecord
    {
        [JsonPropertyName("condition")]
        public string Condition { get; set; } = "intact";

        [JsonPropertyName("prosthetic_item_id")]
        public string? ProstheticItemId { get; set; }

        public LimbRecord() { }

        public LimbRecord(string condition, string? prostheticItemId = null)
        {
            Condition = condition ?? "intact";
            ProstheticItemId = prostheticItemId;
        }
    }

    public sealed class RehabRecord
    {
        [JsonPropertyName("prosthetic_type_key")]
        public string ProstheticTypeKey { get; set; } = string.Empty;

        [JsonPropertyName("phase")]
        public string Phase { get; set; } = "fitting";

        [JsonPropertyName("days_in_phase")]
        public int DaysInPhase { get; set; }

        [JsonPropertyName("quality_ramp_permille")]
        public int QualityRampPermille { get; set; }

        public RehabRecord() { }

        public RehabRecord(string prostheticTypeKey, string phase, int daysInPhase, int qualityRampPermille)
        {
            ProstheticTypeKey = prostheticTypeKey ?? string.Empty;
            Phase = phase ?? "fitting";
            DaysInPhase = Math.Max(0, daysInPhase);
            QualityRampPermille = Math.Clamp(qualityRampPermille, 0, 1000);
        }
    }

    /// <summary>
    /// F14-C / UNBLOCK-01: Additive survivor body_state persistence and read model.
    /// Represents persisted limb conditions (intact, amputated, prosthetized) and rehabilitation state.
    /// Pure domain model with default intact migration for legacy saves.
    /// </summary>
    public sealed class SurvivorBodyState
    {
        public const string LeftArmKey = "left_arm";
        public const string RightArmKey = "right_arm";
        public const string LeftLegKey = "left_leg";
        public const string RightLegKey = "right_leg";

        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("limbs")]
        public Dictionary<string, LimbRecord> Limbs { get; set; } = new(StringComparer.Ordinal);

        [JsonPropertyName("rehab")]
        public RehabRecord? Rehab { get; set; }

        public SurvivorBodyState()
        {
        }

        public static SurvivorBodyState CreateDefaultIntact()
        {
            var state = new SurvivorBodyState
            {
                SchemaVersion = 1,
                Rehab = null
            };
            state.Limbs[LeftArmKey] = new LimbRecord("intact");
            state.Limbs[RightArmKey] = new LimbRecord("intact");
            state.Limbs[LeftLegKey] = new LimbRecord("intact");
            state.Limbs[RightLegKey] = new LimbRecord("intact");
            return state;
        }

        public LimbRecord GetLimb(string limbKey)
        {
            if (Limbs.TryGetValue(limbKey, out var record))
                return record;
            return new LimbRecord("intact");
        }

        public void SetLimbCondition(string limbKey, string condition, string? prostheticItemId = null)
        {
            Limbs[limbKey] = new LimbRecord(condition, prostheticItemId);
        }

        public IEnumerable<LimbState> ToLimbStates()
        {
            var list = new List<LimbState>();
            list.Add(MapToLimbState(LimbId.LeftArm, GetLimb(LeftArmKey)));
            list.Add(MapToLimbState(LimbId.RightArm, GetLimb(RightArmKey)));
            list.Add(MapToLimbState(LimbId.LeftLeg, GetLimb(LeftLegKey)));
            list.Add(MapToLimbState(LimbId.RightLeg, GetLimb(RightLegKey)));
            return list;
        }

        private static LimbState MapToLimbState(LimbId id, LimbRecord record)
        {
            var condition = record.Condition.ToLowerInvariant() switch
            {
                "amputated" => LimbCondition.Amputated,
                "prosthetized" or "prosthetic" => LimbCondition.Prosthetic,
                "bionic" => LimbCondition.Bionic,
                "wounded" => LimbCondition.Wounded,
                "infected" => LimbCondition.Infected,
                "gangrenous" => LimbCondition.Gangrenous,
                _ => LimbCondition.Intact
            };

            return new LimbState
            {
                limb = id,
                condition = condition,
                prostheticId = record.ProstheticItemId
            };
        }

        public static SurvivorBodyState FromLimbStates(IEnumerable<LimbState>? states, RehabRecord? rehab = null)
        {
            var body = CreateDefaultIntact();
            body.Rehab = rehab;
            if (states == null) return body;

            foreach (var s in states)
            {
                string key = s.limb switch
                {
                    LimbId.LeftArm => LeftArmKey,
                    LimbId.RightArm => RightArmKey,
                    LimbId.LeftLeg => LeftLegKey,
                    LimbId.RightLeg => RightLegKey,
                    _ => string.Empty
                };

                if (string.IsNullOrEmpty(key)) continue;

                string condStr = s.condition switch
                {
                    LimbCondition.Amputated => "amputated",
                    LimbCondition.Prosthetic => "prosthetized",
                    LimbCondition.Bionic => "bionic",
                    LimbCondition.Wounded => "wounded",
                    LimbCondition.Infected => "infected",
                    LimbCondition.Gangrenous => "gangrenous",
                    _ => "intact"
                };

                body.SetLimbCondition(key, condStr, s.prostheticId);
            }

            return body;
        }
    }
}
