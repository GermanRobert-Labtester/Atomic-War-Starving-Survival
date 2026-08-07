using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dual-path CapIf → RegisterSystem migration batch 2:
    /// flooded_node / ecosystem / house_to_bunker / location_quest +
    /// full shelter tactical family (structural_integrity … noise).
    /// Capture no longer dual-writes positional DTOs; RestIf remains for
    /// pre-migration saves. Complex special-path systems stay field-only.
    /// </summary>
    [TestFixture]
    public class DualPathMigrationBatch2WiringTests
    {
        private static readonly string[] MigratedIds =
        {
            "flooded_node", "ecosystem", "house_to_bunker", "location_quest",
            "structural_integrity", "waste", "vermin", "jury_rig", "freeze_pipe",
            "excavation", "flooding", "hidden_storage", "ceiling_collapse",
            "perimeter_trap", "tunneling", "hatch_visibility", "escape_hatch",
            "material_shielding", "airlock", "noise"
        };

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_dualpath_b2_" + tag + "_" + Guid.NewGuid().ToString("N"));
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

        private sealed class Batch2Systems
        {
            public FloodedNodeSystem Flooded;
            public MutatedEcosystemSystem Ecosystem;
            public HouseToBunkerSystem House;
            public LocationQuestSystem Quests;
            public StructuralIntegritySystem Integrity;
            public WasteSystem Waste;
            public VerminSystem Vermin;
            public JuryRigSystem Jury;
            public FreezePipeSystem Freeze;
            public ExcavationSystem Excavation;
            public RoomFloodingSystem Flooding;
            public HiddenStorageSystem Hidden;
            public CeilingCollapseSystem Ceiling;
            public PerimeterTrapSystem Perimeter;
            public TunnelingSystem Tunneling;
            public HatchVisibilitySystem HatchVis;
            public EscapeHatchSystem Escape;
            public MaterialShieldingSystem Shielding;
            public AirlockSystem Airlock;
            public NoiseSystem Noise;
        }

        private static Batch2Systems CreateSeeded()
        {
            var s = new Batch2Systems
            {
                Flooded = new FloodedNodeSystem(),
                Ecosystem = new MutatedEcosystemSystem(),
                House = new HouseToBunkerSystem(),
                Quests = new LocationQuestSystem(),
                Integrity = new StructuralIntegritySystem(),
                Waste = new WasteSystem(),
                Vermin = new VerminSystem(),
                Jury = new JuryRigSystem(),
                Freeze = new FreezePipeSystem(),
                Excavation = new ExcavationSystem(),
                Flooding = new RoomFloodingSystem(),
                Hidden = new HiddenStorageSystem(),
                Ceiling = new CeilingCollapseSystem(),
                Perimeter = new PerimeterTrapSystem(),
                Tunneling = new TunnelingSystem(),
                HatchVis = new HatchVisibilitySystem(),
                Escape = new EscapeHatchSystem(),
                Shielding = new MaterialShieldingSystem(),
                Airlock = new AirlockSystem(),
                Noise = new NoiseSystem()
            };

            s.Flooded.RestoreState(new FloodedNodeSave { FloodedNodeIds = new[] { "flood_node_x" } });
            s.Ecosystem.RestoreState(new EcosystemSave { RadiationExposureDays = 12.5f });
            s.House.RestoreState(new HouseToBunkerSave
            {
                HouseDurability = 42f,
                HouseDestroyed = false,
                DebrisCleared = false,
                DebrisClearHoursRemaining = 3f,
                DebrisClearHoursTotal = 6f
            });
            // LocationQuestSystem only applies progress to seeded definition node ids.
            s.Quests.RestoreState(new LocationQuestSave
            {
                Quests = new[]
                {
                    new QuestEntrySave
                    {
                        NodeId = "node_hospital",
                        QuestType = "hospital_centrifuge",
                        CurrentStage = 2,
                        IsCompleted = false,
                        IsFailed = false,
                        TemporaryRewardHoursRemaining = 0f
                    }
                }
            });
            s.Integrity.RestoreState(new StructuralIntegritySave
            {
                Integrity = 55f,
                HasDustLeaks = true,
                CavedInRoomIds = new[] { "room_cave" }
            });
            s.Waste.RestoreState(new WasteSystemSave { AccumulatedWaste = 33f, Hygiene = 40f });
            s.Vermin.RestoreState(new VerminSave { PestLevel = 7f });
            s.Jury.RestoreState(new JuryRigSave
            {
                RiggedModuleIds = new[] { "mod_x" },
                RiggedHours = new[] { 2f },
                DailyFailureAccumulator = 1.5f
            });
            s.Freeze.RestoreState(new FreezePipeSave
            {
                FrozenHours = 9f,
                IsFrozen = true,
                HasBurst = false
            });
            s.Excavation.RestoreState(new ExcavationSave
            {
                Rooms = Array.Empty<ExcavationRoomSave>(),
                PendingRubbleUnits = 5
            });
            s.Flooding.RestoreState(new FloodingSave
            {
                FloodedRoomIds = new[] { "room_a" },
                FloodAccumulator = 2.5f
            });
            s.Hidden.RestoreState(new HiddenStorageSave
            {
                ItemIds = new[] { "scrap" },
                Amounts = new[] { 3 }
            });
            s.Ceiling.RestoreState(new CeilingCollapseSave
            {
                RoomTileKeys = Array.Empty<string>(),
                RoomTileValues = Array.Empty<float>(),
                LoadMultKeys = Array.Empty<string>(),
                LoadMultValues = Array.Empty<float>(),
                CollapsedRoomIds = new[] { "room_b" }
            });
            s.Perimeter.RestoreState(new PerimeterTrapSave
            {
                BearTraps = 2,
                TinCanAlarms = 1,
                Tripwires = 0,
                RaidWarningActive = false,
                RaidWarningRemaining = 0f,
                LastDeployerId = "sv_trap"
            });
            s.Tunneling.RestoreState(new TunnelingSave
            {
                NeighborBreached = true,
                TunnelProgress = 0.4f
            });
            s.HatchVis.RestoreState(new HatchVisibilitySave { Visibility = 0.75f });
            s.Escape.RestoreState(new EscapeHatchSave
            {
                EscapeHatchBuilt = true,
                EvacTriggered = false,
                ExcavationProgress = 0.6f
            });
            s.Shielding.RestoreState(new MaterialShieldingSave
            {
                RoomIds = new[] { "r1" },
                Materials = new[] { 1 }
            });
            s.Airlock.RestoreState(new AirlockSave
            {
                AirlockExists = true,
                InnerDoorSealed = true,
                ScavengerInAirlock = false,
                AirlockContamination = 0.2f
            });
            s.Noise.RestoreState(new NoiseSave { NoiseLevel = 8f });
            return s;
        }

        private static void Wire(SaveSystem ss, Batch2Systems s)
        {
            ss.SetFloodedNodeSystem(s.Flooded);
            ss.SetEcosystemSystem(s.Ecosystem);
            ss.SetHouseToBunkerSystem(s.House);
            ss.SetLocationQuestSystem(s.Quests);
            ss.SetStructuralIntegritySystem(s.Integrity);
            ss.SetWasteSystem(s.Waste);
            ss.SetVerminSystem(s.Vermin);
            ss.SetJuryRigSystem(s.Jury);
            ss.SetFreezePipeSystem(s.Freeze);
            ss.SetExcavationSystem(s.Excavation);
            ss.SetFloodingSystem(s.Flooding);
            ss.SetHiddenStorageSystem(s.Hidden);
            ss.SetCeilingCollapseSystem(s.Ceiling);
            ss.SetPerimeterTrapSystem(s.Perimeter);
            ss.SetTunnelingSystem(s.Tunneling);
            ss.SetHatchVisibilitySystem(s.HatchVis);
            ss.SetEscapeHatchSystem(s.Escape);
            ss.SetMaterialShieldingSystem(s.Shielding);
            ss.SetAirlockSystem(s.Airlock);
            ss.SetNoiseSystem(s.Noise);
        }

        private static Batch2Systems CreateEmpty()
        {
            return new Batch2Systems
            {
                Flooded = new FloodedNodeSystem(),
                Ecosystem = new MutatedEcosystemSystem(),
                House = new HouseToBunkerSystem(),
                Quests = new LocationQuestSystem(),
                Integrity = new StructuralIntegritySystem(),
                Waste = new WasteSystem(),
                Vermin = new VerminSystem(),
                Jury = new JuryRigSystem(),
                Freeze = new FreezePipeSystem(),
                Excavation = new ExcavationSystem(),
                Flooding = new RoomFloodingSystem(),
                Hidden = new HiddenStorageSystem(),
                Ceiling = new CeilingCollapseSystem(),
                Perimeter = new PerimeterTrapSystem(),
                Tunneling = new TunnelingSystem(),
                HatchVis = new HatchVisibilitySystem(),
                Escape = new EscapeHatchSystem(),
                Shielding = new MaterialShieldingSystem(),
                Airlock = new AirlockSystem(),
                Noise = new NoiseSystem()
            };
        }

        [Test]
        public void Capture_DoesNotDualWrite_PositionalDtos_Batch2()
        {
            var seeded = CreateSeeded();
            string dir = TempDir("no_dual");
            try
            {
                var ss = MakeSave(dir, s => Wire(s, seeded));
                Assert.IsTrue(ss.Save("b2_slot"));

                string path = Path.Combine(dir, "save_b2_slot.json");
                Assert.IsTrue(File.Exists(path));
                var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
                Assert.IsNotNull(data);

                // Positional dual-write removed (JsonUtility may materialize empty objects).
                Assert.IsFalse(
                    data.FloodedNodes?.FloodedNodeIds != null && data.FloodedNodes.FloodedNodeIds.Length > 0,
                    "flooded_node must not dual-write node ids");
                Assert.IsFalse(
                    data.Ecosystem != null && data.Ecosystem.RadiationExposureDays > 0.01f,
                    "ecosystem must not dual-write exposure days");
                Assert.IsFalse(
                    data.HouseToBunker != null && Mathf.Abs(data.HouseToBunker.HouseDurability - 42f) < 0.01f,
                    "house_to_bunker must not dual-write durability");
                Assert.IsFalse(
                    data.LocationQuests?.Quests != null && data.LocationQuests.Quests.Length > 0,
                    "location_quest must not dual-write quests");
                Assert.IsFalse(
                    data.StructuralIntegrity != null && Mathf.Abs(data.StructuralIntegrity.Integrity - 55f) < 0.01f,
                    "structural_integrity must not dual-write integrity");
                Assert.IsFalse(
                    data.Waste != null && data.Waste.AccumulatedWaste > 0.01f,
                    "waste must not dual-write accumulated waste");
                Assert.IsFalse(
                    data.Vermin != null && data.Vermin.PestLevel > 0.01f,
                    "vermin must not dual-write pest level");
                Assert.IsFalse(
                    data.JuryRig?.RiggedModuleIds != null && data.JuryRig.RiggedModuleIds.Length > 0,
                    "jury_rig must not dual-write rigged modules");
                Assert.IsFalse(
                    data.FreezePipe != null && data.FreezePipe.IsFrozen,
                    "freeze_pipe must not dual-write frozen flag");
                Assert.IsFalse(
                    data.Excavation != null && data.Excavation.PendingRubbleUnits == 5,
                    "excavation must not dual-write rubble");
                Assert.IsFalse(
                    data.Flooding?.FloodedRoomIds != null && data.Flooding.FloodedRoomIds.Length > 0,
                    "flooding must not dual-write flooded rooms");
                Assert.IsFalse(
                    data.HiddenStorage?.ItemIds != null && data.HiddenStorage.ItemIds.Length > 0,
                    "hidden_storage must not dual-write items");
                Assert.IsFalse(
                    data.CeilingCollapse?.CollapsedRoomIds != null && data.CeilingCollapse.CollapsedRoomIds.Length > 0,
                    "ceiling_collapse must not dual-write collapsed rooms");
                Assert.IsFalse(
                    data.PerimeterTraps != null && data.PerimeterTraps.BearTraps == 2,
                    "perimeter_trap must not dual-write bear traps");
                Assert.IsFalse(
                    data.Tunneling != null && data.Tunneling.NeighborBreached,
                    "tunneling must not dual-write breach flag");
                Assert.IsFalse(
                    data.HatchVisibility != null && Mathf.Abs(data.HatchVisibility.Visibility - 0.75f) < 0.01f,
                    "hatch_visibility must not dual-write visibility");
                Assert.IsFalse(
                    data.EscapeHatch != null && data.EscapeHatch.EscapeHatchBuilt,
                    "escape_hatch must not dual-write built flag");
                Assert.IsFalse(
                    data.MaterialShielding?.RoomIds != null && data.MaterialShielding.RoomIds.Length > 0,
                    "material_shielding must not dual-write rooms");
                Assert.IsFalse(
                    data.Airlock != null && data.Airlock.AirlockExists,
                    "airlock must not dual-write exists flag");
                Assert.IsFalse(
                    data.Noise != null && data.Noise.NoiseLevel > 0.01f,
                    "noise must not dual-write noise level");

                Assert.IsNotNull(data.SubsystemSaveIds);
                Assert.IsNotNull(data.SubsystemSaveJsons);
                foreach (string id in MigratedIds)
                {
                    Assert.IsTrue(ListContains(data.SubsystemSaveIds, id),
                        $"SubsystemSaveIds must include '{id}'");
                    int idx = IndexOfId(data.SubsystemSaveIds, id);
                    Assert.IsFalse(string.IsNullOrEmpty(data.SubsystemSaveJsons[idx]),
                        $"SubsystemSaveJsons[{id}] must be non-empty");
                }

                int floodIdx = IndexOfId(data.SubsystemSaveIds, "flooded_node");
                Assert.IsTrue(data.SubsystemSaveJsons[floodIdx].Contains("flood_node_x"));
                int ecoIdx = IndexOfId(data.SubsystemSaveIds, "ecosystem");
                Assert.IsTrue(
                    data.SubsystemSaveJsons[ecoIdx].Contains("12.5")
                    || data.SubsystemSaveJsons[ecoIdx].Contains("12.5"));
                int wasteIdx = IndexOfId(data.SubsystemSaveIds, "waste");
                Assert.IsTrue(data.SubsystemSaveJsons[wasteIdx].Contains("33"));
                int noiseIdx = IndexOfId(data.SubsystemSaveIds, "noise");
                Assert.IsTrue(data.SubsystemSaveJsons[noiseIdx].Contains("8"));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void RoundTrip_ViaSubsystemIds_RestoresBatch2()
        {
            var seeded = CreateSeeded();
            string dir = TempDir("rt");
            try
            {
                var saveSs = MakeSave(dir, s => Wire(s, seeded));
                Assert.IsTrue(saveSs.Save("b2_rt"));

                var loaded = CreateEmpty();
                var loadSs = MakeSave(dir, s => Wire(s, loaded));
                Assert.IsTrue(loadSs.Load("b2_rt"));

                var floodState = loaded.Flooded.CaptureState();
                Assert.IsNotNull(floodState.FloodedNodeIds);
                Assert.AreEqual("flood_node_x", floodState.FloodedNodeIds[0]);

                Assert.AreEqual(12.5f, loaded.Ecosystem.CaptureState().RadiationExposureDays, 0.01f);
                Assert.AreEqual(42f, loaded.House.CaptureState().HouseDurability, 0.01f);

                var questState = loaded.Quests.CaptureState();
                Assert.IsNotNull(questState.Quests);
                Assert.GreaterOrEqual(questState.Quests.Length, 1);
                bool foundQuest = false;
                for (int i = 0; i < questState.Quests.Length; i++)
                {
                    if (questState.Quests[i] != null
                        && questState.Quests[i].NodeId == "node_hospital"
                        && questState.Quests[i].CurrentStage == 2)
                    {
                        foundQuest = true;
                        break;
                    }
                }
                Assert.IsTrue(foundQuest, "location quest stage must round-trip");

                Assert.AreEqual(55f, loaded.Integrity.CaptureState().Integrity, 0.01f);
                Assert.AreEqual(33f, loaded.Waste.CaptureState().AccumulatedWaste, 0.01f);
                Assert.AreEqual(7f, loaded.Vermin.CaptureState().PestLevel, 0.01f);
                Assert.AreEqual("mod_x", loaded.Jury.CaptureState().RiggedModuleIds[0]);
                Assert.IsTrue(loaded.Freeze.CaptureState().IsFrozen);
                Assert.AreEqual(5, loaded.Excavation.CaptureState().PendingRubbleUnits);
                Assert.AreEqual("room_a", loaded.Flooding.CaptureState().FloodedRoomIds[0]);
                Assert.AreEqual("scrap", loaded.Hidden.CaptureState().ItemIds[0]);
                Assert.AreEqual("room_b", loaded.Ceiling.CaptureState().CollapsedRoomIds[0]);
                Assert.AreEqual(2, loaded.Perimeter.CaptureState().BearTraps);
                Assert.IsTrue(loaded.Tunneling.CaptureState().NeighborBreached);
                Assert.AreEqual(0.75f, loaded.HatchVis.CaptureState().Visibility, 0.01f);
                Assert.IsTrue(loaded.Escape.CaptureState().EscapeHatchBuilt);
                Assert.AreEqual("r1", loaded.Shielding.CaptureState().RoomIds[0]);
                Assert.IsTrue(loaded.Airlock.CaptureState().AirlockExists);
                Assert.AreEqual(8f, loaded.Noise.CaptureState().NoiseLevel, 0.01f);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void LegacyRestIf_PositionalOnly_StillRestoresBatch2()
        {
            var data = new SaveData
            {
                SaveVersion = SaveSystem.CurrentSaveVersion,
                GameState = new GameStateSave { Day = 4, Phase = GamePhase.Running },
                FloodedNodes = new FloodedNodeSave { FloodedNodeIds = new[] { "legacy_flood" } },
                Ecosystem = new EcosystemSave { RadiationExposureDays = 9f },
                HouseToBunker = new HouseToBunkerSave { HouseDurability = 18f, DebrisCleared = true },
                LocationQuests = new LocationQuestSave
                {
                    Quests = new[]
                    {
                        new QuestEntrySave
                        {
                            NodeId = "node_hospital",
                            QuestType = "hospital_centrifuge",
                            CurrentStage = 1
                        }
                    }
                },
                StructuralIntegrity = new StructuralIntegritySave { Integrity = 70f },
                Waste = new WasteSystemSave { AccumulatedWaste = 11f, Hygiene = 80f },
                Vermin = new VerminSave { PestLevel = 3f },
                JuryRig = new JuryRigSave
                {
                    RiggedModuleIds = new[] { "legacy_mod" },
                    RiggedHours = new[] { 1f }
                },
                FreezePipe = new FreezePipeSave { IsFrozen = true, FrozenHours = 2f },
                Excavation = new ExcavationSave { PendingRubbleUnits = 4, Rooms = Array.Empty<ExcavationRoomSave>() },
                Flooding = new FloodingSave { FloodedRoomIds = new[] { "legacy_room" }, FloodAccumulator = 1f },
                HiddenStorage = new HiddenStorageSave { ItemIds = new[] { "tape" }, Amounts = new[] { 2 } },
                CeilingCollapse = new CeilingCollapseSave
                {
                    CollapsedRoomIds = new[] { "legacy_ceil" },
                    RoomTileKeys = Array.Empty<string>(),
                    RoomTileValues = Array.Empty<float>(),
                    LoadMultKeys = Array.Empty<string>(),
                    LoadMultValues = Array.Empty<float>()
                },
                PerimeterTraps = new PerimeterTrapSave { BearTraps = 3, LastDeployerId = "leg" },
                Tunneling = new TunnelingSave { TunnelProgress = 0.25f, NeighborBreached = true },
                HatchVisibility = new HatchVisibilitySave { Visibility = 0.5f },
                EscapeHatch = new EscapeHatchSave { EscapeHatchBuilt = true, ExcavationProgress = 0.3f },
                MaterialShielding = new MaterialShieldingSave { RoomIds = new[] { "lr" }, Materials = new[] { 2 } },
                Airlock = new AirlockSave { AirlockExists = true, InnerDoorSealed = false },
                Noise = new NoiseSave { NoiseLevel = 4f },
                SubsystemSaveIds = new List<string>(),
                SubsystemSaveJsons = new List<string>()
            };

            string dir = TempDir("legacy");
            try
            {
                var systems = CreateEmpty();
                var ss = MakeSave(dir, s => Wire(s, systems));
                InvokeRestoreFromSnapshot(ss, data);

                Assert.AreEqual("legacy_flood", systems.Flooded.CaptureState().FloodedNodeIds[0]);
                Assert.AreEqual(9f, systems.Ecosystem.CaptureState().RadiationExposureDays, 0.01f);
                Assert.AreEqual(18f, systems.House.CaptureState().HouseDurability, 0.01f);
                Assert.AreEqual(70f, systems.Integrity.CaptureState().Integrity, 0.01f);
                Assert.AreEqual(11f, systems.Waste.CaptureState().AccumulatedWaste, 0.01f);
                Assert.AreEqual(3f, systems.Vermin.CaptureState().PestLevel, 0.01f);
                Assert.AreEqual("legacy_mod", systems.Jury.CaptureState().RiggedModuleIds[0]);
                Assert.IsTrue(systems.Freeze.CaptureState().IsFrozen);
                Assert.AreEqual(4, systems.Excavation.CaptureState().PendingRubbleUnits);
                Assert.AreEqual("legacy_room", systems.Flooding.CaptureState().FloodedRoomIds[0]);
                Assert.AreEqual("tape", systems.Hidden.CaptureState().ItemIds[0]);
                Assert.AreEqual("legacy_ceil", systems.Ceiling.CaptureState().CollapsedRoomIds[0]);
                Assert.AreEqual(3, systems.Perimeter.CaptureState().BearTraps);
                Assert.IsTrue(systems.Tunneling.CaptureState().NeighborBreached);
                Assert.AreEqual(0.5f, systems.HatchVis.CaptureState().Visibility, 0.01f);
                Assert.IsTrue(systems.Escape.CaptureState().EscapeHatchBuilt);
                Assert.AreEqual("lr", systems.Shielding.CaptureState().RoomIds[0]);
                Assert.IsTrue(systems.Airlock.CaptureState().AirlockExists);
                Assert.AreEqual(4f, systems.Noise.CaptureState().NoiseLevel, 0.01f);

                var questState = systems.Quests.CaptureState();
                bool found = false;
                if (questState.Quests != null)
                {
                    for (int i = 0; i < questState.Quests.Length; i++)
                    {
                        if (questState.Quests[i]?.NodeId == "node_hospital"
                            && questState.Quests[i].CurrentStage == 1)
                        {
                            found = true;
                            break;
                        }
                    }
                }
                Assert.IsTrue(found, "legacy location quest must restore via RestIf");
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
