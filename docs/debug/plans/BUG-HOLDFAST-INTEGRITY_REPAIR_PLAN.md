# BUG-HOLDFAST-INTEGRITY repair plan

## 1. Bug

Holdfast trade can record rejected inventory grants, wrap stock/wallet counts,
truncate large settlement quotes, and lose integer precision in funds trades.

## 2. Reproduction

Use the shipped Holdfast catalog. Reject a seed with zero inventory capacity;
trade near integer stock/wallet limits; quote more than the result's signed-int
capacity; and trade a representable price above float's exact integer range.
New focused regressions must fail before production changes.

## 3. Root cause

`SeedInventory` writes held counts before checking `AddItem`. Standalone
`CanAdd` does not check count overflow. Buy/sell narrow long quotes by saturation;
funds variants implicitly convert long prices to float, cast back to int, and
replace nonpositive results with one. Sell lacks stock and wallet headroom checks.

## 4. Blast radius

| Surface | Impact | Risk |
|---|---|---|
| Core trade/inventory | phantom grants, cheap purchases, lost proceeds, negative counts | high |
| Preview/execute | must reject the same invalid transaction | medium |
| Holdfast runtime | regular Buy/Sell and seed path are live | high |
| Funds variants | currently called by tests only; no host binding added | bounded |
| Save | capture must contain actual holdings; no schema changes | medium |

## 5. Invariants

No mutation or success notification on rejected transactions. Successful deltas
equal the quote. Counts and balances stay nonnegative. Pricing/RNG/save ownership
stay unchanged. Shared backing inventory remains authoritative.

## 6. Repair options

A: validate ranges before narrowing or mutating, use exact integer funds prices,
and update seed bookkeeping only after accepted inventory additions.
B: widen result fields and funds balances, redesign transaction/save contracts.

## 7. Selected repair

Option A: local guards and existing failure results, with preview parity.

## 8. Why other options were rejected

Option B expands the API/save contract and does not address phantom grants by
itself. It is unnecessary for safe rejection under today's int result contract.

## 9. File impact

Exact paths are claimed under `BUG-HOLDFAST-INTEGRITY` in WORKTREE_OWNERSHIP.md.
Production changes are confined to HoldfastTradeSession.cs; one regression file.

## 10. Save/data implications

No authored-data edits, new save fields, or save version changes. Existing restore
and backing-inventory behavior will be checked with adjacent persistence tests.

## 11. Determinism implications

No RNG use added or removed. Exact integer conversion removes rounding loss.

## 12. Test plan

Run `bash scripts/run_test.sh Ashfall.Core.Tests/Economy/HoldfastTradeIntegrityTests.cs`
alone before and after changes, followed by HoldfastFundsTradeTests,
HoldfastTradeSessionTests, HoldfastTradeArbitrageTests, HoldfastTradeSaveStoreTests,
and affected command-preview/settlement targets identified from source.
Build `dotnet build Ashfall.csproj --no-restore -v minimal` and use the existing
bounded 15 FPS Holdfast runtime test for the live path. No full suite.

## 13. Implementation phases

1. Claim, failing regressions, current-source checkpoint.
2. Local repairs, focused tests, adjacent contract verification.
3. Diff review, host check, recorded results and handoff.

## 14. Rollback strategy

Revert this session's individual hunks only; preserve all pre-existing user work.

## 15. Definition of done

All reproduced defects pass regression; adjacent tests and host build/runtime
pass; exact verification and remaining scope limits appear in the implementation log.

## Verification follow-up — stale runtime resilience probe

The bounded Holdfast UI test passed all trade/save/reload assertions but failed
`quarantine`. Current evidence: HoldfastSaveStore uses FromCodec with backup
disabled and no quarantine contract; HoldfastTradeSaveStore explicitly owns
`.bak` rotation, quarantine and recovery. The test corrupts the wrong file and
looks for a base-store backup which is never created. Two isolated runs reproduce
the failure. Correct the existing test to exercise trade-store recovery, require
that checksum mutation actually changes bytes, and distinguish backup state from
the newer primary. Changing base-store architecture to satisfy the stale test is
rejected. Claim: BUG-HOLDFAST-RUNTIME-PROBE. Verification: rebuild and rerun the
same bounded 15 FPS runtime test. UI orphan/missing-asset diagnostics are separate
observations, not evidence of a trade regression.
