# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Combat Playability Wave 4 — T28–T32

STATUS: APPROVED BY USER
(User instruction: "tackle those suggested 5!" — chain continuation of the
combat-playability waves T12 → T27; same scope rules: full integration,
repair, hardening, polishing, focused verification only.)

## Bounded outcome

Five combat follow-ups from the wave-3 report, integrated against verified
current evidence:

- **T28 — player-hit visual feedback.** A panel-local red flash on
  `CombatPanel` when the event tail shows an enemy hit landing on a player
  combatant (`enemy_fire` / `ai_charge_hit` / `flee_hit` whose `TargetId`
  matches a player in the snapshot). Tween `Modulate` to `Theme.Critical`
  red and back, gated by `UiMotion.CanAnimate` (same reduced-motion gate as
  `UiPanelFlow.Pulse`). No screen shake — panel-local only, presentation
  only, no Core change.
- **T29 — multi-squad realtime sweep.** 3-squad-member vs 3-enemy encounter
  through the 12-seed termination sweep; deterministic bandage-under-fire
  test (down a squadmate, `PlayerBandage` from a standing rescuer, assert
  restored, pump to resolution). Also repairs the wave-3 duplication where
  `Sweep_Unattended` and `Sweep_AutoFiring` both ran `FireHeld=true` —
  unattended now genuinely holds no fire (test-policy rule 10: merge
  near-duplicates).
- **T30 — wake `combat_arenas.json`.** `CombatArenaCatalog.Load(dataDir,
  FileSystemIO, SystemTextJsonSerializer)` inside `CombatHostSession.Create`
  (same try/catch-log-and-fallback pattern as the breaching catalog load).
  Focused Core test loads the real `Assets/StreamingAssets/Data/
  combat_arenas.json` and asserts `arena_lane_spine_default` resolves with
  authored values. No location→arena mapping — no authored data exists for
  one; that is a design decision for a later plan.
- **T31 — combat loot surfacing (verification only).** Evidence: loot is
  already wired — `WireRealState` binds `grantLoot = l => Inventory.Add(
  l.itemId, l.quantity)` (CombatHostSession.cs:191) and the real-campaign
  journey selftest already proves the round trip (RealCampaignJourney.cs
  "GrantVictoryLoot -> Inventory.Add"). No duplicate test per test-policy
  rule 9. Cited in the wave report.
- **T32 — difficulty authority reaches enemy damage.** New 9th scalar
  `enemy_damage_mult` on `DifficultyScalars` (full contract: Legacy, Clone,
  Validate, provider accessor `EnemyDamageMult`, `HasSameScalarsAs`,
  `Equals`, `SetCustomScalar` case, census customized-count line,
  `RestoreState` migration for legacy custom scalars missing the key,
  `difficulty_presets.json` values, `DifficultySettingsHostSession.
  ScalarNames`, cohort panel summary, Host CLI consequence fixture, two
  test scalar constructors). Engine seam: `TacticalCombatSystem.
  EnemyDamageMultLookup` (Func<float>, default null = 1.0, clamped to the
  validated [0.25, 2.5] band) multiplied into all three enemy-dealt damage
  sites (turn-based volley `TacticalCombatSystem.Damage.cs`, realtime
  ranged fire and realtime charge `TacticalCombatSystem.RealtimeAi.cs`).
  Wired in `SetupCombat` next to the `PerformanceLookup` precedent.
  G-08-style focused test: same seed + 2.0 lookup = exactly 2× enemy hit
  damage; null lookup = 1×; legacy save normalization test.

## Non-goals

- No location→arena mapping (no authored data).
- No accuracy scaling (damage-only preserves hit/miss rhythm and
  determinism; `AiAccuracyMod` stays catalog-derived).
- No full test suite (never without explicit `RUN FULL TESTS`).
- No commits; foreign dirty hunks preserved.

## Verification

Focused: combat Core suites + difficulty suites via bin/run-scoped-tests;
host build; `--combat-selftest`, `--difficulty-settings-selftest`,
`--real-campaign-journey-selftest`; `git diff --check`.
