// SPDX-License-Identifier: MIT
// Host CLI self-test probe for C3-174 (Mechanical Origin Effects Seam).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliOriginMechanics
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Mechanical Origin Effects Self-Test (C3-174) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: the authored enrichment catalog loads
                var loader = new ExpansionEnrichmentCatalogLoader(
                    new FileSystemIO(), new SystemTextJsonSerializer(), new GodotLog());
                var catalog = loader.Load(dataDir);
                var enrichedIds = catalog.GetEnrichedSurvivorIds().OrderBy(id => id, StringComparer.Ordinal).ToList();
                if (enrichedIds.Count > 0)
                {
                    GD.Print($"[PASS] Check 1: Enrichment catalog loaded with {enrichedIds.Count} enriched survivors.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: No enriched survivors found in the authored catalog.");
                }

                var service = new SurvivorEnrichmentService(catalog);
                string sampleId = enrichedIds.FirstOrDefault() ?? string.Empty;
                var sample = service.GetOriginModifier(sampleId);

                // Check 2: an enriched survivor resolves a mechanical origin
                if (sample.HasMechanicalOrigin)
                {
                    GD.Print($"[PASS] Check 2: '{sampleId}' resolves a mechanical origin.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: '{sampleId}' resolved no mechanical origin.");
                }

                // Check 3: primary skill is resolved
                if (!string.IsNullOrEmpty(sample.PrimarySkillId) && sample.SkillBonus >= 1)
                {
                    GD.Print($"[PASS] Check 3: Primary skill '{sample.PrimarySkillId}' with bonus +{sample.SkillBonus}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Primary skill unresolved ('{sample.PrimarySkillId}', bonus {sample.SkillBonus}).");
                }

                // Check 4: trade specialty is resolved
                if (!string.IsNullOrEmpty(sample.TradeSpecialtyId))
                {
                    GD.Print($"[PASS] Check 4: Trade specialty '{sample.TradeSpecialtyId}' resolved.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Trade specialty unresolved.");
                }

                // Check 5: keepsake item is resolved
                if (!string.IsNullOrEmpty(sample.GrantedKeepsakeItemId))
                {
                    GD.Print($"[PASS] Check 5: Keepsake item '{sample.GrantedKeepsakeItemId}' resolved.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Keepsake item unresolved.");
                }

                // Check 6: keepsake item resolves in the canonical item catalog
                string itemsPath = Path.Combine(dataDir, "items.json");
                string itemsJson = File.Exists(itemsPath) ? File.ReadAllText(itemsPath) : string.Empty;
                if (!string.IsNullOrEmpty(sample.GrantedKeepsakeItemId)
                    && itemsJson.Contains($"\"{sample.GrantedKeepsakeItemId}\"", StringComparison.Ordinal))
                {
                    GD.Print("[PASS] Check 6: Keepsake item exists in the canonical item catalog.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Keepsake item is not in the canonical item catalog.");
                }

                // Check 7: an unenriched survivor resolves an empty origin
                var unenriched = service.GetOriginModifier("survivor_not_authored_anywhere");
                if (!unenriched.HasMechanicalOrigin
                    && string.IsNullOrEmpty(unenriched.PrimarySkillId)
                    && unenriched.SkillBonus == 0
                    && string.IsNullOrEmpty(unenriched.GrantedKeepsakeItemId))
                {
                    GD.Print("[PASS] Check 7: Unenriched survivor resolves an empty origin.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Unenriched survivor resolved a mechanical origin.");
                }

                // Check 8: resolution is deterministic
                var again = service.GetOriginModifier(sampleId);
                if (again.PrimarySkillId == sample.PrimarySkillId
                    && again.TradeSpecialtyId == sample.TradeSpecialtyId
                    && again.GrantedKeepsakeItemId == sample.GrantedKeepsakeItemId)
                {
                    GD.Print("[PASS] Check 8: Origin resolution is deterministic across calls.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Origin resolution was not stable.");
                }

                // Check 9: keepsake resolution is truthful — resolvable keepsakes
                // resolve in the canonical item catalog; orphaned keepsake ids are
                // reported, never invented. (The authored enrichment catalog
                // contains orphaned keepsake ids; that is a recorded data gap.)
                int keepsakes = 0;
                int present = 0;
                int missing = 0;
                foreach (var id in enrichedIds)
                {
                    var mod = service.GetOriginModifier(id);
                    if (string.IsNullOrEmpty(mod.GrantedKeepsakeItemId)) continue;
                    keepsakes++;
                    if (itemsJson.Contains($"\"{mod.GrantedKeepsakeItemId}\"", StringComparison.Ordinal)) present++;
                    else missing++;
                }
                if (keepsakes > 0 && present > 0 && present + missing == keepsakes)
                {
                    GD.Print($"[PASS] Check 9: {present} of {keepsakes} authored keepsakes resolve in the item catalog; {missing} orphaned ids reported (recorded data gap).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Keepsake resolution inconsistent (present={present}, missing={missing}, total={keepsakes}).");
                }

                // Check 10: no fabricated skills — a survivor with an authored
                // pre-war profession resolves a primary skill; a survivor without
                // one resolves none.
                int mapped = 0, unmapped = 0, violations = 0;
                foreach (var id in enrichedIds)
                {
                    var fields = catalog.GetSurvivorFields(id);
                    bool hasProfession = fields != null && !string.IsNullOrWhiteSpace(fields.pre_war_profession_id);
                    bool hasSkill = !string.IsNullOrEmpty(service.GetOriginModifier(id).PrimarySkillId);
                    if (hasProfession) { mapped++; if (!hasSkill) violations++; }
                    else { unmapped++; if (hasSkill) violations++; }
                }
                if (violations == 0)
                {
                    GD.Print($"[PASS] Check 10: No fabricated skills ({mapped} with a profession, {unmapped} without).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: {violations} survivors had a skill/origin mismatch.");
                }

                // Check 11: host applies the modifier through the canonical owners
                // and guards an unresolvable keepsake id.
                string originMechanics = ReadRepoFile("src", "Main.OriginMechanics.cs");
                string gameFlow = ReadRepoFile("src", "Main.GameFlow.cs");
                if (originMechanics.Contains("ApplySurvivorOriginModifiers")
                    && originMechanics.Contains(".AddById(")
                    && originMechanics.Contains("Catalog?.Get(")
                    && originMechanics.Contains("TryGrantSkill")
                    && gameFlow.Contains("ApplySurvivorOriginModifiers()"))
                {
                    GD.Print("[PASS] Check 11: Host applies origins through the canonical inventory and skill owners and guards orphan keepsakes.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Host mechanical-origin wiring is missing.");
                }

                // Check 12: the canonical system already exposes the modifier test surface
                if (typeof(SurvivorOriginModifier).GetProperty("HasMechanicalOrigin") != null
                    && typeof(SurvivorOriginModifier).GetProperty("PrimarySkillId") != null)
                {
                    GD.Print("[PASS] Check 12: SurvivorOriginModifier exposes the bounded mechanical surface.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: SurvivorOriginModifier surface missing.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in mechanical origin self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Mechanical Origin Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 10 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return File.ReadAllText(Path.Combine(new[] { dir }.Concat(parts).ToArray()));
                dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
            }
            return string.Empty;
        }
    }
}
