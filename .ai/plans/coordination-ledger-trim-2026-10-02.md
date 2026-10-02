# PERF/ROBUSTNESS — Task 55: trim the oversized coordination ledgers

STATUS: APPROVED BY USER

> **Approval basis:** user's Python/CI/hygiene/robustness brief (2026-10-02),
> suggested order "55, 49, 41, 42, 45". This package implements Task 55.

## 1. Goal & Outcome

Cut the AI-agent/review friction caused by the oversized coordination ledgers
without losing provenance:

| File | Before | After | Reduction |
|---|---:|---:|---:|
| `WORKTREE_OWNERSHIP.md` | 1,610,904 B / 17,502 lines | 19.5 KB | ~82× |
| `INTEGRATION_PLANS.md` | 486,684 B / 2,276 lines | 2.1 KB | ~235× |
| `KNOWN_DEBT.md` | 44,980 B / 90 lines | 6.3 KB | ~7× |

Full verbatim snapshots are preserved under
`docs/archive/coordination/2026-10-02/` (with a README and source SHA-256).
Canonical files are now short indexes: ownership/queue contract + active
claims + a pointer to the archive.

## 2. Non-Goals

- No change to `.github/workflows/**` (a concurrent `perf-track-c-ci-tiering`
  session owns Task 49), `scripts/ci/**`, or generator scripts.
- No deletion of content: the trim is archive-and-index, not removal.
- No change to active claim paths or plan approval state.

## 3. Evidence (before change)

- `docs/archive/coordination/` already existed (a 2026-09-27 subdir), so the
  archive home was established.
- `generate-docs-index.py` excludes `WORKTREE_OWNERSHIP.md` and
  `INTEGRATION_PLANS.md` (`COORDINATION_LEDGERS`).
- No gate reads ledger content (only `generate-docs-index.py` references the
  filenames, as exclusions); `doc-link-gate.sh` validates links.
- Active-claim extraction: 9 sections carry `IN PROGRESS`/`ACTIVE`/`BLOCKED`
  without a closing marker; all 30 `2026-10-02` claim headings listed.

## 4. Implementation

1. Copy each ledger verbatim to
   `docs/archive/coordination/2026-10-02/<NAME>_full.md`; record SHA-256.
2. Rewrite `WORKTREE_OWNERSHIP.md` as: contract + active/policy sections +
   recently-closed headings + archive pointer.
3. Rewrite `INTEGRATION_PLANS.md` as: contract + recent batch headings + pointer.
4. Rewrite `KNOWN_DEBT.md` as header + the 10 current (non-RETIRED) rows + pointer;
   the 63 RETIRED rows live in the archive.
5. Add the archive `README.md` (provenance, "do not restore archived claims").
6. Regenerate `docs/INDEX.md` (the trim + sprint-1's `GATE_INVENTORY.md` 74→75
   change both drifted it).

## 5. Verification

- [x] archive line counts match originals: `17502 / 2276 / 90`
- [x] `bash scripts/ci/doc-link-gate.sh` — **PASS (6130 files)**
- [x] `python3 scripts/ci/generate-docs-index.py --check` — **PASS (4162 docs)**
- [x] `git diff --check` — clean
- [x] active claims (sprint 1, sprint 2, trim) present in the new index
- [x] `docs/INDEX.md` regenerated (generated shared path; correct owner action)

## 6. Known limitation

`docs/INDEX.md` still lists the two large `_full.md` archives as HISTORICAL
oversized documents. Excluding archive copies from the index would require a
`COORDINATION_LEDGERS` change in `generate-docs-index.py`; the current approach
keeps the generator untouched and the archive discoverable.