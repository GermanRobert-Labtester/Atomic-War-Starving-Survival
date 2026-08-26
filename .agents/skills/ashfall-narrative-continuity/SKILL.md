---
name: ashfall-narrative-continuity
description: Cross-file canon and flag consistency auditor for ASHFALL's 199 narrative JSON files — quests, encounters, echoes, radio, flags, factions. Finds continuity breaks across the story graph without rewriting prose.
---

# ASHFALL Narrative Continuity Auditor

## ROLE

ASHFALL's story is systemic: ~199 narrative JSON files cross-reference factions, flags, echoes, radio broadcasts, survivors, and endings. You verify the canon graph is consistent — that nothing references what doesn't exist, nothing contradicts established fact, and flag chains are coherent. You audit; `ashfall-write` and `ashfall-expand` create.

## WORKFLOW

### PHASE 1 — Canon Registry
- Build the current ground truth from `docs/ASHFALL_IMPLEMENTED_CANON_REGISTRY.md` plus the data authority: factions (stances, histories), survivor fields, world-state facts (`world_history.json` era: fictional nations only — "Meridian Compact" etc.), known endings.
- Flag any real-country/real-war/real-person references as immediate violations (gated by `DataRuleComplianceTests`).

### PHASE 2 — Reference Integrity
- Every `flag_*` set/read across files must pair: a flag read somewhere must be settable somewhere; a set flag with zero readers is a finding.
- Cross-file id references (faction ids, location ids, survivor ids) must resolve — leverage `CatalogIntegrityValidator` TIER-1/TIER-2 plus manual narrative-specific keys.
- Echo/radio chains: ordering constraints (`minDay`/`maxDay`) must be satisfiable.

### PHASE 3 — Contradiction Sweep
- Pairwise check high-traffic facts: faction positions, character fates, timeline events. A quest in `crossing_quests.json` cannot assume a faction stance that `crossing_factions.json` contradicts.
- Tone violations: magic/fantasy intrusions, glorified violence — flag per tone rules.

### PHASE 4 — Player Reachability
- For each narrative asset: is it reachable in any playthrough? Orphaned content (written, wired to nothing) is `DATA_ONLY` per the forensic taxonomy.

## RULES
- Read-mostly: findings go to the report; JSON edits only for mechanical reference fixes with owner approval.
- Tone and content rules from AGENTS.md are absolute.
- Verify with `godot --headless --path . -- --data-integrity-selftest` and `dotnet test` after any edit.

## OUTPUT
`docs/narrative/CONTINUITY_REPORT.md` — canon registry snapshot, reference matrix, contradiction findings with file:line evidence, orphaned content list, tone flags.

## QUALITY GATE
- Zero dangling references remain (or are explicitly waived with rationale).
- Every contradiction has file-level evidence and a proposed resolution.
