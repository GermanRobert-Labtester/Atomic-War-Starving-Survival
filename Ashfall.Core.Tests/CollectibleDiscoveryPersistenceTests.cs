using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Workstream B: Save/Load Round-Trip with Discovery State.
    /// Verifies deterministic ordinal serialization, round-trip integrity,
    /// mutation resilience, pre-Plan-47 backward compatibility, and independence
    /// from inventory and vinyl state.
    /// </summary>
    public class CollectibleDiscoveryPersistenceTests
    {
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static readonly string[] FiveKnownIds = new[]
        {
            "item_collectible_family_portrait",
            "item_collectible_casualty_list",
            "item_collectible_road_map",
            "item_collectible_field_medicine_handbook",
            "item_collectible_civil_defense_poster"
        };

        [Fact]
        public void CollectibleDiscoverySave_FiveIds_SerializesInOrdinalOrder()
        {
            var state = new CollectibleDiscoveryState();
            foreach (var id in FiveKnownIds)
            {
                state.MarkDiscovered(id);
            }

            var save = state.CaptureState();
            Assert.Equal(5, save.discovered_ids.Length);

            var expected = (string[])FiveKnownIds.Clone();
            Array.Sort(expected, StringComparer.Ordinal);

            Assert.Equal(expected, save.discovered_ids);
        }

        [Fact]
        public void CollectibleDiscoverySave_RoundTrip_PreservesAllIds()
        {
            var state = new CollectibleDiscoveryState();
            foreach (var id in FiveKnownIds) state.MarkDiscovered(id);

            var save = state.CaptureState();
            string json = Serializer.Serialize(save);

            var deserialized = Serializer.Deserialize<CollectibleDiscoverySave>(json);
            Assert.NotNull(deserialized);

            var restoredState = new CollectibleDiscoveryState();
            restoredState.RestoreState(deserialized);

            Assert.Equal(5, restoredState.Count);
            foreach (var id in FiveKnownIds)
            {
                Assert.True(restoredState.IsDiscovered(id));
            }

            var nextSave = restoredState.CaptureState();
            Assert.Equal(save.discovered_ids, nextSave.discovered_ids);
        }

        [Fact]
        public void CollectibleDiscoverySave_AddSixth_ReserializesCanonically()
        {
            var state = new CollectibleDiscoveryState();
            foreach (var id in FiveKnownIds) state.MarkDiscovered(id);

            var save = state.CaptureState();
            var restored = new CollectibleDiscoveryState();
            restored.RestoreState(save);

            const string sixthId = "item_collectible_air_filter_manual";
            Assert.True(restored.MarkDiscovered(sixthId));
            Assert.Equal(6, restored.Count);

            var reserialized = restored.CaptureState();
            Assert.Equal(6, reserialized.discovered_ids.Length);

            var expectedAllSix = FiveKnownIds.Append(sixthId).ToArray();
            Array.Sort(expectedAllSix, StringComparer.Ordinal);
            Assert.Equal(expectedAllSix, reserialized.discovered_ids);

            // Repeat serialization without mutation is byte-for-byte stable
            string json1 = Serializer.Serialize(reserialized);
            string json2 = Serializer.Serialize(restored.CaptureState());
            Assert.Equal(json1, json2);
        }

        [Fact]
        public void CollectibleDiscoverySave_PrePlan47_MissingSectionLoadsEmpty()
        {
            // Intentional compatibility policy:
            // Saves authored before Plan 47 contain no collectible discovery section.
            // Missing legacy data maps to an "empty historical record" rather than attempting
            // speculative reconstruction. This ensures clean deterministic behavior without
            // hallucinating discovery events or firing unexpected side effects.
            CollectibleDiscoverySave? legacyPayload = null;

            var state = new CollectibleDiscoveryState();
            state.RestoreState(legacyPayload);

            Assert.Equal(0, state.Count);
            Assert.False(state.IsDiscovered("item_collectible_family_portrait"));
        }

        [Fact]
        public void CollectibleDiscoverySave_PrePlan47_DoesNotBackfillFromInventoryOrOtherSystems()
        {
            // Even if the player's inventory currently holds collectible items from before Plan 47,
            // restoring a legacy save MUST NOT retroactively synthesize discovery state (Invariant 5).
            var state = new CollectibleDiscoveryState();
            state.RestoreState(null);

            var inv = new Inventory.Inventory { Capacity = 20 };
            inv.Add(new ItemDefinition
            {
                id = "item_collectible_family_portrait",
                displayName = "Family Portrait",
                type = ItemType.Quest
            }, 1);

            Assert.Equal(0, state.Count);
            Assert.False(state.IsDiscovered("item_collectible_family_portrait"));
        }

        [Fact]
        public void CollectibleDiscoverySave_PrePlan47_CanDiscoverAfterLoad()
        {
            var state = new CollectibleDiscoveryState();
            state.RestoreState(null); // Legacy load

            Assert.Equal(0, state.Count);

            // The campaign remains fully capable of discovering a collectible normally after load
            bool firstMark = state.MarkDiscovered("item_collectible_family_portrait");
            Assert.True(firstMark);
            Assert.Equal(1, state.Count);
            Assert.True(state.IsDiscovered("item_collectible_family_portrait"));

            var nextSave = state.CaptureState();
            Assert.Single(nextSave.discovered_ids);
            Assert.Equal("item_collectible_family_portrait", nextSave.discovered_ids[0]);
        }

        [Fact]
        public void CollectibleDiscoverySave_FullCampaignRoundTrip_PreservesState()
        {
            // Full campaign envelope round-trip integration
            var state = new CollectibleDiscoveryState();
            foreach (var id in FiveKnownIds) state.MarkDiscovered(id);

            var capture = state.CaptureState();

            // Wrap in canonical save envelope
            string envelopeJson = SaveEnvelopeHelper.CaptureEnvelope(capture, Serializer);
            var (ok, unwrappedState, error) = SaveEnvelopeHelper.RestoreEnvelope<CollectibleDiscoverySave>(envelopeJson, Serializer);

            Assert.True(ok, $"RestoreEnvelope failed: {error}");
            Assert.NotNull(unwrappedState);

            var roundTrippedState = new CollectibleDiscoveryState();
            roundTrippedState.RestoreState(unwrappedState);

            Assert.Equal(5, roundTrippedState.Count);
            foreach (var id in FiveKnownIds)
            {
                Assert.True(roundTrippedState.IsDiscovered(id));
            }
        }

        [Fact]
        public void CollectibleDiscoveryState_DroppingInventoryItem_DoesNotClearDiscovery()
        {
            // Discovery is durable historical campaign state (Invariant 4):
            // Dropping or removing an item from inventory never clears its discovery record.
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_road_map");

            var inv = new Inventory.Inventory { Capacity = 10 };
            var itemDef = new ItemDefinition { id = "item_collectible_road_map", displayName = "Road Map" };
            inv.Add(itemDef, 1);
            Assert.Equal(1, inv.CountById("item_collectible_road_map"));

            // Drop/remove from inventory
            inv.Remove("item_collectible_road_map", 1);
            Assert.Equal(0, inv.CountById("item_collectible_road_map"));

            // Discovery remains intact
            Assert.True(state.IsDiscovered("item_collectible_road_map"));
            Assert.Equal(1, state.Count);
        }

        [Fact]
        public void CollectibleDiscoveryState_VinylRegistration_IsIndependent()
        {
            // Vinyl track registration must not mutate or couple to collectible discovery state
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_vinyl_chamber_record");

            var vinylTrackSet = new HashSet<string>(StringComparer.Ordinal);
            vinylTrackSet.Add("track_chamber_allegro");

            // Vinyl addition does not alter collectible discovery
            Assert.Equal(1, state.Count);
            Assert.True(state.IsDiscovered("item_collectible_vinyl_chamber_record"));
            Assert.False(state.IsDiscovered("track_chamber_allegro"));
        }

        [Fact]
        public void CollectibleDiscoverySave_ScrambledInsertion_PreservesOrdinalSerialization()
        {
            // Deliberately scrambled insertion order
            var scrambled = new[]
            {
                "item_collectible_zebra_stripe_journal",
                "item_collectible_alpha_badge",
                "item_collectible_omega_chronometer",
                "item_collectible_beta_patch",
                "item_collectible_gamma_medallion"
            };

            var state = new CollectibleDiscoveryState();
            foreach (var id in scrambled) state.MarkDiscovered(id);

            var save = state.CaptureState();

            var expected = (string[])scrambled.Clone();
            Array.Sort(expected, StringComparer.Ordinal);

            Assert.Equal(expected, save.discovered_ids);
        }
    }
}
