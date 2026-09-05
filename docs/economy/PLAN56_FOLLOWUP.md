# Plan 56 Follow-Up — regionalSupply + Category Fill

> Executes the two deferred items from `PLAN56_VERIFICATION.md` §8.

## 1. `regionalSupply` is now a first-class loader field

`GoodDefinition.regionalSupply` (optional string, default empty, trimmed on
load) — present in the strict DTO, parsed for every row. It is production-
source provenance (`foundry` / `traplines` / `greenhouse` / `settlement` /
`flotilla` / `coastal` / `general`), not a pricing input: the market runtime
does not couple to it. **27 of 48 rows** carry it. Pinning:
`Plan56FollowUpTests.RegionalSupply_ParsesAsFirstClassField` /
`RegionalSupply_IsOptional_AndTrimmed`.

## 2. Category fill — 8 new goods (revised count target: 40 → 48)

All ids are canonical `items.json` ids; prices anchor to the item
`tradeValue` where present (§2.5/§7 authority rules).

| id | category | price | vol | elas | stack | kg | Economic role |
|---|---|---|---|---|---|---|---|
| `item_logistics_cipher_sheet` | documents | 60 | 0.35 | 1.8 | 2 | 0.05 | Premium intel — lightest, most labile good in the catalog |
| `sealed_government_document` | documents | 11 | 0.10 | 1.1 | 5 | 0.3 | Common paperwork trade; stable |
| `weapon_sidearm` | weapons | 35 | 0.15 | 0.9 | 1 | 1.0 | Durable defense good; weapons hold value (inelastic) |
| `weapon_pipe_shotgun` | weapons | 20 | 0.18 | 1.1 | 1 | 2.5 | Improvised arms; heavier, cheaper |
| `item_taper_kit_opioid` | contraband | 25 | 0.30 | 1.4 | 2 | 0.2 | Restricted medical; raid/supply shocks, whispers |
| `duct_tape` | misc | 6 | 0.08 | 0.7 | 15 | 0.3 | Repair staple — most inelastic of the fill |
| `rope` | misc | 8 | 0.10 | 0.8 | 6 | 2.0 | Bulk-ish utility; quarry/lighthouse demand |
| `item_cassette_tape` | misc | 9 | 0.25 | 1.6 | 8 | 0.15 | Morale media — volatile, discretionary |

Volatility/elasticity authored to the verified walk semantics (daily demand
amplitude × response), per `PLAN56_FINAL_REPORT.md`. No two of the eight
share an economic signature (test-gated).

## 3. Wiring

**Settlements (8 of 8 goods placed, archetype-plausible, no self-needs):**

| Settlement | Change |
|---|---|
| fort_karkov (stronghold) | goods += weapon_sidearm · needs += cipher sheet, cassette tape |
| tinkers_notch (scrap market) | goods += weapon_pipe_shotgun, duct_tape |
| pilgrim_hearth (sanctuary) | goods += sealed_government_document |
| cape_beacon (lighthouse) | goods += rope |
| ferry_crossing (trade post) | goods += cassette tape · needs += duct_tape |
| slate_hollow (quarry) | needs += rope |
| lock_seven (stronghold) | needs += weapon_sidearm, sealed_government_document |
| silo_burrow (refugee camp) | needs += weapon_pipe_shotgun |

**Caravans (5 of 8 travel):** flotilla salt run carries the cipher sheet +
opioid kit (intelligence and contraband); the grain convoy carries cassette
tapes (morale goods for the settlements it feeds); the free-trader circuit
carries the sidearm + duct tape. `rope` and the pipe shotgun stay regional
(deliberate — bulk/low-value goods reward local need, not caravan runs).

## 4. Verification

- `Plan56FollowUpTests` (6): regionalSupply parse/trim/optional, category
  counts, canonical-id convention, economic-signature distinctness,
  clamp-band + determinism for the new goods, baseline-40 untouched.
- `Plan56CloseOutTests` reference integrity auto-covers the new wiring.
- Gates: tests 6591/6591 · both builds clean · data-integrity / bridge /
  economy / caravan selftests PASS.
