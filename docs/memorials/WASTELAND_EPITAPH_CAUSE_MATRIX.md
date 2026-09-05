# Wasteland Grave Epitaphs — Cause Vocabulary Matrix

**Reference Systems:**
- `Ashfall.Core.Survivors.SurvivorFateSystem` (`SurvivorDeathCause`)
- `Ashfall.Core.Memorial.MemorialSystem`
- `Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json`

---

## 1. Cause Mapping Table

The table below maps the 16 requested causes to the live system death models and memorial catalog causes:

| Requested Cause | Live System Match | Canonical Catalog Key | Source / Origin in ASHFALL | Status |
|---|---|---|---|---|
| `radiation` | `SurvivorDeathCause.Radiation` | `radiation` | Acute radiation sickness, dosimeter saturation, fallout zone | Live & Supported |
| `combat` | `SurvivorDeathCause.Combat` | `combat` | Tactical combat casualty, breach defense, ballistic wounds | Live & Supported |
| `starvation` | `SurvivorDeathCause.Needs` (detail: hunger) | `starvation` | Caloric depletion, crop failure, ration expiration | Live & Supported |
| `exhaustion` | `SurvivorDeathCause.Needs` (detail: stamina/work) | `exhaustion` | Labor collapse, shift overwork, cardiovascular fatigue | Live & Supported |
| `disease` | `SurvivorDeathCause.Disease` | `disease` | Pathogen outbreak, quarantine ward death, epidemic | Live & Supported |
| `expedition` | `SurvivorDeathCause.Expedition` | `expedition` | Lost on surface sortie, vehicle breakdown, route isolation | Live & Supported |
| `trauma` | `SurvivorDeathCause.Medical` / Structural | `trauma` | Heavy machinery crush, cave-in, blast concussion | Live & Supported |
| `exposure` | `SurvivorDeathCause.Needs` (detail: cold/weather) | `exposure` | Blizzard, shelter failure, column separation | Live & Supported |
| `suicide` | `SurvivorDeathCause.Scripted` / Despair | `suicide` | Psychological collapse, grief cascade, lost hope | Live & Supported |
| `infection` | `SurvivorDeathCause.Medical` (detail: sepsis) | `infection` | Untreated puncture, dirty salvage cut, wound fever | Live & Supported |
| `old_age` | `SurvivorDeathCause.Needs` (lifespan/natural) | `old_age` | Advanced age in bunker, natural cessation | Live & Supported |
| `drowning` | `SurvivorDeathCause.Expedition` (detail: water) | `drowning` | Rotten ice crossing, culvert flood, river surge | Live & Supported |
| `frostbite` | `SurvivorDeathCause.Needs` (detail: cold/necrosis)| `frostbite` | Perimeter watch cold injury, frozen extremities | Live & Supported |
| `poisoning` | `SurvivorDeathCause.Needs` / Medical (toxic) | `poisoning` | Industrial runoff, toxic cistern, contaminated water | Live & Supported |
| `execution` | `SurvivorDeathCause.Scripted` / Combat (sentence) | `execution` | Faction trial, checkpoint summary sentence, coal wall | Live & Supported |
| `unknown` | `SurvivorDeathCause.Unknown` | `unknown` | Unidentified grave, weathered initials, missing tag | Live & Supported |
| `unspecified` | `MemorialSystem` fallback default | `unspecified` | Blank/null input fallback in MemorialSystem | Live & Supported |

---

## 2. Fallback Rules

1. If a survivor dies with an empty or null cause string, `MemorialSystem.Memorialize` automatically defaults `Cause` to `"unspecified"`.
2. If an environmental grave generator queries for an unmapped cause string, it resolves to `"unknown"` or `"unspecified"`.
3. Both `"unspecified"` and `"unknown"` are explicitly represented in `wasteland_grave_epitaphs.json`.
