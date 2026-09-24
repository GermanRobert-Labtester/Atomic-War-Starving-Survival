// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : AccessibilitySettingsSelfTest
// Subsystem          : Plan 184 — Accessibility Options System
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Accessibility;

namespace AtomicWar.GodotApp
{
    public static class HostCliAccessibilitySettings
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Accessibility Options System Self-Test (Plan 184) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading from accessibility_profiles.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "accessibility_profiles.json");

                var session = AccessibilitySettingsHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.System.GetAllProfiles().Count >= 5)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.System.GetAllProfiles().Count} accessibility profiles.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Profile catalog failed to load (count={session.System.GetAllProfiles().Count}).");
                }

                // Check 2: Standard default profile application
                bool defaultApplied = session.ApplyProfile("acc_profile_default");
                if (defaultApplied && session.System.ActiveProfileId == "acc_profile_default" &&
                    session.System.ColorblindMode == "None" && session.System.FontScale == 1.0f &&
                    !session.System.HighContrast)
                {
                    Console.WriteLine("[PASS] Check 2: Standard default profile applied successfully.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Default profile application failed.");
                }

                // Check 3: Visual assistance profile (high contrast, deuteranopia, font scale)
                bool visualApplied = session.ApplyProfile("acc_profile_visual");
                if (visualApplied && session.System.HighContrast &&
                    session.System.ColorblindMode == "Deuteranopia" && session.System.FontScale >= 1.25f &&
                    session.System.ScreenReaderFriendly)
                {
                    Console.WriteLine($"[PASS] Check 3: Visual assistance profile applied (HighContrast=true, Mode={session.System.ColorblindMode}, Scale={session.System.FontScale:0.00}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Visual assistance profile application failed.");
                }

                // Check 4: Hearing assistance profile (visual alerts, large subtitles, mono audio)
                bool hearingApplied = session.ApplyProfile("acc_profile_hearing");
                if (hearingApplied && session.System.VisualAudioAlerts && session.System.MonoAudio &&
                    session.System.SubtitleSize == "Large")
                {
                    Console.WriteLine("[PASS] Check 4: Hearing assistance profile applied (VisualAlerts=true, Mono=true, Subtitles=Large).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Hearing assistance profile application failed.");
                }

                // Check 5: Motor assistance profile (auto walk, aim assist)
                bool motorApplied = session.ApplyProfile("acc_profile_motor");
                if (motorApplied && session.System.AutoWalk && session.System.AimAssist)
                {
                    Console.WriteLine("[PASS] Check 5: Motor assistance profile applied (AutoWalk=true, AimAssist=true).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Motor assistance profile application failed.");
                }

                // Check 6: Cognitive clarity profile (reduced motion, cognitive load reduction)
                bool cogApplied = session.ApplyProfile("acc_profile_cognitive");
                if (cogApplied && session.System.CognitiveLoadReduction && session.System.ReducedMotion)
                {
                    Console.WriteLine("[PASS] Check 6: Cognitive clarity profile applied (LoadReduction=true, ReducedMotion=true).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: Cognitive clarity profile application failed.");
                }

                // Check 7: Custom individual setting override marks active profile as custom
                session.SetColorblindMode("Protanopia");
                if (session.System.ColorblindMode == "Protanopia" && session.System.ActiveProfileId == "custom")
                {
                    Console.WriteLine("[PASS] Check 7: Individual override switched profile to 'custom'.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Individual setting override failed.");
                }

                // Check 8: Font scale clamping between 0.75 and 2.0
                session.SetFontScale(5.0f);
                float highClamp = session.System.FontScale;
                session.SetFontScale(0.1f);
                float lowClamp = session.System.FontScale;
                if (highClamp == 2.0f && lowClamp == 0.75f)
                {
                    Console.WriteLine($"[PASS] Check 8: Font scale clamping enforced [0.75, 2.0] (Got {lowClamp:0.00} and {highClamp:0.00}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Font scale clamping failed (Got {lowClamp} and {highClamp}).");
                }

                // Check 9: High contrast and reduced motion individual toggles
                session.SetHighContrast(true);
                session.SetReducedMotion(true);
                if (session.System.HighContrast && session.System.ReducedMotion)
                {
                    Console.WriteLine("[PASS] Check 9: High contrast and reduced motion flags set successfully.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Contrast/Motion flag toggle failed.");
                }

                // Check 10: Event notification on setting change
                bool eventFired = false;
                session.System.OnSettingsChanged += s => eventFired = true;
                session.SetSubtitleSize("ExtraLarge");
                if (eventFired && session.System.SubtitleSize == "ExtraLarge")
                {
                    Console.WriteLine("[PASS] Check 10: OnSettingsChanged event dispatched on modification.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Settings change event not dispatched.");
                }

                // Check 11: State capture & restore round-trip
                var captured = session.System.CaptureState();
                var restoredSys = new AccessibilitySettingsSystem();
                restoredSys.RestoreState(captured);
                if (restoredSys.ActiveProfileId == captured.ActiveProfileId &&
                    restoredSys.ColorblindMode == captured.ColorblindMode &&
                    restoredSys.HighContrast == captured.HighContrast &&
                    restoredSys.ReducedMotion == captured.ReducedMotion)
                {
                    Console.WriteLine("[PASS] Check 11: State capture/restore round-trip verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 11: State capture/restore mismatch.");
                }

                // Check 12: Checksummed save store serialization and census
                string bareJson = AccessibilitySettingsSaveStore.TryCapturePersisted(captured);
                var restoredFromSave = AccessibilitySettingsSaveStore.TryRestorePersisted(bareJson);
                var census = session.Census;
                if (restoredFromSave != null && restoredFromSave.ColorblindMode == captured.ColorblindMode &&
                    census.LoadedProfilesCount >= 5 && census.HighContrast)
                {
                    Console.WriteLine($"[PASS] Check 12: Checksummed save store serialization verified; census valid (Profiles: {census.LoadedProfilesCount}, Mode: {census.ColorblindMode}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Checksummed save store or census verification failed.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception in AccessibilitySettings self-test: {ex.Message}");
            }

            Console.WriteLine($"AccessibilitySettings Self-Test Result: {passed}/{total} checks passed.");
            return passed == total ? 0 : 1;
        }
    }
}
