// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 148 (Ideological Friction → Events & Quests).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliIdeologicalFriction
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Ideological Friction Self-Test (Plan 148) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var host = IdeologicalFrictionHostSession.Create(dataDir);
                var templates = host.Templates;
                if (templates.Count >= 8)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({templates.Count} templates).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 8 templates, got {templates.Count}.");
                }

                // Check 2: Event type diversity
                var types = templates.Select(t => t.event_type.ToLowerInvariant()).Distinct().ToList();
                if (types.Contains("confrontation") && types.Contains("conversion") &&
                    types.Contains("split") && types.Contains("quest"))
                {
                    GD.Print("[PASS] Check 2: Catalog contains all 4 event types (confrontation, conversion, split, quest).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Missing event types. Found: {string.Join(", ", types)}");
                }

                // Check 3: Belief registration
                host.RegisterBelief("survivor_sentry", "military_discipline");
                host.RegisterBelief("survivor_medic", "pacifist");
                host.RegisterBelief("survivor_pastor", "religious_faith");
                host.RegisterBelief("survivor_scientist", "atheist_rationalist");

                if (host.GetBelief("survivor_sentry") == "military_discipline" &&
                    host.GetBelief("survivor_medic") == "pacifist")
                {
                    GD.Print("[PASS] Check 3: Survivor worldview beliefs registered successfully.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Belief registration mismatch.");
                }

                // Check 4: Roommate conflict sleep penalty
                float sentryMedicCompat = host.GetRoommateCompatibilityMultiplier("survivor_sentry", "survivor_medic");
                if (Math.Abs(sentryMedicCompat - 0.80f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 4: Opposing bunkmates trigger 20% sleep penalty (mult = {sentryMedicCompat:F2}x).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Expected 0.80x compatibility, got {sentryMedicCompat:F2}x.");
                }

                // Check 5: Roommate synergy sleep bonus
                host.RegisterBelief("survivor_scout", "military_discipline");
                float sentryScoutCompat = host.GetRoommateCompatibilityMultiplier("survivor_sentry", "survivor_scout");
                if (Math.Abs(sentryScoutCompat - 1.10f) < 0.01f)
                {
                    GD.Print($"[PASS] Check 5: Aligned bunkmates trigger 10% sleep synergy bonus (mult = {sentryScoutCompat:F2}x).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Expected 1.10x compatibility, got {sentryScoutCompat:F2}x.");
                }

                // Check 6: Affinity drain between conflicting roommates
                float initialAffinity = host.GetAffinity("survivor_sentry", "survivor_medic");
                host.TickRoommates("survivor_sentry", "survivor_medic", gameHours: 24f);
                float postTickAffinity = host.GetAffinity("survivor_sentry", "survivor_medic");
                if (postTickAffinity < initialAffinity)
                {
                    GD.Print($"[PASS] Check 6: Roommate friction drained affinity from {initialAffinity:F1} to {postTickAffinity:F1}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Roommate friction did not drain affinity.");
                }

                // Check 7: Emergent confrontation triggering at negative threshold
                var instance = host.CheckDailyFriction(
                    survivorA: "survivor_pastor",
                    beliefA: "religious_faith",
                    survivorB: "survivor_scientist",
                    beliefB: "atheist_rationalist",
                    pairAffinity: -60f,
                    currentDay: 5,
                    forceTrigger: true);

                if (instance != null && instance.eventType == IdeologicalEventType.Confrontation)
                {
                    GD.Print($"[PASS] Check 7: Emergent confrontation triggered: '{instance.title}'.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Confrontation event failed to trigger at -60 affinity.");
                }

                // Check 8: Mediation choice resolution
                if (instance != null)
                {
                    bool resolved = host.ResolveConfrontation(
                        instance.instanceId,
                        IdeologicalMediationChoice.BrokerCompromise,
                        out float affA,
                        out float affB,
                        out float morale);

                    if (resolved && affA > 0f && affB > 0f && morale > 0f && instance.isResolved)
                    {
                        GD.Print("[PASS] Check 8: BrokerCompromise mediation restored mutual affinity and improved morale.");
                        passed++;
                    }
                    else
                    {
                        GD.PrintErr("[FAIL] Check 8: Mediation resolution failed or returned unexpected deltas.");
                    }
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Skipped due to Check 7 null instance.");
                }

                // Check 9: Belief conversion attempt
                var rng = new SeededRng(148);
                bool conversionResult = host.AttemptConversion(
                    actorId: "survivor_pastor",
                    targetId: "survivor_convert",
                    actorBelief: "religious_faith",
                    successProbability: 1.0f,
                    rng: rng,
                    out string msg);

                if (conversionResult && host.GetBelief("survivor_convert") == "religious_faith")
                {
                    GD.Print($"[PASS] Check 9: Conversion attempt successful ({msg}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Conversion failed: {msg}");
                }

                // Check 10: Bunker factions formation
                var beliefs = new Dictionary<string, string>
                {
                    { "s1", "collectivist_solidarity" },
                    { "s2", "collectivist_solidarity" },
                    { "s3", "collectivist_solidarity" },
                    { "s4", "pragmatic_individualism" },
                    { "s5", "pragmatic_individualism" },
                    { "s6", "pragmatic_individualism" }
                };
                var factions = host.UpdateBunkerFactions(beliefs);
                if (factions.Count == 2)
                {
                    GD.Print($"[PASS] Check 10: Bunker factions formed ({factions.Count} active factions of >= 3 members).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Expected 2 factions, got {factions.Count}.");
                }

                // Check 11: Census reporting
                var census = host.Census;
                if (census.LoadedTemplatesCount >= 8 && census.TotalEventsFired >= 1 && census.ActiveFactionsCount >= 2)
                {
                    GD.Print($"[PASS] Check 11: Census reporting accurate (templates={census.LoadedTemplatesCount}, fired={census.TotalEventsFired}, factions={census.ActiveFactionsCount}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Census mismatch.");
                }

                // Check 12: Save capture / restore round-trip & SaveStore metadata
                var captured = host.CaptureState();
                var host2 = IdeologicalFrictionHostSession.Create(dataDir);
                host2.RestoreState(captured);
                if (host2.RecentEvents.Count == host.RecentEvents.Count &&
                    host2.ActiveFactions.Count == host.ActiveFactions.Count &&
                    string.Equals(IdeologicalFrictionSaveStore.FileName, "ideological_friction_save.json", StringComparison.Ordinal) &&
                    string.Equals(IdeologicalFrictionSaveStore.SectionName, "ideological_friction", StringComparison.Ordinal))
                {
                    GD.Print("[PASS] Check 12: Save capture / restore verified with correct SaveStore metadata.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: Save state round-trip or metadata mismatch.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FATAL] HostCliIdeologicalFriction exception: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== [HostCli] IdeologicalFriction Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
