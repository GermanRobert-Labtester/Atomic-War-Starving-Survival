// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : DiplomacySelfTest
// Subsystem          : Faction Diplomacy
// ============================================================================

using System;
using Ashfall.Core.Diplomacy;

namespace AtomicWar.GodotApp
{
    public static class HostCliDiplomacy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Faction Diplomacy Self-Test ===");
            int passed = 0;
            const int total = 8;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = DiplomacyHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                if (catalog && session.System.GetTemplate("non_aggression") != null)
                {
                    Console.WriteLine("[PASS] Check 1: Treaty template catalog loaded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog not ready ({catalog}).");
                }

                var rel = session.GetOrCreateRelation("faction_test");
                if (rel.RelationLevel == "neutral")
                {
                    Console.WriteLine("[PASS] Check 2: New relation defaults to neutral.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Relation default mismatch ({rel.RelationLevel}).");
                }

                var unknown = session.ProposeTreaty("faction_test", "no_such_treaty", day: 1, envoySkill: 100);
                if (!unknown.Success && unknown.Treaty == null)
                {
                    Console.WriteLine("[PASS] Check 3: Unknown treaty template is rejected.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Unknown template unexpectedly succeeded.");
                }

                var proposal = session.ProposeTreaty("faction_test", "non_aggression", day: 1, envoySkill: 100);
                if (proposal.Success && proposal.Treaty != null && session.ActiveTreatyCount == 1)
                {
                    Console.WriteLine("[PASS] Check 4: Non-aggression treaty ratified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Treaty proposal failed ({proposal.Message}).");
                }

                session.AssignEnvoy("faction_test", "survivor_envoy");
                var assigned = session.GetOrCreateRelation("faction_test").EnvoyAssigned;
                if (assigned == "survivor_envoy")
                {
                    Console.WriteLine("[PASS] Check 5: Envoy assignment persisted on the relation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Envoy assignment mismatch ({assigned}).");
                }

                var mission = session.DispatchMission("trade_negotiation", "faction_test", "survivor_envoy", day: 2);
                if (mission != null && !string.IsNullOrEmpty(mission.MissionId))
                {
                    Console.WriteLine("[PASS] Check 6: Diplomatic mission dispatched.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Mission dispatch returned null.");
                }

                int repBefore = session.GlobalReputation;
                session.TickDay(2);
                if (session.GlobalReputation == repBefore)
                {
                    Console.WriteLine("[PASS] Check 7: Day tick advanced without corrupting reputation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Reputation drifted without cause ({repBefore} -> {session.GlobalReputation}).");
                }

                bool saved = session.TrySave();
                var reloaded = DiplomacyHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.ActiveTreatyCount == session.ActiveTreatyCount
                    && DiplomacySaveStore.SectionName.Equals("diplomacy", StringComparison.Ordinal)
                    && DiplomacySaveStore.FileName.Equals("diplomacy_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 8: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Faction Diplomacy Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
