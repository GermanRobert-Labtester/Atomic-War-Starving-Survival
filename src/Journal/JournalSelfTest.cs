using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core.Journal;

namespace AtomicWar.Journal
{
    /// <summary>
    /// Headless self-test for the journal domain + save roundtrip. Runs when
    /// the game is launched with `--journal-selftest` (after `--`). Uses only
    /// ids that exist in the StreamingAssets catalogs.
    /// </summary>
    public static class JournalSelfTest
    {
        public static int Run(JournalCatalogs catalogs)
        {
            int passed = 0;
            int total = 0;

            void Check(bool condition, string name)
            {
                total++;
                if (condition)
                {
                    passed++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.Print($"  [FAIL] {name}");
                }
            }

            GD.Print("[JournalSelfTest] begin");

            // --- KnowledgeBase dedupe ---
            var kb = new KnowledgeBase();
            bool first = kb.Discover(KnowledgeKeys.HighCo2);
            bool second = kb.Discover(KnowledgeKeys.HighCo2);
            Check(first && !second && kb.Count == 1, "knowledge dedupe");

            // --- Entry dedupe + flags ---
            var sys = new JournalSystem();
            var author = new DemoSurvivor("elena_vasquez", "Elena Vasquez", RiskBiasTrait.Realist);
            var e1 = sys.TryAddRawEntry("item_seen_dosimeter", "Found one. It still ticks.", author, 2);
            var e2 = sys.TryAddRawEntry("item_seen_dosimeter", "Second copy must not land.", author, 2);
            Check(e1 != null && e2 == null && sys.EntryCount == 1, "entry dedupe by knowledge key");
            Check(sys.HasUnread && sys.NotificationPing && sys.NotificationPingCount == 1, "unread/ping flags after entry");
            Check(e1 != null && e1.AuthorName == "Elena Vasquez" && e1.Day == 2, "entry author + day");

            // --- JournalVoice shape ---
            string voice = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid, 3);
            Check(!string.IsNullOrEmpty(voice) && voice.StartsWith("Day 3."), "voice text shape");

            // --- Tab clamping + unread tracking ---
            sys.SwitchTab(99);
            Check(sys.ActiveTab == JournalSystem.TabCount - 1, "tab clamp on switch");
            sys.SwitchTab(0);
            sys.MarkTabViewed(0);
            Check(!sys.HasUnread && !sys.NotificationPing, "MarkTabViewed clears log unread");
            int itemsSeenAfterEntry = sys.GetLastSeenIndex(1);
            Check(itemsSeenAfterEntry == -1, "fresh tab unseen index");

            // --- Ring cap (uses only real catalog ids) ---
            var realIds = new List<string>();
            if (catalogs != null)
            {
                foreach (var it in catalogs.Items)
                    if (!string.IsNullOrEmpty(it.id)) realIds.Add(KnowledgeKeys.ItemSeen(it.id));
                foreach (var loc in catalogs.Locations)
                    if (!string.IsNullOrEmpty(loc.id)) realIds.Add(KnowledgeKeys.LocationVisited(loc.id));
                foreach (var evt in catalogs.Events)
                    if (!string.IsNullOrEmpty(evt.id)) realIds.Add(KnowledgeKeys.EventFired(evt.id));
            }
            var ringSys = new JournalSystem();
            int pushed = 0;
            for (int i = 0; i < realIds.Count && pushed < 70; i++)
            {
                if (ringSys.TryAddRawEntry(realIds[i], $"Log {pushed}", author, 1) != null)
                    pushed++;
            }
            int expected = Math.Min(70, Math.Min(64, pushed));
            Check(ringSys.EntryCount == 64 && ringSys.EntryCount == expected, "ring caps at 64");

            // --- Save/restore roundtrip ---
            var seeded = new JournalSystem();
            string? firstItemId = catalogs != null && catalogs.Items.Count > 0 ? catalogs.Items[0].id : null;
            if (!string.IsNullOrEmpty(firstItemId))
            {
                seeded.UnlockItemSeen(firstItemId);
                seeded.UnlockItemSeen(firstItemId); // idempotent
                seeded.TryDiscover(KnowledgeKeys.HasExperiencedStorm, author, 5);
                seeded.SwitchTab(2);
                seeded.SwitchTab(3);
            }
            int beforeEntries = seeded.EntryCount;
            int beforeUnlocks = seeded.CodexUnlockCount;
            int beforeTab = seeded.ActiveTab;
            bool beforeUnread = seeded.HasUnread;

            string tmpPath = Path.Combine(
                ProjectSettings.GlobalizePath("user://"), "journal_selftest.json");
            JournalSaveStore.Save(seeded.CaptureState(), tmpPath);
            var loaded = JournalSaveStore.Load(tmpPath);
            Check(loaded != null, "save file loads");

            var restored = new JournalSystem();
            if (loaded != null)
                restored.RestoreState(loaded);
            Check(restored.EntryCount == beforeEntries, "restore entry count");
            Check(restored.CodexUnlockCount == beforeUnlocks && restored.CodexUnlockCount == 1, "restore codex unlocks (idempotent)");
            Check(restored.ActiveTab == beforeTab && restored.ActiveTab == 3, "restore active tab");
            Check(restored.HasUnread == beforeUnread, "restore unread flag");
            Check(restored.IsItemSeen(firstItemId ?? string.Empty),
                "restore knowledge keys");
            Check(restored.Entries.Count > 0 && restored.Entries[0].AuthorName == "Elena Vasquez",
                "restore entries (newest first)");
            // Best-effort cleanup of the roundtrip temp file. A failure here must not
            // fail the suite, but it must not be invisible either — a leaked temp file
            // makes the next run's "save file loads" check misleading.
            try
            {
                File.Delete(tmpPath);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[JournalSelfTest] temp cleanup failed for {tmpPath}: {e.Message}");
            }

            // --- Codex rows: unlocked + locked ---
            if (catalogs != null && catalogs.Items.Count > 1 && catalogs.Locations.Count > 0
                && !string.IsNullOrEmpty(firstItemId))
            {
                var codexSys = new JournalSystem();
                var codex = new JournalCodex(codexSys, catalogs);
                codexSys.UnlockItemSeen(firstItemId);

                var itemRows = codex.BuildRows(JournalTab.Items);
                Check(itemRows.Count == catalogs.Items.Count, "items tab row count");
                bool foundUnlocked = false;
                bool foundLocked = false;
                for (int i = 0; i < itemRows.Count; i++)
                {
                    var row = itemRows[i];
                    if (row.IsLocked && !string.IsNullOrEmpty(row.DisplayName)) foundLocked = true;
                    if (!row.IsLocked && !string.IsNullOrEmpty(row.Body)) foundUnlocked = true;
                }
                Check(foundUnlocked && foundLocked, "items tab unlocked + locked rows");
                Check(codexSys.UnlockItemSeen(firstItemId) == false, "codex unlock idempotent");

                var placeRows = codex.BuildRows(JournalTab.Places);
                bool placeLocked = false;
                for (int i = 0; i < placeRows.Count; i++)
                    if (placeRows[i].IsLocked) placeLocked = true;
                Check(placeRows.Count == catalogs.Locations.Count && placeLocked, "places tab locked silhouettes");
            }
            else
            {
                Check(false, "catalogs populated for codex checks");
            }

            bool ok = passed == total && total > 0;
            return AtomicWar.GodotApp.HostCli.EmitSummary("journal_selftest", ok, ok ? 0 : 1, passed, total - passed);
        }
    }
}
