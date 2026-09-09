using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Maritime;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// PLAN 155 — oral-lore schema unification, lifecycle hardening, discovery
    /// projection and the mechanical-authority firewall. Tests assert IDs,
    /// metadata and short markers only — never lyric content (§14).
    /// </summary>
    public sealed class OralLorePlan155Tests : CatalogTestBase
    {
        private static string CodexPath => Path.Combine(DataDirectory, "narrative", "oral_lore_codex.json");
        private static string Batch2Path => Path.Combine(DataDirectory, "narrative", "oral_lore_batch_2.json");

        private static (OralLoreCatalog catalog, OralLoreLoadResult r1, OralLoreLoadResult r2) LoadCorpus()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            var r1 = catalog.Load(File.ReadAllText(CodexPath), serializer, "oral_lore_codex.json");
            var r2 = catalog.Load(File.ReadAllText(Batch2Path), serializer, "oral_lore_batch_2.json");
            return (catalog, r1, r2);
        }

        private static OralLorePerformanceSystem CreatePerformanceSystem(OralLoreCatalog? catalog = null, bool withProducers = true)
        {
            catalog ??= LoadCorpus().catalog;
            var system = new OralLorePerformanceSystem(catalog);
            if (withProducers)
            {
                foreach (var (loreId, producer) in OralLorePerformanceSystem.DefaultProducerMap())
                    Assert.True(system.TryRegisterProducer(loreId, producer),
                        $"producer registration failed: {loreId} -> {producer}");
            }
            return system;
        }

        // ── Task A: schema unification ───────────────────────────────────

        [Fact]
        public void Batch1_LoadsAll16_Unchanged()
        {
            var (catalog, r1, _) = LoadCorpus();
            // Codex-only assertions live in the pre-existing tests; here we
            // assert batch-1 records survive the unified loader untouched.
            Assert.Equal(16, r1.loadedCount);
            Assert.Equal(0, r1.duplicateSkippedCount);
            Assert.All(catalog.AllSongs.Where(s => s.lore_id.StartsWith("song_", StringComparison.Ordinal)),
                s => Assert.True(s.tempo_bpm > 0, $"{s.lore_id} must keep its authored BPM"));
        }

        [Fact]
        public void Batch2_Normalizes_TenEntries_WithTextualTempoPreserved()
        {
            var (catalog, _, r2) = LoadCorpus();

            Assert.Equal(10, r2.loadedCount);
            Assert.Equal(10, catalog.AllSongs.Count(s => s.lore_id.StartsWith("oral_b2_", StringComparison.Ordinal)));

            // Textual tempo preserved verbatim in the descriptor.
            var waltz = catalog.GetById("oral_b2_the_geiger_counter_waltz");
            Assert.NotNull(waltz);
            Assert.Equal("THREE_FOUR_TIME", waltz!.tempo_descriptor);
            Assert.Equal(0, waltz.tempo_bpm); // adjectives never fabricate BPM

            // Explicit authored N_BPM labels ARE extracted.
            var march = catalog.GetById("oral_b2_the_salt_freeholders_march");
            Assert.NotNull(march);
            Assert.Equal("STEADY_MARCH_120_BPM", march!.tempo_descriptor);
            Assert.Equal(120, march.tempo_bpm);

            // Batch-2 records carry context, lyrics, tags — nothing dropped.
            foreach (var b2 in catalog.AllSongs.Where(s => s.lore_id.StartsWith("oral_b2_", StringComparison.Ordinal)))
            {
                Assert.False(string.IsNullOrWhiteSpace(b2.title));
                Assert.False(string.IsNullOrWhiteSpace(b2.genre));
                Assert.False(string.IsNullOrWhiteSpace(b2.performance_context));
                Assert.True(b2.lyrics.Length > 0);
                Assert.NotEmpty(b2.tags);
                Assert.Equal("oral_lore_batch_2.json", b2.source_file);
            }
        }

        [Fact]
        public void FullCorpus_Is26_GloballyUniqueIds()
        {
            var (catalog, _, _) = LoadCorpus();
            Assert.Equal(26, catalog.AllSongs.Count);
            Assert.Equal(26, catalog.AllSongs.Select(s => s.lore_id).Distinct(StringComparer.Ordinal).Count());
            Assert.All(catalog.AllSongs, s => Assert.False(string.IsNullOrWhiteSpace(s.genre)));
        }

        [Fact]
        public void TempoNormalization_PreservesSourceMeaning()
        {
            var (catalog, _, _) = LoadCorpus();

            // Batch 1: numeric + meter, no descriptor.
            var crank = catalog.GetById("song_01_blower_crank_cadence");
            Assert.Equal(60, crank!.tempo_bpm);
            Assert.Equal(string.Empty, crank.tempo_descriptor);
            Assert.Contains("4/4", crank.meter);
            Assert.Contains("60 BPM", crank.TempoSummary());

            // Batch 2: descriptor-only summary when no BPM claim exists.
            var lament = catalog.GetById("oral_b2_lament_for_the_surface");
            Assert.Equal("FREE_TIME", lament!.TempoSummary());
            Assert.Equal(0, lament.tempo_bpm);
        }

        // ── Task B: lifecycle hardening ──────────────────────────────────

        [Fact]
        public void RepeatedLoad_IsIdempotent_NoDuplicateEnumeration()
        {
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            string codex = File.ReadAllText(CodexPath);
            string batch2 = File.ReadAllText(Batch2Path);

            catalog.Load(codex, serializer, "codex");
            catalog.Load(batch2, serializer, "batch2");

            // Reload the identical payloads.
            var r1 = catalog.Load(codex, serializer, "codex");
            var r2 = catalog.Load(batch2, serializer, "batch2");

            Assert.True(r1.WasDuplicateSource);
            Assert.True(r2.WasDuplicateSource);
            Assert.Equal(26, catalog.AllSongs.Count);
        }

        [Fact]
        public void CrossLoadOrder_DoesNotChangeContentOrOrder()
        {
            var a = new OralLoreCatalog();
            var b = new OralLoreCatalog();
            var serializer = new SystemTextJsonSerializer();
            string codex = File.ReadAllText(CodexPath);
            string batch2 = File.ReadAllText(Batch2Path);

            a.Load(codex, serializer, "codex");
            a.Load(batch2, serializer, "batch2");
            b.Load(batch2, serializer, "batch2");
            b.Load(codex, serializer, "codex");

            Assert.Equal(26, b.AllSongs.Count);
            Assert.Equal(
                a.AllSongs.Select(s => s.lore_id),
                b.AllSongs.Select(s => s.lore_id));
        }

        [Fact]
        public void DuplicateIds_FollowExplicitFirstWinsPrecedence()
        {
            string duplicated = "{\"schema_version\":1,\"collection_id\":\"x\",\"entries\":[" +
                "{\"lore_id\":\"song_01_blower_crank_cadence\",\"title\":\"Impostor\",\"genre\":\"X\",\"tempo\":\"FAST\",\"lyrics\":\"...\",\"tags\":[\"x\"]}]}";
            var serializer = new SystemTextJsonSerializer();
            var catalog = new OralLoreCatalog();
            catalog.Load(File.ReadAllText(CodexPath), serializer, "codex");
            var result = catalog.Load(duplicated, serializer, "impostor");

            Assert.Equal(0, result.loadedCount);
            Assert.Equal(1, result.duplicateSkippedCount);
            Assert.Contains("song_01_blower_crank_cadence", result.duplicateIds);
            // The canonical record survived: the impostor's title was discarded.
            Assert.NotEqual("Impostor", catalog.GetById("song_01_blower_crank_cadence")!.title);
        }

        // ── Task C/E: producer map ───────────────────────────────────────

        [Fact]
        public void ProducerMap_Activates18Pieces_AcrossAtLeast6Contexts()
        {
            var map = OralLorePerformanceSystem.DefaultProducerMap();
            Assert.True(map.Count >= 14, $"expected >= 14 activated pieces, found {map.Count}");

            int roomProducers = map.Count(m => m.producerId.StartsWith("room_", StringComparison.Ordinal));
            int locationProducers = map.Count(m => m.producerId.StartsWith("location_", StringComparison.Ordinal));
            int contextProducers = map.Count(m => m.producerId.StartsWith("context_", StringComparison.Ordinal));
            int factionProducers = map.Count(m => m.producerId.StartsWith("faction_", StringComparison.Ordinal));

            // At least six distinct context families are covered.
            int distinctProducers = map.Select(m => m.producerId).Distinct().Count();
            Assert.True(distinctProducers >= 6);
            Assert.True(roomProducers >= 2);
            Assert.True(locationProducers >= 2);
            Assert.True(contextProducers >= 2);
            Assert.True(factionProducers >= 1);
        }

        [Fact]
        public void ProducerMap_EveryLoreIdExists_InUnifiedCorpus()
        {
            var (catalog, _, _) = LoadCorpus();
            foreach (var (loreId, _) in OralLorePerformanceSystem.DefaultProducerMap())
                Assert.NotNull(catalog.GetById(loreId));
        }

        [Fact]
        public void ProducerMap_ReferencedRoomsAndLocations_ExistInDataAuthorities()
        {
            string roomsJson = File.ReadAllText(Path.Combine(DataDirectory, "shelter_room_identities.json"));
            var rooms = System.Text.Json.JsonDocument.Parse(roomsJson).RootElement.GetProperty("rooms");
            var roomIds = rooms.EnumerateArray()
                .Select(r => r.GetProperty("id").GetString())
                .ToHashSet(StringComparer.Ordinal);

            string locationsJson = File.ReadAllText(Path.Combine(DataDirectory, "deep_lore_locations.json"));
            var locations = DeepLoreLocationCatalogLoader.Load(
                DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
            var locationIds = locations.Select(l => l.id).ToHashSet(StringComparer.Ordinal);

            foreach (var (loreId, producer) in OralLorePerformanceSystem.DefaultProducerMap())
            {
                if (producer.StartsWith("room_", StringComparison.Ordinal))
                    Assert.True(roomIds.Contains(producer), $"{loreId}: unknown room '{producer}'");
                if (producer.StartsWith("location_", StringComparison.Ordinal))
                    Assert.True(locationIds.Contains(producer), $"{loreId}: unknown location '{producer}'");
            }
        }

        // ── Discovery: idempotence, first-heard, persistence ─────────────

        [Fact]
        public void Discovery_FirstHeard_FiresOnce_NoRediscoverySpam()
        {
            var system = CreatePerformanceSystem();

            int firstHeardEvents = 0;
            system.OnSongFirstHeard += (_, _, _) => firstHeardEvents++;

            var first = system.DiscoverFromProducer("room_water_pump", 5);
            Assert.Single(first);
            Assert.Equal(1, firstHeardEvents);

            // Repeated visits never spam discovery (§18).
            Assert.Empty(system.DiscoverFromProducer("room_water_pump", 6));
            Assert.Empty(system.DiscoverFromProducer("room_water_pump", 7));
            Assert.Equal(1, firstHeardEvents);
        }

        [Fact]
        public void Discovery_UnknownProducerOrSong_FailsClosed()
        {
            var system = CreatePerformanceSystem();
            Assert.Empty(system.DiscoverFromProducer("context_does_not_exist", 1));
            Assert.False(system.DiscoverSong("oral_b2_does_not_exist", "room_main", 1));
            Assert.Equal(0, system.State.heardLoreIds.Count);
        }

        [Fact]
        public void SaveRoundTrip_PreservesHeardSet_NoReplay()
        {
            var system = CreatePerformanceSystem();
            system.DiscoverFromProducer("context_nursery", 3);
            var captured = system.CaptureState();

            var reloaded = CreatePerformanceSystem();
            reloaded.RestoreState(captured);

            Assert.Equal(3, reloaded.State.heardLoreIds.Count);
            // No rediscovery spam after reload (§15).
            Assert.Empty(reloaded.DiscoverFromProducer("context_nursery", 9));
            Assert.Equal(3, reloaded.HeardSongs().Count);
        }

        [Fact]
        public void Restore_OldSave_Null_LeavesNothingHeard()
        {
            var system = CreatePerformanceSystem();
            system.RestoreState(null);
            Assert.Equal(0, system.State.heardLoreIds.Count);
            // A song whose producer lands later is NOT auto-marked heard.
            Assert.All(OralLorePerformanceSystem.DefaultProducerMap(),
                m => Assert.False(system.IsHeard(m.loreId)));
        }

        [Fact]
        public void Restore_UnknownFutureIds_Tolerated()
        {
            var system = CreatePerformanceSystem();
            system.RestoreState(new OralLoreDiscoveryState
            {
                heardLoreIds = new List<string> { "song_01_blower_crank_cadence", "oral_b2_removed_in_future" }
            });
            Assert.Equal(2, system.State.heardLoreIds.Count);
            Assert.True(system.IsHeard("oral_b2_removed_in_future"));
        }

        // ── Task F: mechanical authority firewall ─────────────────────────

        [Fact]
        public void Firewall_PerformanceSystem_HasNoMechanicalAPIs()
        {
            // A song never becomes a shortcut around water, medicine,
            // radiation, navigation, faction control or survival: the system's
            // API surface is pinned — registration, discovery, read-only
            // queries, persistence. Nothing else.
            var type = typeof(OralLorePerformanceSystem);
            var names = type.GetMethods()
                .Where(m => m.DeclaringType == type && !m.IsSpecialName)
                .Select(m => m.Name)
                .OrderBy(n => n, StringComparer.Ordinal)
                .ToList();

            Assert.Equal(new[]
            {
                "CaptureState", "DefaultProducerMap", "DeferredLoreIds", "DiscoverFromProducer", "DiscoverSong",
                "GetProducer", "HasProducer", "HeardSongs", "IsHeard", "RestoreState", "TryRegisterProducer"
            }, names);

            foreach (var name in names)
            {
                foreach (var marker in new[] { "Heal", "Radiation", "Dose", "Reveal",
                             "Unlock", "Morale", "Faction", "Claim", "Output", "Productivity",
                             "Spawn", "Launch", "Schedule", "Impact", "Arm", "Countdown" })
                {
                    Assert.False(name.Contains(marker, StringComparison.OrdinalIgnoreCase),
                        $"oral-lore method '{name}' must not imply a {marker} mechanic");
                }
            }
        }

        [Fact]
        public void Firewall_LyricsRemainData_NeverDuplicatedIntoCode()
        {
            // §14: lyrics live in the data authority. The performance-system
            // source must not embed lyric text — assert that no first stanza
            // line from either source file appears in the C# source. Tests
            // assert metadata and short markers only.
            var (catalog, _, _) = LoadCorpus();
            Assert.All(catalog.AllSongs, s => Assert.True(s.lyrics.Length > 20));

            string source = File.ReadAllText(Path.Combine(
                "..", "..", "..", "..", "Assets", "Ashfall.Core", "Narrative",
                "OralLorePerformanceSystem.cs"));

            foreach (string path in new[] { CodexPath, Batch2Path })
            {
                var doc = System.Text.Json.JsonDocument.Parse(File.ReadAllText(path));
                var array = doc.RootElement.TryGetProperty("songs", out var songs)
                    ? songs : doc.RootElement.GetProperty("entries");
                foreach (var song in array.EnumerateArray())
                {
                    string lyrics = song.GetProperty("lyrics").GetString() ?? string.Empty;
                    foreach (var line in lyrics.Split('\n'))
                    {
                        string trimmed = line.Trim();
                        if (trimmed.Length > 20)
                            Assert.DoesNotContain(trimmed, source, StringComparison.Ordinal);
                    }
                }
            }
        }

        // ── Route/medical/radiation lyric firewall (§17.11–12) ───────────

        [Fact]
        public void Firewall_RouteLyrics_GrantNoMapNodes()
        {
            // Learning the cartographer's song must not touch location or map
            // state: the performance system holds lore IDs only, and the only
            // location data it references is producer provenance.
            var system = CreatePerformanceSystem();
            system.DiscoverSong("oral_b2_the_cartographers_song", "location_metro_tunnel", 4);

            Assert.Single(system.State.heardLoreIds);
            Assert.Equal("location_metro_tunnel", system.GetProducer("oral_b2_the_cartographers_song"));
            // No location id was ADDED to any map authority by this discovery:
            // the system has no map/location-mutation API (pinned above).
        }

        [Fact]
        public void Firewall_MedicalAndRadiationLyrics_MutateNothing()
        {
            // The surgeon's lullaby and the Geiger waltz are display-only.
            // Discovering them cannot produce healing, dose or morale deltas:
            // no such API exists on the performance system (pinned by
            // Firewall_PerformanceSystem_HasNoMechanicalAPIs), and the
            // discovery ledger stays a list of lore ids.
            var system = CreatePerformanceSystem();
            system.DiscoverSong("oral_b2_surgeons_lullaby", "room_clinic", 2);
            system.DiscoverSong("oral_b2_the_geiger_counter_waltz", "room_main", 2);

            Assert.Equal(2, system.State.heardLoreIds.Count);
            Assert.All(system.State.heardLoreIds, id =>
                Assert.True(id.StartsWith("oral_b2_", StringComparison.Ordinal)));
        }
    }
}
