# CAMPAIGN PROVENANCE CONTRACT
## Unified Provenance & Information Lineage Specification

**Document Version:** 1.0.0
**Domain:** Cross-System Strategic Information Architecture
**Status:** Canonical Standard for Plans 72–75

---

## 1. Purpose

In ASHFALL, the settlement's knowledge is not omniscient or synthetic. Every map location, codex fact, briefing report, and faction event originates from an authoritative producer with clear lineage. This contract defines the typed data structures, lifecycle rules, and serialization standards for campaign provenance.

---

## 2. Core Typed Contracts

### 2.1 `KnowledgeSourceKind`

```csharp
namespace Ashfall.Core.Campaign
{
    public enum KnowledgeSourceKind
    {
        ExpeditionVisit = 0,
        ExpeditionSurvey = 1,
        RadioIntercept = 2,
        TraderRumor = 3,
        FieldGuide = 4,
        Research = 5,
        JournalEvidence = 6,
        Manual = 7,
        Autopsy = 8,
        FactionMuster = 9,
        TreatyEvent = 10,
        WorldEvent = 11,
        ShelterSystem = 12,
        NarrativeArticle = 13
    }
}
```

### 2.2 `InformationConfidence`

```csharp
namespace Ashfall.Core.Campaign
{
    public enum InformationConfidence
    {
        Low = 0,        // Rumor, unverified transmission, distant hearsay
        Medium = 1,     // Triangulated signal, secondhand report with physical corroboration
        High = 2,       // Scientific instrument reading, cartographic survey, official document
        Confirmed = 3   // Direct physical observation, completed research, survivor on-site presence
    }
}
```

### 2.3 `CampaignProvenanceRecord`

```csharp
namespace Ashfall.Core.Campaign
{
    [Serializable]
    public sealed class CampaignProvenanceRecord
    {
        public KnowledgeSourceKind SourceKind { get; set; }
        public string SourceId { get; set; } = string.Empty;
        public string ProducerSystemId { get; set; } = string.Empty;
        public int DayObserved { get; set; }
        public InformationConfidence Confidence { get; set; }
        public string? RelatedEntityId { get; set; }

        public CampaignProvenanceRecord Clone() => new CampaignProvenanceRecord
        {
            SourceKind = SourceKind,
            SourceId = SourceId,
            ProducerSystemId = ProducerSystemId,
            DayObserved = DayObserved,
            Confidence = Confidence,
            RelatedEntityId = RelatedEntityId
        };
    }
}
```

---

## 3. Operational Invariants

1. **No Localized Strings as Authority:** Provenance records store typed identifiers (`SourceKind`, `SourceId`, `ProducerSystemId`), never translated UI text or flavor prose.
2. **Deterministic Merge & Union:** When two independent systems discover the same underlying fact:
   - Provenance entries union without duplicates.
   - The effective `DayObserved` is `Min(DayA, DayB)` (earliest discovery date).
   - The effective `Confidence` is `Max(ConfidenceA, ConfidenceB)` (highest certainty).
3. **Immutability of History:** Once a fact's observation is recorded with a given day and source, that observation cannot be retroactively altered or deleted by subsequent reads.
4. **Serialization Portability:** The provenance record schema is primitives-only and supports versioned round-trips via `SystemTextJsonSerializer` without engine-specific types.
