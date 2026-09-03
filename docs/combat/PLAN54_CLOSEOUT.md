# PLAN54_CLOSEOUT — Combat Catalog Expansion

## Summary

Expanded the combat data authority from the **verified 15-weapon /
10-combatant baseline** to the Plan 54 target of **20 weapons and 12 hostile
combatants**, pure-data (zero gameplay-logic changes), with the two additions
chosen to close the plan's largest content gaps and repair three orphaned
calibers.

## Baseline (corrected)

Plan 54's drafted baseline (5 weapons / 0 enemies) was stale. Reconnaissance
proved **15 weapons / 10 combatants** already landed (Plan 10A + expansion
commits). Constraint 1.2 was therefore applied to all existing content; the
delta is +5 weapons / +2 combatants. Details: `PLAN54_BASELINE.md`.

## Combatant authority

**Model A** — `combatants[]` inside `combat_catalog.json`, consumed via
`CombatantFactory` in `BeginEncounter(enemyCombatantIds)`. Full decision
record: `COMBATANT_AUTHORITY_AUDIT.md`.

## Weapon roster (20)

15 baseline (frozen, parity-pinned) + 5 additions:
`weapon_revolver` (civ backup), `weapon_coach_shotgun` (civ close ambush,
12g_buck), `weapon_trail_carbine` (civ long-range, 762x54R),
`weapon_battle_rifle` (mil hard pairs, suppression),
`weapon_quiet_carbine` (mil subsonic precision, suppression).
Matrices: `WEAPON_PARITY_MATRIX.md`, `WEAPON_ROLE_MATRIX.md`.

## Ammo

No new calibers (anti-fragmentation, §55). Orphan calibers reduced 5 → 2:
`762x54r`, `12g_buck`, `556_subsonic` gained consumers; `357_jhp` and
`308_incendiary` remain reload-recipe outputs without a firing weapon —
**deferred** (tradeable items, no broken loop). Matrix:
`AMMO_CALIBER_MATRIX.md`.

## Enemy roster (12) and factions

10 baseline preserved + `combatant_salvage_veteran` (scavenger veteran —
cautious counterpart to the desperate scavenger) and
`combatant_hydro_pump_warden` (Hydro Barons site defense — never resolves,
highest cover in catalog). Both `faction_id`s resolve in
`faction_lore.json` (loader-enforced). Enemy damage stays archetype-owned
(`AiDamageMod`); no weapon/damage duplication. Matrices:
`ENEMY_COMBATANT_MATRIX.md`, `ENEMY_LOADOUT_MATRIX.md`,
`FACTION_EQUIPMENT_MATRIX.md`.

## Supported / unsupported proposed classes (§8 audit)

| Class | Verdict | Disposition |
|---|---|---|
| pistol / revolver / shotgun / rifle / marksman / SMG / burst | supported | used |
| LMG-style heavy (burst + suppression) | supported | already present (`weapon_lmg`) |
| melee (first-class flag) | unsupported | baseline's range-band idiom (rebar_spear) stands; no fake melee |
| throwable / incendiary AoE | unsupported (as AoE) | molotov idiom stands as data; nothing faked |
| explosive launcher / RPG | unsupported | **replaced** by battle rifle (heavy identity via burst+suppression tradeoffs) |
| indirect fire / mortar | unsupported | **replaced** by trail carbine's long-range civilian niche |
| automated turret | unsupported (`kind` = human/mutant/fauna; no machine AI) | **replaced** by DoD #42 equivalent: hydro pump warden |

## Patrol / raid / wildlife / excavation bindings

Six patrol + four raid roles mapped to stable `combatant_*` ids; live
encounter data not rewritten (no src caller passes `enemyCombatantIds` yet —
Plan 45 seam is prepared, per §64). Wildlife stays canonically ecological
(`field_guide.json` cross-refs); turret slot fulfilled by the warden.
`PATROL_RAID_BINDINGS.md`.

## Determinism

Fixed-seed proof: a `weapon_quiet_carbine` encounter with both new
combatants replays **event-by-event identical** (kind, target, value).
All rolls remain on the injected `ISeededRng`; no RNG ordering changed for
existing content (catalog additions are append-only; lookups are keyed, not
ordinal).

## Balance findings (analytic + deterministic runtime checks)

- Additions span distinct roles; no stat clones (test-gated).
- No low-tier enemy accidentally lethal; high-tier dangerous but beatable;
  combat economy capped by the fixed runtime loot grant (no farming loop).
- Tier spread: 7 improvised / 5 civilian / 8 military (lmg = heavy slot) —
  improvised over-weight is pre-existing baseline and uncorrectable under
  constraint 1.2.

## Save

`schema_version` stays 2; no save schema change; round-trip with new content
pinned by test. `PLAN54_SAVE_CONTRACT.md`.

## Regression (exact)

| Gate | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **6513/6513 PASS** (15 new) |
| `dotnet build Ashfall.csproj` | 0 errors / 0 warnings |
| `godot --headless --path . -- --data-integrity-selftest` | PASS, 208 catalogs, 0 findings |
| `godot --headless --path . -- --combat-selftest` | PASS, 26/26 |
| `godot --headless --path . -- --bridge-selftest` | PASS (exit 0) |

## Deferred / follow-on

1. Consumer weapon for `ammo_357_jhp` + `ammo_308_incendiary` (reload
   recipes currently output tradeable items only).
2. `weapon_rust_mosin` caliber lore mismatch (`ammo_762` vs 54R item text) —
   untouched under constraint 1.2.
3. Live `enemyCombatantIds` wiring for encounters (Plan 45) using the
   binding matrix.
4. Per-archetype loot profiles behind `GrantVictoryLoot`'s port (§79.6) —
   single-authority extension seam documented in
   `COMBAT_LOOT_REFERENCE_MATRIX.md`.
5. Weapon attachments/mods and faction doctrine weapon tables (§79.5,
   §79.7) on top of the now-stable 20-weapon catalog.
