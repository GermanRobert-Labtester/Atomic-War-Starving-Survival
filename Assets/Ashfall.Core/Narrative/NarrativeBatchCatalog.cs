using System;
using System.Collections.Generic;

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class JournalTemplateEntry
    {
        public string template_id;
        public string expansion_id;
        public string author_role;
        public string title;
        public string body_template;
        public string[] tags;
        public float stress_delta;
        public float hope_earned;
    }

    [Serializable]
    public sealed class JournalBatchFile
    {
        public int schema_version;
        public string cycle;
        public List<JournalTemplateEntry> templates = new List<JournalTemplateEntry>();
    }

    [Serializable]
    public sealed class FoundDocumentEntry
    {
        public string doc_id;
        public string title;
        public string origin;
        public string material;
        public string physical_description;
        public string transcript;
        public string[] lore_flags;
        public string item_id_source;
    }

    [Serializable]
    public sealed class FoundDocumentsBatchFile
    {
        public int schema_version;
        public List<FoundDocumentEntry> documents = new List<FoundDocumentEntry>();
    }

    [Serializable]
    public sealed class EulogyArchetypeEntry
    {
        public string archetype_id;
        public string opening;
        public string shift_line;
        public string cooking_line;
        public string relic_line;
        public string closing;
    }

    [Serializable]
    public sealed class EulogyCorpusBatchFile
    {
        public int schema_version;
        public List<EulogyArchetypeEntry> eulogies = new List<EulogyArchetypeEntry>();
    }

    [Serializable]
    public sealed class VelTriageLogFile
    {
        public int schema_version;
        public string log_id;
        public string author;
        public string date_recorded;
        public string location;
        public string[] triage_names;
    }

    /// <summary>
    /// Engine-agnostic loader and validator for Phase 4 narrative batch catalogs.
    /// </summary>
    public sealed class NarrativeBatchCatalog
    {
        public Dictionary<string, JournalTemplateEntry> JournalTemplates { get; } =
            new Dictionary<string, JournalTemplateEntry>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, FoundDocumentEntry> Documents { get; } =
            new Dictionary<string, FoundDocumentEntry>(StringComparer.OrdinalIgnoreCase);

        public Dictionary<string, EulogyArchetypeEntry> Eulogies { get; } =
            new Dictionary<string, EulogyArchetypeEntry>(StringComparer.OrdinalIgnoreCase);

        public List<string> VelTriageNames { get; } = new List<string>();

        public void LoadJournalBatch(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var batch = serializer.Deserialize<JournalBatchFile>(json);
            if (batch?.templates == null) return;

            foreach (var tmpl in batch.templates)
            {
                if (tmpl != null && !string.IsNullOrEmpty(tmpl.template_id))
                {
                    JournalTemplates[tmpl.template_id] = tmpl;
                }
            }
        }

        public void LoadDocumentsBatch(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var batch = serializer.Deserialize<FoundDocumentsBatchFile>(json);
            if (batch?.documents == null) return;

            foreach (var doc in batch.documents)
            {
                if (doc != null && !string.IsNullOrEmpty(doc.doc_id))
                {
                    Documents[doc.doc_id] = doc;
                }
            }
        }

        public void LoadEulogyBatch(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var batch = serializer.Deserialize<EulogyCorpusBatchFile>(json);
            if (batch?.eulogies == null) return;

            foreach (var eulogy in batch.eulogies)
            {
                if (eulogy != null && !string.IsNullOrEmpty(eulogy.archetype_id))
                {
                    Eulogies[eulogy.archetype_id] = eulogy;
                }
            }
        }

        public void LoadVelTriageLog(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var log = serializer.Deserialize<VelTriageLogFile>(json);
            if (log?.triage_names == null) return;

            VelTriageNames.Clear();
            VelTriageNames.AddRange(log.triage_names);
        }
    }
}
