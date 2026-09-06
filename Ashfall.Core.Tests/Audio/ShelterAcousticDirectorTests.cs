using System;
using System.Collections.Generic;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    public sealed class ShelterAcousticDirectorTests
    {
        private static ShelterAudioCueCatalog CreateSampleCatalog()
        {
            return new ShelterAudioCueCatalog
            {
                schema_version = 1,
                cues = new List<ShelterAudioCueDefinition>
                {
                    new ShelterAudioCueDefinition
                    {
                        id = "acue_test_generator",
                        bus_id = "generator",
                        playback_mode = "loop",
                        base_volume_db = -6f
                    },
                    new ShelterAudioCueDefinition
                    {
                        id = "acue_test_creak",
                        bus_id = "structural",
                        playback_mode = "one_shot",
                        base_volume_db = -4f
                    },
                    new ShelterAudioCueDefinition
                    {
                        id = "acue_structural_creak",
                        bus_id = "structural",
                        playback_mode = "one_shot",
                        base_volume_db = -4f
                    },
                    new ShelterAudioCueDefinition
                    {
                        id = "acue_falling_dust",
                        bus_id = "structural",
                        playback_mode = "one_shot",
                        base_volume_db = -7f
                    }
                },
                mix_profiles = new List<AcousticMixProfileDefinition>
                {
                    new AcousticMixProfileDefinition
                    {
                        id = "acue_prof_inner_vault",
                        display_name = "Inner Vault",
                        lowpass_cutoff_hz = 9000
                    },
                    new AcousticMixProfileDefinition
                    {
                        id = "acue_prof_airlock",
                        display_name = "Airlock",
                        lowpass_cutoff_hz = 14000
                    },
                    new AcousticMixProfileDefinition
                    {
                        id = "acue_prof_deep_excavation",
                        display_name = "Deep Excavation",
                        lowpass_cutoff_hz = 5500
                    }
                }
            };
        }

        [Fact]
        public void EvaluateSnapshot_CalculatesNormalizedContinuousLayers()
        {
            var director = new ShelterAcousticDirector(CreateSampleCatalog(), new SeededRng(100));
            var facts = new AcousticSimulationFacts
            {
                generatorWattage = 750,
                generatorMaxWattage = 1000,
                ventilationLoadPermille = 500,
                waterPumpLoadPermille = 300,
                ambientRadiationMillisieverts = 4.5f,
                structuralStressPermille = 200,
                hasUnreadRadioBroadcast = true,
                activeZoneOrRoom = "room_main"
            };

            director.UpdateSimulationFacts(facts);
            var snapshot = director.EvaluateSnapshot();

            Assert.Equal(750, snapshot.continuousLayerIntensities["generator"]);
            Assert.Equal(500, snapshot.continuousLayerIntensities["ventilation"]);
            Assert.Equal(300, snapshot.continuousLayerIntensities["machinery"]);
            Assert.Equal(450, snapshot.continuousLayerIntensities["radiation"]);
            Assert.Equal(600, snapshot.continuousLayerIntensities["radio"]);
            Assert.Equal("acue_prof_inner_vault", snapshot.activeMixProfileId);
        }

        [Fact]
        public void EvaluateSnapshot_SelectsCorrectEnvironmentalProfiles()
        {
            var director = new ShelterAcousticDirector(CreateSampleCatalog(), new SeededRng(100));

            director.UpdateSimulationFacts(new AcousticSimulationFacts { activeZoneOrRoom = "room_airlock" });
            Assert.Equal("acue_prof_airlock", director.EvaluateSnapshot().activeMixProfileId);

            director.UpdateSimulationFacts(new AcousticSimulationFacts { activeZoneOrRoom = "excavation_sector_04" });
            Assert.Equal("acue_prof_deep_excavation", director.EvaluateSnapshot().activeMixProfileId);
        }

        [Fact]
        public void TriggerSemanticCue_EnqueuesAndEmitsOneShot()
        {
            var director = new ShelterAcousticDirector(CreateSampleCatalog(), new SeededRng(100));
            director.TriggerSemanticCue("acue_test_creak");

            var snapshot = director.EvaluateSnapshot();
            Assert.Contains("acue_test_creak", snapshot.pendingOneShotCues);

            // Subsequent evaluation should have cleared the one-shot
            var nextSnapshot = director.EvaluateSnapshot();
            Assert.DoesNotContain("acue_test_creak", nextSnapshot.pendingOneShotCues);
        }

        [Fact]
        public void HeadlessSafety_RunsWithoutAudioHardwareOrGodot()
        {
            var director = new ShelterAcousticDirector();
            director.UpdateSimulationFacts(new AcousticSimulationFacts
            {
                generatorWattage = 100,
                structuralStressPermille = 950
            });

            var snapshot = director.EvaluateSnapshot();
            Assert.NotNull(snapshot);
            Assert.True(snapshot.continuousLayerIntensities.ContainsKey("generator"));
        }
    }
}
