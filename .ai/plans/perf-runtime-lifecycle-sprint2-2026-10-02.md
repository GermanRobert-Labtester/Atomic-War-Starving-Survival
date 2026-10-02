# PERF SPRINT 2 — per-frame process ownership + panel/session teardown

STATUS: APPROVED BY USER

> **Approval basis:** user's simulation/runtime/developer-workflow brief
> (2026-10-02), suggested order "21, 22, 31, 32, 39". This package implements
> the two bounded, evidence-backed items (31 and 32) and records why 21/22/39
> need their own packages.

## 1. Goal & Outcome

Stop per-frame polling and event-handler leaks that scale with population and
panel churn, and make the ownership contract enforceable:

- **Task 31 — process ownership.** Audit every `_Process`/`_PhysicsProcess`
  override in `src/` (11 total). Gate the ungated ones so hidden panels and
  idle actors cost nothing; add a static gate that forbids new always-on
  processors without an evidence-backed allowlist entry.
- **Task 32 — scene lifecycle teardown.** Fix the one real subscription leak
  found (a session-subscribing panel with no `_ExitTree`) and pin the
  session-subscribing panels with a teardown gate.

## 2. Evidence (current source, 2026-10-02)

| Finding | Evidence |
|---|---|
| 11 per-frame overrides in `src/` | `grep -rn "override void _Process\|_PhysicsProcess" src` |
| `CombatPanel._Process` ungated | no `SetProcess` in `src/UI/CombatPanel.cs`; runs even when `Visible == false` |
| `HoldfastTerminalPanel._Process` ungated | no `SetProcess`; cooldown timer only matters while open (`OpenTerminal` resets it) |
| `HoldfastTerminalPanel` subscription leak | `_session.StateChanged += RefreshView` in `BindSession`; **no `_ExitTree`** and no teardown unsubscribe (only a re-bind unsubscribe) |
| `SurvivorActorView._PhysicsProcess` always on | per-actor gravity/seek/animation every physics frame even after arriving and grounding |
| Survivors/sump already indexed | `SurvivorRosterSystem.Find` uses `_byId`; `SumpFloodingSystem.GetNode` is O(1) |

## 3. Non-Goals

- No spatial grid / entity-query index (Tasks 21/22) — see §7.
- No test-tier split (Task 39) — see §7.
- No gameplay, save, determinism, or content change.
- No new always-on process manager.

## 4. Claimed Paths

- `src/Host/HoldfastTerminalPanel.cs`
- `src/UI/CombatPanel.cs`
- `src/World/SurvivorActorView.cs`
- `Ashfall.Core.Tests/Tooling/ProcessOwnershipGateTests.cs` (new)
- `Ashfall.Core.Tests/Tooling/PanelTeardownGateTests.cs` (new)
- `.ai/plans/perf-runtime-lifecycle-sprint2-2026-10-02.md` (this plan)
- `WORKTREE_OWNERSHIP.md` (claim), `.ai/state.md` (handoff)

**Untouched:** sprint-1 performance files, `scripts/ci/*`, the gate manifest,
all other panels.

## 5. Implementation

1. `HoldfastTerminalPanel`: add `Unbind()` + `_ExitTree()`; gate `_Process` on
   `Visible` via `VisibilityChanged`; `BindSession` calls `Unbind()` first.
2. `CombatPanel`: gate `_Process` on `Visible` via `VisibilityChanged`; clear the
   held movement frame on hide; detach the visibility signal in `Unbind`.
3. `SurvivorActorView`: `RefreshPhysicsProcess()` enables physics only while
   settling, seeking, or visible-and-off-floor; `SetMoveTarget` re-enables;
   `UpdateFromSurvivor` refreshes.
4. `ProcessOwnershipGateTests`: every per-frame override self-gates or is
   allowlisted; allowlist is shrink-only; pins the two fixes.
5. `PanelTeardownGateTests`: session-subscribing panels have `_ExitTree` +
   unsubscribe; pins the terminal teardown path.

## 6. Verification

- [x] `scripts/run_test.sh …/ProcessOwnershipGateTests.cs` — **4/4 PASS**
- [x] `scripts/run_test.sh …/PanelTeardownGateTests.cs` — **3/3 PASS**
- [x] `scripts/run_test.sh …/PanelSubscriptionHygieneTests.cs` — **3/3 PASS**
- [x] `dotnet build Ashfall.csproj --no-restore` — **0 warnings / 0 errors**
- [x] host `--player-panels-uitest` — **PASS (22/22 lifecycle gates)**
- [x] `git diff --check` — clean

## 7. Deferred (needs its own premise audit + claim)

- **Tasks 21/22 (spatial/entity indexes):** no `SpatialGrid`/`EntityIndex` type
  exists, but the two obvious lookups are already O(1) (`SurvivorRosterSystem._byId`,
  `SumpFloodingSystem.GetNode`). A truthful spatial partition needs a real
  "nearby X" production query and a measured baseline first; building a generic
  grid with no consumer would be disconnected benchmark-only code.
- **Task 39 (test tiers):** gate tiers already exist (`run-gates.py --tier
  fast|full|performance|release`); xUnit has no `[Trait]` classification across
  ~11.7k tests. A fast/smoke subset requires a curated manifest and a runner
  change — a separate package.
- **Broader Task 32 sweep:** 123 `*Panel.cs` files subscribe to a session
  `StateChanged`; 29 have no `_ExitTree`. Many are safe because the panel owns
  the session, so the broad sweep needs a per-panel ownership decision, not a
  blind gate.
