// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : TrophySelfTest
// Subsystem          : Trophy Mount Pipeline
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliTrophy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Trophy Mount Self-Test ===");
            int passed = 0;
            const int total = 7;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = TrophyHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                if (catalog && session.System.Catalog.Count == 11)
                {
                    Console.WriteLine($"[PASS] Check 1: {session.System.Catalog.Count} trophy definitions loaded.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Catalog not ready ({catalog}, {session.System.Catalog.Count}).");
                }

                var def = session.GetTrophy("trophy_wolf_head");
                if (def != null && def.SpeciesId == "wolf" && def.RecipeId == "recipe_trophy_wolf_head")
                {
                    Console.WriteLine("[PASS] Check 2: Wolf trophy definition resolved.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Wolf trophy definition missing.");
                }

                var award = session.RecordQuarryPreserved("wolf", 3);
                if (award != null && award.TrophyId == "trophy_wolf_head")
                {
                    Console.WriteLine("[PASS] Check 3: Quarry preservation awarded the wolf trophy.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Trophy award failed.");
                }

                var again = session.RecordQuarryPreserved("wolf", 4);
                if (again == null)
                {
                    Console.WriteLine("[PASS] Check 4: Trophy award is exactly-once.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Trophy re-awarded.");
                }

                if (session.IsAwarded("trophy_wolf_head") && session.System.UnlockedRecipeIds.Contains("recipe_trophy_wolf_head"))
                {
                    Console.WriteLine("[PASS] Check 5: Award ledger and unlocked recipe agree.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Award ledger/recipe mismatch.");
                }

                if (session.System.Catalog.Count >= session.System.AwardedTrophyIds.Count)
                {
                    Console.WriteLine("[PASS] Check 6: Award count never exceeds catalog.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: More awards than catalog entries.");
                }

                bool saved = session.TrySave();
                var reloaded = TrophyHostSession.Create();
                reloaded.LoadCatalog(data);
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.System.AwardedTrophyIds.Count == 1 && reloaded.IsAwarded("trophy_wolf_head")
                    && TrophySaveStore.SectionName.Equals("trophies", StringComparison.Ordinal)
                    && TrophySaveStore.FileName.Equals("trophies_save.json", StringComparison.Ordinal))
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

            Console.WriteLine($"=== Trophy Mount Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
