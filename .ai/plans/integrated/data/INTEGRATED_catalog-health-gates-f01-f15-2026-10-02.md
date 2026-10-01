# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Catalog Hygiene Follow-ups F01–F15 (2026-10-02)

User-directed continuation ("Continue with these small tasks completely finish
all of them and suggest after"). Source rows: the F01–F15 table from the
E01–E15 closeout.

## Rows

| # | Task | Result |
|---|---|---|
| F01 | Policy `source_glob` must match ≥1 catalog | `Run` emits a fail-closed `reference_integrity` finding; test added |
| F02 | Allowlist sorted + unique | `validatePolicy` rejects both; 2 tests added |
| F03 | `Report.HasStale()` | Added; `--strict-stale` now uses it; test added |
| F04 | `--fail-on-advisory` opt-in | Added; verified FAIL on the 13 live advisories |
| F05 | `--report-json <path>` | Added; verified report written (0 findings / 13 advisories) |
| F06 | Data-driven `docs/ci/*.json` contract scan | `json-schema-policy-gate.py` globs the dir; all 712 data + 8 contract files PASS |
| F07 | C# header-count test | `Manifest_HeaderCountsMatchGatesArray` added |
| F08 | `--check-only` validates header counts | `validate_manifest_counts`; PASS (71/67) |
| F09 | Pin `lootCategories` runtime classification | `LootCategoriesIsAVocabularyKeyByDesign` test added |
| F10 | Negative inventory test | `check_inventory` self-checks a mutated copy; verified mutated file fails |
| F11 | Document the 5 new gates | `docs/ci/GATING_VS_DIAGNOSTIC_CHECKS.md` §6 added |
| F12 | Failure-artifact remediation hints | `GATE_REMEDIATION` + artifact line; verified end-to-end |
| F13 | Release-path coverage | Verified: `content_utilization` is fast tier → `verify-fast.sh --tier fast` → `release-gate.sh`; no edit needed |
| F14 | `--summary` one-line JSON | Added; verified |
| F15 | Policy/baseline version discipline | `validatePolicy` requires `schema_version`; real-corpus test asserts policy == baseline |

## Verification

- Go tests **21/21**; `go vet`/`gofmt` clean.
- Gates `catalog_audit`, `gotools_test`, `gate_inventory_drift`, `json_schema_policy` — all PASS.
- `run-gates.py --check-only` clean (71 gates, 67 fast); `--check-inventory` PASS
  with a working negative self-check.
- `json-schema-policy-gate.py` PASS (712 data files + all `docs/ci/*.json`).
- Whitespace and `git diff --check` clean.
- The manifest was concurrently retuned by another agent mid-task (timeouts only);
  `GATE_INVENTORY.md` was regenerated to match and the drift gate re-verified.
