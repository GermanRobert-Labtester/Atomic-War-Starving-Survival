using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class PneumaticTubeDispatchCatalogTests
    {
        private readonly string _narrativeDir;

        public PneumaticTubeDispatchCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _narrativeDir = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative");
            if (!Directory.Exists(_narrativeDir))
            {
                _narrativeDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative");
            }
        }

        [Fact]
        public void PneumaticTubeDispatchCatalog_LoadsAll30EntriesAcross4Batches()
        {
            Assert.True(Directory.Exists(_narrativeDir), $"Directory not found: {_narrativeDir}");

            var catalog = PneumaticTubeDispatchCatalog.LoadFromDirectory(_narrativeDir);
            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.CarrierEntries.Count);
            Assert.Equal(8, catalog.DiverterEntries.Count);
            Assert.Equal(7, catalog.BlowerEntries.Count);
            Assert.Equal(7, catalog.CylinderEntries.Count);
            Assert.Equal(30, catalog.TotalCount);
        }

        [Fact]
        public void PneumaticTubeDispatchCatalog_Carrier_Integrity()
        {
            var catalog = PneumaticTubeDispatchCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CarrierEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pneumatic_carrier_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.CapsuleSerialNumber));
                Assert.True(item.CarrierDiameterMm > 0);
                Assert.True(item.TransitVelocityMS > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCarrier("pneumatic_carrier_felt_seal_ring_abrasion");
            Assert.NotNull(entry);
            Assert.Equal("DISPATCH_CARRIER_UNIT_042", entry.CapsuleSerialNumber);
        }

        [Fact]
        public void PneumaticTubeDispatchCatalog_Diverter_Integrity()
        {
            var catalog = PneumaticTubeDispatchCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.DiverterEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pneumatic_diverter_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.DiverterJunctionId));
                Assert.False(string.IsNullOrWhiteSpace(item.DiverterMechanismType));
                Assert.True(item.TubeInternalDiameterMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetDiverter("pneumatic_diverter_rotary_switch_flap_misalignment");
            Assert.NotNull(entry);
            Assert.Equal("CENTRAL_HUB_SWITCH_BAY_01", entry.DiverterJunctionId);
        }

        [Fact]
        public void PneumaticTubeDispatchCatalog_Blower_Integrity()
        {
            var catalog = PneumaticTubeDispatchCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.BlowerEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("rootes_blower_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.BlowerStationId));
                Assert.False(string.IsNullOrWhiteSpace(item.RotorLobeConfiguration));
                Assert.True(item.VolumetricFlowM3Min > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetBlower("rootes_blower_lobed_rotor_timing_gear_backlash");
            Assert.NotNull(entry);
            Assert.Equal("MAIN_EXHAUST_VACUUM_BLOWER_01", entry.BlowerStationId);
        }

        [Fact]
        public void PneumaticTubeDispatchCatalog_Cylinder_Integrity()
        {
            var catalog = PneumaticTubeDispatchCatalog.LoadFromDirectory(_narrativeDir);

            foreach (var item in catalog.CylinderEntries)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id));
                Assert.StartsWith("pneumatic_cylinder_", item.Id);
                Assert.False(string.IsNullOrWhiteSpace(item.ActuatorCylinderId));
                Assert.False(string.IsNullOrWhiteSpace(item.PackingLeatherType));
                Assert.True(item.OperatingPressureBar > 0);
                Assert.True(item.BoreDiameterMm > 0);
                Assert.False(string.IsNullOrWhiteSpace(item.Prose));
                Assert.NotEmpty(item.Tags);
            }

            var entry = catalog.GetCylinder("pneumatic_cylinder_oak_tanned_cup_leather_curl");
            Assert.NotNull(entry);
            Assert.Equal("BLAST_VALVE_ACTUATOR_CYLINDER_01", entry.ActuatorCylinderId);
        }
    }
}
