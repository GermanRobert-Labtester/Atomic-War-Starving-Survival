using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Simulation
{
    /// <summary>
    /// Calorie Accounting (Spec #7 of Section VIII). Replaces abstract Hunger
    /// with actual caloric tracking per survivor. Each survivor needs 1800
    /// kcal/day (light), 2400 (heavy), 3000 (expedition). Below thresholds
    /// the survivor weakens, becomes malnourished, starves, then dies in 3 days.
    /// </summary>
    public class CalorieAccountingSystem
    {
        public const float LightWorkKcal = 1800f;
        public const float HeavyWorkKcal = 2400f;
        public const float ExpeditionKcal = 3000f;
        public const float WeakenedBelowKcal = 1200f;
        public const float MalnourishedBelowKcal = 800f;
        public const float StarvingBelowKcal = 400f;
        public const float StarvationDaysUntilDeath = 3f;

        // Caloric content of food items. Extend at the catalog import level.
        public static readonly Dictionary<string, float> KcalPerItem = new Dictionary<string, float>
        {
            { "canned_food",          450f },
            { "cooked_meat",          600f },
            { "rat_meat_skewer",      180f },
            { "insect_paste_brick",   250f },
            { "ration_crackers",      350f },
            { "dried_fruit",          200f },
            { "powdered_eggs",        300f },
            { "mystery_meat",         400f },
            { "sugar",                 50f },
        };

        public enum Workload { Light, Heavy, Expedition }

        [Serializable]
        public class State
        {
            public string SurvivorId;
            public float KcalToday;            // running total consumed today
            public float KcalYesterday;
            public float DailyTarget;
            public Workload Workload;
            public int DaysAtZero;
            public int LastSimulatedDay = -1;
        }

        private readonly Dictionary<string, State> _bySurvivor = new Dictionary<string, State>();
        public event Action<Survivor, float> OnKcalConsumed;     // sv, kcal
        public event Action<Survivor> OnWeakened;
        public event Action<Survivor> OnMalnourished;
        public event Action<Survivor> OnStarving;
        public event Action<Survivor> OnStarvationDeath;

        public State GetOrCreate(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (!_bySurvivor.TryGetValue(survivorId, out var s))
            {
                s = new State { SurvivorId = survivorId, DailyTarget = LightWorkKcal, Workload = Workload.Light };
                _bySurvivor[survivorId] = s;
            }
            return s;
        }

        public void SetWorkload(Survivor sv, Workload w)
        {
            if (sv == null) return;
            var s = GetOrCreate(sv.Id);
            s.Workload = w;
            s.DailyTarget = w == Workload.Expedition ? ExpeditionKcal : w == Workload.Heavy ? HeavyWorkKcal : LightWorkKcal;
        }

        public void Consume(Survivor sv, string itemId, int count = 1)
        {
            if (sv == null || string.IsNullOrEmpty(itemId)) return;
            float kcal = KcalPerItem.TryGetValue(itemId, out var k) ? k : 0f;
            var s = GetOrCreate(sv.Id);
            s.KcalToday += kcal * count;
            OnKcalConsumed?.Invoke(sv, kcal * count);
        }

        public void Tick(Survivor sv, int currentDay)
        {
            if (sv == null || !sv.IsAlive) return;
            var s = GetOrCreate(sv.Id);
            if (currentDay != s.LastSimulatedDay)
            {
                s.KcalYesterday = s.KcalToday;
                s.KcalToday = 0f;
                s.LastSimulatedDay = currentDay;
            }
            if (s.KcalYesterday < StarvingBelowKcal)
            {
                s.DaysAtZero++;
                if (s.DaysAtZero >= Mathf.CeilToInt(StarvationDaysUntilDeath))
                {
                    OnStarvationDeath?.Invoke(sv);
                    return;
                }
            }
            else s.DaysAtZero = 0;

            if (s.KcalYesterday < StarvingBelowKcal) OnStarving?.Invoke(sv);
            else if (s.KcalYesterday < MalnourishedBelowKcal) OnMalnourished?.Invoke(sv);
            else if (s.KcalYesterday < WeakenedBelowKcal) OnWeakened?.Invoke(sv);
        }

        public bool IsWeakened(Survivor sv) => GetKcalYesterday(sv) < WeakenedBelowKcal;
        public bool IsMalnourished(Survivor sv) => GetKcalYesterday(sv) < MalnourishedBelowKcal;
        public bool IsStarving(Survivor sv) => GetKcalYesterday(sv) < StarvingBelowKcal;

        public float GetKcalToday(Survivor sv) => GetOrCreate(sv?.Id)?.KcalToday ?? 0f;
        public float GetKcalYesterday(Survivor sv) => GetOrCreate(sv?.Id)?.KcalYesterday ?? 0f;
        public float DailyTarget(Survivor sv) => GetOrCreate(sv?.Id)?.DailyTarget ?? LightWorkKcal;

        public List<State> CaptureState() => new List<State>(_bySurvivor.Values);
        public void RestoreState(List<State> states)
        {
            _bySurvivor.Clear();
            if (states == null) return;
            for (int i = 0; i < states.Count; i++)
            {
                var s = states[i];
                if (s == null || string.IsNullOrEmpty(s.SurvivorId)) continue;
                _bySurvivor[s.SurvivorId] = s;
            }
        }
    }
}
