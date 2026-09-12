// SPDX-License-Identifier: MIT
using System;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Engine-agnostic data representation of a single item description entry from
    /// item_description_texts.json (authority). Contains sensory, visual, hazard,
    /// and narrative prose used for read-only item inspection.
    /// </summary>
    [Serializable]
    public sealed class ItemDescriptionEntry
    {
        [JsonPropertyName("item_id")]
        public string ItemId { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("base_description")]
        public string BaseDescription { get; set; } = string.Empty;

        [JsonPropertyName("current_state")]
        public string CurrentState { get; set; } = string.Empty;

        [JsonPropertyName("visual_indicators")]
        public string VisualIndicators { get; set; } = string.Empty;

        [JsonPropertyName("functional_description")]
        public string FunctionalDescription { get; set; } = string.Empty;

        [JsonPropertyName("sensory_details")]
        public string SensoryDetails { get; set; } = string.Empty;

        [JsonPropertyName("emotional_weight")]
        public string EmotionalWeight { get; set; } = string.Empty;

        [JsonPropertyName("hazards")]
        public string Hazards { get; set; } = string.Empty;

        [JsonPropertyName("dependencies")]
        public string Dependencies { get; set; } = string.Empty;

        [JsonPropertyName("contamination_status")]
        public string ContaminationStatus { get; set; } = string.Empty;

        [JsonPropertyName("preservation_state")]
        public string PreservationState { get; set; } = string.Empty;

        [JsonPropertyName("makeshift_utility")]
        public string MakeshiftUtility { get; set; } = string.Empty;

        [JsonPropertyName("alternatives")]
        public string Alternatives { get; set; } = string.Empty;

        [JsonPropertyName("system_integration")]
        public string SystemIntegration { get; set; } = string.Empty;

        // Legacy snake_case accessor aliases for cross-compatibility
        [JsonIgnore]
        public string item_id { get => ItemId; set => ItemId = value; }

        [JsonIgnore]
        public string category { get => Category; set => Category = value; }

        [JsonIgnore]
        public string base_description { get => BaseDescription; set => BaseDescription = value; }

        [JsonIgnore]
        public string current_state { get => CurrentState; set => CurrentState = value; }

        [JsonIgnore]
        public string visual_indicators { get => VisualIndicators; set => VisualIndicators = value; }

        [JsonIgnore]
        public string functional_description { get => FunctionalDescription; set => FunctionalDescription = value; }

        [JsonIgnore]
        public string sensory_details { get => SensoryDetails; set => SensoryDetails = value; }

        [JsonIgnore]
        public string emotional_weight { get => EmotionalWeight; set => EmotionalWeight = value; }

        [JsonIgnore]
        public string hazards { get => Hazards; set => Hazards = value; }

        [JsonIgnore]
        public string dependencies { get => Dependencies; set => Dependencies = value; }

        [JsonIgnore]
        public string contamination_status { get => ContaminationStatus; set => ContaminationStatus = value; }

        [JsonIgnore]
        public string preservation_state { get => PreservationState; set => PreservationState = value; }

        [JsonIgnore]
        public string makeshift_utility { get => MakeshiftUtility; set => MakeshiftUtility = value; }

        [JsonIgnore]
        public string alternatives { get => Alternatives; set => Alternatives = value; }

        [JsonIgnore]
        public string alternates { get => Alternatives; set => Alternatives = value; }

        [JsonIgnore]
        public string system_integration { get => SystemIntegration; set => SystemIntegration = value; }

        public void Normalize()
        {
            ItemId = ItemId ?? string.Empty;
            Category = Category ?? string.Empty;
            BaseDescription = BaseDescription ?? string.Empty;
            CurrentState = CurrentState ?? string.Empty;
            VisualIndicators = VisualIndicators ?? string.Empty;
            FunctionalDescription = FunctionalDescription ?? string.Empty;
            SensoryDetails = SensoryDetails ?? string.Empty;
            EmotionalWeight = EmotionalWeight ?? string.Empty;
            Hazards = Hazards ?? string.Empty;
            Dependencies = Dependencies ?? string.Empty;
            ContaminationStatus = ContaminationStatus ?? string.Empty;
            PreservationState = PreservationState ?? string.Empty;
            MakeshiftUtility = MakeshiftUtility ?? string.Empty;
            Alternatives = Alternatives ?? string.Empty;
            SystemIntegration = SystemIntegration ?? string.Empty;
        }
    }
}
