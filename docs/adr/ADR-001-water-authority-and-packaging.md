# ADR-001: Single Physical Water Authority and Explicit Packaging Conversion

- **Status:** Accepted
- **Date:** 2026-09-04
- **Context:** Ticket REM-008 (Finding R14 from Wave 5 Audit)
- **Deciders:** ASHFALL Core Architecture Team

---

## 1. Context and Problem Statement

A forensic audit of the ASHFALL survival loop revealed that water existed under two parallel, disconnected authorities:
1. **Bulk Liquid Reservoir (Litres):** Tracked in `WaterTreatmentState` (`cleanWater`, `rawWater`, `brackishWater`, `irradiatedWater`) managed by `WaterTreatmentSystem`.
2. **Packaged Items (Counts):** Tracked in `Inventory` (`clean_water`, `dirty_water`, `irradiated_water`) seeded in starting supplies and scavenging loot.

This dual authority caused several critical defects:
- `WaterTreatmentHostSession` held a nullable reference to `InventoryHostSession?`, allowing the water plant to run disconnected from inventory storage.
- `WaterTreatmentSystem.ConsumeRation(float needed)` had zero non-test callers across the entire codebase.
- Daily survival rationing (`StartingLevelRationsDayOwner`) deducted items from `Inventory` directly without interacting with or checking `WaterTreatmentSystem`, making bulk water reservoirs functionally invisible to the thirst survival loop.
- No formal conversion existed between bulk fluid in tanks and bottled water in inventory, allowing duplicate accounting or phantom water creation/loss.

---

## 2. Decision Drivers

- **Invariant 5 (No gameplay logic in hosts):** Gameplay mechanics, conversions, and consumption transactions must live in `Ashfall.Core`.
- **Conservation of Mass:** Water must never be created from nothing or silently vanish. Any conversion between bulk and packaged formats must be an exact, conserving transaction.
- **Single Source of Truth:** `WaterTreatmentSystem` is the authority on water volumes, purification state, and fluid treatment.
- **Fail-Safe Sinks:** Output delivery must honor storage and weight constraints using `IOutputSink`.

---

## 3. Considered Options

1. **Keep Independent Parallel Quantities:** Inventory holds "bottles", plant holds "tanks", but they never convert or share state. *(Rejected: Thirst and survival decisions become split across two uncoupled systems; player can die of thirst while the water plant is full of clean water).*
2. **Delete Packaged Water Items Entirely:** Remove water items from inventory and treat all water as an abstract scalar in the shelter. *(Rejected: Water bottles are essential trade goods, expedition supplies, scavenged loot, and quest deliverables across 200+ data files).*
3. **Single Physical Water Authority with Explicit Packaging Conversion:** Designate `WaterTreatmentSystem` as the single physical water authority, treat inventory water items as packaged 1.0-litre units, and introduce explicit atomic conversion verbs (`DrawWater` and `PourWater`) alongside unified daily ration consumption. *(Chosen)*

---

## 4. Decision

We designate `WaterTreatmentSystem` as the **Single Physical Water Authority** for the shelter, and define the following contracts:

### 4.1. Packaging Unit Standard
- Packaged inventory items represent sealed 1.0-litre containers:
  - `clean_water` = 1.0 Litre of potable clean water.
  - `dirty_water` = 1.0 Litre of untreated raw/brackish water.
  - `irradiated_water` = 1.0 Litre of radioactive fallout water.

### 4.2. Explicit Conversion Operations
- **`DrawWater(WaterType type, int units, IOutputSink sink, int day)`**:
  - Validates that the plant reservoir has at least `units` of the specified `WaterType`.
  - Packages the fluid into the corresponding inventory item via `IOutputSink.Deliver(DeliveryBill)`.
  - If delivery fails (e.g. inventory storage full or weight exceeded), the reservoir is **not** decremented.
  - If delivery succeeds (or partially succeeds), the reservoir is decremented by exactly `result.DeliveredQuantity`.
  - Mass is conserved: $-\Delta L_{\text{plant}} + \Delta \text{items}_{\text{inv}} = 0$.

- **`PourWater(WaterType type, int units, Inventory.Inventory inventory)`**:
  - Validates that inventory contains at least `units` of the matching item ID.
  - Atomically removes `units` from inventory and adds `units` to `WaterTreatmentSystem.AddWater(type, units)`.
  - Mass is conserved: $-\Delta \text{items}_{\text{inv}} + \Delta L_{\text{plant}} = 0$.

### 4.3. Non-Null Host Dependency
- `WaterTreatmentHostSession` strictly requires a non-null `InventoryHostSession`. In constructors, it defaults to a valid local session if none is passed, guaranteeing that `InventoryHost` is never null.
- `Main.ShelterInfrastructure.cs` binds `_inventory` to `_waterTreatment` during composition.

### 4.4. Unified Ration Consumption
- `ConsumeRation(float needed, Inventory.Inventory? inventory = null, bool forceIrradiated = false)` is wired as the single canonical consumption transaction.
- When daily rationing runs (`StartingLevelRationsDayOwner`), it invokes `ConsumeRation`, which:
  1. Consumes clean water from the treatment plant bulk reservoir first.
  2. If additional clean water is needed, consumes packaged `clean_water` bottles from inventory.
  3. If clean water is exhausted, falls back to raw, brackish, or irradiated reserves/bottles according to ration policy, emitting exposure events into `DiseaseSystem` and `DoseLedgerSystem`.
- Emits `consumed_rations` day state change events.

---

## 5. Consequences

- **Positive:** Thirst satisfaction and water management are completely unified. Players can purify flood/scavenged water, bottle it for expeditions, or pour found bottles into the shelter purification system.
- **Positive:** Mass conservation is mathematically enforced and tested across 200-day simulation sweeps.
- **Neutral:** Host sessions must pass their inventory dependency at setup time.
