# ASHFALL — First-Hour Onboarding Review (teach-vs-demand)

Audit date: **2026-10-01** (T04 refresh; supersedes the 2026-09-25 edition).
Scope per `ashfall-tutorial-review` skill: the live 7-stage first-hour contract
(`OnboardingProfile.FirstHour`: Water → Power → Food → Duty → Dose → Research →
Expedition) versus the real day 1–3 pressures of the survival systems. Code and
data are truth; planning docs are intent only.

## Live contract (evidence)

- Host entry: `src/Main.Onboarding.cs` → `OnboardingJourney.CreateFirstHour()`
  (profile = FirstHour; legacy protocol stages are retained for old saves only).
- Stage definitions + requirements: `Assets/Ashfall.Core/Onboarding/OnboardingJourney.cs`
  (`FirstHourOrder` / `FirstHour[]`); state ids in `OnboardingSaveState.cs`.
- Sigils are recorded only from genuine commands (`ObserveSigil`); the journey
  never mutates gameplay state, save sections, or RNG.

## Teach-vs-demand matrix (day 1–3)

Lethal-pressure verdicts, ordered by time-to-critical. Every row cites its
producer and route.

| Pressure | Time-to-critical | Taught? | How / evidence |
|---|---|---|---|
| **Mikhail acute radiation (day 1)** | **Hours (fastest)** | **UNTAUGHT (arc) / tip only** | Starting Survivor #2 `acuteRad: true`, health 72, lifetimeDose 38 (`starting_survivors.json`). Remedy (Rad-Away/iodine in Medical) appears only in the passive `TutorialPanel` tip + Journal manual — not in the 7-stage journey and not in any live contextual lesson. **Ranked proposal #1.** |
| Thirst / untreated water | Fast (3 clean units/day Standard at `Main.CampaignOwners.cs:1364-1369`; 12-unit start → ~4-day runway) | **Yes** — stage 1 | `water.treatment_started` at `Main.ShelterInfrastructure.cs:374` (fired from `OnTreatmentStarted`); route `water_treatment` → `R(...Expanded)` + `OpenExpandedPanel` case. |
| Power loss (filtration, production) | Fast | **Yes** — stage 2 | `power.breaker_toggled` at `Main.World.cs:576` (success path only); route `power_grid` → registered descriptor + `PanelRegistry.ConfigureActions`. |
| Hunger / ration misuse | Fast (3 food/day Standard; 16 canned start) | **Yes** — stage 3 | `food.ration_consumed` at `Main.Inventory.cs:225` (consume result success, food item); route `inventory` → `Main.GameFlow.cs:491`. |
| **Unassigned duties → no work** | Immediate (functional) | **Yes** — stage 4 (added 2026-09-25) | `duty.assigned` at `Main.UiPanels.cs:241` via `DutyRosterPanel.OnAssignmentChanged`, raised only on successful mutation (T01 seal); route `duty_roster` → `Main.GameFlow.cs:689`. |
| **Dose / radon accumulation** | Slow burn, expedition-amplified | **Yes** — stage 5 (added 2026-09-25) | `dose.read` at `Main.PlayerSurfaces.cs:188/594` (dose-ledger & radiation-detail open actions); route `dose_ledger`. Note: Show-Me-Where on this stage *completes* it (the sigil IS the ledger open) — acceptable for a knowledge lesson; unlike every other stage, no further action is demanded. |
| Research stall | Medium | **Yes** — stage 6 | `research.started` at `Main.UiPanels.cs:532` (`OnResearchStarted`, success only); route `research` → `Main.GameFlow.cs:423`. Eligibility gate in `ResearchSystem.GetEligibility` (an eligible node is required — no soft lock while the starter catalog is available). |
| Expedition cost (dose, supplies, wear) | Medium | **Yes** — terminal stage 7 | `expedition.dispatched` at `Main.Expeditions.cs:230` (`OnExpeditionStarted`); route `expeditions`. Journey completes only on dispatch (never by clock tick). |
| Outdoor rad on unprotected dispatch | On first unprotected sortie | **Contextual (event-driven)** | `expedition.protection` requested by `MaybeRequestProtectionLesson` (`Main.PlayerSurfaces.cs:1016-1030`) when an estimate has `unprotectedCount > 0`; authored body in `TutorialPanel.ShowContextual`. |
| Fallout storm / black rain | Situational (severe days) | **Contextual + dashboard forecast** | `weather.storm_prep` requested by `MaybeRequestSevereWeatherLesson` (`Main.PlayerSurfaces.cs:1032-1042`); first-hour `Weather` stage deliberately deferred 2026-09-25 (low first-hour lethality; avoids over-teaching). |
| Thermal winter | Seasonal, not day 1–3 | No (deferred) | `ShelterThermalSystem` gates later; keep as a day-10+ contextual lesson. |
| Gear (gas mask / hazmat) | Before expeditions | Tip list only inside `TutorialPanel`; no live stage | Not lethal in the first hour by itself; covered by the protection lesson on first risky dispatch. |

## T04 defects found & closed (2026-10-01)

1. **Assistance-cycle infinite recursion (crash on one button press)** — closed.
   `OnboardingHintPanel.CycleAssistance()` raised `OnAssistanceChanged` while the
   host handler `Main.SetOnboardingAssistance` (wired at `Main.Onboarding.cs:95`)
   calls back into `CycleAssistance` — an unbounded synchronous loop
   (StackOverflow). **Fix:** `CycleAssistance` is now a one-way reactive update
   (label + refresh); the button handler `OnCycleAssistanceClicked` is the single
   raise site. Regression-gated by the new
   `Ashfall.Core.Tests/UI/OnboardingAssistanceLoopGateTests` (3 gates,
   TDD-proven: fails on the old raise, green after the fix). No headless UI test
   ever pressed this button, so the crash was invisible to the suite.
2. **Dead bookkeeping fields** — `Main._onboardingFailedActions` /
   `_onboardingLastInteractionSeconds` were written, never read, and the class
   comment promised a "contextual-hint heuristic" that does not exist. **Fix:**
   removed; comment made truthful. `ObserveFailedAction` now only signals the
   hint panel refresh; the panel owns presentation.
3. **Confirmed-clean wiring (re-verified, no change):** all 7 sigils have exactly
   one production producer; all 7 show-me-where routes resolve in the typed
   `PanelRegistry` or the GameFlow switch; `--onboarding-journey-selftest` PASS
   (20/20 incl. save/load resume) and the 7-stage arc passes headless.

## Tone & friction

- Copy register verified: imperative, unsentimental, no cheerleading ("Work does
  not happen by itself." / "It accumulates quietly." — `StageHintCopy`,
  `FirstHour[]`). No reward or fabricated-state introduction.
- Recovery affordances: per-stage Skip, Show-Me-Where, Dismiss, assistance
  levels, Settings → RESET TUTORIALS, and `TutorialMode = 2` (veteran) bypass in
  `SetupOnboarding`/`ApplyOnboardingSettings` — all reachable.
- **Finding (unwired):** `Main.SkipAllOnboardingStages()` has **no callers** — the
  2026-09-25 review claimed a "Skip-All" affordance; only per-step Skip is
  actually surfaced in the panel. Either wire a Skip-All into the panel action
  row or delete the seam (ranked proposal #3).
- **Finding (minor):** on journey completion the panel disables every action
  including REPLAY, so the checklist can only be re-read via
  Settings → RESET TUTORIALS. Flagged, not changed (friction decision for design).
- **Known-deferred (recorded T01, not this package):** `DailyBriefingModal`
  "SKIP [Tab]" is a dead-zone (viewport consumes Tab) — Plan 37 / C2[15] input
  parity owns it; `water_treatment` has no dashboard-rail entry after onboarding
  (hint route is the only entry) — product decision.

## Ranked remaining proposals (not implemented — content/design owners)

1. **Day-1 acute-radiation Medical lesson (UNTAUGHT_LETHAL)** — the single
   first-hour gap. Add an event-driven contextual lesson (existing
   `OnboardingJourney.RequestContextualTutorial` seam) triggered on day 1 for any
   survivor with `acuteRad`/acute-radiation status, or a Medical stage nudge:
   *"Mikhail is showing acute radiation signs. Check Medical — Rad-Away or iodine
   buys hours."* Trigger site: `Main.Medical.cs` already detects
   `HasAcuteRadiationSickness`. Owns copy → `ashfall-write`.
2. **Copy-threshold truthfulness repair (code-verifiable numbers)** —
   `TutorialPanel` basics and `codex.manual.radiation` (Core + `strings.csv` +
   `JournalPanel`) teach "Above 50 mSv, Acute Sickness takes −5 HP/hr"; code
   truth is `RadiationSystem.AcuteThreshold = 80f` on the 0–100 acute dose scale
   (health loss `HealthLossPerHourAtAcute = 5f` only at/above 80) — no 50 mSv
   acute threshold exists (the DoseLedger mSv bands are 100/300/600).
   Also "~3.6 clean units a day" vs the actual day-tick 3 (Standard) / 2 (Half).
   Reword to the real constants; content change → `ashfall-write` + l10n sync.
3. **Skip-All wiring (from the finding above)** — either expose
   `SkipAllOnboardingStages` on the hint panel action row or delete the seam.
4. **Weather stage re-entry** — re-add the legacy `Weather` stage after `Dose` if
   playtest data shows storm confusion; the contextual lesson currently carries it.
5. **Expedition protection nudge (live-verified, already event-driven)** —
   retained as-is; nothing to do beyond keeping the lesson queue length ≤1
   display + acknowledge (already true via `ContextualTutorialQueue`).
6. **Thermal first-winter teaching** — day-10+ contextual lesson, not first hour.

## Quality gate

- Every day-1..3 lethal pressure has a verdict with code/data evidence (matrix).
- The single `UNTAUGHT_LETHAL` item is Mikhail's day-1 acute radiation — ranked
  proposal #1 with trigger location; no linear stage was force-added (the arc
  stayed 7 stages; over-teaching was resisted per the 2026-09-25 rationale).
- No stage completes without a real recorded command (`Journey_NeverFabricates_ResourcesOrState`
  and the sigil-producer gate remain green); no fabricated numbers introduced by
  this audit's fixes.
- Verification this pass: new assistance-loop gate 3/3 (TDD red→green);
  `OnboardingWiringGateTests` 4/4; `OnboardingTruthfulnessGateTests` 4/4;
  `OnboardingJourneyTests` 34/34; `dotnet build Ashfall.csproj` 0 errors /
  6 pre-existing warnings; headless `--onboarding-journey-selftest` PASS (20/20);
  `git diff --check` clean; no commit; full suite not run.