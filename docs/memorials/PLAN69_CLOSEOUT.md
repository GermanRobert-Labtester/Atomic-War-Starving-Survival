# Plan 69 — Wasteland Grave Epitaphs Expansion: Closeout Report

## Status: **COMPLETE**

---

## 1. Executive Summary

Plan 69 expanded `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json` from **8 baseline entries to exactly 30 entries** (8 existing + 22 new), transforming grave markers into a broad, cause-aware environmental storytelling surface.

Each of the 8 baseline records was preserved intact. All 22 new additions were authored to strict quality, length, grammar, and tone standards:
- Exactly 1 sentence per addition ending in a single period (zero semicolons).
- Length constrained to 14–17 words (within the 5–20 word target).
- Coverage expanded across 17 distinct cause classifications (16 requested causes + 1 baseline `unspecified`).
- Safety standards strictly enforced: suicide memorials are non-graphic, non-instructional, and non-romanticizing; execution and violence are non-gratuitous; poisoning references contain no procedural chemical instructions.
- Full deterministic selection verified via seeded tests.
- Re-enabled `WastelandGraveEpitaphsCatalogTests.cs` in `Ashfall.Core.Tests.csproj` (all 11 tests passing).

---

## 2. Quantitative Verification

```text
Baseline Epitaphs:              8
Target Total Epitaphs:         30
New Authored Epitaphs:         22
Total Verified Records:        30 (100% target achieved)
Causes Covered:                17 distinct keys
Duplicate Epitaphs:             0 (100% unique strings)
New Entries Sentence Count:    22/22 exactly 1 sentence
New Entries Word Count Range:  14 to 17 words (Target: 5–20)
Catalog Tests Passing:         11/11 in WastelandGraveEpitaphsCatalogTests
Full Core Test Suite:          9,395 passed, 0 failed, 0 skipped
Data Integrity Findings:        0 errors across 298 catalogs
```

---

## 3. Authoritative 30-Epitaph Inventory

| # | Cause | Category | Word Count | Epitaph Text |
|---|---|---|---|---|
| 1 | `radiation` | Baseline | 28 | "Lethal cellular degradation. Biological remains require deep burial. Below the official line, smaller, cut with a nail: they were not contagious at the end. We held their hand anyway." |
| 2 | `combat` | Baseline | 20 | "Terminated by hostiles. Equipment recovered and sanitized. Scratched sideways underneath: they did not run. Nobody ran. That is the whole story." |
| 3 | `starvation` | Baseline | 23 | "Caloric deficit reached terminal state. Carved underneath in a different hand: gave their share away three times. The third time is logged here." |
| 4 | `exhaustion` | Baseline | 21 | "Cardiovascular collapse due to sustained labor output. Carved deep underneath: rested on the seventh day, finally. We finished their shift for them." |
| 5 | `disease` | Baseline | 23 | "Pathological contamination event. Sector quarantined. Underneath, in smaller letters: we talked through the door every night. The door logged nothing. We did." |
| 6 | `expedition` | Baseline | 32 | "Asset failed to return from surface operations. Logged as loss. Carved under it with a knife point: not an asset. A friend. The log is wrong, and we are leaving it that way." |
| 7 | `trauma` | Baseline | 23 | "Severe structural damage to biological unit. Underneath, almost too rough to read: carried in. Not left alone. That much is ours to say." |
| 8 | `unspecified` | Baseline | 18 | "Termination logged. Rations redistributed. Underneath, the newest carving on the stone: still owe them a name. Working on it." |
| 9 | `radiation` | New | 14 | "The dosimeter film on her lapel turned jet black before the relief shift arrived." |
| 10 | `radiation` | New | 15 | "He said the air tasted of copper two hours before the fever took his legs." |
| 11 | `combat` | New | 16 | "Three people made it through the culvert gate because she stayed behind to draw the fire." |
| 12 | `combat` | New | 15 | "He dropped behind the fuel tank and never had the chance to unshoulder his rifle." |
| 13 | `starvation` | New | 15 | "Her name was still on the distribution clipboard when the last sack of meal spoiled." |
| 14 | `starvation` | New | 15 | "He weighed less than thirty kilos when they carried him up from the boiler trench." |
| 15 | `exhaustion` | New | 16 | "She laid her wrench on the pump casing and leaned her forehead against the cool pipe." |
| 16 | `disease` | New | 15 | "His name was crossed off the ward roster before the second dose of penicillin arrived." |
| 17 | `expedition` | New | 16 | "They found his pack three miles past the radio mast with the compass dial smashed inward." |
| 18 | `trauma` | New | 16 | "The winch cable snapped during the generator hoist and left no time for anyone to yell." |
| 19 | `exposure` | New | 17 | "The wind tore her tarp away while she was searching for the road marker in the dark." |
| 20 | `exposure` | New | 15 | "Found kneeling inside the hollowed boiler with his damp wool coat stiffened by the rime." |
| 21 | `suicide` | New | 17 | "We carved her name where she used to sit because no one found words for the rest." |
| 22 | `suicide` | New | 16 | "An empty bunk in Section Four that nobody in the squad has the heart to reassign." |
| 23 | `infection` | New | 15 | "We boiled the needle three times, but the red line crept past his elbow anyway." |
| 24 | `old_age` | New | 17 | "He remembered what grass looked like under clear sunlight and told the children until his voice failed." |
| 25 | `old_age` | New | 16 | "Eighty-two winters counted on the beam, which is sixty more than anyone had reason to expect." |
| 26 | `drowning` | New | 16 | "The pontoon rope snapped in the spring current before anyone on the bank could throw another." |
| 27 | `frostbite` | New | 15 | "Blackened boots left beside the fire shovel because the numbness had already reached his knees." |
| 28 | `poisoning` | New | 16 | "Drank from the condensate drip behind the transformer vault before the test strip turned bright purple." |
| 29 | `execution` | New | 17 | "They brought him out to the gravel pit at sunrise and read no charges from the ledger." |
| 30 | `unknown` | New | 16 | "No tags, no journal, and only three brass buttons left in the gravel beneath the cairn." |

---

## 4. Documentation Suite Delivered

1. [`docs/memorials/PLAN69_BASELINE.md`](PLAN69_BASELINE.md) — Pre-change baseline verification and inventory.
2. [`docs/memorials/WASTELAND_EPITAPH_SCHEMA.md`](WASTELAND_EPITAPH_SCHEMA.md) — Exact JSON shape and C# DTO specification.
3. [`docs/memorials/WASTELAND_EPITAPH_EXISTING_8_AUDIT.md`](WASTELAND_EPITAPH_EXISTING_8_AUDIT.md) — Analysis of baseline entries and preservation rationale.
4. [`docs/memorials/WASTELAND_EPITAPH_CAUSE_MATRIX.md`](WASTELAND_EPITAPH_CAUSE_MATRIX.md) — 17-cause mapping against `SurvivorFateSystem` and `MemorialSystem`.
5. [`docs/memorials/WASTELAND_EPITAPH_DISTRIBUTION.md`](WASTELAND_EPITAPH_DISTRIBUTION.md) — Mathematical distribution plan reconciling requested vs. target counts.
6. [`docs/memorials/WASTELAND_EPITAPH_TONE_GUIDE.md`](WASTELAND_EPITAPH_TONE_GUIDE.md) — Restrained, survivor-carved aesthetic standards.
7. [`docs/memorials/WASTELAND_EPITAPH_MOTIF_AUDIT.md`](WASTELAND_EPITAPH_MOTIF_AUDIT.md) — Motif diversity analysis across 12 concrete physical categories.
8. [`docs/memorials/WASTELAND_EPITAPH_LENGTH_AUDIT.md`](WASTELAND_EPITAPH_LENGTH_AUDIT.md) — Automated word-count and sentence-boundary audit.
9. [`docs/memorials/WASTELAND_EPITAPH_SELECTION_CONTRACT.md`](WASTELAND_EPITAPH_SELECTION_CONTRACT.md) — Seeded PRNG selection and fallback specification.
10. [`docs/memorials/WASTELAND_EPITAPH_SAVE_CONTRACT.md`](WASTELAND_EPITAPH_SAVE_CONTRACT.md) — Persistence in `MemorialSaveStore` and `MemorialState`.
11. [`docs/memorials/WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md`](WASTELAND_EPITAPH_MICRO_LOCATION_HANDOFF.md) — Integration with Plan 49 improvised grave discoveries.
12. [`docs/memorials/WASTELAND_EPITAPH_MOURNING_HANDOFF.md`](WASTELAND_EPITAPH_MOURNING_HANDOFF.md) — Separation of text layer from mourning rites and grief cascades.
13. [`docs/memorials/WASTELAND_EPITAPH_FINAL_WISH_HANDOFF.md`](WASTELAND_EPITAPH_FINAL_WISH_HANDOFF.md) — Separation of general epitaphs from Plan 65 survivor wishes.
14. [`docs/memorials/WASTELAND_EPITAPH_CONTENT_UTILIZATION.md`](WASTELAND_EPITAPH_CONTENT_UTILIZATION.md) — Scanner mapping and live consumption validation.
15. [`docs/memorials/WASTELAND_EPITAPH_REGRESSION_MATRIX.md`](WASTELAND_EPITAPH_REGRESSION_MATRIX.md) — Contract verification trace.
16. [`docs/memorials/PLAN69_CLOSEOUT.md`](PLAN69_CLOSEOUT.md) — This closeout document.

---

## 5. Verification Matrix Evidence

| Verification Gate | Command | Result | Evidence |
|---|---|---|---|
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | **PASS (0)** | 30 production scenes checked; 0 errors; 0 warnings |
| **Catalog Integrity** | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0)** | 0 errors across 298 catalogs |
| **Scene Binding** | `godot --headless --path . -- --scene-binding-selftest` | **PASS (0)** | 25/25 passed |
| **Content Utilization** | `godot --headless --path . -- --content-utilization-selftest` | **PASS (0)** | CI Content Utilization Gate: PASS |
| **Epitaph Tests** | `dotnet test --filter "FullyQualifiedName~WastelandGraveEpitaphsCatalogTests"` | **PASS (0)** | 11 passed, 0 failed, 0 skipped (66 ms) |
| **Host Build** | `dotnet build Ashfall.csproj` | **PASS (0)** | 0 errors |
| **Full Regression Suite** | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS (0)** | **9,395 passed**, 0 failed, 0 skipped |
