// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ashfall.Core.Vehicles
{
    public sealed class VehicleModule
    {
        public string ModuleId { get; set; } = string.Empty;
        public string ModuleType { get; set; } = "utility";
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public float DefenseBonus { get; set; }
        public float SpeedModifier { get; set; }
        public float CargoBonus { get; set; }
        public int BunkCapacity { get; set; }
        public int InstallationDays { get; set; } = 1;
        public int ScrapCost { get; set; } = 20;
        public int ComponentsCost { get; set; } = 10;
    }

    public sealed class VehicleCustomizationCatalog
    {
        private readonly Dictionary<string, VehicleModule> _modules = new Dictionary<string, VehicleModule>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<VehicleModule> AllModules => _modules.Values;

        public bool TryGetModule(string moduleId, out VehicleModule module)
        {
            return _modules.TryGetValue(moduleId, out module!);
        }

        public static VehicleCustomizationCatalog LoadFromJson(string json)
        {
            var catalog = new VehicleCustomizationCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            int arrStart = json.IndexOf("\"modules\"", StringComparison.Ordinal);
            if (arrStart < 0) return catalog;
            arrStart = json.IndexOf('[', arrStart);
            if (arrStart < 0) return catalog;
            int arrEnd = json.LastIndexOf(']');
            if (arrEnd <= arrStart) return catalog;

            string arrayText = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
            int idx = 0;
            while (idx < arrayText.Length)
            {
                int objStart = arrayText.IndexOf('{', idx);
                if (objStart < 0) break;
                int depth = 0;
                int objEnd = -1;
                for (int i = objStart; i < arrayText.Length; i++)
                {
                    if (arrayText[i] == '{') depth++;
                    else if (arrayText[i] == '}')
                    {
                        depth--;
                        if (depth == 0) { objEnd = i; break; }
                    }
                }
                if (objEnd < 0) break;

                string objText = arrayText.Substring(objStart, objEnd - objStart + 1);
                var mod = new VehicleModule
                {
                    ModuleId = ExtractString(objText, "module_id"),
                    ModuleType = ExtractString(objText, "module_type"),
                    Name = ExtractString(objText, "name"),
                    Description = ExtractString(objText, "description"),
                    DefenseBonus = ExtractFloat(objText, "defense_bonus", 0f),
                    SpeedModifier = ExtractFloat(objText, "speed_modifier", 0f),
                    CargoBonus = ExtractFloat(objText, "cargo_bonus", 0f),
                    BunkCapacity = ExtractInt(objText, "bunk_capacity", 0),
                    InstallationDays = ExtractInt(objText, "installation_days", 1),
                    ScrapCost = ExtractInt(objText, "scrap_cost", 20),
                    ComponentsCost = ExtractInt(objText, "components_cost", 10)
                };

                if (!string.IsNullOrEmpty(mod.ModuleId))
                {
                    catalog._modules[mod.ModuleId] = mod;
                }

                idx = objEnd + 1;
            }

            return catalog;
        }

        private static string ExtractString(string json, string key)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return string.Empty;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return string.Empty;
            int quoteStart = json.IndexOf('"', colon + 1);
            if (quoteStart < 0) return string.Empty;
            int quoteEnd = json.IndexOf('"', quoteStart + 1);
            if (quoteEnd < 0) return string.Empty;
            return json.Substring(quoteStart + 1, quoteEnd - quoteStart - 1).Trim();
        }

        private static int ExtractInt(string json, string key, int defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '-')) end++;
            if (end > start && int.TryParse(json.Substring(start, end - start), out int val))
            {
                return val;
            }
            return defaultValue;
        }

        private static float ExtractFloat(string json, string key, float defaultValue)
        {
            string pattern = $"\"{key}\"";
            int keyIdx = json.IndexOf(pattern, StringComparison.Ordinal);
            if (keyIdx < 0) return defaultValue;
            int colon = json.IndexOf(':', keyIdx + pattern.Length);
            if (colon < 0) return defaultValue;
            int start = colon + 1;
            while (start < json.Length && (char.IsWhiteSpace(json[start]) || json[start] == '"')) start++;
            int end = start;
            while (end < json.Length && (char.IsDigit(json[end]) || json[end] == '.' || json[end] == '-')) end++;
            if (end > start && float.TryParse(json.Substring(start, end - start), NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
            {
                return val;
            }
            return defaultValue;
        }
    }

    public sealed class VehicleCustomizationState
    {
        public int SchemaVersion { get; set; } = 1;
        public Dictionary<string, List<string>> VehicleModules { get; set; } = new Dictionary<string, List<string>>(StringComparer.OrdinalIgnoreCase);
        public Dictionary<string, string> DeployedBaseCamps { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
    }

    /// <summary>
    /// Plan 152: Vehicle Customization & Mobile Base System.
    /// Manages vehicle modular upgrades (armor, cargo, living, weapons, utility),
    /// mobile base deployments, field shelter sleeping capacities, and state persistence.
    /// Pure domain engine, zero engine references.
    /// </summary>
    public sealed class VehicleCustomizationSystem
    {
        public delegate void ModuleInstalledDelegate(string vehicleId, string moduleId);
        public delegate void ModuleRemovedDelegate(string vehicleId, string moduleId);
        public delegate void BaseCampDeployedDelegate(string vehicleId, string locationId);
        public delegate void BaseCampPackedDelegate(string vehicleId);

        public event ModuleInstalledDelegate? OnModuleInstalledSeam;
        public event ModuleRemovedDelegate? OnModuleRemovedSeam;
        public event BaseCampDeployedDelegate? OnBaseCampDeployedSeam;
        public event BaseCampPackedDelegate? OnBaseCampPackedSeam;

        private readonly VehicleCustomizationCatalog _catalog;
        private VehicleCustomizationState _state;

        public VehicleCustomizationCatalog Catalog => _catalog;
        public VehicleCustomizationState State => _state;

        public VehicleCustomizationSystem(VehicleCustomizationCatalog catalog, VehicleCustomizationState? state = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _state = state ?? new VehicleCustomizationState();
        }

        public bool InstallModule(string vehicleId, string moduleId, int maxSlots = 4)
        {
            if (string.IsNullOrWhiteSpace(vehicleId) || string.IsNullOrWhiteSpace(moduleId))
                return false;

            if (!_catalog.TryGetModule(moduleId, out _))
                return false;

            if (!_state.VehicleModules.TryGetValue(vehicleId, out var installed))
            {
                installed = new List<string>();
                _state.VehicleModules[vehicleId] = installed;
            }

            if (installed.Contains(moduleId, StringComparer.OrdinalIgnoreCase))
                return false;

            if (installed.Count >= maxSlots)
                return false;

            installed.Add(moduleId);
            OnModuleInstalledSeam?.Invoke(vehicleId, moduleId);
            return true;
        }

        public bool RemoveModule(string vehicleId, string moduleId)
        {
            if (!_state.VehicleModules.TryGetValue(vehicleId, out var installed))
                return false;

            int idx = installed.FindIndex(m => string.Equals(m, moduleId, StringComparison.OrdinalIgnoreCase));
            if (idx >= 0)
            {
                installed.RemoveAt(idx);
                OnModuleRemovedSeam?.Invoke(vehicleId, moduleId);
                return true;
            }

            return false;
        }

        public IReadOnlyList<string> GetInstalledModules(string vehicleId)
        {
            if (_state.VehicleModules.TryGetValue(vehicleId, out var list))
            {
                return list;
            }
            return Array.Empty<string>();
        }

        public (float speedMult, float cargoCapacity, float defense) CalculateEffectiveStats(
            string vehicleId,
            float baseSpeedMult = 1.0f,
            float baseCargoCapacity = 100.0f,
            float baseDefense = 0.0f)
        {
            float speed = baseSpeedMult;
            float cargo = baseCargoCapacity;
            float defense = baseDefense;

            var modules = GetInstalledModules(vehicleId);
            foreach (var modId in modules)
            {
                if (_catalog.TryGetModule(modId, out var mod))
                {
                    speed += mod.SpeedModifier;
                    cargo += mod.CargoBonus;
                    defense += mod.DefenseBonus;
                }
            }

            speed = Math.Max(0.20f, speed);
            cargo = Math.Max(10.0f, cargo);
            defense = Math.Max(0.0f, defense);

            return (speed, cargo, defense);
        }

        public bool IsMobileBaseCapable(string vehicleId)
        {
            var modules = GetInstalledModules(vehicleId);
            foreach (var modId in modules)
            {
                if (_catalog.TryGetModule(modId, out var mod))
                {
                    if (string.Equals(mod.ModuleType, "living", StringComparison.OrdinalIgnoreCase))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public int GetBunkCapacity(string vehicleId)
        {
            int total = 0;
            var modules = GetInstalledModules(vehicleId);
            foreach (var modId in modules)
            {
                if (_catalog.TryGetModule(modId, out var mod))
                {
                    total += mod.BunkCapacity;
                }
            }
            return total;
        }

        public bool DeployBaseCamp(string vehicleId, string locationId)
        {
            if (!IsMobileBaseCapable(vehicleId))
                return false;

            if (_state.DeployedBaseCamps.ContainsKey(vehicleId))
                return false;

            _state.DeployedBaseCamps[vehicleId] = locationId;
            OnBaseCampDeployedSeam?.Invoke(vehicleId, locationId);
            return true;
        }

        public bool PackBaseCamp(string vehicleId)
        {
            if (_state.DeployedBaseCamps.Remove(vehicleId))
            {
                OnBaseCampPackedSeam?.Invoke(vehicleId);
                return true;
            }
            return false;
        }

        public bool IsBaseCampDeployed(string vehicleId)
        {
            return _state.DeployedBaseCamps.ContainsKey(vehicleId);
        }

        public string? GetBaseCampLocation(string vehicleId)
        {
            return _state.DeployedBaseCamps.TryGetValue(vehicleId, out var loc) ? loc : null;
        }

        public int RestSurvivorsInVehicle(string vehicleId, IReadOnlyList<string> survivorIds)
        {
            int bunkCapacity = GetBunkCapacity(vehicleId);
            if (bunkCapacity <= 0 || survivorIds == null || survivorIds.Count == 0)
                return 0;

            int restedCount = Math.Min(bunkCapacity, survivorIds.Count);
            return restedCount;
        }

        public string CaptureState()
        {
            var vEntries = new List<string>();
            foreach (var kvp in _state.VehicleModules)
            {
                string modList = string.Join(",", kvp.Value.Select(m => $"\"{m}\""));
                vEntries.Add($"\"{kvp.Key}\":[{modList}]");
            }

            var cEntries = new List<string>();
            foreach (var kvp in _state.DeployedBaseCamps)
            {
                cEntries.Add($"\"{kvp.Key}\":\"{kvp.Value}\"");
            }

            return $"{{\"schema_version\":{_state.SchemaVersion},\"vehicle_modules\":{{{string.Join(",", vEntries)}}},\"base_camps\":{{{string.Join(",", cEntries)}}}}}";
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            var newState = new VehicleCustomizationState();

            // Extract schema_version
            int svIdx = json.IndexOf("\"schema_version\"", StringComparison.Ordinal);
            if (svIdx >= 0)
            {
                int colon = json.IndexOf(':', svIdx);
                int end = colon + 1;
                while (end < json.Length && (char.IsDigit(json[end]) || char.IsWhiteSpace(json[end]))) end++;
                if (int.TryParse(json.Substring(colon + 1, end - colon - 1).Trim(), out int sv))
                {
                    newState.SchemaVersion = sv;
                }
            }

            // Extract vehicle_modules object
            int vmIdx = json.IndexOf("\"vehicle_modules\"", StringComparison.Ordinal);
            if (vmIdx >= 0)
            {
                int startObj = json.IndexOf('{', vmIdx);
                int endObj = json.IndexOf('}', startObj);
                if (startObj >= 0 && endObj > startObj)
                {
                    string inner = json.Substring(startObj + 1, endObj - startObj - 1);
                    int idx = 0;
                    while (idx < inner.Length)
                    {
                        int qStart = inner.IndexOf('"', idx);
                        if (qStart < 0) break;
                        int qEnd = inner.IndexOf('"', qStart + 1);
                        if (qEnd < 0) break;
                        string vId = inner.Substring(qStart + 1, qEnd - qStart - 1);

                        int arrStart = inner.IndexOf('[', qEnd);
                        if (arrStart < 0) break;
                        int arrEnd = inner.IndexOf(']', arrStart);
                        if (arrEnd < 0) break;

                        string arrContent = inner.Substring(arrStart + 1, arrEnd - arrStart - 1);
                        var mods = new List<string>();
                        foreach (var m in arrContent.Split(','))
                        {
                            string cleaned = m.Trim().Trim('"');
                            if (!string.IsNullOrEmpty(cleaned)) mods.Add(cleaned);
                        }

                        newState.VehicleModules[vId] = mods;
                        idx = arrEnd + 1;
                    }
                }
            }

            // Extract base_camps object
            int bcIdx = json.IndexOf("\"base_camps\"", StringComparison.Ordinal);
            if (bcIdx >= 0)
            {
                int startObj = json.IndexOf('{', bcIdx);
                int endObj = json.IndexOf('}', startObj);
                if (startObj >= 0 && endObj > startObj)
                {
                    string inner = json.Substring(startObj + 1, endObj - startObj - 1);
                    foreach (var pair in inner.Split(','))
                    {
                        int colon = pair.IndexOf(':');
                        if (colon > 0)
                        {
                            string k = pair.Substring(0, colon).Trim().Trim('"');
                            string v = pair.Substring(colon + 1).Trim().Trim('"');
                            if (!string.IsNullOrEmpty(k) && !string.IsNullOrEmpty(v))
                            {
                                newState.DeployedBaseCamps[k] = v;
                            }
                        }
                    }
                }
            }

            _state = newState;
        }
    }
}
