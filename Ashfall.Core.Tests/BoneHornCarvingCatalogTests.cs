using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class BoneHornCarvingCatalogTests
    : CatalogTestBase{
        private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
        private static BoneHornCarvingCatalog Load() => BoneHornCarvingCatalog.LoadFromDirectory(DataDir);

        [Fact] public void DegreasingLogs_LoadsEightEntries() => Assert.Equal(8, Load().DegreasingLogs.Count);
        [Fact] public void SawingRecords_LoadsEightEntries() => Assert.Equal(8, Load().SawingRecords.Count);
        [Fact] public void PolishingReports_LoadsSevenEntries() => Assert.Equal(7, Load().PolishingReports.Count);
        [Fact] public void ToolAssays_LoadsSevenEntries() => Assert.Equal(7, Load().ToolAssays.Count);

        [Fact] public void DegreasingLogs_AllIdsPopulated() { foreach (var e in Load().DegreasingLogs) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void DegreasingLogs_AllAnimalsPopulated() { foreach (var e in Load().DegreasingLogs) Assert.False(string.IsNullOrWhiteSpace(e.BoneSourceAnimal)); }
        [Fact] public void DegreasingLogs_AllMethodsPopulated() { foreach (var e in Load().DegreasingLogs) Assert.False(string.IsNullOrWhiteSpace(e.DegreasingMethod)); }
        [Fact] public void DegreasingLogs_PrepDaysPositive() { foreach (var e in Load().DegreasingLogs) Assert.True(e.PrepDurationDays > 0, $"{e.Id}: prep_duration_days must be > 0"); }
        [Fact] public void DegreasingLogs_AllLogTextsPopulated() { foreach (var e in Load().DegreasingLogs) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void SawingRecords_AllIdsPopulated() { foreach (var e in Load().SawingRecords) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void SawingRecords_AllMaterialsPopulated() { foreach (var e in Load().SawingRecords) Assert.False(string.IsNullOrWhiteSpace(e.MaterialType)); }
        [Fact] public void SawingRecords_AllSawToolIdsPopulated() { foreach (var e in Load().SawingRecords) Assert.False(string.IsNullOrWhiteSpace(e.SawToolId)); }
        [Fact] public void SawingRecords_AllShapeCutsPopulated() { foreach (var e in Load().SawingRecords) Assert.False(string.IsNullOrWhiteSpace(e.BlankShapeCut)); }
        [Fact] public void SawingRecords_AllLogTextsPopulated() { foreach (var e in Load().SawingRecords) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void PolishingReports_AllIdsPopulated() { foreach (var e in Load().PolishingReports) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void PolishingReports_AllAbrasivesPopulated() { foreach (var e in Load().PolishingReports) Assert.False(string.IsNullOrWhiteSpace(e.AbrasiveUsed)); }
        [Fact] public void PolishingReports_AllFinishesPopulated() { foreach (var e in Load().PolishingReports) Assert.False(string.IsNullOrWhiteSpace(e.SurfaceFinish)); }
        [Fact] public void PolishingReports_AllLogTextsPopulated() { foreach (var e in Load().PolishingReports) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void ToolAssays_AllIdsPopulated() { foreach (var e in Load().ToolAssays) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void ToolAssays_AllToolTypesPopulated() { foreach (var e in Load().ToolAssays) Assert.False(string.IsNullOrWhiteSpace(e.ToolType)); }
        [Fact] public void ToolAssays_AllBoneBlankIdsPopulated() { foreach (var e in Load().ToolAssays) Assert.False(string.IsNullOrWhiteSpace(e.BoneBlankId)); }
        [Fact] public void ToolAssays_PointAnglePositive() { foreach (var e in Load().ToolAssays) Assert.True(e.PointAngleDegrees > 0f, $"{e.Id}: point_angle_degrees must be > 0"); }
        [Fact] public void ToolAssays_AllLogTextsPopulated() { foreach (var e in Load().ToolAssays) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GetDegreasingByAnimal_Dog_NotEmpty() => Assert.NotEmpty(Load().GetDegreasingLogsByAnimal("dog"));
        [Fact] public void GetSawingByMaterial_DeerAntler_NotEmpty() => Assert.NotEmpty(Load().GetSawingRecordsByMaterial("deer_antler"));
        [Fact] public void GetPolishingByAbrasive_Sandstone_NotEmpty() => Assert.NotEmpty(Load().GetPolishingReportsByAbrasive("sandstone_block"));
        [Fact] public void GetToolByType_Needle_NotEmpty() => Assert.NotEmpty(Load().GetToolAssaysByType("needle"));
        [Fact] public void GetSharpTools_30deg_NotEmpty() => Assert.NotEmpty(Load().GetSharpToolAssays(30f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var c = Load();
            Assert.Equal(30, c.DegreasingLogs.Count + c.SawingRecords.Count + c.PolishingReports.Count + c.ToolAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var c = Load();
            foreach (var e in c.DegreasingLogs) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.SawingRecords) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.PolishingReports) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.ToolAssays) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var c = Load();
            foreach (var e in c.DegreasingLogs) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.SawingRecords) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.PolishingReports) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.ToolAssays) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
        }
    }
}
