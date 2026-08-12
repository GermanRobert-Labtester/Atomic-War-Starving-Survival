using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {
        private void TickSystems(float gameHours)
        {
            if (gameHours <= 0f) return;
            if (_mentalBreakRng == null) WarmDayTickCaches();

            int currentDay = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // SystemRegistry owns hour + day-gated system ticks. _registry is
            // constructed unconditionally in InitFoundation(), so this is only
            // null for a test host that skipped init entirely — same guard
            // RegisterSystemsInRegistry() and DisposeSubscriptions() use.
            //
            // H-8: this used to fall back to a second, hand-maintained copy of
            // the tick list when _registry was null. That path was unreachable
            // in practice (see above) but still had to be kept in sync by hand;
            // it had already drifted once (AUDIT-008: bunker_social, personal
            // quests, chemistry/titles, rebuilders were missing from it). Removed
            // rather than fixed, since there is nothing that legitimately reaches it.
            if (_registry == null) return;
            _registry.TickAll(gameHours, currentDay);
            TickHostSideEffectsAfterRegistry(gameHours);

            TickAiWave(gameHours);
            TickEventsAndJournal(gameHours);
        }

        /// <summary>
        /// Host-only side effects that are not independent systems in the registry
        /// (perk timers, economy reactions). Runs once after registry dispatch.
        /// </summary>
        private void TickHostSideEffectsAfterRegistry(float gameHours)
        {
            // Prompt #205 — Death's Door timer; expired → true death.
            if (MedicalPerks != null && NeedsSystem != null)
            {
                MedicalPerks.TickDeathsDoor(
                    gameHours,
                    Survivors,
                    forceDeath: sv => NeedsSystem.ForceDeath(sv));
            }

            // Cult of the Glow: rad drop across healthy ceiling → hatch raid cascade.
            EconomySystem?.NotifyPartyRadiationChanged();
        }

        private void TickHazardMethaneFromShelterAir(float gameHours)
        {
            if (HazardMethane == null || Shelter == null) return;
            HazardMethane.Tick(gameHours, Shelter.AirQuality);
        }

        /// <summary>
        /// Prompt #167 — compost heat/mold conversion while waste is present.
        /// Daily waste intake is handled by SystemWiring; this is the hour tick.
        /// </summary>
        private void TickCompost(float gameHours)
        {
            if (CompostSystem == null || Shelter == null) return;
            ShelterRoom room = null;
            if (Shelter.Rooms != null && Shelter.Rooms.Count > 0)
            {
                // Prefer a room that already hosts the compost module when present.
                var mod = Shelter.GetModule(CompostSystem.CompostModuleId);
                if (mod != null && !string.IsNullOrEmpty(mod.RoomId))
                    room = Shelter.GetRoom(mod.RoomId);
                if (room == null)
                    room = Shelter.Rooms[0];
            }
            CompostSystem.Tick(gameHours, room);
        }

        private void TickHouseToBunkerDaily(int currentDay)
        {
            if (HouseToBunkerSystem == null || TimeSystem == null) return;
            if (WorldPhaseSystem == null) return;

            if (_lastHouseDay != currentDay && currentDay < WorldPhaseSystem.FlashpointDay)
            {
                _lastHouseDay = currentDay;
                if (WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                {
                    HouseToBunkerSystem.ApplyArtilleryDamage();
                }
            }

            if (currentDay >= WorldPhaseSystem.FlashpointDay
                && !HouseToBunkerSystem.HouseDestroyed
                && WorldPhaseSystem.HasTriggeredExchange)
            {
                HouseToBunkerSystem.CollapseHouse();
            }

            if (ExpeditionSystem != null)
                ExpeditionSystem.HatchBlocksExpeditions = HouseToBunkerSystem.HatchBlocked;
            if (Shelter != null)
                Shelter.OverworldShieldingBonus = HouseToBunkerSystem.GetEffectiveShielding();
        }

    }
}
