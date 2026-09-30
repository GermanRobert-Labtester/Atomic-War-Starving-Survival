# Story Expansion Batch 7 — Index (subjects 65–72)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and
> changes no ledger. It lists the four planned documents of this batch and the boundaries each
> must keep. Each plan carries its own `STATUS: DRAFT — awaiting user approval` and must be
> approved individually (CLAUDE.md Rule 8). **Batch 7 is complete: all four plans are authored
> (2026-09-29) and await individual approval.**

## The four plans

| # | Subjects | File | Prefixes | State |
|---|---|---|---|---|
| 1 | 65 The Visits Paid · 66 The Market's Third Stall | `the-visits-paid-and-the-third-stall-2026-09-29.md` | VP / TS | **Authored (this batch)** |
| 2 | 67 The Far Shelter's Name · 68 The Road in Spring | `the-far-shelters-name-and-the-road-in-spring-2026-09-29.md` | FN / TR | **Authored (this batch)** |
| 3 | 69 The Signpost · 70 The Fence Line | `the-signpost-and-the-fence-line-2026-09-29.md` | SP / FC | **Authored (this batch)** |
| 4 | 71 The Season Words · 72 The Goodbye at the Gate | `the-season-words-and-the-goodbye-at-the-gate-2026-09-29.md` | SE / GT | **Authored (this batch)** |

## Why these subjects (positioning against the existing corpus)

Batch 7 is **the outward spring**: the shelter's relations beyond itself, after four batches of
looking inward. Every subject is a *way the house goes out* — to neighbours, to markets, to
names, to the thawing road:

- **The Visits Paid** is outbound hospitality: the reciprocal of the Guest Book (batch 3 keeps
  the door's ledger; this keeps the caller's). Expeditions own travel; this owns *courtesy with a
  schedule*. It is not diplomacy (Exp. 45) and not trade (barter owners).
- **The Market's Third Stall** is ordinary commerce, deliberately unremarkable: the stall
  everyone passes and nobody's plan mentions. The Underworld's premiums and the Fair's truce are
  theirs; the third stall is the anti-drama of trade.
- **The Far Shelter's Name** is folk naming *outward* — what the household calls the shelters two
  valleys over, and one hearsay row for what they call us. It rhymes with the Wall Map's folk
  names and the Names of the Rooms, and the three ledgers never merge.
- **The Road in Spring** is winter's second excavation: what the snow kept and the melt returns.
  The Dig (batch 3) digs *under floors*; the thaw digs *along roads* — sibling verbs, separate
  tables.
- **The Signpost** is folk cartography in wood: signs *wrong on wood* beside the wall map that is
  *wrong on purpose* — and one sign at the wrong fork that no surface may correct toward truth.
- **The Fence Line** is a folk boundary, not a defence: the gap faces the feeding place and is
  folklore, never a mechanic, and the line ends mid-wild without reason.
- **The Season Words** are *turns*, not words and not winds: the third vocabulary, alongside SL's
  coined words and WN's weather words — three ledgers, none merged.
- **The Goodbye at the Gate** keeps *forms, not texts*: the envelope discipline reaches the
  threshold, and `unsaid` clusters silently in one season.

## Shared design pattern (all four)

- **Derive, don't store.** Read models over owners that already exist; one small additive nested
  DTO at most per plan; no new save section.
- **Folklore and records, never mechanics.** No plan changes any outcome; rows are facts about
  what the household does, says, calls or finds.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Authored ids, snake_case, `schema_version`.
- **Deliberate silences** bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan answers a question
  that index keeps open.

## Cross-plan dependencies (all optional reads)

- Visits Paid may cite the Guest Book's arrivals as *their* record and never writes it; a
  departing call may carry one letter (the Letter-Writing's envelope discipline).
- The Third Stall may render a Wind-Names weather word beside its row and may sit at the Fair's
  ground without touching the Fair's premium table.
- The Far Shelter's Name may write one `Heard`-style hearsay row through the Living Region's
  read-only surface and may cite the Wall Map's folk names.
- The Road in Spring may offer one find to the Cabinet's display verb (batch-3 discipline: offer
  only) and cites the Frost Book's winter's end.

## The deeper layer — the batch as a shape (second prose pass)

*(Non-contractual texture: not a claim, not authorization. Nothing below adds a recorded
question; the plans' registers and the Open Mystery Index are unchanged.)*

**The seventh layer.** Batch 6 taught the house to read what nothing confirms; batch 7 teaches it
to *knock*. A call paid on a neighbour, a stall passed every market day, a name for the far
shelter, a road given back by the melt, a sign nailed at a fork, a fence kept to the wild's edge,
a word for the turning year and a goodbye said at the gate — these are the eight directions of a
household that has decided the world is worth the trip. The eight locks rhyme again: who the
visits are for, who runs the third stall and why the wares never change, what the far shelter
calls *us*, what the snow keeps returning at the same bend, which fork the signpost points *from*,
where the fence line ends, what the unmade turn names, and why goodbyes are said at the gate and
not inside.

> "A household is finished when it can leave and return and still be itself." — the batch's
> working line

> "Four ways out: to visit, to buy, to name, to walk. All four end at the gate." — second pass

> "A signpost is a promise that the road knows where it goes. A fence is a promise that the wild
> knows where it stops. Both promises are kept by hand." — the signpost & the fence line

> "The calendar says what day it is. The season words say what the day is like." — the season
> words & the goodbye at the gate

---
