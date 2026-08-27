// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Persistence Boundary & ID Normalization Determinism Tests.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.Flags;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Save
{
    public class PersistenceBoundaryDeterminismTests
    {
        private static string RepoRoot()
        {
            string[] candidates =
            {
                Directory.GetCurrentDirectory(),
                AppContext.BaseDirectory
            };
            foreach (string start in candidates)
            {
                var dir = new DirectoryInfo(Path.GetFullPath(start));
                while (dir != null)
                {
                    string probeProps = Path.Combine(dir.FullName, "Directory.Packages.props");
                    if (File.Exists(probeProps))
                        return dir.FullName;
                    dir = dir.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate repository root from test execution context.");
        }

        [Fact]
        public void InMemoryFlagLedger_NormalizesIdsDeterministically()
        {
            var ledger = new InMemoryFlagLedger();

            // Set with uppercase / mixed case
            ledger.Set("FLAG_SHELTER_BREACH");
            Assert.True(ledger.IsSet("flag_shelter_breach"));
            Assert.True(ledger.IsSet("FLAG_SHELTER_BREACH"));
            Assert.True(ledger.IsSet("Flag_Shelter_Breach "));

            // Counter operations
            ledger.Increment("COUNTER_RADIATION_DOSE", 5);
            Assert.Equal(5, ledger.GetCounter("counter_radiation_dose"));
            Assert.Equal(5, ledger.GetCounter("COUNTER_RADIATION_DOSE"));

            ledger.Clear("flag_shelter_breach");
            Assert.False(ledger.IsSet("FLAG_SHELTER_BREACH"));
        }

        [Fact]
        public void FactionRadioEngine_NormalizesFactionIdsDeterministically()
        {
            var engine = new FactionRadioEngine();
            var channel = new FactionRadioChannel
            {
                FactionId = "FACTION_ENGINEERS",
                Callsign = "WHEEL & SPINDLE",
                FrequencyMhz = 104.5f
            };

            engine.RegisterChannel(channel);

            // Channel registered under normalized key
            Assert.Equal(104.5f, engine.GetFactionFrequency("faction_engineers"));
            Assert.Equal(104.5f, engine.GetFactionFrequency("FACTION_ENGINEERS"));
            Assert.Equal("WHEEL & SPINDLE", engine.GetFactionCallsign("faction_engineers"));
            Assert.Contains("faction_engineers", engine.GetAllFactions());
        }

        [Fact]
        public void SaveChecksum_IsCaseSensitive_ProvingNeedForIdNormalization()
        {
            var stateLower = new TestSaveDto { itemId = "item_fuel_cell", quantity = 10 };
            var stateUpper = new TestSaveDto { itemId = "ITEM_FUEL_CELL", quantity = 10 };

            string hashLower = SaveChecksum.Compute(stateLower);
            string hashUpper = SaveChecksum.Compute(stateUpper);

            // Checksum hashes must be distinct when IDs are not normalized
            Assert.False(string.IsNullOrEmpty(hashLower));
            Assert.False(string.IsNullOrEmpty(hashUpper));
            Assert.NotEqual(hashLower, hashUpper);
        }

        [Fact]
        public void SaveStoreImplementations_ContainZeroOrdinalIgnoreCaseComparers()
        {
            string root = RepoRoot();
            string hostDir = Path.Combine(root, "src", "Host");

            var forbiddenPattern = new Regex(@"StringComparer\.OrdinalIgnoreCase", RegexOptions.Compiled);
            var saveStoreFiles = Directory.EnumerateFiles(hostDir, "*SaveStore*.cs", SearchOption.AllDirectories)
                .Where(f => !f.EndsWith("SelfTest.cs") && !f.EndsWith("Tests.cs"))
                .ToList();

            var violations = new List<string>();

            foreach (var file in saveStoreFiles)
            {
                string text = File.ReadAllText(file);
                if (forbiddenPattern.IsMatch(text))
                {
                    string relPath = Path.GetRelativePath(root, file).Replace('\\', '/');
                    violations.Add($"{relPath} contains StringComparer.OrdinalIgnoreCase (must use StringComparer.Ordinal or Core normalizers).");
                }
            }

            Assert.True(violations.Count == 0,
                $"Discovered {violations.Count} persistence stores using OrdinalIgnoreCase:\n  " +
                string.Join("\n  ", violations));
        }

        [Serializable]
        private class TestSaveDto
        {
            public string itemId = string.Empty;
            public int quantity;
        }
    }
}
