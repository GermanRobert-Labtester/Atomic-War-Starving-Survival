// SPDX-License-Identifier: MIT
// Plan 166 — strict shelter-origins loader, census, and schema-gate tests.

using System;
using System.IO;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class ShelterOriginCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string AuthoredJson() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "shelter_origins.json"));

        [Fact]
        public void Authored_Catalog_Loads_And_Binds()
        {
            var catalog = ShelterOriginCatalogLoader.LoadFromJson(AuthoredJson());
            Assert.True(catalog.origins.Count >= 6);

            var system = new ShelterIdentitySystem();
            system.BindOrigins(catalog);
            Assert.Equal(catalog.origins.Count, system.Origins.Count);
            Assert.True(system.Origins.ContainsKey("origin_government_bunker"));
        }

        [Theory]
        [InlineData("{\"schema_version\":1,\"origins\":[{\"origin_id\":\"o1\",\"display_name\":\"O1\",\"starting_bonuses\":[\"b\"]},{\"origin_id\":\"o1\",\"display_name\":\"O2\",\"starting_bonuses\":[\"b\"]}]}")]
        [InlineData("{\"schema_version\":1,\"origins\":[{\"origin_id\":\"o1\",\"display_name\":\"\",\"starting_bonuses\":[\"b\"]}]}")]
        [InlineData("{\"schema_version\":1,\"origins\":[{\"origin_id\":\"o1\",\"display_name\":\"O1\",\"starting_bonuses\":[\"b\"],\"radiation_shielding_bp\":99999}]}")]
        [InlineData("{\"schema_version\":1,\"origins\":[{\"origin_id\":\"o1\",\"display_name\":\"O1\"}]}")]
        [InlineData("{\"schema_version\":2,\"origins\":[{\"origin_id\":\"o1\",\"display_name\":\"O1\",\"starting_bonuses\":[\"b\"]}]}")]
        public void Malformed_Catalog_Is_Rejected(string json)
        {
            Assert.Throws<InvalidOperationException>(() => ShelterOriginCatalogLoader.LoadFromJson(json));
        }

        [Fact]
        public void Census_Reflects_Live_Identity()
        {
            var catalog = ShelterOriginCatalogLoader.LoadFromJson(AuthoredJson());
            var system = new ShelterIdentitySystem();
            system.BindOrigins(catalog);
            system.SelectOrigin("origin_mining_facility", 3, "survivor_ada");
            for (int i = 0; i < 5; i++) system.RecordCommunityAction("trade");

            var census = system.GetCensus();
            Assert.Equal(system.Origins.Count, census.OriginCount);
            Assert.Equal("origin_mining_facility", census.OriginId);
            Assert.Equal(1, census.KnownForTagCount);
            Assert.True(census.CommunityActionPoints >= 5);
        }

        [Fact]
        public void RestoreState_Rejects_Newer_Schema_And_Accepts_Legacy()
        {
            var system = new ShelterIdentitySystem();
            var newer = system.CaptureState();
            newer.schema_version = 2;
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(newer));

            var legacy = system.CaptureState();
            legacy.schema_version = 0;
            system.RestoreState(legacy);
            Assert.Equal(1, system.CaptureState().schema_version);
        }
    }
}
