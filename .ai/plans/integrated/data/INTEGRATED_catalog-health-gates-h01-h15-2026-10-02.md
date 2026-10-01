# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Catalog Hygiene Follow-ups H01–H15 (2026-10-02)

User-directed continuation ("Continue with these small tasks completely finish
all of them and suggest after"). Source rows: the H01–H15 table from the
G01–G15 closeout.

## Rows

| # | Task | Result |
|---|---|---|
| H01 | `container: "goods"` on the economy_goods rule | Added; still 0 findings, 51 values checked |
| H02 | Stale rule-allowlist detection | `reference_allowlist:*` stale entries; test added |
| H03 | Dead `id_domains` glob detection | `policy_domain` finding; test added |
| H04 | `--summary` mirror + reference counts | `mirror_unresolved` + per-rule `reference_rules` map |
| H05 | `--explain` shows remediation | Verified |
| H06 | `GATE_REMEDIATION` keys must be registered | `validate_remediation` in `--check-only` |
| H07 | `--check-only` required fields + classification | `validate_gate_fields` (substituted for the non-viable literal-token check — tokens are built dynamically) |
| H08 | `docs/ci/CATALOG_AUDIT_POLICY.md` | Added |
| H09 | `--dump-ids <domain>` | `DomainIDs` helper + flag; test |
| H10 | `allowlist_reasons` | Validated against the allowlist; policy annotated |
| H11 | Per-file mirror breakdown | `by_catalog` map |
| H12 | Dead `NON_CONTRACT_JSON` exclusion detection | `json-schema-policy-gate.py` |
| H13 | `--list-checks` prints enforced/excluded rules | Verified |
| H14 | Per-rule reference counts | `report.reference_rule_counts` |
| H15 | Known-category ratchet | `validate_categories` in `--check-only` |

## Verification

- Go tests **29/29**; `go vet`/`gofmt` clean.
- Gates `catalog_audit`, `gotools_test`, `gate_inventory_drift`, `json_schema_policy` — all PASS.
- `--check-only` clean (71 gates, 67 fast); `--check-inventory` PASS; `--explain` verified.
- `--summary` emits the one-line JSON; `--dump-ids canonical_item` = 1,033 ids.
- `json-schema-policy-gate.py` PASS (712 data + 8 contract files).
- Whitespace and `git diff --check` clean.
