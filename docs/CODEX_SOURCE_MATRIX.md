# CODEX KNOWLEDGE SOURCE MATRIX
## Multi-Source Knowledge Aggregation, Deduplication, and Provenance Union

**Document Version:** 1.0.0
**Scope:** Plan 74 (Codex & Field-Guide Knowledge Surface)
**Status:** Canonical Knowledge Surface Matrix

---

## 1. Architectural Directive

> **The Codex is a pure functional projection. It possesses zero persistent save state.**

Every fact in the Codex is projected on demand from authoritative upstream systems:
- `FieldGuideCatalog` (flora, fauna, ecology observations)
- `ResearchState` / `ResearchSystem` (completed & in-progress technologies)
- `JournalSystem.Knowledge` (historical discoveries, evidence, logs, lore)
- `AutopsySystem` / `Manual` catalogs (biological & technical analysis)

No `codex_save.json` shall ever exist. A save file represents mutations of authoritative state; the Codex is a lens over those states.

---

## 2. Category Classification

| Category Ordinal | Category Name | Description | Authoritative Producers |
|---|---|---|---|
| `0` | `Ecology` | Wasteland flora, fauna, mutations, harvesting | `FieldGuideCatalog`, `WildlifeSeasonalCalendar` |
| `1` | `Technology` | Pre-war blueprints, engineering, power, water | `ResearchSystem`, `WorkshopEngine` |
| `2` | `WastelandLore` | World history, pre-war archives, survivor logs | `JournalSystem`, `NarrativeEngine` |
| `3` | `SurvivalOperations`| Medical triage, decontamination, radon mitigation | `AutopsySystem`, `MedicalDiagnosisSystem` |
| `4` | `Factions` | War doctrines, treaty history, rally intel | `FactionBranchCoordinator`, `MusterWarfareEngine` |

---

## 3. Projection State Model

Each projected entry has one of three states:
1. **`Known` (State = 2):** Fully discovered or completed. Title, metadata, and full body text are displayed.
2. **`Studying` (State = 1):** In-progress research or active inquiry. Title and category are displayed; detailed body text is masked / redacted ("Under active analysis...").
3. **`Locked` (State = 0):** Unlocked in tree or rumored, but not yet analyzed. Displayed with redacted placeholders or hidden depending on filter.

---

## 4. Deduplication & Provenance Union Rules

When multiple upstream producers submit facts sharing the same stable `FactId`:
1. **Single Entry Projection:** The fact appears exactly once in the Codex.
2. **Provenance Union:** All distinct `CampaignProvenanceRecord` instances are preserved in the entry's `Provenance` list.
3. **Earliest Discovery Date:** `DayLearned` becomes `min(record.DayObserved)`.
4. **Highest Confidence:** The primary confidence rating becomes `max(record.Confidence)`.
5. **Deterministic Sort Order:**
   - Primary: `Category` ordinal
   - Secondary: `State` descending (`Known` -> `Studying` -> `Locked`)
   - Tertiary: `Title` (StringComparer.Ordinal)
   - Quaternary: `EntryId` (StringComparer.Ordinal)
