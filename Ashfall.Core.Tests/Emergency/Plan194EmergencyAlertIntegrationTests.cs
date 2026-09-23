// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 194: Emergency Alert & Warning System — Integration Tests
// Verifies alert catalog loading, alert creation, response window countdown,
// priority escalation, evacuation protocol activation, and save/restore state.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Emergency;

namespace Ashfall.Core.Tests.Emergency
{
    public sealed class Plan194EmergencyAlertIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsAlertTypesAndPriorities()
        {
            var system = new EmergencyAlertSystem();
            string path = Path.Combine(DataDirectory, "emergency_alerts.json");
            Assert.True(File.Exists(path), $"emergency_alerts.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var types = system.GetAllAlertTypes();
            Assert.Equal(8, types.Count);

            var radStorm = system.GetAlertType("alert_radiation_storm");
            Assert.NotNull(radStorm);
            Assert.Equal("critical", radStorm.severity);
            Assert.Equal(9, radStorm.base_priority);
            Assert.Equal(4, radStorm.response_window_hours);
            Assert.Equal("shelter_in_place", radStorm.recommended_protocol);
        }

        [Fact]
        public void RaiseAlert_CreatesActiveAlertWithCalculatedPriority()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));

            ActiveEmergencyAlert? raised = null;
            system.OnAlertRaised += a => raised = a;

            var alert = system.RaiseAlert("alert_fire_outbreak", "ShelterFireHazardSystem", "zone_turbine_room", day: 5, hour: 14);

            Assert.NotNull(alert);
            Assert.NotNull(raised);
            Assert.Equal(1, system.ActiveAlertCount);
            Assert.Equal("emergency", alert.Severity);
            Assert.Equal(10, alert.CurrentPriority);
            Assert.Equal(1, alert.RemainingHours);
            Assert.False(alert.IsAcknowledged);
            Assert.False(alert.IsResolved);
        }

        [Fact]
        public void AcknowledgeAndResolveAlert_UpdatesLifecycleState()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));

            var alert = system.RaiseAlert("alert_disease_cluster", "DiseaseSystem", "hold_b", day: 3, hour: 8);

            string? ackedId = null;
            string? resolvedId = null;
            system.OnAlertAcknowledged += id => ackedId = id;
            system.OnAlertResolved += id => resolvedId = id;

            bool acked = system.AcknowledgeAlert(alert.AlertId);
            Assert.True(acked);
            Assert.Equal(alert.AlertId, ackedId);
            Assert.True(alert.IsAcknowledged);

            bool resolved = system.ResolveAlert(alert.AlertId);
            Assert.True(resolved);
            Assert.Equal(alert.AlertId, resolvedId);
            Assert.Equal(0, system.ActiveAlertCount);
            Assert.Equal(1, system.HistoryCount);
        }

        [Fact]
        public void TickHour_DecrementsResponseWindowAndEscalatesPriority()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));

            // Water contamination: base priority 8, 5 hours window, critical severity
            var alert = system.RaiseAlert("alert_water_contamination", "WaterSourceSystem", "intake_primary", day: 2, hour: 10);
            Assert.Equal(8, alert.CurrentPriority);
            Assert.Equal(5, alert.RemainingHours);

            // Tick 1 hour unacknowledged: remaining becomes 4, priority escalates to 9
            system.TickHour();
            Assert.Equal(4, alert.RemainingHours);
            Assert.Equal(9, alert.CurrentPriority);
        }

        [Fact]
        public void GetHighestPriorityAlert_ReturnsMostUrgentThreat()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));

            system.RaiseAlert("alert_power_brownout", "PowerSystem", "grid", day: 1, hour: 1); // priority 5
            system.RaiseAlert("alert_raider_assault", "DefenseSystem", "surface_gate", day: 1, hour: 2); // priority 10

            var highest = system.GetHighestPriorityAlert();
            Assert.NotNull(highest);
            Assert.Equal("alert_raider_assault", highest.TypeId);
            Assert.Equal(10, highest.CurrentPriority);
        }

        [Fact]
        public void SaveRestoreState_PreservesActiveAlertsAndHistory()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));

            var a1 = system.RaiseAlert("alert_sump_flooding", "SumpFloodingSystem", "subbasement", day: 4, hour: 6);
            system.AcknowledgeAlert(a1.AlertId);

            var a2 = system.RaiseAlert("alert_disease_cluster", "DiseaseSystem", "hold_a", day: 4, hour: 7);
            system.ResolveAlert(a2.AlertId);

            var protocol = system.ActivateProtocol("Sump Drainage Muster", "subbasement", 4);

            var state = system.CaptureState();

            var restoredSystem = new EmergencyAlertSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal(1, restoredSystem.ActiveAlertCount);
            Assert.Equal(1, restoredSystem.HistoryCount);

            var active = restoredSystem.GetActiveAlerts().FirstOrDefault();
            Assert.NotNull(active);
            Assert.Equal("alert_sump_flooding", active.TypeId);
            Assert.True(active.IsAcknowledged);

            var history = restoredSystem.GetAlertHistory().FirstOrDefault();
            Assert.NotNull(history);
            Assert.Equal("alert_disease_cluster", history.TypeId);
        }
    }
}
