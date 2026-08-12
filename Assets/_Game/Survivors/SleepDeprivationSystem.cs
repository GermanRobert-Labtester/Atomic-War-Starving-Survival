using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Sleep Deprivation (Spec #1 of Section VIII). Survivors need 6 h of sleep
    /// per 24 h cycle. Missing nights accumulate a SleepDebt counter and progress
    /// through four degrading bands: Tired, Hallucinating, Microsleep, Collapsed.
    /// Quality is governed by bed type, room temperature, ambient noise, and
    /// audible neighbour distress.
    /// </summary>
    public class SleepDeprivationSystem
    {
        public const int RequiredSleepHoursPerCycle = 6;

        public static readonly Dictionary<string, float> BedRecoveryFraction = new Dictionary<string, float>
        {
            { "improvised_rollup_bed", 0.60f },
            { "woolbed",               0.85f },
            { "advanced_heating_bed",  1.00f },
            { "floor",                 0.40f },
        };

        [Serializable]
        public class State
        {
            public string SurvivorId;
            public int MissedNights;
            public int ConsecutiveMissedNights;
            public float LastSleepHours;
            public int LastCycleDay = -1;
            public bool Hallucinating;
            public bool Collapsed;
            public float CollapseUntilCycleHour;
            public float WorkAccidentChance;
        }

        private readonly Dictionary<string, State> _bySurvivor = new Dictionary<string, State>();

        public event Action<Survivor, int> OnMissedNightIncremented;
        public event Action<Survivor> OnHallucinationsBegan;
        public event Action<Survivor> OnMicrosleepRiskChanged;
        public event Action<Survivor> OnCognitiveCollapse;
        public event Action<Survivor> OnRecovered;
        public event Action<Survivor, string> OnWorkAccident;

        public Func<Survivor, string> GetBedTypeIdForSurvivor;
        public Func<Survivor, float> GetRoomTemperatureForSurvivor;
        public Func<float> GetShelterNoiseLevel;
        public Func<Survivor, bool> IsNeighbourAudibleDistressed;
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<Survivor, string> ApplyHallucinationFlag;
        public Action<Survivor, string, float> MarkSkillAccuracy;

        public State GetOrCreate(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return null;
            if (!_bySurvivor.TryGetValue(survivorId, out var s))
            {
                s = new State { SurvivorId = survivorId };
                _bySurvivor[survivorId] = s;
            }
            return s;
        }

        public State Get(string survivorId) =>
            string.IsNullOrEmpty(survivorId) || !_bySurvivor.TryGetValue(survivorId, out var s) ? null : s;

        public IEnumerable<State> All => _bySurvivor.Values;

        public void OnEndOfDay(Survivor sv, int currentDay, float currentCycleHour, float hoursSleptLastNight)
        {
            if (sv == null || !sv.IsAlive) return;
            var st = GetOrCreate(sv.Id);
            st.LastSleepHours = hoursSleptLastNight;
            st.LastCycleDay = currentDay;

            float bedFrac = ResolveBedFraction(sv);
            float noise = 0f;
            if (GetShelterNoiseLevel != null) noise = GetShelterNoiseLevel();
            float temp = 0.5f;
            if (GetRoomTemperatureForSurvivor != null) temp = GetRoomTemperatureForSurvivor(sv);
            bool neighbourBad = false;
            if (IsNeighbourAudibleDistressed != null) neighbourBad = IsNeighbourAudibleDistressed(sv);
            float penalty = (noise > 60f ? 0.2f : 0f) + (Mathf.Abs(temp - 0.5f) * 0.3f) + (neighbourBad ? 0.3f : 0f);
            float effectiveHours = Mathf.Max(0f, hoursSleptLastNight * bedFrac * (1f - Mathf.Clamp01(penalty)));

            if (effectiveHours >= RequiredSleepHoursPerCycle)
            {
                st.ConsecutiveMissedNights = 0;
                st.Hallucinating = false;
                st.WorkAccidentChance = 0f;
                if (st.Collapsed && currentCycleHour >= st.CollapseUntilCycleHour)
                {
                    st.Collapsed = false;
                }
                OnRecovered?.Invoke(sv);
                return;
            }

            st.MissedNights++;
            st.ConsecutiveMissedNights++;
            OnMissedNightIncremented?.Invoke(sv, st.MissedNights);

            if (st.ConsecutiveMissedNights >= 1)
            {
                ApplyMoraleDelta?.Invoke(sv, -10f);
                MarkSkillAccuracy?.Invoke(sv, "all", -0.05f);
            }
            if (st.ConsecutiveMissedNights >= 2 && !st.Hallucinating)
            {
                st.Hallucinating = true;
                st.WorkAccidentChance = 0f;
                OnHallucinationsBegan?.Invoke(sv);
                ApplyHallucinationFlag?.Invoke(sv, "sleep_deprivation");
                ApplyMoraleDelta?.Invoke(sv, -20f);
                MarkSkillAccuracy?.Invoke(sv, "all", -0.15f);
            }
            if (st.ConsecutiveMissedNights >= 3)
            {
                st.WorkAccidentChance = 0.20f;
                OnMicrosleepRiskChanged?.Invoke(sv);
            }
            if (st.ConsecutiveMissedNights >= 4 && !st.Collapsed)
            {
                st.Collapsed = true;
                st.CollapseUntilCycleHour = currentCycleHour + 24f;
                OnCognitiveCollapse?.Invoke(sv);
            }
        }

        public bool RollWorkAccident(Survivor sv, string workActionId, System.Random rng)
        {
            if (sv == null || !sv.IsAlive) return false;
            var st = Get(sv.Id);
            if (st == null || st.WorkAccidentChance <= 0f) return false;
            // Use the seeded stream, not `new System.Random()` — the parameterless
            // ctor is time-seeded with low resolution (rapid calls share a seed)
            // and breaks save replayability for a seed-driven game. Matches the
            // fallback pattern in CombatPerkSystem / BunkerSocialSystems.
            if (rng == null) rng = AtomicWar._Game.Utilities.SeededRandom.Stream("sleep_work_accident");
            if (rng.NextDouble() < st.WorkAccidentChance)
            {
                OnWorkAccident?.Invoke(sv, workActionId);
                return true;
            }
            return false;
        }

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

        private float ResolveBedFraction(Survivor sv)
        {
            if (GetBedTypeIdForSurvivor == null) return 1.0f;
            string bedId = GetBedTypeIdForSurvivor(sv) ?? "bed";
            if (string.IsNullOrEmpty(bedId)) bedId = "bed";
            if (BedRecoveryFraction.TryGetValue(bedId, out float f)) return f;
            return 1.0f;
        }

        public float GetSkillAccuracyPenalty(Survivor sv)
        {
            var st = Get(sv?.Id);
            if (st == null) return 0f;
            if (st.ConsecutiveMissedNights >= 4) return 0.40f;
            if (st.ConsecutiveMissedNights >= 2) return 0.15f;
            if (st.ConsecutiveMissedNights >= 1) return 0.05f;
            return 0f;
        }

        public bool IsHallucinating(Survivor sv) => Get(sv?.Id)?.Hallucinating ?? false;
        public bool IsCollapsed(Survivor sv) => Get(sv?.Id)?.Collapsed ?? false;
    }
}
