using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Grief System (Spec #3 of Section VIII). On a survivor's death every
    /// other survivor processes grief based on their InterpersonalAffinity
    /// with the dead. Grief stacks. Three deaths in a week can cascade
    /// into a MentalBreakSystem event for the entire shelter.
    /// The GriefKeepsakeSystem allows survivors to keep one item from the dead.
    /// </summary>
    public class GriefSystem
    {
        public const string AfflictionSurvivorsGuilt = "affliction_survivors_guilt";

        [Serializable]
        public class PerSurvivor
        {
            public string SurvivorId;
            public int DaysOfReducedProductivityRemaining;
            public bool WorkRefusal;
        }

        [Serializable]
        public class State
        {
            public List<PerSurvivor> PerSurvivor = new List<PerSurvivor>();
            public int RecentDeathCount;        // last 7 days
            public int RecentDeathCountResetDay;
        }

        private State _state = new State();
        public State Current => _state;

        public event Action<Survivor, Survivor, float> OnGriefProcessed;  // bereaved, dead, moraleDelta
        public event Action<Survivor> OnWorkRefusalStarted;
        public event Action<Survivor> OnSurvivorsGuiltAfflicted;
        public event Action OnBunkerWideMentalBreakRisk;

        // Host callbacks.
        public Func<float> GetDay;
        public Func<Survivor, Survivor, float> GetAffinity;            // 0..1
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<Survivor, string> AddAffliction;
        public Action<Survivor> MarkProductivityReduced;
        public Action<Survivor> ClearProductivityReduced;
        public System.Random Rng;

        public void OnSurvivorDied(Survivor dead, IReadOnlyList<Survivor> roster)
        {
            if (dead == null || roster == null) return;
            int day = Mathf.FloorToInt(GetDay?.Invoke() ?? 0);
            if (day - _state.RecentDeathCountResetDay > 7)
            {
                _state.RecentDeathCount = 0;
                _state.RecentDeathCountResetDay = day;
            }
            _state.RecentDeathCount++;

            for (int i = 0; i < roster.Count; i++)
            {
                var s = roster[i];
                if (s == null || s == dead || !s.IsAlive) continue;
                float affinity = GetAffinity?.Invoke(s, dead) ?? 0.3f;
                float delta;
                PerSurvivor entry = EnsureEntry(s.Id);
                if (affinity > 0.7f)
                {
                    delta = -25f;
                    entry.WorkRefusal = true;
                    entry.DaysOfReducedProductivityRemaining = Mathf.Max(entry.DaysOfReducedProductivityRemaining, 2);
                    OnWorkRefusalStarted?.Invoke(s);
                    if (Roll(0.25f)) { AddAffliction?.Invoke(s, AfflictionSurvivorsGuilt); OnSurvivorsGuiltAfflicted?.Invoke(s); }
                }
                else if (affinity >= 0.3f)
                {
                    delta = -10f;
                    entry.DaysOfReducedProductivityRemaining = Mathf.Max(entry.DaysOfReducedProductivityRemaining, 1);
                    MarkProductivityReduced?.Invoke(s);
                }
                else if (affinity < 0f)
                {
                    delta = 5f;
                }
                else
                {
                    delta = -3f;
                }
                ApplyMoraleDelta?.Invoke(s, delta);
                OnGriefProcessed?.Invoke(s, dead, delta);
            }

            if (_state.RecentDeathCount >= 3)
            {
                OnBunkerWideMentalBreakRisk?.Invoke();
            }
        }

        public void Tick()
        {
            int day = Mathf.FloorToInt(GetDay?.Invoke() ?? 0);
            if (day - _state.RecentDeathCountResetDay > 7)
            {
                _state.RecentDeathCount = 0;
                _state.RecentDeathCountResetDay = day;
            }
            for (int i = 0; i < _state.PerSurvivor.Count; i++)
            {
                var p = _state.PerSurvivor[i];
                if (p == null) continue;
                if (p.WorkRefusal && p.DaysOfReducedProductivityRemaining == 2)
                {
                    // end of first day of refusal: still refusing, keep flag
                }
                if (p.DaysOfReducedProductivityRemaining > 0) p.DaysOfReducedProductivityRemaining--;
                if (p.DaysOfReducedProductivityRemaining == 0 && p.WorkRefusal)
                {
                    p.WorkRefusal = false;
                    ClearProductivityReduced?.Invoke(null);
                }
                else if (p.DaysOfReducedProductivityRemaining == 0)
                {
                    ClearProductivityReduced?.Invoke(null);
                }
            }
        }

        public State CaptureState() => _state;
        public void RestoreState(State s) { _state = s ?? new State(); }

        public bool IsRefusingWork(string survivorId)
        {
            var p = FindEntry(survivorId);
            return p != null && p.WorkRefusal;
        }

        private PerSurvivor EnsureEntry(string id)
        {
            var p = FindEntry(id);
            if (p != null) return p;
            p = new PerSurvivor { SurvivorId = id };
            _state.PerSurvivor.Add(p);
            return p;
        }

        private PerSurvivor FindEntry(string id)
        {
            for (int i = 0; i < _state.PerSurvivor.Count; i++)
            {
                var p = _state.PerSurvivor[i];
                if (p != null && p.SurvivorId == id) return p;
            }
            return null;
        }

        private bool Roll(float chance)
        {
            if (chance <= 0f) return false;
            if (Rng == null) Rng = new System.Random();
            return Rng.NextDouble() < chance;
        }
    }
}
