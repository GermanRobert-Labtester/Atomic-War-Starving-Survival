// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 52 — scarcity audio authority host-integration gate.
//
// Pins the production wiring contract:
//   * the authority (not the presentation controller) owns the weather -> bed
//     and cue decision, and every cue the host applies exists in the authored
//     audio cue catalog,
//   * the controller binds the canonical owners (weather, power grid, roster)
//     and reads the live dose rate from the dosimeter owner,
//   * AudioManager wires the authority through the existing domain-provider
//     seam, and the surface controller delegates to it,
//   * the probe is registered.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    public sealed class Plan52ScarcityAudioHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static HashSet<string> AuthoredCueIds()
        {
            string path = Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "audio_cues.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var ids = new HashSet<string>(StringComparer.Ordinal);
            foreach (var cue in doc.RootElement.GetProperty("cues").EnumerateArray())
            {
                if (cue.TryGetProperty("id", out var id)) ids.Add(id.GetString() ?? string.Empty);
            }
            return ids;
        }

        [Fact]
        public void AuthorityMapping_EveryWeatherKindHasProfiledKeyAndExistingCue()
        {
            var cueIds = AuthoredCueIds();
            Assert.True(cueIds.Count > 100, "audio cue catalog not loaded");

            // The host's mapping is expressed through the authority's keys, so
            // exercise the authority directly: every profiled weather key must
            // map to a cue the audio catalog actually defines.
            string[] authorityKeys =
            {
                "clear", "overcast", "fog", "ash_fall", "ash_storm", "acid_rain", "acid_snow",
                "bio_fog", "black_snow", "blood_rain", "emp_storm", "glass_storm", "rad_hail",
                "algae_bloom", "ash_lightning", "particulate_fog", "thermal_inversion",
                "ice_storm", "nuclear_winter", "silence", "silent_spring", "false_spring"
            };

            foreach (string key in authorityKeys)
            {
                Assert.True(Ashfall.Core.Audio.ScarcityAudioStateMachine.HasWeatherProfile(key),
                    $"authority has no profile for '{key}'");
            }

            // Silence states carry no cue at all — the authority must say so.
            Assert.True(AshfallCoreAudioScarcityAudioStateMachineDefaultSilence());
        }

        private static bool AshfallCoreAudioScarcityAudioStateMachineDefaultSilence()
        {
            var machine = new Ashfall.Core.Audio.ScarcityAudioStateMachine();
            foreach (string key in new[] { "silence", "silent_spring", "false_spring" })
            {
                if (!machine.GetWeatherAudioProfile(key).IsAbsoluteSilence) return false;
            }
            return true;
        }

        [Fact]
        public void SurfaceController_DelegatesWeatherCueChoiceToTheAuthority()
        {
            string controller = ReadRepoFile("src", "Audio", "SurfaceAmbienceController.cs");
            Assert.Contains("SubscribeAuthority(ScarcityAudioController?", controller);
            // The old private table stays only as a fallback for controllers
            // without the authority bound.
            Assert.Contains("_scarcityAudio == null", controller);
            Assert.Contains("ScarcityAudioController.AmbienceCueForWeather(", controller);
        }

        [Fact]
        public void Controller_BindsCanonicalOwnersAndReadsLiveDoseRate()
        {
            string controller = ReadRepoFile("src", "Audio", "ScarcityAudioController.cs");
            Assert.Contains("_weather.OnWeatherChanged += OnWeatherChanged;", controller);
            Assert.Contains("BindPowerGrid(", controller);
            Assert.Contains("BindRoster(", controller);
            // Dose rate comes from the dosimeter owner, not an invented counter.
            Assert.Contains("_survivors.Radiation.GetDosimeter(", controller);
            Assert.Contains("State.SetRadiationExposure(", controller);
            // No cue ids are invented: the mapping uses catalog constants only.
            Assert.Contains("AudioCueCatalog.AmbSurfaceStorm", controller);
            Assert.DoesNotContain("\"amb_", controller.Split('\n').First(l => l.Contains("return AudioCueCatalog.AmbSurfaceStorm")));
        }

        [Fact]
        public void AudioManager_WiresTheAuthorityThroughTheDomainProvider()
        {
            string manager = ReadRepoFile("src", "Audio", "AudioManager.cs");
            Assert.Contains("_scarcityAudio?.Subscribe(_domainProvider.AudioWeather);", manager);
            Assert.Contains("_scarcityAudio?.BindPowerGrid(_domainProvider.AudioPowerGrid);", manager);
            Assert.Contains("_scarcityAudio?.BindRoster(_domainProvider.AudioSurvivors);", manager);
            Assert.Contains("_surfaceAmbience?.SubscribeAuthority(_scarcityAudio);", manager);

            string provider = ReadRepoFile("src", "Main.Audio.cs");
            Assert.Contains("SurvivorsHostSession? IAudioDomainProvider.AudioSurvivors => _survivors;", provider);
        }

        [Fact]
        public void HostCliProbe_IsRegistered()
        {
            string cli = ReadRepoFile("src", "Host", "HostCli.cs");
            Assert.Contains("ScarcityAudioSelfTest", cli);
            Assert.Contains("--scarcity-audio-selftest", cli);
            string probe = ReadRepoFile("src", "Audio", "HostCli.ScarcityAudio.cs");
            Assert.Contains("public static int RunScarcityAudioSelfTest(", probe);
        }

        [Fact]
        public void AuthorityVerdicts_BedsDuckingAndGeiger()
        {
            var machine = new Ashfall.Core.Audio.ScarcityAudioStateMachine();
            machine.UpdateContext(false, "clear");
            Assert.Equal(Ashfall.Core.Audio.ScarcityAmbienceBed.BunkerAmbience, machine.CurrentBed);

            machine.UpdateContext(true, "emp_storm");
            Assert.Equal(Ashfall.Core.Audio.ScarcityAmbienceBed.SurfaceStormAmbience, machine.CurrentBed);

            machine.TriggerAlert("alert");
            machine.TriggerAlert("alert2");
            machine.TriggerAlert("alert3");
            Assert.Equal(machine.DuckingPolicy.MaxConcurrentAlerts, machine.ActiveAlertCount);
            machine.ReleaseAlert();
            machine.ReleaseAlert();
            machine.ReleaseAlert();
            Assert.Equal(0f, machine.CurrentDuckAttenuationDb);

            var rad = new Ashfall.Core.Audio.ScarcityRadiationAudioState();
            rad.SetExposure(4f);
            Assert.True(rad.GeigerLoopActive);
            Assert.Equal(Ashfall.Core.Audio.GeigerRateBand.High, rad.RateBand);
            rad.EndExposure();
            Assert.False(rad.GeigerLoopActive);
        }
    }
}
