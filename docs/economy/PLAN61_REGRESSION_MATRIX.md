# Plan 61 — Regression Matrix

**Document Version:** 1.0.0
**Test Suite:** `Ashfall.Core.Tests/TradeScreenSeamTests.cs`, `TradeScreenPresenterSnapshotTests.cs`

---

## 1. Test Coverage Matrix

| Test Suite / Category | Covered Invariants | Verification Command | Status |
|---|---|---|---|
| **Catalog Count & Unique IDs** | All 15 scenarios load cleanly; 0 duplicate IDs; 3 original preserved. | `dotnet test --filter Scenarios_LoadAllFifteenFromData` | PASS |
| **Mathematical Fairness Contract** | Computed `PlayerOfferValue` vs `FactionAskValue` matches `ExpectedFairness` for all 15. | `dotnet test --filter Scenarios_AllScenariosMatchExpectedFairness` | PASS |
| **Archetype Roster Distribution** | Exactly 2 desperate, 2 quartermaster, 2 black market, 2 caravan, 1 debt, 1 bulk, 1 smuggler, 1 refugee. | `dotnet test --filter Scenarios_ArchetypeDistributionSatisfied` | PASS |
| **Catalog Goods Resolution** | Every item referenced in offers, demands, or scarcity resolves to `economy_goods.json`. | `dotnet test --filter Scenarios_AllItemReferencesResolveInEconomyGoods` | PASS |
| **Faction Identity Resolution** | Every `faction_id` resolves to `faction_radio_corpus.json`. | `dotnet test --filter Scenarios_AllFactionReferencesResolveInRadioCorpus` | PASS |
| **Baseline Scenario Preservation** | `fair_deal` (94 vs 44, fair), `offer_short` (18 vs 80, short), `empty_table` (0 vs 0, empty). | `dotnet test --filter TradeScreenSeamTests` | PASS |
| **Presenter Zero-Mutation** | Presenter reads providers without altering trust, aggression, or stances. | `dotnet test --filter Presenter_ZeroMutation_InvariantHolds` | PASS |
| **Selection Snapshot Round-Trip** | Selections, counts, biological items, and totals restore deterministically. | `dotnet test --filter SelectionRestoration_CaptureAndRestore` | PASS |
| **Biological Offering Schedule** | Blood (25), Marrow (50), Plasma (75), Organ (100) pricing verified. | `dotnet test --filter BiologicalOfferings_PricingCalculations` | PASS |
| **Data Integrity Self-Test** | All 208 catalogs pass zero-error check. | `godot --headless --path . -- --data-integrity-selftest` | PASS |
| **Content Utilization Self-Test** | All catalogs consumed; CI gate PASS. | `godot --headless --path . -- --content-utilization-selftest` | PASS |
| **Scene Lint** | Production scenes clean; 0 errors. | `python3 scripts/ci/scene-lint.py` | PASS |

---

## 2. Risk Mitigation Audit

1. **Risk 1 (Price Modifier Inversion):** Mitigated by explicit item `unit_price` fields in data and mathematical assertions comparing `PlayerOfferValue` against `FactionAskValue`.
2. **Risk 2 (Second Economy Engine):** Mitigated by keeping scenario data purely configuration-layer; `economy_goods.json` remains authoritative for base prices.
3. **Risk 3 (Arbitrage Loops):** Mitigated by cross-scenario price spread audit in `TRADE_ARBITRAGE_AUDIT.md`.
4. **Risk 4 (Scenario Cycling):** Mitigated by deterministic scenario selection and presentation-state separation.
5. **Risk 12 & 13 (Unresolved Forward References):** Mitigated by referencing only existing, live items (`economy_goods.json`), factions (`faction_radio_corpus.json`), and tells (`trade_tell_lines.json`).
