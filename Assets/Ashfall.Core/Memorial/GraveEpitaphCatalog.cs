// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.Memorial
{
    [Serializable]
    public sealed class WastelandGraveEpitaphEntry
    {
        public string cause = string.Empty;
        public string epitaph = string.Empty;
    }

    [Serializable]
    public sealed class WastelandGraveEpitaphContainer
    {
        public int schema_version;
        public List<WastelandGraveEpitaphEntry> epitaphs = new List<WastelandGraveEpitaphEntry>();
    }

    /// <summary>
    /// Loads authored grave epitaphs and memorial inscriptions from wasteland_grave_epitaphs.json.
    /// Provides deterministic, seed-based selection per cause of death for environmental graves
    /// and shelter memorialization.
    /// Engine-agnostic; uses IFileIO, IJsonSerializer, and ISeededRng.
    /// </summary>
    public sealed class GraveEpitaphCatalog
    {
        public const string DefaultFileName = "wasteland_grave_epitaphs.json";

        private readonly List<WastelandGraveEpitaphEntry> _entries = new List<WastelandGraveEpitaphEntry>();
        private readonly Dictionary<string, List<WastelandGraveEpitaphEntry>> _byCause =
            new Dictionary<string, List<WastelandGraveEpitaphEntry>>(StringComparer.OrdinalIgnoreCase);

        public int TotalCount => _entries.Count;
        public IReadOnlyList<WastelandGraveEpitaphEntry> AllEntries => _entries;

        public static List<WastelandGraveEpitaphEntry> Load(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null)
        {
            fileIO ??= new FileSystemIO();
            json ??= new SystemTextJsonSerializer();

            if (string.IsNullOrEmpty(dataDir))
                return new List<WastelandGraveEpitaphEntry>();

            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new List<WastelandGraveEpitaphEntry>();

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return new List<WastelandGraveEpitaphEntry>();

            try
            {
                var container = json.Deserialize<WastelandGraveEpitaphContainer>(raw);
                return container?.epitaphs ?? new List<WastelandGraveEpitaphEntry>();
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "WastelandGraveEpitaphEntry list", ex_CATDIAG);
                return new List<WastelandGraveEpitaphEntry>();
            }
        }

        public static GraveEpitaphCatalog LoadFromDataDir(string dataDir, IFileIO? fileIO = null, IJsonSerializer? json = null)
        {
            var catalog = new GraveEpitaphCatalog();
            var entries = Load(dataDir, fileIO, json);
            catalog.Populate(entries);
            return catalog;
        }

        public void Populate(IEnumerable<WastelandGraveEpitaphEntry> entries)
        {
            _entries.Clear();
            _byCause.Clear();

            if (entries == null) return;

            foreach (var entry in entries)
            {
                if (entry == null || string.IsNullOrWhiteSpace(entry.epitaph)) continue;

                _entries.Add(entry);
                string key = entry.cause?.Trim() ?? string.Empty;
                if (!_byCause.TryGetValue(key, out var list))
                {
                    list = new List<WastelandGraveEpitaphEntry>();
                    _byCause[key] = list;
                }
                list.Add(entry);
            }
        }

        public IReadOnlyList<WastelandGraveEpitaphEntry> GetEpitaphsForCause(string cause)
        {
            if (string.IsNullOrWhiteSpace(cause))
                cause = "unspecified";

            if (_byCause.TryGetValue(cause, out var list))
                return list;

            if (_byCause.TryGetValue("unspecified", out var unspecList))
                return unspecList;

            if (_byCause.TryGetValue("unknown", out var unknownList))
                return unknownList;

            return _entries;
        }

        public string SelectEpitaph(string cause, ISeededRng? rng = null)
        {
            var pool = GetEpitaphsForCause(cause);
            if (pool.Count == 0)
                return "The lamp went out.";

            if (rng == null)
                return pool[0].epitaph;

            int index = rng.Next(0, pool.Count);
            return pool[index].epitaph;
        }

        public string SelectEpitaph(string cause, int seed)
        {
            return SelectEpitaph(cause, new SeededRng(seed));
        }
    }
}
