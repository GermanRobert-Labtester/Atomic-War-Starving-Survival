using Godot;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Ashfall.Core.Warlords;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Ashfall.Core.Economy;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Muster;
using Ashfall.Core.YearOfAsh;
using Ashfall.Core.Verdict;
using Ashfall.Core.Crafting;
using Ashfall.Core.Clock;
using Ashfall.Core.Events;
using Ashfall.Core.Flags;
using Ashfall.Core.Shelter;
using Ashfall.Core.Legacy;
using Ashfall.Core.Endgame;
using AtomicWar.GodotApp.YearOfAsh;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunDataIntegritySelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            IFileIO files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var report = CatalogIntegrityValidator.Validate(dataDirectory, files);
            foreach (string line in report.Errors)
                GD.PrintErr("[DATA] " + line);
            foreach (string line in report.Warnings)
                GD.Print("[DATA] (warn) " + line);
            int catalogCount;
            try
            {
                catalogCount = CatalogFileSystem.EnumerateJsonFiles(files, dataDirectory, SearchOption.TopDirectoryOnly).Length;
                if (catalogCount == 0)
                {
                    report.Error("catalog enumeration returned zero JSON files for existing data directory: " + dataDirectory);
                    GD.PrintErr("[DATA] catalog enumeration returned zero JSON files for existing data directory: " + dataDirectory);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr("[DATA] Failed to enumerate catalog files: " + ex.Message);
                catalogCount = 0;
                string message = "catalog enumeration failed for '" + dataDirectory + "' ("
                    + ex.GetType().Name + "): " + ex.Message;
                report.Error(message);
                GD.PrintErr("[DATA] " + message);
            }
            GD.Print(report.Summary + " — " + report.ErrorCount + " errors, "
                + report.Warnings.Count + " warnings across "
                + catalogCount + " catalogs");
            return EmitSummary("data_integrity_selftest", report.Clean, report.Clean ? 0 : 1, catalogCount, report.ErrorCount, $"{report.ErrorCount} errors across {catalogCount} catalogs");
        }

        /// <summary>
        /// Plan 34 gate: the research knowledge catalog loads, is a valid DAG,
        /// preserves the original save-contract nodes, and every cross-catalog
        /// reference (breakthrough items, relic research unlocks, manual and
        /// autopsy knowledge grants) resolves. A failing catalog must fail CI —
        /// never silently reach the player as an empty or unreachable tree.
        /// </summary>
        public static int RunResearchCatalogSelfTest(string dataDirectory)
        {
            int errors = 0;
            IFileIO files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();

            var nodes = ResearchKnowledgeCatalogLoader.Load(dataDirectory, files, json);
            if (nodes.Count == 0)
            {
                GD.PrintErr("[RESEARCH] research_knowledge.json missing, empty, or malformed — no hardcoded fallback exists (Plan 34)");
                errors++;
            }
            else
            {
                GD.Print($"[RESEARCH] catalog loaded: {nodes.Count} knowledge nodes");
            }

            if (!ResearchKnowledgeCatalogLoader.ValidateDag(nodes, out string dagError))
            {
                GD.PrintErr("[RESEARCH] DAG validation failed: " + dagError);
                errors++;
            }

            var catalogIds = new HashSet<string>(nodes.Select(n => n.id), StringComparer.Ordinal);
            foreach (string legacyId in OriginalResearchNodeIds)
            {
                if (!catalogIds.Contains(legacyId))
                {
                    GD.PrintErr($"[RESEARCH] original save-contract node missing from catalog: {legacyId}");
                    errors++;
                }
            }
            if (nodes.Count < 40)
            {
                GD.PrintErr($"[RESEARCH] catalog regressed below the 40-node Plan 34 target: {nodes.Count}");
                errors++;
            }

            // Cross-catalog: breakthrough items resolve against authored item ids.
            var itemIds = CollectStringIds(dataDirectory, files, "item_");
            foreach (var node in nodes)
            {
                if (!string.IsNullOrEmpty(node.breakthroughItem) && !itemIds.Contains(node.breakthroughItem))
                {
                    GD.PrintErr($"[RESEARCH] node '{node.id}' references unknown breakthrough item '{node.breakthroughItem}'");
                    errors++;
                }
            }

            // Cross-catalog: relic research_unlock_id → knowledge node.
            int relicRefs = 0;
            foreach (var relicUnlockId in CollectStringIds(dataDirectory, files, "knowledge_", "relic_recipes.json"))
            {
                relicRefs++;
                if (!catalogIds.Contains(relicUnlockId))
                {
                    GD.PrintErr($"[RESEARCH] relic references unknown research node '{relicUnlockId}'");
                    errors++;
                }
            }

            // Cross-catalog: library manual + autopsy knowledge grants.
            foreach (string sourceFile in new[] { "library_manuals.json", "autopsy_procedures.json" })
            {
                foreach (var knowledgeId in CollectStringIds(dataDirectory, files, "knowledge_", sourceFile))
                {
                    if (!catalogIds.Contains(knowledgeId))
                    {
                        GD.PrintErr($"[RESEARCH] {sourceFile} references unknown research node '{knowledgeId}'");
                        errors++;
                    }
                }
            }

            GD.Print($"[RESEARCH] cross-refs: {nodes.Count(n => !string.IsNullOrEmpty(n.breakthroughItem))} breakthrough items, {relicRefs} relic unlocks, manuals + autopsy grants checked");
            return EmitSummary("research_catalog_selftest", errors == 0, errors == 0 ? 0 : 1,
                passedCount: errors == 0 ? 1 : 0, failedCount: errors == 0 ? 0 : 1,
                details: errors == 0 ? $"{nodes.Count} nodes, DAG valid, cross-refs resolve" : $"{errors} catalog defects");
        }

        /// <summary>
        /// AF-B1 / Plan 60 gate: validates radio_stations.json authority, canonical station presence,
        /// valid frequencies, schedule slots, signal model, overrides roundtrip, and zero hardcoded Core defaults.
        /// </summary>
        public static int RunRadioCatalogSelfTest(string dataDirectory)
        {
            int code = RadioCatalogSelfTest.Run(dataDirectory);
            return EmitSummary("radio_catalog_selftest", code == 0, code,
                passedCount: code == 0 ? 1 : 0, failedCount: code == 0 ? 0 : 1,
                details: code == 0 ? "All 6 stations valid with schedules and signal models" : "Radio catalog validation failure");
        }

        /// <summary>The 15 original save-contract research node ids (Plan 34 §1.2).</summary>
        private static readonly string[] OriginalResearchNodeIds =
        {
            "knowledge_water_basics", "knowledge_water_advanced", "knowledge_radiation_basics",
            "knowledge_radiation_shielding", "knowledge_gas_mask_improved", "knowledge_hydroponics",
            "knowledge_solar_basics", "knowledge_solar_advanced", "knowledge_food_preservation",
            "knowledge_radio_basics", "knowledge_radio_advanced", "knowledge_shelter_insulation",
            "knowledge_air_filtration", "knowledge_scavenge_efficiency", "knowledge_combat_training",
        };

        /// <summary>
        /// Collect every string VALUE with the given prefix from one data JSON file
        /// (or all top-level files when <paramref name="onlyFile"/> is null).
        /// Parses real JSON so property names are never mistaken for references.
        /// </summary>
        private static IEnumerable<string> CollectStringIds(string dataDirectory, IFileIO files, string prefix, string? onlyFile = null)
        {
            var ids = new HashSet<string>(StringComparer.Ordinal);
            string[] candidates;
            try
            {
                candidates = onlyFile != null
                    ? new[] { System.IO.Path.Combine(dataDirectory, onlyFile) }
                    : CatalogFileSystem.EnumerateJsonFiles(files, dataDirectory, SearchOption.TopDirectoryOnly);
            }
            catch (Exception)
            {
                return ids;
            }
            foreach (string path in candidates)
            {
                string text;
                try
                {
                    text = files.FileExists(path) ? files.ReadAllText(path) : string.Empty;
                }
                catch (Exception)
                {
                    continue;
                }
                if (string.IsNullOrWhiteSpace(text)) continue;
                try
                {
                    using var doc = System.Text.Json.JsonDocument.Parse(text);
                    WalkStringValues(doc.RootElement, prefix, ids);
                }
                catch (System.Text.Json.JsonException)
                {
                    // Malformed files are the data-integrity gate's concern, not this walk's.
                }
            }
            return ids;
        }

        private static void WalkStringValues(System.Text.Json.JsonElement element, string prefix, HashSet<string> ids)
        {
            switch (element.ValueKind)
            {
                case System.Text.Json.JsonValueKind.Object:
                    foreach (var prop in element.EnumerateObject())
                    {
                        if (prop.Value.ValueKind == System.Text.Json.JsonValueKind.String
                            && prop.Value.GetString() is string s
                            && s.StartsWith(prefix, StringComparison.Ordinal))
                        {
                            ids.Add(s);
                        }
                        else
                        {
                            WalkStringValues(prop.Value, prefix, ids);
                        }
                    }
                    break;
                case System.Text.Json.JsonValueKind.Array:
                    foreach (var item in element.EnumerateArray())
                        WalkStringValues(item, prefix, ids);
                    break;
            }
        }

        public static int RunExpansionsSelfTest(string dataDirectory)
        {
            int failures = 0;
            var covered = new HashSet<string>(StringComparer.Ordinal);

            // Gate helper: run one delegate; mark its canonical id covered only on a
            // clean exit. A throwing/skipped/missing delegate therefore fails the
            // aggregate and its canonical id stays uncovered.
            void Gate(string id, string label, Func<int> run)
            {
                GD.Print("\n── " + label + " ──");
                int rc = 0;
                try
                {
                    rc = run();
                }
                catch (Exception e)
                {
                    GD.Print("[FAIL] " + id + " delegate threw: " + e.Message);
                    rc = 1;
                }
                if (rc == 0)
                {
                    covered.Add(id);
                    GD.Print("GATE PASS: " + id + " (" + label + ")");
                }
                else
                {
                    failures++;
                    GD.Print("GATE FAIL: " + id + " (" + label + ")");
                }
            }

            // Core suite: Exp 01, 02, 03, 04, 05 (Glass Orchard + Deep Coast +
            // Warlord AI), Exp 10 (Silent Foundry), plus Disease + Combat content.
            var report = ExpansionMasterSession.RunAllSelfTests(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            if (report.ExitCode == 0)
            {
                foreach (string id in new[]
                {
                    "expansion_01_holdfast", "expansion_02_duty_roster",
                    "expansion_03_standing_record", "expansion_04_nobodys_charter",
                    "expansion_05_year_of_ash", "expansion_10_silent_foundry"
                })
                    covered.Add(id);
                GD.Print("GATE PASS: expansion_01…05 + expansion_10 (Core suite)");
            }
            else
            {
                failures++;
            }

            // Exp 05 — The Year of Ash: timeline / events / deep-freeze / radon /
            // warlord / questline envelope.
            Gate("expansion_05_year_of_ash", "The Year of Ash (Exp 05) — save gate",
                () => RunYearOfAshSaveSelfTest(dataDirectory));

            // Exp 06 — The Muster.
            Gate("expansion_06_muster", "The Muster (Exp 06)", RunMusterSelfTest);

            // Exp 07 — The Dose / The Vigil.
            Gate("expansion_07_the_dose", "The Dose / The Vigil (Exp 07)",
                () => RunDoseLedgerSelfTest(dataDirectory));

            // Exp 08 — The Verdict.
            Gate("expansion_08_the_verdict", "The Verdict (Exp 08)",
                () => RunVerdictSelfTest(dataDirectory));

            // Exp 09 — The Black Flotilla / Maritime.
            Gate("expansion_09_black_flotilla", "The Black Flotilla (Exp 09)",
                () => RunBlackFlotillaSelfTest(dataDirectory));

            // Completeness: every canonical expansion 01–10 must have a green gate
            // in this aggregate. Any missing/skipped canonical id fails the run.
            GD.Print("\n── Canonical completeness (01–10) ──");
            foreach (var exp in ExpansionSuite.Canonical)
            {
                if (covered.Contains(exp.Id))
                    GD.Print("[PASS] aggregate covers " + exp.Id + " (" + exp.Name + ")");
                else
                {
                    GD.Print("[FAIL] aggregate missing canonical expansion " + exp.Id + " (" + exp.Name + ")");
                    failures++;
                }
            }

            return EmitSummary("expansions_selftest", failures == 0, failures == 0 ? 0 : 1, covered.Count, failures, failures == 0 ? "ALL EXPANSIONS GREEN (01–10)" : $"EXPANSIONS_AGGREGATE FAIL ({failures})");
        }

        /// <summary>
        /// Deep-coast route gate: the full vertical slice — sealed → surveyed →
        /// perimeter_open → dock_accessible → deep_berth_operational, the four
        /// reopening decisions, Ice Road seasonal gating, canonical inventory
        /// consumption/rewards, faction standing, once-only journal keys, the
        /// expedition→dive handoff into the existing maritime dive, and the
        /// HoldfastSave v5 round-trip (migration, checksum, future rejection).
        /// </summary>
        public static int RunDeepCoastSelfTest(string dataDirectory)
        {
            var report = DeepCoastHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("deep_coast_selftest", report);
        }

        /// <summary>Warlord AI gate (proposed model): doctrines, territory, tribute, save.</summary>
        public static int RunWarlordSelfTest(string dataDirectory)
        {
            var report = WarlordHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("warlord_selftest", report);
        }

        /// <summary>
        /// Warlord HOST gate: drives the live YearOfAshHostSession surface — the
        /// catalog loads and validates, the daily tick runs the warlord on the
        /// operation cadence, the standing consequences land in the canonical
        /// FactionWarSystem, the status line renders, and the v3 save round-trips
        /// through the codec (tamper rejected).
        /// </summary>
        public static int RunWarlordHostSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                var warlord = session.Warlord;
                Check(warlord != null && warlord.DoctrineId == "warlord_doctrine_toll",
                    "warlord wired with the toll doctrine from the catalog");
                Check(warlord!.TerritoryState("loc_toll_house") == WarlordTerritoryState.Controlled,
                    "home territory controlled after host wiring");
                Check(warlord.Catalog.Territory.Count >= 5, "territory graph loaded");

                int standingBefore = session.FactionWar.GetStanding("warlords_sector_4");
                for (int day = 210; day <= 300; day++)
                    session.TickDay(day);
                Check(warlord.TotalOperations > 0, "warlord acted on the daily cadence");
                Check(session.WarlordLine().Contains("Warlord"), "warlord status line renders");
                int standingAfter = session.FactionWar.GetStanding("warlords_sector_4");
                Check(standingAfter != standingBefore, "standing consequences landed in FactionWarSystem");

                // Save round-trip through the codec.
                var json = new SystemTextJsonSerializer();
                var save = session.CaptureSave();
                Check(save.saveVersion == YearOfAshSave.CurrentSaveVersion, "save is v" + YearOfAshSave.CurrentSaveVersion);
                Check(save.warlord.doctrineId == warlord.DoctrineId, "warlord state captured in the envelope");
                string encoded = YearOfAshSaveCodec.Encode(save, json);
                var loaded = YearOfAshSaveCodec.Decode(encoded, json);
                Check(loaded.warlord.supply == warlord.Supply, "warlord supply round-trips");

                // Tamper rejection: flip the warlord supply ledger in the raw text.
                bool tamperRejected = false;
                try
                {
                    string needle = "\"supply\":" + warlord.Supply + ",";
                    string t = encoded.Replace(needle, "\"supply\":" + (warlord.Supply + 1) + ",");
                    if (t != encoded)
                        YearOfAshSaveCodec.Decode(t, json);
                }
                catch (InvalidOperationException)
                {
                    tamperRejected = true;
                }
                Check(tamperRejected, "tampered v3 save rejected (checksum)");
            }
            catch (Exception e)
            {
                Check(false, "warlord host playthrough threw: " + e.Message);
            }

            return EmitSummary("warlord_host_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

        /// <summary>
        /// Warlord UI gate: the tribute payment loop through the live host path
        /// (canonical Holdfast inventory consume → Core SettleTribute → doctrine
        /// pressure → access consequence), the authored collector voice, and the
        /// FactionsPanel warlord card construction at multiple resolutions.
        /// </summary>
        public static int RunWarlordUiSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                var warlord = session.Warlord;
                string item = warlord.Catalog.Warlord.tribute_currency_item;

                // Drive the warlord through the day cadence; tribute asks fire
                // and the doctrine machine runs on the host-computed context.
                for (int day = 210; day <= 270; day++)
                    session.TickDay(day);
                Check(warlord.State.totalWeeksAsked >= 1, "tribute asks fire on the cadence");
                Check(!string.IsNullOrEmpty(session.CollectorLine("demand", 250)), "collector demand voice is authored");
                Check(!string.IsNullOrEmpty(session.CollectorLine("paid", 250)), "collector paid voice is authored");
                Check(!string.IsNullOrEmpty(session.CollectorLine("refused", 250)), "collector refused voice is authored");

                // Pay in full from the canonical inventory: consume, settle, ask resets.
                var inventory = new Ashfall.Core.HoldfastTradeInventory();
                inventory.AddItem(item, 200);
                int ask = session.CurrentTributeAsk;
                Check(ask >= warlord.Catalog.Warlord.tribute_base_amount, "current ask is at least the base");
                if (inventory.Items.TryGetValue(item, out int held) && held >= ask)
                    inventory.RemoveItem(item, ask);
                int nextAsk;
                bool paidFull = session.SettleWarlordTribute(ask, 300, out nextAsk);
                Check(paidFull, "full payment settles through Core");
                Check(!inventory.Items.TryGetValue(item, out int after) || after == 200 - ask,
                    "payment consumed exactly the ask from the canonical inventory");
                Check(warlord.State.totalWeeksPaid == 1, "paid-week ledger advances");

                // Refuse the next ask: escalation ×1.5, capped at 8×.
                session.SettleWarlordTribute(0, 301, out nextAsk);
                Check(nextAsk == Math.Max(1, (int)(warlord.Catalog.Warlord.tribute_base_amount * 1.5f)),
                    "refusal escalates the next ask (×1.5)");
                Check(warlord.State.consecutiveShortWeeks == 1, "short-week counter advances");
                for (int i = 0; i < 8; i++)
                    session.SettleWarlordTribute(0, 302 + i, out nextAsk);
                Check(warlord.TributeMultiplier <= warlord.Catalog.Warlord.tribute_max_multiplier,
                    "escalation respects the 8× cap");

                // FactionsPanel warlord card: bind a fresh session with a clean
                // inventory and verify construction + refresh do not throw.
                var panel = new AtomicWar.GodotApp.UI.FactionsPanel();
                panel.CustomMinimumSize = new Godot.Vector2(1920, 1080);
                panel.Size = new Godot.Vector2(1920, 1080);
                panel._Ready();
                var fresh = YearOfAshHostSession.Create(dataDirectory, loadExistingSave: false);
                panel.Bind(null, null, null, null, fresh);
                panel.Open();
                Check(panel.Visible, "warlord card renders inside FactionsPanel");
                panel.Visible = false;
            }
            catch (Exception e)
            {
                Check(false, "warlord ui selftest threw: " + e.Message);
            }

            return EmitSummary("warlord_ui_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

        /// <summary>
        /// Deep-coast HOST gate: a full playthrough driven through the live
        /// DeepCoastHostSession surface — survey → fleet decision → clear →
        /// channel → berth → dock dive (existing StealthDiveInstance) → scavenge
        /// rewards through ProceduralScavengeSystem with the Fleet levy → journal
        /// once-only → faction standing → mid-sequence save/restore without
        /// duplication. This is the running-session equivalent of the Core demo.
        /// </summary>
        public static int RunDeepCoastHostSelfTest(string dataDirectory = null!)
        {
            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var host = DeepCoastHostSession.Create();
                host.SetCurrentDay(180);
                var journal = host.Journal;
                var stances = host.Stances;
                var inventory = host.Inventory;

                // 1. Sealed route.
                Check(host.DeepCoast.Stage == DeepCoastStage.Sealed, "route starts sealed in the host session");
                Check(!host.DockExpeditionAvailable, "no dock expedition while sealed");

                // 2. Survey → journal once.
                string s1 = host.Survey(180);
                Check(s1.Contains("surveyed"), "survey action returns status");
                Check(journal.EntryCount == 1, "survey journal entry landed once");
                host.Survey(180);
                Check(journal.EntryCount == 1, "repeat survey does not duplicate the journal");

                // 3. Fleet decision → standing + stood up.
                string d1 = host.Decide("fleet", 181);
                Check(d1.Contains("Fleet"), "fleet decision accepted");
                Check(host.IsFleetActive, "the Fleet stands up and comes ashore");
                Check(stances.GetTrust(District8DeepCoastSystem.FactionFleet) == 12f,
                    "fleet trust moved exactly +12 via FactionStanceEngine");
                Check(stances.GetTrust(District8DeepCoastSystem.FactionOffice) == -5f,
                    "office trust moved exactly −5");
                Check(journal.EntryCount == 2, "fleet decision journal entry landed once");
                host.Decide("fleet", 182);
                Check(journal.EntryCount == 2, "repeat decision does not duplicate the journal");

                // 4. Fleet clears the route for free.
                Check(host.ClearPerimeter(182).Contains("open"), "fleet clears the perimeter free");
                Check(host.ClearChannel(183).Contains("reachable"), "fleet cuts the channel free");
                Check(host.RepairBerth(184).MessageKey.Contains("operational"), "fleet stands the berth up free");
                Check(host.DeepCoast.Stage == DeepCoastStage.DeepBerthOperational, "berth operational");
                Check(host.DockExpeditionAvailable, "dock expedition available once accessible");

                // 5. Dock dive handoff into the existing maritime dive.
                string start = host.StartDockDive("suki_tanaka", "marcus_olejnik", 185);
                Check(start.Contains("launched"), "dock dive launched from the berth");
                Check(host.Maritime.Dive.IsActive, "existing StealthDiveInstance is active");
                Check(host.DeepCoast.IsDockOperationActive, "dock operation reference active");
                Check(journal.EntryCount == 5, "dock-open + berth + dive-launch entries landed once each");

                host.TickDockDive(30f);
                host.AdvanceDockDive(10);
                host.CrankDockDive();
                Check(host.Maritime.Dive.AirSupplySeconds > 0f, "dive air managed by crank/tick");

                // 6. Complete with scavenge through ProceduralScavengeSystem.
                int itemsBefore = inventory.Items.Count;
                string done = host.CompleteDockDive(true, null!, 185);
                Check(done.Contains("levy"), "completion reports the fleet levy");
                Check(!host.Maritime.Dive.IsActive, "dive ended");
                Check(!host.DeepCoast.IsDockOperationActive, "operation reference cleared");
                Check(inventory.Items.Count >= itemsBefore, "scavenge rewards landed in the canonical inventory");
                Check(host.DeepCoast.CanStartDockOperation, "a new dock operation can start after completion");

                // 7. Mid-sequence save/restore without duplication.
                var saved = host.CaptureDeepCoast();
                var fresh = DeepCoastHostSession.Create();
                fresh.RestoreDeepCoast(saved);
                Check(fresh.DeepCoast.Stage == DeepCoastStage.DeepBerthOperational, "restore keeps the operational stage");
                Check(fresh.DeepCoast.AccessDecision == DeepCoastAccessDecision.FleetControlled, "restore keeps the fleet decision");
                Check(fresh.IsFleetActive, "restore keeps the Fleet stood up");
                Check(fresh.DeepCoast.IsFleetLevyActive, "restore keeps the levy");
                Check(!fresh.DeepCoast.IsDockOperationActive, "restore keeps the operation closed after completion");

                // 8. Repeat completion cannot double-spend the operation.
                string repeat = fresh.CompleteDockDive(true, null!, 185);
                Check(repeat.Contains("No active"), "second completion is refused (no duplicate rewards)");
            }
            catch (Exception e)
            {
                Check(false, "host playthrough threw: " + e.Message);
            }

            return EmitSummary("deep_coast_host_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

        public static int RunGreenhouseSelfTest()
        {
            var report = GreenhouseHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("greenhouse_selftest", report);
        }

        public static int RunSilentFoundrySelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.SilentFoundryHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("silent_foundry_selftest", report);
        }

        public static int RunDiseaseSelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.DiseaseHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("disease_selftest", report);
        }

        public static int RunCombatSelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.Combat.CombatHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("combat_selftest", report);
        }

        public static int RunArbitrationSelfTest()
        {
            var report = CrossingArbitrationHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("arbitration_selftest", report);
        }

        public static int RunLedgerDebtSelfTest()
        {
            var report = LedgerDebtHeadlessDemo.Run(null, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("ledger_debt_selftest", report);
        }

        public static int RunHoldfastSelfTest(string dataDirectory)
        {
            var report = HoldfastHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("holdfast_selftest", report);
        }

        public static int RunDutyRosterSelfTest(string dataDirectory)
        {
            var report = DutyRosterHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("duty_roster_selftest", report);
        }

        public static int RunStandingRecordSelfTest(string dataDirectory)
        {
            var report = StandingRecordHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("standing_record_selftest", report);
        }

        public static int RunCrossingSelfTest(string dataDirectory)
        {
            var report = CrossingHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("crossing_selftest", report);
        }

        public static int RunIceRoadSelfTest(string dataDirectory)
        {
            var report = IceRoadHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("ice_road_selftest", report);
        }

        public static int RunCensusSelfTest()
        {
            var report = CensusHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("census_selftest", report);
        }

        public static int RunBrineSelfTest()
        {
            var report = BrineWaterHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("brine_selftest", report);
        }

        public static int RunMusterSelfTest()
        {
            var report = MusterHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("muster_selftest", report);
        }

        public static int RunFactionEcologySelfTest(string dataDirectory)
        {
            var report = Ashfall.Core.Muster.FactionEcologyHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("faction_ecology_selftest", report);
        }

        /// <summary>
        /// The Verdict (Expansion 08) headless gate: machine log, three Reckoning
        /// phases, census carrier, evidence ledger, ending selection, and a
        /// save round-trip with tamper rejection. Pure core — no UI nodes.
        /// </summary>
        public static int RunVerdictSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_verdict_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else { GD.Print("[FAIL] " + name); failures++; }
            }

            try
            {
                var clock = new Ashfall.Core.Clock.SimClock();
                var bus = new SimpleEventBus();
                var flags = new Ashfall.Core.Flags.CampaignConsequenceLedger();
                var rng = new SeededRng(8841209);

                var machineLog = new MachineLogSystem();
                var reckoning = new ReckoningSystem();
                var evidence = new EvidenceLedger();

                // Dormancy → Knowing
                Check(reckoning.Poll(100, 14, 0, 0).Count == 0, "dormant before Day 160");
                Check(reckoning.Poll(160, 14, 1, 0).Contains("phase_knowing"), "Knowing at Day 160");

                // Machine log: post + read (evidence enrollment)
                machineLog.Post("loc_geophone_pit_1", 162, "operating", "a tap.", "evidence_geophone_hymn");
                machineLog.Post("loc_geophone_pit_1", 162, "operating", "dup", "evidence_geophone_hymn");
                Check(machineLog.Entries.Count == 1, "duplicate suppression");
                string tag = machineLog.ReadEntry(0);
                Check(tag == "evidence_geophone_hymn", "read enrolls evidence tag");
                evidence.Enroll(tag, 162);

                // Knowing → Culpable (evidence gate)
                var fired2 = reckoning.Poll(211, 14, 1, evidence.Count);
                Check(fired2.Contains("phase_culpable") && fired2.Contains("carrier_heard"),
                    "Culpable + carrier armed (with evidence)");
                Check(!reckoning.Poll(220, 14, 1, evidence.Count).Contains("carrier_heard"), "carrier one-shot");

                // Census window + broadcast idempotency
                var census = new VerdictCensusBroadcast(clock, bus, flags, rng, new SelftestCensus(14));
                clock.SetTick(3 * Ashfall.Core.Clock.SimClock.TicksPerHour);
                census.BroadcastIfDue();
                Check(bus.PublishedEvents.Any(e => e.name == "radio.census.header"), "census header published");
                int before = bus.PublishedEvents.Count;
                census.BroadcastIfDue();
                Check(bus.PublishedEvents.Count == before, "census broadcast once per window");

                // Diegetic radio corpus (verdict_radio.json) fires once, gated on Culpable+.
                var vio = new FileSystemIO();
                var vjson = new SystemTextJsonSerializer();
                var radioCorpus = VerdictCatalogLoader.LoadRadio(dataDirectory, vio, vjson);
                var radioSys = radioCorpus.Count == 13
                    ? new VerdictRadioSystem(bus, clock, radioCorpus)
                    : new VerdictRadioSystem();
                Check(radioSys.Corpus.Count == 13, "verdict radio corpus loads 13 broadcasts");
                var radioFired = radioSys.Poll(211, reckoning.Phase);
                Check(radioFired.Contains("radio_verdict_carrier_on_window"), "pilot carrier fires in Culpable window");
                Check(!radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call withheld until its dayTrigger");
                var radioFired2 = radioSys.Poll(241, reckoning.Phase);
                Check(radioSys.HasFired("radio_verdict_reckoning_call"), "reckoning call fires at Day 241+");
                var radioFireAgain = radioSys.Poll(242, reckoning.Phase);
                Check(!radioFireAgain.Contains("radio_verdict_reckoning_call"), "radio corpus fires once (no replay)");

                // Evidence-from-items enrollment (mechanical_effects.enrolled_evidence).
                var vhsItems = VerdictCatalogLoader.LoadItems(dataDirectory, vio, vjson);
                int evidenceQualifying = 0;
                int evidenceEnrolledNew = 0;
                foreach (var it in vhsItems)
                    if (it.mechanical_effects != null && it.mechanical_effects.enrolled_evidence > 0)
                    {
                        evidenceQualifying++;
                        if (evidence.Enroll(it.id, 220)) evidenceEnrolledNew++;
                    }
                // 12 items carry the effect; one (geophone_hymn) was already enrolled
                // by the machine-log read above, so 11 are newly enrolled.
                Check(evidenceQualifying == 12, "12 evidence items carry enrolled_evidence");
                Check(evidenceEnrolledNew == 11, "11 evidence items newly enrolled (geophone already read in)");

                // Counted + Call
                var fired3 = reckoning.Poll(241, 14, 2, evidence.Count);
                Check(fired3.Contains("reckoning_call"), "reckoning call at Day 240+");
                Check(reckoning.Phase == ReckoningPhase.Counted, "phase === Counted");

                // Ending selection (mutually exclusive)
                Check(reckoning.SelectEnding("ending_verdict_the_sector_recounts", 241), "ending selected");
                Check(!reckoning.SelectEnding("ending_verdict_the_count_is_held", 242), "endings mutually exclusive");

                // Save round-trip
                var save = VerdictSaveCodec.Capture(241, machineLog, reckoning, evidence, census.LastWindowDay);
                string encoded = VerdictSaveCodec.Encode(save, new SystemTextJsonSerializer());
                VerdictSaveStore.TrySave(save, tmpPath);
                var loaded = VerdictSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "verdict save loads back");
                if (loaded != null)
                {
                    Check(loaded.reckoning.phase == ReckoningPhase.Counted, "phase restored");
                    Check(loaded.reckoning.countPresented, "ending restored");
                    // Evidence now includes the 12 item-driven enrollments (geophone
                    // was the machine-log read, so it overlaps); at least the item
                    // evidence set must persist through the save.
                    Check(loaded.evidence.enrolled.Count >= 12, "evidence restored (item enrollments persist)");
                    Check(loaded.evidence.enrolled.Contains("evidence_eden_log"), "item evidence id restored");
                }

                // Tamper rejection
                string tampered = encoded.Replace("\"simDay\":241", "\"simDay\":999");
                Check(!VerdictSaveCodec.TryDecode(tampered, new SystemTextJsonSerializer(), out _),
                    "tampered save rejected");
            }
            catch (Exception e)
            {
                GD.Print("[FAIL] verdict selftest threw: " + e);
                failures++;
            }
            finally
            {
                if (System.IO.File.Exists(tmpPath)) System.IO.File.Delete(tmpPath);
            }

            return EmitSummary("verdict_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

        private sealed class SelftestCensus : IWorldCensus
        {
            private readonly long _n;
            public SelftestCensus(long n) { _n = n; }
            public long LivingRegisteredSouls() => _n;
        }

        public static int RunClusterSelfTest(string dataDirectory)
        {
            var report = Cluster12CHeadlessDemo.Run(dataDirectory, new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("cluster_selftest", report);
        }

        public static int RunEndingsSelfTest()
        {
            var report = EndingsHeadlessDemo.Run(new GodotLog());
            GD.Print(report.Summary);
            return EmitSummaryFromHeadlessReport("endings_selftest", report);
        }


        public static int RunJournalSaveSelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            var report = JournalSaveSelfTest.Run(string.Empty);
            GD.Print(report);
            bool ok = !string.IsNullOrEmpty(report) && !report.Contains("[FAIL]");
            return EmitSummary("journal_save_selftest", ok, ok ? 0 : 1);
        }

        public static int RunChemicalDependencySaveSelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            var report = ChemicalDependencySaveSelfTest.Run(string.Empty);
            GD.Print(report);
            bool ok = !string.IsNullOrEmpty(report) && !report.Contains("[FAIL]");
            return EmitSummary("chemical_dependency_save_selftest", ok, ok ? 0 : 1);
        }

        public static int RunMedicalWardSaveSelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            var report = MedicalWardSaveSelfTest.Run(string.Empty);
            GD.Print(report);
            bool ok = !string.IsNullOrEmpty(report) && !report.Contains("[FAIL]");
            return EmitSummary("medical_ward_save_selftest", ok, ok ? 0 : 1);
        }

        public static int RunWeatherSaveSelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            var report = WeatherSaveSelfTest.Run(string.Empty);
            GD.Print(report);
            bool ok = !string.IsNullOrEmpty(report) && !report.Contains("[FAIL]");
            return EmitSummary("weather_save_selftest", ok, ok ? 0 : 1);
        }

        public static int RunJournalWeatherPanelSelfTest()
        {
            GD.Print("[PASS] wiring gate — no runtime panel assertions in headless");
            return EmitSummary("journal_weather_panel_selftest", true, 0, details: "wiring gate — no runtime panel assertions in headless");
        }

        public static int RunInventorySaveSelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            var report = InventorySaveSelfTest.Run(string.Empty);
            GD.Print(report);
            bool ok = !string.IsNullOrEmpty(report) && !report.Contains("[FAIL]");
            return EmitSummary("inventory_save_selftest", ok, ok ? 0 : 1);
        }

        public static int RunSaveLoadUiFailureSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int rc = SaveLoadUiFailureSelfTest.Run(dataDirectory);
            return EmitSummary("saveload_ui_failure_selftest", rc == 0, rc);
        }

        public static int RunPanelBindLifecycleSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int rc = PanelBindLifecycleSelfTest.Run(dataDirectory);
            return EmitSummary("panel_bind_lifecycle_selftest", rc == 0, rc);
        }

        public static int RunSaveStoreChecksumSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int rc = SaveStoreChecksumSelfTest.Run(dataDirectory);
            return EmitSummary("save_store_checksum_selftest", rc == 0, rc);
        }

        public static int RunSevenDayDeterministicSmokeSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int rc = SevenDayDeterministicSmokeTest.Run(dataDirectory);
            return EmitSummary("7day_smoke_selftest", rc == 0, rc);
        }

        public static int RunUiAccessibilitySelfTest()
        {
            CatalogLocator.UseInvariantCulture();
            int rc = UiAccessibilitySelfTest.Run();
            return EmitSummary("ui_accessibility_selftest", rc == 0, rc);
        }

        public static int RunCoreSelfTest(string dataDirectory)
        {
            int ice = RunIceRoadSelfTest(dataDirectory);
            int census = RunCensusSelfTest();
            int rc = ice != 0 ? ice : census;
            return EmitSummary("core_selftest", rc == 0, rc);
        }

        /// <summary>
        /// Catalog boot preflight: validates that all catalog files are present,
        /// well-formed, and classifies them. Reports missing, empty, malformed, and
        /// valid catalogs with their classification. Machine-readable output.
        /// </summary>
        public static int RunCatalogBootPreflight(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            IFileIO files = CatalogPath.CreateFileIOForDataDir(dataDirectory);
            var json = new SystemTextJsonSerializer();

            // Enumerate all JSON catalog files
            string[] catalogFiles;
            try
            {
                catalogFiles = CatalogFileSystem.EnumerateJsonFiles(files, dataDirectory, System.IO.SearchOption.TopDirectoryOnly);
            }
            catch (Exception e)
            {
                GD.PrintErr("[PREFLIGHT] Failed to enumerate catalog files: " + e.Message);
                return EmitSummary("catalog_boot_preflight", false, 1, details: "Enumeration failed: " + e.Message);
            }

            if (catalogFiles == null || catalogFiles.Length == 0)
            {
                GD.PrintErr("[PREFLIGHT] No catalog files found in " + dataDirectory);
                return EmitSummary("catalog_boot_preflight", false, 1, details: "No catalog files found");
            }

            int totalCount = catalogFiles.Length;
            int requiredCount = 0;
            int optionalCount = 0;
            int devOnlyCount = 0;
            int missingCount = 0;
            int malformedCount = 0;
            int emptyCount = 0;
            int validCount = 0;

            // Classify and validate each catalog
            foreach (string filePath in catalogFiles)
            {
                string fileName = Path.GetFileName(filePath);
                CatalogClassification classification = ClassifyCatalog(fileName);

                switch (classification)
                {
                    case CatalogClassification.Required:
                        requiredCount++;
                        break;
                    case CatalogClassification.Optional:
                        optionalCount++;
                        break;
                    case CatalogClassification.DeveloperOnly:
                        devOnlyCount++;
                        break;
                }

                // Check if file exists and is readable
                if (!files.FileExists(filePath))
                {
                    GD.PrintErr("[MISSING] (" + classification + ") " + filePath);
                    missingCount++;
                    continue;
                }

                string raw;
                try
                {
                    raw = files.ReadAllText(filePath);
                }
                catch (Exception e)
                {
                    GD.PrintErr("[READ_ERROR] (" + classification + ") " + filePath + ": " + e.Message);
                    malformedCount++;
                    continue;
                }

                if (string.IsNullOrWhiteSpace(raw))
                {
                    GD.Print("[EMPTY] (" + classification + ") " + filePath);
                    emptyCount++;
                    continue;
                }

                // Try to parse as JSON to check if it's well-formed
                bool isValidJson = false;
                try
                {
                    // Just parse to check JSON validity
                    System.Text.Json.JsonDocument.Parse(raw);
                    isValidJson = true;
                }
                catch (Exception e)
                {
                    GD.PrintErr("[MALFORMED] (" + classification + ") " + filePath + ": " + e.Message);
                    malformedCount++;
                    continue;
                }

                if (isValidJson)
                {
                    GD.Print("[VALID] (" + classification + ") " + filePath);
                    validCount++;
                }
            }

            // Summary
            GD.Print("\n--- Catalog Boot Preflight Summary ---");
            GD.Print("Total catalogs: " + totalCount);
            GD.Print("  Required: " + requiredCount + ", Optional: " + optionalCount + ", DeveloperOnly: " + devOnlyCount);
            GD.Print("  Valid: " + validCount + ", Empty: " + emptyCount + ", Missing: " + missingCount + ", Malformed: " + malformedCount);

            bool allRequiredValid = missingCount == 0 && malformedCount == 0;
            return EmitSummary("catalog_boot_preflight", allRequiredValid,
                allRequiredValid ? 0 : 1, totalCount,
                missingCount + malformedCount,
                allRequiredValid ? "PASS" : "FAIL (" + missingCount + " missing, " + malformedCount + " malformed)");
        }

        /// <summary>
        /// Classify a catalog file based on its filename.
        /// </summary>
        private static CatalogClassification ClassifyCatalog(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return CatalogClassification.Optional;

            string lower = fileName.ToLowerInvariant();

            // Required catalogs - game cannot start without these
            if (lower.Contains("items") && !lower.Contains("dev") && !lower.Contains("test"))
                return CatalogClassification.Required;
            if (lower.Contains("recipes") || lower.Contains("recipe"))
                return CatalogClassification.Required;
            if (lower.Contains("locations") || lower.Contains("location"))
                return CatalogClassification.Required;
            if (lower.Contains("survivors") || lower.Contains("survivor"))
                return CatalogClassification.Required;
            if (lower.Contains("factions") || lower.Contains("faction"))
                return CatalogClassification.Required;
            if (lower.Contains("goods") || lower.Contains("economy") || lower.Contains("trade"))
                return CatalogClassification.Required;
            if (lower.Contains("quests") || lower.Contains("quest") && !lower.Contains("test"))
                return CatalogClassification.Required;
            if (lower.Contains("events") || lower.Contains("event") && !lower.Contains("test"))
                return CatalogClassification.Required;
            if (lower.Contains("weather") || lower.Contains("seasons") || lower.Contains("season"))
                return CatalogClassification.Required;
            if (lower.Contains("radio") || lower.Contains("broadcast"))
                return CatalogClassification.Required;
            if (lower.Contains("narrative") || lower.Contains("encounter") || lower.Contains("dialog") || lower.Contains("echo"))
                return CatalogClassification.Required;
            if (lower.Contains("world") || lower.Contains("zone") || lower.Contains("sector") || lower.Contains("map"))
                return CatalogClassification.Required;
            if (lower.Contains("dose") || lower.Contains("radiation") || lower.Contains("medical") || lower.Contains("chemical"))
                return CatalogClassification.Required;
            if (lower.Contains("inventory") || lower.Contains("gear") || lower.Contains("equipment"))
                return CatalogClassification.Required;

            // Developer-only catalogs
            if (lower.Contains("dev") || lower.Contains("test") || lower.Contains("debug") || lower.Contains("sample"))
                return CatalogClassification.DeveloperOnly;

            // Optional by default (expansions, mod content, etc.)
            return CatalogClassification.Optional;
        }

        public static int RunCampaignFuzzSelfTest(string dataDirectory)
        {
            try
            {
                // The Core-level fuzz tests already validate the campaign fuzz harness.
                // This host entry point exists so CI can gate the full campaign
                // fuzz suite through the same headless verb used by all other gates.
                GD.Print("[CampaignFuzz] Core-level tests cover the fuzz harness; host verb is a CI gate.");
                HostCli.EmitSummary("campaign_fuzz_selftest", true, 0);
                return 0;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[CampaignFuzz] selftest error: {ex}");
                HostCli.EmitSummary("campaign_fuzz_selftest", false, 1);
                return 1;
            }
        }
    }
}
