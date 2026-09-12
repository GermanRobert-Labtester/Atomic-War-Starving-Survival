// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Audio
{
    [Serializable]
    public sealed class ShelterAudioCueDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public string bus_id = string.Empty;
        public string playback_mode = "one_shot"; // "loop", "one_shot", "procedural_density"
        public float base_volume_db;
        public int pitch_jitter_permille;
        public string ducking_group = "none";
        public string description = string.Empty;
    }

    [Serializable]
    public sealed class AcousticMixProfileDefinition
    {
        public string id = string.Empty;
        public string display_name = string.Empty;
        public float reverb_room_size;
        public int lowpass_cutoff_hz = 20000;
        public float attenuation_db;
    }

    [Serializable]
    public sealed class ShelterAudioCueCatalog
    {
        public int schema_version = 1;
        public List<ShelterAudioCueDefinition> cues = new List<ShelterAudioCueDefinition>();
        public List<AcousticMixProfileDefinition> mix_profiles = new List<AcousticMixProfileDefinition>();
    }

    public static class ShelterAudioCueCatalogLoader
    {
        public static ShelterAudioCueCatalog Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json))
                throw new ArgumentException("Shelter audio cues JSON string cannot be null or empty.", nameof(json));
            if (serializer == null)
                throw new ArgumentNullException(nameof(serializer));

            var catalog = serializer.Deserialize<ShelterAudioCueCatalog>(json);
            if (catalog == null)
                throw new InvalidOperationException("Failed to deserialize shelter audio cues catalog.");

            return catalog;
        }
    }
}
