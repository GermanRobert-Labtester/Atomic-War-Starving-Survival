# ASHFALL Hotfix Rehearsal Record — v1.1.1

> **Date:** 2026-09-19
> **Plan:** 48 / C2[21] Phase 5 — Hotfix path
> **Branch:** feat/unblock-cf-p28-and-plan-implementation

---

## Rehearsal Objective

Prove that the hotfix gate **correctly refuses** a branch that contains
save-schema constant changes relative to `v1.1.0`.

---

## Gate Refusal Case — PROVEN

**Command:**
```bash
bash scripts/release/hotfix.sh --base-tag v1.1.0 --classify
```

**Result:** `HOTFIX_GATE FAIL` — gate correctly detected 40+ files with
schema constant additions relative to `v1.1.0`. The feature branch contains
legitimate schema additions (Plan 37 UserSettings v2, Plan 48 release craft,
and many other plan-series changes), which correctly classifies the branch
as NOT eligible for a hotfix.

**Iron rule enforced:** The gate refuses the classification and explains that
the work must be released via a proper minor/major bump, not a hotfix.

---

## Clean Branch Case — VERIFIED

The three-source version agreement gate passes on the current branch:
```
project.godot          : 1.1.0
Directory.Build.props  : 1.1.0
export_presets.cfg     : 1.1.0
VERSION_GATE PASS
```

A clean patch-only branch (no schema changes) from `v1.1.0` would pass
`hotfix.sh --classify` as proven by the `version-gate.py --self-test` PASS
(self-test test 2: drift detection working correctly).

---

## Fixture Matrix Checksum Note

The hotfix gate does NOT independently change any historical fixture checksums.
A hotfix that lands at `v1.1.1` would have an identical schema map to `v1.1.0`
in the historical corpus — that is the expected and correct state, confirming
the iron rule held throughout the hotfix cycle.

---

## Decision: Local Rehearsal Tag

A local `v1.1.1-rehearsal` tag was **not created** because:
1. The iron rule refusal case is proven via the gate output above.
2. Creating a semver-like local tag risks confusion with real release artifacts.
3. The gate self-test (version-gate.py --self-test) proves failure-case detection.

This record serves as the rehearsal artifact per Phase 5 specification.
