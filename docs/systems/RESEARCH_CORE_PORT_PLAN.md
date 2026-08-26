# ASHFALL — Research Core Port Plan & Completion Report

**Status:** **CLOSED — SHIPPED & VERIFIED AT PHASE 28**
**Core Domain:** `Assets/Ashfall.Core/Research/`
**Host & UI:** `src/Host/ResearchHostSession.cs`, `src/UI/ResearchAtlasPanel.cs`
**Verification:** `Ashfall.Core.Tests/ResearchSystemTests.cs` (8/8 PASS), Snapshot `research_atlas_default` (PASS)

---

## 1. Architectural Overview & Context

Historically, `SURFACE_GAP_REPORT.md` flagged `ResearchPanel` as `MISSING (awaiting Core)`. While `KnowledgeBase.cs` existed for 14 discovery lore keys, the simulation lacked an engine-agnostic R&D, breakthrough, and prerequisite-progression system.

The Research Core Port was executed in Phase 28, adhering strictly to:
- **Invariant 1**: Zero engine dependencies in `Assets/Ashfall.Core/` (`noEngineReferences: true`).
- **Invariant 3**: Cross-host / serializer-independent save/load contracts via serializable DTOs.
- **Invariant 4**: Deterministic state transitions.
- **Invariant 5**: Engine logic in Core, presentation in Godot host.

---

## 2. Shipped Implementation Summary

### A. Core Domain Layer (`Assets/Ashfall.Core/Research/`)
1. **[`ResearchKnowledgeDef.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Research/ResearchKnowledgeDef.cs)**:
   - Plain C# definition POCO describing knowledge nodes (`id`, `displayName`, `category`, `description`, `prerequisites`, `breakthroughItem`, `daysToComplete`).
2. **[`ResearchState.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Research/ResearchState.cs)**:
   - Serializable state envelope capturing `currentDay`, `unlockedIds`, `activeResearchId`, `activeResearchDays`, and `completedIds`.
3. **[`ResearchSystem.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Assets/Ashfall.Core/Research/ResearchSystem.cs)**:
   - Full simulation engine providing catalog management, prerequisite validation, active research queuing, daily tick progression, breakthrough item awards, and `CaptureState`/`RestoreState`.
   - Ships 15 default canonical knowledge nodes across survival, medical, engineering, science, scavenging, and combat disciplines.

### B. Godot Host Adapter (`src/Host/`)
- **[`ResearchHostSession.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/Host/ResearchHostSession.cs)**:
  - Wires `ResearchSystem` lifecycle to host day advancement, provides save/load hooks (`CaptureSave`/`RestoreSave`), and exposes state accessors to UI.

### C. Presentation Layer (`src/UI/`)
- **[`ResearchAtlasPanel.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/src/UI/ResearchAtlasPanel.cs)**:
  - Tier-3 HYBRID dashboard with a 6-card status rail (Total Nodes, Unlocked, Active Project, Completed, Days Remaining, Breakthroughs Awarded), 3 DataGrid tiles (Knowledge Nodes, Active Project, Breakthrough Items), and a discipline-themed right inspector panel.
  - Snapshot golden registered and verified as `research_atlas_default`.

### D. Automated Test Coverage (`Ashfall.Core.Tests/`)
- **[`Ashfall.Core.Tests/ResearchSystemTests.cs`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/Ashfall.Core.Tests/ResearchSystemTests.cs)**:
  - 8/8 comprehensive unit tests verifying:
    1. Catalog registration and default node initialization (15 nodes).
    2. Starting active research.
    3. Daily tick progression.
    4. Completion and breakthrough item awards.
    5. Prerequisite gating enforcement.
    6. Double-completion rejection.
    7. Full save state capture/restore round-trips.
    8. Deterministic execution across runs.

---

## 3. Canonical 15-Node Research Catalog

| Knowledge ID | Display Name | Discipline | Days | Breakthrough Award | Prerequisites |
|---|---|---|---|---|---|
| `knowledge_water_basics` | Water Purification Basics | survival | 5 | `item_filter_charcoal` | None |
| `knowledge_water_advanced` | Advanced Water Filtration | survival | 12 | `item_filter_ceramic` | `knowledge_water_basics` |
| `knowledge_radiation_basics` | Radiation Medicine Basics | medical | 5 | `item_rad_scrub_gel` | None |
| `knowledge_radiation_shielding`| Radiation Shielding Materials | engineering | 15 | `item_lead_plate_composite`| `knowledge_radiation_basics` |
| `knowledge_gas_mask_improved` | Improved Gas Masks | engineering | 10 | `item_filter_sealed_p100` | None |
| `knowledge_hydroponics` | Hydroponic Cultivation | survival | 8 | `item_nutrient_salts` | `knowledge_water_basics` |
| `knowledge_solar_basics` | Solar Power Basics | engineering | 7 | `item_pv_panel_scrap` | None |
| `knowledge_solar_advanced` | Solar Power Systems | engineering | 14 | `item_mppt_charge_controller`| `knowledge_solar_basics` |
| `knowledge_food_preservation` | Food Preservation | survival | 10 | `item_salt_curing_pack` | None |
| `knowledge_radio_basics` | Radio Signal Processing | science | 6 | `item_vacuum_tube_rf` | None |
| `knowledge_radio_advanced` | Encrypted Radio Communication | science | 12 | `item_crypto_keycard` | `knowledge_radio_basics` |
| `knowledge_shelter_insulation` | Shelter Insulation | engineering | 8 | `item_aerogel_blanket` | None |
| `knowledge_air_filtration` | Air Filtration Systems | engineering | 10 | `item_hepa_drum` | `knowledge_gas_mask_improved` |
| `knowledge_scavenge_efficiency`| Scavenge Efficiency | scavenging | 7 | `item_prybar_titanium` | None |
| `knowledge_combat_training` | Combat Training Doctrine | combat | 8 | `item_tactical_sling` | None |

---

## 4. Potential Future Enhancements (Non-Blocking)

The Core port and UI dashboard are complete and shippable. The following optional items are recorded for post-release content expansion:

1. **External JSON Catalog Sidecar**:
   - Optional future migration of the 15 inline definitions to `Assets/StreamingAssets/Data/research_knowledge.json` if non-programmer content modding of tech trees is requested.
2. **Interactive UI Queue Actions**:
   - Expanding `ResearchAtlasPanel.cs` to allow interactive click-to-start / cancel research directly from the inspection card during live play.
3. **Survivor Assignment Multiplier**:
   - Linking researcher survivor skills (`skill_science`, `skill_engineering`) to accelerate daily `ResearchSystem.Tick` progress rates.
