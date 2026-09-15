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
    /// Flagship Trapping Integration — Task 2:
    /// Bait Reachability, Reverse-Reference Integrity, and Parameter Validity.
    /// </summary>
    public sealed class WildlifeBaitReachabilityTests
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
        // WORKSTREAM A: Every authored bait is reached by >= 1 prey species
        // ====================================================================

        [Fact]
        public void AllAuthoredBaits_AreReachedByAtLeastOnePrey()
        {
            var catalog = LoadCatalog();
            Assert.NotEmpty(catalog.Baits);

            // Construct reverse index: baitId -> set of prey species attracted by it
            var reverseMap = new Dictionary<string, List<string>>(StringComparer.Ordinal);
            foreach (var baitId in catalog.Baits.Keys)
            {
                reverseMap[baitId] = new List<string>();
            }

            foreach (var prey in catalog.Prey.Values)
            {
                if (prey.attractedByBaitIds == null) continue;
                foreach (var bId in prey.attractedByBaitIds)
                {
                    if (reverseMap.TryGetValue(bId, out var preyList))
                    {
                        preyList.Add(prey.speciesId);
                    }
                }
            }

            foreach (var kvp in reverseMap)
            {
                string baitId = kvp.Key;
                var attractedPrey = kvp.Value;
                Assert.True(attractedPrey.Count >= 1,
                    $"Bait '{baitId}' is unreachable: no prey species references it in attractedByBaitIds");
            }
        }

        [Fact]
        public void NamedBait_ReachabilityTable_MapsToExpectedPrey()
        {
            var catalog = LoadCatalog();
            var failures = new List<string>();

            foreach (var testCase in new[]
            {
                (BaitId: "bait_scrap_meat", ExpectedPrey: "rat"),
                (BaitId: "bait_grain_lure", ExpectedPrey: "rabbit"),
                (BaitId: "bait_pheromone", ExpectedPrey: "ash_crow"),
                (BaitId: "bait_fat_cake", ExpectedPrey: "fox"),
                (BaitId: "bait_berry_mash", ExpectedPrey: "deer"),
                (BaitId: "bait_salt_lick", ExpectedPrey: "boar"),
            })
            {
                if (!catalog.Baits.ContainsKey(testCase.BaitId))
                {
                    failures.Add($"bait '{testCase.BaitId}' is missing from the catalog");
                    continue;
                }

                if (!catalog.Prey.TryGetValue(testCase.ExpectedPrey, out var prey))
                {
                    failures.Add($"expected prey '{testCase.ExpectedPrey}' is missing from the catalog");
                    continue;
                }

                if (prey.attractedByBaitIds == null || !prey.attractedByBaitIds.Contains(testCase.BaitId))
                {
                    failures.Add(
                        $"prey '{testCase.ExpectedPrey}' does not reference bait '{testCase.BaitId}'");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        // ====================================================================
        // WORKSTREAM B: Reverse reference integrity (prey -> bait)
        // ====================================================================

        [Fact]
        public void AllPreyBaitReferences_ExistInAuthoredBaits()
        {
            var catalog = LoadCatalog();
            foreach (var prey in catalog.Prey.Values)
            {
                if (prey.attractedByBaitIds == null) continue;
                foreach (var bId in prey.attractedByBaitIds)
                {
                    Assert.True(catalog.Baits.ContainsKey(bId),
                        $"Prey '{prey.speciesId}' references undefined baitId '{bId}'");
                }
            }
        }

        // ====================================================================
        // WORKSTREAM C: Bait parameter validity
        // ====================================================================

        [Fact]
        public void AllAuthoredBaits_HavePositiveMultiplierAndNonNegativeToxicityReduction()
        {
            var catalog = LoadCatalog();
            foreach (var bait in catalog.Baits.Values)
            {
                Assert.True(bait.catchBonusMultiplier > 0f,
                    $"Bait '{bait.baitId}' must have catchBonusMultiplier > 0, got {bait.catchBonusMultiplier}");
                Assert.True(bait.toxicReduction >= 0f,
                    $"Bait '{bait.baitId}' must have toxicReduction >= 0, got {bait.toxicReduction}");
            }
        }

        // ====================================================================
        // WORKSTREAM D: Catalog validator failure enforcement
        // ====================================================================

        [Fact]
        public void Validator_RejectsUnreachableBait()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "bait_unreachable_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_used\"]}]," +
                    "\"baits\":[" +
                    "{\"baitId\":\"bait_used\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}," +
                    "{\"baitId\":\"bait_orphaned\",\"catchBonusMultiplier\":1.5,\"toxicReduction\":0.1}" +
                    "]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("bait 'bait_orphaned' is unreachable"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsPreyWithUndefinedBait()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "bait_undef_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_ghost\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_real\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("references undefined baitId 'bait_ghost'"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsNonPositiveCatchMultiplier()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "bait_bad_mult_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_bad\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_bad\",\"catchBonusMultiplier\":0.0,\"toxicReduction\":0.0}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("catchBonusMultiplier must be positive"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsNegativeToxicReduction()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "bait_bad_tox_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_bad\"]}]," +
                    "\"baits\":[{\"baitId\":\"bait_bad\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":-0.5}]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("toxicReduction must be non-negative"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }

        [Fact]
        public void Validator_RejectsDuplicateBaitId()
        {
            string scratch = Path.Combine(Path.GetTempPath(), "bait_dupe_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                File.WriteAllText(Path.Combine(scratch, "wildlife_trapping_catalog.json"),
                    "{\"schema_version\":1,\"traps\":[{\"trap_id\":\"trap_box\",\"trapType\":\"box\",\"compatiblePrey\":[\"rabbit\"]}]," +
                    "\"prey\":[{\"speciesId\":\"rabbit\",\"preferredTrapType\":\"box\",\"attractedByBaitIds\":[\"bait_dupe\"]}]," +
                    "\"baits\":[" +
                    "{\"baitId\":\"bait_dupe\",\"catchBonusMultiplier\":1.2,\"toxicReduction\":0.0}," +
                    "{\"baitId\":\"bait_dupe\",\"catchBonusMultiplier\":1.5,\"toxicReduction\":0.1}" +
                    "]}");

                var report = new CatalogIntegrityReport();
                CatalogIntegrityValidator.ValidateWildlifeTrappingCatalog(scratch, new FileSystemIO(), report);
                Assert.Contains(report.Errors, e => e.Contains("duplicate baitId 'bait_dupe'"));
            }
            finally
            {
                if (Directory.Exists(scratch)) Directory.Delete(scratch, true);
            }
        }
    }
}
