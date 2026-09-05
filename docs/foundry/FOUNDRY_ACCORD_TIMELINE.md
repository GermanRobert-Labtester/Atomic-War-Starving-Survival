# Foundry Accord Chronology & Ratification Timeline

**Authority:** `Assets/StreamingAssets/Data/foundry_accords.json`
**Query Surface:** `RegionalTreatyCatalog.GetRatifiedByDay(int currentDay)`

---

## 1. Chronological Sequence of Accords

| Day | Treaty ID | Title | Historical Context & Causation |
|---:|---|---|---|
| **120** | `treaty_garrison_grain_tithe_compact` | The Garrison Grain Tithe Compact | Spring thaw opens the Verge flats; Rebuilder farmers trade grain tithes for Garrison armed road escorts. |
| **180** | `treaty_flotilla_saline_corridor_concordat` | The Flotilla Saline Corridor Concordat | Summer high-tide season begins; Lock Gate Four dredged and diesel tariff established to enable coastal shipping. |
| **210** | `treaty_switchback_fuel_and_passage_accord` | The Switchback Fuel & Passage Accord | Pre-winter stock-up; lamp oil and thermal liners traded for safe pilgrim passage before early snows. |
| **240** | `treaty_scale_suburban_fair_trade_convention` | The Scale Suburban Fair Trade Convention | Bulk harvest barter season; standard bronze balance weights established across Caravanserai and Grange Hall. |
| **260** | `treaty_scrap_salvage_demarcation` | The Scrap Salvage Demarcation | Heavy equipment recovery begins; Cutters sort Sector 8 structural steel for Scale machine components. |
| **280** | `treaty_brine_pipe_and_iodine_exchange` | The Brine Pipe & Iodine Exchange | Smelter bay cupola fired as frost deepens; first cast of lead-antimony pipe delivered for medical iodine. |
| **290** | `treaty_roster_border_demilitarization_pact` | The Roster Border Demilitarization Pact | Winter standoff frozen in place; 5km Neutral Ground buffer formalized to avoid accidental artillery duels. |
| **305** | `treaty_cluster_labour_schedule` | The Cluster Labour Schedule | Mid-winter peak casting heats; stoker shifts capped at 8 hours and Cluster school boil orders protected. |
| **315** | `treaty_deep_coast_aquifer_protection_treaty` | The Deep Coast Aquifer Protection Treaty | Winter storm surges threaten intake marsh; joint anti-pollution screens and emergency drought terms ratified. |
| **330** | `treaty_road_iron_charter` | The Road Iron Charter | The ice road freezes solid; 60 ice anchors and 3 winch drums delivered to keep haulage columns moving. |
| **340** | `treaty_high_scarp_observatory_sanctuary` | The High Scarp Observatory Sanctuary | Late-winter atmospheric storms; Summit Relay couplers traded for regional radiation forecasting data. |
| **365** | `treaty_the_cluster_charter` | The Cluster Charter | Year One constitution finale; the Silent Foundry entered on the permanent schedule as a civil works. |

---

## 2. Dynamic Ratification Query Progression

As the campaign progresses from Day 1 to Day 365, `RegionalTreatyCatalog.GetRatifiedByDay(day)` dynamically activates accords in step with wasteland history:
- Day 100: 0 accords active.
- Day 150: 1 accord active (`garrison_grain_tithe`).
- Day 200: 2 accords active.
- Day 250: 4 accords active.
- Day 300: 7 accords active.
- Day 350: 11 accords active.
- Day 365: All 12 accords active.
