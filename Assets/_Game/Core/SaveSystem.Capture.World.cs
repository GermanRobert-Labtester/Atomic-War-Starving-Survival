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
            // RegisterSystem dual-path CapIf fully removed (batches 1–4 + audit).
            // RestIf keeps positional DTOs for pre-migration saves.
            //
            // Remaining special-path CapIf only (field injects, not RegisterSystem):
            //   • EventRunner — CaptureScheduledState / RestoreScheduledState
            //   • ShiftingHotspot — Bind(_generatedMap, _knowledgeMap) before restore
            //   • FactionRaidPlan — SetMap before restore
            // (Expedition + GeneratedMap captured in Capture.Entities.)
            CapIf(_eventRunner, s => data.ScheduledEvents = s.CaptureScheduledState());
            CapIf(_shiftingHotspots, s => data.ShiftingHotspots = s.CaptureState());
            CapIf(_factionRaidPlans, s => data.FactionRaidPlans = s.CaptureState());
        }
    }
}
