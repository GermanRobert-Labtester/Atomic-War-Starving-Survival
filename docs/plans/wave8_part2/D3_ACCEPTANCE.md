# D3 — Acceptance

**Task:** Wave 8 Part 2, TASK D3
**Verdict:** PASS — shutdown warning classified benign with flat repeat-cycle evidence; real lifecycle path clean.

## Command log

| Command | Result |
|---|---|
| `godot --headless --path . -- --ui-accessibility-selftest` (×3) | PASS; identical shutdown signature each run (8 ObjectDB / 2 CanvasItem / 1 font RID / 1 resource) |
| `godot --headless --verbose --path . -- --ui-accessibility-selftest` | leaked set identified (2 parentless Label, 2 Object, StyleBoxFlat, StyleBoxEmpty, FontFile, Image) |
| `godot --headless --path . -- --panel-bind-lifecycle-selftest` | PASS; **0** shutdown leak lines |
| `dotnet build Ashfall.csproj` | 0 warnings / 0 errors |
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | green (no code change) |
| `python3 scripts/ci/generate-architecture-map.py --check` | OK |
| `python3 scripts/ci/generate-docs-index.py --check` | OK |

## Acceptance matrix (plan §D3)

| Gate | Status |
|---|---|
| Gate 1 — original a11y reproduction | PASS (reproduced) |
| Gate 2 — panel lifecycle selftest | PASS (clean — counter-proof) |
| Gate 3 — repeated open/close cycle telemetry | PASS (flat across 3 runs) |
| Gate 4 — settings/panel-heavy selftests | N/A (player-panels-uitest is a separate, larger diagnostic class; not implicated) |
| Gate 5 — audio/bridge selftest when signal lifetime implicated | N/A (no signal leak found) |
| Gate 6 — export-smoke-adjacent shutdown check | N/A (no production lifetime change) |
| Gate 7 — unchanged snapshot target | Trivially satisfied — no rendering/disposal code changed |
| Gate 8 — build and verify-fast | PASS |
| Gate 9 — replay/fingerprint unchanged | Trivially satisfied — no simulation code changed |

## Leak-class table

| Warning signature | Class | Owner | Fix/proof | Regression gate |
|---|---|---|---|---|
| a11y: 8 ObjectDB / 2 CanvasItem RIDs / 1 font RID / 1 resource | benign harness specimen lifetime | `UiAccessibilitySelfTest` | flat ×3 + lifecycle path clean; documented in guide §4 | `--panel-bind-lifecycle-selftest` = 0 leaks |
| player-panels-uitest: 269 ObjectDB / 103 CanvasItem | separate diagnostic class (pre-existing) | panel-heavy UI test | not implicated by D3 | `--panel-bind-lifecycle-selftest` |

Remaining standing failures: 0.
