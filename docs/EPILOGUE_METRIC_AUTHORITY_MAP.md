# ASHFALL Plan 65 — Grand Epilogue Simulator & Shelter Chronicle Authority Map

**Subsystem:** Grand Epilogue Simulator & Post-Campaign Chronicle
**Core Authority:** `Assets/Ashfall.Core/Campaign/CampaignEpilogueEngine.cs`
**Data Authority:** `Assets/StreamingAssets/Data/campaign_epilogues.json` (`schema_version: 1`)
**Save Store:** Stateless calculation engine / exportable chronicle DTO
**Host & Presentation:** `src/UI/CampaignEpilogueModal.cs`, `assets/ui/panels/CampaignEpilogueModal.tscn`, `src/Main.Plans62_65.cs`

---

## 1. Subsystem Architecture

Plan 65 provides a grand, multi-dimensional narrative closure to an ASHFALL campaign:
1. **Pure Projection / Read-Only Authority:**
   - `CampaignEpilogueEngine` does not mutate game state. It accepts a read-only `CampaignEpilogueSnapshot` populated by campaign subsystems upon victory, defeat, or evacuation.
2. **Deterministic Narrative Assembly:**
   - Evaluates authored vignette cards from `campaign_epilogues.json` against snapshot metrics across 5 critical pillars:
     1. **Demographics & Survival:** Survivor headcount, mortality rate, genetic health, famine deaths.
     2. **Governance & Ethics:** Captive treatment, moral choices, democratic vs authoritarian policies.
     3. **Technological Recovery:** Decrypted pre-war archives, research completion, industrial restoration.
     4. **Wasteland Diplomacy:** Faction standings, regional hegemony, alliances or wars.
     5. **The Long Horizon:** 10-year, 50-year, and 100-year projection of the shelter's progeny.
3. **Structured Export:**
   - Generates a rich, structured chronicle object containing chapters, summary metrics, and narrative prose.
   - Can be exported as a human-readable text file or JSON document for community sharing and player records.

---

## 2. Catalog Schema (`campaign_epilogues.json`)

Each vignette rule contains:
- `id`: Unique string key (e.g., `epilogue_technological_renaissance`, `epilogue_iron_fist_regime`, `epilogue_starvation_graveyard`, `epilogue_diplomatic_unifier`).
- `category`: `demographics`, `governance`, `technology`, `diplomacy`, `horizon`.
- `priority`: Integer priority for resolution conflicts.
- `conditions`: Key-value expressions (e.g. `survivor_count >= 15`, `archives_decrypted >= 4`, `captive_executions == 0`, `reputation_iron_union >= 50`).
- `title`: Chapter heading.
- `narrative_variants`: Array of variant paragraphs, selected deterministically based on seed and nuance flags.

---

## 3. Core Domain Classes

```csharp
namespace Ashfall.Core.Campaign
{
    public sealed class CampaignEpilogueSnapshot
    {
        public int FinalDay { get; set; }
        public int SurvivorsAlive { get; set; }
        public int TotalCasualties { get; set; }
        public int StarvationDeaths { get; set; }
        public int DiseaseDeaths { get; set; }
        public int ArchivesDecrypted { get; set; }
        public int TechNodesCompleted { get; set; }
        public int CaptivesParoled { get; set; }
        public int CaptivesInterrogated { get; set; }
        public int PenalLaborShiftsRun { get; set; }
        public float AverageFreshnessConsumed { get; set; }
        public int FactionDominanceScore { get; set; }
        public Dictionary<string, int> FactionStandings { get; set; } = new Dictionary<string, int>();
        public List<string> HistoricDecisions { get; set; } = new List<string>();
        public ulong CampaignSeed { get; set; }
    }

    public sealed class EpilogueChapter
    {
        public string Category { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string NarrativeText { get; set; } = string.Empty;
        public List<string> Highlights { get; set; } = new List<string>();
    }

    public sealed class EpilogueChronicle
    {
        public string CampaignId { get; set; } = string.Empty;
        public int TotalDays { get; set; }
        public List<EpilogueChapter> Chapters { get; set; } = new List<EpilogueChapter>();
        public CampaignEpilogueSnapshot FinalMetrics { get; set; } = new CampaignEpilogueSnapshot();
    }
}
```

---

## 4. Invariants & Determinism

- Given the same `CampaignEpilogueSnapshot`, `CampaignEpilogueEngine` must generate identical `EpilogueChronicle` output.
- Text selection uses `ISeededRng` keyed to `epilogue_eval` and `CampaignSeed`.
- Zero engine dependencies.
