# Dose Location Faction Handoff

> **Integration:** Contract connecting faction territorial checkpoints to radiological geography.

---

## 1. Tactical vs. Radiological Risk Decoupling

A critical architectural tenet of ASHFALL is that **radiological danger and faction combat hostility are separate axes**:
- A warlord garrison checkpoint may represent extreme lethal danger due to sniper rifles, landmines, and hostile guards.
- However, the physical terrain itself may only have a moderate radiological baseline (e.g. 4.10 µSv/h at `loc_garrison_checkpoint_gamma_exterior`), derived from diesel soot and churned gravel rather than reactor debris.
- Conversely, an untamed wild forest edge (`loc_irradiated_forest_edge`) has zero enemy combatants, yet poses an acute radiological hazard (18.5 µSv/h) from biological fallout concentration.

---

## 2. Checkpoint Geography & Territory Integration

- **Faction Space:** `loc_garrison_checkpoint_gamma_exterior` maps to the territorial cordon of the Iron Garrison / Sector 4 warlords (`loc_garrison_checkpoint_gamma` in `expeditions.json`).
- **Checkpoint Dynamics:**
  - Approaching the checkpoint exposes the expedition party to 4.10 µSv/h while awaiting clearance or negotiating transit rights.
  - If detained in exterior holding pens, lingering survivors accumulate steady dose.
  - Bribing sentries or presenting forged clean-bill chits (`item_forged_clean_bill_chit`) minimizes dwell time, reducing total booked dose.
