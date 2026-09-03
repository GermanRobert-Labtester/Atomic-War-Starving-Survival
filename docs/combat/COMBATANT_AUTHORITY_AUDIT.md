# Combatant Authority Audit (Plan 54 §25 decision record)

## Decision: Model A — the combat catalog is the combatant authority

`combat_catalog.json → combatants[]` **is** the hostile-combatant authority.
No fifth authority was created; no enemy schema was invented elsewhere.

Evidence chain (all verified by reading source, not file names):

1. `Assets/Ashfall.Core/Combat/CombatCatalog.cs` — `CombatantDefinition` DTO,
   `CombatCatalog.Register(Get/Has)Combatant`, `CombatantFactory`
   (`SpawnFromCatalog` / `SpawnFromCatalogOrThrow` / `TrySpawnFromCatalog`).
2. `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs:186-210` —
   `BeginEncounter` resolves `enemyCombatantIds[i]` via `CombatantFactory`;
   unknown ids emit `enemy_catalog_missing` and fall back to the legacy
   hand-coded "Raider" block.
3. Loader validation: `combatant_*` prefix, lane 0..2, `TacticalStance`
   stance names, `CombatAiMoves` move whitelist, 0..2 AI mods, thresholds
   −1 or 0..1, `faction_id` cross-referenced against `faction_lore.json`.
4. `field_guide.json` already references 6 of the fauna/mutant combatant ids
   (bestiary display layer — stays in sync, no combat data there).

## Answers to the §25 checklist

| # | Question | Answer |
|---|---|---|
| 1 | Where do hostile definitions live? | `combat_catalog.json → combatants[]` |
| 2 | Stable ids? | Yes — `combatant_*`, loader-enforced |
| 3 | HP/armor definition fields? | `base_health`, `base_armor_rating`, `base_cover_rating` |
| 4 | Weapon referenced by id? | **No** — enemies have no weapon field; damage = `(6 + lane-match 4) × AiDamageMod` |
| 5 | Behavior enum/data? | `ai_stance_preference` (`TacticalStance`) + `ai_special_move` (`CombatAiMoves`) — both loader-validated |
| 6 | Faction referenced? | `faction_id` → `faction_lore.json` (loader-enforced) |
| 7 | Loot inline or separate? | **Neither** — victory loot is runtime-fixed in `GrantVictoryLoot`; no per-enemy loot schema exists |
| 8 | Animals same schema? | Yes — `kind: "fauna"` / `"mutant"` rows with special moves (`Charge`, `Flank`, `Spore`, `Burrow`) |
| 9 | Turrets? | No machine/immobile support (`kind` vocabulary is `human|mutant|fauna`; no turret AI). Fulfilled by the DoD #42 equivalent: `combatant_hydro_pump_warden` |
| 10 | Static vs encounter-specific | Static: health/armor/cover/lane/AI traits/thresholds. Encounter-specific: runtime `Id` (`enemy_<encounter>_<i>`), current health, pinned/downed state, weapon assignment |

## Id namespace (Plan 54 §1.6)

Accepted prefix: **`combatant_*`** — confirmed by the loader's
`ValidateRegistered` and by `Plan10CatalogCoverageTests`. The `enemy_*`
namespace exists only as a **runtime** instance-id prefix
(`enemy_<encounterId>_<index>`) assigned by `BeginEncounter`, never as a
catalog id prefix.
