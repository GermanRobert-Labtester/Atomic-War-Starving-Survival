// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class RadioStationCatalogTests
    {
        private static string DataDir()
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
        public void RadioStationCatalog_JsonMatchesHardcodedDefaults_ExactParity()
        {
            // Legacy definitions from test fixture (prior hardcoded values)
            var hardcoded = RadioLegacyCatalogFixture.CreateDefaults();

            // JSON catalog loaded from authoritative radio_stations.json
            string jsonPath = Path.Combine(DataDir(), "radio_stations.json");
            Assert.True(File.Exists(jsonPath), $"radio_stations.json missing at {jsonPath}");

            var jsonCatalog = new RadioStationCatalog();
            int loaded = jsonCatalog.LoadFromJson(File.ReadAllText(jsonPath));

            Assert.Equal(6, loaded);
            Assert.Equal(hardcoded.Count, jsonCatalog.AllStations.Count);

            foreach (var expected in hardcoded)
            {
                var actual = jsonCatalog.GetStation(expected.StationId);
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
        public void RadioStationCatalog_LoadFromDataDirectory_Succeeds()
        {
            var catalog = new RadioStationCatalog();
            catalog.Clear();
            int count = catalog.LoadFromDataDirectory(DataDir());

            Assert.Equal(6, count);
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationCivilDefense));
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationGarrisonOverlord));
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationVitrifiedCrater));
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationOpenClassroom));
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationNumbersSigint));
            Assert.NotNull(catalog.GetStation(RadioStationCatalog.StationAutomatedRelay));
        }

        [Fact]
        public void RadioStationCatalog_FindStationAtFrequency_ResolvesNearFrequencies()
        {
            var catalog = new RadioStationCatalog();
            catalog.Clear();
            catalog.LoadFromDataDirectory(DataDir());

            var cd = catalog.FindStationAtFrequency(88.52f, toleranceMhz: 0.1f);
            Assert.NotNull(cd);
            Assert.Equal(RadioStationCatalog.StationCivilDefense, cd!.StationId);

            var over = catalog.FindStationAtFrequency(88.38f, toleranceMhz: 0.1f);
            Assert.NotNull(over);
            Assert.Equal(RadioStationCatalog.StationGarrisonOverlord, over!.StationId);

            var none = catalog.FindStationAtFrequency(99.0f, toleranceMhz: 0.1f);
            Assert.Null(none);
        }

        [Fact]
        public void RadioStationCatalog_StateOverrides_PersistAndRestore()
        {
            var catalog = new RadioStationCatalog();
            catalog.LoadFromDataDirectory(DataDir());
            Assert.Equal(RadioStationState.Normal, catalog.GetStationState(RadioStationCatalog.StationCivilDefense));

            catalog.SetStationState(RadioStationCatalog.StationCivilDefense, RadioStationState.Silent);
            Assert.Equal(RadioStationState.Silent, catalog.GetStationState(RadioStationCatalog.StationCivilDefense));

            var exported = catalog.ExportOverrides();
            Assert.True(exported.ContainsKey(RadioStationCatalog.StationCivilDefense));
            Assert.Equal(RadioStationState.Silent, exported[RadioStationCatalog.StationCivilDefense]);

            catalog.ResetOverrides();
            Assert.Equal(RadioStationState.Normal, catalog.GetStationState(RadioStationCatalog.StationCivilDefense));

            catalog.ImportOverrides(exported);
            Assert.Equal(RadioStationState.Silent, catalog.GetStationState(RadioStationCatalog.StationCivilDefense));
        }

        [Fact]
        public void RadioStationCatalog_NoHardcodedStationDefaultsInCore_AuthorityGate()
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
    }
}
