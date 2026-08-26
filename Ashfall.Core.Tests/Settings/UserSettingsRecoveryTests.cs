using System;
using Ashfall.Core.Settings;
using Xunit;

namespace Ashfall.Core.Tests.Settings
{
    public class UserSettingsRecoveryTests
    {
        [Fact]
        public void Deserialize_ValidJson_ReturnsExactValues()
        {
            string json = @"{
                ""schema_version"": 1,
                ""window_mode"": 1,
                ""resolution_width"": 2560,
                ""resolution_height"": 1440,
                ""ui_scale"": 1.25,
                ""vsync"": false,
                ""max_fps"": 144,
                ""master_volume"": 0.75,
                ""music_volume"": 0.50,
                ""sfx_volume"": 0.60,
                ""radio_volume"": 0.85,
                ""ambience_volume"": 0.65,
                ""mute_all"": false,
                ""high_contrast"": true,
                ""hazard_text_labels"": true,
                ""reduced_motion"": true,
                ""large_fonts"": true,
                ""confirm_end_day"": false,
                ""verbose_radio_log"": true,
                ""auto_save_on_day"": false
            }";

            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(json);

            Assert.Null(diag);
            Assert.NotNull(data);
            Assert.Equal(1, data.WindowMode);
            Assert.Equal(2560, data.ResolutionWidth);
            Assert.Equal(1440, data.ResolutionHeight);
            Assert.Equal(1.25f, data.UiScale);
            Assert.False(data.VSync);
            Assert.Equal(144, data.MaxFps);
            Assert.Equal(0.75f, data.MasterVolume);
            Assert.True(data.HighContrast);
            Assert.True(data.ReducedMotion);
            Assert.False(data.ConfirmEndDay);
        }

        [Fact]
        public void Deserialize_TruncatedJson_LoadsSafeDefaultsAndPreservesDiagnostic()
        {
            string truncatedJson = @"{ ""master_volume"": 0.5, ""resolution_width"": 256";

            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(truncatedJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Invalid settings JSON", diag);
            Assert.Equal(1.0f, data.MasterVolume); // Defaults loaded safely
            Assert.Equal(1920, data.ResolutionWidth);
            Assert.Equal(1080, data.ResolutionHeight);
            Assert.Equal(60, data.MaxFps);
        }

        [Fact]
        public void Deserialize_CorruptSyntaxJson_LoadsSafeDefaultsAndPreservesDiagnostic()
        {
            string corruptJson = @"{ NOT_A_VALID_JSON_OBJECT @#$%^&* }";

            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(corruptJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Invalid settings JSON", diag);
            Assert.Equal(1.0f, data.MasterVolume);
            Assert.Equal(60, data.MaxFps);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\t\r\n")]
        public void Deserialize_EmptyOrWhitespace_LoadsSafeDefaultsAndPreservesDiagnostic(string? input)
        {
            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(input);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("empty or whitespace", diag);
            Assert.Equal(1920, data.ResolutionWidth);
            Assert.Equal(1.0f, data.MasterVolume);
        }

        [Fact]
        public void Deserialize_InvalidTypesJson_LoadsSafeDefaultsAndPreservesDiagnostic()
        {
            string invalidTypesJson = @"{ ""master_volume"": ""LOUD"", ""vsync"": 12345 }";

            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(invalidTypesJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Invalid settings JSON", diag);
            Assert.Equal(1.0f, data.MasterVolume);
            Assert.True(data.VSync);
        }

        [Fact]
        public void Deserialize_OutOfRangeValues_SanitizesAndPreservesDiagnostic()
        {
            string outOfRangeJson = @"{
                ""window_mode"": 99,
                ""resolution_width"": 100,
                ""resolution_height"": 99999,
                ""ui_scale"": 50.0,
                ""max_fps"": -200,
                ""master_volume"": 5.5,
                ""music_volume"": -0.8
            }";

            var (data, diag) = UserSettingsCodec.DeserializeWithRecovery(outOfRangeJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Sanitized settings", diag);
            Assert.Equal(0, data.WindowMode); // Clamped to 0
            Assert.Equal(1920, data.ResolutionWidth); // Reset to 1920
            Assert.Equal(1080, data.ResolutionHeight); // Reset to 1080
            Assert.Equal(1.0f, data.UiScale); // Clamped to 1.0
            Assert.Equal(60, data.MaxFps); // Clamped to 60
            Assert.Equal(1.0f, data.MasterVolume); // Clamped to 1.0
            Assert.Equal(0.0f, data.MusicVolume); // Clamped to 0.0
        }

        [Fact]
        public void Sanitize_DirectMutation_SafeguardsAllFields()
        {
            var data = new UserSettingsData
            {
                MasterVolume = float.NaN,
                MusicVolume = float.PositiveInfinity,
                UiScale = float.NegativeInfinity,
                MaxFps = -1
            };

            var sanitized = UserSettingsCodec.Sanitize(data, out string? diag);

            Assert.NotNull(diag);
            Assert.Equal(1.0f, sanitized.MasterVolume);
            Assert.Equal(0.8f, sanitized.MusicVolume);
            Assert.Equal(1.0f, sanitized.UiScale);
            Assert.Equal(60, sanitized.MaxFps);
        }

        [Fact]
        public void Serialize_ProducesSanitizedFormattedJson()
        {
            var data = new UserSettingsData
            {
                MasterVolume = 0.5f,
                MaxFps = 120
            };

            string json = UserSettingsCodec.Serialize(data);
            Assert.Contains("\"master_volume\": 0.5", json);
            Assert.Contains("\"max_fps\": 120", json);

            var (reloaded, diag) = UserSettingsCodec.DeserializeWithRecovery(json);
            Assert.Null(diag);
            Assert.Equal(0.5f, reloaded.MasterVolume);
            Assert.Equal(120, reloaded.MaxFps);
        }
    }
}
