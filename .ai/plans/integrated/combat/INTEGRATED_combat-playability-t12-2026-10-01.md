# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (2026-10-01: realtime pump wired, exits proven, 3 new tests + 31/31 realtime
> suite green, combat/journey/ui-layout selftests PASS, boot 0 errors.
> Archived to `.ai/plans/integrated/combat/`.)

# T12 — Realtime combat playability pass (2026-10-01)

STATUS: APPROVED BY USER

## Task

"Realtime combat playability pass | `CombatPanel` routed; verify full encounter
doesn't dead-end"

## Forensic finding (verified in source, 2026-10-01)

`CombatPanel` IS routed (menu sidebar button → `OpenCombatPanel`,
`src/Main.UiHandlers.cs:135`; button `src/UI/MainMenuBuilder.cs:223`), but the
realtime encounter behind it dead-ends in production:

1. `CombatHostSession.StartCombat` always arms realtime
   (`TryEnableRealtime`, `src/Host/CombatHostSession.cs:470-472`), yet
   `PumpRealtime`/`SetInputFrame` have ZERO callers — `Main._Process` never
   pumps. The realtime clock never advances: enemies never act, defeat is
   unreachable, RETREAT arms a flee whose extract hold can never complete,
   END TURN is refused, breaching is phase-blocked.
2. Any encounter not won by pure firing stays active forever, and both
   expedition combat handoffs (`src/Main.Expeditions.cs:471-472,494-495`) drop
   every subsequent hostile encounter while one is unresolved.
3. Auto-spawned encounters surface nothing to the player (only `GD.Print`).
4. `CombatPanel.Bind` re-subscribes `StateChanged` on every open
   (`OpenCombatPanel` → `Bind` each click) — duplicate refresh subscriptions.
5. Post-resolution, `PumpRealtime`'s guard (`RealtimeActive` only) would keep
   no-op pumping and raising `StateChanged` 20×/s forever.

Core turn-based authority is complete and proven; **no Core gameplay change is
needed** — this is a host wiring + surfacing + proof package.

## Scope

### MUST DO

1. **Pump the realtime clock** in `Main._Process`
   (`src/Main.Application.cs`): `_combat?.PumpRealtime((float)delta)` gated on
   `GameState.Playing`, placed before the diagnostics early-return.
2. **Stop the pump at resolution** (`src/Host/CombatHostSession.cs`):
   `PumpRealtime` returns 0 when `Engine.State.Resolved`; the ctor
   `OnEncounterEnded` handler disables `_realtimePumpEnabled`.
3. **Idempotent bind** (`src/UI/CombatPanel.cs`): unsubscribe before subscribe
   in `Bind`; skip heavy `RefreshView` work while the panel is not visible
   (`Open()` refreshes explicitly on open).
4. **Surface auto-spawned combat** (`src/Main.Expeditions.cs`): both handoff
   handlers show a persistent feedback notice when an ambush starts; one
   `OnEncounterEnded` subscription (in guarded `SetupCombat`) shows the
   outcome notice. No auto-open of the panel (lifecycle-safe).
5. **Proof tests** (`Ashfall.Core.Tests/Combat/RealtimeFullEncounterTerminationTests.cs`):
   - Unattended realtime encounter reaches a terminal state within a tick cap
     (Won/Lost/Retreated), `OnEncounterEnded` exactly once.
   - Same seed → same resolution tick and outcome.
   - Ammo-starved squad can always break contact via RETREAT (realtime flee
     extract) — the starved-shooter dead-end contract.

### MUST NOT DO

- No Core gameplay/save/RNG changes.
- No movement-input adapter, bandage button, or HUD/detail/history panel
  revival (recorded as follow-ups).
- No auto-open of CombatPanel from the handoff (panel-stack risk).
- No touching foreign dirty hunks in shared files.

## Verification

- `dotnet test Ashfall.Core.Tests --filter Combat` focused realtime tests.
- `dotnet build Ashfall.csproj` (0 errors).
- `godot --headless --path . -- --combat-selftest`.
- `godot --headless --path . -- --ui-layout-selftest` (panel wiring intact).
- `godot --headless --path . -- --real-campaign-journey-selftest` (combat
  journey must still pass with the live pump).
- Headless boot `--quit-after 2`.

## Claim

`claim-combat-playability-t12-2026-10-01` — files:
`src/Main.Application.cs` (one pump call only), `src/Main.Expeditions.cs`
(notices only), `src/Host/CombatHostSession.cs` (pump guard + disable),
`src/UI/CombatPanel.cs` (bind/refresh hardening), new test file, this plan,
`.ai/state.md`, `WORKTREE_OWNERSHIP.md` claim row.
