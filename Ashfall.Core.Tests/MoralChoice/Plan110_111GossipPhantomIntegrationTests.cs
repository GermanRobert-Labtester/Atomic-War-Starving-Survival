// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Phantoms;
using Xunit;

namespace Ashfall.Core.Tests.MoralChoice
{
    /// <summary>
    /// Wave 41 Batch 3 Cross-System Integration Test:
    /// Validates Plan 110 (Moral Choice Gossip Expansion - 21 arrays × 20 lines = 420 total lines)
    /// alongside Plan 111 (Phantom Memory Triggers Expansion - 7 baseline → 20 survivor backgrounds).
    /// </summary>
    public sealed class Plan110_111GossipPhantomIntegrationTests : CatalogTestBase
    {
        private static readonly MoralPathBand[] AllSevenBands =
        {
            MoralPathBand.VeryPositive,
            MoralPathBand.Positive,
            MoralPathBand.SlightlyPositive,
            MoralPathBand.Neutral,
            MoralPathBand.SlightlyEvil,
            MoralPathBand.Evil,
            MoralPathBand.VeryEvil
        };

        private static readonly string[] ExpectedTwentyBackgrounds =
        {
            "child_refugee",
            "radio_operator",
            "miner",
            "librarian",
            "cleric",
            "scavenger",
            "cook",
            "former_soldier",
            "nurse",
            "teacher",
            "electrician",
            "machinist",
            "farmer",
            "engineer",
            "laborer",
            "architect",
            "chemist",
            "medic",
            "driver",
            "generic"
        };

        [Fact]
        public void Plan110_MoralChoiceGossip_LoadsAllTwentyOneArrays_AtExactlyTwentyLinesEach()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var gossipData = MoralChoiceGossipCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(gossipData);

            // Verify camp chatter
            Assert.NotNull(gossipData.CampChatter);
            Assert.Equal(20, gossipData.CampChatter.VeryPositive.Count);
            Assert.Equal(20, gossipData.CampChatter.Positive.Count);
            Assert.Equal(20, gossipData.CampChatter.SlightlyPositive.Count);
            Assert.Equal(20, gossipData.CampChatter.Neutral.Count);
            Assert.Equal(20, gossipData.CampChatter.SlightlyEvil.Count);
            Assert.Equal(20, gossipData.CampChatter.Evil.Count);
            Assert.Equal(20, gossipData.CampChatter.VeryEvil.Count);

            // Verify NPC greeting shifts
            Assert.NotNull(gossipData.NpcGreetingShifts);
            Assert.Equal(20, gossipData.NpcGreetingShifts.VeryPositive.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.Positive.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.SlightlyPositive.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.Neutral.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.SlightlyEvil.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.Evil.Count);
            Assert.Equal(20, gossipData.NpcGreetingShifts.VeryEvil.Count);

            // Verify whisper lines (including slightly positive which was previously empty)
            Assert.NotNull(gossipData.WhisperLines);
            Assert.Equal(20, gossipData.WhisperLines.VeryPositive.Count);
            Assert.Equal(20, gossipData.WhisperLines.Positive.Count);
            Assert.Equal(20, gossipData.WhisperLines.SlightlyPositive.Count);
            Assert.Equal(20, gossipData.WhisperLines.Neutral.Count);
            Assert.Equal(20, gossipData.WhisperLines.SlightlyEvil.Count);
            Assert.Equal(20, gossipData.WhisperLines.Evil.Count);
            Assert.Equal(20, gossipData.WhisperLines.VeryEvil.Count);

            // Verify runtime querying and line picking
            var rng = new SeededRng(110);
            var runtime = new MoralChoiceGossipRuntime(gossipData, rng);

            foreach (var band in AllSevenBands)
            {
                var chatter = runtime.GetCampChatter(band);
                Assert.Equal(20, chatter.Count);
                Assert.All(chatter, line => Assert.False(string.IsNullOrWhiteSpace(line)));
                Assert.Equal(20, chatter.Distinct(StringComparer.OrdinalIgnoreCase).Count());

                var greetings = runtime.GetNpcGreetings(band);
                Assert.Equal(20, greetings.Count);
                Assert.All(greetings, line => Assert.False(string.IsNullOrWhiteSpace(line)));
                Assert.Equal(20, greetings.Distinct(StringComparer.OrdinalIgnoreCase).Count());

                var whispers = runtime.GetWhisperLines(band);
                Assert.Equal(20, whispers.Count);
                Assert.All(whispers, line => Assert.False(string.IsNullOrWhiteSpace(line)));
                Assert.Equal(20, whispers.Distinct(StringComparer.OrdinalIgnoreCase).Count());

                string picked = runtime.PickCampChatter(band);
                Assert.False(string.IsNullOrWhiteSpace(picked));
                Assert.Contains(picked, chatter);
            }
        }

        [Fact]
        public void Plan111_PhantomMemoryTriggers_LoadsAllTwentyBackgrounds_WithValidTriggerRules()
        {
            string catalogPath = Path.Combine(DataDirectory, "phantom_triggers.json");
            Assert.True(File.Exists(catalogPath), "phantom_triggers.json must exist.");

            var catalog = JsonSerializer.Deserialize<PhantomTriggerCatalogJson>(
                File.ReadAllText(catalogPath), SystemTextJsonSerializer.Options);

            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.NotNull(catalog.items);
            Assert.Equal(20, catalog.items.Count);

            var seenBackgrounds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in catalog.items)
            {
                Assert.NotNull(entry);
                Assert.False(string.IsNullOrWhiteSpace(entry.background_id), "background_id must not be empty.");
                Assert.True(seenBackgrounds.Add(entry.background_id), $"Duplicate background_id: {entry.background_id}");

                Assert.NotNull(entry.triggers);
                Assert.NotEmpty(entry.triggers);

                foreach (var trigger in entry.triggers)
                {
                    Assert.NotNull(trigger);
                    Assert.False(string.IsNullOrWhiteSpace(trigger.item_category) && string.IsNullOrWhiteSpace(trigger.item_id),
                        $"Trigger under background '{entry.background_id}' must specify item_category or item_id.");
                    Assert.False(string.IsNullOrWhiteSpace(trigger.description),
                        $"Trigger description missing for '{entry.background_id}'.");
                    Assert.False(string.IsNullOrWhiteSpace(trigger.motivation_text),
                        $"Motivation text missing for '{entry.background_id}'.");
                    Assert.False(string.IsNullOrWhiteSpace(trigger.breakdown_text),
                        $"Breakdown text missing for '{entry.background_id}'.");
                    Assert.True(trigger.motivation_chance >= 0f && trigger.motivation_chance <= 1f,
                        $"motivation_chance out of bounds for '{entry.background_id}'.");
                }
            }

            foreach (var expectedBg in ExpectedTwentyBackgrounds)
            {
                Assert.Contains(expectedBg, seenBackgrounds);
            }
        }

        [Fact]
        public void Plan111_PhantomMemoryEngine_ResolvesOutcomesAndLifecycle_Deterministically()
        {
            var engine = new PhantomMemoryEngine();
            engine.TriggerChanceOverride = 1.0f; // Force deterministic trigger

            // Register specific rules for the new background 'farmer'
            engine.RegisterRule("farmer", "correspondence", 0.5f, "A harvest ledger recording bushels.", "He remembers the autumn harvest.", "The soil is dead.");

            var survivor = new PhantomSurvivorSnapshot
            {
                survivorId = "sv_farmer_johan",
                displayName = "Johan",
                backgroundId = "farmer",
                isAlive = true
            };

            var rng = new SeededRng(111);
            var outcome = engine.OnItemScavenged(survivor, "farm_ledger", rng);
            Assert.NotEqual(TriggerOutcome.None, outcome);

            Assert.Single(engine.Records);
            var record = engine.Records[0];
            Assert.Equal("sv_farmer_johan", record.survivorId);
            Assert.Equal(1, record.triggersExperienced);

            // Test hourly tick decay
            engine.TickHour("sv_farmer_johan", 1.0f);
            var state = engine.CaptureState();
            Assert.NotNull(state);
            Assert.Single(state.records);

            // Test engine restore
            var newEngine = new PhantomMemoryEngine();
            newEngine.RestoreState(state);
            Assert.Single(newEngine.Records);
            Assert.Equal("sv_farmer_johan", newEngine.Records[0].survivorId);
            Assert.Equal(1, newEngine.Records[0].triggersExperienced);
        }

        [Fact]
        public void Plan110_111_CrossSystem_GossipToneAndPhantomMemories_CoherenceContract()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var gossipData = MoralChoiceGossipCatalogLoader.Load(DataDirectory, files, json);
            string catalogPath = Path.Combine(DataDirectory, "phantom_triggers.json");
            var phantomCatalog = JsonSerializer.Deserialize<PhantomTriggerCatalogJson>(
                File.ReadAllText(catalogPath), SystemTextJsonSerializer.Options);

            Assert.NotNull(gossipData);
            Assert.NotNull(phantomCatalog);

            // Verify camp chatter and whispers contain grounded, concrete survival situations
            var evilWhispers = gossipData.WhisperLines.Evil;
            Assert.All(evilWhispers, w => Assert.True(w.Length > 10, "Whisper line too terse."));

            var positiveChatter = gossipData.CampChatter.Positive;
            Assert.All(positiveChatter, c => Assert.True(c.Length > 10, "Chatter line too terse."));

            // Verify phantom backgrounds cover the civilian, technical, and military spectrum of the shelter
            var backgroundIds = phantomCatalog!.items.Select(i => i.background_id).ToHashSet();
            Assert.Contains("farmer", backgroundIds);
            Assert.Contains("cook", backgroundIds);
            Assert.Contains("former_soldier", backgroundIds);
            Assert.Contains("medic", backgroundIds);
            Assert.Contains("teacher", backgroundIds);
            Assert.Contains("radio_operator", backgroundIds);

            // Verify deterministic catalog reload parity
            var gossipData2 = MoralChoiceGossipCatalogLoader.Load(DataDirectory, files, json);
            var phantomCatalog2 = JsonSerializer.Deserialize<PhantomTriggerCatalogJson>(
                File.ReadAllText(catalogPath), SystemTextJsonSerializer.Options);

            Assert.Equal(phantomCatalog.items.Count, phantomCatalog2!.items.Count);
            Assert.Equal(gossipData.CampChatter.Neutral.Count, gossipData2.CampChatter.Neutral.Count);
        }
    }
}
