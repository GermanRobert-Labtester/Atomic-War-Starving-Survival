using System.IO;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Integration tests proving the three data-wiring catalogs load from the
    /// data authority and expose real content. These seal DATA-WIRE-01/02/03:
    /// QuestlineMasterCatalog, FactionWarContentCatalog, ExpansionEnrichmentCatalog.
    /// </summary>
    public class DataWiringIntegrationTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        // ── QuestlineMasterCatalog (DATA-WIRE-01) ─────────────────────

        [Fact]
        public void QuestlineMaster_LoadsFromDataAuthority()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new QuestlineMasterCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            Assert.True(catalog.Count >= 200, $"Expected >= 200 quest IDs, got {catalog.Count}");
            Assert.True(catalog.All.Count == catalog.Count, "Ordered list must match set count");
        }

        // ── FactionWarContentCatalog (DATA-WIRE-02) ───────────────────

        [Fact]
        public void FactionWarContent_LoadsAllFiveFiles()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            Assert.True(catalog.EventChainCount > 0, "Expected event chains from faction_war_events.json");
            Assert.True(catalog.JournalEntryCount > 0, "Expected journal entries from faction_war_journal.json");
            Assert.True(catalog.BroadcastCount > 0, "Expected broadcasts from faction_war_radio.json");
            Assert.True(catalog.DialogueSnippetCount > 0, "Expected dialogue from faction_war_dialogue.json");
            Assert.True(catalog.CommuniqueCount > 0, "Expected communiques from faction_war_communiques.json");
        }

        [Fact]
        public void FactionWarContent_EligibleChains_RespectsDayGate()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            var early = catalog.GetEligibleChains(0);
            var late = catalog.GetEligibleChains(360);
            Assert.True(late.Count >= early.Count,
                $"Day 360 should have >= chains than day 0 ({late.Count} vs {early.Count})");
        }

        [Fact]
        public void FactionWarContent_LoadsFromMissingDirectoryWithoutCrash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new FactionWarContentCatalogLoader(files, json);
            var catalog = loader.Load("nonexistent/path");

            Assert.Equal(0, catalog.EventChainCount);
            Assert.Equal(0, catalog.JournalEntryCount);
            Assert.Equal(0, catalog.BroadcastCount);
        }

        // ── ExpansionEnrichmentCatalog (DATA-WIRE-03) ─────────────────

        [Fact]
        public void ExpansionEnrichment_LoadsBothFiles()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            Assert.True(catalog.SurvivorFieldCount > 0, "Expected survivor fields from expansion_survivor_fields.json");
            Assert.True(catalog.ItemTagCount > 0, "Expected item tags from expansion_item_tags.json");
        }

        [Fact]
        public void ExpansionEnrichment_SurvivorFields_HaveBeliefProfile()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            int withBelief = 0;
            foreach (var id in catalog.GetEnrichedSurvivorIds())
            {
                var fields = catalog.GetSurvivorFields(id);
                if (fields != null && !string.IsNullOrEmpty(fields.belief_profile_id))
                    withBelief++;
            }
            Assert.True(withBelief > 0, "Expected at least some survivors with belief profiles");
        }

        [Fact]
        public void ExpansionEnrichment_ItemTags_HasTagLookup()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            var catalog = loader.Load(FindDataDir());

            Assert.False(catalog.HasTag("item_nonexistent", "any_tag"));
            Assert.False(catalog.HasTag("", "any_tag"));

            var keepsakes = catalog.GetKeepsakeCandidates();
            Assert.NotNull(keepsakes);
        }

        [Fact]
        public void ExpansionEnrichment_LoadsFromMissingDirectoryWithoutCrash()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            var catalog = loader.Load("nonexistent/path");

            Assert.Equal(0, catalog.SurvivorFieldCount);
            Assert.Equal(0, catalog.ItemTagCount);
        }
    }
}
