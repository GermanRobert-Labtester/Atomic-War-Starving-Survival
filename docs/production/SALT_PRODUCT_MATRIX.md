# Salt Product Matrix & Mine Processing Flow

This document details the chemical and mechanical extraction pipelines of `SaltMineExtractionSystem`, treaty delivery interfaces with District 8/Silent Foundry, and cross-system resource routing.

---

## 1. Primary Output Streams

```text
Salt Ore Extraction (Vein Drill)
    ├── (60%) Coarse Halite Rock ──> Crushing/Grading ──> Coarse Preservation Salt (`item_preservation_salt`)
    │                                                └──> Trade Salt Sack (`item_trade_salt_sack`)
    ├── (30%) Mineral Brine ───────> Lead-Antimony Pipe ──> Evaporator ──> Saline Salt (`item_medical_saline_salt`)
    │                                                   └──> Treaty Delivery (`treaty_brine_pipe_and_iodine_exchange`)
    └── (5%) Raw Sulfur Dust ──────> Slag Separation ──> Chemical/Pharma Feedstock (`item_raw_sulfur`)
```

---

## 2. Extraction & Labor Cost Parameters

| Stream | Extraction Rate / Worker-Day | Power Draw (kW-h) | Tool / Drill Wear | Respiratory Hazard (Contam.) | Primary Bottleneck |
|---|---|---|---|---|---|
| **Rock Salt** | 12.0 kg | 0.5 units | 0.02 condition/day | 0.01 / worker-day | Drill bit hardness (`item_foundry_drill_blanks`) |
| **Brine Pumping** | 6.0 barrels | 0.8 units | 0.01 pressure/day | 0.005 / worker-day | Lead-antimony pipes (`item_foundry_brine_pipe`) |
| **Sulfur Skim** | 1.0 kg | 0.3 units | 0.005 condition/day | 0.025 / worker-day | Air filtration masks (`gas_mask`) |

---

## 3. Treaty Delivery & Accord Compliance

- **Obligation**: `treaty_brine_pipe_and_iodine_exchange` demands 20 barrels of mineral brine and 50 kg of graded salt per assessment cycle.
- **Consequence of Fulfilment**: Unlocks medical iodine pills (`iodine_pills`) and antiseptic supplies from The Office.
- **Consequence of Default**: Halts iodine flow, increases shelter thyroid radiation susceptibility, lowers Office standing (-6).
