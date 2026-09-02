// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class CommsArraySystemTests
    {
        private static string GetCatalogJson()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Assets/StreamingAssets/Data/comms_targets.json");
            if (!File.Exists(path))
            {
                path = Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data/comms_targets.json");
            }
            if (File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return @"{
  ""schema_version"": 1,
  ""targets"": [
    {
      ""id"": ""comms_target_weather_beacon_alpha"",
      ""display_name"": ""Automated Weather Beacon Alpha"",
      ""target_type"": ""automated_beacon"",
      ""min_array_tier"": 1,
      ""frequency_khz"": 14220,
      ""band"": ""HF"",
      ""required_power_watts"": 150,
      ""description"": ""Test beacon"",
      ""has_satellite_window"": false,
      ""is_strategic"": false,
      ""revealed_faction_id"": """"
    },
    {
      ""id"": ""comms_target_orbital_relay_echo"",
      ""display_name"": ""Orbital Telemetry Relay Echo-7"",
      ""target_type"": ""satellite_relay"",
      ""min_array_tier"": 2,
      ""frequency_khz"": 435500,
      ""band"": ""UHF"",
      ""required_power_watts"": 600,
      ""description"": ""Test sat"",
      ""has_satellite_window"": true,
      ""is_strategic"": false,
      ""revealed_faction_id"": """"
    },
    {
      ""id"": ""comms_target_strategic_uplink_cerberus"",
      ""display_name"": ""Strategic Defense Platform Cerberus"",
      ""target_type"": ""strategic_uplink"",
      ""min_array_tier"": 3,
      ""frequency_khz"": 448900,
      ""band"": ""UHF"",
      ""required_power_watts"": 1200,
      ""description"": ""Test strategic"",
      ""has_satellite_window"": true,
      ""is_strategic"": true,
      ""revealed_faction_id"": """"
    }
  ]
}";
        }

        [Fact]
        public void SetArrayTier_ClampsAndPersists()
        {
            var sys = new CommsArraySystem(new SeededRng(200));
            sys.SetArrayTier(2);
            Assert.Equal(2, sys.State.ArrayTier);

            sys.SetArrayTier(5);
            Assert.Equal(3, sys.State.ArrayTier); // clamped to 3

            sys.SetArrayTier(-1);
            Assert.Equal(1, sys.State.ArrayTier); // clamped to 1
        }

        [Fact]
        public void SetPowerState_UpdatesPowerStatus()
        {
            var sys = new CommsArraySystem(new SeededRng(201));
            sys.SetPowerState(false, 0f);
            Assert.False(sys.State.IsPowered);
            Assert.Equal(0f, sys.State.AvailablePowerWatts);

            sys.SetPowerState(true, 850f);
            Assert.True(sys.State.IsPowered);
            Assert.Equal(850f, sys.State.AvailablePowerWatts);
        }

        [Fact]
        public void TuneFrequency_SetsFrequencyAndBand()
        {
            var sys = new CommsArraySystem(new SeededRng(202));
            sys.TuneFrequency(144300, "VHF");
            Assert.Equal(144300, sys.State.CurrentFrequencyKhz);
            Assert.Equal("VHF", sys.State.CurrentBand);
        }

        [Fact]
        public void IsInSatelliteWindow_NonSatelliteTarget_AlwaysTrue()
        {
            var sys = new CommsArraySystem(new SeededRng(203));
            sys.LoadCatalog(GetCatalogJson());

            var target = sys.TargetCatalog["comms_target_weather_beacon_alpha"];
            Assert.True(sys.IsInSatelliteWindow(target, 1, 0));
            Assert.True(sys.IsInSatelliteWindow(target, 1, 12));
        }

        [Fact]
        public void IsInSatelliteWindow_SatelliteTarget_CalculatesDeterministicWindow()
        {
            var sys = new CommsArraySystem(new SeededRng(204));
            sys.LoadCatalog(GetCatalogJson());

            var target = sys.TargetCatalog["comms_target_orbital_relay_echo"];
            // Orbit period is 8 hours, target frequency is 435500 -> phase = (435500/100) % 8 = 4355 % 8 = 3.
            // On Day 0, pass hours are 3 and 4.
            Assert.True(sys.IsInSatelliteWindow(target, 0, 3));
            Assert.True(sys.IsInSatelliteWindow(target, 0, 4));
            Assert.False(sys.IsInSatelliteWindow(target, 0, 0));
            Assert.False(sys.IsInSatelliteWindow(target, 0, 6));
        }

        [Fact]
        public void TickScan_UnpoweredArray_DoesNotAdvanceScan()
        {
            var sys = new CommsArraySystem(new SeededRng(205));
            sys.LoadCatalog(GetCatalogJson());
            sys.SetPowerState(false, 0f);
            sys.TuneFrequency(14220, "HF");

            string? contact = sys.TickScan(1, 12, 0.5f);
            Assert.Null(contact);
            Assert.Equal(0, sys.State.TotalScansConducted);
        }

        [Fact]
        public void TickScan_MatchingFrequency_AdvancesLockAndEstablishesContact()
        {
            var sys = new CommsArraySystem(new SeededRng(206));
            sys.LoadCatalog(GetCatalogJson());
            sys.SetPowerState(true, 1000f);
            sys.TuneFrequency(14220, "HF");

            bool eventFired = false;
            sys.OnContactEstablished += (target, lockState) =>
            {
                if (target.Id == "comms_target_weather_beacon_alpha" && lockState.IsContactEstablished)
                    eventFired = true;
            };

            // Multiple scan ticks to complete 1000 permille lock
            string? contact = null;
            for (int i = 0; i < 8; i++)
            {
                contact = sys.TickScan(1, 12, 0.8f);
                if (contact != null) break;
            }

            Assert.Equal("comms_target_weather_beacon_alpha", contact);
            Assert.True(eventFired);
            Assert.Contains("comms_target_weather_beacon_alpha", sys.State.DecodedTargetIds);
        }

        [Fact]
        public void TickScan_StrategicTarget_GeneratesAuthorizationCode()
        {
            var sys = new CommsArraySystem(new SeededRng(207));
            sys.LoadCatalog(GetCatalogJson());
            sys.SetArrayTier(3);
            sys.SetPowerState(true, 2000f);
            sys.TuneFrequency(448900, "UHF");

            var target = sys.TargetCatalog["comms_target_strategic_uplink_cerberus"];
            // Find a valid window hour for target
            int windowHour = 0;
            for (int h = 0; h < 24; h++)
            {
                if (sys.IsInSatelliteWindow(target, 5, h))
                {
                    windowHour = h;
                    break;
                }
            }

            for (int i = 0; i < 10; i++)
            {
                sys.TickScan(5, windowHour, 1.0f);
            }

            Assert.Single(sys.State.StrategicAuthorizationCodes);
            Assert.StartsWith("AUTH-ORBITAL-", sys.State.StrategicAuthorizationCodes[0]);
        }

        [Fact]
        public void RequestStrategicStrike_ValidCodeAndTier3_FiresEventAndConsumesCode()
        {
            var sys = new CommsArraySystem(new SeededRng(208));
            sys.LoadCatalog(GetCatalogJson());
            sys.SetArrayTier(3);
            sys.SetPowerState(true, 2000f);

            string code = "AUTH-ORBITAL-TEST-00001";
            sys.State.StrategicAuthorizationCodes.Add(code);

            bool strikeFired = false;
            sys.OnStrategicStrikeRequested += (target, auth) =>
            {
                if (target == "comms_target_strategic_uplink_cerberus" && auth == code)
                    strikeFired = true;
            };

            bool success = sys.RequestStrategicStrike("comms_target_strategic_uplink_cerberus", code, out string error);
            Assert.True(success);
            Assert.True(string.IsNullOrEmpty(error));
            Assert.True(strikeFired);
            Assert.DoesNotContain(code, sys.State.StrategicAuthorizationCodes);
        }

        [Fact]
        public void RequestStrategicStrike_LowTier_ReturnsError()
        {
            var sys = new CommsArraySystem(new SeededRng(209));
            sys.LoadCatalog(GetCatalogJson());
            sys.SetArrayTier(2); // Tier 2 is insufficient for Tier 3 strategic strike
            sys.SetPowerState(true, 2000f);

            string code = "AUTH-ORBITAL-TEST-00002";
            sys.State.StrategicAuthorizationCodes.Add(code);

            bool success = sys.RequestStrategicStrike("comms_target_strategic_uplink_cerberus", code, out string error);
            Assert.False(success);
            Assert.Contains("tier 3", error, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void SaveRestore_PreservesLocksCodesAndScans()
        {
            var sys1 = new CommsArraySystem(new SeededRng(210));
            sys1.LoadCatalog(GetCatalogJson());
            sys1.SetArrayTier(2);
            sys1.TuneFrequency(14220, "HF");
            sys1.SetPowerState(true, 1500f);
            sys1.TickScan(1, 12, 0.5f);
            sys1.State.StrategicAuthorizationCodes.Add("AUTH-SAVED-12345");

            var saved = sys1.CaptureState();

            var sys2 = new CommsArraySystem(new SeededRng(211));
            sys2.RestoreState(saved);

            Assert.Equal(2, sys2.State.ArrayTier);
            Assert.Equal(14220, sys2.State.CurrentFrequencyKhz);
            Assert.Equal("HF", sys2.State.CurrentBand);
            Assert.Equal(sys1.State.TotalScansConducted, sys2.State.TotalScansConducted);
            Assert.Single(sys2.State.StrategicAuthorizationCodes);
            Assert.Equal("AUTH-SAVED-12345", sys2.State.StrategicAuthorizationCodes[0]);
            Assert.Single(sys2.State.Locks);
        }

        [Fact]
        public void DeterministicReplay_SameInputsProduceIdenticalScanState()
        {
            var sysA = new CommsArraySystem(new SeededRng(777));
            var sysB = new CommsArraySystem(new SeededRng(777));
            sysA.LoadCatalog(GetCatalogJson());
            sysB.LoadCatalog(GetCatalogJson());

            sysA.SetArrayTier(2);
            sysB.SetArrayTier(2);
            sysA.TuneFrequency(14220, "HF");
            sysB.TuneFrequency(14220, "HF");

            for (int i = 0; i < 5; i++)
            {
                sysA.TickScan(2, 10, 0.6f);
                sysB.TickScan(2, 10, 0.6f);
            }

            Assert.Equal(sysA.State.TotalScansConducted, sysB.State.TotalScansConducted);
            Assert.Equal(sysA.State.Locks[0].LockPermille, sysB.State.Locks[0].LockPermille);
            Assert.Equal(sysA.State.Locks[0].IsContactEstablished, sysB.State.Locks[0].IsContactEstablished);
        }
    }
}
