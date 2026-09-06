# CODEX READ-MODEL CONTRACT
## Pure Functional Projection of Settlement Knowledge

**Document Version:** 1.0.0
**Scope:** Plan 74 (Codex Knowledge Architecture)
**Status:** Canonical Implementation Contract

---

## 1. Interface & Projection Contract

```csharp
namespace Ashfall.Core.Codex
{
    public enum CodexCategory
    {
        Ecology = 0,
        Technology = 1,
        WastelandLore = 2,
        SurvivalOperations = 3,
        Factions = 4
    }

    public enum CodexEntryState
    {
        Locked = 0,
        Studying = 1,
        Known = 2
    }

    public sealed class CodexEntryProjection
    {
        public string EntryId { get; set; } = string.Empty;
        public CodexCategory Category { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Subtitle { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public CodexEntryState State { get; set; }
        public int DayLearned { get; set; }
        public InformationConfidence Confidence { get; set; }
        public IReadOnlyList<CampaignProvenanceRecord> Provenance { get; set; } = Array.Empty<CampaignProvenanceRecord>();
        public IReadOnlyList<string> RelatedLocationIds { get; set; } = Array.Empty<string>();
        public IReadOnlyList<string> Tags { get; set; } = Array.Empty<string>();
    }

    public static class CodexProjectionBuilder
    {
        public static IReadOnlyList<CodexEntryProjection> Build(
            FieldGuideCatalog? fieldGuide,
            ResearchState? researchState,
            IReadOnlyDictionary<string, ResearchKnowledgeDef>? researchCatalog,
            JournalSystem? journalSystem,
            int currentDay = 1);
    }
}
```

---

## 2. Invariants

1. **Zero Persistence Invariant:**
   - No `CodexState` or `CodexSaveStore` shall exist.
   - The Codex projection is created on-demand when the player opens the UI or requests settlement knowledge intel.
2. **Determinism Invariant:**
   - Projections from identical system states produce bitwise identical order and content.
   - Never iterate `Dictionary` or `HashSet` without ordinal sorting.
3. **No Redaction Leakage:**
   - When `State == CodexEntryState.Studying`, detailed mechanics and lore secrets are masked by placeholder text.
   - When `State == CodexEntryState.Locked`, text is inaccessible.
