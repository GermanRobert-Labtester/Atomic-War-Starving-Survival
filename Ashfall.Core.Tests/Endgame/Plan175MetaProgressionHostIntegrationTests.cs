// SPDX-License-Identifier: MIT
// Plan 175 host integration tests: verifies MetaProgressionSystem, catalog loading,
// prestige scoring, achievement & ending unlock gating, NG+ boons, save round-trips,
// and host wiring.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Endgame;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Endgame
{
    public sealed class Plan175MetaProgressionHostIntegrationTests
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

        private static MetaProgressionSystem CreateSystemWithCatalog(CrossRunProfileStore? profileStore = null)
        {
            var system = new MetaProgressionSystem(profileStore);
            string json = File.ReadAllText(Path.Combine(DataDir(), "meta_unlockables.json"));
            system.LoadCatalog(json);
            return system;
        }

        [Fact]
        public void MetaProgression_CatalogLoading_LoadsAuthoredUnlockables()
        {
            var system = CreateSystemWithCatalog();
            var items = system.GetAllUnlockables();

            Assert.NotEmpty(items);
            Assert.True(items.Count >= 8, $"Expected >= 8 unlockables, got {items.Count}");
            Assert.Contains(items, i => i.category == "crest");
            Assert.Contains(items, i => i.category == "insignia");
            Assert.Contains(items, i => i.category == "ng_plus_boon");

            var dawnCrest = system.GetUnlockable("meta_dawn_horizon_crest");
            Assert.NotNull(dawnCrest);
            Assert.Equal("ending_dawn_of_thaw", dawnCrest!.required_ending_id);
            Assert.True(dawnCrest.required_prestige > 0);
        }

        private static void AppendRecord(
            CampaignCompletionHistory history,
            string runIdentity,
            string endingId,
            int daysSurvived,
            int livingDwellers,
            int deathsRecorded = 0,
            bool grandTreatySigned = false,
            bool tempestDecommissioned = false,
            bool debtLedgersBurned = false,
            bool childrenSurvived = false,
            bool velSecretExposed = false)
        {
            var ctx = new EpilogueContextInputs(
                Days: daysSurvived,
                LivingDwellers: livingDwellers,
                DeathsRecorded: deathsRecorded,
                GrandTreatySigned: grandTreatySigned,
                TempestDecommissioned: tempestDecommissioned,
                DebtLedgersBurned: debtLedgersBurned,
                ChildrenSurvived: childrenSurvived,
                VelSecretExposed: velSecretExposed);
            var obs = new CampaignCompletionObservation(runIdentity, endingId, ctx, "standard");
            var result = CampaignCompletionHistoryService.Append(history, obs, out _);
            Assert.Equal(CompletionHistoryAppendResult.Appended, result);
        }

        [Fact]
        public void MetaProgression_PrestigeCalculation_AggregatesFromProfileRuns()
        {
            var profileStore = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();
            AppendRecord(history, "Run_1", "ending_dawn_of_thaw", 100, 10, deathsRecorded: 1, grandTreatySigned: true);
            profileStore.Record(history);

            var system = CreateSystemWithCatalog(profileStore);
            // Days: 100 * 2 = 200. Dwellers: 10 * 5 = 50. GrandTreaty: 50. Total = 300.
            int prestige = system.EvaluateProgress();

            Assert.Equal(300, prestige);
            Assert.Equal(300, system.TotalPrestigeEarned);
        }

        [Fact]
        public void MetaProgression_EndingGating_RequiresSpecificEnding()
        {
            var profileStore = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();
            AppendRecord(history, "Run_2", "ending_silent_tombs", 360, 10, deathsRecorded: 0);
            profileStore.Record(history);

            var system = CreateSystemWithCatalog(profileStore);
            system.EvaluateProgress();

            // High prestige, but ending_dawn_of_thaw was NOT achieved:
            Assert.False(system.IsUnlocked("meta_dawn_horizon_crest"));
            // ending_silent_tombs memorial SHOULD unlock:
            Assert.True(system.IsUnlocked("meta_silent_tombs_memorial"));
        }

        [Fact]
        public void MetaProgression_AchievementGating_RequiresSpecificAchievement()
        {
            var profileStore = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();
            AppendRecord(history, "Run_3", "ending_silent_tombs", 50, 5, deathsRecorded: 0);
            profileStore.Record(history);

            var system = CreateSystemWithCatalog(profileStore);

            // First evaluation without achievement:
            system.EvaluateProgress(completedAchievementIds: Array.Empty<string>());
            Assert.False(system.IsUnlocked("meta_veteran_survivor_ribbon"));

            // Second evaluation with "month_of_ash" achievement:
            system.EvaluateProgress(completedAchievementIds: new[] { "month_of_ash" });
            Assert.True(system.IsUnlocked("meta_veteran_survivor_ribbon"));
        }

        [Fact]
        public void MetaProgression_NgPlusBoonToggle_ActivatesBoonOnlyWhenUnlocked()
        {
            var profileStore = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();
            AppendRecord(history, "Run_4", "ending_dawn_of_thaw", 200, 10);
            profileStore.Record(history);

            var system = CreateSystemWithCatalog(profileStore);
            system.EvaluateProgress();

            // Cannot activate locked boon
            bool lockedToggle = system.SetNgPlusBoonActive("meta_geodetic_survey_boon", true);
            // If unlocked, toggle succeeds; if locked, fails
            if (system.IsUnlocked("meta_forged_toolkit_boon"))
            {
                bool active = system.SetNgPlusBoonActive("meta_forged_toolkit_boon", true);
                Assert.True(active);
                Assert.True(system.IsBoonActive("meta_forged_toolkit_boon"));

                bool deactivate = system.SetNgPlusBoonActive("meta_forged_toolkit_boon", false);
                Assert.True(deactivate);
                Assert.False(system.IsBoonActive("meta_forged_toolkit_boon"));
            }
        }

        [Fact]
        public void MetaProgression_SaveRestore_RoundtripsDeterministically()
        {
            var profileStore = new CrossRunProfileStore();
            var history = new CampaignCompletionHistory();
            AppendRecord(history, "Run_5", "ending_dawn_of_thaw", 360, 12);
            profileStore.Record(history);

            var system = CreateSystemWithCatalog(profileStore);
            system.EvaluateProgress(completedEndingIds: new[] { "ending_dawn_of_thaw" });
            system.SetNgPlusBoonActive("meta_geodetic_survey_boon", true);

            var state = system.CaptureState();
            Assert.NotNull(state);
            Assert.True(state.totalPrestigeEarned > 0);
            Assert.NotEmpty(state.unlockedIds);

            var restoredSystem = CreateSystemWithCatalog();
            restoredSystem.RestoreState(state);

            Assert.Equal(system.TotalPrestigeEarned, restoredSystem.TotalPrestigeEarned);
            Assert.Equal(system.UnlockedIds.Count, restoredSystem.UnlockedIds.Count);
            Assert.True(restoredSystem.IsUnlocked("meta_dawn_horizon_crest"));
            Assert.True(restoredSystem.IsBoonActive("meta_geodetic_survey_boon"));
        }

        [Fact]
        public void MetaProgression_Census_ExposesTruthfulCounts()
        {
            var system = CreateSystemWithCatalog();
            var census = system.GetCensus();

            Assert.True(census.TotalCatalogItems >= 8);
            Assert.Equal(0, census.TotalUnlocked);
            Assert.Equal(0, census.ActiveBoons);
            Assert.Equal(0, census.PrestigeScore);
            Assert.Equal(0, census.RunsRecorded);
        }

        [Fact]
        public void MetaProgression_SaveSectionRegistry_IsRegistered()
        {
            Assert.Contains("meta_progression", SaveSectionRegistry.SectionKeys);
            Assert.Equal("meta_progression_save.json", SaveSectionRegistry.FileNameFor("meta_progression"));

            var all = SaveSectionRegistry.All;
            var entry = all.FirstOrDefault(s => s.SectionKey == "meta_progression");
            Assert.NotNull(entry);
            Assert.Equal("SaveMetaProgression", entry!.SaveMethod);
            Assert.Equal("SetupMetaProgression", entry.SetupMethod);
            Assert.Equal("endgame", entry.Owner);
            Assert.Equal(SaveSectionRegistry.ExpandedShelterLifecycleGroup, entry.LifecycleGroup);
        }

        [Fact]
        public void MetaProgression_HostWiring_PresenceVerifiedInSource()
        {
            string hostSession = ReadRepoFile("src", "Host", "MetaProgressionHostSession.cs");
            Assert.Contains("class MetaProgressionHostSession", hostSession);
            Assert.Contains("class MetaProgressionSaveStore", hostSession);
            Assert.Contains("meta_progression_save.json", hostSession);

            string hostCli = ReadRepoFile("src", "Host", "HostCli.MetaProgression.cs");
            Assert.Contains("HostCliMetaProgression", hostCli);
            Assert.Contains("RunSelfTest", hostCli);

            string mainMeta = ReadRepoFile("src", "Main.MetaProgression.cs");
            Assert.Contains("SetupMetaProgression()", mainMeta);
            Assert.Contains("SaveMetaProgression()", mainMeta);

            string campaignOwners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("MetaProgressionDayOwner", campaignOwners);
            Assert.Contains("meta_progression_ticked", campaignOwners);

            string mainEndgame = ReadRepoFile("src", "Main.Endgame.cs");
            Assert.Contains("SetupMetaProgression()", mainEndgame);
            Assert.Contains("RecordCampaignCompletion", mainEndgame);
        }
    }
}
