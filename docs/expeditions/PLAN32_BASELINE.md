# Plan 32 Baseline: Expedition Destination Wiring (2 → 50 Destinations)

## 1. Executive Summary

- **Context:** ASHFALL possessed 142 canonical locations in `locations.json` and active expedition dispatch, travel, stamina, encounter, vehicle, and save systems, but only 2 destinations wired into `expeditions.json`.
- **Mission:** Wire 48 additional canonical destinations into `expeditions.json`, bringing the total from 2 to 50 dispatchable destinations without creating new runtime code or modifying save schemas.
- **Architectural Directive:** `locations.json` is the sole geographic and environmental authority; `expeditions.json` is a pure gameplay projection.

## 2. Baseline Inventory

- **Canonical Locations in `locations.json`:** 142 definitions.
- **Eligible Surface Locations:** 134 candidate sites (excluding 8 internal bunker rooms/facilities).
- **Initial Wired Destinations in `expeditions.json`:** 2
  - `loc_the_allotments` (The Works Allotment Commune) — Scavenge (Danger 2, Distance 5 ticks)
  - `loc_denial_cut_substation` (The Denial Cut Substation) — Standard (Danger 4, Distance 8 ticks)
- **Target Expansion:** 50 destinations total ($2 \text{ existing} + 48 \text{ new}$).

## 3. Schema & System Seams

- **Loader:** `ExpeditionCatalogLoader.cs` in `Assets/Ashfall.Core/Expeditions/`.
- **Registry:** `ExpeditionDefinitionRegistry.cs`.
- **Runtime Engine:** `ExpeditionSystem.cs` (Phase management, stamina drain, tick travel, push-your-luck looting, auto-retreat).
- **Host Session:** `ExpeditionHostSession.cs` (Garage vehicle dispatch, UI feeds, event listeners).
- **Data Authority:** `Assets/StreamingAssets/Data/expeditions.json`.

## 4. Verification Baseline

- `CatalogIntegrityValidator`: Enforces Tier-1/Tier-2 ID resolution for all `loc_*` IDs and real item strings.
- Headless Self-Tests: `--data-integrity-selftest` and `--expedition-selftest`.
- xUnit Test Suites: `Plan32ExpeditionDestinationWiringTests.cs`, `ExpeditionSystemTests.cs`, `ExpeditionVehicleLogisticsTests.cs`.
