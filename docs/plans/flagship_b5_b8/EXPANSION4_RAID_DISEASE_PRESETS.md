# §27 Follow-On Expansion 4 — Raid Snapshot, Unsafe-Water Disease, Emergency Presets

> Three queued §27 items in one coherent increment. Each rides the contracts
> the flagship built; the queue items 5 (brine economy), 6 (machine-identity
> feedback), and the seasonal crop catalog await user approval.

## 1. Formal defense snapshot (§10.11 — "typed raid-modifier snapshot")

`PerimeterEncounterSnapshot` (the existing encounter projection) gains the
formal readiness surface, additive and UI-safe:

- `emplacements_ready` / `emplacements_disabled` — the repair authority's
  physical subject count (destroyed/disabled emplacements count honestly);
- `turrets_ready` (armed + not jammed) / `turrets_jammed` (barrel-wear
  consequence);
- `alarms_spent` — the early-warning lifecycle read (Plan 203 §6.7);
- `unguarded_sectors` — the honest breach surface: canonical sectors
  (`PerimeterSector.All`, no invented map directions) with **no intact
  emplacement at all**.

Pinned by tests including the degraded-state transition (destroying an
emplacement moves it from ready → disabled and its sector → unguarded).
Power's role stays exactly as migrated in Phase 7: the served-state callback
freezes turrets; power never touches the combat authority.

## 2. Disease-specific unsafe-water outbreaks (§9.12 — the silent gap closed)

**Audit finding:** `WaterTreatmentHostSession`'s doc comment claimed exposure
events "route to Disease, Needs, and Dose systems" — but the handlers only set
a text line. Authored waterborne diseases (`disease_typhoid_waterborne`,
`disease_dysentery`) sat unconsumed by this path.

- `WaterborneExposureRules` (Core, pure, deterministic): dose floor (clean
  output exposes nobody), dose→probability modifier bounded [0.5, 2.0] (dose
  grows exposure; DiseaseSystem owns the roll), and the disease mapping —
  typhoid for any live pathogen dose, dysentery joining on a severe dose
  (a contaminated-treatment emergency).
- Host wiring: `WaterTreatmentHostSession.PathogenExposureSink` (nullable —
  legacy text-only behavior without it) → `Main.SetupWaterTreatment` sweeps
  the roster through the canonical `DiseaseSystem.TryExpose` with the pure
  mapping. Infection ownership stays with the disease authority; zero direct
  stat writes.
- `DiseaseIds` gains the two authored waterborne ids (pinned to resolve in
  `disease_catalog.json` by test).
- Heavy-metal/radiation exposure events remain text-surface: a per-survivor
  dose handoff needs the roster↔rad-state join design (documented deferral).

## 3. Emergency priority presets (§27 — "if the player-facing need is proven")

The need is proven by the Phase 8 soak (52 brownout-hours in the Early
profile with manual-only priority management):

- **Core** (`PowerGridSystem`): `ApplyBrownoutShedPreset()` — demotes every
  non-Critical load one tier (Standard → Low); Critical loads are never
  touched; idempotent; returns the changed room ids (the journal surface).
  `ApplyCatalogDefaultPriorities()` — clears all player overrides so the
  `power_grid.json` classification rules again; returns the cleared count.
- **UI**: `PowerGridPanel` gains "EMERGENCY: PRESERVE LIFE SUPPORT" /
  "RESTORE DEFAULTS" buttons → `Main` applies and journals both
  ("…n non-critical circuit(s) demoted to preserve life support").
- Both presets survive save/restore (priorities are saved player state —
  pinned).

## Verification

| Gate | Result |
|---|---|
| `Integration/Expansion4IntegrationTests.cs` | 8/8 PASS (new) |
| Full `dotnet test` | **11,014 / 11,018** — the same 4 pre-existing dirty-worktree failures; zero new |
| `--data-integrity-selftest` | PASS |
| `--panel-bind-lifecycle-selftest` | PASS |
| `dotnet build Ashfall.csproj` | 0 warnings, 0 errors |

## Files changed

- `Assets/Ashfall.Core/Defense/PerimeterDefenseCatalog.cs` (snapshot fields)
- `Assets/Ashfall.Core/Defense/PerimeterDefenseSystem.cs` (snapshot counts + unguarded sectors)
- `Assets/Ashfall.Core/WaterborneExposureRules.cs` (new, pure policy)
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` (+2 authored ids)
- `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (preset policies)
- `src/Host/WaterTreatmentHostSession.cs` (exposure sink)
- `src/Main.ShelterInfrastructure.cs` (roster sweep wiring)
- `src/UI/PowerGridPanel.cs`, `src/Main.World.cs` (preset buttons + journal)
- `Ashfall.Core.Tests/Integration/Expansion4IntegrationTests.cs` (new, 8 tests)
- `docs/plans/flagship_b5_b8/EXPANSION4_RAID_DISEASE_PRESETS.md` (this file)

## Awaiting approval (queue items 4–6)

- **4.** Deeper Holdfast brine economy (needs a verified new trade consumer)
- **5.** Machine-identity/condition feedback (partially delivered by Expansion 2)
- **6.** Seasonal crop catalog rows (content authoring; mechanics ready)
