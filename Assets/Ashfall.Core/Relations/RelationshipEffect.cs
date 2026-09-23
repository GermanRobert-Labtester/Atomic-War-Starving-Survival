// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Relations
{
    /// <summary>
    /// Plan 44 / C2[19] - Authoring definition for a relationship tier band.
    /// </summary>
    [Serializable]
    public sealed class RelationshipBandDefinition
    {
        [JsonPropertyName("band_id")]
        public string BandId { get; set; } = string.Empty;

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; } = string.Empty;

        [JsonPropertyName("min_affinity")]
        public float MinAffinity { get; set; }

        [JsonPropertyName("max_affinity")]
        public float MaxAffinity { get; set; }

        [JsonPropertyName("working_modifier")]
        public float WorkingModifier { get; set; }

        [JsonPropertyName("morale_modifier")]
        public float MoraleModifier { get; set; }

        [JsonPropertyName("error_risk_modifier")]
        public float ErrorRiskModifier { get; set; }

        [JsonPropertyName("expedition_risk_modifier")]
        public float ExpeditionRiskModifier { get; set; }

        [JsonPropertyName("separation_modifier")]
        public float SeparationModifier { get; set; }

        [JsonPropertyName("caregiving_modifier")]
        public float CaregivingModifier { get; set; }

        [JsonPropertyName("training_modifier")]
        public float TrainingModifier { get; set; }

        [JsonPropertyName("note_key")]
        public string NoteKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Catalog container for relationship bands loaded from StreamingAssets/Data/relationship_bands.json.
    /// </summary>
    [Serializable]
    public sealed class RelationshipBandsCatalog
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("comment")]
        public string Comment { get; set; } = string.Empty;

        [JsonPropertyName("bands")]
        public List<RelationshipBandDefinition> Bands { get; set; } = new List<RelationshipBandDefinition>();
    }

    /// <summary>
    /// Canonical derived relationship consequence query result (Plan 44 / C2[19]).
    /// Consumers use this instead of querying raw affinity numbers.
    /// </summary>
    public readonly struct RelationEffect
    {
        public string BandId { get; }
        public string DisplayName { get; }
        public float WorkingModifier { get; }
        public float MoraleModifier { get; }
        public float ErrorRiskModifier { get; }
        public float ExpeditionRiskModifier { get; }
        public float SeparationModifier { get; }
        public float CaregivingModifier { get; }
        public float TrainingModifier { get; }
        public string NoteKey { get; }

        public RelationEffect(
            string bandId,
            string displayName,
            float workingModifier,
            float moraleModifier,
            float errorRiskModifier,
            float expeditionRiskModifier,
            float separationModifier,
            float caregivingModifier,
            float trainingModifier,
            string noteKey)
        {
            BandId = bandId ?? string.Empty;
            DisplayName = displayName ?? string.Empty;
            WorkingModifier = workingModifier;
            MoraleModifier = moraleModifier;
            ErrorRiskModifier = errorRiskModifier;
            ExpeditionRiskModifier = expeditionRiskModifier;
            SeparationModifier = separationModifier;
            CaregivingModifier = caregivingModifier;
            TrainingModifier = trainingModifier;
            NoteKey = noteKey ?? string.Empty;
        }

        public static RelationEffect NeutralFallback => new RelationEffect(
            "cordial",
            "Cordial",
            workingModifier: 0.0f,
            moraleModifier: 0.0f,
            errorRiskModifier: 0.0f,
            expeditionRiskModifier: 0.0f,
            separationModifier: 0.0f,
            caregivingModifier: 0.0f,
            trainingModifier: 0.0f,
            noteKey: "relations.band.cordial"
        );
    }

    /// <summary>
    /// Traceable, explainable record of a pair relationship event or band change (Plan 44B).
    /// Bounded in size to prevent unbounded save growth.
    /// </summary>
    [Serializable]
    public sealed class PairRelationHistoryEntry
    {
        [JsonPropertyName("event_id")]
        public string EventId { get; set; } = string.Empty;

        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("cause_id")]
        public string CauseId { get; set; } = string.Empty;

        [JsonPropertyName("kind")]
        public string Kind { get; set; } = string.Empty;

        [JsonPropertyName("delta")]
        public float Delta { get; set; }

        [JsonPropertyName("resulting_band")]
        public string ResultingBand { get; set; } = string.Empty;

        [JsonPropertyName("source_owner")]
        public string SourceOwner { get; set; } = string.Empty;

        [JsonPropertyName("note_key")]
        public string NoteKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Aggregated relation consequence for a team or crew (e.g. shift crew or expedition party).
    /// Computed deterministically over all pairs.
    /// </summary>
    public readonly struct TeamRelationAggregate
    {
        public int TotalPairs { get; }
        public int HostilePairsCount { get; }
        public int BondedPairsCount { get; }
        public float AverageWorkingModifier { get; }
        public float DominantRiskModifier { get; }
        public float AverageMoraleModifier { get; }
        public string DominantNoteKey { get; }

        public TeamRelationAggregate(
            int totalPairs,
            int hostilePairsCount,
            int bondedPairsCount,
            float averageWorkingModifier,
            float dominantRiskModifier,
            float averageMoraleModifier,
            string dominantNoteKey)
        {
            TotalPairs = totalPairs;
            HostilePairsCount = hostilePairsCount;
            BondedPairsCount = bondedPairsCount;
            AverageWorkingModifier = Math.Max(-0.50f, Math.Min(0.50f, averageWorkingModifier));
            DominantRiskModifier = Math.Max(-0.30f, Math.Min(0.50f, dominantRiskModifier));
            AverageMoraleModifier = Math.Max(-0.30f, Math.Min(0.30f, averageMoraleModifier));
            DominantNoteKey = dominantNoteKey ?? string.Empty;
        }

        public static TeamRelationAggregate Neutral => new TeamRelationAggregate(
            totalPairs: 0,
            hostilePairsCount: 0,
            bondedPairsCount: 0,
            averageWorkingModifier: 0f,
            dominantRiskModifier: 0f,
            averageMoraleModifier: 0f,
            dominantNoteKey: "relations.team.neutral"
        );
    }
}
