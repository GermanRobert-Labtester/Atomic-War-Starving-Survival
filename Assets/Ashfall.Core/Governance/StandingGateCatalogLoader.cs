// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Governance
{
    /// <summary>
    /// Strict snake_case projection of one authored standing gate row
    /// (<c>standing_gates.json</c>), extended with the plan 59A enforcement
    /// binding: which real, wired CI gate enforces it — or an explicit,
    /// written decision not to gate it.
    /// </summary>
    public sealed class StandingGateData
    {
        public string gate_id { get; set; } = string.Empty;
        public int wave_origin { get; set; }
        public string finding_clause { get; set; } = string.Empty;
        public string enforcement_rule { get; set; } = string.Empty;
        public string tier { get; set; } = "per_push";
        public string owner_role { get; set; } = string.Empty;
        public bool is_enforced { get; set; } = true;

        /// <summary>
        /// Gate id in <c>docs/ci/CI_GATE_MANIFEST.json</c> that actually runs
        /// this rule, or <c>"none"</c> when the rule is enforced by human
        /// review instead of a script. A standing gate that names a gate which
        /// does not exist, or that is registered but not critical, is reported
        /// as unenforced rather than counted as green.
        /// </summary>
        public string enforcement_ref { get; set; } = "none";

        /// <summary>
        /// Required when <c>enforcement_ref</c> is <c>"none"</c>: the written,
        /// dated decision that this finding class is deliberately not gated and
        /// what human instrument replaces the gate (Plan 59A step 6).
        /// </summary>
        public string non_gate_rule { get; set; } = string.Empty;
    }

    /// <summary>Root document of <c>standing_gates.json</c>.</summary>
    public sealed class StandingGateCatalogData
    {
        public int schema_version { get; set; } = 1;
        public List<StandingGateData>? standing_gates { get; set; }
    }

    /// <summary>
    /// Result of a strict load: either validated gate definitions or the
    /// collected errors. A malformed row is never silently dropped — the
    /// registry's lenient <c>FromJson</c> parser would otherwise turn a typo'd
    /// tier or an unnamed owner into a green gate, which is exactly the failure
    /// mode Plan 59 exists to close.
    /// </summary>
    public sealed class StandingGateCatalogLoadResult
    {
        public List<string> Errors { get; } = new List<string>();
        public List<StandingGateDef> Gates { get; } = new List<StandingGateDef>();
        public string? SourcePath { get; private set; }

        public bool HasErrors => Errors.Count > 0;

        internal void Set(string path) => SourcePath = path;
    }

    /// <summary>
    /// Plan 59 / Task 59A — strict loader for the authored standing-gate
    /// register. Mirrors the established catalog-loader contract: snake_case
    /// projection, collected errors, validation before any gate is accepted.
    /// The gate never defaults a tier, an owner, or an enforcement reference.
    /// </summary>
    public static class StandingGateCatalogLoader
    {
        public const string FileName = "standing_gates.json";

        /// <summary>Value of <c>enforcement_ref</c> meaning "no scripted gate".</summary>
        public const string NoEnforcementRef = "none";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            IncludeFields = true,
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true
        };

        /// <summary>Tier vocabulary. Anything else is a load error, not a default.</summary>
        public static readonly IReadOnlyList<string> KnownTiers = new[] { "per_push", "nightly", "per_release" };

        public static StandingGateCatalogLoadResult Load(string dataDirectory, IFileIO files)
        {
            var result = new StandingGateCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(dataDirectory) || files == null)
            {
                result.Errors.Add("StandingGateCatalogLoader: invalid loader arguments.");
                return result;
            }

            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path))
            {
                result.Errors.Add($"StandingGateCatalogLoader: {FileName} does not exist at '{path}'.");
                return result;
            }

            StandingGateCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<StandingGateCatalogData>(
                    files.ReadAllText(path), Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"StandingGateCatalogLoader: failed to parse {FileName}: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add($"StandingGateCatalogLoader: {FileName} produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set(path);
            return result;
        }

        /// <summary>Strict load of an already-read document (used by tests and probes).</summary>
        public static StandingGateCatalogLoadResult LoadFromJson(string json)
        {
            var result = new StandingGateCatalogLoadResult();
            if (string.IsNullOrWhiteSpace(json))
            {
                result.Errors.Add("StandingGateCatalogLoader: empty register document.");
                return result;
            }

            StandingGateCatalogData? data;
            try
            {
                data = JsonSerializer.Deserialize<StandingGateCatalogData>(json, Options);
            }
            catch (Exception ex)
            {
                result.Errors.Add($"StandingGateCatalogLoader: failed to parse: {ex.Message}");
                return result;
            }

            if (data == null)
            {
                result.Errors.Add("StandingGateCatalogLoader: produced no data.");
                return result;
            }

            Validate(data, result);
            if (!result.HasErrors) result.Set("(in-memory)");
            return result;
        }

        private static void Validate(StandingGateCatalogData data, StandingGateCatalogLoadResult result)
        {
            if (data.schema_version != 1)
                result.Errors.Add(
                    $"StandingGateCatalogLoader: unsupported schema_version '{data.schema_version}' (expected 1).");

            if (data.standing_gates == null || data.standing_gates.Count == 0)
            {
                result.Errors.Add("StandingGateCatalogLoader: standing_gates must not be empty.");
                return;
            }

            var ids = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var row in data.standing_gates)
            {
                if (row == null)
                {
                    result.Errors.Add("StandingGateCatalogLoader: null standing gate row.");
                    continue;
                }

                string gateId = (row.gate_id ?? string.Empty).Trim();
                if (gateId.Length == 0)
                {
                    result.Errors.Add("StandingGateCatalogLoader: standing gate with an empty gate_id.");
                }
                else if (!ids.Add(gateId))
                {
                    result.Errors.Add($"StandingGateCatalogLoader: duplicate standing gate id '{gateId}'.");
                }

                // Wave origin is the lineage column of the retrospective table
                // (Plan 59's evidence inventory). 0 means "no wave", which is
                // un-authored, so it is rejected rather than defaulted.
                if (row.wave_origin < 1 || row.wave_origin > 99)
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' has wave_origin '{row.wave_origin}' "
                        + "(must be the retrospective wave that produced the finding, 1..99).");
                }

                string tier = (row.tier ?? string.Empty).Trim();
                if (!ContainsIgnoreCase(KnownTiers, tier))
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' names unknown tier '{tier}'. "
                        + "Known tiers: " + string.Join(", ", KnownTiers) + ".");
                }

                if ((row.owner_role ?? string.Empty).Trim().Length == 0)
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' has no owner_role. "
                        + "An unowned gate is the reason three gates were red while every plan claimed green (Plan 59A step 2).");
                }

                if ((row.finding_clause ?? string.Empty).Trim().Length == 0)
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' has no finding_clause (the audit finding it closes).");
                }

                if ((row.enforcement_rule ?? string.Empty).Trim().Length == 0)
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' has no enforcement_rule.");
                }

                string enforcementRef = (row.enforcement_ref ?? string.Empty).Trim();
                if (enforcementRef.Length == 0) enforcementRef = NoEnforcementRef;

                bool isGated = !string.Equals(enforcementRef, NoEnforcementRef, StringComparison.OrdinalIgnoreCase);
                if (!isGated && (row.non_gate_rule ?? string.Empty).Trim().Length == 0)
                {
                    // Plan 59A step 6: what is NOT gated must carry an explicit,
                    // written, dated decision and the human instrument replacing it.
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' declares no enforcement_ref but no "
                        + "non_gate_rule either. Write the decision not to gate it (Plan 59A step 6).");
                }
                if (isGated && (row.non_gate_rule ?? string.Empty).Trim().Length > 0)
                {
                    result.Errors.Add(
                        $"StandingGateCatalogLoader: standing gate '{gateId}' binds enforcement_ref "
                        + $"'{enforcementRef}' but still carries a non_gate_rule; a gated gate does not need one.");
                }

                result.Gates.Add(new StandingGateDef
                {
                    GateId = gateId,
                    WaveOrigin = row.wave_origin,
                    FindingClause = row.finding_clause ?? string.Empty,
                    EnforcementRule = row.enforcement_rule ?? string.Empty,
                    Tier = ParseTier(tier),
                    OwnerRole = row.owner_role ?? string.Empty,
                    IsEnforced = row.is_enforced
                });
            }
        }

        internal static GateTier ParseTier(string tier)
        {
            return tier.ToLowerInvariant() switch
            {
                "nightly" => GateTier.Nightly,
                "per_release" or "perrelease" => GateTier.PerRelease,
                _ => GateTier.PerPush
            };
        }

        private static bool ContainsIgnoreCase(IReadOnlyList<string> known, string value)
        {
            for (int i = 0; i < known.Count; i++)
                if (string.Equals(known[i], value, StringComparison.OrdinalIgnoreCase)) return true;
            return false;
        }
    }

    /// <summary>One standing gate paired with its authored enforcement binding.</summary>
    public sealed class StandingGateBinding
    {
        public StandingGateDef Def { get; }
        public string EnforcementRef { get; }
        public string NonGateRule { get; }

        /// <summary>True when the row binds a real CI gate id instead of "none".</summary>
        public bool IsGated =>
            !string.Equals(EnforcementRef, StandingGateCatalogLoader.NoEnforcementRef, StringComparison.OrdinalIgnoreCase);

        public StandingGateBinding(StandingGateDef def, string enforcementRef, string nonGateRule)
        {
            Def = def ?? throw new ArgumentNullException(nameof(def));
            EnforcementRef = enforcementRef ?? string.Empty;
            NonGateRule = nonGateRule ?? string.Empty;
        }
    }
}
