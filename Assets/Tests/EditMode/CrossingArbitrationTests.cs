using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Core;
using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// ASHFALL: NOBODY'S CHARTER — Phase 3. The Standing
    /// (CrossingArbitrationSystem §5.2). Proves: call standing,
    /// declare backers, hold ruling, overturn, backer death,
    /// principled vs rigged, save round-trip. Pure C#.
    /// </summary>
    [TestFixture]
    public class CrossingArbitrationTests
    {
        private CrossingArbitrationSystem CreateSystem()
        {
            var sys = new CrossingArbitrationSystem();
            sys.LoadBackerPool(new List<BackerDef>
            {
                new BackerDef { id = "b1", displayName = "Backer One", principled = true },
                new BackerDef { id = "b2", displayName = "Backer Two", principled = false },
                new BackerDef { id = "b3", displayName = "Backer Three", principled = false },
                new BackerDef { id = "b4", displayName = "Backer Four", principled = true },
                new BackerDef { id = "b5", displayName = "Backer Five", principled = false },
                new BackerDef { id = "b6", displayName = "Backer Six", principled = true },
            });
            return sys;
        }

        [Test]
        public void FreshSystem_HasNoRulings()
        {
            var sys = CreateSystem();
            Assert.That(sys.Rulings.Count, Is.Zero);
            Assert.That(sys.IsRulingHeld("any_topic"), Is.False);
        }

        [Test]
        public void CallStanding_CreatesPendingRuling()
        {
            var sys = CreateSystem();
            bool called = false;
            sys.OnStandingCalled += t => called = true;

            Assert.That(sys.CallStanding("dispute_1", 70), Is.True);
            Assert.That(called, Is.True);
            var ruling = sys.GetRuling("dispute_1");
            Assert.That(ruling, Is.Not.Null);
            Assert.That(ruling.shape, Is.EqualTo(RulingShape.Pending));
            Assert.That(ruling.dayCalled, Is.EqualTo(70));
        }

        [Test]
        public void CallStanding_DuplicateWhenPending_IsIdempotent()
        {
            var sys = CreateSystem();
            Assert.That(sys.CallStanding("dispute_1", 70), Is.True);
            int events = 0;
            sys.OnStandingCalled += _ => events++;
            Assert.That(sys.CallStanding("dispute_1", 71), Is.True, "re-call on pending is ok");
            Assert.That(events, Is.EqualTo(1));
        }

        [Test]
        public void DeclareBacker_AddsToRuling()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            Assert.That(sys.DeclareBacker("dispute_1", "b1"), Is.True);
            var ruling = sys.GetRuling("dispute_1");
            Assert.That(ruling.backers.Count, Is.EqualTo(1));
            Assert.That(ruling.shape, Is.EqualTo(RulingShape.Pending));
        }

        [Test]
        public void ThreeBackers_HoldsRuling_AsHonest_WhenMajorityPrincipled()
        {
            var sys = CreateSystem();
            StandingRuling held = null;
            sys.OnRulingMade += r => held = r;

            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1"); // principled
            sys.DeclareBacker("dispute_1", "b4"); // principled
            sys.DeclareBacker("dispute_1", "b2"); // not principled

            Assert.That(sys.IsRulingHeld("dispute_1"), Is.True);
            Assert.That(held, Is.Not.Null);
            Assert.That(held.shape, Is.EqualTo(RulingShape.Honest));
        }

        [Test]
        public void ThreeBackers_HoldsRuling_AsRigged_WhenMajorityNotPrincipled()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b2"); // not principled
            sys.DeclareBacker("dispute_1", "b3"); // not principled
            sys.DeclareBacker("dispute_1", "b1"); // principled — minority

            var ruling = sys.GetRuling("dispute_1");
            Assert.That(ruling.shape, Is.EqualTo(RulingShape.Rigged));
        }

        [Test]
        public void OverturnRuling_ClearsBackers()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1");
            sys.DeclareBacker("dispute_1", "b2");
            sys.DeclareBacker("dispute_1", "b3");

            Assert.That(sys.IsRulingHeld("dispute_1"), Is.True);

            bool overturned = false;
            sys.OnRulingOverturned += _ => overturned = true;

            Assert.That(sys.OverturnRuling("dispute_1",
                new List<string> { "b4", "b5", "b6" }), Is.True);
            Assert.That(overturned, Is.True);
            Assert.That(sys.IsRulingOverturned("dispute_1"), Is.True);
            var ruling = sys.GetRuling("dispute_1");
            Assert.That(ruling.backers.Count, Is.Zero);
        }

        [Test]
        public void OverturnRuling_NeedsMinimumBackers()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1");
            sys.DeclareBacker("dispute_1", "b2");
            sys.DeclareBacker("dispute_1", "b3");

            Assert.That(sys.OverturnRuling("dispute_1",
                new List<string> { "b4", "b5" }), Is.False, "need 3+ counter-backers");
        }

        [Test]
        public void DeadBacker_LosesHold_MayRevertToPending()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1");
            sys.DeclareBacker("dispute_1", "b2");
            sys.DeclareBacker("dispute_1", "b3");
            Assert.That(sys.IsRulingHeld("dispute_1"), Is.True);

            // Kill one backer → drops below 3 → reverts to Pending
            sys.RemoveBacker("b1");
            var ruling = sys.GetRuling("dispute_1");
            Assert.That(ruling.shape, Is.EqualTo(RulingShape.Pending));
            Assert.That(ruling.backers.Count, Is.EqualTo(2));
        }

        [Test]
        public void DeadBacker_CannotDeclare()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.RemoveBacker("b1");
            Assert.That(sys.DeclareBacker("dispute_1", "b1"), Is.False);
        }

        [Test]
        public void CannotDeclareTwice()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            Assert.That(sys.DeclareBacker("dispute_1", "b1"), Is.True);
            Assert.That(sys.DeclareBacker("dispute_1", "b1"), Is.False);
        }

        [Test]
        public void CannotOverturnPending()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            Assert.That(sys.OverturnRuling("dispute_1",
                new List<string> { "b4", "b5", "b6" }), Is.False, "pending cannot be overturned");
        }

        [Test]
        public void GetAvailableBackers_ExcludesCommitted()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1");
            var avail = sys.GetAvailableBackers("dispute_1");
            Assert.That(avail.Count, Is.EqualTo(5)); // 6 - 1 committed
            for (int i = 0; i < avail.Count; i++)
                Assert.That(avail[i].id, Is.Not.EqualTo("b1"));
        }

        [Test]
        public void SaveRoundTrip_PreservesRulingsAndBackers()
        {
            var sys = CreateSystem();
            sys.CallStanding("dispute_1", 70);
            sys.DeclareBacker("dispute_1", "b1");
            sys.DeclareBacker("dispute_1", "b2");
            sys.DeclareBacker("dispute_1", "b3");
            sys.RemoveBacker("b5");

            var captured = sys.CaptureState();
            var restored = new CrossingArbitrationSystem();
            restored.RestoreState(captured);

            Assert.That(restored.Rulings.Count, Is.EqualTo(1));
            Assert.That(restored.GetRuling("dispute_1").backers.Count, Is.EqualTo(3));
            Assert.That(restored.IsRulingHeld("dispute_1"), Is.True);
            Assert.That(restored.GetBacker("b5").isAlive, Is.False);
        }

        [Test]
        public void RestoreState_NullSafe()
        {
            var sys = CreateSystem();
            sys.RestoreState(null);
            Assert.That(sys.Rulings.Count, Is.Zero);
        }

        // ── NPC_DessaVane ──────────────────────────────────────────────

        [Test]
        public void Dessa_DrawContract_Accumulates()
        {
            var dessa = new NPC_DessaVane();
            dessa.Initialise("Dessa Vane");
            int last = 0;
            dessa.OnContractDrawn += (_, count) => last = count;

            Assert.That(dessa.DrawContract(), Is.EqualTo(1));
            Assert.That(dessa.DrawContract(), Is.EqualTo(2));
            Assert.That(last, Is.EqualTo(2));
        }

        [Test]
        public void Dessa_CollectForfeit_Accumulates()
        {
            var dessa = new NPC_DessaVane();
            dessa.Initialise("Dessa Vane");
            Assert.That(dessa.CollectForfeit(), Is.EqualTo(1));
            Assert.That(dessa.State.forfeitsCollected, Is.EqualTo(1));
        }

        [Test]
        public void Dessa_SaveRoundTrip()
        {
            var dessa = new NPC_DessaVane();
            dessa.Initialise("Dessa Vane");
            dessa.DrawContract();
            dessa.CollectForfeit();

            var dessa2 = new NPC_DessaVane();
            dessa2.RestoreState((NPC_DessaVaneState)dessa.CaptureState());
            Assert.That(dessa2.State.contractsDrawn, Is.EqualTo(1));
            Assert.That(dessa2.State.forfeitsCollected, Is.EqualTo(1));
        }
    }
}
