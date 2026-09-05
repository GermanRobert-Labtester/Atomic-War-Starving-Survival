# Ammo Reloading Contract (Plan 55)

## Caliber authority

`combat_catalog.json` is the sole weapon/caliber authority. Live calibers:
`ammo_357`, `ammo_12g`, `ammo_308`, `ammo_556`, `ammo_762`, `ammo_9x19`,
`ammo_22lr`, `ammo_762x54r`, `ammo_762x54r_jhp_ap`, `ammo_357_jhp`,
`ammo_12g_buck`, `ammo_308_incendiary`, `ammo_556_subsonic`,
`ammo_improvised_rod`, `ammo_improvised_burn`.

Plan 55 reload recipes output **existing** caliber IDs only:

| Recipe | Output | Live weapon consumers (combat_catalog.json) |
|---|---|---|
| `reload_556` | `ammo_556` ×10 | assault rifle + 1 additional 5.56mm platform |
| `reload_762` | `ammo_762` ×10 | 7.62mm battle rifle |

No caliber was invented; no Plan-10-era ammo identity was duplicated.

## Component model & conservation

The existing economy models casings concretely: `empty_brass_shell` (loot-only,
stack 100), `reloading_primer` (stack 100), `smokeless_powder` (stack 10),
plus per-recipe special components (`cardboard_wad`, `aluminum_shavings`).

**Casing policy:** shells are consumed 1:1 with rounds produced. No recipe,
dismantle path, or byproduct creates casings, primers, or powder. Conservation
is pinned by `Plan55CraftingCatalogTests.Plan55_ammo_reload_recipes_consume_casings_one_to_one`.

**Component provenance (repaired by Plan 55):** `reloading_primer` and
`smokeless_powder` previously had zero acquisition paths (items.json and
recipes.json only) — all 7 reload recipes were economically dead. Plan 55 adds:
- `table_loot_military_depot`: primers (weight 12, qty 8–20, uncommon), powder (weight 10, qty 1–3, uncommon)
- `table_loot_police_station`: primers (weight 8, qty 5–12, uncommon), powder (weight 6, qty 1–2, rare)

`empty_brass_shell` already drops from 3 existing tables (untouched).

## Handload vs. factory balance

- Craft margin per batch is positive (+62 / +72 tradeValue) — this is the
  intended craft value and is bounded by **loot-only, uncommon** primer/powder
  acquisition (no trade catalog sells components; the margin cannot be
  repeated from vendor purchases).
- Factory ammunition remains strictly better per scarcity hour: it drops from
  military/police tables at common weight with zero labor, while handloads
  spend workbench time plus scarce components. Handloads convert *looted
  brass* into *usable ammunition* — a scarcity bridge, not dominance.
- Reliability/condition tradeoffs are owned by `EquipmentConditionSystem` /
  ballistics authorities; Plan 55 adds no ballistic claims or new durability
  semantics.

## Explicitly out of scope (unchanged)

Casing recovery on shot, primer manufacturing, powder manufacturing,
new calibers, plan-55-only ammo families, generic-metal-to-ammo conversion.
