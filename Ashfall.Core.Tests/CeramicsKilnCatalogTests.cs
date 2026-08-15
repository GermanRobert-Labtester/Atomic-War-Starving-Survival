using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CeramicsKilnCatalogTests
    {
        private static string DataDir => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
        private static CeramicsKilnCatalog Load() => CeramicsKilnCatalog.LoadFromDirectory(DataDir);

        [Fact] public void WedgingLogs_LoadsEightEntries()   => Assert.Equal(8, Load().WedgingLogs.Count);
        [Fact] public void FiringRecords_LoadsEightEntries() => Assert.Equal(8, Load().FiringRecords.Count);
        [Fact] public void GlazeNotes_LoadsSevenEntries()    => Assert.Equal(7, Load().GlazeNotes.Count);
        [Fact] public void DrawTrials_LoadsSevenEntries()    => Assert.Equal(7, Load().DrawTrials.Count);

        [Fact] public void WedgingLogs_AllIdsPopulated()          { foreach (var e in Load().WedgingLogs)   Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void WedgingLogs_AllClaySourcesPopulated()  { foreach (var e in Load().WedgingLogs)   Assert.False(string.IsNullOrWhiteSpace(e.ClayBedSource)); }
        [Fact] public void WedgingLogs_WedgingCycleCountPositive(){ foreach (var e in Load().WedgingLogs)   Assert.True(e.WedgingCycleCount > 0, $"{e.Id}: wedging_cycle_count must be > 0"); }
        [Fact] public void WedgingLogs_AllFormingMethodsPopulated(){ foreach (var e in Load().WedgingLogs)  Assert.False(string.IsNullOrWhiteSpace(e.FormingMethod)); }
        [Fact] public void WedgingLogs_AllLogTextsPopulated()     { foreach (var e in Load().WedgingLogs)   Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void FiringRecords_AllIdsPopulated()         { foreach (var e in Load().FiringRecords) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void FiringRecords_PeakTempPositive()        { foreach (var e in Load().FiringRecords) Assert.True(e.PeakTempCelsius > 0f, $"{e.Id}: peak_temp_celsius must be > 0"); }
        [Fact] public void FiringRecords_DurationPositive()        { foreach (var e in Load().FiringRecords) Assert.True(e.FiringDurationHours > 0f, $"{e.Id}: firing_duration_hours must be > 0"); }
        [Fact] public void FiringRecords_AllKilnIdsPopulated()     { foreach (var e in Load().FiringRecords) Assert.False(string.IsNullOrWhiteSpace(e.KilnChamberId)); }
        [Fact] public void FiringRecords_AllLogTextsPopulated()    { foreach (var e in Load().FiringRecords) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GlazeNotes_AllIdsPopulated()          { foreach (var e in Load().GlazeNotes) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void GlazeNotes_AllFluxMaterialsPopulated(){ foreach (var e in Load().GlazeNotes) Assert.False(string.IsNullOrWhiteSpace(e.FluxMaterial)); }
        [Fact] public void GlazeNotes_AllLogTextsPopulated()     { foreach (var e in Load().GlazeNotes) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void DrawTrials_AllIdsPopulated()          { foreach (var e in Load().DrawTrials) Assert.False(string.IsNullOrWhiteSpace(e.Id)); }
        [Fact] public void DrawTrials_AllSurfaceResultsPopulated(){ foreach (var e in Load().DrawTrials) Assert.False(string.IsNullOrWhiteSpace(e.SurfaceResult)); }
        [Fact] public void DrawTrials_AllLogTextsPopulated()     { foreach (var e in Load().DrawTrials) Assert.False(string.IsNullOrWhiteSpace(e.LogText)); }

        [Fact] public void GetWedgingByFormingMethod_ReturnsResults()   => Assert.NotEmpty(Load().GetWedgingLogsByFormingMethod("coil_building"));
        [Fact] public void GetFiringByKiln_ReturnsResults()             => Assert.NotEmpty(Load().GetFiringRecordsByKiln("kc_updraft_01"));
        [Fact] public void GetGlazeByFlux_ReturnsResults()              => Assert.NotEmpty(Load().GetGlazeNotesByFlux("wood_ash"));
        [Fact] public void GetDrawTrialsByKiln_ReturnsResults()         => Assert.NotEmpty(Load().GetDrawTrialsByKiln("kc_updraft_01"));
        [Fact] public void GetHighTempFirings_600_ReturnsResults()      => Assert.NotEmpty(Load().GetHighTemperatureFirings(600f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var c = Load();
            Assert.Equal(30, c.WedgingLogs.Count + c.FiringRecords.Count + c.GlazeNotes.Count + c.DrawTrials.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var c = Load();
            foreach (var e in c.WedgingLogs)   Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.FiringRecords) Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.GlazeNotes)    Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
            foreach (var e in c.DrawTrials)    Assert.True(seen.Add(e.Id), $"Duplicate: {e.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var c = Load();
            foreach (var e in c.WedgingLogs)   Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.FiringRecords) Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.GlazeNotes)    Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
            foreach (var e in c.DrawTrials)    Assert.True(e.LogText.Length >= 20, $"{e.Id}: too short");
        }
    }
}
