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
        private void InitializeSystems()
        {
            // Seed the global RNG so every CreateFixed() fallback produces
            // campaign-specific streams instead of the same sequence every run.
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = _worldSeed;

            InitFoundation();
            InitUtilityAI();
            InitMedicalSystems();
            InitExpansion4Systems();
            InitEventsAndSurvivors();
            InitSaveAndExpeditions();
            InitRadioAndEndgame();
            FinishSystemRegistration();
        }

















        private void FinishSystemRegistration()
        {
            // H-5: Batch-register all systems with the registry so diagnostics
            // can detect dead (constructed-but-unticked) systems.
            RegisterSystemsInRegistry();

            // Persist the whole social family via one ISaveable slot (#469-#478).
            if (SaveSystem != null && BunkerSocial != null)
                SaveSystem.Register(BunkerSocial);

            // AUDIT-003: hard-fail if foundation systems required by TickSystems
            // were never constructed (partial init / test-host misuse).
            AssertFoundationSystems();
        }


    }
}
