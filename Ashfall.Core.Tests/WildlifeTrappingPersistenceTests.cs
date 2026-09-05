// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingPersistenceTests : IDisposable
    {
        private readonly string _tempDir;

        public WildlifeTrappingPersistenceTests()
        {
            _tempDir = Path.Combine(Path.GetTempPath(), "ashfall_trapping_persist_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(_tempDir);
        }

        public void Dispose()
        {
            if (Directory.Exists(_tempDir))
            {
                try { Directory.Delete(_tempDir, true); } catch { }
            }
        }

        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void CaptureSerializeDeserialize_PartiallyWornTrap_PreservesNewFields()
        {
            // 1. Build system with catalog registration
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // 2. Select trap definition with finite durability (trap_snare: durabilityChecks = 8, checkIntervalDays = 2)
            var trapDef = catalog.Traps["trap_snare"];
            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_dweller",
                trapType: trapDef.trapType, trapId: trapDef.trap_id,
                checkIntervalDays: trapDef.checkIntervalDays, durabilityChecks: trapDef.durabilityChecks);

            // 3. Advance days to consume durability partially
            sys.TickDay(3); // Day 3 > checkDay 3, 1 check performed
            sys.TickDay(5); // Day 5 > checkDay 5, 2nd check performed

            var site = sys.State.trapSites[0];
            Assert.Equal("trap_snare", site.trapId);
            Assert.Equal(6, site.remainingDurability); // 8 initial - 2 checks = 6
            Assert.False(site.isBroken);

            // Include diseaseId and contaminationDose on the site
            site.hasCatch = true;
            site.catchSpecies = "ash_crow";
            site.diseaseId = "disease_blood_fever";
            site.contaminationDose = 6.0f;

            // 4. Capture & Serialize
            var captured = sys.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(captured);

            // 5. Assert wire format includes all 5 fields
            Assert.Contains("\"trapId\"", json);
            Assert.Contains("\"remainingDurability\"", json);
            Assert.Contains("\"isBroken\"", json);
            Assert.Contains("\"diseaseId\"", json);
            Assert.Contains("\"contaminationDose\"", json);

            // Structural JSON check
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var sites = root.GetProperty("trapSites");
            Assert.True(sites.GetArrayLength() > 0);
            var firstSite = sites[0];
            Assert.Equal("trap_snare", firstSite.GetProperty("trapId").GetString());
            Assert.Equal(6, firstSite.GetProperty("remainingDurability").GetInt32());
            Assert.False(firstSite.GetProperty("isBroken").GetBoolean());
            Assert.Equal("disease_blood_fever", firstSite.GetProperty("diseaseId").GetString());
            Assert.Equal(6.0f, firstSite.GetProperty("contaminationDose").GetSingle());

            // 6. Deserialize into fresh state
            var restored = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restored);
            var restoredSite = restored!.trapSites[0];
            Assert.Equal("trap_snare", restoredSite.trapId);
            Assert.Equal(6, restoredSite.remainingDurability);
            Assert.False(restoredSite.isBroken);
            Assert.Equal("disease_blood_fever", restoredSite.diseaseId);
            Assert.Equal(6.0f, restoredSite.contaminationDose);
            Assert.True(restoredSite.hasCatch);
            Assert.Equal("ash_crow", restoredSite.catchSpecies);
        }

        [Fact]
        public void CaptureSerializeDeserialize_BrokenTrap_PreservesBrokenState()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Deploy improvised wire snare (durabilityChecks = 3, checkIntervalDays = 1)
            var trapDef = catalog.Traps["trap_improvised_wire"];
            sys.SetTrap("site_wire_broken", "bait_scrap_meat", "hunter_dweller",
                trapType: trapDef.trapType, trapId: trapDef.trap_id,
                checkIntervalDays: 1, durabilityChecks: 3);

            // Run 3 checks to break it (clearing any catch between days so subsequent checks execute)
            sys.TickDay(2);
            sys.State.trapSites[0].hasCatch = false;
            sys.TickDay(3);
            sys.State.trapSites[0].hasCatch = false;
            sys.TickDay(4);

            var site = sys.State.trapSites[0];
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);

            var captured = sys.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(captured);

            var restored = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(restored);
            var restoredSite = restored!.trapSites[0];
            Assert.Equal(0, restoredSite.remainingDurability);
            Assert.True(restoredSite.isBroken);
        }

        [Fact]
        public void Deserialize_LegacyTrapWithoutDurabilityFields_UsesFunctionalDefaults()
        {
            // Legacy JSON payload omitting trapId, remainingDurability, isBroken, diseaseId, contaminationDose
            string legacyJson = @"{
  ""systemId"": ""wildlife_trapping"",
  ""totalCatch"": 3,
  ""totalToxicRemoved"": 1,
  ""firstCatchLoggedSpeciesIds"": [""rabbit""],
  ""trapSites"": [
    {
      ""siteId"": ""site_legacy_1"",
      ""assignedHunterId"": ""dweller_1"",
      ""baitType"": ""bait_grain_lure"",
      ""trapType"": ""snare"",
      ""setDay"": 1,
      ""checkDay"": 2,
      ""checkIntervalDays"": 2,
      ""hasCatch"": false,
      ""catchSpecies"": """",
      ""bycatchSpecies"": """",
      ""carcassYield"": 0.0,
      ""isToxic"": false,
      ""toxinRemoved"": false,
      ""isMeatProcessed"": false,
      ""hidePreserved"": false
    }
  ]
}";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(legacyJson);
            Assert.NotNull(state);
            Assert.Single(state!.trapSites);

            var site = state.trapSites[0];
            // Backward-compatibility contract assertions:
            // trapId -> empty string
            Assert.Equal(string.Empty, site.trapId);
            // remainingDurability -> -1 (legacy functional sentinel)
            Assert.Equal(-1, site.remainingDurability);
            // isBroken -> false
            Assert.False(site.isBroken);
            // diseaseId -> empty string
            Assert.Equal(string.Empty, site.diseaseId);
            // contaminationDose -> 0
            Assert.Equal(0f, site.contaminationDose);

            // Restore into fresh WildlifeTrappingSystem
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(state);

            // Legacy trap must remain functional: advance to checkDay 2 and verify it can perform checks
            sys.TickDay(2);
            var restoredSite = sys.State.trapSites[0];
            Assert.False(restoredSite.isBroken, "Legacy trap should remain operational without catalog durability");
            Assert.Equal(-1, restoredSite.remainingDurability);
        }

        [Fact]
        public void RestoreLegacyThenCapture_EmitsCurrentTrapFields()
        {
            // Legacy omission loads as functional sentinel state; next save emits the current schema fields.
            string legacyJson = @"{
  ""systemId"": ""wildlife_trapping"",
  ""trapSites"": [
    {
      ""siteId"": ""site_legacy_2"",
      ""baitType"": ""bait_grain_lure"",
      ""trapType"": ""snare"",
      ""setDay"": 1,
      ""checkDay"": 3,
      ""checkIntervalDays"": 2
    }
  ]
}";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(legacyJson);
            Assert.NotNull(state);

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(state!);

            var reCaptured = sys.CaptureState();
            string reSerialized = serializer.Serialize(reCaptured);

            // Assert new fields now appear explicitly with normalized defaults
            Assert.Contains("\"trapId\"", reSerialized);
            Assert.Contains("\"remainingDurability\"", reSerialized);
            Assert.Contains("\"isBroken\"", reSerialized);
            Assert.Contains("\"diseaseId\"", reSerialized);
            Assert.Contains("\"contaminationDose\"", reSerialized);

            using var doc = JsonDocument.Parse(reSerialized);
            var siteElem = doc.RootElement.GetProperty("trapSites")[0];
            Assert.Equal(string.Empty, siteElem.GetProperty("trapId").GetString());
            Assert.Equal(-1, siteElem.GetProperty("remainingDurability").GetInt32());
            Assert.False(siteElem.GetProperty("isBroken").GetBoolean());
            Assert.Equal(string.Empty, siteElem.GetProperty("diseaseId").GetString());
            Assert.Equal(0f, siteElem.GetProperty("contaminationDose").GetSingle());
        }

        [Fact]
        public void WildlifeTrappingSaveStore_RoundTrip_PreservesDiseaseContaminationAndTrapFields()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var log = new NullLog();

            // Instantiate SaveStore<WildlifeTrappingState> using the exact SchemaVersionedEnvelope codec that WildlifeTrappingSaveStore uses
            var store = SaveStore<WildlifeTrappingState>.FromCodec(
                "wildlife_trapping_save.json",
                fileIO,
                json,
                log,
                () => _tempDir,
                "WildlifeTrappingSaveStore",
                SchemaVersionedEnvelope<WildlifeTrappingState>.Encode,
                SchemaVersionedEnvelope<WildlifeTrappingState>.Decode);

            var state = new WildlifeTrappingState
            {
                totalCatch = 5,
                totalToxicRemoved = 2,
                firstCatchLoggedSpeciesIds = new List<string> { "rat", "pheasant" },
                trapSites = new List<TrapSite>
                {
                    new TrapSite
                    {
                        siteId = "site_worn_1",
                        trapId = "trap_snare",
                        trapType = "snare",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "hunter_a",
                        setDay = 2,
                        checkDay = 4,
                        checkIntervalDays = 2,
                        remainingDurability = 5,
                        isBroken = false,
                        hasCatch = true,
                        catchSpecies = "rat",
                        diseaseId = "disease_typhoid_waterborne",
                        contaminationDose = 4.0f
                    },
                    new TrapSite
                    {
                        siteId = "site_broken_1",
                        trapId = "trap_improvised_wire",
                        trapType = "improvised_wire",
                        baitType = "bait_scrap_meat",
                        assignedHunterId = "hunter_b",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 1,
                        remainingDurability = 0,
                        isBroken = true,
                        hasCatch = false,
                        diseaseId = string.Empty,
                        contaminationDose = 0f
                    }
                }
            };

            // Save through production store
            bool saved = store.TrySave(state);
            Assert.True(saved);
            Assert.True(store.Exists());

            // Reload through production store
            var loaded = store.TryLoad();
            Assert.NotNull(loaded);
            Assert.Equal(5, loaded!.totalCatch);
            Assert.Equal(2, loaded.totalToxicRemoved);
            Assert.Equal(2, loaded.trapSites.Count);

            // Worn trap assertions
            var worn = loaded.trapSites[0];
            Assert.Equal("site_worn_1", worn.siteId);
            Assert.Equal("trap_snare", worn.trapId);
            Assert.Equal(5, worn.remainingDurability);
            Assert.False(worn.isBroken);
            Assert.True(worn.hasCatch);
            Assert.Equal("rat", worn.catchSpecies);
            Assert.Equal("disease_typhoid_waterborne", worn.diseaseId);
            Assert.Equal(4.0f, worn.contaminationDose);

            // Broken trap assertions
            var broken = loaded.trapSites[1];
            Assert.Equal("site_broken_1", broken.siteId);
            Assert.Equal("trap_improvised_wire", broken.trapId);
            Assert.Equal(0, broken.remainingDurability);
            Assert.True(broken.isBroken);
            Assert.False(broken.hasCatch);
            Assert.Equal(string.Empty, broken.diseaseId);
            Assert.Equal(0f, broken.contaminationDose);
        }
    }
}
