// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 184: Accessibility Options System — Integration Tests
// Verifies accessibility profile catalog loading, profile application,
// individual accessibility overrides (visual, auditory, motor, cognitive),
// and save/restore state roundtrips.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Accessibility;

namespace Ashfall.Core.Tests.Accessibility
{
    public sealed class Plan184AccessibilitySettingsIntegrationTests
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
        public void LoadCatalog_LoadsProfilesFromCatalog()
        {
            var system = new AccessibilitySettingsSystem();
            string path = ResolveDataPath("accessibility_profiles.json");
            Assert.True(File.Exists(path), $"accessibility_profiles.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var profiles = system.GetAllProfiles();
            Assert.NotEmpty(profiles);
            Assert.Contains(profiles, p => p.profile_id == "acc_profile_default");
            Assert.Contains(profiles, p => p.profile_id == "acc_profile_visual");
            Assert.Contains(profiles, p => p.profile_id == "acc_profile_hearing");
            Assert.Contains(profiles, p => p.profile_id == "acc_profile_motor");
            Assert.Contains(profiles, p => p.profile_id == "acc_profile_cognitive");
        }

        [Fact]
        public void ApplyProfile_SetsStandardDefaults()
        {
            var system = new AccessibilitySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));

            bool applied = system.ApplyProfile("acc_profile_default");

            Assert.True(applied);
            Assert.Equal("acc_profile_default", system.ActiveProfileId);
            Assert.Equal("None", system.ColorblindMode);
            Assert.Equal(1.0f, system.FontScale);
            Assert.False(system.HighContrast);
            Assert.False(system.AutoWalk);
        }

        [Fact]
        public void ApplyProfile_VisualImpairmentSetsHighContrastAndLargeSubtitles()
        {
            var system = new AccessibilitySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));

            string? profileApplied = null;
            system.OnProfileApplied += p => profileApplied = p;

            bool applied = system.ApplyProfile("acc_profile_visual");

            Assert.True(applied);
            Assert.Equal("acc_profile_visual", profileApplied);
            Assert.Equal("Deuteranopia", system.ColorblindMode);
            Assert.True(system.HighContrast);
            Assert.Equal(1.3f, system.FontScale);
            Assert.Equal("Large", system.SubtitleSize);
            Assert.True(system.ScreenReaderFriendly);
        }

        [Fact]
        public void ApplyProfile_MotorAssistanceEnablesAutoWalkAndAimAssist()
        {
            var system = new AccessibilitySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));

            bool applied = system.ApplyProfile("acc_profile_motor");

            Assert.True(applied);
            Assert.True(system.AutoWalk);
            Assert.True(system.AimAssist);
        }

        [Fact]
        public void SetCustomSettings_UpdatesOverridesAndSetsCustomProfile()
        {
            var system = new AccessibilitySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));

            AccessibilitySettingsState? changedState = null;
            system.OnSettingsChanged += s => changedState = s;

            system.SetFontScale(1.75f);
            Assert.Equal(1.75f, system.FontScale);
            Assert.Equal("custom", system.ActiveProfileId);
            Assert.NotNull(changedState);

            system.SetHighContrast(true);
            Assert.True(system.HighContrast);

            system.SetColorblindMode("Protanopia");
            Assert.Equal("Protanopia", system.ColorblindMode);
        }

        [Fact]
        public void SaveRestoreState_PreservesSettingsAndCustomOverrides()
        {
            var system = new AccessibilitySettingsSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));

            system.ApplyProfile("acc_profile_cognitive");
            system.SetFontScale(1.4f);

            var state = system.CaptureState();

            var restored = new AccessibilitySettingsSystem();
            restored.LoadCatalog(File.ReadAllText(ResolveDataPath("accessibility_profiles.json")));
            restored.RestoreState(state);

            Assert.Equal("custom", restored.ActiveProfileId);
            Assert.Equal(1.4f, restored.FontScale);
            Assert.True(restored.ReducedMotion);
            Assert.True(restored.CognitiveLoadReduction);
            Assert.True(restored.HighContrast);
        }
    }
}
