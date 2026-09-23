# PLAN-WEAPON-CONDITION-TRUTH-242 — Firearm Wear, Malfunction & Field Service

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MAINTENANCE-DECAY-TRUTH-119, PLAN-BALLISTICS-WORKBENCH-TRUTH-184, PLAN-COMBAT-DEPTH-62.
**Non-goals:** no generic decay contract (Plan 119), no ammunition production
(Plan 184), no combat resolver (Plan 62).

## 1. Outcome
`Combat/WeaponConditionSystem.cs` (**320 lines**) is reachable and unaddressed:
weapon wear and malfunction in the field. Plan 119 owns the generic contract,
Plan 184 the rounds, Plan 62 combat. The **weapon-specific** wear curve and
malfunction table (jams, misfires, parts breakage) are unowned, so weapons are
either eternal or randomly unreliable.

| Deliverable | Detail |
|---|---|
| Wear curve | condition loss per documented use (shots, environment, mishandling) under Plan 119's multiplier |
| Malfunction table | per condition band, a documented malfunction class with recovery actions (clear, field-strip, swap part) |
| Parts | worn parts are inventory items; service consumes them via Plan 93 |
| Combat coupling | malfunctions enter Plan 62 as typed events, never hidden damage changes |
| Save truth | condition and pending malfunctions restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Combat/WeaponConditionSystem.cs` (320 lines; unaddressed — Wave 18 audit).
- Plan 119 supplies the shared decay contract; Plan 184 the ammunition link.
- Plan 62 receives malfunction events.
- Plan 172's tools/service capability gate field repair (boundary noted).

## 3. Packages
- **WCT-242A** wear curve + difficulty-scaling test.
- **WCT-242B** malfunction table + fixture per class.
- **WCT-242C** part service path + conservation.
- **WCT-242D** Plan 62 event contract tests.
- **WCT-242E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Wear follows the curve and the difficulty multiplier; malfunctions match their band.
- Effects appear in Plan 62 exactly once; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Combat/`.

## 5. Risks
Random unreliability → bands and recovery actions are explicit.
Duplicate decay → Plan 119 remains the contract; the plan cites its multiplier.

---

## 6. Expanded census (2 files · 435 lines)

Scope: `Assets/Ashfall.Core/Combat/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `WeaponConditionSystem.cs` | 320 | System | **yes** | 0 | 0 | 0 |
| `WeaponEquipmentBridge.cs` | 115 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chemical_weapons.json` | object[2 keys] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Combat/` |
| Test references | 4 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Premise | the **premise** file(s) above must match the plan's stated line counts before edits |

## 9. Rollout sequence

1. Premise re-check: premise files unchanged since authoring (hash/mtime), or update the plan.
2. Interfaces: wire through the named owner; do not add a parallel store.
3. State: if capture/restore exists, register per Plan 1 Appendix Q; else state the system is stateless.
4. Data: resolve domain catalogs or report the loader path.
5. Verification: focused region + the plan's own acceptance table.
6. Regression: re-run this census; a changed file is a finding.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Interface/wiring | one owner per state, no parallel store |
| State/save | round-trip or explicit stateless verdict |
| Data binding | catalog resolves or loader path documented |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

Domain files: 2. Other plans referencing their names: **2**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-COMBAT-DEPTH-62` | 2 |
| `PLAN-COMBAT-FAMILY-TRUTH-273` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WCT-242A` | no name match — resolve at claim time |
| `WCT-242B` | no name match — resolve at claim time |
| `WCT-242C` | no name match — resolve at claim time |
| `WCT-242D` | no name match — resolve at claim time |
| `WCT-242E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **7** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 7 | `src/Host/CombatHostSession.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Main.Expeditions.cs`, `src/Main.Plans198_201.cs`, `src/Main.ShelterBatch3.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Combat/ChemWarfareSystemTests.cs`, `Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs`, `Ashfall.Core.Tests/CombatWeaponConditionTests.cs`, `Ashfall.Core.Tests/ExpeditionVehicleLogisticsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **27** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chem_warfare` |
| `chemical_dependency` |
| `chemical_recon` |
| `chemical_synthesis` |
| `combat` |
| `equipment` |
| `equipment_condition` |
| `expanded_shelter` |
| `expedition` |
| `expedition_stealth` |
| `expeditions` |
| `fluid_logistics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **29** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--chemical-dependency-save-selftest` |
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--expedition-encounter-bridge-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--expedition-playtest-selftest` |
| `--expedition-selftest` |
| `--journal-weather-panel-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **20**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnEquipmentChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnEquipmentDamaged` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnExpeditionCompleted` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnExpeditionFailed` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **3**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json` |
| `Assets/StreamingAssets/Data/rerailing_equipment_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (1 files, 4 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Equipment` | 1 | 4 |

**Verdict:** 4 cases sit under matching regions — run those first (`Equipment`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **4**
(1 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/UI/AnalogConditionGauge.cs` |
| `src/UI/EquipmentConditionPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **2**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `equipment` | no |
| `equipment_condition` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **0**.

| Stream |
|---|
| — | no seeded stream shares a token with this domain |

**Verdict:** no seeded stream shares a token — either the domain is deterministic without randomness (fine) or it draws from an unlisted source (check `System.Random`/time seeding before shipping).

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **2**
(CODEX_ONLY 1, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `narrative/equipment_failure_logs.json` | CODEX_ONLY |
| `rerailing_equipment_catalog.json` | UNRESOLVED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 2 (laddered 0) · RNG streams 0 · host files 4 · catalogs 5 · test regions 1 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WEAPON-CONDITION-TRUTH-242
wave: 18
status: PROPOSED — foreman claim required
packages: WCT-242A, WCT-242B, WCT-242C, WCT-242D, WCT-242E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Host/EquipmentConditionHostSession.cs  # §19 candidate host surface
  - src/UI/AnalogConditionGauge.cs  # §19 candidate host surface
  - src/UI/EquipmentConditionPanel.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/breaching_equipment_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Equipment/
  - godot --headless --path . -- --chemical-dependency-save-selftest
dependencies:
  - coordinate: 2 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
```

**Structural checklist**

| Check | Result |
|---|---|
| status | yes |
| wave | yes |
| depends | yes |
| non_goals | yes |
| outcome | yes |
| evidence | yes |
| packages | yes |
| acceptance | yes |
| risks | yes |
| verification | yes |
| coupling | yes |
| binding | yes |

**Pre-claim actions:** none — claim-ready.
