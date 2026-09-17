# C2 — Flagship Integration Plan [25]: Retrospective Closure, Standing Gates, Plan-Layer Compression, and the End of Open-Ended Auditing

> **Deliverable:** `C2_planintegration[25].md`
> **Source scope:** Plan 59 — *Retrospective: Turn Nine Waves of Findings into Rules, Then Stop Auditing*
> **Wave:** Continuity Wave 9 — *Weight, Durability & the Shop Window* (closing plan)
> **Primary objective:** convert nine waves of recurring continuity findings into owned, self-proven standing gates and durable instruction-layer rules; publish an evidence-backed retrospective that includes the audit series’ own mistakes; compress/archive the plan sprawl; then hand ASHFALL from open-ended auditing into a named operating cycle of intake → acceptance → scorecard → release → review.
> **Required execution order:** **59A → 59B → 59C**
> **Program-level hard gate:** **59A must land last across the broader continuity program.** It depends on the gates proposed by Plans 15–58 actually existing. Landing the register early would create a misleading table full of “planned” gates and would defeat the purpose of the retrospective.
> **Hard stop condition:** this plan must not produce a tenth audit wave. Any new issue discovered while executing Plan 59 becomes an intake/register ticket under the standing process.
> **Dependencies:** effectively all prior continuity plans, especially 15C, 29A/29B/29C, 31A, 36A, 37A, 45A, 46A/46C, 48A/48B, 50A, 53A/53B/53C, 54B/54C, 56A/56B/56C, 57A, 58, and the Wave 1–9 indexes.
> **Scope discipline:** no unowned gate, no gate without a deliberate failure proof, no hand-maintained duplicate gate lists, no subjective quality problem forced into CI, no retrospective that omits the audit’s own false premises, no destructive deletion of historical audit material, no new backlog series spawned from the retrospective, and no claim that “gates exist” if the release/CI workflows do not actually run them.

---

# 0. Executive Intent

The audit series has reached the point where continuing to audit would itself be a process failure.

Across nine waves, ASHFALL repeatedly rediscovered the same defect families in different forms:

```text
unwired seam
invented instead of authored
presence measured instead of liveness
claim without evidence
artifact without provenance
```

The individual findings varied:

- moral-choice path unreachable,
- panels claiming bound without authority,
- content loaded but not gameplay-consumed,
- radiation state hardcoded from a survivor ID,
- durability applied to a throwaway copy,
- callbacks passed as null,
- health/needs ignored by assignment logic,
- CI/doc claims drifting from repository truth,
- world simulation methods never called,
- emitted event kinds dropped by a switch,
- map claims wildly exceeding the real graph,
- dozens of Core seams lacking host callers,
- input actions declared but unwired,
- authored belief data replaced by inference,
- affinity written but never consumed,
- balance artifacts nobody could regenerate,
- no tag/release/changelog discipline,
- asset coverage passing on placeholders,
- huge art orphan populations,
- repository hygiene decay,
- provenance docs left as drafts.

The correct conclusion is not:

```text
we need a tenth audit
```

It is:

```text
we need standing machinery that catches these classes before another audit exists
```

The intended post-audit operating model is:

```text
new work / bug / feature request
        │
        ▼
      intake
        │
        ▼
  one active wave
        │
        ▼
  acceptance criteria
        │
        ▼
     standing gates
        │
        ▼
   slice scorecard
        │
        ▼
   release candidate
        │
        ▼
   release gate/report
        │
        ▼
   periodic review
```

The flagship outcome is:

> **The continuity audit becomes historical context, not an active workflow. New findings are handled by intake, owned gates, generated reports, and release discipline—not by creating another plan series.**

---

# 1. Source Diagnosis

The source makes three meta-level findings that are more important than any one bug.

## 1.1 Recurrence Means Process Failure

The same classes of defects appeared repeatedly across nine waves.

That means the missing control was not “another audit.”

It was:

```text
gate
rule
ownership
or cadence
```

---

## 1.2 The Audit Was Not Exempt From Its Own Evidence Rules

The series contained incorrect first-pass claims:

- an early producer-count premise later disproved,
- an input/hotkey count later corrected.

Therefore the retrospective must apply the same standard to itself:

```text
claim
→ evidence
→ gate or explicit judgement
```

A retrospective that only lists other people’s mistakes would be advocacy, not engineering history.

---

## 1.3 Gate Existence Is Not Gate Operation

Several checks already existed but remained red or unexecuted.

Therefore:

```text
implemented gate != effective gate
```

A standing gate needs:

- owner,
- tier,
- workflow execution,
- self-proof,
- generated documentation,
- escalation path.

---

# 2. Program-Level Success Criteria

C2[25] closes only when all of the following are true.

## 2.1 All 22 finding classes are dispositioned

Each is one of:

```text
GATED
RULED
HUMAN_REVIEW
EXPLICITLY_NOT_GATED
```

with owner and rationale.

## 2.2 Every gate has an owner

Never “the project.”

## 2.3 Every gate has a self-proof

A fixture that violates the rule and makes the gate fail.

## 2.4 Duplicate source scanners are consolidated

Gate count can grow without script sprawl.

## 2.5 Instruction rules exist once

`AGENTS.md` is canonical; generated copies follow.

## 2.6 Human-only judgement is explicitly bounded

Tone, art quality, fun, and similar subjective areas use named human review instruments instead of fake automation.

## 2.7 Gate tiers are canonical

Fast/nightly/release come from one manifest/workflow source.

## 2.8 Generated gate docs are idempotent

Two consecutive runs produce no diff.

## 2.9 The retrospective is evidence-backed

Including the audit series’ own mistakes.

## 2.10 Plan sprawl is reduced

Executed/superseded plans are archived; active plan set is bounded.

## 2.11 Parallel plan series are reconciled

The continuity series no longer competes with expansion/backlog planning.

## 2.12 Stale claims are corrected

AGENTS/README/registry/master-plan claims reflect repository truth.

## 2.13 Monolithic master-plan docs are retired to link/index pages

Not maintained as shadow authorities.

## 2.14 Metric lineage is published

Every number promised by prior waves maps to a current value and gate/report.

## 2.15 Failure-mode catalogue exists

Reusable review checklist with detector/gate per item.

## 2.16 Audit series is formally closed

Nine wave indexes become archive/history.

## 2.17 Audit triggers are exhaustive and narrow

No trigger → no audit.

## 2.18 Standing cadence is documented

Push/nightly/release/monthly/quarterly.

## 2.19 Standing reports are named, generated, owned, and checkable

No manual dashboards.

## 2.20 Red-gate escalation is explicit

Fix or time-bounded exemption; never rerun until green.

## 2.21 Process metrics are tracked

Cycle time, findings/release, gates added/retired, port/content trends, plan-folder size.

## 2.22 The final full-stack run is published as closing evidence

And any red becomes a normal ticket, not Wave 10.

---

# 3. Architectural / Process Invariants

## 3.1 Gate Ownership Is Mandatory

Every gate declares:

```text
gate_id
owner
tier
source plan
failure class
self-proof fixture
workflow
```

No owner = incomplete gate.

## 3.2 Gate Self-Proof Is Mandatory

A gate must be demonstrated to fail on a deliberate violation.

No self-proof = documentation, not enforcement.

## 3.3 One Detector Utility Where Semantics Overlap

Avoid multiple bespoke “does this symbol have a caller?” scanners.

Create one shared source-analysis utility.

## 3.4 Instruction Layer Is Single-Source

Durable project rules live in one canonical place and are regenerated elsewhere.

## 3.5 Not Everything Belongs in CI

Subjective quality gets human review.

This prevents gate sprawl and false confidence.

## 3.6 Generated Process Docs Must Be Idempotent

If generators create diff noise, contributors stop trusting them.

## 3.7 History Is Archived, Not Deleted

Audit wave indexes remain useful archaeology.

## 3.8 Active Planning Is Intake-Governed

No more free-form plan accumulation.

## 3.9 A Red Gate Has Two Legitimate Outcomes

```text
fix
or
owned exemption with expiry
```

Never “rerun.”

## 3.10 Release Gate Is the Final Arbiter

Unresolved required reds block release.

## 3.11 Retrospective Claims Are Testable Where Possible

Use the claims gate on the retrospective itself.

## 3.12 Closure Means Operational Handover

The audit is not done when docs are written.
It is done when the standing operating cycle runs.

---

# 4. Dependency Graph

```text
Plans 15–58
   │
   ▼
implemented standing gates
   │
   ▼
59A — gate register / rules / ownership
   │
   ▼
59B — retrospective / record correction / plan compression
   │
   ▼
59C — standing cadence / audit triggers / handover
```

Important source rule:

```text
59A is last across the whole continuity program
```

because it inventories and validates gates proposed earlier.

Cross-dependencies:

```text
53A/53C ─────────► plan register + intake
29A/29B ─────────► rulebook/doc truth
45A/36A/50A ─────► shared scan patterns
46A/50A/54C/56C ─► standing reports
48B/57C ─────────► release process
54B ─────────────► human playtest instrument
```

---

# 5. Baseline Capture

Before executing 59A, freeze the real current process state.

## 5.1 Gate Inventory

Read:

```text
docs/ci/CI_GATE_MANIFEST.json
scripts/ci/
workflow files
docs/CI.md
```

Record:

- total gates,
- implemented gates,
- workflow-executed gates,
- red gates,
- unowned gates,
- gates lacking self-proof,
- duplicate detectors.

---

# 5.2 Plan Inventory

Run/prepare the plan register.

Count:

```text
active
proposed
executed
superseded
blocked
unclassified
```

across:

```text
Next-steps-plans/
piagentsplans/
master plans
wave plans
parallel 1xx series
```

---

# 5.3 Documentation Drift Baseline

Record known stale areas:

- AGENTS legacy references,
- VCS instructions,
- asset debt claims,
- README engine claims,
- GameBootstrap/phase references,
- registry numbers,
- master-plan monoliths.

---

# 5.4 Standing Reports Baseline

For each proposed standing report record:

```text
exists?
generated?
owner?
--check?
workflow tier?
```

Reports include:

- content ladder,
- port contract,
- asset coverage,
- slice scorecard,
- balance/funnel,
- plan register,
- provenance,
- doctor,
- release report.

---

# 5.5 Process Metrics Baseline

Record:

```text
plan-folder file count
unbound-port count
EFFECT_PRODUCED count
open exemptions
red-gate count
release-tag count
cycle time where reconstructable
```

---

# 5.6 Verification Baseline

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Do not start final closure with unexplained baseline reds.

---

# 6. Workstream 59A — Convert Every Finding Class Into a Standing Gate or Rule

## Goal

Each of the 22 finding classes receives a permanent prevention mechanism.

---

# 7. 59A Phase A — Build the Gate Lineage Register

Create one canonical register.

Recommended source:

```text
docs/ci/CI_GATE_MANIFEST.json
```

Generated documentation:

```text
docs/process/GATES.md
```

Columns:

| Finding ID | Wave | Failure class | Gate/rule | Gate ID | Owner | Tier | Implemented | Self-proof | Workflow active |
|---|---|---|---|---|---|---|---:|---:|---:|

This table is the core proof of Plan 59A.

---

# 8. 59A Phase B — Encode the 22 Historical Finding Classes

Map source findings to canonical prevention mechanisms.

Examples:

```text
hardcoded derived ending state
→ ending derived-state assertion

unreachable moral-choice seam
→ port-contract gate

routed panel with no authority
→ panel-liveness gate

catalog parsed but no effect
→ content-acceptance ladder

hardcoded radiation authority
→ single-writer/source-of-truth rule/gate

degradation on throwaway state
→ mass-balance/behavior test

null callbacks at consume seam
→ port contract

assignment ignores survivor condition
→ behavior-per-authority test

docs cite missing class
→ claims/rulebook sync

daily sim method never called
→ port contract

event vocabulary drops kinds
→ vocabulary-contract gate

registry claims 261 nodes but graph has 6
→ claims gate

Core seam has no host caller
→ port contract

input action declared but unwired
→ input-map gate

authored identity inferred heuristically
→ authored-not-inferred rule + content acceptance

affinity has writers but no consumer
→ behavior-per-authority test

balance CSV has no producer
→ reproducible-sweep gate

no release version/tag/changelog
→ release-prep check

asset gate passes placeholders
→ strict asset coverage gate

mockups/orphans in runtime tree
→ hygiene gate

1.34GB working copy/root junk
→ doctor + hygiene gate

AI disclosure placeholder
→ provenance generator
```

Use the exact implemented gate IDs after reconciling current repository state.

---

# 9. 59A Phase C — Gate Owner Model

Each gate gets:

```text
owner_role
backup_owner_role
review cadence
```

If the project has one active maintainer, role names may still be explicit:

```text
release owner
content integrity owner
UI gate owner
```

Do not use:

```text
owner = project
```

---

# 10. 59A Phase D — Gate Self-Proof Registry

Every gate row links to a deliberate violating fixture/test.

Examples:

```text
missing caller
stale generated doc
unknown event kind
unhandled input action
fallback asset
unattributed balance artifact
dirty release candidate
unmanifested asset
unproven provenance row
```

If a gate has no safe fixture yet:

```text
status = NOT COMPLETE
```

No papering over gaps.

---

# 11. 59A Phase E — Shared Source Scan Utility

Create one reusable scanner.

Potential location:

```text
scripts/ci/lib/source_scan.py
```

Capabilities:

- symbol declaration search,
- reference/caller search,
- file/line evidence,
- allowlists,
- path scopes,
- JSON output.

Consumers:

- panel-liveness checks,
- port checks,
- content-field checks,
- identity heuristic scans,
- literal-ID scans,
- intake dead-code checks.

Do not over-generalize into a static-analysis framework beyond project needs.

---

# 12. 59A Phase F — Stable Scanner Contract

Input:

```text
roots
pattern
exclusions
allowed_refs
minimum_ref_count
```

Output:

```text
symbol
declaration
references
classification
```

Deterministically sorted.

---

# 13. 59A Phase G — Remove Duplicate Scanner Implementations

Migrate gates one at a time.

For each:

```text
old output
vs
new utility output
```

must match or differences are documented.

Delete old helper only after equivalence.

---

# 14. 59A Phase H — Durable Instruction Rules

Canonical rules in `AGENTS.md`.

Source-inspired minimum durable rules:

```text
Authored, not inferred.
One authority per fact.
No route without a live authority.
No claim without evidence/gate.
Fallback does not count as strict success.
No generic behavior keyed to literal IDs where a property/tag exists.
No required port without a declared caller and validation path.
Generated docs are never hand-edited.
```

Keep non-negotiable engine rules unchanged unless independently corrected with evidence.

---

# 15. 59A Phase I — Regenerate Rulebooks

Use Plan 29A sync mechanism.

Assert all copies match canonical sections byte-for-byte where required.

No manual thirteen-file edits.

---

# 16. 59A Phase J — Explicit Human-Review Boundary

Create:

```text
docs/process/HUMAN_REVIEW_BOUNDARIES.md
```

or section in `GATES.md`.

Examples that should not be forced into deterministic CI:

- is this fun,
- is the prose emotionally effective,
- art quality,
- musical taste,
- pacing feel.

For each, name the human instrument:

```text
54B playtest
cross-tool narrative review
visual baseline approval
release scorecard review
```

This is essential to prevent gate sprawl.

---

# 17. 59A Phase K — Gate Tiering

Canonical tiers:

## Per Push

Fast, deterministic, high-signal:

- compile/tests,
- data integrity,
- port contract,
- doc sync,
- input-map,
- core liveness.

## Nightly

Expensive coverage/soak:

- balance,
- long-session,
- content utilization,
- snapshot coverage,
- asset sweeps,
- fuzz.

## Release

Final artifact/product:

- release gate,
- scorecard,
- corpus,
- provenance/statements,
- exported artifact boot/load,
- compatibility.

---

# 18. 59A Phase L — Generate `docs/CI.md`

Do not maintain CI table manually.

Generate from:

```text
manifest + workflows
```

Fail if:

- manifest says gate runs but workflow does not,
- workflow runs unknown gate,
- tier mismatches.

---

# 19. 59A Phase M — Gate Retirement Policy

Create criteria.

Candidate for merge/retirement if:

- redundant with stronger gate,
- zero failures over reviewed interval,
- high recurring cost,
- no unique rule coverage.

Retirement requires:

- evidence,
- rule coverage preserved,
- changelog/decision note.

No permanent gate accumulation.

---

# 20. 59A Phase N — Gate Tax Metrics

Track:

```text
gate runtime
failure frequency
false-positive count
maintenance incidents
```

Use this to justify keeping/merging.

---

# 21. 59A Phase O — Plan Template Gate-Coverage Question

Add to intake/plan template:

```text
Which standing gate would have caught this defect class?
Does it exist?
If not, is creating one part of the plan?
```

This turns retrospection into prevention.

---

# 22. 59A Phase P — Core Trend Metrics

Source calls out two primary trends:

```text
EFFECT_PRODUCED catalogs
unbound-port count
```

Record per release in `WAVE_LEDGER.md` or successor metrics ledger.

Desired trend:

```text
effect-produced ↑
unbound required ports → 0
```

---

# 23. 59A Phase Q — Run Full Suite Twice

Procedure:

```text
run all required gates
→ regenerate docs
→ run again
→ git diff
```

Expected:

```text
no generated diff
same gate set
same stable outputs where deterministic
```

This is generator idempotence proof.

---

# 24. 59A Phase R — `generate-gates-doc.py`

Create:

```bash
python3 scripts/ci/generate-gates-doc.py --check
```

Outputs:

```text
docs/process/GATES.md
```

Include:

- gate lineage,
- owners,
- tiers,
- self-proof links,
- implemented state,
- human-review boundary pointer.

---

# 25. 59A Phase S — CURRENT_AUTHORITY Link

`docs/CURRENT_AUTHORITY.md` should point contributors to:

```text
AGENTS.md
GATES.md
plan register
release process
```

No buried audit plans as active authority.

---

# 26. 59A Phase T — Full Gate Reality Check

For each manifest row assert:

```text
script/command exists
self-proof exists
workflow/tier exists
owner exists
```

Do not let “implemented = yes” be prose-only.

---

# 27. 59A Tests

- manifest schema,
- duplicate gate IDs,
- missing owner,
- missing self-proof,
- missing workflow,
- wrong tier,
- stale generated GATES.md,
- scanner equivalence,
- rulebook sync,
- full-suite idempotence.

---

# 28. 59A Definition of Done

- [ ] 22-row lineage register,
- [ ] all finding classes dispositioned,
- [ ] owner per gate,
- [ ] self-proof per gate,
- [ ] shared source scanner,
- [ ] duplicate detectors reduced,
- [ ] durable AGENTS rules,
- [ ] rulebooks regenerated,
- [ ] human-review boundary documented,
- [ ] fast/nightly/release tiering canonical,
- [ ] docs/CI.md generated,
- [ ] gate-retirement policy,
- [ ] gate-tax metrics,
- [ ] intake template asks gate-coverage question,
- [ ] effect-produced/unbound-port trends tracked,
- [ ] full suite idempotent twice,
- [ ] GATES.md generated,
- [ ] CURRENT_AUTHORITY pointer,
- [ ] every gate actually runnable.

---

# 29. Workstream 59B — Publish the Retrospective and Shrink the Plan Layer

## Goal

Produce one honest process history and remove the plan/documentation sprawl the audit series itself created.

---

# 30. 59B Phase A — Retrospective Structure

Create:

```text
docs/process/RETROSPECTIVE-2026-09.md
```

Recommended sections:

1. why the audit existed,
2. what it found,
3. recurring failure patterns,
4. audit-series mistakes,
5. which gates already existed but were not run,
6. process corrections,
7. plan-layer cleanup,
8. final standing operating model,
9. closure statement.

---

# 31. 59B Phase B — Publish the Five Recurring Failure Shapes

Canonical categories:

```text
1. unwired seam
2. invented instead of authored
3. presence measured instead of liveness
4. claim without evidence
5. artifact without provenance
```

For each:

- definition,
- representative examples,
- canonical gate/rule,
- current owner.

This is the reusable residue of nine waves.

---

# 32. 59B Phase C — Evidence Baseline Table

Use source numbers as historical baseline only where still verified.

Historical examples:

```text
5,563 IDs
4 EFFECT_PRODUCED catalogs at one baseline
74/147 integration seams with no host caller
50/5,563 asset IDs checked at one baseline
30 routed consoles with no authority
452 dead definitions
22/27 event kinds dropped
0 tags/shaders/tweens/gamepad bindings at the relevant baseline
```

Every current value must be regenerated, not copied forward blindly.

Table:

| Metric | Historical baseline | Current | Source/generator | Gate |
|---|---:|---:|---|---|

---

# 33. 59B Phase D — Include Audit Errors

Mandatory.

Document at least:

- the early incorrect producer premise,
- the Wave-5 hotkey overstatement/correction.

For each:

```text
original claim
why it was wrong
corrected evidence
which claims rule/gate would have prevented it
```

This gives the retrospective credibility.

---

# 34. 59B Phase E — Existing-But-Unrun Gate Ratio

Answer:

```text
Of 22 finding classes, how many were discoverable by a gate/check already present but not being run?
```

This is a key process metric.

Method:

- classify each historical finding,
- identify check availability at discovery date,
- calculate ratio,
- document assumptions.

No hand-wavy count.

---

# 35. 59B Phase F — Plan Register Sweep

Run:

```bash
python3 scripts/ci/generate-plan-register.py --check
```

Before archival, every plan receives status:

```text
executed
superseded
proposed
blocked-by
```

No unclassified plan file.

---

# 36. 59B Phase G — Active Plan Cap

Define a maximum active set.

Example concept:

```text
one current wave
+ limited next band
```

Do not allow hundreds of simultaneously “active” plans.

Exact cap should follow 53A/53C process.

---

# 37. 59B Phase H — Archive Executed/Superseded Plans

Move/archive per existing docs architecture.

Do not delete history.

Archive retains:

- filename,
- status,
- superseded-by/implemented-by reference,
- date.

---

# 38. 59B Phase I — Reconcile Parallel Expansion Series

Use 53B merge clusters.

For overlapping plans:

```text
merge
supersede
or
assign distinct authority
```

The continuity audit must not remain a competing backlog.

---

# 39. 59B Phase J — Correct AGENTS Record

Source flags stale categories including:

- H5,
- H7,
- H11,
- GameBootstrap phase instruction,
- bit-era VCS section,
- asset-debt paragraph.

For each correction:

```text
old claim
current file:line evidence
new text
```

Do not edit based on memory.

---

# 40. 59B Phase K — Correct README / Registry

Fix stale engine/architecture claims.

Apply claims gate afterward.

---

# 41. 59B Phase L — Retire Master-Plan Monolith

Convert:

```text
ASHFALL_UNIFIED_MASTER_EXECUTION_PLAN.md
sources.md
```

into either:

- archive,
- link/index page,
- pointer to current roadmap/wave ledger/register.

Do not keep a 93KB shadow source of truth.

---

# 42. 59B Phase M — Metric Lineage

Create one table containing every major metric prior waves intended to move.

Fields:

```text
metric_id
historical baseline
current value
generator
gate
owner
release trend
```

This lets contributors verify “we improved it.”

---

# 43. 59B Phase N — Failure Mode Catalogue

Create:

```text
docs/process/FAILURE_MODES.md
```

Target ~12 concise review items.

Suggested:

1. seam exists but no caller,
2. authored fact inferred,
3. route has no live authority,
4. loader exists but no effect,
5. fallback counted as success,
6. state has multiple writers,
7. save capture/restore asymmetric,
8. event kind emitted but dropped,
9. artifact lacks provenance,
10. docs claim unverified number,
11. generated file hand-edited,
12. player action has no handler.

Each row links:

```text
grep/scanner
gate
owner
```

---

# 44. 59B Phase O — Cross-Tool Review

The retrospective should be reviewed by a different analysis/tool path than the authoring one.

Review scope:

```text
Does every factual claim have evidence?
Does every gate have a self-proof?
Does the retrospective include its own errors?
Does it accidentally create new backlog?
```

Record review result.

---

# 45. 59B Phase P — Audit-Layer Deletion Policy

Explicitly state:

```text
wave indexes remain history
executed plans become archived
Next-steps-plans stops accepting free-form new documents
new work enters through intake
```

This is the behavioral stop condition.

---

# 46. 59B Phase Q — Archive Index

Update:

```text
docs/ARCHIVE_INDEX.md
```

Include:

- nine audit waves,
- old master plans,
- retired plan docs,
- retrospective.

---

# 47. 59B Phase R — Docs Index Regeneration

Run docs index generator.

No broken historical links.

---

# 48. 59B Phase S — Plan-Folder Size Metric

Track:

```text
files before
active after
archived after
```

Success metric:

```text
active plan folder stops autonomous growth
```

Not necessarily smaller Git history.

---

# 49. 59B Tests/Gates

- plan register complete,
- archive links valid,
- claims gate,
- rulebook sync,
- docs index,
- no free-form active plan without register entry,
- master-plan shadow-authority check,
- retrospective claims review.

---

# 50. 59B Definition of Done

- [ ] retrospective published,
- [ ] five failure shapes documented,
- [ ] historical/current metric table,
- [ ] audit mistakes included,
- [ ] unrun-gate ratio measured,
- [ ] all plans classified,
- [ ] active plan set capped,
- [ ] executed/superseded plans archived,
- [ ] parallel plan overlap reconciled,
- [ ] AGENTS corrected,
- [ ] README/registry corrected,
- [ ] master-plan monolith retired,
- [ ] metric lineage table,
- [ ] FAILURE_MODES.md,
- [ ] cross-tool evidence review,
- [ ] audit-layer intake-only policy,
- [ ] archive index,
- [ ] docs index regenerated,
- [ ] plan-folder active size reduced/bounded.

---

# 51. Workstream 59C — Hand Over From Auditing to Operating

## Goal

Define the permanent operating cadence and exact conditions under which any future audit is allowed.

---

# 52. 59C Phase A — Formal Closure Statement

Write in:

```text
docs/process/RETROSPECTIVE-2026-09.md
docs/roadmap/WAVE_LEDGER.md
```

Statement:

```text
Continuity audit series closes at Wave 9.
```

Successor loop:

```text
intake
→ one wave
→ acceptance
→ scorecard
→ release
→ periodic review
```

---

# 53. 59C Phase B — Audit Trigger Policy

Create:

```text
docs/process/AUDIT_TRIGGERS.md
```

Allowed triggers only.

Source-inspired list:

1. a gate fails and cannot be explained by normal ownership,
2. same bug class reported twice by users,
3. a wave’s premise checks materially disagree,
4. platform/store requirement changes,
5. annual scheduled review.

No trigger:

```text
no audit
```

---

# 54. 59C Phase C — Audit Scope Contract

If trigger occurs:

- define the narrow question,
- define affected systems,
- define expected output,
- do not create an open-ended plan series automatically.

A future audit is an exception process.

---

# 55. 59C Phase D — Standing Cadence

Create:

```text
docs/process/CADENCE.md
```

## Per Push

Fast CI.

## Nightly

- soak,
- coverage,
- balance,
- assets,
- fuzz where configured.

## Per Release

- release gate,
- scorecard,
- statements/provenance,
- compatibility/corpus,
- artifact boot.

## Monthly

Balance review.

## Quarterly

- claims review,
- plan register prune,
- exemptions review,
- gate retirement review.

## Annually

Audit-trigger review.

---

# 56. 59C Phase E — Standing Report Registry

Canonical list of nine standing reports.

Suggested:

```text
1. content acceptance ladder
2. port contract
3. asset coverage
4. slice scorecard
5. funnel/balance sweep
6. plan register
7. provenance report
8. doctor report
9. release report
```

Each declares:

```text
generator
owner
cadence
--check command
artifact path
```

---

# 57. 59C Phase F — Generated Report Contract

Every standing report must be:

- generated,
- deterministic where expected,
- owned,
- checkable,
- referenced from current authority.

No manual spreadsheet/dashboard as primary truth.

---

# 58. 59C Phase G — Red-Gate Escalation

Create one standard exemption schema.

Fields:

```text
gate_id
owner
reason
date
expiry
risk
linked issue/plan
```

Release rule:

```text
required red + no valid exemption
→ release blocked
```

Expired exemption:

```text
red
```

---

# 59. 59C Phase H — Exemption Review

Quarterly:

- expire,
- renew with evidence,
- fix,
- retire gate if truly obsolete.

No immortal allowlists.

---

# 60. 59C Phase I — Process Metrics

Track:

```text
cycle time per wave
findings per release
gates added
gates retired
red gates at release candidate
unbound ports
EFFECT_PRODUCED
active plan count
expired exemptions
```

Do not optimize process metrics blindly; use them diagnostically.

---

# 61. 59C Phase J — Public Build Order

Source provides first-band high-value tasks:

```text
19A
22A
24A
29A
31A
34B.1
36A
40A
44A
45A
48A
50A
54A
```

Publish once in roadmap as the first band.

After that:

```text
intake governs
```

Do not continuously hand-maintain an ever-growing priority monolith.

---

# 62. 59C Phase K — Archive Audit Wave Indexes

Move/reference nine wave indexes under:

```text
docs/archive/audits/
```

with current-state pointer.

The archive is read-only historical input.

---

# 63. 59C Phase L — New Contributor Onboarding Page

Create:

```text
docs/process/ONBOARDING.md
```

Six screens in order:

```text
1. AGENTS.md
2. design pillars
3. gates
4. plan register
5. slice scorecard
6. doctor output
```

Goal:

```text
nine waves of context
→ six canonical entry points
```

---

# 64. 59C Phase M — One-Sentence “Done” Definition

Put in:

```text
docs/design/PILLARS.md
```

Must be:

- falsifiable,
- aligned with 54C scorecard,
- product-facing rather than “all tests green.”

Example shape:

```text
ASHFALL is done when a release artifact can sustain the intended campaign loop, content reachability, continuity, accessibility, save durability, and presentation quality at the scorecard bar without unresolved critical gates.
```

Use final wording after project review.

---

# 65. 59C Phase N — Loop Ownership

Assign roles/names for:

```text
release runner
scorecard reviewer
exemption reviewer
gate owner
plan-register curator
```

Source explicitly requires ownership to be named.

If one person fills several roles, still list them separately.

---

# 66. 59C Phase O — Closing Full-Stack Run

Run, for real:

```bash
bash scripts/ci/verify-fast.sh
bash scripts/ci/release-gate.sh
bash scripts/ci/doctor.sh
```

plus:

```text
slice scorecard
save corpus gate
generated report checks
```

Capture outputs as closing artifact.

---

# 67. 59C Phase P — Closing Record

Create:

```text
docs/archive/audits/WAVE9_CLOSING_RECORD.md
```

Include:

- commit SHA,
- gate counts,
- red/green state,
- report hashes,
- plan-folder counts,
- current unbound ports,
- current EFFECT_PRODUCED,
- active exemptions,
- archive pointers.

---

# 68. 59C Phase Q — Any Red Becomes Intake

If final run finds red:

```text
create normal intake ticket
assign owner
do not create Wave 10
```

This is the most important behavioral test of the handover.

---

# 69. 59C Phase R — Disable Free-Form Audit Intake

Where process/tools allow, require:

```text
new plan file
→ register/intake metadata
```

Do not accept orphan plans into active folders.

---

# 70. 59C Phase S — Review the Operating Cycle After One Release

Schedule a normal retrospective on the process after first release under the new cadence.

This is not a continuity audit.
It is process review.

---

# 71. 59C Tests/Gates

- audit trigger schema,
- cadence docs linked,
- standing report registry complete,
- every report has generator/owner/check,
- exemption expiry test,
- onboarding links valid,
- done statement present,
- closing run artifact complete,
- new free-form plan rejected/flagged.

---

# 72. 59C Definition of Done

- [ ] audit formally closed,
- [ ] successor loop named,
- [ ] AUDIT_TRIGGERS.md,
- [ ] no-trigger/no-audit rule,
- [ ] CADENCE.md,
- [ ] nine standing reports registered,
- [ ] all reports owned/generated/checkable,
- [ ] red-gate escalation schema,
- [ ] exemption expiry,
- [ ] process metrics,
- [ ] first build band published,
- [ ] audit wave indexes archived,
- [ ] six-screen onboarding page,
- [ ] falsifiable done statement,
- [ ] loop owners named,
- [ ] full-stack closing run,
- [ ] closing record,
- [ ] reds become intake tickets,
- [ ] free-form plan growth blocked.

---

# 73. The 22-Finding Prevention Matrix

The final implementation should generate a matrix similar to:

| Historical Finding Class | Canonical Prevention |
|---|---|
| hardcoded derived outcome | derived-state assertion |
| dead/unwired seam | port-contract gate |
| routed but unbound panel | panel-liveness gate |
| parsed but inert content | content-acceptance ladder |
| fake source of truth | authority/single-writer rule |
| state mutation on temporary copy | behavioral/state-equivalence test |
| null required callback | port-contract gate |
| authority ignored by behavior | behavior-per-authority test |
| stale docs/claims | claims + rulebook sync |
| dropped event vocabulary | vocabulary-contract gate |
| registry numerical fiction | claims gate |
| dead input action | input-map gate |
| authored fact inferred | authored-not-inferred rule |
| state written but not consumed | behavior-per-authority test |
| unproducible balance artifact | reproducible-sweep gate |
| release identity absent | release-prep gate |
| fallback counted as coverage | strict asset gate |
| runtime orphan artifacts | hygiene/manifest gate |
| repository weight/root junk | doctor/hygiene |
| provenance statement stale | provenance generator |

The exact final row count remains 22 per source and must be generated from the canonical register, not manually frozen in this plan.

---

# 74. Five Failure Shapes Contract

## Shape 1 — Unwired Seam

Pattern:

```text
behavior exists
but no production caller/binding
```

Prevention:

- port contract,
- liveness tests,
- call-site scan.

---

## Shape 2 — Invented Instead of Authored

Pattern:

```text
runtime guesses a fact already present in content authority
```

Prevention:

- authored-not-inferred rule,
- content acceptance,
- one-authority test.

---

## Shape 3 — Presence Instead of Liveness

Pattern:

```text
class/file/route exists
so project claims feature exists
```

Prevention:

- runtime effect evidence,
- panel liveness,
- click-through journeys,
- utilization tiers.

---

## Shape 4 — Claim Without Evidence

Pattern:

```text
documentation/registry says a number/state
without a generator/check
```

Prevention:

- claims gate,
- generated docs,
- CURRENT_AUTHORITY.

---

## Shape 5 — Artifact Without Provenance

Pattern:

```text
CSV/image/save/report exists
but nobody can reproduce or attribute it
```

Prevention:

- manifest/provenance,
- sweep generator,
- save corpus manifest,
- release report.

---

# 75. Gate Manifest Contract

Every gate manifest row should contain:

```text
gate_id
title
failure_class
source_plan
owner
tier
command
self_proof
expected_summary
runtime_cost_class
release_blocking
documentation
```

---

# 76. Gate Owner Contract

An owner is responsible for:

- keeping command runnable,
- maintaining fixture,
- triaging failures,
- proposing retirement/merge,
- reviewing exemptions.

---

# 77. Gate Self-Proof Contract

The fixture must violate the exact invariant.

A gate is not self-proven by simply running on a healthy repository.

---

# 78. Shared Scanner Contract

The scanner provides evidence.

It does not decide every domain semantic automatically.

Domain gates still own classification rules.

---

# 79. Human Review Boundary Contract

Human-only checks must still be:

- named,
- scheduled,
- recorded.

“Not gated” does not mean “ignored.”

---

# 80. Gate Retirement Contract

A gate may retire only if:

```text
rule coverage survives
or
stronger merged gate covers it
```

Retirement is a documented decision.

---

# 81. Plan Register Contract

Every active plan must have:

```text
plan id
status
owner
dependencies
intake source
current authority
```

No unregistered plan in active directories.

---

# 82. Plan Status Contract

Allowed statuses:

```text
proposed
accepted
active
blocked
executed
superseded
archived
```

Map to existing 53A schema.

---

# 83. Active-Plan Cap Contract

The process should enforce a limited WIP set.

This reduces:

- contradictory instructions,
- duplicated plans,
- stale dependencies,
- agent context sprawl.

---

# 84. Master-Plan Retirement Contract

A master index may exist.

A giant duplicated execution monolith should not.

Current authority belongs to:

```text
roadmap
register
wave ledger
gates
```

---

# 85. Metric Lineage Contract

Every program metric answers:

```text
historical baseline
current value
generator
owner
gate
trend
```

No copied number without lineage.

---

# 86. Claims Contract for the Retrospective

The retrospective itself must pass claims review.

Especially:

- numeric counts,
- gate availability dates,
- plan-file counts,
- historical error statements.

---

# 87. Audit Trigger Contract

A future audit requires an enumerated trigger.

No “we have not audited this recently” justification unless annual trigger applies.

---

# 88. Audit Scope Contract

A triggered audit must have:

```text
question
scope
evidence
timebox
closure condition
```

No open-ended wave by default.

---

# 89. Exemption Contract

Exemption fields:

```text
gate
owner
expiry
reason
risk
linked ticket
```

Expired exemption is treated as no exemption.

---

# 90. Release Red-Gate Contract

Release gate refuses required red states.

No green-by-rerun culture.

---

# 91. Standing Report Contract

Every standing report is:

```text
generated
owned
versioned
checkable
linked from CURRENT_AUTHORITY
```

---

# 92. Doctor Contract

`doctor.sh` becomes the contributor’s environment/repository-health entry point.

It should report:

- critical tooling,
- gate manifest health,
- generated-doc drift,
- obvious repository hygiene issues,
- current active-plan/register status where cheap.

Do not turn doctor into a full release gate.

---

# 93. Onboarding Contract

New contributor reads only canonical live docs first.

Historical wave plans are optional archaeology.

---

# 94. Done Definition Contract

“Done” must be falsifiable.

Not:

```text
when it feels complete
```

Not:

```text
when all plans are implemented
```

It should reference:

- release artifact,
- scorecard,
- gates,
- intended player loop.

---

# 95. Process Measurement Contract

Track process health without gamifying it.

Useful trends:

- finding recurrence,
- gate runtime,
- plan WIP,
- unresolved exemptions,
- release cycle time.

---

# 96. Closure Record Contract

The closing record is the immutable handoff artifact.

It should allow a later contributor to answer:

```text
What was green when the audit ended?
What was still open?
What became the standing process?
```

---

# 97. Failure Modes

## Gate exists but workflow never runs it

Manifest/workflow consistency gate fails.

## Gate has no owner

59A manifest validation fails.

## Gate always passes but has no self-proof

Gate incomplete.

## Shared scanner changes outputs subtly

Equivalence tests catch migration drift.

## AGENTS updated manually in only one copy

Rulebook sync fails.

## Subjective tone gets a brittle CI score

Move to human review boundary.

## Retrospective hides its own mistakes

Retrospective review fails source requirement.

## Plan archive deletes history

Archive links/receipts required.

## Master plan remains shadow authority

CURRENT_AUTHORITY/doc check fails.

## Final closing run finds red and author creates Wave 10

Process violation: create intake ticket instead.

---

# 98. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| 59A lands before gates exist | Medium | High | program-level sequencing guard |
| gate registry becomes stale | Medium | High | generated manifest/docs/workflow check |
| self-proof fixtures become expensive | Medium | Medium | targeted minimal fixtures |
| scanner abstraction overcomplicates scripts | Medium | Medium | narrow project utility |
| gate sprawl | High | Medium | human boundary + retirement policy |
| retrospective becomes marketing | Medium | High | include audit errors + cross-tool review |
| plan cleanup breaks references | Medium | Medium | archive index + link gate |
| active-plan cap ignored | Medium | High | intake/register enforcement |
| process metrics become vanity targets | Medium | Medium | diagnostic-only interpretation |
| audit closure immediately spawns new series | Medium | Critical | no-trigger/no-audit rule |

---

# 99. Commit Strategy

## Program Precondition

Do not start implementation commits for 59A until earlier required gates have landed.

A preliminary read-only inventory may be prepared earlier, but `implemented` status remains false until real.

---

## 59A

### C2[25].1 — 22-row gate lineage inventory

### C2[25].2 — gate owner/self-proof metadata

### C2[25].3 — shared source-scan utility

### C2[25].4 — migrate duplicate detectors

### C2[25].5 — canonical durable AGENTS rules

### C2[25].6 — rulebook regeneration + claims integration

### C2[25].7 — gate tiers + workflow consistency

### C2[25].8 — gate retirement/tax metrics

### C2[25].9 — generated GATES.md + CURRENT_AUTHORITY

### C2[25].10 — full-suite idempotence proof

### Gate: 59A complete

---

## 59B

### C2[25].11 — retrospective skeleton + five failure shapes

### C2[25].12 — historical/current metrics + audit-error section

### C2[25].13 — unrun-gate ratio analysis

### C2[25].14 — plan register complete-status sweep

### C2[25].15 — archive executed/superseded plans

### C2[25].16 — parallel-series reconciliation

### C2[25].17 — AGENTS/README/registry truth corrections

### C2[25].18 — master-plan monolith retirement

### C2[25].19 — metric lineage + FAILURE_MODES.md

### C2[25].20 — cross-tool review + archive index

### Gate: 59B complete

---

## 59C

### C2[25].21 — audit closure + AUDIT_TRIGGERS

### C2[25].22 — CADENCE + standing-report registry

### C2[25].23 — exemption/escalation model

### C2[25].24 — process metrics + first roadmap band

### C2[25].25 — archive wave indexes

### C2[25].26 — contributor ONBOARDING + done statement

### C2[25].27 — named loop ownership

### C2[25].28 — full-stack closing run

### C2[25].29 — WAVE9_CLOSING_RECORD

### C2[25].30 — active-plan/intake enforcement

### Gate: 59C complete

---

# 100. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/ci/generate-gates-doc.py --check
python3 scripts/ci/generate-plan-register.py --check
python3 scripts/ci/sync-agent-rulebooks.py --check
bash scripts/ci/verify-fast.sh
```

Also run repository-canonical equivalents of:

```text
doc-link-gate
docs-index --check
per-gate self-proof suite
release-gate.sh
doctor.sh
slice scorecard
save corpus gate
content acceptance report
port contract report
asset coverage report
provenance report
```

Critical idempotence proof:

```bash
bash scripts/ci/verify-fast.sh
bash scripts/ci/verify-fast.sh
git diff --exit-code
```

or equivalent generated-artifact diff check.

---

# 101. Flagship Definition of Done

## 59A — Gates and Rules

- [ ] all 22 finding classes registered,
- [ ] gate/rule/human-review disposition for each,
- [ ] owner for each gate,
- [ ] self-proof for each gate,
- [ ] shared source scanner,
- [ ] duplicate detectors reduced,
- [ ] durable instruction rules canonical,
- [ ] rulebooks regenerated,
- [ ] subjective gate boundary explicit,
- [ ] fast/nightly/release tiering,
- [ ] docs/CI generated from manifest/workflow,
- [ ] retirement policy,
- [ ] gate-tax metrics,
- [ ] intake asks prevention-gate question,
- [ ] EFFECT_PRODUCED trend,
- [ ] unbound-port trend,
- [ ] whole suite idempotent twice,
- [ ] GATES.md generated,
- [ ] CURRENT_AUTHORITY updated.

## 59B — Retrospective and Compression

- [ ] honest retrospective,
- [ ] five recurring failure shapes,
- [ ] historical/current baseline table,
- [ ] audit errors included,
- [ ] already-existed-but-unrun gate ratio,
- [ ] complete plan register statuses,
- [ ] active-plan cap,
- [ ] executed/superseded plans archived,
- [ ] parallel plan series reconciled,
- [ ] AGENTS stale claims fixed,
- [ ] README/registry truth corrected,
- [ ] master-plan monolith retired,
- [ ] metric lineage,
- [ ] FAILURE_MODES.md,
- [ ] different-tool review,
- [ ] audit-plan intake-only policy,
- [ ] archive index,
- [ ] docs index regenerated,
- [ ] active plan count bounded.

## 59C — Handover

- [ ] Wave 9 closure declared,
- [ ] successor operating cycle named,
- [ ] exhaustive audit triggers,
- [ ] no-trigger/no-audit rule,
- [ ] CADENCE.md,
- [ ] nine standing reports,
- [ ] generator/owner/check per report,
- [ ] red-gate escalation,
- [ ] expiry-based exemptions,
- [ ] process metrics,
- [ ] first roadmap band,
- [ ] audit indexes archived,
- [ ] six-screen onboarding,
- [ ] falsifiable done statement,
- [ ] loop ownership assigned,
- [ ] closing full-stack run,
- [ ] closing record published,
- [ ] any reds converted to intake tickets,
- [ ] free-form plan growth blocked.

## Global

- [ ] no unowned gate,
- [ ] no gate without self-proof,
- [ ] no tenth wave,
- [ ] no historical deletion,
- [ ] no subjective fake-CI gate,
- [ ] no duplicate instruction authority,
- [ ] no plan-folder autonomous growth,
- [ ] no release on unexplained required reds,
- [ ] full verification green or remaining reds explicitly ticketed under standing intake.

---

# 102. Closure Report Template

```markdown
## C2[25] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- Closing release/tag:

### Program Preconditions
- Prior gate implementations complete:
- Missing prerequisite gates:
- 59A allowed to land:
- Result:

### 59A — Gate Register
- Historical finding classes:
- Gated:
- Ruled:
- Human review:
- Explicitly not gated:
- Gates with owners:
- Gates with self-proofs:
- Duplicate scanners before:
- Duplicate scanners after:
- Fast gates:
- Nightly gates:
- Release gates:
- Full-suite run 1:
- Full-suite run 2:
- Idempotence diff:
- Result:

### 59B — Retrospective
- Retrospective:
- Five failure shapes:
- Historical metrics:
- Current metrics:
- Audit errors documented:
- Already-existed-but-unrun ratio:
- Plans total:
- Active plans before:
- Active plans after:
- Executed archived:
- Superseded archived:
- Parallel overlaps resolved:
- AGENTS corrections:
- README corrections:
- Master-plan retirement:
- Metric lineage:
- Failure catalogue:
- Cross-tool review:
- Result:

### 59C — Handover
- Audit triggers:
- Cadence:
- Standing reports:
- Reports with owners:
- Reports with --check:
- Exemptions active:
- Exemptions expired:
- Process metrics:
- First roadmap band:
- Audit indexes archived:
- Onboarding:
- Done statement:
- Loop owners:
- Closing full-stack run:
- Reds discovered:
- Intake tickets created:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- GATES generator:
- Plan register:
- Rulebook sync:
- Doc links:
- Docs index:
- Verify fast #1:
- Verify fast #2:
- Release gate:
- Doctor:
- Slice:
- Corpus:

### Final Metrics
- FINDING_CLASSES_TOTAL:
- FINDING_CLASSES_GATED:
- FINDING_CLASSES_RULED:
- GATES_UNOWNED:
- GATES_WITHOUT_SELF_PROOF:
- DUPLICATE_SCANNERS:
- EFFECT_PRODUCED:
- UNBOUND_REQUIRED_PORTS:
- ACTIVE_PLAN_COUNT:
- ARCHIVED_PLAN_COUNT:
- RED_REQUIRED_GATES:
- EXPIRED_EXEMPTIONS:
- STANDING_REPORTS_HEALTHY:
- WAVE10_CREATED: 0

### Remaining Intake
- Ticket:
- Owner:
- Gate:
- Severity:
- Expiry if exempted:
```

---

# 103. Final Execution Directive

Execute Plan 59 as the **end of open-ended continuity auditing**.

The critical sequence is:

```text
wait until the earlier gates actually exist
→ map all 22 historical finding classes to standing prevention
→ assign owners
→ prove every gate can fail
→ deduplicate scanners
→ encode durable rules once
→ define the human-review boundary
→ publish the retrospective including the audit's own mistakes
→ classify/archive plan sprawl
→ correct stale authority docs
→ publish standing cadence and audit triggers
→ run the full operating stack once
→ archive the audit series
→ route every future issue through intake
```

Do not land 59A early just to produce a pretty register.

Do not hide a missing gate behind “planned.”

Do not create a gate for subjective taste merely because the audit prefers automation.

Do not publish a retrospective that excludes the series’ own errors.

Do not delete the historical waves.

Do not create Wave 10 because the closing run finds another bug.

The strongest prevention rule is:

> **Every recurring failure class must terminate in a standing gate, a durable instruction rule, or an explicit human-review decision with an owner.**

The strongest retrospective rule is:

> **The audit must apply its own evidence standard to itself, including corrections of claims it got wrong.**

The strongest operating rule is:

> **New work enters through intake; standing reports and gates govern execution; release is the final acceptance point; a new audit occurs only when an enumerated trigger is met.**

The flagship acceptance scenario is:

> **Run the complete standing process from a clean repository: intake/register validation, fast gates, nightly/report checks where practical, slice scorecard, save corpus, doctor, release gate, generated docs, and closing metrics. If any required check is red, create an owned intake ticket or expiring exemption. Do not open a new audit wave. Publish the closing record, archive Waves 1–9, and leave the repository with fewer active plans, more owned gates, and a process that can operate without another continuity audit.**
