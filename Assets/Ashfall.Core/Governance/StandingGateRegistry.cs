#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Governance
{
    public enum GateTier
    {
        PerPush,
        Nightly,
        PerRelease
    }

    /// <summary>
    /// Definition of a standing architectural gate derived from the 22 finding classes of Waves 1-9.
    /// Loaded from StreamingAssets/Data/standing_gates.json.
    /// </summary>
    public sealed class StandingGateDef
    {
        public string GateId { get; set; } = string.Empty;
        public int WaveOrigin { get; set; }
        public string FindingClause { get; set; } = string.Empty;
        public string EnforcementRule { get; set; } = string.Empty;
        public GateTier Tier { get; set; } = GateTier.PerPush;
        public string OwnerRole { get; set; } = string.Empty;
        public bool IsEnforced { get; set; } = true;
    }

    /// <summary>
    /// Violation record produced when a standing gate fails verification.
    /// </summary>
    public sealed class StandingGateViolation
    {
        public string GateId { get; set; } = string.Empty;
        public int WaveOrigin { get; set; }
        public string FindingClause { get; set; } = string.Empty;
        public string EnforcementRule { get; set; } = string.Empty;
        public string OwnerRole { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
    }

    /// <summary>
    /// Consolidated audit report from evaluating standing retrospective gates.
    /// </summary>
    public sealed class RetrospectiveAuditReport
    {
        public int TotalGates { get; set; }
        public int EnforcedGates { get; set; }
        public int EvaluatedGates { get; set; }
        public int PassingGates { get; set; }
        public bool AllGatesPassing { get; set; }
        public List<StandingGateViolation> Violations { get; set; } = new List<StandingGateViolation>();
    }

    /// <summary>
    /// Pure domain engine for Plan 59: Retrospective — Turn Nine Waves of Findings into Rules, Then Stop Auditing.
    /// Codifies the 22 historical finding classes into an enforceable gate registry.
    /// Zero engine references (Godot/UnityEngine free).
    /// </summary>
    public sealed class StandingGateRegistry
    {
        public const string SystemId = "standing_gate_registry";

        private readonly Dictionary<string, StandingGateDef> _gates = new Dictionary<string, StandingGateDef>(StringComparer.OrdinalIgnoreCase);

        // Seam delegates
        public Action<StandingGateDef, bool>? OnGateEvaluatedSeam { get; set; }
        public Action<StandingGateViolation>? OnGateViolationDetectedSeam { get; set; }
        public Action<RetrospectiveAuditReport>? OnRetrospectiveAuditedSeam { get; set; }

        public StandingGateRegistry(IEnumerable<StandingGateDef>? gates = null)
        {
            if (gates != null)
            {
                foreach (var g in gates)
                {
                    if (g != null && !string.IsNullOrEmpty(g.GateId))
                    {
                        _gates[g.GateId] = g;
                    }
                }
            }
        }

        /// <summary>
        /// Factory parser for standing_gates.json catalog.
        /// </summary>
        public static StandingGateRegistry FromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Standing gates JSON cannot be null or empty", nameof(json));

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;
            var list = new List<StandingGateDef>();

            if (root.TryGetProperty("standing_gates", out var gatesElem) && gatesElem.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in gatesElem.EnumerateArray())
                {
                    var gate = new StandingGateDef
                    {
                        GateId = item.TryGetProperty("gate_id", out var idElem) ? idElem.GetString() ?? string.Empty : string.Empty,
                        WaveOrigin = item.TryGetProperty("wave_origin", out var waveElem) ? waveElem.GetInt32() : 0,
                        FindingClause = item.TryGetProperty("finding_clause", out var findElem) ? findElem.GetString() ?? string.Empty : string.Empty,
                        EnforcementRule = item.TryGetProperty("enforcement_rule", out var ruleElem) ? ruleElem.GetString() ?? string.Empty : string.Empty,
                        OwnerRole = item.TryGetProperty("owner_role", out var ownerElem) ? ownerElem.GetString() ?? string.Empty : string.Empty,
                        IsEnforced = !item.TryGetProperty("is_enforced", out var enfElem) || enfElem.GetBoolean()
                    };

                    if (item.TryGetProperty("tier", out var tierElem))
                    {
                        string tierStr = tierElem.GetString() ?? string.Empty;
                        gate.Tier = tierStr.ToLowerInvariant() switch
                        {
                            "nightly" => GateTier.Nightly,
                            "per_release" or "perrelease" => GateTier.PerRelease,
                            _ => GateTier.PerPush
                        };
                    }

                    list.Add(gate);
                }
            }

            return new StandingGateRegistry(list);
        }

        public StandingGateDef? GetGate(string gateId)
        {
            if (string.IsNullOrEmpty(gateId)) return null;
            _gates.TryGetValue(gateId, out var gate);
            return gate;
        }

        public IReadOnlyList<StandingGateDef> GetAllGates() => _gates.Values.ToList();

        public IReadOnlyList<StandingGateDef> GetGatesByTier(GateTier tier) =>
            _gates.Values.Where(g => g.Tier == tier).ToList();

        public IReadOnlyList<StandingGateDef> GetGatesByWave(int wave) =>
            _gates.Values.Where(g => g.WaveOrigin == wave).ToList();

        /// <summary>
        /// Audits all enforced standing gates using a provided status evaluation predicate.
        /// </summary>
        public RetrospectiveAuditReport AuditStandingGates(Func<string, bool> gateStatusProvider, GateTier? filterTier = null)
        {
            if (gateStatusProvider == null) throw new ArgumentNullException(nameof(gateStatusProvider));

            var violations = new List<StandingGateViolation>();
            int evaluatedCount = 0;
            int passingCount = 0;

            var targetGates = _gates.Values
                .Where(g => g.IsEnforced && (!filterTier.HasValue || g.Tier == filterTier.Value))
                .ToList();

            foreach (var gate in targetGates)
            {
                evaluatedCount++;
                bool passing = gateStatusProvider(gate.GateId);

                if (passing)
                {
                    passingCount++;
                }
                else
                {
                    var violation = new StandingGateViolation
                    {
                        GateId = gate.GateId,
                        WaveOrigin = gate.WaveOrigin,
                        FindingClause = gate.FindingClause,
                        EnforcementRule = gate.EnforcementRule,
                        OwnerRole = gate.OwnerRole,
                        Reason = $"Standing gate '{gate.GateId}' failed verification: {gate.EnforcementRule}"
                    };
                    violations.Add(violation);
                    OnGateViolationDetectedSeam?.Invoke(violation);
                }

                OnGateEvaluatedSeam?.Invoke(gate, passing);
            }

            var report = new RetrospectiveAuditReport
            {
                TotalGates = _gates.Count,
                EnforcedGates = _gates.Values.Count(g => g.IsEnforced),
                EvaluatedGates = evaluatedCount,
                PassingGates = passingCount,
                AllGatesPassing = violations.Count == 0 && evaluatedCount > 0,
                Violations = violations
            };

            OnRetrospectiveAuditedSeam?.Invoke(report);
            return report;
        }
    }
}
