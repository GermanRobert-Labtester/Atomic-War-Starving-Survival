using System;
using System.Collections.Generic;
using System.IO;

using Ashfall.Core.IO;
namespace Ashfall.Core.Foundry
{
    // ---------------------------------------------------------------------
    // Treaty consequence policy — foundry_treaty_consequences.json
    // Typed, data-driven rules connecting a treaty outcome to bounded
    // standing and market/logistics consequences. Static authored data; the
    // mutable application ledger lives in SilentFoundryConsequenceState.
    // ---------------------------------------------------------------------

    /// <summary>
    /// Treaty outcome vocabulary (authoritative). NotRatified/Pending are
    /// neutral; Met/Missed/Violated carry policy consequences.
    /// </summary>
    public enum FoundryTreatyOutcome
    {
        /// <summary>Assessment day is before the treaty's ratification day.</summary>
        NotRatified = 0,
        /// <summary>Ratified, but the first assessment day has not been reached.</summary>
        Pending = 1,
        /// <summary>Obligation fulfilled for the cycle (quota met / accord upheld).</summary>
        Met = 2,
        /// <summary>Obligation not fulfilled for the cycle (quota short).</summary>
        Missed = 3,
        /// <summary>Obligation actively breached (labor accord: strike / overtime / child labor).</summary>
        Violated = 4
    }

    /// <summary>One market-demand modifier (positive delta = more scarce / higher price).</summary>
    [Serializable]
    public sealed class FoundryGoodModifier
    {
        public string good_id = string.Empty;
        public float demand_delta = 0f;
        public string reason = string.Empty;
    }

    /// <summary>
    /// One policy row: for a given treaty and outcome, the standing delta and
    /// the market/logistics demand modifiers. All magnitudes are authored and
    /// documented in foundry_treaty_consequences.json; nothing is hard-coded
    /// in code.
    /// </summary>
    [Serializable]
    public sealed class FoundryTreatyConsequencePolicy
    {
        public string treaty_id = string.Empty;
        public string faction_id = string.Empty;
        public string outcome = string.Empty;   // met | missed | violated
        public float standing_delta = 0f;
        public string reason = string.Empty;
        public List<FoundryGoodModifier> market_modifiers = new List<FoundryGoodModifier>();
    }

    [Serializable]
    public sealed class FoundryTreatyConsequenceFile
    {
        public int schema_version = 1;
        public string collection_id = string.Empty;
        public List<FoundryTreatyConsequencePolicy> policies = new List<FoundryTreatyConsequencePolicy>();
    }

    // ---------------------------------------------------------------------
    // Durable application ledger (rides the expansion-hub save envelope)
    // ---------------------------------------------------------------------

    /// <summary>One applied consequence — the idempotency + audit record.</summary>
    [Serializable]
    public sealed class FoundryConsequenceRecord
    {
        public string treatyId = string.Empty;
        public FoundryTreatyOutcome outcome = FoundryTreatyOutcome.Pending;
        public int appliedDay = 0;
        /// <summary>The assessment day this consequence belongs to (idempotency key).</summary>
        public int cycleMarker = 0;
        public float standingDelta = 0f;
        public List<FoundryGoodModifier> modifiers = new List<FoundryGoodModifier>();
        public string reason = string.Empty;
    }

    /// <summary>
    /// Durable consequence state. `guildStanding` is the authoritative net
    /// standing of the Foundry Guild from its treaties; the host mirrors it
    /// into the existing FactionStanceEngine (SetTrust on restore, ModifyTrust
    /// per applied consequence). Market modifiers are applied to the existing
    /// MarketSystem and persist inside its own MarketState.
    /// </summary>
    [Serializable]
    public sealed class SilentFoundryConsequenceState
    {
        public const int CurrentVersion = 1;

        public int stateVersion = CurrentVersion;
        public List<FoundryConsequenceRecord> applied = new List<FoundryConsequenceRecord>();
        public float guildStanding = 0f;

        /// <summary>True when a consequence was already applied for (treaty, cycle).</summary>
        public bool IsApplied(string treatyId, int cycleMarker)
        {
            if (applied == null) return false;
            for (int i = 0; i < applied.Count; i++)
            {
                var r = applied[i];
                if (r != null && r.cycleMarker == cycleMarker
                    && string.Equals(r.treatyId, treatyId, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }
    }

    // ---------------------------------------------------------------------
    // Loader + in-memory catalog
    // ---------------------------------------------------------------------

    /// <summary>Engine-agnostic loader for foundry_treaty_consequences.json.</summary>
    public static class SilentFoundryConsequenceCatalogLoader
    {
        public const string FileName = "foundry_treaty_consequences.json";

        public static FoundryTreatyConsequenceFile Load(
            string dataDirectory,
            IFileIO files = null!,
            IJsonSerializer serializer = null!)
        {
            files = files ?? new FileSystemIO();
            serializer = serializer ?? new SystemTextJsonSerializer();
            string path = Path.Combine(dataDirectory, FileName);
            if (!files.FileExists(path)) return new FoundryTreatyConsequenceFile();
            string text = files.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(text)) return new FoundryTreatyConsequenceFile();
            try
            {
                return serializer.Deserialize<FoundryTreatyConsequenceFile>(text) ?? new FoundryTreatyConsequenceFile();
            }
            catch (Exception ex_CATDIAG)
                                {
                                    CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                                    return new FoundryTreatyConsequenceFile();
                                }
        }
    }

    /// <summary>
    /// In-memory policy lookup. Validates at load: duplicate (treaty, outcome)
    /// rows, unknown outcomes, missing ids. Static data — no mutable state.
    /// </summary>
    public sealed class SilentFoundryConsequencePolicyCatalog
    {
        private readonly Dictionary<string, FoundryTreatyConsequencePolicy> _byKey =
            new Dictionary<string, FoundryTreatyConsequencePolicy>(StringComparer.Ordinal);

        private readonly List<string> _errors = new List<string>();

        public IReadOnlyList<string> Errors => _errors;
        public bool HasErrors => _errors.Count > 0;
        public int PolicyCount => _byKey.Count;
        public IReadOnlyCollection<FoundryTreatyConsequencePolicy> AllPolicies => _byKey.Values;

        /// <summary>Load + validate. Never throws; errors are collected.</summary>
        public void Load(FoundryTreatyConsequenceFile file)
        {
            _byKey.Clear();
            _errors.Clear();
            if (file?.policies == null) return;

            for (int i = 0; i < file.policies.Count; i++)
            {
                var p = file.policies[i];
                if (p == null) continue;
                if (string.IsNullOrEmpty(p.treaty_id) || string.IsNullOrEmpty(p.faction_id))
                {
                    _errors.Add("policy[" + i + "] missing treaty_id/faction_id");
                    continue;
                }
                if (!IsKnownOutcome(p.outcome))
                {
                    _errors.Add("policy[" + i + "] unknown outcome '" + p.outcome + "' on " + p.treaty_id);
                    continue;
                }
                string key = Key(p.treaty_id, p.outcome);
                if (_byKey.ContainsKey(key))
                {
                    _errors.Add("duplicate policy for " + p.treaty_id + " / " + p.outcome);
                    continue;
                }
                _byKey[key] = p;
            }
        }

        public FoundryTreatyConsequencePolicy? Find(string treatyId, FoundryTreatyOutcome outcome)
        {
            if (string.IsNullOrEmpty(treatyId)) return null;
            string key = Key(treatyId, OutcomeName(outcome));
            return _byKey.TryGetValue(key, out var p) ? p : null;
        }

        public static bool IsKnownOutcome(string outcome)
        {
            for (int i = 0; i < KnownOutcomes.Length; i++)
                if (KnownOutcomes[i] == outcome) return true;
            return false;
        }

        /// <summary>Outcomes that can carry a policy (NotRatified/Pending never do).</summary>
        public static readonly string[] KnownOutcomes = { "met", "missed", "violated" };

        public static string OutcomeName(FoundryTreatyOutcome outcome)
        {
            switch (outcome)
            {
                case FoundryTreatyOutcome.Met: return "met";
                case FoundryTreatyOutcome.Missed: return "missed";
                case FoundryTreatyOutcome.Violated: return "violated";
                default: return string.Empty;
            }
        }

        private static string Key(string treatyId, string outcome) => treatyId + "|" + outcome;
    }
}
