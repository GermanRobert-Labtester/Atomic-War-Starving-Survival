// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 194 — Emergency Alert & Warning System: host integration and wiring.
// Core contract: authored catalog load, alert lifecycle, response-window
// countdown and priority escalation, evacuation protocols, and save/restore.
// Production wiring gates: the emergency_alert save section is registered, the
// campaign day heartbeat is classified, and the host composes catalog + save +
// the phase-5 day owner.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Emergency;
using Ashfall.Core.Save;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class Plan194EmergencyAlertHostIntegrationTests : CatalogTestBase
    {
        private static string RepoRoot()
        {
            string[] candidates = { Directory.GetCurrentDirectory(), AppContext.BaseDirectory };
            foreach (string start in candidates)
            {
                var directory = new DirectoryInfo(Path.GetFullPath(start));
                while (directory != null)
                {
                    if (File.Exists(Path.Combine(directory.FullName, "src", "Main.EmergencyAlerts.cs")))
                        return directory.FullName;
                    directory = directory.Parent;
                }
            }
            throw new DirectoryNotFoundException("Could not locate the ASHFALL repository root.");
        }

        private static string Source(string relativePath) =>
            File.ReadAllText(Path.Combine(RepoRoot(), relativePath));

        private static EmergencyAlertSystem Loaded()
        {
            var system = new EmergencyAlertSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "emergency_alerts.json")));
            return system;
        }

        [Fact]
        public void Catalog_LoadsAuthoredAlertTypes()
        {
            var system = Loaded();

            Assert.Equal(8, system.GetAllAlertTypes().Count);
            var storm = system.GetAlertType("alert_radiation_storm");
            Assert.NotNull(storm);
            Assert.Equal("critical", storm.severity);
            Assert.Equal(9, storm.base_priority);
            Assert.Equal(4, storm.response_window_hours);
        }

        [Fact]
        public void RaiseAlert_ComputesLifecycleFieldsFromCatalog()
        {
            var system = Loaded();
            var alert = system.RaiseAlert("alert_fire_outbreak", "ShelterFireHazardSystem", "zone_turbine_room", day: 5, hour: 14);

            Assert.Equal("emergency", alert.Severity);
            Assert.Equal(10, alert.CurrentPriority);
            Assert.Equal(1, alert.RemainingHours);
            Assert.Equal(1, system.ActiveAlertCount);
        }

        [Fact]
        public void AcknowledgeAndResolve_UpdateLifecycleAndHistory()
        {
            var system = Loaded();
            var alert = system.RaiseAlert("alert_disease_cluster", "DiseaseSystem", "hold_b", day: 3, hour: 8);

            Assert.True(system.AcknowledgeAlert(alert.AlertId));
            Assert.False(system.AcknowledgeAlert(alert.AlertId));
            Assert.True(alert.IsAcknowledged);

            Assert.True(system.ResolveAlert(alert.AlertId));
            Assert.False(system.ResolveAlert(alert.AlertId));
            Assert.Equal(0, system.ActiveAlertCount);
            Assert.Equal(1, system.HistoryCount);
        }

        [Fact]
        public void TickHour_DecrementsWindowAndEscalatesUnacknowledgedCritical()
        {
            var system = Loaded();
            var alert = system.RaiseAlert("alert_water_contamination", "WaterSourceSystem", "intake_primary", day: 2, hour: 10);
            Assert.Equal(5, alert.RemainingHours);
            Assert.Equal(8, alert.CurrentPriority);

            system.TickHour();

            Assert.Equal(4, alert.RemainingHours);
            Assert.Equal(9, alert.CurrentPriority);
        }

        [Fact]
        public void CaptureRestore_RoundTripsActiveHistoryAndProtocols()
        {
            var system = Loaded();
            var a1 = system.RaiseAlert("alert_sump_flooding", "SumpFloodingSystem", "subbasement", day: 4, hour: 6);
            system.AcknowledgeAlert(a1.AlertId);
            var a2 = system.RaiseAlert("alert_disease_cluster", "DiseaseSystem", "hold_a", day: 4, hour: 7);
            system.ResolveAlert(a2.AlertId);
            system.ActivateProtocol("Sump Drainage Muster", "subbasement", 4);

            var saved = system.CaptureState();
            var restored = Loaded();
            restored.RestoreState(saved);

            Assert.Equal(1, restored.ActiveAlertCount);
            Assert.Equal(1, restored.HistoryCount);
            Assert.True(restored.GetActiveAlerts().First().IsAcknowledged);
            Assert.Equal("alert_disease_cluster", restored.GetAlertHistory().First().TypeId);
            Assert.Single(restored.CaptureState().ActiveProtocols);
        }

        [Fact]
        public void SaveSectionRegistry_RegistersEmergencyAlertSection()
        {
            Assert.Contains(SaveSectionRegistry.All, s => s.SectionKey == "emergency_alert");
            Assert.Equal("emergency_alert_save.json", SaveSectionRegistry.SectionFileNames["emergency_alert"]);
        }

        [Fact]
        public void DayEventVocabulary_ClassifiesEmergencyAlertHeartbeat()
        {
            Assert.Equal(SemanticKind.Heartbeat, DayEventVocabulary.GetSemanticKind("emergency_alert_ticked"));
        }

        [Fact]
        public void HostSource_WiresCatalogSaveAndDayOwner()
        {
            string main = Source(Path.Combine("src", "Main.EmergencyAlerts.cs"));
            Assert.Contains("SetupEmergencyAlerts", main);
            Assert.Contains("SaveEmergencyAlerts", main);
            Assert.Contains("TickEmergencyAlerts", main);
            Assert.Contains("EmergencyAlertSaveStore", main);

            string owners = Source(Path.Combine("src", "Main.CampaignOwners.cs"));
            Assert.Contains("EmergencyAlertDayOwner", owners);
            Assert.Contains("\"emergency_alert\"", owners);

            string probe = Source(Path.Combine("src", "Host", "HostCli.EmergencyAlert.cs"));
            Assert.Contains("RunSelfTest", probe);
        }
    }
}
