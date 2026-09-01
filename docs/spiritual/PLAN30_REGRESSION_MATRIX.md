# Plan 30 Regression & Verification Matrix

---

## 1. Automated Gates Checklist

- [x] **`dotnet test Ashfall.Core.Tests`**: Verifies unit and determinism tests across all systems.
- [x] **`godot --headless --path . -- --data-integrity-selftest`**: Verifies that all 153+ catalogs (including `spiritual_rituals.json`, `memorial_rites.json`, `belief_movements.json`, `events.json`, `bunker_children_folklore.json`, `bunker_graffiti_postings.json`) have 0 schema and reference errors.
- [x] **`godot --headless --path . -- --content-utilization-selftest`**: Verifies gameplay consumption and lack of orphaned catalogs.
- [x] **`godot --headless --path . -- --scene-binding-selftest`**: 22/22 UI panel scenes bound and passing.
- [x] **`python3 scripts/ci/scene-lint.py`**: 0 scene errors or warnings.

---

## 2. Plan 30 Specific Test Coverage Areas

1. **Catalog Integrity & Parsing:** `spiritual_rituals.json`, `memorial_rites.json`, `belief_movements.json`, expanded `bunker_children_folklore.json`, `bunker_graffiti_postings.json`.
2. **Ritual Cooldown & Anti-Exploit:** Proves repeated ritual triggers cannot farm morale.
3. **Staged Mourning Arc:** Proves deterministic stage transitions (Acute -> Empty Shift -> Return of Ordinary -> Memorial -> Anniversary).
4. **Memorial Rite Execution:** Validates partial grief mitigation without total erasure.
5. **Ideological Conflict Groups:** Validates friction between Ash Witnesses, Rebuilders, and Listeners.
6. **Save/Load Round-Trip:** Proves `SpiritualCoordinatorSaveState` deep copy serialization and restoration.
