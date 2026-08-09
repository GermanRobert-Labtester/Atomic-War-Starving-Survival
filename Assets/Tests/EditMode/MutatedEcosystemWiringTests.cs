using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #67 — MutatedEcosystemSystem reaching the map.
    ///
    /// The system was constructed, save-registered and ticked daily, so its
    /// mutation stage advanced and persisted across loads. But nothing ever called
    /// RollEcosystemEncounter, HarvestFlora or ProcessFaunaAttack — despite
    /// RollEcosystemEncounter's own summary naming ExpeditionSystem.ProcessSingleTick
    /// as its caller. Mutated flora and fauna could not be encountered at any stage.
    /// These pin the binding so the feature cannot go dormant again unnoticed.
    /// </summary>
    [TestFixture]
    public class MutatedEcosystemWiringTests
    {
        private List<Object> _destroy;

        [SetUp]
        public void SetUp() => _destroy = new List<Object>();

        [TearDown]
        public void TearDown()
        {
            for (int i = 0; i < _destroy.Count; i++)
            {
                if (_destroy[i] != null) Object.DestroyImmediate(_destroy[i]);
            }
            _destroy = null;
        }

        /// <summary>Skip the 50 in-game days of exposure the stage normally costs.</summary>
        private static MutatedEcosystemSystem StagedEcosystem(int stage, int seed)
        {
            float days = stage >= 3 ? MutatedEcosystemSystem.Stage3MutationDays
                : stage >= 2 ? MutatedEcosystemSystem.Stage2MutationDays
                : stage >= 1 ? MutatedEcosystemSystem.Stage1MutationDays
                : 0f;
            var eco = new MutatedEcosystemSystem(new System.Random(seed));
            eco.RestoreState(new EcosystemSave { RadiationExposureDays = days });
            Assert.AreEqual(stage, eco.MutationStage, "test helper must reach the requested stage");
            return eco;
        }

        [Test]
        public void HarvestFlora_ReportsTheExpeditionThatFoundIt()
        {
            // OnFloraEncountered sat behind a CS0067 suppression because HarvestFlora
            // took no expedition and so had nothing to report.
            var eco = StagedEcosystem(1, seed: 5);
            var exp = new ExpeditionState { Survivor = new Survivor { Id = "sv_pick" } };

            ExpeditionState reported = null;
            bool fired = false;
            eco.OnFloraEncountered += e => { fired = true; reported = e; };

            var item = eco.HarvestFlora(exp);
            _destroy.Add(item);

            Assert.IsNotNull(item);
            Assert.IsTrue(fired, "Harvesting flora must raise OnFloraEncountered.");
            Assert.AreSame(exp, reported);
        }

        [Test]
        public void RollEcosystemEncounter_StaysSilentBeforeTheEcosystemMutates()
        {
            var eco = StagedEcosystem(0, seed: 5);

            for (int i = 0; i < 200; i++)
                Assert.AreEqual(0, eco.RollEcosystemEncounter(),
                    "Stage 0 must never produce an encounter.");
        }

        [Test]
        public void LootingTicks_EncounterTheEcosystem_OnceBound()
        {
            var eco = StagedEcosystem(3, seed: 11);
            int flora = 0, fauna = 0;
            eco.OnFloraEncountered += _ => flora++;
            eco.OnFaunaEncountered += (_, __) => fauna++;

            RunExpeditions(eco, expeditions: 40);

            Assert.That(flora + fauna, Is.GreaterThan(0),
                "A bound ecosystem must be reachable from expedition looting ticks.");
        }

        [Test]
        public void LootingTicks_AreUnaffected_WhenNoEcosystemIsBound()
        {
            // The binding is optional (tests and partial hosts construct the
            // expedition system alone), so an unbound host must still tick.
            Assert.DoesNotThrow(() => RunExpeditions(ecosystem: null, expeditions: 5));
        }

        /// <summary>
        /// Send one scavenger out repeatedly against a fixed map so the run is
        /// deterministic, healing between trips so a fatal attack does not end the
        /// sample early.
        /// </summary>
        private void RunExpeditions(MutatedEcosystemSystem ecosystem, int expeditions)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            _destroy.Add(profile);
            var needs = new NeedsSystem(profile);
            var rad = new RadiationSystem(needs);
            var inv = new Inventory { Capacity = 40, MaxWeight = 200f };

            var sv = new Survivor
            {
                Id = "sv_forager",
                DisplayName = "Forager",
                State = SurvivorState.Idle
            };
            needs.Register(sv);
            rad.Register(sv);

            var map = MapGenerator.Generate(12345);
            string nodeId = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && !map.Nodes[i].IsShelter)
                {
                    nodeId = map.Nodes[i].NodeId;
                    break;
                }
            }
            Assert.IsNotNull(nodeId, "map must have a non-shelter node");

            var expSys = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
            {
                Seed = 3,
                CreateDefaultEncounters = false
            });
            expSys.SetGeneratedMap(map);
            if (ecosystem != null) expSys.BindEcosystem(ecosystem);

            for (int run = 0; run < expeditions; run++)
            {
                sv.State = SurvivorState.Idle;
                sv.Needs.Health = 100f;
                sv.Needs.Fatigue = 0f;

                if (!expSys.StartExpeditionFromPath(sv, new ExpeditionSystem.PathRequest
                {
                    NodeId = nodeId,
                    DisplayName = "Overgrowth",
                    TravelHours = 1f,
                    TrueRad = 0f,
                    DangerLevel = 1f,
                    Stance = ExpeditionStance.Speed,
                    MaxLootCapacity = 40f
                }))
                {
                    continue;
                }

                for (int t = 0; t < 20 && expSys.ActiveExpeditions.Count > 0; t++)
                    expSys.Tick(1f);
            }

            expSys.UnsubscribeAll();
        }
    }
}
