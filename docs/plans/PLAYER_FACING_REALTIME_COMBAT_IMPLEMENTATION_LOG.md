# PFGL-RT-COMBAT-TETRAD — Implementation Log

**Package:** `PFGL-RT-COMBAT-TETRAD-2026-09-25`
**DEC:** `DEC-358` SIGNED
**Evidence HEAD at start:** `1678c074`

## Phase −1 — DEC gate

Status: PASS

- Appended `DEC-358` to `docs/governance/DECISION_REGISTER.md`
- Copied plan to `docs/plans/PLAYER_FACING_REALTIME_COMBAT_PHYSICS_AI_INTEGRATION_PLAN.md`
- Claimed paths in `WORKTREE_OWNERSHIP.md` (`claim-pfgl-rt-combat-tetrad-2026-09-25`)

## Phase 1 — W1 Core clock + locomotion

Status: PASS

Changed:

- `CombatTypes.cs` — `CombatPhase.ActiveRealtime`, `CombatMotionMode`, pose fields, `CombatInputFrame`, save version 5, realtime state fields
- `CombatArenaCatalog.cs` + `combat_arenas.json` — default lane-spine arena with climb/extract
- `TacticalCombatSystem.Realtime.cs` — `EnableRealtime`, `TickRealtime` Walk/Run/Climb
- `TacticalCombatSystem.Persistence.cs` — clone/capture/migrate pose + realtime fields
- `CombatHeadlessDemo.cs` — phase clamp upper bound
- `CombatHostSession.cs` — input frame + `PumpRealtime` + auto-`TryEnableRealtime` on `StartCombat`
- `CombatPanel.cs` — removed `survivor_yuki` hardcodes (uses `DefaultPlayerSubjectId()`)
- `RealtimeLocomotionTests.cs` — 6 focused tests

Tests:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeLocomotionTests.cs
# Passed 6/6
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs
# Passed 8/8
```

Result:

- Deterministic Walk/Run/Climb under fixed dt
- Legacy EndTurn still works when `RealtimeActive=false`
- Host StartCombat arms realtime clock

Divergences:

- Arena JSON load is optional; `SeedDefaults()` is authoritative until host data-dir load is wired in a later micro-pass
- Full arena Godot view deferred to later W1 host polish / W2

Remaining:

- W2 aim/shoot cadence + ActionReload
- W3 live AI special moves
- W4 flee interrupt under fire
- CombatArenaView presentation
- Catalog integrity registration for `combat_arenas.json`

## Phase 3 — W2 Aim / shoot cadence + reload

Status: PASS

Changed:

- `CombatCatalog.cs` — optional `roundsPerMinute` / `aimBraceBonus` on weapon defs + JSON map
- `TacticalCombatSystem.Realtime.cs` — aim soft-lock, fire cooldown tick, held/pressed cadence fire through `PlayerFire`, reload input
- `TacticalCombatSystem.Actions.cs` — `PlayerFire` subject + motion accuracy scale; `PlayerReload` local mag refill when no inventory port
- `TacticalCombatSystem.cs` — `IsPlayerActionPhase` allows `ActiveRealtime`; `EvaluateReload`
- `CombatHostSession.cs` — `ActionReload` + `EvaluateReload`
- `CombatPanel.cs` — RELOAD [R] button + preflight + keybind
- `RealtimeFireCadenceTests.cs` — 5 tests
- `RealtimeReloadHostTests.cs` — 5 tests

Tests:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFireCadenceTests.cs
# Passed 5/5
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeReloadHostTests.cs
# Passed 5/5
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeLocomotionTests.cs
# Passed 6/6
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/TacticalCombatDeterminismTests.cs
# Passed 8/8
```

Result:

- Held fire expends ammo on weapon ROF cadence (derived from burst when RPM unset)
- Climb blocks fire; brace/run/walk scale accuracy into ballistics
- Reload reachable from TickRealtime input and host/panel ActionReload
- Legacy EndTurn path unchanged when RealtimeActive=false

Divergences:

- Catalog JSON does not yet author per-weapon `rounds_per_minute`; EffectiveRoundsPerMinute derives `max(120, burst*200)` until content authors values
- Hitscan via existing BallisticsSystem (no projectile travel entities) — plan-allowed v1

Remaining:

- W3 live AI special moves (enemies shoot/hunt continuously)
- W4 flee interrupt under fire (shot while running away)
- CombatArenaView presentation
- Catalog integrity registration for `combat_arenas.json`

## Phase 4 — W3 Live enemy AI

Status: PASS

Changed:

- `CombatTypes.cs` — `AiPhaseTimer` on combatants
- `TacticalCombatSystem.RealtimeAi.cs` — Burrow/Flank/Spore/Charge/SuppressiveFire/TacticalRetreat + continuous enemy fire + pin decay
- `TacticalCombatSystem.Realtime.cs` — calls `TickRealtimeAi` each tick; soft-lock skips burrowed
- `TacticalCombatSystem.Actions.cs` — PlayerFire refuses burrowed targets
- `TacticalCombatSystem.Persistence.cs` — clone `AiPhaseTimer`
- `RealtimeAiSpecialMoveTests.cs` — 7 focused tests

Tests:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeAiSpecialMoveTests.cs
# Passed 7/7
```

Result:

- Authored `AiSpecialMove` now changes pose/fire/status on the realtime clock
- Spore emits `spore_cloud` combat events (chem DeployHazard stays host-owned)
- Enemy fire is deterministic under identical seed + dt

Divergences:

- Spore does not call `ChemWarfareSystem.DeployHazard` directly (combat Core has no chem ownership); host may subscribe to `spore_cloud` events

Remaining:

- W4 flee interrupt under fire (shot while running away)
- CombatArenaView presentation
- Catalog integrity registration for `combat_arenas.json`

## Phase 5 — W4 Flee interrupt under fire

Status: PASS

Changed:

- `TacticalCombatSystem.RealtimeFlee.cs` — `RequestFlee`, extract steering, hold meter, `CompleteRealtimeExtract`
- `CombatArenaCatalog.cs` — `Contains` / `VolumeCenter` for extract volume
- `TacticalCombatSystem.Realtime.cs` — flee tick before motion; hip-fire scale while Flee; skip AI after resolve
- `TacticalCombatSystem.RealtimeAi.cs` — `flee_hit` / `flee_miss` event kinds while target flees
- `CombatHostSession.cs` — `ActionRetreat` → `RequestFlee` when realtime
- `CombatPanel.cs` — retreat tooltip reflects live extract risk
- `RealtimeFleeInterruptTests.cs` — 5 focused tests

Tests:

```bash
bash scripts/run_test.sh Ashfall.Core.Tests/Combat/RealtimeFleeInterruptTests.cs
# Passed 5/5
```

Result:

- Flee is a continuous extract under fire; enemies keep shooting
- Mid-escape hits emit `flee_hit`
- Holding the extract volume for 1.25s resolves `Retreated`
- Legacy `PlayerRetreat` roll remains for non-realtime encounters

Divergences:

- Squad-wide flee (all living players) as recommended in plan
- Downed members do not block extract hold

Remaining:

- CombatArenaView presentation (Godot arena mirror)
- Catalog integrity registration for `combat_arenas.json`
- Optional authored `rounds_per_minute` content pass
- Optional host `spore_cloud` → ChemWarfare DeployHazard bridge
