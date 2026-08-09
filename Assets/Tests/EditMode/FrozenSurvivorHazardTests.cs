using NUnit.Framework;
using AtomicWar._Game.Core;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #902 — MapHazard_FrozenSurvivor.
    ///
    /// The expedition host dispatches EnterNode on every looting arrival, and both
    /// of this hazard's rolls are seeded fixed per node id. That combination makes
    /// idempotency load-bearing rather than cosmetic: an unconditional re-arm clears
    /// was_rescued and re-offers the identical, identically-successful rescue on
    /// every revisit, so the morale reward can be farmed without bound.
    /// </summary>
    [TestFixture]
    public class FrozenSurvivorHazardTests
    {
        /// <summary>
        /// Find a node id whose seeded roll leaves the person alive, so the rescue
        /// path is reachable without depending on any one literal id.
        /// </summary>
        private static string FindLivingNodeId()
        {
            for (int i = 0; i < 200; i++)
            {
                string id = "node_snow_" + i;
                var probe = new MapHazard_FrozenSurvivor();
                probe.EnterNode(id);
                if (probe.IsAlive()) return id;
            }
            Assert.Fail("No seeded node id produced a living frozen survivor.");
            return null;
        }

        [Test]
        public void EnterNode_ArmsEncounter_AndReportsSpottedNode()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            string spotted = null;
            hazard.OnCorpseSpotted += id => spotted = id;

            bool armed = hazard.EnterNode("node_snow_test");

            Assert.IsTrue(armed, "First arrival at a node must arm the encounter.");
            Assert.AreEqual("node_snow_test", spotted);
            Assert.AreEqual("node_snow_test", hazard.NodeId);
            Assert.IsNotEmpty(hazard.SurvivorName);
        }

        [Test]
        public void EnterNode_IsIdempotent_WhileEncounterUnresolved()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode("node_snow_test");
            string firstName = hazard.SurvivorName;

            int spottedAgain = 0;
            hazard.OnCorpseSpotted += _ => spottedAgain++;

            Assert.IsTrue(hazard.EnterNode("node_snow_test"),
                "An unresolved encounter stays available on revisit.");
            Assert.AreEqual(0, spottedAgain, "Re-arming must not re-announce the corpse.");
            Assert.AreEqual(firstName, hazard.SurvivorName);
        }

        [Test]
        public void EnterNode_ReturnsFalse_AfterRescue_SoRewardCannotBeFarmed()
        {
            string nodeId = FindLivingNodeId();
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode(nodeId);

            Assert.IsTrue(hazard.CheckForSignsOfLife(1f));
            Assert.IsTrue(hazard.AttemptRescue(100f), "High medical skill should succeed.");
            Assert.IsTrue(hazard.WasRescued);

            Assert.IsFalse(hazard.EnterNode(nodeId),
                "A rescued node must not re-offer the rescue on revisit.");
            Assert.IsFalse(hazard.CheckForSignsOfLife(1f));
        }

        [Test]
        public void EnterNode_ReturnsFalse_AfterCorpseLooted()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode("node_snow_looted");
            hazard.LootCorpse();

            Assert.IsFalse(hazard.EnterNode("node_snow_looted"));
        }

        [Test]
        public void EnterNode_ArmsFreshEncounter_ForADifferentNode()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode("node_snow_a");
            hazard.LootCorpse();

            Assert.IsTrue(hazard.EnterNode("node_snow_b"),
                "A different node is a different body.");
            Assert.AreEqual("node_snow_b", hazard.NodeId);
        }

        [Test]
        public void LootCorpse_ChargesMoraleOnlyOnce()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode("node_snow_test");

            Assert.That(hazard.LootCorpse(), Is.GreaterThan(0f));
            Assert.That(hazard.LootCorpse(), Is.EqualTo(0f),
                "The same body cannot be looted for morale twice.");
        }

        [Test]
        public void WalkAway_ChargesMoraleOnlyOnce_PerNode()
        {
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode("node_snow_test");

            Assert.That(hazard.WalkAway(), Is.GreaterThan(0f));
            Assert.That(hazard.WalkAway(), Is.EqualTo(0f),
                "Repeated passes must not drain morale for the same abandonment.");
        }

        [Test]
        public void SaveRestore_PreservesResolvedEncounter()
        {
            string nodeId = FindLivingNodeId();
            var hazard = new MapHazard_FrozenSurvivor();
            hazard.EnterNode(nodeId);
            hazard.CheckForSignsOfLife(1f);
            hazard.AttemptRescue(100f);

            var restored = new MapHazard_FrozenSurvivor();
            restored.RestoreState(hazard.CaptureState());

            Assert.IsTrue(restored.WasRescued);
            Assert.IsFalse(restored.EnterNode(nodeId),
                "A rescue must survive a save/load round trip, or reloading farms it.");
        }
    }
}
