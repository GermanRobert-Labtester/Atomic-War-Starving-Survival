// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Trapping Integration — Task 3:
    /// Compatibility Matrix Completeness, Domain Specializations, and Preferred Trap Type Satisfiability.
    /// </summary>
    public sealed class WildlifeTrapCompatibilityTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var traps = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(traps);
            return traps!;
        }

        // ====================================================================
        // WORKSTREAM A: Prey & Trap reachability completeness
        // ====================================================================

        [Fact]
        public void EveryPrey_HasAtLeastOneCompatibleTrap()
        {
            var catalog = LoadCatalog();
            Assert.NotEmpty(catalog.Prey);

            var preyCompatibleTraps = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var preyId in catalog.Prey.Keys)
                preyCompatibleTraps[preyId] = new List<string>();

            foreach (var trap in catalog.Traps.Values)
            {
                foreach (var p in trap.compatiblePrey)
                {
                    if (preyCompatibleTraps.TryGetValue(p, out var list))
                        list.Add(trap.trap_id);
                }
            }

            foreach (var kvp in preyCompatibleTraps)
            {
                Assert.True(kvp.Value.Count >= 1,
                    $"Prey '{kvp.Key}' has 0 compatible traps in the catalog");
            }
        }

        [Fact]
        public void EveryTrap_HasAtLeastOneCompatiblePreySpecies()
        {
            var catalog = LoadCatalog();
            Assert.NotEmpty(catalog.Traps);

            foreach (var trap in catalog.Traps.Values)
            {
                Assert.True(trap.compatiblePrey != null && trap.compatiblePrey.Count >= 1,
                    $"Trap '{trap.trap_id}' must have at least one compatible prey species");
            }
        }

        // ====================================================================
        // WORKSTREAM B: Preferred trap type satisfiability
        // ====================================================================

        [Fact]
        public void EveryPrey_PreferredTrapType_IsSatisfiedByAtLeastOneCompatibleTrap()
        {
            var catalog = LoadCatalog();

            // Build map of speciesId -> list of compatible TrapDefinitions
            var compatibility = new Dictionary<string, List<TrapDefinition>>(StringComparer.Ordinal);
            foreach (var preyId in catalog.Prey.Keys)
                compatibility[preyId] = new List<TrapDefinition>();

            foreach (var trap in catalog.Traps.Values)
            {
                foreach (var p in trap.compatiblePrey)
                {
                    if (compatibility.TryGetValue(p, out var list))
                        list.Add(trap);
                }
            }

            foreach (var prey in catalog.Prey.Values)
            {
                Assert.False(string.IsNullOrEmpty(prey.preferredTrapType),
                    $"Prey '{prey.speciesId}' must declare a preferredTrapType");

                var compatibleTraps = compatibility[prey.speciesId];
                bool satisfied = compatibleTraps.Any(t => string.Equals(t.trapType, prey.preferredTrapType, StringComparison.Ordinal));
                Assert.True(satisfied,
                    $"Prey '{prey.speciesId}' has preferredTrapType '{prey.preferredTrapType}' but none of its compatible traps [{string.Join(", ", compatibleTraps.Select(t => t.trap_id + ":" + t.trapType))}] match it");
            }
        }

        // ====================================================================
        // WORKSTREAM C: Specialized domain rules & Generalist breadth
        // ====================================================================

        [Fact]
        public void AquaticPrey_CompatibilityTable_UsesWaterTraps()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (string speciesId in new[] { "mirror_carp", "ash_pike" })
            {
                if (!catalog.Prey.ContainsKey(speciesId))
                {
                    failures.Add($"Aquatic prey '{speciesId}' is missing from the catalog");
                    continue;
                }

                foreach (var trap in catalog.Traps.Values)
                {
                    if (trap.compatiblePrey.Contains(speciesId) && !trap.requiresWater)
                    {
                        failures.Add(
                            $"Aquatic prey '{speciesId}' cannot be caught by non-water trap '{trap.trap_id}'");
                    }
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void HeavyGamePrey_CompatibilityTable_UsesPitTraps()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (string speciesId in new[] { "deer", "boar" })
            {
                if (!catalog.Prey.ContainsKey(speciesId))
                {
                    failures.Add($"Heavy game prey '{speciesId}' is missing from the catalog");
                    continue;
                }

                foreach (var trap in catalog.Traps.Values)
                {
                    if (trap.compatiblePrey.Contains(speciesId)
                        && !string.Equals(trap.trapType, "pit", StringComparison.Ordinal))
                    {
                        failures.Add(
                            $"Heavy game prey '{speciesId}' cannot be caught by non-pit trap '{trap.trap_id}'");
                    }
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void AvianPrey_CompatibilityTable_UsesAvianTraps()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (string speciesId in new[] { "pheasant", "ash_crow", "contaminated_fowl" })
            {
                if (!catalog.Prey.ContainsKey(speciesId))
                {
                    failures.Add($"Avian prey '{speciesId}' is missing from the catalog");
                    continue;
                }

                foreach (var trap in catalog.Traps.Values)
                {
                    if (trap.compatiblePrey.Contains(speciesId)
                        && trap.trapType != "net"
                        && trap.trapType != "bird_snare")
                    {
                        failures.Add(
                            $"Avian prey '{speciesId}' cannot be caught by non-avian trap '{trap.trap_id}' (type '{trap.trapType}')");
                    }
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void NoUniversalTrap_ExistsInCatalog()
        {
            var catalog = LoadCatalog();
            int totalPreyCount = catalog.Prey.Count;

            foreach (var trap in catalog.Traps.Values)
            {
                Assert.True(trap.compatiblePrey.Count < totalPreyCount,
                    $"Trap '{trap.trap_id}' is a universal trap ({trap.compatiblePrey.Count}/{totalPreyCount} prey), violating specialization");
            }
        }

        [Fact]
        public void GeneralistPrey_BreadthTable_MeetsMinimums()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (var expectation in new[]
            {
                (speciesId: "rabbit", minTraps: 4),
                (speciesId: "rat", minTraps: 4),
            })
            {
                int compatibleCount = catalog.Traps.Values.Count(t => t.compatiblePrey.Contains(expectation.speciesId));
                if (compatibleCount < expectation.minTraps)
                {
                    failures.Add(
                        $"Generalist prey '{expectation.speciesId}' must be compatible with at least {expectation.minTraps} traps, got {compatibleCount}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        // ====================================================================
        // WORKSTREAM D: Validator enforcement on compatibility anomalies
        // ====================================================================

        [Fact]
        public void Validator_RejectsTrapWithNoCompatiblePrey()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trap_no_prey_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_empty\",\"trapType\":\"box\",\"compatiblePrey\":[]}],\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("trap 'trap_empty' has no compatiblePrey"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsPreyWithNoCompatibleTrap()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "prey_no_trap_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}," +
                    "{\"speciesId\":\"stranded_prey\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("prey 'stranded_prey' has no compatible traps"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsPreyWithUnsatisfiedPreferredTrapType()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "prey_bad_pref_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"unsupported_type\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("preferredTrapType 'unsupported_type' is not satisfied"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsUniversalTrap()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "trap_universal_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_god\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\",\"rat\"]}]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}," +
                    "{\"speciesId\":\"rat\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("violates specialization; compatible with all prey species"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsAquaticPreyOnNonWaterTrap()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "prey_bad_water_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_land\",\"trapType\":\"box\",\"requiresWater\":false,\"compatiblePrey\":[\"mirror_carp\"]}," +
                    "{\"trap_id\":\"trap_water\",\"trapType\":\"fish_trap\",\"requiresWater\":true,\"compatiblePrey\":[\"other_prey\"]}" +
                    "]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"mirror_carp\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}," +
                    "{\"speciesId\":\"other_prey\",\"preferredTrapType\":\"fish_trap\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("aquatic prey 'mirror_carp' is assigned to non-water trap"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsHeavyPreyOnNonPitTrap()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "prey_bad_heavy_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"deer\"]}," +
                    "{\"trap_id\":\"trap_pit\",\"trapType\":\"pit\",\"compatiblePrey\":[\"other_prey\"]}" +
                    "]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"deer\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}," +
                    "{\"speciesId\":\"other_prey\",\"preferredTrapType\":\"pit\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("heavy prey 'deer' is assigned to non-pit trap"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsAvianPreyOnNonAvianTrap()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "prey_bad_avian_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[" +
                    "{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"ash_crow\"]}," +
                    "{\"trap_id\":\"trap_net\",\"trapType\":\"net\",\"compatiblePrey\":[\"other_prey\"]}" +
                    "]," +
                    "\"prey\":[" +
                    "{\"speciesId\":\"ash_crow\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"b\"]}," +
                    "{\"speciesId\":\"other_prey\",\"preferredTrapType\":\"net\",\"attractedByBaitIds\":[\"b\"]}" +
                    "],\"baits\":[{\"baitId\":\"b\",\"catchBonusMultiplier\":1,\"toxicReduction\":0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("avian prey 'ash_crow' is assigned to non-avian trap"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }
    }
}
