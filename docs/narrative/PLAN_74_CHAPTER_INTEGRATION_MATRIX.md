# Plan 74 — Chapter Integration Matrix

Documentation of each chapter's implied world change, the system that actually owns it, and hook availability. **This is a documentation artifact, not an instruction to add unsupported fields** — the progression schema supports only `order` + `description` (see runtime contract).

| Ch. | Title | Trigger authority | Narrative meaning | Existing system owner | Hook exists? | Deferred plan |
|---:|---|---|---|---|---|---|
| 1 | The Exchange | none (static) | detonations | world history | no — display only | — |
| 2 | Ashfall | none (static) | fallout survival | radiation/hazards | no — display only | — |
| 3 | The Bunker | none (static) | sheltering | shelter/survivors | no — display only | — |
| 4 | First Contact | none (static) | other survivors | expeditions/NPCs | no — display only | — |
| 5 | The Long Winter | none (static) | winter onset | weather/seasons (`window_deep_freeze`, day 60) | no — display only | 19C |
| 6 | The Consolidation | none (static) | factions institutionalize | faction/territory | no | 44/45 |
| 7 | The Long Dark | none (static) | morale attrition | morale/guilt | no | 66/68 |
| 8 | The Thaw | none (static) | mobility returns | weather/expedition (`window_thaw`, day 120) | no | 32/60/48 |
| 9 | The Schism | none (static) | political fracture | factions/beliefs | no | 30C |
| 10 | The Black Market | none (static) | shadow economy | trade | no | 61 |
| 11 | The Reckoning | none (static) | debts return | debt/guilt | no | 40/66 |
| 12 | The Rebuilding | none (static) | durable infrastructure | power/water/rail | no | 55/71 |
| 13 | The Second Winter | none (static) | renewed deep cold | weather/warlords (`window_high_cold`, day 240) | no | 63/19C |
| 14 | The Muster | none (static) | political gathering | coalition/census | no | — |
| 15 | The Inheritance | none (static) | legacy | epilogue (`CampaignEpilogueEngine`) | no | 15A |

## Incident links (Plan 57)

**0 of 5 authored.** The runtime has no transition event to attach incidents to, and the progression schema has no `incident_id` field. The five preferred target chapters (Long Dark, Schism, Reckoning, Second Winter, plus Consolidation/First Winter slot) are recorded above as deferred Plan 57 integration points. No incident IDs were placed in production JSON.

## Territory links (Plan 44)

**0 of 3 authored.** No territory hook or field exists. Consolidation, Schism, and Reckoning remain narrative framing only; no territory state enters progression data.

## Season links (Plan 19C)

**3 of 3 aligned thematically** (Long Winter → Deep Freeze; Thaw → `window_thaw`; Second Winter → High Cold). Since chapters cannot be triggered by season state, alignment is enforced at the authoring level via `PLAN_74_CHAPTER_PACING_MATRIX.md`. Season authority (`WeatherSystem.GetSeasonForDay`) is untouched and remains the sole weather authority.
