# Plan 40 — Regression Matrix

## Test Coverage
- 22 existing LedgerDebt tests pass
- 21 data integrity tests pass
- Build: 0 errors, 3 pre-existing warnings

## Required Scenarios
1. ✅ Debt issuance (PresentContract + SignContract)
2. ✅ Principal transfer (via template catalog)
3. ✅ Interest accrual (flat rate: principal × (1 + rate))
4. ✅ Rounding (float arithmetic)
5. ✅ Early repayment (PayContract before due)
6. ✅ Due-boundary behavior (daysRemaining <= 0 → forfeited)
7. ❌ Partial payment (not supported by runtime)
8. ❌ Late payment (not supported by runtime)
9. ✅ Default (forfeited = true)
10. ✅ Reputation loss (standing_loss consequences)
11. ✅ Embargo (embargo_trade consequence)
12. ✅ Bounty handoff (bounty_moderate consequence)
13. ✅ Collateral seizure (collateral_seizure consequence)
14. ✅ Raid handoff (raid_severe consequence)
15. ❌ Labor obligation (consequence exists, not wired to templates)
16. ✅ Delayed escalation (escalationId chains)
17. ❌ Forgiveness (consequence exists, not wired to templates)
18. ✅ Multi-debt (multiple contracts independent)
19. ✅ Save/load (CaptureState/RestoreState roundtrip)
20. ✅ Old-save compatibility (additive fields, no migration)

## Not Supported (by design)
- Partial payment: runtime is binary paid/unpaid
- Late payment: runtime goes straight to forfeited
- Labor obligation: consequence available but not wired
- Forgiveness: consequence available but not wired
