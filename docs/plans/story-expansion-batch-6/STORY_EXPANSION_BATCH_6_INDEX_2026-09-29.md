# Story Expansion Batch 6 — Index (subjects 57–64)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and
> changes no ledger. It lists the four planned documents of this batch and the boundaries each
> must keep. Each plan carries its own `STATUS: DRAFT — awaiting user approval` and must be
> approved individually (CLAUDE.md Rule 8). **Batch 6 is complete: all four plans are authored
> (2026-09-29) and await individual approval.**

## The four plans

| # | Subjects | File | Prefixes | State |
|---|---|---|---|---|
| 1 | 57 The Frost Book · 58 What the Dog Is Dreaming | `the-frost-book-and-what-the-dog-is-dreaming-2026-09-29.md` | FB / DD | **Authored (this batch)** |
| 2 | 59 The Counting-Out Rhyme · 60 The Sayings | `the-counting-out-rhyme-and-the-sayings-2026-09-29.md` | CR / SA | **Authored (this batch)** |
| 3 | 61 The Hearth · 62 The Sweeping | `the-hearth-and-the-sweeping-2026-09-29.md` | HT / SW | **Authored (this batch)** |
| 4 | 63 The Wall Map · 64 The Doorstep | `the-wall-map-and-the-doorstep-2026-09-29.md` | WM / DS | **Authored (this batch)** |

## Why these subjects (positioning against the existing corpus)

Batch 6 is **the inward winter**: what the house says to itself when nobody is asking it
anything. Each subject is folklore-as-record — readings and sentences the household keeps with
no authority behind them:

- **The Frost Book** records window-frost *as shapes* — unitless, compared between years. It
  rhymes with the Instruments' `unitless` measure and must never become a weather mechanic.
- **What the Dog Is Dreaming** is folk commentary on sleeping companions. Animal dreams are
  deliberately *never modelled*: the Dream System owns human sleep events and this plan must not
  extend it.
- **The Counting-Out Rhyme** is children's folklore whose words belong to no known vocabulary —
  a sibling of the Second Language's undefined word and the *only* sanctioned place where the
  undefined-vocabulary motif may recur (one rhyme, never more).
- **The Sayings** are the household's sentences (SL owns *words*, sayings own *sentences*): one
  proverb with dead meaning, kept in use and out of understanding.
- **The Hearth** is *places, not portions*: the table rule owns food and the thermal owner owns
  warmth; the hearth owns the manners — and its held seat is a *different* silence from the
  Assembly's empty chairs, deliberately paired and never conflated.
- **The Sweeping** is manners, not hygiene (Triad B owns sanitation): finds are one-line records,
  never inventory, and `unnamed` is the closed vocabulary's one blank.
- **The Wall Map** is *wrong on purpose* beside the official one: folk names beside ids, two
  tenses of route, and a pencil line past the paper's edge.
- **The Doorstep** is the household-side sibling of the plate's gift: `anonymous` is a permanent
  column, and `unnamed` joins the corpus's three blanks.

## Shared design pattern (all four)

- **Derive, don't store.** Read models over owners that already exist; one small additive nested
  DTO at most per plan; no new save section.
- **Folklore is not mechanics.** No plan in this batch changes any outcome; every row is a fact
  about what the household *says, draws or keeps*.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Authored ids, snake_case, `schema_version`.
- **Deliberate silences** bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan answers a question
  that index keeps open, and no folklore in this batch is ever *explained, translated or
  predictive*.

## Cross-plan dependencies (all optional reads)

- The Frost Book reads the weather owners read-only and may cite a Wind-Names word; it never
  forecasts.
- The Dog Dreaming rows may cite a companion's state read-only (`CompanionAnimalSystem`); no
  dream content is ever generated.
- The rhyme and the sayings may be cited by Plan 42's voice lines (read-only word/line ids).
- Queued plans 3–4 continue the domestic register: the hearth's seat order vs. the Table Rule's
  portion rule; the wall map vs. cartography's official map.

## Governance note for the integrator (not a decision)

`docs/expansions/wave8/expansion_49_the_mirror_plan.md` (heliographs, relay chains, codes) is a
*sibling* of batch 3's Signal Chain subject and its boundary against that plan should be settled
at approval time. Nothing in batch 6 touches signal codes; the rhyme here is folklore, never a
cipher — and no plan may turn it into one.

## The deeper layer — the batch as a shape (second prose pass)

*(Non-contractual texture: not a claim, not authorization. Nothing below adds a recorded
question; the plans' registers and the Open Mystery Index are unchanged.)*

**The sixth layer.** Batches 3 through 5 built the household's outward and daily life. Batch 6
goes inward: the shapes frost makes, the dreams a dog has, the words of a counting rhyme, the
sentences people repeat without remembering why — and then, in its last two plans, the two faces
of the threshold: the map that pictures the beyond and the step that receives it. This is the
shelter's *folk imagination* — the readings it makes of things it cannot know, kept in the same
plain ledgers as everything else. The eight locks rhyme again: what the frost is a picture of,
what the dog is chasing, what the rhyme's words mean, what the dead saying meant, who the held
seat is for, what the unnamed find is, who draws the pencil line past the edge, and who leaves
things at the doorstep.

> "A household is not only what it does. It is what it guesses at, together." — the batch's
> working line

> "Frost, dreams, rhymes, sayings: four ways the house reads what nothing will confirm." — second
> pass

> "The official map is where things are. The wall map is where things are to us. Both are true
> and only one of them is checked." — the wall map & the doorstep

---
