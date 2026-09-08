# Item Inspection Authority Map

## 1. Architectural Invariant & Separation of Concerns

```
       +------------------------------------+
       |  Mechanics Authority               |
       |  items.json + expansion catalogs   |
       |  (ItemDefinition, ItemCatalog)     |
       +-----------------+------------------+
                         |
                         |  (gameplay stats: weight, radCleanse, stackMax, durability)
                         v
       +------------------------------------+      (flavor prose: sensory, visual, hazards)
       |  ItemInspectionModel               | <--------------------------------------------+
       |  (Pure C# Read-Only Projection)    |                                              |
       +-----------------+------------------+                                              |
                         |                                                                 |
                         |  (binds model to UI)                                            |
                         v                                              +------------------+-------------------+
       +------------------------------------+                           |  Descriptive Prose Authority         |
       |  Presentation Layer (Godot Host)   |                           |  item_description_texts.json         |
       |  InventoryDetailPanel              |                           |  (ItemDescriptionCatalog)            |
       +------------------------------------+                           +--------------------------------------+
```

## 2. Hard Boundaries
1. **Zero Mechanics from Prose**:
   `item_description_texts.json` is strictly descriptive. No text field can alter damage, radiation cleanse, durability, stack limits, trade values, or spawn rates.
2. **Zero Engine Coupling in Core**:
   `ItemDescriptionEntry`, `ItemDescriptionCatalog`, `ItemDescriptionCatalogLoader`, and `ItemInspectionModel` live in `Assets/Ashfall.Core/Inventory/` with 0 references to Godot or Unity.
3. **Zero Save Data Mutation**:
   Descriptions are static data loaded at startup. Neither `ItemDescriptionCatalog` nor `ItemInspectionModel` emits or consumes save data. Saves remain 100% backward/forward compatible.
4. **No Phantom Items**:
   Orphaned flavor items (e.g. `vehicle_car`, `art_painting`, `furniture_bed`) remain read-only lore entries in `ItemDescriptionCatalog`. No fake item definitions are added to `items.json` to satisfy them.
