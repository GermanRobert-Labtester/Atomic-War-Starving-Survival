// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class BoneHornCarvingCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static BoneHornCarvingCatalog Load() =>
            BoneHornCarvingCatalog.LoadFromDirectory(DataDir);

        // Structural checks share one catalog load and retain all failing
        // field names in the assertion message. Query and aggregate behavior
        // stays split below for useful failure isolation.
        [Fact]
        public void Catalog_StructuralContract_IsComplete()
        {
            var catalog = Load();

            AssertCounts(
                ("DegreasingLogs", catalog.DegreasingLogs.Count, 8),
                ("SawingRecords", catalog.SawingRecords.Count, 8),
                ("PolishingReports", catalog.PolishingReports.Count, 7),
                ("ToolAssays", catalog.ToolAssays.Count, 7));

            AssertStringPropertiesPopulated(
                "DegreasingLogs", catalog.DegreasingLogs, e => e.Id,
                ("id", e => e.Id),
                ("bone_source_animal", e => e.BoneSourceAnimal),
                ("degreasing_method", e => e.DegreasingMethod),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "DegreasingLogs", catalog.DegreasingLogs, e => e.Id,
                ("prep_duration_days", e => (double)e.PrepDurationDays));

            AssertStringPropertiesPopulated(
                "SawingRecords", catalog.SawingRecords, e => e.Id,
                ("id", e => e.Id),
                ("material_type", e => e.MaterialType),
                ("saw_tool_id", e => e.SawToolId),
                ("blank_shape_cut", e => e.BlankShapeCut),
                ("log_text", e => e.LogText));

            AssertStringPropertiesPopulated(
                "PolishingReports", catalog.PolishingReports, e => e.Id,
                ("id", e => e.Id),
                ("abrasive_used", e => e.AbrasiveUsed),
                ("surface_finish", e => e.SurfaceFinish),
                ("log_text", e => e.LogText));

            AssertStringPropertiesPopulated(
                "ToolAssays", catalog.ToolAssays, e => e.Id,
                ("id", e => e.Id),
                ("tool_type", e => e.ToolType),
                ("bone_blank_id", e => e.BoneBlankId),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "ToolAssays", catalog.ToolAssays, e => e.Id,
                ("point_angle_degrees", e => (double)e.PointAngleDegrees));
        }

        [Fact] public void GetDegreasingByAnimal_Dog_NotEmpty() =>
            Assert.NotEmpty(Load().GetDegreasingLogsByAnimal("dog"));

        [Fact] public void GetSawingByMaterial_DeerAntler_NotEmpty() =>
            Assert.NotEmpty(Load().GetSawingRecordsByMaterial("deer_antler"));

        [Fact] public void GetPolishingByAbrasive_Sandstone_NotEmpty() =>
            Assert.NotEmpty(Load().GetPolishingReportsByAbrasive("sandstone_block"));

        [Fact] public void GetToolByType_Needle_NotEmpty() =>
            Assert.NotEmpty(Load().GetToolAssaysByType("needle"));

        [Fact] public void GetSharpTools_30deg_NotEmpty() =>
            Assert.NotEmpty(Load().GetSharpToolAssays(30f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var catalog = Load();
            Assert.Equal(30,
                catalog.DegreasingLogs.Count + catalog.SawingRecords.Count +
                catalog.PolishingReports.Count + catalog.ToolAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var catalog = Load();
            foreach (var entry in catalog.DegreasingLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.SawingRecords) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.PolishingReports) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.ToolAssays) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var catalog = Load();
            foreach (var entry in catalog.DegreasingLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.SawingRecords) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.PolishingReports) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.ToolAssays) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
        }
    }
}
