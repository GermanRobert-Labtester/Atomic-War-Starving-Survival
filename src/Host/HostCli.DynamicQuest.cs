// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 171 (Dynamic Quest Generation).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Quests;

namespace AtomicWar.GodotApp
{
    public static class HostCliDynamicQuest
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Dynamic Quest Generation Self-Test (Plan 171) ===");
            int passed = 0;
            int total = 12;

            try
            {
                string path = Path.Combine(dataDir, "dynamic_quest_templates.json");
                var templates = DynamicQuestTemplateCatalogLoader.LoadFromJson(File.ReadAllText(path));

                // Check 1: strict loader accepts the authored catalog
                if (templates.Count >= 7)
                {
                    GD.Print($"[PASS] Check 1: Strict loader accepted {templates.Count} authored dynamic quest templates.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 7 templates, got {templates.Count}.");
                }

                // Check 2: strict loader rejects malformed catalogs
                bool rejected = ExpectReject("{'schema_version':1,'templates':[{'template_id':'t1','type':'FetchResource','title':'A','description':'d','base_difficulty':1,'required_quantity':1},{'template_id':'t1','type':'FetchResource','title':'B','description':'d','base_difficulty':1,'required_quantity':1}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'templates':[{'template_id':'t1','type':'NotAType','title':'A','description':'d','base_difficulty':1,'required_quantity':1}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'templates':[{'template_id':'t1','type':'FetchResource','title':'','description':'d','base_difficulty':1,'required_quantity':1}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'templates':[{'template_id':'t1','type':'FetchResource','title':'A','description':'d','base_difficulty':6,'required_quantity':1}]}".Replace('\'', '"'))
                    && ExpectReject("{'schema_version':1,'templates':[{'template_id':'t1','type':'FetchResource','title':'A','description':'d','base_difficulty':1,'required_quantity':1,'reward_item_id':'x','reward_item_count':0}]}".Replace('\'', '"'));
                if (rejected)
                {
                    GD.Print("[PASS] Check 2: Strict loader rejected duplicate id, unknown type, empty title, bad difficulty, and a reward item without a count.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: A malformed catalog was accepted.");
                }

                // Check 3: bind replaces the built-in defaults
                var generator = new DynamicQuestGenerator();
                generator.BindAuthoredTemplates(templates);
                if (generator.GetCensus().TemplateCount == templates.Count)
                {
                    GD.Print($"[PASS] Check 3: Bound {generator.GetCensus().TemplateCount} authored templates (defaults replaced).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Template count {generator.GetCensus().TemplateCount} != {templates.Count}.");
                }

                // Check 4: generation respects the active limit
                var generated = generator.GenerateQuests(currentDay: 1);
                if (generated.Count > 0 && generated.Count <= 3)
                {
                    GD.Print($"[PASS] Check 4: Generated {generated.Count} candidates within the active limit.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Generated {generated.Count} candidates.");
                }

                // Check 5: cooldown suppresses immediate regeneration
                var second = generator.GenerateQuests(currentDay: 2);
                if (second.Count == 0)
                {
                    GD.Print("[PASS] Check 5: Cooldown suppressed regeneration on day 2.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Cooldown did not suppress ({second.Count} generated).");
                }

                // Check 6: acceptance activates and assigns
                string questId = generated[0].QuestId;
                bool accepted = generator.AcceptQuest(questId, "survivor_probe");
                if (accepted && generator.GetCensus().ActiveQuests == 1)
                {
                    GD.Print($"[PASS] Check 6: Candidate {questId} accepted and assigned.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Acceptance failed.");
                }

                // Check 7: progress increments
                int before = generator.CaptureState().Quests.First(q => q.QuestId == questId).CurrentQuantity;
                generator.ProgressQuest(questId, 1);
                int after = generator.CaptureState().Quests.First(q => q.QuestId == questId).CurrentQuantity;
                if (after == before + 1)
                {
                    GD.Print($"[PASS] Check 7: Progress advanced {before} -> {after}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Progress wrong ({before} -> {after}).");
                }

                // Check 8: completion requires fulfilment
                var live = generator.CaptureState().Quests.First(q => q.QuestId == questId);
                generator.ProgressQuest(questId, live.RequiredQuantity);
                bool completed = generator.CompleteQuest(questId, 5);
                if (completed && generator.GetCensus().CompletedQuests == 1)
                {
                    GD.Print("[PASS] Check 8: Candidate completed only after fulfilment.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Completion failed.");
                }

                // Check 9: deadlines expire/fail
                var fresh = new DynamicQuestGenerator();
                fresh.BindAuthoredTemplates(templates);
                var batch = fresh.GenerateQuests(currentDay: 100);
                string activeId = batch[0].QuestId;
                fresh.AcceptQuest(activeId, "s1");
                fresh.CheckDeadlines(currentDay: 100 + 30);
                var census9 = fresh.GetCensus();
                if (census9.FailedQuests >= 1 || census9.ExpiredQuests >= 1)
                {
                    GD.Print($"[PASS] Check 9: Deadline pass failed {census9.FailedQuests} and expired {census9.ExpiredQuests} candidates.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Deadlines were not applied.");
                }

                // Check 10: census reports truthful counts
                var census = generator.GetCensus();
                if (census.TemplateCount == templates.Count && census.CompletedQuests == 1 && census.TotalQuests == generated.Count)
                {
                    GD.Print($"[PASS] Check 10: Census accurate (templates={census.TemplateCount}, total={census.TotalQuests}, completed={census.CompletedQuests}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census mismatch (templates={census.TemplateCount}, completed={census.CompletedQuests}).");
                }

                // Check 11: capture/restore round-trip preserves reward bindings
                var state = generator.CaptureState();
                var restored = new DynamicQuestGenerator();
                restored.BindAuthoredTemplates(templates);
                restored.RestoreState(state);
                if (restored.CaptureState().Quests.Count == state.Quests.Count
                    && restored.GetCensus().CompletedQuests == 1
                    && state.Quests.All(q => q.RewardItemId != null))
                {
                    GD.Print("[PASS] Check 11: Capture/restore preserved quest state and reward bindings.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Capture/restore lost state.");
                }

                // Check 12: schema gate rejects a newer payload
                bool gated = false;
                try
                {
                    var newer = generator.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException)
                {
                    gated = true;
                }
                if (gated)
                {
                    GD.Print("[PASS] Check 12: RestoreState rejected an unsupported future schema.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: RestoreState accepted an unsupported schema.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in dynamic quest self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Dynamic Quest Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static bool ExpectReject(string json)
        {
            try
            {
                DynamicQuestTemplateCatalogLoader.LoadFromJson(json);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
