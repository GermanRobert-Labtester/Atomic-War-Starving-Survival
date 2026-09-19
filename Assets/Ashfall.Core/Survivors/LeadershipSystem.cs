// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Globalization;
#pragma warning disable CS8618
using System.Linq;

namespace Ashfall.Core.Survivors
{
    // ── Save-state DTOs ───────────────────────────────────────────
    [Serializable]
    public class LeadershipSurvivorStateDTO
    {
        public string survivor_id;
        public bool is_designated_leader;
        public float leader_stress_accumulation;
        public int leader_deaths_witnessed;
    }

    [Serializable]
    public class LeadershipChallengeDTO
    {
        public string challenge_id;
        public string challenger_id;
        public string challenged_leader_id;
        public string reason;
        public bool is_resolved;
        public bool challenger_won;
    }

    [Serializable]
    public class LeadershipSaveState
    {
        public string current_leader_id;
        public float step_down_cooldown;
        public string designated_successor_id;
        public string deputy_leader_id;
        public List<LeadershipSurvivorStateDTO> survivor_states = new List<LeadershipSurvivorStateDTO>();
        public List<LeadershipChallengeDTO> challenges = new List<LeadershipChallengeDTO>();
    }

    /// <summary>
    /// Leadership System — designating an informal bunker leader grants morale
    /// bonuses during crises but accumulates personal stress when deaths or
    /// severe injuries occur. At max stress: 3× mental-break risk.
    ///
    /// Engine-agnostic. All survivor state is owned internally, keyed by
    /// string survivor IDs.
    /// </summary>
    public class LeadershipSystem
    {
        public const float LeaderCrisisMoraleAura = 10f;
        public const float LeaderStressPerDeath = 25f;
        public const float LeaderStressPerInjury = 10f;
        public const float LeaderStressDecayPerDay = 2f;
        public const float LeaderStressMax = 100f;
        public const float LeaderBreakRiskMultiplier = 3f;
        public const float StepDownCooldownDays = 14f;

        // ── Events ────────────────────────────────────────────────
        public event Action<string> OnLeaderDesignated;
        public event Action<string> OnLeaderSteppedDown;
        public event Action<string, float> OnLeaderStressIncreased;
        public event Action<string> OnLeaderBreakRisk;
        public event Action<string, string> OnSuccessionTriggered;
        public event Action<LeadershipChallengeDTO> OnChallengeInitiated;
        public event Action<LeadershipChallengeDTO> OnChallengeResolved;
        public event Action OnStateChanged;

        // ── Host hooks ────────────────────────────────────────────
        public Action<string, float> ApplyMoraleDelta;
        public Action<float> ApplyShelterMoraleDelta;
        public Func<IReadOnlyList<string>> GetAliveSurvivorIds;

        // ── Internal state ────────────────────────────────────────
        private readonly Dictionary<string, SurvivorState> _states =
            new Dictionary<string, SurvivorState>();

        private string _currentLeaderId;
        private float _stepDownCooldown;
        private string _designatedSuccessorId;
        private string _deputyLeaderId;
        private readonly List<LeadershipChallengeDTO> _challenges = new List<LeadershipChallengeDTO>();
        private int _nextChallengeSeq = 1;

        public string CurrentLeaderId => _currentLeaderId;
        public float StepDownCooldown => _stepDownCooldown;
        public string DesignatedSuccessorId => _designatedSuccessorId;
        public string DeputyLeaderId => _deputyLeaderId;
        public IReadOnlyList<LeadershipChallengeDTO> Challenges => _challenges;

        // ── Per-survivor state ────────────────────────────────────
        private class SurvivorState
        {
            public bool IsDesignatedLeader;
            public float LeaderStressAccumulation;
            public int LeaderDeathsWitnessed;
        }

        private SurvivorState GetOrAdd(string id)
        {
            if (!_states.TryGetValue(id, out var s))
            {
                s = new SurvivorState();
                _states[id] = s;
            }
            return s;
        }

        // ── Public queries ────────────────────────────────────────
        public bool IsDesignatedLeader(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var s) && s.IsDesignatedLeader;
        }

        public float GetLeaderStress(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var s)
                ? s.LeaderStressAccumulation : 0f;
        }

        public int GetDeathsWitnessed(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var s)
                ? s.LeaderDeathsWitnessed : 0;
        }

        // ── Designate / step-down ─────────────────────────────────
        public bool DesignateLeader(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (_stepDownCooldown > 0f) return false;

            // Verify survivor is alive (host provides the list)
            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, survivorId)) return false;

            // Clear previous leader
            if (!string.IsNullOrEmpty(_currentLeaderId)
                && _states.TryGetValue(_currentLeaderId, out var prev))
            {
                prev.IsDesignatedLeader = false;
                prev.LeaderStressAccumulation = 0f;
            }

            var st = GetOrAdd(survivorId);
            st.IsDesignatedLeader = true;
            _currentLeaderId = survivorId;

            OnLeaderDesignated?.Invoke(survivorId);
            OnStateChanged?.Invoke();
            return true;
        }

        public bool StepDown(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return false;
            if (survivorId != _currentLeaderId) return false;

            var st = GetOrAdd(survivorId);
            st.IsDesignatedLeader = false;
            _currentLeaderId = null!;
            _stepDownCooldown = StepDownCooldownDays;

            OnLeaderSteppedDown?.Invoke(survivorId);
            OnStateChanged?.Invoke();
            return true;
        }

        // ── Succession & Challenge (Plan 208) ───────────────────────
        public bool DesignateSuccessor(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                _designatedSuccessorId = null!;
                OnStateChanged?.Invoke();
                return true;
            }

            if (string.IsNullOrEmpty(_currentLeaderId)
                || string.Equals(_currentLeaderId, survivorId, StringComparison.Ordinal)
                || !IsAlive(survivorId)) return false;

            _designatedSuccessorId = survivorId;
            OnStateChanged?.Invoke();
            return true;
        }

        public bool AppointDeputy(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                _deputyLeaderId = null!;
                OnStateChanged?.Invoke();
                return true;
            }

            if (string.IsNullOrEmpty(_currentLeaderId)
                || string.Equals(_currentLeaderId, survivorId, StringComparison.Ordinal)
                || !IsAlive(survivorId)) return false;

            _deputyLeaderId = survivorId;
            OnStateChanged?.Invoke();
            return true;
        }

        public LeadershipChallengeDTO? InitiateChallenge(string challengerId, string reason)
        {
            if (string.IsNullOrEmpty(challengerId)
                || string.IsNullOrEmpty(_currentLeaderId)
                || string.Equals(challengerId, _currentLeaderId, StringComparison.Ordinal)
                || !IsAlive(challengerId)) return null;
            if (_challenges.Any(c => !c.is_resolved
                && string.Equals(c.challenger_id, challengerId, StringComparison.Ordinal))) return null;

            var dto = new LeadershipChallengeDTO
            {
                challenge_id = $"chl_{_nextChallengeSeq++}",
                challenger_id = challengerId,
                challenged_leader_id = _currentLeaderId,
                reason = string.IsNullOrWhiteSpace(reason) ? "Challenge for leadership" : reason,
                is_resolved = false,
                challenger_won = false
            };
            _challenges.Add(dto);
            OnChallengeInitiated?.Invoke(dto);
            OnStateChanged?.Invoke();
            return dto;
        }

        public bool ResolveChallenge(string challengeId, bool challengerWon)
        {
            var ch = _challenges.FirstOrDefault(c => c.challenge_id == challengeId);
            if (ch == null || ch.is_resolved) return false;

            // A challenge cannot displace a later leader after its original
            // target has already died, stepped down, or lost another contest.
            if (!string.Equals(ch.challenged_leader_id, _currentLeaderId, StringComparison.Ordinal))
            {
                ch.is_resolved = true;
                ch.challenger_won = false;
                OnChallengeResolved?.Invoke(ch);
                OnStateChanged?.Invoke();
                return true;
            }
            if (challengerWon && !IsAlive(ch.challenger_id)) return false;

            ch.is_resolved = true;
            ch.challenger_won = challengerWon;

            if (challengerWon)
            {
                string prevLeader = _currentLeaderId;
                if (!string.IsNullOrEmpty(prevLeader) && _states.TryGetValue(prevLeader, out var prev))
                {
                    prev.IsDesignatedLeader = false;
                }
                _currentLeaderId = ch.challenger_id;
                var newLeader = GetOrAdd(ch.challenger_id);
                newLeader.IsDesignatedLeader = true;
                if (string.Equals(_designatedSuccessorId, ch.challenger_id, StringComparison.Ordinal))
                    _designatedSuccessorId = null!;
                if (string.Equals(_deputyLeaderId, ch.challenger_id, StringComparison.Ordinal))
                    _deputyLeaderId = null!;
                OnLeaderDesignated?.Invoke(ch.challenger_id);

                for (int i = 0; i < _challenges.Count; i++)
                {
                    var other = _challenges[i];
                    if (ReferenceEquals(other, ch) || other.is_resolved) continue;
                    if (!string.Equals(other.challenged_leader_id, prevLeader, StringComparison.Ordinal)) continue;
                    other.is_resolved = true;
                    other.challenger_won = false;
                    OnChallengeResolved?.Invoke(other);
                }
            }

            OnChallengeResolved?.Invoke(ch);
            OnStateChanged?.Invoke();
            return true;
        }

        // ── Simulation callbacks ──────────────────────────────────
        public void OnSurvivorDied(string deadSurvivorId)
        {
            if (string.IsNullOrEmpty(deadSurvivorId)) return;

            bool changed = false;
            if (string.Equals(_designatedSuccessorId, deadSurvivorId, StringComparison.Ordinal))
            {
                _designatedSuccessorId = null!;
                changed = true;
            }
            if (string.Equals(_deputyLeaderId, deadSurvivorId, StringComparison.Ordinal))
            {
                _deputyLeaderId = null!;
                changed = true;
            }
            for (int i = 0; i < _challenges.Count; i++)
            {
                var challenge = _challenges[i];
                if (challenge.is_resolved
                    || !string.Equals(challenge.challenger_id, deadSurvivorId, StringComparison.Ordinal)) continue;
                challenge.is_resolved = true;
                challenge.challenger_won = false;
                OnChallengeResolved?.Invoke(challenge);
                changed = true;
            }

            // The leader died: vacate the position immediately. Without this,
            // CurrentLeaderId dangles at a dead survivor and every later death
            // exits at the "leader must be alive" guard below, so the camp
            // would keep a corpse in charge and no succession can ever occur.
            if (string.Equals(_currentLeaderId, deadSurvivorId, StringComparison.Ordinal))
            {
                if (_states.TryGetValue(_currentLeaderId, out var fallen))
                    fallen.IsDesignatedLeader = false;
                _currentLeaderId = null!;
                OnLeaderSteppedDown?.Invoke(deadSurvivorId);

                for (int i = 0; i < _challenges.Count; i++)
                {
                    var challenge = _challenges[i];
                    if (challenge.is_resolved
                        || !string.Equals(challenge.challenged_leader_id, deadSurvivorId, StringComparison.Ordinal)) continue;
                    challenge.is_resolved = true;
                    challenge.challenger_won = false;
                    OnChallengeResolved?.Invoke(challenge);
                }

                // Plan 208 Succession: check designated successor, then deputy
                string candidate = _designatedSuccessorId;
                var aliveList = GetAliveSurvivorIds?.Invoke();

                if (string.IsNullOrEmpty(candidate) || (aliveList != null && !ContainsId(aliveList, candidate)))
                {
                    candidate = _deputyLeaderId;
                }

                if (!string.IsNullOrEmpty(candidate) && (aliveList == null || ContainsId(aliveList, candidate)))
                {
                    var successor = GetOrAdd(candidate);
                    successor.IsDesignatedLeader = true;
                    _currentLeaderId = candidate;
                    if (candidate == _designatedSuccessorId) _designatedSuccessorId = null!;
                    if (candidate == _deputyLeaderId) _deputyLeaderId = null!;
                    OnSuccessionTriggered?.Invoke(deadSurvivorId, candidate);
                    OnLeaderDesignated?.Invoke(candidate);
                }

                OnStateChanged?.Invoke();
                return;
            }

            if (string.IsNullOrEmpty(_currentLeaderId))
            {
                if (changed) OnStateChanged?.Invoke();
                return;
            }
            if (!_states.TryGetValue(_currentLeaderId, out var leader))
            {
                if (changed) OnStateChanged?.Invoke();
                return;
            }

            // Leader must be alive
            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, _currentLeaderId))
            {
                if (changed) OnStateChanged?.Invoke();
                return;
            }

            leader.LeaderStressAccumulation = MathfCompat.Min(
                LeaderStressMax,
                leader.LeaderStressAccumulation + LeaderStressPerDeath);
            leader.LeaderDeathsWitnessed++;

            OnLeaderStressIncreased?.Invoke(_currentLeaderId, leader.LeaderStressAccumulation);
            OnStateChanged?.Invoke();

            if (leader.LeaderStressAccumulation >= LeaderStressMax)
                OnLeaderBreakRisk?.Invoke(_currentLeaderId);
        }

        public void OnSurvivorInjured(string injuredSurvivorId)
        {
            if (string.IsNullOrEmpty(injuredSurvivorId)) return;
            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            if (!_states.TryGetValue(_currentLeaderId, out var leader)) return;

            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, _currentLeaderId)) return;

            leader.LeaderStressAccumulation = MathfCompat.Min(
                LeaderStressMax,
                leader.LeaderStressAccumulation + LeaderStressPerInjury);

            OnLeaderStressIncreased?.Invoke(_currentLeaderId, leader.LeaderStressAccumulation);
            OnStateChanged?.Invoke();

            if (leader.LeaderStressAccumulation >= LeaderStressMax)
                OnLeaderBreakRisk?.Invoke(_currentLeaderId);
        }

        public void OnCrisisEvent()
        {
            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            if (!_states.TryGetValue(_currentLeaderId, out _)) return;

            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, _currentLeaderId)) return;

            ApplyShelterMoraleDelta?.Invoke(LeaderCrisisMoraleAura);
        }

        public void Tick(float gameHours)
        {
            if (_stepDownCooldown > 0f)
                _stepDownCooldown = MathfCompat.Max(0f,
                    _stepDownCooldown - gameHours / 24f);

            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            if (!_states.TryGetValue(_currentLeaderId, out var leader)) return;

            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, _currentLeaderId)) return;

            float prev = leader.LeaderStressAccumulation;
            leader.LeaderStressAccumulation = MathfCompat.Max(0f,
                leader.LeaderStressAccumulation -
                LeaderStressDecayPerDay * (gameHours / 24f));

            if (!MathfCompat.Approximately(prev, leader.LeaderStressAccumulation))
                OnStateChanged?.Invoke();
        }

        // ── Save / Load ───────────────────────────────────────────
        public LeadershipSaveState CaptureState()
        {
            var save = new LeadershipSaveState
            {
                current_leader_id = _currentLeaderId,
                step_down_cooldown = _stepDownCooldown,
                designated_successor_id = _designatedSuccessorId,
                deputy_leader_id = _deputyLeaderId,
            };
            foreach (var kv in _states.OrderBy(pair => pair.Key, StringComparer.Ordinal))
            {
                save.survivor_states.Add(new LeadershipSurvivorStateDTO
                {
                    survivor_id = kv.Key,
                    is_designated_leader = kv.Value.IsDesignatedLeader,
                    leader_stress_accumulation = kv.Value.LeaderStressAccumulation,
                    leader_deaths_witnessed = kv.Value.LeaderDeathsWitnessed,
                });
            }
            foreach (var ch in _challenges)
            {
                save.challenges.Add(new LeadershipChallengeDTO
                {
                    challenge_id = ch.challenge_id,
                    challenger_id = ch.challenger_id,
                    challenged_leader_id = ch.challenged_leader_id,
                    reason = ch.reason,
                    is_resolved = ch.is_resolved,
                    challenger_won = ch.challenger_won
                });
            }
            return save;
        }

        public void RestoreState(LeadershipSaveState save)
        {
            _states.Clear();
            _currentLeaderId = null!;
            _stepDownCooldown = 0f;
            _designatedSuccessorId = null!;
            _deputyLeaderId = null!;
            _challenges.Clear();
            _nextChallengeSeq = 1;

            if (save != null)
            {
                _currentLeaderId = save.current_leader_id;
                _stepDownCooldown = save.step_down_cooldown;
                _designatedSuccessorId = save.designated_successor_id;
                _deputyLeaderId = save.deputy_leader_id;
                if (save.survivor_states != null)
                {
                    foreach (var dto in save.survivor_states)
                    {
                        _states[dto.survivor_id] = new SurvivorState
                        {
                            IsDesignatedLeader = dto.is_designated_leader,
                            LeaderStressAccumulation = dto.leader_stress_accumulation,
                            LeaderDeathsWitnessed = dto.leader_deaths_witnessed,
                        };
                    }
                }
                if (save.challenges != null)
                {
                    foreach (var ch in save.challenges)
                    {
                        _challenges.Add(new LeadershipChallengeDTO
                        {
                            challenge_id = ch.challenge_id,
                            challenger_id = ch.challenger_id,
                            challenged_leader_id = string.IsNullOrEmpty(ch.challenged_leader_id)
                                ? _currentLeaderId
                                : ch.challenged_leader_id,
                            reason = ch.reason,
                            is_resolved = ch.is_resolved,
                            challenger_won = ch.challenger_won
                        });
                        AdvanceChallengeSequence(ch.challenge_id);
                    }
                }
            }

            OnStateChanged?.Invoke();
        }

        // ── Helpers ───────────────────────────────────────────────
        private static bool ContainsId(IReadOnlyList<string> list, string id)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == id) return true;
            return false;
        }

        private bool IsAlive(string survivorId)
        {
            var alive = GetAliveSurvivorIds?.Invoke();
            return alive != null && ContainsId(alive, survivorId);
        }

        private void AdvanceChallengeSequence(string challengeId)
        {
            if (string.IsNullOrEmpty(challengeId) || !challengeId.StartsWith("chl_", StringComparison.Ordinal)) return;
            if (int.TryParse(challengeId.Substring(4), NumberStyles.None, CultureInfo.InvariantCulture, out int value)
                && value >= _nextChallengeSeq)
                _nextChallengeSeq = value + 1;
        }
    }
}
