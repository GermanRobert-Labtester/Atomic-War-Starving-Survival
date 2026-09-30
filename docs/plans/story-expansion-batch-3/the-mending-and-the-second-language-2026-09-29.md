# Feature / Task Plan: The Mending (repair culture over the existing maintenance seams) & The Second Language (the shelter's coined words)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 3, plans 4 of 4 (subjects 39–40).

> **Subjects covered (2 of the 8 in the "gatherings and signals" batch):**
> 39. **The Mending** — repair culture as an ethic: what the shelter keeps working, who keeps it
> working, and the small marks repairs leave behind. (Prefix `MN`.)
> 40. **The Second Language** — the words this shelter coined for things the before had no word
> for, and the old words that changed their meaning on the way here. (Prefix `SL`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `Rail/RailTrackMaintenanceLedger.cs` + `RailTrackMaintenanceEngine` (Maintain/PerformMaintenance
> precedents), `SkyLayerArmorSystem.RepairCell`, `SubterraneanSystem.TryShoreNode/
> TryInstallVentilation/TryClearBlockage`, `CompanionAnimalSystem` care, and the Machine in the
> Walls kept-days seam (`.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md`); for the
> language half, `docs/plans/PLAN_42_SURVIVOR_VOICE_INTEGRATION_PLAN.md` (keyed line catalog,
> knowledge class) and `.ai/plans/living-region-2026-09-29.md` E5 (four unmapped region
> vocabularies).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — What We Keep Working, and What We Call It

> *"A repair is a sentence written on an object: this is still ours. A word is the same sentence
> written on the world."*

Everything in this shelter is mended. The rail is measured and oiled, the roof is patched to a
condition, the machines are kept by people who have opinions about them. What none of the owners
records — what no arithmetic can hold — is the *habit*: the fact that the shelter has decided,
collectively and without a vote, to fix things rather than replace them. The Mending makes that
habit legible. One read model over the maintenance seams that already exist: what is broken, what
has been mended, how many times, and by whose hands. And the small marks — a maker's tick under a
patch, a date scratched inside a drawer — that repairs leave the way footsteps leave a path.

The Second Language is the other kind of repair: the repair of *meaning*. The before's words do
not fit the after's world, so the shelter has been quietly cutting new ones — *the un-morning* for
the hour the lights fail, *short-count* for a roster with names missing, *far-water* for anything
that must be fetched. Some old words survived with new bodies. Some new words are already fading
out of use while the log keeps them. The plan gives the shelter a glossary: an authored coinage
table, plain definitions, first-recorded days, and one flag for the words that are on their way
out.

**Tone & register.** Workbench-plain and lexicographic. The mending's vocabulary is *patch, tick,
count, hands, kept, working*; the language's is *word, sense, fading, first-recorded*. Prose for
the mending should read like a repair log kept by someone who loves tools too much to romanticise
them; prose for the language should read like a dictionary's first edition — plain definitions
for words that have never been plain. Neither half is nostalgic. Both are custodial.

**Mystery & texture.** Three silences carry the pair. Under some repairs there are *older*
repairs — mends whose marks predate the damage they sit beneath (§12). One word in the coinage
table is defined, in plain language, by a use nobody at the shelter performs (§12). And one word
appears in the log with no definition at all: recorded, dated, first-heard, and never glossed
(§12). None of these is a puzzle with a solution in this plan. They are the shelter's habits
being older and stranger than its inventory.

**The second layer.** Repair and naming are the same instinct at two scales. A patch says *this
object continues*; a coined word says *this experience will happen again and therefore deserves a
handle*. A shelter that mends and names is a shelter that has decided to stay — because both
acts assume a future in which the mended thing is used again and the new word is said again. The
mark under the repair and the first-recorded day under the word are the two smallest monuments
in the game, and the only ones nobody built on purpose.

---

## 1. Goal & Outcome

> *Design intent: the player should be able to look at one patched roof column and know how many
> hands have touched it, and to read one coined word and hear the shelter saying it.*

### 1.1 The Mending (MN)

- **Goal:** A derived **Mend Index** (pure read model) over the existing maintenance verbs —
  per tracked object: state (Working / Kept / Failing / Mended×n), **mend count**, optional
  opaque **maker's mark** tag written by the repair verb's caller (a string tag, no prose), and
  a one-line **Mend Log** entry per repair (object, verb, owner, day). One optional custom hook
  (evenings-and-memory-work seam): a **Mending Day** gathering that routes its effects through
  existing repair verbs only. Nothing in this plan repairs anything by itself.
- **Outcome (observable):** on a fixed seed, a `RepairCell` / `Maintain` / `TryShoreNode` call
  (through its owner, unchanged) is observed by the index and appears as a mend-log line with the
  owner's own verb name; mend counts accrue and survive save/load; an object mended three times
  reads `Mended×3`; the optional Mending Day custom performs only existing repair verbs; with no
  custom hook and no tracked objects every owner behaves identically to today.
- **Non-Goals:** no new repair verb; no new condition authority (condition stays with each owner);
  no durability model; no crafting; no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Second Language (SL)

- **Goal:** An authored **Coinage Table** (≤ 40 rows: word, part of speech, plain definition,
  first-recorded day, `use_fading` flag, optional `old_word_of` tag for words that inherited an
  old word's body) surfaced as glossary rows (`codex_entries.json` or the vault's display seam —
  decided at P0); a derived **Glossary State** (current / fading / inherited); and one optional
  read hook: survivor-voice lines (Plan 42's keyed catalog) may *reference* a coined word by id,
  resolved read-only at render.
- **Outcome (observable):** on a fixed seed, the glossary renders every authored word with its
  plain definition and first-recorded day; `use_fading` words render with a fading mark; inherited
  words show their old word; a voice line referencing a word id renders with the word substituted
  from the table; with no rows every surface behaves identically to today; save/load preserves
  glossary state.
- **Non-Goals:** no localization seam changes (C2[7] owns technical strings); no language
  generation, no grammar, no translation; no naming of people or places (cartography and the
  Guest Book's silences govern); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a repair's maker's mark may be an
  *inherited word* from the coinage table (the shelter's word for a mender), resolved read-only;
  and a Mending Day custom may surface one coined word in its gathering line. Neither half reads
  the other's state; both ship dark.

---

### 1.4 Worked examples (non-normative)

**A repair, observed (fixed seed).**

> Day 33 — `SkyLayerArmorSystem.RepairCell(C-7)` runs through its own owner path, unchanged. The
> index observes and writes one line: "Column C-7, mended. Verb: RepairCell. Owner: sky. Day 33."
> Mend count C-7: 2. The first mending's tick is now under the patch; the index keeps `mark_count:
> 2` and one opaque tag.
>
> Render: C-7 reads `Mended×2`. The cell's condition moved exactly as its owner decided. Nothing
> in the diff repairs anything.

**A Mending Day (custom, dark).**

> The gathering performs existing repair verbs only: two `Maintain` calls and one `TryShoreNode`.
> Three log lines appear through the index. §6.3 asserts the invariant: the pull request contains
> no new repair verb, no condition write, and no owner signature change.

**A glossary card.**

> *keep* — printed with both senses: "before: to retain; now: to repair." The old sense is not
> struck out.
>
> Card 31 renders: word present, first-recorded day 94, definition blank — the glossary's one
> permitted blank, shown as the ruled line in the format's plain type.

---

## 1b. Texture, Mystery & Voice

**The mark under the repair.**

The maker's mark is an opaque tag, not a signature: no prose, no names, no character. What makes
it powerful is *layering* — when an object is mended again, the old mark is covered and the
index remembers there was one. Repair becomes stratigraphy: the object's little archaeological
section, readable in one glance.

**Definitions are boundaries.**

Every coined word gets a plain definition because plainness is the plan's ethic — a word the
shelter uses daily should be explainable to a stranger in one sentence. And the word with *no*
definition is the plan's held breath: the glossary's format tolerates it exactly once, and the
toleration is the mystery.

**What the player is never told.**

- What the older repairs under the newer repairs were mending. The mark predates the damage it
  sits beneath (§12); whether that is a record-keeping artefact or something stranger is not
  modelled and must not be.
- Who is behind the maker's marks. Marks are opaque tags (DEC-MN-03); a mark is a *habit*, not
  a person, and the index refuses to count them per survivor.
- What the undefined word means. It is recorded, dated, first-heard, and unglossed (§12). Its
  presence in the format is the whole of it.
- Whether the Second Language is diverging from the other settlements' speech. Living Region E5
  records four unmapped region vocabularies; this plan adds a shelter's coinage and refuses to
  reconcile the five.

**Voice — sample fragments (content candidates for `mending_lines.json` / `coinage_words.json`).**

> "Column C-7, mended twice. The first patch is under the second. Neither is the original and
> both are doing work." — mend log (MN)

> "Maker's mark: a tick and a date. The tick is in the same place on everything it is on, which
> is the only biography this plan will ever have." — mend log (MN)

> "Mending Day. Nothing new was repaired that would not have been repaired anyway. Everybody
> came. The coming is the point." — custom line (MN)

> "*Short-count*, n. A roster with names missing. Used at muster; not used at the wall." —
> glossary (SL)

> "*Far-water*, n. Anything that must be fetched. The word has a joke in it that no one laughs at
> any more and everyone still understands." — glossary (SL)

> "Word 31: first heard day 94, recorded day 94, defined — not. The glossary keeps it in the
> format's one permitted blank." — glossary (SL)

**Design texture beats.**

- **The mending never repairs.** It observes repairs and records them. Any verb of its own turns
  the ethic into a mechanic and the mechanic into a chore.
- **Mend counts are eulogies in digits.** `Mended×3` should read, in the UI's plain type, like
  the object's whole medical history.
- **One undefined word, forever.** The format's tolerance must be singular; a second blank would
  make blanks a genre.
- **Fading is a first-class state.** Words die in this world, and the glossary marks them the way
  the Record Keepers mark conditions: plainly, in four words or fewer.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the bench leaves lying around.**

> "Repair slip: tick and date. Under the patch at the corner, another tick, another date, older
> than the crack it is under."

> "Mend index, column of counts: 1, 1, 2, 1, 7. The sevens of this world do not have a column for
> why."

> "Mending Day tally: everyone worked; nothing was fixed that would not have been fixed anyway;
> attendance complete. The tally is filed under 'kept.'"

**What the desk leaves lying around.**

> "Glossary card, *un-morning*: 'the hour the lights fail.' Three citations, two of them from
> people who were asleep for it."

> "Card 31: blank definition, ruled. The rule is the glossary's admission that its format has an
> edge."

> "Inherited-word card: *keep* — before: 'to retain'; now: 'to repair.' The old sense is not
> struck out. Both senses are printed."

**Held silences (texture, not register rows).**

- Whether the older marks are repairs of the same object or of what the object was before. The
  stratigraphy is recorded and its readings disagree; the index keeps both readings and adopts
  neither. Texture only.
- Who says the undefined word aloud. It is first-heard and never overheard again; the log records
  its day and not its mouth.

**Third pass — the tick and the word (texture only; §12 register unchanged).**

*(Polish pass, non-contractual: texture only — no authority, path, decision, acceptance criterion
or verification step. §12 gains no row and loses no silence.)*

> "Mend index, printed weekly. The counts are the only biography in the shelter that is checked
> for accuracy."

> "Glossary, fading word: *un-morning*. It is already being said less. The card will outlive the
> saying, which is what cards are for."

> "Day 94's word has never been overheard again. The format records the day and not the room."

**Held silences (texture, not register rows).**

- Whether the tick and the word were made by the same hands. A mark and a coinage are both
  habits; the plan keeps them in two tables and links neither. Texture only.
- What the glossary's next blank would be if the rule changed. DEC-SL-02 permits exactly one;
  the rule's fragility is part of the pleasure and must never be discussed in-fiction.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | Maintenance verbs exist across owners: `RailTrackMaintenanceLedger.Maintain`, `PerformMaintenance`; `SkyLayerArmorSystem.RepairCell`; `SubterraneanSystem.TryShoreNode/TryInstallVentilation/TryClearBlockage`. | `.ai/plans/iron-road-and-siege-year-2026-09-29.md` E3–E5; `.ai/plans/the-sky-2026-09-29.md` E7; `.ai/plans/deep-works-2026-09-29.md` E2 | LIVE (per corpus) |
| E2 | Machine kept-days and keeper seams (MW); condition bands stay with seven existing owners. | `.ai/plans/works-below-and-machine-in-the-walls-2026-09-29.md` §1.2 | PROPOSED (plan is DRAFT) |
| E3 | Companion care verbs exist (`CompanionAnimalSystem` care/sickness). | `.ai/plans/crews-and-companions-2026-09-29.md` E4 | LIVE |
| E4 | Evening custom machinery can host one authored custom as an optional hook (EV seam; custom = fifth repetition). | `.ai/plans/evenings-and-memory-work-2026-09-29.md` §0/§1.1 | PROPOSED (soft dependency) |
| E5 | Survivor voice: keyed line catalog, pure selection authority over (speaker × state × day event × knowledge class); lines are data rows. | `docs/plans/PLAN_42_SURVIVOR_VOICE_INTEGRATION_PLAN.md` | PROPOSED (AUDIT-PENDING plan) |
| E6 | `codex_entries.json` exists (glossary-like rows possible). | umbrella F16 | **VERIFY (P0)** |
| E7 | A repair verb's caller can attach a small opaque tag without editing the owner's signature (wrapper/observer seam precedents: `ApplyRun` observer patterns; host sessions wrap owners). | host-session precedents (IB/P28 logs) | **VERIFY (P0)** |
| E8 | Living Region E5: four unmapped region vocabularies exist; a mapping file is added and nothing deleted. | `.ai/plans/living-region-2026-09-29.md` §1b/E5 | PROPOSED (plan is DRAFT) |
| E9 | Glossary state is derivable/presentational; persistence needed only for player-coined additions if any (DEC-SL-01). | codex/vault display seam | **VERIFY (P0)** |
| E10 | Additive nested DTO checksum-safety for mend counts and any glossary additions. | codec / snapshot tests | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Repair, condition, wear | each owner (rail, sky, subterranean, garage, companions) | nothing; the index observes and never repairs |
| Machine kept-days | Machine in the Walls seam | read-only; mend counts never touch keeper logic |
| Evening customs | evenings-and-memory-work | one optional custom (Mending Day), routed through existing repair verbs |
| Voice lines | Plan 42 catalog | optional read: word-id substitution at render |
| Technical strings | C2[7] localization seam | nothing; the glossary is fiction, not localization |
| Words | — | `CoinageTable` (pure Core, closed, validated) + glossary rows in data |
| Mend/glossary state | — | `MendIndex` (pure derived, zero stored except counts/marks) nested additively — **DEC-MN-02 / DEC-SL-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Maintenance/MendIndex.cs` (new, pure), `Language/CoinageTable.cs` (new, pure)
**Data:** `coinage_words.json`, `mending_lines.json`, glossary rows in `codex_entries.json` (P0 decides)
**Host:** repair-verb observers (`INT`), custom hook (EV seam, `INT`), voice render substitution (`INT`)
**Presentation:** a "Kept" band on the existing maintenance/board surface; a "Words" band on the existing codex/archive surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Maintenance/MendIndexTests.cs`, `Language/CoinageTableTests.cs`, `Ashfall.Core.Tests/Save/MendIndexSaveTests.cs`

## 5. Packages

### MN-P0 — Premise audit (Auditor; read-only): close E6–E10; confirm the observer seam for repair verbs without owner signature changes; confirm glossary destination.
### MN-P1 — Mend index (Core): derived states, mend counts, opaque marks, one-line log. **Accept:** observation only — no owner behaviour changes; determinism; counts persist.
### MN-P2 — Mending Day custom (host, optional, dark): routes to existing repair verbs only. **Accept:** no new repair verb exists anywhere in the diff.
### SL-P1 — Coinage table + glossary (Core + data): ≤ 40 rows, closed fields, one undefined-word allowance (DEC-SL-02), validator row-level. **Accept:** no prose in code; `use_fading` renders a mark; inherited words show both senses.
### SL-P2 — Voice substitution (host, optional, dark): word-id substitution in Plan 42 render path, read-only. **Accept:** lines without word-ids are byte-identical; no catalog edits.
### X-P1 — Seam hooks (host, shipped dark): maker's-mark vocabulary read; gathering line word. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no tracked objects, no custom hook, no coinage rows → every owner's behaviour identical on a saved corpus.
3. Observation-only invariant: the mending adds **no repair verb, no condition write, no owner signature change** (§6.3 test).
4. Determinism: identical index states, counts and glossary renders on replay (`CampaignStreamIds` fork; never `System.Random`).
5. Save round-trip of counts/marks and any glossary additions; old saves load; checksum-safe (E10).
6. Word discipline: no prose in code; exactly one undefined word permitted in the table; `old_word_of` rows show both senses (DEC-SL-02).
7. Marks are opaque tags, never attributed to survivors (DEC-MN-03).
8. The registered silences of the Machine in the Walls, Living Region and Guest Book plans stay unanswered (§7).

## 7. Cross-plan boundaries
- **Works Below / Machine in the Walls:** kept-days, nicknames and moods are theirs; the mending counts *repairs*, the household supplies the vocabulary for machines (its silences unchanged).
- **Evenings and Memory Work:** custom mechanics are theirs; Mending Day is one authored custom, and `Custom`-ownership questions stay in EV's register.
- **Plan 42 (Survivor Voice):** substitution is read-only at render; the keyed catalog is never edited here.
- **Living Region:** the four unmapped vocabularies remain unmapped; this shelter's coinage is a fifth table, and reconciliation is refused (E5 discipline).
- **Record Keepers / The Cabinet & The Dig:** word decay marks are presentation; RK owns record decay; the twice-repaired object's story (DG-OM-5) is cross-referenced here and *still not explained*.
- **C2[7] localization:** technical strings untouched; the Second Language is fiction inside data rows, not UI localization.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-MN-01 | The mending observes; it never repairs, and adds no repair verb. | rule | Yes |
| DEC-MN-02 | Mend counts/marks nest additively; the index is derived except those two fields. | architecture | Yes; confirm in P0 |
| DEC-MN-03 | Maker's marks are opaque tags; never attributed, never counted per survivor. | tone | Yes |
| DEC-MN-04 | A Mending Day custom is optional and dark; its effects are existing repair verbs only. | design | Yes |
| DEC-SL-01 | Glossary rows live in data; no prose in code; persistence only for fields the row schema already carries. | architecture | Yes |
| DEC-SL-02 | The table allows exactly one undefined word; a second would make blanks a genre. | tone | Yes — **needs canon note** |
| DEC-SL-03 | Inherited words show both senses; the old sense is never struck out. | tone | Yes |
| DEC-SL-04 | No reconciliation with the region's unmapped vocabularies; the fifth table stands alone. | boundary | Yes |
| DEC-X-01 | Both seam hooks ship dark; each side reads nothing of the other. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Mend`, `RepairIndex`, `Coinage`, `Glossary`, `Slang`)
- [ ] Premise re-verified (Rule 7); MW/EV/Plan-42 evidence re-read; their registered silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-MN-01, DEC-SL-02)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing maintenance, custom and voice-catalog tests (list from P0 selector)
- [ ] Host selftests for the observer seam and the render substitution (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: observing a repair verb requires changing any owner's signature or behaviour; the glossary cannot hold one undefined row without weakening the schema for all rows; attribution of marks cannot be prevented by construction; any registered silence of MW/EV/DG/LR would have to be answered; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the habit older than the inventory and the language larger than its
glossary. Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| MN-OM-1 | What were the older repairs repairing? | Marks stratify beneath marks (§12); two readings of the section disagree and the index adopts neither. | Never — deliberately sealed. |
| MN-OM-2 | Whose tick is the maker's mark? | DEC-MN-03 keeps marks opaque; a mark is a habit, not a person. | Never — a rule, not a gap. |
| MN-OM-3 | Why does the seven-times-mended object keep failing? | Mend counts are recorded and causes are not; durability modelling is a Non-Goal and the question is the players' to keep. | Never — tone-locked by DEC-MN-01. |
| MN-OM-4 | Who started Mending Day? | Custom provenance belongs to EV's register ("Who started the first custom… must never be invented"). This plan inherits that silence. | EV plan's owner (its register governs). |
| MN-OM-5 | Is the mending ethic a rule or a mood? | No charter, no law row; the Assembly's statute book is not involved and must not be dragged in. | Shelter Governance's owner, if ever authored. |
| SL-OM-1 | What does the undefined word mean? | DEC-SL-02 permits exactly one blank and this is it; glossing it would end the glossary's best silence. | Never — deliberately sealed. |
| SL-OM-2 | Who said it first, on day 94? | First-heard is recorded; first-speaker is not (§1c). Attribution is not in the format. | Never — texture by omission. |
| SL-OM-3 | Is the Second Language drifting from the region's speech? | E5's four vocabularies stay unmapped (DEC-SL-04); the fifth table stands alone and the drift is unmeasured. | Never — the pair must stay unresolved together (mystery-index §3 discipline). |
| SL-OM-4 | Why does *keep* carry both senses without a struck line? | DEC-SL-03 is a design rule; why the shelter never discards an old sense is not modelled. | Canon owner only, as a signed decision. |
| SL-OM-5 | Does anyone outside the shelter use these words? | The glossary is the shelter's; the Guest Book's guests are not modelled as speakers and must not be. | The FA/GB plan's owner, if ever linked. |
