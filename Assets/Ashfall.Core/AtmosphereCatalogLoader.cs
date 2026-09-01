using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core
{
    /// <summary>
    /// Single entry from environmental_atmosphere_expansion.json.
    /// Each entry is a location-flavoured atmosphere text with optional
    /// filtering dimensions (weather, time-phase, condition, sense, tags).
    /// </summary>
    [Serializable]
    public sealed class AtmosphereTextEntry
    {
        /// <summary>Unique snake_case id, e.g. "atm_loc_approach_thermal_plant".</summary>
        public string id;

        /// <summary>Location id this text is anchored to, e.g. "geothermal_plant_ruins".</summary>
        public string location;

        /// <summary>The atmospheric prose itself.</summary>
        public string text;

        /// <summary>Entry type, e.g. "location_description".</summary>
        public string type;

        /// <summary>Free-form tags, e.g. ["approach", "thermal", "steam", "hazard"].</summary>
        public string[] tags;

        /// <summary>Atmosphere mood keywords, e.g. ["danger", "industrial", "unstable"].</summary>
        public string[] atmosphere;

        /// <summary>Primary sense channel, e.g. "smell_sulfur_heat", "sight".</summary>
        public string sense;

        /// <summary>Time-of-day filter: "any", "day", "night", "dusk", "dawn".</summary>
        public string time_phase;

        /// <summary>Weather filter: "any", "clear", "storm", "fog", etc.</summary>
        public string weather;

        /// <summary>Author attribution, e.g. "narrator_observation".</summary>
        public string author;

        /// <summary>Location condition filter: "intact", "normal", "damaged", "destroyed".</summary>
        public string condition;
    }

    /// <summary>Root shape of environmental_atmosphere_expansion.json.</summary>
    [Serializable]
    public sealed class AtmosphereCatalogFile
    {
        public int schema_version;
        public string collection_id;
        public List<AtmosphereTextEntry> environmental_texts = new List<AtmosphereTextEntry>();
    }

    /// <summary>
    /// Loads atmosphere text entries from JSON.
    /// Engine-agnostic: uses IFileIO and IJsonSerializer ports (Invariant 2).
    /// </summary>
    public static class AtmosphereCatalogLoader
    {
        public const string DefaultFileName = "environmental_atmosphere_expansion.json";

        /// <summary>
        /// Loads all atmosphere text entries from the data directory.
        /// Returns an empty list when the file is missing, empty, or malformed.
        /// </summary>
        public static List<AtmosphereTextEntry> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return new List<AtmosphereTextEntry>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<AtmosphereTextEntry>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<AtmosphereTextEntry>();

            var container = json.Deserialize<AtmosphereCatalogFile>(rawText);
            return container?.environmental_texts ?? new List<AtmosphereTextEntry>();
        }

        /// <summary>
        /// Loads the catalog and feeds entries into an <see cref="AtmosphereTextSystem"/>.
        /// Returns the number of entries loaded.
        /// </summary>
        public static int LoadAndRegister(
            AtmosphereTextSystem system,
            string dataDir,
            IFileIO fileIO,
            IJsonSerializer json)
        {
            if (system == null) return 0;
            var entries = Load(dataDir, fileIO, json);
            system.LoadCatalog(entries);
            return entries.Count;
        }
    }
}
