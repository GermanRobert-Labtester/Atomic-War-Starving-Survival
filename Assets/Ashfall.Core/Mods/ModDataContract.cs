// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace Ashfall.Core.Mods
{
    public enum ModStatus
    {
        Enabled,
        Disabled,
        Incompatible,
        MissingDependency,
        CyclicDependency,
        InvalidManifest
    }

    [Serializable]
    public sealed class ModConflictPolicyDef
    {
        [JsonPropertyName("policy_id")]
        public string PolicyId { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ModManifestSpecificationDef
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("schema_name")]
        public string SchemaName { get; set; } = string.Empty;

        [JsonPropertyName("current_mod_contract_version")]
        public int CurrentModContractVersion { get; set; } = 1;

        [JsonPropertyName("supported_game_versions")]
        public string SupportedGameVersions { get; set; } = ">=1.0.0 <3.0.0";

        [JsonPropertyName("required_manifest_fields")]
        public List<string> RequiredManifestFields { get; set; } = new List<string>();

        [JsonPropertyName("optional_manifest_fields")]
        public List<string> OptionalManifestFields { get; set; } = new List<string>();

        [JsonPropertyName("allowed_catalogs")]
        public List<string> AllowedCatalogs { get; set; } = new List<string>();

        [JsonPropertyName("conflict_policies")]
        public List<ModConflictPolicyDef> ConflictPolicies { get; set; } = new List<ModConflictPolicyDef>();
    }

    [Serializable]
    public sealed class ModRegistration
    {
        public string ModId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Version { get; set; } = "1.0.0";
        public int LoadOrder { get; set; }
        public bool AllowOverrides { get; set; }
        public ModStatus Status { get; set; } = ModStatus.Enabled;
        public string StatusReason { get; set; } = string.Empty;
        public List<string> Catalogs { get; set; } = new List<string>();
        public List<string> Dependencies { get; set; } = new List<string>();
        public ModManifest? Manifest { get; set; }
    }

    [Serializable]
    public sealed class ModConflict
    {
        public string ModIdA { get; set; } = string.Empty;
        public string ModIdB { get; set; } = string.Empty;
        public string CatalogName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class ModContractValidationResult
    {
        public bool IsValid => Errors.Count == 0;
        public List<string> Errors { get; } = new List<string>();
    }

    [Serializable]
    public sealed class ModSaveState
    {
        [JsonPropertyName("schema_version")]
        public int SchemaVersion { get; set; } = 1;

        [JsonPropertyName("active_mod_ids")]
        public List<string> ActiveModIds { get; set; } = new List<string>();

        [JsonPropertyName("disabled_mod_ids")]
        public List<string> DisabledModIds { get; set; } = new List<string>();

        [JsonPropertyName("mod_versions")]
        public Dictionary<string, string> ModVersions { get; set; } = new Dictionary<string, string>();
    }

    /// <summary>
    /// Plan 165 / DEC-160: Modding Support & Mod Data Contract.
    /// Manages community mod registrations, data contract manifest validation,
    /// version range governance, dependency resolution, conflict detection,
    /// deterministic load order, and safe save state preservation.
    /// </summary>
    public sealed class ModSupportSystem
    {
        private static readonly Regex ModIdRegex = new Regex("^[a-z0-9_\\-]+$", RegexOptions.Compiled);

        private readonly Dictionary<string, ModRegistration> _registeredMods = new Dictionary<string, ModRegistration>(StringComparer.OrdinalIgnoreCase);
        private ModManifestSpecificationDef _specification = new ModManifestSpecificationDef();

        public IReadOnlyDictionary<string, ModRegistration> RegisteredMods => _registeredMods;
        public ModManifestSpecificationDef Specification => _specification;

        public Action<ModRegistration>? OnModRegisteredSeam { get; set; }
        public Action<string, ModStatus>? OnModStatusChangedSeam { get; set; }
        public Action<ModConflict>? OnModConflictDetectedSeam { get; set; }

        public ModSupportSystem()
        {
            LoadFallbackSpecification();
        }

        public void LoadSpecification(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return;

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var spec = JsonSerializer.Deserialize<ModManifestSpecificationDef>(json, options);
                if (spec != null)
                {
                    _specification = spec;
                }
            }
            catch (JsonException)
            {
                // Retain fallback specification
            }
        }

        /// <summary>
        /// Plan 165 — binds an already-validated specification (the host uses the
        /// strict <see cref="ModManifestSpecificationLoader"/> and this seam), so
        /// the lenient fallback whitelist is unreachable from the host.
        /// </summary>
        public void BindSpecification(ModManifestSpecificationDef spec)
        {
            _specification = spec ?? throw new ArgumentNullException(nameof(spec));
        }

        /// <summary>
        /// Read-only census of live mod-support state (Plan 165). Exposed for the
        /// architecture scanner and the host self-test probe.
        /// </summary>
        public ModSupportCensus GetCensus()
        {
            int enabled = 0, disabled = 0, invalid = 0, missing = 0, cyclic = 0, incompatible = 0;
            foreach (var reg in _registeredMods.Values)
            {
                switch (reg.Status)
                {
                    case ModStatus.Enabled: enabled++; break;
                    case ModStatus.Disabled: disabled++; break;
                    case ModStatus.InvalidManifest: invalid++; break;
                    case ModStatus.MissingDependency: missing++; break;
                    case ModStatus.CyclicDependency: cyclic++; break;
                    case ModStatus.Incompatible: incompatible++; break;
                }
            }

            return new ModSupportCensus(
                _registeredMods.Count,
                enabled,
                disabled,
                invalid,
                missing,
                cyclic,
                incompatible,
                _specification.AllowedCatalogs?.Count ?? 0);
        }

        public ModContractValidationResult ValidateManifest(ModManifest? manifest)
        {
            var result = new ModContractValidationResult();
            if (manifest == null)
            {
                result.Errors.Add("Mod manifest is null");
                return result;
            }

            string modId = manifest.EffectiveModId;
            if (string.IsNullOrWhiteSpace(modId))
            {
                result.Errors.Add("mod_id is required");
            }
            else if (!ModIdRegex.IsMatch(modId))
            {
                result.Errors.Add($"mod_id '{modId}' must contain only lowercase alphanumeric characters, dashes, and underscores");
            }

            if (string.IsNullOrWhiteSpace(manifest.EffectiveDisplayName))
            {
                result.Errors.Add("display_name is required");
            }

            if (manifest.SchemaVersion != 1)
            {
                result.Errors.Add($"schema_version {manifest.SchemaVersion} is unsupported (expected 1)");
            }

            if (_specification.AllowedCatalogs.Count > 0 && manifest.EffectiveCatalogs != null)
            {
                foreach (var catalog in manifest.EffectiveCatalogs)
                {
                    string filename = System.IO.Path.GetFileName(catalog);
                    if (!_specification.AllowedCatalogs.Contains(filename, StringComparer.OrdinalIgnoreCase))
                    {
                        result.Errors.Add($"catalog '{catalog}' is not in the allowed catalog whitelist");
                    }
                }
            }

            return result;
        }

        public ModRegistration RegisterMod(
            ModManifest manifest,
            string currentGameVersion = "1.0.0",
            int currentContractVersion = 1)
        {
            var validation = ValidateManifest(manifest);
            string modId = manifest.EffectiveModId;

            var reg = new ModRegistration
            {
                ModId = modId,
                DisplayName = manifest.EffectiveDisplayName,
                Version = manifest.Version,
                LoadOrder = manifest.LoadOrder,
                AllowOverrides = manifest.AllowOverrides,
                Catalogs = new List<string>(manifest.EffectiveCatalogs),
                Dependencies = new List<string>(manifest.Dependencies),
                Manifest = manifest
            };

            if (!validation.IsValid)
            {
                reg.Status = ModStatus.InvalidManifest;
                reg.StatusReason = string.Join("; ", validation.Errors);
            }
            else if (!ModCompatibilityEvaluator.EvaluateGameVersion(currentGameVersion, manifest.EffectiveGameRange, out string? gameErr))
            {
                reg.Status = ModStatus.Incompatible;
                reg.StatusReason = gameErr ?? "Incompatible game version";
            }
            else if (!ModCompatibilityEvaluator.EvaluateModContractVersion(currentContractVersion, manifest.EffectiveModContractRange, out string? contractErr))
            {
                reg.Status = ModStatus.Incompatible;
                reg.StatusReason = contractErr ?? "Incompatible mod contract version";
            }
            else
            {
                reg.Status = ModStatus.Enabled;
                reg.StatusReason = "Validated successfully";
            }

            _registeredMods[modId] = reg;
            ReevaluateDependencies();
            OnModRegisteredSeam?.Invoke(reg);
            return reg;
        }

        public bool SetModEnabled(string modId, bool enabled)
        {
            if (!_registeredMods.TryGetValue(modId, out var reg))
                return false;

            if (reg.Status == ModStatus.InvalidManifest || reg.Status == ModStatus.Incompatible)
                return false;

            var oldStatus = reg.Status;
            reg.Status = enabled ? ModStatus.Enabled : ModStatus.Disabled;
            reg.StatusReason = enabled ? "User enabled" : "User disabled";

            if (oldStatus != reg.Status)
            {
                ReevaluateDependencies();
                OnModStatusChangedSeam?.Invoke(modId, reg.Status);
            }

            return true;
        }

        public void ReevaluateDependencies()
        {
            // Check missing or disabled dependencies for all enabled mods
            foreach (var kvp in _registeredMods)
            {
                var reg = kvp.Value;
                if (reg.Status != ModStatus.Enabled && reg.Status != ModStatus.MissingDependency && reg.Status != ModStatus.CyclicDependency)
                    continue;

                bool hasMissing = false;
                foreach (var depId in reg.Dependencies)
                {
                    if (!_registeredMods.TryGetValue(depId, out var depReg) || depReg.Status != ModStatus.Enabled)
                    {
                        reg.Status = ModStatus.MissingDependency;
                        reg.StatusReason = $"Requires missing or inactive mod '{depId}'";
                        hasMissing = true;
                        break;
                    }
                }

                if (!hasMissing && (reg.Status == ModStatus.MissingDependency || reg.Status == ModStatus.CyclicDependency))
                {
                    reg.Status = ModStatus.Enabled;
                    reg.StatusReason = "Dependencies satisfied";
                }
            }

            // Check cyclic dependencies via DFS
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var inStack = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var modId in _registeredMods.Keys)
            {
                if (DetectCycle(modId, visited, inStack, out var cycleNode))
                {
                    if (_registeredMods.TryGetValue(cycleNode, out var cyclicReg))
                    {
                        cyclicReg.Status = ModStatus.CyclicDependency;
                        cyclicReg.StatusReason = "Cyclic dependency detected in mod graph";
                    }
                }
            }
        }

        private bool DetectCycle(string node, HashSet<string> visited, HashSet<string> inStack, out string cycleNode)
        {
            cycleNode = string.Empty;
            if (inStack.Contains(node))
            {
                cycleNode = node;
                return true;
            }

            if (visited.Contains(node))
                return false;

            visited.Add(node);
            inStack.Add(node);

            if (_registeredMods.TryGetValue(node, out var reg))
            {
                foreach (var dep in reg.Dependencies)
                {
                    if (DetectCycle(dep, visited, inStack, out cycleNode))
                        return true;
                }
            }

            inStack.Remove(node);
            return false;
        }

        public IReadOnlyList<string> ResolveLoadOrder()
        {
            var activeMods = _registeredMods.Values
                .Where(r => r.Status == ModStatus.Enabled)
                .ToList();

            var order = new List<string>();
            var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            // Sort initial candidates by LoadOrder ascending, then ModId ordinal
            var sortedCandidates = activeMods
                .OrderBy(m => m.LoadOrder)
                .ThenBy(m => m.ModId, StringComparer.Ordinal)
                .ToList();

            void Visit(ModRegistration mod)
            {
                if (visited.Contains(mod.ModId))
                    return;

                visited.Add(mod.ModId);

                // Visit dependencies first
                foreach (var depId in mod.Dependencies)
                {
                    if (_registeredMods.TryGetValue(depId, out var depReg) && depReg.Status == ModStatus.Enabled)
                    {
                        Visit(depReg);
                    }
                }

                order.Add(mod.ModId);
            }

            foreach (var candidate in sortedCandidates)
            {
                Visit(candidate);
            }

            return order;
        }

        public IReadOnlyList<ModConflict> DetectConflicts()
        {
            var conflicts = new List<ModConflict>();
            var activeMods = _registeredMods.Values
                .Where(r => r.Status == ModStatus.Enabled)
                .ToList();

            // Track catalogs modified by mods without AllowOverrides
            var catalogMods = new Dictionary<string, List<ModRegistration>>(StringComparer.OrdinalIgnoreCase);
            foreach (var mod in activeMods)
            {
                foreach (var catalog in mod.Catalogs)
                {
                    string filename = System.IO.Path.GetFileName(catalog);
                    if (!catalogMods.TryGetValue(filename, out var list))
                    {
                        list = new List<ModRegistration>();
                        catalogMods[filename] = list;
                    }
                    list.Add(mod);
                }
            }

            foreach (var kvp in catalogMods)
            {
                var mods = kvp.Value;
                if (mods.Count > 1)
                {
                    // Check if multiple mods do not allow overrides
                    for (int i = 0; i < mods.Count; i++)
                    {
                        for (int j = i + 1; j < mods.Count; j++)
                        {
                            if (!mods[i].AllowOverrides && !mods[j].AllowOverrides)
                            {
                                var conflict = new ModConflict
                                {
                                    ModIdA = mods[i].ModId,
                                    ModIdB = mods[j].ModId,
                                    CatalogName = kvp.Key,
                                    Description = $"Mods '{mods[i].ModId}' and '{mods[j].ModId}' both modify '{kvp.Key}' without override permission"
                                };
                                conflicts.Add(conflict);
                                OnModConflictDetectedSeam?.Invoke(conflict);
                            }
                        }
                    }
                }
            }

            return conflicts;
        }

        public ModSaveState CaptureState()
        {
            return new ModSaveState
            {
                SchemaVersion = 1,
                ActiveModIds = _registeredMods.Values
                    .Where(r => r.Status == ModStatus.Enabled)
                    .Select(r => r.ModId)
                    .ToList(),
                DisabledModIds = _registeredMods.Values
                    .Where(r => r.Status == ModStatus.Disabled)
                    .Select(r => r.ModId)
                    .ToList(),
                ModVersions = _registeredMods.ToDictionary(k => k.Key, v => v.Value.Version)
            };
        }

        public void RestoreState(ModSaveState? state)
        {
            if (state == null)
                return;

            // Schema gate: a newer payload must not be silently down-cast.
            if (state.SchemaVersion > 1)
                throw new InvalidOperationException(
                    $"ModSaveState schema {state.SchemaVersion} is newer than supported (1).");
            if (state.SchemaVersion < 1)
                state.SchemaVersion = 1;

            if (state.DisabledModIds != null)
            {
                foreach (var id in state.DisabledModIds)
                {
                    if (_registeredMods.TryGetValue(id, out var reg) && reg.Status == ModStatus.Enabled)
                    {
                        reg.Status = ModStatus.Disabled;
                        reg.StatusReason = "Restored as disabled from save";
                    }
                }
            }

            if (state.ActiveModIds != null)
            {
                foreach (var id in state.ActiveModIds)
                {
                    if (_registeredMods.TryGetValue(id, out var reg) && reg.Status == ModStatus.Disabled)
                    {
                        reg.Status = ModStatus.Enabled;
                        reg.StatusReason = "Restored as active from save";
                    }
                }
            }

            ReevaluateDependencies();
        }

        private void LoadFallbackSpecification()
        {
            _specification = new ModManifestSpecificationDef
            {
                SchemaVersion = 1,
                SchemaName = "mod_manifest_specification",
                CurrentModContractVersion = 1,
                SupportedGameVersions = ">=1.0.0 <3.0.0",
                RequiredManifestFields = new List<string> { "schema_version", "mod_id", "display_name", "version", "load_order" },
                OptionalManifestFields = new List<string> { "allow_overrides", "catalogs", "game_range", "mod_contract_range", "pack_type", "dependencies" },
                AllowedCatalogs = new List<string>
                {
                    "items.json",
                    "locations.json",
                    "quests.json",
                    "factions.json",
                    "weather_effects.json",
                    "colony_blueprints.json",
                    "hobby_definitions.json",
                    "map_regions.json",
                    "nuclear_winter_phases.json"
                }
            };
        }
    }

    /// <summary>
    /// Read-only census of live mod-support state (Plan 165). Exposed for the
    /// architecture scanner and the host self-test probe.
    /// </summary>
    public struct ModSupportCensus
    {
        public int TotalMods { get; }
        public int EnabledMods { get; }
        public int DisabledMods { get; }
        public int InvalidMods { get; }
        public int MissingDependencyMods { get; }
        public int CyclicMods { get; }
        public int IncompatibleMods { get; }
        public int AllowedCatalogCount { get; }

        public ModSupportCensus(
            int totalMods,
            int enabledMods,
            int disabledMods,
            int invalidMods,
            int missingDependencyMods,
            int cyclicMods,
            int incompatibleMods,
            int allowedCatalogCount)
        {
            TotalMods = totalMods;
            EnabledMods = enabledMods;
            DisabledMods = disabledMods;
            InvalidMods = invalidMods;
            MissingDependencyMods = missingDependencyMods;
            CyclicMods = cyclicMods;
            IncompatibleMods = incompatibleMods;
            AllowedCatalogCount = allowedCatalogCount;
        }
    }
}
