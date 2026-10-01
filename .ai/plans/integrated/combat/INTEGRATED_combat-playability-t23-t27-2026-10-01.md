# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (2026-10-01: T23 realtime bleed-out fix — 100% sweep termination, T24
> fortified den-raid obstacle content, T25 flee/breach audio cues, T26 combat
> basics contextual lesson, T27 bound-monitor snapshot golden with scoped
> regen filter. 71/71 combat Core tests; build 0 errors; combat/journey/
> ui-layout selftests PASS; boot clean; snapshot diff 2/2 match.
> Archived to `.ai/plans/integrated/combat/`.)

# Combat playability wave 3 — T23…T27 (2026-10-01)

STATUS: APPROVED BY USER

## T23 — ROOT CAUSE FIX, not tuning

Diagnostic harness against stalemate seeds 7/12 (temp test, deleted after use)
revealed a genuine Core bug, not a balance gap: `ApplyDamage` leaves a
killing-blow enemy **downed** (hp 0) rather than dead, `LivingEnemies()` keeps
downed enemies, and realtime had **no bleed-out tick** (turn-based ticks
bleed only in `EndTurn`, and only for players) — so a fight the player won
never resolved. The sweep "stalemates" were this bug. Fix:
`TacticalCombatSystem.Realtime.cs` gains `TickRealtimeBleedOut` — every downed
combatant (players and enemies, mirroring `TickBleedOut` but extended) loses a
bleed turn per deterministic 1.0 sim-second (`BleedOutIntervalSeconds`), dies
via the existing `Kill` at 0, and `CheckResolution` then resolves. Post-fix
sweep: **12/12 unattended terminate** (seeds 7/12 now Won @t60), starved-
retreat 12/12, auto-fire 12/12. Regression test:
`RealtimeFullEncounterTerminationTests.DownedLastEnemy_BleedsOut_AndResolvesWon`
(forced downed enemy → bleed events → death → victory, exactly one
`OnEncounterEnded`).

## T24 — fortified den-raid content

`CombatHostSession.StartCombat` gains optional `obstacleProfileIds` (default
null — zero change for existing callers); successful engage spawns each
profile via the existing Core `EnsureObstacleBarrier` (skip + log on unknown
profile). The Iron Raiders den raid (`Main.Muster.cs`) now fortifies the
approach with `obstacle_barricaded_gate` + `obstacle_debris_choke` — the
first production obstacle content, reachable through realtime breaching
(T21). NOTE: `CombatArenaCatalog.Load` (combat_arenas.json) remains a dormant
seam — waking it is a separate plan, not improvised here.

## T25 — combat audio completion

`AudioEventBridge.OnCombatEvent` mapped events that already had no cue:
`flee_start` → `FootstepDirt`, `breach_begin`/`breach_advance` →
`ActionRepair`, `breach_cleared` → `DangerExplosion`. Presentation-only;
Core untouched.

## T26 — combat basics contextual lesson

New const `OnboardingLessonLocalization.CombatBasicsId` ("combat.basics");
`TutorialPanel.ShowContextual` gains the authored English fallback body
(target/reload/move/retreat, honest about being shot while fleeing); trigger
site in guarded `SetupCombat`: first `encounter_start` combat event →
`_onboardingJourney?.RequestContextualTutorial(...)` — seen-once dedupe and
persistence are Core-owned.

## T27 — bound-monitor snapshot golden + scoped regen

New `src/UI/CombatHudSnapshotFixture.cs`: deterministic seeded realtime
encounter (fixed seed, 2-squad vs 3, 30 warm ticks) bound to `CombatHudOverlay`
— gates the LIVE preflight action rows. New target `combat_hud_bound`
(SnapshotHarness). To avoid mass-golden churn, `BeginSnapshotRun` now honors
`-- --ui-snapshot-ids=id1,id2` to scope a run; golden regenerated scoped:
`1/33 targets — 1 match`. Diff gate on both combat HUD targets: **2 match,
0 drift** (existing unbound golden untouched).

## Verification

71/71 combat Core tests · build 0 errors (6 pre-existing benign foreign
CS0162) · `--combat-selftest` 26/26 · `--real-campaign-journey-selftest`
PASS · `--ui-layout-selftest` PASS · boot 0 script errors ·
`git diff --check` clean · snapshot uitest 2/2 match under xvfb.

## Claim

`claim-combat-playability-t23-t27-2026-10-01` — files:
`Assets/Ashfall.Core/Combat/TacticalCombatSystem.Realtime.cs`,
`Assets/Ashfall.Core/Localization/OnboardingLessonLocalization.cs`,
`src/Host/CombatHostSession.cs`, `src/Main.Muster.cs`,
`src/Main.Expeditions.cs` (T26 trigger; sibling T18a hearing-loss hunk in the
same handler preserved), `src/Audio/AudioEventBridge.cs`,
`src/UI/TutorialPanel.cs`, `src/UI/SnapshotHarness.cs`,
`src/UI/CombatHudSnapshotFixture.cs` (new), `src/Main.Application.cs`
(snapshot id filter), `Ashfall.Core.Tests/Combat/RealtimeFullEncounterTerminationTests.cs`
(+1 test), `snapshots/combat_hud_bound.png` (new golden), this plan,
`.ai/state.md`, `WORKTREE_OWNERSHIP.md`.
