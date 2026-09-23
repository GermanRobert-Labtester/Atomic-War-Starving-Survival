// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 202: Survivor Interpersonal Conflict & Grievances — Integration Tests
// Verifies conflict templates catalog loading, template-driven conflict initiation,
// grievance tracking, escalation ladder & fight risk alerts, mediation & apology,
// canonical relations projection, and save/restore persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan202InterpersonalConflictIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAllTwentyConflictTemplates()
        {
            var system = new InterpersonalConflictSystem();
            string path = Path.Combine(DataDirectory, "conflict_templates.json");
            Assert.True(File.Exists(path), $"conflict_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var templates = system.GetAllTemplates();
            Assert.Equal(20, templates.Count);

            var noise = system.GetTemplate("argument_noise");
            Assert.NotNull(noise);
            Assert.Equal("argument", noise.conflict_type);
            Assert.Equal("mild", noise.default_severity);

            var betrayal = system.GetTemplate("betrayal_stolen_heirloom");
            Assert.NotNull(betrayal);
            Assert.Equal("betrayal", betrayal.conflict_type);
            Assert.Equal("crisis", betrayal.default_severity);
            Assert.True(betrayal.base_escalation >= 80f);
        }

        [Fact]
        public void InitiateConflictFromTemplate_CreatesActiveConflictAndGrievance()
        {
            var system = new InterpersonalConflictSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "conflict_templates.json")));

            InterpersonalConflict? initiated = null;
            system.OnConflictInitiated += c => initiated = c;

            var conflict = system.InitiateConflictFromTemplate(
                templateId: "resource_water_allocation",
                initiatorId: "surv_alpha",
                targetId: "surv_beta",
                currentDay: 2);

            Assert.NotNull(conflict);
            Assert.NotNull(initiated);
            Assert.Equal(1, system.ActiveConflictCount);
            Assert.Equal(ConflictType.ResourceDispute, conflict.Type);
            Assert.Equal(ConflictSeverity.Severe, conflict.Severity);
            Assert.Equal(55.0f, conflict.EscalationScore);

            // Grievance was registered
            var grievances = system.GetGrievancesForSurvivor("surv_alpha");
            Assert.Single(grievances);
            Assert.Equal("surv_beta", grievances[0].AccusedId);
            Assert.Equal(65.0f, grievances[0].Intensity);
        }

        [Fact]
        public void EscalateConflict_IncreasesScoreAndTriggersFightRisk()
        {
            var system = new InterpersonalConflictSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "conflict_templates.json")));

            var conflict = system.InitiateConflictFromTemplate(
                "argument_chores", "surv_c", "surv_d", currentDay: 1)!;

            Assert.NotNull(conflict);
            Assert.Equal(22.0f, conflict.EscalationScore);

            bool fightRiskFired = false;
            system.OnPhysicalFightRisk += c => fightRiskFired = true;

            // Escalate by 60 -> score 82 (>= 80 crisis threshold)
            float score = system.EscalateConflict(conflict.ConflictId, 60.0f, currentDay: 2);

            Assert.Equal(82.0f, score);
            Assert.Equal(ConflictSeverity.Crisis, conflict.Severity);
            Assert.True(fightRiskFired);
        }

        [Fact]
        public void MediateAndApologize_ResolvesConflictAndClearsGrievances()
        {
            var system = new InterpersonalConflictSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "conflict_templates.json")));

            var conflict = system.InitiateConflictFromTemplate(
                "grudge_ration_cut", "surv_cook", "surv_scout", currentDay: 3)!;
            Assert.NotNull(conflict);

            // Mediate conflict
            bool mediated = system.MediateConflict(conflict.ConflictId, mediatorId: "surv_leader", currentDay: 4);
            Assert.True(mediated);
            Assert.True(conflict.IsResolved);
            Assert.Equal(0, system.ActiveConflictCount);
            Assert.Empty(system.GetGrievancesForSurvivor("surv_cook"));

            // Second conflict: apologize and resolve
            var conflict2 = system.InitiateConflictFromTemplate(
                "argument_noise", "surv_e", "surv_f", currentDay: 4)!;
            bool apologized = system.ApologizeAndResolve(conflict2.ConflictId, currentDay: 5);
            Assert.True(apologized);
            Assert.True(conflict2.IsResolved);
            Assert.Equal(ConflictResolutionMethod.Apology, conflict2.ResolutionMethod);
        }

        [Fact]
        public void ProjectCanonicalRelations_GeneratesTypedConflictsAndGrievances()
        {
            var relations = new SurvivorRelationsState();
            relations.activeConflicts.Add(new ConflictEntry
            {
                conflictId = "rel_cnf_01",
                dwellerA = "dweller_1",
                dwellerB = "dweller_2",
                cause = "Dispute over guard rotation",
                isResolved = false,
                dayStarted = 5
            });

            relations.relationships.Add(new RelationshipEntry
            {
                dwellerA = "dweller_1",
                dwellerB = "dweller_2",
                affinity = -40f,
                resentment = 60f
            });

            var projectedConflicts = InterpersonalConflictSystem.ProjectCanonicalRelations(relations, currentDay: 6);
            Assert.Single(projectedConflicts);
            Assert.Equal("relations:rel_cnf_01", projectedConflicts[0].ConflictId);
            Assert.Equal(ConflictSeverity.Severe, projectedConflicts[0].Severity);

            var projectedGrievances = InterpersonalConflictSystem.ProjectCanonicalGrievances(relations, currentDay: 6);
            Assert.Single(projectedGrievances);
            Assert.Equal(60f, projectedGrievances[0].Intensity);
        }

        [Fact]
        public void SaveRestoreState_PreservesConflictsGrievancesAndResolutions()
        {
            var system = new InterpersonalConflictSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "conflict_templates.json")));

            var c1 = system.InitiateConflictFromTemplate("clash_work_pace", "surv_mason", "surv_miner", currentDay: 1)!;
            system.MediateConflict(c1.ConflictId, "surv_chief", currentDay: 2);
            system.InitiateConflictFromTemplate("fairness_bed_assignment", "surv_g", "surv_h", currentDay: 2);

            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(2, captured.Conflicts.Count);
            Assert.Single(captured.Resolutions);

            var restored = new InterpersonalConflictSystem();
            restored.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "conflict_templates.json")));
            restored.RestoreState(captured);

            Assert.Equal(1, restored.ActiveConflictCount);
            Assert.Equal(2, restored.CaptureState().Conflicts.Count);
            Assert.Single(restored.CaptureState().Resolutions);
        }
    }
}
