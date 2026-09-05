// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class AdvancedSurgicalWardTests
    {
        private static string GetDataDir()
        {
            string candidate = Path.Combine(AppContext.BaseDirectory, "../../../..", "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            candidate = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data");
            if (Directory.Exists(candidate)) return Path.GetFullPath(candidate);
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string check = Path.Combine(dir.FullName, "Assets/StreamingAssets/Data");
                if (Directory.Exists(check)) return check;
                dir = dir.Parent;
            }
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void CatalogLoader_LoadsAllTenProcedures()
        {
            string dataDir = GetDataDir();
            var procs = SurgicalProcedureCatalogLoader.Load(dataDir);
            Assert.NotNull(procs);
            Assert.Equal(10, procs.Count);

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var p in procs)
            {
                Assert.False(string.IsNullOrEmpty(p.procedure_id));
                Assert.True(seenIds.Add(p.procedure_id), $"Duplicate procedure_id: {p.procedure_id}");
                Assert.True(p.base_duration_hours > 0);
                Assert.True(p.recovery_days >= 2);
                Assert.NotEmpty(p.required_tools);
                Assert.NotEmpty(p.consumable_costs);
            }
        }

        [Fact]
        public void PreOpValidation_EnforcesTriageSkill_ToolsAndConsumables()
        {
            var def = new SurgicalProcedureDefinition
            {
                procedure_id = "test_surg",
                display_name = "Test Surgery",
                required_tools = new List<string> { "surgical_scalpel" },
                consumable_costs = new Dictionary<string, int> { { "anesthetic_ether", 1 }, { "sterile_gauze", 2 } },
                base_duration_hours = 3
            };

            var inv = new Inventory.Inventory();
            var rng = new SeededRng(42);
            var ward = new AdvancedSurgicalWardSystem(new[] { def }, inv, rng);

            // Blocked: lack triage skill
            var resSkill = ward.ValidatePreOp("survivor_1", "survivor_2", "test_surg", surgeonHasTriageSkill: false);
            Assert.False(resSkill.IsSuccess);
            Assert.Equal("surgeon_lacks_skill", resSkill.FailureCode);

            // Blocked: missing scalpel
            var resTool = ward.ValidatePreOp("survivor_1", "survivor_2", "test_surg", surgeonHasTriageSkill: true);
            Assert.False(resTool.IsSuccess);

            // Add scalpel but missing ether/gauze
            inv.TryProduce("surgical_scalpel", 1);
            var resConsumable = ward.ValidatePreOp("survivor_1", "survivor_2", "test_surg", surgeonHasTriageSkill: true);
            Assert.False(resConsumable.IsSuccess);

            // Add consumables
            inv.TryProduce("anesthetic_ether", 1);
            inv.TryProduce("sterile_gauze", 2);

            var resOk = ward.ValidatePreOp("survivor_1", "survivor_2", "test_surg", surgeonHasTriageSkill: true);
            Assert.True(resOk.IsSuccess);
        }

        [Fact]
        public void SurgicalExecution_ConsumesAnesthesia_AdvancesHours_AndCompletes()
        {
            var def = new SurgicalProcedureDefinition
            {
                procedure_id = "surg_shrapnel_extraction",
                display_name = "Shrapnel Extraction",
                required_tools = new List<string> { "surgical_scalpel" },
                consumable_costs = new Dictionary<string, int> { { "anesthetic_ether", 1 }, { "sterile_gauze", 2 } },
                base_duration_hours = 4,
                base_shock_risk = 0.1f,
                base_complication_risk = 0.0f,
                recovery_days = 3
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("surgical_scalpel", 1);
            inv.TryProduce("anesthetic_ether", 1);
            inv.TryProduce("sterile_gauze", 2);

            var rng = new SeededRng(12345);
            var ward = new AdvancedSurgicalWardSystem(new[] { def }, inv, rng);

            var startRes = ward.StartOperation("patient_alice", "surgeon_bob", "surg_shrapnel_extraction");
            Assert.True(startRes.IsSuccess);
            Assert.Single(ward.ActiveOperations);

            // Verify consumables were consumed
            Assert.Equal(0, inv.CountById("anesthetic_ether"));
            Assert.Equal(0, inv.CountById("sterile_gauze"));

            var op = ward.ActiveOperations[0];
            Assert.Equal(100f, op.anesthesia_level);

            // Advance 4 hours
            for (int h = 0; h < 4; h++)
            {
                ward.TickOperationHour(op);
            }

            Assert.True(op.is_completed);
            Assert.True(op.patient_survived);
            Assert.Empty(ward.ActiveOperations);
            Assert.Single(ward.RecoveryPatients);
            Assert.Equal(3, ward.RecoveryPatients[0].recovery_days_remaining);
        }

        [Fact]
        public void CellularRadScrub_ConsumesReagents_AndPurgesRadDose()
        {
            var def = new SurgicalProcedureDefinition
            {
                procedure_id = "surg_cellular_rad_scrub",
                display_name = "Cellular Rad Scrub",
                required_tools = new List<string> { "surgical_scalpel" },
                consumable_costs = new Dictionary<string, int>
                {
                    { "clean_water", 2 },
                    { "chemical_filter", 1 },
                    { "anesthetic_ether", 1 }
                },
                base_duration_hours = 2,
                base_complication_risk = 0.0f,
                recovery_days = 3
            };

            var inv = new Inventory.Inventory();
            inv.TryProduce("surgical_scalpel", 1);
            inv.TryProduce("clean_water", 2);
            inv.TryProduce("chemical_filter", 1);
            inv.TryProduce("anesthetic_ether", 1);

            var ward = new AdvancedSurgicalWardSystem(new[] { def }, inv, new SeededRng(555));

            var res = ward.StartOperation("patient_rad", "surgeon_bob", "surg_cellular_rad_scrub");
            Assert.True(res.IsSuccess);

            var op = ward.ActiveOperations[0];
            ward.TickOperationHour(op);
            ward.TickOperationHour(op);

            Assert.True(op.is_completed);
            Assert.Equal(150f, op.rad_mSv_purged); // 150 mSv = 15 rads
        }

        [Fact]
        public void AutoclaveCycle_ConsumesWater_AndRestoresSterileField()
        {
            var inv = new Inventory.Inventory();
            var ward = new AdvancedSurgicalWardSystem(Array.Empty<SurgicalProcedureDefinition>(), inv, new SeededRng(7));

            // Blocked without power
            var resNoPower = ward.RunAutoclaveCycle(hasPower: false);
            Assert.False(resNoPower.IsSuccess);
            Assert.Equal("no_power", resNoPower.FailureCode);

            // Blocked without clean water
            var resNoWater = ward.RunAutoclaveCycle(hasPower: true);
            Assert.False(resNoWater.IsSuccess);
            Assert.Equal("no_water", resNoWater.FailureCode);

            // Provide water
            inv.TryProduce("clean_water", 1);
            var resOk = ward.RunAutoclaveCycle(hasPower: true);
            Assert.True(resOk.IsSuccess);
            Assert.Equal(100f, ward.SterileFieldPercent);
            Assert.Equal(0, inv.CountById("clean_water"));
        }

        [Fact]
        public void ThreeDayRecovery_DischargesPatientAfterThreeDays()
        {
            var inv = new Inventory.Inventory();
            var ward = new AdvancedSurgicalWardSystem(Array.Empty<SurgicalProcedureDefinition>(), inv, new SeededRng(8));

            var save = new AdvancedSurgicalWardSave();
            save.recovery_patients.Add(new SurgicalOperationState
            {
                operation_id = "op_test",
                patient_id = "patient_carol",
                is_completed = true,
                patient_survived = true,
                recovery_days_remaining = 3
            });
            ward.RestoreState(save);

            Assert.Single(ward.RecoveryPatients);

            ward.TickDay(1);
            Assert.Equal(2, ward.RecoveryPatients[0].recovery_days_remaining);

            ward.TickDay(2);
            Assert.Equal(1, ward.RecoveryPatients[0].recovery_days_remaining);

            bool discharged = false;
            ward.OnPatientDischarged += _ => discharged = true;

            ward.TickDay(3);
            Assert.True(discharged);
            Assert.Empty(ward.RecoveryPatients);
        }

        [Fact]
        public void Persistence_ActiveAndRecoveryPatientsSurviveSaveLoad()
        {
            var inv = new Inventory.Inventory();
            var wardA = new AdvancedSurgicalWardSystem(Array.Empty<SurgicalProcedureDefinition>(), inv, new SeededRng(9));

            var saveA = new AdvancedSurgicalWardSave
            {
                sterile_field_percent = 75f,
                last_tick_day = 12
            };
            saveA.active_operations.Add(new SurgicalOperationState
            {
                operation_id = "op_active_1",
                patient_id = "p1",
                procedure_id = "surg_shrapnel_extraction",
                progress_hours = 2,
                total_duration_hours = 4,
                shock_percent = 25f,
                anesthesia_level = 50f
            });

            wardA.RestoreState(saveA);

            var captured = wardA.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(75f, captured.sterile_field_percent);
            Assert.Single(captured.active_operations);

            var wardB = new AdvancedSurgicalWardSystem(Array.Empty<SurgicalProcedureDefinition>(), new Inventory.Inventory(), new SeededRng(10));
            wardB.RestoreState(captured);

            Assert.Equal(75f, wardB.SterileFieldPercent);
            Assert.Single(wardB.ActiveOperations);
            Assert.Equal("op_active_1", wardB.ActiveOperations[0].operation_id);
            Assert.Equal(2, wardB.ActiveOperations[0].progress_hours);
            Assert.Equal(25f, wardB.ActiveOperations[0].shock_percent);
        }
    }
}
