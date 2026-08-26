---
name: ashfall-dependency-map
description: Maps ASHFALL Core-to-Godot dependencies, ownership boundaries, event wiring, and orphan or circular paths using current source evidence. Use for architecture audits, migration planning, or suspected host logic leakage.
---

# ASHFALL Dependency Mapper

## Role

Make system ownership visible without turning a dependency report into an
unreviewed refactor. The map covers `Assets/Ashfall.Core/`, `src/`, data
loaders, tests, and explicitly named legacy paths only when needed for
migration evidence.

## Workflow

1. Define the scope and entry points, such as `Main`, a host session, or one
   expansion. Record the current branch state and stale-document caveats.
2. Map actual edges: project references, namespaces, constructor parameters,
   field ownership, direct calls, event subscriptions, save registration, and
   CLI/self-test dispatch. Treat a `using` directive alone as no runtime edge.
3. Label each node as `CORE`, `GODOT_HOST`, `DATA`, `TEST`, or `LEGACY_INPUT`.
4. Identify cycles, duplicated state owners, Core-to-host reverse dependencies,
   unregistered systems, and host calculations that duplicate Core behavior.
5. Verify suspicious edges by reading the implementation. Never infer that a
   system is wired merely because its class exists or a plan mentions it.
6. Produce a dependency matrix and a smallest-safe remediation sequence. Code
   changes go through `ashfall-implement`; this skill is read-only by default.

## Rules

- Core must not depend on Godot, Unity, or `JsonUtility`.
- Godot owns presentation, input, adapters, and lifecycle, not gameplay rules.
- Do not recommend broad decomposition solely because a file is large; show
  shared-state and lifecycle evidence first.
- Do not run Unity tools.

## Output

Return entry points, graph/matrix, ownership violations, cycle evidence,
orphan candidates, confidence notes, and ordered recommendations. If a report
is requested, use `docs/architecture/DEPENDENCY_MATRIX_<scope>.md`.

## Quality gate

- Every reported edge cites a file and line or project declaration.
- “Orphan” means no discovered registration/reachability path, not merely no
  textual reference.
- The report distinguishes confirmed defects from inspection gaps.
