# GAP-48/49 — Destination Weather Gates & Micro-Location Bindings — Implementation Log

Plan: `docs/gaps/plans/GAP-48-49_DESTINATION_SEAMS_SEALING_PLAN.md`

## Phase 1 — Validation

**Broken link before:** `weather_route_gates.json` (15 route gates) and
`micro_locations.json` (25 encounters) were data authorities with zero C#
consumers. Plan 76's deferred destination-level seams.

**Change:** re-validated against the working tree. GAP-49's original
"no destination-level binding field exists" finding was **STALE**: the live
`EncounterDefinition.requiredLocationId` + `GetEffectiveWeight` chain fully
supports destination binding; `narrative_encounters.json` already carries 5
location-bound encounters. GAP-48A confirmed VALID (no loader, no evaluator,
no dispatch consumer).

**Result:** PASS

## Phase 2 — Core behavior (GAP-48A)

**Broken link before:** no `WeatherRouteGateCatalog`, no evaluation.
**Change:** `Assets/Ashfall.Core/World/WeatherRouteGateCatalog.cs` — DTO,
target-indexed catalog, pure `IsGateBlocking`/`EvaluateBlock` evaluation
(blocked-weather / required-weather semantics, override-item lift,
case-insensitive weather match), `LoadFromDirectory` with
`CatalogDiagnostics` on parse failure. Zero engine references.
**Chain after:** data → loader → evaluation [OK].
**Tests:** `WeatherRouteGateCatalogTests` (8).
**Result:** PASS

## Phase 3 — Data

**Change:** `weather_route_gates.json` +3 destination gates
(Silent Observatory/Blizzard+IceStorm, Flooded Subway Depot/BlackRain,
Shallows Market/FalloutStorm); `micro_locations.json` +3
`requiredLocationId`-bound encounters for the Plan 76 §35 targets
(`abandoned_hospital`, `location_flooded_subway_depot`,
`loc_garrison_checkpoint_gamma`). One authored invalid item id
(`candle`) caught in review and replaced with `cigarette_lighter`.
**Verification:** integrity selftest 0 findings; loader test.
**Result:** PASS

## Phase 4 — Core wiring (GAP-49B)

**Broken link before:** `micro_locations.json` unconsumed.
**Change:** `NarrativeEncounterCatalogLoader.Load` adds
`MicroLocationsFileName` as a third source (Plan 52 multi-file precedent).
**Chain after:** micro_locations.json → loader → EncounterCatalog →
GetEffectiveWeight(locationId) → surfacing → NarrativeEncounterState save [OK].
**Tests:** loader + binding + weight tests.
**Result:** PASS

## Phase 5 — Host wiring + UI (both gaps)

**Broken link before:** `ExtraBlocked: Func<string,bool>` could not carry a
reason; panel hardcoded "[CROSSING GATE CLOSED — no vouch]" for any block.
**Change:** `ExpeditionHostSession` gains `ExtraBlockReason:
Func<string,string?>` + `GetBlockReason(locationId)` (crossing gate → boolean
extra gate → reason gate); `IsLocationBlocked` delegates;
`StartExpedition`/`DispatchSortie` reject on either gate;
`Main.Maritime.cs` composes weather-gate evaluation (override via
`_inventory.Inventory.CountById`); `ExpeditionPanel` renders the real reason
label instead of the hardcoded crossing text.
**Verification:** `dotnet build Ashfall.csproj` 0/0; expedition selftest;
faction-ecology selftest; content-utilization gate.
**Result:** PASS (working tree)

## Phase 6 — Persistence

No save changes required: gate state is derived from current weather (Plan 48
authority), micro-location resolution already persists via
`NarrativeEncounterState`. PASS

## Phase 7 — Commit scoping note

`src/Host/ExpeditionHostSession.cs`, `src/Main.Maritime.cs` and
`src/UI/ExpeditionPanel.cs` carry interleaved concurrent-workstream changes
(Plan 60 vehicle-kit bridge). The commit for this seal includes only the pure
subset (Core catalog + loader extension + data + tests + docs + changelog);
the three wiring files remain uncommitted in the working tree with all gates
green, and land with that workstream's commit. The committed tree builds
stand-alone: the catalog and loader are additive and self-contained.

## Status

SEALED in the working tree (all verification gates green).
Committed subset: Core + data + tests + docs.

## Phase 8 — GAP-48B: force passage (follow-up)

**Broken link before:** `consequence_on_force` was dead data — no force
action existed; blocked weather gates were absolute.
**Change:** `WeatherGateDef.force_stamina_cost` (data-driven); the block
carries `ForceStaminaCost`/`ForceConsequence`;
`ExpeditionSystem.Start/ExecuteStart` gain an optional clamped
`startingStamina` (Core, Invariant 5); `StartExpedition`/`DispatchSortie`
gain `forceWeatherGate` — a forced sortie starts `MaxStamina − cost`;
the dispatch bar renders a FORCE PASSAGE action with the consequence as
tooltip when a forceable gate blocks. Gates without a cost stay absolute;
the boolean extra gate (ice road / deep coast) can never be forced.
**Tests:** `ForcePassageTests` (3): startingStamina apply+clamp; force
lookup requires block + positive cost; override lift; authored gates carry
costs + consequence prose.
**Verification:** build 0/0; full suite 6848/6848; all selftests + CI gate.
**Result:** PASS

## Phase 9 — GAP-48B completion: radiological force consequences

**Broken link before:** forced entry through radiological gates (black rain,
fallout storm) applied only the stamina cost — the thematic radiation dose had
no path.
**Change:** `WeatherGateDef.force_rad_dose` (data); `WeatherGateBlock.ForceRadDose`;
`ExpeditionHostSession.OnWeatherGateForced(survivorId, locationId, block)`
raised on successful forced dispatch; Main routes the dose to the radiation
owner (`_survivors.Radiation.AdjustDose(rad, dose)` — the authoritative
path used by Phase 0 and the real-campaign journey). Blizzard gate stays
stamina-only; depot (15) and shallows (20) dose on force.
**Data:** `force_rad_dose` on the two radiological destination gates.
**Tests:** `ForcePassageTests` extended — radiological gates dose, cold gate
does not; override lift applies before any dose.
**Verification:** build 0/0; scoped tests 11/11; full suite 6866/6868→6866/6866
green after the concurrent Plan 85 pin updates (they added 12 destinations,
all table-bound, reusing Plan 76.1 tables).
**Result:** PASS
