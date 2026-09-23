# PLAN-DAILY-ROUTINE-AUTHORITY-107 — Routine Schedule Semantics, Interruption & Resume

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TEMPORAL-AUTHORITY-33, PLAN-LIFECYCLE-SEALING-32, PLAN-SAVE-GOVERNANCE-12.
**Non-goals:** no second scheduler, no new clock, no panel-side routine state;
this plan defines semantics over the existing day owner.

## 1. Outcome
`Survivors/SurvivorRoutineSystem.cs` (host-unreachable, Plan 1 Appendix A)
models daily routines; the day/hour clock is owned by the campaign day
coordinator (Plan 33). What has never been stated is what happens when a
routine is **interrupted**: a crisis, an injury, an assigned shift (Plan 101),
or a save in the middle of a routine block. Without that, routines are either
ignored or resumed incorrectly after load.

| Deliverable | Detail |
|---|---|
| Schedule model | routine blocks keyed to the canonical hour; priority order vs duty roster posts |
| Interruption rules | each interrupt source (crisis, injury, muster, draft) pauses or cancels the current block with a documented rule |
| Resume semantics | mid-block save/load restores the block, remaining duration, and next-block pointer |
| Starvation of needs | an uninterrupted routine never starves a need below its floor; a test proves priority over preference |
| No phantom routine | a survivor unavailable (hospital, grave) has no scheduled block and is never counted as coverage |

## 2. Evidence
- Plan 1 Appendix A: `SurvivorRoutineSystem` host-unreachable; Appendix G lists its candidate attachment points.
- `DutyRoster/DutyHourLedger.cs` and Plan 101 define shift coverage that routines must not contradict.
- Plan 33 Appendix A: hour-consumer inventory — routines must read the canonical clock only.
- Save sections for survivors exist (`survivors`); routine state rides an existing section or gets an explicit row via Plan 87 policy.

## 3. Packages
- **DRA-107A** schedule model doc + priority table vs duty posts.
- **DRA-107B** interruption rules with one test per source.
- **DRA-107C** resume test: save mid-block, load, assert remaining duration and next block.
- **DRA-107D** need-floor test under a full uninterrupted routine day.
- **DRA-107E** unavailable-survivor exclusion test.

## 4. Acceptance & verification
- Each interrupt pauses or cancels exactly per the table; no third behavior.
- Mid-block restore reproduces the exact clock state and next block.
- Need floors hold across a scripted week; unavailable survivors never occupy a block.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/`.

## 5. Risks
Routine vs roster contradiction → priority table is tested against Plan 101's coverage model.
New save section without need → Plan 87 ladder rules decide; state rides `survivors` if it fits.

---

## 6. Expanded census (1 files · 527 lines)

Scope: `Assets/Ashfall.Core/Survivors/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
System 1

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `SurvivorRoutineSystem.cs` | 527 | System | **yes** | 0 | 1 | 2 |

**Totals:** 0 banned refs · 1 empty catches · 1 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `routine_templates.json` | object[2 keys] |
| `exercise_routines.json` | object[2 keys] |

**State surfaces:** `SurvivorRoutineSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 1 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 1 empty-catch sites routed through Plan 35's rules |
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

Domain files: 1. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ORPHAN-SEAL-01` | 1 |
| `EVIDENCE` | 1 |
| `PLAN-SILENT-FAILURE-35` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DRA-107A` | no name match — resolve at claim time |
| `DRA-107B` | no name match — resolve at claim time |
| `DRA-107C` | no name match — resolve at claim time |
| `DRA-107D` | `SurvivorRoutineSystem.cs` |
| `DRA-107E` | `SurvivorRoutineSystem.cs` |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 4. Host files: **1** · Test files: **2** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 1 | `src/Main.SurvivorFitness.cs` |
| Tests (`Ashfall.Core.Tests/`) | 2 | `Ashfall.Core.Tests/DutyRoster/Plan24DutyHourTests.cs`, `Ashfall.Core.Tests/Survivors/Plan188SurvivorRoutineIntegrationTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **7** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `daily_briefing` |
| `dose_ledger` |
| `duty_roster` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **8** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--ledger-debt-selftest` |
| `--real-main-journey-selftest` |
| `--survivor-death-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **8**.

| Event | First declaration |
|---|---|
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnSurvivorJoined` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/backstory_templates.json` |
| `Assets/StreamingAssets/Data/communication_templates.json` |
| `Assets/StreamingAssets/Data/conflict_templates.json` |
| `Assets/StreamingAssets/Data/death_legacy_templates.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/disaster_templates.json` |
| `Assets/StreamingAssets/Data/documentation_templates.json` |
| `Assets/StreamingAssets/Data/dose_items.json` |
| `Assets/StreamingAssets/Data/dose_locations.json` |
| `Assets/StreamingAssets/Data/dose_quests.json` |
| `Assets/StreamingAssets/Data/dose_registers.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **8** (122 files, 957 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Audio` | 5 | 28 |
| `Communication` | 3 | 17 |
| `DutyRoster` | 5 | 49 |
| `Excavation` | 1 | 5 |
| `Integration` | 16 | 74 |
| `Legacy` | 1 | 5 |
| `Quests` | 4 | 25 |
| `Shelter` | 87 | 754 |

**Verdict:** 957 cases sit under matching regions — run those first (`Audio`, `Communication`, `DutyRoster`, `Excavation`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **220**
(27 of them panels/HUD).

| Host file |
|---|
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Dose/DoseRegisterSurface.cs` |
| `src/Host/AnomalyHazardSaveStore.cs` |
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/DailyBriefingSaveStore.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/ExcavationHazardSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **33**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `anomaly_hazard` | no |
| `communication` | no |
| `contractor_roster` | no |
| `daily_briefing` | no |
| `death_legacy` | no |
| `deep_well` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `dynamic_quests` | no |
| `excavation` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `acoustic_detection` |
| `anomaly_hazard` |
| `black_market_debt_event` |
| `deep_coast` |
| `duty_roster` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **77**
(CODEX_ONLY 17, GAMEPLAY_CONSUMED 40, OPTIONAL 4, UNRESOLVED 16).

| Catalog | Classification |
|---|---|
| `acoustic_triangulation_catalog.json` | UNRESOLVED |
| `antigravity_survivor_fields.json` | OPTIONAL |
| `audio_cues.json` | UNRESOLVED |
| `audio_logs_expansion_05.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `chemical_dependency_items.json` | GAMEPLAY_CONSUMED |
| `crossing_items.json` | GAMEPLAY_CONSUMED |
| `crossing_locations.json` | GAMEPLAY_CONSUMED |
| `crossing_quests.json` | GAMEPLAY_CONSUMED |
| `deep_lore_locations.json` | GAMEPLAY_CONSUMED |

**Verdict:** 16 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **2**.

| Flag |
|---|
| `flag_expelled_survivor` |
| `flag_honored_debt` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 33 (laddered 1) · RNG streams 6 · host files 20 · catalogs 22 · test regions 8 · flags 8

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DAILY-ROUTINE-AUTHORITY-107
wave: 9
status: PROPOSED — foreman claim required
packages: DRA-107A, DRA-107B, DRA-107C, DRA-107D, DRA-107E
claim paths:
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - src/Dose/DoseRegisterSurface.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/backstory_templates.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Audio/
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
