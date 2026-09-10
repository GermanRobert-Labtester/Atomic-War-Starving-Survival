# Bunker Court Identity, Provenance & Character Attribution

**Document ID:** ARCH-BUNKER-COURT-IDENTITY-PROVENANCE
**Status:** Approved Architectural Specification
**Project:** ASHFALL (Godot 4.7+ .NET 8 Host / C# Core)
**Date:** 2026-09-09

---

## 1. Dual Identity Architecture

Every case entry in `bunker_court_verdicts_codex.json` possesses two distinct identifiers serving distinct purposes:

### 1.1 Machine Content Identifier: `case_id`
- **Format:** `case_{nn}_{snake_case_descriptor}` (e.g. `case_01_the_air_duct_moonshine_still`).
- **Role:** Primary immutable key for software queries, dictionary lookups, test assertions, and discovery manifest references (`source_record_id`).
- **Invariant:** Case-insensitive uniqueness across the entire collection. Never renamed without database/manifest migration.

### 1.2 Human Display Identifier: `docket_number`
- **Format:** `TRIB-{day}-{KEYWORD}` (e.g. `TRIB-084-MOONSHINE`, `TRIB-3650-CONSTITUTION`).
- **Role:** Diegetic legal docket number displayed to the player in UI headers, search filters, and clerk notes.
- **Embedded Semantics:** Encodes the exact historical in-world day on which the tribunal convened (`TRIB-{historical_day}-{slug}`).
- **Invariant:** Unique across all 24 cases. Case-insensitive uniqueness enforced by tests.

---

## 2. Character Attribution & Entity Decoupling

The tribunal records reference many prominent historical residents of the bunker:

| Character Name | Roles Referenced in Dockets | Appearance in Current Campaign | Boundary Rule |
|---|---|---|---|
| **Dr. Irina Vel** | Chief Medical Officer, Presiding Magistrate | Historical founder / legacy figure | Read-only lore; does not mutate live doctor survivor entity. |
| **Chief Engineer Dmitri** | Infrastructure Lead, Presiding Magistrate | Historical founder / legacy engineer | Read-only lore; does not mutate live engineer survivor entity. |
| **Sister Mara** | Spiritual Guide, Tribunal Arbitrator | Historical elder / moral anchor | Read-only lore; does not mutate live survivor entity. |
| **Cook Oxana** | Head of Communal Canteen, Magistrate | Historical dweller | Read-only lore; does not alter canteen recipes or rations. |
| **Quartermaster Harlan** | Supplies & Scavenging Lead, Magistrate | Historical dweller | Read-only lore; does not alter quartermaster stocks. |
| **Master Oleg** | Master Machinist, Magistrate | Historical craftsman | Read-only lore; does not alter machine shop tools. |
| **Ilya Morozov** | Instrument Technician, Multiple Offender | Historical defendant | Read-only lore; historical character, not an active survivor. |
| **Valery** | Machine Shop Apprentice, Multiple Offender | Historical apprentice | Read-only lore; does not alter apprentice training queues. |
| **Stoker Nadia** | Boiler Tender, Mushroom Cultivator | Historical stoker | Read-only lore; does not alter boiler staffing. |
| **Little Sonya Vel** | Child botanist (Case 04, 16) -> Dr. Sonya Vel (Case 24) | Historical character showing generational passage | Read-only lore; generational storytelling. |

### Architectural Directive on Character Entities:
Under no circumstances shall `BunkerCourtCatalog` instantiate, modify, injure, kill, or promote survivor entities in `SurvivorsHostSession` or `NeedsSystem`. These named figures are historical actors whose deeds and foibles took place in the shelter's chronicle.

---

## 3. Immutability & Zero Runtime Persistence Footprint

- **Read-Only Data Authority:** `Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json` is shipped as immutable data. The runtime never writes back to this JSON file.
- **Zero Schema State:** `BunkerCourtCatalog` does not define a `SystemState` DTO or require a custom `SaveStore`.
- **Volatile Cache:** `BunkerCourtCatalog` loads on demand as a pure in-memory query service.
