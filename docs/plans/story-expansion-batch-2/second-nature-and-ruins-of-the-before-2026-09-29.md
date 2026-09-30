# Feature / Task Plan: Land, Ruins and Starts I — The Second Nature (mutated wildlife, new crops and a food web) & Ruins of the Before (hand-built sites to explore and salvage)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b: seventeen premises checked, the adaptation driver, sites, strains and depletion story reworked) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Open points are in §24 (Expansion backlog) and §25 (Open Mysteries and Deliberate Silence).

> **Subjects covered (2 of the 16 in this batch):**
> 29. **The Second Nature** — mutated wildlife, new crops and a food web. (Prefix `SN`.)
> 30. **Ruins of the Before** — hand-built sites to explore and salvage. (Prefix `RB`.)
>
> **Companions that already exist and are extended, not replaced:** `WildlifeEcosystemSystem` and `WildlifeMigrationSystem` (populations, predation, radiation attrition, seasonal moves), `WildlifeTrappingSystem` and `BestiarySystem`, `AgricultureSystem` (strains, pests, mutation outcomes, unlocked strains), the hydroponic and fungi owners, `NutritionDiversitySystem`, `EcologicalInfestationSystem`, `ExpeditionSystem` (push-your-luck, retreat, camp), `ScavengingTableCatalog` (54 tables), `LocationEvolutionSystem`, `ArchaeologySystem` (excavation), and the 25 civic sites in `deep_lore_locations.json`. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Naming notes.** *Ruins of the Before* (this plan) is **above-ground and ground-level sites you walk into**. It is **not** *The Deep Works* (`deep-works-2026-09-29.md`, held drifts joining the shelter shaft to the generated underground), **not** *The Deep* (`the-deep-2026-09-29.md`, four closed levels below the shelter — "not ruins; these were *closed*"), and **not** *The Drowned Coast* (`drowned-coast-2026-09-29.md`, dive sites). Digging *down* from a site is the existing `ArchaeologySystem`'s job and stays there. *The Second Nature* is the **regional** ecology; it is **not** *The Living Region* (settlement condition and refugees).
>
> **Sister plans (read-only cross-references; all hooks ship dark):** `war-of-words-and-long-inquest-2026-09-29.md` (a site's rooms sharpen Inquest leads), `paper-and-power-and-the-treaty-table-2026-09-29.md` (`salvage_claim` papers on claimed sites), `convoy-wars-and-inside-a-house-2026-09-29.md` (the abandoned convoy yard), `iron-road-and-siege-year-2026-09-29.md` (rail salvage), `living-region-2026-09-29.md` (settlements near sites).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, data, ledger or other plan. Paths are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **9b**, **15b** and **25** carry story texture. Sample lines are content candidates for JSON rows, never code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — What Grew Back, and What Was Left

> *"The land was not punished. It was rearranged. What lived through the first winter is not
> what lived before it, and what is living now is not what lived through it. Nobody made the
> second nature. It made itself, one generation at a time, out of whatever could stand the dose."*

Two things are true of the world outside the shelter, and the game already knows both. **First:** it is alive again in a strange way — there are hares in the orchards and carp in the river and dogs at the junction that are not the dogs anyone remembers, and they eat each other and the crops and the carrion. **Second:** it is full of *places that were built for people and no longer have any*: a library with its reading hall half open to the sky, a bunker that still files, a flooded depot, a seed vault that was never opened.

The game already owns the numbers for both. Populations, predation, radiation attrition, seasonal moves; strains, pests, mutation outcomes; a loot table for a library; a depletion factor for a location. What it does not have is **the shape**: nobody can *see* the food web, the animals never change, a strain is unlocked in a list, and a ruin is a single roll on a table with no rooms, no doors and no reason to come back.

These two subjects are paired because they are the same act at two scales. **The Second Nature** is the land *as a system you learn to read* — who eats whom, what has begun to change, what can be brought home and planted. **Ruins of the Before** is the land *as a set of places you learn to enter* — room by room, door by door, with a plan somebody drew and a floor that may not hold. Both are **patience made visible**: neither gives the player a new lever; both reward *going back and looking again*.

**The binding tone rule.** Cold, exhausted, human, restrained; specificity over adjectives; no supernatural, no monsters, no vengeful land (corpus tone lock; Exp. 40's inherited rule for the shelter's machines applies equally to the animals: **mechanical quirks, never haunted ones — biological quirks, never cursed ones**). A changed animal is still an animal. A ruin is not haunted; it is *finite, honest and slightly dangerous*, like a building.

**Tone & register.**

- **The Second Nature is written in the voice of the field notebook.** Dates, sectors, counts, a pencil sketch of a tail. The survivor who writes it is not a scientist and knows it.
- **Ruins of the Before is written in the voice of the site plan.** Room names in someone else's lettering; a door that is marked and does not open; the one detail somebody thought worth keeping (a chair, a stamp, a lunch tin).

**What the two share.** *Attention over time.* An adaptation takes a season of bad weather that an animal stands through. A ruin takes several visits because the first visit only shows you the doors. Neither part is a quest; both are **habits of returning**.

---

## 1. Goal & Outcome

### 1.1 The Second Nature (SN)

> *Design intent: on day 200 the player should be able to point at a sector on the map and say
> "the boars there are not the boars that were there in spring" — and be right, and know why.*

- **Goal:** Give the existing wildlife and farming owners a **shape**: a derived **Web** (who eats whom, who eats what is planted, what is thinning, what has vanished, gated by what the player has actually learned); a small stored **Second Nature** ledger in which ordinary species that live for a long time *at the edge of their radiation tolerance* slowly **turn** into authored **variant species** (population-conserving, through the migration owner's own operations); and a set of **Wild Stands** — authored plant sites in sectors — from which the player can take **seed** for **new strains** that plant through the existing agriculture owner.
- **Outcome (observable):**
  1. The **Web surface** (an existing panel; chosen at P0) shows, per sector, the species present with their population, the predation edges the player has *learned*, the pressure their own hunting exerts, a **Balance** word (Thin / Steady / Crowded) and any local extinction, in words.
  2. A species that keeps standing through bad weather earns **Exposure Points** (Ashfall +1, fallout storm +2, black rain +2 …). At **60 points** it **Turns**: a fixed share of the pack becomes a **variant species** (six authored: cinder hare, scree goat, slag boar, glass carp, pale heron, slate gull), conserving population at the moment of conversion and leaving the old kind behind as a remnant. At **130 points** it **Settles**: a second share converts.
  3. A variant is an **ordinary catalog species** — it has predation edges, a trapping row, a bestiary entry and a field-guide line — so every existing owner treats it correctly with no code path of its own.
  4. **Wild Stands** (six, authored, per sector, seasonal) can be found and gathered; a gathered **seed** plants through the agriculture owner as one of **six new strains** with their own tolerances and nutrition, entering the diversity owner's profile with no extra wiring.
  5. Save/load round-trips; a legacy save loads with no adaptations, no counters and no stands known, and every wildlife, trapping, bestiary, farming and infestation behaviour equals today's.
- **Non-Goals (SN):** no change to any species definition, predation pressure, radiation rule, seasonal move or strain definition already in the catalogs; **no mutation of the human survivor** (that is `mutations.json`, a different concept); no monsters, no supernatural, no "cursed" ground; no new farming, trapping or bestiary system; no genetic minigame; no new routed panel; no Unity.
- **"Done" (SN):** §19 SN acceptance passes via `bin/run-scoped-tests`; existing wildlife, trapping, bestiary, farming and infestation tests unchanged; handoff lists untouched shared paths.

### 1.2 Ruins of the Before (RB)

> *Design intent: the player should be able to say of a place they have visited three times,
> "I know that building" — where the doors are, what they still have not opened, and why the
> east stair is not to be trusted.*

- **Goal:** Give twelve existing locations a **Site Plan** — an authored graph of **rooms** with doors, conditions, fittings, dens, finds and structural states — and a small stored **Sweep** ledger that records *what the crew did in each room*. A **Site Run** walks the plan through the existing expedition loop (each push-your-luck step opens the next room; retreat leaves), rolls loot through the existing scavenging tables, and lets the crew **strip fittings** for materials at a real structural risk. Rooms, not sites, deplete; sites therefore have **standing** (Untouched, Worked, Picked-over, Bare) and **reasons to return**.
- **Outcome (observable):**
  1. The **Site surface** (an existing expedition/location panel; chosen at P0) shows the plan the crew has *learned so far* — rooms entered, doors seen but unopened, fittings noted — a **standing word**, and the reasons the site might still be worth a visit.
  2. A run enters rooms one at a time; each room's loot comes from a **named scavenging table** (or a slice of the site's own loot), seeded per (site, room, day); a room entered once yields **residue** (25% weight) on later visits.
  3. **Fittings** (pipe runs, wiring looms, slab rebar, roof plating) can be **stripped** for exact, non-random materials, at a per-room **Strain** cost; a room that is over-stripped may **fall** (seeded), sealing itself and injuring through the existing hazard route.
  4. **Dens** are populated from the sector's *present species* (read from the migration owner), so a site's dangers change as the Second Nature turns.
  5. **Finds** (the six Before-records staged at the two dispatchable exhibit sites — the other three sites wait for an expedition definition, E30) are placed in named rooms; by default discovery still fires as today, and an optional flag ties discovery to entering the room (ship dark).
  6. Save/load round-trips; a legacy save loads with no sweeps, and every expedition, scavenging, location-evolution and excavation behaviour equals today's.
- **Non-Goals (RB):** no procedural generation; no new loot tables (rooms reuse the 54 existing ones); no new hazard types; no combat system; no base-building in ruins; no change to any expedition, scavenging or `LocationEvolution` arithmetic; no digging (that stays with `ArchaeologySystem`); no hand-built site whose subject is children (§15b, DEC-RB-09); no real places; no new routed panel; no Unity.
- **"Done" (RB):** §19 RB acceptance passes; existing expedition, scavenging, location-evolution and archaeology tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**The ruin's dens are the land's second nature.** Two seams and no more: (1) a room's **den** picks its occupant from the sector's *present species* (a read of the migration owner), so a slag boar or a pale heron can be behind a door that held only rats in spring; (2) two sites (the seed vault and the agricultural research station) are **seed sources** that unlock strains through the agriculture owner's existing route. Neither part keeps a copy of the other's facts (Rule 5).

---

## 1b. Texture, Mystery & Voice

**The Second Nature: the notebook.**

The Web never says "the ecosystem is unbalanced." It says: *"Floodplain: seven boars in the spring count, five now. Two of them are the wrong colour. Nobody has said what to call them."* The sentence is the surface; the counts stay in the owners. The **Balance** word (Thin / Steady / Crowded) sits under it in small type.

**An adaptation is not an event.**

A species does not "mutate" on a day. It *has stood through a season of ash and hail and storm that thinned everything around it*, and one morning the field notes carry the word **Turned**. The line the game prints is a *survivor's*: *"Marisol counted the boars again. Three of them have a pale stripe down the spine that the others do not. She put a mark by them and did not write a reason."* The word **variant** is never used; the animals are named as people name things (cinder hare, slag boar).

**A new crop is a thing in a hand.**

A Wild Stand is a place, not a menu: *"Reed flats, north side. A stand of something like rye that has gone tall and thin and grey. It seeds late. Someone has been here before and taken the heads and left the stalks."* The seed is a handful. The strain is a row in a list that the shelter has to *grow* before it is a crop.

**Ruins: the plan somebody drew.**

The Site surface is a **plan**, not a map: rooms as labelled boxes in someone else's lettering (*READING HALL*, *MICROFILM*, *STAIR B — NOT FOR PUBLIC*), doors as gaps, and — for a first visit — mostly blanks. *"You have seen three rooms of nine. The plan is on the wall of the third."* (Rooms the crew has not seen are not drawn; a *plan found on a wall* reveals the whole layout once — an authored find.)

**A room is a room.**

A room's description is one paragraph, plain, physical, with one detail somebody kept: *"Long tables with the chairs still under them. A cardigan on the back of one. The light comes down through the roof in a column and the dust hangs in it."* The site never explains itself. It has a floor, a ceiling and a way out.

**Stripping is not looting.**

Looting takes what is lying there. **Stripping** takes the building: the wiring loom behind the panel, the rebar in the slab, the roof steel. It is slow, it is certain, and every strip makes the room a little less certain to hold. *"Nine metres of pipe. The ceiling did something about it a minute later. Nobody was under it."*

**What the player is never told.**

- **What happened to the people at each site.** Rooms show what is left; the game never says who left, when, or why.
- **Whether the pale stripe is the radiation or the winters or the hunting.** Adaptations have an *exposure* recorded and no cause given.
- **Why the seed vault was never opened.** `The Seed Vault` is a record and a place; the reason it stayed shut is not established here (and is not a question in the Inquest).
- **Whether the ministry's registrar is still at the desk.** The room is there. The chair is there.

**Voice — sample fragments (content candidates for `web_lines.json`, `site_lines.json`).**

> "Floodplain: seven boars in the spring count, five now. Two of them are the wrong colour." — Web (SN)

> "Marisol counted the boars again. Three have a pale stripe down the spine. She put a mark by them and did not write a reason." — Turned (SN)

> "Reed flats, north side. A stand of something like rye, gone tall and thin and grey. It seeds late." — Wild Stand (SN)

> "You have seen three rooms of nine. The plan is on the wall of the third." — Site (RB)

> "Long tables with the chairs still under them. A cardigan on the back of one." — Room (RB)

> "Nine metres of pipe. The ceiling did something about it a minute later. Nobody was under it." — Strip (RB)

**Design texture beats.**

- **Turned is a word in a notebook, not a cutscene (SN).**
- **The old kind persists (SN).** A remnant of the original species always remains; conversion never reaches 100%.
- **A plan is earned (RB).** Rooms not yet seen are not drawn.
- **Rooms deplete, buildings do not (RB).** The site stays; its standing changes.
- **A den is behind a door (SN × RB).** The animal is whatever lives in the sector now.

---

both are **habits of returning**.

**The second layer.** Both halves are patience made visible, at two tempos: the Second Nature is
evolution at the pace of pencil notes, the Ruins are architecture at the pace of trust. The world
outside is not hostile and not healing — it is *rearranged*, and reading the rearrangement is the
game. Neither part gives the player a lever; both reward going back and looking again, which is
the only verb this world ever rewards twice. The world rearranges; the pencil and the boot take
notes; the notes are the only harvest here that cannot fail.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §9b/
§15b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the land leaves lying around.**

> "Field note, day 40: 'tail shorter.' The pencil sketch beside the sentence is better than the sentence."

> "Strain row in the seed list. The list is not a reward; it is a record of what came home in someone's pocket."

> "Spring count sheet, two names crossed out. The crossings-out are in a third hand."

**What the ruins leave lying around.**

> "Site plan: room names in someone else's lettering. STAIR B — NOT FOR PUBLIC. The stair is public now."

> "Lunch tin under the long tables. The one detail somebody thought worth keeping."

> "Joist tested with a boot — twice, on two visits. The plan says hold. The boot says next time too."

**Scenes the player may piece together.**

> "The dog at the junction is not the dog anyone remembers. It is still a dog. It behaves like a dog, and that is the unsettling part."

> "The first visit shows the doors. The second shows the rooms. The third is when the floor is trusted."

**Held silences (texture, not register rows).**

- What the reading hall was for on the last day it had a roof. Rooms show what is left; the site never explains itself and this section will not explain it either.
- What the pale stripe is *from*. Exposure is recorded and no cause given; the register above holds the question and this fragment only carries the pencil.

**Fourth pass — the pencil and the boot (texture only; §25 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Two tempos, two instruments. The pencil is evolution at the pace of a
note; the boot is architecture at the pace of trust. Nothing in this plan is hostile and nothing is
healing — it is *rearranged*, and the prose should sound like someone learning to read a country
that is not finished being rewritten. The mystery is not what happened. The mystery is what is
still happening, slowly, while the notebook is closed.

**What the land leaves lying around.**

> "Field note, day 90: the sketch has become a map. The sentence has become shorter. The map is
> doing the remembering now."

> "Seed list, strain row with two dates: when it was pocketed and when it agreed to grow. Only one
> of those dates is a human decision."

**What the ruins leave lying around.**

> "Boot test, third visit. The joist holds. The note says 'holds' and the note is dated, which is
> how trust is stored."

**Held silences (texture, not register rows).**

- Where the dog at the junction sleeps. It is still a dog; a den is behind a door and the door is
  not mapped. Texture only.
- What the fourth visit would show. Doors, then rooms, then the floor (§1c scene); the fourth visit
  is not authored and must not be.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `wildlife_ecosystem.json` holds **13 species** (`radiation_tolerance`, `diet_type`, `apex_population_threshold`, `tameable`, tags): cotton hare 0.30, ash hound 0.55, feral goat 0.50, blight rat 0.90, ash boar 0.60, mirror carp 0.40, ghost moth 0.70, rad dog 0.85, wolf 0.45 (apex 14), dust lynx 0.55 (apex 10), iron crow 0.65, ash gull 0.50, gray heron 0.45; **5 predator→prey edges** (wolf→cotton hare 0.06, wolf→feral goat 0.04, dust lynx→cotton hare 0.05, rad dog→blight rat 0.07, gray heron→mirror carp 0.05); **2 seasonal moves** (cotton hare in `window_deep_freeze` 0.08; ash gull in `window_spring_storms` 0.10). | `Data/wildlife_ecosystem.json` | LIVE |
| E2 | `WildlifeEcosystemSystem` (`World/`, save id `wildlife_ecosystem`) owns hunting **pressure** per (sector, species), **extinct** (remnant) flags, apex activity, domestic animals, observations and `KnowledgeLevel(speciesId)`; `TickDay(day, migration, outdoorRadModifier, seasonWindowId, populationRng, migrationRng, apexRng, sectorHazardModifiers)`: (1) predation thins prey through `migration.ThinSpeciesInSector`, (2) **radiation attrition**: any species whose `radiation_tolerance < radPressure` (`radPressure = clamp(outdoorRad/250, 0, 1.5)`) loses 1 per sector per day at 50%, floored at `ExtinctionThreshold = 2` (**remnant pair = locally extinct**), (3) hunting-pressure decay, (4) seasonal moves through `MigratePack`. | `World/WildlifeEcosystemSystem.cs` L108–345 | LIVE |
| E3 | `WildlifeMigrationSystem` owns packs (`RegisterPack(packId, speciesId, sectorId, population)`, `MigratePack`, `TickDay`, neighbour routing, `ThinSpeciesInSector`); `world_evolution_seeds.json` seeds **24 sectors** (each with a neighbour list) and **24 packs** (e.g. `pack_floodplain_boars` ash boar 7 in `sector_4_floodplain`; `pack_reedflats_boars` ash boar 6; `pack_orchard_hares` cotton hare 11; `pack_river_carp_run` mirror carp 18; `pack_river_herons` gray heron 3; `pack_estuary_gulls` ash gull 14; `pack_bluffs_goats` feral goat 6). | `WildlifeMigrationSystem.cs`; `Data/world_evolution_seeds.json` | LIVE |
| E4 | `WildlifeSeasonalCalendar.ArchetypeOf` switches over the 12 seeded species; unknown ids read as `Resident`; `FieldGuideEntryFor` switches over six and returns null otherwise. | `WildlifeSeasonalCalendar.cs` L77–91, L268–277 | **RESOLVED (§2b)** |
| E5 | Trapping: 10 traps, 15 prey, 6 baits; `OnNewSpeciesDiscovered`, butchery, hides; `BestiarySystem` records encounters, sightings, kills, butchering and a completion percentage. | `WildlifeTrappingSystem.cs`; `Bestiary/BestiarySystem.cs`; `Data/wildlife_trapping_catalog.json` | LIVE |
| E6 | Two species already carry a `mutant` tag (`species_blight_rat`, `species_rad_dog`; tolerances 0.90 and 0.85): the **first-generation mutants exist as ordinary species**. Nothing changes a species over time; there is no adaptation. | `wildlife_ecosystem.json` | LIVE (finding) |
| E7 | Farming: `crop_strains.json` (**12 strains**, e.g. `strain_tuber_heirloom` mutation_threshold 0.55, `strain_grain_ashheart` 0.95; fields: yield, soil/toxicity/radiation tolerance, pest susceptibility, nutrition profile; **3 pests**, **2 compost recipes**); `AgricultureSystem` has a `HardyStrain` mutation outcome and `unlocked_strains`; `hydroponic_crops.json` (10 shelter crops); `NutritionDiversitySystem`; fungi and soil-reclamation engines; `EcologicalInfestationSystem` (**10 infestations**: 6 at locations, 4 in the shelter). | `Farming/*`; `Data/crop_strains.json`, `hydroponic_crops.json`, `ecological_infestations.json` | LIVE |
| E8 | **There is no food web.** Crops and wildlife are separate worlds; the five predation edges are all among animals; plants are not nodes; nothing presents who eats whom; a strain is unlocked by a list entry the player cannot browse as a place. | grep across Core | LIVE (finding) |
| E9 | `mutations.json` (9 rows) is a **human survivor** mutation tree (branches, tiers). It is not wildlife or crops and is out of scope. | `Data/mutations.json` | LIVE |
| E10 | Sites: `locations.json` (**179** locations: `id`, `displayName`, `description`, `dangerLevel`, `travelHours`, `baseRadsPerHour`); `deep_lore_locations.json` (**25** civic sites, each with `radiationUSv`, `dangerLevel`, `travelHours` and a `lootTable` of item, min, max, `spawnChance`, `degradationChance`); `scavenging_tables.json` (**54** tables by `location_type`, each with `depletion_model` finite/renewable/one_time/slow_regeneration, `base_hazard_chance`, `primary_hazard_type`, weighted entries, optional codex/map fragment unlocks); `excavation_sites.json` (8 dig sites with depth bands, structural risk, shoring materials); `micro_locations.json` (28 encounters); `world_evolution_seeds.json` (40 location seeds with sector and contamination; 30 landmarks with integrity). | `Data/*` | LIVE |
| E11 | `LocationEvolutionSystem` (`location_evolution`) keeps per-location owner, `contaminationLevel`, `lootDepletionFactor`, `isCleared`, `isRuined`, `lastVisitedDay`, `activeThreats`, `discoveredCaches`; API `GetOrCreateRecord`, `SetLocationOwner`, `MarkCleared`, `TickDay`. | `LocationEvolutionSystem.cs` | LIVE |
| E12 | `ExpeditionSystem` runs an expedition as `Start(def, survivor, day, stance, night, bicycle, flashlight, vehicle, stamina, weather, speed)`, `PushLuck`, `Retreat`, `EnterCamp`, `CampTick`, `ResolveCampEncounter`, hourly `TickHours`; a destination may `requiresDiscovery`. **A site is one node**: one roll, no interior. | `Expeditions/ExpeditionSystem.cs` L340–1050 | LIVE (finding) |
| E13 | **Two id namespaces** for locations: `location_*` (deep-lore and expansion sites) and `loc_*` (older/other catalogs); both appear in `locations.json`; some places exist under both (`location_flooded_subway_depot` vs `loc_flooded_subway_depot`, the latter targeted by `infestation_subway_molerat_nest`). | `Data/locations.json`, `world_history.json`, `ecological_infestations.json` | LIVE (finding; alias map at P0) |
| E14 | **Nine `world_history` records are staged at five sites**: `location_ministry_of_truth_bunker` — The Office of Continuity, The Reconstruction Utility Rating, The Obstacle-Marking Annex (trigger `inspection`), The Registrar Stays, The Garrison Requests the Schedule; `location_the_memory_vault` — The Vault Holds, The Paperwork Survives; `location_subterranean_seed_vault` — The Seed Vault; `location_flooded_subway_depot` — Allocation 12-B; `location_abandoned_convoy_yard` — Convoy 12 Turns Back. All but one use `location_explore`. | `Data/world_history.json` | LIVE |
| E15 | `salvage_teardown.json` breaks one *item* into parts at a bench; it has no building fittings. | `Data/salvage_teardown.json` | **RESOLVED (§2b)** — fittings keep their own rows |
| E16 | `paper_scrap` is defined in `black_flotilla_items.json`, not `items.json`; item ids span several catalogs. | `Data/*.json` | **RESOLVED (§2b)** — validators use the merged catalog |
| E17 | `RegisterPack` refuses an existing id; `ThinSpeciesInSector(species, sector, amount, floor 2)` returns the number removed; packs give birth. | `WildlifeMigrationSystem*.cs` | **RESOLVED (§2b)** |
| E18 | The ecology ticks in `TickWildlifeEcosystemDay` inside `EvolvingWorldDayOwner`, passing `OutdoorRadModifier`; save via `WildlifeEcosystemSaveStore`. | `src/Main.WildlifeEcosystem.Integration.cs` | **RESOLVED (§2b)** |
| E19 | There is no forage route; gathering can only occur on an expedition looting tick at a definition's location. | `ExpeditionSystem.cs` | **RESOLVED (§2b)** |
| E20 | `KnowledgeLevel` returns `unknown`, `observed` (1–2), `studied` (3–5), `documented` (6+). | `WildlifeEcosystemSystem.cs` L254 | **RESOLVED (§2b)** |
| E21 | Sister plans exist as plans, not source: The Deep, The Deep Works, The Drowned Coast, Living Region, Iron Road, Convoy Wars, Paper and Power, the Long Inquest. | `.ai/plans/` | PLAN |
| E22 | Difficulty scalars are read at owner sites; this plan changes none. | `difficulty_presets.json` | LIVE |

**Six findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E8 — there is no food web to see.** Five edges live in one data file; plants are not in it; nothing renders it. SN's **Web** is a derived read over the wildlife, farming and infestation owners, fogged by the owner's own `KnowledgeLevel`, not by a new fog.
2. **E2 — radiation already shapes the animals, but only by killing them.** Species with tolerance below the pressure lose one a day until a remnant pair; nothing rewards the *survivors*. SN adds the other half of the story: species living **just below** the line for long enough **turn** into a tougher variant.
3. **E6 — the first generation is already in the catalog.** The `mutant`-tagged rat and dog are the *first* second nature. SN's variants are the *second*: slower, subtler, and in the ordinary animals.
4. **E12 — a ruin is one roll.** Locations have danger, travel time and a loot table but no interior. RB gives twelve of them rooms without touching the expedition owner's arithmetic.
5. **E13 — two id namespaces.** A place can be `location_*` and `loc_*`. The plan needs an **alias map** (`site_aliases.json`) so the infestation at `loc_flooded_subway_depot` and the record at `location_flooded_subway_depot` are the same site.
6. **E14 — the Before is already staged in five sites.** Nine Inquest exhibits sit in the ministry bunker, the memory vault, the seed vault, the flooded depot and the convoy yard. RB puts them in **named rooms** so the Inquest's leads can be precise, without changing when or how discovery fires.

---

## 2b. Evidence pass 1 — what checking the source changed (2026-09-29)

Every `VERIFY (P0)` in §2 was re-read against the live worktree, and the design's own assumptions were checked against the data files they name. **Twelve premises in the first draft were wrong or unsupported.** They are listed here so nobody re-derives them, with the edit each one forced.

| # | Finding (LIVE unless stated) | Evidence | Edit made |
|---|---|---|---|
| E23 | **Radiation pressure is weather, not climate.** `radPressure = clamp(OutdoorRadModifier / 250, 0, 1.5)` and `weather_effects.json` gives that modifier only as: 0 on Clear, Rain, Overcast, Blizzard and every "quiet" kind; **Ashfall 45 (0.18)**; **Rad hail 60 (0.24)**; **Fallout storm 150 (0.60)**; **Black rain 250 (1.00)**. Most days are 0. The first draft's "slow rise, stay within 0.20 below tolerance" cannot occur. | `World/WildlifeEcosystemSystem.cs` L282; `Data/weather_effects.json`; `World/WeatherSystem.cs` L62–63 | **§6 rewritten** around *Exposure Points* earned on bad-weather days (below) |
| E24 | Rad-weather frequency by season window (from `weather_seasons.json` weights): First Thaw ≈ 21% of days, Ash Settling ≈ 42%, Deep Freeze ≈ 34%, Spring Storms ≈ 32%, Dry Ash ≈ 56%, First Fallout ≈ 77%, False Spring ≈ 29%, Deep Ash ≈ 63%. | `Data/weather_seasons.json` | expected-rate table in §6.2 |
| E17 | `WildlifeMigrationSystem.RegisterPack(packId, speciesId, sectorId, population)` **refuses an existing id** (`pack_exists`); a new pack's `seededPopulation` is its registration population (the baseline recovery reads against). `ThinSpeciesInSector(speciesId, sectorId, amount, floor = 2)` — **species first, sector second** — removes one at a time from the largest matching pack, never below the floor, and returns how many it removed. Packs also **give birth** (`BreathingRoomDaysForBirth = 3`), migrate, starve and catch rabies. | `WildlifeMigrationSystem.cs` L48–63; `.Live.cs` L168–198 | §6.4 uses unique per-stage pack ids (`_t`, `_s`) and the owner's own return value; "conserved" now means *at the instant of conversion* |
| E18 | The ecology runs in `TickWildlifeEcosystemDay(day)` (`Main.WildlifeEcosystem.Integration.cs` L73–90), called from `EvolvingWorldDayOwner` (`Main.CampaignOwners.cs` L2202) right after the migration authority moves packs. It passes `rad = world.Weather.OutdoorRadModifier` (default 100 if no weather), season, three forked RNGs and hazard modifiers. Save is `WildlifeEcosystemSaveStore` (`TryCapturePersisted` → string payload). | source | §11: the SN step goes **inside that method, after `System.TickDay`**; save home confirmed (DEC-SN-01) |
| E20 | `KnowledgeLevel(speciesId)` returns the lowercase strings **`unknown`** (0 observations), **`observed`** (1–2), **`studied`** (3–5), **`documented`** (6+). | `WildlifeEcosystemSystem.cs` L254–261 | §5 fog table uses these exact words |
| E4 | `WildlifeSeasonalCalendar.ArchetypeOf(speciesId)` is a switch over the 12 seeded species; **anything else reads as `Resident`**. `FieldGuideEntryFor` is a switch over 6 species and returns null otherwise. A variant would therefore lose its base's migration behaviour (a cinder hare would stop being a `BurrowSwarm`). | `WildlifeSeasonalCalendar.cs` L77–91, L268–277 | variant rows carry `base_species_id`; a three-line `INT` fallback (DEC-SN-09) |
| E25 | **Seed packs are small.** Of 24 seeded packs, `pack_river_herons` = 3, `pack_quarries_lynx` = 2, `pack_upland_lynx` = 3, `pack_hill_wolves` = 4, `pack_waterworks_herons` = 4. Cotton hare packs 11 and 10, ash boar 7 and 6, goats 6, carp 18 and 15, gulls 14, 12 and 14. | `Data/world_evolution_seeds.json` | `MinPop` 4 → **3**; conversion size `min(max(1, ⌊p·share⌋), p − 2)` so the smallest packs can still turn one animal |
| E26 | **The sector ids in the first draft did not exist** (`sector_3_orchard`, "reed flats" as a bare word). The 24 real ids are `sector_4_hinterlands`, `_hills`, `_floodplain`, `_canyon`, `_railway_cut`, `_river`, `_highway_junction`, `sector_8_bluffs`, `_lowlands`, `_estuary`, `_quarries`, `sector_4_orchards`, `_waterworks`, `_civic_exchange`, `_industrial_works`, `_chemical_corridor`, `_rail_yard`, `_uplands`, `_northern_timber`, `_checkpoint_belt`, `_ground_zero`, `sector_8_docklands`, `_reed_flats`, `_deep_shelf`. Only **40 locations** carry a sector (`location_seeds`). | `Data/world_evolution_seeds.json` | all examples, stands and sites restated against real ids |
| E27 | **Strains have a real schema.** `crop_strains.json` (12): `seed_item_id`, `yield_modifier` 0.9–1.15, `soil_tolerance` 0.4–0.85, `toxicity_tolerance_permille` 250–700, `radiation_tolerance` 0.35–0.9, `pest_susceptibility` 0.3–0.7, `mutation_threshold` 0.5–0.95, a numeric `nutrition_profile` (`calories`, `protein`, `vitamin_c`, `micronutrients`, `fats`, `fiber`), five weighted `mutation_outcomes` summing to 100, and `tags`. Seed items follow the `item_seed_*` pattern and live in `greenhouse_items.json` (type Material, stack 20, weight 0.1). The first draft's "high/mid/low" and tag-based nutrition were invalid. | `Data/crop_strains.json`; `Data/greenhouse_items.json` | §9.2 rewritten in the real schema |
| E28 | Strains unlock by a **direct public call**, `AgricultureSystem.UnlockStrain(strainId)` (idempotent), which the cryo-vault integration already uses (`Main.CryoVault.Integration.cs` L96). There is no "first planted seed" hook. Item catalogs are loaded from a **fixed file list** (`ItemCatalogLoader.cs` L159), so a new seed file needs a one-line `INT` addition. | `Farming/AgricultureSystem.cs` L635–640 | §9.2: unlock happens **at first gather**; seed items in `wild_seed_items.json` (DEC-SN-10) |
| E29 | **A location is a node with a table id, and the table is inert.** `expeditions.json` holds **75 definitions**, each with a `scavenging_table_id` (all resolve). A run has **3 looting ticks** (`AutoRetreatAfterLootTicks`); each tick rolls at `0.5 + 0.05·danger` (+0.1 at night), then `RollLoot(tableId, rng, …)` picks one weighted entry. `depletion_model` (finite / renewable / one_time / slow_regeneration) is **declared and validated but never consumed at runtime**, and `lootDepletionFactor` ("spoilage") is only *displayed* (`ExpeditionPanel`, playtest CLI) — it never scales a roll. **Nothing in the game today actually runs out.** | `ExpeditionSystem.cs` L259, L1147–1175; `ScavengingTableCatalog.cs` L30 | §13.1 rewritten: rooms are what finally deplete; residue scales the *existing* tick chance |
| E30 | **Three of the five Inquest exhibit sites cannot be dispatched to.** `location_the_memory_vault`, `location_subterranean_seed_vault` and `location_abandoned_convoy_yard` have **no expedition definition** (they exist only as deep-lore rows with their own loot tables). `location_ministry_of_truth_bunker` (danger 9, `table_loot_ministry_bunker`) and `location_flooded_subway_depot` (danger 7, `table_loot_metro_station`) do. | `Data/expeditions.json`; `Data/locations.json` | first-ship sites = those with definitions; the three are **phase-two** (each needs one additive definition row, DEC-RB-10) |
| E31 | Five scavenging tables are **referenced by no definition**: `table_loot_clinic`, `_fire_station`, `_greenhouse`, `_hunting_cabin`, `_monastery`. `table_loot_greenhouse` (renewable; seed_packets 45 × 2–6, chemicals, HEPA filter, glass panes) is the natural table for a seed store. Nine definitions (e.g. `loc_north_freight_yard`, `loc_marsh_hollow`, `loc_charcoat_burns`) have **no `locations.json` row**. | `Data/scavenging_tables.json`; `Data/expeditions.json` | seed rooms use the greenhouse table; a validator row for defs-without-locations |
| E32 | The first draft's sites did not exist. There is **no** public library, agricultural research station, textile mill, grain elevator, rail signal works or "regional hospital" location, and **no `library` table**. Real, dispatchable equivalents: `old_library_cache` (danger 3, `table_loot_school` — books, tapes and a teddy bear), `loc_seed_library_annex` (4, `table_loot_farm`), `loc_municipal_seed_vault` (4), `abandoned_hospital` (6, `table_loot_hospital`) with `hospital_pharmacy` as its **own** location, `loc_underground_fuel_depot` (5), `loc_grain_silo` "The Grain Exchange" (3), `loc_pump_station_nine` (7), `loc_municipal_archive` (5), `loc_printworks` (5), `loc_radio_relay_mast` (6). | `Data/locations.json`; `Data/expeditions.json` | §12.3 rebuilt on real ids |
| E15 | `salvage_teardown.json` (12 recipes) breaks one **item** into parts at a bench (`source_item_id` → `components`, tool and wear). It has no notion of a building fitting. | `Data/salvage_teardown.json` | fittings keep **their own rows**; DEC-RB-04 closed |
| E16 | `paper_scrap` is **not** in `items.json` but is used by nine deep-lore loot rows and defined in `black_flotilla_items.json`; item ids are spread over several catalogs (`items.json` 724 ids, `greenhouse_items.json`, `black_flotilla_items.json`, …). Any validator that checks item ids must use the merged catalog, not `items.json` alone. | `Data/*.json` | RB-V5 / SN-V3 check against the merged item catalog |
| E19 | There is **no forage route**. Gathering can happen only where the expedition loop already does something: a looting tick at a definition's location. | `ExpeditionSystem.cs` | stands are attached to expedition locations (§9.1) |

**What did not change:** derive-don't-store; the Web as a read model; the single stored counter per (sector, species); variants as ordinary catalog rows; the rooms-not-sites idea; dens read from present species. The evidence moved the *drivers* (weather, not climate), the *ids* (real sectors and sites), the *schemas* (strains, seeds), and the *depletion story* (nothing depletes today, so rooms are a real addition, not a re-skin).

**Four consequences worth stating plainly.**

1. **Adaptation is a story about bad weather.** The animals that turn are the ones that keep standing through Ashfall, hail, storms and black rain. The player will *feel* it: a hard first-fallout season is the season the hares start to change.
2. **Three variants shrug off fallout storms** (scree goat 0.62, slag boar 0.72, slate gull 0.63 all exceed the storm's 0.60) and **none shrugs off black rain** (1.00). The ceiling stays; the middle of the sky becomes survivable for some.
3. **Rooms are the first thing in the game that runs out.** Because tables are inert (E29), residue and bare rooms are new play, not a re-skin — and the reason a site becomes worth *remembering*.
4. **Half the exhibit sites are unreachable today** (E30). Ruins ships on the twelve dispatchable sites first; the three deep-lore exhibits wait for a definition row.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Species definitions, predation edges, seasonal moves | `wildlife_ecosystem.json` + `WildlifeEcosystemSystem` | additive rows only (six variant species, additional edges) |
| Populations and packs | `WildlifeMigrationSystem` | requests only (`ThinSpeciesInSector`, `RegisterPack`) |
| Hunting pressure, extinction remnants, observations, knowledge | `WildlifeEcosystemSystem` | read-only |
| Trapping, butchery, bestiary | `WildlifeTrappingSystem`, `BestiarySystem` | additive rows for variants |
| Strains, mutation outcomes, unlocked strains | `AgricultureSystem`, `crop_strains.json` | additive rows (six strains); unlock through the existing route |
| Shelter crops, fungi, soil | hydroponic, fungi, soil owners | none |
| Diet diversity | `NutritionDiversitySystem` | none (new strains carry their profile) |
| Infestations | `EcologicalInfestationSystem` | read-only |
| Location owner, cleared, ruined, depletion, caches | `LocationEvolutionSystem` | read-only; optional `MarkCleared` request |
| Loot rolls, hazard chance | `ScavengingTableCatalog` | read-only (rooms name a table) |
| Expedition loop, stamina, camp | `ExpeditionSystem` | none (the run is driven *through* it) |
| Digging down | `ArchaeologySystem` | none |
| Discovery of Before-records | journal/knowledge owner, each record's trigger | none (optional room-gated flag ships off) |
| **The Web (nodes, edges, Balance)** | — | pure derived read model |
| **Adaptation counters and turns** | — | `SecondNatureState` (stored) |
| **Wild Stands known / seed taken** | — | `SecondNatureState.Stands` (stored) |
| **Site plans (rooms, doors, fittings, dens, finds)** | — | `site_plans.json` (data) |
| **What the crew did in each room** | — | `SiteSweepState` (stored) |
| **Site standing, return reasons, depletion words** | — | pure derived read models |

**Non-duplication statement.** Every population, pressure, strain, loot roll, depletion factor and discovery stays with its owner. The plan stores only **what the world did to itself** (an adaptation stage reached; days endured near a limit; a variant pack registered) and **what the crew did** (a stand found; a seed taken; a room entered, stripped or sealed; a fitting removed). Everything else — the Web, Balance, exposure band, standing, residue, strain, return reasons — is derived on read. **No new population, farming, loot, expedition, depletion or discovery authority is created.**

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (SN):** `Assets/Ashfall.Core/World/SecondNature/SecondNatureState.cs` (state + rules), `SecondNature/FoodWeb.cs` (pure read model), `SecondNature/Adaptation.cs` (edge-exposure counters, stages, conversion plan), `SecondNature/VariantConversion.cs` (requests to the migration owner), `SecondNature/WildStands.cs` (stands, seasons, seed), `SecondNature/SecondNatureCatalogLoader.cs`.

**Core (RB):** `Assets/Ashfall.Core/Exploration/Sites/SitePlanCatalog.cs` (loader + validators), `Sites/SiteSweepState.cs` (state + rules), `Sites/SiteRun.cs` (pure engine over the plan), `Sites/SiteStanding.cs` (pure), `Sites/Fittings.cs` (strip rules), `Sites/SiteAliasMap.cs`. Additive nested DTOs in the wildlife-ecosystem save (SN) and the location-evolution save (RB), `INT`, homes chosen at P0.

**Data (SN):** `second_nature_variants.json` (species rows + edges), `second_nature_strains.json` (strains), `wild_seed_items.json` (six seed items), `wild_stands.json`, `web_lines.json`, `field_notes.json`.
**Data (RB):** `site_plans.json`, `site_aliases.json`, `site_lines.json`, `fitting_yields.json`.

**Host:** `src/Main.CampaignOwners.cs` (`INT`: SN day step after the wildlife tick; sweep credit), the expedition result seam (`INT`: `IExpeditionSiteHook`), `src/Main.Campaign.cs` (`INT`: briefing lines).

**Presentation (both):** extend an existing wildlife/bestiary/farming surface (Web, Beds) and the existing expedition/location surface (Site). No new routed panel (DEC-SN-08, DEC-RB-08).

**Tests:** `Ashfall.Core.Tests/World/SecondNature/*` (Web, Balance, Exposure, Conversion, Variants, Stands, Save); `Ashfall.Core.Tests/Exploration/Sites/*` (Plan, Run, Residue, Strip, Collapse, Standing, Aliases, Save); parity guards extend existing wildlife, trapping, bestiary, farming, expedition, scavenging and location-evolution tests unchanged.


---

# PART ONE — THE SECOND NATURE (SN)

## 5. The Web (derived; nothing stored)

The Web is a pure read model `FoodWeb.Build(sectorId, ecosystem, migration, agriculture, infestations, knowledge)` returning **nodes** and **edges** for one sector. It stores nothing and is recomputed on read.

**Nodes.**

| Kind | Source | Shown as |
|---|---|---|
| Animal species present | packs in the sector (migration owner) | species name, population *word* (Few / Some / Many) and, at `studied`, the count |
| Plant / crop | strains the player has *planted or found* in this sector (agriculture owner; Wild Stands known) | strain name |
| Carrion | any species with pressure > 0 in the sector | a single node "carrion" |
| Infestation | `EcologicalInfestationSystem` active infestations located in the sector | infestation name |

**Edges** (all derived, all labelled with what the *player has learned*).

| Edge | Derived from | Visible when |
|---|---|---|
| Predator → prey | the five (plus additive) edges in `wildlife_ecosystem.json` | `KnowledgeLevel(predator) ≥ observed` *and* prey present |
| Animal → crop | species `diet_type` includes plant/omnivore **and** a strain planted or wild in the sector | `KnowledgeLevel(species) ≥ observed` |
| Player → species | `WildlifeEcosystemSystem` hunting pressure (sector, species) > 0 | always (the player did it) |
| Radiation → species | `radPressure` versus tolerance (thin / carried / safe, by the day's weather) | `KnowledgeLevel ≥ documented` |
| Infestation → node | infestation target list | at discovery |

**Fog is the owner's.** The Web never adds a second fog. `KnowledgeLevel(speciesId)` (E20: `unknown`, then `observed` at 1, `studied` at 3, `documented` at 6 observations) decides which edges and counts appear. Species never seen appear as "something has been eating the [carp]" only when an owner records an **observation**.

**Balance word** per species per sector (derived): let `pop` be the sector's current population of the species and `cap` the sum of `seededPopulation` over the sector's packs of that species (a field the migration owner already keeps, E17). `Thin` if `pop ≤ 0.5·cap`; `Crowded` if `pop ≥ 1.5·cap`; otherwise `Steady`. A species with a remnant pair (extinction flag) is `Gone`. Balance is a *word*, never a control.

**Worked example (E1, E3).** `sector_4_floodplain` holds `pack_floodplain_boars` (ash boar, seeded 7). After a hard winter and heavy hunting the pack stands at 3. The Web row reads: *"Floodplain — boars: Thin (3). You have hunted here (pressure: some)."* With ash boar knowledge at `studied` the count shows; at `unknown` the row would say *"something heavy has been rooting in the floodplain"* only if an observation exists.

---

## 6. Adaptation — Exposure, Turn, Settle

### 6.1 The idea

Weather is the ecology's radiation clock (E23). Every day the ecosystem computes `radPressure = clamp(OutdoorRadModifier / 250, 0, 1.5)` and any species whose `radiation_tolerance` is *below* it loses one animal per sector at 50% (floor 2, the remnant pair). The shipped weather table produces that pressure only five ways:

| Weather | Modifier | `radPressure` |
|---|---|---|
| Clear, Rain, Overcast, Blizzard and every quiet kind | 0 | **0.00** |
| Ashfall | 45 | **0.18** |
| Rad hail | 60 | **0.24** |
| Fallout storm | 150 | **0.60** |
| Black rain | 250 | **1.00** |

So exposure arrives as **bad-weather days**. The animals that adapt are not the ones that sit near a slowly rising line; they are the ones that **keep being there** on the days that thin their neighbours. SN records exactly that, as **Exposure Points**, and after enough of them lets a share of the pack **Turn** into an authored variant. Population is conserved at the moment of conversion; the old kind always leaves a remnant behind.

### 6.2 Exposure points (the only counter)

Once per day, in the SN step (immediately after `System.TickDay`), for each (sector, species) with a live pack group whose total population is **≥ MinPop (3) after that tick**:

| Day's `radPressure` versus the species' tolerance | Points |
|---|---|
| `pressure = 0` | +0 |
| `0 < pressure ≤ tolerance` — *carried it* | **+1** |
| `pressure > tolerance` — *stood it* (the day thinned the species and it is still here) | **+2** |

**There is no decay and no reset**: adaptation is remembered. Points stop accruing while the group is below `MinPop` and the counter is dropped only if the species goes locally extinct (the ecosystem's own remnant flag).

| Constant | Value | Reason |
|---|---|---|
| `MinPop` | 3 | the ecosystem never thins below 2, so 3 is the smallest group with a spare animal (E25) |
| `NoticePoints` | 30 | a field note, no population change |
| `TurnPoints` | 60 | roughly a season of bad weather |
| `SettlePoints` | 130 | roughly two more |

**How fast, honestly.** The expected points per day for a cotton hare (tolerance 0.30: Ashfall +1, hail +1, storm +2, black rain +2), computed from the season-window weights (E24):

| Window (days) | Expected points/day | Points this window | Running total |
|---|---|---|---|
| First Thaw (0–29) | 0.29 | 8.7 | 8.7 |
| Ash Settling (30–59) | 0.49 | 14.7 | 23.4 |
| Deep Freeze (60–89) | 0.34 | 10.2 | 33.6 |
| Spring Storms (90–119) | 0.32 | 9.7 | 43.3 |
| Dry Ash (120–149) | 0.75 | 22.6 | **65.9 → Turns ≈ day 142** |
| First Fallout (150–179) | 1.17 | 35.1 | 101.0 |
| False Spring (180–199) | 0.29 | 5.9 | 106.9 |
| Deep Ash (200–239) | 0.92 | 36.6 | **143.5 → Settles ≈ day 225** |

These are **expectations**; the real weather is seeded per campaign and storms also thin the pack, so a badly hit pack pauses below `MinPop` and turns later, or never. That is the design: **a species has to survive its own bad seasons to change.**

### 6.3 Stages

| Stage | Trigger | Effect |
|---|---|---|
| **Noticed** | points ≥ 30 | an observation-only field note (optional line); no population change |
| **Turned** | points ≥ 60 | convert `TurnShare` (30%) of the group to the variant (§6.4) |
| **Settled** | points ≥ 130 | convert `SettleShare` (40%) of the *remaining originals* |

A **remnant** always persists: a conversion never takes the originals below 2. Stage flags are stored per (sector, species), so a stage fires once; a stage that cannot fire yet (see below) is retried each day and the flag is set only after a successful conversion.

### 6.4 The conversion is the owners' own operation

`VariantConversion` never edits a population. For a stage in sector `S`, species `X`, variant `V`, current group population `p`:

1. `n = min(max(1, ⌊p × share⌋), p − 2)`. If `n < 1` (that is, `p ≤ 2`) the stage is **deferred** — retried the next day, flag not set.
2. `removed = migration.ThinSpeciesInSector(X, S, n)` — **species first, sector second**, floor left at its default 2 (E17). `removed` is what the owner actually took; it may be less than `n`.
3. If `removed ≥ 1`: `migration.RegisterPack(packId, V, S, removed)` with `packId = "pack_" + V_suffix + "_" + S_suffix + ("_t" for Turn | "_s" for Settle)`. `RegisterPack` **refuses an existing id** (`pack_exists`), so each stage uses its own id; the new pack's `seededPopulation` becomes its baseline for recovery.
4. The stage flag and the new pack id are stored.

**Conserved at the instant, not forever.** Animals converted are exactly animals removed. After that, packs follow the owner's own rules — births, migration, starvation, rabies — and the totals move as they always have.

There is **no RNG in the arithmetic**. RNG appears only in the field-note line choice (a fork of `CampaignStreamIds`, never `System.Random`); migration of the new packs is the owner's existing behaviour.

### 6.5 Worked example (real data: `pack_orchard_hares`, E3 / E23 / E24)

`sector_4_orchards` seeds one cotton-hare pack of **11** (tolerance 0.30). Following §6.2, expected points reach 60 around **day 142**. Suppose storms have thinned the group to **9** by then:

- **Turn.** `n = min(max(1, ⌊9 × 0.30⌋ = 2), 9 − 2 = 7) = 2`. `ThinSpeciesInSector("species_cotton_hare", "sector_4_orchards", 2)` returns 2. `RegisterPack("pack_cinder_hare_orchards_t", "species_cinder_hare", "sector_4_orchards", 2)`. Originals 7, variants 2.
- **Settle** around **day 225**, ignoring births: `n = min(max(1, ⌊7 × 0.40⌋ = 2), 7 − 2 = 5) = 2`. Originals 5, `pack_cinder_hare_orchards_s` = 2; variants 4 in all. Nine animals before, nine after.
- **What the hares did.** The cinder hare's tolerance (0.42) lets it ignore Ashfall (0.18) and hail (0.24) — but a fallout storm (0.60) still thins it. The change is real but modest.

**A small pack.** `pack_river_herons` holds 3 (`sector_4_river`). At the Turn: `n = min(max(1, ⌊0.9⌋ = 0 → 1), 3 − 2 = 1) = 1` — one pale heron. At Settle the originals are 2, so `p − 2 = 0`: **deferred**. The stage stays Turned until births lift the originals above 2. The remnant rule doing its job.

**A pack that cannot turn.** A group ground down to 2 by storms accrues no points and, if it goes locally extinct, loses its counter. Adaptation is not a rescue; it only rewards the animals that keep standing.

### 6.6 What Turned changes for the player

| Owner | Effect (all through catalog rows) |
|---|---|
| Ecosystem | the variant has its own tolerance: **scree goat 0.62, slag boar 0.72 and slate gull 0.63 exceed the fallout storm's 0.60** and are no longer thinned by it; **nothing survives black rain (1.00) by tolerance** |
| Trapping | the variant has a prey row (harder to trap, better yield, different butchery) |
| Bestiary | a new entry; first sighting fires `OnNewSpeciesDiscovered` |
| Nutrition | variant meat and hide reuse existing items |
| Ruins | a den's occupant is chosen from present species (§14) |

Nothing else reads Second Nature state. If SN is disabled, every catalog row stays inert and behaviour equals today's.

---

## 7. Variants — Six Authored Species

Variants are **ordinary rows** in `wildlife_ecosystem.json` semantics (added via `second_nature_variants.json`, merged by the catalog loader at boot; **no edit to `wildlife_ecosystem.json`**, DEC-SN-03). Each has a `base_species_id`, a higher tolerance, a diet, and its own predation entries. The base id is what lets `WildlifeSeasonalCalendar` give the variant its base's migration archetype instead of the `Resident` default (E4, DEC-SN-09).

| Variant id | Name | Base | Tolerance (base → variant) | Diet | Note |
|---|---|---|---|---|---|
| `species_cinder_hare` | cinder hare | cotton hare | 0.30 → **0.42** | herbivore | darker coat; smaller litters |
| `species_scree_goat` | scree goat | feral goat | 0.50 → **0.62** | herbivore | climbs; hard to reach |
| `species_slag_boar` | slag boar | ash boar | 0.60 → **0.72** | omnivore | crop raider; fewer but heavier |
| `species_glass_carp` | glass carp | mirror carp | 0.40 → **0.55** | omnivore | pale, translucent fins |
| `species_pale_heron` | pale heron | gray heron | 0.45 → **0.58** | carnivore | stands in shallows all day |
| `species_slate_gull` | slate gull | ash gull | 0.50 → **0.63** | omnivore | follows rubbish and boats |

Rules every variant row obeys (validator, §18):

- Tolerance is **strictly greater** than the base's, **≤ base + 0.16**, and **< 0.90** (the first-generation mutants stay the toughest, E6).
- The variant appears as prey/predator in the **same edges** as its base (a wolf hunts a cinder hare as it hunts a cotton hare) plus, optionally, one new edge.
- It carries `base_species_id`, and that species has a calendar archetype (E4); a variant with no resolvable base fails the validator.
- It has a trapping row (or is explicitly "not trappable") and a bestiary entry.
- It never introduces a new hazard type, a new item id, or a new tag class.

**Predation edges for variants (additive).** The five base edges are copied to the variants at load. **Pack-size note (E25):** seeded packs are small (herons 3–4, lynx 2–3), so the pale heron and the lynx line are the rarest turns; that is intended and is a tuning item (§24). New edges: pale heron → glass carp (0.05, as the base); slag boar → *carrion* is not an edge (boars scavenge in the Web only).

---

## 8. Field Notes (the voice of Turned)

Adaptation surfaces as **one line** in the survivor's notebook — never a popup, never a system message. Lines live in `field_notes.json` with a closed set of **kinds** (`noticed`, `turned`, `settled`, `stand_found`, `seed_taken`, `strain_first_harvest`), each with 3–5 variants chosen by a seeded fork (`CampaignStreamIds.SecondNature`, proposed) keyed by (sector, species, stage) so the same event always reads the same on replay.

| Kind | Sample line |
|---|---|
| `noticed` | "The hares at the orchard are thinner than they were. Marisol thinks they are eating less. She could be wrong." |
| `turned` | "Marisol counted the boars again. Three of them have a pale stripe down the spine that the others do not. She put a mark by them and did not write a reason." |
| `settled` | "It is no longer a matter of a few. The floodplain has a kind of boar now." |
| `stand_found` | "Reed flats, north side. A stand of something like rye, gone tall and thin and grey." |
| `seed_taken` | "A handful. Not enough to call a crop." |
| `strain_first_harvest` | "It grew. It is not what we planted. It is edible." |

The lines name no cause and no mechanism. Field notes are **derived**: they are printed on the day the stage flag is set and are not stored beyond the flag.

---

## 9. Wild Stands and New Strains

### 9.1 Wild Stands (six, authored)

A **Wild Stand** is an authored patch of something growing unattended, **attached to an existing expedition location** (E19, E29): there is no forage route in the game, so the only place a stand can be gathered is where a run already stands and already loots. Each stand names its location, its sector (for the Web), the season windows in which it can be gathered, a fixed seed yield and the strain it unlocks.

| Stand id | Expedition location (table) | Sector | Gatherable in | Seed item → strain |
|---|---|---|---|---|
| `stand_marsh_rye` | `loc_marsh_hollow` (`frozen_wetland`) | `sector_8_reed_flats`* | Dry Ash, First Fallout | `item_seed_grey_rye` → `strain_grey_rye` |
| `stand_cider_crab` | `loc_cider_press` (`farm`, danger 4) | `sector_4_orchards` | False Spring, Deep Ash | `item_seed_crab_pome` → `strain_crab_pome` |
| `stand_grange_oats` | `loc_grange_hall` (`farm`, danger 3) | `sector_4_hinterlands` | Spring Storms, Dry Ash | `item_seed_hinterland_oat` → `strain_hinterland_oat` |
| `stand_allotment_beet` | `loc_the_allotments` (`farm`, danger 2) | `sector_4_orchards` | First Thaw, Ash Settling | `item_seed_allotment_beet` → `strain_allotment_beet` |
| `stand_burn_cress` | `loc_charcoat_burns` (`burned_woodland`) | `sector_4_northern_timber`* | Spring Storms, False Spring, Dry Ash | `item_seed_burn_cress` → `strain_burn_cress` |
| `stand_pan_samphire` | `loc_settlement_brine_pans` (`brine_pans`) | `sector_8_estuary`* | any window | `item_seed_pan_samphire` → `strain_pan_samphire` |

\* These three locations have **no world-seed row** (only 40 locations carry a sector, E26), so the stand's own `sector_id` is authored and validated against the 24 real sector ids (validator SN-V5). Adding location seeds is an integrator decision (backlog).

**Stand state (stored, `SecondNatureState.Stands`):** per stand id — `Known` (bool) and `LastGatheredDay` (int, −1 if never). Nothing else. A stand **regrows** after `RegrowDays` (45); *availability* is derived: `season ∈ windows ∧ (LastGatheredDay < 0 ∨ day − LastGatheredDay ≥ RegrowDays)`.

**Finding and gathering, in one seam.** When a run at a stand's `location_id` completes a looting tick, the expedition result seam (`INT`) calls `SecondNature.TryGatherSeed(locationId, day, seasonWindowId)`. If the stand is available it: marks it `Known` (the first time), adds `yield` (2–4, fixed per stand) of its seed item through the inventory owner, calls `AgricultureSystem.UnlockStrain(strainId)` (idempotent, E28), sets `LastGatheredDay`, and writes one field note. The tick's ordinary table roll is unaffected — the seed is *in addition to* it, at most once per stand per regrow. No new hazard: whatever the location's own table and hazard route do still happen.

**A stand is also a fact the Web can show.** Known stands appear as plant nodes in their sector (§9.3).

### 9.2 New strains (six, authored in the real schema)

Rows in `second_nature_strains.json`, merged into the agriculture catalog at boot (DEC-SN-03). All values sit inside the range of the twelve shipped strains (E27); each row has the shipped five-row `mutation_outcomes` pattern **without** a `hardy_strain` row (weights sum to 100).

| Strain | Yield | Soil | Toxicity (‰) | Radiation | Pests | Mutation threshold | Nutrition (cal / prot / vit C / micro / fat / fibre) | Tags |
|---|---|---|---|---|---|---|---|---|
| `strain_grey_rye` | 0.92 | 0.50 | 600 | **0.85** | 0.35 | 0.90 | 0.8 / 0.5 / 0.0 / 0.3 / 0.1 / 0.6 | grain, staple, wild |
| `strain_crab_pome` | 0.90 | 0.60 | 400 | 0.60 | 0.55 | 0.80 | 0.4 / 0.1 / 0.9 / 0.5 / 0.0 / 0.5 | fruit, wild |
| `strain_hinterland_oat` | 1.00 | 0.70 | 500 | 0.55 | 0.35 | 0.85 | 0.7 / 0.6 / 0.0 / 0.4 / 0.3 / 0.7 | grain, staple, wild |
| `strain_allotment_beet` | 1.15 | 0.80 | 350 | 0.40 | 0.70 | 0.60 | 0.5 / 0.2 / 0.3 / 0.6 / 0.0 / 0.4 | root, wild |
| `strain_burn_cress` | 0.90 | 0.45 | 700 | 0.75 | 0.30 | 0.70 | 0.3 / 0.4 / 0.7 / 0.8 / 0.0 / 0.5 | leaf, wild |
| `strain_pan_samphire` | 0.95 | 0.40 | 650 | **0.90** | 0.40 | 0.90 | 0.3 / 0.3 / 0.4 / 0.8 / 0.0 / 0.6 | leaf, salt, wild |

Mutation outcomes for every row: `no_mutation` 76, `yield_penalty` 12, `toxic_harvest` 8, `sterile_seed` 4.

**Seed items** (`wild_seed_items.json`, added to `ItemCatalogLoader`'s file list — a one-line `INT` change, DEC-SN-10), in the `greenhouse_items.json` shape: `id`, `displayName`, `description`, `type: Material`, `stackMax 20`, `weight 0.1`, `tradeValue 4`. Example: `item_seed_grey_rye` — *"A handful of grey heads stripped from stalks nobody planted. Tall and thin and slow to seed."*

**Why these are worth the walk.** Grey rye (radiation 0.85) and pan samphire (0.90) sit at the very top of the shipped range (0.35–0.9). They are the crops for a shelter whose weather has turned; the hard-country stands are where the hard-country food is.

**Nutrition.** The numeric profile feeds `NutritionDiversitySystem` through the ordinary strain path (E7); no wiring.

### 9.3 A grown strain becomes food for the Web

Planted strains are **not** Web nodes: shelter beds are not in any sector. Only *Wild Stands* (in sectors) are plant nodes, so animals' crop-raiding edges apply to stands (and to any outdoor plot the agriculture owner already models — VERIFY at P0; if none exists, animal → crop edges are shown only for stands). This avoids inventing a shelter-outside link.

---

## 9b. Texture — Second Nature

- **The hares at the orchard.** They are not different animals; they are the same animals a little later. Marisol keeps a count in the margin of an old ledger. *"Eleven in spring, eleven in summer. Three of them darker, from the second week of the second month."*
- **The boar that would not be trapped.** A slag boar takes the bait and leaves the trap. The trapper writes it up; the bestiary line says only *"more careful than the others."*
- **A quiet heron.** Pale herons stand in the shallows all day. The carp are thinner where they stand. Nobody has connected these two facts and the game does not.
- **The rye.** It is a weed. It feeds people. Both are true.
- **The reed-flats trip.** *"You go for one thing and come back with another."* (The survivor who comes back with the seed and no reason to be pleased.)

---

## 10. Save shape (SN)

One additive nested DTO inside the existing wildlife-ecosystem save section (home chosen at P0, DEC-SN-01):

```
secondNature: {
  version: 1,
  edge: [ { sector, species, points, stage } ],       // stage: 0 none, 1 noticed, 2 turned, 3 settled
  stands: [ { id, known, lastGatheredDay } ],
  registeredPacks: [ { packId, sector, species } ]   // for idempotent variant registration
}
```

Restore rules: missing block = empty; unknown sector/species ids are dropped with a log; `points` clamps to ≥ 0; a stage with no matching `edge` row is dropped. **No new save section.** Derived values (Web, Balance, availability, stage words) are recomputed on read.

---

## 11. Host wiring (SN)

| Seam | Owner | Change |
|---|---|---|
| Day step | `TickWildlifeEcosystemDay(day)` in `src/Main.WildlifeEcosystem.Integration.cs` (`INT`) | one call **after** `_wildlifeEcosystem.System.TickDay(...)`: `SecondNature.TickDay(day, radPressure, world.Wildlife)` where `radPressure = clamp(rad / 250, 0, 1.5)` — the *same* value the ecosystem just used, recomputed from the same `rad` local (E18, E23) |
| Conversion | `WildlifeMigrationSystem` | `ThinSpeciesInSector(species, sector, n)` then `RegisterPack(...)` — owner operations only (E17) |
| Archetype fallback | `WildlifeSeasonalCalendar.ArchetypeOf` (`INT`) | three lines: an unknown id resolves through the variant row's `base_species_id`; otherwise unchanged (DEC-SN-09, E4) |
| Seed items | `ItemCatalogLoader` file list (`INT`) | one line adding `wild_seed_items.json` (DEC-SN-10, E28) |
| Seed gather | expedition looting-tick seam (`INT`) | one call `SecondNature.TryGatherSeed(locationId, day, seasonWindowId)` (§9.1) |
| Strain unlock | `AgricultureSystem.UnlockStrain` | existing public call, at first gather (E28) |
| Save | `WildlifeEcosystemSaveStore` | one nested DTO in the existing payload (§10; DEC-SN-01) |
| Web surface | `BestiaryPanel` (`_bestiaryPanel.Bind(...)`, refreshed at the end of the ecology day) | one new tab "Web" (derived text and counts) |
| Notes | existing journal route | one notebook line per stage change |

Ships behind a flag (`SecondNature.Enabled`, default **false** until P3); when off, no counters advance, no variant is registered and no stand is gatherable.

---

# PART TWO — RUINS OF THE BEFORE (RB)

## 12. Site Plans (authored data)

A **Site Plan** is an authored graph for one existing location. It never replaces the location's row in `locations.json` / `deep_lore_locations.json`; it **adds an interior**.

### 12.1 Shape (`site_plans.json`)

```
site: {
  id, location_id,            // alias-resolved (§17)
  entry_room,
  rooms: [ { id, name, blurb, kind, table, den, fittings[], finds[], events[], doors[] } ],
  plan_find                   // optional: item/find id that reveals the whole layout once
}
door: { to, state, requires, note }
```

| Field | Rule |
|---|---|
| `kind` | closed set: `hall`, `office`, `stack`, `vault`, `plant`, `stair`, `shaft`, `yard`, `cell`, `store` |
| `table` | an existing `scavenging_tables.json` id (54 exist, E29), or empty to use the site's own `scavenging_table_id` from `expeditions.json`; the deep-lore `lootTable` (`own`) is used only by phase-two sites |
| `events[]` | closed kinds: `stand_known` (§16); a room event is a fact for the notebook, never an explanation |
| `den` | `none`, or `sector_species` (occupant chosen from present species, §14) |
| `doors[].state` | `open`, `stuck` (needs a tool), `locked` (needs a find), `collapsed` (needs a strip or dig) |
| `doors[].requires` | an existing item or tool id, or a find id; validated against `items.json` |

**Rooms per site: 6–12.** Doors form a connected graph from `entry_room`; a room unreachable without a `collapsed` door is allowed (an *optional* wing) but the validator requires **at least 60%** of the rooms reachable with no requirement.

### 12.2 What the player sees (the plan is earned)

The Site surface draws only what the crew has **entered** or **seen through a door**. Unknown rooms are blank. A `plan_find` (e.g. a floor plan on a wall, an authored find) reveals the whole graph once, still without revealing contents.

### 12.3 The sites — twelve now, three later (all ids verified 2026-09-29, E29–E32)

A site is eligible only if it has an **expedition definition** (so it can be dispatched to) and an existing scavenging table. The first twelve do; three more of the Inquest's exhibit sites do not and wait (DEC-RB-10).

**Ship first (twelve):**

| # | Site | Expedition id | Table (existing) | Danger | Rooms | Why it is here |
|---|---|---|---|---|---|---|
| 1 | Ministry of Truth Bunker | `location_ministry_of_truth_bunker` | `ministry_bunker` (hazard 0.10 radiation) | 9 | 9 | five Before-records; the archive wing |
| 2 | Flooded Subway Depot | `location_flooded_subway_depot` (alias `loc_flooded_subway_depot`) | `metro_station` | 7 | 8 | *Allocation 12-B*; infestation target |
| 3 | Old Library Cache | `old_library_cache` | `school`, `municipal_archive` | 3 | 8 | the worked example; the gentlest site |
| 4 | Seed Library Annex | `loc_seed_library_annex` (sector `sector_4_orchards`) | `farm`, `greenhouse` (unused today) | 4 | 6 | **seed source**; sits next to the cider press stand |
| 5 | Municipal Seed Vault | `loc_municipal_seed_vault` | `farm`, `greenhouse` | 4 | 7 | **seed source** |
| 6 | Abandoned Hospital | `abandoned_hospital` (with `hospital_pharmacy` as its own linked site) | `hospital` (hazard 0.15 disease) | 6 | 10 | the dispensary and the collapsed stair |
| 7 | Underground Fuel Depot | `loc_underground_fuel_depot` | `tank_farm` | 5 | 7 | pipe runs and slab rebar |
| 8 | The Grain Exchange | `loc_grain_silo` | `farm` | 3 | 6 | tall and honest about it |
| 9 | Pump Station Nine | `loc_pump_station_nine` (sector `sector_4_waterworks`) | `waterworks` | 7 | 8 | pumps and pipe |
| 10 | Municipal Archive | `loc_municipal_archive` (sector `sector_4_civic_exchange`) | `municipal_archive` | 5 | 8 | paper and shelving |
| 11 | The Printworks | `loc_printworks` (sector `sector_4_industrial_works`) | `printworks` | 5 | 7 | presses; paper stock; ties to Paper and Power |
| 12 | Relay Mast 12 | `loc_radio_relay_mast` (sector `sector_4_railway_cut`) | `relay_mast` | 6 | 6 | wiring looms and roof steel |

**Phase two — need one additive expedition definition each (DEC-RB-10):**

| Site | Existing row | Why it waits |
|---|---|---|
| The Memory Vault | `location_the_memory_vault` (danger 10, 9 h away, 80 rads/h; deep-lore loot table only) | no definition, no table; two Before-records |
| Subterranean Seed Vault | `location_subterranean_seed_vault` (danger 6, 10 rads/h) | no definition; one Before-record; a natural seed source |
| Abandoned Convoy Yard | `location_abandoned_convoy_yard` (danger 7) | no definition; one Before-record |

Any site whose id or table fails verification at P0 is **replaced by the nearest existing definition with a table of the same type**, and the change is logged. The plan does **not** add locations (DEC-RB-02).

### 12.4 Four full plans (P7 authors the rest)

Rooms name a real table where a room has its own character; `—` means the site's own table. Doors: `open`, `stuck` (needs a tool), `locked` (needs a find), `collapsed` (needs a strip or a dig).

**Old Library Cache** (danger 3; hazard none on `school`; 8 rooms; plan-find *floor plan behind the desk*):

| Room | Kind | Table | Doors | Fittings / finds / den |
|---|---|---|---|---|
| Reading Hall | hall | `school` | → Circulation Desk (open), → Gallery (open) | roof hole; a cardigan on a chair (a detail, not a find) |
| Circulation Desk | office | `school` | → Stacks East (open), → Staff Room (open) | **plan-find**: floor plan on the wall |
| Stacks East | stack | `municipal_archive` | → Stacks West (stuck: crowbar or pry tool) | steel shelving |
| Stacks West | stack | `municipal_archive` | — | steel shelving; den (`sector_species`) |
| Microfilm Room | vault | `municipal_archive` | from Stacks West (locked: key card find in Staff Room) | dry and empty of food |
| Reading Gallery | stair | `school` | → Roof (collapsed) | roof timbers |
| Staff Room | office | `school` | → Boiler Room (open) | key card |
| Boiler Room | plant | `school` | — | pipe run; wiring loom |

**Abandoned Hospital** (danger 6; hazard 0.15 disease; 10 rooms): Reception, Triage, Ward A, Ward B, Theatre, **Dispensary** (linked to `hospital_pharmacy`; door `stuck`), **East Stair** (marked *DO NOT USE — SLABS*; door `collapsed`; slab rebar; strain limit 4), West Stair (open), Laundry, Plant Room (pipe run, wiring loom). All ten use the `hospital` table; the linked `hospital_pharmacy` (danger 5, its own definition) stays a separate dispatch, and a door from the Dispensary to it is a *note*, not a route (DEC-RB-11).

**Ministry of Truth Bunker** (danger 9; hazard 0.10 radiation; 9 rooms): Entry Lock (yard), Guardroom (`military_depot`), **Registry** (office; *The Registrar Stays*), **Continuity Office** (`government_bunker`; *The Office of Continuity*, *The Garrison Requests the Schedule*), **Ratings Desk** (office; *The Reconstruction Utility Rating*), **Annex** (cell; *The Obstacle-Marking Annex*, the `inspection` trigger), Copy Room (stack), **Archive Wing** (stack; door `collapsed` — the optional wing), Generator Bay (plant; wiring loom, pipe run).

**Flooded Subway Depot** (danger 7; `metro_station`; 8 rooms): Concourse, Ticket Hall, **Platform Office** (*Allocation 12-B*, on the wall in a frame), Lower Platform (den), Signal Room (wiring loom), Tunnel Mouth (door `collapsed`), Substation (wiring loom, pipe run), Maintenance Bay (rail stub).

---

## 13. Rooms, Residue and Fittings

### 13.1 Room loot — rooms are what finally run out

**What the game does today (E29).** A run has **three looting ticks** (`AutoRetreatAfterLootTicks = 3`). Each tick rolls at `chance = 0.5 + 0.05 × danger` (+0.1 at night); on success `RollLoot(tableId, rng, …)` picks one weighted entry from the site's single table. Tables are **stateless**: `depletion_model` is declared and never read, and `lootDepletionFactor` is displayed but never applied. Nothing in the game actually runs out.

**What a Site Run changes — and nothing else.** A run's looting tick still uses the expedition owner's `chance` and `RollLoot`. The Site Run supplies two inputs:

1. **Which table.** The tick rolls the table of the *room the crew is standing in* (or the site's own table when the room names none), not one flat site table. A reading hall, a dispensary and a boiler room can therefore give different things.
2. **A residue factor on the chance.** The first entry into a room ticks at `chance × 1.00`; each later entry at `chance × 0.25`. That is the whole of depletion: **the room is worked, not empty**, and a room entered a third time has nearly nothing to give. A room is **Bare** when it has been entered three times (`BareEntries = 3`), or when its `Sealed` flag is set.

Rolls are seeded per **(site, room, entry count, day)** through a `CampaignStreamIds` fork (`SitePlan`, proposed), so reloading cannot re-roll. The scavenging owner's weights, hazard chance and rarity tiers are untouched; the run merely chooses a table and scales one existing number.

**A run's budget.** Because a run has three looting ticks, each room entered uses one. A cautious crew takes the entry, the hall and one side room; a greedy one spends all three on the archive wing. Retreat, camp and stamina remain the expedition owner's.

### 13.2 Fittings (strip, don't loot)

A **fitting** is a fixed part of the building that yields a *certain* amount of a *named* material. `fitting_yields.json` lists them; a room lists which it has.

| Fitting | Yield (per strip) | Strain cost | Notes |
|---|---|---|---|
| pipe run | `metal_pipe` ×4 | 1 | pump station, hospital, boiler rooms |
| wiring loom | `electronic_scrap` ×3 | 2 | relay mast, ministry, depot |
| slab rebar | `steel_rebar` ×6 | 3 | any concrete room; stairs |
| roof plating | `scrap_metal` ×8 | 4 | grain exchange, fuel depot, printworks |
| steel shelving | `scrap_metal` ×3 | 1 | library, archive |
| rail stub | `steel_rail_segment` ×1 | 2 | depot, yards |
| roof timbers | `scrap_wood` ×5 | 2 | library gallery, grain exchange |

Yields are **fixed integers** authored per row (no RNG); the item ids must exist in the **merged** item catalog (validator; `metal_pipe`, `electronic_scrap`, `steel_rebar`, `scrap_metal`, `steel_rail_segment` and `scrap_wood` do — verified). **E15:** the bench-teardown catalog breaks *items*, not buildings, so fittings keep their own rows (DEC-RB-04 closed).

### 13.3 Strain and collapse

Each room has a `Strain` counter (stored, §13.4). A strip adds its strain cost. When `Strain ≥ StrainLimit(room)` (authored; default 6, tall/rotten rooms 4), a **collapse check** rolls (seeded per (site, room, strain) so it cannot be reloaded away): chance `= 10% × (Strain − StrainLimit + 1)`, capped at 60%. A collapse:

1. marks the room `Sealed` (it can no longer be entered, its unstripped fittings are lost);
2. applies the existing **structural injury** route to whoever was in the room (light injury; a `Careful` stance or shoring material — `excavation_sites.json` shoring materials — halves the chance).

No new hazard type. Collapse is the *price* of a certain haul.

### 13.4 Stored state (`SiteSweepState`, nested in the location-evolution save)

Per (site, room): `Entries` (int), `Strain` (int), `Sealed` (bool), `Stripped` (list of fitting ids), `Seen` (bool), `Cleared` (bool). Nothing else. Derived: residue factor, Bare, standing, return reasons.

---

## 14. Dens — the ruin meets the land

A room with `den: sector_species` has an occupant chosen at **entry** from the species *present in the site's sector right now* (migration owner read), weighted by population, excluding species tagged non-hostile (`tameable` species may show as **nest, not attack**). The occupant becomes an ordinary encounter through the existing expedition encounter route — **no combat system is added**: the existing outcome (flee / fight abstraction / cost) applies.

| Occupant (example) | Pressure | Outcome band |
|---|---|---|
| blight rat, rad dog | high | injury or lost time |
| wolf, dust lynx | high | encounter with flee option |
| slag boar | medium | crop-raider; heavy if cornered |
| pale heron, gulls | low | nuisance; loot risk from carrion |

When the Second Nature turns a species, the *occupants of the same rooms change* — the same door hides a different animal in autumn than it did in spring. This is the plan's one seam between parts (§1.3), and it is a read, never a copy.

---

## 15. The Site Run

The Site Run is a **pure engine** driven *through* `ExpeditionSystem`: each `PushLuck` opens the **next room** chosen by the player (from doors seen) instead of a random roll; `Retreat` leaves the site; `EnterCamp` is available in `yard` / `hall` rooms flagged safe. The expedition owner's stamina, weather, hours and hazard arithmetic are unchanged — the run supplies only *which room* and *what its roll is*.

### 15.1 One step

```
run.Enter(roomId):
  requires: door from current room to roomId is open (or its requirement is held)
  Seen(roomId) = true; Entries += 1
  loot  = RollLoot(room.table) at (site chance × residueFactor), seeded per (site, room, entries, day)
  den   = pick occupant (if any)
  finds = room.finds not yet found
  return { loot, den, finds, options: [Strip fitting..., PushLuck, Retreat] }
```

### 15.2 Options in a room

- **Take** the loot (default; free).
- **Strip** a fitting (costs time via the existing route; adds `Strain`; may trigger collapse §13.3).
- **Open** a stuck/locked door (uses the required item or tool; wear via the existing tool route).
- **Retreat** (leave; the run ends; the sweep state is saved).

### 15.3 Push Luck, meaningfully

The push-your-luck spine already exists: each `PushLuck` raises the site's hazard chance and the encounter risk. The run adds nothing to that curve. It changes only *what you get for pushing*: a **new room** (with its own table and finds) rather than another roll on a flat table. A cautious crew takes the entry, the hall and one side room; a greedy one goes for the archive wing.

### 15.4 Finds

The nine staged Before-records keep their **triggers** (E14); six are placed at first ship, three wait for their sites' definition rows (E30). A find is *placed* in a named room for the Site surface and for Inquest leads:

| Site | Room | Records placed there |
|---|---|---|
| Ministry Bunker | Registry | The Registrar Stays |
| Ministry Bunker | Continuity Office | The Office of Continuity, The Garrison Requests the Schedule |
| Ministry Bunker | Ratings Desk | The Reconstruction Utility Rating |
| Ministry Bunker | Annex (`inspection` trigger) | The Obstacle-Marking Annex |
| Memory Vault *(phase two)* | Stacks | The Vault Holds |
| Memory Vault *(phase two)* | Filing hall | The Paperwork Survives |
| Seed Vault *(phase two)* | Cold room | The Seed Vault |
| Flooded Depot | Platform office | Allocation 12-B |
| Convoy Yard *(phase two)* | Dispatch shack | Convoy 12 Turns Back |

**Default:** discovery fires as today (arriving at the site, `location_explore`). **Optional flag** `SiteFinds.RoomGated` (default **false**): discovery fires when the room is *entered* instead. The flag is a per-record trigger change through the journal owner's existing route, not a new discovery mechanism (DEC-RB-06).

---

## 15b. Texture — Ruins of the Before

- **The library.** The reading hall has lost half its roof. The light comes down in a column. *"Long tables with the chairs still under them. A cardigan on the back of one."* The microfilm room is closed and dry and has nothing anyone can eat.
- **The ministry bunker.** Everything is labelled. There are three copies of the same form. The registrar's chair is pushed in. *(Deliberate silence: no sign of the registrar.)*
- **The seed vault.** Cold room, sealed, tidy. Trays labelled in two alphabets. The door was not forced.
- **The flooded depot.** Ankle-deep, then knee-deep. The allocation notice is on the platform office wall, dry because the paper is in a frame.
- **The convoy yard.** Fuel drums with the bungs in. A dispatch board with a route chalked out and a time.
- **The hospital.** Twelve rooms, one of them a pharmacy behind a stuck door. The east stair is marked in someone's hand: *DO NOT USE — SLABS*. The plan lists it as `collapsed`.
- **The grain elevator.** Tall, narrow, honest about its danger. The catwalk is the only way to the top bin and it moves.

**Content rules for children's places (DEC-RB-09).** No site whose subject is children (schools, nurseries, wards) is hand-built in this batch. The corpus can already reference them; RB does not build interiors for them.

---

## 16. The Seed Sources (SN × RB)

Two of the twelve first-ship sites are seed places, and the two parts of this plan meet in them. Both use the **existing, unused `table_loot_greenhouse`** (renewable; `seed_packets` 45 × 2–6, chemicals, HEPA filters, glass panes, drip kits — E31) for their seed rooms, so no table is invented.

- **Seed Library Annex** (`loc_seed_library_annex`, `sector_4_orchards`, danger 4). Room **Card Room** carries a **room event** `find_seed_catalogue`: entering it once marks `stand_cider_crab` and `stand_allotment_beet` **Known** through `SecondNature.MarkStandKnown` — one request, no journal record, no new discovery system. Room **Seed Cabinets** rolls `table_loot_greenhouse`.
- **Municipal Seed Vault** (`loc_municipal_seed_vault`, danger 4). Room **Cold Cellar** carries `find_vault_ledger`: marks `stand_marsh_rye` and `stand_pan_samphire` **Known**. Room **Potting Bench** rolls `table_loot_greenhouse`.

Neither is required: any stand becomes Known the first time a run gathers there (§9.1). The sites are the **fast** route — a crew that reads the ledger walks to the reed flats *knowing* — and never the only one. The phase-two **Subterranean Seed Vault** (E30) joins them once it has a definition row.

**Room events** are a closed kind in the site plan (`stand_known`), like `finds`: the plan says *what enters the notebook*, never *why the vault was closed* (§25).

---

## 17. Aliases and the Standing Words

### 17.1 Alias map (`site_aliases.json`)

A closed list of `{ canonical, aliases[] }` so both id namespaces resolve to one site (E13). The Site surface, sweep state and infestations all key on `canonical`. The validator rejects an alias that appears under two canonicals.

Example: `canonical: location_flooded_subway_depot; aliases: [loc_flooded_subway_depot]`.

### 17.2 Standing (derived)

| Word | Rule |
|---|---|
| **Untouched** | no room `Entries > 0` |
| **Worked** | some rooms entered, ≥ 1 room not yet `Bare`/`Sealed` |
| **Picked-over** | ≥ 70% of rooms entered and residue is all that remains |
| **Bare** | every enterable room is `Bare` or `Sealed` |

### 17.3 Reasons to return (derived, at most three lines)

The Site surface lists why a site may still be worth a visit, computed from the plan and the sweep state: *doors seen but not opened*; *fittings noted but not stripped*; *a room the crew has not entered*; *a wild stand nearby that is ripe* (via the sector). If none apply the line reads *"Nothing more here that you know of."*

---

## 18. Validators (row-level failure output)

| ID | Rule | Failure prints |
|---|---|---|
| SN-V1 | variant tolerance > base and ≤ base + 0.16 and < 0.90 | `variant_id`, base, values |
| SN-V2 | every variant has a calendar row or P0-verified default, a trapping row or `not_trappable`, a bestiary entry | `variant_id`, missing owner |
| SN-V3 | strain numeric fields within existing catalog min/max | `strain_id`, field, range |
| SN-V4 | strain nutrition tags exist | `strain_id`, tag |
| SN-V5 | each stand has a real sector and a strain that exists | `stand_id` |
| SN-V6 | `TurnPoints < SettlePoints`; shares in (0,1); `MinRemnant ≥ 2`; `MinPop ≥ 3` | constant name |
| SN-V7 | field-note kinds closed; 3–5 lines each | kind |
| RB-V1 | each site id resolves via the alias map to exactly one location | `site_id` |
| RB-V2 | rooms 6–12; every door target exists; graph connected from `entry_room`; ≥ 60% reachable with no requirement | `site_id`, room |
| RB-V3 | every room `table` exists in `scavenging_tables.json`; every site has an expedition definition (or is phase-two with a deep-lore table) | `room_id` / `site_id` |
| RB-V4 | each site's `expeditions.json` definition resolves and its table exists (E29) | `site_id` |
| RB-V5 | fitting item ids exist in the **merged** item catalog; yields are fixed integers | `fitting_id` |
| RB-V6 | every find id is a real `world_history` record | `find_id` |
| RB-V7 | strain limits and collapse constants within bounds | `room_id` |
| RB-V8 | no site's subject is children (deny-list by tag) | `site_id` |
| SN-V8 | every variant has a resolvable `base_species_id` with a calendar archetype (E4) | `variant_id` |
| SN-V9 | every stand's `location_id` has an expedition definition and its `sector_id` is one of the 24 real ids (E26, E29) | `stand_id` |
| SN-V10 | every strain row is in the real schema, inside the shipped ranges (E27), with mutation weights summing to 100 | `strain_id`, field |
| RB-V9 | expedition definitions with no `locations.json` row are reported (nine today, E31), never silently accepted | `def_id` |


---

# PART THREE — ACCEPTANCE, DELIVERY, DECISIONS

## 19. Acceptance criteria

**SN**

| # | Criterion | Test |
|---|---|---|
| SN-A1 | Web for a sector lists present species, learned edges only, Balance word; nothing stored | `FoodWebTests` |
| SN-A2 | Balance thresholds: 3 of 7 → Thin; 7 of 7 → Steady; 11 of 7 → Crowded; remnant → Gone | `BalanceTests` |
| SN-A3 | Points per day: 0 at pressure 0; +1 when 0 < pressure ≤ tolerance; +2 when pressure > tolerance — only while the group is ≥ 3; no decay | `ExposureTests` (parameterised over pressures 0, 0.18, 0.24, 0.60, 1.00 and tolerances 0.30, 0.60) |
| SN-A4 | Turn at 60 points converts `min(max(1, ⌊p×0.30⌋), p−2)` via `ThinSpeciesInSector` (species first) and `RegisterPack`; the animals removed equal the animals registered | `ConversionTests` |
| SN-A5 | Settle at 130 converts 40% of the remaining originals; originals never fall below 2 | `ConversionTests` |
| SN-A6 | A stage with `p ≤ 2` is deferred (flag not set) and retried; a `pack_exists` refusal never occurs because each stage has its own id | `ConversionTests` |
| SN-A7 | Six variants pass V1/V2; predation edges inherited | `VariantCatalogTests` |
| SN-A8 | Stand availability derived from `Known`, regrow days and season | `StandTests` |
| SN-A9 | `GatherSeed` adds fixed seed yield through inventory owner; sets `LastGatheredDay`; not available before regrow | `StandTests` |
| SN-A10 | First planted seed unlocks the strain through the existing route | `StrainUnlockTests` |
| SN-A11 | Save round-trip; legacy save loads empty; unknown ids dropped | `SecondNatureSaveTests` |
| SN-A12 | Same seed and days give identical turns and note lines on replay | `DeterminismTests` |
| SN-A13 | Flag off: wildlife, trapping, bestiary, farming, infestation tests unchanged | existing tests, run unmodified |
| SN-A14 | A variant resolves its migration archetype through `base_species_id`; a species with no base still reads `Resident` | `ArchetypeFallbackTests` |
| SN-A15 | `TryGatherSeed` at a stand's location adds the fixed seed yield, calls `UnlockStrain`, sets `LastGatheredDay`, and is unavailable until regrow or out of season | `StandTests` |

**RB**

| # | Criterion | Test |
|---|---|---|
| RB-A1 | Plan validators V1–V8 pass on all twelve sites | `SitePlanCatalogTests` |
| RB-A2 | First entry to a room ticks at the expedition's own chance; later entries at 25% of it; the third entry leaves the room Bare | `ResidueTests` |
| RB-A3 | Rolls seeded per (site, room, entries, day); reload cannot change them | `SiteRunDeterminismTests` |
| RB-A4 | Strip yields fixed integers, adds Strain, requires the tool | `StripTests` |
| RB-A5 | Collapse chance `10%×(Strain−Limit+1)` capped at 60%; collapse seals room, applies injury once | `CollapseTests` |
| RB-A6 | Shoring/Careful halves collapse chance | `CollapseTests` |
| RB-A7 | Den occupant drawn from present species; changes when a variant is registered | `DenTests` |
| RB-A8 | Standing words match rules on fixtures | `StandingTests` |
| RB-A9 | Alias ids resolve to one canonical site | `AliasTests` |
| RB-A10 | Plan is earned: unseen rooms not exposed by the read model | `SiteViewTests` |
| RB-A11 | Room-gated find flag off: discovery unchanged; on: fires on entry | `FindTriggerTests` |
| RB-A12 | Save round-trip; legacy save loads no sweeps | `SiteSweepSaveTests` |
| RB-A13 | Expedition, scavenging, location-evolution, archaeology tests unchanged | existing tests |

Verification uses `bin/run-scoped-tests` with the changed test classes only; the full suite is run only on the user typing `RUN FULL TESTS`.

---

## 20. Worked examples

### 20.1 The Old Library Cache (real numbers, E29)

`old_library_cache`: danger 3, so each looting tick rolls at `0.5 + 0.05 × 3 = 0.65`. Its table `table_loot_school` totals weight 244: bandage 30 (12.3%), canned food 25 (10.2%), clean water 20, photo album 15, cassette tape 15, teddy bear 12, cloth 15, battery 15, and a tail of rare documents. A run has three looting ticks.

- **Day 60, first run.** Reading Hall (`school`, chance 0.65), Circulation Desk (`school`, 0.65 — the floor plan is on the wall behind it), Stacks East (`municipal_archive`, 0.65). Each room is entered once; each ticks at full chance. A bandage from the hall is `0.65 × 30 / 244 ≈ 8.0%` per tick; a teddy bear about 3.2%.
- **Day 75, second run.** The plan is now drawn, so the crew goes for the room they have not seen: Stacks West (its door is `stuck`; they carry a crowbar). That tick is a first entry, 0.65. With the last two ticks they revisit the Reading Hall and Stacks East — now at **0.65 × 0.25 = 0.1625** each. The rooms are worked, not empty.
- **The strip.** In Stacks East the crew strips **steel shelving** twice: two × `scrap_metal ×3`, Strain 1 + 1 = 2 (limit 6). Six certain scrap for no roll, at a small cost to the building.
- **Day 90.** Stacks East has been entered three times: **Bare**. The site's standing is *Worked*; the Boiler Room and the Reading Gallery are still unentered. The read model says so.

Nothing here touched `RollLoot`'s weights, the hazard route or the expedition's stamina. The plan chose a table and scaled one existing number.

### 20.2 The hospital stair

Room `East stair` (kind `stair`, StrainLimit 4). The crew strips **rebar (slab)** twice: Strain 3 + 3 = 6 ≥ 4 → collapse check at `10% × (6−4+1) = 30%`. With shoring material: 15%. A seeded roll on (hospital, east_stair, 6) decides. If it falls: the stair is `Sealed`, the pharmacy wing above becomes reachable only by the `collapsed` door on the west side (needs a strip or dig).

### 20.3 A turned den (real pack: `pack_waterworks_herons`)

**Pump Station Nine** (`loc_pump_station_nine`, danger 7, `sector_4_waterworks`) has a room, **Lower Gallery**, with `den: sector_species`. `world_evolution_seeds.json` seeds one heron pack there (`pack_waterworks_herons`, 4 gray herons; tolerance 0.45).

- **Spring, day ~100.** The sector's present species are gray herons (4) — low pressure, a nuisance. The den is empty or holds a heron.
- **Day ~200.** Bad weather has thinned the herons to 3 while the group kept earning points (`MinPop` 3). At 60 points they **Turn**: `n = min(max(1, ⌊3 × 0.30⌋ = 0 → 1), 3 − 2 = 1) = 1`; `pack_pale_heron_waterworks_t` registers one pale heron.
- **Day ~260.** `species_pale_heron` is present in the sector. The den roll is weighted by population, so the same door can now hold a *pale heron* — the bird that stands in the shallows all day — where in spring it held a gray one.

Same door, different bird. The site did not change. The land did.

---

## 21. Hooks (all ship dark, `Null*` defaults)

| Hook | Purpose |
|---|---|
| `ISecondNatureHook` | listeners for Turned/Settled stage (Long Inquest, Radio, Record Keepers may read) |
| `IStandHook` | seed-taken and first-harvest events |
| `ISiteHook` | room entered, fitting stripped, room collapsed, site standing changed |
| `ISiteFindPlacement` | lets sister plans name a room for a lead |
| `IPaperSalvageClaim` | Paper and Power `salvage_claim` on a site (read-only) |

Sister-plan reads only: the Long Inquest can cite a *room* in a lead; Radio may voice a Turned note; Living Region settlements near a site may raise its standing pressure. None of these is required.

---

## 22. Packages and order

| Pkg | Scope | Depends on | Ships |
|---|---|---|---|
| P0 | **pass 1 done (§2b)**; remaining: confirm where `location_explore` is raised for the two placed exhibit sites, and the encounter route a den uses | — | notes only |
| P1 | SN core: exposure, stages, conversion, save, tests | P0 | dark |
| P2 | Variants + strains + note lines data; validators SN-V1..V7 | P1 | dark |
| P3 | Web read model + surface tab; flag on for playtest | P1, P2 | on |
| P4 | Wild Stands + gather + strain unlock | P2 | on |
| P5 | RB core: plan catalog, run engine, sweep state, aliases, tests | P0 | dark |
| P6 | Fittings, strip, strain/collapse | P5 | dark |
| P7 | Site data: six sites first (1–6), validators RB-V1..V8 | P5 | dark |
| P8 | Site surface + dens seam (reads SN) + finds placement | P6, P7, P3 | on |
| P9 | Six remaining sites; seed-source rooms; closeout | P8, P4 | on |

Each package is a **bounded** unit: one truthful seam, focused tests, no parallel state. SN and RB can proceed in parallel until P8.

---

## 23. Decisions, tests, risks

### 23.1 Decisions (user/foreman signature required unless noted)

| ID | Question | Recommended |
|---|---|---|
| DEC-SN-01 | Save home for `secondNature` | nested in wildlife-ecosystem save |
| DEC-SN-02 | Exposure points +1 / +2, `MinPop` 3, turn 60, settle 130, notice 30 | as §6.2; tuned at P3 |
| DEC-SN-03 | Variants and strains merged at load, not edited into existing files | yes |
| DEC-SN-04 | Turn and Settle shares | 30% / 40%, remnant ≥ 2 |
| DEC-SN-05 | Field-note voice, no cause given | yes |
| DEC-SN-06 | Planted strains are not Web nodes | yes (§9.3) |
| DEC-SN-07 | Flag default off until P3 | yes |
| DEC-SN-08 | No new routed panel; Web is a tab | yes |
| DEC-SN-09 | Variant rows carry `base_species_id`; `WildlifeSeasonalCalendar.ArchetypeOf` falls back through it (three lines, `INT`) | yes |
| DEC-SN-10 | Seed items in a new `wild_seed_items.json`, added to `ItemCatalogLoader`'s file list (one line, `INT`) | yes |
| DEC-RB-01 | Save home for sweeps | nested in location-evolution save |
| DEC-RB-02 | Twelve sites, no new locations; unverifiable sites substituted | yes |
| DEC-RB-03 | Residue factor 0.25 on the expedition's own chance; `BareEntries` 3 | as §13.1 |
| DEC-RB-04 | ~~Fitting yields reference teardown~~ **CLOSED at pass 1**: teardown breaks items, fittings keep their own rows | closed |
| DEC-RB-05 | Residue 25%, collapse formula | as §13 |
| DEC-RB-06 | Room-gated find flag, default off | yes |
| DEC-RB-07 | Dens from present species | yes |
| DEC-RB-08 | No new routed panel; Site is a tab | yes |
| DEC-RB-09 | No children's-place interiors in this batch | yes |
| DEC-RB-10 | The three exhibit sites with no expedition definition (memory vault, seed vault, convoy yard) are phase two, each needing one additive definition row | yes |
| DEC-RB-11 | `hospital_pharmacy` stays its own dispatch; the hospital's Dispensary links to it by note only | yes |

### 23.2 Tests (focused; see §19)

Reuse parameterised fixtures for exposure points (pressures 0, 0.18, 0.24, 0.60, 1.00 against tolerances 0.30 and 0.60) and collapse chances (Strain 4–9). No duplicated wording tests. Parity guards run the *existing* wildlife, trapping, bestiary, farming, expedition, scavenging and location-evolution tests unchanged.

### 23.3 Risks

| Risk | Mitigation |
|---|---|
| Variants unbalance hunting or farming | authored numbers inside existing ranges; flag; playtest at P3 |
| `RegisterPack` cannot add to a pack (E17) | per-sector suffix packs; conserved via `ThinSpeciesInSector` |
| Web overwhelms the panel | fog by `KnowledgeLevel`; one tab; text-first |
| Room sums produce empty rooms | validators RB-V4; minimum-1 rounding rule |
| Collapse feels unfair | seeded, visible strain, halving with shoring, injury light |
| Alias map missed | validator RB-V1 fails fast |
| Save bloat | counters and flags only; no room text stored |

---

## 24. Expansion backlog (to finalise)

1. Full room lists and blurbs for all twelve sites (three are fully specified in P7; the rest need authoring).
2. Balance tuning of exposure points and turn thresholds against real seeded weather (P3 playtest); pack-size effects on the rarest variants (E25).
3. Second generation (variant of variant) — explicitly **out of scope** here.
4. Butchery and hide rows for variants; a field-guide line for each.
5. Interaction with the Deep Works and Drowned Coast dive sites (a site may share a shaft).
6. A `plan_find` for each site (currently only the hospital and library have one authored).
7. Sister-plan integration rows (Inquest leads naming rooms; Radio reading a Turned note).
8. Audio and art briefs for the Web tab and Site plan drawing.

---

## 25. Open Mysteries and Deliberate Silence

Bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`. This plan never answers:

- **Why the seed vault was never opened**, or by whom it was last closed.
- **Whether the registrar is still at the desk**, or ever left.
- **Whether the pale stripe is radiation, winter or hunting.** Adaptation records exposure, never cause.
- **What the people at any site were doing when they left.** Rooms show what remains, not what happened.
- **Whether the land is "healing".** Neither the Web nor the Site plan uses that word.

No supernatural, no monsters, no vengeful land. A changed animal is an animal; a ruin is a building.

---

## 26. Pre-flight checklist (before any P1 code)

1. Confirm no claim overlaps in `WORKTREE_OWNERSHIP.md` for the proposed paths; read `INTEGRATION_PLANS.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`.
2. Run P0: resolve every VERIFY (E4, E15–E20), the alias map, sector matches and site ids; write findings before design changes.
3. Confirm the wildlife day-tick owner and its radiation input (E18) and that `RegisterPack` semantics fit (E17).
4. Run the existing wildlife, farming, scavenging and expedition tests to establish a green baseline (scoped).
5. Get `STATUS: APPROVED BY USER` in this file (Rule 8).
6. Work one package at a time; hand off with files, commands, results and untouched shared paths.
7. On integration: mark `FULLY INTEGRATED` at the top multiple times and move to the integrated plans folder.

*End of plan.*
