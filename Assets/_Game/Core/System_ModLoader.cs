// System_ModLoader.cs — Modding API (Prompt #864)
// Ensures JSON Importer reads from /Mods/ folder.
// Community injects SurvivorProfiles, LootTables, Encounters without touching C#.
using System;
using System.Collections.Generic;
using System.IO;

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

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Initialize the mod loader with the path to the /Mods/ folder.
        /// </summary>
        public void Initialize(string modsPath)
        {
            _state.mods_folder_path = modsPath;
            _state.loaded_mods.Clear();
            _state.errors.Clear();
            _overrides.Clear();
            _overrideSourceMod.Clear();
        }

        /// <summary>
        /// Scan the mods folder for subfolders and fire OnModDiscovered
        /// for each one found.
        /// </summary>
        public void ScanForMods()
        {
            if (string.IsNullOrEmpty(_state.mods_folder_path) ||
                !Directory.Exists(_state.mods_folder_path))
            {
                LogError("_system", "Mods folder not found: " + _state.mods_folder_path);
                return;
            }

            string[] dirs = Directory.GetDirectories(_state.mods_folder_path);
            for (int i = 0; i < dirs.Length; i++)
            {
                string modName = Path.GetFileName(dirs[i]);
                OnModDiscovered?.Invoke(modName);
            }
        }

        /// <summary>
        /// Load a single mod by folder name. Reads manifest.json and
        /// registers any data overrides.
        /// </summary>
        public void LoadMod(string modFolderName)
        {
            string modPath = Path.Combine(_state.mods_folder_path, modFolderName);
            string manifestPath = Path.Combine(modPath, "manifest.json");

            if (!File.Exists(manifestPath))
            {
                LogError(modFolderName, "manifest.json not found");
                return;
            }

            int itemCount = 0;
            try
            {
                // Read manifest to discover override data files
                // In a full implementation this would parse the manifest JSON
                // and load each referenced data file. For now we scan for .json files.
                string[] jsonFiles = Directory.GetFiles(modPath, "*.json");
                for (int i = 0; i < jsonFiles.Length; i++)
                {
                    string fileName = Path.GetFileNameWithoutExtension(jsonFiles[i]);
                    if (fileName == "manifest")
                        continue;

                    string data = File.ReadAllText(jsonFiles[i]);
                    string dataId = modFolderName + "/" + fileName;

                    RegisterOverride(dataId, data, modFolderName);
                    itemCount++;
                }

                _state.loaded_mods.Add(modFolderName);
                OnModLoaded?.Invoke(modFolderName, itemCount);
            }
            catch (Exception ex)
            {
                LogError(modFolderName, ex.Message);
            }
        }

        /// <summary>
        /// Returns the list of successfully loaded mod names.
        /// </summary>
        public IReadOnlyList<string> GetLoadedMods()
        {
            return _state.loaded_mods.AsReadOnly();
        }

        /// <summary>
        /// Returns true if a mod provides an override for the given data id.
        /// </summary>
        public bool HasOverride(string dataId)
        {
            return _overrides.ContainsKey(dataId);
        }

        /// <summary>
        /// Returns the mod-provided override data for a given id, or null.
        /// </summary>
        public string GetOverrideData(string dataId)
        {
            _overrides.TryGetValue(dataId, out string data);
            if (data != null && _overrideSourceMod.TryGetValue(dataId, out string modName))
            {
                OnOverrideApplied?.Invoke(dataId, modName);
            }
            return data;
        }

        /// <summary>
        /// Returns all logged errors.
        /// </summary>
        public IReadOnlyList<string> GetErrors()
        {
            return _state.errors.AsReadOnly();
        }

        // ── Internals ──────────────────────────────────────────────────

        private void RegisterOverride(string dataId, string data, string modName)
        {
            if (_state.override_priority == "mod_first")
            {
                // Mod data wins — overwrite any existing entry
                _overrides[dataId] = data;
                _overrideSourceMod[dataId] = modName;
            }
            else
            {
                // base_first — only register if no base entry exists yet
                if (!_overrides.ContainsKey(dataId))
                {
                    _overrides[dataId] = data;
                    _overrideSourceMod[dataId] = modName;
                }
            }
        }

        private void LogError(string modName, string error)
        {
            string entry = $"[{modName}] {error}";
            _state.errors.Add(entry);
            OnLoadError?.Invoke(modName, error);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public ModLoaderState CaptureState()
        {
            return _state;
        }

        public void RestoreState(ModLoaderState state)
        {
            _state = state ?? new ModLoaderState();
        }
    }
}
