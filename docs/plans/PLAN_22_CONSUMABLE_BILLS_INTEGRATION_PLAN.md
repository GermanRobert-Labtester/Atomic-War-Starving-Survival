# PLAN 22 — Consumable Bills, Replacement Canisters, Patch Kits, Medicine, and Shared Item-Tag Consumption Semantics — INTEGRATION PLAN

> Scaffolded 2026-09-15 via evidence-first recon (ashfall-plan workflow), after Plan 21
> landed (`docs/plans/C2_PLANINTEGRATION_5_BASELINE.md` §6–§8; ownership matrix:
> `docs/systems/CONDITION_LEDGER_OWNERSHIP.md`). Every claim below was verified in
> current source. Companion plans: Plan 21 (landed — D1/D2 decisions pre-seed this),
> Plan 24 (labour — not landed; seam only).

---

# 1. Objective

One consumption semantics layer for consumable bills (repair parts, replacement
canisters, patch kits, medicine): items are consumed atomically through the canonical
bill path, identified by shared item tags/types instead of hardcoded ID lists, priced
by the real economy, and wired to the protective-gear wear authority landed in Plan 21
(replace/repair decisions on `EquippedItem.CurrentDurability`).

# 2. Current Reality (verified)

| Piece | Status | Evidence |
|---|---|---|
| Atomic multi-item bill consumption | **Exists** — `Inventory.TryConsumeBill` (3 overloads; exactly-once commit; `OnInventoryChanged` fired once; `InventoryBill.FromCosts`) | `Inventory.cs:672-691` |
| Repair bills for ECS families | **Exists** — `EquipmentConditionSystem.StartMaintenance` consumes `requiredParts` via `TryConsumeBill` (station + `MaintenanceType` + parts) | `EquipmentConditionSystem.cs:395` |
| Shelter canister maintenance bills | **Exists** — `StartingLevelSystem` air-filter service/replace consumes via `TryConsumeBill` (`scrap_mechanical` / `item_air_filter_hepa`) | `StartingLevelSystem.cs:99` |
| Canisters as trade goods | **Exists** — `air_filter`, `water_filter` authored in `economy_goods.json` with barter notes | `economy_goods.json:194-206` |
| Replacement crafting for protective gear | **Exists** — `craft_hazmat_patch` (cloth+scrap+water_filter → new hazmat_suit), `craft_gas_mask` in `recipes.json` | `recipes.json:123-145` |
| Medicine bills | **Partial** — `MedicalTreatmentCatalog` treatments carry `ItemCosts` dicts, but item IDs are hardcoded C# constants (`ItemBandage = "bandage"`) | `MedicalTreatmentCatalog.cs:102,170` |
| Medical craft classification | **Hardcoded list** — `CraftingSystem.IsMedicalCraftResult` checks `ItemType.Medical` then a literal ID list (`bandage`, `morphine`, `anti_rad`, `rad_away`, `antibiotics`, `iodine_pills`) | `CraftingSystem.cs:354-363` |
| Item tags | **Dead data** — `tags` authored in `items.json` (both schema generations) but **not parsed** by `ItemCatalogLoader` (0 references) | `ItemCatalogLoader.cs` |
| ID aliasing | **Exists** — `ItemAliases` maps `item_bandage → bandage` etc. | `ItemAliases.cs:25` |
| Protective-gear restore-repair | **Absent** — only replacement crafting (Plan 21 decision D2); no bill→`RecordWear` restoration | ownership matrix §4 |
| Canonical protective wear API | **Exists** — `Inventory.RecordWear(item, delta, cause)` + exactly-once `OnProtectiveGearFailed` | Plan 21A |

# 3. Required Delta

1. **Tags become real:** parse `tags` in `ItemCatalogLoader` → `ItemDefinition.Tags`
   (additive; tolerant of both schema generations; absent tags = empty set).
2. **One classification authority:** medical/protective/canister category membership
   reads tags (with `ItemAliases` normalization), replacing the hardcoded ID list in
   `CraftingSystem.IsMedicalCraftResult` and the const-ID pattern where a tag test works.
3. **Protective-gear restore-repair (D2):** data-authored repair bills that restore
   `EquippedItem.CurrentDurability` through `RecordWear(item, −restore, "repair")` —
   consumed via `TryConsumeBill`; bounded by authored `max_repair_fraction` /
   irreparable threshold (Plan 21 §27.1 `beyond_repair` semantics).
4. **Canister economy closure:** filter replacement (shelter + gear) consumes the same
   `air_filter` / `item_air_filter_hepa` items trade and crafting see — verify, pin with
   tests; no new pricing model.
5. **Medicine:** treatment `ItemCosts` stay data-side; hardcoded const lists migrate to
   tags where behavior-identical.

# 4. Existing Extension Seams (collision check — nothing new gets built)

- Bills: `TryConsumeBill` — **extend consumers, never fork** (plan invariant).
- Repair: ECS `StartMaintenance` for its families; protective gear gets the narrow
  `RepairEquippedGear` additive API on `Inventory` (D2) — not a new system.
- Tags: loader DTO + `ItemDefinition` — additive fields only.
- Aliases: `ItemAliases` — reuse for tag/id normalization.
- Economy: `economy_goods.json` + `MarketSystem` canonical pricing — no gear-shop model.

# 5. Ownership Matrix

| Concern | Owner |
|---|---|
| Tag parsing + `ItemDefinition.Tags` | Core `ItemCatalogLoader` |
| Category classification (medical/protective/canister) | Core — tag predicate w/ alias normalization (single static authority) |
| Bill consumption | `Inventory.TryConsumeBill` (unchanged) |
| Protective-gear repair mutation | `Inventory.RecordWear` (unchanged; new caller) |
| Repair bill data (what restores what, caps) | `repair_bills.json` (new) or `recipes.json` extension — decided in Phase 3 |
| Canister/repair goods pricing | `economy_goods.json` + `MarketSystem` (unchanged) |
| Repair UX | Godot panel (existing inventory/workshop surfaces) |
| Validation | `CatalogIntegrityValidator` additive hooks + focused tests |

# 6. Data Flow

```text
items.json tags → ItemCatalogLoader → ItemDefinition.Tags
      → TagCatalog classification (medical/protective/canister)
repair command → bill data (repair_bills.json) → TryConsumeBill (atomic)
      → RecordWear(item, −restore, "repair") → OnInventoryChanged
      → durability ↑ → EffectiveProtection ↑ (Plan 21 chain)
trade/crafting → same item ids → same market/scarcity signals
```

# 7. API / Contracts (proposed, minimal)

```csharp
// ItemDefinitions.cs (additive)
public IReadOnlyList<string> Tags { get; }          // parsed, ordinal, lowercased
public bool HasTag(string tag);                     // alias-normalized

// New: Assets/Ashfall.Core/Inventory/ItemTagCatalog.cs (small static authority)
public static bool IsMedical(ItemDefinition def);   // tag "medical" OR ItemType.Medical
public static bool IsProtective(ItemDefinition def);   // tag "protective" OR radProtection>0 equipable
public static bool IsFilterCanister(ItemDefinition def); // tag "filter"

// Inventory.cs (additive — D2)
public bool TryRepairEquippedGear(EquippedItem item, float maxRestore, Func<IReadOnlyDictionary<string,int>> bill,
    out float restored);   // TryConsumeBill → RecordWear(item, -restored, "repair"); bounded; never above Item.durability
```

No new manager/service/system. Rejected: a "ConsumableService" wrapper (no second consumer set), tag hierarchy framework, gear-shop pricing.

# 8. Data Changes

- `items.json`: **no required changes** (tags already authored on many rows; Phase 3
  adds `"tags": ["filter"]` / `"protective"` to the few items that need classification
  if absent — surgical).
- New `repair_bills.json` (schema_version 1, snake_case):
  `{ id, target_item_id, restore_amount, max_repair_fraction, bill: {item: count} }`;
  validators: known item ids, restore > 0, fraction (0,1], non-negative bill, no
  duplicate target rows without explicit `variant_id`.
- Validation: `CatalogIntegrityValidator` additive hook (ids resolve; bills reference
  trade-visible goods where applicable).

# 9. Save/Load

- **No new save state.** Tags are derived from catalog; repair mutates the existing
  `EquippedItem.CurrentDurability` (already persisted); bills consume existing inventory
  items atomically. Old saves: unaffected (no schema change). Round-trip pinned by
  existing Plan 21 tests + one new repair round-trip test.

# 10. Determinism

No RNG anywhere in this plan. Bill consumption atomic + order-independent
(`TryExecuteTransaction` validates-then-commits). Tag classification is a pure function
of catalog data (ordinal, lowercased, alias-normalized).

# 11. Failure Modes

| Case | Behavior |
|---|---|
| Missing/unparseable tags | empty set → classification falls back to type/alias (behavior-identical for existing items) |
| Unknown id in repair bill | integrity error; repair command refuses honestly |
| Zero/over restore | clamped to `durability − CurrentDurability`, capped by `max_repair_fraction` |
| Failed item (durability 0) | repair allowed only if `max_repair_fraction ≥ 1`? — no: beyond-repair rule authored per bill/def (Plan 21 §27.1); default: failed ⇒ replace-only |
| Insufficient materials | `TryConsumeBill` false → no mutation, honest refusal |
| Duplicate repair rows | validator error |
| Old saves | no-op (no schema change) |
| Alias drift (`bandage` vs `item_bandage`) | normalize through `ItemAliases` before tag/id comparison |

# 12. Test Strategy

- **Core unit:** tag parse (both schema generations, absent tags), `HasTag` alias
  normalization, classification authority (medical/protective/canister), repair
  restore/clamp/cap/beyond-repair/insufficient, exactly-once `OnInventoryChanged`,
  failure-event non-regression (repair never fires `OnProtectiveGearFailed`).
- **Persistence:** repair → capture → restore → durability + storage parity.
- **Data:** integrity hook (ids, ranges, duplicates); every `repair_bills.json` bill
  references existing item ids; canister goods remain trade-visible.
- **Integration/source gates:** `CraftingSystem` medical classification no longer
  contains the literal ID list (tag authority consumed); `MedicalTreatmentCatalog`
  item ids resolve via tags/aliases; repair goes through `TryConsumeBill` + `RecordWear`
  (no `Remove`-loop forks).
- **Headless:** extend data-integrity selftest via the validator hook (no new verb).

# 13. Dependency-Ordered Phases

| Phase | Content | Files | Gate |
|---|---|---|---|
| 0 | Baseline pins (current behavior tests: medical craft classification, filter maintenance bills) | `Ashfall.Core.Tests/Inventory/Plan22*` | failing/legacy pins green |
| 1 | Tag parsing + `HasTag` | `ItemCatalogLoader.cs`, `ItemDefinitions.cs` | parse tests green; 0 catalog errors |
| 2 | Tag classification authority + hardcoded-list migration | new `ItemTagCatalog.cs`, `CraftingSystem.cs` (one method), `MedicalTreatmentCatalog.cs` (additive) | classification parity tests green (old lists ⇒ same outcomes) |
| 3 | `repair_bills.json` + validator hook + `TryRepairEquippedGear` | data + `CatalogIntegrityValidator.cs`, `Inventory.cs` | data-integrity PASS; repair tests green |
| 4 | Host wiring: repair action on inventory/workshop surface; canister parity pins | `src/UI/*`, `src/Main.*` (thin) | panel lifecycle + a11y PASS; source gates green |
| 5 | Trade parity verification (canisters/patch goods priced by market) | read-only + tests | economy suites green |
| 6 | Closeout: ownership doc update, balance note | docs | full battery green |

# 14. File Impact Map

| File | Action | Reason | Risk |
|---|---|---|---|
| `Assets/Ashfall.Core/Inventory/ItemCatalogLoader.cs` | MODIFY | parse tags (additive DTO field) | low |
| `Assets/Ashfall.Core/Inventory/ItemDefinitions.cs` | MODIFY | `Tags` + `HasTag` (additive) | low |
| `Assets/Ashfall.Core/Inventory/ItemTagCatalog.cs` | CREATE | one classification authority | low |
| `Assets/Ashfall.Core/Crafting/CraftingSystem.cs` | MODIFY | replace hardcoded medical list (one method) | low (parity-tested) |
| `Assets/Ashfall.Core/Medical/MedicalTreatmentCatalog.cs` | MODIFY | additive tag-based resolution | low |
| `Assets/Ashfall.Core/Inventory/Inventory.cs` | MODIFY | `TryRepairEquippedGear` (additive, D2) | low |
| `Assets/StreamingAssets/Data/repair_bills.json` | CREATE | authored repair bills | low |
| `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | MODIFY | additive hook | low |
| `src/UI/…` repair action surfaces | MODIFY | presentation only | medium (lifecycle gates) |

# 15. Out of Scope

- Plan 24 labour/duty-shift cost of repair (seam only — `repair_bills.json` may carry a
  `labour_hours` metadata field, unimplemented until Plan 24).
- New consumable item families or new trade rows beyond what classification needs.
- Reformatting items.json schema generations (both remain readable; unification is a
  separate hygiene package).
- `water_sample_contaminated` equipability quirk (Plan 21 foreman flag).
- Economy re-balancing of canister prices.

# 16. Rollback Strategy

Each phase is an independent revert: tag parsing (additive, inert without consumers) →
classification (parity-tested swap) → repair bills (new file + additive API, inert
without host binding) → host surfaces. No save state changes at any phase, so rollback
never touches persistence.

# 17. Definition of Done

- [ ] tags parsed on both item-schema generations; absent = empty
- [ ] one classification authority; hardcoded medical ID list retired (parity-proven)
- [ ] `repair_bills.json` authored + integrity-gated; bills atomic via `TryConsumeBill`
- [ ] protective-gear restore-repair mutates the canonical `EquippedItem` via `RecordWear`
      (repair never triggers the failure event; never exceeds authored caps)
- [ ] canisters: shelter + gear replacement consume trade-visible goods (pinned)
- [ ] no new system/service/panel beyond the listed surfaces
- [ ] all focused suites + data-integrity + panel lifecycle + a11y + triad green
- [ ] ownership doc updated; INDEX regenerated

# 18. Implementation Handoff

**MUST PRESERVE:** `TryConsumeBill` as the only bill path; `RecordWear` as the only
protective-gear mutation; ECS `StartMaintenance` for its families; both items.json
schema generations readable; old-save compatibility (no schema change); Plan 21
exactly-once failure semantics (repair must never fire `OnProtectiveGearFailed`).

**MUST ADD:** tag parsing + `HasTag`; `ItemTagCatalog` classification (alias-normalized);
`repair_bills.json` + validator hook; `Inventory.TryRepairEquippedGear`; parity tests
proving the retired hardcoded lists classify identically.

**MUST NOT DO:** fork a second consumption path; price repairs outside the economy;
create a ConsumableService/manager class; bundle items.json reformatting; auto-repair
or auto-consume anything from ticks (bills are player-action-driven); touch Plan 24
labour beyond a metadata field.

**VERIFY WITH:**
```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/
bash scripts/run_test.sh Ashfall.Core.Tests/Crafting/   # classification parity
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --panel-bind-lifecycle-selftest
bash scripts/ci/triad-drift-gate.sh
```

**FIRST SAFE IMPLEMENTATION STEP:** Phase 0 baseline pins — tests asserting
`IsMedicalRecipe` classifies the current six hardcoded ids as medical and that
StartingLevel air-filter maintenance consumes via `TryConsumeBill`; then Phase 1 tag
parsing with the same outcomes (behavior-identical swap).
