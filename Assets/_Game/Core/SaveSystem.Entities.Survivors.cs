using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {

        private SurvivorSave CaptureSurvivor(Survivor sv)
        {
            var save = new SurvivorSave
            {
                Id = sv.Id,
                DisplayName = sv.DisplayName,
                State = sv.State,

                Hunger = sv.Needs.Hunger,
                Thirst = sv.Needs.Thirst,
                Fatigue = sv.Needs.Fatigue,
                Warmth = sv.Needs.Warmth,
                Morale = sv.Needs.Morale,
                Health = sv.Needs.Health,
                WasHungerCritical = sv.Needs.WasHungerCritical,
                WasThirstCritical = sv.Needs.WasThirstCritical,
                WasWarmthCritical = sv.Needs.WasWarmthCritical,

                RadiationDose = sv.RadiationDose,
                LifetimeRadiationExposure = sv.LifetimeRadiationExposure,
                HasAcuteRadiationSickness = sv.HasAcuteRadiationSickness,
                HasChronicIllness = sv.HasChronicIllness,
                HasRadResistance = sv.HasRadResistance,
                RadResistanceHoursRemaining = sv.RadResistanceHoursRemaining,
                HasFullSuitEquipped = sv.HasFullSuitEquipped,

                // Latent damage / prognosis pipeline
                AcuteDoseWindow = sv.AcuteDoseWindow,
                PrognosisStage = sv.PrognosisStage,
                OnsetTimer = sv.OnsetTimer,
                LatentDamage = sv.LatentDamage,
                IodineProtectionTimer = sv.IodineProtectionTimer,
                HasAcuteRadiationSyndrome = sv.HasAcuteRadiationSyndrome,

                // Light / photoperiod
                LightExposure = sv.LightExposure,
                VitaminDProxy = sv.VitaminDProxy,
                IsListless    = sv.IsListless,

                MedicalSkill  = sv.MedicalSkill,

                // Mental-break system (Prompt #29)
                CurrentMentalBreakId    = sv.currentMentalBreakId ?? string.Empty,
                LowMoraleHours          = sv.lowMoraleHours,
                MentalBreakCureProgress = sv.mentalBreakCureProgress,

                // Internal mysteries (Prompt #45) — permanent Fractured scar
                IsFractured             = sv.IsFractured,

                // Prompt #10/8/7/9 — new fields
                ScienceSkill             = sv.ScienceSkill,
                CraftingSkill            = sv.CraftingSkill,
                ExpertDisciplineId       = sv.ExpertDisciplineId ?? string.Empty,
                ConsecutiveLowMoraleDays = sv.ConsecutiveLowMoraleDays,
                AtrophiedSkills          = sv.AtrophiedSkills != null ? new List<string>(sv.AtrophiedSkills) : new List<string>(),
                Traits                   = sv.Traits != null ? new List<string>(sv.Traits) : new List<string>(),
                Traumas                  = sv.Traumas != null ? new List<string>(sv.Traumas) : new List<string>(),
                RiskBias                 = sv.RiskBias,
                HoursSinceLastDose       = sv.HoursSinceLastDose,
                IsInWithdrawal           = sv.IsInWithdrawal,
                CannotScavenge           = sv.CannotScavenge,
                CannotCraft              = sv.CannotCraft,
                CannotFight              = sv.CannotFight,
                IsChild                  = sv.IsChild,
                ConsumptionHistory       = sv.ConsumptionHistory != null
                    ? sv.ConsumptionHistory.ConvertAll(c => new ConsumptionRecordSave { ItemId = c.ItemId, DayConsumed = c.DayConsumed })
                    : new List<ConsumptionRecordSave>(),

                // Belief / risk-perception (pre-existing gap)
                PerceivedRadRisk         = sv.PerceivedRadRisk,
                TrustInInstruments       = sv.TrustInInstruments,
                RadiationAnxiety         = sv.RadiationAnxiety,
                Numbness                 = sv.Numbness,
                HasRadiationAnxietyStatus = sv.HasRadiationAnxietyStatus,
                IsNumb                   = sv.IsNumb,
                CurrentRoomId            = sv.CurrentRoomId ?? string.Empty,

                // Chronic Disease (pre-existing gap)
                ActiveChronicIllness       = sv.ActiveChronicIllness.HasValue ? sv.ActiveChronicIllness.Value.ToString() : string.Empty,
                ChronicIllnessManagedHours = sv.ChronicIllnessManagedHours,
                DisabilityIds             = sv.DisabilityIds != null ? new List<string>(sv.DisabilityIds) : new List<string>(),

                // Prompt #165 — Clothing degradation (round-trip safe).
                ClothingDurability = sv.ClothingDurability,
                IsRagged           = sv.IsRagged,

                // Grief keepsakes (item ids memorialized after a death).
                KeepsakeItemIds = sv.KeepsakeItemIds != null
                    ? new List<string>(sv.KeepsakeItemIds)
                    : new List<string>(),

                // Prompts #214–#219 — personal quest destiny.
                ArchetypeId          = sv.ArchetypeId ?? string.Empty,
                LatentExpertTraitId  = sv.LatentExpertTraitId ?? string.Empty,
                ActiveQuestlineId    = sv.ActiveQuestlineId ?? string.Empty,
                QuestlineActive      = sv.QuestlineActive,
                LatentTraitUnlocked  = sv.LatentTraitUnlocked,
                QuestStage           = sv.QuestStage,
                QuestProgress        = sv.QuestProgress,
                DaysAlive            = sv.DaysAlive,
                MoraleHitZero        = sv.MoraleHitZero
            };

            if (_radiationSystem != null && !string.IsNullOrEmpty(sv.Id))
            {
                var dos = _radiationSystem.GetDosimeter(sv.Id);
                if (dos != null)
                {
                    save.DosimeterRate = dos.CurrentRate;
                    save.DosimeterRecent = dos.RecentExposure;
                }
            }

            return save;
        }

        private void RestoreSurvivor(Survivor sv, SurvivorSave save)
        {
            sv.Id = save.Id;
            sv.DisplayName = save.DisplayName;
            sv.State = save.State;

            sv.Needs.Hunger = save.Hunger;
            sv.Needs.Thirst = save.Thirst;
            sv.Needs.Fatigue = save.Fatigue;
            sv.Needs.Warmth = save.Warmth;
            sv.Needs.Morale = save.Morale;
            sv.Needs.Health = save.Health;
            sv.Needs.WasHungerCritical = save.WasHungerCritical;
            sv.Needs.WasThirstCritical = save.WasThirstCritical;
            sv.Needs.WasWarmthCritical = save.WasWarmthCritical;

            sv.RadiationDose = save.RadiationDose;
            sv.LifetimeRadiationExposure = save.LifetimeRadiationExposure;
            sv.HasAcuteRadiationSickness = save.HasAcuteRadiationSickness;
            sv.HasChronicIllness = save.HasChronicIllness;
            sv.HasRadResistance = save.HasRadResistance;
            sv.RadResistanceHoursRemaining = save.RadResistanceHoursRemaining;
            sv.HasFullSuitEquipped = save.HasFullSuitEquipped;

            // Latent damage / prognosis pipeline
            sv.AcuteDoseWindow = save.AcuteDoseWindow;
            sv.PrognosisStage = save.PrognosisStage;
            sv.OnsetTimer = save.OnsetTimer;
            sv.LatentDamage = save.LatentDamage;
            sv.IodineProtectionTimer = save.IodineProtectionTimer;
            sv.HasAcuteRadiationSyndrome = save.HasAcuteRadiationSyndrome;

            // Light / photoperiod
            sv.LightExposure = save.LightExposure;
            sv.VitaminDProxy = save.VitaminDProxy;
            sv.IsListless    = save.IsListless;

            sv.MedicalSkill  = save.MedicalSkill;

            // Mental-break system (Prompt #29)
            sv.currentMentalBreakId    = save.CurrentMentalBreakId;
            sv.lowMoraleHours          = save.LowMoraleHours;
            sv.mentalBreakCureProgress = save.MentalBreakCureProgress;

            // Internal mysteries (Prompt #45)
            sv.IsFractured             = save.IsFractured;

            // Prompt #10/8/7/9 — new fields
            sv.ScienceSkill             = save.ScienceSkill;
            sv.CraftingSkill            = save.CraftingSkill;
            sv.ExpertDisciplineId       = save.ExpertDisciplineId;
            sv.ConsecutiveLowMoraleDays = save.ConsecutiveLowMoraleDays;
            sv.AtrophiedSkills          = save.AtrophiedSkills != null ? new List<string>(save.AtrophiedSkills) : new List<string>();
            sv.Traits                   = save.Traits != null ? new List<string>(save.Traits) : new List<string>();
            sv.Traumas                  = save.Traumas != null ? new List<string>(save.Traumas) : new List<string>();
            sv.RiskBias                 = save.RiskBias;
            sv.HoursSinceLastDose       = save.HoursSinceLastDose;
            sv.IsInWithdrawal           = save.IsInWithdrawal;
            sv.CannotScavenge           = save.CannotScavenge;
            sv.CannotCraft              = save.CannotCraft;
            sv.CannotFight              = save.CannotFight;
            sv.IsChild                  = save.IsChild;
            sv.ConsumptionHistory       = save.ConsumptionHistory != null
                ? save.ConsumptionHistory.ConvertAll(c => new Survivors.ConsumptionRecord(c.ItemId, c.DayConsumed))
                : new List<Survivors.ConsumptionRecord>();

            // Belief / risk-perception (pre-existing gap)
            sv.PerceivedRadRisk          = save.PerceivedRadRisk;
            sv.TrustInInstruments        = save.TrustInInstruments;
            sv.RadiationAnxiety          = save.RadiationAnxiety;
            sv.Numbness                  = save.Numbness;
            sv.HasRadiationAnxietyStatus = save.HasRadiationAnxietyStatus;
            sv.IsNumb                    = save.IsNumb;
            sv.CurrentRoomId             = string.IsNullOrEmpty(save.CurrentRoomId) ? null : save.CurrentRoomId;

            // Chronic Disease (pre-existing gap)
            if (!string.IsNullOrEmpty(save.ActiveChronicIllness) && System.Enum.TryParse<Survivors.ChronicIllnessKind>(save.ActiveChronicIllness, out var kind))
                sv.ActiveChronicIllness = kind;
            else
                sv.ActiveChronicIllness = null;
            sv.ChronicIllnessManagedHours = save.ChronicIllnessManagedHours;
            sv.DisabilityIds              = save.DisabilityIds != null ? new List<string>(save.DisabilityIds) : new List<string>();

            // Prompt #165 — Clothing degradation. Survives save/load so a Ragged
            // survivor stays Ragged across sessions and the morale drain continues.
            sv.ClothingDurability = save.ClothingDurability;
            sv.IsRagged           = save.IsRagged;

            // Grief keepsakes.
            sv.KeepsakeItemIds = save.KeepsakeItemIds != null
                ? new List<string>(save.KeepsakeItemIds)
                : new List<string>();

            // Prompts #214–#219 — personal quest destiny.
            sv.ArchetypeId         = save.ArchetypeId ?? string.Empty;
            sv.LatentExpertTraitId = save.LatentExpertTraitId ?? string.Empty;
            sv.ActiveQuestlineId   = save.ActiveQuestlineId ?? string.Empty;
            sv.QuestlineActive     = save.QuestlineActive;
            sv.LatentTraitUnlocked = save.LatentTraitUnlocked;
            sv.QuestStage          = save.QuestStage;
            sv.QuestProgress       = save.QuestProgress;
            sv.DaysAlive           = save.DaysAlive;
            sv.MoraleHitZero       = save.MoraleHitZero;
        }

        private void RestoreShelterModules(List<ShelterModuleSave> saved)
        {
            foreach (var modSave in saved)
            {
                var instance = _shelter.GetModule(modSave.ModuleId);
                if (instance == null)
                {
                    instance = new ShelterModuleInstance(modSave.ModuleId);
                    _shelter.AddModule(instance);
                }

                instance.Level = modSave.Level;
                instance.IsEnabled = modSave.IsEnabled;
                instance.FilterHealth = modSave.FilterHealth;
                instance.Fuel = modSave.Fuel;
                instance.WaterConversionProgress = modSave.WaterConversionProgress;
                instance.RoomId = modSave.RoomId;
                instance.Occupancy = modSave.Occupancy;
                instance.ComfortLevel = modSave.ComfortLevel;
                instance.Capacity = modSave.Capacity;

                if (_moduleLookup != null)
                {
                    instance.Definition = _moduleLookup(modSave.ModuleId);
                }
            }
        }

        private void RestoreDosimeters(List<SurvivorSave> survivors)
        {
            foreach (var svSave in survivors)
            {
                if (string.IsNullOrEmpty(svSave.Id)) continue;
                var dos = _radiationSystem.GetDosimeter(svSave.Id);
                dos.CurrentRate = svSave.DosimeterRate;
                dos.RecentExposure = svSave.DosimeterRecent;
                dos.LifetimeDose = svSave.LifetimeRadiationExposure;
            }
        }

    }
}
