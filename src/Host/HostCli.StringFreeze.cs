// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : StringFreezeSelfTest
// Core Authority     : Ashfall.Core.Localization.StringFreezePolicy (D22)
// Purpose            : frozen text classes must use structured localization keys
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Localization;

namespace AtomicWar.GodotApp
{
    public static class HostCliStringFreeze
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Localization String Freeze Policy Self-Test (D22) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                bool uiValid = StringFreezePolicy.ValidateKeyFormat(StringFreezeClass.UiChrome, "ui.dashboard.btn_craft", out string? uiError);
                if (uiValid && uiError == null)
                {
                    Console.WriteLine("[PASS] Check 1: a valid ui.* key passes format validation.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: ui key invalid ({uiError})."); }

                bool settingsValid = StringFreezePolicy.ValidateKeyFormat(StringFreezeClass.Settings, "audio.master_volume", out string? settingsError);
                if (!settingsValid && settingsError != null && settingsError.Contains("settings."))
                {
                    Console.WriteLine("[PASS] Check 2: a settings key with the wrong prefix is refused with the expected prefix.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: wrong-prefix settings key accepted ({settingsError})."); }

                var policy = new StringFreezePolicy();
                bool rawRejected = !policy.ValidateStringSubmission(StringFreezeClass.WarningsAlerts, "Radiation critical!", isKey: false, out string? rawReason)
                    && rawReason != null && rawReason.Contains("string freeze");
                if (rawRejected)
                {
                    Console.WriteLine("[PASS] Check 3: a raw player-facing string in a frozen class is rejected.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: raw string accepted ({rawReason})."); }

                var allowlistPolicy = new StringFreezePolicy(new List<string> { "System Activity", "Back" });
                bool allowed = allowlistPolicy.ValidateStringSubmission(StringFreezeClass.UiChrome, "Back", isKey: false, out string? allowReason);
                if (allowed && allowReason == null)
                {
                    Console.WriteLine("[PASS] Check 4: allowlisted legacy debt is permitted through the freeze.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: allowlisted debt refused ({allowReason})."); }

                bool itemKey = policy.ValidateStringSubmission(StringFreezeClass.ItemMetadata, "item.canned_beans.name", isKey: true, out string? itemReason);
                if (itemKey && itemReason == null)
                {
                    Console.WriteLine("[PASS] Check 5: a valid item.* key passes submission validation.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: item key refused ({itemReason})."); }

                bool emptyRefused = !policy.ValidateStringSubmission(StringFreezeClass.UiChrome, "   ", isKey: true, out string? emptyReason)
                    && emptyReason != null;
                if (emptyRefused)
                {
                    Console.WriteLine("[PASS] Check 6: an empty/whitespace submission is refused.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: empty submission accepted."); }

                if (StringFreezePolicy.IsClassFrozen(StringFreezeClass.UiChrome)
                    && StringFreezePolicy.IsClassFrozen(StringFreezeClass.VoiceKeys))
                {
                    Console.WriteLine("[PASS] Check 7: UI chrome and voice-key classes report as frozen.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: frozen class check wrong."); }

                bool codexNoDot = StringFreezePolicy.ValidateKeyFormat(StringFreezeClass.CodexManual, "manualentry", out string? codexError);
                if (!codexNoDot && codexError != null && codexError.Contains("dot-delimited"))
                {
                    Console.WriteLine("[PASS] Check 8: a structureless codex key is refused as non-dot-delimited.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 8: codex no-dot accepted ({codexError})."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"String freeze policy: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
