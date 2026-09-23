// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 52 host probe — scarcity audio state machine.
//
// --scarcity-audio-selftest proves, headlessly:
//   * every canonical WeatherKind maps to one of the authority's weather keys
//     (no enum value silently falls through to a default that hides a bug),
//   * every ambience cue the host applies exists in the authored audio cue
//     catalog (no invented cue ids),
//   * the authority's bed transitions, alert ducking and geiger rate bands
//     behave as specified, including the absolute-silence states.
// ============================================================================
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Audio;
using Ashfall.Core.World;
using AtomicWar.GodotApp.Audio;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunScarcityAudioSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; GD.Print($"[PASS] scarcity-audio/{gate}"); }
                else { fail++; GD.Print($"[FAIL] scarcity-audio/{gate}{(note.Length > 0 ? " — " + note : "")}"); }
            }

            // 1 — every canonical weather kind maps to a profiled authority key.
            var unmapped = new List<string>();
            foreach (WeatherKind kind in System.Enum.GetValues<WeatherKind>())
            {
                if (!ScarcityAudioStateMachine.HasWeatherProfile(AtomicWar.GodotApp.Audio.ScarcityAudioController.WeatherKindKey(kind)))
                    unmapped.Add(kind + "->" + AtomicWar.GodotApp.Audio.ScarcityAudioController.WeatherKindKey(kind));
            }
            Check("every_weather_kind_maps_to_authority_key", unmapped.Count == 0, string.Join(",", unmapped));
            Check("authority_covers_all_canonical_kinds",
                ScarcityAudioStateMachine.TotalMappedWeatherKinds == 22,
                ScarcityAudioStateMachine.TotalMappedWeatherKinds.ToString());

            // 2 — every ambience cue the host applies exists in the authored catalog.
            var catalogCues = LoadAudioCueIds(dataDirectory);
            Check("audio_catalog_loaded", catalogCues.Count > 0, catalogCues.Count.ToString());

            var missingCues = new List<string>();
            foreach (WeatherKind kind in System.Enum.GetValues<WeatherKind>())
            {
                string cue = AtomicWar.GodotApp.Audio.ScarcityAudioController.AmbienceCueForWeather(
                    AtomicWar.GodotApp.Audio.ScarcityAudioController.WeatherKindKey(kind));
                if (!catalogCues.Contains(cue)) missingCues.Add($"{kind}->{cue}");
            }
            Check("every_applied_cue_exists_in_catalog", missingCues.Count == 0, string.Join(",", missingCues));

            // Geiger cues are applied by the host too.
            foreach (string geigerCue in new[] { AudioCueCatalog.RadGeigerLoop, AudioCueCatalog.RadGeigerIntense })
                Check($"geiger_cue_exists[{geigerCue}]", catalogCues.Contains(geigerCue));

            // 3 — the authority's bed transitions.
            var machine = new ScarcityAudioStateMachine();
            machine.UpdateContext(onSurface: false, weatherKind: "clear", isGeneratorPowered: true, occupantCount: 4);
            Check("bunker_bed_indoors", machine.CurrentBed == ScarcityAmbienceBed.BunkerAmbience);

            machine.UpdateContext(onSurface: true, weatherKind: "clear");
            Check("surface_bed_outdoors", machine.CurrentBed == ScarcityAmbienceBed.SurfaceAmbience);

            machine.UpdateContext(onSurface: true, weatherKind: "ash_storm");
            Check("storm_bed", machine.CurrentBed == ScarcityAmbienceBed.SurfaceStormAmbience);

            machine.UpdateContext(onSurface: true, weatherKind: "silence");
            Check("absolute_silence_bed", machine.CurrentBed == ScarcityAmbienceBed.AbsoluteSilence);

            var silenceProfile = machine.GetWeatherAudioProfile("silence");
            Check("silence_profile_is_absolute", silenceProfile.IsAbsoluteSilence && silenceProfile.VolumeOffsetDb <= -60f);

            machine.UpdateContext(onSurface: true, weatherKind: "clear");
            Check("clear_profile_audible", !machine.GetWeatherAudioProfile("clear").IsAbsoluteSilence);

            // 4 — alert ducking: first alert ducks, alerts cap, release restores.
            float? lastDuck = null;
            machine.OnDuckingChangedSeam += db => lastDuck = db;
            machine.TriggerAlert("alert_cue");
            Check("first_alert_ducks", lastDuck == machine.DuckingPolicy.AlertDuckingAttenuationDb && lastDuck < 0f);
            machine.TriggerAlert("alert_cue_2");
            machine.TriggerAlert("alert_cue_3");
            machine.TriggerAlert("alert_cue_4");
            Check("alerts_cap_at_policy_max", machine.ActiveAlertCount == machine.DuckingPolicy.MaxConcurrentAlerts,
                machine.ActiveAlertCount.ToString());
            for (int i = 0; i < machine.DuckingPolicy.MaxConcurrentAlerts; i++) machine.ReleaseAlert();
            Check("release_restores_mix", machine.ActiveAlertCount == 0 && lastDuck == 0f);

            // 5 — geiger rate bands.
            var radiation = new ScarcityRadiationAudioState();
            radiation.SetExposure(0f);
            Check("no_exposure_no_geiger", !radiation.GeigerLoopActive && radiation.RateBand == GeigerRateBand.Off);

            radiation.SetExposure(0.3f);
            Check("low_band", radiation.IsExposed && radiation.GeigerLoopActive && radiation.RateBand == GeigerRateBand.Low);

            radiation.SetExposure(1.2f);
            Check("medium_band", radiation.RateBand == GeigerRateBand.Medium);

            radiation.SetExposure(5f);
            Check("high_band", radiation.RateBand == GeigerRateBand.High);

            radiation.SetExposure(25f);
            Check("lethal_band", radiation.RateBand == GeigerRateBand.Lethal);

            radiation.EndExposure();
            Check("end_exposure_stops_geiger", !radiation.GeigerLoopActive && radiation.RateBand == GeigerRateBand.Off);

            GD.Print($"[scarcity-audio-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }

        private static HashSet<string> LoadAudioCueIds(string dataDirectory)
        {
            var cues = new HashSet<string>(System.StringComparer.Ordinal);
            string path = System.IO.Path.Combine(dataDirectory, "audio_cues.json");
            if (!System.IO.File.Exists(path)) return cues;

            using var doc = System.Text.Json.JsonDocument.Parse(System.IO.File.ReadAllText(path));
            if (doc.RootElement.TryGetProperty("cues", out var list) && list.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var cue in list.EnumerateArray())
                {
                    if (cue.ValueKind != System.Text.Json.JsonValueKind.Object) continue;
                    if (cue.TryGetProperty("id", out var id) && id.ValueKind == System.Text.Json.JsonValueKind.String)
                        cues.Add(id.GetString() ?? string.Empty);
                    else if (cue.TryGetProperty("cue_id", out var id2) && id2.ValueKind == System.Text.Json.JsonValueKind.String)
                        cues.Add(id2.GetString() ?? string.Empty);
                }
            }
            return cues;
        }
    }
}
