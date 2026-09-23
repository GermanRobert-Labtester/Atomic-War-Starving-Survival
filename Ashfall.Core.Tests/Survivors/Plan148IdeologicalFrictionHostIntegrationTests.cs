// SPDX-License-Identifier: MIT
// Plan 148 host integration tests: verifies IdeologicalFrictionEvents & IdeologicalFrictionSystem,
// catalog loading, roommate compatibility, confrontation triggering, mediation resolution, save round-trips, and host wiring.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan148IdeologicalFrictionHostIntegrationTests
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

        private static IdeologicalFrictionEvents CreateEventsWithCatalog()
        {
            var events = new IdeologicalFrictionEvents();
            string json = File.ReadAllText(Path.Combine(DataDir(), "ideological_events.json"));
            events.LoadCatalog(json);
            return events;
        }

        [Fact]
        public void IdeologicalFriction_CatalogLoading_LoadsAuthoredEventTemplates()
        {
            var events = CreateEventsWithCatalog();
            var templates = events.Templates;

            Assert.NotEmpty(templates);
            Assert.True(templates.Count >= 8, $"Expected >= 8 event templates, got {templates.Count}");
            Assert.Contains(templates, t => t.event_type == "confrontation");
            Assert.Contains(templates, t => t.event_type == "conversion");
            Assert.Contains(templates, t => t.event_type == "split");
            Assert.Contains(templates, t => t.event_type == "quest");
        }

        [Fact]
        public void IdeologicalFriction_RoommateCompatibility_ConflictPenalizesSleep()
        {
            var friction = new IdeologicalFrictionSystem();

            friction.RegisterBelief("survivor_soldier", "military_discipline");
            friction.RegisterBelief("survivor_healer", "pacifist");

            float mult = friction.GetRoommateCompatibilityMultiplier("survivor_soldier", "survivor_healer");
            Assert.Equal(0.80f, mult, precision: 2);
        }

        [Fact]
        public void IdeologicalFriction_RoommateCompatibility_SynergyBonusesSleep()
        {
            var friction = new IdeologicalFrictionSystem();

            friction.RegisterBelief("survivor_priest", "religious_faith");
            friction.RegisterBelief("survivor_monk", "religious_faith");

            float mult = friction.GetRoommateCompatibilityMultiplier("survivor_priest", "survivor_monk");
            Assert.Equal(1.10f, mult, precision: 2);
        }

        [Fact]
        public void IdeologicalFriction_TickRoommates_DrainsAffinityOverTime()
        {
            var friction = new IdeologicalFrictionSystem();

            friction.RegisterBelief("s_militant", "military_discipline");
            friction.RegisterBelief("s_pacifist", "pacifist");

            float affInitial = friction.GetAffinity("s_militant", "s_pacifist");
            friction.TickRoommates("s_militant", "s_pacifist", gameHours: 48f);
            float affAfter = friction.GetAffinity("s_militant", "s_pacifist");

            Assert.True(affAfter < affInitial);
        }

        [Fact]
        public void IdeologicalFriction_CheckDailyFriction_TriggersConfrontationAtNegativeAffinity()
        {
            var events = CreateEventsWithCatalog();

            var instance = events.CheckDailyFriction(
                survivorA: "survivor_soldier",
                beliefA: "military_discipline",
                survivorB: "survivor_healer",
                beliefB: "pacifist",
                pairAffinity: -60f,
                currentDay: 7,
                forceTrigger: true);

            Assert.NotNull(instance);
            Assert.Equal(IdeologicalEventType.Confrontation, instance.eventType);
            Assert.False(instance.isResolved);
        }

        [Fact]
        public void IdeologicalFriction_ResolveConfrontation_BrokerCompromiseRestoresMoraleAndResolves()
        {
            var events = CreateEventsWithCatalog();

            var instance = events.CheckDailyFriction(
                survivorA: "survivor_soldier",
                beliefA: "military_discipline",
                survivorB: "survivor_healer",
                beliefB: "pacifist",
                pairAffinity: -60f,
                currentDay: 7,
                forceTrigger: true);

            Assert.NotNull(instance);

            bool ok = events.ResolveConfrontation(
                instance.instanceId,
                IdeologicalMediationChoice.BrokerCompromise,
                out float deltaA,
                out float deltaB,
                out float moraleDelta);

            Assert.True(ok);
            Assert.True(deltaA > 0f);
            Assert.True(deltaB > 0f);
            Assert.True(moraleDelta > 0f);
            Assert.True(instance.isResolved);
        }

        [Fact]
        public void IdeologicalFriction_AttemptConversion_CanConvertBeliefDeterministically()
        {
            var events = CreateEventsWithCatalog();
            var rng = new SeededRng(42);

            bool converted = events.AttemptConversion(
                actorId: "preacher",
                targetId: "doubter",
                actorBelief: "religious_faith",
                successProbability: 1.0f,
                rng: rng,
                out string resultMessage);

            Assert.True(converted);
            Assert.Contains("religious_faith", resultMessage);
            Assert.Equal(1, events.TotalConversionsSucceeded);
        }

        [Fact]
        public void IdeologicalFriction_BunkerFactions_FormsGroupsForSharedBeliefs()
        {
            var events = CreateEventsWithCatalog();
            var beliefs = new Dictionary<string, string>
            {
                { "s1", "military_discipline" },
                { "s2", "military_discipline" },
                { "s3", "military_discipline" },
                { "s4", "pacifist" }
            };

            var factions = events.UpdateBunkerFactions(beliefs);
            Assert.Single(factions);
            Assert.Equal("military_discipline", factions[0].beliefId);
            Assert.Equal(3, factions[0].memberIds.Count);
        }

        [Fact]
        public void IdeologicalFriction_SaveRestore_RoundTripPreservesState()
        {
            var events = CreateEventsWithCatalog();

            var instance = events.CheckDailyFriction(
                "s1", "military_discipline",
                "s2", "pacifist",
                -60f,
                currentDay: 3,
                forceTrigger: true);

            Assert.NotNull(instance);
            events.AttemptConversion("s1", "s2", "military_discipline", 1.0f, new SeededRng(1), out _);

            var state = events.CaptureState();
            Assert.Equal(1, state.schema_version);
            Assert.Equal(1, state.totalEventsFired);
            Assert.Equal(1, state.totalConversionsSucceeded);

            var restored = CreateEventsWithCatalog();
            restored.RestoreState(state);

            Assert.Equal(1, restored.TotalEventsFired);
            Assert.Equal(1, restored.TotalConversionsSucceeded);
            Assert.Single(restored.RecentEvents);
            Assert.Equal(instance.instanceId, restored.RecentEvents[0].instanceId);
        }

        [Fact]
        public void IdeologicalFriction_Census_ComputesAccurateCounts()
        {
            var events = CreateEventsWithCatalog();

            var census = events.GetCensus();
            Assert.True(census.LoadedTemplatesCount >= 8);
            Assert.Equal(0, census.TotalEventsFired);
            Assert.Equal(0, census.TotalConversionsSucceeded);
            Assert.Equal(0, census.ActiveFactionsCount);
            Assert.Equal(0, census.RecentEventsCount);
        }

        [Fact]
        public void IdeologicalFriction_HostWiring_FilesAndHooksAreInPlace()
        {
            // Verify SaveSectionRegistry
            Assert.True(SaveSectionRegistry.TryGetSection("ideological_friction", out _));
            Assert.Equal("ideological_friction_save.json", SaveSectionRegistry.FileNameFor("ideological_friction"));

            // Verify DayEventVocabulary
            var kind = DayEventVocabulary.GetSemanticKind("ideological_friction_ticked");
            Assert.True(DayEventVocabulary.IsInternalHeartbeat("ideological_friction_ticked"));

            // Verify HostCliRegistry
            var desc = HostCliRegistry.AllDescriptors.FirstOrDefault(d => d.Action == HostCliAction.IdeologicalFrictionSelfTest);
            Assert.NotNull(desc);
            Assert.Equal("--ideological-friction-selftest", desc.PrimaryFlag);

            // Verify Host files exist and have required declarations
            string hostSessionCode = ReadRepoFile("src", "Host", "IdeologicalFrictionHostSession.cs");
            Assert.Contains("class IdeologicalFrictionHostSession", hostSessionCode);
            Assert.Contains("class IdeologicalFrictionSaveStore", hostSessionCode);

            string hostCliCode = ReadRepoFile("src", "Host", "HostCli.IdeologicalFriction.cs");
            Assert.Contains("static class HostCliIdeologicalFriction", hostCliCode);
            Assert.Contains("RunSelfTest", hostCliCode);

            string mainCode = ReadRepoFile("src", "Main.IdeologicalFriction.cs");
            Assert.Contains("SetupIdeologicalFriction", mainCode);
            Assert.Contains("SaveIdeologicalFriction", mainCode);

            string mainSave = ReadRepoFile("src", "Main.SaveOrchestrator.cs");
            Assert.Contains("SetupIdeologicalFriction", mainSave);
            Assert.Contains("SaveIdeologicalFriction", mainSave);

            string mainCamp = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("IdeologicalFrictionDayOwner", mainCamp);
        }
    }
}
