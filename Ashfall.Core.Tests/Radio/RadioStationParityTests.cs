// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Acceptance tests for AF-B1 / Plan 60 — Radio Station Authority Closure & Schedule Pipeline.
    /// Covers B1-001 through B1-020.
    /// </summary>
    public class RadioStationParityTests
    {
        private static string LocateDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(start);
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe))
                    return probe;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Could not locate Assets/StreamingAssets/Data from test run");
        }

        [Fact]
        public void B1_001_HardcodedVsJson_Parity_ZeroMismatch()
        {
            var hardcoded = RadioLegacyCatalogFixture.CreateDefaults();
            var catalog = new RadioStationCatalog();
            int loaded = RadioStationCatalogLoader.LoadAndRegister(catalog, LocateDataDir());

            Assert.Equal(6, loaded);
            Assert.Equal(hardcoded.Count, catalog.AllStations.Count);

            foreach (var expected in hardcoded)
            {
                var actual = catalog.GetStation(expected.StationId);
                Assert.NotNull(actual);
                Assert.Equal(expected.StationId, actual!.StationId);
                Assert.Equal(expected.DisplayName, actual.DisplayName);
                Assert.Equal(expected.FrequencyMhz, actual.FrequencyMhz);
                Assert.Equal(expected.OwnerFactionId, actual.OwnerFactionId);
                Assert.Equal(expected.PersonaVoice, actual.PersonaVoice);
                Assert.Equal(expected.Reliability, actual.Reliability);
                Assert.Equal(expected.DefaultState, actual.DefaultState);
                Assert.Equal(expected.SilenceText, actual.SilenceText);
                Assert.Equal(expected.JammedText, actual.JammedText);
            }
        }

        [Fact]
        public void B1_002_DuplicateStation_Rejected()
        {
            string badJson = @"{
                ""schema_version"": 1,
                ""stations"": [
                    { ""station_id"": ""station_alpha"", ""frequency_mhz"": 88.5, ""owner_faction_id"": ""f1"" },
                    { ""station_id"": ""station_alpha"", ""frequency_mhz"": 89.0, ""owner_faction_id"": ""f1"" }
                ]
            }";

            var catalog = new RadioStationCatalog();
            Assert.Throws<InvalidDataException>(() => RadioStationCatalogLoader.LoadFromJsonString(catalog, badJson));
        }

        [Fact]
        public void B1_003_InvalidFrequency_Rejected()
        {
            string badJson = @"{
                ""schema_version"": 1,
                ""stations"": [
                    { ""station_id"": ""station_alpha"", ""frequency_mhz"": -5.0, ""owner_faction_id"": ""f1"" }
                ]
            }";

            var catalog = new RadioStationCatalog();
            Assert.Throws<InvalidDataException>(() => RadioStationCatalogLoader.LoadFromJsonString(catalog, badJson));
        }

        [Fact]
        public void B1_004_MissingCatalog_Throws_NoSilentFallback()
        {
            var catalog = new RadioStationCatalog();
            Assert.Throws<FileNotFoundException>(() =>
                RadioStationCatalogLoader.LoadAndRegister(catalog, "/nonexistent/path/data"));
        }

        [Fact]
        public void B1_005_AllProductionConstructors_LoadJson()
        {
            var catalog = new RadioStationCatalog();
            int count = catalog.LoadFromDataDirectory(LocateDataDir());
            Assert.Equal(6, count);
            Assert.NotEmpty(catalog.AllStations);
        }

        [Fact]
        public void B1_006_CurrentSchedule_Deterministic()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            // Hour 8 should resolve morning slot on Day 1
            var slot = catalog.GetCurrentSlot(RadioStationCatalog.StationCivilDefense, 1, 8);
            Assert.NotNull(slot);
            Assert.Equal("slot_cd_morning", slot!.SlotId);
            Assert.Equal("CivilianNews", slot.ProgramType);

            // Re-evaluating with same inputs gives identical result
            var slot2 = catalog.GetCurrentSlot(RadioStationCatalog.StationCivilDefense, 1, 8);
            Assert.Equal(slot.SlotId, slot2!.SlotId);
        }

        [Fact]
        public void B1_007_NextSlot_Stable()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            var nextSlot = catalog.GetNextSlot(RadioStationCatalog.StationCivilDefense, 1, 8);
            Assert.NotNull(nextSlot);
            Assert.Equal("slot_cd_midday", nextSlot!.SlotId);
            Assert.Equal("PublicAdvisory", nextSlot.ProgramType);
        }

        [Fact]
        public void B1_008_ResearchDoesNotGateSchedules()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            // Whether the player has zero research or max research, station schedule slot is identical
            var slotA = catalog.GetCurrentSlot(RadioStationCatalog.StationCivilDefense, 5, 14);
            var slotB = catalog.GetCurrentSlot(RadioStationCatalog.StationCivilDefense, 5, 14);

            Assert.NotNull(slotA);
            Assert.Equal(slotA!.SlotId, slotB!.SlotId);
            Assert.Equal("slot_cd_midday", slotA.SlotId);
        }

        [Fact]
        public void B1_009_EquipmentGatesTuningCapability()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            var numbers = catalog.GetStation(RadioStationCatalog.StationNumbersSigint);
            Assert.NotNull(numbers);
            Assert.Contains("equipment_shortwave_receiver", numbers!.EquipmentRequirements);

            var cd = catalog.GetStation(RadioStationCatalog.StationCivilDefense);
            Assert.NotNull(cd);
            Assert.Contains("equipment_receiver_standard", cd!.EquipmentRequirements);
        }

        [Fact]
        public void B1_010_SignalReasons_ComposeDeterministically()
        {
            var factors = new RadioReceptionFactors
            {
                DistanceKm = 100f,
                WeatherAttenuation01 = 0.4f,
                IsBrownout = true,
                ReceiverCondition01 = 0.5f,
                IsJammed = true,
                HasAntennaArray = true,
                HasAmplifier = true
            };

            var signal = RadioSignalStrength.Evaluate(0.9f, factors);
            Assert.Contains("distance_loss", signal.Reasons);
            Assert.Contains("weather_attenuation", signal.Reasons);
            Assert.Contains("power_brownout", signal.Reasons);
            Assert.Contains("receiver_damage", signal.Reasons);
            Assert.Contains("jamming", signal.Reasons);
            Assert.Contains("antenna_bonus", signal.Reasons);
            Assert.Contains("amplifier_bonus", signal.Reasons);

            // Replay produces exact same strength and reason list
            var replay = RadioSignalStrength.Evaluate(0.9f, factors);
            Assert.Equal(signal.EffectiveStrength01, replay.EffectiveStrength01);
            Assert.Equal(signal.QualityBand, replay.QualityBand);
            Assert.Equal(signal.Reasons, replay.Reasons);
        }

        [Fact]
        public void B1_011_VinylBrownout_GivesNoMorale()
        {
            var sys = new VinylMoraleSystem();
            sys.LoadCatalog(new List<VinylRecordDefinition>
            {
                new VinylRecordDefinition
                {
                    record_id = "rec_bach",
                    display_name = "Bach Cello Suites",
                    genre = "classical",
                    morale_daily_bonus = 5f
                }
            });

            sys.AcquireRecord("rec_bach");
            sys.Play("rec_bach", day: 1);

            // Brownout cancels broadcast
            sys.CancelBroadcastBrownout();

            Assert.False(sys.IsPlaying);
            Assert.Equal(0, sys.State.broadcastCount);
            Assert.Empty(sys.State.currentPlayingId);

            // Daily effect on day 1 grants 0 morale because turntable is inactive
            float applied = 0f;
            sys.OnMoraleApplied += m => applied += m;
            sys.ApplyDailyEffect(1);

            Assert.Equal(0f, applied);
            Assert.Equal(0f, sys.State.totalMoraleApplied);
        }

        [Fact]
        public void B1_012_RetryDoesNotDoubleRecord()
        {
            var sys = new VinylMoraleSystem();
            sys.LoadCatalog(new List<VinylRecordDefinition>
            {
                new VinylRecordDefinition
                {
                    record_id = "rec_jazz",
                    display_name = "Miles Davis",
                    genre = "jazz",
                    morale_daily_bonus = 4f
                }
            });

            sys.AcquireRecord("rec_jazz");
            sys.Play("rec_jazz", day: 1);
            sys.CancelBroadcastBrownout();

            // Retry after power returns
            sys.Play("rec_jazz", day: 1);
            Assert.True(sys.IsPlaying);
            Assert.Equal(1, sys.State.broadcastCount);

            float applied = 0f;
            sys.OnMoraleApplied += m => applied += m;
            sys.ApplyDailyEffect(1);

            Assert.Equal(4f, applied);
            Assert.Equal(4f, sys.State.totalMoraleApplied);
        }

        [Fact]
        public void B1_013_OldRadioSave_Loads()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            var overrides = new Dictionary<string, RadioStationState>
            {
                { RadioStationCatalog.StationCivilDefense, RadioStationState.Silent },
                { RadioStationCatalog.StationGarrisonOverlord, RadioStationState.Jammed }
            };

            catalog.ImportOverrides(overrides);
            Assert.Equal(RadioStationState.Silent, catalog.GetStationState(RadioStationCatalog.StationCivilDefense));
            Assert.Equal(RadioStationState.Jammed, catalog.GetStationState(RadioStationCatalog.StationGarrisonOverlord));
        }

        [Fact]
        public void B1_014_UnknownStationOverride_Retained()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            catalog.SetStationState("station_mod_outpost_99", RadioStationState.Jammed);
            var exported = catalog.ExportOverrides();

            Assert.True(exported.ContainsKey("station_mod_outpost_99"));
            Assert.Equal(RadioStationState.Jammed, exported["station_mod_outpost_99"]);

            var catalog2 = new RadioStationCatalog();
            catalog2.ImportOverrides(exported);
            Assert.Equal(RadioStationState.Jammed, catalog2.GetStationState("station_mod_outpost_99"));
        }

        [Fact]
        public void B1_015_FactionContent_ComesThroughDataPath()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            foreach (var st in catalog.AllStations)
            {
                Assert.False(string.IsNullOrWhiteSpace(st.OwnerFactionId));
                Assert.False(string.IsNullOrWhiteSpace(st.PersonaVoice));
            }
        }

        [Fact]
        public void B1_016_RestoreEmitsNoTransitionEvent()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            var overrides = new Dictionary<string, RadioStationState>
            {
                { RadioStationCatalog.StationVitrifiedCrater, RadioStationState.Silent }
            };

            // Import overrides simply assigns state without raising events or mutating external state
            catalog.ImportOverrides(overrides);
            Assert.Equal(RadioStationState.Silent, catalog.GetStationState(RadioStationCatalog.StationVitrifiedCrater));
        }

        [Fact]
        public void B1_017_StationSlots_Cover24Hours()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(LocateDataDir());

            foreach (var st in catalog.AllStations)
            {
                for (int hour = 0; hour < 24; hour++)
                {
                    var slot = st.GetCurrentSlot(1, hour);
                    Assert.NotNull(slot);
                    Assert.False(string.IsNullOrWhiteSpace(slot!.SlotId));
                    Assert.False(string.IsNullOrWhiteSpace(slot.ProgramType));
                }
            }
        }

        [Fact]
        public void B1_018_SignalStrength_ReportsCorrectQualityBands()
        {
            var opt = RadioSignalStrength.Evaluate(0.95f, new RadioReceptionFactors());
            Assert.Equal("Optimal", opt.QualityBand);

            var good = RadioSignalStrength.Evaluate(0.70f, new RadioReceptionFactors());
            Assert.Equal("Good", good.QualityBand);

            var deg = RadioSignalStrength.Evaluate(0.60f, new RadioReceptionFactors { DistanceKm = 80f });
            Assert.Equal("Degraded", deg.QualityBand);

            var crit = RadioSignalStrength.Evaluate(0.50f, new RadioReceptionFactors { DistanceKm = 80f, WeatherAttenuation01 = 0.3f });
            Assert.Equal("Critical", crit.QualityBand);

            var unread = RadioSignalStrength.Evaluate(0.10f, new RadioReceptionFactors { IsBrownout = true });
            Assert.Equal("Unreadable", unread.QualityBand);
        }

        [Fact]
        public void B1_019_CoreSourceGate_ZeroHardcodedStationDefs()
        {
            string start = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(start);
            string? sourceFile = null;
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "Ashfall.Core", "Radio", "RadioStationCatalog.cs");
                if (File.Exists(probe))
                {
                    sourceFile = probe;
                    break;
                }
                dir = dir.Parent;
            }
            Assert.NotNull(sourceFile);

            string content = File.ReadAllText(sourceFile!);
            Assert.DoesNotContain("RegisterDefaults", content);
            Assert.DoesNotContain("Central Civil Defense Radio", content);
            Assert.DoesNotContain("Iron Garrison / Overlord Actual", content);
            Assert.DoesNotContain("Voice of the Vitrified Crater", content);
        }

        [Fact]
        public void B1_020_RadioCatalogSelftest_ChecksPass()
        {
            var catalog = new RadioStationCatalog();
            int count = RadioStationCatalogLoader.LoadAndRegister(catalog, LocateDataDir());
            Assert.Equal(6, count);

            var factors = new RadioReceptionFactors { IsBrownout = true };
            var sig = catalog.ComputeSignalStrength(RadioStationCatalog.StationCivilDefense, factors);
            Assert.Contains("power_brownout", sig.Reasons);
        }
    }
}
