# PLAN-WORKSHOP-TRUTH-175 — Bench Capacity, Tooling & Work-Order Queue

**Wave 13 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-INDUSTRY-AUTOMATION-45, PLAN-CRAFT-QUALITY-TRUTH-112, PLAN-RECIPE-REACHABILITY-TRUTH-125, PLAN-DUTY-ROSTER-TRUTH-101.
**Implementation scaffold:** [`PLAN-WORKSHOP-TRUTH-175_APPENDIX-A_SCAFFOLD.md`](PLAN-WORKSHOP-TRUTH-175_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-INDUSTRY-AUTOMATION-45` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no recipe content (Plan 125), no quality tiers (Plan 112), no
factory engines (Plan 45).

## 1. Outcome
`Shelter/ShelterWorkshopSystem.cs` (634 lines) is reachable and unaddressed: the
bench-level production surface between raw industry (Plan 45) and personal
crafting. Its contract is capacity and queueing: how many jobs a workshop runs,
which tools are required, how work orders wait, and how a worker's shift (Plan
101) gates throughput.

| Deliverable | Detail |
|---|---|
| Bench model | benches with tool requirements and a job capacity from built state |
| Work-order queue | queued → active → done/cancelled with a documented priority rule and a visible queue |
| Throughput coupling | an active bench requires a worker's shift time (Plan 101); no bench runs unaided unless it has an assigned automation |
| Output truth | outputs enter inventory via Plan 93; quality comes from Plan 112; recipes from Plan 125 |
| Save truth | queue and bench state restore; a load never re-orders or re-produces |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` (634 lines; unmentioned in every plan body — Wave 13 audit).
- Plan 101 supplies shift time; Plan 45 owns industrial engines.
- Plan 125 guarantees recipes this queue executes are reachable; Plan 112 grades outputs.
- Plan 93 verifies input consumption and output creation.

## 3. Packages
- **WKS-175A** bench model + tool requirement table.
- **WKS-175B** queue lifecycle + priority rule tests.
- **WKS-175C** throughput coupling to Plan 101 (no unaided bench without automation).
- **WKS-175D** conservation + quality pass-through tests.
- **WKS-175E** save round-trip; no re-order/re-produce on load.

## 4. Acceptance & verification
- Queue transitions match the lifecycle; priority is deterministic under equal inputs.
- A bench without a worker or automation does not run (fixture).
- Inputs/outputs balance; quality equals Plan 112's result for the job.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Queue as authority → it is a worklist over owners; the conservation test enforces it.
Idle automation ambiguity → the automation rule is explicit and tested.

---

## 6. Expanded census (1 files · 634 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ShelterWorkshopSystem.cs` | 634 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `workshop_recipes.json` | object[2 keys] |

**State surfaces:** `ShelterWorkshopSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Shelter/` |
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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ENERGY-NUCLEAR-48` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `WKS-175A` | no name match — resolve at claim time |
| `WKS-175B` | no name match — resolve at claim time |
| `WKS-175C` | no name match — resolve at claim time |
| `WKS-175D` | no name match — resolve at claim time |
| `WKS-175E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **6** · Test files: **6** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 6 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Host/HostCli.PlansB86_B89.cs`, `src/Main.Audio.cs`, `src/Main.Plans46_49.cs` |
| Tests (`Ashfall.Core.Tests/`) | 6 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/BioFermentationEngineTests.cs`, `Ashfall.Core.Tests/Shelter/PrecisionMetrologySystemTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **21** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `bio_fermentation` |
| `campaign` |
| `campaign_day` |
| `expanded_shelter` |
| `low_background_metrology` |
| `precision_metrology` |
| `precision_optics` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **24** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--deep-coast-playthrough` |
| `--operations-selftest` |
| `--plans-122-125-balance-soak` |
| `--plans-122-125-selftest` |
| `--plans-139-141-selftest` |
| `--precision-metrology-selftest` |
| `--propaganda-campaign-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **4**.

| Event | First declaration |
|---|---|
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnWorkshopStateChanged` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/bio_fermentation_catalog.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/glassworks_recipes.json` |
| `Assets/StreamingAssets/Data/metallurgy_recipes.json` |
| `Assets/StreamingAssets/Data/metrology_standards_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/engineering_logs_expansion.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **9** (155 files, 1153 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `Espionage` | 1 | 4 |
| `Integration` | 16 | 74 |
| `Shelter` | 87 | 754 |
| `Telemetry` | 2 | 11 |

**Verdict:** 1153 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **282**
(30 of them panels/HUD).

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
| `src/Host/BioFermentationHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **28**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `bio_fermentation` | no |
| `campaign` | no |
| `campaign_day` | no |
| `combat` | no |
| `encounter_choice` | no |
| `equipment_condition` | no |
| `espionage` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `host_event` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `acoustic_detection` |
| `black_market_debt_event` |
| `combat` |
| `low_background_metrology` |
| `metrology_calibration_drift` |
| `metrology_measurement_noise` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **105**
(CODEX_ONLY 72, GAMEPLAY_CONSUMED 21, OPTIONAL 1, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 1
**Surface:** save sections 28 (laddered 0) · RNG streams 9 · host files 23 · catalogs 22 · test regions 9 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-WORKSHOP-TRUTH-175
wave: 13
status: PROPOSED — foreman claim required
packages: WKS-175A, WKS-175B, WKS-175C, WKS-175D, WKS-175E
claim paths:
  - src/Audio/AudioConditionHostBridge.cs  # §19 candidate host surface
  - src/Audio/AudioCueCatalog.cs  # §19 candidate host surface
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioManager.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/audio_accessibility_cues.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/audio_cues.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --audio-selftest
dependencies:
  - coordinate: 1 other plan(s) name these artifacts (§12)
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
