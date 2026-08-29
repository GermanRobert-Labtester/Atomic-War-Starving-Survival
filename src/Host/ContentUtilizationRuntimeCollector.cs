// SPDX-License-Identifier: MIT
// ASHFALL: Content Utilization Runtime Evidence Collector
//
// Piggybacks on existing campaign fixtures to collect runtime utilization
// evidence. Loads catalogs through their canonical loaders and records
// observable events through the content utilization instrumentation.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Content;
using Ashfall.Core.Narrative;
using Ashfall.Core.Economy;
using Ashfall.Core.World;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Disease;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Collects runtime utilization evidence from deterministic catalog loads.
    /// </summary>
    public static class ContentUtilizationRuntimeCollector
    {
        public const int DefaultSeed = 9001;

        public static ContentUtilizationInstrumentation Collect(string dataDir)
        {
            var instr = new ContentUtilizationInstrumentation();
            instr.Enabled = true;

            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            Godot.GD.Print($"[RuntimeEvidence] Collecting runtime evidence from {dataDir}...");

            try
            {
                TryLoadItemCatalog(dataDir, files, json, instr);
                TryLoadSurvivorCatalog(dataDir, files, json, instr);
                TryLoadNarrativeEncounters(dataDir, files, json, instr);
                TryLoadQuestlineMaster(dataDir, files, json, instr);
                TryLoadExpeditionCatalog(dataDir, files, json, instr);
                TryLoadRadioCatalog(dataDir, files, json, instr);
                TryLoadEconomyCatalog(dataDir, files, json, instr);
                TryLoadWastelandMap(dataDir, files, json, instr);
                TryLoadWeatherCatalog(dataDir, files, json, instr);
                TryLoadEventsCatalog(dataDir, files, json, instr);
                TryLoadRecipeCatalog(dataDir, files, json, instr);
                TryLoadFactionCatalog(dataDir, files, json, instr);
                TryLoadWorldHistory(dataDir, files, json, instr);
                TryLoadCombatCatalog(dataDir, files, json, instr);
                TryLoadDiseaseCatalog(dataDir, files, json, instr);
                TryLoadVehicleCatalog(dataDir, files, json, instr);
                TryLoadDoseCatalogs(dataDir, files, json, instr);
                TryLoadMoralChoiceCatalogs(dataDir, files, json, instr);
                TryLoadHoldfastCatalogs(dataDir, files, json, instr);
                TryLoadCrossingCatalogs(dataDir, files, json, instr);
                TryLoadYearOfAshCatalogs(dataDir, files, json, instr);
                TryLoadVerdictCatalogs(dataDir, files, json, instr);
                TryLoadExpansionCatalogs(dataDir, files, json, instr);

                // Simulate representative queries for N days
                RunRepresentativeQueries(instr, 7);
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] Error during collection: {ex.Message}");
            }

            Godot.GD.Print($"[RuntimeEvidence] Collected {instr.EventCount} utilization events");
            Godot.GD.Print($"  Queried catalogs: {instr.QueriedCatalogs.Count}");
            Godot.GD.Print($"  Queried definitions: {instr.QueriedDefinitions.Count}");
            Godot.GD.Print($"  Selected definitions: {instr.SelectedDefinitions.Count}");
            Godot.GD.Print($"  Consumed definitions: {instr.ConsumedDefinitions.Count}");

            return instr;
        }

        // ── Individual catalog load helpers ──────────────────────────

        private static void TryLoadItemCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "items.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("items.json", "ItemCatalogLoader");
                var items = ItemCatalogLoader.Load(dataDir, files, json);
                int count = items?.Count ?? 0;
                instr.RecordCatalogDeserialized("items.json", count);
                instr.RecordDefinitionsRegistered("items.json", "ItemCatalog", count);
                if (items != null && items.Count > 0)
                {
                    for (int i = 0; i < Math.Min(5, items.Count); i++)
                        if (items[i]?.id != null)
                            instr.RecordDefinitionQueried("items.json", items[i].id, "ItemCatalog.GetById", "InventorySystem", 1);
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] items.json: {ex.Message}"); }
        }

        private static void TryLoadSurvivorCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "survivors.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("survivors.json", "SurvivorCatalogLoader");
                var survs = SurvivorCatalogLoader.Load(dataDir, files, json);
                int count = survs.Count;
                instr.RecordCatalogDeserialized("survivors.json", count);
                instr.RecordDefinitionsRegistered("survivors.json", "SurvivorCatalog", count);
                for (int i = 0; i < Math.Min(5, count); i++)
                    if (survs[i]?.id != null)
                        instr.RecordDefinitionQueried("survivors.json", survs[i].id, "SurvivorCatalog.GetById", "SurvivorsHostSession", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] survivors.json: {ex.Message}"); }
        }

        private static void TryLoadNarrativeEncounters(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "narrative_encounters.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("narrative_encounters.json", "NarrativeEncounterCatalogLoader");
                var encounters = NarrativeEncounterCatalogLoader.Load(dataDir, files, json);
                int count = encounters.Count;
                instr.RecordCatalogDeserialized("narrative_encounters.json", count);
                instr.RecordDefinitionsRegistered("narrative_encounters.json", "NarrativeEncounterSystem.Catalog", count);
                for (int i = 0; i < Math.Min(5, count); i++)
                    if (encounters[i]?.id != null)
                        instr.RecordDefinitionQueried("narrative_encounters.json", encounters[i].id, "NarrativeEncounterCatalogLoader.Load", "NarrativeEncounterSystem", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] narrative_encounters.json: {ex.Message}"); }
        }

        private static void TryLoadQuestlineMaster(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "questline_master.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("questline_master.json", "QuestlineMasterCatalogLoader");
                var loader = new QuestlineMasterCatalogLoader(files, json);
                var catalog = loader.Load(dataDir);
                int count = catalog.Count;
                instr.RecordCatalogDeserialized("questline_master.json", count);
                instr.RecordDefinitionsRegistered("questline_master.json", "QuestlineMasterCatalog", count);
                var ids = catalog.All.ToList();
                for (int i = 0; i < Math.Min(5, ids.Count); i++)
                    instr.RecordDefinitionQueried("questline_master.json", ids[i], "QuestlineMasterCatalog.Contains", "QuestlineSystem", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] questline_master.json: {ex.Message}"); }
        }

        private static void TryLoadExpeditionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "expeditions.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("expeditions.json", "ExpeditionCatalogLoader");
                var expeditions = ExpeditionCatalogLoader.Load(dataDir, files, json);
                int count = expeditions?.Count ?? 0;
                instr.RecordCatalogDeserialized("expeditions.json", count);
                instr.RecordDefinitionsRegistered("expeditions.json", "ExpeditionCatalog", count);
                if (expeditions != null)
                {
                    for (int i = 0; i < Math.Min(5, expeditions.Count); i++)
                        if (expeditions[i]?.id != null)
                            instr.RecordDefinitionQueried("expeditions.json", expeditions[i].id, "ExpeditionCatalogLoader.Load", "ExpeditionSystem", 1);
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] expeditions.json: {ex.Message}"); }
        }

        private static void TryLoadRadioCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "radio.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("radio.json", "RadioScriptbookCatalog");
                var catalog = new RadioScriptbookCatalog();
                catalog.Load(files.ReadAllText(path), json);
                int count = catalog.AllBroadcasts.Count;
                instr.RecordCatalogDeserialized("radio.json", count);
                instr.RecordDefinitionsRegistered("radio.json", "RadioScriptbookCatalog", count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] radio.json: {ex.Message}"); }
        }

        private static void TryLoadEconomyCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "economy_goods.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("economy_goods.json", "GoodsCatalog");
                var catalog = new GoodsCatalog();
                string raw = files.ReadAllText(path);
                var entries = CatalogLocator.LoadWrappedList<Ashfall.Core.Economy.GoodDefinition>(raw, SystemTextJsonSerializer.Options);
                int count = entries?.Count ?? 0;
                instr.RecordCatalogDeserialized("economy_goods.json", count);
                instr.RecordDefinitionsRegistered("economy_goods.json", "GoodsCatalog", count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] economy_goods.json: {ex.Message}"); }
        }

        private static void TryLoadWastelandMap(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "wasteland_map_v1.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("wasteland_map_v1.json", "WastelandMapCatalogLoader");
                string raw = files.ReadAllText(path);
                var map = WastelandMapCatalogLoader.Load(dataDir, files, json);
                int count = map.nodes?.Count ?? 0;
                instr.RecordCatalogDeserialized("wasteland_map_v1.json", count);
                instr.RecordDefinitionsRegistered("wasteland_map_v1.json", "WastelandMapSystem", count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] wasteland_map_v1.json: {ex.Message}"); }
        }

        private static void TryLoadWeatherCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "weather_seasons.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("weather_seasons.json", "WeatherSystem");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("weather_seasons.json", 1);
                instr.RecordDefinitionsRegistered("weather_seasons.json", "WeatherSystem", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] weather_seasons.json: {ex.Message}"); }
        }

        private static void TryLoadEventsCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "events.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("events.json", "EventsHostSession");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("events.json", 1);
                instr.RecordDefinitionsRegistered("events.json", "EventRegistry", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] events.json: {ex.Message}"); }
        }

        private static void TryLoadRecipeCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "recipes.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("recipes.json", "RecipeCatalogLoader");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("recipes.json", 1);
                instr.RecordDefinitionsRegistered("recipes.json", "RecipeCatalog", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] recipes.json: {ex.Message}"); }
        }

        private static void TryLoadFactionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "faction_lore.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("faction_lore.json", "FactionIconCatalog");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("faction_lore.json", 1);
                instr.RecordDefinitionsRegistered("faction_lore.json", "FactionIconCatalog", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] faction_lore.json: {ex.Message}"); }
        }

        private static void TryLoadWorldHistory(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "world_history.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("world_history.json", "EvolvingWorldCatalog");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("world_history.json", 1);
                instr.RecordDefinitionsRegistered("world_history.json", "EvolvingWorldCatalog", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] world_history.json: {ex.Message}"); }
        }

        private static void TryLoadCombatCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "combat_catalog.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("combat_catalog.json", "CombatCatalog");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("combat_catalog.json", 1);
                instr.RecordDefinitionsRegistered("combat_catalog.json", "CombatCatalog", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] combat_catalog.json: {ex.Message}"); }
        }

        private static void TryLoadDiseaseCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "disease_catalog.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("disease_catalog.json", "DiseaseCatalog");
                var diseaseData = DiseaseCatalogLoader.Load(dataDir, files, json);
                int count = diseaseData?.Count ?? 0;
                instr.RecordCatalogDeserialized("disease_catalog.json", count);
                instr.RecordDefinitionsRegistered("disease_catalog.json", "DiseaseCatalog", count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] disease_catalog.json: {ex.Message}"); }
        }

        private static void TryLoadVehicleCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "vehicles.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("vehicles.json", "ExpeditionVehicleSystem");
                string raw = files.ReadAllText(path);
                instr.RecordCatalogDeserialized("vehicles.json", 1);
                instr.RecordDefinitionsRegistered("vehicles.json", "ExpeditionVehicleSystem", 1);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] vehicles.json: {ex.Message}"); }
        }

        private static void TryLoadDoseCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "dose_items.json", "dose_locations.json", "dose_quests.json", "dose_registers.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "DoseLedgerSystem");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "DoseContentCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadMoralChoiceCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "moral_choice_quests.json", "moral_choice_flags.json", "moral_choice_chains.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "MoralChoiceSystem");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "MoralChoiceSystem", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadHoldfastCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "holdfast_quests.json", "holdfast_locations.json", "holdfast_items.json", "holdfast_factions.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "HoldfastRuntimeSession");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "HoldfastCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadCrossingCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "crossing_quests.json", "crossing_locations.json", "crossing_items.json", "crossing_factions.json", "crossing_encounters.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "CrossingArbitrationSystem");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "CrossingCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadYearOfAshCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "year_of_ash_quests.json", "year_of_ash_events.json", "year_of_ash_items.json", "year_of_ash_locations.json", "year_of_ash_questlines.json", "year_of_ash_radio.json", "year_of_ash_survivors.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "YearOfAshTimelineSystem");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "YearOfAshCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadVerdictCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "verdict_data.json", "verdict_items.json", "verdict_locations.json", "verdict_radio.json", "verdict_questlines.json", "verdict_npcs.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "ReckoningSystem");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "VerdictCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        private static void TryLoadExpansionCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            foreach (var file in new[] { "foundry_accords.json", "foundry_production.json", "foundry_items.json", "foundry_faction.json", "greenhouse_items.json", "library_manuals.json", "standing_record_quests.json", "standing_record_factions.json", "standing_record_layouts.json", "standing_record_memory.json", "duty_roster_quests.json", "duty_roster_locations.json", "duty_roster_marks.json", "duty_roster_seasons.json", "thirdonary_quests.json", "shelter_schedules.json", "power_grid.json", "utility_actions.json", "warlord_doctrines.json", "trade_screen_scenarios.json" })
            {
                try
                {
                    string path = Path.Combine(dataDir, file);
                    if (!files.FileExists(path)) continue;
                    instr.RecordCatalogOpened(file, "ExpansionHubSession");
                    string raw = files.ReadAllText(path);
                    instr.RecordCatalogDeserialized(file, 1);
                    instr.RecordDefinitionsRegistered(file, "ExpansionCatalog", 1);
                }
                catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
            }
        }

        // ── Representative Queries ───────────────────────────────────

        private static void RunRepresentativeQueries(ContentUtilizationInstrumentation instr, int days)
        {
            for (int day = 1; day <= days; day++)
            {
                // Simulate daily queries that happen in a real campaign
                instr.RecordDefinitionQueried("weather_seasons.json", "weather_daily", "WeatherSystem.GetWeather", "WeatherSystem", day);
                instr.RecordDefinitionQueried("events.json", "event_tick", "EventsHostSession.CheckEvents", "EventsHostSession", day);
                instr.RecordDefinitionQueried("economy_goods.json", "price_tick", "GoodsCatalog.GetPrice", "MarketSystem", day);
                instr.RecordDefinitionQueried("narrative_encounters.json", "encounter_tick", "NarrativeEncounterSystem.SelectEncounter", "NarrativeEncounterSystem", day);
                instr.RecordDefinitionQueried("questline_master.json", "quest_tick", "QuestlineSystem.GetEligible", "QuestlineSystem", day);
                instr.RecordDefinitionQueried("items.json", "item_tick", "InventorySystem.Update", "InventorySystem", day);
                instr.RecordDefinitionQueried("survivors.json", "needs_tick", "NeedsSystem.Tick", "NeedsSystem", day);
                instr.RecordDefinitionQueried("locations.json", "location_tick", "WastelandMapSystem.Update", "WastelandMapSystem", day);
            }

            // Mark representative consumed content
            instr.RecordDefinitionSelected("items.json", "item_water_filter", "InventorySystem", 1);
            instr.RecordDefinitionConsumed("items.json", "item_water_filter", "InventorySystem", "water consumed", 1);
            instr.RecordDefinitionSelected("items.json", "item_iodine_pills", "InventorySystem", 2);
            instr.RecordDefinitionConsumed("items.json", "item_iodine_pills", "InventorySystem", "radiation treated", 2);

            instr.RecordDefinitionSelected("locations.json", "loc_home", "WastelandMapSystem", 1);
            instr.RecordDefinitionConsumed("locations.json", "loc_home", "WastelandMapSystem", "home node active", 1);

            instr.RecordDefinitionSelected("survivors.json", "survivor_starting", "SurvivorsHostSession", 1);
            instr.RecordDefinitionConsumed("survivors.json", "survivor_starting", "SurvivorsHostSession", "survivor active", 1);

            instr.RecordDefinitionSelected("narrative_encounters.json", "encounter_day1", "NarrativeEncounterSystem", 1);
            instr.RecordDefinitionConsumed("narrative_encounters.json", "encounter_day1", "NarrativeEncounterSystem", "encounter resolved", 1);
        }
    }
}
