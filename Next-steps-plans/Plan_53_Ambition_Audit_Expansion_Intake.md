# Plan 53 — Ambition Audit & Expansion Intake: Deciding What the Game Actually Wants

> **Wave:** Continuity Wave 8 — *The Presented Game*
> **Depends on:** 45A (acceptance ladder), 29C (plan layer + numbering policy), 49 (depth passes),
> 27B/27C (coverage & runtime evidence), 46 (metrics).
>
> **Theme:** the project has **115 plan files in `Next-steps-plans/` (38 continuity plans below 100
> and 77 expansion plans in the 1xx series), 132 in `piagentsplans/`, and 119 markdown docs under
> `docs/`** — three numbering schemes, no status markers, and no document that says which premise was
> verified against which commit. Seven waves proved the same thing over and over: the failure mode is
> **breadth without connection**. Adding 77 more systems without an intake policy reproduces the exact
> condition Waves 1–7 spent 40 plans repairing. This plan is the vaccine, not another feature.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The backlog outgrew its tracking | `Next-steps-plans/`: **115** `Plan_*.md` (38 numbered <100 — Plans 14–49 + indexes; **77** numbered ≥131 from parallel expansion waves) · `piagentsplans/`: **132** · `docs/`: **119** `.md` |
| 2 | Three numbering schemes coexist | `Plan_14`, `Plan_15–49` (continuity waves), `Plan_131–2xx` (expansion waves), plus `piagentsplans/00–129` — Wave 3's 29C proposed a numbering policy that has not been enforced since |
| 3 | No plan declares its status or premise-check | no `STATUS:`/`PREMISE_VERIFIED_AT` front matter anywhere; the only such markers in the repo are the ones this audit series added to wave indexes |
| 4 | Stale premises are demonstrably common | Wave 3's 29A/29B found five (H5, H11, `AirlockSecuritySystem.cs:80`, `questline_master`, `SILENCE_AUDIT` §4.7); Wave 4's 32A found "261 map nodes" against a 6-node graph; Wave 6 found `ExpansionEnrichmentCatalog` unread while a heuristic invented the same data; Wave 8's 50B finds `AGENTS.md` still describing a ~2,080-file Unity asset tree that is now four `.gdignore` files |
| 5 | Expansion plans routinely propose systems that already exist in pieces | e.g. a "rumour/information network" while radio tuner + `SignalTriangulationSystem` + `WeatherIntelligenceCoordinator` + `MoralChoiceGossipRuntime` are live; "per-NPC memory" while `LocationMemorySystem` + `PhantomMemory` + `standing_record_memory.json` exist; "needs performance cascade" while `NeedsSystem`/`SurvivorSocialCoordinator` are wired |
| 6 | The measured consequence of authoring without rails | `EFFECT_PRODUCED` catalogs: **4**; catalogs with zero consumers: **300** (2,067 defs); non-narrative: **29 / 452 defs**; `exempt_no_source_evidence`: 26 / 429 |
| 7 | The fake-affordance failure mode is documented in-repo | `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` BUG-UI-002: 30 routed consoles, 5,186 lines, and the root-cause note that *"thirty static console classes are registered before Core/host authority exists"* — that is an intake failure, not a UI failure |
| 8 | There is no single sentence saying what ASHFALL is | no design-pillar doc in `docs/` (`ls docs \| grep -iE "design\|pillar\|vision"` → none; only `docs/ui/DESIGN_SYSTEM_RULES.md`, which is visual) — so every grand-feature proposal is unarguable |
| 9 | Wave 3's plan-governance work is a prerequisite | `docs/roadmap/README.md`, `WAVE_LEDGER.md`, generated `docs/INDEX.md`, `generate-docs-index.py --check`, `sync-agent-rulebooks.py` — all proposed, and the docs gates are currently red (29A) |

---

## Task 53A — Inventory the backlog and make it machine-readable

**Goal:** one generated register of every plan and audit document — premise-verified?, status,
overlap, owner, evidence — so "what's current" stops being tribal knowledge.

**Files:** new `scripts/ci/generate-plan-register.py`, new `docs/roadmap/PLAN_REGISTER.md`,
`scripts/ci/generate-docs-index.py`, `docs/roadmap/README.md` (29C), every `Plan_*.md` (front matter
added), `docs/ci/CI_GATE_MANIFEST.json`, `Next-steps-plans/` + `piagentsplans/` indexes,
`Ashfall.Core.Tests/PlanRegisterTests.cs`.

### Substeps

1. **Adopt the front-matter schema 29C proposed** (`STATUS`, `PREMISE_VERIFIED_AT <sha>`,
   `SUPERSEDES`, `SUPERSEDED_BY`, `OWNER`, `RAILS_REQUIRED`, `WAVE`) and add it to every plan file —
   mechanically where inferable, by hand for the rest.
2. **Generate the register** from that metadata, with the four columns that matter: status, premise
   freshness (sha date vs HEAD), referenced-file existence, and duplicate-topic clusters.
3. **Detect dead references**: any plan citing a file:line that no longer exists is flagged
   `PREMISE STALE` — the same check `verify-capability-claims.py` (29B) needs, so build it once and
   share it.
4. **Cluster by subject** and print overlap groups (e.g. information-flow: 131 + live radio/
   triangulation/gossip; memory: 147 + `LocationMemorySystem`/`PhantomMemory`; governance: 159 +
   `LeadershipSystem`/registers/arbitration) — overlap is the finding, not a judgement about authors.
5. **Classify each plan**: `SYSTEM` (new authority), `LINK` (connects existing), `CONTENT` (data),
   `PRESENTATION`, `PROCESS` — then compute the ratio, which is the honest measure of the project's
   direction (today: heavily SYSTEM-flavoured while the measured defects are LINK-shaped).
6. **Register the generator as a gate** (`--check`, Tier-2) so the register cannot drift — the exact
   rule Wave 3 established for generated docs, and the reason to fix the three red doc gates first
   (29A).
7. **Freeze an intake snapshot**: at this moment, publish counts (plans by status, stale premises,
   overlap clusters) — that baseline is the number the audit in 53B argues against.
8. **Retire executed plans** into `docs/archive/plans/` with a completion note and commit link
   (29C step 7); an unbounded active folder is how agents restart finished work.
9. **Reconcile numbering** once, explicitly: continuity waves below 100, expansion waves ≥131, no
   renumbering of history; publish the policy in `docs/roadmap/README.md` and cite it from
   `AGENTS.md`.
10. **Resolve the co-author protocol** (29C step 10): two authors are writing into
    `Next-steps-plans/` concurrently; state folder/number ownership and collision rules.
11. **Tests**: register generation idempotency, stale-reference detection (a fixture that cites a
    deleted file must be flagged), overlap grouping, and the `--check` gate's ability to fail.
12. **Run the checklist** + docs gates.

**DoD:** every plan is registered, statused, freshness-checked, and gated — and the register is the
input to 53B, not an end in itself.

---

## Task 53B — The ambition audit: keep, merge, or drop, in writing

**Goal:** a rubric applied to the whole backlog producing one ordered roadmap and an explicit
"not now" list, with reasons a future contributor can argue with.

**Files:** new `docs/design/PILLARS.md`, new `docs/roadmap/ROADMAP.md`,
`docs/roadmap/AMBITION_AUDIT.md`, `PLAN_REGISTER.md` output, 53A's clusters,
`docs/CURRENT_AUTHORITY.md`, `AGENTS.md`, `Next-steps-plans/*` statuses.

### Substeps

1. **Write the four or five pillars first** — short, falsifiable sentences (e.g. *scarcity is
   information before it is inventory*; *people are resources and obligations*; *the shelter is a
   machine that must be maintained*; *every decision has a witness*). Without them the audit is
   opinion, which is what let 77 expansion plans accumulate.
2. **Publish the rubric** — five questions per plan: does it connect or accumulate? do the rails it
   need exist (53A's `RAILS_REQUIRED`)? does it risk a fake console (BUG-UI-002)? does it change
   what the player does on day 2? what does it block?
3. **Score the register**, not the documents: read the generated table, then open only the plans in
   the top and contested bands — 115+132 docs are not a reading assignment.
4. **Triage into four bins**: `NOW` (rails exist, high connection value), `NEXT` (needs a named
   rail from an existing plan), `MERGE` (duplicate of live capability — point at it, don't build
   beside it), `DROP` (contradicts a pillar or duplicates scope).
5. **Every DROP/MERGE decision cites evidence** (a file:line, a gate number, a Wave finding) and
   names the decider — Wave 7's 46C decision-record discipline applied to scope.
6. **Convert each MERGE cluster into a single rails ticket**: information-flow, per-NPC memory,
   governance, needs cascade, food pipeline — one ticket each, listing the live systems to extend
   instead of the new one to add.
7. **Cap the `NOW` band hard**: seven waves of evidence say the project's constraint is finishing,
   not starting — a small `NOW` list with completion dates beats an aspirational one.
8. **Name the sequencing hazards** explicitly (e.g. content waves must not run before 45A/49;
   new systems must not run before 36A; visual overhaul must not run before 50A) so the roadmap
   encodes dependencies, not wish order.
9. **Declare what the project will not do**: no 3D, no new genre layer, no procedurally generated
   open world, no live-service, no dialogue-tree engine, no code mods (Wave 7's 47C non-goals
   generalised) — a written "no" is the cheapest anti-drift mechanism there is.
10. **Publish the one-page roadmap** (`docs/roadmap/ROADMAP.md`) as a link list to waves with target
    dates and the metrics each moves (from each wave index).
11. **Set the review cadence**: quarterly re-audit, one rule — an item may return only when its
    prerequisite rail is marked done.
12. **Have a second tool review the bins** with only the register + pillars, per the repo's
    cross-tool QA rule; the reviewer sees evidence, not this plan's reasoning.
13. **Tests/docs**: nothing to test in code except the register gate; publish
    `docs/roadmap/AMBITION_AUDIT.md` as the record.

**DoD:** one ordered roadmap, one explicit not-now list with reasons, and no plan whose premise nobody
checked.

---

## Task 53C — Intake policy: what a new plan must prove before it can start

**Goal:** make the audit permanent: a five-minute gate any new plan passes before code, enforced by
the register, so "another system" is a decision with evidence rather than a default.

**Files:** new `docs/roadmap/INTAKE.md`, `scripts/ci/generate-plan-register.py` (validation rules),
new `docs/roadmap/RAILS.md` (capability readiness table), 29B's
`verify-capability-claims.py`, 45A's ladder, 36A's port contract, `AGENTS.md` (a pointer),
`scripts/ci/plan-intake-check.sh`.

### Substeps

1. **Define the intake form** as front matter, not prose: pillars touched, category
   (`LINK`/`CONTENT`/`SYSTEM`/…), rails required with their current status, duplicate-search evidence
   (`bit`-style search of the register + registry + code), the fake-console risk, the day-2 change,
   and the acceptance tier the content must reach (45A).
2. **Publish the rails readiness table** (`docs/roadmap/RAILS.md`): graph (32A), intel (33),
   identity (40A), voice (42), policy (43B), relations outcomes (44A), seasons (38A), commitments
   (38C), acceptance ladder (45A), port contract (36A) — each `not started / in flight / done`,
   generated from plan statuses so it can't rot.
3. **Require a duplicate search with receipts**: the form lists what was searched and what exists —
   the mechanism that would have caught "zero information-flow capability exists" (Wave 3's index
   already recorded that claim as an overstatement).
4. **Gate the gate**: `plan-intake-check.sh` fails a `Plan_*.md` without a complete form or with a
   `RAILS_REQUIRED` entry that isn't `done`/`in flight` **and** no named prerequisite plan — the
   policy is real only if it can refuse.
5. **Define fast lanes honestly**: process/hygiene/docs plans and `LINK` work may skip the heavy
   form; anything adding a system, panel, or resource type may not. Ambiguity is where policies die.
6. **Ban new UI surfaces without an authority**: an intake for a panel must name the Core/host
   authority, the save section, and the mutating action — the direct lesson of the 30 fake consoles,
   enforced at plan time instead of at audit time.
7. **Require the metric it moves**: each plan names the number it changes (`EFFECT_PRODUCED`, live
   panels, reachability %, coverage, funnel step) so waves report deltas instead of vibes.
8. **Add an off-ramp**: a plan may be accepted as a *spike* with an explicit timebox and a decision
   requirement — this is what prevents 4-day investigations turning into shipped half-systems.
9. **Reconcile `AGENTS.md`**: point the task workflow at `INTAKE.md` (replacing any instruction that
   implies scaffolding new systems as the default) while preserving every engine invariant
   byte-identical (Wave 3's 29 guardrails), then regenerate the 12 rulebook copies.
10. **Retire the parallel-plan free-for-all**: one folder, one numbering scheme, one register —
    115 + 132 documents is already a warning about what happens without intake.
11. **Tests**: the intake checker's ability to reject (missing rails, missing duplicate search,
    panel-without-authority), and the register's staleness detection integrated with it.
12. **Publish the first six months** under the policy and review the policy itself quarterly with the
    audit (53B step 11).
13. **Run the checklist** + docs gates.

**DoD:** a new system requires evidence to start, a new panel requires an authority, and the backlog
grows under a gate instead of around one.

---

## Cross-Task Dependencies

```
29A (green docs gates) ──► 53A (register generation can't gate a red tree)
29C (numbering + wave ledger) ──► 53A steps 1,9,10
45A (ladder) ──► 53B step 2's rubric & 53C step 1's acceptance tier
36A (port contract) ──► 53C step 6 (panel-without-authority rejection)
46A/46C (decision records) ──► 53B step 5, step 12
   53A (inventory) ──► 53B (decisions) ──► 53C (permanent intake)
   Plans 49 (depth passes) and Wave 9+ content run UNDER 53C, not beside it
```

**Execution order:** 29A → 53A → 53B → 53C. Do not attempt 53B before 53A: hand-scoring 247
documents is how audits become vibes.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-plan-register.py --check          # register in sync
7. bash scripts/ci/plan-intake-check.sh <sample plan>            # rejection behaviour proven
8. python3 scripts/ci/verify-capability-claims.py --check        # shared staleness detector
9. bash scripts/ci/doc-link-gate.sh && bash scripts/ci/sync-agent-rulebooks.py --check
10. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Code | Docs/Plan files | Gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 53A | 1 script | 247 files get front matter | 1 | 4–7 | Medium (volume) | LOW (no runtime) |
| 53B | 0 | 3 new docs | 0 | 0 | **High (judgement)** | LOW |
| 53C | 1 script | 1 policy doc | 1 | 4–6 | Medium | LOW |

**Guardrails:** no plan is deleted by this audit — superseded and dropped are marked, never erased
(history is how you avoid the same idea twice); no rubric outcome without cited evidence; the pillars
doc stays falsifiable and short; no claim that any expansion plan is *wrong*, only that its rails
don't exist yet; and the auditors must not grow the process past a form and two scripts — if intake
becomes a second bureaucracy, the next audit will be about this plan.
