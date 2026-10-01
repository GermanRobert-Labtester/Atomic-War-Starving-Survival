# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Survival Legibility P009–P012 — Needs Glance, Daily Deltas, Lethal Thresholds, Acute-Rad Lesson

> **STATUS: APPROVED BY USER** (user directive: "Tackle this suggestion fully integrate
> the suggestion after run a deep sweep repair loop … after suggest minor task follow
> ups 10 of them very small! | P009 … | P010 … | P011 … | P012 …")

## Outcome

Make the survival pressure that already exists in the simulation legible to the player
without adding any parallel authority:

1. **P009** — `GameHudOverlay` gains a needs glance row (hunger / thirst / fatigue /
   morale) with per-need danger coloring.
2. **P010** — `SurvivorsPanel` per-survivor rows and the `StatusPanel` cohort section
   show a day-over-day need delta ("HUN 42 (+19/d)") from a transient baseline captured
   by the existing daily needs owner.
3. **P011** — `StatusPanel` shows the real lethal thresholds read from the owning
   constants (`NeedsProfile.hungerCritical/thirstCritical/warmthCritical`,
   `RadiationSystem.AcuteThreshold`, `RadiationSystem.HealthLossPerHourAtAcute`).
4. **P012** — a one-time contextual Medical lesson fires for day-1 acute radiation via
   the existing persisted `OnboardingJourney.RequestContextualTutorial` seam.

## Non-goals

- No new save section. The P010 baseline is a transient presentation read model and is
  cleared on restore; it never mutates needs.
- No new game rules, thresholds, or rebalancing (P013 is explicitly out of scope).
- No Unity dependency; Godot stays authoritative.
- No change to `GameHudOverlay`'s event-driven, no-`_Process` contract.

## Premise evidence (verified this session)

- `GameHudOverlay.cs` fields are day/health/rad/value/faction/weather/menu only — no needs.
- `SurvivorsPanel.RefreshRoster` renders `HP/HUN/THI/WARM/RAD` with no trend.
- `StatusPanel.RenderStats` renders cohort averages with no trend and no thresholds.
- `RadiationSystem.AcuteThreshold = 80f`, `HealthLossPerHourAtAcute = 5f`.
- `NeedsProfile.hungerCritical/thirstCritical = 90f`, `warmthCritical = 20f`.
- `SurvivorsNeedsDayOwner.TickDay` calls `_survivors.TickHour(24f)` once per day.
- `OnboardingLessonLocalization` + `TutorialPanel.ShowContextual` + `Main.Onboarding`
  already implement one-shot contextual lessons; `Main.Medical.cs` already reads
  `HasAcuteRadiationSickness`.

## Owned paths

- `Assets/Ashfall.Core/Survivors/NeedsSystem.cs` (expose readonly `Profile`)
- `Assets/Ashfall.Core/Localization/OnboardingLessonLocalization.cs`
- `src/Host/SurvivorsHostSession.cs`
- `src/Host/HoldfastRuntimeSession.cs`
- `src/UI/GameHudOverlay.cs`
- `src/UI/StatusPanel.cs`
- `src/UI/SurvivorsPanel.cs`
- `src/UI/TutorialPanel.cs`
- `src/Main.CampaignOwners.cs` (baseline capture at the needs day owner)
- `src/Main.GameFlow.cs` (`UpdateHud` needs projection)
- `src/Main.Medical.cs` (acute-rad lesson trigger)
- `Ashfall.Core.Tests/UI/OnboardingWiringGateTests.cs`
- `Ashfall.Core.Tests/Survivors/NeedsDayDeltaTests.cs` (new)
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs` (new; static/source gate)
- the plan, claim, `.ai/state.md`, `INTEGRATION_PLANS.md`

Shared paths intentionally untouched: `assets/l10n/strings.csv` (existing HUD labels are
not localized; new short labels follow the same inline convention and do not add rachet
matches), `PanelRegistryBootstrap`, `Main.PlayerSurfaces`.

## Acceptance criteria

1. HUD shows hunger/thirst/fatigue/morale with critical coloring at the owning
   thresholds; no `_Process` polling added.
2. SurvivorsPanel row text includes a `/d` delta when a baseline exists, and omits it
   when not (legacy/just-loaded).
3. StatusPanel shows a needs-drift row and a lethal-threshold legend using the real
   constants.
4. The acute-radiation contextual lesson fires at most once per campaign, only when a
   living survivor has acute radiation, and is a no-op in veteran mode / before
   onboarding is set up.
5. Build 0 errors / 0 warnings; focused xUnit gates green; `git diff --check` clean.

## Verification

- `dotnet build Ashfall.csproj`
- `bin/run-scoped-tests --filter ...` for the new/updated gates
- `git diff --check`

## Integrated follow-ups (10, all small) — DONE (2026-10-01)

All ten landed in the same wave. Evidence: host build 0/0; `NeedsDayDeltaTests` 8/8;
`StatusPanelThresholdTests` 11/11; `OnboardingWiringGateTests` 5/5;
`StringsCsvLocaleGateTests` 4/4; `LocalizationRatchetTests` 2/2; l10n drift gate PASS
(472 keys); `ui_layout_selftest` PASS (`Failures: 0`); `MainTriadDriftGateTests` 8/8;
`git diff --check` clean. Sweep repair: `UpdateHud` captured a `survivors` local after
`SetupSurvivors()` (lifecycle resets null the field) — a latent CS8602/NRE fixed; and an
old test's `Radiation.RadiationSystem` was fully qualified after a new test namespace
shadowed Core.

1. Add a **Warmth Drift** row to `StatusPanel` (`NeedsDayDeltaTracker` already tracks
   `Warmth`; only HUN/THI/FAT are rendered today).
2. Append a **MOR delta** to the `SurvivorsPanel` per-row string (morale is tracked but
   not shown).
3. Localize the HUD need-chip labels: add `ui.hud.needs.*` rows to
   `assets/l10n/strings.csv` and route `MakeNeedChip` through `AshfallLocalization.Tr`.
4. Replace the hardcoded `Hunger >= 90` / `Thirst >= 90` feedback transitions in
   `Main.GameFlow.UpdateHud` with `_survivors.Needs.Profile` criticals.
5. `NeedsSystem.OnNeedCritical` has **zero subscribers** — wire `SurvivorsHostSession` to
   it (LastEvent + RaiseStateChanged) or delete it as redundant with `OnNeedChanged`.
6. Make the HUD warn band authoritative: add `warn` fields to `NeedsProfile` (default 70)
   instead of the presentation-only `warnAt: 70` literal.
7. Give `HoldfastRuntimeSession` fallback `Fatigue`/`Morale` a decay in the no-survivors
   `TickDay` path so the HUD fallback is not static.
8. Guard `MaybeRequestAcuteRadiationLesson` with a per-session bool (or move it to the
   day-owner tick) so it is not evaluated on every `UpdateHud`.
9. Add `strings.csv` title/body rows for `expedition.protection`, `weather.storm_prep`,
   `combat.basics`, and `medical.acute_radiation` (titles currently fall back to English).
10. Verify the new `LETHAL THRESHOLDS` section fits the 680×560 `StatusPanel` at
    1920×1080 via `--ui-layout-selftest` / a snapshot on a renderer-capable host.

## Second follow-up wave (10) — INTEGRATED (2026-10-01)

All ten landed and verified: (1) `game_hud_default` snapshot target + `GameHudSnapshotFixture`
with a generated golden (diff MATCH); (2) HUD need-chip tooltips naming the critical band;
(3) `StringsCatalog_HasEverySurvivalLegibilityKey` presence gate; (4) WARM day-delta on the
roster row; (5) `NeedsProfile.healthWarn`/`healthCritical` wired through `SurvivorsPanel`,
`StatusPanel`, `SurvivalDetailPanel`, `SurvivorDetailPanel`, `Main.GameFlow`; (6)
`Needs.OnNeedCritical` emits a `FeedbackMessages` toast (display name + per-survivor dedupe)
and the duplicated player-path emits were removed; (7) `HoldfastRuntimeSession` fallback
thresholds derive from `NeedsProfile.Default*Critical`; (8) acute-rad lesson skips when
`_simDay > 1`; (9) `AddDriftRow` colours warmth inversely; (10) threshold/drift labels
localized. **5 find→repair→harden loops:** (1) health/need literals in 4 survival panels →
profile + source gate; (2) duplicated radiation `50` band → `RadiationSystem.WarnThreshold`
+ gate; (3) raw survivor id in the critical toast → display name + gate; (4) hardcoded
"50 mSv" prose → interpolated shared band + gate; (5) `-0` drift formatting → testable
`NeedsDayDeltaFormat` in Core + theory. Also repaired two concurrent-agent compile breaks
(`Variant.IsNil`, a namespace shadow) and kept the host at 0 warnings / 0 errors.

## Next 15 very small tasks (immediately integrable)

1. Localize `StatusPanel`'s remaining cohort rows (`Survivor Cohort`, `Average Health`,
   `Bunker Morale`, `Dosimetry Dose`) via `strings.csv`.
2. Localize `SurvivorsPanel`'s `RESIDENT ROSTER` / `COHORT TELEMETRY` headers.
3. Add a `SurvivorsPanel` test asserting the four filter labels resolve from `strings.csv`.
4. Extract the `Tr`/`TrFormat` private helpers into a shared `AshfallUiText` helper.
5. Add a `HoldfastRuntimeSession` fallback test proving all five fallback needs drift.
6. Gate `GameHudSnapshotFixture` against future `UpdateX` signature changes (source test).
7. Add `WarnThreshold` to the `SurvivorDetailPanel` radiation row colouring.
8. Add a `StatusPanel` drift row for morale (the tracker does not yet track it).
9. Colour `StatusPanel` `avgRad`/`avgMor` rail cards from the shared bands.
10. Add a `ui.hud.needs.warmth` tooltip assertion to the a11y source audit.
11. Emit a `FeedbackMessages` toast for the `radiation_high` warn band from the host.
12. Add a `NeedsDayDeltaFormat` case for `float.NaN`/`Infinity` returning `0`.
13. Gate the `survivors` local guard in `UpdateHud` with a source test.
14. Add a `game_hud_default` golden presence assertion to the snapshot gate tests.
15. Localize the `Main.GameFlow` low-water/low-food toast arguments via display names.

1. Add `GameHudOverlay` to `src/UI/SnapshotHarness.cs` targets so the needs glance row
   gets a golden image (currently only `ShelterHudPanel`/`CombatHudOverlay` are covered).
2. Give each HUD need chip a tooltip naming its critical threshold (hover/keyboard a11y).
3. Add a presence assertion for the four `ui.hud.needs.*` keys and the eight
   `*.title`/`*.body` lesson keys in `strings.csv` (the format gate covers shape, not
   the specific keys the code calls).
4. Add a **WARM day-delta** to the `SurvivorsPanel` roster row (the tracker tracks
   warmth; `StatusPanel` shows it but the roster does not).
5. Move the health warning band out of literals: `SurvivorsPanel` uses `< 25f` and
   `Main.GameFlow` uses `<= 25`; add `healthWarn`/`healthCritical` to the profile (or a
   shared `SurvivalThresholds`) and read them.
6. Route the new `OnNeedCritical` host fact into `FeedbackMessages` so a threshold
   crossing produces a visible alert, not just a `LastEvent` string.
7. Align `HoldfastRuntimeSession.StarvationThreshold`/`DehydrationThreshold` (const 90)
   with the profile defaults, or document why the fallback path is intentionally separate.
8. Make the acute-radiation lesson day-1 explicit: skip when `_simDay > 1` so a late
   restored campaign does not surface a stale opening warning.
9. Color the warmth drift inversely in `StatusPanel` (rising warmth is good, unlike the
   other drift rows) so the signed number reads correctly at a glance.
10. Localize the `LETHAL THRESHOLDS` labels (`Starvation`, `Dehydration`, `Hypothermia`,
    `Acute radiation`, `Chronic exposure`) through `strings.csv`.
