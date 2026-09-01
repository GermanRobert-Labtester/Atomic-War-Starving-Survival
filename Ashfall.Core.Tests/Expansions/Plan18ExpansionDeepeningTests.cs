using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Crossing;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Expansions
{
    public sealed class Plan18ExpansionDeepeningTests
    {
        private static string GetDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out var dir))
                return dir;
            string probe = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data"));
            if (Directory.Exists(probe)) return probe;
            throw new DirectoryNotFoundException("StreamingAssets/Data not found");
        }

        [Fact]
        public void Holdfast_24Quests_WithIceRoadCensusAndBrineMechanics_LoadsCleanly()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDir);

            Assert.NotNull(catalog);
            Assert.True(catalog.Quests.Count >= 22, $"Expected at least 22 quests, got {catalog.Quests.Count}");
            Assert.True(catalog.Locations.Count >= 30, $"Expected at least 30 locations, got {catalog.Locations.Count}");

            // Specific signature quests
            Assert.NotNull(catalog.GetQuest("quest_holdfast_salt_convoy_haul"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_scree_blockage_clear"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_census_claimant_audit"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_census_forged_voucher"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_brine_boiler_scum"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_salter_work_stoppage"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_boiler_crack_panic"));
            Assert.NotNull(catalog.GetQuest("quest_holdfast_ration_lockup_breach"));
        }

        [Fact]
        public void StandingRecord_52Memories_22Quests_14Layouts_ReconstructsAndMutates()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Quests
            var srCat = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
            Assert.True(srCat.Quests.Count >= 22, $"Expected at least 22 quests, got {srCat.Quests.Count}");
            Assert.NotNull(srCat.GetQuest("quest_record_vault_breach_forensics"));
            Assert.NotNull(srCat.GetQuest("quest_record_metro_derailment_triage"));
            Assert.NotNull(srCat.GetQuest("quest_record_mine_shaft_adit_collapse"));
            Assert.NotNull(srCat.GetQuest("quest_record_archive_burn_layer"));
            Assert.NotNull(srCat.GetQuest("quest_record_the_unmarked_plaque"));
            Assert.NotNull(srCat.GetQuest("quest_record_the_last_watch_beacon"));

            // Memories
            var memSys = new LocationMemorySystem(files, json, NullLog.Instance);
            memSys.Load(dataDir);
            Assert.True(memSys.StratumCount >= 50, $"Expected at least 50 memories, got {memSys.StratumCount}");

            // Layouts
            var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
            layoutSys.Load(dataDir);
            Assert.True(layoutSys.LayoutCount >= 14, $"Expected at least 14 layouts, got {layoutSys.LayoutCount}");
        }

        [Fact]
        public void Crossing_20Quests_14Encounters_ArbitrationCasesAndCrises_Resolves()
        {
            string dataDir = GetDataDir();
            var session = CrossingSession.Load(dataDir, NullLog.Instance);

            Assert.NotNull(session);
            Assert.NotNull(session.Catalog);
            Assert.True(session.Catalog.Quests.Count >= 20, $"Expected at least 20 quests, got {session.Catalog.Quests.Count}");
            Assert.True(session.Catalog.Encounters.Count >= 14, $"Expected at least 14 encounters, got {session.Catalog.Encounters.Count}");

            // Specific signature quests
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_asylum_in_the_truss"));
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_contraband_medical_vial"));
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_vehicle_lien_arbitration"));
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_displaced_kin_roll"));
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_quarantine_breach_trial"));
            Assert.NotNull(session.Catalog.GetQuest("quest_crossing_the_null_charter_vote"));

            // Crisis encounters
            Assert.NotNull(session.Catalog.GetEncounter("enc_nc_mass_crossing_surge"));
            Assert.NotNull(session.Catalog.GetEncounter("enc_nc_garrison_iron_blockade"));
            Assert.NotNull(session.Catalog.GetEncounter("enc_nc_pestilence_quarantine_lockdown"));
            Assert.NotNull(session.Catalog.GetEncounter("enc_nc_syndicate_bribe_overture"));
        }

        [Fact]
        public void Verdict_16Questlines_9Npcs_AuthenticationsAndAppeals_Integrates()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Questlines
            string qlPath = Path.Combine(dataDir, "verdict_questlines.json");
            var qlDoc = json.Deserialize<VerdictQuestlinesDto>(files.ReadAllText(qlPath));
            Assert.NotNull(qlDoc);
            Assert.NotNull(qlDoc.quests);
            Assert.True(qlDoc.quests.Count >= 16, $"Expected at least 16 questlines, got {qlDoc.quests.Count}");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_alibi_verification");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_witness_subpoena");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_charter_authentication");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_prior_verdict_appeal");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_chain_of_custody");
            Assert.Contains(qlDoc.quests, q => q.questlineId == "quest_verdict_machine_interpretation_contest");

            // NPCs
            string npcPath = Path.Combine(dataDir, "verdict_npcs.json");
            var npcDoc = json.Deserialize<VerdictNpcsDto>(files.ReadAllText(npcPath));
            Assert.NotNull(npcDoc);
            Assert.NotNull(npcDoc.items);
            Assert.True(npcDoc.items.Count >= 9, $"Expected at least 9 NPCs, got {npcDoc.items.Count}");
            Assert.Contains(npcDoc.items, n => n.id == "npc_tomas_reid");
            Assert.Contains(npcDoc.items, n => n.id == "npc_elena_vane");
            Assert.Contains(npcDoc.items, n => n.id == "npc_kasper_holt");
        }

        [Fact]
        public void QuestlineMaster_ContainsAllAuthoritativeQuestIds()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            string masterPath = Path.Combine(dataDir, "questline_master.json");
            var master = json.Deserialize<QuestlineMasterDto>(files.ReadAllText(masterPath));
            Assert.NotNull(master);
            Assert.NotNull(master.entries);
            Assert.True(master.entries.Count >= 400, $"Expected at least 400 questline master entries, got {master.entries.Count}");

            var masterSet = new HashSet<string>(master.entries.Select(e => e.id), StringComparer.Ordinal);

            // Verify Holdfast quests exist in master
            var hf = new HoldfastCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
            foreach (var q in hf.Quests)
            {
                Assert.Contains(q.id, masterSet);
            }

            // Verify Standing Record quests exist in master
            var sr = new StandingRecordCatalogLoader(files, json, NullLog.Instance).Load(dataDir);
            foreach (var q in sr.Quests)
            {
                Assert.Contains(q.id, masterSet);
            }

            // Verify Crossing quests exist in master
            var cr = CrossingSession.Load(dataDir, NullLog.Instance);
            foreach (var q in cr.Catalog.Quests)
            {
                Assert.Contains(q.id, masterSet);
            }
        }

        [Fact]
        public void SaveRoundtrip_HoldfastAndStandingRecord_PreservesDeterministicState()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            // Holdfast save roundtrip
            var session = HoldfastSession.Load(dataDir, 808, expansionUnlocked: true, NullLog.Instance);
            session.Quests.TryStart("quest_holdfast_the_sheet", 90);
            session.Quests.TryStart("quest_holdfast_salt_convoy_haul", 95);
            var save = HoldfastSaveCodec.Capture(session.IceRoad, session.Census, session.Brine, session.Quests, new SimClock(95));
            string encoded = HoldfastSaveCodec.Encode(save, json);
            var loaded = HoldfastSaveCodec.Decode(encoded, json);
            Assert.NotNull(loaded);
            Assert.Contains("quest_holdfast_salt_convoy_haul", loaded.quests.quests.Select(q => q.questId));

            // Standing Record layout save roundtrip
            var layoutSys = new LocationLayoutSystem(files, json, NullLog.Instance);
            layoutSys.Load(dataDir);
            layoutSys.Unlock();
            layoutSys.ArriveAtParent(LocationLayoutSystem.LocKilometre19);
            layoutSys.EnterRoom(LocationLayoutSystem.RoomKm19Post);
            layoutSys.InspectRoom(LocationLayoutSystem.RoomKm19Post);

            var srState = layoutSys.CaptureState();
            string serializedSr = json.Serialize(srState);
            var restoredSr = json.Deserialize<LocationLayoutState>(serializedSr);
            Assert.NotNull(restoredSr);
            var parent = restoredSr.parents.FirstOrDefault(p => p.parentLocationId == LocationLayoutSystem.LocKilometre19);
            Assert.NotNull(parent);
            Assert.Contains(LocationLayoutSystem.RoomKm19Post, parent.inspectedRoomIds);
        }

        private sealed class VerdictQuestlinesDto
        {
            public int schema_version { get; set; }
            public List<VerdictQuestlineEntryDto>? quests { get; set; }
        }

        private sealed class VerdictQuestlineEntryDto
        {
            public string questlineId { get; set; } = string.Empty;
            public string title { get; set; } = string.Empty;
        }

        private sealed class VerdictNpcsDto
        {
            public int schema_version { get; set; }
            public List<VerdictNpcEntryDto>? items { get; set; }
        }

        private sealed class VerdictNpcEntryDto
        {
            public string id { get; set; } = string.Empty;
            public string name { get; set; } = string.Empty;
        }

        private sealed class QuestlineMasterDto
        {
            public int schema_version { get; set; }
            public List<QuestlineMasterEntryDto>? entries { get; set; }
        }

        private sealed class QuestlineMasterEntryDto
        {
            public string id { get; set; } = string.Empty;
        }
    }
}
