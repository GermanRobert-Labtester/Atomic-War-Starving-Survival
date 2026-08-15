// System_ModLoader.cs — Modding API (Prompt #864)
// Ensures JSON Importer reads from /Mods/ folder.
// Community injects SurvivorProfiles, LootTables, Encounters without touching C#.
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Mod Loader system (Prompt #864).
    /// Tracks loaded mods, override priority, and any load errors.
    /// </summary>
    [Serializable]
    public class ModLoaderState
    {
        public string system_id = "system_mod_loader";
        public string mods_folder_path = string.Empty;
        public List<string> loaded_mods = new List<string>();
        public string override_priority = "mod_first"; // "mod_first" or "base_first"
        public List<string> errors = new List<string>();
    }

    /// <summary>
    /// Mod Loader system (Prompt #864).
    /// Scans /Mods/ for subfolders, each with a manifest.json.
    /// Mods can override existing data or add new entries.
    /// "mod_first" = mod data wins. Errors logged but don't crash game.
    /// </summary>
    public class System_ModLoader
    {
        public const string DefaultPriorityModFirst = "mod_first";
        public const string PriorityBaseFirst = "base_first";
        public const string ModsFolderName = "Mods";

        // ── Events ─────────────────────────────────────────────────────
        public event Action<string> OnModDiscovered;
        public event Action<string, int> OnModLoaded;
        public event Action<string, string> OnOverrideApplied;
        public event Action<string, string> OnLoadError;

        // ── State ──────────────────────────────────────────────────────
        private ModLoaderState _state = new ModLoaderState();

        // In-memory registry: dataId → mod-provided JSON string
        private readonly Dictionary<string, string> _overrides =
            new Dictionary<string, string>();

        private readonly Dictionary<string, string> _overrideSourceMod =
            new Dictionary<string, string>();

        /// <summary>
        /// Live instance set by GameBootstrap (or tests). JSON importers prefer this
        /// when resolving base data file overrides.
        /// </summary>
        public static System_ModLoader Active { get; private set; }

        public string SystemId =>
            string.IsNullOrEmpty(_state.system_id) ? "system_mod_loader" : _state.system_id;

        public string ModsFolderPath => _state.mods_folder_path ?? string.Empty;
        public string OverridePriority =>
            string.IsNullOrEmpty(_state.override_priority) ? DefaultPriorityModFirst : _state.override_priority;

        public int OverrideCount => _overrides.Count;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>Mark this instance as the process-wide Active mod loader.</summary>
        public void SetAsActive()
        {
            Active = this;
        }

        /// <summary>Clear Active if it points at this instance.</summary>
        public void ClearActiveIfSelf()
        {
            if (Active == this)
                Active = null;
        }

        /// <summary>
        /// Initialize the mod loader with the path to the /Mods/ folder.
        /// Clears loaded mods, errors, and override registry.
        /// </summary>
        public void Initialize(string modsPath)
        {
            _state.mods_folder_path = modsPath ?? string.Empty;
            if (string.IsNullOrEmpty(_state.system_id))
                _state.system_id = "system_mod_loader";
            if (string.IsNullOrEmpty(_state.override_priority))
                _state.override_priority = DefaultPriorityModFirst;
            if (_state.loaded_mods == null)
                _state.loaded_mods = new List<string>();
            else
                _state.loaded_mods.Clear();
            if (_state.errors == null)
                _state.errors = new List<string>();
            else
                _state.errors.Clear();
            _overrides.Clear();
            _overrideSourceMod.Clear();
        }

        public void SetOverridePriority(string priority)
        {
            if (priority == PriorityBaseFirst || priority == DefaultPriorityModFirst)
                _state.override_priority = priority;
        }

        /// <summary>
        /// Preferred Mods root: StreamingAssets/Mods when present, else persistentDataPath/Mods
        /// (created if missing). Safe for EditMode (no Unity scene required).
        /// </summary>
        public static string ResolveDefaultModsPath()
        {
            string streaming = Path.Combine(Application.streamingAssetsPath, ModsFolderName);
            if (Directory.Exists(streaming))
                return streaming;

            string persistent = Path.Combine(Application.persistentDataPath, ModsFolderName);
            try
            {
                if (!Directory.Exists(persistent))
                    Directory.CreateDirectory(persistent);
            }
            catch (Exception ex)
            {
                // best-effort; Initialize will log if path unusable
                Debug.LogWarning($"[ModLoader] Could not create mods directory '{persistent}': {ex.Message}");
            }
            return persistent;
        }

        /// <summary>
        /// Scan the mods folder for subfolders and fire OnModDiscovered
        /// for each one found. Returns discovered folder names.
        /// </summary>
        public List<string> ScanForMods()
        {
            var found = new List<string>();
            if (string.IsNullOrEmpty(_state.mods_folder_path) ||
                !Directory.Exists(_state.mods_folder_path))
            {
                LogError("_system", "Mods folder not found: " + _state.mods_folder_path);
                return found;
            }

            string[] dirs = Directory.GetDirectories(_state.mods_folder_path);
            for (int i = 0; i < dirs.Length; i++)
            {
                string modName = Path.GetFileName(dirs[i]);
                if (string.IsNullOrEmpty(modName)) continue;
                found.Add(modName);
                OnModDiscovered?.Invoke(modName);
            }
            return found;
        }

        /// <summary>
        /// Load a single mod by folder name. Requires manifest.json; registers
        /// every other *.json as an override under "modName/fileStem".
        /// </summary>
        public void LoadMod(string modFolderName)
        {
            if (string.IsNullOrEmpty(modFolderName))
            {
                LogError("_system", "LoadMod called with empty folder name");
                return;
            }

            string modPath = Path.Combine(_state.mods_folder_path ?? string.Empty, modFolderName);
            string manifestPath = Path.Combine(modPath, "manifest.json");

            if (!Directory.Exists(modPath))
            {
                LogError(modFolderName, "mod folder not found");
                return;
            }

            if (!File.Exists(manifestPath))
            {
                LogError(modFolderName, "manifest.json not found");
                return;
            }

            int itemCount = 0;
            try
            {
                string[] jsonFiles = Directory.GetFiles(modPath, "*.json");
                for (int i = 0; i < jsonFiles.Length; i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(jsonFiles[i]);
                    if (string.Equals(fileName, "manifest", StringComparison.OrdinalIgnoreCase))
                        continue;

                    string data = File.ReadAllText(jsonFiles[i]);
                    string dataId = modFolderName + "/" + fileName;

                    RegisterOverride(dataId, data, modFolderName);
                    itemCount++;
                }

                if (_state.loaded_mods == null)
                    _state.loaded_mods = new List<string>();
                if (!_state.loaded_mods.Contains(modFolderName))
                    _state.loaded_mods.Add(modFolderName);
                OnModLoaded?.Invoke(modFolderName, itemCount);
            }
            catch (Exception ex)
            {
                LogError(modFolderName, ex.Message);
            }
        }

        /// <summary>
        /// Discover all mod folders and load each that has a manifest.
        /// Returns number of mods successfully listed in loaded_mods after the pass.
        /// </summary>
        public int LoadAllMods()
        {
            var discovered = ScanForMods();
            for (int i = 0; i < discovered.Count; i++)
                LoadMod(discovered[i]);
            return _state.loaded_mods != null ? _state.loaded_mods.Count : 0;
        }

        public IReadOnlyList<string> GetLoadedMods()
        {
            if (_state.loaded_mods == null)
                _state.loaded_mods = new List<string>();
            return _state.loaded_mods.AsReadOnly();
        }

        public bool HasOverride(string dataId)
        {
            return !string.IsNullOrEmpty(dataId) && _overrides.ContainsKey(dataId);
        }

        /// <summary>
        /// Returns the mod-provided override data for a given id, or null.
        /// Id form: "modFolder/fileStem" (e.g. "my_mod/items").
        /// </summary>
        public string GetOverrideData(string dataId)
        {
            if (string.IsNullOrEmpty(dataId)) return null;
            if (!_overrides.TryGetValue(dataId, out string data))
                return null;
            if (data != null && _overrideSourceMod.TryGetValue(dataId, out string modName))
                OnOverrideApplied?.Invoke(dataId, modName);
            return data;
        }

        /// <summary>
        /// Resolve override by base file stem (e.g. "items", "recipes").
        /// Prefers exact key match, then any "modName/stem" under mod_first (last write wins already).
        /// </summary>
        public string FindOverrideByFileStem(string fileStem)
        {
            if (string.IsNullOrEmpty(fileStem)) return null;

            if (_overrides.TryGetValue(fileStem, out string exact))
            {
                if (_overrideSourceMod.TryGetValue(fileStem, out string modExact))
                    OnOverrideApplied?.Invoke(fileStem, modExact);
                return exact;
            }

            string suffix = "/" + fileStem;
            string best = null;
            string bestId = null;
            foreach (var kv in _overrides)
            {
                if (kv.Key != null &&
                    (kv.Key.EndsWith(suffix, StringComparison.OrdinalIgnoreCase)
                     || string.Equals(kv.Key, fileStem, StringComparison.OrdinalIgnoreCase)))
                {
                    best = kv.Value;
                    bestId = kv.Key;
                }
            }

            if (best != null && bestId != null &&
                _overrideSourceMod.TryGetValue(bestId, out string modName))
            {
                OnOverrideApplied?.Invoke(bestId, modName);
            }
            return best;
        }

        /// <summary>
        /// Importer helper: if an Active loader has an override for <paramref name="fileStem"/>,
        /// return it; otherwise read <paramref name="baseFilePath"/> when it exists.
        /// </summary>
        public static string ResolveJsonText(string fileStem, string baseFilePath, System_ModLoader loader = null)
        {
            loader = loader ?? Active;
            if (loader != null)
            {
                string over = loader.FindOverrideByFileStem(fileStem);
                if (!string.IsNullOrEmpty(over))
                    return over;
            }

            if (!string.IsNullOrEmpty(baseFilePath) && File.Exists(baseFilePath))
                return File.ReadAllText(baseFilePath);
            return null;
        }

        public IReadOnlyList<string> GetErrors()
        {
            if (_state.errors == null)
                _state.errors = new List<string>();
            return _state.errors.AsReadOnly();
        }

        // ── Internals ──────────────────────────────────────────────────

        private void RegisterOverride(string dataId, string data, string modName)
        {
            if (_state.override_priority == PriorityBaseFirst)
            {
                if (!_overrides.ContainsKey(dataId))
                {
                    _overrides[dataId] = data;
                    _overrideSourceMod[dataId] = modName;
                }
            }
            else
            {
                // mod_first — overwrite
                _overrides[dataId] = data;
                _overrideSourceMod[dataId] = modName;
            }
        }

        private void LogError(string modName, string error)
        {
            if (_state.errors == null)
                _state.errors = new List<string>();
            string entry = $"[{modName}] {error}";
            _state.errors.Add(entry);
            OnLoadError?.Invoke(modName, error);
        }

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>Snapshot of path, priority, loaded names, and errors (not override bodies).</summary>
        public ModLoaderState CaptureState()
        {
            var cap = new ModLoaderState
            {
                system_id = string.IsNullOrEmpty(_state.system_id) ? "system_mod_loader" : _state.system_id,
                mods_folder_path = _state.mods_folder_path ?? string.Empty,
                override_priority = string.IsNullOrEmpty(_state.override_priority)
                    ? DefaultPriorityModFirst
                    : _state.override_priority,
                loaded_mods = new List<string>(),
                errors = new List<string>()
            };

            if (_state.loaded_mods != null)
            {
                for (int i = 0; i < _state.loaded_mods.Count; i++)
                    cap.loaded_mods.Add(_state.loaded_mods[i]);
            }

            if (_state.errors != null)
            {
                for (int i = 0; i < _state.errors.Count; i++)
                    cap.errors.Add(_state.errors[i]);
            }

            return cap;
        }

        /// <summary>
        /// Restores path/priority, then reloads each listed mod from disk to rebuild overrides.
        /// </summary>
        public void RestoreState(ModLoaderState state)
        {
            if (state == null)
            {
                Initialize(string.Empty);
                return;
            }

            string path = state.mods_folder_path ?? string.Empty;
            string priority = string.IsNullOrEmpty(state.override_priority)
                ? DefaultPriorityModFirst
                : state.override_priority;
            var mods = new List<string>();
            if (state.loaded_mods != null)
            {
                for (int i = 0; i < state.loaded_mods.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.loaded_mods[i]))
                        mods.Add(state.loaded_mods[i]);
                }
            }

            Initialize(path);
            SetOverridePriority(priority);
            _state.system_id = string.IsNullOrEmpty(state.system_id) ? "system_mod_loader" : state.system_id;

            for (int i = 0; i < mods.Count; i++)
                LoadMod(mods[i]);
        }
    }
}
