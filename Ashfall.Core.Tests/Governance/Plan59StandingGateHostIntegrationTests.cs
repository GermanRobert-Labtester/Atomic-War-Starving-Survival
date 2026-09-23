// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 59 — Retrospective: turn nine waves of findings into rules.
//
// Pins the production wiring contract for the standing-gate register:
//   * the 22-row register loads strictly through the Core loader,
//   * every gate carries an owner (Plan 59A step 2) and a tier,
//   * every gate is either bound to a real CI gate id or carries a written,
//     dated decision not to gate it (59A step 6),
//   * every referenced CI gate actually exists in the repo's own enforcement
//     manifest, is critical, and is tier-compatible,
//   * the register's own gates can fail (no self-proof, no gate — 59A step 3).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Governance;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public sealed class Plan59StandingGateHostIntegrationTests
    {
        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent;
            return dir?.FullName ?? throw new InvalidOperationException("repo root not found");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string EnforcementManifestPath() =>
            Path.Combine(RepoRoot(), "docs", "ci", "CI_GATE_MANIFEST.json");

        private static (StandingGateCatalogLoadResult register, CiGateManifestLoadResult manifest) Load()
        {
            var files = new Ashfall.Core.FileSystemIO();
            var register = StandingGateCatalogLoader.Load(DataDir(), files);
            var manifest = CiGateManifestReader.Load(files.ReadAllText(EnforcementManifestPath()));
            return (register, manifest);
        }

        // ── 1 — the register loads strictly ────────────────────────────────

        [Fact]
        public void StandingGateRegister_LoadsStrictly()
        {
            var (register, _) = Load();
            Assert.False(register.HasErrors, string.Join("; ", register.Errors));
            Assert.NotEmpty(register.Gates);
        }

        [Fact]
        public void StandingGateRegister_CoversAllTwentyTwoFindingClasses()
        {
            var (register, _) = Load();
            Assert.False(register.HasErrors, string.Join("; ", register.Errors));
            Assert.Equal(22, register.Gates.Count);
        }

        [Fact]
        public void EveryGate_HasAnOwner()
        {
            var (register, _) = Load();
            foreach (var gate in register.Gates)
                Assert.False(string.IsNullOrWhiteSpace(gate.OwnerRole),
                    $"standing gate '{gate.GateId}' has no owner_role — the Wave 3 failure mode (59A step 2)");
        }

        [Fact]
        public void EveryGate_CarriesAFindingClauseAndAnEnforcementRule()
        {
            var (register, _) = Load();
            foreach (var gate in register.Gates)
            {
                Assert.False(string.IsNullOrWhiteSpace(gate.FindingClause),
                    $"standing gate '{gate.GateId}' lost the finding it closes.");
                Assert.False(string.IsNullOrWhiteSpace(gate.EnforcementRule),
                    $"standing gate '{gate.GateId}' has no enforcement rule.");
            }
        }

        [Fact]
        public void EveryGate_HasAKnownTier()
        {
            var (register, _) = Load();
            foreach (var gate in register.Gates)
                Assert.Contains(gate.Tier, new[] { GateTier.PerPush, GateTier.Nightly, GateTier.PerRelease });
        }

        // ── 2 — every gate is gated by a real CI gate, or ruled on paper ────

        [Fact]
        public void EveryGate_IsGatedOrCarriesAWrittenDecision()
        {
            var files = new Ashfall.Core.FileSystemIO();
            var data = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                files.ReadAllText(Path.Combine(DataDir(), StandingGateCatalogLoader.FileName)),
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower
                });

            Assert.NotNull(data?.standing_gates);
            foreach (var row in data!.standing_gates!)
            {
                string reference = (row.enforcement_ref ?? string.Empty).Trim();
                bool gated = !string.Equals(reference, StandingGateCatalogLoader.NoEnforcementRef,
                    StringComparison.OrdinalIgnoreCase);

                if (gated)
                {
                    // A gated gate does not need a human rule; carrying one would
                    // mean the code path and the paper path disagree.
                    Assert.True(string.IsNullOrWhiteSpace(row.non_gate_rule),
                        $"gate '{row.gate_id}' binds '{reference}' and must not also carry a non_gate_rule");
                }
                else
                {
                    // 59A step 6 — the honest alternative to a gate.
                    Assert.False(string.IsNullOrWhiteSpace(row.non_gate_rule),
                        $"gate '{row.gate_id}' declares no enforcement_ref and no written decision (59A step 6)");
                }
            }
        }

        [Fact]
        public void EveryDeclaredEnforcementRef_IsARealCriticalCiGate()
        {
            var (register, manifest) = Load();
            Assert.False(register.HasErrors, string.Join("; ", register.Errors));
            Assert.False(manifest.HasErrors, string.Join("; ", manifest.Errors));

            var byId = manifest.Gates.ToDictionary(g => g.gate_id, StringComparer.OrdinalIgnoreCase);
            var files = new Ashfall.Core.FileSystemIO();
            var data = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                files.ReadAllText(Path.Combine(DataDir(), StandingGateCatalogLoader.FileName)),
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower
                })!;

            int gatedCount = 0;
            foreach (var row in data.standing_gates!)
            {
                string reference = (row.enforcement_ref ?? string.Empty).Trim();
                if (string.Equals(reference, StandingGateCatalogLoader.NoEnforcementRef,
                        StringComparison.OrdinalIgnoreCase))
                    continue;

                gatedCount++;
                Assert.True(byId.ContainsKey(reference),
                    $"standing gate '{row.gate_id}' names CI gate '{reference}', which is not "
                    + $"declared in {CiGateManifestReader.ManifestFileName}.");
                Assert.True(byId[reference].critical,
                    $"CI gate '{reference}' is not critical, so a red would not block a release — "
                    + $"it cannot enforce standing gate '{row.gate_id}'.");
            }

            Assert.True(gatedCount > 0, "the register must bind at least one real CI gate");
        }

        [Fact]
        public void PerPushStandingRules_AreEnforcedByAFastTierGate()
        {
            var (register, manifest) = Load();
            Assert.False(register.HasErrors, string.Join("; ", register.Errors));

            var byId = manifest.Gates.ToDictionary(g => g.gate_id, StringComparer.OrdinalIgnoreCase);
            var files = new Ashfall.Core.FileSystemIO();
            var data = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                files.ReadAllText(Path.Combine(DataDir(), StandingGateCatalogLoader.FileName)),
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower
                })!;

            foreach (var row in data.standing_gates!)
            {
                if (!string.Equals(row.tier, "per_push", StringComparison.OrdinalIgnoreCase)) continue;
                string reference = (row.enforcement_ref ?? string.Empty).Trim();
                if (string.Equals(reference, StandingGateCatalogLoader.NoEnforcementRef,
                        StringComparison.OrdinalIgnoreCase))
                    continue;
                Assert.True(byId.ContainsKey(reference), $"unknown CI gate '{reference}'");
                Assert.True(
                    string.Equals(byId[reference].classification, "fast", StringComparison.OrdinalIgnoreCase),
                    $"per-push standing gate '{row.gate_id}' binds '{reference}', which runs on the "
                    + $"'{byId[reference].classification}' tier — a per-push rule needs a fast gate.");
            }
        }

        // ── 3 — the enforcement manifest itself is honest ──────────────────

        [Fact]
        public void EnforcementManifest_DeclaresARunnableGateForEveryRow()
        {
            var (_, manifest) = Load();
            Assert.False(manifest.HasErrors, string.Join("; ", manifest.Errors));
            foreach (var gate in manifest.Gates)
                Assert.False(string.IsNullOrWhiteSpace(gate.command),
                    $"CI gate '{gate.gate_id}' declares no command, so it cannot run (Wave 3 finding).");
        }

        [Fact]
        public void EnforcementManifest_TotalMatchesParsedRows()
        {
            var files = new Ashfall.Core.FileSystemIO();
            var manifest = CiGateManifestReader.Load(files.ReadAllText(EnforcementManifestPath()));
            Assert.False(manifest.HasErrors, string.Join("; ", manifest.Errors));
            Assert.True(manifest.Gates.Count > 0);
        }

        // ── 4 — the register's own gates can fail (59A step 3) ──────────────

        [Fact]
        public void RegisterLoader_RejectsAnOwnerlessGate()
        {
            string json = BuildVariant(row => row.owner_role = string.Empty);
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors,
                "an unowned gate must not load — it is the reason three gates were red while claims said green");
        }

        [Fact]
        public void RegisterLoader_RejectsAnEmptyRegister()
        {
            string json = BuildVariant(_ => { }, clearRows: true);
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors);
        }

        [Fact]
        public void RegisterLoader_RejectsAnUnknownTier()
        {
            string json = BuildVariant(row => row.tier = "whenever_feels_right");
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors);
        }

        [Fact]
        public void RegisterLoader_RejectsAnUnwrittenNonGateDecision()
        {
            string json = BuildVariant(row =>
            {
                row.enforcement_ref = StandingGateCatalogLoader.NoEnforcementRef;
                row.non_gate_rule = string.Empty;
            });
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors,
                "a decision not to gate must be written (59A step 6)");
        }

        [Fact]
        public void RegisterLoader_RejectsAWaveOriginOfZero()
        {
            string json = BuildVariant(row => row.wave_origin = 0);
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors);
        }

        [Fact]
        public void RegisterLoader_RejectsADuplicateGateId()
        {
            string json = BuildVariant(row => { }, duplicateFirst: true);
            Assert.True(StandingGateCatalogLoader.LoadFromJson(json).HasErrors);
        }

        // ── 5 — the Core registry evaluates the register truthfully ────────

        [Fact]
        public void Registry_AuditReportsEveryEnforcedGate()
        {
            var (register, _) = Load();
            Assert.False(register.HasErrors, string.Join("; ", register.Errors));

            var registry = new StandingGateRegistry(register.Gates);
            var report = registry.AuditStandingGates(_ => true);

            Assert.Equal(registry.GetAllGates().Count, report.EvaluatedGates);
            Assert.True(report.PassingGates == report.EvaluatedGates);
            Assert.True(report.AllGatesPassing);
            Assert.Empty(report.Violations);
        }

        [Fact]
        public void Registry_AuditNamesEveryViolation_WithoutStoppingAtTheFirst()
        {
            var gates = new[]
            {
                new StandingGateDef { GateId = "gate_a", WaveOrigin = 1, OwnerRole = "CoreIntegrator" },
                new StandingGateDef { GateId = "gate_b", WaveOrigin = 2, OwnerRole = "CoreIntegrator" },
                new StandingGateDef { GateId = "gate_c", WaveOrigin = 3, OwnerRole = "CoreIntegrator" }
            };
            var registry = new StandingGateRegistry(gates);
            var report = registry.AuditStandingGates(id => id == "gate_b");

            Assert.Equal(3, report.EvaluatedGates);
            Assert.Equal(1, report.PassingGates);
            Assert.False(report.AllGatesPassing);
            Assert.Equal(2, report.Violations.Count);
            Assert.Contains(report.Violations, v => v.GateId == "gate_a" && v.WaveOrigin == 1);
            Assert.Contains(report.Violations, v => v.GateId == "gate_c" && v.WaveOrigin == 3);
        }

        [Fact]
        public void Registry_AuditHonoursAnUnenforcedGate()
        {
            var gates = new[]
            {
                new StandingGateDef { GateId = "gate_off", WaveOrigin = 1, OwnerRole = "UiLead", IsEnforced = false }
            };
            var registry = new StandingGateRegistry(gates);
            var report = registry.AuditStandingGates(_ => false);

            Assert.Equal(0, report.EvaluatedGates);
            Assert.Equal(1, report.TotalGates);
            Assert.Equal(0, report.EnforcedGates);
        }

        [Fact]
        public void Registry_FiltersByTierAndWave()
        {
            var (register, _) = Load();
            var registry = new StandingGateRegistry(register.Gates);

            foreach (GateTier tier in new[] { GateTier.PerPush, GateTier.Nightly, GateTier.PerRelease })
            {
                var byTier = registry.GetGatesByTier(tier);
                Assert.All(byTier, g => Assert.Equal(tier, g.Tier));
            }

            var waves = new HashSet<int>(register.Gates.Select(g => g.WaveOrigin));
            foreach (int wave in waves)
                Assert.All(registry.GetGatesByWave(wave), g => Assert.Equal(wave, g.WaveOrigin));
        }

        // ── 6 — the probe is registered ────────────────────────────────────

        [Fact]
        public void StandingGatesProbe_IsRegisteredInTheHostCliRegistry()
        {
            var action = Ashfall.Core.HostCliRegistry.AllDescriptors
                .FirstOrDefault(a => a.Action == Ashfall.Core.HostCliAction.StandingGatesSelfTest);

            Assert.NotNull(action);
            Assert.Contains("--standing-gates-selftest", action!.AllFlags);
            Assert.Contains("Plan 59", action.Description, StringComparison.Ordinal);
        }

        // ── helpers ───────────────────────────────────────────────────────

        private static string BuildVariant(
            Action<StandingGateData> mutate, bool clearRows = false, bool duplicateFirst = false)
        {
            var files = new Ashfall.Core.FileSystemIO();
            var doc = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                files.ReadAllText(Path.Combine(DataDir(), StandingGateCatalogLoader.FileName)),
                new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower,
                    ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
                    AllowTrailingCommas = true
                })!;

            Assert.NotNull(doc.standing_gates);
            if (clearRows)
            {
                doc.standing_gates!.Clear();
            }
            else
            {
                foreach (var row in doc.standing_gates!) mutate(row);
                if (duplicateFirst && doc.standing_gates!.Count > 0)
                    doc.standing_gates!.Add(doc.standing_gates[0]);
            }

            return System.Text.Json.JsonSerializer.Serialize(doc);
        }
    }
}
