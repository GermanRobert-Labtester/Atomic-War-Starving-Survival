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

            // Primary dispatch: SystemRegistry owns hour + day-gated system ticks.
            // Previously TickSystems duplicated a subset of registrations and never
            // called TickAll — so bunker_social, personal quests, chemistry/titles,
            // rebuilders, and other registry-only systems never advanced (AUDIT-008).
            if (_registry != null)
            {
                _registry.TickAll(gameHours, currentDay);
                TickHostSideEffectsAfterRegistry(gameHours);
            }
            else
            {
                // Fallback for partial test hosts that skip RegisterSystemsInRegistry.
                TickEnvironmentAndHatch(gameHours);
                TickNeedsMedicalAndPsyche(gameHours);
                TickRadiationWaterAndCraft(gameHours);
            }

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

        private void TickEnvironmentAndHatch(float gameHours)
        {
            int currentDay = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            TickWeatherShelterPower(gameHours, currentDay);
            TickStructuralWasteAndVermin(gameHours, currentDay);
            TickFactionAndWorldSideSystems(gameHours, currentDay);
            TickHouseEcosystemAndTactical(gameHours, currentDay);
            TickClothing(gameHours);
        }

        private void TickWeatherShelterPower(float gameHours, int currentDay)
        {
            WeatherSystem.Tick(gameHours);
            TemperatureSystem.Tick(gameHours);
            PhotoperiodSystem.Tick(gameHours);

            // Prompt #48 — continuous extreme weather seals the hatch.
            if (HatchEntrapmentSystem != null && WeatherSystem != null)
            {
                HatchEntrapmentSystem.Tick(
                    gameHours,
                    WeatherSystem.Current,
                    Shelter,
                    _getFactionTrustEffective,
                    _scheduleEventCached,
                    currentDay);
                SyncHatchExpeditionLock();
            }

            Shelter.Tick(gameHours);

            if (PowerNetwork != null)
            {
                string weatherName = WeatherSystem != null
                    ? WeatherNameOf(WeatherSystem.Current)
                    : null;
                PowerNetwork.Tick(gameHours, weatherName, _tryApplyPedalCostCached);
                PowerNetwork.ApplyToShelter(Shelter);
            }

            HatchDefenseSystem?.Tick(gameHours, PowerNetwork);

            // Prompt #205 — Death's Door timer; expired → true death.
            if (MedicalPerks != null && NeedsSystem != null)
            {
                MedicalPerks.TickDeathsDoor(
                    gameHours,
                    Survivors,
                    forceDeath: sv => NeedsSystem.ForceDeath(sv));
            }

            // Prompt #197 — HVAC Tech passive ventilation buff while tech is in bunker.
            if (AtmosphereSystem != null)
            {
                if (ShelterPerks != null)
                    AtmosphereSystem.VentilationClearMultiplier =
                        ShelterPerks.GetVentilationClearMultiplier(Survivors);
                AtmosphereSystem.Tick(gameHours, PowerNetwork, Shelter);
            }
            TickHazardMethaneFromShelterAir(gameHours);

            CorpseSystem?.Tick(gameHours, Survivors);
            PantrySystem?.Tick(gameHours, _storesRoom);
        }

        private void TickHazardMethaneFromShelterAir(float gameHours)
        {
            if (HazardMethane == null || Shelter == null) return;
            HazardMethane.Tick(gameHours, Shelter.AirQuality);
        }

        private void TickStructuralWasteAndVermin(float gameHours, int currentDay)
        {
            // Prompt #49 — structural integrity dust leaks + cave-in checks.
            if (StructuralIntegrity != null && WeatherSystem != null)
            {
                var weather = WeatherSystem.Current;
                if (weather == WeatherKind.FalloutStorm)
                {
                    StructuralIntegrity.ApplyDamage(
                        StructuralIntegritySystem.FalloutStormDamagePerHour * gameHours,
                        "fallout_storm");
                }
                else if (weather == WeatherKind.Blizzard && WorldPhaseSystem != null
                    && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                {
                    StructuralIntegrity.ApplyDamage(
                        StructuralIntegritySystem.MortarStrikeDamage * 0.3f * gameHours,
                        "mortar_strike");
                }
                StructuralIntegrity.Tick(gameHours, Shelter);
            }

            WasteSystem?.Tick(gameHours, currentDay);
            VerminSystem?.Tick(gameHours, Inventory);
            JuryRigSystem?.Tick(gameHours, currentDay);
            FreezePipeSystem?.Tick(gameHours);
            TickCompost(gameHours);
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

        private void TickFactionAndWorldSideSystems(float gameHours, int currentDay)
        {
            TrackerSystem?.Tick(gameHours,
                setFactionRaidChance: (factionId, chance) =>
                    HatchDefenseSystem?.SetRaidChanceOverride(factionId, chance),
                scheduleEvent: (eventId, fireDay, originFlag) =>
                    EventRunner?.ScheduleEvent(eventId, fireDay, originFlag),
                currentDay: currentDay);

            DeadDropSystem?.Tick(gameHours);
            HostageSystem?.Tick(gameHours);
            PropagandaSystem?.Tick(gameHours,
                modifyFactionTrust: (fid, delta) => EconomySystem?.ModifyTrust(fid, delta),
                reduceRaidChance: (fid, reduction) =>
                    HatchDefenseSystem?.AdjustRaidChance(fid, -reduction));

            // Prompt #75 — spy sabotage countdown (daily).
            if (DeserterSystem != null && TimeSystem != null && _lastDeserterDay != currentDay)
            {
                _lastDeserterDay = currentDay;
                DeserterSystem.TickDaily(Shelter,
                    scheduleEvent: eventId =>
                        EventRunner?.ScheduleEvent(eventId, currentDay + 1, null));
            }

            ScapegoatSystem?.Tick(gameHours, WeatherSystem.Current,
                scheduleEvent: (eventId, fireDay, flag) =>
                    EventRunner?.ScheduleEvent(eventId, fireDay, flag),
                currentDay: currentDay);
        }

        private void TickHouseEcosystemAndTactical(float gameHours, int currentDay)
        {
            if (EcosystemSystem != null && TimeSystem != null && _lastEcosystemDay != currentDay)
            {
                _lastEcosystemDay = currentDay;
                float outdoorRad = RadiationSystem != null ? 15f : 0f;
                bool exchangeTriggered = WorldPhaseSystem != null
                    && WorldPhaseSystem.HasTriggeredExchange;
                EcosystemSystem.TickDaily(outdoorRad, exchangeTriggered);
            }

            TickHouseToBunkerDaily(currentDay);

            bool preDay30 = WorldPhaseSystem != null && !WorldPhaseSystem.HasTriggeredExchange;
            FloodingSystem?.Tick(gameHours, WeatherSystem.Current == WeatherKind.Rain, preDay30, Shelter,
                roomId => roomId == "cellar" || roomId == "coal_room");
            PerimeterTrapSystem?.Tick(gameHours);
            // #268 Restless night pacing: host night gate via TimeSystem hour.
            bool isNight = TimeSystem != null
                && (TimeSystem.CurrentHour < 6 || TimeSystem.CurrentHour >= 22);
            if (NoiseSystem != null)
            {
                if (isNight)
                    NoiseSystem.TickPersonalQuestNoise(gameHours, isNight: true);
                else
                    NoiseSystem.Tick(gameHours);
            }
            if (HatchVisibilitySystem != null && TimeSystem != null && _lastHatchVisDay != currentDay)
            {
                _lastHatchVisDay = currentDay;
                HatchVisibilitySystem.TickDaily();
            }

            if (TimeSystem != null && _systemWiring != null)
                _systemWiring.WireDaily(BuildDailyWiringContext(currentDay));
        }

        private void TickHouseToBunkerDaily(int currentDay)
        {
            if (HouseToBunkerSystem == null || TimeSystem == null) return;

            if (_lastHouseDay != currentDay && currentDay < WorldPhaseSystem.FlashpointDay)
            {
                _lastHouseDay = currentDay;
                if (WorldPhaseSystem != null
                    && WorldPhaseSystem.CurrentPhase == AtomicWar._Game.Survivors.WorldPhase.CivilWar)
                {
                    HouseToBunkerSystem.ApplyArtilleryDamage();
                }
            }

            if (currentDay >= WorldPhaseSystem.FlashpointDay
                && !HouseToBunkerSystem.HouseDestroyed
                && WorldPhaseSystem != null
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
