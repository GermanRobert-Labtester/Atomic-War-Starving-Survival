# Shelter Decor Design — Plan 12C

## Overview

`ShelterDecorSystem` is a small, deterministic Core system that provides bounded room decoration with localized morale effects. It is expressive, not a power-build meta.

## Design Principles

1. **Expressive, not optimal:** Decor provides small localized morale bonuses. No combat bonuses, economy multipliers, or global hidden boosts.
2. **Room-local only:** Only current occupants of a decorated room benefit. Moving immediately changes who receives the effect.
3. **Bounded slots:** 1-3 stable slots per eligible room type. No arbitrary coordinate placement or furniture-grid simulation.
4. **Inventory-safe:** Placing decor reserves/removes the portable item and returns it on removal. No cloning.
5. **Memorial bridge:** Memorial outcomes can yield plaque decor without duplicating death authority.
6. **Core-owned rules:** Core owns slot eligibility, placement validation, effect calculation, save state. Godot owns presentation.

## Domain Model

```
ShelterDecorSystem
├── State: ShelterDecorState
│   ├── Placements: List<DecorPlacement>
│   │   ├── RoomId (string)
│   │   ├── SlotId (string)
│   │   ├── ItemId (string)
│   │   ├── DayInstalled (int)
│   │   ├── IsMemorialPlaque (bool)
│   │   ├── MemorialSurvivorId (string?)
│   │   └── PlaqueSourceHeirloomId (string?)
│   └── ItemModifiers: Dictionary<string, ShelterDecorItemModifier>
│       ├── ItemId (string)
│       ├── LocalizedMoraleDelta (float)
│       └── Category (string)
├── Commands
│   ├── Assign(roomId, slotId, itemId, day, isMemorialPlaque?, survivorId?, heirloomId?) → bool
│   ├── Remove(roomId, slotId) → bool
│   ├── ListRoomPlacements(roomId) → List<DecorPlacement>
│   ├── GetSlot(roomId, slotId) → DecorPlacement?
│   ├── GetRoomMoraleDelta(roomId) → float
│   ├── ResolvePlaqueItemId(heirloomId) → string
│   └── ResolvePlaqueSlot(survivorId, heirloomId, room, slot, day) → DecorPlacement
├── Events
│   └── OnDecorChanged(ShelterDecorState)
└── Persistence
    ├── CaptureState() → ShelterDecorState
    └── RestoreState(ShelterDecorState)
```

## Slot Rules

- Slots are identified by stable string IDs (e.g., `north_wall`, `peg_27`, `main_panel`)
- Categories: wall, shelf, plaque/memorial, trophy
- Validation: unknown room, unknown slot, occupied slot, incompatible category, item not owned, item already installed
- Deterministic ordering: placements sorted by SlotId (ordinal)

## Morale Model

- Per-item `LocalizedMoraleDelta` (float, positive)
- Room total = sum of all placed item deltas
- Hard cap per room (configured, not exceeded)
- No stacking across rooms (each room independent)
- No compounding with severe survival penalties (decor cannot erase hunger/radiation/grief)
- Queried during morale calculation, not tick-by-tick mutation

## Save Contract

- Section: `shelter_decor` → `shelter_decor_save.json`
- Schema version: 1
- Canonical ordering: RoomId, SlotId, ItemId
- Old saves: empty placements (no fabricated decor)
- Deterministic serialization: ordinal sort by room then slot

## Integration Points

| System | Integration | Direction |
|--------|------------|-----------|
| `NeedsSystem` | Room morale delta queried during morale calculation | Decor → Needs |
| `MemorialSystem` | Plaque provenance references memorial records | Memorial → Decor |
| `Inventory` | Item reserved on place, returned on remove | Bidirectional |
| `ShelterAssignmentSystem` | Occupant lookup for morale application | Assignment → Decor |
| `SaveSectionRegistry` | Section registered as `shelter_decor` | Decor → Save |
