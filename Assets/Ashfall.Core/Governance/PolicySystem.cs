// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Governance
{
    [Serializable]
    public sealed class PolicyOptionDefinition
    {
        public string id = string.Empty;
        public string label = string.Empty;
        public string description = string.Empty;
        public int attention_cost;
        public int reversal_cost;
        public string effect_type = string.Empty;
        public string effect_payload = string.Empty;
    }

    [Serializable]
    public sealed class PolicyDefinition
    {
        public string id = string.Empty;
        public string scope = string.Empty;
        public string proposer_rules = "open_council"; // "leader_only", "open_council"
        public string decision_method = "executive";    // "executive", "consensus"
        public string default_option_id = string.Empty;
        public List<PolicyOptionDefinition> options = new List<PolicyOptionDefinition>();
    }

    [Serializable]
    public sealed class PolicyCatalogJson
    {
        public int schema_version;
        public List<PolicyDefinition> policies = new List<PolicyDefinition>();
    }

    [Serializable]
    public sealed class PolicyDecisionRecord
    {
        public string scope = string.Empty;
        public string option_id = string.Empty;
        public string proposer_id = string.Empty;
        public int day;
        public string reason = string.Empty;
    }

    [Serializable]
    public sealed class PolicySystemState
    {
        public string systemId = PolicySystem.SystemId;
        public Dictionary<string, string> activePolicies = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        public List<PolicyDecisionRecord> history = new List<PolicyDecisionRecord>();
    }

    /// <summary>
    /// Read-only catalog query service for authored policies.
    /// </summary>
    public sealed class PolicyCatalog
    {
        private readonly Dictionary<string, PolicyDefinition> _byScope =
            new Dictionary<string, PolicyDefinition>(StringComparer.OrdinalIgnoreCase);

        private readonly Dictionary<string, PolicyDefinition> _byId =
            new Dictionary<string, PolicyDefinition>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyCollection<PolicyDefinition> AllPolicies => _byId.Values;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var catalog = serializer.Deserialize<PolicyCatalogJson>(json);
            if (catalog?.policies == null) return;

            foreach (var p in catalog.policies)
            {
                if (p == null || string.IsNullOrEmpty(p.scope)) continue;
                _byScope[p.scope] = p;
                if (!string.IsNullOrEmpty(p.id))
                    _byId[p.id] = p;
            }
        }

        public PolicyDefinition? GetByScope(string scope)
        {
            if (string.IsNullOrEmpty(scope)) return null;
            _byScope.TryGetValue(scope, out var def);
            return def;
        }

        public PolicyDefinition? GetById(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _byId.TryGetValue(id, out var def);
            return def;
        }
    }

    /// <summary>
    /// Plan 43 / C1[13]: Authoritative shelter policy governance engine.
    /// Manages active policy options across scopes (rations, curfew, emergency override)
    /// and ensures changes are validated against leader authority, logged to history,
    /// and dispatched to effect appliers.
    /// </summary>
    public sealed class PolicySystem
    {
        public const string SystemId = "policy_system";

        private readonly PolicyCatalog _catalog;
        private readonly PolicySystemState _state;

        public event Action<string, string, string, int>? OnPolicyChanged; // scope, optionId, proposerId, day

        /// <summary>Optional leadership integration for leader-only policies.</summary>
        public LeadershipSystem? Leadership { get; set; }

        /// <summary>Host effect applier hook (scope, optionId, effectType, effectPayload).</summary>
        public Action<string, string, string, string>? EffectApplier { get; set; }

        public PolicySystem(PolicyCatalog catalog, PolicySystemState? state = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _state = state ?? new PolicySystemState();

            // Initialize defaults from catalog if missing
            foreach (var policy in _catalog.AllPolicies)
            {
                if (!_state.activePolicies.ContainsKey(policy.scope) && !string.IsNullOrEmpty(policy.default_option_id))
                {
                    _state.activePolicies[policy.scope] = policy.default_option_id;
                }
            }
        }

        public IReadOnlyDictionary<string, string> ActivePolicies => _state.activePolicies;
        public IReadOnlyList<PolicyDecisionRecord> History => _state.history;

        public string GetActiveOption(string scope)
        {
            if (string.IsNullOrEmpty(scope)) return string.Empty;
            return _state.activePolicies.TryGetValue(scope, out var opt) ? opt : string.Empty;
        }

        public ActionResult SetPolicy(string scope, string optionId, string proposerId, int day, string? reason = null)
        {
            if (string.IsNullOrEmpty(scope))
                return ActionResult.Blocked("missing_scope", "governance.missing_scope");
            if (string.IsNullOrEmpty(optionId))
                return ActionResult.Blocked("missing_option", "governance.missing_option");

            var def = _catalog.GetByScope(scope);
            if (def == null)
                return ActionResult.Blocked("unknown_policy_scope", "governance.unknown_policy_scope");

            PolicyOptionDefinition? chosenOption = null;
            for (int i = 0; i < def.options.Count; i++)
            {
                if (string.Equals(def.options[i].id, optionId, StringComparison.OrdinalIgnoreCase))
                {
                    chosenOption = def.options[i];
                    break;
                }
            }

            if (chosenOption == null)
                return ActionResult.Blocked("unknown_policy_option", "governance.unknown_policy_option");

            if (_state.activePolicies.TryGetValue(scope, out var current) &&
                string.Equals(current, optionId, StringComparison.OrdinalIgnoreCase))
            {
                return ActionResult.Blocked("already_active", "governance.policy_already_active");
            }

            // Proposer validation: if leader_only, proposer must be designated leader (or no leader exists)
            if (string.Equals(def.proposer_rules, "leader_only", StringComparison.OrdinalIgnoreCase) && Leadership != null)
            {
                string leader = Leadership.CurrentLeaderId;
                if (!string.IsNullOrEmpty(leader) && !string.Equals(leader, proposerId, StringComparison.OrdinalIgnoreCase))
                {
                    return ActionResult.Blocked("leader_only_policy", "governance.leader_only_policy");
                }
            }

            _state.activePolicies[scope] = chosenOption.id;
            var record = new PolicyDecisionRecord
            {
                scope = scope,
                option_id = chosenOption.id,
                proposer_id = proposerId ?? string.Empty,
                day = day,
                reason = reason ?? string.Empty
            };
            _state.history.Add(record);

            OnPolicyChanged?.Invoke(scope, chosenOption.id, proposerId ?? string.Empty, day);
            EffectApplier?.Invoke(scope, chosenOption.id, chosenOption.effect_type, chosenOption.effect_payload);

            return ActionResult.Success("governance.policy_enacted");
        }

        public PolicySystemState CaptureState()
        {
            var copy = new PolicySystemState
            {
                systemId = _state.systemId,
                activePolicies = new Dictionary<string, string>(_state.activePolicies, StringComparer.OrdinalIgnoreCase),
                history = new List<PolicyDecisionRecord>()
            };
            for (int i = 0; i < _state.history.Count; i++)
            {
                var h = _state.history[i];
                if (h == null) continue;
                copy.history.Add(new PolicyDecisionRecord
                {
                    scope = h.scope,
                    option_id = h.option_id,
                    proposer_id = h.proposer_id,
                    day = h.day,
                    reason = h.reason
                });
            }
            return copy;
        }

        public void RestoreState(PolicySystemState saved)
        {
            _state.activePolicies.Clear();
            _state.history.Clear();
            if (saved == null) return;

            if (saved.activePolicies != null)
            {
                foreach (var kvp in saved.activePolicies)
                    _state.activePolicies[kvp.Key] = kvp.Value;
            }
            if (saved.history != null)
            {
                for (int i = 0; i < saved.history.Count; i++)
                {
                    var h = saved.history[i];
                    if (h == null) continue;
                    _state.history.Add(new PolicyDecisionRecord
                    {
                        scope = h.scope,
                        option_id = h.option_id,
                        proposer_id = h.proposer_id,
                        day = h.day,
                        reason = h.reason
                    });
                }
            }
        }
    }
}
