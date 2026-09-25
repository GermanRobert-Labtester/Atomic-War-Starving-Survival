// SPDX-License-Identifier: MIT
// ============================================================================
// CORE-MECH W11 core — One-shot trigger primitive (focused suite; alone first).
//
// Plan: docs/plans/CORE_GAME_MECHANICS_GAP_SEAL_MASTER_INTEGRATION_PLAN.md
//       wave W11-ONE-SHOT-TRIGGER-PRIMITIVE (built early, in W8, to clear the
//       W8→W11 ordering dependency).
// ============================================================================

using Ashfall.Core.Flags;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class OneShotTriggerLedgerTests
    {
        private static CampaignConsequenceLedger FreshLedger() => new CampaignConsequenceLedger();

        [Fact]
        public void FiresExactlyOnce()
        {
            var ledger = FreshLedger();
            var triggers = new OneShotTriggerLedger(ledger);
            triggers.Arm("gossip.choice_a", dayThreshold: 10);

            Assert.True(triggers.TryFire("gossip.choice_a", 10));
            Assert.False(triggers.TryFire("gossip.choice_a", 10)); // same tick
            Assert.False(triggers.TryFire("gossip.choice_a", 11)); // later
            Assert.True(triggers.HasFired("gossip.choice_a"));
        }

        [Fact]
        public void RespectsDayThreshold()
        {
            var triggers = new OneShotTriggerLedger(FreshLedger());
            triggers.Arm("t", dayThreshold: 20);
            Assert.False(triggers.TryFire("t", 19));
            Assert.True(triggers.TryFire("t", 20));
        }

        [Fact]
        public void RespectsGateFlag()
        {
            var ledger = FreshLedger();
            var triggers = new OneShotTriggerLedger(ledger);
            triggers.Arm("t", dayThreshold: 1, gateFlagId: "gate.open");

            Assert.False(triggers.TryFire("t", 5));
            ledger.Set("gate.open", "test", "", 1, "");
            Assert.True(triggers.TryFire("t", 5));
        }

        [Fact]
        public void UnarmedOrUnknown_NeverFires()
        {
            var triggers = new OneShotTriggerLedger(FreshLedger());
            Assert.False(triggers.TryFire("never_armed", 99));
        }

        [Fact]
        public void ReArmIsExplicit_AndAllowsExactlyOneMore()
        {
            var triggers = new OneShotTriggerLedger(FreshLedger());
            triggers.Arm("t", 1);
            Assert.True(triggers.TryFire("t", 2));

            // Arming again does NOT resurrect a fired trigger.
            triggers.Arm("t", 1);
            Assert.False(triggers.TryFire("t", 3));

            triggers.ReArm("t", dayThreshold: 4);
            Assert.False(triggers.TryFire("t", 3));
            Assert.True(triggers.TryFire("t", 4));
            Assert.False(triggers.TryFire("t", 5));
        }

        [Fact]
        public void HistorySurvivesSaveAndLoad()
        {
            // The primitive persists through the flag ledger, so a reloaded
            // campaign cannot replay a one-shot effect.
            var ledger = FreshLedger();
            var before = new OneShotTriggerLedger(ledger);
            before.Arm("seed.choice_b", 7);
            before.TryFire("seed.choice_b", 7);

            // A fresh primitive over the SAME ledger (i.e. after a reload).
            var after = new OneShotTriggerLedger(ledger);
            Assert.True(after.HasFired("seed.choice_b"));
            Assert.False(after.TryFire("seed.choice_b", 8));
        }

        [Fact]
        public void Census_IsOrdinalStable()
        {
            var triggers = new OneShotTriggerLedger(FreshLedger());
            triggers.Arm("zulu", 1);
            triggers.Arm("alpha", 1);
            triggers.Arm("mike", 1);

            var ids = new System.Collections.Generic.List<string>();
            foreach (var s in triggers.Census()) ids.Add(s.TriggerId);
            Assert.Equal(new[] { "alpha", "mike", "zulu" }, ids);
        }
    }
}
