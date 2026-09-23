# PLAN-SEISMIC-DYNAMICS-TRUTH-193 — Tremors, Structural Stress & Preparedness

**Wave 15 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-DEEP-STRATA-83, PLAN-SHELTER-ARCHITECTURE-40, PLAN-CRISIS-DISASTER-RESPONSE-80.
**Implementation scaffold:** [`PLAN-SEISMIC-DYNAMICS-TRUTH-193_APPENDIX-A_SCAFFOLD.md`](PLAN-SEISMIC-DYNAMICS-TRUTH-193_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-CASCADE-COORDINATOR-TRUTH-249` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no geology generation (Plan 83), no build costs (Plan 40), no
disaster protocols (Plan 80).

## 1. Outcome
`Shelter/SeismicDynamicsSystem.cs` (**457 lines**) is reachable and unaddressed:
tremors that stress structures. Deep digs (Plan 83), construction (Plan 40), and
crisis response (Plan 80) all assume structures can be damaged by events; the
seismic system is the event source and the stress model, and neither is stated.

| Deliverable | Detail |
|---|---|
| Event model | tremor events with magnitude and origin (natural, deep works, collapse) on a seeded schedule |
| Stress model | structural stress from magnitude + construction quality (Plan 40) + depth (Plan 83); damage is visible per structure |
| Preparedness | documented mitigations (bracing, inspection) reduce damage; inspection discovers latent stress |
| Response routing | injuries/damage route to Plan 124/Plan 40 owners; response protocols stay in Plan 80 |
| Save truth | pending schedule and accumulated stress restore; a load never jumps or skips a tremor |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/SeismicDynamicsSystem.cs` (457 lines; unaddressed — Wave 13/15 audit).
- Plan 83 owns depth/strata context that modulates events.
- Plan 40 owns construction quality the stress model reads.
- Plan 80 owns response procedure; this plan supplies the event.

## 3. Packages
- **SDY-193A** event model + seeded schedule test.
- **SDY-193B** stress model + per-quality damage fixtures.
- **SDY-193C** mitigation/inspection tests (discover latent stress).
- **SDY-193D** response routing to Plan 40/124 owners.
- **SDY-193E** save round-trip of schedule and stress.

## 4. Acceptance & verification
- Same seed + same structures → same tremor/damage sequence.
- Mitigation reduces damage measurably; inspection reveals hidden stress.
- Save/load mid-schedule does not jump events.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Punitive quakes → mitigation and inspection are the player's counterplay, both tested.
Overlap with 80 → this plan is the hazard; response stays there.

---

## 6. Expanded census (2 files · 724 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SeismicDynamicsSystem.Monitoring.cs` | 267 | Support | — | 0 | 0 | 0 |
| `SeismicDynamicsSystem.cs` | 457 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `seismic_fault_catalog.json` | object[2 keys] |
| `seismic_array_fault_alarms.json` | array[8] |

**State surfaces:** `SeismicDynamicsSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
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
| `PLAN-ENERGY-NUCLEAR-48` | 2 |
| `PLAN-SHELTER-FAMILY-TRUTH-265` | 2 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SDY-193A` | no name match — resolve at claim time |
| `SDY-193B` | no name match — resolve at claim time |
| `SDY-193C` | no name match — resolve at claim time |
| `SDY-193D` | no name match — resolve at claim time |
| `SDY-193E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 5. Host files: **1** · Test files: **4** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.PlansB68_B69.cs` |
| Tests (`Ashfall.Core.Tests/`) | 4 | `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/Integration/PlansB66ToB69CrossSystemTests.cs`, `Ashfall.Core.Tests/Shelter/SeismicMonitoringB68Tests.cs`, `Ashfall.Core.Tests/Shelter/ShelterSeismicDynamicsPlan56Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **19** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `comms_array` |
| `expanded_shelter` |
| `seismic_dynamics` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |
| `shelter_fire` |
| `shelter_noise` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **19** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **3**.

| Event | First declaration |
|---|---|
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/narrative/gear_quenching_fault_logs.json` |
| `Assets/StreamingAssets/Data/narrative/seismic_array_fault_alarms.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/sonar_array_fault_logs.json` |
| `Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json` |
| `Assets/StreamingAssets/Data/psychological_trauma.json` |
| `Assets/StreamingAssets/Data/seismic_fault_catalog.json` |
| `Assets/StreamingAssets/Data/shelter_audio_cues.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (167 files, 1294 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `Flagship11` | 7 | 63 |
| `Integration` | 16 | 74 |
| `MoralChoice` | 4 | 33 |
| `PlayerCommand` | 1 | 1 |
| `Shelter` | 87 | 754 |
| `WildlifeTrapping` | 5 | 70 |

**Verdict:** 1294 cases sit under matching regions — run those first (`Audio`, `Campaign`, `Combat`, `Flagship11`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **214**
(25 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/CampaignDayPersistenceAdapter.cs` |
| `src/Host/CampaignDaySaveStore.cs` |
| `src/Host/CombatHostSession.cs` |
| `src/Host/CombatSaveStore.cs` |
| `src/Host/CommsArraySaveStore.cs` |
| `src/Host/EncounterChoiceSaveStore.cs` |
| `src/Host/HostCli.Plans122to125.cs` |
| `src/Host/HostCli.Plans139_141.cs` |
| `src/Host/HostCli.Plans162_165.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `campaign` | no |
| `campaign_day` | no |
| `combat` | no |
| `comms_array` | no |
| `encounter_choice` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `field_guide` | no |
| `moral_choice` | no |
| `psychological_arcs` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **8**.

| Stream |
|---|
| `acoustic_detection` |
| `combat` |
| `moral_choice` |
| `shelter` |
| `wildlife_apex` |
| `wildlife_migration` |
| `wildlife_population` |
| `wildlife_taming` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **117**
(CODEX_ONLY 76, GAMEPLAY_CONSUMED 27, OPTIONAL 2, UNRESOLVED 12).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `camouflage_gear.json` | GAMEPLAY_CONSUMED |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `comms_targets.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |

**Verdict:** 12 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 2
**Surface:** save sections 28 (laddered 0) · RNG streams 8 · host files 22 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SEISMIC-DYNAMICS-TRUTH-193
wave: 15
status: PROPOSED — foreman claim required
packages: SDY-193A, SDY-193B, SDY-193C, SDY-193D, SDY-193E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/CampaignDayPersistenceAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/campaign_epilogues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/combat_catalog.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --campaign-journey-selftest
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
