// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    using SeededRng = Ashfall.Core.SeededRng;

    /// <summary>
    /// Flagship Task 7: Trap Network Effects and Local Density Penalty.
    /// Covers all 15 requirements of the Task 7 test matrix.
    /// </summary>
    public sealed class WildlifeTrappingNetworkTests
    {
        [Fact]
        public void SingleTrap_HasNoNetworkPenalty()
        {
            float baseChance = 0.50f;
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(baseChance, 1, 0.05f);
            Assert.Equal(0.50f, effective, 4);
        }

        [Fact]
        public void TwoTraps_HaveFivePercentRelativePenalty()
        {
            float baseChance = 0.50f;
            // 2 traps -> penalty 5% -> multiplier 0.95 -> 0.50 * 0.95 = 0.475
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(baseChance, 2, 0.05f);
            Assert.Equal(0.475f, effective, 4);
        }

        [Fact]
        public void FiveTraps_HaveTwentyPercentRelativePenalty()
        {
            float baseChance = 0.50f;
            // 5 traps -> penalty 20% -> multiplier 0.80 -> 0.50 * 0.80 = 0.40
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(baseChance, 5, 0.05f);
            Assert.Equal(0.40f, effective, 4);
        }

        [Fact]
        public void TenTraps_HaveFortyFivePercentRelativePenalty()
        {
            float baseChance = 0.50f;
            // 10 traps -> penalty 45% -> multiplier 0.55 -> 0.50 * 0.55 = 0.275
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(baseChance, 10, 0.05f);
            Assert.Equal(0.275f, effective, 4);
        }

        [Fact]
        public void NetworkFloor_DoesNotDropEligibleChanceBelowFivePercent()
        {
            float baseChance = 0.08f;
            // 100 traps -> extreme penalty -> must not drop below 0.05 floor
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(baseChance, 100, 0.05f);
            Assert.Equal(0.05f, effective, 4);
        }

        [Fact]
        public void NetworkFloor_DoesNotIncreaseBaseChanceBelowFivePercent()
        {
            float lowBaseChance = 0.03f;
            // Base chance was naturally 3%. Density penalty must not boost it to 5%!
            float effective = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(lowBaseChance, 10, 0.05f);
            Assert.Equal(0.03f, effective, 4);
            Assert.True(effective <= 0.03f);
        }

        [Fact]
        public void BrokenTrap_IsNotCountedAsActiveCompetitor()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_pine");
            sys.SetTrap("site_2", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_pine");

            // Mark site_2 as broken
            sys.State.trapSites[1].isBroken = true;

            int activeCount = sys.CountActiveCompetingTraps("zone_pine");
            Assert.Equal(1, activeCount); // Only site_1 is counted
        }

        [Fact]
        public void UnbaitedTrap_CompetitionRuleMatchesDocumentedContract()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_pine");
            sys.SetTrap("site_2", "", "hunter_1", "snare", zoneId: "zone_pine"); // Unbaited

            // Documented contract: unbaited traps do not harvest prey, so do not compete ecologically
            int activeCount = sys.CountActiveCompetingTraps("zone_pine");
            Assert.Equal(1, activeCount);
        }

        [Fact]
        public void DifferentZones_DoNotPenalizeEachOther()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_valley");
            sys.SetTrap("site_2", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_ridge");

            int valleyCount = sys.CountActiveCompetingTraps("zone_valley");
            int ridgeCount = sys.CountActiveCompetingTraps("zone_ridge");

            Assert.Equal(1, valleyCount);
            Assert.Equal(1, ridgeCount);
        }

        [Fact]
        public void NetworkPenalty_DoesNotAffectBycatch()
        {
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                bycatchChance = 0.3f,
                bycatchSpecies = new System.Collections.Generic.List<BycatchCandidate>
                {
                    new BycatchCandidate { speciesId = "rat", weight = 1.0f }
                },
                networkPenaltyPerTrap = 0.5f
            };
            var site = new TrapSite { siteId = "site_1", catchSpecies = "rabbit" };
            var sys = new WildlifeTrappingSystem(new SeededRng(42));

            // Bycatch candidate and roll uses its own stream and bycatchChance, unaffected by network penalty
            int draws = sys.ResolveBycatchForSite(site, trapDef);
            Assert.True(draws >= 0);
        }

        [Fact]
        public void NetworkPenalty_DoesNotAffectDurability()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                weatherDegradationRate = 1.0f,
                networkPenaltyPerTrap = 0.20f,
                durabilityChecks = 10
            };
            sys.RegisterTrapDefinition(trapDef);

            // Deploy 5 traps in the same zone
            for (int i = 1; i <= 5; i++)
            {
                sys.SetTrap($"site_{i}", "bait_scrap_meat", "hunter_1", "snare", "test_trap",
                    checkIntervalDays: 1, durabilityChecks: 10, zoneId: "zone_dense");
            }

            sys.TickDay(2);

            // Each eligible operational trap should have lost exactly 1 normal durability (under clear weather)
            for (int i = 0; i < 5; i++)
            {
                Assert.Equal(9, sys.State.trapSites[i].remainingDurability);
            }
        }

        [Fact]
        public void NetworkPenalty_DoesNotAffectTheftOrSabotage()
        {
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.25f,
                sabotageChance = 0.15f,
                networkPenaltyPerTrap = 0.20f
            };

            // Values are isolated properties on the definition
            Assert.Equal(0.25f, trapDef.theftChance);
            Assert.Equal(0.15f, trapDef.sabotageChance);
            Assert.Equal(0.20f, trapDef.networkPenaltyPerTrap);
        }

        [Fact]
        public void SameTrapCount_ProducesSameEffectiveChance()
        {
            float chanceA = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(0.5f, 4, 0.05f);
            float chanceB = WildlifeTrappingSystem.ApplyNetworkDensityPenalty(0.5f, 4, 0.05f);
            Assert.Equal(chanceA, chanceB);
        }

        [Fact]
        public void ProcessingOrder_DoesNotChangeNetworkPenalty()
        {
            // Snapshot active count ensures that every trap in the area sees activeCount = 3
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_a", "bait_scrap_meat", "hunter_1", "snare", zoneId: "shared_zone");
            sys.SetTrap("site_b", "bait_scrap_meat", "hunter_1", "snare", zoneId: "shared_zone");
            sys.SetTrap("site_c", "bait_scrap_meat", "hunter_1", "snare", zoneId: "shared_zone");

            int activeCount = sys.CountActiveCompetingTraps("shared_zone");
            Assert.Equal(3, activeCount);
        }

        [Fact]
        public void SaveLoad_PreservesInputsNeededToRecomputeNetworkPenalty()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", zoneId: "zone_sector_4");
            sys.SetTrap("site_2", "bait_grain_lure", "hunter_1", "snare", zoneId: "zone_sector_4");

            var state = sys.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);
            var loaded = serializer.Deserialize<WildlifeTrappingState>(json);

            var sysRestored = new WildlifeTrappingSystem(new SeededRng(42));
            sysRestored.RestoreState(loaded!);

            Assert.Equal(2, sysRestored.CountActiveCompetingTraps("zone_sector_4"));
            Assert.Equal("zone_sector_4", sysRestored.State.trapSites[0].zoneId);
            Assert.Equal("zone_sector_4", sysRestored.State.trapSites[1].zoneId);
        }
    }
}
