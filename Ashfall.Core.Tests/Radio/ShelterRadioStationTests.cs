// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class ShelterRadioStationTests
    {
        private static string GetRadioInterceptCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/radio_intercepts.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/radio_intercepts.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""intercepts"": [
    {
      ""id"": ""radio_intercept_meridian_supply_column_01"",
      ""callsign"": ""MERIDIAN-ACT-7"",
      ""frequency_khz"": 7115,
      ""band"": ""hf"",
      ""signal_class"": ""logistics_chatter"",
      ""source_faction_id"": ""faction_the_compact"",
      ""base_signal_strength"": 0.65,
      ""encryption"": {
        ""scheme"": ""field_cipher"",
        ""difficulty"": 40,
        ""required_skill_ids"": [""skill_signal_ear"", ""skill_cold_analysis""]
      },
      ""triangulation"": {
        ""required_bearings"": 3,
        ""revealed_location_id"": ""loc_diesel_tank_farm""
      },
      ""expiry_days"": 4,
      ""message"": ""Convoy route Bravo compromised by ash drifts."",
      ""tags"": [""military""]
    },
    {
      ""id"": ""radio_intercept_sos_quarry_shelter_02"",
      ""callsign"": ""SHELTER-44-SOS"",
      ""frequency_khz"": 3850,
      ""band"": ""hf"",
      ""signal_class"": ""sos_distress"",
      ""source_faction_id"": ""faction_the_office"",
      ""base_signal_strength"": 0.50,
      ""encryption"": { ""scheme"": ""none"", ""difficulty"": 0, ""required_skill_ids"": [] },
      ""triangulation"": { ""required_bearings"": 2, ""revealed_location_id"": ""loc_recovery_yard"" },
      ""expiry_days"": 3,
      ""message"": ""MAYDAY. Primary air intake collapsed."",
      ""tags"": [""distress""]
    }
  ]
}";
        }

        private static ShelterRadioStationSystem CreateSystem(
            out OrbitalHarrowTelemetrySystem harrow,
            int seed = 42)
        {
            var rng = new SeededRng(seed);
            var armor = new SkyLayerArmorSystem();
            harrow = new OrbitalHarrowTelemetrySystem(armor, rng);

            var station = new ShelterRadioStationSystem(rng, harrow);
            station.LoadCatalog(GetRadioInterceptCatalogJson());
            return station;
        }

        [Fact]
        public void ScanFrequency_LocksWhenTunedCloseToBroadcast()
        {
            var station = CreateSystem(out _);
            station.TuneTo(7115, "hf"); // Exact match for meridian supply

            var result = station.ScanFrequency(1);
            Assert.True(result.FoundSignal);
            Assert.Equal("radio_intercept_meridian_supply_column_01", result.InterceptId);
            Assert.True(result.SignalStrength > 0.40f);

            var progress = station.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
            Assert.True(progress.Detected);
            Assert.True(progress.SignalLockPermille > 0);
        }

        [Fact]
        public void ScanFrequency_MissesWhenDetuned()
        {
            var station = CreateSystem(out _);
            station.TuneTo(9000, "hf"); // Far from any broadcast

            var result = station.ScanFrequency(1);
            Assert.False(result.FoundSignal);
            Assert.Equal("static_noise", result.StatusMessage);
        }

        [Fact]
        public void Decryption_AdvancesWithOperatorSkill()
        {
            var station = CreateSystem(out _);
            station.BindSkillProvider(skillId => skillId == "skill_signal_ear" ? 1.0f : 0.5f);
            station.TuneTo(7115, "hf");
            station.ScanFrequency(1);

            int progress1 = station.ProgressDecryption("radio_intercept_meridian_supply_column_01");
            Assert.True(progress1 > 0);

            var item = station.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
            Assert.True(item.DecryptProgressPermille > 0);
        }

        [Fact]
        public void Triangulation_RequiresDistinctAzimuthsAndUnlocksLocation()
        {
            var station = CreateSystem(out _);
            station.TuneTo(3850, "hf");
            station.ScanFrequency(1);

            // Bearing 1 at 45 deg
            bool unlocked1 = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 45);
            Assert.False(unlocked1);

            // Duplicate bearing near 45 deg (e.g. 50 deg, < 20 deg diff) -> not counted as distinct
            bool unlockedDup = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 50);
            Assert.False(unlockedDup);

            var progress = station.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
            Assert.Equal(1, progress.BearingsCollected);

            // Bearing 2 at 180 deg (distinct >= 20 deg) -> unlocks!
            bool unlocked2 = station.RecordBearing("radio_intercept_sos_quarry_shelter_02", 180);
            Assert.True(unlocked2);
            Assert.Equal(2, progress.BearingsCollected);
            Assert.True(progress.Resolved);
            Assert.Contains("loc_recovery_yard", station.State.discoveredLocationIds);
        }

        [Fact]
        public void SOSDistress_ExpiresWhenDeadlineReached()
        {
            var station = CreateSystem(out _);
            station.TuneTo(3850, "hf");
            station.ScanFrequency(1); // Detected day 1, expires day 1 + 3 = 4

            var progress = station.GetOrCreateInterceptProgress("radio_intercept_sos_quarry_shelter_02");
            Assert.False(progress.IsExpired);

            station.TickDay(2);
            Assert.False(progress.IsExpired);

            station.TickDay(4); // Day 4 >= ExpiresOnDay 4
            Assert.True(progress.IsExpired);
        }

        [Fact]
        public void OrbitalEarlyWarning_RelaysActiveImpactWarning()
        {
            var station = CreateSystem(out var harrow);
            harrow.ActivateTelemetry(1);

            // Simulate harrow warning
            harrow.State.warnings.Add(new OrbitalWarningEntry
            {
                day = 2,
                targetGridX = 3,
                energyMj = 25f,
                eventId = "harrow_strike_alpha",
                telemetryText = "Incoming rod descent",
                severity = "Critical"
            });
            harrow.State.nextImpactDay = 4;

            var relayed = station.CheckOrbitalEarlyWarning(2);
            Assert.NotNull(relayed);
            Assert.Equal("harrow_strike_alpha", relayed.eventId);
        }

        [Fact]
        public void SaveRestore_PreservesRadioStateAndBearings()
        {
            var station = CreateSystem(out _);
            station.TuneTo(7115, "hf");
            station.SetAntennaAzimuth(135);
            station.ScanFrequency(1);
            station.RecordBearing("radio_intercept_meridian_supply_column_01", 135);

            var save = station.CaptureState();
            var station2 = CreateSystem(out _);
            station2.RestoreState(save);

            Assert.Equal(7115, station2.State.tunedFrequencyKhz);
            Assert.Equal(135, station2.State.antennaAzimuthDegrees);
            var restored = station2.GetOrCreateInterceptProgress("radio_intercept_meridian_supply_column_01");
            Assert.NotNull(restored);
            Assert.True(restored.Detected);
            Assert.Equal(1, restored.BearingsCollected);
        }

        [Fact]
        public void DeterministicReplay_ProducesIdenticalRadioScans()
        {
            var sysA = CreateSystem(out _, seed: 9999);
            var sysB = CreateSystem(out _, seed: 9999);

            sysA.TuneTo(7115, "hf");
            sysB.TuneTo(7115, "hf");

            var resA = sysA.ScanFrequency(1);
            var resB = sysB.ScanFrequency(1);

            Assert.Equal(resA.FoundSignal, resB.FoundSignal);
            Assert.Equal(resA.InterceptId, resB.InterceptId);
            Assert.Equal(resA.SignalStrength, resB.SignalStrength);
        }
    }
}
