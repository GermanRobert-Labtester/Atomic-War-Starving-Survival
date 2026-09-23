# PLAN-INVENTORY-CONSERVATION-93 — Appendix A: Implementation Scaffold

**Generated:** 2026-09-21 by the session scaffold generator (paired variant, batch 3).
**Scaffolding authority:** [`PLAN-CRAFT-QUALITY-TRUTH-112`](../EXPANSION_PROGRAM_WAVE9_2026-09-21/PLAN-CRAFT-QUALITY-TRUTH-112.md) — adopt the authority's patterns rather than inventing a second architecture. **Convention authority:** `PLAN-ORPHAN-SEAL-01` (generated, reproducible appendices).
**Status:** scaffolding only — no production file is created by this appendix.

## 1. Advisory patterns from the scaffolding authority

| Authority package | Pattern to adopt |
|---|---|
| `CQT-112A` | quality model doc + deterministic roll rules (seed from the owning stream). |
| `CQT-112B` | marking on the item instance + stack behavior tests. |
| `CQT-112C` | consumer effect table + one test per consumer named. |

## 2. Source inventory (34 files, Core + host)

| File | Lines |
|---|---:|
| `Economy/BiologicalTradeItem.cs` | 23 |
| `Economy/BlackMarketInventoryCatalog.cs` | 358 |
| `HoldfastItemsCatalog.cs` | 71 |
| `Inventory/ClothingWarmthSystem.cs` | 390 |
| `Inventory/DeviceState.cs` | 136 |
| `Inventory/EquipLimbGate.cs` | 104 |
| `Inventory/IEquipmentConditionSink.cs` | 19 |
| `Inventory/IPlayerInventoryPort.cs` | 24 |
| `Inventory/Inventory.cs` | 1353 |
| `Inventory/InventoryMigrator.cs` | 61 |
| `Inventory/InventoryProvenance.cs` | 44 |
| `Inventory/InventoryTransaction.cs` | 388 |
| `Inventory/ItemAliases.cs` | 83 |
| `Inventory/ItemCatalogLoader.cs` | 783 |
| `Inventory/ItemDefinitions.cs` | 342 |
| `Inventory/ItemDescriptionCatalog.cs` | 129 |
| `Inventory/ItemDescriptionCatalogLoader.cs` | 117 |
| `Inventory/ItemDescriptionEntry.cs` | 129 |
| `Inventory/ItemInspectionModel.cs` | 153 |
| `Inventory/ItemLoreSystem.cs` | 308 |
| `Inventory/ItemTagCatalog.cs` | 35 |
| `Inventory/ItemTypes.cs` | 56 |
| `Inventory/ProceduralItemInstance.cs` | 159 |
| `Inventory/StartingSuppliesCatalog.cs` | 135 |
| `Survivors/NeedsModifierStack.cs` | 239 |
| `UI/ModalStackController.cs` | 160 |
| `UniqueItemClaimRegistry.cs` | 122 |
| `host:Host/InventoryHostSession.cs` | 722 |
| `host:Host/InventorySaveSelfTest.cs` | 43 |
| `host:Host/InventorySaveStore.cs` | 51 |
| `host:Main.Inventory.cs` | 264 |
| `host:Main.UiTests.Inventory.cs` | 114 |
| `host:UI/InventoryDetailPanel.cs` | 289 |
| `host:UI/InventoryPanel.cs` | 283 |

## 3. Data bindings

| Catalog | Records/shape |
|---|---|
| `black_flotilla_items.json` | 36 |
| `chemical_dependency_items.json` | 13 |
| `foundry_items.json` | 30 |
| `holdfast_items.json` | 55 |
| `item_degradation.json` | object(2 keys) |
| `agriculture_items.json` | 2 |

## 4. Host attachment

- Candidate host partials: `Main.UiTests.Inventory.cs`, `Main.Inventory.cs`
- Proposed method names: `SetupInventoryICScaffold` / `SaveInventoryICScaffold` (Plan 1 Appendix AI: collision-free)
- Loop/day coupling: consult Plan 1 Appendix AA before adding any step method.

## 5. Test scaffold (proposed, not created)

Proposed file: `Ashfall.Core.Tests/Inventory/IC93ScaffoldTests.cs`

```csharp
// One fixture per package, plus one authority-conformance fixture.
public class IC93ScaffoldTests
{
    [Fact] public void IC93A_TODO() { /* conservation model doc + owner table (no code change). */ }
    [Fact] public void IC93B_TODO() { /* audit wrapper: counts by id around an operation; opt-in in tests/dev only. */ }
    [Fact] public void IC93C_TODO() { /* reload probes: stack/partial/container cycles comparing counts and checksums. */ }
    [Fact] public void IC93D_TODO() { /* edge-case tests: capacity, partial, dead-owner, nested container, remainder. */ }
    [Fact] public void IC93E_TODO() { /* report: any discovered duplication path filed as a finding for its owner (repair */ }
    [Fact] public void IC93_AuthorityConformance_TODO() { /* pattern parity with PLAN-CRAFT-QUALITY-TRUTH-112 */ }
}}
```

## 6. Commands

- `bash scripts/run_test.sh Ashfall.Core.Tests/Inventory/`
- Regenerate, do not edit (Plan 1 Appendices AJ/AM).
- `python3 scripts/ci/generate-docs-index.py --check` if docs change.

## 7. Scaffolding rules

1. No production file before a claim.
2. The authority's patterns win; a deviation is recorded with a reason.
3. Every fixture maps to a package; a package without a fixture is a finding.
