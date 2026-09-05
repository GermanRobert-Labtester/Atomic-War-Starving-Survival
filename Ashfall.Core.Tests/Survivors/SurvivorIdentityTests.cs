using System;
using System.IO;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SurvivorIdentityTests : CatalogTestBase
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void ExpansionEnrichmentCatalog_LoadsAuthoredSurvivorFields_AndItemTags()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ExpansionEnrichmentCatalogLoader.Load(dataDir, io, serializer);

            Assert.NotNull(catalog);
            Assert.True(catalog.SurvivorFieldCount >= 70, $"Expected >= 70 survivors with authored enrichment, found {catalog.SurvivorFieldCount}");
            Assert.True(catalog.ItemTagCount >= 60, $"Expected >= 60 tagged items, found {catalog.ItemTagCount}");

            // Verify core canonical survivors have authored beliefs and keepsakes
            var elena = catalog.GetSurvivorFields("elena_vasquez");
            Assert.NotNull(elena);
            Assert.Equal("collectivist_solidarity", elena.belief_profile_id);
            Assert.Equal("worn_stethoscope", elena.personal_keepsake_item_id);
            Assert.Equal("nurse", elena.pre_war_profession_id);

            var marcus = catalog.GetSurvivorFields("marcus_olejnik");
            Assert.NotNull(marcus);
            Assert.Equal("pragmatic_individualism", marcus.belief_profile_id);
            Assert.Equal("tarnished_pocket_watch", marcus.personal_keepsake_item_id);
            Assert.Equal("machinist", marcus.pre_war_profession_id);

            var suki = catalog.GetSurvivorFields("suki_tanaka");
            Assert.NotNull(suki);
            Assert.Equal("superstitious_traditional", suki.belief_profile_id);
            Assert.Equal("family_heirloom_seeds", suki.personal_keepsake_item_id);

            var surgeon = catalog.GetSurvivorFields("the_surgeon");
            Assert.NotNull(surgeon);
            Assert.Equal("atheist_rationalist", surgeon.belief_profile_id);
            Assert.Equal("silver_scalpel", surgeon.personal_keepsake_item_id);
        }

        [Fact]
        public void ExpansionEnrichmentCatalog_QueryMethods_ReturnAccurateLists()
        {
            string dataDir = FindDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var catalog = ExpansionEnrichmentCatalogLoader.Load(dataDir, io, serializer);

            var rationalists = catalog.GetSurvivorsByBeliefProfile("atheist_rationalist");
            Assert.Contains("the_surgeon", rationalists);
            Assert.Contains("the_pharmacist", rationalists);

            var collectivists = catalog.GetSurvivorsByBeliefProfile("collectivist_solidarity");
            Assert.Contains("elena_vasquez", collectivists);
            Assert.Contains("the_therapist", collectivists);

            var individualists = catalog.GetSurvivorsByBeliefProfile("pragmatic_individualism");
            Assert.Contains("marcus_olejnik", individualists);
            Assert.Contains("the_vet", individualists);
        }

        [Fact]
        public void ExpansionEnrichmentCatalog_MissingFile_ReturnsEmptyGracefully()
        {
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var empty = ExpansionEnrichmentCatalogLoader.Load("/nonexistent/data/path", io, serializer);

            Assert.NotNull(empty);
            Assert.Equal(0, empty.SurvivorFieldCount);
            Assert.Equal(0, empty.ItemTagCount);
            Assert.Null(empty.GetSurvivorFields("elena_vasquez"));
        }
    }
}
