// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Bestiary;

namespace AtomicWar.GodotApp
{
    public static class BestiarySelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Bestiary & Creature Encounter Tracking Self-Test (Plan 187) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"Bestiary self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Authoritative catalog loading
                var session = BestiaryHostSession.Create(dataDir);
                Check(session.System.Catalog.AllCreatures.Count >= 20,
                    $"Authoritative catalog loaded {session.System.Catalog.AllCreatures.Count} creatures from wasteland_wildlife_bestiary.json.");

                // Check 2: Initial state baseline
                var census0 = session.GetCensus();
                Check(census0.TotalDiscovered == 0 && census0.TotalSightings == 0 && session.System.GetCompletionPercentage() == 0f,
                    "Initial state baseline clean (0 discoveries, 0 sightings, 0.0% completion).");

                // Check 3: Record first encounter & discovery event
                string discoveredCreature = string.Empty;
                session.System.OnCreatureDiscovered += id => discoveredCreature = id;
                var rec = session.RecordEncounter("rad_wolf", day: 1, locationId: "loc_pine_ridge", witnessId: "survivor_scout");
                Check(rec != null && rec.CreatureId == "rad_wolf" && discoveredCreature == "rad_wolf" && rec.UnlockedNoteKeys.Contains("discovery"),
                    $"First encounter registered rad_wolf discovery and unlocked 'discovery' lore note.");

                // Check 4: Sighting record created with metadata
                var sightings = session.System.GetRecentSightings();
                Check(sightings.Count == 1 && sightings[0].LocationId == "loc_pine_ridge" && sightings[0].WitnessSurvivorId == "survivor_scout",
                    "Detailed sighting record saved with location and witness metadata.");

                // Check 5: Three encounters unlock basic stats note
                session.RecordEncounter("rad_wolf", day: 2);
                session.RecordEncounter("rad_wolf", day: 3);
                Check(session.System.IsBasicStatsUnlocked("rad_wolf"),
                    "Reaching 3 encounters unlocked 'basic_stats' lore tier.");

                // Check 6: Five encounters unlock behavior note
                session.RecordEncounter("rad_wolf", day: 4);
                session.RecordEncounter("rad_wolf", day: 5);
                Check(session.System.IsBehaviorUnlocked("rad_wolf"),
                    "Reaching 5 encounters unlocked 'behavior' lore tier.");

                // Check 7: Record kill increments count
                int killObserved = 0;
                session.System.OnCreatureKilled += (id, count) => killObserved = count;
                session.RecordKill("rad_wolf", day: 6, locationId: "loc_pine_ridge");
                Check(rec!.KillCount == 1 && killObserved == 1,
                    $"Creature combat kill recorded (kills={rec.KillCount}).");

                // Check 8: Kill unlocks combat tactics note
                Check(session.System.IsCombatTacticsUnlocked("rad_wolf"),
                    "Combat victory immediately unlocked 'combat_tactics' lore tier.");

                // Check 9: Record butcher unlocks harvest yields note
                session.RecordButcher("rad_wolf", day: 6);
                Check(rec.ButcherCount == 1 && rec.UnlockedNoteKeys.Contains("harvest_yields"),
                    "Butchering specimen unlocked 'harvest_yields' lore tier.");

                // Check 10: Bestiary completion percentage advances
                float completion = session.System.GetCompletionPercentage();
                Check(completion > 0f && completion <= 100f,
                    $"Bestiary completion percentage calculated accurately ({completion:F1}%).");

                // Check 11: Census verification
                var census = session.GetCensus();
                Check(census.TotalDiscovered == 1 && census.TotalSightings == 6 && census.TotalKills == 1 && census.TotalButchered == 1,
                    $"Census metrics match state (Discovered={census.TotalDiscovered}, Sightings={census.TotalSightings}, Kills={census.TotalKills}, Butchered={census.TotalButchered}).");

                // Check 12: Save and restore state fidelity
                var state = session.System.CaptureState();
                var restoredSession = BestiaryHostSession.Create(dataDir, state);
                var restoredRec = restoredSession.System.GetDiscovery("rad_wolf");
                var restoredCensus = restoredSession.GetCensus();
                Check(restoredRec != null &&
                      restoredRec.EncounterCount == rec.EncounterCount &&
                      restoredRec.KillCount == rec.KillCount &&
                      restoredRec.ButcherCount == rec.ButcherCount &&
                      restoredRec.UnlockedNoteKeys.Count == rec.UnlockedNoteKeys.Count &&
                      restoredCensus.TotalDiscovered == census.TotalDiscovered,
                    "Save and restore state verified with full round-trip fidelity.");

                Console.WriteLine($"=== [HostCli] Bestiary Self-Test PASSED ({passed}/12 checks) ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Bestiary self-test threw exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
