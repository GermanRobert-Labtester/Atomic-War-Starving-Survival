// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class AtmosphereTextSystemConsolidationTests
    {
        private static string FindDataDir()
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
        public void AtmosphereCatalogLoader_LoadsBothAtmosphereAndEnvironmentalCatalogs()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var system = AtmosphereCatalogLoader.CreateSystem(dataDir, fileIO, serializer);

            Assert.True(system.Count > 150, $"Expected > 150 entries, got {system.Count}");

            // Verify entry from environmental_atmosphere_expansion.json
            var thermal = system.GetTextForLocation("geothermal_plant_ruins");
            Assert.NotNull(thermal);
            Assert.Contains("sulfur", thermal!.text);

            // Verify entry from consolidated environmental_texts_expansion_05.json
            var bunkerSign = system.GetById("env_bunker_perimeter_sign");
            Assert.NotNull(bunkerSign);
            Assert.Contains("BUNKER PERIMETER", bunkerSign!.text);
            Assert.Equal("bunker_perimeter", bunkerSign.location);
        }

        [Fact]
        public void AtmosphereTextSystem_QueriesEntriesByLocation_ReturnsFlavorText()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var system = AtmosphereCatalogLoader.CreateSystem(dataDir, fileIO, serializer);

            var subway = system.GetTextForLocation("flooded_subway_depot");
            Assert.NotNull(subway);
            Assert.Contains("Water sits in the tunnel mouth", subway!.text);

            var allSubway = system.GetAllTextsForLocation("flooded_subway_depot");
            Assert.NotEmpty(allSubway);

            var warningEntries = system.GetByTag("warning");
            Assert.NotEmpty(warningEntries);
        }

        [Fact]
        public void EnvironmentalTextSystem_AuthorityGate_DuplicateClassesDeleted()
        {
            string start = Directory.GetCurrentDirectory();
            var dir = new DirectoryInfo(start);
            string? coreDir = null;
            while (dir != null)
            {
                string probe = Path.Combine(dir.FullName, "Assets", "Ashfall.Core");
                if (Directory.Exists(probe))
                {
                    coreDir = probe;
                    break;
                }
                dir = dir.Parent;
            }
            Assert.NotNull(coreDir);

            string file1 = Path.Combine(coreDir!, "EnvironmentalTextSystem.cs");
            string file2 = Path.Combine(coreDir!, "EnvironmentalTextCatalogLoader.cs");

            Assert.False(File.Exists(file1), $"Dead duplicate class {file1} should not exist in Core");
            Assert.False(File.Exists(file2), $"Dead duplicate class {file2} should not exist in Core");
        }
    }
}
