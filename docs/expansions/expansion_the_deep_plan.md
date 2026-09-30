# ASHFALL — THE DEEP
### Sealed levels and anomalies below the shelter · The builders dug further than they told you.

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/the-deep-2026-09-29.md`
**Family:** "New Pressures and Places" — `docs/expansions/expansion_new_pressures_and_places_index.md` (subject 15 of the user's list).
**Tone lock (inherited):** cold, exhausted, human, restrained. Nothing below is haunted. Everything below is *measured* — and the instruments do not always agree.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.
**Name note:** *The Deep* (this plan: four authored sealed levels under the home shelter), *The Deep Works* (`expansion_deep_works_plan.md`: generated galleries in the tunnel-node graph), *Abyssal Anomalies* (the 30-record science archive) and the surface *AnomalyHazardSystem* (moving hazard zones on the map) are four different things. Ids are distinct: `deep_lv_*`, `deep_anom_*`, `deep_shift_*`.

---

> *"We dug down for water and found a door that had been closed from the other side."*
>
> There is no monster at the bottom of the shaft. There is a **procedure** — four levels, each with
> a Rule, a Tell and a Toll, kept by something that reads a ledger. The horror of The Deep is not
> that you will meet it. It is that you will *understand* it, and that understanding is what it has
> been charging for all along.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

The shelter already goes down. The shaft blueprint (`bp_deep_shaft_expansion`, "Sub-Level Shaft Excavation") takes the player from depth 1 to depth 5, twelve points of stability and a pile of scrap per level, and each new level is more grid to build on. Then it stops. Depth 5 is a floor. Nothing is ever *under* it.

Meanwhile the game keeps a second, quieter depth in its data. Seven **architect vault audits** describe a facility whose levels are numbered 01 to 06 — a curtain of lead and bismuth, a pair of air scrubbers, an archive core, a room of nutrient vats — each with a page of instrument numbers (`mechanics`: a leak of 240 µSv/h, a filter differential of 1,850 Pa, a peroxide value of 48.5) that **no code ever reads**. Thirty **abyssal records** — hydrophone pings, borehole logs, cryopod failures, salt-mine inscriptions — of which thirteen are switched off pending a "Phase 2". And a surface layer of *moving* anomaly zones that wander across the map and never come indoors.

The Deep is the expansion where **the shaft turns out not to be the bottom.** When you dig to the floor of your own shelter you find a seal that you did not pour. Behind it are four sealed levels — the builders' levels — that were sealed on purpose, by someone, for a reason that is a *rule*, not a ghost. Each level has one anomaly: **a Rule** (what it does, whether or not you are watching), **a Tell** (how an instrument shows it before it hurts), and **a Toll** (what it costs to keep it open). You can go down in shifts. You can read the level before you open it, or you can open it blind and pay. You can hold it, reseal it, or lose it and spend a bad week getting it back.

The promise: **nothing down there is trying to hurt you; it is just true, and the truth has a price per shift.**

Below the shaft is not a ruin. A ruin implies abandonment, and abandonment implies that whoever
was there has gone. These levels were *closed*: curtain, scrubbers, archive, vats — a sequence, a
procedure, an order of operations that somebody carried out and either did not finish or finished
too well. Something is still keeping the schedule. A weekly purge has a cadence, and a cadence
implies attention.

Read the three-line form carefully. The **Rule** is what is forbidden. The **Tell** is what proves
the Rule is still enforced. The **Toll** is what it costs to break it anyway. Three lines, equal in
weight, parallel in syntax — and after the first level the player knows the shape of the poem and
reads the fourth one differently.

### 1.2 Pillars

1. **Everything is measured.** No monsters, no jump-scares, no fake numbers. An anomaly is a rule with a tell and a toll. The mystery is *what the rule is*, not whether the game is fair.
2. **The instruments can disagree — and that is the puzzle.** A level is only *Read* when two readings agree. Until then its toll is shown as a **range**, not a number. This is the archive's own label (`AmbiguousRequiresReconciliation`) turned into a mechanic.
3. **Depth costs people, not resources.** The scarce thing is crew-days, calibration and nerve. A shift is a small, bounded, repeatable sacrifice.
4. **Sealed means someone chose it.** Every seal has a reason on paper. You open it knowing at least what the paper says.
5. **Small engines, four of them.** One level = one anomaly = one Toll, all authored, all ship-dark.

### 1.3 Not this

- Not a dungeon crawler. No combat below, no loot pinatas, no procedural maps (that is *The Deep Works*).
- Not a haunting. Cold spots and whispers are *readings*, and every reading has a source in the level's rule.
- Not the Architect's vault. Canon says that facility is **narrative-only** (see §2.1) and this plan does not break that.
- Not a new resource. Yields arrive through the inventory owner as ordinary stock; tolls are paid in ordinary stock, crew-days and dose.
- Not a punishment trap. Every "loss" state is recoverable.

## 2. What the code and data actually say (audit)

| # | Fact | Where | Status |
|---|---|---|---|
| 1 | Shaft excavation is one project type (`DepthExcavation`); completion raises `MaxDepthUnlocked`; the blueprint's `max_depth_level` is 5, cost 12 stability, 14 labour-days. | `Shelter/ShelterExpansionSystem.cs` L571–594, L911–912; `shelter_construction.json` | LIVE |
| 2 | Nothing consumes depth 5 as a threshold; the panel only prints `D{n}`. | `src/UI/ShelterOperationsPanel.cs` L187 | LIVE / GAP |
| 3 | Stability floor of 40 blocks construction; a warning seam exists. | `ShelterExpansionSystem.cs` L244, L270, L946 | LIVE |
| 4 | 30 abyssal records (hydrophone 8, borehole 7, cryopod 8, salt mine 7): **17 activated, 13 deferred** ("Phase 2"). Discovery is by producer id + `MinDay`; deferred rows never surface. | `Narrative/AbyssalAnomaliesProjection.cs` L44–350; `src/Main.Narrative.cs` L175–202 | LIVE |
| 5 | `AmbiguousRequiresReconciliation` exists only as the **fallback label** for unknown ids; no record carries it. | `AbyssalAnomaliesProjection.cs` L21, L367–369 | LIVE / GAP |
| 6 | 7 architect vault audits carry a `mechanics` object; **no reader** of it exists. 4 are discoverable at surface sites (steelworks → L03 curtain; agricultural research → L06 vats; metro station → L01 sanctuary and **L04 scrubbers**); 3 are deferred (L05 archive purge drill, L02 clock drift, L01 medical/last survivor). | `narrative/architect_vault_audits.json`; `Narrative/BlackProjectsArchiveSystem.cs` L155–160 | LIVE / GAP |
| 7 | A curated `contradicts` pair — the clock-drift finding casts doubt on the last-survivor census — "activate together in a dedicated mystery pass". | `BlackProjectsArchiveSystem.cs` L311; `docs/content/BLACK_PROJECTS_INTELLIGENCE_MATRIX.md` L86–87 | LIVE |
| 8 | **Canon:** `BUNKER-00-ARCHITECT-PRIME` is a *narrative-only historical facility*; "NOT in the location namespace; no vault exists physically in canon; access mapping forbidden"; compliance status never grants anything. | `BLACK_PROJECTS_INTELLIGENCE_MATRIX.md` L31, L77 | LIVE (**constraint**) |
| 9 | Surface `AnomalyHazardSystem`: moving zones with coordinates, radius (km), radiation rate, intensity decay, detection classes; never mutates health, never computes dose. Host wire and `AnomalyWatchPanel` exist. | `World/AnomalyHazardSystem.cs` L1–75; `src/Main.Anomaly.cs`; `anomalies.json` | LIVE |
| 10 | Instruments exist with **fidelity**: dosimeter devices carry `calibrationQuality`, `sensorCondition`, `readingsSinceCalibration` and a live `errorBandMsv` (measurement uncertainty ±). | `Radiation/DosimeterCalibrationSystem.cs` L12–64 | LIVE |
| 11 | Exposure owners with public writes: `RadiationSystem.Expose/AdjustDose`; `SurvivorMentalHealthSystem.AddStress` (positive-only; triggers a crisis at 750‰). | `Radiation/RadiationSystem.cs` L343, L401; `Needs/SurvivorMentalHealthSystem.cs` L130 | LIVE |
| 12 | Shelter atmosphere has air-purity/lighting/acoustic inputs and modifiers; a public input-update method. | `Shelter/ShelterAtmosphereSystem.cs` L78–89, L139 | LIVE |
| 13 | Save owner for shelter growth: `shelter_expansion`; state holds rooms, projects, stability, `MaxDepthUnlocked`. | `Save/SaveSectionRegistry.cs` L270; `ShelterExpansionSystem.cs` L225–235 | LIVE |
| 14 | `ConstructionProjectType.ExpansionTunnel` is unused — and **claimed by The Deep Works** (DEC-DW-03). | `ShelterExpansionSystem.cs` L27–34 | LIVE (**contested**) |
| 15 | The four deferred cryopod rows describe pods with identity corruption, a manual-thaw breach, a power-shedding cascade and a "terminal protocol". Four activated cryopod rows already sit at surface sites (government bunker, low-background lab, abandoned hospital). | `AbyssalAnomaliesProjection.cs` L205–235 (active), L245–278 (deferred) | LIVE |
| 16 | Which survivors are free for a "shift" (job, crew state) and how the host reads it. | crew/job owners | **VERIFY (P0)** |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **The Architect's vault is not a place.** The matrix forbids mapping any real site to `BUNKER-00-ARCHITECT-PRIME`. So The Deep does **not** open the vault. The proposal (DEC-TD-01): the shelter was **built to the same drawings** — the architects' plan called for six levels, and *your* shelter's builders poured only four and sealed them. The four audit records already discoverable on the surface become **Readings** for the matching level ("what the sibling's paper says"). The two upper levels were "built somewhere else", which is exactly what the deferred audits describe.
2. **Depth 5 is a floor and the game already says so.** The shaft's blueprint tops out at 5; The Deep does **not** raise it. The seal is found *at the foot of the existing shaft* (read-only of `MaxDepthUnlocked`), never by changing the blueprint.
3. **Two "deep" expansions must not fight over one slot.** The Deep Works claims `ExpansionTunnel`. The Deep uses **no construction project**: opening is its own small ledger (DEC-TD-02).
4. **"Anomaly" is overloaded.** The surface layer moves and has coordinates; a deep anomaly is fixed to a level and has none. Ids, code and UI use different nouns (*zone* vs *rule*).
5. **The deferred rows are deferred for reasons.** The audit matrix deferred L05, L02, L01-medical as lethal, contradictory or "deliberately ambiguous". The Deep does not switch any of them on without a signature (DEC-TD-03/04).

---

# PART II — THE LEVELS

## 3. The Seal

### 3.1 Finding it
When the shaft reaches depth 5 (read-only of `MaxDepthUnlocked ≥ 5`, or a data-set lower threshold), a **Seal** is *noticed*, not built: the cut face is not rock, it is poured concrete with a wheel. That is one authored event and one journal line. Nothing else changes.

### 3.2 Opening it — five keys
An opening is a **Descent Plan** on the level's own ledger. It asks for:

| Key | What it is | Skippable? |
|---|---|---|
| **Reading** | The matching audit record has been discovered (read-only of the archive). | Yes — *Forced* |
| **Instrument** | A dosimeter device with a live error band below a per-level threshold. | Yes — *Forced* |
| **Air** | Shelter air purity above a floor (read-only) so the shaft does not pull foul air up. | No |
| **Crew** | N named survivors, off other work, for the duration. | No |
| **Time** | A number of days, per level. | No |
| (Gate) | Stability above the safe floor plus a margin (read-only). | No |

Crew and time are physics; reading and instrument are prudence. **A Forced opening is allowed** — and marks the level *Forced*, which draws its toll from the top of its range until it is Read. Haste has a price, not a lock.

### 3.3 The four levels (authored; original ids; echoing the audits, never mapping to the vault)

| Level (`deep_lv_*`) | Echoes | **Rule** | **Tell** | **Toll** | **Find** |
|---|---|---|---|---|---|
| `curtain` — *The Curtain* | L03 lead-bismuth shielding; the steelworks audit | A gap in the shielding leaks gamma; the plume **moves** with the shaft's pressure. | Dosimeter reading climbs *before* the pressure changes. | Lead/scrap to keep the curtain; dose per shift unless shielded. | Salvage table; a record; optionally a standing shielding benefit (DEC-TD-06). |
| `scrubbers` — *The Scrubbers* | L04 atmosphere scrubbing; the metro-station audit | The air below is better after a purge and **worse the more people are down**. | Differential-pressure reading. | Filter stock or power for the purge; stress per shift if skipped. | Held: cleaner air input while kept; a record. |
| `archive` — *The Archive* | L05 archive core; the deferred drill and the clock-drift/census pair | The level keeps **its own clock**; its readings diverge from the shelter's unless someone corrects them daily. | Two clocks that disagree. | One survivor "keeps the clock" every day held. | Documents; a **map hint** through the cartography owner (VERIFY). |
| `vats` — *The Vats* | L06 bio-synthesis; the agricultural-research audit | Nothing grows; everything **turns** (oxidation). Some of it is still good. | Peroxide-style index. | Weekly purge or disease risk. | A bounded food yield; the **Sleeper** (§8). |

Levels 01 and 02 are **not here**. The drawings had them; your builders did not. That absence is a line of prose, not a mechanic.

## 4. Reading and reconciliation

An instrument reading is `truth + noise`, where the noise is bounded by the device's **existing** error band (`errorBandMsv`, from calibration) and drawn from a seeded fork keyed `(day, levelId, device)`. The player sees:

- **Before Read:** the level's toll as a **range** ("0.4 – 2.1 mSv per shift").
- **After two agreeing readings** (two devices, or one device before and after calibration): the range collapses to a number and the level is **Read**.
- **If they disagree:** the level shows a **Disagreement** — a real, authored line ("the first says the curtain breathes at night; the second says it does not"). Reconciliation is calibrating, waiting a day, or sending a second device.

This gives calibration a purpose it has never had, makes a cheap second instrument valuable, and turns the archive's unused `mechanics` fields into the *truth values* the readings orbit.

## 5. Going down

A **Shift** is one day's descent by a small party (max 3, per level): a bounded exposure computed from the level's toll and the party's gear. Exposure is applied **only** through existing public calls — dose through `RadiationSystem`, stress through `SurvivorMentalHealthSystem.AddStress` (positive-only), air through the atmosphere input — and never as a wound. "Turn back" ends a shift early and forfeits the find.

There are no injuries by default. There is *tiredness*, *dose* and *what you saw*.

## 6. Holding, resealing, losing

- **Held:** the level stays open; its toll is paid each day; its standing benefit (if any) applies.
- **Resealed:** a one-time cost closes it; the next opening is faster because the shelter knows its own door.
- **Lost:** the toll goes unpaid for N days and the level **leaks up the shaft** — a small, authored consequence per level (stress and a shaft-room air input for the scrubbers; dose along the shaft for the curtain; a smell and a sickness risk for the vats; a clock nobody wound for the archive). Losing is a bad week, never a wipe: the level can always be resealed under duress.

## 7. What comes up

Finds are **ordinary stock, records and one hint**. Nothing here creates a new resource. The most interesting yield is *paper*: audit copies that say what the sibling shelter's builders were afraid of, fed to the existing journal and codex.

## 8. The Sleeper

At the bottom of `vats`, if two other levels are Read, there is **a pod**. The four deferred cryopod records describe exactly this kind of thing — identity corruption, a manual thaw that breached, a power cascade, a terminal protocol. This is **one authored choice**: leave it dark; wake it; end it. There is no guarantee of rescue and the text never dwells on method. Outcomes route through existing owners (a new arrival via the arrivals owner; a memorial via the memorial owner). It ships **dark** until signed (DEC-TD-07).

## 9. The whole shape

**Act I — the seal.** Depth 5 and a wheel. **Act II — reading and opening.** Calibrate, find the sibling's paper, choose which level and how carefully. **Act III — holding.** Tolls, shifts, leaks, the daily bookkeeping of a basement. **Act IV — the ledger of the levels.** Which are Held, Resealed, Lost; whether the shelter keeps what it found. One closing clause per outcome feeds the existing epilogue owner (VERIFY).

---

# PART III — HOW IT MEETS THE WORLD

## 10. Four stories

1. **The curtain breathes.** Two dosimeters, two answers. The cheap one is a hundred readings past calibration. Recalibrating takes a day the shelter does not have; the crew go down anyway; it is Forced. The night reading spikes. The story is *how long you wait to be sure*.
2. **The scrubbers and the crowd.** The air improves when three go down; it gets worse when five do. Someone counts. The mechanic is a headcount, and a person in the shelter is angry about it.
3. **Two clocks.** The archive's clock is wrong by a number that changes. One survivor is set to keep the time. She stops trusting the shelter's clock. Later, she is right about something else.
4. **The pod.** The vats yield food and rot in the same shift. At the end is a pod with a name on it that does not match the pod's own record. Nobody agrees what to do, and the game does not say.

## 11. Voice samples

- *Seal:* "It isn't rock. Somebody poured this and put a wheel on it. You don't put a wheel on something you never meant to open."
- *Range:* "Zero point four to two point one. That's not an answer, that's a weather report."
- *Disagreement:* "One says it breathes at night. One says it doesn't. One of them is old. That's all you know."
- *Reading:* "The sibling's paper says the curtain settled in the twenty-second year. Yours settled sooner. Somebody built yours worse."
- *Loss:* "You didn't lose the level. You lost the week."

## 12. Boundaries with other expansions

- **The Deep Works:** their galleries are horizontal, generated and shored; these levels are vertical, authored and read. Neither writes the other's state; `ExpansionTunnel` stays theirs. A drift *meeting* a level is an optional later event (VERIFY).
- **Surface anomalies (`AnomalyHazardSystem`):** unchanged; different ids, different noun, no coordinates below.
- **Abyssal and Black-Projects archives:** read-only (`IsDiscovered`). Deferred rows stay deferred unless signed row by row.
- **Faith and Schism:** the Listeners may read a level's Tell as a voice — a data hook, never shared state.
- **Radio Free Ashfall:** the deferred sonar "orphan ping" is a possible signal-from-below broadcast.
- **The Plague Year:** vats and scrubbers are natural read-only signals.
- **The Ration Wars:** vat food is ordinary stock in the existing ledger.
- **The Long Siege:** a Held level might one day be a last refuge (out of scope).
- **The Record Keepers:** documents arrive through the existing discovery call; no second archive.
- **Crews and Companions:** shifts become parties when crews exist.

## 13. Content plan

- **W1:** the Seal, Descent Plan, readings and the Curtain (engine + one level).
- **W2:** the Scrubbers.
- **W3:** the Archive (and the optional deferred-pair pass).
- **W4:** the Vats and the Sleeper.
- **W5 (optional):** row-by-row activation of the 13 deferred abyssal rows.
- Every wave is data plus one focused test set; nothing ships without a Rule, a Tell and a Toll.

## 14. Non-goals (restated)

No monsters or combat below; no procedural maps; no vault access mapping; no new resource; no health writes; no new save section; no new routed panel; no Unity.

## 15. Risks

| Risk | Mitigation |
|---|---|
| Canon read as "you opened the Architect's vault" | DEC-TD-01 frames the levels as the shelter's own, built to the same drawings |
| Range readings feel like a dodge | The range is always authored, always narrows with an action, and every action is named |
| Slot contest with The Deep Works | No construction project; own ledger (DEC-TD-02) |
| Sleeper handled carelessly | Ship dark; one choice; no method detail; sensitivity flagged (DEC-TD-07) |
| Tolls tuned into a chore | Per-level cap; a "hold cost" preview before committing; Resealed is always cheap |

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** The bible's central promise — *nothing down there is trying to hurt you; it
is just true, and the truth has a price per shift* — is the corpus ethic in one sentence: horror
priced per shift is measurable, bounded and recoverable, and that is exactly why the dread
survives. The player can always afford one more reading. One more reading is how understanding
sneaks up on you.

**What the expansion leaves lying around.**

> "Instrument card, Level 2: filter differential 1,850 Pa. The card is laminated; the numbers are older than the lamination."

> "Descent plan: five keys, one marked *Forced* in pencil. Pencil is the shelter's admission that it might change its mind."

> "A wheel on the seal — poured by hands that intended an opening. The wheel is the argument that the seal was a door all along."

**Scenes the player may piece together.**

> "Zero point four to two point one. That is not an answer; that is a weather report. The crew write the range on the wall and go down anyway."

> "The archive's clock is wrong by a number that changes. The keeper of the time stops trusting the shelter's clock, and later she is right about something else."

**Held silences (texture — the register below is unchanged).**

- What the sibling shelter's paper says in full. Audit copies are ordinary stock, records and one hint; the document behind the document is never transcribed and must not be.
- What the third hour of a shift feels like. Crew-days are the currency; the experience of paying them belongs to the prose surfaces and nowhere else.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**Who built the levels, and to whose drawings.** "Built to the same drawings" as the narrative-only
vault is as close to an answer as this document will get. Naming an architect converts dread into
exposition.

**Why the shaft caps at depth five.** A cap that is explained is a difficulty slider. A cap that is
not is a horizon.

**What the vats purge.** A weekly purge has a cadence. A cadence implies a product. The product is
never named and must never be named.

**Whether the Sleeper is asleep, absent, or waiting.** Three readings survive; one does not. This
document will not choose.

**Whether a level *wants* to be opened.** The mechanics model seals, rules and tolls. They model no
intent. Reads of intent belong to the player — and, occasionally, to *Faith and Schism*'s data
hooks. Never to this owner.
