# Plan 33 — Save Compatibility & Wire Contract

## 1. Cross-Host Wire Contract
- `SkillProgressionSystem` captures and restores its state via `SkillProgressionSaveState`.
- Active skills are stored as a list of canonical string IDs (`activeSkillIds`).
- Dormant skills are stored as a list of canonical string IDs (`dormantSkillIds`).
- Action XP per discipline is serialized in matching string/float lists (`disciplineIds`, `disciplineXps`).
- Expert discipline locks are persisted via `expertSkillEarned`.

---

## 2. Invariant Protection
- **No ID Renames:** All 145 baseline skill IDs remain 100% byte-identical to previous versions.
- **Additive Only:** 3 new skill IDs (`skill_field_surgery`, `skill_water_filtration`, `skill_radio_repair`) are purely additive.
- **Save Integrity:** `SaveChecksum` computation on `SkillProgressionSaveState` is completely unchanged. Existing player saves load without migration warnings or data loss.
- **Unknown Skill Tolerance:** If an unrecognized skill ID is encountered in legacy saves, `SkillProgressionSystem.RestoreState` gracefully preserves the entry without crashing.
