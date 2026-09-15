# Expedition vehicle comparison

This table is the current authored ladder, not a replacement economy model.
The 30-day playtest is a lifecycle proof; its route mix is intentionally not
used to claim universal dominance.

| Profile | Speed | Cargo | Fuel/km | Breakdown threshold | 30-day playtest fuel | 30-day loot value | Role evidence |
|---|---:|---:|---:|---:|---:|---:|---|
| Utility Quad | 1.30 | 90 | 0.30 | 0.20 | 6.0 | 16.2 | starter generalist |
| Dirt Bike | 1.80 | 30 | 0.20 | 0.25 | 4.0 | 103.0 | fast/light |
| Cargo Truck | 1.60 | 250 | 0.50 | 0.15 | 5.0 | 6.6 | cargo specialist |
| Steam Halftrack | 0.85 | 180 | 0.70 | 0.18 | 21.0 | 54.0 | heavy rough-terrain option |
| Armored Mobile Base | 0.70 | 380 | 0.95 | 0.15 | 19.0 | 22.0 | maximum cargo/protection role |
| Salvage Dredger | 0.95 | 260 | 0.55 | 0.20 | 16.5 | 15.0 | coastal specialist |
| Scout Motorcycle | 2.40 | 18 | 0.18 | 0.30 | 1.8 | 69.0 | speed/range specialist |
| Ambulance Expedition Rig | 1.25 | 140 | 0.45 | 0.22 | 6.75 | 101.0 | road/medical role |

The profiles expose explicit speed, cargo, fuel, and failure trade-offs. No
strict-dominance assertion is made from a single route because loot and terrain
are route-dependent. The fixed lifecycle run found no invariant violation and
made no rebalance. Future tuning must use the fixed-seed multi-route balance
simulation and preserve the same estimate/execution, save, and conservation
gates.
