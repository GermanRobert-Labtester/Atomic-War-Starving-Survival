// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : MemoryDecaySelfTest
// Subsystem          : Plan 185 — Memory & Knowledge Decay
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Cognition;

namespace AtomicWar.GodotApp
{
    public static class HostCliMemoryDecay
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Memory & Knowledge Decay System Self-Test (Plan 185) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from memory_decay_rates.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "memory_decay_rates.json");

                var session = MemoryDecayHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.Census.DomainRatesCount >= 5)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.Census.DomainRatesCount} memory domain rates.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Memory decay rates catalog failed to load (count={session.Census.DomainRatesCount}).");
                }

                // Check 2: Domain base decay rates verification
                float skillRate = MemoryDecaySystem.GetBaseDailyDecayRate(MemoryDomain.Skill);
                float knowRate = MemoryDecaySystem.GetBaseDailyDecayRate(MemoryDomain.Knowledge);
                float procRate = MemoryDecaySystem.GetBaseDailyDecayRate(MemoryDomain.Procedural);
                if (skillRate == 1.0f && knowRate == 2.5f && procRate == 0.5f)
                {
                    Console.WriteLine("[PASS] Check 2: Domain decay rates verified (Skill: 1.0, Knowledge: 2.5, Procedural: 0.5).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Domain rates mismatch (Skill={skillRate}, Knowledge={knowRate}).");
                }

                // Check 3: Register memory records across domains
                var recSkill = session.RegisterOrUpdate("surv_eva", MemoryDomain.Skill, "radio_repair", 1, 100f);
                var recKnow = session.RegisterOrUpdate("surv_eva", MemoryDomain.Knowledge, "fallout_chemistry", 1, 80f);
                var recRel = session.RegisterOrUpdate("surv_eva", MemoryDomain.Relationship, "bond_with_boris", 1, 60f);
                if (recSkill != null && recKnow != null && recRel != null && session.System.RecordCount >= 3)
                {
                    Console.WriteLine($"[PASS] Check 3: Registered {session.System.RecordCount} memory records across multiple domains.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Memory record registration failed.");
                }

                // Check 4: Memory clarity threshold resolution
                var c100 = MemoryDecaySystem.ResolveClarity(100f);
                var c60 = MemoryDecaySystem.ResolveClarity(60f);
                var c30 = MemoryDecaySystem.ResolveClarity(30f);
                var c10 = MemoryDecaySystem.ResolveClarity(10f);
                var c0 = MemoryDecaySystem.ResolveClarity(0f);
                if (c100 == MemoryClarity.Vivid && c60 == MemoryClarity.Clear && c30 == MemoryClarity.Vague &&
                    c10 == MemoryClarity.Fragmentary && c0 == MemoryClarity.Forgotten)
                {
                    Console.WriteLine("[PASS] Check 4: Clarity ladder resolved (Vivid -> Clear -> Vague -> Fragmentary -> Forgotten).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Clarity resolution ladder failed.");
                }

                // Check 5: Reinforce memory record
                bool reinforced = session.Reinforce("surv_eva", MemoryDomain.Knowledge, "fallout_chemistry", 2, ReinforcementType.Review, 15f);
                var knowMem = session.System.GetMemory("surv_eva", MemoryDomain.Knowledge, "fallout_chemistry");
                if (reinforced && knowMem != null && knowMem.Strength > 80f)
                {
                    Console.WriteLine($"[PASS] Check 5: Memory reinforcement boosted strength to {knowMem.Strength:F1}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Memory reinforcement failed.");
                }

                // Check 6: Daily memory decay tick
                session.TickDay(10);
                var decayedMem = session.System.GetMemory("surv_eva", MemoryDomain.Skill, "radio_repair");
                if (decayedMem != null && decayedMem.Strength < 100f)
                {
                    Console.WriteLine($"[PASS] Check 6: Memory decay applied across elapsed unreinforced days (Strength={decayedMem.Strength:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Daily memory decay tick failed.");
                }

                // Check 7: Certified skill memory decay resistance (90% reduction)
                var certRec = session.RegisterOrUpdate("surv_marcus", MemoryDomain.Skill, "surgical_amputation", 1, 100f, isCertified: true);
                session.TickDay(11);
                var afterCert = session.System.GetMemory("surv_marcus", MemoryDomain.Skill, "surgical_amputation");
                if (afterCert != null && afterCert.IsCertified && afterCert.Strength >= 98.0f)
                {
                    Console.WriteLine($"[PASS] Check 7: Certified skill memory resisted decay ({afterCert.Strength:F2}/100 remaining).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Certified skill decay resistance failed.");
                }

                // Check 8: Preserved memory archive freeze (zero decay)
                var presRec = session.RegisterOrUpdate("surv_marcus", MemoryDomain.EventMemory, "bunker_founding_day", 1, 95f, isPreserved: true);
                session.TickDay(20);
                var afterPres = session.System.GetMemory("surv_marcus", MemoryDomain.EventMemory, "bunker_founding_day");
                if (afterPres != null && afterPres.IsPreserved && afterPres.Strength == 95f)
                {
                    Console.WriteLine("[PASS] Check 8: Preserved memory retained exactly 95.0 strength after 20 days.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Preserved memory archive freeze failed.");
                }

                // Check 9: Memory queries and faded memory filtering
                var memories = session.System.GetSurvivorMemories("surv_eva");
                var faded = session.System.GetFadedMemories("surv_eva", MemoryClarity.Clear);
                if (memories.Count >= 3 && faded != null)
                {
                    Console.WriteLine($"[PASS] Check 9: Survivor queries returned {memories.Count} memories and filtered faded list.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Memory query filtering failed.");
                }

                // Check 10: Census reporting
                var census = session.Census;
                if (census.TotalRecords >= 5 && census.CertifiedCount >= 1 && census.PreservedCount >= 1)
                {
                    Console.WriteLine($"[PASS] Check 10: Cognitive census reported {census.TotalRecords} records (Certified: {census.CertifiedCount}, Preserved: {census.PreservedCount}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Cognitive census reporting failed (Total={census.TotalRecords}).");
                }

                // Check 11: SaveStore persistence round-trip
                var captured = session.CaptureState();
                var restoreSession = MemoryDecayHostSession.Create();
                restoreSession.RestoreState(captured);
                var restoredCensus = restoreSession.Census;
                if (restoredCensus.TotalRecords == census.TotalRecords &&
                    restoredCensus.CertifiedCount == census.CertifiedCount &&
                    restoredCensus.PreservedCount == census.PreservedCount)
                {
                    Console.WriteLine("[PASS] Check 11: SaveStore captured and restored memory decay state cleanly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: SaveStore state restore mismatch.");
                }

                // Check 12: Detached canonical fact projection (Rule 5 compliance)
                var canonicalFacts = new[]
                {
                    new CanonicalMemoryFact { SurvivorId = "surv_clara", Domain = MemoryDomain.Skill, SourceId = "botany", Strength = 90f, LastReinforcedDay = 1 }
                };
                var projected = MemoryDecaySystem.ProjectCanonicalSources(canonicalFacts, currentDay: 5);
                if (projected.Count == 1 && projected[0].SurvivorId == "surv_clara" && projected[0].Strength < 90f)
                {
                    Console.WriteLine($"[PASS] Check 12: Projected canonical memory fact into read-only clarity (Strength={projected[0].Strength:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Canonical memory fact projection failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception during MemoryDecaySelfTest: {ex.Message}");
            }

            Console.WriteLine($"=== MemoryDecaySelfTest: {passed}/{total} checks passed. ===");
            return passed == total ? 0 : 1;
        }
    }
}
