# Plan 54 — Baseline Record

## Stale-plan correction (important)

Plan 54 was drafted against a reported baseline of **5 weapons / 0 enemies**.
Reconnaissance against the repository proved the actual verified baseline at
task start was:

| Dimension | Plan assumed | Repo truth (commit `d66d2f32`) |
|---|---|---|
| `combat_catalog.json` weapons | 5 | **15** |
| `combat_catalog.json` combatants | 0 | **10** |
| Ammo definitions | unknown | 14 |
| Material/armor definitions | unknown | 7 |

The 15/10 baseline was landed by prior plans (Plan 10A "Bestiary & Armory"
combatant pass and the improvised-weapon expansion; commits `34d6d86d`,
`7738facc`). Constraint 1.2 (preserve existing content) therefore applies to
**all 15 weapons and all 10 combatants**, and the Plan 54 delta is
**+5 weapons and +2 combatants**.

## Baseline verification (exact results, before any change)

| Check | Command | Result |
|---|---|---|
| Core tests | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6498/6498 PASS |
| Godot host build | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | PASS — 208 catalogs, 0 errors |
| Combat selftest | `godot --headless --path . -- --combat-selftest` | PASS — 26/26 checks |
| Bridge verb | `godot --headless --path . -- --bridge-selftest` | PASS (exit 0) |

## The original five (plan-named oracle)

The plan's parity oracle — all five present at baseline and untouched:

| id | display_name | caliber |
|---|---|---|
| `weapon_pipe_rifle` | Pipe Rifle | `ammo_357` |
| `weapon_scrap_shotgun` | Scrap Shotgun | `ammo_12g` |
| `weapon_bolt_rifle` | Held-Bolt Rifle | `ammo_308` |
| `weapon_assault_rifle` | Assault Rifle | `ammo_556` |
| `weapon_lmg` | Light Machine Gun | `ammo_762` |

Full 15-row parity matrix: see `WEAPON_PARITY_MATRIX.md`.
