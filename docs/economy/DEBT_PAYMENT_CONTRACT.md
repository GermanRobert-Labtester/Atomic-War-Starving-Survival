# Plan 40 — Payment Contract

## Payment Model
- **Full payment only**: `PayContract(debtorId, day)` sets `paid = true`
- **No partial payments**: binary paid/unpaid
- **No late fees**: when `daysRemaining` hits 0, `forfeited = true`
- **Post-forfeit payment allowed**: "paying the named good back is the honoured path"

## Payment Semantics
- `PayContract()` requires `signed == true` and `paid == false`
- Sets `paid = true`, `forfeited = false`
- Fires `OnContractPaid` event
- Contract moves to `closedContracts` on next `PresentContract()` call

## Early Repayment
- Allowed: `PayContract()` works any time after signing
- No prepayment penalty: total owed is `principal × (1 + rate)` regardless of timing
- Collateral returned on repayment (host layer responsibility)

## Overpayment
- Not applicable: `PayContract()` is all-or-nothing
- No partial payment means no overpayment scenario
