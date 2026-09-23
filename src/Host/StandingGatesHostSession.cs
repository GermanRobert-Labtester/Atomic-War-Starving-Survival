// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Governance;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Verification verdict for one standing gate. The distinction matters: a
    /// gate that merely EXISTS is not enforcement, which is the measured finding
    /// Plan 59 exists to close ("three CI gates were red while every plan
    /// claimed gates were green").
    /// </summary>
    public enum StandingGateVerdict
    {
        /// <summary>Bound to a real, critical, tier-compatible CI gate.</summary>
        Enforced,

        /// <summary>Deliberately not scripted; carries a written rule and owner.</summary>
        RuleOnly,

        /// <summary>Names a CI gate that is not in the manifest, or has no command.</summary>
        Unbound,

        /// <summary>Bound, but the CI gate is not critical, so a red does not block.</summary>
        NotBlocking,

        /// <summary>Bound and critical, but the CI gate's classification cannot
        /// satisfy the standing gate's tier (a per-push rule needs a fast gate).</summary>
        TierMismatch
    }

    /// <summary>One standing gate's measured enforcement state.</summary>
    public sealed class StandingGateStatus
    {
        public StandingGateDef Def { get; }
        public string EnforcementRef { get; }
        public StandingGateVerdict Verdict { get; }
        public string Reason { get; }

        public bool Passing => Verdict == StandingGateVerdict.Enforced
            || Verdict == StandingGateVerdict.RuleOnly;

        public string GateId => Def.GateId;

        public StandingGateStatus(StandingGateDef def, string enforcementRef, StandingGateVerdict verdict, string reason)
        {
            Def = def ?? throw new ArgumentNullException(nameof(def));
            EnforcementRef = enforcementRef ?? string.Empty;
            Verdict = verdict;
            Reason = reason ?? string.Empty;
        }

        public string Describe() =>
            $"{GateId} ({Def.Tier}, {Def.OwnerRole}) = {Verdict}: {Reason}";
    }

    /// <summary>
    /// Plan 59 / Task 59A host session — the retrospective's own gate made real.
    /// <para>
    /// Nine waves produced 22 finding classes and the same finding each time in a
    /// new costume. This session loads the 22-row standing-gate register, then
    /// measures the only column that matters: is each rule actually enforced by a
    /// gate that runs in this repository? Enforcement means the row names a real
    /// gate in <c>docs/ci/CI_GATE_MANIFEST.json</c> — the repo's own authority
    /// for gates that run — that gate has a command, it is marked critical (a red
    /// blocks a release), and its fast/full classification can satisfy the
    /// standing gate's tier.
    /// </para>
    /// <para>
    /// Authority boundary (deliberate): this session runs NO CI gate and
    /// substitutes for none. It reports what the manifest declares; the gate
    /// itself stays owned by its command. A rule that is deliberately not gated
    /// is a first-class, honest result with an owner and a written reason — not a
    /// failure — because Plan 59A step 6 requires an explicit decision rather
    /// than gate sprawl.
    /// </para>
    /// </summary>
    public sealed class StandingGatesHostSession : HostSessionBase
    {
        private readonly Dictionary<string, CiGateDefinition> _ciGates =
            new Dictionary<string, CiGateDefinition>(StringComparer.OrdinalIgnoreCase);
        private readonly List<StandingGateStatus> _statuses = new List<StandingGateStatus>();
        private readonly List<string> _errors = new List<string>();

        public StandingGateRegistry? Registry { get; private set; }
        public IReadOnlyList<StandingGateStatus> Statuses => _statuses;

        /// <summary>Register load errors (empty when the register is bound).</summary>
        public IReadOnlyList<string> Errors => _errors;

        /// <summary>Rows of <c>standing_gates.json</c> with their enforcement binding.</summary>
        public List<StandingGateBinding> Bindings { get; } = new List<StandingGateBinding>();

        public bool IsLoaded => Registry != null;

        // ── Loading ────────────────────────────────────────────────────

        /// <summary>Load and validate the authored standing-gate register.</summary>
        public bool LoadRegister(string dataDirectory, IFileIO files)
        {
            _errors.Clear();
            var load = StandingGateCatalogLoader.Load(dataDirectory, files);
            if (load.HasErrors)
            {
                _errors.AddRange(load.Errors);
                Registry = null;
                return false;
            }

            var registry = new StandingGateRegistry(load.Gates);
            Registry = registry;
            Bindings.Clear();
            // Re-read the authored rows to carry the enforcement binding, which
            // is a governance column and not gameplay state.
            Bindings.AddRange(ReadBindings(dataDirectory, files));
            return true;
        }

        private static List<StandingGateBinding> ReadBindings(string dataDirectory, IFileIO files)
        {
            var result = new List<StandingGateBinding>();
            string path = Path.Combine(dataDirectory, StandingGateCatalogLoader.FileName);
            if (!files.FileExists(path)) return result;
            try
            {
                var doc = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                    files.ReadAllText(path), StandingGatesJsonOptions);
                if (doc?.standing_gates == null) return result;
                foreach (var row in doc.standing_gates)
                {
                    if (row == null) continue;
                    result.Add(new StandingGateBinding(
                        new StandingGateDef
                        {
                            GateId = row.gate_id ?? string.Empty,
                            WaveOrigin = row.wave_origin,
                            FindingClause = row.finding_clause ?? string.Empty,
                            EnforcementRule = row.enforcement_rule ?? string.Empty,
                            Tier = StandingGateCatalogLoader.ParseTier(row.tier ?? string.Empty),
                            OwnerRole = row.owner_role ?? string.Empty,
                            IsEnforced = row.is_enforced
                        },
                        (row.enforcement_ref ?? StandingGateCatalogLoader.NoEnforcementRef).Trim(),
                        row.non_gate_rule ?? string.Empty));
                }
            }
            catch (Exception)
            {
                // The strict loader has already reported a malformed register;
                // this second pass is only for the governance columns.
            }
            return result;
        }

        /// <summary>
        /// Load the repo's enforcement authority. <paramref name="ciManifestPath"/>
        /// is <c>docs/ci/CI_GATE_MANIFEST.json</c> relative to the repo root.
        /// </summary>
        public bool LoadEnforcementManifest(string ciManifestPath, IFileIO files)
        {
            if (!files.FileExists(ciManifestPath))
            {
                _errors.Add($"StandingGatesHostSession: enforcement manifest missing at '{ciManifestPath}'. "
                    + "A missing manifest means NO standing gate can be verified as enforced.");
                return false;
            }
            var load = CiGateManifestReader.Load(files.ReadAllText(ciManifestPath));
            foreach (var error in load.Errors) _errors.Add(error);
            _ciGates.Clear();
            foreach (var gate in load.Gates) _ciGates[gate.gate_id] = gate;
            return load.Gates.Count > 0;
        }

        // ── Audit ──────────────────────────────────────────────────────

        /// <summary>
        /// Measure every enforced standing gate and return the retrospective
        /// audit report from the Core authority. Passing means enforced-by-a-real
        /// gate OR deliberately-rule-only; everything else is a violation the
        /// registry itself reports with the wave that produced the finding.
        /// </summary>
        public RetrospectiveAuditReport Audit()
        {
            _statuses.Clear();
            if (Registry == null)
            {
                _errors.Add("StandingGatesHostSession: no standing-gate register loaded.");
                return new RetrospectiveAuditReport { AllGatesPassing = false };
            }

            foreach (var binding in Bindings)
            {
                if (!binding.Def.IsEnforced) continue;
                _statuses.Add(Measure(binding));
            }

            var byGate = new Dictionary<string, bool>(StringComparer.OrdinalIgnoreCase);
            foreach (var status in _statuses) byGate[status.GateId] = status.Passing;

            var report = Registry.AuditStandingGates(gateId =>
                byGate.TryGetValue(gateId, out bool passing) && passing);

            RaiseStateChanged();
            return report;
        }

        private StandingGateStatus Measure(StandingGateBinding binding)
        {
            string enforcementRef = binding.EnforcementRef;
            bool isGated = binding.IsGated;
            if (_enforcementOverrideKey != null
                && string.Equals(binding.Def.GateId, _enforcementOverrideKey, StringComparison.OrdinalIgnoreCase))
            {
                enforcementRef = _enforcementOverrideValue;
                isGated = !string.Equals(enforcementRef, StandingGateCatalogLoader.NoEnforcementRef,
                    StringComparison.OrdinalIgnoreCase);
            }

            if (!isGated)
            {
                // The loader already proved a non-gate_rule exists, so this is a
                // written, owned, dated decision — Plan 59A step 6's honest
                // alternative to a gate.
                return new StandingGateStatus(
                    binding.Def, binding.EnforcementRef, StandingGateVerdict.RuleOnly,
                    "deliberately not scripted; " + Trim(binding.NonGateRule, 160));
            }

            if (!CiGateManifestReader.TryGet(_ciGates.Values, enforcementRef, out var gate) || gate == null)
            {
                return new StandingGateStatus(
                    binding.Def, enforcementRef, StandingGateVerdict.Unbound,
                    $"names CI gate '{binding.EnforcementRef}', which is not declared in the enforcement manifest.");
            }

            if (!gate.critical)
            {
                return new StandingGateStatus(
                    binding.Def, enforcementRef, StandingGateVerdict.NotBlocking,
                    $"CI gate '{gate.gate_id}' is registered but not critical, so a red does not block a release.");
            }

            string requiredClass = !string.IsNullOrEmpty(_requiredClassOverride)
                ? _requiredClassOverride
                : RequiredClassification(binding.Def.Tier);

            bool classificationOk = ClassificationSatisfiesTier(gate.classification, binding.Def.Tier);
            if (!string.IsNullOrEmpty(_requiredClassOverride))
            {
                // Self-proof path: a caller-forced requirement lets the probe
                // prove the tier check discriminates, without editing data.
                classificationOk = string.Equals(gate.classification, requiredClass, StringComparison.OrdinalIgnoreCase);
            }

            if (!classificationOk)
            {
                return new StandingGateStatus(
                    binding.Def, enforcementRef, StandingGateVerdict.TierMismatch,
                    $"CI gate '{gate.gate_id}' is '{gate.classification}'; a {binding.Def.Tier} standing "
                    + $"rule requires a '{requiredClass}' gate so it actually runs at that cadence.");
            }

            return new StandingGateStatus(
                binding.Def, enforcementRef, StandingGateVerdict.Enforced,
                $"enforced by '{gate.gate_id}' ({gate.classification}, critical) — \"{Shorten(gate.name)}\"");
        }

        /// <summary>
        /// Does a CI gate's classification actually satisfy a standing rule's tier?
        /// <para>
        /// A per-push rule must be enforced by a gate that runs per push — the
        /// <c>fast</c> tier; a <c>full</c>-tier soak does not run on every push. A
        /// nightly or per-release rule accepts either tier, because a fast gate
        /// also runs in the full suite and at release, so it satisfies the slower
        /// cadence as well. Requiring <c>full</c> for nightly rules would mark the
        /// repository's best gates as insufficient, which is not the finding.
        /// </para>
        /// </summary>
        public static bool ClassificationSatisfiesTier(string classification, GateTier tier)
        {
            if (string.IsNullOrEmpty(classification)) return false;
            bool isFast = string.Equals(classification, "fast", StringComparison.OrdinalIgnoreCase);
            bool isFull = string.Equals(classification, "full", StringComparison.OrdinalIgnoreCase);
            if (tier == GateTier.PerPush) return isFast;
            return isFast || isFull;
        }

        /// <summary>Classification a per-push standing rule requires.</summary>
        public static string RequiredClassification(GateTier tier) =>
            tier == GateTier.PerPush ? "fast" : "fast|full";

        // ── Reporting ──────────────────────────────────────────────────

        /// <summary>Human-readable list of the measured gaps, or one verdict only.</summary>
        public string DescribeViolations(StandingGateVerdict? only = null)
        {
            var lines = new List<string>();
            foreach (var status in _statuses)
            {
                if (only.HasValue && status.Verdict != only.Value) continue;
                if (only.HasValue || !status.Passing) lines.Add(status.Describe());
            }
            return lines.Count == 0 ? "(none)" : string.Join(" | ", lines);
        }

        public int CountOf(StandingGateVerdict verdict)
        {
            int n = 0;
            foreach (var status in _statuses)
                if (status.Verdict == verdict) n++;
            return n;
        }

        public string SummaryLine()
        {
            if (_statuses.Count == 0) return "standing gates: not measured";
            return $"standing gates: {_statuses.Count} measured, {CountOf(StandingGateVerdict.Enforced)} enforced, "
                + $"{CountOf(StandingGateVerdict.RuleOnly)} rule-only, "
                + $"{CountOf(StandingGateVerdict.Unbound)} unbound, "
                + $"{CountOf(StandingGateVerdict.NotBlocking)} not-blocking, "
                + $"{CountOf(StandingGateVerdict.TierMismatch)} tier-mismatch";
        }

        // ── Self-proof fixtures (Plan 59A step 3: no self-proof, no gate) ──

        /// <summary>Self-proof: a register row with no owner must be rejected.</summary>
        public static bool BuildOwnerlessRegister(string dataDirectory, IFileIO files)
            => BuildVariant(dataDirectory, files, row => { row.owner_role = string.Empty; });

        /// <summary>Self-proof: a row declaring no gate must also carry a written rule.</summary>
        public static bool BuildUnruledRegister(string dataDirectory, IFileIO files)
            => BuildVariant(dataDirectory, files, row =>
            {
                row.enforcement_ref = StandingGateCatalogLoader.NoEnforcementRef;
                row.non_gate_rule = string.Empty;
            });

        /// <summary>Self-proof: an empty register must be rejected.</summary>
        public static bool BuildEmptyRegister(string dataDirectory, IFileIO files)
            => BuildVariant(dataDirectory, files, row => { row.gate_id = string.Empty; }, clearRows: true);

        /// <summary>Self-proof: an unknown tier must be rejected, not defaulted.</summary>
        public static bool BuildUnknownTierRegister(string dataDirectory, IFileIO files)
            => BuildVariant(dataDirectory, files, row => { row.tier = "whenever_feels_right"; });

        private static bool BuildVariant(
            string dataDirectory, IFileIO files, Action<StandingGateData> mutate, bool clearRows = false)
        {
            string path = Path.Combine(dataDirectory, StandingGateCatalogLoader.FileName);
            if (!files.FileExists(path)) return false;
            try
            {
                var doc = System.Text.Json.JsonSerializer.Deserialize<StandingGateCatalogData>(
                    files.ReadAllText(path), StandingGatesJsonOptions);
                if (doc?.standing_gates == null) return false;
                if (clearRows) doc.standing_gates.Clear();
                else
                {
                    foreach (var row in doc.standing_gates)
                        if (row != null) mutate(row);
                }
                return StandingGateCatalogLoader.LoadFromJson(
                    System.Text.Json.JsonSerializer.Serialize(doc, StandingGatesJsonOptions)).HasErrors;
            }
            catch (Exception)
            {
                return true;
            }
        }

        internal static readonly System.Text.Json.JsonSerializerOptions StandingGatesJsonOptions =
            new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower,
                ReadCommentHandling = System.Text.Json.JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

        /// <summary>
        /// Re-audit with an overridden tier requirement, so a caller can prove the
        /// tier check actually discriminates: a per-push standing rule requires a
        /// <c>fast</c> gate, so demanding <c>full</c> instead must turn every
        /// fast-bound per-push rule into a measured mismatch.
        /// </summary>
        public RetrospectiveAuditReport AuditWithTierOverride(string requiredClass, string newRequiredClass)
        {
            string previous = _requiredClassOverride;
            _requiredClassOverride = newRequiredClass ?? string.Empty;
            try
            {
                return Audit();
            }
            finally
            {
                _requiredClassOverride = previous;
            }
        }

        private string _requiredClassOverride = string.Empty;

        /// <summary>
        /// Re-audit with one gate's enforcement reference replaced, proving the
        /// measurement actually rejects a reference to a gate that does not
        /// exist. The reference lives in the binding, so this is a measurement
        /// override, not a data rewrite.
        /// </summary>
        public RetrospectiveAuditReport AuditWithEnforcementOverride(string gateId, string newRef)
        {
            string? previous = _enforcementOverrideKey;
            string previousValue = _enforcementOverrideValue;
            _enforcementOverrideKey = gateId;
            _enforcementOverrideValue = newRef ?? string.Empty;
            try
            {
                return Audit();
            }
            finally
            {
                _enforcementOverrideKey = previous;
                _enforcementOverrideValue = previousValue;
            }
        }

        private string? _enforcementOverrideKey;
        private string _enforcementOverrideValue = string.Empty;

        private static string Trim(string value, int max) =>
            value == null || value.Length <= max ? value : value.Substring(0, max) + "…";

        private static string Shorten(string value) => Trim(value ?? string.Empty, 64);
    }
}
