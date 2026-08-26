using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class DosimeterCalibrationSystemTests
    {
        // ── Device registration ──────────────────────────────────────

        [Fact]
        public void RegisterDevice_CreatesDeviceWithFullBattery()
        {
            var sys = new DosimeterCalibrationSystem();
            Assert.True(sys.RegisterDevice("tag_1", "sv_x"));
            var device = sys.GetDevice("tag_1");
            Assert.NotNull(device);
            Assert.Equal(1.0f, device!.batteryLevel);
            Assert.Equal(1.0f, device.sensorCondition);
            Assert.Equal(1.0f, device.calibrationQuality);
        }

        [Fact]
        public void RegisterDevice_RejectsDuplicate()
        {
            var sys = new DosimeterCalibrationSystem();
            Assert.True(sys.RegisterDevice("tag_1", "sv_x"));
            Assert.False(sys.RegisterDevice("tag_1", "sv_x"));
        }

        [Fact]
        public void RegisterDevice_RejectsEmptyInputs()
        {
            var sys = new DosimeterCalibrationSystem();
            Assert.False(sys.RegisterDevice("", "sv_x"));
            Assert.False(sys.RegisterDevice("tag_1", ""));
            Assert.False(sys.RegisterDevice(null, "sv_x"));
        }

        [Fact]
        public void UnregisterDevice_RemovesDevice()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            Assert.True(sys.UnregisterDevice("tag_1"));
            Assert.Null(sys.GetDevice("tag_1"));
        }

        // ── Reading consumption ──────────────────────────────────────

        [Fact]
        public void ConsumeReading_DrainsBattery()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            float before = sys.GetDevice("tag_1")!.batteryLevel;
            sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.batteryLevel < before);
        }

        [Fact]
        public void ConsumeReading_WearsSensor()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            float before = sys.GetDevice("tag_1")!.sensorCondition;
            sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.sensorCondition < before);
        }

        [Fact]
        public void ConsumeReading_IncrementsCounter()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.ConsumeReading("tag_1");
            Assert.Equal(1, sys.GetDevice("tag_1")!.readingsSinceCalibration);
        }

        [Fact]
        public void ConsumeReading_MarksOverdueAfterThreshold()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            for (int i = 0; i < DosimeterCalibrationSystem.ReadingsPerCalibration; i++)
                sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.isOverdue);
        }

        [Fact]
        public void ConsumeReading_RaisesOnCalibrationOverdue()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            string? overdueTag = null;
            sys.OnCalibrationOverdue += tag => overdueTag = tag;
            for (int i = 0; i < DosimeterCalibrationSystem.ReadingsPerCalibration; i++)
                sys.ConsumeReading("tag_1");
            Assert.Equal("tag_1", overdueTag);
        }

        [Fact]
        public void ConsumeReading_WidensErrorBandWhenOverdue()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            float errorBefore = sys.GetErrorBand("tag_1");
            for (int i = 0; i < DosimeterCalibrationSystem.ReadingsPerCalibration; i++)
                sys.ConsumeReading("tag_1");
            float errorAfter = sys.GetErrorBand("tag_1");
            Assert.True(errorAfter > errorBefore, "Overdue device should have wider error band");
        }

        // ── Calibration ──────────────────────────────────────────────

        [Fact]
        public void StartCalibration_SetsStationOccupied()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            Assert.True(sys.StartCalibration("tag_1", 10));
            Assert.True(sys.IsCalibrating("tag_1"));
        }

        [Fact]
        public void StartCalibration_RejectsLowBattery()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            var device = sys.GetDevice("tag_1")!;
            device.batteryLevel = 0.01f; // below threshold
            Assert.False(sys.StartCalibration("tag_1", 10));
        }

        [Fact]
        public void StartCalibration_RejectsDamagedSensor()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            var device = sys.GetDevice("tag_1")!;
            device.sensorCondition = 0.05f; // below threshold
            Assert.False(sys.StartCalibration("tag_1", 10));
        }

        [Fact]
        public void StartCalibration_RejectsWhenStationOccupied()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            Assert.True(sys.StartCalibration("tag_1", 10));
            Assert.False(sys.StartCalibration("tag_1", 10)); // same day, still occupied
        }

        [Fact]
        public void CompleteCalibration_ResetsReadingCounter()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            // Add some readings
            for (int i = 0; i < 10; i++) sys.ConsumeReading("tag_1");
            Assert.Equal(10, sys.GetDevice("tag_1")!.readingsSinceCalibration);
            // Calibrate
            sys.StartCalibration("tag_1", 10);
            Assert.True(sys.CompleteCalibration("tag_1", 11)); // after 1 day
            Assert.Equal(0, sys.GetDevice("tag_1")!.readingsSinceCalibration);
        }

        [Fact]
        public void CompleteCalibration_ClearsOverdue()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            for (int i = 0; i < DosimeterCalibrationSystem.ReadingsPerCalibration; i++)
                sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.isOverdue);
            sys.StartCalibration("tag_1", 10);
            sys.CompleteCalibration("tag_1", 11);
            Assert.False(sys.GetDevice("tag_1")!.isOverdue);
        }

        [Fact]
        public void CompleteCalibration_ImprovesQuality()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            var device = sys.GetDevice("tag_1")!;
            device.calibrationQuality = 0.5f; // degraded
            sys.StartCalibration("tag_1", 10);
            sys.CompleteCalibration("tag_1", 11);
            Assert.True(device.calibrationQuality > 0.5f, "Calibration should improve quality");
        }

        [Fact]
        public void CompleteCalibration_NarrowsErrorBand()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            // Degrade the device
            for (int i = 0; i < DosimeterCalibrationSystem.ReadingsPerCalibration; i++)
                sys.ConsumeReading("tag_1");
            float errorOverdue = sys.GetErrorBand("tag_1");
            // Calibrate
            sys.StartCalibration("tag_1", 10);
            sys.CompleteCalibration("tag_1", 11);
            float errorCalibrated = sys.GetErrorBand("tag_1");
            Assert.True(errorCalibrated < errorOverdue, "Calibrated device should have narrower error band");
        }

        [Fact]
        public void CompleteCalibration_RejectsIfNotReady()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.StartCalibration("tag_1", 10);
            Assert.False(sys.CompleteCalibration("tag_1", 10)); // same day, not ready yet
        }

        [Fact]
        public void CompleteCalibration_RejectsIfNotCalibrating()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            Assert.False(sys.CompleteCalibration("tag_1", 10));
        }

        [Fact]
        public void CancelCalibration_ClearsStation()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.StartCalibration("tag_1", 10);
            Assert.True(sys.CancelCalibration("tag_1"));
            Assert.False(sys.IsCalibrating("tag_1"));
        }

        // ── Battery and maintenance ──────────────────────────────────

        [Fact]
        public void ReplaceBattery_RestoresFullBattery()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.batteryLevel < 1.0f);
            sys.ReplaceBattery("tag_1");
            Assert.Equal(1.0f, sys.GetDevice("tag_1")!.batteryLevel);
        }

        [Fact]
        public void ServiceSensor_RestoresFullCondition()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.ConsumeReading("tag_1");
            Assert.True(sys.GetDevice("tag_1")!.sensorCondition < 1.0f);
            sys.ServiceSensor("tag_1");
            Assert.Equal(1.0f, sys.GetDevice("tag_1")!.sensorCondition);
        }

        // ── Queries ──────────────────────────────────────────────────

        [Fact]
        public void CanTakeReading_ReturnsFalseWhenBatteryDead()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.GetDevice("tag_1")!.batteryLevel = 0f;
            Assert.False(sys.CanTakeReading("tag_1"));
        }

        [Fact]
        public void CanTakeReading_ReturnsFalseWhenSensorDead()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.GetDevice("tag_1")!.sensorCondition = 0f;
            Assert.False(sys.CanTakeReading("tag_1"));
        }

        [Fact]
        public void GetConfidence_ReturnsZeroForDeadDevice()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.GetDevice("tag_1")!.batteryLevel = 0f;
            Assert.Equal(0f, sys.GetConfidence("tag_1"));
        }

        [Fact]
        public void GetConfidence_DecreasesWithSensorWear()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            float confBefore = sys.GetConfidence("tag_1");
            sys.GetDevice("tag_1")!.sensorCondition = 0.5f;
            float confAfter = sys.GetConfidence("tag_1");
            Assert.True(confAfter < confBefore);
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.ConsumeReading("tag_1");
            sys.ConsumeReading("tag_1");
            sys.StartCalibration("tag_1", 10);

            var state = sys.CaptureState();
            var sys2 = new DosimeterCalibrationSystem();
            sys2.RestoreState(state);

            var device = sys2.GetDevice("tag_1");
            Assert.NotNull(device);
            Assert.Equal(2, device!.readingsSinceCalibration);
            Assert.True(device.isStationOccupied);
        }

        [Fact]
        public void CaptureState_OrdinalOrdered()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_z", "sv_z");
            sys.RegisterDevice("tag_a", "sv_a");
            var state = sys.CaptureState();
            Assert.Equal("tag_a", state.devices[0].deviceTag);
            Assert.Equal("tag_z", state.devices[1].deviceTag);
        }

        [Fact]
        public void Checksum_Stable()
        {
            var sys = new DosimeterCalibrationSystem();
            sys.RegisterDevice("tag_1", "sv_x");
            sys.ConsumeReading("tag_1");
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new DosimeterCalibrationSystem();
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameInputs_SameErrorBand()
        {
            var a = new DosimeterCalibrationSystem();
            a.RegisterDevice("tag_1", "sv_x");
            for (int i = 0; i < 20; i++) a.ConsumeReading("tag_1");

            var b = new DosimeterCalibrationSystem();
            b.RegisterDevice("tag_1", "sv_x");
            for (int i = 0; i < 20; i++) b.ConsumeReading("tag_1");

            Assert.Equal(a.GetErrorBand("tag_1"), b.GetErrorBand("tag_1"));
        }

        // ── True dose unchanged ──────────────────────────────────────

        [Fact]
        public void Calibration_DoesNotAlterDoseLedger()
        {
            // Key invariant: calibration affects measurement confidence,
            // NOT the actual cumulative dose in DoseLedgerSystem.
            var ledger = new DoseLedgerSystem();
            var cal = new DosimeterCalibrationSystem();
            ledger.AssignDosimeter("sv_x", "tag_1");
            cal.RegisterDevice("tag_1", "sv_x");

            // Book a reading
            ledger.BookReading("sv_x", 1, 100f, "test", false, false, false, new SeededRng(1));
            float doseBefore = ledger.GetCumulative("sv_x");

            // Calibrate
            cal.StartCalibration("tag_1", 1);
            cal.CompleteCalibration("tag_1", 2);

            // Dose must be unchanged
            Assert.Equal(doseBefore, ledger.GetCumulative("sv_x"));
        }
    }
}
