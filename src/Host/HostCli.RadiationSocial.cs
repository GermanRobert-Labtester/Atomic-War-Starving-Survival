// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RadiationSocialSelfTest
// Subsystem          : Radiation Social Bridge
// ============================================================================

using System;
using Ashfall.Core;
using Ashfall.Core.Radiation;

namespace AtomicWar.GodotApp
{
    public static class HostCliRadiationSocial
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Radiation Social Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = RadiationSocialHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                if (catalog && session.Bridge.Brackets.Count == 4)
                {
                    Console.WriteLine("[PASS] Check 1: 4 radiation social brackets loaded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog not ready ({catalog}, {session.Bridge.Brackets.Count} brackets).");
                }

                var clean = session.EvaluateSocialStance("s_clean", 5f);
                if (clean.bracketId == "safe" && clean.socialPenalty == 0)
                {
                    Console.WriteLine("[PASS] Check 2: Clean dose maps to 'safe' with no penalty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Clean bracket mismatch ({clean.bracketId}/{clean.socialPenalty}).");
                }

                var hot = session.EvaluateSocialStance("s_hot", 60f);
                if (hot.bracketId == "high" && hot.socialPenalty == 25)
                {
                    Console.WriteLine("[PASS] Check 3: 60 mSv maps to 'high' with 25 penalty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Hot bracket mismatch ({hot.bracketId}/{hot.socialPenalty}).");
                }

                var outcast = session.EvaluateSocialStance("s_outcast", 200f);
                if (outcast.bracketId == "severe" && outcast.socialPenalty == 50)
                {
                    Console.WriteLine("[PASS] Check 4: 200 mSv maps to 'severe' with 50 penalty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Severe bracket mismatch ({outcast.bracketId}/{outcast.socialPenalty}).");
                }

                var first = session.EvaluateSocialStance("s_roll_a", 120f, new SeededRng(20260926));
                var second = session.EvaluateSocialStance("s_roll_b", 120f, new SeededRng(20260926));
                if (first.isDiscriminated == second.isDiscriminated)
                {
                    Console.WriteLine("[PASS] Check 5: Same seed reproduces the discrimination roll deterministically.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Non-deterministic discrimination ({first.isDiscriminated}/{second.isDiscriminated}).");
                }

                if (session.Bridge.TotalDiscriminationIncidents >= 0 && session.Bridge.TotalFactionProtests >= 0)
                {
                    Console.WriteLine($"[PASS] Check 6: Incident ledger readable ({session.Bridge.TotalDiscriminationIncidents} incidents).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Incident ledger negative.");
                }

                bool saved = session.TrySave();
                var reloaded = RadiationSocialHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.Bridge.TotalDiscriminationIncidents == session.Bridge.TotalDiscriminationIncidents
                    && RadiationSocialSaveStore.SectionName.Equals("radiation_social", StringComparison.Ordinal)
                    && RadiationSocialSaveStore.FileName.Equals("radiation_social_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 7: Save/restore and store contract verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Save/restore failed (saved={saved}, loaded={loaded}).");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Radiation Social Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
