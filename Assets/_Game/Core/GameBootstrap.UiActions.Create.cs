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


        private void InjectQuestNodesIntoMap()
        {
            if (GeneratedMap == null) return;
            var defs = Data.LocationQuestNodeFactory.AllDefinitions();
            for (int i = 0; i < defs.Count; i++)
            {
                var existingNode = GeneratedMap.GetNode(defs[i].NodeId);
                if (existingNode != null)
                {
                    // Override existing random node with quest data.
                    existingNode.DisplayName = defs[i].DisplayName;
                    existingNode.DangerLevel = defs[i].DangerLevel;
                    existingNode.TrueRad = defs[i].TrueRad;
                    existingNode.HasUxo = defs[i].HasUxo;
                    existingNode.LootTableId = defs[i].LootTableId;
                }
                else
                {
                    // Inject new quest node into the map.
                    var node = Data.LocationQuestNodeFactory.ToMapNode(defs[i]);
                    GeneratedMap.Nodes.Add(node);
                }
            }
        }



        private void ApplyLayoutTrait(AtomicWar._Game.Shelter.ShelterLayoutTrait trait)
        {
            switch (trait)
            {
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.RootCellar:
                    // High starting food, high mold risk.
                    if (_storesRoom != null) _storesRoom.HasMold = true;
                    if (Inventory != null && _itemCatalog != null)
                    {
                        var food = _itemCatalog.GetById("canned_food");
                        if (food != null) Inventory.Add(food, 8);
                    }
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.FallenBeam:
                    // Stairs blocked — hatch requires Saw to access.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.DirtFloor:
                    // Integrity degrades faster (handled by multiplier in StructuralIntegritySystem).
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.ExposedHatch:
                    // No rubble shielding; hatch is vulnerable.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.Flooded:
                    // Rooms start with water — must pump.
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.PrepperCache:
                    // Extra starting scrap.
                    if (Inventory != null && _itemCatalog != null)
                    {
                        var scrap = _itemCatalog.GetById("electronic_scrap");
                        if (scrap != null) Inventory.Add(scrap, 3);
                    }
                    break;
                case AtomicWar._Game.Shelter.ShelterLayoutTrait.SharedPipe:
                    // Water anomaly handled by HouseToBunkerSystem.
                    break;
            }
        }



        private void SeedStartingInventory()
        {
            if (_itemCatalog == null) return;
            foreach (var item in _itemCatalog.items)
            {
                if (item == null) continue;
                // Give a reasonable starting stock
                int amount = item.type switch
                {
                    ItemType.Food => 10,
                    ItemType.Water => 10,
                    ItemType.Iodine => 5,
                    ItemType.AntiRad => 3,
                    ItemType.Fuel => 8,
                    ItemType.Filter => 3,
                    ItemType.Material => 15,
                    _ => 1
                };
                Inventory.Add(item, amount);
            }
        }



        private void ForceMentalBreakSabotage(System.Random rng)
        {
            if (Shelter == null || Shelter.Modules == null || Shelter.Modules.Count == 0) return;
            if (rng == null) rng = new System.Random();
            int idx = rng.Next(Shelter.Modules.Count);
            var mod = Shelter.Modules[idx];
            if (mod == null) return;
            if (mod.IsEnabled)
                mod.IsEnabled = false;
            else
                mod.FilterHealth = Mathf.Max(0f, mod.FilterHealth - 25f);
        }



        private void CreateSurvivor(string id, string name)
        {
            var sv = new Survivor { Id = id, DisplayName = name };
            // Varied aptitude bases + one predetermined expert track each
            // (Prompt #179). Bases are not static "levels" — they seed how
            // hard the survivor starts; action-driven perks grow from work.
            ApplyCharacterSkillBases(sv, id);
            // Default room assignment so the MentalBreakSystem has room
            // boundaries from day 1 (Prompt #29 follow-up). Elena stays
            // near the bed in quarters; Marcus watches the stores; Suki
            // is in the entry hallway (closest to the hatch).
            if (id == "sv_elena") sv.CurrentRoomId = "quarters";
            else if (id == "sv_marcus") sv.CurrentRoomId = "stores";
            else if (id == "sv_suki") sv.CurrentRoomId = "entry";
            Survivors.Add(sv);
            NeedsSystem.Register(sv);
            RadiationSystem.Register(sv);
        }

        /// <summary>
        /// Character skill bases (varied) + single expert discipline.
        /// Expert track is the only expert perk they can ever earn.
        /// </summary>
        private static void ApplyCharacterSkillBases(Survivor sv, string id)
        {
            if (sv == null) return;
            switch (id)
            {
                case "sv_elena":
                    // Medic track — steady hands under pressure.
                    sv.MedicalSkill = 0.70f;
                    sv.CraftingSkill = 0.25f;
                    sv.ScienceSkill = 0.40f;
                    sv.ExpertDisciplineId = "medical";
                    break;
                case "sv_marcus":
                    // Hands-on fixer — workshop calluses.
                    sv.MedicalSkill = 0.30f;
                    sv.CraftingSkill = 0.65f;
                    sv.ScienceSkill = 0.25f;
                    sv.ExpertDisciplineId = "crafting";
                    break;
                case "sv_suki":
                    // Signal / analysis mind — cold reading of the world.
                    sv.MedicalSkill = 0.25f;
                    sv.CraftingSkill = 0.35f;
                    sv.ScienceSkill = 0.70f;
                    sv.ExpertDisciplineId = "science";
                    break;
                default:
                    sv.MedicalSkill = 0.30f;
                    sv.CraftingSkill = 0.30f;
                    sv.ScienceSkill = 0.30f;
                    sv.ExpertDisciplineId = null;
                    break;
            }
        }

        /// <summary>
        /// Map runtime-created Utility AI actions to progression disciplines
        /// (Prompt #179). Empty discipline = no XP for that action.
        /// </summary>
        private void AssignActionProgressionDisciplines()
        {
            if (Actions == null) return;
            for (int i = 0; i < Actions.Count; i++)
            {
                var a = Actions[i];
                if (a == null) continue;
                if (!string.IsNullOrEmpty(a.progressionDiscipline)) continue;

                string typeName = a.GetType().Name;
                if (typeName.Contains("Treat") || typeName.Contains("Caregive")
                    || typeName.Contains("Chelation") || typeName.Contains("Boil")
                    || typeName.Contains("Mercy") || typeName.Contains("Iodine")
                    || typeName.Contains("AntiRad") || typeName.Contains("Decon"))
                {
                    a.progressionDiscipline = "medical";
                    a.progressionXp = 6f;
                }
                else if (typeName.Contains("Craft") || typeName.Contains("Repair")
                    || typeName.Contains("Excavate") || typeName.Contains("Tunnel")
                    || typeName.Contains("Shielding") || typeName.Contains("WindTurbine")
                    || typeName.Contains("ClearRubble") || typeName.Contains("Compost"))
                {
                    a.progressionDiscipline = "crafting";
                    a.progressionXp = 5f;
                }
                else if (typeName.Contains("Chart") || typeName.Contains("Survey")
                    || typeName.Contains("Radio") || typeName.Contains("Teach"))
                {
                    a.progressionDiscipline = "science";
                    a.progressionXp = 5f;
                }
                else if (typeName.Contains("Guard") || typeName.Contains("Hunt")
                    || typeName.Contains("SuppressingFire"))
                {
                    a.progressionDiscipline = "combat";
                    a.progressionXp = 5f;
                }
                else if (typeName.Contains("Scavenge") || typeName.Contains("Haul"))
                {
                    a.progressionDiscipline = "scavenging";
                    a.progressionXp = 5f;
                }
                else if (typeName.Contains("Eat") || typeName.Contains("Drink")
                    || typeName.Contains("Sleep") || typeName.Contains("Rest")
                    || typeName.Contains("Warm") || typeName.Contains("Purify")
                    || typeName.Contains("Pedal") || typeName.Contains("Refuel"))
                {
                    a.progressionDiscipline = "survival";
                    a.progressionXp = 2f;
                }
            }
        }



        private void WarmDayTickCaches()
        {
            _mentalBreakRng ??= CreateSaltedRng(_worldSeed, "mental_break");
            _phantomRng ??= CreateSaltedRng(_worldSeed, "phantom");
            _eventCtxRng ??= CreateSaltedRng(_worldSeed, "event_ctx");
            _aiRng ??= CreateSaltedRng(_worldSeed, "ai");
            _getSurvivorsCached ??= () => Survivors;
            _getFactionTrustEffective ??= factionId =>
                EconomySystem != null ? EconomySystem.GetEffectiveTrust(factionId) : 0f;
            _getFactionTrustStored ??= factionId =>
                EconomySystem != null ? EconomySystem.GetTrust(factionId) : 0f;
            _scheduleEventCached ??= (eventId, fireDay, originFlag) =>
                EventRunner?.ScheduleEvent(eventId, fireDay, originFlag);
            _onEventFlagChangedCached ??= (flagId, value) =>
            {
                if (SaveSystem != null)
                    SaveSystem.SetWorldFlag(flagId, value);
            };
            _tryApplyPedalCostCached ??= TryApplyPedalCost;
        }



        private static string WeatherNameOf(WeatherKind kind)
        {
            return kind switch
            {
                WeatherKind.Clear => "Clear",
                WeatherKind.Rain => "Rain",
                WeatherKind.Overcast => "Overcast",
                WeatherKind.Ashfall => "Ashfall",
                WeatherKind.FalloutStorm => "FalloutStorm",
                WeatherKind.Blizzard => "Blizzard",
                WeatherKind.BlackRain => "BlackRain",
                _ => "Clear"
            };
        }

    }
}
