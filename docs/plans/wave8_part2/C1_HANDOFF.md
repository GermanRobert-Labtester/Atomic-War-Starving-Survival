# Wave 8 Part 2 C1 — Implementation Handoff

1. **Terminal state:** `IMPLEMENTED`.
2. **Premise:** Plan 211 owned black-market stock/pricing/debt effects, but player Buy/Sell/Loan/Repay lacked signed funds/goods semantics and the panel was read-only.
3. **Current commit/worktree:** implementation based on `276a359872facedc24fc6632e9156a688c638d7a`; intentionally dirty shared worktree preserved.
4. **Claims used:** `claim-wave8-part2-c1-black-market-actions-2026-09-17`; exact paths are recorded in `WORKTREE_OWNERSHIP.md`.
5. **Decision record:** `C1_DECISION.md`; user/foreman signed immediate canonical-inventory delivery on 2026-09-17.
6. **Files changed:** grouped with reasons and verification in `C1_CHANGE_MATRIX.md`.
7. **Behavior before:** panel displayed contacts, stock, prices, heat, trust, and debt; its host had no action settlement; Core trade calls changed only black-market state.
8. **Behavior after:** player actions route panel → host → stateless settlement service → existing black-market/wallet/inventory owners. Typed authoritative results drive feedback and disabled reasons.
9. **Persistence impact:** no schema or section added. Existing black-market, Holdfast wallet, and inventory owners capture their state; round-trip and overdue exactly-once tests pass.
10. **Determinism impact:** no RNG added. UI open/refresh cannot advance daily stock, interest, or overdue state. Daily seeded stock ownership is unchanged.
11. **UI impact:** stock rows now offer quantity, Buy, and Sell; the credit card offers loan amount/duration; active debts offer Repay. Disabled reasons are text, focus is restored after commands, and close/back behavior remains.
12. **Focused tests and counts:** settlement 18/18; panel contracts 3/3; Plan 211 Core 19/19; host wiring 3/3; inventory ownership 7/7; Holdfast trade 3/3; Plan 14 rematch 7/7; economy 184/184.
13. **Selftests:** data integrity PASS (332, 0 errors); asset registry 55/55; panel lifecycle 17/17; UI accessibility 5/5.
14. **Build result:** Core build and Godot host build PASS with 0 warnings and 0 errors.
15. **Generated-artifact checks:** asset registry regenerated from current inputs and checked, 0 missing; docs index regenerated and PASS with 2,406 documents verified.
16. **Snapshot/a11y/lifecycle evidence:** `black_market_default` MATCH and visually reviewed at 1280×800; a11y/lifecycle pass. Aggregate snapshot truth remains 31 unrelated drifts. Existing D3 shutdown warning signature remains routed.
17. **Debt/docs updated:** Plan 211 closeout, integration ledger, snapshot manifest/coverage, and the five C1 evidence records.
18. **Known remaining blocker:** merchant-restock priority remains unsigned; global snapshot drift needs D1 classification; RID/resource shutdown warnings need D3 lifetime triage. None blocks the implemented C1 action contract.
19. **Rollback point:** revert the C1 claimed paths as one package; no migration rollback is needed because no save schema changed. Removing the additive `diamond` definition also requires removing the action path that transfers that authored stock row.
20. **Next safe package:** D1 verification-truth reconciliation, or another disjoint signed Wave 8 Part 2 package after a fresh claim. Do not promote merchant-restock priority without its decision memo.
