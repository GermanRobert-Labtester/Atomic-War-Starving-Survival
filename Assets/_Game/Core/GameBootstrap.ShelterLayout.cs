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
        private void ApplyShelterLayout(Shelter.ShelterMapSO layout)
        {
            if (layout == null || Shelter == null) return;

            // Register layout rooms.
            if (layout.roomIds != null)
            {
                for (int i = 0; i < layout.roomIds.Length; i++)
                {
                    string name = layout.roomNames != null && i < layout.roomNames.Length
                        ? layout.roomNames[i] : layout.roomIds[i];
                    float size = layout.roomSizes != null && i < layout.roomSizes.Length
                        ? layout.roomSizes[i] : 1f;
                    var room = new Shelter.ShelterRoom(layout.roomIds[i], null);
                    Shelter.RegisterRoom(room);
                    if (AtmosphereSystem != null)
                        AtmosphereSystem.RegisterRoom(room);
                }
            }

            // Install layout-specific starting modules.
            if (layout.startingModules != null)
            {
                for (int i = 0; i < layout.startingModules.Length; i++)
                {
                    var m = layout.startingModules[i];
                    var instance = new Shelter.ShelterModuleInstance(m.moduleId, m.level)
                    {
                        IsEnabled = m.isEnabled,
                        Fuel = m.fuel,
                        FilterHealth = m.filterHealth,
                        RoomId = m.roomId
                    };
                    Shelter.AddModule(instance);
                }
            }

            // Apply traits.
            if (layout.traits != null)
            {
                for (int i = 0; i < layout.traits.Length; i++)
                {
                    ApplyLayoutTrait(layout.traits[i]);
                }
            }

            // Starting water from water heater.
            if (layout.hasWaterHeater && layout.startingCleanWater > 0f && WaterStorage != null)
            {
                WaterStorage.AddClean(layout.startingCleanWater);
            }

            // Initial hatch damage.
            if (layout.startingHatchDamage > 0f && HatchDefenseSystem != null)
            {
                // Apply hatch damage via security reduction.
            }
        }

    }
}
