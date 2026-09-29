// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        public static int RunSettingsSelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print($"  [PASS] {message}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {message}");
                    failures++;
                }
            }

            GD.Print("[SettingsSelfTest] Starting UserSettings persistence, recovery, and engine application test...");
            string testPath = "user://settings_selftest.json";
            string globalTestPath = ProjectSettings.GlobalizePath(testPath);

            try
            {
                if (File.Exists(globalTestPath)) File.Delete(globalTestPath);

                // 1. Default creation
                var defaults = new UserSettingsData();
                Check(defaults.MasterVolume == 1.0f, "default master volume is 100%");
                Check(defaults.VSync, "default VSync is enabled");
                Check(defaults.MaxFps == 60, "default MaxFPS is 60");
                Check(defaults.ConfirmEndDay, "default ConfirmEndDay is enabled");

                // 2. Clone and Mutation
                var modified = defaults.Clone();
                modified.MasterVolume = 0.45f;
                modified.MusicVolume = 0.60f;
                modified.VSync = false;
                modified.MaxFps = 120;
                modified.HighContrast = true;
                modified.LargeFonts = true;
                modified.HazardTextLabels = false;
                modified.ReducedMotion = true;
                modified.ColorblindMode = ColorblindColorMapper.Protanopia;
                modified.TutorialMode = 2;
                modified.ResolutionWidth = 2560;
                modified.ResolutionHeight = 1440;

                // 3. Live Apply (Headless-safe) + Plan 184 Path α preference effects
                float expectedScale = AccessibilityPresentation.ResolveContentScaleFactor(modified);
                Check(Math.Abs(expectedScale - (1.0f * 1.15f * 1.05f)) < 0.001f,
                    "ResolveContentScaleFactor applies LargeFonts×1.15 and HighContrast×1.05");

                UserSettingsStore.Apply(modified);
                Check(Engine.MaxFps == 120, "Engine.MaxFps updated via Apply");

                var tree = Engine.GetMainLoop() as SceneTree;
                if (tree?.Root != null)
                {
                    Check(Math.Abs(tree.Root.ContentScaleFactor - expectedScale) < 0.01f,
                        "ContentScaleFactor applied for LargeFonts/HighContrast");
                    bool modulateOk = false;
                    for (int i = 0; i < tree.Root.GetChildCount(); i++)
                    {
                        if (tree.Root.GetChild(i) is CanvasItem canvas)
                        {
                            modulateOk = canvas.Modulate.R >= 1.14f;
                            break;
                        }
                    }
                    Check(modulateOk, "HighContrast brightens first root CanvasItem modulate");
                }
                else
                {
                    GD.Print("  [SKIP] ContentScaleFactor/modulate (no SceneTree root in this host)");
                }

                Check(!AccessibilityPresentation.MotionAllowed,
                    "ReducedMotion disables AccessibilityPresentation.MotionAllowed");
                Check(AshfallUiHelpers.FormatDoseSource(null, "demo_scan") == "demo_scan",
                    "HazardTextLabels off returns raw source id from FormatDoseSource");

                // Plan 184 Path β — colorblind mode live on Current + ToColor mapper
                Check(AccessibilityPresentation.ColorblindMode == ColorblindColorMapper.Protanopia,
                    "AccessibilityPresentation.ColorblindMode reflects applied protanopia");
                var themeCritical = Ashfall.Core.UI.Theme.Critical;
                var mappedCritical = ColorblindColorMapper.Map(themeCritical, ColorblindColorMapper.Protanopia);
                var toColorCritical = AshfallUiHelpers.ToColor(themeCritical);
                Check(Math.Abs(toColorCritical.R - mappedCritical.r) < 0.01f
                      && Math.Abs(toColorCritical.G - mappedCritical.g) < 0.01f
                      && Math.Abs(toColorCritical.B - mappedCritical.b) < 0.01f,
                    "ToColor applies ColorblindColorMapper to Theme.Critical under protanopia");
                Check(Math.Abs(toColorCritical.R - themeCritical.r) > 0.01f
                      || Math.Abs(toColorCritical.G - themeCritical.g) > 0.01f
                      || Math.Abs(toColorCritical.B - themeCritical.b) > 0.01f,
                    "ToColor Critical differs from Theme.Critical constant under protanopia");
                Check(Math.Abs(themeCritical.r - 0.902f) < 0.001f
                      && Math.Abs(themeCritical.g - 0.200f) < 0.001f,
                    "Theme.Critical constant floors remain unchanged");

                // 4. Save and Reload Round-trip
                bool saved = UserSettingsStore.Save(modified, testPath);
                Check(saved && File.Exists(globalTestPath), "settings successfully saved to disk");

                var loaded = UserSettingsStore.Load(testPath);
                Check(Math.Abs(loaded.MasterVolume - 0.45f) < 0.01f, "reloaded master volume preserved");
                Check(Math.Abs(loaded.MusicVolume - 0.60f) < 0.01f, "reloaded music volume preserved");
                Check(!loaded.VSync, "reloaded VSync state preserved");
                Check(loaded.MaxFps == 120, "reloaded MaxFps preserved");
                Check(loaded.HighContrast, "reloaded HighContrast preserved");
                Check(loaded.LargeFonts, "reloaded LargeFonts preserved");
                Check(!loaded.HazardTextLabels, "reloaded HazardTextLabels preserved");
                Check(loaded.ReducedMotion, "reloaded ReducedMotion preserved");
                Check(loaded.ColorblindMode == ColorblindColorMapper.Protanopia,
                    "reloaded ColorblindMode preserved");
                Check(loaded.TutorialMode == 2, "reloaded veteran onboarding mode preserved");
                Check(loaded.ResolutionWidth == 2560 && loaded.ResolutionHeight == 1440, "reloaded resolution preserved");

                // 5. Corruption Recovery & Diagnostic Preservation
                File.WriteAllText(globalTestPath, "{ CORRUPT_UNCLOSED_JSON_DATA_!!!");
                var recovered = UserSettingsStore.Load(testPath);
                Check(recovered != null && recovered.MasterVolume == 1.0f && recovered.MaxFps == 60, "corrupted file gracefully recovered to defaults");
                Check(UserSettingsStore.HasDiagnosticError && UserSettingsStore.LastDiagnosticMessage!.Contains("Invalid settings JSON"), "diagnostic message preserved upon corrupt load");

                // 6. Out-of-bounds Sanitization Recovery
                File.WriteAllText(globalTestPath, "{\n  \"master_volume\": -5.0,\n  \"resolution_width\": 99999,\n  \"max_fps\": -100,\n  \"ui_scale\": 99.0\n}");
                var sanitized = UserSettingsStore.Load(testPath);
                Check(sanitized.MasterVolume == 0.0f, "negative volume clamped to 0.0");
                Check(sanitized.ResolutionWidth == 1920, "out-of-range resolution sanitized to 1920");
                Check(sanitized.MaxFps == 60, "negative FPS sanitized to 60");
                Check(sanitized.UiScale == 1.0f, "extreme ui_scale sanitized to 1.0");
                Check(UserSettingsStore.HasDiagnosticError && UserSettingsStore.LastDiagnosticMessage!.Contains("Sanitized settings"), "sanitization diagnostic message preserved");

                // Clean up test file
                if (File.Exists(globalTestPath)) File.Delete(globalTestPath);

                GD.Print($"[SettingsSelfTest] Failures: {failures}");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[SettingsSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                failures++;
            }

            return EmitSummary("settings_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
