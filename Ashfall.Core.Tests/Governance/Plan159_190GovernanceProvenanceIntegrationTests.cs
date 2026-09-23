// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 159 & Plan 190 Integration Tests:
// - Plan 159: Shelter Governance & Political System (Ideological Blocs, Consent, Grievances, Disputes)
// - Plan 190: Item Lore, Relic History & Provenance Tracking (Dossiers, Ownership, Significance)
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Governance;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Governance
{
    public sealed class Plan159_190GovernanceProvenanceIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void Plan159_ShelterGovernanceEngine_LoadsAuthoredBlocs_AndValidatesIdeologies()
        {
            string blocsPath = Path.Combine(DataDirectory, "shelter_governance_blocs.json");
            Assert.True(File.Exists(blocsPath), $"shelter_governance_blocs.json must exist at {blocsPath}");

            string json = File.ReadAllText(blocsPath);
            var serializer = new SystemTextJsonSerializer();

            var policyCatalog = new PolicyCatalog();
            var policySys = new PolicySystem(policyCatalog);
            var leaderSys = new LeadershipSystem();
            leaderSys.GetAliveSurvivorIds = () => new List<string> { "surv_guard", "surv_scav", "surv_medic", "surv_worker" };

            var engine = new ShelterGovernanceEngine(policySys, leaderSys, json, serializer);

            // Verify all 4 canonical ideological blocs
            Assert.Equal(4, engine.Definitions.Count);
            Assert.True(engine.Definitions.ContainsKey("bloc_security_first"));
            Assert.True(engine.Definitions.ContainsKey("bloc_egalitarian_commons"));
            Assert.True(engine.Definitions.ContainsKey("bloc_free_pioneers"));
            Assert.True(engine.Definitions.ContainsKey("bloc_heritage_archive"));

            var secBloc = engine.Definitions["bloc_security_first"];
            Assert.Equal("Security & Order Vanguard", secBloc.display_name);
            Assert.Equal("Authoritarian", secBloc.core_ideology);
            Assert.Equal(25, secBloc.baseline_weight);
            Assert.Equal(5, secBloc.grievance_decay_rate);
            Assert.Contains("curfew", secBloc.supported_policy_scopes);

            var egalBloc = engine.Definitions["bloc_egalitarian_commons"];
            Assert.Equal("Communal Equality Union", egalBloc.display_name);
            Assert.Equal("Collectivist", egalBloc.core_ideology);
            Assert.Equal(30, egalBloc.baseline_weight);
            Assert.Equal(8, egalBloc.grievance_decay_rate);

            var freeBloc = engine.Definitions["bloc_free_pioneers"];
            Assert.Equal("Frontier Freeholders", freeBloc.display_name);
            Assert.Equal("Libertarian", freeBloc.core_ideology);
            Assert.Equal(20, freeBloc.baseline_weight);
            Assert.Equal(6, freeBloc.grievance_decay_rate);

            var archiveBloc = engine.Definitions["bloc_heritage_archive"];
            Assert.Equal("Old World Preservationists", archiveBloc.display_name);
            Assert.Equal("Traditionalist", archiveBloc.core_ideology);
            Assert.Equal(25, archiveBloc.baseline_weight);
            Assert.Equal(7, archiveBloc.grievance_decay_rate);

            // Assign survivors across blocs and test influence weight shift
            engine.AssignSurvivorToBloc("surv_guard", "bloc_security_first");
            engine.AssignSurvivorToBloc("surv_scav", "bloc_free_pioneers");
            engine.AssignSurvivorToBloc("surv_medic", "bloc_heritage_archive");
            engine.AssignSurvivorToBloc("surv_worker", "bloc_free_pioneers"); // 2/4 = 50% for free pioneers

            Assert.Equal("bloc_security_first", engine.GetSurvivorBloc("surv_guard"));
            Assert.Equal("bloc_free_pioneers", engine.GetSurvivorBloc("surv_scav"));
            Assert.Equal("bloc_heritage_archive", engine.GetSurvivorBloc("surv_medic"));
            Assert.Equal("bloc_free_pioneers", engine.GetSurvivorBloc("surv_worker"));

            int freeWeight = engine.Blocs["bloc_free_pioneers"].influence_weight_bp;
            int secWeight = engine.Blocs["bloc_security_first"].influence_weight_bp;
            Assert.True(freeWeight > secWeight, "Bloc with majority members holds higher influence weight");
        }

        [Fact]
        public void Plan159_ShelterGovernanceEngine_PolicyConsent_GrievanceAccumulation_AndDisputeResolution()
        {
            string blocsPath = Path.Combine(DataDirectory, "shelter_governance_blocs.json");
            string blocsJson = File.ReadAllText(blocsPath);
            var serializer = new SystemTextJsonSerializer();

            var policyCatalog = new PolicyCatalog();
            string policyJson = @"{
                ""schema_version"": 1,
                ""policies"": [
                    {
                        ""id"": ""policy_curfew"",
                        ""scope"": ""curfew"",
                        ""proposer_rules"": ""open_council"",
                        ""default_option_id"": ""curfew_standard"",
                        ""options"": [
                            { ""id"": ""curfew_none"", ""label"": ""No Curfew"" },
                            { ""id"": ""curfew_standard"", ""label"": ""Standard Curfew"" },
                            { ""id"": ""curfew_lockdown"", ""label"": ""Strict Lockdown"" }
                        ]
                    }
                ]
            }";
            policyCatalog.Load(policyJson, serializer);

            var policySys = new PolicySystem(policyCatalog);
            var leaderSys = new LeadershipSystem();
            leaderSys.GetAliveSurvivorIds = () => new List<string> { "surv_guard", "surv_scav" };

            var engine = new ShelterGovernanceEngine(policySys, leaderSys, blocsJson, serializer);

            engine.AssignSurvivorToBloc("surv_guard", "bloc_security_first");
            engine.AssignSurvivorToBloc("surv_scav", "bloc_free_pioneers");

            // Evaluate consent on curfew lockdown
            var consentEval = engine.EvaluatePolicyConsent("curfew", "curfew_lockdown");
            Assert.Contains("bloc_security_first", consentEval.supporting_bloc_ids);
            Assert.Contains("bloc_free_pioneers", consentEval.opposing_bloc_ids);

            // Enact policy
            int initialFreeGrievance = engine.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.Equal(0, initialFreeGrievance);

            var policyResult = policySys.SetPolicy("curfew", "curfew_lockdown", "surv_guard", day: 2);
            Assert.True(policyResult.IsSuccess);

            // Opposing bloc gains grievance
            int newFreeGrievance = engine.Blocs["bloc_free_pioneers"].grievance_bp;
            Assert.Equal(1500, newFreeGrievance);

            // Initial baseline stability
            int baseStability = engine.CalculateStabilityRating();

            // Open inter-bloc dispute: CurfewViolation
            var dispute = engine.OpenDispute("surv_guard", "surv_scav", GovernanceDisputeType.CurfewViolation, day: 3);
            Assert.NotNull(dispute);
            Assert.False(dispute.is_resolved);
            Assert.Equal("disp_1", dispute.case_id);

            int disputeStability = engine.CalculateStabilityRating();
            Assert.True(disputeStability < baseStability, "Unresolved disputes decrease shelter stability");

            // Resolve dispute via Restitution
            bool resolved = engine.ResolveDispute(dispute.case_id, GovernanceDisputeResolution.Restitution, day: 4);
            Assert.True(resolved);
            Assert.True(dispute.is_resolved);
            Assert.Equal(GovernanceDisputeResolution.Restitution, dispute.applied_resolution);
            Assert.Equal(4, dispute.resolution_day);

            // Round-trip persistence
            var capturedState = engine.CaptureState();
            var restoredEngine = new ShelterGovernanceEngine(policySys, leaderSys, blocsJson, serializer);
            restoredEngine.RestoreState(capturedState);

            Assert.Equal(capturedState.next_dispute_seq, restoredEngine.CaptureState().next_dispute_seq);
            Assert.Single(restoredEngine.Disputes);
            Assert.True(restoredEngine.Disputes[0].is_resolved);
            Assert.Equal("disp_1", restoredEngine.Disputes[0].case_id);
            Assert.Equal(GovernanceDisputeType.CurfewViolation, restoredEngine.Disputes[0].dispute_type);
            Assert.Equal("bloc_free_pioneers", restoredEngine.GetSurvivorBloc("surv_scav"));
        }

        [Fact]
        public void Plan190_RelicProvenanceCatalog_LoadsAuthoredDossiers_AndValidatesToneDistribution()
        {
            string relicPath = Path.Combine(DataDirectory, "narrative", "relic_provenance_dossiers.json");
            Assert.True(File.Exists(relicPath), $"relic_provenance_dossiers.json must exist at {relicPath}");

            string json = File.ReadAllText(relicPath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RelicProvenanceCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(32, catalog.AllRelics.Count);

            // Verify balanced tone distribution: 8 per tone
            var mysterious = catalog.GetByTone("Mysterious");
            var hilarious = catalog.GetByTone("Hilarious");
            var exciting = catalog.GetByTone("Exciting");
            var serious = catalog.GetByTone("Serious");

            Assert.Equal(8, mysterious.Count);
            Assert.Equal(8, hilarious.Count);
            Assert.Equal(8, exciting.Count);
            Assert.Equal(8, serious.Count);

            // Verify specific anchor entries
            var pocketwatch = catalog.GetById("relic_01_reverse_pocketwatch");
            Assert.NotNull(pocketwatch);
            Assert.Equal("The Sapper's Backward Watch", pocketwatch.name);
            Assert.Equal("Mysterious", pocketwatch.tone);
            Assert.False(string.IsNullOrWhiteSpace(pocketwatch.curator_note));
            Assert.False(string.IsNullOrWhiteSpace(pocketwatch.gameplay_effect));

            var chisel = catalog.GetById("relic_32_the_valley_constitution_chisel");
            Assert.NotNull(chisel);
            Assert.Equal("Sonya's Slate Scriber", chisel.name);
            Assert.Equal("Serious", chisel.tone);
            Assert.Contains("Century Seed", chisel.gameplay_effect);
        }

        [Fact]
        public void Plan190_ItemLoreSystem_NarrativeProgression_SignificanceElevation_AndRoundtrip()
        {
            var loreSystem = new ItemLoreSystem();

            // Track events
            int loreEventsFired = 0;
            SignificanceLevel lastSignificanceChange = SignificanceLevel.Mundane;
            string? lastTransferredOwner = null;

            loreSystem.OnLoreAdded += entry => loreEventsFired++;
            loreSystem.OnSignificanceChanged += (chain, level) => lastSignificanceChange = level;
            loreSystem.OnOwnershipTransferred += (itemId, owner) => lastTransferredOwner = owner;

            // 1. Register crafted relic
            string itemId = "relic_01_reverse_pocketwatch";
            var prov = loreSystem.RegisterItem(itemId, crafterId: "surv_clockmaker", craftingDay: 3, context: "forged in shelter workshop");

            Assert.NotNull(prov);
            Assert.Equal(itemId, prov.ItemInstanceId);
            Assert.Equal("surv_clockmaker", prov.CrafterSurvivorId);
            Assert.Equal(3, prov.CraftingDay);
            Assert.Equal(SignificanceLevel.Mundane, prov.Significance);
            Assert.Single(prov.OwnershipChain);
            Assert.Single(prov.LoreEntryIds);
            Assert.Equal(1, loreEventsFired);

            // 2. Add Combat lore -> entry count = 2 -> Notable
            loreSystem.AddLore(itemId, LoreTriggerType.Combat, "Survived EMP detonation during perimeter ambush.", day: 6, survivorId: "surv_clockmaker");
            Assert.Equal(SignificanceLevel.Notable, prov.Significance);
            Assert.Equal(SignificanceLevel.Notable, lastSignificanceChange);
            Assert.Equal(2, loreEventsFired);

            // 3. Transfer ownership to surv_scout -> entry count = 3 (Gift lore)
            bool transferred = loreSystem.TransferOwnership(itemId, "surv_scout", day: 10);
            Assert.True(transferred);
            Assert.Equal("surv_scout", lastTransferredOwner);
            Assert.Equal(2, prov.OwnershipChain.Count);
            Assert.Equal(3, prov.LoreEntryIds.Count);

            // 4. Add Trade lore -> entry count = 4 -> Important
            loreSystem.AddLore(itemId, LoreTriggerType.Trade, "Ransomed from nomadic scavengers for 40 water units.", day: 14, locationId: "loc_trading_post");
            Assert.Equal(SignificanceLevel.Important, prov.Significance);
            Assert.Equal(SignificanceLevel.Important, lastSignificanceChange);

            // 5. Add LossRecovery lore -> entry count = 5
            loreSystem.AddLore(itemId, LoreTriggerType.LossRecovery, "Recovered from crater after flash radiation storm.", day: 18, survivorId: "surv_scout");

            // 6. Add SignificantMoment lore -> entry count = 6 -> Legendary
            loreSystem.AddLore(itemId, LoreTriggerType.SignificantMoment, "Used to synchronize emergency generator restart during reactor breach.", day: 25);
            Assert.Equal(SignificanceLevel.Legendary, prov.Significance);
            Assert.Equal(SignificanceLevel.Legendary, lastSignificanceChange);

            var entries = loreSystem.GetLoreEntries(itemId);
            Assert.Equal(6, entries.Count);
            Assert.Equal(LoreTriggerType.Crafting, entries[0].TriggerType);
            Assert.Equal(LoreTriggerType.Combat, entries[1].TriggerType);
            Assert.Equal(LoreTriggerType.Gift, entries[2].TriggerType);
            Assert.Equal(LoreTriggerType.Trade, entries[3].TriggerType);
            Assert.Equal(LoreTriggerType.LossRecovery, entries[4].TriggerType);
            Assert.Equal(LoreTriggerType.SignificantMoment, entries[5].TriggerType);

            // Save and Restore Roundtrip
            var state = loreSystem.CaptureState();
            var restoredSystem = new ItemLoreSystem();
            restoredSystem.RestoreState(state);

            Assert.Equal(1, restoredSystem.TrackedItemCount);
            Assert.Equal(6, restoredSystem.TotalLoreEntriesCount);

            var restoredProv = restoredSystem.GetProvenance(itemId);
            Assert.NotNull(restoredProv);
            Assert.Equal(SignificanceLevel.Legendary, restoredProv.Significance);
            Assert.Equal("surv_clockmaker", restoredProv.CrafterSurvivorId);
            Assert.Equal(2, restoredProv.OwnershipChain.Count);
            Assert.Equal("surv_scout", restoredProv.OwnershipChain.Last());

            var restoredEntries = restoredSystem.GetLoreEntries(itemId);
            Assert.Equal(6, restoredEntries.Count);
            Assert.Equal("surv_clockmaker", restoredEntries[1].AssociatedSurvivorId);
            Assert.Equal("loc_trading_post", restoredEntries[3].AssociatedLocationId);
        }
    }
}
