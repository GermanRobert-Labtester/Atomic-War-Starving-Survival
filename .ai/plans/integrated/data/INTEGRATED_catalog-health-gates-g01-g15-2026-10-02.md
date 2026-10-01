# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Catalog Hygiene Follow-ups G01–G15 (2026-10-02)

User-directed continuation ("Continue with these small tasks completely finish
all of them and suggest after"). Source rows: the G01–G15 table from the
F01–F15 closeout.

## Rows

| # | Task | Result |
|---|---|---|
| G01 | `economy_goods.json` id → canonical-item rule | Added with a 3-entry allowlist (`bandages`, `coal`, `crowbar`); 0 findings |
| G02 | `combat_catalog.json` ammo → canonical-item rule | Added with a new `container: "ammo"` scope; 0 findings |
| G03 | Mirror-resolution ratchet | `mirror_unresolved_max: 393`; over-ceiling emits `mirror_resolution_ratchet` |
| G04 | `--advisory-allowlist` ratchet | Policy `advisory_allowlist`; verified PASS with all 13 keys allowed |
| G05 | `--update-baseline` hint on failure | Stable message verified |
| G06 | Extend `GATE_REMEDIATION` | 23 gates covered |
| G07 | Critical gates need `expected_summary` | Shrink-only ratchet over the 4 known empty-token gates; `--check-only` clean |
| G08 | Contract `schema_version` must be a string | `json-schema-policy-gate.py`; all 8 contract files PASS |
| G09 | Inventory "Depends on" column | `render_inventory` + regenerated `GATE_INVENTORY.md` |
| G10 | `run-gates.py --explain <gate_id>` | Verified |
| G11 | Validate excluded rules + reject enforced/excluded overlap | `validatePolicy` + 2 tests |
| G12 | `--list-advisories` | Verified (13 lines) |
| G13 | Refresh `GATING_VS_DIAGNOSTIC_CHECKS.md` | Date + stale counts corrected |
| G14 | Go test: shipped baseline is empty | Added to the real-corpus test |
| G15 | Manifest note length guard | `Manifest_SchemaVersionNoteIsBounded` (≤2000 chars) |

## Implementation notes

- Reference-target id sets are now built from each domain rule's **globs**
  (a file may contribute to several domains), while duplicate detection still
  uses each file's single assigned domain. This enables the new `canonical_item`
  domain without disturbing the merged `item` surface.
- New `ReferenceRule` fields: `allowlist` (sorted+unique) and `container`
  (scope to a named root array; missing container fails closed).

## Verification

- Go tests **25/25**; `go vet`/`gofmt` clean.
- Gates `catalog_audit`, `gotools_test`, `gate_inventory_drift`, `json_schema_policy` — all PASS.
- `run-gates.py --check-only` clean (71 gates, 67 fast); `--check-inventory` PASS
  with the negative self-check; `--explain` verified.
- `json-schema-policy-gate.py` PASS (712 data + 8 contract files).
- Whitespace and `git diff --check` clean.
