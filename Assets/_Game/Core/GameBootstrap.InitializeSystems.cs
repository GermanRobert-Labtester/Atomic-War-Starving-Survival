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
            // Seed the global RNG so every SeededRandom.Create() fallback uses
            // the campaign world seed instead of the same sequence every run.
            AtomicWar._Game.Utilities.SeededRandom.WorldSeed = _worldSeed;

            InitFoundation();
            InitUtilityAI();
            InitMedicalSystems();
            InitExpansion4Systems();
            InitEventsAndSurvivors();
            InitSaveAndExpeditions();
            InitRadioAndEndgame();

            // Phase 2 — Wire psychological systems into live gameplay
            // (EventRunner, HatchDefenseSystem, ExpeditionSystem must exist)
            InitPhase2Wiring();

            // Phase 3 — Wire flashback, moral branching, chemical dependency
            InitPhase3Wiring();

            // Phases 4-6 — Trade specialties, final wishes, diegetic artifacts,
            // ham radio, damaged maps, audio cassettes
            InitPhases4to6Wiring();

            // Phases 7-8 — Interpersonal dynamics: trauma bonds, friction,
            // ration conflicts, leadership, caregiving, desertion
            InitPhases7to8Wiring();

            // Phases 9-10 — Faction arcs (Garrison, Cult, Refugees) +
            // Endgame branching refinement
            InitPhases9to10Wiring();

            // Antigravity Expansion — Mechanics #41-80
            // (Phases 16-19: Physical Quirks, Environmental, Legacy Arcs)
            InitAntigravitySystems();

            // Expansions 3 & 4 — Procedural Loot, Dynamic Questlines,
            // Siege Tactics, Faction Intel, Vehicle Systems
            InitExpansions3to4();

            // Deep Lore — Narrative arcs, world history, faction lore
            InitDeepLore();

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
