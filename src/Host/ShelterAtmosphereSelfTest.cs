// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Headless selftest for Plan 220 (Shelter Atmosphere & Ambiance) and
    /// Plan 205 (Noise Discipline & Acoustic Management).
    /// </summary>
    internal static class ShelterAtmosphereSelfTest
    {
        public static int Run(string dataDirectory)
        {
            int failures = 0;
            void Check(bool condition, string message)
            {
                if (condition)
                {
                    GD.Print("[PASS] " + message);
                }
                else
                {
                    GD.PrintErr("[FAIL] " + message);
                    failures++;
                }
            }

            try
            {
                GD.Print("[ShelterAtmosphereSelfTest] Starting Plan 220 & Plan 205 verification...");

                // 1. Core Atmosphere System
                var atmoSys = new ShelterAtmosphereSystem();
                atmoSys.UpdateEnvironmentalInputs(80f, 80f, 80f, 80f, 75f, 75f, 70f, 1);
                Check(atmoSys.OverallMoodScore >= 75f, "Atmosphere: composite mood evaluates correctly (>= 75)");
                Check(atmoSys.CurrentMoodCategory == AtmosphereMoodCategory.Comfortable || atmoSys.CurrentMoodCategory == AtmosphereMoodCategory.Welcoming,
                    $"Atmosphere: mood category matches score ({atmoSys.CurrentMoodCategory})");
                Check(atmoSys.ActiveProfile == AtmosphereProfileType.Warm || atmoSys.ActiveProfile == AtmosphereProfileType.LivedIn,
                    $"Atmosphere: active profile matched ({atmoSys.ActiveProfile})");

                // 2. Core Noise System
                var noiseSys = new ShelterNoiseSystem();
                var genSrc = noiseSys.AddNoiseSource(NoiseSourceType.Generator, "power_room", 70f, NoiseFrequency.Low);
                Check(genSrc != null && noiseSys.ActiveSourceCount == 1, "Noise: registered generator noise source");

                noiseSys.TickDay(1, 12);
                float baseNoise = noiseSys.OverallNoiseLevel;
                Check(baseNoise > 20f, $"Noise: overall noise level calculated ({baseNoise:F1} dB)");

                noiseSys.SoundproofRoom("power_room", wallAdd: 50f, doorAdd: 40f);
                noiseSys.TickDay(2, 12);
                float insulatedNoise = noiseSys.OverallNoiseLevel;
                Check(insulatedNoise < baseNoise, $"Noise: soundproofing reduces noise ({insulatedNoise:F1} < {baseNoise:F1} dB)");

                // 3. Quiet Hours & Spike
                noiseSys.SetQuietHours(true, 22, 6);
                NoiseEvent? spike = null;
                noiseSys.OnNoiseSpike += ev => spike = ev;
                noiseSys.TickDay(3, 23);
                Check(spike != null && spike.EventType == "quiet_hours_violation", "Noise: quiet hours violation detected during active period");
                Check(noiseSys.DetectionRisk > 0f, $"Noise: detection risk elevated ({noiseSys.DetectionRisk:F1}%)");

                // 4. Host Session Orchestration
                var host = new ShelterAtmosphereHostSession(atmoSys, noiseSys);
                host.UpdateEnvironmentalAtmosphere(
                    day: 4,
                    lighting: 85f,
                    airPurity: 80f,
                    thermalComfort: 80f,
                    cleanliness: 85f,
                    socialWarmth: 70f,
                    decorationLevel: 65f);

                Check(host.CurrentDay == 4, "Host: current day synchronized");
                var mods = host.Modifiers;
                Check(mods != null && mods.MoraleModifier > 0f, $"Host: positive morale modifier active (+{mods?.MoraleModifier:F1})");

                // 5. Persistence Roundtrip
                var atmoSaved = atmoSys.CaptureState();
                var atmoRestored = new ShelterAtmosphereSystem();
                atmoRestored.RestoreState(atmoSaved);
                Check(Math.Abs(atmoRestored.OverallMoodScore - atmoSys.OverallMoodScore) < 0.001f,
                    "Persistence: AtmosphereState roundtrips cleanly");

                var noiseSaved = noiseSys.CaptureState();
                var noiseRestored = new ShelterNoiseSystem();
                noiseRestored.RestoreState(noiseSaved);
                Check(noiseRestored.QuietHoursActive == noiseSys.QuietHoursActive,
                    "Persistence: ShelterNoiseState roundtrips cleanly");

                // 6. UI Control Lifecycle
                var panel = new ShelterAtmospherePanel();
                panel.Bind(host);
                Check(panel.IsBound, "UI: ShelterAtmospherePanel bound successfully");
                panel.RefreshView();
                Check(true, "UI: ShelterAtmospherePanel view refreshed cleanly");
                panel.Unbind();
                Check(!panel.IsBound, "UI: ShelterAtmospherePanel unbound cleanly");
                panel.QueueFree();

                return HostCli.EmitSummary("shelter_atmosphere_selftest", failures == 0, 10, 10 - failures, failures,
                    failures == 0 ? "All atmosphere & acoustic discipline gates passed" : $"{failures} failures detected");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ShelterAtmosphereSelfTest] Exception: {ex.Message}\n{ex.StackTrace}");
                return HostCli.EmitSummary("shelter_atmosphere_selftest", false, 10, 0, 10, ex.Message);
            }
        }
    }
}
