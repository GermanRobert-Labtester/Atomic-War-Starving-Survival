# SPECIES_MIGRATION_MATRIX.md — Plan 28

Twelve seeded species across seven ecological archetypes. Species ids resolve to
`world_evolution_seeds.json` (runtime truth); lore ids resolve to
`narrative/wasteland_wildlife_bestiary.json` (field-guide layer, separate id space).

| species_id | archetype | ecological role | peak window | hazard role | opportunity role | taint susceptibility | trapping relevance |
|---|---|---|---|---|---|---|---|
| species_feral_goat | HerdGrazer | large cold-adapted herd | Black Bloom (1.25) | winter desperation → aggression | thaw interception; hide/meat | route-dependent (deferred) | high (density) |
| species_blight_rat | BurrowSwarm | vermin bloom | Thaw–Bloom (1.3) | rabies vector; pantry pressure | nuisance catch rises in abundance | high (scavenger) | nuisance catch |
| species_cotton_hare | BurrowSwarm | cycling prey | Thaw/Bloom (1.3) | — | steady snare protein in boom | medium | high |
| species_ash_boar | Sounder | mast-fed sounder | The Turning (1.4) | bold near farms in mast fall | heavy quarry window | medium | medium |
| species_iron_crow | PassageFlock | passage bird | Thaw + Turning | scavenger omen (carrion clue, deferred) | flock-window trapping | low | low |
| species_ash_gull | PassageFlock | coastal scavenger | Thaw (1.3) | same | follows fish run | medium | low |
| species_gray_heron | CoastalRunner (piscivore) | follows the run | Thaw (1.5) | none | fish-run indicator species | route-dependent | n/a (wet ground) |
| species_mirror_carp | CoastalRunner | **the fish run** | Thaw 1.5 → Bloom 1.4 | none | coastal bounty window | route-dependent (deferred) | fish trap (Plan 36) |
| species_ghost_moth | SwarmBlight | warm-damp insect bloom | Black Bloom (1.5) | crop pressure (Plan 22 hook, deferred) | emergency protein / observation | low | none |
| species_rad_dog | Resident | opportunist predator | year-round | rabid packs (live) | — | high (scavenger) | predator encounters |
| species_wolf | Resident | pursuit predator | year-round | prey-collapse pressure (deferred cap) | — | low | — |
| species_dust_lynx | Resident | lone stalker | year-round | low-density ambush flavor | rare pelt | medium | — |

## Archetype authoring standard (applied)

Each archetype differs in **behavior and season**, never in raw combat strength:
hunger pacing, abundance curve, movement constraint (water-bound runners), and
player-facing notice wording. No "boss monster" framing anywhere.

## Data-integrity

- All 12 `species_*` ids appear in `world_evolution_seeds.json` → single authority.
- 13 packs seed across 11 sectors; every `sector_id` resolves (validator + selftest).
- Archetype assignments pinned by `ArchetypeTable_CoversAllSeededSpecies_WithDistinctRoles`.
