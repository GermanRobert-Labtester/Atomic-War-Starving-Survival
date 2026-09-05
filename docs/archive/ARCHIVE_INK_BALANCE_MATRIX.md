# Archive Ink Balance Matrix

> **Balance Audit:** Multi-criteria trade-offs, Pareto efficiency, and anti-dominance proofs across all 12 formulations.

---

## 1. Trade-Off Vectors

| Ink ID | Legibility | Longevity | Fade/Day | Cost Vector | Primary Advantage | Primary Vulnerability |
|---|:---:|:---:|:---:|---|---|---|
| `ink_blood_emergency` | 0.40 | 100d | 0.0100 | 1 `blood_sample` | Zero fuel/metal cost | Rapid decay, moral/bio hazard |
| `ink_berry_juice` | 0.50 | 150d | 0.0070 | 2 `berries` | Renewable food forage | Consumes precious rations, quick fade |
| `ink_improvised_pigment`| 0.55 | 180d | 0.0060 | 2 `mineral_chunk` | Abundant tunnel stone | Heavy wear on quills, muddy contrast |
| `ink_plant_dye` | 0.60 | 200d | 0.0020 | 1 `cloth` | Low fade rate for tier | Drains clothing/bandage cloth stock |
| `ink_mineral_oxide` | 0.60 | 220d | 0.0050 | 2 `scrap_metal` | Infinite scrap availability | Coarse iron grit, modest fade |
| `ink_lampblack` | 0.65 | 250d | 0.0040 | 1 `charcoal` | Cheapest carbon option | Flakes faster than refined soot |
| `ink_sepia` | 0.70 | 280d | 0.0040 | 1 `organic_residue`| Rich warm tone, clear text | Organic spoil risk |
| `ink_soot_lamp` | 0.70 | 300d | 0.0015 | 1 `charcoal` | Exceptional value ratio | Modest legibility ceiling |
| `ink_diluted_toner` | 0.75 | 350d | 0.0030 | 1 `empty_toner_cartridge`| High crispness | Non-renewable office salvage |
| `ink_chemical_marker` | 0.80 | 400d | 0.0030 | 1 `chemical_solvent` | High legibility, long life | Chemical odor, solvent scarcity |
| `ink_iron_gall` | 0.90 | 500d | 0.0008 | 2 `charcoal` | Superb durability & clarity| High charcoal draw (competes with filter) |
| `ink_archival_carbon` | 0.95 | 600d | 0.0010 | 3 `charcoal` | Peak legibility & survival | Triple charcoal draw (severe early strain) |

---

## 2. Pareto Dominance Audit

- **No Universal Dominance:** No single ink provides highest legibility, longest life, lowest fade, and lowest cost simultaneously.
- `ink_archival_carbon` has the highest legibility (0.95) and longest life (600d), but costs 3 `charcoal`—a heavy fuel/water-filter sacrifice.
- `ink_iron_gall` has a slower fade rate (0.0008) than `archival_carbon` (0.0010) and costs 1 less charcoal, making it a viable alternative for slow-fade archival storage.
- Improvised inks provide crucial utility when coal and solvents must be hoarded for warmth and medicine.
