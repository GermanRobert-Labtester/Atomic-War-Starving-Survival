// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Governance
{
    /// <summary>
    /// Strict projection of one row of <c>docs/ci/CI_GATE_MANIFEST.json</c> —
    /// the repo's real enforcement authority: the gates that actually run, with
    /// the command that runs them and whether a red one blocks a release.
    /// </summary>
    public sealed class CiGateDefinition
    {
        public string gate_id { get; set; } = string.Empty;
        public string name { get; set; } = string.Empty;
        public string category { get; set; } = string.Empty;
        public string command { get; set; } = string.Empty;
        public int timeout_seconds { get; set; } = 60;
        public string expected_summary { get; set; } = string.Empty;
        public string classification { get; set; } = "fast";
        public bool critical { get; set; }
        public List<string>? depends_on { get; set; }
        public string notes { get; set; } = string.Empty;
    }

    /// <summary>Root document of <c>CI_GATE_MANIFEST.json</c>.</summary>
    public sealed class CiGateManifest
    {
        public string schema_version { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public int total_gates { get; set; }
        public int fast_tier_count { get; set; }
        public List<CiGateDefinition>? gates { get; set; }
    }

    /// <summary>
    /// Result of a strict manifest load: either the reported gates or the
    /// collected errors.
    /// </summary>
    public sealed class CiGateManifestLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<CiGateDefinition> Gates { get; } = new List<CiGateDefinition>();
        public bool HasErrors => Errors.Count > 0;
    }

    /// <summary>
    /// Plan 59 / Task 59A — reader for the standing gate register's enforcement
    /// target. Deliberately NOT a duplicate registry: it reports what the repo's
    /// own manifest says, so the retrospective's central measured claim — "gates
    /// exist" was never the same as "gates run" — becomes checkable. A
    /// <c>critical</c> gate is one whose red blocks a release; a non-critical or
    /// unclassified gate is informational and cannot enforce a standing rule.
    /// </summary>
    public static class CiGateManifestReader
    {
        public const string ManifestFileName = "CI_GATE_MANIFEST.json";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        public static CiGateManifestLoadResult Load(string json)
        {
            var result = new CiGateManifestLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add($"{ManifestFileName}: manifest text is empty.");
                return result;
            }

            CiGateManifest? manifest;
            try
            {
                manifest = JsonSerializer.Deserialize<CiGateManifest>(json, Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"{ManifestFileName}: failed to parse: {ex.Message}");
                return result;
            }

            if (manifest?.gates == null || manifest.gates.Count == 0)
            {
                result.Errors.Add($"{ManifestFileName}: no gates declared.");
                return result;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var gate in manifest.gates)
            {
                if (gate == null)
                {
                    result.Errors.Add($"{ManifestFileName}: null gate row.");
                    continue;
                }

                string gateId = (gate.gate_id ?? string.Empty).Trim();
                if (gateId.Length == 0)
                {
                    result.Errors.Add($"{ManifestFileName}: a gate row has no gate_id.");
                    continue;
                }
                if (!ids.Add(gateId))
                {
                    result.Errors.Add($"{ManifestFileName}: duplicate gate_id '{gateId}'.");
                    continue;
                }
                if ((gate.command ?? string.Empty).Trim().Length == 0)
                {
                    // A gate with no command cannot run. That is precisely the
                    // Wave 3 finding (three gates red while claims said green).
                    result.Errors.Add($"{ManifestFileName}: gate '{gateId}' declares no command, so it cannot run.");
                    continue;
                }
                if (gate.timeout_seconds <= 0)
                {
                    result.Errors.Add(
                        $"{ManifestFileName}: gate '{gateId}' has non-positive timeout_seconds '{gate.timeout_seconds}'.");
                }

                result.Gates.Add(gate);
            }

            if (manifest.total_gates != result.Gates.Count)
            {
                result.Errors.Add(
                    $"{ManifestFileName}: declared total_gates '{manifest.total_gates}' but parsed '{result.Gates.Count}' rows.");
            }

            return result;
        }

        /// <summary>O(1) lookup by gate id. True only for a parsed, runnable gate.</summary>
        public static bool TryGet(IEnumerable<CiGateDefinition> gates, string gateId, out CiGateDefinition? gate)
        {
            gate = null;
            if (string.IsNullOrWhiteSpace(gateId)) return false;
            foreach (var candidate in gates)
            {
                if (string.Equals(candidate.gate_id, gateId, StringComparison.OrdinalIgnoreCase))
                {
                    gate = candidate;
                    return true;
                }
            }
            return false;
        }
    }
}
