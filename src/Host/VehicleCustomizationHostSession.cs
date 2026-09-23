// SPDX-License-Identifier: MIT
// ============================================================================
// Save Store : VehicleCustomizationSaveStore
// Core State : Ashfall.Core.Vehicles.VehicleCustomizationSystem (CaptureState/RestoreState)
// Host Caller: Main.VehicleCustomization (SetupVehicleCustomization / SaveVehicleCustomization)
// Purpose    : Plan 152 — Vehicle customization & mobile base: module install,
//              effective vehicle stats, bunk capacity, and deployed base camps.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Save;
using Ashfall.Core.Vehicles;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host-side persistence projection of the vehicle customization document.
    /// The Core owns the document; the host wraps it so the section travels in
    /// the same checksummed envelope every other integrated system uses.
    /// </summary>
    public sealed class VehicleCustomizationPersistedState
    {
        public int schema_version { get; set; } = 1;
        public string core_state { get; set; } = string.Empty;
    }

    public static class VehicleCustomizationSaveStore
    {
        public const string FileName = "vehicle_customization_save.json";
        public const string SectionName = "vehicle_customization";

        private static readonly SaveStore<VehicleCustomizationPersistedState> s_store =
            SaveStoreHub.Checksummed<VehicleCustomizationPersistedState>(FileName, nameof(VehicleCustomizationSaveStore));

        public static string SavePath => s_store.SavePath;
        public static bool Exists => s_store.Exists();

        public static string TryCapturePersisted(VehicleCustomizationPersistedState state) => s_store.CaptureBare(state);
        public static VehicleCustomizationPersistedState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
        public static bool TrySave(VehicleCustomizationPersistedState state) => s_store.TrySave(state);
        public static VehicleCustomizationPersistedState? TryLoad() => s_store.TryLoad();
    }

    /// <summary>
    /// Host session for Plan 152 (Vehicle Customization &amp; Mobile Base).
    /// Binds the strict authored module table onto the Core authority and
    /// exposes live vehicle builds and base camps to the campaign day owner.
    /// </summary>
    public sealed class VehicleCustomizationHostSession : HostSessionBase
    {
        /// <summary>Authored module table in the data authority.</summary>
        public const string CatalogFile = VehicleModuleCatalogLoader.FileName;

        private readonly VehicleCustomizationCatalog _catalog;
        private readonly VehicleCustomizationSystem _system;
        private string _lastEvent = string.Empty;

        public VehicleCustomizationSystem System => _system;
        public VehicleCustomizationCatalog Catalog => _catalog;
        public string LastEvent => _lastEvent;
        public VehicleCustomizationCensus Census => _system.GetCensus();

        /// <summary>True when the authored table loaded and was bound.</summary>
        public bool UsingAuthoredCatalog { get; private set; }

        /// <summary>Overlay error, when the authored table was rejected.</summary>
        public string? LoadError { get; private set; }

        public VehicleCustomizationHostSession(string? dataDir = null, IFileIO? files = null)
        {
            _catalog = new VehicleCustomizationCatalog();
            _system = new VehicleCustomizationSystem(_catalog);

            _system.OnModuleInstalledSeam += (vehicleId, moduleId) =>
            {
                _lastEvent = $"Module installed: {moduleId} on {vehicleId}.";
                RaiseStateChanged();
            };
            _system.OnModuleRemovedSeam += (vehicleId, moduleId) =>
            {
                _lastEvent = $"Module removed: {moduleId} from {vehicleId}.";
                RaiseStateChanged();
            };
            _system.OnBaseCampDeployedSeam += (vehicleId, locationId) =>
            {
                _lastEvent = $"Base camp deployed: {vehicleId} at {locationId}.";
                RaiseStateChanged();
            };
            _system.OnBaseCampPackedSeam += vehicleId =>
            {
                _lastEvent = $"Base camp packed: {vehicleId}.";
                RaiseStateChanged();
            };

            if (!string.IsNullOrEmpty(dataDir))
            {
                LoadCatalog(dataDir!, files);
            }
        }

        public static VehicleCustomizationHostSession Create(string dataDir, IFileIO? files = null)
            => new VehicleCustomizationHostSession(dataDir, files);

        /// <summary>
        /// Load the authored module table through the strict loader and bind
        /// only validated rows. A rejected table leaves the catalog empty
        /// rather than falling back to the lenient parser, so an authored typo
        /// can never become a live vehicle build.
        /// </summary>
        public void LoadCatalog(string dataDirectory, IFileIO? files = null)
        {
            if (string.IsNullOrEmpty(dataDirectory)) return;

            var loaded = VehicleModuleCatalogLoader.Load(dataDirectory, files ?? new FileSystemIO());
            if (loaded.HasErrors)
            {
                UsingAuthoredCatalog = false;
                LoadError = string.Join("; ", loaded.Errors);
                _lastEvent = "Vehicle module catalog rejected: " + LoadError;
                RaiseStateChanged();
                return;
            }

            _catalog.BindValidatedModules(loaded.Modules);
            UsingAuthoredCatalog = true;
            LoadError = null;
            _lastEvent = $"Bound {loaded.Modules.Count} vehicle module(s) from the authored table.";
            RaiseStateChanged();
        }

        public bool TryGetModule(string moduleId, out VehicleModule module)
            => _catalog.TryGetModule(moduleId, out module);

        public bool InstallModule(string vehicleId, string moduleId, int maxSlots = 4)
            => _system.InstallModule(vehicleId, moduleId, maxSlots);

        public bool RemoveModule(string vehicleId, string moduleId)
            => _system.RemoveModule(vehicleId, moduleId);

        public IReadOnlyList<string> GetInstalledModules(string vehicleId)
            => _system.GetInstalledModules(vehicleId);

        public (float speedMult, float cargoCapacity, float defense) CalculateEffectiveStats(
            string vehicleId, float baseSpeedMult = 1.0f, float baseCargoCapacity = 100.0f, float baseDefense = 0.0f)
            => _system.CalculateEffectiveStats(vehicleId, baseSpeedMult, baseCargoCapacity, baseDefense);

        public bool IsMobileBaseCapable(string vehicleId) => _system.IsMobileBaseCapable(vehicleId);

        public int GetBunkCapacity(string vehicleId) => _system.GetBunkCapacity(vehicleId);

        public bool DeployBaseCamp(string vehicleId, string locationId)
            => _system.DeployBaseCamp(vehicleId, locationId);

        public bool PackBaseCamp(string vehicleId) => _system.PackBaseCamp(vehicleId);

        public bool IsBaseCampDeployed(string vehicleId) => _system.IsBaseCampDeployed(vehicleId);

        public string? GetBaseCampLocation(string vehicleId) => _system.GetBaseCampLocation(vehicleId);

        public int RestSurvivorsInVehicle(string vehicleId, IReadOnlyList<string> survivorIds)
            => _system.RestSurvivorsInVehicle(vehicleId, survivorIds);

        public string CaptureCoreState() => _system.CaptureState();

        public void RestoreCoreState(string json) => _system.RestoreState(json);

        /// <summary>Project the live Core document into the persisted section shape.</summary>
        public VehicleCustomizationPersistedState CapturePersistedState()
            => new VehicleCustomizationPersistedState
            {
                schema_version = 1,
                core_state = _system.CaptureState()
            };
    }
}
