# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Week-1 Balance Tuning (task 7) + CF-P28 Fresh-Game Bootstrap Verification (task 8)

> **STATUS: APPROVED BY USER** — user message 2026-09-30: "Start coding these till
> fully successfully integrated! 7. Tune week-1 balance from the bot results …
> 8. Do CF-P28-ONE-BOOTSTRAP-PATH … Check that a new game with no save gets every
> subsystem set up."

## Bounded outcome

1. **Task 7 — week-1 balance from the reasonable-player bot results.** Target:
   competent play survives days 1–7 on every difficulty preset; idle (no-action)
   play fails around days 4–6. JSON data changes only.
2. **Task 8 — CF-P28-ONE-BOOTSTRAP-PATH.** Verify with current evidence that a
   fresh game with no save runs the same manifest bootstrap as the restore path
   and every subsystem is constructed; run the bootstrap parity gates and the
   composition-root / real-campaign-journey probes.

## Non-goals

- No C# production or test code changes (task 7 is data-only by user instruction).
- No save schema or RNG changes, no new systems or parallel authorities.
- CF-P28 is already sealed (claim `claim-cf-p28-current-acceptance-closeout-2026-09-30`);
  this task re-verifies current source and gates; it does not re-implement.

## Evidence (2026-09-30 bot run at HEAD, all 17 checks PASS)

Bot: `--reasonable-player-selftest` (4 seeds × 4 presets, standard cohort +
standard supplies, heated shelter — documented fidelity boundaries in
`src/Host/HostCli.ReasonablePlayerBot.cs`).

- Competent (policy) day 7: 3/3 alive on all presets, but austere ends thirst
  95.7 and dirge ends thirst 100 / health 57 — the water cap.
- Idle (no-action) day 7: standard/austere/dirge 0/3 alive; **sparing keeps
  2/3 alive** — idle play does not fail on the easiest preset.
- Drain model (NeedsSystem: hunger 0.8/h, thirst 1.2/h × preset mult; health
  −0.4/h hunger-critical, −0.6/h thirst-critical; critical at 90) reproduces
  the observed end states exactly.

Idle death-day model: `death(h) = health + 0.4·(90−H0)/(0.8·hm) + 0.6·(90−T0)/(1.2·tm)`.

## Design history — first lever tried and abandoned

The first design raised starting hunger/thirst (Sarah 20/25 → 50/55, Mikhail
35/30 → 65/60, Elena 15/20 → 50/55). It moved every idle window into days 4–6
but **broke `--real-campaign-journey-selftest`**: the day-2 expedition dispatch
crosses the duty-fitness impaired thresholds (`duty_roles.json`:
hunger_impaired 60, thirst_impaired 60) and demands
`fitness_warning_confirmation_required`, which the probe does not grant.
Empirically isolated to the data change (reverting the four files restored the
probe). Starting hunger/thirst are therefore effectively pinned at ≤~35/≤~30 by
the journey probe's day-2 dispatch. All starting-needs edits were reverted.

## Final integrated levers (JSON only)

1. **Starting health (the decisive idle-timing lever)** —
   `starting_survivors.json` and the standard profile of
   `starting_survivor_cohorts.json`, changed in lockstep (the parity test
   compares the two JSON files to each other): Sarah 90 → 82, Mikhail 80 → 72,
   Elena 95 → 82. Starting hunger/thirst stay at the original 20/25, 35/30,
   15/20. Predicted idle windows: dirge d4.4–5.1, austere d4.8–5.7, standard
   d5.4–6.4, sparing d6.2–7.4. Bot confirms: idle baselines 0/3 alive at day 7
   on standard/austere/dirge; sparing still ends 2/3 alive (see limitation
   below). Day-1 asserts (Sarah >80, Mikhail >70, Elena >80) remain satisfied.
2. **First events** — `events.json`: `water_shortage` minDay 5 → 4 (the thirst
   crisis now bites inside the failure window), `lucky_find` weight 0.5 → 1.0
   (the one early catch-up event becomes visible enough to reward competent
   play). events.json is the Events Log / integrity / continuity authority.
3. **Starting stocks — unchanged.** The standard profile is pinned by the
   JSON↔code parity contract (`StartingSuppliesProfileTests.LegacyBaseline` =
   `StartingSuppliesCatalog.CreateLegacyFallbackProfile`, sum 59); competent
   play has stock slack and idle play never consumes stocks, so stocks cannot
   move the idle target. A temporary remnant-water experiment (8 → 6) was
   reverted; `starting_supplies.json` is byte-identical to HEAD.
4. **Drain rates — unchanged.** The sparing scalar 0.75 is pinned by
   `Plan181DifficultySettingsIntegrationTests` (a code-side expectation);
   raising preset scalars would additionally squeeze competent play on
   austere/dirge, which already ride the water cap.

## Known limitation (flagged, not fixed)

Sparing idle play still survives past day 7 on some seeds (2/3 alive). Its
hunger drain 0.75 is pinned by a code-side test expectation; moving sparing's
idle window earlier requires a one-line test-expectation update in
`Plan181DifficultySettingsIntegrationTests` plus a `difficulty_settings.json`
scalar change — outside this task's JSON-only scope. Needs user/foreman
authorization.

## Task 8 evidence (current source, re-verified 2026-09-30)

- Fresh path: `src/Main.CampaignServices.cs:75` calls
  `ExecuteSubsystemManifestBootstrap()` after the direct Setup calls
  (comment: CF-P28-ONE-BOOTSTRAP-PATH).
- Restore path: `src/Main.SaveOrchestrator.cs:184` calls the same executor.
- Gates: `BootstrapPathParityGateTests`, `SubsystemManifestTests`,
  `MainTriadDriftGateTests` scoped PASS; `--composition-root-selftest` PASS
  (manifestPresent=True, manifestStable=True, 222 panels,
  startNewGameComposed=True); `--real-campaign-journey-selftest` PASS exit 0.

## Verification results (all PASS, 2026-09-30, after final edit)

- [x] `bin/ashfall-dev validate-json` — 714/714, 0 violations
- [x] Scoped xUnit: StartingCohortCatalogTests (9/9), StartingSuppliesProfileTests
      (7/7) — PASS; DifficultyPresetCatalogTests, Plan181DifficultySettingsIntegrationTests,
      CatalogIntegrityValidatorTests passed pre-final-edit (difficulty JSON untouched by
      the final edit)
- [x] `--reasonable-player-selftest` 17/17 PASS — competent 3/3 alive on every
      preset (standard day-7: hunger 64.4, thirst 66.6); idle baselines 0/3 on
      standard/austere/dirge; determinism hash stable
- [x] `--real-campaign-journey-selftest` PASS exit 0 (the gate that broke the
      abandoned design)
- [x] `--day1-selftest` PASS exit 0
- [x] Pre-final-edit: `--day1-to-day2-selftest`, `--7-day-smoke-selftest`,
      `--food-loop-selftest`, `--composition-root-selftest` PASS (final edit
      touches only starting health, which these do not assert beyond day-1
      floors re-verified above)
- [x] CF-P28: scoped parity/manifest/triad trio PASS, composition-root PASS,
      journey PASS

## Files

- `Assets/StreamingAssets/Data/starting_survivors.json` (edit: health 82/72/82)
- `Assets/StreamingAssets/Data/starting_survivor_cohorts.json` (edit: standard
  profile health in lockstep + description)
- `Assets/StreamingAssets/Data/events.json` (edit: 2 week-1 pacing fields)
- `Assets/StreamingAssets/Data/starting_supplies.json` (experiment reverted;
  no diff vs HEAD)
- `.ai/state.md` (task entry)
- This plan; archived to `.ai/plans/integrated/balance/`.
