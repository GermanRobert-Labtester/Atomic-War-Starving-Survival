// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Difficulty;
using Xunit;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class DifficultyDirectorTests
    {
        [Fact]
        public void ResolveProvider_UsesTheSelectedPresetScalars()
        {
            var director = new DifficultyDirector(Catalog());

            DifficultyScalarsProvider provider = director.ResolveProvider("difficulty_austere");

            Assert.Equal("difficulty_austere", provider.PresetId);
            Assert.Equal(1.35f, provider.NeedsMult);
            Assert.Equal(1.3f, provider.RadiationMult);
            Assert.Equal(0.8f, provider.CrisisDeadlineMult);
        }

        [Fact]
        public void ResolveProvider_UsesTheCatalogDefaultForLegacyCampaigns()
        {
            var director = new DifficultyDirector(Catalog());

            DifficultyScalarsProvider provider = director.ResolveProvider(null);

            Assert.Equal("difficulty_standard", provider.PresetId);
            Assert.Equal(1f, provider.NeedsMult);
            Assert.Equal(1f, provider.EquipmentDecayMult);
        }

        [Fact]
        public void ResolveProvider_RejectsAnUnknownPersistedPreset()
        {
            var director = new DifficultyDirector(Catalog());

            InvalidOperationException error = Assert.Throws<InvalidOperationException>(
                () => director.ResolveProvider("difficulty_unknown"));

            Assert.Contains("difficulty_unknown", error.Message);
        }

        [Fact]
        public void LegacyProvider_IsByteNeutralAcrossEveryScalar()
        {
            DifficultyScalarsProvider provider = DifficultyScalarsProvider.Legacy;

            Assert.Equal(1f, provider.HungerMult);
            Assert.Equal(1f, provider.ThirstMult);
            Assert.Equal(1f, provider.RadiationMult);
            Assert.Equal(1f, provider.DiseaseMult);
            Assert.Equal(1f, provider.HostileEncounterMult);
            Assert.Equal(1f, provider.MarketPriceMult);
            Assert.Equal(1f, provider.EquipmentDecayMult);
            Assert.Equal(1f, provider.CrisisDeadlineMult);
        }

        private static DifficultyPresetCatalog Catalog()
        {
            return new DifficultyPresetCatalog
            {
                default_preset_id = "difficulty_standard",
                presets = new List<DifficultyPreset>
                {
                    new DifficultyPreset
                    {
                        id = "difficulty_standard",
                        display_name = "STANDARD",
                        scalars = DifficultyScalars.Legacy()
                    },
                    new DifficultyPreset
                    {
                        id = "difficulty_austere",
                        display_name = "AUSTERE",
                        scalars = new DifficultyScalars
                        {
                            hunger_rate_mult = 1.35f,
                            thirst_rate_mult = 1.35f,
                            radiation_gain_mult = 1.3f,
                            disease_onset_mult = 1.25f,
                            hostile_encounter_mult = 1.35f,
                            market_price_mult = 1.15f,
                            equipment_decay_mult = 1.25f,
                            crisis_deadline_mult = 0.8f
                        }
                    }
                }
            };
        }
    }
}
