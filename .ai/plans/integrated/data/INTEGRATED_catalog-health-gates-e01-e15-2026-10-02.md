# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Catalog Hygiene Follow-ups E01–E15 (2026-10-02)

User-directed continuation ("Continue with these small tasks completely finish
all of them and suggest after"). Source rows: the E01–E15 table from the D01–D05
closeout.

## Bounded outcome

Land the 15 suggested catalog-hygiene/tooling follow-ups as small, independently
verifiable changes. No new gameplay, save, or runtime authority.

## Rows

| # | Task | Files |
|---|---|---|
| E01 | `gotools_test` fast gate (`go test ./...` in `tools/gotools`) | CI manifest |
| E02 | Regenerate `GATE_INVENTORY.md` + `run-gates.py` inventory `--check` + `gate_inventory_drift` gate | `run-gates.py`, manifest, inventory doc |
| E03 | Register `--content-utilization-selftest` as `content_utilization` gate | manifest |
| E04 | `audit-catalogs --strict-stale` | `main.go`, `catalogaudit.go`, manifest |
| E05 | Parse `reference_rules_excluded` and echo in `--json` | `catalogaudit.go`, tests |
| E06 | Unit-suffix advisory census (non-failing) | `catalogaudit.go`, tests |
| E07 | Mirror-resolution advisory (non-failing; 162+231 mirrors own ids by design) | `catalogaudit.go` |
| E08 | Manifest `depends_on` must reference a registered gate | `CiGateManifestDriftTests.cs` |
| E09 | `depends_on: ["build_godot_host"]` on `content_certification` | manifest |
| E10 | Validate `docs/ci/*.json` contract roots in the schema-policy gate | `json-schema-policy-gate.py` |
| E11 | Document the dual `lootCategories` contract | `docs/ci/README.md` |
| E12 | Shrink-only assertion: acknowledged duplicates ≤ 10 | `catalogaudit_test.go` |
| E13 | `audit-catalogs --list-checks` + README entry | `main.go`, README |
| E14 | Cross-reference comment near `lootCategories` in the runtime rules | `CatalogIntegrityRules.cs` |
| E15 | Document the new gates in `docs/ci/README.md` | README |

## Verification

- `go test ./...` (tools/gotools), `go vet`, `gofmt`.
- `run-gates.py --gate gotools_test,catalog_audit,gate_inventory_drift`.
- `json-schema-policy-gate.py`, whitespace, `git diff --check`.
- E03 verified by replaying the `ContentUtilizationGate` logic over the current
  `artifacts/content-utilization.json` against the camelCase baseline: 0
  regressions, 0 new orphans.
