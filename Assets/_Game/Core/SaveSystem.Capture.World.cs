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
            // Core world + faction social dual-path CapIf removed (batches 1–4) —
            // RegisterSystem owns capture. RestIf keeps positional DTOs for
            // pre-migration saves. EventRunner stays special-path (CaptureScheduledState).
            CapIf(_eventRunner, s => data.ScheduledEvents = s.CaptureScheduledState());

            // Complex special-path (Bind/SetMap before restore) — keep CapIf.
            CapIf(_shiftingHotspots, s => data.ShiftingHotspots = s.CaptureState());
            CapIf(_factionRaidPlans, s => data.FactionRaidPlans = s.CaptureState());
            // faction_radio_intercepts / debt_collector / ghost_stations / lifeboat —
            // dual-path CapIf removed (batch 4); RegisterSystem owns capture.
            // tracker / dead_drops / hostage…cult_moral — already migrated.
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
            // Simulation extras + perks + personal_quests — dual-path CapIf removed
            // (batch 4); RegisterSystem + SubsystemSaveIds own capture. RestIf still
            // reads positional DTOs for pre-migration saves.
        }
    }
}
