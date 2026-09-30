# ASHFALL — THE RECORD KEEPERS
### The shelter decides what the shelter remembers · What is written can be lost; what is lost leaves a hole

**Document status:** Story-director design bible and prose plan. **Proposal — not a claim, not an authorization.**
**Date:** 2026-09-29 · **Author role:** Story Director (documentation only; no game data, source, or ledger edited)
**Companion integration plan:** `.ai/plans/record-keepers-2026-09-29.md`
**Family:** "The Shelter Under Pressure" — `docs/expansions/expansion_shelter_under_pressure_index.md`.
**Tone lock (inherited):** cold, exhausted, human, restrained. The past is small paper and small names. No real countries, people or copied text.
**Convention:** **LIVE** / **GAP** / **PROPOSED** / **VERIFY**.

---

> *"An archive is not a memory. It is what a shelter agreed to keep, and the shape of everything it
> could not."*
>
> The Shelter Archive is a projection. It is built from two living sources — the journal and the
> memorial wall — and it has never contained anything else. What it does not contain is not
> missing. It is **Gapped**.

---

# PART I — THE ARGUMENT

## 1. Director's statement

### 1.1 The promise

ASHFALL already remembers plenty. The journal has authors, and writes each entry in its author's voice. The memorial wall records every death, with rites and epitaphs. The Shelter Archive projects a timeline out of both. Time capsules hold letters for later. There are retention rules for the logs that would otherwise grow forever. The archive desk has a transcription queue and a catalogue of inks. An epilogue at the end reads it all back.

What none of it has is **a keeper and a risk.** Every record is safe by default, forever, in a state where nothing can get damp, faded, burned, stolen or forgotten. The transcription desk completes a job, adds a line to a list called `unlockedEvidenceIds`, and no other system ever reads that list. Inks come with a *fade rate per day* and an *archival longevity in days*; the panel prints them; nothing applies them. The archive is a perfectly faithful mirror of a place in which nobody ever loses anything.

The Record Keepers is the expansion where **the shelter's memory becomes something it has to look after.** A record lives on a medium, in a place, in the care of someone. Paper fades. Damp reaches the storage bay. A breach takes a shelf. A bearer dies with a story nobody wrote down. And when something is lost, it does not vanish; it leaves a dated hole the shelter knows about — *"Days 212–219: pages lost to the flood."* — and that hole is, in its way, the most honest thing in the archive.

The promise: **you will keep the record, and the record will tell the truth about what you failed to keep.**

This expansion gives records a **body**. Paper fades at a rate written on the ink. Slate survives
damp and does not survive a drum. An oral account lives exactly as long as the person carrying it
and one day less. And the Keeper — the same archivist who already sits at the desk — is the only
thing standing between a fact and a tag in a timeline.

There is **no author and no slant** here, and that is the strongest decision in the document. The
only narrator available is *damage* — and damage has no motive, no politics and no grudge, which
is exactly why it is frightening. A Gap is a tagged entry carrying a date, a medium and a cause,
and nothing else. An archive that shows what it is missing is the only kind of archive that
has earned being trusted.

### 1.2 Pillars

1. **A record lives somewhere.** Medium, location, condition, copies. Nothing is safe by being written.
2. **The Keeper is a person.** Time on the desk is time off duty; their care is the record's condition.
3. **Loss leaves a mark.** A missing record becomes a **Gap** — visible, dated, and true.
4. **Copying is the only preservation.** Transcription finally has a consumer: durable media and second locations.
5. **The living are records too.** An old survivor is an oral account; their clarity and their death are the record's risk.
6. **Ship dark.** No Keeper, no custody rows — every record is exactly as safe as it is today.

### 1.3 Not this

Not a second archive, journal or memorial. The Shelter Archive is a **projection** of the journal and memorial — the project's own "one authority per concern" — and stays that. Not an individual-memory system (Memory Decay owns a survivor's own clarity). Not a knowledge-capability system (*The Reconstruction Tree* owns what the shelter can *do*). Not a diary game. The player writes nothing; the player looks after what is written.

---

## 2. What the code and data actually say (audit)

| Area | What exists | What is missing |
|---|---|---|
| Journal | `JournalEntry { Id, Text, Timestamp, AuthorName, AuthorId, KnowledgeKey, Day, Hour }`; text in the author's voice (risk-bias flavoured); codex unlocks. | Entries never age or go missing. |
| Memorial | `MemorialSystem` (entries, rites, epitaphs); 6 rites; protected obligation in retention. | A memorial is never at risk; no one tends it. |
| Archive | `ShelterArchiveSystem` (Plan 162): entries typed Event/Decision/Memorial/Milestone/Discovery/Achievement, significance, tags, participants; a **static projector** `ProjectCanonicalSources(journal, memorial)`. `RecordEvent` has one gameplay caller — a "Day N Shelter Milestone" heartbeat every ten days (`src/Main.ShelterArchive.cs` L93); the rest are self-test CLI calls. | It mirrors; it cannot lose. |
| Archive desk | Transcription queue, inks (legibility, `fade_rate_per_day`, `archival_longevity_days`), archivist not on the duty roster; completion writes a journal discovery and `unlockedEvidenceIds`. | `unlockedEvidenceIds` has one reader — its own accessor. Ink fade fields are only printed in the panel. |
| Retention | 8 policies (keep newest K, rollup, drop prose keep ids); `IsProtectedObligation` for wills, memorials, grief. | The player never sees *what was forgotten*. |
| Time capsules | 4 capsules, event triggers, legacy messages. | Contents are text, not records with custody. |
| Individual memory | `MemoryDecaySystem` (Plan 185): five domains, clarity Forgotten→Vivid, reinforcement. | Not tied to what anyone *tells the shelter*. |
| Justice | `evidenceClues` with `confidenceWeight`; laws carry `min_evidence_confidence`. | Evidence is never "the book said so". |
| Epilogue | `EpilogueChronicleBuilder.Build(input)`: 20 slides, fate cards, metrics. | It reads a complete past. |

### 2.1 Disagreements to resolve first (Rule 6 — systems win)

1. **Provenance already exists.** The journal has `AuthorId`. A plan that adds "author" is duplicating. → Provenance is **read**, and this plan adds only *custody* (medium, place, condition, copies, state). (DEC-RK-02)
2. **The archive is a projection.** A gap cannot be *stored in* the archive; it must be an overlay the projector can read. → Custody rows live with the archive **desk**, and `ProjectCanonicalSources` gains an optional overlay parameter. (DEC-RK-01)
3. **Retention vs loss.** Retention prunes rolling logs; memorials are a **protected obligation** that must never be pruned. → This plan never loses a memorial *permanently*; a damaged memorial page becomes recopiable from the wall (the memorial system stays the authority). (DEC-RK-06)
4. **Reconstruction Tree overlap.** The Tree owns capability; this plan owns *accounts*. A lost manual page is the Tree's; a lost account of an event is this plan's. (DEC-RK-08)

---

# PART II — THE KEEPING

## 3. Custody

For every record the shelter cares about — a journal entry by knowledge key, a memorial page, a transcribed evidence sheet — an overlay row says:

| Field | Meaning |
|---|---|
| **Medium** | Paper (fades with its ink), Slate (durable, heavy, slow to write), Drum/Tape (needs power and a working machine), Oral (a survivor) |
| **Place** | A room: reading room, storage bay, secure storage; later, a dry drift *(The Deep Works)* |
| **Condition** | 0–100; falls by ink fade per day (Paper), by damp (place), by neglect (no Keeper); never rises except by copying |
| **Copies** | How many, where |
| **State** | Intact · Faded · Damaged · Lost |

Only the overlay is new. The record itself stays where it is.

## 4. The Keeper

The Keeper is the archivist the archive desk already asks for: a survivor who **is not on a duty shift** (the existing "busy" rule). While the chair is filled, condition decays at the normal rate and the Keeper may act. While it is empty, condition decays *faster* and no copy can be made. That is the whole trade: one pair of hands, off the wall, in exchange for the past.

## 5. What can go wrong

Each risk reads a **signal the game already has** and applies a seeded roll:

- **Damp** — from the sump/flood state and the place's room condition; hits Paper.
- **Fire** — read from the existing shelter fire-hazard incident (`ShelterFireHazardSystem`); hits everything in the burning zone's room.
- **Breach or theft** — a raid that breaches; a siege that falls; an informant with a key. Takes a shelf.
- **Neglect** — an empty chair.
- **Time** — ink fade, at the ink's own rate.

Loss is never sudden and never silent: a record moves **Intact → Faded → Damaged → Lost**, and each step is a line on the Keeper's report.

## 6. Gaps

A Lost record becomes a **Gap**: a dated entry in the archive timeline (a tagged Event, so the enum is unchanged) that says what *kind* of thing is missing, over what days, and how it was lost. The Gap is *permanent, true, and readable*. Chronicle slides, tribunals and the epilogue may all cite the silence.

## 7. Copying

The transcription desk gets its missing consumer: **copy this record to a durable medium, or to a second place.** A copy costs ink (or slate, or a working drum), a half-day of the Keeper, and resets condition to full on the *new* medium. Two copies in two places make a record survive one disaster. A copy made in the Second Shelter (*Year Two*) survives the first shelter's fall.

## 8. The living archive

An **oral account** is a survivor who remembers. Their reliability is not invented here: it is the **existing Memory Decay clarity** for the relevant domain. When a bearer dies, or their clarity falls to Forgotten, an oral record of that event is lost unless it was transcribed first. This gives the old and the dying a task that is neither work nor rest: *tell it, now, to someone with a pen.*

## 9. Three verbs

- **The Reading.** A public reading of a chosen record in the common mess hall. A small, bounded change to morale and cohesion, and — if a controversy is in the record — to legitimacy. All through existing public APIs.
- **The Dispute.** Two survivors' accounts disagree with the book. The Keeper can *Amend* (change the book), *Append* (add the counter-account), or *Strike* (remove it — which leaves a Gap and, if found out, costs the Keeper standing). Small relationship effects through the social owner.
- **Seal.** Put a record in a time capsule (the existing system) as a copy.

## 10. Evidence

A well-kept record is *evidence*. When a tribunal (existing) is asked to weigh an incident, an Intact record of it in a durable medium adds a clue with a weight taken from its condition. A Gap adds nothing. A struck entry, discovered, subtracts. It uses `JusticeSystem`'s public clue API and its existing confidence maths.

---

# PART III — HOW IT MEETS THE WORLD

## 11. Four stories

**The damp winter.** The sump is losing to the thaw. On the eleventh day the Keeper's report lists nine pages Faded and two Damaged. The player can move the storage bay's papers upstairs — a day of three people's labour — or copy the two that matter and let the rest go.

**What the old man said.** Ilya is dying, slowly. He is the only person who saw the first winter's decision. His clarity is Fading. The Keeper has one afternoon a week; the player chooses whether it goes to Ilya.

**A Struck line.** The Dispute is about a night Mikhail says he was on the wall. The book says he was not. The player can Strike it and be quiet. Three weeks later a tribunal asks the book what it saw, and the Gap is where the line was.

**The Gap.** Days 212 to 219. Pages lost to the flood. The Chronicle, when it comes, reads: *For these days we have no account. We were there.*

## 12. Voice samples

> **Keeper's report.** *Reading room: 41 intact, 6 faded. Storage bay: 9 faded, 2 damaged. One copy in the vault, three without. The roof is the problem.*

> **Journal, a Gap.** *Days 212–219 — no record. Cause: flood, storage bay. Recovered from other hands: the names of the dead, none of the reasons.*

> **Overheard.** *"Write it down." — "Who reads it?" — "Whoever's left."*

## 13. Boundaries with other expansions

| With | Boundary |
|---|---|
| **The Reconstruction Tree** | The Tree owns capability and bearers-of-skill; this plan owns accounts and their custody. A shared fragment source is read-only. |
| **The Ration Wars** | The Pantry Book is a record and may be custody-tracked; slant is *not* introduced here. |
| **The Long Siege** | A fallen siege can take a shelf; a captured runner may carry a copy. |
| **The Deep Works** | A dry held drift can be a Place; otherwise damp modifiers are the room's. |
| **Shelter Governance** | The Assembly may order a Strike; the Keeper may refuse and pay for it. |
| **The Quiet War** | An informant with access can leak, copy or steal; consumed via QW public seams. |
| **Year Two** | The Chronicle and Verdict may read "record quality"; a second-shelter copy survives the first shelter. |
| **Radio Free Ashfall** | A broadcast can be a copy on a durable medium; no new store. |

## 14. Content plan

- W1: media rows, place modifiers, Keeper report lines, first three risks (Damp, Neglect, Time).
- W2: Copy lines, Gap lines, oral-account hooks.
- W3: Reading and Dispute events, Evidence clues.
- W4: Chronicle variants (presentation-only), cross-plan hooks — ship dark until both ends exist.

## 15. Non-goals (restated)

No new archive, journal, memorial or knowledge store; no invented author or slant; no change to retention semantics or protected obligations; no new memory-decay maths; no player-authored text; no new routed panel; no Unity.

## 16. Risks

| Risk | Mitigation |
|---|---|
| Custody becomes chores | One weekly Keeper report; automatic copying rules are data; never one prompt per page. |
| Losing a memorial | Memorials are a protected obligation: recopiable from the wall, never permanently lost. |
| Duplication of the projection | The overlay is read by the projector; the archive stays a projection. |
| Tribunal overreach | Clue weight is capped by data; a Gap adds nothing. |
| Feels punitive | Every loss has a cheaper alternative earlier in the staircase. |

---

## The deeper layer — objects, scenes & held silences (second prose pass)

*(Second prose pass, non-contractual: narrative texture and writing guidance only — not a claim,
not an authorization. The register below is unchanged; the fragments are content candidates and
deliberate silences, not new recorded questions.)*

**The second layer.** Custody is memory with a responsible adult. The plan's Gaps are the archive
keeping its promise to be honest — a hole in a timeline is not a missing feature but a *shape*,
recorded as carefully as whatever surrounds it. Four words — Intact, Faded, Damaged, Lost — and a
Keeper who makes the interval between a thing happening and a thing being gone as long as paper
and politeness allow.

**What the expansion leaves lying around.**

> "Fade rate, printed on the ink. The ink's own prognosis."

> "Copy notice: second medium, second place. The notice is the only record of where the second place is."

> "Keeper's chair, empty nine days. The decay rate changed on day one. Nobody wrote that down."

**Scenes the player may piece together.**

> "A Gap is a date, a medium, a cause, and nothing else. Read in the Chronicle it is a sentence with three words missing, and the reader supplies them."

> "The memorial wall recopies whatever is asked of it. Nobody has explained the wall, and this expansion declines to be that somebody."

**Held silences (texture — the register below is unchanged).**

- What the damp in the west room sounds like. Condition is a word, not a sensor; the atmosphere is prose's job and the ledger will not lend it a microphone.
- Whether the timeline's Gaps are ordered by date or by wound. The projection tags them; ordering is a display concern and is unauthored.

---

## What stays unsaid (tone register — deliberately unanswered)

These are **not gaps and not content backlog.** They are the questions the expansion refuses to
answer, and the refusal is the atmosphere. Any future plan that answers one must say so explicitly
in its own decision register.

**What the Lost record said.** The custody ledger holds a key, a medium, a place, a condition and a
state — and **no record body text**. The Gap is the *shape* of the loss, and the shape is the entire
design.

**Why the memorial wall is durable.** Pages are never permanently lost and always recopy from the
wall. One thing must be safe in a plan about loss; the reason is left to the shelter's own
mythology.

**Whether a discovered Strike was discovered by care or by luck.** A bounded cost is applied with no
motive model. Attribution would turn an archive into a detective story.

**Who keeps the Keeper.** The chair is an archivist off the duty roster. It has no character, no
successor and no lineage here.

**What a well-kept record is worth at the end of the world.** Record quality reaches the epilogue as
**presentation only**. This expansion declines to make it a score.
