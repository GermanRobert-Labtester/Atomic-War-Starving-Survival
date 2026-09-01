namespace Ashfall.Core.UI
{
    /// <summary>
    /// Registers every player-navigable panel into the engine-agnostic
    /// <see cref="PanelRegistry"/> at host startup. Called once from Main._Ready().
    /// Adding a new panel requires adding a descriptor here AND a matching case
    /// in Main.GameFlow.cs OpenPlayerPanel (or OpenExpandedPanel for Expanded group).
    /// The PanelRouteGateTests CI gate fails when these two sets diverge.
    /// </summary>
    public static class PanelRegistryBootstrap
    {
        public static void RegisterAll()
        {
            // ── Dashboard panels ────────────────────────────────────────────
            R("status",              "Survival Status",               PanelGroup.Dashboard,  new[] { "survivors", "world", "inventory" });
            R("help",                "Tutorial / Help",               PanelGroup.Dashboard);
            R("afflictions",         "Afflictions",                   PanelGroup.Dashboard,  new[] { "survivors", "inventory", "medical", "phase0" });
            R("radiation_detail",    "Radiation Detail",              PanelGroup.Dashboard,  new[] { "survivors", "phase0" });
            R("research",            "Research",                      PanelGroup.Dashboard,  new[] { "research" });
            R("weather_detail",      "Weather Detail",                PanelGroup.Dashboard,  new[] { "world" });
            R("weather_forecast",    "Weather Forecast",              PanelGroup.Dashboard,  new[] { "world" });
            R("event_detail",        "Event Detail",                  PanelGroup.Dashboard,  new[] { "events" });
            R("events_log",          "Events Log",                    PanelGroup.Dashboard,  new[] { "events" });
            R("economy_detail",      "Economy Detail",                PanelGroup.Dashboard,  new[] { "economy" });
            R("radiation_history",   "Radiation History",             PanelGroup.Dashboard,  new[] { "phase0" });
            R("journal_detail",      "Journal Detail",                PanelGroup.Dashboard,  new[] { "journal" });
            R("survival_detail",     "Survival Detail",               PanelGroup.Dashboard,  new[] { "survivors" });
            R("survivor_detail",     "Survivor Detail",               PanelGroup.Dashboard,  new[] { "survivors" });
            R("inventory_detail",    "Inventory Detail",              PanelGroup.Dashboard,  new[] { "inventory" });
            R("achievements",        "Achievements",                  PanelGroup.Dashboard,  new[] { "survivors" });
            R("survivors",           "Survivors Panel",               PanelGroup.Dashboard,  new[] { "survivors" });
            R("inventory",           "Inventory Panel",               PanelGroup.Dashboard,  new[] { "inventory" });
            R("crafting",            "Crafting Panel",                PanelGroup.Dashboard,  new[] { "crafting", "inventory" });
            R("medical",             "Medical Panel",                 PanelGroup.Dashboard,  new[] { "survivors", "inventory", "medical", "phase0" });
            R("phase0",              "Phase 0 Panel",                 PanelGroup.Dashboard,  new[] { "phase0" });
            R("expeditions",         "Expeditions Panel",             PanelGroup.Dashboard,  new[] { "expeditions", "expansions", "survivors", "inventory" });
            R("weather",             "Weather Panel",                 PanelGroup.Dashboard,  new[] { "world" });
            R("radio",               "Radio Panel",                   PanelGroup.Dashboard,  new[] { "radio" });
            R("map",                 "Map Panel",                     PanelGroup.Dashboard,  new[] { "core", "expeditions", "expansions", "world", "journal", "deep_coast", "year_of_ash" });
            R("map_detail",          "Map Location Detail",           PanelGroup.Secondary,  new[] { "world" });
            R("shelter",             "Shelter Panel",                 PanelGroup.Dashboard,  new[] { "survivors", "world", "inventory" });
            R("factions",            "Factions Panel",                PanelGroup.Dashboard,  new[] { "core", "muster", "expansions" });
            R("faction_detail",      "Faction Detail",                PanelGroup.Secondary,  new[] { "factions" });
            R("quests",              "Quests Panel",                  PanelGroup.Dashboard,  new[] { "core", "expansions", "duty_roster" });
            R("quest_detail",        "Quest Detail",                  PanelGroup.Secondary,  new[] { "quests" });
            R("journal",             "Journal Panel",                 PanelGroup.Dashboard,  new[] { "journal" });
            R("protocol",            "Opening Protocol",              PanelGroup.Dashboard,  new[] { "starting_level" });
            R("greenhouse",          "Greenhouse Panel",              PanelGroup.Dashboard,  new[] { "greenhouse" });
            R("silent_foundry",      "Silent Foundry Panel",          PanelGroup.Dashboard,  new[] { "expansions", "silent_foundry" });
            R("trade",               "Trade / Economy Panel",         PanelGroup.Dashboard,  new[] { "economy", "silent_foundry" });
            R("muster",              "The Muster Panel",              PanelGroup.Dashboard,  new[] { "muster" });
            R("expansions",          "Expansions Hub",                PanelGroup.Dashboard,  new[] { "expansions", "greenhouse", "duty_roster", "muster", "maritime", "deep_coast", "world", "medical", "verdict" });
            R("standing_record",     "Standing Record Panel",         PanelGroup.Dashboard,  new[] { "expansions" });
            R("crossing_quests",     "Crossing Quest Panel",          PanelGroup.Dashboard,  new[] { "expansions" });
            R("maritime",            "Maritime / Black Flotilla",     PanelGroup.Dashboard,  new[] { "maritime", "survivors" });
            R("deep_coast",          "Deep Coast Panel",              PanelGroup.Dashboard,  new[] { "deep_coast", "core" });
            R("century_seed",        "Century Seed Panel",            PanelGroup.Dashboard,  new[] { "expansions", "survivors" });
            R("epilogue",            "Epilogue Panel",                PanelGroup.Dashboard,  new[] { "expansions", "survivors" });
            R("verdict",             "Verdict Panel",                 PanelGroup.Dashboard,  new[] { "verdict" });
            R("holdfast",            "Holdfast Terminal",             PanelGroup.Dashboard,  new[] { "core" });
            R("duty_roster",         "Duty Roster Panel",             PanelGroup.Dashboard,  new[] { "duty_roster", "survivors" });
            R("duty_roster_detail",  "Duty Roster Detail",            PanelGroup.Secondary,  new[] { "duty_roster" });
            R("save",                "Save / Load Panel",             PanelGroup.Dashboard);
            R("combat",              "Combat Panel",                  PanelGroup.Dashboard);
            R("combat_detail",       "Combat Detail",                 PanelGroup.Secondary);
            R("combat_history",      "Combat History",                PanelGroup.Secondary);
            R("workshop",            "Relic Workshop",                PanelGroup.Dashboard,  new[] { "crafting", "inventory", "survivors" });
            R("pharma_lab",          "Pharma Lab",                    PanelGroup.Dashboard,  new[] { "crafting", "inventory", "survivors" });
            R("pharma",              "Pharma Lab (alias)",            PanelGroup.Dashboard,  new[] { "crafting", "inventory", "survivors" });

            // ── Main Menu panels ─────────────────────────────────────────────
            // "codex" is requested from the main menu and resolved to the
            // JournalBookUI (same panel opened by "journal" in-game).
            R("codex",               "Codex (Journal from menu)",     PanelGroup.MainMenu,   new[] { "journal" }, availableInMenu: true);
            R("settings",            "Settings Panel",                PanelGroup.MainMenu,   null,                availableInMenu: true);

            // ── Expanded shelter sub-system panels ───────────────────────────
            R("water_treatment",     "Water Treatment",               PanelGroup.Expanded);
            R("airlock_security",    "Airlock Security",              PanelGroup.Expanded);
            R("survivor_relations",  "Survivor Relations",            PanelGroup.Expanded);
            R("regional_treaty",     "Regional Treaty",               PanelGroup.Expanded);
            R("vinyl_morale",        "Vinyl Morale",                  PanelGroup.Expanded);
            R("wildlife_trapping",   "Wildlife Trapping",             PanelGroup.Expanded);
            R("excavation",          "Excavation",                    PanelGroup.Expanded);
            R("apprenticeship",      "Apprenticeship",                PanelGroup.Expanded);
            R("caregiving",          "Caregiving",                    PanelGroup.Expanded);  // was missing from forwarding
            R("shelter_thermal",     "Shelter Thermal",               PanelGroup.Expanded);
            R("shelter_schedule",    "Shelter Schedule",              PanelGroup.Expanded);
            R("shelter_decor",       "Shelter Interior & Memorial Wall", PanelGroup.Expanded);
            R("autopsy_report",      "Autopsy Report",                PanelGroup.Expanded);
            R("waystation_network",  "Waystation Network",            PanelGroup.Expanded);
            R("chemical_dependency", "Chemical Dependency",           PanelGroup.Expanded);
            R("sump_flooding",       "Sump Flooding",                 PanelGroup.Expanded);
            R("decontamination",     "Decontamination",               PanelGroup.Expanded);
            R("kitchen_nutrition",   "Kitchen Nutrition",             PanelGroup.Expanded);
            R("equipment_condition", "Equipment Condition",           PanelGroup.Expanded);
            R("library_study",       "Library Study",                 PanelGroup.Expanded);
            R("archive_desk",        "Archive Desk",                  PanelGroup.Expanded);
            R("contractor_roster",   "Contractor Roster",             PanelGroup.Expanded);
            R("mental_health_crisis","Mental Health Crisis",          PanelGroup.Expanded);
            R("phantom_memory",      "Phantom Memory",                PanelGroup.Expanded);
            R("traveling_caravan",   "Traveling Caravan",             PanelGroup.Expanded);
            R("medical_ward",        "Medical Ward",                  PanelGroup.Expanded);

            // ── Standalone & Subsystem Consoles ──────────────────────────────
            R("brine_extraction",    "Brine Extraction",              PanelGroup.Expanded,   new[] { "silent_foundry" });
            R("expedition_camp",     "Expedition Camp",               PanelGroup.Secondary,  new[] { "expeditions" });
            R("fire_incident",       "Fire Incident",                 PanelGroup.Secondary,  new[] { "survivors" });
            R("geiger_calibration",  "Geiger Calibration",            PanelGroup.Secondary,  new[] { "phase0" });
            R("triangulation",       "Radio Triangulation",           PanelGroup.Secondary,  new[] { "radio" });
            R("weather_sonde",       "Weather Sonde",                 PanelGroup.Secondary,  new[] { "world" });
            R("power_grid",          "Power Grid",                    PanelGroup.Expanded,   new[] { "power_grid" });
            R("expedition_radar",    "Expedition Radar",              PanelGroup.Secondary,  new[] { "expeditions" });
            R("dose_ledger",         "Dose Ledger",                   PanelGroup.Expanded,   new[] { "phase0" });
            R("caravan_barter",      "Caravan Barter Ledger",         PanelGroup.Secondary,  new[] { "economy" });
            R("faction_matrix",      "Faction Stance Matrix",         PanelGroup.Secondary,  new[] { "factions" });
            R("factions_narrative",  "Factions Narrative",            PanelGroup.Secondary,  new[] { "factions" });
            R("skill_matrix",        "Skill Progression Matrix",      PanelGroup.Secondary,  new[] { "survivors" });
            R("survival_workstation","Survival Workstation",          PanelGroup.Dashboard,  new[] { "crafting", "inventory" });
            R("verdict_dashboard",   "Verdict Dashboard",             PanelGroup.Dashboard,  new[] { "verdict" });
            R("map_atlas",           "Subterranean Map Atlas",        PanelGroup.Dashboard,  new[] { "expeditions" });
            R("maritime_atlas",      "Maritime Expedition Atlas",     PanelGroup.Dashboard,  new[] { "maritime" });
            R("muster_atlas",        "The Muster Atlas",              PanelGroup.Dashboard,  new[] { "muster" });
            R("quests_atlas",        "Quests & Operations Atlas",     PanelGroup.Dashboard,  new[] { "quests" });
            R("research_atlas",      "Research Technology Atlas",     PanelGroup.Dashboard,  new[] { "research" });
            R("standing_record_atlas","Standing Record Atlas",        PanelGroup.Dashboard,  new[] { "expansions" });
            R("combat_hud",          "Tactical Combat HUD",           PanelGroup.Secondary,  new[] { "combat" });

            // ── Advanced Survival Consoles ───────────────────────────────────
            R("biogas_digester",     "Anaerobic Biogas Digester",     PanelGroup.Expanded);
            R("cartography_gis",     "3D Cavity GIS Cartography",     PanelGroup.Expanded);
            R("printing_press",      "Clandestine Printing Press",    PanelGroup.Expanded);
            R("silicon_slicing",     "Silicon Ingot Slicing",         PanelGroup.Expanded);
            R("geothermal_turbine",  "Geothermal Steam Turbine",      PanelGroup.Expanded);
            R("war_dog_kennel",      "War Dog Kennel & Bio-Monitor",  PanelGroup.Expanded);
            R("isotope_separator",   "Isotope Separator & Calutron",  PanelGroup.Expanded);
            R("plasma_smelting",     "Plasma Arc Smelting",           PanelGroup.Expanded);
            R("borehole_seismograph","Deep Borehole Seismograph",     PanelGroup.Expanded);
            R("logistics_airlock",   "Heavy Logistics Airlock",       PanelGroup.Expanded);

            // ── Subsystem Consoles (Batch 16 & 17) ───────────────────────────
            R("cryo_permafrost_core", "Cryogenic Permafrost Core",     PanelGroup.Expanded);
            R("basal_radon_migration","Basal Radon Migration",        PanelGroup.Expanded);
            R("trauma_bonding_cohort","Trauma Bonding & Cohort",       PanelGroup.Expanded);
            R("clandestine_insurgency","Clandestine Insurgency",      PanelGroup.Expanded);
            R("subterranean_debt_ledger","Subterranean Debt Ledger",   PanelGroup.Expanded);
            R("surface_shrapnel_aegis","Surface Shrapnel Aegis",       PanelGroup.Expanded);
            R("long_walk_expedition", "Long Walk Expedition",          PanelGroup.Expanded);
            R("sonic_rupture_drill",  "Sonic Rupture Drill",           PanelGroup.Expanded);
            R("vault_door_breaching", "Vault Door Breaching",          PanelGroup.Expanded);
            R("iron_cenotaph_memorial","Iron Cenotaph Memorial",       PanelGroup.Expanded);

            // ── Subsystem Consoles (Batch 18) ────────────────────────────────
            R("aquifer_treaty_concession", "Aquifer Treaty Concession",   PanelGroup.Expanded);
            R("crossing_safe_conduct_vouch","Crossing Safe Conduct Vouch",PanelGroup.Expanded);
            R("mechanical_prosthetics_lathe","Mechanical Prosthetics Lathe",PanelGroup.Expanded);
            R("fungal_protein_fermenter", "Fungal Protein Fermenter",     PanelGroup.Expanded);
            R("ultrasonic_decontam_airlock","Ultrasonic Decontam Airlock",PanelGroup.Expanded);

            // ── Subsystem Consoles (Batch 19) ────────────────────────────────
            R("tropospheric_radio_relay",  "Tropospheric Radio Relay",    PanelGroup.Expanded);
            R("induction_cupola_furnace",  "Induction Cupola Furnace",    PanelGroup.Expanded);
            R("heavy_marine_diesel_gen",   "Heavy Marine Turbodiesel Gen",PanelGroup.Expanded);
            R("slurry_dewatering_sump",    "Slurry Dewatering Sump",      PanelGroup.Expanded);
            R("magnetic_drum_archive",     "Magnetic Drum & Microfiche",  PanelGroup.Expanded);
        }

        private static void R(
            string id,
            string displayName,
            PanelGroup group,
            string[]? setupDeps = null,
            bool availableInMenu = false)
        {
            PanelRegistry.Register(new PanelDescriptor(id, displayName, group, setupDeps, availableInMenu));
        }
    }
}
