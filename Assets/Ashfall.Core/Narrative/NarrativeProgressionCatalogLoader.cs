// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.IO;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class NarrativeProgressionEntry
    {
        public string description { get; set; } = string.Empty;
        public int order { get; set; }
    }

    [Serializable]
    public sealed class NarrativeProgressionContainer
    {
        public int schema_version { get; set; } = 1;
        public List<NarrativeProgressionEntry> entries { get; set; } = new List<NarrativeProgressionEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader for the 15 Narrative Progression Chapters.
    /// Canonical data authority: Assets/StreamingAssets/Data/narrative_progression.json
    /// </summary>
    public static class NarrativeProgressionCatalogLoader
    {
        public const string DefaultFileName = "narrative_progression.json";

        public static List<NarrativeProgressionEntry> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (string.IsNullOrEmpty(dataDir) || fileIO == null || json == null)
            {
                return new List<NarrativeProgressionEntry>();
            }

            string fullPath = Path.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(fullPath))
            {
                return new List<NarrativeProgressionEntry>();
            }

            try
            {
                string rawText = fileIO.ReadAllText(fullPath);
                var container = json.Deserialize<NarrativeProgressionContainer>(rawText);
                return container?.entries ?? new List<NarrativeProgressionEntry>();
            }
            catch
            {
                // Malformed progression catalog: documented fallback to an empty
                // chapter list; the data-integrity gate owns authoring errors.
                return new List<NarrativeProgressionEntry>();
            }
        }
    }
}
