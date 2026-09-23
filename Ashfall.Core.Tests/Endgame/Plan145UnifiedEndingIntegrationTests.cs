// SPDX-License-Identifier: MIT
// Tests for Plan 145: Unified Ending Resolution & Epilogue Personalization

using System;
using System.IO;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public class Plan145UnifiedEndingIntegrationTests
    {
        private readonly string _catalogPath;

        public Plan145UnifiedEndingIntegrationTests()
        {
            _catalogPath = Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/epilogue_personalization.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/epilogue_personalization.json");
            }
        }

        private sealed class TestEpilogueSink : IPersonalizedEpilogueSink
        {
            public UnifiedEndingResult? ReceivedResult { get; private set; }
            public void ReceivePersonalizedEpilogue(UnifiedEndingResult result)
            {
                ReceivedResult = result;
            }
        }

        [Fact]
        public void Catalog_LoadsPersonalizationTemplatesSuccessfully()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file missing at {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);

            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 366,
                livingDwellerCount = 12,
                factionBranchId = "Military",
                holdfastEndingId = HoldfastEndings.Schedule
            };

            var result = resolver.ResolveEnding(context);
            Assert.NotNull(result);
            Assert.Equal("Long", result.campaignDurationLabel);
            Assert.Contains("iron discipline", result.politicalProse);
        }

        [Fact]
        public void UnifiedEnding_MilitarySchedule_ResolvesCorrectProseAndTitle()
        {
            string json = File.ReadAllText(_catalogPath);
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var sink = new TestEpilogueSink();
            resolver.EpilogueSink = sink;

            UnifiedEndingResult? seamResult = null;
            resolver.OnUnifiedEndingResolvedSeam = r => seamResult = r;

            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 250,
                livingDwellerCount = 10,
                factionBranchId = "Military",
                holdfastEndingId = HoldfastEndings.Schedule,
                musterApproachId = "the_open_muster",
                moralChoiceBand = "VeryPositive"
            };

            var result = resolver.ResolveEnding(context);

            Assert.NotNull(result);
            Assert.Equal("Medium", result.campaignDurationLabel);
            Assert.Equal("The Schedule Holds", result.politicalOutcome);
            Assert.Contains("iron discipline", result.politicalProse);
            Assert.Equal("The Open Muster", result.socialOutcome);
            Assert.Contains("commonwealth", result.socialProse);
            Assert.Contains("compassion", result.moralProse);
            Assert.Same(result, sink.ReceivedResult);
            Assert.Same(result, seamResult);
        }

        [Fact]
        public void UnifiedEnding_MusterApproach_ResolvesSocialOutcomes()
        {
            string json = File.ReadAllText(_catalogPath);
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var ctx1 = new UnifiedEndingContext
            {
                totalDaysSurvived = 120,
                livingDwellerCount = 6,
                factionBranchId = "Rebel",
                musterApproachId = "the_blood_price",
                moralChoiceBand = "VeryEvil"
            };

            var res1 = resolver.ResolveEnding(ctx1);
            Assert.Equal("The Blood Price", res1.socialOutcome);
            Assert.Contains("blood price was paid", res1.socialProse);
            Assert.Contains("cold ruthlessness", res1.moralProse);

            var ctx2 = new UnifiedEndingContext
            {
                totalDaysSurvived = 400,
                livingDwellerCount = 15,
                factionBranchId = "Independent",
                musterApproachId = "the_amnesty",
                moralChoiceBand = "Positive"
            };

            var res2 = resolver.ResolveEnding(ctx2);
            Assert.Equal("The Amnesty", res2.socialOutcome);
            Assert.Contains("amnesty petition held firm", res2.socialProse);
        }

        [Fact]
        public void UnifiedEnding_SurvivorEpilogues_GeneratesCustomProseForAliveAndDeceased()
        {
            string json = File.ReadAllText(_catalogPath);
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            int epiloguesGeneratedCount = 0;
            resolver.OnSurvivorEpilogueGeneratedSeam = fate => epiloguesGeneratedCount++;

            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 300,
                livingDwellerCount = 8,
                keySurvivorFates = new()
                {
                    new SurvivorEpilogueFate
                    {
                        survivorId = "survivor_nadia",
                        survivorName = "Nadia Thorne",
                        status = SurvivorFateStatus.Alive,
                        notableTrait = "social"
                    },
                    new SurvivorEpilogueFate
                    {
                        survivorId = "survivor_vask",
                        survivorName = "Vask",
                        status = SurvivorFateStatus.Deceased,
                        notableTrait = ""
                    },
                    new SurvivorEpilogueFate
                    {
                        survivorId = "survivor_alec",
                        survivorName = "Alec Vance",
                        status = SurvivorFateStatus.Retired,
                        notableTrait = ""
                    }
                }
            };

            var result = resolver.ResolveEnding(context);

            Assert.Equal(3, result.survivorEpilogues.Count);
            Assert.Equal(3, epiloguesGeneratedCount);

            var nadia = result.survivorEpilogues.Find(e => e.survivorId == "survivor_nadia");
            Assert.NotNull(nadia);
            Assert.Contains("Nadia Thorne", nadia!.epilogueText);
            Assert.Contains("heart of the emerging settlement", nadia.epilogueText);

            var vask = result.survivorEpilogues.Find(e => e.survivorId == "survivor_vask");
            Assert.NotNull(vask);
            Assert.Contains("Vask", vask!.epilogueText);
            Assert.Contains("memorial wall", vask.epilogueText);

            var alec = result.survivorEpilogues.Find(e => e.survivorId == "survivor_alec");
            Assert.NotNull(alec);
            Assert.Contains("Alec Vance", alec!.epilogueText);
            Assert.Contains("honored role as shelter elder", alec.epilogueText);
        }

        [Fact]
        public void UnifiedEnding_FullChronicle_SynthesizesAllCategoriesDeterministically()
        {
            string json = File.ReadAllText(_catalogPath);
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 450,
                livingDwellerCount = 14,
                grandTreatySigned = true,
                tempestDecommissioned = true,
                factionBranchId = "Independent",
                holdfastEndingId = HoldfastEndings.Tender,
                musterApproachId = "the_open_muster",
                moralChoiceBand = "Positive",
                expeditionDiscoveries = new() { "water_aquifer", "copper_vein" },
                shelterUpgrades = new() { "greenhouse_dome", "perimeter_turrets" },
                keySurvivorFates = new()
                {
                    new SurvivorEpilogueFate
                    {
                        survivorId = "survivor_1",
                        survivorName = "Leader Rowan",
                        status = SurvivorFateStatus.Alive,
                        notableTrait = "ambitious"
                    }
                }
            };

            var result = resolver.ResolveEnding(context);

            Assert.NotEmpty(result.fullPersonalizedChronicle);
            Assert.Contains("ASHFALL RESOLUTION:", result.fullPersonalizedChronicle);
            Assert.Contains("[POLITICAL RESOLUTION:", result.fullPersonalizedChronicle);
            Assert.Contains("[COMMUNITY RESOLUTION:", result.fullPersonalizedChronicle);
            Assert.Contains("[ETHICAL LEGACY:", result.fullPersonalizedChronicle);
            Assert.Contains("[EXPEDITION DISCOVERIES]", result.fullPersonalizedChronicle);
            Assert.Contains("deep clean aquifer", result.fullPersonalizedChronicle);
            Assert.Contains("[SHELTER EXPANSION]", result.fullPersonalizedChronicle);
            Assert.Contains("greenhouse dome", result.fullPersonalizedChronicle);
            Assert.Contains("[FATES OF THE SURVIVORS]", result.fullPersonalizedChronicle);
            Assert.Contains("Leader Rowan", result.fullPersonalizedChronicle);

            Assert.Contains("legacy_trait_diplomat", result.legacyTraitsAwarded);
            Assert.Contains("legacy_trait_storm_breaker", result.legacyTraitsAwarded);
            Assert.Contains("legacy_trait_prosperous_haven", result.legacyTraitsAwarded);
        }

        [Fact]
        public void UnifiedEnding_SaveState_RoundTripsAccurately()
        {
            var resolver = new UnifiedEndingResolver();
            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 200,
                livingDwellerCount = 8,
                factionBranchId = "PRPF",
                holdfastEndingId = HoldfastEndings.Reserve
            };

            var result = resolver.ResolveEnding(context);
            Assert.True(resolver.IsResolved);

            var state = resolver.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.True(state.isResolved);
            Assert.NotNull(state.lastResult);
            Assert.Equal(result.resolutionId, state.lastResult!.resolutionId);

            var newResolver = new UnifiedEndingResolver();
            Assert.False(newResolver.IsResolved);
            Assert.Null(newResolver.LastResult);

            newResolver.RestoreState(state);
            Assert.True(newResolver.IsResolved);
            Assert.NotNull(newResolver.LastResult);
            Assert.Equal(result.resolutionId, newResolver.LastResult!.resolutionId);
        }

        [Fact]
        public void ResolutionId_IsDeterministic_ForIdenticalContext()
        {
            // Determinism invariant: no wall-clock or Guid identity in Core state.
            // The same ending context must always resolve to the same id.
            var context = new UnifiedEndingContext
            {
                totalDaysSurvived = 240,
                livingDwellerCount = 6,
                totalDeathsRecorded = 2,
                factionBranchId = "PRPF",
                moralChoiceBand = "Positive",
                holdfastEndingId = HoldfastEndings.Reserve
            };

            var first = new UnifiedEndingResolver().ResolveEnding(context);
            var second = new UnifiedEndingResolver().ResolveEnding(context);
            Assert.Equal(first.resolutionId, second.resolutionId);

            var diverged = new UnifiedEndingContext
            {
                totalDaysSurvived = 240,
                livingDwellerCount = 5, // one fewer survivor must change the resolution identity
                totalDeathsRecorded = 2,
                factionBranchId = "PRPF",
                moralChoiceBand = "Positive",
                holdfastEndingId = HoldfastEndings.Reserve
            };
            var third = new UnifiedEndingResolver().ResolveEnding(diverged);
            Assert.NotEqual(first.resolutionId, third.resolutionId);
        }
    }
}
