# Weapon Role & Tactical Niche Matrix

**Document:** `docs/combat/WEAPON_ROLE_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`
**Runtime System:** [`Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`](../../Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs)

---

## 1. Authored Armory (20 Weapons — Plan 54 expanded from 15)

> Plan 54 delta: 5 additions below the Plan 10 armory. Baseline 15 rows
> unchanged (parity-pinned by `Plan54CombatCatalogTests`). Full Plan 54
> role/economy analysis: `PLAN54_CLOSEOUT.md`, `COMBAT_BALANCE_AUDIT.md`,
> `AMMO_CALIBER_MATRIX.md`.

| Weapon ID | Display Name | Caliber / Ammo | Dmg / Acc / Range | Burst | Jury-Rigged | Jam Base / Wear | Role & Tactical Niche |
|---|---|---|---|---|---|---|---|
| `weapon_pipe_rifle` | Pipe Rifle | `ammo_357` | 12.0 / 0.46 / 1.00 | 1 | Yes | 0.055 / 0.022 | Early-game improvised rifle; high wear and jam risk, low scrap repair cost. |
| `weapon_scrap_shotgun` | Scrap Shotgun | `ammo_12g` | 20.0 / 0.50 / 1.00 | 1 | Yes | 0.050 / 0.026 | Devastating close-range emergency firearm with high recoil and wear. |
| `weapon_bolt_rifle` | Held-Bolt Rifle | `ammo_308` | 18.0 / 0.68 / 1.00 | 1 | No | 0.025 / 0.012 | Standard civilian hunting rifle; reliable single-shot precision. |
| `weapon_assault_rifle` | Assault Rifle | `ammo_556` | 14.0 / 0.72 / 1.15 | 3 | No | 0.030 / 0.015 | Pre-war military standard; burst fire, suppression capable, moderate ammo consumption. |
| `weapon_lmg` | Light Machine Gun | `ammo_762` | 11.0 / 0.60 / 1.20 | 5 | No | 0.040 / 0.020 | Heavy area-suppression platform; high ammunition appetite and weight. |
| `weapon_pipe_shotgun` | Pipe-Shot Shellgun | `ammo_12g` | 18.0 / 0.42 / 0.90 | 1 | Yes | 0.070 / 0.031 | Crude improvised single-shot break-action; cheap desperation weapon. |
| `weapon_nail_driver` | Nail-Driver Spike Rifle | `ammo_9x19` | 14.0 / 0.40 / 0.95 | 1 | Yes | 0.062 / 0.028 | Pneumatic scrap driver converting 9mm powder or compressed charge into spike kinetic energy. |
| `weapon_rebar_spear` | Rebar Spear & Launcher | `ammo_improvised_rod` | 22.0 / 0.78 / 0.60 | 1 | Yes | 0.018 / 0.010 | High-damage close-range thrust / pipe propelled heavy rebar spike; minimal jam risk. |
| `weapon_molotov_thrower` | Fuel-Sling Molotov Rig | `ammo_improvised_burn` | 16.0 / 0.55 / 0.85 | 1 | Yes | 0.005 / 0.014 | Mechanical sling projector for burning incendiary bottles; zero barrel fouling. |
| `weapon_service_rifle` | M-Spec Service Rifle | `ammo_556` | 15.0 / 0.76 / 1.20 | 2 | No | 0.020 / 0.010 | Pristine pre-war military carbine; 2-round controlled burst, low wear, suppression capable. |
| `weapon_marksman_rifle` | Sniper's Bolt-Action | `ammo_308` | 24.0 / 0.86 / 1.80 | 1 | No | 0.018 / 0.008 | Extreme-range precision rifle; lethal single-shot damage against center anchors. |
| `weapon_smg` | Civilian SMG | `ammo_9x19` | 11.0 / 0.62 / 0.95 | 3 | No | 0.035 / 0.017 | Compact 3-round burst automatic for close-quarter flank engagements. |
| `weapon_sidearm` | Police Sidearm | `ammo_9x19` | 12.0 / 0.74 / 0.95 | 1 | No | 0.022 / 0.011 | Lightweight dependable sidearm; low inventory weight and rapid readiness. |
| `weapon_rust_mosin` | Rust-Pitted Mosin | `ammo_762` | 16.0 / 0.58 / 1.10 | 1 | No | 0.075 / 0.029 | Degraded military relic; high base jam rate (7.5%) and barrel pitting, but hard-hitting. |
| `weapon_farm_carbine` | Farm-Clearing Carbine | `ammo_22lr` | 12.0 / 0.54 / 1.00 | 1 | Yes | 0.058 / 0.023 | Lightweight vermin gun using plentiful .22LR ammo; low penetration against armored beasts. |
| `weapon_revolver` | Six-Shot Revolver | `ammo_357` | 15.0 / 0.72 / 0.90 | 1 | No | 0.015 / 0.008 | Rugged civilian backup sidearm; best jam base and lowest repair cost of any non-improvised firearm. |
| `weapon_coach_shotgun` | Coach-Gun Buckloader | `ammo_12g_buck` | 26.0 / 0.46 / 0.70 | 1 | No | 0.038 / 0.020 | Confined-ruins ambush double gun; highest damage per trigger pull in the catalog, fed by craft-gated buckshot handloads. |
| `weapon_trail_carbine` | Cosaque Trail Carbine | `ammo_762x54r` | 19.0 / 0.64 / 1.30 | 1 | No | 0.030 / 0.013 | Long-range civilian marksman in the old rimmed military round; owns the 1.30 range band between service and sniper rifles. |
| `weapon_battle_rifle` | Slate-Groove Battle Rifle | `ammo_762` | 17.0 / 0.70 / 1.25 | 2 | No | 0.026 / 0.014 | Hard-hitting military pairs; suppression-capable middle ground between the assault rifle's spray and the LMG's volume. |
| `weapon_quiet_carbine` | Whisper-Well Carbine | `ammo_556_subsonic` | 10.0 / 0.68 / 1.00 | 2 | No | 0.030 / 0.012 | Low-signature subsonic precision carbine; trades raw damage for the quiet/precision niche, suppression capable. |
