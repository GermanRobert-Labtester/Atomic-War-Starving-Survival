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
using Ashfall.Core;

namespace AtomicWar._Game.Core
{
    public partial class GameBootstrap
    {

        private void TickAiWave(float gameHours)
        {
            // AI (evaluate per survivor, every EvaluationInterval)
            if (UtilityAI == null || TimeSystem == null || Survivors == null) return;

            UtilityAI.Tick(gameHours * TimeSystem.SecondsPerGameHour);
            if (!UtilityAI.ShouldEvaluate()) return;

            SleepQualitySystem.ResetBedOccupancy(Shelter);
            HatchDefenseSystem?.ClearGuards();

            int day = TimeSystem.CurrentDay;
            var context = FillAiContextScratch(day);
            for (int si = 0; si < Survivors.Count; si++)
            {
                var sv = Survivors[si];
                if (sv == null || !sv.IsAlive) continue;

                float mapUncertainty = GetMapUncertaintyFor(sv);
                BeliefSystem?.Tick(sv, mapUncertainty, gameHours);

                context.Survivor = sv;
                context.IsListless = sv.IsListless;
                context.MapUncertainty = mapUncertainty;
                context.IsAnxious = sv.HasRadiationAnxietyStatus;
                context.IsNumb = sv.IsNumb;

                var action = UtilityAI.SelectAction(context, Actions);
                int antiRadBefore = -1;
                if (action != null && action.id == "action_use_antirad" && Inventory != null)
                    antiRadBefore = Inventory.CountById("anti_rad");

                action?.Execute(context);

                // Prompt #179–#181 — action-driven XP / dormant refresh / epiphany.
                if (action != null && SkillProgression != null
                    && !string.IsNullOrEmpty(action.progressionDiscipline))
                {
                    float xp = action.progressionXp > 0f
                        ? action.progressionXp
                        : SkillProgressionSystem.DefaultXpPerAction;
                    SkillProgression.RecordAction(
                        sv, action.progressionDiscipline, xp, day, _aiRng);
                }

                // Prompt #7 / #551 — AI anti-rad: addiction + blood toxicity + polypharmacy.
                // Only when a dose was taken (inventory dropped) or inventory is unbound.
                if (action != null && action.id == "action_use_antirad")
                {
                    bool doseTaken = Inventory == null
                        || (antiRadBefore >= 0
                            && Inventory.CountById("anti_rad") < antiRadBefore);
                    if (doseTaken)
                        ChemUse?.Notify(sv, "anti_rad");
                }

                // #279 Former Politician: dirty labor morale + daily quest credit.
                if (action != null && PersonalQuests != null)
                {
                    PersonalQuests.ApplyDirtyLaborMorale(sv, action.id);
                    bool didPhysicalLabor = PersonalQuests.IsDirtyLaborAction(action.id);
                    if (didPhysicalLabor)
                        PersonalQuests.MarkDirtyLaborPerformed(sv);

                    // #311 Billionaire Entitled: physical labor tanks morale.
                    PersonalQuests.ApplyEntitledLaborDay(sv, didPhysicalLabor);
                }
            }
        }

        private AIContext FillAiContextScratch(int day)
        {
            float indoorTemp = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;
            float raidThreat = ComputeAiRaidThreat(day);
            int scrapDeficit = WorkbenchSystem != null
                ? WorkbenchSystem.GetCriticalElectronicScrapDeficit()
                : 0;
            float junkUrgency = scrapDeficit > 0
                ? Mathf.Clamp01(scrapDeficit / 4f)
                : 0f;
            bool isStorm = WeatherSystem.Current == WeatherKind.FalloutStorm
                || WeatherSystem.Current == WeatherKind.BlackRain;

            var context = _aiContextScratch;
            context.Shelter = Shelter;
            context.Inventory = Inventory;
            context.BunkerRationsScheduled = BunkerRationingSystem != null;
            context.Random = _aiRng;
            context.IsFalloutStorm = isStorm;
            context.AmbientRadRate = 5f;
            context.GrowLightActive = Shelter != null && Shelter.IsGrowLightActive;
            context.OnRequestSurvey = RequestSurveyForSurvivor;
            context.BeliefSystem = BeliefSystem;
            context.MedicalSystem = MedicalSystem;
            context.MentalBreak = MentalBreakSystem;
            context.PowerNetwork = PowerNetwork;
            context.HatchDefense = HatchDefenseSystem;
            context.RaidThreatLevel = raidThreat;
            context.CurrentDay = day;
            context.IndoorTemperatureC = indoorTemp;
            context.SleepRoomId = SleepQualitySystem.DefaultSleepRoomId;
            context.AreRoomsAdjacent = Shelter != null ? Shelter.AreRoomsAdjacent : null;
            context.WaterStorage = WaterStorage;
            context.NeedsElectronicScrapForCriticalRepair = scrapDeficit > 0;
            context.JunkScavengeUrgency = junkUrgency;
            context.CorpseCount = CorpseSystem != null ? CorpseSystem.CorpseCount : 0;
            context.HallucinationSystem = HallucinationSystem;
            context.RadiationSystem = RadiationSystem;
            context.NeedsSystem = NeedsSystem;
            context.VerminSystem = VerminSystem;
            context.WasteSystem = WasteSystem;
            context.JuryRigSystem = JuryRigSystem;
            context.RepairWorkOrderSystem = RepairWorkOrderSystem;
            context.StructuralIntegrity = StructuralIntegrity;
            context.GetSurvivors = _getSurvivorsCached;
            BindAiShelterSystems(context);
            return context;
        }

        private void BindAiShelterSystems(AIContext context)
        {
            // Audit C-3: bindings for AI actions that drive C-1-wired systems.
            context.ExcavationSystem = ExcavationSystem;
            context.FloodingSystem = FloodingSystem;
            context.HiddenStorageSystem = HiddenStorageSystem;
            context.CeilingCollapseSystem = CeilingCollapseSystem;
            context.PerimeterTrapSystem = PerimeterTrapSystem;
            context.TunnelingSystem = TunnelingSystem;
            context.MaterialShieldingSystem = MaterialShieldingSystem;
            context.EscapeHatchSystem = EscapeHatchSystem;
            context.AirlockSystem = AirlockSystem;
            context.CompostSystem = CompostSystem;
            context.SterilizationSystem = SterilizationSystem;
            context.ChelationSystem = ChelationSystem;
            context.WindTurbineSystem = WindTurbineSystem;
            context.HaulingSystem = HaulingSystem;
            context.ScrapWeaponSystem = ScrapWeaponSystem;
            context.WeaponMaintenanceSystem = WeaponMaintenanceSystem;
            context.CombatPerks = CombatPerks;
            context.SurvivalPerks = SurvivalPerks;
            context.ShelterPerks = ShelterPerks;
            context.MedicalPerks = MedicalPerks;
            context.ExpeditionPerks = ExpeditionPerks;
            context.SocialPerks = SocialPerks;
            context.PersonalQuests = PersonalQuests;
            context.PetSystem = PetSystem;
            context.TriageSystem = TriageSystem;
            context.ResilienceSystem = ResilienceSystem;
            context.AntibioticResistSystem = AntibioticResistSystem;
            context.CartographySystem = CartographySystem;
            context.InternalLockSystem = InternalLockSystem;
            context.MentorshipSystem = MentorshipSystem;
            context.Affinity = MentalBreakSystem != null ? MentalBreakSystem.Affinity : null;
            context.GetUnchartedNodeId = ResolveUnchartedNodeId;
            context.OnRequestCookMeal = RequestCookMealForSurvivor;
            context.OnRequestButcherCorpse = RequestButcherCorpseForSurvivor;
            context.CanSpreadGossip = CanSurvivorSpreadGossip;
            context.OnRequestSpreadGossip = RequestSpreadGossipForSurvivor;
        }

        /// <summary>
        /// First map node that is not yet charted (for ChartMap AI action).
        /// Null when cartography has nothing left to process.
        /// </summary>
        private string ResolveUnchartedNodeId()
        {
            if (CartographySystem == null || GeneratedMap == null || GeneratedMap.Nodes == null)
                return null;
            for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
            {
                var node = GeneratedMap.Nodes[i];
                if (node == null || string.IsNullOrEmpty(node.NodeId) || node.IsShelter) continue;
                if (!CartographySystem.IsCharted(node.NodeId))
                    return node.NodeId;
            }
            return null;
        }

        /// <summary>Cook one meal at the stove for this survivor (for Cook AI action).</summary>
        private bool RequestCookMealForSurvivor(Survivor survivor)
        {
            return CookingSystem != null && CookingSystem.CookMeal(survivor);
        }

        /// <summary>Break down one corpse into bones/meat for this survivor (for Butcher Corpse AI action).</summary>
        private bool RequestButcherCorpseForSurvivor(Survivor survivor)
        {
            return CorpseSystem != null
                && CorpseSystem.ProcessForParts(survivor, out _, out _, out _);
        }

        /// <summary>True if this survivor witnessed a crime they can still gossip about (for Spread Gossip AI action).</summary>
        private bool CanSurvivorSpreadGossip(Survivor survivor)
        {
            return Gossip != null && survivor != null && Gossip.CanSpreadRumor(survivor.Id);
        }

        /// <summary>Have this survivor tell one more person their witnessed rumor (for Spread Gossip AI action).</summary>
        private bool RequestSpreadGossipForSurvivor(Survivor survivor)
        {
            if (Gossip == null || survivor == null || !Gossip.CanSpreadRumor(survivor.Id)) return false;
            Gossip.SpreadRumor(survivor.Id);
            return true;
        }

        private void TickEventsAndJournal(float gameHours)
        {
            if (EventRunner == null) return;

            float indoorTemp = TemperatureSystem != null && Shelter != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;

            // Events (chance per hour)
            var eventContext = BuildEventContext(
                TimeSystem != null ? TimeSystem.CurrentDay : 1,
                TimeSystem != null ? TimeSystem.CurrentHourFloat : 12f,
                indoorTemp);
            EventRunner.Tick(gameHours, eventContext);

            // Internal mysteries: resource-starved Missing Rations pressure.
            SuspicionTracker?.Tick(gameHours, eventContext, EventRunner);

            // Diegetic journal discoveries (first-time atmosphere / rad / storm / etc.)
            if (JournalSystem != null)
                EventRunner.ObserveDiscoveries(JournalSystem, eventContext);

            // Try to trigger an event occasionally (~5% chance per hour).
            // Use the cached AI RNG for deterministic save/load replay (audit bugfix #3).
            if (_aiRng != null && _aiRng.NextDouble() < 0.05f)
            {
                var selectedEvent = EventRunner.SelectEvent(eventContext);
                if (selectedEvent != null)
                {
                    EventRunner.Run(selectedEvent, eventContext);
                }
            }
        }

    }
}
