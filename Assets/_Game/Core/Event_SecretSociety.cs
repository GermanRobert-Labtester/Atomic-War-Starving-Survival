using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SecretClique
    {
        public string cliqueId;
        public List<string> memberIds = new List<string>();
        public int formedDay;
        public string name;
    }

    [Serializable]
    public class SabotageRecord
    {
        public string memberId;
        public string targetId;
        public string sabotageType;
        public int day;
    }

    [Serializable]
    public class SecretSocietyState
    {
        public List<SecretClique> cliques = new List<SecretClique>();
        public List<SabotageRecord> sabotageEvents = new List<SabotageRecord>();
    }

    /// <summary>
    /// Prompt #840: Secret Societies — 3+ survivors with high mutual Affinity
    /// form a clique. They prioritize healing/helping each other and
    /// ignore/sabotage outsiders. "Us vs Them."
    /// </summary>
    public class Event_SecretSociety
    {
        /// <summary>
        /// MISC-005: seeded stream backing the default <c>randomFloat</c>. The
        /// parameter exists so hosts can pass a campaign rng for deterministic
        /// replay; the old default reached for wall-clock UnityEngine.Random, so
        /// every caller that omitted it silently opted out of determinism.
        /// </summary>
        private static System.Random _fallbackRng;
    private static System.Random FallbackRng =>
        _fallbackRng ??= AtomicWar._Game.Utilities.SeededRandom.CreateFixed("event_secretsociety");

        private SecretSocietyState _state = new SecretSocietyState();
        private int _nextCliqueIndex;
        private const float AffinityThreshold = 0.7f;
        private const float HealBonus = 0.20f;
        private const float SabotageChance = 0.10f;

        public event Action<string, string[]> OnCliqueFormed;        // cliqueId, members
        public event Action<string, string> OnInsiderHelped;         // helperId, helpedId
        public event Action<string, string> OnOutsiderIgnored;       // memberId, outsiderId
        public event Action<string, string, string> OnSabotage;      // memberId, targetId, sabotageType

        public SecretSocietyState CaptureState() => _state;

        public void RestoreState(SecretSocietyState state)
        {
            _state = state ?? new SecretSocietyState();
            if (_state.cliques == null)
                _state.cliques = new List<SecretClique>();
            if (_state.sabotageEvents == null)
                _state.sabotageEvents = new List<SabotageRecord>();
        }

        /// <summary>
        /// Scans the affinity matrix for groups of 3+ with mutual affinity > 0.7.
        /// Returns true if a new clique was formed.
        /// </summary>
        public bool CheckFormation(List<string> survivorIds, Func<string, string, float> getAffinity)
        {
            // Simple O(n^3) check for cliques of exactly 3 (extendable)
            for (int i = 0; i < survivorIds.Count; i++)
            {
                for (int j = i + 1; j < survivorIds.Count; j++)
                {
                    if (getAffinity(survivorIds[i], survivorIds[j]) <= AffinityThreshold)
                        continue;

                    for (int k = j + 1; k < survivorIds.Count; k++)
                    {
                        if (getAffinity(survivorIds[i], survivorIds[k]) > AffinityThreshold &&
                            getAffinity(survivorIds[j], survivorIds[k]) > AffinityThreshold)
                        {
                            // Check none of them are already in a clique together
                            var candidateIds = new[] { survivorIds[i], survivorIds[j], survivorIds[k] };
                            if (!AlreadyInClique(candidateIds))
                            {
                                FormClique(candidateIds, 0);
                                return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        private bool AlreadyInClique(string[] ids)
        {
            foreach (var clique in _state.cliques)
            {
                bool allPresent = true;
                foreach (var id in ids)
                {
                    if (!clique.memberIds.Contains(id))
                    {
                        allPresent = false;
                        break;
                    }
                }
                if (allPresent) return true;
            }
            return false;
        }

        /// <summary>
        /// Forms a new clique with the given members on the given day.
        /// </summary>
        public string FormClique(string[] memberIds, int day)
        {
            string cliqueId = $"clique_{_nextCliqueIndex++}";
            var clique = new SecretClique
            {
                cliqueId = cliqueId,
                memberIds = new List<string>(memberIds),
                formedDay = day,
                name = $"society_{cliqueId}"
            };
            _state.cliques.Add(clique);

            OnCliqueFormed?.Invoke(cliqueId, memberIds);
            return cliqueId;
        }

        /// <summary>
        /// Daily tick — resolves sabotage attempts against outsiders.
        /// Use a provided random function for deterministic replay.
        /// </summary>
        public void TickDay(Func<float> randomFloat = null)
        {
            Func<float> rng = randomFloat ?? (() => (float)FallbackRng.NextDouble());

            foreach (var clique in _state.cliques)
            {
                foreach (var memberId in clique.memberIds)
                {
                    if (rng() < SabotageChance)
                    {
                        // Pick a random outsider — caller should provide targets via ResolveSabotage
                        var record = new SabotageRecord
                        {
                            memberId = memberId,
                            targetId = string.Empty, // resolved externally
                            sabotageType = rng() < 0.5f ? "steal_meds" : "break_tools",
                            day = 0
                        };
                        _state.sabotageEvents.Add(record);
                    }
                }
            }
        }

        /// <summary>
        /// Returns all member IDs across all cliques.
        /// </summary>
        public List<string> GetCliqueMembers()
        {
            var all = new List<string>();
            foreach (var clique in _state.cliques)
                all.AddRange(clique.memberIds);
            return all;
        }

        /// <summary>
        /// Returns true if memberId should sabotage targetId (member is in a clique, target is not in same clique).
        /// </summary>
        public bool ShouldSabotage(string memberId, string targetId)
        {
            foreach (var clique in _state.cliques)
            {
                if (clique.memberIds.Contains(memberId) && !clique.memberIds.Contains(targetId))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Resolves pending sabotage events, firing events for each.
        /// </summary>
        public void ResolveSabotage(string targetId)
        {
            foreach (var record in _state.sabotageEvents)
            {
                if (string.IsNullOrEmpty(record.targetId))
                {
                    record.targetId = targetId;
                    OnSabotage?.Invoke(record.memberId, record.targetId, record.sabotageType);
                }
            }
        }

        /// <summary>
        /// Fires the insider-helped event when a clique member heals another.
        /// </summary>
        public void ReportInsiderHelp(string helperId, string helpedId)
        {
            foreach (var clique in _state.cliques)
            {
                if (clique.memberIds.Contains(helperId) && clique.memberIds.Contains(helpedId))
                {
                    OnInsiderHelped?.Invoke(helperId, helpedId);
                    return;
                }
            }
        }

        /// <summary>
        /// Fires the outsider-ignored event when a clique member ignores an outsider.
        /// </summary>
        public void ReportOutsiderIgnored(string memberId, string outsiderId)
        {
            foreach (var clique in _state.cliques)
            {
                if (clique.memberIds.Contains(memberId) && !clique.memberIds.Contains(outsiderId))
                {
                    OnOutsiderIgnored?.Invoke(memberId, outsiderId);
                    return;
                }
            }
        }

        /// <summary>
        /// Returns the healing bonus multiplier for clique members helping each other.
        /// </summary>
        public float GetHealBonus() => HealBonus;
    }
}
