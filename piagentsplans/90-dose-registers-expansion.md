# Plan 90 — Dose Register Bands & Plans Expansion (4 bands → 12 bands, 3 plans → 8 plans)

## Goal (2 lines)
Expand `dose_registers.json` from 4 radiation bands and 3 care plans to 12 bands
and 8 plans. The dose register system (`DoseRegistersCatalog.cs` confirmed live)
defines radiation-exposure thresholds, care plans, guess options, calibration,
register actions, and dose-ledger NPCs. The bands are too coarse (only 4:
green/amber/red/black) and the care plans are too few (only 3).

## Why (P2)
- Verified: `dose_registers.json` has 4 bands (id, label, threshold_msv,
  disposition), 3 plans (id, label, cost, note), 3 guesses, 1 calibration, 4
  registers, and 4 NPCs. `DoseRegistersCatalog.cs` is confirmed in Core.
- Creates the dose-management pillar: the dose register is the shelter's
  radiation-tracking bureaucracy — bands classify exposure, plans define care
  options, guesses let the player estimate before a reading. 4 bands (0, 100,
  300, 600 mSv) is too coarse; the mid-range (100–600) needs granularity.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/dose_registers.json` (expand bands 4 → 12,
  plans 3 → 8)
- Read-only: `Assets/Ashfall.Core/DoseRegistersCatalog.cs` (confirm schema and
  how bands/plans/registers/NPCs are consumed)

## Content grammar (per band)
- `id`: snake_case with prefix `band_` (confirmed prefix).
- `label`: short label (Green, Amber, Red, Black, and new intermediate bands).
- `threshold_msv`: integer millisieverts — the cumulative dose threshold that
  activates this band. Must be strictly increasing across bands.
- `disposition`: 1 sentence of clinical text describing what this band means
  (match the existing cold, bureaucratic tone — "Named on the sick list. Care
  is a choice, not a cure.").

## Content grammar (per plan)
- `id`: snake_case with prefix `plan_` (confirmed prefix).
- `label`: short label (Morphine tray, Comfort rounds, Nothing).
- `cost`: item id or "time" or "none" — the resource cost of the care plan.
- `note`: 1 sentence of prose describing the plan (match the existing tone).

## Steps
1. Read `DoseRegistersCatalog.cs` to confirm how bands are selected (by
   cumulative dose falling within threshold ranges) and how plans are applied.
2. Read the existing 4 bands and 3 plans to confirm the quality bar.
3. Author 8 new bands with finer granularity:
   - `band_white` (0 mSv): baseline, no exposure.
   - `band_yellow` (50 mSv): minor exposure, watch.
   - `band_orange` (150 mSv): moderate exposure, restricted duty.
   - `band_rose` (200 mSv): significant exposure, light duty.
   - `band_crimson` (400 mSv): severe exposure, sick list.
   - `band_violet` (500 mSv): critical exposure, comfort care.
   - `band_indigo` (800 mSv): terminal exposure, palliative only.
   - `band_void` (1000 mSv): lethal exposure, the registrar stops reading.
4. Author 5 new care plans:
   - `plan_chelation` (cost: chelation_agent): chelation therapy to reduce
     body burden.
   - `plan_iodine_prophylaxis` (cost: potassium_iodide): thyroid protection.
   - `plan_isolation` (cost: time): isolate the patient to prevent secondary
     exposure.
   - `plan_rest` (cost: time): bed rest and monitoring.
   - `plan_transfer` (cost: fuel): transfer to a better-equipped facility
     (if one exists).
5. Each band: distinct threshold_msv (strictly increasing), label, and
   disposition. Each plan: distinct cost and note.
6. Cross-reference: all band ids unique; threshold_msv strictly increasing;
   all plan ids unique; plan costs reference existing items or "time"/"none".
7. Validate: `--data-integrity-selftest` (all ids resolve).
8. xUnit: dose register catalog loads 12 bands (thresholds strictly increasing)
   and 8 plans, all ids unique, all item-cost plans resolve in items.json.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is threshold ordering (step 5): bands must be
strictly increasing by threshold_msv, and the system must select the correct
band for a given cumulative dose.

## Definition of Done
- `dose_registers.json` has 12 bands (strictly increasing thresholds) and 8
  plans, all ids resolving, integrity + tests green.

## Follow-on
- Plan 81 (dose locations) — locations feed dose accumulation into bands.
- Plan 09B (radiation system) — bands classify radiation exposure.
- Plan 79 (autopsy procedures) — terminal-band patients may die and require
  autopsy.
- Plan 83 (weather seasons) — seasonal fallout pushes survivors into higher
  bands.
- Existing 09B (radiation) — this plan provides the band/plan data.
