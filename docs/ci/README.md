# ASHFALL CI — Canonical Verification Contract (Plan VIII · Task 24)

One runner, one manifest, one meaning of "green".

```bash
python3 scripts/ci/run-gates.py --tier fast    # pre-merge standard (fail-fast)
python3 scripts/ci/run-gates.py --tier full    # shippable standard (complete report)
python3 scripts/ci/run-gates.py --gate data_integrity,build_godot_host
python3 scripts/ci/run-gates.py --list         # inventory
python3 scripts/ci/run-gates.py --check-only   # manifest validation only
python3 scripts/ci/run-gates.py --report-json artifacts/gates.json --no-fail-fast --tier full
```

- **Manifest authority:** `docs/ci/CI_GATE_MANIFEST.json` (gates, tiers, commands,
  timeouts, expected summary tokens, dependencies). Inventory view:
  [GATE_INVENTORY.md](GATE_INVENTORY.md). Gate order in the manifest is execution
  order; `depends_on` prerequisites are expanded and a gate whose prerequisite
  failed is BLOCKED, never run against stale bytes.
- **Selftest registry authority:** the `HostCli` verb registry in `src/Host/`.
  The fast gate `selftest_manifest_drift` fails CI when
  `docs/ci/SELFTEST_MANIFEST.json` drifts from the registry
  (`generate-selftest-manifest.py --check`). Never hand-edit the manifest.
- **Export parity:** `export_parity` (full tier) runs
  `--export-parity-selftest` against `builds/linux/`; produce a fresh artifact
  with `scripts/ci/export-build.sh` (see `docs/RELEASE_EXPORT.md`). A full-tier
  green without export parity is **not** shippable.

## What green means

| Tier | Meaning | Target |
|---|---|---|
| fast | Every Core invariant, build, drift guard, and smoke gate passes on current source. | < 10 min |
| full | fast + runtime-scale performance + exported-build packaged-data parity. | measured per run (`--report-json`) |

## Failure taxonomy (Task 24.9)

Every failed gate is classified in the JSON report (`failure_taxonomy` summary,
per-gate `failure_type`): `blocked` (prerequisite failed) · `timeout` (budget
exceeded) · `build-break` (compile error) · `assert-fail` (unit suite) ·
`selftest-fail` (host selftest) · `infrastructure` (missing tool/exec error) ·
`quarantined` (see below) · `fail` (anything else).

## Quarantine policy (Task 24.7)

Registry: `scripts/ci/quarantine.json`. Rules enforced by the runner:

- every entry needs `gate`, `owner`, `reason`, `added`, `expiry` (YYYY-MM-DD);
- maximum duration **14 days**; expired entries are policy violations and fail
  the run until removed or re-justified;
- Core-invariant gates (`protected_gates`: builds, unit suite, data integrity,
  bridge) can never be quarantined;
- a quarantined gate **still runs**; a quarantined failure is shown
  (`🛡 QUARANTINED`) and recorded, does not by itself fail the run, and never
  blocks dependent gates (they run — the gate executed).

## Concurrency / transient compile policy (Task 24.8)

The repository is worked by concurrent AI streams; a dirty tree is normal.
Preflight: `git status --porcelain` — a dirty tree is a **warning**, not a
failure. When a build gate fails with compile errors:

1. check for concurrent mutation (files changing under you, foreign staged work);
2. retry the gate **once**, only when the failure is classified
   `build-break`/`infrastructure` and evidence of concurrent edits exists;
3. a repeat failure is a real break — fix or report, never re-run into green.

Assertion failures are never retried as flakes.

## Runtime budgets (Task 24.10)

Gate `timeout_seconds` values in the manifest are enforced ceilings; measured
durations land in every `--report-json` artifact for tuning. If the full tier
exceeds its envelope, optimize the dominant gates first — release-critical
gates (export parity, data integrity) are never moved out to make it fit.
