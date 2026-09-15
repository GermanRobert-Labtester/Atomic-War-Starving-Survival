# Utility-AI authority map

Status: resolved, 2026-09-10.

## Decision

`Assets/Ashfall.Core/UtilityAI/` is the only gameplay Utility-AI authority.
The canonical types are:

| Type | Authority | Responsibility |
|---|---|---|
| `UtilityActionDef` | Core | catalog definition and eligibility data |
| `UtilityActionScorer` | Core | deterministic scoring, clamps, and tie selection |
| `UtilityAiSystem` | Core | explicit-dependency evaluation and selection |

`src/Host/UtilityAiHostSession.cs` is a thin Godot adapter. The only file in
`src/UtilityAI/` is `UtilityAiPanel.cs`; it binds Core results to the UI and
contains no scorer, action, consideration, or decision authority.

## Delta and consumer matrix

| Type/behavior | Core | Host | Decision | Evidence |
|---|---|---|---|---|
| Action identity/catalog | `UtilityAction.cs` | no duplicate | Core owns stable IDs and catalog loading | Core Utility-AI tests |
| Score/clamp/tie behavior | `UtilityActionScorer.cs` | no duplicate | Core owns ordinal tie-breaking and seeded modifier | `UtilityAiTests`, `UtilityAiProbeTests` |
| Evaluation/selection | `UtilityAiSystem.cs` | `UtilityAiHostSession` delegates | Core owns gameplay decision | host source authority gate |
| Presentation | none | `UtilityAiPanel.cs` | host-only adapter | panel binds Core scorer/system |
| Persistence | stateless | no save section | no Utility-AI save migration | `docs/utility_ai/UTILITY_ACTION_SAVE_CONTRACT.md` |

The host consumer count is one gameplay adapter plus the presentation panel;
neither recreates Core scoring. Core catalog expansion remains save-neutral:
current action state belongs to the survivor executor, not Utility AI.

## Determinism contract

The Core scorer explicitly covers empty considerations, zero weights,
unavailable actions, clamp bounds, equal-score ties, stable-ID ordering, and
same-state/same-seed replay. The fixed-scenario golden oracle is the existing
Core selection suite (`UtilityAiTests` and `UtilityAiProbeTests`); the source
gate also rejects Godot or `AtomicWar` namespaces under the Core authority.

Runtime-scale verification remains part of the release sequence. The
unification introduces no host/Core conversion objects or per-frame scoring
allocation path.

## Save compatibility

Utility AI has no persisted state, cooldown, memory, current action, or RNG
cursor. Therefore no wire field or checksum migration is required. If a
future feature persists an executor action, it must persist the stable
`action_*` ID in the owning survivor save section rather than adding state to
`UtilityAiSystem`.
