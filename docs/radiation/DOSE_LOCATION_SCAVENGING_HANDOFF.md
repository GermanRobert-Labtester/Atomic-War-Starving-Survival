# Dose Location Scavenging Handoff

> **Integration:** Contract connecting radiological geography to scavenging tables, loot generation, and survivor risk-reward calculations.

---

## 1. Risk-Reward Core Philosophy

Radiation geography creates natural mechanical incentives:
- **Low Dose / Depleted Sites:** Sites near the shelter with low radiation have been thoroughly scavenged by prior survivors over the initial years following the exchange. Yields consist mostly of common scrap and low-grade salvage.
- **High Dose / High Yield Sites:** Extreme hot zones (such as `loc_military_depot_perimeter` or `loc_ruined_hospital_grounds`) deterred previous scavengers due to lethal acute exposure. As a result, sealed caches, military ordnance, sterile surgical gear, and uncorrupted pre-war electronic components remain intact.

---

## 2. Separation of System Authorities

- **Scavenging Authority:** `ScavengingSystem` and `scavenging_tables.json` own item drop chances, tier pools, and harvest quantities.
- **Radiological Authority:** `DoseContentCatalog` and `DoseLedgerSystem` own the physiological and bureaucratic cost of remaining in that location.
- **Decoupled Linkage:** Radiation does **not** automatically multiply loot drops via generic code hacks. Instead, high-tier loot tables (e.g. `table_loot_ordnance_shoulder`, `table_loot_hospital`) are authored on high-danger destinations that correlate geographically with hot dose perimeters.

---

## 3. Scavenger Planning Trade-Offs

When sending a sortie to `loc_ruined_hospital_grounds`:
- **Loot Potential:** Critical pharmaceuticals, surgical kits, antibiotics.
- **Dose Burden:** ~0.11 mSv per 4-hour search.
- **Mitigation Options:**
  - Equip `item_shielded_badge_case` to protect the dosimeter tag from spurious gamma fogging.
  - Ingest `item_chelation_decorporation_course` post-expedition to reduce booked mSv by 40%.
  - Assign a survivor with high `scavenging` skill to complete the search in 2.5 hours rather than 5 hours.
