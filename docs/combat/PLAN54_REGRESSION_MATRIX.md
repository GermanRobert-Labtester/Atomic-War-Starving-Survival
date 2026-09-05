# Plan 54 Regression Matrix — exact commands and results (post-change)

| # | Gate | Command | Result |
|---|---|---|---|
| 1 | Core tests (full suite) | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **PASS — 6513/6513** (6498 baseline + 15 new Plan 54) |
| 2 | Godot host build | `dotnet build Ashfall.csproj` | **PASS — 0 errors, 0 warnings** |
| 3 | Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | **PASS** — 208 catalogs, 0 errors, 0 warnings |
| 4 | Combat selftest | `godot --headless --path . -- --combat-selftest` | **PASS — 26/26 checks** |
| 5 | Bridge verb (stable CI no-op) | `godot --headless --path . -- --bridge-selftest` | **PASS** (exit 0) |

## New test coverage (`Ashfall.Core.Tests/Plan54CombatCatalogTests.cs`, 15 tests)

Catalog: loads 20 weapons; the plan's original five present; all 15 baseline
weapons keep id+caliber; the 5 additions register with in-range stats; all
calibers resolve; no stat-clone tuples; orphan-caliber seam documented.

Combatants: exactly 12 load; all 10 baseline ids preserved; the 2 additions
register with correct kind/faction/stance/thresholds; `CombatantFactory`
spawns both with catalog-derived traits.

Runtime: new weapon fires in an encounter with both new enemies spawning
from the catalog (health honored at `enemyHealth=0`); fixed-seed encounter
with `weapon_quiet_carbine` replays identically (event-by-event equality);
all five new weapons resolve and fire.

Persistence: JSON round-trip of mid-combat state carrying a Plan 54 weapon
instance and Plan 54 combatants (id/ammo/CatalogId/AI mods survive).

## Pre-change baseline (for comparison)

6498/6498 tests · data-integrity 208/208 · combat-selftest 26/26 ·
both builds clean — see `PLAN54_BASELINE.md`.
