# Plan 87 — Relic Coverage Matrix (existing six + proposed nine)

*Verified against `relic_recipes.json` current source. Cultural tier only
(`category: "relic"`); the 24 `relic_tech_*` records are a separate
functional-salvage progression and out of Plan 87 scope.*

## Existing six (audited from data)

| Relic | Function | Emotional niche | Components | Time (h) | Morale | Event | Flag |
|---|---|---|---|---:|---:|---|---|
| gramophone | communal recorded music | nostalgia, gathering | vacuum_tube, spring_mechanism, phonograph_needle | 8 | 5 | narrative_gramophone_restored | relic_restored_gramophone |
| film_projector | shared visual memory / cinema | watching together | projector_bulb, lubricant_oil, film_reel | 6 | 5 | narrative_projector_restored | relic_restored_film_projector |
| ham_radio | communication, outside voices | connection beyond the shelter | vacuum_tube, antenna_coil, soldering_kit | 12 | 5 | narrative_ham_radio_restored | relic_restored_ham_radio |
| music_box | intimate mechanical music | fragile domestic memory | music_box_comb, spring_key | 4 | 3 | narrative_music_box_restored | relic_restored_music_box |
| typewriter | writing, one copy | records, personal words | typewriter_ribbon, machine_oil | 3 | 3 | narrative_typewriter_restored | relic_restored_typewriter |
| camera | captured images | preserved moments | camera_lens_cleaner, photographic_film | 5 | 3 | narrative_camera_restored | relic_restored_camera |

Observed distribution: time 3–12 h; morale 3 or 5; every relic has a
unique event and a unique `relic_restored_*` flag.

## Final nine (Plan 87 additions)

| Relic | Function | Emotional niche | Components | Time (h) | Morale | Event | Flag |
|---|---|---|---|---:|---:|---|---|
| mantel_clock | shared timekeeping | routine, normalcy | spring_mechanism, mechanical_parts, machine_oil | 4 | 3 | narrative_mantel_clock_restored | relic_restored_mantel_clock |
| sewing_machine | mending as care | quiet domestic craft | mechanical_parts, machine_oil, leather_strap | 6 | 5 | narrative_sewing_machine_restored | relic_restored_sewing_machine |
| telescope | looking at the sky | wonder, perspective | item_optical_flat, mechanical_components, scrap_wood | 9 | 5 | narrative_telescope_restored | relic_restored_telescope |
| hand_printing_press | many copies of words | public written culture | mechanical_components, paper_stock, machine_oil, scrap_wood | 12 | 5 | narrative_hand_printing_press_restored | relic_restored_hand_printing_press |
| violin | live performance | music made by the living | wood_block, copper_wire_10m_of_10m, mechanical_parts | 7 | 5 | narrative_violin_restored | relic_restored_violin |
| laboratory_microscope | disciplined seeing | curiosity, education | item_optical_flat, item_cast_borosilicate_glass_blank, mechanical_parts, machine_oil | 8 | 3 | narrative_laboratory_microscope_restored | relic_restored_laboratory_microscope |
| brass_compass | orientation | certainty, direction | item_magnetic_bearing_coil, mechanical_parts, item_cast_borosilicate_glass_blank | 3 | 3 | narrative_brass_compass_restored | relic_restored_brass_compass |
| box_kite | deliberate play | uselessness, childhood | cloth, scrap_wood, rope | 2 | 3 | narrative_box_kite_restored | relic_restored_box_kite |
| coffee_grinder | morning ritual | smell, hospitality | mechanical_parts, spring_mechanism, scrap_wood | 4 | 3 | narrative_coffee_grinder_restored | relic_restored_coffee_grinder |

## Overlap audit (Task 87C.7)

| Proposed | Nearest existing | Verdict |
|---|---|---|
| mantel_clock | music box (both mechanical/domestic) | distinct — timekeeping/routine vs sound/memory |
| sewing_machine | typewriter (both "useful domestic") | distinct — mending/care vs writing |
| telescope | camera (both optical) | distinct — wonder/looking outward vs recording |
| hand_printing_press | typewriter | distinct by design — typewriter writes **one** copy; press makes **many**; press is public information, typewriter is personal words |
| violin | gramophone, music box | distinct by design — gramophone/music box replay **recorded** pre-war sound; violin is **live** performance by someone alive now |
| laboratory_microscope | film_projector | distinct — **replaces the proposed slide projector**, which would have duplicated the projector's visual-presentation niche |
| brass_compass | — | unique niche (orientation) |
| box_kite | — | unique niche (play) |
| coffee_grinder | — | unique niche (domestic ritual/smell) |

## Rejected proposals

- **Slide projector** — direct duplicate of `film_projector` (shared
  visual presentation niche). Replaced by laboratory_microscope per plan §87E/R12.
- No replacement-pool items needed; the nine-slot slate survived the audit.

All values sit inside the existing distribution (time 2–12 h vs existing
3–12; morale 3/5 vs existing 3/5) — no systematic power creep. The box
kite at 2 h is the deliberately cheapest restoration, matching the
plan's low-complexity ladder; it grants the minimum morale tier.
