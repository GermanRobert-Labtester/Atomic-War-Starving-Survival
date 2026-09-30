# Story Expansion Batch 4 — Index (subjects 41–48)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and
> changes no ledger. It lists the four planned documents of this batch and the boundaries each
> must keep. Each plan carries its own `STATUS: DRAFT — awaiting user approval` and must be
> approved individually (CLAUDE.md Rule 8). **Batch 4 is complete: all four plans are authored
> (2026-09-29) and await individual approval.**

## The four plans

| # | Subjects | File | Prefixes | State |
|---|---|---|---|---|
| 1 | 41 The Loan Shelf · 42 The Wind-Names | `the-loan-shelf-and-the-wind-names-2026-09-29.md` | LN / WN | **Authored (this batch)** |
| 2 | 43 The Day Bell · 44 The Wood Line | `the-day-bell-and-the-wood-line-2026-09-29.md` | BL / WD | **Authored (this batch)** |
| 3 | 45 The Wardrobe · 46 The Night Shift | `the-wardrobe-and-the-night-shift-2026-09-29.md` | WR / NS | **Authored (this batch)** |
| 4 | 47 The Letter-Writing · 48 The Names of the Rooms | `the-letter-writing-and-the-names-of-the-rooms-2026-09-29.md` | LT / NR | **Authored (this batch)** |

## Why these subjects (positioning against the existing corpus)

Batch 4 is **rhythm and circulation**: the things that move through a household and the calls that
structure its days. Checked against the corpus so each subject owns an unclaimed seam:

- **The Loan Shelf** is *circulation inside the household* — objects borrowed without ceremony and
  returned when done. Not inventory (the owner keeps ownership), not the Guest Book's guest debts
  (batch 3 owns those), not collectibles (Flagship XII), not the Cabinet (display, not use).
- **The Wind-Names** is the *regional weather vocabulary* — the one mapping file Living Region E5
  invites ("one mapping file is added and nothing deleted"). It is deliberately the *weather slice
  only*: the Second Language (batch 3, DEC-SL-04) refuses reconciliation, and this plan is
  precisely the narrow exception that proves that refusal.
- **The Day Bell** is the *everyday call* — muster, meal, curfew, come-in. Emergency signalling is
  Exp. 23 *The Alarm*'s (fire, rescue, evacuation); the day bell is the calls nothing is wrong.
- **The Wood Line** is the supply of warmth itself — who cuts, hauls and stacks — and it pays the
  hidden cost of batch 3's Signal Chain fire pits, the kitchen stove, the boiler and the siege
  hearths.
- **The Wardrobe** is garments as biography: pass-chains counted but never named. It is the
  Cabinet's living cousin (display vs. wear) and the Mending's reader (counts marks, writes none).
- **The Night Shift** is *culture, not security*: the Watch patrols, the Shift keeps the house's
  night rules. Sleep stays with The Quiet and the Dream System.
- **The Letter-Writing** is post-by-foot with no content field: a letter is an address with a
  weight. The radio mailbag is a different post and the two must never merge.
- **The Names of the Rooms** is folk toponymy: names live *beside* ids, never over them — distinct
  from machine nicknames (Machine in the Walls) and coined words (the Second Language).

## Shared design pattern (all four)

- **Derive, don't store.** Read models over owners that already exist; one small additive nested
  DTO at most per plan; no new save section.
- **One seam per hook, all dark.** `Null*` defaults; sister-plan reads optional.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Authored ids, snake_case, `schema_version`.
- **Deliberate silences** bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan in this batch
  answers a question that index keeps open.

## Cross-plan dependencies (all optional reads)

- The Loan Shelf's borrow columns extend the Guest Book's format (batch 3) at household scale;
  the two ledgers must not merge and neither reads the other.
- The Wind-Names' mapping file may be *cited* by the Living Region's board lines; writes none.
- The Day Bell's ring table is a *sibling* of the Signal Chain's pattern table (batch 3): one is
  closed grammar for the region, one for the roof — deliberately parallel, deliberately separate.
- The Wood Line supplies the Signal Chain's fire pits and the Mending Day's stove; both reads are
  booleans shipped dark.

## The deeper layer — the batch as a shape (second prose pass)

*(Non-contractual texture: not a claim, not authorization. Nothing below adds a recorded
question; the plans' registers and the Open Mystery Index are unchanged.)*

**The fourth layer.** Batch 3 asked how a region makes itself available; batch 4 asks how a
household *keeps time and keeps things moving*. A shelf that lends, words that travel between
settlements, a bell with five meanings, a woodpile that outlives its builders, a coat that fits
everyone, a shift that is covered but unassigned, a letter with no contents and a room with two
names — these are the humble instruments by which a shelter becomes a home rather than a depot.
The eight locks rhyme again: who stocked the first shelf, which settlement owns a wind-word, who
rings the fifth call, who stacked the ridge's wood, who the fitting coat was made for, who covers
the phantom line, what the anomalous letter says, and who first named the warm room.

> "A household is a circulation and a rhythm. Everything else is furniture." — the batch's working
> line

> "The bell tells the shelter what time it is. The woodpile tells it what winter is." — second
> pass

> "A letter is an address with a weight; a room's name is a letter to the people who come after."
> — the letter-writing & the names of the rooms

---
