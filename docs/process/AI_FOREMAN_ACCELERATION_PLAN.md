# ASHFALL AI Foreman Acceleration Plan

**Status:** Installed 2026-09-12; no production-code changes were made by this bootstrap  
**Scope:** Agent instructions, plan intake, work ownership, testing economics, model routing, integration, and debt handling  
**Authority after rollout:** `AGENTS.md` plus the five operational files defined below  
**Exit condition:** This bootstrap plan is archived after the operating cycle completes two successful batches

## 1. Outcome

Change ASHFALL development from open-ended plan generation and competing agent edits into a
foreman-controlled production line:

```text
human priority
    -> foreman verifies premise and forms one batch
        -> 2-3 builders receive disjoint work packages
            -> cheap sweeper inspects the combined result
                -> integrator fixes confirmed integration defects
                    -> focused verification and acceptance
                        -> debt/register update and next batch
```

The foreman speeds work up by reducing repeated discovery, duplicated prompts, overlapping edits,
speculative tests, and broad verification. It does not achieve speed by accepting unverified code.

## 2. Evidence for the change

The current workspace has process machinery, but too many competing authority surfaces:

- `AGENTS.md` is 1,066 lines and about 95 KB.
- Nine large model-specific root rulebooks duplicate most of that body; the inspected set totals
  9,602 lines.
- `Next-steps-plans/` currently contains 119 direct plan files and another 165 direct files under
  `shipped_to_chat/`.
- At least 247 plan-named Markdown files are discoverable through the normal repository file scan.
- `docs/CURRENT_AUTHORITY.md` points to a missing `docs/CURRENT_INTEGRATION_BATCH.md`.
- `AGENTS.md` currently tells agents to tolerate a faster agent changing the same files, even if
  those changes temporarily break the area. That directly conflicts with safe ownership.
- Plans 29 and 59 already diagnose rulebook drift, plan sprawl, stale claims, and repeated audits.
  This plan implements the missing operating mechanism instead of starting another numbered wave.

## 3. Governing decisions

1. `AGENTS.md` remains the only universal instruction authority.
2. The five operational files below hold changing workflow state and detailed policy. They do not
   each repeat all project architecture rules.
3. Only the foreman edits the active plan registry and ownership ledger.
4. No more than three implementation work packages may be active concurrently.
5. A batch may reference 9-20 existing plans, but it must compress them into coherent dependency
   slices. Plan count is never used as a productivity metric.
6. Builders own disjoint paths. Shared authority files are reserved for the integrator.
7. Cheap models perform read-only sweeps after builders finish. Findings require evidence before
   they become fixes.
8. Tests are risk controls, not generated deliverables. Existing coverage is reused first.
9. Godot and `Assets/Ashfall.Core` remain authoritative. No process rule may weaken the engine,
   data, save, determinism, or ownership invariants.
10. Existing instruction text is preserved through version control and a dated archive snapshot
    before active rulebooks are compacted or corrected.

## 4. Final authority structure

```text
AGENTS.md                         universal invariants and short workflow router
AI_AGENT_WORKFLOW.md              roles, model routing, handoff and evidence protocol
INTEGRATION_PLANS.md              only active batch and ordered dependency ledger
TEST_POLICY.md                    test creation, selection, aggregation and escalation
WORKTREE_OWNERSHIP.md             current path claims; foreman is sole writer
KNOWN_DEBT.md                     accepted, blocked, quarantined and retired work
docs/archive/agent-rules/         byte-preserved historical instruction snapshots
docs/archive/plans/               executed/superseded plan history
```

`docs/CURRENT_AUTHORITY.md` links to these files but does not duplicate their rules.

## 5. Roles and model routing

Model names are examples, not permanent policy. Routing is capability- and cost-based so models
can be replaced without rewriting the workflow.

| Role | Preferred class | Responsibilities | Forbidden work |
|---|---|---|---|
| Foreman | Strong reasoning model | Verify premises, form batches, assign paths, resolve dependencies, accept handoffs | Routine code generation, speculative feature expansion |
| Builder | Capable coding model | Implement one bounded package against current APIs and run focused verification | Editing another owner's paths, creating unrelated tests |
| Sweep / triage | Stepfun Flash, GLM Flash, Luna, or equivalent cheap model | Read-only scans, logs, failure clustering, stale API detection, duplicate detection | Broad rewrites, architecture decisions, silent fixes |
| Reviewer | Cheap or mid-tier model distinct from builder | Review diff against contract and report evidence-ranked findings | Competing implementation or preference-only rewrites |
| Integrator | Strongest available coding/reasoning model | Combine accepted packages, repair confirmed seams, run affected-region checks | Adding features outside the batch |

### Routing rules

- Use a cheap sweeper when the task is primarily search, inventory, classification, log analysis,
  stale-reference detection, or reproduction narrowing.
- Use a builder only after the foreman identifies a current contract and exact owned paths.
- Use the integrator for shared composition roots, save registries, cross-system event routing,
  schema changes, and conflicts between otherwise accepted packages.
- Do not assign multiple agents to race on the same bug. If a builder stalls, the foreman revokes
  and reassigns the claim with a written handoff.
- A different model may review a package, but it may not open a competing fix branch unless the
  foreman rejects the original package.

## 6. Standard operating cycle

### Stage A — Intake

The foreman receives candidate plans or defects and performs a premise check before assigning code.
Each candidate receives one classification:

- `REAL_DEFECT`: current behavior contradicts a current contract.
- `MISSING_INTEGRATION`: implementation exists but has no authoritative runtime path.
- `MISSING_CONTENT`: code exists; authoritative data is absent or thin.
- `STALE_PLAN`: premise no longer matches source.
- `STALE_TEST`: test targets a removed API or retired behavior.
- `DUPLICATE`: another active or completed package owns the outcome.
- `DEBT_ACCEPTED`: intentionally deferred with an owner and promotion condition.

Only the first three classes normally become implementation packages.

### Stage B — Batch formation

The foreman groups 9-20 referenced plans into no more than three active work packages. A package may
combine plans only when they share the same authority, dependency chain, and verification surface.

Good package boundaries:

- one Core authority plus its Godot adapter and save section;
- one catalog family plus its existing consumer and presentation path;
- one UI workflow plus the already-existing commands it exposes.

Bad package boundaries:

- unrelated systems grouped because their plans have nearby numbers;
- one package touching several composition roots;
- a test-only package for behavior that has no current implementation contract;
- independent agents editing `Main.*`, central registries, or the same JSON catalog.

### Stage C — Claim

Before editing, the foreman writes a claim to `WORKTREE_OWNERSHIP.md`. A valid claim contains:

```text
claim_id | package_id | owner | role | baseline | exact paths | shared paths | lease | status
```

Builders may read but not edit the ledger. A claim expires only when the foreman records `HANDED_OFF`,
`ACCEPTED`, `REVOKED`, or `BLOCKED`.

### Stage D — Build

Each builder receives a context packet no longer than 200 lines containing:

1. outcome and non-goals;
2. current API evidence;
3. exact owned files;
4. authoritative data and save owners;
5. acceptance criteria;
6. exact focused verification commands;
7. known adjacent changes that must be preserved.

The builder does not reread the entire plan archive or invent another plan. If evidence disproves the
packet, it stops and returns a premise correction.

### Stage E — Handoff

Every builder returns the same compact record:

```text
Package:
Outcome:
Files changed:
Production contract used:
Tests reused / added / changed:
Commands run and exact results:
Known limitations:
Shared files intentionally not changed:
Ready for sweep: yes/no
```

Reasoning transcripts are not required. The reusable evidence is the diff, contract, commands, and
remaining limitation.

### Stage F — Cheap sweep

After all builders in the slice stop editing, one cheap sweeper inspects the combined state. It is
read-only and reports findings using:

```text
finding_id | severity | confidence | path:line | reproduction/evidence | owner | proposed next action
```

The sweeper checks only these classes unless the foreman expands scope:

- compile errors and stale symbol references;
- unbound events, commands, registrations, save sections, and content consumers;
- duplicate authorities or overlapping implementations;
- missing cleanup/subscription disposal;
- catalog IDs that fail current validation or lack consumers;
- focused test failures and obvious nondeterministic APIs;
- accidental Unity reintroduction.

No finding without a path, current evidence, and an expected contract enters implementation.

### Stage G — Integration

The foreman ranks sweep findings. The integrator repairs only accepted findings and owns all shared
composition files for this stage. Rejected findings are recorded as false positives or debt so the
next sweeper does not rediscover them.

### Stage H — Verification and closeout

The integrator runs the smallest command set covering the combined risk. A package is accepted only
when its observable acceptance criteria pass. Compile success alone is not integration evidence.

The foreman then:

1. marks packages accepted or blocked;
2. releases ownership claims;
3. updates `KNOWN_DEBT.md`;
4. archives or marks consumed plans;
5. records metrics;
6. forms the next batch only after the current batch closes.

## 7. Test economics

### A test may be created only when

- it reproduces a confirmed defect that existing tests do not catch;
- it protects a new public contract or integration seam;
- it protects save/load, determinism, lifecycle, state transition, inventory mutation, or a
  cross-system consequence;
- an explicit acceptance criterion cannot be verified by an existing test or headless check.

### A test must not be created when

- a plan merely lists a feature or catalog row;
- the production API or consumer does not exist;
- an existing test can accept another case without losing diagnostic clarity;
- it restores a deprecated API, Unity seam, bridge, or stale assumption;
- its only purpose is raising test count or making a report look complete.

### Execution levels

| Stage | Default verification |
|---|---|
| Premise / sweep | Static inspection; zero tests unless reproducing one suspected defect |
| Builder | Modified test file and directly affected regional tests; normally below 100 cases |
| Integrator | Union of affected regional targets, deduplicated |
| Nightly / release | Broad suites and long-running checks in a dedicated window only |

No unchanged broad command is rerun without a new hypothesis or a changed integration state. Test
output is summarized; raw logs are stored once and referenced by later agents.

Pure mapping tables may be aggregated with row-level failure messages. Save/load, determinism,
lifecycle, state-transition, mutation, fuzz, and cross-system cases remain independently reported.

## 8. Debt and quarantine protocol

Every deferred item has one status in `KNOWN_DEBT.md`:

| Status | Meaning |
|---|---|
| `ACCEPTED` | Known imperfection that does not currently justify work |
| `BLOCKED` | Valid need lacking an API, content authority, decision, or dependency |
| `QUARANTINED` | Preserved outside active compilation/runtime with manifest evidence |
| `RETIRED` | Historical behavior or architecture that must not be restored |
| `PROMOTED` | Approved for a named active package |

Required fields are ID, status, subsystem, evidence, reason, owner role, promotion condition, and
last-reviewed date. Debt without a promotion condition must not repeatedly appear in sweeps.

## 9. Ready-made operational files

### `AI_AGENT_WORKFLOW.md`

Keep this under 150 lines. It contains the role table, routing rules, context packet, handoff format,
cheap-sweep schema, and escalation rules from Sections 5-6. It contains no gameplay architecture
inventory.

### `INTEGRATION_PLANS.md`

This is not another prose backlog. It contains:

```text
Current batch: BATCH-YYYY-MM-DD-N
Baseline:
Goal:
Integration order:

package_id | source plans | premise | dependencies | owner | state | acceptance | verification
```

Only one current batch is allowed. Backlog plans remain links, not copied prose. Completed batches
move to an archive table containing only outcome and evidence.

### `TEST_POLICY.md`

Keep this under 120 lines. It contains Section 7, the current 180-second targeted limit, explicit
maximums, quarantine/re-enable evidence, and commands for the supported runners. It supersedes
contradictory test-count allowances elsewhere after those passages are archived.

### `WORKTREE_OWNERSHIP.md`

Keep this mostly tabular and under 100 lines. Only the foreman writes it. Claims use exact paths;
wildcards require an explanation. Shared files are owned by the integrator and are never edited by
builders concurrently.

### `KNOWN_DEBT.md`

Keep the active table under 200 rows. Long forensic explanations and retired source manifests live
in `docs/archive/` or `Twin_ASHFall`, with links from the table.

## 10. Exact `AGENTS.md` integration

Append a short section near the non-negotiable rules; do not paste all five documents into it:

```md
## AI FOREMAN AND COORDINATION — REQUIRED

`AGENTS.md` defines universal architecture. Operational authority is:
`AI_AGENT_WORKFLOW.md`, `INTEGRATION_PLANS.md`, `TEST_POLICY.md`,
`WORKTREE_OWNERSHIP.md`, and `KNOWN_DEBT.md`.

- Read the current batch and ownership ledger before editing.
- Do not race another agent or edit claimed paths.
- One foreman assigns work; builders implement disjoint packages; a separate cheap
  sweeper reports evidence; one integrator owns shared seams and acceptance.
- Do not create speculative tests or restore deprecated APIs to satisfy tests.
- Run only the focused verification authorized by the package and test policy.
- A plan is not authority. Verify its premise against current source before work.
- Compile-green is not proof of runtime integration.
- Quarantine and re-enable decisions require current API evidence and targeted results.
```

The existing “faster agent may edit the same files” instruction must become inactive. To preserve
history without leaving a live contradiction:

1. save a byte-identical dated rulebook snapshot under `docs/archive/agent-rules/`;
2. replace the active sentence with the ownership rule;
3. record the supersession in the archive index and Git history;
4. regenerate derived rulebooks through the existing sync script.

## 11. Rollout work packages

### FOREMAN-00 — Snapshot and freeze

**Changes:** Documentation/archive only.  
**Actions:** Snapshot all root rulebooks; record hashes and current line counts; pause new numbered
plan generation during rollout; record existing dirty paths without changing them.  
**Acceptance:** Every existing instruction can be recovered byte-for-byte; no gameplay file changes.

### FOREMAN-01 — Create the five operational authorities

**Changes:** Add the five root Markdown files using Section 9.  
**Actions:** Seed only one pilot batch, active ownership, current test limits, and currently accepted
debt. Do not backfill hundreds of plans in prose.  
**Acceptance:** Each concern has one owner document; links resolve; no repeated architecture essay.

### FOREMAN-02 — Install the AGENTS router and remove contradictions from active authority

**Changes:** `AGENTS.md`, generated client rulebooks, sync report.  
**Actions:** Add Section 10; correct same-file racing, stale report links, full-suite contradictions,
and any instructions disproved by current source. Preserve original text in the snapshot.  
**Acceptance:** An agent cannot reasonably infer that racing, speculative tests, or Unity restoration
is allowed; rulebook sync check passes.

### FOREMAN-03 — Establish one current batch

**Changes:** `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`.  
**Actions:** Select three small, low-coupling packages from existing plans; verify each premise;
assign disjoint paths and shared-file ownership.  
**Acceptance:** Zero overlapping claims; every package has measurable acceptance and exact commands.

### FOREMAN-04 — Enforce test economics

**Changes:** `TEST_POLICY.md` and existing runner/gate documentation; scripts only where enforcement
is missing.  
**Actions:** Make focused selection, zero-match failure, exclusion detection, timeout, and aggregate
reporting canonical. Separate builder checks from nightly/release checks.  
**Acceptance:** No implementation package requires a full suite; stale/quarantined targets cannot
produce false green results.

### FOREMAN-05 — Pilot cheap sweep and evidence triage

**Changes:** Workflow records only unless a confirmed defect is promoted.  
**Actions:** Run Stepfun Flash, GLM Flash, Luna, or equivalent against the completed pilot diff using
the fixed finding schema. Foreman rejects unsupported findings before assigning fixes.  
**Acceptance:** Every promoted finding has path-level evidence, a current contract, and one owner;
the sweeper edits no code.

### FOREMAN-06 — Integrate the pilot

**Changes:** Shared files only by the integrator; debt/register updates by foreman.  
**Actions:** Repair accepted seams, run deduplicated regional checks, close claims, record false
positives and debt.  
**Acceptance:** Three packages close without overlapping edits or competing fixes.

### FOREMAN-07 — Scale to a 9-20-plan batch

**Changes:** One new batch ledger.  
**Actions:** Compress related plans into at most three concurrent packages and repeat the cycle.
Prefer integration of existing systems/content over new system creation.  
**Acceptance:** Batch closes with no more than three simultaneous owners and one integration pass.

### FOREMAN-08 — Measure and simplify

**Changes:** Process metrics and authority links.  
**Actions:** Compare pilot and scaled batch against baseline; archive consumed plans; remove stale
links from current authority; consider compact client wrappers only after confirming each client
loads canonical instructions correctly.  
**Acceptance:** Context and duplicate work decrease without higher reopen or regression rates.

### FOREMAN-09 — Close the bootstrap

**Changes:** Archive this plan and publish a one-page operating record.  
**Actions:** Mark this plan executed, link the live five-file system, resume plan intake only through
the foreman workflow.  
**Acceptance:** No FOREMAN-10 plan is created. The operating cycle replaces the planning series.

## 12. Metrics

Capture the baseline before rollout and compare after two batches:

| Metric | Target |
|---|---|
| Concurrent implementation owners | 2-3, never more than 3 |
| Overlapping owned paths | 0 |
| Competing fixes for one finding | 0 |
| Packages with verified premises | 100% |
| Packages with exact acceptance and commands | 100% |
| Cheap-sweeper findings promoted without evidence | 0 |
| Broad suite runs during builder stage | 0 |
| Repeated unchanged verification commands | 0 |
| Builder context packet | 200 lines or fewer |
| New tests without a stated uncovered risk | 0 |
| Active batch registries | exactly 1 |
| New plan documents created outside intake | 0 |

Track practical cost rather than pretending token totals are exact: number of strong-model turns,
files reread, duplicate scans, repeated test runs, rejected speculative findings, reopened defects,
and elapsed time from claim to acceptance.

## 13. Stop and escalation rules

Stop a package and return it to the foreman when:

- current source disproves the plan premise;
- required paths overlap another active claim;
- the change requires a new authority rather than extending the existing one;
- acceptance requires restoring Unity or a deprecated bridge;
- more than one shared composition root must change unexpectedly;
- the focused failure cannot be reproduced;
- the package grows beyond its explicit outcome;
- user or architecture input is needed.

Being blocked is cheaper than confidently implementing the wrong plan.

## 14. Definition of success

The process is successful when two or three coding agents can complete a coherent batch, a cheap
agent can find integration defects without editing, and one integrator can close the batch from a
small evidence packet—without rereading hundreds of plans, racing another agent, generating a wall
of tests, or reopening deprecated architecture.
