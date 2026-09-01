using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>Plan 25 · 25G.7: the narrative→mechanical treaty bridge. The
    /// host finally ships a RegionalTreatySystem catalog instead of an empty one.</summary>
    public class RegionalTreatyFeedTests
    {
        [Fact]
        public void Feed_MapsTheSixteenCanonicalTreatiesIntoMechanicalDefinitions()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string path = fileIO.Combine(dataDir, "narrative/regional_treaty_protocols.json");
            Assert.True(fileIO.FileExists(path), path);

            var catalog = new RegionalTreatyCatalog();
            catalog.Load(fileIO.ReadAllText(path), json);

            var definitions = RegionalTreatyFeed.Map(catalog.AllTreaties);
            Assert.Equal(16, definitions.Count);
            Assert.All(definitions, d =>
            {
                Assert.False(string.IsNullOrEmpty(d.treaty_id));
                Assert.False(string.IsNullOrEmpty(d.display_name));
                Assert.Equal(RegionalTreatyFeed.FlatRatificationCostScrap, d.ratification_cost_scrap);
            });

            // First treaty has an authored water quota — carried verbatim as an effect.
            var first = catalog.AllTreaties[0];
            var mapped = definitions.Single(d => d.treaty_id == first.treaty_id);
            if (first.water_allocation_lpm > 0)
                Assert.Contains(mapped.effects, e => e.effect_type == "water_quota" && e.value == first.water_allocation_lpm);
        }

        [Fact]
        public void Feed_MappedDefinitions_ProposeAndRatifyThroughTheMechanicalSystem()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(fileIO.ReadAllText(
                fileIO.Combine(dataDir, "narrative/regional_treaty_protocols.json")), json);

            var system = new RegionalTreatySystem();
            system.LoadCatalog(RegionalTreatyFeed.Map(catalog.AllTreaties));

            var def = RegionalTreatyFeed.Map(catalog.AllTreaties)[0];
            Assert.True(system.Propose(def.treaty_id).Status == ActionResult.StatusKind.Success, "propose succeeds with a fed catalog");
            Assert.True(system.Ratify(def.treaty_id, scrapCost: 10).Status == ActionResult.StatusKind.Success, "ratify succeeds at the flat cost");
            Assert.True(system.IsActive(def.treaty_id), "ratified treaty is active");
        }

        [Fact]
        public void Feed_NullOrEmptyEntries_YieldEmptyList()
        {
            Assert.Empty(RegionalTreatyFeed.Map(null));
            Assert.Empty(RegionalTreatyFeed.Map(new RegionalTreatyEntry[] { null }));
        }

        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }
    }
}
