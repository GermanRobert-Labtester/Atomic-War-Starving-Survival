// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Radio;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public class DirectionFindingSystemTests
    {
        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "direction_finding_catalog.json");
                if (File.Exists(probe))
                    return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Directory.GetParent(dir)?.FullName;
            }

            string cwd = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (File.Exists(Path.Combine(cwd, "direction_finding_catalog.json")))
                return cwd;

            throw new DirectoryNotFoundException(
                "Assets/StreamingAssets/Data/direction_finding_catalog.json not found from " + AppContext.BaseDirectory);
        }

        private static DirectionFindingCatalog LoadCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var dto = DirectionFindingCatalogLoader.Load(FindDataDir(), files, json);
            DirectionFindingCatalogLoader.Validate(dto);
            return DirectionFindingCatalogLoader.Build(dto);
        }

        private static RadioObservation Obs(
            string signalId,
            string stationId,
            float bearing,
            string weather = "Clear",
            float error = 1.0f) =>
            new RadioObservation
            {
                signalId = signalId,
                stationId = stationId,
                day = 1,
                hour = 12f,
                bearingDegrees = bearing,
                errorDegrees = error,
                signalStrength = 0.95f,
                noiseLevel = 0.05f,
                frequencyMhz = 12.5f,
                weatherCondition = weather,
                operatorSkill = 0.9f
            };

        [Fact]
        public void Catalog_Loads_WithArraysAndFingerprints()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Arrays.Count >= 3);
            Assert.True(catalog.FingerprintsBySignal.Count >= 3);
            Assert.NotNull(catalog.GetArrayByStation("station_shelter_primary"));
            Assert.NotNull(catalog.GetFingerprintForSignal("sig_civil_defense"));
        }

        [Fact]
        public void LoadCatalog_RegistersStationBaselines()
        {
            var sys = new SignalTriangulationSystem();
            sys.LoadCatalog(LoadCatalog());
            Assert.True(sys.TryGetStationBaseline("station_shelter_primary", out var shelter));
            Assert.Equal(0f, shelter.xKm);
            Assert.True(sys.TryGetStationBaseline("station_ridge_outrigger", out var ridge));
            Assert.Equal(12f, ridge.xKm);
        }

        [Fact]
        public void ClearWeather_ThreeBaselines_MapsFingerprintLocation()
        {
            var sys = new SignalTriangulationSystem();
            sys.LoadCatalog(LoadCatalog());
            const string signalId = "sig_civil_defense";
            Assert.True(sys.RecordObservation(Obs(signalId, "station_shelter_primary", 40f)));
            Assert.True(sys.RecordObservation(Obs(signalId, "station_ridge_outrigger", 95f)));
            Assert.True(sys.RecordObservation(Obs(signalId, "station_long_haul_relay", 150f)));

            var candidate = sys.Triangulate(signalId, new SeededRng(88));
            Assert.NotNull(candidate);
            Assert.True(candidate!.confidence >= SignalTriangulationSystem.ConfidenceThreshold);
            Assert.True(candidate.identityConfidence >= 0.8f);
            Assert.Equal("loc_broadcast_bunker_echo", candidate.locationId);
            Assert.True(sys.IsLocationDiscovered("loc_broadcast_bunker_echo"));
        }

        [Fact]
        public void NightSkywave_IncreasesUncertainty_VersusClear()
        {
            var catalog = LoadCatalog();
            const string signalId = "sig_civil_defense";

            var clear = new SignalTriangulationSystem();
            clear.LoadCatalog(catalog);
            clear.RecordObservation(Obs(signalId, "station_shelter_primary", 40f, "Clear"));
            clear.RecordObservation(Obs(signalId, "station_ridge_outrigger", 95f, "Clear"));
            clear.RecordObservation(Obs(signalId, "station_long_haul_relay", 150f, "Clear"));
            var clearFix = clear.Triangulate(signalId, new SeededRng(88));

            var night = new SignalTriangulationSystem();
            night.LoadCatalog(catalog);
            night.RecordObservation(Obs(signalId, "station_shelter_primary", 40f, "Clear"));
            night.RecordObservation(Obs(signalId, "station_ridge_outrigger", 95f, "Clear"));
            night.RecordObservation(Obs(signalId, "station_long_haul_relay", 150f, "Night"));
            var nightFix = night.Triangulate(signalId, new SeededRng(88));

            Assert.NotNull(clearFix);
            Assert.NotNull(nightFix);
            Assert.True(
                nightFix!.uncertaintyRadiusKm > clearFix!.uncertaintyRadiusKm
                || nightFix.confidence < clearFix.confidence);
        }

        [Fact]
        public void IdentityOnlyFingerprint_DoesNotInventMappedLocation()
        {
            var sys = new SignalTriangulationSystem();
            sys.LoadCatalog(LoadCatalog());
            const string signalId = "sig_numbers";
            sys.RecordObservation(Obs(signalId, "station_shelter_primary", 10f));
            sys.RecordObservation(Obs(signalId, "station_ridge_outrigger", 80f));
            sys.RecordObservation(Obs(signalId, "station_long_haul_relay", 140f));

            var candidate = sys.Triangulate(signalId, new SeededRng(7));
            Assert.NotNull(candidate);
            Assert.True(candidate!.identityConfidence >= 0.8f);
            Assert.Equal("triangulated_" + signalId, candidate.locationId);
        }

        [Fact]
        public void ObservationJitter_SameSeed_ReplaysExactly()
        {
            var catalog = LoadCatalog();
            var obs = Obs("sig_distress", "station_shelter_primary", 77f, "Night", error: 2f);

            var a = new SignalTriangulationSystem();
            a.LoadCatalog(catalog);
            Assert.True(a.RecordObservation(obs, new SeededRng(201)));

            var b = new SignalTriangulationSystem();
            b.LoadCatalog(catalog);
            Assert.True(b.RecordObservation(obs, new SeededRng(201)));

            Assert.Equal(a.Observations[0].bearingDegrees, b.Observations[0].bearingDegrees);
            Assert.Equal(a.Observations[0].errorDegrees, b.Observations[0].errorDegrees);
        }

        [Fact]
        public void CaptureRestore_PreservesBaselinesObservationsAndDiscoveries()
        {
            var sys = new SignalTriangulationSystem();
            sys.LoadCatalog(LoadCatalog());
            const string signalId = "sig_distress";
            sys.RecordObservation(Obs(signalId, "station_shelter_primary", 30f));
            sys.RecordObservation(Obs(signalId, "station_ridge_outrigger", 100f));
            sys.RecordObservation(Obs(signalId, "station_long_haul_relay", 160f));
            var fix = sys.Triangulate(signalId, new SeededRng(11));
            Assert.NotNull(fix);
            Assert.Equal("loc_hidden_relay_bunker", fix!.locationId);

            var snapshot = sys.CaptureState();
            var restored = new SignalTriangulationSystem();
            restored.LoadCatalog(LoadCatalog());
            restored.RestoreState(snapshot);

            Assert.Equal(3, restored.GetObservationCount(signalId));
            Assert.True(restored.IsLocationDiscovered("loc_hidden_relay_bunker"));
            Assert.True(restored.TryGetStationBaseline("station_ridge_outrigger", out _));
        }

        [Fact]
        public void CampaignStreamIds_DfStreams_AreSnakeCase()
        {
            Assert.Equal("df_skywave_jitter", CampaignStreamIds.DfSkywaveJitter);
            Assert.Equal("df_false_signature", CampaignStreamIds.DfFalseSignature);
        }
    }
}
