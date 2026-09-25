# ASHFALL — First-Hour Onboarding Review (teach-vs-demand)

Audit date: 2026-09-25. Scope per `ashfall-tutorial-review` skill: the live
first-hour onboarding contract (`OnboardingProfile.FirstHour`) versus the real
day 1–3 pressures of the survival systems. Code and data are truth; planning
docs are intent only.

## Live contract (evidence)

- Host entry: `src/Main.Onboarding.cs` → `OnboardingJourney.CreateFirstHour()`
  (profile = FirstHour; legacy protocol stages are retained for old saves only).
- Stage definitions + requirements: `Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`
  (`FirstHourOrder`, `FirstHour[]`); state ids in `OnboardingSaveState.cs`.
- Sigils are recorded only from genuine commands (`ObserveSigil`), 31 call
  sites; the journey never mutates gameplay state.

## Teach-vs-demand matrix (day 1–3)

| Pressure | Time-to-critical | Taught before this review? | Evidence |
|---|---|---|---|
| Thirst / untreated water | Fast (needs tick) | **Yes** — stage 1 `Water` ("Start a water-treatment batch") | `water.treatment_started` recorded at `Main.ShelterInfrastructure.cs:357` |
| Power loss (thermal, production stops) | Fast | **Yes** — stage 2 `Power` ("Throw a shelter breaker") | `power.breaker_toggled` at `Main.World.cs:576` |
| Hunger / ration misuse | Fast | **Yes** — stage 3 `Food` ("Eat a food ration") | `food.ration_consumed` at `Main.Inventory.cs:212` |
| **Unassigned duties → no work** | Immediate (functional) | **No (fixed)** — added stage `Duty` | `duty.assigned` already recorded at `Main.GameFlow.cs:686`, but no live stage used it |
| **Dose / radon accumulation** | Slow burn, amplified by expeditions | **No (fixed)** — added stage `Dose` | dose authority is live (`DoseLedgerHostSession`, radon in `YearOfAshSave`); no stage or hint pointed at it |
| Research stall | Medium | **Yes** — stage `Research` | `research.started` at `Main.UiPanels.cs:477` |
| Expedition cost (dose, supplies, wear) | Medium | **Yes** — final stage `Expedition` | `expedition.dispatched` at `Main.Expeditions.cs:204` |
| Weather (storm/radiation transport) | Situational | Legacy `Weather` only | live FirstHour had no weather stage; dashboard forecast remains discoverable, judged **not lethal in the first hour** — left as a ranked proposal below, not added (avoid over-teaching) |
| Thermal winter | Seasonal (not day 1–3) | No | `ShelterThermalSystem` pressure gates later; not a first-hour killer |

## Changes made (2026-09-25)

1. **`Duty` stage added** to the live first-hour order
   (`Water → Power → Food → Duty → Dose → Research → Expedition`):
   *"Put someone on shift — Open the duty roster and assign one survivor.
   Work does not happen by itself."* (route `duty_roster`; sigil `duty.assigned`).
2. **`Dose` stage added** before research/expedition:
   *"Read your dose — Open the dose ledger or radiation detail. It accumulates
   quietly."* (route `dose_ledger`; sigil `dose.read`).
3. Host wiring: `dose_ledger` and `radiation_detail` open actions now record
   `dose.read` (`src/Main.PlayerSurfaces.cs`).
4. Save-safety: enum members appended (`Duty = 12`, `Dose = 13`); state stores
   enum ids, so old saves keep their ids. `Restore` recomputes the first
   incomplete stage, so a mid-journey pre-audit save resumes at the new `Duty`
   lesson (re-teachable, skippable); a **previously completed** campaign is not
   demoted (`saved.journeyComplete || all-complete`).
5. Evidence: `OnboardingJourneyTests` 34/34 (incl. two new migration tests),
   `--onboarding-journey-selftest` PASS (host now drives the 7-stage sequence
   with save/load resume).

## Tone & friction

- New copy follows the established register: imperative, unsentimental, no
  cheerleading ("Work does not happen by itself." / "It accumulates quietly.").
- Recovery affordances unchanged: per-stage Skip, Skip-All, Show-Me-Where,
  assistance levels, and TutorialMode = 2 (veteran) bypass.
- No reward, currency, or false urgency was introduced; stages observe real
  commands only.

## Ranked remaining proposals (not implemented)

1. **Weather read for storm prep** — re-introduce the legacy `Weather` stage
   into the live order between `Dose` and `Research`, using the already-recorded
   `weather.read` sigil. Deferred: first-hour lethality is low and it lengthens
   the arc; revisit after playtest data.
2. **Expedition protection nudge** — a contextual tutorial (existing
   `RequestContextualTutorial` seam) triggered on the first unprotected dispatch
   rather than a stage; requires a protection-state query, which is owned by the
   Inventory/dose seams.
3. **Thermal first-winter teaching** — belongs to the seasonal arc, not the
   first hour; propose as a day-10+ contextual lesson.
4. **Localization keys** — `onboarding.duty.*` / `onboarding.dose.*` fall back
   to the authored English strings; add translations when the string freeze
   lifts (D22-gated).

## Quality gate

- Every day 1–3 lethal pressure has a verdict with code evidence (table above).
- No stage completes without a real recorded command; no fabricated state
  (`Journey_NeverFabricates_ResourcesOrState` still green).
