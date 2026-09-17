# ASHFALL Batch 1 — Vertical-Slice Implementation Plan

## Summary

Deliver the 16 roadmap items as dependency-ordered Godot/Core vertical slices. Each slice includes authoritative Core behavior, a thin Godot host adapter, UI integration, persistence, deterministic tests, and a headless acceptance gate.

Current baseline is healthy: Core tests pass (2,020), both .NET projects build cleanly, data integrity passes with 0 errors, and the Bridge self-test passes 41/41.

Unity remains untouched and read-only.

## Dependency-ordered delivery

### Phase 0 — Shared campaign seams

1. Refactor `Main.CommitAdvance()` and `TickSimDay()` behind a `CampaignDayCoordinator`.

   - Capture pre-day snapshots.
   - Advance all systems exactly once.
   - Collect typed state-change events.
   - Persist state before presenting blocking UI.
   - Emit a single `DayAdvanced` event.
   - Prevent double-click or re-entrant day advancement.
   - Use a host modal gate instead of pausing the entire Godot tree, so typewriter animation and audio continue.

2. Add Core `DailyBriefingReportBuilder` and `DailyBriefingState` for item 01.

   - Report sections: survivor changes, resource consumption, weather forecast, radio intercepts, expedition milestones, deaths, and warnings.
   - Use plain DTO inputs; Core must not reference host systems.
   - Sort entries deterministically by category, survivor/item ID, and event order.
   - Save unacknowledged reports through a checksummed store.
   - Implement `DailyBriefingModal` with CRT styling, typewriter animation, clicker audio, skip-to-complete input, and “Acknowledge”.
   - Acceptance: advancing a day produces one accurate report and blocks further simulation until acknowledgement.

### Phase 1 — Shelter survival loop

3. Add `PowerGridSystem` and `PowerGridHostSession` for item 13.

   - Track generation, fuel, battery reserve, room draw, breaker state, priority, and brownout effects.
   - Add `CaptureState/RestoreState`, typed events, and deterministic daily/hourly ticks.
   - Define room loads and failure effects in `Assets/StreamingAssets/Data/power_grid.json`.
   - Apply power consequences through host adapters to air filtration, water, clinic, greenhouse, foundry, and lighting.
   - Implement `PowerGridPanel` with breaker controls and live reserve/load/fuel displays.

4. Add `ShelterAssignmentSystem` for item 3.

   - Store survivor-to-room/workstation assignments, capacity, eligibility, and assignment status.
   - Reuse `HoldfastInteriorView`, `RoomHotspotView`, and `SurvivorActorView`; do not create a duplicate shelter renderer.
   - Add drag/click assignment, room capacity validation, radiation/fatigue/idle badges, and power-status indicators.
   - Persist assignments with shelter operations state.

5. Replace the fixture-backed survivor detail flow for item 2.

   - Add a real `SurvivorInspectionHostSession` query model over `NeedsSystem`, `RadiationSystem`, roster, inventory, caregiving, flashbacks, traits, skills, and trauma.
   - Route the existing `SurvivorDetailPanel` from the survivor roster; remove hard-coded live content while retaining fixtures only for isolated snapshots.
   - Implement atomic commands for feeding, rest assignment, bandaging, iodine, anti-rad treatment, and speaking.
   - Return stable failure codes and resource deltas so the UI cannot partially consume items.
   - Acceptance: selecting a living survivor shows current state and potassium iodide consumes exactly one valid item while updating dose state.

6. Complete greenhouse actions for item 10.

   - Extend the existing `GreenhousePanel` with plant, water, treat, clear, and harvest controls.
   - Route every action through `GreenhouseHostSession` and the live expansion hub.
   - Integrate clean water, power allocation, heat/frost state, contamination, blight, and inventory rewards.
   - Ensure daily ticking occurs only through `CampaignDayCoordinator`.

7. Add the medical ward authority for item 11.

   - Create Core `MedicalWardSystem` with bed categories, patient/staff assignments, procedure definitions, supply costs, isolation state, and clinical results.
   - Compose existing `MedicalSystem`, `DiseaseSystem`, `DoseLedgerSystem`, respiratory degeneration, and combat trauma systems rather than duplicating their rules.
   - Implement deterministic procedures for bandage, transfusion, chelation, anti-rad treatment, surgery, and isolation.
   - Extend `MedicalPanel` with ward slots, patient vitals, dose graphs, staffing, and procedure controls.
   - Persist ward state through the existing medical save ownership.

8. Add the weather atmosphere layer for item 8.

   - Create a Godot `AtmosphereOverlay` under the HUD.
   - Map authoritative `WeatherSystem.Current` values to tint, ash/fog/rain particles, visibility, and audio intensity.
   - Use existing audio cues and UI textures; resolve art through `AssetRegistry`.
   - Cap particle counts and disable visual-only work safely in headless mode.
   - Acceptance: weather transitions update both HUD atmosphere and soundscape without altering Core weather rules.

9. Add the death-to-memorial pipeline for item 15.

   - Create Core `MemorialSystem` and `MemorialEntry` state.
   - Subscribe to roster, needs, radiation, combat, and trauma death paths through one idempotent death bridge.
   - Record cause, day, survival duration, final-wish status, epitaph, heirloom, and morale effect.
   - Complete or fail final wishes before recording the final memorial state according to existing Core rules.
   - Transfer heirlooms atomically; unresolved recipients return items to storage.
   - Use `wasteland_grave_epitaphs.json` if present, otherwise add it as a versioned JSON authority.
   - Implement `MemorialLedgerPanel` and death modal.
   - Acceptance: one death creates exactly one ledger entry, removes the survivor from the living roster, applies the correct heirloom/morale effects, and survives save/load.

### Phase 2 — Exploration, encounters, and combat

10. Implement the authoritative travel map for item 4.

   - Use `wasteland_map_v1.json` for canonical nodes, coordinates, visibility, and route edges; `locations.json` remains descriptive location authority.
   - Add Core map discovery state and deterministic route planning.
   - Replace `MapAtlasPanel` demo definitions and inferred sectors with live catalog data.
   - Extend `WastelandMapView` and marker scenes for fog-of-war, route hazards, distance, weather danger, and expedition progress.
   - Launch expeditions from a selected destination and loadout.
   - Acceptance: undiscovered nodes remain hidden, authored routes produce stable paths, and launching from the map creates a real expedition.

11. Complete the encounter choice flow for item 5.

   - Keep `ExpeditionEncounterBridge` as the authoritative encounter selector.
   - Extend encounter choice view models with requirements, eligibility, skill/item checks, risk text, consequences, and loot summaries.
   - Resolve choices atomically through `ExpeditionHostSession`.
   - Persist pending encounters and prevent duplicate resolution or duplicate rewards.
   - Remove the current automatic demo-combat hook from `OnEncounterTriggered`.
   - If a choice produces combat, emit a typed `CombatStartRequest` and keep the campaign blocked until combat resolves.

12. Upgrade combat for item 6.

   - Extend Core tactical state with grid/lane position, cover, aim state, ammo, weapon condition, reload, and deterministic hit-probability breakdowns.
   - Add actual encounter combat initialization to `CombatHostSession`; retain `StartDemoCombat` only for self-tests.
   - Implement Aim, Snapshot, Burst, Take Cover, Reload, Flee, and existing trauma/weapon actions.
   - Replace the current list-only panel with a 2D arena overlay and tactical action bar.
   - Return a typed combat result to the expedition, including casualties, injuries, loot, and retreat status.
   - Acceptance: identical seed and state produce identical combat outcomes and save/load preserves an active battle.

### Phase 3 — Signals, production, economy, and factions

13. Complete the analog radio console for item 7.

   - Use existing radio corpus/catalog frequencies; do not hard-code new frequencies.
   - Add continuous tuning, signal lock thresholds, VU strength, transcript history, and journal unlocks.
   - Extend `RadioPanel` with a rotary/slider tuner and live signal display.
   - Drive static, tuning, lock, and broadcast audio through `AudioManager` and `AudioCueCatalog`.
   - Acceptance: tuning changes static intensity, locks authored broadcasts, and records decoded content.

14. Complete Silent Foundry controls for item 9.

   - Add charge hopper, recipe selection, temperature gauge, heat-cycle progress, tap/cast controls, and labor dispute modal.
   - Route all actions through `SilentFoundryHostSession`.
   - Keep material consumption and alloy production inside the Core/host transaction boundary.
   - Test repair, maintenance, preheat, tapping, production, overtime, strike resolution, inventory changes, and save/load.

15. Complete greenhouse cultivation controls for item 10.

   - Add six-bed or catalog-defined bed widgets, water allocation, growth status, frost/blight warnings, and harvest actions.
   - Use existing seed/item IDs and the live greenhouse state.
   - Verify power and clean-water shortages affect production through host adapters, not UI logic.

16. Complete caravan trading for item 12.

   - Extend the existing `CaravanBarterLedgerPanel` and `TradeScreenGodotPanel`; do not create a second barter engine.
   - Bind arrival state, merchant inventory, player offer, transport limits, regional pricing, trust, stance, and debt vouchers.
   - Use an atomic quote/commit flow with no duplicate transaction rewards.
   - Acceptance: live quote values update as goods change, invalid offers are rejected without mutation, and completed trades update shared inventory and caravan state.

17. Add the warlord radar for item 14.

   - Bind directly to `WarlordDoctrineSystem` through `YearOfAshHostSession`.
   - Reuse `FactionWarMapWidget` rendering where practical and add `WarlordRadarPanel` for sector state, raid trajectory, ETA, tribute, and response actions.
   - Use existing territory/action events; do not duplicate faction state in UI.
   - Add deterministic tribute settlement and player response commands where the current Core API lacks them.
   - Acceptance: claim, raid, tribute, contest, and response actions update the radar, warnings, journal, and saved state.

### Phase 4 — End-game chronicle

18. Extend the existing epilogue for item 16.

   - Keep `EpilogueMatrixRuntime`, `EpilogueContextFactory`, and existing ending evaluation authoritative.
   - Add Core `EpilogueChronicle`/`EpilogueSlide` DTOs for ordered slides, survivor fate cards, metrics, prose, and authored `art_asset_id` values.
   - Store presentation-only slide data in a versioned `epilogue_chronicle.json`; do not move ending logic into UI.
   - Trigger the sequence from the existing canonical completion condition and emit it once.
   - Upgrade `EpiloguePanel` to a carousel with next/back/skip, accessible text, illustrated slides, and final survival breakdown.
   - Acceptance: the same campaign state always produces the same ending, slide order, metrics, and survivor fate cards.

## Persistence, data, and file boundaries

- New stateful Core systems must implement `CaptureState/RestoreState`, versioned DTOs, deterministic RNG, and checksum-protected persistence.
- Existing owning saves remain authoritative for greenhouse, foundry, disease, expedition, combat, radio, economy, and world state.
- Add dedicated checksummed state only for newly independent daily briefing, shelter operations/power, and memorial state.
- Missing fields in older saves default safely; future save versions are rejected; malformed new envelopes are never treated as legacy saves.
- Use `IJsonSerializer`, `IFileIO`, and canonical snake_case JSON. Never add `JsonUtility`, `System.Random`, `Guid.NewGuid()`, Unity dependencies, or gameplay logic to `Assets/_Game/`.
- Preserve all pre-existing working-tree JSON changes and merge new catalog fields additively.
- Primary additions/extensions are in:
  - `Assets/Ashfall.Core/` domain systems and tests.
  - `src/Host/`, `src/Main*.cs`, and existing host sessions.
  - `src/UI/`, `src/World/`, `Assets/StreamingAssets/Data/`, and existing Godot-native assets.

## Verification and delivery gates

For each accepted vertical slice:

- Add Core behavior, determinism, invalid-input, and save round-trip tests.
- Add host integration tests for inventory atomicity, event ordering, and campaign-day wiring.
- Add or extend `HostCli` headless self-tests and `SnapshotHarness` coverage for the affected panel.
- Run the canonical verification:

  1. `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
  2. `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
  3. `dotnet build Ashfall.csproj`
  4. `godot --headless --path . -- --data-integrity-selftest`
  5. `godot --headless --path . -- --bridge-selftest`

- Add a separate independent diff review for every new system containing multiple coupled state variables, satisfying the cross-tool QA rule.
- Commit each accepted slice separately; do not combine all 16 items into one change.

## Assumptions and defaults

- The selected delivery strategy is dependency-first vertical slices.
- Existing panels are extended before new panels are created; fixture data remains only for isolated snapshot/self-tests.
- New artwork is not required for Batch 1; existing Godot-native assets and `AssetRegistry` are used.
- Audio and particle presentation receives headless state tests plus interactive Godot acceptance checks; headless tests remain authoritative for simulation correctness.
- The current passing baseline supersedes stale historical issue counts in `REPO_REVIEW_REPORT.md`.
