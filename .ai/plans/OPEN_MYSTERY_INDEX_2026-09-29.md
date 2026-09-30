# ASHFALL — Open Mystery Index

**Cross-reference index. NOT A PLAN.** This file has no `STATUS:` line, claims no paths,
owns no code, and is not subject to the plan lifecycle. It is a read-only companion to the
prose-pass registers written into the non-integrated plan corpus on 2026-09-29.

> Every plan in the corpus now ends with an **Open Mysteries & Deliberate Silence** register
> (narrative plans) or an **Open Items & Deliberate Limits** register (technical plans).
> This document collects the *threads that run between them*.

---

## 0. Why this exists

A single unanswered question is a gap. Two hundred of them, held deliberately and recorded
consistently, are a **world**.

The registers in each plan were written to one rule: *a question that is intentionally
unanswered is not a bug, a TODO, or deferred work — it is the thing that keeps the fiction
larger than the player's ability to audit it.* Left scattered across 38 files, that rule is
easy to violate: the next agent to open one plan sees silence and reaches to fill it.

This index makes the silences **legible as a design**, so nobody fills one by accident.

---

## 1. The corpus

**38 registers · 219 recorded open questions · 39 plans polished in the 2026-09-29 prose pass.**
**Second pass: +30 registers in `docs/expansions/` (prose companions, creative packs, family
indexes) · 69 documents total · see §6.**

| Prefix | # | Plan | Family |
|---|---|---|---|
| `TD` | 6 | The Deep | Shelter Under Pressure |
| `DW` | 6 | The Deep Works | Shelter Under Pressure |
| `LS` | 6 | The Long Siege | Shelter Under Pressure |
| `RW` | 6 | The Ration Wars | Shelter Under Pressure |
| `RK` | 6 | The Record Keepers | Shelter Under Pressure |
| `QW` | 6 | The Quiet War | New Ways to Play |
| `UW` | 6 | The Underworld | New Ways to Play |
| `RF` | 6 | Radio Free Ashfall | New Ways to Play |
| `RT` | 6 | The Reconstruction Tree | New Ways to Play |
| `FS` | 6 | Faith and Schism | New Ways to Play |
| `SG` | 6 | Shelter Governance | New Ways to Play |
| `CC` | 6 | Crews and Companions | New Ways to Play |
| `LR` | 6 | The Living Region | World Moves Without You |
| `LF` | 6 | The Long Line: Freight | World Moves Without You |
| `PY` | 6 | The Plague Year | World Moves Without You |
| `DC` | 6 | The Drowned Coast | World Moves Without You |
| `SK` | 6 | The Sky (Orbital Harrow) | New Pressures and Places |
| `Y2` | 6 | Year Two: The Long Thaw | Year Two (umbrella) |
| `P0`–`P9` | 47 | Year Two package cards | Year Two |
| `PB`/`PH`/`PS` | 15 | Performance, host, save recovery | Performance & QoL |
| `UI1`–`UI4`/`UIT`/`UIF` | 34 | Accessibility audit-fix series | UI Accessibility |

---

## 2. The six threads

### 2.1 — The Artefacts (things that read as history)

Some inconsistencies were **found**, not authored: an unused enum slot, a hard-coded table, a
cap that may be a wound, a field with no reader. In every case the register declines to explain
the origin — because the artefact is more interesting as a trace than it would be as a story.

| ID | The artefact | Register |
|---|---|---|
| `DW-OM-4` | `ExpansionTunnel = 2` exists, unused, already named exactly right | The Deep Works |
| `FS-OM-5` | Opposing belief pairs live in a **static in-code table** | Faith and Schism |
| `SK-OM-2` | `is_false_positive` — two of twelve orbital events designed to waste your attention | The Sky |
| `SG-OM-5` | Exactly four laws, and none for Hoarding or Desertion | Shelter Governance |
| `RW-OM-6` | Exactly five Table Rules | The Ration Wars |
| `UI1-OM-2` | Core advertises `Shortcut = "1"` strings **zero consumers** implement | UI A11Y P1 |
| `PH-OM-5` | Why the `Main.Plans*.cs` bundles were mixed in the first place | Plan 2 |
| `P6-OM-1` | The four legacy outposts are `tether` — to what is not stated | Year Two P6 |
| `P4-OM-6` | A succession clock that runs 365 days and is never read | Year Two P4 |
| `LF-OM-5` | The season closes between Day 280 and 360, for no stated reason | The Long Line |

> **Rule for future agents:** if an artefact in this table is "fixed", the fix must be recorded
> as a *decision* in that plan's decision register — never as a cleanup.

---

### 2.2 — The Named and the Nameless

The corpus is full of people the game declines to identify: last bearers, drivers, runners,
quartermasters, jurors, infiltrators, keepers. In every case the mechanism returns a **status**,
never a **name**.

| ID | Whose name is withheld | Register |
|---|---|---|
| `RT-OM-1` | The last bearer of a dying technique | The Reconstruction Tree |
| `QW-OM-1` | Whoever trains the infiltrators | The Quiet War |
| `RK-OM-4` | Who keeps the Keeper | The Record Keepers |
| `RW-OM-1` | Who skimmed the pantry | The Ration Wars |
| `LF-OM-2` | Who is driving the wagon | The Long Line |
| `LS-OM-4` | Whether the captured runner talked | The Long Siege |
| `UW-OM-3` | Who taught the hunters the four-leg form | The Underworld |
| `SG-OM-6` | Who sits a jury | Shelter Governance |
| `PY-OM-1` | The index case of the outbreak | The Plague Year |
| `P4-OM-1` | Why someone chose *leave-unwritten* | Year Two P4 |

> **Rule:** a system may return a state. It may not return a biography.

---

### 2.3 — The Deliberately Locked

These are silences held **by signed decision**, not by omission. Touching them requires a
foreman signature and a named decision ID.

| ID | Locked silence | Governing decision |
|---|---|---|
| `SK-OM-1` | What Olympus was; why it stops at `DAY_5110 POST_BURST` | DEC-SK-04 |
| `LR-OM-1` | Why there is **no "Thriving" rung** | DEC-LR-03 |
| `FS-OM-1` | Whether the Schism stage cap is a guard or a wound | DEC-FS-05 |
| `Y2-OM-2` | What happens after Day 720 (no Chapter Three) | Umbrella Non-Goals |
| `Y2-OM-3` | What the first Rite of Passage would have been | Umbrella §1 (F7 age floor) |
| `P2-OM-3` | Why there is no third button | P2 scope |
| `P8-OM-1` | What the unexplained idioms mean | P8 Non-Goals |
| `P7-OM-5` | What the final seal seals | P7 scope |
| `DC-OM-5` | What the retired `MaritimeExplorationSystem` knew | DEC-DC-07 |
| `UI1-OM-3` | Whether `ModalManager` should ever be wired | Deferred to governance |

---

### 2.4 — The Engine's Edge

Boundaries the tools themselves impose. These are the only registers where "we could not" is a
complete and sufficient answer.

| ID | The limit | Register |
|---|---|---|
| `UI4-OM-1` | Godot has **no hovered-item `ItemList` stylebox** | UI A11Y sidebar |
| `UI4-OM-2` | `SpinBox` arrow theming, `RichTextLabel` link hover | UI A11Y sidebar |
| `UI3-OM-2` | The engine consumes Tab before the unhandled phase | UI A11Y P3 |
| `UIF-OM-1` | Snapshot goldens cannot be regenerated headless (`headless_compatible: false`) | Font-size lift |
| `PERF-OM-4` | Profiles ran on llvmpipe at 15 FPS — never a GPU-backed 60 FPS target | Performance QoL |
| `PH-OM-3` | *"A successful compile alone does not prove lifecycle integration."* | Plan 2 |

---

### 2.5 — The Honest Limits (what measurement could not establish)

Where the corpus refuses a flattering claim. This is the technical plans' equivalent of a
mystery: the edge of the knowable on a given date.

| ID | What was not established | Register |
|---|---|---|
| `PB-OM-1` | Whether excluding selftests made the game *faster* | Plan 1 |
| `PB-OM-2` | Whether the byte reduction is attributable to the exclusion | Plan 1 |
| `PS-OM-1` | **Progress that has never been saved is unprotected** | Plan 3 |
| `PS-OM-2` | Protection from *every* filesystem or power-loss failure | Plan 3 |
| `PERF-OM-1` | Unexplained long stalls in the after profile | Performance QoL |
| `PERF-OM-2` | Startup 10.11s→11.63s, frame 66.7ms→109.1ms — *noisy captures, not claimed gains* | Performance QoL |
| `PERF-OM-3` | The before/after PCK comparison was **abandoned** (different snapshots) | Performance QoL |
| `PH-OM-1` | *"Missing local callers alone do not prove dead code."* | Plan 2 |
| `UIT-OM-3` | Whether the 23 audit-enumerated files are the whole surface | Target sizes |
| `UI2-OM-5` | Whether any *unlisted* color is below 3:1 | UI A11Y P2 |

---

### 2.6 — Rules, Not Gaps

Questions whose answer is "never — this is a rule." Worth reading separately, because these are
the ones most likely to be mistaken for oversights.

| ID | The rule | Register |
|---|---|---|
| `TD-OM-3` | What the vats purge is never named | The Deep |
| `RK-OM-1` | What the Lost record said — the ledger holds **no record body text** | The Record Keepers |
| `UW-OM-4` | Are there other debt ledgers? The combined view does not assert totality | The Underworld |
| `QW-OM-3` | Whether tells rehearse or reflect the truth | The Quiet War |
| `CC-OM-3` | Who names the party | Crews and Companions |
| `LR-OM-5` | Does the Pulse know about the shelter? It never takes one as input | The Living Region |
| `P4-OM-5` | Whether a raised child owes the shelter anything | Year Two P4 |
| `P3-OM-3` | Whether Standing describes the shelter or the verdict | Year Two P3 |
| `UIT-OM-4` | Why non-interactive sizes are excluded — *a label is not a target* | Target sizes |
| `UI4-OM-6` | Why hover arrives with keyboard activation — all three, or none | UI A11Y sidebar |

---

## 3. Pairs that must stay unresolved **together**

Several silences are shared between plans and are only safe if neither side fills them alone.
These are the most dangerous to "fix" unilaterally.

| Shared silence | Plans | Rule |
|---|---|---|
| **The "signal interceptor"** — one person, or a technique? | `QW-OM-2`, `RF-OM-6` | Resolve jointly or never. Answering in one plan steals the other's dread. |
| **The shared gate adapter** at the shelter door | `LR`, `QW`, `PY`, `CC` | Agreed once (index §4 of *New Ways to Play*), used by four plans. No second gate. |
| **Whether the runner talked** | `LS-OM-4` → QW via `DEC-LS-09` | The Siege hands the leak over *through its public seam only* and without comment; the Quiet War decides whether to notice. |
| **Countermine / sap** | `LS-OM-2`, `DW-P7` | The Siege owns the action; the Works owns the drift and the collapse. Neither may narrate the other. |
| **The four Standing readings vs the Year Two Chronicle** | `P3-OM-1`, `P7-OM-4` | Four voices are never reconciled; the Chronicle takes Standing as input. Disagreement is by design. |
| **What a hearth is** | `P5-OM-3`, `Y2-OM-4` | Binds bunks to a location and stops. Custody of meaning is not asserted. |
| **Where redirected refugees go** | `LR-OM-2`, `PY-OM-4` | Population is *conserved*, not narrated. The ledger does not follow anyone. |

---

## 4. How to use this

1. **Before writing a plan**, read §2. It will stop you from authoring an explanation the corpus
   has deliberately withheld.
2. **Before "fixing" anything in §2.1**, check the governing decision in §2.3. If one exists, it
   needs a signature, not a patch.
3. **Before closing a silence**, check §3. If it is shared, the other plan must be in the room.
4. **After adding a register row**, add the plan to the table in §1 and — if it belongs to a
   thread — to the relevant subsection here.

---

## 5. Provenance and standing

- Written: 2026-09-29, as the closing artifact of the plan prose-polish pass.
- Sources: the `## 12. Open Mysteries & Deliberate Silence` / `## 6. Open Items & Deliberate
  Limits` sections of the 38 registers listed in §1.
- Authority: **none.** This index records what the plans say. Where it and a plan disagree, the
  plan wins.
- Not a claim. Not an approval. Not a decision. Registering a silence here does not authorise
  anyone to answer it.

---

## 6. The prose layer — `docs/expansions/` (second pass, 2026-09-29)

The `.ai/plans/` registers in §1–§5 describe *mechanisms that decline to explain themselves*. The
prose layer registers below describe **people, places and documents that decline to explain
themselves** — a different kind of silence, held for a different reason.

**30 registers · 17 prose companions · 6 creative packs · 4 family indexes · 3 lore/audit documents.**

### 6.1 The prose companions (17) — paired with §1

Every thematic plan in §1 has a prose companion, and the two now hold *the same* silences. Where a
plan says "no record body text is stored," the companion says "the Gap is the shape of the loss."
Neither is more authoritative; the plan owns the contract and the companion owns the voice.

### 6.2 The creative packs (6) — silences inside authored content

These are the ones most at risk of being "filled in" by a well-meaning writer, because they look
like lore that wants expanding. They do not.

| Document | The silence at its centre |
|---|---|
| **The Long Line** (Proposal 11) | What the *ring* was for. A section titled *What actually happened* exists and this corpus will not open it. |
| **The Standing Record** | Ten locations, two marked **SPINE**. Why those two are load-bearing while eight others are merely true is never stated. |
| **The Holdfast** | A Registrar-General, a Census Clerk, a Shift Lead, a Cutter. Whether the census is a safeguard or a selection is asked and refused. |
| **The Duty Roster** | The Stack, the Approach, the Overflow. What happens to the person whose name is not on the roster. |
| **The Verdict** | What the Reckoning was calling *about*. The word-ladder is a corpus of rumours with punctuation. |
| **The Year of Ash** | Five "definitive" epilogues. The word is worth exactly what having five of it suggests. |

### 6.3 The family indexes (4) — silences shared *between* expansions

The most dangerous silences in the corpus, because each is only safe while **no neighbour fills it**:

| Shared silence | Families | Rule |
|---|---|---|
| The "signal interceptor" | New Ways to Play ↔ Radio/Quiet War | Resolve jointly or never. |
| The shared gate adapter | All five families | Agreed once. No second gate, ever. |
| Whether a siege is a battle or a schedule | Shelter Under Pressure | The Siege owns the action; the Works owns the drift. |
| What the Pantry Book is written in | Shelter Under Pressure | Custody belongs to *The Record Keepers*. |
| What is below and what is above | New Pressures and Places | Two silences, load-bearing for each other. |
| Why everything is *late* | The World Moves Without You | No mechanism explains it. The world reports; the player arrives second. |
| Whether the eleven expansions are one event | Master Atlas | The atlas records separateness and never asserts unrelatedness. |

### 6.4 The boundary — what is *not* polishable

For anyone continuing this work: **do not prose-polish the remaining `docs/expansions/` files.**

- **14 Design Bibles / Master Plans** (71k–190k lines) are structural documents. They need a scoped
  chapter treatment, not an atmospheric pass.
- **15 generated matrices and forensic audits** are generated outputs. `AGENTS.md` rule: *"Do not
  modify generated outputs by hand; run the owning generator and its `--check` mode when one
  exists."*
- Adding invented mystery to an audit changes what the audit *claims to have verified*. That is not
  polish; it is falsification.

The prose layer is complete. What remains is either structural, generated, or 70,000 lines wide.

### 6.5 `docs/plans/` — the third pool, and why it is out of scope (measured 2026-09-29)

Verified separately, because it is the largest literal match for "non-integrated plans" and it is
**not** a prose target:

| Pool | Count | Size | Verdict |
|---|---|---|---|
| `.ai/plans/` (thematic + package cards) | 39 | 40–650 lines | ✅ Polished (passes 1–3) |
| `docs/expansions/` (prose companions, creative packs, indexes) | 30 | 32–2,915 lines | ✅ Polished (passes 5–7) |
| `docs/plans/` integration plans | 20+ | **75,902–103,569 lines each** | ❌ Not prose |

`C1_planintegration.md`, `C2_planintegration[3..7].md`, `CF_P1_DISTRESS_CONTENT_SEAL_INTEGRATION_PLAN.md`,
`CROP_ROSTER_INTEGRATION_PLAN.md` and their siblings are 80k–100k-line technical integration
plans. Adding atmospheric prose to them would corrupt contract documents that gates and foremen
read for exact path claims. They are also, at that size, largely machine-assembled.

**Rule for future agents:** if a document is (a) generated, (b) an audit that asserts what it
verified, or (c) an integration plan read by a gate — it is not a prose target. Richness belongs in
the companion, never in the contract.

---

### 6.6 Second prose pass — the Year Two package cards (2026-09-29, later session)

The ten Year Two package cards (`y2-p0` … `y2-p9`) received a **second prose pass**: each Prologue
gained one *second layer* paragraph and each card gained a **§1b Texture, Mystery & Voice** section
(what the player is never told · sample voice fragments · design texture beats), matching the
corpus standard already used by the family plans.

**Register discipline held.** §6 of each card is unchanged — the pass added **zero** recorded open
questions, so every count in §1 above still holds. The new fragments are *texture*: content
candidates and deliberate silences that deliberately do not ask to be answered. If a future pass
promotes one into a register row, this index must be updated in the same change.

---

### 6.7 Second prose pass, batch 2 — the family plans (2026-09-29, later session)

Ten non-integrated family plans received the second prose pass as complete families: **Shelter
Under Pressure** (`the-deep`, `deep-works`, `long-siege`, `ration-wars`, `record-keepers`),
**World Moves Without You** (`living-region`, `long-line-freight`, `plague-year`,
`drowned-coast`) and **New Pressures and Places** (`the-sky`). Each Prologue gained one *second
layer* paragraph and each plan gained a **§1c The Deeper Layer — scenes, artifacts & held
silences** section (what the shelter leaves lying around · scenes the player may piece together ·
held silences), supplementing the existing §1b without repeating it.

**Register discipline held.** §12 of each plan is unchanged — **zero** new recorded open
questions (each register remains its 6 rows), so every count in §1 above still holds. The §1c
"held silences" are explicitly labelled texture, not register rows: if a future pass promotes one
into a register row, this index must be updated in the same change. §1b and the pass's fragment
labels remain consistent with the taxonomy in §2.

---

### 6.8 Second prose pass, batch 3 — New Ways to Play, the composites, and P1B (2026-09-29)

Ten more non-integrated plans received the second prose pass: the complete **New Ways to Play**
family (`quiet-war`, `underworld`, `radio-free-ashfall`, `reconstruction-tree`, `faith-and-schism`,
`shelter-governance`, `crews-and-companions`), the two Movement-and-War composites
(`iron-road-and-siege-year`, `convoy-wars-and-inside-a-house`), and the last Year Two card
(`y2-p1b-chapter-profiles`). Each Prologue gained one *second layer* paragraph; the family plans
gained **§1c The Deeper Layer** (artifacts · micro-scenes · held silences); the composites gained
§1c in an *objects* variant that deliberately does not duplicate their §7b/§9b/§10b day-entry
chronicles; P1B gained the standard card **§1b** and now matches its P0–P9 siblings.

**Register discipline held.** All §12 (and composite §17/§19) registers unchanged — **zero** new
recorded open questions (family registers remain 6 rows each; the index counts in §1 hold). The
"held silences" fragments are labelled texture, not register rows; promoting one later requires
updating this index in the same change.

With this batch, every narrative plan in `.ai/plans/` has been through the second prose pass.
Remaining unexpanded: `year-two-the-long-thaw` (umbrella — already carries §0b/§1b/§14), the two
technical stragglers (`ashfall-chatgpt-item-art-tranche-36`, `performance-host-qol-2026-09-27`),
and `template.md` (not a plan).

---

### 6.9 Second prose pass, batch 4 — new composites, the umbrella, and the technical lane (2026-09-29)

Ten more non-integrated plans received the second prose pass, chosen as the least-expanded pool
after the narrative families closed out in batch 3: the three **new Movement/Place composites**
(`evenings-and-memory-work`, `paper-and-power-and-the-treaty-table`,
`works-below-and-machine-in-the-walls`), the **Year Two umbrella** (`year-two-the-long-thaw`), the
**two new UI a11y packages** (`ui-a11y-final-color-literals`, `ui-a11y-target-size-sweep2`), and
the **Performance & QoL family** (`performance-build-files`, `performance-host-files`,
`performance-qol-save-files`, `performance-host-qol-2026-09-27`).

**Register discipline held.** All registers unchanged — **zero** new recorded open questions.
Composites: second-layer Prologue paragraph + **§1c objects variant** (deliberately distinct from
their §7b/§8b/§10b/§14b/§15b texture sections). Umbrella: second layer + §1c. Technical lane:
deepened Framing with *second layer* + *texture* commentary blocks, each closing with "register
below unchanged"; the two bare UI packages gained a §0 Framing ("The Last Colour" / "The Floor")
whose *deliberate limits* point at their existing Non-goals and recorded skip exceptions rather
than creating new register rows.

Remaining for future passes: `war-of-words-and-long-inquest` (arrived mid-batch),
`ashfall-chatgpt-item-art-tranche-41` (tranche lane), `template.md` (not a plan), and any further
arrivals — the repository visibly refills.

---

### 6.10 Second prose pass, batch 5 — the War of Words, tranche 41, and the a11y series close (2026-09-29)

The remaining ten non-integrated plans received the second prose pass: the new composite
`war-of-words-and-long-inquest` (second layer + **§1c objects & held silences**; its §25 register
unchanged), `ashfall-chatgpt-item-art-tranche-41` ("Fifteen Voices in a Sleeve" framing — *a
cassette is a voice that agreed to be kept; a casualty list is a voice that did not* — carried into
`integrated/visual/` when the tranche lane integrated it mid-batch), and the **eight remaining UI
a11y cards** (`p1-input-correctness`, `p2-focus-contrast`, `p3-nav-overflow`, `scrim-token`,
`sidebar-hover-overflow`, `target-sizes`, `accent-tokens`, `fontsize-lift`), each gaining a
*second layer* paragraph and a *texture* commentary block in its Framing.

**Register discipline held.** All registers unchanged — **zero** new recorded open questions
(a11y registers remain 5–6 rows each; the tranche's Visual specification/Verification stand as its
register; composite §25 untouched). Each addition closes with "register below unchanged" or its
equivalent.

**Every narrative and technical plan in `.ai/plans/` has now seen the second prose pass.** The
pool is open-ended only insofar as the repository refills (each recent batch found new arrivals:
tranches, a11y packages, Movement/Place composites). Future passes should re-scan first, take the
least-expanded arrivals, and repeat the treatment: second layer + deeper-layer section or framing
depth, registers untouched, contract surfaces byte-identical.

---

### 6.11 Second prose pass, batch 6 — the prose companions (2026-09-29)

With `.ai/plans/` exhausted (only `template.md`, which is not a plan), the pass moved to the paired
corpus: the **ten prose companions in `docs/expansions/` for batch 2's ten plans**
(`expansion_the_deep_plan`, `expansion_deep_works_plan`, `expansion_long_siege_plan`,
`expansion_ration_wars_plan`, `expansion_record_keepers_plan`, `expansion_living_region_plan`,
`expansion_long_line_freight_plan`, `expansion_plague_year_plan`, `expansion_drowned_coast_plan`,
`expansion_the_sky_plan`) — chosen so every plan and its design bible sit at equal depth.

Each companion gained a **"The deeper layer — objects, scenes & held silences"** section (second
layer · three artifacts · two micro-scenes · two held silences) placed immediately before its
**"What stays unsaid"** register, with the fragments deliberately non-duplicative of both the
paired plan's §1c and the companion's own register.

**Register discipline held.** Every "What stays unsaid" register is unchanged — **zero** new
recorded questions across all ten bibles. The new held silences are labelled texture and explicitly
defer to the registers below them.

Remaining for future passes: the 7 companions of batch 3's plans, the creative packs and family
indexes, `year_two` umbrella companion, `template.md`, and any new arrivals.

---

### 6.12 Second prose pass, batch 7 — new arrivals + the family companions close (2026-09-29)

Ten plans, per the re-scan-first protocol: the two new arrivals in `.ai/plans/`
(`second-nature-and-ruins-of-the-before` — composite treatment: second layer + **§1c objects &
held silences**; `ui-theme-coverage` — new §0 Framing "A Daytime Theme, Trespassing"), the seven
remaining **family prose companions** (`expansion_quiet_war_plan`, `expansion_the_underworld_plan`,
`expansion_radio_free_ashfall_plan`, `expansion_reconstruction_tree_plan`,
`expansion_faith_and_schism_plan`, `expansion_shelter_governance_plan`,
`expansion_crews_and_companions_plan`) and the **Year Two umbrella bible**
(`expansion_year_two_the_long_thaw_plan`), each gaining a deeper-layer block before its register.

**Register discipline held.** All "What stays unsaid" registers unchanged — **zero** new recorded
questions; §25/§19 registers in the new plans untouched. Fragments non-duplicative of both the
paired plans' §1c and the registers above them.

**All seventeen prose companions are now at pass-2 depth, paired plan for paired plan.** Remaining
corpus: 6 creative packs, 4 family indexes, `template.md`, and future arrivals.

---

### 6.13 Second prose pass, batch 8 — the family indexes and creative packs (2026-09-29)

The last ten in-scope corpus documents: the **four family indexes** (`expansion_new_pressures_and_places_index`,
`expansion_shelter_under_pressure_index`, `expansion_world_moves_without_you_index`,
`expansion_new_ways_to_play_index`) and the **six creative packs** (`expansion_02_the_duty_roster`,
`expansion_03_the_standing_record`, `expansion_05_the_year_of_ash`, `expansion_08_the_verdict`,
`expansion_the_holdfast`, `expansion_11_the_long_line`).

Each index gained a **"The deeper layer — the family as a shape"** section before its shared-silences
register (the four directions and one building; the four that "add a calendar"; the family whose
feeling is *lateness*; the five verbs that already existed). Each pack gained a **second-layer
framing + "what the pack leaves lying around"** block appended to its director's framing blockquote.

**Register discipline held.** Every "What this family refuses to answer" register and every pack's
"What stays unsaid here" block is unchanged — **zero** new recorded questions; each addition says
so explicitly. One markdown defect self-caught and fixed mid-batch: two fragment lists broke
blockquote continuity on a bare blank line (Year of Ash, Verdict) — repaired to `>`-continued form.

**The entire in-scope corpus has now seen the second prose pass**: 40 plans in `.ai/plans/`, 17
prose companions, 6 creative packs, 4 family indexes. Excluded by standing rule (index §6.5):
generated volumes, matrices, implementation logs, audits, and integration plans read by gates —
plus `template.md`, which is not a plan. Future briefs should re-scan for new arrivals first.

---

### 6.14 Second prose pass, batch 9 — arrivals pass (pool held 3; not padded) (2026-09-29)

Re-scan found three in-scope arrivals and no more: `other-beginnings-and-the-hard-road` (composite
treatment — second layer + **§1c objects & held silences**; "a Start is a promise the place makes
to its people; a Vow is a promise the people make to themselves"; the charge board's margin words
in "small type and nobody is scolded"; the day-41 slip where "the vow was broken. The road went
on"), `ui-lifecycle-bypass` (framing "The Lights Were Off" — 36 bypasses as "one defect
photographed thirty-six times", the gate as the real deliverable), and
`STORY_EXPANSION_BATCH_2_INDEX` (batch-as-a-shape section; "216 openings exist unnamed. The batch
names four and declines to name the rest").

**Not padded to ten.** `ashfall-chatgpt-item-art-tranche-44` was integrated to
`integrated/visual/` by the tranche lane mid-batch, before its polish window — receipts are out of
scope. The `docs/plans/expansion_wave1/` and `EXPANSION_PROGRAM_WAVE*` corpora are the documented
word-count-inflation machine (200k–370k-word targets through "synchronized continuation waves";
briefs #10–16 measured this bloat at 2.26 GB and removed 258 MB of it) — adding prose there would
feed the exact problem the corpus rules exist to prevent. Implementation logs, matrices, audits,
catalogs and status docs remain excluded per §6.5. `template.md` is not a plan.

**Register discipline held.** §25 registers and the batch-2 index's silences unchanged; every
addition labelled texture-only. Corpus status: everything in scope is now at pass-2 depth; future
briefs should re-scan for arrivals and treat only what arrives.

---

### 6.15 Third prose pass — ten plans, slight additions + text polish (2026-09-29)

The recurring brief's two asks are *adding* rich text **and** *polishing the existing text*. Passes
1–2 were purely additive; this pass covers both, boundedly. Ten plans across the four families
(`the-deep`, `the-sky`, `record-keepers`, `ration-wars`, `quiet-war`, `radio-free-ashfall`,
`faith-and-schism`, `living-region`, `plague-year`, `drowned-coast`) each gained a **Third pass —
three fragments** set appended to §1c (thirty fragments total; §12 registers untouched — the
counts in §1 hold). Text review found the existing prose largely resistant to improvement — the
few genuine touches were cadence-level, e.g. the-deep's requisition fragment re-cut ("The blank is
ruled — somebody expected it to be filled in later. It never was.") and quiet-war's second-layer
cadence ("Judgement is an instrument, and instruments wear."). Where a line was already at its
best weight, it was left alone — polish that degrades is not polish.

Fragment highlights: "Shift log, 03:00: 'air normal.' The handwriting is normal. The hour is not."
(TD) · "Warning lead time: two days. Long enough to move a bed. Not long enough to move a life."
(SK) · "Ink fade rate: printed on the sheet. The printer did not know they were writing a
prognosis." (RK) · "Unexplained: 0. A clean week reads like a held breath." (RW) · "Turned away at
the gate. The gate records the time. It cannot record the doubt." (QW) · "Affinity: 3. That is not
three listeners. It is three regions not being indifferent." (RF) · "The rite programme is stapled
at the corner. The staple is newer than both pages." (FS) · "The wave arrives at the gate as
arithmetic and leaves as someone's decision." (LR) · "Protocol change-day: dated, initialled,
laminated. The lamination is optimism." (PY) · "Readiness does not move the tide." (DC)
