# Plan 168 — Fluid Logistics Closeout

`FluidLogisticsSystem` owns shelter network topology, nodes, edges, valves, pressure, deterministic sink allocation, pipe condition, leaks, bursts, and volume-weighted contaminant mixing. `WaterTreatmentSystem` remains the authority for bulk raw/brackish/irradiated/clean water and treatment chemistry.

`FluidWaterTreatmentBridge.TransferTreatedWater` validates network capacity before removing treatment inventory and refunds on an unexpected network commit failure. This establishes the one-way transfer seam without duplicating treatment quantities.

The catalog is `Assets/StreamingAssets/Data/fluid_infrastructure.json`. `FluidLogisticsHostSession` is enrolled in the Godot composition root and campaign day coordinator. The solver applies deterministic path order, sink priorities, pump capacity, valve state, pressure, condition, leaks, and quality conservation.

Focused verification: `Plan168FluidLogisticsTests` passed 6/6. Core and Godot host builds passed. Survivor daily drinking ownership, Greenhouse delivery, Disease exposure routing, weather source adapters, and a production PlumbingPanel remain follow-up integration work; no second thirst or disease authority was introduced.
