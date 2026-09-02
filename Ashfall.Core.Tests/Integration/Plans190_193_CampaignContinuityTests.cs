// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Ashfall.Core.Narrative;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans190_193_CampaignContinuityTests
    {
        [Fact]
        public void MultiSystem_Simulation_And_Campaign_Continuity()
        {
            var rng = new SeededRng(190193);
            var inv = new Inventory.Inventory();
            var needs = new NeedsSystem();

            var dwellerA = new SurvivorNeedsState { Id = "survivor_miner", Health = 100f, Morale = 60f };
            var dwellerB = new SurvivorNeedsState { Id = "survivor_doc", Health = 100f, Morale = 60f };
            needs.Register(dwellerA);
            needs.Register(dwellerB);

            // Stockpile items
            inv.AddById("surgical_saw", 1);
            inv.AddById("painkillers", 5);
            inv.AddById("clean_water", 10);
            inv.AddById("cloth", 5);
            inv.AddById("prosthetic_wooden_leg", 1);
            inv.AddById("bionic_leg_prototype", 1);
            inv.AddById("fungus_spores_common", 5);
            inv.AddById("train_coal", 50);

            // 1. Initialize systems
            var amputation = new AmputationSystem(new SeededRng(190), inv, needs);
            var railway = new RailwaySystem(new SeededRng(191), inv);
            var fungi = new FungiCultivationSystem(new SeededRng(192), inv);
            var justice = new JusticeSystem(new SeededRng(193), inv, needs);

            // Catalogs
            railway.RegisterCatalog(new RailwayNetworkCatalog
            {
                nodes = new List<RailNodeDef>
                {
                    new RailNodeDef { node_id = "node_terminal", zone_id = "loc_holdfast" },
                    new RailNodeDef { node_id = "node_outpost", zone_id = "loc_delta" }
                },
                segments = new List<TrackSegmentDef>
                {
                    new TrackSegmentDef
                    {
                        segment_id = "seg_main",
                        start_node_id = "node_terminal",
                        end_node_id = "node_outpost",
                        distance_km = 15f,
                        base_integrity = 0.85f
                    }
                }
            });

            fungi.RegisterCatalog(new UndergroundFloraCatalog
            {
                strains = new List<FungusStrainDef>
                {
                    new FungusStrainDef
                    {
                        strain_id = "strain_grey_mycelium",
                        growth_days = 2,
                        moisture_min = 0.3f,
                        moisture_max = 0.9f,
                        yield_item_id = "harvested_mushrooms_subterranean",
                        yield_count = 6
                    }
                },
                substrates = new List<SubstrateDef>
                {
                    new SubstrateDef { substrate_id = "sub_compost", nutrition_multiplier = 1.0f }
                }
            });

            justice.RegisterLaw(new WastelandLawDef
            {
                law_id = "law_theft",
                crime_type = "Theft",
                min_evidence_confidence = 0.5f,
                allowed_punishments = new List<string> { "Restitution" }
            });

            amputation.RegisterProcedure(new SurgicalProcedureDef
            {
                procedure_id = "procedure_amputation_leg_field",
                required_tool_id = "surgical_saw",
                required_items = new List<SurgicalItemCost>
                {
                    new SurgicalItemCost { item_id = "painkillers", amount = 1 },
                    new SurgicalItemCost { item_id = "clean_water", amount = 1 },
                    new SurgicalItemCost { item_id = "cloth", amount = 2 }
                },
                base_shock_risk = 0.0f
            });

            // 2. Run simulation day 1
            // Fungi plant
            fungi.EnsurePlot("plot_cellar", "room_cellar");
            var plantRes = fungi.CultivateSpores("plot_cellar", "strain_grey_mycelium", "sub_compost", 1);
            Assert.True(plantRes.IsSuccess);

            // Justice report crime & add evidence
            justice.ReportCrime("inc_food_theft", CrimeType.Theft, "survivor_miner", null, 1);
            justice.AddEvidence("inc_food_theft", "clue_crumb", "Crumbs in locker", 0.70f, 1);

            // Train create
            var train = railway.CreateStarterTrain("loco_01", "Grizzly", "node_terminal");

            // Amputation deep wound
            amputation.InflictDeepWound("survivor_miner", LimbId.LeftLeg);

            // 3. Advance to Day 2 and Day 3
            amputation.TickDay(2);
            fungi.TickDay(2, roomIsDark: true);
            fungi.TickDay(3, roomIsDark: true);
            justice.TickDay(2);

            // 4. Resolve Justice Trial (Restitution -> awards scrap)
            var trialRes = justice.HoldTrial(new TrialDecision
            {
                incidentId = "inc_food_theft",
                verdict = TrialVerdict.Guilty,
                punishment = PunishmentLevel.Restitution
            }, 2);
            Assert.True(trialRes.Success);
            Assert.True(inv.CountById("scrap_metal") >= 10);

            // Use awarded scrap to repair damaged track
            railway.EnsureSegmentState("seg_main").integrity = 0.5f;
            var repairRes = railway.RepairTrack("seg_main", 0.4f);
            Assert.True(repairRes.IsSuccess);

            // 5. Dispatch Train
            var dispatchRes = railway.DispatchTrain("loco_01", "seg_main");
            Assert.True(dispatchRes.IsSuccess);
            railway.TickTravel("loco_01", 1.0f);
            Assert.Equal(TrainDispatchStatus.Arrived, train.status);

            // 6. Harvest Fungi
            var harvestRes = fungi.HarvestPlot("plot_cellar");
            Assert.True(harvestRes.IsSuccess);
            Assert.True(inv.CountById("harvested_mushrooms_subterranean") >= 6);

            // 7. Perform Amputation on infected limb
            var ampRes = amputation.PerformAmputation("survivor_miner", LimbId.LeftLeg, "procedure_amputation_leg_field");
            Assert.True(ampRes.Success);
            var limb = amputation.GetLimb("survivor_miner", LimbId.LeftLeg);
            Assert.Equal(LimbCondition.Amputated, limb!.condition);

            // 8. Capture & Restore across all 4 systems
            var ampJson = System.Text.Json.JsonSerializer.Serialize(amputation.State);
            var railJson = System.Text.Json.JsonSerializer.Serialize(railway.State);
            var fungiJson = System.Text.Json.JsonSerializer.Serialize(fungi.State);
            var justJson = System.Text.Json.JsonSerializer.Serialize(justice.State);

            var amp2 = new AmputationSystem(new SeededRng(190), inv, needs);
            var rail2 = new RailwaySystem(new SeededRng(191), inv);
            var fungi2 = new FungiCultivationSystem(new SeededRng(192), inv);
            var just2 = new JusticeSystem(new SeededRng(193), inv, needs);

            amp2.RestoreState(System.Text.Json.JsonSerializer.Deserialize<AmputationSystemState>(ampJson)!);
            rail2.RestoreState(System.Text.Json.JsonSerializer.Deserialize<RailwayState>(railJson)!);
            fungi2.RestoreState(System.Text.Json.JsonSerializer.Deserialize<FungiCultivationState>(fungiJson)!);
            just2.RestoreState(System.Text.Json.JsonSerializer.Deserialize<JusticeState>(justJson)!);

            Assert.Equal(1, amp2.State.totalAmputationsPerformed);
            Assert.Equal("node_outpost", rail2.State.trains[0].currentNodeId);
            Assert.Equal(1, fungi2.State.totalHarvests);
            Assert.Single(just2.State.incidents);
            Assert.Equal(TrialVerdict.Guilty, just2.State.incidents[0].verdict);
        }
    }
}
