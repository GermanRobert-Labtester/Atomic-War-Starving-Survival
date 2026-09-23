# PLAN-TRADE-TELL-TRUTH-248 — Market Signals: Tells, Accuracy & Use

**Wave 18 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-RUMOR-PROPAGATION-TRUTH-120, PLAN-COMMODITY (Plan 96 rows).
**Non-goals:** no price formation (Plan 96), no rumor lifecycle (Plan 120).

## 1. Outcome
`Economy/TradeTellEngine.cs` (**214 lines**) is reachable and unaddressed:
observable market tells (patterns traders read — stockpiling, absences,
price drift). Plans 96/120 own prices and rumors; the **tell layer** that turns
ledger state into readable signals is unowned, so the market is either opaque
or magically knowable.

| Deliverable | Detail |
|---|---|
| Tell model | tells derived from ledger/stock facts (Plan 96/93) with documented accuracy bands (a tell can mislead) |
| Reading | observing a tell requires presence/information paths (Plan 120's sharing); no global knowledge |
| Use | acting on a tell is ordinary trade through Plan 96 — tells never trigger hidden prices |
| Decay | tells expire as the underlying state changes; stale tells are visibly stale |
| Save truth | known tells restore; no re-roll on load |

## 2. Evidence
- `Assets/Ashfall.Core/Economy/TradeTellEngine.cs` (214 lines; unaddressed — Wave 18 audit).
- Plan 96 owns all arithmetic; this plan reads it.
- Plan 120 supplies information paths.
- Plan 93's stock facts are a tell source.

## 3. Packages
- **TTT-248A** tell model + source/accuracy table.
- **TTT-248B** information-path gating tests.
- **TTT-248C** use-through-Plan-96 test (no hidden pricing).
- **TTT-248D** stale-tell visibility fixtures.
- **TTT-248E** save round-trip; no re-roll on load.

## 4. Acceptance & verification
- Tells derive from stored facts; accuracy matches the table over a paired sample.
- Stale tells are marked; save/load preserves the known set.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`.

## 5. Risks
Perfect information → accuracy bands and info paths gate it.
Hidden pricing → tells are read-only; the boundary test asserts it.

---

## 6. Expanded census (22 files · 7,983 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 3 · Support 9 · System 10

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BiologicalTradeItem.cs` | 23 | Support | — | 0 | 0 | 0 |
| `BlackMarketContrabandEngine.cs` | 267 | System | — | 0 | 0 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | System | — | 0 | 0 | 4 |
| `BlackMarketInventoryCatalog.cs` | 358 | Catalog | — | 0 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | Support | — | 0 | 0 | 0 |
| `BlackMarketSystem.cs` | 893 | System | — | 0 | 0 | 4 |
| `CaravanAtomicTrader.cs` | 162 | Support | — | 0 | 0 | 6 |
| `CaravanTradeNetworkSystem.cs` | 670 | System | — | 0 | 0 | 2 |
| `CaravanTradeRouteCatalog.cs` | 68 | Catalog | — | 0 | 0 | 0 |
| `EconomyMarketRumorRules.cs` | 41 | Support | — | 0 | 0 | 0 |
| `MarketSystem.cs` | 1198 | System | — | 0 | 0 | 4 |
| `TradeCreditCoordinator.cs` | 277 | System | — | 0 | 0 | 0 |
| `TradeEmbargoSystem.cs` | 709 | System | — | 0 | 0 | 2 |
| `TradeRouteContract.cs` | 245 | Support | — | 0 | 0 | 2 |
| `TradeRouteMonopolyEngine.cs` | 206 | System | — | 0 | 0 | 2 |
| `TradeRouteRiskBindingEngine.cs` | 130 | System | — | 0 | 0 | 0 |
| `TradeScreenPresenter.cs` | 468 | Support | — | 0 | 0 | 2 |
| `TradeScreenScenarios.cs` | 287 | Support | — | 0 | 0 | 0 |
| `TradeScreenSeam.cs` | 369 | Support | — | 0 | 0 | 0 |
| `TradeStance.cs` | 19 | Support | — | 0 | 0 | 0 |
| `TradeTellEngine.cs` | 214 | System | **yes** | 0 | 0 | 0 |
| `TradeTextCatalog.cs` | 717 | Catalog | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 9 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `caravan_trade_routes.json` | object[2 keys] |
| `faction_intelligence.json` | object[3 keys] |
| `black_market_inventory.json` | array[7] |
| `trade_embargoes.json` | object[4 keys] |
| `trade_screen_scenarios.json` | object[5 keys] |
| `trade_tell_lines.json` | object[6 keys] |

**State surfaces:** `BlackMarketHeatAttentionEngine.cs`, `BlackMarketSystem.cs`, `CaravanAtomicTrader.cs`, `CaravanTradeNetworkSystem.cs`, `MarketSystem.cs`, `TradeEmbargoSystem.cs`, `TradeRouteContract.cs`, `TradeRouteMonopolyEngine.cs`, `TradeScreenPresenter.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 76 name references across the test tree |
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

Domain files: 15. Other plans referencing their names: **10**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 8 |
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 5 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 3 |
| `EVIDENCE` | 3 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 3 |
| `PLAN-ORPHAN-SEAL-01` | 2 |
| `PLAN-CONTRACT-BOARD-109` | 2 |
| `PLAN-UNBLOCK-03` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `TTT-248A` | no name match — resolve at claim time |
| `TTT-248B` | no name match — resolve at claim time |
| `TTT-248C` | no name match — resolve at claim time |
| `TTT-248D` | no name match — resolve at claim time |
| `TTT-248E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 22; intra-domain edges: **14**; isolated files:
**7**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BlackMarketSettlementService` | `BlackMarketSystem` |
| `BlackMarketSystem` | `BlackMarketInventoryCatalog` |
| `BlackMarketSystem` | `MarketSystem` |
| `MarketSystem` | `TradeEmbargoSystem` |
| `TradeRouteMonopolyEngine` | `TradeRouteContract` |
| `TradeRouteRiskBindingEngine` | `TradeRouteContract` |
| `TradeScreenPresenter` | `BiologicalTradeItem` |
| `TradeScreenPresenter` | `TradeStance` |
| `TradeScreenScenarios` | `BiologicalTradeItem` |
| `TradeScreenScenarios` | `TradeStance` |
| `TradeScreenSeam` | `BiologicalTradeItem` |
| `TradeScreenSeam` | `TradeStance` |
| `TradeTellEngine` | `TradeStance` |
| `TradeTextCatalog` | `TradeStance` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `TradeStance` | 5 |
| `BiologicalTradeItem` | 3 |
| `TradeRouteContract` | 2 |
| `BlackMarketInventoryCatalog` | 1 |
| `BlackMarketSystem` | 1 |
| `MarketSystem` | 1 |
| `TradeEmbargoSystem` | 1 |
| `BlackMarketContrabandEngine` | 0 |
| `BlackMarketHeatAttentionEngine` | 0 |
| `BlackMarketSettlementService` | 0 |

**Class split:** hub 2 · sink 5 · source 8 · isolated 7.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 22. Host files: **21** · Test files: **49** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 21 | `src/Economy/EconomyMarketPanel.cs`, `src/Economy/TradeScreenGodotPanel.cs`, `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/BlackMarketHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs` |
| Tests (`Ashfall.Core.Tests/`) | 49 | `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/BalanceTelemetryHarnessTests.cs`, `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/black_market_inventory.json`, `Assets/StreamingAssets/Data/commodity_baselines.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **12** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `black_projects_archive` |
| `caravan` |
| `caravan_trade_network` |
| `contraband_stash` |
| `economy` |
| `holdfast_trade` |
| `inventory` |
| `piezometer_network` |
| `route_infrastructure` |
| `settlement_defenses` |
| `settlement_politics` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **14** (matched by domain keyword
against kebab-case flags — the registry's real join).

| Flag |
|---|
| `--black-flotilla-selftest` |
| `--caravan-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--deep-coast-route-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--holdfast-trade-save-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |
| `--port-contract-selftest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **15**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnDependencyRisk` | `Assets/Ashfall.Core/PharmaLabSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnEmbargoAdded` | `Assets/Ashfall.Core/FactionEmbargoLedger.cs` |
| `OnEmbargoRequested` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnEmbargoRequestedDetailed` | `Assets/Ashfall.Core/DebtConsequenceDispatcher.cs` |
| `OnFrostbiteRisk` | `Assets/Ashfall.Core/ShelterThermalSystem.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnItemAdded` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |

**Verdict:** the domain is reachable through the events above; confirm the host subscribes to them.

---

## 17. Data-catalog binding

Catalog JSON files whose names share a domain token: **12**.

| Catalog |
|---|
| `Assets/StreamingAssets/Data/affliction_bridge_rules.json` |
| `Assets/StreamingAssets/Data/barter_rules.json` |
| `Assets/StreamingAssets/Data/black_flotilla_items.json` |
| `Assets/StreamingAssets/Data/black_market_inventory.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/expansion_item_tags.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/item_degradation.json` |
| `Assets/StreamingAssets/Data/item_description_texts.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **2** (56 files, 443 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |
| `Inventory` | 15 | 114 |

**Verdict:** 443 cases sit under matching regions — run those first (`Economy`, `Inventory`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **44**
(11 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CaravanSaveStore.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |
| `src/Host/ContrabandSaveStore.cs` |
| `src/Host/ContrabandStashSelfTest.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **12**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan` | no |
| `caravan_trade_network` | no |
| `contraband_stash` | no |
| `economy` | no |
| `holdfast_trade` | no |
| `inventory` | no |
| `piezometer_network` | no |
| `route_infrastructure` | no |

**Verdict:** matched sections are unversioned — schema changes need only the current codec path; the five versioned ladders (holdfast, year_of_ash, dose_ledger, expansion_hub, weight_of_choices) are untouched.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **6**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **22**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 8, OPTIONAL 4, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `expansion_item_tags.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `item_degradation.json` | OPTIONAL |
| `item_description_texts.json` | OPTIONAL |
| `narrative/bunker_contraband_barter.json` | CODEX_ONLY |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |

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
**Surface:** save sections 12 (laddered 0) · RNG streams 6 · host files 18 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-TRADE-TELL-TRUTH-248
wave: 18
status: PROPOSED — foreman claim required
packages: TTT-248A, TTT-248B, TTT-248C, TTT-248D, TTT-248E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Economy/EconomyMarketPanel.cs  # §19 candidate host surface
  - src/Economy/TradeScreenGodotPanel.cs  # §19 candidate host surface
  - src/Host/BlackMarketHostSession.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/barter_rules.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --black-flotilla-selftest
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
