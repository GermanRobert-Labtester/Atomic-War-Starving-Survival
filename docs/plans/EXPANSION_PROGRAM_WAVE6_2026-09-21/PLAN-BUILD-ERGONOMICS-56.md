# PLAN-BUILD-ERGONOMICS-56 — Compile Times, Project Hygiene & Iteration Loop

**Wave 6 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TEST-WELFARE-17.
**Non-goals:** no build-system replacement, no module split mandate.

## Outcome
The tree is large (1,168 Core + 844 host + 1,378 test files, 226 setup methods)
and the loop is the main developer cost after CI. This plan measures and
bounds build/iteration time and repairs project-file hygiene.

| Deliverable | Detail |
|---|---|
| Baseline | wall time for `dotnet build` (Core, host, tests) hot and cold, and `run_test.sh` startup |
| Budget | documented ceilings; a regression gate reports deltas |
| Project hygiene | no duplicate/legacy includes, consistent TFM/SDK, no dead compile items |
| Test loop | fastest focused path documented; scratch-fixture setup cost measured |
| Editor loop | Godot import/compile step measured; no full reimport per change |
| Parallelism | safe parallel build/test settings documented (no shared-state tests) |

## Evidence
- Project files: `Ashfall.csproj`, `Ashfall.Core.Tests.csproj`, `Directory.Build.props`, `Directory.Packages.props`, `global.json`.
- Known hygiene issue: the test project still has a Unity-era `Compile Include` for `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` (PLAN-TEST-WELFARE-17 TW-17A).
- `scripts/run_test.sh` (180 s cap); 53 fast gates.
- Build gates: "Build Ashfall.Core.Tests (net9.0)", "Build Godot Host Application (net8.0)", "Compiler Warning Baseline Gate (0 Warnings)".

## Packages
- **BE-56A** baseline harness: timing script producing `artifacts/build-timings.json` (cold/hot per project).
- **BE-56B** budget + gate: warn/fail thresholds; report the slowest step; rebaseline requires a note.
- **BE-56C** project hygiene: remove legacy includes, align TFMs, kill dead compile items, verify zero-warning build stays.
- **BE-56D** focused loop doc: `docs/ci/DEV_LOOP.md` with the exact fast commands for Core/host/tests/headless.
- **BE-56E** Godot import cost: measure the import cache path; document a no-reimport workflow for C#-only changes.

## Acceptance & verification
- Baseline artifact committed; hot build under budget; zero legacy includes.
- `dotnet build Ashfall.slnx` timed; `bash scripts/run_test.sh <focused>` timed; the warning gate stays green.

## Risks
Timing flake on shared machines → median-of-3, generous margins, artifact-based comparison.

---

## 6. Expanded census (8 project/build files)

This plan's domain is the build loop, not the Core source tree; the census
therefore covers project files and build tooling.

| File | Lines | Bytes | Compile Include | TargetFramework |
|---|---:|---:|---:|---|
| `Ashfall.csproj` | 40 | 1778 | 2 | 1 |
| `Ashfall.Core/Ashfall.Core.csproj` | 43 | 1936 | 1 | 1 |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 39 | 1640 | 1 | 1 |
| `tools/ui-preview.csproj` | 16 | 494 | 1 | 1 |
| `Directory.Build.props` | 32 | 1446 | 0 | — |
| `Directory.Packages.props` | 26 | 1028 | 0 | — |
| `global.json` | 8 | 108 | 0 | — |
| `scripts/run_test.sh` | 173 | 5973 | 0 | — |

**Build tooling:** 67 script(s) under `scripts/ci/` · 57 gates in the CI manifest.
**Known hygiene item:** test project still includes `Assets/_Game/Shelter/NoiseDisciplineSystem.cs`: **yes**.

## 7. Expanded measurement surface

| Measure | How |
|---|---|
| Cold build | `dotnet build` from clean obj/bin, Core / host / tests recorded separately |
| Hot build | second consecutive build, same measurement points |
| Focused loop | `bash scripts/run_test.sh <file>` startup + run time |
| Editor loop | Godot import/compile step for one change |
| Gate cost | the fast-gate set from the CI manifest, per-gate wall time |

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Warning gate | 0-warning baseline holds after any project-file edit |
| Compile items | no duplicate includes; the `_Game` include is retired by Plan 94/17 |
| Parallelism | tests that share state are excluded from parallel runs (Plan 17) |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Measure the current loop and record cold/hot numbers.
2. Hygiene pass: duplicate/legacy includes removed with the owning plans.
3. Budgets documented with a regression report (no hard failure until measured twice).
4. Focused-loop path documented for the common cases.
5. Regression gate: compare against the recorded baseline.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Baseline | cold and hot numbers recorded per project |
| Hygiene | no duplicate includes; legacy include retired or explicitly tracked |
| Budget | ceilings documented; a regression report prints deltas |
| Loop | the fastest focused path is written down and verified once |

**Non-goals unchanged:** this expansion adds measurement and hygiene detail; it does not mandate a build-system change.

---

## 12. Cross-plan coupling

Domain method: plan-body artifact list.
Governed artifacts: 11. Other plans referencing them: **258**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-DEPRECATED-TREE-RETIREMENT-94` | 4 |
| `PLAN-TEST-WELFARE-17` | 3 |
| `PLAN-NOISE-DISCIPLINE-TRUTH-116` | 3 |
| `PLAN-INTEGRATION-KIT-02` | 2 |
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `PLAN-UNBLOCK-03` | 1 |
| `PLAN-VERTICAL-CULTURE-04` | 1 |
| `PLAN-LAUNCH-FACE-06` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Ashfall.Core.Tests.csproj` |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` |
| `Ashfall.Core/Ashfall.Core.csproj` |
| `Ashfall.csproj` |
| `Assets/_Game/Shelter/NoiseDisciplineSystem.cs` |
| `artifacts/build-timings.json` |
| `docs/ci/DEV_LOOP.md` |
| `global.json` |
| `run_test.sh` |
| `scripts/run_test.sh` |
| `tools/ui-preview.csproj` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `BE-56A` | `artifacts/build-timings.json` |
| `BE-56B` | no name match — resolve at claim time |
| `BE-56C` | `artifacts/build-timings.json` |
| `BE-56D` | `Ashfall.Core.Tests.csproj`, `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` |
| `BE-56E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 9. Host files: **807** · Test files: **1377** · Data files: **17**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 807 | `src/Audio/AudioConditionHostBridge.cs`, `src/Audio/AudioEventBridge.cs`, `src/Audio/AudioSelfTest.cs`, `src/Audio/AudioSettings.cs`, `src/Audio/ExpansionAudioBridge.cs` |
| Tests (`Ashfall.Core.Tests/`) | 1377 | `Ashfall.Core.Tests/AbyssalAnomaliesCatalogTests.cs`, `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`, `Ashfall.Core.Tests/Achievements/Plan149AchievementIntegrationTests.cs`, `Ashfall.Core.Tests/ActionResultTests.cs` |
| Data (`StreamingAssets/Data/`) | 17 | `Assets/StreamingAssets/Data/ceremonies.json`, `Assets/StreamingAssets/Data/door_encounters.json`, `Assets/StreamingAssets/Data/events.json`, `Assets/StreamingAssets/Data/faction_radio_corpus.json`, `Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `encounters` |
| `equipment_condition` |
| `events` |
| `faction_espionage` |
| `host_event` |
| `microfluidic_diagnostic` |
| `radio` |
| `radio_program_production` |
| `radio_station` |
| `shelter_noise` |
| `travel_encounters` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--accessibility-selftest` |
| `--audio-selftest` |
| `--audio-test` |
| `--duty-roster-loop-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--microfluidic-diagnostic-selftest` |
| `--microfluidic-diagnostic-uitest` |
| `--playable-loop-selftest` |
| `--radio-catalog-selftest` |
| `--radio-selftest` |
| `--shelter-hazard-loop-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnConditionChanged` | `Assets/Ashfall.Core/EquipmentConditionSystem.cs` |
| `OnConditionStarted` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnConditionStopped` | `Assets/Ashfall.Core/AudioConditionSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDeviceConditionChanged` | `Assets/Ashfall.Core/Radiation/DosimeterCalibrationSystem.cs` |
| `OnEventRaised` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/accessibility_profiles.json` |
| `Assets/StreamingAssets/Data/anomalies.json` |
| `Assets/StreamingAssets/Data/anomalous_expedition_encounters.json` |
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/breaching_equipment_catalog.json` |
| `Assets/StreamingAssets/Data/ceremonies.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/crossing_encounters.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **10** (177 files, 1394 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Combat` | 10 | 84 |
| `Crafting` | 1 | 11 |
| `Equipment` | 1 | 4 |
| `Events` | 1 | 6 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `Radio` | 47 | 354 |
| `Shelter` | 87 | 754 |

**Verdict:** 1394 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Combat`, `Crafting`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **115**
(27 of them panels/HUD).

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
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AshfallInputActions.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **34**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `combat` | no |
| `crafting` | no |
| `crossing` | no |
| `encounters` | no |
| `equipment` | no |
| `equipment_condition` | no |
| `events` | no |
| `expanded_shelter` | no |
| `expedition` | no |
| `expedition_stealth` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **14**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `combat` |
| `cupola_foundry` |
| `events` |
| `expedition` |
| `foundry` |
| `medical_microfluidic_diagnostics` |
| `metrology_calibration_drift` |
| `metrology_measurement_noise` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **149**
(CODEX_ONLY 85, GAMEPLAY_CONSUMED 42, OPTIONAL 3, UNRESOLVED 19).

| Catalog | Classification |
|---|---|
| `anomalous_expedition_encounters.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `ceremonies.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `crossing_encounters.json` | GAMEPLAY_CONSUMED |
| `crossing_factions.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 19 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_become_warlord` |
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** governance · **Coupling (incoming plans):** 258
**Surface:** save sections 34 (laddered 0) · RNG streams 14 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-BUILD-ERGONOMICS-56
wave: 6
status: PROPOSED — foreman claim required
packages: BE-56A, BE-56B, BE-56C, BE-56D, BE-56E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/accessibility_profiles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/anomalies.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --accessibility-selftest
dependencies:
  - coordinate: 258 other plan(s) name these artifacts (§12)
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
