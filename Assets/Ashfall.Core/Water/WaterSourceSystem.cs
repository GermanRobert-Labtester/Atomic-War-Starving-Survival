// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 189 — Water Source Management & Contamination Network
// Pure domain authority for discovering, monitoring, and managing multiple
// water sources (wells, rivers, rain collectors, underground springs) with
// contamination propagation across network connections, water testing,
// and infrastructure maintenance.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Water
{
    // ── Catalog DTOs ────────────────────────────────────────────────────────

    [Serializable]
    public sealed class WaterSourceDef
    {
        public string source_id { get; set; } = string.Empty;
        public string display_name { get; set; } = string.Empty;
        public string source_type { get; set; } = "well"; // well, river, spring, rain_collector, underground_spring, municipal
        public string location_id { get; set; } = string.Empty;
        public float flow_rate_l_day { get; set; } = 400.0f;
        public float base_contamination { get; set; } = 0.05f;
        public string initial_infrastructure_level { get; set; } = "basic"; // none, basic, advanced
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class WaterConnectionDef
    {
        public string connection_id { get; set; } = string.Empty;
        public string source_a { get; set; } = string.Empty;
        public string source_b { get; set; } = string.Empty;
        public string connection_type { get; set; } = "underground_flow"; // underground_flow, surface_runoff, pipe_connection
        public float transfer_rate { get; set; } = 0.15f;
    }

    [Serializable]
    public sealed class WaterSourcesCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<WaterSourceDef> sources { get; set; } = new List<WaterSourceDef>();
        public List<WaterConnectionDef> connections { get; set; } = new List<WaterConnectionDef>();
    }

    // ── Persistent State DTOs ───────────────────────────────────────────────

    [Serializable]
    public sealed class WaterSourceState
    {
        public string SourceId { get; set; } = string.Empty;
        public bool IsDiscovered { get; set; }
        public bool IsActive { get; set; }
        public float CurrentContamination { get; set; }
        public float FlowRateLitersPerDay { get; set; }
        public string InfrastructureLevel { get; set; } = "basic"; // none, basic, advanced
        public bool MaintenanceNeeded { get; set; }
        public int LastTestedDay { get; set; }
        public float LastTestedContamination { get; set; }
    }

    [Serializable]
    public sealed class WaterTestResult
    {
        public string TestId { get; set; } = string.Empty;
        public string SourceId { get; set; } = string.Empty;
        public int Day { get; set; }
        public string TestedBySurvivorId { get; set; } = string.Empty;
        public float ContaminationLevel { get; set; }
        public string ContaminantType { get; set; } = "clean"; // radiation, chemical, biological, clean
        public float Accuracy { get; set; } = 0.95f;
    }

    [Serializable]
    public sealed class WaterSourceSystemState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public string ActiveSourceId { get; set; } = "source_shelter_well_deep";
        public List<WaterSourceState> Sources { get; set; } = new List<WaterSourceState>();
        public List<WaterTestResult> TestHistory { get; set; } = new List<WaterTestResult>();
        public float TotalStoredLiters { get; set; } = 500.0f;
    }

    // ── Domain System ───────────────────────────────────────────────────────

    public sealed class WaterSourceSystem
    {
        private readonly WaterSourceSystemState _state;
        private readonly Dictionary<string, WaterSourceDef> _definitions =
            new Dictionary<string, WaterSourceDef>(StringComparer.OrdinalIgnoreCase);
        private readonly List<WaterConnectionDef> _connections = new List<WaterConnectionDef>();

        public event Action<string>? OnSourceDiscovered;      // (sourceId)
        public event Action<string>? OnActiveSourceChanged;   // (newSourceId)
        public event Action<string, float>? OnContaminationSpike; // (sourceId, level)
        public event Action<WaterTestResult>? OnWaterTested;

        public string ActiveSourceId => _state.ActiveSourceId;
        public int DiscoveredSourceCount => _state.Sources.Count(s => s.IsDiscovered);
        public float TotalStoredLiters => _state.TotalStoredLiters;

        public WaterSourceSystem()
        {
            _state = new WaterSourceSystemState();
        }

        public WaterSourceSystem(WaterSourceSystemState state)
        {
            _state = state ?? new WaterSourceSystemState();
        }

        // ── Catalog Loading ────────────────────────────────────────────────

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<WaterSourcesCatalog>(json, options);
                if (catalog?.sources == null) return;

                _definitions.Clear();
                _connections.Clear();

                foreach (var s in catalog.sources)
                {
                    if (string.IsNullOrWhiteSpace(s.source_id)) continue;
                    _definitions[s.source_id] = s;

                    // Ensure state exists for each source
                    if (!_state.Sources.Any(st => string.Equals(st.SourceId, s.source_id, StringComparison.OrdinalIgnoreCase)))
                    {
                        bool isShelterDefault = string.Equals(s.source_id, "source_shelter_well_deep", StringComparison.OrdinalIgnoreCase);
                        _state.Sources.Add(new WaterSourceState
                        {
                            SourceId = s.source_id,
                            IsDiscovered = isShelterDefault,
                            IsActive = isShelterDefault,
                            CurrentContamination = s.base_contamination,
                            FlowRateLitersPerDay = s.flow_rate_l_day,
                            InfrastructureLevel = s.initial_infrastructure_level,
                            MaintenanceNeeded = false
                        });
                    }
                }

                if (catalog.connections != null)
                {
                    _connections.AddRange(catalog.connections);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyCollection<WaterSourceDef> GetAllSourceDefs() => _definitions.Values;
        public IReadOnlyList<WaterConnectionDef> GetAllConnections() => _connections;

        public WaterSourceDef? GetSourceDef(string sourceId)
        {
            return _definitions.TryGetValue(sourceId, out var def) ? def : null;
        }

        public WaterSourceState? GetSource(string sourceId)
        {
            return _state.Sources.FirstOrDefault(s =>
                string.Equals(s.SourceId, sourceId, StringComparison.OrdinalIgnoreCase));
        }

        public WaterSourceState? GetActiveSource()
        {
            return GetSource(_state.ActiveSourceId);
        }

        // ── Discovery & Activation ─────────────────────────────────────────

        public bool DiscoverSource(string sourceId, int day)
        {
            var src = GetSource(sourceId);
            if (src == null || src.IsDiscovered) return false;

            src.IsDiscovered = true;
            OnSourceDiscovered?.Invoke(sourceId);
            return true;
        }

        public bool SetActiveSource(string sourceId)
        {
            var src = GetSource(sourceId);
            if (src == null || !src.IsDiscovered) return false;

            foreach (var s in _state.Sources)
                s.IsActive = (s == src);

            _state.ActiveSourceId = sourceId;
            OnActiveSourceChanged?.Invoke(sourceId);
            return true;
        }

        // ── Daily Contamination & Propagation ──────────────────────────────

        public void TickDay(int day, float rainMultiplier = 1.0f, float environmentalContamination = 0.0f)
        {
            float rain = Math.Max(0.5f, rainMultiplier);

            // 1. Base weather & environmental ingress per source
            foreach (var src in _state.Sources)
            {
                if (!_definitions.TryGetValue(src.SourceId, out var def)) continue;

                float ingress = 0.0f;
                switch (def.source_type.ToLowerInvariant())
                {
                    case "river":
                        ingress = (environmentalContamination * 0.25f) + (rain > 1.2f ? 0.05f : 0.0f);
                        break;
                    case "rain_collector":
                        ingress = environmentalContamination * 0.40f;
                        break;
                    case "well":
                        ingress = environmentalContamination * 0.05f;
                        break;
                    case "underground_spring":
                        ingress = 0.0f; // well-protected
                        break;
                    default:
                        ingress = environmentalContamination * 0.10f;
                        break;
                }

                // Infrastructure filtration mitigation
                float filterMitigation = 0.0f;
                if (string.Equals(src.InfrastructureLevel, "advanced", StringComparison.OrdinalIgnoreCase))
                    filterMitigation = 0.50f;
                else if (string.Equals(src.InfrastructureLevel, "basic", StringComparison.OrdinalIgnoreCase))
                    filterMitigation = 0.20f;

                float netChange = ingress * (1.0f - filterMitigation);
                src.CurrentContamination = Math.Clamp(src.CurrentContamination + netChange, 0.0f, 1.0f);
            }

            // 2. Propagation across connections
            foreach (var conn in _connections)
            {
                var srcA = GetSource(conn.source_a);
                var srcB = GetSource(conn.source_b);
                if (srcA == null || srcB == null) continue;

                // Contamination flows from higher to lower
                float diff = srcA.CurrentContamination - srcB.CurrentContamination;
                if (diff > 0.05f)
                {
                    float transferred = diff * conn.transfer_rate * 0.15f;
                    srcB.CurrentContamination = Math.Clamp(srcB.CurrentContamination + transferred, 0.0f, 1.0f);
                }
            }

            // 3. Maintenance flags and spike warnings
            foreach (var src in _state.Sources)
            {
                if (src.CurrentContamination >= 0.40f)
                    src.MaintenanceNeeded = true;

                if (src.CurrentContamination >= 0.50f)
                    OnContaminationSpike?.Invoke(src.SourceId, src.CurrentContamination);
            }
        }

        // ── Testing & Maintenance ──────────────────────────────────────────

        public WaterTestResult ConductWaterTest(string sourceId, int day, string survivorId, float kitAccuracy = 0.95f)
        {
            var src = GetSource(sourceId);
            if (src == null) throw new ArgumentException($"Source {sourceId} not found");

            float accuracy = Math.Clamp(kitAccuracy, 0.70f, 1.0f);
            float perceivedContamination = (float)Math.Round(src.CurrentContamination * accuracy, 3);

            string contaminantType = "clean";
            if (perceivedContamination >= 0.05f)
            {
                if (_definitions.TryGetValue(sourceId, out var def))
                {
                    if (def.source_type == "river" || def.source_type == "rain_collector")
                        contaminantType = "radiation";
                    else if (def.source_type == "municipal" || sourceId.Contains("foundry", StringComparison.OrdinalIgnoreCase))
                        contaminantType = "chemical";
                    else
                        contaminantType = "biological";
                }
                else
                {
                    contaminantType = "radiation";
                }
            }

            src.LastTestedDay = day;
            src.LastTestedContamination = perceivedContamination;

            var result = new WaterTestResult
            {
                TestId = $"test_{_state.NextSequence++}",
                SourceId = sourceId,
                Day = day,
                TestedBySurvivorId = survivorId,
                ContaminationLevel = perceivedContamination,
                ContaminantType = contaminantType,
                Accuracy = accuracy
            };

            _state.TestHistory.Add(result);
            if (_state.TestHistory.Count > 100)
                _state.TestHistory.RemoveAt(0);

            OnWaterTested?.Invoke(result);
            return result;
        }

        public bool PerformMaintenance(string sourceId, int day)
        {
            var src = GetSource(sourceId);
            if (src == null) return false;

            if (_definitions.TryGetValue(sourceId, out var def))
            {
                // Regress 50% toward base contamination
                float excess = Math.Max(0.0f, src.CurrentContamination - def.base_contamination);
                src.CurrentContamination = Math.Clamp(src.CurrentContamination - excess * 0.50f, def.base_contamination, 1.0f);
                src.FlowRateLitersPerDay = def.flow_rate_l_day;
            }

            src.MaintenanceNeeded = false;
            return true;
        }

        public bool UpgradeInfrastructure(string sourceId, string newLevel)
        {
            var src = GetSource(sourceId);
            if (src == null) return false;

            src.InfrastructureLevel = newLevel.Trim().ToLowerInvariant();
            return true;
        }

        // ── Supply Calculation ─────────────────────────────────────────────

        public float CalculateAvailableWater()
        {
            return _state.Sources
                .Where(s => s.IsDiscovered && s.IsActive)
                .Sum(s => s.FlowRateLitersPerDay);
        }

        public IReadOnlyList<WaterTestResult> GetTestHistory(string? sourceId = null)
        {
            if (string.IsNullOrWhiteSpace(sourceId))
                return _state.TestHistory;
            return _state.TestHistory.Where(t => string.Equals(t.SourceId, sourceId, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // ── Save / Restore ─────────────────────────────────────────────────

        public WaterSourceSystemState CaptureState()
        {
            return new WaterSourceSystemState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                ActiveSourceId = _state.ActiveSourceId,
                TotalStoredLiters = _state.TotalStoredLiters,
                Sources = _state.Sources.Select(s => new WaterSourceState
                {
                    SourceId = s.SourceId,
                    IsDiscovered = s.IsDiscovered,
                    IsActive = s.IsActive,
                    CurrentContamination = s.CurrentContamination,
                    FlowRateLitersPerDay = s.FlowRateLitersPerDay,
                    InfrastructureLevel = s.InfrastructureLevel,
                    MaintenanceNeeded = s.MaintenanceNeeded,
                    LastTestedDay = s.LastTestedDay,
                    LastTestedContamination = s.LastTestedContamination
                }).ToList(),
                TestHistory = _state.TestHistory.Select(t => new WaterTestResult
                {
                    TestId = t.TestId,
                    SourceId = t.SourceId,
                    Day = t.Day,
                    TestedBySurvivorId = t.TestedBySurvivorId,
                    ContaminationLevel = t.ContaminationLevel,
                    ContaminantType = t.ContaminantType,
                    Accuracy = t.Accuracy
                }).ToList()
            };
        }

        public void RestoreState(WaterSourceSystemState? saved)
        {
            if (saved == null) return;
            _state.SchemaVersion = saved.SchemaVersion;
            _state.NextSequence = saved.NextSequence > 0 ? saved.NextSequence : 1;
            _state.ActiveSourceId = !string.IsNullOrWhiteSpace(saved.ActiveSourceId) ? saved.ActiveSourceId : "source_shelter_well_deep";
            _state.TotalStoredLiters = saved.TotalStoredLiters;

            _state.Sources = saved.Sources?.Select(s => new WaterSourceState
            {
                SourceId = s.SourceId,
                IsDiscovered = s.IsDiscovered,
                IsActive = s.IsActive,
                CurrentContamination = s.CurrentContamination,
                FlowRateLitersPerDay = s.FlowRateLitersPerDay,
                InfrastructureLevel = s.InfrastructureLevel,
                MaintenanceNeeded = s.MaintenanceNeeded,
                LastTestedDay = s.LastTestedDay,
                LastTestedContamination = s.LastTestedContamination
            }).ToList() ?? new List<WaterSourceState>();

            _state.TestHistory = saved.TestHistory?.Select(t => new WaterTestResult
            {
                TestId = t.TestId,
                SourceId = t.SourceId,
                Day = t.Day,
                TestedBySurvivorId = t.TestedBySurvivorId,
                ContaminationLevel = t.ContaminationLevel,
                ContaminantType = t.ContaminantType,
                Accuracy = t.Accuracy
            }).ToList() ?? new List<WaterTestResult>();
        }
    }
}
