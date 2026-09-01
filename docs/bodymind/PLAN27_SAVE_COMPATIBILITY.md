# Plan 27 Save Compatibility & Persistence Contract

This document guarantees byte-level and semantic compatibility between pre-Plan-27 save states and the expanded Body & Mind architecture.

---

## 1. Persisted vs. Derived State Table

| State Field | System Owner | Persisted / Derived | Storage Section / Key | Default for Old Saves | Migration / Upgrade Behavior |
| :--- | :--- | :--- | :--- | :--- | :--- |
| `DoseLedgerSystemState` | `DoseLedgerSystem` | Persisted | Section: `dose_ledger.json` | Existing entries preserved | Deserializes `entries`, `ceilingMsv`, `readingsSinceLastCalibration`. |
| `DoseEntry.hasForgedCleanBill` | `DoseLedgerSystem` | Persisted | `DoseEntry.hasForgedCleanBill` | `false` | Defaults to false; set true only if forged chit used. |
| `DoseEntry.adminOverrideBand` | `DoseLedgerSystem` | Persisted | `DoseEntry.adminOverrideBand` | `null` / empty | Defaults to empty; unaffected by older saves. |
| `AutopsyState` | `AutopsySystem` | Persisted | Section: `autopsy.json` | Empty cases / completed list | Pre-Plan-27 cases preserved; existing `completedSpecimenIds` respected. |
| `PsychContaminationSave` | `PsychologicalContaminationSystem` | Persisted | Section: `psych_contamination.json` | Empty list | Loaded into `_bySurvivor` dictionary without backfilling historical trauma. |
| `DoseQuestProgress` | `QuestlineSystem` | Persisted | Section: `dose_ledger.json` (`quests`) | Canonical 4 quests | New 8 questlines become available when `minDay` criteria are met. |

---

## 2. Invariant Save Guarantees
1. **Old Saves Load Cleanly:** An unupgraded save loads without error; new catalogs (`dose_items.json`, `dose_locations.json`, `dose_quests.json`) are immediately available in memory.
2. **No Fabricated History:** Pre-existing saves do not automatically acquire forged records, completed autopsies, or retroactive psychological contamination.
3. **Deterministic Round-Trip:** `CaptureState()` -> JSON Serialize -> Deserialize -> `RestoreState()` yields byte-identical state with normalized ordinal key ordering.
