// SPDX-License-Identifier: MIT
// Plan 169 — strict audio-accessibility catalog loader and census tests.

using System;
using System.IO;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    public sealed class AudioAccessibilityCatalogLoaderTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found");
        }

        private static string AuthoredJson() =>
            File.ReadAllText(Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data", "audio_accessibility_cues.json"));

        [Fact]
        public void Authored_Catalog_Loads_And_Binds()
        {
            var catalog = AudioAccessibilityCatalogLoader.LoadFromJson(AuthoredJson());
            Assert.True(catalog.Cues.Count >= 7);
            Assert.True(catalog.MixPresets.Count >= 3);

            var coordinator = new AudioAccessibilityCoordinator();
            coordinator.BindCatalog(catalog);
            Assert.Equal(catalog.Cues.Count, coordinator.Cues.Count);
            Assert.Equal(catalog.MixPresets.Count, coordinator.Presets.Count);
            Assert.Contains("cue_alarm_general", coordinator.Cues.Keys);
            Assert.Contains("preset_reduced_stimulation", coordinator.Presets.Keys);
        }

        [Theory]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"A\"},{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"B\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"A\",\"severity\":\"Hypercritical\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"A\",\"duck_level_db\":6.0}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"\",\"visual_label\":\"A\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        [InlineData("{\"schema_version\":1,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"A\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"},{\"preset_id\":\"p1\",\"display_name\":\"Q\"}]}")]
        [InlineData("{\"schema_version\":2,\"cues\":[{\"cue_id\":\"c1\",\"bus_name\":\"Sfx\",\"visual_label\":\"A\"}],\"mix_presets\":[{\"preset_id\":\"p1\",\"display_name\":\"P\"}]}")]
        public void Malformed_Catalog_Is_Rejected(string json)
        {
            Assert.Throws<InvalidOperationException>(() => AudioAccessibilityCatalogLoader.LoadFromJson(json));
        }

        [Fact]
        public void Census_Reflects_Live_State()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            coordinator.BindCatalog(AudioAccessibilityCatalogLoader.LoadFromJson(AuthoredJson()));
            coordinator.TriggerCue("cue_raid_incoming", 10.0, out _);
            coordinator.ApplyPreset("preset_reduced_stimulation");

            var census = coordinator.GetCensus();
            Assert.Equal(coordinator.Cues.Count, census.TotalCues);
            Assert.Equal(coordinator.Presets.Count, census.TotalMixPresets);
            Assert.Equal("preset_reduced_stimulation", census.ActivePresetId);
            Assert.True(census.ActiveDuckingDb < 0f);
            Assert.True(census.HasEmittedNotification);
        }

        [Fact]
        public void Coalescing_And_Release_Behave()
        {
            var coordinator = new AudioAccessibilityCoordinator();
            coordinator.BindCatalog(AudioAccessibilityCatalogLoader.LoadFromJson(AuthoredJson()));

            Assert.True(coordinator.TriggerCue("cue_alarm_general", 100.0, out _));
            Assert.False(coordinator.TriggerCue("cue_alarm_general", 101.0, out _));
            Assert.True(coordinator.TriggerCue("cue_alarm_general", 104.0, out _));
            Assert.Equal(1, coordinator.CoalescedAlertCount);

            coordinator.ReleaseDucking();
            Assert.Equal(0f, coordinator.ActiveDuckingDb);
        }
    }
}
