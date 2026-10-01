# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (2026-10-01: T18 pad parity, T19 manual-monitor decision formalized, T20
> hostile ammo surfacing, T21 realtime breaching with deterministic 1 s
> auto-advance, T22 seed sweep gate. 53/53 combat Core tests, build 0 errors,
> combat/journey/ui-layout selftests PASS, boot clean, `git diff --check`
> clean. Archived to `.ai/plans/integrated/combat/`.)

# Combat playability wave 2 — T18…T22 (2026-10-01)

STATUS: APPROVED BY USER
(User directive: "continue with those 5 next suggested tasks full integrate".)

## Packages

### T18 — joypad parity in CombatPanel
`src/UI/CombatPanel.cs` only, no project.godot changes (editor-mangling
hazard). Pad A (raw `JoyButton.A`) = FIRE, pad X (raw `JoyButton.X`) =
RELOAD, pad B close already works (`ashfall_close` via IsCloseOrCancel).
Left stick movement polled in `_Process` via `Input.GetJoyAxis`
(deadzone 0.25), additive with WASD. Hint text updated.

### T19 — live monitor stays MANUAL (formal decision)
Auto-showing a full-screen monitor during background encounters would
interrupt onboarding/day flow; the monitor is one [M] away. Formalized by
mentioning it in the T12 ambush/travel notices (`Main.Expeditions.cs` copy)
and recording the decision here. No auto-show machinery.

### T20 — hostile weapon legibility
`BuildSnapshot` already maps enemy weapon name/condition/jam per combatant;
the missing datum is round count. `CombatTypes.CombatantSnapshot` gains
`WeaponAmmoRemaining` (-1 = no weapon), mapped in `BuildSnapshot`; the
combatants grid weapon cell appends " [n rds]". No new authority.

### T21 — realtime breaching at the Core seam
`TacticalCombatSystem.Breaching.cs`: the three phase guards
(EvaluateBreach/BeginBreach/AdvanceBreach) now admit
`ActiveRealtime` as well as `PlayerTurn` (legacy turn-based unchanged).
`TacticalCombatSystem.Realtime.cs`: new `TickRealtimeBreach` auto-advances
every active breach once per 1.0 sim-second (`RealtimeBreachAdvanceSeconds`,
deterministic via the tick rng, default operator skill 0.5 / condition 1.0 —
the same defaults the turn-based advance uses), called from `TickRealtime`.
Note: standard ambush encounters spawn no barriers, so no UI surface is added;
scenario owners (Plan B86 seam) get a working realtime path. Proof tests.

### T22 — seeded termination sweep gate
`Ashfall.Core.Tests/Combat/RealtimeSeedSweepGateTests.cs`: deterministic
sweep across 12 seeds × 3 scenarios (unattended, auto-firing armed squad,
ammo-starved + retreat). Gate: 100% of fights reach a terminal state within
the tick cap — no seed may dead-end. Outcome distribution reported in the
assertion message as balance telemetry (no tuning authority claimed).

## Verification
Focused Core tests (new + Realtime + Breaching suites), `dotnet build
Ashfall.csproj`, `--combat-selftest`, `--real-campaign-journey-selftest`,
`--ui-layout-selftest`, headless boot, `git diff --check`.

## Claim
`claim-combat-playability-t18-t22-2026-10-01` — files:
`src/UI/CombatPanel.cs`, `Assets/Ashfall.Core/Combat/CombatTypes.cs`,
`TacticalCombatSystem.Persistence.cs` (BuildSnapshot),
`TacticalCombatSystem.Breaching.cs`, `TacticalCombatSystem.Realtime.cs`,
`src/Main.Expeditions.cs` (notice copy only), two new test files, this plan,
`.ai/state.md`, `WORKTREE_OWNERSHIP.md`.
