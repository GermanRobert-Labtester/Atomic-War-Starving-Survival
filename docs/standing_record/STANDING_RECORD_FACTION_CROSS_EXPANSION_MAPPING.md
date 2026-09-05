# Standing Record Faction Cross-Expansion Integration & Mapping

## 1. Expansion Architecture Overview

ASHFALL organizes its content into distinct expansion modules that interact through shared geographic anchors and data-authority contracts:
- **Expansion 01:** Holdfast (`holdfast_factions.json`)
- **Expansion 02:** Nobodys Charter / The Crossing (`crossing_factions.json`)
- **Expansion 03:** The Standing Record (`standing_record_factions.json`, `standing_record_quests.json`)
- **Expansion 04:** The Silent Foundry (`foundry_faction.json`, `foundry_accords.json`)
- **Expansion 05:** The Duty Roster (`duty_roster_quests.json`)
- **Core World:** Wasteland Map & Settlements (`wasteland_map_v1.json`, `wasteland_settlement_gazetteer.json`)

---

## 2. Cross-Expansion Alignment Matrix

| Standing Record Faction | Crossing Equivalent (`crossing_factions.json`) | Holdfast Equivalent (`holdfast_factions.json`) | Foundry Relation (`foundry_accords.json`) | Settlement Control (`wasteland_settlement_gazetteer.json`) |
|---|---|---|---|---|
| `faction_the_overlay` | — | — | Cadastral survey baseline | — |
| `faction_the_scale` | `faction_the_scale` | — | Weighbridge accord party | — |
| `faction_the_compact` | `faction_the_compact` | — | Charter boundary party | — |
| `faction_the_underwrite` | `faction_the_underwrite` | — | Diesel risk insurer | — |
| `faction_the_cutters` | — | `faction_the_cutters` | Ice road haulage signatory | — |
| `faction_the_fleet` | — | `faction_the_fleet` | Berth 9 wharfage partner | — |
| `faction_the_rebuilders` | — | — | Slag/grain trade partner | Verge Allotments & Grain Silo |
| `faction_the_garrison` | — | — | Fortified security checkpoint | Fort Karkov Marshalling Yard |

---

## 3. Coexistence & Foreign Key Discipline

- **No Redundant Morality Engines:** Standing Record factions do not duplicate the player karma meter or `MoralChoiceSystem`.
- **No Divergent Trust Counters:** Starting trust is statically defined in data (`trust: 0`). Dynamic reputation changes are persisted in runtime save state (`StandingRecordSaveStore`) without rewriting base data JSONs.
- **Unified Region Authority:** All geographic strings resolve to regions specified in `WASTELAND_REGION_ATLAS.md`.
