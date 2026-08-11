using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

using AtomicWar._Game.Encounters;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Encounter_* wiring: 15 expedition encounters — API smoke + Capture/Restore + save slots.
    /// </summary>
    [TestFixture]
    public class EncountersWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_enc_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire)
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var weather = new WeatherSystem(null, 3);
            var temp = new TemperatureSystem(null, weather);
            var rad = new RadiationSystem(needs);
            var ss = new SaveSystem(new SaveSystem.CoreDeps
            {
                GameState = new GameState(),
                WeatherSystem = weather,
                TemperatureSystem = temp,
                NeedsSystem = needs,
                RadiationSystem = rad,
                Shelter = new ShelterClass(),
                GetSurvivors = () => new List<Survivor>(),
                ItemLookup = id => null,
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        [Test]
        public void Amalgamation_FireDamage_Capture()
        {
            var e = new Encounter_Amalgamation();
            float dmg = e.ApplyDamage(10f, "firearm", new System.Random(1));
            Assert.Less(dmg, 1f); // 95% reduction
            float fire = e.ApplyDamage(10f, "fire", new System.Random(1));
            Assert.AreEqual(30f, fire, Eps); // 3x
            var save = e.CaptureState();
            Assert.AreEqual("encounter_amalgamation", save.id);
            Assert.Less(save.healthPool, 500f);
            var e2 = new Encounter_Amalgamation();
            e2.RestoreState(save);
            Assert.AreEqual(save.healthPool, e2.State.healthPool, Eps);
        }

        [Test]
        public void Burrowers_BreachPatch_Capture()
        {
            var e = new BurrowersSystem("encounter_burrowers");
            e.TriggerBreach(2);
            Assert.AreEqual(2, e.State.breachLevel);
            Assert.IsTrue(e.State.returnsNightly);
            Assert.IsFalse(e.PatchBreach(5));
            Assert.IsTrue(e.PatchBreach(10));
            Assert.IsTrue(e.State.isBreachPatched);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_burrowers", save.id);
            var e2 = new BurrowersSystem();
            e2.RestoreState(save);
            Assert.IsTrue(e2.State.isBreachPatched);
            Assert.AreEqual(2, e2.State.breachLevel);
        }

        [Test]
        public void FloodedMaze_Enter_Capture()
        {
            var e = new Encounter_FloodedMaze();
            e.EnterMaze("sv_a", 4);
            Assert.IsTrue(e.IsActive);
            Assert.AreEqual(4, e.BreathRemaining);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_flooded_maze", save.encounter_id);
            Assert.AreEqual("sv_a", save.active_survivor_id);
            var e2 = new Encounter_FloodedMaze();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsActive);
            Assert.AreEqual(4, e2.BreathRemaining);
        }

        [Test]
        public void GlowingDead_Loot_Capture()
        {
            var e = new Encounter_GlowingDead();
            string irradiated = null;
            e.OnItemIrradiated += (_, item) => irradiated = item;
            e.LootCorpse("sv_b", new List<string> { "canned_food", "scrap" });
            Assert.AreEqual("canned_food", irradiated);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_glowing_dead", save.encounter_id);
            Assert.AreEqual(50f, save.rad_transfer_amount, Eps);
            var e2 = new Encounter_GlowingDead();
            e2.RestoreState(save);
            Assert.AreEqual(50f, e2.CaptureState().rad_transfer_amount, Eps);
        }

        [Test]
        public void GlowingStag_Hunt_Capture()
        {
            var e = new Encounter_GlowingStag();
            Assert.IsFalse(e.TryHunt(hasSniper: false, shooterSkill: 1f));
            var (hp, rad, morale) = e.ApplyConsumptionEffects(100f, 0f, 50f);
            Assert.AreEqual(115f, hp, Eps);
            Assert.AreEqual(10f, rad, Eps);
            Assert.AreEqual(20f, morale, Eps);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_glowing_stag", save.id);
            var e2 = new Encounter_GlowingStag();
            e2.RestoreState(save);
            Assert.AreEqual(15f, e2.State.maxHealthBonus, Eps);
        }

        [Test]
        public void HitAndRun_Execute_Capture()
        {
            var e = new Encounter_HitAndRun();
            float morale = 100f, engine = 100f;
            e.RunThemDown(ref morale, ref engine, new System.Random(1));
            Assert.AreEqual(75f, morale, Eps);
            Assert.AreEqual(60f, engine, Eps);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_hit_and_run", save.id);
            var e2 = new Encounter_HitAndRun();
            e2.RestoreState(save);
            Assert.AreEqual(0.90f, e2.State.killSuccessChance, Eps);
        }

        [Test]
        public void Leeches_Attach_Capture()
        {
            var e = new Encounter_Leeches();
            e.AttachLeeches("sv_c", 3);
            e.TickHour("sv_c");
            Assert.AreEqual(15f, e.GetNetRadReduction("sv_c"), Eps); // 5*3
            e.AttachLeeches("sv_d", 2);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_leeches", save.encounterId);
            Assert.AreEqual(2, save.survivorIds.Length);
            var e2 = new Encounter_Leeches();
            e2.RestoreState(save);
            Assert.AreEqual(15f, e2.GetNetRadReduction("sv_c"), Eps);
            Assert.AreEqual(3, e2.AttachedBySurvivorId["sv_c"].attachedLeechCount);
            e2.BurnOffLeeches("sv_c");
            Assert.AreEqual(0, e2.AttachedBySurvivorId["sv_c"].attachedLeechCount);
        }

        [Test]
        public void Mirelurker_Damage_Capture()
        {
            var e = new Encounter_Mirelurker();
            Assert.IsFalse(e.CanEngage(false, false));
            Assert.IsTrue(e.CanEngage(true, false));
            float dmg = e.ApplyDamage(100f, "small_arms");
            Assert.AreEqual(5f, dmg, Eps); // 95% armor
            e.ApplyDamage(800f, "explosive");
            Assert.IsTrue(e.IsDefeated());
            var save = e.CaptureState();
            Assert.AreEqual("encounter_mirelurker", save.id);
            var e2 = new Encounter_Mirelurker();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsDefeated());
        }

        [Test]
        public void PressurePlate_Survive_Capture()
        {
            var e = new Encounter_PressurePlate();
            bool ok = e.ResolveTrap("sv_e", TrapResponse.Dive, agility: 0.9f, stamina: 0.1f, engineering: 0.1f, new System.Random(1));
            Assert.IsTrue(ok);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_pressure_plate", save.encounter_id);
            Assert.Contains("sv_e", save.survived_ids);
            var e2 = new Encounter_PressurePlate();
            e2.RestoreState(save);
            Assert.Contains("sv_e", e2.CaptureState().survived_ids);
        }

        [Test]
        public void RiverPirates_Combat_Capture()
        {
            var e = new Encounter_RiverPirates();
            e.EngageCombat("rowboat", 200f, new System.Random(2));
            Assert.IsFalse(e.State.playerCanFlee);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_river_pirates", save.id);
            var e2 = new Encounter_RiverPirates();
            e2.RestoreState(save);
            Assert.IsFalse(e2.State.playerCanFlee);
        }

        [Test]
        public void Roadblock_Choices_Capture()
        {
            var e = new Encounter_Roadblock();
            int fuel = 10;
            float chassis = 100f;
            Assert.IsTrue(e.ResolveChoice(RoadblockChoice.PayToll, ref fuel, ref chassis, out _));
            Assert.AreEqual(5, fuel);
            Assert.IsTrue(e.ResolveChoice(RoadblockChoice.RamBarricade, ref fuel, ref chassis, out _));
            Assert.AreEqual(55f, chassis, Eps);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_roadblock", save.id);
            var e2 = new Encounter_Roadblock();
            e2.RestoreState(save);
            Assert.AreEqual(5, e2.State.tollFuelCost);
        }

        [Test]
        public void RobotDog_CombatHack_Capture()
        {
            var e = new Encounter_RobotDog();
            // high combat power to kill through armor
            for (int i = 0; i < 20 && !e.IsDefeated(); i++)
                e.EngageCombat(100f, new System.Random(i));
            Assert.IsTrue(e.IsDefeated());
            var (scrap, motors) = e.GetDrops();
            Assert.AreEqual(5, scrap);
            Assert.AreEqual(2, motors);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_robot_dog", save.id);
            var e2 = new Encounter_RobotDog();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsDefeated());
        }

        [Test]
        public void SleepingCamp_Steal_Capture()
        {
            var e = new Encounter_SleepingCamp();
            e.DiscoverCamp();
            // high agility + lucky rng for steal
            e.TrySteal(100f, new System.Random(0));
            var save = e.CaptureState();
            Assert.AreEqual("encounter_sleeping_camp", save.id);
            // either looted or firefight — state is non-default
            Assert.IsTrue(save.isLooted || save.isFirefightStarted);
            var e2 = new Encounter_SleepingCamp();
            e2.RestoreState(save);
            Assert.AreEqual(save.isLooted, e2.State.isLooted);
        }

        [Test]
        public void TripwireMaze_Solve_Capture()
        {
            var e = new Encounter_TripwireMaze();
            e.GenerateMaze(new System.Random(42));
            int correct = e.State.correctPathIndex;
            Assert.GreaterOrEqual(correct, 0);
            Assert.IsTrue(e.TryPath(correct, new System.Random(1)));
            Assert.IsTrue(e.State.isSolved);
            var save = e.CaptureState();
            Assert.AreEqual("encounter_tripwire_maze", save.id);
            var e2 = new Encounter_TripwireMaze();
            e2.RestoreState(save);
            Assert.IsTrue(e2.State.isSolved);
            Assert.AreEqual(correct, e2.State.correctPathIndex);
        }

        [Test]
        public void WarlordTank_Phases_Capture()
        {
            var e = new Encounter_WarlordTank();
            Assert.AreEqual(0f, e.AttackWithSmallArms(), Eps);
            Assert.IsTrue(e.AttackWithWeapon("attacker", "rpg"));
            Assert.AreEqual(TankPhase.TreadsDestroyed, e.GetPhase());
            Assert.IsTrue(e.AttackWithWeapon("attacker", "molotov"));
            Assert.IsTrue(e.IsDestroyed());
            var save = e.CaptureState();
            Assert.AreEqual("encounter_warlord_tank", save.encounterId);
            var e2 = new Encounter_WarlordTank();
            e2.RestoreState(save);
            Assert.IsTrue(e2.IsDestroyed());
        }

        [Test]
        public void MultiEncounter_SaveSlot_RoundTrip()
        {
            string dir = TempDir("multi");
            try
            {
                var amalg = new Encounter_Amalgamation();
                amalg.ApplyDamage(100f, "fire", new System.Random(1));

                var burrow = new BurrowersSystem();
                burrow.TriggerBreach(1);

                var leeches = new Encounter_Leeches();
                leeches.AttachLeeches("sv_z", 4);
                leeches.TickHour("sv_z");

                var tank = new Encounter_WarlordTank();
                tank.AttackWithWeapon("a", "c4");

                var camp = new Encounter_SleepingCamp();
                camp.DiscoverCamp();
                camp.TrySteal(200f, new System.Random(99)); // near-guaranteed steal

                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetEncounterAmalgamation(amalg);
                    ss.SetEncounterBurrowers(burrow);
                    ss.SetEncounterLeeches(leeches);
                    ss.SetEncounterWarlordTank(tank);
                    ss.SetEncounterSleepingCamp(camp);
                    ss.SetEncounterFloodedMaze(new Encounter_FloodedMaze());
                    ss.SetEncounterGlowingDead(new Encounter_GlowingDead());
                    ss.SetEncounterGlowingStag(new Encounter_GlowingStag());
                    ss.SetEncounterHitAndRun(new Encounter_HitAndRun());
                    ss.SetEncounterMirelurker(new Encounter_Mirelurker());
                    ss.SetEncounterPressurePlate(new Encounter_PressurePlate());
                    ss.SetEncounterRiverPirates(new Encounter_RiverPirates());
                    ss.SetEncounterRoadblock(new Encounter_Roadblock());
                    ss.SetEncounterRobotDog(new Encounter_RobotDog());
                    ss.SetEncounterTripwireMaze(new Encounter_TripwireMaze());
                }).Save("slot"));

                var amalg2 = new Encounter_Amalgamation();
                var burrow2 = new BurrowersSystem();
                var leeches2 = new Encounter_Leeches();
                var tank2 = new Encounter_WarlordTank();
                var camp2 = new Encounter_SleepingCamp();
                Assert.IsTrue(MakeSave(dir, ss =>
                {
                    ss.SetEncounterAmalgamation(amalg2);
                    ss.SetEncounterBurrowers(burrow2);
                    ss.SetEncounterLeeches(leeches2);
                    ss.SetEncounterWarlordTank(tank2);
                    ss.SetEncounterSleepingCamp(camp2);
                    ss.SetEncounterFloodedMaze(new Encounter_FloodedMaze());
                    ss.SetEncounterGlowingDead(new Encounter_GlowingDead());
                    ss.SetEncounterGlowingStag(new Encounter_GlowingStag());
                    ss.SetEncounterHitAndRun(new Encounter_HitAndRun());
                    ss.SetEncounterMirelurker(new Encounter_Mirelurker());
                    ss.SetEncounterPressurePlate(new Encounter_PressurePlate());
                    ss.SetEncounterRiverPirates(new Encounter_RiverPirates());
                    ss.SetEncounterRoadblock(new Encounter_Roadblock());
                    ss.SetEncounterRobotDog(new Encounter_RobotDog());
                    ss.SetEncounterTripwireMaze(new Encounter_TripwireMaze());
                }).Load("slot"));

                Assert.AreEqual(amalg.State.healthPool, amalg2.State.healthPool, Eps);
                Assert.AreEqual(1, burrow2.State.breachLevel);
                Assert.AreEqual(20f, leeches2.GetNetRadReduction("sv_z"), Eps); // 5*4
                Assert.AreEqual(TankPhase.TreadsDestroyed, tank2.GetPhase());
                Assert.IsTrue(camp2.State.isLooted || camp2.State.isFirefightStarted);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
