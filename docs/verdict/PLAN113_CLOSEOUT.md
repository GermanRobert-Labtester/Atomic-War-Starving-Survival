# Plan 113 Closeout Report — Verdict Questlines Expansion

> **Task Title:** Plan 113 — Verdict Questlines Expansion (8 → 15 Multi-Stage Investigation Cases)
> **Status:** COMPLETE
> **Verification Status:** 100% PASS across unit test suite, data integrity gate, content utilization gate, scene binding, and Godot host build.

---

## 1. Executive Summary

Plan 113 expands ASHFALL's Verdict narrative layer from the verified baseline of 8 core investigation cases (and 8 court procedural cases, total 16) to **23 total questlines**, adding **7 new multi-stage investigation cases**.

The expansion deepens the core Verdict experience: reconstructing post-nuclear truth from signal telemetry, erased archival spools, unlogged radiation anomalies, border body counts, sea-cave jamming transmitters, sealed marine research vaults, and contested census rolls. The central thematic question is upheld throughout: *not merely "what happened?", but "who gets to decide that the count is complete?"*

All seven new questlines are authored with strict directed acyclic graph (DAG) invariants, valid entry and terminal states, meaningful multi-faction standing trade-offs, canon item grants, and day pacing spanning Days 170 through 360.

---

## 2. Deliverables Summary

### 2.1 Data Authority & Catalogs
- **File:** `Assets/StreamingAssets/Data/verdict_questlines.json` (and build mirror at `builds/linux/Assets/StreamingAssets/Data/verdict_questlines.json`)
- **Total Questlines:** 23 (8 original narrative cases + 8 court procedural cases + 7 new investigation cases)
- **All 16 Previous Cases Preserved Verbatim:**
  1. `quest_verdict_the_warm_range`
  2. `quest_verdict_the_reckoning_call`
  3. `quest_verdict_the_hold`
  4. `quest_verdict_eden_grabs`
  5. `quest_verdict_the_tape_silo`
  6. `quest_verdict_the_mortars_timetable`
  7. `quest_verdict_the_shift_charter`
  8. `quest_verdict_the_summons`
  9. `quest_verdict_alibi_verification`
  10. `quest_verdict_witness_subpoena`
  11. `quest_verdict_charter_authentication`
  12. `quest_verdict_prior_verdict_appeal`
  13. `quest_verdict_chain_of_custody`
  14. `quest_verdict_machine_interpretation_contest`
  15. `quest_verdict_forged_evidence_inquest`
  16. `quest_verdict_reconciled_testimony`

### 2.2 Seven New Investigation Cases

| ID | Title | Faction | Days | Stages | Thematic Seam | Key NPCs / Evidence |
|---|---|---|---|---|---|---|
| `quest_verdict_the_dead_frequency` | The Dead Frequency | Tempest | 170–340 | 4 | Ghost carrier cycling on 99.0 MHz at Pass 4 Relay Mast | Karel Norn, military RTG, `evidence_call_calibration`, `item_archive_tape_silo_key` |
| `quest_verdict_the_missing_reel` | The Missing Reel | Archivists | 180–350 | 4 | Erased accession line in Tape-Silo masking southern survey disaster | Kasper Holt, reel 1832, `evidence_reels_matter`, `item_theodolite_brass_precision` |
| `quest_verdict_the_cold_reading` | The Cold Reading | Tempest | 190–360 | 4 | Cape Wrath zero-microsievert anomaly during black fallout | Ilya Venn, intake bypass valve, `item_faraday_mesh`, `evidence_call_calibration` |
| `quest_verdict_the_unsigned_tally` | The Unsigned Tally | Archivists | 200–360 | 4 | Gate Seven registry discrepancy concealing 42 uncounted refugees | Selya Saltmarsh, clinker burn pit, `evidence_census_draft`, `evidence_fuse_linen` |
| `quest_verdict_the_interference_pattern` | The Interference Pattern | Tempest | 220–360 | 4 | Submerged hydro-turbine naval pulse scrambler blanketing 99.0 MHz | Garrick Daal, cryptographic firmware, `item_portable_pid_detector`, `evidence_call_calibration` |
| `quest_verdict_the_last_entry` | The Last Entry | Archivists | 240–360 | 4 | Welded airlock at St. Jude Marine Lab post-evacuation desalination trials | Dr. Sena Korr, titanium RO manifold, `evidence_eden_log`, `evidence_veen_your_people` |
| `quest_verdict_the_open_count` | The Open Count | Independent (`""`) | 250–360 | 4 | Final 48-hour census reconciliation between Garrison, Archivists, and Ash Sign | Maro Veen, unaligned nomads, `evidence_census_draft`, `evidence_call_plain` |

### 2.3 Graph Architecture & Mechanical Guarantees
1. **DAG Structure:** Every stage graph originates at `firstStageId`, progresses along explicit non-null `nextStageId` links, and terminates at authored `isTerminal: true` stages with empty next stage pointers. Zero disconnected cycles or dead-ends.
2. **Choice Density:** Every stage provides 2–4 authored choices featuring distinct mechanical consequences, narrative outcomes, and branch paths.
3. **Item Authority:** Every questline grants canonical items (`items.json` or `verdict_items.json`) including forensic evidence files (`evidence_call_calibration`, `evidence_reels_matter`, `evidence_census_draft`, `evidence_call_plain`, `evidence_fuse_linen`, `evidence_eden_log`, `evidence_veen_your_people`) and expedition tools (`item_archive_tape_silo_key`, `item_portable_pid_detector`, `item_theodolite_brass_precision`, `item_faraday_mesh`).
4. **Faction Standing Balance:** Choice consequences award and subtract standing across six active wasteland factions (`faction_the_tempest`, `faction_archivists`, `faction_central_garrison`, `faction_ash_militia`, `faction_scavengers`, `faction_ash_sign`).

---

## 3. Cross-Domain Integration

- **Plan 82 Canonical Locations:** Directly integrates investigations into Pass 4 Relay Mast, Archive Tape-Silo, Cape Wrath Meteorological Station, Gate Seven Checkpoint, North Cliff sea caverns, and St. Jude Marine Lab.
- **Plan 94 Radio Broadcasts:** Corroborates signal intelligence on the 99.0 MHz carrier frequency, automated telemetry beacons, and acoustic naval scrambler interference.
- **Plan 93 NPC Witness Network:** Anchors deposition and interrogation choices to established figures: Karel Norn, Kasper Holt, Ilya Venn, Selya Saltmarsh, Garrick Daal, Dr. Sena Korr, and Maro Veen.
- **Year of Ash / Verdict Save Continuity:** Cleanly ingested by `VerdictQuestCatalogLoader.cs` and migrated via `VerdictQuestMigration.cs` with zero save breaking changes.

---

## 4. Automated Testing Suite

Authored dedicated xUnit regression suite: `Ashfall.Core.Tests/Verdict/VerdictQuestExpansionTests.cs` (9 tests, all passing):

| Test Method | Purpose | Status |
|---|---|---|
| `LoadAndRegister_LoadsAllQuestlines_CountAtLeast23` | Verifies full catalog deserialization and minimum count threshold | PASS |
| `PreservesAllEightBaselineNarrativeQuestlines` | Ensures all 8 baseline narrative cases are preserved intact | PASS |
| `PreservesAllEightCourtProceduralQuestlines` | Ensures all 8 court procedural cases are preserved intact | PASS |
| `ContainsAllSevenNewInvestigationCases` | Asserts presence and valid headers for all 7 new cases | PASS |
| `NewQuestlines_HaveFourToSevenStages_AndTwoToFourChoicesPerStage` | Validates stage counts and choice branching density | PASS |
| `NewQuestlines_StageGraphsAreAcyclicDirectedGraphs_WithValidFirstStageAndTerminals` | Graph traversal proving all stages are reachable and lead to terminal stages | PASS |
| `NewQuestlines_HaveItemGrants_AndFactionStandingShifts` | Enforces item reward and faction standing integration | PASS |
| `NewQuestlines_DayWindowsAreOrdered_AndWithinRange` | Asserts day bounds conform to Day 160–360 pacing window | PASS |
| `NewQuestlines_SimulatedResolution_ExecutesToCompletion` | End-to-end execution simulation from start to terminal completion | PASS |

---

## 5. Verification Matrix Evidence

| Verification Step | Command | Exit Code | Result |
|---|---|---|---|
| **Expansion Tests** | `dotnet test Ashfall.Core.Tests --filter FullyQualifiedName~VerdictQuestExpansionTests` | 0 | 9 passed, 0 failed (99 ms) |
| **Data Integrity Gate** | `godot --headless --path . -- --data-integrity-selftest` | 0 | PASS (0 findings, 11,962 IDs authored across 298 catalogs) |
| **Content Utilization Gate** | `godot --headless --path . -- --content-utilization-selftest` | 0 | CI gate PASS (316 events, 581 catalogs) |
| **Scene Binding Gate** | `godot --headless --path . -- --scene-binding-selftest` | 0 | PASS (25/25 scenes passed) |
| **Scene Lint** | `python3 scripts/ci/scene-lint.py` | 0 | PASS (30 scenes checked, 0 errors, 0 warnings) |
| **Host Compilation** | `dotnet build Ashfall.csproj` | 0 | PASS (0 errors, 0 warnings) |
| **Full Unit Test Suite** | `dotnet test Ashfall.Core.Tests` | 0 | PASS (9,881 passed, 0 failed, 1m 11s) |

---

## 6. Conclusion

Plan 113 is fully implemented, verified, and integrated into the repository baseline. The Verdict questline catalog now contains 23 rich, resilient, multi-stage cases that provide player agency across the late-game census window without introducing any engine or save regressions.
