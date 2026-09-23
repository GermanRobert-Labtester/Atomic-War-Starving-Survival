# ASHFALL — Expansion 31 Design Bible
# THE KILN
### Wave 4 · Ceramics, Brick, Lime, Mortar, Concrete, Refractories, and Building Permanence

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-21
**Domain owners touched:** `Ashfall.Core.Narrative` (CeramicsKilnCatalog, MasonryBrickworksCatalog), `Ashfall.Core.Shelter` (upgrade path, CupolaFoundryEngine refractory), `Ashfall.Core.World` (RouteInfrastructureSystem), `Ashfall.Core.Crafting`
**Proposed host owner:** `KilnworksHostSession` (extends the shelter build and crafting surfaces)
**Existing save sections:** shelter upgrade state, crafting
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already knows how to fire clay and lay brick in fiction.
`CeramicsKilnCatalog` and `MasonryBrickworksCatalog` are live narrative catalogs.
The narrative data is real and detailed: `kiln_draw_trial_assays.json` (3.3 KB),
`lime_kiln_calcination_logs.json` (5.8 KB), `mudbrick_weathering_assays.json`
(5.2 KB), and `refractory_firebrick_spalling_logs.json` (5.3 KB).
`bunker_blueprints_codex.json` (22.7 KB) is a construction corpus.
`weather_hardening_upgrades.json` (3.7 KB) is a live upgrade catalog.
`RouteInfrastructureSystem` (16.7 KB) owns region-scale infrastructure;
`CupolaFoundryEngine` owns metallurgy and consumes refractory linings;
`SanitationSystem` (Expansion 22 territory) needs ceramic sanitary ware;
`GlassworksSystem` (Expansion 29 territory) needs fireclay pots; and the
greenhouse and the shelter both need foundations, walls, and drainage.

What does not exist: a lime kiln, a brick kiln, a pottery kiln, a refractory
line, mortar and plaster as crafted goods, concrete as a material, and a build
works that turns fired materials into permanent shelter upgrades.

**The Kiln** turns the shelter's second fire into the thing that makes everything
else permanent: fired clay, fired lime, fired brick, and the walls that stop
being improvised.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's first winter is a tent on concrete. The second winter is a wall.
The third is a village.

**The Kiln** is the expansion about building for permanence: limestone into lime,
clay into brick and tile, fireclay into crucibles and furnace linings, sand and
lime into mortar, and mortar into the walls, floors, drains, chimneys, and
furnaces that stop the shelter from being a camp. It extends the live narrative
craft knowledge with real kiln gameplay, ceramic products, masonry materials,
and a build works that consumes them.

The expansion's hard rules follow the live owners: metallurgy stays with the
foundry, optical glass stays with the glassworks, sanitation stays with its
facility owner, region infrastructure stays with `RouteInfrastructureSystem`, and
all build results run through the existing shelter upgrade path. No second
construction, foundry, or glass authority is created.

### 1.2 The five loops it adds

```
   Lime ──► Mortar ──► Walls ──► Shelter upgrades
     │         │          │
     ▼         ▼          ▼
   Kiln     Brick ──► Masonry ──► Drains, floors, chimneys
     │         │
     ▼         ▼
   Clay ──► Ceramics ──► Pottery, tiles, pipes, sanitary ware
     │
     ▼
   Fireclay ─► Refractory ──► Crucibles, furnace linings, kiln furniture
```

### 1.3 What the player manages

1. **Lime.** Limestone, burning, slaking, and lime grades.
2. **Kilns.** Fuel, firing curves, draws, and kiln furniture.
3. **Brick and block.** Clay, molds, drying, firing, and grading.
4. **Mortar and plaster.** Lime, sand, water, and mixes.
5. **Concrete.** Lime, aggregate, forms, and reinforcement.
6. **Ceramics.** Pottery, storage, tiles, pipes, and sanitary ware.
7. **Refractory.** Firebrick, crucibles, and linings for the foundry and the
   glassworks.
8. **Build works.** Plans, materials, labor, and permanent upgrades.

### 1.4 What it is not

- Not a second foundry. Metallurgy stays with `CupolaFoundryEngine`.
- Not a second glassworks. Optical glass stays with `PrecisionOpticsEngine`.
- Not a second sanitation system. Ceramic sanitary ware is an item consumed by
  live sanitation facilities.
- Not a second region infrastructure system. `RouteInfrastructureSystem` keeps
  bridges and roads.
- Not a second shelter upgrade system. Build results land in the live path.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Narrative/CeramicsKilnCatalog.cs` | Kiln and ceramics lore | `LIVE` |
| `Assets/Ashfall.Core/Narrative/MasonryBrickworksCatalog.cs` | Masonry lore | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CupolaFoundryEngine.cs` | Foundry and refractory consumption | `LIVE` |
| `Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs` | Region infrastructure | `LIVE` |
| `Assets/Ashfall.Core/Shelter/SanitationSystem.cs` | Sanitary facilities | `LIVE` |
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | Crafting authority | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterAssignmentSystem.cs` | Room and shelter state | `LIVE` |
| `Assets/StreamingAssets/Data/weather_hardening_upgrades.json` | Upgrade catalog | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `kiln_draw_trial_assays.json` | 3.3 KB | narrative firing trials |
| `lime_kiln_calcination_logs.json` | 5.8 KB | narrative lime work |
| `mudbrick_weathering_assays.json` | 5.2 KB | narrative test data |
| `refractory_firebrick_spalling_logs.json` | 5.3 KB | narrative refractory data |
| `bunker_blueprints_codex.json` | 22.7 KB | construction corpus |
| `weather_hardening_upgrades.json` | 3.7 KB | live upgrades |
| Kiln/ceramics/masonry gameplay data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-31-1 — No lime kiln.** Lime exists in fiction; the burn does not.
- **GAP-31-2 — No brick making.** No clay, molds, drying, or firing.
- **GAP-31-3 — No mortar or plaster.** No mixes or masonry work.
- **GAP-31-4 — No concrete.** No aggregate, forms, or reinforcement path.
- **GAP-31-5 — No ceramic products.** No pottery, tiles, pipes, or sanitary ware
  as items.
- **GAP-31-6 — No refractory gameplay.** Firebrick and crucibles are lore only,
  though the foundry and glassworks depend on them.
- **GAP-31-7 — No build works.** No material-driven permanent upgrades beyond the
  existing catalog.
- **GAP-31-8 — No clay or limestone locations.**
- **GAP-31-9 — No kiln fuel or firing-curve content.**

### 2.4 Non-duplication statement

This expansion will **not** add a second foundry, glassworks, sanitation,
infrastructure, or shelter upgrade system. It extends `CraftingSystem` with
ceramic and masonry recipes, feeds `CupolaFoundryEngine` and
`PrecisionOpticsEngine` with refractories, supplies `SanitationSystem` facilities
with ceramic wares, and lands all build results in the live shelter upgrade path.
It adds state only as an additive sub-object of the existing shelter/crafting
store. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Fire makes permanence.** A kiln is the difference between a camp and
a town.

**Pillar 2 — Materials have names.** Lime, fireclay, aggregate, mortar: the
shelter learns its materials the way it learned its medicines.

**Pillar 3 — Everything the shelter builds depends on something it fired.** The
drain, the chimney, the furnace lining, the storage jar, the greenhouse footing.

**Pillar 4 — Failures are graded.** A cracked brick is still a brick for a
footing; a failed crucible is a lost week.

**Pillar 5 — Build works is labor, not menus.** Every wall is materials, hands,
and a plan.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| First burn | Heat, smoke, patience | Magic |
| First brick | Weight in the hand | Triumph |
| Failed firing | Loss and lesson | Punishment |
| Lime slaking | Danger respected | Gore |
| Build works | Plans and hands | Menu construction |
| Permanence | Walls rising slowly | Instant city |

### 3.3 Content limits

- Kiln work is dangerous but never gory; burns are described practically.
- No real-world brands, building codes, or monuments copied.
- Child labor is absent; young apprentices are 16+ and supervised.
- No demolition spectacle or ruin-porn.
- Permanence is framed as care, not conquest.

---

## 4. THE KILNWORKS WORLD

### 4.1 Interior rooms

- **`room_kiln_yard`** — the kilns, the woodpile, and the draw.
- **`room_clay_room`** — pugging, wedging, and molds.
- **`room_drying_shed`** — green ware and bricks, drying.
- **`room_glaze_room`** — slips, glazes, and tests.
- **`room_pottery`** — wheels, benches, and finished ware.
- **`room_lime_house`** — quicklime, slaking pit, and caution.
- **`room_mortar_mill`** — sand, lime, and mixing.
- **`room_build_works`** — plans, scaffold, and materials.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_limestone_quarry` | The Lime Quarry | 5 | Limestone and haulage |
| `loc_clay_pit` | The Clay Pit | 4 | Clay and shale |
| `loc_sand_bank` | The Sand Bank | 3 | Sand for mortar |
| `loc_brick_ruins` | The Brickworks | 5 | Molds, kiln parts, salvage |
| `loc_stone_field` | The Stone Field | 4 | Field stone and rubble |
| `loc_old_viaduct` | The Viaduct | 6 | Masonry salvage and risk |
| `loc_cement_works` | The Cement Works | 6 | Pre-war cement and forms |
| `loc_charcoal_clamp` | The Clamp | 4 | Fuel for the kilns |
| `loc_test_wall` | The Test Wall | 2 | Weather testing masonry |
| `loc_build_site` | The Site | 3 | Active construction |

All locations require valid item references and scanner registration.

### 4.3 The kiln week

Dig in the morning, mold in the afternoon, dry for days, fire for a night, draw
and grade. The kiln's rhythm is the slowest in the shelter, and that slowness is
the expansion's subject.

---

## 5. MAIN STORYLINE — "WHAT WE BUILT WITH"

### 5.1 Central conflict

The shelter's newest wing is a leak. The drains silt, the greenhouse footing
shifts, and the foundry's furnace lining is spalling — and all three problems
have one answer the shelter has been avoiding: fireclay, lime, and brick.
**Orin Kaal**, a mason who helped build the old viaduct, says the shelter has all
three within a day's walk. **Nyssa Ord**, a potter, has a wheel. **Clem Hoad**
burned lime for a living and is willing to dig a kiln. **Taft Merrow**,
the builder, has plans and no materials.

The shelter discovers that permanence is a supply chain: limestone to lime, clay
to brick, fireclay to crucible, and all of it through one kiln that must be fed,
fired, and repaired before anything else gets better.

The expansion's question: **what is a shelter willing to build slowly, when
everything about survival rewards building fast?**

### 5.2 Theme (unspoken)

**A wall is a decision to stay.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_mason_orin_kaal` | Orin Kaal | Mason | Stone, brick, and standards |
| `npc_potter_nyssa_ord` | Nyssa Ord | Potter | Wheels, clay, and ceramics |
| `npc_limeburner_clem_hoad` | Clem Hoad | Lime burner | Quarry, burn, and slake |
| `npc_builder_taft_merrow` | Taft Merrow | Builder | Plans, labor, and build works |
| `npc_architect_sera_whit` | Sera Whit | Architect | Design and drainage |
| `npc_apprentice_jory` | Jory | Apprentice (16) | Learning the trade |
| `npc_digger_mire` | Mire | Clay digger | Supply and haulage |
| `npc_trader_fenn` | Fenn | Materials trader | Cement, tools, prices |

### 5.4 Story beats (15)

1. **The Leak.** A new wing fails; the cause is materials.
2. **The Survey.** Orin maps the clay, lime, and stone within reach.
3. **The First Kiln.** A lime kiln is dug and fired.
4. **The Slake.** Quicklime is slaked; the shelter learns caution.
5. **The First Brick.** Clay is molded, dried, and fired.
6. **The Grade.** Bricks are sorted; the shelter learns quality.
7. **The Mortar.** Lime and sand become mortar.
8. **The Wall.** The first permanent wall rises.
9. **The Drain.** Ceramic pipes fix the water problem.
10. **The Furnace Lining.** Firebrick saves the foundry.
11. **The Cat.** Hmm — replace: **The Greenhouse Footing.** Masonry for the
    greenhouse and glasshouse.
12. **The Chimney.** Smoke goes where it should.
13. **The Failures.** A bad firing and a cracked crucible.
14. **The Winter Test.** The test wall and the new wing face weather.
15. **What We Built With.** Final disposition of the kilnworks.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Kiln scale | clamp / kiln / works | capacity vs. cost |
| Priorities | shelter / foundry / sanitation | need vs. industry |
| Brick standard | rough / good / fine | speed vs. permanence |
| Lime use | mortar / plaster / both | repair vs. finish |
| Concrete | use / avoid / experimental | modernity vs. supply |
| Ceramics | storage / sanitary / decorative | utility vs. morale |
| Build pace | slow / steady / rushed | quality vs. deadline |
| Final | kilnworks as institution / as trade / as memory | identity |

### 5.6 Endings (5 + fade)

1. **The Fired Town** — brick, lime, and ceramics make the shelter permanent.
2. **The Kilnworks** — fired goods become the shelter's export.
3. **The Dry Shelter** — drains, chimneys, and mortar fix what water and smoke
   were ruining.
4. **The Furnace Saves the Foundry** — refractory keeps the metal flowing.
5. **The Cracked Firing** — the kiln fails and the shelter returns to salvage.
6. **Fade** — the kiln cools; the shelter patches instead of builds.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_kiln_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_kiln_leak`, `quest_kiln_survey`, `quest_kiln_first_kiln`,
`quest_kiln_slake`, `quest_kiln_first_brick`, `quest_kiln_grade`,
`quest_kiln_mortar`, `quest_kiln_wall`, `quest_kiln_drain`,
`quest_kiln_furnace_lining`, `quest_kiln_greenhouse_footing`,
`quest_kiln_chimney`, `quest_kiln_failures`, `quest_kiln_winter_test`,
`quest_kiln_what_we_built`.

### 6.2 Side quests (30)

**Lime (5)**
- `quest_kiln_limestone_run` — quarry and haul limestone
- `quest_kiln_burn` — burn a lime load
- `quest_kiln_slake_safe` — safe slaking practice
- `quest_kiln_lime_grade` — sort lime grades
- `quest_kiln_lime_store` — dry lime storage

**Brick (5)**
- `quest_kiln_clay_dig` — dig and test clay
- `quest_kiln_pug` — pug and wedge
- `quest_kiln_mold` — mold and dry
- `quest_kiln_fire` — fire a brick load
- `quest_kiln_grade_brick` — grade and stack

**Ceramics (5)**
- `quest_kiln_wheel` — build or repair a wheel
- `quest_kiln_pottery_run` — throw and fire pots
- `quest_kiln_tile_run` — tiles for floors and roofs
- `quest_kiln_pipe_run` — ceramic drainage pipes
- `quest_kiln_sanitary` — sanitary ware for the facilities

**Masonry (5)**
- `quest_kiln_mortar_mix` — mix mortar by grade
- `quest_kiln_plaster` — plaster walls
- `quest_kiln_stone_wall` — field stone wall
- `quest_kiln_footing` — a footing that holds
- `quest_kiln_test_wall` — build and test

**Refractory (5)**
- `quest_kiln_fireclay` — find fireclay
- `quest_kiln_firebrick` — make firebrick
- `quest_kiln_crucible` — make a crucible
- `quest_kiln_kiln_furniture` — saggars and shelves
- `quest_kiln_lining` — line the foundry

**Build works (5)**
- `quest_kiln_plans` — draw the plan
- `quest_kiln_materials` — stage materials
- `quest_kiln_scaffold` — build safe scaffold
- `quest_kiln_crew` — assign and train a crew
- `quest_kiln_inspect` — inspect finished work

### 6.3 Repeatable quests (8)

`quest_kiln_repeat_burn`, `quest_kiln_repeat_brick`,
`quest_kiln_repeat_pottery`, `quest_kiln_repeat_mortar`,
`quest_kiln_repeat_grade`, `quest_kiln_repeat_repair`,
`quest_kiln_repeat_haul`, `quest_kiln_repeat_inspect`.

### 6.4 Dynamic hooks

Live events (shelter upgrades, sanitation failures, foundry needs, weather damage,
glassworks demand, construction milestones) attach authored kiln follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Metallurgy stays with `CupolaFoundryEngine`.
- Optical glass stays with `PrecisionOpticsEngine`.
- Sanitation facilities stay with `SanitationSystem`.
- Region infrastructure stays with `RouteInfrastructureSystem`.
- Build results land in the live shelter upgrade path.
- Kilns consume real fuel; no free firing.
- No ceramic may be created without clay, forming, drying, and firing.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `KilnSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** kiln construction, firing curves, loads, draws, and kiln wear.
**Consumes:** fuel (live fuel economy), `CraftingSystem`, `Inventory`,
`SkillProgressionSystem`. **Data:** `kiln_jobs.json`. **Rules:** every firing has
a curve, a fuel cost, a draw, and a grade outcome; kiln furniture wears; a failed
draw is a real loss.

### 7.2 `LimeSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** limestone grades, calcination, slaking, and lime products. **Consumes:**
`KilnSystem`, `Inventory`, caution states for slaking injuries (medical pipeline).
**Data:** `lime_catalog.json`. **Rules:** lime burns hot and slakes dangerously;
grades matter to mortar, plaster, and whitewash; burns route through the medical
pipeline.

### 7.3 `CeramicsSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** clay preparation, throwing, molding, drying, glazing, and ceramic
products. **Consumes:** `KilnSystem`, `Inventory`, `SanitationSystem` (sanitary
ware), preservation (storage jars). **Data:** `ceramics_catalog.json`,
`glaze_recipes.json`. **Rules:** clay quality and firing curve decide the product;
storage, drainage, and sanitary wares are real items with real consumers.

### 7.4 `MasonrySystem` (new, `Ashfall.Core.Crafting`)

**Owns:** brick grades, mortar mixes, plaster, stone selection, and masonry
technique. **Consumes:** `CeramicsSystem`, `LimeSystem`, `CraftingSystem`.
**Data:** `masonry_catalog.json`, `mortar_recipes.json`. **Rules:** mixes are
authored; mortar grades match build types; poor mixes fail in weather; masonry
skill is real.

### 7.5 `ConcreteSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** aggregate, mixing, forms, curing, and reinforcement. **Consumes:**
`LimeSystem`, `MasonrySystem`, `Inventory`, metal for rebar from the foundry.
**Data:** `concrete_recipes.json`. **Rules:** concrete needs time and water to
cure; rushed pours fail; reinforcement is optional and expensive; concrete
consumes an industry's attention, not a menu button.

### 7.6 `RefractorySystem` (new, `Ashfall.Core.Shelter`)

**Owns:** fireclay, firebrick, crucibles, saggars, and furnace linings.
**Consumes:** `CeramicsSystem`, `KilnSystem`, `CupolaFoundryEngine` (demand),
`GlassworksSystem` (pots, fireclay). **Data:** `refractory_catalog.json`.
**Rules:** refractory is graded by heat resistance; linings wear and are
replaced; a failed lining stops a furnace.

### 7.7 `BuildWorksSystem` (new, `Ashfall.Core.Shelter`)

**Owns:** plans, staged materials, crews, scaffold, and build progress toward
live shelter upgrades. **Consumes:** `MasonrySystem`, `ConcreteSystem`,
`RefractorySystem`, `Inventory`, `DutyRoster`, `ShelterAssignmentSystem`.
**Data:** `build_plans.json`. **Rules:** every build consumes staged materials
and labor; results land in the live upgrade path; unfinished builds can sit and
weather; no instant construction.

### 7.8 Systems explicitly not added

- No second foundry, glassworks, sanitation, or infrastructure system.
- No instant-build menu.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `kiln_jobs.json` (new)

```json
{
  "schema_version": 1,
  "jobs": [
    {
      "job_id": "kiln_lime_burn",
      "display_name": "Lime Burn",
      "load": "limestone",
      "quantity": 40,
      "fuel": 30,
      "curve_hours": 24,
      "draw_hours": 12,
      "outcomes": ["quicklime", "lime_grade"],
      "failure": "underburned",
      "tags": ["lime", "hot"]
    }
  ]
}
```

### 8.2 `lime_catalog.json` (new)

Lime rows: source, burn, slake, grades, uses, and safety.

### 8.3 `ceramics_catalog.json` (new)

Ceramic rows: clay, forming, drying, firing, glaze, product, and grade bands.

### 8.4 `glaze_recipes.json` (new)

Glaze rows: base, colorant, firing range, surface, and use.

### 8.5 `masonry_catalog.json` (new)

Masonry rows: material, unit, bond, skill, load, and weather resistance.

### 8.6 `mortar_recipes.json` (new)

Mixes: lime, sand, water, additives, grade, use, and cure time.

### 8.7 `concrete_recipes.json` (new)

Concrete rows: lime, aggregate, water, reinforcement, forms, cure, and strength.

### 8.8 `refractory_catalog.json` (new)

Refractory rows: fireclay, heat rating, use, wear rate, and replacement.

### 8.9 `build_plans.json` (new)

Plans: structure, materials, labor, days, prerequisites, and live upgrade target.

### 8.10 Items

New items appended to `items.json`: `item_limestone`, `item_quicklime`,
`item_slaked_lime`, `item_fireclay`, `item_clay`, `item_brick`,
`item_firebrick`, `item_field_stone`, `item_sand`, `item_aggregate`,
`item_mortar_mix`, `item_plaster_mix`, `item_ceramic_tile`,
`item_ceramic_pipe`, `item_pottery_jar`, `item_sanitary_bowl`,
`item_crucible`, `item_saggar`, `item_kiln_grate`, `item_rebar`,
`item_whitewash`, `item_test_block`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

Shelter upgrade state and crafting remain the live owners. New sub-objects (kiln
jobs, lime stock, ceramic batches, masonry stock, concrete pours, refractory
inventory, build plans) are additive. No new save section.

### 9.2 State to persist

- Kiln construction and wear.
- Firing loads and draws.
- Lime stock and grades.
- Ceramic batches and grades.
- Mortar, plaster, and concrete stock.
- Refractory inventory and lining condition.
- Build plans, staged materials, and progress.

### 9.3 Determinism

- Firing and grading outcomes are deterministic given inputs, curve, and skill.
- Curing and weathering are deterministic over time.
- No wall-clock or unseeded randomness; daily ticks use the live path.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing shelter and crafting state untouched; no kiln,
lime, ceramic, masonry, concrete, refractory, or build-plan state exists until
started. Existing upgrades remain valid.

### 9.5 Checksum

Invariant-culture floats; integer counts for units and grades.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `KilnPanel` (new) | Firing loads and draws | `KilnworksHostSession` |
| `LimePanel` (new) | Burn, slake, grade | same |
| `CeramicsPanel` (new) | Clay, form, fire, glaze | same |
| `MasonryPanel` (new) | Brick, mortar, plaster | same |
| `ConcretePanel` (new) | Mix, pour, cure | same |
| `RefractoryPanel` (new) | Firebrick, crucibles, linings | same |
| `BuildWorksPanel` (new) | Plans, materials, crews | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Fuel and time costs are shown before a firing starts.
- Build progress shows materials staged and work remaining.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Grade and quality always carry text labels alongside any color.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a kiln roaring, bricks stacking, lime
slaking, a trowel, a form being struck. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CraftingSystem` | Kiln, ceramic, masonry recipes |
| `CupolaFoundryEngine` | Refractory linings and crucibles |
| `PrecisionOpticsEngine` / glassworks | Fireclay pots and furnace parts |
| `SanitationSystem` | Ceramic sanitary ware and pipes |
| `RouteInfrastructureSystem` | Region-scale masonry supply, untouched |
| `ShelterAssignmentSystem` | Rooms and upgrade targets |
| `weather_hardening_upgrades.json` | Live upgrade path |
| `DutyRoster` | Build crews |
| `PowerGridSystem` | Kiln fans and electric boost |
| `Fuel economy` | Kiln firing |
| `MedicalPipelineCoordinator` | Burn and slake injuries |
| `TradingSystem` | Fired goods trade |
| `Inventory` | Materials and products |
| `EpilogueChronicleBuilder` | Build milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm narrative kiln and masonry catalogs, the
four narrative data files, `bunker_blueprints_codex.json`, the live upgrade
catalog, foundry refractory consumption, and sanitation facility items. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Author kiln jobs, lime, ceramics, glazes,
masonry, mortar, concrete, refractory, build plans; append items. Register
validators and scanner.

**Phase 2 — Pure Core.** `KilnSystem`, `LimeSystem`, `CeramicsSystem`,
`MasonrySystem`, `ConcreteSystem`, `RefractorySystem`, `BuildWorksSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `KilnworksHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak including failed firings, weather damage,
and the foundry lining cycle.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Kiln jobs | 20 |
| Lime rows | 10 |
| Ceramic products | 40 |
| Glaze recipes | 12 |
| Masonry rows | 15 |
| Mortar recipes | 10 |
| Concrete recipes | 10 |
| Refractory rows | 12 |
| Build plans | 20 |
| Items | 22 |
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
| Second foundry/glass | Critical | Live owners keep their fire |
| Second build system | Critical | Live upgrade path |
| Instant construction | High | Staged materials and crews |
| Kiln too easy | High | Fuel, curve, failure |
| Burn gore | Medium | Practical restraint |
| Duplicate sanitation effect | Medium | Facilities keep their owner |
| Determinism break | Low | Live tick paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `kiln_jobs.json` | 20 | 4,000 |
| `lime_catalog.json` | 10 | 2,500 |
| `ceramics_catalog.json` | 40 | 8,000 |
| `glaze_recipes.json` | 12 | 2,500 |
| `masonry_catalog.json` | 15 | 3,500 |
| `mortar_recipes.json` | 10 | 2,000 |
| `concrete_recipes.json` | 10 | 2,500 |
| `refractory_catalog.json` | 12 | 3,000 |
| `build_plans.json` | 20 | 5,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 22 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~64,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R31-1 | Second foundry | Low | Critical | Foundry untouched |
| R31-2 | Second build system | Low | Critical | Live upgrade path |
| R31-3 | Instant construction | Med | High | Materials and labor |
| R31-4 | Kiln trivial | Med | High | Fuel and failures |
| R31-5 | Burn gore | Low | Med | Restraint |
| R31-6 | Sanitation duplication | Med | Med | Facilities own effects |
| R31-7 | Refractory orphans foundry | Med | Med | Demand from live engine |
| R31-8 | Determinism | Low | High | Live tick paths |
| R31-9 | Content overrun | Med | Med | Budget §13 |
| R31-10 | Location safety | Low | Med | Quarry and viaduct danger |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does concrete exist in the setting at scale, or only salvage?** Recommended:
   small-scale pouring with salvaged cement as the fast path.
2. **Do ceramic sanitary wares replace or repair live facilities?** Recommended:
   repair and upgrade through the live facility owner.
3. **Is fireclay gated behind a location or a trade?** Recommended: both.
4. **Does the kiln share fuel with the foundry and glassworks?** Recommended: yes;
   that competition is the expansion's core tension.
5. **Can unfinished builds decay?** Recommended: yes, slowly, with weather.

---

## 17. APPENDIX D — KILN JOB TABLE (20 JOBS)

| # | Job | Load | Qty | Fuel | Curve | Draw | Failure |
|---|---|---|---|---|---|---|---|
| 1 | Lime Burn | limestone | 40 | 30 | 24h | 12h | underburned |
| 2 | Brick Fire | green brick | 80 | 40 | 36h | 12h | cracked |
| 3 | Tile Fire | green tile | 100 | 36 | 30h | 10h | warped |
| 4 | Pottery Bisque | green ware | 60 | 28 | 24h | 10h | broken |
| 5 | Pottery Glaze | bisque | 60 | 24 | 12h | 10h | run |
| 6 | Pipe Fire | green pipe | 40 | 30 | 30h | 12h | sag |
| 7 | Sanitary Fire | green ware | 20 | 34 | 36h | 12h | crack |
| 8 | Firebrick Fire | fireclay brick | 60 | 44 | 48h | 16h | spall |
| 9 | Crucible Fire | fireclay | 12 | 30 | 36h | 12h | crack |
| 10 | Saggar Fire | fireclay | 20 | 28 | 30h | 12h | warp |
| 11 | Quicklime Reburn | underburned | 20 | 18 | 12h | 8h | waste |
| 12 | Charcoal Clamp | wood | 200 | 0 | 72h | 24h | ash |
| 13 | Test Blocks | mixed | 30 | 20 | 24h | 10h | weak |
| 14 | Drain Tile Fire | green tile | 80 | 32 | 30h | 10h | crack |
| 15 | Roof Tile Fire | green tile | 120 | 40 | 34h | 12h | warp |
| 16 | Floor Tile Fire | green tile | 90 | 34 | 30h | 10h | crack |
| 17 | Storage Jar Fire | green jar | 50 | 28 | 26h | 10h | break |
| 18 | Whitewash Lime | quicklime | 20 | 10 | 8h | 6h | weak |
| 19 | Kiln Furniture | fireclay | 24 | 26 | 28h | 10h | warp |
| 20 | Salvage Brick Refire | old brick | 60 | 30 | 24h | 10h | spall |

Every firing is a night the shelter spends watching a fire it cannot interrupt.
The job list is the expansion's pacing, because a kiln load takes days and a bad
draw takes no time at all to disappoint.

---

## 18. APPENDIX E — LIME TABLE (10 ROWS)

| # | Lime | Source | Burn | Slake | Grade | Use |
|---|---|---|---|---|---|---|
| 1 | Raw Limestone | quarry | none | none | none | kiln feed |
| 2 | Quicklime | limestone | 24h | none | high | mortar, whitewash |
| 3 | Slaked Lime | quicklime | — | 24h | high | mortar, plaster |
| 4 | Hydraulic Lime | clay-limestone | 28h | 24h | strong | foundations |
| 5 | Weak Lime | underburned | 12h | 24h | low | whitewash |
| 6 | Overburned Lime | limestone | 40h | 48h | slow | special |
| 7 | Lime Putty | slaked lime | — | 72h | fine | plaster |
| 8 | Lime Wash | slaked lime | — | 6h | thin | walls |
| 9 | Lime Mortar Base | slaked lime | — | 24h | med | masonry |
| 10 | Agricultural Lime | limestone | 12h | 12h | coarse | soil |

Lime is the expansion's dangerous material: it burns hot to make and burns skin
to slake, and it is the ingredient that turns sand and clay into permanence.

---

## 19. APPENDIX F — CERAMICS TABLE (40 PRODUCTS)

| # | Product | Clay | Forming | Fire | Use |
|---|---|---|---|---|---|
| 1 | Storage Jar | common | thrown | bisque | food |
| 2 | Wide Jar | common | thrown | bisque | preserve |
| 3 | Lid | common | thrown | bisque | seal |
| 4 | Bowl | common | thrown | bisque | kitchen |
| 5 | Plate | common | pressed | bisque | kitchen |
| 6 | Cup | fine | thrown | glazed | table |
| 7 | Pitcher | common | thrown | bisque | water |
| 8 | Bottle | common | thrown | bisque | liquid |
| 9 | Flask | common | thrown | glazed | field |
| 10 | Chamber Pot | common | thrown | glazed | hygiene |
| 11 | Sanitary Bowl | fine | cast | glazed | sanitation |
| 12 | Sanitary Pipe | fine | extruded | glazed | sanitation |
| 13 | Drain Pipe | common | extruded | fire | drainage |
| 14 | Drain Tile | common | molded | fire | drainage |
| 15 | Roof Tile | common | molded | fire | roofing |
| 16 | Floor Tile | common | molded | fire | flooring |
| 17 | Wall Tile | fine | molded | glazed | walls |
| 18 | Brick | common | molded | fire | masonry |
| 19 | Firebrick | fireclay | molded | fire | refractory |
| 20 | Crucible | fireclay | thrown | fire | foundry |
| 21 | Saggar | fireclay | thrown | fire | kiln furniture |
| 22 | Kiln Shelf | fireclay | pressed | fire | kiln furniture |
| 23 | Kiln Grate | fireclay | pressed | fire | kiln |
| 24 | Pot | common | thrown | bisque | cooking |
| 25 | Cauldron Liner | fireclay | molded | fire | cooking |
| 26 | Butter Crock | fine | thrown | glazed | kitchen |
| 27 | Pickle Crock | common | thrown | glazed | preserve |
| 28 | Herb Jar | fine | thrown | glazed | kitchen |
| 29 | Oil Lamp | fine | thrown | glazed | lighting |
| 30 | Lamp Burner | fine | thrown | fire | lighting |
| 31 | Ink Pot | fine | thrown | glazed | press |
| 32 | Paint Pot | common | thrown | bisque | works |
| 33 | Stove Liner | fireclay | molded | fire | heating |
| 34 | Muffle Tile | fireclay | molded | fire | furnace |
| 35 | Gutter | common | extruded | fire | drainage |
| 36 | Cistern Liner | common | molded | fire | water |
| 37 | Soak Pit Ring | common | molded | fire | sanitation |
| 38 | Bead and Button | fine | molded | glazed | trade |
| 39 | Figurine | fine | molded | glazed | morale |
| 40 | Test Block | mixed | molded | fire | quality |

Ceramics is the expansion's most human line: the same clay becomes a roof tile,
a drain pipe, a chamber pot, a cooking pot, and a small figurine someone put on
a shelf, and all of them matter.

---

## 20. APPENDIX G — GLAZE TABLE (12 GLAZES)

| # | Glaze | Base | Colorant | Fire | Surface |
|---|---|---|---|---|---|
| 1 | Clear | ash, clay | none | med | glossy |
| 2 | Salt | salt, clay | none | high | orange peel |
| 3 | Ash | wood ash | iron | med | run |
| 4 | Iron Red | clay | iron | high | deep red |
| 5 | Iron Black | clay | iron, manganese | high | dark |
| 6 | Copper Green | clay | copper | med | green |
| 7 | Lead White | lead, clay | tin | low | whites |
| 8 | Tin White | clay | tin | med | opaque |
| 9 | Slip | clay, water | none | low | matte |
| 10 | Feldspar | feldspar | none | high | glossy |
| 11 | Slag | furnace slag | iron | high | dark |
| 12 | Test Glaze | mixed | mixed | med | varies |

Glaze is where a working product becomes a desirable one. It is also where lead
appears as a hazard, which the expansion authors with a clear safety line.

---

## 21. APPENDIX H — MASONRY TABLE (15 ROWS)

| # | Material | Unit | Bond | Skill | Load | Weather |
|---|---|---|---|---|---|---|
| 1 | Field Stone | irregular | rubble | 2 | high | good |
| 2 | Cut Stone | squared | ashlar | 4 | very high | excellent |
| 3 | Common Brick | molded | running | 2 | med | good |
| 4 | Good Brick | molded | Flemish | 3 | high | very good |
| 5 | Firebrick | molded | stack | 4 | med | heat only |
| 6 | Concrete Block | cast | stack | 3 | high | good |
| 7 | Mud Brick | molded | adobe | 1 | low | poor |
| 8 | Rammed Earth | pounded | layered | 2 | med | fair |
| 9 | Rubble Fill | loose | none | 1 | low | poor |
| 10 | Salvage Brick | mixed | mixed | 2 | med | fair |
| 11 | Tile Block | molded | stack | 3 | med | good |
| 12 | Arch Brick | molded | arch | 4 | high | excellent |
| 13 | Pier Stone | cut | pier | 4 | high | excellent |
| 14 | Gutter Brick | molded | channel | 3 | low | good |
| 15 | Test Wall | mixed | mixed | 2 | — | measured |

Masonry quality is a language the shelter learns by season: which wall holds in
thaw, which footing shifts, and which brick was worth the extra day of firing.

---

## 22. APPENDIX I — MORTAR TABLE (10 MIXES)

| # | Mix | Lime | Sand | Additives | Cure | Use |
|---|---|---|---|---|---|---|
| 1 | Common Mortar | 1 | 3 | none | 7d | brick |
| 2 | Strong Mortar | 1 | 2 | clay | 7d | foundations |
| 3 | Hydraulic Mortar | 1 | 2 | volcanic | 3d | wet areas |
| 4 | Plaster Base | 1 | 4 | hair | 3d | walls |
| 5 | Fine Plaster | 1 | 3 | gypsum | 2d | finish |
| 6 | Whitewash | 1 | 0 | salt | 2d | walls |
| 7 | Stone Mortar | 1 | 3 | stone dust | 7d | field stone |
| 8 | Fire Mortar | 1 | 2 | fireclay | 7d | refractories |
| 9 | Fast Mortar | 1 | 2 | ash | 2d | repair |
| 10 | Test Mortar | 1 | 3 | measured | 7d | testing |

Mortar is the quiet hero of the expansion: a wall is only as good as the mix
between its units, and a bad mix is a wall that comes apart in the first thaw.

---

## 23. APPENDIX J — CONCRETE TABLE (10 RECIPES)

| # | Recipe | Lime | Aggregate | Water | Rebar | Cure | Strength |
|---|---|---|---|---|---|---|---|
| 1 | Lean Concrete | 1 | 6 | 1 | none | 14d | low |
| 2 | Common Concrete | 1 | 4 | 1 | none | 14d | med |
| 3 | Strong Concrete | 1 | 3 | 1 | none | 21d | high |
| 4 | Reinforced | 1 | 3 | 1 | yes | 21d | very high |
| 5 | Footing Mix | 1 | 5 | 1 | none | 21d | high |
| 6 | Floor Mix | 1 | 4 | 1 | mesh | 14d | med |
| 7 | Drain Mix | 1 | 5 | 1 | none | 14d | med |
| 8 | Wall Pour | 1 | 4 | 1 | yes | 21d | high |
| 9 | Quick Patch | 1 | 4 | 2 | none | 3d | low |
| 10 | Salvage Cement | salvage | 4 | 1 | optional | 14d | med |

Concrete is the expansion's expensive shortcut: it needs water, time, and
attention, and a pour that is rushed is a pour that fails. Salvaged pre-war
cement is the fast path and the scarce one.

---

## 24. APPENDIX K — REFRACTORY TABLE (12 ROWS)

| # | Refractory | Heat | Use | Wear | Replace |
|---|---|---|---|---|---|
| 1 | Common Firebrick | 1200 | stoves | med | yearly |
| 2 | High Firebrick | 1400 | foundry | slow | 2 years |
| 3 | Fireclay Mortar | 1200 | joints | med | yearly |
| 4 | Crucible Small | 1300 | metal pours | fast | monthly |
| 5 | Crucible Large | 1300 | glass, metal | fast | monthly |
| 6 | Saggar | 1200 | kiln ware | med | yearly |
| 7 | Kiln Shelf | 1250 | kiln | slow | yearly |
| 8 | Muffle | 1400 | furnace | med | yearly |
| 9 | Stove Liner | 1100 | heating | fast | seasonal |
| 10 | Furnace Lining | 1400 | foundry | slow | 2 years |
| 11 | Glass Pot | 1350 | glassworks | fast | monthly |
| 12 | Test Brick | varies | testing | — | — |

Refractory is the material that keeps other industries alive: the foundry, the
glassworks, the stoves, and the kiln itself all depend on fired clay that does
not soften. The expansion makes that dependency explicit and cyclical.

---

## 25. APPENDIX L — BUILD PLAN TABLE (20 PLANS)

| # | Plan | Materials | Labor | Days | Upgrade target |
|---|---|---|---|---|---|
| 1 | Repair Wing | brick, mortar | 2 | 6 | shelter repair |
| 2 | Drain Run | pipe, mortar | 2 | 5 | sanitation |
| 3 | Cistern | brick, mortar | 3 | 10 | water storage |
| 4 | Chimney | brick, mortar | 2 | 4 | heating |
| 5 | Greenhouse Footing | stone, mortar | 3 | 8 | greenhouse |
| 6 | Foundry Lining | firebrick, mortar | 2 | 5 | foundry |
| 7 | Kiln Repair | firebrick, brick | 2 | 4 | kiln |
| 8 | Cellar Walls | brick, mortar | 3 | 12 | storage |
| 9 | Floor Pour | concrete | 3 | 6 | shelter |
| 10 | Wall Pour | concrete, rebar | 4 | 10 | shelter |
| 11 | Test Wall | mixed | 1 | 3 | testing |
| 12 | Roof Tiles | tiles, mortar | 2 | 8 | shelter |
| 13 | Stove Rebuild | firebrick | 2 | 4 | heating |
| 14 | Oven Rebuild | firebrick | 2 | 5 | kitchen |
| 15 | Bath Floor | tile, concrete | 3 | 8 | sanitation |
| 16 | Smokehouse Floor | brick | 2 | 4 | preservation |
| 17 | Well Lining | brick, mortar | 3 | 7 | water |
| 18 | Soak Pit | rings, pipe | 2 | 5 | sanitation |
| 19 | Path Paving | brick, sand | 2 | 4 | movement |
| 20 | Watch Point | stone, mortar | 3 | 10 | defense |

Every plan consumes staged materials and labor and lands in a live shelter
upgrade. The list is deliberately ordinary: drains, chimneys, floors, and
footings are what permanence actually looks like.

---

## 26. APPENDIX M — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_kiln_leak` | 4 | Diagnose leak; trace to materials |
| `quest_kiln_survey` | 4 | Map clay, lime, stone |
| `quest_kiln_first_kiln` | 5 | Site, dig, build, fire |
| `quest_kiln_slake` | 4 | Burn, slake, store safely |
| `quest_kiln_first_brick` | 5 | Dig, mold, dry, fire |
| `quest_kiln_grade` | 4 | Sort, test, stack |
| `quest_kiln_mortar` | 4 | Mix, test, apply |
| `quest_kiln_wall` | 5 | Footing, wall, cure |
| `quest_kiln_drain` | 5 | Pipe, lay, connect |
| `quest_kiln_furnace_lining` | 5 | Firebrick, line, heat test |
| `quest_kiln_greenhouse_footing` | 4 | Stone, mortar, level |
| `quest_kiln_chimney` | 4 | Brick, mortar, draw test |
| `quest_kiln_failures` | 5 | Bad firing and cracked pot |
| `quest_kiln_winter_test` | 5 | Weather, inspect, repair |
| `quest_kiln_what_we_built` | 3 | Final disposition |

---

## 27. APPENDIX N — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_kiln_limestone_run` | 3 | Quarry, sort, haul |
| `quest_kiln_burn` | 4 | Load, fire, draw, grade |
| `quest_kiln_slake_safe` | 3 | Site, gear, process |
| `quest_kiln_lime_grade` | 3 | Sort, label, store |
| `quest_kiln_lime_store` | 3 | Dry, cover, rotate |
| `quest_kiln_clay_dig` | 3 | Dig, test, stock |
| `quest_kiln_pug` | 3 | Soak, wedge, rest |
| `quest_kiln_mold` | 4 | Mold, dry, turn |
| `quest_kiln_fire` | 4 | Load, curve, draw |
| `quest_kiln_grade_brick` | 3 | Sort, test, stack |
| `quest_kiln_wheel` | 4 | Build, true, test |
| `quest_kiln_pottery_run` | 4 | Throw, dry, fire |
| `quest_kiln_tile_run` | 4 | Mold, dry, fire |
| `quest_kiln_pipe_run` | 4 | Extrude, dry, fire |
| `quest_kiln_sanitary` | 4 | Cast, glaze, fire |
| `quest_kiln_mortar_mix` | 3 | Measure, mix, test |
| `quest_kiln_plaster` | 4 | Mix, apply, finish |
| `quest_kiln_stone_wall` | 4 | Select, lay, cuff |
| `quest_kiln_footing` | 4 | Dig, pour, level |
| `quest_kiln_test_wall` | 4 | Build, weather, assess |
| `quest_kiln_fireclay` | 4 | Locate, test, stock |
| `quest_kiln_firebrick` | 4 | Mold, fire, grade |
| `quest_kiln_crucible` | 4 | Throw, fire, test |
| `quest_kiln_kiln_furniture` | 3 | Form, fire, fit |
| `quest_kiln_lining` | 5 | Strip, line, cure |
| `quest_kiln_plans` | 3 | Draw, review, approve |
| `quest_kiln_materials` | 3 | List, gather, stage |
| `quest_kiln_scaffold` | 3 | Build, inspect, tag |
| `quest_kiln_crew` | 4 | Assign, train, rotate |
| `quest_kiln_inspect` | 3 | Check, record, fix |

---

## 28. APPENDIX O — NPC DOSSIERS (BRIEF)

**Orin Kaal** — mason. Built the viaduct and watched it outlive the town it
served. Judges a wall by how it behaves in its third winter, not its first.

**Nyssa Ord** — potter. Throws jars that the kitchen fights over and treats a
warped batch as a lesson instead of a loss. Keeps a shelf of test tiles.

**Clem Hoad** — lime burner. Digs, burns, and slakes with the calm of someone
who has been careful for forty years. Teaches slaking safety by story because
stories stick better than warnings.

**Taft Merrow** — builder. Has plans, patience, and a crew he refuses to rush.
Believes a footing is the most honest part of a building because no one ever
sees it.

**Sera Whit** — architect. Draws drains and chimneys when the shelter wants
monuments, and is usually right. The expansion's voice for building for use.

**Jory** — apprentice, sixteen. Carries stone, learns mixes, and gets the first
lecture on every hazard by watching an adult make the mistake.

**Mire** — clay digger. Knows the pits by moisture and talks about clay like
other people talk about wine.

**Fenn** — materials trader. Sells salvaged cement, tools, and firebrick; prices
by scarcity and knows exactly which shelter is about to need a lining.

---

## 29. APPENDIX P — LOCATION DETAIL

- **The Lime Quarry** — limestone, haulage, and a cut face that drops.
- **The Clay Pit** — clay, shale, and water table questions.
- **The Sand Bank** — sand for mortar, sorted by grit.
- **The Brickworks** — molds, drying sheds, and a kiln that might be salvageable.
- **The Stone Field** — field stone, rubble, and a wall that predates the war.
- **The Viaduct** — masonry salvage and a climb that kills the careless.
- **The Cement Works** — pre-war cement, forms, and a silo that may still hold.
- **The Clamp** — charcoal for firing; smoke for days.
- **The Test Wall** — samples, weather, and honest results.
- **The Site** — the active build, staged, noisy, and slow.

---

## 30. APPENDIX Q — FIRING CURVE AND GRADING MODEL

| Curve | Ramp | Hold | Cool | Outcome |
|---|---|---|---|---|
| Fast | quick | short | quick | cracks |
| Standard | steady | medium | slow | good |
| Slow | gentle | long | slow | strong |
| Held | steady | very long | slow | vitrified |
| Uneven | variable | variable | variable | mixed |
| Cold | none | none | none | green |

| Grade | Use |
|---|---|
| First | structural, sanitary |
| Second | walls, drains |
| Third | fill, footings |
| Reject | crushing, test |

Firing curves are authored, not hidden probability. The shelter can choose the
fast path and accept losses, or the slow path and accept time, and every draw
teaches the grade.

---

## 31. APPENDIX R — FUEL COMPETITION TABLE

| Consumer | Priority | Demand | Alternative |
|---|---|---|---|
| Kitchen | highest | daily | none |
| Heating | high | seasonal | insulation |
| Kiln | medium | cyclic | charcoal |
| Foundry | medium | cyclic | coke |
| Glassworks | medium | weekly | boost |
| Lime burn | low | batch | none |
| Charcoal clamp | low | batch | self |
| Heating water | medium | daily | solar |

The kiln competes with the kitchen and the shelter's warmth, which is the reason
firing schedules matter. The expansion's fuel table is a social document as much
as a mechanical one.

---

## 32. APPENDIX S — WORKED 360-DAY KILNWORKS SCENARIO

**Days 1–30.** Leak traced to failed mortar. Survey identifies clay, limestone,
and sand. First clamp charcoal burned.

**Days 31–60.** Lime kiln fired; first slake; first bricks molded and dried; first
firing yields thirds and seconds.

**Days 61–90.** Mortar and wall go up; the wing is repaired. Drain pipes fired and
laid. Foundry lining replaced with firebrick.

**Days 91–150.** Greenhouse footing built. First glazed pottery. Test wall
records weather. Chimney draws properly.

**Days 151–220.** Winter: kiln runs on charcoal; stove liners replaced; the test
wall is measured after freeze-thaw.

**Days 221–300.** Concrete pour for the floor; sanitary ware made; cellar walls
built; ceramics trade begins.

**Days 301–360.** Second winter's inspection: the wall holds, the drains flow,
the lining is at half wear, and the kilnworks is a permanent part of the shelter.

---

## 33. APPENDIX T — VIGNETTE (TONE SAMPLE)

> Clem opens the kiln door and the heat comes out in a wall, and he steps back
> and counts, and the lime inside has gone from grey to white, which means the
> night was worth it.

> Nyssa sets a jar on the shelf next to thirty others and taps it once, and the
> jar rings true, and she moves on because there are forty more to throw before
> the light goes.

> Orin lays a brick and checks the line and lays another, and the wall is a
> finger taller than it was an hour ago, and that is how a shelter stops being a
> camp.

---

## 34. APPENDIX U — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Underburned lime | poor mortar | reburn, mix weak |
| Cracked brick load | losses | curve fix, reuse thirds |
| Warped tile | roofing short | re-fire, trade |
| Failed crucible | foundry delay | make several |
| Spalled lining | furnace down | reline, schedule |
| Bad mortar | wall fails | rebuild, test |
| Rushed pour | weak floor | break out, redo |
| Slake burn | injury | medical care, safety |
| Weather damage | repairs | inspect, patch |
| Fuel shortage | kiln cold | charcoal, schedule |

No failure is a game over. Fired materials fail slowly and visibly, and every
failure teaches a grade the shelter can name.

---

## 35. APPENDIX V — CONTENT REVIEW CHECKLIST

- [ ] `CupolaFoundryEngine` remains the metallurgy authority.
- [ ] Optical glass stays with `PrecisionOpticsEngine`.
- [ ] Sanitation facilities stay with `SanitationSystem`.
- [ ] Region infrastructure stays with `RouteInfrastructureSystem`.
- [ ] Build results land in the live upgrade path.
- [ ] Kilns consume real fuel.
- [ ] Every ceramic passes forming, drying, and firing.
- [ ] Burn and slake injuries route through the medical pipeline.
- [ ] No instant construction exists.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live tick paths only.

---

## 36. APPENDIX W — GLOSSARY

- **Calcination** — burning limestone into quicklime.
- **Slaking** — adding water to quicklime.
- **Pugging** — working clay to uniform moisture.
- **Green ware** — unfired clay objects.
- **Bisque** — fired but unglazed ware.
- **Saggar** — protective clay box for kiln ware.
- **Fireclay** — clay that resists heat.
- **Refractory** — material that survives furnace temperatures.
- **Hydraulic lime** — lime that sets under water.
- **Cure** — time for lime or concrete to harden.

---

## 37. APPENDIX X — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `KilnSystem` | fuel, loads | firings, draws | products |
| `LimeSystem` | limestone | lime grades | mortar |
| `CeramicsSystem` | clay | ware | sanitation |
| `MasonrySystem` | brick, lime | masonry stock | upgrades |
| `ConcreteSystem` | lime, aggregate | pours | upgrades |
| `RefractorySystem` | fireclay | linings, crucibles | foundry |
| `BuildWorksSystem` | plans, materials | build progress | upgrades |
| `CupolaFoundryEngine` | refractories | metal | kilns |
| `SurvivalGlassworks` | pots | glass | kilns |
| `SanitationSystem` | wares | facilities | ceramics |
| `RouteInfrastructureSystem` | masonry | routes | build works |
| `ShelterAssignmentSystem` | upgrades | rooms | materials |
| `DutyRoster` | crews | shifts | build |
| `MedicalPipelineCoordinator` | burns | care | kilns |
| `TradingSystem` | fired goods | trade | production |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 38. APPENDIX Y — DATA SCHEMA DETAIL (NEW CATALOGS)

**`kiln_jobs.json`** — `job_id`, `display_name`, `load`, `quantity`, `fuel`,
`curve_hours`, `draw_hours`, `outcomes[]`, `failure`, `tags`.

**`lime_catalog.json`** — `lime_id`, `display_name`, `source`, `burn`,
`slake`, `grade`, `use`, `safety`, `tags`.

**`ceramics_catalog.json`** — `product_id`, `display_name`, `clay`, `forming`,
`fire`, `glaze`, `use`, `grade_bands[]`, `tags`.

**`glaze_recipes.json`** — `glaze_id`, `display_name`, `base`, `colorant`,
`fire`, `surface`, `hazard`, `tags`.

**`masonry_catalog.json`** — `masonry_id`, `display_name`, `unit`, `bond`,
`skill`, `load`, `weather`, `tags`.

**`mortar_recipes.json`** — `mix_id`, `display_name`, `lime`, `sand`,
`additives[]`, `cure_days`, `use`, `tags`.

**`concrete_recipes.json`** — `recipe_id`, `display_name`, `lime`, `aggregate`,
`water`, `rebar`, `cure_days`, `strength`, `tags`.

**`refractory_catalog.json`** — `refractory_id`, `display_name`, `heat`,
`use`, `wear`, `replacement`, `tags`.

**`build_plans.json`** — `plan_id`, `display_name`, `materials[]`, `labor`,
`days`, `prerequisites[]`, `upgrade_target`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 39. APPENDIX Z — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Firings per month | kiln use | KilnSystem |
| Draw grades | quality | KilnSystem |
| Lime stock | mortar readiness | LimeSystem |
| Ceramic output | storage, drainage | CeramicsSystem |
| Mortar stock | build readiness | MasonrySystem |
| Concrete pours | permanence | ConcreteSystem |
| Lining condition | foundry uptime | RefractorySystem |
| Build progress | shelter growth | BuildWorksSystem |
| Weather damage | quality feedback | BuildWorksSystem |
| Fuel consumed | competition | KilnSystem |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 40. APPENDIX AA — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Build results land in the live shelter upgrade path.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §35.
- [ ] Phase 7 soak shows failed firings, weather damage, and the lining cycle.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel foundry, glassworks, sanitation, infrastructure, or build
      system exists.

---

## 41. APPENDIX AB — OPEN QUESTIONS FOR REVIEW

1. Does concrete exist at scale, or only as salvaged cement?
2. Do ceramic sanitary wares count as facility upgrades or facility repairs?
3. Is fireclay a location resource or a trade good? Or both?
4. Does the kiln share the same fuel pool as the foundry and glassworks?
5. Can unfinished builds weather and need rework?
6. Should failed firing grades be sellable, or only usable locally?
7. Does the kiln's draw schedule interrupt the shelter's nightly quiet?
8. Should the test wall produce measurable data the shelter can cite?

None of these may be decided unilaterally; each changes balance and tone.

---

## 42. APPENDIX AC — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Family homes, cradles, hearths |
| 1 | 13 The Faithful | Shrines, altars, ritual vessels |
| 1 | 14 Above the Ash | Landing pads, hardstands |
| 1 | 15 The Deep Root | Greenhouse footings, soil lime |
| 1 | 16 The Rebuilt Body | Sanitary adaptions, ramps |
| 2 | 17 The Long Evening | Hearth, kiln-fired decor |
| 2 | 18 The Underneath | Tunnel linings, vault brick |
| 2 | 19 The Bitter Air | Sealed chambers, lime wash |
| 2 | 20 The Quiet Hand | Hidden rooms, false walls |
| 2 | 21 The Grid | Conduit housings, switch rooms |
| 3 | 22 The Clean Flow | Pipes, sanitary ware, soak pits |
| 3 | 23 The Alarm | Fire walls, chimneys, safe rooms |
| 3 | 24 The Long Goodbye | Memorial masonry |
| 3 | 25 The Iron Road | Rail beds, cuttings, culverts |
| 3 | 26 The Common Table | Ovens, storage jars, crocks |
| 4 | 27 The Thread | Loom weights, dye vats, tiles |
| 4 | 28 The Lesson | Bricks for the schoolhouse |
| 4 | 29 The Glass | Furnace pots, annealing shelves |
| 4 | 30 The Press | Ink pots, type metal trade |

Each hook is additive. The Kiln can ship alone, and every other expansion can
ship without it.

---

## 43. APPENDIX AD — ENDING PROSE SKETCHES

**The Fired Town.** Bricks, tiles, and mortar replace the shelter's improvised
walls, and the next winter is the first one the building does not fight.

**The Kilnworks.** Fired goods leave by cart and return as fuel, tools, and
news, and the kiln runs year after year.

**The Dry Shelter.** Drains flow, chimneys draw, and the damp that ruined
storage and discouraged people is finally fixed.

**The Furnace Saves the Foundry.** New firebrick keeps the metal flowing
through the winter the foundry would otherwise have lost.

**The Cracked Firing.** The kiln fails, the lining spalls, and the shelter
patches what it can and plans a better kiln next year.

**Fade.** The kiln cools and is covered, and repairs go back to salvage, and the
shelter manages, which is not the same as building.

---

## 44. APPENDIX AE — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Instant construction | removes labor | plans, materials, crews |
| Kiln as mana | wrong fiction | fuel, curve, grade |
| Fired goods as loot | no production | clay, form, fire |
| Refractory as decoration | underused | foundry and glass demand |
| Sanitation duplication | authority conflict | facility owner keeps effect |
| Concrete as free upgrade | trivializes | water, cure, failure |
| Mortar as flavor | underused | quality grades, weather |
| Burn injuries as spectacle | tone break | safety and care |
| Build menu as city builder | wrong genre | shelter upgrades only |
| Kiln ignoring fuel politics | hollow | shared fuel economy |

The list exists because construction is the easiest place to accidentally build a
second game. The live upgrade path and the live fuel economy keep the kiln a
worker in the shelter's systems, not a parallel one.

---

## 45. APPENDIX AF — BUILD WORKS MODEL

| Phase | Activity | Materials | Crew | Days |
|---|---|---|---|---|
| Plan | draw and approve | none | 1 | 1–2 |
| Stage | gather and stack | required | 2 | 2–4 |
| Ground | dig and level | none | 2 | 1–3 |
| Build | lay and fit | required | 2–4 | 3–10 |
| Cure | mortar and concrete | none | 0 | 3–21 |
| Inspect | check and fix | minor | 1 | 1 |
| Finish | clean and record | none | 1 | 1 |

Build works is where the expansion's materials become shelter. Every phase is a
real task with a real crew, and the cure phase is the part nobody can rush, which
is the expansion's quiet argument about building well.

---

## 46. APPENDIX AG — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Kiln jobs | 20 | 4,000 |
| Lime rows | 10 | 2,500 |
| Ceramic products | 40 | 8,000 |
| Glaze recipes | 12 | 2,500 |
| Masonry rows | 15 | 3,500 |
| Mortar recipes | 10 | 2,000 |
| Concrete recipes | 10 | 2,500 |
| Refractory rows | 12 | 3,000 |
| Build plans | 20 | 5,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 22 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~64,500** |

---

## 47. APPENDIX AH — WEATHER TEST RESULT TABLE

| Exposure | Cycles | Result | Meaning |
|---|---|---|---|
| Rain | 30 days | runoff | good mix |
| Freeze-thaw | 20 cycles | surface loss | fair |
| Heat | 30 days | stable | good |
| Frost heave | 10 cycles | shift | footing issue |
| Salt | 20 days | spalling | poor mix |
| Wind | 60 days | edge wear | acceptable |
| Smoke | 90 days | blackened | stable |
| Water soak | 60 days | softening | poor |

The test wall is the shelter's laboratory: samples are built, weathered, and
measured, and the results are written down. This is how the kilnworks earns its
standards without pretending to be a laboratory.

---

## 48. APPENDIX AI — DRAINAGE AND CERAMIC PIPE MODEL

| Pipe | Diameter | Use | Slope | Life |
|---|---|---|---|---|
| Field drain | 10 cm | soil | 1% | 10 years |
| Waste pipe | 10 cm | kitchen | 2% | 15 years |
| Sanitary pipe | 15 cm | latrines | 3% | 20 years |
| Storm drain | 20 cm | runoff | 2% | 20 years |
| Culvert | 30 cm | streams | 1% | 25 years |
| Septic feed | 15 cm | soak pit | 3% | 15 years |
| Floor gutter | 10 cm | wash house | 2% | 10 years |
| Vent stack | 10 cm | gas | vertical | 15 years |

Ceramic pipe is where the kiln quietly saves lives: drains that flow end the
water-logged floors, the bad air, and the moisture that ruins stored food and
breeds illness. The expansion ties that directly to the live sanitation and
water systems without owning them.

---

## 49. APPENDIX AJ — KILN WORKS TOOL TABLE

| Tool | Material | Use | Supplier |
|---|---|---|---|
| Trowel | iron, wood | mortar | foundry |
| Brick hammer | iron, wood | brick | foundry |
| Wheelbarrow | wood, iron | haulage | carpentry |
| Screen | wood, wire | sand sieve | carpentry |
| Pottery wheel | wood, stone | throwing | carpentry |
| Kiln door | iron, firebrick | kiln | foundry |
| Rake | iron | draw | foundry |
| Mold | wood, clay | brick | carpentry |
| Level | glass, wood | masonry | glass |
| Test block | clay | testing | kiln |
| Slake vat | wood, lime | slaking | carpentry |
| Grate | fireclay | firing | kiln |

Tools are mostly shared with the foundry and the carpentry shop, which keeps the
expansion inside the shelter's existing workshop culture instead of inventing a
new one.

---

## 50. APPENDIX AK — LOCATION SCAVENGE TABLE

| Location | Find | Rarity | Danger |
|---|---|---|---|
| Brick Ruins | molds, kiln parts | common | collapse |
| Lime Quarry | limestone | abundant | fall |
| Clay Pit | clay, shale | abundant | water table |
| Sand Bank | sand | abundant | none |
| Stone Field | field stone | abundant | none |
| Old Viaduct | cut stone | rare | height |
| Cement Works | cement, forms | rare | dust |
| Charcoal Clamp | charcoal | common | fire |
| Test Wall | data | unique | none |
| Build Site | materials | staging | lifting |

Salvage tables keep the kiln feeling like part of the wasteland rather than a
separate building game. Even the cement works is a place someone walks to with a
barrow and a decision about risk.

---

## 51. APPENDIX AL — TRADE AND PRICE TABLE

| Good | Buyer | Price | Demand |
|---|---|---|---|
| Lime | builders | medium | steady |
| Brick | region | medium | steady |
| Roof tile | towns | medium | seasonal |
| Drain pipe | towns | medium | rising |
| Pottery | households | low | steady |
| Sanitary ware | settlements | high | rising |
| Firebrick | foundries | high | low volume |
| Crucible | foundries | high | steady |
| Floor tile | traders | low | occasional |
| Whitewash | all | low | steady |

Fired goods are heavy and low-margin, which is why the kilnworks mostly serves
the shelter first and trades second. It is the expansion's most regional product
and its least glamorous.

---

## 52. APPENDIX AM — STORYLINE CHAPTER BREAKDOWN

| Beat | Title | Location | Combat | Key decision |
|---|---|---|---|---|
| 1 | The Leak | new wing | none | repair or rebuild |
| 2 | The Survey | region | light | what to dig first |
| 3 | The First Kiln | kiln yard | none | site and scale |
| 4 | The Slake | lime house | none | safety or speed |
| 5 | The First Brick | clay room | none | standard |
| 6 | The Grade | brick yard | none | what to keep |
| 7 | The Mortar | mortar mill | none | mix quality |
| 8 | The Wall | build site | none | pace |
| 9 | The Drain | wet wing | none | replace or patch |
| 10 | The Furnace Lining | foundry | none | downtime |
| 11 | The Footing | greenhouse | none | level and cure |
| 12 | The Chimney | roofs | light | draw or smoke |
| 13 | The Failures | kiln yard | none | salvage or redo |
| 14 | The Winter Test | test wall | none | measure |
| 15 | What We Built | archive | none | final disposition |

The questline is intentionally quiet: the enemy is time, weather, and impatience,
and the stakes are whether the shelter is still improvised next winter.

---

## 53. APPENDIX AN — BUILD CREW TABLE

| Duty | Skill | Fatigue | Notes |
|---|---|---|---|
| Digger | none | high | clay, lime, footings |
| Hauler | none | high | materials |
| Mold maker | 2 | med | brick and tile |
| Kiln tender | 3 | high | firing watch |
| Mason | 3 | med | walls |
| Plasterer | 3 | med | finish |
| Potter | 3 | med | throwing |
| Glazer | 4 | low | glazing |
| Builder | 4 | med | plans and leading |
| Inspector | 4 | low | quality |

A build crew is a shift like any other. The expansion's rule is that every wall
is someone's day, and the roster makes that visible.

---

## 54. APPENDIX AO — FOUNDRY AND GLASS DEPENDENCY TABLE

| Consumer | Refractory need | Wear | Consequence |
|---|---|---|---|
| Cupola | lining, runner | slow | metal stop |
| Glassworks | pot, shelf | fast | glass stop |
| Stove | liner | fast | heat loss |
| Oven | dome brick | medium | kitchen stop |
| Forge | firebox | medium | tool stop |
| Kiln | grate, door | medium | firing stop |
| Boiler | brick | slow | power risk |
| Furnace test | test brick | n/a | data |

The dependency table is the expansion's argument for itself: the shelter's other
industries literally sit on fired clay, and the kiln is what renews it.

---

## 55. APPENDIX AP — PERMANENCE SCORE MODEL

| Structure | Permanence | Maintenance | Lifetime |
|---|---|---|---|
| Tent | 0 | daily | 1 season |
| Timber wall | 2 | seasonal | 5 years |
| Mud brick | 2 | seasonal | 3 years |
| Common brick | 4 | yearly | 20 years |
| Good brick | 5 | yearly | 40 years |
| Stone | 5 | rare | 60 years |
| Concrete | 5 | rare | 50 years |
| Reinforced | 6 | rare | 80 years |
| Tile roof | 4 | yearly | 30 years |
| Earth roof | 2 | seasonal | 5 years |

The permanence score is a design measure, not a hidden stat: it says how often a
structure needs the shelter's attention, and that is the real difference between
a camp and a home.

---

## 56. APPENDIX AQ — OPEN IMPLEMENTATION NOTES

- Lime and clay should be ordinary inventory items with grade metadata, not
  special classes.
- Kiln state should live with the shelter's room model so save/load is natural.
- Firing curves should be data-driven, not code branches per product.
- Build plans should target the live upgrade catalog; no parallel upgrade store.
- Refractory consumption should be registered by the foundry and glassworks when
  they tick, so wear is real.
- Slake and burn injuries must route through the medical pipeline; no local
  health changes.
- The test wall should write a small record the player can read, because the
expansion's whole theme is learning from what the weather does.
- Salvage tables for the viaduct and cement works must use the live scavenging
  model.

---

## 57. APPENDIX AR — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Burn injuries | Gore | Practical, restrained |
| Ruin collapse | Spectacle | Warning and consequence |
| Hard labor | Exploitation | Roster and rotation |
| Apprentices | Child labor | 16+ only, supervised |
| Demolition | Loss | Salvage framed as reuse |
| Old world ruins | Nostalgia | Study, not mourning |
| Housing quality | Class | Shared standards |
| Slake danger | Recklessness | Taught caution |
| Failed builds | Shame | Lessons and rework |
| Permanence | Hubris | Care, not conquest |

The kiln is about making a home, and the expansion's contract is that building
is treated as care and craft, never as domination or disaster tourism.

---

## 58. APPENDIX AS — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Are costs honest? | lifecycle + a11y tests |
| Tone | Is building dignified? | content review |
| Balance | Is the kiln worth feeding? | 360-day soak |

---

## 59. APPENDIX AT — CONTENT VOLUME SUMMARY (FINAL)

| Category | Rows | Prose estimate |
|---|---|---|
| Kiln jobs | 20 | 4,000 |
| Lime rows | 10 | 2,500 |
| Ceramic products | 40 | 8,000 |
| Glaze recipes | 12 | 2,500 |
| Masonry rows | 15 | 3,500 |
| Mortar recipes | 10 | 2,000 |
| Concrete recipes | 10 | 2,500 |
| Refractory rows | 12 | 3,000 |
| Build plans | 20 | 5,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 22 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~64,500** |

---

## 60. APPENDIX AU — FIRST YEAR OF THE KILNWORKS

| Month | Focus | Milestone |
|---|---|---|
| 1 | Clamp and lime | fuel and lime |
| 2 | First bricks | fired units |
| 3 | Mortar | binding works |
| 4 | First wall | repair complete |
| 5 | Drains | water flows |
| 6 | Foundry lining | metal restored |
| 7 | Footing | greenhouse ready |
| 8 | Pottery | storage improved |
| 9 | Tiles | shelter roofed |
| 10 | Concrete | floors poured |
| 11 | Sanitary ware | facilities upgraded |
| 12 | Winter inspection | standards recorded |

A year of the kiln is a year of ordinary things becoming permanent, and the
schedule is the expansion's promise: slow, visible, cumulative.

---

## 61. CLOSING STATEMENT

ASHFALL already tells the story of fired clay, lime, and brick in its narrative
catalogs, and the foundry, glassworks, sanitation, and shelter upgrades all
depend on materials the shelter cannot yet make. What it lacks is the kiln
itself: limestone to lime, clay to brick, fireclay to crucible, and a build works
that turns materials and labor into walls that stay. The Kiln adds that world
without adding a second foundry, glassworks, or construction authority. It adds
the first permanent wall, the drain that finally works, the furnace lining that
saves the metal, and the decision to build slowly because slow is how a camp
becomes a home.

> Wave 4 note: this plan is one of five Wave 4 expansion bibles (27–31). Each is
> self-contained; none requires another to ship. The shared Wave 4 index lives at
> `docs/expansions/wave4/WAVE4_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `CeramicsKilnCatalog`, `MasonryBrickworksCatalog`,
> `kiln_draw_trial_assays.json` (3.3 KB), `lime_kiln_calcination_logs.json`
> (5.8 KB), `mudbrick_weathering_assays.json` (5.2 KB),
> `refractory_firebrick_spalling_logs.json` (5.3 KB),
> `bunker_blueprints_codex.json` (22.7 KB), `weather_hardening_upgrades.json`
> (3.7 KB), and the live foundry, glass, sanitation, and infrastructure owners.