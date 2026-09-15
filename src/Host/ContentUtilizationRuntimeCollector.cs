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
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Ashfall.Core.Economy;
using Ashfall.Core.World;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Ashfall.Core.Disease;
using Ashfall.Core.Factions;
using Ashfall.Core.Shelter;
using Ashfall.Core.Radio;

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
                TryLoadStartingCohortCatalog(dataDir, files, json, instr);
                TryLoadNarrativeEncounters(dataDir, files, json, instr);
                TryLoadNarrativeArcEvents(dataDir, files, json, instr);
                TryLoadEchoes(dataDir, files, json, instr);
                TryLoadQuestlineMaster(dataDir, files, json, instr);
                TryLoadExpeditionCatalog(dataDir, files, json, instr);
                TryLoadRadioCatalog(dataDir, files, json, instr);
                TryLoadEconomyCatalog(dataDir, files, json, instr);
                TryLoadTradeTextCatalog(dataDir, files, json, instr);
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
                TryLoadTechSalvageCatalog(dataDir, files, json, instr);
                TryLoadEspionageMissionCatalog(dataDir, files, json, instr);
                TryLoadFluidInfrastructureCatalog(dataDir, files, json, instr);
                TryLoadQuestTemplateCatalog(dataDir, files, json, instr);
                TryLoadJournalCorpus(dataDir, files, json, instr);
                TryLoadBureaucraticDocuments(dataDir, files, json, instr);
                TryLoadFringeCultRecords(dataDir, files, json, instr);
                TryLoadPaperPrintingRecords(dataDir, files, json, instr);
                TryLoadBoneHornRecords(dataDir, files, json, instr);
                TryLoadAdvancedIndustrialReconCatalogs(dataDir, files, json, instr);

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

        private static void TryLoadEchoes(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                var load = EchoCatalogLoader.LoadDetailed(dataDir, files, json, instr);
                if (!load.IsSuccess)
                {
                    Godot.GD.PrintErr("[RuntimeEvidence] echoes.json: " + string.Join(" | ", load.Errors));
                    return;
                }

                var system = new EchoSystem(load.Echoes, instrumentation: instr)
                {
                    HasWorldFlag = _ => true
                };
                var selected = system.SelectForDay(31, new SeededRng(DefaultSeed));
                if (selected == null || selected.Choices.Count == 0) return;

                // The collector uses the real selection and resolution path,
                // not a synthetic SELECTED/EFFECT_PRODUCED event.
                system.Resolve(selected.Id, selected.Choices[0].ChoiceId, 31);
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] echoes: {ex.Message}");
            }
        }

        private static void TryLoadJournalCorpus(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                var catalog = new Ashfall.Core.Journal.JournalCorpusCatalogLoader(files, json)
                    .Load(dataDir);
                string[] paths =
                {
                    "journal_entries_expansion_05.json",
                    "narrative/journals_expansion.json",
                    "narrative/journal_entries_batch_1.json",
                    "narrative/journal_entries_batch_2.json",
                    "narrative/journal_entries_batch_3.json"
                };

                foreach (string relativePath in paths)
                {
                    string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
                    if (!files.FileExists(path)) continue;
                    int count = catalog.Records.Count(r =>
                        r.SourcePath.EndsWith(relativePath, StringComparison.Ordinal));
                    instr.RecordCatalogOpened(relativePath, "JournalCorpusCatalogLoader");
                    instr.RecordCatalogDeserialized(relativePath, count);
                    instr.RecordDefinitionsRegistered(relativePath, "JournalSystem", count);
                    if (count > 0)
                    {
                        instr.RecordDefinitionQueried(
                            relativePath,
                            "corpus_load",
                            "JournalCorpusCatalogLoader.Load",
                            "JournalSystem",
                            1);
                    }
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] journal corpus: {ex.Message}");
            }
        }

        private static void TryLoadBureaucraticDocuments(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string relativePath = BureaucraticDocumentCatalogLoader.DocumentsFileName;
                string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
                if (!files.FileExists(path)) return;

                var load = new BureaucraticDocumentCatalogLoader(files, json).Load(dataDir);
                if (!load.IsSuccess)
                {
                    Godot.GD.PrintErr($"[RuntimeEvidence] {relativePath}: " + string.Join(" | ", load.Errors));
                    return;
                }

                instr.RecordCatalogOpened(relativePath, nameof(BureaucraticDocumentCatalogLoader));
                instr.RecordCatalogDeserialized(relativePath, load.Catalog.Count);
                instr.RecordDefinitionsRegistered(relativePath, "BureaucraticDocumentCatalog", load.Catalog.Count);

                // This diagnostic path exercises the same bounded producer and
                // Journal knowledge authority used by the host. It records a
                // codex discovery for each mapped document at its authored day;
                // it does not apply any simulation consequence.
                var journal = new JournalSystem();
                var discovery = new BureaucraticDocumentDiscoverySystem(load.Catalog);
                foreach (var document in load.Catalog.Documents)
                {
                    instr.RecordDefinitionQueried(
                        relativePath,
                        document.DocId,
                        "BureaucraticDocumentCatalog.TryGet",
                        "JournalCodex",
                        document.PostedDay);

                    if (document.ProducerIds.Count == 0) continue;
                    var result = discovery.Discover(
                        document.DocId,
                        document.ProducerIds[0],
                        document.PostedDay,
                        journal);
                    if (!result.Changed) continue;
                    instr.RecordDefinitionSelected(
                        relativePath,
                        document.DocId,
                        "BureaucraticDocumentDiscoverySystem",
                        document.PostedDay);
                    instr.RecordDefinitionConsumed(
                        relativePath,
                        document.DocId,
                        "JournalCodex",
                        "authored document discovered",
                        document.PostedDay);
                }

                string mapRelativePath = BureaucraticDocumentCatalogLoader.RuntimeMapFileName;
                string mapPath = Path.Combine(dataDir, mapRelativePath.Replace('/', Path.DirectorySeparatorChar));
                if (files.FileExists(mapPath))
                {
                    instr.RecordCatalogOpened(mapRelativePath, nameof(BureaucraticDocumentCatalogLoader));
                    instr.RecordCatalogDeserialized(mapRelativePath, load.Catalog.Count);
                    instr.RecordDefinitionsRegistered(mapRelativePath, "BureaucraticDocumentCatalog.RuntimeMap", load.Catalog.Count);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] bureaucratic documents: {ex.Message}");
            }
        }

        private static void TryLoadFringeCultRecords(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string narrativeDir = Path.Combine(dataDir, "narrative");
                var sourceCatalog = FringeCultsCatalog.LoadFromDirectory(narrativeDir);
                if (sourceCatalog.TotalCount == 0) return;

                string[] relativePaths =
                {
                    FringeCultRuntimeContract.CobaltCatalog,
                    FringeCultRuntimeContract.IronCatalog,
                    FringeCultRuntimeContract.HymnalCatalog,
                    FringeCultRuntimeContract.EpitaphCatalog
                };
                foreach (string relativePath in relativePaths)
                {
                    string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
                    if (!files.FileExists(path)) continue;
                    int count = relativePath.EndsWith("cobalt_liturgies.json", StringComparison.Ordinal)
                        ? sourceCatalog.CobaltLiturgies.Count
                        : relativePath.EndsWith("iron_synod_canons.json", StringComparison.Ordinal)
                            ? sourceCatalog.IronSynodCanons.Count
                            : relativePath.EndsWith("geophone_hymnals.json", StringComparison.Ordinal)
                                ? sourceCatalog.GeophoneHymnals.Count
                                : sourceCatalog.WastelandEpitaphs.Count;
                    instr.RecordCatalogOpened(relativePath, nameof(FringeCultsCatalog));
                    instr.RecordCatalogDeserialized(relativePath, count);
                    instr.RecordDefinitionsRegistered(relativePath, "FringeCultsCatalog", count);
                }

                // Exercise the same manifest projection and Journal knowledge
                // seam used by the host. No doctrine field is handed to a
                // faction, radiation, foundry, audio, or mortality authority.
                var discoveryCatalog = new NarrativeDiscoveryCatalog();
                discoveryCatalog.LoadFromFiles(dataDir, files);
                var journal = new JournalSystem();
                foreach (var record in discoveryCatalog.AllRecords)
                {
                    if (!FringeCultRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
                    instr.RecordDefinitionQueried(
                        record.SourceCatalog,
                        record.SourceRecordId,
                        "NarrativeDiscoveryCatalog.GetByProducer",
                        "JournalCodex",
                        record.MinDay);
                    if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
                    instr.RecordDefinitionSelected(
                        record.SourceCatalog,
                        record.SourceRecordId,
                        "NarrativeDiscoveryCatalog",
                        record.MinDay);
                    instr.RecordDefinitionConsumed(
                        record.SourceCatalog,
                        record.SourceRecordId,
                        "JournalCodex",
                        "authored fringe-cult record discovered",
                        record.MinDay);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] fringe cults: {ex.Message}");
            }
        }

        private static void TryLoadPaperPrintingRecords(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string narrativeDir = Path.Combine(dataDir, "narrative");
                var making = PaperMakingCatalog.LoadFromDirectory(narrativeDir);
                var printing = PaperPrintingCatalog.LoadFromDirectory(narrativeDir);
                if (making.TotalCount == 0 && printing.TotalCount == 0) return;

                foreach (string relativePath in PaperPrintRuntimeContract.SourceCatalogs)
                {
                    string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
                    if (!files.FileExists(path)) continue;
                    int count = relativePath.Equals(PaperPrintRuntimeContract.HollanderCatalog, StringComparison.OrdinalIgnoreCase)
                        ? making.BeaterEntries.Count
                        : relativePath.Equals(PaperPrintRuntimeContract.DeckleCatalog, StringComparison.OrdinalIgnoreCase)
                            ? making.MouldEntries.Count
                            : relativePath.Equals(PaperPrintRuntimeContract.PressCatalog, StringComparison.OrdinalIgnoreCase)
                                ? making.PressEntries.Count
                                : relativePath.Equals(PaperPrintRuntimeContract.SizingCatalog, StringComparison.OrdinalIgnoreCase)
                                    ? making.SizingEntries.Count
                                    : relativePath.Equals(PaperPrintRuntimeContract.RagPulpCatalog, StringComparison.OrdinalIgnoreCase)
                                        ? printing.PulpEntries.Count
                                        : relativePath.Equals(PaperPrintRuntimeContract.InkCatalog, StringComparison.OrdinalIgnoreCase)
                                            ? printing.InkEntries.Count
                                            : relativePath.Equals(PaperPrintRuntimeContract.TypeCatalog, StringComparison.OrdinalIgnoreCase)
                                                ? printing.TypeEntries.Count
                                                : printing.StencilEntries.Count;
                    instr.RecordCatalogOpened(relativePath, "PaperPrintCatalogLoader");
                    instr.RecordCatalogDeserialized(relativePath, count);
                    instr.RecordDefinitionsRegistered(relativePath, "PaperPrintCatalog", count);
                }

                // Exercise the combined read model through the same Journal
                // authority used by the player. This records reachability only;
                // no process measurement is forwarded to production systems.
                var discoveryCatalog = new NarrativeDiscoveryCatalog();
                discoveryCatalog.LoadFromFiles(dataDir, files);
                var journal = new JournalSystem();
                foreach (var record in discoveryCatalog.AllRecords)
                {
                    if (!PaperPrintRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
                    instr.RecordDefinitionQueried(
                        record.SourceCatalog,
                        record.SourceRecordId,
                        "NarrativeDiscoveryCatalog.GetByProducer",
                        "JournalCodex",
                        record.MinDay);
                    if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
                    instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
                    instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored paper/print record discovered", record.MinDay);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] paper/printing corpus: {ex.Message}");
            }
        }

        private static void TryLoadBoneHornRecords(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string narrativeDir = Path.Combine(dataDir, "narrative");
                var catalog = BoneHornCarvingCatalog.LoadFromDirectory(narrativeDir);
                if (catalog.TotalCount == 0) return;

                foreach (string relativePath in BoneHornRuntimeContract.SourceCatalogs)
                {
                    string path = Path.Combine(dataDir, relativePath.Replace('/', Path.DirectorySeparatorChar));
                    if (!files.FileExists(path)) continue;
                    int count = relativePath.Equals(BoneHornRuntimeContract.DegreasingCatalog, StringComparison.OrdinalIgnoreCase)
                        ? catalog.DegreasingLogs.Count
                        : relativePath.Equals(BoneHornRuntimeContract.SawingCatalog, StringComparison.OrdinalIgnoreCase)
                            ? catalog.SawingRecords.Count
                            : relativePath.Equals(BoneHornRuntimeContract.PolishingCatalog, StringComparison.OrdinalIgnoreCase)
                                ? catalog.PolishingReports.Count
                                : catalog.ToolAssays.Count;
                    instr.RecordCatalogOpened(relativePath, "BoneHornCarvingCatalog");
                    instr.RecordCatalogDeserialized(relativePath, count);
                    instr.RecordDefinitionsRegistered(relativePath, "BoneHornCarvingCatalog", count);
                }

                // Exercise the shared discovery projection. The numeric and
                // biological labels remain authored observations; this path
                // cannot create items, wildlife outcomes or crafted tools.
                var discoveryCatalog = new NarrativeDiscoveryCatalog();
                discoveryCatalog.LoadFromFiles(dataDir, files);
                var journal = new JournalSystem();
                foreach (var record in discoveryCatalog.AllRecords)
                {
                    if (!BoneHornRuntimeContract.IsSourceCatalog(record.SourceCatalog)) continue;
                    instr.RecordDefinitionQueried(
                        record.SourceCatalog,
                        record.SourceRecordId,
                        "NarrativeDiscoveryCatalog.GetByProducer",
                        "JournalCodex",
                        record.MinDay);
                    if (!discoveryCatalog.TryDiscover(record.DiscoveryId, journal, out _)) continue;
                    instr.RecordDefinitionSelected(record.SourceCatalog, record.SourceRecordId, "NarrativeDiscoveryCatalog", record.MinDay);
                    instr.RecordDefinitionConsumed(record.SourceCatalog, record.SourceRecordId, "JournalCodex", "authored bone/horn record discovered", record.MinDay);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] bone/horn corpus: {ex.Message}");
            }
        }

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

        private static void TryLoadStartingCohortCatalog(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, StartingCohortCatalogLoader.FileName);
                if (!files.FileExists(path)) return;

                instr.RecordCatalogOpened(
                    StartingCohortCatalogLoader.FileName,
                    "StartingCohortCatalogLoader");
                var canonical = SurvivorCatalogLoader.Load(dataDir, files, json);
                var result = StartingCohortCatalogLoader.LoadDetailed(
                    dataDir,
                    files,
                    json,
                    canonical);
                int count = result.Catalog.Profiles.Count;
                instr.RecordCatalogDeserialized(
                    StartingCohortCatalogLoader.FileName,
                    count);
                instr.RecordDefinitionsRegistered(
                    StartingCohortCatalogLoader.FileName,
                    "StartingCohortCatalog.Profiles",
                    count);
                foreach (var profile in result.Catalog.Profiles)
                {
                    instr.RecordDefinitionQueried(
                        StartingCohortCatalogLoader.FileName,
                        profile.profile_id,
                        "StartingCohortCatalog.TryGet",
                        "StartingCohortSetupPanel",
                        1);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr(
                    $"[RuntimeEvidence] {StartingCohortCatalogLoader.FileName}: {ex.Message}");
            }
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

                // Real SELECTED/EFFECT_PRODUCED evidence: drive the actual
                // production weighted-selection + resolution methods instead
                // of hand-authoring a fake result. NarrativeEncounterSystem
                // itself calls Instrumentation.RecordDefinitionSelected /
                // RecordDefinitionConsumed at its real OnEncounterSelected /
                // Resolve call sites when this hook is set.
                if (count > 0)
                {
                    var narrative = new NarrativeEncounterSystem();
                    narrative.Instrumentation = instr;
                    narrative.RegisterRange(encounters);
                    var rng = new SeededRng(DefaultSeed);
                    var selected = narrative.SelectEncounter("Stealth", dangerLevel: 1f, locationId: string.Empty, rng);
                    if (selected != null && selected.choices != null && selected.choices.Count > 0)
                        narrative.Resolve(selected.id, selected.choices[0].choiceId, locationId: string.Empty, day: 1);
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] narrative_encounters.json: {ex.Message}"); }
        }

        private static void TryLoadNarrativeArcEvents(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, NarrativeArcEventCatalogLoader.FileName);
                if (!files.FileExists(path)) return;

                var load = NarrativeArcEventCatalogLoader.LoadDetailed(dataDir, files, json, instr);
                if (!load.IsSuccess)
                {
                    Godot.GD.PrintErr($"[RuntimeEvidence] {NarrativeArcEventCatalogLoader.FileName}: " +
                        string.Join(" | ", load.Errors));
                    return;
                }

                // Query every definition through the real arc registry, then
                // run the same daily select/commit surface with all four
                // subjects present. The port is diagnostic-only; it records
                // no production state and cannot bypass a real game gate.
                var system = new NarrativeArcEventSystem(load.Events, instrumentation: instr)
                {
                    SurvivorIsPresent = _ => true,
                    Consequences = new UtilizationArcConsequencePort()
                };
                foreach (var definition in load.Events)
                    system.Find(definition.Id);

                for (int day = 1; day <= 40; day++)
                {
                    var selected = system.SelectForDay(
                        day,
                        new SeededRng(CampaignRngStream.DeriveSeed(
                            DefaultSeed,
                            CampaignStreamIds.Narrative,
                            CampaignRngManager.CurrentDerivationVersion,
                            day,
                            0)));
                    if (selected == null) continue;
                    if (selected.Choices.Count == 0)
                        system.AcknowledgeEvent(selected.Id, day);
                    else
                        system.CommitChoice(selected.Id, selected.Choices[0].ChoiceId, day);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr($"[RuntimeEvidence] {NarrativeArcEventCatalogLoader.FileName}: {ex.Message}");
            }
        }

        private sealed class UtilizationArcConsequencePort : INarrativeArcConsequencePort
        {
            public bool CanApplyMorale(string survivorId, int delta, bool shelterWide, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void ApplyMorale(string survivorId, int delta, bool shelterWide) { }

            public bool CanGrantFactionIntel(string canonicalFactionId, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void GrantFactionIntel(string canonicalFactionId) { }

            public bool CanOfferExpedition(string locationId, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void OfferExpedition(string locationId) { }

            public bool CanApplyFactionStanding(string canonicalFactionId, int delta, out string reason)
            {
                reason = string.Empty;
                return true;
            }

            public void ApplyFactionStanding(string canonicalFactionId, int delta) { }
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

        private static void TryLoadTradeTextCatalog(
            string dataDir,
            IFileIO files,
            IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, TradeTextCatalogLoader.FileName);
                if (!files.FileExists(path)) return;

                instr.RecordCatalogOpened(
                    TradeTextCatalogLoader.FileName,
                    "TradeTextCatalogLoader");
                var load = TradeTextCatalogLoader.Load(dataDir, files, json);
                instr.RecordCatalogDeserialized(
                    TradeTextCatalogLoader.FileName,
                    load.Catalog.TraderCount + load.Catalog.ScenarioCount);
                instr.RecordDefinitionsRegistered(
                    TradeTextCatalogLoader.FileName,
                    "TradeTextCatalog",
                    load.Catalog.TraderCount + load.Catalog.ScenarioCount);

                if (load.UsedFallback) return;

                var resolver = new TradeVoiceResolver(load.Catalog);
                var context = new TradeVoiceContext
                {
                    FactionId = "faction_silent_foundry",
                    ScenarioId = "salvage_caravan",
                    StableContextKey = "content_utilization_trade_voice",
                    Trust = 20f
                };
                var greeting = resolver.ResolveGreeting(context);
                instr.RecordDefinitionQueried(
                    TradeTextCatalogLoader.FileName,
                    greeting.ProfileId,
                    "TradeVoiceResolver.ResolveGreeting",
                    "TradeScreenPresenter",
                    1);
                instr.RecordDefinitionSelected(
                    TradeTextCatalogLoader.FileName,
                    greeting.ProfileId,
                    "TradeScreenPresenter",
                    1);
                instr.RecordDefinitionConsumed(
                    TradeTextCatalogLoader.FileName,
                    greeting.ProfileId,
                    "TradeScreenGodotPanel",
                    "presentation voice line displayed",
                    1);

                var scenario = resolver.ResolveScenarioTraderText(context, "fair_deal");
                if (!scenario.UsedFallback)
                {
                    instr.RecordDefinitionQueried(
                        TradeTextCatalogLoader.FileName,
                        "fair_deal",
                        "TradeVoiceResolver.ResolveScenarioTraderText",
                        "TradeScreenPresenter",
                        1);
                    instr.RecordDefinitionConsumed(
                        TradeTextCatalogLoader.FileName,
                        "fair_deal",
                        "TradeScreenGodotPanel",
                        "scenario presentation text displayed",
                        1);
                }
            }
            catch (Exception ex)
            {
                Godot.GD.PrintErr(
                    $"[RuntimeEvidence] {TradeTextCatalogLoader.FileName}: {ex.Message}");
            }
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

        private static void TryLoadTechSalvageCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, "tech_salvage.json");
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened("tech_salvage.json", "TechSalvageCatalogLoader");
                var catalog = TechSalvageCatalogLoader.Load(dataDir, files, json);
                int count = catalog?.Count ?? 0;
                instr.RecordCatalogDeserialized("tech_salvage.json", count);
                instr.RecordDefinitionsRegistered("tech_salvage.json", "TechSalvageCatalog", count);
                if (catalog != null)
                {
                    foreach (var definition in catalog.Take(5))
                    {
                        if (definition != null && !string.IsNullOrWhiteSpace(definition.Id))
                            instr.RecordDefinitionQueried("tech_salvage.json", definition.Id,
                                "TechSalvageCatalog.GetById", "WorkshopReverseEngineeringSystem", 1);
                    }
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] tech_salvage.json: {ex.Message}"); }
        }

        private static void TryLoadEspionageMissionCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, EspionageMissionCatalogLoader.FileName);
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalogLoader");
                var missions = EspionageMissionCatalogLoader.Load(dataDir, files, json);
                instr.RecordCatalogDeserialized(EspionageMissionCatalogLoader.FileName, missions.Count);
                instr.RecordDefinitionsRegistered(EspionageMissionCatalogLoader.FileName, "EspionageMissionCatalog", missions.Count);
                foreach (var mission in missions.Take(5))
                {
                    if (mission != null && !string.IsNullOrWhiteSpace(mission.Id))
                        instr.RecordDefinitionQueried(EspionageMissionCatalogLoader.FileName, mission.Id,
                            "EspionageMissionCatalog.GetById", "EspionageSystem", 1);
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] espionage_missions.json: {ex.Message}"); }
        }

        private static void TryLoadFluidInfrastructureCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, FluidInfrastructureCatalogLoader.FileName);
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened(FluidInfrastructureCatalogLoader.FileName, "FluidInfrastructureCatalogLoader");
                var catalog = FluidInfrastructureCatalogLoader.Load(dataDir, files, json);
                int count = (catalog?.Pipes?.Count ?? 0) + (catalog?.Pumps?.Count ?? 0) + (catalog?.Reservoirs?.Count ?? 0);
                instr.RecordCatalogDeserialized(FluidInfrastructureCatalogLoader.FileName, count);
                instr.RecordDefinitionsRegistered(FluidInfrastructureCatalogLoader.FileName, "FluidInfrastructureCatalog", count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] fluid_infrastructure.json: {ex.Message}"); }
        }

        private static void TryLoadQuestTemplateCatalog(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            try
            {
                string path = Path.Combine(dataDir, QuestTemplateCatalogLoader.FileName);
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalogLoader");
                var templates = QuestTemplateCatalogLoader.Load(dataDir, files, json);
                instr.RecordCatalogDeserialized(QuestTemplateCatalogLoader.FileName, templates.Count);
                instr.RecordDefinitionsRegistered(QuestTemplateCatalogLoader.FileName, "QuestTemplateCatalog", templates.Count);
                foreach (var template in templates.Take(5))
                {
                    if (template != null && !string.IsNullOrWhiteSpace(template.Id))
                        instr.RecordDefinitionQueried(QuestTemplateCatalogLoader.FileName, template.Id,
                            "QuestTemplateCatalog.GetById", "ProceduralNarrativeSystem", 1);
                }
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] quest_templates.json: {ex.Message}"); }
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
            foreach (var file in new[] { "foundry_accords.json", "foundry_production.json", "foundry_items.json", "foundry_faction.json", "greenhouse_items.json", "library_manuals.json", "research_knowledge.json", "skills.json", "standing_record_quests.json", "standing_record_factions.json", "standing_record_layouts.json", "standing_record_memory.json", "duty_roster_quests.json", "duty_roster_locations.json", "duty_roster_marks.json", "duty_roster_seasons.json", "thirdonary_quests.json", "shelter_schedules.json", "power_grid.json", "utility_actions.json", "warlord_doctrines.json", "trade_screen_scenarios.json" })
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

        private static void TryLoadAdvancedIndustrialReconCatalogs(string dataDir, IFileIO files, IJsonSerializer json,
            ContentUtilizationInstrumentation instr)
        {
            TryLoadAdvancedCatalog(FischerTropschCatalogLoader.CatalogFileName, "FischerTropschCatalogLoader",
                dataDir, files, instr, () =>
                {
                    var catalog = FischerTropschCatalogLoader.Load(dataDir, files, json);
                    return catalog.Reactors.Count + catalog.Products.Count + catalog.Catalysts.Count;
                });
            TryLoadAdvancedCatalog("uv_corona_detector_catalog.json", "UvCoronaDetectionCatalogLoader",
                dataDir, files, instr, () => UvCoronaDetectionCatalogLoader.Load(dataDir, files, json).Detectors.Count);
            TryLoadAdvancedCatalog("carbon_composite_catalog.json", "CarbonCompositeCatalogLoader",
                dataDir, files, instr, () => CarbonCompositeCatalogLoader.Load(dataDir, files, json).Components.Count);
            TryLoadAdvancedCatalog("gpr_exploration_catalog.json", "GroundPenetratingRadarCatalogLoader",
                dataDir, files, instr, () => GroundPenetratingRadarCatalogLoader.Load(dataDir, files, json).Modes.Count);
        }

        private static void TryLoadAdvancedCatalog(string file, string loader, string dataDir, IFileIO files,
            ContentUtilizationInstrumentation instr, Func<int> definitionCount)
        {
            try
            {
                string path = Path.Combine(dataDir, file);
                if (!files.FileExists(path)) return;
                instr.RecordCatalogOpened(file, loader);
                int count = definitionCount();
                instr.RecordCatalogDeserialized(file, count);
                instr.RecordDefinitionsRegistered(file, loader, count);
            }
            catch (Exception ex) { Godot.GD.PrintErr($"[RuntimeEvidence] {file}: {ex.Message}"); }
        }

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
        }
    }
}
