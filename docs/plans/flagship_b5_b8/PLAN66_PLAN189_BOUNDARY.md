# Plan 66 ↔ Plan 189 Scope Boundary

> Required by flagship brief §9.2. Read by any agent implementing either plan.

## What B7 / Plan 66 OWNS

1. **Consumer admission to the shared spendable water authority** — greenhouse, kitchen, medical, decontamination request water through existing authorities (`WaterAuthority`, inventory items, treatment pools). No private counters.
2. **Treatment capacity explicitness** — throughput given incoming contamination, filter state, fuel, powered state; deterministic.
3. **Sump bridge hardening** — the `WireWaterTreatmentSumpBridge()` coupling (incidents → `SetIncomingContamination`), with the `0.8f` constant data-owned **only under exact legacy parity**.
4. **Sump pump power coupling** — pump draw/availability through the power-load contract; `SumpFloodingSystem` keeps flood-state ownership.
5. **Bounded source build options** that already have research/item chains — deep well (`knowledge_deep_well_hydraulics` + `item_hydraulic_actuator`), condenser/desal (membrane chain), advanced filter install/replace (`item_water_filter_advanced`). Each is: built/unbuilt, enabled, bounded yield, power where appropriate, maintenance cost, deterministic output. No aquifer depletion, no topology.
6. **Brine/reject closure audit** — `BrineWaterSystem` already exists; verify a real consumer; itemize nothing new without one.
7. **Contamination → exposure handoffs** — unsafe water reaches survivors only as `DiseaseExposureContext` / dose-contract results.
8. **UI + save migration** for the above, additive and versioned.

## What B7 / Plan 66 DOES NOT OWN (reserved to Plan 189)

- Regional pipe network; source-routing graphs.
- World-map aquifer simulation; per-location hydrology evolution.
- A second/general contamination map beyond the existing incoming-contamination model.
- Large-scale distribution infrastructure.
- Any new global "water units" abstraction over the existing mass-conserving reservoir + inventory + pools.

## Seam exposed for Plan 189 (and no more)

B7 leaves these seams, typed and stable, for Plan 189 to consume:

- **Source identity**: any bounded build (well/condenser) is addressable by a stable source ID and reports deterministic yield/quality — Plan 189 may add more sources and routing without changing the request shape.
- **Request shape**: consumers already go through authoritative preview/commit paths (`CommandPreview`/`CommandResult` family or `WaterAuthority` draw/pour); Plan 189 can interpose source routing behind the same requests.
- **Incoming contamination channel**: `SetIncomingContamination(...)` remains the single contamination inlet — Plan 189's network feeds it rather than writing treatment state directly.

## Anti-merge rules for future agents

1. Do not convert the 4 shelter pools into a node graph.
2. Do not add per-location hydrology inside `WaterTreatmentSystem`/`WaterAuthority`.
3. Do not duplicate `BrineWaterSystem`.
4. If Plan 189 work is tempting during B7, stop and record it in the completion report §I (Deferred scope) instead.
