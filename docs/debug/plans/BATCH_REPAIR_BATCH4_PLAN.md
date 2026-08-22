# Batch 4 Repair Plan

> **CRITICAL ARCHITECTURE DIRECTIVE.** Deviation from the audit's prior candidate list: this batch only closes *unfixed* items. **Six of the audit's "small surgical fixes" items are already closed at Core** in commit history on `audit/fix-batch3-plus-phases`:
> - BUG-05 (`MentalHealthCrisisStatus.Chronic`) — `MentalHealthCrisisSystem.cs:153` long-untreated forward transition.
> - BUG-08 (`equipmentDisabled` latch) — `SumpFloodingSystem.cs:242` natural-drain un-latch.
> - BUG-09 (caregiver eligibility) — `MentalHealthCrisisSystem.cs:112` `_roster.GetRoleOf()` guard with bug-history comment.
> - BUG-10 (`skillXpGrants` odd-pair OOB) — `LibraryStudySystem.cs:83-90` parity check at `LoadCatalog`.
> - BUG-14 Core-side (mentor/apprentice busy checks) — `ApprenticeshipSystem.cs:58-65` switched from `GetAssignment` (role-keyed) to `GetRoleOf` (survivor-keyed).
> - PowerGrid `ComputerTotalDraw` testability — `PowerGridSystem.cs:264` `EffectiveTotalDrawWatts` accessor exists with intent comment.
>
> Re-landing those fixes would be a silent regression. This batch is therefore shaped around what *remains* unfixed and what the audit's "design decided" cluster still requires from Core-side.

**Prior batches.** Batch 1 + Batch 2 + Batch 3 — all closed; resolution reports at `docs/debug/BATCH_REPAIR_{5BUGS,BATCH2,BATCH3}_RESOLUTION.md`.

**Source audit.** `docs/debug/10LOOP_BATCH3_AUDIT.md` (especially §6 fatal/cluster, §7–§8 HIGH/MEDIUM findings, §17 recommended investigation order).

**Branch baseline.** `b2b04212 feat(host): wire ShelterAssignment orphan + checksum sweep tests`, working tree clean per `git status`.

**Scope.** Core-only surgical + one host-side wiring consolidation (audit BUG-14 follow-up). No JSON data authoring. No UI work. No AGENTS.md edits this session.

---

## Reinvestigation method

Following the Batch 3 protocol: prior audit findings treated as hypotheses, re-checked against current source. Six items falsified (already resolved). Three remain actionable.

## Patch set

| Phase | Defect | File(s) | Class |
|---|---|---|---|
| 1 | BUG-04: thermal `*0.1f` placeholder math + `/nRooms` divisor → effective ΔT independent of room count, kW-to-°C/day physics-derived | `Assets/Ashfall.Core/ShelterThermalSystem.cs` | Core logic, magic-literal hardening |
| 2 | `ActionResult.ActionEventIdCounter` seeded from `Environment.TickCount64` (determinism invariant break; AGENTS.md Invariant 4 — no `Environment.TickCount*`, no `DateTime.Now`, no `System.Random`) | `Assets/Ashfall.Core/ActionResult.cs` | Core hardening |
| 3 | BUG-14 host-side: `Main.ExpandedShelterSystems.cs` instantiates five independent `new DutyRosterSystem()` for the five Batch 3 systems (apprenticeship, library, archive desk, contractor, mental health). The shared host-owned `_dutyRoster` exists in `Main.Holdfast.cs` but is not passed through. Per-system `mentor_busy` / `caregiver_busy` / etc. checks observe only their own empty roster. | `src/Main.ExpandedShelterSystems.cs` | Host wiring, no Core signature change |

## Defects held (design-blocked; out of scope this batch)

- **BUG-03** warmth propagation from `ShelterThermalSystem` into `NeedsSystem.Warmth` requires Core to know which survivors are in which room. Core does not have that mapping (`ThermalRoomNode` carries no `occupantIds`); the host owns it via `ShelterAssignmentSystem`. Defer to a later host-side wiring phase.
- **BUG-07** schedule `fatigueRecoveryModifier` does not exist as a field — design gap, not a Core bug. Defer.
- **BUG-11** `Decontamination.CompleteCycle(false)` net-contamination — flagged "*may be intentional*" in audit, no spec change. Defer.

## Invariants

1. Core remains engine-agnostic — no `UnityEngine.*`, `Godot.*`, `JsonUtility`, `Environment.*` for randomness.
2. RNG stays inside `ISeededRng` (host still constructs factory with deterministic seed).
3. `CaptureState/RestoreState` round-trips preserved (no DTO shape change).
4. Existing 2443+ tests stay green — new tests added per phase, none removed.
5. Determinism preserved.
6. **No regression to six previously-closed audit findings.** (Falsified list above.)

## Verification ladder

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    → 0 errors
2. dotnet build Ashfall.csproj                                  → 0 errors
3. dotnet test  Ashfall.Core.Tests/Ashfall.Core.Tests.csproj    → ≥2443 + new tests, 0 failed
4. godot --headless --path . -- --data-integrity-selftest       → 0 errors
5. godot --headless --path . -- --bridge-selftest               → exits 0
```

## Execution order

Sequential. Pre-integration checkpoint before each phase. Per-phase red→green test lifecycle. Each phase ends with verification step 1 + 2 at minimum; full ladder after all three phases land.

## Risk profile

| Phase | Core engine-coupling | New RNG | DTO shape | Save/load | New test |
|---|---|---|---|---|---|
| 1 | yes (no change) | no | no | no | +1 (`HeatGain_Physics_Matches_Manual`) |
| 2 | yes (no change) | no | no | no | +1 (`ActionEventId_FirstID_Is_DeterministicSN`) |
| 3 | n/a (host) | no | no | no | +1 (`ExpandedShelterSystems_SharesRoster_AcrossSystems`) |

## Definition of done

- 3 phases closed.
- 3 regression tests added (each fail-then-pass proven in the log).
- Full suite ≥2443 + 3, all green.
- Batch 4 resolution report at `docs/debug/BATCH_REPAIR_BATCH4_RESOLUTION.md`.
- Implementation log at `docs/debug/logs/BATCH_REPAIR_BATCH4_IMPLEMENTATION_LOG.md`.

## Rollback

Pre-phase `git checkout -- <files>` restores prior behavior. The three changes are independently revertible.
