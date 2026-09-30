# Feature / Task Plan: The Shelter as a Place I — The Works Below (power, water and pipes as a system the player understands) & The Machine in the Walls (the shelter's own systems become a character)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Every open point is listed in §18 (Expansion backlog) and §19 (Open Mysteries).

> **Subjects covered (2 of the 16 in this batch):**
> 21. **The Works Below** — power, water and pipes as a system the player understands. (Prefix `WB`.)
> 22. **The Machine in the Walls** — the shelter's own systems become a character. (Prefix `MW`.)
>
> **Naming note.** *The Works Below* (this plan) is the shelter's **own utility works**. It is **not** *The Deep Works* (`.ai/plans/deep-works-2026-09-29.md`, prefix `DW`), which is held drifts joining the shelter shaft to the generated underground. The two plans touch at the sump only (§12).
>
> **Companions that already exist and are extended, not replaced:** `docs/expansions/wave2/expansion_21_the_grid_plan.md` (Exp. 21 The Grid — power politics), `docs/expansions/wave3/expansion_22_the_clean_flow_plan.md` (Exp. 22 — water), `docs/expansions/wave6/expansion_40_the_wheel_plan.md` (Exp. 40 — mechanical power and machine identities), and the prose waves that wrote the eleven glitch events (`docs/expansions/prose_wave93…100/`). Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data, no ledger and no other plan. Paths below are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **7b**, **10b** and **19** carry story texture and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — Below, and Behind

> *"You do not live in a shelter. You live in what the shelter is doing to keep you alive, all
> day, in the dark, under your feet and behind your head. The walls are not walls. They are
> the loudest quiet thing you own."*

A shelter is a body. It has a circulation (power), a digestion (water and drain), a lungs-and-throat
(air), a thermostat (boiler) and a door (airlock). Every one of those already exists in this game as a
careful, deterministic owner with its own numbers. What does not yet exist is **the feeling of
knowing how it fits together** — and, once you know, **the feeling of being kept alive by something
with habits**.

These two subjects are paired because they are the same acquaintance at two depths. **The Works
Below** is the *diagram*: the player learns, by walking and by fixing, what feeds what — and
therefore *why this room is dark*. **The Machine in the Walls** is the *acquaintance*: the same
machines, over a year, acquire a mood, a keeper, sometimes a name — never a voice, never a ghost.

**The binding tone rule (inherited, non-negotiable).** Exp. 40 states: *"machines have mechanical
quirks, never haunted ones."* Nothing in this plan speaks, wishes, watches or remembers *as a
character*. **The people supply the character.** A machine "grumbles" because a survivor has heard
that whistle for forty days and has started calling it a grumble. The design job is to make that
*human habit* visible, not to invent a spirit.

**Tone & register.**

- **The Works Below is written in the voice of the round.** The walk-through at night with a torch
  and a notebook; the chalk mark on a valve; the fuse drawer; the moment the ground-loop hum goes
  away because someone finally found the bad earth. Procedure as tenderness.
- **The Machine in the Walls is written in the voice of the household.** *"It coughs when it's
  cold."* *"Ines says it likes to be talked to."* A machine that has been kept by the same pair of
  hands for a hundred days is a member of the household in the way a stove is.

**What the two share.** *Attention as the resource.* Neither part gives the player a new lever; both
reward *looking*. A shelter that is understood fails gently. A shelter that is loved fails
*legibly*.

**The second layer.** The plan's deepest move is making *knowledge* a maintenance task. The Works
Board has no lies in it — only the not-yet-traced — which means every dark room is a question with
an answer somewhere in the building, and the answer is usually a decision somebody made three
steps ago, possibly the player. And the Machine in the Walls completes the thought: a shelter that
is understood can be repaired, but a shelter that is *known* — nicknamed, grumbled at, apologised
to — is one that its household has decided to keep. Character is what people put on a stove. And the
stove gives nothing back for it, which is what makes the putting pure.

---

## 1. Goal & Outcome

### 1.1 The Works Below (WB)

> *Design intent: when the clinic goes dark, the player should be able to say **why** — and
> discover that they could not have said it a month ago.*

- **Goal:** Turn the existing utility owners (`PowerGridSystem`, `PowerDistributionSubgridSystem`, `WaterTreatmentSystem`, `DeepWellSystem`, `SumpFloodingSystem`, `VentilationSystem`, `ShelterThermalSystem`, `AirlockSecuritySystem`) into **one legible diagram**: a **Works Graph** (nodes and edges, derived from existing ids plus a small authored topology), a **Cause Chain** read model that explains any *symptom* by reading the owners in order from the affected room back toward the source, a **Works Knowledge** state (what the player has *learned* by walking and fixing — the fog), a **Rounds** duty (the night walk), and a **Works Board** projection (load ladder, node loading, water and air readings, and — for nodes known by heart — forecasts).
- **Outcome (observable):**
  1. The Works Board shows, from the owners' real values, the **load ladder** (18 authored rooms in priority order against present supply, with the *cut line* where supply runs out), the 12 distribution nodes with loading %, and the water, air and heat owners' current readings.
  2. For a **symptom** (a dark room, low water, a loaded filter, a cold room), the **Cause Chain** lists each owner check in order (*room served? → node delivering? → bus? → source?*), stopping at the first step the player has not yet **traced** ("— not yet traced").
  3. A player can **Trace** a node (one survivor-day, no items) to raise its **depth** 1→2 (*Traced*: the chain can read it) and, after a real repair at that node through the owner's own verb, 2→3 (*Known by heart*: forecasts appear).
  4. A **Rounds** duty (a duty-roster post) walks one node per day at low depth and lists any **tells** the machines are making.
  5. Save/load round-trips; a legacy save with no Works state loads with depth 1 everywhere the player has been and every utility behaviour equals today's.
- **Non-Goals (Works Below):** **no change to any electrical, water, air or thermal arithmetic**; the topology is a *diagram*, not authority (DEC-WB-01); no new failure mode; no new power, water or pipe simulation; no new routed panel; no player-authored wiring or plumbing; no Unity.
- **"Done" (WB):** §13 WB acceptance passes via `bin/run-scoped-tests`; parity holds on a saved corpus with the Works state absent; handoff lists untouched shared paths.

### 1.2 The Machine in the Walls (MW)

> *Design intent: after a hundred days of one person's care, the player should be able to hear
> the difference — and the game should have written it down.*

- **Goal:** Give the seven existing **machine identities** a **character** made only of human habit: a **mood** derived from the owner's condition band; a **keeper** (the survivor assigned to its room), a count of **kept days**, a **nickname** (two are authored; the rest emerge from a *survivors-coined* event or the player's own choice); a **Settled** state after a long healthy streak; a **clue ladder** (three origin clues revealed by depth); **Night Sounds** (a read model that surfaces at most one tell per night, named or "a noise you cannot place" depending on what the player has learned); and three authored **arcs** (the Lung, the Drum, the Still) that advance only on *facts*.
- **Outcome (observable):**
  1. Each of the seven machines shows a **mood** (Content / Grumbling / Complaining / Silent, or **Settled**) derived from `ShelterMachineryReport` bands and a stored healthy streak.
  2. A machine whose room has the same assignee for ≥ 20 days is **kept**; after that, survivors *coin a nickname* (seeded pick from a data list) and the chronicle records it. The player may rename any machine at any time (≤ 24 chars, sanitised).
  3. Each night the briefing may carry **one Night Sound**: a tell from the existing quirk catalog, named if the player has traced the node and anonymous if not.
  4. Three arcs (the Lung, the Drum, the Still) advance through authored stages when *observable facts* occur (a threshold crossed, a filter replaced, a streak reached).
  5. Save/load round-trips; a legacy save with no character state loads as "no keeper, no streak" and every existing tell, glitch and audio-sync behaviour equals today's.
- **Non-Goals (Machine in the Walls):** **no haunting, no speech, no will** (Exp. 40 tone rule; DEC-MW-01); no new condition owner (condition stays with the seven existing owners); no change to quirk or glitch triggers; no new audio pipeline (existing `MachineTellAudioSync` stands); no machine "death" mechanic in v1 (§18); no new routed panel; no Unity.
- **"Done" (MW):** §13 MW acceptance passes; existing tell/glitch tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**One knowledge state.** WB's **Works Knowledge** is the *only* fog: MW's Night Sounds consult `Knowledge[node].Depth` to decide whether a tell is *named* or *anonymous*, and WB's Rounds report the tells MW selected. The two parts **share one stored structure** and never keep a second "learned" flag (Rule 5).

---

## 1b. Texture, Mystery & Voice

**The Works Below: a diagram you earn.**

The Works Board never lies. It has two states for any reading: **known** and **not yet traced**.
There is no *wrong*. That single rule is the whole tone: the shelter is honest, and the player is
slowly becoming able to read it. The first time the Cause Chain says *"Clinic dark ← node
breaker open ← overload 94% ← …"* and the player recognises **their own decision** three steps
back, the feature has worked.

**The Machine in the Walls: character is what people put on a stove.**

A machine gets a mood because someone said "it's grumbling," and then everyone did. The plan
keeps that causality *backwards from magic*: the machine does the same thing it always did; the
survivors' *vocabulary for it* grows. The game shows the vocabulary. The nickname is not the
machine's; it is a household's.

**What the player is never told.**

- **Who wired the shelter.** The machine origins (`age_origin`) hint at earlier crews; no plan names them.
- **Why the ground loop is always in the same place.** `glitch_25_ground_loop` is "the Ground Loop, Again". *Again* is authored. Never explained.
- **Whether the draft is a draft.** `glitch_21_phantom_draft` is *"a cold thread of air along the north corridor, ankle height, and every damper shut. Nobody has ever found the gap."* It stays *harmless flavour* forever.
- **What "Settled" costs.** After a hundred healthy days the machine is *Settled*. Nothing mechanical changes. The player only knows they earned something that cannot be spent.

**Voice — sample fragments (content candidates for `works_lines.json`, `machine_character_lines.json`).**

> "Depth two. I can read it now: node breaker open, overload at ninety-four. I had thought it was
> the wiring. It was me." — Rounds notebook (WB)

> "Not yet traced. The chain stops here and so do I." — Works Board footer (WB)

> "Known by heart. The transformer will want oil in eleven days; I would put the chalk mark on the
> wall, but the wall already has one." — Rounds (WB)

> "Ines has been on the Still for twenty-one days. Somebody said it gurgles like a cat that has
> been told it can stay. That is its name now." — chronicle (MW)

> "The Drum's hum dropped an octave for three seconds. Everyone in the room looked at the ceiling
> and then at each other and then at Ines, who said *that is fine*." — Night Sound, named (MW)

> "A noise you cannot place, low, in the north corridor. It stops when you stop." — Night Sound,
> anonymous (MW)

> "The Lung has stopped whistling and started breathing." — chronicle, Settled (MW)

**Design texture beats.**

- **Announce the fog, never the lie (WB).** Unknown is a state; wrong is not.
- **You learn a pipe by fixing it (WB).** Depth 3 requires a real repair through an owner's own verb.
- **The machine never speaks (MW).** Every line of "voice" is a *survivor's* line, and is attributed.
- **Attention is the currency (WB/MW).** The two parts reward *time spent looking*, never a resource.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §19's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §7b/
§10b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the round leaves lying around.**

> "Chalk mark on a valve, three marks deep. The wall already has one; the chalk is for the hand."

> "Fuse drawer, labelled in two handwritings. The second handwriting is tidier and knows less."

> "Rounds notebook: 'Not yet traced. The chain stops here and so do I.'"

**What the household leaves lying around.**

> "Nameplate, unofficial: THE STILL. Stove-paint. The machine did not accept it; the household did."

> "Oil note: eleven days. Written where the wall already had a mark."

> "Night-sound log, one line: low, north corridor, anonymous. It stops when you stop."

**Scenes the player may piece together.**

> "The hum drops an octave for three seconds. Everyone looks at the ceiling, then at each other, then at Ines, who says *that is fine*."

> "After a hundred healthy days the Lung is Settled. Nothing mechanical changes. Something else does."

**Held silences (texture, not register rows).**

- Who wired the shelter. `age_origin` hints at earlier crews; no plan names them and none ever will. Texture only.
- Whether the phantom draft is a draft. `glitch_21_phantom_draft` is harmless flavour forever; the gap is never found, because finding it would make the walls a puzzle instead of a home.

**Fourth pass — the loudest quiet thing you own (texture only; §19 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §19's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. The
binding tone rule holds: machines have mechanical quirks, never haunted ones — every characterful
line is a survivor's line.)*

**The shape of the polish.** Two voices, one building. The round writes in procedure — chalk, fuses,
readings, the tenderness of competence — and the household writes in weather: *it coughs when it's
cold*. The polish is the seam between them, and the seam is always a person. When the Works Board
says *not yet traced* and the notebook says *the chain stops here and so do I*, the fog is doing
what fog does in this plan: it is honest, it is slow, and it is almost always somebody's decision
three steps back.

**What the round leaves lying around.**

> "Works Board, night round: 'all nodes traced.' The board has no column for satisfaction. The
> round wrote it in the margin."

> "Nameplate, unofficial, second one: THE DRUM. Under it, smaller: not that one. The household has
> two names and one machine."

> "Whistle note: 'coughs when it's cold.' A weather report about a machine, filed with the tools."

**Held silences (texture, not register rows).**

- Whether the household's vocabulary ever reaches the machines. Character is what people put on a
  stove (§1b); the stove is not listening and must never be shown listening. Texture only.
- Why the north corridor. The phantom draft is ankle height in the north corridor forever (held
  silence above); its north-ness is authored and the plan declines to compass it.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `PowerGridSystem` ("single authority for shelter electrical state") owns generation/draw/battery/brownout math, per-room breakers, priority, trip marks, surge/EMP and a `Snapshot()`; public surface includes `TotalDrawWatts`, `DeficitWatts`, `SustainableBatteryDischargeWatts`, `IsRoomPowered`, `IsRoomServed`, `EffectivePriority`, `ToggleBreaker`, `SetPriority`, `ApplyBrownoutShedPreset`, `PerformGeneratorMaintenance`, `AddFuel`, `TickDay`. | `Shelter/PowerGridSystem.cs` L50–925 | LIVE |
| E2 | `power_grid.json`: default generation **800 W**, battery **4,000 Wh**, fuel **100** units, **18 rooms** with `draw_watts`, `default_priority` (critical/standard/low) and `failure_effect_id`; full draw sums to **2,910 W** (critical 910 W, standard 1,330 W, low 670 W). | `Data/power_grid.json` | LIVE |
| E3 | `PowerDistributionSubgridSystem` + `power_subgrid_nodes.json`: **12 nodes** (main vault bus 8,000 W; clinic transformer 3,500 W; workshop feed 4,500; foundry subgrid 6,000; greenhouse inverter 3,000; airlock perimeter relay 2,500; filtration stepdown 3,200; water pump junction 3,000; galley distribution 2,200; storage bay branch 1,500; living quarters bus 2,000; communications array 1,800), each with `target_room_id`, `surge_limit_watts`, `transformer_oil_condition`, `cooling_efficiency`, `fuse_rating_amps`, `is_critical`; public `FindNode`, `FindNodeForRoom`, `IsNodeDeliveringPower`, `SetBreaker`, `ReplaceFuse`, `PerformTransformerMaintenance`. | `Shelter/PowerDistributionSubgridSystem.cs`; `power_subgrid_nodes.json` | LIVE |
| E4 | **No electrical topology exists in data.** The 12 nodes each name one target room; nothing says which node *feeds* which. Exp. 21's "main vault bus" is a name, not an edge list. | `power_subgrid_nodes.json`; grep for `parent`/`feeds`/`edges` | LIVE (finding) |
| E5 | Water: `WaterTreatmentSystem` (filter integrity, `ReplaceFilter`), `DeepWellSystem` (yield ledger), `SumpFloodingSystem` + `SumpDrainageCatalog`, `AtmosphericCondenserSystem`, `BrineWaterSystem`. `water_sources.json` has 8 regional sources and 3 connections — a **regional** water map, not the shelter's pipes. | `Shelter/*`, `Water/`, `Data/water_sources.json` | LIVE |
| E6 | **The seven machine identities exist:** `machine_hepa_stack` ("The Lung", `room_filtration`), `machine_foundry_cupola`, `machine_generator`, `machine_ventilation_plant`, `machine_water_still` ("The Still", `room_water_pump`), `machine_boiler`, `machine_airlock_machinery` — each with a `condition_owner` string, `age_origin`, `baseline_sound`, `condition_key`, `quirk_ids`. Only **two** carry nicknames. | `Data/shelter_machine_identities.json` | LIVE |
| E7 | **20 quirks** (13 diagnostic, 7 personality) and **11 glitch events** (6 harmless: phantom draft, relay click, old intercom, boiler sigh, generator hum drop, still gurgle; 5 real faults: seal cycles, ground loop, stuck damper, pressure flutter, boiler cutout). Quirks carry `condition_key`, `comparison`, `trigger_below`, `text_cue`, `audio_cue`, `maintenance_action`, `repeat_policy`. | same file | LIVE |
| E8 | `MachineConditionKeys` (hepa filter health, radon, foundry components, power fuel/battery, ventilation saturation/duct integrity, water filter integrity, boiler fuel, airlock incident) and `MachineTellAudioSync.Apply(...)` (started/stopped tells, per-machine audio bus). | `Shelter/MachineIdentity/*` | LIVE |
| E9 | `ShelterMachineryReport.Build(...)` returns per-machine `Condition01` and `Band` (Healthy / Worn / Critical / Offline); it already feeds the **daily briefing** (`Main.Campaign.cs` L432–443). | `ShelterMachineryReport.cs`; `src/Main.Campaign.cs` | LIVE |
| E10 | The tell catalog is wired to `ShelterPanel` and `SilentFoundryPanel` (`SetMachineTellCatalog`), with `MachineTellAudioSync` applied from `Main.ShelterInfrastructure.cs`. Panels for power/water/thermal exist: `PowerGridPanel`, `WaterTreatmentPanel`, `ShelterThermalPanel`, `ShelterOperationsPanel`. | `src/Main.*`; `src/UI/` | LIVE |
| E11 | **Exp. 40 tone rule:** *"machines have mechanical quirks, never haunted ones."* | `docs/expansions/wave6/WAVE6_INDEX.md` L98 | LIVE (binding) |
| E12 | `ShelterAssignmentSystem` exposes `GetAssignmentsForRoom`, `GetRoomOccupancy`, `GetAssignmentForSurvivor`; `duty_roles.json` has six roles (`night_watch`, `mess`, `hatch_opener`, `intake_sleeper`, `expedition`, `ward`) and **no** works-rounds role. | `Shelter/ShelterAssignmentSystem.cs`; `Data/duty_roles.json` | **RESOLVED (§2b)** |
| E13 | `RelationsGriefSink` and `MemorialSystem` exist for death aftermath; whether a machine can cite a keeper's death is a hook, not an assumption. | `Memorial/` | LIVE (hook only) |
| E14 | `CulturalArchiveVaultSystem.TryRecordChronicleEntry` (fixed `summary_key`, deduped) and `JournalSystem.TryAddRawEntry` (free text, deduped per key). | `Culture/CulturalArchiveVaultSystem.cs` L385; `Journal/JournalSystem.cs` L281 | **RESOLVED (§2b)** |
| E15 | Save sections `shelter_maintenance` and `shelter_identity` exist (`…HostSession.SectionName`); works and machine state nests in `shelter_maintenance`. | `src/Host/ShelterMaintenanceHostSession.cs` L21 | **RESOLVED (§2b)** |
| E16 | Events exist for `OnNodeFuseBlown`, `OnTransformerMaintained`, `OnMaintenanceCompleted`; none for `ReplaceFuse`, `PerformGeneratorMaintenance`, either `ReplaceFilter`. | `Shelter/PowerDistributionSubgridSystem.cs` L47–50; `Shelter/ShelterMaintenanceSystem.cs` L89–92 | **RESOLVED (§2b)** |
| E17 | Difficulty scalars (`equipment_decay_mult`, etc.) apply at read sites; nothing in this plan changes decay. | `difficulty_presets.json` | LIVE |

**Four findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E4 — there is no electrical topology in the data.** The *only* honest way to draw the Works is an **authored topology file** (`works_topology.json`) that is **presentation-only** (DEC-WB-01). Its power edges are structural (each node has one target room); its water and air edges are *authored diagrams* flagged as such until P0 verifies which owners route those flows.
2. **E2 — the defaults are a brownout.** 800 W of generation cannot carry 2,910 W of draw; the shelter is *designed* to shed. The load ladder is therefore the single most instructive thing the Board can show a new player (§6.6).
3. **E6/E10 — the machines are already characters in embryo** (nicknames, quirks, audio) but surface mostly in two panels. MW's job is to make them **accumulate**: the same tell, over a hundred days, means something different.
4. **E11 — the tone rule is binding.** MW must be a *household's* vocabulary for machines, never the machines' own.

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

**Confirmed line for line:** E1 (`PowerGridSystem`), E2 (`power_grid.json`: generation 800 W, battery 4,000 Wh, fuel 100, **18 rooms drawing 2,910 W** — the shelter is *designed* to brown out), E3 (12 subgrid nodes), E6 (seven machine identities), E7 (**20 quirks**, **11 glitch events**), E10 (`PowerGridPanel`, `ShelterOperationsPanel` both exist in `src/UI/`). What the open items resolved to:

| # | Open item | Result | Edit made |
|---|---|---|---|
| E12 | Room-post assignee readable per room? | **Yes.** `ShelterAssignmentSystem.GetAssignmentsForRoom(roomId)`, `GetRoomOccupancy`, `GetAssignmentForSurvivor` exist. A Keeper is therefore a *read* of the assignment owner, never a copy. | Keeper resolution uses `GetAssignmentsForRoom` |
| E12 | A post named `post_works_rounds`? | **Not as named.** The duty roster's `duty_roles.json` has **six roles** (`night_watch`, `mess`, `hatch_opener`, `intake_sleeper`, `expedition`, `ward`); each row carries `skill_id`, `minimum_skill`, `maximum_fatigue`, `minimum_health`, `maximum_dose_msv`, `maximum_hours`, `hazard_class`, … — no room binding. A Rounds duty is one additive role row (`works_rounds`, skill and hazard class authored) plus the plan's own loop. | **DEC-WB-05 resolved**: Rounds is a duty **role** (additive row), the *loop* is the plan's |
| E14 | Chronicle writer | Two writers with different powers: `CulturalArchiveVaultSystem.TryRecordChronicleEntry(day, eventType, summaryKey, participants, authorId, volumeId)` stores a **fixed `summary_key`** (deduped); `JournalSystem.TryAddRawEntry(key, text, author, day)` takes composed free text. | fixed Works milestones → chronicle keys; composed notebook lines → journal |
| E15 | Save home | Two real candidates exist as sections: **`shelter_maintenance`** (`ShelterMaintenanceHostSession.SectionName`) and **`shelter_identity`**. Works and machine-character state is *upkeep history* — the maintenance section is the truthful owner. | **DEC-WB-02 resolved**: nest in `shelter_maintenance` |
| E16 | Do repair verbs raise events? | **Partly.** `PowerDistributionSubgridSystem` raises `OnNodeFuseBlown(nodeId)` and `OnTransformerMaintained(nodeId)`; `ShelterMaintenanceSystem` raises `OnMaintenanceCompleted(record)` (wired in `ShelterMaintenanceHostSession`). **`ReplaceFuse`, `PerformGeneratorMaintenance` (returns `bool`), `WaterTreatmentSystem.ReplaceFilter` and `VentilationSystem.ReplaceFilter` raise nothing.** `ReplaceAirFilter` / `ServiceAirFilter` live in `StartingLevelSystem` (a surprising owner). | depth-3 credit subscribes where an event exists and **wraps the verb in the host** where it does not (P-level, listed per verb in §7) |
| — | Auxiliary power sources | **Only solar exists** (`SolarConcentratorEngine`). There is no wind or hand-crank owner and `PowerGridSystem` has no auxiliary fields. | `src_aux_wind` and `src_aux_hand_crank` **removed**; sources are `src_generator`, `src_battery_bank`, `src_aux_solar` (present only when the solar engine is) |
| — | Water and air owners | `WaterTreatmentSystem` (root `Assets/Ashfall.Core/`) and `VentilationSystem` (root) — **not** under `Shelter/`. | paths in §4 corrected |
| — | Panel for the Works Board | `ShelterOperationsPanel` and `PowerGridPanel` exist. | Board is a tab in `ShelterOperationsPanel`; machine strip in `ShelterPanel` |

**What did not change:** the authored `works_topology.json` (presentation-only; power edges structural, water and air edges authored and flagged), the four depths of knowledge, the Cause Chain, the seven machine identities and the tone rule. The evidence removed two invented power sources and one invented duty post, and named the real save home.

**One consequence worth stating plainly.** Most of the repair verbs the plan wants to *credit* say nothing when they succeed. The household's understanding of the machines cannot be built on events the machines do not raise; it is built on what the **host** watches people do. That is the right shape for a plan whose whole subject is *paying attention*.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Electrical state, breakers, priority, brownout, surge | `PowerGridSystem` | none |
| Distribution nodes: breaker, fuse, load, oil, temperature | `PowerDistributionSubgridSystem` | none |
| Water treatment, filter integrity, wells, sump | `WaterTreatmentSystem`, `DeepWellSystem`, `SumpFloodingSystem` | none |
| Air: ventilation, HEPA | `VentilationSystem`, `StartingLevelSystem.airFilterHealthPercent` | none |
| Heat, boiler | `ShelterThermalSystem` | none |
| Airlock machinery | `AirlockSecuritySystem` | none |
| Machine condition, quirks, glitches, audio | the seven owners + `ShelterMachineTellCatalog` + `MachineTellAudioSync` | none |
| Machinery band | `ShelterMachineryReport` | read-only |
| Works topology (nodes, edges) | — | `works_topology.json` (**data**, presentation-only) + `WorksGraph` (pure read model) |
| Cause explanation | — | `WorksCauseChain` (pure read model over the owners) |
| What the player has learned | — | `WorksKnowledge` (stored: depth per node) |
| Machine character (keeper, kept days, streak, nickname, stage) | — | `MachineCharacterSystem` (stored: small per-machine records) |
| Night Sounds | — | `NightSoundsReadModel` (pure) |
| Chronicle | Plan 34 / `ArchiveDeskState` | append-only writes |

**Non-duplication statement.** Every utility fact stays with its owner. The Works Graph is a **diagram** of ids that already exist (rooms, nodes, machines) plus **six** new ids for things that have no id (deep well, sump, pneumatic trunk, aux sources, battery bank, storage tanks). The Cause Chain **reads**; it never computes a new failure. The two stored structures — **learned depth per node** and **character per machine** — hold only *what the player has learned* and *what the household has come to call things*.

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (WB):** `Assets/Ashfall.Core/Shelter/Works/WorksGraph.cs` (new, pure), `Works/WorksTopologyCatalog.cs` (new loader), `Works/WorksCauseChain.cs` (new, pure), `Works/WorksBoard.cs` (new, read-only projection), `Works/WorksKnowledge.cs` (new, state + rules), `Works/WorksRounds.cs` (new).

**Core (MW):** `Assets/Ashfall.Core/Shelter/MachineIdentity/MachineCharacterSystem.cs` (new), `MachineIdentity/MachineMood.cs` (new, pure derivation), `MachineIdentity/NightSounds.cs` (new, pure), `MachineIdentity/MachineArcCatalog.cs` (new loader). `Shelter/ShelterIdentitySystem.cs` or `Shelter/ShelterMaintenanceSystem.cs` (**additive nested DTO only**, `INT`, home chosen at P0).

**Data (WB):** `works_topology.json`, `works_symptoms.json`, `works_trace_lines.json`, `works_lines.json`.
**Data (MW):** `machine_characters.json`, `machine_arcs.json`, `machine_night_lines.json`, `machine_character_lines.json`.

**Host:** `src/Main.ShelterInfrastructure.cs` (`INT`: knowledge + character binding, Rounds), `src/Main.Campaign.cs` (`INT`: briefing line for Night Sound), phase-5 day-owner registration (`src/Main.SubsystemComposition.cs`, `INT`).

**Presentation (both):** extend existing surfaces only — `ShelterOperationsPanel` (verified §2b; `PowerGridPanel` for the power layer) for the **Works Board**; `ShelterPanel` for the **machine strip**; the daily briefing for **Night Sounds**. No new routed panel (DEC-WB-08, DEC-MW-08).

**Tests:** `Ashfall.Core.Tests/Shelter/Works/WorksGraphTests.cs`, `WorksCauseChainTests.cs`, `WorksKnowledgeTests.cs`, `WorksBoardTests.cs`, `Save/WorksSaveTests.cs`; `Ashfall.Core.Tests/Shelter/MachineIdentity/MachineMoodTests.cs`, `MachineCharacterSystemTests.cs`, `NightSoundsTests.cs`, `MachineArcTests.cs`, `Save/MachineCharacterSaveTests.cs`; extend existing tell/glitch tests for unchanged-behaviour parity.


---

# PART ONE — THE WORKS BELOW

## 5. The Works Graph (a diagram of ids that already exist)

### 5.1 Node kinds

| Kind | Source of the id | Count | Notes |
|---|---|---|---|
| `source` | authored ids: `src_generator`, `src_battery_bank`, `src_aux_solar` | 3 | Generator maps to `machine_generator`; solar appears only when `SolarConcentratorEngine` is present (no wind or hand-crank owner exists — §2b) |
| `bus` | authored: `bus_main_vault` | 1 | Maps to the "main vault bus" distribution node |
| `node` | existing `power_subgrid_nodes.json` ids | 12 | Each has `target_room_id` |
| `room` | existing `power_grid.json` room ids | 18 | Each has `draw_watts`, `default_priority` |
| `water` | authored: `wtr_deep_well`, `wtr_sump`, `wtr_still`, `wtr_tanks`, `wtr_condenser` | 5 | `wtr_still` maps to `machine_water_still` |
| `air` | authored: `air_intake`, `air_hepa`, `air_trunk`, `air_vent_plant` | 4 | `air_hepa` maps to `machine_hepa_stack` |
| `heat` | authored: `heat_boiler`, `heat_loop` | 2 | `heat_boiler` maps to `machine_boiler` |

Six ids have no prior existence anywhere (deep well is a system but not a graph id; sump, tanks, trunk, loop, bank are diagram-only). They are *labels*, never keys into any save.

### 5.2 Edge kinds

`feeds` (power), `pipes` (water), `ducts` (air), `carries` (heat). An edge is `{from, to, kind, verified}`. `verified: true` means the owner's own data proves the edge (e.g. a subgrid node's `target_room_id`). `verified: false` means **authored diagram only** and the Board shows it dashed with the caption *"as drawn on the wall"*. P0 attempts to promote authored edges to verified where an owner routes the flow; any edge that cannot be verified **stays dashed**. Honest uncertainty is a feature (§1b).

### 5.3 Topology rules (validator)

- **W-1** every node id in `works_topology.json` is unique; every `room`/`node`/`machine` reference resolves in its owner catalog.
- **W-2** every `room` node has exactly one incoming `feeds` edge; every subgrid `node` has exactly one incoming `feeds` edge from `bus_main_vault` or another node (no cycles; acyclic check).
- **W-3** every power edge into a `room` from a `node` must match `node.target_room_id` (this is the **only** rule by which power edges are `verified: true`).
- **W-4** `pipes`/`ducts`/`carries` edges are `verified: false` unless P0 records an owner-proof note in the row (`proof` string, ≤ 120 chars).
- **W-5** every machine id in `shelter_machine_identities.json` appears as exactly one graph node's `machine_id`.
- **W-6** no edge crosses kinds except a documented list (`src_generator → bus_main_vault`; `heat_boiler` fed by fuel is *not* an edge — fuel is not a graph concept).
- **W-7** `label` ≤ 32 chars; `blurb` ≤ 140 chars.

### 5.4 Presentation only (DEC-WB-01)

The graph changes **no arithmetic**. `PowerGridSystem` continues to compute served rooms from its own breakers/priorities; the graph *reads* `IsRoomServed`, `IsNodeDeliveringPower`, `TotalDrawWatts`, `DeficitWatts`. If graph and owner ever disagree the owner is right and a **test** fails (parity test WB-T2), not the owner.

### 5.5 Worked example — the clinic

Authored rows (excerpt):

```json
{ "id": "node_clinic_transformer", "kind": "node", "label": "Clinic transformer", "source_ref": "power_subgrid_nodes:clinic_transformer" }
{ "from": "bus_main_vault", "to": "node_clinic_transformer", "kind": "feeds", "verified": false }
{ "from": "node_clinic_transformer", "to": "room_clinic", "kind": "feeds", "verified": true }
```

The second edge is `verified: false` because the data proves only *node → room* (E4). The Board draws it dashed. The third edge is solid.

---

## 6. The Cause Chain (why is this dark?)

### 6.1 The shape

A **symptom** is an authored row: `{id, room_or_system, text, chain_kind}`. `chain_kind` selects an ordered list of **checks**, each a pure predicate over an owner:

**Power chain (`chain_power`), for a room `R`:**

| Step | Check (owner call) | Explains as |
|---|---|---|
| 1 | `IsRoomPowered(R)` false and breaker of `R` open | "The room's own breaker is open." |
| 2 | node for `R` exists and `IsNodeDeliveringPower(node)` false, cause = node breaker open | "The feeding node's breaker is open." |
| 3 | …cause = fuse blown | "The node fuse is blown." |
| 4 | …cause = load over `surge_limit_watts` | "The node is overloaded (94%)." |
| 5 | grid tripped `R` (`IsRoomTripped`) | "The grid tripped this room after a surge." |
| 6 | room shed by brownout priority (`IsRoomServed(R)` false and `EffectivePriority(R)` below the cut line) | "The room is below the cut line: supply ends before it." |
| 7 | source deficit: generation + battery cannot cover draw; generator without fuel | "The source is short: no fuel / generator worn." |

The chain returns **the first failing step** and the **remaining steps as "not reached"**. If the player has not traced the node at step *k*, the chain returns steps 1..*k−1* and a final line **"— not yet traced"** (§7).

**Water chain (`chain_water`)**, **Air chain (`chain_air`)**, **Heat chain (`chain_heat`)** follow the same pattern over their owners: e.g. water: *tap dry ← tanks empty ← still starved ← filter integrity low / pump node unpowered ← deep well yield / sump flooded*. Each check is a **named read-only predicate** bound in the host (`IWorksProbe`) so Core never references a Godot type and the chain can be unit-tested with a fake probe.

### 6.2 Derivation, never storage

The chain is recomputed on demand. **Nothing about the chain is saved** — only what the player has *traced* (§7). This is the same principle as the Long Siege readiness board: **derive, do not store**.

### 6.3 The probe seam

```csharp
public interface IWorksProbe
{
    bool IsRoomServed(string roomId);
    bool IsRoomBreakerOpen(string roomId);
    bool IsRoomTripped(string roomId);
    int  EffectivePriority(string roomId);
    bool IsNodeDelivering(string nodeId);
    NodeFault NodeFault(string nodeId);   // None, BreakerOpen, FuseBlown, Overload, OilLow, Hot
    int  TotalDrawWatts();
    int  SupplyWatts();
    float Condition01(string machineId);
}
public static class NullWorksProbe { /* returns healthy, everything served */ }
```

One additive probe adapter in `src/` (`INT`) binds each method to the existing public owner calls listed in E1/E3/E9. **No owner gains a method.** (If a check needs a value no public owner exposes — e.g. node overload cause — P0 records it as a finding and the check degrades to "cause unknown" rather than adding an owner API.)

### 6.4 Worked example — "Why is the clinic dark?"

State: day 61. Clinic transformer node breaker **open** (the player opened it on day 58 to save the filtration node during a surge and forgot). Filtration node depth 3; clinic node depth 2.

Chain output:

1. ✔ Room `clinic` breaker closed.
2. ✘ **Feeding node breaker is open.** *(Traced: depth 2.)*
3. — *not reached*: fuse; load; source.

Suggested action line (from `works_trace_lines.json`): **"Close the clinic transformer breaker (Power panel). The load on the bus is 910 W of 967 W — closing it will not trip the ladder."** The forecast clause appears **only** because the bus node is known by heart (depth 3).

### 6.5 Worked example — the same room, less known

Same state, clinic node depth **1** (Seen). Output:

1. ✔ Room breaker closed.
2. — **not yet traced.** *"The chain stops here and so do I."*

The player must *Trace* the node (§7.2). This is the whole fog mechanic: *the shelter tells the truth to those who have looked*.

### 6.6 The load ladder (the most instructive single read)

The Board lists the 18 rooms in `EffectivePriority` order (critical → standard → low; ties by authored order) with cumulative draw. Against the authored defaults:

| Band | Rooms' draw (W) | Cumulative (W) |
|---|---|---|
| Critical (rooms authored critical) | 910 | 910 |
| + Standard | 1,330 | 2,240 |
| + Low | 670 | 2,910 |

With default generation 800 W the **cut line falls inside the critical band** unless the battery discharges (`SustainableBatteryDischargeWatts`); with the battery at 4,000 Wh and a sustainable discharge of roughly 167 W (illustrative — read, never compute, at runtime) supply is **≈ 967 W**, so critical (910 W) is *just* covered and **everything else is off**. The Board draws the cut line as a bar across the ladder. **This is the first thing a new player learns and it is drawn, not told.** (Illustrative arithmetic; the runtime reads the owner.)

---

## 7. Works Knowledge (the fog)

### 7.1 Depths

| Depth | Name | How reached | What it unlocks |
|---|---|---|---|
| 1 | **Seen** | node exists in the graph; player opened the Board once | node drawn, label shown, readings shown |
| 2 | **Traced** | one **Trace** (survivor-day, no items) | the Cause Chain may *read through* this node |
| 3 | **Known by heart** | Traced **and** a real repair credited at this node through an owner verb (`ReplaceFuse`, `PerformTransformerMaintenance`, etc. — event or host wrapper, E16) **or** 5 completed Rounds at the node | forecasts (next fault window, oil days, fuse margin) |

Depth never decays. Depth never lowers. There is no "forgetting" in v1 (§18 backlog: *Stale Knowledge* after a rebuild).

### 7.2 The two new verbs

- **Trace(node)** — assigns one healthy survivor for one day (through the existing duty/assignment owner — `ShelterAssignmentSystem`, verified §2b); costs no item; raises depth 1→2 if the node is the *first untraced* along a chain from a known node (you cannot trace the far end of the shelter before you have walked the near end — **adjacency rule**, keeps the diagram earned).
- **Walk(node)** — a Rounds step (below); a walk at a known node *can* surface a tell (§10.3) and counts toward depth 3.

Both are ordinary commands routed through the existing survivor-assignment and day-tick seams; **neither mutates any utility owner**.

### 7.3 Stored shape (additive nested DTO — no new save section)

```csharp
public sealed class WorksKnowledgeState
{
    public int SchemaVersion = 1;
    public Dictionary<string, WorksNodeKnowledge> Nodes = new();   // node id → record
    public string LastRoundsNodeId;
    public int RoundsCompleted;
}
public sealed class WorksNodeKnowledge
{
    public int Depth = 1;          // 1..3
    public int TracedDay = -1;
    public int RepairsCredited;
    public int WalkCount;
}
```

Home: nested in `ShelterIdentitySave` (or the shelter-maintenance save) per P0 (DEC-WB-02). A legacy save without the field loads as *"every node depth 1; none traced"* and every utility behaviour equals today's.

### 7.4 Rules

- **K-1** Depth ∈ {1,2,3}, monotone non-decreasing.
- **K-2** Trace requires an adjacent known node (or `bus_main_vault`, always Traced after first Board open).
- **K-3** Depth 3 needs `RepairsCredited ≥ 1` **or** `WalkCount ≥ 5`; both counters are facts credited by the host on real acts.
- **K-4** `TracedDay` is monotone; re-tracing a Traced node is a no-op (no day spent, no refusal noise).
- **K-5** Knowledge never gates *a utility verb* — a player can close any breaker at any depth. The fog gates **explanation**, never **action**.

**K-5 is the key design rule.** The fog hides *why*, not *how*. A player who never traces anything plays exactly today's game. WB is a reward for looking, not a gate.

---

## 7b. Texture — the round

**The Rounds duty.**

Rounds is a duty-roster **role** (`works_rounds`, one additive row in `duty_roles.json` — six roles ship and none is a works role; §2b). The assignee walks *one node per day* in a fixed loop (bus → clinic transformer → filtration stepdown → water pump junction → galley → living quarters → workshop → foundry → greenhouse → airlock relay → storage → comms → back). The loop is authored (`works_topology.json: rounds_order`). A day's walk produces a **notebook line** (`works_lines.json`) chosen by the node's state:

- *Healthy, depth < 3:* "Bus quiet. Chalk mark unchanged."
- *Healthy, depth 3:* "Oil at eleven days. I would not touch it. I would like to."
- *Tell heard:* "North corridor: the relay clicked twice. I have written it down."
- *Fault:* "Fuse blown at the galley node. Replaced from the drawer. The drawer is lighter."

**What the notebook is for.** The notebook is the chronicle of the Works: a rolling 30-line log surfaced in the Board footer. Old lines age out; the **first** line ever written at each node and every **fault line** are kept forever (bounded; ≤ 12 kept lines).

**Small cruelties.** A missed day of Rounds is not a penalty; it is a *gap in the notebook*, rendered as **"— (no round)"**. Nothing else happens. The player sees the gap and does what they like with it.


---

# PART TWO — THE MACHINE IN THE WALLS

## 8. Machine Character (household vocabulary over existing identities)

### 8.1 Stored record (per machine, nested; no new save section)

```csharp
public sealed class MachineCharacterState
{
    public int SchemaVersion = 1;
    public Dictionary<string, MachineCharacter> Machines = new();   // machine id → record
    public List<string> ChronicleKeys = new();                      // idempotence guard
}
public sealed class MachineCharacter
{
    public string KeeperId;              // survivor id currently assigned to the room; null if none
    public int    KeptDays;              // consecutive days with the same keeper
    public int    LongestKeptDays;
    public int    HealthyStreakDays;     // consecutive days band == Healthy
    public string Nickname;              // null until authored/coined/player-set
    public string NicknameSource;        // "authored" | "coined" | "player"
    public int    ClueDepth;             // 0..3
    public int    ArcStage;              // 0..n (per-machine arc)
    public int    LastNightSoundDay = -1;
}
```

Home: nested beside `WorksKnowledgeState` in the same host save DTO (DEC-MW-02). **Everything else is derived**: mood, "kept" flag, "Settled" flag, whether a Night Sound is named.

### 8.2 Moods (derived, never stored)

| Band (from `ShelterMachineryReport`) | Streak | Mood |
|---|---|---|
| Healthy | ≥ 100 days | **Settled** |
| Healthy | < 100 | **Content** |
| Worn | any | **Grumbling** |
| Critical | any | **Complaining** |
| Offline | any | **Silent** |

Only `HealthyStreakDays` is stored. A band change to non-Healthy resets the streak to 0 (a fact, credited by the day-owner). `Settled` is a *label*; it has **no mechanical effect** (DEC-MW-03 ties to §1b: "what Settled costs").

### 8.3 Keeper, kept days

- Keeper = the survivor returned by `ShelterAssignmentSystem.GetAssignmentsForRoom(machine.room_id)` (verified §2b).
- Each day: same keeper → `KeptDays++`; changed keeper → `KeptDays = 0`, new keeper recorded; no keeper → `KeptDays = 0`, `KeeperId = null`.
- **Kept** ⇔ `KeptDays ≥ 20`.
- Keeper death → the machine's record keeps `LongestKeptDays`; a **keeper-grief** line is queued for the chronicle (once), via `RelationsGriefSink` hook (§12, `NullMachineGriefSink` default).

### 8.4 Nicknames

- **Authored** (2 today): The Lung, The Still — `NicknameSource = "authored"`, present from day 0.
- **Coined:** when a machine becomes **kept**, and has no nickname, a seeded pick from `machine_characters.json: coined_names[machine_id]` (3–5 names each, tone: plainspoken, affectionate, never eerie). Stream: `CampaignStreamIds` fork `machine_nickname:<machine_id>` — deterministic per machine, independent of call order.
- **Player:** `Rename(machine_id, text)` — trim, ≤ 24 chars, control chars stripped, non-empty; `NicknameSource = "player"`. A player name is never overwritten.
- The chronicle receives one line at coin time (idempotent key `nick:<machine_id>`).

### 8.5 The clue ladder (three origin clues per machine)

`age_origin` (already authored) is **clue 0** — free. Clues 1–3 are revealed by `ClueDepth`, raised by *facts*:

| Depth | Trigger (fact) | Reveals |
|---|---|---|
| 1 | machine **kept** (≥ 20 days) | a maker's mark or repair scar the keeper notices |
| 2 | first **repair** credited at the machine's node/owner | what the previous repair left behind (a part that doesn't match) |
| 3 | **Settled** (≥ 100 healthy days) *or* an arc's final stage | what the keeper decides it was for |

Every clue is **a physical detail**, never a backstory. No clue names a person or explains a *why* (§19).

### 8.6 The seven machines — mood vocabulary and authored clue seeds

| Machine | Owner of condition | Nickname | Voice of the mood (sample) | Clue 1 seed |
|---|---|---|---|---|
| The Lung (`machine_hepa_stack`) | `airFilterHealthPercent` | authored | Grumbling: "whistles on the exhale" | frame stamped with a shift number, painted over |
| The Drum (`machine_generator`) | `PowerGridSystem` | coined | Complaining: "hunts its own speed" | a second nameplate riveted over the first |
| The Still (`machine_water_still`) | `WaterTreatmentSystem` | authored | Grumbling: "gurgles like a cat told it can stay" | tally marks scratched inside the access hatch |
| The Bellows (`machine_ventilation_plant`) | `VentilationSystem` | coined | Complaining: "coughs when the wind turns" | a damper handle worn bright on one side only |
| The Cupola (`machine_foundry_cupola`) | `SilentFoundrySystem` | coined | Grumbling: "ticks as it cools" | fire-brick with a child's thumbprint fired in |
| The Kettle (`machine_boiler`) | `ShelterThermalSystem` | coined | Complaining: "sighs before it cuts out" | a chalk tally of cold nights on the flue |
| The Gate (`machine_airlock_machinery`) | `AirlockSecuritySystem` | coined | Grumbling: "clicks twice before it commits" | a manual override wheel that is *newer* than the door |

(The *coined_names* pools are seeds; authors extend them. Names in the first column are working titles only until coined or authored — the runtime shows the *nickname* if set, otherwise the machine's authored plain label.)

---

## 9. Night Sounds

### 9.1 The read model

Each night the briefing may carry **at most one** Night Sound. `NightSounds.Select(day, machines, quirks, knowledge, rng)`:

1. Candidate tells = quirks (existing catalog) whose `condition_key` and threshold are **currently met** for their machine (already the tell catalog's rule; reuse its predicate — no new trigger).
2. Drop machines whose `LastNightSoundDay ≥ day − 2` (no repetition within 3 nights).
3. If none: no sound (silence is valid and common).
4. Else pick one with the seeded stream `night_sound:<day>`; record `LastNightSoundDay`.
5. Render **named** if `Knowledge[machine.node].Depth ≥ 2` (WB seam, §1.3) — using the nickname and the quirk's `text_cue` — else **anonymous** ("a noise you cannot place, low, in the north corridor. It stops when you stop.").

### 9.2 What it never does

- It never creates a tell the quirk catalog would not create.
- It never repeats a **glitch event** (the 11 existing glitches keep their own scheduler and are *not* Night Sounds).
- It never speaks. It is a *survivor* who heard something (line attributed to a survivor when a keeper exists, else "someone").

### 9.3 Audio

`MachineTellAudioSync` is unchanged. The Night Sound *line* is text in the briefing; the existing audio bus continues to play only what it plays today (DEC-MW-06 — no new audio cues in v1).

---

## 10. The three arcs (facts advance them; nothing else does)

An arc is an ordered list of **stages**; each stage has a **fact predicate** (a pure function of observable state) and a **chronicle line**. Stages fire at most once (idempotent by key `arc:<id>:<stage>`); the arc's stored state is only `ArcStage` on the machine record.

### 10.1 The Lung — "Breathing"

| Stage | Fact | Line (candidate) |
|---|---|---|
| 1 | HEPA filter replaced for the **first** time | "The Lung stopped whistling. Marisol said it had never really been whistling; it was asking." |
| 2 | Band Healthy for 30 days after a Critical spell | "It breathes in fives now. Someone has begun counting along." |
| 3 | Settled | "The filtration room has a chair now. Nobody put it there on purpose." |

### 10.2 The Drum — "Hunting"

| Stage | Fact | Line |
|---|---|---|
| 1 | first `glitch_24…28`-class generator fault survived without blackout | "It hunted for its speed and found it. The room let out one breath." |
| 2 | generator maintenance credited while fuel < 25% | "Oiled it with the last of the good can. It ran quieter, which is not the same as better." |
| 3 | Settled | "The Drum's hum is the floor now. When it dropped for three seconds nobody spoke." |

### 10.3 The Still — "Gurgling"

| Stage | Fact | Line |
|---|---|---|
| 1 | filter replaced after integrity < 30% | "The Still coughed and then ran clear. Someone laughed, badly." |
| 2 | kept ≥ 20 days | "Ines keeps a cup by the outflow. She says it is not for drinking." |
| 3 | Settled | "It gurgles like a cat that has been told it can stay. That is its name." (Uses authored nickname.) |

### 10.4 Rule — "the machine never speaks; it changes"

Every arc line describes **what the household did or noticed**, or **what the machine's sound became**. No line attributes intent, feeling or memory to a machine. Validator rule **M-8** (§11) rejects any line containing a *machine-as-subject-of-mind* verb from a small deny-list (`wanted`, `remembered`, `wished`, `decided`, `knew`, `watched`, `wept`, `forgave`) unless the subject is a survivor.

---

## 10b. Texture — the household

**How a name arrives.** On day 21 of Ines' tenure at the Still the briefing carries: *"Someone said the Still gurgled like a cat that had been told it could stay. Nobody argued."* That is the entire mechanism: a seeded pick, a chronicle line, and a household that now calls it that.

**Settled.** A Settled machine has a chair beside it. The chair is not a system. It is a line in a row of prose that unlocks at 100 healthy days. The player will never be able to *spend* Settled. It will simply be there, in the room, on the map, in the log.

**When the keeper dies.** The machine's mood does not change. The line the game writes is about the room: *"The Kettle sighed and cut out at four. Ambrose's chair is still by the flue. Nobody has moved it. Nobody has sat in it."* Then nothing. The machine continues. That is the tone.

**Rename as ritual.** The rename text field is a small affordance. Its only rule is the player's: ≤ 24 characters. A machine renamed by its player after a keeper's death is the only place the game lets the player be sentimental *out loud*.


---

# PART TWO-B — DEPTH: WORKED CHAINS, AUTHORED ROWS, A YEAR IN THE WALLS

> This part deepens Parts One and Two with worked examples, authored catalog rows, interaction specs and edge cases. Nothing here changes an authority, path or decision; row text is content candidates for the JSON catalogs.

## 10c. The other three chains, worked

### 10c.1 Water chain (`chain_water`)

| Step | Check | Explains as |
|---|---|---|
| 1 | tap/ration reads zero while tanks > 0 | "The tanks have water; the tap does not. Look at the distribution." |
| 2 | tanks empty | "The tanks are dry." |
| 3 | still starved: `WaterTreatmentSystem.filterIntegrity` below its own floor | "The Still is running on a spent filter." |
| 4 | water pump junction node not delivering (power chain, subject = pump room) | "The pump has no power; see the power chain for the pump junction." |
| 5 | deep well yield below draw | "The well is giving less than the shelter drinks." |
| 6 | sump flooded (`SumpFloodingSystem`) | "The sump is over its mark; the pump is fighting the ground." |
| 7 | condenser idle (no power or no humidity) | "The condenser is not making up the difference." |

Worked example, day 74. Tanks at 12%, filter integrity 22%, pump node delivering, well yield low after a dry fortnight. Pump junction depth 2, Still depth 1.

1. Tap reads low while tanks are not empty: pass (tanks 12%).
2. Tanks dry: no, low but not empty.
3. Still starved: **yes, but the Still node is depth 1** so the chain prints: "Water is low. The chain reaches the Still and stops: not yet traced."

The player traces the Still (adjacent to the pump junction, which is Traced, so the adjacency rule is satisfied). The next read prints step 3 fully: "The Still is running on a spent filter (integrity 22%)". Suggested action: "Replace the filter (Water panel)." That is a real owner verb; the chain never performs it.

### 10c.2 Air chain (`chain_air`)

Steps: intake blocked or outside air poor (read-only from the air owner) → ventilation plant saturation high → duct integrity low → HEPA health low → power to filtration node lost → radon reading elevated. First failing step wins.

Worked example, day 96. HEPA health 34%, ventilation saturation 0.7, filtration node delivering. Chain: intake fine; saturation 0.7 is under its own threshold; duct integrity fine; **HEPA health under the tell threshold** → "The Lung is worn (34%). Replace or service the filter." If the filtration node were depth 1 the chain would still print step 4 because HEPA reads are Seen-level facts (the Lung's health is a shown reading, not an explanation). **Rule K-6: raw readings are always shown; the fog hides only the *causal link* between readings.**

### 10c.3 Heat chain (`chain_heat`)

Steps: room cold → boiler fuel low → boiler cutout recorded → loop carries heat to the room (authored dashed edge) → insulation/leak state (if the owner exposes it; else "cause unknown"). Where a step cannot be read the chain prints "cause unknown" and stops; it never invents.

### 10c.4 Rule K-6 (added)

Readings (numbers on the Board) are never fogged. Only the *explanation lines* that link one reading to another are. This keeps the Board honest for a player who ignores the fog entirely.

## 10d. The 24 authored symptoms (catalog rows)

| # | id | Subject | Chain | Text (candidate) |
|---|---|---|---|---|
| 1 | `sym_clinic_dark` | room_clinic | power | The clinic is dark. |
| 2 | `sym_filtration_dark` | room_filtration | power | The filtration room has no power. |
| 3 | `sym_pump_dark` | room_water_pump | power | The pump room is dead quiet. |
| 4 | `sym_galley_dark` | room_galley | power | The galley lights are out. |
| 5 | `sym_workshop_dead` | room_workshop | power | The workshop bench is silent. |
| 6 | `sym_greenhouse_dim` | room_greenhouse | power | The grow lights sag. |
| 7 | `sym_airlock_relay` | room_airlock | power | The airlock relay will not answer. |
| 8 | `sym_comms_static` | room_comms | power | The radio room is only static. |
| 9 | `sym_quarters_cold_dark` | room_quarters | power | Sleeping quarters are dark and cold. |
| 10 | `sym_storage_alarm` | room_storage | power | The storage bay alarm is silent when it should not be. |
| 11 | `sym_brownout_all` | shelter | power | Everything but the critical rooms is off. |
| 12 | `sym_flicker` | shelter | power | Lights flicker on the hour. |
| 13 | `sym_tap_dry` | shelter | water | The tap runs dry. |
| 14 | `sym_water_cloudy` | shelter | water | The water is cloudy. |
| 15 | `sym_pump_labouring` | room_water_pump | water | The pump labours and never catches up. |
| 16 | `sym_sump_high` | room_water_pump | water | The sump is over its mark. |
| 17 | `sym_air_stale` | shelter | air | The air is stale. |
| 18 | `sym_air_dust` | shelter | air | Dust hangs in the light. |
| 19 | `sym_radon_up` | shelter | air | The radon meter has crept up. |
| 20 | `sym_vent_cough` | machine_ventilation_plant | air | The vents cough on every turn of the wind. |
| 21 | `sym_room_cold` | shelter | heat | The far rooms are cold. |
| 22 | `sym_boiler_cut` | machine_boiler | heat | The boiler cut out in the night. |
| 23 | `sym_hum_new` | shelter | power | There is a new hum in the walls. |
| 24 | `sym_smell_hot` | shelter | power | Something smells warm and wrong. |

(Room ids above are working labels; P0 replaces each with the real id from `power_grid.json`. Rows 23 and 24 map to the ground-loop and node-hot faults through the power chain's oil and heat step.)

### 10d.1 Suggested-action lines (excerpt)

- Breaker open: "Close the {node} breaker (Power panel). Load on the bus is {load} of {supply}." The clause "Load on the bus…" appears only at depth 3.
- Fuse blown: "Replace the fuse from the drawer (Power panel). The drawer holds {n}."
- Overload: "Shed a low-priority room first, then close it. Preset: Brownout Shed."
- Oil low: "Service the transformer. It will run, but it is running warm."
- Below cut line: "This room sits under the cut line. Raise its priority, or raise supply."
- Filter spent: "Replace the filter (Water panel)."
- HEPA worn: "Service or replace the Lung's filter."
- Boiler fuel: "Feed the Kettle. It sighs before it cuts out; it has sighed."

## 10e. Trace and notebook lines (authored, 40 + 60 candidates; excerpt)

**Trace lines (by depth reached):**

- Depth 2 gained, quiet node: "Traced {node}. Everything I can see agrees with the wall diagram."
- Depth 2 gained, fault: "Traced {node}. The diagram was right. I would rather it had been wrong."
- Depth 3 gained by repair: "Repaired and known. I could find this node blind now."
- Depth 3 gained by walks: "Five rounds. It has stopped surprising me, which is the whole point."
- Refused, not adjacent: "You cannot trace what you have not walked toward. Start at the last mark you made."
- Refused, no survivor free: "No one free to walk it today."

**Notebook lines (by node state, excerpt):**

| State | Depth | Line |
|---|---|---|
| Healthy | 1 | "{node}: nothing to report." |
| Healthy | 2 | "{node}: quiet. Chalk mark unchanged." |
| Healthy | 3 | "{node}: oil at {oil_days} days. I would not touch it. I would like to." |
| Loaded > 85% | 2 | "{node}: warm to the hand. I did not like it." |
| Loaded > 85% | 3 | "{node}: {load_pct}%. Another two on this bus and it goes." |
| Breaker open | any | "{node}: breaker open. Someone has done this on purpose, or forgotten." |
| Fuse blown | any | "{node}: fuse gone. Replaced from the drawer. The drawer is lighter." |
| Tell heard | any | "{node}: {tell_text}. I have written it down." |
| No round | any | "— (no round)" |
| First visit | 1 | "First round. I walked slowly. It is larger than the map." |

**Rule N-1:** notebook lines are rendered from data with `{token}` fills from the probe; a missing token renders "—", never a raw placeholder.

## 10f. The Board and interaction spec (no new routed panel)

**Surface:** a "Works" tab in `ShelterOperationsPanel` (verified §2b; DEC-WB-08 allows only an existing surface).

**Layout (text mock):**

```
WORKS BELOW                                   Day 74
Supply 967 W (gen 800 + battery 167)   Draw 2,910 W   Cut line: after room 5

LOAD LADDER
 1 Filtration      120 W  crit   ON      cumulative  120
 2 Water pump      180 W  crit   ON                  300
 ...
 6 Galley          210 W  std    ---   <-- cut line
 ...
NODES (12)   loading   depth   state
 Bus main vault   61%    3       ok
 Clinic xfmr      94%    2       overload
 ...
WATER  tanks 12%  filter 22%  well low   AIR  HEPA 34%  sat 0.7   HEAT  boiler fuel 41%
CAUSE CHAIN  [symptom picker v]
NOTEBOOK (last 30)
```

**Controls:** Trace, Walk, symptom picker, node select. Every control is keyboard- and controller-reachable with visible focus; Back closes the tab. The panel exposes existing commands (breaker, fuse, shed preset) as links to the existing Power panel rather than duplicating them.

**Accessibility:** cut line drawn as a labelled bar, not colour alone; dashed edges also carry the text "as drawn"; minimum control height per the a11y floor token already in Core.

## 10g. Rounds, worked over a week

Assignee: Marisol. Loop position starts at the bus.

| Day | Node walked | Outcome | Notebook | Knowledge |
|---|---|---|---|---|
| 60 | Bus main vault | healthy | "Bus quiet." | walks 1 |
| 61 | Clinic transformer | loaded 94% | "warm to the hand" | walks 1, depth 2 (already traced) |
| 62 | Filtration stepdown | healthy | "quiet" | walks 1 |
| 63 | (Marisol sick) | no round | "— (no round)" | none |
| 64 | Water pump junction | tell heard: relay click | "clicked twice. Written down." | walks 1 |
| 65 | Galley | fuse blown | "fuse gone" | repair credit if she replaces it: depth 3 |
| 66 | Living quarters | healthy | "quiet" | walks 1 |

The loop resumes where it left off; a missed day does not skip a node. Depth 3 at the galley arrives on day 65 through the repair, not through walks. **A repair credited by anyone counts, not only the assignee.**

## 10h. Edge cases and rules

- **R-1** Two Traces on one day for the same survivor are refused; different survivors may each Trace a different node.
- **R-2** If a node is destroyed or removed by another owner, its knowledge record stays but the Board marks it "gone"; never crashes, never deletes history.
- **R-3** If the shelter has fewer than 12 nodes (variant start, see Other Beginnings), the Board lists only those present; `rounds_order` skips absent nodes.
- **R-4** Difficulty presets never scale Trace cost; it is always one survivor-day.
- **R-5** A survivor sick, injured or on expedition cannot be assigned; refusal text is explicit.
- **R-6** The probe may return "unknown"; the chain prints "cause unknown" and stops rather than guess.
- **R-7** Determinism: the Works layer has no randomness except Night Sounds and coined nicknames, both through named `CampaignStreamIds` forks.
- **R-8** Save size bound: at most 12 node records plus 7 machine records plus 12 kept notebook lines.

## 10i. Machine character catalog — full authored rows

### 10i.1 Coined name pools (3–5 each; tone: plainspoken, affectionate)

| Machine | Coined names |
|---|---|
| Generator (Drum) | The Drum, Old Steady, The Heart, Big Thumb |
| Ventilation plant (Bellows) | The Bellows, Windy, The Cough, Auntie Draft |
| Foundry cupola (Cupola) | The Cupola, The Chimney, Slow Fire |
| Boiler (Kettle) | The Kettle, Old Sigh, The Stove, Mother Warm |
| Airlock machinery (Gate) | The Gate, Two-Click, The Latch, The Doorman |

The Lung and the Still keep their authored names (`authored`). A player rename overrides any of these.

### 10i.2 Clue lines (3 per machine; physical details only)

**The Lung:** (1) "The frame is stamped with a shift number, painted over twice." (2) "The last filter frame was cut down with a hacksaw to fit." (3) "Someone taped a ration card to the housing. It has never been used."

**The Drum:** (1) "A second nameplate is riveted over the first." (2) "One bearing housing is newer than the rest and bolted with the wrong thread." (3) "A hand-lettered card by the fuel line: NOT BEFORE SIX."

**The Still:** (1) "Tally marks are scratched inside the access hatch." (2) "The outflow spout has been replaced with a length of bicycle tube." (3) "A cup hangs on a hook by the outflow. Nobody admits to it."

**The Bellows:** (1) "One damper handle is worn bright on one side only." (2) "Duct tape, then wire, then tape again, in the same seam." (3) "A pencil mark on the housing: the wind was from the east."

**The Cupola:** (1) "A fire-brick has a child's thumbprint fired into it." (2) "The tap-hole has been re-lined with clay from somewhere that is not here." (3) "There is a bench by the cooling wall. The paint on it is not fire-paint."

**The Kettle:** (1) "A chalk tally of cold nights runs up the flue." (2) "The safety valve has a hand-cut washer." (3) "A mug ring on the top plate, older than anyone here."

**The Gate:** (1) "The manual override wheel is newer than the door." (2) "Two sets of scratches on the frame at different heights." (3) "A key hangs on a nail, cut for a lock this door does not have."

**Rule C-1:** no clue states who, when or why. Each is a *thing in the room*.

### 10i.3 Mood lines (2+ per mood per machine; excerpt for the Drum)

- Content: "The Drum runs level. The lights do not remember what dim was."
- Grumbling: "The Drum hunts a little at load. Someone has started tapping the gauge."
- Complaining: "The Drum surges and drops. Everyone in the room has one eye on the ceiling."
- Silent: "The Drum is off. The room is the quietest it has ever been, and nobody likes it."
- Settled: "The Drum's hum is the floor now."

(Mood lines describe sound and people. **C-2:** no mood line uses a mind verb for a machine; M-8 enforces.)

### 10i.4 Night-sound lines (excerpt; keyed to quirks)

| Quirk (working id) | Named line | Anonymous line |
|---|---|---|
| relay click | "{name} clicked twice in the north corridor. {keeper} says it does that." | "Two small clicks, somewhere along the north wall." |
| generator hum drop | "{name}'s hum dropped for three seconds. {keeper} said that is fine." | "The floor's hum dropped, then came back." |
| still gurgle | "{name} gurgled after midnight. {keeper} was already awake." | "A wet, patient noise from below the pump room." |
| boiler sigh | "{name} sighed before it cut out. {keeper} had put the kettle on." | "A long breath of air from the far wall. Then quiet." |
| damper knock | "{name} knocked twice. {keeper} counted." | "Something knocked twice inside the wall." |
| hepa whistle | "{name} whistled on the exhale. {keeper} slept through it." | "A thin whistle, high, on and off." |

**Rule S-1:** `{keeper}` renders as "someone" when there is no keeper.

## 10j. A hundred and twenty days in the walls (one worked timeline)

Fixture: a Lung and a Still, two survivors (Marisol on filtration, Ines on the pump room), default difficulty, no disasters.

| Day | Event | Stored change | Player sees |
|---|---|---|---|
| 1 | new game | nicknames authored; KeptDays 0 | "The Lung" and "The Still" on the machine strip |
| 5 | Marisol assigned to filtration | Keeper set, KeptDays 1 | strip shows keeper name |
| 12 | HEPA band drops to Worn | HealthyStreakDays 0 | Lung: Grumbling |
| 15 | filter serviced | Healthy again, streak 1 | Lung: Content |
| 25 | KeptDays 21 (Marisol) | kept; clue depth 1 | clue line: painted-over shift number |
| 25 | Ines at the Still since day 4 | kept on day 24; clue 1 | chronicle: coined line not used (authored nickname) |
| 30 | first HEPA replacement | Lung arc stage 1 | chronicle: "stopped whistling" |
| 46 | Lung Healthy 31 days after Critical spell | arc stage 2 | "breathes in fives" |
| 58 | Still filter replaced after < 30% | Still arc stage 1; clue depth 2 | "coughed and ran clear" |
| 60 | Night Sound: relay click; pump junction depth 2 | LastNightSoundDay 60 | named line in briefing |
| 64 | Night Sound: hepa whistle; filtration depth 1 | LastNightSoundDay 64 | anonymous line |
| 115 | Lung Healthy 100 days | Settled; arc stage 3; clue depth 3 | chair line; mood Settled |
| 120 | Marisol dies | KeptDays reset; grief line queued once | "her chair is still by the frame" |

Note day 64: the same tell at the same machine reads as a mystery on day 64 and as a habit on day 60 purely because the node is traced. That difference is the point of the seam in section 1.3.

## 10k. How the two parts feel together (design intent, restated)

A new player sees only the load ladder and the machine strip. A player at day 60 has walked half the shelter and can read "why". A player at day 120 has a household that talks about machines the way households talk about stoves. None of it costs a resource. All of it is attention, recorded.

## 10l. What this plan does not decide (for the next pass)

- The exact panel that hosts the Works tab (P0).
- Whether Rounds is a duty post or a Trace side effect (DEC-WB-05).
- Which aux sources exist as graph nodes (depends on which owners are present).
- Whether Settled should ever have a *tiny* mechanical echo (rejected in v1, DEC-MW-03).
- Machine death (backlog).

---

# PART THREE — CATALOGS, HOOKS, ACCEPTANCE, DELIVERY

## 11. Catalog specs and validator rules

| File | Rows | Key fields |
|---|---|---|
| `works_topology.json` | ~46 nodes, ~60 edges, `rounds_order` (12) | `id`, `kind`, `label`, `source_ref`, `machine_id?`; edges `{from,to,kind,verified,proof?}` |
| `works_symptoms.json` | 24 | `id`, `subject_id`, `chain_kind`, `text`, `suggest_line_id` |
| `works_trace_lines.json` | 40 | `id`, `depth`, `state`, `text` |
| `works_lines.json` | 60 | notebook lines keyed by node state and depth |
| `machine_characters.json` | 7 machines | `machine_id`, `coined_names[3..5]`, `clue_lines[1..3]`, `mood_lines{content,grumbling,complaining,silent,settled}` |
| `machine_arcs.json` | 3 arcs x 3 stages | `arc_id`, `machine_id`, `stage`, `predicate_id`, `line` |
| `machine_night_lines.json` | 28 | quirk-keyed named lines + 6 anonymous lines |
| `machine_character_lines.json` | 30 | keeper-grief, nickname-coined, rename, settled chronicle lines |

**Validator rules (added to the integrity pipeline; row-level failure output):**

- **W-1…W-7** (§5.3).
- **W-8** every symptom `subject_id` resolves to a room, node or machine; every `chain_kind` is one of `chain_power/water/air/heat`.
- **W-9** `predicate_id` values resolve in a closed C# predicate table (no string eval).
- **W-10** every `works_lines` row has a valid `state` and `depth` in 1..3.
- **M-1** each machine has 3–5 coined names, each 3–24 chars, unique across the catalog.
- **M-2** every machine has exactly 3 clue lines; each is ≤ 160 chars.
- **M-3** every arc has stages numbered 1..n without gaps; predicates resolve.
- **M-4** every night line references an existing quirk id; anonymous lines carry no machine name.
- **M-5** every mood in §8.2 has at least 2 lines per machine.
- **M-6** all text rows ≤ 200 chars, no placeholder tokens.
- **M-7** no real place, faction or person names (tone rule).
- **M-8** deny-list check (§10.4): machine-as-subject-of-mind verbs rejected.

---

## 12. Cross-plan hooks (ship dark; `Null*` defaults)

| Hook | Direction | Default | Purpose |
|---|---|---|---|
| `IWorksProbe` | Works reads owners | `NullWorksProbe` | keeps Core engine-free and testable |
| `IWorksRepairSink.OnRepairCredited(nodeId)` | host to Knowledge | no-op | depth-3 credit from owner repairs (E16) |
| `IMachineGriefSink` | keeper death to chronicle | `NullMachineGriefSink` | hook to `RelationsGriefSink` |
| `IRoundsAssignee` | duty roster to Rounds | `NullRoundsAssignee` | no assignee means no walk |
| Deep Works sump hook | `DW` reads `wtr_sump` label | none | DW may cite the sump node; no shared state |
| Long Siege hook | Siege Year readiness may read Works Board power/water rows | none | read-only, no new state |
| Record Keepers / chronicle | append-only lines | existing writer | idempotent keys |

No hook changes another plan's authority. Every hook is optional; the plan is complete with all defaults.

---

## 13. Acceptance criteria

**WB**

- **WB-A1** Board load ladder matches the owner: for every fixture, listed rooms, cumulative draw and cut line equal `PowerGridSystem` values (parity test).
- **WB-A2** For each of the 24 symptoms, the Cause Chain names the first failing step for a scripted fault and marks later steps "not reached".
- **WB-A3** A step at an untraced node returns "not yet traced" and stops.
- **WB-A4** Trace raises depth 1 to 2 only through adjacency; a non-adjacent Trace is refused with a reason.
- **WB-A5** Depth 3 needs a credited repair or five walks; forecasts appear only at depth 3.
- **WB-A6** No utility verb is gated by depth (K-5).
- **WB-A7** Round-trip save; legacy save loads depth 1 everywhere with unchanged utility behaviour.
- **WB-A8** No owner file gained a method; the diff to `Shelter/*System.cs` is empty except the one nested DTO.

**MW**

- **MW-A1** Mood derivation table (§8.2) holds for all bands and streaks, with parametrised tests.
- **MW-A2** Kept flag and KeptDays follow §8.3 including keeper change and vacancy.
- **MW-A3** Coined nickname is deterministic per machine and seed and independent of coin order; player rename is never overwritten.
- **MW-A4** Night Sound: at most one per night, no repeat within 3 nights, named only at depth 2 or more, silence allowed.
- **MW-A5** Arc stages fire once, on facts only, idempotent under reload.
- **MW-A6** Deny-list validator rejects a crafted bad line.
- **MW-A7** Round-trip save; legacy save loads with no keeper or streak; existing tell, glitch and audio-sync tests pass unchanged.

---

## 14. Packages

| Pkg | Scope | Depends |
|---|---|---|
| P0 | Premise audit: E12, E14, E15, E16; choose save home; record findings | none |
| P1 | `works_topology.json` + topology loader + validators W-1..W-7 | P0 |
| P2 | `WorksGraph` + `IWorksProbe` + host probe adapter (`INT`) | P1 |
| P3 | `WorksCauseChain` + symptoms + trace lines + validators W-8..W-10 | P2 |
| P4 | `WorksKnowledge` + Trace/Walk + save DTO + repair credit (`INT`) | P3 |
| P5 | Works Board projection + panel surface + Rounds | P4 |
| P6 | `MachineMood` + `MachineCharacterSystem` + keeper/streak day-owner (`INT`) | P0 |
| P7 | Nicknames, rename, clue ladder, catalogs, validators M-1..M-8 | P6 |
| P8 | `NightSounds` + briefing line (`INT`) + arcs | P4, P7 |
| P9 | Save parity, legacy load, prose review pass, handoff | all |

WB (P1..P5) and MW (P6..P7) may proceed in parallel after P0; P8 is the join.

---

## 15. Decision register (all unsigned; foreman/user signature required)

- **DEC-WB-01** Topology is presentation-only; no arithmetic change. *Recommend yes.*
- **DEC-WB-02** Save home for Works and Character state: shelter-identity save vs shelter-maintenance save. *Decide at P0.*
- **DEC-WB-03** Fog gates explanation only, never action (K-5). *Recommend yes.*
- **DEC-WB-04** Adjacency rule for Trace. *Recommend yes.*
- **DEC-WB-05** Rounds as a duty role vs a Trace side effect. *Resolved at pass 1: a duty **role** (one additive `duty_roles.json` row); the loop is the plan's.*
- **DEC-WB-06** Unverified edges drawn dashed. *Recommend yes.*
- **DEC-WB-07** Forecasts limited to depth 3.
- **DEC-WB-08** No new routed panel; extend existing surface.
- **DEC-MW-01** Tone rule: household vocabulary, never machine will. *Binding via Exp. 40.*
- **DEC-MW-02** Character state nested beside Works state, both inside the `shelter_maintenance` section (§2b).
- **DEC-MW-03** Settled has no mechanical effect. *Recommend yes.*
- **DEC-MW-04** Keeper threshold 20 days, Settled threshold 100 days. *Tunable data.*
- **DEC-MW-05** Player rename permitted, 24-char cap.
- **DEC-MW-06** No new audio in v1.
- **DEC-MW-07** Machine death out of scope for v1.
- **DEC-MW-08** No new routed panel; machine strip in `ShelterPanel`.
- **DEC-WB-09** Depth-3 credit subscribes to `OnTransformerMaintained` / `OnMaintenanceCompleted` where they exist and wraps `ReplaceFuse`, `PerformGeneratorMaintenance` and both `ReplaceFilter` verbs in the host where they do not.
- **DEC-WB-10** Auxiliary sources limited to solar until a wind or hand-crank owner exists.

---

## 16. Test plan (focused; `bin/run-scoped-tests`)

- Graph: acyclicity, edge verification rule W-3, machine coverage W-5 (parametrised over the catalog).
- Cause Chain: one parametrised test over the 24 symptoms with a fake probe; fog stop test.
- Parity: Board versus `PowerGridSystem` on three fixtures (default brownout, full fuel, tripped room).
- Knowledge: monotone depth, adjacency refusal, depth-3 paths, idempotent credit.
- Mood table: parametrised bands and streaks.
- Character: keeper transitions, coin determinism across order permutations, rename sanitising.
- Night Sounds: seeded selection, no-repeat window, named versus anonymous.
- Arcs: idempotence under save/load.
- Save: round-trip and legacy load for both DTOs.
- Parity guard: existing tell, glitch and audio-sync tests, run unchanged.

Target 30 to 40 focused tests. No duplicate wording variants; one test per behaviour.

---

## 17. Risk register

| Risk | Likelihood | Mitigation |
|---|---|---|
| Diagram drifts from owner behaviour | Med | Parity tests; owner wins; dashed edges for unverified |
| Players read dashed edges as bugs | Low | Caption "as drawn on the wall"; help line |
| Fog feels like a gate | Med | K-5; explanation only; playtest note |
| Machines read as haunted | Med | M-8 deny-list; tone review in P9 |
| Save bloat | Low | Bounded dictionaries (12 nodes, 7 machines) |
| Depth-3 credit unobservable | Med | Host wrapper fallback (E16) |
| Two "learned" flags appear | Low | One `WorksKnowledgeState`; Night Sounds consult it |
| Confusion with The Deep Works | Med | Naming note; distinct prefixes |

---

## 18. Expansion backlog

- Stale Knowledge after a rebuild or flood (depth decay).
- Machine death and replacement arcs.
- Second-tier arcs for the other four machines.
- Player-drawn chalk marks on the Board.
- Rounds routes chosen by the player.
- Seasonal quirks (winter Kettle, dust-season Bellows).
- Visitor reactions to named machines.
- A "how it was built" epilogue keyed to clue depth 3 across all seven.
- Verified water and air edges as owners expose routing.

---

## 19. Open Mysteries and Deliberate Silence

- Who wired the shelter is never stated.
- The Ground Loop, Again: *again* is never explained.
- The phantom draft stays harmless and unfound.
- The second nameplate on the Drum, the newer wheel on the Gate, the thumbprint in the Cupola brick: physical facts, no owners.
- What the chair beside a Settled machine means is left to the player.
- Whether any machine is "kept" by the household or keeps the household is never asked.

---

## 20. Pre-flight, verification and stop conditions

**Pre-flight (before any edit):** read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md`; confirm claims for every path in section 4; complete P0 and record findings.

**Verification:** run only focused targets via `bin/run-scoped-tests` for the changed files; run `bin/ashfall-dev validate-config` for the new catalogs; a Godot headless check only if the Board or briefing wiring is touched (15 FPS); never the full suite without `RUN FULL TESTS`.

**Stop and report to the foreman if:**

- a path in section 4 is claimed by another owner;
- P0 shows a needed owner method does not exist and cannot be read another way (do not add owner APIs to work around it);
- Rounds cannot be expressed through the existing duty/assignment owner;
- any change would alter electrical, water, air or thermal arithmetic;
- any line would give a machine intent, speech or memory.

**Handoff:** outcome, files, contract, commands and results, limitations, shared paths intentionally untouched, per `AI_AGENT_WORKFLOW.md`. On integration, mark this file FULLY INTEGRATED at the top (multiple times) and move it to `.ai/plans/integrated/<category>/`.
