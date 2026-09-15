# ASHFALL PROJECT — CRUSH Instructions
# AUTO-GENERATED from AGENTS.md (canonical source). Run sync-agent-rulebooks.py to regenerate.
# Last generated: 2026-09-15

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

## TARGETED TESTING ONLY

- Run the smallest test file or directly affected region for the change; a
  builder normally stays below 100 cases.
- Prefer static inspection for sweeps. A larger diagnostic run requires a new
  hypothesis and explicit foreman/user reason; it must remain bounded and not
  compete with active builders.
- Run `bash scripts/run_test.sh <test-file-or-focused-directory>` for xUnit
  targets. New test files run alone first.
- Aggregate only homogeneous static mappings, labels, thresholds, and catalog
  tables with useful per-row failure output. Preserve independent save/load,
  determinism, lifecycle, mutation, fuzzing, state-transition, and
  cross-system tests.
- Quarantine/re-enable decisions require current API/content evidence, a
  written reason, and a passing focused target. Never silently re-enable.
- If a Godot runtime session is needed, use 15 FPS unless the user explicitly
  requests another target. Do not invoke Unity to test Godot behavior.

## ACTIVE HANDOFF — AGY (Antigravity): C1 UI PANEL WAVE

**AGY starts here.** Read `C1_COMPLETION.md` (repository root) before any other
file: the C1 economy core (Plans 14A + 14B + market/caravan wiring) is complete
and verified, and the tagged task is to generate/extend the `economy_detail`
and `traveling_caravan` panels on top of the existing Core read models
(embargo summary, typed price-factor records, caravan blocked state).
Presentation only — panels never recompute Core outcomes (rule D of the plan's
integration doctrine). Style authority: `DESIGN.md`. Full contract:
`docs/plans/C1_planintegration.md` §9, §14A.8–14A.9, §14B.7–14B.8.

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

- Prefer repository scripts and current APIs over ad-hoc replacement tooling.
- Do not modify generated outputs by hand; run the owning generator and its
  `--check` mode when one exists.
- Do not alter unrelated user changes, delete data, rewrite history, or reset
  the worktree.
- Do not invent MCP connections, external accounts, capabilities, or tool
  results. Use only configured tools and their documented scope.
- Keep changes reviewable: one owned system or governance package at a time.
