# Relic Component Inventory (Plan 87)

*Audit of `items.json` (659 items) against the nine new relic recipes.
Result: **zero new component items required** — every recipe reuses the
existing component vocabulary.*

## Existing components available (verified IDs)

### Mechanical
`spring_mechanism`, `mechanical_parts`, `mechanical_components` (higher tier),
`scrap_mechanical`, `scrap_metal`, `machine_oil`, `lubricant_oil`,
`item_magnetic_bearing_coil`, `item_foundry_bearing_housing`

### Optical / glass
`item_optical_flat` (precision optical element), `item_welders_glass`,
`item_cast_borosilicate_glass_blank`, `camera_lens_cleaner`

### Wood / cloth / cordage
`scrap_wood`, `wood_block`, `wooden_plank`, `cloth`, `rope`, `leather_strap`

### Electrical / other
`vacuum_tube`, `copper_wire_10m_of_10m`, `soldering_kit`, `antenna_coil`,
`scrap_plastic`, `paper_stock`, `phonograph_needle`, `film_reel`,
`photographic_film`, `typewriter_ribbon`, `spring_key`, `music_box_comb`,
`projector_bulb`

## New-item threshold review (Task 87D.4)

| Candidate new item | Decision | Reason |
|---|---|---|
| `clock_spring` | **not added** | `spring_mechanism` already covers clockwork springs (used by gramophone + 3 tech relics) |
| `sewing_machine_belt` | **not added** | `mechanical_parts` covers drive components; no existing belt item and no need for the distinction |
| `telescope_lens` | **not added** | `item_optical_flat` is the existing precision-optics component (metallurgy/optics economy) |
| `printing_ink` | **not added** | no ink item exists, but the press recipe reads correctly with `paper_stock` + mechanism parts; the plan's threshold ("at least one relic genuinely needs the distinction") is not met — ink adds no distinct gameplay decision |
| `violin_string` | **not added** | `copper_wire_10m_of_10m` is the established string/wire equivalent |
| `compass_magnet` | **not added** | `item_magnetic_bearing_coil` exists and is magnetized-precision by definition |
| `kite_paper` / kite string | **not added** | `cloth` + `rope` are the exact existing correspondents |

No relic requires more than one component from any single family, and no
recipe contains a bespoke component. Recipe component counts: 3–4,
matching the existing six (2–3) and staying within the tech-relic range
(2–5).

## Scarcity logic (Task 87F.4)

- **Optical artifacts** (telescope, microscope): `item_optical_flat` — the
  rarest shared component in the set; glass blanks as secondary.
- **Mechanical artifacts** (clock, sewing machine, compass, grinder):
  `spring_mechanism` / `mechanical_parts` — common workshop stock.
- **Art/culture** (violin): wood + wire + fine parts.
- **Domestic/play** (press, kite): wood, cloth, rope, paper — the most
  common vocabulary.
