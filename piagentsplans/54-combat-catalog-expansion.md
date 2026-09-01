# Plan 54 — Combat Catalog Expansion (5 weapons → 20, 0 enemies → 12)

## Goal (2 lines)
Expand `combat_catalog.json` from 5 weapons and zero enemies to 20 weapons and 12 enemies.
The `TacticalCombatSystem` is fully implemented (actions, damage, targeting, persistence,
headless demo) but has almost nothing to fight with or fight against.

## Why (P2)
- Verified: `combat_catalog.json` has 5 weapons (`weapon_pipe_rifle` and 4 others), 0
  enemy definitions. `TacticalCombatSystem.cs` + 5 partial files are fully implemented with
  save persistence. The combat system is the most underfed live system in the project.
- Without weapons and enemies, the combat loop (aim → fire → damage → jam → degrade →
  loot) has no content. Plan 45 patrol encounters need enemies; this plan is the sole
  combat-catalog expansion owner.
- Pure DATA work — zero new Core code.

## Files to touch
- `Assets/StreamingAssets/Data/combat_catalog.json` (expand 5 → 20 weapons, add 12 enemies)
- Read-only: `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`, `CombatCatalog.cs`,
  `CombatTypes.cs` (confirm weapon schema: id, display_name, accuracy, damage, range,
  caliber, burst, is_jury_rigged, is_suppression_capable, degrade_per_shot, jam_base,
  scrap_repair_cost, condition_threshold; confirm enemy schema — if none exists, model
  enemies on the weapon entry pattern with HP, armor, damage, behavior fields)
- `Assets/StreamingAssets/Data/items.json` (weapon ids must resolve as items; ammo calibers
  must resolve)

## Content grammar (per weapon)
- snake_case `id` with prefix `weapon_` (confirmed prefix).
- Grounded tier system: improvised (pipe weapons, scrap guns) / civilian (hunting rifles,
  pistols, shotguns) / military (assault rifles, SMGs, sniper rifles) / heavy (LMG,
  RPG, mortar).
- Each weapon: distinct accuracy/damage/range/jam/degrade profile — no two weapons
  should feel identical.
- Caliber must reference an existing ammo `item_*` id (confirm ammo items exist in
  `items.json`; add missing calibers in the same commit).

## Content grammar (per enemy)
- snake_case `id` with prefix `enemy_` (confirm accepted prefix — do not invent; if
  `enemy_` is not in the validator, use `unit_` or `npc_` per existing conventions).
- enemy_type: scavenger / raider / faction_soldier / feral_dog / wolf_pack / mutant_fauna /
  deserter / press_gang / border_patrol / automated_turret.
- stats: HP, armor value, weapon id (from combat_catalog), accuracy, damage, behavior
  (aggressive / defensive / ranged / melee / flee_when_injured).
- faction_link: optional `faction_*` id — faction soldiers use faction equipment.
- loot: `item_*` ids dropped on defeat (weapon, ammo, supplies).

## Steps
1. Read `CombatCatalog.cs` + `CombatTypes.cs` to confirm the weapon and enemy schema; if
   no enemy schema exists, check how `TacticalCombatSystem` resolves combatants (does it
   load enemies from the catalog, or are they spawned by encounters?).
2. Read `TacticalCombatSystem.cs` to confirm how weapons and enemies are resolved in
   combat (damage calculation, jam rolls, degradation, loot drops).
3. Read `items.json` to confirm which ammo items exist; add missing calibers (e.g.
  `item_ammo_9mm`, `item_ammo_556`, `item_ammo_12gauge`, `item_ammo_762`).
4. Author 15 new weapons across 4 tiers:
   - Improvised: pipe pistol, scrap shotgun, nail gun, molotov.
   - Civilian: hunting rifle, revolver, pump shotgun, hunting knife.
   - Military: assault rifle, SMG, sniper rifle, sidearm pistol.
   - Heavy: LMG, RPG (rare), mortar (very rare, faction-only).
5. Author 12 enemies across 6 types:
   - 3 scavengers (lone, pack, armed — low tier, improvised weapons).
   - 2 raiders (organized, military-grade loot — feeds existing 14).
   - 2 faction soldiers (garrison, rebel — linked to `factions.json`).
   - 2 feral animals (dog pack, wolf pack — linked to Plan 35 wildlife).
   - 2 mutant fauna (irradiated boar, contaminated predator — canon per AGENTS.md).
   - 1 automated turret (military site defense — feeds Plan 37 excavation hazards).
6. Cross-reference: every weapon `id` resolves in `items.json`; every ammo caliber
   resolves; every enemy `weapon_id` resolves in the combat catalog; every `faction_*`
   link resolves; every loot `item_*` id exists.
7. Wire 6 enemies into Plan 45 patrol encounters (faction soldiers, raiders, border
   patrols use these enemy definitions).
8. Wire 4 enemies into existing 14 raid encounters (raiders, scavengers, press gangs).
9. Validate: `--data-integrity-selftest`; run `--combat-selftest` or headless combat demo
   to confirm weapons fire and enemies take damage.
10. xUnit: combat catalog loads, all weapon ids resolve, all enemy ids resolve, damage
    calculation works with new weapons, jam rolls are deterministic (seeded), degradation
    applies, loot drops resolve, save round-trip preserves combat state.

## Verification
```bash
godot --headless --path . -- --data-integrity-selftest
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
```

## Risk
LOW — pure data. The one trap is the enemy schema (step 1): if no enemy schema exists in
the catalog, confirm how `TacticalCombatSystem` spawns combatants before authoring.

## Definition of Done
- `combat_catalog.json` has 20 weapons + 12 enemies, all ids resolving, 6 wired into
  patrol encounters, 4 wired into raids, combat demo runs with new content, save
  round-trip green, integrity + tests green.

## Follow-on
- Plan 45 (patrol encounters) — enemies populate patrol combat.
- Plan 10 (combat resolution/readiness) consumes this catalog without expanding it.
- Plan 37 (excavation) — automated turrets as site defenses.
- Plan 35 (wildlife) — feral animals and mutant fauna link to migration data.
