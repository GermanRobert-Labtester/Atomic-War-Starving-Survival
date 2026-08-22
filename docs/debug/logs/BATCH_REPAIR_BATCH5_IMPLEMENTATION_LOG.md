# Batch 5 Repair Implementation Log (filled)

**Plan.** `docs/debug/plans/BATCH_REPAIR_BATCH5_PLAN.md`
**Source audit.** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batches.** Batch 1 + 2 + 3 + 4 — RESOLVED.
**Scope.** 4 phases: BUG-03 warmth propagation; BUG-11 decon deltas; BUG-04 physics runtime + `KwPerFuelUnit` retune; BUG-15 deferred-from-Batch-2 brownout regression test.
**AGENTS.md.** Untouched per user's standing direction.

---

## Phase 0 — Stitch integration + external infra

- `stitch-mcp@0.9.0` force-loaded via `STITCH_API_KEY` from `/home/robertsrff/Desktop/design.env` (user direction).
- `stitch-mcp doctor`: `✔ API Key: Detected` + `✔ Stitch API: Healthy (200)`.
- Read-only tool enumeration: `get_screen_code`, `get_screen_image`, `build_site`, `list_tools`, `get_project`. No `create_project` calls against `lightgames77@gmail.com`.
- Token value never echoed, never written into the repo.

---

## Phase 1 — BUG-03 warmth propagation

**Pre-integration checkpoint.** PASS — `ShelterThermalSystem.cs:84` already injected `NeedsSystem _needs` but never used; `ShelterAssignmentSystem.GetAssignmentsForRoom(roomId)` (Core) is the canonical residency API. Host comment claiming "host session reads this" was forward-looking fiction.

**Changes:**
- `Assets/Ashfall.Core/ShelterThermalSystem.cs` — added optional `ShelterAssignmentSystem? assignment = null!` constructor parameter and `_assignments` field; added per-room warmth propagation block in `TickDay`.
- `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` — added `Bug03_Warmth_Propagates_ToInRoomSurvivors`.

**Regression test:** survivor in 25 °C room with warm ambient → `Warmth` raised >30 after `TickDay(1)`.

**Verification:** `dotnet test --filter FullyQualifiedName~ShelterThermalSystemTests` → 13/13 PASS.

**Result:** ✅ RESOLVED.

---

## Phase 2 — BUG-11 decon net-contamination

**Pre-integration checkpoint.** PASS — deltas were `BypassSurfaceDelta = -0.1f`, `BypassShelterDelta = +0.1f` (symmetric transfer; audit §8).

**Changes:**
- `Assets/Ashfall.Core/DecontaminationSystem.cs` — `BypassShelterDelta = 0f` with bug-history comment; surfaced dust no longer transferred into shelter air.
- `Ashfall.Core.Tests/DecontaminationSystemTests.cs` — semantic update of `CompleteCycle_Bypass_IncreasesShelterContamination` to post-fix expectation; added `Bug11_Bypass_NetShelterContamination_IsNotIncreased`.

**Regression test:** bypassed cycle after partial shelter contamination → shelter level never increases.

**Verification:** `dotnet test --filter FullyQualifiedName~DecontaminationSystemTests` → 11/11 PASS.

**Result:** ✅ RESOLVED.

---

## Phase 3 — BUG-04 physics runtime follow-up

**Pre-integration checkpoint.** PASS — Batch 4's explicit-Euler with `t = 86400 s` overshot every room into the `boilerTarget+10` clamp (numeric instability), from `KwPerFuelUnit=10` → instant saturation.

**Changes:**
- `Assets/Ashfall.Core/ShelterThermalSystem.cs` — per-room solve changed to the analytic-form of `dT/dt = (G - k·(T - T_out))/C` (stable at any timestep; `τ = ρ·cp/h` independent of volume); `KwPerFuelUnit = 0.05f` tuning with docstring.
- `Ashfall.Core.Tests/ShelterThermalSystemTests.cs` — two Bug-04 regressions rewritten for analytic steady-state expectations.

**Regression tests:** `Bug04_HeatGain_Physics_Matches_Audit_Formula` (steady ≈ `T_out + G/k` at high-power fixture under clamp); `Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat` (per-room independence).

**Verification:** `dotnet test --filter FullyQualifiedName~ShelterThermalSystemTests` → 13/13 PASS.

**Result:** ✅ RESOLVED.

---

## Phase 4 — BUG-15 deferred from Batch 2

**Pre-integration checkpoint.** PASS — Batch 2 resolution explicitly deferred: "*the deferred automated test should be authored in a future batch that addresses the PowerGridSystem brownout testability issue first*". Batch 4 provided the testability surface (`EffectiveTotalDrawWatts`); this phase pays off the deferral.

**Changes:**
- `Ashfall.Core.Tests/ShelterScheduleSystemTests.cs` — added `TickDay_Brownout_DoublesLightingDemandHalving`.

**Regression test:** real brownout grid (`Generation=50`, `BatteryReserve=0`, draw > generation) → lighting demand halves from 0.5 → 0.25.

**Verification:** `dotnet test --filter FullyQualifiedName~ShelterScheduleSystemTests` → all PASS.

**Result:** ✅ RESOLVED.

---

## Falsified candidates (logged for honesty)

- **BUG-07 re-open** — falsified (already closed in Batch 2, tests live).
- **Batch 1/2 UI-visual gaps** — falsified (both resolution reports "Remaining Risk: None"). No fabricated work performed.

## Final verification

| Step | Command | Result |
|---|---|---|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 errors, 0 warnings |
| 2 | `dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 2497 PASS / 0 FAILED |
| 3 | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| 4 | `godot --headless -- --data-integrity-selftest` | PASS (3600 ids, 680 reuses, 0 errors) |
| 5 | `godot --headless -- --bridge-selftest` | exits 0 |

## Adversarial post-fix review

| Question | Outcome |
|---|---|
| Save round-trip broken? | No DTO changes. |
| Determinism changed? | No new RNG draws. Runtime warmth values change by design. |
| New event count? | No new events. |
| Cross-host drift? | Warmth propagation is Core-only with optional assignment injection; no engine coupling. |
| Did I hide a symptom? | No — documentary comments cite audit IDs. BUG-11's `shelterContaminated` still flips true, only the level stops increasing. |
| Are pre-existing tests still green? | Yes — 2497/2497 (2494 prior + 3 new, 1 semantic update). |

## Plan divergences

| Phase | Divergence | Why |
|---|---|---|
| 2 | Updated pre-existing `CompleteCycle_Bypass_IncreasesShelterContamination` test | Pinned the old pre-fix transfer; assertions now pin post-fix zero-net |
| 3 | Replaced Batch 4's explicit-Euler solve with the analytic solve + retuned `KwPerFuelUnit` | Batch 4 regression exposed numeric overshoot at 86400 s; analytic form is stable by construction; `KwPerFuelUnit = 0.05f` documented |

## Status

**4 phases planned. 4 phases resolved. Batch 5 fully CLOSED.**

Remaining host-side follow-up (next batch): wire `ShelterAssignmentSystem` into `ShelterThermalHostSession` so live Godot runs pass the optional reference into `ShelterThermalSystem` (Bug-03 propagation opt-in at runtime); separate pinned plan required.
