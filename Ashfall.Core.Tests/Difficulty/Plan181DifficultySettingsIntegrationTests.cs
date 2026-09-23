// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 181: Difficulty Settings System — Integration Tests
// Verifies preset catalog loading, preset switching, custom slider modification,
// ironman/campaign lock enforcement, and save/restore state persistence.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Difficulty;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class Plan181DifficultySettingsIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsPresetsFromDifficultyCatalog()
        {
            var system = new DifficultySettingsSystem();
            string path = ResolveDataPath("difficulty_presets.json");
            Assert.True(File.Exists(path), $"difficulty_presets.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            Assert.NotEmpty(system.Catalog.presets);
            Assert.True(system.Catalog.TryGet("difficulty_standard", out _));
            Assert.True(system.Catalog.TryGet("difficulty_sparing", out _));
        }

        [Fact]
        public void SelectPreset_ChangesActivePresetAndFiresEvent()
        {
            var system = new DifficultySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));

            DifficultyScalars? changedScalars = null;
            system.OnSettingsChanged += s => changedScalars = s;

            bool success = system.SelectPreset("difficulty_sparing");

            Assert.True(success);
            Assert.Equal("difficulty_sparing", system.ActivePresetId);
            Assert.False(system.IsCustom);
            Assert.NotNull(changedScalars);

            var scalars = system.GetEffectiveScalars();
            Assert.Equal(0.75f, scalars.hunger_rate_mult, 2);
            Assert.Equal(0.75f, scalars.radiation_gain_mult, 2);
        }

        [Fact]
        public void SetCustomScalar_EnablesCustomModeAndAppliesClampedMultiplier()
        {
            var system = new DifficultySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));

            bool updated = system.SetCustomScalar("radiation_gain_mult", 1.8f);

            Assert.True(updated);
            Assert.True(system.IsCustom);
            Assert.Equal("difficulty_custom", system.ActivePresetId);

            var scalars = system.GetEffectiveScalars();
            Assert.Equal(1.8f, scalars.radiation_gain_mult, 2);

            // Test clamp below minimum (0.25)
            system.SetCustomScalar("hunger_rate_mult", 0.05f);
            Assert.Equal(0.25f, system.GetEffectiveScalars().hunger_rate_mult, 2);
        }

        [Fact]
        public void LockSettings_PreventsPresetAndSliderChanges()
        {
            var system = new DifficultySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));

            bool lockedFired = false;
            system.OnSettingsLocked += () => lockedFired = true;

            system.LockSettings();

            Assert.True(system.IsLocked);
            Assert.True(lockedFired);

            // Mid-campaign modification attempt should fail
            bool presetChanged = system.SelectPreset("difficulty_sparing");
            Assert.False(presetChanged);

            bool sliderChanged = system.SetCustomScalar("radiation_gain_mult", 2.0f);
            Assert.False(sliderChanged);
        }

        [Fact]
        public void UnknownPreset_FailsGracefullyWithoutMutatingState()
        {
            var system = new DifficultySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));

            string originalPreset = system.ActivePresetId;
            bool success = system.SelectPreset("difficulty_nonexistent_preset");

            Assert.False(success);
            Assert.Equal(originalPreset, system.ActivePresetId);
        }

        [Fact]
        public void SaveRestoreState_PreservesActivePresetCustomValuesAndLock()
        {
            var system = new DifficultySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));

            system.SetCustomScalar("hunger_rate_mult", 1.4f);
            system.LockSettings();

            var state = system.CaptureState();

            var restored = new DifficultySettingsSystem();
            restored.LoadCatalog(File.ReadAllText(ResolveDataPath("difficulty_presets.json")));
            restored.RestoreState(state);

            Assert.True(restored.IsLocked);
            Assert.True(restored.IsCustom);
            Assert.Equal("difficulty_custom", restored.ActivePresetId);
            Assert.Equal(1.4f, restored.GetEffectiveScalars().hunger_rate_mult, 2);
        }
    }
}
