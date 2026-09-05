# Relic Component Inventory & Economy Audit (Plan 87)

## 1. Design Strategy: Anti-Proliferation

A core hazard in relic expansion is introducing bespoke, single-use component items for every new recipe (e.g. `clock_spring`, `violin_string`, `printing_ink`, `kite_spar`). Doing so fragments the item economy and burdens scavenging tables with hyper-specific junk.

Plan 87 strictly limits new additions by reusing the existing rich component vocabulary of `items.json`.

---

## 2. Component Cross-Reference & Consumption Map

Across all 15 relics in `relic_recipes.json`, the following 22 components are referenced:

| Component Item ID | Item Display Name | Category | Status | Consuming Relics |
|---|---|---|---|---|
| `vacuum_tube` | Vacuum Tube | Component | Existing | `gramophone`, `ham_radio` |
| `spring_mechanism` | Spring Mechanism | Component | Existing | `gramophone`, `mantel_clock`, `brass_compass` |
| `phonograph_needle` | Phonograph Needle | Component | Existing | `gramophone` |
| `projector_bulb` | Projector Bulb | Component | Existing | `film_projector` |
| `lubricant_oil` | Lubricant Oil | Component | Existing | `film_projector`, `sewing_machine` |
| `film_reel` | 8mm Film Reel | Component | Existing | `film_projector` |
| `antenna_coil` | Antenna Coil | Component | Existing | `ham_radio` |
| `soldering_kit` | Soldering Kit | Component | Existing | `ham_radio` |
| `music_box_comb` | Music Box Comb | Component | Existing | `music_box` |
| `spring_key` | Music Box Key | Component | Existing | `music_box` |
| `typewriter_ribbon` | Typewriter Ribbon | Component | Existing | `typewriter` |
| `machine_oil` | Machine Oil | Component | Existing | `typewriter`, `mantel_clock`, `coffee_grinder` |
| `camera_lens_cleaner` | Lens Cleaning Kit | Component | Existing | `camera`, `laboratory_microscope` |
| `photographic_film` | Photographic Film | Component | Existing | `camera` |
| `mechanical_parts` | Mechanical Parts | Component | Existing | `mantel_clock`, `sewing_machine`, `telescope`, `hand_printing_press`, `laboratory_microscope`, `brass_compass`, `coffee_grinder` |
| `leather_strap` | Leather Strap | Component | Existing | `sewing_machine`, `violin` |
| `scrap_metal` | Scrap Metal | Component | Existing | `telescope`, `brass_compass`, `coffee_grinder` |
| `empty_toner_cartridge` | Empty Toner Cartridge | Component | Existing | `hand_printing_press` |
| `wooden_plank` | Wooden Plank | Component | Existing | `hand_printing_press`, `violin` |
| `copper_wire_10m_of_10m` | Copper Wire (10m) | Component | Existing | `violin` |
| `cloth` | Cloth | Material/Comp | Existing | `box_kite` |
| `scrap_wood` | Scrap Wood | Component | Existing | `box_kite` |
| `rope` | Hemp Rope (10 m) | Component | Existing | `box_kite` |
| `optical_lens` | Optical Lens Element | Component | **New (Plan 87)** | `telescope`, `laboratory_microscope` |

---

## 3. Justification for the Single New Item: `optical_lens`

### Requirement
Both the `telescope` (astronomical refractor) and `laboratory_microscope` (compound optical microscope) demand precision glass optics. While `camera_lens_cleaner` existed in `items.json`, there was no physical lens element in the component catalog.

### Economy Fit
Instead of creating two separate items (`telescope_objective` and `microscope_objective`), a single generic, high-utility component was added:

```json
{
  "id": "optical_lens",
  "displayName": "Optical Lens Element",
  "description": "A precision-ground optical glass lens element in a knurled brass retention cell. Clear, scratch-free, and coated for maximum light transmission. Pre-war observatories, laboratories, and surveying crews depended on such optics to resolve what the naked eye could not.",
  "type": "Component",
  "stackMax": 5,
  "weight": 0.15,
  "tradeValue": 10,
  "empShielded": false
}
```

- **Tagging:** Registered with tag `relic_component` in `expansion_item_tags.json`.
- **Zero Core Dependency:** Uses the standard `Component` item schema.
- **Shared Utility:** Powers both scientific/observation relics (`telescope`, `laboratory_microscope`).
