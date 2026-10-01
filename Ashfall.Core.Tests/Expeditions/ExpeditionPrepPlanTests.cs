// SPDX-License-Identifier: MIT
// P105/P106 — pre-dispatch prep projection: checklist from shelter inventory,
// supply burn, risk grading, and failure-outcome classification. Pure Core;
// no engine, no save state.
using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public sealed class ExpeditionPrepPlanTests
    {
        private static ExpeditionDefinition Def(
            int danger = 1, float encounter = 0.12f, int ticks = 6, string id = "loc_prep_site")
        {
            return new ExpeditionDefinition
            {
                id = id,
                displayName = "Prep Site",
                distanceTicks = ticks,
                dangerLevel = danger,
                encounterChancePerTick = encounter
            };
        }

        private static ExpeditionPrepPlan Build(
            ExpeditionDefinition def,
            ExpeditionEstimate est,
            IDictionary<string, int> stock,
            bool weaponReady = true,
            bool hasLight = true)
        {
            return ExpeditionPrepPlanner.Build(def, est, id => stock.TryGetValue(id, out int n) ? n : 0,
                weaponReady, hasLight);
        }

        [Fact]
        public void Checklist_HasFixedFiveRows_InOrder()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>());

            Assert.Equal(5, plan.Checklist.Count);
            Assert.Equal(new[] { "food", "water", "rad_meds", "weapon", "light" },
                plan.Checklist.ConvertAll(r => r.Id).ToArray());
        }

        [Fact]
        public void EmptyShelter_IsNotReady_AndNamesEachShortage()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>(), weaponReady: false, hasLight: false);

            Assert.False(plan.Ready);
            // food, water and weapon are short; light is advisory; rad meds are
            // not required. Advisory rows never gate readiness.
            Assert.Equal(3, plan.MissingCount);
            foreach (var row in plan.Checklist)
            {
                if (row.Id == "rad_meds") continue;
                Assert.False(row.Satisfied);
            }
            Assert.True(plan.Checklist.Find(r => r.Id == "rad_meds")!.Satisfied);
        }

        [Fact]
        public void StockedShelter_IsReady()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var stock = new Dictionary<string, int>
            {
                ["canned_food"] = 10,
                ["clean_water"] = 10,
                ["anti_rad"] = 2,
                ["headlamp"] = 1
            };
            var plan = Build(def, est, stock);

            Assert.True(plan.Ready);
            Assert.Equal(0, plan.MissingCount);
        }

        [Fact]
        public void SupplyBurn_UsesAtLeastOneOvernightWindow()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>());

            int nights = Math.Max(1, (int)Math.Ceiling(est.totalTicks / ExpeditionPrepPlanner.HoursPerOvernight));
            int expected = Math.Max(1, (int)Math.Ceiling(nights * ExpeditionPrepPlanner.UnitsPerOvernight));
            Assert.True(expected >= 2);
            Assert.Equal(expected, plan.FoodBurn);
            Assert.Equal(plan.FoodBurn, plan.WaterBurn);
            Assert.Equal(plan.FoodBurn, plan.Checklist.Find(r => r.Id == "food")!.Required);
            Assert.Equal(plan.WaterBurn, plan.Checklist.Find(r => r.Id == "water")!.Required);
        }

        [Fact]
        public void HeldCounts_SumAcrossAcceptableItemVariants()
        {
            var def = Def(ticks: 1);
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var stock = new Dictionary<string, int>
            {
                ["canned_food"] = 1,
                ["dried_rations"] = 1,
                ["clean_water"] = 1,
                ["clean_water_jug"] = 1
            };
            var plan = Build(def, est, stock);

            Assert.Equal(2, plan.Checklist.Find(r => r.Id == "food")!.Held);
            Assert.True(plan.Checklist.Find(r => r.Id == "food")!.Satisfied);
            Assert.Equal(2, plan.Checklist.Find(r => r.Id == "water")!.Held);
            Assert.True(plan.Checklist.Find(r => r.Id == "water")!.Satisfied);
        }

        [Fact]
        public void RadMeds_NotRequired_WithoutProjectedDose()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>());

            var rad = plan.Checklist.Find(r => r.Id == "rad_meds")!;
            Assert.Equal(0, rad.Required);
            Assert.True(rad.Satisfied);
        }

        [Fact]
        public void RadMeds_Required_WhenEstimateProjectsADose()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth, protective: new ExpeditionProtectiveInputs
            {
                LocationRadRatePerHour = 24f,
                WorkingProtection = 0f,
                UnprotectedCount = 1,
                HoursPerTick = 1f
            });
            var plan = Build(def, est, new Dictionary<string, int>());

            var rad = plan.Checklist.Find(r => r.Id == "rad_meds")!;
            Assert.Equal(1, rad.Required);
            Assert.False(rad.Satisfied);
            Assert.True(est.projectedDoseTotal > 0f);
        }

        [Fact]
        public void WeaponAndLight_AreEquipmentReadinessRows()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>(), weaponReady: true, hasLight: false);

            var weapon = plan.Checklist.Find(r => r.Id == "weapon")!;
            var light = plan.Checklist.Find(r => r.Id == "light")!;
            Assert.True(weapon.IsEquipment);
            Assert.True(weapon.Satisfied);
            Assert.True(light.IsEquipment);
            Assert.False(light.Satisfied);
        }

        [Theory]
        [InlineData(0, ExpeditionRiskGrade.Low)]
        [InlineData(1, ExpeditionRiskGrade.Low)]
        [InlineData(3, ExpeditionRiskGrade.Moderate)]
        [InlineData(5, ExpeditionRiskGrade.High)]
        [InlineData(6, ExpeditionRiskGrade.Extreme)]
        public void RiskGrade_ScalesWithAuthoredDanger(int danger, ExpeditionRiskGrade expected)
        {
            var def = Def(danger: danger);
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed);
            Assert.Equal(expected, ExpeditionPrepPlanner.GradeRisk(def, est));
        }

        [Fact]
        public void RiskGrade_Extreme_WhenBreakdownAndDoseAndFailureAlign()
        {
            var def = Def(danger: 3);
            var vehicle = new ExpeditionVehicleProfile
            {
                vehicleId = "vehicle_test",
                speedMultiplier = 1f,
                breakdownChancePerTick = 0.25f,
                fuelPerTravelTick = 1f
            };
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed, vehicle: vehicle,
                protective: new ExpeditionProtectiveInputs
                {
                    LocationRadRatePerHour = 60f,
                    WorkingProtection = 0f,
                    UnprotectedCount = 1,
                    HoursPerTick = 1f
                });
            Assert.True(est.predictsMidRouteFailure || est.breakdownRiskTotal >= 0.15f);
            Assert.True((int)ExpeditionPrepPlanner.GradeRisk(def, est) >= (int)ExpeditionRiskGrade.High);
        }

        [Fact]
        public void FailureHealthLoss_KnownAndUnknownReasons()
        {
            Assert.Equal(ExpeditionPrepPlanner.FailureInjuryHealthLoss,
                ExpeditionPrepPlanner.FailureHealthLoss("Collapsed from exhaustion."));
            Assert.Equal(ExpeditionPrepPlanner.FailureInjuryHealthLoss,
                ExpeditionPrepPlanner.FailureHealthLoss("Collapsed during overnight camp."));
            Assert.Equal(12f, ExpeditionPrepPlanner.FailureHealthLoss("Something unexplained."));
        }

        [Fact]
        public void ResolveFailureOutcome_InjuredOrLost()
        {
            Assert.Equal(ExpeditionReturnClass.Injured,
                ExpeditionPrepPlanner.ResolveFailureOutcome("Collapsed from exhaustion.", 60f, 18f));
            Assert.Equal(ExpeditionReturnClass.Lost,
                ExpeditionPrepPlanner.ResolveFailureOutcome("Collapsed from exhaustion.", 10f, 18f));
        }

        [Fact]
        public void Summary_ReportsDurationBurnRiskAndChecklist()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>(), weaponReady: false, hasLight: false);

            string summary = plan.Summary();
            Assert.Contains("PREP", summary);
            Assert.Contains("burn food", summary);
            Assert.Contains("LOW", summary);
            Assert.Contains("[!] LIGHT", summary);
            Assert.False(plan.Ready);
        }

        [Fact]
        public void Build_DoesNotMutateEstimateOrInventory()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            float ticks = est.totalTicks;
            var stock = new Dictionary<string, int> { ["canned_food"] = 5 };
            Build(def, est, stock);
            Assert.Equal(ticks, est.totalTicks);
            Assert.Equal(5, stock["canned_food"]);
        }

        [Fact]
        public void LightRow_IsAdvisory_AndNeverGatesReadiness()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var stock = new Dictionary<string, int>
            {
                ["canned_food"] = 10,
                ["clean_water"] = 10,
                ["anti_rad"] = 2
            };
            // No light; weapon ready. The plan is READY because light is advisory.
            var plan = Build(def, est, stock, weaponReady: true, hasLight: false);
            var light = plan.Checklist.Find(r => r.Id == "light")!;
            Assert.True(light.Advisory);
            Assert.False(light.Satisfied);
            Assert.True(plan.Ready);
            Assert.Equal(0, plan.MissingCount);
            Assert.Contains("LIGHT", plan.ChecklistLine());
        }

        [Fact]
        public void RiskNote_NamesTheDominantDriver()
        {
            var def = Def(danger: 5);
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed);
            var plan = Build(def, est, new Dictionary<string, int>());
            Assert.Contains("danger lvl 5", plan.RiskNote);
            Assert.Contains("danger lvl 5", plan.Summary());
        }

        [Fact]
        public void OvernightLine_UsesAuthoredCampConstants()
        {
            var def = Def(ticks: 6);
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = Build(def, est, new Dictionary<string, int>());

            Assert.True(plan.OvernightNights >= 1);
            Assert.Equal(
                plan.OvernightNights * ExpeditionSystem.CampFirewoodPerSegment * ExpeditionSystem.CampNightSegments,
                plan.CampFirewoodUnits);
            Assert.Contains("CAMP BURN", plan.OvernightLine());
        }

        [Fact]
        public void OvernightLine_IsEmptyWithoutAnOvernightWindow()
        {
            var plan = new ExpeditionPrepPlan { OvernightNights = 0 };
            Assert.Equal(string.Empty, plan.OvernightLine());

            plan.OvernightNights = -2;
            Assert.Equal(string.Empty, plan.OvernightLine());
        }

        [Fact]
        public void ReturnReport_CeremonyAndAftermath()
        {
            var state = new ExpeditionState
            {
                survivorId = "survivor_test",
                locationId = "loc_test",
                displayName = "Test Site",
                loot = new List<ExpeditionLootEntry>
                {
                    new ExpeditionLootEntry { itemId = "canned_food", quantity = 2, weightKg = 1f },
                    new ExpeditionLootEntry { itemId = "clean_water", quantity = 1, weightKg = 0.5f }
                }
            };
            string ceremony = ExpeditionReturnReport.BuildCeremony(state, id => id == "canned_food" ? "Canned Food" : null);
            Assert.Contains("RETURN LOOT CEREMONY", ceremony);
            Assert.Contains("Canned Food ×2", ceremony);
            Assert.Contains("3 item(s)", ceremony);

            string aftermath = ExpeditionReturnReport.BuildAftermath(state, "Collapsed from exhaustion.");
            Assert.Contains("SORTIE AFTERMATH", aftermath);
            Assert.Contains("Collapsed from exhaustion.", aftermath);
            Assert.Contains("lost in the field", aftermath);
        }

        [Fact]
        public void ReturnReport_Aftermath_NamesEachLostLootLine()
        {
            var state = new ExpeditionState
            {
                survivorId = "survivor_test",
                locationId = "loc_test",
                displayName = "Test Site",
                loot = new List<ExpeditionLootEntry>
                {
                    new ExpeditionLootEntry { itemId = "canned_food", quantity = 2, weightKg = 1f },
                    new ExpeditionLootEntry { itemId = "scrap_metal", quantity = 3, weightKg = 2f }
                }
            };
            string aftermath = ExpeditionReturnReport.BuildAftermath(state, "Collapsed from exhaustion.");
            Assert.Contains("2 salvage line(s) lost in the field", aftermath);
        }

        [Fact]
        public void ReturnReport_EmptyLootSaysNoSalvage()
        {
            var state = new ExpeditionState { survivorId = "s", displayName = "Site" };
            string ceremony = ExpeditionReturnReport.BuildCeremony(state, null);
            Assert.Contains("No salvage recovered", ceremony);
        }

        [Fact]
        public void ReturnReport_NullState_ReturnsEmpty()
        {
            Assert.Equal(string.Empty, ExpeditionReturnReport.BuildCeremony(null!, null));
            Assert.Equal(string.Empty, ExpeditionReturnReport.BuildAftermath(null!, "reason"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void FailureHealthLoss_BlankReason_ReturnsDefault(string? reason)
        {
            Assert.Equal(12f, ExpeditionPrepPlanner.FailureHealthLoss(reason!));
        }

        [Theory]
        [InlineData("canned_food", "Canned Food")]
        [InlineData("clean_water_jug", "Clean Water Jug")]
        [InlineData("", "")]
        [InlineData("   ", "")]
        public void HumanizeToken_StableIdToTitleCase(string id, string expected)
        {
            Assert.Equal(expected, ExpeditionReturnReport.HumanizeToken(id));
        }

        [Fact]
        public void OvernightNights_AreClamped()
        {
            var def = Def(ticks: 100000);
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed);
            var plan = Build(def, est, new Dictionary<string, int>());
            Assert.Equal(ExpeditionPrepPlanner.MaxOvernightNights, plan.OvernightNights);
            Assert.True(plan.CampFirewoodUnits > 0f);
        }

        [Fact]
        public void Build_WithVehicleProfile_ReportsNonZeroFuel()
        {
            var def = Def(ticks: 6);
            var vehicle = new ExpeditionVehicleProfile
            {
                vehicleId = "vehicle_test",
                speedMultiplier = 1f,
                fuelPerTravelTick = 1.5f
            };
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Speed, vehicle: vehicle);

            Assert.True(est.usingVehicle);
            Assert.True(est.fuelRequired > 0f);

            // The prep plan still builds from the vehicle-adjusted estimate.
            var plan = Build(def, est, new Dictionary<string, int>());
            Assert.NotNull(plan);
            Assert.True(plan.FoodBurn >= 1);
        }

        [Fact]
        public void InjuryDigest_ExcludesDeadSurvivors_KeepsLiving()
        {
            var roster = new List<(string Id, bool Alive)>
            {
                ("survivor_a", true),
                ("survivor_b", false),
                ("survivor_c", true)
            };
            HealthEvent? Lookup(string id) => id switch
            {
                "survivor_a" => new HealthEvent { SurvivorId = id, EventType = "expedition_failure", EventDay = 4, Description = "Collapsed from exhaustion." },
                "survivor_b" => new HealthEvent { SurvivorId = id, EventType = "expedition_failure", EventDay = 2, Description = "dead" },
                _ => null
            };

            var rows = ExpeditionInjuryDigest.Build(roster, Lookup);
            Assert.Single(rows);
            Assert.Equal("survivor_a", rows[0].SurvivorId);
            Assert.Equal(4, rows[0].Day);
        }

        [Fact]
        public void InjuryDigest_NullInputs_AreEmpty()
        {
            Assert.Empty(ExpeditionInjuryDigest.Build(null!, _ => null));
            Assert.Empty(ExpeditionInjuryDigest.Build(new List<(string, bool)>(), null!));
        }

        [Fact]
        public void DescribeStateRisk_NamesDriver()
        {
            Assert.Equal(string.Empty, ExpeditionPrepPlanner.DescribeStateRisk(null!));

            var state = new ExpeditionState { dangerLevel = 4, encounterChancePerTick = 0.05f };
            Assert.Contains("danger lvl 4", ExpeditionPrepPlanner.DescribeStateRisk(state));

            var broken = new ExpeditionState { dangerLevel = 1, encounterChancePerTick = 0.05f, vehicleBrokenDown = true };
            Assert.Contains("vehicle disabled", ExpeditionPrepPlanner.DescribeStateRisk(broken));
        }

        [Fact]
        public void HealthHistory_LatestEventOfTypePrefix()
        {
            var system = new HealthHistorySystem();
            system.LogHealthEvent("survivor_a", "expedition_failure", "old", 1);
            system.LogHealthEvent("survivor_a", "checkup", "unrelated", 5);
            system.LogHealthEvent("survivor_a", "expedition_failure_lost", "newer", 6);
            system.LogHealthEvent("survivor_b", "expedition_failure", "other", 9);

            var latest = system.GetLatestEventOfTypePrefix("survivor_a", "expedition_failure");
            Assert.NotNull(latest);
            Assert.Equal(6, latest!.EventDay);
            Assert.Equal("newer", latest.Description);

            Assert.Null(system.GetLatestEventOfTypePrefix("survivor_a", "no_such_prefix"));
            Assert.Null(system.GetLatestEventOfTypePrefix(string.Empty, "expedition_failure"));
            Assert.Null(system.GetLatestEventOfTypePrefix("survivor_a", string.Empty));
        }

        [Fact]
        public void HealthHistory_LaterSameDayEventWins()
        {
            var system = new HealthHistorySystem();
            system.LogHealthEvent("survivor_a", "expedition_failure", "first", 7);
            system.LogHealthEvent("survivor_a", "expedition_failure", "second", 7);
            var latest = system.GetLatestEventOfTypePrefix("survivor_a", "expedition_failure");
            Assert.NotNull(latest);
            Assert.Equal("second", latest!.Description);
        }

        [Fact]
        public void HealthHistory_DoesNotLeakOtherSurvivors()
        {
            var system = new HealthHistorySystem();
            system.LogHealthEvent("survivor_b", "expedition_failure", "b hurt", 3);
            Assert.Null(system.GetLatestEventOfTypePrefix("survivor_a", "expedition_failure"));
            Assert.NotNull(system.GetLatestEventOfTypePrefix("survivor_b", "expedition_failure"));
        }

        [Fact]
        public void InjuryDigest_PreservesRosterOrder()
        {
            var roster = new List<(string Id, bool Alive)>
            {
                ("survivor_b", true),
                ("survivor_a", true)
            };
            HealthEvent? Lookup(string id) => new HealthEvent
            {
                SurvivorId = id,
                EventType = "expedition_failure",
                EventDay = 2,
                Description = id + " hurt"
            };

            var rows = ExpeditionInjuryDigest.Build(roster, Lookup);
            Assert.Equal(2, rows.Count);
            Assert.Equal("survivor_b", rows[0].SurvivorId);
            Assert.Equal("survivor_a", rows[1].SurvivorId);
        }

        [Fact]
        public void OvernightNights_ClampBoundary()
        {
            var def = Def();
            var est = new ExpeditionEstimate { totalTicks = 360f, projectedTripHours = 360f };
            Assert.Equal(ExpeditionPrepPlanner.MaxOvernightNights, Build(def, est, new Dictionary<string, int>()).OvernightNights);

            est.projectedTripHours = 372f;
            est.totalTicks = 372f;
            Assert.Equal(ExpeditionPrepPlanner.MaxOvernightNights, Build(def, est, new Dictionary<string, int>()).OvernightNights);
        }

        [Fact]
        public void DescribeStateRisk_IgnoresZeroOrNegativeEncounter()
        {
            var zero = new ExpeditionState { dangerLevel = 0, encounterChancePerTick = 0f };
            Assert.Equal(ExpeditionPrepPlanner.LowRiskNote, ExpeditionPrepPlanner.DescribeStateRisk(zero));

            var negative = new ExpeditionState { dangerLevel = 0, encounterChancePerTick = -0.5f };
            Assert.Equal(ExpeditionPrepPlanner.LowRiskNote, ExpeditionPrepPlanner.DescribeStateRisk(negative));
            Assert.False(string.IsNullOrEmpty(ExpeditionPrepPlanner.LowRiskNote));
        }

        [Fact]
        public void DescribeStateRisk_VehicleDisabledOnlyWhenBrokenDown()
        {
            var intact = new ExpeditionState { dangerLevel = 1, encounterChancePerTick = 0.05f, vehicleBrokenDown = false };
            Assert.DoesNotContain("vehicle disabled", ExpeditionPrepPlanner.DescribeStateRisk(intact));

            var broken = new ExpeditionState { dangerLevel = 1, encounterChancePerTick = 0.05f, vehicleBrokenDown = true };
            Assert.Equal("vehicle disabled", ExpeditionPrepPlanner.DescribeStateRisk(broken));
        }

        [Fact]
        public void DescribeStateRisk_HighEncounterOverridesZeroDanger()
        {
            var state = new ExpeditionState { dangerLevel = 0, encounterChancePerTick = 0.25f };
            string note = ExpeditionPrepPlanner.DescribeStateRisk(state);
            Assert.NotEqual(ExpeditionPrepPlanner.LowRiskNote, note);
            Assert.Contains("encounter", note);
        }

        [Fact]
        public void InjuryDigest_ToleratesNullDescription()
        {
            var roster = new List<(string Id, bool Alive)> { ("survivor_a", true) };
            HealthEvent? Lookup(string id) => new HealthEvent
            {
                SurvivorId = id,
                EventType = "expedition_failure",
                EventDay = 3,
                Description = null!
            };

            var rows = ExpeditionInjuryDigest.Build(roster, Lookup);
            Assert.Single(rows);
            Assert.Equal(string.Empty, rows[0].Description);
        }

        [Fact]
        public void Build_NullCountById_DefaultsToZero()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var plan = ExpeditionPrepPlanner.Build(def, est, null!, weaponReady: false, hasLight: false);

            Assert.Equal(0, plan.Checklist.Find(r => r.Id == "food")!.Held);
            Assert.False(plan.Checklist.Find(r => r.Id == "food")!.Satisfied);
            Assert.False(plan.Ready);
        }

        [Fact]
        public void GradeRisk_NullInputs_ReturnsLow()
        {
            Assert.Equal(ExpeditionRiskGrade.Low, ExpeditionPrepPlanner.GradeRisk(null!, null!));
        }

        [Fact]
        public void ChecklistLine_MarksAdvisoryRows()
        {
            var def = Def();
            var est = ExpeditionSystem.Estimate(def, ExpeditionStance.Stealth);
            var stock = new Dictionary<string, int>
            {
                ["canned_food"] = 10,
                ["clean_water"] = 10,
                ["anti_rad"] = 2
            };
            var plan = Build(def, est, stock, weaponReady: true, hasLight: false);
            Assert.Contains("LIGHT", plan.ChecklistLine());
            Assert.Contains("(adv)", plan.ChecklistLine());
        }

        [Fact]
        public void ReturnReport_ZeroQuantityLootFloorsToOne()
        {
            var state = new ExpeditionState
            {
                survivorId = "s",
                displayName = "Site",
                loot = new List<ExpeditionLootEntry>
                {
                    new ExpeditionLootEntry { itemId = "canned_food", quantity = 0, weightKg = 0.5f }
                }
            };
            string ceremony = ExpeditionReturnReport.BuildCeremony(state, id => "Canned Food");
            Assert.Contains("Canned Food ×1", ceremony);
            Assert.Contains("1 item(s)", ceremony);
        }

        [Fact]
        public void HealthHistory_TypePrefixLookup_IsSurvivorIdCaseInsensitive()
        {
            var system = new HealthHistorySystem();
            system.LogHealthEvent("Survivor_A", "expedition_failure", "hurt", 2);
            var latest = system.GetLatestEventOfTypePrefix("survivor_a", "expedition_failure");
            Assert.NotNull(latest);
            Assert.Equal(2, latest!.EventDay);
        }

        [Fact]
        public void InjuryDigest_SkipsBlankSurvivorIds()
        {
            var roster = new List<(string Id, bool Alive)> { ("", true), ("survivor_a", true) };
            HealthEvent? Lookup(string id) => new HealthEvent
            {
                SurvivorId = id,
                EventType = "expedition_failure",
                EventDay = 1,
                Description = "hurt"
            };
            var rows = ExpeditionInjuryDigest.Build(roster, Lookup);
            Assert.Single(rows);
            Assert.Equal("survivor_a", rows[0].SurvivorId);
        }
    }
}
