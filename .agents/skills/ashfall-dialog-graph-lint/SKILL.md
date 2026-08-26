---
name: ashfall-dialog-graph-lint
description: Graphs and lints quest/flag/echo/radio/dialog reachability — unreachable quest_, dead endings, missing flag_ producers/consumers, and minDay/maxDay windows. Use when adding narrative, quests, or flag-gated content.
---

# ASHFALL Dialog Graph Lint

## ROLE
199 narrative JSON files (`Assets/StreamingAssets/Data/narrative/`) form a flag-gated graph. `ashfall-narrative-continuity` checks canon; `ashfall-narrative-check` checks tone/reachability prose. You build the graph and prove every `quest_*`/`ending_*`/`echo_*`/`radio_*` is reachable and every `flag_*` has a writer and reader.

## RULES
1. JSON is authority — graph nodes are `quest_*`, `encounter_*`, `flag_*`, `ending_*`, `echo_*`, `radio_*`, `event_*` snake_case IDs; never invent outside master prefix list (`AGENTS.md:DATA INTEGRITY`).
2. Ranges `minDay`/`maxDay` must be ordered; `flag_` semantics respect `InMemoryFlagLedger` ordinal drift note.
3. Read-only lint; emits DOT/SVG, never rewrites prose.

## WORKFLOW
### PHASE 1 — Graph Build
- Parse all narrative JSON under `Assets/StreamingAssets/Data/narrative/` + quest/event JSON elsewhere. Nodes: quest steps, branches, prerequisites (`requiredItemId`, `flag_*`, `faction_*` trust), results (`flag_*` sets, `ending_*`).
- Edges: `requires → quest`, `sets → flag`, `flag → gated quest/event`, `choice → ending`.

### PHASE 2 — Reachability
- Orphan nodes: `quest_*` never required/gated from start set or prior flag.
- Dead `flag_*`: written but never read, or read but never written (missing producer/consumer).
- `minDay`/`maxDay` window never overlaps prerequisite flag availability (e.g., flag set Day 20 but quest `maxDay=15`).
- `ending_*` unreachable from any choice chain.

### PHASE 3 — Cross-Check
- Compare against `CatalogIntegrityValidator` TIER-1/TIER-2 — dangling `resultItemId`/`requiredItemId` already flagged there; add narrative-specific `flag_`/`quest_` edges.
- Emit `out/graph.dot` + rendered `out/graph.svg` (via `dot` if available) clustered by faction/sector.

### PHASE 4 — Verify
- `godot --headless --path . -- --data-integrity-selftest` 0 errors
- `dotnet test --filter DataRuleComplianceTests` green (no real countries/people leak)

## OUTPUT
`docs/narrative/DIALOG_GRAPH_LINT.md` — node/edge counts, orphan list, dead-flag table, unreachable endings, DOT/SVG paths, suggested wiring (which flag to set or which quest to gate).

## QUALITY GATE
- 0 orphan `quest_*`, 0 `flag_*` with zero producers or zero consumers (or explicit start-state exception), 0 unreachable `ending_*`.
