using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Prompts #206–#210 — expedition / wasteland milestone perks.
    /// </summary>
    [TestFixture]
    public class ExpeditionPerkTests
    {
        private SkillProgressionSystem _progression;
        private ExpeditionPerkSystem _perks;
        private Survivor _sv;

        [SetUp]
        public void SetUp()
        {
            _progression = new SkillProgressionSystem();
            _progression.RegisterDefaultPerks();
            _perks = new ExpeditionPerkSystem();
            _perks.Bind(_progression);
            _sv = MakeSurvivor("sv_scout", "Scout");
        }

        private static Survivor MakeSurvivor(string id, string name)
        {
            var sv = new Survivor
            {
                Id = id,
                DisplayName = name,
                State = SurvivorState.Idle
            };
            sv.Needs.Morale = 70f;
            sv.Needs.Health = 100f;
            sv.Needs.Fatigue = 10f;
            return sv;
        }

        // ── #206 Pack Mule ───────────────────────────────────────────────

        [Test]
        public void PackMule_EarnedAfterFiveMaxWeightReturns()
        {
            Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.PackMuleId));
            for (int i = 0; i < ExpeditionPerkSystem.MaxWeightReturnsForPackMule - 1; i++)
            {
                _perks.RecordMaxWeightReturn(_sv, 30f, 30f, 1);
                Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.PackMuleId));
            }
            _perks.RecordMaxWeightReturn(_sv, 30f, 30f, 1);
            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.PackMuleId));
            Assert.AreEqual(ExpeditionPerkSystem.MaxWeightReturnsForPackMule,
                _perks.GetCounters(_sv.Id).MaxWeightReturns);
        }

        [Test]
        public void PackMule_IgnoresUnderCapacityReturns()
        {
            for (int i = 0; i < 10; i++)
                _perks.RecordMaxWeightReturn(_sv, 20f, 30f, 1);
            Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.PackMuleId));
            Assert.AreEqual(0, _perks.GetCounters(_sv.Id).MaxWeightReturns);
        }

        [Test]
        public void PackMule_AddsTenKgAndHalvesOverEncumberPenalty()
        {
            Assert.AreEqual(0f, _perks.GetCarryCapacityBonus(_sv), 0.001f);
            Assert.AreEqual(1f, _perks.GetOverEncumberPenaltyMultiplier(_sv), 0.001f);

            for (int i = 0; i < ExpeditionPerkSystem.MaxWeightReturnsForPackMule; i++)
                _perks.RecordMaxWeightReturn(_sv, 30f, 30f, 1);

            Assert.AreEqual(ExpeditionPerkSystem.PackMuleCarryBonusKg,
                _perks.GetCarryCapacityBonus(_sv), 0.001f);
            Assert.AreEqual(ExpeditionPerkSystem.PackMuleOverEncumberPenaltyMult,
                _perks.GetOverEncumberPenaltyMultiplier(_sv), 0.001f);
        }

        [Test]
        public void PackMule_ExpeditionStart_BoostsCapacity_AndHalvesStaminaLoad()
        {
            for (int i = 0; i < ExpeditionPerkSystem.MaxWeightReturnsForPackMule; i++)
                _perks.RecordMaxWeightReturn(_sv, 30f, 30f, 1);

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>(), sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new InventoryClass { Capacity = 100 };
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items = new List<ItemDefinition>();
            var expSys = new ExpeditionSystem(rad, inv, catalog, seed: 7);
            expSys.BindExpeditionPerks(_perks, () => 1);

            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = "loc_test";
            loc.displayName = "Test";
            loc.travelHours = 2f;
            loc.dangerLevel = 1f;
            loc.baseRadsPerHour = 1f;

            needs.Register(_sv);
            rad.Register(_sv);
            Assert.IsTrue(expSys.StartExpedition(_sv, loc));
            var state = expSys.GetExpeditionBySurvivor(_sv.Id);
            Assert.IsNotNull(state);
            Assert.AreEqual(
                ExpeditionSystem.MaxCarryingCapacityDefault + ExpeditionPerkSystem.PackMuleCarryBonusKg,
                state.CarryingCapacity, 0.001f);

            // Fill to capacity and tick stamina once.
            var heavy = ScriptableObject.CreateInstance<ItemDefinition>();
            heavy.id = "heavy_scrap";
            heavy.weight = state.CarryingCapacity;
            heavy.displayName = "Heavy";
            state.TryAddLoot(heavy);

            float staminaBefore = state.Stamina;
            expSys.Tick(1f);
            float drained = staminaBefore - state.Stamina;

            // Base 5 + loadRatio*15*0.5 (pack mule) = 5 + 7.5 = 12.5 (plus possible weather 0)
            Assert.AreEqual(12.5f, drained, 0.5f,
                "Over-encumber portion should be halved with Pack Mule");

            expSys.UnsubscribeAll();
        }

        // ── #207 Light Step ──────────────────────────────────────────────

        [Test]
        public void LightStep_EarnedAfterFiveTrapDisarmsOrSneaks()
        {
            for (int i = 0; i < 3; i++)
                _perks.RecordTrapDisarmed(_sv, 1);
            Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.LightStepId));
            _perks.RecordSneakPast(_sv, 1);
            _perks.RecordSneakPast(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.LightStepId));
        }

        [Test]
        public void LightStep_SuppressesNoiseAndBypassesDogsGhouls()
        {
            for (int i = 0; i < ExpeditionPerkSystem.TrapsOrSneaksForLightStep; i++)
                _perks.RecordTrapDisarmed(_sv, 1);

            Assert.IsTrue(_perks.SuppressesScavengeNoise(_sv));
            Assert.IsTrue(_perks.CanBypassEncounter(_sv, ExpeditionPerkSystem.EncFeralDogs));
            Assert.IsTrue(_perks.CanBypassEncounter(_sv, ExpeditionPerkSystem.EncSleepingGhoul));
            Assert.IsFalse(_perks.CanBypassEncounter(_sv, "enc_deserters"));
        }

        [Test]
        public void LightStep_PerimeterTrapDisarm_CountsTowardPerk()
        {
            var traps = new PerimeterTrapSystem();
            traps.BindExpeditionPerks(_perks, () => 1);
            traps.SetRng(new System.Random(1)); // deterministic

            // Force success by also binding Trap Setter combat perk (100% disarm)
            var combat = new CombatPerkSystem();
            combat.Bind(_progression);
            for (int i = 0; i < CombatPerkSystem.TrapsForTrapSetter; i++)
                combat.RecordTrapDeployed(_sv, 1, 1);
            traps.BindCombatPerks(combat, id => id == _sv.Id ? _sv : null);

            for (int i = 0; i < ExpeditionPerkSystem.TrapsOrSneaksForLightStep; i++)
                Assert.IsTrue(traps.TryDisarmWastelandTrap(_sv));

            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.LightStepId));
        }

        [Test]
        public void LightStep_ScavengeNoiseSuppressed_WhenNoiseSystemWired()
        {
            for (int i = 0; i < ExpeditionPerkSystem.TrapsOrSneaksForLightStep; i++)
                _perks.RecordSneakPast(_sv, 1);

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>(), sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new InventoryClass { Capacity = 50 };
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items = new List<ItemDefinition>();
            var noise = new NoiseSystem();
            var expSys = new ExpeditionSystem(rad, inv, catalog, seed: 11);
            expSys.BindExpeditionPerks(_perks, () => 1, noiseSystem: noise, isStormActive: () => false);

            var loc = ScriptableObject.CreateInstance<LocationDefinitionSO>();
            loc.id = "loc_alley";
            loc.displayName = "Alley";
            loc.travelHours = 1f;
            loc.dangerLevel = 2f;
            loc.baseRadsPerHour = 1f;
            needs.Register(_sv);
            rad.Register(_sv);
            Assert.IsTrue(expSys.StartExpedition(_sv, loc));
            var state = expSys.GetExpeditionBySurvivor(_sv.Id);
            // Jump to looting so PerformLootRoll runs
            state.Phase = ExpeditionPhase.Looting;
            state.TravelTicksCompleted = state.TotalDistanceTicks;
            float noiseBefore = noise.NoiseLevel;
            expSys.Tick(1f);
            Assert.AreEqual(noiseBefore, noise.NoiseLevel, 0.001f,
                "Light Step must not raise scavenging noise");
            expSys.UnsubscribeAll();
        }

        // ── #208 Urban Pathfinder ────────────────────────────────────────

        [Test]
        public void UrbanPathfinder_EarnedAfterTenCitySurveys()
        {
            for (int i = 0; i < ExpeditionPerkSystem.CitySurveysForUrbanPathfinder - 1; i++)
            {
                _perks.RecordCityNodeSurvey(_sv, 1);
                Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.UrbanPathfinderId));
            }
            _perks.RecordCityNodeSurvey(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.UrbanPathfinderId));
        }

        [Test]
        public void UrbanPathfinder_ReducesCityTravelByThirtyPercent_StacksWithBicycle()
        {
            for (int i = 0; i < ExpeditionPerkSystem.CitySurveysForUrbanPathfinder; i++)
                _perks.RecordCityNodeSurvey(_sv, 1);

            Assert.AreEqual(ExpeditionPerkSystem.UrbanPathfinderTravelMult,
                _perks.GetCityRuinTravelMultiplier(_sv, true), 0.001f);
            Assert.AreEqual(1f, _perks.GetCityRuinTravelMultiplier(_sv, false), 0.001f);

            // Stack with Bicycle #68: 0.5 * 0.7 = 0.35
            float stacked = BicycleSystem.BicycleSpeedMultiplier
                            * _perks.GetCityRuinTravelMultiplier(_sv, true);
            Assert.AreEqual(0.35f, stacked, 0.001f);
        }

        [Test]
        public void UrbanPathfinder_ExpeditionStart_ShortensCityNodeTravel()
        {
            for (int i = 0; i < ExpeditionPerkSystem.CitySurveysForUrbanPathfinder; i++)
                _perks.RecordCityNodeSurvey(_sv, 1);

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>(), sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new InventoryClass { Capacity = 50 };
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items = new List<ItemDefinition>();
            var expSys = new ExpeditionSystem(rad, inv, catalog, seed: 3);
            expSys.BindExpeditionPerks(_perks, () => 1);
            needs.Register(_sv);
            rad.Register(_sv);

            var map = new GeneratedMap { Seed = 1 };
            map.Nodes.Add(new MapNode
            {
                NodeId = GeneratedMap.ShelterNodeId,
                DisplayName = "Bunker",
                Ring = DangerRing.Shelter,
                IsRevealed = true,
                IsVisited = true
            });
            var city = new MapNode
            {
                NodeId = "node_city_1",
                DisplayName = "Downtown",
                Ring = DangerRing.CityOutskirts,
                DistanceFromShelter = 10f,
                DangerLevel = 2f,
                Tags = new List<string> { "city", "urban" }
            };
            map.Nodes.Add(city);
            expSys.SetGeneratedMap(map);

            // Path hours via StartExpedition(MapNode) uses ResolveNodeTravelHours
            // which falls back to DistanceFromShelter when no paths exist.
            Assert.IsTrue(expSys.StartExpedition(_sv, city));
            var state = expSys.GetExpeditionBySurvivor(_sv.Id);
            // 10 * 0.7 = 7 → ticks 7
            Assert.AreEqual(7, state.TotalDistanceTicks);
            expSys.UnsubscribeAll();
        }

        // ── #209 Night Terror ────────────────────────────────────────────

        [Test]
        public void NightTerror_EarnedAfterFiveNightNoFlashlightSurvivals()
        {
            for (int i = 0; i < ExpeditionPerkSystem.NightNoFlashlightForNightTerror - 1; i++)
            {
                _perks.RecordNightExpeditionNoFlashlight(_sv, 1);
                Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.NightTerrorId));
            }
            _perks.RecordNightExpeditionNoFlashlight(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.NightTerrorId));
        }

        [Test]
        public void NightTerror_CombatAndStealthBonusesAtNight_ZeroDarkMorale()
        {
            for (int i = 0; i < ExpeditionPerkSystem.NightNoFlashlightForNightTerror; i++)
                _perks.RecordNightExpeditionNoFlashlight(_sv, 1);

            Assert.AreEqual(ExpeditionPerkSystem.NightTerrorCombatBonus,
                _perks.GetNightCombatMultiplier(_sv, true), 0.001f);
            Assert.AreEqual(1f, _perks.GetNightCombatMultiplier(_sv, false), 0.001f);
            Assert.AreEqual(ExpeditionPerkSystem.NightTerrorStealthBonus,
                _perks.GetNightStealthMultiplier(_sv, true), 0.001f);
            Assert.IsTrue(_perks.IgnoresDarknessMorale(_sv));

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>());
            needs.IgnoresDarknessMorale = sv => _perks.IgnoresDarknessMorale(sv);
            needs.Register(_sv);

            var lp = ScriptableObject.CreateInstance<LightProfile>();
            lp.listlessThreshold = 100f; // force listless immediately
            lp.listlessMoraleDrainPerHour = 5f;
            lp.lightExposureLossPerHourDark = 50f;
            lp.vitaminDLowThreshold = 0f; // disable vit-D morale path
            needs.SetPhotoPeriodSystem(() => 0f, lp, () => false);

            float moraleBefore = _sv.Needs.Morale;
            _sv.LightExposure = 0f;
            needs.Tick(2f);
            Assert.AreEqual(moraleBefore, _sv.Needs.Morale, 0.01f,
                "Night Terror should block darkness morale drain");
            Assert.IsTrue(_sv.IsListless, "Still becomes Listless; only morale is spared");
        }

        // ── #210 Forager ─────────────────────────────────────────────────

        [Test]
        public void Forager_EarnedAfterFiveForestOrSwampScavenges()
        {
            for (int i = 0; i < ExpeditionPerkSystem.ForestSwampScavengesForForager - 1; i++)
            {
                _perks.RecordForestOrSwampScavenge(_sv, 1);
                Assert.IsFalse(_perks.Has(_sv, ExpeditionPerkSystem.ForagerId));
            }
            _perks.RecordForestOrSwampScavenge(_sv, 1);
            Assert.IsTrue(_perks.Has(_sv, ExpeditionPerkSystem.ForagerId));
        }

        [Test]
        public void Forager_EmptyLoot_ReturnsOneOrTwoWildFood()
        {
            for (int i = 0; i < ExpeditionPerkSystem.ForestSwampScavengesForForager; i++)
                _perks.RecordForestOrSwampScavenge(_sv, 1);

            var rng = new System.Random(42);
            int n = _perks.GetForagerGuaranteedFoodCount(_sv, existingLootCount: 0, rng);
            Assert.GreaterOrEqual(n, ExpeditionPerkSystem.ForagerMinFood);
            Assert.LessOrEqual(n, ExpeditionPerkSystem.ForagerMaxFood);

            Assert.AreEqual(0, _perks.GetForagerGuaranteedFoodCount(_sv, existingLootCount: 3, rng));

            string foodId = ExpeditionPerkSystem.PickForagerFoodId(rng);
            Assert.IsTrue(
                foodId == ExpeditionPerkSystem.RootsItemId
                || foodId == ExpeditionPerkSystem.BerriesItemId);
        }

        [Test]
        public void Forager_ExpeditionEmptyLoot_AddsRootsOrBerries()
        {
            for (int i = 0; i < ExpeditionPerkSystem.ForestSwampScavengesForForager; i++)
                _perks.RecordForestOrSwampScavenge(_sv, 1);

            var needs = new NeedsSystem(ScriptableObject.CreateInstance<NeedsProfile>(), sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new InventoryClass { Capacity = 50 };
            var catalog = ScriptableObject.CreateInstance<ItemCatalogSO>();
            catalog.items = new List<ItemDefinition>(); // empty loot table
            var expSys = new ExpeditionSystem(rad, inv, catalog, seed: 99);
            expSys.BindExpeditionPerks(_perks, () => 1);
            needs.Register(_sv);
            rad.Register(_sv);

            var map = new GeneratedMap { Seed = 2 };
            map.Nodes.Add(new MapNode
            {
                NodeId = GeneratedMap.ShelterNodeId,
                Ring = DangerRing.Shelter,
                IsRevealed = true,
                IsVisited = true
            });
            var forest = new MapNode
            {
                NodeId = "node_forest_1",
                DisplayName = "Dead Woods",
                Ring = DangerRing.Suburbs,
                DistanceFromShelter = 1f,
                DangerLevel = 1f,
                Tags = new List<string> { "forest" }
            };
            map.Nodes.Add(forest);
            expSys.SetGeneratedMap(map);

            Assert.IsTrue(expSys.StartExpedition(_sv, forest));
            var state = expSys.GetExpeditionBySurvivor(_sv.Id);
            state.Phase = ExpeditionPhase.Looting;
            // Drive enough ticks for loot phase then inbound complete.
            state.IsPushingLuck = false;
            // Force complete path: set inbound and finish.
            state.Phase = ExpeditionPhase.Inbound;
            state.TravelTicksCompleted = 0;
            // Apply forager via empty loot roll first
            state.Phase = ExpeditionPhase.Looting;
            expSys.Tick(1f);

            Assert.GreaterOrEqual(state.CollectedLoot.Count, 1,
                "Forager should add wild food when catalog is empty");
            bool wild = false;
            for (int i = 0; i < state.CollectedLoot.Count; i++)
            {
                var it = state.CollectedLoot[i];
                if (it == null) continue;
                if (it.id == ExpeditionPerkSystem.RootsItemId
                    || it.id == ExpeditionPerkSystem.BerriesItemId)
                    wild = true;
            }
            Assert.IsTrue(wild, "Loot should include roots or berries");
            expSys.UnsubscribeAll();
        }

        // ── Save / load ──────────────────────────────────────────────────

        [Test]
        public void ExpeditionPerks_CaptureRestore_PreservesCountersAndGrants()
        {
            for (int i = 0; i < ExpeditionPerkSystem.MaxWeightReturnsForPackMule; i++)
                _perks.RecordMaxWeightReturn(_sv, 30f, 30f, 1);
            for (int i = 0; i < 2; i++)
                _perks.RecordTrapDisarmed(_sv, 1);
            _perks.RecordCityNodeSurvey(_sv, 1);

            var save = _perks.CaptureState();

            var progression2 = new SkillProgressionSystem();
            progression2.RegisterDefaultPerks();
            // Restore progression perks separately — milestone grants live on progression.
            // Re-grant via counters is not automatic; active perks must be on progression save.
            // Here we verify counter restore + re-bind grant state for Pack Mule via progression.
            progression2.TryGrantPerk(_sv, ExpeditionPerkSystem.PackMuleId, 1);

            var restored = new ExpeditionPerkSystem();
            restored.Bind(progression2);
            restored.RestoreState(save);

            var c = restored.GetCounters(_sv.Id);
            Assert.AreEqual(ExpeditionPerkSystem.MaxWeightReturnsForPackMule, c.MaxWeightReturns);
            Assert.AreEqual(2, c.TrapsDisarmed);
            Assert.AreEqual(1, c.CityNodesSurveyed);
            Assert.IsTrue(restored.Has(_sv, ExpeditionPerkSystem.PackMuleId));
        }

        [Test]
        public void RegisterExpeditionPerks_AreInCatalog()
        {
            Assert.IsNotNull(_progression.GetPerk(ExpeditionPerkSystem.PackMuleId));
            Assert.IsNotNull(_progression.GetPerk(ExpeditionPerkSystem.LightStepId));
            Assert.IsNotNull(_progression.GetPerk(ExpeditionPerkSystem.UrbanPathfinderId));
            Assert.IsNotNull(_progression.GetPerk(ExpeditionPerkSystem.NightTerrorId));
            Assert.IsNotNull(_progression.GetPerk(ExpeditionPerkSystem.ForagerId));
        }
    }
}
