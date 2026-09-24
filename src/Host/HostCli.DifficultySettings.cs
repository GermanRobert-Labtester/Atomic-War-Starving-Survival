// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 181 (Difficulty Settings System).
// Exercises the signed pure-domain DifficultySettingsSystem through the host
// session over the campaign's single DifficultyPresetCatalog instance.

using System;
using System.IO;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Difficulty;

namespace AtomicWar.GodotApp
{
    public static class HostCliDifficultySettings
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Difficulty Settings Self-Test (Plan 181) ===");
            int passed = 0;
            int total = 12;

            try
            {
                var catalog = DifficultyPresetCatalogLoader.Load(dataDir, new FileSystemIO());

                // Check 1: canonical catalog loads and validates.
                if (catalog.Validate(out string catalogError) && catalog.AllPresets.Count == 4)
                {
                    GD.Print($"[PASS] Check 1: Canonical catalog loaded ({catalog.AllPresets.Count} presets, default '{catalog.default_preset_id}').");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog load/validate failed: {catalogError}");
                }

                var director = new DifficultyDirector(catalog);
                var session = DifficultySettingsHostSession.Create(catalog);

                // Check 2: the session shares the campaign catalog (no second catalog).
                if (session.Presets.Count == catalog.AllPresets.Count && session.Census.PresetCount == 4)
                {
                    GD.Print($"[PASS] Check 2: Settings session bound to the shared catalog ({session.Census.PresetCount} presets).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Shared-catalog binding mismatch (session={session.Presets.Count}, catalog={catalog.AllPresets.Count}).");
                }

                // Check 3: the seeded identity preset resolves to the director's scalars.
                if (session.EffectiveProvider.HasSameScalarsAs(director.ResolveProvider("difficulty_standard")))
                {
                    GD.Print("[PASS] Check 3: Seeded 'difficulty_standard' effective scalars match the canonical director.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Standard preset scalars diverged from the director (active='{session.ActivePresetId}').");
                }

                // Check 4: preset selection changes the effective scalars.
                bool selectedDirge = session.SelectPreset("difficulty_dirge");
                if (selectedDirge && session.ActivePresetId == "difficulty_dirge"
                    && session.EffectiveProvider.HasSameScalarsAs(director.ResolveProvider("difficulty_dirge")))
                {
                    GD.Print("[PASS] Check 4: Selecting 'difficulty_dirge' applies that preset's scalars.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Preset selection failed (selected={selectedDirge}, active='{session.ActivePresetId}').");
                }

                // Check 5: unknown preset fails without mutating state.
                string before = session.ActivePresetId;
                bool unknownRejected = !session.SelectPreset("difficulty_does_not_exist") && session.ActivePresetId == before;
                if (unknownRejected)
                {
                    GD.Print("[PASS] Check 5: Unknown preset is rejected without mutating the active selection.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Unknown preset handling incorrect (active now '{session.ActivePresetId}').");
                }

                // Check 6: a custom slider switches to custom mode and applies.
                bool customSet = session.SetCustomScalar("radiation_gain_mult", 1.8f);
                var customScalars = session.EffectiveScalars;
                if (customSet && session.IsCustom && session.ActivePresetId == "difficulty_custom"
                    && Math.Abs(customScalars.radiation_gain_mult - 1.8f) < 0.001f)
                {
                    GD.Print("[PASS] Check 6: Custom slider sets custom mode and applies the value.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Custom slider failed (custom={session.IsCustom}, radiation={customScalars.radiation_gain_mult}).");
                }

                // Check 7: custom values clamp to the authored [0.25, 2.5] band.
                session.SetCustomScalar("hunger_rate_mult", 0.05f);
                session.SetCustomScalar("thirst_rate_mult", 9.0f);
                var clamped = session.EffectiveScalars;
                if (Math.Abs(clamped.hunger_rate_mult - 0.25f) < 0.001f && Math.Abs(clamped.thirst_rate_mult - 2.5f) < 0.001f)
                {
                    GD.Print("[PASS] Check 7: Custom sliders clamp to [0.25, 2.5].");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Clamp failed (hunger={clamped.hunger_rate_mult}, thirst={clamped.thirst_rate_mult}).");
                }

                // Check 8: the effective provider is a valid typed scalar view.
                var provider = session.EffectiveProvider;
                bool providerValid = provider.PresetId == "difficulty_custom"
                    && provider.HasSameScalarsAs(DifficultyScalarsProvider.FromScalars("difficulty_custom", clamped));
                if (providerValid)
                {
                    GD.Print("[PASS] Check 8: Effective provider carries the custom configuration.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Effective provider mismatch (preset='{provider.PresetId}').");
                }

                // Check 9: locking prevents further preset and slider changes.
                bool locked = session.Lock();
                bool presetBlocked = !session.SelectPreset("difficulty_sparing");
                bool sliderBlocked = !session.SetCustomScalar("radiation_gain_mult", 2.4f);
                if (locked && session.IsLocked && presetBlocked && sliderBlocked)
                {
                    GD.Print("[PASS] Check 9: Ironman lock blocks preset and slider changes.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Lock enforcement failed (locked={locked}, presetBlocked={presetBlocked}, sliderBlocked={sliderBlocked}).");
                }

                // Check 10: capture/restore round-trips preset, custom values, and lock.
                var state = session.CaptureState();
                var restored = DifficultySettingsHostSession.Create(catalog);
                restored.RestoreState(state);
                if (restored.IsLocked && restored.IsCustom
                    && Math.Abs(restored.EffectiveScalars.radiation_gain_mult - 1.8f) < 0.001f)
                {
                    GD.Print("[PASS] Check 10: Settings state round-trips custom values and lock.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Round-trip failed (locked={restored.IsLocked}, custom={restored.IsCustom}).");
                }

                // Check 11: schema gate refuses a newer payload and accepts legacy v1.
                bool newerRejected = false;
                var newer = restored.CaptureState();
                newer.SchemaVersion = 99;
                try { restored.RestoreState(newer); }
                catch (InvalidOperationException) { newerRejected = true; }
                var legacy = restored.CaptureState();
                legacy.SchemaVersion = 0;
                bool legacyAccepted = false;
                try { restored.RestoreState(legacy); legacyAccepted = true; }
                catch (Exception) { legacyAccepted = false; }
                if (newerRejected && legacyAccepted)
                {
                    GD.Print("[PASS] Check 11: Schema gate rejects newer payloads and accepts legacy v1.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Schema gate incorrect (newerRejected={newerRejected}, legacyAccepted={legacyAccepted}).");
                }

                // Check 12: host wiring routes the effective scalars into the XP-01
                // binding field and registers a save section, without a second catalog.
                string wiring = ReadRepoFile("src", "Main.DifficultySettings.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                if (wiring.Contains("_difficultyScalars = _difficultySettings.EffectiveProvider")
                    && wiring.Contains("DifficultySettingsSaveStore.TrySave")
                    && wiring.Contains("EnsureDifficultyCatalog()")
                    && registry.Contains("difficulty_settings")
                    && registry.Contains("difficulty_settings_save.json"))
                {
                    GD.Print("[PASS] Check 12: Host routes effective scalars into XP-01 and registers the settings save section.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: Difficulty settings host wiring or save section missing.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during difficulty settings self-test: {ex}");
            }

            GD.Print($"=== Difficulty Settings Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { /* source-wiring assertion degrades to a failed check */ }
            return string.Empty;
        }
    }
}
