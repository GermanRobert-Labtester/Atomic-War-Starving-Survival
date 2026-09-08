# Plan 113 — Baseline Forensic Report: Verdict Questlines Expansion

> **Scope:** Forensic reconnaissance of `verdict_questlines.json`, `VerdictQuestCatalogLoader.cs`, `VerdictQuestMigration.cs`, `QuestlineSystem.cs`, `verdict_locations.json`, `verdict_npcs.json`, and `verdict_radio.json` prior to the Plan 113 expansion.

---

## 1. Initial State & Repository Truth

| Vector | Plan Assumption | Repository Reality | Notes / Reconciliation |
|---|---|---|---|
| **Catalog Path** | `Assets/StreamingAssets/Data/verdict_questlines.json` | `Assets/StreamingAssets/Data/verdict_questlines.json` | Exact match. Mirror at `builds/linux/...` synchronized. |
| **Catalog Count** | 8 baseline investigation cases | 16 questlines (8 baseline narrative + 8 court procedural) | Quests 1–8 are the original core narrative investigation cases (`quest_verdict_the_warm_range` through `quest_verdict_the_summons`). Quests 9–16 are court procedural cases added in commit `ccd1322c` (`quest_verdict_alibi_verification` through `quest_verdict_reconciled_testimony`), gated by `Plan18ExpansionDeepeningTests`. |
| **Expansion Target** | Add 7 new investigation cases (8 → 15 narrative) | Added 7 new cases (total catalog 16 → 23) | Preserved all 8 narrative cases and all 8 court cases; added the 7 planned investigation cases. |
| **Loader Class** | `VerdictQuestCatalogLoader.cs` | `Assets/Ashfall.Core/Verdict/VerdictQuestCatalogLoader.cs` | Confirmed live. Loads wrapped `{"schema_version": 1, "quests": [...]}` into `QuestlineDefinition` instances and registers with `QuestlineSystem`. |
| **Migration Class** | `VerdictQuestMigration.cs` | `Assets/Ashfall.Core/Verdict/VerdictQuestMigration.cs` | Migrates `quest_verdict_*` prefixes between YearOfAsh and Verdict save states without hardcoded ID limits. |
| **Runtime Engine** | `QuestlineSystem.cs` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` | Directed acyclic graph stage evaluator supporting branching choices, item grants, morale/guilt deltas, faction standing shifts, and day gating. |

---

## 2. Baseline Eight Narrative Questlines

1. **`quest_verdict_the_warm_range`** ("The Warm Range", Days 160–360, Tempest)
   - Entry: `stage_warm_path` | Follow cable east into fuse world.
2. **`quest_verdict_the_reckoning_call`** ("The Reckoning Call", Days 210–360, Tempest)
   - Entry: `stage_call_carrier` | Carrier on 99.0 MHz; department speaking.
3. **`quest_verdict_the_hold`** ("The Hold Pending Count", Days 200–360, Tempest)
   - Entry: `stage_hold_register` | Dead hand UXO field clearance vs census registry.
4. **`quest_verdict_eden_grabs`** ("Eden Was Here", Days 190–360, Archivists)
   - Entry: `stage_eden_tape` | Tube-bleed recovery from the array's memory.
5. **`quest_verdict_the_tape_silo`** ("The Reels That Matter", Days 200–360, Archivists)
   - Entry: `stage_reels_lectern` | 2,016 reels, 1,831 mattering, routed before count comes due.
6. **`quest_verdict_the_mortars_timetable`** ("The Mortar's Timetable", Days 170–360, Tempest)
   - Entry: `stage_mortar_chart` | 12-hour clock firing schedule analysis.
7. **`quest_verdict_the_shift_charter`** ("Shift 36", Days 180–360, Tempest)
   - Entry: `stage_shift_ledger` | Six names, five hands, one missing hand turning the valve.
8. **`quest_verdict_the_summons`** ("The Summons", Days 250–360, Central Garrison / Multi-faction)
   - Entry: `stage_summons_carrier` | Count presented to Garrison, Militia, Cult, or Warlords.

---

## 3. Seven New Investigation Cases to Integrate

1. `quest_verdict_the_dead_frequency` (Tempest, Days 170–340): Ghost carrier cycling on 99.0 MHz at Pass 4 Relay Mast.
2. `quest_verdict_the_missing_reel` (Archivists, Days 180–350): Missing reel 1832 from Archive Tape-Silo masking surveyor erasure.
3. `quest_verdict_the_cold_reading` (Tempest, Days 190–360): Cape Wrath zero-radiation anomaly forensic inquiry.
4. `quest_verdict_the_unsigned_tally` (Archivists, Days 200–360): Border checkpoint ledger with 42 missing corpses.
5. `quest_verdict_the_interference_pattern` (Tempest, Days 220–360): Unsanctioned sea-cave scrambler nulling 99.0 MHz.
6. `quest_verdict_the_last_entry` (Archivists, Days 240–360): St. Jude Marine Lab sealed vault survival log.
7. `quest_verdict_the_open_count` (Independent, Days 250–360): Final 48-hour census window disposition.
