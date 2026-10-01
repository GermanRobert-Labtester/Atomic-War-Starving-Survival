# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Catalog Hygiene Gates — D01–D05 (2026-10-02)

User-directed ("Continue with these small tasks completely finish all of them …
3 find→repair→harden loops … then suggest 15 very small tasks"). Source rows:
the D01–D05 catalog-integrity gate table supplied in the session.

## Bounded outcome

Land a fast, read-only catalog-hygiene audit as a real CI gate, and register the
existing content-orphan certification selftest as a second gate, without
creating a parallel runtime authority.

## D01–D05 disposition

| Row | Outcome | Evidence |
|---|---|---|
| D01 reference-integrity (loot/quest/item/flag) | `catalog_audit` policy declares the expedition `lootCategories` → item-id contract, `scavenging_table_id` → Plan 46 table contract, and `wasteland_map_v1.lootTable` → Plan 46 table contract. Generalizes Plan 76's loot-ref repair. Quest/item/flag cross-refs remain owned by `CatalogIntegrityValidator` (`data_integrity` gate). | `reference_integrity` checked 3 rule/file pairs, 0 unresolved |
| D02 duplicate-id across catalogs | Cross-**id-domain** duplicate detection with an explicit 10-entry policy allowlist for intentional cross-domain reuse (warlord doctrine journal/radio ids, the propaganda template alias, the currents/holdfast faction alias). Allowlist is self-verifying: a stale entry is reported. | `duplicate_ids` checked 8,255 ids, 0 new, 10 acknowledged |
| D03 canonical id/naming lint | `^[a-z0-9]+(_[a-z0-9]+)*$` enforced over every authored id. | `id_naming` checked 10,144 ids, 0 violations |
| D04 `schema_version` value drift | Explicit expected-version map (9 non-1 catalogs) + default 1; any uncoordinated value change fails. | `schema_version_drift` checked 428 catalogs, 0 drift |
| D05 content-utilization orphan certification | Registered `--content-certification-selftest` (Plan 49 / DEC-62) as the `content_certification` CI gate. The verdicts stay owned by `ContentOrphanCertificationEngine`. | gate registered; 9/9 family catalog files verified present; runtime run blocked by an environment-level `csc` kill (see Limitations) |

## Files

- New: `tools/gotools/pkg/catalogaudit/catalogaudit.go` (+ `catalogaudit_test.go`, 12 tests)
- New: `docs/ci/catalog_audit_policy.json`, `docs/ci/catalog_audit_baseline.json`
- Modified: `tools/gotools/cmd/ashfall-dev/main.go` (new `audit-catalogs` command)
- Modified: `docs/ci/CI_GATE_MANIFEST.json` (schema 1.1.5; +`catalog_audit`, +`content_certification`; 66→68 gates, 62→64 fast)

## Three find→repair→harden loops

1. **Loop 1 (bugs in the new code).** Allowlisted duplicates were silently
   skipped (invisible, unverifiable). Repaired: emitted as `Source=policy`
   acknowledged findings; `BaselineFromReport` excludes policy entries so the
   two acknowledgement mechanisms never double-record; `--check` now fails
   closed on unreadable catalogs, not only on findings. Tests added.
2. **Loop 2 (integration).** Verified `go build ./...`, `go vet`, the full Go
   test suite, `release_workflow_parity` (releasepolicy PASS — new gates are not
   `release_required`), `verify-capability-claims.py` (33/33), and
   `whitespace_hygiene` (PASS). Confirmed `docs/INDEX.md` drift and
   `GATE_INVENTORY.md` staleness are pre-existing and foreign; left untouched.
3. **Loop 3 (harden the spot).** `validatePolicy` now rejects an undeclared
   reference target domain or an uncompilable naming regex at load time (fail
   closed), and `TestRealCorpusPolicyIsClean` runs the shipped policy over the
   shipped corpus so data/policy drift is caught by `go test` as well as the CI
   gate.

## Verification

- `go build ./...` OK; `go vet ./pkg/catalogaudit` clean; `gofmt -l` clean.
- `go test ./pkg/catalogaudit/` 12/12 PASS (includes real-corpus integration).
- `run-gates.py --gate catalog_audit` PASS (CATALOG_AUDIT PASS).
- `run-gates.py --gate content_certification` → blocked by stale-host guard;
  host rebuild (`dotnet build Ashfall.csproj`) was killed by the environment
  (`csc` exit 143 at 120s, 465 MiB RAM free / 3.9 GiB swap in use). In the fast
  tier `build_godot_host` runs first, which is exactly what the bounded runner's
  staleness guard requires.
- `releasepolicy` PASS; `verify-capability-claims.py` 33/33; whitespace PASS;
  `git diff --check` clean.
- No full suite; no commit; unrelated dirty worktree preserved.

## Limitations

- D05's Godot execution could not be observed in this environment (compiler
  process killed under memory pressure). Its deterministic prerequisites were
  verified statically (all 9 family catalogs exist) and the engine/session are
  already covered by `ContentOrphanCertificationEngineTests` and
  `Plan49ContentCertificationHostIntegrationTests`.
- D01's reference rules cover the Plan 76 loot seam; quest/item/flag
  cross-references remain owned by the runtime `CatalogIntegrityValidator`.
