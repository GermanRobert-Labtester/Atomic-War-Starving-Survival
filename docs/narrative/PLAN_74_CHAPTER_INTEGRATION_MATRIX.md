# Plan 74 — Chapter Integration Matrix

_ASHFALL · docs/narrative · Plan 74_

---

## Runtime Consumers

`narrative_progression.json` is consumed by two live systems:

| Consumer | Role | Read field |
|---|---|---|
| `NarrativePanel` | Displays chapter list in Events Log UI | `description`, `order` |
| `NarrativeEncounterSystem` | Gates encounters by chapter `order` | `order` |

No other Core system directly reads `narrative_progression.json` at runtime.

---

## Cross-System Integration Points (per chapter)

| # | Title | Relevant live systems | Integration type |
|---|---|---|---|
| 1 | The Exchange | — | Display only |
| 2 | Ashfall | RadiationSystem, WeatherSystem | Display + encounter gating |
| 3 | The Bunker | ShelterSystem, SurvivorsHostSession | Display + encounter gating |
| 4 | First Contact | FactionCatalog, NPCSystem | Display + encounter gating |
| 5 | The Long Winter | WeatherSystem | Display + encounter gating |
| 6 | The Consolidation | NeedsSystem, SurvivorWorkSystem | Encounter gating (by order) |
| 7 | The First Winter | WeatherSystem, FuelSystem, TradeSystem | Encounter gating |
| 8 | The Long Dark | MoraleSystem, NeedsSystem | Encounter gating |
| 9 | The Thaw | ExpeditionSystem, WeatherSystem | Encounter gating |
| 10 | The Schism | FlagSystem, NPCSystem | Encounter gating |
| 11 | The Black Market | TradeSystem, FactionCatalog | Encounter gating |
| 12 | The Reckoning | VerdictSystem (Plan 82 seam) | Encounter gating |
| 13 | The Rebuilding | ShelterSystem, FactionCatalog | Encounter gating |
| 14 | The Second Winter | WeatherSystem, FuelSystem | Encounter gating |
| 15 | The Inheritance | EndingSystem | Encounter gating |

---

## Data File Cross-References

| Chapter | Relevant data authority |
|---|---|
| 9 | `expeditions.json` (53 destinations) |
| 11 | `factions.json`, trade catalogs |
| 12 | `verdict_locations.json` (15 sites, Plan 82) |
| 14–15 | `endings.json`, `narrative_questlines.json` |

---

## ContentUtilizationScanner Registration

`narrative_progression.json` is registered under:

- `NarrativeEncounterSystem` (lines 360, 529, 690 of `ContentUtilizationScanner.cs`)
- `NarrativePanel` (line 858)
- Knowledgeable catalog list (lines 82, 951)

No scanner changes required under Plan 74.

---

_Last updated: Plan 74 — 2026-09-03_
