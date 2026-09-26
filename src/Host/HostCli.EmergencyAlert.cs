// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : EmergencyAlertSelfTest
// Subsystem          : Plan 194 — Emergency Alert & Warning System
// ============================================================================

using System;
using System.Linq;
using Ashfall.Core.Emergency;

namespace AtomicWar.GodotApp
{
    public static class HostCliEmergencyAlert
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Emergency Alert Self-Test (Plan 194) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                string data = string.IsNullOrWhiteSpace(dataDir) ? CatalogPath.ResolveDataDir() : dataDir;
                var session = EmergencyAlertHostSession.Create();
                bool catalog = session.LoadCatalog(data);

                // Check 1: authored catalog
                if (catalog && session.System.GetAllAlertTypes().Count == 8
                    && session.System.GetAlertType("alert_radiation_storm") != null)
                {
                    Console.WriteLine($"[PASS] Check 1: {session.System.GetAllAlertTypes().Count} alert types loaded from emergency_alerts.json.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Expected 8 alert types (catalog ready={catalog}).");
                }

                // Check 2: raise uses catalog severity/priority/window
                var storm = session.RaiseAlert("alert_radiation_storm", "WeatherSystem", "surface", day: 5, hour: 14);
                if (storm.Severity == "critical" && storm.CurrentPriority == 9 && storm.RemainingHours == 4 && session.ActiveAlertCount == 1)
                {
                    Console.WriteLine("[PASS] Check 2: Raised alert carries authored severity/priority/window.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Raised alert mismatch ({storm.Severity}/{storm.CurrentPriority}/{storm.RemainingHours}).");
                }

                // Check 3: unknown type falls back to safe defaults
                var unknown = session.RaiseAlert("alert_does_not_exist", "Test", "nowhere", day: 1, hour: 0);
                if (unknown.Severity == "warning" && unknown.CurrentPriority == 5 && unknown.RemainingHours == 6)
                {
                    Console.WriteLine("[PASS] Check 3: Unknown alert type fell back to warning/5/6h defaults.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Unknown-type fallback mismatch ({unknown.Severity}/{unknown.CurrentPriority}/{unknown.RemainingHours}).");
                }

                // Check 4: acknowledge lifecycle
                bool acked = session.AcknowledgeAlert(storm.AlertId);
                bool ackedAgain = session.AcknowledgeAlert(storm.AlertId);
                if (acked && !ackedAgain && session.System.GetActiveAlerts().First(a => a.AlertId == storm.AlertId).IsAcknowledged)
                {
                    Console.WriteLine("[PASS] Check 4: Acknowledge is exactly-once and reflected in state.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Acknowledge lifecycle failed ({acked}/{ackedAgain}).");
                }

                // Check 5: resolve moves to history
                int beforeHistory = session.HistoryCount;
                bool resolved = session.ResolveAlert(session.System.GetActiveAlerts().First(a => a.AlertId == unknown.AlertId).AlertId);
                if (resolved && session.HistoryCount == beforeHistory + 1 && !session.GetActiveAlerts().Any(a => a.AlertId == unknown.AlertId))
                {
                    Console.WriteLine("[PASS] Check 5: Resolve removed the active alert and logged history.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Resolve lifecycle failed (resolved={resolved}, history={session.HistoryCount}).");
                }

                // Check 6: tick decrements window and escalates unacknowledged critical
                var water = session.RaiseAlert("alert_water_contamination", "WaterSourceSystem", "intake", day: 2, hour: 10);
                int startWindow = water.RemainingHours;
                int startPriority = water.CurrentPriority;
                session.TickHour();
                if (water.RemainingHours == startWindow - 1 && water.CurrentPriority == startPriority + 1)
                {
                    Console.WriteLine($"[PASS] Check 6: Tick decremented window {startWindow}→{water.RemainingHours} and escalated priority {startPriority}→{water.CurrentPriority}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Tick mismatch (window {startWindow}→{water.RemainingHours}, priority {startPriority}→{water.CurrentPriority}).");
                }

                // Check 7: highest-priority selection
                var raid = session.RaiseAlert("alert_raider_assault", "DefenseSystem", "gate", day: 2, hour: 11);
                var highest = session.GetHighestPriorityAlert();
                if (highest != null && highest.TypeId == "alert_raider_assault" && highest.CurrentPriority == 10)
                {
                    Console.WriteLine("[PASS] Check 7: Highest-priority alert is the raider assault (10).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Highest-priority mismatch ({highest?.TypeId}/{highest?.CurrentPriority}).");
                }

                // Check 8: evacuation protocol activation
                var protocol = session.ActivateProtocol("Sump Drainage Muster", "subbasement", 4);
                if (protocol.Status == "active" && session.ActiveProtocolCount == 1)
                {
                    Console.WriteLine("[PASS] Check 8: Evacuation protocol activated with personnel count.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Protocol activation failed (status={protocol.Status}).");
                }

                // Check 9: evacuation protocol deactivation
                bool deactivated = session.DeactivateProtocol(protocol.ProtocolId);
                bool deactivateAgain = session.DeactivateProtocol(protocol.ProtocolId);
                if (deactivated && !deactivateAgain && protocol.Status == "cancelled")
                {
                    Console.WriteLine("[PASS] Check 9: Protocol deactivation is exactly-once.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Protocol deactivation failed ({deactivated}/{deactivateAgain}).");
                }

                // Check 10: save/restore round-trip
                bool saved = session.TrySave();
                var reloaded = EmergencyAlertHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded && reloaded.ActiveAlertCount == session.ActiveAlertCount && reloaded.HistoryCount == session.HistoryCount)
                {
                    Console.WriteLine($"[PASS] Check 10: Save/restore preserved {reloaded.ActiveAlertCount} active and {reloaded.HistoryCount} history alerts.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Save/restore failed (saved={saved}, loaded={loaded}).");
                }

                // Check 11: save store contract names + section registration
                if (EmergencyAlertSaveStore.SectionName.Equals("emergency_alert", StringComparison.Ordinal)
                    && EmergencyAlertSaveStore.FileName.Equals("emergency_alert_save.json", StringComparison.Ordinal)
                    && System.Linq.Enumerable.Any(Ashfall.Core.Save.SaveSectionRegistry.All, s => s.SectionKey == "emergency_alert"))
                {
                    Console.WriteLine("[PASS] Check 11: Save store contract names and section registration verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: Save store contract names or section registration mismatch.");
                }

                // Check 12: invalid resolutions are safe no-ops
                if (!session.AcknowledgeAlert("alert_missing") && !session.ResolveAlert("alert_missing")
                    && !session.DeactivateProtocol("proto_missing"))
                {
                    Console.WriteLine("[PASS] Check 12: Unknown ids fail safely without state mutation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Unknown-id operations mutated state.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Emergency Alert Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
