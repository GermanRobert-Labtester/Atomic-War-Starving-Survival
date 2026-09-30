// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 194 — Emergency alert & warning system host wiring.
// DEC-184: EmergencyAlertSystem owns alert records, prioritization, response
// windows, and evacuation protocol state. The host composes its catalog, save
// section, and the canonical campaign day clock. Threat owners (weather,
// defense, disease, power, water) remain sovereign — alerts report facts they
// already produce; this partial never invents a second threat authority.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Emergency;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        private EmergencyAlertHostSession? _emergencyAlerts;
        private bool _emergencyAlertsDirty;

        public EmergencyAlertHostSession? EmergencyAlerts => _emergencyAlerts;

        public EmergencyAlertHostSession EnsureEmergencyAlertsSession()
        {
            SetupEmergencyAlerts();
            return _emergencyAlerts!;
        }

        public void SetupEmergencyAlerts()
        {
            if (_emergencyAlerts != null) return;

            var saved = EmergencyAlertSaveStore.TryLoad();
            _emergencyAlerts = EmergencyAlertHostSession.Create(saved);
            _emergencyAlerts.StateChanged += () => _emergencyAlertsDirty = true;
            _emergencyAlerts.LoadCatalog(CatalogPath.ResolveDataDir());
        }

        /// <summary>
        /// Raises a live alert from an existing threat owner. The caller supplies
        /// the threat's own zone/source labels; this system only prioritizes,
        /// tracks the response window, and logs resolution.
        /// </summary>
        public ActiveEmergencyAlert RaiseEmergencyAlert(string typeId, string sourceSystem, string affectedZone)
        {
            SetupEmergencyAlerts();
            int day = _campaignDay?.Calendar.CurrentDay ?? _simDay;
            var alert = _emergencyAlerts!.RaiseAlert(typeId, sourceSystem, affectedZone, day, 0);
            _emergencyAlertsDirty = true;
            return alert;
        }

        public bool AcknowledgeEmergencyAlert(string alertId)
        {
            SetupEmergencyAlerts();
            bool ok = _emergencyAlerts!.AcknowledgeAlert(alertId);
            if (ok) _emergencyAlertsDirty = true;
            return ok;
        }

        public bool ResolveEmergencyAlert(string alertId)
        {
            SetupEmergencyAlerts();
            bool ok = _emergencyAlerts!.ResolveAlert(alertId);
            if (ok) _emergencyAlertsDirty = true;
            return ok;
        }

        public EvacuationProtocolState ActivateEvacuationProtocol(string protocolName, string assemblyZone, int assignedCount)
        {
            SetupEmergencyAlerts();
            var protocol = _emergencyAlerts!.ActivateProtocol(protocolName, assemblyZone, assignedCount);
            _emergencyAlertsDirty = true;
            return protocol;
        }

        public bool DeactivateEvacuationProtocol(string protocolId)
        {
            SetupEmergencyAlerts();
            bool ok = _emergencyAlerts!.DeactivateProtocol(protocolId);
            if (ok) _emergencyAlertsDirty = true;
            return ok;
        }

        /// <summary>
        /// Deterministic campaign-day advance. The alert clock is compressed to
        /// one alert-hour per campaign day so a 1-6 hour response window remains
        /// observable across a multi-day campaign; escalation fires when an
        /// unacknowledged critical/emergency alert ages or a window expires.
        /// </summary>
        public void TickEmergencyAlerts(int day)
        {
            SetupEmergencyAlerts();
            if (_emergencyAlerts == null) return;
            _emergencyAlerts.TickHour();
            _emergencyAlertsDirty = true;
        }

        /// <summary>Read-only snapshot for the dashboard/summary surfaces.</summary>
        public (int Active, int History, int Protocols, string HighestType, int HighestPriority, int RemainingHours) GetEmergencyAlertReadout()
        {
            SetupEmergencyAlerts();
            if (_emergencyAlerts == null) return (0, 0, 0, string.Empty, 0, 0);

            var highest = _emergencyAlerts.GetHighestPriorityAlert();
            return (
                _emergencyAlerts.ActiveAlertCount,
                _emergencyAlerts.HistoryCount,
                _emergencyAlerts.ActiveProtocolCount,
                highest?.TypeId ?? string.Empty,
                highest?.CurrentPriority ?? 0,
                highest?.RemainingHours ?? 0);
        }

        public void SaveEmergencyAlerts()
        {
            if (_emergencyAlerts == null) return;
            var state = _emergencyAlerts.CaptureState();
            if (CaptureSection(
                    EmergencyAlertSaveStore.SectionName,
                    EmergencyAlertSaveStore.TryCapturePersisted(state)))
            {
                _emergencyAlertsDirty = false;
            }
        }

        public void FlushEmergencyAlertsIfDirty()
        {
            if (_emergencyAlertsDirty)
            {
                SaveEmergencyAlerts();
            }
        }

        public void ResetEmergencyAlerts()
        {
            _emergencyAlerts = null;
            _emergencyAlertsDirty = false;
        }
    }
}
