// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Difficulty
{
    /// <summary>
    /// Immutable typed scalar view supplied to existing systems. A missing
    /// provider at a consumer remains the legacy behavior; this class exposes
    /// the same all-ones values for callers that need an explicit fallback.
    /// </summary>
    public sealed class DifficultyScalarsProvider
    {
        private readonly DifficultyScalars _scalars;

        private DifficultyScalarsProvider(string presetId, DifficultyScalars scalars)
        {
            PresetId = presetId;
            _scalars = scalars?.Clone() ?? throw new ArgumentNullException(nameof(scalars));
        }

        public string PresetId { get; }
        public float NeedsMult => _scalars.hunger_rate_mult;
        public float HungerMult => _scalars.hunger_rate_mult;
        public float ThirstMult => _scalars.thirst_rate_mult;
        public float RadiationMult => _scalars.radiation_gain_mult;
        public float DiseaseMult => _scalars.disease_onset_mult;
        public float HostileEncounterMult => _scalars.hostile_encounter_mult;
        public float MarketPriceMult => _scalars.market_price_mult;
        public float EquipmentDecayMult => _scalars.equipment_decay_mult;
        public float CrisisDeadlineMult => _scalars.crisis_deadline_mult;

        public static DifficultyScalarsProvider Legacy { get; } =
            new DifficultyScalarsProvider("difficulty_standard", DifficultyScalars.Legacy());

        public static DifficultyScalarsProvider FromPreset(DifficultyPreset preset)
        {
            if (preset == null) throw new ArgumentNullException(nameof(preset));
            if (!preset.Validate(out string error))
                throw new ArgumentException("invalid difficulty preset: " + error, nameof(preset));
            return new DifficultyScalarsProvider(preset.id, preset.scalars);
        }

        public override bool Equals(object? obj)
        {
            if (obj is DifficultyScalarsProvider other)
            {
                return string.Equals(PresetId, other.PresetId, StringComparison.Ordinal)
                    && _scalars.hunger_rate_mult == other._scalars.hunger_rate_mult
                    && _scalars.thirst_rate_mult == other._scalars.thirst_rate_mult
                    && _scalars.radiation_gain_mult == other._scalars.radiation_gain_mult
                    && _scalars.disease_onset_mult == other._scalars.disease_onset_mult
                    && _scalars.hostile_encounter_mult == other._scalars.hostile_encounter_mult
                    && _scalars.market_price_mult == other._scalars.market_price_mult
                    && _scalars.equipment_decay_mult == other._scalars.equipment_decay_mult
                    && _scalars.crisis_deadline_mult == other._scalars.crisis_deadline_mult;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(PresetId, _scalars.hunger_rate_mult, _scalars.thirst_rate_mult, _scalars.radiation_gain_mult);
        }
    }
}
