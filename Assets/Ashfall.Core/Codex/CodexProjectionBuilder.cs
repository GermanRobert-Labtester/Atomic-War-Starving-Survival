using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Campaign;
using Ashfall.Core.Journal;
using Ashfall.Core.World;

namespace Ashfall.Core.Codex
{
    /// <summary>
    /// Pure functional projection builder for the settlement Codex.
    /// Invariant: Zero persistent save state. Derives all facts on demand
    /// from authoritative upstream systems.
    /// </summary>
    public static class CodexProjectionBuilder
    {
        public static IReadOnlyList<CodexEntryProjection> Build(
            FieldGuideCatalog? fieldGuide,
            ResearchState? researchState,
            IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog,
            JournalSystem? journalSystem,
            int currentDay = 1)
        {
            var rawEntries = new List<CodexEntryProjection>();

            // 1. Field Guide entries (Flora, Fauna, Ecology)
            if (fieldGuide != null)
            {
                foreach (var fg in fieldGuide.Entries)
                {
                    if (fg == null || string.IsNullOrEmpty(fg.Id)) continue;
                    bool unlocked = fieldGuide.IsUnlocked(fg.Id);
                    if (!unlocked) continue; // Only unlocked observations appear in Codex

                    var prov = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.FieldGuide,
                        fg.Id,
                        "field_guide_catalog",
                        1,
                        InformationConfidence.Confirmed,
                        fg.SubjectId);

                    rawEntries.Add(new CodexEntryProjection
                    {
                        EntryId = $"codex_fg_{fg.Id}",
                        Category = CodexCategory.Ecology,
                        Title = string.IsNullOrEmpty(fg.CommonName) ? fg.Id : fg.CommonName,
                        Subtitle = fg.ScientificName ?? string.Empty,
                        Body = string.IsNullOrEmpty(fg.Observation) ? (fg.FieldIntel ?? string.Empty) : fg.Observation,
                        State = CodexEntryState.Known,
                        DayLearned = 1,
                        Confidence = InformationConfidence.Confirmed,
                        Provenance = new[] { prov },
                        RelatedLocationIds = Array.Empty<string>(),
                        Tags = fg.Tags != null ? fg.Tags.ToArray() : Array.Empty<string>()
                    });
                }
            }

            // 2. Research & Technology
            if (researchState != null)
            {
                var completedSet = new HashSet<string>(researchState.completedIds ?? Enumerable.Empty<string>(), StringComparer.Ordinal);
                var activeId = researchState.activeResearchId;

                if (researchCatalog != null)
                {
                    var sortedTechKeys = researchCatalog.Keys.OrderBy(k => k, StringComparer.Ordinal);
                    foreach (var techKey in sortedTechKeys)
                    {
                        var def = researchCatalog[techKey];
                        if (def == null) continue;

                        bool isCompleted = completedSet.Contains(def.id) || def.isCompleted;
                        bool isActive = string.Equals(activeId, def.id, StringComparison.Ordinal);
                        bool isUnlocked = (researchState.unlockedIds != null && researchState.unlockedIds.Contains(def.id)) || def.isUnlocked;

                        if (!isCompleted && !isActive && !isUnlocked) continue;

                        CodexEntryState state = isCompleted ? CodexEntryState.Known :
                                                isActive ? CodexEntryState.Studying :
                                                CodexEntryState.Locked;

                        string body = isCompleted ? (def.description ?? string.Empty) :
                                      isActive ? "Under active laboratory analysis..." :
                                      "Prerequisites met. Ready for research study.";

                        var prov = new CampaignProvenanceRecord(
                            KnowledgeSourceKind.Research,
                            def.id,
                            "research_system",
                            currentDay,
                            isCompleted ? InformationConfidence.Confirmed : InformationConfidence.High);

                        rawEntries.Add(new CodexEntryProjection
                        {
                            EntryId = $"codex_tech_{def.id}",
                            Category = CodexCategory.Technology,
                            Title = string.IsNullOrEmpty(def.displayName) ? def.id : def.displayName,
                            Subtitle = def.category ?? "Technology",
                            Body = body,
                            State = state,
                            DayLearned = currentDay,
                            Confidence = isCompleted ? InformationConfidence.Confirmed : InformationConfidence.High,
                            Provenance = new[] { prov },
                            RelatedLocationIds = Array.Empty<string>(),
                            Tags = string.IsNullOrEmpty(def.category) ? Array.Empty<string>() : new[] { def.category }
                        });
                    }
                }
            }

            // 3. Journal Knowledge Evidence & Historical Lore
            if (journalSystem?.Knowledge != null)
            {
                var snap = journalSystem.Knowledge.Snapshot();
                var sortedKeys = snap.OrderBy(k => k, StringComparer.Ordinal);

                foreach (var key in sortedKeys)
                {
                    if (string.IsNullOrEmpty(key)) continue;

                    CodexCategory category;
                    string title;
                    string subtitle;
                    string body = $"Discovered settlement intelligence regarding {key}.";

                    if (key.StartsWith("history_", StringComparison.Ordinal))
                    {
                        category = CodexCategory.WastelandLore;
                        title = HumanizeKey(key);
                        subtitle = "Wasteland History Archive";
                    }
                    else if (key.StartsWith("location_visited_", StringComparison.Ordinal))
                    {
                        string locId = key.Substring("location_visited_".Length);
                        category = CodexCategory.WastelandLore;
                        title = $"Survey Record: {HumanizeKey(locId)}";
                        subtitle = "Exploration Log";
                    }
                    else if (key.StartsWith("room_history_seen_", StringComparison.Ordinal))
                    {
                        string vignetteId = key.Substring("room_history_seen_".Length);
                        category = CodexCategory.SurvivalOperations;
                        title = $"Shelter Vignette: {HumanizeKey(vignetteId)}";
                        subtitle = "Bunker History";
                    }
                    else if (key.StartsWith("glitch_noted_", StringComparison.Ordinal))
                    {
                        string glitchId = key.Substring("glitch_noted_".Length);
                        category = CodexCategory.SurvivalOperations;
                        title = $"Anomaly Report: {HumanizeKey(glitchId)}";
                        subtitle = "Engineering Diagnostic";
                    }
                    else
                    {
                        category = CodexCategory.SurvivalOperations;
                        title = HumanizeKey(key);
                        subtitle = "Survival Doctrine";
                    }

                    var prov = new CampaignProvenanceRecord(
                        KnowledgeSourceKind.JournalEvidence,
                        key,
                        "journal_system",
                        currentDay,
                        InformationConfidence.Confirmed);

                    rawEntries.Add(new CodexEntryProjection
                    {
                        EntryId = $"codex_journal_{key}",
                        Category = category,
                        Title = title,
                        Subtitle = subtitle,
                        Body = body,
                        State = CodexEntryState.Known,
                        DayLearned = currentDay,
                        Confidence = InformationConfidence.Confirmed,
                        Provenance = new[] { prov },
                        RelatedLocationIds = Array.Empty<string>(),
                        Tags = new[] { category.ToString() }
                    });
                }
            }

            // 4. Deduplicate entries sharing the same EntryId and Union Provenance
            var dedupMap = new Dictionary<string, CodexEntryProjection>(StringComparer.Ordinal);
            foreach (var entry in rawEntries)
            {
                if (!dedupMap.TryGetValue(entry.EntryId, out var existing))
                {
                    dedupMap[entry.EntryId] = entry;
                }
                else
                {
                    if (entry.State > existing.State)
                    {
                        existing.State = entry.State;
                        existing.Body = entry.Body;
                    }
                    existing.DayLearned = Math.Min(existing.DayLearned, entry.DayLearned);
                    if (entry.Confidence > existing.Confidence)
                    {
                        existing.Confidence = entry.Confidence;
                    }
                    existing.Provenance = CampaignProvenanceEvaluator.Merge(existing.Provenance, entry.Provenance);
                }
            }

            // 5. Deterministic Sort: Category ordinal -> State descending -> Title (Ordinal) -> EntryId (Ordinal)
            var sorted = dedupMap.Values
                .OrderBy(e => (int)e.Category)
                .ThenByDescending(e => (int)e.State)
                .ThenBy(e => e.Title, StringComparer.Ordinal)
                .ThenBy(e => e.EntryId, StringComparer.Ordinal)
                .ToList();

            return sorted;
        }

        private static string HumanizeKey(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            string s = key.Replace('_', ' ');
            if (s.Length > 0 && char.IsLower(s[0]))
            {
                s = char.ToUpperInvariant(s[0]) + s.Substring(1);
            }
            return s;
        }
    }
}
