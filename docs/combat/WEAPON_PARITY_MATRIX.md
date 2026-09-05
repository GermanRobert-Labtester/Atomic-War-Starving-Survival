# Weapon Parity Matrix — 15 baseline weapons (frozen oracle)

Constraint 1.2: none of these values were altered. Pinned by
`Plan54CombatCatalogTests.Catalog_PreservesAll15BaselineWeaponsWithCalibers`.

| id | display | acc | dmg | rng | cal | burst | jury | supp | degr | jam | rep | thr |
|---|---|---|---|---|---|---|---|---|---|---|---|---|
| weapon_pipe_rifle | Pipe Rifle | .46 | 12 | 1.00 | 357 | 1 | ✓ | — | .022 | .055 | 3 | .30 |
| weapon_scrap_shotgun | Scrap Shotgun | .50 | 20 | 1.00 | 12g | 1 | ✓ | — | .026 | .050 | 4 | .28 |
| weapon_bolt_rifle | Held-Bolt Rifle | .68 | 18 | 1.00 | 308 | 1 | — | — | .012 | .025 | 4 | .22 |
| weapon_assault_rifle | Assault Rifle | .72 | 14 | 1.15 | 556 | 3 | — | ✓ | .015 | .030 | 5 | .25 |
| weapon_lmg | Light Machine Gun | .60 | 11 | 1.20 | 762 | 5 | — | ✓ | .020 | .040 | 6 | .28 |
| weapon_pipe_shotgun | Pipe-Shot Shellgun | .42 | 18 | 0.90 | 12g | 1 | ✓ | — | .031 | .070 | 4 | .30 |
| weapon_nail_driver | Nail-Driver Spike Rifle | .40 | 14 | 0.95 | 9x19 | 1 | ✓ | — | .028 | .062 | 3 | .28 |
| weapon_rebar_spear | Rebar Spear & Pipe Launcher | .78 | 22 | 0.60 | rod | 1 | ✓ | — | .010 | .018 | 2 | .18 |
| weapon_molotov_thrower | Fuel-Sling Molotov Rig | .55 | 16 | 0.85 | burn | 1 | ✓ | — | .014 | .005 | 1 | .10 |
| weapon_service_rifle | M-Spec Service Rifle | .76 | 15 | 1.20 | 556 | 2 | — | ✓ | .010 | .020 | 5 | .22 |
| weapon_marksman_rifle | Sniper's Bolt-Action | .86 | 24 | 1.80 | 308 | 1 | — | — | .008 | .018 | 5 | .20 |
| weapon_smg | Civilian SMG | .62 | 11 | 0.95 | 9x19 | 3 | — | ✓ | .017 | .035 | 4 | .24 |
| weapon_sidearm | Police Sidearm | .74 | 12 | 0.95 | 9x19 | 1 | — | — | .011 | .022 | 3 | .20 |
| weapon_rust_mosin | Rust-Pitted Mosin | .58 | 16 | 1.10 | 762 | 1 | — | — | .029 | .075 | 4 | .32 |
| weapon_farm_carbine | Farm-Clearing Carbine | .54 | 12 | 1.00 | 22lr | 1 | ✓ | — | .023 | .058 | 2 | .26 |

**Cal = caliber shorthand; jury = is_jury_rigged; supp = is_suppression_capable;
degr = degrade_per_shot; rep = scrap_repair_cost; thr = condition_threshold.**

## Observed baseline debt (documented, NOT corrected — constraint 1.2)

- `weapon_rust_mosin` is chambered `ammo_762` while its lore item
  (`ammo_762x54r` description) says the Mosin feeds 7.62×54R. A caliber swap
  would violate "change ammo semantics" — left as-is.
- `weapon_smg` / `weapon_sidearm` are tagged non-jury despite "Civilian"/
  "Police" names — kept (display-name lore, not a mechanic).
