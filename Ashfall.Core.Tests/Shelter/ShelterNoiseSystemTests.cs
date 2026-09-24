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
        public void Constructor_CapturedState_DoesNotAliasInput()
        {
            var state = new ShelterNoiseState
            {
                Sources = new List<NoiseSource>
                {
                    new NoiseSource { SourceId = "ns_loaded", RoomId = "room_a", NoiseOutput = 20f }
                },
                RoomProfiles = new List<RoomAcousticProfile>
                {
                    new RoomAcousticProfile { RoomId = "room_a" }
                }
            };

            var system = new ShelterNoiseSystem(state);
            state.Sources.Clear();
            state.RoomProfiles.Clear();

            Assert.Single(system.Sources);
            Assert.Single(system.RoomProfiles);
        }

        [Fact]
        public void AddNoiseSource_BlankRoom_DoesNotLeavePartialMutation()
        {
            var system = new ShelterNoiseSystem();

            Assert.Throws<ArgumentNullException>(() =>
                system.AddNoiseSource(NoiseSourceType.Machinery, " ", output: 50f));

            Assert.Empty(system.Sources);
            Assert.Empty(system.RoomProfiles);
            Assert.Equal(1, system.State.NextSequence);
        }

        [Fact]
        public void Restore_StaleSequence_DoesNotDuplicateSourceOrEventIds()
        {
            var state = new ShelterNoiseState
            {
                NextSequence = 1,
                QuietHoursActive = true,
                QuietHoursStart = 22,
                QuietHoursEnd = 6,
                Sources = new List<NoiseSource>
                {
                    new NoiseSource { SourceId = "ns_2", RoomId = "room_a", NoiseOutput = 20f }
                },
                Events = new List<NoiseEvent>
                {
                    new NoiseEvent { EventId = "nev_2", EventType = "existing" }
                },
                RoomProfiles = new List<RoomAcousticProfile>
                {
                    new RoomAcousticProfile { RoomId = "room_a" }
                }
            };
            var system = new ShelterNoiseSystem();
            system.RestoreState(state);

            system.AddNoiseSource(NoiseSourceType.Machinery, "room_a", output: 20f);
            system.TickDay(1, 23);

            Assert.Equal(2, system.Sources.Count);
            Assert.Equal(2, system.Sources.Select(source => source.SourceId).Distinct().Count());
            Assert.Equal(2, system.Events.Select(noiseEvent => noiseEvent.EventId).Distinct().Count());
        }

        [Fact]
        public void NonFiniteStateAndCommands_RemainBounded()
        {
            var state = new ShelterNoiseState
            {
                OverallNoiseLevel = float.NaN,
                DetectionRisk = float.PositiveInfinity,
                QuietHoursStart = -5,
                QuietHoursEnd = 99,
                RoomProfiles = new List<RoomAcousticProfile>
                {
                    new RoomAcousticProfile
                    {
                        RoomId = "room_a",
                        WallSoundproofing = float.NaN,
                        DoorSoundproofing = float.PositiveInfinity
                    }
                },
                Sources = new List<NoiseSource>
                {
                    new NoiseSource
                    {
                        SourceId = "ns_1",
                        RoomId = "room_a",
                        NoiseOutput = float.NaN,
                        DurationHours = float.PositiveInfinity
                    }
                }
            };
            var system = new ShelterNoiseSystem(state);

            system.SoundproofRoom("room_a", float.NaN, float.PositiveInfinity);
            system.AttenuateDetectionRisk(float.NaN);
            system.TickDay(1, 23);

            Assert.InRange(system.OverallNoiseLevel, 0f, 100f);
            Assert.InRange(system.DetectionRisk, 0f, 100f);
            Assert.InRange(system.GetRoomNoise("room_a"), 0f, 100f);
            Assert.InRange(system.State.RoomProfiles[0].WallSoundproofing, 0f, 100f);
        }

        [Fact]
        public void LoadCatalog_NullSourceEntry_IsIgnored()
        {
            var system = new ShelterNoiseSystem();

            system.LoadCatalog("{\"schema_version\":1,\"sources\":[null,{\"source_def_id\":\"source_ok\",\"type\":\"generator\",\"frequency\":\"low\",\"default_output\":20,\"duration_hours\":4}]}" );

            Assert.Single(system.GetAllSourceDefs());
            Assert.NotNull(system.GetSourceDef("source_ok"));
        }

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
