// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Medical
{
    /// <summary>
    /// One authored medical condition text entry from <c>medical_texts.json</c>.
    /// Presentation and descriptive authority only — no simulation mechanics are derived from this entry.
    /// </summary>
    [Serializable]
    public sealed class MedicalConditionTextEntry
    {
        public string id = string.Empty;
        public string category = string.Empty;
        public string display_name = string.Empty;
        public string diagnosis_text = string.Empty;
        public List<string> symptom_descriptions = new List<string>();
        public List<string> treatment_steps = new List<string>();
        public List<string> required_items = new List<string>();
        public Dictionary<string, double> success_chances = new Dictionary<string, double>();
        public List<string> failure_consequences = new List<string>();
        public List<string> recovery_descriptions = new List<string>();
        public List<string> complication_warnings = new List<string>();
        public List<string> prevention_advice = new List<string>();
        public List<string> long_term_effects = new List<string>();
        public List<string> pain_descriptions = new List<string>();
        public string mental_state = string.Empty;
        public string physical_state = string.Empty;
        public string emotional_impact = string.Empty;
        public string system_integration = string.Empty;
    }

    /// <summary>
    /// Root envelope for <c>medical_texts.json</c>.
    /// </summary>
    [Serializable]
    public sealed class MedicalTextsFile
    {
        public int schema_version = 1;
        public string collection_id = "medical_texts";
        public List<MedicalConditionTextEntry> conditions = new List<MedicalConditionTextEntry>();
    }

    /// <summary>
    /// Engine-agnostic, deterministic query interface and catalog for authored medical prose.
    /// Plan 141 presentation authority.
    /// </summary>
    public sealed class MedicalTextCatalog
    {
        private readonly Dictionary<string, MedicalConditionTextEntry> _byId =
            new Dictionary<string, MedicalConditionTextEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<MedicalConditionTextEntry> _allConditions = new List<MedicalConditionTextEntry>();

        public IReadOnlyList<MedicalConditionTextEntry> AllConditions => _allConditions;

        public int Count => _allConditions.Count;

        /// <summary>
        /// Parses JSON and populates the catalog. Validates against duplicate IDs and sorts deterministically.
        /// </summary>
        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrWhiteSpace(json) || serializer == null) return;

            var file = serializer.Deserialize<MedicalTextsFile>(json);
            if (file?.conditions == null) return;

            _byId.Clear();
            _allConditions.Clear();

            foreach (var condition in file.conditions)
            {
                if (condition == null || string.IsNullOrWhiteSpace(condition.id)) continue;

                if (_byId.ContainsKey(condition.id))
                {
                    // Deduplication safeguard: preserve the first occurrence
                    continue;
                }

                _byId[condition.id] = condition;
                _allConditions.Add(condition);
            }

            // Stable deterministic sort by ID
            _allConditions.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
        }

        /// <summary>
        /// Loads the catalog from disk using Ports, with graceful non-fatal fallback.
        /// </summary>
        public static MedicalTextCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer? serializer = null)
        {
            var catalog = new MedicalTextCatalog();
            serializer ??= new SystemTextJsonSerializer();
            if (string.IsNullOrWhiteSpace(dataDir) || fileIo == null)
                return catalog;

            string path = fileIo.Combine(dataDir, "medical_texts.json");
            if (!fileIo.FileExists(path))
                return catalog;

            try
            {
                string json = fileIo.ReadAllText(path);
                catalog.Load(json, serializer);
            }
            catch (Exception ex)
            {
                // Non-fatal catalog loading failure: returns empty catalog
                Ashfall.Core.IO.CatalogDiagnostics.Warn(path, "MedicalTextsFile", ex);
            }

            return catalog;
        }

        public MedicalConditionTextEntry? TryGetConditionText(string? conditionId)
        {
            if (string.IsNullOrWhiteSpace(conditionId)) return null;
            _byId.TryGetValue(conditionId, out var entry);
            return entry;
        }

        /// <summary>
        /// Returns the display-safe diagnostic summary for a condition.
        /// </summary>
        public string GetSafeDiagnosisSummary(string? conditionId)
        {
            var entry = TryGetConditionText(conditionId);
            return entry != null && !string.IsNullOrWhiteSpace(entry.diagnosis_text)
                ? entry.diagnosis_text
                : string.Empty;
        }

        /// <summary>
        /// Deterministically selects a symptom description line. Keyed by optional seed or stable index.
        /// </summary>
        public string GetSymptomProse(string? conditionId, int? seed = null)
        {
            var entry = TryGetConditionText(conditionId);
            if (entry == null || entry.symptom_descriptions == null || entry.symptom_descriptions.Count == 0)
                return string.Empty;

            int count = entry.symptom_descriptions.Count;
            int index = seed.HasValue
                ? StableHash.NonNegativeRemainder(seed.Value, count)
                : 0;
            return entry.symptom_descriptions[index];
        }

        /// <summary>
        /// Returns the primary complication warning, if present.
        /// </summary>
        public string GetComplicationWarning(string? conditionId)
        {
            var entry = TryGetConditionText(conditionId);
            if (entry == null || entry.complication_warnings == null || entry.complication_warnings.Count == 0)
                return string.Empty;

            return entry.complication_warnings[0];
        }

        /// <summary>
        /// Returns recovery description prose for convalescing patients.
        /// </summary>
        public string GetRecoveryProse(string? conditionId)
        {
            var entry = TryGetConditionText(conditionId);
            if (entry == null || entry.recovery_descriptions == null || entry.recovery_descriptions.Count == 0)
                return string.Empty;

            return entry.recovery_descriptions[0];
        }
    }
}
