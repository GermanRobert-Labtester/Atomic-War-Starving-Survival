# PLAN-SHELTER-CAPACITY-AUTHORITY-103 — Room Occupancy, Overcrowding & Sleep Quality

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SHELTER-ARCHITECTURE-40, PLAN-PANDEMIC-PUBLIC-HEALTH-47, PLAN-LIFECYCLE-SEALING-32.
**Non-goals:** no construction/cost work (Plan 40 owns it), no new room catalog,
no parallel bedding inventory.

## 1. Outcome
Shelter construction, excavation, and water are owned (Plan 40); disease
spread is owned (Plan 47). Between them sits **capacity**: how many occupants a
room hosts, what overcrowding does, who sleeps where, and how sleep quality
resolves. `Needs/SleepAcousticRestEngine.cs` is host-unreachable today (Plan 1
Appendix A) and `sanitation` already persists per-room state under the
`holdfast` lifecycle. The gap is a stated occupancy contract and its tests.

| Deliverable | Detail |
|---|---|
| Occupancy model | per-room capacity from the existing room data; occupant assignment rule; no local counter in panels |
| Overcrowding effects | documented consequences (rest quality, disease pressure handoff to Plan 47, morale mark) with owners per effect |
| Sleep resolution | sleep quality reads occupancy + acoustic state through `SleepAcousticRestEngine` once sealed; no wall-clock input |
| Save truth | occupancy restores with the room section; a moved survivor is not double-counted across rooms |
| Panel truth | the shelter panel shows occupancy from the authority, not a recomputed cache |

## 2. Evidence
- `SaveSectionRegistry.All`: `sanitation` under the `shelter` lifecycle; `holdfast` family sections.
- Plan 1 Appendix A: `SleepAcousticRestEngine` (Needs/) host-unreachable; Appendix G maps its candidate attach points.
- Plan 47 owns outbreak response; this plan only hands overcrowding pressure to it.
- Plan 40 owns build/upgrade; capacity is a property of built state, not a new system.

## 3. Packages
- **SCP-103A** occupancy model doc + room data capacity rows (re-verified).
- **SCP-103B** assignment rule implementation at the existing shelter seam + tests (fill, move, evict).
- **SCP-103C** overcrowding consequences: each effect named to its owner; measurable in a scripted night.
- **SCP-103D** sleep resolution with the acoustic engine (or explicit deferral note).
- **SCP-103E** save round-trip + panel-truth check (panel reads authority).

## 4. Acceptance & verification
- Scripted night: occupancy totals equal assigned survivors; zero double-counts after move.
- Overcrowding produces the documented effects exactly once per resolution.
- Save/load preserves occupancy; injecting a wall-clock skew leaves sleep output unchanged.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/`.

## 5. Risks
Capacity becoming a second authority beside built rooms → capacity is read from built state; the panel-truth check enforces it.

---

## 6. Expanded census (2 files · 393 lines)

Scope: `Assets/Ashfall.Core/Shelter/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Support 1 · System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SleepAcousticRestEngine.cs` | 230 | System | **yes** | 0 | 0 | 0 |
| `SleepNarrativeProjection.cs` | 163 | Support | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 0 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `acoustic_triangulation_catalog.json` | object[4 keys] |
| `hydrophone_acoustic_logs.json` | array[8] |

**State surfaces:** none.

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
Governed artifacts: 5. Other plans referencing them: **5**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-AUDIO-MIX-AUTHORITY-97` | 2 |
| `PLAN-UNBLOCK-03` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-DATA-AUTHORITY-14` | 1 |
| `PLAN-SIGNALS-REMOTE-SENSING-49` | 1 |

**Governed artifacts (first 12):**

| Artifact |
|---|
| `Needs/SleepAcousticRestEngine.cs` |
| `SleepAcousticRestEngine.cs` |
| `SleepNarrativeProjection.cs` |
| `acoustic_triangulation_catalog.json` |
| `hydrophone_acoustic_logs.json` |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `SCP-103A` | no name match — resolve at claim time |
| `SCP-103B` | no name match — resolve at claim time |
| `SCP-103C` | no name match — resolve at claim time |
| `SCP-103D` | `Needs/SleepAcousticRestEngine.cs`, `SleepAcousticRestEngine.cs`, `SleepNarrativeProjection.cs` |
| `SCP-103E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 13. Authority binding map

Symbols used: 4. Host files: **1** · Test files: **5** · Data files: **1**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.SleepNarrative.cs` |
| Tests (`Ashfall.Core.Tests/`) | 5 | `Ashfall.Core.Tests/AbyssalAnomaliesRuntimeActivationTests.cs`, `Ashfall.Core.Tests/Needs/Plan177SleepNarrativeProjectionTests.cs`, `Ashfall.Core.Tests/Needs/SleepAcousticRestEngineTests.cs`, `Ashfall.Core.Tests/Needs/SleepNarrativePhantomPainTests.cs`, `Ashfall.Core.Tests/PersonalLetterRuntimeActivationTests.cs` |
| Data (`StreamingAssets/Data/`) | 1 | `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **21** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `collectible_discovery` |
| `expanded_shelter` |
| `narrative` |
| `narrative_questlines` |
| `personal_quests` |
| `phantom_memory` |
| `procedural_narrative` |
| `shelter` |
| `shelter_assignment` |
| `shelter_atmosphere` |
| `shelter_barter` |
| `shelter_decor` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **18** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--narrative-selftest` |
| `--personal-quest-selftest` |
| `--personal-quests-selftest` |
| `--real-main-journey-selftest` |
| `--selftest-manifest` |
| `--shelter-actor-physics-selftest` |
| `--shelter-atmosphere-selftest` |
| `--shelter-decor-selftest` |
| `--shelter-hazard-loop-selftest` |
| `--shelter-hazard-selftest` |
| `--shelter-interior-selftest` |
| `--shelter-noise-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **11**.

| Event | First declaration |
|---|---|
| `OnBlightNarrative` | `Assets/Ashfall.Core/Farming/AgricultureSystem.cs` |
| `OnLungCapacityReduced` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnNarrativeMarker` | `Assets/Ashfall.Core/District8DeepCoastSystem.cs` |
| `OnNarrativeRequested` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnPhantomKnock` | `Assets/Ashfall.Core/Medical/VigilStateMachine.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnStageNarrativeEmitted` | `Assets/Ashfall.Core/Crossing/CrossingQuestSystem.cs` |
| `OnTriangulationCompleted` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |
| `OnTriangulationFailed` | `Assets/Ashfall.Core/Radio/SignalTriangulationSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json` |
| `Assets/StreamingAssets/Data/narrative/hydrophone_acoustic_logs.json` |
| `Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json` |
| `Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json` |
| `Assets/StreamingAssets/Data/narrative_arc_events.json` |
| `Assets/StreamingAssets/Data/narrative_discovery_manifest.json` |
| `Assets/StreamingAssets/Data/narrative_encounters.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` |
| `Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json` |
| `Assets/StreamingAssets/Data/narrative_progression.json` |
| `Assets/StreamingAssets/Data/narrative_questlines.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (114 files, 1067 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Narrative` | 26 | 293 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |

**Verdict:** 1067 cases sit under matching regions — run those first (`Narrative`, `NarrativeConsequence`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **60**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Host/NarrativeArcConsequenceAdapter.cs` |
| `src/Host/NarrativeContinuitySelfTest.cs` |
| `src/Host/NarrativeHostSession.cs` |
| `src/Host/NarrativeQuestlineHostSession.cs` |
| `src/Host/NarrativeQuestlineSaveStore.cs` |
| `src/Host/NarrativeSaveStore.cs` |
| `src/Host/ProceduralNarrativeHostSession.cs` |
| `src/Host/ProceduralNarrativeSaveStore.cs` |
| `src/Host/ShelterAssignmentHostSession.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **18**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `expanded_shelter` | no |
| `narrative` | no |
| `narrative_questlines` | no |
| `procedural_narrative` | no |
| `shelter` | no |
| `shelter_assignment` | no |
| `shelter_atmosphere` | no |
| `shelter_barter` | no |
| `shelter_decor` | no |
| `shelter_fire` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **3**.

| Stream |
|---|
| `acoustic_detection` |
| `narrative` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **291**
(CODEX_ONLY 279, GAMEPLAY_CONSUMED 5, OPTIONAL 2, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `narrative/activated_carbon_adsorption_records.json` | CODEX_ONLY |
| `narrative/ammo_hoist_jam_reports.json` | CODEX_ONLY |
| `narrative/ammonia_chiller_leak_logs.json` | CODEX_ONLY |
| `narrative/annealing_lehr_birefringence_records.json` | CODEX_ONLY |
| `narrative/antler_horn_sawing_records.json` | CODEX_ONLY |
| `narrative/apiculture_red_light_audits.json` | CODEX_ONLY |
| `narrative/aramid_fiber_rot_reports.json` | CODEX_ONLY |
| `narrative/architect_vault_audits.json` | CODEX_ONLY |
| `narrative/armored_cockroach_hive_logs.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 18 (laddered 0) · RNG streams 3 · host files 15 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-SHELTER-CAPACITY-AUTHORITY-103
wave: 9
status: PROPOSED — foreman claim required
packages: SCP-103A, SCP-103B, SCP-103C, SCP-103D, SCP-103E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Host/NarrativeArcConsequenceAdapter.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/narrative/hydrophone_acoustic_logs.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Narrative/
  - godot --headless --path . -- --narrative-selftest
dependencies:
  - coordinate: 5 other plan(s) name these artifacts (§12)
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
