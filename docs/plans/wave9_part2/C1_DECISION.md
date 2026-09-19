# Wave 9 Part 2 C1 — Merchant Restock Priority Decision

## Decision ID

`WAVE9-PART2-C1-RESTOCK-PRIORITY`

## Header

- **Source blocker (resolved):** Plan 147 completion report / PLANS-210-213 deferral; DEC-05 now ratifies Option C (deterministic display-order priority).
- **Current HEAD:** Commit `HEAD` (`ShelterBarterSystem.cs`, `MarketSystem.cs`).
- **Current owner:** `ShelterBarterSystem` (caravan arrival & restock owner); `MarketSystem` (dynamic economy / category indices owner).
- **Current measurement/behavior:** `ShelterBarterSystem.RestockCaravan` iterates `def.stock` and populates `cState.remainingStock[item.item_id] = item.quantity` (or 0 if `_state.currentDay < item.available_from_day`). Restock occurs strictly on caravan arrival (`shouldBePresent && !cState.isAtAirlock`). Once populated, stock is pinned in `_state.caravans[def.caravan_id]`. Reopening the market panel does not re-evaluate, restock, or consume RNG.

---

## Why a Decision is Required

Historical Plan 210–213 §2.3 item 6 stated:
*"Merchant stock response: CaravanTradeNetworkSystem/ShelterBarterSystem query category indices for restock priority — stock refresh cadence stays with the caravan owners."*

However, `MerchantCaravanDef` in JSON defines a fixed list of stocked items without capacity or slot caps. What "priority" means under this architecture was left ambiguous:
1. Does priority select *which* items are stocked from an oversized candidate pool (requiring an invented slot limit)?
2. Does priority scale *quantities* based on category scarcity?
3. Does priority simply sort the manifest for display?
4. Or should current behavior (stocking all authored items that have passed their day gate) be retained?

Implementing any behavior without a signed decision risks distorting economy balance, inventing unapproved slot limits, or violating deterministic stock pinning.

---

## Options Analysis

### Option A — Selection Priority with Capacity Constraints
- **Behavior:** Introduce an item slot or weight capacity to caravans. Evaluate priority score for each candidate item based on dynamic category scarcity index (`MarketSystem.GetCategoryMultiplier`) and active shocks. Highest-priority candidates fill available slots first.
- **Owner:** `ShelterBarterSystem.RestockCaravan`.
- **Persistence:** None (derived at arrival). Stock remains pinned.
- **Determinism:** Pure function, stable item ID ordinal tie-break.
- **UI/Content Impact:** Major. Caravans will carry only a subset of authored items. Player access to specific goods becomes volatile.
- **Compatibility:** Old saves load with already-pinned stock; future arrivals change mix.
- **Test Impact:** Requires testing selection sorting, capacity cutoffs, and edge cases.
- **Rollback Risk:** High balance impact; may starve players of essential survival tools.

### Option B — Quantity Scaling Priority
- **Behavior:** All authored items remain stocked, but quantities scale with category scarcity/shock index (e.g. `quantity = round(base_quantity * category_multiplier)`).
- **Owner:** `ShelterBarterSystem.RestockCaravan`.
- **Persistence:** None (derived at arrival).
- **Determinism:** Pure integer math with clamped bounds.
- **UI/Content Impact:** Moderate to high. Merchants bring more of scarce/demanded goods and less of surplus goods.
- **Compatibility:** Compatible; existing caravans carry authored base quantities when multipliers are neutral.
- **Test Impact:** Requires extensive economy balance re-sweeps.
- **Rollback Risk:** Moderate; could amplify runaway inflation or hoarding.

### Option C — Evaluation/Display Order Only
- **Behavior:** Stock quantities and item inclusion remain exactly as authored in `def.stock` (gated by `available_from_day`), but the dictionary/list exposed to the UI is ordered by category demand/priority score.
- **Owner:** `ShelterBarterSystem` / `EconomyHostSession`.
- **Persistence:** None.
- **Determinism:** Deterministic sort by (PriorityScore descending, ItemId ascending).
- **UI/Content Impact:** Low. High-priority goods appear at top of trading lists.
- **Compatibility:** Fully compatible.
- **Test Impact:** Unit test for ordering stability.
- **Rollback Risk:** Near zero.

### Option D — Retain Current Behavior (DECIDED-DEFERRED)
- **Behavior:** Retain the verified, stable Plan 147 contract: all authored items in `def.stock` are stocked when `currentDay >= available_from_day`. No artificial slot cap or quantity inflation is introduced.
- **Owner:** Unchanged (`ShelterBarterSystem`).
- **Persistence:** None.
- **Determinism:** Unchanged.
- **UI/Content Impact:** Zero change.
- **Compatibility:** 100% backward compatible.
- **Test Impact:** Existing economy and barter tests remain authoritative.
- **Rollback Risk:** Zero.

---

## Architecture-Safe Recommendation

**Option D (Retain Current Behavior / DECIDED-DEFERRED)** or **Option C (Display Ordering Only)**.
Option D is strongly recommended because `MerchantCaravanDef` manifests are curated by authors for specific gameplay balance. Introducing artificial slot cutoffs (Option A) or quantity scaling (Option B) creates unintended scarcity spikes and duplicates market price-elasticity mechanisms already handled by `MarketSystem` price adjustments.

---

## Foreman Signature Gate

- **Chosen Option:** Option C — Evaluation/Display Order Only (ratified as `DEC-05`, SIGNED 2026-09-17; wording ratified verbatim by the CF-P5 reconcile, 2026-09-19)
- **Signer:** User / Foreman (Wave 9 Part 2 authorization; recorded in `docs/governance/DECISION_REGISTER.md` row `DEC-05` and `docs/plans/wave9_part2/WAVE9_PART2_CLOSEOUT.md`)
- **Date:** 2026-09-17 (decision); ratification re-confirmed 2026-09-19 (CF-P5)
- **Conditions:**
  1. Restock occurs strictly at the arrival edge (`shouldBePresent && !cState.isAtAirlock`).
  2. Stock pinning and same-day no-reroll invariant remain absolute.
  3. No new RNG streams or save sections may be added.
