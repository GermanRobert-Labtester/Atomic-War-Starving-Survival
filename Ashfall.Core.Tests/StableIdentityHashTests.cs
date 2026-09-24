// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Ashfall.Core.UI;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Cross-process contract for value objects whose hashes can participate in
    /// host collections or persisted diagnostic maps. Runtime-randomized string
    /// hashes make equivalent state appear different after a restart.
    /// </summary>
    public sealed class StableIdentityHashTests
    {
        [Fact]
        public void CanonicalIdentityWrappers_UseStableHash()
        {
            Assert.Equal(StableHash.Of("default"), new SaveProfileId("default").GetHashCode());
            Assert.Equal(StableHash.Of("slot_1"), new SaveSlotId("slot_1").GetHashCode());
            Assert.Equal(StableHash.Of("survivor_17"), new SurvivorId("survivor_17").GetHashCode());
            Assert.Equal(StableHash.Of("affliction_radiation_sickness"), new AfflictionId("affliction_radiation_sickness").GetHashCode());
            Assert.Equal(
                StableHash.Of("survivor_17:affliction_radiation_sickness:0"),
                AfflictionEpisodeId.Create(new SurvivorId("survivor_17"), new AfflictionId("affliction_radiation_sickness")).GetHashCode());
        }

        [Fact]
        public void ImmutablePresentationAndBillValues_UseStableHash()
        {
            var item = new InventoryBillItem("scrap_metal", 2);
            var snapshot = new CrisisPresentationSnapshot
            {
                CrisisId = "crisis_blackout",
                Kind = "power",
                Severity = CrisisSeverity.Severe,
                IsActive = true,
                Title = "Blackout",
                Summary = "The shelter lights failed."
            };

            Assert.Equal(StableHash.Combine(StableHash.Of("scrap_metal"), 2), item.GetHashCode());
            Assert.Equal(
                StableHash.Combine(
                    StableHash.Combine(
                        StableHash.Combine(StableHash.Of("crisis_blackout"), "power"),
                        (int)CrisisSeverity.Severe),
                    1),
                snapshot.GetHashCode());
        }
    }
}
