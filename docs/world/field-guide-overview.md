# Plan 20 — Field Guide: Wasteland Flora & Fauna

> **Data authority:** `Assets/StreamingAssets/Data/field_guide.json`
> **Core system:** `Assets/Ashfall.Core/World/FieldGuideCatalog.cs`
> **Schema version:** 1

## Purpose

The Field Guide is a discoverable knowledge layer that teaches the player mechanically true facts about the wasteland's creatures and plants. Every entry agrees with the live trap, combat, greenhouse, and harvest systems.

Entries unlock when the player encounters a creature or plant in context — through a travel encounter choice, a trap, a combat resolution, or an expedition observation. Unlocked entries are readable from the Codex panel (Field Guide tab).

## Catalog Summary

**32 total entries — 20 fauna, 12 flora/fungus**

All fauna IDs use `field_fauna_` prefix. All flora/fungus IDs use `field_flora_` prefix.

Key fauna: `field_fauna_two_headed_wolf`, `field_fauna_slag_beetle`, `field_fauna_timber_tick`, `field_fauna_bristleback_boar`, `field_fauna_marsh_adder`, `field_fauna_rustbeak_harpy`, `field_fauna_mud_lurker`, `field_fauna_cinder_fox`, `field_fauna_bleach_moth`, `field_fauna_ash_vulture`, `field_fauna_brine_crab`, `field_fauna_rust_rat`, `field_fauna_ember_hawk`, `field_fauna_ironback_tortoise`, `field_fauna_salt_leech`, `field_fauna_decay_crow`, `field_fauna_cave_salamander`, `field_fauna_ashfall_spider`, `field_fauna_feral_hog_mutant`, `field_fauna_ghost_moth`

Key flora: `field_flora_ashbloom`, `field_flora_rad_moss`, `field_flora_blight_fungus`, `field_flora_iron_nettle`, `field_flora_pale_tuber`, `field_flora_dust_lichen`, `field_flora_wire_grass`, `field_flora_glow_caps`, `field_flora_salt_cactus`, `field_flora_black_bloom`, `field_flora_albino_kale`, `field_flora_root_lace`

## Engine Model

```csharp
// Load
catalog.Load(dataDir, fileIO);
// Unlock
catalog.Unlock(survivorId, "field_fauna_two_headed_wolf");
// Query
catalog.GetByCategory("fauna");
catalog.GetByTag("edible");
catalog.IsUnlocked("field_fauna_two_headed_wolf");
// Save state
var state = catalog.CaptureState();
catalog.RestoreState(state);
```

## Unlock Triggers

| Source | Mechanism |
|--------|-----------|
| Travel encounter choices | `unlocks_field_guide_id` on the choice DTO |
| Expedition observation events | world flag `field_guide_unlocked_<id>` |
| Trap harvest | Trap type mapped to field guide entry at resolution |

## Design Rules

1. Every entry must contain mechanically true information — trap preferences, edibility, and combat behavior must agree with the live data.
2. Flavor language is written in the register of practical field notes.
3. No entry may reference a real-world species name.
4. New entries require a corresponding travel encounter or trap trigger before shipping.
