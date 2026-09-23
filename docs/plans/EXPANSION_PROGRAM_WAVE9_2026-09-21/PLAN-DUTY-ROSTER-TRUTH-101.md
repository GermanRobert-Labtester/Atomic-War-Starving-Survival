# PLAN-DUTY-ROSTER-TRUTH-101 — Shift Coverage, Fatigue & Overflow Semantics

**Wave 9 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-TEMPORAL-AUTHORITY-33, PLAN-LABOUR-PROFESSIONS-68, PLAN-SAVE-MIGRATION-CORRIDOR-87.
**Non-goals:** no new scheduling simulation, no second roster store, no panel
becoming a roster authority.

## 1. Outcome
`Assets/Ashfall.Core/DutyRoster/` is a full subsystem — `DutyRosterSystem`,
`DutyRosterAssignmentEngine`, `DutyRosterCatalog`, `DutyRosterChartEngine`,
`DutyRosterOverflowEngine`, `DutyHourLedger`, `DutyRosterSave`,
`DutyRosterHoldfastBridge`, `DutyRosterQuestRuntime`, plus `MoraleMarkSystem`
and `ShelterEncounterSystem` — and the save registry already declares
`("duty_roster", "SaveDutyRoster", "SetupDutyRoster", "duty_roster")`. What is
not stated or tested is the **coverage contract**: every required post has an
assignment or a typed overflow, fatigue accrues from the hour ledger (not wall
time), and the chart presented matches the ledger day-for-day.

| Deliverable | Detail |
|---|---|
| Post coverage model | required posts per day/shift with their catalog source; unfilled posts resolve to `DutyRosterOverflowEngine` with a visible consequence |
| Fatigue semantics | fatigue reads `DutyHourLedger` hours; no frame-time or wall-clock input; rest clears per documented rule |
| Chart truth | the player-facing chart is a projection of the ledger; a chart↔ledger reconciliation test runs on a scripted week |
| Overflow visibility | unfilled post → surfaced mark/notice through the existing morale surface, never a silent gap |
| Save round-trip | roster + ledger restore exactly; mid-shift save resumes on the same hour |

## 2. Evidence
- `Assets/Ashfall.Core/DutyRoster/` file list (13 types incl. headless demo).
- `SaveSectionRegistry.All`: `duty_roster` key with `SaveDutyRoster`/`SetupDutyRoster`.
- `DutyRosterHeadlessDemo.cs` provides an existing headless entry point for a bounded check.
- `MoraleMarkSystem`, `ShelterEncounterSystem` sit in the same folder and already consume roster state (re-verify per package).

## 3. Packages
- **DRT-101A** coverage model doc + catalog row table (required posts).
- **DRT-101B** fatigue semantics test: scripted 7-day roster, hour ledger totals match expected fatigue.
- **DRT-101C** chart↔ledger reconciliation on the scripted week.
- **DRT-101D** overflow paths: zero available survivors, every post edge case; visible consequence asserted.
- **DRT-101E** save round-trip incl. mid-shift restore.

## 4. Acceptance & verification
- Scripted week: every required post is filled or overflow-flagged; no third state.
- Fatigue totals derive from ledger hours only (inject a wall-clock change; result unchanged).
- Save/load mid-shift restores the same hour and assignments.
- `bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/` + the existing headless demo verb.

## 5. Risks
Chart diverging from ledger → reconciliation test is the guard.
Fatigue becoming a parallel clock → the hour-consumer audit (Plan 33 Appendix A) already lists `TickHour` readers; this plan stays inside it.

---

## 6. Expanded census (12 files · 3,418 lines)

Scope: `Assets/Ashfall.Core/DutyRoster/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Save 1 · Support 3 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `DutyHourLedger.cs` | 126 | Support | **yes** | 0 | 0 | 0 |
| `DutyRosterAssignmentEngine.cs` | 320 | System | — | 0 | 0 | 0 |
| `DutyRosterCatalog.cs` | 227 | Catalog | — | 0 | 0 | 0 |
| `DutyRosterChartEngine.cs` | 298 | System | — | 0 | 0 | 0 |
| `DutyRosterHeadlessDemo.cs` | 153 | Demo | — | 0 | 0 | 4 |
| `DutyRosterHoldfastBridge.cs` | 259 | Support | — | 0 | 0 | 0 |
| `DutyRosterIds.cs` | 129 | DTO/Type | — | 0 | 0 | 0 |
| `DutyRosterOverflowEngine.cs` | 92 | System | — | 0 | 0 | 2 |
| `DutyRosterQuestRuntime.cs` | 484 | Support | — | 0 | 0 | 2 |
| `DutyRosterSave.cs` | 229 | Save | — | 0 | 0 | 13 |
| `DutyRosterSystem.cs` | 925 | System | **yes** | 0 | 0 | 6 |
| `MoraleMarkSystem.cs` | 176 | System | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 6 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `duty_roster_locations.json` | object[2 keys] |
| `duty_roster_marks.json` | array[43] |
| `duty_roster_quests.json` | object[2 keys] |
| `duty_roster_seasons.json` | array[8] |
| `duty_roles.json` | object[6 keys] |

**State surfaces:** `DutyRosterHeadlessDemo.cs`, `DutyRosterOverflowEngine.cs`, `DutyRosterQuestRuntime.cs`, `DutyRosterSave.cs`, `DutyRosterSystem.cs`, `MoraleMarkSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/DutyRoster/` |
| Test references | 94 name references across the test tree |
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

## 11. Tier-2: intra-domain reference graph

Computed across 12 domain files: **45 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `DutyRosterSystem.cs` | 925 | 18 | 6 |
| `DutyRosterQuestRuntime.cs` | 484 | 3 | 6 |
| `DutyRosterAssignmentEngine.cs` | 320 | 2 | 4 |
| `DutyRosterChartEngine.cs` | 298 | 1 | 6 |
| `DutyRosterHoldfastBridge.cs` | 259 | 1 | 5 |
| `DutyRosterSave.cs` | 229 | 2 | 6 |
| `DutyRosterCatalog.cs` | 227 | 5 | 1 |
| `MoraleMarkSystem.cs` | 176 | 4 | 2 |
| `DutyRosterHeadlessDemo.cs` | 153 | 0 | 5 |
| `DutyRosterIds.cs` | 129 | 7 | 1 |

**Highest-coupling files (in×2 + out):**

- `DutyRosterSystem.cs` — in 18, out 6
- `DutyRosterIds.cs` — in 7, out 1
- `DutyRosterQuestRuntime.cs` — in 3, out 6
- `DutyRosterCatalog.cs` — in 5, out 1
- `DutyRosterSave.cs` — in 2, out 6
- `MoraleMarkSystem.cs` — in 4, out 2
- `DutyRosterAssignmentEngine.cs` — in 2, out 4
- `DutyRosterChartEngine.cs` — in 1, out 6

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 12. Other plans referencing their names: **10**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-VOLUNTARY-REGISTER-TRUTH-253` | 11 |
| `EVIDENCE` | 4 |
| `PLAN-EVENT-WIRING-21` | 2 |
| `PLAN-BELIEF-IDEOLOGY-36` | 1 |
| `PLAN-RECREATION-MORALE-50` | 1 |
| `PLAN-LABOUR-PROFESSIONS-68` | 1 |
| `PLAN-DEV-TOOLING-TRUTH-75` | 1 |
| `PLAN-SCENARIO-AUTHORING-102` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `DRT-101A` | `DutyRosterCatalog.cs` |
| `DRT-101B` | `DutyHourLedger.cs`, `DutyRosterAssignmentEngine.cs`, `DutyRosterCatalog.cs` |
| `DRT-101C` | `DutyHourLedger.cs`, `DutyRosterChartEngine.cs` |
| `DRT-101D` | `DutyRosterOverflowEngine.cs` |
| `DRT-101E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 12. Host files: **23** · Test files: **49** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 23 | `src/Host/ArchiveDeskHostSession.cs`, `src/Host/ContractorRosterHostSession.cs`, `src/Host/DutyRosterHostSession.cs`, `src/Host/DutyRosterSaveStore.cs`, `src/Host/ExpansionHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 49 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **10** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `dose_ledger` |
| `duty_roster` |
| `expansion_quest` |
| `holdfast` |
| `holdfast_trade` |
| `morale` |
| `morale_contagion` |
| `shelter_assignment` |
| `vinyl_morale` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **15** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--dose-ledger-selftest` |
| `--duty-roster-loop-selftest` |
| `--duty-roster-save-selftest` |
| `--duty-roster-selftest` |
| `--duty-roster-uitest` |
| `--holdfast-briefing` |
| `--holdfast-runtime-selftest` |
| `--holdfast-runtime-ui-test` |
| `--holdfast-runtime-uitest` |
| `--holdfast-save-selftest` |
| `--holdfast-selftest` |
| `--holdfast-trade-save-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnAssignmentChanged` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnMarkCleared` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMarkSet` | `Assets/Ashfall.Core/DutyRoster/MoraleMarkSystem.cs` |
| `OnMoraleApplied` | `Assets/Ashfall.Core/VinylMoraleSystem.cs` |
| `OnMoraleDelta` | `Assets/Ashfall.Core/Survivors/RationConflictSystem.cs` |
| `OnMoraleDeltaRequested` | `Assets/Ashfall.Core/Radiation/RadiationPhaseProgression.cs` |
| `OnMoraleDrainRequested` | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` |
| `OnPermanentMoraleBuffApplied` | `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/holdfast_factions.json` |
| `Assets/StreamingAssets/Data/holdfast_flavor.json` |
| `Assets/StreamingAssets/Data/holdfast_items.json` |
| `Assets/StreamingAssets/Data/holdfast_locations.json` |
| `Assets/StreamingAssets/Data/holdfast_npcs.json` |
| `Assets/StreamingAssets/Data/holdfast_quests.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (6 files, 62 cases).

| Region | Files | Cases |
|---|---:|---:|
| `DutyRoster` | 5 | 49 |
| `Holdfast` | 1 | 13 |

**Verdict:** 62 cases sit under matching regions — run those first (`DutyRoster`, `Holdfast`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **44**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DoseLedgerHostSession.cs` |
| `src/Host/DoseLedgerSaveStore.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HoldfastBriefingView.cs` |
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/HoldfastFlavorCatalog.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **10**, of which versioned-ladder sections:
**2**.

| Section key | Laddered |
|---|---|
| `contractor_roster` | no |
| `dose_ledger` | yes |
| `duty_roster` | no |
| `expansion_quest` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `morale` | no |
| `morale_contagion` | no |
| `shelter_assignment` | no |
| `vinyl_morale` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `duty_roster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **14**
(CODEX_ONLY 2, GAMEPLAY_CONSUMED 9, OPTIONAL 1, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |
| `holdfast_locations.json` | GAMEPLAY_CONSUMED |
| `holdfast_npcs.json` | UNRESOLVED |
| `holdfast_quests.json` | GAMEPLAY_CONSUMED |

**Verdict:** 2 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **0**.

| Flag |
|---|
| — | no persistent flag shares a token with this domain |

**Verdict:** no persistent flag matches — the domain's narrative consequences are carried by other state (or the flag set is a candidate for cleanup).

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 10
**Surface:** save sections 10 (laddered 2) · RNG streams 1 · host files 13 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-DUTY-ROSTER-TRUTH-101
wave: 9
status: PROPOSED — foreman claim required
packages: DRT-101A, DRT-101B, DRT-101C, DRT-101D, DRT-101E
claim paths:
  - src/Host/ContractorRosterHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerHostSession.cs  # §19 candidate host surface
  - src/Host/DoseLedgerSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/duty_roles.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/duty_roster_locations.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --dose-ledger-selftest
dependencies:
  - coordinate: 10 other plan(s) name these artifacts (§12)
  - governance spine must land first: 56 → 86 → 17 → 100
  - touches 2 versioned save ladder(s) — extend, never fork
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
