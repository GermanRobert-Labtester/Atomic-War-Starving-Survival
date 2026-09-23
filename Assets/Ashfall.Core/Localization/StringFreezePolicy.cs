// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Localization
{
    public enum StringFreezeClass
    {
        UiChrome = 1,
        Settings = 2,
        TutorialOnboarding = 3,
        WarningsAlerts = 4,
        ItemMetadata = 5,
        CodexManual = 6,
        VoiceKeys = 7,
        BriefingTitles = 8,
    }

    /// <summary>
    /// D22: Player-facing Localization String Freeze Policy & Extraction Gate.
    /// Enforces that frozen text classes must use structured, valid localization keys
    /// rather than unkeyed raw strings. Prevents UI string drift and provides allowlist debt tracking.
    /// </summary>
    public sealed class StringFreezePolicy
    {
        private static readonly HashSet<StringFreezeClass> FrozenClasses = new()
        {
            StringFreezeClass.UiChrome,
            StringFreezeClass.Settings,
            StringFreezeClass.TutorialOnboarding,
            StringFreezeClass.WarningsAlerts,
            StringFreezeClass.ItemMetadata,
            StringFreezeClass.CodexManual,
            StringFreezeClass.VoiceKeys,
            StringFreezeClass.BriefingTitles,
        };

        private readonly HashSet<string> _allowlistedDebt = new(StringComparer.OrdinalIgnoreCase);

        public StringFreezePolicy(IEnumerable<string>? debtAllowlist = null)
        {
            if (debtAllowlist != null)
            {
                foreach (var item in debtAllowlist)
                {
                    if (!string.IsNullOrWhiteSpace(item))
                        _allowlistedDebt.Add(item.Trim());
                }
            }
        }

        public static bool IsClassFrozen(StringFreezeClass stringClass)
        {
            return FrozenClasses.Contains(stringClass);
        }

        public static bool ValidateKeyFormat(StringFreezeClass stringClass, string key, out string? error)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                error = "Key cannot be null or empty.";
                return false;
            }

            key = key.Trim();

            switch (stringClass)
            {
                case StringFreezeClass.UiChrome:
                    if (!key.StartsWith("ui.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "UI chrome keys must start with 'ui.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.Settings:
                    if (!key.StartsWith("settings.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Settings keys must start with 'settings.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.TutorialOnboarding:
                    if (!key.StartsWith("tutorial.", StringComparison.OrdinalIgnoreCase) &&
                        !key.StartsWith("onboarding.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Tutorial keys must start with 'tutorial.' or 'onboarding.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.WarningsAlerts:
                    if (!key.StartsWith("warning.", StringComparison.OrdinalIgnoreCase) &&
                        !key.StartsWith("alert.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Warning keys must start with 'warning.' or 'alert.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.ItemMetadata:
                    if (!key.StartsWith("item.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Item metadata keys must start with 'item.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.VoiceKeys:
                    if (!key.StartsWith("voice.", StringComparison.OrdinalIgnoreCase))
                    {
                        error = "Voice keys must start with 'voice.' prefix.";
                        return false;
                    }
                    break;

                case StringFreezeClass.BriefingTitles:
                case StringFreezeClass.CodexManual:
                    // General valid dot-delimited key
                    if (!key.Contains("."))
                    {
                        error = "Keys must be structured and dot-delimited.";
                        return false;
                    }
                    break;
            }

            error = null;
            return true;
        }

        public bool ValidateStringSubmission(
            StringFreezeClass stringClass,
            string keyOrRawText,
            bool isKey,
            out string? rejectionReason)
        {
            if (string.IsNullOrWhiteSpace(keyOrRawText))
            {
                rejectionReason = "Text or key cannot be empty.";
                return false;
            }

            // If not frozen, allow
            if (!IsClassFrozen(stringClass))
            {
                rejectionReason = null;
                return true;
            }

            // If provided as a key, validate format
            if (isKey)
            {
                return ValidateKeyFormat(stringClass, keyOrRawText, out rejectionReason);
            }

            // If raw text is provided for a frozen class, check allowlist debt
            if (_allowlistedDebt.Contains(keyOrRawText.Trim()))
            {
                rejectionReason = null;
                return true; // Allowlisted legacy debt
            }

            rejectionReason = $"Class '{stringClass}' is under string freeze. Raw player-facing string '{keyOrRawText}' must be extracted to an authored localization key.";
            return false;
        }
    }
}
