// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : InterpersonalConflictSelfTest
// Subsystem          : Plan 202 — Interpersonal Conflict & Grievance
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;


namespace AtomicWar.GodotApp
{
    public static class HostCliInterpersonalConflict
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Interpersonal Conflict & Grievance System Self-Test (Plan 202) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from conflict_templates.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "conflict_templates.json");

                var session = InterpersonalConflictHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.System.GetAllTemplates().Count >= 4)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.System.GetAllTemplates().Count} conflict templates.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Conflict templates catalog failed to load (count={session.System.GetAllTemplates().Count}).");
                }

                // Check 2: Conflict template query verification
                var noiseTmpl = session.System.GetTemplate("argument_noise");
                var rationTmpl = session.System.GetTemplate("grudge_ration_cut");
                if (noiseTmpl != null && rationTmpl != null && noiseTmpl.base_escalation == 20f && rationTmpl.grievance_intensity == 50f)
                {
                    Console.WriteLine("[PASS] Check 2: Conflict templates verified (Noise: 20 base, Ration: 50 grievance).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Conflict template query verification failed.");
                }

                // Check 3: Conflict initiation between survivors
                var conflict = session.InitiateConflict(
                    "surv_dan", "surv_jack", ConflictType.ResourceDispute,
                    "Dispute over battery allocation for radio transceiver",
                    ConflictSeverity.Mild, currentDay: 1);
                if (conflict != null && conflict.InitiatorId == "surv_dan" && conflict.TargetId == "surv_jack")
                {
                    Console.WriteLine($"[PASS] Check 3: Initiated conflict {conflict.ConflictId} between surv_dan and surv_jack.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Conflict initiation failed.");
                }

                // Check 4: Grievance recording and intensity tracking
                var grievance = session.AddGrievance("surv_dan", "surv_jack", "Accused of hoarding flashlight batteries", intensity: 45f, currentDay: 1);
                var survGrievances = session.System.GetGrievancesForSurvivor("surv_dan");
                if (grievance != null && survGrievances.Count >= 1 && grievance.Intensity == 45f)
                {
                    Console.WriteLine($"[PASS] Check 4: Recorded grievance {grievance.GrievanceId} with intensity {grievance.Intensity:F0}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Grievance recording failed.");
                }

                // Check 5: Conflict escalation delta
                float newScore = session.EscalateConflict(conflict!.ConflictId, delta: 35f, currentDay: 2);
                if (newScore > conflict.EscalationScore - 1f)
                {
                    Console.WriteLine($"[PASS] Check 5: Escalated conflict {conflict.ConflictId} to score {newScore:F1}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Conflict escalation failed.");
                }

                // Check 6: Physical fight risk on crisis escalation threshold (80+)
                bool fightRiskFired = false;
                session.System.OnPhysicalFightRisk += _ => fightRiskFired = true;
                session.EscalateConflict(conflict.ConflictId, delta: 50f, currentDay: 2);
                if (conflict.Severity == ConflictSeverity.Crisis && fightRiskFired)
                {
                    Console.WriteLine($"[PASS] Check 6: Crisis severity reached ({conflict.EscalationScore:F1}) and physical fight risk event fired.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Crisis escalation or fight risk event failed.");
                }

                // Check 7: Mediation resolution
                bool mediated = session.MediateConflict(conflict.ConflictId, mediatorId: "surv_leader", currentDay: 3);
                if (mediated && conflict.IsResolved && conflict.ResolutionMethod == ConflictResolutionMethod.Mediation)
                {
                    Console.WriteLine($"[PASS] Check 7: Conflict {conflict.ConflictId} mediated successfully by surv_leader.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Conflict mediation failed.");
                }

                // Check 8: Apology resolution and second conflict handling
                var conflict2 = session.InitiateConflict(
                    "surv_elena", "surv_marcus", ConflictType.PersonalityClash,
                    "Disagreement on watch shifts",
                    ConflictSeverity.Mild, currentDay: 3);
                bool apologized = session.ApologizeAndResolve(conflict2.ConflictId, currentDay: 3);
                if (apologized && conflict2.IsResolved && conflict2.ResolutionMethod == ConflictResolutionMethod.Apology)
                {
                    Console.WriteLine($"[PASS] Check 8: Conflict {conflict2.ConflictId} resolved via apology.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Apology resolution failed.");
                }

                // Check 9: Daily tick natural decay of grievances
                var decayGrievance = session.AddGrievance("surv_clara", "surv_marcus", "Took extra warm clothing", intensity: 30f, currentDay: 3);
                float prevGrievanceIntensity = decayGrievance.Intensity;
                session.TickDay(4);
                if (decayGrievance.Intensity < prevGrievanceIntensity)
                {
                    Console.WriteLine($"[PASS] Check 9: Daily tick reduced grievance intensity ({prevGrievanceIntensity:F0} -> {decayGrievance.Intensity:F0}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Grievance natural decay tick failed.");
                }



                // Check 10: Canonical relations projection (Rule 5 compliance)
                var relationsState = new SurvivorRelationsState();
                relationsState.relationships.Add(new RelationshipEntry
                {
                    dwellerA = "surv_anna",
                    dwellerB = "surv_ben",
                    affinity = -40f,
                    resentment = 60f
                });
                var projected = InterpersonalConflictSystem.ProjectCanonicalGrievances(relationsState, currentDay: 4);
                if (projected.Count == 1 && projected[0].HolderId == "surv_anna" && projected[0].AccusedId == "surv_ben")
                {
                    Console.WriteLine($"[PASS] Check 10: Projected canonical relationship stress into detached grievance (Intensity={projected[0].Intensity:F0}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Canonical relationship stress projection failed.");
                }

                // Check 11: Census reporting
                var census = session.Census;
                if (census.TotalConflicts >= 2 && census.ResolvedConflicts >= 2)
                {
                    Console.WriteLine($"[PASS] Check 11: Census reported {census.TotalConflicts} total conflicts ({census.ResolvedConflicts} resolved, {census.CrisisConflicts} crisis).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Census reporting failed (Total={census.TotalConflicts}, Resolved={census.ResolvedConflicts}).");
                }

                // Check 12: SaveStore persistence round-trip
                var captured = session.CaptureState();
                var restoreSession = InterpersonalConflictHostSession.Create();
                restoreSession.RestoreState(captured);
                var restoredCensus = restoreSession.Census;
                if (restoredCensus.TotalConflicts == census.TotalConflicts &&
                    restoredCensus.ResolvedConflicts == census.ResolvedConflicts)
                {
                    Console.WriteLine("[PASS] Check 12: SaveStore captured and restored interpersonal conflict state cleanly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: SaveStore state restore mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception during InterpersonalConflictSelfTest: {ex.Message}");
            }

            Console.WriteLine($"=== InterpersonalConflictSelfTest: {passed}/{total} checks passed. ===");
            return passed == total ? 0 : 1;
        }
    }
}
