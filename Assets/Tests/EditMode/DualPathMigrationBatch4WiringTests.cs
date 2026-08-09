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
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Survivors;
using ShelterClass = AtomicWar._Game.Shelter.Shelter;

namespace AtomicWar.Tests.EditMode
{
    /// <summary>
    /// Dual-path CapIf → RegisterSystem migration batch 4:
    /// faction_radio_intercepts / debt_collector / ghost_stations / lifeboat +
    /// CaptureSimulationExtras (resilience…personal_quests). Complex specials
    /// (EventRunner / GeneratedMap / ShiftingHotspot / Expedition / FactionRaidPlan)
    /// stay special-path.
    /// </summary>
    [TestFixture]
    public class DualPathMigrationBatch4WiringTests
    {
        private static readonly string[] MigratedIds =
        {
            "faction_radio_intercepts", "debt_collector", "ghost_stations", "lifeboat",
            "resilience", "compost", "wind_turbine", "hauling", "weapon_maint",
            "aesthetics", "ham_radio", "skill_progression",
            "combat_perks", "survival_perks", "shelter_perks", "medical_perks",
            "expedition_perks", "social_perks", "personal_quests"
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

        private sealed class Batch4Systems
        {
            public FactionRadioInterceptSystem Radio;
            public DebtCollectorSystem Debt;
            public GhostStationSystem Ghost;
            public LifeboatTransmissionSystem Lifeboat;
            public ResilienceSystem Resilience;
            public CompostSystem Compost;
            public WindTurbineSystem Wind;
            public InternalHaulingSystem Hauling;
            public WeaponMaintenanceSystem WeaponMaint;
            public RoomAestheticsSystem Aesthetics;
            public HamRadioSystem Ham;
            public SkillProgressionSystem Skills;
            public CombatPerkSystem Combat;
            public SurvivalPerkSystem Survival;
            public ShelterPerkSystem ShelterPerks;
            public MedicalPerkSystem Medical;
            public ExpeditionPerkSystem Expedition;
            public SocialPerkSystem Social;
            public PersonalQuestSystem Personal;
        }

        private static Batch4Systems CreateSeeded()
        {
            var s = CreateEmpty();
            s.Radio.RestoreState(new FactionRadioInterceptSave
            {
                NextSeq = 4,
                HudHasUnread = true,
                HudTunerIndex = 2,
                Entries = new[]
                {
                    new FactionRadioInterceptSystem.InterceptEntry
                    {
                        Id = "intercept_1_succession",
                        FactionId = "raiders",
                        Kind = "Succession",
                        Message = "batch4 radio seed",
                        Day = 9
                    }
                }
            });
            s.Debt.RestoreState(new DebtCollectorSave
            {
                NextSeq = 3,
                Debts = new[]
                {
                    new DebtEntrySave
                    {
                        Id = "debt_b4",
                        FactionId = "warlords",
                        ScheduledDay = 12,
                        CollectorDay = 15,
                        FuelDemanded = 8f,
                        WaterDemanded = 4f
                    }
                }
            });
            s.Ghost.RestoreState(new GhostStationSave
            {
                Unlocked = true,
                HeardStationIds = new[] { "ghost_weather_loop" }
            });
            s.Lifeboat.RestoreState(new LifeboatTransmissionSave
            {
                Contacted = true,
                Offered = true,
                Resolved = false,
                ExtractedSurvivorId = "sv_life",
                ExtractedSurvivorName = "Lee",
                LeftBehindIds = new[] { "sv_left" },
                LeftBehindNames = new[] { "Lefty" }
            });
            s.Resilience.RestoreState(new ResilienceSave
            {
                Keys = new[] { "sv_r" },
                Values = new[] { 15f }
            });
            s.Compost.RestoreState(new CompostSave
            {
                CompostProgress = 7.5f,
                FertilizerReady = 2.25f
            });
            s.Wind.RestoreState(new WindTurbineSave { TurbineBuilt = true });
            s.Hauling.RestoreState(new HaulingSave { AirlockDumpedWeight = 33f });
            s.WeaponMaint.RestoreState(new WeaponMaintSave
            {
                Keys = new[] { "pipe_gun" },
                Values = new[] { 42f },
                JamKeys = Array.Empty<string>(),
                JamTicks = Array.Empty<int>()
            });
            s.Aesthetics.RestoreState(new AestheticsSave
            {
                Keys = new[] { "sleep" },
                Values = new[] { 0.8f }
            });
            s.Ham.RestoreState(new HamRadioSave
            {
                BroadcastDays = 11f,
                CarrierContacted = true,
                LZCleared = false
            });
            s.Skills.RestoreState(new SkillProgressionSave
            {
                Entries = new List<SurvivorProgressionSave>
                {
                    new SurvivorProgressionSave
                    {
                        SurvivorId = "sv_sk",
                        ExpertPerkEarned = true,
                        ActivePerkIds = new List<string> { "perk_a" },
                        DormantPerkIds = new List<string>(),
                        DisciplineIds = new List<string> { "scavenge" },
                        XpValues = new List<float> { 40f },
                        LastUsedDays = new List<int> { 5 }
                    }
                }
            });
            s.Combat.RestoreState(new CombatPerkSave
            {
                Entries = new List<CombatCounterSave>
                {
                    new CombatCounterSave { SurvivorId = "sv_c", JamsSurvived = 3, StealthKills = 1 }
                }
            });
            s.Survival.RestoreState(new SurvivalPerkSave
            {
                Entries = new List<SurvivalCounterSave>
                {
                    new SurvivalCounterSave { SurvivorId = "sv_s", MealsCooked = 6, CropsHarvested = 2 }
                }
            });
            s.ShelterPerks.RestoreState(new ShelterPerkSave
            {
                ConsecutiveWarmDays = 4,
                Entries = new List<ShelterCounterSave>
                {
                    new ShelterCounterSave { SurvivorId = "sv_sh", JuryRigActions = 5 }
                }
            });
            s.Medical.RestoreState(new MedicalPerkSave
            {
                Entries = new List<MedicalCounterSave>
                {
                    new MedicalCounterSave { SurvivorId = "sv_m", Phase2Cures = 2 }
                },
                CleanAmputeeIds = new List<string>(),
                DeathsDoor = new List<DeathsDoorSave>()
            });
            s.Expedition.RestoreState(new ExpeditionPerkSave
            {
                Entries = new List<ExpeditionCounterSave>
                {
                    new ExpeditionCounterSave { SurvivorId = "sv_e", TrapsDisarmed = 3 }
                }
            });
            s.Social.RestoreState(new SocialPerkSave
            {
                Entries = new List<SocialCounterSave>
                {
                    new SocialCounterSave { SurvivorId = "sv_so", PeacefulDeEscalations = 2 }
                }
            });
            s.Personal.RestoreState(new PersonalQuestSave
            {
                PillarOfAtlasDeathDebuffActive = true,
                LivingSaintInspiredActive = false,
                Entries = new List<PersonalQuestEntrySave>
                {
                    new PersonalQuestEntrySave
                    {
                        SurvivorId = "sv_pq",
                        ArchetypeId = "vet",
                        QuestlineId = "quest_b4",
                        QuestActive = true,
                        Stage = 2,
                        Progress = 0.4f
                    }
                }
            });
            return s;
        }

        private static Batch4Systems CreateEmpty()
        {
            return new Batch4Systems
            {
                Radio = new FactionRadioInterceptSystem(),
                Debt = new DebtCollectorSystem(),
                Ghost = new GhostStationSystem(),
                Lifeboat = new LifeboatTransmissionSystem(),
                Resilience = new ResilienceSystem(),
                Compost = new CompostSystem(),
                Wind = new WindTurbineSystem(),
                Hauling = new InternalHaulingSystem(),
                WeaponMaint = new WeaponMaintenanceSystem(),
                Aesthetics = new RoomAestheticsSystem(),
                Ham = new HamRadioSystem(),
                Skills = new SkillProgressionSystem(),
                Combat = new CombatPerkSystem(),
                Survival = new SurvivalPerkSystem(),
                ShelterPerks = new ShelterPerkSystem(),
                Medical = new MedicalPerkSystem(),
                Expedition = new ExpeditionPerkSystem(),
                Social = new SocialPerkSystem(),
                Personal = new PersonalQuestSystem()
            };
        }

        private static void Wire(SaveSystem ss, Batch4Systems s)
        {
            ss.SetFactionRadioIntercepts(s.Radio);
            ss.SetDebtCollectorSystem(s.Debt);
            ss.SetGhostStationSystem(s.Ghost);
            ss.SetLifeboatTransmissionSystem(s.Lifeboat);
            ss.SetResilienceSystem(s.Resilience);
            ss.SetCompostSystem(s.Compost);
            ss.SetWindTurbineSystem(s.Wind);
            ss.SetHaulingSystem(s.Hauling);
            ss.SetWeaponMaintenanceSystem(s.WeaponMaint);
            ss.SetAestheticsSystem(s.Aesthetics);
            ss.SetHamRadioSystem(s.Ham);
            ss.SetSkillProgressionSystem(s.Skills);
            ss.SetCombatPerkSystem(s.Combat);
            ss.SetSurvivalPerkSystem(s.Survival);
            ss.SetShelterPerkSystem(s.ShelterPerks);
            ss.SetMedicalPerkSystem(s.Medical);
            ss.SetExpeditionPerkSystem(s.Expedition);
            ss.SetSocialPerkSystem(s.Social);
            ss.SetPersonalQuestSystem(s.Personal);
        }

        [Test]
        public void Capture_DoesNotDualWrite_PositionalDtos_Batch4()
        {
            var seeded = CreateSeeded();
            string dir = SaveSystemTestFactory.TempDir("dualpath_b4_no_dual");
            try
            {
                var ss = SaveSystemTestFactory.MakeSave(dir, s => Wire(s, seeded));
                Assert.IsTrue(ss.Save("b4_slot"));
                var data = JsonUtility.FromJson<SaveData>(
                    File.ReadAllText(Path.Combine(dir, "save_b4_slot.json")));
                Assert.IsNotNull(data);

                Assert.IsFalse(data.FactionRadioIntercepts != null && data.FactionRadioIntercepts.NextSeq == 4,
                    "faction_radio must not dual-write NextSeq");
                Assert.IsFalse(data.DebtCollector != null && data.DebtCollector.NextSeq == 3,
                    "debt_collector must not dual-write NextSeq");
                Assert.IsFalse(data.GhostStations != null && data.GhostStations.Unlocked,
                    "ghost_stations must not dual-write Unlocked");
                Assert.IsFalse(data.Lifeboat != null && data.Lifeboat.Contacted,
                    "lifeboat must not dual-write Contacted");
                Assert.IsFalse(data.Resilience?.Keys != null && data.Resilience.Keys.Length > 0,
                    "resilience must not dual-write keys");
                Assert.IsFalse(data.Compost != null && data.Compost.CompostProgress > 0.01f,
                    "compost must not dual-write progress");
                Assert.IsFalse(data.WindTurbine != null && data.WindTurbine.TurbineBuilt,
                    "wind_turbine must not dual-write built");
                Assert.IsFalse(data.Hauling != null && data.Hauling.AirlockDumpedWeight > 0.01f,
                    "hauling must not dual-write weight");
                Assert.IsFalse(data.WeaponMaint?.Keys != null && data.WeaponMaint.Keys.Length > 0,
                    "weapon_maint must not dual-write keys");
                Assert.IsFalse(data.Aesthetics?.Keys != null && data.Aesthetics.Keys.Length > 0,
                    "aesthetics must not dual-write keys");
                Assert.IsFalse(data.HamRadio != null && data.HamRadio.CarrierContacted,
                    "ham_radio must not dual-write carrier");
                Assert.IsFalse(data.SkillProgression?.Entries != null && data.SkillProgression.Entries.Count > 0,
                    "skill_progression must not dual-write entries");
                Assert.IsFalse(data.CombatPerks?.Entries != null && data.CombatPerks.Entries.Count > 0,
                    "combat_perks must not dual-write entries");
                Assert.IsFalse(data.SurvivalPerks?.Entries != null && data.SurvivalPerks.Entries.Count > 0,
                    "survival_perks must not dual-write entries");
                Assert.IsFalse(data.ShelterPerks != null && data.ShelterPerks.ConsecutiveWarmDays == 4,
                    "shelter_perks must not dual-write warm days");
                Assert.IsFalse(data.MedicalPerks?.Entries != null && data.MedicalPerks.Entries.Count > 0,
                    "medical_perks must not dual-write entries");
                Assert.IsFalse(data.ExpeditionPerks?.Entries != null && data.ExpeditionPerks.Entries.Count > 0,
                    "expedition_perks must not dual-write entries");
                Assert.IsFalse(data.SocialPerks?.Entries != null && data.SocialPerks.Entries.Count > 0,
                    "social_perks must not dual-write entries");
                Assert.IsFalse(data.PersonalQuests != null && data.PersonalQuests.PillarOfAtlasDeathDebuffActive,
                    "personal_quests must not dual-write pillar debuff");

                Assert.IsNotNull(data.SubsystemSaveIds);
                foreach (string id in MigratedIds)
                {
                    Assert.IsTrue(ListContains(data.SubsystemSaveIds, id),
                        $"SubsystemSaveIds must include '{id}'");
                    int idx = IndexOfId(data.SubsystemSaveIds, id);
                    Assert.IsFalse(string.IsNullOrEmpty(data.SubsystemSaveJsons[idx]));
                }

                int debtIdx = IndexOfId(data.SubsystemSaveIds, "debt_collector");
                Assert.IsTrue(data.SubsystemSaveJsons[debtIdx].Contains("debt_b4"));
                int compostIdx = IndexOfId(data.SubsystemSaveIds, "compost");
                Assert.IsTrue(data.SubsystemSaveJsons[compostIdx].Contains("7.5")
                    || data.SubsystemSaveJsons[compostIdx].Contains("7.50"));
                int personalIdx = IndexOfId(data.SubsystemSaveIds, "personal_quests");
                Assert.IsTrue(data.SubsystemSaveJsons[personalIdx].Contains("sv_pq")
                    || data.SubsystemSaveJsons[personalIdx].Contains("PillarOfAtlas"));
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void RoundTrip_ViaSubsystemIds_RestoresBatch4()
        {
            var seeded = CreateSeeded();
            string dir = SaveSystemTestFactory.TempDir("dualpath_b4_rt");
            try
            {
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, seeded)).Save("b4_rt"));
                var loaded = CreateEmpty();
                Assert.IsTrue(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, loaded)).Load("b4_rt"));

                Assert.AreEqual(4, loaded.Radio.CaptureState().NextSeq);
                Assert.IsTrue(loaded.Radio.CaptureState().HudHasUnread);
                Assert.AreEqual("batch4 radio seed", loaded.Radio.CaptureState().Entries[0].Message);
                Assert.AreEqual(3, loaded.Debt.CaptureState().NextSeq);
                Assert.AreEqual("debt_b4", loaded.Debt.CaptureState().Debts[0].Id);
                Assert.IsTrue(loaded.Ghost.CaptureState().Unlocked);
                Assert.IsTrue(loaded.Lifeboat.CaptureState().Contacted);
                Assert.AreEqual("sv_life", loaded.Lifeboat.CaptureState().ExtractedSurvivorId);
                Assert.AreEqual(15f, loaded.Resilience.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(7.5f, loaded.Compost.CaptureState().CompostProgress, 0.01f);
                Assert.IsTrue(loaded.Wind.CaptureState().TurbineBuilt);
                Assert.AreEqual(33f, loaded.Hauling.CaptureState().AirlockDumpedWeight, 0.01f);
                Assert.AreEqual(42f, loaded.WeaponMaint.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(0.8f, loaded.Aesthetics.CaptureState().Values[0], 0.01f);
                Assert.IsTrue(loaded.Ham.CaptureState().CarrierContacted);
                Assert.AreEqual(11f, loaded.Ham.CaptureState().BroadcastDays, 0.01f);
                Assert.AreEqual("sv_sk", loaded.Skills.CaptureState().Entries[0].SurvivorId);
                Assert.IsTrue(loaded.Skills.CaptureState().Entries[0].ExpertPerkEarned);
                Assert.AreEqual(3, loaded.Combat.CaptureState().Entries[0].JamsSurvived);
                Assert.AreEqual(6, loaded.Survival.CaptureState().Entries[0].MealsCooked);
                Assert.AreEqual(4, loaded.ShelterPerks.CaptureState().ConsecutiveWarmDays);
                Assert.AreEqual(2, loaded.Medical.CaptureState().Entries[0].Phase2Cures);
                Assert.AreEqual(3, loaded.Expedition.CaptureState().Entries[0].TrapsDisarmed);
                Assert.AreEqual(2, loaded.Social.CaptureState().Entries[0].PeacefulDeEscalations);
                Assert.IsTrue(loaded.Personal.CaptureState().PillarOfAtlasDeathDebuffActive);
                Assert.AreEqual("sv_pq", loaded.Personal.CaptureState().Entries[0].SurvivorId);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }

        [Test]
        public void LegacyRestIf_PositionalOnly_StillRestoresBatch4()
        {
            var data = new SaveData
            {
                SaveVersion = SaveSystem.CurrentSaveVersion,
                GameState = new GameStateSave { Day = 5, Phase = GamePhase.Running },
                FactionRadioIntercepts = new FactionRadioInterceptSave
                {
                    NextSeq = 2,
                    HudTunerIndex = 1,
                    Entries = new[]
                    {
                        new FactionRadioInterceptSystem.InterceptEntry
                        {
                            Id = "leg_int",
                            FactionId = "cult",
                            Kind = "Parley",
                            Message = "legacy radio",
                            Day = 3
                        }
                    }
                },
                DebtCollector = new DebtCollectorSave
                {
                    NextSeq = 1,
                    Debts = new[]
                    {
                        new DebtEntrySave { Id = "leg_debt", FactionId = "raiders", FuelDemanded = 2f }
                    }
                },
                GhostStations = new GhostStationSave
                {
                    Unlocked = true,
                    HeardStationIds = new[] { "ghost_civil_defense" }
                },
                Lifeboat = new LifeboatTransmissionSave
                {
                    Contacted = true,
                    Offered = false,
                    Resolved = false,
                    ExtractedSurvivorId = "leg_life",
                    LeftBehindIds = Array.Empty<string>(),
                    LeftBehindNames = Array.Empty<string>()
                },
                Resilience = new ResilienceSave { Keys = new[] { "leg_r" }, Values = new[] { 10f } },
                Compost = new CompostSave { CompostProgress = 3f, FertilizerReady = 1f },
                WindTurbine = new WindTurbineSave { TurbineBuilt = true },
                Hauling = new HaulingSave { AirlockDumpedWeight = 9f },
                WeaponMaint = new WeaponMaintSave
                {
                    Keys = new[] { "leg_gun" },
                    Values = new[] { 55f },
                    JamKeys = Array.Empty<string>(),
                    JamTicks = Array.Empty<int>()
                },
                Aesthetics = new AestheticsSave { Keys = new[] { "entry" }, Values = new[] { 0.5f } },
                HamRadio = new HamRadioSave { BroadcastDays = 4f, CarrierContacted = false },
                SkillProgression = new SkillProgressionSave
                {
                    Entries = new List<SurvivorProgressionSave>
                    {
                        new SurvivorProgressionSave
                        {
                            SurvivorId = "leg_sk",
                            ActivePerkIds = new List<string>(),
                            DormantPerkIds = new List<string>(),
                            DisciplineIds = new List<string> { "craft" },
                            XpValues = new List<float> { 12f },
                            LastUsedDays = new List<int> { 1 }
                        }
                    }
                },
                CombatPerks = new CombatPerkSave
                {
                    Entries = new List<CombatCounterSave>
                    {
                        new CombatCounterSave { SurvivorId = "leg_c", HumanKills = 1 }
                    }
                },
                SurvivalPerks = new SurvivalPerkSave
                {
                    Entries = new List<SurvivalCounterSave>
                    {
                        new SurvivalCounterSave { SurvivorId = "leg_s", MedicalCrafts = 2 }
                    }
                },
                ShelterPerks = new ShelterPerkSave
                {
                    ConsecutiveWarmDays = 1,
                    Entries = new List<ShelterCounterSave>
                    {
                        new ShelterCounterSave { SurvivorId = "leg_sh", RoomsCleared = 1 }
                    }
                },
                MedicalPerks = new MedicalPerkSave
                {
                    Entries = new List<MedicalCounterSave>
                    {
                        new MedicalCounterSave { SurvivorId = "leg_m", Phase2Cures = 1 }
                    },
                    CleanAmputeeIds = new List<string>(),
                    DeathsDoor = new List<DeathsDoorSave>()
                },
                ExpeditionPerks = new ExpeditionPerkSave
                {
                    Entries = new List<ExpeditionCounterSave>
                    {
                        new ExpeditionCounterSave { SurvivorId = "leg_e", MaxWeightReturns = 2 }
                    }
                },
                SocialPerks = new SocialPerkSave
                {
                    Entries = new List<SocialCounterSave>
                    {
                        new SocialCounterSave { SurvivorId = "leg_so", HighMoraleDays = 3 }
                    }
                },
                PersonalQuests = new PersonalQuestSave
                {
                    LivingSaintInspiredActive = true,
                    Entries = new List<PersonalQuestEntrySave>
                    {
                        new PersonalQuestEntrySave
                        {
                            SurvivorId = "leg_pq",
                            QuestlineId = "leg_quest",
                            Stage = 1
                        }
                    }
                },
                SubsystemSaveIds = new List<string>(),
                SubsystemSaveJsons = new List<string>()
            };

            string dir = SaveSystemTestFactory.TempDir("dualpath_b4_legacy");
            try
            {
                var systems = CreateEmpty();
                InvokeRestoreFromSnapshot(SaveSystemTestFactory.MakeSave(dir, s => Wire(s, systems)), data);

                Assert.AreEqual(2, systems.Radio.CaptureState().NextSeq);
                Assert.AreEqual("legacy radio", systems.Radio.CaptureState().Entries[0].Message);
                Assert.AreEqual("leg_debt", systems.Debt.CaptureState().Debts[0].Id);
                Assert.IsTrue(systems.Ghost.CaptureState().Unlocked);
                Assert.AreEqual("leg_life", systems.Lifeboat.CaptureState().ExtractedSurvivorId);
                Assert.AreEqual(10f, systems.Resilience.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(3f, systems.Compost.CaptureState().CompostProgress, 0.01f);
                Assert.IsTrue(systems.Wind.CaptureState().TurbineBuilt);
                Assert.AreEqual(9f, systems.Hauling.CaptureState().AirlockDumpedWeight, 0.01f);
                Assert.AreEqual(55f, systems.WeaponMaint.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(0.5f, systems.Aesthetics.CaptureState().Values[0], 0.01f);
                Assert.AreEqual(4f, systems.Ham.CaptureState().BroadcastDays, 0.01f);
                Assert.AreEqual("leg_sk", systems.Skills.CaptureState().Entries[0].SurvivorId);
                Assert.AreEqual(1, systems.Combat.CaptureState().Entries[0].HumanKills);
                Assert.AreEqual(2, systems.Survival.CaptureState().Entries[0].MedicalCrafts);
                Assert.AreEqual(1, systems.ShelterPerks.CaptureState().ConsecutiveWarmDays);
                Assert.AreEqual(1, systems.Medical.CaptureState().Entries[0].Phase2Cures);
                Assert.AreEqual(2, systems.Expedition.CaptureState().Entries[0].MaxWeightReturns);
                Assert.AreEqual(3, systems.Social.CaptureState().Entries[0].HighMoraleDays);
                Assert.IsTrue(systems.Personal.CaptureState().LivingSaintInspiredActive);
                Assert.AreEqual("leg_pq", systems.Personal.CaptureState().Entries[0].SurvivorId);
            }
            finally
            {
                if (Directory.Exists(dir)) Directory.Delete(dir, true);
            }
        }
    }
}
