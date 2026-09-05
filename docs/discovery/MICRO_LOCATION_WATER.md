# Micro-Location Water Source — Water Integration (F20)

Flagship plan §10 deliverable. Proven by `Ashfall.Core.Tests.MicroLocationWaterIntegrationTests` (12 tests).

## Reward quantities (authored, deterministic)

| Choice | Grant | Morale | Notes |
|---|---|---|---|
| `collect_water` | `clean_water` ×3 | +2 | pump without testing |
| `test_water` | `clean_water` ×2 | 0 | testing costs yield — the meaningful, deterministic difference |
| `avoid_water` | nothing | 0 | non-depleting |

## Canonical water authority

- **Item:** `clean_water` (items.json, type Water, `thirstRestore` > 0, `contamination` 0).
- **Hydration path:** Core `Inventory.Consume(item, applyNeed, …)` — removes one unit, applies `thirstRestore` through the needs callback, rolls the unit back if the needs authority refuses. The Godot host's `HoldfastRuntimeSession.ConsumeWaterResult` uses exactly this contract. `F20_06`–`F20_08` prove consumption, stack decrease, hydration delta parity with any other `clean_water` source, and rollback on refusal.
- **No special micro-location water path exists or was created.**

## One-shot semantics

Both water choices deplete the whole site. Revisit grants zero: the depleted encounter is excluded from weighted selection for every stance and seed (`F20_04`), and save/reload preserves depletion (`F20_05`). The source is a **discovery reward, not a production node**.

## Scarcity protection

- No recurring daily water, no permanent producer registration, no map-respawn regeneration — the payload carries no flags or location discoveries that could gate production (`F20_09`), and depletion is permanent.
- Shelter water treatment and container capacity are downstream systems the reward never bypasses: the grant is finite inventory, consumed only through the canonical path.
- Drought weighting: the discovery selector reads **no drought/weather/scarcity context today** (same finding as F18's seasonal check). Documented per §10.10 — if drought-aware weighting is ever added, detection should improve while quantity/quality stay constrained; nothing was hard-coded.

## Contamination-risk decision

**Deferred — documented, not implemented.** The authoritative water model already distinguishes potable and contaminated items (`clean_water` vs `irradiated_water`, pinned by `F20_10`), and the disease catalog carries four water-vector diseases with `clean_water` as the authored countermeasure. Per plan §10.8, unsafe water must **not** be labelled `clean_water`; a real risk model would either swap the authored grant toward `irradiated_water` (a balance/content decision) or route a narrowly-named flag through the F17 hazard registry into a water-vector disease. Both are canonical extensions — the authored data currently grants potable water for both choices, so no risk hook was fabricated. The F17 coordinator makes the flag-based variant a two-line data change when the project owner opts in.

## Deterministic behavior

No RNG anywhere in the chain: same fixture + same choice ⇒ identical grant, morale, and depletion state (`F20_12` byte-identical traces).

## Save/load behavior

Depletion and inventory round-trip through the production serializer; restored campaigns cannot re-grant (`F20_05`).

## Tests

`F20_01`–`F20_12` in `MicroLocationWaterIntegrationTests`, plus the water slice of the shared determinism trace (`MicroLocationIntegrationDeterminismTests.Trace_*`).

## Deferred work

Unsafe-collection risk (canonical options documented above), drought-aware discovery weighting, water testing kits, survivor knowledge improving testing yield.
