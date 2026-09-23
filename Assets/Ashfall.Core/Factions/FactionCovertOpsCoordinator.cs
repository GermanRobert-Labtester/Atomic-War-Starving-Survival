// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Ashfall.Core.Random;

namespace Ashfall.Core.Factions
{
    public enum CovertOperationStatus
    {
        Planned = 0,
        Active = 1,
        Succeeded = 2,
        Failed = 3,
        Compromised = 4
    }

    public enum SuspicionBand
    {
        Calm = 0,
        Cautious = 1,
        Hostile = 2,
        Lockdown = 3
    }

    public sealed class EspionageOperationDef
    {
        public string OperationId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string OperationType { get; set; } = "infiltrate"; // infiltrate, steal, sabotage, propaganda, assassinate
        public string TargetFactionId { get; set; } = string.Empty;
        public float BaseSuccessRate { get; set; } = 0.60f;
        public string RiskLevel { get; set; } = "medium";
        public int BaseDurationDays { get; set; } = 7;
        public int SuspicionGain { get; set; } = 15;
        public string IntelType { get; set; } = "military";
        public int IntelValue { get; set; } = 40;
        public string Description { get; set; } = string.Empty;
    }

    public sealed class ActiveCovertOperation
    {
        public string OperationId { get; set; } = string.Empty;
        public string OperationType { get; set; } = string.Empty;
        public string TargetFactionId { get; set; } = string.Empty;
        public string AssignedAgentId { get; set; } = string.Empty;
        public int StartDay { get; set; } = 1;
        public int DurationDays { get; set; } = 7;
        public int RemainingDays { get; set; } = 7;
        public CovertOperationStatus Status { get; set; } = CovertOperationStatus.Active;
        public float SuccessChance { get; set; } = 0.60f;
    }

    public sealed class IntelligenceReport
    {
        public string ReportId { get; set; } = string.Empty;
        public string SourceFactionId { get; set; } = string.Empty;
        public string IntelligenceType { get; set; } = "military";
        public int Value { get; set; } = 40;
        public float Accuracy { get; set; } = 0.85f;
        public int DayObtained { get; set; } = 1;
        public bool IsDecoded { get; set; } = false;
        public string Summary { get; set; } = string.Empty;
    }

    public sealed class CovertOpsCatalog
    {
        private readonly Dictionary<string, EspionageOperationDef> _operations = new Dictionary<string, EspionageOperationDef>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<EspionageOperationDef> AllOperations => _operations.Values;
        public float DailyCoolingRate { get; set; } = 2.0f;

        public bool TryGetOperation(string operationId, out EspionageOperationDef op)
        {
            return _operations.TryGetValue(operationId, out op!);
        }

        public static CovertOpsCatalog LoadFromJson(string json)
        {
            var catalog = new CovertOpsCatalog();
            if (string.IsNullOrWhiteSpace(json)) return catalog;

            int arrStart = json.IndexOf("\"operations\"", StringComparison.Ordinal);
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

                string obj = arrayText.Substring(objStart, objEnd - objStart + 1);
                var def = new EspionageOperationDef
                {
                    OperationId = ExtractString(obj, "operation_id"),
                    Name = ExtractString(obj, "name"),
                    OperationType = ExtractString(obj, "operation_type"),
                    TargetFactionId = ExtractString(obj, "target_faction_id"),
                    BaseSuccessRate = ExtractFloat(obj, "base_success_rate", 0.60f),
                    RiskLevel = ExtractString(obj, "risk_level"),
                    BaseDurationDays = ExtractInt(obj, "base_duration_days", 7),
                    SuspicionGain = ExtractInt(obj, "suspicion_gain", 15),
                    IntelType = ExtractString(obj, "intel_type"),
                    IntelValue = ExtractInt(obj, "intel_value", 40),
                    Description = ExtractString(obj, "description")
                };

                if (!string.IsNullOrEmpty(def.OperationId))
                {
                    catalog._operations[def.OperationId] = def;
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

    /// <summary>
    /// Plan 153: Faction Espionage, Infiltration & Counter-Intelligence System.
    /// Manages player-directed covert operations, intelligence theft, infiltration,
    /// faction suspicion levels, report decoding, and agent compromise risks.
    /// Pure domain engine, zero engine references.
    /// </summary>
    public sealed class FactionCovertOpsCoordinator
    {
        public delegate void OperationLaunchedDelegate(string operationId, string agentId, string targetFaction);
        public delegate void OperationResolvedDelegate(string operationId, string agentId, CovertOperationStatus status);
        public delegate void IntelligenceDecodedDelegate(string reportId, string factionId, string intelType, int value);
        public delegate void SuspicionChangedDelegate(string factionId, float newSuspicion, SuspicionBand band);
        public delegate void AgentCompromisedDelegate(string agentId, string factionId, string reason);

        public event OperationLaunchedDelegate? OnCovertOperationLaunchedSeam;
        public event OperationResolvedDelegate? OnCovertOperationResolvedSeam;
        public event IntelligenceDecodedDelegate? OnIntelligenceDecodedSeam;
        public event SuspicionChangedDelegate? OnFactionSuspicionChangedSeam;
        public event AgentCompromisedDelegate? OnAgentCompromisedSeam;

        private readonly CovertOpsCatalog _catalog;
        private readonly List<ActiveCovertOperation> _activeOperations = new List<ActiveCovertOperation>();
        private readonly List<IntelligenceReport> _reports = new List<IntelligenceReport>();
        private readonly Dictionary<string, float> _factionSuspicion = new Dictionary<string, float>(StringComparer.OrdinalIgnoreCase);

        public CovertOpsCatalog Catalog => _catalog;
        public IReadOnlyList<ActiveCovertOperation> ActiveOperations => _activeOperations;
        public IReadOnlyList<IntelligenceReport> Reports => _reports;

        public FactionCovertOpsCoordinator(CovertOpsCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public float GetSuspicion(string factionId)
        {
            return _factionSuspicion.TryGetValue(factionId, out float v) ? v : 0f;
        }

        public SuspicionBand GetSuspicionBand(string factionId)
        {
            float s = GetSuspicion(factionId);
            if (s >= 75f) return SuspicionBand.Lockdown;
            if (s >= 50f) return SuspicionBand.Hostile;
            if (s >= 25f) return SuspicionBand.Cautious;
            return SuspicionBand.Calm;
        }

        public void AddSuspicion(string factionId, float delta)
        {
            float cur = GetSuspicion(factionId);
            float next = Math.Clamp(cur + delta, 0f, 100f);
            _factionSuspicion[factionId] = next;
            OnFactionSuspicionChangedSeam?.Invoke(factionId, next, GetSuspicionBand(factionId));
        }

        public bool LaunchOperation(string operationId, string agentId, float agentStealthSkill = 1.0f, int currentDay = 1)
        {
            if (string.IsNullOrWhiteSpace(operationId) || string.IsNullOrWhiteSpace(agentId))
                return false;

            if (!_catalog.TryGetOperation(operationId, out var def))
                return false;

            // Cannot assign agent if already on an active operation
            if (_activeOperations.Exists(o => o.Status == CovertOperationStatus.Active && string.Equals(o.AssignedAgentId, agentId, StringComparison.OrdinalIgnoreCase)))
                return false;

            // Security penalty from faction suspicion
            float suspicion = GetSuspicion(def.TargetFactionId);
            float securityPenalty = (suspicion / 100f) * 0.25f;

            float effectiveSuccessChance = Math.Clamp((def.BaseSuccessRate * agentStealthSkill) - securityPenalty, 0.10f, 0.95f);

            var op = new ActiveCovertOperation
            {
                OperationId = operationId,
                OperationType = def.OperationType,
                TargetFactionId = def.TargetFactionId,
                AssignedAgentId = agentId,
                StartDay = currentDay,
                DurationDays = def.BaseDurationDays,
                RemainingDays = def.BaseDurationDays,
                Status = CovertOperationStatus.Active,
                SuccessChance = effectiveSuccessChance
            };

            _activeOperations.Add(op);
            OnCovertOperationLaunchedSeam?.Invoke(operationId, agentId, def.TargetFactionId);
            return true;
        }

        public void AdvanceDay(ISeededRng rng, int currentDay)
        {
            // 1. Cool down suspicion for factions without active covert ops against them
            var targetedFactions = new HashSet<string>(_activeOperations.Where(o => o.Status == CovertOperationStatus.Active).Select(o => o.TargetFactionId), StringComparer.OrdinalIgnoreCase);
            var factions = _factionSuspicion.Keys.ToList();
            foreach (var f in factions)
            {
                if (!targetedFactions.Contains(f))
                {
                    AddSuspicion(f, -_catalog.DailyCoolingRate);
                }
            }

            // 2. Advance active operations
            for (int i = _activeOperations.Count - 1; i >= 0; i--)
            {
                var op = _activeOperations[i];
                if (op.Status != CovertOperationStatus.Active) continue;

                op.RemainingDays--;
                if (op.RemainingDays <= 0)
                {
                    ResolveOperation(op, rng, currentDay);
                }
            }
        }

        public void ResolveOperation(ActiveCovertOperation op, ISeededRng rng, int currentDay, bool forceSuccess = false, bool forceCompromise = false)
        {
            if (!_catalog.TryGetOperation(op.OperationId, out var def))
            {
                op.Status = CovertOperationStatus.Failed;
                return;
            }

            bool success = forceSuccess || (!forceCompromise && rng.NextFloat() <= op.SuccessChance);

            if (success)
            {
                op.Status = CovertOperationStatus.Succeeded;
                AddSuspicion(op.TargetFactionId, def.SuspicionGain * 0.5f);

                // Create raw intelligence report
                string reportId = $"intel_{op.TargetFactionId}_{_reports.Count + 1}";
                var report = new IntelligenceReport
                {
                    ReportId = reportId,
                    SourceFactionId = op.TargetFactionId,
                    IntelligenceType = def.IntelType,
                    Value = def.IntelValue,
                    Accuracy = 0.80f + (rng.NextFloat() * 0.15f),
                    DayObtained = currentDay,
                    IsDecoded = false,
                    Summary = $"Raw field intercept from {def.Name} targeting {op.TargetFactionId}."
                };
                _reports.Add(report);

                OnCovertOperationResolvedSeam?.Invoke(op.OperationId, op.AssignedAgentId, CovertOperationStatus.Succeeded);
            }
            else
            {
                // Check if compromised or simple failure
                bool compromised = forceCompromise || rng.NextFloat() < 0.40f;
                if (compromised)
                {
                    op.Status = CovertOperationStatus.Compromised;
                    AddSuspicion(op.TargetFactionId, def.SuspicionGain);
                    OnAgentCompromisedSeam?.Invoke(op.AssignedAgentId, op.TargetFactionId, $"Agent caught during {def.Name}");
                    OnCovertOperationResolvedSeam?.Invoke(op.OperationId, op.AssignedAgentId, CovertOperationStatus.Compromised);
                }
                else
                {
                    op.Status = CovertOperationStatus.Failed;
                    AddSuspicion(op.TargetFactionId, def.SuspicionGain * 0.25f);
                    OnCovertOperationResolvedSeam?.Invoke(op.OperationId, op.AssignedAgentId, CovertOperationStatus.Failed);
                }
            }
        }

        public bool DecodeIntelligenceReport(string reportId)
        {
            var report = _reports.FirstOrDefault(r => string.Equals(r.ReportId, reportId, StringComparison.OrdinalIgnoreCase));
            if (report == null || report.IsDecoded)
                return false;

            report.IsDecoded = true;
            OnIntelligenceDecodedSeam?.Invoke(report.ReportId, report.SourceFactionId, report.IntelligenceType, report.Value);
            return true;
        }

        public string CaptureState()
        {
            var opList = new List<string>();
            foreach (var op in _activeOperations)
            {
                opList.Add($"{{\"op_id\":\"{op.OperationId}\",\"agent\":\"{op.AssignedAgentId}\",\"faction\":\"{op.TargetFactionId}\",\"start\":{op.StartDay},\"dur\":{op.DurationDays},\"rem\":{op.RemainingDays},\"status\":{(int)op.Status},\"chance\":{op.SuccessChance.ToString("F2", CultureInfo.InvariantCulture)}}}");
            }

            var repList = new List<string>();
            foreach (var r in _reports)
            {
                repList.Add($"{{\"id\":\"{r.ReportId}\",\"faction\":\"{r.SourceFactionId}\",\"type\":\"{r.IntelligenceType}\",\"val\":{r.Value},\"acc\":{r.Accuracy.ToString("F2", CultureInfo.InvariantCulture)},\"day\":{r.DayObtained},\"decoded\":{(r.IsDecoded ? "true" : "false")},\"summary\":\"{r.Summary}\"}}");
            }

            var susList = new List<string>();
            foreach (var kvp in _factionSuspicion)
            {
                susList.Add($"\"{kvp.Key}\":{kvp.Value.ToString("F1", CultureInfo.InvariantCulture)}");
            }

            return $"{{\"schema_version\":1,\"operations\":[{string.Join(",", opList)}],\"reports\":[{string.Join(",", repList)}],\"suspicion\":{{{string.Join(",", susList)}}}}}";
        }

        public void RestoreState(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;

            _activeOperations.Clear();
            _reports.Clear();
            _factionSuspicion.Clear();

            // Restore active operations. CaptureState serialized them since the
            // beginning, but RestoreState silently dropped them (ORPHAN-SEAL-W1
            // fix, 2026-09-23): every in-flight covert operation was lost across
            // save/load, including agent availability and remaining days.
            int opIdx = json.IndexOf("\"operations\"", StringComparison.Ordinal);
            if (opIdx >= 0)
            {
                int opArrStart = json.IndexOf('[', opIdx);
                int opArrEnd = json.IndexOf(']', opArrStart);
                if (opArrStart >= 0 && opArrEnd > opArrStart)
                {
                    string opInner = json.Substring(opArrStart + 1, opArrEnd - opArrStart - 1);
                    int opCursor = 0;
                    while (opCursor < opInner.Length)
                    {
                        int opObjStart = opInner.IndexOf('{', opCursor);
                        if (opObjStart < 0) break;
                        int opObjEnd = opInner.IndexOf('}', opObjStart);
                        if (opObjEnd < 0) break;
                        string opObj = opInner.Substring(opObjStart, opObjEnd - opObjStart + 1);
                        string opId = ExtractString(opObj, "op_id");
                        if (!string.IsNullOrEmpty(opId))
                        {
                            bool authored = _catalog.TryGetOperation(opId, out var def);
                            _activeOperations.Add(new ActiveCovertOperation
                            {
                                OperationId = opId,
                                OperationType = authored ? def.OperationType : "infiltrate",
                                TargetFactionId = ExtractString(opObj, "faction"),
                                AssignedAgentId = ExtractString(opObj, "agent"),
                                StartDay = ExtractInt(opObj, "start", 1),
                                DurationDays = ExtractInt(opObj, "dur", authored ? def.BaseDurationDays : 7),
                                RemainingDays = ExtractInt(opObj, "rem", authored ? def.BaseDurationDays : 7),
                                Status = (CovertOperationStatus)ExtractInt(opObj, "status", (int)CovertOperationStatus.Active),
                                SuccessChance = ExtractFloat(opObj, "chance", authored ? def.BaseSuccessRate : 0.6f)
                            });
                        }
                        opCursor = opObjEnd + 1;
                    }
                }
            }

            // Restore suspicion
            int susIdx = json.IndexOf("\"suspicion\"", StringComparison.Ordinal);
            if (susIdx >= 0)
            {
                int startObj = json.IndexOf('{', susIdx);
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
                            string v = pair.Substring(colon + 1).Trim();
                            if (float.TryParse(v, NumberStyles.Float, CultureInfo.InvariantCulture, out float val))
                            {
                                _factionSuspicion[k] = val;
                            }
                        }
                    }
                }
            }

            // Restore reports
            int repIdx = json.IndexOf("\"reports\"", StringComparison.Ordinal);
            if (repIdx >= 0)
            {
                int arrStart = json.IndexOf('[', repIdx);
                int arrEnd = json.IndexOf(']', arrStart);
                if (arrStart >= 0 && arrEnd > arrStart)
                {
                    string inner = json.Substring(arrStart + 1, arrEnd - arrStart - 1);
                    int idx = 0;
                    while (idx < inner.Length)
                    {
                        int objStart = inner.IndexOf('{', idx);
                        if (objStart < 0) break;
                        int objEnd = inner.IndexOf('}', objStart);
                        if (objEnd < 0) break;

                        string obj = inner.Substring(objStart, objEnd - objStart + 1);
                        var rep = new IntelligenceReport
                        {
                            ReportId = ExtractString(obj, "id"),
                            SourceFactionId = ExtractString(obj, "faction"),
                            IntelligenceType = ExtractString(obj, "type"),
                            Value = ExtractInt(obj, "val", 40),
                            Accuracy = ExtractFloat(obj, "acc", 0.85f),
                            DayObtained = ExtractInt(obj, "day", 1),
                            IsDecoded = obj.Contains("\"decoded\":true"),
                            Summary = ExtractString(obj, "summary")
                        };

                        if (!string.IsNullOrEmpty(rep.ReportId))
                        {
                            _reports.Add(rep);
                        }

                        idx = objEnd + 1;
                    }
                }
            }
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
}
