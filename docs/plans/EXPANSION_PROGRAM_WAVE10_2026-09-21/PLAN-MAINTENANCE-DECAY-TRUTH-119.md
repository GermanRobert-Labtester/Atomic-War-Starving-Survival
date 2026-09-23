# PLAN-MAINTENANCE-DECAY-TRUTH-119 — Condition Decay, Repair & Service Cycles

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-BALANCE-DIFFICULTY-INTEGRATION-73.
**Non-goals:** no new item catalog, no repair economy rewrite (Plan 96 owns
arithmetic), no durability simulation on cosmetic items.

## 1. Outcome
Decay exists as a difficulty scalar (`equipment_decay_mult`: 0.8 → 1.5 across
SPARING…DIRGE, Plan 73 Appendix A) and as host-unreachable systems:
`Shelter/ShelterMaintenanceSystem.cs`, `Medical/ProstheticConditionWearEngine.cs`,
plus `Medical/SurgicalGraftRejectionEngine.cs` (failure-side). No contract
states which items decay, from what use, at what rate, or how repair restores
them — so the scalar either does nothing or something inconsistent.

| Deliverable | Detail |
|---|---|
| Decay classes | per item/tool/shelter-component class: the use event that decays it and the rate row (scaled by the difficulty multiplier) |
| Repair rules | repair restores a documented fraction per action, consumes a documented kit/part, and cannot exceed original condition |
| Service cycles | shelter components (pumps, filters, generators) decay on operational hours, not wall time |
| Failure thresholds | at/below threshold the item degrades to a documented state (inefficient/broken) — never silent removal |
| Difficulty binding | the scalar multiplies rate rows; a test proves SPARING ≠ DIRGE without touching any other constant |

## 2. Evidence
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs` exists (Core root, 555 lines) and is the likely existing condition owner — the package verifies whether this plan extends it rather than adding a second contract.
- `Assets/StreamingAssets/Data/difficulty_presets.json`: `equipment_decay_mult` 0.8/1.0/1.25/1.5 (Plan 73 Appendix A).
- Plan 1 Appendix A: `ShelterMaintenanceSystem`, `ProstheticConditionWearEngine`, `SurgicalGraftRejectionEngine` host-unreachable; Appendix K lists their public members (e.g. capture/restore on the prosthetic engine).
- Plan 112 owns produced-goods quality; condition is orthogonal and must not alias quality.
- Plan 96 owns price/repair arithmetic rows.

## 3. Packages
- **MDT-119A** decay class table (use event → rate row) with data rows schema-checked.
- **MDT-119B** difficulty binding test (SPARING vs DIRGE on one class).
- **MDT-119C** repair rules + kit consumption through the inventory seam.
- **MDT-119D** service-hour decay for shelter components (hour source = Plan 33 clock).
- **MDT-119E** failure-threshold state tests (inefficient/broken, no silent removal).

## 4. Acceptance & verification
- Decay on identical activity is seed-stable and multiplier-scaled.
- Repair cannot exceed original condition; kit consumption balances in Plan 93's wrapper.
- Service decay uses game hours only; OS clock change has no effect.
- `bash scripts/run_test.sh` on the inventory/shelter regions.

## 5. Risks
Condition aliasing quality → orthogonal by contract; a test asserts a fine item can be worn.
Scalar unbound → the difficulty binding test is the proof it is live.

---

## 6. Expanded census (2 files · 325 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `IEquipmentConditionSink.cs` | 19 | Support | — | 0 | 0 | 0 |
| `ShelterMaintenanceSystem.cs` | 306 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `chronic_conditions.json` | object[3 keys] |
| `bunker_maintenance_glitches.json` | object[3 keys] |
| `bunker_maintenance_logs_batch_2.json` | object[4 keys] |
| `bunker_maintenance_logs_batch_3.json` | array[25] |
| `carbide_tool_wear_audits.json` | array[8] |
| `deadbeat_escapement_wear_logs.json` | array[8] |

**State surfaces:** `ShelterMaintenanceSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain method: plan-body artifact list.
Governed artifacts: 13. Other plans referencing them: **10**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `EVIDENCE` | 4 |
| `PLAN-BIONICS-ENHANCEMENT-78` | 2 |
| `PLAN-SILENT-FAILURE-35` | 1 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 1 |
| `PLAN-ACUTE-TRAUMA-CARE-124` | 1 |
| `PLAN-STARTING-LEVEL-TRUTH-145` | 1 |
| `PLAN-SURGICAL-WARD-TRUTH-213` | 1 |
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `Assets/StreamingAssets/Data/difficulty_presets.json` |
| `IEquipmentConditionSink.cs` |
| `Medical/ProstheticConditionWearEngine.cs` |
| `Medical/SurgicalGraftRejectionEngine.cs` |
| `Shelter/ShelterMaintenanceSystem.cs` |
| `ShelterMaintenanceSystem.cs` |
| `bunker_maintenance_glitches.json` |
| `bunker_maintenance_logs_batch_2.json` |
| `bunker_maintenance_logs_batch_3.json` |
| `carbide_tool_wear_audits.json` |
| `chronic_conditions.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `MDT-119A` | no name match — resolve at claim time |
| `MDT-119B` | `Assets/StreamingAssets/Data/difficulty_presets.json` |
| `MDT-119C` | no name match — resolve at claim time |
| `MDT-119D` | `Shelter/ShelterMaintenanceSystem.cs`, `ShelterMaintenanceSystem.cs` |
| `MDT-119E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 12. Host files: **10** · Test files: **18** · Data files: **3**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/CombatHostSession.cs`, `src/Host/EquipmentConditionHostSession.cs`, `src/Host/HostCli.Difficulty.cs`, `src/Host/HostCli.PanelTests.cs`, `src/Host/Plans74To77HostSessions.cs` |
| Tests (`Ashfall.Core.Tests/`) | 18 | `Ashfall.Core.Tests/BunkerMaintenanceCatalogTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs`, `Ashfall.Core.Tests/Difficulty/Plan181DifficultySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/Equipment/EquipmentConditionCatalogTests.cs`, `Ashfall.Core.Tests/EquipmentConditionDegradationTests.cs` |
| Data (`StreamingAssets/Data/`) | 3 | `Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json`, `Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json`, `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **25** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `combat` |
| `equipment` |
| `equipment_condition` |
| `expanded_shelter` |
| `maintenance` |
| `narrative` |
| `narrative_questlines` |
| `procedural_narrative` |
| `relationship_decay` |
| `shelter` |
| `shelter_assignment` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **31** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--combat-breaching-selftest` |
| `--combat-selftest` |
| `--difficulty-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--narrative-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **28**.

| Event | First declaration |
|---|---|
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnChronicFibrosisMarked` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnChronicIllnessRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnCombatPenaltyChanged` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnCombatPerkEarned` | `Assets/Ashfall.Core/Combat/CombatPerks.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionsChanged` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/memory_decay_rates.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/relationship_decay_profiles.json` |
| `Assets/StreamingAssets/Data/rerailing_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (88 files, 758 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Equipment` | 1 | 4 |
| `Shelter` | 87 | 754 |

**Verdict:** 758 cases sit under matching regions — run those first (`Equipment`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **56**
(12 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |
| `src/Host/RelationshipDecayHostSession.cs` |
| `src/Host/RelationshipDecaySaveStore.cs` |
| `src/Host/RelationshipDecaySelfTest.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |
| `src/Host/ShelterAtmosphereHostSession.cs` |
| `src/Host/ShelterAtmosphereSaveStore.cs` |
| `src/Host/ShelterAtmosphereSelfTest.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **19**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `equipment` | no |
| `equipment_condition` | no |
| `expanded_shelter` | no |
| `maintenance` | no |
| `relationship_decay` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **12**
(CODEX_ONLY 6, GAMEPLAY_CONSUMED 2, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `narrative/bunker_maintenance_glitches.json` | CODEX_ONLY |
| `narrative/bunker_maintenance_logs_batch_2.json` | CODEX_ONLY |
| `narrative/bunker_maintenance_logs_batch_3.json` | CODEX_ONLY |
| `narrative/equipment_failure_logs.json` | CODEX_ONLY |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `rerailing_equipment_catalog.json` | UNRESOLVED |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |

**Verdict:** 4 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 19 (laddered 0) · RNG streams 1 · host files 13 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MAINTENANCE-DECAY-TRUTH-119
wave: 10
status: PROPOSED — foreman claim required
packages: MDT-119A, MDT-119B, MDT-119C, MDT-119D, MDT-119E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/breaching_equipment_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/memory_decay_rates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Equipment/
  - godot --headless --path . -- --combat-breaching-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
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
