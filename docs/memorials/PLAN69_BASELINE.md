# Plan 69 — Wasteland Grave Epitaphs Expansion: Baseline Reconnaissance

**Date:** 2026-09-03
**Corpus:** ASHFALL Core & Godot Host
**Authority:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`
**System:** `Assets/Ashfall.Core/Memorial/MemorialSystem.cs`

---

## 1. Executive Summary

Plan 69 expands `wasteland_grave_epitaphs.json` from **8 verified entries to 30** so graves encountered across ASHFALL stop repeating the same small set of memorial lines and instead provide a rich, grounded, cause-aware environmental storytelling surface.

This document establishes the verified pre-change baseline:
- `wasteland_grave_epitaphs.json` had exactly 8 records.
- All 8 records featured an official administrative/clinical death summary followed by an improvised survivor carving.
- Causes covered in baseline: `radiation`, `combat`, `starvation`, `exhaustion`, `disease`, `expedition`, `trauma`, `unspecified`.
- Zero compiler errors, 6623 passed xUnit tests, and 0 data integrity errors in baseline.

---

## 2. Baseline Verification Results

| Command | Exit Code | Result | Evidence |
|---|---|---|---|
| `dotnet test Ashfall.Core.Tests` | 0 | 6623 passed, 0 failed, 0 skipped | Net9.0 xUnit suite ran in 27s |
| `godot --headless --path . -- --data-integrity-selftest` | 0 | 0 errors across 208 catalogs | 10617 authored IDs, 3598 reuses |
| `godot --headless --path . -- --content-utilization-selftest` | 0 | CI Gate: PASS | 490 catalogs scanned, 146 gameplay consumed |
| `godot --headless --path . -- --scene-binding-selftest` | 0 | 22/22 passed | All UI scenes cleanly bound |
| `python3 scripts/ci/scene-lint.py` | 0 | 0 errors, 0 warnings | 27 production scenes checked |
| `dotnet build Ashfall.csproj` | 0 | 0 warnings, 0 errors | Host build clean |

---

## 3. Inventory of the Existing 8 Baseline Entries

| # | Cause | Word Count | Baseline Text |
|---|---|---|---|
| 1 | `radiation` | 28 | "Lethal cellular degradation. Biological remains require deep burial. Below the official line, smaller, cut with a nail: they were not contagious at the end. We held their hand anyway." |
| 2 | `combat` | 20 | "Terminated by hostiles. Equipment recovered and sanitized. Scratched sideways underneath: they did not run. Nobody ran. That is the whole story." |
| 3 | `starvation` | 23 | "Caloric deficit reached terminal state. Carved underneath in a different hand: gave their share away three times. The third time is logged here." |
| 4 | `exhaustion` | 21 | "Cardiovascular collapse due to sustained labor output. Carved deep underneath: rested on the seventh day, finally. We finished their shift for them." |
| 5 | `disease` | 23 | "Pathological contamination event. Sector quarantined. Underneath, in smaller letters: we talked through the door every night. The door logged nothing. We did." |
| 6 | `expedition` | 32 | "Asset failed to return from surface operations. Logged as loss. Carved under it with a knife point: not an asset. A friend. The log is wrong, and we are leaving it that way." |
| 7 | `trauma` | 23 | "Severe structural damage to biological unit. Underneath, almost too rough to read: carried in. Not left alone. That much is ours to say." |
| 8 | `unspecified` | 18 | "Termination logged. Rations redistributed. Underneath, the newest carving on the stone: still owe them a name. Working on it." |

---

## 4. Architectural Findings

1. **System Authority:** `MemorialSystem` (`Assets/Ashfall.Core/Memorial/MemorialSystem.cs`) is the Core authority for memorial records. It accepts `MemorialInput` which carries `SurvivorId`, `Cause`, `Day`, `BirthDay`, `FinalWishResolved`, `Epitaph`, `HeirloomItemId`, `HeirloomRecipientId`, `MoraleDelta`, `DeathQuality`, and `Outcome`.
2. **Persistence Authority:** `MemorialSaveStore` (`src/Host/MemorialSaveStore.cs`) persists `MemorialSave` containing `MemorialState`, with SHA-256 `SaveChecksum` verification. `MemorialEntry` stores `Epitaph` and `Cause`.
3. **Data Authority:** `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` is the top-level catalog providing short grave epitaph lines. It is separate from `Data/narrative/wasteland_grave_epitaphs.json`, which stores longer narrative codex entries.
4. **Pure Data Work:** No runtime changes to `MemorialSystem` are required; all 22 new epitaphs expand the data catalog to reach 30 entries.
