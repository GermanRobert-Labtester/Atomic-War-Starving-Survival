# PLAN-MUSTER-FAMILY-TRUTH-275 — Path Evaluation, Warfare Types & Demos

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-MUSTER-COALITION-TRUTH-130, PLAN-MUSTER-FACTIONS-TRUTH-254, PLAN-BASE-DEFENSE-RAIDS-61.
**Non-goals:** no camp model change; the family is audited for path/warfare
data wiring.

## 1. Outcome
**11 `Muster/` files** are referenced by no plan: `MusterPathEvaluator`,
`MusterWarfareTypes`, `MusterSystem`, `MusterWarfareEngine`,
`MusterHeadlessDemo`, `FactionCultureCatalog`, `FactionEcologyHeadlessDemo`.
Plan 130 owns the camp; Plan 254 the named factions — path evaluation and
warfare types are the glue between them and Plan 61.

| Deliverable | Detail |
|---|---|
| Path evaluation | route/scoring inputs documented; results deterministic |
| Warfare types | types consumed by Plan 61's resolution or flagged dead |
| System boundary | `MusterSystem` vs Plan 130's coalition — one owner per state, stated |
| Catalog wiring | culture catalog resolves to Plan 254's factions |
| Demo truth | headless demos resolve to verbs |

## 2. Evidence
- 11 `Muster/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 130 documents camp state; Plan 254 faction standings; both boundaries cited.
- Plan 61 owns resolution the warfare types feed.

## 3. Packages
- **MUF-275A** path-evaluation determinism fixture.
- **MUF-275B** warfare type consumption table.
- **MUF-275C** system-boundary statement + test.
- **MUF-275D** catalog wiring test.
- **MUF-275E** demo→verb checks.

## 4. Acceptance & verification
- Path scores deterministic; types consumed or dead; one owner per state.
- `bash scripts/run_test.sh` on the muster region.

## 5. Risks
Three muster plans overlapping → owner-per-state statement is mandatory.
Dead types → consumption table reports them.

---

## 6. Expanded census (22 family files · 4,119 lines)

Scope: files under `Assets/Ashfall.Core/Muster/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: System 9 · Catalog 5 · Support 5 · Demo 2 · DTO/Type 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `CampSceneCatalog.cs` | 204 | Catalog | 0 | 0 | 0 |
| `CoalitionCampSystem.cs` | 171 | System | 0 | 0 | 2 |
| `ColdCountSystem.cs` | 153 | System | 0 | 0 | 2 |
| `CurrentsCatalog.cs` | 118 | Catalog | 0 | 0 | 0 |
| `EpilogueMatrix.cs` | 219 | Support | 0 | 0 | 0 |
| `FactionActionBoard.cs` | 405 | Support | 0 | 0 | 2 |
| `FactionActionCatalog.cs` | 227 | Catalog | 0 | 0 | 0 |
| `FactionCultureCatalog.cs` | 90 | Catalog | 0 | 0 | 0 |
| `FactionEcologyHeadlessDemo.cs` | 199 | Demo | 0 | 0 | 9 |
| `HydroBaronsSystem.cs` | 138 | System | 0 | 0 | 2 |
| `IronRaidersSystem.cs` | 111 | System | 0 | 0 | 2 |
| `LongWalkSystem.cs` | 153 | System | 0 | 0 | 2 |
| `MusterHeadlessDemo.cs` | 103 | Demo | 0 | 0 | 6 |
| `MusterPathEvaluator.cs` | 92 | Support | 0 | 0 | 0 |
| `MusterSystem.cs` | 527 | System | 0 | 0 | 3 |
| `MusterWarfareEngine.cs` | 323 | System | 0 | 0 | 0 |
| `MusterWarfareTypes.cs` | 265 | DTO/Type | 0 | 0 | 1 |
| `ProvisionedSystem.cs` | 112 | System | 0 | 0 | 2 |
| `QuestApproach.cs` | 37 | Support | 0 | 0 | 0 |
| `ScavengerGuildSystem.cs` | 135 | System | 0 | 0 | 2 |
| `WitnessCatalog.cs` | 164 | Catalog | 0 | 0 | 0 |
| `WitnessSelector.cs` | 173 | Support | 0 | 0 | 0 |

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 12 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `muster_camp_scenes.json` | object[2 keys] |
| `muster_faction_actions.json` | object[2 keys] |
| `muster_epilogues.json` | object[2 keys] |
| `muster_faction_culture.json` | array[25] |
| `muster_witnesses.json` | object[2 keys] |

**State surfaces (capture/restore present):**

- `CoalitionCampSystem.cs`
- `ColdCountSystem.cs`
- `FactionActionBoard.cs`
- `FactionEcologyHeadlessDemo.cs`
- `HydroBaronsSystem.cs`
- `IronRaidersSystem.cs`
- `LongWalkSystem.cs`
- `MusterHeadlessDemo.cs`
- `MusterSystem.cs`
- `MusterWarfareTypes.cs`
- `ProvisionedSystem.cs`
- `ScavengerGuildSystem.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Muster/` (create if absent) |
| Family files referenced by tests | 47 name references across the test tree |
| Determinism scan | 0 banned references to fix or justify |
| Failure scan | 0 empty-catch sites to route through Plan 35's rules |
| Docs | `python3 scripts/ci/generate-docs-index.py --check` |

## 9. Rollout sequence

1. Census first (section 6) — classify every family file; no edits in this step.
2. Catalogs and loaders: prove a consumer or report the file as inert.
3. DTO/Type files: round-trip or consume-only proof; unknown values fail typed.
4. System files: confirm the single owner per state; remove duplicated stores.
5. Demo/tooling files: resolve to a real CLI verb or retire (Plan 86 pattern).
6. Regression: focused region plus this family census regenerated.

## 10. Acceptance matrix

| File class | Acceptance |
|---|---|
| Catalog | resolves through a loader; malformed fixture fails typed with the field named |
| Loader | valid/invalid fixture pair; unknown id names the field |
| DTO/Type | round-trip or consume-only proof; no orphan type |
| Save | capture/restore round-trip; key per Plan 1 Appendix Q |
| System | one owner per state; no parallel store |
| Demo | resolves to an existing verb or is retired |
| Support | consumed by a system or reported ownerless |

**Non-goals unchanged:** this expansion adds census and verification detail; it
does not widen the plan's scope or create new authorities.

---

## 12. Cross-plan coupling

This is a family-survey plan; the domain set is the plan's own `.cs` enumeration
(22 files). Other plans referencing those names: **5**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-MUSTER-COALITION-TRUTH-130` | 13 |
| `PLAN-ELECTRONICS-COMPUTING-65` | 5 |
| `PLAN-WARLORDS-DIPLOMACY-29` | 4 |
| `PLAN-MUSTER-FACTIONS-TRUTH-254` | 4 |
| `PLAN-ACHIEVEMENTS-COMPLETION-TRUTH-76` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `MUF-275A` | no name match — resolve at claim time |
| `MUF-275B` | `MusterWarfareEngine.cs`, `MusterWarfareTypes.cs` |
| `MUF-275C` | `CoalitionCampSystem.cs`, `ColdCountSystem.cs`, `HydroBaronsSystem.cs` |
| `MUF-275D` | `CampSceneCatalog.cs`, `CurrentsCatalog.cs`, `FactionActionCatalog.cs` |
| `MUF-275E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 22; intra-domain edges: **21**; isolated files:
**7**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `CampSceneCatalog` | `MusterSystem` |
| `CoalitionCampSystem` | `MusterSystem` |
| `CoalitionCampSystem` | `QuestApproach` |
| `EpilogueMatrix` | `MusterSystem` |
| `FactionActionBoard` | `CoalitionCampSystem` |
| `FactionActionBoard` | `HydroBaronsSystem` |
| `FactionActionBoard` | `IronRaidersSystem` |
| `FactionActionBoard` | `ScavengerGuildSystem` |
| `FactionActionCatalog` | `FactionActionBoard` |
| `FactionEcologyHeadlessDemo` | `FactionActionBoard` |
| `FactionEcologyHeadlessDemo` | `MusterPathEvaluator` |
| `FactionEcologyHeadlessDemo` | `MusterSystem` |
| `FactionEcologyHeadlessDemo` | `ScavengerGuildSystem` |
| `FactionEcologyHeadlessDemo` | `WitnessSelector` |
| `HydroBaronsSystem` | `QuestApproach` |
| `MusterHeadlessDemo` | `CoalitionCampSystem` |
| `MusterHeadlessDemo` | `MusterSystem` |
| `MusterHeadlessDemo` | `QuestApproach` |
| `MusterPathEvaluator` | `CoalitionCampSystem` |
| `MusterSystem` | `MusterWarfareEngine` |
| `MusterSystem` | `QuestApproach` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `MusterSystem` | 5 |
| `QuestApproach` | 4 |
| `CoalitionCampSystem` | 3 |
| `FactionActionBoard` | 2 |
| `ScavengerGuildSystem` | 2 |
| `HydroBaronsSystem` | 1 |
| `IronRaidersSystem` | 1 |
| `MusterPathEvaluator` | 1 |
| `MusterWarfareEngine` | 1 |
| `WitnessSelector` | 1 |

**Class split:** hub 5 · sink 5 · source 5 · isolated 7.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 22. Host files: **11** · Test files: **16** · Data files: **0**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 11 | `src/Host/HostCli.SelfTests.cs`, `src/Host/HostCli.cs`, `src/Host/MusterHostSession.cs`, `src/Main.Muster.cs`, `src/Main.UiTests.Muster.cs` |
| Tests (`Ashfall.Core.Tests/`) | 16 | `Ashfall.Core.Tests/CoalitionCampSystemTests.cs`, `Ashfall.Core.Tests/DebtConsequenceIntegrationTests.cs`, `Ashfall.Core.Tests/Endgame/EpilogueChronicleCatalogTests.cs`, `Ashfall.Core.Tests/EventTriggerTests.cs`, `Ashfall.Core.Tests/ExpansionAggregateCompletenessTests.cs` |
| Data (`StreamingAssets/Data/`) | 0 | — |

**Verdict:** no data reference found — confirm whether the authority is code-only

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **4** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `chem_warfare` |
| `expansion_quest` |
| `faction_espionage` |
| `muster` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **7** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--communique-board-selftest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--ice-road-tick-demo` |
| `--muster-selftest` |
| `--muster-uitest` |
| `--personal-quest-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **21**.

| Event | First declaration |
|---|---|
| `OnActionCompleted` | `Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs` |
| `OnActionExecuted` | `Assets/Ashfall.Core/Warlords/WarlordDoctrineSystem.cs` |
| `OnActionResolved` | `Assets/Ashfall.Core/Muster/FactionActionBoard.cs` |
| `OnActionSelected` | `Assets/Ashfall.Core/UtilityAI/UtilityAiSystem.cs` |
| `OnApproachResolved` | `Assets/Ashfall.Core/Muster/HydroBaronsSystem.cs` |
| `OnCampDawnResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEncounterSurfaced` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampEntered` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampFormed` | `Assets/Ashfall.Core/Muster/CoalitionCampSystem.cs` |
| `OnCampNightSegmentResolved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |
| `OnCampSuppliesReserved` | `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/currents.json` |
| `Assets/StreamingAssets/Data/dynamic_quest_templates.json` |
| `Assets/StreamingAssets/Data/epilogue_chronicle.json` |
| `Assets/StreamingAssets/Data/epilogue_personalization.json` |
| `Assets/StreamingAssets/Data/faction_combat_thresholds.json` |
| `Assets/StreamingAssets/Data/faction_intelligence.json` |
| `Assets/StreamingAssets/Data/faction_lore.json` |
| `Assets/StreamingAssets/Data/faction_radio_corpus.json` |
| `Assets/StreamingAssets/Data/faction_territory.json` |
| `Assets/StreamingAssets/Data/faction_war_communiques.json` |
| `Assets/StreamingAssets/Data/faction_war_dialogue.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **1** (7 files, 40 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Culture` | 7 | 40 |

**Verdict:** 40 cases sit under matching regions — run those first (`Culture`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **48**
(21 of them panels/HUD).

| Host file |
|---|
| `src/Host/CatalogPath.cs` |
| `src/Host/ChemWarfareSaveStore.cs` |
| `src/Host/CoreDemoSession.cs` |
| `src/Host/DynamicQuestSaveStore.cs` |
| `src/Host/ExpansionQuestHostSession.cs` |
| `src/Host/ExpansionQuestSaveStore.cs` |
| `src/Host/FactionBranchHostSession.cs` |
| `src/Host/FactionIconLoader.cs` |
| `src/Host/HostCli.FactionCommuniqueSelfTests.cs` |
| `src/Host/HydroGeologyArchiveSaveStore.cs` |
| `src/Host/MusterHostSession.cs` |
| `src/Host/MusterSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **4**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `chem_warfare` | no |
| `expansion_quest` | no |
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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **33**
(CODEX_ONLY 9, GAMEPLAY_CONSUMED 18, OPTIONAL 1, UNRESOLVED 5).

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

Matching persistent flags: **3**.

| Flag |
|---|
| `flag_betrayed_faction` |
| `flag_branch_iron_way_locked` |
| `flag_chosen_faction_side` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 5
**Surface:** save sections 4 (laddered 0) · RNG streams 1 · host files 16 · catalogs 22 · test regions 1 · flags 7

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-MUSTER-FAMILY-TRUTH-275
wave: 19
status: PROPOSED — foreman claim required
packages: MUF-275A, MUF-275B, MUF-275C, MUF-275D, MUF-275E
claim paths:
  - src/Host/CatalogPath.cs  # §19 candidate host surface
  - src/Host/ChemWarfareSaveStore.cs  # §19 candidate host surface
  - src/Host/CoreDemoSession.cs  # §19 candidate host surface
  - src/Host/DynamicQuestSaveStore.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/bounty_board.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/currents.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Culture/
  - godot --headless --path . -- --communique-board-selftest
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
