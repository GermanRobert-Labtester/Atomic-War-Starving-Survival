// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Pure C# read-only inspection projection model.
    /// Composes authoritative gameplay mechanics from ItemDefinition with
    /// rich descriptive overlay prose from ItemDescriptionEntry.
    /// </summary>
    public sealed class ItemInspectionModel
    {
        public string ItemId { get; }
        public string DisplayName { get; }
        public string Category { get; }
        public ItemType Type { get; }
        public bool HasEnhancedDescription { get; }

        // Primary Narrative Prose
        public string BaseDescription { get; }

        // Detailed Sensory & Operational Attributes (Empty if unauthored)
        public string CurrentState { get; }
        public string VisualIndicators { get; }
        public string FunctionalDescription { get; }
        public string SensoryDetails { get; }
        public string EmotionalWeight { get; }
        public string Hazards { get; }
        public string Dependencies { get; }
        public string ContaminationStatus { get; }
        public string PreservationState { get; }
        public string MakeshiftUtility { get; }
        public string Alternatives { get; }
        public string SystemIntegration { get; }

        // Mechanics & Gameplay Stats (from ItemDefinition)
        public float Weight { get; }
        public int StackMax { get; }
        public float RadProtection { get; }
        public float Durability { get; }
        public float Contamination { get; }
        public float HungerRestore { get; }
        public float ThirstRestore { get; }
        public float HealthEffect { get; }
        public float RadCleanse { get; }
        public float MoraleEffect { get; }
        public float TradeValue { get; }
        public int TradeTier { get; }
        public bool IsEquipable { get; }
        public string EquipSlot { get; }
        public bool IsConsumable { get; }

        public ItemInspectionModel(ItemDefinition def, ItemDescriptionEntry? description = null, System.Collections.Generic.List<string>? tags = null)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));

            ItemId = def.id ?? string.Empty;
            DisplayName = string.IsNullOrWhiteSpace(def.displayName) ? ItemId : def.displayName;
            Type = def.type;
            Weight = def.weight;
            StackMax = def.stackMax;
            RadProtection = def.radProtection;
            Durability = def.durability;
            Contamination = def.contamination;
            HungerRestore = def.hungerRestore;
            ThirstRestore = def.thirstRestore;
            HealthEffect = def.healthEffect;
            RadCleanse = def.radCleanse;
            MoraleEffect = def.moraleEffect;
            TradeValue = def.tradeValue;
            TradeTier = def.tradeTier;
            IsEquipable = def.isEquipable;
            EquipSlot = def.equipSlot.ToString();
            IsConsumable = def.IsConsumable();

            if (description != null)
            {
                HasEnhancedDescription = true;
                Category = string.IsNullOrWhiteSpace(description.Category) ? def.type.ToString().ToLowerInvariant() : description.Category;
                BaseDescription = !string.IsNullOrWhiteSpace(description.BaseDescription) ? description.BaseDescription : (def.description ?? string.Empty);
                CurrentState = description.CurrentState ?? string.Empty;
                VisualIndicators = description.VisualIndicators ?? string.Empty;
                FunctionalDescription = description.FunctionalDescription ?? string.Empty;
                SensoryDetails = description.SensoryDetails ?? string.Empty;
                EmotionalWeight = description.EmotionalWeight ?? string.Empty;
                Hazards = description.Hazards ?? string.Empty;
                Dependencies = description.Dependencies ?? string.Empty;
                ContaminationStatus = description.ContaminationStatus ?? string.Empty;
                PreservationState = description.PreservationState ?? string.Empty;
                MakeshiftUtility = description.MakeshiftUtility ?? string.Empty;
                Alternatives = description.Alternatives ?? string.Empty;
                SystemIntegration = description.SystemIntegration ?? string.Empty;
            }
            else
            {
                HasEnhancedDescription = false;
                Category = def.type.ToString().ToLowerInvariant();
                BaseDescription = def.description ?? string.Empty;
                CurrentState = string.Empty;
                VisualIndicators = string.Empty;
                FunctionalDescription = string.Empty;
                SensoryDetails = string.Empty;
                EmotionalWeight = string.Empty;
                Hazards = string.Empty;
                Dependencies = string.Empty;
                ContaminationStatus = string.Empty;
                PreservationState = string.Empty;
                MakeshiftUtility = string.Empty;
                Alternatives = string.Empty;
                SystemIntegration = string.Empty;
            }

            NarrativeTags = tags != null ? tags.AsReadOnly() : (System.Collections.Generic.IReadOnlyList<string>)System.Array.Empty<string>();
            IsKeepsakeCandidate = tags != null && tags.Contains("personal_keepsake_candidate");
        }

        public bool IsKeepsakeCandidate { get; }
        public System.Collections.Generic.IReadOnlyList<string> NarrativeTags { get; }

        public static ItemInspectionModel Create(ItemDefinition def, ItemDescriptionCatalog? catalog = null)
        {
            return Create(def, catalog, null);
        }

        public static ItemInspectionModel Create(ItemDefinition def, ItemDescriptionCatalog? catalog, ExpansionEnrichmentCatalog? enrichment)
        {
            var entry = catalog?.Get(def.id);
            var tags = enrichment?.GetItemTags(def.id)?.tags;
            return new ItemInspectionModel(def, entry, tags);
        }
    }
}
