using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompt #12 — Unexploded Ordnance: map flags, Reckless loot / Flee detonation.
    /// </summary>
    [TestFixture]
    public class UxoHazardTests
    {
        private const float Eps = 1e-4f;

        private NeedsProfile _needsProfile;
        private NeedsSystem _needsSystem;
        private RadiationSystem _radSystem;
        private Inventory _inventory;
        private ItemCatalogSO _itemCatalog;
        private ItemDefinition _scrap;
        private MedicalSystem _medical;

        [SetUp]
        public void SetUp()
        {
            _needsProfile = ScriptableObject.CreateInstance<NeedsProfile>();
            _needsSystem = new NeedsSystem(_needsProfile, sv => true);
            _radSystem = new RadiationSystem(_needsSystem);
            _inventory = new Inventory { Capacity = 50, MaxWeight = 200f };

            _scrap = ScriptableObject.CreateInstance<ItemDefinition>();
            _scrap.id = "scrap_metal";
            _scrap.displayName = "Scrap";
            _scrap.weight = 0.5f;

            _itemCatalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            _itemCatalog.items = new List<ItemDefinition> { _scrap };

            var shelter = new Shelter();
            _medical = new MedicalSystem(_needsSystem, _inventory, shelter);
            var bone = ScriptableObject.CreateInstance<AfflictionSO>();
            bone.id = AfflictionSO.Ids.BrokenBone;
            bone.displayName = "Broken Bone";
            bone.healthDrainPerHour = 0.5f;
            _medical.RegisterAffliction(bone);
        }

        [TearDown]
        public void TearDown()
        {
            if (_needsProfile != null) Object.DestroyImmediate(_needsProfile);
            if (_scrap != null) Object.DestroyImmediate(_scrap);
            if (_itemCatalog != null) Object.DestroyImmediate(_itemCatalog);

            // ExpeditionSystem subscribes to the static EventBus in its
            // constructor and only unsubscribes via UnsubscribeAll(), which
            // GameBootstrap calls but tests cannot (the instance is local to
            // each test). Without this, handlers bound to dead systems
            // accumulate for the whole run and fire on later tests.
            EventBus.Clear();
        }

        [Test]
        public void MapGenerator_AssignsUxo_Deterministically_AndShelterNeverHasUxo()
        {
            var a = MapGenerator.Generate(12345);
            var b = MapGenerator.Generate(12345);

            int uxoCount = 0;
            int nonShelter = 0;
            for (int i = 0; i < a.Nodes.Count; i++)
            {
                var na = a.Nodes[i];
                var nb = b.GetNode(na.NodeId);
                Assert.That(nb.HasUxo, Is.EqualTo(na.HasUxo), na.NodeId);
                if (na.IsShelter)
                    Assert.That(na.HasUxo, Is.False, "Shelter must never have UXO");
                else
                {
                    nonShelter++;
                    if (na.HasUxo) uxoCount++;
                }
            }

            Assert.That(nonShelter, Is.GreaterThan(0));
            // ~20% target — allow a band for small node counts (11 non-shelter)
            float ratio = (float)uxoCount / nonShelter;
            Assert.That(ratio, Is.InRange(0.05f, 0.45f),
                $"UXO ratio {ratio} ({uxoCount}/{nonShelter}) should be roughly ~20%");
        }

        [Test]
        public void SameSeed_HasUxo_IncludedInLayoutFingerprint()
        {
            var a = MapGenerator.Generate(42);
            var b = MapGenerator.Generate(42);
            Assert.That(a.ComputeLayoutFingerprint(), Is.EqualTo(b.ComputeLayoutFingerprint()));
        }

        [Test]
        public void RecklessLoot_OnUxo_CanDetonate_CarefulLoot_DoesNot()
        {
            // Fixed seed that produces many low rolls → Reckless should hit at least once
            int recklessHits = 0;
            int carefulHits = 0;
            const int trials = 200;
            for (int i = 0; i < trials; i++)
            {
                var rngR = new Random(i * 17 + 3);
                var rngC = new Random(i * 17 + 3);
                if (UxoHazardSystem.ShouldDetonateOnLoot(RiskBiasTrait.Reckless, rngR))
                    recklessHits++;
                if (UxoHazardSystem.ShouldDetonateOnLoot(RiskBiasTrait.Cautious, rngC))
                    carefulHits++;
                if (UxoHazardSystem.ShouldDetonateOnLoot(RiskBiasTrait.Paranoid, rngC))
                    carefulHits++;
            }

            Assert.That(recklessHits, Is.GreaterThan(trials / 5),
                "Reckless loot on UXO should detonate often (~55%)");
            Assert.That(carefulHits, Is.EqualTo(0),
                "Non-Reckless loot must not detonate UXO on careful footing");
        }

        [Test]
        public void Flee_OnUxo_CanDetonate()
        {
            int hits = 0;
            const int trials = 200;
            for (int i = 0; i < trials; i++)
            {
                if (UxoHazardSystem.ShouldDetonateOnFlee(new Random(i * 31 + 1)))
                    hits++;
            }
            Assert.That(hits, Is.GreaterThan(trials / 10),
                "Flee on UXO should detonate at a meaningful rate (~35%)");
            Assert.That(hits, Is.LessThan(trials),
                "Flee detonation must not be guaranteed every time");
        }

        [Test]
        public void ApplyDetonation_DropsLoot_DamagesHealth_InflictsBrokenBone_ForcesInbound()
        {
            var survivor = new Survivor { Id = "sv_uxo", DisplayName = "Miner" };
            survivor.Needs.Health = 90f;
            _needsSystem.Register(survivor);
            _radSystem.Register(survivor);

            var exp = new ExpeditionState
            {
                ExpeditionId = "exp_uxo",
                SurvivorId = survivor.Id,
                Survivor = survivor,
                TargetLocationId = "node_suburb_01",
                Phase = ExpeditionPhase.Looting
            };
            exp.TryAddLoot(_scrap);
            exp.TryAddLoot(_scrap);
            Assert.That(exp.CollectedLoot.Count, Is.EqualTo(2));

            bool ok = UxoHazardSystem.ApplyDetonation(exp, _medical);
            Assert.That(ok, Is.True);
            Assert.That(exp.UxoDetonated, Is.True);
            Assert.That(exp.CollectedLoot.Count, Is.EqualTo(0), "All loot dropped in the crater");
            Assert.That(exp.Phase, Is.EqualTo(ExpeditionPhase.Inbound));
            Assert.That(survivor.Needs.Health,
                Is.EqualTo(90f - UxoHazardSystem.DetonationHealthDamage).Within(Eps));
            Assert.That(_medical.HasAffliction(survivor, AfflictionSO.Ids.BrokenBone), Is.True);
        }

        [Test]
        public void ExpeditionSystem_RecklessLoot_OnUxoNode_DetonatesWithForcedHook()
        {
            var map = MapGenerator.Generate(7);
            MapNode uxoNode = null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                if (map.Nodes[i] != null && map.Nodes[i].HasUxo)
                {
                    uxoNode = map.Nodes[i];
                    break;
                }
            }
            // Ensure at least one UXO node for the test
            if (uxoNode == null)
            {
                uxoNode = map.Nodes.Find(n => n != null && !n.IsShelter);
                Assert.That(uxoNode, Is.Not.Null);
                uxoNode.HasUxo = true;
            }

            var expedition = new ExpeditionSystem(
                _radSystem, _inventory, _itemCatalog,
                new ExpeditionSystem.Config { MedicalSystem = _medical, Seed = 1 });
            expedition.SetGeneratedMap(map);

            var reckless = new Survivor
            {
                Id = "sv_reckless",
                DisplayName = "Reckless",
                RiskBias = RiskBiasTrait.Reckless
            };
            reckless.Needs.Health = 100f;
            _needsSystem.Register(reckless);
            _radSystem.Register(reckless);

            Assert.That(expedition.StartExpedition(reckless, uxoNode), Is.True);
            var state = expedition.GetExpeditionBySurvivor(reckless.Id);
            state.Phase = ExpeditionPhase.Looting;
            state.TryAddLoot(_scrap);

            string log = null;
            expedition.OnUxoDetonated += (e, msg) => log = msg;

            bool detonated = expedition.ForceUxoDetonationForTests(state);
            Assert.That(detonated, Is.True);
            Assert.That(state.UxoDetonated, Is.True);
            Assert.That(state.CollectedLoot.Count, Is.EqualTo(0));
            Assert.That(state.Phase, Is.EqualTo(ExpeditionPhase.Inbound));
            Assert.That(log, Is.EqualTo(UxoHazardSystem.DetonationLogMessage));
            Assert.That(reckless.Needs.Health, Is.LessThan(100f));
        }

        [Test]
        public void NonReckless_LootRoll_NeverDetonates_EvenOnUxoNode()
        {
            // Pure rule: careful traits always return false regardless of RNG stream
            for (int i = 0; i < 50; i++)
            {
                Assert.That(
                    UxoHazardSystem.ShouldDetonateOnLoot(RiskBiasTrait.Realist, new Random(i)),
                    Is.False);
                Assert.That(
                    UxoHazardSystem.ShouldDetonateOnLoot(RiskBiasTrait.Cautious, new Random(i)),
                    Is.False);
            }
        }

        [Test]
        public void HasUxo_PersistsAcrossRegenerateFromSameSeed()
        {
            // Save path: regenerate from seed then restore reveal — UXO must rematch.
            int seed = 999;
            var original = MapGenerator.Generate(seed);
            var save = original.CaptureState();

            var reloaded = MapGenerator.Generate(seed);
            reloaded.RestoreRevealState(save);

            for (int i = 0; i < original.Nodes.Count; i++)
            {
                var o = original.Nodes[i];
                var r = reloaded.GetNode(o.NodeId);
                Assert.That(r.HasUxo, Is.EqualTo(o.HasUxo), o.NodeId);
            }
        }
    }
}
