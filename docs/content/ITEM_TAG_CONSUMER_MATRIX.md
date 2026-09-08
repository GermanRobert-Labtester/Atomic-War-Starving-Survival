# Item Tag Consumer Matrix & Utilization

## 1. Overview
`expansion_item_tags.json` defines narrative and functional metadata across 115 in-game items with 33 distinct tag identifiers. This document maps these tags to their system consumers and behavioral impacts.

---

## 2. Tag Registry & Consumer Touchpoints

| Tag Token | Item Count | Primary Consumer | Behavioral / Presentation Effect |
|---|---|---|---|
| `personal_keepsake_candidate` | 51 | `ItemInspectionModel`, `InventoryDetailPanel` | Renders `[Keepsake: Suitable as a personal keepsake]` during item inspection. |
| `pre_war_artifact` | 34 | `ItemInspectionModel`, `TradeLedgerSystem` | Identified as irreplaceable relic; enhanced barter interest with collectors/scholars. |
| `phantom_memory_trigger` | 28 | `PhantomMemoryHostSession` | Holding or scavenging item has a chance to trigger background-specific flashbacks. |
| `restorable_photograph` | 12 | `DarkroomReconstructionSystem` | Can be processed at chemical workbenches to reveal lore vignettes. |
| `heirloom` | 10 | `SurvivorEnrichmentService`, `JournalHostSession` | Distinct survivor emotional attachment marker. |
| `contraband` | 8 | `TribunalLawSystem`, `SecurityEnforcement` | Subject to bunker inspection confiscation under strict civil codes. |
| `medical_grade` | 7 | `MedicalHostSession` | Qualifies for sterile surgical procedures in the clinic. |
| `technical_manual` | 6 | `ResearchWorkshopSystem` | Unlocks specific pre-war engineering schematics upon study. |
| `salvageable_copper` | 6 | `ScrapRecyclingSystem` | High-yield electrical reclamation value. |
| `subterranean_tool` | 5 | `ExcavationMiningEngine` | Usable in shaft tunneling and bedrock stabilization. |

---

## 3. Integration Architecture

### 3.1 Inspection Pipeline
```csharp
// Inside ItemInspectionModel.Create(def, catalog, enrichment)
bool isKeepsake = false;
var tags = new List<string>();

if (enrichment != null && !string.IsNullOrEmpty(def.Id))
{
    var tagData = enrichment.GetItemTags(def.Id);
    if (tagData?.tags != null)
    {
        tags.AddRange(tagData.tags);
        if (tagData.tags.Contains("personal_keepsake_candidate"))
        {
            isKeepsake = true;
        }
    }
}
```

### 3.2 Presentation in Inventory UI
In `InventoryDetailPanel.cs`, items tagged with `personal_keepsake_candidate` display a gold-tinted contextual notice:
`[Keepsake: Suitable as a personal keepsake]`
This informs the player that the item may possess profound emotional value to one of the holdfast's survivors.
