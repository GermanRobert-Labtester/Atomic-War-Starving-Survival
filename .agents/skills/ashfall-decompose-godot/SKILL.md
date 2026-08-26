---
name: ashfall-decompose-godot
description: Produces an evidence-backed, behavior-preserving decomposition plan for ASHFALL Godot orchestration files such as Main.cs or GameBootstrap. Use when host ownership and lifecycle coupling need review; implementation remains a separate approved task.
---

# ASHFALL Godot Host Decomposer

## Role

Reduce host navigation and ownership risk without moving code blindly. This is
a read-only planning skill, not an automated file splitter.

## Workflow

1. Confirm the active Godot entry point and current partial-file layout. Read
   the lifecycle methods, save orchestration, CLI dispatch, and relevant domain
   partials.
2. Build a method-to-domain table for `Setup*`, tick/update, event wiring,
   `Save*`, `Flush*`, UI registration, and shutdown paths.
3. Record shared fields, initialization order, cross-domain calls, and hidden
   static/global state for each candidate boundary.
4. Check triad completeness: construction/wiring, capture/save, and deferred
   flush behavior. A method name alone does not prove a path is active.
5. Propose the smallest extraction that preserves lifecycle order and public
   contracts. Include an ordered move list, conflict risks, and tests required.
6. Hand the approved plan to `ashfall-implement`; verify each move incrementally.

## Rules

- Do not edit `Main.cs`, scenes, or Core from this skill.
- Do not split methods merely to reduce line count.
- Do not move gameplay rules into another Godot file; migrate shared behavior
  to Core instead.
- Preserve the established Godot namespace and project conventions.

## Output

Return current ownership, candidate boundaries, dependency hazards, and triad
gaps with file/line citations, plus an ordered extraction plan and rollback
checkpoints. If a report is requested, use
`docs/architecture/GODOT_HOST_DECOMPOSITION_<file>.md`.

## Quality gate

- Every proposed move lists its field and lifecycle dependencies.
- Save/load and headless paths are included in the impact assessment.
- No implementation is claimed until the compiler and relevant self-tests pass.
