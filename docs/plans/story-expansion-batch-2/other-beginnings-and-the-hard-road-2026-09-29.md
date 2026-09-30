# Feature / Task Plan: Land, Ruins and Starts II — Other Beginnings (start as garrison outpost, relief station, caravan or hospital) & The Hard Road (challenge modes)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Open points are in §24 (Expansion backlog) and §25 (Open Mysteries and Deliberate Silence).

> **Subjects covered (2 of the 16 in this batch):**
> 31. **Other Beginnings** — start the campaign as a garrison outpost, a relief station, a caravan or a hospital. (Prefix `OB`.)
> 32. **The Hard Road** — challenge modes. (Prefix `HR`.)
>
> **Companions that already exist and are extended, not replaced:** `shelter_origins.json` (6 origins, `ShelterIdentitySystem.SelectOrigin`), `starting_supplies.json` (6 supply profiles, `StartingSuppliesCatalog`), `starting_survivor_cohorts.json` (6 cohorts, `StartingCohortCatalog`), `difficulty_presets.json` (4 presets × 8 scalars) with `DifficultySettingsSystem` (custom lanes, `LockSettings`), `CampaignMode.IronMan` with `IronManTerminalState` and `SaveSlotService.MarkTerminal`, `hardcore_economy_tuning.json`, `endings.json` (8 endings) with `HoldfastEndings`, and the campaign chronicle. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Naming notes.** *Other Beginnings* is the **first hour of a campaign** — what the player's people, stores and walls are on day 0 and what the place expects of them. It is **not** a new location (the shelter stays the shelter), **not** the buildable `outposts.json` (four *build* projects such as North Watch — a different concept, DEC-OB-06), **not** the merchant caravans that visit (`caravans.json`, `merchant_caravans.json`), and **not** the Regional Hospital *ruin* of `second-nature-and-ruins-of-the-before-2026-09-29.md` (a place you walk into, later). *The Hard Road* is **not** a difficulty rewrite: the four presets, eight scalars, lock and Iron Man all stay exactly as they are. It adds *promises* on top of them.
>
> **Sister plans (read-only cross-references; all hooks ship dark):** `year-two-the-long-thaw-2026-09-29.md` and `y2-p7-year-two-chronicle-2026-09-29.md` (chronicle reads Marks), `living-region-2026-09-29.md` (refugees at a relief station), `convoy-wars-and-inside-a-house-2026-09-29.md` (the caravan start's road), `shelter-governance-2026-09-29.md` (who the people answer to), `evenings-and-memory-work-2026-09-29.md` (a Start's first evening).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, data, ledger or other plan. Paths are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **9b**, **15b** and **25** carry story texture. Sample lines are content candidates for JSON rows, never code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — Where You Begin, and What You Promised

> *"Nobody arrives at the shelter as a blank. They arrive from somewhere — a post they were
> told to hold, a line they were told to keep open, a road they could not keep driving, a ward
> they could not leave. The door closes and the somewhere comes in with them."*

Every campaign of ASHFALL begins in the same hour and, until now, almost the same way. Three people, a shelf of stores, a set of walls. The game already lets those three things vary independently — six cohorts, six supply profiles, six shelter origins, four difficulty presets — and it already contains, in its data, the *pieces* of stories: a hardened military outpost, a field clinic's stock, a convoy's remnant, an evacuated checkpoint's batteries. What it lacks is **a beginning that knows what it is**. The pieces are never assembled under a name, and the player never chooses *"we are the people who held a garrison post"* — they pick from three separate lists, or, more often, from none.

**Other Beginnings** assembles four of those pieces under four names — the **Garrison Post**, the **Relief Station**, the **Halted Caravan**, the **Ward Below** — and gives each a small, honest **Charge**: three duties the place expects of its people for the first sixty days. Nothing in a Start is a new rule. Every number in it is already in a shipped catalog; the Start only *chooses together* and *says so*.

**The Hard Road** is the other half of the same idea seen from the far end. A player who has finished a campaign, or three, may want to make a promise: *no one comes in after us; nobody dies; we do not trade; we do not go out at night.* The game already has the blunt instruments — a difficulty that scales hunger, a lock that freezes it, an Iron Man seal that refuses a reload — but it has no way to *record what was promised* and no way to say afterwards whether it was kept. **The Hard Road** adds **Vows**: authored promises with a witness in the world's own ledgers; **Roads**: five named bundles of difficulty, seal, start, vows and a goal; and **Marks**: one line in the chronicle, plainly written, when a road ends.

The two subjects are paired because they are the same act at two ends of a campaign. **A Start is a promise the place makes to its people; a Vow is a promise the people make to themselves.** Neither is a reward system, a score or a menu of perks. Both are *shapes* — of the opening, of the run — that the player chooses knowingly, and that the game then **remembers truthfully and does not judge**.

There is a moment every player has met and the game has never marked: the second the door closes and the three people behind it look at one another and realise nobody has said what they are *for*. A garrison knows. A ward knows. A relief station, a column stopped at the side of a road — each of them arrived with a job, and the job did not stop being theirs because the world did. **Other Beginnings** is the game noticing that, and letting the first sixty days be *about* it: a chalk board with three lines on it, and the quiet question of whether the lines are still true when the food is low.

And there is the other moment, later, that the game has never marked either: a cold morning around day 40 when someone, over the register, says *"we said we would not let anyone in,"* and someone else says *"I know what we said."* **The Hard Road** is the game keeping that sentence. Not to punish it; to remember it, the way people remember what they swore — plainly, with the day.

**The binding tone rule.** Cold, exhausted, human, restrained; specificity over adjectives; no supernatural; no real countries, wars or people (corpus tone lock). One rule is specific to this pair: **the game never sneers at a broken vow and never congratulates a kept one.** A vow broken on day 41 is a fact in a ledger (*"The vow was broken on day 41. The road went on."*), not a failure screen. A vow kept is another fact. The people supply the character; the game keeps the notebook.

**Tone & register.**

- **Other Beginnings is written in the voice of the duty roster.** Who stands where, what is in the locker, what the sergeant, nurse or driver said last. Lists, initials, a name and a number.
- **The Hard Road is written in the voice of the vow sworn on a cold morning** — short, plain, slightly formal, meant to be read back later by someone who is not sure they meant it.

**What the two share.** *Consent and memory.* The player chooses both knowingly, from a plain comparison of what changes; and the game keeps, in the chronicle, exactly what was chosen and what came of it.

---

## 1. Goal & Outcome

### 1.1 Other Beginnings (OB)

> *Design intent: on day 1 the player should be able to say "we are the Ward Below" and mean
> something specific — who is in the room, what is on the shelf, what the place expects — and
> on day 60 be able to say whether they kept it.*

- **Goal:** Add a **Start** — an authored bundle in `starts.json` that names one existing shelter origin, one existing supply profile and one existing survivor cohort under a place frame (Garrison Post, Relief Station, Halted Caravan, Ward Below) — together with a one-time **Opening Choice** (two options; items and morale through existing owners) and a **Charge** of three **Duties**, each a *derived* word (Kept / Strained / Lapsed) over ledgers that already exist. A small stored **`StartState`** records only what the player *chose* and what *first lapsed*.
- **Outcome (observable):**
  1. The new-game picker (an existing host surface; chosen at P0) offers the four Starts plus **Standard Holdfast** (unchanged). Each shows a **StartDelta**: a plain, derived comparison against Standard (*"Bandage +6, canned food −6, rad-away +2 …"*, origin shielding, ventilation and space), never a marketing sentence.
  2. Choosing a Start passes its three ids into the *existing* new-game seam (`StartNewGame(cohortId, suppliesId, difficultyId)`) and selects its origin through the *existing* `SelectShelterOrigin` path — instead of today's fixed default of `origin_government_bunker`.
  3. During days 0–3 the player may make the **Opening Choice** once. It applies item deltas through the inventory owner and a morale delta through the survivor owner, and writes one notebook line.
  4. For sixty days the shelter surface shows the **Charge**: three duties, each with a word and a plain reason (*"Batteries: 3 in stock, the post needs 4."*). The Charge **fades** on day 60 into history, or is **Set Down** earlier by the player.
  5. When the Charge fades or is set down, one line enters the chronicle stating what was held and what lapsed and when.
  6. Save/load round-trips; a legacy save loads with no Start recorded and every new-game, origin, supply, cohort and identity behaviour equals today's.
- **Non-Goals (OB):** **no new origin, supply profile, cohort, location or item**; no change to any existing catalog row; no new needs, health, power or inventory authority; no fixed difficulty attached to a Start (difficulty is its own axis); no punishment or reward *mechanics* for duties in this plan (words and history only — DEC-OB-05); no children-centred start (the corpus tone lock and §15b); no new routed panel; no Unity.
- **"Done" (OB):** §19 OB acceptance passes via `bin/run-scoped-tests`; existing shelter-identity, starting-supplies, starting-cohort and new-game tests unchanged; handoff lists untouched shared paths.

### 1.2 The Hard Road (HR)

> *Design intent: a player should be able to sit down and choose, in a minute, "no one comes in
> after us, and nobody dies" — and, at day 150, look at one line in the chronicle and know
> whether they did.*

- **Goal:** Add **Vows** (ten authored promises, each with a *witness* — a fact an existing owner already holds), **Roads** (five authored bundles: difficulty preset, lock, Iron Man flag, optional pinned Start, vows, goal), and a small stored **`HardRoadState`** that records the chosen road, the day each vow **first broke**, and the road's **result**. A **Mark** (one journal line) is written once when the road ends.
- **Outcome (observable):**
  1. The new-game picker offers **Roads** beside Starts. Choosing a Road preselects difficulty, Iron Man and (if pinned) the Start, shows its vows in plain words, and states what will be **locked** for the run.
  2. Each vow's status is a derived word — **Kept so far** or **Broken on day N** — computed from its witness. Only the *first* break day is stored (a historical fact).
  3. A vow whose witness needs a counter that does not exist today is marked **NEEDS-COUNTER** by the validator and **not offered** until an owner supplies the counter (no vow is ever "honour system").
  4. A road ends at its goal (a day, or an ending id from `endings.json`), on an Iron Man **terminal loss**, or when abandoned. One **Mark** line enters the chronicle: what road, which vows kept, which broken and when, how it ended.
  5. Nothing about a road is a reward: no currency, no unlock, no meta-progression (DEC-HR-05). Difficulty scalars and locks behave exactly as today.
  6. Save/load round-trips; a legacy save loads with no road and every difficulty, lock and Iron Man behaviour equals today's.
- **Non-Goals (HR):** no change to any preset, scalar, lock or Iron Man rule; no new fail state, no new game-over path; no leaderboard, score or achievement system; no cross-campaign profile book (DEC-HR-06, backlog); no enforcement that *prevents* a broken vow (vows are witnessed, not policed — DEC-HR-03); no new routed panel; no Unity.
- **"Done" (HR):** §19 HR acceptance passes; existing difficulty, lock, Iron Man and save-slot tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**A Road may pin a Start; a Start never requires a Road.** Two seams and no more: (1) `road.start_id` (optional) names a Start row; (2) a Start's Charge and a Road's vows are **independent** ledgers that both write to the chronicle through one shared line-writer, so a campaign that has both produces two lines, not a merged record. Neither part keeps a copy of the other's facts (Rule 5).

---

## 1b. Texture, Mystery & Voice

**A beginning is a list.**

The Start picker never says *"An exciting new way to play!"* It says what is different, in a column, and lets the player read it: *"Ward Below. Nine bandages, three rad-away, eight clean water, ten cans. The walls are deep. Nobody left the ward."* The **StartDelta** is a table; the prose above it is two lines.

**The Opening Choice is a small thing.**

It is not a branching cutscene. It is the first decision at the door — one sentence of situation and two plain options. *"The ordnance cache is behind the cage. The sergeant has the key and has not offered it."* — **Break it open** / **Leave it locked**. Its effects are small and real: three batteries, four morale, one line in the notebook.

**A Charge is a roster.**

The Charge does not say *"Objective."* It says *"The post"* and lists three lines the way a duty board would: *"Hands: 3 of 3. Lights: 3 batteries, the post needs 4. Health: everyone above 50."* Each has a word beside it in the margin, in small type. *Kept. Strained. Lapsed.* Nobody is scolded.

**When a Charge fades.**

Sixty days later it does not end with a fanfare. The board is taken down. One line in the chronicle: *"The post was held. The lights lapsed on day 23 and were holding again at the end."* (The lapse day is stored; the *holding again* is derived from the duty's word at fade time.)

**A vow is a sentence.**

*"We will not take anyone in after today."* *"We will bring everyone through."* *"We will not go out after dark."* Ten of these, no more. Each says what it means in one line and what the game will *watch* in another: *"Watches: anyone who joins after day 0."*

**A road is a name for a bundle.**

*The Long Ration. One More Dawn. The Closed Door.* They are named the way people name a hard winter afterwards — after the thing they remember. A road's card lists, in order: **Difficulty. Sealed or not. Where you begin. What you have promised. Where it ends.**

**A Mark is one line.**

*"Walked the Closed Door to day 150. Everyone came through. The door opened on day 88."* It sits in the chronicle between a harvest and a burial. It is not bigger than either.

**What the player is never told.**

- **Why the garrison was left.** The post has a schedule and a locked cage. The game does not say who ordered the schedule or who stopped keeping it.
- **Who ran the relief station.** The register is there. The last entry is not.
- **Where the caravan was going.** The wagons are pointed one way. The road is not on any map.
- **What the ward's last patient had.** The bed is made.
- **Whether a broken vow mattered.** The chronicle states the day. It does not state the cost.

**Voice — sample fragments (content candidates for `start_lines.json`, `mark_lines.json`).**

> "Ward Below. Nine bandages, three rad-away, eight clean water, ten cans. The walls are deep. Nobody left the ward." — Start card (OB)

> "The ordnance cache is behind the cage. The sergeant has the key and has not offered it." — Opening Choice (OB)

> "Hands: 3 of 3. Lights: 3 batteries, the post needs 4. Health: everyone above 50." — Charge board (OB)

> "The post was held. The lights lapsed on day 23 and were holding again at the end." — Charge fades (OB)

> "We will not take anyone in after today." — Vow (HR)

> "The vow was broken on day 41. The road went on." — Vow status (HR)

> "Walked the Closed Door to day 150. Everyone came through. The door opened on day 88." — Mark (HR)

**Design texture beats.**

- **A Start is a comparison, not a pitch (OB).**
- **The Opening Choice is small and once (OB).**
- **A Charge is watched, not enforced (OB).**
- **A broken vow is a date (HR).**
- **A Mark is one line, never a screen (HR).**
- **Neither part gives anything back (OB × HR).** No perks, no currency, no unlock.

---

exactly what was chosen and what came of it.

**The second layer.** A Start is a promise the place makes to its people; a Vow is a promise the
people make to themselves — and the game's whole job in both is the notebook. It never sneers at a
broken vow and never congratulates a kept one, which is the only kind of remembering a person
could bear to live inside. What the two subjects share is *consent*: both are chosen knowingly,
from a plain comparison of what changes, and then remembered truthfully and not judged. Consent is the only difficulty setting this plan
has, and it cannot be lowered after day three.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §9b/
§15b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the beginnings leave lying around.**

> "Charge board, three lines in chalk. The margin word beside each — Kept, Strained, Lapsed — is small type and nobody is scolded."

> "Sergeant's key ring: the cage key is on it. The key has not been offered, and the game will not say why."

> "Register at the relief station: every entry but the last. The last is not missing. It is unwritten."

**What the roads leave lying around.**

> "Vow card: plain sentences, ten of them. Each has a second line — what the game will watch."

> "Day-41 slip: the vow was broken. The road went on. Both facts, one line."

> "Mark, one line in the chronicle between a harvest and a burial. Not bigger than either."

**Scenes the player may piece together.**

> "The door closes and three people look at one another and realise nobody has said what they are for. The board goes up that evening."

> "A cold morning, day 40: 'we said we would not let anyone in.' 'I know what we said.' The register records neither sentence; the vow records both."

**Held silences (texture, not register rows).**

- What the place was before it was a Start. The pieces exist in shipped catalogs; their assembling is a *name*, and names have no history here. Texture only.
- Whether a kept vow feels like anything to the people who kept it. The game keeps the notebook; the feeling is prose's job and must never be a bonus.

**Fourth pass — the notebook that never sneers (texture only; §25 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** The notebook is the game's whole job here: it records and does not
interpret. Every artefact below is written in the grammar of *plain sentences and their margins* —
the charge in chalk, the vow in card, the register ruled for an entry nobody has written yet. The
mystery of this plan is not what the player will promise; it is that the format already knows a
promise can lapse, and is ruled for it.

**What the beginnings leave lying around.**

> "Charge board, fourth margin word: 'Strained,' written smaller than the charge. Nobody writes the
> margin words large."

> "Register at the relief station: the unwritten last entry has been ruled for. The rule is the
> only prediction the format makes."

**What the roads leave lying around.**

> "Vow card, day-41 slip filed behind it. The card is still in the box. The box is where kept and
> broken go to be the same size."

**Held silences (texture, not register rows).**

- Who teaches a new shelter the plain comparison. Both halves are chosen knowingly (§0); the
  teaching is not modelled and the consent must never be assumed. Texture only.
- Whether the game ever reads the notebook back. It records and does not interpret; the reading is
  the player's and must stay theirs.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `shelter_origins.json` holds **6 origins** with `origin_id`, `starting_bonuses`, `starting_drawbacks`, `radiation_shielding_bp`, `ventilation_bonus_bp`, `space_modifier_bp`, `flavor_text`: government bunker (+2500 / +1500 / −1000), mining facility (+1000 / −1500 / +2500), school basement (−1500 / 0 / +1000), private vault (+1500 / +1000 / −2000), improvised cellar (−2000 / +500 / −500), military outpost (+1500 / +500 / +500). | `Data/shelter_origins.json` | LIVE |
| E2 | `starting_supplies.json` (schema 2) holds **6 profiles**, `default_profile_id: origin_standard_holdfast`: standard holdfast (15 lines), waterworks failure (15), field clinic (16), machine room (13), greenhouse remnant (18), evacuated checkpoint (16). Each is a list of `{itemId, amount}`. Field clinic: clean_water 8, canned_food 10, bandage 8, rad_away 3, iodine_pills 6. Evacuated checkpoint: battery 8, scrap_mechanical 6, filter_pack 4, calibration_kit 2. Machine room: battery 14, scrap_mechanical 24, scrap_electronic 18 (no hazmat suit, no rad-away). | `Data/starting_supplies.json` | LIVE |
| E3 | `starting_survivor_cohorts.json` holds **6 cohorts** of 3 (`cohort_standard_holdfast`, `cohort_triage_ward`, `cohort_repair_crew`, `cohort_growers_stewards`, `cohort_convoy_remnant`, `cohort_civilian_improvisers`), each member with `health`, `hunger`, `thirst`, `warmth`, `morale`, `lifetimeDose`, `acuteRad`, `joinedDay`. Convoy remnant: health 80–88, lifetimeDose 24–34, one `acuteRad`. Triage ward: health 84–88, dose 12–18. | `Data/starting_survivor_cohorts.json` | LIVE |
| E4 | **The three axes are independent.** `StartNewGame(cohortProfileId, startingSuppliesProfileId, difficultyPresetId)` takes two of them plus difficulty; **the origin is not a parameter**. After the survivors bind, `SelectDefaultShelterOrigin` picks `origin_government_bunker` (ordinal order, preferred id) if none is set; `SelectShelterOrigin(originId)` exists as a later, separate call. A no-argument `StartNewGame()` uses the standard cohort and standard supplies (or a CLI-supplied supply id). | `src/Main.GameFlow.cs` L142–252; `src/Main.ShelterIdentity.cs` L86–124 | LIVE (finding) |
| E5 | 6 × 6 × 6 = **216 possible openings** exist and none has a name, a description as a whole, or a way to be compared. Three of the six origins, supplies and cohorts already *read* as garrison, clinic and convoy material (military outpost; field clinic stock and triage ward; evacuated checkpoint stock; convoy remnant). Nothing reads as a **relief station**. | E1–E3 | LIVE (finding) |
| E6 | **Namespace collision.** Shelter origins are `origin_*` (`origin_military_outpost`); starting-supply profiles are *also* `origin_*` (`origin_field_clinic`). The same prefix names two different catalogs. A Start that keys by bare id would be ambiguous. | `shelter_origins.json`; `starting_supplies.json` | LIVE (finding) |
| E7 | `difficulty_presets.json` holds **4 presets** — SPARING, STANDARD, AUSTERE, DIRGE — each with **8 scalars** (`hunger_rate_mult`, `thirst_rate_mult`, `radiation_gain_mult`, `disease_onset_mult`, `hostile_encounter_mult`, `market_price_mult`, `equipment_decay_mult`, `crisis_deadline_mult`). DIRGE: 1.75 / 1.75 / 1.6 / 1.5 / 1.75 / 1.3 / 1.5 / 0.65. `default_preset_id: difficulty_standard`. | `Data/difficulty_presets.json` | LIVE |
| E8 | `DifficultySettingsSystem` (state: `ActivePresetId`, `IsLocked`, `IsCustom`, `CustomScalars`, `SchemaVersion = 1`) supports `SelectPreset`, `SetCustomScalar` (both **refused once locked**) and `LockSettings`; the host wrapper `LockDifficultySettings()` documents itself as "ironman style". | `Difficulty/DifficultySettingsSystem.cs` L15–219; `src/Main.DifficultySettings.cs` | LIVE |
| E9 | `CampaignMode { Normal, IronMan }` and `IronManTerminalState { Active, TerminalLoss, Overridden }` live in the save manifest; `SaveSlotService.MarkTerminal(profile, slot, finalDay)` seals a slot against manual restore; `SaveLoadHostSession.MarkActiveSlotTerminal(finalDay)` is the host call. Terminal loss is described as "death, evacuation, or other data-defined end". | `Save/SaveSlotTypes.cs`; `Save/SaveSlotService.cs` L1136–1172; `src/Host/SaveLoadHostSession.cs` L382 | LIVE |
| E10 | `hardcore_economy_tuning.json` (scarcity tiers: Critical 2.5× days 1–15, High 2.0×, Moderate 1.6×, Stable 1.3×; faction preferences; price-shock rules) is loaded by `Main.LoadHardcoreEconomyTuning()` and **bound to the trade panel** when the trade screen opens. It is live for every campaign, not a mode. | `src/Main.Economy.cs` L331–375 | **LIVE — resolved at pass 1: not orphaned, not a Road lever (DEC-HR-09 closed)** |
| E11 | `endings.json` holds **8 endings** (`ending_dawn_of_thaw`, `ending_iron_hegemony`, `ending_exodus_to_sea`, `ending_silent_tombs`, `ending_the_reckoning`, `ending_wasteland_sanctuary`, `ending_frozen_silence`, `ending_warlord_tribute`), consumed by `HoldfastEndings`. Endings are reached through `_endgame.TriggerEnding(ctx)` (`Main.Endgame.cs` L237; also the headless `endgameHost.TriggerEnding(new CampaignEvaluationContext …)`). | `Data/endings.json`; `src/Main.Endgame.cs` | **LIVE — an ending-goal road subscribes to this call (pass 1)** |
| E12 | `outposts.json` holds **4 buildable outposts** (`outpost_north_watch` graph node `node_north_ridge`, 4 bunks, defence 80; `outpost_rail_depot`; `outpost_relay_tower`; `outpost_quarry_camp`) with build costs — a *construction* concept, not a start. `caravans.json` / `merchant_caravans.json` (4 rows) are visiting traders. | `Data/outposts.json`, `caravans.json` | LIVE (finding; do not conflate) |
| E13 | Nothing records **what a run promised**. There is no vow, no road, no run summary, and no journal line for how a difficulty or seal was chosen. | grep across Core | LIVE (finding) |
| E14 | Survivor state carries `joinedDay`, `lifetimeDose`, `health`, `morale` (cohort schema E3), so **roster-based witnesses are derivable today** ("anyone with `joinedDay > 0`", "anyone dead", "anyone above dose N"). Whether a **trade count**, **radio use count**, **expedition-at-night count**, **vehicle use count** or **irradiated-water consumption count** exists is unconfirmed. | E3; `Economy/*`, `Expeditions/*` | roster: LIVE; others **VERIFY (P0)** |
| E15 | Morale is applied per survivor through `_survivors.Needs.ApplyAttributedDelta(id, NeedKind.Morale, delta, source)` (`Main.MoraleContagion.cs` L45) — an *attributed* seam that names its cause. There is no all-survivors call; the Opening Choice loops the living roster and passes the source `start:<choice_id>`. | `src/Main.MoraleContagion.cs` | **LIVE — resolved at pass 1** |
| E16 | There is **no running chronicle append call**. Three real seams exist: (a) `JournalSystem.TryAddRawEntry(knowledgeKey, text, author, day)` — a free-text entry **deduped once per knowledge key**, which is exactly the once-only property Charge and Mark lines need; (b) `CulturalArchiveVaultSystem` chronicle entries (`OnChronicleEntryAdded`); (c) `EpilogueChronicleBuilder` — an *end-of-campaign* epilogue, not a running log. | `Journal/JournalSystem.cs` L281; `Culture/CulturalArchiveVaultSystem.cs` L164; `Endgame/EpilogueChronicleBuilder.cs` | **LIVE — resolved at pass 1: lines go to the journal (DEC-OB-10)**. Note: `TryRecordChronicleEntry` stores only a fixed `summary_key` with no arguments, so it can carry a *fixed* sentence but not a composed line with a day in it. |
| E19 | **The host cannot create an Iron Man slot today.** `SaveSlotService.CreateSlot(…, CampaignMode mode = Normal)` accepts a mode, but `SaveLoadHostSession.CreateSlot(slotId)` builds its manifest with `mode = CampaignMode.Normal` hard-coded (L201) and no UI or host call passes `IronMan`. Sealing (`MarkActiveSlotTerminal`) and terminal blocking are live; *entering* the mode is not wired. | `src/Host/SaveLoadHostSession.cs` L185–206; `Save/SaveSlotService.cs` L90–100 | **LIVE (finding)** |
| E20 | **The dose scales differ.** `SurvivorRadState.RadiationDose` is a current 0–100 acute scale (`AcuteThreshold = 80`); the lifetime figure carried by cohorts (`lifetimeDose`, `LifetimeRadiationExposure`) is judged against `ChronicLifetimeThreshold = 400`. Starting lifetime doses are 8–38, i.e. 2–10% of the chronic line. | `Radiation/RadiationSystem.cs` L99–100 | **LIVE (finding)** |
| E17 | `SaveProfileId` and slot manifests exist; a **profile-level** (cross-campaign) store for Marks is not established. | `Save/*` | LIVE (finding; DEC-HR-06) |
| E18 | Sister plans exist as plans, not source: Year Two, Living Region, Convoy Wars, Shelter Governance, Evenings. | `.ai/plans/` | PLAN |

**Seven findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E4 — the origin is not part of `StartNewGame`.** The one axis that gives a place its physical character (shielding, ventilation, space) is picked *after* survivors bind, and unless the player finds the later call it is the government bunker every time. OB adds an origin id to the new-game seam as a bounded `INT` host change.
2. **E5 — there are 216 unnamed openings.** OB names four of them and compares them to Standard. It does not add a row to any of the three catalogs.
3. **E6 — the `origin_*` collision.** Starts store **three fields** (`origin_id`, `supplies_profile_id`, `cohort_profile_id`), never a single "origin" string, and the validator resolves each against its own catalog.
4. **E9 — the seal already exists.** Iron Man, lock and terminal loss are complete. HR does not touch them; a Road is a bundle that *selects* them.
5. **E13 — nothing remembers the promise.** The hard part of a challenge mode is not the difficulty but the **witness**. HR adds witnesses (roster-based, LIVE today) and refuses to offer a vow whose witness does not exist.
6. **E12 — outposts are builds, not starts.** The Garrison Post start is a *shelter origin story*; it does not place, unlock or consume any `outposts.json` row.
7. **E10 — `hardcore_economy_tuning` is already live.** It is the trade screen's tuning for every campaign, so no Road may claim it as a lever; a road never touches it.
8. **E19 — Iron Man can be sealed but not entered.** The manifest mode is hard-coded to Normal in the host's slot creation. A Road with `iron_man` therefore needs one small `INT` host change (pass the mode through `CreateSlot`) before it can be offered.
9. **E20 — the dose vow's number was wrong.** The lifetime scale's chronic line is 400, so a vow at 60 would break on the first hard expedition. The vow line becomes 200 (half the chronic line), and every ceiling in this plan is restated against the real scale (§2b).

---

## 2b. Evidence pass 1 — what checking the source changed (2026-09-29)

Every `VERIFY (P0)` in §2 was re-read against the live worktree. Results, and the edits they forced:

| Item | Result | Edit made |
|---|---|---|
| E10 hardcore tuning | live for all campaigns (trade panel) | Finding 7 rewritten; **DEC-HR-09 closed** (a road never touches it) |
| E11 endings | reached through `_endgame.TriggerEnding(ctx)` | ending goals subscribe to that call (§15.3) |
| E15 morale | per-survivor, attributed: `Needs.ApplyAttributedDelta(id, Morale, delta, source)` | Opening Choice loops the living roster with source `start:<id>`; §8.1 |
| E16 chronicle | no running chronicle; journal `TryAddRawEntry` dedupes per key | all Charge/Mark lines go to the **journal** (DEC-OB-10); §9.4, §15.4, §17.2 |
| Iron Man entry | host `CreateSlot` hard-codes Normal | new `INT` seam **DEC-HR-13**; two Iron Man roads are gated on it (§14.1) |
| Dose scale | lifetime judged against 400; starting 8–38 | `vow_whole_dose` 60 → **200**; caravan duty ceiling 60 → **120 (margin 30)**; HR-V9 now compares against 400 |
| E14 counters | still unconfirmed for `trades`, `expeditions`, night, vehicle, water, radio | unchanged; vows stay gated (correct) |

**What did not change:** the three-field Start row, the closed predicate and witness tables, derive-don't-store, no rewards. The evidence sharpened numbers and seams; it did not move the design.

**Two consequences worth stating plainly.** First, *The Long Ration* and *One More Dawn* cannot ship until the host can create an Iron Man slot — so at first ship **only two roads are available** (The First Winter, The Closed Door), and the plan says so rather than pretending. Second, the Charge lines and Marks sit in the journal, beside the survivors' own entries — which is where they belong: a promise kept or broken is something *they* would have written down.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Shelter origin (shielding, ventilation, space) | `shelter_origins.json` + `ShelterIdentitySystem` | reads only; selects through `SelectOrigin` |
| Starting supplies | `starting_supplies.json` + `StartingSuppliesCatalog` | reads only |
| Starting cohort | `starting_survivor_cohorts.json` + `StartingCohortCatalog` | reads only |
| Difficulty presets, scalars, custom, lock | `DifficultySettingsSystem` | reads only; selects through `SelectPreset` and `LockSettings` |
| Iron Man mode, terminal seal | `CampaignMode`, `SaveSlotService` | reads only; result read on terminal |
| Endings | `endings.json` + `HoldfastEndings` | reads only (a road goal may name one) |
| Inventory, morale | inventory and survivor owners | requests only (Opening Choice deltas) |
| Chronicle | chronicle owner | one line per Charge and per Mark |
| **The named bundles (Starts)** | — | `starts.json` (data) |
| **Which Start was chosen, the Opening Choice made, first-lapse days** | — | `StartState` (stored) |
| **Duty words, StartDelta, Charge fade** | — | pure derived read models |
| **Vows, Roads, Marks (definitions)** | — | `vows.json`, `roads.json`, `mark_lines.json` (data) |
| **Chosen road, first-broken days, result** | — | `HardRoadState` (stored) |
| **Vow status words** | — | pure derived read model (over witnesses) |

**Non-duplication statement.** Every stock, survivor, difficulty scalar, seal and ending stays with its owner. The plan stores only **what the player chose** (a start, an opening choice, a road, a vow set) and **what first happened** (the day a duty first lapsed; the day a vow first broke; how a road ended). Everything else — duty words, StartDelta, vow status, Charge fade, Mark text — is derived on read or written once to the chronicle. **No new stock, roster, difficulty, seal, score or reward authority is created.**

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (OB):** `Assets/Ashfall.Core/Shelter/Starts/StartCatalog.cs` (loader + validators), `Starts/StartState.cs` (state + rules), `Starts/StartDelta.cs` (pure diff), `Starts/OpeningChoice.cs` (effects via requests), `Starts/Charge.cs` (duty predicates + words), `Starts/ChargeChronicle.cs`.

**Core (HR):** `Assets/Ashfall.Core/Difficulty/HardRoad/VowCatalog.cs`, `HardRoad/RoadCatalog.cs` (loaders + validators), `HardRoad/HardRoadState.cs` (state + rules), `HardRoad/VowWitness.cs` (closed witness table), `HardRoad/RoadResult.cs`, `HardRoad/MarkWriter.cs`. Additive nested DTOs in the shelter-identity save (Start) and the difficulty-settings state (Road, `SchemaVersion` 1 → 2, additive), `INT`, homes chosen at P0.

**Data (OB):** `starts.json`, `start_lines.json`, `opening_choices.json`, `charges.json`.
**Data (HR):** `vows.json`, `roads.json`, `mark_lines.json`.

**Host:** `src/Main.GameFlow.cs` (`INT`: a `StartNewGame(startId, difficultyId)` overload that resolves ids and forwards to the existing overload, and passes the origin id), `src/Main.ShelterIdentity.cs` (`INT`: honour a pre-selected origin instead of `SelectDefaultShelterOrigin`), `src/Main.DifficultySettings.cs` (`INT`: apply a road's preset, lock and mode), `src/Main.CampaignOwners.cs` (`INT`: daily Charge and vow evaluation, one call), the chronicle seam (`INT`).

**Presentation (both):** extend the existing main-menu / new-game surface (Start and Road picker with the derived comparison) and the existing shelter or chronicle surface (Charge board, vow status). No new routed panel (DEC-OB-08, DEC-HR-08).

**Tests:** `Ashfall.Core.Tests/Shelter/Starts/*` (Catalog, Delta, Opening, Charge, Save); `Ashfall.Core.Tests/Difficulty/HardRoad/*` (Vows, Roads, Witness, Result, Mark, Save); parity guards extend existing shelter-identity, starting-supply, starting-cohort, difficulty, lock and Iron Man tests unchanged.


---

# PART ONE — OTHER BEGINNINGS (OB)

## 5. The Start (authored bundle)

A **Start** is one row in `starts.json`. It **names** three existing catalog rows and **adds** a place frame, an Opening Choice and a Charge. It never copies a value from those rows.

### 5.1 Row schema

```
start: {
  id,                       // "start_garrison_post"
  name, place,              // "The Garrison Post", "garrison outpost"
  blurb,                    // two plain lines
  origin_id,                // key into shelter_origins.json
  supplies_profile_id,      // key into starting_supplies.json
  cohort_profile_id,        // key into starting_survivor_cohorts.json
  opening_choice_id,        // key into opening_choices.json, or null
  charge_id,                // key into charges.json, or null
  suggested_preset_id       // optional; a suggestion only, never applied silently
}
```

| Field | Rule |
|---|---|
| three id fields | **each resolved against its own catalog** (E6). The single word "origin" is never used as a key. |
| `opening_choice_id` / `charge_id` | both null for `start_standard_holdfast` |
| `suggested_preset_id` | must exist in `difficulty_presets.json`; shown as text, **never** selected for the player (difficulty is its own axis — DEC-OB-04) |
| `blurb` | ≤ 240 characters; describes what is *in the room*, never what the player should feel |

### 5.2 Why a bundle and not three pickers

The three axes already exist and stay independently usable (a CLI or a tester may still pass a cohort and a supply id directly). A Start is a **convenience and a commitment**: it selects a coherent triple, states it plainly, and — because it is recorded — lets the campaign remember the name of its beginning.

---

## 6. The Five Rows (one unchanged, four new)

| Start | Place | Origin (E1) | Supplies (E2) | Cohort (E3) |
|---|---|---|---|---|
| `start_standard_holdfast` | the unchanged opening | `origin_government_bunker` (today's default) | `origin_standard_holdfast` | `cohort_standard_holdfast` |
| `start_garrison_post` | garrison outpost | `origin_military_outpost` | `origin_evacuated_checkpoint` | `cohort_standard_holdfast` |
| `start_relief_station` | relief station | `origin_mining_facility` | `origin_waterworks_failure` | `cohort_growers_stewards` |
| `start_halted_caravan` | caravan | `origin_improvised_cellar` | `origin_machine_room` | `cohort_convoy_remnant` |
| `start_ward_below` | hospital | `origin_government_bunker` | `origin_field_clinic` | `cohort_triage_ward` |

**Standard Holdfast is a Start row with no choice and no charge**, so the picker is uniform and today's default behaviour is exactly reproduced (parity test, §19).

### 6.1 Why these triples

- **The Garrison Post.** *Military outpost* gives shielding +1500 bp, ventilation +500 and space +500 with its own drawbacks (austere quarters, scarce medical supplies). *Evacuated checkpoint* stores are batteries (8), filter packs (4), calibration kits (2) and a second dosimeter — the kit of a post that measured things — and thin food and water. The standard cohort's *Gunner Mikhail (Heavy Artillery Loader)* is the nearest thing the shipped roster has to a garrison veteran. A new cohort row is a **backlog item** (§24), not this plan.
- **The Relief Station.** *Subterranean mining facility* has the most room in the catalog (space +2500 bp) with the worst air (ventilation −1500), which is the shape of a place that expects to house more than it can ventilate. *Waterworks failure* stores carry two desalination membranes and three HEPA filters — a relief station's water and air kit — and no hazmat suit. *Growers and stewards* is the food-minded cohort with thin expedition and perimeter cover.
- **The Halted Caravan.** *Improvised cellar* is the origin with rapid surface access and the weakest shielding (−2000 bp): the place a column stopped and dug in. *Machine room* stores are the repair kit of a convoy (battery 14, mechanical scrap 24, electronic scrap 18) with no rad-away and no hazmat suit. *Convoy remnant* — "people used to moving through bad country, arriving with the cost already on them" — carries the highest starting doses in the roster (24–34, one `acuteRad`).
- **The Ward Below.** *Government continuity bunker* is deep, sealed and cramped (shielding +2500, space −1000). *Field clinic* stores are bandages 8, rad-away 3, iodine 6 and a calibration kit, with thin pantry and water. *Triage ward* is three people "who can stabilize a crisis" with the lowest starting doses (12–18).

**Not a location.** None of the four is a place on the map, none references a location id, and none places, unlocks or consumes an `outposts.json` row (E12, DEC-OB-06). The *Ward Below* is not the Regional Hospital ruin of the Ruins plan (DEC-OB-07).

---

## 7. The StartDelta (derived; nothing stored)

`StartDelta.Compare(start, baseline)` returns a plain table of differences from **today's opening** — `start_standard_holdfast`. It reads the three catalogs and nothing else; it is recomputed on demand.

### 7.1 Rows in the table

| Section | Rows | Rule |
|---|---|---|
| **Supplies** | one per item whose amount differs from the baseline | signed integers, sorted by item id; items absent on one side count as 0 |
| **Walls** | shielding, ventilation, space (basis points → percent) | signed, from `origin_id` |
| **People** | count, mean health, mean lifetime dose, count `acuteRad` | mean rounded to one decimal; names listed |
| **Drawbacks** | the origin's `starting_drawbacks` list, verbatim | never paraphrased |

### 7.2 Worked deltas (real shipped numbers; baseline = standard holdfast)

**The Ward Below** (origin same as baseline):

| Supplies | Δ | | Supplies | Δ |
|---|---|---|---|---|
| clean_water | −4 | | bandage | **+6** |
| canned_food | −6 | | rad_away | **+2** |
| irradiated_water | −2 | | iodine_pills | +2 |
| item_air_filter_hepa | −1 | | calibration_kit | +1 |
| item_desal_membrane | −1 | | filter_pack | +1 |
| battery | −2 | | scrap_mechanical | −4 |
| scrap_electronic | −2 | | | |

Walls: no change. People: 3; mean health 86.0 (baseline 88.3, **−2.3**); mean dose 15.3 (baseline 20.0, **−4.7**); `acuteRad` 0 (baseline 1).

**The Halted Caravan:**

| Supplies | Δ | | Supplies | Δ |
|---|---|---|---|---|
| battery | **+10** | | clean_water | −6 |
| scrap_mechanical | **+18** | | canned_food | −6 |
| scrap_electronic | **+15** | | iodine_pills | −2 |
| calibration_kit | +3 | | bandage | −1 |
| item_air_filter_hepa | −1 | | rad_away | −1 |
| item_desal_membrane | −1 | | hazmat_suit | −1 |

Walls: shielding **−4500 bp** (−2000 vs +2500), ventilation −1000 bp, space +500 bp. People: 3; mean health 84.0 (**−4.3**); mean dose 28.7 (**+8.7**); `acuteRad` 1.

**The Garrison Post:**

| Supplies | Δ | | Supplies | Δ |
|---|---|---|---|---|
| battery | **+4** | | clean_water | −6 |
| filter_pack | **+4** | | canned_food | −8 |
| calibration_kit | +2 | | bandage | −1 |
| item_dosimeter_pen | +1 | | item_air_filter_hepa | −1 |
| scrap_electronic | −1 | | item_desal_membrane | −1 |

Walls: shielding −1000 bp, ventilation −1000 bp, space **+1500 bp**. People: the standard three, no change.

(The Relief Station delta is produced by the same function; its cohort numbers are filled at P0 from `cohort_growers_stewards`. Supplies: clean_water −6, canned_food −4, item_air_filter_hepa +1, item_desal_membrane +1, iodine_pills −2, bandage −1, hazmat_suit −1, battery +2, scrap_mechanical +4, scrap_electronic +1, filter_pack +2. Walls: shielding −1500 bp, ventilation **−3000 bp**, space **+3500 bp**.)

### 7.3 What the picker shows

The picker shows the delta as a two-column list (*more* / *less*), the origin's drawbacks as written, the Start's blurb, and — as plain text — the Start's `suggested_preset_id` name (*"People who begin here often find STANDARD sufficient."* — content candidate, never a recommendation engine). It shows **no rating, no star count, no "recommended"** (DEC-OB-04).

---

## 8. The Opening Choice (once, small, in days 0–3)

Each non-standard Start has one **Opening Choice**: a one-sentence situation and two options. It is the only *decision* a Start adds, and it can be made **once**, on days 0–3 (`OpeningWindowDays = 3`), from the shelter surface. After day 3 the choice **closes unchosen** (recorded as `choiceId = null`, `choiceDay = −1`).

### 8.1 Effect vocabulary (closed)

| Effect | Owner called | Bound |
|---|---|---|
| `item_delta { item, amount }` | inventory owner (add/remove) | item exists in `items.json`; amount within ±10; a removal never takes stock below 0 (clamped, logged) |
| `morale_all { amount }` | survivor needs owner — `Needs.ApplyAttributedDelta(id, Morale, delta, "start:<choice_id>")` once per living survivor (E15) | within ±6; clamped by the owner |
| `note { kind }` | notebook line | `kind` from `start_lines.json` |

No other verb exists. An Opening Choice **cannot** add a survivor, change the origin, grant an item that does not already appear in a starting-supply profile, or change a difficulty scalar (validator OB-V5).

### 8.2 The four choices

| Choice id | Situation | Option A | Option B |
|---|---|---|---|
| `oc_garrison_cache` | The ordnance cache is behind the cage. The sergeant has the key and has not offered it. | **Keep the schedule** — battery −2, morale_all +4 | **Break up the cache** — scrap_mechanical +6, morale_all −3 |
| `oc_relief_line` | The register is open on the desk. There is a line outside that nobody has counted. | **Open the line** — canned_food −3, clean_water −2, morale_all +5 | **Ration the line** — canned_food −1, morale_all +2 |
| `oc_caravan_wagons` | The wagons are still hitched. Someone has already unchained the first. | **Strip the wagons** — scrap_mechanical +8, scrap_electronic +4, morale_all −3 | **Keep them loaded** — morale_all +3 |
| `oc_ward_pharmacy` | The pharmacy is behind a grille. The nurse is standing in front of it. | **Run the ward** — bandage −2, morale_all +4 | **Lock the pharmacy** — morale_all −2 |

Each option's effect is small enough to matter in the first week and vanish by the first season — a *character note*, not a build path. (Balance is checked at P4 against the day-30 stock profile; DEC-OB-03.)

### 8.3 What the notebook says

The chosen option writes one `note` line (§1b voice), for example: *"They broke the cage on the third morning. The sergeant watched and said nothing."* The unchosen option writes nothing. A choice that closes unchosen writes no line.

---

## 9. The Charge (three duties, sixty days, derived)

### 9.1 What a Charge is

A **Charge** is a named row in `charges.json` listing three **Duties**. Each duty is one **predicate** from a closed table, evaluated **on read** against a `ChargeSnapshot` (a small struct of counts the host supplies from existing owners). Nothing in a Charge changes any owner's state (DEC-OB-05).

### 9.2 Predicate kinds (closed; all fields LIVE per E3/E14 except where noted)

| Kind | Reads | Kept when | Lapsed when |
|---|---|---|---|
| `stock_min(item, N, margin)` | inventory count of `item` | count ≥ N + margin | count < N |
| `roster_living(N)` | living survivors | living ≥ N | living < N |
| `health_floor(N, margin)` | minimum health among living | min ≥ N + margin | min < N |
| `dose_ceiling(N, margin)` | maximum lifetime dose among living | max ≤ N − margin | max > N |
| `morale_mean(N, margin)` | mean morale among living | mean ≥ N + margin | mean < N |

**Strained** is the band between: for the "min" kinds, `N ≤ value < N + margin`; for `dose_ceiling`, `N − margin < value ≤ N`. `roster_living` has no margin (Kept or Lapsed). Words are **derived every time the board is drawn**; the only stored fact is the *first day* a duty was Lapsed (§10).

### 9.3 The four Charges

| Charge | Duty | Predicate | Start value(s) it is judged against |
|---|---|---|---|
| **The Post** (garrison) | Hands | `roster_living(3)` | 3 |
| | Lights | `stock_min(battery, 4, margin 2)` | battery 8 → **Kept** |
| | Health | `health_floor(50, margin 10)` | min 80 → **Kept** |
| **The Line** (relief) | Water | `stock_min(clean_water, 4, margin 2)` | 6 → **Kept** |
| | Pantry | `stock_min(canned_food, 8, margin 2)` | 12 → **Kept** |
| | Spirits | `morale_mean(45, margin 5)` | from cohort (P0) |
| **The Road** (caravan) | Cost | `dose_ceiling(120, margin 30)` | max 34 → **Kept** (Strained above 90) |
| | Wagons | `stock_min(scrap_mechanical, 10, margin 6)` | 24 → **Kept** |
| | Hands | `roster_living(3)` | 3 |
| **The Ward** (hospital) | Patients | `health_floor(60, margin 10)` | min 84 → **Kept** |
| | Dressings | `stock_min(bandage, 4, margin 2)` | 8 → **Kept** |
| | The drug cabinet | `stock_min(rad_away, 1, margin 1)` | 3 → **Kept** |

Every duty **begins Kept** by construction (validator OB-V6 checks each against its Start's own catalog values). A Charge is a picture of *what the place expects to keep*, not a puzzle to solve on day 1.

### 9.4 Fade, Set Down and the journal line

- **Fade.** On day `OpenedDay + ChargeDays` (default 60) the Charge **fades**: `ended = faded`, `endedDay` stored, one journal line composed from `start_lines.json` templates.
- **Set Down.** The player may **Set Down** the Charge at any time from the board. `ended = setDown`, `endedDay` stored, one line. Nothing else changes; a set-down Charge is simply no longer shown.
- **Line composition (deterministic).** Tokens: `{start}`, `{duty}`, `{day}`. One clause per duty that ever lapsed (*"The lights lapsed on day 23"*), a closing clause from the duty's **word at the moment of fade** (*"…and were holding again at the end"* if the word is Kept or Strained; *"…and did not recover"* if Lapsed). A Charge with no lapse reads *"The post was held."* Variant selection uses a seeded fork (`CampaignStreamIds.StartCharge`, proposed) keyed by `(startId, dutyId)`, never `System.Random`.

### 9.5 Why words and history only

A Charge that *rewarded* keeping (morale, items) would invent a bonus economy and reopen every balance decision; one that *punished* lapsing would create a new fail state. This plan does neither (DEC-OB-05). The Charge's value is that it **gives the first sixty days a shape** and **leaves a true line in the chronicle**. A later plan may attach consequences through the owners' own seams (backlog §24).

---

## 9b. Texture — Other Beginnings

- **The Garrison Post.** The duty board is chalk on a slate, and the last entry is a Tuesday. Someone has drawn a line under the day and not written the next.
- **The Relief Station.** The register has a column headed *Family* and a column headed *Left*. Only the first is filled in.
- **The Halted Caravan.** Drivers' initials on the tailboards. The lead wagon's keys are on a hook by the door, on the inside. Nobody says who put them there.
- **The Ward Below.** Beds made to a standard. A whiteboard with names and a time next to each; the last time is the same for all of them.
- **The first evening.** In each Start the first evening has a different sound: the post is quiet because it is disciplined; the station is loud because it is not; the caravan is busy because it has not stopped; the ward is low because it has learned how. (Evenings plan may read this; ships dark.)

**Content rules for beginnings (DEC-OB-09).** No Start is built around children (schools, nurseries, paediatric wards); the corpus may reference such places elsewhere but this plan builds none. `origin_school_basement` remains selectable through the existing origin call and is used by **no** Start. No Start references a real country, war or person.

---

## 9c. Four First Mornings (content candidates for `start_lines.json`)

Each Start opens with one paragraph on the shelter surface — read once, on day 1, then kept in the journal. They are written for a person standing in the room, not for a player reading a menu. None names a cause; none says what to do.

**The Garrison Post.**
> *The duty board is slate, and the chalk is still in the tray. The last entry is a Tuesday: three names, a shift, a line drawn under it. Mikhail reads it twice and does not rub it out. Someone has to write the next line, and it turns out to be him. He writes the date. Then, because nobody stops him, he writes the three names again.*

**The Relief Station.**
> *The register lies open on the desk with the pen still on it. Two columns: FAMILY and LEFT. The first is full down the page in a dozen hands. The second has two entries, both in the same careful writing, both from the first day. Outside the door the noise has stopped, which is worse. Someone is going to have to decide whether to count what is out there.*

**The Halted Caravan.**
> *The wagons are still nose to tail, tarpaulins roped down, drivers' initials chalked on the tailboards: R.D., M.L., a heart with nothing in it. The lead wagon's keys hang on a hook inside the door. Morgan looks at them for a long time and then turns her back on them. It is the first thing anyone here has decided.*

**The Ward Below.**
> *Beds made to a standard, corners squared. A whiteboard with names in blue marker and a time beside each — the same time, all the way down. Somebody has wiped one name off, carefully, with the side of a hand. Riley finds the marker on the sill and stands holding it and does not put it back.*

**The register of the room.** Notice what the four have in common: someone standing in front of an object that belongs to someone who is not there, and a small act that is theirs. The game supplies the object. The people supply the act. (Tone rule: *the people supply the character.*)

**Content rule.** Lines name no cause, no author, no date before day 0, no faction, and no number that the StartDelta does not already show.

---

## 10. Save shape (OB)

One additive nested DTO inside the shelter-identity save section (home chosen at P0, DEC-OB-01 — it already stores `OriginId`):

```
start: {
  version: 1,
  startId,                    // null for a legacy or standard-by-default campaign
  openedDay,
  choiceId,                   // null until chosen or closed
  choiceOptionId,             // "a" | "b" | null
  choiceDay,                  // -1 unless chosen
  duties: [ { dutyId, firstLapsedDay } ],   // firstLapsedDay -1 until lapsed
  ended,                      // 0 active, 1 faded, 2 setDown
  endedDay
}
```

Restore rules: missing block = no Start (legacy behaviour); an unknown `startId` is dropped with a log and the Charge disappears (the three chosen catalog rows are already applied and unaffected); `firstLapsedDay` clamps to ≥ −1; `ended` outside 0–2 resets to 0. **No new save section.** Derived values (duty words, StartDelta, board text) are recomputed on read.

---

## 11. Host wiring (OB)

| Seam | Owner | Change |
|---|---|---|
| New-game entry | `Main.GameFlow` (`INT`) | `StartNewGame(startId, difficultyId)` overload: resolve the row; call the existing overload with its cohort and supply ids; pass `origin_id` |
| Origin | `Main.ShelterIdentity` (`INT`) | if a Start pre-selected an origin, use it instead of `SelectDefaultShelterOrigin`; otherwise unchanged (E4) |
| Opening Choice | shelter surface | one command `MakeOpeningChoice(optionId)` while `day ≤ 3` |
| Daily Charge tick | `Main.CampaignOwners` (`INT`) | one call: `Charge.Tick(day, snapshot)` — records first lapses, fades on day 60 |
| Snapshot source | host adapter | an `IChargeSnapshotSource` reading inventory counts and survivor stats (Core stays engine-free) |
| Board | existing shelter or chronicle surface | a Charge section; a **Set Down** button |
| CLI | `HostCliRegistry` (`INT`) | optional `--start <id>` beside the existing `--supplies` flag (DEC-OB-02) |

Ships behind a flag (`Starts.Enabled`, default **false** until P4). With the flag off the picker lists only *Standard Holdfast* and every path equals today's.


---

# PART TWO — THE HARD ROAD (HR)

## 12. Principles

1. **Witnessed, not policed.** A vow is a promise the game can *check*, not a rule the game *enforces*. The player is never prevented from breaking one (DEC-HR-03). What the game does is notice, once, and remember the day.
2. **No witness, no vow.** A vow is offered only if a fact that proves its break exists in an owner's ledger. An honour-system vow is refused by the validator (HR-V3). Where the fact is not yet counted, the vow is **NEEDS-COUNTER** and the roads that use it are **gated** (not offered) until an owner supplies the counter.
3. **Difficulty is not rebuilt.** The four presets, eight scalars, custom lanes, lock and Iron Man behave exactly as today (E7–E9). A Road *selects* them; it never redefines them.
4. **Nothing is given back.** No currency, unlock, score, badge or meta-progression (DEC-HR-05). The only product of a road is one line of truth in the chronicle.
5. **The promise is chosen knowingly.** Every road card states, in order, what will be **locked**, **sealed**, **where you begin**, **what you have promised** and **where it ends**, in derived, plain text.

---

## 13. Vows (ten authored; three witnessed today)

A **Vow** is a row in `vows.json`: a one-sentence promise, a plain "watches" line, a **witness** from a closed table, a `kept_line` and a `broken_line` (which must contain `{day}`).

### 13.1 Witness table (closed)

| Witness kind | Reads | True (= broken) when | Status |
|---|---|---|---|
| `roster_join_after` | survivors' `joinedDay` versus the road's `openedDay` | any living survivor has `joinedDay > openedDay` | **LIVE** (E14) |
| `roster_loss` | the opening roster (ids recorded at open) | any opening survivor is dead, departed or absent | **LIVE** (roster; departure semantics VERIFY) |
| `dose_over(N)` | maximum lifetime dose among living | max > N | **LIVE** (E3) |
| `counter_gt0(counterId)` | an owner-supplied, monotone, integer counter | counter > 0 | **NEEDS-COUNTER** until registered |

Counters requested from owners (each is a single monotone integer the owner already knows how to increment at one call site; none is created by this plan): `trades`, `expeditions`, `night_expeditions`, `vehicle_expeditions`, `irradiated_water_drunk`, `radio_transmissions`, `refusals`. Registration is an `INT` package (P6); until then the vows using them are gated.

### 13.2 The ten vows

| Vow id | Promise | Watches | Witness | Status |
|---|---|---|---|---|
| `vow_closed_door` | We will not take anyone in after today. | anyone who joins after day 0 | `roster_join_after` | LIVE |
| `vow_all_hands` | We will bring everyone through. | any of the first roster lost | `roster_loss` | LIVE |
| `vow_whole_dose` | Nobody goes past the line. | anyone with a lifetime dose over 200 | `dose_over(200)` | LIVE |
| `vow_no_market` | We will not trade. | any trade | `counter_gt0(trades)` | NEEDS-COUNTER |
| `vow_stores_only` | We will not go out for anything. | any expedition | `counter_gt0(expeditions)` | NEEDS-COUNTER |
| `vow_dark_nights` | We will not go out after dark. | any expedition begun at night | `counter_gt0(night_expeditions)` | NEEDS-COUNTER |
| `vow_slow_road` | We will not ride. | any vehicle used outside | `counter_gt0(vehicle_expeditions)` | NEEDS-COUNTER |
| `vow_sealed_water` | We will not drink what the counter will not clear. | any irradiated water drunk | `counter_gt0(irradiated_water_drunk)` | NEEDS-COUNTER |
| `vow_no_radio` | We will not put a voice on the air. | any transmission | `counter_gt0(radio_transmissions)` | NEEDS-COUNTER |
| `vow_open_hand` | We will not turn anyone from the door. | any refusal at the door | `counter_gt0(refusals)` | NEEDS-COUNTER (blocked on the Living Region plan) |

**The dose line.** `dose_over(200)` is half of `RadiationSystem.ChronicLifetimeThreshold` (400, E20) and is far above every shipped starting dose (8–38, E3). It leaves room for a hard season and breaks when someone has truly been *taken past the halfway mark* to chronic illness. The exact figure is a playtest value (DEC-HR-14); the validator requires only that it exceeds every shipped cohort's maximum starting dose and stays below 400.

### 13.3 Kept and broken lines (per vow, authored)

| Vow | `kept_line` | `broken_line` |
|---|---|---|
| closed door | *Nobody was let in.* | *The door opened on day {day}.* |
| all hands | *Everyone came through.* | *Someone did not come through, on day {day}.* |
| whole dose | *Nobody went past the line.* | *Someone went past the line on day {day}.* |
| no market | *Nothing was traded.* | *The first trade was on day {day}.* |
| stores only | *Nobody went out.* | *Someone went out on day {day}.* |
| dark nights | *They kept the nights.* | *They went out after dark on day {day}.* |
| slow road | *They walked.* | *They rode, on day {day}.* |
| sealed water | *They drank clean.* | *They drank the other water on day {day}.* |
| no radio | *The air stayed empty.* | *They spoke on day {day}.* |
| open hand | *No one was turned away.* | *A door was closed on day {day}.* |

Lines are **facts**, in the register of a ledger. No line contains a value judgement (validator HR-V8 rejects the words *failed*, *shame*, *weak*, *proud*, *hero*).

---

## 14. Roads (five authored bundles)

A **Road** is a row in `roads.json`.

```
road: {
  id, name, blurb,
  difficulty_preset_id,    // key into difficulty_presets.json
  lock,                    // bool: LockSettings at open
  iron_man,                // bool: CampaignMode.IronMan at slot creation
  start_id,                // optional: pins a Start (OB)
  vow_ids,                 // 1..4
  goal                     // { kind: "day", value: N }  or  { kind: "ending", id }
}
```

Rules: `iron_man` implies `lock` (DEC-HR-04, mirrors the "ironman style" lock of E8); a **day** goal is ≥ 90; an **ending** goal names an id in `endings.json`; a road with any NEEDS-COUNTER vow is **gated**.

### 14.1 The five

| Road | Difficulty | Lock | Iron Man | Begins | Vows | Goal | Available at ship |
|---|---|---|---|---|---|---|---|
| **The First Winter** | STANDARD | no | no | any | all hands | day 120 | **yes** |
| **The Closed Door** | AUSTERE | yes | no | Garrison Post | closed door, all hands | day 150 | **yes** |
| **The Long Ration** | DIRGE | yes | **yes** | any | all hands, whole dose | day 200 | gated (Iron Man entry, E19) |
| **Quiet Hands** | AUSTERE | yes | no | Ward Below | no market, dark nights, whole dose | day 150 | gated (trades, night expeditions) |
| **One More Dawn** | DIRGE | yes | **yes** | any | closed door, all hands, no market, whole dose | ending `ending_dawn_of_thaw` | gated (trades, Iron Man entry) |

### 14.2 The road card (derived)

The card for any road is generated, never authored as prose:

1. **Difficulty** — preset name and its eight scalars as numbers.
2. **Sealed** — *"Difficulty cannot be changed after you begin."* if `lock`; *"If the shelter is lost, this save cannot be restored."* if `iron_man`.
3. **Where you begin** — the pinned Start's name and its StartDelta link, or *"Your choice."*
4. **What you have promised** — each vow's promise and its *Watches* line.
5. **Where it ends** — *"Day N"* or the ending's display name.

**Worked card — The Long Ration (real shipped numbers, E7).** Difficulty DIRGE: hunger 1.75×, thirst 1.75×, radiation gain 1.6×, disease onset 1.5×, hostile encounters 1.75×, market prices 1.3×, equipment decay 1.5×, crisis deadlines 0.65×. A stock that lasts D days at STANDARD hunger lasts about **D ÷ 1.75 ≈ 0.57 D** here; a crisis that offered 10 days offers **6.5**. Sealed: difficulty locked; Iron Man on. Begins: your choice. Promised: *We will bring everyone through* (watches: any of the first roster lost); *Nobody goes past the line* (watches: anyone with a lifetime dose over 200). Ends: day 200. The card ends with the sentence DIRGE's own description supplies — *"A stable shelter is possible; comfort is not."*

### 14.3 Feasibility is a playtest fact, not a validator fact

No validator can prove a road is winnable. Every road carries a `playtest_note` field (free text, unshown) recording who played it, when, and to what day; a road is enabled only after P7 records a reached goal or an honest near-miss (DEC-HR-10). The three shipping roads are chosen because their vows are **roster-based** (nothing the player *must* do is forbidden), and DIRGE's own text states a stable shelter is possible.

---

## 15. The Run Record and the Mark

### 15.1 State (`HardRoadState`, stored)

`roadId`, `openedDay`, `openingRoster` (ids), per-vow `brokenDay` (−1 until broken; **the first day only**), `result` (0 open, 1 completed, 2 sealed, 3 abandoned), `resultDay`, `markWritten`. Nothing else. Vow **status** (*Kept so far* / *Broken on day N*) is derived from `brokenDay`.

### 15.2 The daily check

`HardRoad.Tick(day, snapshot)` (deterministic, no RNG): for each vow with `brokenDay = −1`, evaluate its witness against a `RoadSnapshot` supplied by a host adapter (`joinedAfterOpening`, `openingLost`, `maxDose`, counters). If the witness is true, `brokenDay = day`. Once set it is never changed. The day recorded is the day the check *observed* the break (within one day of the event; DEC-HR-11 — a per-event hook may tighten this later).

### 15.3 How a road ends

| Result | When | Then |
|---|---|---|
| **completed** | `day ≥ goal.value` (day goal) or the named ending is reached through `TriggerEnding` (E11) | Mark written; the campaign **continues** unless the ending itself ends it (DEC-HR-07); vows stop being watched |
| **sealed** | Iron Man terminal loss is recorded (`MarkActiveSlotTerminal`) | Mark written *before* the seal is applied; chronicle export stays available (E9) |
| **abandoned** | the player chooses **Set Down the road** | Mark written; vows stop being watched; difficulty and lock stay as they were |

### 15.4 The Mark (one line, composed)

`MarkWriter.Compose(road, state)` produces **one** journal line:

```
<opening clause> <one clause per vow, in road order> <closing clause>
```

- **Opening**: *"Walked {road} to day {day}."* (completed) · *"Walked {road}. The road ended on day {day}."* (sealed) · *"Set down {road} on day {day}."* (abandoned).
- **Vow clauses**: the vow's `kept_line` if `brokenDay = −1`, else its `broken_line` with `{day}`.
- **Closing**: none.

Variants for the opening clause (two per result) are chosen by a seeded fork (`CampaignStreamIds.HardRoad`, proposed) keyed by `(roadId, result)`. Clauses are joined with a single space; the whole line is ≤ 320 characters (validator HR-V10).

### 15.5 Worked example — The Closed Door on the Garrison Post

Road **The Closed Door**: AUSTERE, locked, begins as the Garrison Post, vows *closed door* and *all hands*, goal day 150. `openedDay = 0`; opening roster = Dr. Sarah Chen, Gunner Mikhail, Elena Vasquez (all `joinedDay 0`).

- **Day 88.** The tick sees a living survivor with `joinedDay = 88 > 0`. `vow_closed_door.brokenDay = 88`.
- **Day 100.** One of the opening three is dead. `vow_all_hands.brokenDay = 100`.
- **Day 150.** The goal is reached. `result = completed`, `resultDay = 150`. The Mark: *"Walked the Closed Door to day 150. The door opened on day 88. Someone did not come through, on day 100."*

Alternative: had nobody joined and nobody died, the same road would end with *"Walked the Closed Door to day 150. Nobody was let in. Everyone came through."* The two lines are the whole of what the game says either way.

---

## 15b. Texture — The Hard Road

- **The morning it is sworn.** The road card is on the table next to the register. The first line is the vow. Someone reads it aloud, and someone says it is a lot to promise.
- **The day it breaks.** The journal line does not have an exclamation mark. The survivors notice it by not mentioning it at supper.
- **The road that went on.** *"The vow was broken on day 41. The road went on."* — the game does not stop, does not scold and does not offer a way to un-break it.
- **A quiet Mark.** It sits in the chronicle between a harvest and a burial and is no larger than either.
- **The road nobody finished.** A sealed Iron Man road writes its Mark before the seal falls. The line is *"Walked the Long Ration. The road ended on day 73. Everyone came through."* The save cannot be reopened; the sentence can be read.

**The morning it breaks — a scene, for the tone.** It is day 88. A woman has come to the door in the rain with a bag on her shoulder and nothing else, and the shelter has voted before the game can say a word. The road card is on the table beside the register, where it has been for eighty-eight days, and nobody looks at it. In the journal that night, in whoever's hand it is: *"The door opened on day 88."* Nothing else. No comment on whether it was right. The next entry is about the water pump.

**The morning it holds.** Day 150. The road card is taken off the table and put in the drawer with the other papers. Someone reads the line aloud — *"Nobody was let in. Everyone came through."* — and it sounds smaller aloud than it did when they swore it, and it is not smaller.

**Content rules (DEC-HR-12).** Vow and Mark text is written in the register of a ledger: no adjectives of praise or blame, no score, no ranking. No reference to real countries, wars or people. No Road is named for a real event.

---

## 16. Compatibility with what exists

| Existing behaviour | Effect of this plan |
|---|---|
| `SelectPreset` and `SetCustomScalar` refuse once locked | unchanged; a Road with `lock` calls `LockSettings` at open |
| Iron Man `CampaignMode` set at slot creation | today only `SaveSlotService.CreateSlot` accepts a mode; the host session hard-codes Normal (E19). A Road with `iron_man` needs `SaveLoadHostSession.CreateSlot` to pass the mode through (DEC-HR-13); until then such roads are gated |
| `MarkTerminal` seals the slot | unchanged; a `sealed` Mark is written first |
| Custom scalars | a Road **requires a preset** (no custom lanes); the picker disables custom while a Road is chosen |
| Difficulty scalars applied at owner sites | unchanged (E22-style guard: no scalar changes in this plan) |
| `hardcore_economy_tuning.json` | **left alone** — it is the live trade-screen tuning for all campaigns (E10, DEC-HR-09 closed) |
| A campaign started with no Road | identical to today; `HardRoadState` absent |
| A Road plus a Start | independent; two journal lines, never merged (§1.3) |

---

## 17. Save shape and host wiring (HR)

### 17.1 Save shape

One additive nested DTO inside the difficulty-settings state, bumping `SchemaVersion` 1 → 2 (additive; DEC-HR-01):

```
road: {
  version: 1,
  roadId,                     // null for a legacy or road-less campaign
  openedDay,
  openingRoster: [ survivorId ],
  vows: [ { id, brokenDay } ],  // brokenDay -1 until broken
  result,                     // 0 open, 1 completed, 2 sealed, 3 abandoned
  resultDay,
  markWritten                 // bool
}
```

Restore rules: `SchemaVersion` 1 (or a missing block) = no road (legacy behaviour); an unknown `roadId` is dropped with a log and the road disappears while the preset and lock already applied are untouched; unknown vow ids are dropped; `brokenDay` clamps to ≥ −1; `result` outside 0–3 resets to 0; `markWritten` prevents a second line. **No new save section.**

### 17.2 Host wiring

| Seam | Owner | Change |
|---|---|---|
| Picker | main-menu / new-game surface | road list beside starts; the derived road card; disabled custom lanes |
| Open | `Main.GameFlow` + `Main.DifficultySettings` (`INT`) | `SelectPreset`, `LockSettings` if lock, request `IronMan` if set, apply the pinned Start (OB), record `openingRoster` |
| Daily check | `Main.CampaignOwners` (`INT`) | one call `HardRoad.Tick(day, snapshot)` |
| Snapshot source | host adapter | `IRoadSnapshotSource`: roster ids, `joinedDay`, doses, and the registered counters |
| End: goal | same | when `day ≥ goal` or ending reached, compose Mark and append to chronicle |
| End: sealed | `SaveLoadHostSession` (`INT`) | compose the Mark, then `MarkActiveSlotTerminal` |
| End: abandoned | shelter surface | a **Set Down the road** command |
| Journal | `JournalSystem.TryAddRawEntry(key, text, author, day)` (`INT` adapter) | one entry per Charge end (`key = start_charge_end`) and per Mark (`key = road_mark:<roadId>`); the per-key dedupe makes a second write impossible (E16) |

Ships behind a flag (`HardRoad.Enabled`, default **false** until P7). With the flag off the picker shows no roads and every difficulty, lock and Iron Man path equals today's.

---

## 18. Validators (row-level failure output)

| ID | Rule | Failure prints |
|---|---|---|
| OB-V1 | each of `origin_id`, `supplies_profile_id`, `cohort_profile_id` resolves **in its own catalog** (E6) | `start_id`, field, id |
| OB-V2 | `opening_choice_id` and `charge_id` resolve or are both null | `start_id` |
| OB-V3 | `blurb` ≤ 240 chars; `place` non-empty | `start_id` |
| OB-V4 | `suggested_preset_id` exists in `difficulty_presets.json` | `start_id` |
| OB-V5 | choice effects use the closed vocabulary; item ids exist; amounts within bounds; every granted item appears in some starting-supply profile | `choice_id`, effect |
| OB-V6 | every duty **begins Kept** against its Start's catalog values (before the Opening Choice), and **no option of the Start's Opening Choice leaves any duty Lapsed** (Strained is allowed and reported) | `charge_id`, duty, option, value |
| OB-V7 | duty kinds in the closed table; margins ≥ 0; item ids exist | `charge_id`, duty |
| OB-V8 | no Start references `origin_school_basement` or a children tag | `start_id` |
| OB-V9 | `start_standard_holdfast` equals the catalog defaults (origin, supplies, cohort) | field |
| OB-V10 | start ids unique | `start_id` |
| HR-V1 | `difficulty_preset_id`, `start_id`, `vow_ids`, ending id resolve | `road_id`, field |
| HR-V2 | vow witness kind in the closed table | `vow_id` |
| HR-V3 | a `counter_gt0` vow is NEEDS-COUNTER unless its counter is registered; no honour-system vow exists | `vow_id`, counter |
| HR-V4 | a road containing any NEEDS-COUNTER vow is `gated` and not offered | `road_id`, vow |
| HR-V5 | `iron_man` ⇒ `lock` | `road_id` |
| HR-V6 | day goal ≥ 90, or ending id exists | `road_id` |
| HR-V7 | 1–4 distinct vows per road | `road_id` |
| HR-V8 | every vow has `kept_line` and a `broken_line` containing `{day}`; no judgement words | `vow_id`, word |
| HR-V9 | `dose_over` value exceeds every shipped cohort's maximum starting dose and is below 400 | `vow_id`, value |
| HR-V10 | mark line variants closed by result; composed line ≤ 320 chars | `road_id`, result |


---

# PART THREE — ACCEPTANCE, DELIVERY, DECISIONS

## 19. Acceptance criteria

**OB**

| # | Criterion | Test |
|---|---|---|
| OB-A1 | All five Start rows load; OB-V1..V10 pass; each id resolves in its own catalog | `StartCatalogTests` |
| OB-A2 | `start_standard_holdfast` reproduces today's defaults exactly (origin, supplies, cohort) | `StartParityTests` |
| OB-A3 | StartDelta for the Ward Below equals the §7.2 table (bandage +6, rad-away +2, clean_water −4, canned_food −6; mean health −2.3; mean dose −4.7) | `StartDeltaTests` (parameterised over the four Starts) |
| OB-A4 | StartDelta walls are basis-point differences from the government bunker (caravan: shielding −4500, ventilation −1000, space +500) | `StartDeltaTests` |
| OB-A5 | Choosing a Start passes its cohort and supply ids to the existing new-game seam and selects its origin; no other seam is called | `StartSelectionTests` |
| OB-A6 | Opening Choice applies only closed-vocabulary effects, once, on days 0–3; closes unchosen after day 3 | `OpeningChoiceTests` |
| OB-A7 | An item removal never drives stock below 0 (clamped, logged); a morale delta clamps to the owner's range | `OpeningChoiceTests` |
| OB-A8 | Duty words for all five predicate kinds at the boundary values (Kept at N+margin, Strained at N+margin−1, Lapsed at N−1; ceiling mirrored) | `ChargePredicateTests` (parameterised) |
| OB-A9 | `firstLapsedDay` is recorded once and never overwritten by a later lapse | `ChargeStateTests` |
| OB-A10 | Charge fades on `openedDay + 60`; Set Down ends it at once; each writes exactly one journal line | `ChargeEndTests` |
| OB-A11 | Composed fade line names each lapsed duty with its day and the closing clause from the duty's word at fade | `ChargeChronicleTests` |
| OB-A12 | Save round-trip; legacy save loads with no Start; unknown `startId` dropped with a log | `StartSaveTests` |
| OB-A13 | Same seed and days produce identical lines on replay | `StartDeterminismTests` |
| OB-A14 | Flag off: picker lists Standard only; shelter-identity, starting-supply, starting-cohort and new-game tests unchanged | existing tests, run unmodified |

**HR**

| # | Criterion | Test |
|---|---|---|
| HR-A1 | Vow, Road and Mark catalogs load; HR-V1..V10 pass | `HardRoadCatalogTests` |
| HR-A2 | `roster_join_after` breaks on a survivor with `joinedDay > openedDay`; `roster_loss` breaks on death or absence of an opening survivor; `dose_over(200)` breaks on max dose > 200 | `VowWitnessTests` (parameterised) |
| HR-A3 | `brokenDay` records the first observing day and is never changed by later checks | `HardRoadStateTests` |
| HR-A4 | A vow whose counter is unregistered is NEEDS-COUNTER; a road that contains it is gated and not offered | `RoadAvailabilityTests` |
| HR-A5 | Open a road: preset selected, lock applied if set, IronMan requested if set, pinned Start applied, opening roster recorded | `RoadOpenTests` |
| HR-A6 | `completed` at `day ≥ goal`; the campaign continues; vows stop being watched | `RoadResultTests` |
| HR-A7 | `sealed`: the Mark is composed before `MarkActiveSlotTerminal`; chronicle export remains available | `RoadResultTests` |
| HR-A8 | `abandoned` writes a Mark and leaves difficulty and lock as they were | `RoadResultTests` |
| HR-A9 | The §15.5 example produces exactly the two documented lines | `MarkWriterTests` |
| HR-A10 | A Mark is written at most once (`markWritten`) across save/load | `MarkWriterTests`, `HardRoadSaveTests` |
| HR-A11 | The road card for The Long Ration shows the DIRGE scalars and the sealed statements | `RoadCardTests` |
| HR-A12 | Save round-trip; `SchemaVersion` 1 loads with no road; unknown ids dropped with a log | `HardRoadSaveTests` |
| HR-A13 | Flag off: no roads offered; difficulty, lock, Iron Man and save-slot tests unchanged | existing tests, run unmodified |

Verification uses `bin/run-scoped-tests` with the changed test classes only; the full suite runs only when the user types `RUN FULL TESTS`.

---

## 20. Worked examples

### 20.1 The Ward Below, sixty days (bandage duty)

Charge **The Ward**, duty *Dressings* = `stock_min(bandage, 4, margin 2)`: **Kept** at ≥ 6, **Strained** at 4–5, **Lapsed** at ≤ 3.

- **Day 0.** Field clinic supplies give bandage **8** → Kept.
- **Day 1 — Opening Choice.** *Run the ward* (bandage −2, morale +4) leaves **6**: exactly the Kept margin. *Lock the pharmacy* leaves 8 but morale −2.
- **Day 12.** Stock 5 → **Strained**. Nothing is stored.
- **Day 19.** Stock 3 → **Lapsed**. `firstLapsedDay = 19` is stored once.
- **Day 34.** The survivors found dressings; stock 9 → Kept. The stored day stays 19.
- **Day 60.** Fade. The word is Kept at the moment of fade. Chronicle: *"The ward was kept. The dressings lapsed on day 19 and were holding again at the end."*

### 20.2 The Relief Station's opening choice costs something

*Open the line* takes canned food 12 → **9** and clean water 6 → **4**. Against the Line's thresholds (water ≥ 4 + margin 2 = 6 for Kept; pantry ≥ 8 + margin 2 = 10) both duties are **Strained** from day 1 and neither is Lapsed (OB-V6). The player has chosen to be generous and has been shown, plainly, that the water is now at the edge. *Ration the line* takes food to 11 and leaves water at 6: both duties remain Kept, with +2 morale rather than +5.

### 20.3 The Long Ration on the Ward Below (available once DEC-HR-13 lands)

Difficulty DIRGE, locked, Iron Man, vows *all hands* and *whole dose*. The Start supplies 8 cans of the field clinic's 10 (after a choice) at hunger 1.75×: relative to today's opening (16 cans at 1.0×) the food endurance is `(10 ÷ 16) ÷ 1.75 ≈ 0.36`, **36%**, and the crisis clock is 0.65×. The picker shows these two numbers before the player commits; the vows are watched from day 0. On day 73 the shelter is lost; the Mark is written first — *"Walked the Long Ration. The road ended on day 73. Everyone came through."* — then the slot is sealed and cannot be restored. The chronicle can still be exported.

### 20.4 A Road and a Start are two lines

A player picks **The Closed Door** (pins the Garrison Post). At day 60 the Charge fades: *"The post was held."* At day 150 the road completes: *"Walked the Closed Door to day 150. Nobody was let in. Everyone came through."* The chronicle holds both. Neither knows about the other.

---

## 21. Hooks (all ship dark, `Null*` defaults)

| Hook | Purpose |
|---|---|
| `IStartHook` | start chosen, opening choice made or closed, duty first lapsed, charge ended |
| `IRoadHook` | road opened, vow broken (day), road ended (result) |
| `IChargeSnapshotSource` | host adapter: stock counts, living roster stats |
| `IRoadSnapshotSource` | host adapter: opening roster, joins, doses, registered counters |
| `IMarkSink` | writes a composed line to the chronicle (E16) |
| `IVowCounterRegistry` | owners register monotone counters by id (P6) |

Sister-plan reads only: the Year Two chronicle may cite a Mark; the Evenings plan may voice a Start's first evening; the Living Region plan may register the `refusals` counter that un-gates `vow_open_hand`. None is required.

---

## 22. Packages and order

| Pkg | Scope | Depends on | Ships |
|---|---|---|---|
| P0 | **pass 1 done (§2b)**; remaining: E14 counters (`trades`, `expeditions`, night, vehicle, water, radio) and departure semantics | — | notes only |
| P1 | OB core: catalog, StartDelta, StartState, OpeningChoice, Charge, chronicle composition, save, tests | P0 | dark |
| P2 | OB data: five Starts, four choices, four charges, lines; validators OB-V1..V10 | P1 | dark |
| P3 | new-game seam and origin honour (`INT`): `StartNewGame(startId, …)`, origin selection, daily tick, snapshot adapter | P1 | dark |
| P4 | OB surface: picker delta view, Charge board, Set Down; flag on for playtest | P2, P3 | on |
| P5 | HR core: vows, roads, witnesses, state, Mark writer, save, tests | P0 | dark |
| P6 | counters (`INT`, owner asks): register `trades`, `expeditions`, `night_expeditions`, … one owner at a time; un-gate vows as each lands | P5 | dark |
| P7 | HR data and surface: ten vows, five roads, road card, picker, result seams, sealed-before-seal ordering; playtest gate for each road | P5, P4 | on |
| P8 | closeout: sister-plan rows, documentation, handoff | P4, P7 | on |

Each package is a **bounded** unit: one truthful seam, focused tests, no parallel state. OB and HR can proceed in parallel until P7.

---

## 23. Decisions, tests, risks

### 23.1 Decisions (user/foreman signature required unless noted)

| ID | Question | Recommended |
|---|---|---|
| DEC-OB-01 | Save home for `start` | nested in the shelter-identity save |
| DEC-OB-02 | Add `--start <id>` CLI flag beside `--supplies` | yes |
| DEC-OB-03 | Opening Choice magnitudes (items ±10, morale ±6) | as §8, balance-checked at P4 |
| DEC-OB-04 | Difficulty stays a separate axis; Start only *suggests* a preset as text; no rating | yes |
| DEC-OB-05 | Charge has no rewards or penalties in this plan — words and history only | yes |
| DEC-OB-06 | Garrison Post ≠ `outposts.json` builds; no row placed, unlocked or consumed | yes |
| DEC-OB-07 | Ward Below ≠ the Regional Hospital ruin (Ruins plan) | yes |
| DEC-OB-08 | No new routed panel; picker and board extend existing surfaces | yes |
| DEC-OB-09 | No children-centred Start; `origin_school_basement` used by none | yes |
| DEC-OB-10 | Charge and Mark lines are written to the survivors' journal (`TryAddRawEntry`), once per key | yes |
| DEC-HR-01 | Save home for `road` | nested in the difficulty-settings state, `SchemaVersion` 1 → 2 |
| DEC-HR-02 | Witness table closed at four kinds | yes |
| DEC-HR-03 | Vows are witnessed, not policed | yes |
| DEC-HR-04 | `iron_man` implies `lock` | yes |
| DEC-HR-05 | No rewards, currency, unlocks or meta-progression | yes |
| DEC-HR-06 | Cross-campaign profile book of Marks | **out of scope**; backlog (needs a profile-store decision, E17) |
| DEC-HR-07 | Reaching a road's goal writes the Mark and the campaign continues | yes |
| DEC-HR-08 | No new routed panel | yes |
| DEC-HR-09 | ~~Leave `hardcore_economy_tuning` alone~~ **CLOSED at pass 1**: it is live trade tuning; roads never touch it | closed |
| DEC-HR-10 | A road is enabled only after a recorded playtest | yes |
| DEC-HR-11 | Break day = day observed by the daily check (±1 day) | yes; per-event hook later |
| DEC-HR-12 | Vow and Mark text in ledger register; judgement words rejected | yes |
| DEC-HR-13 | `SaveLoadHostSession.CreateSlot` gains a `CampaignMode` parameter (default Normal) so roads can enter Iron Man; `INT` | yes; without it Iron Man roads stay gated |
| DEC-HR-14 | `dose_over` value and caravan dose ceiling are playtest values (200 / 120) | yes |

### 23.2 Tests (focused; see §19)

Reuse parameterised fixtures for predicate boundaries (value = N−1, N, N+margin−1, N+margin), StartDelta over the four Starts, and witness kinds. No duplicated wording tests. Parity guards run the *existing* shelter-identity, starting-supply, starting-cohort, difficulty, lock, Iron Man and save-slot tests unchanged.

### 23.3 Risks

| Risk | Mitigation |
|---|---|
| A Start reads as a hidden difficulty tier | StartDelta shows only differences; no rating; difficulty separate (DEC-OB-04) |
| Charge feels toothless without consequences | intended for v1 (DEC-OB-05); consequences are a backlog item that must route through owners |
| Opening Choice unbalances the first week | small magnitudes; P4 balance check on the day-30 stock profile |
| `origin_*` namespace collision (E6) | three separate id fields; validator OB-V1 |
| Morale delta API missing (E15) | P0 verify; fall back to per-survivor loop through the survivor owner |
| A road is unwinnable | roster-based vows for shipping roads; playtest gate (DEC-HR-10); DIRGE text supports a stable shelter |
| Vow counters land unevenly across owners | gated roads; vows un-gate one at a time; no honour-system vow |
| Mark written twice or lost across reload | `markWritten` flag; save round-trip test |
| Iron Man seal races the Mark | Mark composed **before** `MarkActiveSlotTerminal` (HR-A7) |
| Save bloat | ids, days and small ints only; no text stored |

---

## 24. Expansion backlog (to finalise)

1. New cohort rows for the Garrison Post (a true garrison roster) and the Relief Station (a relief-post crew), and a matching supply row for a relief station's own stock — each an **additive catalog row** with its own decision.
2. Charge consequences through owners' own seams (a modest morale effect when a duty is Kept at fade; a small requisition when it lapses) — needs a balance decision and must not create a bonus economy.
3. Additional Starts (a mine head, a customs post, a ferry landing) once the four have shipped.
4. Profile-level **Book of Roads**: Marks that survive across campaigns (needs a save-profile owner decision, E17).
5. Custom Roads: the player composes difficulty, vows and goal from the ten vows (the *witness* rule stays: only offered vows).
6. Per-event vow hooks to replace the daily observation (exact break day).
7. More vows once counters exist (`vow_no_hunting`, `vow_one_fire`) — each needs a real witness.
8. Art and audio briefs for the Start picker and road card (a duty-board slate; a chalk line).
9. Interaction with the Year Two chronicle (Marks in the year-end summary) and Evenings (first-evening lines per Start).
10. Localisation pass on vow and mark text (ledger register must survive translation).

---

## 25. Open Mysteries and Deliberate Silence

Bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`. This plan never answers:

- **Why the garrison post was left**, who ordered its schedule or who stopped keeping it.
- **Who ran the relief station**, or what the last register entry was.
- **Where the caravan was going**, or why the lead wagon's keys are on the inside.
- **What the ward's last patient had.**
- **Whether a broken vow mattered.** The chronicle states the day and nothing else.
- **What "the line" (the dose vow) means** beyond a number; the game never says what happens to someone over it.

No supernatural. No real countries, wars or people. A Start is a place with a schedule; a Vow is a sentence somebody said.

---

## 26. Pre-flight checklist (before any P1 code)

1. Confirm no claim overlaps in `WORKTREE_OWNERSHIP.md` for the proposed paths; read `INTEGRATION_PLANS.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`.
2. Run P0: resolve every VERIFY (E10, E11, E14 counters, E15, E16, where `IronMan` is chosen, dose scale, `hardcore_economy_tuning` consumers); write findings before design changes.
3. Confirm `StartNewGame` overload shape and where the origin can be pre-selected (E4) without touching survivor binding order.
4. Run the existing shelter-identity, starting-cohort, starting-supply, difficulty and save-slot tests to establish a green baseline (scoped).
5. Get `STATUS: APPROVED BY USER` in this file (Rule 8).
6. Work one package at a time; hand off with files, commands, results and untouched shared paths.
7. On integration: mark `FULLY INTEGRATED` at the top multiple times and move to the integrated plans folder.

*End of plan.*
