# Duty Season Save Compatibility

> **Persistence Contract:** Save/load guarantees, checksum verification, and backward compatibility across versions.

---

## 1. Stateless Season Derivation

- `DutyRosterSave` (CurrentSaveVersion: 3) serializes:
  - `simDay` (integer)
  - `roster` (`DutyRosterSystemState`)
  - `marks` (`MoraleMarkSystemState`)
  - `encounters` (`ShelterEncounterSystemState`)
  - `overflow` (`DutyRosterOverflowState`)
  - `quests` (`DutyRosterQuestState`)
  - `Checksum` (SaveChecksum hash)
- **Zero Save Schema Changes:** Active season ID is NOT persisted in `DutyRosterSave`.
- **Restoration Behavior:** Upon loading any save, the restored `simDay` is passed to `GetSeasonForDay(day)`. The active season is immediately re-resolved with zero serialization drift or stale cache risk.

---

## 2. Backward Compatibility with Legacy Saves

- Saves created prior to Plan 77 continue to load cleanly.
- If a legacy save was saved on Day 50, it previously resolved no season; after Plan 77, it dynamically maps to `season_spring_thaw` (days 31–60).
- Checksum contracts remain 100% intact because the save envelope fields are unchanged.
