# PLAN-NOISE-DISCIPLINE-TRUTH-116 — Noise, Light & Detection Discipline

**Wave 10 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DEPRECATED-TREE-RETIREMENT-94, PLAN-BASE-DEFENSE-RAIDS-61, PLAN-SPATIAL-SIM-AUTHORITY-95.
**Non-goals:** no new stealth engine while the Unity-era file is unresolved;
this plan defines semantics, then binds to whichever port Plan 94 lands.

## 1. Outcome
Two noise implementations exist side by side: `Shelter/ShelterNoiseSystem.cs`
(reachable Core, 560 lines — previously unaddressed by any plan) and the
Unity-era `Assets/_Game/Shelter/NoiseDisciplineSystem.cs`, which is compiled
**only** by the test project (`Ashfall.Core.Tests.csproj` line 29) and
exercised by `Performance/NoiseDisciplineBenchmarkTests`. Plan 94 decides the
latter's port or retirement; this plan first reconciles the **two authorities**
into one model. Meanwhile the raid/readiness half exists
(`World/NightWatchPatrolReadinessEngine` orphan, `Defense/PerimeterEarlyWarningEngine`
orphan, Plan 61) with no stated contract for what "quiet" buys a holdfast.

| Deliverable | Detail |
|---|---|
| Discipline model | noise sources (work, doors, machinery, gunfire, alarms) and light sources (lamps, fires, open shutters) with intensity bands |
| Detection effect | how noise/light feed raid likelihood and patrol readiness — one owner per input, no panel-side recomputation |
| Discipline actions | blackout, quiet hours, damped work — each maps to an existing action/order, not a new mode system |
| Port binding | the model binds to Plan 94's outcome: if ported, the engine is the calculator; if retired, the model names the replacement owner |
| Save truth | any persisted discipline state restores with its owner section; a missing section means default discipline, never a crash |

## 2. Evidence
- `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` + its `Compile Include` in `Ashfall.Core.Tests.csproj`; benchmark at `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`.
- Plan 1 Appendix G: `NightWatchPatrolReadinessEngine`, `PerimeterEarlyWarningEngine` candidate attach points.
- Plan 61 owns raid resolution; this plan supplies the detection input only.
- Plan 94 owns the port/retire decision; this plan must not pre-empt it.

## 3. Packages
- **NDT-116A** discipline model doc + intensity band table.
- **NDT-116B** detection input wiring at the Plan 61 seam (single owner).
- **NDT-116C** discipline action bindings (blackout/quiet hours/damped work).
- **NDT-116D** port binding note consumed by Plan 94's decision.
- **NDT-116E** save/default tests: absent state behaves as default discipline.

## 4. Acceptance & verification
- A scripted noisy day measurably raises the documented detection input; a quiet day lowers it — through the owning seam only.
- Panel values match the authority (no recomputation drift).
- Absent discipline save state loads with defaults.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Performance/` (focused) + any Plan 94 port target region.

## 5. Risks
Duplicating Plan 61 → this plan produces inputs, not raid outcomes.
Port timing → packages A/C/E land regardless; B binds after Plan 94.

---

## 6. Expanded census (1 files · 560 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ShelterNoiseSystem.cs` | 560 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `noise_sources.json` | object[2 keys] |

**State surfaces:** `ShelterNoiseSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 3 name references across the test tree |
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

Domain method: plan-body artifact list.
Governed artifacts: 6. Other plans referencing them: **3**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 3 |
| `PLAN-TEST-WELFARE-17` | 2 |
| `PLAN-BUILD-ERGONOMICS-56` | 2 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Ashfall.Core.Tests.csproj` |
| `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs` |
| `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `Shelter/ShelterNoiseSystem.cs` |
| `ShelterNoiseSystem.cs` |
| `noise_sources.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `NDT-116A` | `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`, `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `NDT-116B` | no name match — resolve at claim time |
| `NDT-116C` | `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`, `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `NDT-116D` | no name match — resolve at claim time |
| `NDT-116E` | `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`, `Ashfall.Core.Tests.csproj`, `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 5. Host files: **3** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/ShelterAtmosphereHostSession.cs`, `src/Host/ShelterAtmosphereSelfTest.cs`, `src/Main.ShelterAtmosphere.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/Performance/NoiseDisciplineBenchmarkTests.cs`, `Ashfall.Core.Tests/Shelter/Plan205ShelterNoiseIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/Plan220ShelterAtmosphereIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/ShelterNoiseSystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

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

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--atmosphere-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |
| `--shelter-operations-selftest` |
| `--shelter-ops-selftest` |
| `--shelter-physics-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnNoiseGenerated` | `Assets/Ashfall.Core/Maritime/SafeCrackingSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/atmosphere_profiles.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/guilt_sources.json` |
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **7** (133 files, 1094 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Shelter` | 87 | 754 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 1094 cases sit under matching regions — run those first (`Audio`, `Combat`, `Education`, `Foundry`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **362**
(22 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AmphibiousDraisineHostSession.cs` |
| `src/Host/ApprenticeshipHostSession.cs` |
| `src/Host/ArchiveDeskHostSession.cs` |
| `src/Host/AshfallInputActions.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `black_projects_archive` | no |
| `combat` | no |
| `disease` | no |
| `draisine_recovery` | no |
| `encounter_choice` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **16**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `combat` |
| `cupola_foundry` |
| `disease` |
| `foundry` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **152**
(CODEX_ONLY 110, GAMEPLAY_CONSUMED 27, OPTIONAL 2, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `environmental_atmosphere_expansion.json` | GAMEPLAY_CONSUMED |
| `environmental_texts_expansion_05.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 35 (laddered 0) · RNG streams 16 · host files 25 · catalogs 22 · test regions 7 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-NOISE-DISCIPLINE-TRUTH-116
wave: 10
status: PROPOSED — foreman claim required
packages: NDT-116A, NDT-116B, NDT-116C, NDT-116D, NDT-116E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/atmosphere_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --atmosphere-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
