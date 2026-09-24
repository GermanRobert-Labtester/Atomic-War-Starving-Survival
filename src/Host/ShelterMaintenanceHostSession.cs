// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : ShelterMaintenanceSaveStore
// Core State : Ashfall.Core.Shelter.ShelterMaintenanceState
// Host Caller: Main.ShelterMaintenance (SetupShelterMaintenance / SaveShelterMaintenance)
// Purpose    : Plan 186 — Shelter maintenance & component degradation system:
//              tracking physical condition, daily wear/degradation ticks,
//              preventive maintenance actions, and component failure alerts.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Save;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class ShelterMaintenanceSaveStore
    {
        public const string FileName = "shelter_maintenance_save.json";
        public const string SectionName = "shelter_maintenance";

        private static readonly SaveStore<ShelterMaintenanceState> s_store =
            SaveStoreHub.Checksummed<ShelterMaintenanceState>(FileName, nameof(ShelterMaintenanceSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(ShelterMaintenanceState state) => s_store.CaptureBare(state);
        public static ShelterMaintenanceState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(ShelterMaintenanceState state) => s_store.TrySave(state);
        public static ShelterMaintenanceState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session manager for Plan 186 (Shelter Maintenance & Degradation).
    /// Binds shelter components from shelter_components.json, executes daily degradation ticks,
    /// logs repair/maintenance interventions, and exposes component health to the shelter host and UI.
    /// </summary>
    public sealed class ShelterMaintenanceHostSession : HostSessionBase
    {
        private readonly ShelterMaintenanceSystem _system;
        private string _lastEvent = string.Empty;

        public ShelterMaintenanceSystem System => _system;
        public string LastEvent => _lastEvent;
        public ShelterMaintenanceCensus Census => _system.GetCensus();
        public int TrackedComponentCount => _system.TrackedComponentCount;
        public float AverageIntegrity => _system.GetAverageIntegrity();

        public ShelterMaintenanceHostSession(
            string? dataDir = null,
            ShelterMaintenanceSystem? system = null)
        {
            _system = system ?? new ShelterMaintenanceSystem();

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalogs(dataDir);
            }

            _system.OnComponentDegraded += (compId, cond) =>
            {
                _lastEvent = $"Component '{compId}' degraded to {cond:F1}%.";
                RaiseStateChanged();
            };

            _system.OnComponentWarning += (compId, cond) =>
            {
                _lastEvent = $"WARNING: Component '{compId}' dropped below threshold ({cond:F1}%).";
                RaiseStateChanged();
            };

            _system.OnComponentFailed += compId =>
            {
                _lastEvent = $"CRITICAL FAILURE: Component '{compId}' is no longer operational!";
                RaiseStateChanged();
            };

            _system.OnMaintenanceCompleted += record =>
            {
                _lastEvent = $"Maintenance '{record.ActionType}' on '{record.ComponentId}' completed (+{record.ConditionRestored:F1}%).";
                RaiseStateChanged();
            };
        }

        public static ShelterMaintenanceHostSession Create(
            string? dataDir = null,
            ShelterMaintenanceSystem? system = null)
        {
            return new ShelterMaintenanceHostSession(dataDir, system);
        }

        public void LoadCatalogs(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            try
            {
                var result = ShelterComponentCatalogLoader.Load(dataDir);
                if (result.Success && result.Catalog != null)
                {
                    _system.BindValidatedCatalog(result.Catalog);
                    _lastEvent = $"Loaded {result.Catalog.components.Count} shelter components.";
                    RaiseStateChanged();
                }
                else if (result.Errors.Count > 0)
                {
                    _lastEvent = $"Failed to load shelter component catalog: {string.Join("; ", result.Errors)}";
                }
            }
            catch (Exception ex)
            {
                _lastEvent = $"Exception loading shelter component catalog: {ex.Message}";
            }
        }

        public void TickDay(int currentDay, float weatherStressMult = 1.0f, float radiationStressMult = 1.0f)
        {
            _system.TickDay(currentDay, weatherStressMult, radiationStressMult);
            _lastEvent = $"Day {currentDay} maintenance tick applied (Integrity: {AverageIntegrity:F1}%).";
            RaiseStateChanged();
        }

        public bool PerformMaintenance(string componentId, string actionType, float skillLevel, int day)
        {
            bool ok = _system.PerformMaintenance(componentId, actionType, skillLevel, day);
            if (ok)
            {
                RaiseStateChanged();
            }
            return ok;
        }

        public ShelterComponentState? GetComponent(string componentId) => _system.GetComponent(componentId);
        public ShelterComponentDef? GetDefinition(string componentId) => _system.GetDefinition(componentId);
        public IReadOnlyCollection<ShelterComponentDef> GetAllDefinitions() => _system.GetAllDefinitions();
        public IReadOnlyList<ShelterComponentState> GetFailedComponents() => _system.GetFailedComponents();
        public IReadOnlyList<ShelterComponentState> GetWarningComponents() => _system.GetWarningComponents();

        public ShelterMaintenanceState CaptureState() => _system.CaptureState();

        public void RestoreState(ShelterMaintenanceState? state)
        {
            _system.RestoreState(state);
            RaiseStateChanged();
        }

        public void Reset()
        {
            _system.RestoreState(new ShelterMaintenanceState());
            _lastEvent = string.Empty;
            RaiseStateChanged();
        }
    }
}
