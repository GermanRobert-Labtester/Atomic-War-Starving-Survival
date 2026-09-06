# WASTELAND MAP DATA MATRIX
## Node Classification, Fog State Progression, and Cartographic Traits

**Document Version:** 1.0.0
**Scope:** Plan 73 (Strategic Wasteland Cartography)
**Status:** Canonical Map Data Specification

---

## 1. Fog State Progression Model

The wasteland map utilizes four discrete fog states for every POI / node:

```text
  Unknown
     │
     ├───────────► [Radio Intercept / Trader Gossip]
     ▼
  Rumored
     │
     ├───────────► [Aerial Survey / Seismic Mapping / Recon Radar]
     ▼
  Surveyed
     │
     ├───────────► [Survivor Expedition Arrival / Exploration]
     ▼
  Visited
```

### 1.1 State Precision Rules

| Fog State | Position Display | Danger Classification | Scavenging / Loot | Travel Route Estimate |
|---|---|---|---|---|
| **Unknown** | Hidden (no marker on map canvas) | Hidden | Hidden | Unroutable |
| **Rumored** | Fuzzed region (±50–100km offset/zone) | Approximate Band (Low / Med / High) | Obfuscated ("Unconfirmed scrap/rumor") | Coarse estimate only; exact duration hidden |
| **Surveyed**| Precise Coordinates | Confirmed Hazard Rating + Active Trait Annotations | Identified Category ("Electronics", "Ammunition") | Exact travel ticks & fuel requirement via `ExpeditionSystem.Estimate` |
| **Visited** | Exact Coordinates | Ground Truth Verified | Precise Salvage History & Depletion State | Exact verified route history |

---

## 2. Cartography Survey Annotations

When a node transitions to `Surveyed` or is analyzed via settlement research, specific domain traits are unlocked:

1. **Hydrogeology:** Water salinity, aquifer depth, flow rate, contamination index (`trait_aquifer_potable`, `trait_heavy_brine`).
2. **Seismic Dynamics:** Fault faultline proximity, subsidence risk, tectonic stability (`trait_seismic_fault_active`).
3. **Structural Condition:** Pre-war concrete integrity, collapse probability, bunker seal status (`trait_bunker_hermetic`).
4. **Terrain Mobility:** Swamp, radiation mud, vehicle passability (`trait_chassis_passable`, `trait_amphibious_required`).
5. **Radiation Micro-Climates:** Hotspots, windblown fallout corridors, radon venting (`trait_rad_hotspot`).

---

## 3. Route Estimation Parity Contract

- **Single Authority Invariant:** Map route displays must not compute distance, travel ticks, fuel, or breakdown chances with independent math.
- All projections call `ExpeditionSystem.Estimate(def, stance, isNightScavenge, vehicle, weaponReadiness, weaponJamRisk)`.
- Parity is mechanically verified by unit tests comparing `WastelandMapSystem.EstimateRoute` with `ExpeditionSystem.Estimate`.
