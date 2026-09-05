# Wasteland Grave Epitaphs — Distribution Plan

**Target Total:** Exactly 30 records (8 existing + 22 new)

---

## 1. Reconciliation of Proposed Additions

The initial conceptual roadmap proposal listed additions per cause that totaled 24 records (which would yield 32 records if added naively to the 8 existing).
In accordance with Task 69G, the numbers have been reconciled against the 22-addition target to prioritize:
1. Uncovered causes (0 baseline entries) -> guaranteed representation.
2. High-frequency causes (radiation, combat, starvation) -> 3 total entries each to prevent repetition.
3. Secondary causes (exhaustion, disease, expedition, trauma, exposure, suicide, old_age) -> 2 total entries each.
4. Rare/situational causes (infection, drowning, frostbite, poisoning, execution, unknown, unspecified) -> 1 total entry each.

---

## 2. Distribution Table

| Cause | Existing Baseline | Additions | Final Total Count | Category / Priority |
|---|---|---|---|---|
| `radiation` | 1 | +2 | 3 | High Frequency / Core Hazard |
| `combat` | 1 | +2 | 3 | High Frequency / Tactical |
| `starvation` | 1 | +2 | 3 | High Frequency / Survival |
| `exhaustion` | 1 | +1 | 2 | Medium Frequency / Shelter Labor |
| `disease` | 1 | +1 | 2 | Medium Frequency / Medical Outbreak |
| `expedition` | 1 | +1 | 2 | Medium Frequency / Surface Exploration |
| `trauma` | 1 | +1 | 2 | Medium Frequency / Structural Hazard |
| `unspecified` | 1 | +0 | 1 | Fallback / Baseline Preserved |
| `exposure` | 0 | +2 | 2 | Uncovered -> Environmental Weather |
| `suicide` | 0 | +2 | 2 | Uncovered -> Grief / Despair |
| `infection` | 0 | +1 | 1 | Uncovered -> Wound Sepsis |
| `old_age` | 0 | +2 | 2 | Uncovered -> Natural Lifespan |
| `drowning` | 0 | +1 | 1 | Uncovered -> River / Ice Failure |
| `frostbite` | 0 | +1 | 1 | Uncovered -> Severe Cold Necrosis |
| `poisoning` | 0 | +1 | 1 | Uncovered -> Toxic Runoff / Chemical |
| `execution` | 0 | +1 | 1 | Uncovered -> Faction Violence |
| `unknown` | 0 | +1 | 1 | Uncovered -> Anonymous / Weathered Marker |
| **TOTALS** | **8** | **+22** | **30** | **Exact Plan 69 Target** |

---

## 3. Mathematical Verification

- Existing records: 8
- Added records: 2 + 2 + 2 + 1 + 1 + 1 + 1 + 0 + 2 + 2 + 1 + 2 + 1 + 1 + 1 + 1 + 1 = 22
- Sum: 8 + 22 = 30
- Unique causes represented: 17 distinct keys (16 requested causes + 1 baseline `unspecified`).
