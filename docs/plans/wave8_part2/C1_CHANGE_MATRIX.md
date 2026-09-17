# Wave 8 Part 2 C1 — Change Matrix

| Owner | Path | Change | Reason | Verification |
|---|---|---|---|---|
| Black-market Core | `Assets/Ashfall.Core/Economy/BlackMarketSystem.cs` | Added side-effect-free action previews, typed loan/repay quotes, settlement callbacks, rollback, and post-commit state notifications | Keep policy in its existing owner while permitting atomic external legs | Plan 211 Core 19/19; settlement 18/18; economy 184/184 |
| Wallet Core | `Assets/Ashfall.Core/HoldfastTradeSession.cs` | Added checked debit/credit boundaries with rollback-safe second leg and one final notification | Reuse canonical trade value without exposing partial settlement | Settlement 18/18; trade session 3/3 |
| Settlement seam | `Assets/Ashfall.Core/Economy/BlackMarketSettlementService.cs` | Added stateless composition and stable preview/result reason IDs | Compose existing owners without another wallet, inventory, or policy store | Settlement 18/18 |
| Canonical item data | `Assets/StreamingAssets/Data/items.json` | Added the previously unresolved canonical `diamond` definition | Make every authored black-market stock item deliverable to canonical inventory | Real-catalog resolution test; data integrity PASS |
| Generated asset registry | `artifacts/asset_registry.json`, `artifacts/asset_registry.md` | Regenerated from current data inputs | Keep generated asset truth synchronized | generator PASS; asset registry selftest 55/55 |
| Host adapter | `src/Host/BlackMarketHostSession.cs` | Bound settlement owners; exposed previews/commands, wallet/item reads, and one action-result event | Route UI input and authoritative results through the host | host wiring 3/3; panel contract 3/3 |
| Composition | `src/Main.BlackMarket.cs` | Bound canonical Holdfast wallet, inventory, catalog, and current day; opened panel through `Open()` | Make the action seam reachable from the existing route | host build; snapshot target |
| UI | `src/UI/BlackMarketPanel.cs` | Added quantity controls, Buy/Sell/Loan/Repay controls, text disabled reasons, returned outcomes, focus restoration, and symmetric unbind | Expose the signed commands without duplicating domain rules | panel contract 3/3; lifecycle 17/17; a11y 5/5; snapshot MATCH |
| Snapshot fixture/harness | `src/UI/BlackMarketSnapshotFixture.cs`, `src/UI/SnapshotHarness.cs` | Added a real-owner seeded fixture and target | Protect the new surface with deterministic visual evidence | `black_market_default` MATCH |
| Snapshot artifacts/docs | `snapshots/black_market_default.png`, `docs/ui/snapshot_manifest.json`, `docs/ui/SNAPSHOT_COVERAGE.md` | Added the approved target and corrected current coverage truth | Record the intentional surface | 1280×800 visual inspection; manifest JSON valid |
| Settlement tests | `Ashfall.Core.Tests/Economy/Plan211BlackMarketSettlementTests.cs` | Added 18 command, atomicity, save, and exactly-once tests | Falsify every signed transaction invariant | 18/18 |
| UI contract tests | `Ashfall.Core.Tests/Economy/Plan211BlackMarketPanelContractTests.cs` | Added host-routing, no-restock, and unbind source gates | Prevent policy or simulation work from moving into the panel | 3/3 |
| D1 stale-contract adjunct | `Ashfall.Core.Tests/Economy/Plan14AEconomyIntegrationTests.cs` | Rematched one Plan 14 quote expectation to the already-live Plan 56 `RegionalSupply` factor with a drift comment | The required economy suite exposed a stale assertion; production matched current contract | isolated 7/7; economy 184/184 |
| C1 evidence | `docs/plans/wave8_part2/C1_*.md` | Added premise, decision, change, acceptance, and handoff records | Make the signed blocker closure reviewable | docs-index check |
| Plan 211 truth | `docs/economy/PLAN_211_BLACK_MARKET_CLOSEOUT.md` | Added the Wave 8 action-surface closeout | Replace read-only completion truth with implemented settlement truth | docs-index check |
| Governance | `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md` | Closed the deferred action row and path claim | Keep package and ownership truth current | final diff review |

## Diff boundary note

`items.json`, both asset-registry outputs, the snapshot manifest/coverage files, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `docs/INDEX.md` already contained unrelated worktree edits. C1 changed only the rows described above and preserved the remaining worktree state. The asset registry and docs index are generator-owned full-current-input outputs.
