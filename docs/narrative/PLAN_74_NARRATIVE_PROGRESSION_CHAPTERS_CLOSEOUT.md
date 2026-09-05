# Plan 74 — Narrative Progression Chapters Closeout

_ASHFALL · docs/narrative · Plan 74 — 2026-09-03_

---

## Objective

Expand `narrative_progression.json` from **5 → 15 campaign chapters** using only the fields the live runtime supports (`description`, `order`). No new Core logic, save schema, incident system, or C# class changes.

---

## Deliverables

| File | Status |
|---|---|
| `Assets/StreamingAssets/Data/narrative_progression.json` | ✅ 5 → 15 entries |
| `builds/linux/Assets/StreamingAssets/Data/narrative_progression.json` | ✅ synced |
| `docs/narrative/NARRATIVE_PROGRESSION_RUNTIME_CONTRACT.md` | ✅ |
| `docs/narrative/PLAN_74_CHAPTER_COVERAGE_MATRIX.md` | ✅ |
| `docs/narrative/PLAN_74_CHAPTER_PACING_MATRIX.md` | ✅ |
| `docs/narrative/PLAN_74_CHAPTER_INTEGRATION_MATRIX.md` | ✅ |
| `docs/narrative/PLAN_74_NARRATIVE_PROGRESSION_CHAPTERS_CLOSEOUT.md` | ✅ (this file) |

---

## Schema Evidence

```csharp
// src/Host/EventsHostSession.cs — NarrativeEntryData
public class NarrativeEntryData
{
    public string Description { get; set; } = string.Empty;
    public int Order { get; set; }
}
```

Only `description` and `order` exist in the live DTO. No `trigger_day`, `phase`, `world_state_changes`, or `winter_window` were invented.

---

## Existing Chapters (preserved verbatim)

| order | Title | Status token |
|---|---|---|
| 1 | The Exchange | Complete |
| 2 | Ashfall | Complete |
| 3 | The Bunker | Active |
| 4 | First Contact | Pending |
| 5 | The Long Winter | Pending |

---

## New Chapters (Plan 74)

| order | Title | Arc Phase |
|---|---|---|
| 6 | The Consolidation | Mid |
| 7 | The First Winter | Mid |
| 8 | The Long Dark | Mid-crisis |
| 9 | The Thaw | Mid-recovery |
| 10 | The Schism | Mid-late |
| 11 | The Black Market | Late |
| 12 | The Reckoning | Late |
| 13 | The Rebuilding | Late |
| 14 | The Second Winter | Late-crisis |
| 15 | The Inheritance | Endgame |

---

## Verification Results

| Gate | Command | Result |
|---|---|---|
| Unit tests | `dotnet test Ashfall.Core.Tests` | ✅ 6,792 passed, 0 failed |
| Data integrity | `godot --headless -- --data-integrity-selftest` | ✅ 0 errors, 208 catalogs |
| Content utilization | `godot --headless -- --content-utilization-selftest` | ✅ CI gate PASS |
| Scene binding | `godot --headless -- --scene-binding-selftest` | ✅ 22/22 passed |
| Scene lint | `python3 scripts/ci/scene-lint.py` | ✅ 27 scenes, 0 errors |

---

## Constraints Honoured

- ✅ No `trigger_day`, `phase`, or `world_state_changes` fields added
- ✅ No new C# class or Core system added
- ✅ No new save section or incident scheduler created
- ✅ No weather/season authority duplicated in JSON
- ✅ Chapters 1–5 preserved verbatim
- ✅ `schema_version: 1` retained

---

_Plan 74 COMPLETE — 2026-09-03_
