# Ammunition & Ballistics Matrix

**Document:** `docs/combat/AMMO_BALLISTICS_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`
**Runtime System:** [`Assets/Ashfall.Core/Combat/BallisticsSystem.cs`](../../Assets/Ashfall.Core/Combat/BallisticsSystem.cs)

---

## 1. Authored Ammunition Types (14 Total)

| Ammo ID | Display Name | Damage Mod | Range Mod | Military Tier | Primary Consumers | Ballistic & Tactical Profile |
|---|---|---|---|---|---|---|
| `ammo_357` | .357 Magnum | 1.00 | 1.00 | No | `weapon_pipe_rifle` | Standard handgun/lever cartridge with solid stopping power. |
| `ammo_12g` | 12ga Standard Shell | 1.05 | 0.90 | No | `weapon_scrap_shotgun`, `weapon_pipe_shotgun` | Heavy short-range kinetic spread; effective against unarmored fauna. |
| `ammo_308` | .308 Winchester | 1.10 | 1.20 | No | `weapon_bolt_rifle`, `weapon_marksman_rifle` | High-velocity full-power rifle round with long range penetration. |
| `ammo_556` | 5.56x45mm NATO | 1.00 | 1.15 | Yes | `weapon_assault_rifle`, `weapon_service_rifle` | Military standard cartridge; balanced trajectory and controlled burst handling. |
| `ammo_762` | 7.62x39mm Soviet | 1.10 | 1.25 | Yes | `weapon_lmg`, `weapon_rust_mosin` | Heavy military rifle round with strong cover penetration and barrier punch. |
| `ammo_9x19` | 9x19mm Parabellum | 0.95 | 1.00 | No | `weapon_smg`, `weapon_sidearm`, `weapon_nail_driver` | Common pistol ammunition; compact, lightweight, ideal for volume fire. |
| `ammo_22lr` | .22 Long Rifle | 0.70 | 0.85 | No | `weapon_farm_carbine` | Small-game scavenging cartridge; minimal recoil, low material cost. |
| `ammo_762x54r` | 7.62x54R Rimmed | 1.15 | 1.30 | Yes | Sniper / heavy platforms | Heavy rimmed military cartridge; maximum range and ceramic plate penetration. |
| `ammo_357_jhp` | .357 JHP Hand-Loaded | 1.25 | 1.00 | No | `weapon_pipe_rifle` | Jacketed hollow-point hand-load; massive tissue damage on unarmored targets. |
| `ammo_12g_buck` | 12ga Buckshot Hand-Loaded | 1.40 | 0.85 | No | `weapon_scrap_shotgun`, `weapon_pipe_shotgun` | Heavy pellet payload hand-packed with lead shot; lethal at point-blank range. |
| `ammo_308_incendiary` | .308 Incendiary Hand-Loaded | 1.15 | 1.05 | No | `weapon_bolt_rifle`, `weapon_marksman_rifle` | Specialty tracer/pyrophoric tip; ignites flammable targets and cover. |
| `ammo_556_subsonic` | 5.56 Subsonic | 0.85 | 0.90 | No | `weapon_assault_rifle`, `weapon_service_rifle` | Reduced propellant charge for acoustic stealth and low weapon wear. |
| `ammo_improvised_rod` | Improvised Rebar Rod | 1.20 | 0.60 | No | `weapon_rebar_spear` | Heavy cut rebar projectile; armor-puncturing mass at very close range. |
| `ammo_improvised_burn` | Improvised Burn Charge | 1.05 | 0.85 | No | `weapon_molotov_thrower` | Chemical accelerant canister; creates persistent flame patches on impact. |

---

## 2. Ballistic Materials & Cover Interaction

| Material ID | Display Name | Kind | Armor Reduction | Ricochet Chance | Energy Retained |
|---|---|---|---|---|---|
| `material_wood` | Rotted Wood | Cover | 30% | 5% | 50% |
| `material_concrete` | Concrete | Cover | 60% | 15% | 60% |
| `material_metal` | Sheet Metal | Cover | 50% | 35% | 70% |
| `material_rebar` | Rebar Barricade | Barrier | 65% | 20% | 60% |
| `armor_cloth` | Padded Cloth | Armor | 25% | 0% | 50% |
| `armor_kevlar` | Scavenged Kevlar | Armor | 50% | 10% | 60% |
| `armor_plate` | Ceramic Plate | Armor | 65% | 15% | 70% |
