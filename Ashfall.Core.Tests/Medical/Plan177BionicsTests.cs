// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 177 Phase 1 — Bionics Core contract tests (§6.19 matrix).
// Covers: strict catalog validation, eligibility (slot/limb/recovery/item),
// surgery determinism + complication determinism, integration/rehab delay,
// bounded functional restoration, maintenance decay + repair, power states
// (charger-gated charge, expedition battery drain), TYPED electrical
// disruption (passive immunity, no health damage), combat damage → limb
// revert through the limb authority, removal/replacement, save/load,
// old-save baseline, and deterministic replay.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Plan177Medical
{
    public sealed class Plan177BionicsTests
    {
        private static ImplantDefinition ArmMech() => new ImplantDefinition
        {
            implant_id = "implant_piston_grip_frame",
            display_name = "Piston Grip Frame",
            body_slot = "arm",
            implant_class = "mechanical",
            functional_restore_bp = 700,
            skill_modifier_bp = 100,
            power_profile = "passive",
            maintenance_interval_days = 6,
            daily_condition_decay_bp = 40,
            integration_risk_bp = 1200,
            integration_recovery_days = 6,
            malfunction_risk_bp = 300,
            electrical_vulnerability = "none",
            required_item_ids = { "item_surgical_kit" }
        };

        private static ImplantDefinition LegServo() => new ImplantDefinition
        {
            implant_id = "implant_reaction_piston_leg",
            display_name = "Reaction-Piston Leg",
            body_slot = "leg",
            implant_class = "rechargeable",
            functional_restore_bp = 1150,
            skill_modifier_bp = 150,
            power_profile = "rechargeable",
            daily_power_draw_watts = 20,
            battery_days = 4,
            maintenance_interval_days = 5,
            daily_condition_decay_bp = 50,
            integration_risk_bp = 1600,
            integration_recovery_days = 10,
            malfunction_risk_bp = 450,
            electrical_vulnerability = "high",
            required_item_ids = { "bionic_leg_prototype" }
        };

        private static (BionicsSystem system, Dictionary<string, int> counts) Create(params ImplantDefinition[] defs)
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "item_surgical_kit", 10 }, { "surgical_saw", 3 },
                { "bionic_leg_prototype", 3 }, { "repair_kit", 10 }
            };
            var amputation = new AmputationSystem(new SeededRng(177));
            var system = new BionicsSystem(amputation, defs);
            system.BindInventory(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, a) => { counts[id] = counts.TryGetValue(id, out var n) ? n : 0; counts[id] = Math.Max(0, counts[id] - a); });
            return (system, counts);
        }

        private static void GiveAmputatedLimb(AmputationSystem amputation, string survivorId, LimbId limb, int recoveryDays = 0)
        {
            amputation.EnsureSurvivorLimbs(survivorId);
            var l = amputation.GetLimb(survivorId, limb)!;
            l.condition = LimbCondition.Amputated;
            l.recoveryDaysLeft = recoveryDays;
        }

        private static string FindDataDir()
        {
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null)
            {
                string candidate = System.IO.Path.Combine(dir.FullName, "Assets", "StreamingAssets", "Data");
                if (System.IO.File.Exists(System.IO.Path.Combine(candidate, "bionics.json"))) return candidate;
                dir = dir.Parent;
            }
            return string.Empty;
        }

        // ── catalog loader ─────────────────────────────────────────────

        [Fact]
        public void Loader_AuthoredCatalog_LoadsWithoutErrors()
        {
            string dataDir = FindDataDir();
            Assert.True(dataDir.Length > 0, "bionics.json not found");
            var result = BionicsCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Implants.Count >= 5);
        }

        [Fact]
        public void Loader_RejectsPassiveWithBatteryAndDuplicates()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            string dir = System.IO.Path.Combine(System.IO.Path.GetTempPath(), "ashfall_plan177_" + Guid.NewGuid().ToString("N"));
            System.IO.Directory.CreateDirectory(dir);
            try
            {
                System.IO.File.WriteAllText(System.IO.Path.Combine(dir, BionicsCatalogLoader.FileName),
                    "{\"schema_version\":1,\"implants\":[" +
                    "{\"implant_id\":\"implant_a\",\"display_name\":\"A\",\"body_slot\":\"arm\",\"implant_class\":\"mechanical\"," +
                    "\"functional_restore_bp\":700,\"power_profile\":\"passive\",\"battery_days\":5," +
                    "\"maintenance_interval_days\":6,\"integration_recovery_days\":6,\"required_item_ids\":[\"x\"]}]}");
                var r = BionicsCatalogLoader.Load(dir, fileIO, json);
                Assert.True(r.HasErrors);
                Assert.Contains(r.Errors, e => e.Contains("passive profile"));

                System.IO.File.WriteAllText(System.IO.Path.Combine(dir, BionicsCatalogLoader.FileName),
                    "{\"schema_version\":1,\"implants\":[" +
                    "{\"implant_id\":\"implant_b\",\"display_name\":\"B\",\"body_slot\":\"arm\",\"implant_class\":\"mechanical\",\"functional_restore_bp\":700,\"power_profile\":\"passive\",\"maintenance_interval_days\":6,\"integration_recovery_days\":6,\"required_item_ids\":[\"raw_meat\"]}," +
                    "{\"implant_id\":\"implant_b\",\"display_name\":\"C\",\"body_slot\":\"leg\",\"implant_class\":\"mechanical\",\"functional_restore_bp\":700,\"power_profile\":\"passive\",\"maintenance_interval_days\":6,\"integration_recovery_days\":6,\"required_item_ids\":[\"raw_meat\"]}]}");
                var dup = BionicsCatalogLoader.Load(dir, fileIO, json);
                Assert.True(dup.HasErrors);
                Assert.Contains(dup.Errors, e => e.Contains("duplicate"));
            }
            finally
            {
                System.IO.Directory.Delete(dir, recursive: true);
            }
        }

        // ── eligibility ────────────────────────────────────────────────

        private static (BionicsSystem system, AmputationSystem amputation, Dictionary<string, int> counts) CreateFresh(params ImplantDefinition[] defs)
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal)
            {
                { "item_surgical_kit", 10 }, { "surgical_saw", 3 },
                { "bionic_leg_prototype", 3 }, { "bionic_arm_prototype", 3 }, { "repair_kit", 10 }
            };
            var amputation = new AmputationSystem(new SeededRng(177));
            var system = new BionicsSystem(amputation, defs);
            system.BindInventory(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, a) => { counts[id] = counts.TryGetValue(id, out var n) ? n : 0; counts[id] = Math.Max(0, counts[id] - a); });
            return (system, amputation, counts);
        }

        [Fact]
        public void Eligibility_WithFreshFixture()
        {
            var (system, amputation, counts) = CreateFresh(ArmMech(), LegServo());

            Assert.Equal("socket_ineligible", system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 1, null).ReasonCode);

            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            Assert.Equal("body_slot_mismatch", system.TryInstall("s1", LimbId.LeftArm, "implant_reaction_piston_leg", 2, null).ReasonCode);

            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm, recoveryDays: 5);
            Assert.Equal("limb_in_recovery", system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 2, null).ReasonCode);

            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            int before = counts["item_surgical_kit"];
            // Missing-item case on a VALID slot: drain the prototype stock first.
            GiveAmputatedLimb(amputation, "s2", LimbId.RightLeg);
            counts["bionic_leg_prototype"] = 0;
            Assert.Equal("missing_item_bionic_leg_prototype",
                system.TryInstall("s2", LimbId.RightLeg, "implant_reaction_piston_leg", 2, null).ReasonCode);
            Assert.Equal(before, counts["item_surgical_kit"]);   // nothing consumed on failure
        }

        // ── surgery + complications ────────────────────────────────────

        [Fact]
        public void Surgery_DeterministicOutcome_ComplicationRollScales()
        {
            var (systemA, amputationA, _) = CreateFresh(ArmMech());
            var (systemB, amputationB, _) = CreateFresh(ArmMech());
            GiveAmputatedLimb(amputationA, "s1", LimbId.LeftArm);
            GiveAmputatedLimb(amputationB, "s1", LimbId.LeftArm);

            var ra = systemA.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 5, new SeededRng(99));
            var rb = systemB.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 5, new SeededRng(5));
            Assert.True(ra.Success);
            Assert.True(rb.Success);
            Assert.Equal(ra.Complication, rb.Complication);   // same seed → same complication
            Assert.NotNull(ra.Instance);
            Assert.Equal(ra.Instance!.integration_days_total, rb.Instance!.integration_days_total);
            Assert.Equal((int)LimbCondition.Bionic,
                (int)amputationA.GetLimb("s1", LimbId.LeftArm)!.condition);
        }

        [Fact]
        public void Surgery_NeuroLinkedHighRisk_ProducesTypedComplication()
        {
            var neuro = ArmMech();
            neuro.implant_class = "neuro_linked";
            neuro.integration_risk_bp = 10000; // always
            var (system, amputation, _) = CreateFresh(neuro);
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            var r = system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 5, new SeededRng(2));
            Assert.True(r.Success);
            Assert.NotEqual(ImplantComplication.None, r.Complication);
        }

        // ── rehab + bounded restoration ────────────────────────────────

        [Fact]
        public void Rehab_DelayGatesCapability_BonusBounded()
        {
            var (system, amputation, counts) = CreateFresh(LegServo());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftLeg);
            var r = system.TryInstall("s1", LimbId.LeftLeg, "implant_reaction_piston_leg", 1, null);
            Assert.True(r.Success);

            // Day 1 of rehab: partial capability, never instant full (§6.8).
            float day1 = system.GetLimbCapabilityBonusBp("s1", LimbId.LeftLeg);
            Assert.True(day1 > 0f && day1 < BionicsCaps.CapabilityBonusCapBp,
                "rehab day 1 must be partial, positive, under cap");

            // Battery profile with NO charger and empty battery → zero bonus.
            var instance = system.InstanceFor("s1", LimbId.LeftLeg)!;
            system.TickDay(2); // no charger → battery drains
            instance.battery_days_remaining = 0f;
            Assert.Equal(0f, system.GetLimbCapabilityBonusBp("s1", LimbId.LeftLeg), 3);

            // Fully charged + integrated → bonus ≤ hard cap.
            instance.battery_days_remaining = 4f;
            instance.is_charging = true;
            for (int day = 3; day <= 15; day++) system.TickDay(day);
            float integrated = system.GetLimbCapabilityBonusBp("s1", LimbId.LeftLeg);
            Assert.InRange(integrated, 0f, BionicsCaps.CapabilityBonusCapBp);
        }

        // ── maintenance, condition, destruction ────────────────────────

        [Fact]
        public void ConditionDecays_MaintenanceRestores_DestructionRevertsLimb()
        {
            var (system, amputation, counts) = CreateFresh(ArmMech());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            var r = system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 1, null);
            Assert.True(r.Success);
            var instance = system.InstanceFor("s1", LimbId.LeftArm)!;

            int destroyedEvents = 0;
            system.OnImplantDestroyed += _ => destroyedEvents++;

            // 0.40/day decay: ~125 days to fail hard; tick 110 days with maintenance every 5.
            for (int day = 2; day <= 40; day++)
            {
                system.TickDay(day);
                if (day % 5 == 0) system.PerformMaintenance("s1", LimbId.LeftArm, "repair_kit", day);
            }
            Assert.False(instance.destroyed);
            Assert.True(destroyedEvents == 0);

            // Stop maintaining → decay wins → implant destroyed, limb reverts.
            instance.last_maintenance_day = 0;
            for (int day = 41; day <= 320; day++) system.TickDay(day);
            Assert.True(instance.destroyed);
            Assert.Equal(1, destroyedEvents);
            Assert.Equal((int)LimbCondition.Amputated,
                (int)amputation.GetLimb("s1", LimbId.LeftArm)!.condition);
        }

        // ── power (§6.10) ──────────────────────────────────────────────

        [Fact]
        public void Power_ChargesOnlyWithRealCharger_DrainsOnExpedition()
        {
            var (system, amputation, counts) = CreateFresh(LegServo());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftLeg);
            var r = system.TryInstall("s1", LimbId.LeftLeg, "implant_reaction_piston_leg", 1, null);
            Assert.True(r.Success);
            var instance = system.InstanceFor("s1", LimbId.LeftLeg)!;

            bool charger = false;
            system.ChargerAvailable = () => charger;

            // 3 days without charger: battery 4 → 1.
            for (int day = 2; day <= 4; day++) system.TickDay(day);
            Assert.Equal(1f, instance.battery_days_remaining, 3);

            // Charging day: refills to authored capacity.
            charger = true;
            system.TickDay(5);
            Assert.Equal(4f, instance.battery_days_remaining, 3);
            Assert.True(instance.is_charging);

            // Passive implants ignore power entirely.
            var (sysM, ampM, _) = CreateFresh(ArmMech());
            GiveAmputatedLimb(ampM, "s2", LimbId.RightArm);
            sysM.TryInstall("s2", LimbId.RightArm, "implant_piston_grip_frame", 1, null);
            sysM.ChargerAvailable = () => false;
            sysM.TickDay(2);
            Assert.False(sysM.InstanceFor("s2", LimbId.RightArm)!.is_charging);
        }

        // ── electrical disruption (typed, §6.11) ──────────────────────

        [Fact]
        public void ElectricalDisruption_TypedByClass_PassiveImmune_NoHealthDamage()
        {
            var (system, amputation, counts) = CreateFresh(ArmMech(), LegServo());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            GiveAmputatedLimb(amputation, "s1", LimbId.RightLeg);
            system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 1, null);
            system.TryInstall("s1", LimbId.RightLeg, "implant_reaction_piston_leg", 1, null);

            var outcomes = system.ApplyElectricalDisruption("s1", severity: 0.9f);

            // Passive mechanical arm: immune (vulnerability none).
            Assert.DoesNotContain(outcomes, o => o.InstanceId == system.InstanceFor("s1", LimbId.LeftArm)!.instance_id);

            // Powered leg: typed effects only.
            var legOutcome = Assert.Single(outcomes);
            Assert.Contains("actuator_lock", legOutcome.Effect);
            Assert.True(legOutcome.BatteryDaysDrained > 0f);
            var legInstance = system.InstanceFor("s1", LimbId.RightLeg)!;
            Assert.Equal((int)ImplantMalfunction.ActuatorLock, legInstance.malfunction);
            Assert.True(legInstance.condition < 100f);

            // Structural guard: the system has no needs/health surface at all.
            Assert.Null(typeof(BionicsSystem).GetProperty("Needs"));
            Assert.Null(typeof(BionicsSystem).GetProperty("Health"));
        }

        // ── removal / replacement ─────────────────────────────────────

        [Fact]
        public void Removal_RevertsLimb_AllowsReinstallation()
        {
            var (system, amputation, counts) = CreateFresh(ArmMech());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
            system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 1, null);
            Assert.True(system.RemoveImplant("s1", LimbId.LeftArm));
            Assert.Equal((int)LimbCondition.Amputated,
                (int)amputation.GetLimb("s1", LimbId.LeftArm)!.condition);
            Assert.Null(system.InstanceFor("s1", LimbId.LeftArm));

            // Reinstall works (replacement path).
            counts["item_surgical_kit"] += 2;
            var again = system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 10, null);
            Assert.True(again.Success);
        }

        // ── combat damage handoff ──────────────────────────────────────

        [Fact]
        public void CombatDamage_CanDestroyImplant_AndRevertLimb()
        {
            var (system, amputation, counts) = CreateFresh(LegServo());
            GiveAmputatedLimb(amputation, "s1", LimbId.RightLeg);
            system.TryInstall("s1", LimbId.RightLeg, "implant_reaction_piston_leg", 1, null);

            // Two severe hits (60 condition each) destroy the implant.
            var first = system.ApplyCombatDamage("s1", LimbId.RightLeg, severity: 1f);
            Assert.Equal("condition_damage", first.Effect);
            var second = system.ApplyCombatDamage("s1", LimbId.RightLeg, severity: 1f);
            Assert.Equal("destroyed", second.Effect);
            Assert.Null(system.InstanceFor("s1", LimbId.RightLeg));
            Assert.Equal((int)LimbCondition.Amputated,
                (int)amputation.GetLimb("s1", LimbId.RightLeg)!.condition);
        }

        // ── persistence ────────────────────────────────────────────────

        [Fact]
        public void SaveLoad_PreservesExactImplantState()
        {
            var (system, amputation, counts) = CreateFresh(LegServo());
            GiveAmputatedLimb(amputation, "s1", LimbId.LeftLeg);
            system.TryInstall("s1", LimbId.LeftLeg, "implant_reaction_piston_leg", 3, null);
            system.TickDay(4);

            var saved = system.CaptureState();
            var (restored, _, _) = CreateFresh(LegServo());
            restored.RestoreState(saved);

            Assert.Equal(saved.instance_counter, restored.State.instance_counter);
            var a = saved.implants[0];
            var b = restored.State.implants[0];
            Assert.Equal(a.instance_id, b.instance_id);
            Assert.Equal(a.condition, b.condition, 5);
            Assert.Equal(a.integration_days_left, b.integration_days_left);
            Assert.Equal(a.battery_days_remaining, b.battery_days_remaining, 5);
            Assert.Equal(a.malfunction, b.malfunction);
        }

        [Fact]
        public void OldSaveBaseline_RestoreFromNullIsSafe()
        {
            var (system, _, _) = CreateFresh(ArmMech());
            system.RestoreState(null);
            Assert.Empty(system.State.implants);
            Assert.Equal(0f, system.GetLimbCapabilityBonusBp("s1", LimbId.LeftArm), 3);
        }

        [Fact]
        public void DeterministicReplay_SameSeedSameSurgeryPath()
        {
            var run = (int seed) =>
            {
                var (system, amputation, counts) = CreateFresh(ArmMech());
                GiveAmputatedLimb(amputation, "s1", LimbId.LeftArm);
                var r = system.TryInstall("s1", LimbId.LeftArm, "implant_piston_grip_frame", 1, new SeededRng(seed));
                for (int day = 2; day <= 30; day++)
                {
                    system.TickDay(day);
                    if (day % 5 == 0) system.PerformMaintenance("s1", LimbId.LeftArm, "repair_kit", day);
                }
                var i = r.Instance!;
                return (r.Complication, i.condition, i.integration_status, i.malfunction);
            };
            Assert.Equal(run(9), run(9));
        }
    }
}
