using System;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Gate tests for CatalogIntegrityValidator: the 56-catalog cross-reference
    /// validator must report zero errors against the shipped StreamingAssets data.
    /// A failure here means someone introduced a dangling id (typo, renamed
    /// entity, or a reference to an id the master list never defined).
    /// </summary>
    public class CatalogIntegrityValidatorTests
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
        public void AllCatalogIdsCrossReferenceCleanly()
        {
            string dir = DataDir();
            Assert.False(string.IsNullOrEmpty(dir), "StreamingAssets/Data must be findable from the test run");
            Assert.True(Directory.Exists(dir), "data directory must exist: " + dir);

            var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
            Assert.True(report.Clean,
                "catalog integrity violations (" + report.ErrorCount + "):\n"
                + string.Join("\n", report.Errors));
        }

        [Fact]
        public void ValidatorFlagsAnUnknownPrefixedId()
        {
            // Fault injection: a fabricated id must be reported, proving the
            // gate actually detects dangling references (it is not vacuous).
            var probe = ProbeReport("item_this_id_never_exists_zzz");
            Assert.Contains(probe.Errors, line => line.Contains("item_this_id_never_exists_zzz"));
        }

        [Fact]
        public void ValidatorFlagsAnUnknownBareReference()
        {
            // Same for a bare (non-prefixed) id in a reference-key position.
            var probe = ProbeReport("never_defined_thing_xyzzy");
            Assert.Contains(probe.Errors, line => line.Contains("never_defined_thing_xyzzy"));
        }

        private static CatalogIntegrityReport ProbeReport(string probe)
        {
            string dir = DataDir();
            string path = Path.Combine(dir, "probe_integrity_tmp.json");
            try
            {
                File.WriteAllText(path,
                    "{\"entries\":[{\"id\":\"probe_entry\",\"resultItemId\":\"" + probe
                    + "\",\"choices\":[{\"choiceId\":\"c1\",\"requiredItemId\":\"" + probe + "\"}]}]}");
                var report = CatalogIntegrityValidator.Validate(dir, new FileSystemIO());
                return report;
            }
            finally
            {
                if (File.Exists(path)) File.Delete(path);
            }
        }
    }
}
