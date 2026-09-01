// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioSignalLogTests
    {
        [Fact]
        public void DiscoverStation_AndCustomPresets_WorkAndPersist()
        {
            var log = new RadioSignalLog();

            Assert.False(log.IsStationDiscovered(RadioStationCatalog.StationCivilDefense));
            bool newlyDiscovered = log.DiscoverStation(RadioStationCatalog.StationCivilDefense);
            Assert.True(newlyDiscovered);
            Assert.True(log.IsStationDiscovered(RadioStationCatalog.StationCivilDefense));

            // Duplicate discovery returns false
            Assert.False(log.DiscoverStation(RadioStationCatalog.StationCivilDefense));

            log.AddPreset(88.5f);
            log.AddPreset(104.2f);
            Assert.Equal(2, log.Presets.Count);

            // Capture state
            var stations = log.CaptureDiscoveredStations();
            var presets = log.CapturePresets();
            var entries = log.CaptureEntries();

            var newLog = new RadioSignalLog();
            newLog.RestoreState(entries, stations, presets);

            Assert.True(newLog.IsStationDiscovered(RadioStationCatalog.StationCivilDefense));
            Assert.Equal(2, newLog.Presets.Count);
            Assert.Contains(88.5f, newLog.Presets);
            Assert.Contains(104.2f, newLog.Presets);
        }

        [Fact]
        public void LogIntercept_CreatesAnalyticRecord_AndDiscoversStation()
        {
            var log = new RadioSignalLog();
            var bcast = new ScheduledBroadcastResult
            {
                HasTransmission = true,
                FrequencyMhz = 91.30f,
                StationId = RadioStationCatalog.StationOpenClassroom,
                StationName = "The Open Classroom",
                Headline = "Lesson 9 — Heirloom Seed Storage",
                Message = "Saving seeds in glass jars with bay leaves.",
                Genre = BroadcastGenre.Educational
            };

            var entry = log.LogIntercept(bcast, day: 40);
            Assert.NotNull(entry);
            Assert.Equal("The Open Classroom", bcast.StationName);
            Assert.True(log.IsStationDiscovered(RadioStationCatalog.StationOpenClassroom));
            Assert.Equal(1, log.Entries.Count);
        }
    }
}
