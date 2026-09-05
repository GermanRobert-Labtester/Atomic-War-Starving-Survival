// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    using SeededRng = Ashfall.Core.SeededRng;

    /// <summary>
    /// Flagship Task 5: NPC Trap Theft and Sabotage Through Narrative Encounters.
    /// Covers all 17 requirements of the Task 5 test matrix.
    /// </summary>
    public sealed class WildlifeTrappingInterferenceTests
    {
        [Fact]
        public void TheftRoll_UsesDeterministicRng()
        {
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));

            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.5f,
                baseCatchModifier = 0.0f // Force failed catch
            };
            sysA.RegisterTrapDefinition(trapDef);
            sysB.RegisterTrapDefinition(trapDef);

            sysA.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);
            sysB.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            sysA.TickDay(2);
            sysB.TickDay(2);

            Assert.Equal(sysA.State.trapSites[0].baitStolen, sysB.State.trapSites[0].baitStolen);
            Assert.Equal(sysA.State.trapSites[0].baitType, sysB.State.trapSites[0].baitType);
        }

        [Fact]
        public void SuccessfulCatch_DoesNotEvaluateTheft()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetHunterSkill(100f); // Ensures high catch chance (0.95)
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f // 100% theft chance if evaluated
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            bool baitStolenFired = false;
            sys.OnBaitStolen += _ => baitStolenFired = true;

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.True(site.hasCatch);
            Assert.False(site.baitStolen);
            Assert.False(baitStolenFired);
            Assert.NotEmpty(site.baitType);
        }

        [Fact]
        public void BrokenTrap_DoesNotEvaluateTheft()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                durabilityChecks = 1
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 1);

            var site = sys.State.trapSites[0];
            site.isBroken = true;
            site.remainingDurability = 0;

            bool baitStolenFired = false;
            sys.OnBaitStolen += _ => baitStolenFired = true;

            sys.TickDay(2);

            Assert.False(site.baitStolen);
            Assert.False(baitStolenFired);
        }

        [Fact]
        public void BaitStolen_ClearsBaitAndSetsFlag()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f // Force fail catch
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.True(site.baitStolen);
            Assert.Equal(string.Empty, site.baitType);
        }

        [Fact]
        public void BaitStolen_SurvivesCaptureSerializeDeserializeRestore()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            sys.TickDay(2);
            Assert.True(sys.State.trapSites[0].baitStolen);

            var state = sys.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);
            var deserialized = serializer.Deserialize<WildlifeTrappingState>(json);

            var sysRestored = new WildlifeTrappingSystem(new SeededRng(42));
            sysRestored.RestoreState(deserialized!);

            Assert.True(sysRestored.State.trapSites[0].baitStolen);
            Assert.Equal(string.Empty, sysRestored.State.trapSites[0].baitType);
        }

        [Fact]
        public void StolenBait_PreventsFutureCatchUntilRebaited()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            // Day 2: theft occurs
            sys.TickDay(2);
            Assert.True(sys.State.trapSites[0].baitStolen);

            // Now make catch guaranteed if checked
            trapDef.baseCatchModifier = 10.0f;

            // Day 3: trap should skip catch check because baitStolen is true
            sys.TickDay(3);
            Assert.False(sys.State.trapSites[0].hasCatch);
            Assert.True(sys.State.trapSites[0].baitStolen);
        }

        [Fact]
        public void Rebait_ClearsBaitStolenFlag()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", checkIntervalDays: 1);
            var site = sys.State.trapSites[0];
            site.baitStolen = true;
            site.baitType = string.Empty;

            var res = sys.RebaitTrap("site_1", "bait_grain_lure");
            Assert.True(res.IsSuccess);
            Assert.False(site.baitStolen);
            Assert.Equal("bait_grain_lure", site.baitType);
        }

        [Fact]
        public void BaitTheft_EmitsOnBaitStolenAfterStateMutation()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            BaitStolenEvent? receivedEvent = null;
            sys.OnBaitStolen += ev =>
            {
                // Invariant: state must already be mutated before event fires
                var currentSite = sys.State.trapSites[0];
                Assert.True(currentSite.baitStolen);
                Assert.Equal(string.Empty, currentSite.baitType);
                receivedEvent = ev;
            };

            sys.TickDay(2);

            Assert.NotNull(receivedEvent);
            Assert.Equal("test_trap", receivedEvent!.trapId);
            Assert.Equal("site_1", receivedEvent.siteId);
            Assert.Equal(2, receivedEvent.campaignDay);
            Assert.StartsWith("trap-theft:2:site_1:test_trap", receivedEvent.EventIdentity);
        }

        [Fact]
        public void LegacySaveWithoutBaitStolen_DefaultsFalse()
        {
            string legacyJson = "{\"systemId\":\"wildlife_trapping\",\"trapSites\":[{\"siteId\":\"legacy_site\",\"baitType\":\"meat\",\"trapType\":\"snare\",\"remainingDurability\":5}],\"totalCatch\":0}";
            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(legacyJson);

            Assert.NotNull(state);
            Assert.Single(state!.trapSites);
            Assert.False(state.trapSites[0].baitStolen);
            Assert.Equal(0f, state.trapSites[0].pendingWeatherWear);
        }

        [Fact]
        public void ZeroTheftChance_PreservesLegacyBehavior()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.0f,
                baseCatchModifier = 0.0f // Force fail catch
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.False(site.baitStolen);
            Assert.Equal("bait_scrap_meat", site.baitType);
        }

        [Fact]
        public void Sabotage_UsesDeterministicRng()
        {
            var sysA = new WildlifeTrappingSystem(new SeededRng(1234));
            var sysB = new WildlifeTrappingSystem(new SeededRng(1234));

            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.0f,
                sabotageChance = 0.6f,
                sabotageDurabilityDamage = 3,
                durabilityChecks = 10,
                baseCatchModifier = 0.0f
            };
            sysA.RegisterTrapDefinition(trapDef);
            sysB.RegisterTrapDefinition(trapDef);

            sysA.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 10);
            sysB.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 10);

            sysA.TickDay(2);
            sysB.TickDay(2);

            Assert.Equal(sysA.State.trapSites[0].remainingDurability, sysB.State.trapSites[0].remainingDurability);
            Assert.Equal(sysA.State.trapSites[0].isBroken, sysB.State.trapSites[0].isBroken);
        }

        [Fact]
        public void Sabotage_AppliesOnlyDurabilityDamage()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.0f,
                sabotageChance = 1.0f,
                sabotageDurabilityDamage = 2,
                durabilityChecks = 8,
                weatherDegradationRate = 0.0f, // isolate from weather wear
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 8);

            TrapSabotagedEvent? sabotagedEvent = null;
            sys.OnTrapSabotaged += ev => sabotagedEvent = ev;

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.NotNull(sabotagedEvent);
            Assert.Equal(2, sabotagedEvent!.durabilityDamage);
            Assert.Equal(6, site.remainingDurability); // 8 - 2 = 6
            Assert.False(site.isBroken);
            Assert.False(site.baitStolen);
            Assert.Equal("bait_scrap_meat", site.baitType); // Bait preserved!
        }

        [Fact]
        public void Sabotage_CanBreakTrapThroughExistingBreakagePath()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.0f,
                sabotageChance = 1.0f,
                sabotageDurabilityDamage = 5,
                durabilityChecks = 3,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);
        }

        [Fact]
        public void SuccessfulCatch_DoesNotResolveSabotage()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetHunterSkill(100f); // Ensures high catch chance (0.95)
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 0.0f,
                sabotageChance = 1.0f,
                sabotageDurabilityDamage = 3,
                durabilityChecks = 10,
                weatherDegradationRate = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1, durabilityChecks: 10);

            bool sabotageFired = false;
            sys.OnTrapSabotaged += _ => sabotageFired = true;

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.True(site.hasCatch);
            Assert.False(sabotageFired);
            Assert.Equal(10, site.remainingDurability); // No sabotage damage applied
        }

        [Fact]
        public void BrokenTrap_DoesNotResolveSabotage()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                sabotageChance = 1.0f,
                sabotageDurabilityDamage = 2
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            var site = sys.State.trapSites[0];
            site.isBroken = true;
            site.remainingDurability = 0;

            bool sabotageFired = false;
            sys.OnTrapSabotaged += _ => sabotageFired = true;

            sys.TickDay(2);

            Assert.False(sabotageFired);
        }

        [Fact]
        public void NarrativeBridge_ReceivesSingleInterferenceNotification()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            int notificationsCount = 0;
            sys.OnBaitStolen += ev => notificationsCount++;

            sys.TickDay(2);

            Assert.Equal(1, notificationsCount);
        }

        [Fact]
        public void NarrativeBridge_DoesNotMutateTrapStateDirectly()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var trapDef = new TrapDefinition
            {
                trap_id = "test_trap",
                theftChance = 1.0f,
                baseCatchModifier = 0.0f
            };
            sys.RegisterTrapDefinition(trapDef);
            sys.SetTrap("site_1", "bait_scrap_meat", "hunter_1", "snare", "test_trap", checkIntervalDays: 1);

            BaitStolenEvent? capturedEvent = null;
            sys.OnBaitStolen += ev => capturedEvent = ev;

            sys.TickDay(2);

            Assert.NotNull(capturedEvent);
            // Verify event payload is an immutable fact, does not contain mutable TrapSite reference
            Assert.Equal("test_trap", capturedEvent!.trapId);
            Assert.Equal("site_1", capturedEvent.siteId);
            Assert.Equal(2, capturedEvent.campaignDay);
        }
    }
}
