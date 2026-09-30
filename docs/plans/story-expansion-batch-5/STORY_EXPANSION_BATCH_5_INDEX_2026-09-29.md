# Story Expansion Batch 5 — Index (subjects 49–56)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and
> changes no ledger. It lists the four planned documents of this batch and the boundaries each
> must keep. Each plan carries its own `STATUS: DRAFT — awaiting user approval` and must be
> approved individually (CLAUDE.md Rule 8). **Batch 5 is complete: all four plans are authored
> (2026-09-29) and await individual approval.**

## The four plans

| # | Subjects | File | Prefixes | State |
|---|---|---|---|---|
| 1 | 49 The Sick Room · 50 The Birthday Book | `the-sick-room-and-the-birthday-book-2026-09-29.md` | SR / BB | **Authored (this batch)** |
| 2 | 51 The Feeding Place · 52 The Lamps | `the-feeding-place-and-the-lamps-2026-09-29.md` | FP / LP | **Authored (this batch)** |
| 3 | 53 The Shed · 54 The Instruments | `the-shed-and-the-instruments-2026-09-29.md` | SH / IN | **Authored (this batch)** |
| 4 | 55 The Walk · 56 The Keys | `the-walk-and-the-keys-2026-09-29.md` | WK / KY | **Authored (this batch)** |

## Why these subjects (positioning against the existing corpus)

Batch 5 is **the counted days and the tended edge**: the household's attention to individual
lives, and what it gives away to the dark and the wild. Checked against the corpus:

- **The Sick Room** is *sitting with the ill* — bedside culture. Clinical mechanics belong to
  Exp. 38 *The Ward* and Plan 24's medical journey; disease mechanics to `DiseaseSystem`; night
  staffing to the Night Shift (batch 4). This plan owns only the vigil: who sits, what the room
  keeps, and the hours no roster records.
- **The Birthday Book** is *dates for the living*. Remembrance of the dead is Memory Work's; the
  Founding Day is a custom in EV's register; the Rite of Passage is Year Two's. The book keeps
  arrivals, name-days and birthdays — the wall is for the dead and the book is for the living.
- **The Feeding Place** is a deliberate *gift to the wild* at the edge of the yard — the exact
  opposite of Wildlife Trapping's harvest and the exact complement of Second Nature's silence.
  It is a custom, never an ecology mechanic.
- **The Lamps** is the analogue layer of light: oil, candle, the lamp route at night. The power
  grid owns electricity; this owns the flame — and the one lamp that is always lit in a room
  nobody uses.
- **The Shed** is where the hands live: tool *homes* as authored rows, folk ownership, no tool
  stats ever. It is the Cabinet's working cousin and the Loan Shelf's address.
- **The Instruments** is checks and disagreements, never corrections: calibration stays with
  `DosimeterCalibrationSystem` and the disputed pair renders in equal type, rhyming with the
  wind-names and The Sky's catalogue conflict.
- **The Walk** produces nothing and reports nothing: the exact inverse of the night rota's phantom
  line (that one unnamed and purposeful; this one named and purposeless).
- **The Keys** is custody only — no lock state — and its `never_offered` row *mirrors* the Other
  Beginnings key-ring silence without touching it.

## Shared design pattern (all four)

- **Derive, don't store.** Read models over owners that already exist; one small additive nested
  DTO at most per plan; no new save section.
- **One seam per hook, all dark.** `Null*` defaults; sister-plan reads optional.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Authored ids, snake_case, `schema_version`.
- **Deliberate silences** bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan answers a question
  that index keeps open, and no mystery in this batch resolves into a haunting, a monster or a
  mechanism.

## Cross-plan dependencies (all optional reads)

- The Sick Room may read the Night Shift's ward line and the Mending's mark-count on a blanket;
  it writes neither.
- The Birthday Book may surface one line in the evenings-and-memory-work custom machinery and may
  cite the Guest Book's arrivals as *arrivals*, never as admissions.
- The Feeding Place reads the wild species catalog read-only (Second Nature discipline) and may
  leave one line in the Living Region's `Heard` register.
- The Lamps may cite the Loan Shelf's lamp loans and the Wind-Names' weather words for the
  route's conditions; both are strings, both dark.

## The deeper layer — the batch as a shape (second prose pass)

*(Non-contractual texture: not a claim, not authorization. Nothing below adds a recorded
question; the plans' registers and the Open Mystery Index are unchanged.)*

**The fifth layer.** Batches 3 and 4 taught the shelter to make itself available and to keep
itself running. Batch 5 is about what the household *gives away*: attention to the sick and the
young, food to the wild, light to the dark, and — in its last two plans — the unprofitable care
of tools, measures, a daily circuit and a ring of keys. These are the acts that no survival
calculus can justify and no household can do without. The eight locks rhyme again: who sits the
unrecorded hours, whose date is in the book with no owner, what takes the food, who fills the
lamps, who hung the older hook, why the out-of-true instrument is kept, why the walker walks,
and what the blank key opens.

> "A household is not what it keeps. It is what it gives away on purpose." — the batch's working
> line

> "The wall is for the dead, the book is for the living, and the plate by the door is for whoever
> is neither." — second pass

> "One is walked, one is turned. A shelter's borders are habits." — the walk & the keys

---
