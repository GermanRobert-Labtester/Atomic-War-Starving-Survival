using System;
using System.Collections.Generic;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Prompt #11 — Black Rain: oily hyper-radioactive weather that melts hazmat,
    /// ruins open catchment (handled by WaterEconomySystem), and applies Dread
    /// to outdoor scavengers and hatch listeners.
    /// </summary>
    public class BlackRainHazardSystem
    {
        /// <summary>Morale lost per game-hour while Dread is active.</summary>
        public const float DreadMoraleDrainPerHour = 4f;

        private readonly WeatherSystem _weather;

        public event Action<Survivor> OnDreadApplied;
        public event Action<Survivor> OnDreadCleared;

        public BlackRainHazardSystem(WeatherSystem weather)
        {
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
        }

        public bool IsActive => _weather != null && _weather.Current == WeatherKind.BlackRain;

        /// <summary>
        /// Hazmat durability multiplier for outdoor gear ticks.
        /// BlackRain = <see cref="WeatherSystem.BlackRainHazmatMeltMultiplier"/>; else 1.
        /// </summary>
        public float GetHazmatDegradeMultiplier()
        {
            return _weather != null ? _weather.HazmatDegradeMultiplier : 1f;
        }

        /// <summary>
        /// Degrade worn protective gear by weather-scaled hours.
        /// </summary>
        public void DegradeHazmat(WornGear gear, float gameHours)
        {
            if (gear == null || gameHours <= 0f) return;
            float mult = GetHazmatDegradeMultiplier();
            gear.Degrade(gameHours * mult);
        }

        /// <summary>
        /// Apply or clear Dread based on Black Rain and exposure flags.
        /// Outdoor expedition survivors and hatch listeners both qualify.
        /// </summary>
        /// <param name="survivors">All living candidates.</param>
        /// <param name="isOutdoor">Per-survivor outdoor exposure (expedition).</param>
        /// <param name="isHatchListener">True when survivor can hear rain on hatch
        /// (e.g. in entry room, or hatch sealed/open during BlackRain — caller decides).</param>
        /// <param name="gameHours">Elapsed hours for morale drain.</param>
        public void TickDread(
            IReadOnlyList<Survivor> survivors,
            Func<Survivor, bool> isOutdoor,
            Func<Survivor, bool> isHatchListener,
            float gameHours)
        {
            if (survivors == null) return;

            bool storm = IsActive;
            for (int i = 0; i < survivors.Count; i++)
            {
                var s = survivors[i];
                if (s == null || !s.IsAlive) continue;

                bool exposed = storm
                    && ((isOutdoor != null && isOutdoor(s))
                        || (isHatchListener != null && isHatchListener(s)));

                if (exposed)
                {
                    if (!s.HasDread)
                    {
                        s.HasDread = true;
                        OnDreadApplied?.Invoke(s);
                    }
                    if (gameHours > 0f && s.Needs != null)
                    {
                        s.Needs.Morale = Math.Max(0f, s.Needs.Morale - DreadMoraleDrainPerHour * gameHours);
                    }
                }
                else if (s.HasDread)
                {
                    s.HasDread = false;
                    OnDreadCleared?.Invoke(s);
                }
            }
        }

        /// <summary>
        /// Convenience for tests / simple hosts: everyone outdoor or hatch-listening
        /// uses the same bool flags for the whole party.
        /// </summary>
        public void TickDread(
            IReadOnlyList<Survivor> survivors,
            bool anyOutdoor,
            bool hatchListening,
            float gameHours)
        {
            TickDread(
                survivors,
                s => anyOutdoor,
                s => hatchListening,
                gameHours);
        }
    }
}
