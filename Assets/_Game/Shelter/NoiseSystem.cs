using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Shelter
{
    /// <summary>Prompt #164 — Noise pollution: generators/construction/guns spike noise, raids path toward it, storms muffle.</summary>
    public class NoiseSystem
    {
        public const float GeneratorNoisePerHour = 0.8f;
        public const float ConstructionNoisePerHour = 1.5f;
        public const float GunfireNoiseSpike = 2f;
        public const float StormMuffleMultiplier = 0.2f;
        public const float MaxNoise = 10f;
        public const float DecayPerHour = 0.5f;

        private float _noiseLevel;
        private PersonalQuestSystem _personalQuests;
        private Func<IReadOnlyList<Survivor>> _getSurvivors;

        public float NoiseLevel => _noiseLevel;
        public float RaidAttractionChance => Mathf.Clamp01(_noiseLevel / MaxNoise);
        public event Action<float> OnNoiseChanged;

        /// <summary>#268 Insomniac night pacing → noise.</summary>
        public void BindPersonalQuests(
            PersonalQuestSystem personalQuests,
            Func<IReadOnlyList<Survivor>> getSurvivors = null)
        {
            _personalQuests = personalQuests;
            _getSurvivors = getSurvivors;
        }

        public void AddNoise(float amount, bool isStormActive)
        {
            float effective = amount * (isStormActive ? StormMuffleMultiplier : 1f);
            SetNoise(Mathf.Min(MaxNoise, _noiseLevel + effective));
        }

        /// <summary>
        /// Add noise attributed to a survivor action. Quiet (#291) contributes zero.
        /// </summary>
        public void AddNoiseFromSurvivor(Survivor sv, float amount, bool isStormActive)
        {
            if (_personalQuests != null && _personalQuests.GeneratesZeroNoise(sv))
                return;
            AddNoise(amount, isStormActive);
        }

        public void Tick(float gameHours)
        {
            if (_noiseLevel > 0f) SetNoise(Mathf.Max(0f, _noiseLevel - DecayPerHour * gameHours));
        }

        /// <summary>
        /// Host night tick: Restless insomniacs pace and generate noise (#268).
        /// The Watcher latent stops generation.
        /// </summary>
        public void TickPersonalQuestNoise(float gameHours, bool isNight, bool isStormActive = false)
        {
            Tick(gameHours);
            if (!isNight || gameHours <= 0f || _personalQuests == null) return;
            var list = _getSurvivors?.Invoke();
            if (list == null) return;
            for (int i = 0; i < list.Count; i++)
            {
                var sv = list[i];
                if (sv == null || !sv.IsAlive) continue;
                float n = _personalQuests.GetNightPaceNoisePerHour(sv);
                if (n > 0f)
                    AddNoise(n * gameHours * 0.1f, isStormActive); // scale 8/hr into 0..10 noise space
            }
        }

        private void SetNoise(float v)
        {
            float o = _noiseLevel;
            _noiseLevel = v;
            if (Mathf.Abs(o - v) > 0.01f) OnNoiseChanged?.Invoke(_noiseLevel);
        }

        public NoiseSave CaptureState() => new NoiseSave { NoiseLevel = _noiseLevel };
        public void RestoreState(NoiseSave s) => _noiseLevel = s?.NoiseLevel ?? 0f;
    }

    [Serializable]
    public class NoiseSave { public float NoiseLevel; }
}
