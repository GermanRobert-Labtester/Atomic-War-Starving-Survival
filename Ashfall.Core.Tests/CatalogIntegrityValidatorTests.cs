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
        public void ValidatorReportsMalformedJsonWithFileAndExceptionContext()
        {
            var report = ValidateScratch((scratch) =>
            {
                File.WriteAllText(Path.Combine(scratch, "broken_catalog.json"),
                    "{\"schema_version\":1,\"items\":[}");
            });

            Assert.False(report.Clean,
                "malformed authored JSON must not disappear as an empty catalog");
            Assert.Contains(report.Errors, line =>
                line.Contains("broken_catalog.json")
                && line.Contains("malformed JSON")
                && line.Contains("JsonReaderException"));
        }

        [Fact]
        public void ValidatorFlagsAnUnknownPrefixedReference()
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

        /// <summary>
        /// An id authored in one catalog and referenced in another (enrichment
        /// foreign keys, shared stage templates) is legitimate REUSE, not a
        /// conflict. This gates the majority of the former "duplicate id"
        /// noise: cross-file reuse must never surface as an error.
        /// </summary>
        [Fact]
        public void CrossFileReuseOfAnAuthoredIdIsNotReported()
        {
            var report = ValidateScratch((scratch) =>
            {
                File.WriteAllText(Path.Combine(scratch, "items.json"),
                    "[{\"id\":\"morphine\",\"category\":\"drug\"}]");
                // enrichment table: strong by a polymorphic definition-key that
                // actually references the already-authored canonical item.
                File.WriteAllText(Path.Combine(scratch, "expansion_tags.json"),
                    "[{\"item_id\":\"morphine\",\"tags\":[\"addictive_opioid\"]}]");
            });

            Assert.True(report.Clean,
                "cross-file reuse of an authored id must not error:\n"
                + string.Join("\n", report.Errors));
            Assert.Equal(1, report.AuthoredIds);
            Assert.True(report.ReuseCount >= 1);
        }

        /// <summary>
        /// Two rows of the SAME catalog claiming the same entity-root id is a
        /// genuine Invariant-6 violation and must still be caught as an error.
        /// </summary>
        [Fact]
        public void SameFileEntityRootDuplicateIdIsAnError()
        {
            var report = ValidateScratch((scratch) =>
            {
                File.WriteAllText(Path.Combine(scratch, "quests.json"),
                    "[{\"id\":\"q_alpha\"},{\"id\":\"q_alpha\"}]");
            });

            Assert.False(report.Clean,
                "a genuine same-file entity-root duplicate id must be caught");
            Assert.Contains(report.Errors, line => line.Contains("duplicate id"));
        }

        /// <summary>
        /// Nested template ids (shared stage/choice steps) reused across rows in
        /// one file are legitimate composition, not conflicting registrations.
        /// </summary>
        [Fact]
        public void NestedSharedTemplateIdsAreNotConflicts()
        {
            var report = ValidateScratch((scratch) =>
            {
                File.WriteAllText(Path.Combine(scratch, "quests.json"),
                  "[{\"id\":\"q1\",\"stages\":[{\"id\":\"stage_choose\",\"text\":\"t\"}]},"
                + " {\"id\":\"q2\",\"stages\":[{\"id\":\"stage_choose\",\"text\":\"u\"}]}]");
            });

            Assert.True(report.Clean,
                "shared stage template ids must not be flagged:\n"
                + string.Join("\n", report.Errors));
        }

        private static CatalogIntegrityReport ValidateScratch(Action<string> seed)
        {
            string scratch = Path.Combine(Path.GetTempPath(), "ashfall_integrity_" + Guid.NewGuid().ToString("N"));
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
