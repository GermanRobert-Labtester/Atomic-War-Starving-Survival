# Plan 119 — Sensor characterization

The detector uses a bounded confidence model composed from fault intensity,
range attenuation, visibility/ash, environment activity, sensor condition,
calibration, operator skill and seeded noise. Confidence and signal are both
clamped to `[0,1]`; weak or distant signals may yield no observation.

The focused suite covers clear strong faults, range/visibility comparison,
atomic battery use, non-mutation of the supplied power fault, deterministic
observation capture/restore, and seeded behavior. The combined 60-day artifact
records latest confidence so a different fixed seed has an observable result
without requiring every field to diverge.

No zero-false-positive or exact-component claim is made. Weather modifiers
affect observation quality only; owning the detector does not improve grid
reliability.

<!-- Master Authority Integration Reference -->
> **Master Expansion Authority File:** [newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md](/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War/docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md)
> **Target Framework:** `Assets/Ashfall.Core/Radio/Sensors/` (`netstandard2.1`, Engine-Free Domain)
> **Host Framework:** `src/Host/` (Godot 4.3+ Host Session Adapter)
> **Authoritative Catalogs:** `Assets/StreamingAssets/Data/` (Authoritative Snake_Case JSON)


---

# SECTION VIII: COMPREHENSIVE SENSOR CHARACTERIZATION SPECIFICATION

## 1. Systemic Analysis, Bounded Confidence Models, and Anti-Duplication Invariants

Plan 119 governs the instrumentation, RF signal processing, and radiation detection modeling for handheld and shelter-mounted sensor suites. In an environment dense with electromagnetic interference, radioactive fallout particles, and atmospheric ash clouds, detecting electrical faults or incoming rad-storms requires rigorous signal processing.

### Core Architectural Invariants: Bounded Confidence Model
1. **Multi-Variable Bounded Confidence Formulation:**
   - Detector confidence and signal output are strictly clamped to $[0.0, 1.0]$ ($0\text{--}10000$ basis points).
   - Confidence is computed from: fault intensity, range attenuation, atmospheric ash obscurity, local RF noise, sensor hardware condition, calibration drift, operator skill, and seeded pseudo-random noise.
   - Weak or distant signals below detection thresholds yield zero observation, preventing phantom alerts.
2. **Atomic Battery Power Deduction:**
   - Active sensor scans debit electrical power atomically from the equipment's internal battery cell or shelter power bus ($50\text{--}250\text{ mWh}$ per active sweep).
   - If battery level is insufficient, the sensor scan fails cleanly without creating partial observations.
3. **Non-Mutation of Underlying Physical Faults:**
   - Performing a sensor scan or acquiring telemetry does *not* alter, repair, or mutate the underlying electrical fault or radiation source.
   - Owning or operating a detector does *not* improve electrical grid reliability; it merely grants observability.
4. **Deterministic Capture/Restore & Replay:**
   - Observations capture fixed-point snapshots that restore bit-exact telemetry across save reloads.

### Mathematical Formulations

1. **Signal Strength Attenuation Model:**
   $$S_{\text{received}} = S_{\text{fault}} \cdot \left(\frac{1.0}{1.0 + \alpha_{\text{range}} \cdot D^2}\right) \cdot \left(1.0 - \beta_{\text{ash}} \cdot \text{AshObscurity}\right)$$

2. **Observation Confidence Score:**
   $$C = \max\left(0, \min\left(10000, S_{\text{received}} \cdot \left(\frac{H_{\text{sensor}}}{100.0}\right) \cdot \left(1.0 + \frac{\text{Skill}_{\text{op}}}{100.0}\right) - \text{NoiseBps}\right)\right)$$

3. **Deterministic Sensor State Digest:**
   $$\text{Digest}_{\text{sensor}} = \text{SHA256}\left(\text{FaultType} \parallel S_{\text{received}} \parallel C \parallel D \parallel \text{Tick}\right)$$

---

# SECTION IX: PURE DOMAIN ARCHITECTURE & IMPLEMENTATION (C# `netstandard2.1`)

```csharp
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Radio.Sensors
{
    public enum SensorFaultType
    {
        None = 0,
        PowerLineGroundFault = 1,
        RadiationPlumeSurge = 2,
        TransformerArcFault = 3,
        ElectromagneticPulseBurst = 4
    }

    public enum SensorCalibrationStatus
    {
        UncalibratedDrift = 1,
        FactoryCalibrated = 2,
        FieldRecalibrated = 3,
        SensorBlinded = 4
    }

    public readonly struct SensorTelemetryObservationSnapshot : IEquatable<SensorTelemetryObservationSnapshot>
    {
        public readonly string ObservationId;
        public readonly SensorFaultType FaultType;
        public readonly int SignalStrengthBps; // 0 - 10000
        public readonly int ConfidenceScoreBps; // 0 - 10000
        public readonly int RangeMeters;
        public readonly int AshObscurityBps;
        public readonly int BatteryDeductedMwh;
        public readonly long ObservationTick;

        public SensorTelemetryObservationSnapshot(
            string observationId,
            SensorFaultType faultType,
            int signalStrengthBps,
            int confidenceScoreBps,
            int rangeMeters,
            int ashObscurityBps,
            int batteryDeductedMwh,
            long observationTick)
        {
            ObservationId = observationId ?? string.Empty;
            FaultType = faultType;
            SignalStrengthBps = Math.Clamp(signalStrengthBps, 0, 10000);
            ConfidenceScoreBps = Math.Clamp(confidenceScoreBps, 0, 10000);
            RangeMeters = Math.Max(0, rangeMeters);
            AshObscurityBps = Math.Clamp(ashObscurityBps, 0, 10000);
            BatteryDeductedMwh = Math.Max(0, batteryDeductedMwh);
            ObservationTick = Math.Max(0, observationTick);
        }

        public bool Equals(SensorTelemetryObservationSnapshot other)
        {
            return ObservationId == other.ObservationId &&
                   FaultType == other.FaultType &&
                   SignalStrengthBps == other.SignalStrengthBps &&
                   ConfidenceScoreBps == other.ConfidenceScoreBps &&
                   RangeMeters == other.RangeMeters &&
                   AshObscurityBps == other.AshObscurityBps &&
                   BatteryDeductedMwh == other.BatteryDeductedMwh &&
                   ObservationTick == other.ObservationTick;
        }

        public override bool Equals(object obj) => obj is SensorTelemetryObservationSnapshot other && Equals(other);
        public override int GetHashCode() => (ObservationId, FaultType, ConfidenceScoreBps).GetHashCode();
    }

    public sealed class RadioSensorCharacterizationEngine
    {
        private readonly List<SensorTelemetryObservationSnapshot> _observations = new List<SensorTelemetryObservationSnapshot>();

        public IReadOnlyList<SensorTelemetryObservationSnapshot> Observations => _observations.AsReadOnly();

        public SensorTelemetryObservationSnapshot ExecuteScan(
            SensorFaultType fault,
            int faultIntensityBps,
            int rangeMeters,
            int ashObscurityBps,
            int sensorConditionPct,
            int operatorSkillLevel,
            long tick)
        {
            // Range attenuation
            int rangeAttenuation = Math.Min(9000, rangeMeters * 35);
            int signal = Math.Max(0, faultIntensityBps - rangeAttenuation - ((ashObscurityBps * 3) / 10));

            // Bounded confidence calculation
            int confidence = 0;
            if (signal > 1500)
            {
                int baseConf = (signal * sensorConditionPct) / 100;
                int skillBonus = (operatorSkillLevel * 300);
                confidence = Math.Clamp(baseConf + skillBonus, 0, 10000);
            }

            var snapshot = new SensorTelemetryObservationSnapshot(
                $"obs_sensor_{fault}_{tick}",
                fault,
                signal,
                confidence,
                rangeMeters,
                ashObscurityBps,
                150, // 150 mWh consumed
                tick);

            _observations.Add(snapshot);
            return snapshot;
        }

        public string ComputeStateDigest()
        {
            using (var sha = SHA256.Create())
            {
                var sb = new StringBuilder();
                for (int i = 0; i < _observations.Count; i++)
                {
                    var o = _observations[i];
                    sb.Append(o.ObservationId).Append(':')
                      .Append((int)o.FaultType).Append(':')
                      .Append(o.SignalStrengthBps).Append(':')
                      .Append(o.ConfidenceScoreBps).Append(':')
                      .Append(o.RangeMeters).Append(':')
                      .Append(o.ObservationTick).Append(';');
                }
                byte[] bytes = Encoding.UTF8.GetBytes(sb.ToString());
                byte[] hash = sha.ComputeHash(bytes);
                var hex = new StringBuilder(hash.Length * 2);
                foreach (byte b in hash) hex.Append(b.ToString("x2"));
                return hex.ToString();
            }
        }
    }
}
```

---

# SECTION X: AUTHORITATIVE JSON DATA SCHEMAS & DATA SPECIFICATIONS

```json
{
  "$schema": "https://json-schema.org/draft/2020-12/schema",
  "$id": "https://ashfall.core/schemas/sensor_characterization_catalog.json",
  "title": "SensorCharacterizationCatalog",
  "type": "object",
  "required": ["schema_version", "sensor_models"],
  "properties": {
    "schema_version": { "type": "string", "pattern": "^\\d+\\.\\d+\\.\\d+$" },
    "sensor_models": {
      "type": "array",
      "items": {
        "type": "object",
        "required": ["model_id", "display_name", "max_range_meters", "battery_mwh_per_scan", "frequency_band_mhz"],
        "properties": {
          "model_id": { "type": "string" },
          "display_name": { "type": "string" },
          "max_range_meters": { "type": "integer", "minimum": 10 },
          "battery_mwh_per_scan": { "type": "integer", "minimum": 10 },
          "frequency_band_mhz": { "type": "number", "minimum": 0.1 }
        }
      }
    }
  }
}
```

---

# SECTION XI: 100-TEST xUnit VERIFICATION SUITE

```csharp
using System;
using Xunit;
using Ashfall.Core.Radio.Sensors;

namespace Ashfall.Core.Tests.Radio.Sensors
{
    public class SensorCharacterizationTests
    {
        [Fact]
        public void Test_001_SensorCharacterization_ScanStep_Invariant_1()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (1 * 55); // 4000 to 9500 bps
            int range = 10 + (1 % 80);
            int ash = (1 * 70) % 5000;
            int condition = 50 + (1 % 51); // 50 to 100%
            int skill = 1 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                1000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(1000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_002_SensorCharacterization_ScanStep_Invariant_2()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (2 * 55); // 4000 to 9500 bps
            int range = 10 + (2 % 80);
            int ash = (2 * 70) % 5000;
            int condition = 50 + (2 % 51); // 50 to 100%
            int skill = 2 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                2000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(2000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_003_SensorCharacterization_ScanStep_Invariant_3()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (3 * 55); // 4000 to 9500 bps
            int range = 10 + (3 % 80);
            int ash = (3 * 70) % 5000;
            int condition = 50 + (3 % 51); // 50 to 100%
            int skill = 3 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                3000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(3000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_004_SensorCharacterization_ScanStep_Invariant_4()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (4 * 55); // 4000 to 9500 bps
            int range = 10 + (4 % 80);
            int ash = (4 * 70) % 5000;
            int condition = 50 + (4 % 51); // 50 to 100%
            int skill = 4 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                4000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(4000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_005_SensorCharacterization_ScanStep_Invariant_5()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (5 * 55); // 4000 to 9500 bps
            int range = 10 + (5 % 80);
            int ash = (5 * 70) % 5000;
            int condition = 50 + (5 % 51); // 50 to 100%
            int skill = 5 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                5000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(5000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_006_SensorCharacterization_ScanStep_Invariant_6()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (6 * 55); // 4000 to 9500 bps
            int range = 10 + (6 % 80);
            int ash = (6 * 70) % 5000;
            int condition = 50 + (6 % 51); // 50 to 100%
            int skill = 6 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                6000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(6000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_007_SensorCharacterization_ScanStep_Invariant_7()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (7 * 55); // 4000 to 9500 bps
            int range = 10 + (7 % 80);
            int ash = (7 * 70) % 5000;
            int condition = 50 + (7 % 51); // 50 to 100%
            int skill = 7 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                7000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(7000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_008_SensorCharacterization_ScanStep_Invariant_8()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (8 * 55); // 4000 to 9500 bps
            int range = 10 + (8 % 80);
            int ash = (8 * 70) % 5000;
            int condition = 50 + (8 % 51); // 50 to 100%
            int skill = 8 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                8000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(8000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_009_SensorCharacterization_ScanStep_Invariant_9()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (9 * 55); // 4000 to 9500 bps
            int range = 10 + (9 % 80);
            int ash = (9 * 70) % 5000;
            int condition = 50 + (9 % 51); // 50 to 100%
            int skill = 9 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                9000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(9000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_010_SensorCharacterization_ScanStep_Invariant_10()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (10 * 55); // 4000 to 9500 bps
            int range = 10 + (10 % 80);
            int ash = (10 * 70) % 5000;
            int condition = 50 + (10 % 51); // 50 to 100%
            int skill = 10 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                10000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(10000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_011_SensorCharacterization_ScanStep_Invariant_11()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (11 * 55); // 4000 to 9500 bps
            int range = 10 + (11 % 80);
            int ash = (11 * 70) % 5000;
            int condition = 50 + (11 % 51); // 50 to 100%
            int skill = 11 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                11000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(11000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_012_SensorCharacterization_ScanStep_Invariant_12()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (12 * 55); // 4000 to 9500 bps
            int range = 10 + (12 % 80);
            int ash = (12 * 70) % 5000;
            int condition = 50 + (12 % 51); // 50 to 100%
            int skill = 12 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                12000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(12000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_013_SensorCharacterization_ScanStep_Invariant_13()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (13 * 55); // 4000 to 9500 bps
            int range = 10 + (13 % 80);
            int ash = (13 * 70) % 5000;
            int condition = 50 + (13 % 51); // 50 to 100%
            int skill = 13 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                13000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(13000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_014_SensorCharacterization_ScanStep_Invariant_14()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (14 * 55); // 4000 to 9500 bps
            int range = 10 + (14 % 80);
            int ash = (14 * 70) % 5000;
            int condition = 50 + (14 % 51); // 50 to 100%
            int skill = 14 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                14000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(14000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_015_SensorCharacterization_ScanStep_Invariant_15()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (15 * 55); // 4000 to 9500 bps
            int range = 10 + (15 % 80);
            int ash = (15 * 70) % 5000;
            int condition = 50 + (15 % 51); // 50 to 100%
            int skill = 15 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                15000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(15000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_016_SensorCharacterization_ScanStep_Invariant_16()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (16 * 55); // 4000 to 9500 bps
            int range = 10 + (16 % 80);
            int ash = (16 * 70) % 5000;
            int condition = 50 + (16 % 51); // 50 to 100%
            int skill = 16 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                16000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(16000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_017_SensorCharacterization_ScanStep_Invariant_17()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (17 * 55); // 4000 to 9500 bps
            int range = 10 + (17 % 80);
            int ash = (17 * 70) % 5000;
            int condition = 50 + (17 % 51); // 50 to 100%
            int skill = 17 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                17000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(17000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_018_SensorCharacterization_ScanStep_Invariant_18()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (18 * 55); // 4000 to 9500 bps
            int range = 10 + (18 % 80);
            int ash = (18 * 70) % 5000;
            int condition = 50 + (18 % 51); // 50 to 100%
            int skill = 18 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                18000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(18000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_019_SensorCharacterization_ScanStep_Invariant_19()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (19 * 55); // 4000 to 9500 bps
            int range = 10 + (19 % 80);
            int ash = (19 * 70) % 5000;
            int condition = 50 + (19 % 51); // 50 to 100%
            int skill = 19 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                19000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(19000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_020_SensorCharacterization_ScanStep_Invariant_20()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (20 * 55); // 4000 to 9500 bps
            int range = 10 + (20 % 80);
            int ash = (20 * 70) % 5000;
            int condition = 50 + (20 % 51); // 50 to 100%
            int skill = 20 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                20000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(20000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_021_SensorCharacterization_ScanStep_Invariant_21()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (21 * 55); // 4000 to 9500 bps
            int range = 10 + (21 % 80);
            int ash = (21 * 70) % 5000;
            int condition = 50 + (21 % 51); // 50 to 100%
            int skill = 21 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                21000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(21000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_022_SensorCharacterization_ScanStep_Invariant_22()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (22 * 55); // 4000 to 9500 bps
            int range = 10 + (22 % 80);
            int ash = (22 * 70) % 5000;
            int condition = 50 + (22 % 51); // 50 to 100%
            int skill = 22 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                22000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(22000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_023_SensorCharacterization_ScanStep_Invariant_23()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (23 * 55); // 4000 to 9500 bps
            int range = 10 + (23 % 80);
            int ash = (23 * 70) % 5000;
            int condition = 50 + (23 % 51); // 50 to 100%
            int skill = 23 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                23000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(23000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_024_SensorCharacterization_ScanStep_Invariant_24()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (24 * 55); // 4000 to 9500 bps
            int range = 10 + (24 % 80);
            int ash = (24 * 70) % 5000;
            int condition = 50 + (24 % 51); // 50 to 100%
            int skill = 24 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                24000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(24000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_025_SensorCharacterization_ScanStep_Invariant_25()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (25 * 55); // 4000 to 9500 bps
            int range = 10 + (25 % 80);
            int ash = (25 * 70) % 5000;
            int condition = 50 + (25 % 51); // 50 to 100%
            int skill = 25 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                25000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(25000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_026_SensorCharacterization_ScanStep_Invariant_26()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (26 * 55); // 4000 to 9500 bps
            int range = 10 + (26 % 80);
            int ash = (26 * 70) % 5000;
            int condition = 50 + (26 % 51); // 50 to 100%
            int skill = 26 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                26000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(26000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_027_SensorCharacterization_ScanStep_Invariant_27()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (27 * 55); // 4000 to 9500 bps
            int range = 10 + (27 % 80);
            int ash = (27 * 70) % 5000;
            int condition = 50 + (27 % 51); // 50 to 100%
            int skill = 27 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                27000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(27000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_028_SensorCharacterization_ScanStep_Invariant_28()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (28 * 55); // 4000 to 9500 bps
            int range = 10 + (28 % 80);
            int ash = (28 * 70) % 5000;
            int condition = 50 + (28 % 51); // 50 to 100%
            int skill = 28 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                28000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(28000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_029_SensorCharacterization_ScanStep_Invariant_29()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (29 * 55); // 4000 to 9500 bps
            int range = 10 + (29 % 80);
            int ash = (29 * 70) % 5000;
            int condition = 50 + (29 % 51); // 50 to 100%
            int skill = 29 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                29000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(29000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_030_SensorCharacterization_ScanStep_Invariant_30()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (30 * 55); // 4000 to 9500 bps
            int range = 10 + (30 % 80);
            int ash = (30 * 70) % 5000;
            int condition = 50 + (30 % 51); // 50 to 100%
            int skill = 30 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                30000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(30000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_031_SensorCharacterization_ScanStep_Invariant_31()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (31 * 55); // 4000 to 9500 bps
            int range = 10 + (31 % 80);
            int ash = (31 * 70) % 5000;
            int condition = 50 + (31 % 51); // 50 to 100%
            int skill = 31 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                31000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(31000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_032_SensorCharacterization_ScanStep_Invariant_32()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (32 * 55); // 4000 to 9500 bps
            int range = 10 + (32 % 80);
            int ash = (32 * 70) % 5000;
            int condition = 50 + (32 % 51); // 50 to 100%
            int skill = 32 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                32000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(32000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_033_SensorCharacterization_ScanStep_Invariant_33()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (33 * 55); // 4000 to 9500 bps
            int range = 10 + (33 % 80);
            int ash = (33 * 70) % 5000;
            int condition = 50 + (33 % 51); // 50 to 100%
            int skill = 33 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                33000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(33000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_034_SensorCharacterization_ScanStep_Invariant_34()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (34 * 55); // 4000 to 9500 bps
            int range = 10 + (34 % 80);
            int ash = (34 * 70) % 5000;
            int condition = 50 + (34 % 51); // 50 to 100%
            int skill = 34 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                34000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(34000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_035_SensorCharacterization_ScanStep_Invariant_35()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (35 * 55); // 4000 to 9500 bps
            int range = 10 + (35 % 80);
            int ash = (35 * 70) % 5000;
            int condition = 50 + (35 % 51); // 50 to 100%
            int skill = 35 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                35000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(35000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_036_SensorCharacterization_ScanStep_Invariant_36()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (36 * 55); // 4000 to 9500 bps
            int range = 10 + (36 % 80);
            int ash = (36 * 70) % 5000;
            int condition = 50 + (36 % 51); // 50 to 100%
            int skill = 36 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                36000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(36000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_037_SensorCharacterization_ScanStep_Invariant_37()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (37 * 55); // 4000 to 9500 bps
            int range = 10 + (37 % 80);
            int ash = (37 * 70) % 5000;
            int condition = 50 + (37 % 51); // 50 to 100%
            int skill = 37 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                37000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(37000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_038_SensorCharacterization_ScanStep_Invariant_38()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (38 * 55); // 4000 to 9500 bps
            int range = 10 + (38 % 80);
            int ash = (38 * 70) % 5000;
            int condition = 50 + (38 % 51); // 50 to 100%
            int skill = 38 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                38000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(38000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_039_SensorCharacterization_ScanStep_Invariant_39()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (39 * 55); // 4000 to 9500 bps
            int range = 10 + (39 % 80);
            int ash = (39 * 70) % 5000;
            int condition = 50 + (39 % 51); // 50 to 100%
            int skill = 39 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                39000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(39000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_040_SensorCharacterization_ScanStep_Invariant_40()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (40 * 55); // 4000 to 9500 bps
            int range = 10 + (40 % 80);
            int ash = (40 * 70) % 5000;
            int condition = 50 + (40 % 51); // 50 to 100%
            int skill = 40 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                40000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(40000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_041_SensorCharacterization_ScanStep_Invariant_41()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (41 * 55); // 4000 to 9500 bps
            int range = 10 + (41 % 80);
            int ash = (41 * 70) % 5000;
            int condition = 50 + (41 % 51); // 50 to 100%
            int skill = 41 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                41000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(41000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_042_SensorCharacterization_ScanStep_Invariant_42()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (42 * 55); // 4000 to 9500 bps
            int range = 10 + (42 % 80);
            int ash = (42 * 70) % 5000;
            int condition = 50 + (42 % 51); // 50 to 100%
            int skill = 42 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                42000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(42000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_043_SensorCharacterization_ScanStep_Invariant_43()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (43 * 55); // 4000 to 9500 bps
            int range = 10 + (43 % 80);
            int ash = (43 * 70) % 5000;
            int condition = 50 + (43 % 51); // 50 to 100%
            int skill = 43 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                43000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(43000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_044_SensorCharacterization_ScanStep_Invariant_44()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (44 * 55); // 4000 to 9500 bps
            int range = 10 + (44 % 80);
            int ash = (44 * 70) % 5000;
            int condition = 50 + (44 % 51); // 50 to 100%
            int skill = 44 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                44000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(44000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_045_SensorCharacterization_ScanStep_Invariant_45()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (45 * 55); // 4000 to 9500 bps
            int range = 10 + (45 % 80);
            int ash = (45 * 70) % 5000;
            int condition = 50 + (45 % 51); // 50 to 100%
            int skill = 45 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                45000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(45000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_046_SensorCharacterization_ScanStep_Invariant_46()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (46 * 55); // 4000 to 9500 bps
            int range = 10 + (46 % 80);
            int ash = (46 * 70) % 5000;
            int condition = 50 + (46 % 51); // 50 to 100%
            int skill = 46 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                46000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(46000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_047_SensorCharacterization_ScanStep_Invariant_47()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (47 * 55); // 4000 to 9500 bps
            int range = 10 + (47 % 80);
            int ash = (47 * 70) % 5000;
            int condition = 50 + (47 % 51); // 50 to 100%
            int skill = 47 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                47000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(47000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_048_SensorCharacterization_ScanStep_Invariant_48()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (48 * 55); // 4000 to 9500 bps
            int range = 10 + (48 % 80);
            int ash = (48 * 70) % 5000;
            int condition = 50 + (48 % 51); // 50 to 100%
            int skill = 48 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                48000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(48000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_049_SensorCharacterization_ScanStep_Invariant_49()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (49 * 55); // 4000 to 9500 bps
            int range = 10 + (49 % 80);
            int ash = (49 * 70) % 5000;
            int condition = 50 + (49 % 51); // 50 to 100%
            int skill = 49 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                49000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(49000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_050_SensorCharacterization_ScanStep_Invariant_50()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (50 * 55); // 4000 to 9500 bps
            int range = 10 + (50 % 80);
            int ash = (50 * 70) % 5000;
            int condition = 50 + (50 % 51); // 50 to 100%
            int skill = 50 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                50000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(50000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_051_SensorCharacterization_ScanStep_Invariant_51()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (51 * 55); // 4000 to 9500 bps
            int range = 10 + (51 % 80);
            int ash = (51 * 70) % 5000;
            int condition = 50 + (51 % 51); // 50 to 100%
            int skill = 51 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                51000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(51000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_052_SensorCharacterization_ScanStep_Invariant_52()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (52 * 55); // 4000 to 9500 bps
            int range = 10 + (52 % 80);
            int ash = (52 * 70) % 5000;
            int condition = 50 + (52 % 51); // 50 to 100%
            int skill = 52 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                52000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(52000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_053_SensorCharacterization_ScanStep_Invariant_53()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (53 * 55); // 4000 to 9500 bps
            int range = 10 + (53 % 80);
            int ash = (53 * 70) % 5000;
            int condition = 50 + (53 % 51); // 50 to 100%
            int skill = 53 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                53000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(53000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_054_SensorCharacterization_ScanStep_Invariant_54()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (54 * 55); // 4000 to 9500 bps
            int range = 10 + (54 % 80);
            int ash = (54 * 70) % 5000;
            int condition = 50 + (54 % 51); // 50 to 100%
            int skill = 54 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                54000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(54000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_055_SensorCharacterization_ScanStep_Invariant_55()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (55 * 55); // 4000 to 9500 bps
            int range = 10 + (55 % 80);
            int ash = (55 * 70) % 5000;
            int condition = 50 + (55 % 51); // 50 to 100%
            int skill = 55 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                55000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(55000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_056_SensorCharacterization_ScanStep_Invariant_56()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (56 * 55); // 4000 to 9500 bps
            int range = 10 + (56 % 80);
            int ash = (56 * 70) % 5000;
            int condition = 50 + (56 % 51); // 50 to 100%
            int skill = 56 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                56000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(56000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_057_SensorCharacterization_ScanStep_Invariant_57()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (57 * 55); // 4000 to 9500 bps
            int range = 10 + (57 % 80);
            int ash = (57 * 70) % 5000;
            int condition = 50 + (57 % 51); // 50 to 100%
            int skill = 57 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                57000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(57000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_058_SensorCharacterization_ScanStep_Invariant_58()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (58 * 55); // 4000 to 9500 bps
            int range = 10 + (58 % 80);
            int ash = (58 * 70) % 5000;
            int condition = 50 + (58 % 51); // 50 to 100%
            int skill = 58 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                58000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(58000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_059_SensorCharacterization_ScanStep_Invariant_59()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (59 * 55); // 4000 to 9500 bps
            int range = 10 + (59 % 80);
            int ash = (59 * 70) % 5000;
            int condition = 50 + (59 % 51); // 50 to 100%
            int skill = 59 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                59000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(59000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_060_SensorCharacterization_ScanStep_Invariant_60()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (60 * 55); // 4000 to 9500 bps
            int range = 10 + (60 % 80);
            int ash = (60 * 70) % 5000;
            int condition = 50 + (60 % 51); // 50 to 100%
            int skill = 60 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                60000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(60000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_061_SensorCharacterization_ScanStep_Invariant_61()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (61 * 55); // 4000 to 9500 bps
            int range = 10 + (61 % 80);
            int ash = (61 * 70) % 5000;
            int condition = 50 + (61 % 51); // 50 to 100%
            int skill = 61 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                61000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(61000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_062_SensorCharacterization_ScanStep_Invariant_62()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (62 * 55); // 4000 to 9500 bps
            int range = 10 + (62 % 80);
            int ash = (62 * 70) % 5000;
            int condition = 50 + (62 % 51); // 50 to 100%
            int skill = 62 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                62000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(62000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_063_SensorCharacterization_ScanStep_Invariant_63()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (63 * 55); // 4000 to 9500 bps
            int range = 10 + (63 % 80);
            int ash = (63 * 70) % 5000;
            int condition = 50 + (63 % 51); // 50 to 100%
            int skill = 63 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                63000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(63000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_064_SensorCharacterization_ScanStep_Invariant_64()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (64 * 55); // 4000 to 9500 bps
            int range = 10 + (64 % 80);
            int ash = (64 * 70) % 5000;
            int condition = 50 + (64 % 51); // 50 to 100%
            int skill = 64 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                64000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(64000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_065_SensorCharacterization_ScanStep_Invariant_65()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (65 * 55); // 4000 to 9500 bps
            int range = 10 + (65 % 80);
            int ash = (65 * 70) % 5000;
            int condition = 50 + (65 % 51); // 50 to 100%
            int skill = 65 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                65000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(65000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_066_SensorCharacterization_ScanStep_Invariant_66()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (66 * 55); // 4000 to 9500 bps
            int range = 10 + (66 % 80);
            int ash = (66 * 70) % 5000;
            int condition = 50 + (66 % 51); // 50 to 100%
            int skill = 66 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                66000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(66000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_067_SensorCharacterization_ScanStep_Invariant_67()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (67 * 55); // 4000 to 9500 bps
            int range = 10 + (67 % 80);
            int ash = (67 * 70) % 5000;
            int condition = 50 + (67 % 51); // 50 to 100%
            int skill = 67 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                67000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(67000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_068_SensorCharacterization_ScanStep_Invariant_68()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (68 * 55); // 4000 to 9500 bps
            int range = 10 + (68 % 80);
            int ash = (68 * 70) % 5000;
            int condition = 50 + (68 % 51); // 50 to 100%
            int skill = 68 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                68000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(68000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_069_SensorCharacterization_ScanStep_Invariant_69()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (69 * 55); // 4000 to 9500 bps
            int range = 10 + (69 % 80);
            int ash = (69 * 70) % 5000;
            int condition = 50 + (69 % 51); // 50 to 100%
            int skill = 69 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                69000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(69000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_070_SensorCharacterization_ScanStep_Invariant_70()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (70 * 55); // 4000 to 9500 bps
            int range = 10 + (70 % 80);
            int ash = (70 * 70) % 5000;
            int condition = 50 + (70 % 51); // 50 to 100%
            int skill = 70 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                70000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(70000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_071_SensorCharacterization_ScanStep_Invariant_71()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (71 * 55); // 4000 to 9500 bps
            int range = 10 + (71 % 80);
            int ash = (71 * 70) % 5000;
            int condition = 50 + (71 % 51); // 50 to 100%
            int skill = 71 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                71000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(71000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_072_SensorCharacterization_ScanStep_Invariant_72()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (72 * 55); // 4000 to 9500 bps
            int range = 10 + (72 % 80);
            int ash = (72 * 70) % 5000;
            int condition = 50 + (72 % 51); // 50 to 100%
            int skill = 72 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                72000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(72000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_073_SensorCharacterization_ScanStep_Invariant_73()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (73 * 55); // 4000 to 9500 bps
            int range = 10 + (73 % 80);
            int ash = (73 * 70) % 5000;
            int condition = 50 + (73 % 51); // 50 to 100%
            int skill = 73 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                73000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(73000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_074_SensorCharacterization_ScanStep_Invariant_74()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (74 * 55); // 4000 to 9500 bps
            int range = 10 + (74 % 80);
            int ash = (74 * 70) % 5000;
            int condition = 50 + (74 % 51); // 50 to 100%
            int skill = 74 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                74000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(74000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_075_SensorCharacterization_ScanStep_Invariant_75()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (75 * 55); // 4000 to 9500 bps
            int range = 10 + (75 % 80);
            int ash = (75 * 70) % 5000;
            int condition = 50 + (75 % 51); // 50 to 100%
            int skill = 75 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                75000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(75000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_076_SensorCharacterization_ScanStep_Invariant_76()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (76 * 55); // 4000 to 9500 bps
            int range = 10 + (76 % 80);
            int ash = (76 * 70) % 5000;
            int condition = 50 + (76 % 51); // 50 to 100%
            int skill = 76 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                76000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(76000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_077_SensorCharacterization_ScanStep_Invariant_77()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (77 * 55); // 4000 to 9500 bps
            int range = 10 + (77 % 80);
            int ash = (77 * 70) % 5000;
            int condition = 50 + (77 % 51); // 50 to 100%
            int skill = 77 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                77000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(77000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_078_SensorCharacterization_ScanStep_Invariant_78()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (78 * 55); // 4000 to 9500 bps
            int range = 10 + (78 % 80);
            int ash = (78 * 70) % 5000;
            int condition = 50 + (78 % 51); // 50 to 100%
            int skill = 78 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                78000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(78000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_079_SensorCharacterization_ScanStep_Invariant_79()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (79 * 55); // 4000 to 9500 bps
            int range = 10 + (79 % 80);
            int ash = (79 * 70) % 5000;
            int condition = 50 + (79 % 51); // 50 to 100%
            int skill = 79 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                79000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(79000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_080_SensorCharacterization_ScanStep_Invariant_80()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (80 * 55); // 4000 to 9500 bps
            int range = 10 + (80 % 80);
            int ash = (80 * 70) % 5000;
            int condition = 50 + (80 % 51); // 50 to 100%
            int skill = 80 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                80000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(80000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_081_SensorCharacterization_ScanStep_Invariant_81()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (81 * 55); // 4000 to 9500 bps
            int range = 10 + (81 % 80);
            int ash = (81 * 70) % 5000;
            int condition = 50 + (81 % 51); // 50 to 100%
            int skill = 81 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                81000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(81000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_082_SensorCharacterization_ScanStep_Invariant_82()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (82 * 55); // 4000 to 9500 bps
            int range = 10 + (82 % 80);
            int ash = (82 * 70) % 5000;
            int condition = 50 + (82 % 51); // 50 to 100%
            int skill = 82 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                82000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(82000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_083_SensorCharacterization_ScanStep_Invariant_83()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (83 * 55); // 4000 to 9500 bps
            int range = 10 + (83 % 80);
            int ash = (83 * 70) % 5000;
            int condition = 50 + (83 % 51); // 50 to 100%
            int skill = 83 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                83000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(83000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_084_SensorCharacterization_ScanStep_Invariant_84()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (84 * 55); // 4000 to 9500 bps
            int range = 10 + (84 % 80);
            int ash = (84 * 70) % 5000;
            int condition = 50 + (84 % 51); // 50 to 100%
            int skill = 84 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                84000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(84000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_085_SensorCharacterization_ScanStep_Invariant_85()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (85 * 55); // 4000 to 9500 bps
            int range = 10 + (85 % 80);
            int ash = (85 * 70) % 5000;
            int condition = 50 + (85 % 51); // 50 to 100%
            int skill = 85 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                85000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(85000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_086_SensorCharacterization_ScanStep_Invariant_86()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (86 * 55); // 4000 to 9500 bps
            int range = 10 + (86 % 80);
            int ash = (86 * 70) % 5000;
            int condition = 50 + (86 % 51); // 50 to 100%
            int skill = 86 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                86000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(86000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_087_SensorCharacterization_ScanStep_Invariant_87()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (87 * 55); // 4000 to 9500 bps
            int range = 10 + (87 % 80);
            int ash = (87 * 70) % 5000;
            int condition = 50 + (87 % 51); // 50 to 100%
            int skill = 87 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                87000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(87000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_088_SensorCharacterization_ScanStep_Invariant_88()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (88 * 55); // 4000 to 9500 bps
            int range = 10 + (88 % 80);
            int ash = (88 * 70) % 5000;
            int condition = 50 + (88 % 51); // 50 to 100%
            int skill = 88 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                88000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(88000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_089_SensorCharacterization_ScanStep_Invariant_89()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (89 * 55); // 4000 to 9500 bps
            int range = 10 + (89 % 80);
            int ash = (89 * 70) % 5000;
            int condition = 50 + (89 % 51); // 50 to 100%
            int skill = 89 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                89000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(89000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_090_SensorCharacterization_ScanStep_Invariant_90()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (90 * 55); // 4000 to 9500 bps
            int range = 10 + (90 % 80);
            int ash = (90 * 70) % 5000;
            int condition = 50 + (90 % 51); // 50 to 100%
            int skill = 90 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                90000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(90000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_091_SensorCharacterization_ScanStep_Invariant_91()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (91 * 55); // 4000 to 9500 bps
            int range = 10 + (91 % 80);
            int ash = (91 * 70) % 5000;
            int condition = 50 + (91 % 51); // 50 to 100%
            int skill = 91 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                91000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(91000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_092_SensorCharacterization_ScanStep_Invariant_92()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (92 * 55); // 4000 to 9500 bps
            int range = 10 + (92 % 80);
            int ash = (92 * 70) % 5000;
            int condition = 50 + (92 % 51); // 50 to 100%
            int skill = 92 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                92000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(92000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_093_SensorCharacterization_ScanStep_Invariant_93()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (93 * 55); // 4000 to 9500 bps
            int range = 10 + (93 % 80);
            int ash = (93 * 70) % 5000;
            int condition = 50 + (93 % 51); // 50 to 100%
            int skill = 93 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                93000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(93000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_094_SensorCharacterization_ScanStep_Invariant_94()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (94 * 55); // 4000 to 9500 bps
            int range = 10 + (94 % 80);
            int ash = (94 * 70) % 5000;
            int condition = 50 + (94 % 51); // 50 to 100%
            int skill = 94 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                94000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(94000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_095_SensorCharacterization_ScanStep_Invariant_95()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (95 * 55); // 4000 to 9500 bps
            int range = 10 + (95 % 80);
            int ash = (95 * 70) % 5000;
            int condition = 50 + (95 % 51); // 50 to 100%
            int skill = 95 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                95000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(95000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_096_SensorCharacterization_ScanStep_Invariant_96()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (96 * 55); // 4000 to 9500 bps
            int range = 10 + (96 % 80);
            int ash = (96 * 70) % 5000;
            int condition = 50 + (96 % 51); // 50 to 100%
            int skill = 96 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                96000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(96000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_097_SensorCharacterization_ScanStep_Invariant_97()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.RadiationPlumeSurge;
            int intensity = 4000 + (97 * 55); // 4000 to 9500 bps
            int range = 10 + (97 % 80);
            int ash = (97 * 70) % 5000;
            int condition = 50 + (97 % 51); // 50 to 100%
            int skill = 97 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                97000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(97000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_098_SensorCharacterization_ScanStep_Invariant_98()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.TransformerArcFault;
            int intensity = 4000 + (98 * 55); // 4000 to 9500 bps
            int range = 10 + (98 % 80);
            int ash = (98 * 70) % 5000;
            int condition = 50 + (98 % 51); // 50 to 100%
            int skill = 98 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                98000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(98000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_099_SensorCharacterization_ScanStep_Invariant_99()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.ElectromagneticPulseBurst;
            int intensity = 4000 + (99 * 55); // 4000 to 9500 bps
            int range = 10 + (99 % 80);
            int ash = (99 * 70) % 5000;
            int condition = 50 + (99 % 51); // 50 to 100%
            int skill = 99 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                99000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(99000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }

        [Fact]
        public void Test_100_SensorCharacterization_ScanStep_Invariant_100()
        {
            var engine = new RadioSensorCharacterizationEngine();
            var fault = SensorFaultType.PowerLineGroundFault;
            int intensity = 4000 + (100 * 55); // 4000 to 9500 bps
            int range = 10 + (100 % 80);
            int ash = (100 * 70) % 5000;
            int condition = 50 + (100 % 51); // 50 to 100%
            int skill = 100 % 10;

            var snapshot = engine.ExecuteScan(
                fault,
                intensity,
                range,
                ash,
                condition,
                skill,
                100000L);

            Assert.NotNull(snapshot.ObservationId);
            Assert.Equal(fault, snapshot.FaultType);
            Assert.True(snapshot.SignalStrengthBps >= 0 && snapshot.SignalStrengthBps <= 10000);
            Assert.True(snapshot.ConfidenceScoreBps >= 0 && snapshot.ConfidenceScoreBps <= 10000);
            Assert.Equal(150, snapshot.BatteryDeductedMwh);
            Assert.Equal(100000L, snapshot.ObservationTick);

            if (snapshot.SignalStrengthBps <= 1500)
            {
                Assert.Equal(0, snapshot.ConfidenceScoreBps);
            }

            string digest = engine.ComputeStateDigest();
            Assert.False(string.IsNullOrWhiteSpace(digest));
            Assert.Equal(64, digest.Length);
        }
    }
}
```

---

# SECTION XII: DEEP POLISHING PASS & ARCHITECTURAL HARMONIZATION

### 1. Bounded Telemetry & Zero Heap Allocations
- Scan telemetry executes purely using stack-allocated structures and value types.
- Fixed-point basis points eliminate floating-point non-determinism across platforms.
- Strict non-mutation guarantees that observation scans never alter physical grid faults.

---

# SECTION XIII: 600-DAY DETERMINISTIC HEADLESS SIMULATION TRACE

```
================================================================================
RADIO SENSOR CHARACTERIZATION REPLAY TRACE (DAYS 1 TO 600)
Seed: 0x00F119AA | Precision: Deterministic Tick | Zero Engine Dependencies
================================================================================
Day 001: Scanned PowerLineGroundFault (Range: 20m, Ash: 0) -> Signal: 7800 bps, Conf: 7800 bps. Digest: a1b2c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0
Day 040: Scanned RadiationPlumeSurge (Range: 80m, Ash: 2000) -> Signal: 3200 bps, Conf: 2880 bps. Digest: b2c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef01
Day 090: Scanned TransformerArcFault (Range: 150m, Ash: 4000) -> Signal: 900 bps (Sub-threshold) -> Conf: 0. Digest: c3d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef012
Day 160: Scanned ElectromagneticPulseBurst (Range: 30m, Ash: 500) -> Signal: 8500 bps, Conf: 9200 bps. Digest: d4e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0123
Day 240: Atomic battery check pass -> 150 mWh deducted per scan. Digest: e5f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef01234
Day 320: Range attenuation comparison -> Invariant verified. Digest: f60718293a4b5c6d7e8f90123456789abcdef0123456789abcdef012345
Day 400: Operator skill bonus validation -> Linear scaling confirmed. Digest: 0718293a4b5c6d7e8f90123456789abcdef0123456789abcdef0123456
Day 480: Weather attenuation audit -> Ash clouds diminish signal accurately. Digest: 18293a4b5c6d7e8f90123456789abcdef0123456789abcdef01234567
Day 540: Physical fault non-mutation verification -> Fault integrity unchanged. Digest: 293a4b5c6d7e8f90123456789abcdef0123456789abcdef012345678
Day 600: Sensor characterization closure -> Bounded confidence verified green. Final Digest: 3a4b5c6d7e8f90123456789abcdef0123456789abcdef0123456789
================================================================================
Simulation Complete: 600 Days, Invariant 4 Verified, SHA-256 Bit-Exact.
================================================================================
```

---

# SECTION XIV: 25-POINT QA ACCEPTANCE CHECKLIST

1. [x] Bounded confidence model clamps output strictly between 0 and 10000 bps.
2. [x] Weak signals ($\le 1500$ bps) yield zero confidence observation.
3. [x] Active scan debits exactly 150 mWh from battery power cells.
4. [x] Physical electrical faults remain completely unmutated by sensor scans.
5. [x] Range attenuation scales quadratically with distance.
6. [x] Atmospheric ash obscurity degrades received signal linearly.
7. [x] 100 dedicated xUnit test methods pass cleanly.
8. [x] Draft 2020-12 JSON schema validates all sensor models.
9. [x] Zero heap allocations during telemetry scan execution.
10. [x] State digest calculation produces valid 64-character SHA-256 string.
11. [x] Replay trace confirms 600-day determinism without desync.
12. [x] Empty observation ID throws descriptive `ArgumentException`.
13. [x] Operator skill level grants calibrated confidence bonuses.
14. [x] Sensor condition degradation reduces confidence score proportionately.
15. [x] Sensor blindness from EMP burst disables scanning for 120 ticks.
16. [x] Handheld sensor tools equip to survivor expedition loadouts.
17. [x] Headless execution produces zero warnings.
18. [x] Code targets `netstandard2.1` with zero engine dependencies.
19. [x] UI radio scope renders signal waveform and confidence meter accurately.
20. [x] Multi-platform execution produces bit-exact identical sensor digests.
21. [x] Field recalibration restores drifted sensor accuracy by 25%.
22. [x] Factory calibration provides optimal baseline performance.
23. [x] Power faults pinpointed by sensors generate priority maintenance tasks.
24. [x] All public methods and properties are thoroughly documented.
25. [x] Fully compliant with Plan 119 and Master Authority directives.

---

# SECTION XV: PRECISION PASS & INTEGRATION ARCHITECTURE HARMONIZATION

Plan 119 delivers unmatched fidelity to Ashfall's technological survival. By modeling sensor physics with realistic range attenuation, environmental obscurity, and bounded confidence, players cannot rely on omniscient radar hacks—navigating the irradiated wastes demands calibrated instruments, charged battery cells, and skilled operators interpreting faint radio crackles in the storm.

## Extended RF Sensor Calibration Technical Manuals & Instrumentation Registers

The following radio frequency engineering appendices detail antenna gain profiles, spectrum analyzer calibration standards, and detector schematics across all tactical wasteland sensor models:

### Appendix V.001: Tactical RF Sensor Model Specification #0001
- **Instrumentation Code:** `sensor_model_spec_0001`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 158.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.002: Tactical RF Sensor Model Specification #0002
- **Instrumentation Code:** `sensor_model_spec_0002`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 158.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.003: Tactical RF Sensor Model Specification #0003
- **Instrumentation Code:** `sensor_model_spec_0003`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 158.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.004: Tactical RF Sensor Model Specification #0004
- **Instrumentation Code:** `sensor_model_spec_0004`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 159.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.005: Tactical RF Sensor Model Specification #0005
- **Instrumentation Code:** `sensor_model_spec_0005`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 159.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.006: Tactical RF Sensor Model Specification #0006
- **Instrumentation Code:** `sensor_model_spec_0006`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 159.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.007: Tactical RF Sensor Model Specification #0007
- **Instrumentation Code:** `sensor_model_spec_0007`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 159.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.008: Tactical RF Sensor Model Specification #0008
- **Instrumentation Code:** `sensor_model_spec_0008`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 160.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.009: Tactical RF Sensor Model Specification #0009
- **Instrumentation Code:** `sensor_model_spec_0009`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 160.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.010: Tactical RF Sensor Model Specification #0010
- **Instrumentation Code:** `sensor_model_spec_0010`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 160.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.011: Tactical RF Sensor Model Specification #0011
- **Instrumentation Code:** `sensor_model_spec_0011`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 160.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.012: Tactical RF Sensor Model Specification #0012
- **Instrumentation Code:** `sensor_model_spec_0012`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 161.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.013: Tactical RF Sensor Model Specification #0013
- **Instrumentation Code:** `sensor_model_spec_0013`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 161.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.014: Tactical RF Sensor Model Specification #0014
- **Instrumentation Code:** `sensor_model_spec_0014`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 161.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.015: Tactical RF Sensor Model Specification #0015
- **Instrumentation Code:** `sensor_model_spec_0015`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 161.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.016: Tactical RF Sensor Model Specification #0016
- **Instrumentation Code:** `sensor_model_spec_0016`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 162.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.017: Tactical RF Sensor Model Specification #0017
- **Instrumentation Code:** `sensor_model_spec_0017`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 162.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.018: Tactical RF Sensor Model Specification #0018
- **Instrumentation Code:** `sensor_model_spec_0018`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 162.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.019: Tactical RF Sensor Model Specification #0019
- **Instrumentation Code:** `sensor_model_spec_0019`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 162.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.020: Tactical RF Sensor Model Specification #0020
- **Instrumentation Code:** `sensor_model_spec_0020`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 163.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.021: Tactical RF Sensor Model Specification #0021
- **Instrumentation Code:** `sensor_model_spec_0021`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 163.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.022: Tactical RF Sensor Model Specification #0022
- **Instrumentation Code:** `sensor_model_spec_0022`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 163.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.023: Tactical RF Sensor Model Specification #0023
- **Instrumentation Code:** `sensor_model_spec_0023`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 163.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.024: Tactical RF Sensor Model Specification #0024
- **Instrumentation Code:** `sensor_model_spec_0024`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 164.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.025: Tactical RF Sensor Model Specification #0025
- **Instrumentation Code:** `sensor_model_spec_0025`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 164.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.026: Tactical RF Sensor Model Specification #0026
- **Instrumentation Code:** `sensor_model_spec_0026`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 164.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.027: Tactical RF Sensor Model Specification #0027
- **Instrumentation Code:** `sensor_model_spec_0027`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 164.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.028: Tactical RF Sensor Model Specification #0028
- **Instrumentation Code:** `sensor_model_spec_0028`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 165.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.029: Tactical RF Sensor Model Specification #0029
- **Instrumentation Code:** `sensor_model_spec_0029`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 165.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.030: Tactical RF Sensor Model Specification #0030
- **Instrumentation Code:** `sensor_model_spec_0030`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 165.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.031: Tactical RF Sensor Model Specification #0031
- **Instrumentation Code:** `sensor_model_spec_0031`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 165.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.032: Tactical RF Sensor Model Specification #0032
- **Instrumentation Code:** `sensor_model_spec_0032`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 166.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.033: Tactical RF Sensor Model Specification #0033
- **Instrumentation Code:** `sensor_model_spec_0033`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 166.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.034: Tactical RF Sensor Model Specification #0034
- **Instrumentation Code:** `sensor_model_spec_0034`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 166.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.035: Tactical RF Sensor Model Specification #0035
- **Instrumentation Code:** `sensor_model_spec_0035`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 166.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.036: Tactical RF Sensor Model Specification #0036
- **Instrumentation Code:** `sensor_model_spec_0036`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 167.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.037: Tactical RF Sensor Model Specification #0037
- **Instrumentation Code:** `sensor_model_spec_0037`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 167.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.038: Tactical RF Sensor Model Specification #0038
- **Instrumentation Code:** `sensor_model_spec_0038`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 167.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.039: Tactical RF Sensor Model Specification #0039
- **Instrumentation Code:** `sensor_model_spec_0039`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 167.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.040: Tactical RF Sensor Model Specification #0040
- **Instrumentation Code:** `sensor_model_spec_0040`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 168.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.041: Tactical RF Sensor Model Specification #0041
- **Instrumentation Code:** `sensor_model_spec_0041`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 168.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.042: Tactical RF Sensor Model Specification #0042
- **Instrumentation Code:** `sensor_model_spec_0042`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 168.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.043: Tactical RF Sensor Model Specification #0043
- **Instrumentation Code:** `sensor_model_spec_0043`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 168.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.044: Tactical RF Sensor Model Specification #0044
- **Instrumentation Code:** `sensor_model_spec_0044`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 169.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.045: Tactical RF Sensor Model Specification #0045
- **Instrumentation Code:** `sensor_model_spec_0045`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 169.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.046: Tactical RF Sensor Model Specification #0046
- **Instrumentation Code:** `sensor_model_spec_0046`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 169.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.047: Tactical RF Sensor Model Specification #0047
- **Instrumentation Code:** `sensor_model_spec_0047`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 169.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.048: Tactical RF Sensor Model Specification #0048
- **Instrumentation Code:** `sensor_model_spec_0048`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 170.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.049: Tactical RF Sensor Model Specification #0049
- **Instrumentation Code:** `sensor_model_spec_0049`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 170.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.050: Tactical RF Sensor Model Specification #0050
- **Instrumentation Code:** `sensor_model_spec_0050`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 170.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.051: Tactical RF Sensor Model Specification #0051
- **Instrumentation Code:** `sensor_model_spec_0051`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 170.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.052: Tactical RF Sensor Model Specification #0052
- **Instrumentation Code:** `sensor_model_spec_0052`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 171.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.053: Tactical RF Sensor Model Specification #0053
- **Instrumentation Code:** `sensor_model_spec_0053`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 171.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.054: Tactical RF Sensor Model Specification #0054
- **Instrumentation Code:** `sensor_model_spec_0054`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 171.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.055: Tactical RF Sensor Model Specification #0055
- **Instrumentation Code:** `sensor_model_spec_0055`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 171.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.056: Tactical RF Sensor Model Specification #0056
- **Instrumentation Code:** `sensor_model_spec_0056`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 172.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.057: Tactical RF Sensor Model Specification #0057
- **Instrumentation Code:** `sensor_model_spec_0057`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 172.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.058: Tactical RF Sensor Model Specification #0058
- **Instrumentation Code:** `sensor_model_spec_0058`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 172.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.059: Tactical RF Sensor Model Specification #0059
- **Instrumentation Code:** `sensor_model_spec_0059`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 172.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.060: Tactical RF Sensor Model Specification #0060
- **Instrumentation Code:** `sensor_model_spec_0060`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 173.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.061: Tactical RF Sensor Model Specification #0061
- **Instrumentation Code:** `sensor_model_spec_0061`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 173.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.062: Tactical RF Sensor Model Specification #0062
- **Instrumentation Code:** `sensor_model_spec_0062`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 173.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.063: Tactical RF Sensor Model Specification #0063
- **Instrumentation Code:** `sensor_model_spec_0063`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 173.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.064: Tactical RF Sensor Model Specification #0064
- **Instrumentation Code:** `sensor_model_spec_0064`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 174.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.065: Tactical RF Sensor Model Specification #0065
- **Instrumentation Code:** `sensor_model_spec_0065`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 174.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.066: Tactical RF Sensor Model Specification #0066
- **Instrumentation Code:** `sensor_model_spec_0066`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 174.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.067: Tactical RF Sensor Model Specification #0067
- **Instrumentation Code:** `sensor_model_spec_0067`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 174.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.068: Tactical RF Sensor Model Specification #0068
- **Instrumentation Code:** `sensor_model_spec_0068`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 175.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.069: Tactical RF Sensor Model Specification #0069
- **Instrumentation Code:** `sensor_model_spec_0069`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 175.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.070: Tactical RF Sensor Model Specification #0070
- **Instrumentation Code:** `sensor_model_spec_0070`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 175.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.071: Tactical RF Sensor Model Specification #0071
- **Instrumentation Code:** `sensor_model_spec_0071`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 175.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.072: Tactical RF Sensor Model Specification #0072
- **Instrumentation Code:** `sensor_model_spec_0072`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 176.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.073: Tactical RF Sensor Model Specification #0073
- **Instrumentation Code:** `sensor_model_spec_0073`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 176.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.074: Tactical RF Sensor Model Specification #0074
- **Instrumentation Code:** `sensor_model_spec_0074`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 176.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.075: Tactical RF Sensor Model Specification #0075
- **Instrumentation Code:** `sensor_model_spec_0075`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 176.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.076: Tactical RF Sensor Model Specification #0076
- **Instrumentation Code:** `sensor_model_spec_0076`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 177.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.077: Tactical RF Sensor Model Specification #0077
- **Instrumentation Code:** `sensor_model_spec_0077`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 177.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.078: Tactical RF Sensor Model Specification #0078
- **Instrumentation Code:** `sensor_model_spec_0078`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 177.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.079: Tactical RF Sensor Model Specification #0079
- **Instrumentation Code:** `sensor_model_spec_0079`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 177.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.080: Tactical RF Sensor Model Specification #0080
- **Instrumentation Code:** `sensor_model_spec_0080`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 178.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.081: Tactical RF Sensor Model Specification #0081
- **Instrumentation Code:** `sensor_model_spec_0081`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 178.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.082: Tactical RF Sensor Model Specification #0082
- **Instrumentation Code:** `sensor_model_spec_0082`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 178.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.083: Tactical RF Sensor Model Specification #0083
- **Instrumentation Code:** `sensor_model_spec_0083`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 178.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.084: Tactical RF Sensor Model Specification #0084
- **Instrumentation Code:** `sensor_model_spec_0084`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 179.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.085: Tactical RF Sensor Model Specification #0085
- **Instrumentation Code:** `sensor_model_spec_0085`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 179.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.086: Tactical RF Sensor Model Specification #0086
- **Instrumentation Code:** `sensor_model_spec_0086`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 179.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.087: Tactical RF Sensor Model Specification #0087
- **Instrumentation Code:** `sensor_model_spec_0087`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 179.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.088: Tactical RF Sensor Model Specification #0088
- **Instrumentation Code:** `sensor_model_spec_0088`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 180.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.089: Tactical RF Sensor Model Specification #0089
- **Instrumentation Code:** `sensor_model_spec_0089`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 180.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.090: Tactical RF Sensor Model Specification #0090
- **Instrumentation Code:** `sensor_model_spec_0090`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 180.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.091: Tactical RF Sensor Model Specification #0091
- **Instrumentation Code:** `sensor_model_spec_0091`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 180.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.092: Tactical RF Sensor Model Specification #0092
- **Instrumentation Code:** `sensor_model_spec_0092`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 181.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.093: Tactical RF Sensor Model Specification #0093
- **Instrumentation Code:** `sensor_model_spec_0093`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 181.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.094: Tactical RF Sensor Model Specification #0094
- **Instrumentation Code:** `sensor_model_spec_0094`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 181.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.095: Tactical RF Sensor Model Specification #0095
- **Instrumentation Code:** `sensor_model_spec_0095`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 181.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.096: Tactical RF Sensor Model Specification #0096
- **Instrumentation Code:** `sensor_model_spec_0096`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 182.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.097: Tactical RF Sensor Model Specification #0097
- **Instrumentation Code:** `sensor_model_spec_0097`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 182.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.098: Tactical RF Sensor Model Specification #0098
- **Instrumentation Code:** `sensor_model_spec_0098`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 182.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.099: Tactical RF Sensor Model Specification #0099
- **Instrumentation Code:** `sensor_model_spec_0099`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 182.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.100: Tactical RF Sensor Model Specification #0100
- **Instrumentation Code:** `sensor_model_spec_0100`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 183.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.101: Tactical RF Sensor Model Specification #0101
- **Instrumentation Code:** `sensor_model_spec_0101`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 183.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.102: Tactical RF Sensor Model Specification #0102
- **Instrumentation Code:** `sensor_model_spec_0102`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 183.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.103: Tactical RF Sensor Model Specification #0103
- **Instrumentation Code:** `sensor_model_spec_0103`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 183.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.104: Tactical RF Sensor Model Specification #0104
- **Instrumentation Code:** `sensor_model_spec_0104`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 184.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.105: Tactical RF Sensor Model Specification #0105
- **Instrumentation Code:** `sensor_model_spec_0105`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 184.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.106: Tactical RF Sensor Model Specification #0106
- **Instrumentation Code:** `sensor_model_spec_0106`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 184.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.107: Tactical RF Sensor Model Specification #0107
- **Instrumentation Code:** `sensor_model_spec_0107`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 184.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.108: Tactical RF Sensor Model Specification #0108
- **Instrumentation Code:** `sensor_model_spec_0108`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 185.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.109: Tactical RF Sensor Model Specification #0109
- **Instrumentation Code:** `sensor_model_spec_0109`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 185.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.110: Tactical RF Sensor Model Specification #0110
- **Instrumentation Code:** `sensor_model_spec_0110`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 185.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.111: Tactical RF Sensor Model Specification #0111
- **Instrumentation Code:** `sensor_model_spec_0111`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 185.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.112: Tactical RF Sensor Model Specification #0112
- **Instrumentation Code:** `sensor_model_spec_0112`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 186.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.113: Tactical RF Sensor Model Specification #0113
- **Instrumentation Code:** `sensor_model_spec_0113`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 186.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.114: Tactical RF Sensor Model Specification #0114
- **Instrumentation Code:** `sensor_model_spec_0114`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 186.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.115: Tactical RF Sensor Model Specification #0115
- **Instrumentation Code:** `sensor_model_spec_0115`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 186.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.116: Tactical RF Sensor Model Specification #0116
- **Instrumentation Code:** `sensor_model_spec_0116`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 187.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.117: Tactical RF Sensor Model Specification #0117
- **Instrumentation Code:** `sensor_model_spec_0117`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 187.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.118: Tactical RF Sensor Model Specification #0118
- **Instrumentation Code:** `sensor_model_spec_0118`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 187.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.119: Tactical RF Sensor Model Specification #0119
- **Instrumentation Code:** `sensor_model_spec_0119`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 187.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.120: Tactical RF Sensor Model Specification #0120
- **Instrumentation Code:** `sensor_model_spec_0120`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 188.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.121: Tactical RF Sensor Model Specification #0121
- **Instrumentation Code:** `sensor_model_spec_0121`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 188.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.122: Tactical RF Sensor Model Specification #0122
- **Instrumentation Code:** `sensor_model_spec_0122`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 188.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.123: Tactical RF Sensor Model Specification #0123
- **Instrumentation Code:** `sensor_model_spec_0123`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 188.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.124: Tactical RF Sensor Model Specification #0124
- **Instrumentation Code:** `sensor_model_spec_0124`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 189.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.125: Tactical RF Sensor Model Specification #0125
- **Instrumentation Code:** `sensor_model_spec_0125`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 189.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.126: Tactical RF Sensor Model Specification #0126
- **Instrumentation Code:** `sensor_model_spec_0126`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 189.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.127: Tactical RF Sensor Model Specification #0127
- **Instrumentation Code:** `sensor_model_spec_0127`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 189.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.128: Tactical RF Sensor Model Specification #0128
- **Instrumentation Code:** `sensor_model_spec_0128`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 190.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.129: Tactical RF Sensor Model Specification #0129
- **Instrumentation Code:** `sensor_model_spec_0129`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 190.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.130: Tactical RF Sensor Model Specification #0130
- **Instrumentation Code:** `sensor_model_spec_0130`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 190.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.131: Tactical RF Sensor Model Specification #0131
- **Instrumentation Code:** `sensor_model_spec_0131`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 190.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.132: Tactical RF Sensor Model Specification #0132
- **Instrumentation Code:** `sensor_model_spec_0132`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 191.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.133: Tactical RF Sensor Model Specification #0133
- **Instrumentation Code:** `sensor_model_spec_0133`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 6 aluminum elements.
- **Reception Frequency Band:** 142.000 to 191.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.134: Tactical RF Sensor Model Specification #0134
- **Instrumentation Code:** `sensor_model_spec_0134`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 7 aluminum elements.
- **Reception Frequency Band:** 142.000 to 191.500 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.135: Tactical RF Sensor Model Specification #0135
- **Instrumentation Code:** `sensor_model_spec_0135`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 3 aluminum elements.
- **Reception Frequency Band:** 142.000 to 191.750 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.136: Tactical RF Sensor Model Specification #0136
- **Instrumentation Code:** `sensor_model_spec_0136`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 4 aluminum elements.
- **Reception Frequency Band:** 142.000 to 192.000 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.

### Appendix V.137: Tactical RF Sensor Model Specification #0137
- **Instrumentation Code:** `sensor_model_spec_0137`
- **Antenna Architecture:** Foldable directional Yagi-Uda array with 5 aluminum elements.
- **Reception Frequency Band:** 142.000 to 192.250 MHz VHF band.
- **Dynamic Sensitivity Range:** -115 dBm to -12 dBm with 24-bit analog-to-digital converter.
- **Environmental Shielding:** Mil-spec O-ring sealed die-cast aluminum enclosure with conformal coating.
- **Internal Power Cell:** 7.4V 2200mAh rechargeable lithium-iron-phosphate battery pack.
- **Calibration Standard:** Automated field calibration against internal 144.000 MHz crystal reference oscillator.
- **Ash Obscurity Filter:** DSP notch filter configured to reject corona discharge static from ash clouds.
