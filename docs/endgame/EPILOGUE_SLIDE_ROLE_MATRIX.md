# Epilogue Slide Role Matrix (20 Distinct Campaign Roles)

**Document ID:** `docs/endgame/EPILOGUE_SLIDE_ROLE_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/epilogue_chronicle.json`

---

## 1. Twenty-Slide Role Specification

| Order | Title | Words | Category | Campaign Pillar | Narrative Presentation Function |
|---:|---|---:|---|---|---|
| 0 | `Opening` | 1 | Opening | Prologue | The calm before: pre-war warnings, sirens sounding across the valley. |
| 1 | `After the Flash` | 3 | Opening | Catastrophe | The atomic exchange: blinding light, shockwave, ruptured communication lines. |
| 2 | `The Bunker` | 2 | Opening | Shelter Redoubt | Arrival: hydraulic blast doors slamming shut, steel locks engaging. |
| 3 | `First Winter` | 2 | Opening | Nuclear Winter | Early survival: ash clouds blocking the sun, plummeting temperatures. |
| 4 | `Water and Heat` | 3 | Mid | Infrastructure | The lifeline: boiler pressure, hydrothermal conduits, desalination filters. |
| 5 | `Survivors` | 1 | Mid | Demographics | The living: dwellers who endured hunger, sickness, and hard shifts. |
| 6 | `Empty Bunks` | 2 | Mid | Memorial | The cost: dweller deaths, cold bunks, engraved memorial walls. |
| 7 | `The Factions` | 2 | Mid | Regional Politics | Outer wasteland powers: Fleet, Garrison, Cutters, Rebuilders, Foundry. |
| 8 | `Lines on the Map` | 4 | Mid | Exploration | Expeditions: supply routes, frozen highways, scavenger tracks. |
| 9 | `Voices in Static` | 3 | Mid | Communications | Radio network: distress signals, emergency broadcasts, encrypted relays. |
| 10 | `The Verdict` | 2 | Mid | Forensics | Machine records: tribunal evidence, missile telemetry, classified logs. |
| 11 | `The Witnesses` | 2 | Mid | Testimony | Oral histories: depositions from survivors, soldiers, and deserters. |
| 12 | `Restored Relics` | 2 | Mid | Archives | Cultural preservation: salvaged blueprints, microfilms, seed libraries. |
| 13 | `What We Chose` | 3 | Mid | Moral Dilemmas | Ethical legacy: mercy vs. iron discipline, amnesty vs. execution. |
| 14 | `The Muster` | 2 | Late | Coalition | Gathering: regional assembly, diplomatic treaties, common councils. |
| 15 | `The Resolution` | 2 | Late | Plan 89 Outcome | The earned ending: authoritative resolution from `muster_epilogues.json`. |
| 16 | `The Census` | 2 | Late | Demographics | Final ledger: headcount, days survived, demographic balance sheet. |
| 17 | `What Remains` | 2 | Late | Environmental Scars | Physical enduring legacy: scarred concrete, rusted towers, quiet craters. |
| 18 | `After Us` | 2 | Late | Generational Horizon | The tomorrow: children raised underground, eventual return to the surface. |
| 19 | `Final Word` | 2 | Late | Epilogue Close | Closing reflection: solemn final statement on the Year of Ash. |

---

## 2. Semantic Collision Prevention Audit

The 20 roles explicitly eliminate the 4 potential collisions identified in Section 8:
- **`The Shelter` vs `The Bunker`**: `The Shelter` was discarded; `The Bunker` (Order 2) solely represents the physical bunker redoubt.
- **`The Ending` vs `Final Word`**: `The Ending` was eliminated in favor of `The Resolution` (Order 15, Plan 89 outcome) and `Final Word` (Order 19, close).
- **`The Last Word` vs `Final Word`**: `The Last Word` was rejected to avoid any duplication of `Final Word` (Order 19).
- **`The Future` vs `What Remains`**: `What Remains` (Order 17) represents ruins/past scars, while `After Us` (Order 18) represents future generations.
