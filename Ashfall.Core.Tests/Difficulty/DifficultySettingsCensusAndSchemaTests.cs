// SPDX-License-Identifier: MIT
// Plan 181 — census, shared-catalog binding, schema gate, and provider factory.

using System;
using System.IO;
using Ashfall.Core.Difficulty;
using Xunit;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class DifficultySettingsCensusAndSchemaTests
    {
        private static string Json()
        {
            string dir = AppContext.BaseDirectory;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                string p = Path.Combine(dir, "Assets", "StreamingAssets", "Data", "difficulty_presets.json");
                if (File.Exists(p)) return File.ReadAllText(p);
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("difficulty_presets.json not found");
        }

        private static DifficultySettingsSystem SystemOverSharedCatalog(out DifficultyPresetCatalog catalog)
        {
            catalog = DifficultyPresetCatalogLoader.LoadFromJson(Json());
            catalog.Index();
            var system = new DifficultySettingsSystem();
            system.BindCatalog(catalog);
            return system;
        }

        [Fact]
        public void BindCatalog_SharesTheProvidedCatalogInstance()
        {
            var system = SystemOverSharedCatalog(out var catalog);
            Assert.Same(catalog, system.Catalog);
            Assert.Equal(4, system.GetCensus().PresetCount);
        }

        [Fact]
        public void Census_ReportsCustomLanesAndLock()
        {
            var system = SystemOverSharedCatalog(out _);
            system.SelectPreset("difficulty_dirge");
            var presetCensus = system.GetCensus();
            Assert.Equal("difficulty_dirge", presetCensus.ActivePresetId);
            Assert.False(presetCensus.IsCustom);
            Assert.False(presetCensus.IsLocked);
            Assert.True(presetCensus.ActivePresetResolves);

            system.SetCustomScalar("raging_unknown_lane", 1.5f); // unknown lane is a no-op
            Assert.False(system.IsCustom);

            system.SelectPreset("difficulty_standard");
            system.SetCustomScalar("radiation_gain_mult", 1.5f);
            var customCensus = system.GetCensus();
            Assert.True(customCensus.IsCustom);
            Assert.Equal("difficulty_custom", customCensus.ActivePresetId);
            Assert.False(customCensus.ActivePresetResolves); // synthetic id is not an authored preset
            Assert.Equal(1, customCensus.CustomizedScalarCount);

            system.LockSettings();
            Assert.True(system.GetCensus().IsLocked);
        }

        [Fact]
        public void GetEffectiveProvider_ReturnsValidatedCustomProvider()
        {
            var system = SystemOverSharedCatalog(out _);
            system.SetCustomScalar("hunger_rate_mult", 1.4f);

            var provider = system.GetEffectiveProvider();
            Assert.Equal("difficulty_custom", provider.PresetId);
            Assert.Equal(1.4f, provider.HungerMult, 3);
        }

        [Fact]
        public void FromScalars_RejectsOutOfBandAndClones()
        {
            var bad = DifficultyScalars.Legacy();
            bad.hunger_rate_mult = 9f;
            Assert.Throws<ArgumentException>(() => DifficultyScalarsProvider.FromScalars("x", bad));

            var good = DifficultyScalars.Legacy();
            good.hunger_rate_mult = 1.25f;
            var provider = DifficultyScalarsProvider.FromScalars("difficulty_custom", good);
            good.hunger_rate_mult = 0.5f; // mutate the source after construction
            Assert.Equal(1.25f, provider.HungerMult, 3);
        }

        [Fact]
        public void RestoreState_RejectsNewerSchema_AndNormalizesLegacy()
        {
            var system = SystemOverSharedCatalog(out _);

            var newer = new DifficultySettingsState { SchemaVersion = 99 };
            Assert.Throws<InvalidOperationException>(() => system.RestoreState(newer));

            var legacy = new DifficultySettingsState
            {
                SchemaVersion = 0,
                ActivePresetId = "difficulty_sparing",
                IsLocked = true
            };
            system.RestoreState(legacy);
            Assert.Equal("difficulty_sparing", system.ActivePresetId);
            Assert.True(system.IsLocked);
            Assert.Equal(1, system.CaptureState().SchemaVersion);
        }
    }
}
