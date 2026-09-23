# PLAN-SESSION-DURABILITY-111 — Crash Recovery, Autosave Cadence & Interrupted Day

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SAVE-GOVERNANCE-12, PLAN-RUNTIME-RESILIENCE-57, PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98.
**Non-goals:** no second save format, no cloud backup, no always-on journaling;
durability rides the existing slot store.

## 1. Outcome
`Save/SessionDurabilityManager.cs` is host-unreachable today (Plan 1 Appendix
A) — the exact component whose job this is. Runtime resilience (Plan 57) and
fuzz operations (Plan 98) cover failure handling and recovery policy, but
**interrupted sessions** (crash, kill, power loss mid-day) have no stated
contract: how much progress is guaranteed, what the next launch offers, and how
an interrupted day is resumed or discarded.

| Deliverable | Detail |
|---|---|
| Durability contract | guaranteed progress point (e.g. last completed day boundary or autosave interval), stated in days not minutes |
| Autosave cadence | interval tied to the canonical day/hour; rotation bounded; visible per Plan 105 |
| Interrupted-launch offer | resume vs rollback choice with the exact day/state described before acting |
| Typed interruption mark | a partial write is detected and never loaded as valid (checksum + completeness marker) |
| Crash-simulation tests | kill between write steps; next launch reports the correct state |

## 2. Evidence
- Plan 1 Appendix A: `SessionDurabilityManager` host-unreachable; Appendix H sizes it; Appendix G lists candidate attach points.
- `Save/SaveEnvelopeHelper.cs` checksum verify + `SaveStore` slot ownership provide the primitives.
- Plan 98 defines recovery behavior for corrupt artifacts; this plan covers interrupted writes specifically.
- Plan 105 renders the visibility; this plan owns the semantics.

## 3. Packages
- **SDU-111A** durability contract doc (progress point, cadence, rotation).
- **SDU-111B** completeness marker + detection; a torn write fails typed, never half-loads.
- **SDU-111C** autosave cadence at the day/hour owner (no frame timing).
- **SDU-111D** interrupted-launch surface (resume/rollback) with exact state text.
- **SDU-111E** crash-simulation tests at each write step.

## 4. Acceptance & verification
- Kill-injection at each write step: next launch never loads a torn file; offered state matches the contract.
- Cadence measured in game days; OS clock change does not alter it.
- Rotation bound asserted; orphaned temp files are cleaned or listed.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/` (focused durability region).

## 5. Risks
Durability becoming a second save path → the manager orchestrates the existing store; no new codec.
Over-frequent autosave → cadence is day/hour based, measured and bounded.

---

## 6. Expanded census (1 files · 365 lines)

Scope: `Assets/Ashfall.Core/Save/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SessionDurabilityManager.cs` | 365 | System | **yes** | 2 | 0 | 2 |

**Totals:** 2 banned refs · 0 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `education_session_records.json` | array[20] |
| `therapist_session_notes.json` | array[20] |
| `therapist_session_notes_batch_2.json` | array[20] |
| `therapist_session_notes_batch_3.json` | object[3 keys] |

**State surfaces:** `SessionDurabilityManager.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Save/` |
| Test references | 2 name references across the test tree |
| Determinism | 2 banned refs to fix or justify |
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
Governed artifacts: 7. Other plans referencing them: **10**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-SAVE-MIGRATION-CORRIDOR-87` | 2 |
| `EVIDENCE` | 1 |
| `PLAN-SCIENCE-EDUCATION-38` | 1 |
| `PLAN-RUNTIME-RESILIENCE-57` | 1 |
| `PLAN-THREADING-ASYNCHRONY-72` | 1 |
| `PLAN-DETERMINISM-CROSS-HOST-89` | 1 |
| `PLAN-SAVE-INTEGRITY-FUZZ-OPERATIONS-98` | 1 |
| `PLAN-CAMPAIGN-PORTABILITY-104` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Save/SaveEnvelopeHelper.cs` |
| `Save/SessionDurabilityManager.cs` |
| `SessionDurabilityManager.cs` |
| `education_session_records.json` |
| `therapist_session_notes.json` |
| `therapist_session_notes_batch_2.json` |
| `therapist_session_notes_batch_3.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SDU-111A` | `Save/SessionDurabilityManager.cs`, `SessionDurabilityManager.cs` |
| `SDU-111B` | no name match — resolve at claim time |
| `SDU-111C` | no name match — resolve at claim time |
| `SDU-111D` | no name match — resolve at claim time |
| `SDU-111E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 6. Host files: **3** · Test files: **12** · Data files: **4**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 3 | `src/Host/HostCli.PanelTests.cs`, `src/Host/SaveStoreChecksumSelfTest.cs`, `src/Host/SaveStoreHub.cs` |
| Tests (`Ashfall.Core.Tests/`) | 12 | `Ashfall.Core.Tests/CollectibleDiscoveryPersistenceTests.cs`, `Ashfall.Core.Tests/CollectibleDiscoveryStateTests.cs`, `Ashfall.Core.Tests/Host/HostSessionStateSemanticsTests.cs`, `Ashfall.Core.Tests/Journeys/FireIncidentJourneyTests.cs`, `Ashfall.Core.Tests/Journeys/MoralChoiceJourneyTests.cs` |
| Data (`StreamingAssets/Data/`) | 4 | `Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json`, `Assets/StreamingAssets/Data/narrative/council_meeting_minutes.json`, `Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json`, `Assets/StreamingAssets/Data/store_capability_claims.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **5** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `encounter_choice` |
| `moral_choice` |
| `shelter_fire` |
| `unique_claims` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--campaign-journey-selftest` |
| `--checksum-sweep-selftest` |
| `--expedition-panel-lifecycle` |
| `--expedition-panel-uitest` |
| `--journal-weather-panel-selftest` |
| `--moral-choice-selftest` |
| `--onboarding-journey-selftest` |
| `--panel-bind-lifecycle-selftest` |
| `--panel-bind-selftest` |
| `--panel-lifecycle-selftest` |
| `--real-campaign-journey-selftest` |
| `--real-main-journey-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `MoralChoiceSystem` | `Assets/Ashfall.Core/MoralChoice/MoralChoiceIds.cs` |
| `OnBatchCompleted` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnConflictResolved` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnConflictStarted` | `Assets/Ashfall.Core/SurvivorRelationsSystem.cs` |
| `OnExtractionBatchProduced` | `Assets/Ashfall.Core/Foundry/SaltMineExtractionSystem.cs` |
| `OnFireIgnited` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnIncident` | `Assets/Ashfall.Core/ShelterThermalSystem.cs` |
| `OnIncidentResolved` | `Assets/Ashfall.Core/AirlockSecuritySystem.cs` |
| `OnIncidentSuppressed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnMoralChronicleEntry` | `Assets/Ashfall.Core/Maritime/PsychologicalContaminationSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/discovery_consequences.json` |
| `Assets/StreamingAssets/Data/education_curriculum.json` |
| `Assets/StreamingAssets/Data/excavation_hazard_mitigation.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/mine_flail_catalog.json` |
| `Assets/StreamingAssets/Data/moral_choice_chains.json` |
| `Assets/StreamingAssets/Data/moral_choice_faction_reactions.json` |
| `Assets/StreamingAssets/Data/moral_choice_flags.json` |
| `Assets/StreamingAssets/Data/moral_choice_gossip.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **11** (168 files, 1343 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Accessibility` | 1 | 6 |
| `Audio` | 5 | 28 |
| `Codex` | 2 | 29 |
| `Economy` | 41 | 329 |
| `Education` | 2 | 11 |
| `Excavation` | 1 | 5 |
| `Foundry` | 8 | 73 |
| `Integration` | 16 | 74 |
| `MoralChoice` | 4 | 33 |
| `PlayerCommand` | 1 | 1 |

**Verdict:** 1343 cases sit under matching regions — run those first (`Accessibility`, `Audio`, `Codex`, `Economy`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **625**
(229 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioSelfTest.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Disease/DiseaseHostSession.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Foundry/SilentFoundryHostSession.cs` |
| `src/Host/AgricultureHostSession.cs` |
| `src/Host/AgricultureSaveStore.cs` |
| `src/Host/AirlockSecurityHostSession.cs` |
| `src/Host/AirlockSecuritySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **42**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `agriculture` | no |
| `airlock_security` | no |
| `anomaly_hazard` | no |
| `black_market` | no |
| `caravan_trade_network` | no |
| `collectible_discovery` | no |
| `deep_well` | no |
| `disease` | no |
| `economy` | no |
| `encounter_choice` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **18**.

| Stream |
|---|
| `acoustic_detection` |
| `agriculture_blight` |
| `agriculture_mutation` |
| `agriculture_pest` |
| `anomaly_hazard` |
| `aquaponics_disease` |
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `breach_operator_incident` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **126**
(CODEX_ONLY 64, GAMEPLAY_CONSUMED 38, OPTIONAL 9, UNRESOLVED 15).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `cupola_foundry_catalog.json` | UNRESOLVED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `disease_catalog.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |

**Verdict:** 15 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_chosen_faction_side` |
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 42 (laddered 0) · RNG streams 18 · host files 25 · catalogs 22 · test regions 10 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SESSION-DURABILITY-111
wave: 9
status: PROPOSED — foreman claim required
packages: SDU-111A, SDU-111B, SDU-111C, SDU-111D, SDU-111E
claim paths:
  - src/Audio/AudioSelfTest.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/conflict_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/
  - godot --headless --path . -- --campaign-journey-selftest
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
