# Ammo / Caliber Matrix — 14 catalog calibers × 20 weapons

Caliber authority: `combat_catalog.json → ammo[]` (+ matching `items.json`
ammo items for inventory/trade/reload economy).

| Ammo id | dmg_mod | rng_mod | military | Consumer weapons (combat catalog) | Item | Reload recipe |
|---|---|---|---|---|---|---|
| ammo_357 | 1.00 | 1.00 | no | pipe_rifle, **revolver** | ✓ | — |
| ammo_12g | 1.05 | 0.90 | no | scrap_shotgun, pipe_shotgun | ✓ | — |
| ammo_308 | 1.10 | 1.20 | no | bolt_rifle, marksman_rifle | ✓ | reload_308_incendiary (→ different id) |
| ammo_556 | 1.00 | 1.15 | **yes** | assault_rifle, service_rifle | ✓ | reload_9x19 (→ different id) |
| ammo_762 | 1.10 | 1.25 | **yes** | lmg, rust_mosin, **battle_rifle** | ✓ | — |
| ammo_9x19 | 0.95 | 1.00 | no | nail_driver, smg, sidearm | ✓ | reload_9x19 ✓ |
| ammo_22lr | 0.70 | 0.85 | no | farm_carbine | ✓ | reload_22lr ✓ |
| **ammo_762x54r** | 1.15 | 1.30 | yes | **trail_carbine** (was orphan) | ✓ | — |
| **ammo_357_jhp** | 1.25 | 1.00 | no | — (deferred) | ✓ | reload_357_jhp ✓ |
| **ammo_12g_buck** | 1.40 | 0.85 | no | **coach_shotgun** (was orphan) | ✓ | reload_12g_buck ✓ |
| **ammo_308_incendiary** | 1.15 | 1.05 | no | — (deferred) | ✓ | reload_308_incendiary ✓ |
| **ammo_556_subsonic** | 0.85 | 0.90 | no | **quiet_carbine** (was orphan) | ✓ | — |
| ammo_improvised_rod | 1.20 | 0.60 | no | rebar_spear | — (improvised) | — |
| ammo_improvised_burn | 1.05 | 0.85 | no | molotov_thrower | — (improvised) | — |

## Plan 54 outcome

- **Orphan calibers reduced 5 → 2.** `762x54r`, `12g_buck`, `556_subsonic`
  gained consumer weapons (before: no weapon could fire them).
- Remaining orphans `ammo_357_jhp` + `ammo_308_incendiary` are reload-recipe
  outputs (tradeable items, craft loops intact) awaiting a future consumer
  weapon. Gated by
  `Plan54CombatCatalogTests.Catalog_OrphanCalibersAreDocumented`.
- **No new calibers were added** (constraint 1.9 / §55): all five new weapons
  reuse existing families, so the ammo economy is not fragmented.
- Military-tier interaction preserved: `battle_rifle`/`trail_carbine` fire
  military-tier rounds but are non-jury, so no burst-failure exposure;
  subsonic stays `is_military_tier: false` (the "safe" 5.56-family round).
