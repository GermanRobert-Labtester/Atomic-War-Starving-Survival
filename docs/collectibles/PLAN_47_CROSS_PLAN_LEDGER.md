# PLAN 47 — Cross-Plan Collectible Integration Ledger

**Plan Reference:** Plan 47 §6 (World Culture & Collectibles Integration Matrix)
**Status:** Canonical Executable Integration Ledger
**Authority:** `Ashfall.Core.Tests/Fixtures/collectibles/collectible_cross_plan_integrations.json`
**Test Suite:** `Ashfall.Core.Tests/Collectibles/CrossPlanCollectibleIntegrationTests.cs`

---

## 1. Executive Summary

This ledger records the verified integration status of every producer→consumer relationship defined in Plan 47 §6. Each row represents a formal contract between the collectible subsystem and an existing ASHFALL campaign authority. No relationship is left in an unknown, partial, or silently unwired state.

- **Total Integration Contracts:** 9
- **Wired & Executable:** 8
- **Explicitly Deferred:** 1 (Folklore runtime consumer)
- **Unknown / Undocumented Gaps:** 0

---

## 2. Integration Matrix Status

| Row ID | Producer | Consumer | Status | Runtime Seam | Persistence Owner | Verification Key |
|---|---|---|---|---|---|---|
| `collectible_to_scavenging` | `collectible_catalog` | `scavenging` | **WIRED** | `ScavengingTableCatalog` / `LootSelector` | `ShelterInventory` / `UniqueItemClaimRegistry` | `collectible_scavenging_coverage` |
| `technical_manual_to_research` | `collectible_discovery` | `research` | **WIRED** | `CollectibleEffectDispatcher.ApplyKnowledge` → `ResearchSystem.UnlockManual` | `ResearchSystemState` (`unlocked_manuals`) | `technical_manual_research_unlock` |
| `map_collectible_to_world_map` | `collectible_discovery` | `world_map` | **WIRED** | `CollectibleEffectDispatcher.ApplyLocationClue` → `WastelandMapSystem.Discover` | `WastelandMapState.Discovered` | `map_collectible_reveal` |
| `faction_document_to_faction_intel` | `collectible_discovery` | `faction_intel` | **WIRED** | `CollectibleEffectDispatcher.ApplyJournalUnlock` → `JournalSystem.TryDiscoverKnowledge` | `JournalSaveState` (`discovered_knowledge`) | `faction_document_intel_unlock` |
| `readable_artifact_to_journal` | `collectible_discovery` | `journal_codex` | **WIRED** | `CollectibleEffectDispatcher.ApplyJournalUnlock` → `JournalSystem.TryDiscoverKnowledge` | `JournalSaveState` (`entries` / `discovered_knowledge`) | `readable_artifact_journal_unlock` |
| `vinyl_to_vinyl_system` | `collectible_inventory` | `vinyl_system` | **WIRED** | `Inventory.OnItemAdded` → `VinylMoraleSystem` | `VinylMoraleState` / inventory save | `vinyl_record_morale` |
| `cultural_object_to_folklore` | `collectible_discovery` | `folklore` | **DEFERRED** | None (deferred until runtime consumer is implemented) | None (metadata in `collectibles.json`) | `cultural_object_folklore_deferred` |
| `toy_photo_letter_to_morale` | `collectible_discovery` | `morale` | **WIRED** | `CollectibleEffectDispatcher.ApplyMorale` → `NeedsSystem.Modify(Morale)` | `SurvivorNeedsState` | `toy_photo_morale_boost` |
| `discovery_to_save_state` | `collectible_discovery` | `save_codex` | **WIRED** | `CollectibleDiscoveryState.CaptureState` → `CollectibleDiscoverySaveStore` | `collectible_discovery_save.json` | `discovery_save_persistence` |

---

## 3. Detailed Row Contracts & Verifications

### 3.1 `collectible_to_scavenging` (Wired)
- **Producer:** `CollectibleCatalog` (`Assets/StreamingAssets/Data/collectibles.json`)
- **Consumer:** Scavenging / Loot selection (`Assets/StreamingAssets/Data/scavenging_tables.json`)
- **Contract:** Collectible item definitions provide valid `item_id`, `category`, and rarity tiers for scavenging tables.
- **Verification:** Verified by `CrossPlanCollectibleIntegrationTests` and placement fixtures ensuring all catalog collectible items resolve against the item database and maintain positive weights.

### 3.2 `technical_manual_to_research` (Wired)
- **Producer:** `CollectibleDiscoveryState` via `CollectibleEffectDispatcher`
- **Consumer:** `ResearchSystem` (`Assets/Ashfall.Core/ResearchSystem.cs`)
- **Contract:** Collectibles with `effect_type: "knowledge"` pass `effect_target` to `ResearchSystem.UnlockManual(target)`. Unlocks are idempotent and persisted in research state.
- **Verification:** Validated by unit and E2E tests checking that discovering a manual unlocks the knowledge node and survives round-trip save/load without re-dispatch.

### 3.3 `map_collectible_to_world_map` (Wired)
- **Producer:** `CollectibleDiscoveryState` via `CollectibleEffectDispatcher`
- **Consumer:** `WastelandMapSystem` (`Assets/Ashfall.Core/World/WastelandMapSystem.cs`)
- **Contract:** Collectibles with `effect_type: "location_clue"` reveal the map node specified by `effect_target` using `WastelandMapSystem.Discover(nodeId)`.
- **Architectural Separation:** The *effect reveal target* is strictly decoupled from the *discovery origin location* where the collectible was physically found. Neither leaks into the other.
- **Verification:** Verified by `CollectibleMapIntegrationTests` and `CrossPlanCollectibleIntegrationTests`.

### 3.4 `faction_document_to_faction_intel` (Wired)
- **Producer:** `CollectibleDiscoveryState` via `CollectibleEffectDispatcher`
- **Consumer:** Faction Intel / Codex Authority (`JournalSystem.TryDiscoverKnowledge`)
- **Contract:** Because ASHFALL maintains the journal/codex as the single canonical knowledge authority (no separate isolated faction-intel store exists in the campaign), faction documents route through `JournalSystem.TryDiscoverKnowledge(target)`.
- **Verification:** Verified by `CrossPlanCollectibleIntegrationTests` ensuring discovery unlocks faction records and does not corrupt survivor faction standing.

### 3.5 `readable_artifact_to_journal` (Wired)
- **Producer:** `CollectibleDiscoveryState` via `CollectibleEffectDispatcher`
- **Consumer:** `JournalSystem` (`Assets/Ashfall.Core/Journal/JournalSystem.cs`)
- **Contract:** Readable artifacts (books, letters, logs) with `effect_type: "journal_unlock"` unlock journal entries and codex articles. Content text resides in the catalog, not duplicated in discovery state.
- **Verification:** Verified by `CrossPlanCollectibleIntegrationTests` and `CollectibleCodexUnlockLiveTests`.

### 3.6 `vinyl_to_vinyl_system` (Wired)
- **Producer:** Shelter Inventory (`ShelterInventory`)
- **Consumer:** `VinylMoraleSystem`
- **Contract:** Vinyl records (`category: "vinyl"`) acquired into inventory are accessible for playback and morale bonuses. Unlock is inventory-driven in accordance with survival mechanics.
- **Verification:** Verified by `CrossPlanCollectibleIntegrationTests`.

### 3.7 `cultural_object_to_folklore` (Deferred)
- **Producer:** `CollectibleDiscoveryState`
- **Consumer:** Folklore System (`FolkloreSystem`)
- **Status:** **DEFERRED**
- **Blocking Dependency:** No runtime `FolkloreSystem` or persistent cultural memory API currently exists in `Assets/Ashfall.Core/`.
- **Why wiring is unsafe/impossible now:** Fabricating a dummy runtime folklore class would violate Invariant 5 (no placeholder fake systems) and Invariant 3 (destabilizing save codecs).
- **Temporary runtime behavior:** Cultural objects (`relic`, `prayer_book`, etc.) with `effect_type: "none"` or cultural tags register their discovery normally and project map markers without attempting folklore unlocks.
- **Data preserved for future integration:** All category tags and cultural identifiers remain intact in `collectibles.json`.
- **Resolution condition:** Wire when canonical `FolkloreSystem` is formally introduced into Core with a persistent unlock and query API.
- **Verification that deferment is intentional:** Gated by `CrossPlanIntegration_DeferredRowsDescribeBlockerAndResolution`.

### 3.8 `toy_photo_letter_to_morale` (Wired)
- **Producer:** `CollectibleDiscoveryState` via `CollectibleEffectDispatcher`
- **Consumer:** `NeedsSystem` (`Assets/Ashfall.Core/Survivors/NeedsSystem.cs`)
- **Contract:** Mementos and personal artifacts with `effect_type: "morale"` grant a bounded morale boost (clamped to `MaxMoraleEffectValue = 10f`) to all living survivors in the shelter roster.
- **Verification:** Verified by `CrossPlanCollectibleIntegrationTests` checking that dead survivors are skipped, living survivors receive bounded morale, and reacquisition does not farm morale.

### 3.9 `discovery_to_save_state` (Wired)
- **Producer:** `CollectibleDiscoveryState`
- **Consumer:** `CollectibleDiscoverySaveStore` (`SaveStoreHub.Checksummed`)
- **Contract:** Discovered IDs, unacknowledged IDs, ever-acquired IDs, and `discovery_locations` persist in `collectible_discovery_save.json` using canonical schema-versioned envelopes and checksum verification.
- **Verification:** Verified by `CollectibleDiscoveryPersistenceTests` and `CollectiblePersistenceParityTests`.
