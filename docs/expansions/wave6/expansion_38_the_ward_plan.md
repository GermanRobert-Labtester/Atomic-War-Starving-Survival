# ASHFALL — Expansion 38 Design Bible
# THE WARD
### Wave 6 · Acute Care, Surgery, Recovery Wards, Isolation, and Hospital Operations

**Document status:** Design plan (pre-integration). Not a claim. Not an authorization.
**Date:** 2026-09-23
**Domain owners touched:** `Ashfall.Core.Medical` (MedicalWardSystem, AdvancedSurgicalWardSystem, MedicalPipelineCoordinator, MedicalProcedureSchedule, SurgicalProcedureCatalog, NarcoticsSystem, DiagnosisKnowledgeStore)
**Proposed host owner:** `WardHostSession` (extends ward, surgery, and recovery surfaces)
**Existing save sections:** `MedicalWardSave`, `AdvancedSurgicalWardSave`, `MedicalPipelineSave`, patient records
**Existing CLI verbs:** `--medical-selftest` (if present), `--data-integrity-selftest`, `--content-utilization-selftest`
**Rule compliance:** Godot authoritative; Core engine-free; JSON data authoritative; one authority per concern; deterministic seeded RNG.

---

## 0. HOW TO READ THIS DOCUMENT

This is a **design bible**, not an integration plan. Implementation must later pass
through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and `TEST_POLICY.md`.

ASHFALL already has a working hospital skeleton. `MedicalWardSystem` exposes
`Beds`, `Procedures`, `State`, `StaffingPreflight`, `OnWardChanged`,
`OnPatientAdmitted`, `Admit(patientId, bedId, day)`,
`Discharge(patientId, day)`, `RunProcedure(patientId, procedureId, day)`,
`CaptureState()`, `RestoreState(state)`, `GetBedOccupant(bedId)`, and
`GetActiveAdmission(patientId)`; `MedicalBed` carries `BedId`, `DisplayName`,
`Category`, and `Isolation`. `AdvancedSurgicalWardSystem` defines
`SurgicalOperationState` with `progress_hours`, `total_duration_hours` (default
four), `shock_percent`, `anesthesia_level`, incisions/midpoint/closure milestone
flags, `is_completed`, `patient_survived`, `recovery_days_remaining`,
`complications`, `rad_mSv_purged`, and `installed_prosthetic_item_id`, plus
`AdvancedSurgicalWardSave`. `MedicalProcedureSchedule` tracks `procedureId`,
`survivorId`, `treatmentId`, `afflictionEpisodeId`, `startDay`, `remainingHours`,
`totalHours`, `reservationIds`, `status`, and `endDay`. There are also
`NarcoticsSystem` (`NarcoticDefinition` with `tolerance_gain` and
`dependency_pressure`), `DiagnosisKnowledgeStore`, `MedicalRecordLog`,
`MedicalReservationLedger`, `PatientRecord`, `MedicalTreatmentCatalog`,
`MicrofluidicDiagnosticEngine`, `MutationSystem`, `DiseaseProtocolHandler`,
`RespiratoryDegenerationSystem`, and `LyophilizationSystem`.

What does not exist: wards as places with rooms, beds, staffing, and shifts;
triage; surgery teams and schedules; recovery care; isolation practice;
hospital-acquired infection controls; supply and sterilization logistics;
patient rounds; ward culture; and long-term care content. `surgical_procedures.json`
is only **2,141 bytes**.

**The Ward** turns the hospital skeleton into a working hospital: triage,
admission, surgery, recovery, isolation, rounds, and the staff who keep it
running. It extends the live ward, surgery, and schedule owners and hands every
clinical fact to them.

Conventions: **`LIVE`** confirmed in source/data; **`GAP`** confirmed thin;
**`PROPOSED`** new.

### 0.1 Boundary contracts

| Neighbour | Owns | This plan does |
|---|---|---|
| 16 The Rebuilt Body | Amputation, bionics, prosthetics, rehab devices | Schedules their procedures, never defines them |
| 22 The Clean Flow | Water, sanitation, exposure, public health | Uses its exposure and hygiene rules |
| 24 The Long Goodbye | Palliative care, grief, legacy | Refers comfort cases, never manages dying |
| 35 The Habit | Dependency, detox, narcotic affliction handling | Uses its handlers; surgery uses analgesia only |
| 19 The Bitter Air | Hazard agents and decontamination | Uses its decon routes for contaminated arrivals |
| 26 The Common Table | Nutrition, diets | Requests recovery diets |
| 37 The Quickening | Maternity and nursery care | Shares the ward for complicated births |
| 36 The Watch | Night readiness | Shares night staffing surfaces |

---

## 1. EXECUTIVE SUMMARY

### 1.1 Pitch

There are beds, and there are procedures, and there is nobody in charge of the
middle.

**The Ward** is the expansion about the part of medicine that happens after the
diagnosis and before the discharge: who is admitted, who is watched, who
operates, who cleans, who sleeps, who decides, and what the hospital writes
down. It gives the shelter a triage line, a surgery roster, a recovery ward, an
isolation room, a sterilization room, rounds, and shifts.

### 1.2 The five loops it adds

```
  Triage ──► Admit ──► Treat ──► Recover ──► Discharge
     │         │          │          │           │
     ▼         ▼          ▼          ▼           ▼
   Priority, Beds,      Procedures, Rest,      Notes,
   capacity  isolation  surgery    rounds      follow-up
                                      │
                                      ▼
                              Review ──► Improve ward practice
```

### 1.3 What the player manages

1. **Triage.** Priority, capacity, and honest wait decisions.
2. **Admission.** Beds, isolation, and observation.
3. **Surgery.** Teams, schedules, supplies, and milestone checkpoints.
4. **Recovery.** Rounds, rest, feeding, and therapy referrals.
5. **Isolation.** Separate beds, strict hygiene, and kindness.
6. **Infection control.** Sterilization, hand hygiene, laundry, waste.
7. **Diagnostics.** Samples, tests, and knowledge that must be kept.
8. **Staffing.** Shifts, training, call rotas, and fatigue.
9. **Supplies.** Narcotics, dressings, instruments, and blood substitutes.
10. **Records.** Honest charts, sealed privacy, and ward review.

### 1.4 What it is not

- Not graphic or gore-forward. Surgery is represented by phases, teams, and
  outcomes, never by close detail.
- Not a second medical, disease, dependency, or prosthetics system. It extends
  the live owners and refers to them.
- Not a torture or experimentation system. Prisoners receive the same care as
  residents; no interrogation happens in a ward.
- Not a blood-and-donor economy. Transfusion is abstracted as a procedure with
  supplies, screening, and risk, without targeting donors as content.
- Not a death factory. Losses are rare, caused by traceable factors, and routed
  to grief owners with dignity.
- Not a new save section.

---

## 2. EVIDENCE BASE AND GAP ANALYSIS

### 2.1 Live systems (verified)

| File | Role | Status |
|---|---|---|
| `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` | Beds, admissions, procedures | `LIVE` |
| `Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs` | Surgery phases and outcomes | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` | Care routing | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalProcedureSchedule.cs` | Procedure scheduling | `LIVE` |
| `Assets/Ashfall.Core/Medical/SurgicalProcedureCatalog.cs` | Procedure content | `LIVE` |
| `Assets/Ashfall.Core/Medical/NarcoticsSystem.cs` | Analgesia, tolerance, pressure | `LIVE` |
| `Assets/Ashfall.Core/Medical/DiagnosisKnowledgeStore.cs` | Diagnostic knowledge | `LIVE` |
| `Assets/Ashfall.Core/Medical/DiseaseProtocolHandler.cs` | Disease protocols | `LIVE` |
| `Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs` | Diagnostics | `LIVE` |
| `Assets/Ashfall.Core/Medical/MutationSystem.cs` | Mutation content | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalRecordLog.cs` | Records | `LIVE` |
| `Assets/Ashfall.Core/Medical/MedicalReservationLedger.cs` | Supply reservations | `LIVE` |

### 2.2 Live data (counted)

| Catalog | Size | Notes |
|---|---|---|
| `surgical_procedures.json` | **2,141 B** | the core gap |
| `disease_catalog.json` | 54,638 B | disease content (used, not owned by this plan) |
| `medical_texts.json` | 222,244 B | prose corpus |
| `narcotics.json`, `pharma_recipes.json`, `tablet_manufacturing_catalog.json` | small | chemistry and analgesia |
| Ward rooms, triage, bed, shift, isolation, sterilization data | absent | confirmed none |

### 2.3 Confirmed gaps

- **GAP-38-1 — No triage.** Priority and capacity are unmodeled.
- **GAP-38-2 — No ward rooms or beds as content.** Beds exist as state, not place.
- **GAP-38-3 — No surgery team or roster content.**
- **GAP-38-4 — No recovery care content.**
- **GAP-38-5 — No isolation practice.**
- **GAP-38-6 — No infection-control loop.**
- **GAP-38-7 — No sterilization logistics.**
- **GAP-38-8 — No ward rounds or review culture.**
- **GAP-38-9 — No ward staffing or training content.**
- **GAP-38-10 — Procedure content is a 2 KB stub.**

### 2.4 Non-duplication statement

This expansion will **not** add a second ward, surgery, disease, dependency,
diagnostics, or prosthetics system. It extends `MedicalWardSystem` with beds and
rooms, `AdvancedSurgicalWardSystem` with teams and phases,
`MedicalProcedureSchedule` with rosters, and `SurgicalProcedureCatalog` with
content. It adds state only as additive sub-objects of the existing ward, surgery,
and pipeline stores. No new save section.

---

## 3. DESIGN PILLARS AND TONE

### 3.1 Pillars

**Pillar 1 — Triage is honesty under scarcity.** The ward says out loud who is
treated first and why.

**Pillar 2 — The chart is a promise.** If it is not written down, it did not
happen, and someone will pay for the silence.

**Pillar 3 — Recovery is work, not waiting.** Rounds, feeding, movement, and
rest are active care.

**Pillar 4 — Clean beats heroic.** Hand hygiene and sterilization save more
people than any single operation.

**Pillar 5 — The ward treats people, not cases.** Names, dignity, and privacy
belong in every procedure.

### 3.2 Tone calibration

| Element | Do | Do not |
|---|---|---|
| Triage | Calm priority | Cold calculus |
| Surgery | Phases and teams | Gore |
| Recovery | Rounds and small wins | Montage |
| Isolation | Strict kindness | Stigma |
| Error | Review and repair | Blame |
| Loss | Traceability and grief | Spectacle |
| Staff fatigue | Rota and relief | Martyrdom |
| Supplies | Reuse and sterilize | Hoarding porn |

### 3.3 Content limits

- No graphic surgical detail. Phases, shock percentage, and outcomes only.
- No experimentation, no torture, no forced procedures.
- Prisoners and outsiders receive identical care; the ward is neutral ground.
- No using patients as content for horror; death is quiet and consequential.
- Narcotics are clinical: analgesia with tracked pressure, never a party.
- Children in the ward are handled with the same dignity as adults and with a
  guardian present by default.

---

## 4. THE WARD WORLD

### 4.1 Interior rooms

- **`room_triage`** — the desk by the door where priority is decided.
- **`room_ward_main`** — six beds, one window, and a smell of soap.
- **`room_ward_isolation`** — separate door, separate staff, same care.
- **`room_theatre`** — the operating room: table, lamp, instrument tray.
- **`room_recovery`** — four beds near the stove.
- **`room_scrub`** — sinks, aprons, and the hand-hygiene ledger.
- **`room_sterile_store`** — instruments, dressings, and the autoclave.
- **`room_ward_office`** — charts, keys, and the duty board.

### 4.2 Exterior locations

| ID | Name | Tone | Purpose |
|---|---|---|---|
| `loc_ward_door` | The Ward Door | 3 | Arrivals and handover |
| `loc_ambulance_yard` | The Stretcher Yard | 4 | Arrivals from the gate |
| `loc_laundry_shed` | The Laundry Shed | 2 | Ward linen cycle |
| `loc_waste_pit` | The Clinical Waste Pit | 3 | Safe disposal |
| `loc_herb_bed` | The Herb Bed | 1 | Simple remedies |
| `loc_quiet_corner` | The Quiet Corner | 2 | Families and waiting |
| `loc_overflow_tent` | The Overflow Tent | 4 | Surge capacity |
| `loc_burn_pit` | The Burn Pit | 4 | Contaminated cloth |
| `loc_water_tank` | The Ward Tank | 3 | Water reserve |
| `loc_train_yard_ward` | The Rail Head | 4 | Casualty arrivals |

All locations require valid item references and scanner registration.

### 4.3 Ward rhythm

Rounds at morning and evening; procedures scheduled between; night staff keep
obs; a review every week. The expansion's clock is the ward day, and the ward
never fully sleeps.

---

## 5. MAIN STORYLINE — "THE MIDDLE OF THE NIGHT"

### 5.1 Central conflict

A collapse in the foundry brings three injured people to a hospital that has
beds, a procedure list, and no one who has ever run a ward. **Halden Rooke** the
surgeon wants a theatre team and a sterile tray. **Bel Arun** the head nurse
wants triage, rounds, and a hand-hygiene rule people actually follow. **Mar
Quill** the anesthetist wants a schedule that keeps one person from staying
awake for thirty hours. **Tobin** the ward clerk wants honest charts. **Sena**
the recovery aide wants patients fed and moved instead of left to lie still.

Then an isolation case arrives from the gate, the overflow tent goes up, and the
ward discovers that the enemy is not the injury. It is the paperwork nobody
wrote, the tray nobody sterilized, and the nurse nobody relieved.

The expansion's question: **can a shelter build a hospital out of a room, a
rabbit, and a rota?**

### 5.2 Theme (unspoken)

**Care is a system, and systems fail in the spaces between people.**

### 5.3 Principal NPCs

| ID | Name | Role | Function |
|---|---|---|---|
| `npc_surgeon_halden_rooke` | Halden Rooke | Surgeon | Theatre, procedures, decisions |
| `npc_head_nurse_bel_arun` | Bel Arun | Head nurse | Triage, rounds, hygiene |
| `npc_anesthetist_mar_quill` | Mar Quill | Anesthetist | Sleep, pain, schedule |
| `npc_ward_clerk_tobin` | Tobin | Ward clerk | Charts, keys, records |
| `npc_recovery_aide_sena` | Sena | Recovery aide | Feeding, movement, watch |
| `npc_orderly_pim` | Pim | Orderly | Sterilization, linen, waste |
| `npc_isolation_nurse_junia` | Junia | Isolation nurse | Separate care, same dignity |
| `npc_patient_arl` | Arl | Patient | The foundry injury |

### 5.4 Story beats (15)

1. **The Collapse.** Three injured arrive at once.
2. **The Desk.** Triage is improvised and nearly gets it wrong.
3. **The Tray.** A procedure fails on an unclean instrument.
4. **The Rule.** Hand hygiene becomes a ledger with names.
5. **The Rota.** The first night schedule is written and tested.
6. **The Theatre.** A team performs a scheduled operation, start to finish.
7. **The Round.** Morning rounds find a problem nobody reported.
8. **The Chart.** Tobin's charts catch a medication error.
9. **The Isolation.** A fever case forces separate care.
10. **The Overflow.** The tent goes up and the ward triages for real.
11. **The Exhaustion.** A nurse collapses; the rota is rebuilt.
12. **The Recovery.** Sena gets a patient out of bed and back to the yard.
13. **The Review.** The ward reviews its own failures without blaming anyone.
14. **The Winter Surge.** Cold, fractures, and respiratory cases at once.
15. **The Middle of the Night.** The ward becomes an institution.

### 5.5 Branching choices (8)

| Choice | Options | Axis |
|---|---|---|
| Triage rule | severity / salvage / queue | honesty vs. hope |
| Theatre | single table / two shifts / rationed | throughput vs. safety |
| Isolation | closed ward / separate room / sent home | control vs. kindness |
| Charts | full / problem-only / memory | discipline vs. speed |
| Rota | eight-hour / twelve-hour / crisis | health vs. coverage |
| Sterilization | autoclave / boil / chemical | reliability vs. speed |
| Overflow | tent / corridor / refuse | surge vs. standards |
| Final | ward as institution / practice / memory | identity |

### 5.6 Endings (5 + fade)

1. **The Quiet Ward** — the system works: triage, rota, sterile tray, charts,
   and rounds.
2. **The Open Door** — the ward is small, humane, and never turns away an
   emergency.
3. **The Long Rota** — staffing is always tight; the ward survives on goodwill
   and the plan to fix it.
4. **The Hard Season** — the winter surge breaks the ward and rebuilds it.
5. **The Empty Bed** — a loss traced to a missing step, mourned, and never
   repeated.
6. **Fade** — a lamp over a chart and someone checking a pulse in the dark.

---

## 6. QUEST DESIGN

New IDs use prefix `quest_ward_`. Schema follows existing quest catalogs.

### 6.1 Main questline (15)

`quest_ward_collapse`, `quest_ward_desk`, `quest_ward_tray`, `quest_ward_rule`,
`quest_ward_rota`, `quest_ward_theatre`, `quest_ward_round`, `quest_ward_chart`,
`quest_ward_isolation`, `quest_ward_overflow`, `quest_ward_exhaustion`,
`quest_ward_recovery`, `quest_ward_review`, `quest_ward_winter_surge`,
`quest_ward_middle_of_night`.

### 6.2 Side quests (30)

**Triage (5)**
- `quest_ward_priority` — write a triage rule
- `quest_ward_wait` — make waiting humane
- `quest_ward_capacity` — count real beds
- `quest_ward_transfer` — move a patient safely
- `quest_ward_door` — keep the door manned

**Surgery (5)**
- `quest_ward_team` — assemble a theatre team
- `quest_ward_trays` — build sterile trays
- `quest_ward_schedule` — schedule procedures
- `quest_ward_checkpoint` — milestone discipline
- `quest_ward_shock` — manage a bad turn

**Recovery (5)**
- `quest_ward_rounds` — morning rounds
- `quest_ward_feed` — recovery feeding
- `quest_ward_move` — early movement
- `quest_ward_pain` — analgesia review
- `quest_ward_followup` — follow-up plan

**Isolation (5)**
- `quest_ward_separate` — set a separate room
- `quest_ward_hygiene` — strict hygiene
- `quest_ward_visits` — family visits done safely
- `quest_ward_vent` — airflow and distance
- `quest_ward_end` — safe release from isolation

**Operations (5)**
- `quest_ward_sterile` — sterilization loop
- `quest_ward_linen` — ward laundry cycle
- `quest_ward_waste` — clinical waste
- `quest_ward_supply` — instrument count
- `quest_ward_keys` — sealed store control

**Records (5)**
- `quest_ward_charts` — honest charts
- `quest_ward_consent` — consent recorded
- `quest_ward_review_month` — monthly review
- `quest_ward_training` — staff training
- `quest_ward_archive` — ward archive

### 6.3 Repeatable quests (8)

`quest_ward_repeat_round`, `quest_ward_repeat_obs`, `quest_ward_repeat_sterile`,
`quest_ward_repeat_linen`, `quest_ward_repeat_supply`, `quest_ward_repeat_chart`,
`quest_ward_repeat_feed`, `quest_ward_repeat_restock`.

### 6.4 Dynamic hooks

Live events (injuries, disease, radiation cases, birth complications, foundry
accidents, gate arrivals, weather, dependency crises) attach authored follow-ups
through existing seams. No new event bus.

### 6.5 Constraints

- Bed, admission, procedure, and surgery state stay with the live ward owners.
- Disease stays with the disease engine; dependency with its owner; prosthetics
  with 16.
- No graphic content; no torture; no experimentation.
- Prisoners and outsiders get identical care.
- No new save section.

---

## 7. NEW GAMEPLAY SYSTEMS

### 7.1 `TriageSystem` (new, `Ashfall.Core.Medical`)

**Owns:** priority rules, wait states, capacity counts, and the triage record.
**Consumes:** `MedicalWardSystem`, `MedicalPipelineCoordinator`,
`MedicalProcedureSchedule`, `DiseaseTriage`. **Data:** `triage_rules.json`,
`ward_capacity.json`. **Rules:** every arrival gets a recorded priority and a
reason; the rule is chosen by the shelter and applied consistently; the record
is reviewable and never secret.

### 7.2 `WardOpsSystem` (new, `Ashfall.Core.Medical`)

**Owns:** ward rooms, beds as places, shifts, rounds, and duty boards.
**Consumes:** `MedicalWardSystem.Beds`, `ShelterAssignmentSystem`, `DutyRoster`,
`NeedsSystem`. **Data:** `ward_rooms.json`, `ward_shifts.json`.
**Rules:** beds are physical with condition and location; staffing is real and
fatigue-tracked; rounds generate observations that feed the live patient record.

### 7.3 `SurgeryRosterSystem` (new, `Ashfall.Core.Medical`)

**Owns:** theatre teams, rosters, instrument trays, and scheduled procedures.
**Consumes:** `AdvancedSurgicalWardSystem`, `MedicalProcedureSchedule`,
`SurgicalProcedureCatalog`, `NarcoticsSystem`, `MedicalReservationLedger`.
**Data:** `surgery_teams.json`, `surgical_trays.json`.
**Rules:** a procedure requires a complete team, a sterile tray, and a scheduled
slot; milestone flags come from the live surgery system; complications resolve
through it, never through a parallel roll.

### 7.4 `RecoveryCareSystem` (new, `Ashfall.Core.Medical`)

**Owns:** recovery rounds, feeding requests, movement plans, and follow-ups.
**Consumes:** `MedicalWardSystem`, `NeedsSystem`, `KitchenNutritionSystem`
(requests only), `MedicalProcedureSchedule`. **Data:**
`recovery_protocols.json`. **Rules:** recovery is active care with observations;
no hidden healing bonus, no instant discharge.

### 7.5 `IsolationSystem` (new, `Ashfall.Core.Medical`)

**Owns:** isolation rooms, separate staffing, visit rules, and release criteria.
**Consumes:** `MedicalWardSystem` isolation beds, `DiseaseQuarantineCoordinator`
(Wave 2), `DiseaseProtocolHandler`, ventilation data. **Data:**
`isolation_protocols.json`. **Rules:** isolation is strict and kind; a person in
isolation keeps their name, their visitors when safe, and their dignity; release
requires criteria, not mood.

### 7.6 `SterileSupplySystem` (new, `Ashfall.Core.Medical`)

**Owns:** instrument sterilization, dressing supply, linen cycle, and clinical
waste. **Consumes:** `MedicalReservationLedger`, `Inventory`,
`WaterTreatmentSystem` (Wave 3), `SanitationSystem`, `ShelterFireHazardSystem`
(burn pit safety). **Data:** `sterilization_cycles.json`,
`ward_supplies.json`. **Rules:** sterilization has time, temperature, and
verification; every tray has a cycle record; failures trace to a cause.

### 7.7 `WardRecordsSystem` (new, thin, extends records owners)

**Owns:** charts, consent flags, medication records, and ward review notes.
**Consumes:** `MedicalRecordLog`, `PatientRecord`, `DiagnosisKnowledgeStore`,
`MedicalTextCatalog`. **Data:** `ward_chart_fields.json`. **Rules:** charts are
honest and complete enough for a stranger to continue care; consent is recorded
whenever a person is able; privacy follows the existing medical seal rules.

### 7.8 Systems explicitly not added

- No second ward, surgery, disease, dependency, prosthetics, or diagnostics
  system.
- No blood-targeting donor content, no organ market, no experimentation.
- No new currency.
- No new RNG stream.
- No new save section.

---

## 8. DATA CATALOG SPECIFICATION

All catalogs snake_case, integer `schema_version: 1`, validated and scanner-registered.

### 8.1 `triage_rules.json` (new)

```json
{
  "schema_version": 1,
  "rules": [
    {
      "rule_id": "triage_severity",
      "display_name": "Severity First",
      "order": ["critical", "urgent", "stable", "walking"],
      "review_days": 7,
      "notes": "consistent, recorded, reviewable",
      "tags": ["policy", "ward"]
    }
  ]
}
```

### 8.2 `ward_capacity.json` (new)

Capacity: rooms, beds, isolation counts, overflow triggers.

### 8.3 `ward_rooms.json` (new)

Rooms: id, beds, isolation, ventilation, water, condition.

### 8.4 `ward_shifts.json` (new)

Shifts: role, hours, coverage, relief, fatigue rules.

### 8.5 `surgery_teams.json` (new)

Teams: lead, assistant, anesthetist, runner, required skills.

### 8.6 `surgical_trays.json` (new)

Trays: procedure, instruments, sterilization cycle, verification.

### 8.7 `surgical_procedures.json` (extend, currently 2,141 B)

Add authored procedures: id, phases, duration, supplies, complications, recovery.

### 8.8 `recovery_protocols.json` (new)

Protocols: observation, feeding, movement, pain review, follow-up.

### 8.9 `isolation_protocols.json` (new)

Protocols: room, staff, visit, release criteria, waste route.

### 8.10 `sterilization_cycles.json` (new)

Cycles: method, time, temperature or concentration, verification, failure mode.

### 8.11 `ward_supplies.json` (new)

Supplies: item, use, reorder, substitute, criticality.

### 8.12 `ward_chart_fields.json` (new)

Fields: name, required, consent flag, privacy level.

### 8.13 Items

New items appended to `items.json`: `item_ward_bed`, `item_isolation_screen`,
`item_surgical_tray`, `item_anesthetic_mask`, `item_sterile_dressing`,
`item_ward_apron`, `item_hand_scrub_brush`, `item_ward_lamp`,
`item_stretcher`, `item_wheel_chair`, `item_obs_chart`, `item_dressing_kit`,
`item_autoclave`, `item_bedpan`, `item_ward_key`, `item_waste_bin_sealed`.

---

## 9. SAVE, DETERMINISM, AND PERSISTENCE

### 9.1 Ownership

`MedicalWardSave`, `AdvancedSurgicalWardSave`, and `MedicalPipelineSave` remain
the live save owners. New sub-objects (triage, rooms, shifts, teams, trays,
recovery, isolation, sterilization, charts) are additive inside them. No new
save section.

### 9.2 State to persist

- Triage decisions and wait records.
- Bed, room, and isolation state.
- Shift rosters and fatigue.
- Surgery teams, trays, schedules, and milestone state.
- Recovery observations and follow-ups.
- Sterilization cycles and supply counts.
- Chart entries and consent flags.

### 9.3 Determinism

- Admissions, procedures, and milestones resolve through the live ward systems.
- Anesthesia, shock, and complications use the live surgery state and
  deterministic modifiers.
- Sterilization and supply outcomes derive from records and skill, not chance
  beyond the live seeded paths.
- Paired replay hashes must match; no wall-clock or `System.Random`.

### 9.4 Migration

Legacy saves load with beds, admissions, and procedures untouched; no triage,
roster, tray, or chart state exists until started. Existing `MedicalBed`
definitions keep working; new rooms are additive.

### 9.5 Checksum

Invariant-culture floats; integer day and hour counts.

---

## 10. UI, ACCESSIBILITY, AND PRESENTATION

### 10.1 Surfaces

| Surface | Purpose | Owner |
|---|---|---|
| `TriagePanel` (new) | Priority queue and reasons | `WardHostSession` |
| `WardBoardPanel` (new) | Beds, rooms, isolation | same |
| `SurgeryPanel` (new) | Teams, trays, schedule | same |
| `RecoveryPanel` (new) | Rounds and observations | same |
| `SterilePanel` (new) | Cycles and supplies | same |
| `ChartPanel` (new) | Records and consent | same |
| `WardReviewPanel` (new) | Reviews and lessons | same |

### 10.2 Accessibility and honesty

- Panels show live state and expose existing commands only.
- Priority is explained in plain language, visible to the player, never hidden.
- No flashing or alarm-heavy UI; state is readable at a glance.
- Keyboard/controller close/back preserved; focus maintained on refresh.
- Charts have text-only equivalents and can be read aloud.
- High-contrast and scalable text for ward boards.

### 10.3 Presentation

Audio cues appended to `audio_cues.json`: a door swing, a tray set down, a
kettle, boots on tile, a quiet bell, a chart page turned. No cue is required;
text carries meaning.

---

## 11. INTEGRATION SEAMS

| Existing system | Attachment |
|---|---|
| `MedicalWardSystem` | Beds, admissions, procedures |
| `AdvancedSurgicalWardSystem` | Theatre phases and outcomes |
| `MedicalProcedureSchedule` | Scheduling |
| `SurgicalProcedureCatalog` | Procedure content |
| `MedicalPipelineCoordinator` | Care routing |
| `NarcoticsSystem` | Analgesia |
| `DiagnosisKnowledgeStore` | Diagnostics |
| `MedicalRecordLog` | Records |
| `MedicalReservationLedger` | Supplies |
| `DiseaseProtocolHandler` | Disease care |
| `DiseaseQuarantineCoordinator` (Wave 2) | Isolation |
| `NeedsSystem` | Rest, food, pain |
| `ShelterAssignmentSystem` | Rooms |
| `DutyRoster` (Exp 02) | Shifts |
| `WaterTreatmentSystem` (Wave 3) | Sterile water |
| `SanitationSystem` (Wave 3) | Waste |
| `ShelterFireHazardSystem` (Wave 3) | Burn pit |
| `MemorialSystem` | Losses |
| `EpilogueChronicleBuilder` | Ward milestones |

---

## 12. TECHNICAL IMPLEMENTATION PLAN

### 12.1 Phase order

**Phase 0 — Premise re-audit.** Confirm ward, surgery, schedule, narcotics,
diagnostics, and records owners. Record file:line; change nothing.

**Phase 1 — Data + validators.** Author catalogs; extend
`surgical_procedures.json`; append items; register validators and scanner.

**Phase 2 — Pure Core.** `TriageSystem`, `WardOpsSystem`, `SurgeryRosterSystem`,
`RecoveryCareSystem`, `IsolationSystem`, `SterileSupplySystem`,
`WardRecordsSystem`.

**Phase 3 — Persistence.** Additive sub-objects, migration, round-trip,
determinism.

**Phase 4 — Host + CLI.** `WardHostSession`, selftest coverage, fresh journey
from collapse to review.

**Phase 5 — UI.** New panels with lifecycle and accessibility.

**Phase 6 — Content.** Rooms, locations, NPCs, quests, items, prose, audio.

**Phase 7 — Balance.** 360-day soak: surge, worker injury, shift fatigue.

**Phase 8 — Verification and closeout.**

### 12.2 Content volume

| Content | Count |
|---|---|
| Triage rules | 6 |
| Ward rooms | 10 |
| Shifts | 8 |
| Surgery teams | 8 |
| Surgical trays | 10 |
| Procedures | 30 |
| Recovery protocols | 10 |
| Isolation protocols | 8 |
| Sterilization cycles | 8 |
| Ward supplies | 20 |
| Chart fields | 16 |
| Items | 16 |
| Locations | 10 |
| Rooms | 8 |
| NPCs | 8 |
| Main quests | 15 |
| Side quests | 30 |
| Repeatable | 8 |
| Endings | 5 + fade |
| Prose estimate | 55,000–70,000 words |

### 12.3 Risks

| Risk | Severity | Mitigation |
|---|---|---|
| Graphic content | Critical | Phase-only representation |
| Torture or experimentation | Critical | Care-neutral contract |
| Second medical system | Critical | Extend live owners |
| Hidden healing bonus | High | Observations only |
| Nurse fatigue ignored | High | Rota and relief |
| Hygiene theater | Medium | Recorded cycles |
| Determinism break | Low | Live seeded paths |
| Content overrun | Medium | Budget §13 |

---

## 13. APPENDIX A — AUTHORING BUDGET

| File | Rows | Est. words |
|---|---|---|
| `triage_rules.json` | 6 | 2,000 |
| `ward_capacity.json` | 10 | 2,000 |
| `ward_rooms.json` | 10 | 3,000 |
| `ward_shifts.json` | 8 | 2,500 |
| `surgery_teams.json` | 8 | 2,500 |
| `surgical_trays.json` | 10 | 2,500 |
| `surgical_procedures.json` (extension) | 30 | 8,000 |
| `recovery_protocols.json` | 10 | 2,500 |
| `isolation_protocols.json` | 8 | 2,000 |
| `sterilization_cycles.json` | 8 | 2,000 |
| `ward_supplies.json` | 20 | 3,000 |
| `ward_chart_fields.json` | 16 | 2,500 |
| Quest objectives | 53 quests | 15,000 |
| NPC prose | 8 NPCs | 7,000 |
| Location prose | 10 | 3,500 |
| Item descriptions | 16 | 2,500 |
| Ending prose | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 14. APPENDIX B — RISK REGISTER

| ID | Risk | Likelihood | Impact | Mitigation |
|---|---|---|---|---|
| R38-1 | Graphic surgery | Low | Critical | Phase representation |
| R38-2 | Torture content | Low | Critical | Care-neutral contract |
| R38-3 | Second medical system | Low | Critical | Live owners |
| R38-4 | Hidden healing | Med | High | Observation records |
| R38-5 | Fatigue ignored | Med | High | Rota |
| R38-6 | Isolation stigma | Med | High | Strict kindness rule |
| R38-7 | Supply hoarding loop | Med | Med | Reorder tables |
| R38-8 | Determinism | Low | High | Live paths |
| R38-9 | Content overrun | Med | Med | Budget §13 |
| R38-10 | Records drift | Med | Med | Chart fields |

---

## 15. APPENDIX C — OPEN DECISIONS REQUIRING A FOREMAN SIGNATURE

1. **Does triage ever refuse care?** Recommended: yes under genuine scarcity,
   always recorded, always reviewable, never the default.
2. **Are patients' charts visible to the player?** Recommended: yes at ward
   level with privacy flags honored; sealed fields stay sealed.
3. **Can the ward fail permanently?** Recommended: no; wards degrade and recover
   through review and repair.
4. **Is anesthesia a dependency risk?** Recommended: yes, clinical pressure only,
   routed through the live narcotics and dependency owners.
5. **Does the ward treat prisoners and outsiders identically?** Recommended: yes,
   unconditionally, as a hard content rule.

---

## 17. APPENDIX D — SURGICAL PROCEDURE TABLE (30 PROCEDURES)

| # | Procedure | Duration | Team | Supplies | Recovery |
|---|---|---|---|---|---|
| 1 | Wound debridement | 1h | surgeon, nurse | tray A, dressing | 3 days |
| 2 | Wound closure | 1h | surgeon, nurse | tray A, thread | 2 days |
| 3 | Burn dressing | 1h | surgeon, nurse | dressing, salve | 7 days |
| 4 | Abscess drainage | 1h | surgeon, nurse | tray B | 3 days |
| 5 | Fracture setting | 1h | surgeon, aide | splints | 14 days |
| 6 | Traction setup | 2h | surgeon, aide | weights, rope | 21 days |
| 7 | Joint washout | 2h | surgeon, nurse | tray C, saline | 7 days |
| 8 | Foreign body removal | 1h | surgeon, nurse | tray B | 3 days |
| 9 | Dental extraction | 1h | surgeon | tray D | 2 days |
| 10 | Eye foreign body | 1h | surgeon, optic | tray E | 3 days |
| 11 | Appendicitis | 3h | full team | tray F | 10 days |
| 12 | Hernia repair | 3h | full team | tray F | 10 days |
| 13 | Bowel repair | 4h | full team | tray G | 14 days |
| 14 | Cesarean referral | 4h | full team, midwife | tray G, warm cot | 14 days |
| 15 | Amputation | 3h | surgeon, prosthetics | tray H | 21 days |
| 16 | Skin graft | 3h | surgeon, nurse | tray I | 14 days |
| 17 | Bone pinning | 4h | full team | tray J | 21 days |
| 18 | Chest drain | 2h | surgeon, nurse | tray K | 10 days |
| 19 | Airway clearance | 1h | surgeon, anesthetist | tray E | 2 days |
| 20 | Hemorrhage control | 1h | surgeon, nurse | tray A, pressure | 5 days |
| 21 | Rad debridement | 3h | full team | tray H, lead kit | 21 days |
| 22 | Prosthetic fitting | 2h | prosthetics, surgeon | socket kit | 30 days |
| 23 | Scar revision | 2h | surgeon, nurse | tray A | 7 days |
| 24 | Tendon repair | 4h | full team | tray L | 28 days |
| 25 | Nerve exploration | 4h | full team | tray L | 28 days |
| 26 | Reconstructive graft | 5h | full team | tray I, tray M | 30 days |
| 27 | Chronic wound care | 1h | nurse, aide | dressing | ongoing |
| 28 | Stoma care | 2h | surgeon, nurse | tray N | 21 days |
| 29 | Extraction of shrapnel | 2h | surgeon, nurse | tray B | 7 days |
| 30 | Palliative procedure | 2h | surgeon, comfort team | tray A | referred |

Procedures are the ward's vocabulary. The table is written so that every
intervention has a duration, a team, and a recovery path, which means the ward
can plan instead of improvising.

---

## 18. APPENDIX E — TRIAGE RULE TABLE

| Rule | Order | Strength | Weakness | When chosen |
|---|---|---|---|---|
| Severity first | critical to walking | saves worst | ignores hope | default |
| Salvage first | most likely to live | efficient | hard faces | shortage |
| Queue | first come | fair | slow | calm |
| Family first | kin of residents | cohesive | unjust | crisis |
| Worker first | key trades | functional | callous | emergency |
| Rotation | by cohort | even | erratic | review-only |

Each rule is honest about what it sacrifices. The expansion's requirement is
that the shelter writes down which rule it uses and reviews the consequences;
nothing is hidden and nothing is decided by mood.

---

## 19. APPENDIX F — WARD ROOM AND BED TABLE

| Room | Beds | Isolation | Water | Vent | Condition |
|---|---|---|---|---|---|
| Triage | 0 | no | yes | pass | warm |
| Main ward | 6 | no | yes | pass | clean |
| Isolation | 2 | yes | yes | separate | strict |
| Recovery | 4 | no | yes | pass | near stove |
| Theatre | 1 table | no | yes | filtered | sterile |
| Scrubs | 0 | no | yes | pass | drained |
| Sterile store | 0 | no | yes | dry | locked |
| Office | 0 | no | no | pass | warm |
| Overlflow tent | 4 | partial | carried | open | temporary |
| Rail head bay | 2 | no | carried | open | casualty |

Every bed is a place with a room, a condition, and a set of constraints. The
table is how the ward stops being an abstract count and starts being a building.

---

## 20. APPENDIX G — SHIFT TABLE

| Shift | Hours | Staff | Relief | Fatigue rule |
|---|---|---|---|---|
| Morning | 06–14 | nurse, aide, clerk | 8h | rest logged |
| Afternoon | 14–22 | nurse, aide | 8h | rest logged |
| Night | 22–06 | nurse, orderly | 8h | double cover |
| Surgery day | 08–16 | team | one theatre | no double shift |
| Call | 24h | surgeon, anesthetist | on call | post-call rest |
| Weekend | rota | reduced | rotation | review |
| Crisis | 12h | all | staggered | captain approval |
| Recovery | as needed | aide | takeover | feed and move |

The shift table exists because the expansion treats exhaustion as a clinical
risk. A ward that respects the rota makes fewer mistakes than one that respects
heroism.

---

## 21. APPENDIX H — SURGERY TEAM TABLE

| Team | Lead | Members | Procedures | Training |
|---|---|---|---|---|
| Basic | surgeon | nurse | 1–10 | in-house |
| Abdominal | surgeon | nurse, aide | 11–14 | advanced |
| Orthopedic | surgeon | aide, prosthetics | 15, 17, 22 | advanced |
| Soft tissue | surgeon | nurse | 16, 23, 26 | advanced |
| Chest | surgeon | anesthetist, nurse | 18, 19 | advanced |
| Trauma | surgeon | full team | 20, 21, 29 | trauma |
| Neuro-tendon | surgeon | full team | 24, 25 | advanced |
| Comfort | surgeon | comfort team | 30 | palliative tie |

Teams are named so the player can see who is on call, who is tired, and who has
never assisted. The comfort team is included because dying people still deserve
skill.

---

## 22. APPENDIX I — SURGICAL TRAY TABLE

| Tray | Instruments | Cycle | Verification | Procedures |
|---|---|---|---|---|
| A | knife, scissors, needle | steam 20m | strip check | 1, 2, 20, 23, 29, 30 |
| B | knife, probe, forceps | steam 20m | strip check | 4, 8, 29 |
| C | knife, syringe, tube | steam 25m | strip check | 7 |
| D | dental set | boil | visual | 9 |
| E | fine tools | steam 20m | strip check | 10, 19 |
| F | abdominal set | steam 30m | two strips | 11, 12 |
| G | abdominal extended | steam 30m | two strips | 13, 14 |
| H | bone set | steam 30m | strip check | 15, 21 |
| I | graft set | steam 25m | strip check | 16, 26 |
| J | pinning set | steam 30m | two strips | 17 |
| K | drain set | steam 25m | strip check | 18 |
| L | tendon set | steam 30m | two strips | 24, 25 |
| M | reconstruction set | steam 30m | two strips | 26 |
| N | stoma set | steam 25m | strip check | 28 |

Trays turn sterilization from a mood into an object. Every tray has an owner, a
cycle, and a check, and the check is the thing the ward talks about after a bad
week.

---

## 23. APPENDIX J — RECOVERY PROTOCOL TABLE

| Protocol | Day band | Focus | Observations | Referral |
|---|---|---|---|---|
| Watch | 0–1 | vitals, warmth | hourly | any change |
| Feed | 1–3 | small food, water | per meal | kitchen |
| Move | 2–5 | sitting, standing | daily | aide |
| Walk | 4–10 | short walks | daily | none |
| Pain review | daily | analgesia | chart | anesthetist |
| Wound check | 2, 5, 9 | dressing | per check | surgeon |
| Mood check | 3, 7 | quiet talk | note | counselor |
| Strength | 5–14 | light tasks | per day | none |
| Follow-up | discharge | plan, date | written | clinic |
| Home note | discharge | household limits | written | family |

Recovery is a protocol, not a waiting period. The table is also how the ward
proves it gave active care and not just a bed.

---

## 24. APPENDIX K — ISOLATION PROTOCOL TABLE

| Aspect | Rule | Exception | Owner |
|---|---|---|---|
| Room | separate, ventilated | overflow wing | ward |
| Staff | dedicated | emergency | head nurse |
| Apron | single use | shortage wash | orderly |
| Hands | scrub on entry and exit | never | all |
| Visits | glass or distance | dying case | head nurse |
| Food | separate dishes | none | kitchen |
| Waste | sealed, burned | none | sanitation |
| Linen | separate wash | none | laundry |
| Release | criteria met | medical advice | doctor |
| Dignity | name used always | never waived | all |

Isolation is strict and kind at the same time. The dignity row is at the bottom
of the table and at the top of the ward's priorities.

---

## 25. APPENDIX L — STERILIZATION CYCLE TABLE

| Method | Time | Check | Failure mode | Use |
|---|---|---|---|---|
| Steam | 20–30m | strip | cold pocket | instruments |
| Boil | 15m | timer | scorch | small tools |
| Chemical | 30m | solution test | expired bath | surfaces |
| Dry heat | 60m | beads | uneven | glassware |
| Flame | instant | visual | damage | loops |
| Filtered air | continuous | flow test | blockage | theatre |
| Soap and scrub | 5m | ledger | skipped | hands |
| Burn | open | record | recontamination | waste |

Sterilization is a record before it is a method. Every cycle has a check and
every failure has a cause, which is why the ward's infection rate is a number it
can defend.

---

## 26. APPENDIX M — WARD SUPPLY TABLE

| Supply | Use | Reorder at | Substitute | Critical |
|---|---|---|---|---|
| Dressing | wounds | 20 | clean cloth | yes |
| Thread | closure | 10 | sinew | yes |
| Salve | burns | 6 | herb base | yes |
| Analgesia | pain | 12 | cold, rest | yes |
| Splints | fractures | 4 | board | yes |
| Saline | washout | 8 | boiled water | yes |
| Aprons | barrier | 10 | rewash | yes |
| Soap | hand hygiene | 6 | ash lye | yes |
| Fuel | lamps, boil | weekly | stores | yes |
| Linen | beds | 12 | wash cycle | yes |
| Pins | fixation | 6 | reuse sterilized | yes |
| Drains | chest | 4 | tube kit | yes |
| Splint pads | comfort | 8 | cloth | no |
| Recording paper | charts | 30 | press run | yes |
| Keys | stores | 1 | duplicate | no |
| Waste bags | disposal | 10 | sealed bins | yes |

Supplies are the ward's honesty. The reorder column exists so that the hospital
does not run out of the items that keep people alive while the player is busy
with something else.

---

## 27. APPENDIX N — WARD CHART FIELD TABLE

| Field | Required | Consent | Seal | Purpose |
|---|---|---|---|---|
| Name | yes | no | ward | identity |
| Day in | yes | no | ward | record |
| Complaint | yes | yes | sealed | context |
| Triage | yes | no | ward | priority |
| Vitals | yes | no | ward | trend |
| Procedure | yes | yes | sealed | care |
| Anesthesia | yes | yes | sealed | safety |
| Supplies used | yes | no | ward | stock |
| Observations | yes | no | ward | rounds |
| Complications | yes | yes | sealed | learning |
| Analgesia | yes | yes | sealed | care |
| Consent | if able | yes | sealed | right |
| Family note | optional | yes | sealed | continuity |
| Follow-up | yes | no | ward | discharge |
| Outcome | yes | no | ward | review |
| Review note | weekly | no | ward | improve |

The chart is the ward's memory and the patient's protection. The consent and
seal columns are the expansion's legal conscience, expressed as data.

---

## 28. APPENDIX O — MAIN QUESTLINE STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_ward_collapse` | 4 | Three patients, one door |
| `quest_ward_desk` | 4 | Triage rule written |
| `quest_ward_tray` | 4 | Sterile tray discipline |
| `quest_ward_rule` | 4 | Hand-hygiene ledger |
| `quest_ward_rota` | 4 | Rota tested |
| `quest_ward_theatre` | 5 | Full scheduled procedure |
| `quest_ward_round` | 4 | Rounds find a problem |
| `quest_ward_chart` | 4 | Charts catch an error |
| `quest_ward_isolation` | 5 | Separate care, same dignity |
| `quest_ward_overflow` | 5 | Surge handled |
| `quest_ward_exhaustion` | 4 | Collapse and rota rebuild |
| `quest_ward_recovery` | 4 | Patient out of bed |
| `quest_ward_review` | 4 | Blameless review |
| `quest_ward_winter_surge` | 5 | Cold-season surge |
| `quest_ward_middle_of_night` | 3 | Final disposition |

---

## 29. APPENDIX P — SIDE QUEST STAGE DETAIL

| Quest | Stages | Objective kernel |
|---|---|---|
| `quest_ward_priority` | 4 | Rule drafted and posted |
| `quest_ward_wait` | 3 | Waiting made humane |
| `quest_ward_capacity` | 3 | Real bed count |
| `quest_ward_transfer` | 4 | Safe transfer |
| `quest_ward_door` | 3 | Door manned |
| `quest_ward_team` | 5 | Team assembled |
| `quest_ward_trays` | 4 | Trays built and checked |
| `quest_ward_schedule` | 4 | Procedures scheduled |
| `quest_ward_checkpoint` | 4 | Milestones hit |
| `quest_ward_shock` | 5 | Bad turn managed |
| `quest_ward_rounds` | 3 | Rounds running |
| `quest_ward_feed` | 3 | Recovery feeding |
| `quest_ward_move` | 3 | Early movement |
| `quest_ward_pain` | 4 | Analgesia reviewed |
| `quest_ward_followup` | 3 | Follow-up written |
| `quest_ward_separate` | 4 | Isolation room set |
| `quest_ward_hygiene` | 4 | Strict hygiene |
| `quest_ward_visits` | 3 | Safe visits |
| `quest_ward_vent` | 3 | Airflow improved |
| `quest_ward_end` | 4 | Safe release |
| `quest_ward_sterile` | 4 | Loop verified |
| `quest_ward_linen` | 3 | Laundry cycle |
| `quest_ward_waste` | 3 | Waste route |
| `quest_ward_supply` | 4 | Instrument count |
| `quest_ward_keys` | 3 | Store controlled |
| `quest_ward_charts` | 4 | Charts complete |
| `quest_ward_consent` | 3 | Consent recorded |
| `quest_ward_review_month` | 4 | Monthly review |
| `quest_ward_training` | 4 | Staff trained |
| `quest_ward_archive` | 3 | Ward archive |

---

## 30. APPENDIX Q — NPC DOSSIERS (BRIEF)

**Halden Rooke** — surgeon. Steady hands, dry humor, no patience for an
unsterile tray. Believes the procedure is the easy part and the preparation is
the craft.

**Bel Arun** — head nurse. Runs triage, rounds, and the hand-hygiene ledger
with equal seriousness. Protects her staff from the roster as fiercely as she
protects patients from infection.

**Mar Quill** — anesthetist. Keeps sleep and pain in the same ledger. Refuses
to let one person cover two theatres and has the exhaustion records to prove
why.

**Tobin** — ward clerk. Writes charts that a stranger could continue from and
keeps the keys to the sterile store on a chain around his neck.

**Sena** — recovery aide. Believes recovery is work and that a person who sits
up today walks next week. Feeds people who do not want to eat.

**Pim** — orderly. Sterilizes, launders, and hauls, and knows the infection rate
depends on him more than on anyone.

**Junia** — isolation nurse. Works the separate room and refuses to let
separation become exile; uses names constantly.

**Arl** — patient. Foundry injury, long recovery, and a stubborn insistence on
walking again.

---

## 31. APPENDIX R — LOCATION DETAIL

- **The Ward Door** — where a person becomes a patient and keeps their name.
- **The Stretcher Yard** — arrivals, handovers, and the first assessment.
- **The Laundry Shed** — the war on infection fought with boiling water.
- **The Clinical Waste Pit** — sealed, burned, recorded.
- **The Herb Bed** — simple remedies beside real medicine.
- **The Quiet Corner** — families wait here and are told the truth.
- **The Overflow Tent** — surge capacity with the same standards.
- **The Burn Pit** — contaminated cloth and the smell of a hard week.
- **The Ward Tank** — the water reserve that decides whether the theatre runs.
- **The Rail Head** — casualty arrivals from the corridor.

---

## 32. APPENDIX S — ERROR REVIEW PROTOCOL

| Step | Action | Tone |
|---|---|---|
| Report | anyone may report | protected |
| Record | facts only | blameless |
| Trace | find the missing step | systemic |
| Hear | everyone in the chain | respectful |
| Change | one procedure, written | concrete |
| Train | brief and specific | quiet |
| Check | verify back later | follow-up |
| Thank | thank the reporter | explicit |

The protocol is the ward's immune system. The expansion's rule is that mistakes
are found by systems, fixed by procedures, and never punished by shame.

---

## 33. APPENDIX T — WORKED 360-DAY WARD SCENARIO

**Days 1–20.** Foundry collapse: three injured, no triage rule, one near-miss.

**Days 21–50.** Triage desk established; hand-hygiene ledger started; the first
sterile trays built and checked; two infections traced to a cold pocket in steam.

**Days 51–80.** Morning rounds introduced; charts standardized; Tobin finds a
pain-dose error before it harms anyone.

**Days 81–110.** Night rota tested; a nurse collapses from exhaustion and the
rota is rebuilt around eight hours; Mar's records are entered into evidence.

**Days 111–140.** Theatre runs a scheduled abdominal procedure start to finish;
milestone discipline holds; recovery protocols begin feeding on day one.

**Days 141–170.** Isolation case arrives from the gate; separate room set; a
family visit at glass; the patient keeps their name and their visitors.

**Days 171–200.** Overflow tent goes up for a wave of respiratory cases; the
triage rule holds under real pressure; the tent is stocked by rota not heroism.

**Days 201–230.** Sterilization cycle audit finds two trays with no strip; both
are re-cycled; the check is added to the tray table.

**Days 231–260.** Winter surge: fractures, cold, and a contaminated arrival;
ward coordinates with decon and the watch; no one is refused.

**Days 261–290.** Blameless review of the foundry week; three procedures change;
the reporter of the tray issue is thanked in front of the whole ward.

**Days 291–330.** Recovery wins: Arl walks to the yard; follow-up plan written;
the ward archive opens its first volume.

**Days 331–360.** Year review published; infection rate down; the ward is now an
institution with habits, not a room with good intentions.

---

## 34. APPENDIX U — VIGNETTES (TONE SAMPLE)

> Bel stands at the triage desk with three names on a board and says the rule
> out loud, and the rule is severity first, and everyone hears it, and that is
> the whole point of the board.

> Pim sets the tray down and checks the strip and the strip is grey when it
> should be black, and he carries the whole tray back to the sterilizer without
> being asked, and later someone thanks him for it in writing.

> Junia sits on the far side of the glass and talks to the person on the other
> side about their garden, and neither of them mentions the fever, and the fever
> is the reason for the glass and not the reason for the silence.

> Sena gets a patient to sit up on day two and the patient says it is too soon
> and Sena says it is exactly soon and holds the shoulder anyway, and by day six
> the patient walks, which is the only argument Sena ever needed.

---

## 35. APPENDIX V — DEATH IN THE WARD PROTOCOL

| Step | Action | Owner |
|---|---|---|
| First | Stop the procedure | surgeon |
| Second | Record the facts | clerk |
| Third | Tell the family in person | head nurse |
| Fourth | Provide company | comfort team |
| Fifth | Keep the chart sealed | records |
| Sixth | Review without blame | ward |
| Seventh | Change one thing | ward |
| Eighth | Route to grief owners | relations |
| Ninth | Offer a rite | 13 / 24 |
| Tenth | Honor the name | all |

Death is not a fail screen and not a lesson. The protocol exists so the ward
behaves like people instead of like a mechanic, and so the review that follows
is about the system and never about a person's worth.

---

## 36. APPENDIX W — FAILURE MODES AND RECOVERY

| Failure | Effect | Recovery |
|---|---|---|
| No triage rule | unfair waits | write and post |
| Cold pocket | infection | re-cycle, check strips |
| Missed round | late finding | double-check round |
| Chart gap | unsafe handoff | chart audit |
| Nurse exhaustion | errors | rota rebuild |
| Isolation breach | spread | strict reset |
| Supply out | procedure delayed | substitute list |
| Overflow chaos | standards slip | tent protocol |
| Theatre delay | shock | schedule discipline |
| Death | grief | protocol §35 |

No failure ends the ward. The ward's whole culture is built on the idea that a
problem is information, not a verdict.

---

## 37. APPENDIX X — CONTENT REVIEW CHECKLIST

- [ ] No graphic surgical content exists.
- [ ] No torture, experimentation, or interrogation in the ward.
- [ ] Prisoners and outsiders receive identical care.
- [ ] `MedicalWardSystem` remains the bed and admission authority.
- [ ] `AdvancedSurgicalWardSystem` remains the surgery authority.
- [ ] Dependency routes through the live handlers only.
- [ ] Recovery is active care, never a hidden bonus.
- [ ] Triage rules are visible and reviewable.
- [ ] Charts are complete and privacy-sealed.
- [ ] Save additions are additive and legacy-neutral.
- [ ] Determinism uses live seeded paths only.

---

## 38. APPENDIX Y — GLOSSARY

- **Triage** — the recorded decision about who is treated first.
- **Theatre** — the operating room and its team.
- **Tray** — a sterilized instrument set with a cycle record.
- **Round** — a scheduled review of every patient.
- **Chart** — the written record that continues care.
- **Isolation** — separate care with the same dignity.
- **Sterile store** — the locked instrument room.
- **Overflow** — temporary surge capacity holding the same standards.
- **Blameless review** — finding the missing step, never the guilty person.
- **Comfort team** — the people who make dying less lonely.

---

## 39. APPENDIX Z — INTEGRATION MATRIX

| System | Reads | Writes | Never writes |
|---|---|---|---|
| `MedicalWardSystem` | beds | admissions | triage policy |
| `TriageSystem` | capacity | priority records | bed state |
| `WardOpsSystem` | rooms | shifts, rounds | procedures |
| `SurgeryRosterSystem` | teams | schedules | outcomes |
| `AdvancedSurgicalWardSystem` | procedures | phases, survival | rosters |
| `RecoveryCareSystem` | ward state | observations | healing state |
| `IsolationSystem` | disease rules | isolation state | quarantine policy |
| `SterileSupplySystem` | inventory | cycles | sterilization policy |
| `WardRecordsSystem` | medical log | charts, consent | clinical facts |
| `NarcoticsSystem` | recipes | analgesia | dependency policy |
| `DiseaseProtocolHandler` | disease | protocols | ward state |
| `MedicalPipelineCoordinator` | referrals | routing | ward state |
| `MemorialSystem` | loss fact | memorial | chart |
| `DutyRoster` | shifts | rota | fatigue policy |
| `EpilogueChronicleBuilder` | milestones | chronicle | ward state |

---

## 40. APPENDIX AA — DATA SCHEMA DETAIL (NEW CATALOGS)

**`triage_rules.json`** — `rule_id`, `display_name`, `order[]`, `review_days`,
`notes`, `tags`.

**`ward_capacity.json`** — `room_id`, `beds`, `isolation_beds`, `overflow_at`,
`tags`.

**`ward_rooms.json`** — `room_id`, `display_name`, `beds`, `isolation`,
`ventilation`, `water`, `condition`, `tags`.

**`ward_shifts.json`** — `shift_id`, `hours`, `role`, `coverage`, `relief`,
`fatigue_rule`, `tags`.

**`surgery_teams.json`** — `team_id`, `display_name`, `lead`, `members[]`,
`procedures[]`, `training`, `tags`.

**`surgical_trays.json`** — `tray_id`, `instruments[]`, `cycle`,
`verification`, `procedures[]`, `tags`.

**`surgical_procedures.json` (extension)** — `procedure_id`, `display_name`,
`duration_hours`, `team`, `tray`, `supplies[]`, `complications[]`,
`recovery_days`, `tags`.

**`recovery_protocols.json`** — `protocol_id`, `day_band`, `focus[]`,
`observations`, `referral`, `tags`.

**`isolation_protocols.json`** — `aspect_id`, `rule`, `exception`, `owner`,
`tags`.

**`sterilization_cycles.json`** — `cycle_id`, `display_name`, `time_minutes`,
`check`, `failure_mode`, `use`, `tags`.

**`ward_supplies.json`** — `supply_id`, `display_name`, `use`, `reorder_at`,
`substitute`, `critical`, `tags`.

**`ward_chart_fields.json`** — `field_id`, `display_name`, `required`,
`consent`, `seal`, `purpose`, `tags`.

All new catalogs carry `schema_version: 1` and fail the integrity gate on missing
or duplicate IDs, invalid references, or out-of-range numbers.

---

## 41. APPENDIX AB — MEASUREMENT AND TELEMETRY

| Metric | Purpose | Source |
|---|---|---|
| Triage decisions recorded | fairness | Triage |
| Time to theatre | throughput | Roster |
| Sterile failure rate | safety | Sterile |
| Infection rate | quality | Ward |
| Round completeness | care | Recovery |
| Chart completeness | continuity | Records |
| Shift fatigue flags | staff health | Ops |
| Isolation breaches | safety | Isolation |
| Supply outages | logistics | Supplies |
| Review actions closed | learning | Review |

Telemetry is diagnostic only; it never gates content and never becomes a
ranking of staff or patients.

---

## 42. APPENDIX AC — IMPLEMENTATION CHECKLIST

- [ ] Phase 0 premise re-audit recorded with file:line evidence.
- [ ] Phase 1 catalogs authored and registered with validators and scanner.
- [ ] `surgical_procedures.json` extended beyond the 2 KB stub.
- [ ] Phase 2 Core systems are pure, engine-free, and deterministic.
- [ ] Phase 3 save additions are additive, legacy-neutral, and round-trip tested.
- [ ] Ward, surgery, and schedule authorities remain untouched.
- [ ] Triad parity (Setup / Save / Flush) holds for every new state.
- [ ] Phase 4 host session exposes live state and real commands only.
- [ ] Phase 5 panels pass bind/unbind/rebind, focus, contrast, and scaling checks.
- [ ] Phase 6 content passes the review checklist in §37.
- [ ] Phase 7 soak shows a triage surge, a theatre run, and a rota failure.
- [ ] Phase 8 data integrity and content utilization selftests pass.
- [ ] No graphic, torture, or experimentation content exists.

---

## 43. APPENDIX AD — OPEN QUESTIONS FOR REVIEW

1. Should triage rules be chosen by the player, the head nurse, or the
   steward?
2. Does the ward publish its infection rate, and to whom?
3. Should overflow reduce standards visibly or hold them at cost?
4. Can a patient refuse a procedure, and how is that recorded?
5. Is the comfort team a standing assignment or a rotating one?
6. Should staff exhaustion ever force a theatre to close?
7. How much of the chart is visible on the patient panel?
8. Does the ward ever treat animals, and who decides?

None of these may be decided unilaterally; each changes tone and balance.

---

## 44. APPENDIX AE — CROSS-WAVE HOOKS

| Wave | Expansion | Hook |
|---|---|---|
| 1 | 12 The Second Generation | Children visiting, learning care |
| 1 | 13 The Faithful | Rites for the sick and dying |
| 1 | 14 Above the Ash | Casualty arrivals by air |
| 1 | 15 The Deep Root | Herbs, food, and recovery diets |
| 1 | 16 The Rebuilt Body | Prosthetics and rehab |
| 2 | 17 The Long Evening | Music in the ward |
| 2 | 18 The Underneath | Ventilation and sealed rooms |
| 2 | 19 The Bitter Air | Decon and contaminated arrivals |
| 2 | 20 The Quiet Hand | Neutral care for prisoners |
| 2 | 21 The Grid | The theatre lamp and power |
| 3 | 22 The Clean Flow | Sterile water and waste |
| 3 | 23 The Alarm | Triage during emergencies |
| 3 | 24 The Long Goodbye | Comfort and palliative ties |
| 3 | 25 The Iron Road | Casualties by rail |
| 3 | 26 The Common Table | Recovery meals |
| 4 | 27 The Thread | Linen, aprons, and dressings |
| 4 | 28 The Lesson | Nurse and orderly training |
| 4 | 29 The Glass | Lamps, lenses, and instruments |
| 4 | 30 The Press | Charts, forms, and records |
| 4 | 31 The Kiln | Autoclave parts and rooms |
| 5 | 32 The Wild | Herbs and simple remedies |
| 5 | 33 The Weather | Winter surge |
| 5 | 34 The Long Road | Casualty transport |
| 5 | 35 The Habit | Analgesia pressure and care |
| 5 | 36 The Watch | Night coverage and stretcher teams |
| 6 | 37 The Quickening | Complicated births |

Each hook is additive. The Ward can ship alone, and every other expansion can
ship without it.

---

## 45. APPENDIX AF — ENDING PROSE SKETCHES

**The Quiet Ward.** Rounds run, trays check, charts fill, and the ward makes
fewer mistakes every month, which is the only outcome that matters.

**The Open Door.** The ward is small and never turns away an emergency, and the
triage board is honest about the cost of that promise.

**The Long Rota.** Staffing is tight and the ward survives on goodwill, and the
plan to fix it is written and posted where everyone can see it.

**The Hard Season.** The winter surge breaks the ward's routine and rebuilds it
around what actually worked, and the review is published in full.

**The Empty Bed.** A loss traced to a missing step is mourned by the whole ward,
and the step is added to the tray table, and nobody is blamed.

**Fade.** A lamp over a chart and someone checking a pulse in the dark, and the
ward is quiet, and quiet is what a working hospital sounds like.

---

## 46. APPENDIX AG — COMMON FAILURE PATTERNS TO AVOID

| Pattern | Why it is bad | Correct approach |
|---|---|---|
| Surgery close-up | spectacle | phases and outcomes |
| Miracle healing | dishonesty | observations and time |
| Blame mechanic | cruelty | blameless review |
| Prisoner experimentation | atrocity | identical care |
| Blood economy | commodification | abstracted screening |
| Chart as decoration | unsafe | fields that matter |
| Triage hidden | distrust | posted rules |
| Heroic all-nighters | exploitation | rota and relief |
| Isolation as exile | stigma | strict kindness |
| Death as fail state | cruelty | protocol and grief |

The list exists because hospital content is easy to turn into either horror or
fantasy. The expansion's rule is that good medicine is boring, recorded, and
kind, and that the ward's drama comes from doing ordinary things well under
pressure.

---

## 47. APPENDIX AH — CONTENT VOLUME SUMMARY

| Category | Rows | Prose estimate |
|---|---|---|
| Triage rules | 6 | 2,000 |
| Capacity | 10 | 2,000 |
| Ward rooms | 10 | 3,000 |
| Shifts | 8 | 2,500 |
| Surgery teams | 8 | 2,500 |
| Trays | 10 | 2,500 |
| Procedures | 30 | 8,000 |
| Recovery protocols | 10 | 2,500 |
| Isolation protocols | 8 | 2,000 |
| Sterilization cycles | 8 | 2,000 |
| Supplies | 20 | 3,000 |
| Chart fields | 16 | 2,500 |
| Quests | 53 | 15,000 |
| NPCs | 8 | 7,000 |
| Locations | 10 | 3,500 |
| Items | 16 | 2,500 |
| Endings | 6 | 3,000 |
| **Total** | | **~66,500** |

---

## 48. APPENDIX AI — FIRST YEAR OF THE WARD

| Month | Focus | Milestone |
|---|---|---|
| 1 | Collapse | triage desk |
| 2 | Hygiene | hand ledger |
| 3 | Trays | sterilization checks |
| 4 | Rota | eight-hour shifts |
| 5 | Theatre | first full schedule |
| 6 | Rounds | daily discipline |
| 7 | Charts | full records |
| 8 | Isolation | separate room |
| 9 | Overflow | surge protocol |
| 10 | Review | blameless process |
| 11 | Surge | winter handled |
| 12 | Year review | published |

A year of the ward is a year of habits built one at a time, and the hospital at
the end of it is the sum of every small correction the shelter made along the
way.

---

## 49. APPENDIX AJ — CARE-NEUTRAL CONTRACT

| Clause | Promise |
|---|---|
| Identity | Every patient keeps their name |
| Equality | Prisoners, outsiders, residents: same care |
| Consent | Recorded whenever a person is able |
| Privacy | Charts sealed by existing medical rules |
| Neutrality | No interrogation, no coercion, no experiments |
| Honesty | Rules and outcomes are visible |
| Review | Mistakes are systemic, not personal |
| Comfort | Dying is not left alone |
| Staff | Exhaustion is a risk, not a virtue |
| Learning | Every failure changes a procedure |

The contract is the ward's first-class design object. A hospital is a promise
that the worst day of a person's life will be met by a system instead of by
luck, and this table is that promise written down.

---

## 50. APPENDIX AK — OVERFLOW SURGE TABLE

| Surge level | Trigger | Space | Staff | Standards |
|---|---|---|---|---|
| Green | routine | ward | normal | full |
| Yellow | +2 beds | recovery | +1 aide | full |
| Orange | +4 beds | tent | rota call | full |
| Red | +8 beds | corridor | all hands | recorded |
| Black | mass casualty | any floor | captains | triage only |
| After | return | close tent | rest rota | review |

The table has one hard rule: standards are maintained until the level is Black,
and Black is always reviewed. Surge is a measurement of the shelter's preparation,
not an excuse for abandoning care.

---

## 51. APPENDIX AL — WARD TRAINING TABLE

| Skill | Prerequisite | Course | Hours | Recertify |
|---|---|---|---|---|
| Hand hygiene | none | induction | 1 | yearly |
| Sterile tray prep | hygiene | orderly | 8 | yearly |
| Triage clerk | literacy | clerk | 8 | yearly |
| Rounds note | literacy | clerk | 4 | yearly |
| Recovery aid | none | aide | 12 | yearly |
| Wound dressing | aid | nurse | 16 | yearly |
| Scrubbing | hygiene | theatre | 8 | yearly |
| Theatre assist | scrubbing | surgeon | 24 | yearly |
| Isolation care | hygiene | head nurse | 12 | yearly |
| Charting | literacy | clerk | 6 | yearly |

Training is short and local, and the table admits it. The shelter trains the
people it has and asks them to do the work their training supports, which is why
the ward is honest about what an orderly is and is not allowed to do.

---

## 52. APPENDIX AM — WARD EQUIPMENT TABLE

| Equipment | Room | Source wave | Upkeep |
|---|---|---|---|
| Bed frame | ward | 10 | repair |
| Mattress | ward | 27 | wash |
| Screen | isolation | 27 | wash |
| Table | theatre | 10 | level |
| Lamp | theatre | 21, 29 | fuel |
| Sterilizer | sterile | 10, 31 | seal |
| Tray set | sterile | 10 | replace |
| Scale | clinic | 29 | calibration |
| Crutches | recovery | 10 | repair |
| Wheelchair | recovery | 10 | repair |
| Basin | ward | 31 | clean |
| Laundry tub | laundry | 31 | clean |
| Burn bin | waste | 31 | replace |
| Chart cabinet | office | 30 | lock |
| Duty board | office | 30 | update |
| Water tank | yard | 22 | seal |

The ward is assembled out of every other wave's workshop. The table is also a
map of dependencies: a broken sterilizer is a kiln order, a burnt lamp is a
glass order, and a torn screen is a thread order.

---

## 53. APPENDIX AN — WARD REVIEW CADENCE TABLE

| Review | Frequency | Question | Owner |
|---|---|---|---|
| Hand hygiene | weekly | ledger honest? | head nurse |
| Tray audit | weekly | any missed check? | orderly |
| Chart audit | monthly | any gap? | clerk |
| Infection rate | monthly | up or down? | ward |
| Rota fatigue | monthly | anyone exhausted? | head nurse |
| Supply count | weekly | what runs out? | clerk |
| Isolation check | monthly | rules held? | isolation nurse |
| Blameless review | per event | what changed? | all |
| Thearte audit | quarterly | team and tray ready? | surgeon |
| Year review | yearly | what are we now? | ward |

Reviews make the ward a learning organization instead of a heroic one. The
year review is written, posted, and read by everyone including the people who
work in it, which is the only way improvement survives a change of staff.

---

## 54. APPENDIX AO — PATIENT JOURNEY TABLE

| Stage | Place | Owner | Record |
|---|---|---|---|
| Arrival | door | triage | priority |
| Assessment | desk | nurse | vitals |
| Admission | ward | clerk | chart |
| Preparation | scrub | orderly | tray |
| Procedure | theatre | surgeon | phases |
| Watch | recovery | aide | observations |
| Feeding | recovery | aide | intake |
| Movement | recovery | aide | daily |
| Review | office | ward | note |
| Discharge | door | clerk | follow-up |

Ten stages, ten owners, ten records. The journey table is what makes it
impossible for a patient to be lost in the shelter's middle, which is the
quietest and most common hospital failure of all.

---

## 55. APPENDIX AP — WARD CONSUMABLE AND DRESSING TABLE

| Consumable | Use | Daily use | Reorder | Substitute |
|---|---|---|---|---|
| Dressing cloth | wounds | 12 | 40 | clean cloth |
| Bandage roll | fixation | 8 | 25 | strips |
| Salve pot | burns | 2 | 6 | herb base |
| Analgesia dose | pain | 10 | 30 | rest, cold |
| Saline bottle | washout | 4 | 12 | boiled water |
| Apron | barrier | 6 | 20 | rewash |
| Soap bar | hygiene | 1 | 4 | ash lye |
| Linen set | beds | 6 | 20 | wash cycle |
| Chart sheet | records | 4 | 20 | press run |
| Waste bag | disposal | 5 | 15 | sealed bin |
| Thread spool | closure | 1 | 4 | sinew |
| Splint pad | comfort | 3 | 10 | cloth |

Consumables are the ward's quiet count. The daily-use and reorder columns make
logistics visible, and the substitute column is the ward's plan for the week the
supply run does not arrive.

---

## 56. APPENDIX AQ — SEASONAL WARD LOAD TABLE

| Season | Primary load | Secondary | Staffing | Supplies |
|---|---|---|---|---|
| Spring | fractures | colds | normal | splints, salve |
| Summer | wounds | heat cases | normal | dressing, water |
| Autumn | respiratory | strains | +1 aide | steam, blankets |
| Winter | cold, burns | fractures | +2 staff | fuel, salve |
| Ash | respiratory | burns | masks | filters |
| Surge | mixed | exhaustion | all hands | everything |

The ward is a seasonal system, and the table lets the shelter prepare in the
quiet months for the loud ones. Every row is drawn from live weather, works, and
foundry activity rather than being scripted.

---

## 57. CLOSING STATEMENT

ASHFALL already has beds, admissions, procedures, surgery phases, schedules,
analgesia, diagnostics, and records. What it lacks is the hospital around them:
triage, rooms, rota, teams, trays, rounds, isolation, sterilization, and honest
charts. The Ward adds that middle without adding a second medical system or a
spectacle. It adds a desk that decides honestly, a tray that is always sterile, a
nurse who is always relieved, and a room where nobody is ever just a case.

> Wave 6 note: this plan is one of five Wave 6 expansion bibles (37–41). Each is
> self-contained; none requires another to ship. The shared Wave 6 index lives at
> `docs/expansions/wave6/WAVE6_INDEX.md`. The safe pre-signature step is Phase 1
> (data schemas and validators), which is additive and reversible. Evidence
> anchors: `MedicalWardSystem` (`Beds`, `Procedures`, `Admit`, `Discharge`,
> `RunProcedure`, `CaptureState`, `RestoreState`, `GetBedOccupant`,
> `GetActiveAdmission`), `AdvancedSurgicalWardSystem` (`SurgicalOperationState`
> with `shock_percent`, `anesthesia_level`, milestone flags, `patient_survived`,
> `recovery_days_remaining`, `complications`), `MedicalProcedureSchedule`
> (`procedureId`, `survivorId`, `remainingHours`, `reservationIds`, `status`),
> `NarcoticsSystem` (`NarcoticDefinition` with `tolerance_gain`,
> `dependency_pressure`), `SurgicalProcedureCatalog`, `DiagnosisKnowledgeStore`,
> `MedicalRecordLog`, and `MedicalReservationLedger`.