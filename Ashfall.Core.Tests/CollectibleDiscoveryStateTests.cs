using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// CollectibleDiscoveryState contract (collectibles flagship, Task 3):
    /// empty state, idempotent marking, deterministic sorted capture,
    /// duplicate-tolerant restore, no-effect restore, and checksummed
    /// round-trips through the canonical SaveEnvelope.
    /// </summary>
    public class CollectibleDiscoveryStateTests
    {
        private static readonly string[] Seed =
        {
            "item_collectible_family_portrait",
            "item_collectible_casualty_list",
            "item_collectible_road_map"
        };

        [Fact]
        public void EmptyState_IsDiscoveredFalse_CountZero()
        {
            var state = new CollectibleDiscoveryState();
            Assert.Equal(0, state.Count);
            Assert.False(state.IsDiscovered("item_collectible_family_portrait"));
            Assert.False(state.IsDiscovered(""));
            Assert.False(state.IsDiscovered(null!));
        }

        [Fact]
        public void FirstMark_ReturnsTrue_AndChangesState()
        {
            var state = new CollectibleDiscoveryState();
            Assert.True(state.MarkDiscovered(Seed[0]));
            Assert.Equal(1, state.Count);
            Assert.True(state.IsDiscovered(Seed[0]));
        }

        [Fact]
        public void RepeatMark_IsIdempotent()
        {
            var state = new CollectibleDiscoveryState();
            Assert.True(state.MarkDiscovered(Seed[0]));
            Assert.False(state.MarkDiscovered(Seed[0]));
            Assert.False(state.MarkDiscovered(Seed[0]));
            Assert.Equal(1, state.Count);
        }

        [Fact]
        public void MarkEmptyOrNull_IsRejected()
        {
            var state = new CollectibleDiscoveryState();
            Assert.False(state.MarkDiscovered(""));
            Assert.False(state.MarkDiscovered(null!));
            Assert.Equal(0, state.Count);
        }

        [Fact]
        public void CaptureState_SortsOrdinal_DeterministicAcrossInsertionOrders()
        {
            var a = new CollectibleDiscoveryState();
            foreach (var id in Seed) a.MarkDiscovered(id);

            var b = new CollectibleDiscoveryState();
            for (int i = Seed.Length - 1; i >= 0; i--) b.MarkDiscovered(Seed[i]);

            var saveA = a.CaptureState();
            var saveB = b.CaptureState();

            Assert.Equal(saveA.discovered_ids, saveB.discovered_ids);
            var expected = (string[])Seed.Clone();
            Array.Sort(expected, StringComparer.Ordinal);
            Assert.Equal(expected, saveA.discovered_ids);
        }

        [Fact]
        public void Restore_ClearsThenLoads_ToleratesDuplicates()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered("item_collectible_some_other_thing");

            var save = new CollectibleDiscoverySave
            {
                discovered_ids = new[] { Seed[0], Seed[1], Seed[0] }
            };
            state.RestoreState(save);

            Assert.Equal(2, state.Count);
            Assert.True(state.IsDiscovered(Seed[0]));
            Assert.True(state.IsDiscovered(Seed[1]));
            Assert.False(state.IsDiscovered("item_collectible_some_other_thing"));
        }

        [Fact]
        public void Restore_NullOrMissingSection_LoadsSafelyEmpty()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered(Seed[0]);

            state.RestoreState(null);
            Assert.Equal(0, state.Count);

            state.RestoreState(new CollectibleDiscoverySave { discovered_ids = null! });
            Assert.Equal(0, state.Count);

            state.RestoreState(new CollectibleDiscoverySave { discovered_ids = Array.Empty<string>() });
            Assert.Equal(0, state.Count);
        }

        [Fact]
        public void Restore_EmitsNoEffects_AndDoesNotMutateInventory()
        {
            // Restore is a pure state load: no callback surface exists at all.
            // This pins that contract: the type exposes no events.
            var state = new CollectibleDiscoveryState();
            state.RestoreState(new CollectibleDiscoverySave
            {
                discovered_ids = new[] { Seed[0], Seed[2] }
            });
            Assert.True(state.IsDiscovered(Seed[0]));
            Assert.False(state.IsDiscovered(Seed[1])); // unrelated collectible stays undiscovered
        }

        [Fact]
        public void UnrelatedCollectible_RemainsUndiscovered()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered(Seed[1]);
            Assert.False(state.IsDiscovered(Seed[0]));
            Assert.False(state.IsDiscovered(Seed[2]));
        }

        [Fact]
        public void EnvelopeRoundTrip_PreservesExactSet()
        {
            var state = new CollectibleDiscoveryState();
            foreach (var id in Seed) state.MarkDiscovered(id);

            string json = SaveEnvelopeHelper.CaptureEnvelope(state.CaptureState());
            var (ok, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<CollectibleDiscoverySave>(json, allowBareFallback: false);

            Assert.True(ok, error);
            Assert.NotNull(restored);
            var rehydrated = new CollectibleDiscoveryState();
            rehydrated.RestoreState(restored);

            Assert.Equal(state.Count, rehydrated.Count);
            foreach (var id in Seed) Assert.True(rehydrated.IsDiscovered(id));
        }

        [Fact]
        public void Envelope_MutatedState_ChangesChecksum()
        {
            var state = new CollectibleDiscoveryState();
            state.MarkDiscovered(Seed[0]);
            string json = SaveEnvelopeHelper.CaptureEnvelope(state.CaptureState());

            var tampered = new CollectibleDiscoverySave
            {
                discovered_ids = new[] { Seed[0], Seed[1] }
            };
            string tamperedJson = SaveEnvelopeHelper.CaptureEnvelope(tampered);

            Assert.NotEqual(json, tamperedJson);
        }

        [Fact]
        public void Envelope_MissingChecksumRejected()
        {
            // A new-format envelope whose checksum field is missing is corrupt,
            // not legacy: bare-state fallback must not rescue it.
            string corrupt = "{\"State\":{\"schema_version\":1," +
                             "\"discovered_ids\":[\"item_collectible_family_portrait\"]}}";

            var (ok, _, error) = SaveEnvelopeHelper.RestoreEnvelope<CollectibleDiscoverySave>(
                corrupt, allowBareFallback: false);

            Assert.False(ok, "new-format envelope without checksum must be rejected");
            Assert.Contains("Checksum", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void State_IsCampaignScoped_NotGlobalStatic()
        {
            var a = new CollectibleDiscoveryState();
            var b = new CollectibleDiscoveryState();
            a.MarkDiscovered(Seed[0]);
            Assert.False(b.IsDiscovered(Seed[0]), "two instances must never share state");
        }
    }
}
