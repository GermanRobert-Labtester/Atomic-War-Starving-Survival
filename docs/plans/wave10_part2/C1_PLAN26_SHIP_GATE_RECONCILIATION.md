# Plan 26 (C2[8]) Reconciliation Report: "The Ship Gate"

**Package:** Wave 10 Part 2 — Task C1\
**Plan Authority:** `C-integration-plans/C2_planintegration[8].md` (Plan 26)\
**Status:** **SEALED / VERIFIED-GREEN**\
**Date:** 2026-09-17\
**Integrator:** Antigravity\

---

## 1. Executive Summary

Plan 26 ("The Ship Gate: Exported Builds That Actually Find Their Data") converts release artifacts from unverified outputs into fully tested runtime products across three sequenced axes:
- **26A — Single Data Path Authority:** All catalog accesses route through `CatalogPath` or sanctioned helpers. Bypasses and dev-only hardcoded relative paths are eliminated.
- **26B — Real Export Staging & Packaged Parity Smoke:** Canonical export scripts stage data once, build the executable, deploy loose data matching `CatalogPath` precedence #2, verify 100% byte-parity and exact casing, and boot the exported binary headlessly.
- **26C — Performance Budgets:** Advisory telemetry metrics are converted into contractual performance budgets with enforced failure thresholds.

---

## 2. Clause Reconciliation Matrix

| Sub-Plan / Clause | Historical Intent | Prior Sealing Package | Current Implementation Status | Final Verdict | Verification Evidence |
|---|---|---|---|---|---|
| **26A.1 CatalogPath Centralization** | Eliminate bypass readers in `src/` | Wave 10 Part 1 Task C2 | `CatalogPath.ResolveCatalog()` & `ResolveSub()` used across all 40 former bypass sites. | **SEALED** | `CatalogPathForbiddenGateTests` (2/2 PASS; allowlist = 1: `CatalogPath.cs` itself). |
| **26A.2 Boot Resolution Diagnostics** | Log resolved data root on startup | Wave 10 Part 1 Task C2 | `CatalogPath.ResolveRepoRoot()` records resolution source (`LastResolutionSource`). | **SEALED** | Catalog boot report in headless logs. |
| **26B.1 Canonical Staging & Packaging** | One staging/export pipeline matching developer workflow | Wave 2 / Wave 8 | `scripts/ci/godot-export-linux.sh` and `scripts/ci/export-build.sh` execute 7-stage pipeline. | **SEALED** | Clean export pipeline; mirror-deploy to `builds/linux/Assets/StreamingAssets/Data`. |
| **26B.2 Packaged Parity Gate** | Assert SHA-256 byte-parity, casing, and absence of Git-LFS pointers | Wave 8 Part 1 | `src/Host/HostCli.ExportParity.cs` (`--export-parity-selftest --parity-target <path>`). | **SEALED** | Checked files validated byte-identical and parseable. |
| **26B.3 Shipped Binary Boot Smoke** | Boot exported executable headlessly for ≥60 frames | Wave 8 Part 1 | `export-build.sh` step 7: executes `ashfall.x86_64 --headless --quit-after 60`. | **SEALED** | Boot smoke: OK (60 frames). |
| **26C.1 Performance Budgets Document** | Author enforceable latency, frame, and memory thresholds | Wave 10 Part 1 Task C2 | `docs/perf/BUDGETS.md` specifies 5 contractual budgets (startup, frame, save/load, memory, payload). | **SEALED** | `docs/perf/BUDGETS.md` active at HEAD. |
| **26C.2 Runtime Scale Verification Gate** | Automated selftest evaluating budgets against real simulation | Wave 10 Part 1 Task C2 | `godot --headless --path . -- --performance-selftest` evaluates 6 contract assertions. | **SEALED** | 6/6 checks PASS (30d advance 1.3ms < 2s, save 39.6ms < 500ms, alloc 2.6KB < 5MB). |

---

## 3. Verification Commands & Results

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs` — 2/2 PASS.
2. `godot --headless --path . -- --performance-selftest` — 6/6 PASS.
3. Fast CI Gate 32 (`PerformanceSelfTest`) — PASS.
4. All 48 Fast CI Gates (`scripts/ci/verify-fast.sh`) — PASS.

**Conclusion:** Plan 26 (C2[8]) is **SEALED**.
