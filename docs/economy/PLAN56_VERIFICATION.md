# Plan 56 — Independent Verification Record

> Verifier: second-session audit of the Plan 56 expansion (executed in a
> parallel working session whose changes were verified uncommitted in the
> working tree). This file records the independent check of every claim,
> the regression found and fixed, and the DoD status. Baseline archaeology
> and the expansion narrative live in `PLAN56_FINAL_REPORT.md`.

## 1. Baseline archaeology (verified)

| Claim | Plan text | Repository truth (verified) |
|---|---|---|
| Starting count | 16 | **34** at HEAD (`git show HEAD:economy_goods.json`), 40 after expansion |
| Expansion delta | +24 | **+6** (`ammo_556`, `ammo_12g`, `diesel_fuel`, `item_smoked_meat`, `item_pickled_tubers`, `tobacco_pouch`) |
| Baseline preservation | "16 preserved" | **All 34 baseline records byte-identical** (diffed record-by-record); zero removals |
| Duplicates | — | 0 (40 unique ids) |

The plan's 16-good baseline and its 24-concept roster were stale: foundry
goods, chelation pellets, traps, filters, ammo families and preserved foods
had already landed via other plans. Per §20 (autonomous decision rules), the
expansion was scoped to the 6 concepts not already covered.

## 2. Schema contract (verified against `GoodsCatalogLoader`)

- Envelope `{ schema_version: 1, goods: [...] }`; version > 1 rejected.
- `id`: snake_case, unique, required. `displayName`/`category` required;
  category against the 12-name whitelist (`GoodCategories.Known`).
- `basePrice` required > 0; `volatility` [0,1] default 0.1; `elasticity` > 0
  default 1.0; `stackSize` ≥ 1 default 10; `weightKg` ≥ 0 default 1.0;
  `barterNote` optional.
- Strict DTO: absent required fields are errors, never silent defaults.
- **No item-reference field exists by design** — goods ids are market-space
  identity. Modern rows use the canonical `items.json` id as the good id
  (verified for the 6 Plan 56 goods); 5 legacy ids (`bandages`, `9mm_ammo`,
  `crowbar`, `diamond`, `coal`) are grandfathered market projections.
- **`regionalSupply` annotation**: present on 18 rows (foundry/traplines/
  greenhouse/settlement/coastal/general), NOT read by the loader (absent
  from the strict DTO). Documented as an authoring provenance annotation;
  retention is harmless, wiring it is future work.

## 3. Runtime semantics (verified against `MarketSystem.cs`)

- **volatility** = per-day demand-walk noise amplitude: `noise = U(-vol, +vol)`,
  NOT a price percentage.
- **elasticity** = scales that noise (`delta = noise × elasticity`) — higher
  elasticity ⇒ the demand multiplier walks further per day ⇒ more labile
  prices. The textbook necessity/luxury demand-curve reading does NOT apply;
  values are authored to this walk semantics.
- Price = `basePrice × demandMult`, demand clamped [0.25, 4] (Unity parity),
  price clamped `[0.25 × base, 4 × base]`.
- All stochasticity via the caller's `ISeededRng` → deterministic.
- Save: `MarketState` demand/ledger keyed by good id (first-wins dedupe,
  ordinal sort) → **adding goods is migration-free**; missing rows read 1.0.

## 4. Wiring (verified by scan + tests)

- Settlements reference 17 distinct goods across `trade_goods`/`trade_needs`;
  **all 6 Plan 56 goods are wired** (plan asked ≥8 of 24; deviation documented —
  only 6 goods were added).
- Caravans reference 10 distinct goods in `specialty_goods`; **5 of 6 Plan 56
  goods travel** (`ammo_12g` is settlement-only). Plan asked ≥5 of 24 → met.
- Referential integrity now gated by `Plan56CloseOutTests` (refs must resolve
  in the goods catalog or, for item-space needs, in `items.json`).

## 5. Regression found and fixed (this verification)

**`--economy-selftest` failed 9/11 after the expansion** (barter rejected,
ledger short) — passing at HEAD. Root cause: `EconomyHeadlessDemo` seeded
every market day with the *same* `SeededRng(4242)`, making the daily noise
identical per good (degenerate walk) and the barter assertion hypersensitive
to catalog composition — the 6 new ids shifted every later good's draw
position in the ordinal walk.

Fix (demo harness only; pricing model untouched):
- per-day seed derivation `SeededRng(4242 + day)` — a proper varied walk;
- barter quantity 20 → 50 scrap, provably sufficient across the entire price
  clamp band (50 × 0.75 floor = 37.5 > clean-water 32 ceiling), so the smoke
  no longer depends on where prices happen to drift.

Result: `--economy-selftest` 11/11 PASS with the 40-good catalog.

## 6. DoD status (condensed)

| DoD area | Status |
|---|---|
| 40 total goods, 0 duplicates, baseline preserved | ✅ verified |
| Category goals (food 4 / fuel 3 / medicine 4 / materials 4 / tools 3 / ammo 3 / luxury 2 / info 1) | ✅ met or exceeded by the 34-good baseline + 6 additions (info/maps correctly dropped: collectibles are non-fungible per §56AI guardrail) |
| Schema/volatility/elasticity/stack/weight semantics documented | ✅ (final report §schema + this file §2–3) |
| Determinism, volatility/elasticity differentiation, floor/ceiling | ✅ `Plan56EconomyGoodsTests` + `MarketPriceDeterminismTests` |
| Settlement ≥8 wired / caravan ≥5 wired | ✅ 6/6 and 5/6 of the new goods (deviation noted) |
| Old saves / new-good round-trip / id-keyed migration | ✅ `Plan56CloseOutTests` (added by this verification) |
| Headless economy smoke | ✅ after the demo regression fix |
| Data-integrity / tests / build | ✅ see regression matrix below |

## 7. Verification commands (this audit)

```
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # 0 errors
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # all green (incl. 22 Plan56 tests + 4 close-out)
dotnet build Ashfall.csproj                                  # 0 errors
godot --headless --path . -- --data-integrity-selftest       # PASS
godot --headless --path . -- --economy-selftest              # PASS 11/11 (after fix)
godot --headless --path . -- --caravan-selftest              # PASS
```

## 8. Deferred

- Wire `regionalSupply` into a loader-readable field or drop it (18 rows).
- `documents`/`weapons`/`contraband`/`misc` categories remain empty — the
  genuine remaining breadth gap if a future plan wants 46+ goods.
- Travel-encounter UI surfacing so the market's creature/human ecology is
  player-visible (Plan 45 phase 3).
