# Feature / Task Plan: The Cabinet of Ordinary Things (a display of the before's small objects) & The Dig (domestic archaeology under a prewar floor)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 3, plans 3 of 4 (subjects 37–38).

> **Subjects covered (2 of the 8 in the "gatherings and signals" batch):**
> 37. **The Cabinet of Ordinary Things** — a shelf of the before's smallest objects, displayed,
> labelled, and never explained: the shelter's museum of the unremarkable. (Prefix `CB`.)
> 38. **The Dig** — domestic archaeology under a prewar floor: strata of ordinary life lifted a
> layer at a time, through the existing archive-decryption and ruin seams. (Prefix `DG`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `CulturalArchiveVaultSystem` and `PrewarArchiveDecryptionSystem` (both live — see
> `docs/plans/PLANS_FLAGSHIP_INSTITUTIONS_T5_8_IMPLEMENTATION_LOG.md` and
> `.ai/plans/plans-62-64-65-runtime-wiring-2026-09-29.md`), `.ai/plans/second-nature-and-ruins-of-the-before-2026-09-29.md`
> (ruin sites, rooms, exhibits; its held silences bind this plan), `.ai/plans/war-of-words-and-long-inquest-2026-09-29.md`
> (exhibits and chains of custody), `.ai/plans/reconstruction-tree-2026-09-29.md` (fragments are
> citations; knowledge recovery), `.ai/plans/record-keepers-2026-09-29.md` (custody of records).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Shelf and the Floor

> *"A museum keeps what was important. A cabinet keeps what was ordinary. The second one tells
> you more."*

Nobody puts a monument in the cabinet. The cabinet is for the things the before used without
thinking: a key to a door that is not standing, a tin opener, a sock with a darn in it, a
birthday candle, a shopping list in a handwriting. The shelter's archive vault already knows how
to keep things; this plan gives it a *shelf* — an authored display where ordinary objects are
labelled in plain language and never explained, because explaining them would be a guess wearing
a curator's coat.

The Dig is where the shelf's objects come from. Under the floor of a prewar house there are
layers: boards laid over boards, a hearth with three ashes, a child's height marked on a door
frame that is now indoors. The Dig lifts those strata one at a time — through the existing
archive-decryption seam, at the existing ruin sites, with no new map and no new interior — and
what it lifts either becomes an object, a fragment, or a silence in the record.

**Tone & register.** Domestic archaeology: patient, exact, and unmawkish. The vocabulary is the
kitchen drawer and the trowel: *shelf, label, stratum, lift, drawer, listed, plain*. Prose should
read like a volunteer museum guide who has decided that the small things deserve the same
grammar as the big ones. Never sentimentalise an object. The label's plainness *is* the tribute.

**Mystery & texture.** Three silences hold the pair. The labels describe *use* and never
*purpose*: the label may say "a tin opener" and may never say what it meant to open tins for.
Some objects have labels describing uses nobody at the shelter recognises, in confident plain
language (§12). And the dig's deepest stratum — the one beneath the hearth — is not an object
layer at all (§12). None of these is a puzzle. They are the before being *ordinary* at a scale
the shelter cannot reach.

**The second layer.** An archive keeps what was important; a cabinet keeps what was *used* — and
used things are where a civilisation actually lives. The plan's quiet thesis is that grief for
the before is best conducted in the register of housewares: no heroes, no dates, just the
objects that were in a drawer when the drawer stopped being opened. The Dig supplies the cabinet
with its specimens, and the cabinet supplies the dig with its only justification — the floor is
worth lifting because the shelf is worth filling, and the shelf is worth filling because someone
will stand in front of it and recognise a tin opener.

---

## 1. Goal & Outcome

> *Design intent: the player should be able to label an object "for keeping bread soft, we
> think" and feel the difference between knowing and guessing.*

### 1.1 The Cabinet of Ordinary Things (CB)

- **Goal:** An authored **Exhibit Table** (≤ 24 rows: object id, plain label, source site, optional
  `use_unknown` flag) surfaced through the existing `CulturalArchiveVaultSystem` display seam;
  one verb — **Label** (player text constrained to a closed label vocabulary plus a small
  sanctioned word list, per DEC-CB-03); and a **Shelf State** read model (which slots are filled,
  which are empty, which labels are guess-marked). Items exhibited remain owned by the inventory
  owner; exhibition is a *display reference*, never a transfer (DEC-CB-01).
- **Outcome (observable):** on a fixed seed, an object lifted at a dig site appears as an exhibit
  row; its label renders in plain language with an optional guess mark; the shelf state derives
  from present rows; removing an exhibit returns it to ordinary inventory state with no residue;
  with no exhibit rows the vault behaves identically to today; save/load mid-display round-trips.
- **Non-Goals:** no new archive/knowledge authority (`CulturalArchiveVaultSystem` keeps its
  meaning); no valuation or appraisal; no collectible mechanics (Flagship XII owns collection);
  no restoration/cleaning verbs; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Dig (DG)

- **Goal:** An authored **Strata Table** per diggable site (3–5 layers per site, closed vocabulary:
  `boards`, `ash`, `silt`, `under_hearth`, each with a lift cost in days through the existing
  expedition/labour seam), a **Lift** verb that draws seeded finds from the site's own authored
  find table through the existing acquisition-source enum, and a **Dig Log** of one line per lift
  (site, layer, finds, day). Finds are exactly three kinds: *object* (→ cabinet), *fragment* (→
  Reconstruction Tree's existing fragment seam, one optional hook), *silence* (a recorded layer
  with nothing in it — a first-class outcome).
- **Outcome (observable):** on a fixed seed, lifting `boards` at a site yields an authored find
  set; the object becomes an exhibit row; the fragment is offered to the RT seam unchanged; the
  silence layer records "empty, dated" and yields nothing; `under_hearth` behaves as authored and
  no stratum below it exists in any table; with no strata rows every owner behaves identically to
  today; save/load mid-lift round-trips.
- **Non-Goals:** no new map/interior/excavation physics (The Deep Works and SN/RB own ground); no
  dating methods, no provenance science; no knowledge mechanics (RT owns knowledge); no evidence
  chains (Long Inquest owns custody of proof); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented find-kind boundary and one optional hook: an object find *becomes* an
  exhibit row through the vault's own display verb; a fragment find is offered to RT's seam and
  refused-or-kept by RT's rules. Neither half reads the other's state; both are shipped dark until
  the receiving owner's verb is confirmed at P0.

---

### 1.4 Worked examples (non-normative)

**A dig week at `loc_house_7` (fixed seed).**

> Day 41 — Lift 1, `boards`, one survivor-day. Find draw: object `cabinet_object_11` (tin opener),
> fragment `frag_rt_07` offered to the RT seam (kept, node 14). Log: "Lift 1, boards layer: one
> object, one fragment, dated."
>
> Day 42 — Lift 2, `boards`. Find draw: silence. Log: "Lift 2, boards layer: empty, dated. The
> emptiness is filed."
>
> Day 45 — Lift 3, `silt`. Find draw: object `cabinet_object_19` (door frame, sawn out whole) —
> the only find bigger than a drawer. Log notes the size and stops.
>
> Day 46 — the worker looks at `under_hearth`. The lift verb refuses with reason
> `stratum_terminal`; the log records the date of the refusal and nothing else.

**A label at the shelf.**

> Object 11 is exhibited through the vault's own display verb; it remains inventory-owned. The
> label is written in the sanctioned vocabulary: "for opening tins". An unrecognisable object's
> card renders with the guess mark: "for keeping something closed — *we think*" — same type,
> smaller, italicised. Un-exhibiting the object returns it to ordinary inventory with no residue;
> the shelf's slot becomes empty and may carry a card.

---

## 1b. Texture, Mystery & Voice

**Labels are guesses with good manners.**

The `use_unknown` flag and the guess mark are the plan's ethics in two fields. A label may
describe a *use* ("for keeping bread soft") and must never assert a *purpose* ("for a family that
baked"). The sanctioned word list exists to make that discipline enforceable in the fiction as
well as the code: the player labels in plain nouns and plain verbs, and the prose treats every
label as provisional.

**Strata are a stack of ordinary days.**

Boards over boards is not history; it is *housekeeping*. Each layer's find density should feel
like a drawer: mostly nothing, occasionally everything. The silence layers are load-bearing —
an empty stratum is a *dated* emptiness, and dated emptiness is the archaeology's whole voice.

**What the player is never told.**

- What the unrecognisable objects were for. Their labels are confident and their uses are not in
  any authored vocabulary (§12). The plan records the confidence and declines to translate it.
- Whether the label vocabulary is the before's or the shelter's. `use_unknown` is a guess mark,
  not a language tag; whose words these are is not modelled.
- What is beneath `under_hearth`. The strata table ends there by construction. The deepest layer
  is a floor of the *table*, not of the world.
- Who marked the child's height on the door frame. It is recorded as an object. It is not a
  quest, a name, or a trace of anyone.

**Voice — sample fragments (content candidates for `cabinet_lines.json` / `dig_lines.json`).**

> "Shelf 3: for keeping something closed. The label is honest about the something." — cabinet
> guide (CB)

> "Object 11, listed plain: a tin opener. Somebody's hands used this weekly. The weekly is the
> part the shelf cannot hold." — cabinet guide (CB)

> "Lift 2, boards layer: one object, two fragments, and a date for the emptiness around them." —
> dig log (DG)

> "Under-hearth entry, dated, sealed at the layer. The dig stops where the table stops. The
> floor under the floor is not ours to lift." — dig log (DG)

> "Door frame, marked in pencil: two heights and a year. Listed as one object. It is the only
> exhibit that is also a measurement of somebody." — dig log (DG)

**Design texture beats.**

- **A guess mark is not a flaw; it is a genre.** The whole shelf should read as *careful
  inference*, and the UI must never let a guess wear the typography of a fact.
- **Silence layers are findings.** Award them the same log treatment as finds; an archaeology
  that records only objects is a treasure hunt.
- **Exhibition is a reference, not a transfer** (DEC-CB-01). The moment the shelf owns things,
  it becomes an inventory and the cabinet becomes a shop window.
- **The dig stops where the table stops.** Do not extend the strata to feed the shelf; the
  shelf's insatiability is the player's problem, not the data's.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the shelf leaves lying around.**

> "Label card: 'for keeping bread soft, we think.' The 'we think' is in every card's hand and on
> none of the printed ones."

> "Shelf 1, slot 4: empty, labelled anyway. The empty slot has a card. The card is the exhibit."

> "Object 7: a key. Label: 'for a door not standing.' The key is on the shelf. The door is not
> on any map."

**What the floor leaves lying around.**

> "Stratum sheet, boards layer: lifted, dated, sifted. Two objects and a pencilled note —
> 'someone repaired this twice.'"

> "Ash layer, third lift. Three hearth-fires in one hearth, one above the other. The middle one
> is the smallest. The sheet records the sizes and stops."

> "Door frame, sawn out whole. It is the only object the dig has ever lifted that was bigger
> than a drawer."

**Held silences (texture, not register rows).**

- What the confident labels for unrecognisable objects describe. The uses are plain in the
  fiction's own register and absent from every vocabulary (§12); the confidence is real and the
  referent is not. Texture only.
- What the hearth-fires' three sizes were about. The sheet records sizes and stops; whether
  anyone cooked differently in the middle period is not modelled and must not be.

**Third pass — the plain label (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: texture only — no authority, path, decision, acceptance criterion
or verification step. §12 gains no row and loses no silence.)*

> "Guide's tour, shelf 2: 'and this was for a game we don't play.' The guide says it every time.
> Nobody has ever asked the rules."

> "Lift 4, silt: a shoe, one only. The dig log notes 'one only' and the note is the whole eulogy."

> "The empty slot's card is the only exhibit that needs no dusting."

**Held silences (texture, not register rows).**

- Whether the confident labels and the unrecognisable objects are the same mystery wearing two
  coats. The plan observes both and connects neither. Texture only.
- Who taught the dig to stop at the hearth. The table stops; whether the habit of stopping is
  older than the table is not modelled and must not be.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | `CulturalArchiveVaultSystem` exists and is host-wired (Flagship Institutions T5–8). | `docs/plans/PLANS_FLAGSHIP_INSTITUTIONS_T5_8_IMPLEMENTATION_LOG.md` | LIVE (closeout) |
| E2 | `PrewarArchiveDecryptionSystem` (Plan 62) exists and is now invoked at runtime via `plans_62_65` day owner. | `.ai/plans/plans-62-64-65-runtime-wiring-2026-09-29.md` | LIVE (approved plan) |
| E3 | Ruin sites exist as expedition nodes with authored rooms/details; **no interiors modelled**; two id namespaces (`location_*`, `loc_*`). | SN/RB findings (its §1b/§1c; "a location is one expedition node with no interior") | LIVE (per corpus) |
| E4 | Nine Inquest exhibits already staged at five sites; exhibits have chains of custody and "Not established here" discipline. | `.ai/plans/war-of-words-and-long-inquest-2026-09-29.md` (E-notes); SN/RB finding | LIVE (per corpus) |
| E5 | Reconstruction Tree fragment seam: `{fragmentId, nodeId, foundDay, source}` reusing the acquisition-source enum; fragments are citations, not items. | `.ai/plans/reconstruction-tree-2026-09-29.md` §1b / RT-P3 | PROPOSED (plan is DRAFT) |
| E6 | Record Keepers custody overlay: medium, place, condition, copies, state; gaps tagged; no record body text. | `.ai/plans/record-keepers-2026-09-29.md` RK-P1 | PROPOSED (plan is DRAFT) |
| E7 | Inventory owner can express "exhibited but still owned" as a display reference; collectibles exist with narrative quality (Flagship XII). | `docs/plans/flagship_xii_collectibles_IMPLEMENTATION_LOG.md`; inventory owner | **VERIFY (P0)** |
| E8 | `codex_entries.json` exists and can carry glossary-like rows; label text fields must land in data, never code. | umbrella F16 (`codex_entries.json` cited) | **VERIFY (P0)** |
| E9 | Expedition/labour seam can express a lift cost in days at a site (per-day cost precedents: Trace (WB) one survivor-day; load-test crews). | WB plan §1.1; IR plan §1.2 | **VERIFY (P0)** |
| E10 | Additive nested DTO checksum-safety in the vault/expedition save owner for shelf state and dig logs. | codec / snapshot tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Archive vault display | `CulturalArchiveVaultSystem` | exhibit rows via its own display verb; no vault state invented |
| Decryption / prewar records | `PrewarArchiveDecryptionSystem` | read-only context; a dig layer may cite a decryption as a *source*, never the reverse |
| Sites, rooms, ground | SN/RB + excavation owners | strata table keyed to existing site ids; no ground model |
| Knowledge fragments | Reconstruction Tree | one optional offer hook; RT keeps all accept/refuse rules |
| Records custody | Record Keepers | an exhibit may *reference* a custody row; RK's decay rules untouched |
| Items | inventory owner | objects remain owned while exhibited (DEC-CB-01) |
| Exhibit/label/dig state | — | `CabinetShelf` + `DigStrata` (pure Core), nested additively — **DEC-CB-02 / DEC-DG-02** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Archive/CabinetShelf.cs` (new, pure), `Archive/DigStrata.cs` (new, pure), `Archive/DigLog.cs` (new, pure)
**Data:** `cabinet_exhibits.json`, `dig_strata.json`, `dig_find_tables.json`, `cabinet_lines.json`, `dig_lines.json`
**Host:** vault host session (`INT`), expedition host session (`INT`), day-owner registration (`src/Main.CampaignOwners.cs`, `INT`)
**Presentation:** a "Cabinet" band on the existing vault/archive surface and a "Layers" view on the existing site surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Archive/CabinetShelfTests.cs`, `DigStrataTests.cs`, `DigLogTests.cs`, `Ashfall.Core.Tests/Save/CabinetDigSaveTests.cs`

## 5. Packages

### CB-P0 — Premise audit (Auditor; read-only): close E7–E10; confirm the vault display verb and the label-text destination (`codex_entries.json` or equivalent); confirm exhibition-as-reference wording with the inventory owner.
### CB-P1 — Exhibit table + shelf state (Core + data): ≤ 24 rows, `use_unknown`, guess-marked labels, closed label vocabulary + sanctioned word list (DEC-CB-03), validator row-level. **Accept:** round-trip; no rows → vault unchanged; label text lives in data only.
### CB-P2 — Label verb + shelf band (host): constrained input, guess mark rendering, exhibit/unexhibit through the vault verb. **Accept:** items remain inventory-owned; removal leaves no residue.
### DG-P1 — Strata table + lift verb (Core + data): 3–5 layers per site, closed layer vocabulary, seeded find draws, dig log. **Accept:** determinism; `under_hearth` is terminal by construction; silence layers recorded as findings.
### DG-P2 — Find routing (host): object → exhibit row; fragment → RT offer hook; silence → log only. **Accept:** each route ≤ one owner call; RT refuses-or-keeps by its own rules.
### X-P1 — Seam hooks (host, shipped dark): dig→cabinet and dig→RT boundaries as documented booleans/enums. **Accept:** no cross-reads; hooks dark until P0 confirms receiving verbs.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no exhibit rows and no strata rows → vault, expedition, inventory, RT and RK outputs identical on a saved corpus.
3. Conservation: objects remain inventory-owned while exhibited; every find leaves the site's table exactly once; nothing is created by a lift.
4. Determinism: identical find sets, layer orderings and log contents on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip mid-lift and mid-display; old saves load; additive DTO checksum-safe (E10 closed at P0).
6. Label discipline by construction: no label text in code; no `purpose` assertions; guess marks render differently from printed labels (DEC-CB-03).
7. Silence layers are first-class findings with dated log entries (DEC-DG-01).
8. `under_hearth` is terminal; no authored content may exist below it (DEC-DG-03) — and the SN/RB ruin silences stay unanswered (§7).

## 7. Cross-plan boundaries
- **Second Nature / Ruins of the Before:** SN/RB owns surfaces, rooms and trusts (its held silences — *what the reading hall was for on its last roofed day* — stay unanswered here; this plan adds strata *under* floors and nothing about the rooms themselves).
- **The Long Inquest:** the Inquest owns exhibits-as-evidence and chains of custody; cabinet objects are display, never attested evidence. A dig find may sit at a site that hosts an Inquest exhibit without touching it.
- **The Reconstruction Tree:** fragments are offered and RT decides; the dig never recovers knowledge itself.
- **The Record Keepers:** custody rows may be referenced; RK's decay and gap rules are untouched.
- **Flagship XII (collectibles):** a collectible may optionally be exhibited (display reference only); collection mechanics stay XII's.
- **The Deep Works:** ground, drifts and collapse are theirs; the dig digs *under floors*, not into rock.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-CB-01 | Exhibition is a display reference; items remain inventory-owned at all times. | rule | Yes |
| DEC-CB-02 | Shelf state nests additively in the vault save owner; no new save section. | architecture | Yes; confirm in P0 |
| DEC-CB-03 | Labels use a closed vocabulary + sanctioned word list; `use_unknown` renders a guess mark; no purpose assertions. | tone/rule | Yes |
| DEC-CB-04 | An empty slot may carry a card; absence is exhibit-worthy. | design | Yes |
| DEC-DG-01 | Silence layers are findings: dated, logged, rewarded like finds in log treatment. | rule | Yes |
| DEC-DG-02 | Dig state nests additively in the expedition save owner; no new save section. | architecture | Yes; confirm in P0 |
| DEC-DG-03 | `under_hearth` is terminal in the data; nothing below it exists in any table, ever. | tone | Yes — **needs canon note** |
| DEC-DG-04 | Finds are exactly three kinds (object / fragment / silence); the enum is closed. | rule | Yes |
| DEC-X-01 | Both find-routing hooks ship dark until the receiving verbs are confirmed at P0. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Cabinet`, `Exhibit`, `Museum`, `Strata`, `Dig`)
- [ ] Premise re-verified (Rule 7); SN/RB, Long Inquest and RT evidence re-read; their registered silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-CB-01, DEC-DG-03)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing vault, expedition, RT-fragment and RK tests (list from P0 selector)
- [ ] Vault and expedition host selftests (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: exhibition cannot be a reference without transferring ownership; label text cannot be kept out of code; a stratum below `under_hearth` would be required by any authoring; a find cannot be routed without reading RT/RK state; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the before ordinary at a scale the shelter cannot audit. Any future plan
that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| CB-OM-1 | What are the unrecognisable objects for? | Their labels are confident and their uses are outside every authored vocabulary (§12). Translating them would end the shelf's best silence. | Never — deliberately sealed. |
| CB-OM-2 | Whose words are in the label vocabulary? | `use_unknown` marks a guess, not a language; whether the labels speak the before's words or the shelter's is not modelled. | Canon owner only, as a signed decision. |
| CB-OM-3 | Is the cabinet a museum or a shrine? | DEC-CB-04 lets absence be exhibited; the plan refuses to name the register the shelf is kept in. | Never — tone-locked. |
| CB-OM-4 | Who chose the first object worth a shelf? | Provenance of the *display* is not authored; the shelf's founding is as unrecorded as the chain's (batch-3 index). | Canon owner only. |
| CB-OM-5 | What is on the card in the empty slot 4? | The card exists and its text is one of the shelf's plain mysteries; authoring it would fill a silence that is doing structural work. | Never — texture by omission. |
| DG-OM-1 | Who lived in the house? | The door frame measures two heights; naming the household converts an object into a biography. | Never — tone-locked by DEC-DG-03's boundary. |
| DG-OM-2 | What is beneath `under_hearth`? | The table ends there by construction (DEC-DG-03). The floor under the floor is the dig's permanent silence. | Never — deliberately sealed. |
| DG-OM-3 | Why is the middle hearth-fire the smallest? | Sizes recorded, cause unrecorded (§1c). The plan observes and declines to infer. | Never — texture by omission. |
| DG-OM-4 | What was the reading hall for on the last day it had a roof? | **Bound by SN/RB's own register** — that question belongs to the ruins plan and is unresolved there. This plan must not answer it from beneath the floor. | SN/RB's owner only (its register governs). |
| DG-OM-5 | Who repaired the twice-repaired object, and why twice? | The pencilled note is the whole record; motive is not modelled and the mending's story belongs to the Mending plan's own register (batch-3, plan 4). | The MN plan's owner, if ever linked. |
