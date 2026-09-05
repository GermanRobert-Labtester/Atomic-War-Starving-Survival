# Narrative continuity and prose audit — 2026-09-05

**Scope:** current JSON authority and current runtime reachability; historical continuity reports were treated as leads, not authority.
**Companion tasks:** T079–T100 in `docs/remediation/plans/2026-09-05_whole_repository_200_task_audit_plan.md`.

## Baseline

- `godot --headless --path . -- --narrative-selftest` passed 10/10.
- `elena_vasquez` and `marcus_olejnik` are both present in the current survivor authority and are valid narrative participants. Earlier claims that they were uncanonical are stale.
- Content utilization identifies 61 loaderless catalogs. Relevant authored families include NPC arcs, personal/repeatable/faction quests, endings, radio stations, travel/anomalous encounters, and multiple world/faction collections. Existing prose is not evidence of playable content until a live loader/consumer route exists.

## Confirmed editorial and presentation issues

1. `weather_almanac_expansion.json` and `ration_records_expansion.json` frequently repeat reversible constructions such as “the flour is the number; the number is the flour,” often several times in a single document. This is a current readability/voice problem, not a claim that its facts are false.
2. `epilogue_chronicle.json` still contains five `placeholder` art references. Ending presentation remains incomplete even if outcome logic is present elsewhere.
3. Existing structured records use recurring facts (day, ash, flour, headcount, heat/fuel) across collections without a current generated temporal ledger. Day-100/Day-110 records should be reconciled by date, store/perspective, and game-state gate before editing prose.
4. Geothermal/fuel material appears in narrative around the early-to-mid campaign timeline, but its discovery, construction/access, trade impact, and faction-knowledge gates require a runtime cross-check before treating any line as a contradiction.

## Required continuity discipline

- One canonical chapter map must tie opening pressure, survivor stakes, factions, discoveries, choice consequences, endgame gates, and epilogues to playable sources.
- Structured tags/participants/references need linting against canonical survivors, factions, locations, items, quests, and timeline facts; free prose should receive an editorial review rather than destructive token replacement.
- Document discovery must lead to a journal, route, choice, or clearly marked archival purpose. Loaderless content must be wired, explicitly archival, merged into a canonical catalog, or removed through a reviewed migration.
- Keep intentional ritual/trauma diction only when a named speaker, context, and editorial rationale make it purposeful; replace bulk repetition with concrete observation, distinct voice, and playable implication.

## First editorial/wiring batch

Execute T079–T086 first (reachability/quest/ending/radio authority), then T088–T100 (chapter map, consequences, continuity ledger, prose, and epilogue presentation). Do not rewrite high-volume narrative before its runtime route and factual constraints are known.
