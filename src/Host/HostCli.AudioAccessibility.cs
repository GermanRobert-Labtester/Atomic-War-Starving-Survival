// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 169 (Audio Accessibility & Mix Legibility).

using System;
using System.IO;
using Godot;
using Ashfall.Core.Audio;

namespace AtomicWar.GodotApp
{
    public static class HostCliAudioAccessibility
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Audio Accessibility & Mix Legibility Self-Test (Plan 169) ===");
            int passed = 0;
            int total = 12;

            try
            {
                string path = Path.Combine(dataDir, "audio_accessibility_cues.json");
                AudioAccessibilityCatalogData? catalog = null;
                if (File.Exists(path))
                    catalog = AudioAccessibilityCatalogLoader.LoadFromJson(File.ReadAllText(path));

                // Check 1: authored catalog loads through the strict loader
                if (catalog != null && catalog.Cues.Count >= 7 && catalog.MixPresets.Count >= 3)
                {
                    GD.Print($"[PASS] Check 1: Strict loader accepted {catalog.Cues.Count} cues and {catalog.MixPresets.Count} presets.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: Authored accessibility catalog missing or incomplete.");
                }

                // Check 2: rejects duplicate cue_id
                if (ExpectReject("{'schema_version':1,'cues':[{'cue_id':'c1','bus_name':'Sfx','visual_label':'A'},{'cue_id':'c1','bus_name':'Sfx','visual_label':'B'}],'mix_presets':[{'preset_id':'p1','display_name':'P'}]}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 2: Strict loader rejected a duplicate cue_id.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Duplicate cue_id was not rejected.");
                }

                // Check 3: rejects an invalid severity
                if (ExpectReject("{'schema_version':1,'cues':[{'cue_id':'c1','bus_name':'Sfx','visual_label':'A','severity':'Hypercritical'}],'mix_presets':[{'preset_id':'p1','display_name':'P'}]}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 3: Strict loader rejected an invalid severity.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Invalid severity was not rejected.");
                }

                // Check 4: rejects out-of-range ducking
                if (ExpectReject("{'schema_version':1,'cues':[{'cue_id':'c1','bus_name':'Sfx','visual_label':'A','duck_level_db':6.0}],'mix_presets':[{'preset_id':'p1','display_name':'P'}]}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 4: Strict loader rejected a positive duck level.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Out-of-range duck level was not rejected.");
                }

                // Check 5: rejects an empty visual label (the whole point of the feature)
                if (ExpectReject("{'schema_version':1,'cues':[{'cue_id':'c1','bus_name':'Sfx','visual_label':''}],'mix_presets':[{'preset_id':'p1','display_name':'P'}]}".Replace('\'', '"')))
                {
                    GD.Print("[PASS] Check 5: Strict loader rejected a cue with no visual label.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Cue without a visual label was not rejected.");
                }

                // Bind the coordinator from the authored catalog.
                var coordinator = new AudioAccessibilityCoordinator();
                if (catalog != null)
                    coordinator.BindCatalog(catalog);

                // Check 6: catalog bound into the live coordinator
                if (coordinator.Cues.Count >= 7 && coordinator.Presets.Count >= 3)
                {
                    GD.Print($"[PASS] Check 6: Coordinator holds {coordinator.Cues.Count} cues and {coordinator.Presets.Count} presets.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Coordinator not bound ({coordinator.Cues.Count} cues).");
                }

                // Check 7: trigger emits visual notification and applies ducking
                VisualAudioNotification? emitted = null;
                float ducked = 0f;
                coordinator.OnVisualNotificationEmittedSeam = n => emitted = n;
                coordinator.OnDuckingChangedSeam = d => ducked = d;
                bool triggered = coordinator.TriggerCue("cue_raid_incoming", 10.0, out var notif);
                if (triggered && emitted != null && notif != null
                    && notif.CueId == "cue_raid_incoming" && ducked < 0f
                    && Math.Abs(coordinator.ActiveDuckingDb - ducked) < 0.001f)
                {
                    GD.Print($"[PASS] Check 7: Cue emitted a {notif.Severity} visual and ducked to {ducked:0.0} dB.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: Cue did not emit a visual/ducking.");
                }

                // Check 8: alert coalescing suppresses rapid duplicates
                bool first = coordinator.TriggerCue("cue_alarm_general", 100.0, out _);
                bool second = coordinator.TriggerCue("cue_alarm_general", 101.0, out _);
                bool third = coordinator.TriggerCue("cue_alarm_general", 104.0, out _);
                if (first && !second && third && coordinator.CoalescedAlertCount >= 1)
                {
                    GD.Print($"[PASS] Check 8: Coalescing suppressed the in-window duplicate (count={coordinator.CoalescedAlertCount}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Coalescing wrong (first={first}, second={second}, third={third}).");
                }

                // Check 9: ducking release restores zero
                coordinator.ReleaseDucking();
                if (Math.Abs(coordinator.ActiveDuckingDb) < 0.001f)
                {
                    GD.Print("[PASS] Check 9: Ducking released to 0 dB.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Ducking not released ({coordinator.ActiveDuckingDb}).");
                }

                // Check 10: preset application and diagnostic readout
                string? presetSeen = null;
                coordinator.OnMixPresetChangedSeam = id => presetSeen = id;
                bool applied = coordinator.ApplyPreset("preset_reduced_stimulation");
                var readout = coordinator.GetDiagnosticReadout();
                if (applied && presetSeen == "preset_reduced_stimulation"
                    && readout.CurrentPresetId == "preset_reduced_stimulation"
                    && readout.ActiveBuses.Count > 0)
                {
                    GD.Print($"[PASS] Check 10: Preset applied and readout lists {readout.ActiveBuses.Count} active buses.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: Preset application or readout failed.");
                }

                // Check 11: host session binds sinks; the preference gates only the visual layer
                var host = AudioAccessibilityHostSession.Create(dataDir);
                float hostDucked = 0f;
                string? hostPreset = null;
                int hostVisuals = 0;
                host.DuckingSink = d => hostDucked = d;
                host.MixPresetSink = p => hostPreset = p;
                host.VisualNotificationSink = _ => hostVisuals++;
                host.ApplyPreset("preset_compressed");
                host.TriggerCue("cue_raid_incoming", 11.0, out _);

                host.BindVisualAlertsEnabled(() => false);
                host.TriggerCue("cue_medical_emergency", 12.0, out _);

                if (hostPreset == "preset_compressed" && hostDucked < 0f && hostVisuals == 1)
                {
                    GD.Print($"[PASS] Check 11: Host sinks wired; visual-layer gate suppressed the second alert ({hostVisuals} shown).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Host sink wiring wrong (preset={hostPreset}, ducked={hostDucked}, visuals={hostVisuals}).");
                }

                // Check 12: census reports truthful counts
                var census = host.Census;
                if (census.TotalCues == coordinator.Cues.Count
                    && census.TotalMixPresets == coordinator.Presets.Count
                    && census.ActivePresetId == "preset_compressed"
                    && census.ActiveDuckingDb < 0f
                    && census.HasEmittedNotification)
                {
                    GD.Print($"[PASS] Check 12: Census accurate (cues={census.TotalCues}, presets={census.TotalMixPresets}, preset={census.ActivePresetId}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Census mismatch (cues={census.TotalCues}, preset={census.ActivePresetId}).");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in audio accessibility self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Audio Accessibility Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static bool ExpectReject(string json)
        {
            try
            {
                AudioAccessibilityCatalogLoader.LoadFromJson(json);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
