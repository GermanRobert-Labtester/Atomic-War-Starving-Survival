// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CeramicsKilnCatalogTests : CatalogTestBase
    {
        private static string DataDir => Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..",
            "Assets", "StreamingAssets", "Data", "narrative");

        private static CeramicsKilnCatalog Load() =>
            CeramicsKilnCatalog.LoadFromDirectory(DataDir);

        [Fact]
        public void Catalog_StructuralContract_IsComplete()
        {
            var catalog = Load();

            AssertCounts(
                ("WedgingLogs", catalog.WedgingLogs.Count, 8),
                ("FiringRecords", catalog.FiringRecords.Count, 8),
                ("GlazeNotes", catalog.GlazeNotes.Count, 7),
                ("DrawTrials", catalog.DrawTrials.Count, 7));

            AssertStringPropertiesPopulated(
                "WedgingLogs", catalog.WedgingLogs, e => e.Id,
                ("id", e => e.Id),
                ("clay_bed_source", e => e.ClayBedSource),
                ("forming_method", e => e.FormingMethod),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "WedgingLogs", catalog.WedgingLogs, e => e.Id,
                ("wedging_cycle_count", e => e.WedgingCycleCount));

            AssertStringPropertiesPopulated(
                "FiringRecords", catalog.FiringRecords, e => e.Id,
                ("id", e => e.Id),
                ("kiln_chamber_id", e => e.KilnChamberId),
                ("log_text", e => e.LogText));
            AssertPositiveProperties(
                "FiringRecords", catalog.FiringRecords, e => e.Id,
                ("peak_temp_celsius", e => (double)e.PeakTempCelsius),
                ("firing_duration_hours", e => (double)e.FiringDurationHours));

            AssertStringPropertiesPopulated(
                "GlazeNotes", catalog.GlazeNotes, e => e.Id,
                ("id", e => e.Id),
                ("flux_material", e => e.FluxMaterial),
                ("log_text", e => e.LogText));

            AssertStringPropertiesPopulated(
                "DrawTrials", catalog.DrawTrials, e => e.Id,
                ("id", e => e.Id),
                ("surface_result", e => e.SurfaceResult),
                ("log_text", e => e.LogText));
        }

        [Fact] public void GetWedgingByFormingMethod_ReturnsResults() =>
            Assert.NotEmpty(Load().GetWedgingLogsByFormingMethod("coil_building"));

        [Fact] public void GetFiringByKiln_ReturnsResults() =>
            Assert.NotEmpty(Load().GetFiringRecordsByKiln("kc_updraft_01"));

        [Fact] public void GetGlazeByFlux_ReturnsResults() =>
            Assert.NotEmpty(Load().GetGlazeNotesByFlux("wood_ash"));

        [Fact] public void GetDrawTrialsByKiln_ReturnsResults() =>
            Assert.NotEmpty(Load().GetDrawTrialsByKiln("kc_updraft_01"));

        [Fact] public void GetHighTempFirings_600_ReturnsResults() =>
            Assert.NotEmpty(Load().GetHighTemperatureFirings(600f));

        [Fact]
        public void AllEntries_TotalIsThirty()
        {
            var catalog = Load();
            Assert.Equal(30,
                catalog.WedgingLogs.Count + catalog.FiringRecords.Count +
                catalog.GlazeNotes.Count + catalog.DrawTrials.Count);
        }

        [Fact]
        public void AllEntries_IdsAreUnique()
        {
            var seen = new HashSet<string>();
            var catalog = Load();
            foreach (var entry in catalog.WedgingLogs) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.FiringRecords) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.GlazeNotes) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
            foreach (var entry in catalog.DrawTrials) Assert.True(seen.Add(entry.Id), $"Duplicate: {entry.Id}");
        }

        [Fact]
        public void AllLogTexts_AtLeastTwentyChars()
        {
            var catalog = Load();
            foreach (var entry in catalog.WedgingLogs) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.FiringRecords) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.GlazeNotes) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
            foreach (var entry in catalog.DrawTrials) Assert.True(entry.LogText.Length >= 20, $"{entry.Id}: too short");
        }
    }
}
