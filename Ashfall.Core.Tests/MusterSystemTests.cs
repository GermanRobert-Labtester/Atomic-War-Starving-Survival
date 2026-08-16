using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Muster;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class MusterSystemTests
    {
        private static MusterSystem NewSystem() => new MusterSystem();

        [Fact]
        public void FoundingCatalog_RegistersEightQuestlines()
        {
            var sys = NewSystem();
            Assert.NotNull(sys.FindDefinition("quest_the_muster_uprising"));
            Assert.NotNull(sys.FindDefinition("quest_the_rate_card_war"));
            Assert.NotNull(sys.FindDefinition("quest_the_unsigned_order"));
            Assert.NotNull(sys.FindDefinition("quest_four_names_on_the_roster"));
            Assert.NotNull(sys.FindDefinition("quest_the_second_winter"));
            Assert.NotNull(sys.FindDefinition("quest_the_eleven_month_circuit"));
            Assert.NotNull(sys.FindDefinition("quest_the_second_color_ledger"));
            Assert.NotNull(sys.FindDefinition("quest_nothing_to_offer"));
            Assert.Equal(8, sys.Catalog.Count);
        }

        [Fact]
        public void MusterQuestline_OffersAllFourStrategies()
        {
            var sys = NewSystem();
            var def = sys.FindDefinition("quest_the_muster_uprising");
            var keys = new List<string>();
            foreach (var a in def.approaches) keys.Add(a.approach.ToString());
            Assert.Equal(4, def.approaches.Count);
            Assert.Contains("A", keys);
            Assert.Contains("B", keys);
            Assert.Contains("C", keys);
            Assert.Contains("D", keys);
        }

        [Fact]
        public void SelectApproach_ResolvesWithEndingKey()
        {
            var sys = NewSystem();
            sys.SetEscalationDay(300);
            Assert.True(sys.SelectApproach(QuestApproach.A));
            Assert.True(sys.IsResolved);
            Assert.Equal(QuestApproach.A, sys.SelectedApproach);
            Assert.Equal("the_amnesty", sys.ResolveEndingKey());
        }

        [Fact]
        public void SelectApproach_AfterResolutionIsRejected()
        {
            var sys = NewSystem();
            Assert.True(sys.SelectApproach(QuestApproach.C));
            Assert.False(sys.SelectApproach(QuestApproach.B));
            Assert.Equal("the_corridor", sys.ResolveEndingKey());
        }

        [Fact]
        public void SelectApproach_UnOfferedApproachRejected()
        {
            // The unsigned order is registered forkless (Section III): every approach is rejected.
            var sys = NewSystem();
            Assert.False(sys.SelectApproachFor("quest_the_unsigned_order", QuestApproach.B));
            Assert.False(sys.SelectApproachFor("quest_missing", QuestApproach.A));
            Assert.False(sys.IsResolved);
            Assert.Equal(string.Empty, sys.ResolveEndingKey());
        }

        [Fact]
        public void SelectApproach_OnAnotherQuestlineResolvesIndependently()
        {
            var sys = NewSystem();
            Assert.True(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.C));
            Assert.Equal("the_administrator", sys.EndingKeyFor("quest_the_rate_card_war"));
            Assert.False(sys.IsResolved);
            Assert.True(sys.SelectApproach(QuestApproach.B));
            Assert.Equal("the_open_muster", sys.ResolveEndingKey());
        }

        [Fact]
        public void RateCardWar_OffersAllFourSpecApproaches()
        {
            var sys = NewSystem();
            var def = sys.FindDefinition("quest_the_rate_card_war");
            var keys = new List<string>();
            foreach (var a in def.approaches) keys.Add(a.approach.ToString());
            Assert.Equal(4, def.approaches.Count);
            Assert.Contains("A", keys);
            Assert.Contains("B", keys);
            Assert.Contains("C", keys);
            Assert.Contains("D", keys);
            Assert.True(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.D));
            Assert.Equal("the_rate_card_revised", sys.EndingKeyFor("quest_the_rate_card_war"));
        }

        [Fact]
        public void MusterTrigger_RequiresDay260Plus()
        {
            var sys = NewSystem();
            sys.SetEscalationDay(259);
            Assert.False(sys.MusterTriggered);
            sys.SetEscalationDay(260);
            Assert.True(sys.MusterTriggered);
            Assert.Equal(260, sys.EscalationDay);
        }

        [Fact]
        public void RegisterQuestline_NullAndDuplicateIgnored()
        {
            var sys = NewSystem();
            sys.RegisterQuestline(null);
            sys.RegisterQuestline(new MusterQuestlineDefinition());
            var dup = new MusterQuestlineDefinition { questlineId = "quest_the_unsigned_order" };
            sys.RegisterQuestline(dup);
            Assert.Equal(8, sys.Catalog.Count);
            Assert.Null(sys.FindDefinition("quest_missing"));
        }

        [Fact]
        public void CaptureState_ReturnsSnapshotNotLiveState()
        {
            var sys = NewSystem();
            sys.SetEscalationDay(320);
            sys.SelectApproach(QuestApproach.B);

            var snapshot = sys.CaptureState();
            snapshot.records[0].endingKey = "injected";

            Assert.Equal("the_open_muster", sys.ResolveEndingKey());
        }

        [Fact]
        public void CaptureState_EmitsRecordsInOrdinalOrder()
        {
            var sys = NewSystem();
            sys.SelectApproach(QuestApproach.B);
            var snapshot = sys.CaptureState();
            for (int i = 1; i < snapshot.records.Count; i++)
                Assert.True(
                    string.CompareOrdinal(snapshot.records[i - 1].questlineId, snapshot.records[i].questlineId) <= 0);
        }

        [Fact]
        public void SaveLoad_RoundTripsAllState()
        {
            var sys = NewSystem();
            sys.SetEscalationDay(310);
            sys.SelectApproach(QuestApproach.D);

            var restored = new MusterSystem();
            restored.RestoreState(sys.CaptureState());

            Assert.Equal(310, restored.EscalationDay);
            Assert.True(restored.MusterTriggered);
            Assert.True(restored.IsResolved);
            Assert.Equal(QuestApproach.D, restored.SelectedApproach);
            Assert.Equal("the_blood_price", restored.ResolveEndingKey());
            Assert.Equal("the_blood_price", restored.EndingKeyFor("quest_the_muster_uprising"));
        }

        [Fact]
        public void SaveLoad_ChecksumStable()
        {
            var sys = NewSystem();
            sys.SetEscalationDay(261);
            sys.SelectApproach(QuestApproach.A);
            var before = SaveChecksum.Compute(sys.CaptureState());

            var restored = new MusterSystem();
            restored.RestoreState(sys.CaptureState());
            var after = SaveChecksum.Compute(restored.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void OnQuestlineResolved_Fires_WhenApproachSelected()
        {
            var sys = NewSystem();
            MusterRecord emitted = null;
            sys.OnQuestlineResolved += r => emitted = r;

            Assert.True(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.A));
            Assert.NotNull(emitted);
            Assert.Equal("quest_the_rate_card_war", emitted.questlineId);
            Assert.Equal("A", emitted.selectedApproach);
            Assert.Equal("the_rate_card_revised", emitted.endingKey);
            Assert.True(emitted.resolved);
        }

        [Fact]
        public void OnQuestlineResolved_DoesNotReFire_OnRejectedApproach()
        {
            var sys = NewSystem();
            int emitCount = 0;
            sys.OnQuestlineResolved += _ => emitCount++;

            Assert.True(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.A));
            Assert.Equal(1, emitCount);

            // Subsequent selection on already resolved questline must fail and not re-emit
            Assert.False(sys.SelectApproachFor("quest_the_rate_card_war", QuestApproach.B));
            Assert.Equal(1, emitCount);
        }
    }
}
