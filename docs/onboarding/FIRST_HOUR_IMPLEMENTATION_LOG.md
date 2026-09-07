# First-Hour Onboarding Implementation Log

## Phase 1 — State contract

Status: PASS

Changed:

- Added a migration-safe `FirstHour` onboarding profile.
- Added the five authoritative stages: water, power, food, research, expedition.
- Kept the original seven-stage profile as the default for legacy saves and direct legacy Core callers.
- Persisted the profile in `OnboardingSaveState`; missing profile fields remain legacy.

Tests:

- `OnboardingJourneyTests` — 32 passed.

Result:

- New campaigns use `OnboardingJourney.CreateFirstHour()`.
- Day advancement no longer completes the first-hour journey.
- Only the expedition dispatch signal completes the first-hour journey.

## Phase 2 — Runtime observation

Status: PASS

Changed:

- Water treatment emits a signal only after a successful treatment start.
- Power emits a signal only after a successful breaker toggle.
- Food emits a signal only after a successful inventory consume of a catalog item typed as food.
- Research emits a signal only after a successful research start.
- Expeditions emit a signal from the authoritative expedition-start event.
- Settings reset, contextual mode, and veteran mode now affect the onboarding surface.

Result:

- The onboarding tracker records evidence from existing owners. It does not mutate inventory, power, research, water, or expedition state.

## Phase 3 — Presentation and localization

Status: PASS

Changed:

- Added localized first-hour titles, objectives, hints, completion copy, and veteran-mode feedback.
- Checklist order now follows the active profile.
- Existing `Show Me Where`, skip, replay, assistance, and save/load surfaces remain available.

Tests:

- `python3 scripts/ci/l10n_drift_gate.py` — PASS.

## Known verification limitation

- `dotnet build Ashfall.csproj --no-restore` is currently blocked by an unrelated pre-existing host error: `Main.Application.cs(106)` references missing `HostCliAction.PrecisionMetrologySelfTest`.
- No onboarding-related compiler error was reported before that existing blocker.
