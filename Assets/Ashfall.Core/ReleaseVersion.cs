using System;

namespace Ashfall.Core
{
    public enum ReleaseBumpKind
    {
        Invalid,
        Patch,
        Minor,
        Major
    }

    /// <summary>
    /// Strict semver X.Y.Z for the GAME axis only.
    /// Data and save axes are integers owned by their respective codecs and catalogs.
    /// </summary>
    public static class ReleaseVersion
    {
        public static bool TryParse(string? text, out string normalized)
        {
            normalized = string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string trimmed = text!.Trim();
            string[] parts = trimmed.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            if (!TryParseComponent(parts[0], out int major) ||
                !TryParseComponent(parts[1], out int minor) ||
                !TryParseComponent(parts[2], out int patch))
            {
                return false;
            }

            normalized = $"{major}.{minor}.{patch}";
            return true;
        }

        public static bool TryParse(string? text, out int major, out int minor, out int patch)
        {
            major = 0;
            minor = 0;
            patch = 0;

            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            string trimmed = text!.Trim();
            string[] parts = trimmed.Split('.');
            if (parts.Length != 3)
            {
                return false;
            }

            return TryParseComponent(parts[0], out major) &&
                   TryParseComponent(parts[1], out minor) &&
                   TryParseComponent(parts[2], out patch);
        }

        public static ReleaseBumpKind ClassifyBump(string? oldVersion, string? newVersion)
        {
            if (!TryParse(oldVersion, out int oldMajor, out int oldMinor, out int oldPatch) ||
                !TryParse(newVersion, out int newMajor, out int newMinor, out int newPatch))
            {
                return ReleaseBumpKind.Invalid;
            }

            if (newMajor > oldMajor)
            {
                return ReleaseBumpKind.Major;
            }

            if (newMajor == oldMajor)
            {
                if (newMinor > oldMinor)
                {
                    return ReleaseBumpKind.Minor;
                }

                if (newMinor == oldMinor && newPatch > oldPatch)
                {
                    return ReleaseBumpKind.Patch;
                }
            }

            return ReleaseBumpKind.Invalid;
        }

        public static string Format(int major, int minor, int patch)
        {
            return $"{major}.{minor}.{patch}";
        }

        private static bool TryParseComponent(string segment, out int value)
        {
            value = 0;
            if (string.IsNullOrEmpty(segment))
            {
                return false;
            }

            if (segment.Length > 1 && segment[0] == '0')
            {
                return false; // No leading zeroes like "01"
            }

            for (int i = 0; i < segment.Length; i++)
            {
                if (segment[i] < '0' || segment[i] > '9')
                {
                    return false;
                }
            }

            return int.TryParse(segment, out value) && value >= 0;
        }
    }
}
