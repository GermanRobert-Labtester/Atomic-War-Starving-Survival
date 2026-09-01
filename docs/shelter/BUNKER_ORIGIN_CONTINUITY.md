# Plan 29 — Bunker Origin Continuity (Task 29A §29A.17 gate)

> **Purpose:** reconcile every shelter-origin claim available in the data authority
> BEFORE any Plan 29 room-history or origin-thread content lands (Plan 29 §1.4:
> "continuity wins over a better isolated line").
> **Verdict:** reconcilable without rewriting any existing file. One Phase 1 motif fix
> required (§7.1). Gate **OPEN** for Task 29A authoring under the rules in §4–§6.

---

## 1. Three registers describe one shelter

| Register | Source | Voice | What it is authoritative for |
|---|---|---|---|
| **Mechanical** | runtime rosters: `StartingLevelSystem`, `ShelterAssignmentHostSession`, `HoldfastInteriorView`, `power_grid.json` | code / spec | room existence, capacity, power draw, attenuation, condition owners |
| **Engineering record** | `narrative/bunker_blueprints_codex.json` (24 × `room_bp_*`), `bunker_maintenance_glitches.json` (20), `bunker_maintenance_logs_batch_2/3`, `bunker_shift_schedules_and_notices` | pre-war chief-engineer register ("dwellers", maintenance cycles, failure modes) | the shelter's **original design, specification and pre-war maintenance culture** |
| **Diegetic present** | `duty_roster_locations.json` (`the_stack`, `the_approach`, `the_overflow`), `echoes.json`, `bunker_graffiti_postings.json`, `duty_roster_quests/marks` | survivor register (present tense, second person) | **what the rooms look and feel like now**, and what the survivors know about their past |

**Identity statement (canonical):** the playable shelter is **the Holdfast** (`loc_holdfast`,
"the home bunker"), known to its occupants as **the Stack** — *Allocation 12* of a
numbered pre-war distribution network. Evidence, independent files:

- roster wall chart headed `ALLOCATION 12 — DUTY ROSTER`, print date before the Exchange
  (`duty_roster_locations.json → loc_stack_roster_wall`);
- supply crate stencilled `ALLOC-12 / NOT FOR GENERAL ISSUE` (`loc_stack_airlock`);
- packing reference `ALLOC-12/DEP` (`echoes.json → echo_unopened_boots`);
- a sub-allocation `Allocation 12-B` with its own engineering notes (`duty_roster_quests.json`);
- sibling allocations 11 and 13 as a neighbouring narrative space (`the_overflow`);
- survivors refer to the community itself as "the Stack" (`duty_roster_quests.json`).

The 24-room blueprint codex is therefore **the design/maintenance documentation of this
facility**, not a second bunker: the runtime shelter exposes the habitable core (11 rooms
in `SHELTER_ROOM_INVENTORY.md` §3); the remaining documented spaces are sealed, flooded,
collapsed or never completed. Nothing in the data authority claims the current crew
occupies all 24.

## 2. Blueprint → runtime room map (former-use authority)

Plan 29 room identity `former_use` lines must be derivable from the mapped blueprint
entry (or from diegetic canon). Unmapped rooms get **no** invented pre-war purpose.

| Runtime room | Blueprint (`room_bp_*`) | Diegetic portrait | Former-use authority |
|---|---|---|---|
| `room_airlock` | 01 Surface Airlock & Decontamination Vestibule | `loc_stack_airlock` Inner Airlock | both |
| `room_filtration` | 03 Central Air Filtration & Blower Station | `loc_stack_filtration` | both |
| `room_bunks` | 05 Tier-3 Residential Bunk Cubicle Block | `loc_stack_sleeping` Sleeping Stack | both |
| `room_kitchen` | 06 Central Galley & Community Canteen | `loc_stack_mess` The Mess | both |
| `room_clinic` | 08 Dr. Vel's Clinic & Surgical Suite | `loc_stack_clinic_alcove` Clinic Alcove | **diegetic wins for present state** (§7.2) |
| `room_workshop` | 09 Master Oleg's Machine & Tooling Shop | (duty roster "the Quad strip") | both |
| `room_storage_bay` | 18 Hermetic Grain Silo & Dry Stores Magazine | deep stores (`echo_unopened_boots`) | both |
| `room_radio_tuner` | 10 Long-Range Radio Transceiver Alcove | (142.850 MHz, `StartingLevelSystem`) | both |
| `room_foundry` | 11 The Silent Foundry Blast Furnace & Casting Bay | (Silent Foundry expansion) | both |
| `room_greenhouse` | 07 Subterranean Peat & LED Hydroponic Bed | — | blueprint only |
| `room_bunker_corridor` | (nameplate wall, no dedicated bp) | `loc_stack_roster_wall` The Chart | diegetic |
| `room_water_pump` (technical) | 04 Deep Artesian Well & Water Pump Station / 13 Brine Distillation & RO | `loc_overflow_pump_hatch` | blueprint only |

Blueprint entries with **no** runtime room (02 generator vault, 12 battery vault,
14 armory, 15 memorial crypt, 16 tailor loft, 17 chemical lab, 19 carpentry shop,
20 sky-armor strata, 21 brewery, 22 apothecary, 23 schoolroom, 24 sunken atrium): these
remain **unreachable documented spaces**. Plan 29 content may *mention* them as record
only; it must not present them as walkable rooms.

## 3. Numeric & id drift register (mechanical wins)

| Drift | Mechanical authority | Diegetic/lore value | Rule |
|---|---|---|---|
| Bunk capacity | assignment cap 4, interior def 6 | blueprint bp-05: "sixteen dwellers in fifty square meters"; diegetic: "eleven bolted footboards… manifest reads fourteen" | three registers, one room: **mechanical numbers stay gameplay authority**; lore numbers describe original fit and provisioning. Never restate 16/11/14 as a gameplay value |
| Boots crate location | — | `loc_stack_airlock` (crate in the inner airlock) vs `echo_unopened_boots` ("in the deep stores") | **existing canon tension, not introduced by Plan 29.** Reading: the airlock crate is the one on show; the depot batch sits behind the store door. Plan 29 text keeps the crate's *form* (banded, taped, `ALLOC-12/DEP`, late date) and avoids asserting which room it stands in |
| Home location id | `loc_holdfast` (locations.json) | — | **fixed in Phase 1**: `StartingLevelSystem` const migrated, legacy save value mapped |
| Filtration id | `room_filtration` (spatial) | `room_filtration_stack` (Day-1), `room_air_filtration` (grid) | alias map in `shelter_room_identities.json` |
| Bunks id | `room_bunks` | `room_bunks_living` (Day-1) | alias map |
| Electrical-only spaces | `room_water_pump`, `room_lighting_main` (power_grid.json) | absent from spatial roster | identity optional; never invent positions for them |
| `room_command_vault` | excavation **blueprint id** (`ExcavationCatalogLoader`/`HostCli.WorldExploration`) | — | it is a **world site** roomBlueprintId, NOT a shelter room; do not bind shelter lore to it |
| `room_memorial_wall` | `ShelterDecorHostSession.MemorialWallRoomId` | blueprint bp-15 Memorial Crypt | valid room id for decor/memorial hooks; deferred from the Phase 2 identity set (Task 29B.20 territory) |
| `room_medical`, `room_hydroponics` | selftest-only power fixtures (`PanelBindLifecycleSelfTest`) | — | test vocabulary, not canon rooms; not whitelisted as shelter identity |

## 4. Era model (must be used, not invented)

All Plan 29 histories tag one of five eras (Plan 29 §8.3). Canon support exists for each:

1. **original construction** — blueprints, ALLOC-12 provisioning (boots crate "not for general issue");
2. **pre-war maintenance** — chief-engineer notes, maintenance cycles, glitch log codes, the "LATER" charcoal-bake cycle;
3. **crisis conversion** — the Exchange ("the sirens"), dental chair bolted in, curtain on a wire, basin that was a mixing bowl;
4. **early shelter occupancy** — nameplates taken down "in the first week", first-watch drill marks, skipped stencil numbers;
5. **current campaign** — the crew's own additions (tallies, tape marks, height lines).

## 5. Named-person register

**Rule:** Plan 29 room histories are about the room's *past*. Name only figures already
registered in the data authority, and keep them in their own era. Prefer role-anonymous
("the first watch", "the stoker", "the technician who signed the plate") unless the name
does real work.

| Era | Registered names (do not move between eras) |
|---|---|
| pre-war / engineering record | Dmitri (chief engineer), Valery, Fyodor the Stoker, Dr. Vel, Master Oleg (tool shop), Elena (greenhouse), Taras (radio), Harlan (stores), Sonya, Sister Mara, Oxana, Waystation A (sister site), "The Keeper" (104.7 MHz broadcaster) |
| present campaign (Duty Roster cast) | Kess, Ansel (and child), Hadi Morrow, Edor, Pell, Leva, Ianov, Tamsin, Halvard, Sela |

Present-campaign figures must not appear as deceased ancestors or pre-war crew.
`characters.json` is empty — there is **no** historical-person registry to register
against, which is exactly why §5 restricts new naming to existing canon.

**Phase 2 application:** the authored vignettes name **no one**. Even though the
engineering register supplies names, every one of the eight histories is carried by role
and object ("the first watch", "the crew", "one adult who had read about birth once"),
because naming a first-occupancy figure would silently promote a flavour suggestion into a
canon person with no registry to hold it. The names above are available to the origin
thread (§9) where the archive infrastructure can carry them properly.

## 6. Canon chains Plan 29 must reuse rather than reinvent

These already connect files across registers; new content should attach to them:

1. **The nameplates.** `echo_the_nameplates`: fourteen brass nameplates in a tin behind
   the filtration stack, "somebody took them down in the first week and could not make
   themselves throw them away". `loc_stack_roster_wall`: "Four unfaded rectangles on the
   corridor behind you are the same width as brass plates." `loc_stack_filtration`:
   "You can screw one plate under a living name. It will catch the lamp." → corridor +
   filtration histories reference this chain; do **not** explain whose names they were.
2. **The provisioning that never landed.** `echo_unopened_boots`: eight pairs of
   children's winter boots, sizes 1–4, packing note "delivery date three days **after**
   the Exchange", reference `ALLOC-12/DEP`; same crate in `loc_stack_airlock` with tape
   uncut. → deliberate, preserved impossibility (§8).
3. **Manifest vs bodies.** Manifest fourteen; eleven footboards; stencil skips 4 and 13;
   three camp-pads fill the difference; a blanket folded on an empty pad; Tamsin's
   "Manifest says fourteen. Count the pads." → the shortfall is canon texture; histories
   may show the counting, not resolve it.
4. **Charcoal bake cycle.** `room_bp_03`: "The charcoal beds must be baked out with steam
   every three months." → filtration repair/maintenance histories sit on this cycle.
5. **Improvised medical care.** `room_bp_08` (a named surgical suite) vs
   `loc_stack_clinic_alcove` ("Not a hospital. A curtain on a wire… a basin that was a
   mixing bowl"). → the gap between design and reality is *the* Ward story and is the
   natural anchor for Task 29C's "Proper Ward" renovation arc.
6. **The intercom and the apron.** cracked button (`loc_approach_hatch`), Tamsin at the
   intercom, "the wall is the document" → radio/airlock histories.

## 7. Contradictions found, and their resolution

### 7.1 Phase 1 filtration vignette motif collision — **FIXED (Phase 2)**
`room_history_the_first_filter_change` ended with a grease pencil hanging **on its
string**. Canon reserves the pencil-on-string image for the duty chart
(`loc_stack_roster_wall`: "a pencil hangs through it on a string darkened by hands").
Two pencil-on-string motifs in one shelter dilutes both. Resolution applied: the service
record is now **file notches on the canister straps** (itself canon, from
`loc_stack_filtration`: "Canisters notch-filed for days"), and the roster wall keeps the
pencil alone. The corridor keeps a separate pencil stub fixture, described by wear rather
than by its string.

### 7.2 Clinic overclaim risk — **resolved by rule**
Blueprint bp-08 documents a surgical suite ("Dr. Vel's Clinic & Surgical Suite"); the
runtime room is a curtain-on-a-wire alcove and diegetic canon says plainly "Not a
hospital." Rule: `former_use` may cite the design; `current_use`/inspection text must not
imply surgical capability. Mechanical authority (MedicalWard systems) is unchanged.

### 7.3 Generator has no room — **RESOLVED (Phase 3)**
Blueprint bp-02 documents a generator & alternator vault and bp-12 a battery/inverter
vault; mechanically the generator exists only as `PowerGridSystem` state with no spatial
room (`MACHINE_INVENTORY.md` §4). **Decision: the plant's home is `room_main` ("Main
Vault")**, the power room the live host already constructs in
`ShelterScheduleHostSession` — now whitelisted as a code-authored id in
`CatalogIntegrityValidator.KnownRuntimeIds` so identity/machine data can reference it.
Classification: non-player-facing technical space (Plan 29 §29A.1). Authority chain:
`PowerGridSystem` owns generation/fuel/battery state (no hardware wear — Phase 0 recon
recommendation (b) stands); bp-02 and bp-12 remain the engineering record for the space.
Task 29B Phase 4 binds the generator machine record to `room_main`; until then the
**generator wears no condition and its tells stay contextual-only** (fuel-starve
partial-generation state, cold-start tells from `FuelUnits`/brownout state — no new wear
field without the §29C.4 gate).

### 7.4 "Excavation expands the shelter" — **corrected**
`excavation_sites.json` is world-side (external sites); `room_command_vault` there is a
blueprint id, not a new bunker room. Origin content must therefore attribute layout
differences to **collapse, sealing and emergency conversion**, never to player
excavation (which does not exist as a shelter-construction system).

### 7.5 Timeline-year claims — **avoided**
`culinary_ration_codex` dates "the sirens" to 1981 in one flavour line while other
catalogs are era-agnostic. Plan 29 room histories use **era tags, not years**, so no new
content is pinned to a contested date. Applied: all 8 vignettes and 44 fixtures are
year-free; the only date reference is the canonical "three days after" depot form, stated
relatively.

### 7.6 Invented alias ids — **rejected during Phase 2 authoring**
Candidate legacy aliases `room_stores`, `room_medical_ward` and `room_hydroponics` were
dropped: no live shelter roster creates those ids (`room_hydroponics`/`room_medical`
appear only as power-fixture strings in `PanelBindLifecycleSelfTest`, which is test
vocabulary, not a room). The alias map therefore carries **only** the three ids real
rosters actually produce: `room_filtration_stack`, `room_air_filtration`,
`room_bunks_living`. Tier-1 validation would also reject a fabricated alias, which is the
point of the whitelist.

### 7.7 Diegetic capacity numbers vs `former_use` claims — **resolved by sourcing**
Every authored `former_use` traces to a mapped blueprint entry in §2 (blower rated for a
three-hand crew, cupola at five hundred kilos per heat, hermetic bins under an inert
blanket, peat troughs under lamp banks, lead-faced door on a counterweight winch,
three-phase busbars, Faraday screen, galley range with flue bypass, tier-3 bolted frames).
The one Phase 1 claim that was **not** canon-backed — "rated for twelve", which had
collapsed *Allocation 12* into a capacity figure — was corrected to bp-03's actual crew
size. No `former_use` asserts a number absent from §2's sources.

## 8. Deliberately unresolved (do not answer in Plan 29)

Canon preserves these as open; answering them would flatten the shelter (§8.2):

- whose names the fourteen plates carried, and why only four rectangles are unfaded;
- why the stencil skips 4 and 13 (canon compares it to building convention; no cause given);
- the packing note dated after the Exchange, and who never came to collect the boots;
- where the dental chair came from ("Dentists' Row is missing exactly this" — owner unknown);
- the child's drawing labelled with a bunk number that is **not** on the stencil;
- what the first watch wrote in the remarks column and who erased it.

## 9. Gate verdict & downstream hooks

- **Gate OPEN** for authoring: all 11 identity records and 8 vignettes may proceed, each
  traceable to §2 (former use), §4 (era), §5 (naming), §6 (chains) or role-anonymous.
- **Origin archive thread (29A.15/29A.16)** is still gated: it must use Plan 17B
  archive/document infrastructure, answer only §4-era questions, and explicitly leave §8
  open. Not authored in this pass (per instruction that continuity precedes origin).
- **29B** inherits §7.3 (generator room decision) and the blueprint `catastrophic_failure_mode`
  + `chief_engineer_note` voice as the diagnostic-tell register.
- **29C** inherits §6.5 (alcove → proper ward) as the renovation thesis, and §3's rule that
  renovation must not restate diegetic numbers as mechanical ones.

## 10. Phase 2 content audit (evidence of gate compliance)

| Check | Result |
|---|---|
| Identity records for major player-facing rooms | **11/11** (corridor, storage, bunks, kitchen, clinic, workshop, filtration, airlock, radio tuner, foundry, greenhouse) |
| `former_use` traceable to §2 mapping or diegetic canon | 11/11 |
| Vignettes | **8**, eras spread over pre-war emergency / crisis conversion / early occupancy |
| Vignette room refs resolve | 8/8 (Core `Validate()` + tests) |
| Vignettes reachable through a wired trigger | 8/8 (5 inspect · 1 repair · 2 day milestone) |
| Fixtures | **44**, ≤ 6 per room, all bidirectionally referenced, all canon-anchored or non-contradictory material traces |
| Named persons introduced | **0** (§5 rule) |
| Years asserted | **0** (§7.5) |
| §8 mysteries preserved unanswered | 5/5, explicitly (see `ROOM_HISTORY_MATRIX.md` §4) |
| Origin-thread content authored | **none** — remains gated by §9 as instructed |
| Data files edited (existing canon) | **none** — Phase 2 is purely additive |
