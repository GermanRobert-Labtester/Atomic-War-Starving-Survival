# PLAN-ECONOMY-DATA-FAMILY-TRUTH-270 — Trade Catalogs, Types & Event Results

**Wave 19 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-COMMODITY (Plan 96 map), PLAN-DATA-SCHEMA-COVERAGE-90.
**Non-goals:** no price formation change; the family is audited for catalog and
type wiring.

## 1. Outcome
**19 `Economy/` files** are referenced by no plan: `BiologicalTradeItem`,
`CaravanAtomicTrader`, `CaravanTradeRouteCatalog`, `GoodsCatalog`,
`FactionEventResults`, `FactionStanceTypes`, `HardcoreEconomyEnums`, and
loaders. Types without a consuming system become dead vocabulary; catalogs
without loaders become inert data.

| Deliverable | Detail |
|---|---|
| Type consumption | each type is used by a named system or flagged dead |
| Catalog→loader map | route/goods catalogs resolve through their loaders |
| Event results | `FactionEventResults` routed to the faction owners |
| Enum coverage | enums parsed by at least one consumer; unknown values fail typed |
| Test presence | fixtures for type round-trips and catalog resolution |

## 2. Evidence
- 19 `Economy/` basenames absent from every plan body (Wave 19 file-level audit).
- Plan 96 owns the arithmetic map; this plan verifies vocabulary and data.
- Plan 34's `faction_` and `item_` families cover the id spaces.

## 3. Packages
- **EDF-270A** type-consumption table + dead report.
- **EDF-270B** catalog→loader tests.
- **EDF-270C** event-result routing tests.
- **EDF-270D** enum parse/typed-failure fixtures.

## 4. Acceptance & verification
- Every type has a consumer or a dead verdict; catalogs resolve; unknown enum values fail.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`.

## 5. Risks
Dead vocabulary → consumption table is the guard.
Enum drift → typed failure on unknown values.

---

## 6. Expanded census (48 family files · 14,193 lines)

Scope: files under `Assets/Ashfall.Core/Economy/` whose basename is referenced
by no plan body (the Wave 19 family definition). Class distribution: System 20 · Support 18 · Catalog 5 · Loader 2 · DTO/Type 2 · Demo 1.

| File | Lines | Class | Banned refs | Empty catches | Capture/Restore |
|---|---:|---|---:|---:|---:|
| `BiologicalTradeItem.cs` | 23 | Support | 0 | 0 | 0 |
| `BlackMarketContrabandEngine.cs` | 267 | System | 0 | 0 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | System | 0 | 0 | 4 |
| `BlackMarketInventoryCatalog.cs` | 358 | Catalog | 0 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | Support | 0 | 0 | 0 |
| `BlackMarketSystem.cs` | 893 | System | 0 | 0 | 4 |
| `CaravanAtomicTrader.cs` | 162 | Support | 0 | 0 | 6 |
| `CaravanCatalogLoader.cs` | 143 | Loader | 0 | 0 | 0 |
| `CaravanTradeNetworkSystem.cs` | 670 | System | 0 | 0 | 2 |
| `CaravanTradeRouteCatalog.cs` | 68 | Catalog | 0 | 0 | 0 |
| `ChitPurityAssayEngine.cs` | 228 | System | 0 | 0 | 0 |
| `CommodityBaselineCatalog.cs` | 252 | Catalog | 0 | 0 | 0 |
| `EconomyHeadlessDemo.cs` | 103 | Demo | 0 | 0 | 3 |
| `EconomyMarketRumorRules.cs` | 41 | Support | 0 | 0 | 0 |
| `EconomyWeatherShockRules.cs` | 56 | Support | 0 | 0 | 0 |
| `FactionEventResults.cs` | 59 | Support | 0 | 0 | 0 |
| `FactionStanceEngine.cs` | 174 | System | 0 | 0 | 0 |
| `FactionStanceTypes.cs` | 57 | DTO/Type | 0 | 0 | 0 |
| `FundsLedger.cs` | 227 | Support | 0 | 0 | 2 |
| `GoodsCatalog.cs` | 356 | Catalog | 0 | 0 | 0 |
| `HardcoreEconomyEnums.cs` | 37 | Support | 0 | 0 | 0 |
| `HardcoreEconomyTuning.cs` | 258 | Support | 0 | 0 | 0 |
| `HardcoreEconomyTuningDto.cs` | 78 | DTO/Type | 0 | 0 | 0 |
| `HardcoreEconomyTuningLoader.cs` | 148 | Loader | 0 | 0 | 0 |
| `IEconomyInterfaces.cs` | 45 | Support | 0 | 0 | 0 |
| `LoanSharkEnforcerEngine.cs` | 442 | System | 0 | 0 | 4 |
| `MarketSystem.cs` | 1198 | System | 0 | 0 | 4 |
| `MercenarySystem.cs` | 358 | System | 0 | 0 | 2 |
| `MigrationConsequenceEngine.cs` | 144 | System | 0 | 0 | 2 |
| `RegionalPriceAtlas.cs` | 346 | Support | 0 | 0 | 0 |
| `RegionalSupplyRouter.cs` | 275 | Support | 0 | 0 | 0 |
| `ResourceRationingSystem.cs` | 631 | System | 0 | 0 | 2 |
| `RestockAllocationEngine.cs` | 245 | System | 0 | 0 | 0 |
| `SeasonalHumanMigrationEngine.cs` | 162 | System | 0 | 0 | 2 |
| `ShelterBarterSystem.cs` | 611 | System | 0 | 0 | 2 |
| `SurvivorBarterSystem.cs` | 666 | System | 0 | 0 | 2 |
| `TradeCreditCoordinator.cs` | 277 | System | 0 | 0 | 0 |
| `TradeEmbargoSystem.cs` | 709 | System | 0 | 0 | 2 |
| `TradeRouteContract.cs` | 245 | Support | 0 | 0 | 2 |
| `TradeRouteMonopolyEngine.cs` | 206 | System | 0 | 0 | 2 |

… and 8 more family files in the same scope.

**Census totals:** 0 banned nondeterministic references · 0 empty-catch sites · 18 files with capture/restore methods.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `hardcore_economy_tuning.json` | object[5 keys] |
| `economy_goods.json` | object[2 keys] |
| `radiation_economy_social.json` | object[4 keys] |

**State surfaces (capture/restore present):**

- `BlackMarketHeatAttentionEngine.cs`
- `BlackMarketSystem.cs`
- `CaravanAtomicTrader.cs`
- `CaravanTradeNetworkSystem.cs`
- `EconomyHeadlessDemo.cs`
- `FundsLedger.cs`
- `LoanSharkEnforcerEngine.cs`
- `MarketSystem.cs`
- `MercenarySystem.cs`
- `MigrationConsequenceEngine.cs`
- `ResourceRationingSystem.cs`
- `SeasonalHumanMigrationEngine.cs`

## 8. Expanded verification

| Check | Baseline to establish at claim time |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Family files referenced by tests | 167 name references across the test tree |
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
(40 files). Other plans referencing those names: **17**.

**Incoming plan edges (top 8):**

| Plan | Family-file mentions |
|---|---:|
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 21 |
| `PLAN-TRADE-TELL-TRUTH-248` | 15 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 14 |
| `PLAN-ORPHAN-SEAL-01` | 13 |
| `EVIDENCE` | 11 |
| `PLAN-CONTRACT-BOARD-109` | 11 |
| `PLAN-CRIME-SYNDICATES-44` | 10 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 6 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `EDF-270A` | no name match — resolve at claim time |
| `EDF-270B` | `CaravanCatalogLoader.cs`, `BlackMarketInventoryCatalog.cs`, `CaravanTradeRouteCatalog.cs` |
| `EDF-270C` | `FactionEventResults.cs` |
| `EDF-270D` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk; candidate files are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 40; intra-domain edges: **20**; isolated files:
**20**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BlackMarketContrabandEngine` | `FundsLedger` |
| `BlackMarketInventoryCatalog` | `GoodsCatalog` |
| `BlackMarketSettlementService` | `BlackMarketSystem` |
| `BlackMarketSystem` | `BlackMarketInventoryCatalog` |
| `BlackMarketSystem` | `MarketSystem` |
| `EconomyHeadlessDemo` | `MarketSystem` |
| `EconomyWeatherShockRules` | `MarketSystem` |
| `GoodsCatalog` | `RegionalSupplyRouter` |
| `HardcoreEconomyTuningLoader` | `HardcoreEconomyTuningDto` |
| `LoanSharkEnforcerEngine` | `FundsLedger` |
| `MarketSystem` | `CommodityBaselineCatalog` |
| `MarketSystem` | `GoodsCatalog` |
| `MarketSystem` | `RegionalPriceAtlas` |
| `MarketSystem` | `RegionalSupplyRouter` |
| `MarketSystem` | `TradeEmbargoSystem` |
| `MigrationConsequenceEngine` | `SeasonalHumanMigrationEngine` |
| `RegionalPriceAtlas` | `RegionalSupplyRouter` |
| `RegionalSupplyRouter` | `GoodsCatalog` |
| `TradeEmbargoSystem` | `RegionalSupplyRouter` |
| `TradeRouteMonopolyEngine` | `TradeRouteContract` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `RegionalSupplyRouter` | 4 |
| `GoodsCatalog` | 3 |
| `MarketSystem` | 3 |
| `FundsLedger` | 2 |
| `BlackMarketInventoryCatalog` | 1 |
| `BlackMarketSystem` | 1 |
| `CommodityBaselineCatalog` | 1 |
| `HardcoreEconomyTuningDto` | 1 |
| `RegionalPriceAtlas` | 1 |
| `SeasonalHumanMigrationEngine` | 1 |

**Class split:** hub 7 · sink 5 · source 8 · isolated 20.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 40. Host files: **32** · Test files: **78** · Data files: **3**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 32 | `src/Economy/EconomyMarketPanel.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/BlackMarketHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 78 | `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/BalanceTelemetryHarnessTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs` |
| Data (`StreamingAssets/Data/`) | 3 | `Assets/StreamingAssets/Data/black_market_inventory.json`, `Assets/StreamingAssets/Data/commodity_baselines.json`, `Assets/StreamingAssets/Data/trade_embargoes.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **37** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `black_projects_archive` |
| `caravan` |
| `caravan_trade_network` |
| `contraband_stash` |
| `dose_ledger` |
| `economy` |
| `expanded_shelter` |
| `faction_espionage` |
| `holdfast_trade` |
| `host_event` |
| `inventory` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **34** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--black-flotilla-selftest` |
| `--caravan-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--deep-coast-route-selftest` |
| `--dose-ledger-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--faction-communique-board-selftest` |
| `--faction-ecology-selftest` |
| `--holdfast-trade-save-selftest` |
| `--ice-road-tick-demo` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **35**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnBaselineCorrected` | `Assets/Ashfall.Core/CohortSystem.cs` |
| `OnCombatEvent` | `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs` |
| `OnConsequenceApplied` | `Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs` |
| `OnConsequenceDispatched` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnEmbargoAdded` | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` |
| `OnEmbargoRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/antigravity_survivor_fields.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/cohort_tuning.json` |
| `Assets/StreamingAssets/Data/commodity_baselines.json` |
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/expansion_item_tags.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **5** (146 files, 1228 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |
| `Inventory` | 15 | 114 |
| `NarrativeConsequence` | 1 | 20 |
| `Shelter` | 87 | 754 |
| `Weather` | 2 | 11 |

**Verdict:** 1228 cases sit under matching regions — run those first (`Economy`, `Inventory`, `NarrativeConsequence`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **153**
(49 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioEventBridge.cs` |
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Audio/ShelterAcousticBridge.cs` |
| `src/Audio/ShelterAudioController.cs` |
| `src/Audio/ShelterOperationsAudioBridge.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CaravanSaveStore.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **37**, of which versioned-ladder sections:
**1**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `contraband_stash` | no |
| `dose_ledger` | yes |
| `economy` | no |
| `expanded_shelter` | no |
| `faction_espionage` | no |
| `holdfast_trade` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **9**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `shelter` |
| `weather` |
| `wildlife_migration` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **65**
(CODEX_ONLY 22, GAMEPLAY_CONSUMED 26, OPTIONAL 6, UNRESOLVED 11).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `expansion_item_tags.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `faction_lore.json` | GAMEPLAY_CONSUMED |
| `faction_radio_corpus.json` | GAMEPLAY_CONSUMED |
| `faction_territory.json` | UNRESOLVED |

**Verdict:** 11 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

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

**Readiness:** READY (12/12) · **Class:** hub · **Coupling (incoming plans):** 17
**Surface:** save sections 37 (laddered 1) · RNG streams 9 · host files 24 · catalogs 22 · test regions 5 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ECONOMY-DATA-FAMILY-TRUTH-270
wave: 19
status: PROPOSED — foreman claim required
packages: EDF-270A, EDF-270B, EDF-270C, EDF-270D
claim paths:
  - src/Audio/AudioEventBridge.cs  # §19 candidate host surface
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --black-flotilla-selftest
dependencies:
  - coordinate: 17 other plan(s) name these artifacts (§12)
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
