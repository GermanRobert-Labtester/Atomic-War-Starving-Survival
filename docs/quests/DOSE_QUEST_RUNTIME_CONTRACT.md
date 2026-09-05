# Dose Quest Runtime Contract

**Document ID:** `docs/quests/DOSE_QUEST_RUNTIME_CONTRACT.md`
**Related Systems:** `Assets/Ashfall.Core/DoseContentCatalog.cs`, `Assets/Ashfall.Core/DoseQuestMigration.cs`, `Assets/Ashfall.Core/QuestlineSystem.cs`
**Authority File:** `Assets/StreamingAssets/Data/dose_quests.json`

---

## 1. Catalog Architecture & Ingestion Path

`dose_quests.json` is loaded via `DoseContentCatalogLoader.Load(...)` in `Assets/Ashfall.Core/DoseContentCatalog.cs`.
The JSON payload is wrapped in a standard versioned container:
```json
{
  "schema_version": 1,
  "quests": [ ... ]
}
```

Each quest item maps to `DoseQuestDef`, which is then converted by `ToQuestlineDefinition` into the engine's canonical `QuestlineDefinition`:
- `questlineId`: unique identifier matching the canonical pattern.
- `title`, `synopsis`: presentation metadata.
- `factionTag`: filtering and standing metadata (`"none"` for internal shelter dose quests).
- `minDay`, `maxDay`: campaign scheduling bounds.
- `stages`: ordered list of `DoseQuestStage` records.

### First Stage Semantics
`firstStageId` is assigned deterministically to the first stage in the `stages` array (`def.firstStageId = rs.stageId;`).

---

## 2. Graph & Choice Semantics

### Stage Rules
- **Non-Terminal Stages:** `isTerminal: false`. Must contain 2 or 3 choices.
- **Terminal Stages:** `isTerminal: true`. Must have `choices: []`.
- **Transitions:** Every non-terminal choice specifies `nextStageId`, which must resolve within the same questline's stage map. No cross-questline jumps are permitted.
- **Topology:** Strictly directed acyclic graphs (DAGs) with zero cycles.

### Choice Execution & Reward Semantics
Each choice defines:
- `choiceId`: Unique choice identifier.
- `text`: Player action prompt.
- `nextStageId`: Next stage to advance to upon taking choice.
- `moraleDelta`: Integer morale delta applied to the active shelter session.
- `guiltDelta`: Integer guilt delta recorded in the guilt ledger.
- `grantItemId`: Optional item granted into shelter inventory.
- `grantItemQuantity`: Quantity of granted item (> 0 when item ID is set).
- `outcomeNarrative`: Narrative summary describing the consequence.

Rewards (morale, guilt, item grant) are applied exactly once at the moment of taking the choice in `QuestlineSystem.TakeChoice(...)`. Once recorded in `ActiveQuestlineRecord.choiceHistory`, re-entering or re-loading does not re-apply grants.

---

## 3. Save, Persistence & Migration Contract

Dose quests are owned exclusively by `DoseLedgerSave.quests` (v2+).
- In older v1 saves, dose quest records were shared inside the general Year of Ash envelope.
- `DoseQuestMigration.AdoptFromYearOfAsh(...)` scans `YearOfAshSave.quests` against `DoseQuestMigration.CanonicalQuestlineIds` and folds records into `DoseLedgerSave.quests`.
- `DoseQuestMigration.StripFromYearOfAsh(...)` removes the adopted records from the Year of Ash envelope to ensure single-owner persistence.

### Canonical Allowlist
The 12 canonical questline IDs are:
1. `quest_the_dose_the_first_reading`
2. `quest_the_sick_of_room_seven`
3. `quest_the_childs_number`
4. `quest_the_signed_hour`
5. `quest_the_falsified_reading`
6. `quest_the_stolen_dosimeter`
7. `quest_child_over_the_limit`
8. `quest_the_register_audit`
9. `quest_black_market_clean_bill`
10. `quest_the_broken_calibration_chain`
11. `quest_exposure_for_the_essential_worker`
12. `quest_the_missing_page`
