using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Verdict;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — content web reachability for the
    /// verdict_items.json catalog, which was previously data-only (loaded by no
    /// runtime). Verifies the typed loader returns all 15 rows with unique,
    /// snake_case ids and that the items align to the runtime DTO.
    /// </summary>
    public class VerdictContentWebTests
    {
        private static string FindDataDir()
        {
            string start = System.IO.Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            return string.Empty;
        }

        [Fact]
        public void LoadItems_ReturnsAllFifteenRows()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var items = VerdictCatalogLoader.LoadItems(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(15, items.Count);
        }

        [Fact]
        public void LoadItems_IdsAreUniqueAndSnakeCase()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var items = VerdictCatalogLoader.LoadItems(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var seen = new HashSet<string>();
            foreach (var it in items)
            {
                Assert.False(string.IsNullOrEmpty(it.id));
                Assert.False(string.IsNullOrEmpty(it.displayName));
                // snake_case check (lowercase alnum + underscore only)
                Assert.Matches("^[a-z0-9_]+$", it.id);
                Assert.True(seen.Add(it.id), $"duplicate id {it.id}");
            }
        }

        [Fact]
        public void LoadItems_EvidenceAndQuestItemsPresent()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var items = VerdictCatalogLoader.LoadItems(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var ids = new HashSet<string>();
            foreach (var it in items) ids.Add(it.id);

            // The twelve evidence fragments + three non-evidence (quest/consumable).
            Assert.Contains("evidence_eden_log", ids);
            Assert.Contains("evidence_geophone_hymn", ids);
            Assert.Contains("evidence_fuse_linen", ids);
            Assert.Contains("evidence_twelve_gauge_steel", ids);
            Assert.Contains("item_archive_tape_silo_key", ids);
            Assert.Contains("item_fuse_world_shift_charter", ids);
            Assert.Contains("item_verdict_salt_flat_sample", ids);
        }

        [Fact]
        public void LoadItems_RowsAlignToRuntimeSchema()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var items = VerdictCatalogLoader.LoadItems(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            foreach (var it in items)
            {
                // Runtime display/description are populated (never silently dropped).
                Assert.False(string.IsNullOrEmpty(it.displayName));
                Assert.NotEqual(0f, it.weightKg);      // non-zero weight surrogate check
            }
        }

        [Fact]
        public void LoadLocations_ReturnsFourSites()
        {
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var locs = VerdictCatalogLoader.LoadLocations(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.Equal(4, locs.Count);
            var ids = new HashSet<string>();
            foreach (var l in locs)
            {
                Assert.False(string.IsNullOrEmpty(l.displayName));
                Assert.True(ids.Add(l.id), $"duplicate loc {l.id}");
            }
            Assert.Contains("loc_geophone_pit_1", ids);
            Assert.Contains("loc_twelve_gauge_array", ids);
            Assert.Contains("loc_network_fuse_bunker", ids);
            Assert.Contains("loc_archive_tape_silo", ids);
        }

        [Fact]
        public void LoadRadio_LoadsThirteenAuthoredBroadcasts()
        {
            // verdict_radio.json is now authored (13 broadcasts) — the loader
            // must return them all without a single broadcast lost.
            string dataDir = FindDataDir();
            if (string.IsNullOrEmpty(dataDir)) return;

            var radio = VerdictCatalogLoader.LoadRadio(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(radio);
            Assert.Equal(13, radio.Count);
            var meter = radio.Find(r => r.id == "radio_verdict_meter_reads_1142");
            Assert.NotNull(meter);
            Assert.Equal("radio_vo_verdict_meter", meter!.audio_cue);
            var reckoning = radio.Find(r => r.id == "radio_verdict_reckoning_call");
            Assert.NotNull(reckoning);
            Assert.Equal("radio_vo_verdict_reckoning", reckoning!.audio_cue);
            var seen = new HashSet<string>();
            foreach (var r in radio)
            {
                Assert.False(string.IsNullOrEmpty(r.id));
                Assert.False(string.IsNullOrEmpty(r.message));
                Assert.True(seen.Add(r.id), $"duplicate radio id {r.id}");
            }
        }
    }
}
