# Micro-Location Greenhouse — Agriculture Integration (F18)

Flagship plan §8 deliverable. Proven by `Ashfall.Core.Tests.MicroLocationGreenhouseIntegrationTests` (13 tests) plus the updated `GreenhouseCropExpansionTests`.

## Reward mapping (authored, `micro_locations.json`)

| Choice | Grant | Depletes |
|---|---|---|
| `take_greenhouse_seeds` | `seed_packets` ×2 (morale +1) | yes |
| `open_greenhouse_cabinet` | `crop_medicinal_herb` ×1 | yes |
| `leave_greenhouse` | nothing (non-depleting) | no |

## Item validity

Both ids exist in `items.json` (`seed_packets` — "A envelope of assorted vegetable seeds, some dated, some anonymous"; `crop_medicinal_herb` — Medical). Item existence alone was **not sufficient**: `seed_packets` was a dead item for agriculture — the planting authority only accepted `item_seed_*`.

## Seed/crop authority & the mapping fix

`GreenhouseSystem.Plant(plot, seedItemId, …)` resolves inputs exclusively through `GreenhouseExpansionCatalog.CropCatalog.Get(seedItemId)` — the one canonical crop table (12 `item_seed_*` entries before F18).

**Fix applied at the mapping contract** (per plan §8.4 — no micro-location-only planting exception): `CropCatalog` gained a 13th `CropDef` with `SeedItemId = "seed_packets"`, mirroring the hardy-tuber profile (yield `crop_tuber`, 144 h growth, water 12/day, light 6 h, no unlock). The mixed assorted-vegetable packet grows the starter staple — the least balance-inventive mapping, and plantability now flows through the same lookup every seed uses. `GreenhouseCropExpansionTests.CropCatalog_ContainsAll13Crops` pins the count and `CropCatalog_MixedSeedPacket_IsPlantable_AndCanonical` pins the route.

## Planting contract

`GreenhouseHostSession.Plant` (inventory gate → `System.Plant` → consume one seed) works for granted packets with zero changes: `F18_07` proves the packet enters the canonical planted state, `F18_08` proves planting consumes the packet (not infinite), `F18_09` proves growth advances through the normal `Water`/`TickDay` economy.

## One-shot semantics

`take_greenhouse_seeds` depletes the whole site (Core F1). `F18_11`: save/reload keeps the site depleted and the production selector can never re-surface it (64 seeds). Cabinet grant identical.

## Deterministic behavior

Grant quantities are fixed authored data; no RNG in resolution or planting. `F18_13` pins byte-identical traces across passes.

## Progression gates preserved

Owning the reward unlocks nothing by itself: occupied plots refuse re-plant, unknown plots refuse, unknown seed ids refuse, and growth still requires water + light (`F18_10`). The packet carries no private exemption from any greenhouse rule.

## Seasonal weighting findings

`EncounterDefinition.GetEffectiveWeight` supports stance multipliers (`stealthWeightMultiplier`, `speedWeightMultiplier`), danger floors, and destination filters — but **no season/weather/condition tags** exist in the selection path today. A data-driven `micro_ruined_greenhouse` weight adjustment (e.g. ×1.25–1.5 in growing season) would require extending the selection context with a season input — a selection-layer change, not a greenhouse change. Documented as deferred; nothing was hard-coded.

## Seasonal seed variation decision

**Preserved explicit rewards.** The authored grant is a concrete item id; the crop catalog has no seasonal reward-pool resolver, and plan §8.11 forbids adding randomness for novelty. A future `rewardPool` keyed by season could build on the same resolver without breaking this contract.

## Tests

`F18_01`–`F18_13` in `MicroLocationGreenhouseIntegrationTests` (rewards, item validity, downstream consumers, planting integration, consumption, growth, progression gates, one-shot/save-reload, determinism).

## Deferred work

Season-aware discovery weighting (needs a season-aware selection context), seasonal seed pools, `crop_medicinal_herb`-based medical crafting depth (the herb already feeds `craft_dried_herb_packets` in `recipes.json`).
