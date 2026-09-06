# PLAN 47 — Collectible Integration Closure Report

**Scope:** Tasks 5–8 (Cross-Plan Verification, Collectible Onboarding, Cartography & Deterministic E2E Campaign Proof)
**Date:** 2026-09-06
**Status:** COMPLETE — All Gates Passed
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`

---

## 1. Executive Summary

Plan 47 Tasks 5–8 close the world culture and collectible feature as a fully integrated, campaign-scoped subsystem. Every documented producer→consumer contract in Plan 47 §6 is either verified as live or explicitly deferred with a concrete blocker and resolution condition. Collectible discovery origin locations are now persisted and projected onto wasteland cartography without leaking hidden information. A deterministic two-moment onboarding system guides players on their first discoveries. An end-to-end 20-scavenge campaign smoke scenario proves full lifecycle determinism, restore parity, reacquisition idempotency, and canonical SHA-256 state equality across three fresh runs.

---

## 2. Verification Summary Table

```text
PLAN 47 COLLECTIBLE INTEGRATION CLOSURE

Cross-plan integrations:
  Wired:              8
  Deferred:           1
  N/A:                0
  Unknown:            0
  Undocumented gaps:  0

Tutorial:
  Cultural Artifacts:       PASS
  Reading and Discovering:  PASS
  First-time semantics:     PASS
  Save/load:                PASS

Cartography:
  Discovery location persistence: PASS
  Marker projection:              PASS
  Same-location behavior:         PASS
  Hidden-data sanitization:       PASS
  Accessibility:                  PASS
  Save/load:                      PASS

Campaign smoke:
  Seed:                     42
  Scavenges:                20
  Collectibles first 10:    PASS (>= 3 verified)
  Effect-bearing first 10:  PASS (>= 1 verified)
  Unique invariant:         PASS (<= 1 generation per unique)
  Reacquisition idempotency:PASS (AlreadyDiscovered == true, no re-dispatch)
  Restore parity:           PASS (exact semantic equality)
  Three-run trace equality: PASS (identical event traces across 3 runs)
  Hash equality:            PASS (identical SHA-256 state hashes across 3 runs)

Integrity:
  --data-integrity-selftest:   PASS / 0 errors across 269 catalogs
  --scene-binding-selftest:    PASS / 25 passed, 0 failed
  --content-utilization-selftest: PASS / CI gate PASS
  python3 scene-lint.py:       PASS / 30 scenes, 0 errors
  dotnet build Ashfall.csproj: PASS / 0 warnings, 0 errors
  dotnet test:                 PASS / 8517 passed, 0 failed (0 regressions)
```

---

## 3. Workstream A (Task 5) — Cross-Plan Integration Matrix Verification

- **Executable Manifest:** `Ashfall.Core.Tests/Fixtures/collectibles/collectible_cross_plan_integrations.json`
- **Integration Ledger:** `docs/collectibles/PLAN_47_CROSS_PLAN_LEDGER.md`
- **Test Suite:** `Ashfall.Core.Tests/Collectibles/CrossPlanCollectibleIntegrationTests.cs`

### Status of Matrix Rows:
1. `collectible_to_scavenging`: **WIRED**. All 40 items in `collectibles.json` resolve against `items.json` with positive weights and verified categories.
2. `technical_manual_to_research`: **WIRED**. Knowledge collectibles invoke `ResearchSystem.UnlockManual(target)`, persisting in research state.
3. `map_collectible_to_world_map`: **WIRED**. Location clues invoke `WastelandMapSystem.Discover(target)` while preserving physical discovery location independently.
4. `faction_document_to_faction_intel`: **WIRED**. Faction documents route through `JournalSystem.TryDiscoverKnowledge(target)` as canonical codex knowledge authority.
5. `readable_artifact_to_journal`: **WIRED**. Readable artifacts unlock journal entries and codex entries via `JournalSystem.TryDiscoverKnowledge(target)`.
6. `vinyl_to_vinyl_system`: **WIRED**. Vinyl records in shelter inventory supply playback and morale bonuses via `VinylMoraleSystem`.
7. `cultural_object_to_folklore`: **DEFERRED**.
   - *Blocker:* No runtime `FolkloreSystem` or cultural memory API currently exists in `Ashfall.Core`.
   - *Resolution:* Wire when canonical `FolkloreSystem` is formally introduced with persistent unlock and query APIs.
   - *Temporary behavior:* Category metadata preserved in `collectibles.json`; discoveries register without attempting runtime folklore unlocks.
8. `toy_photo_letter_to_morale`: **WIRED**. Mementos apply bounded morale grants (clamped to `MaxMoraleEffectValue = 10f`) to living survivors in the shelter roster via `NeedsSystem.Modify(Morale)`.
9. `discovery_to_save_state`: **WIRED**. Discovered IDs, acknowledgement status, acquisition history, and origin locations persist via `CollectibleDiscoverySaveStore` (`SaveStoreHub.Checksummed`).

---

## 4. Workstream B (Task 6) — Tutorial & Onboarding for Collectible Discovery

- **Core Service:** `Assets/Ashfall.Core/Collectibles/CollectibleTutorialTracker.cs`
- **Localization Integration:** `Assets/Ashfall.Core/Localization/LocalizationService.cs`
- **Test Suite:** `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs` (8 tests passing)

### Implemented Mechanics:
- **Moment 1 (`cultural_artifacts`):** Triggered on the first collectible ever acquired/discovered in the campaign.
- **Moment 2 (`reading_and_discovering`):** Triggered on the first effect-bearing collectible (`HasDiscoveryEffects == true`).
- **Stable Ordering:** When the first collectible found in the campaign is also effect-bearing, queues `cultural_artifacts` first, then `reading_and_discovering` in deterministic FIFO order.
- **Idempotency & Save Safety:** Seen tutorial flags survive save/load via `CollectibleTutorialSave`. Restoring historical save states never re-triggers tutorial popups.

---

## 5. Workstream C (Task 7) — Map & Cartography Integration for Discovery Locations

- **Core Model Extension:** `Assets/Ashfall.Core/CollectibleDiscoveryState.cs`
  - `CollectibleDiscoveryLocationEntry` `{ item_id, location_id }`
  - `CollectibleDiscoverySave.discovery_locations` array (persisted ordinally sorted for `SaveChecksum` determinism)
  - `GetDiscoveryLocation(itemId)` / `MarkDiscovered(itemId, discoveryLocationId)`
- **Dispatcher Extension:** `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`
  - `DispatchOnAcquire(itemId, discoveryLocationId)`
  - `CollectibleDispatchResult.DiscoveryLocationId` and `HasDiscoveryEffects`
  - `OnCollectibleDiscovered` event
- **Cartography Projector:** `Assets/Ashfall.Core/Collectibles/CollectibleMapProjector.cs`
  - Logical marker identity: `collectible-discovery:{collectibleId}:{discoveryLocationId}`
  - Presentation clustering: `CollectibleMapProjector.ClusterByLocation(...)`
  - Strict Sanitization: Never exposes `effect_target`, hidden clue nodes, or internal dispatch keys.
  - Textual Accessibility: Accessible text provided for every marker and cluster.
- **Test Suite:** `Ashfall.Core.Tests/Collectibles/CollectibleMapIntegrationTests.cs` (11 tests passing)

---

## 6. Workstream D (Task 8) — Deterministic End-to-End Campaign Smoke Test

- **Test Suite:** `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs` (6 tests passing)
- **Fixture Specifications:**
  - Seed: `42`
  - 40 full collectibles distributed across 20 distinct sectors
  - Seeded PRNG (`Ashfall.Core.SeededRng` with SplitMix64 / xorshift64*)
- **Phase Breakdown:**
  1. *First 10 Scavenges:* Verified >= 3 collectibles found, >= 1 effect-bearing found, correct origin locations recorded, unique generation rules enforced.
  2. *State Snapshot & Save:* Full multi-system state captured (inventory, discovery, unique claims, tutorials, needs/morale, research, journal, map, RNG).
  3. *Restore Parity:* Restored into fresh runtime graph; verified exact semantic equality, zero effect replays on restore, zero historical tutorial firings.
  4. *Next 10 Scavenges:* Executed scavenges 11–20; continued deterministically.
  5. *Reacquisition Probe:* Reprocessed an already-discovered collectible; verified `AlreadyDiscovered == true`, zero morale replay, zero research replay, zero tutorial replay, zero duplicate markers.
  6. *Balance Sanity:* Proved all 40 collectibles in catalog satisfy `0 < tradeValue < 100` and `0 <= weight < 5.0 kg`.
  7. *Three-Run Determinism Proof:* Three independent runs produced identical event traces and identical canonical SHA-256 state hashes.

---

## 7. Commits & File Modification Trace

1. `Assets/Ashfall.Core/CollectibleDiscoveryState.cs`:
   - Added `CollectibleDiscoveryLocationEntry` and `discovery_locations` to `CollectibleDiscoverySave`.
   - Added `_discoveryLocations`, `GetDiscoveryLocation(itemId)`, `DiscoveryLocations` property.
   - Updated `MarkDiscovered`, `CaptureState` (ordinal sort), and `RestoreState` (legacy save safety).
2. `Assets/Ashfall.Core/Collectibles/CollectibleEffectDispatcher.cs`:
   - Added `DiscoveryLocationId` and `HasDiscoveryEffects` to `CollectibleDispatchResult`.
   - Added `OnCollectibleDiscovered` event.
   - Added `discoveryLocationId` support to `DispatchOnAcquire`.
3. `Assets/Ashfall.Core/Collectibles/CollectibleMapProjector.cs`:
   - Engine-agnostic domain classes: `CollectibleMapMarker`, `CollectibleMapCluster`, `CollectibleMapProjector`.
4. `Assets/Ashfall.Core/Collectibles/CollectibleTutorialTracker.cs`:
   - Engine-agnostic domain classes: `CollectibleTutorialEntry`, `CollectibleTutorialSave`, `CollectibleTutorialTracker`.
5. `Assets/Ashfall.Core/Localization/LocalizationService.cs`:
   - Registered `tutorial.collectible.cultural_artifacts.*` and `tutorial.collectible.reading_and_discovering.*`.
6. `Ashfall.Core.Tests/Fixtures/collectibles/collectible_cross_plan_integrations.json`:
   - Authored machine-readable 9-row manifest.
7. `docs/collectibles/PLAN_47_CROSS_PLAN_LEDGER.md`:
   - Authored Plan 47 cross-plan integration ledger.
8. `Ashfall.Core.Tests/Collectibles/CrossPlanCollectibleIntegrationTests.cs`:
   - Authored 3 tests verifying zero undocumented gaps, live consumer execution, and deferred row specifications.
9. `Ashfall.Core.Tests/Collectibles/CollectibleTutorialIntegrationTests.cs`:
   - Authored 8 tests verifying tutorial triggers, queue order, save/load, and historical state safety.
10. `Ashfall.Core.Tests/Collectibles/CollectibleMapIntegrationTests.cs`:
    - Authored 11 tests verifying origin tracking, projection, clustering, sanitization, and accessibility.
11. `Ashfall.Core.Tests/Collectibles/CollectibleCampaignSmokeTests.cs`:
    - Authored 6 tests verifying the full 20-scavenge lifecycle, reacquisition probe, unique invariant, trade/weight bounds, 3-run trace equality, and canonical SHA-256 hash equality.
