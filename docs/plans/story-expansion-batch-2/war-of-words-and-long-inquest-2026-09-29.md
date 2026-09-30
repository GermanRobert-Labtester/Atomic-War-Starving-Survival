# Feature / Task Plan: Power, Paper and Place II — The War of Words (propaganda and rumours move regions) & The Long Inquest (a slow mystery about what happened before)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — evidence pass 1 complete (2026-09-29, §2b) — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). **Treat as a first full draft to be expanded and finalised.** Open points are in §24 (Expansion backlog) and §25 (Open Mysteries and Deliberate Silence).

> **Subjects covered (2 of the 16 in this batch):**
> 27. **The War of Words** — propaganda and rumours move regions. (Prefix `WW`.)
> 28. **The Long Inquest** — a slow mystery about what happened before. (Prefix `LI`.)
>
> **Companions that already exist and are extended, not replaced:** `RumorSystem` and its six hubs, `PsyOpsSystem` and its eight campaigns, `FactionRadioEngine` and the radio owners, the market-shock rumour lines, the 79-item `world_history.json` with its discovery keys, the pre-war and lore archives, the Verdict witnesses, the bibles `docs/expansions/expansion_08_the_verdict_plan.md` and `docs/expansions/expansion_03_nobodys_charter_plan.md` (Nobody's Charter), and the design plans `.ai/plans/radio-free-ashfall-2026-09-29.md`, `living-region-2026-09-29.md`, `record-keepers-2026-09-29.md`, `quiet-war-2026-09-29.md`. Where this plan disagrees with source on *facts*, source wins (Rule 7).
>
> **Binding companion:** `.ai/plans/OPEN_MYSTERY_INDEX_2026-09-29.md`. That index records which silences are **deliberate** and which are **locked by signed decision**. The Long Inquest is the one feature that *asks* about the Before, so it is bound by that index more tightly than any other plan: it may return **grades and silences**, never **a biography and never a "why"** (§16, §25).
>
> **Sister plans (read-only cross-references; all hooks ship dark):** `paper-and-power-and-the-treaty-table-2026-09-29.md` (papers, forged exhibits, a treaty as a story seed), `convoy-wars-and-inside-a-house-2026-09-29.md` (carriers move stories), `iron-road-and-siege-year-2026-09-29.md` (rail and siege events as story seeds), `evenings-and-memory-work-2026-09-29.md` (a Report may be read at Founding Day).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, data, ledger or other plan. Paths are *proposed*; `INT` marks integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c**, **10b**, **14b** and **25** carry story texture. Sample lines are content candidates for JSON rows, never code. No authority, path, decision, acceptance criterion or verification step is changed by any prose section.

---

## 0. Prologue — What Is Said, and What Is Kept

> *"A region does not fall when it is beaten. It falls when it stops believing the road is open.
> And it does not remember the war. It remembers what it was told about the war, and by whom."*

The world after the fall is made of two things: **what happened** and **what people say happened**. The game already has good machinery for the second. There is a rumour network with hubs that have credibility and bias; a set of eight propaganda campaigns that pressure factions; a radio dial full of stations; market shocks that generate their own rumour lines. What it does not yet have is the **effect of a story on a place**: the sense that the crossroads *believes* something, that the belief has a strength, that it spread by three different roads, and that it can be undone by a better-told fact. And for the first, the game has seventy-nine authored records of the Before scattered through bunkers, journals, tapes and testimonies, but nothing that lets a player *sit down with them* and ask what they add up to.

These two subjects are paired because they are the same question turned around. **The War of Words** asks: *what can be made to be believed, and what does belief do?* **The Long Inquest** asks: *what can be shown to be so, and what will stay unknown?* One is about the power of a story. The other is about the discipline of a finding. A shelter that plays both learns the difference, slowly, the way people do.

**The binding tone rule.** Cold, exhausted, human, restrained; specificity over adjectives; the game never tells the player how to feel (corpus tone lock). Propaganda here is not a villain's tool and not a hero's; it is a *rate* — how many hubs, how many days, at what credibility. The Inquest is not a detective game. There is no moment where a face is named and music plays. There is a desk, a lamp, a survivor with a list, and a sentence that reads *Attested*.

**Tone & register.**

- **The War of Words is written in the voice of the hub.** The caravanserai bench, the relay operator's log, the pamphlet with a thumbprint on the corner. People repeat what they heard and add one small thing.
- **The Long Inquest is written in the voice of the finding.** *Attested. Two sources, one of them a tape.* Then the line that follows every finding in this plan: *Not established here.*

**What the two share.** *Provenance.* A story is only as strong as its route. A finding is only as strong as its exhibits. Neither part ever asks the player to believe a number; both ask them to weigh where a thing came from.

**The second layer.** The world after the fall is made of two things — what happened and what
people say happened — and the plan's quiet thesis is that confusing them is how regions fall. A
rumour is a story with a *route*; a finding is a fact with a *chain of custody*. The War of Words
measures belief and declines to report truth; the Long Inquest measures proof and declines to
report belief; and the only seam between the two instruments is the one place they are allowed to
touch. A shelter that plays both learns, slowly, that being believed and being right are different
skills — and that only one of them can be printed under a lamp. And under the lamp the sentence stops being believed
and starts being held.

---

## 1. Goal & Outcome

### 1.1 The War of Words (WW)

> *Design intent: the player should be able to say, of a settlement they have never visited,
> "they believe we have water" — and know exactly which three roads that belief travelled.*

- **Goal:** Add a **Narrative Field** over the existing rumour, propaganda and radio owners: an authored catalog of **Stories** (short claims that can spread), a derived per-region **Grip** (how strongly a region holds a story), a stored ledger of the player's **Tellings** (the human act of planting, broadcasting, printing or correcting), a seeded **Exposure** rule for false tellings, and a small table of **Consequences** that reach regions only through seams that already exist (Living Region, faction standing, market shock).
- **Outcome (observable):**
  1. A **Field surface** (an existing panel; chosen at P0) lists, for each region, the stories present, their **Grip band** (Whisper / Talk / Belief / Certainty) and the **three channels** that carry them (Hubs, Air, Hand) in words.
  2. The player can **Tell** (plant a rumour at a hub), **Broadcast** (through the shelter's radio, or a PsyOps campaign), **Print and Carry** (a pamphlet on real paper stock with a carrier), and **Correct** (counter a story with a better-supported one).
  3. **Grip** is recomputed on read from real records: rumours that reached hubs in the region, active broadcasts, pamphlet drops — each decaying with age — minus half the strongest opposing story.
  4. A story that reaches **Belief** in a region fires its **consequence rows** exactly once, through the owner's own seam.
  5. A **false or half-true** telling can be **exposed** (seeded, weekly); exposure zeroes its contribution from that day and costs Voice Trust (if the radio plan exists) and standing where the row says so.
  6. Save/load round-trips; a legacy save loads with no tellings and no taken-stories, and every rumour, PsyOps, radio and market behaviour equals today's.
- **Non-Goals (WW):** no second rumour, propaganda or radio system; no change to `Truthfulness`, decay, hub credibility or PsyOps arithmetic; no real-world propaganda, parties, countries or persuasion technique (campaign notes in the catalog already say *"abstract faction-level influence values, not persuasion guidance"* — this plan keeps that line); no player-authored free text; no new routed panel; no Unity.
- **"Done" (WW):** §19 WW acceptance passes via `bin/run-scoped-tests`; existing rumour, PsyOps and radio tests unchanged; handoff lists untouched shared paths.

### 1.2 The Long Inquest (LI)

> *Design intent: by day 600 the player should hold a case file they built themselves — and be
> able to say exactly which sentences in it the world has earned, and which it never will.*

- **Goal:** Give the seventy-nine authored records of the Before an **assembler**: a derived **Case File** of **Exhibits** (records the player has actually discovered), twelve authored **Questions** each with a ladder of **Findings** (a fact about *what*, never *why*), a stored **Line of Inquiry** per question the player chooses to pursue with an assigned **Investigator**, **Sittings** (survivor-days at the desk) that slowly raise a finding's grade, **Leads** (a sitting on an under-evidenced finding names the *place* of the next exhibit), **Statements** (a finding said aloud, once), a **Report** (a chronicle artifact), and — always — a printed **Silence**.
- **Outcome (observable):**
  1. The **Inquest surface** (an existing archive/journal panel; chosen at P0) shows the twelve Questions, the exhibits held for each (by title), each finding's **grade** (Rumoured / Attested / Corroborated / Established) and the Silence at the foot of every question.
  2. The player opens a **Line of Inquiry** and assigns an **Investigator**; each day the investigator spends at the desk is a **Sitting** spent on one finding.
  3. A finding's grade is the **lower** of what its exhibits support and what its sittings have earned. Grades are computed on read; nothing about the case file is stored except the *player's acts*.
  4. A sitting spent on a finding that still lacks exhibits produces a **Lead**: *"The desk suggests the civil defence bunker."* It is drawn from the existing `discovery_location_id` of the next undiscovered exhibit. Leads drive expeditions; nothing else does.
  5. A **Statement** and a **Report** are chronicle facts, stored once. A Report always prints the twelve Silences.
  6. Save/load round-trips; a legacy save loads with no lines and no reports, and every discovery, journal and archive behaviour equals today's.
- **Non-Goals (LI):** no solving; no revelation moment; no "who" and no "why" (§16); no change to any `world_history` record, its trigger or its discovery; no change to the locked silences in the Open Mystery Index; no new exhibit store (exhibits are the existing discoveries); no player-authored theories in free text; no new routed panel; no Unity.
- **"Done" (LI):** §19 LI acceptance passes; existing journal, archive, discovery and Verdict tests unchanged; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

**A story can contest a finding; a finding can seed a true story.** One Narrative Field and one Case File, joined in two places and no more: (1) a **Contested** flag on a finding, derived when a story that claims otherwise holds **Belief** in the shelter's own region (display only); (2) a **Statement to a region** creates a Telling with `Honesty = True` (Corroborated or better) or `Half` (Attested) — it goes through the same Telling ledger as any other. Neither part keeps a copy of the other's facts (Rule 5).

---

## 1b. Texture, Mystery & Voice

**Words: the crossroads has a bench.**

The Field never says "the region is 62% convinced." It says: *"At the crossroads they are saying the shelter has water to spare. It came by the caravanserai and by a pamphlet. Nobody remembers where the pamphlet came from."* The band word (*Talk*, *Belief*) sits under the sentence in small type. The number stays inside the derivation.

**A telling is a small act.**

Planting a rumour is a survivor walking to a bench and saying one sentence to the right person and then leaving. The game never shows the person. It shows the *next day's line*: *"A woman at the caravanserai repeated it to a driver. The driver said he had heard it already."* That second line — *he had heard it already* — is the one the player is playing for.

**The exposure of a lie is ordinary.**

A false telling is not caught by a detective. It is caught because a driver went to look. *"The driver went to look. He came back and did not say anything, and that is how it got round."*

**The Inquest: a desk and a lamp.**

The Inquest never shows a corkboard. It shows a list of what has been found, and next to each item how sure the desk is, in one word. *Rumoured. Attested. Corroborated. Established.* The fourth word is rare and is never shown as a reward; it is shown as a *plain statement*: **Established: the exchange lasted forty-five minutes.** And underneath, in the same type, every time: **Not established here: what was aimed at what.**

**Silence is printed, not hidden.**

The Silence line under a question is not a teaser. It is a fact about the case: **this is the part the records do not reach.** The game prints it so that nobody — not the player, not a later agent — mistakes it for something still to be dug up. (The corpus rule, Open Mystery Index §0: *a question that is intentionally unanswered is not a bug, a TODO, or deferred work.*)

**What the player is never told.**

- **Who opened the door** (`The Open Door`, hour zero). Findings establish *that* it was open; the Silence says who is not established here.
- **What the Vessel was** (`The Vessel's Cell`). Findings establish that a cell existed; the Silence holds the rest.
- **What was read** (`The Reading`). The finding is that a reading took place; what was read is not established here.
- **Who did not sign** (`The Ledger Nobody Signed`). The finding is the ledger; not the missing hand.
- **Whether any story in the Field is true, unless a finding says so.** The Field reports belief, never truth.

**Voice — sample fragments (content candidates for `story_lines.json`, `inquest_lines.json`).**

> "At the crossroads they are saying the shelter has water to spare. It came by the caravanserai and by a pamphlet." — Field (WW)

> "A woman repeated it to a driver. The driver said he had heard it already." — telling, day after (WW)

> "The driver went to look. He came back and did not say anything, and that is how it got round." — exposure (WW)

> "Attested. Two sources, one of them a tape." — finding line (LI)

> "Established: the exchange lasted forty-five minutes. Not established here: what was aimed at what." — finding with Silence (LI)

> "The desk suggests the civil defence bunker. It does not say why. It does not know." — lead (LI)

**Design texture beats.**

- **Belief is a band, truth is a finding (WW / LI).** The Field never says a story is true; the Case File never says a story is believed.
- **The second line is the game (WW).** Each telling's next-day line reveals whether it took.
- **The desk suggests, it does not know (LI).** Leads are places, not answers.
- **Every finding carries its Silence (LI).** The unanswered part is printed with the answered part, in the same type.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. §10b/
§14b remain the texture sections; this section is the **objects** those sections leave behind.)*

**What the words leave lying around.**

> "Pamphlet, one corner thumbprinted. Nobody remembers where it came from, which is exactly how it travelled so far."

> "Bench at the caravanserai: the sentence arrived with a driver and left with two people."

> "Relay log, day 4: the same sentence, three hubs, two roads. The third road is the one we did not write down."

**What the inquest leaves lying around.**

> "Exhibit tag: tape, attested. The tag records that it exists and refuses to summarise it."

> "Lamp, desk, list. The furniture of a discipline that has no verdicts, only findings."

> "Silence line, printed under the finding, same type. It is not a teaser. It is the case's honesty."

**Scenes the player may piece together.**

> "The driver went to look. He came back and did not say anything, and that is how it got round."

> "Attested. Two sources, one of them a tape. Not established here: what was aimed at what."

**Held silences (texture, not register rows).**

- Who set the twelve questions. The questions are authored; who chose them, and in what order, is *not established here* and must not be. Texture only.
- Whether the Field ever repeats a finding. Belief and truth are separate instruments; §17 is the one seam where they touch, and beyond it neither may quote the other.

**Fourth pass — provenance (texture only; §25 register unchanged).**

*(Polish pass, non-contractual: wording and texture only — no authority, no claimed path, no
decision, no acceptance criterion, no verification step. §25's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions.)*

**The shape of the polish.** Everything in this plan travels. A sentence arrives with a driver and
leaves with two people; an exhibit arrives with a tag and refuses to summarise itself. The prose
should treat every artefact as a parcel with a route — the route is the meaning — and keep the two
instruments separate: the hub reports belief, the desk reports custody, and neither may quote the
other past the seam.

**What the words leave lying around.**

> "Pamphlet, second printing. The thumbprint moved corners. The sentence did not move."

> "Bench at the caravanserai: the sentence left with two people and arrived with a shape."

**What the inquest leaves lying around.**

> "Finding, page two: 'attested, two sources.' Page three is the silence line, set in the same type
> as the finding — which is the coldest thing this plan does."

**Held silences (texture, not register rows).**

- What the numbering of the twelve questions is for. The order is authored and *not established
  here* (held silence above); the numbering does quiet work and must not be interpreted. Texture
  only.
- What the third road carries. The relay log records two roads and declines the third (§1c); it is
  not hidden evidence, it is a route nobody took notes on, and it must stay that.

---

## 2. Evidence table (verified 2026-09-29 against the live worktree; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `RumorSystem` owns `WastelandRumor` (id, origin location and day, subject type/id, headline, description, `Truthfulness` 0..1, `DecayRate` default 0.05, `PropagationSpeed`, `IsIntercepted`, `ReachedHubIds`), `InformationHub` (id, name, location, `Credibility`, `Bias`), `RegisterHub`, `GenerateRumor`, `PropagateRumorToHub`, `InterceptRumor`, `TickDay`, and a briefing report; state `RumorNetworkState`; host `RumorNetworkHostSession`; panel `RumorBoardPanel`; save section `wasteland_rumors`. | `InformationFlow/RumorSystem.cs`; `src/Main.RumorNetwork.cs` | LIVE |
| E2 | `rumor_hubs.json` holds **6 hubs**: Crossroads Caravanserai (credibility 0.85, trader, capacity 6), Rustwater Scavenger Canteen (0.70, scavenger, 4), High Ridge Relay Station 7 (0.95, neutral, 8), Iron Coalition Checkpoint Bravo (0.80, militant, 5), Subterranean Blind Exchange (0.65, underground, 5), Ashen Plateau Hermitage (0.60, neutral, 3). Hubs have a `location_id` but **no region**. | `Data/rumor_hubs.json` | LIVE (finding) |
| E3 | `PsyOpsSystem` owns 8 campaigns (`propaganda_campaigns.json`): each has `target_faction_id`, a **theme label** (Hope, DefectionAppeal, AidPromise, Fear, Unity, CounterRumor), `base_reach`, `power_demand_watts`, `duration_days`, `receptiveness`, `loyalty_pressure_per_day`, `countered_by`, and the note *"abstract faction-level influence values, not persuasion guidance"*; API `StartCampaign`, `EffectiveReach`, `TickCampaigns`, `StartJamming`, `StartCounterPropaganda`, `PressureOn(factionId)`; section `psyops`. | `Radio/PsyOpsSystem.cs`, `PsyOpsCatalog.cs`; `Data/propaganda_campaigns.json` | LIVE |
| E4 | `FactionRadioEngine` maps factions to frequencies and callsigns and serves broadcasts/events by frequency and day. The player-station plan (call sign, audience ledger, Voice Trust, broadcast Signature, mailbag) is a **plan, not source**. | `Radio/FactionRadioEngine.cs`; `.ai/plans/radio-free-ashfall-2026-09-29.md` | LIVE / PLAN |
| E5 | `EconomyMarketRumorRules` generates rumour *lines* when a market shock starts or expires — the market already speaks in rumours, but does not accept them as input. | `Economy/EconomyMarketRumorRules.cs` | LIVE |
| E6 | **Nothing reads a region's story.** No owner holds "what a region believes"; rumours have hubs but no region; PsyOps has faction pressure but no region; the Living Region plan reads war tension and settlement condition, not narrative. | grep across Core; `living-region-2026-09-29.md` | LIVE (finding) |
| E7 | `Truthfulness` set at generation, scaled by hub credibility on spread, decayed daily; no consumer refutes or penalises a false rumour. | `InformationFlow/RumorSystem.cs` L149–230 | **RESOLVED (§2b)** |
| E8 | `world_history.json` holds **79 records** in five eras (pre_exchange 20, hour_zero 13, black_sky 14, post_exchange 11, ashfall 21); each has `title`, `body`, `year_month`, `discovery_location_id`, `discovery_trigger`, `knowledge_key`. Triggers: `location_explore` 37, `survivor_dialogue` 11, `journal` 9, `inspection` 5, `ending_reached` 5, `radio_intercept` 3, and single-use event triggers. | `Data/world_history.json` | LIVE |
| E9 | The game **does not assemble** these records: each is discovered on its own trigger and read on its own. No board, question or synthesis exists. | grep; `journal` usage | LIVE (finding) |
| E10 | `journal.Knowledge.Has(knowledgeKey)` is the public read; `BureaucraticDocumentDiscoverySystem.IsDiscovered` uses it. | `Narrative/BureaucraticDocumentCatalog.cs` L455–473 | **RESOLVED (§2b)** |
| E11 | Other Before-records exist: `prewar_archives.json` (6), `lore_archives.json` (4), `echoes.json` (23), `deep_lore_locations.json` (25), `faction_war_journal.json` (26); black-project records carry a `BlackProjectsTruthClass` (InstrumentTelemetry, VehicleBlackbox, ClassifiedDirective, ComplianceAudit, Allegation); bureaucratic documents carry a `BureaucraticDocumentTruthClass` (HistoricalCanonicalRecord, ContemporaneousAuthoredRecord, TemplateCompatibleRecord, FlavorOnlyArtifact, UnsafeUnresolved). | `Data/*`; `Narrative/BlackProjectsArchiveSystem.cs`, `BureaucraticDocumentCatalog.cs` | LIVE |
| E12 | The **Verdict** already runs a three-witness arc: Witness 1 (checkpoint conscript, day 241, `history_checkpoint_conscripts_confession`), Witness 2 (quartermaster's paperwork, day 243, `history_quartermasters_paperwork`), Witness 3 (intercepted cipher, day 261, `history_the_intercepted_cipher`), and `The Ledger Nobody Signed` (day 262, `history_the_ledger_nobody_signed`). The Inquest **reads** these as exhibits and never competes with the arc. | `Data/world_history.json`; `docs/expansions/expansion_08_the_verdict_plan.md` | LIVE |
| E13 | `CipherQuestChainEngine` (`RecordBroadcastHeard`, `RecordKeyAcquired`, `EvaluateDecode`, `MarkResolved`) already ties broadcasts and keys to decodes. | `Narrative/CipherQuestChainEngine.cs` | LIVE |
| E14 | Five `world_history` records use the trigger `ending_reached`: they are **endgame-only** and must never be exhibits. | `Data/world_history.json` | LIVE |
| E15 | The Open Mystery Index: deliberate silences (do not fill), locked silences by signed decision (SK-OM-1 Olympus; Y2-OM-2 no Chapter Three; and others), and the rule *"a system may return a state. It may not return a biography."* | `.ai/plans/OPEN_MYSTERY_INDEX_2026-09-29.md` §2 | LIVE (binding) |
| E16 | Record custody, condition, Gaps and a Reading/Dispute exist as a **plan**, not source. | `.ai/plans/record-keepers-2026-09-29.md` | PLAN |
| E17 | Desk queues transcriptions of evidence (`QueueTranscription(evidenceId, archivistId, inkId)`), save section `archive_desk`; a Sitting is not a transcription. | `ArchiveDeskSystem.cs` L99–186 | **RESOLVED (§2b)** |
| E18 | `GenerateRumor(originLocationId, subjectType, subjectId, headline, description, truthfulness, currentDay)` — the caller chooses all of them. | `InformationFlow/RumorSystem.cs` L149 | **RESOLVED (§2b)** |
| E19 | Living Region's `IRegionNarrativeReader` hook does not exist yet (plan only); Living Region region ids are unverified against the hub locations in E2. | `living-region-2026-09-29.md` | **VERIFY (P0)** |
| E20 | `MarketSystem.ApplyShock(categoryId, isShortage, severityBp, startDay, durationDays, sourceId)` is a request seam (severity clamped, duration ≤ 60). | `Economy/MarketSystem.cs` L489 | **RESOLVED (§2b)** |
| E21 | `MemoryRecord.Clarity` exists; no reading or literacy skill exists; archivist is an assignment. | `Cognition/MemoryDecaySystem.cs` L43 | **RESOLVED (§2b)** |
| E22 | Difficulty scalars are read at owner sites; this plan changes none. | `difficulty_presets.json` | LIVE |

**Five findings that shape this plan (recorded so nobody rediscovers them mid-package):**

1. **E6 — four owners speak, nobody listens.** Rumours, PsyOps, radio and market rumour lines exist; none has a *region*, and none reads what a region believes. The Narrative Field is a derived read over all four, with one authored map from hubs to regions.
2. **E7 — falsehood is free today.** `Truthfulness` is generated and displayed but has no consumer that punishes or exposes. WW adds exposure as a seeded, recorded rule — the only place this plan invents a *consequence*.
3. **E9 — the Before is never assembled.** Seventy-nine records are discovered one at a time. The Inquest is the first thing that reads them *together*, and it does so without editing any of them.
4. **E12 — the Verdict is already an inquest of a kind.** A three-witness arc runs days 241–262. The Inquest treats those records as ordinary exhibits and must not duplicate, gate or replace the arc.
5. **E15 — the silences are binding.** The Inquest is the feature most likely to tempt a later agent to fill a silence. Every finding therefore carries a printed Silence, a validator rule forbids "why" and "who" wording (§16), and the locked silences are enumerated as a *do-not-answer* list (§25).

---

## 2b. Evidence pass 1 — premises checked against source (2026-09-29)

| # | Open item | Result | Edit made |
|---|---|---|---|
| E7 | Truthfulness consumers | Confirmed, with a sharper finding. `RumorSystem` (`InformationFlow/RumorSystem.cs`) sets `Truthfulness` at generation, **scales it by the hub's `Credibility × 0.95` on spread** (L200, floored at 0.1) and **subtracts `DecayRate` every day** (L230). Nothing refutes or penalises a false rumour. | A Telling's chosen honesty is **an input, not a fate**: it will already be degraded by hub credibility and daily decay by the time anyone hears it. The plan treats that as *fidelity of the road*, not a bug |
| E18 | Can a caller choose origin, subject, truthfulness? | **Yes.** `GenerateRumor(originLocationId, subjectType, subjectId, headline, description, truthfulness = 0.8f, currentDay = 1)` takes all of them; truthfulness is clamped 0..1. | the fallback ("ledger holds the honesty, rumour uses default") is **removed**; a Telling passes 1.0 / 0.5 / 0.0 directly |
| E10 | Read for an arbitrary `knowledge_key` | **Exists:** `journal.Knowledge.Has(key)` (used by `BureaucraticDocumentDiscoverySystem.IsDiscovered`, `Narrative/BureaucraticDocumentCatalog.cs` L455–473); keys are prefixed (`bureaucratic_document_`). | `CaseFile.Exhibits` calls `journal.Knowledge.Has(record.knowledge_key)` |
| E20 | Market-shock request seam | **Exists as an input:** `MarketSystem.ApplyShock(categoryId, isShortage, severityBp, startDay, durationDays, sourceId)` — returns null for an unknown category, clamps severity and caps duration at 60 days. | `IShockRequest` real adapter calls `ApplyShock` with `sourceId = "ww:<consequenceId>"`; `NullShockRequest` stays the default |
| E17 | Does a Sitting fit the desk's job model? | **No.** `ArchiveDeskSystem.QueueTranscription(evidenceId, archivistId, inkId)` queues a *transcription of evidence*; jobs carry `archivistId`; `GetActiveJobs()` and `IsEvidenceUnlocked` are the reads. A sitting on a case line is a different act. | **No additive job kind.** The investigator's Sitting is credited by the plan's own host day tick, and the desk is *read* only to see whether the investigator is already an archivist on an active job (a busy investigator cannot sit) — DEC-LI-12 |
| E21 | Investigator aptitude, clarity | `MemoryDecaySystem` holds `MemoryRecord.Clarity` (`MemoryClarity`, default Vivid) — a real clarity read. No reading or literacy skill exists in `skills.json`; the archivist role is an assignment, not a skill. | investigator aptitude = **archivist assignment + Vivid/Clear clarity of the relevant memory**; no skill is invented (DEC-LI-13) |
| E19 | Living Region reader and ids | The hook is still plan-only. Region ids remain working labels until that plan ships. | unchanged and honest: `IRegionNarrativeReader` ships `Null*`; region ids are labelled *working* in §8 |
| E15/E12/E14 | Open Mystery Index, Verdict arc, endgame-only records | Read as written. | unchanged |

**What did not change:** the Region Ledger, Tellings, the Case File and exhibit rule (a discovered record is read, never copied), the five-way standing/market consequence rows, and the silences bound to the Open Mystery Index.

**One consequence worth stating plainly.** The world already lies on its own. A rumour told with perfect honesty will arrive at the far hub thinner, and a lie told well will arrive as a rumour anyone might have started. The War of Words does not add deceit to the road — it gives the player a hand on it.

---

## 3. Authority table (one authority per concern — CLAUDE.md Rule 5)

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Rumours, hubs, truthfulness, decay, interception | `RumorSystem` | none (requests via `GenerateRumor` / `PropagateRumorToHub` only) |
| Faction propaganda campaigns, jamming, pressure | `PsyOpsSystem` | none (read `PressureOn`, `EffectiveReach`) |
| NPC and shelter radio | `FactionRadioEngine`, radio owners; player station per the RF plan | none |
| Market shocks and rumour lines | market owner, `EconomyMarketRumorRules` | requests through an existing shock seam only (E20) |
| Settlement condition, refugees, graded news | Living Region (plan) | none (LR reads the narrative reader hook) |
| Faction standing | existing standing owner | requests only |
| Discovery of world-history records | journal/knowledge store and each record's trigger | read-only |
| Archive desk, journal, memorial | `ArchiveDeskSystem`, `JournalSystem`, `MemorialSystem` | read-only; chronicle appends |
| Record custody and condition | Record Keepers (plan) | read-only if present |
| **Story catalog** | — | `stories.json` (data) |
| **Hub → region map, region grip weights** | — | `narrative_regions.json` (data) |
| **Grip, bands, contest, Contested** | — | pure derived read models |
| **Tellings, pamphlet drops, corrections, exposures, taken-stories** | — | `NarrativeLedger` (stored) |
| **Questions, findings, exhibit bindings, silences** | — | `inquest_questions.json` (data) |
| **Exhibits, grades, leads (derivable)** | — | pure derived read models |
| **Lines of Inquiry, sittings, leads shown, statements, reports** | — | `InquestState` (stored) |

**Non-duplication statement.** Every rumour, campaign, broadcast, shock, standing value, discovery and journal entry stays with its owner. The plan stores only **what the shelter did**: a telling made, a pamphlet dropped, a correction issued, an exposure recorded, a story that *took* in a region; a line of inquiry opened, a sitting spent, a lead shown, a finding stated, a report compiled. Everything else — Grip, band, Contested, exhibit set, grade, lead — is derived on read. **No new rumour, propaganda, radio, market, standing, discovery or archive authority is created.**

---

## 4. Claimed Paths (proposed; `INT` = integrator-owned)

**Core (WW):** `Assets/Ashfall.Core/InformationFlow/Narrative/NarrativeLedger.cs` (new: state + rules), `Narrative/StoryCatalog.cs` (loader), `Narrative/NarrativeField.cs` (pure: Grip, bands, contest), `Narrative/Tellings.cs` (acts and honesty), `Narrative/Exposure.cs` (seeded rule), `Narrative/StoryConsequences.cs` (rows to owner seams), `Narrative/StoryEventSeeds.cs` (owner events to stories), `Narrative/NarrativeCatalogLoader.cs`.

**Core (LI):** `Assets/Ashfall.Core/Narrative/Inquest/InquestCatalog.cs` (loader), `Inquest/CaseFile.cs` (pure: exhibits, grades, contested, leads), `Inquest/InquestState.cs` (state + rules), `Inquest/Sittings.cs`, `Inquest/Statements.cs`, `Inquest/InquestReport.cs`, `Inquest/InquestCatalogLoader.cs`. Additive nested DTOs in the rumour-network save (WW) and the archive-desk or journal save (LI), `INT`, homes chosen at P0.

**Data (WW):** `stories.json`, `narrative_regions.json`, `story_event_seeds.json`, `truth_predicates.json`, `story_lines.json`.
**Data (LI):** `inquest_questions.json`, `inquest_exhibit_overrides.json`, `inquest_lines.json`.

**Host:** `src/Main.RumorNetwork.cs` (`INT`: telling and pamphlet credit; region tick), `src/Main.SubsystemComposition.cs` (`INT`: day-owner and probe registration), the archive-desk host session (`INT`: sitting credit), `src/Main.Campaign.cs` (`INT`: briefing lines).

**Presentation (both):** extend `RumorBoardPanel` (Field) and the archive/journal surface (Inquest). No new routed panel (DEC-WW-08, DEC-LI-08).

**Tests:** `Ashfall.Core.Tests/InformationFlow/Narrative/*` (Field, Grip, Tellings, Exposure, Consequences, Seeds, Save); `Ashfall.Core.Tests/Narrative/Inquest/*` (CaseFile, Grades, Sittings, Leads, Statements, Report, Silences, Save); parity guards extend the existing rumour, PsyOps, radio, journal and discovery tests unchanged.


---

# PART ONE — THE WAR OF WORDS

## 5. Regions, hubs and the Narrative Field

### 5.1 The hub-to-region map (authored, the one new fact)

Hubs have locations (E2) but no region (E6). `narrative_regions.json` adds a single authored mapping from **hub id** to **region id** and a **region weight** used by broadcasts. Region ids are the Living Region settlement ids when that plan exists (E19 — still plan-only, §2b); until then they are *working labels* in this table.

| Hub (E2) | Credibility | Capacity | Region (working label) | Bias carries into |
|---|---|---|---|---|
| Crossroads Caravanserai | 0.85 | 6 | `region_crossroads` | trade stories weigh more |
| Rustwater Scavenger Canteen | 0.70 | 4 | `region_rustwater` | scavenger stories |
| High Ridge Relay Station 7 | 0.95 | 8 | `region_high_ridge` | news/neutral stories |
| Iron Coalition Checkpoint Bravo | 0.80 | 5 | `region_iron_line` | militant stories |
| Subterranean Blind Exchange | 0.65 | 5 | `region_blind_exchange` | underground stories |
| Ashen Plateau Hermitage | 0.60 | 3 | `region_ashen_plateau` | hermit/quiet stories |

**Rule N-1.** A hub belongs to exactly one region; a region may have zero hubs (it is then reached only by Air and Hand). A region with no hubs shows *"No one there listens to the bench."* Validator N-V1 enforces it.

### 5.2 The Field (derived, never stored)

`NarrativeField.Read(day)` returns, per region, a list of **Presences**:

```csharp
public sealed class StoryPresence
{
    public string StoryId;
    public string RegionId;
    public int    Grip;               // 0..100
    public GripBand Band;             // Whisper | Talk | Belief | Certainty
    public int    HubGrip, AirGrip, HandGrip;   // channel parts, for the three-roads sentence
    public int    Opposition;         // half of the strongest opposing story's grip
    public List<string> ChannelWords; // "the caravanserai", "the pamphlet", ...
}
```

**Bands:** Whisper < 20, Talk 20–44, Belief 45–69, Certainty ≥ 70 (data, DEC-WW-04). Bands are words on a row; the number is never shown (DEC-WW-07).

### 5.3 The three channels

| Channel | Carries | Owner record read | Notes |
|---|---|---|---|
| **Hubs** | rumours planted or seeded | `RumorSystem` rumours with a `story_id` tag and `ReachedHubIds` | credibility per hub is the owner's |
| **Air** | broadcasts | active `PsyOps` campaigns (theme→story map) and the shelter's own broadcasts (RF plan; `NullBroadcastChannel` if absent) | reach from owner reads |
| **Hand** | pamphlets and carriers | the Narrative Ledger's pamphlet drops | needs `paper_stock` and a carrier |

A story present in no channel has no Presence. A story never *stays* by itself: every contribution decays with age (§6), so belief has to be *kept fed*.

---

## 6. Grip (a derived reading)

### 6.1 Definition

For story S in region R on day d:

`Raw = Hub + Air + Hand`

`Grip = clamp( Raw − 0.5 × StrongestOpposingRaw , 0 , 100 )`

**Hub.** For each rumour `r` tagged `S` that reached a hub `h` in R (and is not intercepted and not exposed):
`c(r,h) = 18 × credibility(h) × fresh(r, d)` where `fresh = max(0, 1 − DecayRate × age_days)`. Sum over rumours and hubs, **capped at 30 per region** (in v1 each region has at most one hub, so this is also the per-hub cap).

**Air.** For each broadcast or active campaign tagged `S` that ran in the last 7 days over R:
`a = 0.35 × AirReach(R) × max(0, 1 − age_days / 7)`, where `AirReach(R)` is a 0..100 read from the RF audience affinity for R (if present), else from `PsyOps.EffectiveReach` mapped through `narrative_regions.json: air_weight`. Sum, cap 35.

**Hand.** For each pamphlet drop tagged `S` in R:
`h = 9 × max(0, 1 − age_days / 20)`. Sum, cap 27.

**Opposition.** The **strongest** presence in R whose story is listed in S's `opposed_by`; its **Raw** sum (not its net Grip — that would be circular) is halved and subtracted. Two opposing stories in one region cannot both reach Certainty.

All constants are data (DEC-WW-04). **No contribution is ever stored**; only the acts that created them (a rumour, a campaign, a drop) exist as records, in their owners and in the ledger.

### 6.2 Worked example — "the shelter has water to spare", Crossroads, day 60

Story `story_clean_water_at_holdfast`, region `region_crossroads`.

| Source | Detail | Contribution |
|---|---|---|
| Hub rumour | planted at Crossroads Caravanserai (credibility 0.85), age 4 days, `DecayRate` 0.05 → fresh = 0.80 | `18 × 0.85 × 0.80 = 12.24` |
| Air | broadcast 3 days ago, `AirReach` 55 → fresh = 1 − 3/7 = 0.571 | `0.35 × 55 × 0.571 = 11.00` |
| Hand | two pamphlet drops, age 6 → fresh = 0.70 | `2 × 9 × 0.70 = 12.60` |
| Sum | | **35.84** |
| Opposition | `story_shelter_hoards` present with Raw 20 | `0.5 × 20 = 10` |
| **Grip** | `Raw 35.84 − 10 = 25.84` → 25 | **Talk** |

The Field row reads: *"At the crossroads they are saying the shelter has water to spare. It came by the caravanserai, by the air and by a pamphlet. Some say the opposite."* Band word **Talk**. To reach Belief (45) the player must add ~20 more or remove the opposition: a second rumour at a fresher age (+~12), a third pamphlet drop (+6.3), or a **Correction** (§8.4) of the hoarding story.

### 6.3 Rules

- **G-1** Grip is recomputed on read; it has no tick and no hidden state.
- **G-2** Stale contributions vanish by arithmetic (age); the Field never needs a cleanup pass.
- **G-3** An **intercepted** rumour (owner flag) and an **exposed** telling (ledger flag) contribute 0 from the flag's day.
- **G-4** The caps stop any single channel from carrying a story alone: the highest single-channel Raw is 35 (Air), below Belief (45). **Belief needs at least two channels** — the design rule that makes stories *travel*.
- **G-5** Difficulty never scales any Grip constant (X-6).

---

## 7. The story catalog (`stories.json`, 14 authored stories)

A **story** is a short claim with a subject, a truth test, the regions it fits, opposing stories, consequence rows and a short line. Stories are **abstract claims about the world**, never persuasion technique and never about a real place or person.

| # | Story id | The claim (line) | Theme | Truth predicate | Opposed by |
|---|---|---|---|---|---|
| 1 | `story_clean_water_at_holdfast` | The shelter has water to spare. | Water | `water_days_ge_10` | `story_shelter_hoards`, `story_the_water_is_bad` |
| 2 | `story_shelter_hoards` | The shelter is sitting on more than it says. | Grievance | `stores_ge_hoard_line` | `story_clean_water_at_holdfast` |
| 3 | `story_guild_lines_running` | The rail guild is running its lines again. | Road | `rail_running` | — |
| 4 | `story_the_coast_road_is_safe` | The coast road is open and quiet. | Road | `coast_road_clear` | `story_the_coast_road_is_unsafe` |
| 5 | `story_the_coast_road_is_unsafe` | Something is taking carts on the coast road. | Road / Fear | `coast_road_troubled` | `story_the_coast_road_is_safe` |
| 6 | `story_the_garrison_is_coming` | The garrison is coming down the road. | Fear | `garrison_near` | `story_the_garrison_is_far` |
| 7 | `story_sickness_in_the_low_fields` | There is sickness in the low fields. | Sickness | `sickness_reported` | `story_the_low_fields_are_clean` |
| 8 | `story_the_cutters_take_a_tenth` | The road band only takes a tenth. | Toll | `toll_at_or_below_100` | `story_the_cutters_take_it_all` |
| 9 | `story_the_foundry_is_starving` | The foundry is short of coal and brine. | Trade | `foundry_short` | — |
| 10 | `story_a_treaty_is_signed` | Two of the big ones have signed something. | Treaty | `any_treaty_active` | — |
| 11 | `story_the_water_is_bad` | The shelter's water is not fit to drink. | Water / Grievance | `water_quality_bad` | `story_clean_water_at_holdfast` |
| 12 | `story_someone_lives_in_the_deep` | Someone came up out of the deep alive. | Hope | `deep_survivor_known` | — |
| 13 | `story_relief_is_coming` | A relief column is on its way. | Hope / Siege | `relief_dispatched` | `story_no_one_is_coming` |
| 14 | `story_the_before_was_not_what_they_say` | It was not what they told you, before. | Doubt | `inquest_finding_ge_attested` | — |

(Four **named opposites** — `story_the_garrison_is_far`, `story_the_low_fields_are_clean`, `story_the_cutters_take_it_all`, `story_no_one_is_coming` — are catalog rows too, so every contest is symmetric: 14 + 4 = **18** rows, plus **two** finding-linked rows added by the Inquest (`story_the_count_was_published`, `story_the_coalition_is_founded`, §15.2), **20** in all. Rows 4 and 5 are each other's opposite and are both in the table.)

### 7.1 Story row shape

```json
{
  "story_id": "story_clean_water_at_holdfast",
  "line": "The shelter has water to spare.",
  "theme": "water",
  "truth_predicate": "water_days_ge_10",
  "regions": ["region_crossroads", "region_high_ridge", "region_rustwater"],
  "opposed_by": ["story_shelter_hoards", "story_the_water_is_bad"],
  "contests_findings": [],
  "consequences": [
    { "at": "belief",    "kind": "lr_bias",  "params": { "petition_bias": 1 } },
    { "at": "certainty", "kind": "standing", "params": { "faction_ref": "hub_region_faction", "delta": 2 } }
  ],
  "seed_from": ["water_surplus_event"]
}
```

### 7.2 Truth predicates (`truth_predicates.json`, a closed table)

Each predicate is a **named read** of an existing owner returning true, false or unknown. Unknown is treated as **Half**. No predicate computes anything the owner does not already expose.

| Predicate | Reads (owner) |
|---|---|
| `water_days_ge_10` | days of water in store (water/inventory owners) |
| `stores_ge_hoard_line` | food+fuel days above an authored line |
| `rail_running` | Iron Road ledger rung ≥ Running (plan 1), else false |
| `coast_road_clear` / `coast_road_troubled` | recent Road War / threat reads (plan 2), else unknown |
| `garrison_near` | garrison faction position/pressure read |
| `sickness_reported` | plague/quarantine owner |
| `toll_at_or_below_100` | effective toll hook (plan 5), else authored row |
| `foundry_short` | foundry accords / market shock read |
| `any_treaty_active` | treaty owners |
| `water_quality_bad` | water treatment quality read |
| `deep_survivor_known` | a recorded fact from the Deep plan; else false |
| `relief_dispatched` | Siege Year relief carrier (plan 1), else false |
| `inquest_finding_ge_attested` | Case File read (Part Two) |
| `always_true` / `always_false` | constants for authored test stories |

### 7.3 Rules

- **S-1** Every story has exactly one truth predicate that resolves in the closed table.
- **S-2** Every `opposed_by` pair is symmetric (if A lists B, B lists A) — validator N-V4.
- **S-3** No story names a real person, place, faction of the real world, or a persuasion tactic; lines are plain claims (validator N-V7).
- **S-4** A story is **seedable** (a player can plant it) only if its `seedable` flag is set; the four named opposites are seedable **only** as Corrections (§8.4).

---

## 8. The four acts (Tell, Broadcast, Print and Carry, Correct)

Every act creates **one record** in the Narrative Ledger and (where an owner exists) **one request** to that owner. Each act runs through the same **Honesty** rule.

### 8.1 Honesty (recorded once, at the moment of the act)

`Honesty = predicate(story) at the act's day`: **True**, **False**, or **Half** (predicate unknown, or the story is a Statement of an Attested finding). It is stored on the record and never recomputed (what was true *then* is the historical fact; the world may change). It matters for exposure (§10) and for the Voice Trust cost.

### 8.2 Tell (plant a rumour)

- **Preconditions:** a story flagged seedable; a **hub** in reach (the shelter's known hub set); a survivor free for one **carrier-day** (the assignment owner).
- **Effect:** `RumorSystem.GenerateRumor(...)` with the hub as origin, the story's line as headline, `Truthfulness` = 1.0 (True) / 0.5 (Half) / 0.0 (False) — **the owner's own field**; a caller can set it (E18, verified §2b), though hub credibility and daily decay will thin it in transit. Then `PropagateRumorToHub` for the origin hub only; the owner's own propagation does the rest.
- **Ledger record:** `Telling { Id, Kind: "tell", StoryId, RegionId, HubId, Day, Honesty, CarrierId, RumorId }`.
- **Cost:** one survivor-day. No items.

### 8.3 Broadcast and Print and Carry

**Broadcast.** If the shelter has a station (RF plan), the player books the story as a program in a slot; the RF owner delivers it; the ledger records `Kind: "broadcast"`, region set from the station's audience reach, and `Honesty`. Without a station the act is **unavailable** (never faked). A **PsyOps campaign** aimed at a faction is also an Air source: the catalog maps each of the eight campaign themes to a story (Unity → `story_a_treaty_is_signed`, Hope → `story_someone_lives_in_the_deep`, AidPromise → `story_clean_water_at_holdfast`, Fear → `story_the_garrison_is_coming`, DefectionAppeal → `story_the_before_was_not_what_they_say`, CounterRumor → a **Correction** of the most-gripped opposing story). The mapping is data (`stories.json: psyops_theme_map`), so the eight authored campaigns become story carriers **without editing the campaign catalog**.

**Print and Carry.** Consumes `paper_stock` ×1 per drop (inventory command) and a survivor-day of *printing*, then a **carrier** (a caravan on a route; plan 2) delivers at its next arrival in the region. Ledger: `Kind: "print"`, `DropDay` = arrival day (a carrier that never arrives never creates the drop — the record is written **on arrival**, not on printing, so a lost cart lost the pamphlets). Without a caravan the player may carry by hand: `DropDay = day + travel days` (data). Pamphlets are **paper**: the Paper and Power plan's paper economy is the same `paper_stock`; forged authority marks are out of scope here.

### 8.4 Correct

A **Correction** is a Tell (or Broadcast/Print) of an **opposing story** — the true counter. It has one additional rule: if the player cites **evidence** — an Inquest finding graded Corroborated or better (LI) or a Record Keepers Reading (E16) — the correction's contributions are multiplied by **1.5** for that record (`Evidence: "finding:<id>"` on the ledger row). Corrections are the only way to cut into a story that already has Belief: the opposition term of §6 does the arithmetic.

### 8.5 Worked example — a correction

Day 70, region `region_crossroads`. `story_shelter_hoards` has Raw 50 (its own channels, aged). `story_clean_water_at_holdfast` has Raw 35.84 (§6.2).

| Story | Raw | Opposition | Grip | Band |
|---|---|---|---|---|
| Hoards | 50 | `0.5 × 35.84 = 17.9` | 32 | Talk |
| Clean water | 35.84 | `0.5 × 50 = 25` | 10 | Whisper |

The player prints two more pamphlets of `story_clean_water_at_holdfast` (True; water days 14) and cites a **Corroborated** finding: the drops arrive on day 74, each `9 × 1.0 × 1.5 = 13.5`, so `+27` — but the **Hand cap is 27 per region** and two older drops already hold `12.6`, so the Hand channel is capped at 27 (a gain of only 14.4). The Raw becomes `12.24 + 11.0 + 27 = 50.24`.

| Story | Raw | Opposition | Grip | Band |
|---|---|---|---|---|
| Hoards | 50 | `0.5 × 50.24 = 25.1` | 24 | Talk |
| Clean water | 50.24 | `0.5 × 50 = 25` | 25 | Talk |

**The bench is split.** The caps did their job: pamphlets alone cannot win the argument. To reach Belief the player must feed **another channel** (a fresh hub rumour, a broadcast) — the design rule of G-4. *Recomputed on read; no stored value moved.*

---

## 9. Consequences (rows onto seams that already exist)

### 9.1 The rule

A **consequence row** fires when a story's Grip in a region first reaches a band (`belief` or `certainty`). It fires **once per taken-instance** (below) and calls **one** existing seam. If the seam does not exist yet, the row is **inert** (ship dark).

| Kind | Seam | Effect | Status |
|---|---|---|---|
| `lr_bias` | `IRegionNarrativeReader` read by Living Region | a small bias on petitions or wave direction | dark until LR exists (E19) |
| `standing` | the standing write owner | a small delta with the regional faction | `FactionWarSystem.ModifyStanding` (plan 5 §2b, DEC-PP-14) |
| `market_shock` | the market's shock seam | a bounded price nudge on named goods | `MarketSystem.ApplyShock` (verified §2b) |
| `chronicle` | chronicle writer | a line only | LIVE |

**No consequence writes to an owner's private state.** Each is a request the owner already accepts, or a read the owner opts into.

### 9.2 Taken-instances (the one stored fact besides tellings)

When a Grip first reaches Belief in a region the ledger records `StoryTaken { StoryId, RegionId, TakenDay, EndedDay = -1, PeakGrip, Origin }`. It **ends** when Grip falls below Talk (recorded the first read that finds it so; `EndedDay` set, `PeakGrip` kept). A story that rises again later creates a **new** instance with a new `TakenDay`. Consequence rows are keyed `story:<id>:<region>:<takenDay>:<row>` in `ChronicleKeys` for exactly-once firing across save/load.

**Detection of "first reached".** The ledger records a taken-instance when the day-owner's read finds Grip ≥ Belief and no open instance exists. Because Grip is derived, the day-owner reads once per day per region with presences (cheap: at most 6 regions × the handful of stories present).

### 9.3 Consequence rows (excerpt, `stories.json`)

| Story | At | Kind | Params | Line |
|---|---|---|---|---|
| `story_clean_water_at_holdfast` | Belief | `lr_bias` | petition_bias +1 (people ask to come) | "They are asking the road how far it is." |
| `story_clean_water_at_holdfast` | Certainty | `standing` | +2 with the hub-region's faction | "The caravanserai sends its regards." |
| `story_shelter_hoards` | Belief | `standing` | −3 with the hub-region's faction | "They have stopped looking you in the eye at the bench." |
| `story_shelter_hoards` | Certainty | `market_shock` | +5% on water and grain in the region for 10 days | "The price of asking went up." |
| `story_the_coast_road_is_unsafe` | Belief | `market_shock` | +8% on sea goods | "Nobody wants to take a cart down there." |
| `story_the_garrison_is_coming` | Belief | `lr_bias` | wave direction away from the garrison line | "The families from the line are packing." |
| `story_sickness_in_the_low_fields` | Belief | `market_shock` | +10% on medical goods, 14 days | "Someone bought all the boiled cloth." |
| `story_a_treaty_is_signed` | Belief | `chronicle` | — | "Everyone has an opinion on what was signed." |
| `story_relief_is_coming` | Belief | `lr_bias` | resolve +1 in siege regions | "They have started to leave the lamps on." |
| `story_the_before_was_not_what_they_say` | Belief | `chronicle` | — | "There is a new way of saying *before* at the bench." |

Numbers are **data** (DEC-WW-05) and bounded by validator N-V5 (no delta above ±5 standing, no shock above ±12%, no duration above 20 days).

---

## 10. Exposure and cost (the only consequence this plan invents)

### 10.1 Rule

Each **False** or **Half** telling that is still contributing is checked once per **week** (7 days after the act, then every 7): a seeded roll from the stream `story_exposure:<telling_id>:<week>`:

`Expose% = clamp( 6 + 4 × HubsReached + 2 × RegionsReached − 3 × Careful , 3 , 60 )`, with **Half** halving the result (round down).

- `HubsReached` from the rumour's `ReachedHubIds` (owner read); `RegionsReached` = distinct regions in which the telling has a contribution; `Careful` = 1 if the telling was made with an **evidence** citation (a Half telling backed by an Attested finding) else 0.
- An **exposed** telling is flagged `Exposed`, `ExposedDay` = the day; its contributions are 0 from that day (G-3).
- **True** tellings are never exposed (nothing to expose).

### 10.2 Costs (through existing owners only)

| Consequence | Route |
|---|---|
| Voice Trust falls | the RF `VoiceTrustLedger` if present (hook, `NullVoiceTrust` default) |
| Standing with the region's faction | `standing` request from the exposure row (−2, bounded) |
| Chronicle | one line, idempotent |
| A `StoryTaken` instance in that region **ends** | derived: the exposed telling no longer contributes; if Grip falls below Talk the instance closes on the next read |

### 10.3 Worked example

A **False** telling of `story_the_coast_road_is_safe` was planted at Crossroads (1 hub, 1 region) and later reached the Relay Station (2 hubs, 2 regions). `Expose% = 6 + 4×2 + 2×2 − 0 = 18%` per week. Week 1 roll 61: not exposed. Week 2 roll 9: **exposed** on day 84. The chronicle line: *"The driver went to look. He came back and did not say anything, and that is how it got round."* Voice Trust −(owner's constant, from the RF ledger); standing −2 with the caravanserai's faction; the story's Grip in both regions drops by that telling's contribution on the next read.

### 10.4 Rules

- **X-1** Exposure is seeded and per (telling, week); re-loading cannot re-roll.
- **X-2** Exposure never edits the rumour owner's `Truthfulness`; the owner's data are untouched.
- **X-3** A telling made with `Honesty = True` at the time stays True even if the world later changes; the *new* falsehood would need a new telling to be exposed.
- **X-4** No penalty exists for **not** telling. Silence is free.

---

## 10b. Texture — the bench

**The second line.** Each telling produces two lines: the act ("Marek walked to the caravanserai and said it to the right person") and, the next day, its *fate* ("A woman repeated it to a driver. The driver said he had heard it already"; or "Nobody repeated it"). The fate line is chosen from Grip movement: rising, flat, falling.

**A pamphlet is paper.** *"Two hundred words and a thumbprint on the corner. It was read aloud at the canteen by someone who could read, and believed by someone who could not."*

**The air.** *"It went out on the evening slot. Fifty listeners, by the operator's count; the operator said that meant nothing."* (The count is the RF plan's; the line is the Field's.)

**The lie.** A False telling is never given a villain's voice. The player's carrier says it in the same flat tone as the True ones. The game does not warn; it records. *"He said it exactly as he had said the true one."*

**Belief is uncomfortable to watch.** When a story reaches Belief, the row is quiet: *"They believe it now. It is not clear that it matters whether it is so."* (Only when Honesty was False or Half at the origin of the strongest telling.)

---

## 11. The Field in play — worked scenes

### 11.1 A quiet week

Day 60. The shelter has 14 days of water. The player tells `story_clean_water_at_holdfast` at the Crossroads bench (True). Day 61 fate line: *"A woman repeated it to a driver."* Days 62–66 no more acts. Day 67 the rumour has aged 7 days: fresh = 0.65; contribution `18 × 0.85 × 0.65 = 9.9`, Grip 9 → **Whisper** (below Talk). The Field row reads *"There is some talk at the crossroads about water. It is old talk."* Nothing fires. **A single telling never reaches Talk on its own after a week**; stories must be kept fed.

### 11.2 A campaign

Day 100. The shelter runs the authored PsyOps campaign `psyops_campaign_hydro_water_honesty` (theme AidPromise, target `faction_hydro_barons`, base reach 45, 6 days). The catalog map turns it into an **Air** source for `story_clean_water_at_holdfast` over the regions whose hubs' faction is hydro barons (per `narrative_regions.json`). `AirReach` 45; day 102: `0.35 × 45 × (1 − 2/7) = 11.25`. **The campaign is untouched**; the Field only *reads* it.

### 11.3 A rival story

Day 110. An NPC rumour (authored seed from a market shock) spreads `story_the_water_is_bad` at Rustwater. The player has no action there: the Field shows *"At Rustwater they are saying the shelter's water is not fit to drink. It came by the canteen."* The player may Correct with a pamphlet and, if Corroborated evidence exists (an Inquest finding about the water? none) — else without the ×1.5. The lesson is in the row: **a lie you did not tell can still be yours to answer.**

### 11.4 Event seeds

`story_event_seeds.json` maps owner events to **seeded stories** (never a player act, never a telling): `water_surplus_event`, a rail segment reaching Running, a treaty ratification, a relief carrier dispatched, a market shock start, a Detected forgery (plan 5). A seed calls `RumorSystem.GenerateRumor` from a hub near the event with `Truthfulness` from the predicate and tags it with the story. Seeds ship dark unless the source plan exists. Seeds are **always** True/False by the predicate — the world tells stories of its own.


---

# PART TWO — THE LONG INQUEST

## 12. Exhibits (derived from discovery, never stored)

### 12.1 What an exhibit is

An **exhibit** is a `world_history` record (E8) — or another Before-record bound in `inquest_exhibit_overrides.json` — that the player has **discovered**. It is not copied, indexed or stored: `CaseFile.Exhibits(day)` asks the journal/knowledge store whether the record's `knowledge_key` is held (E10 — `journal.Knowledge.Has`, verified §2b) and returns the set. An undiscovered record is **not an exhibit**; the case file never lists what the player has not found (except through a Lead, §14).

### 12.2 Source classes and weights

Every exhibit has a **source class** (default from its `discovery_trigger`, override per row) and a **weight** — how much a finding may lean on it.

| Source class | Default from trigger | Weight | Notes |
|---|---|---|---|
| **Document** | `journal` (and archive/ledger items) | 3 | ×⅔ (round down, min 1) if its truth class is `TemplateCompatibleRecord`; **0** if `FlavorOnlyArtifact`; **excluded** if `UnsafeUnresolved` |
| **Physical** | `location_explore`, `inspection`, `first_visit` | 2 | a thing found where it lay |
| **Transmission** | `radio_intercept` | 2 | a tape, a broadcast, a cipher |
| **Testimony** | `survivor_dialogue`, `witness_*_heard` | 2 | 1 if the teller's clarity is Faded or worse (MemoryDecay read, E21) |
| **Excluded** | `ending_reached` | — | **never an exhibit** (E14): endgame-only; the Inquest stops before them |

`BlackProjectsTruthClass` **Allegation** counts as *Testimony* (weight 1); **ComplianceAudit** and **ClassifiedDirective** as *Document*; **InstrumentTelemetry** and **VehicleBlackbox** as *Physical*. The mapping is data.

### 12.3 Exhibit condition (optional, ships dark)

If Record Keepers exists (E16), an exhibit's **custody condition** modifies weight: Intact ×1, Faded ×0.75 (round down), Damaged ×0.5, **Lost** = a **Gap** (weight 0, the case file prints "a page is missing here"). Without it every exhibit is Intact (`NullCustodyReader`). The Inquest never edits custody.

### 12.4 Suspect exhibits

An exhibit whose source is flagged **Suspect** by a hook (`ISuspectExhibitReader`; e.g. a paper the Paper and Power plan marks Forged, or a record a Detected forgery touched) still appears in the case file but with **weight 0** and the line *"the desk has its doubts about this one."* It cannot corroborate and cannot be lifted by any act; it can only be **replaced** by a genuine exhibit of the same fact. Default: none suspect.

### 12.5 Worked example — the Office of Continuity

The player has explored `location_ministry_of_truth_bunker` and found the record titled *The Office of Continuity* (`lore_pre_continuity_office`, trigger `location_explore`) → **Physical**, weight 2. The body's own words (a formula "applied to everyone, with the arithmetic shown") are a *presentation* of the record; the Inquest **never reads the body text** — only the fact of discovery, class and weight. (**Rule C-1: the Inquest does not parse prose.** Findings are authored; exhibits only *support* them.)

---

## 13. Questions and Findings (twelve authored questions)

### 13.1 Shape

A **Question** asks *what* happened (never *why*, never *who is to blame*). It has:

- a **title** and a one-line **prompt**;
- 3 **Findings** (a ladder: an early plain statement, a middle one, a rare last one);
- for each finding, an **exhibit list** (`titles` bound to records; keys resolved at P0) and a **story_ref** (optional) for a Statement to a region;
- a **Silence** — the fixed line that closes the question ("Not established here: …").

```json
{
  "question_id": "q_the_count",
  "title": "The Count",
  "prompt": "How were the households that were continued chosen?",
  "findings": [
    { "finding_id": "f_the_count_1",
      "text": "A formula, not a person, decided which households were continued.",
      "exhibits": ["The Office of Continuity", "The Reconstruction Utility Rating"],
      "story_ref": null },
    { "finding_id": "f_the_count_2",
      "text": "The formula was applied to every household, and the arithmetic was published.",
      "exhibits": ["The Office of Continuity", "The Reconstruction Utility Rating", "The Bunker Boom"],
      "story_ref": null },
    { "finding_id": "f_the_count_3",
      "text": "An allocation was made in the first months after the exchange, and it was numbered 12-B.",
      "exhibits": ["Allocation 12-B", "The Ledger Nobody Signed", "Witness 2 — The Quartermaster's Paperwork"],
      "story_ref": null }
  ],
  "silence": "Who signed the allocation is not established here."
}
```

### 13.2 The twelve questions (`inquest_questions.json`)

Exhibit titles are **verified against `world_history.json`** (E8); their `knowledge_key`s are resolved at P0 from the data. Findings are drafted as **facts about what**, phrased from the *titles and eras*, never from the prose bodies (C-1).

| # | Question | Prompt | Exhibits (by title; era) | Finding ladder (what is established) | **Silence** |
|---|---|---|---|---|---|
| 1 | **The Count** | How were the continued households chosen? | The Office of Continuity; The Reconstruction Utility Rating; The Bunker Boom (pre); Allocation 12-B (black_sky); The Ledger Nobody Signed; Witness 2 (post) | 1: a formula chose. 2: it was applied to all and published. 3: an allocation, numbered 12-B, followed the exchange. | Who signed the allocation. |
| 2 | **The Forty-Five Minutes** | How long did it last, and what came after? | The Forty-Five Minute War; The EMP Silence; The First Night (hour_zero) | 1: it lasted forty-five minutes. 2: an electronic silence followed at once. 3: the first night was survived in the dark. | What was aimed at what. |
| 3 | **The Open Door** | Was a door open at the hour? | The Open Door; The Registrar Stays; Convoy 12 Turns Back (hour_zero) | 1: a door stood open at hour zero. 2: someone stayed to keep the register. 3: a convoy turned back rather than pass it. | Who opened the door. |
| 4 | **Convoy 12** | What became of the twelfth convoy? | Convoy 12 Turns Back; The First Night; Witness 1 — The Checkpoint Conscript | 1: Convoy 12 turned back. 2: a conscript at a checkpoint saw it. 3: the checkpoint did not stop it. | Whether it should have gone on. |
| 5 | **The Registrar** | Who kept the register, and what did it hold? | The Registrar Stays; The Office of Continuity; The Letters That Went Out | 1: a registrar stayed at the post. 2: the register was kept through the first night. 3: letters went out from it in the first months. | What the last entry said. |
| 6 | **The Failure** | What failed in the third week? | The Failure (Exchange+3W); The Nuclear Winter Begins; The Ozone Collapse | 1: something failed three weeks after. 2: winter began within the month. 3: the ozone went within two. | What failed first. |
| 7 | **The Vessel's Cell** | What was the cell? | The Vessel's Cell; Allocation 12-B; The Engineer Dies at 12-B | 1: a cell existed at 12-B. 2: an allocation touched it. 3: its engineer died there. | What the Vessel was. |
| 8 | **The Ledger** | What is the ledger nobody signed? | The Ledger Nobody Signed; Witness 1; Witness 2; Witness 3 — The Intercepted Cipher; Day 238 — The Continuity Reclamation Decree | 1: a ledger exists, unsigned. 2: three witnesses each held a part. 3: a decree followed on day 238. | Who did not sign. |
| 9 | **The Letters** | What went out, and to where? | The Letters That Went Out; The Obstacle-Marking Annex; The Last Harvest | 1: letters went out two months on. 2: the annex marked obstacles along a route. 3: the last harvest preceded both. | Who they were written to. |
| 10 | **The Reading** | Did a reading take place? | The Reading (Exchange+3Y); The Children of the Dark; The Defection Crisis | 1: a reading took place three years on. 2: it followed the defection crisis. 3: children of the dark were present. | What was read. |
| 11 | **The Water Wars** | What did the pre-war years turn on? | The Water Wars; The Rare Earth Crisis; The Bunker Boom | 1: water was contested ten years before. 2: a rare-earth crisis followed. 3: bunkers became standard. | Who drilled first. |
| 12 | **The Coalition** | When did the Coalition form? | The Coalition's Founding (Day 174); Day 238 — The Continuity Reclamation Decree; Witness 3 | 1: it was founded on day 174. 2: a decree answered it on day 238. 3: a cipher was intercepted in between. | Who first said "us". |

**Rules for findings.** Every finding: **one sentence**, ≤ 120 characters, a *fact about what/when/where*; contains **no** motive, blame or biography (§16 validator LI-V5). The three-step ladder always ends at a fact **one step short** of the Silence. The silence is *always printed* (§13.5).

### 13.3 Grading a finding (derived)

For finding *f* with exhibit list *E(f)*, let `held(f)` = the exhibits in *E(f)* the player has discovered (§12.1), `W(f) = Σ weight(e)` over `held(f)` (§12.2–12.4), `C(f)` = the number of **distinct source classes** in `held(f)` (weight > 0).

**Grade by exhibits:**

| Grade | Rule |
|---|---|
| **Rumoured** | `W ≥ 2` (one good exhibit) |
| **Attested** | `W ≥ 5` |
| **Corroborated** | `W ≥ 8` and `C ≥ 2` |
| **Established** | `W ≥ 11` and `C ≥ 3` and no Suspect exhibit in `held(f)` |

**Grade by sittings:** `S(f)` = sittings spent on *f* (§14). Required sittings: **Attested 1, Corroborated 3, Established 6**.

**Effective grade = min( grade by exhibits, grade by sittings )**. With `S = 0` the grade is therefore at most **Rumoured**.

Because sittings are spent by the player over days, **Established** is *slow by construction*: the fewest possible is 6 sittings on that one finding.

### 13.4 Worked example — Question 3, The Open Door

Finding 3 (`convoy turned back rather than pass it`): exhibits = *The Open Door* (Physical, 2), *The Registrar Stays* (Physical, 2), *Convoy 12 Turns Back* (Testimony from a survivor dialogue, weight 2; teller clarity Vivid); the list is extended below by override rows for a radio intercept and a document, to show the top of the ladder.

| State | `W` | `C` | Grade by exhibits | `S` | Effective |
|---|---|---|---|---|---|
| Only *Convoy 12* found | 2 | 1 | Rumoured | 0 | Rumoured |
| + *The Open Door* | 4 | 2 | Rumoured (W < 5) | 1 | Rumoured |
| + *The Registrar Stays* | 6 | 2 | Attested | 1 | **Attested** |
| … a Radio intercept (weight 2) added by override | 8 | 3 | Corroborated | 3 | **Corroborated** |
| … one more Document (3) | 11 | 3 | Established | 5 | Corroborated (sittings < 6) |
| … sixth sitting | 11 | 3 | Established | 6 | **Established** |

Line at Established: *"Established: a convoy turned back rather than pass the open door. Not established here: who opened it."*

### 13.5 The Silence, always printed

Under every question the surface prints the question's **Silence** in the same size as its findings. It is **not** hidden behind a grade; it is not a reward; it does not change. It is authored once (`inquest_questions.json`), fixed by validator (LI-V4), and **cross-checked against the do-not-answer list** in §25 (LI-V6).

---

## 14. Lines of Inquiry, Sittings and Leads

### 14.1 The stored state (the player's acts only)

```csharp
public sealed class InquestState
{
    public int SchemaVersion = 1;
    public List<LineOfInquiry> Lines = new();          // <= 12
    public List<LeadShown> Leads = new();              // <= 48 (history)
    public List<StatementRecord> Statements = new();   // <= 36
    public List<ReportRecord> Reports = new();         // <= 6
    public List<string> ChronicleKeys = new();
}
public sealed class LineOfInquiry
{
    public string QuestionId;
    public int    OpenedDay;
    public string InvestigatorId;                      // survivor id; null = unassigned
    public bool   SetAside;                            // player closed it
    public int    SetAsideDay = -1;
    public Dictionary<string,int> SittingsByFinding = new();   // finding id -> sittings spent
    public string CurrentFindingId;                    // where the next sitting goes
}
public sealed class LeadShown { public string FindingId; public string ExhibitTitle; public string LocationId; public int Day; }
public sealed class StatementRecord { public string FindingId; public string Grade; public string Audience; public int Day; public string TellingId; }
public sealed class ReportRecord { public int Day; public string CompiledBy; public List<string> FindingGrades; }
```

Home: nested in a single existing save owner (archive-desk save or journal save, DEC-LI-02). A legacy save loads with **no lines, no leads, no statements, no reports** and every discovery, journal and archive behaviour equals today's.

### 14.2 Opening a line

`OpenLine(questionId, day)` — allowed for any question; **free**. At most **12** lines (one per question) and **no more than 4 open at once** (data; DEC-LI-05) so the desk is never a spreadsheet. A line may be **set aside** (closed) and **reopened**; its sittings are kept.

### 14.3 The investigator and a Sitting

- The **investigator** is a survivor assigned through the existing assignment owner, with a reading/archive aptitude (archivist assignment + clarity read, DEC-LI-13; §2b). At most **one line per investigator**.
- A **Sitting** is *one survivor-day at the archive desk* spent on the line's **current finding**. It is credited when the desk's day-owner records the investigator's day (the desk cannot carry a sitting — E17, §2b; credited by the plan's own tick, DEC-LI-12). A sitting can only be spent on a finding that has `held(f) ≥ 1` exhibit **or** yields a Lead (§14.4).
- The investigator **cannot** sit on a day they are sick, on shift, away, or dead (assignment owner). There is no catch-up.
- `SittingsByFinding[f]++` on each credited sitting.

### 14.4 Leads

A sitting spent on a finding for which `held(f) < |E(f)|` (there is an exhibit the player has not found) **produces one Lead** the *first* time per (finding, missing exhibit): `LeadShown { FindingId, ExhibitTitle, LocationId = record.discovery_location_id, Day }`, in the exhibit list's authored order. The surface prints: *"The desk suggests {location display name}. It does not say why. It does not know."*

- A lead names a **place** (the record's own `discovery_location_id`), never the record's contents.
- A lead names the record's `discovery_location_id` **where one exists and the trigger can be met there** (`location_explore`, `inspection`, `first_visit`, the witness triggers, `radio_intercept` at a place). A record with **no location**, or with a pure event trigger, produces a **type lead** instead — *"The desk suggests someone would remember this."* / *"…a broadcast."* — so a lead never leaks a trigger the player could not act on.
- A lead **never** names an `ending_reached` record (excluded, §12.2).
- Leads are **history**: they record what the desk *said*, so a lead once shown is not shown twice.

### 14.5 Pace (design intent)

A player who finds every exhibit for one question and gives it one investigator will need **at least 6 sittings** on each finding's top grade and can sit on one finding at a time: about **12–18 days** to fully grade a three-finding question if nothing is missing. Twelve questions is a **year of a desk**, run beside the war, the road and the shelter's own life. There is no "Done": the Inquest is a long habit, not a quest. (§1b: *a desk and a lamp*.)

### 14.6 Worked example — twenty-two days on Question 8

Day 300. Marisol (aptitude 6) is assigned to **The Ledger**. Held: *The Ledger Nobody Signed* (Document 3), *Witness 1* (Testimony 2). Missing: *Witness 2*, *Witness 3*, *Day 238 Decree*.

| Day | Sitting on | Result |
|---|---|---|
| 300 | Finding 1 (`a ledger exists, unsigned`; exhibits: Ledger, Witness 1) | `W = 5`, C = 2 → Attested by exhibits; sittings 1 → **Attested** |
| 301 | Finding 1 | sittings 2; still Attested (needs 3 sittings for Corroborated, and `W` 5 < 8) |
| 302 | Finding 2 (`three witnesses each held a part`) | missing Witness 2/3 → **Lead:** *"The desk suggests the maritime icebreaker dock."* |
| 303 | Finding 2 | second missing exhibit → **Lead:** *"The desk suggests the D9 cache bunker."* |
| 310 | (player explores the dock; Witness 2 discovered) | Finding 2 exhibits held: Ledger 3 + Witness 1 2 + Witness 2 2 → `W = 7` → Attested by exhibits |
| 322 | (Witness 3 discovered) | `W = 3+2+2+2 = 9`, `C = 3` (Document, Testimony, Transmission) → Corroborated by exhibits; with `S ≥ 3` → **Corroborated** |

The desk never shows Marisol a "solution". It shows the grades; the **Silence** — *Who did not sign* — sits under all of it.

---

## 14b. Texture — the lamp

**The desk.** The Inquest occupies a corner of the archive desk (E17): a lamp, a stack, a ruled sheet, and a survivor with a list. There is no corkboard, no red string. The Case File is a *list of sentences* with a grade beside each and, under each question, the Silence.

**The investigator's day.** A sitting produces one line of the desk log: *"Marisol went through the ledger page by page again. She did not find the missing signature. She found that she had stopped expecting to."* (Line pool keyed by outcome: lead, no lead, grade rose, grade held.)

**A grade rising is quiet.** *"Attested. Two sources, one of them a tape."* Then the next line: *"Not established here: who opened it."* The second line is the same size as the first.

**A lead is a hunch with a place name.** *"The desk suggests the civil defence bunker. It does not say why. It does not know."* The player, in the first hours of the Inquest, learns that leads are **cheap and honest**: they never explain themselves.

**The desk has doubts.** A Suspect exhibit reads: *"The desk has its doubts about this one."* The desk never says *why* it doubts; the player learns to read the doubt as a fact about the exhibit's *route*, the way a clerk learns to read a stamp (Paper and Power, §1b).

**What the desk will not do.** It will not name a person. It will not use the words *because*, *decided*, *wanted*, *ordered* or *knew*. Validator LI-V5 forbids them in any finding (§16).

---

## 15. Statements and the Report

### 15.1 A Statement

A **Statement** says one finding aloud, **once**, to an **audience**:

| Audience | What happens |
|---|---|
| **The shelter** | a chronicle entry with the finding, grade and Silence; a line in the briefing next day |
| **The region** | a **Telling** (WW §8) is created with `story_id = finding.story_ref` (if the finding has one), `Honesty = True` if the grade is Corroborated or Established, `Half` if Attested, and the finding as `Evidence` for the ×1.5 on Corrections |
| **A faction** | a chronicle entry only in v1 (faction-facing effects are a backlog item) |

A **Rumoured** finding **cannot** be stated (the desk will not). A finding can be stated **once per audience**; the record stores `StatementRecord`. Re-statement after a grade rises is allowed as a **new** statement (the older one stays in history).

### 15.2 Which findings can reach a region

Most findings have **no** `story_ref` and can be stated only to the shelter. A `story_ref` is authored **only where a true story fits** (DEC-LI-07): at ship, **two**: Q1 finding 3 → `story_the_count_was_published` (a catalog row, the named opposite of `story_the_before_was_not_what_they_say`), and Q12 finding 1 → `story_the_coalition_is_founded` (a catalog row). Both are added by the same stories file. A statement to the region with such a ref creates the Telling; the Field does the rest.

**Worked example.** Q1 finding 3 reaches Corroborated on day 520. The player states it to the region: a carrier-day is spent, a Telling is written with `Honesty = True` and `Evidence = finding:f_the_count_3`; the rumour goes to the hub; the next day's fate line reads *"It went as a plain sentence."* If `story_the_before_was_not_what_they_say` holds Belief in the home region, Q1 findings 1–2 show **Contested** until a Correction lowers it (§17).

### 15.3 The Report

`CompileReport(day)` requires: at least one open or set-aside line; a survivor at the archive desk on the day (assignment owner). It writes a **chronicle artifact**: the twelve Questions in order, each with the **best effective grade of its findings** and its **Silence**. Bounded: ≤ 6 reports; the oldest is summarised to a count.

The Report is a *historical fact* ("what the desk knew on day 420") — never edited, never recomputed. A later Report shows progress by comparison.

**The Report always prints all twelve Silences**, even for a question with no line opened (its Silence stands alone: *"Not asked."*).

### 15.4 The Report read aloud (optional hook)

The Evenings and Memory Work plan's **Name Reading** ritual may read a Report at Founding Day (`IReportReader` hook; default none). It is a chronicle event only; no mechanic.

---

## 16. What the Inquest never returns (the Open Mystery Index, made mechanical)

The Long Inquest is bound by the Open Mystery Index's rule: *a system may return a state; it may not return a biography.* It is made mechanical in five ways:

1. **No motive words.** Validator **LI-V5**: no finding, lead line, desk-log line or silence contains a word from the deny-list — *because, decided, wanted, ordered, knew, planned, meant, blamed, guilty, betrayed, lied* — unless the subject is explicitly a **survivor of the shelter** (desk-log lines only).
2. **No names of the Before.** Findings name **events, places, dates and documents** by their existing titles. They name no person of the Before. (Validator LI-V3: every capitalised noun phrase in a finding must appear in a `world_history` title, a location display name, or a small allowlist.)
3. **Every question has a Silence** (LI-V4), printed at the foot.
4. **Silences are checked against the do-not-answer list** (§25; validator LI-V6): no finding may state a fact the list marks locked. In particular, **Olympus** (`SK-OM-1`, locked by DEC-SK-04) is never a question, exhibit or finding; the **post-day-720** silence (`Y2-OM-2`) is never touched; and no finding asserts what the retired Maritime Exploration system knew (`DC-OM-5`).
5. **The five `ending_reached` records are never exhibits** (E14). The Inquest stops short of the endings.

The Inquest therefore *can* show that a door was open and *cannot* show who opened it. That is not a limit of the tool; it is the design.

---

## 17. Contested findings (the one seam back to WW)

### 17.1 Rule

A finding is **Contested** on read when a story that lists it in `contests_findings` holds **Belief or Certainty** in the *shelter's home region* (a data constant; `region_high_ridge` by default) on that day. The surface adds *"The story going round says otherwise."* under the finding. That is **all**: the grade is unchanged, nothing is blocked, nothing is stored.

### 17.2 Rows (data)

| Story | Contests | Reason (line, plain) |
|---|---|---|
| `story_the_before_was_not_what_they_say` | Q1 findings 1–2 | "The story going round says the formula was not fair." |
| `story_a_treaty_is_signed` | Q12 finding 2 | "The story going round says the decree was answered by something else." |

### 17.3 Clearing

A finding stops being Contested when the contesting story falls below Talk — which the player may cause by a **Correction** citing that very finding (WW §8.4). That is the only loop between the two parts: **evidence buys reach; reach buys the right to be believed.**

---

## 17b. Worked timeline — a year of the desk (day 300 to day 665, both parts)

| Day | Event | Stored change | Player sees |
|---|---|---|---|
| 300 | Marisol assigned to Q8 | `LineOfInquiry` (Q8) | "The Ledger" opens at the desk |
| 300–303 | four sittings | sittings by finding; two leads | leads: icebreaker dock; D9 bunker |
| 310 | expedition to the dock; Witness 2 found | (discovery by owner) | exhibit appears |
| 322 | D9 bunker; Witness 3 found | (discovery by owner) | Finding 2: Corroborated |
| 330 | Statement of Q8 F1 to the shelter | `StatementRecord` | chronicle |
| 340 | NPC rumour: *the ledger is forged* (event seed) | Narrative rumour by the owner | Field row at Iron Line |
| 360 | it reaches Belief at Iron Line | `StoryTaken`; consequence row: chronicle | "There is a new way of saying *ledger*…" |
| 361 | Q8 F1 is **Contested** only if the story is in the home region | derived | (not contested: not home region) |
| 400 | player prints two pamphlets citing Q8 F2 (Corroborated) | ledger `print` ×2 with evidence | Correction, ×1.5 |
| 420 | first Report compiled | `ReportRecord` | twelve Silences printed |
| 500 | Q3 line opened, Q1 line opened | two lines | — |
| 580 | Q3 F3 Established after six sittings | none (derived) | "Established: a convoy turned back rather than pass the open door. Not established here: who opened it." |
| 665 | second Report | `ReportRecord` | comparison with day 420 by the player, not by code |


---

# PART THREE — DEPTH, CATALOGS, HOOKS, ACCEPTANCE, DELIVERY

## 17c. Presentation spec (no new routed panel)

**Field tab** (extends `RumorBoardPanel`; DEC-WW-08):

```
THE FIELD — what the regions are saying                       Day 60
REGION Crossroads     (hub: Caravanserai)
  "The shelter has water to spare."     TALK      by the caravanserai, the air, a pamphlet
      opposed: "The shelter is sitting on more than it says."   WHISPER
REGION Rustwater      (hub: Canteen)
  "The shelter's water is not fit to drink."   TALK   by the canteen
REGION Ashen Plateau  No one there listens to the bench.
ACTS   [Tell…] [Broadcast…] [Print & carry…] [Correct…]      Papers on hand: 6
LEDGER (last 8)  d56 Told  d57 Printed  d58 Dropped  d59 Correction…
```

**Inquest tab** (extends the archive/journal surface; DEC-LI-08):

```
THE LONG INQUEST — the desk                                   Day 310
Q8  THE LEDGER      line open · investigator Marisol · 3 of 4 open lines
  1  A ledger exists, unsigned.                       ATTESTED
  2  Three witnesses each held a part.                RUMOURED   desk suggests: the D9 bunker
  3  A decree followed on day 238.                    —
     Not established here: who did not sign.
Q3  THE OPEN DOOR   not opened
     Not established here: who opened the door.
[Open a line] [Assign] [Sit on finding…] [State…] [Compile a report]
```

**Every control** is keyboard and controller reachable with visible focus; Back closes the tab; controls meet the a11y height floor already in Core; band, grade and stage are always **words**, never colour alone. **No numbers are ever shown for Grip, weight or odds** (DEC-WW-07, DEC-LI-06).

---

## 17d. Authored content

### 17d.1 Fate lines (`story_lines.json`, day after a telling; keyed by Grip movement)

| Movement | Line |
|---|---|
| Rising | "A woman repeated it to a driver. The driver said he had heard it already." |
| Rising, second hub | "It was being said at the relay before it was said at the canteen." |
| Flat | "Nobody repeated it. Nobody contradicted it either." |
| Falling | "It came back to the bench with a small correction on it, which is worse." |
| Exposed | "The driver went to look. He came back and did not say anything, and that is how it got round." |
| Belief reached | "They believe it now." |
| Belief lost | "They have stopped saying it. They have not started saying the opposite." |

### 17d.2 Field lines by band

- **Whisper:** "There is some talk of it. It is old talk."
- **Talk:** "At {region} they are saying {claim}. It came by {channels}."
- **Belief:** "At {region} they say {claim} the way they say what day it is."
- **Certainty:** "At {region} nobody remembers when they were told. They only know it."

### 17d.3 Desk log lines (`inquest_lines.json`)

- Sitting, grade held: "{name} went through it again. It said what it said the first time."
- Sitting, grade rose: "{name} put two of them side by side. They agreed about something small."
- Sitting, lead: "{name} could not close the gap. She wrote down where she would look."
- Sitting, no lead, nothing missing: "{name} had nothing to add and stayed a while anyway."
- Setting a line aside: "{name} closed the folder and did not label it done."
- Report: "The report is {n} pages. Twelve of them are the same sentence."

### 17d.4 Lead lines

- Place: "The desk suggests {location}. It does not say why. It does not know."
- Someone: "The desk suggests someone would remember this."
- A broadcast: "The desk suggests it was said on the air, once."

### 17d.5 Statement lines (chronicle)

- Shelter: "Marisol stated it at muster: {finding}. She did not say the next part. There is no next part."
- Region: "It was carried to {region}: {finding}. It went as a plain sentence."

---

## 17e. Edge cases and rules

- **X-1** If a `knowledge_key` read is unavailable (E10 unresolved), the exhibit set is **empty** and the surface says *"the desk cannot see the records yet"* — never a guess.
- **X-2** An exhibit discovered *after* a statement was made does not change the statement; only later statements reflect it.
- **X-3** A survivor investigator who dies leaves the line open and unassigned; sittings are kept.
- **X-4** Two lines cannot share an investigator; one investigator cannot sit on two findings in a day.
- **X-5** A story planted in a region no hub reaches has no Hub contribution; it can still grow by Air and Hand.
- **X-6** Difficulty presets never scale Grip constants, exposure odds, grade thresholds, sitting requirements or lead behaviour.
- **X-7** Determinism: the only randomness is (a) exposure rolls per (telling, week), (b) seeded event-seed origin hub choice, (c) fate-line and desk-log line selection. All through named `CampaignStreamIds` forks; none from clock or hash order.
- **X-8** Save bounds: ≤ 48 tellings live (older summarised to counts), ≤ 24 taken-instances, ≤ 12 lines, ≤ 48 leads, ≤ 36 statements, ≤ 6 reports, ≤ 96 chronicle keys.
- **X-9** A story present in zero channels has no Presence and no row.
- **X-10** A hub or region removed from the data leaves ledger records intact; the Field skips unknown regions with `region_unknown` in the debug log only.
- **X-11** A Correction of a story that is not present is refused with *"there is nothing there to answer."*
- **X-12** Statements to the region create exactly one Telling; a failed carrier day (no free survivor) refuses the statement, leaving no record.

---

## 17f. Interaction with existing owners (what reads what, what requests what)

| Interaction | Direction | Note |
|---|---|---|
| `RumorSystem` | WW requests | `GenerateRumor`, `PropagateRumorToHub`; reads rumours, hubs, `ReachedHubIds`, flags |
| `PsyOpsSystem` | WW reads | campaign activity, `EffectiveReach`, theme; **the campaign catalog is not edited** |
| RF station (plan) | WW reads/requests | audience reach; program slot; Voice Trust write (hook) |
| Inventory | WW requests | `paper_stock` for pamphlets |
| Assignment | WW and LI request | carrier-day, printing-day, sitting-day |
| Standing owner | WW requests | consequence and exposure rows |
| Market shock seam | WW requests | bounded nudges (`ApplyShock`, verified) |
| Living Region | LR reads WW | `IRegionNarrativeReader` (dark) |
| Journal/knowledge | LI reads | discovery of records |
| `world_history.json`, other Before-records | LI reads | never edited |
| Archive desk | LI credits sittings | read-only busy check (E17, resolved §2b) |
| Record Keepers (plan) | LI reads | custody/condition (dark) |
| Verdict | LI reads | witnesses as ordinary exhibits |
| Chronicle | append-only | idempotent keys |

No owner changes behaviour. The only writes are to the Narrative Ledger and the Inquest State, plus **requests** the owners already support.

---

## 18. Catalog specs and validator rules

| File | Rows | Key fields |
|---|---|---|
| `stories.json` | 20 stories (14 seedable + 4 named opposites + 2 finding-linked) + `psyops_theme_map` (6) | `story_id`, `line`, `theme`, `truth_predicate`, `regions[]`, `opposed_by[]`, `contests_findings[]`, `consequences[]`, `seedable`, `seed_from[]` |
| `narrative_regions.json` | 6 hub→region rows + region weights (`air_weight`, `home_region`) | `hub_id`, `region_id`, `air_weight` |
| `story_event_seeds.json` | ~10 | `event_id`, `story_id`, `hub_pick`, `min_day` |
| `truth_predicates.json` | ~14 | `predicate_id`, `owner_read`, `unknown_as` |
| `story_lines.json` | ~40 | movement/band, `text` |
| `inquest_questions.json` | 12 questions x 3 findings + silences | `question_id`, `findings[]`, `exhibits[]`, `story_ref?`, `silence` |
| `inquest_exhibit_overrides.json` | as needed | `title`, `source_class`, `weight_override?`, `include?` |
| `inquest_lines.json` | ~30 | `kind`, `text` |

**Validator rules (added to the integrity pipeline; row-level failure output):**

- **N-V1** every hub in `rumor_hubs.json` appears in exactly one region row; region ids unique; a region with no hub is marked `no_hub`.
- **N-V2** every story's `truth_predicate` resolves in `truth_predicates.json`; every `regions` id resolves.
- **N-V3** every `consequences[].kind` is in {`lr_bias`, `standing`, `market_shock`, `chronicle`}; `at` in {`belief`, `certainty`}.
- **N-V4** `opposed_by` is symmetric across the whole catalog.
- **N-V5** consequence params bounded: standing delta within ±5; market shock within ±12% and ≤ 20 days; `petition_bias` within ±2.
- **N-V6** every PsyOps theme in `psyops_theme_map` names a theme that exists in `propaganda_campaigns.json`, and every story it names exists.
- **N-V7** all text ≤ 140 chars for claims and ≤ 200 for lines; no placeholder tokens; no real places, parties or people; no persuasion-technique wording (small deny-list: `manipulate`, `brainwash`, `gaslight`, `psychological operation`).
- **N-V8** every `seed_from` event id is in `story_event_seeds.json`; every seed's story exists and is `seedable` or an opposite.
- **LI-V1** every question has exactly 3 findings; every finding has ≥ 2 exhibits (titles) that resolve to a `world_history` record (or an override) at boot.
- **LI-V2** no finding lists an exhibit whose `discovery_trigger` is `ending_reached` (E14).
- **LI-V3** every capitalised noun phrase in a finding is a `world_history` title, a location display name, or on a small allowlist; no person of the Before is named.
- **LI-V4** every question has a non-empty Silence beginning `Not established here:` or ending with it; ≤ 120 chars.
- **LI-V5** motive/blame deny-list applied to findings, leads, desk-log lines and silences (§16).
- **LI-V6** each finding and silence is checked against the do-not-answer list (§25); a finding whose text matches a locked-silence tag fails.
- **LI-V7** findings ≤ 120 chars; ladder increases in specificity (finding 1 length ≤ finding 3 length is *not* required; each must be a single sentence).
- **LI-V8** a finding's `story_ref` (if any) resolves to a story in `stories.json`.

---

## 19. Acceptance criteria

**WW**

- **WW-A1** Grip matches the §6.2 worked example to the integer (25); bands follow the cuts; caps hold (G-4: no story reaches Belief on one channel).
- **WW-A2** Opposition uses the strongest opposing story's *raw* sum, never its net Grip (no circularity); symmetric pairs both computed.
- **WW-A3** Freshness: contributions vanish at age = 1/`DecayRate` (hub), 7 days (air), 20 days (hand); an intercepted rumour and an exposed telling contribute 0.
- **WW-A4** Each act creates exactly one ledger record and exactly one owner request, or none if the owner is absent (`Null*`); a pamphlet is recorded on **arrival**, never on printing.
- **WW-A5** Exposure follows §10.1 (percent formula, Half halves, True never exposed) and is stable across save/load (seeded per telling and week).
- **WW-A6** A story reaching Belief creates one `StoryTaken` and fires each consequence row exactly once per instance; falling below Talk ends it; a later rise creates a new instance.
- **WW-A7** With no ledger records and no seeds, every rumour, PsyOps, radio and market behaviour equals today's (parity).
- **WW-A8** Round-trip save; legacy save loads with an empty ledger; existing rumour, PsyOps and radio tests pass unchanged.

**LI**

- **LI-A1** Exhibits are exactly the discovered records; `ending_reached` records are never exhibits; an unavailable read yields an empty set (X-1).
- **LI-A2** Weights and classes follow §12.2 including truth-class adjustments and Suspect exhibits (weight 0).
- **LI-A3** Grade table (§13.3) holds for every boundary; the §13.4 example reproduces exactly (Rumoured, Rumoured, Attested, Corroborated, Corroborated, Established).
- **LI-A4** Effective grade is the lower of exhibit grade and sitting grade; with no sittings it is at most Rumoured.
- **LI-A5** Sittings: one per investigator-day, only if the investigator is free; leads appear once per (finding, missing exhibit) and never name an `ending_reached` record or leak an unreachable trigger.
- **LI-A6** The §14.6 timeline reproduces (Attested on day 300; leads on 302 and 303; Corroborated once Witness 3 is found and sittings ≥ 3).
- **LI-A7** A Statement of a Rumoured finding is refused; Corroborated or Established creates a Telling with `Honesty = True`, Attested with `Half`; once per audience per grade.
- **LI-A8** A Report prints all twelve Silences, including for questions with no line.
- **LI-A9** Contested is derived from the home region's story band and blocks nothing.
- **LI-A10** The motive deny-list and the do-not-answer check reject crafted bad rows (LI-V5, LI-V6).
- **LI-A11** Round-trip save; legacy save loads with no lines or reports; existing journal, archive, discovery and Verdict tests pass unchanged.

---

## 20. Cross-plan hooks (ship dark; `Null*` defaults)

| Hook | Direction | Default | Purpose |
|---|---|---|---|
| `IBroadcastChannel` | WW to RF station | `NullBroadcastChannel` (unavailable) | Air source and program booking |
| `IVoiceTrustSink` | exposure | `NullVoiceTrust` | Voice Trust cost |
| `IRegionNarrativeReader` | Living Region reads WW | none | petitions and wave bias |
| `IShockRequest` | consequence row | `NullShockRequest` | market nudge (`ApplyShock`, verified §2b) |
| `IStandingRequest` | consequence and exposure row | `NullStandingRequest` | standing delta |
| `ICarrierArrival` | Convoy Wars caravan arrival | `NullCarrierArrival` | pamphlet drops on arrival |
| `ICustodyReader` | Record Keepers | `NullCustodyReader` | exhibit condition and Gaps |
| `ISuspectExhibitReader` | Paper and Power | `NullSuspectReader` | Suspect exhibits |
| `IReportReader` | Evenings and Memory Work | none | read a Report at Founding Day |
| Treaty ratified / rail Running / relief dispatched | owner events to `story_event_seeds` | none | seeded stories |
| Quiet War door interest | Sensitive questions | none | reserved; not built here |
| Verdict | LI reads witnesses | none | ordinary exhibits |

Every hook is optional; the plan is complete with all defaults.

---

## 21. Packages

| Pkg | Scope | Depends |
|---|---|---|
| P0 | Premise audit: E7, E10, E17, E18, E19, E20, E21; resolve `knowledge_key` read; choose save homes; verify hub locations against LR region ids; record findings | none |
| P1 | Catalogs and loaders (narrative + inquest) + validators N-V1..8, LI-V1..8 | P0 |
| P2 | `NarrativeField` (Grip, bands, contest) + `StoryCatalog` + hub-region map | P1 |
| P3 | `NarrativeLedger` + Tellings + honesty + Tell/Print acts through `RumorSystem` | P2 |
| P4 | Exposure + taken-instances + consequence rows + event seeds (`INT`) | P3 |
| P5 | Field surface + Broadcast/Correct wiring (hooks) + briefing lines (`INT`) | P4 |
| P6 | `CaseFile` (exhibits, classes, weights, grades) + `InquestCatalog` | P1 |
| P7 | `InquestState` + Lines + Sittings + Leads + desk credit (`INT`) | P6 |
| P8 | Statements + Report + Contested + Silence rendering | P7, P3 |
| P9 | Inquest surface + save parity + tone review + handoff | all |

WW (P2..P5) and LI (P6..P8) may proceed in parallel after P1; P8 is the join (Statements create Tellings; Contested reads the Field).

---

## 22. Decision register (all unsigned; foreman/user signature required)

- **DEC-WW-01** The Narrative Field is a derived read over existing owners; one authored hub→region map. *Recommend yes.*
- **DEC-WW-02** Save home for the Narrative Ledger (nested in the rumour-network save). *Decide at P0.*
- **DEC-WW-03** Stories are abstract claims; no persuasion technique, no real-world content. *Binding.*
- **DEC-WW-04** Grip constants, caps and band cuts. *Tunable data.*
- **DEC-WW-05** Consequence rows and bounds. *Tunable data; bounded by N-V5.*
- **DEC-WW-06** Exposure is the only invented consequence; formula and cadence. *Recommend yes.*
- **DEC-WW-07** Grip is never shown as a number. *Recommend yes.*
- **DEC-WW-08** No new routed panel; extend the rumour board.
- **DEC-WW-09** The eight authored PsyOps campaigns become Air sources by map, uncatalogued edits. *Recommend yes.*
- **DEC-WW-10** Pamphlets recorded on arrival, not on printing. *Recommend yes.*
- **DEC-LI-01** The Inquest is a derived Case File plus a small stored act ledger; no revelation. *Recommend yes.*
- **DEC-LI-02** Save home for the Inquest State. *Decide at P0.*
- **DEC-LI-03** The Inquest never reads prose bodies and never names a person of the Before (C-1, LI-V3). *Binding.*
- **DEC-LI-04** Grade thresholds, weights and required sittings (5/8/11; 1/3/6). *Tunable data.*
- **DEC-LI-05** At most 4 lines open at once. *Tunable data.*
- **DEC-LI-06** Grades are words only; weights never shown. *Recommend yes.*
- **DEC-LI-07** Finding-to-story refs authored only where a true story fits (ship 2–3).
- **DEC-LI-08** No new routed panel; extend the archive surface.
- **DEC-LI-09** The do-not-answer list is a maintained data file cross-checked by validator (LI-V6). *Binding.*
- **DEC-LI-10** `ending_reached` records are never exhibits. *Binding.*
- **DEC-LI-11** A sitting fits the desk's job model, or one additive job kind is added to the desk. *Decide at P0.*
- **DEC-LI-12** Sitting credited by the plan's own tick; the archive desk is read only (§2b).
- **DEC-LI-13** Investigator aptitude = archivist assignment + Vivid/Clear clarity; no skill is invented (§2b).

---

## 23. Test plan and risks

**Focused tests (`bin/run-scoped-tests`), 45–55 total:**

- Grip: the worked example, per-channel caps, freshness curves, raw-sum opposition (no circularity).
- Bands: boundary table; the "Belief needs two channels" property.
- Acts: one record and one request per act; pamphlet on arrival; refusals with reasons.
- Exposure: formula table, Half halving, True immunity, stable roll across load.
- Taken-instances: begin, end, restart; exactly-once consequence keys.
- Seeds: owner events create tagged rumours through the rumour owner.
- Parity: empty ledger equals today's rumour, PsyOps and radio behaviour.
- Exhibits: discovered set, excluded endings, unavailable read, truth-class weights, Suspect.
- Grades: the §13.4 table (six states) and boundaries.
- Sittings and leads: the §14.6 timeline; lead once; no leak of unreachable triggers.
- Statements: refusal at Rumoured; honesty by grade; once per audience.
- Report: twelve silences; bounded history.
- Contested: derived from the home-region story; blocks nothing.
- Validators: N-V1..8, LI-V1..8 (catalog-level with row failure output); crafted-bad-row tests for LI-V5 and LI-V6.
- Save: round-trip and legacy load for both DTOs; parity guard runs existing rumour, PsyOps, radio, journal, archive and discovery tests unchanged.

| Risk | Likelihood | Mitigation |
|---|---|---|
| The Inquest fills a deliberate silence | Med | Silence printed on every question; LI-V3/V5/V6; do-not-answer list; no prose parsing |
| Findings read as an answer key | Med | Grades are slow, words only; leads name places, never reasons |
| Hub→region map disagrees with Living Region | Med | P0 verifies; ship-dark hook; working labels until then |
| Stories feel like a minigame | Med | No number shown; Belief needs two channels; every act is one survivor-day |
| A lie feels unpunished or over-punished | Med | Exposure formula bounded; costs are owner requests; silence is free |
| A second rumour/propaganda store appears | Low | Requests only; Rule 5 review at P9 |
| Discovery read for `knowledge_key` unavailable | Med | X-1 empty set; P0 stop condition |
| Save bloat | Low | Bounds X-8 |

---

## 24. Expansion backlog

- Faction-facing statements (a finding said to a power, with standing consequences).
- Rumour interception as a player act (spies; Quiet War hook).
- Counter-propaganda by NPC factions that reads the Field.
- A second Report format for the chronicle epilogue.
- Oral accounts as testimony exhibits with clarity from Memory Decay.
- Regional idioms: a region's *way of saying* a story changes over time (prose only).
- Cross-shelter Fields (a treaty partner's regions).
- Exhibits from the Deep (drifts) once the Deep Works records exist.
- Printing as a gameplay chain (paper and ink), replacing raw `paper_stock`.
- Player-authored (bounded, sanitised) headline text for pamphlets.

---

## 25. Open Mysteries and Deliberate Silence

### 25.1 The twelve printed Silences (fixed; never filled by code)

*Who signed the allocation. What was aimed at what. Who opened the door. Whether it should have gone on. What the last entry said. What failed first. What the Vessel was. Who did not sign. Who they were written to. What was read. Who drilled first. Who first said "us".*

### 25.2 The do-not-answer list (`inquest_do_not_answer.json`; machine-checked by LI-V6)

The Open Mystery Index (E15) locks these; **no finding, exhibit, lead or line may assert or imply them**:

| Tag | Locked silence (index id) |
|---|---|
| `olympus` | what Olympus was and why it stops at `DAY_5110 POST_BURST` (`SK-OM-1`, DEC-SK-04) |
| `after_day_720` | what happens after Day 720 (`Y2-OM-2`) |
| `maritime_explorer` | what the retired maritime exploration system knew (`DC-OM-5`, DEC-DC-07) |
| `final_seal` | what the final seal seals (`P7-OM-5`) |
| `first_rite` | what the first Rite of Passage would have been (`Y2-OM-3`) |
| `idioms` | what the unexplained idioms mean (`P8-OM-1`) |
| `last_bearer` | the name of the last bearer of a dying technique (`RT-OM-1`) |
| `index_case` | the index case of the outbreak (`PY-OM-1`) |
| `who_trains_infiltrators` | who trains the infiltrators (`QW-OM-1`) |

(The list is a **subset** extracted for this plan; P0 re-reads the index and completes it. Adding a tag is a *decision*, never a cleanup.)

### 25.3 Other silences held by this plan

- Whether any story in the Field is *true* is never stated unless a finding says so.
- Who plants the rumours that the world plants (event seeds) is never named.
- Whether the desk suggests a place because it knows or because it hopes is never asked.
- What the twelve questions would add up to, read together, is not a thirteenth question. There is no thirteenth.
- Whether a Report is *finished* is never stated. A Report is the desk on that day.
- Why the crossroads bench was chosen as the place to say it. The bench is the bench.

---

## 26. Pre-flight, verification and stop conditions

**Pre-flight (before any edit):** read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, `AI_AGENT_WORKFLOW.md` and `OPEN_MYSTERY_INDEX_2026-09-29.md`; confirm claims for every path in section 4; complete P0 and record findings.

**Verification:** run only focused targets via `bin/run-scoped-tests` for the changed files; run `bin/ashfall-dev validate-config` for the new catalogs; a Godot headless check only if a panel or briefing wiring is touched (15 FPS); never the full suite without `RUN FULL TESTS`.

**Stop and report to the foreman if:**

- a path in section 4 is claimed by another owner;
- P0 shows no public read exists for a `knowledge_key` and none can be added without editing the journal owner;
- `RumorSystem` cannot accept a caller-chosen origin hub and honesty without a signature change on the owner;
- any change would edit a `world_history` record, a PsyOps campaign, a hub's credibility or a rumour's `Truthfulness`;
- any finding, lead, desk line or silence would assert a locked silence, name a person of the Before, or give a motive;
- an `ending_reached` record would become reachable through the Inquest.

**Handoff:** outcome, files, contract, commands and results, limitations, shared paths intentionally untouched, per `AI_AGENT_WORKFLOW.md`. On integration, mark this file FULLY INTEGRATED at the top (multiple times) and move it to `.ai/plans/integrated/<category>/`.
