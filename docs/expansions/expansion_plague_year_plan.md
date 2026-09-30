# ASHFALL — THE PLAGUE YEAR
### Outbreaks, quarantine politics and zoonotic vectors · Four strains, four seasons, one door

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/plague-year-2026-09-29.md`
**Family:** "The world moves without you" — `docs/expansions/expansion_world_moves_without_you_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. Fictional pathogens only — no real disease, outbreak, agency or public figure is depicted; no clinical instructions; no gore for its own sake. The game never tells the player how to feel.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"The sickness did not come to the shelter. It came to the region, and the shelter is only a
> place inside a region."*
>
> Every survival game models disease as a status effect on a person. This one models it as a
> **weather system over a map** — while a shelter sits inside it and decides how much of the world
> to let through the door.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

Disease in ASHFALL is currently a thing that happens **inside the walls**. It has an excellent anatomy — twenty diseases, four vectors, incubation and phase tables, immunity, treatment windows, isolation beds that eat water and medicine, strains that mutate under radiation, a cure project that takes ten days — and it starts in the shelter and ends in the shelter. No settlement in the region ever coughs. No caravan ever carries a fever. Nobody at the gate is ever sick in a way that costs the shelter a choice.

The Plague Year is about the **door**. It moves the disease out into the region and back in through the ways people actually arrive — a family at the gate, a wagon at the depot, a boat at the berth, a butchered animal on the table — and it makes the shelter's real question *political*: **who gets in, who gets told, who gets blamed, and who pays for the beds?**

There are four authored strains in the data today. This expansion gives them a calendar: **one strain per season**, a full turn of the seasonal cycle — a *Plague Year*. The player will not "beat" it. They will decide, week by week, what kind of shelter to be while it passes.

The **Gate Protocol** is the only dial: Open, Screen, Sealed. It is not a difficulty slider and it
is not a moral axis. It is a single persisted knob saying how much of *other people's emergency*
you are willing to make your own — and the answer is written on a change-day and cannot be taken
back quietly.

Notice the epistemology underneath the whole design. Spillover produces **Rumour only** — the model
can suspect and never confirm. Confirmation is a thing the *world* does, and the player learns about
it second-hand. You are always one bulletin behind, and *Heard* is wrong by exactly one before
*Told* corrects it. The world does not lie to you. It just gets to you late.

### 1.2 Pillars

1. **The outbreak is a rumour before it is a fact.** It has a first report, a confirmation, a peak and a wane, and the player sees each with a grade of certainty.
2. **The door is the game.** Every vector into the shelter is a decision the player can see coming.
3. **Quarantine is politics.** A sealed gate saves lives and costs standing; an open gate does the reverse. Nobody in the region agrees on which is right.
4. **Zoonotic means animal.** Spillover comes from reservoirs the game already simulates — packs, trapping, butchery.
5. **People are not numbers.** Every death runs through the existing survivor legacy; every ward has a cost in the shelter's stores.
6. **Ship dark.** No outbreak data present → the disease system behaves exactly as today.

### 1.3 Not this

Not an epidemiology sim. Not a second medical system. Not a lockdown-management minigame. No real pathogens or real-world outbreak analogues. No guarantee of a cure. No child-endangerment set-pieces (Year Two owns children; this plan only reads their immunity).

---

## 2. What the code and data actually say (audit)

| # | Finding | Evidence | Status |
|---|---|---|---|
| F1 | 20 diseases across 4 vectors (`water`, `air`, `blood`, `spore`), each with incubation, illness days, lethality, infectivity, spread interval/radius, treatments, immunity duration, phased contagion; 5 exposure sources. | `disease_catalog.json` (`schema_version` 3); `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`, `DiseaseCatalog.cs` | LIVE |
| F2 | Outbreak/immunity/save: per-disease `outbreak_active`, `outbreaks_total`, `outbreaks_prevented`, deaths, infections; immunities; seed and rng position; save section `disease` (`disease_save.json`), state version 2. | `DiseaseSystem.cs` L169–235; `Save/SaveSectionRegistry.cs` L108, L479 | LIVE |
| F3 | **Four authored strains**: `pathogen_ash_fever` (of `zoonotic_flu`; lethality 0.22, infectivity 0.6, mutation 0.04/day), `pathogen_red_lung` (of `condemned_air_cough`), `pathogen_frost_rot` (of `cholera`), `pathogen_glass_cough` (of `dry_bunker_hiss`, mutation 0). | `pathogens.json`; `Disease/PathogenStrainSystem.cs`; cure projects (default 10 days) | LIVE |
| F4 | **The arrival pattern already exists**: an outbreak *source* is a host adapter implementing `IDiseaseOutbreakSource` with a contracted list of `AuthoredDiseaseIds`; `TriggerOutbreak` rejects diseases outside that contract. Sources today: infestations, flooding/excavation. | `Disease/IDiseaseOutbreakSource.cs`; `src/Host/DiseaseOutbreakHostAdapter.cs`; `src/Main.EcologicalInfestations.cs` L167–174 | LIVE |
| F5 | Isolation: beds, a daily care burden (1 clean water, 1 canned food, 1 medical kit or bandage per isolated patient), quality 0.10–1.0, containment bonus from research. | `Disease/DiseaseQuarantineCoordinator.cs` `TickDaily`; `ContainmentCapability.cs` | LIVE |
| F6 | Spread is **intra-shelter** (`TickDaily(day, candidates)`); nothing models an outbreak in a region, settlement, harbour or caravan. | `DiseaseSystem.cs` L737; grep `Settlements/`, `Economy/`, `Maritime/` | **GAP (the central one)** |
| F7 | Zoonotic sources: `wildlife_butchery` (0.30), `autopsy_pathogen` (0.25), `micro_hazard_contamination` (1.0); trapping catalog entries carry `diseaseId`, `diseaseRisk`, `contaminationRisk` (e.g., rad dog 0.3/0.2). | `disease_catalog.json` `exposure_sources`; `wildlife_trapping_catalog.json` | LIVE |
| F8 | Reservoir density is simulated: wildlife pack populations, rabid packs, ecology layer, global population ratio. | `Assets/Ashfall.Core/World/WildlifeEcosystemSystem.cs`; `EvolvingWorldDayOwner` | LIVE (VERIFY: no disease read of it) |
| F9 | Embargo rules are **weather-triggered** (14 rules: `weather_kind`, regions or `*`, categories, price multiplier, `caravan_blocked`, `route_slow_permille`, `decay_days`). No outbreak-triggered kind. | `trade_embargoes.json`; `Economy/TradeEmbargoSystem.cs` | GAP |
| F10 | Diagnosis knowledge and microfluidic diagnostics exist (`is_diagnosed`, tells, timing clues). | `Medical/DiagnosisKnowledgeStore.cs`; `MicrofluidicDiagnosticEngine.cs`; `disease_catalog.json` `tell`, `tell_secondary`, `timing_clue` | LIVE |
| F11 | Emotional contagion (grief, etc.) is a separate live system: 6 events with bond/proximity multipliers. | `contagion_events.json` | LIVE |
| F12 | Post-illness psychological care exists. | `Assets/Ashfall.Core/Sanatorium/PsychologicalSanatoriumSystem.cs`; `SickListSystem.cs` | LIVE |
| F13 | Selftests: `--disease-selftest`, `--disease-expansion-selftest`. | `src/Host/HostCli*.cs` | LIVE (VERIFY args) |
| F14 | Who is at the gate: not yet located. | — | **VERIFY (P0)** |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

- **The premise "outbreaks are regional" vs the code (F6).** Systems win: the shelter's `DiseaseSystem` stays the single disease authority; the regional layer is a **watch** — a small ledger that *feeds* it through the existing `IDiseaseOutbreakSource` contract, never a second disease simulation.
- **Embargo vs weather (F9).** The embargo authority is weather-only by design. Adding an outbreak trigger is an **extension of the same catalog** (an additive trigger field defaulting to weather), not a new quarantine system (DEC-PY-05).

---

# PART II — THE STORY

## 3. The Plague Year

### 3.1 Four strains, four seasons

The four authored strains already have a natural home in the four migration seasons the world already runs.

| Season | Strain (existing) | Vector | The shape of it |
|---|---|---|---|
| **Deep winter** | **Red Lung** (of Condemned-Air Cough) | air | people crowd; vents are opened at shift change; it is a *closed-room* sickness |
| **Thaw** | **Frost Rot** (of Cholera) | water | roads open, water pools; it follows the meltwater and the wagons |
| **Dry heat** | **Glass Cough** (of Dry Bunker Hiss) | air/dust | low lethality, high spread; the *tiring* one; it wears settlements down |
| **Ash winds** | **Ash Fever** (of Zoonotic Flu) | air, animal | the one that jumps from animals; the fastest, the most political |

*The Year is not one disease.* It is four, one after the other, and the player learns the region's habits each time.

### 3.2 An outbreak, start to finish

An outbreak is a **watch entry** in the region where it starts (canonical region ids from *The Living Region*): `{ strain, region, cause, stage, since }`.

| Stage | What the world does | What the player knows |
|---|---|---|
| **Rumour** | Nothing measurable. A cough at a ferry; a butcher's warning. | Heard: "Some say the Drown is coughing." |
| **Confirmed** | Local case count rises; the settlement's Health pillar fails (Living Region). | Told: a trader's account with numbers. |
| **Spreading** | Neighbouring regions become exposed via roads, harbours, gates. | Told/Seen; the Board shows the front. |
| **Waning** | Immunity accumulates; case rate falls; fear does not. | Seen. |
| **Ended** | Immunity fades on its authored clock (28 days for zoonotic flu). | The count. |

### 3.3 The four ways it reaches the door

Each is a *vector* — a small host adapter implementing the existing `IDiseaseOutbreakSource` contract with **an authored list of diseases it may seed** (F4). None writes disease state directly.

1. **The gate** — petitions from *The Living Region*: a family of six, one of whom has the `tell` (F10) the player may or may not know how to read.
2. **The wagon** — freight arrivals from *The Long Line: Freight*: a run that arrives with a sick driver; a depot that closes its gates to the shelter's own wagon.
3. **The berth** — a boat at a harbour flying a yellow flag (*The Drowned Coast*): the crew asks for a berth, a lamp and no questions.
4. **The table** — a butchered animal, an autopsy for a sample, a trap line that caught a dog that should not have been caught (F7). The shelter brings this one in **on purpose**.

### 3.4 The Gate Protocol

The player holds one dial, visible in the existing gate/door surface:

| Setting | What it does | What it costs |
|---|---|---|
| **Open** | Petitions proceed as today. | Every sick arrival is a risk the shelter accepts. |
| **Screen** | Petitioners are checked (a diagnosis action; a look at the `tell`); suspect cases are offered isolation. | Isolation bed capacity; the daily care burden (F5); time. |
| **Sealed** | No petitions; freight dwell blocked. | Standing with those turned away; a wave that goes elsewhere; the shelter's own supply if it depends on the road. |

The Protocol is one field with a change-day, persisted; nothing else.

### 3.5 Quarantine politics

The region does not agree, and it does not have to.

| Faction/settlement | Position | What it does |
|---|---|---|
| **The Compact** | Cordon everything. | Enforces route closures; offers "protection" that becomes control. |
| **The Scale** | Sell the cure, or the cordon. | Medical prices climb; a certificate of health can be bought. |
| **The Black Flotilla** | No inspection; a yellow flag is a courtesy, not an order. | Refuses boarding; harbours stay open — and so does the risk. |
| **St. Nicholas** | Refuses the account. | Denial keeps the outbreak alive; a quiet horror. |
| **Pilgrim Hearth** | Takes the sick in. | The first to run out of everything; the last to abandon anyone. |

**Cordons** are embargo rules with an outbreak trigger (additive, DEC-PY-05): they block caravans, slow routes, and add a medical price shock that decays. The same authority the weather already uses.

### 3.6 The count

Every disease death is a survivor death recorded through the existing legacy path — **plus a line in the Count**: a season-by-season tally of who was lost, where, and *why they were let in or kept out*. The Count does not comment. It is read at the end of each season.

### 3.7 Cure and knowledge

The four strains have cure projects (10 days by default). The Plague Year adds one *moral* input: **samples**. A cure project starts faster with a sample from an index case — obtained by autopsy (an existing exposure source at 0.25) or from a survivor who agreed. The game does not tell the player it is right or wrong.

Research (containment capability) improves isolation efficacy, care efficiency and monitoring — already live.

### 3.8 Aftermath

- **Grief** rides the existing contagion system (funeral grief lingers; bond multiplier 1.4).
- **The sanatorium** and sick list carry the survivors of the sickness through the existing care path.
- **The Board** (Living Region) records which settlements the Year emptied.

## 4. Spillover: the zoonotic engine

Each region has *reservoir pressure* (wildlife density × season × rabid share) and *contact pressure* (trapping, butchery, scavenging of animal remains). Once a day, a **seeded** hazard converts pressure into a **Rumour** entry (never straight into Confirmed): deterministic under a `CampaignStreamIds` fork; no `System.Random`. Hunting hard raises contact pressure; sparing packs raises reservoir density instead. The game lets the player feel the trade without stating it.

## 5. The four movements

**I — The Cough at the Ferry (deep winter).** A rumour. The shelter has a choice about whether to believe it. *Ends on:* the first isolated patient.

**II — The Meltwater (thaw).** Roads open and lie. Three vectors converge. *Ends on:* a gate decision that costs the shelter a friend.

**III — The Long Ward (dry heat).** Low lethality, high fatigue. The beds are full and the stores thin. *Ends on:* the count for the season.

**IV — Ash Fever (ash winds).** The animal one. A dog on a trap line; a sample. *Ends on:* the cure project — and what it took to start it.

Coda: the Count, and what the Year did to the region (Living Region Board).

## 6. Voice samples

- *Rumour, Heard:* "They say the ferryman is coughing. They say a lot of things at the ferry."
- *Trader, Told:* "Wouldn't come in. Stood at the fence and told me the price of everything he had left."
- *Gate, Seen:* "The younger one has the tell. The mother says it is only dust. The mother is not lying, exactly."
- *Ward log:* "Bed two: water, food, kit. Bed three: water, food. Bed four: we ran out of kit. We did not say so to bed four."
- *The Count:* "Season of thaw. Eleven at the gate turned away. Four of them are on the list. Two were ours."

## 7. Content plan

- **W1 — Season packets:** 4 strains × 5 stages × 3 registers (60 radio/trader lines).
- **W2 — Gate petitions with tells:** 24 (each keyed to an existing `tell`/`timing_clue`).
- **W3 — Politics:** 5 faction/settlement positions × 4 beats (20).
- **W4 — Ward log lines:** 30.
- **W5 — The Count:** season summaries and per-death "why" lines (40).

## 8. Non-goals (restated)

No new pathogen in v1. No second disease authority. No new save section (nested in `disease`, DEC-PY-02). No new routed panel. No clinical instruction content. No real-world analogues.

## 9. Risks

- **Regional layer becomes a second disease sim.** *Bound:* the watch is data only; all infection still goes through `DiseaseSystem.TriggerOutbreak` under the existing contract.
- **The Protocol becomes a chore.** *Bound:* one dial, one change-day; effects are read at existing decision points.
- **Tone.** *Bound:* the Count does not editorialise; no graphic depiction.
- **Difficulty spikes.** *Bound:* outbreak severity respects the existing difficulty authority (VERIFY hook in P0).

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** The plan models disease as weather over a map, and the Gate Protocol is the
only dial — which changes nothing about the sickness, only how much of *other people's emergency*
you are willing to make your own. The answer is written on a change-day and cannot be taken back
quietly. That is the most honest difficulty knob ever designed: it grades the player, not the
plague.

**What the expansion leaves lying around.**

> "Gate card, Protocol — Screen. Laminated; somebody expected to change it often. Somebody did not."

> "Ward sheet, one line: isolation offered, accepted. Not a moral document. It has been read as one."

> "Count line: one. The Count is one line per death and the ink is the same on every line."

**Scenes the player may piece together.**

> "A rumour arrives before the bulletin. The shelter acts on the rumour. The correction arrives after the acting."

> "The cordon notice names no region. The gate changes anyway."

**Held silences (texture — the register below is unchanged).**

- What the fifth season would have carried. Four strains map to four seasons; the fifth season is not asked about and must not be.
- What the regions that ended their outbreaks look like now. 'Emptied by the Year' reaches the Board and stops at the Board.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What the index case was.** The spillover model computes reservoir pressure × contact pressure. It
names no animal, no place, no patient zero. Naming a culprit converts a hazard into a villain.

**Whether *Screen* is compassion or arithmetic.** It consumes the existing daily care burden. This
expansion declines to weigh that.

**Why there is one strain per season.** Four strains map to four seasons. The fit is observed and
never explained.

**Who wrote the outbreak-source contract.** A contract that rejects anything outside itself implies a
drafter. This expansion declines to name one.

**What happened to the regions that ended their outbreak.** "Emptied by the Year" reaches the Board.
*Emptied* is doing work the expansion refuses to define.

**Whether the Count includes the people who left.** Every disease death has a Count line. The Count
is not a census and never claims to be.
