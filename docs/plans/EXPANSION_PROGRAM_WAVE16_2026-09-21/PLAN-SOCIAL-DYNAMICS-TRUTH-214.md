# PLAN-SOCIAL-DYNAMICS-TRUTH-214 — Cliques, Standing & Social Structure

**Wave 16 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MORALE-UNREST-TRUTH-129, PLAN-RELATIONSHIP-DECAY-TRUTH-195, PLAN-LEADERSHIP-TRUTH-173, PLAN-SHELTER-POLITICS-69.
**Non-goals:** no mood model (Plan 64/129), no affinity drift (Plan 195), no
authority/succession (Plan 173).

## 1. Outcome
`Shelter/ShelterSocialDynamicsSystem.cs` (**374 lines**) is reachable and
unaddressed: the **structure** of shelter society — groups, standing within
them, and how structure affects decisions. Individual relations (Plan 43/195),
collective mood (Plan 129), authority (Plan 173), and politics (Plan 69) all
exist; the middle layer — who clusters with whom and what that means — is
unowned.

| Deliverable | Detail |
|---|---|
| Group detection | groups from documented signals (shared duty, household, history) — derived, never hand-assigned |
| Standing model | per-survivor standing within groups from existing facts; no parallel reputation number outside Plan 29/141 owners |
| Effect routing | group structure feeds Plan 129 marks and Plan 173 legitimacy as typed inputs |
| Stability rule | groups form/dissolve on documented thresholds with hysteresis; no oscillation |
| Save truth | group structure and standings restore; a load never re-clusters differently |

## 2. Evidence
- `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` (374 lines; unaddressed — Wave 16 audit).
- Plan 195's affinity pairs and Plan 129's marks are the adjacent layers.
- Plan 29/141 own reputation/offices the standing reads.
- Plan 69 presents collective outcomes.

## 3. Packages
- **SDT-214A** group detection + signal table.
- **SDT-214B** standing derivation (no parallel number proof).
- **SDT-214C** effect routing to Plans 129/173.
- **SDT-214D** stability/hysteresis fixtures.
- **SDT-214E** save round-trip; no re-clustering on load.

## 4. Acceptance & verification
- Groups derive from declared signals only; standings trace to owners.
- Structure changes with hysteresis; save/load preserves it exactly.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Hidden social graph → signals are listed and tested.
Oscillation → hysteresis fixture is the guard.

---

## 6. Expanded census (1 files · 374 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ShelterSocialDynamicsSystem.cs` | 374 | System | **yes** | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `shelter_social_events.json` | object[2 keys] |
| `radiation_economy_social.json` | object[4 keys] |

**State surfaces:** `ShelterSocialDynamicsSystem.cs`.

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

Domain files: 1. Other plans referencing their names: **1**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MORTUARY-MEMORIAL-TRUTH-123` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `SDT-214A` | no name match — resolve at claim time |
| `SDT-214B` | no name match — resolve at claim time |
| `SDT-214C` | no name match — resolve at claim time |
| `SDT-214D` | no name match — resolve at claim time |
| `SDT-214E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 3. Host files: **5** · Test files: **5** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 5 | `src/Audio/AudioSelfTest.cs`, `src/Audio/ShelterOperationsAudioBridge.cs`, `src/Main.Audio.cs`, `src/Main.Plans46_49.cs`, `src/UI/ShelterSocialPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/Balance/ShelterOperationsBalanceSim.cs`, `Ashfall.Core.Tests/Integration/FullCampaign30DayShelterPlaythroughTests.cs`, `Ashfall.Core.Tests/Integration/Plans46_49_CrossSystemIntegrationTests.cs`, `Ashfall.Core.Tests/Radiation/Plan146RadiationBridgeIntegrationTests.cs`, `Ashfall.Core.Tests/Shelter/ShelterSocialDynamicsTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **23** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `campaign` |
| `campaign_day` |
| `economy` |
| `events` |
| `expanded_shelter` |
| `radiation` |
| `seismic_dynamics` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **32** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--audio-selftest` |
| `--audio-test` |
| `--campaign-journey-selftest` |
| `--deep-coast-playthrough` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--operations-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnRadiationDoseResetRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnRadiationExposure` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/audio_accessibility_cues.json` |
| `Assets/StreamingAssets/Data/audio_cues.json` |
| `Assets/StreamingAssets/Data/audio_logs_expansion_05.json` |
| `Assets/StreamingAssets/Data/campaign_epilogues.json` |
| `Assets/StreamingAssets/Data/combat_catalog.json` |
| `Assets/StreamingAssets/Data/contagion_events.json` |
| `Assets/StreamingAssets/Data/desperation_events.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/espionage_operations.json` |
| `Assets/StreamingAssets/Data/events.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **14** (223 files, 1686 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Balance` | 1 | 5 |
| `Campaign` | 32 | 187 |
| `Combat` | 10 | 84 |
| `Economy` | 41 | 329 |
| `Espionage` | 1 | 4 |
| `Events` | 1 | 6 |
| `Integration` | 16 | 74 |
| `Progression` | 11 | 83 |

**Verdict:** 1686 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Balance`, `Campaign`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **471**
(234 of them panels/HUD).

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
| `src/Economy/EconomyMarketPanel.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **35**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `campaign` | no |
| `campaign_day` | no |
| `combat` | no |
| `desperation` | no |
| `economy` | no |
| `encounter_choice` | no |
| `equipment_condition` | no |
| `espionage` | no |
| `events` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `acoustic_detection` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `combat` |
| `economy` |
| `events` |
| `shelter` |
| `social` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **109**
(CODEX_ONLY 71, GAMEPLAY_CONSUMED 23, OPTIONAL 2, UNRESOLVED 13).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `combat_catalog.json` | GAMEPLAY_CONSUMED |
| `contagion_events.json` | UNRESOLVED |
| `desperation_events.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `events.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |

**Verdict:** 13 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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
**Surface:** save sections 35 (laddered 0) · RNG streams 9 · host files 23 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SOCIAL-DYNAMICS-TRUTH-214
wave: 16
status: PROPOSED — foreman claim required
packages: SDT-214A, SDT-214B, SDT-214C, SDT-214D, SDT-214E
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
