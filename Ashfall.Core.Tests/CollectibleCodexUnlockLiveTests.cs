using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Journal;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship XII — live-data codex unlock contract for every collectible
    /// whose effect routes through the journal codex authority
    /// (journal_unlock and faction_info). Targets are enumerated from
    /// collectibles.json — no hardcoded ID list — and each is exercised for:
    /// authored-entry acquisition, duplicate-acquisition idempotency,
    /// save/restore preservation without notification replay, and (for
    /// faction_info) strict standing isolation against FactionWarSystem.
    /// </summary>
    public class CollectibleCodexUnlockLiveTests
    {
        private static readonly string DataDir = FindDataDir();
        private static readonly IFileIO FileIO = new FileSystemIO();
        private static readonly IJsonSerializer Serializer = new SystemTextJsonSerializer();
        private const int FixtureDay = 14;

        private static string FindDataDir()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            while (dir != null)
            {
                string probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "collectibles.json");
                if (File.Exists(probe)) return Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                dir = Path.GetDirectoryName(dir.TrimEnd(Path.DirectorySeparatorChar));
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCatalog() =>
            CollectibleCatalogLoader.Load(DataDir, FileIO, Serializer)
                ?? throw new InvalidOperationException("collectibles.json must load");

        private static List<CollectibleDefinition> CodexCollectibles(string effectType) =>
            LoadCatalog().ByItemId.Values
                .Where(d => d.effect_type == effectType)
                .OrderBy(d => d.item_id, StringComparer.Ordinal)
                .ToList();

        /// <summary>Event counters shared by reference so handlers can mutate
        /// them after this helper returns.</summary>
        private sealed class JournalCounters { public int Entries, Codex, Pings; }

        private static JournalSystem NewJournal(JournalCounters c)
        {
            var journal = new JournalSystem();
            journal.OnEntryAdded += _ => c.Entries++;
            journal.OnCodexUnlocked += _ => c.Codex++;
            journal.OnNotificationPing += _ => c.Pings++;
            return journal;
        }

        private static CollectibleEffectDispatcher NewDispatcher(
            CollectibleCatalog catalog, CollectibleDiscoveryState discovery, JournalSystem journal)
        {
            JournalVoice.BindCatalog(new JournalVoiceProseCatalogLoader(FileIO, Serializer).Load(DataDir));
            return new CollectibleEffectDispatcher(
                catalog, discovery,
                needsProvider: () => null,
                researchProvider: () => null,
                journalProvider: () => journal,
                mapProvider: () => null,
                dayProvider: () => FixtureDay);
        }

        // ── Acquisition: every live codex collectible writes authored content ──

        public static IEnumerable<object[]> CodexEffectTypes()
        {
            yield return new object[] { "journal_unlock" };
            yield return new object[] { "faction_info" };
        }

        [Theory]
        [MemberData(nameof(CodexEffectTypes))]
        public void EveryLiveCodexCollectible_WritesAuthoredEntry_OnFirstAcquisition(string effectType)
        {
            var items = CodexCollectibles(effectType);
            Assert.True(items.Count > 0, $"live data must contain {effectType} collectibles");

            foreach (var def in items)
            {
                var counters = new JournalCounters();
                var journal = NewJournal(counters);
                var dispatcher = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);

                var result = dispatcher.DispatchOnAcquire(def.item_id);

                Assert.True(result.EffectApplied, $"{def.item_id}: effect must apply ({result.FailureReason})");
                Assert.True(result.DiscoveryRegistered, $"{def.item_id}: discovery must register");
                Assert.Equal(1, journal.Knowledge.Has(def.effect_target) ? 1 : 0);
                Assert.Equal(1, journal.EntryCount);
                Assert.Equal(1, counters.Codex);
                Assert.Equal(1, counters.Entries);
                Assert.Equal(1, counters.Pings);

                var entry = journal.Entries.Single(e => e.KnowledgeKey == def.effect_target);
                string expected = JournalVoice.ComposeFullText(def.effect_target, RiskBiasTrait.Realist, FixtureDay);
                Assert.Equal(expected, entry.Text);
                Assert.StartsWith("Day ", entry.Text);
                Assert.DoesNotContain("Something changed. I wrote it down", entry.Text); // no placeholder fallback
            }
        }

        // ── Duplicate acquisition: idempotent, one entry, no replay ──

        [Theory]
        [MemberData(nameof(CodexEffectTypes))]
        public void RepeatAcquisition_IsIdempotent(string effectType)
        {
            foreach (var def in CodexCollectibles(effectType))
            {
                var counters = new JournalCounters();
                var journal = NewJournal(counters);
                var discovery = new CollectibleDiscoveryState();
                var dispatcher = NewDispatcher(LoadCatalog(), discovery, journal);

                dispatcher.DispatchOnAcquire(def.item_id);
                var second = dispatcher.DispatchOnAcquire(def.item_id);

                Assert.True(second.AlreadyDiscovered);
                Assert.False(second.EffectApplied);
                Assert.Equal(1, journal.EntryCount);
                Assert.Equal(1, counters.Codex);
            }
        }

        [Fact]
        public void CodexAlreadyKnowsKey_SecondDiscovery_RegistersWithoutDuplicateEntry()
        {
            // A second acquisition path (e.g. another collectible instance or a
            // post-restore re-acquire with a fresh discovery ledger) must not
            // duplicate the entry, and the dispatch must still count as handled.
            foreach (var def in CodexCollectibles("journal_unlock").Concat(CodexCollectibles("faction_info")))
            {
                var journal = NewJournal(new JournalCounters());
                var first = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
                first.DispatchOnAcquire(def.item_id);
                int entriesBefore = journal.EntryCount;

                var second = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
                var result = second.DispatchOnAcquire(def.item_id);

                Assert.True(result.EffectApplied, $"{def.item_id}: unlock content exists so discovery counts as handled");
                Assert.True(result.DiscoveryRegistered);
                Assert.Equal(entriesBefore, journal.EntryCount);
            }
        }

        // ── Save/restore: unlocks persist, notifications never replay ──

        [Fact]
        public void SaveRestore_PreservesUnlocks_WithoutReplayingNotifications()
        {
            var journal = NewJournal(new JournalCounters());
            var discovery = new CollectibleDiscoveryState();
            var dispatcher = NewDispatcher(LoadCatalog(), discovery, journal);

            var all = CodexCollectibles("journal_unlock").Concat(CodexCollectibles("faction_info")).ToList();
            foreach (var def in all)
                dispatcher.DispatchOnAcquire(def.item_id);
            Assert.Equal(all.Count, journal.EntryCount);

            var save = journal.CaptureState();
            var restoredCounters = new JournalCounters();
            var restored = NewJournal(restoredCounters);
            restored.RestoreState(save);

            Assert.Equal(0, restoredCounters.Entries); // restore reconstructs; it never notifies
            Assert.Equal(0, restoredCounters.Codex);
            Assert.Equal(0, restoredCounters.Pings);
            foreach (var def in all)
                Assert.True(restored.Knowledge.Has(def.effect_target), $"{def.effect_target} must survive restore");

            // A dispatcher backed by the restored discovery ledger must treat
            // re-acquisition as already-discovered: no replay, no new entries.
            var redispatch = NewDispatcher(LoadCatalog(), discovery, restored);
            foreach (var def in all)
            {
                var result = redispatch.DispatchOnAcquire(def.item_id);
                Assert.True(result.AlreadyDiscovered, $"{def.item_id}: must be already discovered after restore");
                Assert.False(result.EffectApplied);
            }
            Assert.Equal(all.Count, restored.EntryCount);
        }

        // ── Standing isolation: faction_info is informational only ──

        [Fact]
        public void FactionInfoAcquisition_DoesNotMutateFactionStanding()
        {
            var journal = NewJournal(new JournalCounters());
            var factionWar = new FactionWarSystem();
            factionWar.ModifyStanding("faction_rebuilders", 20); // unrelated faction with real standing

            var before = factionWar.State.factions.ToDictionary(f => f.factionId, f => f.standing);
            int standingEvents = 0;
            factionWar.OnFactionStandingChanged += (_, _) => standingEvents++;

            var dispatcher = NewDispatcher(LoadCatalog(), new CollectibleDiscoveryState(), journal);
            foreach (var def in CodexCollectibles("faction_info"))
                dispatcher.DispatchOnAcquire(def.item_id);

            var after = factionWar.State.factions.ToDictionary(f => f.factionId, f => f.standing);
            Assert.Equal(before, after); // no faction created, modified, or removed
            Assert.Equal(20, factionWar.GetStanding("faction_rebuilders"));
            Assert.Equal(0, standingEvents);

            // The codex knowledge itself did land — isolation is about
            // diplomacy, not about swallowing the unlock.
            foreach (var def in CodexCollectibles("faction_info"))
                Assert.True(journal.Knowledge.Has(def.effect_target));
        }
    }
}
