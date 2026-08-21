using System;
using System.Collections.Generic;
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
    public class LeadershipSaveState
    {
        public string current_leader_id;
        public float step_down_cooldown;
        public List<LeadershipSurvivorStateDTO> survivor_states = new List<LeadershipSurvivorStateDTO>();
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

        public string CurrentLeaderId => _currentLeaderId;
        public float StepDownCooldown => _stepDownCooldown;

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

        // ── Simulation callbacks ──────────────────────────────────
        public void OnSurvivorDied(string deadSurvivorId)
        {
            if (string.IsNullOrEmpty(deadSurvivorId)) return;
            if (string.IsNullOrEmpty(_currentLeaderId)) return;
            if (!_states.TryGetValue(_currentLeaderId, out var leader)) return;

            // Leader must be alive
            var alive = GetAliveSurvivorIds?.Invoke();
            if (alive == null || !ContainsId(alive, _currentLeaderId)) return;

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
            };
            foreach (var kv in _states)
            {
                save.survivor_states.Add(new LeadershipSurvivorStateDTO
                {
                    survivor_id = kv.Key,
                    is_designated_leader = kv.Value.IsDesignatedLeader,
                    leader_stress_accumulation = kv.Value.LeaderStressAccumulation,
                    leader_deaths_witnessed = kv.Value.LeaderDeathsWitnessed,
                });
            }
            return save;
        }

        public void RestoreState(LeadershipSaveState save)
        {
            _states.Clear();
            _currentLeaderId = null!;
            _stepDownCooldown = 0f;

            if (save != null)
            {
                _currentLeaderId = save.current_leader_id;
                _stepDownCooldown = save.step_down_cooldown;
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

            OnStateChanged?.Invoke();
        }

        // ── Helpers ───────────────────────────────────────────────
        private static bool ContainsId(IReadOnlyList<string> list, string id)
        {
            for (int i = 0; i < list.Count; i++)
                if (list[i] == id) return true;
            return false;
        }
    }
}
