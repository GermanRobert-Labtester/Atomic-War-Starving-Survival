using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class OralLoreCatalogTests
    : CatalogTestBase{
        private readonly string _catalogPath;

        public OralLoreCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _catalogPath = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative", "oral_lore_codex.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative", "oral_lore_codex.json");
            }
        }

        [Fact]
        public void OralLoreCatalog_LoadsAll16Entries()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file not found at: {_catalogPath}");

            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            catalog.Load(json, serializer);

            Assert.NotNull(catalog);
            Assert.Equal(16, catalog.AllSongs.Count);
        }

        [Fact]
        public void OralLoreCatalog_AllEntriesHaveValidFields()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            catalog.Load(json, serializer);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var song in catalog.AllSongs)
            {
                Assert.False(string.IsNullOrWhiteSpace(song.lore_id), "Missing lore_id");
                Assert.StartsWith("song_", song.lore_id);
                Assert.True(seenIds.Add(song.lore_id), $"Duplicate lore ID: {song.lore_id}");

                Assert.False(string.IsNullOrWhiteSpace(song.title), $"Missing title on {song.lore_id}");
                Assert.False(string.IsNullOrWhiteSpace(song.genre), $"Missing genre on {song.lore_id}");
                Assert.True(song.tempo_bpm > 0, $"Invalid tempo_bpm on {song.lore_id}");
                Assert.False(string.IsNullOrWhiteSpace(song.meter), $"Missing meter on {song.lore_id}");
                Assert.False(string.IsNullOrWhiteSpace(song.performance_context), $"Missing performance_context on {song.lore_id}");
                Assert.False(string.IsNullOrWhiteSpace(song.lyrics), $"Missing lyrics on {song.lore_id}");
                Assert.NotNull(song.tags);
                Assert.NotEmpty(song.tags);
            }
        }

        [Fact]
        public void OralLoreCatalog_QueriesFunctionCorrectly()
        {
            string json = File.ReadAllText(_catalogPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            catalog.Load(json, serializer);

            // By ID
            var blower = catalog.GetById("song_01_blower_crank_cadence");
            Assert.NotNull(blower);
            Assert.Equal("The Blower's Turn (Sixteen to the Minute)", blower.title);
            Assert.Equal("Work Shanty / Pumping Cadence", blower.genre);
            Assert.Equal(60, blower.tempo_bpm);

            // By tag
            var shanties = catalog.GetByTag("work_shanty");
            Assert.NotEmpty(shanties);
            Assert.Contains(shanties, s => s.lore_id == "song_01_blower_crank_cadence");

            // By genre
            var ballads = catalog.GetByGenre("Canal Ballad");
            Assert.NotEmpty(ballads);
        }
    }
}
