# Schema Version Policy

**Date:** 2026-08-23
**Status:** Policy established; migration tool pending

---

## 1. Policy

### 1.1 What Gets a `schema_version`

| Category | Examples | Action |
|----------|----------|--------|
| **Versioned catalogs** | `questline_master.json`, `expansion_*` catalogs | Add `schema_version` at root |
| **Wrapper-list files** | `items.json`, `locations.json`, `survivors.json` | Wrap as `{"schema_version": 1, "key": [...]}` |
| **Object-root catalogs** | `disease_catalog.json`, `utility_actions.json` | Add `schema_version` at root |
| **Narrative content blobs** | `radio_scriptbook.json`, `oral_lore_codex.json` | Already have `schema_version`; keep |
| **Static content** | `holdfast_flavor.json`, `foundry_*.json` | Evaluate; likely no version needed |

### 1.2 Root-Shape Classification

| Root Shape | Examples | Representation |
|------------|----------|----------------|
| **Object root** | `{"schema_version": 1, "key": {...}}` | Single catalog object |
| **Wrapper list** | `{"schema_version": 1, "key": [...]}` | List wrapped in object |
| **Bare list** | `[...]` | Migrate to wrapper list |
| **Static blob** | `{"flavor_text": "..."}` | No version; content-only |

### 1.3 Loader Contract

Every loader must:
1. Accept versioned form (preferred)
2. Accept legacy unversioned form (fallback)
3. Reject unsupported future versions with clear error
4. Preserve data semantics during migration

### 1.4 Compatibility Window

- Legacy unversioned files are supported for **2 major releases**
- After that, loaders may hard-reject missing `schema_version`
- Migration tool must be idempotent

---

## 2. Pilot Matrix

Select 5 representative files for pilot migration:

| # | File | Shape | Expected Action |
|---|------|-------|-----------------|
| 1 | `disease_catalog.json` | Object root | Add `schema_version: 1` at root |
| 2 | `utility_actions.json` | Object root | Add `schema_version: 1` at root |
| 3 | `holdfast_items.json` | Wrapper list | Verify existing wrapper |
| 4 | `verdict_items.json` | Wrapper list | Verify existing wrapper |
| 5 | `narrative/radio_scriptbook.json` | Object root | Already has `schema_version`; verify loader |

---

## 3. Migration Tool Design

### Modes

```text
--check   # Report what would change; no writes
--write   # Mutate only validated eligible files
--dry-run # Same as --check
```

### Requirements

- Parse JSON; do not detect `schema_version` by searching first 200 bytes
- Classify root shape
- Preserve semantic content
- Avoid double-wrapping
- Output migration manifest
- Idempotent after migration

---

## 4. Exit Criteria

- [ ] Policy documented and approved
- [ ] Pilot files migrated and verified
- [ ] Loader contract tests pass
- [ ] `--check` is idempotent after migration
- [ ] Aggregate data-integrity remains green
