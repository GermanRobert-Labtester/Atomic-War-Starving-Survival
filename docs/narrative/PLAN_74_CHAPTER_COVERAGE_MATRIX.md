# Plan 74 — Chapter Coverage Matrix

_ASHFALL · docs/narrative · Plan 74_

## Summary

| # | Title | Baseline? | Arc Phase | Primary Themes |
|---|---|---|---|---|
| 1 | The Exchange | ✅ preserved | Early | Nuclear detonations; world-end moment |
| 2 | Ashfall | ✅ preserved | Early | Fallout survival; radiation; the first days |
| 3 | The Bunker | ✅ preserved | Early | Shelter establishment; community genesis |
| 4 | First Contact | ✅ preserved | Mid-early | External survivors; first relationships |
| 5 | The Long Winter | ✅ preserved | Mid | Nuclear winter onset |
| 6 | The Consolidation | 🆕 new | Mid | Shelter institutionalisation; leadership fractures |
| 7 | The First Winter | 🆕 new | Mid | Cold + scarcity; trade choices; exclusion |
| 8 | The Long Dark | 🆕 new | Mid-crisis | Mid-winter morale collapse; identity crisis |
| 9 | The Thaw | 🆕 new | Mid-recovery | Expeditions resume; factions engage; geography |
| 10 | The Schism | 🆕 new | Mid-late | Internal fracture resolved by action or exile |
| 11 | The Black Market | 🆕 new | Late | Trade networks; isolation vs. entanglement |
| 12 | The Reckoning | 🆕 new | Late | Cause of the exchange revealed; moral weight |
| 13 | The Rebuilding | 🆕 new | Late | Infrastructure + alliances; legacy planting |
| 14 | The Second Winter | 🆕 new | Late-crisis | Harder winter; stress-tests earlier decisions |
| 15 | The Inheritance | 🆕 new | Endgame | Legacy; succession; shelter's future without founders |

---

## Existing Chapter Audit (verbatim preservation check)

| order | Original description | Preserved |
|---|---|---|
| 1 | `Chapter 1 Complete: The Exchange — Nuclear detonations across the globe` | ✅ |
| 2 | `Chapter 2 Complete: Ashfall — Surviving the initial fallout and radiation` | ✅ |
| 3 | `Chapter 3 Active: The Bunker — Establishing shelter and community` | ✅ |
| 4 | `Chapter 4 Pending: First Contact — Encountering other survivors` | ✅ |
| 5 | `Chapter 5 Pending: The Long Winter — Nuclear winter conditions setting in` | ✅ |

> Chapter 3 carries the `Active:` status token from the baseline. This is a display convention; the runtime does not parse it. Not modified under Plan 74.

---

## Coverage Gaps (cross-plan handoffs)

Chapters 6–15 have no status tokens — status advancement belongs to the campaign state machinery. The following systems provide the runtime triggers that correspond to these chapters:

| Chapter | Runtime trigger authority | Handoff plan |
|---|---|---|
| 6 | `NarrativeEncounterSystem` / incident gating | Future plan |
| 7–8 | `WeatherSystem` winter window | Future plan |
| 9 | `WeatherSystem` thaw + expedition unlock | Future plan |
| 10 | Flag system (schism event) | Future plan |
| 11 | Faction catalog + trade unlock | Future plan |
| 12 | Verdict / Reckoning data | Plan 82 seam |
| 13 | Shelter upgrade gates | Future plan |
| 14 | `WeatherSystem` second winter | Future plan |
| 15 | Ending system | Future plan |

---

_Last updated: Plan 74 — 2026-09-03_
