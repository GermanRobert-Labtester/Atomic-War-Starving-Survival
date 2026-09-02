// SPDX-License-Identifier: MIT
// ASHFALL Core: Content Utilization Scanner
//
// Phase 1–3 implementation: static inventory of all content files,
// loaders, registries, queries, systems, and their relationships.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ashfall.Core.Content
{
    /// <summary>
    /// Static scanner that builds a ContentUtilizationGraph from repository
    /// source analysis. Phase 1–3: discovery, graph schema, static inventory.
    /// </summary>
    public sealed class ContentUtilizationScanner
    {
        private readonly string _repoRoot;
        private readonly string _dataDir;
        private readonly string _coreDir;
        private readonly string _srcDir;
        private readonly ILog? _log;

        private readonly ContentUtilizationGraph _graph = new ContentUtilizationGraph();
        private readonly Dictionary<string, ContentNode> _nodesById = new Dictionary<string, ContentNode>(StringComparer.Ordinal);
        private readonly HashSet<string> _availableFiles = new HashSet<string>(StringComparer.Ordinal);

        // Known JSON authoritative catalogs (not narrative flavor text)
        private static readonly HashSet<string> AuthoritativeCatalogs = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "items.json", "recipes.json", "locations.json", "survivors.json",
            "faction_lore.json", "economy_goods.json", "events.json",
            "weather_seasons.json", "radio.json", "narrative_encounters.json",
            "questline_master.json", "world_history.json", "wasteland_map_v1.json",
            "dive_sites.json", "foundry_accords.json", "foundry_production.json",
            "foundry_treaty_consequences.json", "warlord_doctrines.json",
            "combat_catalog.json", "verdict_data.json", "verdict_items.json",
            "verdict_locations.json", "verdict_radio.json",
            "black_flotilla_items.json", "deep_lore_locations.json",
            "dose_items.json", "dose_locations.json", "dose_quests.json",
            "dose_registers.json", "holdfast_factions.json", "holdfast_flavor.json",
            "holdfast_items.json", "holdfast_locations.json", "holdfast_quests.json",
            "crossing_encounters.json", "crossing_factions.json",
            "crossing_items.json", "crossing_locations.json", "crossing_quests.json",
            "expeditions.json", "disease_catalog.json", "vehicles.json",
            "starting_supplies.json", "starting_survivors.json",
            "greenhouse_items.json", "library_manuals.json",
            "research_knowledge.json", "skills.json",
            "standing_record_factions.json", "standing_record_layouts.json",
            "standing_record_memory.json", "standing_record_quests.json",
            "year_of_ash_events.json", "year_of_ash_items.json",
            "year_of_ash_locations.json", "year_of_ash_questlines.json",
            "year_of_ash_quests.json", "year_of_ash_radio.json",
            "year_of_ash_survivors.json", "trade_screen_scenarios.json",
            "trade_specialties.json", "trade_tell_lines.json", "trade_texts.json",
            "feedback_messages.json", "final_wishes.json", "guilt_sources.json",
            "incidents.json", "moral_choice_chains.json", "moral_choice_flags.json",
            "moral_choice_quests.json", "moral_choice_quests_branching.json",
            "moral_choice_quests_expansion.json", "moral_choice_quest_stubs.json",
            "moral_choice_faction_reactions.json", "moral_choice_gossip.json",
            "hardcore_economy_tuning.json", "relic_recipes.json",
            "world_evolution_seeds.json", "shelter_schedules.json",
            "utility_actions.json", "confession_secrets.json",
            "dynamic_questlines.json", "door_encounters.json",
            "faction_war_communiques.json", "faction_war_dialogue.json",
            "faction_war_events.json", "faction_war_journal.json",
            "faction_war_location_overrides.json", "faction_war_radio.json",
            "independent_faction_branch.json", "military_faction_branch.json",
            "rebel_faction_branch.json", "faction_radio_corpus.json",
            "radio_distress_signals.json", "radio_distress_signals_expansion.json",
            "phantom_triggers.json", "phantom_heirlooms.json", "archive_inks.json",
            "autopsy_procedures.json", "chemical_dependency_items.json",
            "antigravity_survivor_fields.json", "expansion_survivor_fields.json",
            "deep_lore_survivor_fields.json", "expansion_item_tags.json",
            "audio_logs_expansion_05.json", "environmental_texts_expansion_05.json",
            "journal_entries_expansion_05.json", "memorials_expansion_05.json",
            "quests_expansion_05.json", "quests_expansion_06.json",
            "narrative_arc_events.json", "narrative_encounters_expansion.json",
            "narrative_progression.json", "narrative_questlines.json",
            "pharma_recipes.json", "power_grid.json",
            "characters.json", "item_description_texts.json",
            "journal_voice_prose.json", "medical_texts.json",
            "muster_epilogues.json", "muster_witnesses.json",
            "wall_carving_templates.json", "wasteland_grave_epitaphs.json",
            "damaged_map_zones.json", "currents.json",
            "cassette_sets.json", "epilogue_chronicle.json",
            "duty_roster_locations.json", "duty_roster_marks.json",
            "duty_roster_quests.json", "duty_roster_seasons.json",
            "locations_expansion3.json", "environmental_atmosphere_expansion.json",
            "thirdonary_quests.json", "foundry_faction.json",
            "foundry_items.json", "verdict_npcs.json", "verdict_questlines.json",
            "workshop_recipes.json", "radio_intercepts.json",
            "shelter_social_events.json", "excavation_hazard_mitigation.json",
            "chemical_weapons.json", "comms_targets.json",
            "ceremonies.json", "robotics.json",
            "item_degradation.json", "thermal_gear.json",
            "naval_vessels.json", "recreation.json",
            "fallout_patterns.json", "desperation_events.json",
            "bounty_board.json", "lore_archives.json",
            "surgical_procedures.json", "rail_network.json",
            "underground_flora.json", "wasteland_laws.json",
            "development_traits.json", "interrogation_tactics.json",
            "mutations.json", "camouflage_gear.json",
            "aircraft_parts.json", "labor_camps.json",
            "narcotics.json", "political_policies.json",
        };

        // Narrative JSON files in the narrative/ subdirectory — these are codex/lore, not gameplay catalogs
        public static bool IsNarrativeSubdirectoryFile(string relativePath)
        {
            return relativePath.Replace('\\', '/').StartsWith("narrative/", StringComparison.OrdinalIgnoreCase);
        }

        public static bool IsAuthoritativeCatalog(string fileName)
        {
            return AuthoritativeCatalogs.Contains(fileName);
        }

        public ContentUtilizationScanner(string repoRoot, string dataDir, string coreDir, string srcDir, ILog? log = null)
        {
            _repoRoot = repoRoot;
            _dataDir = dataDir;
            _coreDir = coreDir;
            _srcDir = srcDir;
            _log = log;
        }

        // ── Phase 1: Discovery ──────────────────────────────────────

        public ContentUtilizationGraph Scan()
        {
            _graph.ContentRoots.Add(_dataDir);
            _graph.SchemaVersion = "1.0.0";

            InventoryContentFiles();
            InventoryLoaders();
            InventoryRegistries();
            InventoryRuntimeSystems();
            InventoryQueries();
            InventoryUiSurfaces();
            InventoryCodexSurfaces();
            InventoryTests();
            CountDefinitions();
            ClassifyContent();
            VerifyConsumersInSource();
            BuildRelationships();
            DetectDisconnects();
            _graph.Stabilize();
            _graph.ComputeSummaries();
            return _graph;
        }

        private void EnsureNode(string id, ContentNodeKind kind, string label)
        {
            if (!_nodesById.ContainsKey(id))
            {
                var node = new ContentNode(id, kind, label);
                _nodesById[id] = node;
                _graph.Nodes.Add(node);
            }
        }

        private void AddEdge(string from, string to, ContentEdgeKind kind, EvidenceTier evidence, string context = "")
        {
            EnsureNode(from, ContentNodeKind.ContentFile, from);
            EnsureNode(to, ContentNodeKind.RuntimeSystem, to);
            _graph.Edges.Add(new ContentEdge(from, to, kind, evidence, context));
        }

        // ── Inventory Content Files ─────────────────────────────────

        private void InventoryContentFiles()
        {
            if (!Directory.Exists(_dataDir))
            {
                _log?.Warn($"[ContentUtilizationScanner] Data directory not found: {_dataDir}");
                return;
            }

            var jsonFiles = Directory.GetFiles(_dataDir, "*.json", SearchOption.AllDirectories);
            foreach (var file in jsonFiles)
            {
                string relativePath = Path.GetRelativePath(_dataDir, file).Replace('\\', '/');
                string fileName = Path.GetFileName(file);
                _availableFiles.Add(relativePath);

                string nodeId = $"file:{relativePath}";
                ContentNodeKind kind = IsNarrativeSubdirectoryFile(relativePath)
                    ? ContentNodeKind.ContentFile
                    : (IsAuthoritativeCatalog(fileName) ? ContentNodeKind.ContentFile : ContentNodeKind.ContentFile);

                EnsureNode(nodeId, kind, relativePath);

                _graph.Catalogs.Add(new CatalogEntry
                {
                    Path = relativePath,
                    Classification = IsNarrativeSubdirectoryFile(relativePath)
                        ? ContentClassification.CODEX_ONLY
                        : (IsAuthoritativeCatalog(fileName)
                            ? ContentClassification.UNRESOLVED
                            : ContentClassification.UNRESOLVED),
                    MaxStage = UtilizationStage.DISCOVERED,
                    BestEvidence = EvidenceTier.STATIC
                });
            }

            _log?.Info($"[ContentUtilizationScanner] Discovered {jsonFiles.Length} JSON files");
        }

        // ── Inventory Loaders ───────────────────────────────────────

        private void InventoryLoaders()
        {
            if (!Directory.Exists(_coreDir))
            {
                _log?.Warn($"[ContentUtilizationScanner] Core directory not found: {_coreDir}");
                return;
            }

            var csFiles = Directory.GetFiles(_coreDir, "*.cs", SearchOption.AllDirectories);
            // ... recursive
            var allCsFiles = new List<string>(csFiles);
            // Also include subdirectories
            foreach (var dir in Directory.GetDirectories(_coreDir, "*", SearchOption.AllDirectories))
            {
                allCsFiles.AddRange(Directory.GetFiles(dir, "*.cs"));
            }

            var loaderPatterns = new Dictionary<string, string[]>
            {
                ["items.json"] = new[] { "ItemCatalogLoader", "LoadItems" },
                ["recipes.json"] = new[] { "RecipeCatalogLoader" },
                ["locations.json"] = new[] { "LocationLayoutSystem", "WastelandMapCatalogLoader" },
                ["survivors.json"] = new[] { "SurvivorCatalogLoader", "SurvivorCatalog" },
                ["faction_lore.json"] = new[] { "FactionIconCatalog", "FactionIconLoader" },
                ["economy_goods.json"] = new[] { "GoodsCatalog" },
                ["events.json"] = new[] { "EventsHostSession" },
                ["weather_seasons.json"] = new[] { "WeatherSystem" },
                ["radio.json"] = new[] { "RadioHostSession", "RadioScriptbookCatalog" },
                ["narrative_encounters.json"] = new[] { "NarrativeEncounterCatalogLoader" },
                ["questline_master.json"] = new[] { "QuestlineMasterCatalog" },
                ["world_history.json"] = new[] { "EvolvingWorldCatalog" },
                ["wasteland_map_v1.json"] = new[] { "WastelandMapCatalogLoader" },
                ["dive_sites.json"] = new[] { "DiveSiteCatalog" },
                ["foundry_accords.json"] = new[] { "SilentFoundryCatalog", "SilentFoundryCatalogLoader" },
                ["foundry_production.json"] = new[] { "SilentFoundryCatalogLoader" },
                ["foundry_treaty_consequences.json"] = new[] { "SilentFoundryConsequencePolicy" },
                ["warlord_doctrines.json"] = new[] { "WarlordDoctrineCatalog" },
                ["combat_catalog.json"] = new[] { "CombatCatalog" },
                ["verdict_data.json"] = new[] { "VerdictCatalogLoader" },
                ["verdict_questlines.json"] = new[] { "VerdictQuestCatalogLoader" },
                ["disease_catalog.json"] = new[] { "DiseaseCatalog", "DiseaseSystem" },
                ["expeditions.json"] = new[] { "ExpeditionCatalogLoader" },
                ["vehicles.json"] = new[] { "ExpeditionVehicleSystem" },
                ["greenhouse_items.json"] = new[] { "GreenhouseExpansionCatalog" },
                ["library_manuals.json"] = new[] { "LibraryManualCatalogLoader" },
                ["research_knowledge.json"] = new[] { "ResearchKnowledgeCatalogLoader" },
                ["skills.json"] = new[] { "SkillCatalogLoader" },
                ["trade_screen_scenarios.json"] = new[] { "TradeScreenScenarios" },
                ["trade_specialties.json"] = new[] { "TradeSpecialtySystem" },
                ["trade_tell_lines.json"] = new[] { "TradeTellEngine" },
                ["trade_texts.json"] = new[] { "TradeScreenPresenter" },
                ["feedback_messages.json"] = new[] { "FeedbackMessageCatalogLoader" },
                ["final_wishes.json"] = new[] { "FinalWishSystem" },
                ["guilt_sources.json"] = new[] { "GuiltInsomniaSystem" },
                ["incidents.json"] = new[] { "ShelterEncounterSystem" },
                ["moral_choice_chains.json"] = new[] { "MoralChoiceChainCatalogLoader" },
                ["moral_choice_quests.json"] = new[] { "MoralChoiceCatalogLoader" },
                ["moral_choice_quests_branching.json"] = new[] { "MoralChoiceBranchQuestCatalogLoader" },
                ["moral_choice_quests_expansion.json"] = new[] { "MoralChoiceExpansionQuestCatalogLoader" },
                ["moral_choice_faction_reactions.json"] = new[] { "MoralChoiceFactionReactionsCatalogLoader" },
                ["moral_choice_flags.json"] = new[] { "MoralChoiceFlagCatalogLoader" },
                ["moral_choice_gossip.json"] = new[] { "MoralChoiceGossipCatalogLoader" },
                ["hardcore_economy_tuning.json"] = new[] { "HardcoreEconomyTuningLoader" },
                ["relic_recipes.json"] = new[] { "RelicCatalogLoader" },
                ["world_evolution_seeds.json"] = new[] { "EvolvingWorldCatalog" },
                ["shelter_schedules.json"] = new[] { "ShelterScheduleCatalogLoader" },
                ["utility_actions.json"] = new[] { "UtilityAiSystem" },
                ["confession_secrets.json"] = new[] { "ConfessionSecrets" },
                ["dynamic_questlines.json"] = new[] { "QuestlineSystem" },
                ["door_encounters.json"] = new[] { "DoorEncounterCatalogLoader" },
                ["faction_war_communiques.json"] = new[] { "FactionWarContentCatalog" },
                ["faction_war_dialogue.json"] = new[] { "FactionWarContentCatalog" },
                ["faction_war_events.json"] = new[] { "FactionWarContentCatalog" },
                ["faction_war_journal.json"] = new[] { "FactionWarContentCatalog" },
                ["faction_war_location_overrides.json"] = new[] { "FactionWarContentCatalog" },
                ["faction_war_radio.json"] = new[] { "FactionWarContentCatalog" },
                ["independent_faction_branch.json"] = new[] { "IndependentBranchCatalog" },
                ["military_faction_branch.json"] = new[] { "MilitaryBranchCatalog" },
                ["rebel_faction_branch.json"] = new[] { "RebelBranchCatalog" },
                ["faction_radio_corpus.json"] = new[] { "FactionWarContentCatalog" },
                ["radio_distress_signals.json"] = new[] { "SignalTriangulationSystem" },
                ["radio_distress_signals_expansion.json"] = new[] { "SignalTriangulationSystem" },
                ["phantom_triggers.json"] = new[] { "PhantomMemoryHostSession", "PhantomMemoryEngine" },
                ["phantom_heirlooms.json"] = new[] { "HeirloomCatalog", "HeirloomSystem" },
                ["archive_inks.json"] = new[] { "ArchiveInkCatalogLoader" },
                ["autopsy_procedures.json"] = new[] { "AutopsyProcedureCatalogLoader" },
                ["chemical_dependency_items.json"] = new[] { "ChemicalDependencySystem" },
                ["pharma_recipes.json"] = new[] { "PharmaRecipeCatalogLoader" },
                ["power_grid.json"] = new[] { "PowerGridSystem" },
                ["journal_voice_prose.json"] = new[] { "JournalVoiceProseCatalog" },
                ["medical_texts.json"] = new[] { "MedicalWardSystem" },
                ["muster_epilogues.json"] = new[] { "EpilogueMatrix" },
                ["muster_witnesses.json"] = new[] { "WitnessCatalog" },
                ["currents.json"] = new[] { "CurrentsCatalog" },
                ["duty_roster_locations.json"] = new[] { "DutyRosterCatalog" },
                ["duty_roster_marks.json"] = new[] { "DutyRosterCatalog" },
                ["duty_roster_quests.json"] = new[] { "DutyRosterQuestRuntime" },
                ["duty_roster_seasons.json"] = new[] { "DutyRosterCatalog" },
                ["thirdonary_quests.json"] = new[] { "ThirdonaryCatalogLoader" },
                ["foundry_faction.json"] = new[] { "SilentFoundryCatalogLoader" },
                ["foundry_items.json"] = new[] { "SilentFoundryCatalog" },
                ["verdict_npcs.json"] = new[] { "VerdictNpcSystem" },
                ["crossing_encounters.json"] = new[] { "CrossingCatalog" },
                ["crossing_factions.json"] = new[] { "CrossingCatalog" },
                ["crossing_items.json"] = new[] { "CrossingCatalog" },
                ["crossing_locations.json"] = new[] { "CrossingCatalog" },
                ["crossing_quests.json"] = new[] { "CrossingQuestSystem" },
                ["holdfast_factions.json"] = new[] { "HoldfastFactionsCatalog" },
                ["holdfast_items.json"] = new[] { "HoldfastItemsCatalog" },
                ["holdfast_locations.json"] = new[] { "HoldfastCatalog" },
                ["holdfast_quests.json"] = new[] { "HoldfastQuestSystem" },
                ["holdfast_flavor.json"] = new[] { "HoldfastFlavorCatalog" },
                ["starting_supplies.json"] = new[] { "StartingLevelSystem" },
                ["starting_survivors.json"] = new[] { "SurvivorStartingStateLoader" },
                ["standing_record_factions.json"] = new[] { "StandingRecordCatalog" },
                ["standing_record_layouts.json"] = new[] { "LocationLayoutSystem" },
                ["standing_record_memory.json"] = new[] { "LocationMemorySystem" },
                ["standing_record_quests.json"] = new[] { "StandingRecordCatalog" },
                ["year_of_ash_events.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_items.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_locations.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_questlines.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_quests.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_radio.json"] = new[] { "YearOfAshCatalogLoader" },
                ["year_of_ash_survivors.json"] = new[] { "YearOfAshCatalogLoader" },
                ["dose_items.json"] = new[] { "DoseContentCatalog" },
                ["dose_locations.json"] = new[] { "DoseContentCatalog" },
                ["dose_quests.json"] = new[] { "DoseContentCatalog" },
                ["dose_registers.json"] = new[] { "DoseRegistersCatalog" },
                ["deep_lore_locations.json"] = new[] { "DeepLoreLocationCatalogLoader" },
                ["black_flotilla_items.json"] = new[] { "ProceduralScavengeSystem" },
                ["locations_expansion3.json"] = new[] { "LocationLayoutSystem" },
                ["environmental_atmosphere_expansion.json"] = new[] { "WeatherSystem" },
                ["antigravity_survivor_fields.json"] = new[] { "SurvivorCatalog" },
                ["expansion_survivor_fields.json"] = new[] { "SurvivorCatalog" },
                ["deep_lore_survivor_fields.json"] = new[] { "SurvivorCatalog" },
                ["expansion_item_tags.json"] = new[] { "ItemCatalogLoader" },
                ["audio_logs_expansion_05.json"] = new[] { "AudioConditionSystem" },
                ["environmental_texts_expansion_05.json"] = new[] { "NarrativeEncounterSystem" },
                ["journal_entries_expansion_05.json"] = new[] { "JournalSystem" },
                ["memorials_expansion_05.json"] = new[] { "MemorialSystem" },
                ["quests_expansion_05.json"] = new[] { "ExpansionQuestSystem" },
                ["quests_expansion_06.json"] = new[] { "ExpansionQuestSystem" },
                ["narrative_arc_events.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_encounters_expansion.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_progression.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_questlines.json"] = new[] { "NarrativeEncounterSystem" },
                ["characters.json"] = new[] { "SurvivorCatalog" },
                ["item_description_texts.json"] = new[] { "ItemCatalogLoader" },
                ["wall_carving_templates.json"] = new[] { "MemorialSystem" },
                ["wasteland_grave_epitaphs.json"] = new[] { "MemorialSystem" },
                ["damaged_map_zones.json"] = new[] { "WastelandMapSystem" },
                ["cassette_sets.json"] = new[] { "VinylMoraleSystem" },
                ["epilogue_chronicle.json"] = new[] { "EpilogueMatrix" },
            };

            // Additional mappings for previously UNRESOLVED catalogs
            loaderPatterns["vel_triage_log_names.json"] = new[] { "NarrativeBatchCatalog" };
            loaderPatterns["echoes.json"] = Array.Empty<string>(); // Future content, no loader
            loaderPatterns["orphan_knocks.json"] = Array.Empty<string>(); // Whitelist infrastructure
            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (loaderPatterns.TryGetValue(fileName, out var loaders))
                {
                    cat.Loader = string.Join(", ", loaders);
                    cat.MaxStage = UtilizationStage.LOADED;

                    if (loaders.Length > 0)
                    {
                        string loaderId = $"loader:{loaders[0]}";
                        EnsureNode(loaderId, ContentNodeKind.Loader, loaders[0]);
                        AddEdge(cat.Path, loaderId, ContentEdgeKind.LOADED_BY, EvidenceTier.STATIC);
                    }
                }
            }
        }

        // ── Inventory Registries ────────────────────────────────────

        private void InventoryRegistries()
        {
            // Map catalogs to their registries
            var registryMap = new Dictionary<string, string>
            {
                ["items.json"] = "ItemCatalog",
                ["recipes.json"] = "RecipeCatalog",
                ["locations.json"] = "LocationRegistry",
                ["survivors.json"] = "SurvivorCatalog",
                ["faction_lore.json"] = "FactionIconCatalog",
                ["economy_goods.json"] = "GoodsCatalog",
                ["events.json"] = "EventRegistry",
                ["narrative_encounters.json"] = "NarrativeEncounterSystem.Catalog",
                ["questline_master.json"] = "QuestlineMasterCatalog",
                ["warlord_doctrines.json"] = "WarlordDoctrineCatalog",
                ["combat_catalog.json"] = "CombatCatalog",
                ["disease_catalog.json"] = "DiseaseCatalog",
                ["expeditions.json"] = "ExpeditionCatalogLoader",
                ["vehicles.json"] = "ExpeditionVehicleSystem",
                ["greenhouse_items.json"] = "GreenhouseExpansionCatalog",
                ["trade_screen_scenarios.json"] = "TradeScreenScenarios",
                ["moral_choice_quests.json"] = "MoralChoiceSystem",
                ["holdfast_quests.json"] = "HoldfastQuestSystem",
                ["crossing_quests.json"] = "CrossingQuestSystem",
                ["duty_roster_quests.json"] = "DutyRosterQuestRuntime",
                ["year_of_ash_quests.json"] = "YearOfAshCatalog",
                ["verdict_questlines.json"] = "VerdictQuestCatalogLoader",
                ["thirdonary_quests.json"] = "ThirdonaryQuestSystem",
                ["standing_record_quests.json"] = "StandingRecordCatalog",
                ["radio.json"] = "RadioScriptbookCatalog",
                ["faction_war_radio.json"] = "FactionWarContentCatalog",
                ["dose_items.json"] = "DoseContentCatalog",
                ["dose_quests.json"] = "DoseContentCatalog",
                ["dose_registers.json"] = "DoseRegistersCatalog",
                ["journal_voice_prose.json"] = "JournalVoiceProseCatalog",
                ["muster_witnesses.json"] = "WitnessCatalog",
                ["muster_epilogues.json"] = "EpilogueMatrix",
                ["currents.json"] = "CurrentsCatalog",
                ["foundry_accords.json"] = "SilentFoundryCatalog",
                ["foundry_production.json"] = "SilentFoundryCatalog",
                ["foundry_treaty_consequences.json"] = "SilentFoundryConsequencePolicy",
                ["power_grid.json"] = "PowerGridSystem",
                ["shelter_schedules.json"] = "ShelterScheduleSystem",
                ["utility_actions.json"] = "UtilityAiSystem",
                ["pharma_recipes.json"] = "PharmaLabSystem",
                ["relic_recipes.json"] = "WorkshopReverseEngineeringSystem",
                ["feedback_messages.json"] = "FeedbackMessageCatalog",
                ["final_wishes.json"] = "FinalWishSystem",
                ["guilt_sources.json"] = "GuiltInsomniaSystem",
                ["incidents.json"] = "ShelterEncounterSystem",
                ["confession_secrets.json"] = "ConfessionSecrets",
                ["dynamic_questlines.json"] = "QuestlineSystem",
                ["door_encounters.json"] = "DoorEncounterSystem",
                ["independent_faction_branch.json"] = "IndependentBranchSystem",
                ["military_faction_branch.json"] = "MilitaryBranchSystem",
                ["rebel_faction_branch.json"] = "RebelBranchSystem",
                ["radio_distress_signals.json"] = "SignalTriangulationSystem",
                ["phantom_triggers.json"] = "PhantomMemoryHostSession",
                ["phantom_heirlooms.json"] = "HeirloomCatalog",
                ["archive_inks.json"] = "ArchiveDeskSystem",
                ["autopsy_procedures.json"] = "AutopsySystem",
                ["chemical_dependency_items.json"] = "ChemicalDependencySystem",
                ["medical_texts.json"] = "MedicalWardSystem",
                ["trade_specialties.json"] = "TradeSpecialtySystem",
                ["trade_tell_lines.json"] = "TradeTellEngine",
                ["trade_texts.json"] = "TradeScreenPresenter",
                ["hardcore_economy_tuning.json"] = "HardcoreEconomyTuning",
                ["world_evolution_seeds.json"] = "EvolvingWorldCatalog",
                ["library_manuals.json"] = "LibraryStudySystem",
                ["research_knowledge.json"] = "ResearchSystem",
                ["skills.json"] = "SkillProgressionSystem",
                ["starting_supplies.json"] = "StartingLevelSystem",
                ["starting_survivors.json"] = "SurvivorStartingStateLoader",
                ["dive_sites.json"] = "DiveSiteCatalog",
                ["deep_lore_locations.json"] = "DeepLoreLocationCatalogLoader",
                ["black_flotilla_items.json"] = "ProceduralScavengeSystem",
                ["wasteland_map_v1.json"] = "WastelandMapSystem",
                ["weather_seasons.json"] = "WeatherSystem",
                ["world_history.json"] = "EvolvingWorldCatalog",
                ["faction_radio_corpus.json"] = "FactionWarContentCatalog",
                ["faction_war_communiques.json"] = "FactionWarContentCatalog",
                ["faction_war_dialogue.json"] = "FactionWarContentCatalog",
                ["faction_war_events.json"] = "FactionWarContentCatalog",
                ["faction_war_journal.json"] = "FactionWarContentCatalog",
                ["faction_war_location_overrides.json"] = "FactionWarContentCatalog",
                ["moral_choice_chains.json"] = "MoralChoiceSystem",
                ["moral_choice_quests_branching.json"] = "MoralChoiceSystem",
                ["moral_choice_quests_expansion.json"] = "MoralChoiceSystem",
                ["moral_choice_faction_reactions.json"] = "MoralChoiceSystem",
                ["moral_choice_flags.json"] = "MoralChoiceSystem",
                ["moral_choice_gossip.json"] = "MoralChoiceSystem",
                ["moral_choice_quest_stubs.json"] = "MoralChoiceSystem",
                ["duty_roster_locations.json"] = "DutyRosterCatalog",
                ["duty_roster_marks.json"] = "DutyRosterCatalog",
                ["duty_roster_seasons.json"] = "DutyRosterCatalog",
                ["holdfast_factions.json"] = "HoldfastFactionsCatalog",
                ["holdfast_items.json"] = "HoldfastItemsCatalog",
                ["holdfast_locations.json"] = "HoldfastCatalog",
                ["holdfast_flavor.json"] = "HoldfastFlavorCatalog",
                ["crossing_encounters.json"] = "CrossingCatalog",
                ["crossing_factions.json"] = "CrossingCatalog",
                ["crossing_items.json"] = "CrossingCatalog",
                ["crossing_locations.json"] = "CrossingCatalog",
                ["standing_record_factions.json"] = "StandingRecordCatalog",
                ["standing_record_layouts.json"] = "LocationLayoutSystem",
                ["standing_record_memory.json"] = "LocationMemorySystem",
                ["year_of_ash_events.json"] = "YearOfAshCatalog",
                ["year_of_ash_items.json"] = "YearOfAshCatalog",
                ["year_of_ash_locations.json"] = "YearOfAshCatalog",
                ["year_of_ash_questlines.json"] = "YearOfAshCatalog",
                ["year_of_ash_radio.json"] = "YearOfAshCatalog",
                ["year_of_ash_survivors.json"] = "YearOfAshCatalog",
                ["dose_locations.json"] = "DoseContentCatalog",
                ["verdict_data.json"] = "VerdictCatalog",
                ["verdict_items.json"] = "VerdictCatalog",
                ["verdict_locations.json"] = "VerdictCatalog",
                ["verdict_radio.json"] = "VerdictCatalog",
                ["verdict_npcs.json"] = "VerdictNpcSystem",
                ["foundry_faction.json"] = "SilentFoundryCatalog",
                ["foundry_items.json"] = "SilentFoundryCatalog",
                ["locations_expansion3.json"] = "LocationLayoutSystem",
                ["environmental_atmosphere_expansion.json"] = "WeatherSystem",
                ["antigravity_survivor_fields.json"] = "SurvivorCatalog",
                ["expansion_survivor_fields.json"] = "SurvivorCatalog",
                ["deep_lore_survivor_fields.json"] = "SurvivorCatalog",
                ["expansion_item_tags.json"] = "ItemCatalog",
                ["audio_logs_expansion_05.json"] = "AudioConditionSystem",
                ["environmental_texts_expansion_05.json"] = "NarrativeEncounterSystem",
                ["journal_entries_expansion_05.json"] = "JournalSystem",
                ["memorials_expansion_05.json"] = "MemorialSystem",
                ["quests_expansion_05.json"] = "ExpansionQuestSystem",
                ["quests_expansion_06.json"] = "ExpansionQuestSystem",
                ["narrative_arc_events.json"] = "NarrativeEncounterSystem",
                ["narrative_encounters_expansion.json"] = "NarrativeEncounterSystem",
                ["narrative_progression.json"] = "NarrativeEncounterSystem",
                ["narrative_questlines.json"] = "NarrativeEncounterSystem",
                ["characters.json"] = "SurvivorCatalog",
                ["item_description_texts.json"] = "ItemCatalog",
                ["wall_carving_templates.json"] = "MemorialSystem",
                ["wasteland_grave_epitaphs.json"] = "MemorialSystem",
                ["damaged_map_zones.json"] = "WastelandMapSystem",
                ["cassette_sets.json"] = "VinylMoraleSystem",
                ["epilogue_chronicle.json"] = "EpilogueMatrix",
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (registryMap.TryGetValue(fileName, out var registry))
                {
                    string registryId = $"registry:{registry}";
                    EnsureNode(registryId, ContentNodeKind.Registry, registry);
                    AddEdge($"file:{cat.Path}", registryId, ContentEdgeKind.REGISTERED_IN, EvidenceTier.STATIC);
                    cat.MaxStage = UtilizationStage.REGISTERED;
                }
            }
        }

        // ── Inventory Runtime Systems ────────────────────────────────

        private void InventoryRuntimeSystems()
        {
            // Map catalogs to known runtime consumer systems
            var consumerMap = new Dictionary<string, string[]>
            {
                ["items.json"] = new[] { "InventorySystem", "CraftingSystem", "ProceduralItemInstance", "EquipmentConditionSystem" },
                ["recipes.json"] = new[] { "CraftingSystem" },
                ["locations.json"] = new[] { "LocationEvolutionSystem", "WastelandMapSystem", "ExpeditionSystem" },
                ["survivors.json"] = new[] { "SurvivorsHostSession", "NeedsSystem", "RadiationSystem", "CaregivingSystem" },
                ["faction_lore.json"] = new[] { "FactionStanceEngine", "FactionIconLoader" },
                ["economy_goods.json"] = new[] { "MarketSystem", "TradeScreenPresenter" },
                ["events.json"] = new[] { "EventsHostSession", "WorldHostSession" },
                ["weather_seasons.json"] = new[] { "WeatherSystem" },
                ["radio.json"] = new[] { "RadioHostSession", "RadioScriptbookCatalog" },
                ["narrative_encounters.json"] = new[] { "NarrativeEncounterSystem" },
                ["questline_master.json"] = new[] { "QuestlineSystem" },
                ["world_history.json"] = new[] { "EvolvingWorldCatalog" },
                ["wasteland_map_v1.json"] = new[] { "WastelandMapSystem" },
                ["dive_sites.json"] = new[] { "MaritimeDiveSystem" },
                ["foundry_accords.json"] = new[] { "SilentFoundrySystem" },
                ["foundry_production.json"] = new[] { "SilentFoundrySystem" },
                ["foundry_treaty_consequences.json"] = new[] { "SilentFoundrySystem" },
                ["warlord_doctrines.json"] = new[] { "WarlordDoctrineSystem" },
                ["combat_catalog.json"] = new[] { "TacticalCombatSystem", "BallisticsSystem", "WeaponConditionSystem" },
                ["verdict_data.json"] = new[] { "ReckoningSystem", "MachineLogSystem" },
                ["verdict_items.json"] = new[] { "ReckoningSystem" },
                ["verdict_locations.json"] = new[] { "ReckoningSystem" },
                ["verdict_radio.json"] = new[] { "VerdictRadioSystem" },
                ["verdict_questlines.json"] = new[] { "VerdictQuestSystem" },
                ["verdict_npcs.json"] = new[] { "VerdictNpcSystem" },
                ["disease_catalog.json"] = new[] { "DiseaseSystem" },
                ["expeditions.json"] = new[] { "ExpeditionSystem", "ExpeditionEncounterBridge" },
                ["vehicles.json"] = new[] { "ExpeditionVehicleSystem", "ExpeditionSystem" },
                ["greenhouse_items.json"] = new[] { "GreenhouseSystem", "ApicultureSystem" },
                ["library_manuals.json"] = new[] { "LibraryStudySystem" },
                ["research_knowledge.json"] = new[] { "ResearchSystem" },
                ["skills.json"] = new[] { "SkillProgressionSystem", "LatentExpertAwakeningSystem" },
                ["trade_screen_scenarios.json"] = new[] { "TradeScreenPresenter" },
                ["trade_specialties.json"] = new[] { "TradeSpecialtySystem" },
                ["trade_tell_lines.json"] = new[] { "TradeScreenPresenter" },
                ["trade_texts.json"] = new[] { "TradeScreenPresenter" },
                ["feedback_messages.json"] = new[] { "FeedbackMessageCatalog" },
                ["final_wishes.json"] = new[] { "FinalWishSystem" },
                ["guilt_sources.json"] = new[] { "GuiltInsomniaSystem" },
                ["incidents.json"] = new[] { "ShelterEncounterSystem" },
                ["moral_choice_chains.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_quests.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_quests_branching.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_quests_expansion.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_faction_reactions.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_flags.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_gossip.json"] = new[] { "MoralChoiceSystem" },
                ["moral_choice_quest_stubs.json"] = new[] { "MoralChoiceSystem" },
                ["hardcore_economy_tuning.json"] = new[] { "MarketSystem" },
                ["relic_recipes.json"] = new[] { "WorkshopReverseEngineeringSystem" },
                ["world_evolution_seeds.json"] = new[] { "EvolvingWorldCatalog" },
                ["shelter_schedules.json"] = new[] { "ShelterScheduleSystem" },
                ["utility_actions.json"] = new[] { "UtilityAiSystem" },
                ["confession_secrets.json"] = new[] { "ConfessionSecrets" },
                ["dynamic_questlines.json"] = new[] { "QuestlineSystem" },
                ["door_encounters.json"] = new[] { "DoorEncounterSystem" },
                ["faction_war_communiques.json"] = new[] { "FactionWarSystem" },
                ["faction_war_dialogue.json"] = new[] { "FactionWarSystem" },
                ["faction_war_events.json"] = new[] { "FactionWarSystem" },
                ["faction_war_journal.json"] = new[] { "FactionWarSystem" },
                ["faction_war_location_overrides.json"] = new[] { "FactionWarSystem" },
                ["faction_war_radio.json"] = new[] { "FactionWarSystem" },
                ["faction_radio_corpus.json"] = new[] { "FactionWarSystem" },
                ["independent_faction_branch.json"] = new[] { "IndependentBranchSystem" },
                ["military_faction_branch.json"] = new[] { "MilitaryBranchSystem" },
                ["rebel_faction_branch.json"] = new[] { "RebelBranchSystem" },
                ["radio_distress_signals.json"] = new[] { "SignalTriangulationSystem" },
                ["radio_distress_signals_expansion.json"] = new[] { "SignalTriangulationSystem" },
                ["phantom_triggers.json"] = new[] { "PhantomMemoryHostSession", "PhantomMemoryEngine" },
                ["phantom_heirlooms.json"] = new[] { "HeirloomSystem", "GenerationalLineageExtension", "SurvivorRelationsSystem" },
                ["archive_inks.json"] = new[] { "ArchiveDeskSystem" },
                ["autopsy_procedures.json"] = new[] { "AutopsySystem" },
                ["chemical_dependency_items.json"] = new[] { "ChemicalDependencySystem" },
                ["pharma_recipes.json"] = new[] { "PharmaLabSystem" },
                ["power_grid.json"] = new[] { "PowerGridSystem" },
                ["journal_voice_prose.json"] = new[] { "JournalSystem" },
                ["medical_texts.json"] = new[] { "MedicalWardSystem" },
                ["muster_epilogues.json"] = new[] { "MusterSystem" },
                ["muster_witnesses.json"] = new[] { "MusterSystem" },
                ["currents.json"] = new[] { "MusterSystem" },
                ["duty_roster_locations.json"] = new[] { "DutyRosterSystem" },
                ["duty_roster_marks.json"] = new[] { "DutyRosterSystem" },
                ["duty_roster_quests.json"] = new[] { "DutyRosterSystem" },
                ["duty_roster_seasons.json"] = new[] { "DutyRosterSystem" },
                ["thirdonary_quests.json"] = new[] { "ThirdonaryQuestSystem" },
                ["foundry_faction.json"] = new[] { "SilentFoundrySystem" },
                ["foundry_items.json"] = new[] { "SilentFoundrySystem" },
                ["crossing_encounters.json"] = new[] { "CrossingArbitrationSystem" },
                ["crossing_factions.json"] = new[] { "CrossingArbitrationSystem" },
                ["crossing_items.json"] = new[] { "CrossingArbitrationSystem" },
                ["crossing_locations.json"] = new[] { "CrossingArbitrationSystem" },
                ["crossing_quests.json"] = new[] { "CrossingQuestSystem" },
                ["holdfast_factions.json"] = new[] { "HoldfastRuntimeSession" },
                ["holdfast_items.json"] = new[] { "HoldfastRuntimeSession" },
                ["holdfast_locations.json"] = new[] { "HoldfastRuntimeSession", "IceRoadSystem" },
                ["holdfast_quests.json"] = new[] { "HoldfastQuestSystem" },
                ["holdfast_flavor.json"] = new[] { "HoldfastRuntimeSession" },
                ["starting_supplies.json"] = new[] { "StartingLevelSystem" },
                ["starting_survivors.json"] = new[] { "SurvivorsHostSession" },
                ["standing_record_factions.json"] = new[] { "ExpansionHubSession" },
                ["standing_record_layouts.json"] = new[] { "LocationLayoutSystem" },
                ["standing_record_memory.json"] = new[] { "LocationMemorySystem" },
                ["standing_record_quests.json"] = new[] { "ExpansionHubSession" },
                ["year_of_ash_events.json"] = new[] { "YearOfAshTimelineSystem" },
                ["year_of_ash_items.json"] = new[] { "YearOfAshSystem" },
                ["year_of_ash_locations.json"] = new[] { "YearOfAshSystem" },
                ["year_of_ash_questlines.json"] = new[] { "YearOfAshSystem" },
                ["year_of_ash_quests.json"] = new[] { "YearOfAshSystem" },
                ["year_of_ash_radio.json"] = new[] { "YearOfAshSystem" },
                ["year_of_ash_survivors.json"] = new[] { "YearOfAshSystem" },
                ["dose_items.json"] = new[] { "DoseLedgerSystem" },
                ["dose_locations.json"] = new[] { "DoseLedgerSystem" },
                ["dose_quests.json"] = new[] { "DoseLedgerSystem" },
                ["dose_registers.json"] = new[] { "DoseLedgerSystem" },
                ["deep_lore_locations.json"] = new[] { "MaritimeDiveSystem" },
                ["black_flotilla_items.json"] = new[] { "MaritimeDiveSystem", "ProceduralScavengeSystem" },
                ["locations_expansion3.json"] = new[] { "LocationLayoutSystem" },
                ["environmental_atmosphere_expansion.json"] = new[] { "WeatherSystem" },
                ["antigravity_survivor_fields.json"] = new[] { "SurvivorsHostSession" },
                ["expansion_survivor_fields.json"] = new[] { "SurvivorsHostSession" },
                ["deep_lore_survivor_fields.json"] = new[] { "SurvivorsHostSession" },
                ["expansion_item_tags.json"] = new[] { "InventorySystem" },
                ["audio_logs_expansion_05.json"] = new[] { "AudioConditionSystem" },
                ["environmental_texts_expansion_05.json"] = new[] { "NarrativeEncounterSystem" },
                ["journal_entries_expansion_05.json"] = new[] { "JournalSystem" },
                ["memorials_expansion_05.json"] = new[] { "MemorialSystem" },
                ["quests_expansion_05.json"] = new[] { "ExpansionQuestSystem" },
                ["quests_expansion_06.json"] = new[] { "ExpansionQuestSystem" },
                ["narrative_arc_events.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_encounters_expansion.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_progression.json"] = new[] { "NarrativeEncounterSystem" },
                ["narrative_questlines.json"] = new[] { "NarrativeEncounterSystem" },
                ["characters.json"] = new[] { "SurvivorsHostSession" },
                ["item_description_texts.json"] = new[] { "InventorySystem" },
                ["wall_carving_templates.json"] = new[] { "MemorialSystem" },
                ["wasteland_grave_epitaphs.json"] = new[] { "MemorialSystem" },
                ["damaged_map_zones.json"] = new[] { "WastelandMapSystem" },
                ["cassette_sets.json"] = new[] { "VinylMoraleSystem" },
                ["epilogue_chronicle.json"] = new[] { "MusterSystem" },
                ["workshop_recipes.json"] = new[] { "ShelterWorkshopSystem" },
                ["radio_intercepts.json"] = new[] { "ShelterRadioStationSystem" },
                ["shelter_social_events.json"] = new[] { "ShelterSocialDynamicsSystem" },
                ["excavation_hazard_mitigation.json"] = new[] { "ExcavationHazardSystem" },
                ["chemical_weapons.json"] = new[] { "ChemWarfareSystem" },
                ["comms_targets.json"] = new[] { "CommsArraySystem" },
                ["ceremonies.json"] = new[] { "CeremonySystem" },
                ["robotics.json"] = new[] { "RoboticsSystem" },
                ["item_degradation.json"] = new[] { "EquipmentConditionSystem" },
                ["thermal_gear.json"] = new[] { "ShelterThermalSystem" },
                ["naval_vessels.json"] = new[] { "ExpeditionNavalSystem" },
                ["recreation.json"] = new[] { "SurvivorDowntimeSystem" },
                ["fallout_patterns.json"] = new[] { "FalloutSystem" },
                ["desperation_events.json"] = new[] { "DesperationSystem" },
                ["bounty_board.json"] = new[] { "MercenarySystem" },
                ["lore_archives.json"] = new[] { "ArchaeologySystem" },
                ["surgical_procedures.json"] = new[] { "AmputationSystem" },
                ["rail_network.json"] = new[] { "RailwaySystem" },
                ["underground_flora.json"] = new[] { "FungiCultivationSystem" },
                ["wasteland_laws.json"] = new[] { "JusticeSystem" },
                ["development_traits.json"] = new[] { "GenerationalSystem" },
                ["interrogation_tactics.json"] = new[] { "PrisonerSystem" },
                ["mutations.json"] = new[] { "MutationSystem" },
                ["camouflage_gear.json"] = new[] { "StealthSystem" },
                ["aircraft_parts.json"] = new[] { "AviationSystem" },
                ["labor_camps.json"] = new[] { "ForcedLaborSystem" },
                ["narcotics.json"] = new[] { "NarcoticsSystem" },
                ["political_policies.json"] = new[] { "PoliticsSystem" },
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (consumerMap.TryGetValue(fileName, out var consumers))
                {
                    cat.ConsumerSystems.AddRange(consumers);
                    cat.MaxStage = UtilizationStage.QUERIED;

                    foreach (var consumer in consumers)
                    {
                        string consumerId = $"system:{consumer}";
                        EnsureNode(consumerId, ContentNodeKind.RuntimeSystem, consumer);
                        AddEdge($"file:{cat.Path}", consumerId, ContentEdgeKind.CONSUMED_BY, EvidenceTier.STATIC);
                    }
                }
            }
        }

        // ── Inventory Queries ────────────────────────────────────────

        private void InventoryQueries()
        {
            // Mark catalogs that have runtime query APIs
            var querySystems = new HashSet<string>
            {
                "items.json", "recipes.json", "locations.json", "survivors.json",
                "narrative_encounters.json", "questline_master.json", "radio.json",
                "economy_goods.json", "events.json", "weather_seasons.json",
                "expeditions.json", "warlord_doctrines.json", "combat_catalog.json",
                "disease_catalog.json", "trade_screen_scenarios.json",
                "moral_choice_quests.json", "duty_roster_quests.json",
                "holdfast_quests.json", "crossing_quests.json",
                "year_of_ash_quests.json", "verdict_questlines.json",
                "thirdonary_quests.json", "standing_record_quests.json",
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (querySystems.Contains(fileName))
                {
                    if (cat.MaxStage < UtilizationStage.QUERIED)
                        cat.MaxStage = UtilizationStage.QUERIED;
                }
            }
        }

        // ── Inventory UI Surfaces ────────────────────────────────────

        private void InventoryUiSurfaces()
        {
            var uiConsumers = new Dictionary<string, string[]>
            {
                ["items.json"] = new[] { "InventoryPanel", "CraftingPanel", "EquipmentPanel" },
                ["survivors.json"] = new[] { "SurvivorsPanel", "RosterPanel" },
                ["locations.json"] = new[] { "MapPanel", "ExpeditionPanel" },
                ["economy_goods.json"] = new[] { "TradePanel", "MarketPanel" },
                ["radio.json"] = new[] { "RadioPanel" },
                ["narrative_encounters.json"] = new[] { "NarrativePanel" },
                ["questline_master.json"] = new[] { "QuestPanel" },
                ["duty_roster_quests.json"] = new[] { "DutyRosterPanel" },
                ["holdfast_quests.json"] = new[] { "HoldfastTerminal" },
                ["trade_screen_scenarios.json"] = new[] { "TradePanel" },
                ["journal_voice_prose.json"] = new[] { "JournalBookUI" },
                ["world_history.json"] = new[] { "CodexPanel" },
                ["faction_lore.json"] = new[] { "FactionsPanel" },
                ["wasteland_map_v1.json"] = new[] { "MapPanel" },
                ["weather_seasons.json"] = new[] { "DashboardHUD" },
                ["verdict_data.json"] = new[] { "VerdictPanel" },
                ["expeditions.json"] = new[] { "ExpeditionPanel" },
                ["disease_catalog.json"] = new[] { "MedicalPanel" },
                ["power_grid.json"] = new[] { "PowerGridPanel" },
                ["shelter_schedules.json"] = new[] { "ShelterPanel" },
                ["greenhouse_items.json"] = new[] { "GreenhousePanel" },
                ["foundry_production.json"] = new[] { "FoundryPanel" },
                ["year_of_ash_quests.json"] = new[] { "YearOfAshPanel" },
                ["dose_items.json"] = new[] { "DosePanel" },
                ["muster_epilogues.json"] = new[] { "MusterPanel" },
                ["muster_witnesses.json"] = new[] { "MusterPanel" },
                ["currents.json"] = new[] { "MusterPanel" },
                ["combat_catalog.json"] = new[] { "CombatPanel" },
                ["warlord_doctrines.json"] = new[] { "WarlordPanel" },
                ["crossing_quests.json"] = new[] { "CrossingPanel" },
                ["moral_choice_quests.json"] = new[] { "MoralChoicePanel" },
                ["faction_war_radio.json"] = new[] { "FactionWarPanel" },
                ["thirdonary_quests.json"] = new[] { "ThirdonaryPanel" },
                ["standing_record_quests.json"] = new[] { "StandingRecordPanel" },
                ["verdict_questlines.json"] = new[] { "VerdictPanel" },
                ["library_manuals.json"] = new[] { "LibraryPanel" },
                ["medical_texts.json"] = new[] { "MedicalPanel" },
                ["feedback_messages.json"] = new[] { "FeedbackPanel" },
                ["final_wishes.json"] = new[] { "FinalWishPanel" },
                ["guilt_sources.json"] = new[] { "GuiltPanel" },
                ["incidents.json"] = new[] { "IncidentPanel" },
                ["confession_secrets.json"] = new[] { "ConfessionPanel" },
                ["dynamic_questlines.json"] = new[] { "QuestPanel" },
                ["door_encounters.json"] = new[] { "DoorPanel" },
                ["independent_faction_branch.json"] = new[] { "FactionsPanel" },
                ["military_faction_branch.json"] = new[] { "FactionsPanel" },
                ["rebel_faction_branch.json"] = new[] { "FactionsPanel" },
                ["radio_distress_signals.json"] = new[] { "RadioPanel" },
                ["phantom_triggers.json"] = new[] { "PhantomPanel" },
                ["archive_inks.json"] = new[] { "ArchivePanel" },
                ["autopsy_procedures.json"] = new[] { "AutopsyPanel" },
                ["chemical_dependency_items.json"] = new[] { "ChemicalPanel" },
                ["pharma_recipes.json"] = new[] { "PharmaPanel" },
                ["relic_recipes.json"] = new[] { "RelicPanel" },
                ["wall_carving_templates.json"] = new[] { "MemorialPanel" },
                ["wasteland_grave_epitaphs.json"] = new[] { "MemorialPanel" },
                ["damaged_map_zones.json"] = new[] { "MapPanel" },
                ["cassette_sets.json"] = new[] { "VinylPanel" },
                ["epilogue_chronicle.json"] = new[] { "EpiloguePanel" },
                ["dive_sites.json"] = new[] { "MaritimePanel" },
                ["deep_lore_locations.json"] = new[] { "MaritimePanel" },
                ["black_flotilla_items.json"] = new[] { "MaritimePanel" },
                ["locations_expansion3.json"] = new[] { "MapPanel" },
                ["environmental_atmosphere_expansion.json"] = new[] { "DashboardHUD" },
                ["antigravity_survivor_fields.json"] = new[] { "SurvivorsPanel" },
                ["expansion_survivor_fields.json"] = new[] { "SurvivorsPanel" },
                ["deep_lore_survivor_fields.json"] = new[] { "SurvivorsPanel" },
                ["expansion_item_tags.json"] = new[] { "InventoryPanel" },
                ["audio_logs_expansion_05.json"] = new[] { "AudioPanel" },
                ["environmental_texts_expansion_05.json"] = new[] { "NarrativePanel" },
                ["journal_entries_expansion_05.json"] = new[] { "JournalBookUI" },
                ["memorials_expansion_05.json"] = new[] { "MemorialPanel" },
                ["quests_expansion_05.json"] = new[] { "QuestPanel" },
                ["quests_expansion_06.json"] = new[] { "QuestPanel" },
                ["narrative_arc_events.json"] = new[] { "NarrativePanel" },
                ["narrative_encounters_expansion.json"] = new[] { "NarrativePanel" },
                ["narrative_progression.json"] = new[] { "NarrativePanel" },
                ["narrative_questlines.json"] = new[] { "NarrativePanel" },
                ["characters.json"] = new[] { "SurvivorsPanel" },
                ["item_description_texts.json"] = new[] { "InventoryPanel" },
                ["holdfast_factions.json"] = new[] { "HoldfastTerminal" },
                ["holdfast_items.json"] = new[] { "HoldfastTerminal" },
                ["holdfast_locations.json"] = new[] { "HoldfastTerminal" },
                ["holdfast_flavor.json"] = new[] { "HoldfastTerminal" },
                ["crossing_encounters.json"] = new[] { "CrossingPanel" },
                ["crossing_factions.json"] = new[] { "CrossingPanel" },
                ["crossing_items.json"] = new[] { "CrossingPanel" },
                ["crossing_locations.json"] = new[] { "CrossingPanel" },
                ["standing_record_factions.json"] = new[] { "StandingRecordPanel" },
                ["standing_record_layouts.json"] = new[] { "StandingRecordPanel" },
                ["standing_record_memory.json"] = new[] { "StandingRecordPanel" },
                ["year_of_ash_events.json"] = new[] { "YearOfAshPanel" },
                ["year_of_ash_items.json"] = new[] { "YearOfAshPanel" },
                ["year_of_ash_locations.json"] = new[] { "YearOfAshPanel" },
                ["year_of_ash_questlines.json"] = new[] { "YearOfAshPanel" },
                ["year_of_ash_radio.json"] = new[] { "YearOfAshPanel" },
                ["year_of_ash_survivors.json"] = new[] { "YearOfAshPanel" },
                ["dose_locations.json"] = new[] { "DosePanel" },
                ["dose_quests.json"] = new[] { "DosePanel" },
                ["dose_registers.json"] = new[] { "DosePanel" },
                ["verdict_items.json"] = new[] { "VerdictPanel" },
                ["verdict_locations.json"] = new[] { "VerdictPanel" },
                ["verdict_radio.json"] = new[] { "VerdictPanel" },
                ["verdict_npcs.json"] = new[] { "VerdictPanel" },
                ["foundry_faction.json"] = new[] { "FoundryPanel" },
                ["foundry_items.json"] = new[] { "FoundryPanel" },
                ["foundry_treaty_consequences.json"] = new[] { "FoundryPanel" },
                ["duty_roster_locations.json"] = new[] { "DutyRosterPanel" },
                ["duty_roster_marks.json"] = new[] { "DutyRosterPanel" },
                ["duty_roster_seasons.json"] = new[] { "DutyRosterPanel" },
                ["faction_radio_corpus.json"] = new[] { "FactionWarPanel" },
                ["faction_war_communiques.json"] = new[] { "FactionWarPanel" },
                ["faction_war_dialogue.json"] = new[] { "FactionWarPanel" },
                ["faction_war_events.json"] = new[] { "FactionWarPanel" },
                ["faction_war_journal.json"] = new[] { "FactionWarPanel" },
                ["faction_war_location_overrides.json"] = new[] { "FactionWarPanel" },
                ["moral_choice_chains.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_quests_branching.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_quests_expansion.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_faction_reactions.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_flags.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_gossip.json"] = new[] { "MoralChoicePanel" },
                ["moral_choice_quest_stubs.json"] = new[] { "MoralChoicePanel" },
                ["hardcore_economy_tuning.json"] = new[] { "TradePanel" },
                ["world_evolution_seeds.json"] = new[] { "WorldPanel" },
                ["radio_distress_signals_expansion.json"] = new[] { "RadioPanel" },
                ["starting_supplies.json"] = new[] { "StartingLevelPanel" },
                ["starting_survivors.json"] = new[] { "StartingLevelPanel" },
                ["trade_specialties.json"] = new[] { "TradePanel" },
                ["trade_tell_lines.json"] = new[] { "TradePanel" },
                ["trade_texts.json"] = new[] { "TradePanel" },
                ["research_knowledge.json"] = new[] { "ResearchPanel" },
                ["skills.json"] = new[] { "SurvivorsPanel" },
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (uiConsumers.TryGetValue(fileName, out var panels))
                {
                    foreach (var panel in panels)
                    {
                        string panelId = $"ui:{panel}";
                        EnsureNode(panelId, ContentNodeKind.UiSurface, panel);
                        AddEdge($"file:{cat.Path}", panelId, ContentEdgeKind.DISPLAYED_BY, EvidenceTier.STATIC);
                    }
                }
            }
        }

        // ── Inventory Codex Surfaces ─────────────────────────────────

        private void InventoryCodexSurfaces()
        {
            // Codex/Journal surfaces that consume content
            var codexConsumers = new HashSet<string>
            {
                "items.json", "locations.json", "survivors.json", "events.json",
                "world_history.json", "faction_lore.json", "radio.json",
                "journal_voice_prose.json", "verdict_data.json",
                "muster_epilogues.json", "muster_witnesses.json",
                "currents.json", "narrative_encounters.json",
                "medical_texts.json", "library_manuals.json",
                "wasteland_grave_epitaphs.json", "wall_carving_templates.json",
                "epilogue_chronicle.json", "cassette_sets.json",
                "archive_inks.json", "autopsy_procedures.json",
                "warlord_doctrines.json", "combat_catalog.json",
                "disease_catalog.json", "environmental_atmosphere_expansion.json",
                "environmental_texts_expansion_05.json", "narrative_arc_events.json",
                "narrative_encounters_expansion.json", "narrative_progression.json",
                "narrative_questlines.json", "confession_secrets.json",
                "deep_lore_locations.json", "characters.json",
                "item_description_texts.json", "audio_logs_expansion_05.json",
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (codexConsumers.Contains(fileName))
                {
                    string codexId = "codex:JournalCodex";
                    EnsureNode(codexId, ContentNodeKind.CodexSurface, "JournalCodex");
                    AddEdge($"file:{cat.Path}", codexId, ContentEdgeKind.DISPLAYED_BY, EvidenceTier.STATIC);
                }

                // Narrative subdirectory files are codex surfaces
                if (IsNarrativeSubdirectoryFile(cat.Path))
                {
                    string codexId = "codex:JournalCodex";
                    EnsureNode(codexId, ContentNodeKind.CodexSurface, "JournalCodex");
                    AddEdge($"file:{cat.Path}", codexId, ContentEdgeKind.DISPLAYED_BY, EvidenceTier.CONFIG);
                }
            }
        }

        // ── Inventory Tests ──────────────────────────────────────────

        private void InventoryTests()
        {
            var testCoveredCatalogs = new HashSet<string>
            {
                "items.json", "recipes.json", "locations.json", "survivors.json",
                "narrative_encounters.json", "questline_master.json",
                "warlord_doctrines.json", "combat_catalog.json",
                "disease_catalog.json", "expeditions.json",
                "moral_choice_quests.json", "duty_roster_quests.json",
                "holdfast_quests.json", "crossing_quests.json",
                "year_of_ash_quests.json", "verdict_questlines.json",
                "thirdonary_quests.json", "standing_record_quests.json",
                "trade_screen_scenarios.json", "economy_goods.json",
                "radio.json", "events.json", "weather_seasons.json",
                "world_history.json", "wasteland_map_v1.json",
                "faction_lore.json", "foundry_accords.json",
                "foundry_production.json", "shelter_schedules.json",
                "power_grid.json", "greenhouse_items.json",
                "library_manuals.json", "journal_voice_prose.json",
                "medical_texts.json", "muster_epilogues.json",
                "muster_witnesses.json", "currents.json",
                "dose_items.json", "dose_quests.json", "dose_registers.json",
                "vehicles.json", "feedback_messages.json",
                "final_wishes.json", "guilt_sources.json",
                "incidents.json", "utility_actions.json",
                "confession_secrets.json", "dynamic_questlines.json",
                "door_encounters.json", "faction_war_radio.json",
                "independent_faction_branch.json", "military_faction_branch.json",
                "rebel_faction_branch.json", "radio_distress_signals.json",
                "phantom_triggers.json", "archive_inks.json",
                "autopsy_procedures.json", "chemical_dependency_items.json",
                "pharma_recipes.json", "relic_recipes.json",
                "wall_carving_templates.json", "wasteland_grave_epitaphs.json",
                "damaged_map_zones.json", "cassette_sets.json",
                "epilogue_chronicle.json", "dive_sites.json",
                "deep_lore_locations.json", "black_flotilla_items.json",
            };

            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);
                if (testCoveredCatalogs.Contains(fileName))
                {
                    string testId = $"test:DataWiringIntegrationTests";
                    EnsureNode(testId, ContentNodeKind.Test, "DataWiringIntegrationTests");
                    AddEdge($"file:{cat.Path}", testId, ContentEdgeKind.TESTED_BY, EvidenceTier.TEST);
                }
            }
        }

        // ── Count Definitions ────────────────────────────────────────
        // Phase: scan every catalog file's raw JSON for "id":"..." pairs and
        // populate cat.DefinitionCount + DefinitionEntry rows. Engine-agnostic
        // IFileIO read; no campaign mutation; no RNG; deterministic ordinal
        // sort. Bounded memory: caps per-catalog storage to MaxSampleIDs (200
        // representative ids) so the manifest stays compact for large narrative
        // subdirectories. Total counts always reported through DefinitionEntry.

        private const int MaxSampleIdsPerCatalog = 200;
        private static readonly System.Text.RegularExpressions.Regex IdPairRegex =
            new System.Text.RegularExpressions.Regex(
                "\"id\"\\s*:\\s*\"([a-z][a-z0-9_]{1,63})\"",
                System.Text.RegularExpressions.RegexOptions.Compiled);

        private void CountDefinitions()
        {
            if (!Directory.Exists(_dataDir)) return;

            foreach (var cat in _graph.Catalogs)
            {
                string full = Path.Combine(_dataDir, cat.Path);
                if (!File.Exists(full)) continue;

                string text;
                try { text = File.ReadAllText(full); }
                catch (Exception ex)
                {
                    _log?.Warn($"[CountDefinitions] Failed to read {cat.Path}: {ex.Message}");
                    continue;
                }

                var matches = IdPairRegex.Matches(text);
                int total = 0;
                var seen = new HashSet<string>(StringComparer.Ordinal);
                var sampleStore = new List<DefinitionEntry>(Math.Min(MaxSampleIdsPerCatalog, matches.Count));

                foreach (System.Text.RegularExpressions.Match m in matches)
                {
                    string id = m.Groups[1].Value;
                    if (string.IsNullOrEmpty(id)) continue;
                    if (!seen.Add(id)) continue; // dedupe per catalog
                    total++;
                    if (sampleStore.Count < MaxSampleIdsPerCatalog)
                    {
                        sampleStore.Add(new DefinitionEntry
                        {
                            Id = id,
                            Catalog = cat.Path,
                            Classification = cat.Classification,
                            Reachability = ReachabilityStatus.UNKNOWN,
                            Confidence = ConfidenceLevel.UNVERIFIED,
                        });
                    }
                }
                _graph.Definitions.AddRange(sampleStore);
                cat.DefinitionCount = total;
                cat.RegisteredCount = total;
            }
            // Stabilize() (called from Scan() after this method) does the
            // global Definitions.Sort; per-catalog sample stores are append-
            // ordered so the stable sort yields deterministic output.
        }

        // ── Build Relationships ──────────────────────────────────────

        private void BuildRelationships()
        {
            // Build family summaries
            var families = new Dictionary<string, ContentFamilySummary>();
            foreach (var cat in _graph.Catalogs)
            {
                string family = DetermineFamily(cat.Path);
                if (!families.TryGetValue(family, out var summary))
                {
                    summary = new ContentFamilySummary { Family = family };
                    families[family] = summary;
                }
                summary.Catalogs++;

                switch (cat.Classification)
                {
                    case ContentClassification.GAMEPLAY_CONSUMED: summary.GameplayConsumed++; break;
                    case ContentClassification.UI_ONLY: summary.UiOnly++; break;
                    case ContentClassification.CODEX_ONLY: summary.CodexOnly++; break;
                    case ContentClassification.OPTIONAL: summary.Optional++; break;
                    case ContentClassification.TEST_ONLY: summary.TestOnly++; break;
                    case ContentClassification.ORPHANED: summary.Orphaned++; break;
                    case ContentClassification.UNRESOLVED: summary.Unresolved++; break;
                }

                if (cat.DefinitionCount > 0) summary.Definitions += cat.DefinitionCount;
            }
            _graph.FamilySummaries.AddRange(families.Values);
        }

        private string DetermineFamily(string path)
        {
            if (path.StartsWith("narrative/", StringComparison.OrdinalIgnoreCase)) return "Narrative";
            if (path.StartsWith("documents/", StringComparison.OrdinalIgnoreCase)) return "Documents";
            if (path.StartsWith("whitelists/", StringComparison.OrdinalIgnoreCase)) return "Whitelists";

            string fileName = Path.GetFileName(path).ToLowerInvariant();
            if (fileName.Contains("quest")) return "Quests";
            if (fileName.Contains("item")) return "Items";
            if (fileName.Contains("location") || fileName.Contains("map")) return "Locations";
            if (fileName.Contains("survivor") || fileName.Contains("character")) return "Survivors";
            if (fileName.Contains("faction")) return "Factions";
            if (fileName.Contains("economy") || fileName.Contains("trade") || fileName.Contains("market")) return "Economy";
            if (fileName.Contains("radio") || fileName.Contains("signal")) return "Radio";
            if (fileName.Contains("event") || fileName.Contains("incident")) return "Events";
            if (fileName.Contains("weather") || fileName.Contains("climate")) return "Weather";
            if (fileName.Contains("world") || fileName.Contains("history")) return "World";
            if (fileName.Contains("disease") || fileName.Contains("medical") || fileName.Contains("dose") || fileName.Contains("autopsy") || fileName.Contains("pharma")) return "Medical";
            if (fileName.Contains("combat") || fileName.Contains("warlord") || fileName.Contains("weapon")) return "Combat";
            if (fileName.Contains("expedition") || fileName.Contains("vehicle")) return "Expeditions";
            if (fileName.Contains("shelter") || fileName.Contains("power") || fileName.Contains("schedule")) return "Shelter";
            if (fileName.Contains("greenhouse") || fileName.Contains("apiculture") || fileName.Contains("crop")) return "Greenhouse";
            if (fileName.Contains("foundry") || fileName.Contains("forge") || fileName.Contains("craft")) return "Foundry";
            if (fileName.Contains("recipe") || fileName.Contains("relic") || fileName.Contains("library")) return "Crafting";
            if (fileName.Contains("moral") || fileName.Contains("choice")) return "MoralChoice";
            if (fileName.Contains("muster") || fileName.Contains("epilogue") || fileName.Contains("currents")) return "Muster";
            if (fileName.Contains("verdict") || fileName.Contains("reckoning")) return "Verdict";
            if (fileName.Contains("year_of_ash") || fileName.Contains("door_encounter")) return "YearOfAsh";
            if (fileName.Contains("duty_roster") || fileName.Contains("roster")) return "DutyRoster";
            if (fileName.Contains("holdfast") || fileName.Contains("ice_road")) return "Holdfast";
            if (fileName.Contains("crossing") || fileName.Contains("arbitration")) return "Crossing";
            if (fileName.Contains("standing_record")) return "StandingRecord";
            if (fileName.Contains("journal") || fileName.Contains("codex")) return "Journal";
            if (fileName.Contains("feedback") || fileName.Contains("message")) return "Feedback";
            if (fileName.Contains("guilt") || fileName.Contains("final_wish") || fileName.Contains("confession")) return "Social";
            if (fileName.Contains("maritime") || fileName.Contains("dive") || fileName.Contains("deep_lore") || fileName.Contains("black_flotilla")) return "Maritime";
            if (fileName.Contains("expansion") || fileName.Contains("audio_log") || fileName.Contains("environmental_text") || fileName.Contains("memorial")) return "Expansion";
            if (fileName.Contains("phantom") || fileName.Contains("utility")) return "Utility";
            if (fileName.Contains("archive") || fileName.Contains("ink")) return "Archive";
            if (fileName.Contains("chemical") || fileName.Contains("dependency")) return "Medical";
            if (fileName.Contains("vinyl") || fileName.Contains("cassette") || fileName.Contains("audio")) return "Audio";
            if (fileName.Contains("thirdonary")) return "Thirdonary";
            if (fileName.Contains("wasteland") || fileName.Contains("grave") || fileName.Contains("wall_")) return "World";
            return "Other";
        }

        // ── Classify Content ─────────────────────────────────────────

        private void ClassifyContent()
        {
            foreach (var cat in _graph.Catalogs)
            {
                string fileName = Path.GetFileName(cat.Path);

                // Narrative subdirectory files are codex-only
                if (IsNarrativeSubdirectoryFile(cat.Path))
                {
                    cat.Classification = ContentClassification.CODEX_ONLY;
                    continue;
                }

                // Whitelist and documents are infrastructure
                if (cat.Path.StartsWith("whitelists/", StringComparison.OrdinalIgnoreCase)
                    || cat.Path.StartsWith("documents/", StringComparison.OrdinalIgnoreCase))
                {
                    cat.Classification = ContentClassification.OPTIONAL;
                    cat.ExemptionId = cat.Path.StartsWith("whitelists/", StringComparison.OrdinalIgnoreCase)
                        ? "exempt_whitelist_infra" : "exempt_documents_supplementary";
                    continue;
                }

                // echoes.json is future narrative content
                if (fileName.Equals("echoes.json", StringComparison.OrdinalIgnoreCase))
                {
                    cat.Classification = ContentClassification.OPTIONAL;
                    cat.ExemptionId = "exempt_echoes_future";
                    cat.Findings.Add("Future narrative content — no loader or consumer yet");
                    continue;
                }

                // Has consumer systems → GAMEPLAY_CONSUMED
                if (cat.ConsumerSystems.Count > 0 && cat.MaxStage >= UtilizationStage.QUERIED)
                {
                    cat.Classification = ContentClassification.GAMEPLAY_CONSUMED;
                }
                // Has loader but no consumer → ORPHANED
                else if (!string.IsNullOrEmpty(cat.Loader) && cat.ConsumerSystems.Count == 0)
                {
                    cat.Classification = ContentClassification.ORPHANED;
                    cat.Findings.Add("Registered catalog has loader but no known runtime consumer");
                }
                // No loader → ORPHANED
                else if (string.IsNullOrEmpty(cat.Loader) && IsAuthoritativeCatalog(fileName))
                {
                    cat.Classification = ContentClassification.ORPHANED;
                    cat.Findings.Add("No known loader for this authoritative catalog");
                }
                // Has loader, has consumers → check if it's actually utilized
                else if (cat.ConsumerSystems.Count > 0)
                {
                    cat.Classification = ContentClassification.GAMEPLAY_CONSUMED;
                }
            }
        }

        // ── Verify Consumers In Source ─────────────────────────────

        /// <summary>
        /// Cross-references consumer claims against actual source code.
        /// Downgrades catalogs whose consumer claims are only from the
        /// scanner itself (name inference) with no source evidence.
        /// </summary>
        private void VerifyConsumersInSource()
        {
            if (!Directory.Exists(_coreDir)) return;

            // Build a cache of all source text for fast lookup
            string allSourceText = string.Empty;
            try
            {
                var coreFiles = Directory.GetFiles(_coreDir, "*.cs", SearchOption.AllDirectories);
                var srcDir = Path.Combine(_repoRoot, "src");
                var srcFiles = Directory.Exists(srcDir)
                    ? Directory.GetFiles(srcDir, "*.cs", SearchOption.AllDirectories)
                    : Array.Empty<string>();

                var sb = new System.Text.StringBuilder();
                foreach (var f in coreFiles)
                {
                    if (f.Contains("ContentUtilization")) continue; // Skip scanner itself
                    try { sb.AppendLine(File.ReadAllText(f)); } catch { /* cleanup: skip unreadable core files */ }
                }
                foreach (var f in srcFiles)
                {
                    if (f.Contains("ContentUtilization")) continue; // Skip scanner itself
                    try { sb.AppendLine(File.ReadAllText(f)); } catch { /* cleanup: skip unreadable src files */ }
                }
                allSourceText = sb.ToString();
            }
            catch (Exception ex)
            {
                _log?.Warn($"[VerifyConsumers] Failed to read source: {ex.Message}");
                return;
            }

            foreach (var cat in _graph.Catalogs)
            {
                // Skip already non-gameplay catalogs
                if (cat.Classification != ContentClassification.GAMEPLAY_CONSUMED)
                    continue;

                string fileName = Path.GetFileName(cat.Path);

                // Check if the JSON filename appears in actual source code
                bool foundInSource = allSourceText.Contains(fileName);

                // Also check the base name (without .json) for loader references
                string baseName = Path.GetFileNameWithoutExtension(cat.Path);
                bool baseNameInSource = allSourceText.Contains(baseName);

                if (!foundInSource && !baseNameInSource)
                {
                    // This catalog has zero source code evidence.
                    // The consumer claims are from name-inference heuristics only.
                    cat.Classification = ContentClassification.OPTIONAL;
                    cat.ConsumerSystems.Clear();
                    cat.Findings.Add("VERIFIED: No source code references found. Consumer claims were scanner name-inference only. Downgraded from GAMEPLAY_CONSUMED to OPTIONAL.");
                    cat.ExemptionId = "exempt_no_source_evidence";
                }
            }
        }

        // ── Detect Disconnects ──────────────────────────────────────

        private void DetectDisconnects()
        {
            foreach (var cat in _graph.Catalogs)
            {
                // Skip properly classified catalogs
                if (cat.Classification == ContentClassification.GAMEPLAY_CONSUMED
                    || cat.Classification == ContentClassification.CODEX_ONLY
                    || cat.Classification == ContentClassification.OPTIONAL
                    || cat.Classification == ContentClassification.TEST_ONLY)
                    continue;

                // Catalog with loader but no consumer
                if (!string.IsNullOrEmpty(cat.Loader) && cat.ConsumerSystems.Count == 0)
                {
                    _graph.Disconnects.Add(new DisconnectFinding
                    {
                        Catalog = cat.Path,
                        Category = "REGISTERED_NOT_QUERIED",
                        LastStage = cat.MaxStage,
                        MissingLink = "No production runtime consumer found",
                        Details = $"Loader: {cat.Loader}"
                    });
                }
                // Catalog with no loader
                else if (string.IsNullOrEmpty(cat.Loader))
                {
                    _graph.Disconnects.Add(new DisconnectFinding
                    {
                        Catalog = cat.Path,
                        Category = "NO_LOADER",
                        LastStage = UtilizationStage.DISCOVERED,
                        MissingLink = "No loader registered",
                        Details = "File exists but has no known loader"
                    });
                }
            }
        }
    }
}
