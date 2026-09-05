# ASHFALL Plans 94–97 Authority and Reconnaissance Map

**Scope:** Vehicle track gear, grain processing and silo safety, heliograph
signaling, and abstract cryogenic air separation requested as the current
ASHFALL implementation batch.

**Status:** Reconnaissance complete. Implementation has not started.

## 1. Naming and repository divergence

The requested work uses “Plans 94–97” as a new batch label. The repository
already contains documents named Plan 94, but those documents describe the
completed Verdict machine-register radio expansion. They are unrelated to the
vehicle, grain, heliograph, and cryogenic systems in this batch.

The existing Verdict Plan 94 implementation must not be modified for this
work. New runtime authorities should use distinct system IDs and save keys.

The repository does not currently contain live Core engines/catalogs under the
requested grain, heliograph, or cryogenic system names. Existing related
content is adjacent infrastructure or narrative/catalog material:

- `GrainMillingCatalog` contains authored milling, sifting, silo, and tempering
  records, but it is not a stateful processing authority.
- `CryogenicPermafrostCorePanel` is a presentation stub and is not an
  authority.
- Existing radio, weather, map, power, medical, kitchen, and foundry systems
  provide integration seams, not implementations of the requested systems.

## 2. One-authority map

| Concern | Canonical authority | Boundary for Plans 94–97 |
|---|---|---|
| Engine-agnostic simulation | `Assets/Ashfall.Core/` | All new state, rules, transactions, events, and DTOs live here. |
| Godot presentation and wiring | `src/` | Hosts forward commands, bind typed state, register saves, and refresh panels. They do not calculate outcomes. |
| Authored data | `Assets/StreamingAssets/Data/` | New definitions use JSON, `schema_version`, snake_case IDs, and existing catalog validation. |
| Inventory quantities | `Ashfall.Core.Inventory.Inventory` | Inputs and outputs use inventory bills/transactions. No subsystem maintains shadow item counts. |
| Campaign time | Existing campaign-day and tick wiring | Systems receive the established day/tick boundary; they do not create clocks. |
| Random outcomes | Injected `ISeededRng` | Fixed draw order, deterministic replay, and no `System.Random`. |
| Survivor identity/lifecycle | `SurvivorEntityStore` | Systems store survivor IDs and resolve through canonical adapters. They do not copy survivor records. |
| Traits and skills | Existing survivor/skill adapters | Worker bonuses are read through the canonical trait/skill path. No local trait ledger. |
| Campaign persistence | `SaveSectionRegistry` and `SaveStoreHub` | Each stateful authority gets one registry key and one checksummed/codec-backed section. |
| Content reachability | `ContentUtilizationScanner` and runtime collector | Every new catalog is loaded and queried by a runtime consumer or is reported as intentionally inert. |

## 3. Plan 94 — vehicle track gear

### Existing seam

`ExpeditionVehicleSystem` owns the garage vehicle instance: ownership,
condition, fuel, attachments, repair, preparation wear, and deterministic
breakdown. `ExpeditionSystem` owns the active sortie: travel ticks, vehicle
speed/cargo projection, breakdown transition, and expedition persistence.
`ExpeditionHostSession` bridges the garage instance into an
`ExpeditionVehicleProfile` at dispatch and persists the aggregate garage plus
sorties.

### Authority decision

Track gear is an extension of the existing vehicle authority, not a new travel
simulator. The first slice should:

1. add persisted track-gear state to the garage vehicle/definition seam;
2. project only the normalized mobility facts needed by
   `ExpeditionVehicleProfile`;
3. let `ExpeditionSystem` consume those facts during its existing travel tick;
4. keep fuel, wear, breakdown, cargo, and route state in their current owners.

Terrain effects must remain normalized gameplay modifiers. The implementation
must not model real drivetrain temperatures, torque curves, or engineering
measurements.

### Persistence and tests

The existing `expedition` aggregate section remains the save authority. No
second vehicle save file is permitted. Tests must cover attachment/state
round-trip, profile projection, terrain modifier determinism, breakdown
parity, and legacy aggregate loading.

## 4. Plan 95 — grain processing and silo safety

### Existing seam

`Inventory.Inventory` is the quantity authority and provides
`InventoryBill`, transaction quoting, rollback, and atomic execution.
`KitchenNutritionSystem` owns prep jobs, pantry portions, spoilage, meals, and
nutrition effects. Existing narrative grain records are data inputs only.

### Authority decision

Create one Core grain-processing authority for grain lots, milling/sifting
jobs, silo condition, pest pressure, and processing outcomes. It may own
process state and spoilage/pest state, but it must never own duplicate item
quantities. Starting a job consumes inputs atomically; completion grants
canonical output items atomically. A failed validation or grant leaves the
inventory unchanged.

Kitchen integration should happen through canonical inventory items and
existing kitchen recipe/prep inputs. The kitchen remains the owner of meal
production and survivor nutrition. The grain authority must not call meal
serving or mutate needs directly.

Pest and silo safety are abstract bands and thresholds suitable for a survival
management game. Do not introduce real-world grain-storage instrumentation.
Worker traits may modify authored processing risk or throughput only through
the canonical survivor adapter.

### Persistence and tests

The grain authority receives one dedicated registry section and a
checksummed save store. Tests must cover atomic input/output behavior,
job/tick progression, pest/silo transitions, kitchen-consumable output,
deterministic seeded outcomes, and capture/restore without extra rolls.

## 5. Plan 96 — heliograph signaling

### Existing seam

`SignalTriangulationSystem` owns radio directional observations and
triangulated candidates. `RadioDistressSystem` owns distress lifecycle,
dispatch, expiry, and resolution. `WastelandMapSystem` owns discovered nodes,
route topology, locks, and deterministic route planning. Weather systems
provide existing weather severity/context rather than a second weather model.
`CommsArraySystem` owns powered long-range array state and target locks; it is
not the optical signaling authority.

### Authority decision

Create one Core heliograph authority for optical station state, signal jobs,
line-of-sight/weather gating, and deterministic message delivery. It should
use existing weather context and map/node facts through narrow adapters.
It must not duplicate radio triangulation observations or create a second
distress lifecycle.

When a heliograph message represents a distress call, dispatch must route
through `RadioDistressSystem` or its existing expedition handoff seam.
Discovery and route availability must flow through `WastelandMapSystem`.
Line-of-sight and weather should be bounded gameplay gates, not a physical
ray-tracing or atmospheric simulator.

### Persistence and tests

The heliograph authority receives one dedicated save section. Tests must
cover LOS/weather blocking, successful deterministic transmission, station
state round-trip, one-shot dispatch handoff, map discovery/lock behavior, and
no mutation on rejected commands.

## 6. Plan 97 — cryogenic air separation

### Existing seam

`PowerGridSystem` owns generation, draw, room power, brownouts, breakers, and
daily power ticks. `CupolaFoundryEngine` owns foundry batches, power checks,
hazards, inventory bills, and persistence. Medical systems own treatment and
clinical effects. Inventory remains the material quantity authority.

### Authority decision

Create one abstract cryogenic air-separation authority for plant condition,
run state, normalized gas-product outputs, power gating, and deterministic
failure bands. It must not simulate real cryogenic thermodynamics or expose
unsafe engineering instructions. Inputs and outputs use atomic inventory
transactions.

Medical and foundry consumers should read canonical product availability or
consume canonical inventory items through their existing authorities. The
cryogenic system must not duplicate medical treatment state, foundry batch
state, or power allocation logic. The existing cryogenic/permafrost panel
must remain presentation-only until it is bound to this authority.

### Persistence and tests

The authority receives one dedicated registry section and checksummed store.
Tests must cover power/brownout gating, atomic product grants, normalized
condition/failure transitions, medical/foundry consumer integration,
deterministic seeded failure, and save round-trip.

## 7. Host, panel, and save rules

Each new host session follows the existing thin-session pattern:

- construct the Core authority with injected inventory, RNG, clock/context,
  and narrow callbacks;
- load JSON through the existing Core/host catalog conventions;
- forward button commands and expose typed snapshots;
- subscribe/unsubscribe to Core events;
- capture/restore through `SaveStoreHub`;
- enroll exactly one section in `SaveSectionRegistry`;
- bind a panel to state, blocker, cost, and consequence.

New panels must use the existing ASHFALL UI helpers and theme. They must not
render raw item IDs or embed gameplay constants. If a panel is currently a
stub, its hardcoded presentation content is replaced only after the Core
binding exists.

## 8. Content-utilization contract

Every new definition ID must be reachable from a registered JSON catalog,
loaded by a live authority, and exercised by a representative runtime query
or selftest. The implementation closeout must report:

- catalog files and loader owners;
- runtime consumer and representative query;
- no orphaned catalog/definition findings;
- any unresolved references as pre-existing or explicitly reviewed.

## 9. Required implementation order

1. Vehicle track gear on the existing garage/expedition seam.
2. Grain processing and silo safety, then kitchen output consumption.
3. Heliograph signaling, then map/dispatch handoff.
4. Cryogenic air separation, then medical/foundry consumers.
5. Panels, save registration, content-utilization evidence, and full gates.

The order keeps each new system attached to an existing authority before the
next cross-system consumer is added. No Unity files, Unity commands, or new
parallel simulation islands are in scope.
