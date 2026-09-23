# PLAN-CONTRACTOR-ROSTER-TRUTH-245 — Outside Labor: Terms, Lodging & Departure

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-SURVIVOR-ROSTER-TRUTH-244, PLAN-COMMITMENTS-OBLIGATIONS-TRUTH-122, PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-LABOUR-PROFESSIONS-68.
**Non-goals:** no person registry (Plan 244), no obligations (Plan 122), no
professions (Plan 68).

## 1. Outcome
`ContractorRosterSystem.cs` (**236 lines**, Core root) is reachable and
unaddressed: hired outside labor — terms, lodging, work assignment, and
departure. The person registry (Plan 244) tracks residents; contractors are a
distinct population with a contract clock and an exit.

| Deliverable | Detail |
|---|---|
| Contract model | contractor records with terms (duration, pay, task class), referenced by Plan 122's obligation records |
| Registry integration | contractors appear in Plan 244's registry with a `contractor` status while present |
| Pay/wages | payments are Plan 96 ledger rows; no private wage ledger |
| Lodging/needs | contractors consume housing/food through existing owners (Plan 103/39) |
| Departure | contract end/breach/dismissal paths with a departure record and roster cleanup |
| Save truth | contracts and presence restore; no duplicate contractor on load |

## 2. Evidence
- `Assets/Ashfall.Core/ContractorRosterSystem.cs` (236 lines; unaddressed — Wave 18 audit).
- Plan 244 supplies the registry; Plan 122 the obligation shape.
- Plan 96 holds payments; Plan 103/39 lodging/food.

## 3. Packages
- **CRT-245A** contract model + terms table.
- **CRT-245B** registry integration test (contractor status, no duplicate).
- **CRT-245C** pay rows through Plan 96 + reconciliation.
- **CRT-245D** lodging/needs consumption tests.
- **CRT-245E** departure paths + roster cleanup; save round-trip.

## 4. Acceptance & verification
- A contractor appears once in the registry with its status; pay reconciles.
- Departure removes presence and leaves the contract record.
- `bash scripts/run_test.sh` on the roster/labour region.

## 5. Risks
Parallel population → one registry; the duplicate test enforces it.
Wage duplication → Plan 96 rows only.

---

## 6. Expanded census (2 files · 533 lines)

Scope: the Core-root `ContractorRosterSystem.cs` plus `Survivors/` and
`Economy/` files matching the roster/contractor tokens, plus the survivor
registry sibling. (Corrected: the root-level placement of the premise file made
a directory-only scan empty.)

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `ContractorRosterSystem.cs` | 236 | System | **yes** | 0 | 0 | 2 |
| `SurvivorCatalog.cs` | 297 | Catalog | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 2 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `duty_roster_locations.json` | object[2 keys] |
| `duty_roster_marks.json` | object[2 keys] |
| `duty_roster_quests.json` | object[2 keys] |
| `duty_roster_seasons.json` | object[2 keys] |

**State surfaces:** `ContractorRosterSystem.cs`, `SurvivorCatalog.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Survivors/` |
| Test references | 6 name references across the test tree |
| Determinism | 0 banned refs to fix or justify |
| Failures | 0 empty-catch sites routed through Plan 35's rules |
| Registry contract | contractors appear once in `SurvivorCatalog` with a distinct status (Plan 244) |

## 9. Rollout sequence

1. Premise re-check: `ContractorRosterSystem.cs` unchanged since authoring.
2. Registry integration: one contractor record, no duplicate person (Plan 244).
3. Pay/wages: rows through Plan 96's ledger; no private wage store.
4. Lodging/needs: consume through Plan 103/39 owners.
5. Departure: records + roster cleanup; conservation check.
6. Regression: focused region plus this census regenerated.

## 10. Acceptance matrix

| Deliverable class | Acceptance |
|---|---|
| Premise file | matches its stated surface; no scope drift |
| Registry | one record per contractor; status distinct from residents |
| Ledger | wages reconcile in Plan 96 |
| Departure | presence removed; contract record retained |
| Verification | focused region green; census delta recorded |

**Non-goals unchanged:** this expansion adds census and verification detail; it does not widen the plan's scope.

---

## 12. Cross-plan coupling

(Corrected scope: the premise system lives at the Core root, so a directory scan
was empty; this section uses the root file plus the registry sibling and any
roster-named file.)

Domain files: 12 — `ContractorRosterSystem.cs`, `DutyRosterAssignmentEngine.cs`, `DutyRosterCatalog.cs`, `DutyRosterChartEngine.cs`, `DutyRosterHeadlessDemo.cs`, `DutyRosterHoldfastBridge.cs`, `DutyRosterIds.cs`, `DutyRosterOverflowEngine.cs`, `DutyRosterQuestRuntime.cs`, `DutyRosterSave.cs`, `DutyRosterSystem.cs`, `SurvivorCatalog.cs`.
Other plans referencing their names: **8**.

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-DUTY-ROSTER-TRUTH-101` | 10 |
| `PLAN-VOLUNTARY-REGISTER-TRUTH-253` | 10 |
| `EVIDENCE` | 2 |
| `PLAN-EVENT-WIRING-21` | 1 |
| `PLAN-DEV-TOOLING-TRUTH-75` | 1 |
| `PLAN-SCENARIO-AUTHORING-102` | 1 |
| `PLAN-SURVIVOR-ROSTER-TRUTH-244` | 1 |
| `PLAN-CORE-ROOT-FAMILY-TRUTH-262` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `CRT-245A` | `ContractorRosterSystem.cs` |
| `CRT-245B` | `ContractorRosterSystem.cs` |
| `CRT-245C` | no name match — resolve at claim time |
| `CRT-245D` | no name match — resolve at claim time |
| `CRT-245E` | `ContractorRosterSystem.cs`, `DutyRosterAssignmentEngine.cs`, `DutyRosterCatalog.cs` |

**Reading:** incoming edges are coordination risk; candidate files are a starting
point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 12; intra-domain edges: **24**; isolated files:
**1**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `ContractorRosterSystem` | `DutyRosterSystem` |
| `DutyRosterAssignmentEngine` | `DutyRosterIds` |
| `DutyRosterAssignmentEngine` | `DutyRosterSystem` |
| `DutyRosterCatalog` | `DutyRosterIds` |
| `DutyRosterChartEngine` | `DutyRosterAssignmentEngine` |
| `DutyRosterChartEngine` | `DutyRosterIds` |
| `DutyRosterChartEngine` | `DutyRosterSystem` |
| `DutyRosterHeadlessDemo` | `DutyRosterIds` |
| `DutyRosterHeadlessDemo` | `DutyRosterSystem` |
| `DutyRosterHoldfastBridge` | `DutyRosterIds` |
| `DutyRosterHoldfastBridge` | `DutyRosterQuestRuntime` |
| `DutyRosterHoldfastBridge` | `DutyRosterSystem` |
| `DutyRosterIds` | `DutyRosterSystem` |
| `DutyRosterOverflowEngine` | `DutyRosterSystem` |
| `DutyRosterQuestRuntime` | `DutyRosterCatalog` |
| `DutyRosterQuestRuntime` | `DutyRosterHoldfastBridge` |
| `DutyRosterQuestRuntime` | `DutyRosterIds` |
| `DutyRosterQuestRuntime` | `DutyRosterSystem` |
| `DutyRosterSave` | `DutyRosterQuestRuntime` |
| `DutyRosterSave` | `DutyRosterSystem` |
| `DutyRosterSystem` | `DutyRosterAssignmentEngine` |
| `DutyRosterSystem` | `DutyRosterChartEngine` |
| `DutyRosterSystem` | `DutyRosterIds` |
| `DutyRosterSystem` | `DutyRosterOverflowEngine` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `DutyRosterSystem` | 9 |
| `DutyRosterIds` | 7 |
| `DutyRosterAssignmentEngine` | 2 |
| `DutyRosterQuestRuntime` | 2 |
| `DutyRosterCatalog` | 1 |
| `DutyRosterChartEngine` | 1 |
| `DutyRosterHoldfastBridge` | 1 |
| `DutyRosterOverflowEngine` | 1 |
| `ContractorRosterSystem` | 0 |
| `DutyRosterHeadlessDemo` | 0 |

**Class split:** hub 8 · sink 0 · source 3 · isolated 1.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 12. Host files: **23** · Test files: **47** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 23 | `src/Host/ArchiveDeskHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ContractorRosterHostSession.cs`, `src/Host/DutyRosterHostSession.cs`, `src/Host/DutyRosterSaveStore.cs` |
| Tests (`Ashfall.Core.Tests/`) | 47 | `Ashfall.Core.Tests/ApprenticeshipIntegrationTests.cs`, `Ashfall.Core.Tests/ApprenticeshipSystemTests.cs`, `Ashfall.Core.Tests/ArchiveDeskSystemTests.cs`, `Ashfall.Core.Tests/ArchiveInksCatalogTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **10** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `contractor_roster` |
| `duty_roster` |
| `expansion_quest` |
| `holdfast` |
| `holdfast_trade` |
| `shelter_assignment` |
| `survivor_fate` |
| `survivor_mental_health` |
| `survivor_relations` |
| `survivor_social` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
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
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **18**.

| Event | First declaration |
|---|---|
| `OnAssignmentChanged` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnContractorStatusChanged` | `Assets/Ashfall.Core/ContractorRosterSystem.cs` |
| `OnCraftResultOverflow` | `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` |
| `OnDutyVacated` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnQuestChoiceTaken` | `Assets/Ashfall.Core/YearOfAsh/QuestlineSystem.cs` |
| `OnQuestCompleted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestFailed` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnQuestStageAdvanced` | `Assets/Ashfall.Core/DutyRoster/DutyRosterQuestRuntime.cs` |
| `OnQuestStageChanged` | `Assets/Ashfall.Core/HoldfastQuestSystem.cs` |
| `OnQuestStarted` | `Assets/Ashfall.Core/ExpansionQuestSystem.cs` |
| `OnRosterBurned` | `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/duty_roles.json` |
| `Assets/StreamingAssets/Data/duty_roster_locations.json` |
| `Assets/StreamingAssets/Data/duty_roster_marks.json` |
| `Assets/StreamingAssets/Data/duty_roster_quests.json` |
| `Assets/StreamingAssets/Data/duty_roster_seasons.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/holdfast_factions.json` |
| `Assets/StreamingAssets/Data/holdfast_flavor.json` |
| `Assets/StreamingAssets/Data/holdfast_items.json` |

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

Host files (`src/`) whose names share a domain token: **50**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Host/ContractorRosterHostSession.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DutyRosterHostSession.cs` |
| `src/Host/DutyRosterSaveStore.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/HoldfastBriefingView.cs` |
| `src/Host/HoldfastDispatchLog.cs` |
| `src/Host/HoldfastFlavorCatalog.cs` |
| `src/Host/HoldfastRuntimeSession.cs` |
| `src/Host/HoldfastSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **10**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `contractor_roster` | no |
| `duty_roster` | no |
| `expansion_quest` | no |
| `holdfast` | yes |
| `holdfast_trade` | no |
| `shelter_assignment` | no |
| `survivor_fate` | no |
| `survivor_mental_health` | no |
| `survivor_relations` | no |
| `survivor_social` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **17**
(CODEX_ONLY 3, GAMEPLAY_CONSUMED 10, OPTIONAL 3, UNRESOLVED 1).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `duty_roster_locations.json` | GAMEPLAY_CONSUMED |
| `duty_roster_marks.json` | GAMEPLAY_CONSUMED |
| `duty_roster_quests.json` | GAMEPLAY_CONSUMED |
| `duty_roster_seasons.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `holdfast_factions.json` | GAMEPLAY_CONSUMED |
| `holdfast_flavor.json` | GAMEPLAY_CONSUMED |
| `holdfast_items.json` | GAMEPLAY_CONSUMED |

**Verdict:** 1 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 8
**Surface:** save sections 10 (laddered 1) · RNG streams 1 · host files 14 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CONTRACTOR-ROSTER-TRUTH-245
wave: 18
status: PROPOSED — foreman claim required
packages: CRT-245A, CRT-245B, CRT-245C, CRT-245D, CRT-245E
claim paths:
  - src/Host/ContractorRosterHostSession.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DutyRosterHostSession.cs  # §19 candidate host surface
  - src/Host/DutyRosterSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/deep_lore_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/DutyRoster/
  - godot --headless --path . -- --duty-roster-loop-selftest
dependencies:
  - coordinate: 8 other plan(s) name these artifacts (§12)
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
