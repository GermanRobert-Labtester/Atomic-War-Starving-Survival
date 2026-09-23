// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 147 (Per-NPC Memory & Relationship Depth).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Narrative;

namespace AtomicWar.GodotApp
{
    public static class HostCliNpcMemory
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Per-NPC Memory & Relationship Depth Self-Test (Plan 147) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = NpcMemoryHostSession.Create(dataDir);
                var templates = host.System.GetAllDialogueTemplates();
                if (templates.Count >= 10)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({templates.Count} templates).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 10 dialogue templates, got {templates.Count}.");
                }

                // Check 2: Dialogue tone variety
                var tones = templates.Select(t => t.tone.ToLowerInvariant()).Distinct().ToList();
                if (tones.Contains("high_trust") && tones.Contains("high_grudge") && tones.Contains("favor_owed") &&
                    tones.Contains("betrayed") && tones.Contains("reconciled"))
                {
                    GD.Print("[PASS] Check 2: Catalog contains all 5 required dialogue emotional tones.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Missing dialogue tones. Found: {string.Join(", ", tones)}");
                }

                // Check 3: Helping and saving life boosts trust and favors
                host.RecordAction("npc_marta", NpcMemoryActionType.Helped, day: 1);
                host.RecordAction("npc_marta", NpcMemoryActionType.SavedLife, day: 3);
                var relMarta = host.Get("npc_marta");
                if (relMarta != null && relMarta.PersonalTrust == 50f && relMarta.FavorOwed == 25f && relMarta.GrudgeLevel == 0f)
                {
                    GD.Print($"[PASS] Check 3: Positive actions properly computed trust={relMarta.PersonalTrust} and favor={relMarta.FavorOwed}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Positive actions mismatch. Trust={relMarta?.PersonalTrust}, Favor={relMarta?.FavorOwed}.");
                }

                // Check 4: Refusal and betrayal escalate grudge and wipe favors
                host.RecordAction("npc_brant", NpcMemoryActionType.Refused, day: 2);
                host.RecordAction("npc_brant", NpcMemoryActionType.Betrayed, day: 4);
                host.RecordAction("npc_brant", NpcMemoryActionType.Betrayed, day: 5);
                var relBrant = host.Get("npc_brant");
                if (relBrant != null && relBrant.GrudgeLevel > 75f && relBrant.FavorOwed == 0f)
                {
                    GD.Print($"[PASS] Check 4: Betrayal escalated grudge to {relBrant.GrudgeLevel:F0} and wiped favors.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Betrayal mismatch. Grudge={relBrant?.GrudgeLevel}, Favor={relBrant?.FavorOwed}.");
                }

                // Check 5: Severe grudge triggers trade embargo
                if (host.IsTradeRefused("npc_brant") && float.IsPositiveInfinity(host.GetTradePriceMultiplier("npc_brant")))
                {
                    GD.Print("[PASS] Check 5: Severe grudge correctly triggered trade embargo / refusal.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Trade embargo not active for severe grudge. Mult={host.GetTradePriceMultiplier("npc_brant")}.");
                }

                // Check 6: Trusted ally receives price discount
                float martaMult = host.GetTradePriceMultiplier("npc_marta");
                if (Math.Abs(martaMult - 0.85f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 6: Trusted ally receives expected discount (multiplier = {martaMult:F2}x).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Discount mismatch. Expected 0.85x, got {martaMult:F2}x.");
                }

                // Check 7: Forgiveness mechanic and restitution reduces grudge
                host.RecordAction("npc_kane", NpcMemoryActionType.Refused, day: 1);
                host.RecordAction("npc_kane", NpcMemoryActionType.Refused, day: 2);
                var relKane = host.Get("npc_kane");
                float priorGrudge = relKane?.GrudgeLevel ?? 0f;
                bool forgiven = host.Forgive("npc_kane", "apology_accepted", restitutionAmount: 20f);
                if (forgiven && relKane != null && relKane.GrudgeLevel < priorGrudge)
                {
                    GD.Print($"[PASS] Check 7: Forgiveness successful. Grudge reduced from {priorGrudge:F0} to {relKane.GrudgeLevel:F0}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Forgiveness failed or grudge not reduced.");
                }

                // Check 8: Dialogue tone derivation matches relationship status
                NpcDialogueTone martaTone = host.GetDialogueTone("npc_marta");
                NpcDialogueTone brantTone = host.GetDialogueTone("npc_brant");
                if (martaTone == NpcDialogueTone.FavorOwed && brantTone == NpcDialogueTone.Betrayed)
                {
                    GD.Print($"[PASS] Check 8: Dialogue tones correctly resolved (Marta={martaTone}, Brant={brantTone}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Tone mismatch. Marta={martaTone}, Brant={brantTone}.");
                }

                // Check 9: Daily decay softens grudge levels over time
                float preDecayGrudge = relBrant?.GrudgeLevel ?? 0f;
                host.TickDailyDecay(currentDay: 10, decayRatePerDay: 1.0f);
                if (relBrant != null && relBrant.GrudgeLevel < preDecayGrudge)
                {
                    GD.Print($"[PASS] Check 9: Daily decay reduced grudge from {preDecayGrudge:F1} to {relBrant.GrudgeLevel:F1}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Daily decay did not reduce grudge.");
                }

                // Check 10: Census reporting
                var census = host.Census;
                if (census.TotalTrackedNpcs >= 3 && census.TotalMemoriesRecorded >= 6)
                {
                    GD.Print($"[PASS] Check 10: Census reporting accurate ({census.TotalTrackedNpcs} NPCs, {census.TotalMemoriesRecorded} memories).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census inaccurate. NPCs={census.TotalTrackedNpcs}, Memories={census.TotalMemoriesRecorded}.");
                }

                // Check 11: Save capture and restore round-trip
                var capturedState = host.CaptureState();
                var host2 = NpcMemoryHostSession.Create(dataDir);
                host2.RestoreState(capturedState);
                var relMarta2 = host2.Get("npc_marta");
                if (relMarta2 != null && relMarta2.PersonalTrust == relMarta?.PersonalTrust &&
                    relMarta2.Memories.Count == relMarta?.Memories.Count)
                {
                    GD.Print("[PASS] Check 11: Save capture and restore round-trip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Save restore corrupted or dropped relationships.");
                }

                // Check 12: SaveStore metadata
                if (string.Equals(NpcMemorySaveStore.FileName, "npc_memory_save.json", StringComparison.Ordinal) &&
                    string.Equals(NpcMemorySaveStore.SectionName, "npc_memory", StringComparison.Ordinal))
                {
                    GD.Print("[PASS] Check 12: NpcMemorySaveStore constants correctly registered.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: Save store constants mismatch.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FATAL] HostCliNpcMemory exception: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== [HostCli] NpcMemory Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
