# Plan 142 — Clothing & Warmth Gear Progression

## Goal

Add warmth bonuses from equipped clothing so that players can protect survivors from nuclear winter cold through gear choices. Currently `NeedsSystem.ApplyWarmth()` only checks a shelter-level boolean (`_isNearHeatSource`) — equipped clothing provides zero cold protection. This plan connects the equipment system to the warmth need, creating a meaningful gear progression for cold survival.

## Why

**Repository evidence:** `EquipmentConditionSystem.cs` (189 lines) tracks equipment wear. `Inventory.Equip()` (line 853) handles equipping items. `Inventory.GetTotalRadProtection()` (line 920) sums radiation protection from gear. But `NeedsSystem.ApplyWarmth()` has **zero references to equipped clothing** — it only checks `_isNearHeatSource(survivor)`. The `Warmth` need decays at a flat rate regardless of what the survivor wears. The gameplay gaps agent confirmed: "No clothing warmth bonus."

**What is missing:** Players cannot protect survivors from cold through clothing. A survivor in the freezing wasteland wearing no gear loses warmth at the same rate as one wearing a full cold-weather outfit. The equipment system exists but has no warmth dimension. Nuclear winter — a core ASHFALL theme — has no gear-based countermeasure.

**Why existing plans don't solve it:** Plan 137 (needs→performance cascade) connects needs to performance but doesn't fix the warmth input side. Plan 135 (weather cascade) makes weather affect shelter/needs but doesn't add clothing mitigation. Plan 10 (combat/expedition depth) adds vehicles but not clothing. No plan addresses clothing warmth bonuses.

**Player value:** Creates gear progression for cold survival, makes equipment choices meaningful beyond combat/radiation, adds scavenging motivation (find warm clothes), and makes nuclear winter a manageable threat rather than an unavoidable death sentence.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` — warmth decay (no clothing check)
- `Assets/Ashfall.Core/Inventory/Inventory.cs` — equipment system
- `Assets/Ashfall.Core/EquipmentConditionSystem.cs` — equipment wear
- `Assets/StreamingAssets/Data/items.json` — clothing items
- NEW: `Assets/Ashfall.Core/Inventory/ClothingWarmthSystem.cs`

## Main Task 1 — Foundation / System Contract

1. Create `ClothingWarmthSystem.cs` in `Assets/Ashfall.Core/Inventory/`
2. Define `ClothingWarmthProfile` DTO: `itemId`, `warmthBonus` (0-100), `coldResistance` (0-1.0 fraction), `wetPenalty` (0-1.0 fraction), `durability` (0-100)
3. Define `ClothingWarmthState` DTO: map of survivor → equipped clothing warmth, wetness state
4. Implement `CaptureState/RestoreState` with schema versioning
5. Define warmth calculation:
   - Base warmth decay from `NeedsSystem` (environmental)
   - Clothing warmth bonus reduces decay rate
   - Cold resistance fraction reduces effective temperature penalty
   - Wetness increases decay rate (rain/sweat)
   - Clothing durability affects warmth bonus (damaged clothes provide less warmth)
6. Create warmth tiers:
   - Tier 0 (no clothing): full decay
   - Tier 1 (basic): -10% decay
   - Tier 2 (warm): -30% decay
   - Tier 3 (insulated): -50% decay
   - Tier 4 (arctic): -70% decay
7. Implement `GetTotalWarmthBonus(string survivorId)` method that reads equipped clothing
8. Wire into `NeedsSystem.ApplyWarmth()`: replace shelter-only check with shelter + clothing
9. Add deterministic calculation: warmth is pure function of equipment state (no RNG)
10. Wire into `GameBootstrap`: `SetupClothingWarmth`, `SaveClothingWarmth`
11. Create `ClothingWarmthCatalogLoader` for clothing warmth profiles
12. Implement wetness mechanic: rain/sweat reduces clothing effectiveness
13. Add clothing durability: warmth gear degrades with use, needs repair/replacement
14. Create UI hook: survivor panel shows warmth bonus from clothing

## Main Task 2 — Implementation / Clothing Items / Warmth Mechanics

1. Define 20 clothing items with warmth bonuses:
   - Basic: ragged coat (+5), wool scarf (+3), leather gloves (+2)
   - Warm: winter coat (+15), fur-lined boots (+8), thermal underwear (+10)
   - Insulated: hazmat cold-weather suit (+25), heated vest (+20), insulated gloves (+12)
   - Arctic: full arctic survival suit (+40), heated boots (+15), thermal balaclava (+10)
   - Special: radiation-cold combo suit (+30 warmth, +20 rad protection)
2. Implement clothing layering:
   - Inner layer: thermal underwear (base warmth)
   - Middle layer: insulated vest/jacket (bonus warmth)
   - Outer layer: coat/suit (cold resistance)
   - Accessories: gloves, boots, headwear (stacking bonuses)
   - Maximum 4 layers; too many layers causes overheating (fatigue penalty)
3. Create clothing condition system:
   - Clothing degrades with use (durability decreases)
   - Damaged clothing provides reduced warmth
   - Clothing can be repaired (requires sewing kit, scrap)
   - Clothing can be upgraded (requires research + materials)
4. Implement wetness mechanic:
   - Rain/snow wets clothing (reduces warmth bonus by 50%)
   - Sweating during work/exercise wets inner layers
   - Wet clothing must be dried (near heat source)
   - Prolonged wetness causes hypothermia risk
5. Create clothing crafting:
   - Basic clothing from scavenged materials (rags, leather)
   - Warm clothing from animal hides (trapping integration)
   - Insulated clothing from synthetic materials (research required)
   - Arctic clothing from advanced materials (rare components)
6. Implement clothing trade:
   - Factions trade warm clothing (standing required)
   - Traders sell cold-weather gear (economic cost)
   - Clothing can be scavenged from expeditions
7. Create clothing events:
   - "Frozen Fingers" — survivor without gloves loses dexterity (crafting penalty)
   - "Hypothermia Warning" — survivor warmth critical, immediate action required
   - "Overheated" — too many layers, fatigue penalty
   - "Soaked to the Bone" — wet clothing in freezing weather, severe penalty
8. Add clothing quest hooks:
   - "The Tailor" — survivor with sewing skill crafts warm clothing
   - "Winter Preparation" — equip all survivors before nuclear winter arrives
   - "The Arctic Expedition" — special gear required for frozen zone
   - "Clothing Drive" — trade warm clothes to refugees for standing
9. Implement clothing interaction with other systems:
   - `NeedsSystem`: warmth decay modified by clothing
   - `ExpeditionSystem`: cold zones require specific gear
   - `CombatSystem`: heavy clothing reduces mobility (armor trade-off)
   - `WorkSystem`: some clothing improves work efficiency (gloves for crafting)
10. Add UI: clothing panel showing equipped items and warmth bonuses
11. Create clothing journal: automatic log of clothing acquisition and upgrades
12. Implement clothing tutorial: first cold exposure explains clothing system
13. Add clothing tooltips: hover over item shows warmth bonus and durability
14. Create 20 clothing warmth profiles in data file

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `NeedsSystem.ApplyWarmth()`: clothing bonus reduces warmth decay
2. Connect to `Inventory.Equip()`: equipped clothing registered for warmth calculation
3. Integrate with `EquipmentConditionSystem`: clothing durability tracked
4. Connect to `ExpeditionSystem`: cold zones check clothing requirements
5. Wire into `WildlifeTrappingSystem`: animal hides used for warm clothing
6. Connect to `ResearchSystem`: advanced clothing requires research
7. Implement old-save compatibility: existing saves get empty clothing warmth state
8. Add deterministic calculation: warmth is pure function of equipment state
9. Create exploit prevention: clothing has durability, can't be permanent
10. Add tests: warmth calculation, clothing equipping, durability degradation, save round-trip
11. Verify catalog integrity: all clothing item IDs resolve
12. Test edge cases: no clothing (full decay), full arctic gear (70% reduction)
13. Verify headless behavior: warmth calculates correctly without UI
14. Add data-integrity-selftest: clothing warmth profiles validate against item catalog
15. Create `--clothing-warmth-selftest` verb for CI validation

## State / System Interaction Model

```text
Survivor equipped with clothing
├─ Calculate total warmth bonus
│  ├─ Inner layer: thermal underwear (+10)
│  ├─ Middle layer: insulated vest (+20)
│  ├─ Outer layer: arctic suit (+40)
│  ├─ Accessories: gloves (+12), boots (+15), balaclava (+10)
│  └─ Total: +107 (capped at tier 4: -70% decay)
├─ Apply cold resistance
│  ├─ Arctic suit: 60% cold resistance
│  └─ Effective temperature penalty reduced
├─ Apply wetness modifier
│  ├─ Rain/snow: -50% warmth bonus
│  ├─ Sweating: -25% warmth bonus
│  └─ Dried: back to full bonus
├─ Apply durability modifier
│  ├─ 100% durability: full bonus
│  ├─ 50% durability: 50% bonus
│  └─ 0% durability: clothing destroyed
└─ NeedsSystem.ApplyWarmth() uses modified decay rate
   ├─ Base decay: -2/day (nuclear winter)
   ├─ Clothing bonus: -70% decay
   └─ Effective decay: -0.6/day
```

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --clothing-warmth-selftest
```

## Risk

**LOW** — Clothing warmth is a straightforward stat modifier with clear inputs (equipment) and outputs (warmth decay reduction). Risk of balance issues (clothing makes cold trivial) mitigated by durability degradation, wetness penalties, and layering limits.

## Definition of Done

- `ClothingWarmthSystem.cs` exists with full `CaptureState/RestoreState`
- 20 clothing items with warmth bonuses defined
- Clothing layering system functional (4 layers max)
- Warmth calculation integrates with `NeedsSystem.ApplyWarmth()`
- Clothing durability and wetness mechanics working
- Clothing crafting and trade options
- Save/load round-trip tested
- Deterministic warmth calculation verified
- Old saves load without error
- 20 clothing warmth profiles in data authority
- UI panel shows clothing and warmth bonuses
- Cross-system integration (needs, inventory, equipment, expedition, trapping, research)

## Follow-On Opportunities

- Clothing fashion (morale bonus from stylish outfits)
- Clothing customization (dyes, patches, faction emblems)
- Clothing trade specialization (survivors become tailors)
- Clothing legacy (heirloom garments with history)
- Clothing hazards (contaminated clothing spreads radiation)
