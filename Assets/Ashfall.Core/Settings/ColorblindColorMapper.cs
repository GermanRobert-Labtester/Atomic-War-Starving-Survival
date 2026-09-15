// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Settings
{
    /// <summary>
    /// Plan 184 — engine-free RGB simulation matrices for colorblind modes.
    /// Does not own Theme token constants; presentation calls this from
    /// <c>AshfallUiHelpers.ToColor</c> after reading <see cref="UserSettingsData.ColorblindMode"/>.
    /// Matrices are standard linear CVD approximations (Viénot-style).
    /// </summary>
    public static class ColorblindColorMapper
    {
        public const string None = "none";
        public const string Protanopia = "protanopia";
        public const string Deuteranopia = "deuteranopia";
        public const string Tritanopia = "tritanopia";

        public static readonly string[] AllowedModes =
        {
            None,
            Protanopia,
            Deuteranopia,
            Tritanopia
        };

        public static string NormalizeMode(string? mode)
        {
            if (string.IsNullOrWhiteSpace(mode)) return None;
            string m = mode.Trim().ToLowerInvariant();
            for (int i = 0; i < AllowedModes.Length; i++)
            {
                if (string.Equals(AllowedModes[i], m, StringComparison.Ordinal))
                    return AllowedModes[i];
            }
            return None;
        }

        public static bool IsKnownMode(string? mode)
        {
            if (string.IsNullOrWhiteSpace(mode)) return false;
            string m = mode.Trim().ToLowerInvariant();
            for (int i = 0; i < AllowedModes.Length; i++)
            {
                if (string.Equals(AllowedModes[i], m, StringComparison.Ordinal))
                    return true;
            }
            return false;
        }

        /// <summary>Map an RGBA token under the given mode. Alpha unchanged.</summary>
        public static (float r, float g, float b, float a) Map(
            (float r, float g, float b, float a) color,
            string? mode)
        {
            string m = NormalizeMode(mode);
            if (m == None) return color;

            float r = color.r;
            float g = color.g;
            float b = color.b;
            float nr, ng, nb;

            switch (m)
            {
                case Protanopia:
                    // Approx. protanopia simulation matrix
                    nr = 0.56667f * r + 0.43333f * g;
                    ng = 0.55833f * r + 0.44167f * g;
                    nb = 0.24167f * g + 0.75833f * b;
                    break;
                case Deuteranopia:
                    nr = 0.625f * r + 0.375f * g;
                    ng = 0.700f * r + 0.300f * g;
                    nb = 0.300f * g + 0.700f * b;
                    break;
                case Tritanopia:
                    nr = 0.950f * r + 0.050f * g;
                    ng = 0.433f * g + 0.567f * b;
                    nb = 0.475f * g + 0.525f * b;
                    break;
                default:
                    return color;
            }

            return (
                Clamp01(nr),
                Clamp01(ng),
                Clamp01(nb),
                color.a);
        }

        private static float Clamp01(float v)
        {
            if (float.IsNaN(v) || float.IsInfinity(v)) return 0f;
            if (v < 0f) return 0f;
            if (v > 1f) return 1f;
            return v;
        }
    }
}
