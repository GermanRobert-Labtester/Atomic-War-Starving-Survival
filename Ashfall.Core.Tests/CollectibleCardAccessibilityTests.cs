using System;
using Ashfall.Core;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstream D: Accessibility Audit for Collectible UI (Sections 27–34).
    /// Enforces that collectible category, rarity, discovery state, effect intent,
    /// description, and locked reasons are conveyed in accessible text, preventing
    /// color-only or icon-only semantics.
    /// </summary>
    public class CollectibleCardAccessibilityTests
    {
        [Fact]
        public void CollectibleCard_PresentationContainsCategoryAndRarityText()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_family_portrait",
                category = "photograph",
                rarity = "common",
                effect_type = "morale",
                effect_value = 2f
            };

            var model = new CollectiblePresentationModel(def, isDiscovered: false, displayName: "Family Portrait");

            // Critical text semantics must be explicitly present in text (Section 29)
            Assert.Equal("Photograph", model.Category);
            Assert.Equal("Common", model.Rarity);
            Assert.Contains("Photograph", model.AccessibleLabel);
            Assert.Contains("Common", model.AccessibleLabel);
        }

        [Fact]
        public void CollectibleCard_NewDiscovery_RendersNewAsText()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_diesel_service_manual",
                category = "technical_manual",
                rarity = "rare",
                effect_type = "knowledge",
                effect_target = "knowledge_diesel_mechanics"
            };

            // First discovery (not yet discovered)
            var model = new CollectiblePresentationModel(def, isDiscovered: false);

            Assert.True(model.IsNewDiscovery);
            Assert.Equal("NEW", model.DiscoveryStateText);
            Assert.Contains("NEW", model.AccessibleLabel);
        }

        [Fact]
        public void CollectibleCard_KnownDiscovery_RendersDiscoveredAsText()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_diesel_service_manual",
                category = "technical_manual",
                rarity = "rare",
                effect_type = "knowledge",
                effect_target = "knowledge_diesel_mechanics"
            };

            // Already discovered
            var model = new CollectiblePresentationModel(def, isDiscovered: true);

            Assert.False(model.IsNewDiscovery);
            Assert.Equal("DISCOVERED", model.DiscoveryStateText);
            Assert.Contains("DISCOVERED", model.AccessibleLabel);
        }

        [Fact]
        public void CollectibleCard_EffectHint_IsTextual()
        {
            var defMorale = new CollectibleDefinition
            {
                item_id = "item_collectible_family_portrait",
                effect_type = "morale",
                effect_value = 2f
            };
            var modelMorale = new CollectiblePresentationModel(defMorale, isDiscovered: false);
            Assert.Contains("Grants morale (+2)", modelMorale.EffectIntentText);

            var defKnowledge = new CollectibleDefinition
            {
                item_id = "item_collectible_diesel_service_manual",
                effect_type = "knowledge",
                effect_target = "knowledge_diesel_mechanics"
            };
            var modelKnowledge = new CollectiblePresentationModel(defKnowledge, isDiscovered: false);
            Assert.Contains("Reveals research knowledge", modelKnowledge.EffectIntentText);
            Assert.Contains("knowledge_diesel_mechanics", modelKnowledge.EffectIntentText);

            var defMap = new CollectibleDefinition
            {
                item_id = "item_collectible_road_map",
                effect_type = "location_clue",
                effect_target = "loc_road_junction_cache"
            };
            var modelMap = new CollectiblePresentationModel(defMap, isDiscovered: false);
            Assert.Contains("Reveals map location", modelMap.EffectIntentText);

            var defNone = new CollectibleDefinition
            {
                item_id = "item_collectible_vinyl_chamber_record",
                effect_type = "none"
            };
            var modelNone = new CollectiblePresentationModel(defNone, isDiscovered: false);
            Assert.Contains("Archive record", modelNone.EffectIntentText);
        }

        [Fact]
        public void CollectibleCard_AccessibleNameOrTooltip_IncludesDescription()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_civil_defense_poster",
                category = "poster",
                rarity = "common",
                effect_type = "none"
            };
            const string description = "A laminated card warning against fallout dust and tainted water supplies.";

            var model = new CollectiblePresentationModel(def, isDiscovered: true, description: description);

            // Canonical semantic composition (Section 30):
            // {DisplayName}. {Category}. {Rarity}. {DiscoveryState}. {EffectHint}. {Description}.
            Assert.Contains(description, model.AccessibleLabel);
            Assert.Contains(description, model.TooltipText);
            Assert.StartsWith(model.DisplayName, model.AccessibleLabel);
        }

        [Fact]
        public void CollectibleCard_Disabled_WhenSupported_IncludesReason()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_casualty_list",
                category = "military_document",
                rarity = "unique",
                unique = true,
                effect_type = "journal_unlock",
                effect_target = "journal_casualty_records"
            };

            const string lockedReason = "Shelter archive desk damaged; requires repair.";
            var model = new CollectiblePresentationModel(
                def, isDiscovered: false, isLocked: true, lockedReason: lockedReason);

            Assert.True(model.IsLocked);
            Assert.Equal(lockedReason, model.LockedReason);
            Assert.Contains("Locked: Shelter archive desk damaged; requires repair.", model.AccessibleLabel);
            Assert.Contains(lockedReason, model.TooltipText);
        }
    }
}
