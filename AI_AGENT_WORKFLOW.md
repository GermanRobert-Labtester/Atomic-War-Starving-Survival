# ASHFALL AI Agent Workflow

This file governs agent roles, evidence, handoffs, and escalation. `AGENTS.md`
holds universal architecture rules; the current batch and path claims live in
`INTEGRATION_PLANS.md` and `WORKTREE_OWNERSHIP.md`.

## Read order

1. `AGENTS.md`
2. `INTEGRATION_PLANS.md`
3. `WORKTREE_OWNERSHIP.md`
4. `TEST_POLICY.md`
5. `KNOWN_DEBT.md`

Read domain documentation only after the batch identifies the authoritative
system, data, and host path.

## Roles

| Role | Typical model class | May do | Must not do |
|---|---|---|---|
| Foreman | Strong reasoning model | Verify premise, form packages, claim paths, accept handoffs | Routine implementation outside integration seams |
| Builder | Capable coding model | Implement one owned package and focused checks | Touch another claim or invent scope |
| Sweep | Cheap/fast model | Read-only search, log triage, stale API and duplicate detection | Edit code or promote unproven findings |
| Reviewer | Different cheap/mid model | Check a diff against its contract | Race the builder with another rewrite |
| Integrator | Strong coding/reasoning model | Join accepted packages and own shared seams | Add unrelated features |

Examples of sweep-capable models include Stepfun Flash, GLM Flash, Luna, and
their future equivalents. Choose by capability and cost, not by a hard-coded
vendor name.

## Package protocol

Before editing, a builder must have a package containing:

- outcome and explicit non-goals;
- premise evidence from current source/data;
- exact owned paths and any integrator-only shared paths;
- existing authority, save owner, and event/host seam when applicable;
- 3-6 observable acceptance criteria;
- exact focused verification commands.

If current evidence disproves the premise, stop and return `STALE_PLAN` or
`STALE_TEST` to the foreman. Do not adapt a deprecated API merely to keep a
plan moving.

## Handoff format

```text
Package:
Outcome:
Files changed:
Current contract used:
Verification commands and results:
Tests reused / added / aggregated:
Known limitation or debt:
Shared files intentionally untouched:
Ready for sweep: yes/no
```

Reasoning transcripts are not handoff artifacts. The diff, current contract,
commands, results, and remaining limitation are.

## Sweep protocol

Sweep agents are read-only. Every finding must include:

```text
finding_id | severity | confidence | path:line | current evidence | expected contract | proposed owner
```

Permitted checks: build errors, stale symbol use, unbound host/event/save
paths, duplicate authority, invalid catalog references, missing lifecycle
cleanup, accidental Unity use, and focused-test failures. A finding without
path-level evidence is discarded rather than implemented.

## Escalation

Stop and return the package when it needs a new authority, overlaps a live
claim, changes multiple composition roots unexpectedly, cannot reproduce its
failure, restores retired architecture, or exceeds its stated outcome.

Only the foreman edits the integration ledger and ownership ledger. Only the
integrator edits a shared path during integration.

## Subagent Role Isolation & Context Scoping

Instead of one agent attempting to execute everything in one huge context, split
responsibilities across isolated roles with strictly scoped context:

| Subagent Role | Input Context | Responsibility | Output Artifact |
|---|---|---|---|
| **Planner** | High-level brief + catalog schemas | Read brief, inspect authority, draft bounded plan | `.ai/plan.md` |
| **Coder** | `.ai/plan.md` + target files only | Implement smallest coherent change | Code diffs + `.ai/state.md` update |
| **Tester** | `.ai/state.md` + changed test files | Run targeted tests only (<30s fast) | Test report in `.ai/state.md` |
| **Auditor** | Schemas + world bible + diff | Verify data schema validity, integrity, conventions | Findings log / clearance |

Each subagent receives only the context it needs (plan + relevant files + last summary),
not the full conversational history. This prevents cross-contamination.

## Operational Discipline & Limits

- **No Repeated Tool Calls:** If identical tool + arguments appear >2 times, stop immediately and ask for guidance. Never loop blindly.
- **Hard Limits:** Max 60–100 steps and 10–20 minutes per task. On limit reached: stop, record attempted progress in `.ai/state.md`, and report failure.
- **Explicit Termination Criteria:** Define clear criteria for completion (e.g. tests pass, no new compile errors, validator clean) before beginning edits.
- **Progress Tracking (`.ai/state.md`):** Keep a state file tracking changed files, tests run, and remaining errors. Read this file before acting.
- **Conflict Resolution Rule:** If narrative and systems conflict, **Systems win** unless explicitly overridden by the foreman/user. Log conflicts in `.ai/state.md`.
- **Pre-generation Checks:**
  1. Always run Go config/save validators (`bin/validate-config` / `bin/ashfall-dev validate-config`) before generating data.
  2. Search for existing equivalent systems, items, or quests before creating new ones. Extend existing authority instead of adding duplicates.
- **Plan Approval Requirement:** Any commit modifying code files must have an approved plan in `.ai/plans/` (or `.ai/plan.md`) with `STATUS: APPROVED BY USER`. Pre-commit and CI will reject commits missing this.
- **Testing Scoping & Ceilings:**
  - Always run tests via `bin/run-scoped-tests`. Do not invoke `pytest` or `dotnet test` directly.
  - Full test suite runs are banned unless the user explicitly enters `RUN FULL TESTS`.
  - Maximum 10–15 testing steps per task: if failing tests cannot be fixed within 10–15 steps, auto-flag the issue in `.ai/state.md` for a bug validator to fix instead of repeatedly editing and re-running.
