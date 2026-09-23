# ASHFALL — Expansion 27 Design Bible
# THE THREAD
### Wave 4 · Clothing, Warmth, Laundry, Textiles, Leather, Insulation, and Identity

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-21
**Domain owners touched:** `Ashfall.Core.Inventory` (WornGear, equipment durability), `Ashfall.Core.Survivors` (NeedsSystem), `Ashfall.Core.Shelter` (ShelterAtmosphereSystem), `Ashfall.Core.Crafting` (CraftingSystem), `Ashfall.Core.Radiation`
**Proposed host owner:** `WardrobeHostSession` (extends inventory, needs, and atmosphere surfaces)
**Existing save sections:** inventory/equipment, `survivors`, `shelter_atmosphere`, `crafting`
**Existing CLI verbs:** inventory/equipment selftests, `--needs-selftest`, `--atmosphere-selftest`, `--data-integrity-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has the bones of a wardrobe. `Inventory.cs` defines
`EquippedItem`, equipment durability (`MaxDurability`, `CurrentDurability`,
`DegradeRate`, `ConditionSink`, `RecordWear`), and a `WornGear` record whose
`EffectiveProtection()` is consumed by radiation exposure, so clothing already
degrades and already feeds one real system. `NeedsSystem` tracks `Warmth` and
`Hygiene` per survivor — 100 and 100 at start — and hard-codes a
`WasWarmthCritical` flag. `ShelterAtmosphereSystem` tracks `ThermalComfort` and
`Cleanliness` for the shelter as a whole. `items.json` contains real clothing:
`item_heavy_wool_coat`, `item_insulated_boots`, `wool_blanket`, `thermal_blanket`,
`childs_red_scarf`, `camo_ash_cloak`, `protective_childs_coat`,
`protective_rubber_gloves`, `mechanic_gloves`, and a `cloth` material.
`recipes.json` includes `craft_textile_repair` among 38 textile-adjacent recipes.

What does not exist: a garment and layering model, a laundry loop, fiber
production (spinning, weaving, knitting, felting), leatherwork as a gameplay
path, a uniform and identity layer, and shelter insulation as a planned
upgrade. The clothing that exists is equipment with durability and one
protection number.

**The Thread** turns that seam into the shelter's most intimate craft: what
people wear, why it matters in the cold, how it is washed and mended, how cloth
is made from fiber, and how a coat can carry a name.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The first winter a shelter loses someone to cold is the winter it learns that
clothing is infrastructure.

**The Thread** is the expansion about what the shelter wears: layers and warmth,
mending and laundry, fiber and looms, leather and boots, uniforms and mourning
dress. It extends the live equipment durability model with garments that have
warmth, fit, and condition; it extends the live warmth and hygiene needs with
consequences that come from what people actually put on their bodies; it extends
the live shelter atmosphere with insulation, bedding, and curtains; and it adds
a textile production chain from fiber to cloth so a shelter can stop depending
on salvage.

The expansion's hard rules follow the live owners: `Inventory` and `WornGear`
remain the equipment authority, `NeedsSystem` remains the only place warmth and
hygiene consequences are applied, `ShelterAtmosphereSystem` remains the shelter
comfort authority, radiation protection stays in the radiation pipeline, and
disease exposure goes through the medical pipeline. Nothing is invented twice.

### 1.2 The five loops it adds

```
   Fiber ──► Spin ──► Weave ──► Cloth ──► Garment
     │                    │                   │
     ▼                    ▼                   ▼
   Flax, wool,        Loom, knit,         Layers, warmth,
   hide, salvage      felt                fit, condition
                                              │
                    ┌─────────────────────────┤
                    ▼                         ▼
                 Laundry ──► Hygiene      Mending ──► Durability
                    │                         │
                    ▼                         ▼
                 Insulation ──► Shelter     Identity ──► Uniform, mourning
                 warmth                     dress, heirlooms
```

### 1.3 What the player manages

1. **Layers.** Base, mid, outer, and footwear per survivor; warmth, bulk,
   protection, and fit.
2. **Condition.** Wear, tears, holes, thinning, and repair before a coat fails.
3. **Laundry.** Washing, drying, mending rotation, and hygiene; dirty clothes
   carry risk.
4. **Fiber.** Flax, wool, hide, salvaged textiles, and the path from raw fiber
   to cloth.
5. **Leather and tanning.** Hides to leather to boots, belts, straps, and
   harness.
6. **Insulation.** Bedding, curtains, door seals, window covers, and the shelter
   thermal comfort number.
7. **Identity.** Work clothing, uniforms, mourning dress, children's clothes,
   and heirlooms that outlive a person.
8. **Trade.** Cloth and finished garments as goods with real regional demand.

### 1.4 What it is not

- Not a second inventory or equipment system. Items stay in `Inventory`;
  `WornGear` remains the protection record.
- Not a second needs system. Warmth and hygiene consequences route through
  `NeedsSystem.Modify`.
- Not a fashion simulator. Identity is authored and light; warmth and
  condition are the mechanical core.
- Not a second radiation or disease authority.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Inventory/Inventory.cs` | Equipment, durability, `WornGear`, `RecordWear` | `LIVE` |
| `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` | `Warmth`, `Hygiene`, `WasWarmthCritical` | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAtmosphereSystem.cs` | `ThermalComfort`, `Cleanliness`, mood modifiers | `LIVE` |
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | Recipes including `craft_textile_repair` | `LIVE` |
| `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` | Consumes `WornGear.EffectiveProtection()` | `LIVE` |
| `Assets/Ashfall.Core/Narrative/TextileSpinningWeavingCatalog.cs` | Textile lore catalog | `LIVE` |
| `Assets/Ashfall.Core/Narrative/PolymerTextileCatalog.cs` | Polymer textile lore | `LIVE` |
| `Assets/Ashfall.Core/Narrative/TanningLeatherCatalog.cs` | Leather lore | `LIVE` |
| `Assets/Ashfall.Core/Narrative/TanningLeatherworkCatalog.cs` | Leatherwork lore | `LIVE` |
| `Assets/Ashfall.Core/Narrative/LeatherworkArchiveSystem.cs` | Leather archive | `LIVE` |
| `Assets/StreamingAssets/Data/items.json` | Clothing and `cloth` items | `LIVE` |
| `Assets/StreamingAssets/Data/recipes.json` | 38 textile-adjacent recipes | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `items.json` | 379.6 KB | Clothing items exist, no garment metadata |
| `recipes.json` | rich | `craft_textile_repair` among textile recipes |
| `leather_harness_conditioning_audits.json` | 5.3 KB | narrative |
| `pneumatic_cylinder_leather_assays.json` | 5.3 KB | narrative |
| Textile JSON catalogs | none found | textile knowledge is C# narrative only |

### 2.3 Confirmed gaps

- **GAP-27-1 — No garment model.** Clothing is equipment with durability and
  rad protection; no warmth, layers, fit, or coverage.
- **GAP-27-2 — No laundry loop.** `Hygiene` exists; nothing washes clothes.
- **GAP-27-3 — No textile production.** No spinning, weaving, knitting, or
  felting anywhere in gameplay.
- **GAP-27-4 — No leatherwork gameplay.** Lore catalogs and archive exist; no
  tanning or leather crafting path.
- **GAP-27-5 — No insulation planning.** `ThermalComfort` exists; no bedding,
  curtains, seals, or window covers.
- **GAP-27-6 — No uniform or identity layer.** Work roles have no dress; no
  mourning dress, no heirlooms.
- **GAP-27-7 — No textile content data.** No `garments.json`, no textile item
  family, no fiber catalog.
- **GAP-27-8 — No mending depth.** One repair recipe; no tear states, patching,
  or hand-me-downs.
- **GAP-27-9 — No cold-weather death path.** `WasWarmthCritical` exists without
  an authored failure story.

### 2.4 Non-duplication statement

This expansion will **not** add a second inventory, equipment, needs, atmosphere,
radiation, disease, or crafting authority. Garments are items with metadata;
`WornGear` continues to derive protection from equipped items; all warmth and
hygiene consequences route through `NeedsSystem.Modify`; shelter comfort stays
in `ShelterAtmosphereSystem`; radiation stays in `RadiationSystem`; exposure
events route through `DiseaseSystem.TryExpose` where applicable. No second save
section is added.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Clothing is infrastructure.** Warmth is a survival system with a
body count, not a cosmetic layer.

**Pillar 2 — Every garment has a history.** Worn, mended, handed down, buried.
The expansion makes that history visible without turning clothes into loot.

**Pillar 3 — Making cloth is slow on purpose.** Fiber to yarn to cloth to
garment is a chain of patient work; that is why salvage mattered first.

**Pillar 4 — Clean clothes are medicine.** Laundry is a hygiene and disease
loop, not a chore list.

**Pillar 5 — What you wear says who you are.** Uniforms, mourning dress, work
clothes, and children's clothes carry identity with a light touch.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Winter | Cold hands, layers counted | Freezing spectacle |
| Mending | A needle, a patch, patience | Craft-show triumph |
| Laundry | Steam, lines, soap | Montage |
| Leather | Smoke, hides, work | Gross-out |
| Uniform | A role worn visibly | Military fetish |
| Heirloom | A coat that outlives someone | Sentimental overload |

### 3.3 Content limits

- Cold is depicted with restraint; no lingering death scenes.
- No shaming of bodies, dress, or poverty.
- No real-world brands, uniforms, or insignia.
- Children's clothing handled with care and no sentimentality.
- No sexualized or exploitative dress content.

---

## 4. THE SHELTER'S WARDROBE WORLD

### 4.1 Interior rooms

- **`room_sewing_room`** — benches, needles, thread, and fitting.
- **`room_loom_hall`** — looms, warping frame, and the rhythm of weaving.
- **`room_laundry`** — tubs, boiling, wringing, and drying lines.
- **`room_tannery`** — soaking, liming, scraping, and curing.
- **`room_cobbler`** — lasts, awls, soles, and boots.
- **`room_wardrobe_store`** — racks, cedar, moth protection, and issue.
- **`room_drying_loft`** — warmth rising, clothes drying overhead.
- **`room_fitting_room`** — mirrors where they exist, and quiet.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_fiber_field` | The Fiber Field | 3 | Flax, hemp, and retting |
| `loc_shearing_pen` | The Shearing Pen | 3 | Wool from herd animals |
| `loc_tannery_yard` | The Tan Yard | 4 | Hides and curing |
| `loc_textile_ruins` | The Mill Ruins | 5 | Looms, spindles, salvage |
| `loc_dye_walk` | The Dye Walk | 3 | Plant dyes and mordants |
| `loc_rag_market` | The Rag Market | 4 | Salvage cloth and trade |
| `loc_cold_hollow` | The Cold Hollow | 5 | Winter test for gear |
| `loc_felt_works` | The Felt Works | 4 | Pressed wool and blankets |
| `loc_bootmaker_ruins` | The Bootmaker | 4 | Lasts, tools, and leather |
| `loc_dyers_stream` | The Dyers' Stream | 4 | Water for dye and retting |

All locations require valid item references and scanner registration.

### 4.3 The daily wardrobe loop

Morning issue, day wear, work changes, evening wash, weekly mending, seasonal
rotation. The expansion's cadence is the laundry week, because that is the
rhythm that makes a shelter feel lived in.

---

## 5. MAIN STORYLINE — "WHAT WE WORE THROUGH IT"

### 5.1 Central conflict

The shelter's first real winter is coming and the clothing is not ready.
**Isa Vell**, the shelter's tailor, can see it in the inventory: two coats for
eleven people, boots with soles going, and a laundry that soaps and rinses the
same water until it is grey. **Gil Ostrek** the tanner has hides but no lime.
**Amma Roth** the weaver has a loom frame and no yarn. The quartermaster,
**Dorn**, has a ration book that says clothes are consumables, and the children
have nothing that fits.

At the same time, a death in the outlying camp raises a question the shelter
has avoided: do the dead get their clothes back, or are they burned, buried, or
given? And a trader arrives with salvaged bolts of cloth and prices that make
the quartermaster wince.

The expansion's question: **how does a shelter keep its people warm, clean, and
recognizable without pretending cloth grows on trees?**

### 5.2 Theme (unspoken)

**Clothing is how a shelter says a person belongs to it.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_tailor_isa_vell` | Isa Vell | Tailor | Fit, layers, and dignity of dress |
| `npc_weaver_amma_roth` | Amma Roth | Weaver | Loom, yarn, and cloth production |
| `npc_tanner_gil_ostrek` | Gil Ostrek | Tanner | Hides, leather, and the yard |
| `npc_launderer_nesta` | Nesta | Launderer | Washing, drying, and hygiene |
| `npc_quartermaster_dorn` | Dorn | Quartermaster | Issue, stock, and winter planning |
| `npc_child_wren` | Wren | Child | Growing, hand-me-downs, and first boots |
| `npc_trader_sable` | Sable | Cloth trader | Bolt prices and market runs |
| `npc_cobbler_michal` | Michal | Cobbler | Boots, soles, and repair |

### 5.4 Story beats (15)

1. **The Count.** The wardrobe is inventoried; the numbers do not work.
2. **The First Cold.** Someone works a shift in wet boots; warmth drops.
3. **The Wash.** Laundry is organized; grey water becomes a hygiene problem.
4. **The Loom.** Amma's frame is warped; yarn is needed.
5. **The Fibre.** Flax and wool must be found, grown, or traded.
6. **The Lime.** The tannery needs lime; hides start to turn.
7. **The Fitting.** Isa fits the children first; the adults object.
8. **The Boots.** Soles are cut from salvage leather.
9. **The Issue.** Dorn writes the winter clothing plan.
10. **The Dead.** The shelter decides what happens to the clothes of the dead.
11. **The Dyes.** Color arrives for the first time in years.
12. **The Deep Cold.** The winter test; layers and insulation are counted.
13. **The Mending.** A mending week; every tear in the shelter gets attention.
14. **The Return.** Spring; the wardrobe is counted again.
15. **What We Wore Through It.** Final disposition of cloth, dress, and memory.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Issue policy | equal / need / role / age | fairness vs. fitness |
| The dead | keep / burn / give / archive | memory vs. hygiene |
| Fiber source | grow / trade / salvage / herd | independence vs. speed |
| Kids first | yes / adults first / mixed | future vs. present |
| Uniforms | formal / work-cloth / none | identity vs. equality |
| Dyes | practical / bright / none | morale vs. cost |
| Heirloom | keep / display / give / bury | memory |
| Final | clothed / enough / thin | system outcome |

### 5.6 Endings (5 + fade)

1. **The Full Wardrobe** — every survivor has layers, boots, and clean cloth; the
   winter costs nothing.
2. **The Mended Year** — everything is patched, nothing is new, and the shelter
   holds.
3. **The Thin Winter** — clothing runs short; warmth drops; the cold takes a
   price.
4. **The Cloth Road** — the shelter trades cloth and becomes a regional supplier.
5. **The Stored Thread** — the wardrobe is archived, labeled, and remembered.
6. **Fade** — the same coats continue; nothing gets worse for a while.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_thread_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_thread_the_count`, `quest_thread_first_cold`, `quest_thread_the_wash`,
`quest_thread_the_loom`, `quest_thread_the_fibre`, `quest_thread_the_lime`,
`quest_thread_the_fitting`, `quest_thread_the_boots`, `quest_thread_the_issue`,
`quest_thread_the_dead`, `quest_thread_the_dyes`, `quest_thread_deep_cold`,
`quest_thread_the_mending`, `quest_thread_the_return`,
`quest_thread_what_we_wore`.

### 6.2 Side quests (30)

**Warmth (5)**
- `quest_thread_layer_audit` — audit layers per survivor
- `quest_thread_bedding` — blankets for the dormitory
- `quest_thread_door_seal` — seal the shelter doors
- `quest_thread_window_cover` — covers for broken windows
- `quest_thread_wet_gear` — dry and rotate wet gear

**Laundry (5)**
- `quest_thread_soap_make` — make soap
- `quest_thread_wash_day` — organize washing
- `quest_thread_drying` — drying in cold weather
- `quest_thread_dirty_clothes` — clothes and disease risk
- `quest_thread_louse` — treat an infestation

**Fiber and cloth (5)**
- `quest_thread_flax_plant` — plant and ret flax
- `quest_thread_shearing` — shear and skirt wool
- `quest_thread_spinning` — spin yarn
- `quest_thread_warping` — warp the loom
- `quest_thread_first_bolt` — weave the first bolt

**Leather (5)**
- `quest_thread_hide_cure` — cure a hide
- `quest_thread_lime_run` — find lime
- `quest_thread_leather_repair` — repair straps and harness
- `quest_thread_boot_make` — make boots
- `quest_thread_tan_yard` — build the tan yard

**Mending (5)**
- `quest_thread_mend_week` — a mending week
- `quest_thread_patch_child` — patch a child's coat
- `quest_thread_heirloom` — an heirloom garment
- `quest_thread_hand_me_down` — pass clothes down
- `quest_thread_thread_kit` — thread and needle stock

**Identity (5)**
- `quest_thread_work_apron` — work clothing for roles
- `quest_thread_mourning_dress` — mourning dress
- `quest_thread_first_dye` — dye cloth
- `quest_thread_child_grow` — clothes for a growing child
- `quest_thread_uniform_talk` — debate uniforms

### 6.3 Repeatable quests (8)

`quest_thread_repeat_mend`, `quest_thread_repeat_wash`,
`quest_thread_repeat_issue`, `quest_thread_repeat_spin`,
`quest_thread_repeat_tan`, `quest_thread_repeat_trade`,
`quest_thread_repeat_audit`, `quest_thread_repeat_dye`.

### 6.4 Dynamic hooks

Live events (equipment wear, warmth drops, hygiene drops, atmosphere changes,
crafting completion, deaths, births, seasons) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Warmth and hygiene consequences route only through `NeedsSystem.Modify`.
- Radiation protection stays derived from `WornGear`.
- Disease exposure routes through `DiseaseSystem.TryExpose`.
- Garments are items in `Inventory`; no parallel wardrobe ledger.
- Cloth is crafted through `CraftingSystem`.
- No garment may be created without fiber, trade, or salvage.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `GarmentSystem` (new, `Ashfall.Core.Inventory`)

**Owns:** garment metadata resolution, layers, warmth contribution, fit,
coverage, and condition categories over existing item durability.
**Consumes:** `Inventory`, `EquippedItem`, `WornGear`, `NeedsSystem`.
**Data:** `garments.json`. **Rules:** a garment is an item with authored
metadata; warmth contribution is deterministic from layers and condition; no
second equipment store.

### 7.2 `LaundrySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** wash cycles, soap and water use, drying, contamination, and hygiene
effects. **Consumes:** `NeedsSystem.Hygiene`, `SanitationSystem` (water),
`FluidLogisticsSystem`, `ShelterAtmosphereSystem.Cleanliness`,
`DiseaseSystem.TryExpose`. **Data:** `laundry_programs.json`.
**Rules:** washing consumes real water and soap; wet clothes are a risk; dirty
clothes feed hygiene and disease exposure; no hidden hygiene bar.

### 7.3 `FiberProductionSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** fiber to yarn to cloth, retting, spinning, weaving, knitting, felting.
**Consumes:** `CraftingSystem`, `Inventory`, greenhouse/agriculture (flax),
herd animals (wool), `SkillProgressionSystem`. **Data:** `fibers.json`,
`textile_recipes.json`. **Rules:** every step consumes inputs and time; quality
is deterministic; byproducts are real.

### 7.4 `LeatherworkSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** tanning stages, leather grades, and leather goods. **Consumes:**
`CraftingSystem`, `Inventory`, hides from hunting/livestock, `TanningLeather`
lore. **Data:** `leather_grades.json`, `leather_recipes.json`.
**Rules:** tanning needs lime, brains, bark, or smoke; failure ruins hides;
grades matter for boots and harness.

### 7.5 `InsulationSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** bedding, curtains, door seals, window covers, and their contribution
to shelter thermal comfort. **Consumes:** `ShelterAtmosphereSystem`,
`Inventory`, `MasonrySystem`/construction ties. **Data:**
`insulation_upgrades.json`. **Rules:** insulation consumes cloth and labor; the
effect is visible in `ThermalComfort`; no direct warmth writes.

### 7.6 `UniformSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** role clothing, work dress, mourning dress, and identity tags.
**Consumes:** `GarmentSystem`, `JobBoard`/roles, `MemorialSystem`, `NeedsSystem`
morale. **Data:** `dress_codes.json`. **Rules:** identity effects are morale and
recognition only; no stat duplication; mourning dress is authored per culture.

### 7.7 `MendingSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** tear states, patches, reweaving, and hand-me-downs. **Consumes:**
`Inventory` durability, `CraftingSystem`, `GarmentSystem`. **Data:**
`mending_recipes.json`. **Rules:** mending restores partial condition; patches
carry visible history; a garment can be retired to rags.

### 7.8 Systems explicitly not added

- No second inventory, equipment, needs, atmosphere, radiation, or disease system.
- No fashion stat system.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `garments.json` (new)

```json
{
  "schema_version": 1,
  "garments": [
    {
      "garment_id": "garment_wool_coat",
      "display_name": "Wool Coat",
      "layer": "outer",
      "warmth": 18,
      "coverage": ["torso", "arms"],
      "bulk": 3,
      "durability": 120,
      "degrade_rate": 0.02,
      "repairable": true,
      "tags": ["winter", "heavy", "trade"]
    }
  ]
}
```

### 8.2 `laundry_programs.json` (new)

Program rows: water, soap, heat, hours, hygiene effect, contamination removal,
drying requirement, and failure mode.

### 8.3 `fibers.json` (new)

Fiber rows: source, yield, retting/processing, spinning difficulty, and cloth
quality bands.

### 8.4 `textile_recipes.json` (new)

Recipe rows: inputs, station, hours, skill, outputs, and byproducts.

### 8.5 `leather_grades.json` (new)

Grade rows: source hide, process, hours, grade bands, use cases, and failure.

### 8.6 `leather_recipes.json` (new)

Leather goods: boots, belts, straps, harness, gloves, jerkins.

### 8.7 `insulation_upgrades.json` (new)

Upgrade rows: inputs, hours, thermal contribution, decay, and dependencies.

### 8.8 `dress_codes.json` (new)

Dress rows: role, garment set, recognition, morale effect, and restrictions.

### 8.9 `mending_recipes.json` (new)

Mending rows: tear type, inputs, condition restored, and visible patch.

### 8.10 Items

New items appended to `items.json`: `item_linen_shirt`, `item_wool_tunic`,
`item_leather_jerkin`, `item_felt_hat`, `item_work_apron`, `item_leather_belt`,
`item_house_slippers`, `item_mourning_band`, `item_child_coat`,
`item_thread_spool`, `item_needle_set`, `item_soap_bar`, `item_dye_pouch`,
`item_shears`, `item_spindle`, `item_loom_shuttle`, `item_tanning_lime`,
`item_bark_tan`, `item_patch_cloth`, `item_rag_bundle`, `item_insulation_batt`,
`item_window_cover`, `item_bedding_straw`, `item_laundry_tub`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Garments are items and persist with inventory/equipment state. New sub-objects
(laundry jobs, fiber jobs, tanning jobs, insulation levels, dress tags, mending
queue) are additive inside existing shelter, crafting, and survivor stores. No
new save section.

### 9.2 State to persist

- Garment metadata on equipped and stored items.
- Laundry jobs and drying state.
- Fiber and tanning job progress.
- Insulation upgrades and decay.
- Dress tags and heirloom records.
- Mending queue and patch history.

### 9.3 Determinism

- Warmth contribution is a pure function of layers, condition, and coverage.
- Wear and mending outcomes are deterministic given rates and inputs.
- No wall-clock or unseeded randomness.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing equipment and needs untouched; garments without
metadata resolve to neutral values; no laundry, fiber, tanning, or insulation
state exists until started. `Warmth` is already present and remains 100 at
start.

### 9.5 Checksum

Invariant-culture floats; integer permille for condition and quality where the
schema allows.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `WardrobePanel` (new) | Issue, layers, condition | `WardrobeHostSession` |
| `LaundryPanel` (new) | Wash, dry, hygiene | same |
| `LoomPanel` (new) | Fiber, spin, weave | same |
| `TanneryPanel` (new) | Hides, grades, leather goods | same |
| `InsulationPanel` (new) | Bedding, seals, covers | same |
| `DressPanel` (new) | Work dress, mourning, identity | same |
| `InventoryPanel` (extend) | Garment metadata shown | existing |
| `NeedsPanel` (existing) | Warmth/hygiene status | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Warmth is shown as layers and condition, not a hidden number.
- Laundry shows water, soap, and heat costs before starting.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Color-coded condition never carries meaning alone; text labels required.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a needle, shears, a loom beat, a
wash tub, a drying line, a door seal. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `Inventory` / `WornGear` | Garment metadata, condition, protection |
| `NeedsSystem` | Sole warmth and hygiene consequence surface |
| `ShelterAtmosphereSystem` | Insulation and cleanliness |
| `CraftingSystem` | Fiber, leather, mending recipes |
| `SkillProgressionSystem` | Tailor, weaver, tanner skills |
| `SanitationSystem` / `FluidLogisticsSystem` | Laundry water |
| `DiseaseSystem` | Exposure from dirty clothes and louse |
| `RadiationSystem` | WornGear protection unchanged |
| `MemorialSystem` | Mourning dress and heirlooms |
| `TradingSystem` | Cloth and garment trade |
| `Agriculture` / herd | Flax and wool inputs |
| `EpilogueChronicleBuilder` | Wardrobe milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `Inventory.WornGear`, `NeedsSystem`
warmth/hygiene, `ShelterAtmosphereSystem` thermal/cleanliness, crafting textile
recipes, and clothing items. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author garments, laundry programs, fibers,
textile recipes, leather grades, leather recipes, insulation upgrades, dress
codes, mending recipes; append items. Register validators and scanner.

**Phase 2 — Pure Core.** `GarmentSystem`, `LaundrySystem`,
`FiberProductionSystem`, `LeatherworkSystem`, `InsulationSystem`,
`UniformSystem`, `MendingSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WardrobeHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 180/360-day soak through a full winter.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Garments | 40 |
| Laundry programs | 10 |
| Fibers | 12 |
| Textile recipes | 30 |
| Leather grades | 10 |
| Leather recipes | 20 |
| Insulation upgrades | 12 |
| Dress codes | 10 |
| Mending recipes | 15 |
| Items | 24 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second equipment system | Critical | Extend `Inventory`/`WornGear` |
| Direct warmth writes | Critical | Route through `NeedsSystem.Modify` |
| Hidden hygiene bar | High | Visible laundry and status |
| Cold death too harsh | High | Authored warnings and recovery |
| Fiber chain too grindy | Medium | Trade and salvage alternatives |
| Tone becomes fashion | Medium | Warmth first, identity light |
| Determinism break | Low | Pure functions only |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `garments.json` | 40 | 8,000 |
| `laundry_programs.json` | 10 | 2,500 |
| `fibers.json` | 12 | 3,000 |
| `textile_recipes.json` | 30 | 6,000 |
| `leather_grades.json` | 10 | 2,500 |
| `leather_recipes.json` | 20 | 4,000 |
| `insulation_upgrades.json` | 12 | 3,000 |
| `dress_codes.json` | 10 | 2,500 |
| `mending_recipes.json` | 15 | 3,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 24 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R27-1 | Second equipment system | Low | Critical | One inventory owner |
| R27-2 | Needs bypass | Low | Critical | `NeedsSystem.Modify` only |
| R27-3 | Hidden hygiene bar | Med | High | Visible laundry state |
| R27-4 | Cold death harsh | Med | High | Warnings and recovery |
| R27-5 | Fiber grind | Med | Med | Trade and salvage paths |
| R27-6 | Fashion drift | Med | Med | Warmth-first framing |
| R27-7 | Determinism | Low | High | Pure functions |
| R27-8 | Content overrun | Med | Med | Budget §13 |
| R27-9 | Louse horror | Low | Med | Practical, calm prose |
| R27-10 | Dead-clothes choice tasteless | Low | Med | Dignified options |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Are garments separate items or metadata on existing clothing items?**
   Recommended: metadata on items, resolved by `GarmentSystem`.
2. **Does dirty clothing always cause disease exposure?** Recommended: no;
   exposure scales with cleanliness and crowding.
3. **Can clothing be taken from the dead?** Recommended: yes, as one of several
   dignified options with hygiene consequences.
4. **Do uniforms grant recognition only?** Recommended: yes; no combat stats.
5. **Does insulation persist through shelter moves?** Recommended: no; it is
   shelter-bound.

---

## 17. APPENDIX D — GARMENT TABLE (40 GARMENTS)

| # | Garment | Layer | Warmth | Coverage | Bulk | Durability |
|---|---|---|---|---|---|---|
| 1 | Linen Shirt | base | 4 | torso, arms | 1 | 80 |
| 2 | Wool Undershirt | base | 7 | torso | 1 | 90 |
| 3 | Long Johns | base | 8 | legs | 1 | 90 |
| 4 | Wool Tunic | mid | 10 | torso, arms | 2 | 110 |
| 5 | Felt Vest | mid | 8 | torso | 2 | 100 |
| 6 | Knit Cardigan | mid | 9 | torso, arms | 2 | 100 |
| 7 | Wool Coat | outer | 18 | torso, arms | 3 | 120 |
| 8 | Canvas Coat | outer | 10 | torso, arms | 3 | 140 |
| 9 | Oilskin Cape | outer | 6 | torso | 2 | 130 |
| 10 | Ash Cloak | outer | 8 | torso | 2 | 100 |
| 11 | Leather Jerkin | outer | 9 | torso | 3 | 150 |
| 12 | Fur-Lined Coat | outer | 22 | torso, arms | 4 | 110 |
| 13 | Padded Overcoat | outer | 16 | torso, arms | 4 | 130 |
| 14 | Work Apron | over | 2 | torso | 1 | 70 |
| 15 | Smith's Leather Apron | over | 3 | torso | 2 | 120 |
| 16 | Wool Trousers | base | 7 | legs | 2 | 100 |
| 17 | Canvas Trousers | base | 4 | legs | 2 | 110 |
| 18 | Leather Trousers | base | 6 | legs | 3 | 130 |
| 19 | Padded Leggings | mid | 8 | legs | 2 | 100 |
| 20 | Wool Socks | base | 4 | feet | 1 | 60 |
| 21 | Felt Boots | footwear | 10 | feet, lower-leg | 2 | 90 |
| 22 | Leather Boots | footwear | 8 | feet | 2 | 130 |
| 23 | Insulated Boots | footwear | 14 | feet, lower-leg | 3 | 120 |
| 24 | Tan Work Boots | footwear | 7 | feet | 2 | 140 |
| 25 | House Slippers | footwear | 3 | feet | 1 | 50 |
| 26 | Felt Hat | head | 6 | head | 1 | 80 |
| 27 | Wool Cap | head | 5 | head | 1 | 70 |
| 28 | Hooded Scarf | head | 7 | head, neck | 1 | 60 |
| 29 | Work Gloves | hands | 3 | hands | 1 | 60 |
| 30 | Leather Gloves | hands | 5 | hands | 1 | 90 |
| 31 | Mittens | hands | 6 | hands | 1 | 70 |
| 32 | Child's Coat | outer | 12 | torso, arms | 2 | 80 |
| 33 | Child's Scarf | head | 5 | neck | 1 | 50 |
| 34 | Child's Boots | footwear | 7 | feet | 1 | 70 |
| 35 | Mourning Band | over | 1 | arm | 1 | 40 |
| 36 | Mourning Shawl | outer | 8 | torso | 2 | 70 |
| 37 | Heirloom Coat | outer | 16 | torso, arms | 3 | 90 |
| 38 | Bed Shirt | base | 5 | torso | 1 | 70 |
| 39 | Rain Hood | head | 4 | head | 1 | 60 |
| 40 | Sun Cloth | over | 1 | head, neck | 1 | 40 |

Garments are items with metadata resolved by `GarmentSystem`. Warmth is a
contribution, not a stat; total warmth is a deterministic function of layers,
coverage, condition, and bulk.

---

## 18. APPENDIX E — LAUNDRY PROGRAM TABLE (10 PROGRAMS)

| # | Program | Water | Soap | Heat | Hours | Hygiene | Contamination |
|---|---|---|---|---|---|---|---|
| 1 | Cold Rinse | 8 | 0 | none | 2 | +2 | none |
| 2 | Warm Wash | 12 | 1 | low | 3 | +6 | −20% |
| 3 | Boil Wash | 16 | 2 | high | 5 | +12 | −70% |
| 4 | Louse Treatment | 14 | 2 | high | 6 | +14 | −90% |
| 5 | Grease Scrub | 10 | 3 | warm | 4 | +8 | −40% |
| 6 | Bedding Wash | 20 | 3 | high | 8 | +10 | −60% |
| 7 | Leather Wipe | 4 | 1 | none | 1 | +3 | −10% |
| 8 | Wool Care | 9 | 1 | low | 4 | +5 | −15% |
| 9 | Smoke Cleanse | 6 | 0 | smoke | 6 | +6 | −50% |
| 10 | Ash Scrub | 11 | 2 | warm | 4 | +7 | −35% |

Laundry consumes real water and soap, and wet clothes are a real risk in cold
weather. `NeedsSystem.Hygiene` receives the effect; `ShelterAtmosphereSystem`
sees the cleanliness term; `DiseaseSystem.TryExpose` sees the louse path.

---

## 19. APPENDIX F — FIBER TABLE (12 FIBERS)

| # | Fiber | Source | Yield | Processing | Spin skill | Cloth grade |
|---|---|---|---|---|---|---|
| 1 | Flax | fiber field | 0.4 | ret, break, scutch | 2 | fine |
| 2 | Hemp | fiber field | 0.5 | ret, break | 3 | coarse |
| 3 | Wool | herd animals | 0.6 | skirt, wash, card | 1 | warm |
| 4 | Mohair | goats | 0.3 | comb | 3 | luxury |
| 5 | Cotton | trade only | n/a | gin, card | 2 | fine |
| 6 | Nettle | foraging | 0.2 | ret, beat | 2 | coarse |
| 7 | Bark Cloth | trees | 0.3 | beat, dry | 2 | crude |
| 8 | Salvage Cloth | ruins | 0.8 | sort, wash | 1 | mixed |
| 9 | Rag Fiber | rag market | 0.4 | shred, card | 2 | shoddy |
| 10 | Polymer Fiber | salvage | 1.0 | melt-draw | 4 | durable |
| 11 | Hide | hunting, herd | 1.0 | tan | 2 | leather |
| 12 | Felt Waste | workshops | 0.3 | card, press | 2 | pressed |

Every fiber has a real source and a real cost. The shelter's first independence
comes from flax and wool; polymer fiber is a salvage luxury.

---

## 20. APPENDIX G — TEXTILE RECIPE TABLE (30 RECIPES)

| # | Recipe | Inputs | Station | Hours | Output |
|---|---|---|---|---|---|
| 1 | Ret Flax | flax straw, water | retting pit | 72 | retted flax |
| 2 | Break and Scutch | retted flax | break | 6 | flax fiber |
| 3 | Card Wool | washed wool | cards | 3 | roving |
| 4 | Spin Yarn | fiber | spindle | 8 | yarn |
| 5 | Ply Yarn | yarn ×2 | spindle | 3 | plied yarn |
| 6 | Warp Loom | plied yarn | loom | 10 | warp |
| 7 | Weave Plain | warp, weft | loom | 16 | plain cloth |
| 8 | Weave Twill | warp, weft | loom | 20 | twill cloth |
| 9 | Knit Socks | yarn | needles | 6 | socks |
| 10 | Knit Cardigan | yarn | needles | 14 | cardigan |
| 11 | Felt Cloth | wool, water, heat | felt works | 12 | felt |
| 12 | Dye Cloth | cloth, dye, mordant | dye vat | 8 | dyed cloth |
| 13 | Sew Shirt | cloth, thread | bench | 5 | shirt |
| 14 | Sew Trousers | cloth, thread | bench | 6 | trousers |
| 15 | Sew Coat | cloth, thread | bench | 12 | coat |
| 16 | Sew Overcoat | cloth, thread, lining | bench | 18 | overcoat |
| 17 | Sew Apron | cloth, thread | bench | 3 | apron |
| 18 | Make Felt Boots | felt, sole leather | cobbler | 10 | felt boots |
| 19 | Make Leather Boots | leather, thread | cobbler | 16 | boots |
| 20 | Make Mittens | yarn or felt | bench | 4 | mittens |
| 21 | Make Hat | felt | bench | 5 | hat |
| 22 | Make Scarf | yarn | needles | 4 | scarf |
| 23 | Patch Garment | cloth patch, thread | bench | 1 | patched garment |
| 24 | Reweave Tear | yarn, thread | loom | 4 | rewoven cloth |
| 25 | Turn Collar | thread | bench | 2 | renewed garment |
| 26 | Make Bedding | straw, cloth | bench | 6 | bedding |
| 27 | Make Curtain | cloth | bench | 4 | curtain |
| 28 | Make Door Seal | cloth, felt | bench | 3 | door seal |
| 29 | Make Window Cover | cloth, batt | bench | 4 | window cover |
| 30 | Rag Bundle | worn garment | bench | 1 | rags, salvage fiber |

All recipes route through `CraftingSystem`; stations are rooms with capacity and
staffing, consistent with the live crafting model.

---

## 21. APPENDIX H — LEATHER GRADE TABLE (10 GRADES)

| # | Grade | Source | Process | Hours | Use | Failure |
|---|---|---|---|---|---|---|
| 1 | Raw Hide | any | none | 0 | temporary | rot |
| 2 | Lime-Soaked | hide, lime | soak | 48 | prep | burn |
| 3 | Scraped Hide | soaked | beam | 6 | prep | tear |
| 4 | Brain-Tanned | scraped | dress | 12 | soft leather | stiffen |
| 5 | Bark-Tanned | scraped, bark | pit | 120 | durable | uneven |
| 6 | Smoke-Tanned | scraped, smoke | smoke | 36 | flexible | soot |
| 7 | Alum-Tanned | scraped, alum | soak | 72 | pale leather | weak |
| 8 | Harness Leather | bark-tanned | currier | 20 | straps | crack |
| 9 | Sole Bend | bark-tanned | press | 24 | boot soles | split |
| 10 | Parchment | scraped, lime | dry-stretch | 30 | records | brittle |

The live leather narrative catalogs describe exactly this craft; the expansion
gives it stages, failure, and grades that matter to boots, harness, and armor
straps without becoming a second foundry.

---

## 22. APPENDIX I — LEATHER RECIPE TABLE (20 GOODS)

| # | Good | Inputs | Hours | Use |
|---|---|---|---|---|
| 1 | Leather Belt | strap leather | 3 | trousers, kit |
| 2 | Boot Soles | sole bend | 6 | boot repair |
| 3 | Leather Boots | leather, thread | 16 | footwear |
| 4 | Work Gloves | soft leather | 4 | hands |
| 5 | Leather Apron | leather | 8 | foundry, forge |
| 6 | Jerkin | leather | 12 | torso |
| 7 | Harness Strap | harness leather | 3 | carts, animals |
| 8 | Pack Saddle | harness, padding | 10 | hauling |
| 9 | Waterskin | leather, pitch | 6 | water carry |
| 10 | Leather Pouch | soft leather | 2 | storage |
| 11 | Tool Sheath | leather | 2 | tools |
| 12 | Scabbard Wrap | leather | 3 | blades |
| 13 | Leather Cap | leather | 4 | head |
| 14 | Knee Guard | leather | 5 | crawling |
| 15 | Gaiters | leather | 6 | legs |
| 16 | Leather Cloak | leather, lining | 14 | outer |
| 17 | Book Covers | parchment, leather | 6 | records |
| 18 | Drum Skin | hide | 5 | music |
| 19 | Pump Gasket | leather | 3 | machinery |
| 20 | Belting | harness leather | 8 | machines |

Leather goods tie the wardrobe to the rest of the shelter: gaskets and belting
are machinery, not fashion, and the tan yard matters because of them.

---

## 23. APPENDIX J — INSULATION UPGRADE TABLE (12 UPGRADES)

| # | Upgrade | Inputs | Hours | Thermal | Decay |
|---|---|---|---|---|---|
| 1 | Straw Bedding | straw, cloth | 6 | +2 | seasonal |
| 2 | Wool Blankets | wool, loom | 14 | +3 | slow |
| 3 | Door Seals | felt, leather | 3 | +2 | winter |
| 4 | Window Covers | cloth, batt | 4 | +3 | winter |
| 5 | Curtains | cloth | 4 | +1 | slow |
| 6 | Loft Drying | racks, cloth | 5 | +1 | none |
| 7 | Bunk Covers | cloth, batt | 6 | +2 | winter |
| 8 | Entry Windbreak | wood, cloth | 8 | +2 | storm |
| 9 | Floor Rugs | rag, felt | 6 | +2 | slow |
| 10 | Wall Hangings | cloth | 8 | +1 | slow |
| 11 | Ceiling Baffles | cloth, batt | 10 | +2 | slow |
| 12 | Sleeping Pods | cloth, batt, frame | 16 | +3 | slow |

Insulation upgrades raise `ShelterAtmosphereSystem.ThermalComfort` through the
live atmosphere path. They never write survivor warmth directly; the atmosphere
modifier is the only bridge, which keeps one authority per concern.

---

## 24. APPENDIX K — DRESS CODE TABLE (10 CODES)

| # | Code | Role | Garments | Recognition | Morale |
|---|---|---|---|---|---|
| 1 | Work Cloth | general | apron, trousers | low | +1 |
| 2 | Smith's Dress | foundry | leather apron, boots | high | +1 |
| 3 | Grower's Dress | agriculture | apron, sun cloth | med | +1 |
| 4 | Medic's Band | medical | armband, clean cloth | high | +1 |
| 5 | Guard's Kit | security | cloak, belt | high | +1 |
| 6 | Teacher's Sash | education | sash, tunic | med | +2 |
| 7 | Mourning Band | bereaved | band, shawl | high | +2 |
| 8 | Child's Cloth | children | child coat, scarf | low | +2 |
| 9 | Feast Dress | all | best garments | low | +3 |
| 10 | Heirloom Mark | family | marked garment | personal | +3 |

Dress codes are recognition and morale only. They never grant combat, research,
or production bonuses; their function is to let a shelter see itself.

---

## 25. APPENDIX L — MENDING RECIPE TABLE (15 RECIPES)

| # | Recipe | Tear | Inputs | Condition | Visible |
|---|---|---|---|---|---|
| 1 | Small Patch | hole | patch cloth | +15 | patch |
| 2 | Large Patch | tear | patch cloth | +20 | patch |
| 3 | Seam Repair | split seam | thread | +10 | seam |
| 4 | Button Fix | missing button | button, thread | +5 | button |
| 5 | Hem Repair | frayed hem | thread | +8 | hem |
| 6 | Elbow Patch | worn elbow | patch cloth | +12 | patch |
| 7 | Knee Patch | worn knee | patch cloth | +12 | patch |
| 8 | Reweave | thin cloth | yarn | +18 | weave |
| 9 | Sole Stitch | boot split | thread, wax | +15 | stitching |
| 10 | Resole | worn sole | sole bend | +40 | new sole |
| 11 | Zip Cord | broken closure | cord, toggle | +10 | closure |
| 12 | Reinforce | stress point | cloth, thread | +10 | layer |
| 13 | Turn Collar | worn collar | thread | +12 | reversed |
| 14 | Line Lining | worn lining | lining cloth | +20 | lining |
| 15 | Retire to Rags | destroyed | none | −100 | none |

Mending is the expansion's quiet hero: a shelter that mends survives the winter
it could not re-clothe, and every patch is a visible record of care.

---

## 26. APPENDIX M — STORYLINE CHAPTER BREAKDOWN

| Beat | Title | Location | Combat | Key decision |
|---|---|---|---|---|
| 1 | The Count | wardrobe store | none | inventory policy |
| 2 | The First Cold | workshop | none | issue or wait |
| 3 | The Wash | laundry | none | water budget |
| 4 | The Loom | loom hall | none | yarn source |
| 5 | The Fibre | fiber field | light | grow or trade |
| 6 | The Lime | tannery | none | lime budget |
| 7 | The Fitting | sewing room | none | kids first |
| 8 | The Boots | cobbler | none | leather allocation |
| 9 | The Issue | quartermaster | none | policy choice |
| 10 | The Dead | morgue | none | dignified options |
| 11 | The Dyes | dye walk | light | morale vs. cost |
| 12 | The Deep Cold | shelter | none | layer management |
| 13 | The Mending | sewing room | none | mend week |
| 14 | The Return | wardrobe store | none | counting |
| 15 | What We Wore | common room | none | final disposition |

The questline is deliberately low-combat: its tension is arithmetic, weather,
and care.

---

## 27. APPENDIX N — NPC DOSSIERS (BRIEF)

**Isa Vell** — tailor. Fits children first because they grow faster than cloth
arrives. Keeps a notebook of every person's measurements and pretends it is
ordinary paperwork.

**Amma Roth** — weaver. Warped the first loom from a salvaged frame and talks to
it when the shed sticks. Believes a shelter without cloth has no memory.

**Gil Ostrek** — tanner. Knows that lime is the difference between a hide and a
smell. Runs the yard with a calm that hides the stakes.

**Nesta** — launderer. Runs the wash with a sergeant's discipline, because wet
clothes in winter are a casualty waiting to happen.

**Dorn** — quartermaster. Writes the winter clothing plan in pencil because it
changes weekly. Hates the phrase "we'll manage."

**Wren** — child. Outgrows everything. Treats hand-me-downs as inheritance and
the first pair of boots as a promotion.

**Sable** — cloth trader. Sells bolts from a cart and prices according to the
weather. Knows the shelter will remember who sold fairly.

**Michal** — cobbler. Resoles boots no one else can save and considers a good
welt a moral achievement.

---

## 28. APPENDIX O — LOCATION DETAIL

- **The Fiber Field** — flax and hemp; retting pits smell and depend on water.
- **The Shearing Pen** — herd animals; wool by season, grease and skirtings.
- **The Tan Yard** — hides, lime, bark, smoke; ugly work with clean results.
- **The Mill Ruins** — looms and spindles; salvage with a floor that may not hold.
- **The Dye Walk** — plant dyes and mordants; color is morale with a price.
- **The Rag Market** — salvage cloth, thread, needles; five sellers and one scale.
- **The Cold Hollow** — cold pocket used to test garments before winter.
- **The Felt Works** — wool, water, heat, and pressure; blankets start here.
- **The Bootmaker** — lasts, awls, and a wall of sizes.
- **The Dyers' Stream** — water for retting and dye; downstream complaints start here.

---

## 29. APPENDIX P — WARMTH MODEL

| Layer count | Effective warmth | Effect |
|---|---|---|
| 0 | 0 | critical exposure |
| 1 | base only | cold risk outdoors |
| 2 | base + mid | survivable outdoors |
| 3 | base + mid + outer | comfortable outdoors |
| 4+ | layered heavy | warm, bulky, tiring |

| Condition | Multiplier |
|---|---|
| Pristine | 1.0 |
| Good | 0.9 |
| Worn | 0.75 |
| Ragged | 0.5 |
| Destroyed | 0.0 |

Warmth is computed, never stored as a hidden stat. The shelter sees layers and
condition, and the cold sees the result.

---

## 30. APPENDIX Q — WORKED WINTER SCENARIO

**Day 0.** Eleven survivors, two coats, three blankets. Thermal comfort 65.

**Day 20.** Flax retted; lime found; first wash day. Hygiene rises. Two survivors
still lack boots.

**Day 45.** Loom warped; first bolt woven; wool socks knitted. Insulation: door
seals and window covers installed. Thermal comfort 72.

**Day 70.** First hard frost. Outdoor shifts cut to short rotations. No warmth
critical flags. Quartermaster's plan is mostly met.

**Day 90.** Deep cold. One survivor's boots fail; mending week catches it. Dirty
laundry accumulates; louse treatment run; exposure avoided.

**Day 110.** Hides tanned, boots made for two. Blankets and bedding in place.
Thermal comfort 78. Warmth stable even for those on watch.

**Day 140.** Thaw. Wardrobe counted: four coats, eleven sets of socks, no cold
casualties. The shelter begins its first spring with cloth of its own making.

---

## 31. APPENDIX R — VIGNETTE (TONE SAMPLE)

> Nesta boils the wash until the room fogs, and when the fog clears she hangs the
> clothes in the loft, and the loft is the warmest place in the shelter because
> clothes drying overhead keep the ceiling honest.

> Gil spreads the hide on the beam and scrapes it with slow strokes, and the yard
> smells like work, and he does not mind that no one visits the tan yard.

> Isa measures Wren's arm with a knotted string and writes the number in the
> notebook, and says nothing about how much taller the number is than last
> winter.

---

## 32. APPENDIX S — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Warmth critical | health loss | layers, shelter, rest |
| Wet clothes | warmth drop, illness | dry, change, warmth |
| Dirty clothes | hygiene, exposure | wash, louse treatment |
| Boot failure | injury, mobility | resole, reassign |
| Hide ruined | material loss | new hide, teach process |
| Looms idle | no cloth | fix, trade, salvage |
| Insulation decay | comfort loss | repair, replace |
| Rag shortages | no mending | salvage, trade |
| Dye failure | morale only | retry, accept plain |
| Dead's clothes dispute | trust, grief | hearing, policy |

No failure is a game over. Every failure has a recovery path, and the deepest
failure is a shelter that lets its people go cold while cloth sits unused.

---

## 33. APPENDIX T — CONTENT REVIEW CHECKLIST

- [ ] `Inventory`/`WornGear` remains the equipment authority.
- [ ] Warmth and hygiene consequences route through `NeedsSystem.Modify`.
- [ ] Shelter comfort stays in `ShelterAtmosphereSystem`.
- [ ] Radiation protection still derives from `WornGear`.
- [ ] Disease exposure routes through `DiseaseSystem.TryExpose`.
- [ ] No hidden wardrobe or cleanliness stat exists.
- [ ] Laundry shows real water, soap, and heat costs.
- [ ] Fiber chain has trade and salvage alternatives.
- [ ] Mourning and dead's-clothes content is dignified.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses pure functions only.

---

## 34. APPENDIX U — GLOSSARY

- **Garment** — an item with layer, warmth, coverage, and condition metadata.
- **Layer** — base, mid, outer, footwear, head, hands.
- **Warmth** — computed contribution, never stored as a hidden stat.
- **Condition** — pristine, good, worn, ragged, destroyed.
- **Mending** — restoring partial condition with visible patches.
- **Retting** — soaking flax to separate fiber from stalk.
- **Roving** — carded fiber ready to spin.
- **Bolt** — a length of woven cloth.
- **Felt** — pressed wool cloth.
- **Grade** — leather quality from raw hide to sole bend.

---

## 35. APPENDIX V — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `Inventory` | garments | durability, condition | needs |
| `GarmentSystem` | items, layers | warmth contribution | inventory |
| `LaundrySystem` | water, soap | laundry jobs | hygiene |
| `FiberProductionSystem` | fiber | yarn, cloth | inventory |
| `LeatherworkSystem` | hides | leather, goods | inventory |
| `InsulationSystem` | cloth | thermal contribution | warmth |
| `UniformSystem` | garments, roles | dress tags | stats |
| `MendingSystem` | garments | condition restored | inventory |
| `NeedsSystem` | contributions | warmth, hygiene | — |
| `ShelterAtmosphereSystem` | insulation | thermal comfort | needs |
| `CraftingSystem` | recipes | outputs | — |
| `DiseaseSystem` | dirty clothes | exposure | — |
| `RadiationSystem` | worn gear | protection | — |
| `MemorialSystem` | mourning | records | — |
| `TradingSystem` | cloth | trade | — |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 36. APPENDIX W — DATA SCHEMA DETAIL (NEW CATALOGS)

**`garments.json`** — `garment_id`, `display_name`, `layer`, `warmth`,
`coverage[]`, `bulk`, `durability`, `degrade_rate`, `repairable`,
`rad_protection`, `tags`.

**`laundry_programs.json`** — `program_id`, `display_name`, `water`, `soap`,
`heat`, `hours`, `hygiene_effect`, `contamination_removal`, `drying_hours`,
`failure_mode`, `tags`.

**`fibers.json`** — `fiber_id`, `display_name`, `source`, `yield`,
`processing[]`, `spin_skill`, `cloth_grade`, `tags`.

**`textile_recipes.json`** — `recipe_id`, `inputs[]`, `station`, `hours`,
`skill`, `outputs[]`, `byproducts[]`, `tags`.

**`leather_grades.json`** — `grade_id`, `display_name`, `source_hide`,
`process`, `hours`, `quality_bands[]`, `use_cases[]`, `failure`, `tags`.

**`leather_recipes.json`** — `recipe_id`, `inputs[]`, `hours`, `outputs[]`,
`use`, `tags`.

**`insulation_upgrades.json`** — `upgrade_id`, `display_name`, `inputs[]`,
`hours`, `thermal`, `decay`, `dependencies[]`, `tags`.

**`dress_codes.json`** — `code_id`, `display_name`, `role`, `garments[]`,
`recognition`, `morale_effect`, `restrictions[]`, `tags`.

**`mending_recipes.json`** — `recipe_id`, `tear_type`, `inputs[]`,
`condition_restored`, `visible_effect`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid item/recipe references, or out-of-range numbers.

---

## 37. APPENDIX X — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Average effective warmth | winter readiness | GarmentSystem |
| Warmth critical events | cold risk | NeedsSystem |
| Laundry backlog | hygiene risk | LaundrySystem |
| Louse exposures | disease path | DiseaseSystem |
| Cloth produced | independence | FiberProductionSystem |
| Leather produced | footwear | LeatherworkSystem |
| Insulation level | shelter comfort | InsulationSystem |
| Mending throughput | equipment care | MendingSystem |
| Garment condition | stock health | Inventory |
| Trade cloth volume | regional role | TradingSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 38. APPENDIX Y — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Warmth and hygiene consequences route only through `NeedsSystem.Modify`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §33.
- [ ] Phase 7 soak survives a full winter without hidden warmth writes.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel inventory, needs, atmosphere, or radiation system exists.

---

## 39. APPENDIX Z — OPEN QUESTIONS FOR REVIEW

1. Should wet clothes reduce warmth immediately or after a delay?
2. Should laundry be a per-survivor task or a shelter-level rotation?
3. Should uniforms ever grant recognition mechanics (e.g., faster triage)?
4. Should the dead's clothes have a contamination cost when kept?
5. Should heirlooms persist across generations into Expansion 12's cohort system?
6. Should polymer fiber production require the foundry expansion?
7. Should insulation decay every winter or only on storm events?
8. Should mending be a skill with quality bands or a flat recipe?

None of these may be decided unilaterally; each changes balance and tone.

---

## 40. APPENDIX AA — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children's clothing, inheritance, growth |
| 1 | 13 The Faithful | Ritual dress, mourning, feast garments |
| 1 | 14 Above the Ash | Flight suits, cold-weather gear, parachute cloth |
| 1 | 15 The Deep Root | Flax, wool from companion species, fiber crops |
| 1 | 16 The Rebuilt Body | Adaptive clothing, prosthetics fitting, pressure wear |
| 2 | 17 The Long Evening | Feast dress, performance costume |
| 2 | 18 The Underneath | Cold underground clothing, damp protection |
| 2 | 19 The Bitter Air | Protective suits, decontamination of cloth |
| 2 | 20 The Quiet Hand | Disguise, courier clothing, unmarked garments |
| 2 | 21 The Grid | Insulation and heating interface |
| 3 | 22 The Clean Flow | Laundry water, soap, hygiene |
| 3 | 23 The Alarm | Fire-safe clothing, rescue gear |
| 3 | 24 The Long Goodbye | Last dress, mourning, heirlooms |
| 3 | 25 The Iron Road | Freight, cloth trade, rail towns |
| 4 | 28 The Lesson | School clothes, uniforms for apprentices |
| 4 | 29 The Glass | Spectacles, lenses, optical safety |
| 4 | 30 The Press | Printed patterns, manuals, catalogs |
| 4 | 31 The Kiln | Loom weights, kiln-fired buttons, tiles |

Each hook is additive. The Thread can ship alone, and every other expansion can
ship without it.

---

## 41. APPENDIX AB — CLOSING VIGNETTE

> The mending week starts with a basket of torn clothes and ends with the same
> basket, lighter, and the pile on the bench is stacked by size, and the smallest
> stack is Wren's.

> In the loom hall the shuttle goes back and forth, and the cloth grows by a
> finger's width every minute, and Amma does not stop for the bell because the
> warp is not finished and the warp does not care about bells.

> Outside, the first snow lands on the drying lines and melts on the clothes that
> were hung too late, and Nesta swears quietly and brings them in, and the shelter
> smells like wet wool and soap, which is the smell of a winter that was
> prepared for.

---

## 43. APPENDIX AC — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_thread_the_count` | 3 | Count garments; compare to people |
| `quest_thread_first_cold` | 4 | Observe warmth drop; issue layers |
| `quest_thread_the_wash` | 4 | Organize wash; budget water |
| `quest_thread_the_loom` | 5 | Repair frame; source yarn |
| `quest_thread_the_fibre` | 5 | Ret, break, and card fiber |
| `quest_thread_the_lime` | 4 | Find or burn lime; start tanning |
| `quest_thread_the_fitting` | 4 | Measure; fit children first |
| `quest_thread_the_boots` | 4 | Cut soles; make boots |
| `quest_thread_the_issue` | 5 | Write and defend winter plan |
| `quest_thread_the_dead` | 5 | Decide fate of the dead's clothes |
| `quest_thread_the_dyes` | 4 | Gather dye; dye first cloth |
| `quest_thread_deep_cold` | 6 | Manage layers and insulation through the freeze |
| `quest_thread_the_mending` | 5 | Mend every tear in the shelter |
| `quest_thread_the_return` | 4 | Count the wardrobe in spring |
| `quest_thread_what_we_wore` | 3 | Final disposition; epilogue |

---

## 44. APPENDIX AD — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_thread_layer_audit` | 3 | List, assess, plan |
| `quest_thread_bedding` | 4 | Gather, stuff, distribute |
| `quest_thread_door_seal` | 3 | Cut, fit, test |
| `quest_thread_window_cover` | 3 | Measure, sew, hang |
| `quest_thread_wet_gear` | 3 | Collect, dry, rotate |
| `quest_thread_soap_make` | 4 | Ash, fat, boil, cure |
| `quest_thread_wash_day` | 3 | Sort, wash, hang |
| `quest_thread_drying` | 3 | Loft, lines, rotation |
| `quest_thread_dirty_clothes` | 3 | Assess, wash, verify |
| `quest_thread_louse` | 4 | Inspect, treat, recheck |
| `quest_thread_flax_plant` | 4 | Sow, tend, harvest, ret |
| `quest_thread_shearing` | 3 | Gather, skirt, wash |
| `quest_thread_spinning` | 4 | Card, spin, ply |
| `quest_thread_warping` | 4 | Measure, thread, tension |
| `quest_thread_first_bolt` | 4 | Weave, finish, count |
| `quest_thread_hide_cure` | 4 | Soak, scrape, dress |
| `quest_thread_lime_run` | 4 | Locate, haul, burn |
| `quest_thread_leather_repair` | 3 | Assess, cut, stitch |
| `quest_thread_boot_make` | 4 | Last, cut, sew, fit |
| `quest_thread_tan_yard` | 4 | Site, build, stock |
| `quest_thread_mend_week` | 3 | Sort, mend, return |
| `quest_thread_patch_child` | 3 | Choose, patch, reinforce |
| `quest_thread_heirloom` | 4 | Identify, repair, record |
| `quest_thread_hand_me_down` | 3 | Sort, fit, pass |
| `quest_thread_thread_kit` | 3 | Needles, thread, case |
| `quest_thread_work_apron` | 3 | Pattern, sew, issue |
| `quest_thread_mourning_dress` | 4 | Choose, make, wear |
| `quest_thread_first_dye` | 4 | Gather, mordant, dye |
| `quest_thread_child_grow` | 3 | Measure, extend, remake |
| `quest_thread_uniform_talk` | 4 | Debate, vote, adopt |

---

## 45. APPENDIX AE — SOAP AND HYGIENE MODEL

| Input | Source | Cost | Output |
|---|---|---|---|
| Wood ash | stoves | free | lye |
| Fat | kitchen, tallow | food-adjacent | soap base |
| Lye water | ash + water | labor | saponify |
| Soap bar | cured mix | 2 days | cleaning |
| Soft soap | fast mix | 1 day | laundry |

The shelter already tracks hygiene and sanitation; soap is the missing input.
The expansion gives soap a supply chain and a cost, which is what turns laundry
from a button into a decision.

---

## 46. APPENDIX AF — DYE TABLE (10 DYES)

| # | Dye | Source | Mordant | Color | Cost |
|---|---|---|---|---|---|
| 1 | Madder | roots | alum | red | med |
| 2 | Woad | leaves | none | blue | high |
| 3 | Onion Skin | kitchen | none | amber | low |
| 4 | Walnut Hull | trees | none | brown | low |
| 5 | Charcoal | fire | none | grey | none |
| 6 | Lichen | stones | ammonia | purple | high |
| 7 | Moss | forest | none | green | med |
| 8 | Rust Iron | scrap | vinegar | black | low |
| 9 | Birch Bark | trees | none | tan | low |
| 10 | Indigo Substitute | trade | lye | blue | high |

Dye is morale with a cost. A shelter that dyes its cloth has decided it intends
to keep living, not only to survive.

---

## 47. APPENDIX AG — LOUSE AND DISEASE PATH

| Stage | Condition | Effect | Response |
|---|---|---|---|
| Clean | hygiene > 70 | none | maintain |
| Warning | hygiene 40–70 | itching, morale | wash soon |
| Infected | hygiene < 40 | louse | boil wash |
| Outbreak | crowded + dirty | exposure | treat, isolate |
| Cleared | treatment | none | monitor |

The expansion never invents a second disease system: the louse path routes
through `DiseaseSystem.TryExpose` exactly like every other exposure source, and
hygiene remains `NeedsSystem` property.

---

## 48. APPENDIX AH — REGIONAL WARDROBE MAP

| Settlement | Strength | Need | Trade |
|---|---|---|---|
| The shelter | mending skill | cloth, boots | repairs for goods |
| Market Town | bolts | leather | sells cloth |
| Fog Ridge Camp | hides, fur | thread | sells hides |
| Spring Village | flax | wool | sells fiber |
| Foundry Enclave | buckles, tools | cloth | trades hardware |
| Deep Bunker | stores | dye, soap | trades clothes |
| River Flotilla | salvage cloth | boots | moves goods |
| Coal Stage | fuel | warm clothing | barters heat |

No settlement can clothe itself alone. That is why the shelter's first customer
for mending is its first supplier of cloth.

---

## 49. APPENDIX AI — LORE: THE TEXTILE TRADITION

The fiction:

- **The Mill Ruins** was a regional weaving mill; its looms are the shelter's
  inheritance and its floor is dangerous.
- **The Tan Yard** follows a pre-war trade pattern; the smell kept it at the
  settlement's edge long before the Exchange.
- **The Dye Walk** was a dyers' row along a stream; the colors are remembered
  even where the recipes are not.
- **The Rag Market** is a barter custom older than the coalition: cloth changes
  hands, and every seller knows who mends well.
- **The Felt Works** pressed wool for blankets and hats before the war and
  presses them now.

No real brand, regiment, or fashion house is copied. The tradition is generic
and community-owned.

---

## 50. APPENDIX AJ — WORKED TEXTILE ECONOMY

| Season | Fiber | Cloth | Garments | Mending | Stock |
|---|---|---|---|---|---|
| Spring | plant, shear | low | low | high | draw down |
| Summer | harvest, ret | rising | moderate | moderate | build |
| Autumn | spin, weave | peak | peak | low | growing |
| Winter | none | none | none | peak | consumed |

A shelter that mends through winter and weaves through autumn never has to
choose between warmth and work. A shelter that starts in winter pays a price.

---

## 51. APPENDIX AK — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is warmth honest? | lifecycle + a11y tests |
| Tone | Is cold dignified? | content review |
| Balance | Is the winter tense and fair? | long soak |

The wardrobe is where players feel the shelter's care or its neglect. The tone
gate matters as much as the balance gate.

---

## 52. APPENDIX AL — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Hidden warmth stat | removes agency | layers and condition visible |
| Clothing as pure loot | removes craft | fiber and mending chains |
| Laundry as busywork | wastes attention | real costs, real effects |
| Cold as sudden death | unfair | warnings and withdrawal |
| Fashion as stat system | tone break | recognition and morale only |
| Dead's clothes as free loot | tasteless | dignified choice with cost |
| Fiber as pure grind | boring | trade and salvage paths |
| Dye as mandatory | punishes | optional morale only |
| Louse as horror | tone break | practical, calm handling |
| Uniform as military fantasy | tone break | work dress and recognition |

The list exists because the wardrobe is easy to get subtly wrong: it can become
a second needs system, a loot treadmill, or a tone problem. The live authority
splits exist to prevent exactly that.

---

## 53. APPENDIX AM — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Garments | 40 | 8,000 |
| Laundry programs | 10 | 2,500 |
| Fibers | 12 | 3,000 |
| Textile recipes | 30 | 6,000 |
| Leather grades | 10 | 2,500 |
| Leather recipes | 20 | 4,000 |
| Insulation upgrades | 12 | 3,000 |
| Dress codes | 10 | 2,500 |
| Mending recipes | 15 | 3,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 24 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,000** |

---

## 54. APPENDIX AN — ENDING PROSE SKETCHES

**The Full Wardrobe.** Every survivor has layers, boots, and two changes of clean
cloth, and the shelter's first winter of cloth ends with no one counting
blankets.

**The Mended Year.** Nothing is new. Everything is patched, reworked, turned,
and handed down, and the shelter holds the winter on a thousand small stitches.

**The Thin Winter.** The clothing runs short and the cold finds the gaps, and
the shelter learns that a wardrobe is not a luxury but a wall.

**The Cloth Road.** The shelter sells cloth and mending to the region and finds
that a needle can be a trade as surely as a forge.

**The Stored Thread.** The wardrobe is archived: names on garments, measurements
in a notebook, a record of what each person wore through it.

**Fade.** The same coats continue, and the shelter does not count layers this
year, and that is its own kind of answer.

---

## 55. APPENDIX AO — OPEN IMPLEMENTATION NOTES

- Garment metadata should live in item definitions where possible so legacy items
  degrade gracefully.
- Condition bands should be derived from existing durability fractions rather
  than stored twice.
- Insulation upgrades must register as shelter state so they survive save/load
  with the atmosphere system.
- Louse treatment must not become a second disease; it is an exposure trigger.
- Mending should be able to consume generic `cloth` if no patch item exists.
- The wardrobe store should be a room with capacity, not a global list.

---

## 56. APPENDIX AP — BEDDING AND NIGHT WARMTH MODEL

| Bedding state | Warmth at night | Effect |
|---|---|---|
| None | 0 | warmth drains fast |
| Straw alone | +3 | survivable mild nights |
| Blanket alone | +4 | survivable cool nights |
| Straw + blanket | +7 | survivable cold nights |
| Bedding + rugs | +9 | comfortable |
| Sleeping pod | +11 | warm through freeze |

Night warmth is where the wardrobe and the shelter meet: bedding is issued like
clothing, tracked with condition like clothing, and upgraded with insulation like
the shelter. The expansion's night model is deliberately simple because the
tension is in the decision to spend cloth on beds instead of coats.

---

## 57. APPENDIX AQ — WINTER WATCH KIT TABLE

| Kit | Contents | Weight | Warmth | Use |
|---|---|---|---|---|
| Standard Watch | coat, gloves, boots | 3 | 16 | gate duty |
| Cold Watch | coat, mittens, cap, boots | 4 | 22 | night patrol |
| Roof Watch | coat, gloves, felt boots, scarf | 4 | 24 | repairs |
| Salvage Watch | cloak, gloves, boots | 3 | 14 | ruins runs |
| Medical Watch | coat, gloves, clean cloth | 3 | 15 | clinic |
| Child Escort | child coat, scarf, boots | 2 | 10 | school run |

Watch kits are issued from the wardrobe store and returned at rotation. They
give the quartermaster a concrete planning object and give players a reason to
care whether the boots came back dry.

---

## 58. CLOSING STATEMENT

ASHFALL already degrades equipment, tracks warmth and hygiene, models shelter
thermal comfort, and stocks real clothing items with one protection number. What
it lacks is the meaning: layers, condition, laundry, fiber, leather, insulation,
and the small identities that clothing carries. The Thread adds that world
without adding a second inventory or a hidden warmth bar. It adds a mended coat,
a first bolt of cloth, a pair of boots that fits, and the winter the shelter
counted layers instead of losses.

> Wave 4 note: this plan is one of five Wave 4 expansion bibles (27–31). Each is
> self-contained; none requires another to ship. The shared Wave 4 index lives at
> `docs/expansions/wave4/WAVE4_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `Inventory.WornGear`, `NeedsSystem` warmth/hygiene,
> `ShelterAtmosphereSystem.ThermalComfort`/`Cleanliness`, `craft_textile_repair`,
> the clothing items in `items.json`, and the textile/leather narrative catalogs.