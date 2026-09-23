// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 174 (Procedural Survivor Backstories & Origin Mechanics).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliBackstory
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Procedural Survivor Backstories Self-Test (Plan 174) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = BackstoryHostSession.Create(dataDir);
                var occupations = host.System.GetAllOccupations();
                var experiences = host.System.GetAllExperiences();
                var templates = host.System.GetAllTemplates();
                if (occupations.Count >= 8 && experiences.Count >= 5 && templates.Count >= 4)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({occupations.Count} occ, {experiences.Count} exp, {templates.Count} tmpl).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog counts mismatch: {occupations.Count} occ, {experiences.Count} exp, {templates.Count} tmpl.");
                }

                // Check 2: Specific occupations contain skill bonuses and penalties
                var doc = host.System.GetOccupation("occ_doctor");
                var soldier = host.System.GetOccupation("occ_soldier");
                if (doc != null && soldier != null && doc.skill_bonuses.Any(b => b.skill_id == "medical") && soldier.skill_penalties.Any(p => p.skill_id == "social"))
                {
                    GD.Print("[PASS] Check 2: Doctor and Soldier occupations contain valid skill bonuses and penalties.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Occupations missing expected skill bonuses or penalties.");
                }

                // Check 3: Template assignment
                var backstory = host.AssignFromTemplate("survivor_elena", "template_combat_medic", day: 1);
                if (backstory != null && backstory.SurvivorId == "survivor_elena" && backstory.OccupationId == "occ_doctor" && backstory.Secrets.Count > 0)
                {
                    GD.Print($"[PASS] Check 3: Template assignment succeeded for {backstory.SurvivorId} with occ={backstory.OccupationId}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Template assignment failed or missing secrets.");
                }

                // Check 4: Mechanical projection
                var projection = host.ProjectEffects("survivor_elena");
                if (projection != null && projection.SkillBonuses.TryGetValue("medical", out int medBonus) && medBonus > 0)
                {
                    GD.Print($"[PASS] Check 4: Mechanical effects projected medical bonus +{medBonus}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Mechanical projection failed or medical bonus missing.");
                }

                // Check 5: Starting traits and items projection
                if (projection != null && projection.StartingTraits.Count > 0 && projection.StartingItemIds.Count > 0)
                {
                    GD.Print($"[PASS] Check 5: Starting traits ({projection.StartingTraits.Count}) and items ({projection.StartingItemIds.Count}) present.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Starting traits or items missing from projection.");
                }

                // Check 6: Secret revelation
                string secretToReveal = (backstory?.Secrets != null && backstory.Secrets.Count > 0)
                    ? backstory.Secrets[0]
                    : "secret_field_loss";
                bool revealed = host.RevealSecret("survivor_elena", secretToReveal);
                var reloadedBackstory = host.GetBackstory("survivor_elena");
                if (revealed && reloadedBackstory != null && reloadedBackstory.RevealedSecrets.Contains(secretToReveal))
                {
                    GD.Print($"[PASS] Check 6: Secret '{secretToReveal}' successfully revealed.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Secret revelation failed.");
                }

                // Check 7: Duplicate revelation is idempotent
                bool secondReveal = host.RevealSecret("survivor_elena", secretToReveal);
                if (!secondReveal)
                {
                    GD.Print("[PASS] Check 7: Duplicate secret revelation correctly rejected.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Duplicate secret revelation was allowed.");
                }

                // Check 8: Custom backstory assignment
                var custom = host.AssignCustom(
                    "survivor_marcus",
                    "occ_engineer",
                    new[] { "exp_scavenger_ruins" },
                    "Worked in a machine shop",
                    "Repaired the bunker blast valve under fire",
                    "Wants to build a new workshop",
                    day: 2);
                if (custom != null && custom.SurvivorId == "survivor_marcus" && custom.OccupationId == "occ_engineer")
                {
                    GD.Print($"[PASS] Check 8: Custom backstory assigned for {custom.SurvivorId}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Custom backstory assignment failed.");
                }

                // Check 9: Custom backstory projection includes stacked experience bonuses
                var customProj = host.ProjectEffects("survivor_marcus");
                if (customProj != null && customProj.OccupationLabel == "Engineer" && customProj.ExperienceLabels.Count == 1)
                {
                    GD.Print($"[PASS] Check 9: Custom backstory projected occ '{customProj.OccupationLabel}' and {customProj.ExperienceLabels.Count} experience.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Custom backstory projection mismatch.");
                }

                // Check 10: Census reporting
                var census = host.Census;
                if (census.TotalBackstories == 2 && census.TotalRevealedSecrets == 1 && census.TotalOccupations >= 8)
                {
                    GD.Print($"[PASS] Check 10: Census accurate (Total={census.TotalBackstories}, Revealed={census.TotalRevealedSecrets}, Occ={census.TotalOccupations}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census mismatch. Total={census.TotalBackstories}, Revealed={census.TotalRevealedSecrets}.");
                }

                // Check 11: State capture & restore round-trip
                var snapshot = host.CaptureState();
                var freshHost = BackstoryHostSession.Create(dataDir);
                freshHost.RestoreState(snapshot);
                var restoredElena = freshHost.GetBackstory("survivor_elena");
                if (restoredElena != null && restoredElena.RevealedSecrets.Contains(secretToReveal) && freshHost.Census.TotalBackstories == 2)
                {
                    GD.Print("[PASS] Check 11: Host state capture and restore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: State restore did not retain survivor or revealed secrets.");
                }

                // Check 12: Checksummed SaveStore roundtrip
                string persistedJson = BackstorySaveStore.TryCapturePersisted(snapshot);
                var parsedBack = BackstorySaveStore.TryRestorePersisted(persistedJson);
                if (parsedBack != null && parsedBack.Backstories.Count == 2 && parsedBack.SchemaVersion == snapshot.SchemaVersion)
                {
                    GD.Print("[PASS] Check 12: BackstorySaveStore serialization and round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: BackstorySaveStore round-trip failed.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in Backstory self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Backstory Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
