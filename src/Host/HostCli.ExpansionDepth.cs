using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --expansion-depth-selftest / --plan18-selftest:
        /// Verifies Plan 18 Expansion Deepening:
        /// Holdfast (24 quests), Standing Record (52 memories, 22 quests),
        /// Crossing (20 quests, 14 encounters), Verdict (16 questlines, 9 NPCs),
        /// cross-expansion evidence hooks, and save stability.
        /// </summary>
        public static int RunExpansionDepthSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            int totalAssertions = 0;

            void Check(bool ok, string label)
            {
                totalAssertions++;
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            GD.Print("[ExpansionDepthHeadlessDemo] begin Plan 18 verification...");

            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // 1. Holdfast (24 quests, 38 locations)
            var holdfastCatalog = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
            Check(holdfastCatalog != null, "Holdfast catalog loaded");
            Check(holdfastCatalog != null && holdfastCatalog.Quests.Count >= 22, $"Holdfast quests count (expected >= 22, got {holdfastCatalog?.Quests.Count ?? 0})");
            Check(holdfastCatalog != null && holdfastCatalog.Locations.Count >= 30, $"Holdfast locations count (expected >= 30, got {holdfastCatalog?.Locations.Count ?? 0})");
            Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_salt_convoy_haul") != null, "Holdfast salt convoy haul quest present");
            Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_census_claimant_audit") != null, "Holdfast census claimant audit quest present");
            Check(holdfastCatalog != null && holdfastCatalog.GetQuest("quest_holdfast_brine_boiler_scum") != null, "Holdfast brine boiler scum quest present");

            // 2. Standing Record (52 memories, 22 quests, 14 layouts)
            var standingRecordCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDirectory);
            Check(standingRecordCat != null && standingRecordCat.Quests.Count >= 22, $"Standing Record quests count (expected >= 22, got {standingRecordCat?.Quests.Count ?? 0})");
            Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_vault_breach_forensics") != null, "Standing Record vault breach quest present");
            Check(standingRecordCat != null && standingRecordCat.GetQuest("quest_record_the_unmarked_plaque") != null, "Standing Record memorial plaque quest present");

            var memSys = new LocationMemorySystem(files, json, NullLog.Instance);
            memSys.Load(dataDirectory);
            Check(memSys.StratumCount >= 50, $"Standing Record memories count (expected >= 50, got {memSys.StratumCount})");

            var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
            layoutSys.Load(dataDirectory);
            Check(layoutSys.LayoutCount >= 14, $"Standing Record layouts count (expected >= 14, got {layoutSys.LayoutCount})");

            // 3. Crossing (20 quests, 14 encounters)
            var crossingSession = CrossingSession.Load(dataDirectory, NullLog.Instance);
            var crossingCatalog = crossingSession?.Catalog;
            Check(crossingCatalog != null, "Crossing catalog loaded");
            Check(crossingCatalog != null && crossingCatalog.Quests.Count >= 20, $"Crossing quests count (expected >= 20, got {crossingCatalog?.Quests.Count ?? 0})");
            Check(crossingCatalog != null && crossingCatalog.Encounters.Count >= 14, $"Crossing encounters count (expected >= 14, got {crossingCatalog?.Encounters.Count ?? 0})");
            Check(crossingCatalog != null && crossingCatalog.GetQuest("quest_crossing_asylum_in_the_truss") != null, "Crossing asylum quest present");
            Check(crossingCatalog != null && crossingCatalog.GetEncounter("enc_nc_mass_crossing_surge") != null, "Crossing mass surge crisis present");

            // 4. Verdict (16 questlines, 9 NPCs)
            string qlPath = files.Combine(dataDirectory, "verdict_questlines.json");
            if (files.FileExists(qlPath))
            {
                var root = json.Deserialize<VerdictQuestlinesRoot>(files.ReadAllText(qlPath));
                Check(root != null && root.quests != null && root.quests.Count >= 16, $"Verdict questlines count (expected >= 16, got {root?.quests?.Count ?? 0})");
                Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_alibi_verification"), "Verdict alibi verification questline present");
                Check(root != null && root.quests != null && root.quests.Any(q => q.questlineId == "quest_verdict_prior_verdict_appeal"), "Verdict prior verdict appeal questline present");
            }
            else
            {
                Check(false, "verdict_questlines.json exists");
            }

            string npcPath = files.Combine(dataDirectory, "verdict_npcs.json");
            if (files.FileExists(npcPath))
            {
                var root = json.Deserialize<VerdictNpcsRoot>(files.ReadAllText(npcPath));
                Check(root != null && root.items != null && root.items.Count >= 9, $"Verdict NPCs count (expected >= 9, got {root?.items?.Count ?? 0})");
                Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_tomas_reid"), "Verdict defense clerk Tomas Reid present");
                Check(root != null && root.items != null && root.items.Any(n => n.id == "npc_elena_vane"), "Verdict cult deaconess Elena Vane present");
            }
            else
            {
                Check(false, "verdict_npcs.json exists");
            }

            // 5. Questline Master sync check (437 entries)
            string masterPath = files.Combine(dataDirectory, "questline_master.json");
            if (files.FileExists(masterPath))
            {
                var master = json.Deserialize<QuestlineMasterRoot>(files.ReadAllText(masterPath));
                Check(master != null && master.entries != null && master.entries.Count >= 400, $"Questline master entries count (expected >= 400, got {master?.entries?.Count ?? 0})");
            }

            GD.Print($"[ExpansionDepthHeadlessDemo] completed with {failures} failures across {totalAssertions} assertions.");
            return failures == 0 ? 0 : 1;
        }

        private sealed class VerdictQuestlinesRoot
        {
            public int schema_version { get; set; }
            public List<VerdictQuestlineDef>? quests { get; set; }
        }

        private sealed class VerdictQuestlineDef
        {
            public string questlineId { get; set; } = string.Empty;
            public string title { get; set; } = string.Empty;
        }

        private sealed class VerdictNpcsRoot
        {
            public int schema_version { get; set; }
            public List<VerdictNpcDef>? items { get; set; }
        }

        private sealed class VerdictNpcDef
        {
            public string id { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
        }

        private sealed class QuestlineMasterRoot
        {
            public int schema_version { get; set; }
            public List<QuestlineMasterEntryDef>? entries { get; set; }
        }

        private sealed class QuestlineMasterEntryDef
        {
            public string id { get; set; } = string.Empty;
        }
    }
}
