# ASHFALL — Maintenance & Migration Scripts

This directory houses historical one-off migration utilities and reusable batch-transformation tools for the ASHFALL codebase.

---

## Script Catalog

| Script | Status | Owner / Subsystem | Description & Usage |
|---|---|---|---|
| [`add_p11_methods.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/add_p11_methods.py) | `HISTORICAL` | Godot Host Sessions / Save | Scaffolds `IsDirty`, `MarkDirty`, and `Save()` methods on legacy `HostSession` classes. Superseded by v2 and generic `SaveStore<T>`. |
| [`add_p11_methods_v2.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/add_p11_methods_v2.py) | `HISTORICAL` | Godot Host Sessions / Save | Batch-added dirty tracking and save delegation across 28+ HostSessions with per-store DTO mappings. |
| [`cleanup_p11_hostsessions.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/cleanup_p11_hostsessions.py) | `HISTORICAL` | Godot Host Sessions / Architecture | Cleaned up newly scaffolded HostSessions to inherit `HostSessionBase` and stripped duplicate event definitions. |
| [`convert_hostsessions.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/convert_hostsessions.py) | `HISTORICAL` | Godot Host Sessions / Architecture | Migrated sealed `HostSession` classes in `src/Host/` to inherit from `HostSessionBase`. |
| [`fix_event_leaks.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/fix_event_leaks.py) | `HISTORICAL` | Godot Host Sessions / Lifecycle | Scaffolds `UnsubscribeAll()` teardown methods in HostSessions to prevent C# delegate leaks on reload. |
| [`consolidate_catalog_tests.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/consolidate_catalog_tests.py) | `REUSABLE` | Test Suite / Catalog Infrastructure | Batch-consolidates catalog test classes in `Ashfall.Core.Tests` to inherit from `CatalogTestBase`. (`--dry-run` supported) |
| [`migrate_schema_version.py`](file:///home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic%20War/scripts/maintenance/migrate_schema_version.py) | `REUSABLE` | Data Authority / JSON Schemas | Utility to migrate or validate JSON schema version headers across `Assets/StreamingAssets/Data/`. (`--check`, `--dry-run`, `--write`) |

---

## Operating Guidelines

1. **Historical Scripts (`HISTORICAL`)**:
   - Preserved for architectural provenance, forensic audits, and reference.
   - Do **not** execute against production trees without a clean Git checkpoint (`git status` clean).
2. **Reusable Maintenance Tools (`REUSABLE`)**:
   - Always run in `--dry-run` or `--check` mode first to inspect prospective AST or file changes before committing writes.
   - Any modifications to data or host files must be followed by `bash scripts/ci/verify-fast.sh` and `dotnet test`.
