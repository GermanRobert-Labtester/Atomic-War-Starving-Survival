# ORAL LORE PERFORMANCE MATRIX — Plan 155 Task C

All 26 pieces classified by `performance_context` into the producer
categories; primary producer; allowed/forbidden consequences. **Activated**
pieces have a registered primary producer in
`OralLorePerformanceSystem.DefaultProducerMap()`; **deferred** pieces state
their reason.

Context families: shelter-labour · social · nursery · memorial ·
faction/local · expedition · medical-comfort · radio/archive · solitary.

## Shelter labour (4)

| Piece | Context (authored) | Producer | Allowed consequence | Forbidden |
|---|---|---|---|---|
| `song_01_blower_crank_cadence` | Pairs cranking the ventilation fan during storm alerts | `room_filtration` | first-heard journal; codex | No air output change; no fatigue delta |
| `oral_b2_the_pump_room_shanty` | Water crew hand-pumping the emergency well | `room_water_pump` | first-heard journal; codex | No water output change |
| `song_05_smiths_striking_chant` | Master Oleg + striker at the anvil | `room_foundry` | first-heard journal; codex | No crafting bonus |
| `song_09_syndicate_scales_shanty` | Porters hauling salt sacks to the counting table | `room_storage_bay` | first-heard journal; codex | No trade-price effect |

## Social / common-room (3)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `song_06_bakers_sawdust_tune` | Mess-hall wait for biscuit distribution | `room_kitchen` | first-heard journal; codex | No hunger/morale delta |
| `oral_b2_the_geiger_counter_waltz` | Social dance; rhythm mimics dosimeter clicks | `room_main` | first-heard journal; codex | Never defines safe CPM/dose; no radiation change |
| `oral_b2_the_bunker_is_my_body` | Unison grounding exercise turned hymn ("The Therapist introduced it") | `room_main` | first-heard journal; codex | No sanity/stress mechanic; role prose is not an NPC claim |

## Nursery / education (3)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `song_02_hazard_rhyme_colors` | Picket Schoolroom hazard-indicator memorization | `context_nursery` | first-heard journal; codex | Lyrics do not authenticate real hazard signs (presentation only) |
| `oral_b2_nursery_rhyme_the_ash_falls_down` | Toddlers' hand-gesture rhyme | `context_nursery` | first-heard journal; codex | — |
| `song_13_childrens_skipping_rhyme` | Jump-rope counting-out in the main corridor | `context_nursery` | first-heard journal; codex | — |

## Memorial / funeral (2)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `song_10_crypt_chiseler_dirge` | Sister Mara carving epitaphs in the granite crypt | `context_memorial` | first-heard journal; memorial-surface association | No resurrection of memorial mechanics |
| `oral_b2_hymn_of_the_settling_dust` | Day of Remembrance annual unison hymn | `context_memorial` | first-heard journal; memorial-surface association | No calendar event created by the archive |

## Expedition / scout travel (2)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `song_08_ice_road_courier_cadence` | Couriers pacing between marker posts in whiteouts | `location_frozen_wetland` (deep-lore site) | first-heard journal; discovered-at-site provenance | Route mnemonics never modify distance, unlock sites or supersede map topology |
| `oral_b2_the_cartographers_song` | Bram Ostrowski's landmark mnemonic, sung by couriers/scouts | `location_metro_tunnel` (deep-lore site) | first-heard journal; codex | Landmark lyrics are narrative; no typed clue mapping exists |

## Faction / local culture (2)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `oral_b2_the_salt_freeholders_march` | Salt Freeholders walking claim posts; sung as a territorial statement | `faction_salt_freeholders` (canonical faction) | first-heard journal; codex | Lyrics are perspective, never map ownership or faction standing |
| `song_15_strike_anthem_iron` | Picket School students + foundry workers, Article 4 rail strike | `room_foundry` | first-heard journal; codex | No faction aggression change |

## Medical comfort (1)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `oral_b2_surgeons_lullaby` | The Surgeon hums it pre-surgery; wordless, from her mother | `room_clinic` | first-heard journal; codex display | The authored "patients settle" claim is testimony; NO healing/anxiety delta exists (no typed comfort effect wired) |

## Radio / archive-recovered (1)

| Piece | Context | Producer | Allowed | Forbidden |
|---|---|---|---|---|
| `song_14_keepers_sky_chant` | Broadcast on 88.4 Pirate Free-Net during the Harrow-4 orbital decay pass | `context_radio_archive` | first-heard journal; codex; Plan 152/73 flavor cross-link | No countdown, no orbital event; Harrow-4 is a distinct platform family from OLYMPUS |

## Deferred (8) — explicit reasons

| Piece | Deferred reason |
|---|---|
| `song_03_canal_rowers_ballad` | Producer site for the flooded-lowland barge routes needs a Plan 116 maritime anchor; keep for the caravan/route content pass |
| `song_04_hearth_lullaby_coal` | Solitary/private performance; needs a bunk-room privacy surface (no per-room ambient trigger exists yet) |
| `song_07_vels_keys_hymn` | Anniversary vigil needs the memorial/calendar surface; Dr. Irina Vel is canon — activation should be a deliberate ceremony tie-in |
| `song_11_greenhouse_sprout_carol` | First-sprout trigger belongs to the greenhouse system's event stream; wire with GreenhouseExpansionCatalog.Events.FirstSprout in a follow-up |
| `song_12_sovereign_wreck_shanty` | Diver/salvage context belongs to the Maritime/Black Flotilla systems; defer to their content pass |
| `song_16_century_opening_anthem` | Tessarat ten-year ceremony is future content; defer |
| `oral_b2_ballad_of_the_last_caravan` | Gatherings AND funerals dual-context; defer until a caravan-arrival surface is chosen (candidate: Plan 147 broker arrival journal) |
| `oral_b2_lament_for_the_surface` | "Never performed for an audience" — solitary airlock-night context; requires a solitary-surface trigger; defer (and respect the taboo in any future presentation) |

## Cross-cutting rules

- Performer identities: only canon-verified people are referenced (Bram
  Ostrowski, Dr. Irina Vel, Mara Veln, Master Oleg, Sister Mara, Harlan).
  "The Therapist", "The Surgeon", "The Priest" remain role prose.
- Every consequence is first-heard journal + codex display. Zero mechanical
  effects exist in Plan 155 (Task F firewall; pinned by tests).
- Persistence: lore IDs only (checksummed `oral_lore` save section);
  lyrics/context/tempo always derive from the data authority at runtime.
