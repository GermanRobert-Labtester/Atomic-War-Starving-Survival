using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Task 5: UI Presentation for Collectible Discovery (Sections 5.1–5.13).
    /// Tests discovery status transitions, accessible presentation models,
    /// acknowledgement policies, and save/load round-trips for NEW and DISCOVERED states.
    /// </summary>
    public class CollectibleItemPresentationTests
    {
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        [Fact]
        public void Test1_CollectibleCard_ShowsCategory()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_family_portrait",
                category = "photograph",
                rarity = "common"
            };
            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.Equal("Photograph", model.Category);
            Assert.Contains("Photograph", model.AccessibleLabel);
        }

        [Fact]
        public void Test2_CollectibleCard_ShowsRarity()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_diesel_service_manual",
                category = "technical_manual",
                rarity = "rare"
            };
            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.DiscoveredAcknowledged);
            Assert.Equal("Rare", model.Rarity);
            Assert.Contains("Rare", model.AccessibleLabel);
        }

        [Fact]
        public void Test3_FirstDiscovery_ShowsTextualNew()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_unit_photograph",
                category = "photograph",
                rarity = "uncommon"
            };
            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.True(model.IsNewDiscovery);
            Assert.Equal("NEW", model.DiscoveryStateText);
            Assert.Contains("NEW", model.AccessibleLabel);
        }

        [Fact]
        public void Test4_AcknowledgedDiscovery_ShowsTextualDiscovered()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_unit_photograph",
                category = "photograph",
                rarity = "uncommon"
            };
            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.DiscoveredAcknowledged);
            Assert.False(model.IsNewDiscovery);
            Assert.Equal("DISCOVERED", model.DiscoveryStateText);
            Assert.Contains("DISCOVERED", model.AccessibleLabel);
        }

        [Fact]
        public void Test5_NonCollectibleCard_UnchangedOrNull()
        {
            var catalog = new CollectibleCatalog(new List<CollectibleDefinition>
            {
                new CollectibleDefinition { item_id = "item_collectible_family_portrait", category = "photograph" }
            });

            Assert.False(catalog.IsCollectible("scrap_metal"));
            Assert.Null(catalog.GetByItemId("scrap_metal"));
        }

        [Fact]
        public void Test6_EffectBearingItem_ShowsCorrectHint()
        {
            var defKnowledge = new CollectibleDefinition
            {
                item_id = "item_collectible_field_medicine_handbook",
                effect_type = "knowledge",
                effect_target = "knowledge_field_medicine"
            };
            var modelKnowledge = new CollectiblePresentationModel(defKnowledge, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.True(modelKnowledge.HasEffectBenefit);
            Assert.Contains("Reveals research knowledge", modelKnowledge.EffectIntentText);

            var defJournal = new CollectibleDefinition
            {
                item_id = "item_collectible_casualty_list",
                effect_type = "journal_unlock",
                effect_target = "journal_casualty_records"
            };
            var modelJournal = new CollectiblePresentationModel(defJournal, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.True(modelJournal.HasEffectBenefit);
            Assert.Contains("Unlocks journal entry", modelJournal.EffectIntentText);

            var defFaction = new CollectibleDefinition
            {
                item_id = "item_collectible_unit_photograph",
                effect_type = "faction_info",
                effect_target = "faction_military_history"
            };
            var modelFaction = new CollectiblePresentationModel(defFaction, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.True(modelFaction.HasEffectBenefit);
            Assert.Contains("Reveals faction intelligence", modelFaction.EffectIntentText);

            var defLocation = new CollectibleDefinition
            {
                item_id = "item_collectible_road_map",
                effect_type = "location_clue",
                effect_target = "loc_road_junction_cache"
            };
            var modelLocation = new CollectiblePresentationModel(defLocation, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.True(modelLocation.HasEffectBenefit);
            Assert.Contains("Reveals map location", modelLocation.EffectIntentText);
        }

        [Fact]
        public void Test7_NoEffectItem_ShowsNoFabricatedBenefit()
        {
            var defNone = new CollectibleDefinition
            {
                item_id = "item_collectible_vinyl_chamber_record",
                effect_type = "none"
            };
            var modelNone = new CollectiblePresentationModel(defNone, CollectibleDiscoveryStatus.NewUnacknowledged);
            Assert.False(modelNone.HasEffectBenefit);
            Assert.Contains("Archive record", modelNone.EffectIntentText);
            Assert.DoesNotContain("Reveals", modelNone.EffectIntentText);
            Assert.DoesNotContain("Grants", modelNone.EffectIntentText);
        }

        [Fact]
        public void Test8_Acknowledgement_ChangesStatusExactlyOnce()
        {
            var state = new CollectibleDiscoveryState();
            const string id = "item_collectible_family_portrait";

            // 1. Initial undiscovered
            Assert.Equal(CollectibleDiscoveryStatus.Undiscovered, state.GetDiscoveryStatus(id));

            // 2. Acquired -> NewUnacknowledged
            bool marked = state.MarkDiscovered(id);
            Assert.True(marked);
            Assert.True(state.WasEverAcquired(id));
            Assert.True(state.IsUnacknowledged(id));
            Assert.False(state.IsAcknowledged(id));
            Assert.Equal(CollectibleDiscoveryStatus.NewUnacknowledged, state.GetDiscoveryStatus(id));

            // 3. First acknowledgement succeeds
            bool ack1 = state.AcknowledgeDiscovery(id);
            Assert.True(ack1);
            Assert.False(state.IsUnacknowledged(id));
            Assert.True(state.IsAcknowledged(id));
            Assert.Equal(CollectibleDiscoveryStatus.DiscoveredAcknowledged, state.GetDiscoveryStatus(id));

            // 4. Second acknowledgement is no-op
            bool ack2 = state.AcknowledgeDiscovery(id);
            Assert.False(ack2);
            Assert.Equal(CollectibleDiscoveryStatus.DiscoveredAcknowledged, state.GetDiscoveryStatus(id));
        }

        [Fact]
        public void Test9_SaveLoad_PreservesNewState()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_family_portrait");
            Assert.Equal(CollectibleDiscoveryStatus.NewUnacknowledged, state.GetDiscoveryStatus("item_collectible_family_portrait"));

            var capture = state.CaptureState();
            string json = Serializer.Serialize(capture);

            var restoredSave = Serializer.Deserialize<CollectibleDiscoverySave>(json);
            var restoredState = new CollectibleDiscoveryState();
            restoredState.RestoreState(restoredSave);

            // Must still be NewUnacknowledged (Section 5.8)
            Assert.Equal(CollectibleDiscoveryStatus.NewUnacknowledged, restoredState.GetDiscoveryStatus("item_collectible_family_portrait"));
            Assert.True(restoredState.IsUnacknowledged("item_collectible_family_portrait"));
            Assert.False(restoredState.IsAcknowledged("item_collectible_family_portrait"));
        }

        [Fact]
        public void Test10_SaveLoad_PreservesDiscoveredState()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_family_portrait");
            state.AcknowledgeDiscovery("item_collectible_family_portrait");
            Assert.Equal(CollectibleDiscoveryStatus.DiscoveredAcknowledged, state.GetDiscoveryStatus("item_collectible_family_portrait"));

            var capture = state.CaptureState();
            string json = Serializer.Serialize(capture);

            var restoredSave = Serializer.Deserialize<CollectibleDiscoverySave>(json);
            var restoredState = new CollectibleDiscoveryState();
            restoredState.RestoreState(restoredSave);

            // Must still be DiscoveredAcknowledged (Section 5.8)
            Assert.Equal(CollectibleDiscoveryStatus.DiscoveredAcknowledged, restoredState.GetDiscoveryStatus("item_collectible_family_portrait"));
            Assert.False(restoredState.IsUnacknowledged("item_collectible_family_portrait"));
            Assert.True(restoredState.IsAcknowledged("item_collectible_family_portrait"));
        }

        [Fact]
        public void Test11_UiReopen_DoesNotAutoAcknowledge()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_concert_poster");

            // Simulating multiple card creations or UI reopens (Section 5.7)
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_concert_poster",
                category = "poster",
                rarity = "common"
            };

            for (int i = 0; i < 5; i++)
            {
                var model = new CollectiblePresentationModel(def, state.GetDiscoveryStatus("item_collectible_concert_poster"));
                Assert.Equal("NEW", model.DiscoveryStateText);
                Assert.True(model.IsNewDiscovery);
            }

            // State in authority remains unacknowledged
            Assert.Equal(CollectibleDiscoveryStatus.NewUnacknowledged, state.GetDiscoveryStatus("item_collectible_concert_poster"));
        }

        [Fact]
        public void Test12_DuplicateNonUnique_DisplaysDiscovered()
        {
            var state = new CollectibleDiscoveryState();
            const string id = "item_collectible_civil_defense_poster";

            state.MarkDiscovered(id);
            state.AcknowledgeDiscovery(id);

            // Re-acquiring duplicate non-unique
            bool reMarked = state.MarkDiscovered(id);
            Assert.False(reMarked); // Idempotent

            var def = new CollectibleDefinition
            {
                item_id = id,
                category = "poster",
                rarity = "common"
            };

            var model = new CollectiblePresentationModel(def, state.GetDiscoveryStatus(id));
            Assert.Equal("DISCOVERED", model.DiscoveryStateText);
            Assert.False(model.IsNewDiscovery);
        }

        [Fact]
        public void Test13_LongLocalizedMetadata_DoesNotBreakLayout()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_diesel_service_manual",
                category = "technical_manual",
                rarity = "rare",
                effect_type = "knowledge",
                effect_target = "knowledge_diesel_mechanics"
            };

            string longDesc = new string('X', 500);
            string longReason = new string('Y', 200);

            var model = new CollectiblePresentationModel(
                def,
                CollectibleDiscoveryStatus.NewUnacknowledged,
                displayName: "Comprehensive Pre-War Heavy Diesel Engineering and Filtration Maintenance Manual",
                description: longDesc,
                isLocked: true,
                lockedReason: longReason);

            Assert.NotNull(model.AccessibleLabel);
            Assert.Contains(longDesc, model.AccessibleLabel);
            Assert.Contains(longReason, model.AccessibleLabel);
        }

        [Fact]
        public void Test14_Status_IsMeaningfulWithoutColor()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_air_filter_manual",
                category = "technical_manual",
                rarity = "rare"
            };

            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.NewUnacknowledged);

            // Text semantics alone must communicate Category, Rarity, and Status
            Assert.Equal("Technical Manual", model.Category);
            Assert.Equal("Rare", model.Rarity);
            Assert.Equal("NEW", model.DiscoveryStateText);
        }

        [Fact]
        public void Test15_ItemCardBindings_RemainRegressionSafe()
        {
            var def = new CollectibleDefinition
            {
                item_id = "item_collectible_pre_war_novel",
                category = "book",
                rarity = "common",
                effect_type = "morale",
                effect_value = 2f
            };

            var model = new CollectiblePresentationModel(def, CollectibleDiscoveryStatus.DiscoveredAcknowledged);
            Assert.Equal("item_collectible_pre_war_novel", model.ItemId);
            Assert.Equal("Pre War Novel", model.DisplayName);
            Assert.StartsWith("Pre War Novel. Book. Common. DISCOVERED.", model.AccessibleLabel);
        }
    }
}
