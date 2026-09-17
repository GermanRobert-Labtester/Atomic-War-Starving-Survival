# C2 — Flagship Integration Plan [20]: Reproducible Balance Evidence, Local Play Metrics, and Measurement-Driven Difficulty Decisions

> **Deliverable:** `C2_planintegration[20].md`
> **Source scope:** Plan 46 — *Playable Metrics: Measure the Player, Decide the Difficulty*
> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> **Primary objective:** make all balance artifacts reproducible and attributable, add local/private/opt-in play-session measurement over existing action/event instrumentation, then establish a disciplined decision loop that changes difficulty, guidance, and content reachability only when named evidence supports the change.
> **Required execution order:** **46A → 46B → 46C**
> **Wave dependency context:** 31C day record, 34B difficulty presets, 26C performance budgets, 17B onboarding/guidance, 39A release gate, 45A/45B content rails/reachability.
> **Scope discipline:** no network telemetry, no per-player identity, no remote config, no silent adaptive difficulty, no manual/off-repo balance producer, no balance claim without scenario+seed+build SHA, and no privacy-sensitive prose/path/user data in metric records.

---

# 0. Executive Intent

ASHFALL already produces useful simulation evidence:

- seeded balance CSVs,
- deterministic day progression,
- needs/radiation outcomes,
- runtime-scale metrics,
- content-utilization measurements,
- semantic day events,
- action sigils.

The current defect is not lack of data.

It is lack of **provenance, reproducibility, and decision linkage**.

Today a balance CSV can exist without answering:

```text
Which build produced this?
Which scenario?
Which seed?
Which policy?
Which difficulty preset?
Which tuning decision did it support?
```

Likewise, the game can pass machine selftests without answering:

```text
Did a player understand the first hour?
Where did they get stuck?
Which panel did they repeatedly open?
Did they ever craft?
Did they understand ration policy?
Did keyboard/controller work in actual use?
```

C2[20] makes the measurement layer explicit:

```text
deterministic scenario definition
→ reproducible sweep
→ generated metrics
→ documented target
→ player-action/session metrics
→ funnel/dead-end/stuck analysis
→ evidence-backed decision
→ tuning/guidance/content change
→ next sweep verifies expected movement
```

The flagship outcome is:

> **Every balance claim can be regenerated from a named scenario, every player-facing usability claim can be supported by a privacy-safe local report, and every release carries at least one documented design decision tied to measurable evidence.**

---

# 1. Source Diagnosis

The source plan establishes:

- 27 balance CSVs exist but have no in-repo producer.
- Nothing references those artifacts in docs/scripts.
- No balance design-record home exists.
- No player-side telemetry exists.
- Existing action sigils provide a seed for instrumentation.
- Day-event streams provide the consequence half of player-action analysis.
- performance sampling is being hardened elsewhere.
- onboarding is currently validated only by machine checks.
- difficulty presets are not meaningful without measured targets.
- content utilization measures implementation reachability but not player experience.

The architectural reading is:

```text
simulation measurement exists
player measurement does not
decision provenance exists nowhere
```

The solution is to connect all three.

---

# 2. Program-Level Success Criteria

C2[20] closes only when:

1. Every retained balance artifact has a reproducible in-repo producer.
2. Every sweep records commit/build/scenario/seed metadata.
3. Balance scenarios are data-authored.
4. Balance targets are explicit and versioned.
5. Tuning decisions cite sweep evidence.
6. Nightly drift tests detect unintended balance movement.
7. Player metrics are local-only and off by default in release.
8. Player metrics have no network path.
9. Metric records contain stable IDs, not prose or identifying data.
10. Action metrics and day-state consequences can be correlated.
11. First-hour funnel metrics are generated.
12. Dead-end/stuck behavior can be detected.
13. Synthetic players exercise the same recorder in CI.
14. Files are size-bounded and rotated.
15. Opt-out produces no telemetry file.
16. Reports are locally aggregated into readable tables.
17. Release acceptance includes a metrics/funnel review.
18. No adaptive difficulty exists without explicit authored rules.
19. Content reachability and player reachability can be compared.
20. At least one design change per release is traceable to measurement.

---

# 3. Architectural Invariants

## 3.1 One reproducible balance harness

No off-repo skill/manual script is the authoritative producer.

## 3.2 Scenarios are data

Scenario definitions belong in version-controlled JSON or equivalent.

## 3.3 Results are attributable

Every run records:

- Git SHA,
- scenario,
- seed,
- difficulty,
- policy,
- build/runtime versions.

## 3.4 Targets are explicit

“Feels fair” is not a machine contract.

## 3.5 Metrics recorder is local only

No upload code.
No endpoint.
No device/account identity.

## 3.6 Metrics use IDs, not text

No player-generated prose.
No usernames.
No absolute paths.

## 3.7 Opt-out means zero writes

Not “record but ignore later.”

## 3.8 Action and consequence are separate channels

Player action stream + day-state stream are joined analytically.

Do not overload semantic day events to mean player intent.

## 3.9 Synthetic players use the same recorder

No fake metrics-only path.

## 3.10 Metrics never silently change difficulty

Measurement informs authored decisions.
It does not become a hidden director.

---

# 4. Dependency Graph

```text
31C day record ───────────────► consequence stream
34B difficulty presets ───────► scenario/preset labels
26C perf budgets ─────────────► balance/perf drift
17B onboarding ───────────────► funnel steps
39A release gate ─────────────► release metrics gate
45A/45B rails ────────────────► reachability evidence

46A reproducible sweeps
 │
 ▼
46B local play-session metrics
 │
 ▼
46C measurement-driven decisions
```

Required order:

```text
46A → 46B → 46C
```

---

# 5. Baseline Capture

Before modification, record:

- current `artifacts/balance` file list,
- CSV schemas,
- scenario names inferred from filenames,
- seeds,
- day counts,
- existing performance harness APIs,
- current action sigil sites,
- current day-event schema,
- current settings schema,
- current CI/nightly structure,
- current content utilization outputs,
- current onboarding/selftest steps.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Archive hashes of the 27 existing CSVs before re-generation.

---

# 6. Workstream 46A — Reproducible Balance Corpus

## Goal

Every balance artifact can be regenerated and every tuning decision can cite the exact sweep that justified it.

---

# 7. 46A Phase A — Reconstruct the Producer

Trace existing CSV columns back to the existing performance/campaign harness.

Document:

```text
current column
→ source authority
→ harness getter/query
```

Do not delete old artifacts until a reproducible producer exists.

---

# 8. 46A Phase B — In-Repo Sweep Runner

Create:

```text
scripts/balance/run_sweep.py
```

Responsibilities:

- load scenario JSON,
- run canonical executable/harness,
- collect CSV/JSON result,
- inject provenance header,
- write deterministic output,
- fail on missing expected columns.

---

# 9. 46A Phase C — Scenario Schema

Create:

```text
scripts/balance/scenarios/*.json
```

Recommended fields:

```text
scenario_id
description_key/docs note
difficulty_preset_id
seed_set
days
roster_profile
policy_script
world/weather assumptions
feature toggles
expected_metrics
```

Keep scenario definitions diff-reviewable.

---

# 10. 46A Phase D — Canonical Scenario IDs

Migrate source examples into explicit scenarios:

- daily ration,
- scarcity,
- severe scarcity,
- power/economy combined stress,
- season severity,
- load shedding,
- gear lifespan,
- ration policy,
- relation effects.

Use actual current naming.

---

# 11. 46A Phase E — Scripted Player Policies

Define deterministic policy profiles.

Suggested:

```text
cautious
greedy
neglectful
expert
```

Each policy defines explicit action priorities.

No random actions except through seeded simulation mechanics.

---

# 12. 46A Phase F — Run Metadata Header

Every generated artifact records:

```text
git_sha
scenario_id
seed
difficulty_preset_id
policy_id
day_count
build_config
dotnet_version
godot_version
generated_utc
harness_version
```

If CSV comments are awkward, pair with a manifest JSON.

---

# 13. 46A Phase G — Canonical Metrics Panel

Generate per sweep:

```text
survival day 7/30/90
time to first critical need
time to first death
morale floor
radiation dose curve
lifetime exposure
resource exhaustion day
power failure hours
major policy events
```

Only include metrics supported by current systems.

---

# 14. 46A Phase H — `docs/balance/README.md`

Generate summary tables from artifacts.

Do not hand-edit numerical result rows.

Separate:

- generated current results,
- human interpretation.

---

# 15. 46A Phase I — Define `TARGETS.md`

Create:

```text
docs/balance/TARGETS.md
```

For each difficulty preset:

```text
metric
target/range
scenario/policy
rationale
noise floor
review date
owner
```

Do not invent target values in code.

Initial values must be chosen from product intent plus measured baseline.

---

# 16. 46A Phase J — Difficulty Target Philosophy

Define what each preset is intended to do.

Examples:

- crisis timing,
- survival probability under competent policy,
- resource exhaustion window,
- damage/recovery pressure.

Avoid defining difficulty only as multipliers.

Targets describe outcome experience.

---

# 17. 46A Phase K — Decision Log

Create:

```text
docs/balance/DECISIONS.md
```

Each ADR entry:

```text
date
decision id
tuning changed
reason
scenario
seed set
before metrics
after metrics
preset(s)
expected effect
rollback condition
owner
```

No tuning change without evidence link once this system is established.

---

# 18. 46A Phase L — Nightly Drift Gate

Reference scenario set runs nightly.

Compare to approved baseline.

Fail only on meaningful defined drift.

Examples:

```text
day-30 survival delta > tolerance
first-critical median shifts outside band
dose curve delta > threshold
```

Thresholds live in canonical target/budget config.

---

# 19. 46A Phase M — Noise Floor

Define minimum sample/seed count needed before a delta is actionable.

Avoid reacting to 2% random variation.

Document:

```text
sample n
confidence heuristic
minimum meaningful delta
```

Use statistical hygiene appropriate for deterministic seed ensembles.

---

# 20. 46A Phase N — Intentional Regression Proof

Inject a controlled 5% tuning regression.

Assert nightly drift gate fails.

This proves the gate can say no.

---

# 21. 46A Phase O — Rebaseline Procedure

Create explicit command/process:

```text
run approved sweep
→ inspect before/after
→ write DECISION ADR
→ update baseline
→ regenerate README
```

A rebaseline without ADR is invalid.

---

# 22. 46A Phase P — Old Artifact Disposition

For the 27 unattributed CSVs:

```text
REGENERATE
or
ARCHIVE with provenance note
```

No unattributed artifacts remain in active balance corpus.

---

# 23. 46A Phase Q — CI/Artifact Hygiene

Decide which generated artifacts are:

- committed baseline,
- ignored run output,
- release artifact.

Avoid repository bloat from every seed/run.

---

# 24. 46A Tests

- scenario schema validation,
- unknown preset fails,
- unknown policy fails,
- same seed/scenario produces same CSV,
- metadata header present,
- generated README matches artifacts,
- target schema validation,
- ADR evidence link validation,
- intentional 5% regression fails gate.

---

# 25. 46A Definition of Done

- [ ] producer reconstructed,
- [ ] in-repo sweep runner,
- [ ] scenario JSON schema,
- [ ] deterministic policy scripts,
- [ ] provenance headers,
- [ ] standard metrics panel,
- [ ] generated balance README,
- [ ] explicit TARGETS,
- [ ] DECISIONS ADR log,
- [ ] nightly drift gate,
- [ ] noise floor documented,
- [ ] regression failure proof,
- [ ] rebaseline workflow,
- [ ] 27 orphan CSVs regenerated/archived,
- [ ] sweep deterministic.

---

# 26. Workstream 46B — Local, Private Player Action Metrics

## Goal

Record enough local behavior to understand onboarding, confusion, and action→consequence flow without creating an analytics service.

---

# 27. 46B Phase A — Privacy ADR First

Create:

```text
docs/telemetry/PRIVACY.md
```

Hard rules:

```text
release default = off
dev default = on
storage = user:// only
network = none
identity = none
absolute paths = prohibited
usernames = prohibited
free text = prohibited
```

If the implementation cannot satisfy these, stop.

---

# 28. 46B Phase B — Threat/Privacy Scan

Before shipping recorder, scan for:

- HTTP clients,
- upload endpoints,
- analytics SDKs,
- account identifiers.

The telemetry module must not depend on them.

Add architecture test if feasible.

---

# 29. 46B Phase C — Event Schema

Create engine-free Core record.

Conceptual:

```text
PlayMetricEvent
{
    schema_version
    session_id
    build_id
    difficulty_preset_id
    seed
    day
    sequence
    action_id
    target_id?
    outcome_id?
    input_mode?
    elapsed_ms?
}
```

No prose fields.

---

# 30. 46B Phase D — Session ID

Use random ephemeral/session identifier.

It must not encode:

- user account,
- device ID,
- machine name.

Rotation starts new session ID.

---

# 31. 46B Phase E — `PlaySessionRecorder`

Create:

```text
Assets/Ashfall.Core/Telemetry/PlaySessionRecorder.cs
```

Responsibilities:

- accept sanitized metric events,
- enforce schema,
- bounded buffer,
- local persistence abstraction,
- no network behavior.

Keep engine-free.

---

# 32. 46B Phase F — Generalize Existing Sigils

Find:

```text
ObserveSigil(...)
```

and similar sites.

Migrate to:

```text
PlaySessionRecorder.Record(...)
```

or wrap existing sigil system as producer.

Do not maintain two parallel action-instrumentation systems.

---

# 33. 46B Phase G — Action Vocabulary

Define stable actions.

Examples from source:

```text
panel_opened
ration_policy_set
dispatch
choice_resolved
consume
save
quit
```

Extend only for useful product questions.

Do not record every hover/cursor movement.

---

# 34. 46B Phase H — Target/Outcome Vocabulary

Use IDs:

```text
route id
item id
choice id
policy id
success/failure reason id
```

No localized labels.

---

# 35. 46B Phase I — Consequence Join

Day-event stream supplies resulting state transitions.

Correlation strategy should include:

- day,
- ordered sequence,
- `causeId` where available,
- target IDs.

This allows queries like:

```text
ration policy set day 4
→ later morale/exodus events
```

without inventing causal certainty where only temporal association exists.

---

# 36. 46B Phase J — First-Hour Funnel

Define canonical funnel steps.

Source examples:

```text
guidance opened
first craft
first dispatch
first ration decision
first storm survived
tutorial day advanced
first death witnessed
```

Each step defines:

- event predicate,
- completion timestamp/day,
- prerequisite.

---

# 37. 46B Phase K — Funnel Versioning

Funnel definition changes over time.

Assign:

```text
funnel_version
```

so historical sessions remain interpretable.

---

# 38. 46B Phase L — Dead-End Detection

Define heuristics:

```text
same panel open/close repeated N times
same failed action repeated N times
same route entered/exited repeatedly
```

Store aggregate flags, not raw mouse movement.

---

# 39. 46B Phase M — Stuck Detection

Source high-value metric:

```text
days advanced with zero meaningful player actions
```

Define “meaningful” in action vocabulary.

Report consecutive count.

Do not assume all zero-action days are confusion; flag for review.

---

# 40. 46B Phase N — Input Mode Signal

Record coarse:

```text
keyboard
mouse
controller
mixed
```

per session or action family.

No hardware identifiers.

---

# 41. 46B Phase O — Local Storage

Write to:

```text
user://play_metrics/
```

or canonical user-data path.

Use bounded files.

Suggested:

```text
one session file
+ aggregate report output
```

---

# 42. 46B Phase P — Rotation/Retention

Define:

- max sessions,
- max bytes,
- max age.

On limit:

- delete oldest,
- preserve newest,
- never grow forever.

Coordinate Plan 39B retention.

---

# 43. 46B Phase Q — Opt-Out

Release default off.

When off:

```text
no metric file created
no event buffer persisted
```

Add hard test.

---

# 44. 46B Phase R — Dev Default

Development builds may default on.

Still respect explicit opt-out.

---

# 45. 46B Phase S — Deletion Route

Document and implement one user action to delete local play metrics.

No hidden cache.

---

# 46. 46B Phase T — Local Report Generator

Create CLI/headless verb.

Outputs aggregate:

```text
sessions
funnel completion %
median time-to-step
median day-to-step
top dead-end panels
repeated failed actions
stuck-day counts
input-mode distribution
```

No raw identity.

---

# 47. 46B Phase U — Synthetic Players

Run 46A scripted policies through same game paths/recorder.

This proves:

- event schema,
- funnel logic,
- aggregation,
- release report generation.

No synthetic-only recorder shortcut.

---

# 48. 46B Phase V — Redaction Validator

Test each record field.

Reject:

- slash-containing absolute paths where not canonical IDs,
- `@` emails,
- usernames,
- arbitrary prose,
- newline-rich strings,
- long unbounded values.

Prefer whitelisted enums/ID validators.

---

# 49. 46B Phase W — Schema Stability

Version record format.

Add migration/reader compatibility for prior local metric versions if reports are expected to read them.

Metrics storage is not gameplay save state.

Do not let telemetry corruption affect campaign loading.

---

# 50. 46B Phase X — Telemetry Selftest

Create:

```text
--play-metrics-selftest
```

Scenarios:

- opt-in synthetic session,
- opt-out no file,
- rotation,
- funnel,
- redaction,
- aggregation.

Expected summary required.

---

# 51. 46B Docs

Create:

```text
docs/telemetry/PLAY_METRICS.md
docs/telemetry/PRIVACY.md
```

Explain:

- schema,
- report meanings,
- privacy boundaries,
- deletion.

---

# 52. 46B Definition of Done

- [ ] privacy ADR,
- [ ] no network dependency/path,
- [ ] stable event schema,
- [ ] non-identifying session IDs,
- [ ] PlaySessionRecorder,
- [ ] sigils generalized/migrated,
- [ ] action/target/outcome vocabularies,
- [ ] day-event join,
- [ ] versioned first-hour funnel,
- [ ] dead-end detection,
- [ ] stuck detection,
- [ ] coarse input-mode signal,
- [ ] bounded local storage,
- [ ] rotation,
- [ ] release opt-out default,
- [ ] opt-out creates no file,
- [ ] delete-local-metrics route,
- [ ] local report generator,
- [ ] synthetic players use same recorder,
- [ ] redaction gate,
- [ ] selftest,
- [ ] privacy/play-metrics docs.

---

# 53. Workstream 46C — Metrics That Change the Game

## Goal

Turn metrics into an explicit design-decision loop rather than a passive dashboard.

---

# 54. 46C Phase A — Release Acceptance Thresholds

For each release candidate, define:

- balance target status,
- funnel target status,
- acceptable regressions,
- known exceptions.

Release gate consumes the generated report.

---

# 55. 46C Phase B — Release-Gate Integration

Add one required report line to Plan 39A release gate.

Example:

```text
PLAY_METRICS_REPORT_OK
BALANCE_REFERENCE_SWEEP_OK
```

Release report should link artifact hashes/summary.

---

# 56. 46C Phase C — Expected-Movement Discipline

For every tuning/onboarding change, write:

```text
expected metric to move
expected direction
expected magnitude/range
```

Examples:

- food tuning → day-30 survival,
- load shedding → power failure hours,
- season tuning → crisis timing,
- onboarding hint → funnel completion.

Then compare after sweep.

---

# 57. 46C Phase D — No Silent Adaptive Difficulty

Add explicit design rule:

```text
runtime difficulty does not adapt from hidden telemetry
```

If adaptive/director mechanics are later proposed:

- authored rule,
- player-visible contract where appropriate,
- design ADR,
- deterministic inputs.

Metrics themselves never mutate tuning.

---

# 58. 46C Phase E — Guidance Fix Surface

When funnel stall exceeds threshold:

Preferred fixes:

- guidance overlay,
- briefing attribution,
- labels,
- route discoverability,
- contextual hint.

Do not invent mechanics first.

---

# 59. 46C Phase F — Dead-End Fix Surface

Repeated panel loops may indicate:

- unclear navigation,
- missing action affordance,
- disabled state with no reason.

Route findings to UI/UX plans.

---

# 60. 46C Phase G — Reachability vs Utilization

Join:

```text
content utilization
+
synthetic player reachability
+
real local play reports when voluntarily available
```

Classify content:

```text
reachable and used
reachable but rarely used
unreachable due to discovery
unreachable due to broken wiring
intentionally rare
```

---

# 61. 46C Phase H — Evidence-Based Content Retirement

Content with 0% synthetic reachability is reviewed.

Choose:

```text
fix discovery
wire missing path
archive/remove
mark intentionally unreachable/test-only
```

No “dead because nobody saw it” without checking reachability.

---

# 62. 46C Phase I — Per-Release Balance Delta

Generate changelog-ready summary from `DECISIONS.md`.

Examples:

```text
Normal preset: median first-critical day shifted 5 → 7.
Severe scarcity: day-30 survival changed 42% → 36%.
```

Only publish numerically grounded changes.

---

# 63. 46C Phase J — Long-Tail Metrics

Define before release:

- chapter/deadline completion,
- generations reached,
- campaign completion records,
- long-term legacy records,
- late-game route usage,
- long-session save/load frequency.

Do not wait until 200-hour failures appear.

---

# 64. 46C Phase K — CI Tier Discipline

Per push:

- schema tests,
- privacy tests,
- generator tests.

Nightly:

- balance sweeps,
- drift checks.

Release:

- synthetic funnel report,
- signed-off balance/funnel review.

Keep fast CI fast.

---

# 65. 46C Phase L — Statistical Review Hygiene

In `TARGETS.md`, state:

- minimum seeds,
- meaningful delta,
- variance expectations,
- when rebaseline is allowed.

No reaction to noise below threshold.

---

# 66. 46C Phase M — Monthly Review Ritual

Create:

```text
docs/balance/REVIEW_RITUAL.md
```

Monthly:

1. regenerate/read reports,
2. choose at most 3 decisions,
3. assign owner,
4. assign plan number,
5. state expected metric movement,
6. record ADR.

This prevents measurement becoming decoration.

---

# 67. 46C Phase N — Decision Evidence Gate

Docs gate checks every new `DECISIONS.md` entry cites:

- sweep artifact,
- scenario,
- seed set,
- build SHA.

ADR without evidence fails.

---

# 68. 46C Phase O — Report Completeness Test

Meta-test ensures aggregate report includes all declared sections.

If new funnel/metric is added but report omits it, fail.

---

# 69. 46C Phase P — First Evidence-Driven Change

C2[20] should not close with only infrastructure.

Pick one measured finding and execute a small design change.

Examples:

- onboarding stall,
- over-severe ration curve,
- underused content family.

Record:

```text
before
decision
change
after
```

This proves the loop.

---

# 70. 46C Definition of Done

- [ ] release thresholds defined,
- [ ] release gate includes reports,
- [ ] tuning changes state expected movement,
- [ ] no adaptive difficulty without ADR,
- [ ] guidance changes driven by funnel evidence,
- [ ] reachability joined to content utilization,
- [ ] unreachable content dispositioned,
- [ ] per-release balance delta generated,
- [ ] long-tail metrics defined,
- [ ] CI tiering preserved,
- [ ] noise floor documented,
- [ ] monthly review ritual,
- [ ] decision evidence docs gate,
- [ ] report completeness test,
- [ ] at least one measured design change closed-loop.

---

# 71. Integrated Measurement Pipeline

```text
deterministic scenario
      │
      ▼
balance harness
      │
      ▼
reproducible artifact
      │
      ▼
TARGETS + DECISIONS
      │
      ├─────────────┐
      │             │
      ▼             ▼
player action     day-state
metrics           events
      │             │
      └──────┬──────┘
             ▼
       local aggregate
             │
             ▼
       funnel/dead-end
             │
             ▼
     design review ritual
             │
             ▼
       explicit change
             │
             ▼
      next sweep verifies
```

---

# 72. Balance Artifact Contract

Every artifact answers:

```text
who/what generated it?
which build?
which scenario?
which policy?
which seed?
which difficulty?
which runtime?
```

If not, it is not active balance evidence.

---

# 73. Scenario Contract

Every scenario is immutable/versioned enough to reproduce.

Changes to scenario assumptions should be reviewed like tuning changes.

---

# 74. Target Contract

Every balance target includes:

```text
metric
scenario
preset
acceptable band
rationale
noise floor
review date
```

---

# 75. Decision Contract

No tuning ADR without:

```text
evidence artifact
before
after/expected
owner
rollback criterion
```

---

# 76. Privacy Contract

Play metrics must contain no:

- name,
- email,
- account ID,
- device ID,
- IP,
- absolute path,
- arbitrary prose,
- network destination.

This is a hard testable contract.

---

# 77. Opt-Out Contract

Release default:

```text
disabled
```

If disabled:

```text
zero telemetry files written
```

No silent session log.

---

# 78. Local-Only Contract

Architecture/source scan should prove telemetry package has no upload/network dependency.

If any future network capability is introduced elsewhere, telemetry must remain isolated from it.

---

# 79. Metric Event Contract

Each event:

```text
session
build
preset
seed
day
sequence
action
target
outcome
input mode
```

all using stable IDs/enums.

---

# 80. Causality Contract

Joining player action and later consequence may establish:

- temporal relation,
- shared `causeId`,
- explicit domain causal link.

Reports must not imply causal certainty without a canonical cause link.

---

# 81. Retention Contract

Metric files are bounded by:

- count,
- size,
- age.

No telemetry log becomes a new 200-hour growth bug.

---

# 82. Synthetic Player Contract

Synthetic policy events go through the same:

- action APIs,
- recorder,
- consequence stream.

No direct writing of fake metric records.

---

# 83. Input Accessibility Signal Contract

Input mode is coarse only.

Do not store:

- controller model,
- USB IDs,
- keyboard layout unless separately justified.

---

# 84. Release Report Integration

Plan 39 release artifact should include:

```text
balance sweep status
synthetic funnel status
metrics schema/privacy status
latest decision IDs
```

No raw player logs in release artifact.

---

# 85. Failure Modes

## Balance CSV exists with no producer

Archive/remove from active corpus.

## Sweep lacks Git SHA

Fail artifact validation.

## Target is prose only

Add numeric range.

## Metrics accidentally write when opted out

Privacy test fails.

## Recorder stores localized text

Schema/redaction test fails.

## Telemetry module imports network client

Architecture gate fails.

## Synthetic players bypass recorder

Integration test fails.

## Funnel step changes but version does not

Schema/version test fails.

## Team reacts to 2% noise

Noise-floor contract blocks decision.

## Metrics trigger hidden difficulty adaptation

Scope violation.

## ADR has no sweep citation

Docs gate fails.

---

# 86. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| privacy regression | Medium | Critical | local-only architecture + redaction tests |
| balance baseline churn | High | Medium | ADR-controlled rebaseline |
| flaky drift gate | Medium | High | deterministic seeds + noise floor |
| too many metrics | Medium | Medium | question-driven schema |
| telemetry file growth | Medium | Medium | retention cap |
| misleading causality | Medium | High | causeId distinction |
| synthetic policy unrepresentative | Medium | Medium | multiple archetypes |
| targets chosen after seeing results | Medium | Medium | versioned TARGETS with rationale |
| measurement becomes unused dashboard | Medium | High | review ritual + one decision/release |
| release CI becomes too slow | Medium | Medium | tiering |

---

# 87. Commit Strategy

## C2[20].1 — balance artifact baseline + producer reconstruction

## C2[20].2 — scenario schema + policy scripts

## C2[20].3 — sweep runner + provenance metadata

## C2[20].4 — generated metrics panel/README

## C2[20].5 — TARGETS + DECISIONS

## C2[20].6 — nightly drift gate + regression fixture

## C2[20].7 — orphan CSV regeneration/archive

### Gate: 46A complete

## C2[20].8 — privacy ADR + telemetry architecture

## C2[20].9 — PlaySessionRecorder schema

## C2[20].10 — sigil/action instrumentation migration

## C2[20].11 — day-event consequence join

## C2[20].12 — funnel/dead-end/stuck detection

## C2[20].13 — local storage/rotation/opt-out

## C2[20].14 — report generator + synthetic players

## C2[20].15 — redaction/privacy/selftests

### Gate: 46B complete

## C2[20].16 — release thresholds + gate integration

## C2[20].17 — expected-movement/decision contract

## C2[20].18 — content reachability review

## C2[20].19 — per-release balance delta + long-tail metrics

## C2[20].20 — review ritual + evidence docs gate

## C2[20].21 — first closed-loop design change

### Gate: 46C complete

## C2[20].22 — Wave‑7 measurement-layer closure

---

# 88. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
python3 scripts/balance/run_sweep.py --scenario reference --seeds ...
godot --headless --path . -- --play-metrics-selftest
bash scripts/ci/verify-fast.sh
```

Also verify:

```text
opt-out writes no metrics file
no identity/path/prose in metric records
intentional 5% balance regression fails nightly drift
same seed/scenario reproduces balance artifact
release candidate produces synthetic funnel report
```

---

# 89. Flagship Definition of Done

## 46A — Reproducible Balance

- [ ] in-repo producer,
- [ ] scenario JSON,
- [ ] deterministic policies,
- [ ] provenance metadata,
- [ ] standard generated panel,
- [ ] explicit targets,
- [ ] decisions ADR,
- [ ] nightly drift gate,
- [ ] noise floor,
- [ ] intentional regression failure,
- [ ] rebaseline procedure,
- [ ] orphan artifacts resolved.

## 46B — Local Play Metrics

- [ ] privacy stance documented,
- [ ] no network path,
- [ ] no identity,
- [ ] stable event schema,
- [ ] recorder built,
- [ ] existing sigils unified,
- [ ] action+consequence linkage,
- [ ] funnel,
- [ ] dead-end detection,
- [ ] stuck detection,
- [ ] input-mode signal,
- [ ] bounded storage,
- [ ] release default off,
- [ ] opt-out = zero files,
- [ ] deletion route,
- [ ] local report generator,
- [ ] synthetic players,
- [ ] redaction tests,
- [ ] play-metrics selftest.

## 46C — Closed Decision Loop

- [ ] release thresholds,
- [ ] release gate report requirement,
- [ ] expected-movement discipline,
- [ ] no silent adaptive difficulty,
- [ ] guidance uses funnel evidence,
- [ ] content reachability review,
- [ ] evidence-based retirement,
- [ ] generated balance delta,
- [ ] long-tail metrics,
- [ ] statistical hygiene,
- [ ] monthly review ritual,
- [ ] ADR evidence gate,
- [ ] report completeness test,
- [ ] at least one measurement-driven change completed.

## Global

- [ ] no manual/off-repo authoritative sweep,
- [ ] no remote telemetry,
- [ ] no per-player identity,
- [ ] no remote config,
- [ ] no unattributed balance number,
- [ ] full verification green.

---

# 90. Closure Report Template

```markdown
## C2[20] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline
- Balance CSV count:
- Attributed CSV count:
- Scenario definitions:
- Action sigil sites:
- Telemetry files:
- Difficulty targets:
- Decision ADRs:

### 46A — Balance
- Sweep runner:
- Scenarios:
- Policies:
- Provenance:
- Standard metrics:
- TARGETS:
- DECISIONS:
- Drift gate:
- Noise floor:
- Regression proof:
- Orphan CSV disposition:
- Result:

### 46B — Play Metrics
- Privacy:
- Network dependencies:
- Recorder:
- Schema version:
- Action vocabulary:
- Consequence join:
- Funnel version:
- Dead-end detection:
- Stuck detection:
- Input mode:
- Rotation:
- Opt-out:
- Redaction:
- Synthetic sessions:
- Report generator:
- Result:

### 46C — Decision Loop
- Release thresholds:
- Release-gate metrics line:
- Expected movement:
- Guidance changes:
- Content reachability:
- Balance delta:
- Long-tail metrics:
- Review ritual:
- Evidence gate:
- First measured change:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Reference sweep:
- Play-metrics selftest:
- Privacy assertions:
- Intentional regression:
- Verify fast:

### Final Metrics
- REPRODUCIBLE_BALANCE_ARTIFACTS:
- UNATTRIBUTED_BALANCE_ARTIFACTS:
- SCENARIOS:
- SEEDS_REFERENCE:
- TELEMETRY_NETWORK_PATHS:
- TELEMETRY_IDENTITY_FIELDS:
- OPT_OUT_FILES_WRITTEN:
- FUNNEL_SESSIONS_SYNTHETIC:
- ADRS_WITH_EVIDENCE:
- MEASUREMENT_DRIVEN_CHANGES:

### Remaining Debt
- Balance:
- Funnel:
- Privacy:
- Content reachability:
- Difficulty:
- Release reporting:
```

---

# 91. Final Execution Directive

Execute Plan 46 as a **measurement-and-decision provenance** repair.

The critical sequence is:

```text
reconstruct every balance producer
→ encode scenarios/policies in repo
→ make outputs attributable
→ define targets
→ record decisions against evidence
→ add local/private player-action metrics
→ join actions to consequences
→ generate first-hour funnel/dead-end reports
→ run synthetic players through the same path
→ require reports at release
→ make at least one documented design change from the evidence
```

Do not add network telemetry.

Do not store player identity.

Do not react to noise below the declared floor.

Do not let a balance spreadsheet exist without a producer.

Do not let a tuning decision exist without a scenario, seed set, and build SHA.

The strongest evidence rule is:

> **Every balance number must be reproducible and attributable to a named scenario, seed set, policy, difficulty preset, and build.**

The strongest privacy rule is:

> **Play metrics remain local, bounded, anonymous, opt-in for release, and architecturally incapable of uploading anything.**

The strongest design rule is:

> **Measurement does not change the game automatically; it creates explicit, reviewed decisions whose expected effects are verified by the next sweep.**

The flagship acceptance scenario is:

> **Run the same reference scenario twice and reproduce the same balance artifact; run a synthetic first-hour player through the local recorder and generate the same funnel report; then change one documented tuning or guidance element, record the expected movement, rerun the sweep/session, and show the measured before/after in `DECISIONS.md`.**
