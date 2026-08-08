using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;
using Random = System.Random;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// End-to-end SaveSystem round-trips for field-only special-path systems that
    /// cannot safely use plain RegisterSystem adapters:
    /// EventRunner queue, GeneratedMap seed rebuild, ShiftingHotspot Bind-before-restore,
    /// FactionRaidPlan SetMap-before-restore, Expedition list rebuild,
    /// Affinity matrix (nested on MentalBreakSystem), FlashpointChoreographer host delegates.
    /// </summary>
    [TestFixture]
    public class SpecialPathSaveRoundTripWiringTests
    {
        private const float Eps = 0.01f;
        private const int MapSeed = 55;

        private static string TempDir(string tag)
        {
            string dir = Path.Combine(Path.GetTempPath(), "ashfall_special_path_" + tag + "_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(dir);
            return dir;
        }

        private static SaveSystem MakeSave(
            string dir,
            Action<SaveSystem> wire,
            Func<IReadOnlyList<Survivor>> getSurvivors = null,
            Func<string, ItemDefinition> itemLookup = null)
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
                GetSurvivors = getSurvivors ?? (() => new List<Survivor>()),
                ItemLookup = itemLookup ?? (id => null),
                ModuleLookup = id => null,
                SavesDir = dir
            });
            wire(ss);
            return ss;
        }

        private static bool ListContains(List<string> ids, string id)
        {
            if (ids == null) return false;
            for (int i = 0; i < ids.Count; i++)
                if (string.Equals(ids[i], id, StringComparison.Ordinal))
                    return true;
            return false;
        }

        private static List<string> CollectSaveableIds(SaveSystem ss)
        {
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
            return ids;
        }

        private sealed class AlwaysShiftRng : Random
        {
            public override double NextDouble() => 0.0;
            public override int Next(int maxValue) => 0;
        }

        private static void SeedKnowledge(GeneratedMap map, RadiationKnowledgeMap knowledge)
        {
            if (map?.Nodes == null || knowledge == null) return;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null || n.IsShelter) continue;
                knowledge.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
            }
        }

        private static MapNode FindFirstNonShelter(GeneratedMap map)
        {
            if (map?.Nodes == null) return null;
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n != null && !n.IsShelter) return n;
            }
            return null;
        }

        // ─────────────────────────────────────────────────────────────
        // EventRunner — CaptureScheduledState / RestoreScheduledState
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void EventRunner_ScheduledQueue_RoundTrips_ViaSaveSystem()
        {
            var seeded = new EventRunner();
            seeded.ScheduleEvent("evt_special_chain_a", 12, "flag_origin_a");
            seeded.ScheduleEvent("evt_special_chain_b", 20, "flag_origin_b");

            string dir = TempDir("event");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => s.SetEventRunner(seeded)).Save("sp_evt"));

                var loaded = new EventRunner();
                Assert.IsTrue(MakeSave(dir, s => s.SetEventRunner(loaded)).Load("sp_evt"));

                Assert.AreEqual(2, loaded.ScheduledEvents.Count);
                Assert.AreEqual("evt_special_chain_a", loaded.ScheduledEvents[0].EventId);
                Assert.AreEqual(12, loaded.ScheduledEvents[0].ExecuteOnDay);
                Assert.AreEqual("flag_origin_a", loaded.ScheduledEvents[0].OriginFlag);
                Assert.AreEqual("evt_special_chain_b", loaded.ScheduledEvents[1].EventId);
                Assert.AreEqual(20, loaded.ScheduledEvents[1].ExecuteOnDay);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // GeneratedMap — seed rebuild + RestoreRevealState
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void GeneratedMap_SeedRebuild_PreservesRevealFlags_ViaSaveSystem()
        {
            var map = MapGenerator.Generate(MapSeed);
            var node = FindFirstNonShelter(map);
            Assert.IsNotNull(node, "Map should have a non-shelter node");
            string nodeId = node.NodeId;
            map.MarkVisited(nodeId);
            map.SetRumoredRad(nodeId, 77f);

            string dir = TempDir("map");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => s.SetGeneratedMap(map)).Save("sp_map"));

                // Empty host map (wrong seed) forces RestoreGeneratedMap rebuild path.
                var loadedMap = new GeneratedMap { Seed = 0 };
                Assert.IsTrue(MakeSave(dir, s => s.SetGeneratedMap(loadedMap)).Load("sp_map"));

                Assert.AreEqual(MapSeed, loadedMap.Seed);
                Assert.IsNotNull(loadedMap.Nodes);
                Assert.Greater(loadedMap.Nodes.Count, 0);
                var reloaded = loadedMap.GetNode(nodeId);
                Assert.IsNotNull(reloaded);
                Assert.IsTrue(reloaded.IsVisited);
                Assert.IsTrue(reloaded.IsRevealed);
                Assert.AreEqual(77f, reloaded.RumoredRad, Eps);

                // Layout matches a fresh generate of the same seed.
                var fresh = MapGenerator.Generate(MapSeed);
                Assert.AreEqual(fresh.Nodes.Count, loadedMap.Nodes.Count);
                Assert.AreEqual(fresh.Paths.Count, loadedMap.Paths.Count);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // ShiftingHotspot — Bind(map, knowledge) before RestoreState
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void ShiftingHotspot_BindBeforeRestore_ReplaysHistory_ViaSaveSystem()
        {
            var map = MapGenerator.Generate(MapSeed);
            var knowledge = new RadiationKnowledgeMap();
            SeedKnowledge(map, knowledge);

            var hotspots = new ShiftingHotspotSystem(new AlwaysShiftRng());
            hotspots.Bind(map, knowledge);
            var shift = hotspots.TryShift(40);
            Assert.IsNotNull(shift, "AlwaysShiftRng should produce a windstorm shift");
            Assert.GreaterOrEqual(hotspots.History.Count, 1);
            Assert.Greater(hotspots.ShiftCount, 0);

            var radSnapshot = new Dictionary<string, float>();
            var deathSnapshot = new Dictionary<string, bool>();
            for (int i = 0; i < map.Nodes.Count; i++)
            {
                var n = map.Nodes[i];
                if (n == null) continue;
                radSnapshot[n.NodeId] = n.TrueRad;
                deathSnapshot[n.NodeId] = n.IsDeathZone;
            }

            string dir = TempDir("hotspot");
            try
            {
                Assert.IsTrue(MakeSave(dir, s =>
                {
                    s.SetGeneratedMap(map);
                    s.SetKnowledgeMap(knowledge);
                    s.SetShiftingHotspotSystem(hotspots);
                }).Save("sp_hot"));

                // Fresh seed map (no shifts applied) + empty hotspot system.
                var map2 = MapGenerator.Generate(MapSeed);
                var knowledge2 = new RadiationKnowledgeMap();
                SeedKnowledge(map2, knowledge2);
                var hotspots2 = new ShiftingHotspotSystem(new Random(999));
                // Intentionally unbound — SaveSystem must Bind before RestoreState.

                Assert.IsTrue(MakeSave(dir, s =>
                {
                    s.SetGeneratedMap(map2);
                    s.SetKnowledgeMap(knowledge2);
                    s.SetShiftingHotspotSystem(hotspots2);
                }).Load("sp_hot"));

                Assert.AreEqual(hotspots.ShiftCount, hotspots2.ShiftCount);
                Assert.AreEqual(hotspots.History.Count, hotspots2.History.Count);
                Assert.AreEqual(shift.FromNodeId, hotspots2.History[0].FromNodeId);
                Assert.AreEqual(shift.ToNodeId, hotspots2.History[0].ToNodeId);

                for (int i = 0; i < map2.Nodes.Count; i++)
                {
                    var n = map2.Nodes[i];
                    if (n == null) continue;
                    Assert.AreEqual(radSnapshot[n.NodeId], n.TrueRad, Eps, n.NodeId);
                    Assert.AreEqual(deathSnapshot[n.NodeId], n.IsDeathZone, n.NodeId);
                }
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // FactionRaidPlan — SetMap before RestoreState
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void FactionRaidPlan_SetMapBeforeRestore_RoundTrips_ViaSaveSystem()
        {
            var map = MapGenerator.Generate(MapSeed);
            var plans = new FactionRaidPlanSystem(new Random(7));
            plans.SetMap(map);
            plans.RestoreState(new FactionRaidPlanSave
            {
                NextSeq = 3,
                Plans = new[]
                {
                    new FactionRaidPlanSaveEntry
                    {
                        Id = "raid_sp_1",
                        AttackerFactionId = "military_remnants",
                        TargetFactionId = "scavenger_camp",
                        ScheduledDay = 32,
                        FireDay = 34,
                        Resolved = false,
                        InterceptPresented = true,
                        BattlefieldNodeId = "ring_road",
                        ChoiceId = string.Empty
                    }
                }
            });

            string dir = TempDir("raid");
            try
            {
                Assert.IsTrue(MakeSave(dir, s =>
                {
                    s.SetGeneratedMap(map);
                    s.SetFactionRaidPlanSystem(plans);
                }).Save("sp_raid"));

                var map2 = MapGenerator.Generate(MapSeed);
                var plans2 = new FactionRaidPlanSystem(new Random(99));
                // Unbound — SaveSystem SetMap(_generatedMap) before restore.

                Assert.IsTrue(MakeSave(dir, s =>
                {
                    s.SetGeneratedMap(map2);
                    s.SetFactionRaidPlanSystem(plans2);
                }).Load("sp_raid"));

                Assert.AreEqual(1, plans2.Plans.Count);
                Assert.AreEqual("raid_sp_1", plans2.Plans[0].Id);
                Assert.AreEqual("military_remnants", plans2.Plans[0].AttackerFactionId);
                Assert.AreEqual("scavenger_camp", plans2.Plans[0].TargetFactionId);
                Assert.AreEqual(34, plans2.Plans[0].FireDay);
                Assert.IsTrue(plans2.Plans[0].InterceptPresented);
                Assert.IsFalse(plans2.Plans[0].Resolved);

                var cap = plans2.CaptureState();
                Assert.AreEqual(3, cap.NextSeq);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Expedition — active list rebuild (GetOrCreate + fields + loot)
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void Expedition_ListRebuild_RoundTrips_ViaSaveSystem()
        {
            var profile = ScriptableObject.CreateInstance<NeedsProfile>();
            var needs = new NeedsSystem(profile, sv => true);
            var rad = new RadiationSystem(needs);
            var inv = new InventoryClass { Capacity = 40, MaxWeight = 100f };
            var lootItem = ScriptableObject.CreateInstance<ItemDefinition>();
            lootItem.id = "canned_food";
            lootItem.displayName = "Canned Food";
            lootItem.weight = 0.5f;

            var survivor = new Survivor
            {
                Id = "sv_sp_exp",
                DisplayName = "Scout",
                State = SurvivorState.Idle
            };
            needs.Register(survivor);
            rad.Register(survivor);
            var survivors = new List<Survivor> { survivor };

            ExpeditionSystem seeded = null;
            ExpeditionSystem loaded = null;
            string dir = TempDir("exp");
            try
            {
                seeded = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
                {
                    Seed = 11,
                    CreateDefaultEncounters = false
                });
                Assert.IsTrue(seeded.StartExpeditionFromPath(survivor, new ExpeditionSystem.PathRequest
                {
                    NodeId = "ruined_subway",
                    DisplayName = "Ruined Subway",
                    TravelHours = 3f,
                    TrueRad = 15f,
                    DangerLevel = 2f,
                    Stance = ExpeditionStance.Stealth,
                    MaxLootCapacity = 20f
                }));
                var state = seeded.GetExpeditionBySurvivor(survivor.Id);
                Assert.IsNotNull(state);
                state.Phase = ExpeditionPhase.Looting;
                state.TravelTicksCompleted = 2;
                state.CurrentTick = 5;
                state.Stamina = 62f;
                state.CollectedLootItemIds.Add("canned_food");

                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(seeded),
                    getSurvivors: () => survivors,
                    itemLookup: id => id == "canned_food" ? lootItem : null).Save("sp_exp"));

                // Fresh expedition system + same survivor list for rebuild.
                loaded = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
                {
                    Seed = 99,
                    CreateDefaultEncounters = false
                });
                Assert.AreEqual(0, loaded.ActiveExpeditions.Count);

                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(loaded),
                    getSurvivors: () => survivors,
                    itemLookup: id => id == "canned_food" ? lootItem : null).Load("sp_exp"));

                Assert.AreEqual(1, loaded.ActiveExpeditions.Count);
                var restored = loaded.GetExpeditionBySurvivor("sv_sp_exp");
                Assert.IsNotNull(restored);
                Assert.AreEqual("ruined_subway", restored.TargetLocationId);
                Assert.AreEqual("Ruined Subway", restored.TargetLocationName);
                Assert.AreEqual(ExpeditionPhase.Looting, restored.Phase);
                Assert.AreEqual(2, restored.TravelTicksCompleted);
                Assert.AreEqual(5, restored.CurrentTick);
                Assert.AreEqual(62f, restored.Stamina, Eps);
                Assert.AreEqual(1, restored.CollectedLootItemIds.Count);
                Assert.AreEqual("canned_food", restored.CollectedLootItemIds[0]);
                // Loot item defs rebuilt via ItemLookup.
                Assert.AreEqual(1, restored.CollectedLoot.Count);
                Assert.AreEqual("canned_food", restored.CollectedLoot[0].id);
                Assert.AreSame(survivor, restored.Survivor);
            }
            finally
            {
                seeded?.UnsubscribeAll();
                loaded?.UnsubscribeAll();
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Expedition — Day-30 Flashpoint intercept state (SAVE-002)
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// SAVE-002: ApplyExpeditionSaveFields restores the six flashpoint
        /// fields, so CaptureExpeditionState must write them. Loading
        /// mid-flashpoint used to reset the survivor to default behavior.
        /// </summary>
        [Test]
        public void Expedition_FlashpointFields_RoundTrip_ViaSaveSystem()
        {
            var host = new ExpeditionHostFixture("sv_sp_flash");
            ExpeditionSystem seeded = null;
            ExpeditionSystem loaded = null;
            string dir = TempDir("expflash");
            try
            {
                seeded = host.StartExpedition(out var state);
                state.Phase = ExpeditionPhase.Inbound;
                state.isCommsSevered = true;
                state.flashpointBehavior = FlashpointBehavior.CautiousShelter;
                state.originalEtaTicks = 18f;
                state.shelterDelayTicksRemaining = 7;
                state.returnSpeedMultiplier = 2f;
                state.returnSpeedDivisor = 0.5f;

                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(seeded),
                    getSurvivors: () => host.Survivors).Save("sp_expflash"));

                loaded = host.NewSystem();
                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(loaded),
                    getSurvivors: () => host.Survivors).Load("sp_expflash"));

                Assert.AreEqual(1, loaded.ActiveExpeditions.Count);
                var exp = loaded.GetExpeditionBySurvivor("sv_sp_flash");
                Assert.IsNotNull(exp);
                Assert.IsTrue(exp.isCommsSevered, "isCommsSevered must survive save/load");
                Assert.AreEqual(FlashpointBehavior.CautiousShelter, exp.flashpointBehavior);
                Assert.AreEqual(18f, exp.originalEtaTicks, Eps);
                Assert.AreEqual(7, exp.shelterDelayTicksRemaining);
                Assert.AreEqual(2f, exp.returnSpeedMultiplier, Eps);
                Assert.AreEqual(0.5f, exp.returnSpeedDivisor, Eps);
            }
            finally
            {
                seeded?.UnsubscribeAll();
                loaded?.UnsubscribeAll();
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        /// <summary>
        /// Ratchet for SAVE-002: every serialized ExpeditionSaveState field must
        /// be written by CaptureExpeditionState. Seed the live state with
        /// non-default values, save, and assert no DTO field came out at its
        /// constructor default. A newly added field that restore reads but
        /// capture forgets fails here instead of silently resetting on load.
        /// </summary>
        [Test]
        public void Expedition_Capture_Writes_Every_SaveState_Field()
        {
            var host = new ExpeditionHostFixture("sv_cover");
            ExpeditionSystem seeded = null;
            string dir = TempDir("expcover");
            try
            {
                seeded = host.StartExpedition(out var state);
                SetEveryMirroredFieldNonDefault(state);

                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(seeded),
                    getSurvivors: () => host.Survivors).Save("sp_expcover"));

                var data = ReadSaveFile(dir, "sp_expcover");
                Assert.IsNotNull(data.Expeditions);
                Assert.AreEqual(1, data.Expeditions.Count);
                var captured = data.Expeditions[0];
                var defaults = new ExpeditionSaveState();

                var missed = new List<string>();
                foreach (var f in typeof(ExpeditionSaveState).GetFields(
                             BindingFlags.Instance | BindingFlags.Public))
                {
                    if (f.IsNotSerialized) continue;
                    if (f.FieldType == typeof(List<string>))
                    {
                        var list = f.GetValue(captured) as List<string>;
                        if (list == null || list.Count == 0) missed.Add(f.Name);
                        continue;
                    }
                    if (Equals(f.GetValue(captured), f.GetValue(defaults)))
                        missed.Add(f.Name);
                }

                Assert.IsEmpty(
                    missed,
                    "CaptureExpeditionState left these ExpeditionSaveState fields at their default: "
                    + string.Join(", ", missed));
            }
            finally
            {
                seeded?.UnsubscribeAll();
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        /// <summary>
        /// SAVE-008: loading over a live session must not leave expeditions the
        /// save never mentioned. Restore rebuilds by survivor id, so without a
        /// clear an unrelated runner keeps ticking as a phantom.
        /// </summary>
        [Test]
        public void Expedition_Load_Clears_Expeditions_Absent_From_Save()
        {
            var host = new ExpeditionHostFixture("sv_saved");
            var phantomHost = new ExpeditionHostFixture("sv_phantom");
            ExpeditionSystem seeded = null;
            ExpeditionSystem live = null;
            string dir = TempDir("expphantom");
            try
            {
                seeded = host.StartExpedition(out _);
                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(seeded),
                    getSurvivors: () => host.Survivors).Save("sp_expphantom"));

                // Live session running a different survivor's expedition.
                live = phantomHost.StartExpedition(out _);
                Assert.AreEqual(1, live.ActiveExpeditions.Count);

                var allSurvivors = new List<Survivor>(host.Survivors);
                allSurvivors.AddRange(phantomHost.Survivors);

                Assert.IsTrue(MakeSave(
                    dir,
                    s => s.SetExpeditionSystem(live),
                    getSurvivors: () => allSurvivors).Load("sp_expphantom"));

                Assert.AreEqual(1, live.ActiveExpeditions.Count,
                    "Load must leave only the saved expedition active");
                Assert.IsNotNull(live.GetExpeditionBySurvivor("sv_saved"));
                Assert.IsNull(live.GetExpeditionBySurvivor("sv_phantom"),
                    "Expedition absent from the save must not survive load");
            }
            finally
            {
                seeded?.UnsubscribeAll();
                live?.UnsubscribeAll();
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        private static SaveData ReadSaveFile(string dir, string slotId)
        {
            string path = AtomicWar._Game.Utilities.SaveSlotPaths.SlotPath(dir, slotId);
            Assert.IsTrue(File.Exists(path), "Save file not written: " + path);
            var data = JsonUtility.FromJson<SaveData>(File.ReadAllText(path));
            Assert.IsNotNull(data, "Save file did not parse as SaveData");
            return data;
        }

        /// <summary>
        /// Every ExpeditionState field the DTO mirrors, moved off its default so
        /// a forgotten capture assignment shows up as a default-valued DTO field.
        /// </summary>
        private static void SetEveryMirroredFieldNonDefault(ExpeditionState exp)
        {
            exp.ExpeditionId = "exp_cover_1";
            exp.TargetLocationId = "collapsed_mall";
            exp.TargetLocationName = "Collapsed Mall";
            exp.Stance = ExpeditionStance.Speed;
            exp.Phase = ExpeditionPhase.Looting;
            exp.CurrentTick = 9;
            exp.TotalDistanceTicks = 14;
            exp.TravelTicksCompleted = 6;
            exp.LootingTicksCompleted = 3;
            exp.CarryingCapacity = 25f;
            exp.CurrentWeight = 11f;
            exp.Stamina = 58f;
            exp.SuitDegradation = 12f;
            exp.TrueRadPerHour = 21f;
            exp.DangerLevel = 3f;
            exp.IsPushingLuck = true;
            exp.IsRetreating = true;
            exp.isCommsSevered = true;
            exp.flashpointBehavior = FlashpointBehavior.ParanoidSprint;
            exp.originalEtaTicks = 18f;
            exp.shelterDelayTicksRemaining = 7;
            exp.returnSpeedMultiplier = 2f;
            exp.returnSpeedDivisor = 0.5f;
            exp.LocationEncounterFired = true;
            exp.UxoDetonated = true;
            exp.HasBicycle = true;
            exp.BicycleDurability = 44f;
            exp.IsWading = true;
            exp.IsNightScavenge = true;
            exp.HasFlashlight = true;
            exp.FlashlightBattery = 66f;
            exp.CollectedLootItemIds.Clear();
            exp.CollectedLootItemIds.Add("canned_food");
        }

        /// <summary>
        /// One registered survivor plus the dependencies ExpeditionSystem needs,
        /// so a test can spin up seed/load systems that share a survivor list.
        /// </summary>
        private sealed class ExpeditionHostFixture
        {
            private readonly RadiationSystem _rad;
            private readonly InventoryClass _inv;
            private readonly Survivor _survivor;
            private int _seed = 11;

            public ExpeditionHostFixture(string survivorId)
            {
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                _rad = new RadiationSystem(needs);
                _inv = new InventoryClass { Capacity = 40, MaxWeight = 100f };
                _survivor = new Survivor
                {
                    Id = survivorId,
                    DisplayName = "Runner",
                    State = SurvivorState.Idle
                };
                needs.Register(_survivor);
                _rad.Register(_survivor);
                Survivors = new List<Survivor> { _survivor };
            }

            public List<Survivor> Survivors { get; }

            public ExpeditionSystem NewSystem() =>
                new ExpeditionSystem(_rad, _inv, null, new ExpeditionSystem.Config
                {
                    Seed = _seed++,
                    CreateDefaultEncounters = false
                });

            public ExpeditionSystem StartExpedition(out ExpeditionState state)
            {
                var system = NewSystem();
                Assert.IsTrue(system.StartExpeditionFromPath(_survivor, new ExpeditionSystem.PathRequest
                {
                    NodeId = "ruined_subway",
                    DisplayName = "Ruined Subway",
                    TravelHours = 3f,
                    TrueRad = 15f,
                    DangerLevel = 2f,
                    Stance = ExpeditionStance.Stealth,
                    MaxLootCapacity = 20f
                }));
                state = system.GetExpeditionBySurvivor(_survivor.Id);
                Assert.IsNotNull(state);
                return system;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Affinity matrix — nested on MentalBreakSystem (field-only)
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void Affinity_Matrix_RoundTrips_ViaSaveSystem()
        {
            var seeded = new MentalBreakSystem();
            seeded.Affinity.Set("sv_alpha", "sv_bravo", 42f);
            seeded.Affinity.Set("sv_alpha", "sv_charlie", -18.5f);
            seeded.Affinity.Set("sv_bravo", "sv_charlie", 7f);

            string dir = TempDir("affinity");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => s.SetMentalBreakSystem(seeded)).Save("sp_aff"));

                var loaded = new MentalBreakSystem();
                Assert.AreEqual(0f, loaded.Affinity.Get("sv_alpha", "sv_bravo"), Eps);

                Assert.IsTrue(MakeSave(dir, s => s.SetMentalBreakSystem(loaded)).Load("sp_aff"));

                Assert.AreEqual(42f, loaded.Affinity.Get("sv_alpha", "sv_bravo"), Eps);
                Assert.AreEqual(-18.5f, loaded.Affinity.Get("sv_alpha", "sv_charlie"), Eps);
                Assert.AreEqual(7f, loaded.Affinity.Get("sv_bravo", "sv_charlie"), Eps);
                // Symmetric undirected matrix.
                Assert.AreEqual(42f, loaded.Affinity.Get("sv_bravo", "sv_alpha"), Eps);
                // Unset pair stays neutral.
                Assert.AreEqual(0f, loaded.Affinity.Get("sv_alpha", "sv_delta"), Eps);

                var snap = loaded.Affinity.Snapshot();
                Assert.AreEqual(3, snap.Count);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // FlashpointChoreographer — host capture/restore delegates
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void FlashpointChoreographer_DelegateAdapters_RoundTrips_ViaSaveSystem()
        {
            var sequence = ScriptableObject.CreateInstance<FlashpointSequenceSO>();
            sequence.sequenceId = "sp_flashpoint";
            sequence.buildupDays = new List<FlashpointBuildupDay>();
            sequence.economyModifiers = new List<FlashpointEconomyModifier>();
            sequence.steps = new List<FlashpointChoreographyStep>
            {
                new FlashpointChoreographyStep { actionId = "white_flash", delayFromPreviousSeconds = 0f },
                new FlashpointChoreographyStep { actionId = "emp", delayFromPreviousSeconds = 1f },
                new FlashpointChoreographyStep { actionId = "complete", delayFromPreviousSeconds = 0f }
            };
            sequence.accessibility = new FlashpointAccessibilityOverrides();

            var systems = new FlashpointChoreographerSystems
            {
                Inventory = new InventoryClass { Capacity = 10, MaxWeight = 50f },
                Shelter = new ShelterClass(),
                RadioState = new RadioState(),
                EconomySystem = new DynamicEconomySystem(),
                Survivors = new List<Survivor>(),
                ExchangeMoraleHit = 25f
            };

            var seeded = new FlashpointChoreographer(sequence, () => false, systems, () => true);
            seeded.RestoreState(new FlashpointChoreographerSave
            {
                BuildupDaysProcessed = new List<int> { 25, 26, 27 },
                ChoreographyStepIndex = 1,
                ElapsedRealSeconds = 12.5f,
                ChoreographyCompleted = false
            });

            string dir = TempDir("choreo");
            try
            {
                Assert.IsTrue(MakeSave(dir, s => s.SetFlashpointChoreographer(
                    seeded.CaptureState,
                    seeded.RestoreState)).Save("sp_ch"));

                var loaded = new FlashpointChoreographer(sequence, () => false, systems, () => true);
                Assert.AreEqual(0, loaded.BuildupDaysProcessed.Count);
                Assert.AreEqual(-1, loaded.CurrentStepIndex);

                Assert.IsTrue(MakeSave(dir, s => s.SetFlashpointChoreographer(
                    loaded.CaptureState,
                    loaded.RestoreState)).Load("sp_ch"));

                Assert.That(loaded.BuildupDaysProcessed, Is.EquivalentTo(new[] { 25, 26, 27 }));
                Assert.AreEqual(1, loaded.CurrentStepIndex);
                Assert.IsFalse(loaded.IsChoreographyCompleted);
                Assert.IsTrue(loaded.IsChoreographyActive,
                    "Exchange already triggered: restore must mark choreography started");

                var recap = loaded.CaptureState();
                Assert.AreEqual(12.5f, recap.ElapsedRealSeconds, Eps);
                Assert.AreEqual(1, recap.ChoreographyStepIndex);
                Assert.IsFalse(recap.ChoreographyCompleted);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
                if (sequence != null) UnityEngine.Object.DestroyImmediate(sequence);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Guard: specials stay field-only (not ISaveable RegisterSystem)
        // ─────────────────────────────────────────────────────────────

        [Test]
        public void SpecialPath_Systems_Remain_FieldOnly_Not_RegisterSystem()
        {
            string dir = TempDir("guard");
            try
            {
                var map = MapGenerator.Generate(MapSeed);
                var knowledge = new RadiationKnowledgeMap();
                var hotspots = new ShiftingHotspotSystem();
                var plans = new FactionRaidPlanSystem();
                var runner = new EventRunner();
                var mental = new MentalBreakSystem();
                var profile = ScriptableObject.CreateInstance<NeedsProfile>();
                var needs = new NeedsSystem(profile, sv => true);
                var rad = new RadiationSystem(needs);
                var inv = new InventoryClass { Capacity = 10, MaxWeight = 50f };
                ExpeditionSystem exp = null;
                try
                {
                    exp = new ExpeditionSystem(rad, inv, null, new ExpeditionSystem.Config
                    {
                        Seed = 1,
                        CreateDefaultEncounters = false
                    });

                    var ss = MakeSave(dir, s =>
                    {
                        s.SetEventRunner(runner);
                        s.SetGeneratedMap(map);
                        s.SetKnowledgeMap(knowledge);
                        s.SetShiftingHotspotSystem(hotspots);
                        s.SetFactionRaidPlanSystem(plans);
                        s.SetExpeditionSystem(exp);
                        s.SetMentalBreakSystem(mental);
                        s.SetFlashpointChoreographer(
                            () => new FlashpointChoreographerSave(),
                            _ => { });
                    });

                    var ids = CollectSaveableIds(ss);
                    Assert.IsFalse(ListContains(ids, "event_runner"));
                    Assert.IsFalse(ListContains(ids, "generated_map"));
                    Assert.IsFalse(ListContains(ids, "shifting_hotspots"));
                    Assert.IsFalse(ListContains(ids, "faction_raid_plans"));
                    Assert.IsFalse(ListContains(ids, "expedition"));
                    Assert.IsFalse(ListContains(ids, "affinity"));
                    Assert.IsFalse(ListContains(ids, "mental_break"));
                    Assert.IsFalse(ListContains(ids, "flashpoint_choreographer"));
                    // Knowledge is RegisterSystem (not a special-path field inject).
                    Assert.IsTrue(ListContains(ids, "radiation_knowledge"));
                }
                finally
                {
                    exp?.UnsubscribeAll();
                }
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
