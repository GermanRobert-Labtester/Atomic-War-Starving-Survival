# Plan 10 — Combat & Expedition Depth: Bestiary, Armory & the Fleet

> **Theme:** Both tactical combat (5 lanes, 7 stances, ballistics) and expeditions (vehicles,
> dives, waystations, caravans) are fully built and badly under-filled: **5 weapons, 5 ammo,
> 4 warlord doctrines, 3 vehicles, 4 dive sites**. This is the action-content layer.
>
> **Key evidence:** `combat_catalog.json` = 5 weapons / 5 ammo / 7 materials;
> `warlord_doctrines.json` = 4 doctrines; `vehicles.json` = 3; `dive_sites.json` = 4.
> `TacticalCombatSystem.cs` (1351 lines), `BallisticsSystem.cs`, `ExpeditionVehicleSystem.cs`,
> `District8DeepCoastSystem.cs` (667 lines) all live.

---

## Task 10A — Combat bestiary & warlord roster (4 → 8 doctrines)

**Goal:** Fill tactical combat with distinct, behavior-defined enemies and double the warlord
doctrine roster so encounters stop repeating.

**Files:** `combat_catalog.json`, `warlord_doctrines.json`, `warlord response` data,
`faction_lore.json` (warlord entries), read-only `WarlordDoctrineSystem.cs`,
`TacticalCombatSystem.cs`, `CombatCatalog.cs`.

**Substeps:**
1. Read `combat_catalog.json` + `CombatCatalog.cs` to extract the enemy/behavior schema (lanes used, stance preferences, special moves).
2. Read `WarlordDoctrineSystem.cs` + the 4 existing doctrines to learn doctrine fields (territory, response actions, alias warnings).
3. Author 6 mutant/fauna combatants with distinct lane behavior (burrower that flanks, spore-hound that debuffs, armored boar that holds center) — grounded mutations, no fantasy.
4. Author 4 human combatant archetypes (conscript levy, warlord veteran, flotilla marine, desperate scavenger) with distinct surrender/bribery thresholds (non-combat paths already exist — use them).
5. Author 4 new warlord doctrines, each with a named warlord, territory, response-action set, and a personality (cautious besieger, slaver economist, ash-cult zealot, ex-military professional).
6. Give each doctrine 3–4 bespoke `WarlordResponseActions` and trade/war stances.
7. Add faction-lore entries for the new warlords (fictional names only; tone rules).
8. Cross-check all ids (`faction_`, `warlord_`, weapon/ammo refs) against catalogs.
9. Run data-integrity selftest + combat headless selftest.
10. xUnit: each new enemy parses, behaves per doctrine; each doctrine resolves response actions; determinism preserved (`ISeededRng`).

**Next steps:** warlord succession/civil-war events (registry suggestion); doctrine-driven raid scheduling.

---

## Task 10B — Armory & ammunition expansion (5 → 15 weapons)

**Goal:** Triple the weapon and ammo catalog with degraded, improvised, and pre-war tiers so
loadout is a real decision and `WeaponConditionSystem`/`BallisticsSystem` have material to chew on.

**Files:** `combat_catalog.json`, `items.json` (weapon/ammo item entries),
`recipes.json` (improvised weapon crafting, custom ammo loading), read-only
`WeaponConditionSystem.cs`, `BallisticsSystem.cs`, `CraftingSystem.cs`.

**Substeps:**
1. Read the weapon/ammo schema (caliber, penetration, ricochet, fouling/jam rates, wear).
2. Map existing 5 weapons to their tier/role to find empty niches (melee? improvised? precision?).
3. Author 4 improvised weapons (pipe shotgun, nail driver, rebar spear, molotov) — craftable, high jam/wear, low ceiling.
4. Author 4 pre-war military weapons (service rifle, marksman rifle, SMG, sidearm) — rare, low wear, ammo-hungry.
5. Author 2 degraded relic weapons (rust-pitted, unreliable but available) to feed the condition system.
6. Author 6+ ammo types incl. custom-loading recipes (hand-loaded, incendiary, subsonic) wired to `CraftingSystem`.
7. Balance each against ballistics (penetration vs armor classes) — run `ashfall-balance-sim` / `ashfall-equipment-balance`.
8. Wire improvised weapons into `recipes.json` with real component ids.
9. Validate ids; data-integrity selftest; combat selftest.
10. xUnit: new weapons parse, fouling/jam curves hold, ammo recipes consume correct components; cross-tool QA (coupled balance variables).

**Next steps:** weapon mods/attachments (registry suggestion); ammo scarcity as economy lever.

---

## Task 10C — Vehicle fleet & dive-site expansion (3 → 8 vehicles, 4 → 12 dives)

**Goal:** Expand the motorized expedition layer and the deep-coast diving layer, both fully
coded and nearly empty.

**Files:** `vehicles.json`, `dive_sites.json`, `black_flotilla_items.json` (marine gear),
`locations.json` (coastal/underwater `loc_*`), read-only `ExpeditionVehicleSystem.cs`,
`District8DeepCoastSystem.cs` (dive + noise model).

**Substeps:**
1. Read `vehicles.json` schema (chassis, fuel, armor, cargo, breakdown) + `ExpeditionVehicleSystem`.
2. Read `dive_sites.json` schema + the dive/noise model in `District8DeepCoastSystem`.
3. Author 5 new vehicles across roles: steam halftrack (slow, fuel-flexible), armored mobile base (huge cargo, huge fuel), salvage dredger (coastal bonus), scout motorcycle (fast, fragile), ambulance rig (medical expedition bonus).
4. Give each a fuel/logistics profile that pressures existing resources (diesel, parts) — no new currencies.
5. Author 8 new dive sites: sunken submarine wreck (registry suggestion), flooded metro, submerged convoy, drowned fuel depot — each with acoustic-noise constraints and tiered loot.
6. Tie 2 dive sites to faction-war aftermath (Plan 06C) and 1 to a cipher-quest coordinate (Plan 11C).
7. Author the marine-gear items the dives require (rebreather, weighted line) in `black_flotilla_items.json` if absent.
8. Cross-check `loc_*` refs, item refs, vehicle stat ranges.
9. Run data-integrity + expedition + maritime selftests.
10. xUnit: vehicle stat application (fuel/cargo/breakdown determinism), dive noise threshold behavior, save round-trip for garage + dive progress.

**Next steps:** vehicle chase encounters (needs 10A enemies); a dedicated dive-gear condition loop (equipment-balance skill).
