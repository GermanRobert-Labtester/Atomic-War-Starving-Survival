# ASHFALL PROJECT — CRUSH Instructions
# AUTO-GENERATED from AGENTS.md (canonical source). Run sync-agent-rulebooks.py to regenerate.
# Last generated: 2026-09-25

---

## READ THIS FIRST — NON-NEGOTIABLE RULES

1. **Godot is authoritative; Unity is retired.** Never invoke Unity or add a
   Unity dependency unless the user explicitly requests historical Unity work
   in this message. Do not restore Unity architecture to satisfy a test.
2. **Core stays engine-free.** `Assets/Ashfall.Core/` may not reference
   `Godot`, `UnityEngine`, or engine serialization APIs. Engine-neutral logic
   belongs in Core; Godot presentation and adapters belong in `src/`.
3. **JSON data is authoritative.** Use `Assets/StreamingAssets/Data/` for
   authored game data. Do not duplicate mutable gameplay authority in panels,
   hosts, caches, or parallel systems.
4. **Preserve deterministic and persistent behavior.** Stateful Core changes
   require the existing save ownership, deterministic RNG, and restore path.
   Never use `System.Random` in deterministic Core behavior.
5. **One authority per concern.** Extend the current owner and route through
   its existing host/event/save seam. Do not create a parallel resource,
   ledger, save store, registry, simulation, or modality manager.
6. **Do not race agents.** Read `INTEGRATION_PLANS.md` and
   `WORKTREE_OWNERSHIP.md` before editing. Claimed paths are read-only to
   everyone except their owner; shared paths belong to the integrator.
7. **Use current evidence.** A plan, audit, or test name is not proof that an
   API, catalog, route, or bug still exists. Verify the premise in source and
   data before changing code.
8. **Use focused verification.** Follow `TEST_POLICY.md`; do not run the full
   test suite or broad commands by default. `scripts/run_test.sh` caps focused
   xUnit runs at 180 seconds and rejects excluded targets.
9. **Never leak secrets.** Do not request, print, store, commit, or place API
   keys, tokens, credentials, or private configuration in prompts, source,
   logs, JSON, or instructions.
10. **Stop when authority is missing.** If a task needs a new architecture
    decision, overlaps a claim, restores retired behavior, or contradicts
    current evidence, report the blocker to the foreman/user instead of
    improvising a workaround.

## AI FOREMAN AND COORDINATION — REQUIRED

Read in this order before implementation:

1. `INTEGRATION_PLANS.md` — current batch, order, and acceptance.
2. `WORKTREE_OWNERSHIP.md` — exact file claims.
3. `TEST_POLICY.md` — test selection and quarantine rules.
4. `KNOWN_DEBT.md` — accepted, blocked, quarantined, and retired work.
5. `AI_AGENT_WORKFLOW.md` — role, evidence, handoff, and sweep protocol.

One foreman assigns packages; builders implement disjoint paths; cheap sweep
agents remain read-only; one integrator owns shared seams and acceptance. Do
not create speculative tests, revive deprecated APIs, or start a competing
fix. A compile-green result is not proof of runtime integration.

## TARGETED TESTING ONLY — DO NOT RUN EVERY TEST EVERY EDIT

Run only tests directly related to files changed during the current task. Run the full suite only after a completed feature, before a commit, or in CI. Never create duplicate tests that assert the same behavior through slightly different wording. If an agent creates thousands of tests and executes all of them after each small edit, that is inefficient regardless of language.

### Test Execution Hierarchy

- **Fast (< 30 seconds):** Changed-module tests only; run on each edit.
- **Medium (< 5 minutes):** Subsystem tests before feature commit.
- **Full (10–60+ min):** All tests before merge / release / nightly CI.
- **Stress (long-running):** Fuzzing, simulations, soak tests overnight.

### 10 Testing Policy Rules

1. Prefer 3–10 high-signal tests per behavior over dozens of near-duplicate tests.
2. Reuse parameterized tests instead of generating one test file per input.
3. Do not test third-party library internals, trivial getters/setters, or framework behavior.
4. Mark tests as `fast`, `integration`, `slow`, or `e2e` (`pyproject.toml` markers / xUnit traits).
5. Do not run the full test suite after every edit.
6. Run only tests affected by changed files, then report the exact command and result.
7. Use deterministic seeds; never use arbitrary `sleep()` calls.
8. Mock network, disk-heavy assets, time, and external processes in fast tests.
9. Check for an existing equivalent test before adding a new test.
10. Delete or merge redundant generated tests when found.

### Explicit Ban on Full Test Suite

- **NEVER run the full test suite unless the user types exactly: `RUN FULL TESTS`.**
- If you feel the need to run the full suite, instead:
  1. List which additional tests you think are relevant.
  2. Ask: "Should I run these extra tests now, or only the scoped ones?"
- **Use `bin/run-scoped-tests` for all test runs by default.**
- **Do not invoke `pytest`, `dotnet test`, etc. directly unless explicitly told.**
- **Testing Step Limit & Auto-Flagging:** Maximum 10–15 testing steps per task. If the AI model cannot resolve failing tests within 10–15 steps, it must auto-flag the issue in `.ai/state.md` for a bug validator to fix rather than repeatedly continuing to hit tests with endless micro-edits.

### Execution Rules

- Run only targeted tests via `bin/run-scoped-tests` (or the smallest test file; < 30 seconds).
- Prefer static inspection for sweeps. A larger diagnostic run requires a new
  hypothesis and explicit foreman/user reason; it must remain bounded and not
  compete with active builders.
- Aggregate only homogeneous static mappings, labels, thresholds, and catalog
  tables with useful per-row failure output. Preserve independent save/load,
  determinism, lifecycle, mutation, fuzzing, state-transition, and
  cross-system tests.
- Quarantine/re-enable decisions require current API/content evidence, a
  written reason, and a passing focused target. Never silently re-enable.
- If a Godot runtime session is needed, use 15 FPS unless the user explicitly
  requests another target. Do not invoke Unity to test Godot behavior.

## ACTIVE QUEUE — UNBLOCKED PLANS (audited 2026-09-19)

**8 plans are available for integration now.** Full audit with per-plan
evidence and verification results: `docs/plans/UNBLOCKED_PLANS_AUDIT_2026-09-19.md`.
Queue authority stays `INTEGRATION_PLANS.md` (current batch: XP Expansion W1,
package `XP-WAVE1-DIFFICULTY-AUTHORITY`); claim exact paths in
`WORKTREE_OWNERSHIP.md` before editing.

Available, unexecuted, no new foreman signature needed:

1. `CF-P1-DISTRESS-CONTENT-SEAL` — distress follow-up/audio content seal
   (verify-and-seal; validator rules, population replay, PR3 closeout).
2. `CF-P5-RESTOCK-RECONCILE` — merchant-restock ledger reconciliation only
   (DEC-05 already signed; implementation live; ledger paths integrator-owned).
3. `CF-P6-VEHICLE-ARMOR-GRADES` — 4 armor grade tiers on the Plan 50 vehicle
   seam (new claim + premise note).
4. `CF-P28-ONE-BOOTSTRAP-PATH` — run the manifest bootstrap on the fresh-game
   path (bounded host change).
5. `CF-XP01-DIFFICULTY-FULL-BINDING` — preset selection, persistence,
   remaining scalar consumers, panel (per-consumer premise checks under the
   active W1 claim).
6. `E1/Plan 53` — census `READY-UNCLAIMED` governance programme.
7. `C2[15]/Plan 37` — input/focus/controller parity (prerequisites sealed;
   run the premise audit first).
8. `C2[21]/Plan 48` — release craft: versions, tags, hotfix path
   (prerequisite sealed; run the premise audit first).

Completed since 2026-09-18 (do not redo): 10 debt seals, 6 of the 15
completion-first roster plans (Plan 24 closure, Plan 30 war projection +
clock, Plan 32 graph travel, Plan 34 chronicle, Plan 36C port sweep,
Plan 26A tranche-2), census anchors C2[9]–C2[13], Plan 24 residual (L-P24R
snapshot rebaseline protocol), and Plan 31 residual (D11-B closed-section
routing). Still **decision-blocked** — never start without the named signature:
quarantine drain (D21), XP-04 economy legs (F13), XP-06 body-integrity schema
(F14), EN-01…EN-08 proposals, Plan 49 (needs Plan 42/46 audits), C3 HOLDs
174/175/192/199, string freeze (D22).

## FOLLOW-UP PARTIAL WAVE — REMAINING PLACEHOLDERS (2026-09-19)

Plans 185 (Memory Decay), 162 (Shelter Archive), 216 (Exercise), 202
(Interpersonal Conflict), 163 (Cartography), and 210 (Personal Belongings)
now have bounded implementation logs. The remaining ranked partials are
intentionally deferred and tracked as placeholders in
`docs/plans/PARTIAL_REMAINING_PLACEHOLDER_2026-09-19.md`.

That file is a pointer, not an approval or ownership claim. Before promoting a
row, re-audit the current authority, claim exact paths, and keep the work
small: one truthful production seam, focused tests, and no parallel mutable
state or save section.

Wave 5 details: `docs/plans/PARTIAL_2_WAVE5_FULL_INTEGRATION_IMPLEMENTATION_LOG.md`.

## ARCHITECTURE AND DATA

- Core target: `Assets/Ashfall.Core/` (`netstandard2.1`); pure domain logic.
- Godot host target: `src/` (`net8.0`); thin nodes, panels, routing, adapters,
  and host CLI.
- Test target: `Ashfall.Core.Tests/` (`net9.0`); test Core contracts through
  current public APIs.
- Data target: `Assets/StreamingAssets/Data/`; schema-valid snake_case JSON.
- Assets: use Godot-native `assets/` imports and the existing asset registry;
  never extend deprecated `Assets/_Game/` structures.
- Core events expose facts. Host adapters apply presentation and persistence
  effects. Do not put gameplay decisions in panels or generic UI callbacks.
- Inventory, needs, health, power, water, radiation, relationships, and
  campaign state already have owners. Find and extend them rather than adding
  a local counter or cache.

## SAVE, DETERMINISM, AND CONTENT

- Register state through the current save-section owner. Implement the
  matching capture/restore path before claiming persistence.
- Use the existing seeded RNG contract for replayable behavior; never seed
  from wall-clock time or hash iteration order.
- Validate catalog IDs, references, ranges, and consumers through the current
  integrity pipeline. Presence in JSON is not gameplay reachability.
- A system is integrated only when its Core authority, host owner, route or
  event path, persistence where needed, and observable outcome agree.

## UI, TONE, AND ACCESSIBILITY

- A panel exposes an existing command and truthful current state; it does not
  become a new gameplay authority or a fake operational route.
- Preserve keyboard/controller close/back behavior, focus, visible feedback,
  readable contrast, and refresh/disposal lifecycle when touching UI.
- Keep tone restrained, human, and fictional. Do not use real countries,
  wars, people, copied art, copied text, or copied UI layouts.

## WORKFLOW

1. Restate the bounded outcome, non-goals, and current evidence.
2. Confirm package ownership and list exact files.
3. Inspect the existing owner, public API, data schema, save path, and host
   adapter before editing.
4. Make the smallest coherent change. Preserve unrelated dirty worktree
   changes and never mass-format a shared area.
5. Run the package's focused verification; include a Godot headless check only
   when the change affects that runtime path.
6. Hand off outcome, files, contract, commands/results, limitations, and
   shared paths intentionally untouched using `AI_AGENT_WORKFLOW.md`.
7. Update the live ledger/debt only if you are the foreman or named integrator.

## SOURCE OF TRUTH

| Need | Authority |
|---|---|
| Current package and acceptance | `INTEGRATION_PLANS.md` |
| Path ownership | `WORKTREE_OWNERSHIP.md` |
| Test selection and test debt | `TEST_POLICY.md` |
| Deferred or retired work | `KNOWN_DEBT.md` |
| Agent roles and handoffs | `AI_AGENT_WORKFLOW.md` |
| Domain documentation map | `docs/CURRENT_AUTHORITY.md` |
| Historical rules | `docs/archive/agent-rules/2026-09-12-pre-foreman/` |
| Quarantined test manifest | `Twin_ASHFall/quarantine/manifests/2026-09-12-quarantined-tests.md` |

## TOOLS AND CHANGE HYGIENE

- **Strict Go-Only Tool Creation Policy (8 Core Types):** Of the following 8 tool types, **ONLY `.go` implementations (integrated into `tools/gotools` / `bin/ashfall-dev`) can be created or modified by AI agents**. AI agents must NEVER create Python or shell scripts for these 8 concerns:
  1. Repository file indexer
  2. Changed-file / changed-test selector
  3. Test-result parser
  4. Fast JSON/YAML validator
  5. Save-file scanner
  6. Asset manifest builder
  7. Parallel subprocess/task runner
  8. LLM API proxy/router
- **Tooling Language Policy:** For persistent or repeatedly invoked development tools, prefer Go (`bin/ashfall-dev`). Use Python only when a required Python-only AI, data, or testing library provides a clear benefit. Use Rust for CPU-heavy, memory-sensitive simulation, validation, parsing, or fuzzing.
- **Process Spawning Rule:** Do not launch a Python process for trivial filesystem, JSON, process-management, or test-selection tasks when an existing Go/Rust utility can do the job.
- **Performance Verification:** Measure peak RSS and total elapsed time before rewriting a working tool.
- Prefer repository scripts and current APIs over ad-hoc replacement tooling.
- Do not modify generated outputs by hand; run the owning generator and its
  `--check` mode when one exists.
- Do not alter unrelated user changes, delete data, rewrite history, or reset
  the worktree.
- Do not invent MCP connections, external accounts, capabilities, or tool
  results. Use only configured tools and their documented scope.
- Keep changes reviewable: one owned system or governance package at a time.

## AGENT OPERATIONAL DISCIPLINE & EXECUTION LIMITS

1. **No repeated tool calls with identical inputs:** If the same tool + arguments appear more than twice in one task, the agent must immediately stop and request guidance. Never loop blindly on identical operations.
2. **Hard iteration and time limits:**
   - Maximum 60–100 steps per task.
   - Maximum 10–20 minutes wall-clock time per task.
   - On limit hit: stop immediately, record what was attempted in `.ai/state.md`, and surface the failure/blocker explicitly to the user/foreman.
3. **Explicit termination criteria:**
   - Define what "done" means before executing (e.g., "all tests in changed files pass", "no new compile errors", "schema validation passes").
   - If the agent cannot meet the criteria within the iteration/time limits, it must stop instead of retrying blindly.
4. **Progress tracking state file:**
   - Maintain a small state file per task in `.ai/state.md` (recording changed files, tests run, and remaining errors).
   - Agents must read this state before acting to avoid re-doing work or repeating failed attempts.
5. **Subagents / role isolation:**
   - Instead of one agent doing everything in a single bloated context, split responsibilities across specialized roles:
     - **Planner:** reads high-level brief, writes a short plan in `.ai/plan.md`.
     - **Coder:** given the plan and a small set of target files, writes/edits code.
     - **Tester:** runs tests only for changed areas, summarizes results.
     - **Auditor:** checks consistency with schemas, world bible, and architecture docs.
   - Each subagent gets only the context it needs (plan + relevant files + last summary), preventing context contamination.
6. **Conflict resolution rule:**
   - If narrative and systems conflict, **Systems win** unless explicitly overridden by the foreman/user.
   - The agent must log all conflicts in `.ai/state.md` instead of silently picking an arbitrary resolution.
7. **Pre-generation and pre-edit checks:**
   - **Check existing tests and schemas:** Run Go config/save validators (`bin/validate-config` or `bin/ashfall-dev validate-config`) and existing tests. If validation fails, fix root cause instead of generating more content on top of broken data.
   - **Check for existing equivalent content:** Before creating a new quest, system, or test, search for similar IDs/concepts in data and code. Prefer extending or refining an existing system rather than adding a parallel one.
8. **Plan Approval Requirement (Pre-commit / CI Guard):**
   - Every commit with code changes MUST have a corresponding plan file in `.ai/plans/` (or `.ai/plan.md`) with `STATUS: APPROVED BY USER`.
   - If missing, pre-commit and CI will fail: `No approved plan found for changed files. Create/update a plan in .ai/plans/ and set STATUS: APPROVED BY USER.`
9. **Testing Budget & Scoped Runner:**
   - Always run tests via `bin/run-scoped-tests`. Do not invoke `pytest`, `dotnet test`, etc. directly unless explicitly told.
   - NEVER run the full test suite unless the user types exactly: `RUN FULL TESTS`.
   - Maximum 10–15 testing steps per task: if failing tests cannot be fixed within 10–15 steps, auto-flag the issue in `.ai/state.md` for a bug validator to fix. Do not loop on micro-edits.
