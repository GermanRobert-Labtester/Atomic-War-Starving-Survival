using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dual-path CapIf → RegisterSystem migration batch:
    /// cartography / tracker / dead_drops / medical family.
    /// Capture no longer dual-writes positional SaveData DTOs; RestIf remains
    /// for pre-migration saves. Complex special-path systems stay field-only.
    /// </summary>
    [TestFixture]
    public class DualPathMigrationWiringTests
    {
        private static readonly string[] MigratedIds =
        {
            "cartography", "tracker", "dead_drops",
            "medical", "blood_transfusion", "amputation", "scurvy", "mutagenesis"
        };

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_dualpath_" + tag + "_" + Guid.NewGuid().ToString("N"));
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

        private static void InvokeRestoreFromSnapshot(SaveSystem ss, SaveData data)
        {
            var m = typeof(SaveSystem).GetMethod(
                "RestoreFromSnapshot", BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.IsNotNull(m, "RestoreFromSnapshot must exist");
            m.Invoke(ss, new object[] { data });
        }

        private static bool ListContains(List<string> ids, string id)
        {
            if (ids == null) return false;
            for (int i = 0; i < ids.Count; i++)
                if (string.Equals(ids[i], id, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static int IndexOfId(List<string> ids, string id)
        {
            if (ids == null) return -1;
            for (int i = 0; i < ids.Count; i++)
                if (string.Equals(ids[i], id, StringComparison.Ordinal))
                    return i;
            return -1;
        }

        [Test]
        public void Capture_DoesNotDualWrite_PositionalDtos_ForMigratedBatch()
        {
            var carto = new CartographySystem();
            carto.RestoreState(new CartographySave
            {
                ChartedNodeIds = new[] { "node_alpha" },
                PencilDurability = 5,
                PaperStock = 3
            });
            var tracker = new TrackerSystem(new System.Random(1));
            tracker.RestoreState(new TrackerSave
            {
                Tracks = new[]
                {
                    new TrackedEntrySave
                    {
                        FactionId = "raiders",
                        HoursUntilEvent = 12f,
                        TrackedSurvivorId = "sv_1"
                    }
                }
            });
            var drop = new DeadDropSystem();
            drop.RestoreState(new DeadDropSave
            {
                DropSeq = 4,
                DeadDropNodeIds = new[] { "drop_node" },
                Drops = Array.Empty<DeadDropEntrySave>()
            });

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var medical = new MedicalSystem(needs);
            medical.RestoreState(new MedicalSystemSave
            {
                BySurvivor = Array.Empty<SurvivorAfflictionsSave>()
            });
            var blood = new BloodTransfusionSystem();
            blood.RestoreState(new BloodTransfusionSave
            {
                BloodTypeKeys = new[] { "sv_1" },
                BloodTypeValues = new[] { 1 },
                TestedSurvivorIds = new[] { "sv_1" }
            });
            var amp = new AmputationSystem();
            amp.RestoreState(new AmputationSave { AmputeeIds = new[] { "sv_2" } });
            var scurvy = new ScurvySystem();
            scurvy.RestoreState(new ScurvySave
            {
                DaysWithoutCKeys = new[] { "sv_1" },
                DaysWithoutCValues = new[] { 9f },
                HasScurvyIds = new[] { "sv_1" },
                HasToothacheIds = Array.Empty<string>()
            });
            var mut = new RadiationMutagenesisSystem();
            mut.RestoreState(new MutagenesisSave
            {
                StageKeys = new[] { "sv_1" },
                StageValues = new[] { 2 },
                HairLossAppliedIds = new[] { "sv_1" }
            });

            string dir = TempDir("no_dual");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetCartographySystem(carto);
                    s.SetTrackerSystem(tracker);
                    s.SetDeadDropSystem(drop);
                    s.SetMedicalSystem(medical);
                    s.SetBloodTransfusionSystem(blood);
                    s.SetAmputationSystem(amp);
                    s.SetScurvySystem(scurvy);
                    s.SetMutagenesisSystem(mut);
                });
                Assert.IsTrue(ss.Save("mig_slot"));

                string path = Path.Combine(dir, "save_mig_slot.json");
                Assert.IsTrue(File.Exists(path));
                string raw = File.ReadAllText(path);
                var data = JsonUtility.FromJson<SaveData>(raw);
                Assert.IsNotNull(data);

                // Positional dual-write removed. JsonUtility may materialize empty
                // nested objects for omitted nulls on some versions — treat as
                // "not written" when the seeded payload is absent.
                Assert.IsFalse(
                    data.Cartography?.ChartedNodeIds != null && data.Cartography.ChartedNodeIds.Length > 0,
                    "cartography must not dual-write charted nodes to positional DTO");
                Assert.IsFalse(
                    data.Tracker?.Tracks != null && data.Tracker.Tracks.Length > 0,
                    "tracker must not dual-write tracks to positional DTO");
                Assert.IsFalse(
                    data.DeadDrops != null && data.DeadDrops.DropSeq == 4,
                    "dead_drops must not dual-write DropSeq to positional DTO");
                Assert.IsFalse(
                    data.BloodTransfusion?.BloodTypeKeys != null && data.BloodTransfusion.BloodTypeKeys.Length > 0,
                    "blood_transfusion must not dual-write blood types to positional DTO");
                Assert.IsFalse(
                    data.Amputation?.AmputeeIds != null && data.Amputation.AmputeeIds.Length > 0,
                    "amputation must not dual-write amputees to positional DTO");
                Assert.IsFalse(
                    data.Scurvy?.HasScurvyIds != null && data.Scurvy.HasScurvyIds.Length > 0,
                    "scurvy must not dual-write has-scurvy to positional DTO");
                Assert.IsFalse(
                    data.Mutagenesis?.StageKeys != null && data.Mutagenesis.StageKeys.Length > 0,
                    "mutagenesis must not dual-write stages to positional DTO");

                // Subsystem registry owns capture — real payload lives here.
                Assert.IsNotNull(data.SubsystemSaveIds);
                Assert.IsNotNull(data.SubsystemSaveJsons);
                foreach (string id in MigratedIds)
                {
                    Assert.IsTrue(ListContains(data.SubsystemSaveIds, id),
                        $"SubsystemSaveIds must include '{id}'");
                    int idx = IndexOfId(data.SubsystemSaveIds, id);
                    Assert.GreaterOrEqual(idx, 0);
                    Assert.IsFalse(string.IsNullOrEmpty(data.SubsystemSaveJsons[idx]),
                        $"SubsystemSaveJsons[{id}] must be non-empty");
                    Assert.AreNotEqual("null", data.SubsystemSaveJsons[idx]);
                }

                int cartoIdx = IndexOfId(data.SubsystemSaveIds, "cartography");
                Assert.IsTrue(data.SubsystemSaveJsons[cartoIdx].Contains("node_alpha"),
                    "cartography subsystem JSON must carry charted node payload");
                int trackerIdx = IndexOfId(data.SubsystemSaveIds, "tracker");
                Assert.IsTrue(data.SubsystemSaveJsons[trackerIdx].Contains("raiders"),
                    "tracker subsystem JSON must carry faction payload");
                int dropIdx = IndexOfId(data.SubsystemSaveIds, "dead_drops");
                Assert.IsTrue(data.SubsystemSaveJsons[dropIdx].Contains("drop_node")
                    || data.SubsystemSaveJsons[dropIdx].Contains("\"DropSeq\":4")
                    || data.SubsystemSaveJsons[dropIdx].Contains("\"DropSeq\": 4"),
                    "dead_drops subsystem JSON must carry drop payload");
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void RoundTrip_ViaSubsystemIds_RestoresMigratedBatch()
        {
            var carto = new CartographySystem();
            carto.RestoreState(new CartographySave
            {
                ChartedNodeIds = new[] { "node_bravo" },
                PencilDurability = 7,
                PaperStock = 2
            });
            var tracker = new TrackerSystem(new System.Random(2));
            tracker.RestoreState(new TrackerSave
            {
                Tracks = new[]
                {
                    new TrackedEntrySave
                    {
                        FactionId = "warlords",
                        HoursUntilEvent = 6f,
                        TrackedSurvivorId = "sv_a"
                    }
                }
            });
            var drop = new DeadDropSystem();
            drop.RestoreState(new DeadDropSave
            {
                DropSeq = 9,
                DeadDropNodeIds = new[] { "cache_7" },
                Drops = Array.Empty<DeadDropEntrySave>()
            });

            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var medical = new MedicalSystem(needs);
            var blood = new BloodTransfusionSystem();
            blood.RestoreState(new BloodTransfusionSave
            {
                BloodTypeKeys = new[] { "sv_a" },
                BloodTypeValues = new[] { 2 },
                TestedSurvivorIds = new[] { "sv_a" }
            });
            var amp = new AmputationSystem();
            amp.RestoreState(new AmputationSave { AmputeeIds = new[] { "sv_b" } });
            var scurvy = new ScurvySystem();
            scurvy.RestoreState(new ScurvySave
            {
                DaysWithoutCKeys = new[] { "sv_a" },
                DaysWithoutCValues = new[] { 14f },
                HasScurvyIds = new[] { "sv_a" },
                HasToothacheIds = new[] { "sv_a" }
            });
            var mut = new RadiationMutagenesisSystem();
            mut.RestoreState(new MutagenesisSave
            {
                StageKeys = new[] { "sv_a" },
                StageValues = new[] { 1 },
                HairLossAppliedIds = Array.Empty<string>()
            });

            string dir = TempDir("rt");
            try
            {
                var saveSs = MakeSave(dir, s =>
                {
                    s.SetCartographySystem(carto);
                    s.SetTrackerSystem(tracker);
                    s.SetDeadDropSystem(drop);
                    s.SetMedicalSystem(medical);
                    s.SetBloodTransfusionSystem(blood);
                    s.SetAmputationSystem(amp);
                    s.SetScurvySystem(scurvy);
                    s.SetMutagenesisSystem(mut);
                });
                Assert.IsTrue(saveSs.Save("rt_slot"));

                var carto2 = new CartographySystem();
                var tracker2 = new TrackerSystem(new System.Random(99));
                var drop2 = new DeadDropSystem();
                var medical2 = new MedicalSystem(needs);
                var blood2 = new BloodTransfusionSystem();
                var amp2 = new AmputationSystem();
                var scurvy2 = new ScurvySystem();
                var mut2 = new RadiationMutagenesisSystem();

                var loadSs = MakeSave(dir, s =>
                {
                    s.SetCartographySystem(carto2);
                    s.SetTrackerSystem(tracker2);
                    s.SetDeadDropSystem(drop2);
                    s.SetMedicalSystem(medical2);
                    s.SetBloodTransfusionSystem(blood2);
                    s.SetAmputationSystem(amp2);
                    s.SetScurvySystem(scurvy2);
                    s.SetMutagenesisSystem(mut2);
                });
                Assert.IsTrue(loadSs.Load("rt_slot"));

                Assert.IsTrue(carto2.IsCharted("node_bravo"));
                Assert.AreEqual(7, carto2.PencilDurability);
                Assert.AreEqual(2, carto2.PaperStock);

                Assert.IsTrue(tracker2.IsTracked("warlords"));
                Assert.AreEqual(1, tracker2.ActiveTracks.Count);
                Assert.AreEqual("sv_a", tracker2.ActiveTracks[0].TrackedSurvivorId);

                var dropState = drop2.CaptureState();
                Assert.AreEqual(9, dropState.DropSeq);
                Assert.IsNotNull(dropState.DeadDropNodeIds);
                Assert.AreEqual(1, dropState.DeadDropNodeIds.Length);
                Assert.AreEqual("cache_7", dropState.DeadDropNodeIds[0]);

                var bloodState = blood2.CaptureState();
                Assert.IsNotNull(bloodState.BloodTypeKeys);
                Assert.AreEqual("sv_a", bloodState.BloodTypeKeys[0]);
                Assert.AreEqual(2, bloodState.BloodTypeValues[0]);

                var ampState = amp2.CaptureState();
                Assert.AreEqual("sv_b", ampState.AmputeeIds[0]);

                var scurvyState = scurvy2.CaptureState();
                Assert.AreEqual("sv_a", scurvyState.HasScurvyIds[0]);
                Assert.AreEqual("sv_a", scurvyState.HasToothacheIds[0]);

                var mutState = mut2.CaptureState();
                Assert.AreEqual("sv_a", mutState.StageKeys[0]);
                Assert.AreEqual(1, mutState.StageValues[0]);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void LegacyRestIf_PositionalOnly_StillRestoresMigratedBatch()
        {
            // Pre-migration save shape: positional DTOs filled, no subsystem entries.
            var data = new SaveData
            {
                SaveVersion = SaveSystem.CurrentSaveVersion,
                GameState = new GameStateSave { Day = 3, Phase = GamePhase.Running },
                Cartography = new CartographySave
                {
                    ChartedNodeIds = new[] { "legacy_node" },
                    PencilDurability = 4,
                    PaperStock = 1
                },
                Tracker = new TrackerSave
                {
                    Tracks = new[]
                    {
                        new TrackedEntrySave
                        {
                            FactionId = "legacy_faction",
                            HoursUntilEvent = 18f,
                            TrackedSurvivorId = "sv_legacy"
                        }
                    }
                },
                DeadDrops = new DeadDropSave
                {
                    DropSeq = 2,
                    DeadDropNodeIds = new[] { "legacy_drop" },
                    Drops = Array.Empty<DeadDropEntrySave>()
                },
                Medical = new MedicalSystemSave
                {
                    BySurvivor = Array.Empty<SurvivorAfflictionsSave>()
                },
                BloodTransfusion = new BloodTransfusionSave
                {
                    BloodTypeKeys = new[] { "sv_legacy" },
                    BloodTypeValues = new[] { 0 },
                    TestedSurvivorIds = new[] { "sv_legacy" }
                },
                Amputation = new AmputationSave { AmputeeIds = new[] { "sv_leg" } },
                Scurvy = new ScurvySave
                {
                    DaysWithoutCKeys = new[] { "sv_legacy" },
                    DaysWithoutCValues = new[] { 5f },
                    HasScurvyIds = Array.Empty<string>(),
                    HasToothacheIds = Array.Empty<string>()
                },
                Mutagenesis = new MutagenesisSave
                {
                    StageKeys = new[] { "sv_legacy" },
                    StageValues = new[] { 3 },
                    HairLossAppliedIds = new[] { "sv_legacy" }
                },
                SubsystemSaveIds = new List<string>(),
                SubsystemSaveJsons = new List<string>()
            };

            string dir = TempDir("legacy");
            try
            {
                var carto = new CartographySystem();
                var tracker = new TrackerSystem();
                var drop = new DeadDropSystem();
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var medical = new MedicalSystem(needs);
                var blood = new BloodTransfusionSystem();
                var amp = new AmputationSystem();
                var scurvy = new ScurvySystem();
                var mut = new RadiationMutagenesisSystem();

                var ss = MakeSave(dir, s =>
                {
                    s.SetCartographySystem(carto);
                    s.SetTrackerSystem(tracker);
                    s.SetDeadDropSystem(drop);
                    s.SetMedicalSystem(medical);
                    s.SetBloodTransfusionSystem(blood);
                    s.SetAmputationSystem(amp);
                    s.SetScurvySystem(scurvy);
                    s.SetMutagenesisSystem(mut);
                });

                InvokeRestoreFromSnapshot(ss, data);

                Assert.IsTrue(carto.IsCharted("legacy_node"));
                Assert.AreEqual(4, carto.PencilDurability);
                Assert.IsTrue(tracker.IsTracked("legacy_faction"));
                Assert.AreEqual(2, drop.CaptureState().DropSeq);
                Assert.AreEqual("sv_legacy", blood.CaptureState().BloodTypeKeys[0]);
                Assert.AreEqual("sv_leg", amp.CaptureState().AmputeeIds[0]);
                Assert.AreEqual(5f, scurvy.CaptureState().DaysWithoutCValues[0], 0.001f);
                Assert.AreEqual(3, mut.CaptureState().StageValues[0]);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
