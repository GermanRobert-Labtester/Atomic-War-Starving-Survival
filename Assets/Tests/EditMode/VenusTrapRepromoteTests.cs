using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// REPROMOTE-MapHazard-001 — VenusTrap on swamp-tagged looting arrival.
    /// </summary>
    [TestFixture]
    public class VenusTrapRepromoteTests
    {
        private NeedsProfile _profile;
        private List<Object> _destroy;

        [SetUp]
        public void SetUp()
        {
            _destroy = new List<Object>();
            _profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _destroy.Add(_profile);
        }

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _destroy.Count; i++)
            {
                if (_destroy[i] != null) Object.DestroyImmediate(_destroy[i]);
            }
            _destroy = null;
        }

        [Test]
        public void MapGenerator_TagsSwampNodes_Deterministically()
        {
            var a = MapGenerator.Generate(12345);
            var b = MapGenerator.Generate(12345);
            int swamp = 0;
            for (int i = 0; i < a.Nodes.Count; i++)
            {
                if (a.Nodes[i] == null || !a.Nodes[i].HasTag("swamp")) continue;
                swamp++;
                Assert.That(b.GetNode(a.Nodes[i].NodeId).HasTag("swamp"), Is.True);
            }
            Assert.That(swamp, Is.GreaterThan(0));
        }

        [Test]
        public void VenusTrap_EnterNode_ArmsDisguisedTraps()
        {
            var trap = new MapHazard_VenusTrap();
            string entered = null;
            trap.OnNodeEntered += id => entered = id;
            trap.EnterNode("node_swamp_test");
            Assert.AreEqual("node_swamp_test", entered);
            Assert.IsTrue(trap.IsDisguisedAsBerry());
            Assert.That(trap.GetArmLossResult(), Is.EqualTo(0));
        }

        [Test]
        public void Expedition_SwampLootingArrival_DispatchesVenusTrap()
        {
            var needs = new NeedsSystem(_profile, sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new Inventory { Capacity = 20, MaxWeight = 50f };
            var sv = new Survivor
            {
                Id = "sv_berry",
                DisplayName = "Picker",
                State = SurvivorState.Idle
            };
            sv.Needs.Morale = 40f;   // low perception → may not spot
            sv.Needs.Fatigue = 80f;  // low strength → may lose arm
            needs.Register(sv);
            rad.Register(sv);

            var map = MapGenerator.Generate(12345);
            string swampId = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].HasTag("swamp"))
                {
                    swampId = map.Nodes[i].NodeId;
                    break;
                }
            }
            Assert.IsNotNull(swampId, "map must have a swamp-tagged node");

            var expSys = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
            {
                Seed = 3,
                CreateDefaultEncounters = false
            });
            expSys.SetGeneratedMap(map);

            var trap = new MapHazard_VenusTrap();
            int entered = 0;
            int triggered = 0;
            trap.OnNodeEntered += _ => entered++;
            trap.OnTrapTriggered += _ => triggered++;
            expSys.BindVenusTrap(trap);

            // Short travel so one tick can reach looting.
            Assert.IsTrue(expSys.StartExpeditionFromPath(sv, new ExpeditionSystem.PathRequest
            {
                NodeId = swampId,
                DisplayName = "Swamp Edge",
                TravelHours = 1f,
                TrueRad = 10f,
                DangerLevel = 1f,
                Stance = ExpeditionStance.Speed,
                MaxLootCapacity = 10f
            }));

            // Advance until looting (or fail safety).
            for (int t = 0; t < 20 && entered == 0; t++)
                expSys.Tick(1f);

            Assert.AreEqual(1, entered, "VenusTrap.EnterNode must run on swamp looting arrival");
            Assert.That(triggered, Is.GreaterThanOrEqualTo(0));
            expSys.UnsubscribeAll();
        }

        // -----------------------------------------------------------------
        // VENUS-001 — traps_active decrement + EnterNode idempotency fix
        // -----------------------------------------------------------------

        [Test]
        public void VenusTrap_AttemptHarvest_DecrementsTrapsActive_AndClearsDisguise()
        {
            // Repro of VENUS-001: pre-fix, traps_active was never decremented and
            // EnterNode re-armed on every call, making swamp nodes permanently
            // trapped. Post-fix, each AttemptHarvest consumes exactly one trap,
            // and the disguise clears when the last trap fires.
            var trap = new MapHazard_VenusTrap();
            trap.EnterNode("swamp_test_1");

            int initial = GetTrapsActive(trap);
            Assert.That(initial, Is.InRange(2, 4),
                "EnterNode should arm 2-4 traps for a fresh node");

            // Each harvest (escape or amputation) consumes one trap.
            for (int i = initial; i > 0; i--)
            {
                int before = GetTrapsActive(trap);
                trap.AttemptHarvest("sv_harvester", 0.8f); // escape
                Assert.AreEqual(before - 1, GetTrapsActive(trap),
                    "AttemptHarvest must decrement traps_active by 1");
            }

            Assert.AreEqual(0, GetTrapsActive(trap),
                "All traps should be consumed after the expected number of harvests");
            Assert.IsFalse(GetIsDisguised(trap),
                "Once the last trap fires, the disguise must be cleared");
        }

        [Test]
        public void VenusTrap_EnterNode_IdempotentForSameNode_DoesNotReArm()
        {
            // Repro of VENUS-001 (b): pre-fix, EnterNode unconditionally re-armed
            // 2-4 traps on every call. Post-fix, re-entering the same node while
            // traps are still active is a no-op (the live traps are the truth).
            var trap = new MapHazard_VenusTrap();
            trap.EnterNode("swamp_test_2");

            // Consume one trap so the state is "in progress".
            trap.AttemptHarvest("sv_a", 0.8f);
            int afterHarvest = GetTrapsActive(trap);
            Assert.That(afterHarvest, Is.InRange(1, 3));

            // Re-enter the same node — must NOT reset traps_active.
            trap.EnterNode("swamp_test_2");
            Assert.AreEqual(afterHarvest, GetTrapsActive(trap),
                "Re-entering the same node must not re-arm while traps are still active");

            // Clear all traps, then re-enter — this IS a fresh arming.
            while (GetTrapsActive(trap) > 0) trap.AttemptHarvest("sv_a", 0.8f);
            trap.EnterNode("swamp_test_2");
            Assert.That(GetTrapsActive(trap), Is.InRange(2, 4),
                "Re-entering a cleared node must re-arm (2-4 fresh traps)");
        }

        [Test]
        public void VenusTrap_EnterNode_DifferentNodeId_ResetsState()
        {
            // Switching to a different swamp node must fully re-arm. This is
            // correct behavior — a new node is a new encounter, with new traps.
            var trap = new MapHazard_VenusTrap();
            trap.EnterNode("swamp_A");
            int firstNodeTraps = GetTrapsActive(trap);
            Assert.That(firstNodeTraps, Is.InRange(2, 4));

            // Enter a different node while the first is still active. The new
            // node is fresh and must arm 2-4 traps. (The previous node's traps
            // are scoped to its node_id; switching is a clean reset.)
            trap.EnterNode("swamp_B");
            Assert.AreEqual("swamp_B", trap.NodeId);
            Assert.That(GetTrapsActive(trap), Is.InRange(2, 4),
                "Switching to a new node must arm fresh traps");
        }

        // Reflection helpers — public surface exposes NodeId and HazardId only.
        private static int GetTrapsActive(MapHazard_VenusTrap trap)
        {
            var f = typeof(MapHazard_VenusTrap).GetField("_state",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var state = f.GetValue(trap);
            return (int)state.GetType().GetField("traps_active").GetValue(state);
        }

        private static bool GetIsDisguised(MapHazard_VenusTrap trap)
        {
            var f = typeof(MapHazard_VenusTrap).GetField("_state",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var state = f.GetValue(trap);
            return (bool)state.GetType().GetField("is_disguised").GetValue(state);
        }
    }
}
