# Feature / Task Plan: The Counting-Out Rhyme (the children's rhyme with words from nowhere) & The Sayings (the household's proverbs)

<!-- UPON INTEGRATION (MANDATORY):
Prepend header at the top saying:
# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **INTEGRATION STATE: FULLY INTEGRATED**
And immediately move this file to .ai/plans/integrated/<category>/
-->

STATUS: DRAFT — awaiting user approval (not self-approved; CLAUDE.md Rule 8 requires `STATUS: APPROVED BY USER` before any code commit). Story batch 6, plans 2 of 4 (subjects 59–60).

> **Subjects covered (2 of the 8 in the "inward winter" batch):**
> 59. **The Counting-Out Rhyme** — the rhyme children count with: six lines, seven words that
> belong to no known vocabulary, sung since before anyone remembers. (Prefix `CR`.)
> 60. **The Sayings** — the household's proverbs: sentences repeated until they mean less and
> matter more. One with dead meaning. (Prefix `SA`.)
>
> **Prose / design companions that already exist and are extended, not replaced:**
> `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` (SL's undefined word — **its
> register governs the rhyme's sibling motif**), Year Two's generational seam (children exist;
> the Rite of Passage is the umbrella's), `.ai/plans/evenings-and-memory-work-2026-09-29.md`
> (custom machinery), `docs/plans/PLAN_42_SURVIVOR_VOICE_INTEGRATION_PLAN.md` (voice lines may
> cite sayings read-only), `.ai/plans/the-wardrobe-and-the-night-shift-2026-09-29.md` (role-only
> attributions).
>
> **Not a claim.** This plan claims no path in `WORKTREE_OWNERSHIP.md`, edits no source, no data,
> no ledger and no other plan. It is documentation only. Paths below are *proposed*; `INT` marks
> integrator-owned shared seams.

> **Editorial note (prose texture).** Sections **0**, **1b**, **1c** and **12** carry story texture
> and voice. Sample lines are content candidates for `*.json` rows; they belong in data, never in
> code. No authority, path, decision, acceptance criterion or verification step is changed by any
> prose section.

---

## 0. Prologue — The Rhyme and the Repetition

> *"A counting rhyme is a spell for choosing who is 'it'. A saying is a spell for ending an
> argument. Both work on rhythm and neither works on meaning."*

The counting-out rhyme is six lines long and every child in the shelter knows it, because every
child in the region knows it — that is the first strange thing. The second strange thing is the
words. Some of them are ordinary (*one, two, door, floor*) and some of them are *not*: a handful
of words in the rhyme belong to no vocabulary the shelter speaks, including its own coined words.
The rhyme is sung at choosing-time, its odd words pronounced confidently and consistently, and
nobody — child or adult — can say what they mean.

The sayings are the adult half of the same inheritance. "Mend it in the light." "The ledger is
not the meal." "Count twice, cut once." Sentences repeated at the moment they are needed,
attributed to nobody, worn smooth by use. One of them — *"the third bell is for the wind"* — is
repeated confidently and means nothing to anyone living: no third bell exists, and the wind has
no bell. It is kept in use and out of understanding, and the household would notice its absence
more than its nonsense.

**Tone & register.** Play-yard plain and proverb-worn. The rhyme's vocabulary is *line, word,
sung, choosing, odd*; the sayings' is *said, worn, fits, dead, kept*. Prose for the rhyme should
read like a folklore field note by someone who has stopped trying to translate it; prose for the
sayings like a margin full of small worn truths and one worn not-truth.

**Mystery & texture.** Two silences hold the pair. The rhyme's odd words are **pronounced
consistently across settlements** — the same sounds for the same untranslatable words wherever
the rhyme is known (§12). And the dead saying's meaning **is not dead everywhere**: one traveller
is recorded as having *understood* the third-bell line and said nothing (§12). Neither is a
puzzle to solve in this plan. Both are what happens when language outlives its occasions.

**The second layer.** The rhyme and the sayings are the two ends of the same rope: the game that
chooses, and the sentence that settles. Both survive on *rhythm* rather than reason, and both are
the shelter's proof that it inherited its culture rather than inventing it. The plan's quiet
thesis is that a household's oldest possessions are not its tools but its words — and that some
words survive precisely because nothing is riding on understanding them.

---

## 1. Goal & Outcome

> *Design intent: the player should hear the rhyme at choosing-time and feel the odd words as
> weather — and hear a saying land in an argument and understand why the argument ends.*

### 1.1 The Counting-Out Rhyme (CR)

- **Goal:** A **Rhyme Row** (exactly one authored rhyme: six lines, line-by-line text in data,
  `odd_word` flags on the words outside every vocabulary) and a derived **Rhyme Read** (the rhyme
  rendered at choosing-time; the odd words marked in plain type); one optional custom hook
  (evenings-and-memory-work seam) whose only effect is the choosing-time render. The rhyme is
  **text in data and rhythm in prose**: no meaning, translation or glossary entry is ever added
  for the odd words (DEC-CR-01), and exactly one rhyme exists (DEC-CR-02).
- **Outcome (observable):** on a fixed seed, choosing-time renders the rhyme's six lines with odd
  words marked; the marks are the same plain type as every other annotation in the corpus; no
  surface anywhere offers a translation; with no rhyme row every surface behaves identically to
  today; save/load round-trips.
- **Non-Goals:** no minigame (choosing stays whatever the host already does — Exp. 48 owns
  leisure); no children mechanics (Year Two's generational seam keeps its meanings); no
  language-generation or phonetics model; no folklore authority (the Second Language keeps its
  meaning); no new save section; no new routed panel; no Unity.
- **"Done":** §6 acceptance passes via `bin/run-scoped-tests`; ship-dark parity holds; handoff
  lists untouched shared paths.

### 1.2 The Sayings (SA)

- **Goal:** A **Sayings Table** (≤ 20 rows: text, `fit` occasions from a closed list
  (`choosing`, `mending`, `serving`, `arguing`, `weather`, `leaving`), `said_count`, role-only
  attribution where a saying has a known *sayer-role*, `dead` flag for sayings whose meaning is
  gone) and a derived **Saying Read** (which sayings fit today's occasions, worn counts); one
  optional read hook: Plan 42's voice lines may cite a saying id read-only. Sayings are sentences,
  not words: the Second Language owns words and the boundary is total (DEC-SA-03).
- **Outcome (observable):** on a fixed seed, the saying read renders the household's table with
  `said_count`s and fits; a `dead` saying renders its flag in the same plain type and is *not*
  deprecated — it keeps being said; a voice line citing a saying id renders the saying's text
  beside its line; with no rows every surface behaves identically to today; save/load
  round-trips.
- **Non-Goals:** no dialogue generation; no sentiment or morale effects of any kind (DEC-SA-02);
  no proverb *authoring* by the player; no localization changes; no new save section; no new
  routed panel; no Unity.
- **"Done":** §6 acceptance passes; ship-dark parity holds; handoff lists untouched shared paths.

### 1.3 The seam between them (X)

- **Goal:** One documented boundary and one optional hook: a saying may be *said at* the rhyme's
  choosing-time as one read-only string ("count twice, cut once — over the counting"); and the
  rhyme read may cite the saying that occasions it. One string each way, shipped dark.

---

## 1b. Texture, Mystery & Voice

**Pronunciation is the data the format cannot hold.**

The odd words' consistent sounds across settlements is the rhyme's real mystery — and the plan
deliberately stores *text*, not phonetics (DEC-CR-03). Prose may describe the confidence and
consistency of the pronunciation and must never write it down; the sounds live in mouths.

**A dead saying is not a broken saying.**

The `dead` flag marks meaning gone, not use gone. The saying keeps being said — "the third bell
is for the wind" lands in arguments like any other sentence, and its flag renders flatly beside
it. The prose must treat the dead saying with exactly the respect the living ones get.

**What the player is never told.**

- What the odd words mean. No glossary entry, ever (DEC-CR-01); the rhyme is the *one* sanctioned
  place in the corpus where the undefined-vocabulary motif recurs.
- Where the rhyme came from. Every settlement knows it (§12); whether that is inheritance,
  convergence or something older is refused exactly as the Signal Chain's shared grammar is.
- What the third bell was for. No third bell exists; the saying's occasion is not in any table
  (§12).
- What the traveller understood. One record: a traveller understood the line and said nothing
  (§12). The record is the whole of it.

**Voice — sample fragments (content candidates for `rhyme_lines.json` / `sayings_rows.json`).**

> "Choosing-time. Six lines. The odd words are pronounced the same way here as at the coast,
> which is the rhyme's only claim to anything." — rhyme read (CR)

> "Line 4 is mostly odd words. Children sing it loudest. Children are not translators; children
> are carriers." — rhyme read (CR)

> "Mend it in the light. Said 41 times. Fits: mending. Nobody has ever had to explain it." —
> saying read (SA)

> "The third bell is for the wind. Said 9 times. Dead since — the column says 'dead since: —.'
> The house says it anyway, in the same tone as everything else." — saying read (SA)

**Design texture beats.**

- **One rhyme only** (DEC-CR-02). The motif's power is its uniqueness; a second rhyme would turn
  strangeness into genre.
- **No meaning fields anywhere** (DEC-CR-01): the diff must contain no gloss, no translation, no
  definition column for the odd words.
- **Sayings have no effects** (DEC-SA-02). They *fit occasions* and are *said*; the moment one
  changes a number, the proverb becomes a buff.
- **The dead flag is flat and non-deprecated** (DEC-SA-04): same type, same rows, same respect.

---

## 1c. The Deeper Layer — objects & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — no authority,
no claimed path, no acceptance criterion, no verification step. §12's register is unchanged; the
fragments below are content candidates and deliberate silences, not new recorded questions. Where
a named data file holds no prose field, fragments are texture only and gain no schema.)*

**What the rhyme leaves lying around.**

> "Choosing-count, scratched on the yard wall: seven marks, four pocks. The wall keeps score of
> games the rules cannot remember." — texture only

> "The rhyme's second line has a word that sounds like *water* and a word that sounds like
> *waiting*. Neither is either. The line has been checked." — texture only

> "A child asked what the odd words mean. The adult said 'they are the counting words.' The
> answer satisfied the child and no one else, including the adult."

**What the sayings leave lying around.**

> "Saying, worn: 'the ledger is not the meal.' Said over food, over accounts, once over a
> funeral. The saying fits everything and means one thing." — texture only

> "Margin note beside the dead saying: 'ask the coast.' Two words, undated. The coast is where
> the traveller was said to have understood." — texture only

> "Phrasebook, third page: three sayings in the same hand, older sayings in three other hands.
> The book is a graveyard of margins."

**Held silences (texture, not register rows).**

- Who wrote 'ask the coast'. An undated margin note beside a dead saying (§1c); the coast is
  four settlements away and the note is not a quest. Texture only.
- Whether the counting words predate the counting. The rhyme counts with words it does not
  need (§0); the order of the inheritance is unmodelled and must not be asserted.

---

## 1.4 Worked examples (non-normative)

**A choosing-time (fixed seed).**

> The yard gathers; the host's existing choosing flow runs as it always has. The rhyme renders
> its six lines beside it with the odd words marked in plain type. Line 4's odd words are sung
> confidently and consistently, in the same sounds the coast uses. No surface offers a
> translation; the diff contains no meaning field of any kind. A saying is said over the
> counting — "count twice, cut once" — rendered as one read-only string.

**A week of sayings (fixed seed).**

> The table renders: "mend it in the light" (41 said, fits: mending), "the ledger is not the
> meal" (23 said, fits: serving, arguing), "the third bell is for the wind" (9 said, `dead`,
> flag rendered flatly beside it). A voice line citing `saying_003` renders the saying's text
> beside its line and nothing more. The dead saying is said twice this week, in the same tone as
> everything else — because the flag marks meaning gone, not use gone.

---

## 2. Evidence table (checked against the corpus 2026-09-29; re-verify at P0 — Rule 7)

| # | Claim | Evidence | Status |
|---|---|---|---|
| E1 | The Second Language's one-undefined-word rule (DEC-SL-02) governs the sibling motif; its register lists what may never be glossed. | `.ai/plans/the-mending-and-the-second-language-2026-09-29.md` §12 | PROPOSED (plan is DRAFT) |
| E2 | Children exist as a generational system (Year Two F7: stages, coming-of-age); no children mechanics may be added here. | `.ai/plans/year-two-the-long-thaw-2026-09-29.md` F7 | LIVE (umbrella evidence) |
| E3 | Plan 42's keyed voice catalog supports read-only citation precedents (word-id substitution, batch-3 SL-P2). | `docs/plans/PLAN_42_SURVIVOR_VOICE_INTEGRATION_PLAN.md`; batch-3 SL plan | PROPOSED |
| E4 | Custom machinery hosts authored customs (EV seam; choosing-time render precedent). | batch-3–6 custom hooks | PROPOSED (soft dependency) |
| E5 | Exp. 48 *The Pastime* owns leisure/games; choosing flows stay the host's. | `docs/expansions/wave8/expansion_48_the_pastime_plan.md` | LIVE (design bible) |
| E6 | Role-only attribution precedents (handover, fire-side, bell log). | batch-4–6 plans | PROPOSED (plans are DRAFT) |
| E7 | Whether text-only rhyme rows need any persistence beyond the closed table itself. | derived-render precedents | **VERIFY (P0)** |
| E8 | Board/evening render points for rhyme and saying reads. | batch-3–6 render precedents | **VERIFY (P0)** |

## 3. Authority table

| Concern | Owner (unchanged) | This plan adds |
|---|---|---|
| Choosing/minigames | host + Exp. 48 | one optional rhyme render beside the existing flow |
| Children | Year Two generational seam | nothing; children are carriers in prose only |
| Words, glossary | Second Language | nothing; the odd words are deliberately unglossed and its register governs |
| Voice lines | Plan 42 | one read-only saying citation |
| Customs | EV | one optional choosing-time custom |
| Rhyme/sayings | — | `RhymeRow` + `SayingsTable` (pure Core; text in data only) — **DEC-CR-04 / DEC-SA-01** |

## 4. Claimed Paths (proposed; `INT` = integrator-owned)
**Core:** `Assets/Ashfall.Core/Folklore/RhymeRow.cs` (new, pure), `Folklore/SayingsTable.cs` (new, pure)
**Data:** `counting_out_rhyme.json`, `sayings_rows.json`
**Host:** choosing-time render (`INT`), voice citation (`INT`), custom hook (EV seam, `INT`)
**Presentation:** the rhyme beside the existing choosing surface; a "Sayings" band on the existing evening/board surface — no new routed panel
**Tests:** `Ashfall.Core.Tests/Folklore/RhymeRowTests.cs`, `SayingsTableTests.cs`, `Ashfall.Core.Tests/Save/FolkloreSaveTests.cs`

## 5. Packages

### CR-P0 — Premise audit (Auditor; read-only): close E7–E8; confirm the Second Language register's wording for sibling motifs; confirm choosing-flow render point with Exp. 48's boundary intact.
### CR-P1 — Rhyme row + read (Core + data): one rhyme, six lines, odd-word flags, validator row-level. **Accept:** no meaning field exists anywhere (§6.3); determinism; round-trip.
### CR-P2 — Choosing-time render + custom hook (host, dark): text in data, rhythm in prose. **Accept:** no translation surface of any kind.
### SA-P1 — Sayings table + read (Core + data): ≤ 20 rows, fits, `said_count`, `dead` flags, role-only sayer. **Accept:** no effect field exists anywhere (§6.4); dead renders flatly and is never deprecated.
### SA-P2 — Voice citation (host, dark): read-only saying text beside a line. **Accept:** lines without citations are byte-identical.
### X-P1 — Seam hooks (host, dark): one string each way. **Accept:** no cross-reads.

## 6. Acceptance criteria
1. Core authority + host owner + event path + persistence + observable outcome agree (CLAUDE.md "integrated").
2. Ship-dark parity: no rhyme row and no sayings rows → choosing, voice, customs and board outputs identical on a saved corpus.
3. No-meaning invariant: the diff contains no gloss, translation or definition field for the odd words anywhere (DEC-CR-01).
4. No-effect invariant: sayings change no number of any kind (DEC-SA-02); the diff contains no morale, sentiment or stat field.
5. One-rhyme invariant: exactly one rhyme row is authorable (DEC-CR-02); the validator rejects a second.
6. Flat-render invariant: odd-word marks and `dead` flags render in the same plain type as every other annotation (DEC-SA-04).
7. Determinism: identical renders, counts and citations on replay (`CampaignStreamIds` fork; never `System.Random`).
8. The Second Language's register and Year Two's children boundary stay unanswered (§7).

## 7. Cross-plan boundaries
- **The Second Language (batch 3):** its register governs the undefined-vocabulary motif. The odd words are the *one* sanctioned recurrence and may never be glossed; the sayings are sentences and its words are words (DEC-SA-03).
- **Year Two:** children carry the rhyme in prose only; the Rite of Passage and all children mechanics stay the umbrella's.
- **Exp. 48 / host choosing flows:** the rhyme renders beside the existing flow and never replaces or gamifies it.
- **Plan 42:** citations are read-only; the voice catalog is never edited.
- **The Frost Book (batch 6):** the anomaly page and the odd words are sibling motifs (one out-of-list row each) and the tables never merge.

## 8. Decision register (proposals — unsigned)
| ID | Decision | Type | Proposed |
|---|---|---|---|
| DEC-CR-01 | Odd words are never glossed, translated or defined; the SL register governs. | tone/rule | Yes |
| DEC-CR-02 | Exactly one rhyme exists; the validator rejects a second. | rule | Yes — **needs canon note** |
| DEC-CR-03 | Text in data, rhythm in prose: no phonetics are stored. | tone | Yes |
| DEC-CR-04 | The rhyme row is text-only and derived-rendered; no state beyond the table. | architecture | Yes; confirm in P0 |
| DEC-SA-01 | Sayings are sentences in a closed table with fits and counts; roles only. | architecture | Yes |
| DEC-SA-02 | Sayings have no effects of any kind. | rule | Yes |
| DEC-SA-03 | Sentences vs. words: the boundary with the Second Language is total. | boundary | Yes |
| DEC-SA-04 | `dead` marks meaning gone, not use gone; the flag is flat and non-deprecated. | tone | Yes — **needs canon note** |
| DEC-X-01 | Both seam hooks ship dark; one string each way. | architecture | Yes |

## 9. Pre-flight Checks
- [ ] Go config/save validator passes (`bin/validate-config` / `bin/ashfall-dev validate-config`)
- [ ] No equivalent existing system found (search `Rhyme`, `CountingOut`, `Saying`, `Proverb`, `Folklore`)
- [ ] Premise re-verified (Rule 7); the Second Language register re-read; its silences listed in the handoff as untouchable
- [ ] Signed decisions in hand (at minimum DEC-CR-01, DEC-SA-02)

## 10. Verification
- [ ] `bin/run-scoped-tests` on new tests + existing voice-catalog, custom and choosing-flow tests (list from P0 selector)
- [ ] Voice citation selftest with rows on and off (VERIFY args)
- [ ] Scoped only (<30 s each); ≤10–15 test-edit steps per failure, then auto-flag in `.ai/state.md`; **no full suite** without `RUN FULL TESTS`
- [ ] Handoff in `AI_AGENT_WORKFLOW.md` format; `.ai/state.md` updated

## 11. Stop conditions (Rule 10)
Stop and report if: any surface would require a translation or gloss; a saying would need an effect to justify its row; a second rhyme row could pass validation; the voice citation could alter an uncited line; any path overlaps a live claim.

## 12. Open Mysteries & Deliberate Silence (lore register — no authority, no claimed path)

These questions are **intentionally unanswered**. They are not bugs, not TODOs, and not hidden
work items. They keep the rhyme unsung in translation and the sayings said past their meaning.
Any future plan that answers one must name the decision that permits it.

| # | Question | Why it stays open | Who may answer it (later, signed) |
|---|---|---|---|
| CR-OM-1 | What do the odd words mean? | Never glossed (DEC-CR-01); the rhyme is the sanctioned recurrence of the SL motif and its silence is inherited. | Never — deliberately sealed (governed by SL's register). |
| CR-OM-2 | Where did the rhyme come from? | Every settlement knows it (§12); inheritance, convergence and coincidence are all refused exactly as the Signal Chain's shared grammar is. | Never — the pair stays unresolved (mystery-index §3 discipline). |
| CR-OM-3 | Why do children sing line 4 loudest? | Observation only (§1b); children are carriers, not translators, and their reasons are not modelled. | Never — texture by omission. |
| CR-OM-4 | Do the odd words predate the counting? | The rhyme counts with words it does not need (§1c); the order of inheritance is unasserted. | Canon owner only, as a signed decision. |
| SA-OM-1 | What was the third bell? | No third bell exists in any table (§12); the saying's occasion is unmodelled and must stay so. | Never — deliberately sealed. |
| SA-OM-2 | What did the traveller understand? | One record: understood, said nothing (§12); the record is the whole of it. | Never — a rule, not a gap. |
| SA-OM-3 | Who wrote 'ask the coast'? | Undated margin note (§1c); not a quest, not a thread — texture only. | Never — texture by omission. |
| SA-OM-4 | Why does the dead saying stay in use? | Meaning gone, use kept (DEC-SA-04); the house's attachment is prose's job and the engine's refusal. | Never — tone-locked. |
| SA-OM-5 | Are the sayings older than the shelter? | Worn smooth by use (§0) and unattributed; dating them would turn proverbs into history with authors. | Never — the pair with CR-OM-2 stays open. |
