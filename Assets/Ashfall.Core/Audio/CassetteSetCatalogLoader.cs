// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.IO;

namespace Ashfall.Core.Audio
{
    [Serializable]
    public sealed class CassettePartDefinition
    {
        public int part;
        public string item_id = string.Empty;
        public string title = string.Empty;
        public string description = string.Empty;
    }

    [Serializable]
    public sealed class CassetteSetDefinition
    {
        public string set_id = string.Empty;
        public string set_title = string.Empty;
        public int total_parts;
        public List<CassettePartDefinition> parts = new List<CassettePartDefinition>();
        public string hidden_cache_location = string.Empty;
        public List<string> hidden_cache_items = new List<string>();
        public string completion_narrative = string.Empty;
    }

    [Serializable]
    public sealed class CassetteSetsContainer
    {
        public int schema_version;
        public List<CassetteSetDefinition> items = new List<CassetteSetDefinition>();
    }

    /// <summary>
    /// Loads authored cassette sets and multi-part audio narratives from cassette_sets.json.
    /// Engine-agnostic; uses IFileIO and IJsonSerializer ports.
    /// </summary>
    public static class CassetteSetCatalogLoader
    {
        public const string DefaultFileName = "cassette_sets.json";

        public static List<CassetteSetDefinition> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            if (string.IsNullOrEmpty(dataDir))
                return new List<CassetteSetDefinition>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<CassetteSetDefinition>();

            string rawText = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(rawText))
                return new List<CassetteSetDefinition>();

            try
            {
                var container = json.Deserialize<CassetteSetsContainer>(rawText);
                return container?.items ?? new List<CassetteSetDefinition>();
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "CassetteSetDefinition list", ex_CATDIAG);
                return new List<CassetteSetDefinition>();
            }
        }
    }
}
