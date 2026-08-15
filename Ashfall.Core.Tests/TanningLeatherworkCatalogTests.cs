using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class TanningLeatherworkCatalogTests
    {
        private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
        private static TanningLeatherworkCatalog Load() => TanningLeatherworkCatalog.LoadFromDirectory(DataDir);

        [Fact] public void VatLogs_LoadsEightEntries()    => Assert.Equal(8, Load().VatLogs.Count);
        [Fact] public void HideReports_LoadsEightEntries() => Assert.Equal(8, Load().HideReports.Count);
        [Fact] public void CurryingAssays_LoadsSevenEntries() => Assert.Equal(7, Load().CurryingAssays.Count);
        [Fact] public void StitchJournals_LoadsSevenEntries() => Assert.Equal(7, Load().StitchJournals.Count);

        [Fact] public void VatLogs_AllIdsPopulated()        { foreach (var e in Load().VatLogs)        Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void VatLogs_AllBarkSpeciesPopulated() { foreach (var e in Load().VatLogs)        Assert.False(string.IsNullOrWhiteSpace(e.BarkSpecies)); }
        [Fact] public void VatLogs_LiquorStrengthPositive()  { foreach (var e in Load().VatLogs)        Assert.True(e.LiquorStrengthBaume > 0f, $"{e.Id}: liquor_strength_baume must be > 0"); }
        [Fact] public void VatLogs_AllLogTextsPopulated()    { foreach (var e in Load().VatLogs)        Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void HideReports_AllIdsPopulated()       { foreach (var e in Load().HideReports) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void HideReports_AllAnimalsPopulated()    { foreach (var e in Load().HideReports) Assert.False(string.IsNullOrWhiteSpace(e.HideSourceAnimal)); }
        [Fact] public void HideReports_SmokeCycleCountPositive(){ foreach (var e in Load().HideReports) Assert.True(e.SmokeCycleCount > 0, $"{e.Id}: smoke_cycle_count must be > 0"); }
        [Fact] public void HideReports_AllLogTextsPopulated()   { foreach (var e in Load().HideReports) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void CurryingAssays_AllIdsPopulated()         { foreach (var e in Load().CurryingAssays) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void CurryingAssays_AllFatLiquorTypesPopulated(){ foreach (var e in Load().CurryingAssays) Assert.False(string.IsNullOrWhiteSpace(e.FatLiquorType)); }
        [Fact] public void CurryingAssays_AllLogTextsPopulated()     { foreach (var e in Load().CurryingAssays) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void StitchJournals_AllIdsPopulated()       { foreach (var e in Load().StitchJournals) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void StitchJournals_StitchLengthPositive()  { foreach (var e in Load().StitchJournals) Assert.True(e.StitchLengthMm > 0f, $"{e.Id}: stitch_length_mm must be > 0"); }
        [Fact] public void StitchJournals_AllThreadsPopulated()   { foreach (var e in Load().StitchJournals) Assert.False(string.IsNullOrWhiteSpace(e.ThreadMaterial)); }
        [Fact] public void StitchJournals_AllLogTextsPopulated()  { foreach (var e in Load().StitchJournals) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GetVatLogsByBarkSpecies_ReturnsResults()  => Assert.NotEmpty(Load().GetVatLogsByBarkSpecies("oak"));
        [Fact] public void GetBrainTanByAnimal_ReturnsResults()      => Assert.NotEmpty(Load().GetBrainTanReportsByAnimal("dog"));
        [Fact] public void GetCurryingByFatLiquor_ReturnsResults()   => Assert.NotEmpty(Load().GetCurryingAssaysByFatLiquor("tallow"));
        [Fact] public void GetStitchByThread_ReturnsResults()        => Assert.NotEmpty(Load().GetStitchJournalsByThread("sinew"));
        [Fact] public void GetSmokeCycleReports_MinTwo_NotEmpty()    => Assert.NotEmpty(Load().GetSmokeCycleReports(2));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var c = Load();
            Assert.Equal(30, c.VatLogs.Count + c.HideReports.Count + c.CurryingAssays.Count + c.StitchJournals.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var c = Load();
            foreach (var e in c.VatLogs)        Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.HideReports)    Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.CurryingAssays) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.StitchJournals) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var c = Load();
            foreach (var e in c.VatLogs)        Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.HideReports)    Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.CurryingAssays) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.StitchJournals) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
        }
    }
}
