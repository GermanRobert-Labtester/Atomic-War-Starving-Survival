# Plan 145 Graffiti Authority Map

This document establishes the architectural ownership boundaries for bunker graffiti and wall text, preventing authority drift and protecting neighboring systems.

## 1. Authority Matrix

| Concern | Primary Authority | Plan 145 Interaction | Invariant |
|---|---|---|---|
| **Posting Definitions** | `Data/narrative/bunker_graffiti_postings.json` & `Data/narrative/graffiti_expansion.json` | Definition Authority (read via `BunkerGraffitiCatalog`) | Engine-agnostic, immutable data authority |
| **Campaign Day** | `CampaignDaySystem` / `ISimClock` | Read-Only Eligibility Input (`recorded_day <= currentDay`) | No mutation; past days do not retroactively trigger cascades |
| **Player / Shelter Location** | `HoldfastInteriorView` / `WorldHostSession` / `MapLocationMarkerView` | Read-Only Projection Input (resolves which postings belong to the inspected space) | No mutation; wall marks do not relocate |
| **Location Discovery** | `LocationMemorySystem` / `MapAtlasPanel` | Read-Only Reference (postings at undiscovered world sites remain hidden until site is discovered) | Preserves wasteland fog-of-war |
| **Player Memorial Carvings** | `MemorialSystem` / `ShelterDecorPanel` | **Completely Separate System** | Never merged; memorial carvings are player-created or death-derived mutable records |
| **Moral Band Gossip** | `MoralChoiceGossipRuntime` / `moral_choice_gossip.json` | **Completely Separate System** | Never merged; moral gossip dynamically reflects current morality band, whereas graffiti is static environmental text |
| **Survivor Needs & Morale** | `NeedsSystem` / `SurvivorState` | **No Mutation** | Viewing graffiti is read-only presentation; `morale_effect` is strictly descriptive tone |
| **Faction Standing** | `FactionSystem` / `MusterSystem` | **No Mutation** | Reading faction scrawls does not award or deduct faction reputation |
| **Journal Persistence** | `JournalSystem` | Optional downstream discovery record only | No dedicated save store needed for ambient text |
| **Presentation Surfaces** | `ShelterPanel`, `HoldfastInteriorView`, `MapDetailPanel` | View Layer (Renderers) | Presentation-only; no gameplay logic or simulation loops in UI |

## 2. Distinct Systems Boundary (The Four Neighbors)

1. **Graffiti & Wall Text (Plan 145):**
   Pre-authored, static environmental postings loaded by `BunkerGraffitiCatalog`. Anchored to physical walls and historical days.
2. **Player Memorial Wall Carvings (Plan 12C / Plan 68):**
   Dynamic, player-driven remembrance plaques and survivor death records managed by `MemorialSystem` and `ShelterDecorSystem`.
3. **Moral-Choice Gossip (Plan 110):**
   Dynamic, reactive survivor dialogue lines selected based on the player's moral alignment band (Cruel, Pragmatic, Compassionate).
4. **Location Environmental Descriptions (Plan 17 / Plan 29):**
   Broad sensory atmosphere prose and room identity overviews (`former_use`, `one_line_history`). Graffiti complements these by providing the authentic, uncensored human markings left behind on those physical surfaces.
