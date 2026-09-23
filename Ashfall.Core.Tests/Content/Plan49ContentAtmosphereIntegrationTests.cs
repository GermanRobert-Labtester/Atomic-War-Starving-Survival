// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class Plan49ContentAtmosphereIntegrationTests
    {
        [Fact]
        public void ContentOrphanCertification_CertifiesActiveConsumersAndPermitsPromotion()
        {
            var candidates = new List<ContentCandidateRow>
            {
                new ContentCandidateRow("atm_loc_approach_thermal_plant", "environmental_atmosphere_expansion.json", "AtmosphereTextSystem", true),
                new ContentCandidateRow("med_doc_triage_radiation", "medical_texts.json", "MedicalWardSystem", true),
                new ContentCandidateRow("voice_ration_cut_01", "survivor_voice_lines.json", "SurvivorVoiceSystem", true)
            };

            var activeConsumers = new HashSet<string>(StringComparer.Ordinal)
            {
                "AtmosphereTextSystem",
                "MedicalWardSystem",
                "SurvivorVoiceSystem"
            };

            var report = ContentOrphanCertificationEngine.Certify(candidates, activeConsumers);

            Assert.True(report.IsCertificationClean);
            Assert.True(report.CanPromotePlan49);
            Assert.Equal(3, report.TotalCandidatesEvaluated);
            Assert.Equal(3, report.CertifiedActiveCount);
            Assert.Equal(0, report.OrphanWarningCount);
            Assert.Empty(report.OrphanIds);
        }

        [Fact]
        public void ContentOrphanCertification_FlagsUncomposedConsumerAsOrphan()
        {
            var candidates = new List<ContentCandidateRow>
            {
                new ContentCandidateRow("atm_depot_01", "environmental_atmosphere_expansion.json", "AtmosphereTextSystem", true),
                new ContentCandidateRow("unwired_row_02", "unwired_catalog.json", "UnwiredPhantomSystem", true)
            };

            var activeConsumers = new HashSet<string>(StringComparer.Ordinal)
            {
                "AtmosphereTextSystem"
            };

            var report = ContentOrphanCertificationEngine.Certify(candidates, activeConsumers);

            Assert.False(report.IsCertificationClean);
            Assert.False(report.CanPromotePlan49);
            Assert.Equal(1, report.OrphanWarningCount);
            Assert.Contains("unwired_row_02", report.OrphanIds);
        }

        [Fact]
        public void AtmosphereTextSystem_IndexesEntriesAndResolvesLocation()
        {
            var system = new AtmosphereTextSystem();
            var entries = new List<AtmosphereTextEntry>
            {
                new AtmosphereTextEntry
                {
                    id = "atm_thermal_01",
                    location = "geothermal_plant_ruins",
                    text = "Steam leaks from a cracked valve pit to the left.",
                    weather = "any",
                    condition = "intact",
                    atmosphere = new[] { "industrial", "danger" },
                    tags = new[] { "thermal", "steam" }
                },
                new AtmosphereTextEntry
                {
                    id = "atm_thermal_storm",
                    location = "geothermal_plant_ruins",
                    text = "Ash and hot steam collide in blinding gray plumes.",
                    weather = "ash_storm",
                    condition = "intact",
                    atmosphere = new[] { "danger", "storm" },
                    tags = new[] { "thermal", "storm" }
                },
                new AtmosphereTextEntry
                {
                    id = "atm_subway_01",
                    location = "flooded_subway_depot",
                    text = "Water sits in the tunnel mouth, black and still.",
                    weather = "any",
                    condition = "flooded",
                    atmosphere = new[] { "isolation" },
                    tags = new[] { "flooded" }
                }
            };

            system.LoadCatalog(entries);

            Assert.Equal(3, system.Count);

            // Default location query (prefers weather=any, condition=intact)
            var defaultEntry = system.GetTextForLocation("geothermal_plant_ruins");
            Assert.NotNull(defaultEntry);
            Assert.Equal("atm_thermal_01", defaultEntry.id);

            // Filtered by weather
            var stormEntry = system.GetTextForLocationAndWeather("geothermal_plant_ruins", "ash_storm");
            Assert.NotNull(stormEntry);
            Assert.Equal("atm_thermal_storm", stormEntry.id);

            // Filtered by weather fallback to "any"
            var rainEntry = system.GetTextForLocationAndWeather("geothermal_plant_ruins", "acid_rain");
            Assert.NotNull(rainEntry);
            Assert.Equal("atm_thermal_01", rainEntry.id);

            // Filtered by state
            var floodedEntry = system.GetTextForLocationAndState("flooded_subway_depot", "flooded");
            Assert.NotNull(floodedEntry);
            Assert.Equal("atm_subway_01", floodedEntry.id);

            // By tag and keyword
            var thermalEntries = system.GetByTag("thermal");
            Assert.Equal(2, thermalEntries.Count);

            var dangerEntries = system.GetByAtmosphere("danger");
            Assert.Equal(2, dangerEntries.Count);
        }

        [Fact]
        public void AtmosphereTextSystem_FiresDeliveredSeam()
        {
            var system = new AtmosphereTextSystem();
            AtmosphereTextEntry? delivered = null;
            string deliveredLoc = string.Empty;

            system.AtmosphereTextDeliveredSeam = (loc, entry) =>
            {
                deliveredLoc = loc;
                delivered = entry;
            };

            var entries = new List<AtmosphereTextEntry>
            {
                new AtmosphereTextEntry
                {
                    id = "atm_01",
                    location = "silo_gamma",
                    text = "Cold metal groans under wind.",
                    weather = "any",
                    condition = "intact"
                }
            };

            system.LoadCatalog(entries);
            var result = system.GetTextForLocation("silo_gamma");

            Assert.NotNull(result);
            Assert.NotNull(delivered);
            Assert.Equal("silo_gamma", deliveredLoc);
            Assert.Equal("atm_01", delivered.id);
        }
    }
}
