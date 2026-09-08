# Plan 79 — Autopsy Procedures Expansion Closeout

## Summary

Expanded `autopsy_procedures.json` from **9 → 12 procedures** (the plan's stated baseline of 3 was stale — concurrent work had already authored six of the plan's proposed procedures) and fixed a **pre-existing critical data defect** that made the entire autopsy system unplayable. Pure data; zero Core changes; zero save-schema changes.

## Stale-premise reconciliation

Baseline audit found 9 procedures, not 3: the plan's working slate P4–P8 (blunt, ballistic, respiratory, hypothermia, spore isolation, neurotoxin assay) already existed alongside the original three (radiation, toxicology, containment). The gap analysis (see [PLAN_79_AUTOPSY_COVERAGE_MATRIX.md](PLAN_79_AUTOPSY_COVERAGE_MATRIX.md)) identified exactly **three uncovered death families**, which became the three new procedures:

1. **`procedure_deprivation_pathology`** — Deprivation & Wasting Pathology (starvation/dehydration; lowest risk tier; 3h; `finding_starvation_wasting` / `finding_severe_dehydration` / `finding_immune_collapse`; unlocks `knowledge_food_preservation`)
2. **`procedure_blast_injury`** — Blast Overpressure Forensics (primary blast injury distinct from ballistic fragment extraction; 5h; `finding_blast_lung` / `finding_overpressure_hemorrhage` / `finding_concussive_trauma`; unlocks `knowledge_fortified_chokepoints`)
3. **`procedure_forensic_unknown`** — Full Forensic Examination (broad uncertain/mixed cause; longest at 7h; moderate risk, not auto-highest; `finding_concealed_trauma` / `finding_mixed_cause` / `finding_toxin_indicator`; unlocks `knowledge_pathogen_containment`)

"Chemical exposure" and "suspicious death" from the working slate were **skipped as duplicates** (covered by existing toxicology/neurotoxin assay; suspicious-death folded into the forensic exam).

## Critical defect fixed (data)

`medical_scissors`, `protective_rubber_gloves`, `sterilised_bandage` existed in **no item catalog** — only as refs in the procedure file. All 9 baseline procedures required at least one phantom, so every `QueueAutopsy` returned `Blocked("missing_tool")`. Added the three items to `items.json` as ordinary schema-valid entries (plan §9/§12 exception). The autopsy system is now fully playable end-to-end.

## Final values

Full 12-procedure matrix with risks, hours, findings, and research: [PLAN_79_AUTOPSY_COVERAGE_MATRIX.md](PLAN_79_AUTOPSY_COVERAGE_MATRIX.md). Tool/consumable profiles: [AUTOPSY_TOOL_CONSUMABLE_INVENTORY.md](AUTOPSY_TOOL_CONSUMABLE_INVENTORY.md).

- All findings are inline `finding_*` strings per the actual schema (no external finding catalog exists — none manufactured as orphans; all follow the existing naming register).
- All research unlocks resolve against `research_knowledge.json` (56 `knowledge_*` nodes) — 0 invented IDs.
- `pathogenRisk` remains authored-but-unconsumed data (runtime only rolls `airborneRisk`); documented in the runtime contract for future consumers, values authored to the plan's relative-risk ladder.
- `procedure_hours` is int per DTO; new values 3/5/7 following the existing ladder.
- Runtime semantics honored: findings are a seeded random single pick; all research unlocks fire on completion; tools are consumed like supplies (existing behavior); one autopsy per specimen.

## Plan 09 / cross-plan integration

- Pathogen/containment procedures (#3, #8) + forensic (#12) connect to existing medical/ventilation content through existing IDs and the owned `VentilationSystem` — no new disease runtime.
- Research links are prevention-oriented: deprivation → food preservation, blast → fortification, forensic/containment → pathogen containment.
- Plan 65/69/55: no coupling added (documented handoffs only).

## Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS — 0 errors |
| Autopsy tests | PASS — **22/22** (catalog-expansion pin updated 9 → 12, the established pattern for pre-written contract tests) |
| Full `Ashfall.Core.Tests` | PASS — **9461/9461** |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `--data-integrity-selftest` | PASS — 0 errors, 0 warnings, 298 catalogs (11548 ids) |
| `--content-utilization-selftest` | PASS — CI gate PASS |
| Programmatic audit | 12/12 unique IDs · 0 unresolved item refs · 0 unresolved knowledge refs · risk/hours in range · 12 unique finding sets |

## Final status

**COMPLETE.** Twelve reference-clean, clinically distinct procedures cover ASHFALL's meaningful death families, use existing medical logistics (plus three ordinary items that should always have existed), expose real findings and existing research, and require no new Core code, save schema, or medical architecture.
