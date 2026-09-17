# Wave 8 Part 2 C1 — Acceptance Evidence

**Terminal candidate:** `IMPLEMENTED`<br>
**Premise commit:** `276a359872facedc24fc6632e9156a688c638d7a`<br>
**Decision:** `WAVE8-PART2-C1-BLACK-MARKET-SETTLEMENT`, signed 2026-09-17

## Action matrix

| Action | Core method | Currency owner | Goods owner | Existing domain effects | UI behavior | Regression evidence |
|---|---|---|---|---|---|---|
| Buy | `PreviewBuy` → `Buy` | `HoldfastTradeSession` debit | `Inventory` grant | Black-market stock decreases; existing policy unchanged | BUY plus text reason/outcome; focus returns to quantity | success, funds/stock/access/capacity failures, forced second-leg rollback |
| Sell | `PreviewSell` → `Sell` | `HoldfastTradeSession` credit | `Inventory` removal | Black-market stock increases; existing policy unchanged | SELL plus text reason/outcome; focus returns to quantity | success, missing goods, forced rollback |
| Take Loan | `PreviewLoan` → `TakeLoan` | `HoldfastTradeSession` credit | none | Existing debt, due date, and trust-on-credit behavior | amount/duration, TAKE LOAN, due-date preview, returned outcome | success, active-loan rejection, forced wallet failure rollback |
| Repay | `PreviewRepay` → `RepayDebt` | `HoldfastTradeSession` debit | none | Existing debt completion and trust-on-repay behavior | active-debt amount, REPAY, text reason/outcome | success, insufficient funds, forced debit failure rollback |

## Focused test results

| Command/target | Result |
|---|---|
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | PASS 18/18 |
| `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/Plan211BlackMarketPanelContractTests.cs` | PASS 3/3 |
| Existing Plan 211 black-market Core target | PASS 19/19 |
| Existing Plan 211 host-wiring target | PASS 3/3 |
| Unified inventory ownership target | PASS 7/7 |
| `HoldfastTradeSessionTests` | PASS 3/3 |
| Required economy directory | initial 183/184; one stale Plan 14 assertion classified and rematched; final PASS 184/184 |
| Isolated rematched `Plan14AEconomyIntegrationTests` | PASS 7/7 |

The economy failure was not a production regression. Current `MarketSystem` intentionally applies Plan 56's fallback regional-supply factor when the atlas has no explicit row. Existing Plan 56/RegionalSupply tests prove that contract; the Plan 14 test still expected the older base quote. The assertion now verifies the current `RegionalSupply` multiplier and contains a drift note.

## Persistence, exactly-once, and determinism

- Settled Buy round-trips through the existing black-market, Holdfast wallet, and inventory save owners; no new save field or section was added.
- Active loan restore followed by the overdue edge emits the overdue consequence exactly once: covered in the 18-test settlement target.
- Stock generation remains under the existing seeded daily owner. Panel source gates prohibit `EnsureStockSnapshot`, `TickDaily`, `TickDay`, or RNG use.
- Opening, closing, and reopening reads the same authoritative daily snapshot; panel refresh has no restock path.

## Build and integrity

| Gate | Result |
|---|---|
| `dotnet build Assets/Ashfall.Core/Ashfall.Core.csproj --no-restore` | PASS, 0 warnings, 0 errors |
| `dotnet build Ashfall.csproj --no-restore` | PASS, 0 warnings, 0 errors |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 332 catalogs, 0 errors; 5 registered primary-wins warnings |
| `python3 scripts/ci/generate-asset-registry.py --check` | PASS, 1,706 IDs, 508 loaded, 1,198 fallback, 0 missing |
| `godot --headless --path . -- --asset-registry-selftest` | PASS 55/55, 0 missing/load failures |
| `python3 scripts/ci/generate-docs-index.py --check` | PASS, 2,406 documents verified |

## UI evidence

| Gate | Result |
|---|---|
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | PASS 17/17 |
| `godot --headless --path . -- --ui-accessibility-selftest` | PASS 5/5; 98 controls, 345 text elements, 241 UI files |
| Real-display snapshot target `black_market_default` | MATCH, 52,303 bytes, SHA-256 `27cd88536d0f67d19e1190ebb607b302e0bc88599dfeb0b294f3dcd8fc00253a` |
| Visual review | PASS at 1280×800; actions, reason text, wallet, loan, and debt ledger visible without overflow |

The full snapshot invocation still exits nonzero because 31 unrelated repository baselines drift at current `HEAD`; the changed `black_market_default` target itself is an exact match. Those identities belong to verification-truth repair, not to C1 rebaselining. Godot also emits the already-scoped D3 RID/resource shutdown signature after panel-heavy runs; C1 introduced no global suppression or exit cleanup.

## Acceptance matrix disposition

1. Buy success and reachable Core failures: **PASS**.
2. Sell success and reachable Core failures: **PASS**.
3. Funds/goods atomicity: **PASS**.
4. Loan creation and repayment: **PASS**.
5. Overdue exactly once across restore: **PASS**.
6. Same-day reopen/no-restock: **PASS in the existing Plan 211 seeded same-day test plus the panel source regression gate**.
7. Keyboard/focus/text disabled reasons: **PASS**.
8. Dispose/unsubscribe: **PASS**.
9. Economy suite, build, integrity, asset registry, and changed snapshot: **PASS**.

## Remaining external baseline findings

- Snapshot corpus: 31 pre-existing/unrelated drift rows require a named D1 reconciliation package; C1 did not rebaseline them.
- Shutdown: panel-heavy/a11y paths still emit the known D3 resource/RID warnings and require owner-lifetime triage.
- Merchant-restock priority remains unsigned and outside C1.
