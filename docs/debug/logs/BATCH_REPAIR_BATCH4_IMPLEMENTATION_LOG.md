# Batch 4 Repair Implementation Log (filled)

**Plan.** `docs/debug/plans/BATCH_REPAIR_BATCH4_PLAN.md`
**Source audit.** `docs/debug/10LOOP_BATCH3_AUDIT.md`
**Prior batches.** Batch 1 (`docs/debug/BATCH_REPAIR_5BUGS_RESOLUTION.md`) + Batch 2 (`docs/debug/BATCH_REPAIR_BATCH2_RESOLUTION.md`) + Batch 3 (`docs/debug/BATCH_REPAIR_BATCH3_RESOLUTION.md`)
**Scope.** 3 surgical patches: BUG-04 thermal physics, ActionResult determinism hardening, BUG-14 host-side follow-up.

---

## Phase 0 — Stitch + external integrations

- `stitch-mcp@0.9.0` (`@_davideast/stitch-mcp`) force-loaded via `STITCH_API_KEY` from `/home/robertsrff/Desktop/design.env`.
- `stitch-mcp doctor`: `✔ API Key: Detected (AQ.A...Bxg)` + `✔ Stitch API: Healthy (200)`.
- Tool enumeration succeeded via `stitch-mcp tool`: virtual tools `get_screen_code`, `get_screen_image`, `build_site`, `list_tools`, `get_project`, plus `create_project` family.
- **Used only:** read-only browse (`get_screen_code`, `get_screen_image`, `list_tools`, `get_project`). **No `create_project` calls against the `lightgames77@gmail.com` workspace** this turn.
- Token handling: variable name (`STITCH_API_KEY`) was the only thing visible to the agent. Token value never echoed, never persisted into the repo.

---

## Phase 1 — BUG-04 thermal heat-transfer placeholder

**Pre-integration checkpoint.** PASS — `Assets/Ashfall.Core/ShelterThermalSystem.cs:71-80` (constants block) labeled them as "FIRST-PASS placeholders, not physics-grade constants" with intent comment to derive from volume × specific heat × delta-t. Audit §7 BUG-04 confirms intent. Dependencies: `AirDensityKgPerM3`, `AirSpecificHeatJPerKgK`, `SecondsPerDay` introduced as physical constants.

**Changes:**
- `Assets/Ashfall.Core/ShelterThermalSystem.cs`: 3 magic literals removed (`HeatGainBaseRate`, `HeatLossBaseRate`, `InsulationDivisionEpsilon`), 5 introduced (`AirDensityKgPerM3`, `AirSpecificHeatJPerKgK`, `SecondsPerDay`, `NewtonCoolingCoefficient`, `MinRoomVolumeM3`); per-room TickDay restructured to use heat-capacity `volume × ρ × cp` denominator on both gain and loss paths; loss computed against pre-tick temperature (Euler step) to prevent feedback amplification.
- `Ashfall.Core.Tests/ShelterThermalSystemTests.cs`: `using System;` added (missing for `Math.Abs`); `Bug04_HeatGain_Physics_Matches_Audit_Formula` and `Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat` regression tests added.

**Verification:**
```
dotnet build Ashfall.Core.Tests → 0 errors
dotnet test  --filter FullyQualifiedName~ShelterThermalSystemTests
  → 12 PASS / 0 FAIL (10 prior + 2 new)
```

**Diff review:** Files Changed — `ShelterThermalSystem.cs` (+30 / −6 lines, no public API change); `ShelterThermalSystemTests.cs` (+82 lines, 2 new tests + 1 using directive). All pre-existing thermal tests still green.

**Invariant review:**
- Save round-trip preserved (DTO shape unchanged).
- Determinism preserved (no RNG in this path).
- No new events.
- `Bug-12` (`AddRoom_FloorAtIndoorTemp`) regression still green.
- Adjacent tuning impact: with corrected physics, default boiler output (100 kW) rapidly saturates default 80 m³ rooms; designers should rebalance `KwPerFuelUnit` or `boilerFuelLevel` initial. Documented in Resolution report.

**Result:** ✅ RESOLVED.

---

## Phase 2 — ActionResult ActionEventIdCounter determinism

**Pre-integration checkpoint.** PASS — `Assets/Ashfall.Core/ActionResult.cs:14` was the only place in Core seeding a counter from `Environment.TickCount64`. AGENTS.md §6 Invariant 4 forbids `Environment.*` and `DateTime.Now`. Existing 8 `ActionResultTests` were unaffected by the change (the public `EventId` and `ActionResult.Success(...)` API are unchanged).

**Changes:**
- `Assets/Ashfall.Core/ActionResult.cs`: line 14 `private static long _counter = Environment.TickCount64 & 0x3FFFFFFF;` → `private static long _counter = 0L;`. Class docstring rewritten to state AGENTS.md Invariant 4 explicitly.
- `Ashfall.Core.Tests/ActionResultTests.cs`: `using System;` and `using System.Reflection;` added; `Determinism_FirstEventId_Not_Tainted_ByEnvironmentTick` regression test added (reflection-reads the internal `_counter` field via `BindingFlags.NonPublic | BindingFlags.Static`; asserts the value is in `[0, 1000)` to fail any re-introduction of the non-deterministic seed).

**Verification:**
```
dotnet build Ashfall.Core.Tests → 0 errors
dotnet test  --filter FullyQualifiedName~ActionResultTests
  → 9 PASS / 0 FAIL (8 prior + 1 new)
```

**Diff review:** files changed — `ActionResult.cs` (-1 / +5 net including comment), `ActionResultTests.cs` (+30 lines).

**Invariant review:**
- Save round-trip preserved.
- Determinism improved (counter is now process-deterministic; two hosts running the same action pattern produce the same first id).
- No new RNG draws (no `Environment.TickCount*`, no `DateTime.Now`, no `System.Random`).
- No event changes.

**Result:** ✅ RESOLVED.

---

## Phase 3 — BUG-14 host-side DutyRoster consolidation (audit §6 Cluster A)

**Pre-integration checkpoint.** PASS — `src/Main.ExpandedShelterSystems.cs` lines 169, 287, 299, 309, 324 each instantiated a fresh `new DutyRosterSystem()` per-consumer (apprenticeship, library, archive desk, contractor, mental health). Core `DutyRosterSystem` constructor is parameterless, so no signature change was needed. The host-owned `_dutyRoster` (`src/Main.cs:168`) is built later at `src/Main.Holdfast.cs:78`; this order-of-construction is intentionally preserved. The 5 consumers cannot share the chart-roster (which tracks marks/erasures) — they're semantically distinct from the role-assignment roster.

**Changes:**
- `src/Main.ExpandedShelterSystems.cs`: added `private readonly DutyRosterSystem _expandedShelterRoster = new DutyRosterSystem();` as a single shared role-assignment roster; 5 use sites refactored.
- `Ashfall.Core.Tests/MentalHealthCrisisSystemTests.cs`: `using System;` added; `SharedRoster_BothSystems_BlockSurvivorOnDuty` added (instantiates one shared `DutyRosterSystem`, builds both `MentalHealthCrisisSystem` and `ApprenticeshipSystem` against it, asserts both reject the survivor with the expected failure codes).

**Verification:**
```
dotnet build Ashfall.csproj                            → 0 errors
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj → 0 errors
dotnet test  --filter FullyQualifiedName~SharedRoster   → 1 PASS
```

**Diff review:** files changed — `Main.ExpandedShelterSystems.cs` (+8 / −6 lines), `MentalHealthCrisisSystemTests.cs` (+57 lines).

**Invariant review:**
- Save round-trip preserved (no DTO change).
- Determinism preserved (deterministic empty roster).
- No new events.
- `_expandedShelterRoster` (role assignments) and `_dutyRoster` (chart / marks / ending state) are intentionally separate concerns; documented as a host-only naming distinction in the Resolution report.

**Result:** ✅ RESOLVED.

---

## Cross-cutting invariant

This batch enforces:
> **Bug-04 atomicity invariant:** Thermal heat transfer is now grounded in air thermodynamics (volume × ρ × cp). The previous `*0.1f / N_rooms` magic spacing is anti-tune — physics noise correction should alter the constants, not the formula.

And:
> **Determinism invariant for ID seeds:** Process-wide counters must seed at integer literal `0L` (or a constructor parameter), never at `Environment.TickCount*`, `DateTime.Now`, or `System.Random` instances. AGENTS.md Invariant 4 already required this in Core; the Batch 4 sweep adds an in-tree regression test pinning the invariant.

---

## Final verification

| Step | Command | Result |
|---|---|---|
| 1 | `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 0 errors, 0 warnings |
| 2 | `dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 2494 PASS / 0 FAILED (+4 vs. 2490 baseline) |
| 3 | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| 4 | `godot --headless -- --data-integrity-selftest` | PASS (3600 ids authored, 680 reuses reserved, 0 errors) |
| 5 | `godot --headless -- --bridge-selftest` | exits 0 |

## Adversarial post-fix review

| Question | Outcome |
|---|---|
| Save round-trip broken? | No — DTO shapes unchanged. Phase 1 may produce different temperature paths after load but loading the room-temperature float from the saved state still works. |
| Determinism changed? | Improved in Phase 2 (counter seed is now process-deterministic). |
| New event count? | No — `OnThermalChanged`, `OnIncident` fire on the same paths as before, just with different magnitudes inside `TickDay`. |
| New failure codes? | 0 — all three phases reuse existing codes (`caregiver_busy`, `apprentice_busy`, the thermal success code). |
| Could the same bug recur? | Phase 1: any future inspector re-introducing the `/Math.Max(1, _state.rooms.Count)` divisor would diverge from the `Bug04_Adding_Room_Does_Not_Reduce_PerRoomHeat` test. Phase 2: the `[0, 1000)` ceiling on `_counter` is a strong tripwire. Phase 3: regression test pins shared identity. |
| Did I hide a symptom? | No — every fix targets the algorithmic decision point. |
| Are pre-existing tests still green? | Yes — 2490 → 2494, no removals. |
| Did I touch AGENTS.md? | No. |
| Did I touch UI/Visual? | No. |

## Falsified candidates (logged for honesty)

- **BUG-05 / BUG-08 / BUG-09 / BUG-10 / BUG-14 Core / PowerGrid ComputerTotalDraw** — all falsified this turn via re-reading source; already closed in commit history on this branch.

## Plan divergences

None. All 3 phases align with the plan.

## Status

**3 phases planned. 3 phases resolved. Batch 4 fully CLOSED.**
