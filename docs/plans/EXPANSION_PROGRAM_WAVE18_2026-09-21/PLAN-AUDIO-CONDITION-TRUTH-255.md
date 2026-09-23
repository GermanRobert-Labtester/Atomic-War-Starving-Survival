# PLAN-AUDIO-CONDITION-TRUTH-255 — Sound Equipment: Wear, Faults & Maintenance

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-AUDIO-MIX-AUTHORITY-97, PLAN-MAINTENANCE-DECAY-TRUTH-119, PLAN-RADIO-STATION-TRUTH-209.
**Non-goals:** no mix/bus policy (Plan 97), no generic decay contract (Plan 119),
no station operations (Plan 209).

## 1. Outcome
`AudioConditionSystem.cs` (**128 lines**, Core root) is reachable and
unaddressed: physical condition of audio equipment (speakers, receivers,
recording gear). Plan 97 owns the mix; Plan 119 the generic contract. What
**condition does to function** for audio devices is unstated — a speaker either
works or vanishes.

| Deliverable | Detail |
|---|---|
| Device model | audio devices with condition under Plan 119's multiplier and a failure band |
| Functional effect | degraded devices lose fidelity/range per documented bands, surfaced as a device state (not a silent mix change) |
| Maintenance | repair consumes parts via Plan 93; a repaired device returns to its band |
| Availability | a failed device is removed from active routing (Plan 97) visibly |
| Save truth | device conditions restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/AudioConditionSystem.cs` (128 lines; unaddressed — Wave 18 audit).
- Plan 97's routing consumes device availability.
- Plan 119 supplies the decay contract; Plan 209 may own station devices.

## 3. Packages
- **ACT-255A** device model + band table.
- **ACT-255B** fidelity-effect tests per band.
- **ACT-255C** repair path + conservation.
- **ACT-255D** routing-availability test with Plan 97.
- **ACT-255E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Bands change function per the table; failed devices leave routing.
- Repair restores the band and consumes parts; save/load preserves state.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Audio/`.

## 5. Risks
Silent mix change → device state is explicit; routing reads it.
Decay duplication → Plan 119 remains the contract.

---

## 6. Expanded census (5 files · 1,013 lines)

Scope: `Assets/Ashfall.Core/Audio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Support 2 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AudioAccessibilityCoordinator.cs` | 294 | System | — | 0 | 0 | 0 |
| `AudioSettingsCodec.cs` | 189 | Support | — | 0 | 0 | 0 |
| `AudioSettingsData.cs` | 175 | DTO/Type | — | 0 | 0 | 0 |
| `ScarcityAudioStateMachine.cs` | 300 | Support | — | 0 | 0 | 0 |
| `ShelterAudioCueCatalog.cs` | 55 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `audio_logs_expansion_05.json` | object[2 keys] |
| `audio_cues.json` | object[2 keys] |
| `shelter_audio_cues.json` | object[3 keys] |
| `audio_accessibility_cues.json` | object[3 keys] |
| `chronic_conditions.json` | object[3 keys] |
| `leather_harness_conditioning_audits.json` | array[7] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Audio/` |
| Test references | 6 name references across the test tree |
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

Domain files: 5. Other plans referencing their names: **7**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 5 |
| `PLAN-SETTINGS-INTEGRITY-54` | 2 |
| `PLAN-INPUT-REBINDING-106` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-ACCESSIBILITY-CLOSURE-51` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ACT-255A` | no name match — resolve at claim time |
| `ACT-255B` | no name match — resolve at claim time |
| `ACT-255C` | no name match — resolve at claim time |
| `ACT-255D` | no name match — resolve at claim time |
| `ACT-255E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 6; intra-domain edges: **1**; isolated files:
**4**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AudioSettingsCodec` | `AudioSettingsData` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `AudioSettingsData` | 1 |
| `AudioAccessibilityCoordinator` | 0 |
| `AudioConditionSystem` | 0 |
| `AudioSettingsCodec` | 0 |
| `ScarcityAudioStateMachine` | 0 |
| `ShelterAudioCueCatalog` | 0 |

**Class split:** hub 0 · sink 1 · source 1 · isolated 4.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 6. Host files: **6** · Test files: **8** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Audio/AudioConditionHostBridge.cs`, `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioManager.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Audio/AudioSettingsRecoveryTests.cs`, `Ashfall.Core.Tests/Audio/Plan169AudioAccessibilityIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/Plan52SoundOfScarcityIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/ShelterAcousticDirectorTests.cs`, `Ashfall.Core.Tests/AudioConditionSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **16** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `equipment_condition` |
| `expanded_shelter` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |
| `shelter_prisoners` |
| `shelter_reputation` |
| `shelter_schedule` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **16** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |
| `--shelter-ops-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **7**.

| Event | First declaration |
|---|---|
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |
| `Assets/StreamingAssets/Data/shelter_construction.json` |
| `Assets/StreamingAssets/Data/shelter_governance_blocs.json` |
| `Assets/StreamingAssets/Data/shelter_insulation_catalog.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (93 files, 788 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Shelter` | 87 | 754 |

**Verdict:** 788 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **61**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioConditionHostBridge.cs` |
| `src/Audio/AudioCueCatalog.cs` |
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioManager.cs` |
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/AudioSettings.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ExpansionAudioBridge.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/EquipmentConditionHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **16**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `equipment_condition` | no |
| `expanded_shelter` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **9**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 2, OPTIONAL 1, UNRESOLVED 4).

| Catalog | Classification |
|---|---|
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |
| `shelter_schedules.json` | GAMEPLAY_CONSUMED |
| `shelter_social_events.json` | GAMEPLAY_CONSUMED |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 7
**Surface:** save sections 16 (laddered 0) · RNG streams 1 · host files 13 · catalogs 21 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AUDIO-CONDITION-TRUTH-255
wave: 18
status: PROPOSED — foreman claim required
packages: ACT-255A, ACT-255B, ACT-255C, ACT-255D, ACT-255E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 7 other plan(s) name these artifacts (§12)
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
