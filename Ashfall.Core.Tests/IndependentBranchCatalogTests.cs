using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate tests for the Independent branch data authority
    /// (independent_faction_branch.json) against the shared branch_/ending_
    /// id prefixes registered in CatalogIntegrityValidator.
    /// </summary>
    public class IndependentBranchCatalogTests
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
        public void IndependentBranchIds_AllBranchesAndEndingsExistInShippedCatalog()
        {
            var catalog = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.Equal(IndependentBranchIds.BranchCount, catalog.Count);
            foreach (var branchId in IndependentBranchIds.AllBranches)
            {
                Assert.True(catalog.Contains(branchId), $"missing branch '{branchId}' in independent_faction_branch.json");
                var entry = catalog.GetById(branchId)!;
                Assert.Equal(IndependentBranchIds.PonrFlagFor(branchId), entry.ponr_flag);
            }
        }

        [Fact]
        public void ShippedIndependentBranchDataCrossReferencesCleanly()
        {
            string dir = DataDir();
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");

            var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
            Assert.True(report.Clean,
                "catalog integrity violations (" + report.ErrorCount + "):\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void NoBranchIdsCollideAcrossAllThreeFactionCatalogs()
        {
            var military = MilitaryBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var rebel = RebelBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            var independent = IndependentBranchCatalog.LoadAndRegister(DataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (var branchId in IndependentBranchIds.AllBranches)
            {
                Assert.False(military.Contains(branchId), $"Military catalog must not contain Independent branch id '{branchId}'");
                Assert.False(rebel.Contains(branchId), $"Rebel catalog must not contain Independent branch id '{branchId}'");
            }
        }
    }
}
