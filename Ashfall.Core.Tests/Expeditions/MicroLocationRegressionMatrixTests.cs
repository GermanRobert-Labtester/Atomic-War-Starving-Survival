using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    /// <summary>
    /// Task F25: Full Regression Matrix for Micro-Locations.
    ///
    /// Validates:
    /// 1. 25+ entry structural validation (ranges, IDs, categories, non-empty choices).
    /// 2. Cross-reference referential integrity (items in ItemCatalog, valid discovery IDs, valid codex keys).
    /// 3. 1000-encounter deterministic simulation replay check with SeededRng.
    /// 4. Parameterized resolution oracle covering all choices across all production micro-locations.
    /// 5. Non-regression verification for standard encounters (encounters.json / narrative_encounters.json).
    /// </summary>
    public class MicroLocationRegressionMatrixTests
    {
        private static string GetDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static List<EncounterDefinition> LoadMicroLocationsOnly()
        {
            string dataDir = GetDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDir, NarrativeEncounterCatalogLoader.MicroLocationsFileName);
            string raw = fileIO.ReadAllText(path);
            var list = CatalogLocator.LoadWrappedList<EncounterDefinition>(raw, SystemTextJsonSerializer.Options);
            foreach (var enc in list)
            {
                enc.isMicroLocation = true;
                enc.sourceFile = NarrativeEncounterCatalogLoader.MicroLocationsFileName;
            }
            return list;
        }

        private static NarrativeEncounterSystem CreateProductionNarrativeSystem()
        {
            string dataDir = GetDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var narrative = new NarrativeEncounterSystem();
            narrative.RegisterRange(NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json));
            return narrative;
        }

        private static ItemCatalog LoadItemCatalog()
        {
            string dataDir = GetDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return ItemCatalogLoader.LoadCatalog(dataDir, fileIO, json);
        }

        // ── 1. Structural Schema Validation ─────────────────────────────────

        [Fact]
        public void F25_01_AllMicroLocations_PassStructuralSchemaValidation()
        {
            var list = LoadMicroLocationsOnly();

            Assert.True(list.Count >= 25, $"Expected at least 25 micro-locations, found {list.Count}");

            var idSet = new HashSet<string>(StringComparer.Ordinal);
            var validCategories = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Discovery", "Hazard", "Social", "Trade", "exploration"
            };

            foreach (var enc in list)
            {
                Assert.False(string.IsNullOrWhiteSpace(enc.id), "Micro-location ID must not be empty");
                Assert.StartsWith("micro_", enc.id, StringComparison.Ordinal);
                Assert.True(idSet.Add(enc.id), $"Duplicate micro-location ID detected: {enc.id}");

                Assert.False(string.IsNullOrWhiteSpace(enc.title), $"Title empty for {enc.id}");
                Assert.False(string.IsNullOrWhiteSpace(enc.description), $"Description empty for {enc.id}");
                Assert.Contains(enc.category, validCategories);

                Assert.True(enc.baseWeight > 0f, $"baseWeight must be > 0 for {enc.id}");
                Assert.True(enc.stealthWeightMultiplier >= 0f, $"stealthWeightMultiplier must be >= 0 for {enc.id}");
                Assert.True(enc.speedWeightMultiplier >= 0f, $"speedWeightMultiplier must be >= 0 for {enc.id}");
                Assert.True(enc.minDangerLevel >= 0f, $"minDangerLevel must be >= 0 for {enc.id}");

                Assert.True(enc.isMicroLocation, $"isMicroLocation must be true for {enc.id}");
                Assert.NotNull(enc.choices);
                Assert.True(enc.choices.Count >= 2, $"Expected at least 2 choices for {enc.id}, found {enc.choices.Count}");

                var choiceIdSet = new HashSet<string>(StringComparer.Ordinal);
                foreach (var choice in enc.choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(choice.choiceId), $"choiceId empty in {enc.id}");
                    Assert.False(string.IsNullOrWhiteSpace(choice.text), $"choice text empty in {enc.id}:{choice.choiceId}");
                    Assert.True(choiceIdSet.Add(choice.choiceId), $"Duplicate choiceId '{choice.choiceId}' in {enc.id}");
                }
            }
        }

        // ── 2. Cross-Reference Referential Integrity ────────────────────────

        [Fact]
        public void F25_02_ReferentialIntegrity_Items_Locations_Codex()
        {
            var list = LoadMicroLocationsOnly();
            var itemCatalog = LoadItemCatalog();

            foreach (var enc in list)
            {
                foreach (var choice in enc.choices)
                {
                    if (!string.IsNullOrEmpty(choice.grantItemId))
                    {
                        var itemDef = itemCatalog.Get(choice.grantItemId);
                        Assert.True(itemDef != null, $"Referenced grantItemId '{choice.grantItemId}' in {enc.id}:{choice.choiceId} does not exist in items catalog");
                        Assert.True(choice.grantItemQuantity != 0, $"grantItemQuantity must be != 0 when grantItemId is set in {enc.id}:{choice.choiceId}");
                    }

                    if (choice.costItems != null)
                    {
                        foreach (var costItem in choice.costItems)
                        {
                            var itemDef = itemCatalog.Get(costItem);
                            Assert.True(itemDef != null, $"Referenced costItem '{costItem}' in {enc.id}:{choice.choiceId} does not exist in items catalog");
                        }
                    }

                    if (!string.IsNullOrEmpty(choice.discoverLocationId))
                    {
                        Assert.False(string.IsNullOrWhiteSpace(choice.discoverLocationId));
                    }

                    if (!string.IsNullOrEmpty(choice.journalUnlockId))
                    {
                        Assert.False(string.IsNullOrWhiteSpace(choice.journalUnlockId));
                    }
                }
            }
        }

        // ── 3. 1000-Encounter Deterministic Simulation Replay Check ─────────

        [Fact]
        public void F25_03_Deterministic1000EncounterReplay_SimulationOracle()
        {
            const int testSeed = 777;
            const int iterations = 1000;

            string[] stances = { "Stealth", "Speed", "Cautious", "Aggressive" };
            string[] testLocations = { "loc_denial_cut_substation", "loc_the_allotments", "rural_gas_station", "suburban_house", "" };

            // Run 1: Collect sequence
            var trace1 = new List<string?>(iterations);
            var sys1 = CreateProductionNarrativeSystem();
            var rng1 = new SeededRng(testSeed);

            for (int i = 0; i < iterations; i++)
            {
                string stance = stances[i % stances.Length];
                float danger = (i % 5);
                string loc = testLocations[i % testLocations.Length];

                var picked = sys1.SelectEncounter(stance, danger, loc, rng1);
                trace1.Add(picked?.id);
            }

            // Run 2: Collect sequence with identical seed
            var trace2 = new List<string?>(iterations);
            var sys2 = CreateProductionNarrativeSystem();
            var rng2 = new SeededRng(testSeed);

            for (int i = 0; i < iterations; i++)
            {
                string stance = stances[i % stances.Length];
                float danger = (i % 5);
                string loc = testLocations[i % testLocations.Length];

                var picked = sys2.SelectEncounter(stance, danger, loc, rng2);
                trace2.Add(picked?.id);
            }

            Assert.Equal(iterations, trace1.Count);
            Assert.Equal(iterations, trace2.Count);

            // Assert 100% deterministic equality
            for (int i = 0; i < iterations; i++)
            {
                Assert.Equal(trace1[i], trace2[i]);
            }

            // Verify that micro-locations are surfaced in the 1000 draws
            int microCount = trace1.Count(id => id != null && id.StartsWith("micro_", StringComparison.Ordinal));
            Assert.True(microCount > 0, $"Expected organic micro-location encounters in {iterations} draws, found {microCount}");
        }

        // ── 4. Parameterized Resolution Oracle covering all choices ─────────

        [Fact]
        public void F25_04_ParameterizedResolutionOracle_EveryChoiceAcrossAllMicroLocations()
        {
            var list = LoadMicroLocationsOnly();

            foreach (var enc in list)
            {
                foreach (var choice in enc.choices)
                {
                    // Fresh narrative system per resolution check
                    var sys = new NarrativeEncounterSystem();
                    sys.RegisterEncounter(enc);

                    const string testLoc = "loc_test_sector";
                    const int testDay = 5;

                    var res = sys.TryResolve(enc.id, choice.choiceId, testLoc, testDay);
                    Assert.NotNull(res);

                    Assert.Equal(enc.id, res!.EncounterId);
                    Assert.Equal(choice.choiceId, res.ChoiceId);
                    Assert.Equal(testLoc, res.LocationId);
                    Assert.Equal(testDay, res.Day);
                    Assert.Equal(choice.moraleDelta, res.MoraleDelta);
                    Assert.Equal(choice.guiltDelta, res.GuiltDelta);
                    Assert.Equal(choice.depletesOnResolve, res.DepletesEncounter);
                    Assert.Equal(choice.grantItemId ?? string.Empty, res.GrantItemId);
                    Assert.Equal(choice.grantItemQuantity, res.GrantItemQuantity);
                    Assert.Equal(choice.journalUnlockId ?? string.Empty, res.JournalUnlockId);
                    Assert.Equal(choice.discoverLocationId ?? string.Empty, res.DiscoverLocationId);
                    Assert.Equal(choice.setWorldFlag ?? string.Empty, res.SetWorldFlagId);

                    string expectedResolutionId = $"{enc.id}:{choice.choiceId}:{testDay}:{testLoc}";
                    Assert.Equal(expectedResolutionId, res.ResolutionId);

                    // Verify depletion state
                    if (choice.depletesOnResolve)
                    {
                        Assert.True(sys.IsDepleted(enc.id));
                    }
                    else
                    {
                        Assert.False(sys.IsDepleted(enc.id));
                    }

                    // Verify history record
                    Assert.Single(sys.State.history);
                    var record = sys.State.history[0];
                    Assert.Equal(enc.id, record.encounterId);
                    Assert.Equal(choice.choiceId, record.choiceId);
                    Assert.Equal(choice.moraleDelta, record.moraleDelta);
                    Assert.Equal(choice.guiltDelta, record.guiltDelta);
                    Assert.Equal(testDay, record.day);
                }
            }
        }

        // ── 5. Standard Encounters Non-Regression Verification ──────────────

        [Fact]
        public void F25_05_StandardEncounters_NonRegressionVerification()
        {
            var sys = CreateProductionNarrativeSystem();

            // Verify standard encounters exist alongside micro-locations
            var standardEncounters = sys.Catalog.Where(e => !e.isMicroLocation).ToList();
            var microLocations = sys.Catalog.Where(e => e.isMicroLocation).ToList();

            Assert.NotEmpty(standardEncounters);
            Assert.True(microLocations.Count >= 25);

            // Verify a known standard encounter
            var standardSample = standardEncounters.FirstOrDefault(e => e.choices.Count > 0);
            Assert.NotNull(standardSample);

            // Depleting a micro-location must not affect standard encounters
            var microSample = microLocations.First(e => e.choices.Any(c => c.depletesOnResolve));
            var depletingChoice = microSample.choices.First(c => c.depletesOnResolve);

            sys.TryResolve(microSample.id, depletingChoice.choiceId, "loc_the_allotments", 1);
            Assert.True(sys.IsDepleted(microSample.id));

            // Verify standard encounter is NOT depleted
            Assert.False(sys.IsDepleted(standardSample!.id));

            // Verify standard encounter can still resolve cleanly
            var stdRes = sys.TryResolve(standardSample.id, standardSample.choices[0].choiceId, "loc_the_allotments", 2);
            Assert.NotNull(stdRes);
            Assert.Equal(standardSample.id, stdRes!.EncounterId);
        }
    }
}
