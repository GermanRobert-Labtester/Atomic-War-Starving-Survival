// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 159 (Shelter Governance & Political System).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Governance;

namespace AtomicWar.GodotApp
{
    public static class HostCliShelterGovernance
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Shelter Governance Self-Test (Plan 159) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var loadResult = ShelterGovernanceCatalogLoader.Load(dataDir);
                if (loadResult.Success && loadResult.Blocs.Count >= 4)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({loadResult.Blocs.Count} blocs).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog load failed: {string.Join("; ", loadResult.Errors)}");
                }

                // Check 2: Host session instantiation and bloc presence
                var host = ShelterGovernanceHostSession.Create(dataDir);
                bool hasAllFour = host.Definitions.ContainsKey("bloc_security_first")
                    && host.Definitions.ContainsKey("bloc_egalitarian_commons")
                    && host.Definitions.ContainsKey("bloc_free_pioneers")
                    && host.Definitions.ContainsKey("bloc_heritage_archive");

                if (hasAllFour && host.Blocs.Count >= 4)
                {
                    GD.Print($"[PASS] Check 2: Host session loaded all 4 canonical ideological blocs.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Missing expected ideological blocs in host session.");
                }

                // Check 3: Baseline weights and decay rates
                var secDef = host.Definitions["bloc_security_first"];
                if (secDef.baseline_weight > 0 && secDef.grievance_decay_rate > 0 && secDef.core_ideology == "Authoritarian")
                {
                    GD.Print("[PASS] Check 3: Baseline weights and decay rates match definitions.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Invalid baseline definition properties.");
                }

                // Check 4: Survivor bloc assignment
                bool assigned = host.AssignSurvivorToBloc("survivor_alpha", "bloc_security_first");
                string? blocId = host.GetSurvivorBloc("survivor_alpha");
                string? displayName = host.GetSurvivorBlocDisplayName("survivor_alpha");

                if (assigned && blocId == "bloc_security_first" && displayName == "Security & Order Vanguard")
                {
                    GD.Print("[PASS] Check 4: Survivor assigned to bloc and display name resolved.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Survivor assignment failed: bloc={blocId}, name={displayName}");
                }

                // Check 5: Recalculate weights based on membership
                host.AssignSurvivorToBloc("survivor_beta", "bloc_security_first");
                host.AssignSurvivorToBloc("survivor_gamma", "bloc_egalitarian_commons");
                var secState = host.Blocs["bloc_security_first"];
                var egalState = host.Blocs["bloc_egalitarian_commons"];

                if (secState.influence_weight_bp > egalState.influence_weight_bp)
                {
                    GD.Print($"[PASS] Check 5: Influence weights recalculated by membership ({secState.influence_weight_bp} > {egalState.influence_weight_bp}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Weight recalculation failed: sec={secState.influence_weight_bp}, egal={egalState.influence_weight_bp}");
                }

                // Check 6: Policy consent evaluation
                var consent = host.EvaluatePolicyConsent("curfew", "strict_curfew");
                if (consent != null && consent.supporting_bloc_ids.Contains("bloc_security_first")
                    && consent.opposing_bloc_ids.Contains("bloc_free_pioneers"))
                {
                    GD.Print("[PASS] Check 6: Policy consent correctly resolved supporting and opposing blocs.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 6: Policy consent evaluation failed.");
                }

                // Check 7: Grievance accumulation on opposing policy
                int initialPioneerGrievance = host.Blocs["bloc_free_pioneers"].grievance_bp;
                host.Engine.RecordPolicyEnactmentGrievance("curfew", "strict_curfew");
                int newPioneerGrievance = host.Blocs["bloc_free_pioneers"].grievance_bp;

                if (newPioneerGrievance > initialPioneerGrievance)
                {
                    GD.Print($"[PASS] Check 7: Grievance accumulated on opposed policy ({initialPioneerGrievance} -> {newPioneerGrievance}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Grievance failed to accumulate: {initialPioneerGrievance} -> {newPioneerGrievance}");
                }

                // Check 8: Open civil dispute
                var dispute = host.OpenDispute("survivor_alpha", "survivor_gamma", GovernanceDisputeType.IdeologicalSchism, 5);
                if (dispute != null && !dispute.is_resolved && host.Disputes.Count == 1)
                {
                    GD.Print($"[PASS] Check 8: Opened civil dispute {dispute.case_id} ({dispute.dispute_type}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: Failed to open civil dispute.");
                }

                // Check 9: Resolve civil dispute
                bool resolved = host.ResolveDispute(dispute!.case_id, GovernanceDisputeResolution.Mediation, 6);
                var resolvedCase = host.Disputes.FirstOrDefault(d => d.case_id == dispute.case_id);
                if (resolved && resolvedCase != null && resolvedCase.is_resolved && resolvedCase.applied_resolution == GovernanceDisputeResolution.Mediation)
                {
                    GD.Print("[PASS] Check 9: Civil dispute resolved via Mediation.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Failed to resolve civil dispute.");
                }

                // Check 10: Daily tick & grievance decay
                int grievanceBeforeTick = host.Blocs["bloc_free_pioneers"].grievance_bp;
                host.AdvanceDay(7);
                int grievanceAfterTick = host.Blocs["bloc_free_pioneers"].grievance_bp;

                if (grievanceAfterTick < grievanceBeforeTick)
                {
                    GD.Print($"[PASS] Check 10: Grievance decayed after day tick ({grievanceBeforeTick} -> {grievanceAfterTick}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Grievance did not decay: {grievanceBeforeTick} -> {grievanceAfterTick}");
                }

                // Check 11: Stability rating and census calculation
                var census = host.Census;
                int stability = host.StabilityRating;

                if (census.TotalBlocs >= 4 && census.TotalMembers == 3 && census.ResolvedDisputes == 1 && census.OpenDisputes == 0 && stability > 0)
                {
                    GD.Print($"[PASS] Check 11: Stability rating {stability}% and census verified (Members={census.TotalMembers}, Resolved={census.ResolvedDisputes}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Census or stability mismatch: Members={census.TotalMembers}, Stability={stability}");
                }

                // Check 12: Save/restore roundtrip via ShelterGovernanceSaveStore
                var captured = host.CaptureState();
                bool saveOk = ShelterGovernanceSaveStore.TrySave(captured);
                var loaded = ShelterGovernanceSaveStore.TryLoad();

                var restoredHost = ShelterGovernanceHostSession.Create(dataDir);
                restoredHost.RestoreState(loaded);
                var restoredCensus = restoredHost.Census;

                if (saveOk && loaded != null && restoredCensus.TotalMembers == 3 && restoredCensus.ResolvedDisputes == 1)
                {
                    GD.Print("[PASS] Check 12: ShelterGovernanceSaveStore save/load/restore roundtrip verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Save/restore roundtrip failed.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during shelter governance self-test: {ex}");
            }

            GD.Print($"=== Shelter Governance Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
