# Hardcore Shock Item Matrix

## 1. Dynamic Shock Goods Coverage

This matrix maps each transient event kind to its operational parameters and commodity targets:

| Shock Kind (`PriceShockKind`) | Multiplier | Duration (Days) | Targeted Item Patterns | Targeted Item Categories | Primary Simulation Trigger |
|:---|:---:|:---:|:---|:---|:---|
| `PlumePassing` | 1.8x | 3 | `*` (Catch-all) | Universal commodities across market | Radioactive fallout plume intersects trade corridor |
| `ConvoyAmbush` | 1.6x | 3 | `fuel`, `canned_food`, `medical_kit` | Expedition logistics & preserved rations | Highway ambush by organized raider warband |
| `FactionConflict` | 1.7x | 5 | `ammo_*`, `medical_kit`, `fuel` | Munitions, combat surgery, logistics | Border dispute escalates into open mortar exchange |
| `SeasonalScarcity` | 1.5x | 7 | `canned_food`, `clean_water`, `seed_packets` | Sustenance calories & agricultural inputs | Sudden blizzard locks down surface transport routes |
| `DiseaseOutbreak` | 2.0x | 4 | `antibiotics`, `medical_kit`, `clean_water` | Pharmaceuticals & sterile hydration | Waterborne bacterial epidemic in crowded shelter |
| `FuelShortage` | 1.9x | 3 | `fuel`, `engine`, `scrap_mechanical` | Hydrocarbons & mechanical prime movers | Regional refinery pumping station failure or fire |
