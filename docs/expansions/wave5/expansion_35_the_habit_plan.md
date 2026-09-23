# ASHFALL — Expansion 35 Design Bible
# THE HABIT
### Wave 5 · Dependency, Withdrawal, Pain Medicine, Care, Recovery, and Policy

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-22
**Domain owners touched:** `Ashfall.Core.Medical` (ChemicalDependencySystem, ChemicalDependencyAfflictionHandler), `Ashfall.Core.Medical` (PharmaLabSystem, PharmaceuticalTabletEngine), `Ashfall.Core.Survivors` (mental health and caregiving systems)
**Proposed host owner:** `DependencyCareHostSession` (extends medical, pharmacy, and care surfaces)
**Existing save sections:** chemical dependency ledger, medical state, mental health state
**Existing CLI verbs:** `--medical-selftest`, `--dependency-selftest` (if present), `--data-integrity-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already models dependency with real machinery.
`ChemicalDependencySystem` (25 KB) defines `ChemicalDependencyState`,
`ChemicalDependencyLedgerState`, and `SurvivorDependencyList`, and exposes
`OnSubstanceConsumed`, `ReportStress`, `BeginManagedDetox`, `BeginColdTurkey`,
`TickHours` with staffing and staff speed, `HasActiveWithdrawal`,
`DependencyLevel`, and `RestoreState`. `ChemicalDependencyAfflictionHandler`
(10 KB) connects dependency to the medical pipeline. The data file
`chemical_dependency_items.json` (2.8 KB) already defines morphine (severity
0.9), opium (0.85), prescription opioid painkillers (0.8), alcohol (0.7), and
others with dependency kinds and severities. `PharmaLabSystem` and
`PharmaceuticalTabletEngine` produce medicines from `pharma_recipes.json`
(12.9 KB). The mental health, therapy, caregiving, and grief systems of Wave 3
already exist.

What does not exist: treatment programs and staged care, taper schedules,
withdrawal symptom management as authored content, counseling and peer support,
substitution planning, recovery and relapse arcs, workplace and prescribing
policy, and any content that treats dependency as a story about people rather
than a hidden modifier.

**The Habit** turns the live dependency ledger into the shelter's hardest
medical and social problem: pain that needs medicine, medicine that creates
need, people who are hurt, and a community that decides how to care.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

The shelter's medicine cabinet is also a risk cabinet. Every strong painkiller
that saves a life can take one.

**The Habit** is the expansion about dependency: how it forms, how withdrawal
is managed, how treatment is staffed, how recovery is lived, how families and
workplaces respond, and how a shelter writes a policy that is both safe and
humane. It extends the live dependency, pharmacy, and mental health systems with
authored programs, profiles, policies, and story content — never with a second
dependency model.

The expansion's hard rules follow the live owners: `ChemicalDependencySystem`
remains the ledger and progression authority, the medical pipeline owns all
symptoms and treatments, the pharmacy owns medicine production, mental health
owns therapy, and the expansion adds only content and thin decision layers.
There is no black-market system, no contraband authority, no new currency, and
no reward for cruelty.

### 1.2 The five loops it adds

```
   Injury / pain ──► Medicine ──► Dependency forms
        │                │              │
        ▼                ▼              ▼
   Treatment      Prescribing     Withdrawal
   choices        policy          symptoms
        │                              │
        ▼                              ▼
   Managed detox ◄───────────── Care and staffing
        │                              │
        ▼                              ▼
   Taper / substitute ──► Recovery ──► Relapse ──► Recovery again
        │
        ▼
   Family ──► Community ──► Policy ──► Trust
```

### 1.3 What the player manages

1. **Pain.** Injury and illness treatment where strong medicine is sometimes
   right.
2. **Risk.** Dependency formation from repeated use, with visible levels.
3. **Withdrawal.** Hours and days of symptoms, staffing, comfort, and safety.
4. **Taper.** Managed step-down schedules versus abrupt cessation.
5. **Care.** Beds, staff, monitoring, food, water, and dignity.
6. **Recovery.** Long-term function, work return, triggers, and relapse.
7. **Family.** Households, children, grief, and the strain of caregiving.
8. **Policy.** Prescribing rules, fitness for safety-critical work, privacy,
   and accountability.

### 1.4 What it is not

- Not a second dependency system. `ChemicalDependencySystem` remains the
  authority.
- Not a contraband or black-market system. Trade of any kind stays with the
  economy authority.
- Not a punishment system. There is no flogging, no shaming, and no torture
  mechanic anywhere in this expansion.
- Not a "substances are fun" system. No glamorized use, no use-for-buffs.
- Not a realism simulator of addiction mechanics; it is a care-and-consequence
  story with authored content.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | Dependency ledger, withdrawal, detox, stress | `LIVE` |
| `Assets/Ashfall.Core/Medical/ChemicalDependencyAfflictionHandler.cs` | Medical pipeline bridge | `LIVE` |
| `Assets/Ashfall.Core/Medical/PharmaLabSystem.cs` | Medicine production | `LIVE` |
| `Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs` | Tablet dosing | `LIVE` |
| `Assets/Ashfall.Core/Survivors/SurvivorMentalHealthSystem.cs` | Mental health | `LIVE` |
| `Assets/Ashfall.Core/Shelter/CaregivingSystem.cs` | Care labor | `LIVE` |
| `Assets/Ashfall.Core/Survivors/GuiltInsomniaSystem.cs` | Sleep and guilt | `LIVE` |
| `Assets/StreamingAssets/Data/chemical_dependency_items.json` | Dependency items | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `chemical_dependency_items.json` | **2.8 KB** | few items, severities defined |
| `pharma_recipes.json` | 12.9 KB | medicines exist |
| Therapy and mental health catalogs | live | Wave 3 content |
| Programs / tapers / counseling / policy data | none | confirmed absent |

### 2.3 Confirmed gaps

- **GAP-35-1 — No treatment programs.** Detox exists as a call; staged care does
  not.
- **GAP-35-2 — No taper content.** Managed step-down schedules are unmodeled.
- **GAP-35-3 — No withdrawal symptom content.** Symptoms are not authored.
- **GAP-35-4 — No counseling or peer support.**
- **GAP-35-5 — No substitution planning.**
- **GAP-35-6 — No recovery or relapse arcs.**
- **GAP-35-7 — No family content.** Household strain is invisible.
- **GAP-35-8 — No prescribing or workplace policy.**
- **GAP-35-9 — Dependency has almost no items and no locations.**

### 2.4 Non-duplication statement

This expansion will **not** add a second dependency, medical, mental health,
pharmacy, or economy system. It extends `ChemicalDependencySystem` with authored
programs and taper plans, extends the medical pipeline with symptoms and care
orders, extends mental health with counseling content, extends the pharmacy with
protocols, and adds a thin policy layer that writes only to its own additive
sub-objects. Trade and contraband remain with the economy authority. No new save
section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Pain is real.** Strong medicine exists because people are hurt.
The expansion does not pretend the shelter can refuse to treat pain.

**Pillar 2 — Dependency is a medical condition, not a moral failing.** Care is
the response; contempt is not a mechanic.

**Pillar 3 — Withdrawal needs staff, time, and dignity.** The hardest part of
detox is the hours, and the hours need people.

**Pillar 4 — Recovery is non-linear.** Relapse is part of many recovery paths;
the shelter's policy decides whether relapse ends care or continues it.

**Pillar 5 — Policy is care written down.** Prescribing rules, confidentiality,
and work safety are the shelter protecting everyone, including the person who
is dependent.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Withdrawal | Symptoms, care, time | Horror |
| A relapse | Honest setback | Shame spiral |
| A death | Rare, grieved | Punishment |
| Counseling | Real talk | Therapy-speak |
| A taper | Math and patience | Miracle |
| Policy debate | Arguments with stakes | Lecture |

### 3.3 Content limits

- No glamorized use; substances are never presented as fun or beneficial
  beyond real medical use.
- No graphic self-harm, injection imagery, or drug-culture aesthetics.
- Children are never depicted using substances; family content focuses on care
  and stability.
- No torture, no forced withdrawal as punishment, no "tough love" reward.
- Real-world substance names are used only where the live data already uses
  them, and always in a medical register.
- Recovery is presented as possible, difficult, and worth it.

---

## 4. THE CARE WORLD

### 4.1 Interior rooms

- **`room_care_ward`** — beds, monitoring, and quiet.
- **`room_pharmacy`** — storage, dosing, and the locked cabinet.
- **`room_pharmacy_log`** — the controlled register.
- **`room_circle_room`** — peer support and group talk.
- **`room_quiet_room`** — counseling and privacy.
- **`room_kitchen_comfort`** — simple food and warm drinks.
- **`room_work_returns`** — graded return to duty.
- **`room_family_room`** — visits and support.

### 4.2 Exterior locations

| ID | Name | Danger | Purpose |
|---|---|---|---|
| `loc_poppy_field` | The Poppy Field | 4 | Medical crop source |
| `loc_chem_wreck` | The Chem Wreck | 6 | Salvage, hazard |
| `loc_old_clinic` | The Old Clinic | 5 | Medicine and equipment |
| `loc_herb_walk_med` | The Herb Walk | 3 | Comfort herbs |
| `loc_care_house` | The Care House | 2 | Step-down housing |
| `loc_court_garden` | The Court Garden | 2 | Light work and air |
| `loc_quiet_field` | The Quiet Field | 3 | Walking and recovery |
| `loc_registry_office` | The Registry | 3 | Confidential records |
| `loc_watch_post_care` | The Care Post | 4 | Observation during crisis |
| `loc_supply_cache` | The Cache | 5 | Medicine reserve |

All locations require valid item references and scanner registration.

### 4.3 The care day

Morning rounds, dosing log, group circle, light work, evening check, night watch.
The care ward's rhythm is what makes recovery possible, and the expansion makes
that rhythm playable.

---

## 5. MAIN STORYLINE — "WHAT WE COULD CARRY"

### 5.1 Central conflict

The shelter's clinic has been managing pain with morphine since the foundry
accident. **Tolan Reeve** has been on it for four months, and the dose is no
longer holding the pain — or the person. **Sur Hala**, the pharmacist, is
rationing the last of the stock. **Maeve Skell**, who ran a group before the
Exchange, wants a care ward and a policy. The steward, **Kori**, wants a rule
that keeps everyone safe and does not ruin anyone's life.

When Tolan's daughter, **Nell**, starts sleeping in the clinic waiting room
because home is worse, the shelter's abstract policy becomes a family. The
shelter must decide how to taper, how to staff the withdrawal hours, whether
Tolan keeps a safety-critical job, and what recovery actually means in a place
where every pair of hands is needed.

The expansion's question: **what does a shelter owe a person it has both hurt
and healed?**

### 5.2 Theme (unspoken)

**Care is not a reward. It is maintenance on people.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_medic_orian_vale` | Orián Vale | Medic | Pain treatment and clinical judgment |
| `npc_pharmacist_sur_hala` | Sur Hala | Pharmacist | Stock, dosing, and the register |
| `npc_counselor_maeve_skell` | Maeve Skell | Counselor | Groups, privacy, and honest talk |
| `npc_worker_tolan_reeve` | Tolan Reeve | Foundry worker | Pain, dependency, and recovery |
| `npc_daughter_nell_reeve` | Nell Reeve | Child | Family strain and stability |
| `npc_steward_kori` | Kori | Steward | Policy, work, and trust |
| `npc_peer_amos` | Amos | Peer | Recovery by example |
| `npc_elder_juna` | Juna | Elder | Comfort, memory, and grief |

### 5.4 Story beats (15)

1. **The Dose.** Tolan's medicine stops working.
2. **The Stock.** Sur counts what is left and says the number out loud.
3. **The Number.** The dependency ledger is explained to the steward.
4. **The Ward.** A care ward is proposed and argued.
5. **The Policy.** Prescribing, privacy, and work rules are debated.
6. **The Taper.** A managed step-down plan is written.
7. **The Hours.** Withdrawal begins and the hours need staffing.
8. **The Night.** A crisis night tests the ward.
9. **The Work.** Tolan's return to the foundry is negotiated.
10. **The Family.** Nell's situation forces the shelter to support households.
11. **The Relapse.** Tolan relapses and the policy is tested.
12. **The Cabinet.** A medicine theft is discovered.
13. **The Circle.** Peer support becomes a standing institution.
14. **The Year.** Recovery is measured in months, not events.
15. **What We Could Carry.** Final disposition of care and policy.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Taper | managed / abrupt / maintain | care vs. risk |
| Ward | full / partial / none | investment |
| Policy | strict / balanced / lenient | safety vs. trust |
| Work | remove / restrict / restore | safety vs. dignity |
| Privacy | sealed / staff / open | confidentiality |
| Theft | prosecute / treat / repair | justice vs. care |
| Family | support / minimal / foster | community |
| Final | care as institution / clinic / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Carried Year** — the ward, the circle, and the policy hold; Tolan works
   again and the number stays zero.
2. **The Quiet Taper** — recovery happens slowly and privately; the shelter
   learns and the policy is better for it.
3. **The Open Cabinet** — a theft collapses trust and the clinic reopens under
   watch.
4. **The Long Night** — a crisis and a death change the shelter's policy toward
   strict control and quiet grief.
5. **The Continuous Care** — some people never fully recover, and the shelter
   decides that is acceptable, and cares anyway.
6. **Fade** — the same dose continues; nobody writes anything down, yet.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_habit_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_habit_dose`, `quest_habit_stock`, `quest_habit_number`,
`quest_habit_ward`, `quest_habit_policy`, `quest_habit_taper`,
`quest_habit_hours`, `quest_habit_night`, `quest_habit_work`,
`quest_habit_family`, `quest_habit_relapse`, `quest_habit_cabinet`,
`quest_habit_circle`, `quest_habit_year`, `quest_habit_what_we_carry`.

### 6.2 Side quests (30)

**Care (5)**
- `quest_habit_ward_setup` — set up the care ward
- `quest_habit_staff_shift` — staff a withdrawal shift
- `quest_habit_comfort_food` — comfort food and drink
- `quest_habit_monitor` — monitor vitals and mood
- `quest_habit_quiet_hours` — keep the ward quiet

**Withdrawal (5)**
- `quest_habit_symptoms` — recognize and record symptoms
- `quest_habit_seizure_watch` — crisis watch
- `quest_habit_hydration` — fluids and food
- `quest_habit_sleep` — sleep support
- `quest_habit_safety` — safety during crisis

**Taper and pharmacy (5)**
- `quest_habit_taper_plan` — write a taper schedule
- `quest_habit_dose_log` — keep the dosing register
- `quest_habit_stock_count` — count and secure the stock
- `quest_habit_substitute` — plan a substitution
- `quest_habit_reserve` — build a medical reserve

**Recovery (5)**
- `quest_habit_circle_start` — start the peer circle
- `quest_habit_triggers` — map personal triggers
- `quest_habit_work_return` — graded return to duty
- `quest_habit_relapse_plan` — write the relapse plan
- `quest_habit_year_mark` — mark a year of recovery

**Family and community (5)**
- `quest_habit_family_talk` — a family conversation
- `quest_habit_child_care` — support a child in the household
- `quest_habit_household` — stabilize a household
- `quest_habit_peer_pair` — pair a peer supporter
- `quest_habit_stigma` — address stigma in the shelter

**Policy (5)**
- `quest_habit_write_policy` — draft the care policy
- `quest_habit_work_rule` — safety-critical work rules
- `quest_habit_privacy_rule` — confidentiality rules
- `quest_habit_hear_case` — hear a discipline case
- `quest_habit_review_year` — review the policy yearly

### 6.3 Repeatable quests (8)

`quest_habit_repeat_round`, `quest_habit_repeat_shift`,
`quest_habit_repeat_count`, `quest_habit_repeat_circle`,
`quest_habit_repeat_check`, `quest_habit_repeat_talk`,
`quest_habit_repeat_log`, `quest_habit_repeat_work`.

### 6.4 Dynamic hooks

Live events (substance consumption, dependency level changes, stress reports,
withdrawal ticks, injuries, medicine production, work accidents) attach authored
follow-ups through existing seams. No new event bus.

### 6.5 Constraints

- Dependency progression remains in `ChemicalDependencySystem`.
- Symptoms and treatments route through the medical pipeline.
- Medicines come from the pharmacy; no conjuring.
- Mental health content extends the live systems.
- Policy writes only to its additive sub-objects.
- No black-market, contraband, or trade system.
- No torture, punishment, or shaming mechanic.
- No child substance content.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `DependencyCareSystem` (new, `Ashfall.Core.Medical`)

**Owns:** care programs, ward state, staffing, comfort, and monitoring during
withdrawal and detox. **Consumes:** `ChemicalDependencySystem` state,
`CaregivingSystem`, `MedicalPipelineCoordinator`, `DutyRoster`, `Inventory`,
`NeedsSystem`. **Data:** `dependency_programs.json`.
**Rules:** care requires staffed hours and real supplies; staffing quality
affects drift and comfort through the live tick; the ward is a place, not a
button.

### 7.2 `TaperSystem` (new, `Ashfall.Core.Medical`)

**Owns:** taper schedules, step-down math, substitution plans, and adherence.
**Consumes:** `ChemicalDependencySystem` levels, pharmacy stock,
`PharmaceuticalTabletEngine`, `DependencyCareSystem`. **Data:**
`taper_schedules.json`, `substitution_plans.json`.
**Rules:** tapers are written, followed, and adjusted; abrupt cessation is
possible and riskier; substitution is a real option with real tradeoffs.

### 7.3 `WithdrawalCareSystem` (new, `Ashfall.Core.Medical`)

**Owns:** symptom content, comfort orders, crisis watch, and safety protocols.
**Consumes:** `ChemicalDependencySystem` withdrawal state,
`MedicalPipelineCoordinator`, `NeedsSystem`, `DependencyCareSystem`.
**Data:** `withdrawal_profiles.json`. **Rules:** symptoms are authored and
treatable; crisis moments are handled procedurally; no symptom is a spectacle.

### 7.4 `RecoverySystem` (new, `Ashfall.Core.Survivors`)

**Owns:** recovery timelines, function restoration, work return, triggers, and
relapse handling. **Consumes:** dependency state, mental health, work systems,
`SkillProgressionSystem`. **Data:** `recovery_plans.json`.
**Rules:** recovery is measured in months and non-linear; work return is graded;
relapse is authored and survivable.

### 7.5 `PeerSupportSystem` (new, thin, `Ashfall.Core.Survivors`)

**Owns:** groups, peer pairs, circle content, and stigma reduction.
**Consumes:** `SurvivorMentalHealthSystem`, `CaregivingSystem`, `NeedsSystem`,
`DutyRoster`. **Data:** `counseling_programs.json`.
**Rules:** support is people and privacy; group content is authored; stigma is a
real social effect with real costs.

### 7.6 `CarePolicySystem` (new, thin, `Ashfall.Core`)

**Owns:** prescribing rules, confidentiality, safety-critical work rules, and
review cycles. **Consumes:** `PolicySystem`, `DutyRoster`,
`ChemicalDependencySystem` levels, `PharmaceuticalTabletEngine` logs.
**Data:** `dependency_policies.json`. **Rules:** policy is written, published,
reviewed, and enforced by people; every policy has tradeoffs and visible
consequences.

### 7.7 Systems explicitly not added

- No second dependency, medical, mental health, or pharmacy system.
- No black market, contraband, or trade authority.
- No glamorized use or use-for-buffs.
- No punishment, torture, or shaming mechanics.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `dependency_programs.json` (new)

```json
{
  "schema_version": 1,
  "programs": [
    {
      "program_id": "program_managed_taper",
      "display_name": "Managed Taper",
      "staff_hours_per_day": 12,
      "supplies": ["comfort_kit", "herbal_tea"],
      "duration_days": 28,
      "risk_modifier": -0.35,
      "relapse_modifier": -0.20,
      "tags": ["opioid", "staffed", "slow"]
    }
  ]
}
```

### 8.2 `taper_schedules.json` (new)

Schedules: substance kind, starting dose, step size, step days, hold rules, and
completion criteria.

### 8.3 `withdrawal_profiles.json` (new)

Profiles: kind, onset hours, peak hours, symptom list, severity bands, crisis
risk, and care orders.

### 8.4 `substitution_plans.json` (new)

Plans: substitute item, conversion, duration, access, supply risk, and outcome.

### 8.5 `recovery_plans.json` (new)

Plans: phase, duration, function goals, work grade, trigger list, and relapse
rule.

### 8.6 `counseling_programs.json` (new)

Programs: circle, one-to-one, family session, peer pair, with hours and effects.

### 8.7 `dependency_policies.json` (new)

Policies: prescribing limits, privacy level, work restrictions, review cycle,
and enforcement.

### 8.8 `stress_profiles.json` (new)

Stress sources: grief, injury, hunger, cold, work, and their dependency
interactions.

### 8.9 Extend `chemical_dependency_items.json`

Additional dependency-listed medicines and comfort items consistent with the
live schema.

### 8.10 Items

New items appended to `items.json`: `item_dose_measure`, `item_taper_schedule`,
`item_comfort_kit`, `item_pain_journal`, `item_circle_note`, `item_pharma_log`,
`item_locked_cabinet`, `item_herbal_tea`, `item_soft_blanket`,
`item_comfort_meal`, `item_quiet_lamp`, `item_monitor_board`,
`item_care_protocol`, `item_family_letter`, `item_recovery_mark`,
`item_medicine_reserve`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

The chemical dependency ledger and medical state remain the live save owners.
New sub-objects (care programs, tapers, withdrawal care, recovery plans, peer
support, policies) are additive inside them. No new save section.

### 9.2 State to persist

- Active care program and ward state.
- Taper schedule and adherence.
- Withdrawal severity and care orders.
- Recovery phase, work grade, and triggers.
- Peer support participation.
- Policy definitions and review dates.
- Stock counts and register entries.

### 9.3 Determinism

- Dependency formation and withdrawal ticks remain in
  `ChemicalDependencySystem`.
- Care and staffing modify the live tick parameters, never replace them.
- Taper adherence and relapse resolve deterministically from authored rules and
  treatment quality; any randomness uses the live seeded path.
- Paired replay hashes must match.

### 9.4 Migration

Legacy saves load with existing dependency and medical state untouched; no care
program, taper, recovery, support, or policy state exists until started.
Existing dependency levels and withdrawal states keep working.

### 9.5 Checksum

Invariant-culture floats; integer days and permille where schemas allow.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `CareWardPanel` (new) | Beds, staff, comfort, watch | `DependencyCareHostSession` |
| `TaperPanel` (new) | Schedules and adherence | same |
| `WithdrawalPanel` (new) | Symptoms and care orders | same |
| `RecoveryPanel` (new) | Phases, work grade, triggers | same |
| `CirclePanel` (new) | Groups and peers | same |
| `CarePolicyPanel` (new) | Rules and reviews | same |
| `PharmacyPanel` (extend) | Dosing register | existing |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Dependency levels are shown as medical numbers with context, never as shame
  meters.
- Withdrawal care lists exactly what is needed and who is assigned.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Content is text-forward and never relies on distressing imagery.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a quiet ward, a kettle, a door
closing softly, a page turning in the register, a circle starting. No cue is
required; text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `ChemicalDependencySystem` | Ledger, withdrawal, detox |
| `ChemicalDependencyAfflictionHandler` | Medical bridge |
| `MedicalPipelineCoordinator` | Symptoms and treatments |
| `PharmaLabSystem` / tablet engine | Medicines and dosing |
| `SurvivorMentalHealthSystem` | Counseling and stress |
| `CaregivingSystem` | Staffing and comfort |
| `GuiltInsomniaSystem` | Sleep and grief |
| `NeedsSystem` | Hunger, morale, fatigue |
| `DutyRoster` | Care shifts |
| `SkillProgressionSystem` | Work return |
| `PolicySystem` | Written care policy |
| `TradingSystem` | Supply acquisition (no contraband) |
| `NoticeSystem` (Wave 4) | Policy notices |
| `SchoolingSystem` (Wave 4) | Health education |
| `EpilogueChronicleBuilder` | Care milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm `ChemicalDependencySystem`,
affliction handler, pharmacy systems, mental health, caregiving, dependency item
data, and medical pipeline API. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author programs, tapers, withdrawal profiles,
substitutions, recovery plans, counseling programs, policies, stress profiles;
extend dependency items; append items. Register validators and scanner.

**Phase 2 — Pure Core.** `DependencyCareSystem`, `TaperSystem`,
`WithdrawalCareSystem`, `RecoverySystem`, `PeerSupportSystem`,
`CarePolicySystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `DependencyCareHostSession`, selftest coverage, fresh
journey.

**Phase 5 — UI.** New and extended panels with lifecycle and accessibility.

**Phase 6 — Content.** Locations, rooms, NPCs, quests, items, prose, audio,
with a dedicated sensitivity review.

**Phase 7 — Balance.** 360-day soak: injury, treatment, dependency, taper,
relapse, recovery, and policy.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Care programs | 8 |
| Taper schedules | 10 |
| Withdrawal profiles | 8 |
| Substitution plans | 6 |
| Recovery plans | 10 |
| Counseling programs | 8 |
| Policies | 10 |
| Stress profiles | 12 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 50,000–65,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Second dependency system | Critical | Extend live ledger |
| Glamorization | Critical | Tone rules, sensitivity review |
| Punishment/shame framing | Critical | Explicit exclusion |
| Contraband duplication | High | Economy authority untouched |
| Medical horror | High | Restraint review |
| Child content mishandled | High | Family-focused, no use |
| Determinism break | Low | Live seeded ticks |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `dependency_programs.json` | 8 | 2,500 |
| `taper_schedules.json` | 10 | 2,500 |
| `withdrawal_profiles.json` | 8 | 3,000 |
| `substitution_plans.json` | 6 | 2,000 |
| `recovery_plans.json` | 10 | 3,000 |
| `counseling_programs.json` | 8 | 2,500 |
| `dependency_policies.json` | 10 | 3,500 |
| `stress_profiles.json` | 12 | 2,500 |
| Quest objectives | 53 quests | 14,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,000 |
| Ending prose | 6 | 3,000 |
| Sensitivity notes | full pass | 2,000 |
| **Total** | | **~55,000** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R35-1 | Second dependency system | Low | Critical | One ledger owner |
| R35-2 | Glamorization | Med | Critical | Tone review |
| R35-3 | Shame mechanics | Low | Critical | Exclusion rule |
| R35-4 | Contraband duplication | Med | High | Economy untouched |
| R35-5 | Medical horror | Med | High | Restraint |
| R35-6 | Child framing | Med | High | Family-focused only |
| R35-7 | Recovery too easy | Med | Med | Months and relapses |
| R35-8 | Determinism | Low | High | Live ticks |
| R35-9 | Content overrun | Med | Med | Budget §13 |
| R35-10 | Policy ignored | Med | Med | Enforcement and tradeoffs |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Should withdrawal always be survivable?** Recommended: yes with care; the
   risk comes from neglect, not from dice.
2. **Can someone be removed from safety-critical work permanently?**
   Recommended: temporarily with review; permanent only with a hearing.
3. **Does relapse end recovery?** Recommended: no; it resets a phase and costs
   trust.
4. **Is a substitution program always better?** Recommended: no; supply and
   access make it a real tradeoff.
5. **Who can see the register?** Recommended: policy-defined; sealed by default
   with clinical exceptions.

---

## 17. APPENDIX D — CARE PROGRAM TABLE (8 PROGRAMS)

| # | Program | Staff/day | Supplies | Days | Risk | Relapse |
|---|---|---|---|---|---|---|
| 1 | Managed Taper | 12h | comfort, tea | 28 | −35% | −20% |
| 2 | Staffed Detox | 16h | comfort, food | 14 | −40% | −15% |
| 3 | Cold Turkey | 4h | water | 7 | +20% | +10% |
| 4 | Substitution | 8h | substitute | 60 | −30% | −25% |
| 5 | Home Care | 6h | comfort | 21 | −15% | −5% |
| 6 | Ward Care | 20h | full | 21 | −45% | −30% |
| 7 | Peer-Led | 4h | none | 42 | −20% | −15% |
| 8 | Clinic Referral | 0h | transport | 30 | −25% | −10% |

Programs are real labor commitments. The ward costs staff-hours the shelter does
not get back, and that cost is the expansion's central trade.

---

## 18. APPENDIX E — WITHDRAWAL PROFILE TABLE (8 PROFILES)

| # | Profile | Kind | Onset | Peak | Crisis | Care |
|---|---|---|---|---|---|---|
| 1 | Opioid Light | opioid | 12h | 48h | low | comfort, hydrate |
| 2 | Opioid Heavy | opioid | 8h | 60h | med | ward, watch |
| 3 | Opioid Crisis | opioid | 4h | 36h | high | full ward |
| 4 | Alcohol Light | alcohol | 6h | 24h | low | food, rest |
| 5 | Alcohol Heavy | alcohol | 4h | 36h | high | watch, sedate |
| 6 | Alcohol Crisis | alcohol | 2h | 24h | extreme | full care |
| 7 | Mixed Use | mixed | 6h | 48h | high | ward |
| 8 | Chronic Low | any | 24h | 72h | low | support |

Withdrawal is authored as a clinical timeline, not a moral test. The crisis
bands exist so that the player can plan staff and supplies before the first bad
night.

---

## 19. APPENDIX F — TAPER SCHEDULE TABLE (10 SCHEDULES)

| # | Schedule | Substance | Start | Step | Days | Hold rule |
|---|---|---|---|---|---|---|
| 1 | Standard Opioid | opioid | full | 10% | 28 | hold if crisis |
| 2 | Slow Opioid | opioid | full | 5% | 56 | hold if insomnia |
| 3 | Fast Opioid | opioid | full | 20% | 14 | hold if severe |
| 4 | Alcohol Standard | alcohol | full | 15% | 21 | hold if tremor |
| 5 | Alcohol Slow | alcohol | full | 8% | 42 | hold if crisis |
| 6 | Mixed Standard | mixed | full | 10% | 35 | hold if any |
| 7 | Maintenance | any | stable | 0% | open | clinical review |
| 8 | Substitution Ramp | opioid | low | increase | 14 | hold if over |
| 9 | Substitution Taper | substitute | full | 10% | 60 | hold if crisis |
| 10 | Comfort Only | any | none | n/a | 21 | watch daily |

Tapers are written down so that every step is a decision the shelter makes
together, not a number that drifts because nobody is counting.

---

## 20. APPENDIX G — SUBSTITUTION PLAN TABLE (6 PLANS)

| # | Plan | From | To | Conversion | Days | Supply risk |
|---|---|---|---|---|---|---|
| 1 | Partial Sub | opioid | partial dose | 1:0.6 | 60 | med |
| 2 | Full Sub | opioid | substitute | 1:1 | 90 | high |
| 3 | Herbal Comfort | any | herbs | n/a | 21 | low |
| 4 | Alcohol Cover | alcohol | tapered dose | 1:0.8 | 14 | med |
| 5 | Sleep Aid | any | sleep mix | n/a | 30 | low |
| 6 | Staged Exit | any | none | step | 90 | low |

Substitution is the expansion's most realistic medicine: it keeps a person
functional while their body adjusts, and it depends entirely on supply. A
substitution program that runs out is a withdrawal the shelter chose.

---

## 21. APPENDIX H — RECOVERY PLAN TABLE (10 PLANS)

| # | Plan | Phase | Days | Work grade | Triggers |
|---|---|---|---|---|---|
| 1 | First Week | stabilize | 7 | none | stress, pain |
| 2 | First Month | adjust | 30 | light | work, crowds |
| 3 | Third Month | rebuild | 60 | half | injury, grief |
| 4 | Half Year | return | 90 | full-grade | bad nights |
| 5 | Year Mark | maintain | 180 | full | anniversaries |
| 6 | Relapse Reset | restart | 14 | none | shame, isolation |
| 7 | Chronic Care | ongoing | open | adapted | all |
| 8 | Family Track | with care | 90 | varies | home stress |
| 9 | Work Track | graded | 120 | graded | job pressure |
| 10 | Peer Track | group | 180 | varies | isolation |

Recovery is a phase system with a work grade and a trigger list. It is
non-linear on purpose: the plan names where a person is, not whether they have
won.

---

## 22. APPENDIX I — COUNSELING PROGRAM TABLE (8 PROGRAMS)

| # | Program | Format | Hours | Crowd | Effect |
|---|---|---|---|---|---|
| 1 | Peer Circle | group | 1.5 | 6 | support |
| 2 | One to One | private | 1 | 1 | trust |
| 3 | Family Session | family | 1.5 | 3 | stability |
| 4 | Work Talk | role | 1 | 2 | reintegration |
| 5 | Grief Share | group | 1.5 | 8 | mourning |
| 6 | Sleep Class | group | 1 | 8 | rest |
| 7 | Stress Walk | outdoor | 1 | 4 | calm |
| 8 | Quiet Hour | solo | 1 | 0 | reflection |

Counseling is people in a room with a purpose. The expansion keeps it modest,
private, and optional, and never turns therapy into a buff that requires
attendance.

---

## 23. APPENDIX J — POLICY TABLE (10 POLICIES)

| # | Policy | Prescribing | Privacy | Work | Review |
|---|---|---|---|---|---|
| 1 | Strict | logged, limited | sealed | restricted | quarterly |
| 2 | Balanced | logged | staff | graded | half-yearly |
| 3 | Lenient | open | open | free | yearly |
| 4 | Clinical | medic only | sealed | clinical | quarterly |
| 5 | Steward | steward sign | staff | restricted | quarterly |
| 6 | Open Register | logged, public | open | graded | yearly |
| 7 | Family First | logged | family | graded | half-yearly |
| 8 | Work Safety | logged, capped | staff | safety rule | quarterly |
| 9 | Care First | logged | sealed | adapted | yearly |
| 10 | No Policy | none | none | none | none |

Every policy reflects a different answer to the same question, and every one has
a cost. The expansion makes the shelter's choice visible in the clinic, the
workplace, and the family.

---

## 24. APPENDIX K — STRESS PROFILE TABLE (12 SOURCES)

| # | Stress | Magnitude | Dependency link | Mitigation |
|---|---|---|---|---|
| 1 | Grief | high | risk up | circle, ritual |
| 2 | Injury | high | pain meds | care |
| 3 | Hunger | med | risk up | ration |
| 4 | Cold | med | risk up | warmth |
| 5 | Overwork | med | risk up | roster |
| 6 | Insomnia | med | risk up | sleep class |
| 7 | Fear | med | risk up | drills |
| 8 | Guilt | high | risk up | counsel |
| 9 | Isolation | med | risk up | circle |
| 10 | Work loss | high | risk up | graded return |
| 11 | Anniversary | high | risk up | support |
| 12 | Chronic pain | high | pain meds | protocol |

Stress is the bridge between the shelter's harsh world and dependency. The
expansion does not moralize stress; it names the sources and gives the shelter
ways to lower them.

---

## 25. APPENDIX L — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_habit_dose` | 4 | Medicine stops working |
| `quest_habit_stock` | 4 | Count the stock |
| `quest_habit_number` | 4 | Explain the ledger |
| `quest_habit_ward` | 5 | Argue and build the ward |
| `quest_habit_policy` | 5 | Debate and write policy |
| `quest_habit_taper` | 5 | Write and start the taper |
| `quest_habit_hours` | 5 | Staff the hours |
| `quest_habit_night` | 5 | Crisis night |
| `quest_habit_work` | 4 | Negotiate work return |
| `quest_habit_family` | 5 | Support the household |
| `quest_habit_relapse` | 5 | Relapse and response |
| `quest_habit_cabinet` | 4 | A theft discovered |
| `quest_habit_circle` | 4 | Start the circle |
| `quest_habit_year` | 4 | Measure the year |
| `quest_habit_what_we_carry` | 3 | Final disposition |

---

## 26. APPENDIX M — NPC DOSSIERS (BRIEF)

**Orián Vale** — medic. Believes pain is real and so is risk, and refuses to
choose one lie to make the other comfortable. Runs the ward by protocol and
patience.

**Sur Hala** — pharmacist. Counts everything and trusts no one's memory,
including her own. Keeps the register because the register is how the shelter
stays honest.

**Maeve Skell** — counselor. Ran a group before the Exchange and runs one now.
Listens with her whole attention and says uncomfortable things kindly.

**Tolan Reeve** — foundry worker. Strong, hurt, dependent, and ashamed in a way
that helps no one. His recovery is the expansion's spine.

**Nell Reeve** — child. Sleeps in the clinic waiting room because it is quiet
and warm. The reason policy is about people.

**Kori** — steward. Wants a rule that keeps the shelter safe and does not
break a family. Drafts policy in pencil and revises it in public.

**Amos** — peer. Three years in recovery and steady because of it. Runs the
circle when Maeve cannot and drinks tea instead of talking about himself.

**Juna** — elder. Lost a brother to drink before the war and carries the memory
without judgment. Sits with people during the worst hours.

---

## 27. APPENDIX N — LOCATION DETAIL

- **The Poppy Field** — medical crop with a guarded perimeter and a policy.
- **The Chem Wreck** — salvage with hazard and the temptation of easy stock.
- **The Old Clinic** — beds, equipment, and a cabinet that still locks.
- **The Herb Walk** — comfort herbs and tea.
- **The Care House** — step-down housing with light work.
- **The Court Garden** — recovery in daylight.
- **The Quiet Field** — walking, air, and distance from temptation.
- **The Registry** — confidential records and a locked drawer.
- **The Care Post** — observation during crisis nights.
- **The Cache** — the medical reserve, and the argument about when to open it.

---

## 28. APPENDIX O — CARE AND STAFFING MODEL

| Staff level | Care quality | Risk | Notes |
|---|---|---|---|
| None | minimal | high | home care only |
| 4h/day | basic | med | check-ins |
| 8h/day | decent | low | day coverage |
| 12h/day | good | low | extended |
| 16h/day | strong | very low | ward |
| 20h+ | full | minimal | two shifts |

Staffing is the expansion's true cost. Every hour in the ward is an hour not
spent on the foundry, the fields, or the road, and the expansion makes the
player feel that absence.

---

## 29. APPENDIX P — WITHDRAWAL CARE MODEL

| Hour band | Symptom level | Care order | Watch |
|---|---|---|---|
| 0–6 | mild | hydrate, rest | periodic |
| 6–12 | moderate | comfort, food | frequent |
| 12–24 | strong | ward, monitor | continuous |
| 24–48 | peak | full care | continuous |
| 48–72 | easing | comfort | frequent |
| 72–120 | settling | rest, food | periodic |
| 120+ | residual | support | daily |

Withdrawal is a schedule of care, not a wall of suffering. The player can see
the hours, plan the shifts, and watch the curve come down.

---

## 30. APPENDIX Q — RECOVERY AND RELAPSE MODEL

| State | Duration | Function | Risk | Response |
|---|---|---|---|---|
| Active use | varies | reduced | high | care entry |
| Withdrawal | days | low | crisis | ward |
| Early recovery | weeks | light | high | support |
| Mid recovery | months | half | med | graded work |
| Late recovery | months | full | low | maintain |
| Relapse | days | low | high | reset, support |
| Chronic care | open | adapted | med | ongoing |

Relapse is a state in the model, not a failure ending. The shelter's policy
decides whether someone re-enters care or is discarded, and the expansion never
rewards the second choice.

---

## 31. APPENDIX R — WORKED 360-DAY CARE SCENARIO

**Days 1–14.** Tolan's dose stops holding; stock counted; ward proposed.

**Days 15–30.** Policy debated and written; taper scheduled; first withdrawal
nights staffed by Maeve and Juna.

**Days 31–60.** Tolan stabilizes; Nell moves into the care house; light work
begins in the court garden; the circle starts with four people.

**Days 61–120.** Taper progresses with two holds; foundry return negotiated
grade by grade; Sur builds a reserve and the register tightens.

**Days 121–180.** A relapse on the anniversary of the accident; policy holds;
Tolan re-enters care without losing his place; the circle grows.

**Days 181–240.** Second taper and slow climb; work grade rises; family
stability improves; one medicine theft discovered and treated as a care case.

**Days 241–360.** Year mark; Tolan works full shifts with a standing check-in;
the ward becomes a permanent room; the policy is reviewed and revised in public.

---

## 32. APPENDIX S — VIGNETTE (TONE SAMPLE)

> Orián checks the pulse and the pupils and writes the hour in the log, and does
> not say anything reassuring, because Tolan has heard reassurance and it did
> not help; what helps is the log and the next hour.

> Juna sits by the bed and talks about her brother and the boat they had, and
> Tolan shakes and listens, and neither of them pretends the story is about
> anything other than staying.

> Nell draws at the clinic table while her father sleeps, and Sur puts a cup of
> tea where Nell can reach it without being asked, and that small thing is the
> policy working.

---

## 33. APPENDIX T — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| Withdrawal crisis | injury risk | ward, watch |
| Relapse | phase reset | re-enter care |
| Stock runout | forced taper | substitution, trade |
| Care understaffed | poor outcomes | roster, volunteers |
| Policy too strict | concealment | review, privacy |
| Policy too loose | safety risk | tighten, explain |
| Theft | trust loss | treat, secure |
| Work return too early | accident | regrade, rest |
| Family collapse | child risk | care house, support |
| Stigma | isolation | circle, education |

No failure is a game over. The deepest failure is a shelter that lets shame
decide its policy.

---

## 34. APPENDIX U — CONTENT REVIEW AND SENSITIVITY CHECKLIST

- [ ] No glamorized substance use anywhere in the expansion.
- [ ] No punishment, torture, or public shaming mechanic.
- [ ] No child substance content; family content is care and stability.
- [ ] Withdrawal is authored clinically and never as spectacle.
- [ ] Dependency levels are medical context, not shame meters.
- [ ] Recovery is possible, non-linear, and worth it.
- [ ] Relapse re-enters care rather than ending it.
- [ ] No contraband or black-market authority is created.
- [ ] Register privacy is a policy decision with real effects.
- [ ] Real substance names appear only where live data already uses them.
- [ ] A dedicated sensitivity review signs off before content ships.

---

## 35. APPENDIX V — GLOSSARY

- **Dependency level** — live ledger value per survivor and item.
- **Withdrawal** — the live state with authored symptoms.
- **Taper** — a written step-down schedule.
- **Substitution** — managed replacement to stabilize function.
- **Detox** — the removal process, managed or abrupt.
- **Recovery phase** — a measured stage with a work grade.
- **Relapse** — return to use, handled as a state.
- **Trigger** — an authored stressor that raises risk.
- **Register** — the controlled medicine log.
- **Policy** — the written care and safety rules.

---

## 36. APPENDIX W — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `ChemicalDependencySystem` | consumption, stress | levels, withdrawal | care |
| `DependencyCareSystem` | ledger | programs, ward | levels |
| `TaperSystem` | levels, stock | schedules | levels |
| `WithdrawalCareSystem` | withdrawal | care orders | levels |
| `RecoverySystem` | ledger, mental | phases, grades | levels |
| `PeerSupportSystem` | stress | programs | levels |
| `CarePolicySystem` | logs | policies | clinical state |
| `MedicalPipelineCoordinator` | symptoms | treatment | dependency |
| `PharmaLabSystem` | recipes | medicine | dependency |
| `SurvivorMentalHealthSystem` | stress | mental state | dependency |
| `CaregivingSystem` | roster | care labor | dependency |
| `DutyRoster` | shifts | assignments | care |
| `NeedsSystem` | comfort | morale, fatigue | dependency |
| `PolicySystem` | policy | enforcement | dependency |
| `EpilogueChronicleBuilder` | milestones | chronicle | — |

---

## 37. APPENDIX X — DATA SCHEMA DETAIL (NEW CATALOGS)

**`dependency_programs.json`** — `program_id`, `display_name`,
`staff_hours_per_day`, `supplies[]`, `duration_days`, `risk_modifier`,
`relapse_modifier`, `tags`.

**`taper_schedules.json`** — `schedule_id`, `display_name`, `substance_kind`,
`start`, `step`, `step_days`, `hold_rule`, `completion`, `tags`.

**`withdrawal_profiles.json`** — `profile_id`, `display_name`, `kind`,
`onset_hours`, `peak_hours`, `symptoms[]`, `severity_bands[]`, `crisis_risk`,
`care_orders[]`, `tags`.

**`substitution_plans.json`** — `plan_id`, `display_name`, `from`, `to`,
`conversion`, `days`, `supply_risk`, `tags`.

**`recovery_plans.json`** — `plan_id`, `display_name`, `phase`, `days`,
`work_grade`, `triggers[]`, `relapse_rule`, `tags`.

**`counseling_programs.json`** — `program_id`, `display_name`, `format`,
`hours`, `crowd`, `effect`, `tags`.

**`dependency_policies.json`** — `policy_id`, `display_name`, `prescribing`,
`privacy`, `work`, `review_cycle`, `enforcement`, `tags`.

**`stress_profiles.json`** — `stress_id`, `display_name`, `magnitude`,
`dependency_link`, `mitigation[]`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 38. APPENDIX Y — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Active dependency cases | caseload | DependencySystem |
| Withdrawal hours staffed | care capacity | CareWard |
| Taper adherence | treatment quality | Taper |
| Relapse rate | outcome | Recovery |
| Time to work return | function | Recovery |
| Register accuracy | governance | Pharmacy |
| Policy review currency | governance | Policy |
| Stigma incidents | social health | PeerSupport |
| Medicine reserve days | resilience | Pharmacy |
| Family stability | welfare | Recovery |

Telemetry is diagnostic only; it never gates content and never becomes a hidden
score.

---

## 39. APPENDIX Z — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Dependency progression stays in the live ledger only.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the sensitivity checklist in §34.
- [ ] Phase 7 soak shows injury, dependency, taper, relapse, and recovery.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No parallel dependency, contraband, or medical system exists.

---

## 40. APPENDIX AA — OPEN QUESTIONS FOR REVIEW

1. Does withdrawal ever kill in this system? Recommended: only through
   unattended crisis, never through dice.
2. Is the register public, staff-only, or sealed?
3. Can a dependent survivor hold a safety-critical role with a care plan?
4. Does substitution exist for every kind, or only some?
5. Who pays for a long taper: the ward, the person, or the workplace?
6. Can a household refuse care for a family member?
7. Does a relapse affect work grade automatically or by review?
8. Should the circle be run by a peer, a counselor, or both?

None of these may be decided unilaterally; each changes balance and tone.

---

## 41. APPENDIX AB — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children of dependent parents |
| 1 | 13 The Faithful | Confession, penance, and care ethics |
| 1 | 14 Above the Ash | Flight stress and medication |
| 1 | 15 The Deep Root | Medical crops and pain herbs |
| 1 | 16 The Rebuilt Body | Post-surgical pain and implants |
| 2 | 17 The Long Evening | Music, memory, and recovery |
| 2 | 18 The Underneath | Sealed care rooms |
| 2 | 19 The Bitter Air | Exposure medicine and dependency |
| 2 | 20 The Quiet Hand | Interrogation medicine refusal |
| 2 | 21 The Grid | Work stress and night shifts |
| 3 | 22 The Clean Flow | Clean care water |
| 3 | 23 The Alarm | Crisis response and care |
| 3 | 24 The Long Goodbye | Palliative pain management |
| 3 | 25 The Iron Road | Long-haul work and rest |
| 3 | 26 The Common Table | Comfort food and tea |
| 4 | 27 The Thread | Blankets and comfort items |
| 4 | 28 The Lesson | Health education and stigma |
| 4 | 29 The Glass | Dosing measures and instruments |
| 4 | 30 The Press | Privacy, policy, and notices |
| 4 | 31 The Kiln | Foundry injuries and pain |
| 5 | 32 The Wild | Poppy and herb sources |
| 5 | 33 The Weather | Cold, pain, and crisis nights |
| 5 | 34 The Long Road | Driver hours and dependency |
| 5 | 36 The Watch | Night watch and fatigue risk |

Each hook is additive. The Habit can ship alone, and every other expansion can
ship without it.

---

## 42. APPENDIX AC — ENDING PROSE SKETCHES

**The Carried Year.** The ward exists, the circle meets, the register is clean,
and Tolan works again; the shelter learned to carry what it could not cure.

**The Quiet Taper.** Nothing dramatic happens for months, and that is the
victory; the taper is followed, the policy is boring, and the family stays
together.

**The Open Cabinet.** A theft breaks trust, the clinic closes its doors to
loose access, and the shelter learns that a medicine cabinet is a promise.

**The Long Night.** A crisis and a death change everything; the policy turns
strict, the grief is real, and the circle keeps meeting because it has to.

**The Continuous Care.** Some people never fully recover, and the shelter
decides that is acceptable; the ward has one permanent bed and one permanent
name.

**Fade.** The same dose continues, the same person hides it, and nobody has
written anything down yet.

---

## 43. APPENDIX AD — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Dependency as debuff | dehumanizing | medical care story |
| Glamorized use | harmful | exclusion rule |
| Shame mechanics | cruel | care and privacy |
| Detox as minigame | trivializing | hours, staff, dignity |
| Relapse as game over | hopeless | re-entry and support |
| Contraband trade | authority break | economy untouched |
| Miracle cure | false | months and maintenance |
| Policy as flavor | no stakes | enforcement and review |
| Family invisible | shallow | household stability |
| Child exposure | unacceptable | family care only |

The list exists because this subject can go wrong in ways that hurt players.
The expansion's design rule is simple: dependency is a medical condition treated
by people who care, and the game never rewards contempt.

---

## 44. APPENDIX AE — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Care programs | 8 | 2,500 |
| Taper schedules | 10 | 2,500 |
| Withdrawal profiles | 8 | 3,000 |
| Substitution plans | 6 | 2,000 |
| Recovery plans | 10 | 3,000 |
| Counseling programs | 8 | 2,500 |
| Policies | 10 | 3,500 |
| Stress profiles | 12 | 2,500 |
| Quests | 53 | 14,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,000 |
| Endings | 6 | 3,000 |
| Sensitivity pass | n/a | 2,000 |
| **Total** | | **~55,000** |

---

## 45. APPENDIX AF — FIRST YEAR OF CARE

| Month | Focus | Milestone |
|---|---|---|
| 1 | Dose and stock | case opened |
| 2 | Policy | rules written |
| 3 | Ward | first shifts |
| 4 | Taper | schedule followed |
| 5 | Crisis night | protocol tested |
| 6 | Work return | graded duty |
| 7 | Family | household stabilized |
| 8 | Circle | peer support |
| 9 | Relapse | policy holds |
| 10 | Register | reserve secured |
| 11 | Review | policy revised |
| 12 | Year mark | recovery measured |

A year of care is a year of ordinary persistence, and the schedule is the
expansion's argument that recovery is built out of months rather than moments.

---

## 46. APPENDIX AG — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_habit_ward_setup` | 4 | Site, furnish, staff, open |
| `quest_habit_staff_shift` | 3 | Assign, brief, cover |
| `quest_habit_comfort_food` | 3 | Plan, cook, serve |
| `quest_habit_monitor` | 4 | Observe, record, report |
| `quest_habit_quiet_hours` | 3 | Schedule, enforce, adjust |
| `quest_habit_symptoms` | 4 | Recognize, score, document |
| `quest_habit_seizure_watch` | 3 | Prepare, watch, respond |
| `quest_habit_hydration` | 3 | Source, offer, track |
| `quest_habit_sleep` | 4 | Assess, support, verify |
| `quest_habit_safety` | 3 | Clear room, plan, drill |
| `quest_habit_taper_plan` | 4 | Assess, write, approve |
| `quest_habit_dose_log` | 3 | Count, record, verify |
| `quest_habit_stock_count` | 3 | Count, lock, report |
| `quest_habit_substitute` | 4 | Choose, source, start |
| `quest_habit_reserve` | 4 | Budget, acquire, store |
| `quest_habit_circle_start` | 4 | Invite, host, continue |
| `quest_habit_triggers` | 4 | Interview, map, plan |
| `quest_habit_work_return` | 4 | Assess, grade, review |
| `quest_habit_relapse_plan` | 4 | Draft, agree, keep |
| `quest_habit_year_mark` | 3 | Count, mark, celebrate |
| `quest_habit_family_talk` | 3 | Prepare, talk, follow up |
| `quest_habit_child_care` | 3 | Assess, arrange, verify |
| `quest_habit_household` | 4 | Budget, support, stabilize |
| `quest_habit_peer_pair` | 3 | Match, introduce, check |
| `quest_habit_stigma` | 4 | Name, discuss, change |
| `quest_habit_write_policy` | 4 | Draft, debate, publish |
| `quest_habit_work_rule` | 4 | Define, apply, review |
| `quest_habit_privacy_rule` | 3 | Choose, apply, seal |
| `quest_habit_hear_case` | 4 | Hear, weigh, decide |
| `quest_habit_review_year` | 3 | Review, revise, republish |

---

## 47. APPENDIX AH — COMFORT AND ENVIRONMENT MODEL

| Factor | Quality range | Effect | Source |
|---|---|---|---|
| Bed | hard–soft | rest | inventory |
| Blanket | torn–clean | warmth | thread |
| Lamp | dark–warm | calm | glass, fuel |
| Food | thin–plain | comfort | kitchen |
| Tea | none–herbal | calm | herb walk |
| Quiet | loud–quiet | sleep | room choice |
| Light cycle | rough–gentle | sleep | windows |
| Company | alone–peer | morale | circle |
| Air | stale–fresh | comfort | ventilation |
| Cleanliness | poor–clean | dignity | laundry |

The care environment is built from systems the shelter already has: bedding from
the Thread, light from the Glass, food from the Table, laundry from the Flow.
Care is not a separate economy; it is the same shelter applied to its most
vulnerable room.

---

## 48. APPENDIX AI — MEDICAL STOCK MODEL

| Stock level | Days | Action | Risk |
|---|---|---|---|
| Full | 60+ | maintain | none |
| Good | 30–60 | monitor | low |
| Adequate | 14–30 | plan resupply | med |
| Low | 7–14 | ration, substitute | high |
| Critical | 3–7 | emergency policy | very high |
| Out | 0 | forced change | crisis |

Medicine stock is the expansion's suspense. A full cabinet is invisible; a
nearly empty one changes every decision in the clinic, and the register makes
the number impossible to ignore.

---

## 49. APPENDIX AJ — FAMILY AND HOUSEHOLD MODEL

| Household state | Risk | Effect | Support |
|---|---|---|---|
| Stable | low | function | light check |
| Strained | med | sleep loss | family session |
| Unstable | high | child risk | care house |
| Split | high | grief | counseling |
| Reunited | med | recovery boost | follow-up |
| Supported | low | stability | ongoing |

Dependency is never only a personal condition; it lands on households. The
expansion models the family as a stability state with real support options, and
children are always the priority.

---

## 50. APPENDIX AK — COMMUNITY TRUST MODEL

| Event | Trust change | Notes |
|---|---|---|
| Care offered without shame | +5 | visible policy |
| Public shaming | −15 | prohibited |
| Register accurate | +3/year | governance |
| Register abused | −20 | scandal |
| Relapse handled well | +5 | policy proof |
| Relapse punished | −10 | concealment |
| Circle starts | +5 | visible care |
| Theft unaddressed | −10 | safety |
| Family supported | +5 | welfare |
| Privacy violated | −15 | trust collapse |

Trust is the expansion's social score, and it moves for the same reasons trust
moves in a real shelter: whether people are safe and whether they are treated
like people.

---

## 51. APPENDIX AL — WORKED RELAPSE SCENARIO

**Day 1.** The anniversary of the foundry accident arrives. Tolan works a full
shift and does not talk about it.

**Day 2.** Tolan misses a dose log entry. Sur notices the gap before the
medicine does.

**Day 3.** Tolan does not come to the circle. Nell tells Maeve that her father
is sleeping.

**Day 4.** The relapse is discovered; the care plan's relapse rule activates;
Tolan re-enters the ward the same day, without a disciplinary hearing.

**Day 5.** The steward considers a work restriction; the policy says review, not
punishment; the restriction is temporary and clinical.

**Day 12.** Tolan stabilizes; the taper restarts at a lower step; the circle
adds a second peer meeting.

**Day 30.** Work grade restored to light; family session held; the policy review
notes that the anniversary is a known trigger and adds it to the calendar.

The relapse costs the shelter weeks and trust, and it does not end the
recovery. That sentence is the expansion's thesis.

---

## 52. APPENDIX AM — LORE: THE CARE TRADITION

The fiction:

- **The Old Clinic** was a county hospital; its locked cabinet and ward layout
  are the reason the shelter has a standard for both.
- **The Court Garden** was a hospital courtyard and remains the best place in
  the shelter to sit in daylight and not be watched.
- **The Registry** was a records office; its promise of confidentiality
  survived the government that wrote it.
- **The Chem Wreck** is a salvage site that everyone knows about and nobody
  enjoys visiting, and the shelter's policy begins there.
- **The Care House** was a hostel; it now houses households in transition and
  is the quietest building in the shelter.

The tradition is generic and local. No real hospital, program, or agency is
copied.

---

## 53. APPENDIX AN — SENSITIVE TOPICS TABLE

| Topic | Risk | Handling |
|---|---|---|
| Dependency | Stigma | Medical framing |
| Withdrawal | Horror | Clinical, non-graphic |
| Relapse | Shame | Re-entry, support |
| Death | Spectacle | Rare, grieved |
| Children | Harm | Family stability first |
| Work safety | Blame | Policy, review |
| Theft | Punishment | Care and repair |
| Privacy | Betrayal | Policy-defined |
| Peer support | Therapy cliché | Plain talk |
| Medicine | Misuse | Register and policy |

The subject matter requires care in every line, and the expansion's contract is
that the shelter treats dependent people as neighbors, family, and colleagues —
never as a cautionary tale.

---

## 54. APPENDIX AO — MEASUREMENT AND REVIEW CADENCE

| Gate | Question | Evidence |
|---|---|---|
| Premise | Do live owners still match? | file:line audit |
| Data | Are all rows valid and reachable? | integrity + scanner |
| Core | Are systems pure and deterministic? | unit tests |
| Persistence | Does legacy load neutral? | round-trip tests |
| Host | Is every command real? | selftest + journey |
| UI | Is the care state honest? | lifecycle + a11y tests |
| Tone | Is care humane? | sensitivity review |
| Balance | Is recovery earned? | 360-day soak |

---

## 55. APPENDIX AP — OPEN IMPLEMENTATION NOTES

- Dependency progression must stay entirely inside
  `ChemicalDependencySystem`; new systems modify tick parameters only.
- Care orders should route through `MedicalPipelineCoordinator` like any other
  treatment.
- Taper schedules should consume real stock through the pharmacy path.
- Recovery phases should drive work grades through the existing work model, not
  a parallel employment system.
- Counseling should extend mental health and caregiving rather than adding a new
  therapy store.
- Policy should register with the live policy surface and publish through the
  notice system.
- The register should be a real log with access control, not a decorative
  counter.
- Sensitivity review is a required phase gate, not a suggestion.

---

## 56. APPENDIX AQ — CARE STAFFING SCHEDULE TABLE

| Shift | Hours | Roles | Coverage | Fatigue |
|---|---|---|---|---|
| Morning round | 07–09 | medic | meds, check | low |
| Day watch | 09–15 | caregiver | comfort | med |
| Afternoon | 15–17 | peer | talk, walk | low |
| Evening round | 17–19 | medic | check | med |
| Night watch | 19–07 | caregiver | crisis | high |
| Relief | on call | peer | cover | low |
| Family hour | 16–17 | family | visits | none |
| Review | weekly | team | plan | low |

The ward schedule is the same shape as every other shift in the shelter, which
keeps care inside the live roster instead of outside it. Night watch is the
hardest post and the expansion says so in the data.

---

## 57. APPENDIX AR — CARE OUTCOME TABLE

| Treatment quality | Stabilization | Relapse risk | Work return |
|---|---|---|---|
| Ward full staff | fast | low | graded |
| Ward partial | steady | med | slower |
| Home care | slow | med | variable |
| Self-managed | uncertain | high | late |
| None | crisis | high | unknown |
| Substitution | stable | low | maintained |
| Peer only | variable | med | social |
| Clinic referral | extern | unknown | pending |

Outcomes are authored from treatment quality rather than rolls. The shelter that
staffs its own ward earns the outcome; the shelter that does not gets the
uncertainty it paid for.

---

## 58. APPENDIX AS — CLINICAL INTERACTION TABLE (FICTIONAL)

| Combination | Effect | Risk | Guidance |
|---|---|---|---|
| Opioid + alcohol | sedation | high | separate |
| Opioid + sedative | respiratory | high | clinical only |
| Alcohol + sedative | crisis | extreme | prohibited |
| Opioid + herbal | variable | low | record |
| Substitution + comfort | eased | low | standard |
| Taper + sleep aid | restful | low | monitor |
| Multiple opioids | overdose | high | never |
| Chronic + acute pain | conflict | med | review |

The interaction table is deliberately simple and fictionalized. The expansion
is not a pharmacology text; it is a small set of authored risks that the clinic
can teach, log, and avoid.

---

## 59. APPENDIX AT — PEER CIRCLE SCHEDULE TABLE

| Day | Time | Topic | Group | Host |
|---|---|---|---|---|
| Monday | 18:00 | check-in | all | Maeve |
| Tuesday | 18:00 | triggers | recovery | Amos |
| Wednesday | 18:00 | family | families | Maeve |
| Thursday | 18:00 | work | returning | Kori |
| Friday | 18:00 | grief | bereaved | Juna |
| Saturday | 10:00 | walk | all | Amos |
| Sunday | 17:00 | quiet | optional | none |
| Monthly | varies | review | all | team |

A circle is a schedule and a room and a person who shows up. The expansion's
rule is that support is ordinary, frequent, and voluntary.

---

## 60. APPENDIX AU — POLICY ENFORCEMENT TABLE

| Rule | Enforcement | Violation | Response |
|---|---|---|---|
| Register log | pharmacist | missing entry | review |
| Prescribe limits | medic | over | hearing |
| Privacy | all staff | breach | discipline |
| Work safety | steward | unsafe duty | restriction |
| Substance secrecy | all | concealment | care, not shame |
| Cabinet access | pharmacist | unauthorized | secure, repair |
| Family priority | steward | neglect | welfare check |
| Review cycle | steward | overdue | schedule |
| Register audit | steward | error | correct |
| Circle conduct | host | harm | remove |

Every policy rule has a named enforcer and a humane response. The expansion
never lets a rule be enforced by abstraction; a person enforces it, and a person
is accountable for how.

---

## 61. APPENDIX AV — RECOVERY MILESTONE TABLE

| Milestone | Condition | Mark | Effect |
|---|---|---|---|
| First day | 24h clean | mark | hope |
| First week | 7 days | mark | stability |
| First month | 30 days | mark | function |
| Taper step | each step | log | progress |
| Return to work | graded pass | mark | dignity |
| Six months | 180 days | mark | trust |
| One year | 365 days | mark | identity |
| Circle helper | 3 months | role | service |
| Family stable | review | mark | welfare |
| Policy proof | review | cite | governance |

Milestones are quiet and private by default. They exist so a person can see
progress in months, because the daily count is the worst way to measure a
recovery.

---

## 62. APPENDIX AW — COMFORT ROUNDS CHECKLIST TABLE

| Round | Time | Check | Record |
|---|---|---|---|
| Morning | 07:00 | sleep, food, mood | yes |
| Midday | 12:00 | hydration, pain | yes |
| Afternoon | 15:00 | activity, light work | yes |
| Evening | 18:00 | taper dose, mood | yes |
| Night | 22:00 | settle, safety | yes |
| Watch | 02:00 | crisis check | yes |
| Dawn | 05:00 | rest, notes | yes |
| Weekly | review | plan, goals | yes |

Rounds are the ward's rhythm: small, frequent, and recorded. Care is not a
gesture, it is a schedule that people keep.

---

## 63. APPENDIX AX — CONFIDENTIALITY AND ACCESS TABLE

| Record | Sealed | Staff | Family | Survivor |
|---|---|---|---|---|
| Dependency level | yes | medic | no | yes |
| Taper schedule | yes | medic | if agreed | yes |
| Care ward notes | yes | care team | no | yes |
| Register entries | yes | pharmacist | no | no |
| Policy rules | no | all | all | all |
| Recovery milestone | yes | care team | if agreed | yes |
| Work grade | yes | steward | no | yes |
| Incident report | yes | care team | no | if involved |

Access is a table because privacy is a decision the shelter makes on purpose.
The default is sealed, the exceptions are clinical, and the person always has
the right to their own record.

---

## 64. APPENDIX AY — CARE SUPPLY TABLE

| Supply | Daily use | Source | Criticality |
|---|---|---|---|
| Water | high | clean flow | critical |
| Food | high | kitchen | critical |
| Blankets | low | thread | high |
| Tea | med | herb walk | comfort |
| Medicine | low | pharmacy | critical |
| Comfort meals | low | kitchen | high |
| Lamps | low | glass | comfort |
| Logbooks | low | press | high |
| Cleaning | med | clean flow | high |
| Pain records | low | press | med |

Care runs on the same shelves as everything else in the shelter, which is the
expansion's quiet point: looking after people is not a separate economy with
separate resources, it is the ordinary shelter turned toward its hardest room.

---

## 65. CLOSING STATEMENT

ASHFALL already models dependency with a real ledger, real withdrawal ticks,
managed detox and cold-turkey paths, staffing effects, stress, pharmacy
production, and a medical pipeline. What it lacks is the care: programs, tapers,
symptoms, hours, people, families, recovery, relapse, and the policy that keeps
everyone safe without discarding anyone. The Habit adds that world without
adding a second dependency system and without a single punishing mechanic. It
adds a quiet ward, a written taper, a circle of peers, a father who comes back
to work, and a shelter that decided care was maintenance on people.

> Wave 5 note: this plan is one of five Wave 5 expansion bibles (32–36). Each is
> self-contained; none requires another to ship. The shared Wave 5 index lives at
> `docs/expansions/wave5/WAVE5_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `ChemicalDependencySystem` (`OnSubstanceConsumed`, `ReportStress`,
> `BeginManagedDetox`, `BeginColdTurkey`, `TickHours` with staffing and staff
> speed, `HasActiveWithdrawal`, `DependencyLevel`),
> `ChemicalDependencyAfflictionHandler`, `PharmaLabSystem`,
> `PharmaceuticalTabletEngine`, `chemical_dependency_items.json` (2.8 KB with
> morphine 0.9, opium 0.85, painkiller 0.8, alcohol 0.7), and the live mental
> health, caregiving, and grief systems.