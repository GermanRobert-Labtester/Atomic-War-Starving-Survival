# PLAN-ECONOMY-LEDGER-TRUTH-96 — Cross-System Trade Ledger Reconciliation

**Wave 8 · Kind:** GAP SEALING · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-REFERENCE-INTEGRITY-34, PLAN-INVENTORY-CONSERVATION-93, PLAN-DATA-CONSUMER-22.
**Implementation scaffold:** [`PLAN-ECONOMY-LEDGER-TRUTH-96_APPENDIX-A_SCAFFOLD.md`](PLAN-ECONOMY-LEDGER-TRUTH-96_APPENDIX-A_SCAFFOLD.md) — paired with `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` as scaffolding authority: source inventory, data bindings, host attachment, and a package-derived test skeleton (generated; no production files created).

**Non-goals:** no second economy, no restock ledger work — `CF-P5-RESTOCK-RECONCILE`
(DEC-05) owns that seam and its paths are integrator-owned; this plan must not
touch them. No new counter becomes authority.

## 1. Outcome
`Economy/` implements market orders, barter, caravan trade, black-market
settlement, and routes through many types: `BlackMarketSettlementService`,
`CaravanTradeNetworkSystem`, `CommodityBaselineCatalog`,
`EconomyMarketRumorRules`, `EconomyWeatherShockRules`, `FactionStanceEngine`,
plus ten host-unreachable engines (`ChitPurityAssayEngine`,
`RestockAllocationEngine`, `TradeRouteMonopolyEngine`,
`TradeRouteRiskBindingEngine`, `SurvivorBarterSystem`,
`MigrationConsequenceEngine`, `BlackMarketHeatAttentionEngine`,
`BlackMarketContrabandEngine`, `LoanSharkEnforcerEngine` and the settlement
pair). The save section `economy` (`SaveEconomy`/`SetupEconomy`) is the single
persistence seam. The gap is **cross-system reconciliation**: a trade must
produce matching cash, stock, and order effects with one accountable owner per
arithmetic fact.

| Deliverable | Detail |
|---|---|
| Ledger map | every arithmetic fact (cash, stock, order qty, debt, heat) with its single owner |
| Reconciliation harness | dev/test wrapper: before/after snapshots across a scripted trade; deltas must balance or name the mismatch |
| Cross-checks | order fill vs inventory delta (with Plan 93), cash delta vs price catalog, debt vs enforcer state |
| Orphan linkage | the ten `Economy/` orphans mapped to their intended consumer/producer rows (Plan 1 wiring) |
| Finding route | mismatches become findings for the owning system; no ad-hoc compensating counter |

## 2. Evidence
- `Assets/Ashfall.Core/Economy/` file list (premise; re-verified per package).
- Plan 1 Appendix A: the ten engines are host-unreachable today.
- `SaveSectionRegistry.All`: `("economy", "SaveEconomy", "SetupEconomy", "economy", "Dynamic economy rates and market orders")`.
- `Ashfall.Core.Tests/Economy/` exists as a focused target (premise to re-check per package).
- DEC-05/CF-P5 boundary: restock ledger paths are integrator-owned and explicitly out of scope.

## 3. Packages
- **ELT-96A** ledger map doc (owner per fact; no code).
- **ELT-96B** reconciliation wrapper + snapshot helper (test-only).
- **ELT-96C** scripted trade scenarios: market order, barter, caravan leg, black-market lot, loan repayment.
- **ELT-96D** orphan linkage notes feeding Plan 1 (and Plan 44's appendix).
- **ELT-96E** mismatch report format: fact, expected, actual, owner file.

## 4. Acceptance & verification
- All scripted scenarios balance to zero unexplained delta.
- Any injected mismatch is reported with the owning file named.
- `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/`.

## 5. Risks
Overlapping with CF-P5 → the non-goal is explicit and the claim must exclude
restock ledger paths. Snapshot overhead → test-only helper, never runtime.

---

## 6. Expanded census (9 files · 3,923 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 3 · System 5

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BlackMarketContrabandEngine.cs` | 267 | System | — | 0 | 0 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | System | — | 0 | 0 | 4 |
| `BlackMarketInventoryCatalog.cs` | 358 | Catalog | — | 0 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | Support | — | 0 | 0 | 0 |
| `BlackMarketSystem.cs` | 893 | System | — | 0 | 0 | 4 |
| `EconomyMarketRumorRules.cs` | 41 | Support | — | 0 | 0 | 0 |
| `FundsLedger.cs` | 227 | Support | **yes** | 0 | 0 | 2 |
| `MarketSystem.cs` | 1198 | System | **yes** | 0 | 0 | 4 |
| `TradeCreditCoordinator.cs` | 277 | System | — | 0 | 0 | 0 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `ledger_debt_templates.json` | object[3 keys] |
| `black_market_inventory.json` | array[7] |
| `bunker_trade_ledger_batch_2.json` | object[4 keys] |
| `trade_ledgers_expansion.json` | object[4 keys] |

**State surfaces:** `BlackMarketHeatAttentionEngine.cs`, `BlackMarketSystem.cs`, `FundsLedger.cs`, `MarketSystem.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 44 name references across the test tree |
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

Computed across 15 domain files: **9 type-reference edges**.

| File | Lines | In-degree | Out-degree |
|---|---:|---:|---:|
| `MarketSystem.cs` | 1198 | 2 | 0 |
| `BlackMarketSystem.cs` | 893 | 1 | 4 |
| `CaravanTradeNetworkSystem.cs` | 670 | 0 | 1 |
| `SurvivorBarterSystem.cs` | 666 | 0 | 0 |
| `ShelterBarterSystem.cs` | 611 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | 0 | 1 |
| `BlackMarketInventoryCatalog.cs` | 358 | 3 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | 0 | 0 |
| `TradeCreditCoordinator.cs` | 277 | 0 | 0 |
| `BlackMarketContrabandEngine.cs` | 267 | 0 | 2 |

**Highest-coupling files (in×2 + out):**

- `BlackMarketInventoryCatalog.cs` — in 3, out 0
- `BlackMarketSystem.cs` — in 1, out 4
- `FundsLedger.cs` — in 2, out 0
- `MarketSystem.cs` — in 2, out 0
- `BlackMarketContrabandEngine.cs` — in 0, out 2
- `CaravanTradeRouteCatalog.cs` — in 1, out 0
- `BlackMarketSettlementService.cs` — in 0, out 1
- `CaravanTradeNetworkSystem.cs` — in 0, out 1

**Ordering implication:** high in-degree files are depended upon — verify or seal
them first. High out-degree files are consumers whose claims should land after
their dependencies; a file with both is the domain's hub and needs its own
bounded package.

---

## 12. Cross-plan coupling

Domain files: 15. Other plans referencing their names: **12**.
**Incoming plan edges (top 8):**

| Plan | Domain-file mentions |
|---|---:|
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 15 |
| `PLAN-TRADE-TELL-TRUTH-248` | 11 |
| `PLAN-CONTRACT-BOARD-109` | 9 |
| `PLAN-CRIME-SYNDICATES-44` | 8 |
| `PLAN-ORPHAN-SEAL-01` | 7 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 7 |
| `EVIDENCE` | 4 |
| `PLAN-NOMADS-CARAVAN-CULTURE-82` | 4 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidate files |
|---|---|
| `ELT-96A` | `FundsLedger.cs` |
| `ELT-96B` | no name match — resolve at claim time |
| `ELT-96C` | `BlackMarketContrabandEngine.cs`, `BlackMarketHeatAttentionEngine.cs`, `BlackMarketInventoryCatalog.cs` |
| `ELT-96D` | no name match — resolve at claim time |
| `ELT-96E` | no name match — resolve at claim time |

**Reading:** incoming edges are coordination risk — a claim here should land
before or with those plans, or explicitly wait. Candidate files are a starting
point for the touch map, not a decision; the claim confirms each file at
premise check.

---

## 13. Authority binding map

Symbols used: 13. Host files: **21** · Test files: **44** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 21 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/BlackMarketHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/ContrabandStashSelfTest.cs`, `src/Host/EconomyHostSession.cs` |
| Tests (`Ashfall.Core.Tests/`) | 44 | `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/BalanceTelemetryHarnessTests.cs`, `Ashfall.Core.Tests/CampaignContinuityFlagship54_57Tests.cs`, `Ashfall.Core.Tests/CollectibleMerchantSimulationTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/black_market_inventory.json`, `Assets/StreamingAssets/Data/commodity_baselines.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **32** (matched by
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
| `holdfast_trade` |
| `inventory` |
| `piezometer_network` |
| `route_infrastructure` |

**Verdict:** persisted state is registered under the keys above; confirm capture/restore pairing at claim time.

---

## 15. CLI and selftest coverage

Matching flags in `HostCliRegistry.cs`: **28** (matched by domain keyword
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
| `--holdfast-trade-save-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **13**.

| Event | First declaration |
|---|---|
| `OnBarterOnlyModeChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |
| `OnLastSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |
| `OnLedgerCalibrated` | `Assets/Ashfall.Core/DoseLedgerSystem.cs` |
| `OnLedgerTampered` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnShelterEncounterResolved` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterEncounterStarted` | `Assets/Ashfall.Core/DutyRoster/ShelterEncounterSystem.cs` |
| `OnShelterFalseAlarm` | `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs` |
| `OnSurvivorDied` | `Assets/Ashfall.Core/Survivors/SurvivorCatalog.cs` |
| `OnSurvivorExposed` | `Assets/Ashfall.Core/Shelter/ShelterFireHazardSystem.cs` |
| `OnSurvivorFate` | `Assets/Ashfall.Core/Survivors/SurvivorFateSystem.cs` |

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
| `Assets/StreamingAssets/Data/deep_lore_survivor_fields.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/expansion_survivor_fields.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/ledger_debt_templates.json` |

**Verdict:** authored data exists for this domain; validate schema and consumer wiring at claim time.

---

## 18. Test-region mapping

Test regions (top-level directories under `Ashfall.Core.Tests/`) whose name
shares a domain token: **3** (143 files, 1197 cases).

| Region | Files | Cases |
|---|---:|---:|
| `Economy` | 41 | 329 |
| `Inventory` | 15 | 114 |
| `Shelter` | 87 | 754 |

**Verdict:** 1197 cases sit under matching regions — run those first (`Economy`, `Inventory`, `Shelter`).

---

## 19. Host-surface route map

Host files (`src/`) whose names share a domain token: **108**
(27 of them panels/HUD).

| Host file |
|---|
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
| `src/Host/ContrabandSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **32**, of which versioned-ladder sections:
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
| `holdfast_trade` | no |
| `inventory` | no |

**Verdict:** matched sections include versioned ladders — a schema change here must extend the existing ladder, not fork it.

---

## 21. Determinism / RNG stream binding

Matching seeded streams in `Random/CampaignRngStream.cs`: **7**.

| Stream |
|---|
| `black_market_bounty` |
| `black_market_debt_event` |
| `black_market_stock` |
| `economy` |
| `route_engineering_mine_flail` |
| `route_engineering_rail_grinding` |
| `shelter` |

**Verdict:** the domain draws from seeded streams above — replay is deterministic if every draw uses them and none uses `System.Random`.

---

## 22. Content-consumption classification

Matching catalogs in `artifacts/content-utilization-baseline.json`: **32**
(CODEX_ONLY 12, GAMEPLAY_CONSUMED 10, OPTIONAL 4, UNRESOLVED 6).

| Catalog | Classification |
|---|---|
| `antigravity_survivor_fields.json` | OPTIONAL |
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `deep_lore_survivor_fields.json` | OPTIONAL |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `expansion_survivor_fields.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `ledger_debt_templates.json` | UNRESOLVED |
| `narrative/bunker_contraband_barter.json` | CODEX_ONLY |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |

**Verdict:** 6 matched catalog(s) are UNRESOLVED (surveyed but no confirmed consumer) — check the owning loader before assuming the data is reachable.

---

## 23. Flag wiring

Matching persistent flags: **1**.

| Flag |
|---|
| `flag_expelled_survivor` |

**Verdict:** the domain writes/reads the flags above; confirm every one has a reader, and every written flag has a defined owner.

---

## 24. Claim readiness

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 12
**Surface:** save sections 32 (laddered 1) · RNG streams 7 · host files 20 · catalogs 22 · test regions 3 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-ECONOMY-LEDGER-TRUTH-96
wave: 8
status: PROPOSED — foreman claim required
packages: ELT-96A, ELT-96B, ELT-96C, ELT-96D, ELT-96E
claim paths:
  - src/Audio/AudioStateCoordinator.cs  # §19 candidate host surface
  - src/Audio/ShelterAcousticBridge.cs  # §19 candidate host surface
  - src/Audio/ShelterAudioController.cs  # §19 candidate host surface
  - src/Audio/ShelterOperationsAudioBridge.cs  # §19 candidate host surface
  - Assets/StreamingAssets/Data/affliction_bridge_rules.json  # §17 catalog (verify schema + consumer)
  - Assets/StreamingAssets/Data/antigravity_survivor_fields.json  # §17 catalog (verify schema + consumer)
verification:
  - bash scripts/run_test.sh Ashfall.Core.Tests/Economy/
  - godot --headless --path . -- --black-flotilla-selftest
dependencies:
  - coordinate: 12 other plan(s) name these artifacts (§12)
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
