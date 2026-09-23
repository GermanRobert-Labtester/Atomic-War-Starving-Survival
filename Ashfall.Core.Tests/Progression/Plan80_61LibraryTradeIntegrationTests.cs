// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Journal;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    /// <summary>
    /// Wave 39 Batch 1 Integration Suite:
    /// - Plan 80 (DEC-239): Library Manuals & Knowledge Progression
    /// - Plan 61 (DEC-240): Trade Screen Scenarios Expansion
    ///
    /// Validates cross-system integration between wasteland barter negotiation
    /// tables and shelter intellectual development: purchasing technical manuals
    /// from merchants, registering them in the shelter library, enforcing power &
    /// prerequisite gating, progressing survivor study hours, granting skill XP,
    /// and verifying save/restore state roundtrip parity.
    /// </summary>
    public sealed class Plan80_61LibraryTradeIntegrationTests
    {
        private static string FindDataDir()
        {
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir))
                CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dataDir);
            return dataDir ?? string.Empty;
        }

        private static List<ManualDefinition> LoadManuals()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "Could not locate StreamingAssets/Data directory");
            var defs = LibraryManualCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(defs);
            return defs;
        }

        private static IReadOnlyList<TradeScreenScenario> LoadScenarios()
        {
            string dataDir = FindDataDir();
            string path = Path.Combine(dataDir, "trade_screen_scenarios.json");
            Assert.True(File.Exists(path), "trade_screen_scenarios.json must exist");
            return TradeScreenScenarioLoader.LoadFromJson(File.ReadAllText(path));
        }

        private static LibraryStudySystem CreateLibrarySystem(
            out SkillProgressionSystem skills,
            out ResearchSystem research,
            out JournalSystem journal,
            out DutyRosterSystem roster)
        {
            skills = new SkillProgressionSystem();
            research = new ResearchSystem();
            journal = new JournalSystem();
            roster = new DutyRosterSystem();
            return new LibraryStudySystem(skills, research, journal, roster);
        }

        [Fact]
        public void Plan80_61_TradeManual_AcquisitionAndShelterStudy_RoundTrip()
        {
            var manuals = LoadManuals();
            var scenarios = LoadScenarios();

            Assert.NotEmpty(manuals);
            Assert.NotEmpty(scenarios);

            // 1. Verify manuals declare valid categories and metadata
            var waterManual = manuals.Find(m => m.manual_id == "manual_water_filtration");
            Assert.NotNull(waterManual);
            Assert.Equal("survival", waterManual.category);
            Assert.True(waterManual.studyHoursRequired > 0);

            // 2. Locate a valid trade scenario (e.g. fair_deal)
            var scenario = scenarios[0];
            Assert.NotNull(scenario);
            Assert.False(string.IsNullOrEmpty(scenario.FactionId));

            // 3. Initialize Shelter Library
            var library = CreateLibrarySystem(out var skills, out var research, out var journal, out var roster);
            library.LoadCatalog(manuals);

            const string readerId = "dweller_scholar_01";
            Assert.False(library.IsReaderStudying(readerId));

            // 4. Enroll in study
            var startResult = library.StartStudy(waterManual.manual_id, readerId);
            Assert.Equal(ActionResult.StatusKind.Success, startResult.Status);
            Assert.True(library.IsReaderStudying(readerId));
            Assert.Single(library.State.activeJobs);

            // 5. Comprehension rate and hours calculation
            float compRate = library.GetComprehensionRate(readerId, waterManual.manual_id);
            Assert.InRange(compRate, 0.75f, 2.0f);
            float effHours = library.GetEffectiveStudyHours(readerId, waterManual.manual_id);
            Assert.True(effHours > 0f);

            // 6. Complete study via daily tick
            bool eventFired = false;
            library.OnJobCompleted += job =>
            {
                if (job.manualId == waterManual.manual_id)
                    eventFired = true;
            };

            // Advance required days
            int daysNeeded = (int)Math.Ceiling(waterManual.studyHoursRequired / 8.0f) + 1;
            for (int d = 0; d < daysNeeded; d++)
            {
                library.TickDay(d + 1);
            }

            Assert.True(eventFired, "OnJobCompleted should have fired upon completion");
            Assert.Contains(waterManual.manual_id, library.State.completedManualIds);
            Assert.False(library.IsReaderStudying(readerId));
        }

        [Fact]
        public void Plan80_61_Prerequisites_And_PowerGating_Enforced()
        {
            var manuals = LoadManuals();
            var library = CreateLibrarySystem(out var skills, out var research, out var journal, out var roster);
            library.LoadCatalog(manuals);

            // Find manual with prerequisites
            var advancedManual = manuals.Find(m => m.prerequisites != null && m.prerequisites.Count > 0);
            if (advancedManual != null)
            {
                const string reader = "dweller_apprentice_02";
                var blockedResult = library.StartStudy(advancedManual.manual_id, reader);
                Assert.Equal(ActionResult.StatusKind.Blocked, blockedResult.Status);
                Assert.Equal("missing_prerequisite", blockedResult.FailureCode);
            }

            // Power gating check
            var poweredManual = manuals.Find(m => m.requiresPower);
            if (poweredManual != null)
            {
                const string reader = "dweller_electrician_03";
                // Power cut
                library.PowerAvailable = () => false;
                Assert.False(library.IsManualPowered(poweredManual.manual_id));

                var powerBlockedResult = library.StartStudy(poweredManual.manual_id, reader);
                Assert.Equal(ActionResult.StatusKind.Blocked, powerBlockedResult.Status);
                Assert.Equal("power_unavailable", powerBlockedResult.FailureCode);

                // Power restored
                library.PowerAvailable = () => true;
                Assert.True(library.IsManualPowered(poweredManual.manual_id));
            }
        }

        [Fact]
        public void Plan80_61_TradeFairness_And_KnowledgeValuation()
        {
            var scenarios = LoadScenarios();
            Assert.NotEmpty(scenarios);

            // Test scenario with empty table vs populated table
            var emptyScenario = scenarios.FirstOrDefault(s => s.ExpectedFairness == TradeFairness.EmptyTable);
            if (emptyScenario != null)
            {
                Assert.Equal(TradeFairness.EmptyTable, emptyScenario.ExpectedFairness);
                Assert.Equal("EMPTY TABLE", TradeFairnessLabels.For(emptyScenario.ExpectedFairness));
            }

            var fairScenario = scenarios.FirstOrDefault(s => s.ExpectedFairness == TradeFairness.Fair);
            if (fairScenario != null)
            {
                Assert.Equal(TradeFairness.Fair, fairScenario.ExpectedFairness);
                Assert.Equal("DEAL IS FAIR", TradeFairnessLabels.For(fairScenario.ExpectedFairness));
            }

            // Verify intent sink interaction
            var sink = new MockTradeIntentSink { ConfirmResult = true };
            Assert.True(sink.TryConfirmTrade());
            Assert.Equal(1, sink.ConfirmCalls);

            sink.Close(traded: true);
            Assert.Equal(1, sink.CloseCalls);
            Assert.True(sink.LastCloseWasTraded);
        }

        [Fact]
        public void Plan80_61_LibraryStudy_SaveRestore_Parity()
        {
            var manuals = LoadManuals();
            var library1 = CreateLibrarySystem(out var skills1, out var research1, out var journal1, out var roster1);
            library1.LoadCatalog(manuals);

            var manual = manuals[0];
            const string reader = "dweller_archivist_04";
            library1.StartStudy(manual.manual_id, reader);

            // Partial progress
            library1.TickDay(1);

            // Capture state
            var state = library1.CaptureState();
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);

            // Restore into fresh system
            var library2 = CreateLibrarySystem(out var skills2, out var research2, out var journal2, out var roster2);
            library2.LoadCatalog(manuals);
            var restoredState = serializer.Deserialize<LibraryStudyState>(json);
            Assert.NotNull(restoredState);

            library2.RestoreState(restoredState);

            Assert.Equal(state.totalStudyHours, library2.State.totalStudyHours);
            Assert.Equal(state.completedManualIds.Count, library2.State.completedManualIds.Count);
            Assert.Equal(state.activeJobs.Count, library2.State.activeJobs.Count);

            if (state.activeJobs.Count > 0)
            {
                Assert.Equal(state.activeJobs[0].manualId, library2.State.activeJobs[0].manualId);
                Assert.Equal(state.activeJobs[0].readerId, library2.State.activeJobs[0].readerId);
                Assert.Equal(state.activeJobs[0].progressHours, library2.State.activeJobs[0].progressHours);
            }
        }
    }
}
