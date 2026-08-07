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
        /// <summary>Capture when system is non-null; collapses repeated if-null chains.</summary>
        private static void CapIf<T>(T system, Action<T> capture) where T : class
        {
            if (system != null) capture(system);
        }

        private void CaptureWorldAndFactionSystems(SaveData data)
        {
            // Core world dual-path CapIf removed (world_phase…sabotaged_caches) —
            // RegisterSystem owns capture. RestIf keeps positional DTOs for
            // pre-migration saves. EventRunner stays special-path (CaptureScheduledState).
            CapIf(_factionRadioIntercepts, s => data.FactionRadioIntercepts = s.CaptureState());
            CapIf(_eventRunner, s => data.ScheduledEvents = s.CaptureScheduledState());

            // Complex special-path / remaining dual-path faction side
            CapIf(_shiftingHotspots, s => data.ShiftingHotspots = s.CaptureState());
            CapIf(_factionRaidPlans, s => data.FactionRaidPlans = s.CaptureState());
            CapIf(_debtCollector, s => data.DebtCollector = s.CaptureState());
            CapIf(_ghostStations, s => data.GhostStations = s.CaptureState());
            CapIf(_lifeboat, s => data.Lifeboat = s.CaptureState());
            // tracker / dead_drops / hostage / propaganda / deserter / scapegoat /
            // labor_camp / cult_moral — dual-path CapIf removed; RegisterSystem owns capture.
            // RestIf still reads positional DTOs for pre-migration saves.

            // Narrative side systems (batch 1–2) already migrated off CapIf.
        }

        private void CaptureShelterTacticalSystems(SaveData data)
        {
            // Shelter tactical family (structural_integrity … noise) — dual-path CapIf
            // removed; RegisterSystem + SubsystemSaveIds own capture. RestIf still
            // reads positional DTOs for pre-migration saves.
        }

        private void CaptureSimulationExtras(SaveData data)
        {
            CapIf(_resilienceSystem, s => data.Resilience = s.CaptureState());
            CapIf(_compostSystem, s => data.Compost = s.CaptureState());
            CapIf(_windTurbineSystem, s => data.WindTurbine = s.CaptureState());
            CapIf(_haulingSystem, s => data.Hauling = s.CaptureState());
            CapIf(_weaponMaintenanceSystem, s => data.WeaponMaint = s.CaptureState());
            CapIf(_aestheticsSystem, s => data.Aesthetics = s.CaptureState());
            CapIf(_hamRadioSystem, s => data.HamRadio = s.CaptureState());
            CapIf(_skillProgression, s => data.SkillProgression = s.CaptureState());
            CapIf(_combatPerkSystem, s => data.CombatPerks = s.CaptureState());
            CapIf(_survivalPerkSystem, s => data.SurvivalPerks = s.CaptureState());
            CapIf(_shelterPerkSystem, s => data.ShelterPerks = s.CaptureState());
            CapIf(_medicalPerkSystem, s => data.MedicalPerks = s.CaptureState());
            CapIf(_expeditionPerkSystem, s => data.ExpeditionPerks = s.CaptureState());
            CapIf(_socialPerkSystem, s => data.SocialPerks = s.CaptureState());
            CapIf(_personalQuestSystem, s => data.PersonalQuests = s.CaptureState());
        }
    }
}
