# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# CF-P6 — Vehicle armor grades

**Package:** `CF-P6-VEHICLE-ARMOR-GRADES` · completion-first Plan 04.
**Authority:** Plan 213 decision D6 and the existing Plan 50 vehicle-garage owner.
**Historical status:** INTEGRATED, 2026-09-19; original focused acceptance retained below.
**Editorial review:** 2026-09-28. Current code/catalog inspected; tests were not rerun.

## 0. Framing — Plate, Not Promise (editorial polish pass — commentary only)

*(Post-hoc, non-contractual editorial block. It changes no scope, claim, decision, acceptance
criterion or recorded status.)*

> "'Armor' is a word that invites combat into a room it was not invited to."

The package's own fence is its best line: *those mechanisms must not be inferred from the word
"armor."* What is actually built is quieter and more honest — a finite plate-integrity pool,
canonical materials consumed, breakdown probability lowered, travel wear absorbed. Protection
here is maintenance, not warfare; the vehicle is armoured the way a roof is repaired.

- **The Foundry quality stamp is craftsmanship turned into a number.** Who made the plate matters
  because how it was made matters — a rare case of provenance with a mechanical payoff.
- **Absorption into a finite pool is the whole theory of protection**: everything can be spent,
  and the ledger says how much is left.

---

## Outcome

Fit one authored armor grade to a garage vehicle, consume canonical materials,
reduce its mechanical breakdown probability, and absorb part of chassis-bound
travel wear into a finite plate-integrity pool. Foundry quality stamps that pool
when installed; the existing garage save state preserves it.

This package adds no vehicle combat-damage model, radiation shielding, armor
stacking, RNG stream, save section or daily armor coordinator. Those mechanisms
must not be inferred from the word “armor.”

## Authority and integration

| Concern | Existing owner |
|---|---|
| Definitions and costs | `Assets/StreamingAssets/Data/vehicle_armor_grades.json` and `VehicleArmorGradeCatalog.cs` |
| Commands and state | `Assets/Ashfall.Core/Expeditions/VehicleGarageSystem.cs`: install/reforge, profile decoration, trip wear, capture/restore |
| Host binding | `src/Main.VehicleGarage.cs`; foundry quality comes through the existing expansion-hub seam |
| Travel consumer | `src/Host/ExpeditionHostSession.cs` uses the garage-decorated profile and records trip wear |
| Player commands | `src/UI/VehicleGaragePanel.cs` selects owned vehicles and forwards commands |
| Persistence | Existing `vehicle_garage` section through `VehicleGarageSaveStore` |

The historical `Main.Plans50_53.cs` path has moved. Do not introduce the unrelated
coordinator, manifest or save-state names that appeared in the removed appendices.

## Authored ladder

Values below summarize the current JSON; that file remains authoritative.
Mitigation and absorption are permille, not percentages.

| Grade ID | Breakdown mitigation | Chassis-wear absorption | Base integrity | Speed delta | Fuel multiplier | Terrain |
|---|---:|---:|---:|---:|---:|---|
| `grade_0_stock` | 0 | 0 | 0 | 0 | 1.00 | road, rough, coastal |
| `grade_1_scrap_plate` | 100 | 200 | 100 | −0.02 | 1.02 | road, rough, coastal |
| `grade_2_sheet_plate` | 150 | 250 | 140 | −0.04 | 1.05 | road, rough, coastal |
| `grade_3_composite_plate` | 200 | 300 | 180 | −0.07 | 1.08 | road, rough |
| `grade_4_alloyed_heavy_plate` | 250 | 350 | 240 | −0.10 | 1.12 | road, rough |

Install/reforge bills use existing item IDs. Reforge includes mechanical parts
for tiers 2–4; it is not universally scrap-only. `install_labor_ticks` is metadata,
not evidence of a running work scheduler.

## Behavior contract

1. **Install:** validate the grade, terrain, vehicle condition and material bill
   through the garage command seam. Replace the single fitted grade; the existing
   replacement rule refunds half the previous scrap line, with a minimum of one.
2. **Stamp:** multiply base integrity by foundry armor and purity factors, round
   away from zero, then clamp between `ceil(base × 0.5)` and `ceil(base × 1.3)`.
   Save the material profile, purity, maximum and current integrity at installation.
   Later foundry production must not retroactively change the fitted pool.
3. **Decorate:** active mitigation scales positive breakdown probability by
   `(1000 − mitigation) / 1000`, clamped to [0,1]. Zero base probability stays zero.
   Speed and fuel penalties belong to the fitted grade. Depletion disables its
   mitigation and absorption while retaining the grade and penalties.
4. **Wear:** compute the existing base travel wear; absorb the lesser of remaining
   plate integrity and rounded `baseWear × absorption / 1000`. Deduct absorption
   from the plate and chassis increment. Engine/transmission wear remains governed
   by the existing garage rules. Preserve the code's distinct rounding modes.
5. **Reforge:** consume the authored bill and restore the stamped maximum without
   changing grade or provenance. Legacy records without armor fields remain neutral.

Validation bounds and runtime defensive clamps differ: catalog validation caps
mitigation/absorption/base pool at 250/350/400; the runtime read model additionally
caps mitigation/absorption at 400/500. Do not substitute one set for the other.

## Boundary conditions worth preserving

- Owned-vehicle selection is enforced by the panel. Core compatibility/existence
  checks depend on the terrain resolver; pure-Core callers can omit it.
- A null inventory bypasses material charging for supported Core callers. Player
  commands must retain the canonical inventory binding.
- Install validation can create a neutral vehicle record before rejecting an
  insufficient bill. Do not claim that every failed command is mutation-free.
- Condition display distinguishes none, depleted, critical, worn and nominal;
  nominal begins at 75% integrity and worn at 25%.

## Acceptance and historical evidence

Preserve coverage for catalog validation; install costs and refusals; terrain
compatibility; replacement/reforge; provenance stamping; positive-risk mitigation;
chassis absorption and depletion; legacy/round-trip saves; estimate/runtime profile
parity; and deterministic replay. UI checks must exercise the actual garage binding.

The September 19 closeout records `Plan213VehicleArmorGradeTests` **21/21**, garage
contract suites **6/6 + 5/5 + 3/3**, and `--vehicle-garage-selftest` **27/27**. It also
records data integrity with zero errors/five pinned warnings, content utilization,
catalog boot, panel lifecycle, accessibility, builds and generator checks passing.
These remain historical evidence, not fresh verification of this revision.

## Current closeout verification — 2026-09-30

Rechecked signed DEC-95, the five-row stock-plus-four-tier catalog, the live
`VehicleGarageSystem` install/reforge authority, and its Plan 50 host/UI seam.
Fresh focused checks passed:

- `bin/run-scoped-tests Ashfall.Core.Tests/Expeditions/Plan213VehicleArmorGradeTests.cs` — 21/21.
- `bin/run-scoped-tests Ashfall.Core.Tests/Expeditions/Plan50VehicleGarageIntegrationTests.cs` — 5/5.
- `godot --headless --path . -- --vehicle-garage-selftest` — 27/27.

No production, data, or test files changed. The broad gates listed above remain
dated 2026-09-19 evidence; they were not rerun for this archival closeout.

The existing soak proves integrity bounds and non-strict final chassis ordering;
it does not prove strict ordering every day, breakdown ordering or reforge cadence.
For future code changes, run the affected existing tests through
`bin/run-scoped-tests` and the relevant bounded runtime probe. This archival
closeout adds no production authority and leaves the documented soak limitations
visible.
