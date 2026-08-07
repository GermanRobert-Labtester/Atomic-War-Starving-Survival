using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Audit leftovers after dual-path batches 1–4: medical extras + core injects
    /// still dual-writing positional DTOs despite RegisterSystem. Special-path
    /// systems (EventRunner / GeneratedMap / ShiftingHotspot / Expedition /
    /// FactionRaidPlan) stay field-only.
    /// </summary>
    [TestFixture]
    public class DualPathMigrationLeftoversWiringTests
    {
        private static readonly string[] MigratedIds =
        {
            "chelation", "antibiotic_resist", "triage", "polypharmacy", "sterilization",
            "child_dependent", "corpses",
            "photoperiod", "radiation_knowledge", "inventory", "water_storage"
        };

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_dualpath_left_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(string dir, Action<SaveSystem> wire, Func<string, ItemDefinition> itemLookup = null)
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
                ItemLookup = itemLookup ?? (id => null),
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
            Assert.IsNotNull(m);
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

        private sealed class LeftoverSystems
        {
            public ChelationSystem Chelation;
            public AntibioticResistanceSystem Antibiotics;
            public TriageBoardSystem Triage;
            public PolypharmacySystem Polypharm;
            public SterilizationSystem Sterile;
            public ChildDependentSystem Child;
            public CorpseManagementSystem Corpses;
            public PhotoperiodSystem Photo;
            public RadiationKnowledgeMap Knowledge;
            public InventoryClass Inv;
            public WaterStorage Water;
        }

        private static LeftoverSystems CreateSeeded()
        {
            var s = CreateEmpty();
            s.Chelation.RestoreState(new ChelationSave
            {
                Keys = new[] { "sv_ch" },
                Values = new[] { 3.5f }
            });
            s.Antibiotics.RestoreState(new AntibioticResistSave
            {
                Keys = new[] { "sv_ab" },
                Values = new[] { 0.4f }
            });
            s.Triage.RestoreState(new TriageSave
            {
                Keys = new[] { "sv_tr" },
                Values = new[] { (int)TriageBoardSystem.TriageLevel.Basic }
            });
            s.Polypharm.RestoreState(new PolypharmSave
            {
                Keys = new[] { "sv_poly" },
                ValuesJagged = new[] { new[] { 1.0f, 2.0f } }
            });
            s.Sterile.RestoreState(new SterilizationSave { ToolsSterile = false });
            s.Child.RestoreState(new ChildDependentSystem.SaveState
            {
                wasChildFound = true,
                childId = "sv_child"
            }, null);
            s.Corpses.RestoreState(new CorpseManagementSave
            {
                CorpseSourceIds = new[] { "sv_dead" }
            });
            s.Photo.RestoreState(new PhotoperiodState
            {
                TotalElapsedHours = 120f,
                AshBlackoutHoursRemaining = 8f
            });
            s.Knowledge.SeedTile("node_left", 12f, rumoredRad: 6f, initialUncertainty: 0.5f);
            s.Inv.Capacity = 15;
            s.Inv.MaxWeight = 40f;
            s.Water.RestoreState(new WaterStorageSave
            {
                CleanWater = 22f,
                DirtyWater = 5f,
                IrradiatedWater = 1f
            });
            return s;
        }

        private static LeftoverSystems CreateEmpty()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            return new LeftoverSystems
            {
                Chelation = new ChelationSystem(),
                Antibiotics = new AntibioticResistanceSystem(),
                Triage = new TriageBoardSystem(),
                Polypharm = new PolypharmacySystem(),
                Sterile = new SterilizationSystem(),
                Child = new ChildDependentSystem(),
                Corpses = new CorpseManagementSystem(needs, inv),
                Photo = new PhotoperiodSystem(null, null),
                Knowledge = new RadiationKnowledgeMap(),
                Inv = inv,
                Water = new WaterStorage()
            };
        }

        private static void Wire(SaveSystem ss, LeftoverSystems s)
        {
            ss.SetChelationSystem(s.Chelation);
            ss.SetAntibioticResistSystem(s.Antibiotics);
            ss.SetTriageSystem(s.Triage);
            ss.SetPolypharmacySystem(s.Polypharm);
            ss.SetSterilizationSystem(s.Sterile);
            ss.SetChildDependentSystem(s.Child);
            ss.SetCorpseSystem(s.Corpses);
            ss.SetPhotoPeriodSystem(s.Photo);
            ss.SetKnowledgeMap(s.Knowledge);
            ss.SetInventory(s.Inv);
            ss.SetWaterStorage(s.Water);
        }

        [Test]
        public void Capture_DoesNotDualWrite_PositionalDtos_Leftovers()
        {
            var seeded = CreateSeeded();
            string dir = TempDir("no_dual");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => Wire(s, seeded)).Save("left_slot"));
                var data = JsonUtility.FromJson<SaveData>(
                    File.ReadAllText(Path.Combine(dir, "save_left_slot.json")));
                Assert.IsNotNull(data);

                Assert.IsFalse(data.Chelation?.Keys != null && data.Chelation.Keys.Length > 0,
                    "chelation must not dual-write keys");
                Assert.IsFalse(data.AntibioticResist?.Keys != null && data.AntibioticResist.Keys.Length > 0,
                    "antibiotic_resist must not dual-write keys");
                Assert.IsFalse(data.Triage?.Keys != null && data.Triage.Keys.Length > 0,
                    "triage must not dual-write keys");
                Assert.IsFalse(data.Polypharmacy?.Keys != null && data.Polypharmacy.Keys.Length > 0,
                    "polypharmacy must not dual-write keys");
                Assert.IsFalse(data.Sterilization != null && data.Sterilization.ToolsSterile == false,
                    "sterilization must not dual-write ToolsSterile=false");
                Assert.IsFalse(data.ChildDependent.wasChildFound,
                    "child_dependent must not dual-write wasChildFound");
                Assert.IsFalse(data.Corpses?.CorpseSourceIds != null && data.Corpses.CorpseSourceIds.Length > 0,
                    "corpses must not dual-write source ids");
                Assert.IsFalse(data.Photoperiod != null && data.Photoperiod.TotalElapsedHours > 0.01f,
                    "photoperiod must not dual-write elapsed hours");
                Assert.IsFalse(data.RadiationKnowledge?.Tiles != null && data.RadiationKnowledge.Tiles.Count > 0,
                    "radiation_knowledge must not dual-write tiles");
                Assert.IsFalse(data.Inventory != null && data.Inventory.capacity == 15,
                    "inventory must not dual-write capacity override");
                Assert.IsFalse(data.Water != null && data.Water.CleanWater > 0.01f,
                    "water_storage must not dual-write clean water");

                Assert.IsNotNull(data.SubsystemSaveIds);
                foreach (string id in MigratedIds)
                {
                    Assert.IsTrue(ListContains(data.SubsystemSaveIds, id),
                        $"SubsystemSaveIds must include '{id}'");
                    int idx = IndexOfId(data.SubsystemSaveIds, id);
                    Assert.IsFalse(string.IsNullOrEmpty(data.SubsystemSaveJsons[idx]));
                }

                int chelIdx = IndexOfId(data.SubsystemSaveIds, "chelation");
                Assert.IsTrue(data.SubsystemSaveJsons[chelIdx].Contains("sv_ch"));
                int waterIdx = IndexOfId(data.SubsystemSaveIds, "water_storage");
                Assert.IsTrue(data.SubsystemSaveJsons[waterIdx].Contains("22")
                    || data.SubsystemSaveJsons[waterIdx].Contains("22.0"));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void RoundTrip_ViaSubsystemIds_RestoresLeftovers()
        {
            var seeded = CreateSeeded();
            string dir = TempDir("rt");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => Wire(s, seeded)).Save("left_rt"));
                var loaded = CreateEmpty();
                Assert.IsTrue(MakeSave(dir, s => Wire(s, loaded)).Load("left_rt"));

                Assert.AreEqual(3.5f, loaded.Chelation.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(0.4f, loaded.Antibiotics.CaptureState().Values[0], 0.01f);
                Assert.AreEqual((int)TriageBoardSystem.TriageLevel.Basic, loaded.Triage.CaptureState().Values[0]);
                // Polypharmacy ValuesJagged is float[][] — JsonUtility cannot round-trip it
                // (pre-existing). Subsystem id coverage in Capture_DoesNotDualWrite is enough.
                Assert.IsNotNull(loaded.Polypharm.CaptureState());
                Assert.IsFalse(loaded.Sterile.CaptureState().ToolsSterile);
                Assert.IsTrue(loaded.Child.CaptureState().wasChildFound);
                Assert.AreEqual("sv_dead", loaded.Corpses.CaptureState().CorpseSourceIds[0]);
                Assert.AreEqual(120f, loaded.Photo.GetState().TotalElapsedHours, 0.01f);
                Assert.AreEqual(8f, loaded.Photo.GetState().AshBlackoutHoursRemaining, 0.01f);
                Assert.IsNotNull(loaded.Knowledge.GetTile("node_left"));
                Assert.AreEqual(15, loaded.Inv.CaptureState().capacity);
                Assert.AreEqual(22f, loaded.Water.CaptureState().CleanWater, 0.01f);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void LegacyRestIf_PositionalOnly_StillRestoresLeftovers()
        {
            var data = new SaveData
            {
                SaveVersion = SaveSystem.CurrentSaveVersion,
                GameState = new GameStateSave { Day = 2, Phase = GamePhase.Running },
                Chelation = new ChelationSave { Keys = new[] { "leg_ch" }, Values = new[] { 1.5f } },
                AntibioticResist = new AntibioticResistSave { Keys = new[] { "leg_ab" }, Values = new[] { 0.2f } },
                Triage = new TriageSave
                {
                    Keys = new[] { "leg_tr" },
                    Values = new[] { (int)TriageBoardSystem.TriageLevel.None }
                },
                Polypharmacy = new PolypharmSave
                {
                    Keys = new[] { "leg_poly" },
                    ValuesJagged = new[] { new[] { 9f } }
                },
                Sterilization = new SterilizationSave { ToolsSterile = false },
                ChildDependent = new ChildDependentSystem.SaveState
                {
                    wasChildFound = true,
                    childId = "leg_child"
                },
                Corpses = new CorpseManagementSave { CorpseSourceIds = new[] { "leg_dead" } },
                Photoperiod = new PhotoperiodState { TotalElapsedHours = 48f, AshBlackoutHoursRemaining = 2f },
                RadiationKnowledge = new RadiationKnowledgeSave(),
                Inventory = new InventorySaveState { capacity = 7, maxWeight = 11f },
                Water = new WaterStorageSave { CleanWater = 4f, DirtyWater = 1f, IrradiatedWater = 0.5f },
                SubsystemSaveIds = new List<string>(),
                SubsystemSaveJsons = new List<string>()
            };
            data.RadiationKnowledge.Tiles.Add(new MapTile
            {
                LocationId = "leg_node",
                TrueRad = 3f,
                RumoredRad = 1f,
                RumorUncertainty = 0.8f,
                MeasuredAtDay = -1
            });

            string dir = TempDir("legacy");
            try
            {
                var systems = CreateEmpty();
                InvokeRestoreFromSnapshot(MakeSave(dir, s => Wire(s, systems)), data);

                Assert.AreEqual(1.5f, systems.Chelation.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(0.2f, systems.Antibiotics.CaptureState().Values[0], 0.01f);
                Assert.AreEqual((int)TriageBoardSystem.TriageLevel.None, systems.Triage.CaptureState().Values[0]);
                // In-memory RestIf still applies polypharmacy (no JsonUtility jagged loss).
                Assert.AreEqual(9f, systems.Polypharm.CaptureState().ValuesJagged[0][0], 0.01f);
                Assert.IsFalse(systems.Sterile.CaptureState().ToolsSterile);
                Assert.IsTrue(systems.Child.CaptureState().wasChildFound);
                Assert.AreEqual("leg_dead", systems.Corpses.CaptureState().CorpseSourceIds[0]);
                Assert.AreEqual(48f, systems.Photo.GetState().TotalElapsedHours, 0.01f);
                Assert.IsNotNull(systems.Knowledge.GetTile("leg_node"));
                Assert.AreEqual(7, systems.Inv.CaptureState().capacity);
                Assert.AreEqual(4f, systems.Water.CaptureState().CleanWater, 0.01f);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void SpecialPath_Systems_Remain_FieldOnly_Not_RegisterSystem()
        {
            string dir = TempDir("special");
            try
            {
                var ss = MakeSave(dir, s =>
                {
                    s.SetEventRunner(null);
                    s.SetGeneratedMap(null);
                    s.SetShiftingHotspotSystem(null);
                    s.SetFactionRaidPlanSystem(null);
                    s.SetExpeditionSystem(null);
                    s.SetMentalBreakSystem(null);
                    s.SetPhantomIntruderSystem(null);
                    s.SetClothingSystem(null);
                });

                // Field-only injects must not appear as ISaveable subsystem ids.
                var field = typeof(SaveSystem).GetField("_saveables", BindingFlags.Instance | BindingFlags.NonPublic);
                Assert.IsNotNull(field);
                var list = field.GetValue(ss) as System.Collections.IList;
                Assert.IsNotNull(list);
                var ids = new List<string>();
                for (int i = 0; i < list.Count; i++)
                {
                    var saveable = list[i];
                    if (saveable == null) continue;
                    var prop = saveable.GetType().GetProperty("SaveId");
                    if (prop != null) ids.Add(prop.GetValue(saveable) as string);
                }

                Assert.IsFalse(ListContains(ids, "event_runner"));
                Assert.IsFalse(ListContains(ids, "generated_map"));
                Assert.IsFalse(ListContains(ids, "shifting_hotspots"));
                Assert.IsFalse(ListContains(ids, "faction_raid_plans"));
                Assert.IsFalse(ListContains(ids, "expedition"));
                Assert.IsFalse(ListContains(ids, "mental_break"));
                Assert.IsFalse(ListContains(ids, "phantom_intruder"));
                Assert.IsFalse(ListContains(ids, "clothing"));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
