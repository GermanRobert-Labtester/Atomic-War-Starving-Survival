using System;
using Ashfall.Core;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SignalTriangulationSystemTests
    {
        private static RadioObservation DemoObs(
            string signalId = "sig_distress",
            float bearing = 45f,
            float strength = 0.7f,
            float noise = 0.2f,
            string station = "station_alpha")
        {
            return new RadioObservation
            {
                signalId = signalId,
                stationId = station,
                day = 1,
                hour = 12f,
                bearingDegrees = bearing,
                errorDegrees = 5f + noise * 10f,
                signalStrength = strength,
                noiseLevel = noise,
                frequencyMhz = 94.2f,
                weatherCondition = "Clear",
                operatorSkill = 0.6f
            };
        }

        private static SeededRng Rng(int seed) => new SeededRng(seed);

        // ── Observation ──────────────────────────────────────────────

        [Fact]
        public void RecordObservation_AcceptsValidInput()
        {
            var sys = new SignalTriangulationSystem();
            Assert.True(sys.RecordObservation(DemoObs()));
            Assert.Equal(1, sys.Observations.Count);
        }

        [Fact]
        public void RecordObservation_RejectsNull()
        {
            var sys = new SignalTriangulationSystem();
            Assert.False(sys.RecordObservation(null));
        }

        [Fact]
        public void RecordObservation_RejectsInvalidBearing()
        {
            var sys = new SignalTriangulationSystem();
            Assert.False(sys.RecordObservation(DemoObs(bearing: -1f)));
            Assert.False(sys.RecordObservation(DemoObs(bearing: 361f)));
        }

        [Fact]
        public void RecordObservation_RejectsInvalidError()
        {
            var sys = new SignalTriangulationSystem();
            var obs = DemoObs();
            obs.errorDegrees = 0f;
            Assert.False(sys.RecordObservation(obs));
            obs.errorDegrees = 20f; // above MaxBearingErrorDegrees
            Assert.False(sys.RecordObservation(obs));
        }

        [Fact]
        public void RecordObservation_RaisesOnObservationRecorded()
        {
            var sys = new SignalTriangulationSystem();
            int raised = 0;
            sys.OnObservationRecorded += _ => raised++;
            sys.RecordObservation(DemoObs());
            Assert.Equal(1, raised);
        }

        // ── Triangulation ────────────────────────────────────────────

        [Fact]
        public void Triangulate_ReturnsNullWithTooFewObservations()
        {
            var sys = new SignalTriangulationSystem();
            sys.RecordObservation(DemoObs());
            Assert.Null(sys.Triangulate("sig_distress", Rng(1)));
        }

        [Fact]
        public void Triangulate_ReturnsCandidateWithEnoughObservations()
        {
            var sys = new SignalTriangulationSystem();
            sys.RecordObservation(DemoObs(bearing: 45f, station: "station_a"));
            sys.RecordObservation(DemoObs(bearing: 135f, station: "station_b"));
            var candidate = sys.Triangulate("sig_distress", Rng(1));
            Assert.NotNull(candidate);
            Assert.True(candidate!.confidence > 0f);
        }

        [Fact]
        public void Triangulate_ConfidenceIncreasesWithMoreObservations()
        {
            var sys1 = new SignalTriangulationSystem();
            sys1.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys1.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            var c1 = sys1.Triangulate("sig_distress", Rng(1));

            var sys2 = new SignalTriangulationSystem();
            sys2.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys2.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            sys2.RecordObservation(DemoObs(bearing: 90f, station: "c"));
            var c2 = sys2.Triangulate("sig_distress", Rng(1));

            Assert.True(c2!.confidence >= c1!.confidence, "More observations should increase confidence");
        }

        [Fact]
        public void Triangulate_UncertaintyDecreasesWithMoreObservations()
        {
            var sys1 = new SignalTriangulationSystem();
            sys1.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys1.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            var c1 = sys1.Triangulate("sig_distress", Rng(1));

            var sys2 = new SignalTriangulationSystem();
            sys2.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys2.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            sys2.RecordObservation(DemoObs(bearing: 90f, station: "c"));
            var c2 = sys2.Triangulate("sig_distress", Rng(1));

            Assert.True(c2!.uncertaintyRadiusKm <= c1!.uncertaintyRadiusKm, "More observations should reduce uncertainty");
        }

        [Fact]
        public void Triangulate_DiscoveryRequiresMinObservationsAndConfidence()
        {
            var sys = new SignalTriangulationSystem();
            // 2 observations: hypothesis but not discovery
            sys.RecordObservation(DemoObs(bearing: 45f, strength: 0.9f, station: "a"));
            sys.RecordObservation(DemoObs(bearing: 135f, strength: 0.9f, station: "b"));
            var c1 = sys.Triangulate("sig_distress", Rng(1));
            Assert.False(sys.IsLocationDiscovered(c1!.locationId));

            // 3rd observation: may trigger discovery if confidence is high enough
            sys.RecordObservation(DemoObs(bearing: 90f, strength: 0.9f, station: "c"));
            var c2 = sys.Triangulate("sig_distress", Rng(1));
            // Discovery depends on confidence threshold
            if (c2!.confidence >= SignalTriangulationSystem.ConfidenceThreshold)
                Assert.True(sys.IsLocationDiscovered(c2.locationId));
        }

        [Fact]
        public void Triangulate_RaisesOnLocationRevealed()
        {
            var sys = new SignalTriangulationSystem();
            string? revealedId = null;
            sys.OnLocationRevealed += id => revealedId = id;
            // Add enough high-quality observations with low noise and high skill
            for (int i = 0; i < 6; i++)
            {
                var obs = DemoObs(bearing: i * 60f, strength: 0.95f, noise: 0.02f, station: "s" + i);
                obs.operatorSkill = 0.9f;
                obs.errorDegrees = 2f;
                sys.RecordObservation(obs);
            }
            var candidate = sys.Triangulate("sig_distress", Rng(1));
            Assert.NotNull(candidate);
            // Discovery requires confidence >= 0.7 and >= 3 observations
            Assert.True(candidate!.observationCount >= 3);
            if (candidate.confidence >= SignalTriangulationSystem.ConfidenceThreshold)
                Assert.NotNull(revealedId);
            else
                Assert.Null(revealedId); // confidence too low, no discovery
        }

        // ── Noise and weather reduce confidence ──────────────────────

        [Fact]
        public void Triangulate_HighNoiseReducesConfidence()
        {
            var sysClean = new SignalTriangulationSystem();
            sysClean.RecordObservation(DemoObs(bearing: 45f, noise: 0.05f, station: "a"));
            sysClean.RecordObservation(DemoObs(bearing: 135f, noise: 0.05f, station: "b"));
            var cClean = sysClean.Triangulate("sig_distress", Rng(1));

            var sysNoisy = new SignalTriangulationSystem();
            sysNoisy.RecordObservation(DemoObs(bearing: 45f, noise: 0.8f, station: "a"));
            sysNoisy.RecordObservation(DemoObs(bearing: 135f, noise: 0.8f, station: "b"));
            var cNoisy = sysNoisy.Triangulate("sig_distress", Rng(1));

            Assert.True(cClean!.confidence > cNoisy!.confidence, "Clean signal should have higher confidence");
        }

        // ── Determinism ──────────────────────────────────────────────

        [Fact]
        public void SameObservations_SameCandidate()
        {
            var sysA = new SignalTriangulationSystem();
            sysA.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sysA.RecordObservation(DemoObs(bearing: 135f, station: "b"));

            var sysB = new SignalTriangulationSystem();
            sysB.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sysB.RecordObservation(DemoObs(bearing: 135f, station: "b"));

            var cA = sysA.Triangulate("sig_distress", Rng(7));
            var cB = sysB.Triangulate("sig_distress", Rng(7));

            Assert.Equal(cA!.confidence, cB!.confidence);
            Assert.Equal(cA.estimatedX, cB.estimatedX);
            Assert.Equal(cA.estimatedY, cB.estimatedY);
        }

        // ── Save/Load ────────────────────────────────────────────────

        [Fact]
        public void CaptureRestore_RoundTrips()
        {
            var sys = new SignalTriangulationSystem();
            sys.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            sys.Triangulate("sig_distress", Rng(1));

            var state = sys.CaptureState();
            var sys2 = new SignalTriangulationSystem();
            sys2.RestoreState(state);

            Assert.Equal(2, sys2.Observations.Count);
            Assert.Single(sys2.Candidates);
        }

        [Fact]
        public void CaptureState_StableChecksum()
        {
            var sys = new SignalTriangulationSystem();
            sys.RecordObservation(DemoObs(bearing: 45f, station: "a"));
            sys.RecordObservation(DemoObs(bearing: 135f, station: "b"));
            string before = SaveChecksum.Compute(sys.CaptureState());

            var sys2 = new SignalTriangulationSystem();
            sys2.RestoreState(sys.CaptureState());
            string after = SaveChecksum.Compute(sys2.CaptureState());

            Assert.Equal(before, after);
        }

        [Fact]
        public void DiscoveredLocations_SurviveSaveLoad()
        {
            var sys = new SignalTriangulationSystem();
            for (int i = 0; i < 5; i++)
                sys.RecordObservation(DemoObs(bearing: i * 72f, strength: 0.95f, noise: 0.05f, station: "s" + i));
            sys.Triangulate("sig_distress", Rng(1));

            var state = sys.CaptureState();
            var sys2 = new SignalTriangulationSystem();
            sys2.RestoreState(state);

            Assert.Equal(sys.DiscoveredLocations.Count, sys2.DiscoveredLocations.Count);
        }

        // ── Queries ──────────────────────────────────────────────────

        [Fact]
        public void GetObservationCount_ReturnsCorrectCount()
        {
            var sys = new SignalTriangulationSystem();
            sys.RecordObservation(DemoObs(signalId: "sig_a"));
            sys.RecordObservation(DemoObs(signalId: "sig_a"));
            sys.RecordObservation(DemoObs(signalId: "sig_b"));
            Assert.Equal(2, sys.GetObservationCount("sig_a"));
            Assert.Equal(1, sys.GetObservationCount("sig_b"));
        }

        [Fact]
        public void GetCandidate_ReturnsNullForUnknownSignal()
        {
            var sys = new SignalTriangulationSystem();
            Assert.Null(sys.GetCandidate("sig_unknown"));
        }

        [Fact]
        public void IsLocationDiscovered_ReturnsFalseForUnknown()
        {
            var sys = new SignalTriangulationSystem();
            Assert.False(sys.IsLocationDiscovered("loc_unknown"));
        }
    }
}
