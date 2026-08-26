using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class SignalIntelligenceCatalogTests
    : CatalogTestBase{
        private readonly string _narrativeDir;

        public SignalIntelligenceCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void SignalIntelligenceCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = SignalIntelligenceCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.CipherEntries.Count);
            Assert.Equal(8, catalog.SeismicEntries.Count);
            Assert.Equal(7, catalog.EmpEntries.Count);
            Assert.Equal(7, catalog.WiretapEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void SignalIntelligenceCatalog_Ciphers_Integrity()
        {
            var catalog = SignalIntelligenceCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CipherEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("cipher_station_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StationNickname));
                Assert.True(item.TransmissionFrequencyKhz > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.ModulationMode));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCipher("cipher_station_lincolnshire_poacher_echo");
            Assert.NotNull(entry);
            Assert.Equal("THE_POACHER_ECHO", entry.StationNickname);
        }

        [Fact]
        public void SignalIntelligenceCatalog_Seismic_Integrity()
        {
            var catalog = SignalIntelligenceCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.SeismicEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("alarm_seismic_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.StationId));
                Assert.False(string.IsNullOrWhiteSpace(item.AlertTier));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetSeismic("alarm_seismic_overpressure_fault_rupture");
            Assert.NotNull(entry);
            Assert.Equal("SEISMIC-MONITOR-STATION-03", entry.StationId);
        }

        [Fact]
        public void SignalIntelligenceCatalog_Emp_Integrity()
        {
            var catalog = SignalIntelligenceCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.EmpEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("emp_sniffer_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DetectorId));
                Assert.False(string.IsNullOrWhiteSpace(item.PulseClassification));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetEmp("emp_sniffer_high_altitude_e1_spike");
            Assert.NotNull(entry);
            Assert.Equal("EMP-SNIFFER-MAST-ALPHA", entry.DetectorId);
        }

        [Fact]
        public void SignalIntelligenceCatalog_Wiretap_Integrity()
        {
            var catalog = SignalIntelligenceCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.WiretapEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("wiretap_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.InterceptChannel));
                Assert.False(string.IsNullOrWhiteSpace(item.TargetFaction));
                Assert.False(string.IsNullOrWhiteSpace(item.SpeakerIdentities));
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetWiretap("wiretap_office_cartridge_allocation_quarrel");
            Assert.NotNull(entry);
            Assert.Equal("THE_OFFICE", entry.TargetFaction);
        }
    }
}
