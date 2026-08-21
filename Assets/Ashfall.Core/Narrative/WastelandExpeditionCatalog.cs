using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Narrative
{
    [Serializable]
    public sealed class ExpeditionChoiceEntry
    {
        public string choice_id;
        public string label;
        public string skill_required;
        public string outcome_success;
        public string outcome_risk;
    }

    [Serializable]
    public sealed class WastelandExpeditionEntry
    {
        public string expedition_id;
        public string title;
        public string zone;
        public string ambient_prose;
        public string threat_description;
        public List<ExpeditionChoiceEntry> choices = new List<ExpeditionChoiceEntry>();
        public string[] tags;
    }

    [Serializable]
    public sealed class WastelandExpeditionFile
    {
        public int schema_version;
        public string collection_id;
        public List<WastelandExpeditionEntry> expeditions = new List<WastelandExpeditionEntry>();
    }

    /// <summary>
    /// Engine-agnostic loader and choice evaluator for The 30 Landmark Wasteland Expedition Branching Scriptbooks.
    /// </summary>
    public sealed class WastelandExpeditionCatalog
    {
        private readonly Dictionary<string, WastelandExpeditionEntry> _byId =
            new Dictionary<string, WastelandExpeditionEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly List<WastelandExpeditionEntry> _allExpeditions = new List<WastelandExpeditionEntry>();

        public IReadOnlyList<WastelandExpeditionEntry> AllExpeditions => _allExpeditions;

        public void Load(string json, IJsonSerializer serializer)
        {
            if (string.IsNullOrEmpty(json) || serializer == null) return;
            var file = serializer.Deserialize<WastelandExpeditionFile>(json);
            if (file?.expeditions == null) return;

            foreach (var e in file.expeditions)
            {
                if (e == null || string.IsNullOrEmpty(e.expedition_id)) continue;
                _byId[e.expedition_id] = e;
                _allExpeditions.Add(e);
            }
        }

        public WastelandExpeditionEntry? GetById(string expeditionId)
        {
            if (string.IsNullOrEmpty(expeditionId)) return null;
            _byId.TryGetValue(expeditionId, out var entry);
            return entry;
        }

        public List<WastelandExpeditionEntry> GetByZone(string zoneSnippet)
        {
            var results = new List<WastelandExpeditionEntry>();
            if (string.IsNullOrEmpty(zoneSnippet)) return results;

            for (int i = 0; i < _allExpeditions.Count; i++)
            {
                var e = _allExpeditions[i];
                if (e.zone != null && e.zone.IndexOf(zoneSnippet, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    results.Add(e);
                }
            }
            return results;
        }

        public List<WastelandExpeditionEntry> GetByTag(string tag)
        {
            var results = new List<WastelandExpeditionEntry>();
            if (string.IsNullOrEmpty(tag)) return results;

            for (int i = 0; i < _allExpeditions.Count; i++)
            {
                var e = _allExpeditions[i];
                if (e.tags == null) continue;
                for (int j = 0; j < e.tags.Length; j++)
                {
                    if (string.Equals(e.tags[j], tag, StringComparison.OrdinalIgnoreCase))
                    {
                        results.Add(e);
                        break;
                    }
                }
            }
            return results;
        }
    }
}
