# Plan 43 Baseline & Scope

## 1. Goal
Establish an authoritative catalog of 12 living survivor settlements in `Assets/StreamingAssets/Data/settlements.json`, providing a persistent social geography independent of the player with population, trade flows, faction allegiance, threat exposure, and physical location bindings.

## 2. Rationale
- The wasteland previously consisted primarily of ruins and scavenging locations in `locations.json`.
- Settlements create the anchor points for trade caravans (Plan 16B), faction territory (Plan 44), patrols (Plan 45), and refugee movement (Plan 18C).
- Proof of four caravan network endpoints and three friendly expedition stops connects static catalog definitions directly into live gameplay loops.

## 3. Scope Boundaries
- **In Scope:**
  - Authoritative `settlements.json` with 12 canonical living settlements.
  - Integration with `SettlementCatalog.cs` in `Ashfall.Core.World`.
  - Physical location linkage in `locations.json`.
  - Caravan route integration in `caravans.json`.
  - Friendly expedition stops in `expeditions.json`.
  - Validation test suite in `Ashfall.Core.Tests/World/SettlementCatalogTests.cs`.
- **Out of Scope:**
  - Procedural settlement economy simulator.
  - City-builder or housing construction engine.
  - Dynamic real-time faction war ticks (deferred to Plan 44).
