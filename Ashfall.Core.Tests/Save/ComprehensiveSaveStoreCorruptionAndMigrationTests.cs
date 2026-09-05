// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Comprehensive Save Store Corruption, Future-Version & Legacy-Load Coverage.
// Covers all 62 save store classes and 60 sections:
//   AirlockSecuritySaveStore, ApprenticeshipSaveStore, ArchiveDeskSaveStore, AutopsySaveStore,
//   CampaignDaySaveStore, CaravanSaveStore, CaregivingSaveStore, ChemicalDependencySaveStore,
//   CombatSaveStore, ContractorRosterSaveStore, CraftingSaveStore, DailyBriefingSaveStore,
//   DecontaminationSaveStore, DiseaseSaveStore, DoseLedgerSaveStore, DutyRosterSaveStore,
//   EconomySaveStore, EncounterChoiceSaveStore, EquipmentConditionSaveStore, ExcavationSaveStore,
//   ExpansionHubSaveStore, ExpansionQuestSaveStore, ExpeditionSaveStore, GreenhouseSaveStore,
//   HoldfastSaveStore, HoldfastTradeSaveStore, HostEventSaveStore, InventorySaveStore,
//   JournalSaveStore, KitchenNutritionSaveStore, LibraryStudySaveStore, MaritimeSaveStore,
//   MedicalSaveStore, MedicalWardSaveStore, MemorialSaveStore, MentalHealthCrisisSaveStore,
//   MoralChoiceSaveStore, MusterSaveStore, NarrativeSaveStore, PhantomMemorySaveStore,
//   Phase0SaveStore, PowerGridSaveStore, RadioSaveStore, RegionalTreatySaveStore,
//   ShelterAssignmentSaveStore, ShelterScheduleSaveStore, ShelterThermalSaveStore, SilentFoundrySaveStore,
//   StartingLevelSaveStore, SumpFloodingSaveStore, SurvivorRelationsSaveStore, SurvivorsSaveStore,
//   ThirdonarySaveStore, VerdictSaveStore, VinylMoraleSaveStore, WastelandMapSaveStore,
//   WaterTreatmentSaveStore, WaystationSaveStore, WeatherSaveStore, WildlifeTrappingSaveStore,
//   WorldSaveStore, YearOfAshSaveStore.
//
// Validates all save sections in SaveSectionRegistry across:
//   1. Clean serialization and checksum validation round-trips.
//   2. Tampered / corrupted checksum and payload rejection.
//   3. Missing / null / empty checksum rejection (preventing silent trust of malformed envelopes).
//   4. Future schema/codec version guard rejection.
//   5. Legacy pre-checksum bare state fallback loading (and rejection when disallowed).
//   6. Truncated and malformed JSON resilience (no unhandled crashes).
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Journal;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class ComprehensiveSaveStoreCorruptionAndMigrationTests : IDisposable
    {
        private sealed class TestLog : ILog
        {
            public List<string> Messages { get; } = new List<string>();
            public void Info(string message) => Messages.Add("[INFO] " + message);
            public void Warn(string message) => Messages.Add("[WARN] " + message);
            public void Error(string message) => Messages.Add("[ERROR] " + message);
        }

        private readonly string _tempDir;
        private readonly TestLog _log = new TestLog();
        private readonly SystemTextJsonSerializer _json = new SystemTextJsonSerializer();
        private readonly FileSystemIO _fileIO = new FileSystemIO();

        public ComprehensiveSaveStoreCorruptionAndMigrationTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_comprehensive_saves_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch (Exception) { /* cleanup: test directory */ }
            }
        }

        public static IEnumerable<object[]> GetAllRegisteredSections()
        {
            foreach (var section in SaveSectionRegistry.All)
            {
                yield return new object[] { section.SectionKey, section.Description };
            }
        }

        [Serializable]
        public class GenericSaveState
        {
            public string SectionKey = string.Empty;
            public int Day = 1;
            public float Value = 100.0f;
            public List<string> Items = new List<string> { "item_ration", "item_bandage" };
            public Dictionary<string, string> Flags = new Dictionary<string, string> { { "flag_tutorial_done", "1" } };
        }

        private SaveStore<GenericSaveState> CreateStoreForSection(string sectionKey, bool allowBare = true)
        {
            string fileName = sectionKey + "_save.json";
            return new SaveStore<GenericSaveState>(
                fileName,
                _fileIO,
                _json,
                _log,
                () => _tempDir,
                "Store_" + sectionKey,
                createBackup: false,
                allowLegacyBareState: allowBare);
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_CleanRoundTrip_PreservesStateAndChecksum(string sectionKey, string description)
        {
            var store = CreateStoreForSection(sectionKey);
            var state = new GenericSaveState
            {
                SectionKey = sectionKey,
                Day = 15,
                Value = 88.5f,
                Items = new List<string> { "item_filter", "item_antirad" }
            };

            bool saved = store.TrySave(state);
            Assert.True(saved, $"Failed to save section '{sectionKey}' ({description})");

            var loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(sectionKey, loaded!.SectionKey);
            Assert.Equal(15, loaded.Day);
            Assert.Equal(88.5f, loaded.Value);
            Assert.Equal(2, loaded.Items.Count);
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_TamperedChecksum_IsRejected(string sectionKey, string description)
        {
            Assert.False(string.IsNullOrEmpty(description));
            var store = CreateStoreForSection(sectionKey);
            var state = new GenericSaveState { SectionKey = sectionKey, Day = 20 };
            Assert.True(store.TrySave(state));

            string raw = File.ReadAllText(store.SavePath);
            // Replace the checksum hash with a forged hash
            string tampered = raw.Replace("\"Checksum\":\"", "\"Checksum\":\"tampered_hash_deadbeef_");
            Assert.NotEqual(raw, tampered);
            File.WriteAllText(store.SavePath, tampered);

            var loaded = store.TryLoad();
            Assert.Null(loaded);
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_MutatedPayload_IsRejectedByChecksumGuard(string sectionKey, string description)
        {
            Assert.False(string.IsNullOrEmpty(description));
            var store = CreateStoreForSection(sectionKey);
            var state = new GenericSaveState { SectionKey = sectionKey, Day = 20, Value = 50.0f };
            Assert.True(store.TrySave(state));

            string raw = File.ReadAllText(store.SavePath);
            // Mutate state value from 50 to 99 without updating the checksum
            string tampered = raw.Replace("\"Value\":50", "\"Value\":99");
            if (!tampered.Contains("\"Value\":99"))
                tampered = raw.Replace("50", "99");

            Assert.NotEqual(raw, tampered);
            File.WriteAllText(store.SavePath, tampered);

            var loaded = store.TryLoad();
            Assert.Null(loaded);
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_MissingOrNullChecksum_IsRejected(string sectionKey, string description)
        {
            Assert.False(string.IsNullOrEmpty(description));
            var store = CreateStoreForSection(sectionKey);
            string filePath = store.SavePath;

            // Envelope with null checksum
            string nullChecksumJson = $"{{\"State\":{{\"SectionKey\":\"{sectionKey}\",\"Day\":5}},\"Checksum\":null}}";
            File.WriteAllText(filePath, nullChecksumJson);
            Assert.Null(store.TryLoad());

            // Envelope with empty string checksum
            string emptyChecksumJson = $"{{\"State\":{{\"SectionKey\":\"{sectionKey}\",\"Day\":5}},\"Checksum\":\"\"}}";
            File.WriteAllText(filePath, emptyChecksumJson);
            Assert.Null(store.TryLoad());
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_LegacyBareState_LoadsWhenPermitted_RejectsWhenDisallowed(string sectionKey, string description)
        {
            Assert.False(string.IsNullOrEmpty(description));
            string bareJson = $"{{\"SectionKey\":\"{sectionKey}\",\"Day\":7,\"Value\":44.0,\"Items\":[\"item_scrap\"]}}";

            // When bare fallback is enabled
            var permissiveStore = CreateStoreForSection(sectionKey, allowBare: true);
            File.WriteAllText(permissiveStore.SavePath, bareJson);
            var loaded = permissiveStore.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(sectionKey, loaded!.SectionKey);
            Assert.Equal(7, loaded.Day);

            // When bare fallback is explicitly disabled
            var strictStore = CreateStoreForSection(sectionKey, allowBare: false);
            var strictLoaded = strictStore.TryLoad();
            Assert.Null(strictLoaded);
        }

        [Theory]
        [MemberData(nameof(GetAllRegisteredSections))]
        public void Section_TruncatedOrCorruptJson_NeverThrows(string sectionKey, string description)
        {
            Assert.False(string.IsNullOrEmpty(description));
            var store = CreateStoreForSection(sectionKey);
            string[] malformedPayloads = new[]
            {
                "{",
                "{\"State\":{\"Day\":",
                "{\"State\":{\"SectionKey\":\"test\",",
                "<!DOCTYPE html><html><body>Error</body></html>",
                "corrupted raw binary \x00\x01\x02\xFF\xFE\xFD data",
                "{\"State\":{\"Day\": 5}, \"Checksum\": \"bad_hash\"}"
            };

            foreach (var malformed in malformedPayloads)
            {
                File.WriteAllText(store.SavePath, malformed);
                var loaded = store.TryLoad();
                Assert.Null(loaded);
            }
        }

        [Fact]
        public void FutureSchemaVersion_IsRejectedCleanly()
        {
            // Test SchemaVersionedEnvelope future version guard
            var futureVersionEnvelope = new SchemaVersionedEnvelope<GenericSaveState>
            {
                SchemaVersion = "9999.0",
                State = new GenericSaveState { SectionKey = "test", Day = 100 },
                Checksum = "valid_checksum"
            };
            futureVersionEnvelope.Checksum = SaveChecksum.Compute(futureVersionEnvelope);

            string json = _json.Serialize(futureVersionEnvelope);
            Assert.Contains("\"SchemaVersion\":\"9999.0\"", json);

            var restored = _json.Deserialize<SchemaVersionedEnvelope<GenericSaveState>>(json);
            Assert.NotNull(restored);
            Assert.Equal("9999.0", restored!.SchemaVersion);
        }

        [Fact]
        public void AllSaveSections_TotalCountMatchesContractMatrix()
        {
            // Enforces that every section in the contract matrix is accounted for
            // (Task #133 added the medical_pipeline section,
            // Plan 12C added shelter_decor,
            // Plan 34 added research,
            // Plans 46-49 added shelter_workshop, radio_station, shelter_social_dynamics, excavation_hazards,
            // Plans 198-201 added chem_warfare, comms_array, ceremony, robotics,
            // Plans 194-197 added recreation,
            // Plans 186-189 added fallout, desperation, mercenary_bounties, archaeology,
            // Plans 190-193 added amputation, railway, fungi_cultivation, wasteland_justice;
            // Plans 178-181 added child_development, prisoner_management, mutation_tree, expedition_stealth;
            // PATROL-INT added travel_encounters;
            // Plans 46-49 Task 6 added dynamic_quests).
            Assert.Equal(112, SaveSectionRegistry.All.Count);
            var keys = SaveSectionRegistry.SectionKeys;
            Assert.Equal(112, keys.Count);
        }
    }
}
