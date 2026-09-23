#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Governance;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public sealed class Plan59StandingGateIntegrationTests
    {
        private static string ResolveCatalogPath(string filename)
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Data", filename);
            if (!File.Exists(path))
            {
                path = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", filename));
            }
            if (!File.Exists(path))
            {
                path = Path.Combine("Assets", "StreamingAssets", "Data", filename);
            }
            return path;
        }

        [Fact]
        public void StandingGatesCatalog_LoadsAndParsesAll22Gates()
        {
            string path = ResolveCatalogPath("standing_gates.json");
            Assert.True(File.Exists(path), $"Catalog not found at {path}");

            string json = File.ReadAllText(path);
            var registry = StandingGateRegistry.FromJson(json);

            var allGates = registry.GetAllGates();
            Assert.Equal(22, allGates.Count);

            var epilogueGate = registry.GetGate("gate_epilogue_derived_state");
            Assert.NotNull(epilogueGate);
            Assert.Equal(1, epilogueGate!.WaveOrigin);
            Assert.Equal(GateTier.PerRelease, epilogueGate.Tier);
            Assert.Equal("NarrativeArchitect", epilogueGate.OwnerRole);
            Assert.True(epilogueGate.IsEnforced);

            var portSeamsGate = registry.GetGate("gate_port_contract_host_wiring");
            Assert.NotNull(portSeamsGate);
            Assert.Equal(5, portSeamsGate!.WaveOrigin);
            Assert.Equal(GateTier.Nightly, portSeamsGate.Tier);
            Assert.Equal("CoreIntegrator", portSeamsGate.OwnerRole);
        }

        [Fact]
        public void QueryGates_ByTierAndWave_ReturnsAccurateSubsets()
        {
            string path = ResolveCatalogPath("standing_gates.json");
            var registry = StandingGateRegistry.FromJson(File.ReadAllText(path));

            var perPushGates = registry.GetGatesByTier(GateTier.PerPush);
            var nightlyGates = registry.GetGatesByTier(GateTier.Nightly);
            var perReleaseGates = registry.GetGatesByTier(GateTier.PerRelease);

            Assert.NotEmpty(perPushGates);
            Assert.NotEmpty(nightlyGates);
            Assert.NotEmpty(perReleaseGates);
            Assert.Equal(22, perPushGates.Count + nightlyGates.Count + perReleaseGates.Count);

            var wave1Gates = registry.GetGatesByWave(1);
            Assert.Equal(4, wave1Gates.Count);

            var wave2Gates = registry.GetGatesByWave(2);
            Assert.Equal(4, wave2Gates.Count);

            var wave9Gates = registry.GetGatesByWave(9);
            Assert.Equal(2, wave9Gates.Count);
        }

        [Fact]
        public void AuditStandingGates_WhenAllPassing_ReportsSuccessAndInvokesSeams()
        {
            string path = ResolveCatalogPath("standing_gates.json");
            var registry = StandingGateRegistry.FromJson(File.ReadAllText(path));

            int evaluatedCount = 0;
            registry.OnGateEvaluatedSeam = (gate, passing) =>
            {
                evaluatedCount++;
                Assert.True(passing);
            };

            RetrospectiveAuditReport? auditedReport = null;
            registry.OnRetrospectiveAuditedSeam = report => auditedReport = report;

            // Mock provider where all gates pass
            bool allPassProvider(string gateId) => true;

            var report = registry.AuditStandingGates(allPassProvider);

            Assert.Equal(22, report.TotalGates);
            Assert.Equal(22, report.EvaluatedGates);
            Assert.Equal(22, report.PassingGates);
            Assert.True(report.AllGatesPassing);
            Assert.Empty(report.Violations);
            Assert.Equal(22, evaluatedCount);
            Assert.NotNull(auditedReport);
            Assert.Same(report, auditedReport);
        }

        [Fact]
        public void AuditStandingGates_WhenViolationsOccur_ProducesDetailedReportAndFiresViolationSeam()
        {
            string path = ResolveCatalogPath("standing_gates.json");
            var registry = StandingGateRegistry.FromJson(File.ReadAllText(path));

            var detectedViolations = new List<StandingGateViolation>();
            registry.OnGateViolationDetectedSeam = v => detectedViolations.Add(v);

            // Mock provider where gate_single_writer_rad and gate_asset_manifest_strict_tier fail
            bool failingProvider(string gateId)
            {
                return gateId != "gate_single_writer_rad" && gateId != "gate_asset_manifest_strict_tier";
            }

            var report = registry.AuditStandingGates(failingProvider);

            Assert.Equal(22, report.TotalGates);
            Assert.Equal(22, report.EvaluatedGates);
            Assert.Equal(20, report.PassingGates);
            Assert.False(report.AllGatesPassing);
            Assert.Equal(2, report.Violations.Count);
            Assert.Equal(2, detectedViolations.Count);

            var radViolation = report.Violations.Find(v => v.GateId == "gate_single_writer_rad");
            Assert.NotNull(radViolation);
            Assert.Equal(2, radViolation!.WaveOrigin);
            Assert.Equal("CoreIntegrator", radViolation.OwnerRole);
            Assert.Contains("WeatherSystem and RadiationLedger", radViolation.Reason);
        }

        [Fact]
        public void AuditStandingGates_WithTierFilter_OnlyEvaluatesTargetTier()
        {
            string path = ResolveCatalogPath("standing_gates.json");
            var registry = StandingGateRegistry.FromJson(File.ReadAllText(path));

            var perReleaseGates = registry.GetGatesByTier(GateTier.PerRelease);
            int expectedCount = perReleaseGates.Count;

            var evaluatedGateIds = new List<string>();
            registry.OnGateEvaluatedSeam = (gate, passing) => evaluatedGateIds.Add(gate.GateId);

            var report = registry.AuditStandingGates(_ => true, filterTier: GateTier.PerRelease);

            Assert.Equal(expectedCount, report.EvaluatedGates);
            Assert.Equal(expectedCount, report.PassingGates);
            Assert.True(report.AllGatesPassing);
            Assert.Equal(expectedCount, evaluatedGateIds.Count);

            foreach (var id in evaluatedGateIds)
            {
                var def = registry.GetGate(id);
                Assert.NotNull(def);
                Assert.Equal(GateTier.PerRelease, def!.Tier);
            }
        }
    }
}
