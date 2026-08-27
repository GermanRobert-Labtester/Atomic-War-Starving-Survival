using System;
using Ashfall.Core.Audio;
using Xunit;

namespace Ashfall.Core.Tests.Audio
{
    public class AudioSettingsRecoveryTests
    {
        [Fact]
        public void Deserialize_ValidJson_ReturnsExactValues()
        {
            string json = @"{
                ""version"": 1,
                ""master_volume"": 75.0,
                ""music_volume"": 45.0,
                ""ambience_volume"": 55.0,
                ""sfx_volume"": 85.0,
                ""ui_volume"": 40.0,
                ""voice_volume"": 95.0,
                ""alert_volume"": 90.0,
                ""generator_volume"": 65.0,
                ""ventilation_volume"": 50.0,
                ""radio_volume"": 70.0,
                ""medical_volume"": 60.0,
                ""surface_volume"": 35.0,
                ""master_mute"": false,
                ""music_mute"": true,
                ""sfx_mute"": false,
                ""voice_mute"": false,
                ""alert_mute"": false,
                ""ambience_mute"": true,
                ""ui_mute"": false,
                ""generator_mute"": false,
                ""ventilation_mute"": false,
                ""radio_mute"": true,
                ""medical_mute"": false,
                ""surface_mute"": false
            }";

            var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(json);

            Assert.Null(diag);
            Assert.NotNull(data);
            Assert.Equal(75.0f, data.MasterVolume);
            Assert.Equal(45.0f, data.MusicVolume);
            Assert.True(data.MusicMute);
            Assert.True(data.AmbienceMute);
            Assert.True(data.RadioMute);
            Assert.False(data.MasterMute);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        [InlineData("\r\n\t")]
        public void Deserialize_MissingOrEmptyJson_LoadsDefaultsWithDiagnostic(string? input)
        {
            var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(input);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("empty or null", diag);
            Assert.Equal(100f, data.MasterVolume);
            Assert.Equal(70f, data.MusicVolume);
            Assert.False(data.MasterMute);
        }

        [Fact]
        public void Deserialize_TruncatedOrCorruptSyntax_LoadsDefaultsWithDiagnostic()
        {
            string corruptJson = "{ \"master_volume\": 50, \"music_volume\": ";

            var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(corruptJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Malformed JSON syntax", diag);
            Assert.Equal(100f, data.MasterVolume);
            Assert.Equal(70f, data.MusicVolume);
        }

        [Fact]
        public void Deserialize_PartiallyInvalidJson_PreservesValidValuesAndRestoresDefaultsForInvalid()
        {
            // master_volume is 45 (valid), sfx_mute is true (valid), but music_volume has an invalid string type
            string partialJson = @"{
                ""master_volume"": 45.0,
                ""music_volume"": ""INVALID_VOLUME_TYPE"",
                ""sfx_mute"": true
            }";

            var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(partialJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Contains("Partially recovered settings", diag);
            Assert.Equal(45.0f, data.MasterVolume); // Valid value PRESERVED
            Assert.True(data.SfxMute); // Valid mute PRESERVED
            Assert.Equal(70.0f, data.MusicVolume); // Invalid field RESTORED TO DEFAULT
            Assert.Equal(60.0f, data.AmbienceVolume); // Missing field HAS DEFAULT
        }

        [Fact]
        public void Deserialize_OutOfRangeVolumes_ClampsSafely()
        {
            string outOfRangeJson = @"{
                ""master_volume"": -50.0,
                ""music_volume"": 350.0
            }";

            var (data, diag) = AudioSettingsCodec.DeserializeWithRecovery(outOfRangeJson);

            Assert.NotNull(data);
            Assert.NotNull(diag);
            Assert.Equal(0.0f, data.MasterVolume);
            Assert.Equal(100.0f, data.MusicVolume);
        }

        [Fact]
        public void Sanitize_NonFiniteNumbers_RestoresDefaults()
        {
            var data = new AudioSettingsData
            {
                MasterVolume = float.NaN,
                MusicVolume = float.PositiveInfinity,
                SfxVolume = float.NegativeInfinity
            };

            var sanitized = AudioSettingsCodec.Sanitize(data, out string? diag);

            Assert.NotNull(diag);
            Assert.Equal(100.0f, sanitized.MasterVolume);
            Assert.Equal(70.0f, sanitized.MusicVolume);
            Assert.Equal(80.0f, sanitized.SfxVolume);
        }

        [Fact]
        public void GetEffectiveVolume_MasterMuteOrCategoryMute_ReturnsZero()
        {
            var data = new AudioSettingsData { MasterVolume = 80f };

            Assert.Equal(0.4f, data.GetEffectiveVolume(50f, false), 3);
            Assert.Equal(0.0f, data.GetEffectiveVolume(50f, true), 3);

            data.MasterMute = true;
            Assert.Equal(0.0f, data.GetEffectiveVolume(50f, false), 3);
        }

        [Fact]
        public void Serialize_Roundtrip_PreservesAllValues()
        {
            var data = new AudioSettingsData
            {
                MasterVolume = 65f,
                MusicVolume = 40f,
                VoiceMute = true
            };

            string json = AudioSettingsCodec.Serialize(data);
            var (reloaded, diag) = AudioSettingsCodec.DeserializeWithRecovery(json);

            Assert.Null(diag);
            Assert.Equal(65f, reloaded.MasterVolume);
            Assert.Equal(40f, reloaded.MusicVolume);
            Assert.True(reloaded.VoiceMute);
        }
    }
}
