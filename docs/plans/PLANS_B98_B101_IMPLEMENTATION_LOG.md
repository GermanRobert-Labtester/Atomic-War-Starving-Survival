# Plans B98–B101 Implementation Log

## Authority and divergence matrix

| Plan | Current authority | Missing seam | Safe slice |
|---|---|---|---|
| B98 RTG baseline power | `NuclearCoreLifecycleSystem` + `PowerGridSystem` | Nuclear output is never published to the grid | Host projection keyed by a Core constant, republished after restore and before grid resolution |
| B99 abstract optical fire-control | `PrecisionOpticsEngine` + `BallisticsWorkbenchSystem` + tactical combat projection | Optic quality has no persisted weapon projection; combat bridge never applies ballistics modifiers | Add bounded optic quality to the existing ballistics profile and apply it at the existing combat token seam |
| B100 scientific glassware | `SilentFoundrySystem` heat machine | Glass blank and viewport items have no gameplay producer | Add a data-driven glassworks catalog merged into Silent Foundry; no parallel production system |
| B101 armored draisine logistics | `RailwaySystem` + `DraisineRerailingSystem` | Rail transmission wear is absent and rail travel is not advanced by the campaign | Add additive train transmission state/service, recovery restoration, and a thin daily rail travel owner |

Historical narrative glass catalogs remain read-only lore. `VehicleGarageSystem`
continues to own overland vehicle transmission wear; it is a convention
precedent, not a second rail state store. No Unity or operational real-world
construction instructions are introduced.

## Phase 1 — B98

Status: PASS

Changed:

* Added `NuclearCoreLifecycleSystem.PowerSourceId`.
* Republished nuclear generation after construction/restore, install, scram,
  and before the phase-1 power-grid owner.
* Added focused Core coverage for idempotency, fuel-free generation, scram
  projection, and restore/republish.

Tests:

* Focused baseline: 52 passed before edits.
* B98 focused coverage: 21 passed.

Divergences:

* The nuclear lifecycle tick remains separate. B98 publishes output only and
  does not activate the previously orphaned wear/coolant tick.

## Phase 2 — B99

Status: PASS

Changed:

* Added bounded optic quality to `BallisticsWorkbenchSystem` profiles.
* Applied the existing ballistics projection at `CombatHostSession` encounter
  token creation.
* Added the explicit optics output item and a host attach action that consumes
  the completed optic after mounting it.

Tests:

* Focused B99 coverage: 23 passed.

Divergences:

* No new player-facing optics panel was added. The existing B75 panel remains
  the ballistics route; UI completion is outside this backend slice.

## Phase 3 — B100

Status: PASS

Changed:

* Added `glassworks_recipes.json` and its Core loader/validation surface.
* Projected glassworks recipes into the existing Silent Foundry heat machine.
* Bound the catalog in both normal expansion setup and standalone fallback.
* Registered the catalog with the utilization scanner.

Tests:

* Focused B100 coverage: 5 passed.

Divergences:

* Existing glass item IDs were reused as outputs and feedstock; no duplicate
  glass item authority or extra production system was introduced.

## Phase 4 — B101

Status: PASS

Changed:

* Added additive per-train transmission wear, service-required state, and
  service-day persistence to `RailwaySystem`.
* Added deterministic, day-guarded `RailwaySystem.TickDay`, including
  campaign progression for en-route trains.
* Added the canonical transmission service action and reset transmission
  state when draisine recovery succeeds.
* Registered railway daily advancement in `Main.TickPlans190_193`.

Tests:

* Railway focused coverage includes campaign tick idempotency, persistence,
  service material consumption, and draisine recovery restoration.

Divergences:

* Transmission wear is additive to `TrainState`; `VehicleGarageSystem`
  remains the owner for overland vehicles.
