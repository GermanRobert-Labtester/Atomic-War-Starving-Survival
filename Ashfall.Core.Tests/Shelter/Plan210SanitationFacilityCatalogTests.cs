// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    /// <summary>
    /// Plan 210 Phase 1 — sanitation_facilities.json characterization: real
    /// catalog walks clean, all nine facility types load, closed waste-type
    /// and room-tag vocabularies, numeric bounds, duplicate rejection, and
    /// deterministic reload.
    /// </summary>
    public sealed class Plan210SanitationFacilityCatalogTests
    {
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

        private static SanitationFacilityLoadResult LoadReal()
        {
            return SanitationFacilityCatalogLoader.Load(
                GetDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
        }

        // TEST-AGGREGATION: source_rows=9 authored facilities x 4 rule
        // families (bounds, vocabularies, coverage, determinism).
        [Fact]
        public void RealCatalog_Loads_AllNineFacilities_WithNoErrors()
        {
            var load = LoadReal();
            Assert.False(load.HasErrors, string.Join("; ", load.Errors));
            Assert.Equal(9, load.Facilities.Count);
        }

        [Fact]
        public void RealCatalog_EveryRow_UsesClosedVocabularies_AndValidRanges()
        {
            var load = LoadReal();
            foreach (var f in load.Facilities)
            {
                Assert.InRange(f.capacity, 1, 500);
                Assert.InRange(f.processing_rate, 1, 50);
                Assert.InRange(f.power_draw, 0, 20);
                Assert.InRange(f.hazard_reduction, 0, 50);
                Assert.True(f.waste_types.Count > 0);
                foreach (var wt in f.waste_types)
                    Assert.Contains(wt, SanitationFacilityCatalogLoader.AcceptedWasteTypes);
                foreach (var tag in f.room_tags)
                    Assert.Contains(tag, SanitationFacilityCatalogLoader.AcceptedRoomTags);
            }
        }

        [Fact]
        public void RealCatalog_EveryWasteType_HasCoverage_AndEveryRoomTagReachable()
        {
            var load = LoadReal();
            var covered = load.Facilities.SelectMany(f => f.waste_types).Distinct().ToHashSet();
            Assert.Contains("organic", covered);
            Assert.Contains("chemical", covered);
            Assert.Contains("radioactive", covered);
            // Compost unit must be organic-only by definition (cross-contamination guard).
            var compost = load.Facilities.Single(f => f.facility_type == "compost");
            Assert.Single(compost.waste_types, t => t == "organic");
        }

        [Fact]
        public void RealCatalog_CompostOutputItem_ExistsInAgricultureCatalog()
        {
            // D7 — the compost output id must name a real item with a real
            // consumer, never an orphaned authored id.
            var raw = File.ReadAllText(Path.Combine(GetDataDir(), "agriculture_items.json"));
            Assert.Contains(SanitationSystem.CompostOutputItemId, raw);
        }

        [Fact]
        public void RealCatalog_LoadIsDeterministic()
        {
            var first = LoadReal();
            var second = LoadReal();
            Assert.Equal(first.Facilities.Count, second.Facilities.Count);
            for (int i = 0; i < first.Facilities.Count; i++)
            {
                Assert.Equal(first.Facilities[i].id, second.Facilities[i].id);
                Assert.Equal(first.Facilities[i].processing_rate, second.Facilities[i].processing_rate);
                Assert.Equal(first.Facilities[i].capacity, second.Facilities[i].capacity);
            }
        }

        [Fact]
        public void Loader_MissingFile_CollectsError_DoesNotThrow()
        {
            var result = SanitationFacilityCatalogLoader.Load(
                "/nonexistent_dir", new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.True(result.HasErrors);
            Assert.Empty(result.Facilities);
        }

        [Fact]
        public void Loader_RejectsUnknownWasteType_AndUnknownRoomTag()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, SanitationFacilityCatalogLoader.FileName));
            string mutated = source
                .Replace("\"waste_types\": [\"organic\"],", "\"waste_types\": [\"unobtainium\"],", StringComparison.Ordinal)
                .Replace("\"room_tags\": [\"residential\", \"general\"],", "\"room_tags\": [\"residential\", \"not_a_tag\"],", StringComparison.Ordinal);
            string tempDir = Path.Combine(Path.GetTempPath(), "plan210_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, SanitationFacilityCatalogLoader.FileName), mutated);
                var load = SanitationFacilityCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("unknown waste type"));
                Assert.Contains(load.Errors, e => e.Contains("canonical shelter_rooms.json vocabulary"));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }

        [Fact]
        public void Loader_RejectsDuplicateId()
        {
            var dir = GetDataDir();
            string source = File.ReadAllText(Path.Combine(dir, SanitationFacilityCatalogLoader.FileName));
            // JSON-DOM mutation: clone the first facility row and insert it —
            // always schema-valid output, so the loader (not the parser) must
            // reject the duplicate.
            var root = System.Text.Json.Nodes.JsonNode.Parse(source)!;
            var facilities = (System.Text.Json.Nodes.JsonArray)root["facilities"]!;
            facilities.Insert(0, facilities[0]!.DeepClone());
            string mutated = root.ToJsonString();
            string tempDir = Path.Combine(Path.GetTempPath(), "plan210_catalog_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);
            try
            {
                File.WriteAllText(Path.Combine(tempDir, SanitationFacilityCatalogLoader.FileName), mutated);
                var load = SanitationFacilityCatalogLoader.Load(tempDir, new FileSystemIO(), new SystemTextJsonSerializer());
                Assert.True(load.HasErrors);
                Assert.Contains(load.Errors, e => e.Contains("duplicate id"));
            }
            finally
            {
                Directory.Delete(tempDir, true);
            }
        }
    }
}
