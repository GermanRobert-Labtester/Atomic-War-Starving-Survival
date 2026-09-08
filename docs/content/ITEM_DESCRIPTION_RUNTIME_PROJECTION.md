# Item Description Runtime Projection

## 1. Domain Catalog: `ItemDescriptionCatalog`
- Implemented in `Assets/Ashfall.Core/Inventory/ItemDescriptionCatalog.cs`.
- Thread-safe, case-insensitive ID dictionary with alias lookup.
- Methods:
  - `Register(ItemDescriptionEntry entry)`
  - `RegisterAlias(string aliasId, string targetId)`
  - `Get(string itemId)`: Resolves direct ID, or alias, or returns null.
  - `Contains(string itemId)`: True if item or its alias exists.

## 2. Projection Model: `ItemInspectionModel`
- Implemented in `Assets/Ashfall.Core/Inventory/ItemInspectionModel.cs`.
- Aggregates `ItemDefinition` mechanics with optional `ItemDescriptionEntry` prose.
- Key properties:
  - `ItemId`, `DisplayName`, `Category`
  - `HasEnhancedDescription`: True if rich prose exists.
  - `BaseDescription`: Rich overview if present; otherwise `ItemDefinition.description`.
  - `CurrentState`, `VisualIndicators`, `SensoryDetails`, `EmotionalWeight`, `Hazards`, `Dependencies`, `ContaminationStatus`, `PreservationState`, `MakeshiftUtility`.
  - Mechanics: `Weight`, `RadProtection`, `Durability`, `HungerRestore`, `ThirstRestore`, `HealthEffect`, `RadCleanse`, `MoraleEffect`, `TradeValue`, `IsConsumable`, `IsEquipable`, `EquipSlot`.

## 3. Host Surface Integration: `InventoryDetailPanel.cs`
- Bound in `RefreshView()`:
  - Section 1: Item Identity & Primary Description.
  - Section 2: Visual & Sensory Details (if enhanced prose available).
  - Section 3: Hazards, Preservation & Operational Dependencies (if enhanced prose available).
  - Section 4: Item Stats & Mechanics (rad protection, hunger/thirst/health, trade tier).
  - Section 5: Contextual Actions (CONSUME, EQUIP).
