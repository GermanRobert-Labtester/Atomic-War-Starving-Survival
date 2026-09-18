// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Tests.Fixtures;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    /// <summary>
    /// Plan 27A.8 &amp; 27A.9 — Golden Save Fixtures &amp; Determinism Digest Test.
    /// Pins early (day 1), mid (day 15), and late (day 60) campaign save fixtures
    /// with checksum verification and tamper-detection against artifacts/golden_saves/.
    /// </summary>
    public sealed class GoldenSaveFixtureTests
    {
        private static readonly string GoldenSavesDir = ResolveGoldenSavesDir();

        public GoldenSaveFixtureTests()
        {
            EnsureFixturesExist();
        }

        private static string ResolveGoldenSavesDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string dir = baseDir;
            for (int i = 0; i < 8; i++)
            {
                string probe = Path.Combine(dir, "artifacts", "golden_saves");
                if (Directory.Exists(Path.Combine(dir, "artifacts")))
                    return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return Path.Combine(baseDir, "artifacts", "golden_saves");
        }

        private static void EnsureFixturesExist()
        {
            Directory.CreateDirectory(GoldenSavesDir);
            var earlyPath = Path.Combine(GoldenSavesDir, "early_campaign.json");
            var midPath = Path.Combine(GoldenSavesDir, "mid_campaign.json");
            var latePath = Path.Combine(GoldenSavesDir, "late_campaign.json");
            var manifestPath = Path.Combine(GoldenSavesDir, "manifest.json");

            if (File.Exists(earlyPath) && File.Exists(midPath) && File.Exists(latePath) && File.Exists(manifestPath))
                return;

            var json = new SystemTextJsonSerializer();

            // 1. Early Campaign (Day 1)
            var earlyManifest = new SaveManifest
            {
                manifestVersion = 1,
                gameVersion = "0.9.0",
                buildId = "golden-early",
                currentDay = 1,
                seed = 1337,
                lastSaveTick = 100,
                mode = CampaignMode.Normal,
                slotId = new SaveSlotId("golden-early"),
                profileId = new SaveProfileId("golden-profile"),
                campaignName = "Golden Early Campaign",
                ironManTerminalState = IronManTerminalState.Active,
                lastSaveTimestamp = "2026-09-17T00:00:00Z",
                generationId = "gen_golden_early_100"
            };
            var earlyPayloads = new Dictionary<string, string>
            {
                ["campaign_day"] = "{\"saveVersion\":1,\"lastAdvancedDay\":1,\"masterSeed\":1337,\"derivationVersion\":1,\"streamPositions\":{}}",
                ["inventory"] = "{\"capacity\":50,\"maxWeight\":100.0,\"slots\":[{\"itemId\":\"canned_food\",\"amount\":10,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0},{\"itemId\":\"clean_water\",\"amount\":10,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0}],\"equipped\":[]}",
                ["power_grid"] = "{\"saveVersion\":1,\"simDay\":1,\"Rooms\":[],\"State\":{\"FuelReserve\":50.0,\"BatteryCapacity\":1000.0,\"BatteryStored\":800.0}}",
                ["starting_level"] = "{\"tier\":1,\"powerOnline\":true,\"defenseRating\":10}",
                ["journal"] = "{\"Entries\":[{\"Id\":\"entry_01\",\"Day\":1,\"Title\":\"Arrival\",\"Body\":\"The bunker gates close behind us.\"}],\"NextSeq\":2,\"HasUnread\":false,\"NotificationPing\":false,\"NotificationPingCount\":0,\"HudIsOpen\":false,\"ActiveTab\":0,\"LastSeenIndexPerTab\":[0,0,0,0],\"LastSeenCodexPerTab\":[0,0,0,0],\"CodexUnlockCount\":0}"
            };
            var earlyEnvelope = CampaignEnvelopeBuilder.Build(earlyPayloads, earlyManifest);
            string earlyJson = json.Serialize(earlyEnvelope);
            File.WriteAllText(earlyPath, earlyJson);

            // 2. Mid Campaign (Day 15)
            var midManifest = new SaveManifest
            {
                manifestVersion = 1,
                gameVersion = "0.9.0",
                buildId = "golden-mid",
                currentDay = 15,
                seed = 1337,
                lastSaveTick = 1500,
                mode = CampaignMode.Normal,
                slotId = new SaveSlotId("golden-mid"),
                profileId = new SaveProfileId("golden-profile"),
                campaignName = "Golden Mid Campaign",
                ironManTerminalState = IronManTerminalState.Active,
                lastSaveTimestamp = "2026-09-17T15:00:00Z",
                generationId = "gen_golden_mid_1500"
            };
            var midPayloads = new Dictionary<string, string>
            {
                ["campaign_day"] = "{\"saveVersion\":1,\"lastAdvancedDay\":15,\"masterSeed\":1337,\"derivationVersion\":1,\"streamPositions\":{\"rations\":15}}",
                ["inventory"] = "{\"capacity\":60,\"maxWeight\":120.0,\"slots\":[{\"itemId\":\"canned_food\",\"amount\":4,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0},{\"itemId\":\"clean_water\",\"amount\":6,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0},{\"itemId\":\"scrap_metal\",\"amount\":12,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0}],\"equipped\":[{\"itemId\":\"gas_mask\",\"durability\":85.0}]}",
                ["power_grid"] = "{\"saveVersion\":1,\"simDay\":15,\"Rooms\":[],\"State\":{\"FuelReserve\":28.0,\"BatteryCapacity\":1000.0,\"BatteryStored\":450.0}}",
                ["starting_level"] = "{\"tier\":1,\"powerOnline\":true,\"defenseRating\":10}",
                ["journal"] = "{\"Entries\":[{\"Id\":\"entry_01\",\"Day\":1,\"Title\":\"Arrival\",\"Body\":\"The bunker gates close behind us.\"},{\"Id\":\"entry_02\",\"Day\":7,\"Title\":\"First Dust Storm\",\"Body\":\"Filters held up against the fallout.\"}],\"NextSeq\":3,\"HasUnread\":false,\"NotificationPing\":false,\"NotificationPingCount\":0,\"HudIsOpen\":false,\"ActiveTab\":0,\"LastSeenIndexPerTab\":[0,0,0,0],\"LastSeenCodexPerTab\":[0,0,0,0],\"CodexUnlockCount\":1}",
                ["crafting"] = "{\"knownRecipes\":[\"recipe_filter\",\"recipe_bandage\"],\"activeQueues\":[]}",
                ["caravan"] = "{\"visitedCaravans\":1,\"lastArrivalDay\":10,\"activeRoute\":\"dust_road\"}"
            };
            var midEnvelope = CampaignEnvelopeBuilder.Build(midPayloads, midManifest);
            string midJson = json.Serialize(midEnvelope);
            File.WriteAllText(midPath, midJson);

            // 3. Late Campaign (Day 60)
            var lateManifest = new SaveManifest
            {
                manifestVersion = 1,
                gameVersion = "0.9.0",
                buildId = "golden-late",
                currentDay = 60,
                seed = 1337,
                lastSaveTick = 6000,
                mode = CampaignMode.Normal,
                slotId = new SaveSlotId("golden-late"),
                profileId = new SaveProfileId("golden-profile"),
                campaignName = "Golden Late Campaign",
                ironManTerminalState = IronManTerminalState.Active,
                lastSaveTimestamp = "2026-09-17T20:00:00Z",
                generationId = "gen_golden_late_6000"
            };
            var latePayloads = new Dictionary<string, string>
            {
                ["campaign_day"] = "{\"saveVersion\":1,\"lastAdvancedDay\":60,\"masterSeed\":1337,\"derivationVersion\":1,\"streamPositions\":{\"rations\":60,\"weather\":120}}",
                ["inventory"] = "{\"capacity\":80,\"maxWeight\":160.0,\"slots\":[{\"itemId\":\"canned_food\",\"amount\":22,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0},{\"itemId\":\"clean_water\",\"amount\":35,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0},{\"itemId\":\"medkit\",\"amount\":5,\"hasDevice\":false,\"battery\":0.0,\"calibration\":0.0,\"broken\":false,\"lastCalibratedDay\":0}],\"equipped\":[{\"itemId\":\"hazmat_suit\",\"durability\":92.0}]}",
                ["power_grid"] = "{\"saveVersion\":1,\"simDay\":60,\"Rooms\":[],\"State\":{\"FuelReserve\":110.0,\"BatteryCapacity\":2500.0,\"BatteryStored\":2100.0}}",
                ["starting_level"] = "{\"tier\":2,\"powerOnline\":true,\"defenseRating\":25}",
                ["journal"] = "{\"Entries\":[{\"Id\":\"entry_01\",\"Day\":1,\"Title\":\"Arrival\",\"Body\":\"The bunker gates close behind us.\"},{\"Id\":\"entry_03\",\"Day\":30,\"Title\":\"Hydroponics Online\",\"Body\":\"First harvest yielded fresh wheat.\"},{\"Id\":\"entry_04\",\"Day\":55,\"Title\":\"Beacon Active\",\"Body\":\"Transmitting coordinates across the wastes.\"}],\"NextSeq\":5,\"HasUnread\":false,\"NotificationPing\":false,\"NotificationPingCount\":0,\"HudIsOpen\":false,\"ActiveTab\":0,\"LastSeenIndexPerTab\":[0,0,0,0],\"LastSeenCodexPerTab\":[0,0,0,0],\"CodexUnlockCount\":3}",
                ["crafting"] = "{\"knownRecipes\":[\"recipe_filter\",\"recipe_bandage\",\"recipe_hazmat_patch\",\"recipe_advanced_ammo\"],\"activeQueues\":[]}",
                ["caravan"] = "{\"visitedCaravans\":6,\"lastArrivalDay\":52,\"activeRoute\":\"northern_pass\"}",
                ["greenhouse"] = "{\"saveId\":\"greenhouse\",\"preWarWheatUnlocked\":true,\"totalHarvests\":4,\"blightRollCount\":12,\"plots\":[]}",
                ["narrative"] = "{\"flags\":{\"beacon_online\":true,\"convo_warden_pact\":true,\"refugee_covenant\":true},\"resolvedArcs\":[\"arc_water_crisis\"]}"
            };
            var lateEnvelope = CampaignEnvelopeBuilder.Build(latePayloads, lateManifest);
            string lateJson = json.Serialize(lateEnvelope);
            File.WriteAllText(latePath, lateJson);

            // 4. Manifest
            string manifestJson = $@"{{
  ""schema_version"": 1,
  ""description"": ""Golden save fixtures for early, mid, and late campaign regression and load fidelity testing."",
  ""fixtures"": [
    {{
      ""fixture_name"": ""early_campaign.json"",
      ""save_version"": 2,
      ""seed"": 1337,
      ""day"": 1,
      ""mode"": ""Normal"",
      ""required_sections"": [""campaign_day"", ""inventory"", ""power_grid"", ""starting_level"", ""journal""],
      ""expected_checksum"": ""{earlyEnvelope.aggregateChecksum}""
    }},
    {{
      ""fixture_name"": ""mid_campaign.json"",
      ""save_version"": 2,
      ""seed"": 1337,
      ""day"": 15,
      ""mode"": ""Normal"",
      ""required_sections"": [""campaign_day"", ""inventory"", ""power_grid"", ""starting_level"", ""journal"", ""crafting"", ""caravan""],
      ""expected_checksum"": ""{midEnvelope.aggregateChecksum}""
    }},
    {{
      ""fixture_name"": ""late_campaign.json"",
      ""save_version"": 2,
      ""seed"": 1337,
      ""day"": 60,
      ""mode"": ""Normal"",
      ""required_sections"": [""campaign_day"", ""inventory"", ""power_grid"", ""starting_level"", ""journal"", ""crafting"", ""caravan"", ""greenhouse"", ""narrative""],
      ""expected_checksum"": ""{lateEnvelope.aggregateChecksum}""
    }}
  ]
}}";
            File.WriteAllText(manifestPath, manifestJson);
        }

        private static SaveSlotService NewSaveSlotService() =>
            new SaveSlotService(new FileSystemIO(), new SystemTextJsonSerializer(), new ConsoleLog(), GoldenSavesDir);

        [Fact]
        public void EarlyCampaign_GoldenFixture_LoadsAndValidatesChecksum()
        {
            var path = Path.Combine(GoldenSavesDir, "early_campaign.json");
            Assert.True(File.Exists(path), $"Fixture '{path}' must exist.");

            var json = new SystemTextJsonSerializer();
            var text = File.ReadAllText(path);
            var envelope = json.Deserialize<AggregateSaveEnvelope>(text);

            Assert.NotNull(envelope);
            Assert.Equal(1, envelope!.manifest.currentDay);
            Assert.Equal(1337, envelope.manifest.seed);
            Assert.False(string.IsNullOrEmpty(envelope.aggregateChecksum));

            var service = NewSaveSlotService();
            var val = service.ValidateAggregate(envelope);
            Assert.True(val.IsValid, "Early campaign fixture failed validation: " + string.Join(", ", val.Errors));
            Assert.Equal(envelope.aggregateChecksum, SaveSlotService.ComputeAggregateChecksum(envelope));
        }

        [Fact]
        public void MidCampaign_GoldenFixture_LoadsAndValidatesChecksum()
        {
            var path = Path.Combine(GoldenSavesDir, "mid_campaign.json");
            Assert.True(File.Exists(path), $"Fixture '{path}' must exist.");

            var json = new SystemTextJsonSerializer();
            var text = File.ReadAllText(path);
            var envelope = json.Deserialize<AggregateSaveEnvelope>(text);

            Assert.NotNull(envelope);
            Assert.Equal(15, envelope!.manifest.currentDay);
            Assert.Equal(1337, envelope.manifest.seed);

            var service = NewSaveSlotService();
            var val = service.ValidateAggregate(envelope);
            Assert.True(val.IsValid, "Mid campaign fixture failed validation: " + string.Join(", ", val.Errors));
            Assert.Equal(envelope.aggregateChecksum, SaveSlotService.ComputeAggregateChecksum(envelope));
        }

        [Fact]
        public void LateCampaign_GoldenFixture_LoadsAndValidatesChecksum()
        {
            var path = Path.Combine(GoldenSavesDir, "late_campaign.json");
            Assert.True(File.Exists(path), $"Fixture '{path}' must exist.");

            var json = new SystemTextJsonSerializer();
            var text = File.ReadAllText(path);
            var envelope = json.Deserialize<AggregateSaveEnvelope>(text);

            Assert.NotNull(envelope);
            Assert.Equal(60, envelope!.manifest.currentDay);
            Assert.Equal(1337, envelope.manifest.seed);

            var service = NewSaveSlotService();
            var val = service.ValidateAggregate(envelope);
            Assert.True(val.IsValid, "Late campaign fixture failed validation: " + string.Join(", ", val.Errors));
            Assert.Equal(envelope.aggregateChecksum, SaveSlotService.ComputeAggregateChecksum(envelope));
        }

        [Fact]
        public void Manifest_MatchesAllGoldenFixtures()
        {
            var manifestPath = Path.Combine(GoldenSavesDir, "manifest.json");
            Assert.True(File.Exists(manifestPath), $"Manifest '{manifestPath}' must exist.");

            var text = File.ReadAllText(manifestPath);
            using var doc = JsonDocument.Parse(text);
            var root = doc.RootElement;
            Assert.Equal(1, root.GetProperty("schema_version").GetInt32());

            var fixtures = root.GetProperty("fixtures");
            var json = new SystemTextJsonSerializer();

            foreach (var element in fixtures.EnumerateArray())
            {
                string fixtureName = element.GetProperty("fixture_name").GetString()!;
                string expectedHash = element.GetProperty("expected_checksum").GetString()!;
                int expectedDay = element.GetProperty("day").GetInt32();
                int expectedSeed = element.GetProperty("seed").GetInt32();

                var fixturePath = Path.Combine(GoldenSavesDir, fixtureName);
                Assert.True(File.Exists(fixturePath), $"Referenced fixture '{fixturePath}' must exist.");

                var fileText = File.ReadAllText(fixturePath);
                var envelope = json.Deserialize<AggregateSaveEnvelope>(fileText)!;
                Assert.NotNull(envelope);
                Assert.Equal(expectedDay, envelope.manifest.currentDay);
                Assert.Equal(expectedSeed, envelope.manifest.seed);
                Assert.Equal(expectedHash, envelope.aggregateChecksum);

                // Required sections check
                var requiredSections = element.GetProperty("required_sections");
                foreach (var req in requiredSections.EnumerateArray())
                {
                    string secName = req.GetString()!;
                    Assert.Contains(envelope.sections, s => s.sectionName == secName);
                }
            }
        }

        [Fact]
        public void TamperedPayload_FailsChecksumValidation()
        {
            var path = Path.Combine(GoldenSavesDir, "early_campaign.json");
            var json = new SystemTextJsonSerializer();
            var text = File.ReadAllText(path);
            var envelope = json.Deserialize<AggregateSaveEnvelope>(text)!;

            // Mutate inventory section payload
            var invSection = envelope.sections.Find(s => s.sectionName == "inventory")!;
            invSection.payloadJson = invSection.payloadJson.Replace("\"canned_food\"", "\"tampered_item\"");

            var val = NewSaveSlotService().ValidateAggregate(envelope);
            Assert.False(val.IsValid, "Tampered envelope must fail validation.");
            Assert.Contains(val.Errors, e => e.Contains("checksum", StringComparison.OrdinalIgnoreCase) ||
                                            val.SectionErrors.Exists(se => se.Contains("checksum", StringComparison.OrdinalIgnoreCase)));
        }

        [Fact]
        public void CampaignFixture_DeterminismDigest_IsReproducible()
        {
            var dataDir = CampaignFixture.ResolveAuthorityDataDir();
            var f1 = CampaignFixture.CreateAuthorityBacked(dataDir, seed: 1337);
            var f2 = CampaignFixture.CreateAuthorityBacked(dataDir, seed: 1337);

            f1.PopulateStandardStartingSupplies();
            f2.PopulateStandardStartingSupplies();

            // Advance 5 days on both
            for (int i = 0; i < 5; i++)
            {
                f1.AdvanceDay();
                f2.AdvanceDay();
            }

            string digest1 = f1.ComputeStateDigest();
            string digest2 = f2.ComputeStateDigest();

            Assert.Equal(digest1, digest2);
            Assert.False(string.IsNullOrEmpty(digest1));
            Assert.Equal(6, f1.Calendar.CurrentDay);
        }
    }
}
