// SPDX-License-Identifier: MIT
// Host and domain integration tests for Plan 145: Unified Ending Resolution & Epilogue Personalization.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class Plan145UnifiedEndingHostIntegrationTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void Resolver_GetCensus_ReportsValidTemplateCounts()
        {
            string dataDir = GetDataDir();
            string json = File.ReadAllText(Path.Combine(dataDir, "epilogue_personalization.json"));
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var census = resolver.GetCensus();
            Assert.True(census.DurationTemplatesCount >= 3, $"Expected >= 3 duration templates, got {census.DurationTemplatesCount}");
            Assert.True(census.PoliticalTemplatesCount >= 5, $"Expected >= 5 political templates, got {census.PoliticalTemplatesCount}");
            Assert.True(census.SocialTemplatesCount >= 5, $"Expected >= 5 social templates, got {census.SocialTemplatesCount}");
            Assert.True(census.MoralTemplatesCount >= 5, $"Expected >= 5 moral templates, got {census.MoralTemplatesCount}");
        }

        [Fact]
        public void Resolver_ResolvesAllDurationBands_ShortMediumLong()
        {
            string dataDir = GetDataDir();
            string json = File.ReadAllText(Path.Combine(dataDir, "epilogue_personalization.json"));
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var ctxShort = new UnifiedEndingContext { totalDaysSurvived = 60 };
            var resShort = resolver.ResolveEnding(ctxShort);
            Assert.Equal("Short", resShort.campaignDurationLabel);

            var ctxMed = new UnifiedEndingContext { totalDaysSurvived = 220 };
            var resMed = resolver.ResolveEnding(ctxMed);
            Assert.Equal("Medium", resMed.campaignDurationLabel);

            var ctxLong = new UnifiedEndingContext { totalDaysSurvived = 400 };
            var resLong = resolver.ResolveEnding(ctxLong);
            Assert.Equal("Long", resLong.campaignDurationLabel);
        }

        [Fact]
        public void Resolver_GeneratesKeySurvivorFates_WithAliveAndDeceasedStatuses()
        {
            string dataDir = GetDataDir();
            string json = File.ReadAllText(Path.Combine(dataDir, "epilogue_personalization.json"));
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var ctx = new UnifiedEndingContext
            {
                totalDaysSurvived = 300,
                livingDwellerCount = 4,
                keySurvivorFates =
                {
                    new SurvivorEpilogueFate { survivorId = "s1", survivorName = "Elena", status = SurvivorFateStatus.Alive, notableTrait = "engineer" },
                    new SurvivorEpilogueFate { survivorId = "s2", survivorName = "Marcus", status = SurvivorFateStatus.Deceased, notableTrait = "scout" }
                }
            };

            var res = resolver.ResolveEnding(ctx);
            Assert.Equal(2, res.survivorEpilogues.Count);
            Assert.Contains(res.survivorEpilogues, e => e.survivorId == "s1" && e.status == SurvivorFateStatus.Alive && e.epilogueText.Contains("Elena"));
            Assert.Contains(res.survivorEpilogues, e => e.survivorId == "s2" && e.status == SurvivorFateStatus.Deceased && e.epilogueText.Contains("Marcus"));
        }

        [Fact]
        public void Resolver_AwardsLegacyTraits_AndSynthesizesChronicle()
        {
            string dataDir = GetDataDir();
            string json = File.ReadAllText(Path.Combine(dataDir, "epilogue_personalization.json"));
            var resolver = new UnifiedEndingResolver();
            resolver.LoadCatalog(json);

            var ctx = new UnifiedEndingContext
            {
                totalDaysSurvived = 370,
                livingDwellerCount = 10,
                grandTreatySigned = true,
                tempestDecommissioned = true,
                factionBranchId = "Rebel",
                holdfastEndingId = HoldfastEndings.Tender,
                moralChoiceBand = "VeryPositive",
                shelterUpgrades = { "hydroponic_bay", "lead_lining" },
                expeditionDiscoveries = { "encrypted_caches" }
            };

            var res = resolver.ResolveEnding(ctx);
            Assert.NotNull(res.fullPersonalizedChronicle);
            Assert.Contains("ASHFALL RESOLUTION:", res.fullPersonalizedChronicle);
            Assert.Contains("legacy_trait_diplomat", res.legacyTraitsAwarded);
            Assert.Contains("legacy_trait_prosperous_haven", res.legacyTraitsAwarded);
            Assert.Contains("legacy_trait_storm_breaker", res.legacyTraitsAwarded);
            Assert.Contains("legacy_trait_humanitarian", res.legacyTraitsAwarded);
        }

        [Fact]
        public void Resolver_SaveStateRoundTrip_PreservesLastResolution()
        {
            var saveState = new UnifiedEndingSaveState
            {
                isResolved = true,
                lastResult = new UnifiedEndingResult
                {
                    resolutionId = "res1",
                    campaignDurationLabel = "Long",
                    politicalOutcome = "The Tender Frontier",
                    socialOutcome = "The Open Muster",
                    moralOutcome = "A Beacon in the Dark",
                    legacyTraitsAwarded = { "legacy_trait_diplomat", "legacy_trait_prosperous_haven" }
                }
            };

            Assert.Equal(1, saveState.schema_version);
            Assert.True(saveState.isResolved);
            Assert.NotNull(saveState.lastResult);
            Assert.Equal("Long", saveState.lastResult.campaignDurationLabel);
            Assert.Equal(2, saveState.lastResult.legacyTraitsAwarded.Count);
        }
    }
}
