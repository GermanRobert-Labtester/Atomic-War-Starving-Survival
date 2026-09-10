# Plan 145 Regression Matrix

Test coverage matrix and verification cases for bunker graffiti activation, loader hardening, location projection, and cross-system isolation.

## 1. Automated Test Suites

| Test Suite | File Path | Coverage / Invariants Tested |
|---|---|---|
| **Catalog Baseline & Hardening** | `Ashfall.Core.Tests/BunkerGraffitiCatalogTests.cs` | Canonical 36 postings load; expansion 40 postings load; multi-source load yields 76 unique postings; idempotent reload prevents duplicate rows; case-insensitive ID collision rejection; malformed entry validation (null/empty ID, empty content, negative day). |
| **Location Projection & Scoping** | `Ashfall.Core.Tests/BunkerGraffitiProjectionTests.cs` | Projection maps 76 postings to valid canonical scopes (`room_*`, `loc_*`); exact room isolation (pump posting does not leak into clinic); world map isolation (shelter postings do not leak to wasteland map); unresolved locations remain deferred. |
| **Day Semantics & Boundary** | `Ashfall.Core.Tests/BunkerGraffitiDaySemanticsTests.cs` | Eligibility at `Day N-1` (hidden), `Day N` (visible), `Day N+1` (visible); deterministic sorting by `(recorded_day, posting_id)`; negative day handling. |
| **Cross-System Non-Interference** | `Ashfall.Core.Tests/BunkerGraffitiCrossSystemTests.cs` | Viewing wall text does not alter morale; does not alter faction standing; does not alter flags or quests; does not generate memorial plaques; does not trigger moral choice gossip; `morale_effect` is non-executable text. |

## 2. Negative Fixture Suite

| Negative Case | Expected Handling |
|---|---|
| Duplicate `posting_id` in same file or secondary file | Second entry ignored; `_allPostings` count remains exactly 1; `_byId` retains valid entry. |
| Case-only duplicate `posting_id` (e.g. `GRAF_01` vs `graf_01`) | Detected as collision; ignored/deduped; no duplicate row added. |
| Negative `recorded_day` (e.g. `-5`) | Validation marks entry invalid; skipped or rejected with warning. |
| Empty / whitespace `content` | Validation marks entry invalid; skipped or rejected with warning. |
| Malformed JSON syntax | Handled safely by `IJsonSerializer` with `CatalogDiagnostics.Warn`; returns empty catalog without crashing host. |
| Potentially dangerous `morale_effect` string (`+999 morale; execute_revolt()`) | Treated strictly as inert string; zero mechanical execution. |
| Double `Load()` call with identical JSON | Idempotency guard prevents duplicate list entries; count matches unique records. |

## 3. Verification Gate Execution

```bash
# 1. Core Unit & Determinism Tests
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# 2. Project Host Build
dotnet build Ashfall.csproj

# 3. Godot Data Integrity Self-Test
godot --headless --path . -- --data-integrity-selftest

# 4. Godot Content Utilization Self-Test
godot --headless --path . -- --content-utilization-selftest

# 5. Godot Scene Binding Self-Test
godot --headless --path . -- --scene-binding-selftest

# 6. CI Scene Linting
python3 scripts/ci/scene-lint.py
```
