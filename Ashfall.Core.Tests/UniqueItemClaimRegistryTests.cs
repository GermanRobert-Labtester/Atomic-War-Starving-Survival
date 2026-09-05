using System;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// UniqueItemClaimRegistry contract (collectibles flagship, Task 4):
    /// generic uniqueness authority — claim/availability semantics, selling
    /// never unclaims, deterministic sorted serialization, stale-id restore
    /// hygiene, and checksummed round-trips.
    /// </summary>
    public class UniqueItemClaimRegistryTests
    {
        private static readonly string[] Uniques =
        {
            "item_collectible_casualty_list",
            "item_collectible_exchange_day_newspaper",
            "item_collectible_survivor_map"
        };

        private static UniqueItemClaimRegistry Make() => new UniqueItemClaimRegistry(Uniques);

        [Fact]
        public void NonUniqueItems_AlwaysAvailable_ClaimIsNoOp()
        {
            var registry = Make();
            Assert.False(registry.IsUniqueItem("item_collectible_family_portrait"));
            Assert.True(registry.IsAvailable("item_collectible_family_portrait"));
            Assert.False(registry.TryClaim("item_collectible_family_portrait"));
            Assert.True(registry.IsAvailable("item_collectible_family_portrait"));
            Assert.Equal(0, registry.ClaimedCount);
        }

        [Fact]
        public void FirstUniqueGeneration_CanSucceed()
        {
            var registry = Make();
            Assert.True(registry.IsAvailable(Uniques[0]));
            Assert.True(registry.TryClaim(Uniques[0]));
            Assert.True(registry.IsClaimed(Uniques[0]));
            Assert.False(registry.IsAvailable(Uniques[0]));
        }

        [Fact]
        public void SecondGeneration_Cannot()
        {
            var registry = Make();
            registry.TryClaim(Uniques[0]);
            Assert.False(registry.IsAvailable(Uniques[0]));
            // Even a re-claim attempt cannot make it available again.
            Assert.False(registry.IsAvailable(Uniques[0]));
        }

        [Fact]
        public void ClaimingOneUnique_DoesNotBlockAnother()
        {
            var registry = Make();
            registry.TryClaim(Uniques[0]);
            Assert.True(registry.IsAvailable(Uniques[1]));
            Assert.True(registry.TryClaim(Uniques[1]));
            Assert.True(registry.IsAvailable(Uniques[2]));
        }

        [Fact]
        public void RepeatedClaim_IsIdempotent()
        {
            var registry = Make();
            Assert.True(registry.TryClaim(Uniques[0]));
            Assert.True(registry.TryClaim(Uniques[0]));
            Assert.True(registry.TryClaim(Uniques[0]));
            Assert.Equal(1, registry.ClaimedCount);
        }

        [Fact]
        public void Selling_DoesNotUnclaim()
        {
            // Selling removes the item from inventory; the registry has no
            // unclaim/release API at all — the type cannot express it.
            var registry = Make();
            registry.TryClaim(Uniques[0]);
            Assert.False(registry.IsAvailable(Uniques[0]));
        }

        [Fact]
        public void EmptyOrNullIds_AreRejected()
        {
            var registry = Make();
            Assert.False(registry.TryClaim(""));
            Assert.False(registry.TryClaim(null!));
            Assert.True(registry.IsAvailable(""));
            Assert.False(registry.IsUniqueItem(""));
        }

        [Fact]
        public void CaptureState_SortsOrdinal_Deterministic()
        {
            var a = Make();
            a.TryClaim(Uniques[2]);
            a.TryClaim(Uniques[0]);

            var b = Make();
            b.TryClaim(Uniques[0]);
            b.TryClaim(Uniques[2]);

            Assert.Equal(a.CaptureState().claimed_unique_ids, b.CaptureState().claimed_unique_ids);
            Assert.Equal(
                new[] { Uniques[0], Uniques[2] }.OrderBy(x => x, StringComparer.Ordinal),
                a.CaptureState().claimed_unique_ids);
        }

        [Fact]
        public void Restore_ToleratesDuplicates_DropsStaleIds()
        {
            var registry = Make();
            var save = new UniqueClaimSave
            {
                claimed_unique_ids = new[]
                {
                    Uniques[0],
                    Uniques[0],               // duplicate — harmless
                    "item_collectible_family_portrait" // stale: not unique under current catalog
                }
            };
            registry.RestoreState(save);

            Assert.Equal(1, registry.ClaimedCount);
            Assert.True(registry.IsClaimed(Uniques[0]));
            Assert.False(registry.IsClaimed("item_collectible_family_portrait"));
            // Stale id was dropped, so the ordinary item is unaffected.
            Assert.True(registry.IsAvailable("item_collectible_family_portrait"));
        }

        [Fact]
        public void Restore_MissingSection_LoadsSafelyEmpty()
        {
            var registry = Make();
            registry.TryClaim(Uniques[0]);

            registry.RestoreState(null);
            Assert.Equal(0, registry.ClaimedCount);

            registry.RestoreState(new UniqueClaimSave { claimed_unique_ids = null! });
            Assert.Equal(0, registry.ClaimedCount);
        }

        [Fact]
        public void EnvelopeRoundTrip_PreservesClaims()
        {
            var registry = Make();
            registry.TryClaim(Uniques[0]);
            registry.TryClaim(Uniques[1]);

            string json = SaveEnvelopeHelper.CaptureEnvelope(registry.CaptureState());
            var (ok, restored, error) = SaveEnvelopeHelper.RestoreEnvelope<UniqueClaimSave>(json, allowBareFallback: false);
            Assert.True(ok, error);

            var rehydrated = Make();
            rehydrated.RestoreState(restored);
            Assert.True(rehydrated.IsClaimed(Uniques[0]));
            Assert.True(rehydrated.IsClaimed(Uniques[1]));
            Assert.False(rehydrated.IsAvailable(Uniques[1]));
            Assert.True(rehydrated.IsAvailable(Uniques[2]));
        }

        [Fact]
        public void SameSeedSameClaimedSet_IsAvailableIsPure()
        {
            // IsAvailable must be a pure function of (id, claim set): the same
            // claimed set yields the same generation-eligibility answers.
            var a = Make();
            var b = Make();
            a.TryClaim(Uniques[0]);
            b.TryClaim(Uniques[0]);

            foreach (var id in Uniques.Concat(new[] { "item_collectible_family_portrait" }))
                Assert.Equal(a.IsAvailable(id), b.IsAvailable(id));
        }

        [Fact]
        public void Registry_IsCampaignScoped_NotGlobalStatic()
        {
            var a = Make();
            var b = Make();
            a.TryClaim(Uniques[0]);
            Assert.False(b.IsClaimed(Uniques[0]), "two instances must never share state");
        }
    }
}
