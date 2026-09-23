# ASHFALL — Expansion 59 Design Bible
# THE BONE SHOP
### Wave 10 · Bone, Horn, Antler, Shell, Needles, Handles, Glue, Combs, Scrimshaw, and the Work of Small Hands

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-24
**Domain owners touched:** `Ashfall.Core.Narrative` (`BoneHornCarvingCatalog`), `Ashfall.Core.Crafting` (`CraftingSystem`, `ShelterWorkshopSystem`), `Ashfall.Core.Shelter` (`ShelterDecorSystem` — trophy and display placements)
**Proposed host owner:** `BoneShopHostSession` (extends the workshop and supply surfaces)
**Existing save sections:** `crafting` (workbench jobs and workshop state)
**Existing CLI verbs:** `--data-integrity-selftest`, `--content-utilization-selftest`, `--panel-bind-lifecycle-selftest` (no bonework-specific verb exists)
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already kills, keeps, and uses animals. Expansion 32 owns the wild:
hunting, trapping, migration, and companion animals. Expansion 15 owns
livestock and the vet. The shelter eats what it raises and catches, and then it
throws away the hardest-working materials it owns: the bone, horn, antler, and
shell that a hunting camp uses for needles, handles, and glue. Two items exist
at the end of this chain — `item_surgical_bone_chisel` in the ward's kit and
`item_decor_trophy_deer_antlers` on a wall — and nothing exists between the
carcass and the shelf.

What does not exist: a bone yard, a sorting rule, a degreasing vat, a sawing
bench, a scraping and polishing method, a needle spec, an awl, a hook, a
handle, a button, a comb, a glue batch, or a record. There are no bone
materials in `items.json`. The four authored trade logs sit in the narrative
store unread: `antler_horn_sawing_records.json` (3,650 B: `material_type`,
`saw_tool_id`, `blank_shape_cut`), `bone_degreasing_prep_logs.json` (3,623 B:
`bone_source_animal`, `degreasing_method`, `prep_duration_days`),
`scraping_polishing_reports.json` (3,290 B: `blank_material`, `abrasive_used`,
`surface_finish`), and `needle_awl_hook_assays.json` (3,228 B: `tool_type`,
`bone_blank_id`, `point_angle_degrees`). `BoneHornCarvingCatalog` models all
four (`BoneDegreasingPrepLog`, `AntlerHornSawingRecord`,
`ScrapingPolishingReport`, `NeedleAwlHookAssay`) and is referenced only by the
content-utilization scanner and the journal catalog — never by a system that
makes anything.

**The Bone Shop** is the expansion about the smallest and most patient trade in
the shelter: taking the rigid parts that hunting and herding leave behind and
turning them into needles, awls, handles, buttons, combs, chisels, glue, and
the quiet display of a life that hunts to live.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 32 The Wild (Wave 5) | Hunting, trapping, animals | Uses animal materials after the kill |
| 15 The Deep Root (Wave 1) | Livestock, vet, hides | Uses bones and horn from that stock |
| 27 The Thread (Wave 4) | Leather, textiles | Rigid animal materials only; no second leather |
| 38 The Ward (Wave 6) | Surgery, instruments | Supplies fine bone points; no surgery |
| 58 The Joinery (Wave 10) | Feed | Supplies bone glue; no wood authority |
| 60 The Wick (Wave 10) | Lamps and light | Supplies horn panes; no light authority |
| 10 The Silent Foundry (Exp 10) | Metal tools | Wood and bone handles for its heads |
| 39 The Reagent (Wave 6) | Chemistry | Degreasers and soaps come from it |
| 47 The Brigade (Wave 8) | Fire and boil safety | Glue boiling routes through its rules |
| `ShelterDecorSystem` | Display and placement | Supplies mounts; no second decor |
| `CraftingSystem` | Recipes and jobs | Adds bone-family recipes |
| 24 The Long Goodbye (Wave 3) | Human remains and mourning | **Human remains are never materials** |
| 13 The Faithful (Wave 1) | Belief and rites | Handling rules only; no doctrine |
| `StandingRecord` | Records | Files the trade log |

**Hard contract: human remains are never worked, traded, or displayed in this
expansion.** The memorial and burial owners hold every question of the dead;
the bone shop's material rule is animal only, and the rule is written into the
data, not just the prose.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's ward cannot suture without a needle that will hold an edge, the
thread trade is down to its last three awls, the joinery cooks its glue from
whatever hides are left, and the hunting camp brings back beautiful antler that
gets thrown on the bone heap and burned. The heap itself is a mess of old
marrow, wet horn, cracked shell, and one femur that the sorter is afraid to
touch because nobody ever wrote down what happened to the rest of that animal.

**The Bone Shop** is the expansion about the small trade that turns a kill's
hard parts into the fine things a shelter cannot make any other way: needles,
awls, hooks, buttons, combs, handles, chisels, glue, and the modest showpieces
that remember what fed the winter. It is about patient hands, sharp angles,
clean vats, and the plain ethics of using what an animal left behind without
ever touching what belongs to a person.

### 1.2 The five loops it adds

```
  Gather ──► Sort ──► Clean ─► Shape ──► Finish
     │         │        │         │          │
     ▼         ▼        ▼         ▼          ▼
  sources   grades   vat, dry  saw, carve  scrape, polish
                                              │
                                              ▼
                          Assemble ──► Use ──► Record
```

### 1.3 What the player manages

1. **The yard.** What arrives, from whom, and with what permission.
2. **The sorting.** Grades: bone, horn, antler, shell, tooth.
3. **The vat.** Degreasing, drying, and the smell nobody enjoys.
4. **The blanks.** Sawing shapes for each intended tool.
5. **The bench.** Carving, scraping, polishing, and finish.
6. **The tools.** Needles, awls, hooks, handles, buttons, combs, chisels.
7. **The pot.** Glue batches and their strength.
8. **The display.** Mounts, memory pieces, and the wall they belong on.
9. **The rules.** Animal-only sourcing, permission, and handling.
10. **The records.** Batches, blanks, points, and lessons.

### 1.4 What it is not

- Not a hunting, trapping, or livestock system.
- Not a leather or textile system; cloth and hide stay with expansion 27.
- Not a medical system; the ward owns surgery.
- Not a chemistry system; degreasers and soaps come from expansion 39.
- Not a decor system; placements stay with `ShelterDecorSystem`.
- Not a memorial system; **human remains are out of scope entirely**.
- Not a wood system; handles are supplied, not joined.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | Recipes and jobs | `LIVE` |
| `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs` | Bench and condition | `LIVE` |
| `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs` | Placement | `LIVE` |
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` | Tool wear | `LIVE` |
| `Assets/Ashfall.Core/Ecology/*` | Animal sources | `LIVE` (boundary) |
| `Assets/Ashfall.Core/Farming/*` | Livestock sources | `LIVE` (boundary) |
| `Assets/Ashfall.Core/Medical/*` | Ward instruments | `LIVE` (boundary) |
| `NeedsSystem` | Morale sink | `LIVE` |
| `StandingRecord` | Records | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Rows | Notes |
|---|---|---|
| `antler_horn_sawing_records.json` | 4+ entries, 3,650 B | sawing and shapes; unread |
| `bone_degreasing_prep_logs.json` | 4+ entries, 3,623 B | source, method, days; unread |
| `scraping_polishing_reports.json` | 4+ entries, 3,290 B | finishes; unread |
| `needle_awl_hook_assays.json` | 4+ entries, 3,228 B | point angles; unread |
| Bone, horn, shell catalogs | **0** | confirmed absent |
| Bone materials in `items.json` | **0** | `item_surgical_bone_chisel` and `item_decor_trophy_deer_antlers` only |

### 2.3 Confirmed gaps

- **GAP-59-1 — No bone, horn, antler, or shell material grades.**
- **GAP-59-2 — No sourcing or permission rules for animal remains.**
- **GAP-59-3 — No degreasing, drying, or vat practice.**
- **GAP-59-4 — No sawing of blanks or tool shapes.**
- **GAP-59-5 — No scraping, polishing, or finish standards.**
- **GAP-59-6 — No needle, awl, or hook production with point angles.**
- **GAP-59-7 — No handles, buttons, combs, or small goods.**
- **GAP-59-8 — No bone glue and no supply to the joinery.**
- **GAP-59-9 — No mounts, memory pieces, or display sourcing.**
- **GAP-59-10 — Four authored bonework logs are read by nothing.**

### 2.4 Non-duplication statement

This expansion adds **no** second hunting, livestock, leather, medical,
chemistry, fire, decor, or record system. It consumes animal sources from
expansions 32 and 15, supplies points to 27 and the ward, glue to 58, horn
panes to 60, handles to 10 and 58, buys degreasers from 39, routes boiling
through 47, places displays through `ShelterDecorSystem`, and files its records
with `StandingRecord`. All new state is additive inside the existing `crafting`
save owner. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Nothing edible dies twice.** A kill is a meal and then a set of
tools, and wasting the second half wastes the animal.

**Pillar 2 — Small hands, fine points.** A needle is the hardest small object
in the shelter and the one the ward cannot do without.

**Pillar 3 — Clean work is the whole trade.** Degreasing and drying are
unromantic and they are what separates a tool from a hazard.

**Pillar 4 — Permission is part of sourcing.** Whose animal, whose herd, whose
kill, and who agreed — the question is asked before the saw.

**Pillar 5 — A person is not a material.** Human remains belong to the people
who grieve them, and this trade never touches them.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Yard | Sorting, grades, honesty | Horror heaps |
| Degreasing | Plain work, smell jokes | Gross-out |
| Sawing | Shapes, kerfs, restraint | Trophy gore |
| Needles | Angles, tests, ward need | Prestige objects |
| Glue | Pots, strength, supply | Alchemy |
| Display | Memory, restraint, wall | War trophies |
| Sourcing | Permission, records | Poaching or theft |
| Remains | Animal only, written rule | Any human remains |

### 3.3 Content limits

- No real boneworking traditions, cultures, or named crafts copied.
- No human remains in any recipe, item, display, or record.
- No slaughter, butchering, or kill description beyond sourcing lines.
- No torture, trophy-of-war, or body-part collection framing.
- No magical bone, no shamanic charge, no ritual use; belief stays with 13.
- No poison or caustic glamour; degreasers are ordinary workshop goods.
- No new save section.

---

## 4. THE BONE SHOP WORLD

### 4.1 Interior rooms

- **`room_bone_yard`** — the sorting bay, buckets, and the arrival board.
- **`room_degreasing_vat`** — the vat, the rinse, and the drying racks.
- **`room_bone_saw_bench`** — the vise, the saws, the kerf gauge.
- **`room_carving_bench`** — blades, files, scrapers, and the light.
- **`room_polish_wheel`** — abrasive, cloth, and the finish shelf.
- **`room_glue_pot`** — the pot, the forms, and the strength test bar.
- **`room_small_goods`** — buttons, combs, and the count board.
- **`room_mount_room`** — mounts, plaques, and the vent.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_bone_beach` | The Bone Beach | 5 | Shell and drift material |
| `loc_kill_line` | The Kill Line | 5 | Hunt delivery and sorting |
| `loc_antler_shed` | The Shed Stand | 4 | Cast antler collection |
| `loc_horn_pasture` | The Horn Pasture | 4 | Livestock horn sources |
| `loc_shell_bar` | The Shell Bar | 4 | River shell and lime feed |
| `loc_old_shop_ruin` | The Old Curio Shop | 6 | Pre-war scrimshaw and tools |
| `loc_glue_shed` | The Glue Shed | 5 | Rendering yard and fire rules |
| `loc_mount_wall` | The Mount Wall | 6 | Display outdoors and lessons |
| `loc_grave_margin` | The Grave Margin | 7 | Boundary: what is never taken |
| `loc_point_stones` | The Point Stones | 3 | River stones for polishing |

All locations must resolve in `locations.json` and pass the map loader gate.

### 4.3 The rhythm

Sourcing follows the hunting and herding seasons; degreasing follows the vats;
sawing and carving fill the winter bench; finishing and assembly run through
the spring; glue boils in cool weather; displays wait for a year before they
are mounted. The trade's clock is the carcass.

---

## 5. MAIN STORYLINE — "THE WORK OF SMALL HANDS"

### 5.1 Central conflict

**Nessa Clave** carves bone and knows the ward needs chisels and the thread
trade needs awls, and gets both requests denied because "it's only bone."
**Weft Carrow** makes needles and has three left. **Rane Pellow** works horn
and wants to cut panes for lanterns, which nobody takes seriously until the
grid dies and the hall needs light. **Ottiline Sward** boils glue and is told
her pot is a smell, not a supply line, until the joinery runs out of hide glue
mid-repair. **Jora Shaw** sorts the yard and keeps finding material mixed with
things that should never have been on the heap. **Tarn Flint** finds a pre-war
scrimshaw set in a curio shop and brings back a question the shelter is not
ready for: what do you do with beautiful work from a world that is gone?

Then a winter ward crisis: the shelter's last surgical needle breaks during a
suturing, and the only person who can replace it is Weft, at a bench, with a
femur, a saw, and a point-angle gauge. The replacement takes six hours and
holds. The ward stops treating bonework as a hobby, the yard gets rules, the
vat gets a rota, and Nessa's first sorted batch supplies eleven tools in a
month.

The expansion's question: **what is the smallest work that keeps a shelter
alive?**

### 5.2 Theme (unspoken)

**The dignity of a place is in the tools nobody thinks about until they break.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_carver_nessa_clave` | Nessa Clave | Bone carver | Bench, chisels, shapes |
| `npc_needlemaker_weft_carrow` | Weft Carrow | Needle maker | Needles, angles, ward need |
| `npc_hornworker_rane_pellow` | Rane Pellow | Horn worker | Panes, combs, handles |
| `npc_gluemaker_ottiline_sward` | Ottiline Sward | Glue maker | Pots, strength, supply |
| `npc_scrimshaw_tarn_flint` | Tarn Flint | Scrimshaw | Memory work, restraint |
| `npc_sorter_jora_shaw` | Jora Shaw | Sorter | Grades, yard, rules |
| `npc_apprentice_sedge_hann` | Sedge Hann | Apprentice | Scraping, polishing |
| `npc_liaison_tansy_mord` | Tansy Mord | Sourcing liaison | Permission, records |

### 5.4 Story beats (15)

1. **The Bone Heap.** The yard is a mess and nobody owns it.
2. **Three Needles.** Weft counts what the ward has left.
3. **The Permission.** Tansy writes the first sourcing rule.
4. **The Vat.** Degreasing becomes a rota instead of a chore.
5. **The Shed Stand.** Cast antler is gathered cleanly for the first time.
6. **The Kerf.** Nessa saws the first true blanks.
7. **The Angles.** Needle points are gauged, tested, and recorded.
8. **The Break.** The last surgical needle fails mid-suture.
9. **Six Hours.** A replacement is made and holds.
10. **The Pot.** Glue runs to the joinery in measured strength.
11. **The Pane.** Rane cuts horn for the lanterns.
12. **The Buttons.** Small goods reach the quarter's coats.
13. **The Curio.** Scrimshaw is displayed with a spoken restraint.
14. **The Margin.** The grave margin is marked and explained.
15. **The Work of Small Hands.** The shop becomes a scheduled trade.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Sourcing | all sources / permitted only / own stock | ethics |
| Degreasing | water vat / soap wash / solvent | cleanliness |
| Bench | needles first / handles first / small goods | priority |
| Glue | strong / general / none | supply |
| Display | wall / case / private | memory |
| Scrimshaw | copy / honor / store | heritage |
| Rules | written / spoken / implied | clarity |
| Final | scheduled trade / apprentice shop / single bench | identity |

### 5.6 Endings (5 + fade)

1. **The Fine Point** — needles and awls are made on schedule, and the ward
   and the thread trade never wait again.
2. **The Full Pot** — glue runs to the joinery every cool month, and the
   mended furniture stops being a joke.
3. **The Pane Light** — horn panes light the hall through the dark weeks, and
   the trade that seemed marginal becomes the one that carries winter.
4. **The Quiet Display** — mounts and one scrimshaw panel hang with a spoken
   rule about memory, restraint, and the dead.
5. **The Sorted Yard** — every material in the yard is graded, permitted, and
   marked, and the shop schedules its own work.
6. **Fade** — a bone needle in a ward kit, a bowl of buttons on a count board,
   and a horn pane glowing where the electric light used to be.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_bone_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_bone_heap`, `quest_bone_three_needles`, `quest_bone_permission`,
`quest_bone_vat`, `quest_bone_shed_stand`, `quest_bone_kerf`,
`quest_bone_angles`, `quest_bone_break`, `quest_bone_six_hours`,
`quest_bone_pot`, `quest_bone_pane`, `quest_bone_buttons`,
`quest_bone_curio`, `quest_bone_margin`, `quest_bone_small_hands`.

### 6.2 Side quests (30)

**Sourcing (5)**
- `quest_bone_sources` — sources listed
- `quest_bone_permission` — permission forms kept
- `quest_bone_kill_line` — hunt delivery sorted
- `quest_bone_pasture` — livestock horn collected
- `quest_bone_beach` — shell gathered

**Yard (5)**
- `quest_bone_sort` — grades separated
- `quest_bone_mark` — materials marked
- `quest_bone_reject` — rejects pulled
- `quest_bone_store` — dry store kept
- `quest_bone_ledger` — yard count written

**Clean (5)**
- `quest_bone_degrease` — vat run
- `quest_bone_rinse` — rinse changed
- `quest_bone_dry` — racks turned
- `quest_bone_smell` — vent checked
- `quest_bone_clean_check` — clean standard

**Shape (5)**
- `quest_bone_saw` — blanks sawn
- `quest_bone_carve` — shapes carved
- `quest_bone_scrape` — surfaces scraped
- `quest_bone_polish` — finishes done
- `quest_bone_fit` — test fit

**Goods (5)**
- `quest_bone_needle` — needle made
- `quest_bone_awl` — awl made
- `quest_bone_handle` — handle fitted
- `quest_bone_button` — buttons counted
- `quest_bone_comb` — comb finished

**Pot and record (5)**
- `quest_bone_pot_run` — glue boiled
- `quest_bone_strength` — test bar broken
- `quest_bone_supply` — joinery supplied
- `quest_bone_record` — batch recorded
- `quest_bone_lesson` — lesson kept

### 6.3 Repeatable quests (8)

`quest_bone_repeat_sort`, `quest_bone_repeat_vat`,
`quest_bone_repeat_saw`, `quest_bone_repeat_needle`,
`quest_bone_repeat_pot`, `quest_bone_repeat_polish`,
`quest_bone_repeat_count`, `quest_bone_repeat_teach`.

### 6.4 Dynamic hooks

Live events (hunts, herding, ward demand, joinery repair, workshop jobs, fire
rules, decor placement) attach authored follow-ups through existing seams.
No new event bus.

### 6.5 Constraints

- Recipes and jobs stay with `CraftingSystem`.
- Animal sources stay with expansions 32 and 15.
- The ward owns surgery; this plan supplies points only.
- The joinery owns wood; this plan supplies glue.
- Decor placement stays with `ShelterDecorSystem`.
- Boiling routes through expansion 47's fire rules.
- **Human remains never enter any recipe, item, or record.**
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `BoneYardSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** sourcing, arrival, grading, marking, storage, and the yard ledger.
**Consumes:** hunt and herd deliveries, permission records. **Data:**
`bone_sources.json`, `bone_materials.json`, `yard_ledger.json`. **Rules:**
every material has a source and a permission; grades are visible; rejects are
pulled and destroyed; the yard is counted.

### 7.2 `DegreasingSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** vats, degreasers, rinse cycles, drying racks, and the clean
standard. **Consumes:** soap from expansion 39, water, workshop. **Data:**
`degreasing_methods.json`. **Rules:** clean work is the whole trade; a vat
that is not changed is a hazard; drying is measured in days.

### 7.3 `BoneSawSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** sawing: vises, fine saws, kerf, blank shapes, and waste. **Consumes:**
materials, `EquipmentConditionSystem`, the bench. **Data:**
`blank_shapes.json`. **Rules:** cut for the tool, not for the material; a
poor blank is remade; sawdust and offcuts are collected for the pot.

### 7.4 `CarvingSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** carving, scraping, polishing, finish standards, and test fits.
**Consumes:** blanks, abrasives, point stones. **Data:**
`carving_finishes.json`, `point_angles.json`. **Rules:** every point has a
specified angle; finishes are graded; a tool that fails its test is not
issued.

### 7.5 `BoneToolSystem` (new, `Ashfall.Core.Crafting`)

**Owns:** the small goods catalog: needles, awls, hooks, handles, buttons,
combs, chisels, gauges. **Consumes:** finished pieces, requester demand.
**Data:** `bone_tools.json`. **Rules:** the ward's request comes first; every
issued tool carries a batch mark; handles are fitted, not forced.

### 7.6 `GlueSystem` (new, thin, `Ashfall.Core.Crafting`)

**Owns:** pots, boiling, forms, strength test bars, and supply. **Consumes:**
offcuts, water, fire rules, the joinery's demand. **Data:**
`glue_batches.json`. **Rules:** glue is boiled only under fire rules; strength
is tested and recorded; supply is measured, not promised.

### 7.7 `MountSystem` (new, thin, `Ashfall.Core.Shelter`)

**Owns:** mounts, plaques, memory pieces, and display eligibility. **Consumes:**
`ShelterDecorSystem`, the curio finds. **Data:** `mounts.json`. **Rules:** a
mount names the animal and the year; display is restrained; nothing human is
ever mounted; scrimshaw from the old world is honored or stored, never
copied for sale.

### 7.8 `RemainsRuleSystem` (new, thin, `Ashfall.Core.Narrative`)

**Owns:** the written sourcing and handling rules: animal-only, permission,
respect, the grave margin, and the refuse rule. **Data:** `remains_rules.json`.
**Rules:** the rules are visible in the shop; the grave margin is never
crossed; any ambiguity stops the batch and goes to the sourcing liaison.

### 7.9 Systems explicitly not added

- No second hunting, livestock, leather, medical, chemistry, fire, decor,
  or record system.
- No human remains, ever.
- No magical bone, ritual charge, or belief mechanics.
- No kill or butchery descriptions.
- No new RNG stream beyond the live tick and demand paths.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `bone_materials.json` (new)

```json
{
  "schema_version": 1,
  "materials": [
    {
      "material_id": "mat_bone_long",
      "display_name": "Long Bone",
      "kind": "bone",
      "source_animal": "deer",
      "grade": "tool_stock",
      "clean_days": 6,
      "uses": ["needles", "chisels", "handles"],
      "tags": ["hunt"]
    }
  ]
}
```

### 8.2 `bone_sources.json` (new)

Sources: source kind, animal, provider, permission kind, season, volume.

### 8.3 `degreasing_methods.json` (new)

Methods: water, soap, solvent; duration, water, risk, clean level.

### 8.4 `blank_shapes.json` (new)

Shapes: tool target, blank dimensions, kerf, waste use.

### 8.5 `carving_finishes.json` (new)

Finishes: scraped, sanded, polished, burnished; use, look, time.

### 8.6 `point_angles.json` (new)

Angles: tool, angle degrees, test, use, failure note.

### 8.7 `bone_tools.json` (new)

Tools: needle, awl, hook, handle, button, comb, chisel; specs and demand.

### 8.8 `glue_batches.json` (new)

Batches: source, boil hours, strength, dry days, supply target.

### 8.9 `mounts.json` (new)

Mounts: piece, animal, year, maker, display rule, story line.

### 8.10 `remains_rules.json` (new)

Rules: animal-only, permission, margin, refuse, review, keeper.

### 8.11 Items

New items appended to `items.json`: `item_bone_blank`,
`item_antler_blank`, `item_horn_blank`, `item_shell_blank`,
`item_bone_needle`, `item_bone_awl`, `item_bone_hook`,
`item_bone_handle`, `item_bone_button`, `item_bone_comb`,
`item_bone_gauge`, `item_glue_batch`, `item_glue_test_bar`,
`item_antler_mount`, `item_scrimshaw_panel`, `item_horn_pane`,
`item_degreasing_vat_kit`, `item_point_stone`.

**Validation rule.** Every material, source, batch, and mount is validated
animal-only at load: a fixture whose source resolves to human remains fails
the integrity gate, and an ambiguous source fails closed. The rule lives in
the validator as well as in `remains_rules.json`, so it cannot be relaxed by
editing prose.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`crafting` remains the live save owner. The yard ledger, vat state, blank
inventory, tool batches, glue batches, mounts, and the rules acknowledgment
are additive sub-objects. No new save section.

### 9.2 State to persist

- Materials held, grade, and source permission.
- Vat charges, rinse cycles, and drying progress.
- Blanks sawn and their shapes.
- Carved pieces, finishes, and test results.
- Tool batches issued and their marks.
- Glue batches, strength, and supply.
- Mounts and display placements.
- Rule acknowledgments and the yard count.

### 9.3 Determinism

- Sourcing volumes follow the live hunt and herd yields.
- Degreasing and drying follow authored day counts.
- Point angles and test results are deterministic comparisons.
- Glue strength is authored per batch source and boil hours.
- No seeded noise in any conversion; the same save makes the same needle.
- Paired replay hashes must match; no `System.Random`.

### 9.4 Migration

Legacy saves load with an unsorted yard and empty batches; existing wards keep
their surgical chisels, and the new supply line starts from zero. The rules
page loads with defaults acknowledged by the sourcing liaison. No material
grants are retroactive.

### 9.5 Checksum

Invariant-culture floats for angles and strength; integer counts, days, and
grades.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `BoneShopPanel` (new) | Yard, vat, bench, batches | `BoneShopHostSession` |
| `BoneYardPanel` (new) | Sourcing and grades | same |
| `DegreasingPanel` (new) | Vat and drying | same |
| `CarvingBenchPanel` (new) | Blanks, angles, finishes | same |
| `SmallGoodsPanel` (new) | Tools, counts, issues | same |
| `GluePotPanel` (new) | Batches and supply | same |
| `MountPanel` (new) | Displays and rules | same |
| `ShelterWorkshopPanel` (extend) | Bone-family jobs | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Cleanliness is shown as words and numbers, never color alone.
- The animal-only rule is always visible in the shop's header.
- Refusals and rejections show their reason.
- Keyboard/controller close/back preserved; focus maintained on refresh.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a fine saw entering bone, a scraper
finishing a surface, a pot coming to a low boil, buttons poured onto a count
board. No cue is required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `CraftingSystem` | Bone-family recipes and jobs |
| `ShelterWorkshopSystem` | Bench condition |
| 32 The Wild (Wave 5) | Animal material supply |
| 15 The Deep Root (Wave 1) | Livestock horn and bone |
| 27 The Thread (Wave 4) | Awls and needles |
| 38 The Ward (Wave 6) | Fine chisels and surgical points |
| 58 The Joinery (Wave 10) | Glue supply |
| 60 The Wick (Wave 10) | Horn panes and lanterns |
| 10 Silent Foundry (Exp 10) | Metal saws and files |
| 39 The Reagent (Wave 6) | Degreasers and soap |
| 47 The Brigade (Wave 8) | Boiling and fire rules |
| 24 The Long Goodbye (Wave 3) | **The hard boundary on human remains** |
| `ShelterDecorSystem` | Mount placement |
| `EquipmentConditionSystem` | Saw and tool wear |
| `NeedsSystem` | Comfort via `Modify` |
| `StandingRecord` | Batch and yard records |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm crafting, workshop, decor, animal
source, leather, ward, joinery, chemistry, fire, and record owners. Record
file:line; change nothing.

**Phase 1 — Data + validators.** Author the ten catalogs; register validators
and scanner.

**Wiring note (foreman-critical).** The animal-only rule must be enforced by
the integrity gate, not only by the prose. The catalog validators for
`bone_materials.json`, `bone_sources.json`, `glue_batches.json`, and
`mounts.json` must reject any entry whose `source_animal` or provider resolves
to a human-remains kind, and `RemainsRuleSystem` must fail closed on an
ambiguous source rather than queueing a batch. A focused validator test must
prove the rejection with a deliberately invalid fixture, so the rule cannot be
edited out of the data without failing the gate.

**Phase 2 — Pure Core.** `BoneYardSystem`, `DegreasingSystem`, `BoneSawSystem`,
`CarvingSystem`, `BoneToolSystem`, `GlueSystem`, `MountSystem`,
`RemainsRuleSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `BoneShopHostSession`, focused selftest coverage,
fresh journey from the bone heap to the needle that saves the ward.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** Multi-season soak: sourcing volume, vat throughput,
tool demand, glue supply, mount restraint.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Materials | 14 |
| Sources | 10 |
| Methods | 6 |
| Shapes | 12 |
| Finishes | 6 |
| Angles | 10 |
| Tools | 14 |
| Glue batches | 8 |
| Mounts | 10 |
| Rules | 6 |
| Items | 18 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 60,000–75,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Human remains accident | Critical | Written rule, validation |
| Hunting overlap | High | After-kill only |
| Leather overlap | High | Rigid materials only |
| Ward overlap | High | Supply, not surgery |
| Fire incident | High | Boil rules from 47 |
| Tone (gross-out) | High | Plain work tone |
| Determinism break | Low | Authored conversions |
| Tiny-domain thinness | Medium | Deep tool chain |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `bone_materials.json` | 14 | 3,500 |
| `bone_sources.json` | 10 | 2,500 |
| `degreasing_methods.json` | 6 | 2,000 |
| `blank_shapes.json` | 12 | 3,000 |
| `carving_finishes.json` | 6 | 2,000 |
| `point_angles.json` | 10 | 2,500 |
| `bone_tools.json` | 14 | 3,000 |
| `glue_batches.json` | 8 | 2,000 |
| `mounts.json` | 10 | 2,500 |
| `remains_rules.json` | 6 | 2,000 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 18 | 3,000 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~56,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R59-1 | Human remains | Low | Critical | Hard rule |
| R59-2 | Hunting overlap | Med | High | Boundary |
| R59-3 | Leather fork | Med | High | Material scope |
| R59-4 | Ward fork | Med | High | Supply only |
| R59-5 | Fire risk | Low | High | Boil rules |
| R59-6 | Gross-out tone | Med | High | Plain work |
| R59-7 | Tiny scope | Med | Medium | Deep chain |
| R59-8 | Determinism | Low | High | Authored |
| R59-9 | Trophy framing | Low | High | Restraint rule |
| R59-10 | Content overrun | Med | Medium | Budget |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Who may take material from a hunt?** Recommended: the hunt owner's
   permission with a written slip, kept in the yard ledger.
2. **Are degreasers bought or made?** Recommended: soap from expansion 39,
   water from the shelter; no new chemistry.
3. **What happens to a rejected batch?** Recommended: destroyed under the fire
   rules, recorded, never re-entering the yard.
4. **Are mounts public or private?** Recommended: public walls by default with
   a private option for a family's own piece.
5. **What is the status of pre-war scrimshaw?** Recommended: honored or
   stored; never copied for sale, never used as currency.

---

## 17. APPENDIX D — MATERIAL TABLE

| # | Material | Kind | Source | Grade | Clean days | Uses |
|---|---|---|---|---|---|---|
| 1 | Long bone | bone | deer, cattle | tool stock | 6 | needles, chisels |
| 2 | Flat bone | bone | shoulder cuts | plate stock | 5 | buttons, gauges |
| 3 | Rib | bone | deer, pig | blade stock | 5 | scrapers, hooks |
| 4 | Femur head | bone | cattle | knob stock | 8 | handles, mallets |
| 5 | Antler beam | antler | cast sheds | rod stock | 10 | handles, hooks |
| 6 | Antler crown | antler | cast sheds | display stock | 2 | mounts |
| 7 | Horn core | horn | cattle, goat | sleeve stock | 12 | panes, combs |
| 8 | Horn shell | horn | cattle, goat | sheet stock | 14 | lantern panes |
| 9 | Hoof | horn | cattle, horse | small stock | 7 | buttons, pegs |
| 10 | Shell | shell | river bar | plate stock | 3 | lime feed, inlay |
| 11 | Oyster shell | shell | river bar | lime feed | 1 | kiln feed |
| 12 | Teeth | tooth | boar, deer | small stock | 4 | toggles, inlay |
| 13 | Sinew scrap | sinew | hunt trim | cord stock | 3 | lashings (27) |
| 14 | Bone charcoal | char | rejected | fuel stock | 0 | filters (22) |

Fourteen materials and every one of them is something the shelter currently
burns or buries. The fifth row, cast antler, is the trade's kindest source: a
deer sheds its antlers every year, so the yard can gather rod stock without
taking anything from the animal that grew it. The fourteenth row pairs the
shop with water treatment, because charred bone filters water and the reject
bin is not waste.

---

## 18. APPENDIX E — SOURCE TABLE

| # | Source | Kind | Provider | Permission | Season | Volume |
|---|---|---|---|---|---|---|
| 1 | Hunt line | deer | hunting camp | slip | autumn | high |
| 2 | Herd cull | cattle | livestock | slip | winter | medium |
| 3 | Goat horn | goat | livestock | standing | spring | low |
| 4 | Cast antler | deer | wild walk | none needed | spring | medium |
| 5 | River shell | shell | river crew | none needed | summer | high |
| 6 | Boar jaw | boar | hunt line | slip | autumn | low |
| 7 | Horse hoof | horse | stable | standing | as shed | low |
| 8 | Curio finds | mixed | salvage | finder's share | any | rare |
| 9 | Neighbor herd | cattle | trade | trade slip | any | low |
| 10 | Triage cache | bone | ward | transfer note | any | rare |

Ten sources and the fourth is the trade's cleanest: nobody owns a shed
antler, so it needs no permission, only a walk in the right season. The tenth
row is a supply of last resort that the rules handle carefully — bone pieces
released by the ward are only ever animal material, and the transfer note
says so.

---

## 19. APPENDIX F — DEGREASING TABLE

| # | Method | Duration | Water | Clean level | Risk |
|---|---|---|---|---|---|
| 1 | Cold water soak | 14 d | high | basic | smell |
| 2 | Warm soak | 7 d | high | fair | smell |
| 3 | Soap wash | 4 d | medium | good | none |
| 4 | Soap and ash | 3 d | medium | good | caustic |
| 5 | Solvent wash | 1 d | low | best | fire |
| 6 | Boiling | 1 d | medium | fast | fire |

Six methods and the yard's rota reads left to right down the table as the shop
gets better at its work: the story opens with a cold soak that reeks through
the whole corridor, and the shop's first real reform is the soap wash, which
takes four days and does not make the quarter complain. The boiling row is
kept for small urgent batches because it destroys fine bone, and the fire
rules are read before the pot goes on.

---

## 20. APPENDIX G — BLANK SHAPE TABLE

| # | Shape | Target | Dimensions | Kerf | Waste use |
|---|---|---|---|---|---|
| 1 | Needle blank | needle | 70×6 mm | fine | polish powder |
| 2 | Awl blank | awl | 90×8 mm | fine | handles |
| 3 | Hook blank | fishing hook | 50×5 mm | fine | none |
| 4 | Handle blank | knife handle | 110×20 mm | coarse | pegs |
| 5 | Button blank | button | 22×4 mm | fine | inlay |
| 6 | Comb blank | comb | 100×30 mm | medium | buttons |
| 7 | Chisel blank | chisel | 80×8 mm | fine | none |
| 8 | Gauge blank | point gauge | 60×10 mm | fine | scraped down |
| 9 | Pane blank | horn pane | 150×120 mm | coarse | lanterns |
| 10 | Toggle blank | clothing toggle | 40×10 mm | fine | none |
| 11 | Mount plinth | display | varies | medium | firewood |
| 12 | Test bar | glue test | 100×20 mm | coarse | firewood |

Twelve shapes and the eleventh and twelfth are the trade's honesty: plinths
and test bars are made from the same stock as everything else, and a shop that
will not cut a test bar does not know what its glue will do. The first row is
the whole story compressed into a rectangle seventy millimetres long.

---

## 21. APPENDIX H — POINT ANGLE TABLE

| # | Tool | Angle | Test | Use | Failure note |
|---|---|---|---|---|---|
| 1 | Suture needle | 12° | bend test | ward | too fine shatters |
| 2 | Sewing needle | 15° | bend test | thread trade | tip curls |
| 3 | Leather awl | 20° | pierce test | leather | tip blunts |
| 4 | Cord awl | 25° | pierce test | cordage | shaft splits |
| 5 | Fishing hook | 30° | pull test | river crew | barb snaps |
| 6 | Buttonhole awl | 18° | pierce test | quarter | tip skids |
| 7 | Marking point | 22° | line test | joinery (58) | dulls fast |
| 8 | Surgical chisel | 35° | cut test | ward | edge rolls |
| 9 | Scraper edge | 45° | scrape test | bench | chatters |
| 10 | Gauge point | 10° | check test | bench | touches dull |

Ten angles and the first row is the expansion's climax: a twelve-degree
suture needle is the most delicate object the shelter can make, and it is
made at a bench by a person with a saw and a gauge because the ward has run
out. The test column is not decoration — every issued point has passed its
test, and the batch mark on the needle says which hand made it.

---

## 22. APPENDIX I — TOOL DEMAND TABLE

| # | Tool | Requester | Rate | Batch | Substitute |
|---|---|---|---|---|---|
| 1 | Suture needle | ward | 2/month | 6 | none acceptable |
| 2 | Sewing needle | thread | 4/month | 12 | thorns, briefly |
| 3 | Leather awl | leather | 1/month | 6 | metal awl |
| 4 | Cord awl | cordage | 1/quarter | 3 | spliced stick |
| 5 | Button | quarter | 20/season | 60 | wooden discs |
| 6 | Comb | quarter | 4/season | 12 | wooden comb |
| 7 | Knife handle | workshop | 2/season | 6 | wood |
| 8 | Chisel | ward | 1/quarter | 4 | metal |
| 9 | Hook | river crew | 10/season | 30 | metal hook |
| 10 | Gauge point | bench | 1/month | 4 | metal |
| 11 | Toggle | quarter | 8/season | 20 | wood |
| 12 | Pane | wick (60) | 6/season | 8 | none |
| 13 | Test bar | glue | 4/month | 16 | reuse |
| 14 | Marking point | joinery | 1/quarter | 3 | chalk |

Fourteen demand lines and the first is the only one with no substitute: a
suture needle is why the shop exists. The fifth and sixth rows show the trade
becoming ordinary — buttons and combs are the small goods that make the shop
part of daily life, and their absence is only noticed by the people wearing
the coats and combing their hair.

---

## 23. APPENDIX J — GLUE TABLE

| # | Batch | Source | Boil hours | Strength | Dry days | Supply |
|---|---|---|---|---|---|---|
| 1 | G-01 | hide trim | 6 | weak | 3 | bench |
| 2 | G-02 | hide trim | 9 | fair | 4 | joinery |
| 3 | G-03 | sinew scrap | 5 | strong | 3 | joinery |
| 4 | G-04 | bone offcut | 12 | strong | 5 | joinery |
| 5 | G-05 | mixed | 8 | fair | 4 | workshop |
| 6 | G-06 | sinew scrap | 5 | strong | 3 | ward splints |
| 7 | G-07 | hide trim | 10 | strong | 4 | joinery |
| 8 | G-08 | test batch | 4 | weak | 2 | none |

Eight batches and the eighth exists to fail: the shop keeps one deliberately
weak batch per season to test its own standards, because a glue maker who has
never seen a bad batch cannot judge a good one. The fourth row is the trade's
quiet crossover — bone offcut glue is the strongest the shelter makes, and it
goes to the joinery for the furniture and frames that hold up the people
arguing about whose bone heap it is.

---

## 24. APPENDIX K — MOUNT TABLE

| # | Piece | Animal | Year | Maker | Display | Story line |
|---|---|---|---|---|---|---|
| 1 | Antler pair | deer | 611 | Nessa | mess wall | first clean hunt |
| 2 | Boar tusk | boar | 612 | Tarn | workshop | the winter pig |
| 3 | Horn spoon | goat | 612 | Rane | kitchen | lost herd goat |
| 4 | Shell inlay | river | 612 | Sedge | guest room | the bridge diver |
| 5 | Scrimshaw panel | whale bone | pre-war | unknown | hall case | **honored, not copied** |
| 6 | Comb | cattle | 613 | Jora | quarter | the comb that started it |
| 7 | Toggle set | deer | 613 | Weft | quarter | winter coats |
| 8 | Chisel mark | cattle | 614 | Nessa | bench | first ward batch |
| 9 | Bone whistle | bird | 614 | Tarn | memorial shelf | a child's gift |
| 10 | Test bar set | mixed | 615 | Ottiline | bench wall | the standards wall |

Ten mounts and the fifth is the expansion's restraint test: a pre-war
scrimshaw panel found in a curio shop is displayed with the maker listed as
unknown and a small card that says where it came from, and the shelter's rule
is that it is honored or stored and never copied for sale. The ninth row is
the gentlest: a bone whistle made for a child and placed on the memorial
shelf when the child's grandmother dies.

---

## 25. APPENDIX L — RULE TABLE

| # | Rule | Text | Keeper | Review |
|---|---|---|---|---|
| 1 | Animal only | Human remains are never materials | liaison | annual |
| 2 | Permission | A kill's material follows the slip | liaison | each batch |
| 3 | Margin | The grave margin is never crossed | all | as seen |
| 4 | Refuse | Ambiguity stops the batch | sorter | each batch |
| 5 | Mark | Every batch carries a mark | bench | each batch |
| 6 | Destroy | Rejects are burned and logged | fire crew | each reject |

Six rules and the first is absolute. It is written at the top of the shop's
board, it is validated in the data, and it is repeated in every batch record,
because the expansion's most important design decision is what it refuses to
touch. The third row protects the dead; the fourth protects the living trade
from becoming an excuse.

---

## 26. APPENDIX M — WORKED YEAR

**Autumn.** The hunt line delivers deer and the yard gets its first proper
arrival: eleven long bones, two rib sets, and a boar jaw with a slip for each.
Jora sorts by grade and the cold soak starts, and the corridor smells for two
weeks until the soap wash reform is made. Rane claims the deer antlers for
handles and the horn core for panes.

**Early winter.** The vat rota becomes real work: four days of soap washing,
seven days of drying, and a clean standard that is written on the wall. Nessa
saws the first true needle blanks and Weft gauges them to fifteen degrees, and
the thread trade gets six needles that hold.

**Deep winter.** The ward's last surgical needle breaks during a suturing,
and Weft spends six hours at the bench cutting a twelve-degree point from a
cattle femur. The replacement passes its bend test and the ward's chief carries
it back in a folded cloth. From that night the shop is a scheduled trade.

**Late winter.** Ottiline's pot runs every cool week: hide trim, sinew scrap,
bone offcut. The fourth batch goes to the joinery and the seventh batch holds
the mess hall's benches through a season of arguing about whose bone heap it
is. The batch log proves that strong glue comes from bone offcut, which
nobody had written down.

**Spring.** Cast antler is gathered on the shed walk with no permission needed,
and Rane cuts the first horn panes. The panes are hung in the hall and the
hall is brighter in the evening than it has been all winter. The trade that
seemed marginal in autumn is now supplying the ward, the thread trade, the
joinery, and the light.

**Summer.** Jora walks the shell bar and the yard ledger records every arrival
by source and permission. The scrimshaw panel is mounted in the hall case with
its card, the grave margin is marked and explained to the quarter, and the
shop's first full year ends with eleven tools issued in a month and no
shortages anywhere.

---

## 27. APPENDIX N — VIGNETTES (TONE SAMPLE)

> Weft holds the needle up against the lamp and the tip catches light at
> fifteen degrees, and she tests it against her thumbnail, and the needle
> bends and comes back, and she puts it in the ward's cloth with eleven
> others just like it.

> The vat smells for two weeks and the quarter complains, and Jora writes the
> soap wash rota on the wall, and by the third month the complaint stops,
> which is what progress smells like.

> Nessa finds a cast antler on the shed walk and turns it over twice and puts
> it in the rod bin, and the deer that grew it is somewhere in the treeline
> growing another, and no animal was touched for this handle.

> The scrimshaw panel goes into the hall case with a card that says unknown
> maker and a note about where it was found, and Tarn says the shelter's rule
> in one sentence: we honor it or we store it, but we don't sell it.

---

## 28. APPENDIX O — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Dirty vat | tools smell, degrade | change rota |
| Poor blank | tool fails test | remake it |
| Wrong angle | point curls | reg ind, regauge |
| Weak glue | joints fail | boil longer |
| No permission | batch stopped | get the slip |
| Ambiguous source | batch stopped | liaison review |
| Reject reused | contamination risk | burn, log |
| Yard uncounted | shortages | ledger count |
| Ward waits | surgery delayed | priority queue |
| Mount excess | tone drift | restraint rule |

Every recovery is a rule the shop already contains, and the ninth row is the
one the trade judges itself by: when the ward waits, the shop's other work
stops, because a suture needle is the only object in the trade with no
substitute.

---

## 29. APPENDIX P — TONE AND ETHICS CHECKLIST

- [ ] Human remains never appear in recipes, items, mounts, or records.
- [ ] No hunt, kill, or butchery scenes are described.
- [ ] Sourcing always names a provider and a permission.
- [ ] Animal material is treated as use, not as spectacle.
- [ ] No ritual, magical, or doctrinal charge is attached to bone.
- [ ] Displays are restrained and name the animal and year.
- [ ] Pre-war work is honored or stored, never resold.
- [ ] The ward's demand outranks every other tool request.
- [ ] Salvage and shell gathering leave living things alone.
- [ ] The grave margin is visible in the shop's own data.

---

## 31. APPENDIX Q — GLOSSARY

- **Yard** — where animal material arrives, is graded, and waits.
- **Slip** — the written permission that follows a carcass's material.
- **Green bone** — bone still holding fat; not yet tool stock.
- **Degreasing** — removing fat so a tool does not rot or stink.
- **Blank** — a cut shape sized for one intended tool.
- **Point angle** — the degrees that decide what a tool can pierce.
- **Finish** — scraped, sanded, polished, or burnished surface.
- **Batch mark** — the small code that names who made a piece.
- **Test bar** — a glue sample broken on purpose to learn its strength.
- **Grave margin** — the ground where nothing is ever taken.

---

## 32. APPENDIX R — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `BoneYardSystem` | hunt yields | grades, ledger | animals |
| `DegreasingSystem` | soap stock | vat, clean state | water ledger |
| `BoneSawSystem` | blanks | shapes, waste | materials |
| `CarvingSystem` | angles | finishes, tests | tools |
| `BoneToolSystem` | demand | issues, marks | ward |
| `GlueSystem` | fire rules | batches, supply | joinery |
| `MountSystem` | decor | mounts | memory |
| `RemainsRuleSystem` | nothing | rules, stops | anything |
| `CraftingSystem` | nothing | recipes | bone |
| `WildSystems` | nothing | nothing | nothing |
| `LivestockSystems` | nothing | nothing | nothing |
| `LeatherSystems` | nothing | nothing | nothing |
| `WardSystems` | nothing | nothing | nothing |
| `JoinerySystems` | nothing | nothing | nothing |
| `WickSystems` | nothing | nothing | nothing |
| `ShelterDecorSystem` | nothing | nothing | nothing |
| `StandingRecord` | records | records | nothing |

---

## 33. APPENDIX S — DATA SCHEMA DETAIL (NEW CATALOGS)

**`bone_materials.json`** — `material_id`, `display_name`, `kind`,
`source_animal`, `grade`, `clean_days`, `uses[]`, `tags[]`.

**`bone_sources.json`** — `source_id`, `kind`, `animal`, `provider`,
`permission_kind`, `season`, `volume`, `tags[]`.

**`degreasing_methods.json`** — `method_id`, `duration_days`, `water`,
`clean_level`, `risk`, `tags[]`.

**`blank_shapes.json`** — `shape_id`, `target_tool`, `dimensions`, `kerf`,
`waste_use`, `tags[]`.

**`carving_finishes.json`** — `finish_id`, `name`, `use`, `look`, `time_days`,
`tags[]`.

**`point_angles.json`** — `angle_id`, `tool`, `degrees`, `test`, `use`,
`failure_note`, `tags[]`.

**`bone_tools.json`** — `tool_id`, `requester`, `rate`, `batch_size`,
`substitute`, `tags[]`.

**`glue_batches.json`** — `batch_id`, `source`, `boil_hours`, `strength`,
`dry_days`, `supply`, `tags[]`.

**`mounts.json`** — `mount_id`, `piece`, `animal`, `year`, `maker_id`,
`display_rule`, `story_line`, `tags[]`.

**`remains_rules.json`** — `rule_id`, `text`, `keeper_id`, `review`,
`enforcement`, `tags[]`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on
missing or duplicate IDs, invalid source, item, or provider references, or
out-of-range numbers.

---

## 34. APPENDIX T — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Materials sorted | yard health | Yard |
| Reject rate | skill | Yard |
| Clean standard met | practice | Vat |
| Blanks per material | efficiency | Saw |
| Points tested | rigor | Bench |
| Tools issued | supply | Goods |
| Glue strength mean | pot quality | Pot |
| Ward wait days | priority | Demand |
| Mounts displayed | restraint | Mounts |
| Rule stops | integrity | Rules |

Telemetry is diagnostic only; it never gates content, never ranks a maker,
and never turns a stopped batch into a failure statistic.

---

## 35. APPENDIX U — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive inside `crafting`.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling.
- [ ] Phase 6 content passes the tone and ethics checklist in §29.
- [ ] Phase 7 soak shows a season sourced, a needle replaced, a pot supplying.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No human remains, kill scenes, or trophy framing exists anywhere.

---

## 36. APPENDIX V — OPEN QUESTIONS FOR REVIEW

1. Does the shop accept material from outside traders, and who checks it?
2. How is the animal-only rule enforced in case of ambiguity?
3. Are batch marks visible on issued tools, or kept in the ledger only?
4. Can a family keep a private mount, and where does it live?
5. Who arbitrates when the ward and the thread trade both need needles?
6. Does the shop sell buttons and combs, or issue them freely?
7. What happens to the yard when the hunt fails for a season?
8. Who teaches the point angles after Weft stops coming to the bench?

None of these may be decided unilaterally; each changes tone and balance.

---

## 37. APPENDIX W — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 15 The Deep Root | Livestock horn and bone |
| 1 | 16 The Rebuilt Body | Hand prosthetics need small fittings |
| 2 | 19 The Bitter Air | Bone charcoal for filters |
| 3 | 24 The Long Goodbye | The hard boundary on remains |
| 3 | 27 The Thread | Awls, needles, toggles |
| 4 | 31 The Kiln | Shell lime feed |
| 5 | 32 The Wild | Hunt and shed sources |
| 6 | 38 The Ward | Surgical points and chisels |
| 6 | 39 The Reagent | Soap and degreasers |
| 8 | 47 The Brigade | Boiling and fire rules |
| 8 | 48 The Pastime | Bone whistles and small games |
| 8 | 50 The Vault | Pre-war scrimshaw honored |
| 9 | 54 The Uninvited | Bone meal versus vermin |
| 9 | 55 The Quarter | Buttons, combs, toggles |
| 10 | 58 The Joinery | Glue supply |
| 10 | 60 The Wick | Horn panes for lanterns |

Each hook is additive. The Bone Shop can ship alone, and every other
expansion can ship without it.

---

## 38. APPENDIX X — ENDING PROSE SKETCHES

**The Fine Point.** Needles and awls are made on schedule, and the ward and
the thread trade never wait again, and the bench's gauge hangs where anyone
can check a point.

**The Full Pot.** Glue runs to the joinery every cool month, and the mended
benches stop being a joke, and the strength wall shows eight batches of
history.

**The Pane Light.** Horn panes light the hall through the dark weeks, and the
trade that seemed marginal is the one that carries winter.

**The Quiet Display.** Mounts and one scrimshaw panel hang with a spoken rule
about memory and restraint, and the wall names the animal and the year.

**The Sorted Yard.** Every material is graded, permitted, and marked, and the
shop schedules its own work, and the ledger is boring in the best way.

**Fade.** A bone needle in a ward kit, a bowl of buttons on a count board, and
a horn pane glowing where the electric light used to be.

---

## 39. APPENDIX Y — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Human remains | unethical | absolute rule |
| Kill scenes | gratuitous | sourcing lines only |
| Gross-out | tone | plain work |
| War trophies | glorification | restraint rule |
| Magic bone | genre drift | material only |
| Hidden craft | gatekeeping | taught angles |
| Tiny-domain filler | thinness | deep tool chain |
| Trophy economy | exploitation | no resale |
| Hero workshop | fantasy | batch marks |
| Instant degreasing | false | days |

The list exists because bonework is easy to write as either horror or
romance. The expansion's rule is that the trade is small, clean, permitted,
and patient, and that its finest product is a suture needle that is used up
and thrown away like it should be.

---

## 40. APPENDIX Z — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Materials | 14 | 3,500 |
| Sources | 10 | 2,500 |
| Methods | 6 | 2,000 |
| Shapes | 12 | 3,000 |
| Finishes | 6 | 2,000 |
| Angles | 10 | 2,500 |
| Tools | 14 | 3,000 |
| Glue batches | 8 | 2,000 |
| Mounts | 10 | 2,500 |
| Rules | 6 | 2,000 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 18 | 3,000 |
| Endings | 6 | 3,000 |
| **Total** | | **~56,500** |

---

## 41. APPENDIX AA — FIRST SHOP YEAR

| Season | Focus | Milestone |
|---|---|---|
| Autumn | sourcing | eleven bones, two ribs, one jaw |
| Autumn | vat | cold soak reform |
| Winter | blanks | first true needle blanks |
| Winter | angles | fifteen-degree gauged points |
| Winter | crisis | surgical needle breaks |
| Winter | six hours | twelve-degree replacement holds |
| Winter | pot | bone-offcut glue to joinery |
| Spring | antler | cast antlers gathered |
| Spring | panes | horn panes lit in the hall |
| Spring | small goods | buttons reach the quarter |
| Summer | shell | shell bar walked and logged |
| Summer | display | scrimshaw honored with a card |

Twelve milestones and the shape is the trade's whole journey in one year:
sourcing in autumn, crisis in winter, light in spring, and a display that
proves the shop knows what belongs to memory and what belongs to the bench.

---

## 42. APPENDIX AB — BENCH CREW TABLE

| # | Crew | Work | Size | Season | Check |
|---|---|---|---|---|---|
| 1 | Sourcing crew | collect, slips | 2 | any | ledger |
| 2 | Yard crew | sort, mark, store | 2 | any | count |
| 3 | Vat crew | degrease, rinse, dry | 2 | cool months | standard |
| 4 | Saw crew | blanks | 2 | winter | kerf |
| 5 | Carving crew | shapes, angles | 3 | winter | test |
| 6 | Finish crew | scrape, polish | 2 | winter | grade |
| 7 | Pot crew | boil, test, store | 2 | cool months | strength |
| 8 | Goods crew | assemble, count, issue | 2 | any | batch |
| 9 | Display crew | mounts, cards | 1 | any | rule |
| 10 | Teaching crew | apprentices | 1 | winter | sample |

Ten crews, and the smallest one on the list is the one the expansion treats
as most important: the display crew of one who mounts a piece with its animal
and year, because that is where the shop shows the shelter what kind of place
it is. The tenth keeps the bench staffed after the names in this plan have
stopped carving.

---

## 43. APPENDIX AC — TEACHING TABLE

| # | Lesson | Audience | Method | Record |
|---|---|---|---|---|
| 1 | Sort by grade | yard crew | bins | sheet |
| 2 | Change a vat | vat crew | rota | wall |
| 3 | Cut a blank | apprentices | vise | sample |
| 4 | Gauge a point | bench | gauge | sheet |
| 5 | Break a test bar | pot crew | press | wall |
| 6 | Count a batch | goods | board | ledger |
| 7 | Write a slip | liaisons | form | ledger |
| 8 | Honor a curio | all | card | case |

Eight lessons and the fourth is the trade's heart: an angle gauge in a
beginner's hand teaches faster than a lecture, and the sample points on the
bench wall show every angle the shop makes. The eighth is the ethics lesson,
and it is taught to everyone and not just the carvers.

---

## 44. APPENDIX AD — BONE SHOP CHARTER

| Clause | Promise |
|---|---|
| Animal only | Human remains are never materials |
| Permitted | Every carcass's material follows a written slip |
| Clean | Nothing leaves the vat until it meets the standard |
| Pointed | Every issued point passed its test |
| Marked | Every batch carries a mark |
| Supplied | The ward's need comes before every other request |
| Boiled | Glue runs under fire rules and is tested |
| Restrained | Displays name the animal and the year |
| Honored | Pre-war work is honored or stored, never sold |
| Taught | The bench teaches by cutting, not by talking |

The bone shop charter is the expansion's first-class design object, posted at
the yard gate where the slips are filed. Its first clause is complete and
unconditional, and every other clause is a promise a small trade keeps to a
shelter that mostly will not notice until something breaks.

---

## 45. APPENDIX AE — BENCH SUCCESSION TABLE

| Role | First | Successor | Handover |
|---|---|---|---|
| Carver | Nessa | Sedge | one winter bench |
| Needle maker | Weft | carver hand | one angle set |
| Horn worker | Rane | bench hand | one pane batch |
| Glue maker | Ottiline | pot hand | one boil season |
| Scrimshaw | Tarn | none claimed | stored work only |
| Sorter | Jora | yard crew | one ledger year |
| Liaison | Tansy | sourcing crew | one slip cycle |
| Apprentice | Sedge | next recruit | one small goods batch |

The succession table is measured in batches and seasons, and the fifth row is
left deliberately empty: scrimshaw is not a role the shelter fills on
schedule, because the work is rare, personal, and allowed to stop. The
apprentice's last row is the one that keeps the shop alive — a batch of
buttons is the easiest thing to teach and the first thing a new pair of hands
can finish alone.

---

## 46. APPENDIX AF — REVIEW TABLE

| # | Question | Answer source | Action |
|---|---|---|---|
| 1 | Which material arrived dirtiest? | yard | change source |
| 2 | Which vat method was cleanest? | vat log | make it standard |
| 3 | Which blank wasted most stock? | saw log | resize |
| 4 | Which point failed tests? | bench log | regauge |
| 5 | Did the ward wait? | demand | reprioritize |
| 6 | Which batch was strongest? | pot log | keep the source |
| 7 | Which mount drew the most guests? | decor | keep or move |
| 8 | Did any batch stop on a rule? | rules log | review the source |
| 9 | What did the year teach? | crew | one lesson |
| 10 | What should next year make? | meeting | one plan |

Ten questions asked once a year with the yard ledger and the strength wall
open, and the answers are supposed to be numbers that change one habit each.
The tenth question is the only ambitious one: a shop that plans what next
year makes is a shop that expects to still be there, and that expectation is
the whole civic argument of the expansion.

---

## 47. APPENDIX AG — SOURCING ARITHMETIC (WORKED)

A carcass yields material according to its size and the season, and the
shelter's numbers are authored plainly. A deer yields about two long bones
for tool stock, one rib set for blades, and one antler pair if it is a shed
year; a cattle cull yields four long bones, two rib sets, two horn cores, and
a hoof set; a goat yields one horn pair and small stock. The yard's planning
rule is that the shop should hold two seasons of tool stock because hunting
fails in bad years, and the second season is stored dry and marked.

The other arithmetic is loss. A long bone saws into three needle blanks or one
handle blank and one chisel blank, and the rule is cut for the tool and not
for the material: a bone that could have been two needles and was cut into a
handle has been spent badly. The waste column in the shape table is not
sentiment; it is what a careful bench does with the piece between the two
blanks it actually wanted.

---

## 48. APPENDIX AH — TONE WATCHLIST

| Temptation | Why it fails | House rule |
|---|---|---|
| Horror heap | tone | sorted yard |
| War trophy wall | glorification | name and year |
| Ritual bone | genre drift | material only |
| Secret angles | gatekeeping | gauged and taught |
| Sole genius | fantasy | batch marks |
| Endless supply | no cost | two-season rule |
| Resold curio | exploitation | honored or stored |
| Magic glue | false | test bars |

The watchlist exists because bone is the material most likely to drag a
survival story somewhere ugly. The expansion keeps it plain and clean: a
sorted yard, a soap wash, a gauge, a test bar, a card under a found panel,
and a hard rule that the dead are never materials.

---

## 49. APPENDIX AI — SMALL MISTAKES TABLE

| Mistake | Consequence | Lesson |
|---|---|---|
| Skipped the vat | tool smells, rots | rota first |
| Cut for the material | wasted needles | cut for the tool |
| Overheated a point | tip shatters | slow the file |
| Boiled too short | weak glue | keep hours |
| Forgot the mark | can't trace a batch | mark at finish |
| Mounted too soon | tone drift | one year wait |

Six mistakes, each costing a piece or a week, and the last is the one the
shop writes into its rules: a mount waits a year, because grief and pride
both need to sit down before anything goes on a wall.

---

## 50. APPENDIX AJ — CURIOSITY AND CAREFULNESS TABLE

| # | Find | Where | Treatment | Rule |
|---|---|---|---|---|
| 1 | Scrimshaw panel | curio shop | case, card | honored |
| 2 | Carved comb | rubble | cleaned, displayed | named |
| 3 | Bone buttons | coat pocket | counted, reused | marked |
| 4 | Whistle | child's box | gift, then memory | told |
| 5 | Dice pair | floorboards | games only | no wagers |
| 6 | Doll | nursery ruin | given, not stored | asked |

Six finds and the sixth is the expansion's quiet rule about salvage: a found
doll is not a curio, it is somebody's childhood, and it goes to a child if a
child wants it and to storage if not. The fifth row is a hard boundary with
the pastime owners — bone dice are a game, never a gamble, and no wager
system appears anywhere near this trade.

---

## 51. APPENDIX AK — FOOD AND TOOL SEPARATION TABLE

| # | Space | Bone tools | Rule | Check |
|---|---|---|---|---|
| 1 | Kitchen | never | separate store | monthly |
| 2 | Mess | never | separate store | monthly |
| 3 | Nursery | never | separate store | monthly |
| 4 | Ward | surgical only | sealed kit | weekly |
| 5 | Workshop | yes | marked rack | weekly |
| 6 | Bench | yes | marked rack | daily |
| 7 | Quarter | buttons only | issued | as made |
| 8 | Store | packed | marked crates | quarterly |

Eight spaces and the first three are absolute: bone work never enters a room
where food is prepared, served, or eaten. The fourth row is the ward's sealed
kit, and the rule is not superstition — a workshop trade that handles animal
material keeps itself visibly separate from the places where the shelter eats
and heals, and the separation is checked on a rota like everything else.

---

## 52. APPENDIX AL — TRADE BOUNDARY TABLE

| # | Trade | Gives | Takes | Never |
|---|---|---|---|---|
| 1 | Wild (32) | carcasses, sheds | needles, toggles | animals |
| 2 | Herd (15) | bone, horn | combs, buttons | livestock |
| 3 | Thread (27) | sinew, cloth | awls, needles | leather |
| 4 | Ward (38) | transfer notes | points, chisels | surgery |
| 5 | Joinery (58) | offcut stock | glue | wood |
| 6 | Wick (60) | none | panes, wicks | light |
| 7 | Foundry (10) | files, saws | handles | metal |
| 8 | Reagent (39) | soap, solvent | degreasing | chemistry |
| 9 | Brigade (47) | fire rules | none | boil safety |
| 10 | Vault (50) | curio finds | honored pieces | memory |

The boundary table is the expansion's map of who owes what to whom, and its
sixth row is the smallest and most charming: the Wick gives the bone shop
nothing but takes panes and wicks, and the two trades end up sharing a bench
light between them because neither can work in the dark.

---

## 53. APPENDIX AM — WORKED BATCH

A worked batch keeps the whole chain honest in one page. Start with a cattle
cull in the last week of autumn: two horn cores, four long bones, and fat trim
weighing six kilograms. Jora sorts the trim into T-611-a, the yard marks it,
and the vat runs for six hours and yields four point two kilograms of usable
tallow. Rane claims one horn core for panes and one for combs. Nessa saws one
long bone into three needle blanks, one rib into two scrapers, and reserves
two femurs for handles. Weft gauges six needle points: five at fifteen degrees
for the thread trade and one at twelve for the ward's spare kit. Ottiline
boils the offcut into glue batch G-04, which tests strong, and the joinery
receives two jars. The reject bin gets the cracked pieces and they go to the
bone charcoal bin for the water filters.

At the end of the batch, six objects have entered the shelter's life — a
needle, an awl, a horn pane, a comb, a handle, and two jars of glue — and none
of them existed six days earlier. The worked batch is included because it is
the expansion's smallest complete argument: a single animal, used well, holds
a suture, lights a lantern, combs a child's hair, and glues a bench back
together, and the shelter never has to notice any of it unless the trade
stops.

---

## 54. APPENDIX AN — CROSS-TRADE LEDGER

| # | Trade | Receives | Date | Count | Returns |
|---|---|---|---|---|---|
| 1 | Ward | suture needles | day 612 | 1 | transfer notes |
| 2 | Thread | sewing needles | day 615 | 6 | sinew |
| 3 | Joinery | glue jars | day 620 | 2 | offcuts |
| 4 | Wick | horn panes | day 630 | 4 | wicks |
| 5 | Quarter | buttons | day 640 | 20 | coats repaired |
| 6 | Quarter | combs | day 645 | 6 | hair, morale |
| 7 | Foundry | handles | day 660 | 4 | files |
| 8 | River crew | hooks | day 680 | 10 | fish |
| 9 | Workshop | chisels | day 700 | 2 | repair work |
| 10 | Ward | splints glue | day 715 | 1 | gratitude |

The ledger's tenth row is the one the shelter quotes: a jar of bone glue
returned as gratitude, which the ward records because the drying of a splint
is a real clinical event and the record is honest. The fourth row is the
quietest and most charming exchange in Wave 10 — panes for wicks — two small
trades that keep each other working after dark.

---

## 55. CLOSING STATEMENT

ASHFALL has two bone objects in it — a surgical chisel and a pair of trophy
antlers — and nothing that explains how either was made. The Bone Shop adds
the missing half of every hunt and every herd: the yard, the vat, the saw, the
scraper, the angle gauge, the pot, the count board, and the display wall. It
is the smallest trade in the shelter and the one whose absence is measured in
broken sutures and missing awls, and it carries a single hard rule written into
its data: the dead are never materials, and the animals that fed the winter
are thanked by being used well.

> Wave 10 note: this plan is one of five Wave 10 expansion bibles (57–61). Each
> is self-contained; none requires another to ship. The shared Wave 10 index
> lives at `docs/expansions/wave10/WAVE10_INDEX.md`. The safe pre-signature
> step is Phase 1 (data schemas and validators), which is additive and
> reversible. Evidence anchors:
> `Assets/Ashfall.Core/Narrative/BoneHornCarvingCatalog.cs` (orphaned;
> `BoneDegreasingPrepLog`, `AntlerHornSawingRecord`, `ScrapingPolishingReport`,
> `NeedleAwlHookAssay`; referenced only by `ContentUtilizationScanner`,
> `ContentUtilizationRuntimeCollector`, and `src/Journal/JournalCatalogData.cs`),
> `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`,
> `Assets/Ashfall.Core/Crafting/ShelterWorkshopSystem.cs`,
> `Assets/Ashfall.Core/Shelter/ShelterDecorSystem.cs`, and the narrative logs
> `antler_horn_sawing_records.json` (3,650 B),
> `bone_degreasing_prep_logs.json` (3,623 B),
> `scraping_polishing_reports.json` (3,290 B),
> `needle_awl_hook_assays.json` (3,228 B). Items `item_surgical_bone_chisel`
> and `item_decor_trophy_deer_antlers` exist with no production chain.