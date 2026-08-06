using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BrittleBonesState
    {
        public string survivorId;
        public int daysWithoutSunlight;
        public bool hasBrittleBones;
        public float fractureChance = 0.20f;
    }

    public class BrittleBonesSystem
    {
        private const int SunlightDeprivationDays = 60;

        private readonly Dictionary<string, BrittleBonesState> _states = new Dictionary<string, BrittleBonesState>();

        public IReadOnlyDictionary<string, BrittleBonesState> States => _states;

        public event Action<string> OnBrittleBonesDeveloped;  // survivorId
        public event Action<string, string> OnLaborFracture;  // survivorId, actionType

        private BrittleBonesState GetOrCreate(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state))
            {
                state = new BrittleBonesState
                {
                    survivorId = survivorId,
                    daysWithoutSunlight = 0,
                    hasBrittleBones = false,
                    fractureChance = 0.20f
                };
                _states[survivorId] = state;
            }
            return state;
        }

        public void RecordDayWithoutSunlight(string survivorId)
        {
            var state = GetOrCreate(survivorId);
            state.daysWithoutSunlight++;

            if (state.daysWithoutSunlight >= SunlightDeprivationDays && !state.hasBrittleBones)
            {
                state.hasBrittleBones = true;
                OnBrittleBonesDeveloped?.Invoke(survivorId);
            }
        }

        public void RecordSunlightExposure(string survivorId, float hours)
        {
            var state = GetOrCreate(survivorId);
            state.daysWithoutSunlight = 0;
        }

        /// <summary>
        /// Returns affliction id ("broken_bone") if fracture occurs, null otherwise.
        /// </summary>
        public string TryLaborAction(string survivorId, string actionType, System.Random rng)
        {
            if (!_states.TryGetValue(survivorId, out var state))
                return null;

            if (!state.hasBrittleBones)
                return null;

            if (actionType != "excavate" && actionType != "melee")
                return null;

            if ((float)rng.NextDouble() < state.fractureChance)
            {
                OnLaborFracture?.Invoke(survivorId, actionType);
                return "broken_bone";
            }

            return null;
        }
    }
}
