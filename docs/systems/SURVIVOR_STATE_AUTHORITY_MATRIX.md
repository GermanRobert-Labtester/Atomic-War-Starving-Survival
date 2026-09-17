# Survivor State Authority Matrix

Plan 24 integration reference. This matrix names the current owner for each
survivor fact and distinguishes live reads from broader work that remains.
It is not a second gameplay model or a declaration that every effect has been
migrated into the modifier stack.

| Fact | Authority | Persisted? | Read by | Mutated by | Fitness input? | Modifier-stack source? |
|---|---|---:|---|---|---:|---|
| Hunger | `NeedsSystem` / survivor needs save | Yes | HUD, survivor detail, fitness | needs tick and meal consumption | Yes | No; meals own their direct hunger transaction |
| Thirst | `NeedsSystem` / survivor needs save | Yes | HUD, fitness | needs tick and water effects | Yes | No; broad water/hygiene conversion remains open |
| Fatigue | `NeedsSystem` / survivor needs save | Yes | UI, fitness, caregiving | needs tick, schedule modifier, caregiving | Yes | Yes; schedule rate plus attributable caregiving changes |
| Warmth | `NeedsSystem` / survivor needs save | Yes | UI, fitness | needs tick and thermal consequences | Yes | Room-warmth restoration is attributed (`thermal.room_warmth`, Plan 24B A1); cold-side pressure still follows the needs tick |
| Morale | `NeedsSystem` / survivor needs save | Yes | UI, social systems | meal/social/narrative transactions | No | Meal quality (`kitchen.meal_quality`), grief (`grief.shelter_loss`), leadership (`leadership.crisis_aura`), ration conflict (`ration.confrontation`/`ration.theft`), and contagion stress (`contagion.isolation`/`contagion.pressure`) are attributed one-shot sources (Plan 24B A1); other narrative writers remain direct |
| Health | `NeedsSystem` / survivor needs save | Yes | UI, medical, fitness | treatment, illness, needs tick | Yes | Unsafe-meal penalty attributed (`kitchen.meal_unsafe`, Plan 24B A1); other treatment/illness writers remain authoritative direct writes |
| Hygiene | `NeedsSystem` / survivor needs save | Yes | UI and sanitation consumers | needs tick and hygiene actions | No | Self-care withdrawal attributed (`hygiene.self_care_withdrawn`, Plan 24B A1); water-outage hygiene decay/recovery still unmodeled |
| Radiation state | `SurvivorsHostSession` radiation authority | Yes | dose, medical, fitness | exposure and treatment | Yes; acute-radiation flag and cumulative dose | No |
| Cumulative dose | `DoseLedgerSystem` in dose-ledger save | Yes | sick list, fitness, dose UI | dosimeter readings and corrections | Yes | No |
| Quarantine / contagious state | disease authority and quarantine coordinator | Yes | medical, duty vacancy, fitness | disease progression and isolation commands | Yes | No |
| Active illness band | `SickListSystem`, driven by disease triage | Yes, in dose-ledger save | medical UI, fitness, briefing | triage / recovery release | Yes; only active illness-sourced bands | No |
| Other affliction/work restriction | source-specific affliction and medical contracts | By source owner | diagnosis/medical knowledge and source consumers | disease, radiation, trauma, or treatment owners | No unified restriction provider found; underlying numeric facts are used where currently exposed | No |
| Dependency / withdrawal | `ChemicalDependencySystem` | Yes | medical, host effects, fitness | substance use, stress, detox | Withdrawal flag only | Stress still enters the existing dependency API; not a parallel stress bar |
| Combat trauma | `CombatTraumaSystem` | Yes | medical/social/host effects, fitness | combat and trauma events | Yes; high-hypervigilance threshold | No complete general trauma modifier projection |
| Sleep assignment | `ShelterScheduleSystem` | Yes | schedule UI and daily host projection | bed/schedule commands | Indirectly, through `lastSleptDay` | Yes; eligible sleep contributes a fatigue rate |
| Last slept day | `DutyRosterSystem` row | Yes | fitness | duty-roster morning/sleep observation | Yes | No; schedule projection does not duplicate this fact |
| Skill progression | campaign-shared `SkillProgressionSystem` | Yes | skill UI, apprenticeship, production consumers | work and apprenticeship | Role skill fields are evaluated; current minima are zero until starting skills are seeded | No |
| Duty assignment | `DutyRosterSystem` | Yes | duty UI and assigned-work consumers | assignment, vacancy, invalidation | Not a fitness input; fitness gates assignment | Committed vs recommended hours are a derived projection (`DutyHourLedger`, Plan 24B A2) with measured overwork (`duty_hours_overwork`) routing data-authored fatigue/morale rates through the shared seam; hours themselves are never persisted |
| Medical admission / discharge | `MedicalWardSystem` | Yes | ward UI, fitness, duty release, briefing | admission, procedure, discharge | Active admission blocks; recent discharge adds data-authored recovery impairment | No |
| Caregiving assignment | `CaregivingSystem` | Yes | caregiving UI, duty vacancy | care commands and lifecycle events | Care eligibility reads fitness | Yes for routed fatigue/recovery transactions; no generic care-hour accumulator |
| Death / fate | `SurvivorFateSystem` plus existing survivor owners | Yes | roster, medical, memorial, briefing | authoritative fate cascade | Dead state hard-blocks | Shelter-wide grief is an attributed one-shot (`grief.shelter_loss`); bond grief is a derived decaying projection from the persisted relationship ledger (`grief.bond_loss`, Plan 24C A3); the mourning vigil is a once-per-death memorial command (`MournedDay` on the entry); bond-scaled ramp refinements and the recovery ramp remain design-note-gated |

## Integration boundary

Fitness is a derived Core projection over current needs, alive/dead state,
quarantine, infectiousness, acute-radiation status, cumulative dose, active
illness band, withdrawal, trauma, sleep recency, recent discharge, and the
campaign-shared skill projection. Role requirements live in
`Assets/StreamingAssets/Data/duty_roles.json` and are validated by both the
runtime loader and `CatalogIntegrityValidator`.

The needs modifier stack is owned by `NeedsSystem` and is not serialized as a
second copy of source state. Schedule fatigue recovery, caregiving
contributions, and the seven Plan 24B A1 one-shot families are routed through
it with stable source ids: meal quality (`kitchen.meal_quality`), unsafe meals
(`kitchen.meal_unsafe`), thermal room warmth (`thermal.room_warmth`),
self-care hygiene withdrawal (`hygiene.self_care_withdrawn`), shelter grief
(`grief.shelter_loss`), leadership morale (`leadership.crisis_aura` /
`leadership.morale`), ration conflict (`ration.confrontation` / `ration.theft`),
and contagion stress (`contagion.isolation` / `contagion.pressure`). These are
attributions over the same numeric deltas the legacy direct paths applied —
no persistent rates were added. Families with no existing need effect were not
fabricated: missed meals (hunger decay models them; §24B.11), ideological
friction morale (affects relations/affinity only, §24B.17 "if authored"), and
overwork (no duty-hour accumulator exists — Task A2's scope). Water-outage
hygiene decay (§24B.13) has no current model and remains open.

The repository currently has no ward-worker/procedure-staff assignment owner.
Fitness can block a patient from duty and can validate the existing
cook/expedition actions, but ward staffing awaits the foreman's signed choice
among the options in the Plan 24 log's Task A2 decision memo (recommendation:
data-authored ward role through the duty-roster engine). Overwork-by-hours IS
integrated as of Plan 24B Task A2: the derived `DutyHourLedger` makes the
stale-assignment gap measurable and its consequences route through the shared
needs seam with data-authored magnitudes. The duty roster panel exposes
candidate assignment, vacation, and the impaired-warning confirmation dialog.
