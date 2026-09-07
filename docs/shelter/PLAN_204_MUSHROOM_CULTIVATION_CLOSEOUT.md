# Plan 204 — Subterranean Mushroom Cultivation Extension — Closeout

**Flagship:** Plans 202–205 (see `docs/plans/PLANS_202_205_RECONNAISSANCE.md` for the
renumbering note and authority map). **Plan class:** Wave B — extension of an existing
live system, not a new authority.

**Status: IMPLEMENTED & VERIFIED.** All work extends the canonical
`Ashfall.Core.Farming.FungiCultivationSystem` and the `underground_flora.json`
data authority. No new catalog, no new panel, no new save store.

---

## 1. Implemented gameplay contracts

### 1.1 Substrate preparation states (Core: `SubstratePreparation`)

Abstract, non-procedural. No real-world sterilization parameters are encoded.

| State | Risk multiplier (at inoculation) | Growth multiplier (daily) |
|---|---|---|
| `untreated` (old-save default) | ×1.0 | ×1.0 |
| `prepared` | ×0.5 | ×1.1 |
| `clean` | ×0.25 | ×1.2 |
| `compromised` | ×1.5 | ×0.8 (reset to `untreated` at plant) |

Command: `PrepareSubstrate(plotId, useHeat)` — fallow, unquarantined, unbloomed plots only.
Costs 1 `clean_water` (+1 `fuel` when heated). Seeded roll against the substrate's
`contamination_risk` yields `compromised` on failure; heated success splits
`clean`/`prepared` bands deterministically.

### 1.2 Temperature band

`FungusStrainDef.temperature_min/temperature_max` (°C, broad defaults 4–32 keep legacy
catalogs unchanged). `TickDay` accepts `roomTemperatureC` plus an optional
`roomTemperatureOverride(roomId)` delegate; the Godot host projects real
`ShelterThermalSystem` per-room temperatures (`Main.Plans190_193.TickPlans190_193`).
Outside the band → growth ×0.35 (stall, not stop).

### 1.3 Flush curve

`FungusStrainDef.flush_count` (default 1 = historical behavior). Yield per flush:
**100 % → 60 % → 40 %** (deterministic rounding, min 1). Non-final harvests leave the plot
colonized at growth stage 0.5 with `remainingFlushes` decremented; the final harvest resets
the plot to fallow exactly as the pre-204 behavior did. Old-save plots with
`remainingFlushes == 0` harvest as a single full flush and return to fallow.

### 1.4 Contamination model

- Threshold bloom: contamination ≥ 1.0 blooms the plot outright (deterministic — no RNG).
- Daily decay: −0.02/day on healthy plots (evaluated **after** the threshold check so
  spread pressure that reaches the threshold actually fires).
- Spread boundary: blooming plots apply +0.05/day **only to planted, unquarantined plots in
  the same room**. Cross-room spread is structurally impossible.
- The historical seeded wet-moisture bloom roll and `Toxic`-category instant bloom are kept.

### 1.5 Disposal matrix (all outcomes are real transactions — no free deletion)

| Method | Cost | Effect |
|---|---|---|
| `discard` | free | Plot fully cleared; contaminated mass written off |
| `burn` | 1 `fuel` | Plot cleared; blocked without fuel |
| `quarantine` | 2 `scrap_wood` | Plot sealed: no growth, no spore emission, no spread; recoverable via `PurgeToxicBloom` (which also releases the seal) |

Disposal is refused on non-contaminated beds (`not_contaminated`).

### 1.6 Spore hazard

Unchanged routing: `OnSporeExposure(roomId, dose)` aggregates per-room hazard for the
ventilation/medical authority. Quarantined beds emit nothing and their spore density decays.

### 1.7 Kitchen routing fix (data authority)

The harvest item `harvested_mushrooms_subterranean` was type `Food` with **no nutrition
values and no recipe consumer** — a dead-end. Fixed:
- `items.json`: +`hungerRestore: 6`, +`moraleEffect: 1`.
- `KitchenNutritionPanel`: new recipe option "Cultivated Mushroom Mash"
  (`recipe_subterranean_mushroom_mash`, 2× harvest + 1× clean water).
- Gated by `FungiCultivationPlan204Tests.HarvestItem_IsEdibleInItemCatalog_KitchenRoutable`.

---

## 2. Save / migration matrix

| Feature | Old-save baseline |
|---|---|
| `substratePreparation` | defaults `untreated` (historical behavior) |
| `remainingFlushes` | 0 = legacy single-flush plot; harvest → fallow as before |
| `isQuarantined` | false |
| `FungiCultivationState.schema_version` | bumped 1 → 2; v1 payloads restore via additive defaults |
| Existing greenhouse / agriculture | untouched |

No free growth, no free blooms, no inventory mutation occurs on migration. The
`FungiSaveStore` / campaign-envelope section (`fungi_cultivation`) is reused unchanged —
the state payload is additive only.

## 3. Data authority changes (`underground_flora.json`, additive only)

| Strain | temperature_min | temperature_max | flush_count |
|---|---|---|---|
| `strain_phosphor_bracket` | 8 | 26 | 2 |
| `strain_grey_mycelium` | 6 | 24 | 3 |
| `strain_cordyceps_mutant` | 12 | 28 | 2 |
| `strain_black_rot_mold` | 4 | 34 | 4 |

`schema_version` stays 1 (additive optional fields; loader unchanged).

## 4. UI (`FungiCultivationBedPanel` — UI-07 stub → implemented)

Presentation-only. Bind remains typed (`Bind(FungiCultivationSystem)`). Implements the
`state → blocker → cost → consequence` standard: bed list with phase labels, full status
readout (strain, growth, flushes, prep, moisture, contamination, room spore load,
bioluminescent light), and ten commands — prepare (cold/heated), plant, water, harvest,
purge, discard, burn, quarantine, dig new bed (canonical `room_greenhouse_shelter`).
All feedback mirrors Core `ActionResult`; every failure code renders as player-readable
text; no raw item IDs in prose.

## 5. New journal events (host)

`fungi_substrate_prepared`, `fungi_substrate_disposed`, `fungi_contamination_spread`
(plus the pre-existing `fungi_toxic_bloom`, `fungi_harvest`).

## 6. Test coverage (`Ashfall.Core.Tests/Farming/FungiCultivationPlan204Tests.cs`)

16 tests covering: prep outcomes/costs/determinism and blocked-on-planted; prepared-vs-
untreated starting contamination; modifier bounds; thermal stall and host room projection;
3-flush taper to fallow; legacy single-flush; threshold bloom; same-room-only spread;
daily decay; burn/quarantine/discard transactions incl. refusal on healthy beds;
purge-releases-quarantine; old-save baseline; save round-trip; same-seed replay;
items.json edibility gate. Full suite: **9070/9070 PASS**.

## 7. Verification record (5-step checklist)

| Check | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests` | PASS — 0 errors, 0 warnings |
| `dotnet test Ashfall.Core.Tests` (full, blame-hang guarded) | PASS — 9070/9070, 27 s |
| `dotnet build Ashfall.csproj` | PASS — 0 errors, 0 warnings |
| `godot --headless -- --data-integrity-selftest` | PASS — 0 findings, 293 catalogs |
| `godot --headless -- --bridge-selftest` | PASS — exit 0 |

Core fix shipped with this plan: contamination decay previously ran before the threshold
check, so spread pressure reaching exactly 1.0 decayed to 0.98 and never bloomed.

## 8. Real-world abstraction boundary

Preparation is an abstract state machine with resource costs; inoculation is a command that
consumes a spore item; disposal is abstract discard/burn/seal. No cultivation procedures,
sterilization parameters, species claims, or hardware instructions are encoded. Strain
names and displays remain fictionalized (`Corpse-Glow Bracket Fungus`, `Glass-Root
Cordyceps`, …).
