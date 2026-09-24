// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 165 — Modding Support & Mod Data Contract host session.
// Wraps the Core ModSupportSystem (the manifest contract / dependency /
// conflict / load-order authority). The overlay materializer stays
// JsonModLayering; this session governs which manifests are eligible and in
// what order, and reports the verdict. Enablement persists through the sole
// user-settings authority, never a campaign save section.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Mods;

namespace AtomicWar.GodotApp
{
    public sealed class ModSupportHostSession : HostSessionBase
    {
        private readonly ModSupportSystem _system = new();
        private string _lastEvent = string.Empty;

        public ModSupportSystem System => _system;
        public ModSupportCensus Census => _system.GetCensus();
        public string LastEvent => _lastEvent;
        public IReadOnlyList<ModConflict> Conflicts { get; private set; } = Array.Empty<ModConflict>();

        public ModSupportHostSession()
        {
            _system.OnModRegisteredSeam = reg =>
            {
                _lastEvent = $"Registered mod {reg.ModId} ({reg.Status})";
                RaiseStateChanged();
            };
            _system.OnModStatusChangedSeam = (modId, status) =>
            {
                _lastEvent = $"Mod {modId} is now {status}";
                RaiseStateChanged();
            };
            _system.OnModConflictDetectedSeam = conflict => { _lastEvent = conflict.Description; };
        }

        public static ModSupportHostSession Create(string dataDir)
        {
            var session = new ModSupportHostSession();
            if (!string.IsNullOrEmpty(dataDir))
                session.LoadSpecification(dataDir);
            return session;
        }

        /// <summary>
        /// Loads the authored manifest specification through the strict loader;
        /// the lenient fallback whitelist is therefore unreachable from the host.
        /// </summary>
        public void LoadSpecification(string dataDir)
        {
            if (string.IsNullOrEmpty(dataDir)) return;

            string path = Path.Combine(dataDir, "mod_manifest_schema.json");
            if (!File.Exists(path)) return;

            var spec = ModManifestSpecificationLoader.LoadFromJson(File.ReadAllText(path));
            _system.BindSpecification(spec);
            _lastEvent = $"Loaded manifest specification with {spec.AllowedCatalogs.Count} allowed catalogs.";
            RaiseStateChanged();
        }

        /// <summary>
        /// Discovers every mod directory, parses its manifest, and registers it
        /// with the Core authority. When <paramref name="enabledModIds"/> is a
        /// non-empty user selection, only those ids are considered; otherwise all
        /// discovered manifests are considered. Returns the number registered.
        /// </summary>
        public int DiscoverAndRegister(
            string modsDirectory,
            IReadOnlyCollection<string>? enabledModIds = null,
            string currentGameVersion = "1.0.0",
            int currentContractVersion = 1)
        {
            if (string.IsNullOrEmpty(modsDirectory) || !Directory.Exists(modsDirectory))
            {
                Conflicts = Array.Empty<ModConflict>();
                return 0;
            }

            var serializer = new SystemTextJsonSerializer();
            bool filterBySelection = enabledModIds != null && enabledModIds.Count > 0;
            int registered = 0;

            foreach (string directory in Directory.GetDirectories(modsDirectory).OrderBy(d => d, StringComparer.Ordinal))
            {
                string manifestPath = Path.Combine(directory, "manifest.json");
                if (!File.Exists(manifestPath)) continue;

                ModManifest? manifest;
                try
                {
                    manifest = serializer.Deserialize<ModManifest>(File.ReadAllText(manifestPath));
                }
                catch (Exception)
                {
                    continue; // malformed manifest JSON is reported by the layering engine
                }

                if (manifest == null) continue;

                string modId = manifest.EffectiveModId?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(modId)) continue;

                if (filterBySelection && !enabledModIds!.Contains(modId, StringComparer.Ordinal))
                    continue;

                _system.RegisterMod(manifest, currentGameVersion, currentContractVersion);
                registered++;
            }

            Conflicts = _system.DetectConflicts();
            return registered;
        }

        /// <summary>Authored-verdict mod ids in deterministic dependency order.</summary>
        public IReadOnlyList<string> ResolveLoadOrder() => _system.ResolveLoadOrder();

        /// <summary>Ids the authority judged eligible (status Enabled).</summary>
        public IReadOnlyList<string> EligibleModIds() =>
            _system.RegisteredMods.Values
                .Where(r => r.Status == ModStatus.Enabled)
                .OrderBy(r => r.LoadOrder)
                .ThenBy(r => r.ModId, StringComparer.Ordinal)
                .Select(r => r.ModId)
                .ToList();

        public bool SetModEnabled(string modId, bool enabled) =>
            !string.IsNullOrEmpty(modId) && _system.SetModEnabled(modId, enabled);

        public ModStatus GetStatus(string modId) =>
            _system.RegisteredMods.TryGetValue(modId, out var reg) ? reg.Status : ModStatus.Disabled;

        public ModSaveState CaptureState() => _system.CaptureState();

        public void RestoreState(ModSaveState? state) => _system.RestoreState(state);

        public IReadOnlyList<ModConflict> DetectConflicts()
        {
            Conflicts = _system.DetectConflicts();
            return Conflicts;
        }
    }
}
