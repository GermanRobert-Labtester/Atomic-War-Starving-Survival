// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 184 Path α — shared preference presentation predicates.
// Reads UserSettingsStore.Current only; no second settings authority.
// ============================================================================
using Ashfall.Core.Settings;

namespace AtomicWar.GodotApp.Settings
{
    /// <summary>
    /// Thin presentation helpers for accessibility preferences under sole
    /// <see cref="UserSettingsStore"/> authority (Path α flags + Path β
    /// colorblind mode). Engine application remains
    /// <see cref="UserSettingsStore.Apply"/>; color mapping happens in
    /// <c>AshfallUiHelpers.ToColor</c> via Core <see cref="ColorblindColorMapper"/>.
    /// </summary>
    public static class AccessibilityPresentation
    {
        /// <summary>False when <see cref="UserSettingsData.ReducedMotion"/> is on.</summary>
        public static bool MotionAllowed =>
            !UserSettingsStore.Current.ReducedMotion;

        /// <summary>
        /// Viewport content scale from UiScale, with LargeFonts (+15%) and
        /// HighContrast (+5%) multipliers. Matches <see cref="UserSettingsStore.Apply"/>.
        /// </summary>
        public static float ResolveContentScaleFactor(UserSettingsData data)
        {
            if (data == null) return 1f;
            float scale = System.Math.Clamp(data.UiScale, 0.5f, 2.5f);
            if (data.LargeFonts)
                scale = System.Math.Clamp(scale * 1.15f, 0.5f, 2.5f);
            if (data.HighContrast)
                scale = System.Math.Clamp(scale * 1.05f, 0.5f, 2.5f);
            return scale;
        }

        /// <summary>Normalized colorblind mode from current settings.</summary>
        public static string ColorblindMode =>
            ColorblindColorMapper.NormalizeMode(UserSettingsStore.Current.ColorblindMode);
    }
}
