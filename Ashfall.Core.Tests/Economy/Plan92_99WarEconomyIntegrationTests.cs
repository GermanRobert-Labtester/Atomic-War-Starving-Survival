// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    /// <summary>
    /// Cross-system integration tests for Wave 40 Batch 4:
    /// - Plan 92 (DEC-261): Faction War Dialogue Expansion (18 -> 40 overheard dialogue snippets)
    /// - Plan 99 (DEC-262): Hardcore Economy Tuning Expansion (8 scarcity tiers, 8 faction prefs, 6 price shocks)
    ///
    /// Validates referential integrity, timeline gating, scarcity pricing curves,
    /// faction barter preferences, and narrative-economic coherence between
    /// wartime radio/dialogue chatter and regional market shocks.
    /// </summary>
    public sealed class Plan92_99WarEconomyIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(candidate)) return candidate;

            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }

            string current = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(current, out string found))
                return found;

            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data directory.");
        }

        [Fact]
        public void Plan92_FactionWarDialogue_LoadsAll40Snippets_WithValidStructureAndGating()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(io, json);

            FactionWarContentCatalog catalog = loader.Load(dataDir);

            Assert.NotNull(catalog);
            Assert.Equal(40, catalog.DialogueSnippetCount);
            Assert.Equal(40, catalog.DialogueSnippets.Count);

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var snippet in catalog.DialogueSnippets)
            {
                Assert.True(snippet.id.StartsWith("dlg_"), $"Snippet id must start with dlg_: {snippet.id}");
                Assert.False(string.IsNullOrWhiteSpace(snippet.locationId), $"locationId must not be empty for {snippet.id}");
                Assert.True(snippet.minDay > 0, $"minDay must be positive for {snippet.id}");
                Assert.False(string.IsNullOrWhiteSpace(snippet.speakerTag), $"speakerTag must not be empty for {snippet.id}");
                Assert.False(string.IsNullOrWhiteSpace(snippet.body), $"body must not be empty for {snippet.id}");
                Assert.True(seenIds.Add(snippet.id), $"Duplicate snippet id: {snippet.id}");
            }

            // Verify location-based querying with campaign day gating
            var gammaEarly = catalog.GetDialogueForLocation("loc_garrison_checkpoint_gamma", 100);
            Assert.Empty(gammaEarly);

            var gammaLate = catalog.GetDialogueForLocation("loc_garrison_checkpoint_gamma", 500);
            Assert.NotEmpty(gammaLate);
            Assert.Contains(gammaLate, s => s.id == "dlg_d482_checkpoint_quartermasters");
        }

        [Fact]
        public void Plan99_HardcoreEconomyTuning_LoadsAllTiersFactionPrefsAndPriceShocks()
        {
            string dataDir = ResolveDataDir();
            string jsonPath = Path.Combine(dataDir, "hardcore_economy_tuning.json");
            Assert.True(File.Exists(jsonPath));
            string rawJson = File.ReadAllText(jsonPath);

            var loadResult = HardcoreEconomyTuningLoader.Load(rawJson);
            Assert.True(loadResult.IsValid, string.Join("; ", loadResult.Errors));
            Assert.NotNull(loadResult.Bundle);

            var bundle = loadResult.Bundle!;
            Assert.Equal(8, bundle.ScarcityTiers.Count);
            Assert.Equal(8, bundle.FactionPreferences.Count);
            Assert.Equal(6, bundle.PriceShockRules.Count);

            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(bundle);
            Assert.True(tuning.IsActive);

            // 1. Scarcity multipliers across campaign progression
            // Day 5: Critical tier (2.5x) for clean water
            Assert.Equal(2.5f, tuning.GetScarcityMultiplier(5, "clean_water"));
            // Day 25: High tier (2.0x) for antibiotics
            Assert.Equal(2.0f, tuning.GetScarcityMultiplier(25, "antibiotics"));
            // Day 70: Moderate tier (1.6x) for scrap_mechanical
            Assert.Equal(1.6f, tuning.GetScarcityMultiplier(70, "scrap_mechanical"));
            // Day 130: Stable tier (1.3x) for seed stock
            Assert.Equal(1.3f, tuning.GetScarcityMultiplier(130, "seed_packets"));
            // Day 250: Late Scarcity tier (1.8x) for fuel and ammo wildcard
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "fuel"));
            Assert.Equal(1.8f, tuning.GetScarcityMultiplier(250, "ammo_9mm"));
            // Day 300: Deep Winter tier (2.2f) for clean water
            Assert.Equal(2.2f, tuning.GetScarcityMultiplier(300, "clean_water"));
            // Day 360: Endgame tier (2.4f) for dosimeter
            Assert.Equal(2.4f, tuning.GetScarcityMultiplier(360, "dosimeter"));

            // 2. Faction trade preferences
            Assert.True(tuning.TryGetFactionPreference("central_garrison_remnants", out var garrisonPref));
            Assert.Contains("fuel", garrisonPref.BuysAtPremium);
            Assert.Contains("jewelry", garrisonPref.Refuses);

            Assert.True(tuning.TryGetFactionPreference("faction_the_scale", out var scalePref));
            Assert.Contains("water_filter", scalePref.BuysAtPremium);

            Assert.True(tuning.TryGetFactionPreference("faction_the_rebuilders", out var rebuildersPref));
            Assert.Contains("seed_packets", rebuildersPref.BuysAtPremium);

            // 3. Price shock rules
            Assert.True(tuning.TryGetPriceShock(PriceShockKind.FactionConflict, 0, out var conflictShock));
            Assert.Equal(1.7f, conflictShock.Multiplier);
            Assert.Equal(5, conflictShock.DurationDays);

            Assert.True(tuning.TryGetPriceShock(PriceShockKind.FuelShortage, 1, out var fuelShock));
            Assert.Equal(1.9f, fuelShock.Multiplier);
            Assert.Equal(3, fuelShock.DurationDays);
        }

        [Fact]
        public void CrossSystem_WartimeDialogue_ReflectsEconomicScarcityAndPriceShocks()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var dialogLoader = new FactionWarContentCatalogLoader(io, json);
            var catalog = dialogLoader.Load(dataDir);

            string tuningJson = File.ReadAllText(Path.Combine(dataDir, "hardcore_economy_tuning.json"));
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(HardcoreEconomyTuningLoader.Load(tuningJson).Bundle!);

            // 1. Fuel scarcity, price shock, and garrison motor pool economics
            // Dialogue at depot reflects garrison fuel accounting, fuel shortage shock (1.9x), and prime mover maintenance
            var fuelDialogue = catalog.DialogueSnippets.FirstOrDefault(s => s.id == "dlg_d562_garrison_fuel_drum_tare");
            Assert.NotNull(fuelDialogue);
            Assert.Equal("loc_garrison_motor_pool", fuelDialogue.locationId);
            Assert.Contains("fuel depot", fuelDialogue.speakerTag);
            Assert.Contains("diesel", fuelDialogue.body);

            // Garrison faction pays premium for fuel
            Assert.True(tuning.TryGetFactionPreference("central_garrison_remnants", out var garrisonPref));
            Assert.Contains("fuel", garrisonPref.BuysAtPremium);

            // Fuel shortage price shock spikes fuel prices by 1.9x
            Assert.True(tuning.TryGetPriceShock(PriceShockKind.FuelShortage, 0, out var fuelShock));
            Assert.Equal(1.9f, fuelShock.Multiplier);
            Assert.Contains("fuel", fuelShock.AffectedItemIds);

            // Endgame tier (Days 341+) prices prime movers / engines at 2.4x
            float lateDayEngineMult = tuning.GetScarcityMultiplier(fuelDialogue.minDay, "engine");
            Assert.Equal(2.4f, lateDayEngineMult);

            // 2. Grain exchange and seed trade
            // Dialogue at grain silo reflects Rebuilders grain trade currency
            var siloDialogue = catalog.DialogueSnippets.FirstOrDefault(s => s.id == "dlg_d483_exchange_lean_pool");
            Assert.NotNull(siloDialogue);
            Assert.Equal("loc_grain_silo", siloDialogue.locationId);

            Assert.True(tuning.TryGetFactionPreference("faction_the_rebuilders", out var rebuilderPref));
            Assert.Contains("seed_packets", rebuilderPref.BuysAtPremium);
            Assert.Contains("Grain bushels", rebuilderPref.TradeCurrency);

            // 3. Garrison quartermasters and military supply reconciliations
            var garrisonDlg = catalog.DialogueSnippets.FirstOrDefault(s => s.id == "dlg_d482_checkpoint_quartermasters");
            Assert.NotNull(garrisonDlg);
            Assert.Equal("loc_garrison_checkpoint_gamma", garrisonDlg.locationId);
            Assert.Contains("quartermasters", garrisonDlg.speakerTag);

            Assert.Contains("ammo_*", garrisonPref.BuysAtPremium);
            Assert.Contains("body_armour_military", garrisonPref.BuysAtPremium);
        }

        [Fact]
        public void CrossSystem_DynamicEconomyAndDialogueGating_ArePureCoreAndDeterministic()
        {
            string dataDir = ResolveDataDir();
            var io = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var dialogLoader = new FactionWarContentCatalogLoader(io, json);
            var catalog = dialogLoader.Load(dataDir);

            string tuningJson = File.ReadAllText(Path.Combine(dataDir, "hardcore_economy_tuning.json"));
            var tuning = new HardcoreEconomyTuning();
            tuning.Apply(HardcoreEconomyTuningLoader.Load(tuningJson).Bundle!);

            // Repeated executions produce identical results across 50 iterations
            for (int i = 0; i < 50; i++)
            {
                var snippets = catalog.GetDialogueForLocation("loc_weighbridge", 520);
                Assert.Equal(3, snippets.Count);
                var ids = snippets.Select(s => s.id).ToHashSet();
                Assert.Contains("dlg_d493_weighbridge_toll_grumble", ids);
                Assert.Contains("dlg_d508_exchange_axle_grease_delay", ids);
                Assert.Contains("dlg_d512_weighbridge_reroute", ids);

                float multiplier = tuning.GetScarcityMultiplier(520, "medical_kit");
                Assert.Equal(2.4f, multiplier); // Day 520 is in Endgame tier (341+) where medical_kit is priced at 2.4x
            }
        }
    }
}
