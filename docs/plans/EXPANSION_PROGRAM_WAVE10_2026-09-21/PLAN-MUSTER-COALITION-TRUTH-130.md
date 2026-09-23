# PLAN-MUSTER-COALITION-TRUTH-130 — Muster Camp, Coalition Actions & the Epilogue Matrix

**Wave 10 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-WARLORDS-DIPLOMACY-29, PLAN-BASE-DEFENSE-RAIDS-61, PLAN-CRISIS-DISASTER-RESPONSE-80.
**Non-goals:** no new faction system (Plan 29), no raid resolution (Plan 61), no
crisis protocol system (Plan 80).

## 1. Outcome
`Muster/` is a substantial late-game subsystem: `CampSceneCatalog`,
`CoalitionCampSystem`, `ColdCountSystem`, `CurrentsCatalog`, `EpilogueMatrix`,
`FactionActionBoard`, `FactionActionCatalog`, and more; the save registry
already declares `("muster", "SaveMuster", "SetupMuster", "muster", "The Muster
military rally & conflict state")`. What is unstated is how the camp,
coalition, faction actions, and ending matrix relate: which system owns a
coalition's readiness, what a faction action changes, and what the epilogue
matrix reads to produce an ending.

| Deliverable | Detail |
|---|---|
| Camp model | muster camp state: participants, provisions, readiness — owners named, no parallel force ledger |
| Coalition actions | `FactionActionBoard` actions with preconditions and effects on named owners (diplomacy Plan 29, readiness here) |
| Cold count | `ColdCountSystem` defined (what is counted, when, from which authority) — read-only over owners |
| Epilogue matrix | inputs documented: which persisted facts each ending row reads; a missing input degrades to an explicit "unknown" row, never a silent default |
| Save truth | muster state restores; the camp does not re-roll readiness on load |

## 2. Evidence
- `Assets/Ashfall.Core/Muster/` file list: `CampSceneCatalog.cs`, `CoalitionCampSystem.cs`, `ColdCountSystem.cs`, `CurrentsCatalog.cs`, `EpilogueMatrix.cs`, `FactionActionBoard.cs`, `FactionActionCatalog.cs` (verified).
- `SaveSectionRegistry.All`: `("muster", "SaveMuster", "SetupMuster", "muster", "The Muster military rally & conflict state")`.
- Plan 29 owns diplomacy/territory; Plan 61 owns raids; Plan 80 owns disasters.
- Plan 1 Appendix G: Muster-domain candidates include `Main.Muster*` partials (re-verify per package).

## 3. Packages
- **MCT-130A** camp model + owner table.
- **MCT-130B** faction action preconditions/effects wired to Plan 29 owners.
- **MCT-130C** cold-count definition + authority read test.
- **MCT-130D** epilogue matrix input table + missing-input degradation test.
- **MCT-130E** save round-trip + no-re-roll-on-load test.

## 4. Acceptance & verification
- Each faction action changes only its named owners; a scripted action set is traceable.
- Cold count equals the authorities it reads (no drift after a scripted day).
- Epilogue with a missing input shows the unknown row; with all inputs, the expected row.
- `bash scripts/run_test.sh` on the muster region + one headless muster run if a verb exists.

## 5. Risks
Ending logic duplication → `VerdictEndingEvaluator` (Verdict/) and the epilogue matrix have separate scopes; the input table states which owns which ending class.
Force ledger duplication → readiness reads participants/provisions from their owners; no second army number.

---

## 6. Expanded census (9 files · 2,309 lines)

Scope: `Assets/Ashfall.Core/Muster/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · DTO/Type 1 · Demo 1 · Support 3 · System 3

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `CampSceneCatalog.cs` | 204 | Catalog | — | 0 | 0 | 0 |
| `CoalitionCampSystem.cs` | 171 | System | **yes** | 0 | 0 | 2 |
| `EpilogueMatrix.cs` | 219 | Support | **yes** | 0 | 0 | 0 |
| `FactionActionBoard.cs` | 405 | Support | **yes** | 0 | 0 | 2 |
| `MusterHeadlessDemo.cs` | 103 | Demo | — | 0 | 0 | 6 |
| `MusterPathEvaluator.cs` | 92 | Support | — | 0 | 0 | 0 |
| `MusterSystem.cs` | 527 | System | — | 0 | 0 | 3 |
| `MusterWarfareEngine.cs` | 323 | System | — | 0 | 0 | 0 |
| `MusterWarfareTypes.cs` | 265 | DTO/Type | — | 0 | 0 | 1 |

**Totals:** 0 banned refs · 0 empty catches · 5 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `labor_camps.json` | object[2 keys] |
| `muster_camp_scenes.json` | object[2 keys] |
| `muster_faction_actions.json` | object[2 keys] |
| `campaign_epilogues.json` | object[2 keys] |
| `propaganda_campaigns.json` | object[3 keys] |
| `epilogue_chronicle.json` | object[2 keys] |

**State surfaces:** `CoalitionCampSystem.cs`, `FactionActionBoard.cs`, `MusterHeadlessDemo.cs`, `MusterSystem.cs`, `MusterWarfareTypes.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Muster/` (create if absent) |
| Test references | 20 name references across the test tree |
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

Computed across 9 domain files: **20 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `MusterSystem.cs` | 527 | 6 | 4 |
| `MusterWarfareEngine.cs` | 323 | 1 | 7 |
| `MusterWarfareTypes.cs` | 265 | 8 | 0 |
| `EpilogueMatrix.cs` | 219 | 0 | 1 |
| `CampSceneCatalog.cs` | 204 | 0 | 2 |
| `CoalitionCampSystem.cs` | 171 | 2 | 1 |
| `LongWalkSystem.cs` | 153 | 0 | 0 |
| `MusterHeadlessDemo.cs` | 103 | 0 | 3 |
| `MusterPathEvaluator.cs` | 92 | 3 | 2 |

**Highest-coupling files (in×2 + out):**

- `MusterSystem.cs` — in 6, out 4
- `MusterWarfareTypes.cs` — in 8, out 0
- `MusterWarfareEngine.cs` — in 1, out 7
- `MusterPathEvaluator.cs` — in 3, out 2
- `CoalitionCampSystem.cs` — in 2, out 1
- `MusterHeadlessDemo.cs` — in 0, out 3
- `CampSceneCatalog.cs` — in 0, out 2
- `EpilogueMatrix.cs` — in 0, out 1

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 9. Other plans referencing their names: **3**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-MUSTER-FAMILY-TRUTH-275` | 9 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 4 |
| `PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MCT-130A` | no name match — resolve at claim time |
| `MCT-130B` | no name match — resolve at claim time |
| `MCT-130C` | no name match — resolve at claim time |
| `MCT-130D` | `EpilogueMatrix.cs` |
| `MCT-130E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 13. Host files: **10** · Test files: **13** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 10 | `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.cs`, `src/Host/MusterHostSession.cs`, `src/Main.UiTests.Muster.cs`, `src/Muster/ApproachSelectionModal.cs` |
| Tests (`Ashfall.Core.Tests/`) | 13 | `Ashfall.Core.Tests/CoalitionCampSystemTests.cs`, `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`, `Ashfall.Core.Tests/EventTriggerTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs`, `Ashfall.Core.Tests/FactionActionBoardTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **3** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chem_warfare` |
| `faction_espionage` |
| `muster` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **6** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--communique-board-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-tick-demo` |
| `--muster-selftest` |
| `--muster-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **14**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnCampDawnResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEntered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampFormed` | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` |
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampSuppliesReserved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnFactionStandingChanged` | `Assets/Ashfall.Core/YearOfAsh/FactionWarSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/currents.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/epilogue_personalization.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |
| `Assets/StreamingAssets/Data/faction_war_events.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **0** (0 files, 0 cases).

| Region | Files | Cases |
|---|---:|---:|
| — | no test region shares a token with this domain |

**Verdict:** no test region shares a token with this domain. Region coverage is directory-based, so check root-level test files too (560 exist) before concluding coverage is absent.

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **35**
(16 of them panels/HUD).

| Host file |
|---|
| `src/Host/CatalogPath.cs` |
| `src/Host/ChemWarfareSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/MusterHostSession.cs` |
| `src/Host/MusterSaveStore.cs` |
| `src/Host/SceneBindingSelfTest.cs` |
| `src/Journal/JournalDemoHarness.cs` |
| `src/Main.FactionBranch.cs` |
| `src/Main.Muster.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **3**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chem_warfare` | no |
| `faction_espionage` | no |
| `muster` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **1**.

| Stream |
|---|
| `muster` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **28**
(CODEX_ONLY 5, GAMEPLAY_CONSUMED 18, UNRESOLVED 5).

| Catalog | Classification |
|---|---|
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `currents.json` | GAMEPLAY_CONSUMED |
| `epilogue_chronicle.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |
| `faction_war_communiques.json` | GAMEPLAY_CONSUMED |
| `faction_war_dialogue.json` | GAMEPLAY_CONSUMED |
| `faction_war_events.json` | GAMEPLAY_CONSUMED |
| `faction_war_journal.json` | GAMEPLAY_CONSUMED |

**Verdict:** 5 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 3
**Surface:** save sections 3 (laddered 0) · RNG streams 1 · host files 15 · catalogs 22 · test regions 0 · flags 6

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MUSTER-COALITION-TRUTH-130
wave: 10
status: PROPOSED — foreman claim required
packages: MCT-130A, MCT-130B, MCT-130C, MCT-130D, MCT-130E
claim paths:
  - src/Host/CatalogPath.cs  # §19 candidate host surface
  - src/Host/ChemWarfareSaveStore.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/FactionBranchHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bounty_board.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/currents.json  # §17 catalog (verify schema + consumer)
verification:
  - godot --headless --path . -- --communique-board-selftest
dependencies:
  - coordinate: 3 other plan(s) name these artifacts (§12)
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
