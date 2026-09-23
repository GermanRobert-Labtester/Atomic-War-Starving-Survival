// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 193: Chronic Conditions & Disabilities System — Integration Tests
// Verifies chronic conditions and accommodations catalog loading, condition
// addition, capability modifier calculations, accommodation mitigation,
// multi-condition penalty stacking, and save/restore state persistence.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Medical;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan193ChronicConditionIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsConditionsAndAccommodations()
        {
            var system = new ChronicConditionSystem();
            string path = Path.Combine(DataDirectory, "chronic_conditions.json");
            Assert.True(File.Exists(path), $"chronic_conditions.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var conditions = system.GetAllConditionDefs();
            Assert.Equal(6, conditions.Count);

            var accommodations = system.GetAllAccommodationDefs();
            Assert.Equal(6, accommodations.Count);

            var limp = system.GetConditionDef("cond_chronic_limp");
            Assert.NotNull(limp);
            Assert.Equal("mobility", limp.condition_type);
            Assert.Equal(0.20f, limp.capability_penalties["movement_speed"]);
            Assert.Equal("accom_cane_crutch", limp.recommended_accommodation_id);

            var cane = system.GetAccommodationDef("accom_cane_crutch");
            Assert.NotNull(cane);
            Assert.Equal(0.15f, cane.effective_bonus);
        }

        [Fact]
        public void AddCondition_RecordsConditionAndReducesCapability()
        {
            var system = new ChronicConditionSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));

            SurvivorConditionRecord? added = null;
            system.OnConditionAdded += rec => added = rec;

            var record = system.AddCondition("surv_01", "cond_chronic_limp", day: 10, cause: "blast_injury");

            Assert.NotNull(record);
            Assert.NotNull(added);
            Assert.Equal("surv_01", record.SurvivorId);
            Assert.Equal("cond_chronic_limp", record.ConditionId);
            Assert.Equal(1, system.TrackedConditionCount);

            // Baseline was 1.0, limp imposes 0.20 penalty -> 0.80
            float moveMod = system.CalculateCapabilityModifier("surv_01", "movement_speed");
            Assert.Equal(0.80f, moveMod);

            // Unaffected capability remains 1.0
            float workMod = system.CalculateCapabilityModifier("surv_01", "work_speed");
            Assert.Equal(1.0f, workMod);
        }

        [Fact]
        public void AssignAccommodation_MitigatesPenalty()
        {
            var system = new ChronicConditionSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));

            system.AddCondition("surv_01", "cond_chronic_limp", day: 10);
            Assert.Equal(0.80f, system.CalculateCapabilityModifier("surv_01", "movement_speed"));

            SurvivorAccommodationRecord? assigned = null;
            system.OnAccommodationAssigned += a => assigned = a;

            // Cane provides +0.15 bonus, reducing penalty from 0.20 to 0.05 -> 0.95
            var accom = system.AssignAccommodation("surv_01", "accom_cane_crutch", "cond_chronic_limp", day: 11);

            Assert.NotNull(accom);
            Assert.NotNull(assigned);
            Assert.Equal(1, system.ActiveAccommodationCount);

            float mitigatedMod = system.CalculateCapabilityModifier("surv_01", "movement_speed");
            Assert.Equal(0.95f, mitigatedMod);

            // Remove accommodation and penalty returns
            system.RemoveAccommodation("surv_01", "accom_cane_crutch");
            Assert.Equal(0.80f, system.CalculateCapabilityModifier("surv_01", "movement_speed"));
        }

        [Fact]
        public void MultipleConditions_StackPenalties()
        {
            var system = new ChronicConditionSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));

            // Limp: movement_speed -0.20
            system.AddCondition("surv_02", "cond_chronic_limp", day: 5);
            // Ash fibrosis: movement_speed -0.15, work_speed -0.25
            system.AddCondition("surv_02", "cond_respiratory_damage", day: 8);

            // Total movement penalty = 0.20 + 0.15 = 0.35 -> 0.65
            float moveMod = system.CalculateCapabilityModifier("surv_02", "movement_speed");
            Assert.Equal(0.65f, moveMod);

            // Work penalty = 0.25 -> 0.75
            float workMod = system.CalculateCapabilityModifier("surv_02", "work_speed");
            Assert.Equal(0.75f, workMod);
        }

        [Fact]
        public void GetTotalImpairmentScore_CalculatesSurvivorImpact()
        {
            var system = new ChronicConditionSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));

            // Clean survivor has 0% impairment
            Assert.Equal(0.0f, system.GetTotalImpairmentScore("surv_healthy"));

            // Survivor with partial blindness (combat -0.30, learning -0.15, crafting -0.20)
            system.AddCondition("surv_blind", "cond_partial_blindness", day: 12);
            float score = system.GetTotalImpairmentScore("surv_blind");
            Assert.True(score > 5.0f);
        }

        [Fact]
        public void SaveRestoreState_PreservesConditionsAndAccommodations()
        {
            var system = new ChronicConditionSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));

            system.AddCondition("surv_veteran", "cond_tremors_neurological", day: 20, cause: "toxic_waste");
            system.AssignAccommodation("surv_veteran", "accom_stabilizing_brace", "cond_tremors_neurological", day: 21);

            var state = system.CaptureState();

            var restoredSystem = new ChronicConditionSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "chronic_conditions.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal(1, restoredSystem.TrackedConditionCount);
            Assert.Equal(1, restoredSystem.ActiveAccommodationCount);

            var cond = restoredSystem.GetSurvivorConditions("surv_veteran").FirstOrDefault();
            Assert.NotNull(cond);
            Assert.Equal("cond_tremors_neurological", cond.ConditionId);
            Assert.Equal("toxic_waste", cond.Cause);

            var accom = restoredSystem.GetSurvivorAccommodations("surv_veteran").FirstOrDefault();
            Assert.NotNull(accom);
            Assert.Equal("accom_stabilizing_brace", accom.AccommodationId);
        }
    }
}
