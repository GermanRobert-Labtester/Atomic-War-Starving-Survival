# Production Save Migration & Versioning Contract

This document guarantees backward compatibility, state persistence, and schema migration across all expanded production systems.

---

## 1. Persisted Production Sections

| System | State DTO | Save Store | Invariants & Backward Compatibility |
|---|---|---|---|
| **Silent Foundry** | `SilentFoundryState` | `SilentFoundrySaveStore` | Product IDs are string-keyed; adding 14 new products does not disrupt existing active heat index or queue records. |
| **Greenhouse** | `GreenhouseState` | `GreenhouseHostSession` | Plot states map by integer `plotIndex`; seed IDs use string IDs. Unrecognized future seeds default to fallow without save corruption. |
| **Apiculture** | `ApicultureState` | `GreenhouseHostSession` / embedded | Hive state lists preserve `queenVitality`, buffers, and installation timestamps. |
| **Salt Extraction** | `SaltMineState` | `SaltMineExtractionSystem` | Vein list string-keyed; deliveries list preserved with acceptance flags. |
| **Kitchen Nutrition**| `KitchenNutritionState` | `KitchenNutritionSystem` | Active prep jobs and pantry items persist with `PreservationMethod` enum serialization. |

---

## 2. Migration Tests

- Loading a save created with only 11 foundry products and 4 greenhouse crops must load cleanly, initialize new catalog lookups, and resume simulation without null exceptions.
- Active heats and active crop growth cycles are deep-copied during capture to avoid aliasing and state corruption during disk writes.
