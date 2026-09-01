# Foundry Material, Heat & Labor Matrix

This document defines the physical and operational parameters for all 25 foundry products across 4 distinct Heat & Labor bands in `SilentFoundrySystem.Heat` and `SilentFoundrySystem.TreatyLabor`.

---

## 1. Heat & Labor Bands

```text
Band 1: Low Heat / Light Labor (850°C–1000°C · 3–6 labor hours · 2–3 fuel)
  ├── Ice Anchors (bundle)
  ├── Bracket & Fastener Set
  ├── Heavy Structural Coupling
  └── Heavy Cast Shot

Band 2: Medium Heat / Standard Labor (1000°C–1200°C · 6–10 labor hours · 4–5 fuel)
  ├── Plowshare Set
  ├── Repair Plate
  ├── Water Valve Body
  ├── Heavy Foundry Tool
  ├── Excavation Shoring Bracket
  ├── Hardened Drill Blank Set
  ├── Hydraulic Press Fitting
  └── Weather Canister Shell Body

Band 3: High Heat / Heavy Structural (1200°C–1350°C · 12–14 labor hours · 6–7 fuel)
  ├── Structural T-Beam
  ├── Blast-Door Armor Plate
  ├── Brine-Resistant Pipe
  ├── Foundation Reinforcement Shoe
  ├── Tooling Die Set
  ├── Cast Crucible Shell
  ├── High-Duty Furnace Grate
  └── Brass-Alloy Casing Blank Set

Band 4: Extreme Heat / Precision Alloy (1350°C–1500°C · 16–18 labor hours · 8–9 fuel)
  ├── Winch Drum
  ├── Heavy-Alloy Part
  ├── Heavy Roof-Armor Plate
  ├── Blast-Door Hinge Fitting
  └── Cast Bearing Housing
```

---

## 2. Scrap Conversion & Conservation Invariants

1. **No Net-Gain Smelting**: Smelting scrap into finished goods always consumes net energy (fuel, water, flux) and produces 0% material duplication.
2. **Failure Recycled at Loss**: A failed cast (`FoundryFailedCastRecord`) returns at most 60% of the input scrap; flux and additives are consumed completely as slag.
3. **Additive Scarcity**: Heavy alloy products (`item_foundry_alloy_additive`, lead-antimony shot) cannot be recycled infinitely from ordinary scrap; they require distinct regional acquisition or treaty allocations.
