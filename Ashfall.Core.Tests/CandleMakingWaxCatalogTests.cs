using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CandleMakingWaxCatalogTests
    {
        private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
        private static CandleMakingWaxCatalog Load() => CandleMakingWaxCatalog.LoadFromDirectory(DataDir);

        [Fact] public void TallowLogs_LoadsEightEntries()  => Assert.Equal(8, Load().TallowLogs.Count);
        [Fact] public void WaxRecords_LoadsEightEntries()  => Assert.Equal(8, Load().WaxRecords.Count);
        [Fact] public void WickReports_LoadsSevenEntries() => Assert.Equal(7, Load().WickReports.Count);
        [Fact] public void CandleAssays_LoadsSevenEntries()=> Assert.Equal(7, Load().CandleAssays.Count);

        [Fact] public void TallowLogs_AllIdsPopulated()        { foreach (var e in Load().TallowLogs)  Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void TallowLogs_AllAnimalsPopulated()    { foreach (var e in Load().TallowLogs)  Assert.False(string.IsNullOrWhiteSpace(e.FatSourceAnimal)); }
        [Fact] public void TallowLogs_YieldGramsPositive()     { foreach (var e in Load().TallowLogs)  Assert.True(e.YieldGrams > 0f, $"{e.Id}: yield_grams must be > 0"); }
        [Fact] public void TallowLogs_AllVatIdsPopulated()     { foreach (var e in Load().TallowLogs)  Assert.False(string.IsNullOrWhiteSpace(e.RenderingVatId)); }
        [Fact] public void TallowLogs_AllLogTextsPopulated()   { foreach (var e in Load().TallowLogs)  Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void WaxRecords_AllIdsPopulated()            { foreach (var e in Load().WaxRecords) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void WaxRecords_AllMethodsPopulated()        { foreach (var e in Load().WaxRecords) Assert.False(string.IsNullOrWhiteSpace(e.ClarificationMethod)); }
        [Fact] public void WaxRecords_AllClarityGradesPopulated()  { foreach (var e in Load().WaxRecords) Assert.False(string.IsNullOrWhiteSpace(e.ClarityGrade)); }
        [Fact] public void WaxRecords_AllLogTextsPopulated()       { foreach (var e in Load().WaxRecords) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void WickReports_AllIdsPopulated()         { foreach (var e in Load().WickReports) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void WickReports_AllFibreTypesPopulated()  { foreach (var e in Load().WickReports) Assert.False(string.IsNullOrWhiteSpace(e.WickFibreType)); }
        [Fact] public void WickReports_BraidPlyCountPositive()   { foreach (var e in Load().WickReports) Assert.True(e.BraidPlyCount > 0, $"{e.Id}: braid_ply_count must be > 0"); }
        [Fact] public void WickReports_AllLogTextsPopulated()    { foreach (var e in Load().WickReports) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void CandleAssays_AllIdsPopulated()          { foreach (var e in Load().CandleAssays) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void CandleAssays_AllMethodsPopulated()      { foreach (var e in Load().CandleAssays) Assert.False(string.IsNullOrWhiteSpace(e.CandleMethod)); }
        [Fact] public void CandleAssays_BurnDurationPositive()     { foreach (var e in Load().CandleAssays) Assert.True(e.BurnDurationHours > 0f, $"{e.Id}: burn_duration_hours must be > 0"); }
        [Fact] public void CandleAssays_AllWaxBlendsPopulated()    { foreach (var e in Load().CandleAssays) Assert.False(string.IsNullOrWhiteSpace(e.WaxBlendType)); }
        [Fact] public void CandleAssays_AllLogTextsPopulated()     { foreach (var e in Load().CandleAssays) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GetTallowByAnimal_Dog_NotEmpty()          => Assert.NotEmpty(Load().GetTallowLogsByAnimal("dog"));
        [Fact] public void GetClarificationByMethod_Float_NotEmpty() => Assert.NotEmpty(Load().GetClarificationRecordsByMethod("hot_water_float"));
        [Fact] public void GetWickByFibre_Cotton_NotEmpty()          => Assert.NotEmpty(Load().GetWickReportsByFibre("cotton_rag_strip"));
        [Fact] public void GetCandleByMethod_Dipping_NotEmpty()      => Assert.NotEmpty(Load().GetCandleAssaysByMethod("dipping"));
        [Fact] public void GetLongBurningCandles_3h_NotEmpty()       => Assert.NotEmpty(Load().GetLongBurningCandles(3f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var c = Load();
            Assert.Equal(30, c.TallowLogs.Count + c.WaxRecords.Count + c.WickReports.Count + c.CandleAssays.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var c = Load();
            foreach (var e in c.TallowLogs)   Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.WaxRecords)   Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.WickReports)  Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.CandleAssays) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var c = Load();
            foreach (var e in c.TallowLogs)   Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.WaxRecords)   Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.WickReports)  Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.CandleAssays) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
        }
    }
}
