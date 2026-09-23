#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 140 — Generational Legacy & Campaign Inheritance host-integration gate.
//
// Pins the production wiring contract:
//   * authored legacy_traits.json loads cleanly with strict validation (>= 20 traits),
//   * state capture/restore round-trips with schema versioning and checksum,
//   * save section registry registers "campaign_legacy" -> "campaign_legacy_save.json",
//   * event vocabulary classifies "campaign_legacy_ticked" as Heartbeat,
//   * CLI registry defines CampaignLegacySelfTest and --campaign-legacy-selftest descriptor,
//   * host files (CampaignLegacyHostSession, HostCli.CampaignLegacy, Main.CampaignLegacy)
//     exist, wire into campaign day owners, and lifecycle hooks.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Legacy;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Legacy
{
    public sealed class Plan140CampaignLegacyHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void AuthoredLegacyTraitsCatalog_LoadsAndPassesStrictValidation()
        {
            var system = CampaignLegacySystem.LoadFromDirectory(DataDir(), new FileSystemIO());

            Assert.NotNull(system.CatalogTraits);
            Assert.True(system.CatalogTraits.Count >= 20, $"Expected >= 20 legacy traits, found {system.CatalogTraits.Count}");

            Assert.True(system.TryGetTrait("trait_leaders_blood", out var leader));
            Assert.NotNull(leader);
            Assert.Equal("survivor", leader!.source);
            Assert.Equal("trait_dynasty_call", leader.evolution_target_id);

            Assert.True(system.TryGetTrait("trait_fortified_walls", out var walls));
            Assert.NotNull(walls);
            Assert.Equal("shelter", walls!.source);

            Assert.True(system.TryGetTrait("trait_old_alliance_vanguard", out var alliance));
            Assert.NotNull(alliance);
            Assert.Equal("faction", alliance!.source);
        }

        [Fact]
        public void CampaignLegacy_StateRoundTrip_PreservesAllFields()
        {
            var system = CampaignLegacySystem.LoadFromDirectory(DataDir(), new FileSystemIO(), new SeededRng(42));
            system.ArchiveCampaign(new CampaignLegacy
            {
                campaignId = "camp_001",
                endingId = "ending_escape",
                daysSurvived = 45,
                survivorCount = 6,
                deathsRecorded = 1,
                legacyTraits = new List<string> { "trait_leaders_blood", "trait_fortified_walls" },
                factionStandings = new Dictionary<string, float> { { "settlers", 60f } },
                shelterImprovements = new List<string> { "reinforced_bulkhead" }
            });

            var captured = system.CaptureState();
            string jsonA = JsonSerializer.Serialize(captured);

            var restored = CampaignLegacySystem.LoadFromDirectory(DataDir(), new FileSystemIO(), new SeededRng(99));
            restored.RestoreState(captured);

            var capturedAgain = restored.CaptureState();
            string jsonB = JsonSerializer.Serialize(capturedAgain);

            Assert.Equal(jsonA, jsonB);
            Assert.Single(restored.State.completedCampaigns);
            Assert.Equal("camp_001", restored.State.completedCampaigns[0].campaignId);
            Assert.NotEmpty(restored.State.activeLegacyTraits);
        }

        [Fact]
        public void SaveSectionRegistry_RegistersCampaignLegacySection()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("campaign_legacy", out var meta));
            Assert.NotNull(meta);
            Assert.Equal("campaign_legacy_save.json", SaveSectionRegistry.FileNameFor("campaign_legacy"));
            Assert.Equal("SaveCampaignLegacy", meta!.SaveMethod);
            Assert.Equal("SetupCampaignLegacy", meta.SetupMethod);
            Assert.Equal("campaign", meta.Owner);

            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("campaign_legacy", out string? file));
            Assert.Equal("campaign_legacy_save.json", file);
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesCampaignLegacyHeartbeat()
        {
            var kind = DayEventVocabulary.GetSemanticKind("campaign_legacy_ticked");
            Assert.Equal(SemanticKind.Heartbeat, kind);
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("campaign_legacy_ticked"));

            string matrix = ReadRepoFile("docs", "campaign", "EVENT_SEMANTIC_PARITY_MATRIX.md");
            Assert.Contains("campaign_legacy_ticked", matrix);
        }

        [Fact]
        public void HostCliRegistry_RegistersCampaignLegacySelfTest()
        {
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.CampaignLegacySelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--campaign-legacy-selftest", desc!.PrimaryFlag);
            Assert.Contains("--legacy-selftest", desc.Aliases);
        }

        [Fact]
        public void HostWiring_SourceFilesExistAndDeclareSeams()
        {
            string hostSession = ReadRepoFile("src", "Host", "CampaignLegacyHostSession.cs");
            Assert.Contains("class CampaignLegacyHostSession", hostSession);
            Assert.Contains("class CampaignLegacySaveStore", hostSession);
            Assert.Contains("SaveStoreHub.Checksummed", hostSession);

            string hostCli = ReadRepoFile("src", "Host", "HostCli.CampaignLegacy.cs");
            Assert.Contains("RunCampaignLegacySelfTest", hostCli);

            string mainLegacy = ReadRepoFile("src", "Main.CampaignLegacy.cs");
            Assert.Contains("EnsureCampaignLegacy", mainLegacy);
            Assert.Contains("SetupCampaignLegacy", mainLegacy);
            Assert.Contains("SaveCampaignLegacy", mainLegacy);
            Assert.Contains("RestoreCampaignLegacy", mainLegacy);
            Assert.Contains("ArchiveCurrentCampaign", mainLegacy);
            Assert.Contains("PrepareStartingCampaignContext", mainLegacy);

            string campaignOwners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("CampaignLegacyDayOwner", campaignOwners);
            Assert.Contains("\"campaign_legacy\"", campaignOwners);

            string expandedShelter = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupCampaignLegacy();", expandedShelter);
            Assert.Contains("SaveCampaignLegacy();", expandedShelter);
            Assert.Contains("ResetCampaignLegacy();", expandedShelter);

            string mainApp = ReadRepoFile("src", "Main.Application.cs");
            Assert.Contains("HostCliAction.CampaignLegacySelfTest", mainApp);
            Assert.Contains("FlushCampaignLegacyIfDirty", mainApp);
        }

        [Fact]
        public void EndgameSealing_And_NewGamePlus_AreWiredInHost()
        {
            // Sealing archive wiring in Main.Endgame.cs
            string mainEndgame = ReadRepoFile("src", "Main.Endgame.cs");
            Assert.Contains("ArchiveCurrentCampaign(legacy)", mainEndgame);
            Assert.Contains("SaveCampaignLegacy()", mainEndgame);

            // New Game+ starting context preparation and application in Main.GameFlow.cs
            string mainGameFlow = ReadRepoFile("src", "Main.GameFlow.cs");
            Assert.Contains("PrepareStartingCampaignContext()", mainGameFlow);
            Assert.Contains("legacyContext.factionModifiers", mainGameFlow);
            Assert.Contains("legacyContext.inheritedTraits", mainGameFlow);
        }

        [Fact]
        public void SealedCampaign_YieldsInheritedStartingContext_OnNewGame()
        {
            var system = CampaignLegacySystem.LoadFromDirectory(DataDir(), new FileSystemIO(), new SeededRng(100));

            // Archive a sealed campaign with high standing and traits
            var legacy = new CampaignLegacy
            {
                campaignId = "camp_victory_01",
                endingId = "ending_stand_up",
                daysSurvived = 120,
                survivorCount = 12,
                deathsRecorded = 0,
                completionDay = 120,
                legacyTraits = new List<string> { "trait_leaders_blood", "trait_fortified_walls" },
                factionStandings = new Dictionary<string, float> { { "settlers", 80f }, { "raiders", -70f } }
            };
            system.ArchiveCampaign(legacy);

            // Prepare new game context
            var startingContext = system.PrepareNewGameContext();
            Assert.NotNull(startingContext);
            Assert.True(startingContext.factionModifiers.ContainsKey("settlers"), "Allied faction should pass positive standing bonus");
            Assert.True(startingContext.factionModifiers["settlers"] > 0f, "Settlers standing should be positive");
            Assert.NotEmpty(startingContext.inheritedTraits);
        }
    }
}
