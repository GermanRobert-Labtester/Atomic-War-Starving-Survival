// SPDX-License-Identifier: MIT
// ASHFALL Core: Confession & secret discovery and moral leverage system (Plan 21).

using System;
using System.Collections.Generic;
#pragma warning disable CS8618
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Phantoms
{
    [Serializable]
    public sealed class SecretChoiceRecord
    {
        public string secretId = string.Empty;
        public string choice = string.Empty; // "expose", "blackmail", "keep", "forgive", "grudge"
        public int dayResolved;
    }

    [Serializable]
    public sealed class ConfessionSecretState
    {
        public string systemId = ConfessionSecretSystem.SystemId;
        public List<string> discoveredSecretIds = new List<string>();
        public List<string> resolvedSecretIds = new List<string>();
        public List<SecretChoiceRecord> leverageChoices = new List<SecretChoiceRecord>();
    }

    public sealed class ConfessionSecretSystem
    {
        public const string SystemId = "confession_secret_system";

        private readonly ConfessionSecretCatalog _catalog;
        private readonly ILog _log;
        private readonly HashSet<string> _discovered = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _resolved = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, SecretChoiceRecord> _choices =
            new Dictionary<string, SecretChoiceRecord>(StringComparer.OrdinalIgnoreCase);

        public event Action<string, string>? OnSecretDiscovered; // secretId, sourceId
        public event Action<string, string>? OnSecretExposed;    // secretId, factionId
        public event Action<string, string>? OnSecretBlackmailed;// secretId, resourceGain
        public event Action<string>? OnSecretKept;               // secretId
        public event Action<string, bool>? OnConfessionResolved; // secretId, isForgiven
        public event Action? OnStateChanged;

        public ConfessionSecretSystem(ConfessionSecretCatalog catalog, ILog? log = null)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
            _log = log ?? NullLog.Instance;
        }

        public IReadOnlyCollection<string> DiscoveredSecrets => _discovered;
        public IReadOnlyCollection<string> ResolvedSecrets => _resolved;

        public bool IsDiscovered(string secretId) =>
            !string.IsNullOrEmpty(secretId) && _discovered.Contains(secretId);

        public bool IsResolved(string secretId) =>
            !string.IsNullOrEmpty(secretId) && _resolved.Contains(secretId);

        public SecretChoiceRecord? GetChoice(string secretId)
        {
            if (string.IsNullOrEmpty(secretId)) return null;
            _choices.TryGetValue(secretId, out var choice);
            return choice;
        }

        public bool DiscoverSecret(string secretId, int currentDay, string sourceId = "")
        {
            if (string.IsNullOrEmpty(secretId) || !_catalog.Contains(secretId)) return false;
            if (_discovered.Contains(secretId)) return false;

            _discovered.Add(secretId);
            _log.Info($"[Secret] Discovered {secretId} from {sourceId} on day {currentDay}");
            OnSecretDiscovered?.Invoke(secretId, sourceId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool ExposeSecret(
            string secretId,
            int currentDay,
            NeedsSystem? needs = null,
            GuiltInsomniaSystem? guilt = null,
            Action<string, float>? onFactionStandingChanged = null)
        {
            if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
            var entry = _catalog.GetById(secretId);
            if (entry == null) return false;

            _resolved.Add(secretId);
            _choices[secretId] = new SecretChoiceRecord
            {
                secretId = secretId,
                choice = "expose",
                dayResolved = Math.Max(1, currentDay)
            };

            if (entry.expose_standing_delta != 0 && !string.IsNullOrEmpty(entry.expose_standing_faction))
            {
                onFactionStandingChanged?.Invoke(entry.expose_standing_faction, entry.expose_standing_delta);
            }

            if (entry.expose_guilt_delta > 0 && guilt != null && !string.IsNullOrEmpty(entry.subject_id))
            {
                guilt.RecordGuilt(entry.subject_id, $"secret_exposed_{secretId}", entry.expose_guilt_delta, currentDay);
            }

            _log.Info($"[Secret] Exposed {secretId} on day {currentDay}");
            OnSecretExposed?.Invoke(secretId, entry.expose_standing_faction ?? string.Empty);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool BlackmailSecret(
            string secretId,
            int currentDay,
            MoralBranchingSystem? moral = null)
        {
            if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
            var entry = _catalog.GetById(secretId);
            if (entry == null) return false;

            _resolved.Add(secretId);
            _choices[secretId] = new SecretChoiceRecord
            {
                secretId = secretId,
                choice = "blackmail",
                dayResolved = Math.Max(1, currentDay)
            };

            // Moral hardening consequence
            if (entry.blackmail_hardening_delta > 0 && moral != null && !string.IsNullOrEmpty(entry.subject_id))
            {
                var branchState = moral.GetState(entry.subject_id);
                if (branchState != null)
                {
                    branchState.NumbedResilienceLevel = Math.Min(1.0f, branchState.NumbedResilienceLevel + entry.blackmail_hardening_delta);
                }
            }

            _log.Info($"[Secret] Blackmailed {secretId} on day {currentDay}");
            OnSecretBlackmailed?.Invoke(secretId, entry.blackmail_resource_gain ?? string.Empty);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool KeepSecret(
            string secretId,
            int currentDay,
            SurvivorRelationsSystem? relations = null,
            string confidantSurvivorId = "")
        {
            if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
            var entry = _catalog.GetById(secretId);
            if (entry == null) return false;

            _resolved.Add(secretId);
            _choices[secretId] = new SecretChoiceRecord
            {
                secretId = secretId,
                choice = "keep",
                dayResolved = Math.Max(1, currentDay)
            };

            if (relations != null && !string.IsNullOrEmpty(entry.subject_id) && !string.IsNullOrEmpty(confidantSurvivorId))
            {
                relations.ModifyTrust(entry.subject_id, confidantSurvivorId, entry.keep_trust_delta);
            }

            _log.Info($"[Secret] Kept secret {secretId} on day {currentDay}");
            OnSecretKept?.Invoke(secretId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool ResolveInterpersonal(
            string secretId,
            int currentDay,
            bool forgive,
            string confessorId,
            string listenerId,
            SurvivorRelationsSystem? relations = null,
            NeedsSystem? needs = null)
        {
            if (!IsDiscovered(secretId) || IsResolved(secretId)) return false;
            var entry = _catalog.GetById(secretId);
            if (entry == null) return false;

            _resolved.Add(secretId);
            _choices[secretId] = new SecretChoiceRecord
            {
                secretId = secretId,
                choice = forgive ? "forgive" : "grudge",
                dayResolved = Math.Max(1, currentDay)
            };

            if (relations != null && !string.IsNullOrEmpty(confessorId) && !string.IsNullOrEmpty(listenerId))
            {
                float affinityDelta = forgive ? entry.forgiveness_affinity : entry.grudge_affinity;
                relations.ModifyAffinity(confessorId, listenerId, affinityDelta);
                if (forgive)
                {
                    relations.ModifyTrust(confessorId, listenerId, 15f);
                }
            }

            _log.Info($"[Secret] Resolved interpersonal confession {secretId}: forgive={forgive}");
            OnConfessionResolved?.Invoke(secretId, forgive);
            OnStateChanged?.Invoke();
            return true;
        }

        public ConfessionSecretState CaptureState()
        {
            var state = new ConfessionSecretState { systemId = SystemId };

            var discList = new List<string>(_discovered);
            discList.Sort(string.CompareOrdinal);
            state.discoveredSecretIds = discList;

            var resList = new List<string>(_resolved);
            resList.Sort(string.CompareOrdinal);
            state.resolvedSecretIds = resList;

            var choiceKeys = new List<string>(_choices.Keys);
            choiceKeys.Sort(string.CompareOrdinal);
            for (int i = 0; i < choiceKeys.Count; i++)
            {
                var r = _choices[choiceKeys[i]];
                state.leverageChoices.Add(new SecretChoiceRecord
                {
                    secretId = r.secretId,
                    choice = r.choice,
                    dayResolved = r.dayResolved
                });
            }

            return state;
        }

        public void RestoreState(ConfessionSecretState state)
        {
            if (state == null) return;
            _discovered.Clear();
            _resolved.Clear();
            _choices.Clear();

            if (state.discoveredSecretIds != null)
            {
                for (int i = 0; i < state.discoveredSecretIds.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.discoveredSecretIds[i]))
                        _discovered.Add(state.discoveredSecretIds[i]);
                }
            }

            if (state.resolvedSecretIds != null)
            {
                for (int i = 0; i < state.resolvedSecretIds.Count; i++)
                {
                    if (!string.IsNullOrEmpty(state.resolvedSecretIds[i]))
                        _resolved.Add(state.resolvedSecretIds[i]);
                }
            }

            if (state.leverageChoices != null)
            {
                for (int i = 0; i < state.leverageChoices.Count; i++)
                {
                    var r = state.leverageChoices[i];
                    if (r != null && !string.IsNullOrEmpty(r.secretId))
                    {
                        _choices[r.secretId] = new SecretChoiceRecord
                        {
                            secretId = r.secretId,
                            choice = r.choice ?? string.Empty,
                            dayResolved = r.dayResolved
                        };
                    }
                }
            }

            OnStateChanged?.Invoke();
        }
    }
}
