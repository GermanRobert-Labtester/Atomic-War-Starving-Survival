# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (2026-10-01: T13 movement adapter, T14 BANDAGE surfaced, T15 result block,
> T16 HUD overlay adopted as truthful live monitor, T17 realtime restore
> hardening. 61/61 combat-focused Core tests, build 0 errors, combat/journey/
> ui-layout selftests PASS, boot clean, goldens unaffected — orchestrator
> forces Visible=true before capture. Archived to `.ai/plans/integrated/combat/`.)

# Combat playability wave — T13…T17 (2026-10-01)

STATUS: APPROVED BY USER
(User directive: "continue with all 5 of these suggested tasks and improve
upon the known limitations" — follows T12, plan archived at
`.ai/plans/integrated/combat/INTEGRATED_combat-playability-t12-2026-10-01.md`.)

## Five bounded packages

### T13 — movement input adapter
`src/UI/CombatPanel.cs` only. While the panel is visible and the session is
realtime-active/unresolved, poll WASD/arrows (+Shift sprint) into a
`CombatInputFrame` and feed `CombatHostSession.SetInputFrame` (currently zero
callers). Reset to `CombatInputFrame.Empty` when the panel hides or the
encounter resolves, so the pump never keeps a stale frame. Host-only binding;
Core locomotion is already tested (`RealtimeLocomotionTests`).

### T14 — BANDAGE surfaced
Core `TacticalCombatSystem.cs`: new `EvaluateBandage()` preflight (needs a
downed squadmate + a standing rescuer; honest reasons). Session forwards it.
`src/UI/CombatPanel.cs`: BANDAGE button (squad = snapshot `IsPlayer` rows;
downedId = first downed squadmate; rescuer = `DefaultPlayerSubjectId()`).
Core `PlayerBandage` stays the sole authority.

### T15 — combat end-state presentation
`src/UI/CombatPanel.cs`: when the snapshot is resolved, render a result block
(aftermath survivor deaths/injuries from `snap.Aftermath` + loot lines from
`snap.Loot`) instead of only "loot N lines". No auto-close (player reads,
then CLOSE — lifecycle-safe).

### T16 — CombatHudOverlay ADOPTED as truthful live monitor
ADOPT, not delete (snapshot harness renders it; deletion would churn goldens).
`src/UI/CombatHudOverlay.cs`: idempotent `Bind`; when bound, the action-bar
grid shows LIVE preflight rows (Fire/Suppress/Clear Jam via session Evaluate*;
End Turn shows the realtime refusal honestly) instead of the hard-coded
fixture rows — fixtures remain only for the unbound snapshot path. Closed-
panel refresh skip (pump raises ~20 Hz). Routed for real: CombatPanel gets a
LIVE MONITOR button (+[M] key) → `OnMonitorRequested`; `Main.UiHandlers` opens
the overlay bound to the session; `Main.UiPanels` action lambdas surface their
result strings via `ShowAdvanceFeedback` (T10 convention).

### T17 — mid-encounter realtime save/restore hardening
Core `TacticalCombatSystem.Realtime.cs`: `SeedPoses` skips already-seeded
combatants (identical behavior on fresh encounters — only caller is
`EnableRealtime`); new `EnsureRealtimePosesSeeded()` seeds ONLY unseeded poses
of a realtime-active unresolved state (deterministic, rng null → no jitter).
`TacticalCombatSystem.Persistence.cs`: `RestoreState` calls it — legacy saves
restored mid-realtime no longer fight from degenerate zero poses.
`src/Host/CombatHostSession.cs`: `Create` re-arms the realtime pump after
restoring a realtime-active unresolved save (previously the pump stayed
disabled → the encounter froze again after every load).
Tests: `Ashfall.Core.Tests/Combat/RealtimeRestoreHardeningTests.cs` — mid-fight
restore preserves poses and continues the tick count; legacy unseeded realtime
state is seeded and immediately tickable; resolved restores untouched.

## MUST NOT DO
No save-section/schema changes; no new RNG; no movement of gameplay decisions
into UI (Core preflights decide); no golden/snapshot-harness changes; foreign
dirty hunks preserved.

## Verification
Focused: new Core tests + Realtime suite. `dotnet build Ashfall.csproj`.
`--combat-selftest`, `--real-campaign-journey-selftest` (restore path),
`--ui-layout-selftest`, headless boot, `git diff --check`.

## Claim
`claim-combat-playability-t13-t17-2026-10-01` — files:
`src/UI/CombatPanel.cs`, `src/UI/CombatHudOverlay.cs`,
`src/Host/CombatHostSession.cs`, `src/Main.UiHandlers.cs`,
`src/Main.UiPanels.cs`, `Assets/Ashfall.Core/Combat/TacticalCombatSystem.cs`,
`Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs`,
`Assets/Ashfall.Core/Combat/TacticalCombatSystem.Persistence.cs`, new test
file, this plan, `.ai/state.md`, `WORKTREE_OWNERSHIP.md`.
