// SPDX-License-Identifier: MIT
// Plans 202-205 flagship Wave F — unified 30-day cross-plan deterministic
// replay. All four engines run on forked campaign streams with identical
// day-indexed inputs; a mid-campaign save/load split (day 15 → fresh
// instances → restore → continue) must converge exactly with the continuous
// run (roadmap §12–§13). Hazard rolls are day-derived everywhere (house
// fresh-seed pattern), so no engine-internal RNG sequence crosses the split.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Defense;
using Ashfall.Core.Farming;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Integration
{
    public class Plans202To205CampaignIntegrationTests
    {
        private const int SplitDay = 15;
        private const int FinalDay = 30;

        // ── Deterministic day-indexed campaign inputs (single truths) ──

        private static bool SevereWeather(int day) => day % 5 == 0;           // storms every 5th day
        private static float GridPowerKwh(int day) => day % 7 == 0 ? 0f : 9f; // brownout weekly
        private static float WindDirDeg(int day) => (day * 47f) % 360f;
        private static float WindSpeedKph(int day) => 8f + (day * 13f) % 25f;

        /// <summary>One campaign: all four engines on forked seeds + shared inventory.</summary>
        private sealed class Campaign
        {
            public Inventory.Inventory Inv = new Inventory.Inventory();
            public PlasticPyrolysisSystem Pyrolysis = null!;
            public PerimeterDefenseSystem Perimeter = null!;
            public FungiCultivationSystem Fungi = null!;
            public CargoAirdropSystem Airdrop = null!;

            public static Campaign Create(int campaignSeed)
            {
                var inv = new Inventory.Inventory();
                // Shared starting stock — every engine draws from this one inventory.
                inv.AddById("plastic_sheet", 200);
                inv.AddById("scrap_plastic", 200);
                inv.AddById("scrap_metal", 200);
                inv.AddById("scrap_wood", 100);
                inv.AddById("clean_water", 300);
                inv.AddById("fuel", 100);
                inv.AddById("fungus_spores_common", 50);

                // Pyrolysis — forked stream (house campaign pattern).
                var pyroCatalog = new PlasticPyrolysisCatalog
                {
                    machine = new PyrolysisMachineDef
                    {
                        machine_id = "m1",
                        construction_required_items = new Dictionary<string, int> { ["scrap_metal"] = 8 },
                        maintenance_required_items = new Dictionary<string, int> { ["scrap_metal"] = 2 },
                        room_id = "room_generator"
                    },
                    feedstock_profiles = new List<PyrolysisFeedstockProfile>
                    {
                        new PyrolysisFeedstockProfile
                        {
                            feedstock_profile_id = "fp_mixed",
                            accepted_item_ids = new List<string> { "scrap_plastic" },
                            contamination_level = "mixed",
                            batch_input_units = 12,
                            energy_cost_kwh_per_day = 6f,
                            process_duration_days = 2,
                            liquid_fuel_yield_units = 3,
                            light_fraction_yield_units = 3,
                            solid_carbon_yield_units = 4,
                            offgas_energy_credit_kwh = 2f,
                            hazard_risk_bp = 300,
                            equipment_wear_per_batch = 2.5f,
                            operator_skill_risk_reduction_pct = 30f
                        }
                    },
                    outputs = new PyrolysisOutputMap
                    {
                        liquid_fuel_item_id = "synthetic_fuel_canister",
                        light_fraction_item_id = "fuel_1l",
                        solid_carbon_item_id = "carbon_black_powder"
                    }
                };
                var pyrolysis = new PlasticPyrolysisSystem(new SeededRng(campaignSeed * 10 + 1));
                pyrolysis.BindCatalog(pyroCatalog);
                pyrolysis.BindInventory(
                    itemId => inv.CountById(itemId),
                    (_, _) => true,
                    (itemId, amount) => inv.AddById(itemId, amount),
                    (itemId, amount) => inv.RemoveById(itemId, amount));

                // Perimeter — forked stream.
                var perimeterDefs = new List<PerimeterDefenseDefinition>
                {
                    new PerimeterDefenseDefinition
                    {
                        defense_id = "def_razorwire", display_name = "Razorwire", defense_type = "entanglement",
                        max_hp = 150, slow_factor = 0.6f, base_damage = 5f,
                        counter_tags = new List<string> { "cutting_tools" },
                        weather_wear_per_storm = 12f
                    },
                    new PerimeterDefenseDefinition
                    {
                        defense_id = "def_tripwire", display_name = "Flare Line", defense_type = "early_warning",
                        max_hp = 50, prevents_stealth_breach = true, night_accuracy_bonus = 0.3f,
                        counter_tags = new List<string> { "stealth" },
                        weather_wear_per_storm = 25f, false_alarm_rate_bp = 300, alert_device = true
                    }
                };
                var perimeter = new PerimeterDefenseSystem(
                    perimeterDefs, inv, new SeededRng(campaignSeed * 10 + 2));

                // Fungi — forked stream.
                var fungiCatalog = new UndergroundFloraCatalog
                {
                    strains = new List<FungusStrainDef>
                    {
                        new FungusStrainDef
                        {
                            strain_id = "strain_edible", category = "Edible", growth_days = 2,
                            moisture_min = 0.3f, moisture_max = 0.95f,
                            temperature_min = 4f, temperature_max = 32f,
                            darkness_required = true, spore_hazard = 0.1f,
                            yield_item_id = "harvested_mushrooms_subterranean",
                            yield_count = 6, flush_count = 3
                        }
                    },
                    substrates = new List<SubstrateDef>
                    {
                        new SubstrateDef
                        {
                            substrate_id = "substrate_compost", nutrition_multiplier = 1.0f,
                            moisture_retention = 0.8f, contamination_risk = 0.1f
                        }
                    }
                };
                var fungi = new FungiCultivationSystem(new SeededRng(campaignSeed * 10 + 3), inv);
                fungi.RegisterCatalog(fungiCatalog);

                // Airdrop — forked stream; wind from the shared day table.
                var airdropCatalog = new CargoAirdropCatalog
                {
                    descent_bands = 3,
                    max_active_drops = 2,
                    interception = new AirdropInterceptionDef { rate_per_day_bp = 800, expired_grace_days = 2 },
                    drop_profiles = new List<AirdropProfileDef>
                    {
                        new AirdropProfileDef
                        {
                            drop_profile_id = "drop_fuel",
                            trigger_signal_outcomes = new List<string> { "supply_cache" },
                            cargo_pool_id = "pool_fuel",
                            wind_sensitivity = 1.0f,
                            base_impact_kph = 20f,
                            wind_impact_factor_kph = 0.5f,
                            light_impact_kph = 25f,
                            heavy_impact_kph = 40f,
                            integrity_loss_light_pct = 15,
                            integrity_loss_heavy_pct = 50,
                            beacon_duration_days = 4,
                            interception_rate_per_day_bp = 1200
                        }
                    },
                    cargo_pools = new List<AirdropCargoPoolDef>
                    {
                        new AirdropCargoPoolDef
                        {
                            cargo_pool_id = "pool_fuel",
                            entries = new List<AirdropCargoEntryDef>
                            {
                                new AirdropCargoEntryDef { item_id = "fuel", quantity = 3, weight_kg_per_unit = 2f },
                                new AirdropCargoEntryDef { item_id = "scrap_metal", quantity = 4, weight_kg_per_unit = 0.5f }
                            }
                        }
                    }
                };
                var airdrop = new CargoAirdropSystem(new SeededRng(campaignSeed * 10 + 4));
                airdrop.BindCatalog(airdropCatalog);

                var c = new Campaign
                {
                    Inv = inv,
                    Pyrolysis = pyrolysis,
                    Perimeter = perimeter,
                    Fungi = fungi,
                    Airdrop = airdrop
                };

                // Providers capture the campaign instance — the driver sets
                // c.campaignDay before each day's commands.
                pyrolysis.DayProvider = () => c.campaignDay;
                pyrolysis.OperatorSkillProvider = () => 0.5f;
                fungi.DayProvider = () => c.campaignDay;
                airdrop.DayProvider = () => c.campaignDay;
                airdrop.WindDirectionDeg = () => WindDirDeg(c.campaignDay);
                airdrop.WindSpeedKph = () => WindSpeedKph(c.campaignDay);

                return c;
            }

            /// <summary>The campaign day — set by the driver before each day's commands.</summary>
            public int campaignDay;
        }

        // ── Identical daily command script (pure function of the day) ──

        private static void RunDay(Campaign c, int day)
        {
            c.campaignDay = day;

            // Plan 202: keep the retort charged; claim outputs when staged.
            if (!c.Pyrolysis.State.machine_constructed)
                c.Pyrolysis.ConstructMachine();
            else
            {
                if (c.Pyrolysis.State.active_batch == null
                    && c.Pyrolysis.State.output_buffer.Count < c.Pyrolysis.Catalog.storage_buffer_max_batches
                    && day % 2 == 1)
                    c.Pyrolysis.StartBatch("fp_mixed");
                c.Pyrolysis.ClaimOutputs();
            }

            // Plan 203: raise the perimeter grid once, keep the turret fed,
            // reset spent alarms.
            if (c.Perimeter.Emplacements.Count == 0)
            {
                c.Perimeter.ConstructEmplacement("def_razorwire");
                c.Perimeter.ConstructEmplacement("def_tripwire");
            }
            foreach (var emp in c.Perimeter.Emplacements)
            {
                if (!emp.is_destroyed && emp.magazine_capacity > 0 && emp.loaded_ammo_count == 0)
                    c.Perimeter.LoadAmmo(emp.emplacement_id, emp.magazine_capacity);
            }
            foreach (var sector in c.Perimeter.Sectors.ToList())
            {
                if (sector.alarm_spent)
                    c.Perimeter.ResetSectorAlarm(sector.sector_id);
            }

            // Plan 204: keep the bed watered and replanted after fallow.
            if (c.Fungi.State.plots.Count == 0)
            {
                c.Fungi.EnsurePlot("plot_main", "room_grow");
                c.Fungi.PrepareSubstrate("plot_main", useHeat: false);
                c.Fungi.CultivateSpores("plot_main", "strain_edible", "substrate_compost", day);
            }
            else
            {
                var plot = c.Fungi.State.plots[0];
                if (string.IsNullOrEmpty(plot.strainId) && !plot.hasToxicBloom)
                {
                    c.Fungi.CultivateSpores("plot_main", "strain_edible", "substrate_compost", day);
                }
                if (plot.isHarvestReady)
                    c.Fungi.HarvestPlot("plot_main");
            }
            c.Fungi.WaterPlot("plot_main");

            // Plan 205: keep one drop inbound; collect from landed crates.
            if (c.Airdrop.State.drops.Count(d => d.phase == "scheduled" || d.phase == "descending") == 0
                && day % 6 == 1)
                c.Airdrop.ScheduleDrop("drop_fuel", $"sig_{day}", 0, 0);
            foreach (var drop in c.Airdrop.State.drops.ToList())
            {
                if (drop.phase == "landed" && drop.remaining_contents.Count > 0)
                    c.Airdrop.CollectCrate(drop.event_id, (itemId, weight, qty) => true);
            }

            // ── Shared daily tick ordering (roadmap §3) ──
            c.Perimeter.TickDay(day, SevereWeather(day));
            c.Pyrolysis.TickDay(GridPowerKwh(day));
            c.Fungi.TickDay(day, roomIsDark: true, roomTemperatureC: 15f);
            c.Airdrop.TickDay(day);
        }

        private static void RunSpan(Campaign c, int fromDay, int toDay)
        {
            for (int day = fromDay; day <= toDay; day++)
                RunDay(c, day);
        }

        /// <summary>Authoritative cross-plan snapshot (all four engine states).</summary>
        private static string Snapshot(Campaign c)
        {
            var parts = new Dictionary<string, string>
            {
                ["pyrolysis"] = System.Text.Json.JsonSerializer.Serialize(c.Pyrolysis.CaptureState()),
                ["perimeter"] = System.Text.Json.JsonSerializer.Serialize(c.Perimeter.CaptureState()),
                ["fungi"] = System.Text.Json.JsonSerializer.Serialize(c.Fungi.State),
                ["airdrop"] = System.Text.Json.JsonSerializer.Serialize(c.Airdrop.CaptureState())
            };
            return System.Text.Json.JsonSerializer.Serialize(parts);
        }

        /// <summary>Serializes the four engine states (the campaign-envelope split).</summary>
        private static Dictionary<string, string> CaptureSections(Campaign c)
        {
            return new Dictionary<string, string>
            {
                ["plastic_pyrolysis"] = System.Text.Json.JsonSerializer.Serialize(c.Pyrolysis.CaptureState()),
                ["perimeter_defense"] = System.Text.Json.JsonSerializer.Serialize(c.Perimeter.CaptureState()),
                ["fungi_cultivation"] = System.Text.Json.JsonSerializer.Serialize(c.Fungi.State),
                ["cargo_airdrop"] = System.Text.Json.JsonSerializer.Serialize(c.Airdrop.CaptureState())
            };
        }

        private static Campaign RestoreFromSections(int campaignSeed, Dictionary<string, string> sections)
        {
            var c = Campaign.Create(campaignSeed);
            c.Pyrolysis.RestoreState(System.Text.Json.JsonSerializer.Deserialize<PlasticPyrolysisState>(sections["plastic_pyrolysis"]));
            c.Perimeter.RestoreState(System.Text.Json.JsonSerializer.Deserialize<PerimeterDefenseSave>(sections["perimeter_defense"]));
            c.Fungi.RestoreState(System.Text.Json.JsonSerializer.Deserialize<FungiCultivationState>(sections["fungi_cultivation"]));
            c.Airdrop.RestoreState(System.Text.Json.JsonSerializer.Deserialize<CargoAirdropState>(sections["cargo_airdrop"]));
            return c;
        }

        // ── Wave F: §12 same-seed replay ────────────────────────────────

        [Fact]
        public void SameSeed_TwentyContinuousDays_ProduceIdenticalStates()
        {
            var a = Campaign.Create(20250);
            RunSpan(a, 1, 20);
            var b = Campaign.Create(20250);
            RunSpan(b, 1, 20);

            Assert.Equal(Snapshot(a), Snapshot(b));
        }

        // ── Wave F: §13 save/load split convergence ─────────────────────

        [Fact]
        public void ThirtyDayCampaign_SplitAtDay15_ConvergesWithContinuous()
        {
            // Run A — 30 continuous days.
            var continuous = Campaign.Create(777);
            RunSpan(continuous, 1, FinalDay);
            var continuousSnapshot = Snapshot(continuous);

            // Run B — 15 days → save → fresh instances → restore → 16 more days.
            var split = Campaign.Create(777);
            RunSpan(split, 1, SplitDay);
            var sections = CaptureSections(split);

            var restored = RestoreFromSections(777, sections);
            RunSpan(restored, SplitDay + 1, FinalDay);
            var splitSnapshot = Snapshot(restored);

            Assert.Equal(continuousSnapshot, splitSnapshot);
        }

        [Fact]
        public void SplitAtEverySevenDays_AlsoConverges()
        {
            // Continuous.
            var continuous = Campaign.Create(4242);
            RunSpan(continuous, 1, FinalDay);

            // Split at days 7, 14, 21, 28.
            var split = Campaign.Create(4242);
            int lastSplit = 1;
            foreach (var splitDay in new[] { 7, 14, 21, 28 })
            {
                RunSpan(split, lastSplit, splitDay);
                var sections = CaptureSections(split);
                split = RestoreFromSections(4242, sections);
                lastSplit = splitDay + 1;
            }
            RunSpan(split, lastSplit, FinalDay);

            Assert.Equal(Snapshot(continuous), Snapshot(split));
        }

        // ── §12 daily-capture assertions on the connected loop ──────────

        [Fact]
        public void ConnectedCampaign_ProducesRealOutputs_AndNoNegativeInventory()
        {
            var c = Campaign.Create(31337);
            RunSpan(c, 1, FinalDay);

            // Fuel was produced and claimed into the shared inventory.
            Assert.True(c.Inv.CountById("synthetic_fuel_canister") > 0
                     || c.Inv.CountById("fuel_1l") > 0,
                "The retort must have produced fuel over 30 days.");
            Assert.True(c.Inv.CountById("carbon_black_powder") > 0,
                "The carbon byproduct must have a produced stock.");

            // Mushrooms fed the kitchen chain through inventory.
            Assert.True(c.Inv.CountById("harvested_mushrooms_subterranean") > 0,
                "The fungi bed must have produced food over 30 days.");

            // Airdrop crates delivered fuel through the expedition-transfer path.
            Assert.True(c.Airdrop.State.total_recovered > 0
                     || c.Airdrop.State.drops.Any(d => d.remaining_contents.Count < d.crate_contents.Count),
                "At least one airdrop must have transferred part or all of its cargo.");

            // No negative inventory anywhere.
            foreach (var count in new[]
            {
                c.Inv.CountById("plastic_sheet"), c.Inv.CountById("scrap_plastic"),
                c.Inv.CountById("scrap_metal"), c.Inv.CountById("clean_water"),
                c.Inv.CountById("fuel"), c.Inv.CountById("fungus_spores_common")
            })
                Assert.True(count >= 0, "Inventory went negative — duplication or double-spend.");

            // Perimeter weather wear actually bit (storms every 5th day).
            Assert.True(c.Perimeter.Emplacements.Any(e => e.current_hp < e.max_hp),
                "Severe weather must have worn the perimeter over 30 days.");

            // Bounded intrusion log stayed bounded.
            Assert.True(c.Perimeter.IntrusionLog.Count <= PerimeterDefenseSystem.IntrusionLogCapacity);
        }
    }
}
