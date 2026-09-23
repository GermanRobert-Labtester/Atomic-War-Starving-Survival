// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class NpcMemorySystemTests
    {
        [Fact]
        public void Unknown_npc_evaluates_to_neutral()
        {
            var sys = new NpcMemorySystem();
            var rel = sys.GetOrCreate("npc_unknown");

            Assert.Equal(0f, rel.PersonalTrust);
            Assert.Equal(0f, rel.GrudgeLevel);
            Assert.Equal(0f, rel.FavorOwed);
            Assert.Equal(1.0f, sys.GetTradePriceMultiplier("npc_unknown"));
            Assert.False(sys.IsTradeRefused("npc_unknown"));
            Assert.Equal(NpcDialogueTone.Neutral, sys.GetDialogueTone("npc_unknown"));
        }

        [Fact]
        public void Helping_and_saving_accumulates_trust_favors_and_discounts()
        {
            var sys = new NpcMemorySystem();
            sys.RecordAction("npc_cael_ormund", NpcMemoryActionType.Helped, day: 2);
            sys.RecordAction("npc_cael_ormund", NpcMemoryActionType.SavedLife, day: 5);

            var rel = sys.Get("npc_cael_ormund");
            Assert.NotNull(rel);
            Assert.Equal(50f, rel.PersonalTrust);
            Assert.Equal(25f, rel.FavorOwed);
            Assert.Equal(0f, rel.GrudgeLevel);

            // Trust >= 50 gets 15% discount
            Assert.Equal(0.85f, sys.GetTradePriceMultiplier("npc_cael_ormund"));
            Assert.Equal(NpcDialogueTone.FavorOwed, sys.GetDialogueTone("npc_cael_ormund"));
        }

        [Fact]
        public void Refusal_and_betrayal_escalate_grudges_and_trigger_embargo()
        {
            var sys = new NpcMemorySystem();
            sys.RecordAction("npc_edor_vale", NpcMemoryActionType.Refused, day: 3);
            sys.RecordAction("npc_edor_vale", NpcMemoryActionType.Betrayed, day: 4);
            sys.RecordAction("npc_edor_vale", NpcMemoryActionType.Betrayed, day: 6);

            var rel = sys.Get("npc_edor_vale");
            Assert.NotNull(rel);
            Assert.True(rel.GrudgeLevel > 75f);
            Assert.True(rel.PersonalTrust < -50f);

            // Severe grudge triggers trade refusal
            Assert.True(sys.IsTradeRefused("npc_edor_vale"));
            Assert.Equal(float.PositiveInfinity, sys.GetTradePriceMultiplier("npc_edor_vale"));
            Assert.Equal(NpcDialogueTone.Betrayed, sys.GetDialogueTone("npc_edor_vale"));
        }

        [Fact]
        public void Forgiveness_and_restitution_mitigate_grudge()
        {
            var sys = new NpcMemorySystem();
            sys.RecordAction("npc_trader", NpcMemoryActionType.Refused, day: 1);
            sys.RecordAction("npc_trader", NpcMemoryActionType.Refused, day: 2);

            var rel = sys.Get("npc_trader");
            Assert.NotNull(rel);
            Assert.Equal(20f, rel.GrudgeLevel);

            // Forgive with restitution
            bool forgiven = sys.Forgive("npc_trader", "restitution_delivered", restitutionAmount: 20f);
            Assert.True(forgiven);
            Assert.Equal(0f, rel.GrudgeLevel);
            Assert.True(rel.Memories[0].Forgiven);
        }

        [Fact]
        public void Daily_decay_gradually_softens_old_memories()
        {
            var sys = new NpcMemorySystem();
            sys.RecordAction("npc_neighbor", NpcMemoryActionType.Refused, day: 1);

            var rel = sys.Get("npc_neighbor");
            Assert.NotNull(rel);
            Assert.Equal(10f, rel.GrudgeLevel);

            sys.TickDailyDecay(currentDay: 6, decayRatePerDay: 1.0f);
            Assert.True(rel.GrudgeLevel < 10f);
        }

        [Fact]
        public void Save_load_round_trip_preserves_all_memories()
        {
            var sys1 = new NpcMemorySystem();
            sys1.RecordAction("npc_alpha", NpcMemoryActionType.SavedLife, day: 3, targetId: "bunker", intensity: 80f, "heroic", "salvage");
            sys1.RecordAction("npc_beta", NpcMemoryActionType.Refused, day: 4);
            sys1.TickDailyDecay(currentDay: 5);

            var state = sys1.CaptureState();

            var sys2 = new NpcMemorySystem();
            sys2.RestoreState(state);

            Assert.Equal(sys1.LastDecayedDay, sys2.LastDecayedDay);
            var alpha = sys2.Get("npc_alpha");
            Assert.NotNull(alpha);
            Assert.Equal(40f, alpha.PersonalTrust);
            Assert.Equal(20f, alpha.FavorOwed);
            Assert.Single(alpha.Memories);
            Assert.Equal(NpcMemoryActionType.SavedLife, alpha.Memories[0].Action);
            Assert.Contains("heroic", alpha.Memories[0].Tags);

            var beta = sys2.Get("npc_beta");
            Assert.NotNull(beta);
            Assert.True(beta.GrudgeLevel > 0f);
        }
    }
}
