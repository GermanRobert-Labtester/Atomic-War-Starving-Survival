# Damaged-Map Zone Matrix — 12 Zones (Plan 85)

All data from the catalog as landed. Area types and reward identities are materially distinct; no two installations share a concept.

| # | zone_id | Frag | Area type | Installation | Function | Reward identity | Dest (danger/ticks) | Producers (location types) |
|---|---|---|---|---|---|---|---|---|
| 1 | `industrial_district` * | 3 | urban-industrial | Underground Fuel Depot | emergency fuel reserve | fuel / mechanical parts | 4 / 11 | industrial_district, rail_yard |
| 2 | `suburban_heights` * | 2 | suburban-civic | Municipal Seed Vault | community seed bank | agriculture | 2 / 8 | apartment_block, school |
| 3 | `military_corridor` * | 3 | military | Blacksite Armory 7 | hardened armory | comms/electronics, bounded ammo | 6 / 18 | military_depot, checkpoint |
| 4 | `crater_ground_zero` * | 3 | ground zero | Collapsed Command Vault | tactical command center | military comms/rations, bounded ammo | 7 / 14 | dead_hand_core, government_bunker |
| 5 | `deep_coast_shelf` * | 2 | coastal-maritime | Dead-Drop Command Shelter | maritime relay station | fuel, filtration, salvage | 5 / 21 | tank_farm, relay_mast |
| 6 | `high_scarp_ridgeline` * | 2 | mountain-comms | Hidden Relay Bunker 09 | cliffside relay shelter | comms, electronics, medical | 4 / 13 | relay_mast, hunting_cabin |
| 7 | `old_medical_quarter` | 3 | urban-medical | Sealed Triage Annex | mass-casualty intake annex | medical consumables (bounded) | 3 / 8 | hospital, clinic, fire_station |
| 8 | `court_district` | 3 | civic-records | Evidence Sub-Basement | courthouse evidence store | documents, records, collectible | 3 / 9 | police_station, archive, printworks |
| 9 | `pasture_valley` | 2 | agro-veterinary | Quarantine Barn | sealed feed/herd quarantine barn | seeds, hand tools | 2 / 12 | veterinary_surgery, farm |
| 10 | `north_woods` | 3 | forestry | Forestry Emergency Store | bermed fire-crew store | fuel, rope, weatherproof gear | 3 / 16 | forestry_compound, hunting_cabin |
| 11 | `university_quarter` | 3 | academic-scientific | Materials Research Sublevel | restricted materials lab | precision tools, reagents, wire | 4 / 12 | school, observatory |
| 12 | `metro_service_ring` | 3 | underground-transit | Electrical Maintenance Exchange | traction-power junction | batteries, cable | 5 / 15 | metro_station, power_substation |

\* = original zones preserved verbatim (reward refs corrected: `generator_parts`→`mechanical_parts`, `heirloom_seeds`→`family_heirloom_seeds` — neither id existed in `items.json`).

## Diversity contract check (§6)

- **Area types:** industrial, suburban, military, ground-zero, coastal, mountain, urban-medical, civic, pastoral, forestry, academic, transit — 12 distinct.
- **Installation functions:** fuel reserve, seed bank, armory, command vault, relay shelter, maritime relay, triage annex, evidence store, quarantine barn, fire-crew store, research lab, power junction — at most two are "storage-bunker-like" (forestry store, evidence store) and their fictions, access stories, and reward identities differ.
- **Fragment counts:** 2 (×3 zones), 3 (×9) — within the 2–4 band.
- **Reward pillars:** medical (1), agriculture (2), tools/repair (2), electrical (1), comms/electronics (3 pre-existing), documents/records (1), fuel/industrial (1), bounded military (2 pre-existing). No single category dominates; no new zone prints currency-equivalent goods; no new zone grants ammunition.
- **Reveal/visit policy (uniform):** node locked until completion edge; reveal is one-way and persisted; destination loot flows through the standard seeded expedition loop (bounded by capacity, stamina, encounter risk — no repeat-visit jackpot).
