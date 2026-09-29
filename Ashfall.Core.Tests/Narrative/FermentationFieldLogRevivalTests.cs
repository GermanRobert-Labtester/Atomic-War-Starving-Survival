// SPDX-License-Identifier: MIT
// ============================================================================
// W2-06 · Decision Point 1 · Path B — "revive with existing consumers".
//
// The authored fermentation field log (four assay/report catalogs under
// Assets/StreamingAssets/Data/narrative/) had no runtime consumer. The
// host-side loader (src/Host/FermentationFieldLogCatalogLoader.cs) is a thin
// iteration over the Core port — IFileIO + IJsonSerializer +
// CatalogLocator.LoadWrappedList — exactly as CommodityBaselineCatalogLoader
// and DwellerMedicalCatalog already do. The Ashfall.Core.Tests project does
// not reference src/, so this test proves that port contract, which is the
// part that silently breaks in an exported build (raw File.ReadAllText
// returns nothing for res:// Data).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Locks the authored fermentation field log: present on disk, parseable
    /// through the port, and every entry carries prose (no silent empty
    /// revival). Proven per entry type, because a wrapped-array failure in
    /// just one file would otherwise pass as a partial success.
    /// </summary>
    public sealed class FermentationFieldLogRevivalTests
    {
        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static readonly IFileIO Io = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();

        private static string ReadNarrative(string fileName)
        {
            string path = Io.Combine(GetDataDir(), "narrative", fileName);
            Assert.True(Io.FileExists(path), $"missing authored corpus: {path}");
            return Io.ReadAllText(path);
        }

        public static IEnumerable<object[]> AuthoredFiles()
        {
            yield return new object[] { "sourdough_mother_acidity_logs.json" };
            yield return new object[] { "brewers_yeast_krausen_audits.json" };
            yield return new object[] { "silage_lactic_pit_reports.json" };
            yield return new object[] { "fermentation_crock_airlock_assays.json" };
        }

        [Theory]
        [MemberData(nameof(AuthoredFiles))]
        public void FermentationCorpus_ReadsThroughPort_WithNonEmptyProse(string fileName)
        {
            string json = ReadNarrative(fileName);

            // Same wrapped-or-bare parsing the host loader performs.
            List<string> ids = new List<string>();
            List<string> prose = new List<string>();

            if (fileName.StartsWith("sourdough", StringComparison.Ordinal))
            {
                foreach (var e in CatalogLocator.LoadWrappedList<SourdoughMotherAcidityEntry>(json, Options))
                { ids.Add(e.Id); prose.Add(e.Prose); }
            }
            else if (fileName.StartsWith("brewers", StringComparison.Ordinal))
            {
                foreach (var e in CatalogLocator.LoadWrappedList<BrewersYeastKrausenEntry>(json, Options))
                { ids.Add(e.Id); prose.Add(e.Prose); }
            }
            else if (fileName.StartsWith("silage", StringComparison.Ordinal))
            {
                foreach (var e in CatalogLocator.LoadWrappedList<SilageLacticPitEntry>(json, Options))
                { ids.Add(e.Id); prose.Add(e.Prose); }
            }
            else
            {
                foreach (var e in CatalogLocator.LoadWrappedList<FermentationCrockAirlockEntry>(json, Options))
                { ids.Add(e.Id); prose.Add(e.Prose); }
            }

            Assert.NotEmpty(ids);
            Assert.All(ids, id => Assert.False(string.IsNullOrWhiteSpace(id)));
            Assert.All(prose, p => Assert.False(string.IsNullOrWhiteSpace(p),
                "a revival that renders an empty line is not reachable"));
        }

        [Fact]
        public void FermentationCorpus_HasNoDuplicateIds_AcrossAllFourFiles()
        {
            var ids = new List<string>();
            foreach (string fileName in new[]
                {
                    "sourdough_mother_acidity_logs.json",
                    "brewers_yeast_krausen_audits.json",
                    "silage_lactic_pit_reports.json",
                    "fermentation_crock_airlock_assays.json",
                })
            {
                string json = ReadNarrative(fileName);
                if (fileName.StartsWith("sourdough", StringComparison.Ordinal))
                    ids.AddRange(CatalogLocator.LoadWrappedList<SourdoughMotherAcidityEntry>(json, Options).Select(e => e.Id));
                else if (fileName.StartsWith("brewers", StringComparison.Ordinal))
                    ids.AddRange(CatalogLocator.LoadWrappedList<BrewersYeastKrausenEntry>(json, Options).Select(e => e.Id));
                else if (fileName.StartsWith("silage", StringComparison.Ordinal))
                    ids.AddRange(CatalogLocator.LoadWrappedList<SilageLacticPitEntry>(json, Options).Select(e => e.Id));
                else
                    ids.AddRange(CatalogLocator.LoadWrappedList<FermentationCrockAirlockEntry>(json, Options).Select(e => e.Id));
            }

            Assert.True(ids.Count >= 28, $"expected at least 28 authored entries, found {ids.Count}");
            Assert.Equal(ids.Count, ids.Distinct(StringComparer.Ordinal).Count());
        }
    }
}
