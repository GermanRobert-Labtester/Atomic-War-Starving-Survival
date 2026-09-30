# Story Expansion Batch 2 — Index (subjects 17–32)

> **This is an index, not a plan.** It has no STATUS line, claims no path, approves nothing and changes no ledger. It only lists the eight draft plans of this batch, what each one is built on, and the findings each one recorded, so that a foreman or integrator can sequence them. Each plan carries its own `STATUS: DRAFT — awaiting user approval` and must be approved individually (CLAUDE.md Rule 8).

## The eight plans

| # | Subjects | File | Prefixes |
|---|---|---|---|
| 1 | 17 The Iron Road · 18 Siege Year | `iron-road-and-siege-year-2026-09-29.md` | IR / SY |
| 2 | 19 Convoy Wars · 20 Inside a House | `convoy-wars-and-inside-a-house-2026-09-29.md` | CW / IH |
| 3 | 21 The Works Below · 22 The Machine in the Walls | `works-below-and-machine-in-the-walls-2026-09-29.md` | WB / MW |
| 4 | 23 What We Do With the Evenings · 24 Memory Work | `evenings-and-memory-work-2026-09-29.md` | EV / MK |
| 5 | 25 Paper and Power · 26 The Treaty Table | `paper-and-power-and-the-treaty-table-2026-09-29.md` | PP / TT |
| 6 | 27 The War of Words · 28 The Long Inquest | `war-of-words-and-long-inquest-2026-09-29.md` | WW / LI |
| 7 | 29 The Second Nature · 30 Ruins of the Before | `second-nature-and-ruins-of-the-before-2026-09-29.md` | SN / RB |
| 8 | 31 Other Beginnings · 32 The Hard Road | `other-beginnings-and-the-hard-road-2026-09-29.md` | OB / HR |

## Shared design pattern (all eight)

- **Derive, don't store.** Each subject layers derived read models over owners that already exist and adds one small state owner that stores only human acts or historical facts, as an additive nested DTO — no new save section.
- **One seam per hook, all dark.** Hooks ship with `Null*` defaults; sister-plan reads are optional.
- **Deterministic.** Seeded `CampaignStreamIds` forks only; never `System.Random`.
- **Closed tables and row-level validators.** Every catalog row has a validator with row-level failure output.
- **Deliberate silences** are bound by `OPEN_MYSTERY_INDEX_2026-09-29.md`; no plan answers a question that index keeps open.

## Findings recorded per plan (so nobody rediscovers them)

- **Plan 5 (PP/TT):** `FactionEmbargoLedger` is scope-blind and has no removal call (E21); three treaty owners are folded by a read model.
- **Plan 4 (EV/MK):** two hobby catalogs exist; three remembrance owners are folded into one Calendar.
- **Plan 7 (SN/RB):** there is no food web to see (plants are not nodes); the first-generation mutants (`species_blight_rat`, `species_rad_dog`) already exist as ordinary species; a location is one expedition node with no interior; locations use two id namespaces (`location_*` and `loc_*`); nine Inquest exhibits are already staged at five sites.
- **Plan 8 (OB/HR):** `StartNewGame` takes cohort, supplies and difficulty but **not** the origin, which defaults to `origin_government_bunker`; 216 openings exist unnamed; `origin_*` names two different catalogs (shelter origins and supply profiles); difficulty, lock and Iron Man are complete and untouched; nothing records what a run promised.

## Cross-plan dependencies (all optional reads)

- Plan 7's Regional Hospital ruin ≠ plan 8's Ward Below start (DEC-OB-07); plan 8's Garrison Post ≠ `outposts.json` builds (DEC-OB-06).
- Plan 8's `vow_open_hand` counter is blocked on the Living Region plan's refusal events.
- Plan 7's sites can name rooms for Long Inquest leads (plan 6); plan 8's Marks can be cited by the Year Two chronicle.

## The deeper layer — the batch as a shape (second prose pass)

*(Second prose pass, non-contractual: texture and writing guidance only — not a claim, not an
authorization. Nothing below adds a recorded question; the plans' own §25 registers and the
Open Mystery Index are unchanged.)*

**The second layer.** Eight plans, sixteen subjects, one shared design pattern — *derive, don't
store*: every subject layers read models over owners that already exist and stores only human acts
or historical facts. And the batch's binding silence is institutional: no plan answers a question
the Open Mystery Index keeps open, which is what makes sixteen subjects read as one world instead
of sixteen proposals.

**What the batch leaves between its plans.**

> "Findings recorded so nobody rediscovers them — the truest form of institutional memory is the note that saves the next person an afternoon."

> "216 openings exist unnamed. The batch names four and declines to name the rest."

> "Nothing records what a run promised — until plan 8, which is the entire point of plan 8."

---

## Governance reminders

Plans are drafts. No claims, ledgers, `.ai/state.md` or source were touched by this batch. Claims belong in `WORKTREE_OWNERSHIP.md` (foreman/integrator). Before any package: read `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`.

**Stale-queue note for the foreman.** The `CLAUDE.md` active-queue item `CF-P6-VEHICLE-ARMOR-GRADES` ("4 armor grade tiers") appears stale: five grades already exist and are tested. Verify before executing.
