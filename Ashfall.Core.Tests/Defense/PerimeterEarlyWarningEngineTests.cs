// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Defense;
using Xunit;

namespace Ashfall.Core.Tests.Defense
{
    public class PerimeterEarlyWarningEngineTests
    {
        [Fact]
        public void SetMode_UpdatesPowerDrawAndRange()
        {
            var engine = new PerimeterEarlyWarningEngine();
            Assert.Equal(RadarOperationalMode.ActiveScan, engine.Mode);
            Assert.Equal(250, engine.PowerDrawWatts);
            Assert.Equal(3000, engine.DetectionRangeMeters);

            engine.SetMode(RadarOperationalMode.HighFrequencySweep);
            Assert.Equal(600, engine.PowerDrawWatts);
            Assert.Equal(6000, engine.DetectionRangeMeters);

            engine.SetMode(RadarOperationalMode.Off);
            Assert.Equal(0, engine.PowerDrawWatts);
            Assert.Equal(0, engine.DetectionRangeMeters);
        }

        [Fact]
        public void ProcessScanSweep_DetectsHostilesWithinRange()
        {
            var engine = new PerimeterEarlyWarningEngine();
            engine.SetMode(RadarOperationalMode.ActiveScan); // 3000m range

            RadarContact? detectedThreat = null;
            engine.OnThreatDetected += c => detectedThreat = c;

            // Hostile raiders detected at 1500m in north sector
            var contact = engine.ProcessScanSweep(
                sector: PerimeterSector.North,
                actualDistanceMeters: 1500,
                isHostile: true,
                isStormOrDust: false,
                currentTick: 100,
                seededRollPermille: 200);

            Assert.NotNull(contact);
            Assert.Equal(PerimeterSector.North, contact.Sector);
            Assert.Equal(1500, contact.DistanceMeters);
            Assert.Equal(RadarTargetClassification.HostileIncursion, contact.Classification);
            Assert.Equal(30, contact.EstimatedArrivalMinutes); // 1500m / 50 m/min = 30 min
            Assert.NotNull(detectedThreat);
            Assert.Equal(contact.ContactId, detectedThreat.ContactId);
        }

        [Fact]
        public void ProcessScanSweep_BeyondRangeOrOff_ReturnsNull()
        {
            var engine = new PerimeterEarlyWarningEngine();
            engine.SetMode(RadarOperationalMode.LowPowerStandby); // 1000m range

            // Contact at 2500m (beyond 1000m)
            var contact = engine.ProcessScanSweep(PerimeterSector.East, 2500, true, false, 50, 100);
            Assert.Null(contact);

            // Radar turned off
            engine.SetMode(RadarOperationalMode.Off);
            var contactOff = engine.ProcessScanSweep(PerimeterSector.East, 500, true, false, 50, 100);
            Assert.Null(contactOff);
        }

        [Fact]
        public void FalseAlarmCalibration_StormMisclassification_ResolvedByHigherCalibration()
        {
            var engine = new PerimeterEarlyWarningEngine();
            // Default calibration is 500 permille (50%)
            Assert.Equal(500, engine.CalibrationPermille);

            RadarContact? falseAlarm = null;
            engine.OnFalseAlarmTriggered += c => falseAlarm = c;

            // Dust storm with roll 700 (700 >= 500 calibration) -> False alarm!
            var contact1 = engine.ProcessScanSweep(
                PerimeterSector.West,
                actualDistanceMeters: 1000,
                isHostile: false,
                isStormOrDust: true,
                currentTick: 80,
                seededRollPermille: 700);

            Assert.NotNull(contact1);
            Assert.Equal(RadarTargetClassification.HostileIncursion, contact1.Classification);
            Assert.NotNull(falseAlarm);

            // Now calibrate sensors up to 850 permille (+350)
            engine.CalibrateSensors(350);
            Assert.Equal(850, engine.CalibrationPermille);

            // Same dust storm with roll 700 (700 < 850 calibration) -> Correctly classified as EnvironmentalNoise!
            falseAlarm = null;
            var contact2 = engine.ProcessScanSweep(
                PerimeterSector.West,
                actualDistanceMeters: 1000,
                isHostile: false,
                isStormOrDust: true,
                currentTick: 90,
                seededRollPermille: 700);

            Assert.NotNull(contact2);
            Assert.Equal(RadarTargetClassification.EnvironmentalNoise, contact2.Classification);
            Assert.Null(falseAlarm);
        }

        [Fact]
        public void SaveRestoreRoundTrip_PreservesRadarStateAndContacts()
        {
            var engine1 = new PerimeterEarlyWarningEngine();
            engine1.SetMode(RadarOperationalMode.HighFrequencySweep);
            engine1.CalibrateSensors(200); // 700 permille
            engine1.ProcessScanSweep(PerimeterSector.Gate, 1200, true, false, 150, 100);

            var save = engine1.CaptureState();
            Assert.NotNull(save);
            Assert.Equal(RadarOperationalMode.HighFrequencySweep, save.Mode);
            Assert.Equal(700, save.CalibrationPermille);
            Assert.Single(save.Contacts);

            var engine2 = new PerimeterEarlyWarningEngine();
            engine2.RestoreState(save);

            Assert.Equal(RadarOperationalMode.HighFrequencySweep, engine2.Mode);
            Assert.Equal(700, engine2.CalibrationPermille);
            Assert.Single(engine2.ActiveContacts);
            Assert.Equal(PerimeterSector.Gate, engine2.ActiveContacts[0].Sector);
            Assert.Equal(1200, engine2.ActiveContacts[0].DistanceMeters);
        }
    }
}
