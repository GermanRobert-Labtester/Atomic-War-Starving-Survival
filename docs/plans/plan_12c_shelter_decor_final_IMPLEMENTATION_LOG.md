# Plan 12C Final — Shelter Interior & Memorial Wall

## Scope

Finish the deferred player-facing Plan 12C lane without creating a second
morale or memorial authority. The Core `ShelterDecorSystem` remains the
placement registry; `NeedsSystem` remains the sole morale authority; and
`MemorialSystem` remains the death-record authority.

## Implemented seams

- `ShelterDecorHostSession` loads the twelve modifiers from the live
  `ItemCatalog`, mounts or returns real inventory items, and applies the
  aggregated per-room modifier once daily to alive, actively assigned
  survivors through `NeedsSystem.Modify(..., NeedKind.Morale, ...)`.
- `Main.SetupShelterDecor` restores the registered `shelter_decor` campaign
  section, reconciles existing memorial entries, binds `ShelterDecorPanel`,
  and `SaveShelterDecor` captures it through `ShelterDecorSaveStore`.
- A memorial event projects its canonical plaque only after the memorial
  record has been committed. The plaque carries survivor and heirloom
  provenance and never fabricates an inventory item.
- The panel is reachable as `shelter_decor` through the player-surface
  registry, game-flow forwarding, and expanded-panel action configuration.
- `--shelter-decor-selftest` exercises catalog loading, real inventory
  consumption/return, daily NeedsSystem morale, plaque projection,
  SaveStore round-trip, and panel rendering.

## Verification record

- Focused Plan 12C Core tests and the new Godot self-test are run before the
  full canonical build/test/headless gate sweep.
- The populated snapshot fixture has been captured on Forward+ / X11, visually
  inspected, promoted as `snapshots/shelter_decor_default.png`, and fingerprinted
  in the two snapshot manifests. The run also revealed 29 unrelated pre-existing
  baseline drifts; this task did not overwrite them.

### Final gate results (2026-08-31)

| Gate | Result |
|---|---|
| `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 0 warnings, 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | PASS — 5,303 / 5,303 |
| `dotnet build Ashfall.csproj` | PASS — 0 warnings, 0 errors |
| `godot --headless -- --data-integrity-selftest` | PASS — 138 catalogs, 0 findings |
| `godot --headless -- --bridge-selftest` | PASS — shim-removal contract |
| `godot --headless -- --shelter-decor-selftest` | PASS — catalog, storage, morale, plaque, save, and panel path |
| `godot --rendering-method forward_plus -- --ui-snapshot-uitest` | Local target MATCH (103,917 B); global gate remains FAIL from 29 unrelated existing drifts, 0 capture failures |
