# Wave 20 — Claim-Readiness Closure (2026-09-21)

**Status:** PROPOSED — foreman claim required. No plan here is a claim, and none
modifies `INTEGRATION_PLANS.md` or `WORKTREE_OWNERSHIP.md`.

## Why this wave exists

Waves 1–19 produced 276 plans, each carrying fourteen integration layers
(§11–§24). The finalisation pass reported three residual gaps. Re-verification
(documented in each plan below) showed that **only one gap is real**; the other
two were false negatives produced by the readiness detector itself:

| Reported gap | Reported count | Re-verified truth |
|---|---:|---|
| No package IDs | 6 | **3 real** (01, 02, 03) · 3 false negatives (`### C4-1` / `B5-1` / `F6-1` digit-prefix format) |
| No verification command | 5 | **0 real** · all five cite real commands in prose (`scripts/ci/*.py --check`, `scripts/ci/*.sh`) |
| Wave line not `**Wave N` form | 20 | **0 real** · 19 use the equally valid `**Wave:** N (date) · **Kind:**` form |

The root cause is shared: the detector knew one format per field. Wave 20 closes
the real gap, corrects the record, and installs the auditor that prevents the
class of error from recurring.

## Plans

| Plan | Closes | Real work |
|---|---|---|
| `PLAN-READINESS-PACKAGE-IDS-281.md` | 3 plans without package IDs | Author IDs for 01/02/03; register the three accepted package formats |
| `PLAN-READINESS-VERIFICATION-CONTRACT-282.md` | 5 false-negative verification rows | Define the command-family contract; backfill explicit commands into §24; correct the index |
| `PLAN-READINESS-HEADER-NORMALISATION-283.md` | 20 false-negative wave rows | Define the header contract (two legal forms); optional uniformity pass; directory-vs-header check |
| `PLAN-READINESS-AUDITOR-284.md` | systemic | `tools/claim-readiness-audit.py` reproduces §24 verdicts, regenerates the index, and reports gap classes with fixtures |

## Order

281 → 282 → 283 → 284. Plans 281–283 are documentation edits; 284 is the tool
that keeps all of it honest and is the only one that touches executable code
(tooling only — never `Assets/Ashfall.Core/`, `src/`, or `Ashfall.Core.Tests/`).

## Authority

Queue authority remains `INTEGRATION_PLANS.md`; path ownership remains
`WORKTREE_OWNERSHIP.md`; the master readiness view is
[`../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md`](../EXPANSION_PROGRAM_2026-09-21/CLAIM_READINESS_INDEX.md).
