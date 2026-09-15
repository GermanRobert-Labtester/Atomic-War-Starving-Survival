// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 177 Phase 2 — host/save wiring contract tests.
// Proves: the save-section registry row, the codec envelope round trip,
// the power-grid-gated charger seam semantics, and the old-save baseline.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests.Plan177Medical
{
    public sealed class Plan177BionicsHostWiringTests
    {
        private static ImplantDefinition LegServo() => new ImplantDefinition
        {
            implant_id = "implant_reaction_piston_leg",
            display_name = "Reaction-Piston Leg",
            body_slot = "leg",
            implant_class = "rechargeable",
            functional_restore_bp = 1150,
            power_profile = "rechargeable",
            daily_power_draw_watts = 20,
            battery_days = 4,
            maintenance_interval_days = 5,
            daily_condition_decay_bp = 50,
            integration_risk_bp = 1600,
            integration_recovery_days = 10,
            electrical_vulnerability = "high",
            required_item_ids = { "bionic_leg_prototype" }
        };

        [Fact]
        public void SaveSectionRegistry_HasBionicsRow_WithMatchingMetadata()
        {
            var row = Assert.Single(SaveSectionRegistry.All, r => r.SectionKey == "bionics");
            Assert.Equal("SaveBionics", row.SaveMethod);
            Assert.Equal("SetupBionics", row.SetupMethod);
            Assert.Equal("medical", row.Owner);
        }

        [Fact]
        public void SaveCodec_RoundTripsThroughEnvelope()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "bionic_leg_prototype", 5 }, { "surgical_saw", 3 } };
            var amputation = new AmputationSystem(new SeededRng(177));
            var system = new BionicsSystem(amputation, new[] { LegServo() });
            system.BindInventory(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, a) => counts[id] -= a);
            amputation.EnsureSurvivorLimbs("s1");
            var limb = amputation.GetLimb("s1", LimbId.LeftLeg)!;
            limb.condition = LimbCondition.Amputated;

            var r = system.TryInstall("s1", LimbId.LeftLeg, "implant_reaction_piston_leg", 3, null);
            Assert.True(r.Success);
            system.TickDay(4);

            var json = new SystemTextJsonSerializer();
            string persisted = SchemaVersionedEnvelope<BionicsSystemState>.Encode(system.CaptureState(), json);
            var decoded = SchemaVersionedEnvelope<BionicsSystemState>.Decode(persisted, json);

            Assert.NotNull(decoded);
            Assert.Single(decoded!.implants);
            Assert.Equal(system.State.implants[0].instance_id, decoded.implants[0].instance_id);
            Assert.Equal(system.State.implants[0].condition, decoded.implants[0].condition, 5);
            Assert.Equal(system.State.implants[0].battery_days_remaining, decoded.implants[0].battery_days_remaining, 5);
            Assert.Equal(system.State.implants[0].integration_days_left, decoded.implants[0].integration_days_left);
        }

        [Fact]
        public void ChargerGate_NoPowerGrid_ChargeOnlyWhenChargerReportsTrue()
        {
            var counts = new Dictionary<string, int>(StringComparer.Ordinal) { { "bionic_leg_prototype", 5 }, { "surgical_saw", 3 } };
            var amputation = new AmputationSystem(new SeededRng(177));
            var system = new BionicsSystem(amputation, new[] { LegServo() });
            system.BindInventory(
                id => counts.TryGetValue(id, out var n) ? n : 0,
                (id, a) => counts[id] -= a);
            amputation.EnsureSurvivorLimbs("s1");
            var limb = amputation.GetLimb("s1", LimbId.LeftLeg)!;
            limb.condition = LimbCondition.Amputated;
            system.TryInstall("s1", LimbId.LeftLeg, "implant_reaction_piston_leg", 1, null);
            var instance = system.InstanceFor("s1", LimbId.LeftLeg)!;

            // No grid → no charge, battery drains daily.
            system.ChargerAvailable = () => false;
            for (int day = 2; day <= 4; day++) system.TickDay(day);
            Assert.Equal(1f, instance.battery_days_remaining, 3);

            // Grid back → refill to authored capacity (host wires IsRoomPowered).
            system.ChargerAvailable = () => true;
            system.TickDay(5);
            Assert.Equal(4f, instance.battery_days_remaining, 3);
        }

        [Fact]
        public void OldSaveBaseline_NoBionicsSection_YieldsEmptySystem()
        {
            var amputation = new AmputationSystem(new SeededRng(177));
            var system = new BionicsSystem(amputation, new[] { LegServo() });
            system.RestoreState(null);
            Assert.Empty(system.State.implants);
            // Existing prosthetics/limb state are untouched by the empty section.
            Assert.Equal((int)LimbCondition.Intact,
                (int)amputation.GetLimb("s1", LimbId.LeftArm)!.condition);
        }
    }
}
