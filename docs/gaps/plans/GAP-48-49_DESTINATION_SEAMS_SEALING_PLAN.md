# GAP-48/49 — Destination-Level Weather Gates & Micro-Location Bindings — Sealing Plan

Sequel to `docs/expeditions/PLAN76_CLOSEOUT.md` (deferred items) and
`docs/weather/PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md` (integration path).

## 1. Gap

- **GAP-48A**: `weather_route_gates.json` (15 gates) is a reference-clean data
  authority with **no runtime consumer** on expedition dispatch. Plan 76's
  deferred "destination-level weather gates" item.
- **GAP-49B**: `micro_locations.json` (25 encounters) has **no C# consumer at
  all** (CATALOG_REGISTRY: UNRESOLVED). The live `EncounterCatalog` chain
  already implements location-bound weighting (`requiredLocationId` →
  `GetEffectiveWeight`), so destination-bound micro-locations are unreachable
  data. Plan 76 §35 deferred the three destination bindings.

## 2. Evidence

- `PLAN_48_WEATHER_ROUTE_GATES_CLOSEOUT.md`: "No runtime consumer currently
  enforces weather gates on expedition dispatch"; integration path steps 1–6
  prescribes the dispatch check + override evaluation + reason surfacing.
- `grep micro_locations Assets/Ashfall.Core src` → 0 hits.
- `NarrativeEncounterSystem.PickEncounter` selects by
  `GetEffectiveWeight(stance, dangerLevel, locationId)` with `ISeededRng`.
- `ExpeditionHostSession.ExtraBlocked: Func<string,bool>` composed at
  `Main.Maritime.cs:66` (IceRoad ‖ DeepCoast); UI renders a hardcoded
  "[CROSSING GATE CLOSED — no vouch]" label for any block
  (`ExpeditionPanel.cs:407`) — becomes false once weather blocks exist.

## 3. Current broken chain

```text
weather_route_gates.json → [loader MISSING] → [evaluation MISSING]
  → ExtraBlocked seam [OK, unwired for weather] → dispatch block [MISSING]
  → UI reason [WRONG — hardcoded crossing text]
micro_locations.json → [loader MISSING] → EncounterCatalog [OK]
  → GetEffectiveWeight(locationId) [OK] → surfacing [OK] → save [OK]
```

## 4. Missing links

1. Core loader + evaluator for weather gates (Core logic gap).
2. Destination-targeted gate entries (data gap; existing gates target
   caravan route IDs only).
3. `micro_locations.json` merged into `NarrativeEncounterCatalogLoader`
   (wiring gap; Plan 52 `ArcFileName` precedent).
4. Location-bound micro-location entries for the three Plan 76 §35 targets
   (data gap).
5. Block-reason plumbing: `ExtraBlocked: Func<string,bool>` →
   reason-carrying API; UI label uses the real reason (UI gap).

## 5. Target behavior

- Dispatch eligibility (and the dispatch-bar label) reflect weather gates:
  a destination with a gate is blocked while the gate's weather is current,
  unless the override item is present in shelter inventory.
- Micro-location encounters bound to a destination can surface on approach
  via the existing seeded `PickEncounter` path, resolve through the existing
  choices/morale/guilt/loot path, persist via `NarrativeEncounterState`.
- No save changes (gate state derived; micro-location state already saved).

## 6. Ownership

- Gate catalog + evaluation: `Assets/Ashfall.Core/World/` (engine-agnostic).
- Loader extension: `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`
  (existing loader, Plan 52 pattern).
- Composition: `src/Main.Maritime.cs` (`ExtraBlockReason` lambda; weather via
  `_world.Weather.Current`; overrides via `_inventory.Inventory.CountById`).
- Data: `Assets/StreamingAssets/Data/weather_route_gates.json`,
  `micro_locations.json`.

## 7. Selected approach

**Option A — minimal wiring**, reusing existing seams: `ExtraBlocked` →
`ExtraBlockReason` (reason-carrying), `NarrativeEncounterCatalogLoader`
multi-file pattern, `CatalogLocator` wrapped-list loading,
`CatalogDiagnostics` on parse failure. No new systems, no save changes, no
RNG changes (gate evaluation is pure; micro-location selection already seeded).

## 8. Data changes

- `weather_route_gates.json`: +3 destination gates
  (`gate_type: "destination"`): Silent Observatory (Blizzard), Flooded Subway
  Depot (BlackRain), The Shallows Market (FalloutStorm) — the authored-catalog
  adaptation of Plan 76 §37's frozen-wetland/irradiated-forest intent.
- `micro_locations.json`: +3 location-bound encounters for Plan 76 §35
  targets: `abandoned_hospital`, `location_flooded_subway_depot`,
  `loc_garrison_checkpoint_gamma` (`requiredLocationId` set).

## 9. Tests

- `WeatherRouteGateCatalogTests`: loader counts; evaluation matrix
  (blocked-weather hit/miss, required-weather semantics, override-item lift,
  no-gate pass-through); destination-target resolution.
- `ExpeditionHostSession` integration: composed reason lambda blocks
  `IsLocationBlocked`/`StartExpedition` and surfaces the reason.
- `NarrativeEncounterCatalogLoader` loads the three bound micro-locations;
  `GetEffectiveWeight` is 0 off-destination, >0 on-destination.

## 10. Rollback

Revert the commit; no save/model migration exists to undo (gate state is
derived; micro-location entries simply stop loading).
