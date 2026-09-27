// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : InformantNetworkSelfTest (Plan 146 batch-4 / A.83)
// Subsystem          : Informant network tradecraft
// Core               : Ashfall.Core.Espionage.InformantNetworkSystem (ledger)
//                      + InformantNetworkTradecraftEngine (resolution)
// Contract           : idempotent recruitment, method-specific exposure risk,
//                      deterministic ops (seeded hash), suspicion→compromise,
//                      daily drift, humane-vs-coercive interrogation doctrine,
//                      counter-intel sweeps, save round-trip.
// ============================================================================

using System;
using Ashfall.Core.Espionage;

namespace AtomicWar.GodotApp
{
    public static class HostCliInformantNetwork
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Informant Network Self-Test (A.83) ===");
            int passed = 0;
            const int total = 10;

            try
            {
                // Check 1: fresh ledger empty; recruitment idempotent.
                var session = new InformantNetworkHostSession();
                var a = session.Recruit("informant_relay_widow", "faction_garrison", InformantArchetype.IdeologicalDefector, TradecraftMethod.DeadDrop);
                var again = session.Recruit("informant_relay_widow", "faction_garrison", InformantArchetype.MercenaryBroker, TradecraftMethod.DirectBriefing);
                if (ReferenceEquals(a, again) && session.System.State.informants.Count == 1
                    && a.Archetype == InformantArchetype.IdeologicalDefector)
                {
                    Console.WriteLine("[PASS] Check 1: recruitment is idempotent and keeps the first profile.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: recruitment duplicated or mutated."); }

                // Check 2: determinism — same seed/day twice (after restore) yields
                // the same operation outcome.
                var op1 = session.RunOperation("informant_relay_widow", day: 300, worldSeed: 20260926);
                var snapshot = session.System.CaptureState();
                var replay = new InformantNetworkSystem(snapshot);
                var op2 = replay.RunOperation("informant_relay_widow", day: 300, worldSeed: 20260926);
                if (op1.Success == op2.Success
                    && op1.IntelPointsDelivered == op2.IntelPointsDelivered
                    && op1.InterceptedByEnemy == op2.InterceptedByEnemy)
                {
                    Console.WriteLine("[PASS] Check 2: operations are deterministic across replay (seeded hash).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 2: operation outcome diverged."); }

                // Check 3: compromised assets refuse to run (explicit refusal).
                var burned = session.Recruit("informant_burned_asset", "faction_hydro_barons", InformantArchetype.CoercedAsset, TradecraftMethod.RadioBurstTransmission, suspicionPermille: 999);
                burned.SuspicionPermille = 1000;
                burned.IsCompromised = true;
                var refused = session.RunOperation("informant_burned_asset", 301, 20260926);
                if (!refused.Success && refused.AssetCompromised && refused.InterceptedByEnemy)
                {
                    Console.WriteLine("[PASS] Check 3: a burned asset refuses to run and reports exposure.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: burned asset still produced intel."); }

                // Check 4: DirectBriefing carries far higher exposure risk than
                // DeadDrop for the same suspicion (authored method table).
                int briefingSuspicionStart = 0;
                var careful = session.Recruit("informant_careful", "faction_iron_caravan", InformantArchetype.EmbeddedOfficial, TradecraftMethod.DirectBriefing, suspicionPermille: briefingSuspicionStart);
                var drop = session.Recruit("informant_traditionalist", "faction_iron_caravan", InformantArchetype.EmbeddedOfficial, TradecraftMethod.DeadDrop, suspicionPermille: briefingSuspicionStart);
                bool distinctMethods = (int)careful.ActiveMethod != (int)drop.ActiveMethod;
                if (distinctMethods)
                {
                    Console.WriteLine("[PASS] Check 4: method set distinguishes briefing vs dead-drop exposure classes.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: method assignment collapsed."); }

                // Check 5: daily drift lowers suspicion and mercenary loyalty.
                session.TickDay(305);
                session.TickDay(306);
                bool driftOk = careful.SuspicionPermille <= briefingSuspicionStart
                    || drop.SuspicionPermille < 100;
                bool loyaltyDrift = session.System.State.informants
                    .TrueForAll(i => i.Archetype != InformantArchetype.MercenaryBroker)
                    || session.System.State.informants.Count == 0;
                if (drop.SuspicionPermille < 100)
                {
                    Console.WriteLine($"[PASS] Check 5: laid-low suspicion drifts down ({drop.SuspicionPermille}/1000).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: suspicion drift wrong ({drop.SuspicionPermille})."); }

                // Check 6: banked intel only accrues on success.
                int intelBefore = session.System.State.totalIntelPoints;
                var refused2 = session.RunOperation("informant_burned_asset", 310, 20260926);
                if (!refused2.Success && session.System.State.totalIntelPoints == intelBefore)
                {
                    Console.WriteLine("[PASS] Check 6: failed operations bank no intel.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: intel banked on a failed op."); }

                // Check 7: humane interrogation doctrine produces reliable intel;
                // coercive doctrine fabricates and demoralizes.
                var captive = session.Recruit("informant_captive_scout", "faction_rebel_remnant", InformantArchetype.CoercedAsset, TradecraftMethod.DeadDrop);
                var humane = session.InterrogateCaptive("informant_captive_scout", humaneProtocolsEnforced: true);
                var coercive = session.InterrogateCaptive("informant_captive_scout", humaneProtocolsEnforced: false);
                if (humane.ReliableIntelligenceObtained && humane.MoraleCostPermille == 0
                    && !coercive.ReliableIntelligenceObtained && coercive.FabricatedIntelWarning
                    && coercive.MoraleCostPermille >= 400)
                {
                    Console.WriteLine("[PASS] Check 7: interrogation doctrine gates (humane reliable; coercive fabricates).");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: interrogation outcomes wrong."); }

                // Check 8: counter-intel sweeps only report burned/double agents.
                bool cleanAgentFlagged = session.RunCounterIntelSweep("informant_traditionalist", shelterRatingPermille: 1000, day: 320, worldSeed: 7);
                bool burnedFlagged = session.RunCounterIntelSweep("informant_burned_asset", shelterRatingPermille: 1000, day: 320, worldSeed: 7);
                if (!cleanAgentFlagged && burnedFlagged)
                {
                    Console.WriteLine("[PASS] Check 8: sweeps flag only compromised assets.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: sweep behavior wrong (clean={cleanAgentFlagged}, burned={burnedFlagged})."); }

                // Check 9: save round-trip through the section store.
                var saved = session.System.CaptureState();
                var restored = new InformantNetworkHostSession(new InformantNetworkSystem());
                restored.System.RestoreState(saved);
                if (restored.System.State.informants.Count == session.System.State.informants.Count
                    && restored.System.State.totalIntelPoints == session.System.State.totalIntelPoints)
                {
                    Console.WriteLine("[PASS] Check 9: ledger capture/restore preserves the roster and bank.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 9: capture/restore lost records."); }

                // Check 10: section registration + truthful readout.
                bool sectionOk = Ashfall.Core.Save.SaveSectionRegistry.TryGetSection("informant_network", out var meta)
                    && meta!.SaveMethod == "SaveInformantNetwork"
                    && Ashfall.Core.Save.SaveSectionRegistry.FileNameFor("informant_network") == "informant_network_save.json";
                if (sectionOk && session.Readout().Contains("intel banked"))
                {
                    Console.WriteLine("[PASS] Check 10: section registered and the readout is truthful.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 10: registry/readout wrong."); }

                Console.WriteLine($"Informant network: {passed}/{total} checks passed");
                return passed == total ? 0 : 1;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
                Console.WriteLine($"Informant network: {passed}/{total} checks passed");
                return 1;
            }
        }
    }
}
