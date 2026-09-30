# Story Expansion Batch 3 — Index (subjects 33–40)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and
> changes no ledger. It lists the four planned documents of this batch, what each one is built on,
> and the boundaries each one must keep, so that a foreman or integrator can sequence them. Each
> plan carries its own `STATUS: DRAFT — awaiting user approval` and must be approved individually
> (CLAUDE.md Rule 8). **Batch 3 is complete: all four plans are authored (2026-09-29) and await
> individual approval.**

## The four plans

| # | Subjects | File | Prefixes | State |
|---|---|---|---|---|
| 1 | 33 The Fair · 34 The Guest Book | `the-fair-and-the-guest-book-2026-09-29.md` | FA / GB | **Authored (this batch)** |
| 2 | 35 The Signal Chain · 36 The Listening Hour | `the-signal-chain-and-the-listening-hour-2026-09-29.md` | SC / LH | **Authored (this batch)** |
| 3 | 37 The Cabinet of Ordinary Things · 38 The Dig | `the-cabinet-and-the-dig-2026-09-29.md` | CB / DG | **Authored (this batch)** |
| 4 | 39 The Mending · 40 The Second Language | `the-mending-and-the-second-language-2026-09-29.md` | MN / SL | **Authored (this batch)** |

## Why these subjects (positioning against the existing corpus)

The corpus is dense: 17 family plans (batch 1), 8 paired plans (batch 2), the Year Two programme,
and the wave 1–9 Expansion design bibles. Every subject below was checked against that corpus and
chosen to *extend a seam nobody owns* rather than deepen a seam that is already deep.

- **The Fair** is an *event*, not a food culture (Expansion 26 owns cuisine) and not diplomacy
  (Expansion 45 owns protocol). It is the one week a year the region's existing barter, caravan,
  petition and news seams happen in the same place at the same time.
- **The Guest Book** is *hospitality*, not door security (The Quiet War owns the knock) and not
  population (Plan 204 owns recruitment). It is the ledger of people who stay without joining.
- **The Signal Chain** is *visual* ground signalling — fires and mirrors along ridges — and
  deliberately not radio (Radio Free Ashfall), not aviation (Expansion 14), and not sound ranging
  (Expansion 36). It is the oldest network layer in the region that still works.
- **The Listening Hour** is the seventh station's authored silence — a practice, not a system —
  riding Radio Free Ashfall's seams and optionally the evenings-and-memory-work custom machinery.
- **The Cabinet of Ordinary Things** is a *display*, not a collection (Flagship XII owns
  collecting) and not an archive (the vault and the Record Keepers own keeping). It is the shelf
  where ordinary objects are labelled plainly and never explained.
- **The Dig** is *under the floors*, not into rock (The Deep Works) and not onto surfaces
  (Second Nature / Ruins of the Before owns rooms and trusts). It lifts domestic strata through
  the existing archive-decryption and expedition seams.
- **The Mending** *observes* repairs across owners that already have repair verbs and adds none;
  it is the habit made legible — counts, marks, and the mark under the mark.
- **The Second Language** is *fiction inside data rows*: coined words for the after's world,
  distinct from technical localization (C2[7]) and from the Machine in the Walls' household
  nicknames, and deliberately unreconciled with the region's unmapped vocabularies (LR E5).

## Shared design pattern (all four)

- **Derive, don't store.** Read models over owners that already exist; the only new state is human
  acts and historical facts, as one small additive nested DTO per plan — no new save section.
- **One seam per hook, all dark.** Hooks ship with `Null*` defaults; sister-plan reads optional.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Every catalog row validated with per-row failure
  output; authored ids, snake_case, `schema_version`.
- **Deliberate silences** bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan in this batch
  answers a question that index keeps open, and no plan explains the Signal Chain's builders, the
  Fair's origin, the Guest Book's first hand, or what the Listening Hour hears.

## Cross-plan dependencies (all optional reads)

- The Fair's news chatter may *read* the Living Region's graded-news seam (Heard/Told) and must
  never write it.
- The Guest Book's gate answers must route through the same door-adapter seam agreed once for
  The Quiet War (QW), The Living Region (LR-P6) and The Plague Year (PY-P3/P4) — see batch-2
  index §4. **This batch adds a fourth consumer and changes the contract none.**
- The Signal Chain may expose one boolean to The Long Siege's relief-train hook (a certified
  corridor *and* a lit chain may carry relief), shipped dark until both ends exist.
- The Listening Hour reads Radio Free Ashfall's carrier state and writes nothing; it may surface
  one line in the mailbag voice, gated behind RF's own trust rules.

## The deeper layer — the batch as a shape (second prose pass)

*(Non-contractual texture: not a claim, not an authorization. Nothing below adds a recorded
question; the plans' own §12 registers and the Open Mystery Index are unchanged.)*

**The third layer.** Batch 1 was places and pressures; batch 2 was movement and paper; batch 3 is
**gatherings and signals** — the things a region does when it has stopped merely surviving. A
fair, a guest book, a chain of fires, an hour of silence, a shelf of ordinary objects, a lifted
floor, a mended patch and a coined word are all the same verb: *the shelter making itself
available* — to strangers, to guests, to distance, to the dark, to the before, to the ground, to
the future, and to its own experience. Each plan keeps one mystery the way a house keeps one
locked room, and the eight locks rhyme: who started the Fair, who wrote the first guest entry,
who built the chain, what the hour hears, who chose the first object worth a shelf, what is under
the hearth, whose tick is under the patch, and what the undefined word means.

> "A region is not a map. It is a habit of meeting." — the batch's working line

> "Eight plans' worth of hospitality: to strangers, to guests, to distance, to the dark, to the
> before, to the ground, to the future, and to our own words." — second pass

> "The mark under the repair and the first-recorded day under the word are the two smallest
> monuments in the game, and the only ones nobody built on purpose." — the mending & the language

---
