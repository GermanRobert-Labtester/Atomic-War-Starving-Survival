// SPDX-License-Identifier: MIT
// ============================================================================
// Ashfall Core Tests : Plan 158 — Disaster & Emergency Response System
// Subsystem          : DisasterResponseSystem / Crisis Management & Mitigation Tests
// Authority          : Next-steps-plans/Plan_158_Disaster_Emergency_Response_System.md
//                      UNBLOCK-PROGRAM-WAVE32-BATCH6-PLANS (DEC-154)
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Plan158Disaster
{
    public sealed class Plan158DisasterResponseIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void Catalog_Loads_ProtocolsSuccessfully()
        {
            var system = new DisasterResponseSystem();
            string path = ResolveDataPath("disaster_templates.json");
            Assert.True(File.Exists(path), $"disaster_templates.json missing at {path}");

            string json = File.ReadAllText(path);
            system.LoadCatalog(json);

            Assert.False(system.IsProtocolActive(EmergencyProtocolType.FireSuppression));
            system.ActivateProtocol(EmergencyProtocolType.FireSuppression);
            Assert.True(system.IsProtocolActive(EmergencyProtocolType.FireSuppression));
        }

        [Fact]
        public void DisasterTrigger_InitializesActiveCrisisWithAffectedRooms()
        {
            var system = new DisasterResponseSystem();
            var rooms = new List<string> { "room_power_generator", "room_dormitory_a" };

            var disaster = system.TriggerDisaster(DisasterType.ElectricalFire, DisasterSeverity.Moderate, rooms, currentDay: 12);
            Assert.NotNull(disaster);
            Assert.Equal(DisasterType.ElectricalFire, disaster.Type);
            Assert.Equal(DisasterSeverity.Moderate, disaster.Severity);
            Assert.Equal(DisasterStatus.Active, disaster.Status);
            Assert.Equal(2, disaster.AffectedRoomIds.Count);
            Assert.Equal(1, system.TotalDisastersTriggered);
        }

        [Fact]
        public void EmergencyProtocol_ActivationAndDamageMitigation()
        {
            var system = new DisasterResponseSystem();
            var rooms = new List<string> { "room_workshop" };
            var disaster = system.TriggerDisaster(DisasterType.ElectricalFire, DisasterSeverity.Severe, rooms, currentDay: 15);

            // Unmitigated damage
            double unmitigated = system.CalculateRoomDamage(disaster, "room_workshop");

            // Activate FireSuppression & Lockdown
            system.ActivateProtocol(EmergencyProtocolType.FireSuppression);
            system.ActivateProtocol(EmergencyProtocolType.Lockdown);

            double mitigated = system.CalculateRoomDamage(disaster, "room_workshop");

            // Mitigated damage is drastically lower (fire suppression reduces to 30%, lockdown adds another 15% reduction)
            Assert.True(mitigated < unmitigated * 0.40);
        }

        [Fact]
        public void MitigationLabor_ResolvesDisasterWithSynergyBonus()
        {
            var system = new DisasterResponseSystem();
            var disaster = system.TriggerDisaster(DisasterType.Flooding, DisasterSeverity.Minor, new List<string> { "room_sump" }, currentDay: 20);

            var rng = new SeededRng(555);

            // Activate Sump Pumps Overdrive (+70% labor synergy)
            system.ActivateProtocol(EmergencyProtocolType.PumpsOverdrive);

            // Tick progress with base 3.0 labor
            bool resolved = system.TickDisaster(disaster.DisasterId, baseMitigationLabor: 3.0, currentDay: 21, rng);
            // Minor flood requires 6.0 mitigation. 3.0 * 1.70 * 0.75 resilience = 3.825 (contained, not yet resolved)
            Assert.False(resolved);
            var status = system.GetDisaster(disaster.DisasterId)!;
            Assert.Equal(DisasterStatus.Contained, status.Status);

            // Tick remainder of labor
            resolved = system.TickDisaster(disaster.DisasterId, baseMitigationLabor: 3.0, currentDay: 22, rng);
            Assert.True(resolved);

            var resolvedDisaster = system.GetDisaster(disaster.DisasterId)!;
            Assert.Equal(DisasterStatus.Resolved, resolvedDisaster.Status);
            Assert.Equal(22, resolvedDisaster.ResolvedDay);
            Assert.Equal(1, system.TotalDisastersResolved);
        }

        [Fact]
        public void ResilienceRating_ScalesMitigationSpeed()
        {
            var systemLowRes = new DisasterResponseSystem();
            systemLowRes.AdjustResilience(-40.0); // 35 resilience (factor 0.5)

            var systemHighRes = new DisasterResponseSystem();
            systemHighRes.AdjustResilience(25.0); // 100 resilience (factor 1.0)

            var disLow = systemLowRes.TriggerDisaster(DisasterType.Earthquake, DisasterSeverity.Moderate, new List<string>(), currentDay: 1);
            var disHigh = systemHighRes.TriggerDisaster(DisasterType.Earthquake, DisasterSeverity.Moderate, new List<string>(), currentDay: 1);

            var rng = new SeededRng(77);
            systemLowRes.TickDisaster(disLow.DisasterId, baseMitigationLabor: 5.0, currentDay: 2, rng);
            systemHighRes.TickDisaster(disHigh.DisasterId, baseMitigationLabor: 5.0, currentDay: 2, rng);

            var lowStatus = systemLowRes.GetDisaster(disLow.DisasterId)!;
            var highStatus = systemHighRes.GetDisaster(disHigh.DisasterId)!;

            Assert.True(highStatus.MitigationInvested > lowStatus.MitigationInvested);
        }

        [Fact]
        public void SaveRestore_PreservesDisasterAndProtocolState()
        {
            var system = new DisasterResponseSystem();
            system.ActivateProtocol(EmergencyProtocolType.StructuralShoring);
            system.AdjustResilience(15.0);

            var dis = system.TriggerDisaster(DisasterType.StructuralSubsidence, DisasterSeverity.Severe, new List<string> { "room_corridor" }, currentDay: 30);
            var rng = new SeededRng(101);
            system.TickDisaster(dis.DisasterId, 8.0, currentDay: 31, rng);

            var state = system.CaptureState();
            Assert.Equal(1, state.SchemaVersion);

            var restored = new DisasterResponseSystem();
            restored.RestoreState(state);

            Assert.Equal(system.ResilienceRating, restored.ResilienceRating);
            Assert.True(restored.IsProtocolActive(EmergencyProtocolType.StructuralShoring));

            var restoredDis = restored.GetDisaster(dis.DisasterId);
            Assert.NotNull(restoredDis);
            Assert.Equal(DisasterType.StructuralSubsidence, restoredDis.Type);
            Assert.Equal(system.GetDisaster(dis.DisasterId)!.MitigationInvested, restoredDis.MitigationInvested);
        }

        [Fact]
        public void DisasterIds_AreDeterministic_AcrossIdenticalRuns()
        {
            var rooms = new List<string> { "room_corridor" };
            var a = new DisasterResponseSystem();
            var b = new DisasterResponseSystem();

            var firstA = a.TriggerDisaster(DisasterType.StructuralSubsidence, DisasterSeverity.Severe, rooms, currentDay: 30);
            var firstB = b.TriggerDisaster(DisasterType.StructuralSubsidence, DisasterSeverity.Severe, rooms, currentDay: 30);
            Assert.Equal(firstA.DisasterId, firstB.DisasterId);

            var secondA = a.TriggerDisaster(DisasterType.ElectricalFire, DisasterSeverity.Minor, rooms, currentDay: 30);
            Assert.NotEqual(firstA.DisasterId, secondA.DisasterId);
        }
    }
}
