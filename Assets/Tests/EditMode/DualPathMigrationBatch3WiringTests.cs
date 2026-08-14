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
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;
using InventoryClass = AtomicWar._Game.Inventory.Inventory;
using Ashfall.Core;
using Ashfall.Core.Journal;
using JournalSystem = AtomicWar._Game.Events.JournalSystem;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dual-path CapIf → RegisterSystem migration batch 3:
    /// hostage/propaganda/deserter/scapegoat/labor_camp/cult_moral +
    /// core world (world_phase…sabotaged_caches). EventRunner / GeneratedMap /
    /// ShiftingHotspot / Expedition / FactionRaidPlan stay special-path.
    /// </summary>
    [TestFixture]
    public class DualPathMigrationBatch3WiringTests
    {
        private static readonly string[] MigratedIds =
        {
            "hostage", "propaganda", "deserter", "scapegoat", "labor_camp", "cult_moral",
            "world_phase", "economy", "power_network", "hatch_defense", "journal",
            "victory_project", "suspicion", "hatch_entrapment", "atmosphere", "pantry",
            "sabotaged_caches"
        };

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

        private sealed class Batch3Systems
        {
            public HostageSystem Hostage;
            public PropagandaSystem Propaganda;
            public DeserterSystem Deserter;
            public WeatherScapegoatSystem Scapegoat;
            public LaborCampSystem Labor;
            public CultMoralDisgustSystem Cult;
            public WorldPhaseSystem Phase;
            public DynamicEconomySystem Economy;
            public PowerNetwork Power;
            public HatchDefenseSystem HatchDef;
            public JournalSystem Journal;
            public VictoryProjectManager Victory;
            public SuspicionTracker Suspicion;
            public HatchEntrapmentSystem HatchEnt;
            public ShelterAtmosphereSystem Atmosphere;
            public PantryContaminationSystem Pantry;
            public SabotagedCacheSystem Sabotage;
        }

        private static Batch3Systems CreateSeeded()
        {
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            var s = new Batch3Systems
            {
                Hostage = new HostageSystem(),
                Propaganda = new PropagandaSystem(),
                Deserter = new DeserterSystem(),
                Scapegoat = new WeatherScapegoatSystem(),
                Labor = new LaborCampSystem(),
                Cult = new CultMoralDisgustSystem(),
                Phase = new WorldPhaseSystem(),
                Economy = new DynamicEconomySystem(),
                Power = new PowerNetwork(),
                HatchDef = new HatchDefenseSystem(),
                Journal = new JournalSystem(),
                Victory = new VictoryProjectManager(),
                Suspicion = new SuspicionTracker(),
                HatchEnt = new HatchEntrapmentSystem(),
                Atmosphere = new ShelterAtmosphereSystem(),
                Pantry = new PantryContaminationSystem(inv),
                Sabotage = new SabotagedCacheSystem()
            };

            s.Hostage.RestoreState(new HostageSave
            {
                Entries = new[]
                {
                    new HostageEntrySave
                    {
                        ExpeditionId = "exp_1",
                        SurvivorId = "sv_h",
                        CaptorFactionId = "raiders",
                        HoursUntilExpire = 12f,
                        RansomWater = 5f
                    }
                }
            });
            s.Propaganda.RestoreState(new PropagandaSave
            {
                CooldownRemaining = 6f,
                Broadcasts = new[]
                {
                    new PropagandaEntrySave
                    {
                        TargetFactionId = "cult",
                        RivalFactionId = "raiders",
                        RemainingEffectHours = 10f,
                        TrustApplied = -0.2f
                    }
                }
            });
            s.Deserter.RestoreState(new DeserterSave
            {
                Entries = new[]
                {
                    new DeserterEntrySave
                    {
                        SurvivorId = "sv_d",
                        OriginFactionId = "warlords",
                        IsSpy = true,
                        SpyDaysUntilSabotage = 3f
                    }
                }
            });
            s.Scapegoat.RestoreState(new ScapegoatSave
            {
                ConsecutiveBlizzardHours = 40f,
                TributeDemanded = true,
                TributeActive = true,
                TributeHoursRemaining = 8f
            });
            s.Labor.RestoreState(new LaborCampSave
            {
                LaborCampNodeIds = new[] { "camp_a" },
                FreedKeys = Array.Empty<string>(),
                FreedValues = Array.Empty<int>(),
                TotalSlavesFreed = 2
            });
            s.Cult.RestoreState(new CultMoralSave
            {
                TotalCultTrades = 4,
                TotalIrradiatedWaterSold = 15f,
                MassAscensionTriggered = false
            });
            s.Phase.RestoreState(new WorldPhaseSave
            {
                CurrentPhase = WorldPhase.NuclearWinter,
                HasTriggeredExchange = true
            });
            s.Economy.RestoreState(new DynamicEconomySave
            {
                BarterOnlyMode = true,
                BarterOnlyAccepted = new[] { "scrap" },
                LastRepelledFactionId = "raiders",
                Trust = Array.Empty<FactionTrustSave>(),
                Demand = Array.Empty<DemandSave>()
            });
            s.Power.RestoreState(new PowerNetworkSave
            {
                CarbonMonoxidePpm = 15f,
                Sources = Array.Empty<PowerSourceSave>(),
                Consumers = Array.Empty<PowerConsumerSave>()
            });
            s.HatchDef.RestoreState(new HatchDefenseSave
            {
                ExternalNoise = 7f,
                HoursSinceLastRaid = 3f,
                TotalRaidsResolved = 2,
                LastRaidSummary = "repelled_batch3"
            });
            s.Journal.RestoreState(new JournalSave
            {
                Entries = Array.Empty<JournalEntry>(),
                NextSeq = 17,
                HasUnread = true,
                NotificationPingCount = 2
            });
            s.Victory.RestoreState(new VictoryProjectSave
            {
                State = EndgameState.Ongoing,
                MilitaryIntelDecrypted = 3,
                ExtractionUnlocked = true,
                ExtractionUnlockedDay = 40,
                MoralChoicesMade = 2,
                DaysSurvived = 40
            });
            s.Suspicion.RestoreState(new SuspicionTrackerSave
            {
                StarvedHours = 5f,
                MysteryOpen = true,
                TrueThiefId = "sv_thief",
                VanishCount = 1
            });
            s.HatchEnt.RestoreState(new HatchEntrapmentSave
            {
                State = HatchState.Buried,
                ContinuousHazardHours = 80f,
                LastHazardWeather = WeatherKind.Blizzard,
                BuriedEventFired = true
            });
            s.Atmosphere.RestoreState(new ShelterAtmosphereSave
            {
                Rooms = new[]
                {
                    new ShelterRoomAtmosphereSave
                    {
                        RoomId = "entry",
                        Humidity = 0.6f,
                        OxygenFraction = 0.18f,
                        LocalCoPpm = 40f,
                        IsOnFire = false
                    }
                }
            });
            // Pantry CaptureState is intentionally a versioned placeholder (no runtime fields yet).
            s.Sabotage.RestoreState(new SabotagedCacheSave
            {
                HabitScore = 9,
                CachesPlanted = 3,
                CachesDetected = 1,
                PoisonsConsumed = 0
            });
            return s;
        }

        private static Batch3Systems CreateEmpty()
        {
            var inv = new InventoryClass { Capacity = 20, MaxWeight = 100f };
            return new Batch3Systems
            {
                Hostage = new HostageSystem(),
                Propaganda = new PropagandaSystem(),
                Deserter = new DeserterSystem(),
                Scapegoat = new WeatherScapegoatSystem(),
                Labor = new LaborCampSystem(),
                Cult = new CultMoralDisgustSystem(),
                Phase = new WorldPhaseSystem(),
                Economy = new DynamicEconomySystem(),
                Power = new PowerNetwork(),
                HatchDef = new HatchDefenseSystem(),
                Journal = new JournalSystem(),
                Victory = new VictoryProjectManager(),
                Suspicion = new SuspicionTracker(),
                HatchEnt = new HatchEntrapmentSystem(),
                Atmosphere = new ShelterAtmosphereSystem(),
                Pantry = new PantryContaminationSystem(inv),
                Sabotage = new SabotagedCacheSystem()
            };
        }

        private static void Wire(SaveSystem ss, Batch3Systems s)
        {
            ss.SetHostageSystem(s.Hostage);
            ss.SetPropagandaSystem(s.Propaganda);
            ss.SetDeserterSystem(s.Deserter);
            ss.SetScapegoatSystem(s.Scapegoat);
            ss.SetLaborCampSystem(s.Labor);
            ss.SetCultMoralSystem(s.Cult);
            ss.SetWorldPhaseSystem(s.Phase);
            ss.SetEconomySystem(s.Economy);
            ss.SetPowerNetwork(s.Power);
            ss.SetHatchDefense(s.HatchDef);
            ss.SetJournalSystem(s.Journal);
            ss.SetVictoryProjectManager(s.Victory);
            ss.SetSuspicionTracker(s.Suspicion);
            ss.SetHatchEntrapment(s.HatchEnt);
            ss.SetAtmosphereSystem(s.Atmosphere);
            ss.SetPantrySystem(s.Pantry);
            ss.SetSabotagedCacheSystem(s.Sabotage);
        }

        [Test]
        public void Capture_DoesNotDualWrite_PositionalDtos_Batch3()
        {
            var seeded = CreateSeeded();
            string dir = SaveSystemTestFactory.TempDir("dualpath_b3_no_dual");
            try
            {
                var ss = SaveSystemTestFactory.MakeSave(dir, s => Wire(s, seeded));
                Assert.IsTrue(ss.Save("b3_slot"));
                var data = JsonUtility.FromJson<SaveData>(
                    File.ReadAllText(Path.Combine(dir, "save_b3_slot.json")));
                Assert.IsNotNull(data);

                Assert.IsFalse(data.Hostages?.Entries != null && data.Hostages.Entries.Length > 0,
                    "hostage must not dual-write entries");
                Assert.IsFalse(data.Propaganda != null && data.Propaganda.CooldownRemaining > 0.01f,
                    "propaganda must not dual-write cooldown");
                Assert.IsFalse(data.Deserters?.Entries != null && data.Deserters.Entries.Length > 0,
                    "deserter must not dual-write entries");
                Assert.IsFalse(data.Scapegoat != null && data.Scapegoat.TributeDemanded,
                    "scapegoat must not dual-write tribute");
                Assert.IsFalse(data.LaborCamps != null && data.LaborCamps.TotalSlavesFreed == 2,
                    "labor_camp must not dual-write freed count");
                Assert.IsFalse(data.CultMoral != null && data.CultMoral.TotalCultTrades == 4,
                    "cult_moral must not dual-write trades");
                Assert.IsFalse(data.WorldPhase != null && data.WorldPhase.HasTriggeredExchange,
                    "world_phase must not dual-write exchange flag");
                Assert.IsFalse(data.Economy != null && data.Economy.BarterOnlyMode,
                    "economy must not dual-write barter mode");
                Assert.IsFalse(data.Power != null && data.Power.CarbonMonoxidePpm > 0.01f,
                    "power must not dual-write CO ppm");
                Assert.IsFalse(data.HatchDefense != null && data.HatchDefense.TotalRaidsResolved == 2,
                    "hatch_defense must not dual-write raids");
                Assert.IsFalse(data.Journal != null && data.Journal.NextSeq == 17,
                    "journal must not dual-write NextSeq");
                Assert.IsFalse(data.VictoryProject != null && data.VictoryProject.MilitaryIntelDecrypted == 3,
                    "victory_project must not dual-write intel");
                Assert.IsFalse(data.Suspicion != null && data.Suspicion.MysteryOpen,
                    "suspicion must not dual-write mystery");
                Assert.IsFalse(data.HatchEntrapment != null && data.HatchEntrapment.State == HatchState.Buried,
                    "hatch_entrapment must not dual-write buried");
                Assert.IsFalse(data.Atmosphere?.Rooms != null && data.Atmosphere.Rooms.Length > 0,
                    "atmosphere must not dual-write rooms");
                // pantry CR is a placeholder (always Version=1); subsystem id coverage is enough.
                Assert.IsFalse(data.SabotagedCaches != null && data.SabotagedCaches.HabitScore == 9,
                    "sabotaged_caches must not dual-write habit");

                Assert.IsNotNull(data.SubsystemSaveIds);
                foreach (string id in MigratedIds)
                {
                    Assert.IsTrue(ListContains(data.SubsystemSaveIds, id),
                        $"SubsystemSaveIds must include '{id}'");
                    int idx = IndexOfId(data.SubsystemSaveIds, id);
                    Assert.IsFalse(string.IsNullOrEmpty(data.SubsystemSaveJsons[idx]));
                }

                int hostIdx = IndexOfId(data.SubsystemSaveIds, "hostage");
                Assert.IsTrue(data.SubsystemSaveJsons[hostIdx].Contains("sv_h"));
                int phaseIdx = IndexOfId(data.SubsystemSaveIds, "world_phase");
                Assert.IsTrue(data.SubsystemSaveJsons[phaseIdx].Contains("HasTriggeredExchange")
                    || data.SubsystemSaveJsons[phaseIdx].Contains("true")
                    || data.SubsystemSaveJsons[phaseIdx].Contains("NuclearWinter")
                    || data.SubsystemSaveJsons[phaseIdx].Contains("3"));
                int sabIdx = IndexOfId(data.SubsystemSaveIds, "sabotaged_caches");
                Assert.IsTrue(data.SubsystemSaveJsons[sabIdx].Contains("9"));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void RoundTrip_ViaSubsystemIds_RestoresBatch3()
        {
            var seeded = CreateSeeded();
            string dir = SaveSystemTestFactory.TempDir("dualpath_b3_rt");
            try
            {
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, seeded)).Save("b3_rt"));
                var loaded = CreateEmpty();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, loaded)).Load("b3_rt"));

                Assert.AreEqual("sv_h", loaded.Hostage.CaptureState().Entries[0].SurvivorId);
                Assert.AreEqual(6f, loaded.Propaganda.CaptureState().CooldownRemaining, 0.01f);
                Assert.IsTrue(loaded.Deserter.CaptureState().Entries[0].IsSpy);
                Assert.IsTrue(loaded.Scapegoat.CaptureState().TributeActive);
                Assert.AreEqual(2, loaded.Labor.CaptureState().TotalSlavesFreed);
                Assert.AreEqual(4, loaded.Cult.CaptureState().TotalCultTrades);
                Assert.IsTrue(loaded.Phase.CaptureState().HasTriggeredExchange);
                Assert.AreEqual(WorldPhase.NuclearWinter, loaded.Phase.CaptureState().CurrentPhase);
                Assert.IsTrue(loaded.Economy.CaptureState().BarterOnlyMode);
                Assert.AreEqual(15f, loaded.Power.CaptureState().CarbonMonoxidePpm, 0.01f);
                Assert.AreEqual(2, loaded.HatchDef.CaptureState().TotalRaidsResolved);
                Assert.AreEqual(17, loaded.Journal.CaptureState().NextSeq);
                Assert.AreEqual(3, loaded.Victory.CaptureState().MilitaryIntelDecrypted);
                Assert.IsTrue(loaded.Suspicion.CaptureState().MysteryOpen);
                Assert.AreEqual(HatchState.Buried, loaded.HatchEnt.CaptureState().State);
                Assert.AreEqual("entry", loaded.Atmosphere.CaptureState().Rooms[0].RoomId);
                Assert.IsNotNull(loaded.Pantry.CaptureState(), "pantry placeholder CR must still round-trip");
                Assert.AreEqual(9, loaded.Sabotage.CaptureState().HabitScore);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void LegacyRestIf_PositionalOnly_StillRestoresBatch3()
        {
            var data = new SaveData
            {
                SaveVersion = SaveSystem.CurrentSaveVersion,
                GameState = new GameStateSave { Day = 5, Phase = GamePhase.Running },
                Hostages = new HostageSave
                {
                    Entries = new[]
                    {
                        new HostageEntrySave
                        {
                            ExpeditionId = "leg_exp",
                            SurvivorId = "leg_sv",
                            CaptorFactionId = "cult",
                            HoursUntilExpire = 4f
                        }
                    }
                },
                Propaganda = new PropagandaSave { CooldownRemaining = 2f, Broadcasts = Array.Empty<PropagandaEntrySave>() },
                Deserters = new DeserterSave
                {
                    Entries = new[]
                    {
                        new DeserterEntrySave { SurvivorId = "leg_d", OriginFactionId = "raiders", IsSpy = false }
                    }
                },
                Scapegoat = new ScapegoatSave { ConsecutiveBlizzardHours = 10f, TributeRefused = true },
                LaborCamps = new LaborCampSave
                {
                    LaborCampNodeIds = new[] { "leg_camp" },
                    TotalSlavesFreed = 1,
                    FreedKeys = Array.Empty<string>(),
                    FreedValues = Array.Empty<int>()
                },
                CultMoral = new CultMoralSave { TotalCultTrades = 1, TotalIrradiatedWaterSold = 2f },
                WorldPhase = new WorldPhaseSave { CurrentPhase = WorldPhase.Flashpoint, HasTriggeredExchange = true },
                Economy = new DynamicEconomySave
                {
                    BarterOnlyMode = false,
                    LastRepelledFactionId = "leg_fac",
                    Trust = Array.Empty<FactionTrustSave>(),
                    Demand = Array.Empty<DemandSave>()
                },
                Power = new PowerNetworkSave
                {
                    CarbonMonoxidePpm = 5f,
                    Sources = Array.Empty<PowerSourceSave>(),
                    Consumers = Array.Empty<PowerConsumerSave>()
                },
                HatchDefense = new HatchDefenseSave { TotalBreaches = 1, ExternalNoise = 2f },
                Journal = new JournalSave { NextSeq = 3, Entries = Array.Empty<JournalEntry>() },
                VictoryProject = new VictoryProjectSave { MilitaryIntelDecrypted = 1, DaysSurvived = 10 },
                Suspicion = new SuspicionTrackerSave { StarvedHours = 2f, TrueThiefId = "leg_thief" },
                HatchEntrapment = new HatchEntrapmentSave { State = HatchState.Frozen, ContinuousHazardHours = 90f },
                Atmosphere = new ShelterAtmosphereSave
                {
                    Rooms = new[]
                    {
                        new ShelterRoomAtmosphereSave { RoomId = "sleep", Humidity = 0.4f, OxygenFraction = 0.2f }
                    }
                },
                Pantry = new PantryContaminationSave(),
                SabotagedCaches = new SabotagedCacheSave { HabitScore = 4, CachesPlanted = 1 },
                SubsystemSaveIds = new List<string>(),
                SubsystemSaveJsons = new List<string>()
            };

            string dir = SaveSystemTestFactory.TempDir("dualpath_b3_legacy");
            try
            {
                var systems = CreateEmpty();
                InvokeRestoreFromSnapshot(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, systems)), data);

                Assert.AreEqual("leg_sv", systems.Hostage.CaptureState().Entries[0].SurvivorId);
                Assert.AreEqual(2f, systems.Propaganda.CaptureState().CooldownRemaining, 0.01f);
                Assert.AreEqual("leg_d", systems.Deserter.CaptureState().Entries[0].SurvivorId);
                Assert.IsTrue(systems.Scapegoat.CaptureState().TributeRefused);
                Assert.AreEqual(1, systems.Labor.CaptureState().TotalSlavesFreed);
                Assert.AreEqual(1, systems.Cult.CaptureState().TotalCultTrades);
                Assert.AreEqual(WorldPhase.Flashpoint, systems.Phase.CaptureState().CurrentPhase);
                Assert.AreEqual("leg_fac", systems.Economy.CaptureState().LastRepelledFactionId);
                Assert.AreEqual(5f, systems.Power.CaptureState().CarbonMonoxidePpm, 0.01f);
                Assert.AreEqual(1, systems.HatchDef.CaptureState().TotalBreaches);
                Assert.AreEqual(3, systems.Journal.CaptureState().NextSeq);
                Assert.AreEqual(1, systems.Victory.CaptureState().MilitaryIntelDecrypted);
                Assert.AreEqual("leg_thief", systems.Suspicion.CaptureState().TrueThiefId);
                Assert.AreEqual(HatchState.Frozen, systems.HatchEnt.CaptureState().State);
                Assert.AreEqual("sleep", systems.Atmosphere.CaptureState().Rooms[0].RoomId);
                Assert.IsNotNull(systems.Pantry.CaptureState());
                Assert.AreEqual(4, systems.Sabotage.CaptureState().HabitScore);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
