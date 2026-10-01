# ASHFALL Go Tooling (`tools/gotools`)

Persistent development/CI tooling, invoked through `bin/ashfall-dev`
(`cmd/ashfall-dev`) or as standalone commands. All checks are read-only unless a
command explicitly writes a report/baseline.

```bash
go run -C tools/gotools ./cmd/ashfall-dev <command> [options]
```

## `audit-catalogs` — catalog-hygiene audit (D01–D04)

Fast, read-only audit of `Assets/StreamingAssets/Data`. Policy:
`docs/ci/catalog_audit_policy.json`; findings baseline:
`docs/ci/catalog_audit_baseline.json`. The runtime cross-reference authority
remains `Ashfall.Core.CatalogIntegrityValidator` (`data_integrity`).

```bash
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --check
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --check --strict-stale
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --json
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --summary
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --list-checks
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --list-advisories
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --list-advisories --advisory-check id_unit_suffix
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --dump-ids canonical_item
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --dump-duplicates
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --fail-on-advisory
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --report-json /tmp/cat-audit.json
go run -C tools/gotools ./cmd/ashfall-dev audit-catalogs --root ../.. --update-baseline
```

Checks (fail): `reference_integrity`, `duplicate_ids`, `id_naming`,
`schema_version_drift`. Advisories (never fail): `id_unit_suffix`,
`mirror_resolution`. `--strict-stale` also fails on stale baseline/allowlist
entries; `--update-baseline` records every unacknowledged finding.

## Other commands

`index`, `select-tests`, `run-scoped-tests`, `check-plan`, `parse-results`,
`validate-json`, `validate-config`, `scan-saves`, `build-manifest`, `run-tasks`,
`llm-proxy`, `sync-agents`, `agent-core`, `monitor-size`, `monitor-compile`.
Run `bin/ashfall-dev help` for the full list.
