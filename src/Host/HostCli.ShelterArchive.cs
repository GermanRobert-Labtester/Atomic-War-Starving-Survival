// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ShelterArchiveSelfTest
// Subsystem          : Plan 162 — Shelter History & Archive System
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Journal;
using Ashfall.Core.Memorial;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliShelterArchive
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Shelter History & Archive System Self-Test (Plan 162) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading from archive_categories.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "archive_categories.json");

                var session = ShelterArchiveHostSession.Create(foundingDay: 1);
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.System.AuthoredCategories.Count >= 6)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.System.AuthoredCategories.Count} archive categories.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Category catalog failed to load (count={session.System.AuthoredCategories.Count}).");
                }

                // Check 2: Canonical category definitions verification
                var decCat = session.System.AuthoredCategories.FirstOrDefault(c => c.CategoryId == "cat_decisions");
                var memCat = session.System.AuthoredCategories.FirstOrDefault(c => c.CategoryId == "cat_memorials");
                if (decCat != null && memCat != null && decCat.EntryTypes.Contains("Decision") && memCat.EntryTypes.Contains("Memorial"))
                {
                    Console.WriteLine("[PASS] Check 2: Governance decision and memorial categories verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Required category types missing.");
                }

                // Check 3: Founding day initialization
                if (session.System.FoundingDay == 1 && session.Census.FoundingDay == 1)
                {
                    Console.WriteLine("[PASS] Check 3: Founding day correctly initialized to Day 1.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Founding day initialization mismatch.");
                }

                // Check 4: Record historical discovery/event
                var ev = session.RecordEvent(day: 2, title: "Shelter Sealed", description: "Primary blast doors secured.", type: ArchiveEntryType.Milestone, significance: ArchiveSignificance.Major, tags: new[] { "defense", "shelter" });
                if (ev != null && session.System.EntryCount >= 1 && ev.Title == "Shelter Sealed")
                {
                    Console.WriteLine("[PASS] Check 4: Historical milestone event recorded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Event recording failed.");
                }

                // Check 5: Record governance resolution decision
                var dec = session.RecordEvent(day: 5, title: "Rationing Policy Enacted", description: "Council approved emergency ration quotas.", type: ArchiveEntryType.Decision, significance: ArchiveSignificance.Notable, participantIds: new[] { "council_chair" });
                if (dec != null && dec.Type == ArchiveEntryType.Decision)
                {
                    Console.WriteLine("[PASS] Check 5: Governance resolution decision recorded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Decision recording failed.");
                }

                // Check 6: Record casualty memorial loss with cause and epitaph
                var mem = new MemorialEntry
                {
                    SurvivorId = "survivor_vance",
                    Day = 12,
                    SurvivedDays = 11,
                    Cause = "ElectricalFire",
                    Epitaph = "Protected the oxygen recirculators.",
                    HeirloomRecipientId = "survivor_mara"
                };
                var memEntry = session.RecordMemorialLoss(mem, dwellerName: "Harlan Vance");
                if (memEntry != null && memEntry.Type == ArchiveEntryType.Memorial && memEntry.Title.Contains("Harlan Vance") && memEntry.Tags.Contains("electricalfire"))
                {
                    Console.WriteLine("[PASS] Check 6: Memorial casualty tribute recorded with tags.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Memorial loss recording failed.");
                }

                // Check 7: Timeline chronological ordering
                session.RecordEvent(day: 20, title: "Hydroponics Harvest", description: "First harvest yielded fresh greens.", type: ArchiveEntryType.Discovery);
                session.RecordEvent(day: 8, title: "Filter Replaced", description: "Exhaust scrubbers replaced.", type: ArchiveEntryType.Event);
                var timeline = session.GetTimeline();
                bool sorted = true;
                for (int i = 1; i < timeline.Count; i++)
                {
                    if (timeline[i].Day < timeline[i - 1].Day) { sorted = false; break; }
                }
                if (sorted && timeline.Count == 5)
                {
                    Console.WriteLine($"[PASS] Check 7: Timeline chronologically sorted across {timeline.Count} entries.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Timeline sorting failed.");
                }

                // Check 8: Keyword search indexing
                var searchResults = session.Search(keyword: "oxygen");
                if (searchResults.Count == 1 && searchResults[0].EntryId == memEntry?.EntryId)
                {
                    Console.WriteLine("[PASS] Check 8: Keyword search accurately resolved target archive entry.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Keyword search query failed.");
                }

                // Check 9: Participant ID search indexing
                var partResults = session.Search(participantId: "council_chair");
                if (partResults.Count == 1 && partResults[0].Title == "Rationing Policy Enacted")
                {
                    Console.WriteLine("[PASS] Check 9: Participant search resolved relevant entry.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Participant search query failed.");
                }

                // Check 10: Canonical projection bridge (Journal + Memorial without parallel ledger)
                var testJournal = new JournalSystem();
                testJournal.TryAddRawEntry("journal_note_1", "Discovered abandoned cache.", null!, 3);
                var projEntries = ShelterArchiveSystem.ProjectCanonicalSources(testJournal, null);
                if (projEntries.Count == 1 && projEntries[0].EntryId.StartsWith("journal:journal_") && projEntries[0].Title == "journal_note_1")
                {
                    Console.WriteLine("[PASS] Check 10: Canonical source projection succeeded without data duplication.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Canonical projection failed.");
                }

                // Check 11: State capture & restore round-trip
                var captured = session.System.CaptureState();
                var restoredSys = new ShelterArchiveSystem();
                restoredSys.RestoreState(captured);
                if (restoredSys.EntryCount == session.System.EntryCount && restoredSys.AuthoredCategories.Count == session.System.AuthoredCategories.Count)
                {
                    Console.WriteLine($"[PASS] Check 11: State capture/restore round-trip verified ({restoredSys.EntryCount} entries).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: State capture/restore mismatch.");
                }

                // Check 12: Checksummed save store serialization and census
                string bareJson = ShelterArchiveSaveStore.TryCapturePersisted(captured);
                var restoredFromSave = ShelterArchiveSaveStore.TryRestorePersisted(bareJson);
                var census = session.Census;
                if (restoredFromSave != null && restoredFromSave.Entries.Count == captured.Entries.Count && census.EntryCount == 5 && census.MemorialCount == 1)
                {
                    Console.WriteLine($"[PASS] Check 12: Checksummed save store serialization verified; census valid (Entries: {census.EntryCount}, Memorials: {census.MemorialCount}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Checksummed save store or census verification failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception in ShelterArchive self-test: {ex.Message}");
            }

            Console.WriteLine($"ShelterArchive Self-Test Result: {passed}/{total} checks passed.");
            return passed == total ? 0 : 1;
        }
    }
}
