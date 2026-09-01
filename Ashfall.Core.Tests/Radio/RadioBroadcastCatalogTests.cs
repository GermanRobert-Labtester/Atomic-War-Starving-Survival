// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioBroadcastCatalogTests
    {
        [Fact]
        public void GapBroadcasts_AreRegistered_And12ItemsAuthored()
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.RegisterAuthoredGapBroadcasts();

            Assert.Equal(12, catalog.TotalCount);

            var chlorine = catalog.GetById("rad_gap_civil_chlorine_reserve");
            Assert.NotNull(chlorine);
            Assert.Equal(88.50f, chlorine!.FrequencyMhz);
            Assert.Equal(BroadcastPriority.Important, chlorine.Priority);
            Assert.Equal(RadioStationCatalog.StationCivilDefense, chlorine.StationId);

            var rockslide = catalog.GetById("rad_gap_route_south_pass_rockslide");
            Assert.NotNull(rockslide);
            Assert.Equal(142.50f, rockslide!.FrequencyMhz);
            Assert.Equal(BroadcastGenre.InfrastructureLogistics, rockslide.Genre);
        }

        [Fact]
        public void GetEligibleBroadcasts_FiltersCorrectlyByDayAndFrequency()
        {
            var catalog = new RadioBroadcastCatalog();
            catalog.RegisterAuthoredGapBroadcasts();

            // Day 40, 88.5 MHz
            var eligibleDay40 = catalog.GetEligibleBroadcasts(88.50f, 40);
            Assert.NotEmpty(eligibleDay40);
            Assert.Contains(eligibleDay40, b => b.BroadcastId == "rad_gap_civil_chlorine_reserve");

            // Day 20, 88.5 MHz - chlorine minDay is 35, should not be eligible
            var eligibleDay20 = catalog.GetEligibleBroadcasts(88.50f, 20);
            Assert.DoesNotContain(eligibleDay20, b => b.BroadcastId == "rad_gap_civil_chlorine_reserve");
        }

        [Fact]
        public void LoadBaseRadioJson_ParsesCivilianAndEmergencyRecords()
        {
            string sampleJson = @"
            {
              ""schema_version"": 1,
              ""radio_broadcasts"": [
                {
                  ""id"": ""radio_broadcast_01"",
                  ""frequency"": 88.5,
                  ""minDay"": 1,
                  ""maxDay"": 30,
                  ""intelType"": ""Civilian"",
                  ""message"": ""Good morning, citizens.""
                },
                {
                  ""id"": ""radio_broadcast_11"",
                  ""frequency"": 102.1,
                  ""minDay"": 1,
                  ""maxDay"": 45,
                  ""intelType"": ""Military"",
                  ""message"": ""EMERGENCY ALERT. Fallout detected.""
                }
              ]
            }";

            var catalog = new RadioBroadcastCatalog();
            catalog.LoadBaseRadioJson(sampleJson);

            Assert.Equal(2, catalog.TotalCount);
            var b01 = catalog.GetById("radio_broadcast_01");
            Assert.NotNull(b01);
            Assert.Equal(88.5f, b01!.FrequencyMhz);
            Assert.Equal(BroadcastGenre.CivilianNews, b01.Genre);
            Assert.Equal(SourceReliability.Official, b01.Reliability);

            var b11 = catalog.GetById("radio_broadcast_11");
            Assert.NotNull(b11);
            Assert.Equal(102.1f, b11!.FrequencyMhz);
            Assert.Equal(BroadcastGenre.MilitaryEdict, b11.Genre);
        }
    }
}
