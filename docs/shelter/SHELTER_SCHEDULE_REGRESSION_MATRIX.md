# Shelter Schedule Regression Matrix

This matrix documents the verification coverage for Plan 70 (Shelter Schedules Expansion: 3 → 12 Duty Rhythms).

---

## 1. Test Suite Coverage

| Test Class | Test Method | Target Invariant | Result |
|---|---|---|---|
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_LoadsSuccessfully_HasExactCountOf12` | Schema Versioning & Catalog Size | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_JsonDocument_HasSchemaVersionAndCollectionId` | Invariant 6 (JSON Authority) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_PreservesThreeBaselineSchedules` | Backward Compatibility (Baseline Preservation) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_ContainsAllNineNewSchedules` | Expansion Completeness (9 New Authored) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_AllSchedules_HaveUniqueValidIdsWithPrefix` | ID Grammar (`schedule_*`) & Set Uniqueness | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_AllSchedules_HaveValidHoursAndModifiers` | Domain Range Validation (Hours, Fatigue, Lighting) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `Catalog_AllSchedules_HaveValidShiftPatternsAndTriggerConditions` | Grammar Compliance (Patterns, Triggers, Prose) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `System_SetSchedule_SwitchesToAll12Schedules` | Core Runtime Activation & Reflection | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `System_EmergencyOverride_RespectsScheduleAllowFlag` | Security Policy Enforcement (Lockout Gates) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `System_TryActivateScheduleByTrigger_ActivatesTargetSchedule` | Incident & Seasonal Trigger Integration | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `System_SaveAndRestore_PreservesActiveSchedule` | Invariant 3 (Save/Load Round-Trip Fidelity) | PASS |
| `ShelterSchedulesPlan70CatalogTests` | `System_TickDay_AppliesActiveScheduleModifiers` | Daily Simulation Math & Lighting Demand | PASS |
| `NewCatalogLoaderTests` | `LoadsThreeSchedules_FromRealJson` | Legacy Catalog Loader Regression | PASS |
| `ShelterScheduleSystemTests` | All 8 tests (Phase change, bed assign, compliance) | Core Behavior Invariants | PASS |
| `ShelterScheduleIntegrationTests` | All 3 tests (Curfew/Emergency, Bed tracking, Save) | Subsystem Integration Invariants | PASS |

---

## 2. Invariant Compliance Checklist

- [x] **Invariant 1 — Zero engine coupling in Core:** `ShelterScheduleSystem` and `ScheduleDefinition` use standard .NET BCL (`System.Text.Json.Serialization`), with no references to Unity, Godot, or engine namespaces.
- [x] **Invariant 2 — Ports & Adapters:** Uses `IFileIO` and `IJsonSerializer` via `ShelterScheduleCatalogLoader`.
- [x] **Invariant 3 — Cross-host save compatibility:** `ShelterScheduleState` persists `activeScheduleId`, maintaining round-trip fidelity with backward-compatible defaults.
- [x] **Invariant 4 — Determinism:** Zero unseeded random, zero clock leaks. State changes are fully deterministic.
- [x] **Invariant 5 — No gameplay logic in hosts:** Godot `ShelterScheduleHostSession` and `ShelterSchedulePanel` remain strictly presentation/event-binding layers.
- [x] **Invariant 6 — Data authority is JSON:** `Assets/StreamingAssets/Data/shelter_schedules.json` is the sole source of truth for all schedule parameters.
