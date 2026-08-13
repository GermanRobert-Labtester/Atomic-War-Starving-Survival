using System.Collections.Generic;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Tests for the CrossingArbitrationSystem extraction (Nobody's Charter
    /// §5.1): the 3-backer rule, principled-majority honest/rigged split,
    /// overturn by 3+ counters, dead backers reverting held rulings, and the
    /// port-based save roundtrip. Backer ids are master-list ids from
    /// characters.json.
    /// </summary>
    public class CrossingArbitrationSystemTests
    {
        private const string Topic = "quest_crossing_the_standing";

        private static CrossingArbitrationSystem Fixture()
        {
            var sys = new CrossingArbitrationSystem();
            sys.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "npc_osran_kell", displayName = "Osran", principled = true },
                new BackerDef { id = "npc_mattis_cray", displayName = "Mattis", principled = true },
                new BackerDef { id = "npc_bram_ostrowski", displayName = "Bram", principled = false },
                new BackerDef { id = "npc_leva_quist", displayName = "Leva", principled = false },
                new BackerDef { id = "npc_halden_mire", displayName = "Halden", principled = true }
            });
            return sys;
        }

        [Fact]
        public void LoadBackerPoolAndLookup()
        {
            var sys = Fixture();
            Assert.Equal(5, sys.BackerPool.Count);
            Assert.Equal("Osran", sys.GetBacker("npc_osran_kell").displayName);
            Assert.Null(sys.GetBacker("npc_nobody"));
        }

        [Fact]
        public void CallStandingCreatesPendingRulingAndEvent()
        {
            var sys = Fixture();
            string calledTopic = null;
            sys.OnStandingCalled += t => calledTopic = t;

            Assert.True(sys.CallStanding(Topic, 40));
            Assert.Equal(Topic, calledTopic);
            Assert.Equal(RulingShape.Pending, sys.GetRuling(Topic).shape);
            Assert.Equal(1, sys.State.rulingsCalled);
        }

        [Fact]
        public void CallStandingRejectsEmptyTopicAndFinalRulings()
        {
            var sys = Fixture();
            Assert.False(sys.CallStanding("", 40));
            Assert.False(sys.CallStanding(null, 40));

            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.True(sys.DeclareBacker(Topic, "npc_mattis_cray"));
            Assert.True(sys.DeclareBacker(Topic, "npc_bram_ostrowski"));
            Assert.Equal(RulingShape.Honest, sys.GetRuling(Topic).shape);

            Assert.False(sys.CallStanding(Topic, 41), "no re-call once a ruling holds");
        }

        [Fact]
        public void DeclareBackerRequiresCalledStanding()
        {
            var sys = Fixture();
            Assert.False(sys.DeclareBacker(Topic, "npc_osran_kell"));
        }

        [Fact]
        public void DeclareBackerRejectsDeadAndDuplicate()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.False(sys.DeclareBacker(Topic, "npc_osran_kell"), "no double declaration");

            sys.RemoveBacker("npc_leva_quist");
            Assert.False(sys.DeclareBacker(Topic, "npc_leva_quist"), "dead backers cannot declare");
        }

        [Fact]
        public void ThreeBackersMakeRuling_PrincipledMajorityHonest()
        {
            var sys = Fixture();
            StandingRuling made = null;
            sys.OnRulingMade += r => made = r;

            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.True(sys.DeclareBacker(Topic, "npc_mattis_cray"));
            Assert.True(sys.DeclareBacker(Topic, "npc_halden_mire"));

            Assert.Equal(RulingShape.Honest, sys.GetRuling(Topic).shape);
            Assert.NotNull(made);
            Assert.True(sys.IsRulingHeld(Topic));
        }

        [Fact]
        public void ThreeBackersMakeRuling_NonPrincipledMajorityRigged()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));    // principled
            Assert.True(sys.DeclareBacker(Topic, "npc_bram_ostrowski")); // bought
            Assert.True(sys.DeclareBacker(Topic, "npc_leva_quist"));    // bought

            Assert.Equal(RulingShape.Rigged, sys.GetRuling(Topic).shape);
            Assert.False(sys.IsRulingHeld(Topic), "a rigged ruling is not a held one");
        }

        [Fact]
        public void OverturnRequiresThreeCounters()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.True(sys.DeclareBacker(Topic, "npc_mattis_cray"));
            Assert.True(sys.DeclareBacker(Topic, "npc_halden_mire"));

            Assert.False(sys.OverturnRuling(Topic, new List<string> { "npc_bram_ostrowski", "npc_leva_quist" }),
                "two counters are not enough");

            StandingRuling overturned = null;
            sys.OnRulingOverturned += r => overturned = r;
            Assert.True(sys.OverturnRuling(Topic, new List<string>
            {
                "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire"
            }));

            Assert.Equal(RulingShape.Overturned, sys.GetRuling(Topic).shape);
            Assert.Empty(sys.GetRuling(Topic).backers);
            Assert.NotNull(overturned);
            Assert.Equal(1, sys.State.rulingsOverturned);
            Assert.True(sys.IsRulingOverturned(Topic));
        }

        [Fact]
        public void OverturnOfPendingRejected()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.False(sys.OverturnRuling(Topic, new List<string>
            {
                "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire"
            }), "a pending ruling cannot be overturned");
        }

        [Fact]
        public void DeadBackerRevertsHeldRulingToPending()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.True(sys.DeclareBacker(Topic, "npc_mattis_cray"));
            Assert.True(sys.DeclareBacker(Topic, "npc_halden_mire"));
            Assert.True(sys.IsRulingHeld(Topic));

            Assert.True(sys.RemoveBacker("npc_mattis_cray"));
            Assert.True(sys.GetRuling(Topic).shape == RulingShape.Pending,
                "two living holders cannot keep the ruling");
            Assert.False(sys.IsRulingHeld(Topic));
        }

        [Fact]
        public void RemoveBackerRejectsUnknownAndDead()
        {
            var sys = Fixture();
            Assert.False(sys.RemoveBacker("npc_nobody"));
            Assert.True(sys.RemoveBacker("npc_leva_quist"));
            Assert.False(sys.RemoveBacker("npc_leva_quist"), "already dead");
        }

        [Fact]
        public void AvailableBackersExcludeCommittedAndDead()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            sys.RemoveBacker("npc_halden_mire");

            var available = sys.GetAvailableBackers(Topic);
            Assert.Equal(3, available.Count);
            Assert.DoesNotContain(available, b => b.id == "npc_osran_kell");
            Assert.DoesNotContain(available, b => b.id == "npc_halden_mire");
        }

        [Fact]
        public void SaveRoundTripPreservesRulingsAndPool()
        {
            var sys = Fixture();
            Assert.True(sys.CallStanding(Topic, 40));
            Assert.True(sys.DeclareBacker(Topic, "npc_osran_kell"));
            Assert.True(sys.DeclareBacker(Topic, "npc_mattis_cray"));
            Assert.True(sys.DeclareBacker(Topic, "npc_halden_mire"));
            Assert.True(sys.OverturnRuling(Topic, new List<string>
            {
                "npc_bram_ostrowski", "npc_leva_quist", "npc_halden_mire"
            }));

            var json = new SystemTextJsonSerializer();
            var restored = new CrossingArbitrationSystem();
            restored.RestoreState(json.Deserialize<CrossingArbitrationState>(json.Serialize(sys.CaptureState())));

            Assert.Equal(5, restored.BackerPool.Count);
            Assert.Equal(RulingShape.Overturned, restored.GetRuling(Topic).shape);
            Assert.Equal(1, restored.State.rulingsOverturned);
            Assert.Equal(1, restored.State.rulingsCalled);
            Assert.True(restored.IsRulingOverturned(Topic));
        }

        [Fact]
        public void RestoreStateNullSafeAndIdempotent()
        {
            var sys = Fixture();
            sys.CallStanding(Topic, 40);
            var saved = sys.CaptureState();

            var restored = new CrossingArbitrationSystem();
            restored.RestoreState(saved);
            restored.RestoreState(saved);
            Assert.Equal(1, restored.State.rulings.Count);

            var nullRestored = new CrossingArbitrationSystem();
            nullRestored.RestoreState(null);
            Assert.Empty(nullRestored.State.rulings);
            Assert.True(nullRestored.CallStanding(Topic, 40), "still usable after null restore");
        }

        [Fact]
        public void StateChangedFiresOnMutations()
        {
            var sys = Fixture();
            int changed = 0;
            sys.OnStateChanged += _ => changed++;
            sys.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "npc_osran_kell", principled = true }
            });
            sys.CallStanding(Topic, 40);
            sys.DeclareBacker(Topic, "npc_osran_kell");
            sys.RemoveBacker("npc_osran_kell");
            Assert.True(changed >= 4);
        }
    }

    public class CrossingArbitrationHeadlessDemoTests
    {
        [Fact]
        public void HeadlessDemoPasses()
        {
            var report = CrossingArbitrationHeadlessDemo.Run();
            Assert.True(report.Passed, report.Summary);
            Assert.Equal(0, report.FailedCount);
            Assert.True(report.Checks.Count >= 20);
        }
    }
}
