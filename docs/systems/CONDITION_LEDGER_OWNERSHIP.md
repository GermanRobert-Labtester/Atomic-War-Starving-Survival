# CONDITION LEDGER OWNERSHIP — C2 / Plan 21B Phase A Matrix

> Evidence-first inventory (2026-09-15), recorded before any 21B migration
> decision. Every row was verified in current source. This document is the
> plan §18/§29 deliverable: one number per thing, one owner per number, one
> save path per number.

## 1. Wear-ownership matrix

| # | Family | Authority value | Owner (write authority) | Save path | Mutation API(s) | Readers | Classification |
|---|---|---|---|---|---|---|---|
| 1 | Protective gear (rad-protection equippables: gas masks, hazmat suits, …) | `EquippedItem.CurrentDurability` (0..`Item.durability`) | **`Inventory`** | inventory save `EquippedSave.durability` (`Inventory.cs:1073/1155`) | `Inventory.RecordWear(item, delta, cause)` — **the one API** since 21A; radiation via `WornGear.Degrade`→sink; bulk via `DegradeEquippedGear`→`RecordWear` | `FillWornGear` projection → RadiationSystem; `GetTotalRadProtection`; remaining-life estimate; RadiationDetailPanel | `ITEM_INSTANCE` |
| 2 | Weapons / tools / watercraft / medical / electronics / container instances | `EquipmentInstance.condition` (0–100) | **`EquipmentConditionSystem`** | `equipment_condition` save section | `UseItem(instanceId, …)` (combat via `WeaponEquipmentBridge` write-back), `StartMaintenance` | combat (`WeaponEquipmentBridge` — stateless projection), expedition estimate (`weaponReadiness`/`weaponJamRisk`), EquipmentConditionPanel | `ITEM_INSTANCE` (registration is **explicit** — no production auto-registration) |
| 3 | Vehicles | fuel + condition + `isBrokenDown` | `VehicleGarageSystem` | vehicle save | travel/breakdown paths in the garage system | expedition estimate (`breakdownRiskPerTick`), ExpeditionPanel display | `VEHICLE_INSTANCE` — remains vehicle-owned (plan §19.2: do not force into an item ledger) |
| 4 | Shelter fixed plant (pipes, machinery, ceilings, sump, ventilation…) | structurally-owned condition per system (e.g. `MaterialShieldingSystem` ceiling materials, `VentilationSystem.ductIntegrity`, `SumpFloodingSystem.pumpCondition`) | each shelter system | each shelter save section | each system's own maintenance paths | shelter UI, 20B shielding model (read-only providers) | `STRUCTURAL` — **disjoint by design** (plan §3.2) |

## 2. Key findings (premise corrections)

- **No live duplicate exists.** `EquipmentConditionSystem.RegisterItem` is called only
  from explicit flows (UI-test journey, panel-driven registration). No production path
  auto-registers inventory items — so a protective item cannot currently be tracked in
  both stores. The plan's duplicate-condition premise described an older or potential
  state; the 21B "migration" is therefore **not required in current source**.
- **Weapons are already canonical.** `WeaponEquipmentBridge` owns no state; it projects
  `EquipmentConditionSystem` into combat tokens and writes combat wear back through
  `UseItem` (21B Phase D was already done). Do not re-migrate.
- **Protective gear has one write API** since 21A: `Inventory.RecordWear` with cause
  attribution and the exactly-once `OnProtectiveGearFailed` transition. The `WornGear`
  projection is read-only (source of truth contract documented on `FillWornGear`).
- **The dual-namespace `WornGear` premise is stale** — one class, one using-alias.

## 3. Cause attribution (plan §3.5)

| Store | Cause carrier |
|---|---|
| Inventory protective gear | `RecordWear(…, cause)` string (`"radiation"`, `"use"`, …) + `OnProtectiveGearFailed(item, cause)` |
| EquipmentConditionSystem | `WearEvent.source` + typed family profiles; `OnItemJammed/OnItemBroken/OnItemRepaired` transitions |
| Vehicles / shelter | each system's own incident/cause records |

## 4. Repair paths (plan §25/§48)

| Store | Repair today | Status |
|---|---|---|
| ECS families | `StartMaintenance` (station + `MaintenanceType` + material list, e.g. `scrap_mechanical`; jam/break thresholds; `max_durability_loss_per_repair` authored in profiles) | data-driven, working |
| Inventory protective gear | **Replacement crafting only** — authored recipes `craft_hazmat_patch` (cloth+scrap+water_filter → new hazmat_suit), `craft_gas_mask`, through the canonical recipe/bill path | restore-style repair of an equipped item (bill → `RecordWear` restoration) is **absent** — decision item D2 below |
| Vehicles / shelter | their own maintenance paths | disjoint, unchanged |

**D2 (foreman decision):** 21A/21B deliberately did not invent an equipped-item
restore-repair API: replacement crafting already provides the authored resource path,
and plan §27.2 wants replacement to stay a real decision. If restore-repair is wanted,
it must go through the shared recipe/bill path (`TryConsumeBill` semantics, Plan 22
coordination) and mutate the same `EquippedItem.CurrentDurability` — never a parallel
value. No temporary gear-only bill model was created (plan §49).

**D1 (recorded earlier):** authored `degradeRate` overrides Core family defaults
(Face 1.0 / Body 0.5 / other 0.25 per hour); defaults are documented domain fallbacks.
Full data authoring of every protective item's rate remains an optional content tranche.

## 5. Save-migration conclusion (plan §23 — highest-risk phase)

**No migration required:** no persisted duplicate condition value exists for any item
instance in current source (verified: registration is explicit; weapons live only in
ECS; protective gear only in Inventory). The 21B fixtures of plan §46 therefore apply
to *future* divergences, guarded by:

- `Plan21ProtectiveWearTests` pins: save round-trip preserves worn durability;
  projection writes have no authority; one mutation API; exactly-once failure.
- The boundary rule above: protective gear must never be registered into ECS (and ECS
  families must not be re-registered as equipped inventory duplicates). If a future
  feature needs cross-store visibility, it must read — not copy — the other authority.

## 6. Performance notes (plan §53)

- Protective-gear projection is a reused cleared buffer (21A P4) — no per-tick allocation.
- `FillWornGear` iterates the equipped list (bounded by equip slots) — O(slots).
- Remaining-life estimate iterates equipped items once per UI refresh — not per tick.

## 7. Remaining 21C scope

P6 (plan §33): extend `ExpeditionSystem.Estimate` with party protection inputs —
best/median protection, no-working-mask count, projected dose, projected gear wear,
mid-route failure prediction — consumed from the canonical authorities above
(Inventory projection + ECS weapons already feed `weaponReadiness`). Warn-don't-block
dispatch semantics per plan §34/§35.
