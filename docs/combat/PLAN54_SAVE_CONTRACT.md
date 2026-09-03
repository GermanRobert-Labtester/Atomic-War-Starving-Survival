# Plan 54 Save Contract

## Rule: static definitions stay outside saves

Weapon and combatant **definitions** are data-authority content and are
never serialized into combat saves. `CombatState` (save version 3) stores
only stable id references and runtime state:

- `WeaponInstanceState.WeaponId` → `weapon_*` id (resolves through
  `CombatCatalog.GetWeapon` at load; `SeedWeaponAmmo` re-seeds caliber/ammo
  when the token lacks them).
- `CombatantState.CatalogId` → `combatant_*` id (catalog-derived AI traits
  re-project through `CombatantFactory` on demand; runtime state — current
  health, pinned, downed — is serialized directly).

## Compatibility results

- Old saves referencing any of the 15 baseline weapons resolve unchanged —
  no id was renamed, no stat changed (pinned by
  `Plan54CombatCatalogTests.Catalog_PreservesAll15BaselineWeaponsWithCalibers`).
- Plan 54 weapons/enemies introduce **no schema change**: catalog
  `schema_version` remains 2 (`CombatCatalogLoader.CurrentSchemaVersion = 2`).
- Round-trip proof with new content:
  `Plan54CombatCatalogTests.SaveRoundTrip_Plan54WeaponAndEnemiesSuriveReload`
  — captures a mid-combat state carrying a `weapon_trail_carbine` instance
  and two Plan 54 combatants, JSON round-trips it, and asserts weapon
  id/ammo, combatant count, and catalog-derived AI mods survive.
- No orphaned combatants after reload: `CatalogId` is persisted on the wire
  and re-resolvable because the static catalog always loads from JSON.
