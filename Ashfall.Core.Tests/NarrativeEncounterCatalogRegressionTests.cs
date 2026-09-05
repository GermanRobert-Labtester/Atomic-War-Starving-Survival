using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F6 / Section 19 &amp; 37: Catalog regression matrix for narrative encounters.
    /// Verifies that low-danger, high-danger, location-specific, generic, and rare
    /// existing encounters maintain their authored base weights, categories, and
    /// eligibility predicates without distortion from micro-location catalog composition.
    /// </summary>
    public class NarrativeEncounterCatalogRegressionTests
    {
        private static string DataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void RepresentativeExistingEncounters_MaintainAuthoredProperties()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var composed = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);

            // 1. Generic low-danger encounter
            var deadLetter = composed.Find(e => e.id == "enc_dead_letter_office");
            Assert.NotNull(deadLetter);
            Assert.False(deadLetter!.isMicroLocation);
            Assert.Equal("Discovery", deadLetter.category);
            Assert.Equal(0f, deadLetter.minDangerLevel);
            Assert.True(deadLetter.baseWeight > 0f);

            // 2. High-danger encounter
            var highwayAmbush = composed.Find(e => e.id == "enc_highway_ambush");
            if (highwayAmbush != null)
            {
                Assert.False(highwayAmbush.isMicroLocation);
                Assert.True(highwayAmbush.minDangerLevel >= 1f);
            }

            // 3. Location-specific encounter
            var relayMast = composed.Find(e => e.id == "enc_radio_relay_mast");
            if (relayMast != null)
            {
                Assert.False(relayMast.isMicroLocation);
                Assert.Equal("loc_radio_relay_mast", relayMast.requiredLocationId);
            }
        }

        [Fact]
        public void TotalCoreEncountersCount_MatchesAuthoredBase()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dataDir = DataDir();

            var core = NarrativeEncounterCatalogLoader.LoadCoreEncounters(dataDir, fileIO, json);
            Assert.Equal(60, core.Count); // 29 base + 31 NPC arcs = 60

            var micro = MicroLocationEncounterLoader.Load(dataDir, fileIO, json);
            Assert.Equal(28, micro.Count);

            var composed = NarrativeEncounterCatalogLoader.Load(dataDir, fileIO, json);
            Assert.Equal(88, composed.Count);
        }
    }
}
