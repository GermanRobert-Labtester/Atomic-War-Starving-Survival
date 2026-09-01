# Weapon Condition, Degradation & Jam Matrix

**Document:** `docs/combat/WEAPON_CONDITION_MATRIX.md`
**Catalog Authority:** `Assets/StreamingAssets/Data/combat_catalog.json`
**Runtime System:** [`Assets/Ashfall.Core/EquipmentConditionSystem.cs`](../../Assets/Ashfall.Core/EquipmentConditionSystem.cs)

---

## 1. Degradation, Jam Probability & Maintenance Profile

| Weapon ID | Tier | Degrade / Shot | Base Jam Rate | Critical Threshold | Scrap Repair Cost | Maintenance Profile |
|---|---|---|---|---|---|---|
| `weapon_pipe_rifle` | Improvised | 0.022 | 0.055 | 0.30 | 3 scrap | High wear, crude barrel machining; cheap field repair. |
| `weapon_scrap_shotgun` | Improvised | 0.026 | 0.050 | 0.28 | 4 scrap | Heavy chamber stress from 12ga loads; moderate scrap repair. |
| `weapon_bolt_rifle` | Civilian | 0.012 | 0.025 | 0.22 | 4 scrap | Rugged manual action; very low degradation rate. |
| `weapon_assault_rifle` | Military | 0.015 | 0.030 | 0.25 | 5 scrap | Standard military gas system; reliable when cleaned. |
| `weapon_lmg` | Military | 0.020 | 0.040 | 0.28 | 6 scrap | Sustained automatic fire builds heat and fouling rapidly. |
| `weapon_pipe_shotgun` | Improvised | 0.031 | 0.070 | 0.30 | 4 scrap | Fragile break-action hinge; highest jam risk in shotgun class. |
| `weapon_nail_driver` | Improvised | 0.028 | 0.062 | 0.28 | 3 scrap | Pneumatic seals leak and foul under wasteland dust. |
| `weapon_rebar_spear` | Improvised | 0.010 | 0.018 | 0.18 | 2 scrap | Mechanical launcher / thrust weapon; near-zero mechanical failure. |
| `weapon_molotov_thrower` | Improvised | 0.014 | 0.005 | 0.10 | 1 scrap | Sling tension cord wears slowly; virtually immune to barrel jams. |
| `weapon_service_rifle` | Military | 0.010 | 0.020 | 0.22 | 5 scrap | High-grade mil-spec chrome lining; minimal wear per shot. |
| `weapon_marksman_rifle` | Precision | 0.008 | 0.018 | 0.20 | 5 scrap | Precision match action; extremely durable when preserved. |
| `weapon_smg` | Civilian | 0.017 | 0.035 | 0.24 | 4 scrap | Blowback automatic mechanism; moderate fouling in 3-round bursts. |
| `weapon_sidearm` | Police | 0.011 | 0.022 | 0.20 | 3 scrap | Compact semi-automatic pistol; dependable backup sidearm. |
| `weapon_rust_mosin` | Relic | 0.029 | 0.075 | 0.32 | 4 scrap | Heavy corrosion and pitted bore; high jam rate despite steel construction. |
| `weapon_farm_carbine` | Improvised | 0.023 | 0.058 | 0.26 | 2 scrap | Lightweight rimfire action prone to extraction failures when fouled. |
