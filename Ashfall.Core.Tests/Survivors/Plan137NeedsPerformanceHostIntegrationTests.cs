#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 137 — Needs → Performance Cascade host-integration gate.
//
// Pins the production wiring contract:
//   * NeedsPerformanceBridge acts as the single domain read-model over survivor needs,
//   * monotonic performance band progression (Optimal -> Impaired -> Severe -> Critical),
//   * event vocabulary registers "needs_performance_ticked" as Heartbeat,
//   * HostCliRegistry exposes NeedsPerformanceSelfTest (--needs-performance-selftest),
//   * host files (NeedsPerformanceHostSession, HostCli.NeedsPerformance, Main.NeedsPerformance)
//     exist, wire into campaign day owners, and lifecycle hooks,
//   * zero parallel shadow state or save duplicate per Rule 5.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan137NeedsPerformanceHostIntegrationTests
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

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        [Fact]
        public void NeedsPerformance_PureProjection_ProducesExpectedBands()
        {
            var healthy = new SurvivorNeedsState { Id = "s1", Hunger = 5f, Thirst = 5f, Fatigue = 5f, Warmth = 95f, Morale = 60f };
            var impaired = new SurvivorNeedsState { Id = "s2", Hunger = 45f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 50f };
            var severe = new SurvivorNeedsState { Id = "s3", Hunger = 75f, Thirst = 10f, Fatigue = 10f, Warmth = 90f, Morale = 50f };
            var critical = new SurvivorNeedsState { Id = "s4", Hunger = 95f, Thirst = 90f, Fatigue = 85f, Warmth = 20f, Morale = 10f };

            var mHealthy = NeedsPerformanceBridge.Project(healthy);
            var mImpaired = NeedsPerformanceBridge.Project(impaired);
            var mSevere = NeedsPerformanceBridge.Project(severe);
            var mCritical = NeedsPerformanceBridge.Project(critical);

            Assert.Equal(PerformanceBand.Optimal, mHealthy.OverallBand);
            Assert.Equal(PerformanceBand.Impaired, mImpaired.OverallBand);
            Assert.Equal(PerformanceBand.Severe, mSevere.OverallBand);
            Assert.Equal(PerformanceBand.Critical, mCritical.OverallBand);

            Assert.True(mCritical.WorkSpeedMultiplier < mSevere.WorkSpeedMultiplier);
            Assert.True(mSevere.WorkSpeedMultiplier < mImpaired.WorkSpeedMultiplier);
            Assert.True(mImpaired.WorkSpeedMultiplier < mHealthy.WorkSpeedMultiplier);
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesNeedsPerformanceHeartbeat()
        {
            var kind = DayEventVocabulary.GetSemanticKind("needs_performance_ticked");
            Assert.Equal(SemanticKind.Heartbeat, kind);
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("needs_performance_ticked"));

            string matrix = ReadRepoFile("docs", "campaign", "EVENT_SEMANTIC_PARITY_MATRIX.md");
            Assert.Contains("needs_performance_ticked", matrix);
        }

        [Fact]
        public void HostCliRegistry_RegistersNeedsPerformanceSelfTest()
        {
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.NeedsPerformanceSelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--needs-performance-selftest", desc!.PrimaryFlag);
            Assert.Contains("--needs-perf-selftest", desc.Aliases);
        }

        [Fact]
        public void HostWiring_SourceFilesExistAndDeclareSeams()
        {
            string hostSession = ReadRepoFile("src", "Host", "NeedsPerformanceHostSession.cs");
            Assert.Contains("class NeedsPerformanceHostSession", hostSession);
            Assert.Contains("NeedsPerformanceBridge.Project", hostSession);

            string hostCli = ReadRepoFile("src", "Host", "HostCli.NeedsPerformance.cs");
            Assert.Contains("RunNeedsPerformanceSelfTest", hostCli);

            string mainNeedsPerf = ReadRepoFile("src", "Main.NeedsPerformance.cs");
            Assert.Contains("EnsureNeedsPerformance", mainNeedsPerf);
            Assert.Contains("SetupNeedsPerformance", mainNeedsPerf);
            Assert.Contains("ResetNeedsPerformance", mainNeedsPerf);
            Assert.Contains("TickNeedsPerformance", mainNeedsPerf);
            Assert.Contains("GetNeedsPerformanceModifiers", mainNeedsPerf);
            Assert.Contains("GetNeedsPerformanceCensus", mainNeedsPerf);

            string campaignOwners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("NeedsPerformanceDayOwner", campaignOwners);
            Assert.Contains("\"needs_performance\"", campaignOwners);

            string expandedShelter = ReadRepoFile("src", "Main.ExpandedShelterSystems.cs");
            Assert.Contains("SetupNeedsPerformance();", expandedShelter);
            Assert.Contains("ResetNeedsPerformance();", expandedShelter);

            string mainApp = ReadRepoFile("src", "Main.Application.cs");
            Assert.Contains("HostCliAction.NeedsPerformanceSelfTest", mainApp);
        }

        [Fact]
        public void DownstreamConsumerSeams_AreWiredInHost()
        {
            // TacticalCombatSystem wiring in Main.Expeditions.cs
            string mainExpeditions = ReadRepoFile("src", "Main.Expeditions.cs");
            Assert.Contains("_combat.Engine.PerformanceLookup", mainExpeditions);
            Assert.Contains("GetNeedsPerformanceModifiers", mainExpeditions);

            // ExpeditionSystem speed multiplier wiring in Main.Expeditions.cs
            Assert.Contains("_expeditions.Engine.SetSurvivorSpeedMultiplierQuery", mainExpeditions);

            // DutyRosterSystem work speed multiplier wiring in Main.DutyRoster.cs
            string mainDutyRoster = ReadRepoFile("src", "Main.DutyRoster.cs");
            Assert.Contains("_dutyRoster.Roster.WorkSpeedMultiplierLookup", mainDutyRoster);

            // UI presentation in SurvivorDetailPanel.cs
            string uiDetailPanel = ReadRepoFile("src", "UI", "SurvivorDetailPanel.cs");
            Assert.Contains("NeedsPerformanceBridge.Project", uiDetailPanel);
            Assert.Contains("Performance:", uiDetailPanel);

            // Core interface contracts
            string coreBridge = ReadRepoFile("Assets", "Ashfall.Core", "Survivors", "NeedsPerformanceBridge.cs");
            Assert.Contains("interface ICombatPerformanceModifier", coreBridge);
            Assert.Contains("interface IWorkEfficiencyModifier", coreBridge);
            Assert.Contains("interface IExpeditionPerformanceModifier", coreBridge);
        }

        [Fact]
        public void DownstreamConsumers_ReceiveScaledModifiers_WhenNeedsDegrade()
        {
            var starving = new SurvivorNeedsState { Id = "starving_joe", Hunger = 95f, Thirst = 20f, Fatigue = 20f, Warmth = 90f, Morale = 40f };
            var exhausted = new SurvivorNeedsState { Id = "tired_ann", Hunger = 10f, Thirst = 10f, Fatigue = 95f, Warmth = 90f, Morale = 40f };

            var starvingMods = NeedsPerformanceBridge.Project(starving);
            var exhaustedMods = NeedsPerformanceBridge.Project(exhausted);

            // Combat lookup contract: (accuracy, damage)
            Assert.True(starvingMods.CombatAccuracyMultiplier <= 0.60f, "Starving survivor accuracy should be severely penalized");
            Assert.True(starvingMods.CombatDamageMultiplier <= 0.60f, "Starving survivor damage should be severely penalized");

            // Duty roster work speed lookup contract
            Assert.True(exhaustedMods.WorkSpeedMultiplier <= 0.50f, "Exhausted survivor work speed should be severely penalized");

            // Expedition speed lookup contract
            Assert.True(exhaustedMods.ExpeditionSpeedMultiplier <= 0.60f, "Exhausted survivor expedition speed should be penalized");
            Assert.True(exhaustedMods.ExpeditionStaminaDrainMultiplier >= 1.50f, "Exhausted survivor stamina drain should be amplified");
        }
    }
}
