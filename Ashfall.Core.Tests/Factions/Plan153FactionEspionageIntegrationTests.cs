// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Factions;
using Ashfall.Core.Random;

namespace Ashfall.Core.Tests.Plan153Espionage
{
    public class Plan153FactionEspionageIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string filename = "espionage_operations.json";
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return File.ReadAllText(c);
            }
            throw new FileNotFoundException($"Could not find {filename} in candidate paths.");
        }

        [Fact]
        public void CatalogLoad_LoadsAuthoredCovertOperations()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);

            Assert.NotNull(catalog);
            Assert.True(catalog.AllOperations.Count >= 5, $"Expected >= 5 operations, found {catalog.AllOperations.Count}");

            Assert.True(catalog.TryGetOperation("infiltrate_garrison", out var op1));
            Assert.Equal("infiltrate", op1.OperationType);
            Assert.Equal("faction_garrison", op1.TargetFactionId);

            Assert.True(catalog.TryGetOperation("steal_hydro_blueprints", out var op2));
            Assert.Equal("steal", op2.OperationType);
            Assert.Equal("faction_hydro_barons", op2.TargetFactionId);

            Assert.True(catalog.TryGetOperation("sabotage_supply_depot", out var op3));
            Assert.Equal("sabotage", op3.OperationType);

            Assert.True(catalog.TryGetOperation("covert_propaganda_campaign", out var op4));
            Assert.Equal("propaganda", op4.OperationType);

            Assert.True(catalog.TryGetOperation("covert_assassination_contract", out var op5));
            Assert.Equal("assassinate", op5.OperationType);
            Assert.Equal("extreme", op5.RiskLevel);
        }

        [Fact]
        public void OperationLaunch_GatingAndSecurityPenalties()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator = new FactionCovertOpsCoordinator(catalog);

            bool launched = false;
            coordinator.OnCovertOperationLaunchedSeam += (opId, agId, target) =>
            {
                launched = true;
                Assert.Equal("infiltrate_garrison", opId);
                Assert.Equal("agent_vera", agId);
                Assert.Equal("faction_garrison", target);
            };

            Assert.True(coordinator.LaunchOperation("infiltrate_garrison", "agent_vera", agentStealthSkill: 1.1f, currentDay: 5));
            Assert.True(launched);
            Assert.Single(coordinator.ActiveOperations);

            // Same agent cannot be launched on concurrent operation
            Assert.False(coordinator.LaunchOperation("steal_hydro_blueprints", "agent_vera", agentStealthSkill: 1.0f, currentDay: 5));
        }

        [Fact]
        public void OperationResolution_Success_YieldsRawIntelligence()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator = new FactionCovertOpsCoordinator(catalog);
            var rng = new SeededRng(101);

            coordinator.LaunchOperation("steal_hydro_blueprints", "agent_felix", agentStealthSkill: 1.2f, currentDay: 1);
            var op = coordinator.ActiveOperations[0];

            bool resolvedFired = false;
            coordinator.OnCovertOperationResolvedSeam += (opId, agId, status) =>
            {
                resolvedFired = true;
                Assert.Equal("steal_hydro_blueprints", opId);
                Assert.Equal("agent_felix", agId);
                Assert.Equal(CovertOperationStatus.Succeeded, status);
            };

            coordinator.ResolveOperation(op, rng, currentDay: 8, forceSuccess: true);

            Assert.True(resolvedFired);
            Assert.Equal(CovertOperationStatus.Succeeded, op.Status);
            Assert.Single(coordinator.Reports);

            var report = coordinator.Reports[0];
            Assert.Equal("faction_hydro_barons", report.SourceFactionId);
            Assert.Equal("technical", report.IntelligenceType);
            Assert.False(report.IsDecoded);
            Assert.True(report.Value >= 50);
        }

        [Fact]
        public void OperationResolution_FailureOrCompromised_IncreasesSuspicion()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator = new FactionCovertOpsCoordinator(catalog);
            var rng = new SeededRng(555);

            coordinator.LaunchOperation("covert_assassination_contract", "agent_boris", agentStealthSkill: 0.8f, currentDay: 1);
            var op = coordinator.ActiveOperations[0];

            bool compromiseFired = false;
            coordinator.OnAgentCompromisedSeam += (agId, fId, reason) =>
            {
                compromiseFired = true;
                Assert.Equal("agent_boris", agId);
                Assert.Equal("raiders", fId);
            };

            coordinator.ResolveOperation(op, rng, currentDay: 11, forceCompromise: true);

            Assert.True(compromiseFired);
            Assert.Equal(CovertOperationStatus.Compromised, op.Status);
            Assert.True(coordinator.GetSuspicion("raiders") >= 50f);
            Assert.True(coordinator.GetSuspicionBand("raiders") >= SuspicionBand.Hostile);
        }

        [Fact]
        public void IntelligenceDecoding_RevealsIntelValue()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator = new FactionCovertOpsCoordinator(catalog);
            var rng = new SeededRng(2026);

            coordinator.LaunchOperation("infiltrate_garrison", "agent_tanya", agentStealthSkill: 1.0f, currentDay: 1);
            var op = coordinator.ActiveOperations[0];
            coordinator.ResolveOperation(op, rng, currentDay: 15, forceSuccess: true);

            var report = coordinator.Reports[0];
            Assert.False(report.IsDecoded);

            bool decodeFired = false;
            coordinator.OnIntelligenceDecodedSeam += (repId, fId, type, val) =>
            {
                decodeFired = true;
                Assert.Equal(report.ReportId, repId);
                Assert.Equal("faction_garrison", fId);
                Assert.Equal("military", type);
                Assert.Equal(40, val);
            };

            Assert.True(coordinator.DecodeIntelligenceReport(report.ReportId));
            Assert.True(decodeFired);
            Assert.True(report.IsDecoded);

            // Duplicate decoding returns false
            Assert.False(coordinator.DecodeIntelligenceReport(report.ReportId));
        }

        [Fact]
        public void Persistence_CaptureAndRestoreRoundtrip()
        {
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator1 = new FactionCovertOpsCoordinator(catalog);
            var rng = new SeededRng(999);

            coordinator1.AddSuspicion("faction_garrison", 45f);
            coordinator1.LaunchOperation("infiltrate_garrison", "agent_nadia", currentDay: 1);
            var op = coordinator1.ActiveOperations[0];
            coordinator1.ResolveOperation(op, rng, currentDay: 15, forceSuccess: true);

            string saved = coordinator1.CaptureState();
            Assert.Contains("faction_garrison", saved);
            Assert.Contains("infiltrate_garrison", saved);
            Assert.Contains("agent_nadia", saved);

            var coordinator2 = new FactionCovertOpsCoordinator(catalog);
            coordinator2.RestoreState(saved);

            Assert.True(coordinator2.GetSuspicion("faction_garrison") >= 45f);
            Assert.Single(coordinator2.Reports);
            Assert.Equal("faction_garrison", coordinator2.Reports[0].SourceFactionId);
        }

        [Fact]
        public void ActiveOperations_SurviveCaptureRestore()
        {
            // ORPHAN-SEAL-W1 regression: RestoreState used to drop every
            // in-flight operation even though CaptureState serialized them.
            string json = GetCatalogJson();
            var catalog = CovertOpsCatalog.LoadFromJson(json);
            var coordinator = new FactionCovertOpsCoordinator(catalog);
            Assert.True(coordinator.LaunchOperation("infiltrate_garrison", "agent_nadia", 1.0f, 1));
            var original = coordinator.ActiveOperations[0];

            var reloaded = new FactionCovertOpsCoordinator(catalog);
            reloaded.RestoreState(coordinator.CaptureState());

            var restored = Assert.Single(reloaded.ActiveOperations);
            Assert.Equal(original.OperationId, restored.OperationId);
            Assert.Equal(original.AssignedAgentId, restored.AssignedAgentId);
            Assert.Equal(original.TargetFactionId, restored.TargetFactionId);
            Assert.Equal(original.RemainingDays, restored.RemainingDays);
            Assert.Equal(original.Status, restored.Status);
        }
    }
}
