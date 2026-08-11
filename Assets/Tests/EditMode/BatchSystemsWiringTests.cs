using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

using AtomicWar._Game.Endgame;

using AtomicWar._Game.Encounters;

using AtomicWar._Game.Factions;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Batch wiring: 16 previously-unconstructed CaptureState systems
    /// (disease, scapegoat, iron man, android, sheriff, scenario, speedrun,
    /// true ending, 8 sieges) — API smoke + save round-trips.
    /// </summary>
    [TestFixture]
    public class BatchSystemsWiringTests
    {
        private const float Eps = 1e-3f;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_batch_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        // ── Disease ────────────────────────────────────────────────────

        [Test]
        public void DiseaseExpansion_InfectAndCaptureRestore()
        {
            var a = new DiseaseSystem_Expansion();
            a.RegisterDisease("cholera", DiseaseSystem_Expansion.VECTOR_WATER);
            a.Infect("sv_a", "cholera");
            a.Infect("sv_b", "cholera");
            a.PurifyWater();

            var save = a.CaptureState();
            Assert.AreEqual(1, save.diseases.Count);
            Assert.AreEqual(2, save.diseases[0].infected_ids.Count);
            Assert.IsTrue(save.water_purified);

            a.Infect("sv_c", "cholera"); // mutate after capture
            Assert.AreEqual(2, save.diseases[0].infected_ids.Count);

            var b = new DiseaseSystem_Expansion();
            b.RestoreState(save);
            var again = b.CaptureState();
            Assert.AreEqual(2, again.diseases[0].infected_ids.Count);
            Assert.IsTrue(again.water_purified);
        }

        [Test]
        public void DiseaseExpansion_SaveSlot_RoundTrip()
        {
            string dir = TempDir("disease");
            try
            {
                var a = new DiseaseSystem_Expansion();
                a.RegisterDisease("flu", DiseaseSystem_Expansion.VECTOR_AIR);
                a.Infect("x", "flu");
                a.SealVents();

                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                SaveSystem Make(DiseaseSystem_Expansion sys)
                {
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
                    ss.SetDiseaseExpansionSystem(sys);
                    return ss;
                }

                Assert.IsTrue(Make(a).Save("slot"));
                var b = new DiseaseSystem_Expansion();
                Assert.IsTrue(Make(b).Load("slot"));
                var cap = b.CaptureState();
                Assert.AreEqual(1, cap.diseases.Count);
                Assert.AreEqual("flu", cap.diseases[0].disease_id);
                Assert.IsTrue(cap.vents_sealed);
                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ── Scapegoat ──────────────────────────────────────────────────

        [Test]
        public void DynamicScapegoat_SelectsLowestSkillWhenMoraleLow()
        {
            var s = new Dynamic_Scapegoat();
            string chosen = null;
            s.OnScapegoatSelected += id => chosen = id;

            s.SelectScapegoat(new List<(string, float, float)>
            {
                ("strong", 0.9f, 0.9f),
                ("weak", 0.1f, 0.1f)
            }, avgMorale: 0.1f);

            Assert.AreEqual("weak", chosen);
            Assert.AreEqual("weak", s.GetScapegoat());

            var save = s.CaptureState();
            s.ClearScapegoat();
            Assert.IsNull(s.GetScapegoat());
            Assert.AreEqual("weak", save.currentScapegoatId);

            var s2 = new Dynamic_Scapegoat();
            s2.RestoreState(save);
            Assert.AreEqual("weak", s2.GetScapegoat());
        }

        // ── Iron Man ───────────────────────────────────────────────────

        [Test]
        public void IronMan_LastDeath_MarksDeleteAndDeletesFile()
        {
            string path = Path.Combine(Path.GetTempPath(), "ironman_" + Guid.NewGuid().ToString("N") + ".json");
            File.WriteAllText(path, "{}");
            try
            {
                var mode = new Mode_IronMan();
                mode.EnableIronMan(path);
                Assert.IsTrue(mode.IsIronManActive());

                mode.OnSurvivorDeath("a", 1);
                Assert.IsFalse(mode.ShouldDeleteSave());

                mode.OnSurvivorDeath("b", 0);
                Assert.IsTrue(mode.ShouldDeleteSave());
                mode.DeleteSave();
                Assert.IsFalse(File.Exists(path));
                Assert.IsTrue(File.Exists(Path.ChangeExtension(path, ".memorial.txt")));

                var save = mode.CaptureState();
                Assert.IsTrue(save.is_active);
                Assert.IsTrue(save.save_deleted);
                Assert.IsTrue(save.last_survivor_died);
            }
            finally
            {
                try { if (File.Exists(path)) File.Delete(path); } catch { }
                try
                {
                    string m = Path.ChangeExtension(path, ".memorial.txt");
                    if (File.Exists(m)) File.Delete(m);
                }
                catch { }
            }
        }

        [Test]
        public void IronMan_DeletionFailure_DoesNotReportSaveDeleted()
        {
            string dir = Path.Combine(Path.GetTempPath(), "ironman_locked_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            string path = Path.Combine(dir, "save.json");
            File.WriteAllText(path, "{}");

            // On Windows, an open handle blocks File.Delete with IOException.
            // On Unix, deleting a file requires write permission on its parent
            // directory (the file's own permissions don't matter), so we lock
            // the directory instead. Either way this forces File.Delete to
            // throw so we can assert the honest failure path (C-2: it used to
            // report success regardless).
            FileStream lockHandle = null;
            bool isWindows = Path.DirectorySeparatorChar == '\\';
            try
            {
                if (isWindows)
                    lockHandle = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
                else
                    RunChmod(dir, "555");

                var mode = new Mode_IronMan();
                mode.EnableIronMan(path);
                mode.OnSurvivorDeath("a", 0);
                Assert.IsTrue(mode.ShouldDeleteSave());

                LogAssert.Expect(LogType.Error, new System.Text.RegularExpressions.Regex(
                    @"\[Mode_IronMan\] Failed to delete save"));
                mode.DeleteSave();

                Assert.IsTrue(File.Exists(path), "save file should still exist after a failed deletion");
                var save = mode.CaptureState();
                Assert.IsFalse(save.save_deleted, "save_deleted must stay false when File.Delete throws");
                Assert.IsTrue(mode.ShouldDeleteSave(), "mode should still consider the save pending deletion so it can retry");
            }
            finally
            {
                lockHandle?.Dispose();
                if (!isWindows) RunChmod(dir, "755");
                try { Directory.Delete(dir, true); } catch { }
            }
        }

        private static void RunChmod(string path, string mode)
        {
            var psi = new System.Diagnostics.ProcessStartInfo("chmod", $"{mode} \"{path}\"")
            {
                UseShellExecute = false,
                CreateNoWindow = true
            };
            using (var proc = System.Diagnostics.Process.Start(psi))
            {
                proc.WaitForExit(5000);
            }
        }

        // ── Android / Sheriff ──────────────────────────────────────────

        [Test]
        public void Android_JoinRevealMutiny_RoundTrip()
        {
            var a = new NPC_Android();
            a.JoinBunker("andy_1");
            a.CheckShot("andy_1", wasShotInRaid: true);
            Assert.IsTrue(a.IsRevealed("andy_1"));

            var save = a.CaptureState();
            var b = new NPC_Android();
            b.RestoreState(save);
            Assert.IsTrue(b.IsRevealed("andy_1"));
        }

        [Test]
        public void Sheriff_AssignAndCapture()
        {
            var role = new Role_Sheriff();
            role.AssignSheriff("sv_chief", new List<string> { "sv_chief", "sv_other" });
            Assert.AreEqual("sv_chief", role.GetCurrentSheriff());

            var save = role.CaptureState();
            role.RemoveSheriff();
            Assert.IsNull(role.GetCurrentSheriff());

            role.RestoreState(save);
            Assert.AreEqual("sv_chief", role.GetCurrentSheriff());
        }

        // ── Scenario / Speedrun / True Ending ───────────────────────────

        [Test]
        public void ScenarioGen_SetModifier_CaptureRestore()
        {
            var gen = new UI_ScenarioGen();
            gen.SetModifier("start_day", 45);
            gen.SetModifier("radiation", 2f);
            Assert.IsTrue(gen.GetActiveModifiers().Any(m => m == "start_day"));

            var save = gen.CaptureState();
            gen.ResetToDefaults();
            Assert.AreEqual(0, gen.GetActiveModifiers().Count);
            Assert.AreEqual(45, save.start_day);

            gen.RestoreState(save);
            Assert.AreEqual(45, gen.CaptureState().start_day);
            Assert.AreEqual(2f, gen.CaptureState().radiation_multiplier, Eps);
        }

        [Test]
        public void SpeedrunTimer_SplitsAndSave()
        {
            var t = new UI_SpeedrunTimer();
            t.StartTimer();
            t.TickSecond(10f, 5f);
            t.RecordSplit("First Death", 5);
            Assert.AreEqual(1, t.GetSplits().Count);
            Assert.AreEqual(10f, t.GetRealTime(), Eps);

            var save = t.CaptureState();
            t.PauseTimer();
            t.CompleteRun();

            var t2 = new UI_SpeedrunTimer();
            t2.RestoreState(save);
            Assert.IsTrue(t2.CaptureState().is_active);
            Assert.AreEqual(1, t2.GetSplits().Count);
            Assert.AreEqual("First Death", t2.GetSplits()[0].name);
        }

        [Test]
        public void TrueEnding_PrereqsHackComplete()
        {
            var v = new Victory_TrueEnding();
            v.SetCurrentDay(120);
            v.CheckPrerequisites("highest", true, true, true);
            v.UpdatePower(1000);
            v.StartHack(1000);
            for (int i = 0; i < 48; i++)
                v.TickHour();
            Assert.IsTrue(v.IsComplete());
            Assert.AreEqual(120, v.GetEndingDay());

            var save = v.CaptureState();
            Assert.IsTrue(save.terraformer_hacked);
            Assert.IsTrue(save.ash_cleared);

            var v2 = new Victory_TrueEnding();
            v2.RestoreState(save);
            Assert.IsTrue(v2.IsComplete());
        }

        // ── Sieges ─────────────────────────────────────────────────────

        [Test]
        public void SiegeArtillery_StartAndCapture()
        {
            var s = new Siege_Artillery();
            s.StartSiege();
            s.TickTurn(10f, new List<string> { "door", "vent" }, new System.Random(1));
            var save = s.CaptureState();
            Assert.Greater(save.turnsActive, 0);

            var s2 = new Siege_Artillery();
            s2.RestoreState(save);
            Assert.AreEqual(save.turnsActive, s2.CaptureState().turnsActive);
        }

        [Test]
        public void SiegeBiowarfare_LaunchInfect()
        {
            var s = new Siege_Biowarfare();
            s.LaunchCorpse();
            for (int i = 0; i < 30; i++) s.TickHour();
            // May infect depending on timers; state should capture.
            var save = s.CaptureState();
            Assert.IsTrue(save.corpseOnVent || save.hoursSinceLaunch > 0f || save.bunkerInfected || !save.cleared);

            var s2 = new Siege_Biowarfare();
            s2.RestoreState(save);
            Assert.AreEqual(save.hoursSinceLaunch, s2.CaptureState().hoursSinceLaunch, Eps);
        }

        [Test]
        public void SiegeBlockade_LocksExpeditions()
        {
            var s = new Siege_Blockade();
            s.StartBlockade();
            Assert.IsTrue(s.IsExpeditionLocked());
            s.TickDay(1f, 1f, 100f);
            var save = s.CaptureState();
            Assert.IsTrue(save.expeditionsLocked);
            Assert.AreEqual(1, save.daysActive);
        }

        [Test]
        public void SiegeHostageShield_Start()
        {
            var s = new Siege_HostageShield();
            s.StartSiege(3);
            var save = s.CaptureState();
            Assert.AreEqual(3, save.hostagesCount);
        }

        [Test]
        public void SiegeNightRaid_CutPower()
        {
            var s = new Siege_NightRaid();
            s.CutPower();
            s.TickTurn();
            var save = s.CaptureState();
            Assert.IsTrue(save.powerCut);
            Assert.GreaterOrEqual(save.turnsInDark, 1);
        }

        [Test]
        public void SiegeSappers_DigToBreach()
        {
            var s = new Siege_Sappers();
            s.StartDigging();
            int guard = 0;
            while (!s.IsTunnelComplete() && guard++ < 50)
                s.TickTurn();
            Assert.IsTrue(s.IsTunnelComplete());
            s.TriggerBreach();
            Assert.IsFalse(string.IsNullOrEmpty(s.GetBreachLocation()));
        }

        [Test]
        public void SiegeSmokeOut_BlockVents()
        {
            var s = new Siege_SmokeOut();
            s.BlockVents();
            s.TickMinute();
            var save = s.CaptureState();
            Assert.IsTrue(save.ventsBlocked);
            Assert.Less(save.o2Level, 100f + Eps);
        }

        [Test]
        public void SiegeVehicleRam_Approach()
        {
            var s = new Siege_VehicleRam();
            s.StartApproach();
            s.TickTurn();
            var save = s.CaptureState();
            Assert.IsTrue(save.truckApproaching);
        }

        [Test]
        public void SiegeArtillery_SaveSlot_RoundTrip()
        {
            string dir = TempDir("siege_art");
            try
            {
                var a = new Siege_Artillery();
                a.StartSiege();
                a.TickTurn(5f, new List<string> { "m1" }, new System.Random(2));

                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                SaveSystem Make(Siege_Artillery sys)
                {
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
                    ss.SetSiegeArtillerySystem(sys);
                    return ss;
                }

                Assert.IsTrue(Make(a).Save("art"));
                var b = new Siege_Artillery();
                Assert.IsTrue(Make(b).Load("art"));
                Assert.AreEqual(a.CaptureState().turnsActive, b.CaptureState().turnsActive);
                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        [Test]
        public void IronMan_SaveSlot_RoundTrip()
        {
            string dir = TempDir("iron");
            try
            {
                var a = new Mode_IronMan();
                a.EnableIronMan(Path.Combine(dir, "run.json"));
                a.OnSurvivorDeath("x", 2);

                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                SaveSystem Make(Mode_IronMan sys)
                {
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
                    ss.SetIronManMode(sys);
                    return ss;
                }

                Assert.IsTrue(Make(a).Save("im"));
                var b = new Mode_IronMan();
                Assert.IsTrue(Make(b).Load("im"));
                Assert.IsTrue(b.IsIronManActive());
                Assert.IsTrue(b.CaptureState().death_log.Contains("x"));
                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }

        // ── Endgame (H-2: CampaignResult was never saved) ────────────────

        [Test]
        public void Endgame_SaveSlot_RoundTrip()
        {
            string dir = TempDir("endgame");
            try
            {
                var a = new EndgameEngine(GameModeKind.Expert, 180);
                a.TriggerVictory(EndgameConditionKind.LongTermSelfSufficiency, "Zero losses in 100 days.", currentDay: 101);

                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var weather = new WeatherSystem(null, 3);
                var temp = new TemperatureSystem(null, weather);
                var rad = new RadiationSystem(needs);

                SaveSystem Make(EndgameEngine sys)
                {
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
                    ss.SetEndgameEngine(sys);
                    return ss;
                }

                Assert.IsTrue(Make(a).Save("endgame"));
                var b = new EndgameEngine(GameModeKind.Story, 120);
                Assert.IsTrue(Make(b).Load("endgame"));

                Assert.IsTrue(b.Result.IsVictory, "victory flag must survive save/load (C-2 class bug: was reset to in-progress)");
                Assert.IsFalse(b.Result.IsDefeat);
                Assert.AreEqual(EndgameConditionKind.LongTermSelfSufficiency, b.Result.ConditionKind);
                Assert.AreEqual(101, b.Result.DaysSurvived);
                Assert.AreEqual(GameModeKind.Expert, b.Result.Mode);
                Assert.AreEqual(180, b.Result.TargetDurationDays);
                Assert.AreEqual("Zero losses in 100 days.", b.Result.OutcomeSummary);
                UnityEngine.Object.DestroyImmediate(profile);
            }
            finally { try { Directory.Delete(dir, true); } catch { } }
        }
    }
}
