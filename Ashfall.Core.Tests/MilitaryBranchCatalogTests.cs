using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate tests for the Military branch data authority
    /// (military_faction_branch.json) and the new branch_/ending_ id
    /// prefixes registered in CatalogIntegrityValidator.
    /// </summary>
    public class MilitaryBranchCatalogTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            return null;
        }

        [Fact]
        public void MilitaryBranchIds_AllBranchesAndEndingsExistInShippedCatalog()
        {
            var catalog = MilitaryBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(MilitaryBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in MilitaryBranchIds.AllBranches)
            {
                Assert.True(catalog.Contains(branchId), $"missing branch '{branchId}' in military_faction_branch.json");
                var entry = catalog.GetById(branchId)!;
                Assert.Equal(MilitaryBranchIds.PonrFlagFor(branchId), entry.ponr_flag);
            }
        }

        [Fact]
        public void ShippedMilitaryBranchDataCrossReferencesCleanly()
        {
            string dir = DataDir();
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");

            var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
            Assert.True(report.Clean,
                "catalog integrity violations (" + report.ErrorCount + "):\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void ValidatorAcceptsARegisteredBranchAndEndingId()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "branch_probe.json"),
                    "[{\"id\":\"branch_probe_alpha\",\"branchId\":\"branch_probe_alpha\"}," +
                    "{\"id\":\"ending_probe_alpha\"}]");
            });

            Assert.True(report.Clean,
                "a self-registered branch_/ending_ id must not be flagged as dangling:\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void ValidatorFlagsAnUnknownBranchReference()
        {
            var report = ValidateScratch(scratch =>
            {
                File.WriteAllText(Path.Combine(scratch, "branch_probe.json"),
                    "[{\"id\":\"branch_probe_alpha\",\"branchId\":\"branch_probe_never_defined\"}]");
            });

            Assert.False(report.Clean, "an unregistered branch_ reference must be caught");
            Assert.Contains(report.Errors, line => line.Contains("branch_probe_never_defined"));
        }

        private static CatalogIntegrityReport ValidateScratch(Action<string> seed)
        {
            string scratch = Path.Combine(Path.GetTempPath(), "ashfall_military_branch_" + Guid.NewGuid().ToString("N"));
            try
            {
                Directory.CreateDirectory(scratch);
                seed(scratch);
                return CatalogIntegrityValidator.Validate(scratch, new FileSystemIO());
            }
            finally
            {
                if (Directory.Exists(scratch))
                    Directory.Delete(scratch, true);
            }
        }
    }
}
