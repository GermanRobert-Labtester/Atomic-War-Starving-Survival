// SPDX-License-Identifier: MIT
// Unit tests for Ashfall.Core.Endgame.EndgameSystem (Plan 84 / Task B25).

using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Endgame;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public class EndgameSystemTests
    {
        private readonly string _catalogJson;
        private readonly SystemTextJsonSerializer _serializer = new();

        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            return "Assets/StreamingAssets/Data";
        }

        public EndgameSystemTests()
        {
            string dataDir = FindDataDir();
            string path = Path.Combine(dataDir, "endings.json");
            _catalogJson = File.Exists(path) ? File.ReadAllText(path) : string.Empty;
        }

        private EndgameSystem CreateSystem()
        {
            var sys = new EndgameSystem(new SeededRng(84), NullLog.Instance);
            if (!string.IsNullOrEmpty(_catalogJson))
            {
                sys.LoadCatalog(_catalogJson, _serializer);
            }
            return sys;
        }

        [Fact]
        public void LoadCatalog_PopulatesAuthoredEndings()
        {
            var sys = CreateSystem();
            Assert.True(sys.Catalog.Count >= 8, $"Expected >= 8 endings, got {sys.Catalog.Count}");
            Assert.True(sys.Catalog.ContainsKey("ending_dawn_of_thaw"));
            Assert.True(sys.Catalog.ContainsKey("ending_silent_tombs"));
            Assert.True(sys.Catalog.ContainsKey("ending_iron_hegemony"));
            Assert.True(sys.Catalog.ContainsKey("ending_exodus_to_sea"));
            Assert.True(sys.Catalog.ContainsKey("ending_wasteland_sanctuary"));
        }

        [Fact]
        public void EvaluateEnding_ZeroLivingSurvivors_SelectsSilentTombs()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 120,
                LivingSurvivors = 0,
                DeceasedSurvivors = 10
            };
            var ending = sys.EvaluateEnding(ctx);
            Assert.Equal("ending_silent_tombs", ending.id);
            Assert.Equal("bleak", ending.tone);
        }

        [Fact]
        public void EvaluateEnding_Day360_SelectsDawnOfThaw()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 14,
                AverageMorale = 65f,
                DominantFaction = "independent"
            };
            var ending = sys.EvaluateEnding(ctx);
            Assert.Equal("ending_dawn_of_thaw", ending.id);
            Assert.Equal("hopeful", ending.tone);
        }

        [Fact]
        public void EvaluateEnding_GarrisonDominance_SelectsIronBastion()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 250,
                LivingSurvivors = 10,
                DominantFaction = "garrison"
            };
            var ending = sys.EvaluateEnding(ctx);
            Assert.Equal("ending_iron_hegemony", ending.id);
            Assert.Equal("militaristic", ending.tone);
        }

        [Fact]
        public void EvaluateEnding_BlackFlotilla_SelectsExodus()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 280,
                LivingSurvivors = 8,
                DominantFaction = "black_flotilla"
            };
            var ending = sys.EvaluateEnding(ctx);
            Assert.Equal("ending_exodus_to_sea", ending.id);
            Assert.Equal("wistful", ending.tone);
        }

        [Fact]
        public void EvaluateEnding_HumanitarianHighPopulation_SelectsSanctuary()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 320,
                LivingSurvivors = 16,
                AverageMorale = 80f,
                DominantFaction = "humanitarian"
            };
            var ending = sys.EvaluateEnding(ctx);
            Assert.Equal("ending_wasteland_sanctuary", ending.id);
            Assert.Equal("humanitarian", ending.tone);
        }

        [Fact]
        public void TriggerEnding_GeneratesEpilogueAndTransitionsPhase()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 12,
                DeceasedSurvivors = 2,
                AverageMorale = 70f,
                ExpeditionsCount = 25,
                NotableFallenNames = new() { "Dr. Vance", "Sergeant Miller" }
            };

            bool triggered = sys.TriggerEnding(ctx);
            Assert.True(triggered);
            Assert.Equal(EndgamePhase.Epilogue, sys.Phase);
            Assert.NotNull(sys.State.epilogueReport);
            Assert.Equal("ending_dawn_of_thaw", sys.State.selectedEndingId);
            Assert.Contains("Dr. Vance", sys.State.epilogueReport.memorialTributes[0]);
        }

        [Fact]
        public void SealCampaign_LocksStatePermanently()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 12
            };
            sys.TriggerEnding(ctx);

            bool sealedOk = sys.SealCampaign(360);
            Assert.True(sealedOk);
            Assert.True(sys.IsSealed);
            Assert.Equal(EndgamePhase.Sealed, sys.Phase);

            // Attempting to re-trigger or re-seal is blocked
            Assert.False(sys.TriggerEnding(ctx));
            Assert.False(sys.SealCampaign(361));
        }

        [Fact]
        public void StateCaptureAndRestore_PreservesAllFields()
        {
            var sys = CreateSystem();
            var ctx = new CampaignEvaluationContext
            {
                CurrentDay = 360,
                LivingSurvivors = 12,
                DeceasedSurvivors = 1,
                NotableFallenNames = new() { "Elena Rostova" }
            };
            sys.TriggerEnding(ctx);
            sys.SealCampaign(360);

            var saved = sys.CaptureState();
            var restored = CreateSystem();
            restored.RestoreState(saved);

            Assert.True(restored.IsSealed);
            Assert.Equal(EndgamePhase.Sealed, restored.Phase);
            Assert.Equal("ending_dawn_of_thaw", restored.State.selectedEndingId);
            Assert.NotNull(restored.State.epilogueReport);
            Assert.Contains("Elena Rostova", restored.State.epilogueReport.memorialTributes[0]);

            string hashA = SaveChecksum.Compute(saved);
            string hashB = SaveChecksum.Compute(restored.CaptureState());
            Assert.Equal(hashA, hashB);
        }
    }
}
