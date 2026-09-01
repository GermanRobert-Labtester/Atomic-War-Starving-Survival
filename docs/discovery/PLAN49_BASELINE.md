# Plan 49 — Baseline Reconnaissance

## Integration Path: B — Category Registration

The `NarrativeEncounterSystem` already supports weighted encounter selection during expedition travel via `ExpeditionEncounterBridge`. Micro-locations are registered as `EncounterDefinition` entries with `category: "Discovery"` (or "Hazard", "Social").

## Expedition System

- **Tick model**: `TickHours(float hours, ISeededRng rng)` — one tick per game hour
- **Encounter roll**: `RollEncounter()` fires every tick on every leg (Outbound, Looting, Inbound, Camp)
- **Bridge**: `ExpeditionEncounterBridge` → `NarrativeEncounterSystem.SelectEncounter(stance, dangerLevel, locationId, rng)`
- **RNG**: Single `ISeededRng` stream from `TickHours` through encounter selection — fully deterministic
- **No depletion**: `NarrativeEncounterSystem` has no cooldown or depletion mechanism
- **No route tags**: Expeditions have no `route_tag`, `biome`, or `region_tag` fields

## Encounter Schema (Extended)

`EncounterChoiceDefinition` extended with:
- `grantItemId` — item to grant on resolution
- `grantItemQuantity` — quantity of granted item
- `setWorldFlag` — world flag to set
- `journalUnlockId` — journal/codex knowledge key to unlock
- `discoverLocationId` — location to discover via radio triangulation
- `depletesOnResolve` — whether this choice depletes the micro-location

## Event System

- **No narrative event runtime in Ashfall.Core/** — `SeasonalEventSystem` is weather-only
- **Event effects are schema-driven** — JSON objects with ad-hoc keys
- **Event chains** use `scheduleEventId` + `scheduleOnDay` + flag conditions

## Loot System

- `ScavengingTableCatalog` — weighted tables with `item_id`, `weight`, `min_quantity`, `max_quantity`
- `codex_unlock_id` on loot entries for journal unlocks
- `ExpeditionSystem.PerformLootRoll()` bridges to `ScavengingTableCatalog.RollLoot()`

## Location Discovery

- `SignalTriangulationSystem.IsLocationDiscovered(locationId)` — checks `discoveredLocationIds`
- Locations discovered through radio signal triangulation
- Flag-gated access via `requiredFlagId` in `locations.json`

## Journal/Codex

- `JournalSystem.TryDiscover(knowledgeKey, author, day, hour)` — deduped via `KnowledgeBase`
- `OnCodexUnlocked` event fires on first discovery
- Max 64 entries with eviction + recycling

## Content History/Depletion

- **No expedition-level depletion** — `DutyRosterOverflowEngine` has visited ledger for overflow nodes only
- **`TravelEncounterSystem`** has 5-day cooldown per encounter
- **`NarrativeEncounterSystem`** (used by expedition bridge) has NO cooldown or depletion
- Depletion for micro-locations tracked via `NarrativeEncounterState.history` (resolution records)
