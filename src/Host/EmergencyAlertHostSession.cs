// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : EmergencyAlertSaveStore
// Core State : Ashfall.Core.Emergency.EmergencyAlertState
// Host Caller: Main.EmergencyAlerts
// Purpose    : Plan 194 — Emergency alert & warning system host session &
//              persistence. DEC-184 ownership: this system owns active alert
//              records, response windows, priority escalation, and evacuation
//              protocol state. It does not own the threats it reports on
//              (weather, defense, disease, power, water each remain sovereign).
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Emergency;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    public static class EmergencyAlertSaveStore
    {
        public const string FileName = "emergency_alert_save.json";
        public const string SectionName = "emergency_alert";

        private static readonly SaveStore<EmergencyAlertState> s_store =
            SaveStoreHub.Checksummed<EmergencyAlertState>(FileName, nameof(EmergencyAlertSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(EmergencyAlertState state) => s_store.CaptureBare(state);
        public static EmergencyAlertState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(EmergencyAlertState state) => s_store.TrySave(state);
        public static EmergencyAlertState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Plan 194 host session. Wraps <see cref="EmergencyAlertSystem"/> and
    /// loads the authored <c>emergency_alerts.json</c> catalog from the
    /// canonical StreamingAssets data directory.
    /// </summary>
    public sealed class EmergencyAlertHostSession : HostSessionBase
    {
        private readonly EmergencyAlertSystem _system;

        public EmergencyAlertSystem System => _system;
        public bool CatalogReady { get; private set; }
        public string LastEvent { get; private set; } = string.Empty;

        public EmergencyAlertHostSession(EmergencyAlertState? state = null)
        {
            _system = new EmergencyAlertSystem();
            if (state != null) _system.RestoreState(state);
        }

        public static EmergencyAlertHostSession Create(EmergencyAlertState? state = null) =>
            new EmergencyAlertHostSession(state);

        /// <summary>Loads the authored alert-type catalog from the data directory.</summary>
        public bool LoadCatalog(string dataDirectory)
        {
            if (string.IsNullOrWhiteSpace(dataDirectory)) return false;
            string path = Path.Combine(dataDirectory, "emergency_alerts.json");
            if (!File.Exists(path)) return false;
            try
            {
                _system.LoadCatalog(File.ReadAllText(path));
                CatalogReady = _system.GetAllAlertTypes().Count > 0;
                return CatalogReady;
            }
            catch (Exception)
            {
                CatalogReady = false;
                return false;
            }
        }

        public ActiveEmergencyAlert RaiseAlert(string typeId, string sourceSystem, string affectedZone, int day, int hour)
        {
            var alert = _system.RaiseAlert(typeId, sourceSystem, affectedZone, day, hour);
            LastEvent = $"Raised {typeId} ({alert.Severity}) in {affectedZone}.";
            RaiseStateChanged();
            return alert;
        }

        public bool AcknowledgeAlert(string alertId)
        {
            bool ok = _system.AcknowledgeAlert(alertId);
            if (ok)
            {
                LastEvent = $"Acknowledged {alertId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public bool ResolveAlert(string alertId)
        {
            bool ok = _system.ResolveAlert(alertId);
            if (ok)
            {
                LastEvent = $"Resolved {alertId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public void TickHour()
        {
            _system.TickHour();
            RaiseStateChanged();
        }

        public EvacuationProtocolState ActivateProtocol(string protocolName, string assemblyZone, int assignedCount)
        {
            var protocol = _system.ActivateProtocol(protocolName, assemblyZone, assignedCount);
            LastEvent = $"Activated protocol {protocol.Name}.";
            RaiseStateChanged();
            return protocol;
        }

        public bool DeactivateProtocol(string protocolId)
        {
            bool ok = _system.DeactivateProtocol(protocolId);
            if (ok)
            {
                LastEvent = $"Deactivated protocol {protocolId}.";
                RaiseStateChanged();
            }
            return ok;
        }

        public int ActiveAlertCount => _system.ActiveAlertCount;
        public int HistoryCount => _system.HistoryCount;
        public int ActiveProtocolCount => _system.CaptureState().ActiveProtocols.Count;

        public IReadOnlyList<ActiveEmergencyAlert> GetActiveAlerts() => _system.GetActiveAlerts();
        public IReadOnlyList<ActiveEmergencyAlert> GetAlertHistory() => _system.GetAlertHistory();
        public ActiveEmergencyAlert? GetHighestPriorityAlert() => _system.GetHighestPriorityAlert();

        public EmergencyAlertState CaptureState() => _system.CaptureState();

        public void RestoreState(EmergencyAlertState state)
        {
            _system.RestoreState(state);
            LastEvent = "Restored emergency alert state.";
            RaiseStateChanged();
        }

        public bool TrySave() => EmergencyAlertSaveStore.TrySave(CaptureState());

        public bool TryLoad()
        {
            var state = EmergencyAlertSaveStore.TryLoad();
            if (state == null) return false;
            RestoreState(state);
            return true;
        }
    }
}
