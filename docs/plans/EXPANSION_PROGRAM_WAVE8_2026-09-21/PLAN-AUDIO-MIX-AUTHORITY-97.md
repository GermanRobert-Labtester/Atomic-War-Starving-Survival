# PLAN-AUDIO-MIX-AUTHORITY-97 — Bus Layout, Loudness, Ducking & Caption Coverage

**Wave 8 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-VERTICAL-CULTURE-04, PLAN-ACCESSIBILITY-CLOSURE-51, PLAN-HOST-COMPOSITION-GOVERNANCE-71.
**Implementation scaffold:** [`PLAN-AUDIO-MIX-AUTHORITY-97_APPENDIX-A_SCAFFOLD.md`](PLAN-AUDIO-MIX-AUTHORITY-97_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-AUDIO-CONDITION-TRUTH-255` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no new sound assets, no middleware, no engine swap; the cue
catalog and event bridge stay the authorities.

## 1. Outcome
`Assets/Ashfall.Core/Audio/` already owns cue and ambience logic:
`ShelterAudioCueCatalog`, `ShelterAcousticDirector`, `ReactiveAmbienceEvaluator`,
`ScarcityAudioStateMachine`, `AudioSettingsCodec`, `AudioSettingsData`,
`CassetteSetCatalogLoader`, plus two host-unreachable types
(`AudioAccessibilityCoordinator`, `CassettePlaybackSystem`), with host wiring in
`src/Main.Audio.cs`. What is missing is a **mix contract**: a stated bus
layout, loudness targets, ducking/priority rules, and captions for non-speech
cues that carry information.

| Deliverable | Detail |
|---|---|
| Bus layout | named buses (master, music, ambience, sfx, voice, alerts) with routing owned by the audio host; documented once |
| Loudness | per-bus normalization targets and a measurement procedure on the existing assets |
| Ducking/priority | voice and alert ducks ambience/music; emergency alerts outrank; no ducking loops |
| Captions | every information-bearing non-speech cue has a caption/subtitle entry; settings toggle respected |
| Orphan wiring | `AudioAccessibilityCoordinator` and `CassettePlaybackSystem` wired per Plan 1, or explicitly deferred with a named owner |

## 2. Evidence
- `Assets/Ashfall.Core/Audio/` file list; `Audio/ShelterAudioCueCatalog.cs`, `ShelterAcousticDirector.cs`, `ReactiveAmbienceEvaluator.cs`, `ScarcityAudioStateMachine.cs`.
- Plan 1 Appendix A: `AudioAccessibilityCoordinator`, `CassettePlaybackSystem` host-unreachable.
- `src/Main.Audio.cs` host adapter exists; `AudioSettingsCodec`/`AudioSettingsData` own settings persistence.
- Plan 51 (accessibility) owns the toggle surface; this plan supplies the caption coverage it toggles.

## 3. Packages
- **AMX-97A** bus layout + routing doc; one host implementation; default bus resource checked in if absent.
- **AMX-97B** loudness targets + measurement script over the existing catalog (report, no asset rewrite).
- **AMX-97C** ducking rules + tests over the evaluator state machine (priority table).
- **AMX-97D** caption table generation for information-bearing cues + toggle wiring test.
- **AMX-97E** orphan wiring packages (two types) or explicit deferral notes with owners.

## 4. Acceptance & verification
- Ducking table tests pass: alert > voice > ambience; no oscillation across a scripted scene sequence.
- Every information-bearing cue id appears in the caption table; the toggle suppresses captions but not cues.
- Loudness report produced with per-bus numbers.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Audio/` + one headless cue-catalog check.

## 5. Risks
Caption table drift → generated from the cue catalog with `--check`; a new
information cue without a caption row fails.
Asset churn → this plan measures and routes; it does not re-encode assets.

---

## 6. Expanded census (7 files · 1,328 lines)

Scope: `Assets/Ashfall.Core/Audio/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Support 4 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `AudioAccessibilityCoordinator.cs` | 294 | System | — | 0 | 0 | 0 |
| `AudioSettingsCodec.cs` | 189 | Support | — | 0 | 0 | 0 |
| `AudioSettingsData.cs` | 175 | DTO/Type | — | 0 | 0 | 0 |
| `ReactiveAmbienceEvaluator.cs` | 167 | Support | — | 0 | 0 | 0 |
| `ScarcityAudioStateMachine.cs` | 300 | Support | — | 0 | 0 | 0 |
| `ShelterAcousticDirector.cs` | 148 | Support | **yes** | 0 | 0 | 0 |
| `ShelterAudioCueCatalog.cs` | 55 | Catalog | **yes** | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `audio_logs_expansion_05.json` | object[2 keys] |
| `acoustic_triangulation_catalog.json` | object[4 keys] |
| `audio_cues.json` | object[2 keys] |
| `shelter_audio_cues.json` | object[3 keys] |
| `audio_accessibility_cues.json` | object[3 keys] |
| `hydrophone_acoustic_logs.json` | array[8] |

**State surfaces:** none.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Audio/` |
| Test references | 9 name references across the test tree |
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

Domain files: 9. Other plans referencing their names: **9**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-AUDIO-CONDITION-TRUTH-255` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-VERTICAL-CULTURE-04` | 2 |
| `EVIDENCE` | 2 |
| `PLAN-SETTINGS-INTEGRITY-54` | 2 |
| `PLAN-INPUT-REBINDING-106` | 2 |
| `PLAN-RECREATION-MORALE-50` | 1 |
| `PLAN-ACCESSIBILITY-CLOSURE-51` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `AMX-97A` | no name match — resolve at claim time |
| `AMX-97B` | `CassetteSetCatalogLoader.cs`, `CassettePlaybackSystem.cs`, `ShelterAudioCueCatalog.cs` |
| `AMX-97C` | `ScarcityAudioStateMachine.cs`, `ReactiveAmbienceEvaluator.cs` |
| `AMX-97D` | no name match — resolve at claim time |
| `AMX-97E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 9; intra-domain edges: **2**; isolated files:
**5**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `AudioSettingsCodec` | `AudioSettingsData` |
| `ShelterAcousticDirector` | `ShelterAudioCueCatalog` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `AudioSettingsData` | 1 |
| `ShelterAudioCueCatalog` | 1 |
| `AudioAccessibilityCoordinator` | 0 |
| `AudioSettingsCodec` | 0 |
| `CassettePlaybackSystem` | 0 |
| `CassetteSetCatalogLoader` | 0 |
| `ReactiveAmbienceEvaluator` | 0 |
| `ScarcityAudioStateMachine` | 0 |
| `ShelterAcousticDirector` | 0 |

**Class split:** hub 0 · sink 2 · source 2 · isolated 5.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 9. Host files: **4** · Test files: **8** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 4 | `src/Audio/AudioSettings.cs`, `src/Audio/ShelterAcousticBridge.cs`, `src/Audio/ShelterAudioController.cs`, `src/Main.Plans50_53.cs` |
| Tests (`Ashfall.Core.Tests/`) | 8 | `Ashfall.Core.Tests/Audio/AudioSettingsRecoveryTests.cs`, `Ashfall.Core.Tests/Audio/Plan169AudioAccessibilityIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/Plan52SoundOfScarcityIntegrationTests.cs`, `Ashfall.Core.Tests/Audio/Plan67CassetteSetsTests.cs`, `Ashfall.Core.Tests/Audio/ShelterAcousticDirectorTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **15** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
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
| `shelter_security` |

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

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnPlaybackChanged` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
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
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/cassette_sets.json` |
| `Assets/StreamingAssets/Data/narrative/hydrophone_acoustic_logs.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |
| `Assets/StreamingAssets/Data/shelter_components.json` |

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

Host files (`src/`) whose names share a domain token: **62**
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
| `src/Audio/SurfaceAmbienceController.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **15**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |
| `shelter_noise` | no |
| `shelter_prisoners` | no |
| `shelter_reputation` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **2**.

| Stream |
|---|
| `acoustic_detection` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **12**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 2, OPTIONAL 2, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `cassette_sets.json` | OPTIONAL |
| `narrative/hydrophone_acoustic_logs.json` | CODEX_ONLY |
| `narrative/shelter_notices_expansion.json` | CODEX_ONLY |
| `narrative/shelter_songs_expansion.json` | CODEX_ONLY |
| `shelter_machine_identities.json` | UNRESOLVED |
| `shelter_room_identities.json` | UNRESOLVED |
| `shelter_rooms.json` | UNRESOLVED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 15 (laddered 0) · RNG streams 2 · host files 14 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-AUDIO-MIX-AUTHORITY-97
wave: 8
status: PROPOSED — foreman claim required
packages: AMX-97A, AMX-97B, AMX-97C, AMX-97D, AMX-97E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
