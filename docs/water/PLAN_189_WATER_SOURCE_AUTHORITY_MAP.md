# Plan 189 — Water source / intake authority map

**Status:** ACCEPTED — §3 signed 2026-09-12  
**Package:** `DEBT-189-WATER-SOURCE-AUTHORITY-MAP`  
**Batch:** `BATCH-2026-09-12-DEBT-189-WATER-SOURCE-MAP`  
**Rebase source:** Plan 189 row in `docs/foreman/PARTIAL_IMPLEMENTATION_REBASE.md`  
**Date:** 2026-09-12  
**Unblock:** `DEBT-PLAN168-WATER-DELIVERY` is RETIRED. This family is no longer blocked on fluid delivery.

**Related (not this package):**

- Sealed delivery: `docs/water/PLAN_168_WATER_DELIVERY_AUTHORITY_MAP.md` — WT liters, Fluid network, WT→Fluid transfer, greenhouse/disease sinks.  
- Piezometer intelligence: `Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs` (Plan 115) — never grants, moves, or purifies water.  
- Historical proposal (stale premise): `Next-steps-plans/Plan_189_Water_Source_Management_Contamination_Network.md` — proposes a new `WaterSourceSystem` + `water_sources.json` + pipes/pumps/tanks. **Do not treat as current evidence.**

---

## 1. Premise (current evidence)

### 1.1 Bulk water and pipes already have owners

| Concern | Owner | Path | Save |
|---|---|---|---|
| Spendable bulk liters + `WaterType` | `WaterTreatmentSystem` | `Assets/Ashfall.Core/WaterTreatmentSystem.cs` | WT host save |
| In-network transit liters, pipes, pumps, reservoirs, mixing | `FluidLogisticsSystem` | `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs` | `fluid_logistics` |
| Atomic WT → Fluid handoff | `FluidWaterTreatmentBridge.TransferTreatedWater` | `Assets/Ashfall.Core/Shelter/FluidWaterTreatmentBridge.cs` | none (ephemeral transfer) |
| Packaged bottles | Inventory | `clean_water` items | inventory |

Plan 168 no-double-ledger rule still holds: liters leave WT exactly once into Fluid. A Plan 189 `WaterSourceSystem` that also stores “total available water” would be a third bulk ledger.

### 1.2 Abstract source identity already exists (intelligence, not liters)

`piezometer_network_catalog.json` strata already name intake IDs:

| Strata | `monitored_source_ids` |
|---|---|
| `strata_shallow_gravel` | `source_shallow_well`, `source_cistern_inlet` |
| `strata_sandstone_mid` | `source_mid_borehole` |
| `strata_deep_fractured_bedrock` | `source_deep_borehole` |

`AquiferPiezometerEngine` forecasts drawdown/contamination against those IDs and builds `HydrogeologyAdvisory.contaminated_source_ids`. It **must not** `AddWater`.

`WaterTreatmentSystem` already has the intake gate, unused in production:

- `RegisterContaminationAdvisory(...)`
- `IsIntakeSourceBlocked(sourceId)`

Grep finds **no production caller** of either method outside WT itself. Piezometer comments name the host/test bridge; that bridge is not wired.

### 1.3 Current liter producers (ungated by source id)

| Producer | Call | Type granted |
|---|---|---|
| `SumpFloodingSystem` | `WaterTreatmentSystem.AddWater(Raw, greywaterL)` | Raw |
| `SolarConcentratorEngine` | `AddWater(Clean, cleanYield)` | Clean |
| Host CLI / tests / mass-balance sim | `AddWater` | various |

`AddWater` takes only `WaterType` + amount. It does **not** take a source id, so piezometer isolation cannot currently refuse an intake.

### 1.4 Adjacent, not bulk-water, authorities

| System | Role | Must not become |
|---|---|---|
| `HydroGeologyDiscoverySystem` + well-contamination narrative catalog | Science/archive prose (`artesian_well_contamination_logs.json`) | Liter ledger or live well sim |
| Micro-location `micro_water_source` | Expedition collect/test/deplete encounter | Shelter bulk tanks |
| `LocationEvolutionSystem` contamination | Location-level world stain | Per-well flow network |
| `SumpFloodingSystem` | Shelter flood → Raw WT | External well graph |

### 1.5 Historical Plan 189 would duplicate three owners

The historical plan asks for `WaterSource` flow/contamination, `WaterSourceConnection` propagation, and `WaterInfrastructure` pipes/pumps/tanks. Those concerns are already split:

- liters → WT  
- pipes/pumps/reservoirs/mixing → Fluid  
- source IDs + contamination forecast → piezometer  

A new `water_sources.json` ledger is **OUT**.

---

## 2. Ownership table (proposed)

| Concern | Authority | Boundary |
|---|---|---|
| Spendable bulk liters | **`WaterTreatmentSystem`** | Sole tank owner. No source DTO stores liters. |
| Shelter pipe/pump/reservoir topology | **`FluidLogisticsSystem`** | Plan 168 remains the only infrastructure network. |
| Abstract intake identity | **`piezometer_network_catalog.json` `monitored_source_ids`** | Canonical source IDs. Do not invent a parallel `water_sources.json` id space. |
| Contamination / drawdown intelligence | **`AquiferPiezometerEngine`** | Forecasts + isolation zones. Forwards advisories; never `AddWater`. |
| Intake admission | **`WaterTreatmentSystem.IsIntakeSourceBlocked`** | Producers must pass source id; blocked source grants zero liters. |
| Advisory handoff | Host/test bridge: piezometer `BuildAdvisory` → `RegisterContaminationAdvisory` | Primitive seam already documented on piezometer; currently unwired. |
| Liter grants from the world | Named **intake producers** calling `AddWater` | Sump, solar, and any well/cistern/rain producer. Each names a source id from the piezometer catalog (or a documented synthetic id for non-aquifer producers). |
| Discovery / lore of wells | Hydrogeology archive + expeditions + micro-locations | Presence in a log is not a bulk-water grant. |
| Disease / greenhouse | Existing Plan 168 sinks | Unchanged. |

---

## 3. Recommended defaults (sign these)

### 3.1 Modality table

| Proposal from historical Plan 189 | Default | Reason |
|---|---|---|
| New `WaterSourceSystem` + `water_sources.json` bulk ledger | **OUT** | Third liter store; violates Plan 168 no-double-ledger. |
| Source-to-source contamination graph / connections | **OUT** | Piezometer already forecasts transport per strata; Fluid already mixes in-network. |
| Duplicate pipes/pumps/tanks on the source DTO | **OUT** | `FluidLogisticsSystem` is the infrastructure owner. |
| Weather-driven rain collector as a Core weather copy | **OUT** | Weather stays on its owner; a rain producer may *read* a host-fed mm/day fact later. |
| Piezometer `monitored_source_ids` as canonical intake IDs | **IN** | Already authored; four live IDs. |
| Wire `BuildAdvisory` → `RegisterContaminationAdvisory` | **IN** (implement package after this map) | API exists; unused. |
| Source-gated `AddWater` (source id + type + amount) | **IN** (implement package) | Makes isolation real. Existing untagged `AddWater` remains for tests/legacy producers until they name a source. |
| Sump / solar remain producers | **IN** | Keep; assign documented source ids (`source_sump_greywater`, `source_solar_concentrator`) **or** leave them untagged (always admitted). Recommend: documented synthetic ids, not piezometer strata. |
| Player “switch active well” as a second network | **OUT** | Isolation zones + blocked intake IDs are the switch. |
| Micro-location water collect → WT tanks | **OUT** (this map) | Expedition packaging stays inventory; do not dump encounter water into shelter tanks without a later named package. |

### 3.2 Refresh / cadence (if Path α of 189 is signed)

Daily host order already: piezometer tick (if installed) → WT → Fluid transfer/solve. Add:

1. Piezometer `BuildAdvisory`  
2. WT `RegisterContaminationAdvisory`  
3. Intake producers `AddWater` (skip when `IsIntakeSourceBlocked`)  
4. Existing Plan 168 WT→Fluid→sinks  

No new save section for “sources.” Piezometer and WT already persist.

### 3.3 Honesty rules

- Catalog source IDs without a producer still grant **zero** liters. Presence ≠ flow.  
- Advisory without a producer is still valid intelligence.  
- Do not claim “water source management” in UI until at least one piezometer-catalog source has a producer.

### 3.4 Non-goals

- Unified durability/maintenance ledger (Plan 186 family).  
- Human/faction migration (Plan 199).  
- Weather simulation inside WT.  
- Restoring quarantined `AquiferPiezometerEngineTests` in this map package.

---

## 4. Next implement package (after sign-off; separate claim)

**Suggested id:** `DEBT-189-INTAKE-ADVISORY-BRIDGE`

Exact seams:

- Host: piezometer `BuildAdvisory` → WT `RegisterContaminationAdvisory`  
- Core: source-id overload or required argument on the intake path (`TryAddWaterFromSource(sourceId, type, amount)` wrapping `AddWater` + `IsIntakeSourceBlocked`)  
- One producer proof: either gate an existing producer with a documented id, or add a **thin** well/cistern grant that reads piezometer catalog IDs and a host-fed yield — **no new liter field on piezometer**  
- Focused tests: blocked source grants 0; unblocked source increases WT raw/clean; advisory round-trip; Theme/Fluid/WT tank totals unchanged in meaning  

Must not add `WaterSourceSystem`, `water_sources.json`, or Fluid topology rows.

---

## 5. Exact paths (this map package)

| Path | Role |
|---|---|
| `docs/water/PLAN_189_WATER_SOURCE_AUTHORITY_MAP.md` | This contract |
| `KNOWN_DEBT.md` | `DEBT-189-WATER-SOURCE-AUTHORITY-MAP` |
| `INTEGRATION_PLANS.md` | Active batch row |
| `WORKTREE_OWNERSHIP.md` | `claim-debt-189-water-source-map-2026-09-12` |

**Read-only evidence (not claimed for edit):**

- `Assets/Ashfall.Core/WaterTreatmentSystem.cs`  
- `Assets/Ashfall.Core/Shelter/FluidLogisticsSystem.cs`  
- `Assets/Ashfall.Core/Shelter/FluidWaterTreatmentBridge.cs`  
- `Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs`  
- `Assets/StreamingAssets/Data/piezometer_network_catalog.json`  
- `docs/water/PLAN_168_WATER_DELIVERY_AUTHORITY_MAP.md`  
- `Next-steps-plans/Plan_189_Water_Source_Management_Contamination_Network.md`  
- `SumpFloodingSystem.cs`, `SolarConcentratorEngine.cs`

---

## 6. Remaining 170–199 PARTIAL families (not this package)

After 189, each family still needs its own signed map before builders:

| Family | Missing contract (rebase) |
|---|---|
| 176 / 183 aging + child development | Live age chronology; do not encode children as starting cohorts |
| 177 / 179 dreams + unified psychology | Per-survivor psych record + narrative-event input |
| 178 / 190 culture creation + item lore | Creator/artifact identity vs archive vs provenance |
| 180 / 185 / 195 certification, knowledge decay, specialization | Derived vs persisted capability; duty projection |
| 182 relationship drift | Real interaction producers before decay |
| 186 shelter maintenance | Cross-owner inspection projection; no unified HP ledger |
| 188 daily routines | Sub-day clock vs schedule extension |
| 192 / 199 trade routes + human migration | Player route + seasonal population; never wildlife migration |
| 193 / 198 chronic conditions + medical history | One medical-record owner |
| 194 emergency alerts | Typed producer inventory + ack/persist policy |
| 175 meta / 181 difficulty | BLOCKED / UNSTARTED — product authority, not this map |

---

## 7. Sign-off checklist

- [x] §2 ownership table  
- [x] §3.1 OUT: new `WaterSourceSystem` / `water_sources.json` / duplicate pipes / source graph  
- [x] §3.1 IN: piezometer source IDs + advisory bridge + source-gated AddWater  
- [x] §3.2 cadence after Plan 168 daily order  
- [x] §3.3 honesty (catalog ID ≠ liters)  
- [x] §4 implement package scoped  

**Signed 2026-09-12.** Next implement package: `DEBT-189-INTAKE-ADVISORY-BRIDGE` (claim separately). Other 170–199 PARTIAL families still need their own maps.

**Signed 2026-09-12.** Next implement package: `DEBT-189-INTAKE-ADVISORY-BRIDGE` (claim separately). Other 170–199 PARTIAL families still need their own maps.
