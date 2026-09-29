# CF-P5 — Merchant restock decision reconciliation

**Package:** `CF-P5-RESTOCK-RECONCILE` · completion-first Plan 03 / roster 02.
**Historical status:** underlying feature sealed 2026-09-17; reconciliation recorded
DONE on 2026-09-19 in `INTEGRATION_PLANS.md`; DEC-97 records subsequent ratification.
**Editorial review:** 2026-09-28. Source inspected; historical test results were not rerun.

## Outcome and authority

Align the queue ledger, signed decision and source memo with DEC-05 Option C:
merchant priority changes evaluation/display order without adding a second
merchant or inventory authority. This package is a documentation reconciliation.
It does not authorize new stock allocation, pricing, schema or gameplay changes.

Authorities:

- [Decision register](../governance/DECISION_REGISTER.md): DEC-05 remains SIGNED;
  DEC-97 records reconciliation and ratification.
- [Source decision](wave9_part2/C1_DECISION.md): Option C and its existing signature.
- [Integration ledger](../../INTEGRATION_PLANS.md): CF-P5 completion record.

## Current source contract

| Concern | Existing owner and behavior |
|---|---|
| Priority score | `ShelterBarterSystem.ComputeItemPriorityScore`: authored `price_multiplier_bp` plus an optional item scorer. This calculation draws no RNG. |
| Ordering | `GetPrioritizedStock`: descending score, then ascending ordinal item ID; returns a sorted copy. |
| Player display | `src/UI/ShelterBarterPanel.cs` consumes `GetPrioritizedStock`. |
| Host | `src/Main.ShelterBarter.Integration.cs` constructs the canonical barter owner and captures its state; this replaces the historical `Main.Plans147.cs` path. |
| Persistence | Existing `shelter_barter` section through `ShelterBarterSaveStore`; no new section belongs to CF-P5. |

No production assignment to `PriorityScorer` was found in `src/` during this
review. An optional tested hook must not be described as a live scarcity binding.

### Later allocation work must remain distinct

The September 19 plan described priority ordering on the restock path. Current
`RestockCaravan` instead uses `RestockAllocationEngine.Allocate`, introduced by
[F13-C](../../.ai/plans/integrated/economy/INTEGRATED_F13C_RESTOCK_ALLOCATION_HOST_INTEGRATION.md).
It resolves capacity, groups stock by category, allocates quantities, then applies
`available_from_day`; stock remains pinned for the stay. Default capacity/par
settings preserve the legacy full-restock behavior.

Consequently, “Option C changes order only” describes CF-P5's decision, not a ban
on the separately authorized F13-C allocator. Do not restore the old restock
implementation or claim that current allocation uses `GetPrioritizedStock`.

## Reconciliation checklist

1. Keep DEC-05's signed verdict and Option C scope. Replace stale “awaiting signed
   design” wording in the relevant queue entries with the existing decision.
2. Preserve the corrected historical evidence: `Plan147RestockPriorityTests`
   contains six facts; the former 14/14 count was wrong.
3. Keep the source memo's ratification aligned with the decision register.
   Reference the existing signer/date; an editorial revision is not a signature.
4. Do not invent a debt row. The original reconciliation found no matching open
   restock-priority entry in `KNOWN_DEBT.md`.
5. Keep F13-C's later quantity-allocation authority explicit wherever the old
   description would imply that priority ordering still controls restocking.

These reconciliation actions are historical acceptance criteria, not a new queue
of unimplemented work. Governance changes remain integrator-owned.

## Evidence and verification

The September 19 record reports `Plan147RestockPriorityTests` **6/6 PASS**:
pure scoring, custom modifiers, score/ID ordering, legacy quantities, same-day
stock stability, and next-arrival behavior. Later F13-C documentation separately
reports allocation integration **4/4** and the same priority suite **6/6**.
Those are dated records, not results of this prose edit.

A future implementation change should use `bin/run-scoped-tests` for the affected
priority/allocation tests. For this revision, acceptance is source-consistent
wording, preserved decision identity, resolving links and a clean whitespace check.
