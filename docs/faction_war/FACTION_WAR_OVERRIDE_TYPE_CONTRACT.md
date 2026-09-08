# Faction War Override Type Contract

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Test Gate:** `Ashfall.Core.Tests.FactionWarContentCatalogTests.LocationOverrides_HaveRequiredFieldsAndConsistentDayWindowRule`

---

## 1. Type Vocabulary & Semantic Specification

The `overrideType` property communicates the strategic, physical, and environmental condition of a location during its active temporal window. The catalog supports a closed vocabulary of 9 canonical types:

```
pre_strike | post_strike | ambient_addendum | occupied | abandoned | fortified | liberated | reclaimed | contaminated
```

### 1.1 Detailed Type Definitions

| Override Type | Category | Semantic Meaning | Gameplay / Visual Presentation |
|---|---|---|---|
| `pre_strike` | Temporal Horizon | The location in its earlier, functioning, or pre-artillery state before major physical devastation. | Orderly civic infrastructure, intact structures, functional civil rationing or shelter. |
| `post_strike` | Structural State | Direct consequence of shelling, blast, fire, or violent breach. | Pulverized masonry, charred timber, cratered ground, twisted rebar, breached vaults. |
| `ambient_addendum` | Social / Operational | Subtle shift in occupancy, atmosphere, or radio presence without destruction. | Overcrowded refugee camps, active listening posts, radio hum, whisper networks. |
| `occupied` | Faction Presence | Active armed garrison or militia detachment controlling the facility. | Sentry posts, searchlights, log revetments, checkpoint gates, perimeter wire. |
| `abandoned` | Post-Conflict Desertion | A previously vital site deserted due to raids, supply collapse, or pestilence. | Rusting machinery, wind through empty doorways, scavenged scrap, silence. |
| `fortified` | Defensive Hardening | Heavy defensive preparation by defenders expecting siege or bombardment. | Earthen revetments, timber cribbing, reinforced parapets, sandbagged gun positions. |
| `liberated` | Civic Recovery | Hostile militia or occupying force repelled or collapsed; access re-opened. | Torn faction banners, burned barricades, cautious civilian trade returning. |
| `reclaimed` | Re-Settlement | Organized survivors or municipal crews returning to repair and repurpose ruins. | Shore-up scaffolding, bucket chains, chemical testing rigs, basic lighting. |
| `contaminated` | Environmental Hazard | Toxic runoff, chemical spillage, industrial sludge, or heavy fallout seepage. | Discolored water pools, chemical scum, caustic vapors, respiratory warnings. |

---

## 2. Type Distribution Across All 20 Overrides

The 20 overrides in the expanded catalog exhibit a balanced distribution across the 9 recognized types:

| Override Type | Count | Override IDs |
|---|---|---|
| `post_strike` | 7 | `loc_override_almshouse_post_strike`, `loc_override_silo_fortified`, `loc_override_conscription_burned`, `loc_override_cache_looted`, `loc_override_weighbridge_barricaded`, `loc_override_granary_burned`, `loc_override_bridge_destroyed`, `loc_override_field_scorched` |
| `pre_strike` | 2 | `loc_override_almshouse_pre_strike`, `loc_override_plaza_cleared` |
| `ambient_addendum` | 2 | `loc_override_waystation_refugee`, `loc_override_understory_transmitter_ambient` |
| `occupied` | 2 | `loc_override_checkpoint_occupied`, `loc_override_factory_occupied` |
| `abandoned` | 2 | `loc_override_village_abandoned`, `loc_override_camp_overrun` |
| `fortified` | 1 | `loc_override_rail_yard_fortified` |
| `liberated` | 1 | `loc_override_roadblock_liberated` |
| `reclaimed` | 1 | `loc_override_station_reclaimed` |
| `contaminated` | 1 | `loc_override_well_contaminated` |

Total: **20 overrides**. Every type represents a distinct world-state transition, ensuring diversity across the campaign timeline.
