using System;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Loads WeatherGate definitions from weather_route_gates.json.
    ///
    /// Parse contract (mirrors WeatherProfileLoader):
    ///   1. try the canonical shape (root object with "gates" array);
    ///   2. on failure, fall back to the bare-array shape;
    ///   3. if both fail, return null — the catalog stays empty and every
    ///      gate-dependent consumer keeps its pre-gate behaviour. Missing
    ///      optional data stays silent-empty by design (not a failure), but
    ///      every parse failure is reported through CatalogDiagnostics.
    /// </summary>
    public static class WeatherGateFile
    {
        public const string FileName = "weather_route_gates.json";
        public const int CurrentSchemaVersion = WeatherGateCatalogLoader.CurrentSchemaVersion;

        public static WeatherGateCatalog? Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return null;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return null; // optional file: catalog stays empty, silently

            var catalog = WeatherGateCatalogLoader.LoadFromDirectory(dataDir, fileIO, json);
            return catalog;
        }

        [Serializable]
        public sealed class WeatherGateEnvelope
        {
            public int schema_version = 1;
            public System.Collections.Generic.List<WeatherGateDef> gates = new System.Collections.Generic.List<WeatherGateDef>();
        }
    }
}
