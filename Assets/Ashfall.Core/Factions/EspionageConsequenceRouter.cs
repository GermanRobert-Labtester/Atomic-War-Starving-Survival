// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Factions
{
    /// <summary>
    /// Plans 167 MED: routes typed <see cref="EspionageConsequenceIntent"/> once
    /// into named canonical consumers. Fired incident ids are the idempotency
    /// key; consumers own gameplay mutation and their own save sections.
    /// </summary>
    public sealed class EspionageConsequenceRouter
    {
        public const string SupplyDisruption = "consequence_supply_disruption";
        public const string CommunicationsDisruption = "consequence_communications_disruption";
        public const string DefenseReadinessReduced = "consequence_defense_readiness_reduced";

        private readonly HashSet<string> _firedIncidentIds =
            new HashSet<string>(StringComparer.Ordinal);

        private Func<EspionageConsequenceIntent, bool>? _applySupply;
        private Func<EspionageConsequenceIntent, bool>? _applyComms;
        private Func<EspionageConsequenceIntent, bool>? _applyDefense;

        public IReadOnlyCollection<string> FiredIncidentIds => _firedIncidentIds;
        public string LastRouteResult { get; private set; } = string.Empty;

        public void BindConsumers(
            Func<EspionageConsequenceIntent, bool>? applySupply,
            Func<EspionageConsequenceIntent, bool>? applyComms,
            Func<EspionageConsequenceIntent, bool>? applyDefense)
        {
            _applySupply = applySupply;
            _applyComms = applyComms;
            _applyDefense = applyDefense;
        }

        public void Attach(EspionageSystem system)
        {
            if (system == null) throw new ArgumentNullException(nameof(system));
            system.OnConsequenceIntent += intent => TryRoute(intent);
        }

        public void RestoreFired(IEnumerable<string>? incidentIds)
        {
            _firedIncidentIds.Clear();
            if (incidentIds == null) return;
            foreach (var id in incidentIds)
            {
                if (!string.IsNullOrWhiteSpace(id))
                    _firedIncidentIds.Add(id);
            }
        }

        public List<string> CaptureFired()
        {
            var list = new List<string>(_firedIncidentIds.Count);
            foreach (var id in _firedIncidentIds)
                list.Add(id);
            list.Sort(StringComparer.Ordinal);
            return list;
        }

        public bool TryRoute(EspionageConsequenceIntent intent)
        {
            if (intent == null)
            {
                LastRouteResult = "null_intent";
                return false;
            }
            if (string.IsNullOrWhiteSpace(intent.incidentId))
            {
                LastRouteResult = "missing_incident";
                return false;
            }
            if (!_firedIncidentIds.Add(intent.incidentId))
            {
                LastRouteResult = "already_fired";
                return false;
            }

            string type = intent.consequenceType ?? string.Empty;
            // Normalize legacy fallback without consequence_ prefix.
            if (string.Equals(type, "supply_disruption", StringComparison.Ordinal))
                type = SupplyDisruption;

            bool applied;
            if (string.Equals(type, SupplyDisruption, StringComparison.Ordinal))
            {
                applied = _applySupply?.Invoke(intent) == true;
                LastRouteResult = applied ? "supply_applied" : "supply_consumer_missing";
            }
            else if (string.Equals(type, CommunicationsDisruption, StringComparison.Ordinal))
            {
                applied = _applyComms?.Invoke(intent) == true;
                LastRouteResult = applied ? "comms_applied" : "comms_consumer_missing";
            }
            else if (string.Equals(type, DefenseReadinessReduced, StringComparison.Ordinal))
            {
                applied = _applyDefense?.Invoke(intent) == true;
                LastRouteResult = applied ? "defense_applied" : "defense_consumer_missing";
            }
            else
            {
                // Keep fired-set entry so unknown types do not retry forever.
                LastRouteResult = "unknown_consequence_type";
                return false;
            }

            if (!applied)
            {
                // Allow retry next session if consumer was unbound this tick.
                _firedIncidentIds.Remove(intent.incidentId);
                return false;
            }
            return true;
        }
    }
}
