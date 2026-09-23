# PLAN-SOLAR-CONCENTRATOR-TRUTH-217 — Mirrors, Heat Storage & Intermittency

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ENERGY-NUCLEAR-48, PLAN-WEATHER-ATMOSPHERE-28, PLAN-MAINTENANCE-DECAY-TRUTH-119.
**Non-goals:** no grid model (Plan 48), no weather generation (Plan 28), no new
mirror catalog.

## 1. Outcome
`Shelter/SolarConcentratorEngine.cs` (**360 lines**) is reachable and
unaddressed: concentrated solar heat — the one generator whose output is
**weather-bound** and whose heat must be stored to matter. Nothing states the
intermittency curve, storage losses, or mirror condition, so it is either
perfect or useless.

| Deliverable | Detail |
|---|---|
| Output model | output derives from plan 28's sky/weather state and day phase; a documented curve, not a constant |
| Heat storage | a thermal store with capacity and loss per hour; without storage, output only matches production |
| Mirror/condition | mirror cleanliness/condition (Plan 119 contract) reduces output; cleaning consumes water/labor |
| Grid interface | output enters Plan 48's balance; intermittency is visible in the grid view |
| Save truth | store temperature and mirror state restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/SolarConcentratorEngine.cs` (360 lines; unaddressed — Wave 16 audit).
- Plan 28 owns the sky state that drives output.
- Plan 48 owns grid balance; Plan 119 supplies condition decay.
- Plan 46's water is a cleaning input.

## 3. Packages
- **SCT-217A** output curve + weather fixtures.
- **SCT-217B** thermal storage/loss tests.
- **SCT-217C** mirror condition/cleaning path (water consumed).
- **SCT-217D** grid integration + intermittency visibility test.
- **SCT-217E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Output follows the curve for scripted weather; storage shifts availability, not totals beyond loss.
- Cleaning restores output and consumes water; save/load preserves store and mirrors.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Weather-proof generator → the curve is a fixture.
Invisible intermittency → grid view shows the daily pattern.

---

## 6. Expanded census (1 files · 360 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SolarConcentratorEngine.cs` | 360 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `solar_concentrator_catalog.json` | object[2 keys] |

**State surfaces:** `SolarConcentratorEngine.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
| Test references | 2 name references across the test tree |
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

Domain files: 1. Other plans referencing their names: **0**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| — | no other plan references these files |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SCT-217A` | no name match — resolve at claim time |
| `SCT-217B` | no name match — resolve at claim time |
| `SCT-217C` | no name match — resolve at claim time |
| `SCT-217D` | no name match — resolve at claim time |
| `SCT-217E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **2** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 2 | `src/Host/SolarConcentratorHostSession.cs`, `src/Main.Plans110_113.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/IndustrialCapabilityExpansionTests.cs`, `Ashfall.Core.Tests/Shelter/PowerGridPhase2GenerationPortfolioTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `power_grid` |
| `power_subgrids` |
| `sofc_power` |
| `solar_concentrator` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--advanced-industrial-recon-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--real-main-journey-selftest` |
| `--sofc-power-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **2**.

| Event | First declaration |
|---|---|
| `OnPhaseChanged` | `Assets/Ashfall.Core/ShelterScheduleSystem.cs` |
| `OnPhaseTransitioned` | `Assets/Ashfall.Core/YearOfAsh/YearOfAshTimelineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/narrative/education_session_records.json` |
| `Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json` |
| `Assets/StreamingAssets/Data/narrative/power_grid_management_logs.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json` |
| `Assets/StreamingAssets/Data/power_grid.json` |
| `Assets/StreamingAssets/Data/power_subgrid_nodes.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |
| `Assets/StreamingAssets/Data/shelter_celebrations.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (177 files, 1400 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Foundry` | 8 | 73 |
| `Holdfast` | 1 | 13 |
| `Shelter` | 87 | 754 |

**Verdict:** 1400 cases sit under matching regions — run those first (`Audio`, `Balance`, `Campaign`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **328**
(21 of them panels/HUD).

| Host file |
|---|
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
| `src/Host/AutopsyHostSession.cs` |
| `src/Host/BallisticShieldHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **39**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `amphibious_draisine` | no |
| `apprenticeship` | no |
| `archive_desk` | no |
| `autopsy` | no |
| `ballistic_shield` | no |
| `black_projects_archive` | no |
| `disease` | no |
| `draisine_recovery` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **11**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `amphibious_draisine` |
| `aquaponics_disease` |
| `cupola_foundry` |
| `disease` |
| `foundry` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **137**
(CODEX_ONLY 107, GAMEPLAY_CONSUMED 19, OPTIONAL 1, UNRESOLVED 10).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `archive_inks.json` | GAMEPLAY_CONSUMED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `autopsy_procedures.json` | GAMEPLAY_CONSUMED |
| `ballistic_shield_catalog.json` | UNRESOLVED |
| `cultural_archive_tomes.json` | UNRESOLVED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `foundry_accords.json` | GAMEPLAY_CONSUMED |

**Verdict:** 10 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_preserved_archive` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** free-start · **Coupling (incoming plans):** 0
**Surface:** save sections 39 (laddered 1) · RNG streams 11 · host files 23 · catalogs 22 · test regions 8 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SOLAR-CONCENTRATOR-TRUTH-217
wave: 16
status: PROPOSED — foreman claim required
packages: SCT-217A, SCT-217B, SCT-217C, SCT-217D, SCT-217E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Disease/DiseaseHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/narrative/education_session_records.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --advanced-industrial-recon-selftest
dependencies:
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 1 versioned save ladder(s) — extend, never fork
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
