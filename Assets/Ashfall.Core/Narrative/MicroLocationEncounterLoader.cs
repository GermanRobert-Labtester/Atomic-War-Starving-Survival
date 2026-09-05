using System;
using System.Collections.Generic;
using Ashfall.Core.Content;
using Ashfall.Core.IO;

namespace Ashfall.Core.Narrative
{
    /// <summary>
    /// F6 / Section 6.3: Dedicated loader for micro_locations.json.
    /// Loads micro-location encounter definitions, stamps each with isMicroLocation = true
    /// and sourceFile = "micro_locations.json", and preserves all extended choice fields.
    /// </summary>
    public static class MicroLocationEncounterLoader
    {
        public const string FileName = "micro_locations.json";

        public static List<EncounterDefinition> Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new List<EncounterDefinition>();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
                return result;

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
                return result;

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
                return result;

            try
            {
                var parsed = CatalogLocator.LoadWrappedList<EncounterDefinition>(raw, SystemTextJsonSerializer.Options).ToArray();
                if (parsed == null) return result;
                for (int i = 0; i < parsed.Length; i++)
                {
                    var def = parsed[i];
                    if (def == null || string.IsNullOrEmpty(def.id)) continue;
                    if (def.choices == null) def.choices = new List<EncounterChoiceDefinition>();

                    // Explicitly stamp micro-location marker and source file
                    def.isMicroLocation = true;
                    def.sourceFile = FileName;

                    result.Add(def);
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "MicroLocation EncounterDefinition list", ex_CATDIAG);
                return result;
            }

            return result;
        }
    }

    /// <summary>
    /// F6 / Section 6.3 &amp; 6.6 &amp; 10: Encounter catalog builder with source-aware duplicate detection
    /// and deterministic ordering. Composes core, arc, and micro-location definitions into
    /// a single canonical encounter catalog.
    /// </summary>
    public sealed class EncounterCatalogBuilder
    {
        private readonly List<EncounterDefinition> _definitions = new List<EncounterDefinition>();
        private readonly Dictionary<string, string> _sourceByEncounterId = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly List<string> _duplicateDiagnostics = new List<string>();

        public IReadOnlyList<string> DuplicateDiagnostics => _duplicateDiagnostics;

        public EncounterCatalogBuilder Add(EncounterDefinition def, string sourceFile = "")
        {
            if (def == null || string.IsNullOrEmpty(def.id))
                return this;

            string effectiveSource = string.IsNullOrEmpty(def.sourceFile) ? sourceFile : def.sourceFile;
            if (string.IsNullOrEmpty(def.sourceFile))
                def.sourceFile = effectiveSource;

            if (_sourceByEncounterId.TryGetValue(def.id, out var existingSource))
            {
                string diag = $"Duplicate encounter ID '{def.id}' detected in '{effectiveSource}'. Already registered from '{existingSource}'.";
                _duplicateDiagnostics.Add(diag);
                throw new InvalidOperationException(diag);
            }

            _sourceByEncounterId[def.id] = effectiveSource;
            _definitions.Add(def);
            return this;
        }

        public EncounterCatalogBuilder AddRange(IEnumerable<EncounterDefinition> defs, string sourceFile = "")
        {
            if (defs == null) return this;
            foreach (var def in defs)
            {
                Add(def, sourceFile);
            }
            return this;
        }

        public List<EncounterDefinition> Build()
        {
            return new List<EncounterDefinition>(_definitions);
        }
    }
}
