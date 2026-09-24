// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Governance;
using Ashfall.Core.IO;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public sealed class Plan159ShelterGovernanceHostIntegrationTests
    {
        private static string GetDataDir()
        {
            // Walk up to find Assets/StreamingAssets/Data
            string dir = AppContext.BaseDirectory;
            while (!string.IsNullOrEmpty(dir))
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? string.Empty;
            }
            return string.Empty;
        }

        [Fact]
        public void CatalogLoader_LoadsValidBlocs_ReturnsAllDefinitions()
        {
            string dataDir = GetDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "StreamingAssets Data dir not found");

            var result = ShelterGovernanceCatalogLoader.Load(dataDir);
            Assert.True(result.Success, $"Catalog load failed: {string.Join("; ", result.Errors)}");
            Assert.True(result.Blocs.Count >= 4, "Expected at least 4 ideological blocs");

            var sec = result.Blocs.FirstOrDefault(b => b.bloc_id == "bloc_security_first");
            Assert.NotNull(sec);
            Assert.Equal("Security & Order Vanguard", sec.display_name);
            Assert.Equal("Authoritarian", sec.core_ideology);
            Assert.True(sec.baseline_weight > 0);
            Assert.True(sec.grievance_decay_rate > 0);
            Assert.Contains("curfew", sec.supported_policy_scopes);
            Assert.Contains("open_admission", sec.opposed_policy_scopes);

            var egal = result.Blocs.FirstOrDefault(b => b.bloc_id == "bloc_egalitarian_commons");
            Assert.NotNull(egal);
            Assert.Equal("Collectivist", egal.core_ideology);

            var pio = result.Blocs.FirstOrDefault(b => b.bloc_id == "bloc_free_pioneers");
            Assert.NotNull(pio);
            Assert.Equal("Libertarian", pio.core_ideology);

            var arch = result.Blocs.FirstOrDefault(b => b.bloc_id == "bloc_heritage_archive");
            Assert.NotNull(arch);
            Assert.Equal("Traditionalist", arch.core_ideology);
        }

        [Fact]
        public void CatalogLoader_RejectsInvalidJson_ReportsErrors()
        {
            var resEmpty = ShelterGovernanceCatalogLoader.LoadFromJson("");
            Assert.False(resEmpty.Success);

            var resBadVersion = ShelterGovernanceCatalogLoader.LoadFromJson("{\"schema_version\": 0, \"blocs\": []}");
            Assert.False(resBadVersion.Success);

            var resNoBlocs = ShelterGovernanceCatalogLoader.LoadFromJson("{\"schema_version\": 1, \"blocs\": []}");
            Assert.False(resNoBlocs.Success);

            var resDupId = ShelterGovernanceCatalogLoader.LoadFromJson(@"{
                ""schema_version"": 1,
                ""blocs"": [
                    { ""bloc_id"": ""b1"", ""display_name"": ""D1"", ""core_ideology"": ""I1"", ""baseline_weight"": 20, ""grievance_decay_rate"": 5 },
                    { ""bloc_id"": ""b1"", ""display_name"": ""D2"", ""core_ideology"": ""I2"", ""baseline_weight"": 20, ""grievance_decay_rate"": 5 }
                ]
            }");
            Assert.False(resDupId.Success);
            Assert.Contains(resDupId.Errors, e => e.Contains("Duplicate bloc_id"));
        }

        [Fact]
        public void Engine_Initialization_LoadsDefaultsOrCatalog()
        {
            string dataDir = GetDataDir();
            var engine = new ShelterGovernanceEngine();

            if (!string.IsNullOrEmpty(dataDir))
            {
                var loadResult = ShelterGovernanceCatalogLoader.Load(dataDir);
                if (loadResult.Success)
                {
                    engine.BindValidatedBlocs(loadResult.Blocs);
                }
            }

            Assert.True(engine.Definitions.Count >= 4);
            Assert.True(engine.Blocs.Count >= 4);
            Assert.True(engine.CalculateStabilityRating() > 0);

            var census = engine.GetCensus();
            Assert.True(census.TotalBlocs >= 4);
            Assert.Equal(0, census.TotalMembers);
            Assert.Equal(0, census.OpenDisputes);
            Assert.Equal(0, census.ResolvedDisputes);
        }

        [Fact]
        public void Engine_SurvivorAssignment_UpdatesBlocMembershipAndRecalculatesWeights()
        {
            string dataDir = GetDataDir();
            var engine = new ShelterGovernanceEngine();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var loadResult = ShelterGovernanceCatalogLoader.Load(dataDir);
                if (loadResult.Success) engine.BindValidatedBlocs(loadResult.Blocs);
            }

            bool ok = engine.AssignSurvivorToBloc("survivor_01", "bloc_security_first");
            Assert.True(ok);
            Assert.Equal("bloc_security_first", engine.GetSurvivorBloc("survivor_01"));
            Assert.Equal("Security & Order Vanguard", engine.GetSurvivorBlocDisplayName("survivor_01"));

            // Assigning to a second bloc moves the survivor
            engine.AssignSurvivorToBloc("survivor_01", "bloc_egalitarian_commons");
            Assert.Equal("bloc_egalitarian_commons", engine.GetSurvivorBloc("survivor_01"));
            Assert.DoesNotContain("survivor_01", engine.Blocs["bloc_security_first"].member_survivor_ids);
            Assert.Contains("survivor_01", engine.Blocs["bloc_egalitarian_commons"].member_survivor_ids);

            // Adding multiple survivors shifts influence weight
            engine.AssignSurvivorToBloc("survivor_02", "bloc_egalitarian_commons");
            engine.AssignSurvivorToBloc("survivor_03", "bloc_egalitarian_commons");
            engine.AssignSurvivorToBloc("survivor_04", "bloc_free_pioneers");

            var egal = engine.Blocs["bloc_egalitarian_commons"];
            var pio = engine.Blocs["bloc_free_pioneers"];
            Assert.True(egal.influence_weight_bp > pio.influence_weight_bp);
        }

        [Fact]
        public void Engine_EvaluatePolicyConsent_ComputesNetConsentAndProjectedGrievance()
        {
            string dataDir = GetDataDir();
            var engine = new ShelterGovernanceEngine();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var loadResult = ShelterGovernanceCatalogLoader.Load(dataDir);
                if (loadResult.Success) engine.BindValidatedBlocs(loadResult.Blocs);
            }

            engine.AssignSurvivorToBloc("s1", "bloc_security_first");
            engine.AssignSurvivorToBloc("s2", "bloc_security_first");

            // Curfew is supported by security_first, opposed by free_pioneers
            var consent = engine.EvaluatePolicyConsent("curfew", "strict_curfew");
            Assert.NotNull(consent);
            Assert.Contains("bloc_security_first", consent.supporting_bloc_ids);
            Assert.Contains("bloc_free_pioneers", consent.opposing_bloc_ids);
        }

        [Fact]
        public void Engine_CivilDisputes_OpenAndResolve_UpdatesCensus()
        {
            var engine = new ShelterGovernanceEngine();

            var dispute = engine.OpenDispute("survivor_1", "survivor_2", GovernanceDisputeType.WorkRefusal, 10);
            Assert.NotNull(dispute);
            Assert.False(dispute.is_resolved);
            Assert.Equal(GovernanceDisputeType.WorkRefusal, dispute.dispute_type);
            Assert.Equal(10, dispute.day_initiated);

            var censusBefore = engine.GetCensus();
            Assert.Equal(1, censusBefore.OpenDisputes);
            Assert.Equal(0, censusBefore.ResolvedDisputes);

            bool resolved = engine.ResolveDispute(dispute.case_id, GovernanceDisputeResolution.OfficialReprimand, 12);
            Assert.True(resolved);

            var resolvedCase = engine.Disputes.First(d => d.case_id == dispute.case_id);
            Assert.True(resolvedCase.is_resolved);
            Assert.Equal(GovernanceDisputeResolution.OfficialReprimand, resolvedCase.applied_resolution);
            Assert.Equal(12, resolvedCase.resolution_day);

            var censusAfter = engine.GetCensus();
            Assert.Equal(0, censusAfter.OpenDisputes);
            Assert.Equal(1, censusAfter.ResolvedDisputes);
        }

        [Fact]
        public void Engine_DailyTick_DecaysGrievanceAndAdvancesSimulation()
        {
            var engine = new ShelterGovernanceEngine();

            // Record policy grievance on free_pioneers
            engine.RecordPolicyEnactmentGrievance("curfew", "night_lock");
            int initialGrievance = engine.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.True(initialGrievance > 0);

            // Advance day (ticks 24 hours)
            engine.Tick(24f);
            int postTickGrievance = engine.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.True(postTickGrievance < initialGrievance, "Grievance should decay after daily tick");
        }

        [Fact]
        public void Engine_StabilityRating_ReflectsDisputesGrievancesAndLeadership()
        {
            var engine = new ShelterGovernanceEngine();

            int baselineStability = engine.CalculateStabilityRating();
            Assert.True(baselineStability >= 80);

            // Open multiple disputes
            engine.OpenDispute("s1", "s2", GovernanceDisputeType.ResourceTheft, 1);
            engine.OpenDispute("s2", "s3", GovernanceDisputeType.IdeologicalSchism, 1);
            engine.OpenDispute("s3", "s4", GovernanceDisputeType.CurfewViolation, 1);

            int penalizedStability = engine.CalculateStabilityRating();
            Assert.True(penalizedStability < baselineStability, "Open disputes should penalize stability");
        }

        [Fact]
        public void Engine_SaveAndRestore_PreservesAllBlocsAndDisputes()
        {
            var engine = new ShelterGovernanceEngine();

            engine.AssignSurvivorToBloc("surv_alpha", "bloc_security_first");
            engine.AssignSurvivorToBloc("surv_beta", "bloc_egalitarian_commons");
            var dispute = engine.OpenDispute("surv_alpha", "surv_beta", GovernanceDisputeType.Insult, 3);
            engine.ResolveDispute(dispute.case_id, GovernanceDisputeResolution.Restitution, 4);

            var captured = engine.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(1, captured.schema_version);
            Assert.Single(captured.disputes);
            Assert.Contains("surv_alpha", captured.blocs["bloc_security_first"].member_survivor_ids);

            var freshEngine = new ShelterGovernanceEngine();
            freshEngine.RestoreState(captured);

            Assert.Equal("bloc_security_first", freshEngine.GetSurvivorBloc("surv_alpha"));
            Assert.Equal("bloc_egalitarian_commons", freshEngine.GetSurvivorBloc("surv_beta"));
            Assert.Single(freshEngine.Disputes);
            Assert.True(freshEngine.Disputes[0].is_resolved);
            Assert.Equal(GovernanceDisputeResolution.Restitution, freshEngine.Disputes[0].applied_resolution);
        }

        [Fact]
        public void SaveSectionRegistry_ContainsShelterGovernance()
        {
            Assert.True(SaveSectionRegistry.TryGetSection("shelter_governance", out var metadata));
            Assert.NotNull(metadata);
            Assert.Equal("shelter_governance", metadata.SectionKey);
            Assert.Equal("SaveShelterGovernance", metadata.SaveMethod);
            Assert.Equal("SetupShelterGovernance", metadata.SetupMethod);
            Assert.Equal("governance", metadata.Owner);
            Assert.Equal(SaveSectionRegistry.ExpandedShelterLifecycleGroup, metadata.LifecycleGroup);

            Assert.True(SaveSectionRegistry.SectionFileNames.TryGetValue("shelter_governance", out string? fileName));
            Assert.Equal("shelter_governance_save.json", fileName);
        }
    }
}
