// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Shelter
{
    public enum AtmosphereMoodCategory
    {
        Bleak = 0,
        Tense = 1,
        Neutral = 2,
        Comfortable = 3,
        Welcoming = 4,
        Vibrant = 5
    }

    public enum AtmosphereProfileType
    {
        Industrial = 0,
        Sterile = 1,
        LivedIn = 2,
        Warm = 3,
        Cold = 4,
        Chaotic = 5,
        Serene = 6
    }

    public sealed class AtmosphereModifiers
    {
        public float MoraleModifier { get; set; }
        public float ProductivityModifier { get; set; }
        public float StressReliefModifier { get; set; }
    }

    [Serializable]
    public sealed class AtmosphereState
    {
        public int SchemaVersion { get; set; } = 1;
        public int NextSequence { get; set; } = 1;
        public float OverallMoodScore { get; set; } = 50f; // 0 to 100
        public AtmosphereMoodCategory CurrentMoodCategory { get; set; } = AtmosphereMoodCategory.Neutral;
        public AtmosphereProfileType ActiveProfile { get; set; } = AtmosphereProfileType.LivedIn;
        public float LightingQuality { get; set; } = 60f;
        public float AcousticComfort { get; set; } = 60f;
        public float AirPurity { get; set; } = 70f;
        public float ThermalComfort { get; set; } = 65f;
        public float Cleanliness { get; set; } = 70f;
        public float SocialWarmth { get; set; } = 50f;
        public float DecorationLevel { get; set; } = 30f;
        public int LastUpdatedDay { get; set; } = 1;
    }

    /// <summary>
    /// Plan 220 — Shelter Atmosphere & Ambiance System.
    /// Tracks composite shelter atmosphere (lighting, acoustics, air purity, thermal comfort,
    /// cleanliness, social warmth, decoration), evaluates the active atmospheric profile,
    /// and exposes holistic shelter modifiers for morale and productivity.
    /// </summary>
    public sealed class ShelterAtmosphereSystem
    {
        private readonly AtmosphereState _state;

        public event Action<AtmosphereMoodCategory, float>? OnMoodShifted;
        public event Action<AtmosphereProfileType>? OnProfileChanged;

        public float OverallMoodScore => _state.OverallMoodScore;
        public AtmosphereMoodCategory CurrentMoodCategory => _state.CurrentMoodCategory;
        public AtmosphereProfileType ActiveProfile => _state.ActiveProfile;

        public ShelterAtmosphereSystem(AtmosphereState? state = null)
        {
            _state = state ?? new AtmosphereState();
        }

        public void UpdateEnvironmentalInputs(
            float lighting,
            float acousticComfort,
            float airPurity,
            float thermalComfort,
            float cleanliness,
            float socialWarmth,
            float decorationLevel,
            int currentDay)
        {
            _state.LightingQuality = Math.Clamp(lighting, 0f, 100f);
            _state.AcousticComfort = Math.Clamp(acousticComfort, 0f, 100f);
            _state.AirPurity = Math.Clamp(airPurity, 0f, 100f);
            _state.ThermalComfort = Math.Clamp(thermalComfort, 0f, 100f);
            _state.Cleanliness = Math.Clamp(cleanliness, 0f, 100f);
            _state.SocialWarmth = Math.Clamp(socialWarmth, 0f, 100f);
            _state.DecorationLevel = Math.Clamp(decorationLevel, 0f, 100f);
            _state.LastUpdatedDay = currentDay;

            EvaluateAtmosphere(currentDay);
        }

        public void EvaluateAtmosphere(int currentDay)
        {
            // Weighted composite:
            // Lighting: 15%, Acoustics: 15%, Air: 20%, Thermal: 15%, Cleanliness: 15%, Social: 10%, Decoration: 10%
            float composite =
                (_state.LightingQuality * 0.15f) +
                (_state.AcousticComfort * 0.15f) +
                (_state.AirPurity * 0.20f) +
                (_state.ThermalComfort * 0.15f) +
                (_state.Cleanliness * 0.15f) +
                (_state.SocialWarmth * 0.10f) +
                (_state.DecorationLevel * 0.10f);

            _state.OverallMoodScore = MathF.Round(Math.Clamp(composite, 0f, 100f), 1);

            // Determine Mood Category
            var newCategory = _state.OverallMoodScore switch
            {
                < 25f => AtmosphereMoodCategory.Bleak,
                < 45f => AtmosphereMoodCategory.Tense,
                < 60f => AtmosphereMoodCategory.Neutral,
                < 75f => AtmosphereMoodCategory.Comfortable,
                < 90f => AtmosphereMoodCategory.Welcoming,
                _ => AtmosphereMoodCategory.Vibrant
            };

            if (newCategory != _state.CurrentMoodCategory)
            {
                _state.CurrentMoodCategory = newCategory;
                OnMoodShifted?.Invoke(newCategory, _state.OverallMoodScore);
            }

            // Determine Profile
            AtmosphereProfileType newProfile;
            if (_state.Cleanliness >= 85f && _state.DecorationLevel < 30f)
            {
                newProfile = AtmosphereProfileType.Sterile;
            }
            else if (_state.ThermalComfort < 35f || _state.OverallMoodScore < 30f)
            {
                newProfile = AtmosphereProfileType.Cold;
            }
            else if (_state.AcousticComfort < 35f && _state.SocialWarmth >= 60f)
            {
                newProfile = AtmosphereProfileType.Chaotic;
            }
            else if (_state.DecorationLevel >= 60f && _state.SocialWarmth >= 60f)
            {
                newProfile = AtmosphereProfileType.Warm;
            }
            else if (_state.AcousticComfort >= 75f && _state.AirPurity >= 75f)
            {
                newProfile = AtmosphereProfileType.Serene;
            }
            else if (_state.AcousticComfort < 50f && _state.LightingQuality >= 60f)
            {
                newProfile = AtmosphereProfileType.Industrial;
            }
            else
            {
                newProfile = AtmosphereProfileType.LivedIn;
            }

            if (newProfile != _state.ActiveProfile)
            {
                _state.ActiveProfile = newProfile;
                OnProfileChanged?.Invoke(newProfile);
            }
        }

        public AtmosphereModifiers GetActiveModifiers()
        {
            float baseMorale = (_state.OverallMoodScore - 50f) * 0.1f; // -5 to +5
            float baseProd = 0f;
            float baseStress = 0f;

            switch (_state.ActiveProfile)
            {
                case AtmosphereProfileType.Industrial:
                    baseProd += 0.10f;
                    baseMorale -= 1.0f;
                    break;
                case AtmosphereProfileType.Sterile:
                    baseStress -= 0.05f;
                    baseProd += 0.05f;
                    break;
                case AtmosphereProfileType.Warm:
                    baseMorale += 2.5f;
                    baseStress += 0.10f;
                    break;
                case AtmosphereProfileType.Cold:
                    baseMorale -= 3.0f;
                    baseProd -= 0.05f;
                    break;
                case AtmosphereProfileType.Chaotic:
                    baseStress -= 0.15f;
                    baseProd -= 0.10f;
                    break;
                case AtmosphereProfileType.Serene:
                    baseStress += 0.20f;
                    baseMorale += 1.5f;
                    break;
                case AtmosphereProfileType.LivedIn:
                default:
                    baseMorale += 0.5f;
                    break;
            }

            return new AtmosphereModifiers
            {
                MoraleModifier = MathF.Round(baseMorale, 1),
                ProductivityModifier = MathF.Round(baseProd, 2),
                StressReliefModifier = MathF.Round(baseStress, 2)
            };
        }

        public AtmosphereState CaptureState()
        {
            return new AtmosphereState
            {
                SchemaVersion = _state.SchemaVersion,
                NextSequence = _state.NextSequence,
                OverallMoodScore = _state.OverallMoodScore,
                CurrentMoodCategory = _state.CurrentMoodCategory,
                ActiveProfile = _state.ActiveProfile,
                LightingQuality = _state.LightingQuality,
                AcousticComfort = _state.AcousticComfort,
                AirPurity = _state.AirPurity,
                ThermalComfort = _state.ThermalComfort,
                Cleanliness = _state.Cleanliness,
                SocialWarmth = _state.SocialWarmth,
                DecorationLevel = _state.DecorationLevel,
                LastUpdatedDay = _state.LastUpdatedDay
            };
        }

        public void RestoreState(AtmosphereState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));

            _state.SchemaVersion = state.SchemaVersion;
            _state.NextSequence = state.NextSequence;
            _state.OverallMoodScore = state.OverallMoodScore;
            _state.CurrentMoodCategory = state.CurrentMoodCategory;
            _state.ActiveProfile = state.ActiveProfile;
            _state.LightingQuality = state.LightingQuality;
            _state.AcousticComfort = state.AcousticComfort;
            _state.AirPurity = state.AirPurity;
            _state.ThermalComfort = state.ThermalComfort;
            _state.Cleanliness = state.Cleanliness;
            _state.SocialWarmth = state.SocialWarmth;
            _state.DecorationLevel = state.DecorationLevel;
            _state.LastUpdatedDay = state.LastUpdatedDay;
        }
    }
}
