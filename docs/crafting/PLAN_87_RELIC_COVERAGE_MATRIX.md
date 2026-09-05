# Plan 87 — Relic Coverage & Emotional Niche Matrix

## 1. Executive Summary & Design Principles

Relic restoration represents the shelter choosing to spend scarce labor and materials on preserving humanity and normalcy beyond immediate survival.

The progression expands from 6 initial relics to a balanced 15-relic progression arc without duplicating emotional or functional niches.

---

## 2. Critical Coverage Decisions & Deduplication

### Why the Slide Projector was Replaced with the Laboratory Microscope
- **Conflict:** The draft proposal suggested a "Slide Projector". However, the existing catalog already contains `film_projector` ("8mm Film Projector"), which completely occupies the shared visual memory/projected imagery niche. Adding a slide projector alongside a film projector would be an overt functional and emotional duplicate.
- **Resolution:** Replaced with `laboratory_microscope` ("Monocular Compound Microscope"). This introduces a completely fresh niche: scientific observation, inquiry, and curiosity about the microscopic world that sustained or threatened them.

### Distinction: Live Performance (`violin`) vs Mechanical Playback (`gramophone`, `music_box`)
- `gramophone`: Communal playback of historical records; static nostalgia for a lost world.
- `music_box`: Intimate, mechanical, delicate music of a single repetitive melody.
- `violin`: Live, imperfect, human performance created by someone currently alive in the shelter. The first note catches and breaks, but it represents living art, not recorded ghosts.

### Distinction: Public Reproduction (`hand_printing_press`) vs Personal Record (`typewriter`)
- `typewriter`: Individual writing, one-off records, typing duty rosters and personal letters.
- `hand_printing_press`: Multi-copy civic publication, broadsides, poems, shared public information, and community memory.

---

## 3. The 15 Relic Coverage Matrix

| # | Relic ID | Display Name | Primary Niche | Emotional Payoff | Components | Time (h) | Morale | Narrative Event | World Flag |
|---|---|---|---|---|---|---:|---:|---|---|
| 1 | `gramophone` | Hand-Crank Gramophone | Communal Audio | Communal gathering around lost recordings | `vacuum_tube`, `spring_mechanism`, `phonograph_needle` | 8 | 5 | `narrative_gramophone_restored` | `relic_restored_gramophone` |
| 2 | `film_projector` | 8mm Film Projector | Shared Cinema | Shared visual memory on a blank wall | `projector_bulb`, `lubricant_oil`, `film_reel` | 6 | 5 | `narrative_projector_restored` | `relic_restored_film_projector` |
| 3 | `ham_radio` | Vintage Ham Radio Set | Long-Range Comms | Hearing a faint human voice through the static | `vacuum_tube`, `antenna_coil`, `soldering_kit` | 12 | 5 | `narrative_ham_radio_restored` | `relic_restored_ham_radio` |
| 4 | `music_box` | Antique Music Box | Intimate Music | Private domestic memory and quiet comfort | `music_box_comb`, `spring_key` | 4 | 3 | `narrative_music_box_restored` | `relic_restored_music_box` |
| 5 | `typewriter` | Mechanical Typewriter | Personal Record | Documentation, letters, and permanent ink | `typewriter_ribbon`, `machine_oil` | 3 | 3 | `narrative_typewriter_restored` | `relic_restored_typewriter` |
| 6 | `camera` | Twin-Lens Reflex Camera | Visual Capture | Preserving the faces of living survivors | `camera_lens_cleaner`, `photographic_film` | 5 | 3 | `narrative_camera_restored` | `relic_restored_camera` |
| 7 | `mantel_clock` | Brass Mantel Clock | Shared Routine | Daily shifts measured against steady ticking, not sirens | `spring_mechanism`, `mechanical_parts`, `machine_oil` | 6 | 4 | `narrative_mantel_clock_restored` | `relic_restored_mantel_clock` |
| 8 | `sewing_machine` | Treadle Sewing Machine | Practical Care | Quiet mending of worn garments rather than emergency fixes | `mechanical_parts`, `lubricant_oil`, `leather_strap` | 7 | 4 | `narrative_sewing_machine_restored` | `relic_restored_sewing_machine` |
| 9 | `telescope` | Brass Refractor Telescope | Wonder & Sky | Looking at stars without checking for fallout drift | `optical_lens`, `mechanical_parts`, `scrap_metal` | 9 | 5 | `narrative_telescope_restored` | `relic_restored_telescope` |
| 10 | `hand_printing_press` | Tabletop Platen Press | Civic Culture | Replicating words, notices, and memorial sheets for all | `mechanical_parts`, `empty_toner_cartridge`, `wooden_plank` | 10 | 5 | `narrative_printing_press_restored` | `relic_restored_printing_press` |
| 11 | `violin` | Spruce & Maple Violin | Live Art | Music made by living breath and hand in the room | `wooden_plank`, `copper_wire_10m_of_10m`, `leather_strap` | 8 | 5 | `narrative_violin_restored` | `relic_restored_violin` |
| 12 | `laboratory_microscope` | Monocular Compound Microscope | Science & Inquiry | Disciplined curiosity examining the hidden fabric of life | `optical_lens`, `camera_lens_cleaner`, `mechanical_parts` | 8 | 4 | `narrative_microscope_restored` | `relic_restored_laboratory_microscope` |
| 13 | `brass_compass` | Prismatic Marching Compass | Orientation | Tangible directional certainty on the table | `spring_mechanism`, `mechanical_parts`, `scrap_metal` | 4 | 3 | `narrative_compass_restored` | `relic_restored_brass_compass` |
| 14 | `box_kite` | Weather-Station Box Kite | Play & Wonder | Something climbing into the sky purely for joy | `cloth`, `scrap_wood`, `rope` | 3 | 2 | `narrative_kite_restored` | `relic_restored_box_kite` |
| 15 | `coffee_grinder` | Cast-Iron Coffee Mill | Domestic Ritual | The comforting morning aroma of fresh grounds | `mechanical_parts`, `scrap_metal`, `machine_oil` | 4 | 3 | `narrative_coffee_grinder_restored` | `relic_restored_coffee_grinder` |
