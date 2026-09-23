// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 205: Shelter Noise Discipline & Acoustic Management — Integration Tests
// Verifies noise sources catalog loading, definition-based registration,
// soundproofing attenuation, quiet hours enforcement, daily noise aggregation,
// and state capture/restore persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class Plan205ShelterNoiseIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllFifteenNoiseSources()
        {
            var system = new ShelterNoiseSystem();
            string path = Path.Combine(DataDirectory, "noise_sources.json");
            Assert.True(File.Exists(path), $"noise_sources.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var defs = system.GetAllSourceDefs();
            Assert.True(defs.Count >= 15, $"Expected at least 15 noise sources, found {defs.Count}");

            // Verify major categories are represented
            var types = defs.Select(d => d.type).ToHashSet(StringComparer.OrdinalIgnoreCase);
            Assert.Contains("generator", types);
            Assert.Contains("machinery", types);
            Assert.Contains("human_activity", types);
            Assert.Contains("industrial_process", types);
            Assert.Contains("alarm", types);
            Assert.Contains("ventilation", types);
            Assert.Contains("construction", types);
            Assert.Contains("music_recreation", types);
            Assert.Contains("argument", types);

            // Verify specific def fields
            var dieselGen = system.GetSourceDef("noise_generator_diesel");
            Assert.NotNull(dieselGen);
            Assert.Equal("generator", dieselGen.type);
            Assert.Equal(80.0f, dieselGen.default_output);
            Assert.Equal("low", dieselGen.frequency);
            Assert.True(dieselGen.can_be_soundproofed);

            var klaxon = system.GetSourceDef("noise_siren_klaxon");
            Assert.NotNull(klaxon);
            Assert.Equal("alarm", klaxon.type);
            Assert.Equal(95.0f, klaxon.default_output);
            Assert.Equal("high", klaxon.frequency);
            Assert.False(klaxon.can_be_soundproofed);

            foreach (var def in defs)
            {
                Assert.False(string.IsNullOrWhiteSpace(def.source_def_id));
                Assert.False(string.IsNullOrWhiteSpace(def.display_name));
                Assert.True(def.default_output > 0f);
                Assert.True(def.duration_hours > 0f);
            }
        }

        [Fact]
        public void AddNoiseSourceFromDef_RegistersSourceWithCorrectOutputAndFrequency()
        {
            var system = new ShelterNoiseSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));

            var dieselSrc = system.AddNoiseSourceFromDef("noise_generator_diesel", "power_bay");
            Assert.NotNull(dieselSrc);
            Assert.Equal(NoiseSourceType.Generator, dieselSrc.Type);
            Assert.Equal(NoiseFrequency.Low, dieselSrc.Frequency);
            Assert.Equal(80f, dieselSrc.NoiseOutput);
            Assert.Equal(24f, dieselSrc.DurationHours);
            Assert.True(dieselSrc.CanBeSoundproofed);
            Assert.Equal("power_bay", dieselSrc.RoomId);

            var sirenSrc = system.AddNoiseSourceFromDef("noise_siren_klaxon", "siren_post");
            Assert.NotNull(sirenSrc);
            Assert.Equal(NoiseSourceType.Alarm, sirenSrc.Type);
            Assert.Equal(NoiseFrequency.High, sirenSrc.Frequency);
            Assert.Equal(95f, sirenSrc.NoiseOutput);
            Assert.Equal(1f, sirenSrc.DurationHours);
            Assert.False(sirenSrc.CanBeSoundproofed);

            Assert.Equal(2, system.ActiveSourceCount);
        }

        [Fact]
        public void SoundproofRoom_AttenuatesEffectiveNoiseLevel()
        {
            // Unattenuated system
            var systemRaw = new ShelterNoiseSystem();
            systemRaw.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));
            systemRaw.AddNoiseSourceFromDef("noise_metal_lathe", "workshop_a");
            systemRaw.TickDay(currentDay: 1, currentHour: 12);
            float noiseUnattenuated = systemRaw.GetRoomNoise("workshop_a");

            // Soundproofed system
            var systemPadded = new ShelterNoiseSystem();
            systemPadded.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));
            systemPadded.SoundproofRoom("workshop_b", wallAdd: 70f, doorAdd: 50f);
            systemPadded.AddNoiseSourceFromDef("noise_metal_lathe", "workshop_b");
            systemPadded.TickDay(currentDay: 1, currentHour: 12);
            float noiseAttenuated = systemPadded.GetRoomNoise("workshop_b");

            Assert.True(noiseAttenuated < noiseUnattenuated,
                $"Soundproofed room noise ({noiseAttenuated}) must be less than raw noise ({noiseUnattenuated})");
        }

        [Fact]
        public void SetQuietHours_DetectsViolationsDuringQuietHours()
        {
            var system = new ShelterNoiseSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));
            system.SetQuietHours(enabled: true, startHour: 22, endHour: 6);

            NoiseEvent? violationEvent = null;
            system.OnNoiseSpike += ev => violationEvent = ev;

            // Add loud generator in power room
            system.AddNoiseSourceFromDef("noise_generator_diesel", "power_room");

            float riskBefore = system.DetectionRisk;

            // Tick during quiet hours at 23:00 (11 PM)
            system.TickDay(currentDay: 4, currentHour: 23);

            Assert.NotNull(violationEvent);
            Assert.Equal("quiet_hours_violation", violationEvent.EventType);
            Assert.Equal(4, violationEvent.Day);
            Assert.True(violationEvent.NoiseLevel > 30f);
            Assert.True(system.DetectionRisk > riskBefore,
                $"Detection risk ({system.DetectionRisk}) should exceed baseline ({riskBefore}) after quiet hour violation");
        }

        [Fact]
        public void TickDay_CalculatesOverallNoiseAndDetectionRisk()
        {
            var system = new ShelterNoiseSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));

            system.AddNoiseSourceFromDef("noise_generator_diesel", "sub_basement");
            system.AddNoiseSourceFromDef("noise_forge_hammer", "foundry_bay");

            float riskReported = 0f;
            system.OnThreatDetectionRiskIncreased += risk => riskReported = risk;

            system.TickDay(currentDay: 1, currentHour: 14);

            Assert.True(system.OverallNoiseLevel > 40f,
                $"Overall noise level ({system.OverallNoiseLevel}) should reflect running diesel gen and forge hammer");
            Assert.True(system.DetectionRisk > 0f);
            Assert.True(riskReported > 0f);
        }

        [Fact]
        public void SaveRestoreState_PreservesNoiseSourcesProfilesAndEvents()
        {
            var system1 = new ShelterNoiseSystem();
            system1.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "noise_sources.json")));
            system1.SetQuietHours(true, 21, 5);
            system1.SoundproofRoom("reactor_core", 80f, 60f);

            var src1 = system1.AddNoiseSourceFromDef("noise_generator_diesel", "reactor_core");
            var src2 = system1.AddNoiseSourceFromDef("noise_siren_klaxon", "shelter_hub");

            // Tick during quiet hours to generate an event
            system1.TickDay(currentDay: 7, currentHour: 22);

            var captured = system1.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(2, captured.Sources.Count);
            Assert.True(captured.QuietHoursActive);
            Assert.Equal(21, captured.QuietHoursStart);
            Assert.Equal(5, captured.QuietHoursEnd);
            Assert.NotEmpty(captured.Events);

            // Restore in fresh system
            var system2 = new ShelterNoiseSystem();
            system2.RestoreState(captured);

            Assert.Equal(system1.OverallNoiseLevel, system2.OverallNoiseLevel);
            Assert.Equal(system1.DetectionRisk, system2.DetectionRisk);
            Assert.Equal(2, system2.Sources.Count);
            Assert.Equal(system1.QuietHoursActive, system2.QuietHoursActive);
            Assert.Equal(system1.QuietHoursStart, system2.QuietHoursStart);
            Assert.Equal(system1.QuietHoursEnd, system2.QuietHoursEnd);

            var restoredRoom = system2.RoomProfiles.FirstOrDefault(r => r.RoomId == "reactor_core");
            Assert.NotNull(restoredRoom);
            Assert.Equal(80f, restoredRoom.WallSoundproofing);
            Assert.Equal(60f, restoredRoom.DoorSoundproofing);
        }
    }
}
