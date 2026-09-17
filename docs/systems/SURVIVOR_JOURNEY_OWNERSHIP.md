# Survivor Journey Ownership

This document records the live ownership seams for Plan 24 — the survivor
ledger covering fitness, needs, duty, illness, caregiving, recovery, and
expeditions. The per-fact persistence/read/write map is in
[`SURVIVOR_STATE_AUTHORITY_MATRIX.md`](SURVIVOR_STATE_AUTHORITY_MATRIX.md).
It is an implementation contract, not a second gameplay model.

## Authority map

| Concern | Current authority | Integration seam |
|---|---|---|
| Survivor identity, alive/dead state | `SurvivorNeedsSystem` / survivor roster | `SurvivorFateSystem` owns terminal death cascade |
| Needs and need mutation | `NeedsSystem` | `NeedsModifierStack` adds attributable external rates |
| Fitness verdict | Core `FitnessForDutyModel` | Host builds facts from live systems; verdicts are derived, not saved |
| Duty assignment | `DutyRosterSystem` | `DutyRosterAssignmentEngine` validates role fitness and emits vacancy |
| Duty requirements | `duty_roles.json` | `FitnessRoleCatalogLoader` and `CatalogIntegrityValidator` |
| Illness/quarantine | disease and medical authorities | quarantine/admission clear duty through the existing roster seam |
| Caregiving | `CaregivingSystem` | care start clears a competing duty assignment |
| Expedition dispatch | `ExpeditionHostSession` | dispatch uses the same host fitness projection |
| Kitchen cook admission | `KitchenNutritionSystem` | prep job rejects incapacitated/unfit cooks when the host provider is bound |
| Recovery modifiers | `ShelterScheduleSystem` + `NeedsModifierStack` | sleep eligibility contributes an attributable fatigue rate |
| Daily briefing consequences | `CampaignDayCoordinator` | buffered fate and duty-vacancy events are drained by day owners |

## Fitness contract

`FitnessForDutyModel` evaluates a `FitnessEvaluationFacts` projection into a
base verdict, then applies role requirements. Hard blockers are dead,
quarantined, unconscious, medically incapacitated, acute radiation syndrome,
and active illness at the data-authored outcome-pending band. Fatigue, health,
hunger, thirst, warmth, sleep debt, dose, active illness, infection, withdrawal,
and combat trauma produce visible degraded factors or warnings. Only
illness-sourced, unreleased sick-list bands feed the illness thresholds; dose
bands remain represented by the cumulative-dose input.

The current policy is deliberately conservative at the authority boundary:

- `Fit` and `Impaired` can be assigned; impaired assignments require explicit
  confirmation through `DutyRosterHostSession.AssignDuty`.
- `Unfit` is blocked except for a data-authored light-duty role that explicitly
  opts in.
- hard-blocked survivors cannot be assigned, dispatched, or admitted as cooks.
- role verdicts are recomputed on preview and commit, so stale UI cannot bypass
  the live check.

Fitness has no save section. It is reconstructed from survivor needs, radiation
state, active disease/sick-list and medical state, dependency/trauma state,
last sleep day, recent persisted discharge day, and the shared skill authority
after load.

## State transitions

```text
roster survivor
  -> needs/radiation/medical/disease facts
  -> derived FitnessVerdict
  -> duty preview + explicit commit
  -> role work / expedition / kitchen command
  -> invalidation (admission, quarantine, death, care conflict, worsening state)
  -> existing assignment cleared + duty_vacated briefing event
```

Caregiving is a competing labor claim. Starting care clears the caregiver’s
existing duty through `DutyRosterSystem`; the caregiving system remains the
authority for care eligibility and health/fatigue effects. Medical admission
is idempotent and raises one admission signal; the host uses that signal to
clear any duty assignment for the patient. Quarantine already uses the roster
assignment API, so it receives the same vacancy callback.

## Modifier attribution

`NeedsModifierStack` stores one deterministic contribution per
`(survivor, source, need)`. Entries can have inclusive start/end day windows,
are applied only by `NeedsSystem`, and expose active and recent contributors
for UI diagnostics. The stack is not a second needs authority and is not
persisted; source systems refresh their contributions from their own state.

The shelter sleep schedule currently contributes an attributable fatigue
recovery rate. Existing systems that still apply direct need deltas remain
their own authorities and must be migrated only when their source contract is
verified; this document does not authorize duplicate labor, meal, water, or
medical ledgers.

## Data contract

`Assets/StreamingAssets/Data/duty_roles.json` is the authority for current duty
requirements. It covers the five live roles:

```text
night_watch, mess, hatch_opener, intake_sleeper, expedition
```

The current role skill minimums are zero because the live campaign does not yet
seed those skill IDs for all starting survivors. The skill IDs are nevertheless
authored and validated, so a future seeded progression can raise the minimums
without changing the roster schema.

Mechanical IDs and save shape remain unchanged. Fitness fields and modifier
entries are derived/presentation data, not an additional persistence authority.

## Deliberate repository limits

The current repository has no ward-staff assignment or procedure-worker
authority. Medical ward procedures are patient/procedure records only, so Plan
24A cannot safely invent a staffing ledger here. Likewise, there is no generic
duty-hours accumulator or precision-output authority that can be integrated
without creating a parallel system. The implemented fitness contract exposes
role maximum-hour recommendations and precision/hazard metadata for existing
consumers; those consumers must be connected when their owning work systems are
identified.

No unconscious-state producer was found in the current survivor authority, so
the model accepts the fact and blocks it when supplied without fabricating a
new status store. Recovery remains derived from existing schedule and medical
owners. Discharge currently creates a data-authored two-day impaired projection
from the persisted ward discharge day; it is not yet an affliction-specific
treatment ramp. Discharge and treatment completion continue through their
existing owners.

## Verification contract

Focused Plan 24 tests cover fitness thresholds/reason ordering, role gating and
warning acknowledgement, deterministic/timed modifier aggregation, kitchen
admission, medical admission idempotence, and survivor journey signals. Host
and Core builds must remain clean. Data changes require the existing Godot
headless data-integrity self-test.
