# Combat Catalog Schema (verified against `CombatCatalog.cs` / `CombatTypes.cs`)

Source of truth: `Assets/Ashfall.Core/Combat/CombatCatalog.cs`
(`CombatCatalogLoader`, `CurrentSchemaVersion = 2`),
`Assets/Ashfall.Core/Combat/CombatTypes.cs`.

## File: `Assets/StreamingAssets/Data/combat_catalog.json`

Top level: `schema_version` (2), `collection_id`, `weapons[]`, `ammo[]`,
`materials[]`, `combatants[]`. All fields snake_case in JSON, mapped to
camelCase runtime DTOs by the loader. A future schema throws
(`InvalidDataException`); canonical-prefix and cross-reference violations
throw (`FormatException`) — a malformed catalog can never load silently.

## weapons[] — `CombatWeaponDefinition`

| JSON field | Runtime field | Semantics |
|---|---|---|
| `id` | `id` | must be `weapon_*` |
| `display_name` | `displayName` | |
| `accuracy` | `accuracy` | base hit chance 0..1 |
| `damage` | `damage` | per round, before ammo/stance mods |
| `range` | `range` | range modifier (1 = standard) |
| `caliber` | `caliber` | must resolve to an `ammo[]` id |
| `burst` | `burst` | rounds per trigger pull |
| `is_jury_rigged` | `isJuryRigged` | +0.03 jam, +0.08 jam with military-tier ammo, 50% burst-failure roll |
| `is_suppression_capable` | `isSuppressionCapable` | gates `PlayerSuppress` |
| `degrade_per_shot` | `degradePerShot` | condition loss × burst per pull |
| `jam_base` | `jamBase` | pristine-condition jam chance |
| `scrap_repair_cost` | `scrapRepairCost` | field repair to full, `scrap_metal` |
| `condition_threshold` | `conditionThreshold` | below this, jam risk rises steeply (+1.2 × shortfall) |

## ammo[] — `CombatAmmoDefinition`

`id` (`ammo_*`), `display_name`, `damage_mod`, `range_mod`,
`is_military_tier`. Military-tier ammo fired by a jury-rigged weapon adds
jam risk and the 50% burst-failure roll (`WeaponConditionSystem`).

## materials[] — `CombatMaterialDefinition`

`id` (`material_*` or `armor_*`), `kind` (`cover|armor|barrier`),
`armor_reduction`, `ricochet_chance`, `ricochet_energy_retained`.

## combatants[] — `CombatantDefinition`

| JSON field | Validation |
|---|---|
| `id` | must be `combatant_*` |
| `kind` | documented vocabulary `human\|mutant\|fauna` (display metadata; not read by the AI loop) |
| `faction_id` | must resolve in `faction_lore.json` (loader-enforced); blank = unaligned |
| `base_health` | > 0 |
| `base_armor_rating` / `base_cover_rating` | 0..1 (clamped at spawn) |
| `preferred_lane` | 0..2 (`CombatLane` Left/Center/Right) |
| `ai_stance_preference` | must be a `TacticalStance` name: `HoldPosition\|Advance\|SuppressiveFire\|Retreat\|LastStand` |
| `ai_special_move` | must be in `CombatAiMoves`: `None\|Burrow\|Flank\|Spore\|Charge\|SuppressiveFire\|TacticalRetreat` |
| `ai_accuracy_mod` / `ai_damage_mod` | 0..2 (clamped at spawn) |
| `surrender_threshold` / `flee_threshold` | -1 (never) or 0..1 |
| `journal_key` | optional narrative hook |

## Runtime consumption contract

1. **Spawning**: `TacticalCombatSystem.BeginEncounter(..., enemyCombatantIds)`
   resolves the i-th catalog id through `CombatantFactory.TrySpawnFromCatalog`.
   Unknown ids emit an `enemy_catalog_missing` event and fall back to the
   legacy hand-coded enemy block. `enemyHealth > 0` overrides catalog
   `base_health`; `0` honors it.
2. **Enemy damage does NOT use weapon stats** (Plan 54 constraint 1.14):
   `EndTurn` fires enemies at `accuracy = 0.50 × AiAccuracyMod × (1 − stance
   defense)` and `damage = (6 + lane-match bonus 4) × AiDamageMod`.
3. **Loot** is runtime-owned (`GrantVictoryLoot`: fixed `scrap_metal` ×3 +
   `ammo_556` ×6). There is **no per-combatant loot schema** — none was
   invented in Plan 54 (constraint 1.17).
4. **Weapon inventory binding**: weapon definitions function in combat
   without `items.json` backing (the host passes `WeaponId` literals into
   `WeaponInstanceState`). Item entries exist for trade/craft/loot economy;
   10 of 15 baseline weapons and all 5 Plan 54 weapons have them.

## Deliberately unsupported content classes (Plan 54 §8 audit)

Melee (as a first-class flag), thrown/incendiary AoE, explosive launchers,
indirect fire: **no schema or runtime support**. The baseline approximates
short-range identities through range values (`weapon_rebar_spear` 0.6,
`weapon_molotov_thrower` 0.85). Plan 54 follows that idiom and does not fake
new mechanics in JSON.
