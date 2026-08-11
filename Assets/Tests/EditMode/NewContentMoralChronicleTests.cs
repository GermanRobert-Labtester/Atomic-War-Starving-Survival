using NUnit.Framework;
using AtomicWar._Game.Events;
using AtomicWar._Game.Environment;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class MoralChronicleEntryRequestedTests
    {
        [Test]
        public void EventCarriesAllFields()
        {
            var req = new MoralChronicleEntryRequested(
                day: 42,
                description: "The Silence: alice walked out into the clear sky.",
                kind: MoralChronicleEntryKind.SurvivorLost,
                survivorName: "alice");
            Assert.AreEqual(42, req.Day);
            Assert.AreEqual("The Silence: alice walked out into the clear sky.", req.Description);
            Assert.AreEqual(MoralChronicleEntryKind.SurvivorLost, req.Kind);
            Assert.AreEqual("alice", req.SurvivorName);
        }

        [Test]
        public void EventIsValueType()
        {
            Assert.IsTrue(typeof(MoralChronicleEntryRequested).IsValueType);
        }

        [Test]
        public void DefaultKindIsUnknown()
        {
            var req = new MoralChronicleEntryRequested(0, "any");
            Assert.AreEqual(MoralChronicleEntryKind.Unknown, req.Kind);
        }
    }

    [TestFixture]
    public class SilenceVenturesRaiseChronicleTests
    {
        // Verifies that the Silence weather system surfaces the survivor
        // name through OnSurfaceVentured exactly as the bridge expects.
        [Test]
        public void OnSurfaceVenturedCarriesSurvivorId()
        {
            var w = new Weather_Silence();
            w.SetActive(true);
            string recorded = null;
            w.OnSurfaceVentured += (state, svId) => recorded = svId;
            w.RecordSurfaceVentured("sv_alice");
            Assert.AreEqual("sv_alice", recorded);
        }

        [Test]
        public void InactiveSilenceDoesNotFire()
        {
            var w = new Weather_Silence();
            int fired = 0;
            w.OnSurfaceVentured += (state, svId) => fired++;
            w.RecordSurfaceVentured("sv_alice");
            Assert.AreEqual(0, fired);
        }
    }
}
