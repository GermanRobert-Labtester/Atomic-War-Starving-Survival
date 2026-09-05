using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 81 — chemical recon save/restore parity tests.
    /// Pins: observations/samples/battery round-trip; restore is silent;
    /// lab handoff once survives restore; recapture is normalized.
    /// </summary>
    public class ChemicalReconSaveTests
    {
        private static ToxicChemicalCatalog MakeCatalog()
        {
            return new ToxicChemicalCatalog
            {
                hazard_profiles = new List<ChemicalHazardProfile>
                {
                    new ChemicalHazardProfile
                    {
                        hazard_id = "hazard_save_blister", display_name = "Save Blister",
                        hazard_class = "blister_agent", detector_response_band = "low_band",
                        normalized_concentration = 0.85f, persistence = 0.9f, volatility = 0.3f,
                        wind_response = 0.4f, filter_category = "organic_vapor",
                        filter_load_rate = 0.18f, exposure_severity = 0.85f,
                        sample_value = 30f, detection_threshold = 0.05f, safe_exposure_band = "critical"
                    }
                },
                detector_equipment = new DetectorEquipmentDef
                {
                    detector_bands = new List<string> { "low_band", "medium_band", "wide_band" },
                    base_detection_confidence = 0.85f,
                    battery_ticks_per_charge = 10,
                    per_scan_battery_drain = 1
                },
                sample_collection = new SampleCollectionDef { max_samples_per_mission = 4 },
                filter_model = new FilterModelDef { filter_capacity_base = 100f, incompatible_filter_penalty = 2.5f, breakthrough_warning_threshold = 0.15f },
                map_overlay = new MapOverlayDef { safe_corridor_confidence_required = 0.7f, overlay_persistence_days = 30 }
            };
        }

        private static ChemicalReconEngine Create(int seed)
        {
            return new ChemicalReconEngine(MakeCatalog(), new SeededRng(seed));
        }

        [Fact]
        public void SaveMidExposure_Restore_PreservesDetectorState()
        {
            var sys = Create(10);
            sys.ScanLocation("ruin_1", "low_band");
            sys.CollectSample("hazard_save_blister", "ruin_1", 0.5f, (itemId, n) => true);
            var captured = sys.CaptureState();

            var restored = Create(999); // different seed — state must come from the save
            restored.RestoreState(captured);

            Assert.Equal(sys.State.detectorBatteryRemaining, restored.State.detectorBatteryRemaining);
            Assert.Single(restored.State.hazardObservations);
            Assert.Single(restored.State.collectedSamples);
            Assert.Equal(
                sys.State.hazardObservations[0].confidence,
                restored.State.hazardObservations[0].confidence, 6);
            Assert.Equal(
                sys.State.hazardObservations[0].discoveryState,
                restored.State.hazardObservations[0].discoveryState);
            Assert.Equal("organic_vapor", restored.GetRecommendedFilter("ruin_1"));
            Assert.Equal("critical", restored.GetSafeExposureBand("ruin_1"));
        }

        [Fact]
        public void Restore_ContinuesIdenticalDetectionSequence()
        {
            // Control: uninterrupted scan sequence on the same node.
            var control = Create(10);
            control.ScanLocation("ruin_1", "low_band");
            control.ScanLocation("ruin_1", "low_band");

            // Saved: one scan, restore, one more scan with the same seed.
            var saved = Create(10);
            saved.ScanLocation("ruin_1", "low_band");
            var captured = saved.CaptureState();

            var restored = Create(555);
            restored.RestoreState(captured);
            restored.ScanLocation("ruin_1", "low_band");

            Assert.Equal(
                control.State.hazardObservations[0].confidence,
                restored.State.hazardObservations[0].confidence, 6);
            Assert.Equal(control.State.detectorBatteryRemaining, restored.State.detectorBatteryRemaining);
        }

        [Fact]
        public void LabHandoff_GrantedExactlyOnce_AcrossRestore()
        {
            var sys = Create(10);
            sys.CollectSample("hazard_save_blister", "ruin_1", 0.5f, (itemId, n) => true);
            var sampleId = sys.State.collectedSamples[0].sampleId;
            var captured = sys.CaptureState();

            var restored = Create(77);
            restored.RestoreState(captured);

            Assert.NotNull(restored.DeliverSampleToLab(sampleId));
            Assert.Null(restored.DeliverSampleToLab(sampleId)); // no double unlock
        }

        [Fact]
        public void Restore_IsSilent_NoEvents()
        {
            var sys = Create(10);
            sys.ScanLocation("ruin_1", "low_band");
            var captured = sys.CaptureState();

            int identified = 0, sampled = 0, corridor = 0, changed = 0;
            var restored = Create(88);
            restored.OnHazardIdentified += _ => identified++;
            restored.OnSampleCollected += _ => sampled++;
            restored.OnSafeCorridorDiscovered += _ => corridor++;
            restored.OnReconChanged += () => changed++;
            restored.RestoreState(captured);

            Assert.Equal(0, identified);
            Assert.Equal(0, sampled);
            Assert.Equal(0, corridor);
            Assert.Equal(0, changed);
        }

        [Fact]
        public void Recapture_AfterRestore_IsNormalized()
        {
            var sys = Create(10);
            sys.ScanLocation("ruin_1", "low_band");
            var first = sys.CaptureState();

            var restored = Create(31);
            restored.RestoreState(first);
            var second = restored.CaptureState();

            Assert.Equal(
                new SystemTextJsonSerializer().Serialize(first),
                new SystemTextJsonSerializer().Serialize(second));
        }
    }
}
