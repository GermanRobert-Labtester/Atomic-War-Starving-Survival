// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterNoiseSystemTests
    {
        [Fact]
        public void AddNoiseSource_RegistersSourceAndRoom()
        {
            var system = new ShelterNoiseSystem();
            var src = system.AddNoiseSource(NoiseSourceType.Generator, "power_room", output: 75f, freq: NoiseFrequency.Low);

            Assert.NotNull(src);
            Assert.Equal(NoiseSourceType.Generator, src.Type);
            Assert.Equal("power_room", src.RoomId);
            Assert.Equal(75f, src.NoiseOutput);
            Assert.Equal(1, system.ActiveSourceCount);
        }

        [Fact]
        public void TickDay_CalculatesRoomAndOverallNoise()
        {
            var system = new ShelterNoiseSystem();
            system.AddNoiseSource(NoiseSourceType.Generator, "room_gen", output: 60f);

            system.TickDay(currentDay: 1, currentHour: 14);

            float roomNoise = system.GetRoomNoise("room_gen");
            Assert.True(roomNoise > 0f);
            Assert.True(system.OverallNoiseLevel > 0f);
        }

        [Fact]
        public void Soundproofing_ReducesEffectiveNoiseLevel()
        {
            var systemNoSoundproof = new ShelterNoiseSystem();
            systemNoSoundproof.AddNoiseSource(NoiseSourceType.Machinery, "work_room", output: 80f, freq: NoiseFrequency.Medium);
            systemNoSoundproof.TickDay(1, 12);
            float noiseUnattenuated = systemNoSoundproof.GetRoomNoise("work_room");

            var systemSoundproof = new ShelterNoiseSystem();
            systemSoundproof.SoundproofRoom("work_room", wallAdd: 50f, doorAdd: 50f);
            systemSoundproof.AddNoiseSource(NoiseSourceType.Machinery, "work_room", output: 80f, freq: NoiseFrequency.Medium);
            systemSoundproof.TickDay(1, 12);
            float noiseAttenuated = systemSoundproof.GetRoomNoise("work_room");

            Assert.True(noiseAttenuated < noiseUnattenuated);
        }

        [Fact]
        public void SetQuietHours_DetectsViolationsDuringQuietPeriod()
        {
            var system = new ShelterNoiseSystem();
            system.SetQuietHours(enabled: true, startHour: 22, endHour: 6);
            system.AddNoiseSource(NoiseSourceType.Construction, "quarry", output: 70f);

            NoiseEvent? violation = null;
            system.OnNoiseSpike += ev => violation = ev;

            // Tick during quiet hours (e.g. 23:00)
            system.TickDay(currentDay: 2, currentHour: 23);

            Assert.NotNull(violation);
            Assert.Equal("quiet_hours_violation", violation.EventType);
            Assert.True(system.DetectionRisk > 5f);
        }

        [Fact]
        public void SetSourceActive_DisablesNoiseContribution()
        {
            var system = new ShelterNoiseSystem();
            var src = system.AddNoiseSource(NoiseSourceType.Alarm, "siren_tower", output: 90f);

            system.SetSourceActive(src.SourceId, false);
            system.TickDay(currentDay: 3, currentHour: 10);

            Assert.Equal(0f, system.GetRoomNoise("siren_tower"));
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ShelterNoiseSystem();
            system1.SetQuietHours(true, 21, 7);
            system1.SoundproofRoom("rec_room", 40f, 30f);
            var src = system1.AddNoiseSource(NoiseSourceType.MusicRecreation, "rec_room", 50f);

            var state = system1.CaptureState();
            Assert.Single(state.Sources);
            Assert.Single(state.RoomProfiles);
            Assert.True(state.QuietHoursActive);

            var system2 = new ShelterNoiseSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ActiveSourceCount);
            Assert.True(system2.QuietHoursActive);
            var room = system2.GetRoomNoise("rec_room");
            system2.TickDay(1, 12);
            Assert.True(system2.GetRoomNoise("rec_room") > 0f);
        }
    }
}
