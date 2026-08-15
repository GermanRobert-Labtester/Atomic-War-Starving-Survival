using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class RopeMakingCordageCatalogTests
    {
        private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
        private static RopeMakingCordageCatalog Load() => RopeMakingCordageCatalog.LoadFromDirectory(DataDir);

        [Fact] public void HecklingLogs_LoadsEightEntries()  => Assert.Equal(8, Load().HecklingLogs.Count);
        [Fact] public void StrandReports_LoadsEightEntries() => Assert.Equal(8, Load().StrandReports.Count);
        [Fact] public void ClosingLogs_LoadsSevenEntries()   => Assert.Equal(7, Load().ClosingLogs.Count);
        [Fact] public void BreakAssays_LoadsSevenEntries()   => Assert.Equal(7, Load().BreakAssays.Count);

        [Fact] public void HecklingLogs_AllIdsPopulated()        { foreach (var e in Load().HecklingLogs)  Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void HecklingLogs_AllPlantsPopulated()     { foreach (var e in Load().HecklingLogs)  Assert.False(string.IsNullOrWhiteSpace(e.FibreSourcePlant)); }
        [Fact] public void HecklingLogs_RettingDaysPositive()    { foreach (var e in Load().HecklingLogs)  Assert.True(e.RettingDays > 0, $"{e.Id}: retting_days must be > 0"); }
        [Fact] public void HecklingLogs_AllCombIdsPopulated()    { foreach (var e in Load().HecklingLogs)  Assert.False(string.IsNullOrWhiteSpace(e.HecklingCombId)); }
        [Fact] public void HecklingLogs_AllLogTextsPopulated()   { foreach (var e in Load().HecklingLogs)  Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void StrandReports_AllIdsPopulated()         { foreach (var e in Load().StrandReports) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void StrandReports_AllFibreTypesPopulated()  { foreach (var e in Load().StrandReports) Assert.False(string.IsNullOrWhiteSpace(e.FibreType)); }
        [Fact] public void StrandReports_AllTwistDirsPopulated()   { foreach (var e in Load().StrandReports) Assert.False(string.IsNullOrWhiteSpace(e.TwistDirection)); }
        [Fact] public void StrandReports_StrandCountPositive()     { foreach (var e in Load().StrandReports) Assert.True(e.StrandCountPerYarn > 0, $"{e.Id}: strand_count_per_yarn must be > 0"); }
        [Fact] public void StrandReports_AllLogTextsPopulated()    { foreach (var e in Load().StrandReports) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void ClosingLogs_AllIdsPopulated()       { foreach (var e in Load().ClosingLogs) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void ClosingLogs_DiameterPositive()      { foreach (var e in Load().ClosingLogs) Assert.True(e.RopeDiameterMm > 0f, $"{e.Id}: rope_diameter_mm must be > 0"); }
        [Fact] public void ClosingLogs_AllToolsPopulated()     { foreach (var e in Load().ClosingLogs) Assert.False(string.IsNullOrWhiteSpace(e.ClosingTool)); }
        [Fact] public void ClosingLogs_AllLogTextsPopulated()  { foreach (var e in Load().ClosingLogs) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void BreakAssays_AllIdsPopulated()        { foreach (var e in Load().BreakAssays) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void BreakAssays_TestLoadPositive()       { foreach (var e in Load().BreakAssays) Assert.True(e.TestLoadKg > 0f, $"{e.Id}: test_load_kg must be > 0"); }
        [Fact] public void BreakAssays_AllFailureModesPopulated(){ foreach (var e in Load().BreakAssays) Assert.False(string.IsNullOrWhiteSpace(e.FailureMode)); }
        [Fact] public void BreakAssays_AllLogTextsPopulated()   { foreach (var e in Load().BreakAssays) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GetHecklingByPlant_Nettle_NotEmpty()      => Assert.NotEmpty(Load().GetHecklingLogsByPlant("nettle"));
        [Fact] public void GetStrandByFibre_Hemp_NotEmpty()          => Assert.NotEmpty(Load().GetStrandReportsByFibre("hemp_line"));
        [Fact] public void GetClosingByTool_TopHook_NotEmpty()       => Assert.NotEmpty(Load().GetClosingLogsByTool("top_hook_iron"));
        [Fact] public void GetBreakByFailureMode_StrandParting_NotEmpty() => Assert.NotEmpty(Load().GetBreakAssaysByFailureMode("strand_parting"));
        [Fact] public void GetRopesAbove50kg_NotEmpty()              => Assert.NotEmpty(Load().GetRopesAboveTestLoad(50f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var c = Load();
            Assert.Equal(30, c.HecklingLogs.Count + c.StrandReports.Count + c.ClosingLogs.Count + c.BreakAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var c = Load();
            foreach (var e in c.HecklingLogs)  Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.StrandReports) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.ClosingLogs)   Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.BreakAssays)   Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var c = Load();
            foreach (var e in c.HecklingLogs)  Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.StrandReports) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.ClosingLogs)   Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.BreakAssays)   Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
        }
    }
}
