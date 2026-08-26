using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ExpansionEnrichmentCatalogTests
    : CatalogTestBase{
        private static string FindDataDir() => DataDirectory;

        private static ExpansionEnrichmentCatalog LoadReal()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            return loader.Load(FindDataDir());
        }

        [Fact]
        public void Loads_Both_Files()
        {
            var catalog = LoadReal();
            Assert.True(catalog.SurvivorFieldCount > 0, "survivor fields loaded");
            Assert.True(catalog.ItemTagCount > 0, "item tags loaded");
        }

        [Fact]
        public void Survivor_Fields_All_Reference_Known_Survivors()
        {
            var catalog = LoadReal();
            var survivors = json_load_survivors();
            foreach (var sid in catalog.GetEnrichedSurvivorIds())
            {
                Assert.Contains(sid, survivors);
            }
        }

        [Fact]
        public void GetSurvivorFields_Returns_Null_For_Unknown()
        {
            var catalog = LoadReal();
            Assert.Null(catalog.GetSurvivorFields("nonexistent_survivor"));
        }

        [Fact]
        public void GetSurvivorFields_Returns_Data_For_Known_Survivor()
        {
            var catalog = LoadReal();
            var fields = catalog.GetSurvivorFields("elena_vasquez");
            Assert.NotNull(fields);
            Assert.False(string.IsNullOrEmpty(fields!.phantom_background_id));
            Assert.False(string.IsNullOrEmpty(fields.belief_profile_id));
        }

        [Fact]
        public void GetItemTags_Returns_Null_For_Unknown_Item()
        {
            var catalog = LoadReal();
            Assert.Null(catalog.GetItemTags("nonexistent_item"));
        }

        [Fact]
        public void HasTag_Works_Correctly()
        {
            var catalog = LoadReal();
            // teddy_bear is tagged as phantom_childhood and personal_keepsake_candidate
            Assert.True(catalog.HasTag("teddy_bear", "phantom_childhood"));
            Assert.True(catalog.HasTag("teddy_bear", "personal_keepsake_candidate"));
            Assert.False(catalog.HasTag("teddy_bear", "nonexistent_tag"));
            Assert.False(catalog.HasTag("nonexistent_item", "phantom_childhood"));
        }

        [Fact]
        public void GetKeepsakeCandidates_Returns_Items()
        {
            var catalog = LoadReal();
            var keepsakes = catalog.GetKeepsakeCandidates();
            Assert.NotEmpty(keepsakes);
            Assert.Contains("teddy_bear", keepsakes);
            Assert.Contains("worn_photograph", keepsakes);
        }

        [Fact]
        public void GetSurvivorsByBeliefProfile_Returns_Matching_Survivors()
        {
            var catalog = LoadReal();
            // collectivist_solidarity is a known belief profile in the data
            var collective = catalog.GetSurvivorsByBeliefProfile("collectivist_solidarity");
            Assert.NotEmpty(collective);
            Assert.Contains("elena_vasquez", collective);
        }

        [Fact]
        public void GetSurvivorsByPhantomBackground_Returns_Matching_Survivors()
        {
            var catalog = LoadReal();
            var nurses = catalog.GetSurvivorsByPhantomBackground("nurse");
            Assert.NotEmpty(nurses);
            Assert.Contains("elena_vasquez", nurses);
        }

        [Fact]
        public void Loads_From_Missing_Directory_Without_Crash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            var catalog = loader.Load("nonexistent/path");
            Assert.Equal(0, catalog.SurvivorFieldCount);
            Assert.Equal(0, catalog.ItemTagCount);
        }

        [Fact]
        public void All_Enriched_Survivor_Ids_Are_Unique()
        {
            var catalog = LoadReal();
            var ids = catalog.GetEnrichedSurvivorIds().ToList();
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }

        [Fact]
        public void All_Tagged_Item_Ids_Are_Unique()
        {
            var catalog = LoadReal();
            var ids = catalog.GetTaggedItemIds().ToList();
            Assert.Equal(ids.Count, ids.Distinct().Count());
        }

        private static System.Collections.Generic.HashSet<string> json_load_survivors()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string raw = files.ReadAllText(files.Combine(FindDataDir(), "survivors.json"));
            using var doc = JsonDocument.Parse(raw);
            JsonElement array = doc.RootElement;
            if (array.ValueKind == JsonValueKind.Object)
            {
                foreach (var prop in array.EnumerateObject())
                {
                    if (prop.Name.Equals("schema_version", StringComparison.OrdinalIgnoreCase))
                        continue;
                    if (prop.Value.ValueKind == JsonValueKind.Array)
                    {
                        array = prop.Value;
                        break;
                    }
                }
            }
            var entries = json.Deserialize<System.Collections.Generic.List<SurvivorProbe>>(array.GetRawText());
            var ids = new System.Collections.Generic.HashSet<string>();
            foreach (var e in entries!)
                if (!string.IsNullOrEmpty(e.id)) ids.Add(e.id);
            return ids;
        }

        private sealed class SurvivorProbe { public string id = string.Empty; }
    }
}
