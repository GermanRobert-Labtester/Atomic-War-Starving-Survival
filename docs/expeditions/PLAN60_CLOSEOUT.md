# Plan 60 — Vehicle Expansion: Closeout

## Status: **COMPLETE** (count-reconciliation variant)

## Baseline & count reconciliation

The plan's verified baseline of 3 was stale: `vehicles.json` contained
**8 vehicles** (the 3 originals + 5 from the Plan 10 era:
`vehicle_steam_halftrack`, `vehicle_armored_mobile_base`,
`vehicle_salvage_dredger`, `vehicle_scout_motorcycle`,
`vehicle_ambulance_rig`). Plan 60 adds the **2 missing roster roles** — the
entire improvised/no-fuel family — reaching **10 total**.

## On-foot finding (Task 60C): **implicit — Case A**

`ExpeditionVehicleSystem.PrepareForExpedition` returns `(fuel 0, travelMod
1.0, no breakdown)` for unowned/unknown vehicle ids — foot travel is the
implicit baseline. **No foot vehicle record was added** (Plan 60 §2.2 Case A).

## Runtime semantics (verified in code, not assumed)

| Field | Verified semantics |
|---|---|
| `speed_multiplier` | returned as `travelTimeMod` — higher = faster; no clamp |
| `cargo_capacity` | abstract units, stored per instance |
| fuel | `fuelNeeded = distanceKm × fuel_consumption_per_km`; dispatch blocked when `fuel < fuelNeeded`; zero-fuel (0/0) passes cleanly |
| `breakdown_threshold` | **never read by the runtime** — breakdown is `condition < 20` + 30% seeded roll; the field is decorative (documented, values kept in-family) |
| wear | `condition -= distanceKm × 0.5` — flat across all vehicles (runtime limitation: per-vehicle wear is not data-tunable; documented) |
| `condition_max` | applied at acquisition; `Repair` clamps to 100 |
| attachments | stored as id list; no stat application exists in the runtime (decorative state; `winch_kit` precedent honored — no invented ids) |
| terrain | stored/display state; no route gating consumes it yet (documented follow-up) |
| acquisition | `AcquireVehicle(vehicleId)` — ownership is vehicle-id-keyed; **no `items.json` records required** for ownership |

## Final roster (10)

| # | ID | Tier | Role | Speed | Cargo | Fuel | Terrain |
|---|---|---|---|---|---|---|---|
| 1 | `vehicle_utility_quad` | 3 utility | all-terrain workhorse | 1.3 | 90 | 40 @ 0.3 | rough |
| 2 | `vehicle_dirt_bike` | 3 utility | fast rough scout | 1.8 | 30 | 25 @ 0.2 | rough |
| 3 | `vehicle_cargo_truck` | 4 logistics | heavy road hauler | 1.6 | 250 | 80 @ 0.5 | road |
| 4 | `vehicle_steam_halftrack` | 4 logistics | heavy rough hauler | 0.85 | 180 | 120 @ 0.7 | rough |
| 5 | `vehicle_armored_mobile_base` | 5 rare | mobile base | 0.7 | 380 | 200 @ 0.95 | road |
| 6 | `vehicle_salvage_dredger` | 4 specialized | coastal salvage | 0.95 | 260 | 95 @ 0.55 | coastal |
| 7 | `vehicle_scout_motorcycle` | 2 civilian | speed scout | 2.4 | 18 | 18 @ 0.18 | rough |
| 8 | `vehicle_ambulance_rig` | 3 civilian | medical transport | 1.25 | 140 | 60 @ 0.45 | road |
| 9 | **`vehicle_bicycle`** (new) | 1 improvised | fuel-free speed | 1.15 | 25 | **0 @ 0** | rough |
| 10 | **`vehicle_cargo_cart`** (new) | 1 improvised | fuel-free haulage | 0.8 | 110 | **0 @ 0** | rough |

Requested-family reconciliation: motorcycle/APC/military-transport were
**already covered** by the 8 (scout motorcycle, armored mobile base, cargo
truck). The two genuine gaps were the improvised family — the plan's Tier 1 —
which the 8-vehicle roster lacked entirely. Both new vehicles are zero-fuel
(`max_fuel: 0`, `fuel_consumption_per_km: 0`) — verified through the dispatch
gate (`fuelNeeded = 0` passes; consumption 0; refuel no-op).

## Acquisition (the discovered integration blocker + fix)

**Discovered blocker:** the only acquisition path was the fresh-shelter
starter quad (`AcquireVehicle` — one call site). The other 7 vehicles had no
way to enter a campaign. Minimal fix (the "vehicle-item/recovery pattern"
Plan 60 §60AN anticipated, which did not exist yet):

- **3 kit items** (`items.json`): `item_vehicle_kit_bicycle`,
  `item_vehicle_kit_cargo_cart`, `item_vehicle_kit_scout_motorcycle`.
- **3 assembly recipes** (`recipes.json`, workbench): canonical materials
  (scrap metal, copper wire, duct tape, wooden planks, nails, mechanical
  parts, fuel) → kit items. Abstract costs; no real-world assembly detail.
- **Host bridge** (`ExpeditionHostSession.AssembleVehicleFromKit`): consumes
  the kit atomically (`TryConsumeBill`), acquires via `AcquireVehicle`,
  refunds the kit on failure (consume-first ordering — the player is never
  left with one without the other). New stable command code
  `PlayerCommandCode.AssembleVehicle = "vehicle.assemble"`.
- **Settlement trade:** the scout-motorcycle crate wired into
  `settlement_nine_rails` trade goods (mechanical settlement — canon-plausible;
  garrison confiscates these on sight, hence "priced like a toolbox").

**Non-farmability:** assembly is consume-once per kit; kits are craft-limited
by materials; the motorcycle kit's trade path is a settlement stock decision.
No repeatable vehicle-reward loop exists.

## Balance profile

- **No dominant vehicle:** the scout motorcycle (2.4×) pays 18 cargo + 18
  fuel; the armored mobile base (380 cargo) pays 0.7× + 0.95/km + 200 fuel;
  the improvised pair pay zero fuel for low speed/low cargo.
- **Zero-fuel niche:** bicycle (1.15×, faster than the implicit 1.0× foot
  baseline) and cargo cart (110 cargo, between quad 90 and ambulance 140)
  remain the only options when fuel is unavailable — fuel scarcity never
  strands mobility entirely.
- **Wear honesty:** all vehicles wear 0.5/km (runtime-flat); the improvised
  pair's kits are cheap to rebuild, the truck/base require real parts —
  maintenance scales with tier through acquisition cost instead.
- **`breakdown_threshold`** left in-family (0.15–0.3) — decorative until the
  runtime consumes it (flagged follow-up).

## Verification

| Gate | Result |
|---|---|
| `dotnet build Ashfall.csproj` | **PASS** 0 errors (after concurrent-writer race resolved — see note) |
| `dotnet test Ashfall.Core.Tests` | **PASS** 6,612/6,612 (updated stale Plan-10 count pin → 10-vehicle roster assertions incl. zero-fuel invariants) |
| `--data-integrity-selftest` | **PASS** 0 findings / 208 catalogs (10,287 ids) |
| `--expedition-selftest` | **PASS** — all vehicle gates green (V3 estimate, V6 fueled dispatch, V9 repair) |
| `--content-utilization-selftest` | **PASS** |
| `--bridge-selftest` | **PASS** exit 0 |

**Concurrent-writer note:** mid-implementation, `src/Main.CampaignOwners.cs`
(uncommitted Plan 56 phase 5 work from a parallel session) transiently failed
compilation (`_dataDir` static-context error). The file was not touched by
Plan 60; the writer fixed it (mtime 12:14) and the build went green. No
Plan 60 change was affected.

## Deferred

1. Per-vehicle wear rates (runtime-hardcoded 0.5/km — improvised vehicles
   wear like trucks; a data-tunable wear field is the follow-up).
2. `breakdown_threshold` consumption by the runtime (currently decorative).
3. Terrain gating in expedition dispatch (`terrain_type` is display state).
4. Attachment stat effects (stored, never applied).
5. Military-vehicle faction acquisition hook (armored mobile base exists in
   data; a one-shot recovery/quest-reward path is future work).
6. Vehicle selector UI audit at 10 entries (dispatch UI untouched per scope).
