# Plan 59 — Retrospective: Turn Nine Waves of Findings into Rules, Then Stop Auditing

> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window* (closing plan)
> **Depends on:** everything, literally — this plan consumes Plans 15–58.
>
> **Theme:** nine waves produced 45 plans, 135 tasks, and the same finding each time, in a new
> costume. That is a process failure, not a discovery streak: **every one of those findings was
> avoidable with a gate that either didn't exist or wasn't run.** This plan builds the gates, retires
> the audit as an activity, and hands the work to the standing machinery — intake, acceptance,
> scorecard, release. The correct end state of an audit series is that nobody needs another one.

---

## Evidence Inventory — the audit series, measured

| # | Wave | The finding, in one clause | The gate that would have caught it |
|---|---|---|---|
| 1 | 1 | the epilogue was bound to `0, true, true, true, true, true` (`Main.GameFlow.cs:444`) | a derived-state assertion on the ending path (19A step 1) |
| 2 | 1 | `TryResolveMoralChoice` had zero call sites | port-contract gate (36A) |
| 3 | 1 | 30 routed consoles with `IsBound = true` | panel-liveness gate (15C) + intake (53C) |
| 4 | 1 | 272 catalogs codex-only; `EFFECT_PRODUCED` = 4 | content-acceptance ladder (45A) |
| 5 | 2 | `ZoneRadLevel` set by one survivor-id ternary | single-writer/per-source-of-truth scan (50A/26A style) |
| 6 | 2 | `DegradeRate = 0f` + degradation on a throwaway copy | mass-balance soak (35A step 12) |
| 7 | 2 | `Inventory.Consume` called with every callback null | port-contract gate (36A) |
| 8 | 2 | duty roster reads no health/needs anywhere | behaviour-per-authority test (24A step 11) |
| 9 | 3 | three CI gates red; `AGENTS.md` citing a class that doesn't exist | claims gate + rulebook sync (29A/29B) |
| 10 | 4 | `SimulateDailyFriction` never called | port-contract gate (36A) |
| 11 | 4 | 20 of 27 emitted event kinds dropped by a `switch` with no `default` | vocabulary-contract test (31A step 7) |
| 12 | 4 | 6-node graph described as "261 nodes" in the registry | claims gate (29B) |
| 13 | 5 | 74 of 147 Core integration seams have no host caller | port-contract gate (36A) |
| 14 | 5 | zero of 16 nav/focus/controller affordances wired | input-map gate (37A step 7) |
| 15 | 6 | `InferBeliefProfile` guessing data that 72 authored records already hold | content-acceptance ladder (45A) + one-authority rule (40A step 1) |
| 16 | 6 | affinity computed by three systems, read by zero | behaviour-per-authority test (44A step 12) |
| 17 | 7 | 27 balance CSVs no one can regenerate | reproducible-sweep gate (46A step 1) |
| 18 | 7 | zero git tags, no changelog, `version → "unknown"` fallback | release-prep check (48B step 7) |
| 19 | 8 | asset gate checks 50 of 5,563 ids and passes on fallbacks | asset-coverage/strict tier (50A step 2) |
| 20 | 8 | 122 design mockups inside `assets/`, 0 references; 1,189 art orphans | hygiene gate (56A step 12) |
| 21 | 9 | 1.34 GB working copy; a Unity playmode-results XML at root | hygiene gate (56A) + doctor (56C) |
| 22 | 9 | `AI_DISCLOSURE.md` still a placeholder draft | provenance generator (57A step 4) |

| Meta-fact | Evidence |
|---|---|
| **The audit series' own error rate is measurable** | Wave 1's Task 17A premise ("one producer") was disproved in Wave 4 and published as an erratum (`Wave4_…INDEX.md`); a first-pass claim in Wave 5 ("15/16 hotkeys dead") was corrected in-session to "3/13 predicates uncalled, 4 nav actions unhandled" — audits are not exempt from the claims rule |
| The backlog now includes this audit | **122** plan files in `Next-steps-plans/` (41 below 100, 81 in the parallel 1xx series) + **133** in `piagentsplans/` — the audit contributed to the very sprawl Wave 8's 53 had to govern |
| Red gates persisted across weeks of work | three doc gates were failing at `ccac926e` (found by running them, not by CI reporting them) — so "gates exist" was never the same as "gates run" |

---

## Task 59A — Convert every finding class into a standing gate or a written rule

**Goal:** 22 finding classes above → a named gate, a rule in the instruction layer, or an explicit,
dated decision not to gate it.

**Files:** `docs/ci/CI_GATE_MANIFEST.json`, `scripts/ci/*` (the gates proposed across Waves 1–9:
15C, 17A, 26A, 27B, 29A/B, 31A, 36A, 37A, 45A, 46A, 47C, 48A/B, 50A, 53C, 54C, 56A/B, 57A),
`AGENTS.md` + the 13 rulebook copies, `docs/CURRENT_AUTHORITY.md`, new
`docs/process/GATES.md` (generated), `scripts/ci/generate-gates-doc.py`,
`Ashfall.Core.Tests/` (self-proof fixtures), `docs/roadmap/WAVE_LEDGER.md`.

### Substeps

1. **Build the gate register** from the 22 rows: gate id, what it forbids, the plan that proposed
   it, its tier, and whether it is *implemented* — three columns of that table will be "no", and
   that is the finding of this task.
2. **Assign an owner to each gate** (name/role, not "the project") — an unowned gate is the reason
   three were red while every plan claimed gates were green.
3. **Prove each gate can fail** (the rule repeated in 15C, 26B, 27A, 36A, 45A, 46A, 50A, 53C, 54C,
   56A, 57A): a fixture that violates it, added with the gate. No self-proof, no gate.
4. **Deduplicate the detectors** — several gates re-implement "a name in source has no caller"
   (15C, 18C, 27A, 36A, 40A/40B, 50A, 53C): build the one source-scan utility they all call, so
   adding a gate stops adding scripts.
5. **Write the rules into the instruction layer once**, not thirteen times: `AGENTS.md` carries the
   durable list ("authored, not inferred"; "one authority per fact"; "no route without an authority";
   "no claim without a gate"; "no fallback counts as pass"), then regenerate rulebooks (29A) —
   and keep the non-negotiable engine rules byte-identical while doing it.
6. **Decide, in writing, what will *not* be gated** (a rule for each: subjective tone, art quality,
   "is this fun"), with the human instrument that replaces the gate (54B playtests, 29B review) —
   the honest boundary prevents gate sprawl.
7. **Tier the gates**: per-push (fast), per-nightly (soak/coverage/balance/assets), per-release
   (scorecard, statements, corpus, boot) — and make `docs/CI.md`'s table generated from the
   manifest + workflow so the three sources can't disagree (56C step 11).
8. **Retire gates that stop earning their cost**: a gate that has never failed in N months and has
   no rule attached is a candidate for merge or deletion — gates impose a tax and must justify it.
9. **Add a gate-coverage claim to every plan template** (53C's intake form): "which gate would have
   caught this bug class, and does it exist?" — the question this plan just answered for nine waves.
10. **Measure before/after on the two numbers that matter**: `EFFECT_PRODUCED` catalogs and
    unbound-port count, trended per release in the wave ledger.
11. **Run the whole suite twice** and diff, proving idempotence (Wave 3's 29A step 12 lesson:
    generators that aren't idempotent are generators that will be ignored).
12. **Docs**: `docs/process/GATES.md` generated from the manifest with the 22-row lineage table, and
    a pointer from `docs/CURRENT_AUTHORITY.md`.
13. **Run the checklist** + every gate at least once.

**DoD:** each of the 22 finding classes has a gate, a rule, or a written decision — with an owner.

---

## Task 59B — Publish the retrospective, correct the record, and shrink the plan layer

**Goal:** one honest document about how this project fails and what now catches it — plus the cleanup
the nine waves earned but never performed.

**Files:** new `docs/process/RETROSPECTIVE-2026-09.md`, `docs/CURRENT_AUTHORITY.md`,
`docs/ARCHIVE_INDEX.md`, `docs/roadmap/WAVE_LEDGER.md`, `Next-steps-plans/`
(status front matter via 53A; executed plans archived), `piagentsplans/README.md`,
`ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md`, `sources.md`, `README.md`, `AGENTS.md`,
`docs/INDEX.md` (generator), 53A's plan register.

### Substeps

1. **Publish the pattern, not the list**: five failure shapes recur across all nine waves —
   *unwired seam*, *invented instead of authored*, *presence measured instead of liveness*, *claim
   without evidence*, *artifact without provenance*. Name them, link their canonical gate, and put
   them in `docs/process/` where a new contributor reads them before writing code.
2. **State the numbers as a baseline**: 5,563 ids / 4 effect-producing catalogs (W1), 74 of 147
   seams unbound (W5), 50 of 5,563 ids asset-checked (W8), 30 consoles without authority (W1),
   452 dead definitions (W7), 22/27 event kinds dropped (W4), 0 tags / 0 shaders / 0 tweens / 0
   gamepad bindings (W8–W9). Without a baseline the retrospective is vibes.
3. **Include the audit's own misses** — the 17A erratum and the Wave 5 hotkey overstatement — because
   the credibility of "every claim needs a gate" depends on applying it to the claims made *here*.
4. **Answer the uncomfortable question**: of the 22 findings, how many were *discoverable by a gate
   that already existed but was not run*? Report that ratio — it is the project's real process metric,
   and it is the argument for 59A's ownership column.
5. **Reconcile the plan sprawl this series contributed to**: 122 + 133 plan documents with three
   numbering schemes → run 53A's register, mark every plan `executed / superseded / proposed /
   blocked-by`, archive the executed (29C step 7), and cap the active set.
6. **Resolve duplication between this series and the parallel expansion waves** (81 plans ≥131):
   the MERGE clusters from 53B step 6 are the mechanism; the audit series must not be a competing
   backlog generator — it hands its remaining items to intake (59C step 1).
7. **Fix the record**: `AGENTS.md` (H5, H7, H11, the `GameBootstrap` Phase-4 instruction, the
   `bit`-era VCS section, the asset-debt paragraph) + `README.md`'s stale Unity claim + the registry
   rows the waves disproved, each with file:line evidence (29B), then regenerate rulebooks.
8. **Retire the master-plan monolith** (93 KB `ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md`,
   `sources.md` 50 KB) to a link page pointing at the wave ledger + roadmap (29C step 5).
9. **Publish the metric lineage**: one table of every number any wave index promised to move, with
   its current value and its gate — so "we did the work" is checkable in one place.
10. **Write the failure-mode catalogue as a checklist** (`docs/process/FAILURE_MODES.md`) for code
    review: 12 items, each with the grep or gate that finds it — the audit's reusable residue.
11. **Have the review done by a different tool** than the authoring (the repo's cross-tool rule),
    scoped to: does every claim have evidence, and is every gate self-proven.
12. **Set the deletion policy for the audit itself**: the wave indexes stay as history; the plans
    become executed/superseded; the `Next-steps-plans/` folder stops accepting new documents except
    through intake (59C step 1) — otherwise this plan is just the ninth instalment.
13. **Run the checklist** + docs gates + register `--check`.

**DoD:** one honest retrospective, a corrected instruction layer, and a plan folder that stops
growing on its own.

---

## Task 59C — Hand over: from auditing to operating

**Goal:** define the standing cycle — intake, acceptance, scorecard, release, review — and the exact
conditions under which a new audit is warranted. Then stop.

**Files:** `docs/roadmap/INTAKE.md` (53C), `docs/CI.md` + manifest,
`docs/release/PROCESS.md` (48B/57C), `docs/qa/SLICE_SCORECARD.md` (54C),
`docs/balance/REVIEW_RITUAL.md` (46C step 10), `docs/process/CADENCE.md` (new),
`docs/CURRENT_AUTHORITY.md`, `scripts/ci/doctor.sh` (56C), `scripts/ci/release-gate.sh`,
`docs/process/AUDIT_TRIGGERS.md` (new), this series' nine wave indexes (as archive pointers).

### Substeps

1. **Declare the audit series closed** at Wave 9, with the successor loop named: intake →
   one wave at a time → acceptance → scorecard → release. New findings go to the register as
   tickets, not to a new plan series.
2. **Define audit triggers, exhaustively**: a gate that fails and can't be explained; a class of bug
   reported twice by users; a wave of plans whose premise checks disagree; a platform/store
   requirement change; a year. No trigger → no audit.
3. **Publish the cadence** (`docs/process/CADENCE.md`): per-push fast tier; nightly soak/coverage/
   balance/assets; per-release gate report + scorecard + statements; monthly balance review (46C);
   quarterly claims/register/prune review (53A/29B).
4. **Name the standing reports** and their generators: content ladder (45A), port contract (36A),
   asset coverage (50A), slice scorecard (54C), funnel + sweep (46), plan register (53A),
   provenance (57A), doctor (56C) — nine artifacts, each generated, each owned, each with an
   `--check`.
5. **Set the escalation route** for a red gate: fix, or write an exemption with owner + expiry
   (45A step 4's rule), never "re-run until green"; the release gate refuses on unresolved reds.
6. **Instrument the process itself**: cycle time per wave, findings-per-release, gates-added vs
   gates-retired (59A step 8), unbound-port trend, `EFFECT_PRODUCED` trend, plan-folder size.
7. **Decide the next build order once, publicly**: the highest-value 13 tasks from all nine waves
   (19A, 22A, 24A, 29A, 31A, 34B.1, 36A, 40A, 44A, 45A, 48A, 50A, 54A) as the roadmap's first band,
   and let intake govern everything after (53C).
8. **Archive the audit layer**: nine wave indexes → `docs/archive/audits/` with the current-state
   pointer; the live documents are the register, the roadmap, and the gates — an archive is how a
   retrospective stops being an input.
9. **Write the onboarding page** a new contributor (human or agent) actually needs: 6 screens, in
   order — `AGENTS.md`, pillars, gates, register, slice scorecard, doctor output. Nine waves of
   context compresses into that page or it never existed.
10. **Define what "done" means for this project** — one sentence, falsifiable, in
    `docs/design/PILLARS.md`, and cross-check that the 54C bar expresses it.
11. **Assign ownership of the loop** (not of the codebase): who runs the release, who reviews the
    scorecard, who expires an exemption — with names, since nine waves showed unowned checks rot.
12. **Run everything once, for real**: `verify-fast.sh` + `release-gate.sh` + `doctor.sh` +
    the slice + the corpus, and publish that run as the closing record. Any red found now becomes a
    59A row, not a tenth wave.
13. **Docs**: `docs/process/CADENCE.md`, `docs/process/AUDIT_TRIGGERS.md`, and a final entry in the
    wave ledger marking Plans 15–58 statuses.

**DoD:** a named operating cycle with owners, and no open-ended audit activity.

---

## Cross-Task Dependencies

```
every wave index (15–58) ──► 59A's 22-row lineage   53A/53C ──► 59B step 5, 59C step 1
29A/29B ──► 59B step 7 (record correction)          46A/50A/54C/56A/B ──► 59C step 4 (standing reports)
45A/36A/50A/53C ──► 59A's deduplicated source-scan utility
        59A (gates & rules) ──► 59B (retrospective & cleanup) ──► 59C (handover & stop)
```

**Execution order:** 59A → 59B → 59C, and 59A last *across the program* (it needs the gates that
Waves 1–9 each proposed; landing it early leaves the register half-"no").

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-gates-doc.py --check              # register in sync
7. per-gate self-proof present (59A step 3) — fixture per gate
8. python3 scripts/ci/generate-plan-register.py --check          # statuses complete
9. python3 scripts/ci/sync-agent-rulebooks.py --check && doc-link-gate && docs-index --check
10. verify-fast.sh twice (idempotence) + release-gate.sh + doctor.sh + slice + corpus
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Docs | Gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 59A | 1 shared scanner | 1 generated | up to 22 | self-proofs per gate | High (integration) | LOW (CI-side) |
| 59B | 0 | 1 retrospective + cleanups | 0 | 2–3 | Medium (judgement) | LOW |
| 59C | 0 | 3 process docs | ownership | 1–2 | Low | LOW |

**Guardrails:** the retrospective must include the audit's own errors (a document that only lists
others' mistakes is marketing); no gate without an owner and a self-proof; no gate sprawl — retire
what doesn't earn its tax; do not spawn a tenth wave of plans from this plan's conclusions; keep
history (archive, don't delete); preserve the non-negotiable engine rules while correcting the rest;
and the success metric of Wave 9 is that **the plan folder stops growing**.
