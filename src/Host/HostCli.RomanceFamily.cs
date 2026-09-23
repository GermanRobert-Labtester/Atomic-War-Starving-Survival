// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 150 (Romance & Family Dynamics).

using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliRomanceFamily
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Romance & Family Dynamics Self-Test (Plan 150) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Strict catalog loading
                var host = RomanceFamilyHostSession.Create(dataDir);
                if (host.UsingAuthoredCatalog && host.Catalog.CourtshipEvents.Count >= 20)
                {
                    GD.Print($"[PASS] Check 1: Authored courtship catalog loaded strictly ({host.Catalog.CourtshipEvents.Count} events).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 20 authored events, got {host.Catalog.CourtshipEvents.Count} (error: {host.LoadError}).");
                }

                // Check 2: Compatibility reacts to belief alignment and age gap
                float aligned = host.CalculateCompatibility(30, 31, "militarism", "militarism", false);
                float opposing = host.CalculateCompatibility(30, 31, "militarism", "pacifism", false);
                float wideGap = host.CalculateCompatibility(20, 60, "", "", false);
                if (aligned > opposing && wideGap <= 25f)
                {
                    GD.Print($"[PASS] Check 2: Compatibility aligned={aligned:F1} > opposing={opposing:F1}; wide age gap floored at {wideGap:F1}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Compatibility unexpected (aligned={aligned:F1}, opposing={opposing:F1}, gap={wideGap:F1}).");
                }

                // Check 3: Attraction initiation
                var rng = new SeededRng(150);
                bool attracted = host.TryInitiateAttraction("s_aryn", "s_bex", 80f, 30, 31, "militarism", "militarism", false, rng, currentDay: 3, force: true);
                var rel = host.GetRelationship("s_aryn", "s_bex");
                if (attracted && rel != null && rel.Stage == RomanceStage.Attraction)
                {
                    GD.Print($"[PASS] Check 3: Attraction initiated at stage {rel.Stage} with score {rel.RomanceScore}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Attraction initiation failed.");
                }

                // Check 4: A courtship event raises the romance score
                int beforeScore = rel?.RomanceScore ?? 0;
                host.ConductCourtshipEvent("s_aryn", "s_bex", "shared_meal", 80f, rng, currentDay: 4, forceSuccess: true);
                int afterScore = host.GetRelationship("s_aryn", "s_bex")?.RomanceScore ?? 0;
                if (afterScore > beforeScore)
                {
                    GD.Print($"[PASS] Check 4: Courtship event advanced score {beforeScore} -> {afterScore}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Courtship event did not advance score ({beforeScore} -> {afterScore}).");
                }

                // Check 5: Progression to Courtship
                PushTo(host, "s_aryn", "s_bex", 25, rng);
                var stage5 = host.GetRelationship("s_aryn", "s_bex")?.Stage;
                if (stage5 == RomanceStage.Courtship || stage5 > RomanceStage.Courtship)
                {
                    GD.Print($"[PASS] Check 5: Reached {stage5} at score {host.GetRelationship("s_aryn", "s_bex")?.RomanceScore}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Expected Courtship, got {stage5}.");
                }

                // Check 6: Progression to Partnership
                PushTo(host, "s_aryn", "s_bex", 50, rng);
                var stage6 = host.GetRelationship("s_aryn", "s_bex")?.Stage;
                if (stage6 == RomanceStage.Partnership || stage6 > RomanceStage.Partnership)
                {
                    GD.Print($"[PASS] Check 6: Reached {stage6} at score {host.GetRelationship("s_aryn", "s_bex")?.RomanceScore}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Expected Partnership, got {stage6}.");
                }

                // Check 7: Progression to Bonded
                PushTo(host, "s_aryn", "s_bex", 75, rng);
                var stage7 = host.GetRelationship("s_aryn", "s_bex")?.Stage;
                if (stage7 == RomanceStage.Bonded)
                {
                    GD.Print($"[PASS] Check 7: Reached {stage7} at score {host.GetRelationship("s_aryn", "s_bex")?.RomanceScore}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Expected Bonded, got {stage7}.");
                }

                // Check 8: Sustained bond grants soulmate
                for (int day = 5; day < 40; day++) host.AdvanceDay(day);
                var bondedRel = host.GetRelationship("s_aryn", "s_bex");
                if (bondedRel != null && bondedRel.IsSoulmate && bondedRel.BondedDaysCount >= 30)
                {
                    GD.Print($"[PASS] Check 8: Soulmate granted after {bondedRel.BondedDaysCount} bonded days.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Soulmate not granted (bonded days {bondedRel?.BondedDaysCount}).");
                }

                // Check 9: Family unit formation
                var family = host.FormFamilyUnit("s_aryn", "s_bex", "Household Aryn-Bex");
                if (family != null && family.ParentIds.Count == 2 && host.GetFamilyForSurvivor("s_aryn") != null)
                {
                    GD.Print($"[PASS] Check 9: Family unit formed ({family.FamilyId}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Family unit formation failed.");
                }

                // Check 10: Adoption into the family
                bool adopted = host.AddChildToFamily(family?.FamilyId ?? string.Empty, "s_child", isAdopted: true);
                var unit = host.GetFamilyForSurvivor("s_child");
                if (adopted && unit != null && unit.ChildIds.Contains("s_child"))
                {
                    GD.Print("[PASS] Check 10: Adoption recorded and child resolves to the family.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: Adoption failed.");
                }

                // Check 11: Census reporting
                var census = host.Census;
                if (census.TotalRelationships >= 1 && census.BondedCount >= 1 &&
                    census.TotalFamilies >= 1 && census.LoadedCourtshipEvents >= 20)
                {
                    GD.Print($"[PASS] Check 11: Census accurate (rels={census.TotalRelationships}, bonded={census.BondedCount}, families={census.TotalFamilies}, events={census.LoadedCourtshipEvents}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Census mismatch.");
                }

                // Check 12: Save capture / restore round-trip + SaveStore metadata
                var persisted = host.CapturePersistedState();
                var restored = new RomanceFamilyHostSession(dataDir);
                restored.RestoreCoreState(persisted.core_state);
                if (restored.Relationships.Count == host.Relationships.Count &&
                    restored.FamilyUnits.Count == host.FamilyUnits.Count &&
                    string.Equals(RomanceFamilySaveStore.FileName, "romance_family_save.json", StringComparison.Ordinal) &&
                    string.Equals(RomanceFamilySaveStore.SectionName, "romance_family", StringComparison.Ordinal))
                {
                    GD.Print("[PASS] Check 12: Save capture / restore verified with correct SaveStore metadata.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Round-trip mismatch ({restored.Relationships.Count}/{host.Relationships.Count} rels, {restored.FamilyUnits.Count}/{host.FamilyUnits.Count} families).");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FATAL] HostCliRomanceFamily exception: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== [HostCli] RomanceFamily Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static void PushTo(RomanceFamilyHostSession host, string a, string b, int target, ISeededRng rng)
        {
            int guard = 0;
            while (guard++ < 80)
            {
                int score = host.GetRelationship(a, b)?.RomanceScore ?? 0;
                if (score >= target) return;
                host.ConductCourtshipEvent(a, b, "deep_conversation", 90f, rng, currentDay: 4 + guard, forceSuccess: true);
            }
        }
    }
}
