# Currents ↔ Coastal Route Matrix (Plan 23 / Task 23C — repository-truth audit)

**Repository truth (verified):** `currents.json` is the **Muster roster of wandering
communities** (`CurrentsCatalogLoader`, "the sector's political actors" — Expansion 06
roster data). It is **not** a sea-current catalog; no sea-current catalog exists
anywhere in the repository. The source plan's "17 sea currents" are these 17 political
actors. Per the plan's own contract, repository truth wins; no fake hydrology catalog
is invented.

## The 17 Muster currents — coastal relevance audit

| Current | Region | Maritime relevance | Plan 23 action |
|---|---|---|---|
| `faction_archivists` | the_drown | coastal | live (roster); lore hooks via Flotilla burial/records culture |
| `faction_lamplighters` | all_regions | routes/beacons | lore-only (inactive) — documented |
| `faction_quiet_house` | the_grid | inland | none (documented inactive) |
| `faction_grain_exchange` | the_verge | inland | none |
| `faction_sun_seekers` | all_regions | — | none |
| `faction_osteophages` | the_drown | coastal, dangerous | coastal encounter flavor; salvage-culture overlap |
| `faction_the_tally` | the_toll | inland | none |
| **`faction_undertow`** | the_drown | **maritime: offers rescue, salvage_recovery, local_knowledge — inactive** | **registered as the Flotilla's rival salvage culture** (Black Flotilla lore + radio claims language); Flotilla claim-tag rules explicitly answer Undertow salvage-recovery offers |
| `faction_cold_count` | the_spine | — | none |
| `faction_deserter_coalition` | the_verge | — | none |
| `faction_the_provisioned` | the_grid | — | none |
| `faction_long_walk` | all_regions | — | none |
| `faction_scavenger_guild` | the_grid | salvage overlap | wreck-rights quest flavor |
| `faction_iron_raiders` | the_toll | dangerous | escort-fleet threat model |
| **`faction_hydro_barons`** | **the_coast** | **coastal water authority — inactive** | wired to the drowned fuel depot + siphon station content; desalination access ties to the brine cistern |
| `faction_the_tempest` | the_spine | — | none |
| `faction_blank_rows` | alloc_12b | — | none |

Two neglected coastal currents (`faction_undertow`, `faction_hydro_barons`) gain live
Flotilla-region consumers via item/quest/radio hooks rather than a fake sea-current
layer. No currents.json row was edited (roster ids are stable save/content contracts).

## Fair/adverse coastal opportunities (23C.4 / 23C.5 — as implemented)

The travel-modifier surface that exists in repository truth is the **deep-coast route
graph** (`District8DeepCoastSystem`: travel hours, danger, rads, contamination) and the
tide windows above. Four fair-water opportunities and three adverse cases are authored
through those existing formulas:

- **Fair (4):** high-water approach to the siphon (tide `high`); falling-tide entries at
  the relay station and quarantine barge; slack-water window at the Sovereign (safer,
  better-tilmed launch); Cape Beacon cistern gallery access at low water.
- **Adverse (3):** `unsafe_at_peak` window on the Barrik (launch refused at peak flow —
  a hidden-cost avoidance, never an ambush); surge-grade storms suspend dock operations
  and raise contamination (economic cost through existing bills); noise floor interacts
  with the stealth model on current-heavy sites (relay, submarine).

No teleportation, no free cargo, no negative travel costs, no route-eligibility bypass:
currents/tides gate **when** existing travel happens, never whether it is free.
