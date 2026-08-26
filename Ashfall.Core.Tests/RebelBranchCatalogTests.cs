using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate tests for the Rebel branch data authority
    /// (rebel_faction_branch.json) against the shared branch_/ending_ id
    /// prefixes registered in CatalogIntegrityValidator.
    /// </summary>
    public class RebelBranchCatalogTests
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
        public void RebelBranchIds_AllBranchesAndEndingsExistInShippedCatalog()
        {
            var catalog = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(RebelBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in RebelBranchIds.AllBranches)
            {
                Assert.True(catalog.Contains(branchId), $"missing branch '{branchId}' in rebel_faction_branch.json");
                var entry = catalog.GetById(branchId)!;
                Assert.Equal(RebelBranchIds.PonrFlagFor(branchId), entry.ponr_flag);
            }
        }

        [Fact]
        public void ShippedRebelBranchDataCrossReferencesCleanly()
        {
            string dir = DataDir();
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");

            var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
            Assert.True(report.Clean,
                "catalog integrity violations (" + report.ErrorCount + "):\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void MilitaryAndRebelBranchIdsDoNotCollide()
        {
            // The two catalogs must never share a branch, ending, or PoNR flag
            // id — a collision would mean one faction's branch resolves
            // against the wrong data at runtime.
            var military = MilitaryBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var rebel = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (var branchId in MilitaryBranchIds.AllBranches)
                Assert.False(rebel.Contains(branchId), $"Rebel catalog must not contain Military branch id '{branchId}'");

            foreach (var branchId in RebelBranchIds.AllBranches)
                Assert.False(military.Contains(branchId), $"Military catalog must not contain Rebel branch id '{branchId}'");
        }
    }
}
