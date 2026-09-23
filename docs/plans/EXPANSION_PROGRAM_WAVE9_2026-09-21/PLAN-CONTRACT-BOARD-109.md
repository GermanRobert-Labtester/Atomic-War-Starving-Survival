# PLAN-CONTRACT-BOARD-109 — Posted Work, Deadlines, Escrow & Failure Consequences

**Wave 9 · Kind:** MAJOR EXPANSION · **Status:** PROPOSED — foreman claim required.
**Depends on:** PLAN-ECONOMY-LEDGER-TRUTH-96, PLAN-JUSTICE-LAW-37, PLAN-DATA-CONSUMER-22.
**Non-goals:** no new currency, no parallel quest system, no contract state in a
panel; the existing quest/narrative graph owns authored content.

## 1. Outcome
The economy sustains funds and credit (`FundsLedger`, `TradeCreditCoordinator`,
`MarketSystem`, `MercenarySystem`) and the narrative graph owns quests and
flags. A **contract board** — posted work with a deadline, a deposit, and a
visible failure consequence — is the missing social layer: it turns existing
systems into recurring player choices without authoring a quest per listing.

| Deliverable | Detail |
|---|---|
| Listing model | a contract row: issuer, deliverable (item/visit/kill-free), deadline day, reward, deposit, penalty; sourced from a catalog |
| Lifecycle | offered → accepted → fulfilled / failed / expired; each transition has one owner and an event fact |
| Escrow | deposit and reward move through the existing funds seam; no second ledger (Plan 96 reconciliation includes contract flows) |
| Failure consequences | typed per contract (standing loss, credit hit, future listing refusal) — never a silent expiry |
| Rotation truth | expiration is evaluated on the canonical day boundary; catch-up after load advances correctly |

## 2. Evidence
- `Economy/FundsLedger.cs`, `TradeCreditCoordinator.cs`, `MarketSystem.cs`, `MercenarySystem.cs` (existing money/credit seams; re-verify per package).
- Questline/progression systems already persist authored content; the board persists only its own listing rows.
- Plan 96 ledger map owns the arithmetic facts; this plan's escrow adds rows to that map, not a new balance.
- `SaveSectionRegistry` aliases (`CanonicalizeSectionKey`) show the convention for key retirement if a listing schema changes.

## 3. Packages
- **CTB-109A** listing catalog + schema row (validated by Plan 90's stage).
- **CTB-109B** lifecycle implementation at the existing event/save seam + transition tests.
- **CTB-109C** escrow flows through the funds seam; Plan 96 reconciliation includes contract scenarios.
- **CTB-109D** failure consequences per contract + visible notice; expiry on day boundary incl. catch-up.
- **CTB-109E** save round-trip mid-contract; expiry across load asserted.

## 4. Acceptance & verification
- Every transition in the table occurs exactly once; an expired contract never silently disappears.
- Funds reconciliation balances with contract flows included.
- Save/load mid-contract preserves deadline and escrow; catch-up expires correctly.
- `bash scripts/run_test.sh` on the economy/quest regions touched.

## 5. Risks
Contract board becoming a quest duplicator → authored story stays in the narrative graph; listings are systemic and catalog-driven.
Escrow leak → reconciliation scenarios cover accept/refund/penalty paths.

---

## 6. Expanded census (8 files · 3,664 lines)

Scope: `Assets/Ashfall.Core/Economy/` files matching the plan's domain tokens,
plus the plan's premise file(s) marked **premise**. Class distribution:
Catalog 1 · Support 3 · System 4

| File | Lines | Class | Premise | Banned | Empty catches | Capture/Restore |
|---|---:|:---:|:---:|---:|---:|---:|
| `BlackMarketContrabandEngine.cs` | 267 | System | — | 0 | 0 | 0 |
| `BlackMarketHeatAttentionEngine.cs` | 289 | System | — | 0 | 0 | 4 |
| `BlackMarketInventoryCatalog.cs` | 358 | Catalog | — | 0 | 0 | 0 |
| `BlackMarketSettlementService.cs` | 373 | Support | — | 0 | 0 | 0 |
| `BlackMarketSystem.cs` | 893 | System | — | 0 | 0 | 4 |
| `EconomyMarketRumorRules.cs` | 41 | Support | — | 0 | 0 | 0 |
| `MarketSystem.cs` | 1198 | System | — | 0 | 0 | 4 |
| `TradeRouteContract.cs` | 245 | Support | — | 0 | 0 | 2 |

**Totals:** 0 banned refs · 0 empty catches · 4 files with capture/restore.

## 7. Expanded data & state surface

| Catalog | Shape |
|---|---|
| `bounty_board.json` | object[2 keys] |
| `black_market_inventory.json` | array[7] |

**State surfaces:** `BlackMarketHeatAttentionEngine.cs`, `BlackMarketSystem.cs`, `MarketSystem.cs`, `TradeRouteContract.cs`.

## 8. Expanded verification

| Check | Baseline |
|---|---|
| Focused region | `Ashfall.Core.Tests/Economy/` |
| Test references | 41 name references across the test tree |
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

Domain method: plan-body `.cs` enumeration.
Domain files: 10. Other plans referencing them: **9**.

**Incoming plan edges (top 8):**

| Plan | Mentions |
|---|---:|
| `PLAN-ECONOMY-DATA-FAMILY-TRUTH-270` | 10 |
| `PLAN-TRADE-TELL-TRUTH-248` | 9 |
| `PLAN-ECONOMY-LEDGER-TRUTH-96` | 8 |
| `PLAN-CRIME-SYNDICATES-44` | 6 |
| `PLAN-VERTICAL-BODY-INDUSTRY-05` | 5 |
| `PLAN-ORPHAN-SEAL-01` | 4 |
| `EVIDENCE` | 3 |
| `PLAN-UNBLOCK-03` | 1 |

**Package → candidate files (heuristic by name overlap):**

| Package | Candidates |
|---|---|
| `CTB-109A` | `BlackMarketInventoryCatalog.cs` |
| `CTB-109B` | no name match — resolve at claim time |
| `CTB-109C` | `TradeRouteContract.cs` |
| `CTB-109D` | `TradeRouteContract.cs` |
| `CTB-109E` | `TradeRouteContract.cs` |

**Reading:** incoming edges are coordination risk; candidates are a starting point for the touch map, not a decision.

---

## 11. Intra-domain reference graph (tier-2)

Domain files: 10; intra-domain edges: **3**; isolated files:
**6**.

**Top edges (by source name):**

| From | → To |
|---|---|
| `BlackMarketSettlementService` | `BlackMarketSystem` |
| `BlackMarketSystem` | `BlackMarketInventoryCatalog` |
| `BlackMarketSystem` | `MarketSystem` |

**In-degree hubs:**

| File | inbound refs |
|---|---:|
| `BlackMarketInventoryCatalog` | 1 |
| `BlackMarketSystem` | 1 |
| `MarketSystem` | 1 |
| `BlackMarketContrabandEngine` | 0 |
| `BlackMarketHeatAttentionEngine` | 0 |
| `BlackMarketSettlementService` | 0 |
| `EconomyMarketRumorRules` | 0 |
| `MercenarySystem` | 0 |
| `TradeCreditCoordinator` | 0 |
| `TradeRouteContract` | 0 |

**Class split:** hub 1 · sink 2 · source 1 · isolated 6.

**Reading:** sinks are the domain's load-bearing leaves (nothing inside depends
on them); hubs are rewiring risk — changing one fans out across the domain.
Isolated files are integration candidates: they compile but no sibling calls
them, so their value must come from the host adapter or the save path.

---

## 13. Authority binding map

Symbols used: 10. Host files: **18** · Test files: **36** · Data files: **2**.

| Layer | Count | Examples |
|---|---:|---|
| Host (`src/`) | 18 | `src/Foundry/SilentFoundryHostSession.cs`, `src/Host/BlackMarketHostSession.cs`, `src/Host/ContentUtilizationRuntimeCollector.cs`, `src/Host/EconomyHostSession.cs`, `src/Host/HoldfastTerminalPanel.cs` |
| Tests (`Ashfall.Core.Tests/`) | 36 | `Ashfall.Core.Tests/BalanceFedLoopTelemetryTests.cs`, `Ashfall.Core.Tests/BalancePowerEconomyTests.cs`, `Ashfall.Core.Tests/BalanceTelemetryHarnessTests.cs`, `Ashfall.Core.Tests/Core/DeterminismSeedSweepTests.cs`, `Ashfall.Core.Tests/Difficulty/DifficultyFullBindingTests.cs` |
| Data (`StreamingAssets/Data/`) | 2 | `Assets/StreamingAssets/Data/black_market_inventory.json`, `Assets/StreamingAssets/Data/commodity_baselines.json` |

**Verdict:** all three layers attach — surface is bound end to end.

---

## 14. Save-section ownership

Matching section keys in `Save/SaveSectionRegistry.cs`: **11** (matched by
domain keyword against snake_case keys — the registry's real join).

| Section key |
|---|
| `black_market` |
| `black_projects_archive` |
| `caravan_trade_network` |
| `contraband_stash` |
| `economy` |
| `holdfast_trade` |
| `inventory` |
| `mercenary_bounties` |
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
| `--communique-board-selftest` |
| `--contraband-selftest` |
| `--contraband-stash-selftest` |
| `--deep-coast-route-selftest` |
| `--economy-selftest` |
| `--economy-uitest` |
| `--faction-communique-board-selftest` |
| `--holdfast-trade-save-selftest` |
| `--inventory-save-selftest` |
| `--inventory-selftest` |
| `--inventory-uitest` |

**Verdict:** operator-visible coverage exists via the flags above.

---

## 16. Event-route reachability

Events whose name shares a domain token: **6**.

| Event | First declaration |
|---|---|
| `OnContractForgiven` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractPaid` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractRenegotiated` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnContractSigned` | `Assets/Ashfall.Core/LedgerDebtSystem.cs` |
| `OnEconomyChanged` | `Assets/Ashfall.Core/Economy/IEconomyInterfaces.cs` |
| `OnInventoryChanged` | `Assets/Ashfall.Core/Inventory/Inventory.cs` |

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
| `Assets/StreamingAssets/Data/bounty_board.json` |
| `Assets/StreamingAssets/Data/caravan_trade_routes.json` |
| `Assets/StreamingAssets/Data/cascade_rules.json` |
| `Assets/StreamingAssets/Data/economy_goods.json` |
| `Assets/StreamingAssets/Data/hardcore_economy_tuning.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json` |
| `Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json` |
| `Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json` |

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

Host files (`src/`) whose names share a domain token: **39**
(10 of them panels/HUD).

| Host file |
|---|
| `src/Audio/AudioStateCoordinator.cs` |
| `src/Economy/EconomyMarketPanel.cs` |
| `src/Economy/TradeScreenGodotPanel.cs` |
| `src/Host/BlackMarketHostSession.cs` |
| `src/Host/BlackMarketSaveStore.cs` |
| `src/Host/BlackProjectsArchiveSaveStore.cs` |
| `src/Host/CaravanTradeSaveStore.cs` |
| `src/Host/ContrabandSaveStore.cs` |
| `src/Host/ContrabandStashSelfTest.cs` |
| `src/Host/EconomyHostSession.cs` |
| `src/Host/EconomySaveStore.cs` |
| `src/Host/HoldfastTradeSaveStore.cs` |

**Verdict:** route exists through the host files above; confirm the panel exposes a real command and truthful state, not a placeholder.

---

## 20. Save schema ladder

Matched save sections: **11**, of which versioned-ladder sections:
**0**.

| Section key | Laddered |
|---|---|
| `black_market` | no |
| `black_projects_archive` | no |
| `caravan_trade_network` | no |
| `contraband_stash` | no |
| `economy` | no |
| `holdfast_trade` | no |
| `inventory` | no |
| `mercenary_bounties` | no |
| `route_infrastructure` | no |
| `settlement_defenses` | no |

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

Matching catalogs in `artifacts/content-utilization-baseline.json`: **19**
(CODEX_ONLY 8, GAMEPLAY_CONSUMED 7, OPTIONAL 2, UNRESOLVED 2).

| Catalog | Classification |
|---|---|
| `black_flotilla_items.json` | GAMEPLAY_CONSUMED |
| `bounty_board.json` | GAMEPLAY_CONSUMED |
| `caravan_trade_routes.json` | GAMEPLAY_CONSUMED |
| `economy_goods.json` | GAMEPLAY_CONSUMED |
| `hardcore_economy_tuning.json` | GAMEPLAY_CONSUMED |
| `narrative/bunker_contraband_barter.json` | CODEX_ONLY |
| `narrative/bunker_trade_ledger_batch_2.json` | CODEX_ONLY |
| `narrative/expedition_route_waypoint_notes_batch_2.json` | CODEX_ONLY |
| `narrative/personal_effects_inventory_batch_2.json` | CODEX_ONLY |
| `narrative/scavenger_expedition_route_notes.json` | CODEX_ONLY |

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

**Readiness:** READY (12/12) · **Class:** standard · **Coupling (incoming plans):** 9
**Surface:** save sections 11 (laddered 0) · RNG streams 6 · host files 18 · catalogs 22 · test regions 2 · flags 12

**Proposed claim block (paste into `WORKTREE_OWNERSHIP.md`):**

```
plan: PLAN-CONTRACT-BOARD-109
wave: 9
status: PROPOSED — foreman claim required
packages: CTB-109A, CTB-109B, CTB-109C, CTB-109D, CTB-109E
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
  - coordinate: 9 other plan(s) name these artifacts (§12)
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
