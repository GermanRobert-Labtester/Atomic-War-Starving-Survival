# ASHFALL — WAVE 4 INTEGRATION PROGRAM · PLAN 6 OF 6

# MEDICINE, RADIATION & THE BODY INTEGRATION PLAN

**Status:** PROPOSAL — planning-only · no production path claimed
**Wave:** W4 (six-plan integration wave — the shelter's remaining machinery)
**Document:** W4-06
**Date:** 2026-09-21
**Repo:** `Atomic War` @ `Zcode_Branch`, HEAD `5be1a30a`
**Companion plans:** W4-01 (save/state), W4-02 (world/travel), W4-03 (infrastructure), W4-04 (ecology), W4-05 (society)
**Plan-unblocking annex:** Annex U at the end — separately.

---

## 0. How to read this plan

This plan integrates **the body**: one injury/affliction truth, one triage pipeline,
one dose/radiation authority, one disease/quarantine path, one treatment and procedure
grammar, pharmacy and dependency, surgery and prosthetics, the vigil and the record,
and the surfaces through which care is understood. It extends existing owners — the
medical pipeline, the body state, the dose ledger — and it never builds a parallel
injury channel, a second dose counter, or a private triage queue.

### 0.1 Two selection levels

| Level | Choice | Granularity |
|---|---|---|
| **Level 1** | Plan Path **A**, **B**, or **C** | the whole plan's posture |
| **Level 2** | ten decision points, each **A/B/C** | per-concern depth |

### 0.2 Default mapping

| Plan Path | A points | B points | C points |
|---|---|---|---|
| A Truth & Triage | 1–10 | — | — |
| B One Body | 1,2,3 | 4,5,6,7 | 8,9,10 |
| C Care Across Years | — | 2,5,8 | 1,3,4,6,7,9,10 |

### 0.3 The Wave 4 rule for this plan

> **One affliction truth, one dose ledger, one disease authority, one record.**
> `SurvivorBodyState` + affliction contracts own injuries and conditions;
> `MedicalPipelineCoordinator` owns triage and scheduling; `DoseLedgerSystem` +
> `RadiationSystem` own exposure; `DiseaseSystem` + `PathogenStrainSystem` own
> outbreaks. No parallel wound list, no second dosimeter truth, no health math in
> panels or other systems.

### 0.4 Vocabulary

| Term | Meaning |
|---|---|
| affliction | a modeled body condition with phases, causes, and handlers |
| body state | the authoritative record of limbs, senses, and chronic conditions |
| pipeline | triage → reservation → treatment/procedure → recovery |
| reservation | a bookable capacity on an operator/ward/bed |
| dose | accumulated radiation exposure, ledgered per survivor |
| pathogen strain | an authored disease with transmission and severity |
| containment | quarantine/isolation capability and its costs |
| procedure | a scheduled surgical/medical act with inputs and outcomes |
| vigil | the end-of-life care state machine and its dignity rules |
| record | the persistent medical log (facts, not prose duplication) |

---

## 1. Premise audit — what P0 must verify (Rule 7)

```text
[ ] Assets/Ashfall.Core/Medical/: MedicalPipelineCoordinator + Save,
    MedicalWardSystem + Save + MedicalWardPipelineBridge, AdvancedSurgicalWardSystem,
    AfflictionContracts + AfflictionId + AfflictionDutyBridge,
    amputation/bionics/prosthetics: AmputationSystem, BionicsSystem,
    ProstheticConditionWearEngine, RehabilitationProgressionEngine,
    SurvivorBodyState + SurvivorBodyPresentationSlate,
    DiagnosisKnowledgeStore, MicrofluidicDiagnosticEngine + catalog loader,
    MedicalReservationLedger, MedicalProcedureSchedule, MedicalRecordLog,
    PatientRecord + IntegrityValidator, MedicalTreatmentCatalog,
    SurgicalProcedureCatalog, PharmaceuticalTabletEngine, LyophilizationSystem,
    MutationSystem, NarcoticsSystem, ChemicalDependencySystem + handler,
    RespiratoryDegenerationSystem, StressRelapseRules, VigilCare + VigilStateMachine,
    affliction handlers (Disease/Radiation/Psychology/Chemical/Respiratory)
[ ] Assets/Ashfall.Core/Radiation/: RadiationSystem, Dosimeter +
    DosimeterCalibrationSystem, RadiationPhaseProgression, LowBackgroundLeadEngine,
    ExposureBreakdown, ExposureEnvironment
[ ] dose: DoseLedgerSystem + DoseLedgerSave + DoseContentCatalog + DoseRegistersCatalog
    + DoseQuestMigration
[ ] Assets/Ashfall.Core/Disease/: DiseaseSystem, DiseaseCatalog, DiseaseQuarantineCoordinator,
    DiseaseTriage, ContainmentCapability, PathogenStrainSystem + PathogenStrainSave,
    IDiseaseOutbreakSource
[ ] Sanatorium: PsychologicalSanatoriumSystem (coordinate W3-03; do not duplicate)
[ ] host: src/Main.Medical.cs, Main.MedicalTriage.cs, Main.UiTests.Dose.cs
[ ] existing rulings: UNBLOCK-01 (F14 body integrity F14-C/D/E/F/G),
    XP-06 body-integrity schema, D19 statics (shared service nulls),
    EXPANSION-16 (rebuilt body) seam
[ ] data: medical/treatment/procedure/disease/dose catalogs under
    Assets/StreamingAssets/Data/
```

### 1.1 Evidence posture

- Proposal only; no path claims; no ledger rows; read-only until Annex U.
- Core engine-free; JSON authoritative; determinism through existing seeded paths.
- One authority per concern; additive fields only; no parallel medical simulation.
- Dignity: no clinical-advice framing; chronic outcomes respected; no gore; the dead
  and the injured keep their names and their place.
- Focused verification per `TEST_POLICY.md`.

### 1.2 Known anchors (verify, don't trust)

| Anchor | Why it matters |
|---|---|
| `SurvivorBodyState` + F14 wave | body schema already integrated (UNBLOCK-01 waves) |
| `MedicalPipelineCoordinator` | the pipeline exists; do not fork triage |
| `DoseLedgerSystem` | the dose ledger is the exposure truth |
| `PathogenStrainSave` | strain state already persists |
| `VigilStateMachine` | end-of-life care already modeled — dignity lives here |
| `W3-03` psychology plan | mental health owned there; coordinate, never duplicate |

---

## 2. The three Plan Paths

### 2.1 Path A — Truth & Triage

Audit every medical data set and consumer: affliction coverage, handler routing,
pipeline stages, dose paths, disease lifecycle, and surfaces. Produce the body ledger;
repair orphans and silent asymmetries only.

### 2.2 Path B — One Body

Unify: one affliction truth, one triage queue, one reservation ledger, one dose truth,
one disease authority, one record — consumed by needs, morale, labor, and surfaces
without local health math.

### 2.3 Path C — Care Across Years

Make care historical: chronic conditions that persist with respect, recovery arcs,
dependency management, generational health (coordinate UNBLOCK-01), and a medical
record that survivors and the shelter carry across the campaign — deterministic and
save-backed via W4-01.

---

## 3. The ten decision points

### 3.1 Point 1 — Body state and affliction authority

**Owner anchor:** `SurvivorBodyState`, `AfflictionContracts`, `AfflictionId`,
affliction handlers.
**Decision:** every injury/condition is an affliction instance routed to its handler;
body state is the single record; no system invents its own wound fields.

- **A:** enumerate afflictions and handlers vs live data; find unhandled IDs and
  duplicate fields (e.g., limbs tracked twice).
- **B:** registration: new afflictions declare ID, phases, handler, and presentation;
  handlers route effects to owners.
- **C:** chronic/permacondition policy: what heals, what persists, what compensates —
  with dignity rules and save migration.

**Verify:** affliction registration coverage; handler routing; body-state round-trip.
**Never:** a second wound store, a hidden injury, or a condition without a handler.

### 3.2 Point 2 — Triage and the care pipeline

**Owner anchor:** `MedicalPipelineCoordinator`, `MedicalReservationLedger`,
`MedicalProcedureSchedule`, `MedicalWardSystem`.
**Decision:** one queue: symptoms → triage priority → reservation → care → recovery,
with capacity and operator skill as real constraints.

- **A:** audit pipeline stages vs host wiring; find patients who never enter, or enter
  twice.
- **B:** one coordinator; reservations keyed; schedules respect operators and time;
  events emit facts for surfaces.
- **C:** surge arcs: mass-casualty triage, staged care, and recovery time at scale.

**Verify:** pipeline traversal; reservation uniqueness; schedule determinism; surge
scenario.
**Never:** a private triage list, double admission, or care without a reservation.

### 3.3 Point 3 — Radiation and the dose ledger

**Owner anchor:** `RadiationSystem`, `DoseLedgerSystem` + save/catalog, `Dosimeter*`,
`RadiationPhaseProgression`, `ExposureBreakdown/Environment`.
**Decision:** one exposure truth: dose accumulates per survivor through the ledger,
phases advance deterministically, dosimeters tell the truth, and effects route through
affliction handlers.

- **A:** audit every writer to dose; find double-counting (environment + event) and
  unledgered exposure.
- **B:** one ledger write path; phase progression reads the ledger; dosimeter display
  reads owner; effects through handlers.
- **C:** long-horizon arcs: chronic dose, low-background living, and generational
  exposure (coordinate UNBLOCK-01).

**Verify:** writer inventory; double-count scenario; phase boundaries; dosimeter truth.
**Never:** a second dose counter, exposure without a ledger row, or dose math in UI.

### 3.4 Point 4 — Disease and quarantine

**Owner anchor:** `DiseaseSystem`, `PathogenStrainSystem` + save, `DiseaseQuarantineCoordinator`,
`DiseaseTriage`, `ContainmentCapability`, `IDiseaseOutbreakSource`.
**Decision:** outbreaks are modeled: strains transmit, triage sorts, containment costs
real capability, and endings (recovery/death/immunity) are recorded once.

- **A:** audit strains and outbreak sources vs live consumers; find inert strains and
  untriggered sources.
- **B:** one outbreak lifecycle; transmission bounded and warned; quarantine consumes
  beds/supplies/labor; outcomes once.
- **C:** epidemic arcs: multi-wave outbreaks, immunity, and institutional memory of
  procedures.

**Verify:** transmission bounds; quarantine cost; outcome-once; save round-trip.
**Never:** random mass death without a modeled chain, or duplicate outbreak state.

### 3.5 Point 5 — Treatment and procedures

**Owner anchor:** `MedicalTreatmentCatalog`, `SurgicalProcedureCatalog`,
`AdvancedSurgicalWardSystem`, `MedicalTreatmentCatalog`, `LyophilizationSystem`.
**Decision:** treatments and procedures are authored acts with prerequisites, inputs,
risks, and recovery — scheduled through the pipeline, never applied instantly.

- **A:** audit catalog rows vs consumers; find unreachable procedures and free heals.
- **B:** one prescription path; procedure prerequisites enforced (skill, tools, meds);
  risk modeled; recovery scheduled.
- **C:** care quality arcs: skill growth, better procedures, and outcomes that reflect
  the shelter's investment.

**Verify:** catalog coverage; prerequisite enforcement; scheduling; outcome variance.
**Never:** an instant full heal, or a procedure without prerequisites.

### 3.6 Point 6 — Pharmacy and dependency

**Owner anchor:** `PharmaceuticalTabletEngine`, `ChemicalDependencySystem`,
`NarcoticsSystem`, `ChemicalDependencyAfflictionHandler`, `LyophilizationSystem`.
**Decision:** medicines are made, dosed, and dependence is a real affliction with
warned onset and manageable course; narcotics relieve and bind.

- **A:** audit medicines/dependencies vs recipes and handlers; find double-dose paths.
- **B:** one dosing path; dependency onset warned and handler-routed; withdrawal
  managed; supply from W3-05 craft chains.
- **C:** long-term arcs: dependency management, tapering, and the cost of relief.

**Verify:** dosing once; dependency progression; withdrawal management; supply linkage.
**Never:** free unlimited medication, or an unwarned addiction.

### 3.7 Point 7 — Surgery, amputation, bionics, prosthetics

**Owner anchor:** `AmputationSystem`, `BionicsSystem`, `ProstheticConditionWearEngine`,
`RehabilitationProgressionEngine`, `SurvivorBodyPresentationSlate`.
**Decision:** surgery changes the body with consequence: procedures are scheduled and
risked, limbs and prosthetics are recorded in body state, wear and rehab follow the
F14 engines, and dignity rules bind all presentation.

- **A:** audit the F14 seam consumers; find fields not consumed or displays that
  overstate capabilities.
- **B:** one body mutation path; wear/rehab through their engines; grip/mobility reads
  consumed by actions (W3-04 combat checks, W3-05 crafts).
- **C:** years of adaptation: condition cycles, refits, and acceptance arcs recorded.

**Verify:** mutation round-trip; wear/rehab integration; capability consumption;
dignity review.
**Never:** a surgery with no record, free bionics, or capability claims the body
does not have.

### 3.8 Point 8 — Vigil, death, and the record

**Owner anchor:** `VigilCare`, `VigilStateMachine`, `MedicalRecordLog`, `PatientRecord`
+ integrity validator; coordinate `MemorialSystem` (W3-03).
**Decision:** dying is cared for: vigil states have dignity rules, death is recorded
as fact, records persist, and memoria/resources route to existing owners.

- **A:** audit vigil paths and record consumers; find deaths without records or records
  without faces.
- **B:** one vigil machine; records keyed; notification and memorial routing once.
- **C:** institutional memory: outbreak/procedure history readable across years.

**Verify:** vigil state coverage; death-once; record round-trip; memorial routing.
**Never:** a death with no record, or a vigil with no care semantics.

### 3.9 Point 9 — Diagnosis and knowledge

**Owner anchor:** `DiagnosisKnowledgeStore`, `MicrofluidicDiagnosticEngine` + loader,
`MutationSystem`.
**Decision:** knowing is gameplay: diagnosis requires tools/knowledge, results are
recorded, and uncertainty is visible; mutation changes the picture over time.

- **A:** audit diagnostic paths and knowledge gates; find auto-diagnosis and free
  certainty.
- **B:** one diagnostic result path; knowledge store consumed by treatments; uncertainty
  modeled.
- **C:** medical progress arcs: better tools, better knowledge, and faster, surer care.

**Verify:** diagnostic gating; result recording; uncertainty display; mutation effects.
**Never:** free omniscience, or a diagnosis without a source record.

### 3.10 Point 10 — Medical surfaces, morale, and aftermath

**Owner anchor:** `Main.Medical.cs`, `Main.MedicalTriage.cs`, ward/triage surfaces
(W3-06 coordination), journal (W3-01), morale (W3-03).
**Decision:** care is legible: patients, states, queues, and outcomes are truthful; the
tone is restrained; aftermath (recovery, loss, duty) routes to existing owners.

- **A:** surface audit: fabricated vitals, missing states, clinical coldness, missing
  causes.
- **B:** surfaces read owners; every outcome states its cause; messages canonical;
  dignity copy reviewed.
- **C:** aftermath arcs: recovery time respected, duties adjusted, and grief routed
  through W3-03 owners.

**Verify:** W3-06 kits over medical surfaces; cause-present test; tone review;
aftermath routing.
**Never:** fabricated medical data, instant recovery presentation, or gore.

---

## 4. Selection sheet

```text
ASHFALL WAVE 4 · PLAN W4-06 · SELECTION SHEET

Plan Path:   [ ] A Truth & Triage   [ ] B One Body   [ ] C Care Across Years

Points (mark A/B/C or leave default):
 1 body/afflictions ........ [ ]
 2 triage pipeline ......... [ ]
 3 radiation/dose .......... [ ]
 4 disease/quarantine ...... [ ]
 5 treatment/procedures .... [ ]
 6 pharmacy/dependency ..... [ ]
 7 surgery/bionics ......... [ ]
 8 vigil/death/record ...... [ ]
 9 diagnosis/knowledge ..... [ ]
10 surfaces/aftermath ...... [ ]

Selected by: ____________   Date: ________   Foreman: ____________
```

---

## 5. Phase ladder

| Phase | Name | Exit |
|---|---|---|
| P0 | Premise audit + body ledger | ledger filed; premises re-verified |
| P1 | Affliction + pipeline discipline | routing; reservation; schedule tests green |
| P2 | Dose + disease | ledger writes; phases; outbreak lifecycle green |
| P3 | Treatment + pharmacy | prerequisite/scheduling; dosing/dependency green |
| P4 | Surgery + F14 consumers | mutation/wear/rehab/capability green |
| P5 | Vigil + diagnosis + surfaces | records; diagnostic gates; W3-06 kits; tone review |
| P6 | Closeout | evidence pack; determinism; limitations recorded |

---

## 6. Non-goals, never-touch, one-authority

**Non-goals**

- No clinical-advice framing or real-world medical claims.
- No second body/dose/disease store; no health math outside handlers.
- No gore; no torture content; chronic outcomes are respected.
- No psychology duplication (W3-03 owns mental health).

**Never-touch**

- UNBLOCK-01 / F14 body-integrity closures and XP-06 schema decisions.
- D19c statics conclusions unless re-verified with evidence.
- Sealed debt rows; quarantined tests.
- Shared paths claimed by other packages.

**One authority per concern**

| Concern | Owner |
|---|---|
| body/afflictions | `SurvivorBodyState` + affliction contracts/handlers |
| pipeline | `MedicalPipelineCoordinator` + reservations/schedule |
| dose | `DoseLedgerSystem` + `RadiationSystem` + dosimeters |
| disease | `DiseaseSystem` + `PathogenStrainSystem` + quarantine coordinator |
| treatment | treatment/procedure catalogs + ward systems |
| pharmacy | tablet engine + dependency/narcotics owners |
| body mutation | amputation/bionics/prosthetics + rehab engines |
| vigil/record | vigil machine + record log |
| diagnosis | knowledge store + diagnostic engine |

---

## 7. Verification and acceptance

- **T1 static:** body ledger; writer inventory to dose; duplicate-store scans;
  surface-source audit; tone checklist.
- **T2 focused:** affliction routing; pipeline traversal; reservation uniqueness; dose
  double-count scenario; phase boundaries; outbreak lifecycle; outcome-once; treatment
  prerequisites; dosing; dependency; mutation round-trip; wear/rehab; capability
  consumption; vigil states; record round-trip; diagnostic gating; W3-06 kits.
- **T3 soak:** 60-day medical load at seed: admissions, outcomes, dose trend, zero drift.
- **Acceptance:** evidence pack + Annex U signature; compile-green is not acceptance.

## 8. Handoffs and dependencies

| Direction | Detail |
|---|---|
| W3-03 | psychology, grief, memorial routing; sanatorium coordination |
| W3-04 | combat injuries, prisoners' care, coercion limits |
| W3-05 | medicines, prosthetics, surgical tools craft chains |
| W4-01 | every new medical state ships its save section |
| W4-03 | water/air/thermal exposure sources |
| W4-04 | nutrition deficiency and contamination sources |
| W2-02 | silent-failure and absence rules in medical paths |
| UNBLOCK-01 | body presentation and condition engines (F14-C..G) |

---

# ANNEX U — PLAN-UNBLOCKING (SEPARATELY)

## U.1 What this plan releases

| Release | Unblocks |
|---|---|
| U1 | body ledger work blocked on "which injury fields are canonical" |
| U2 | dose integration held for double-writer confirmation |
| U3 | outbreak/containment depth blocked on strain-save evidence |
| U4 | F14 consumer work blocked on capability-read inventory |
| U5 | medical surfaces blocked on W3-06 registry + tone review |

## U.2 Signature block

```text
ASHFALL WAVE 4 · PLAN W4-06 · RELEASE SIGNATURE
HEAD: ________  Date: ________
[ ] P0 premise audit completed and filed
[ ] body ledger exists; duplicate writers listed
[ ] no path claimed outside the package
[ ] focused test targets named
[ ] rollback position recorded
Signed: ________   Foreman: ________
```

## U.3 Never-touches

- No edit to another Wave 4 plan's claimed paths.
- No re-open of UNBLOCK-01/F14 closures without evidence.
- No change to W3-03 psychology contracts.
- No revival of quarantined tests outside the documented procedure.

## U.4 Release rule

> This plan executes only after U.2 is signed. Until then it is read-only planning.

---

*End of W4-06 — Part I. Expansion parts continue on the established Wave 3 pattern.*---

# W4-06 · PART II — DEEP DESIGN: BODY STATE, TRIAGE, RADIATION (POINTS 1–3)

## II.1 The body ledger: schema and meaning

One row per medical concern: afflictions, pipeline, dose, disease, pharmacy,
surgery, vigil, diagnosis.

```yaml
body_ledger:
  - id: body.state
    owner: "SurvivorBodyState + affliction contracts/handlers"
    state: ["limbs", "senses", "chronic_conditions", "affliction_instances"]
    consumers: ["pipeline", "actions (grip/mobility)", "surfaces", "W3-04"]
    save_section: "body.state"
    surfaces: ["body slate", "ward board"]
    warnings: ["affliction_onset", "chronic_progress", "capability_loss"]
    tests: ["AfflictionRoutingTests", "BodyRoundTripTests"]
  - id: dose.ledger
    owner: "DoseLedgerSystem + RadiationSystem + Dosimeter"
    ...
```

### II.1.1 Ledger rules

```text
L1  one owner per medical concern; no shadow injuries
L2  every affliction instance has a handler; unhandled ids are build failures
L3  every health outcome applies once, keyed
L4  every harm preceded by a warning with an authored window
L5  every save section obeys W4-01 budgets and bounds
L6  every surface reads; no panel computes health
```

### II.1.2 The handler law

An affliction without a handler is a defect, and a handler without an
affliction is dead code. The registry asserts both directions at build time.

## II.2 Body state and afflictions

### II.2.1 Affliction instance

```jsonc
{
  "id": "aff_0091",
  "kind": "laceration",
  "source": "combat",
  "phase": "bleeding",           // onset | bleeding | stabilizing | healing | resolved
  "severity": 2,
  "since_day": 212,
  "handler": "trauma_handler",
  "capabilities": { "grip": "simple_only", "mobility": "permille_820" }
}
```

### II.2.2 Rules

```text
F1  one affliction truth; body state is the single record
F2  handlers route effects to owners (needs, morale, labor) — never compute
F3  capability reads (grip, mobility, senses) are consumed by actions
F4  chronic conditions persist with dignity and compensation paths
F5  no second wound fields anywhere; no hidden injuries
F6  affliction phases advance deterministically with care/treatment inputs
```

### II.2.3 The capability contract

```text
grip:       full | simple_only | none
mobility:   permille (1000 = full)
vision:     clear | impaired | lost
breathing:  clear | labored | assisted
auditory:   clear | impaired | lost

actions declare which capabilities they read; the body supplies them.
```

## II.3 Triage and the care pipeline

### II.3.1 Pipeline stages

```text
arrival -> triage -> reservation -> care -> recovery -> discharge
            |           |            |         |
            +-- priority + capacity   +-- skill +-- record
```

### II.3.2 Rules

```text
P1  one queue; triage priority authored and visible
P2  reservations keyed; no double admission; operators/beds are real limits
P3  schedules respect campaign time and operator availability
P4  surge scales by staging, not by magic capacity
P5  pipeline events emit facts for surfaces; no direct UI writes
P6  death in care is recorded as a fact; no silent loss
```

### II.3.3 Priority table (authored; verified at P0)

| Priority | Condition class | Behavior |
|---|---|---|
| P0 | immediately life-threatening | seen first, always |
| P1 | severe, time-sensitive | fast-track |
| P2 | serious, stable | standard |
| P3 | minor | deferred under load |
| P4 | maintenance | scheduled last |

## II.4 Radiation and the dose ledger

### II.4.1 The dose truth

```text
DoseLedgerSystem owns accumulated exposure per survivor; RadiationSystem owns
environmental fields; Dosimeter* read and display; phase progression reads the
ledger.
```

### II.4.2 Rules

```text
R1  one writer to dose: the ledger; environment submits exposure events
R2  the "last applied day" marker co-locates with the dose (W4-01)
R3  phase progression reads totals deterministically; authored thresholds
R4  dosimeters display truth; calibration is a real device state
R5  acute/chronic routing goes through affliction handlers
R6  no second counter; no double application across load (the classic bug)
```

### II.4.3 The phase table (authored)

| Phase | Dose band | Symptoms | Route |
|---|---|---|---|
| clean | 0–band1 | none | — |
| elevated | band1–2 | fatigue, nausea | handler |
| acute | band2–3 | serious sickness | handler + care |
| severe | 3+ | life-threatening | handler + pipeline |

## II.5 Worked example: the dose that doubled

**Report:** a survivor's meter read ~2× expected after loading mid-expedition.

**Walk:**

```text
1. root: ambient tick re-applied the day at restore because the applied-day
   marker lived in a different section
2. repair: co-locate marker with dose (R2); one write path; paired test
3. verify: save-load-continue exposure test; reconciliation
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| MD-01 | marker in wrong section | ownership |
| MD-02 | no load-continue exposure test | coverage |

*End of Part II. Continues in Part III (disease, treatment, pharmacy).*---

# W4-06 · PART III — DEEP DESIGN: DISEASE, TREATMENT, PHARMACY (POINTS 4–6)

## III.1 Disease and quarantine

### III.1.1 Outbreak record

```jsonc
{
  "id": "ob_004",
  "strain": "str_flu_a",
  "index_day": 208,
  "cases": [ { "survivor": "sv_014", "phase": "incubating" } ],
  "containment": "ward_isolation",
  "wave": 1
}
```

### III.1.2 Rules

```text
D1  one outbreak lifecycle; strains authored with transmission and severity
D2  transmission bounded and deterministic; incubation real
D3  quarantine consumes capability (bed, supplies, labor) and reduces spread
D4  outcomes (recovery/immunity/death) apply once, keyed
D5  multi-wave arcs are authored; immunity persists (W4-01)
D6  no random mass death; every chain names its cause
```

### III.1.3 The containment table

| Capability | Cost | Effect |
|---|---|---|
| ward isolation | beds + labor | spread ↓ |
| decon protocol | supplies + time | environmental ↓ |
| limited contact | morale − | spread ↓ |
| none | — | spread free |

## III.2 Treatment and procedures

### III.2.1 Treatment act

```jsonc
{
  "id": "tx_044",
  "kind": "surgical_repair",
  "patient": "sv_014",
  "prereqs": { "skill": "surgeon_2", "tools": ["surgical_kit"], "meds": ["anesthetic"] },
  "risk": "moderate",
  "recovery": { "days": 14, "care": "ward" }
}
```

### III.2.2 Rules

```text
T1  one prescription path; treatments authored with prereqs and risks
T2  prerequisites enforced (skill/tools/meds); missing = refusal with reasons
T3  risk modeled; outcomes authored (success | complication | failure)
T4  recovery scheduled through the pipeline; not instant
T5  care quality affects outcomes via skill/supplies, deterministically
T6  procedures consume materials from W3-05 chains
```

## III.3 Pharmacy and dependency

### III.3.1 Medicine chain

```text
substance -> formulation -> dose -> effect
effects route through handlers; supply through crafting (W3-05)
```

### III.3.2 Rules

```text
P1  one dosing path; no double-dose from parallel systems
P2  dependency is a real affliction with warned onset and a course
P3  withdrawal is managed through the pipeline; not left to chance
P4  narcotics relieve and bind; the binding is authored and visible
P5  medicines spoil or expire per authored rules; storage matters
P6  no free unlimited medication; every dose traces to supply
```

### III.3.3 The dependency curve (authored)

| Stage | Behavior | Warning |
|---|---|---|
| exposure | occasional use | none yet |
| habit | regular use | dependency_forming |
| dependent | withdrawal on gap | dependency_warned |
| managed | tapering path | dependency_managed |
| clear | resolved | — |

## III.4 Worked example: the medicine that never ran out

**Report:** painkillers never depleted despite daily use.

**Walk:**

```text
1. root: dosing read a separate stock used only by the panel; the real stock
   was never decremented
2. repair: one stock owner; dosing decrements it; panel reads it; test
3. verify: consumption reconciliation over a month
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| MD-03 | shadow stock | Rule 5 |
| MD-04 | no consumption reconciliation | coverage |

*End of Part III. Continues in Part IV (surgery, vigil, diagnosis, surfaces).*---

# W4-06 · PART IV — DEEP DESIGN: SURGERY, VIGIL, DIAGNOSIS, SURFACES (POINTS 7–10)

## IV.1 Surgery, amputation, bionics, prosthetics

### IV.1.1 The F14 seam (already integrated)

```text
body mutation -> body state records -> wear/rehab engines -> capability reads
```

### IV.1.2 Rules

```text
S1  one body-mutation path; procedures scheduled and risked
S2  amputations/prosthetics recorded in body state; wear via F14 engines
S3  rehabilitation progression uses the F14 engine; no parallel rehab
S4  capabilities consumed by actions (combat, crafts, travel) — truthfully
S5  no free bionics; materials and skill required
S6  dignity: chronic outcomes respected; presentation never mocks
```

### IV.1.3 The capability-consumer table

| Action family | Reads | Failure mode |
|---|---|---|
| combat (W3-04) | grip, mobility | authored penalties |
| crafts (W3-05) | grip, vision | slower/failed |
| travel (W4-02) | mobility, breathing | slower/refused |
| care (W4-06) | vision, grip | care quality |

## IV.2 Vigil, death, and the record

### IV.2.1 Vigil states

```text
begin -> attend -> quiet -> passing -> recorded
          |         |
          +-- care +-- family (W3-03) notified once
```

### IV.2.2 Rules

```text
V1  one vigil machine; states with dignity rules and paced copy
V2  death recorded as a fact (day, cause, place); no silent vanish
V3  records persist; memoria routes to MemorialSystem (W3-03) once
V4  notification and aftermath route through owners; once only
V5  no gore; no spectacle; the record carries the weight
V6  a death closes all open medical states for that survivor (sentences,
    reservations, dependencies flagged for record)
```

## IV.3 Diagnosis and knowledge

### IV.3.1 Diagnostic act

```jsonc
{
  "id": "dx_102",
  "patient": "sv_014",
  "method": "microfluidic",
  "result": "strain_flu_a",
  "confidence": "high",
  "recorded": true
}
```

### IV.3.2 Rules

```text
K1  one diagnostic result path; knowledge store consumed by treatments
K2  uncertainty modeled and displayed; no free certainty
K3  tools gate quality of diagnosis; failure yields "inconclusive" with a path
K4  results recorded per patient; bounded; migrations (W4-01)
K5  mutation changes the picture over time (authored)
K6  no auto-diagnosis; someone must look
```

## IV.4 Medical surfaces and aftermath

### IV.4.1 Surface contract

```text
triage board:  patients, priorities, wait states, reasons
ward board:    beds, occupants, care plans, warnings
pharmacy:      stock, expiry, dependency states
dose board:    per-survivor totals, phase, dosimeter state
records:       history, verdict-like clarity (facts, not prose)
```

### IV.4.2 Rules

```text
X1  all surfaces read owners; zero local health math
X2  every outcome states its cause; every refusal its reason
X3  tone restrained; no clinical-advice framing; no gore
X4  units and thresholds present; bands for uncertainty
X5  history bounded (per-event), saved (W4-01)
X6  aftermath (recovery, loss, duty) routes to owners once
```

## IV.5 Worked example: the ward that never discharged

**Report:** patients stayed "in recovery" forever.

**Walk:**

```text
1. root: discharge required a condition (bed clear) that the ward itself
   controlled but never evaluated because the care loop stopped at recovery
2. repair: discharge is a scheduled step; timeouts with authored fallbacks;
   stalled admissions are findings
3. verify: admission-to-discharge test; stall sweep
```

**Findings:**

| ID | Class | Repair |
|---|---|---|
| MD-05 | stalled discharge | completeness |
| MD-06 | no stall sweep | coverage |

*End of Part IV. Continues in Part V (playbooks).*---

# W4-06 · PART V — PLAYBOOKS

## V.1 The P0 premise playbook

```text
1. freeze HEAD; enumerate afflictions, handlers, pipeline stages, dose paths,
   strains, treatments, procedures, medicines, dependencies, vigil paths,
   diagnostics, records
2. for each: find the live owner and consumers; list unhandled/unconsumed
3. list every writer to dose; find double-application paths
4. read treatments/procedures vs catalogs; find unreachable or free acts
5. list capability readers (combat/craft/travel/care); find bypasses
6. read dependency/narcotics paths; find double-dose or free supply
7. read vigil/death paths; find unrecorded deaths
8. confirm F14 engine consumption (wear/rehab/capabilities)
9. write the premise note; claim paths; draft the package row
```

## V.2 The affliction authoring playbook

```text
1. id, kind, phases, severity bands, handler
2. source attribution (combat, environment, disease, dependency)
3. capability effects (declared reads)
4. warning at onset; progression deterministic with care inputs
5. resolution path (heal, chronic, loss) with dignity rules
6. test: routing + phases + capability reads
```

## V.3 The treatment authoring playbook

```text
1. kind, prereqs (skill/tools/meds), risk bands, recovery schedule
2. consumes materials through W3-05; no free acts
3. outcomes authored: success | complication | failure
4. refusal reasons with refs when prereqs missing
5. record written; surfaces read
6. test: prereq matrix + outcome variance
```

## V.4 The dose authoring playbook

```text
1. exposure event: source, amount, day
2. ledger is the only writer; marker co-located
3. phase bands authored; affinity for handlers
4. dosimeter state read; calibration real
5. test: double-application, load-continue, phase boundaries
```

## V.5 Anti-pattern drills

**Drill 1 — the second wound store.** Find an injury field outside body
state; delete.

**Drill 2 — the free heal.** Find a treatment with no consumption or prereqs;
halt.

**Drill 3 — the dose double.** Force the classic reload case; the marker must
prevent it.

**Drill 4 — the silent death.** Find a death without a record; halt.

**Drill 5 — the health math in UI.** Find a panel computing a health outcome;
route to the handler.

*End of Part V. Continues in Part VI (verification catalog).*---

# W4-06 · PART VI — VERIFICATION CATALOG

## VI.1 T1 — static

| ID | Check | Fails when |
|---|---|---|
| T1.1 | handler coverage | affliction without handler (both directions) |
| T1.2 | second-store scan | wounds/dose/disease tracked twice |
| T1.3 | free-act scan | treatment without prereqs/consumption |
| T1.4 | dose-writer scan | exposure applied outside the ledger |
| T1.5 | capability-bypass scan | actions reading raw fields |
| T1.6 | record scan | death/verdict-like outcomes without records |
| T1.7 | tone scan | gore/clinical-advice phrasing |
| T1.8 | wall-clock scan | recovery/dose on real time |

## VI.2 T2 — focused per point

### Point 1 — body/afflictions
```text
T2.1.1 routing: each affliction to handler; effects via owners
T2.1.2 phases advance deterministically with care inputs
T2.1.3 capability reads consistent (grip/mobility/vision)
T2.1.4 round-trip of body state; neutral load
```

### Point 2 — pipeline
```text
T2.2.1 admission→discharge complete; no stall
T2.2.2 reservation uniqueness; capacity limits
T2.2.3 schedule respects campaign time/operators
T2.2.4 surge staging; priority order correct
```

### Point 3 — dose
```text
T2.3.1 double-application prevented across load
T2.3.2 phase boundaries exact; handler routing
T2.3.3 dosimeter truth vs ledger
T2.3.4 reconciliation of exposure events
```

### Point 4 — disease
```text
T2.4.1 transmission bounds; incubation timing
T2.4.2 quarantine cost/effect; capability consumption
T2.4.3 outcomes once; immunity persists
T2.4.4 multi-wave arcs authored
```

### Point 5 — treatment
```text
T2.5.1 prereq enforcement with refusals
T2.5.2 outcome paths (success/complication/failure)
T2.5.3 recovery scheduling; care quality effect
T2.5.4 material consumption via W3-05
```

### Point 6 — pharmacy
```text
T2.6.1 dosing once; stock decrement
T2.6.2 dependency onset warned; course managed
T2.6.3 withdrawal handled through pipeline
T2.6.4 expiry/spoilage authored
```

### Point 7 — surgery/F14
```text
T2.7.1 mutation path single; records written
T2.7.2 wear/rehab via F14 engines
T2.7.3 capability consumption by actions
T2.7.4 no free bionics; costs real
```

### Point 8 — vigil
```text
T2.8.1 states paced; death recorded once
T2.8.2 memoria routed once; notification once
T2.8.3 open medical states closed on death
T2.8.4 tone review
```

### Point 9 — diagnosis
```text
T2.9.1 tools gate quality; uncertainty displayed
T2.9.2 results recorded; bounded
T2.9.3 mutation changes picture (authored)
T2.9.4 no auto-diagnosis
```

### Point 10 — surfaces
```text
T2.10.1 reads only; no local math
T2.10.2 causes/reasons present
T2.10.3 units + bands
T2.10.4 history bounded + round-trip
T2.10.5 W3-06 kits over medical surfaces
```

## VI.3 T3 — seeded soak

```text
60 days: one outbreak, one mass-casualty, one surgery program, one dependency
arc, one death, one dose incident
assert: outcomes once; records complete; no stalls; reconciliation zero;
        no double dose; replay green
```

## VI.4 Evidence formats

```yaml
run: T2.3.1
date: ____  head: ____
case: save mid-exposure -> load -> continue
dose_before: __  dose_after: __  expected: __
result: pass
```

*End of Part VI. Continues in Part VII (worked threads).*---

# W4-06 · PART VII — WORKED THREADS AND FINDINGS (MD-07–MD-20)

## VII.1 Thread A — "the wound that healed twice"

**Report:** a laceration healed, then reopened after a reload.

**Walk:**

```text
1. root: healing applied at event, but the phase cursor was persisted without
   the healing event's key; restore re-ran the phase step
2. repair: phase advances carry keys; restore reads; test
3. verify: load-mid-healing test
```

| ID | Class | Repair |
|---|---|---|
| MD-07 | phase without key | integrity |
| MD-08 | no mid-healing test | coverage |

## VII.2 Thread B — "the triage that ignored the dying"

**Report:** a P0 patient waited behind P3s.

**Walk:**

```text
1. root: priority was assigned at arrival but re-sorted by arrival time later
2. repair: one sort authority; priority wins; test
3. verify: priority-order test
```

| ID | Class | Repair |
|---|---|---|
| MD-09 | order override | fairness |
| MD-10 | no order test | coverage |

## VII.3 Thread C — "the immunity that reset"

**Report:** survivors caught the same strain twice.

**Walk:**

```text
1. root: immunity recorded in the outbreak instance, dropped at outbreak end
2. repair: immunity is per-survivor durable state; test
3. verify: re-infection test
```

| ID | Class | Repair |
|---|---|---|
| MD-11 | transient immunity | durability |
| MD-12 | no re-infection test | coverage |

## VII.4 Thread D — "the anesthetic that never existed"

**Report:** surgeries succeeded with no anesthetic in stock.

**Walk:**

```text
1. root: the meds prereq list was unchecked in the simple path
2. repair: all paths share prereq enforcement; test both paths
3. verify: prereq path matrix
```

| ID | Class | Repair |
|---|---|---|
| MD-13 | unchecked prereqs | Rule 5 |
| MD-14 | no matrix test | coverage |

## VII.5 Thread E — "the death that left a chair"

**Report:** a dead survivor stayed on the duty roster.

**Walk:**

```text
1. root: death closed medical states but not duty assignments
2. repair: death closes all references (roster, schedules, reservations),
   with a record; test
3. verify: reference-cleanup test
```

| ID | Class | Repair |
|---|---|---|
| MD-15 | partial death cleanup | completeness |
| MD-16 | no cleanup test | coverage |

## VII.6 Thread F — "the diagnosis that read the future"

**Report:** tests identified a strain before exposure.

**Walk:**

```text
1. root: diagnostic read the outbreak's full case list instead of the patient
2. repair: diagnostics read the patient's own state; test
3. verify: patient-scope test
```

| ID | Class | Repair |
|---|---|---|
| MD-17 | wrong scope | correctness |
| MD-18 | no scope test | coverage |

## VII.7 Thread G — "the dependency that never came"

**Report:** daily narcotics use produced no dependency.

**Walk:**

```text
1. root: dependency accumulation existed but was never ticked
2. repair: consumption feeds the curve; warns at bands; test
3. verify: dependency-curve test
```

| ID | Class | Repair |
|---|---|---|
| MD-19 | inert dependency | completeness |
| MD-20 | no curve test | coverage |

## VII.8 The summary

```text
A: phases carry keys
B: priority wins the sort
C: immunity is durable
D: prereqs check on every path
E: death closes every reference
F: diagnostics read their patient
G: dependency accumulates and warns
```

*End of Part VII. Continues in Part VIII (Q&A).*---

# W4-06 · PART VIII — QUESTIONS AND ANSWERS

**Q1. What is the medical plan's core law?**
One body truth, one dose ledger, one pipeline, one record.

**Q2. Why one affliction truth?**
Because wounds are the classic duplicated state: combat, environment, and
disease all want their own field. One record, many sources.

**Q3. What is a handler?**
The one function that turns an affliction fact into effects on owners.

**Q4. What makes harm fair?**
A warning window, a cause, and a route back where the design allows one.

**Q5. What are capabilities for?**
So the body's truth reaches actions: grip, mobility, vision, breathing.

**Q6. What makes triage trustworthy?**
Priority authored, sort authoritative, capacity real, reasons visible.

**Q7. What stops admission double-booking?**
Reservation keys and one queue.

**Q8. What makes surge playable?**
Staging and deferral with warnings — never magic capacity.

**Q9. What is the dose ledger's single job?**
Own every unit of exposure, applied once, with its day.

**Q10. Why is the applied-day marker co-located?**
Because the classic bug is a marker in another section applying exposure
twice after a load.

**Q11. What makes radiation fair?**
Bands, warnings, dosimeter truth, and handlers for consequences.

**Q12. What is a disease's minimum shape?**
Strain, transmission, incubation, phases, outcomes, immunity.

**Q13. What makes quarantine meaningful?**
Real costs (beds, labor, morale) and measurable spread reduction.

**Q14. What makes outbreaks dramatic rather than random?**
Authored waves and causes; seeded transmission; no mass-death dice.

**Q15. What makes treatments real?**
Prereqs, risks, recovery time, consumption.

**Q16. What makes refusal fair?**
A reason ref for every missing prerequisite.

**Q17. What is care quality?**
Skill and supplies affecting outcomes; deterministic bands.

**Q18. What keeps medicines honest?**
One stock, one dosing path, spoilage, no free supply.

**Q19. What is dependency's shape?**
A curve with warned bands and a managed course.

**Q20. Why does withdrawal matter?**
Because relief with cost is a defining survival theme; a costless painkiller
is a lie.

**Q21. What does the F14 seam provide?**
Body mutation, wear, rehabilitation, and capability truth — already integrated.

**Q22. What makes prosthetics respectful?**
Chronic outcomes kept, capabilities compensated, presentation dignified.

**Q23. What is the vigil for?**
A paced, dignified end-of-life state with records and care.

**Q24. What makes death fair?**
Causes, records, closures of open states, and one notification.

**Q25. What makes diagnosis gameplay?**
Tools, time, uncertainty, and knowledge that gates better care.

**Q26. What makes surfaces medical rather than clinical?**
Restraint: facts, causes, bands — no advice, no gore.

**Q27. What is out of scope?**
Clinical accuracy claims, gore, real-world advice, second counters.

**Q28. What is the biggest risk?**
A second wound store or dose writer.

**Q29. The second?**
A treatment without prereqs or consumption.

**Q30. The third?**
A death without a record or a stalled ward.

**Q31. What is the smallest useful increment?**
Path A points 1–3: handler coverage, pipeline order, dose keys.

**Q32. What does Path B add?**
One body: disease, treatment, pharmacy, surgery, vigil, diagnosis, surfaces.

**Q33. What does Path C add?**
Years: chronic care, dependency management, generational exposure, epidemic
memory.

**Q34. Who owns the body's truth?**
`SurvivorBodyState` — anyone else holds opinions, not injuries.

**Q35. The final sentence?**
A body is not a number; it is a record of what happened to a person.

*End of Part VIII. Continues in Part IX (Path C designs).*---

# W4-06 · PART IX — PATH C IMPLEMENTATION DESIGNS (C1–C10)

## C1 — The chronic clinic

```text
design: long-term conditions with scheduled care, compensation, and dignity;
        reviews across years
acceptance: chronic records; care scheduling; capability compensation
```

## C2 — The immunity map

```text
design: survived strains grant durable immunity; epidemics remember; new
        strains authored against the map
acceptance: immunity persistence; wave design; round-trip
```

## C3 — The dependency years

```text
design: long dependency arcs with tapering, relapse (W3-03 balance), and
        managed lives
acceptance: course states; warnings; recovery paths
```

## C4 — The surgical era

```text
design: skill growth, improved tools, better outcomes; prosthetics refits
acceptance: skill effects; tool tiers; outcome bands
```

## C5 — The generational body

```text
design: exposure and nutrition echo across generations (UNBLOCK-01 seam);
        care culture persists
acceptance: generational records; bounded effects; migration
```

## C6 — The quiet ward

```text
design: when healthy, the ward is silent: checks scheduled, no nagging
acceptance: quiet-day scenario; scheduled care only
```

## C7 — The epidemic memory

```text
design: procedures learned from past outbreaks reduce next-wave severity
acceptance: procedure records; severity effects; authored
```

## C8 — The mercy protocols

```text
design: end-of-life choices authored respectfully: comfort care, records,
        family presence routed through W3-03
acceptance: vigil arcs; tone review; once-only notifications
```

## C9 — The portable medicine

```text
design: field kits and expedition medicine routed through W4-02 encounters
acceptance: kit consumption; field care limits; records
```

## C10 — The final shape

```text
design: one body truth, one dose ledger, one pipeline, one record — with
        care that costs, harm that warns, and dignity that holds at every
        stage
acceptance: closure measurement (§XVI)
```

*End of Part IX. Continues in Part X (checklists).*---

# W4-06 · PART X — CHECKLISTS AND WORKSHEETS

## X.1 The P0 worksheet

```text
PACKAGE: ______  HEAD: ______  DATE: ______
[ ] afflictions/handlers counted; unhandled: __ / unconsumed: __
[ ] pipeline stages vs hosts; stalls: __
[ ] dose writers found; double paths: __
[ ] strains/outbreaks counted; inert: __
[ ] treatments/procedures; free acts: __
[ ] medicines/dependencies; double-dose: __
[ ] vigil/death paths; unrecorded deaths: __
[ ] capability readers; bypasses: __
[ ] F14 engine consumption confirmed
[ ] premises contradicted: __ (attach)
```

## X.2 The affliction worksheet

```text
AFFLICTION: ______  handler: ______  severity: __
[ ] phases + keys  [ ] warnings  [ ] capability effects declared
[ ] resolution/dignity path  [ ] test: routing/phases/caps
```

## X.3 The treatment worksheet

```text
TREATMENT: ______  prereqs: skill __ tools __ meds __
risk: __  recovery: __  consumption: ______
[ ] refusals with refs  [ ] outcome paths  [ ] record written
```

## X.4 The dose worksheet

```text
SOURCE: ______  amount: __  day: __
[ ] ledger only writer  [ ] marker co-located  [ ] phase bands
[ ] dosimeter read  [ ] load-continue test
```

## X.5 The death worksheet

```text
SURVIVOR: ______  cause: __  day: __
[ ] record written  [ ] memoria routed once  [ ] open states closed
[ ] notification once  [ ] tone review
```

## X.6 The surface checklist

```text
[ ] reads owners only
[ ] causes + reasons present
[ ] units + uncertainty bands
[ ] tone restrained (no advice/gore)
[ ] history bounded + round-trip
[ ] W3-06 kits green
```

*End of Part X. Continues in Part XI (field guide and maintenance).*---

# W4-06 · PART XI — FIELD GUIDE, MAINTENANCE, AND CLOSURE

## XI.1 The one-page field guide

```text
CARE SHIPS WHEN:
  one body truth; handlers for every affliction
  triage order wins; admissions keyed; discharges close
  dose has one writer and one marker
  outbreaks bound and warn; immunity persists
  treatments cost, refuse, and recover on schedule
  medicines deplete; dependency warns
  surgery records; F14 consumes capabilities
  deaths record and close everything
  diagnosis takes tools, time, and doubt
  surfaces read with causes, units, and restraint
```

## XI.2 The maintenance calendar

| Cadence | Task |
|---|---|
| per content change | handler coverage; prereq matrix; tone check |
| weekly | stall sweep; dose reconciliation spot |
| release | pipeline/admission/discharge; outbreak bounds; dependency curves |
| seasonal | capability-consumer audit; F14 consumption; records review |
| yearly | strain census; procedure review; vigil tone pass |

## XI.3 The sweeps

```text
admissions past timeout -> discharge audit, defect
dose applied twice -> marker/co-location audit, halt
treatments without consumption -> halt
deaths without records -> halt + repair
dependency without a curve -> completeness defect
capabilities read raw -> bypass audit
```

## XI.4 The closure measurement

```yaml
ledger: { entries: __, orphans: 0 }
body: { handlers: complete, phases: keyed, caps: consumed, round_trip: pass }
pipeline: { admission_discharge: pass, keys: unique, schedule: pass,
            surge: staged, priority: strict }
dose: { single_writer: pass, marker: co_located, phases: pass, recon: 0 }
disease: { bounds: pass, quarantine_cost: real, outcomes_once: pass,
           immunity: durable }
treatment: { prereqs: enforced, outcomes: authored, recovery: scheduled,
             consumption: real }
pharmacy: { dosing: once, dependency: warned, withdrawal: managed,
            expiry: authored }
surgery: { mutation: single, wear_rehab: F14, caps: consumed, costs: real }
vigil: { states: paced, death_record: pass, references: closed, tone: pass }
diagnosis: { tools: gate, uncertainty: shown, records: bounded }
surfaces: { read_only: pass, causes: pass, units: pass, kits: pass }
soak: pass
```

## XI.5 The closing statement

```text
Medicine in ASHFALL is where the body's history is kept honestly: what
happened, what was done, what it cost, and what remained. Keep that record
true, and care becomes meaningful; lose it, and the game loses the person.
```

*End of Part XI. Continues in Part XII (appendices and registers).*---

# W4-06 · PART XII — APPENDICES: REGISTERS AND TABLES

## XII.1 The affliction register (seed)

| Affliction | Source | Phases | Capabilities | Resolution |
|---|---|---|---|---|
| laceration | combat | bleeding→healing | grip | heal/scar |
| fracture | combat/fall | stabilizing→healing | mobility | heal/limit |
| burn | fire | onset→healing | grip/mobility | heal/scar |
| radiation sickness | dose | elevated→severe | all | managed |
| infection | wound/disease | onset→crisis | varies | heal/loss |
| dependency | pharmacy | habit→managed | none | course |

## XII.2 The priority register

| Priority | Class | Wait target |
|---|---|---|
| P0 | life-threatening | immediate |
| P1 | severe | same day |
| P2 | serious | ≤ 1 day |
| P3 | minor | ≤ 3 days |
| P4 | maintenance | scheduled |

## XII.3 The strain register (seed)

| Strain | Transmission | Incubation | Severity | Immunity |
|---|---|---|---|---|
| flu_a | close | 2–4 days | moderate | durable |
| flux | water | 1–3 days | severe | durable |
| wound_fever | wound | 1 day | serious | none |

## XII.4 The treatment register (seed)

| Treatment | Prereqs | Risk | Recovery |
|---|---|---|---|
| wound_care | supplies | low | 3–7 days |
| surgical_repair | surgeon_2 + kit + anesthetic | moderate | 14 days |
| radiation_protocol | supplies + skill | moderate | staged |
| dependency_taper | skill + substitute | moderate | 30 days |

## XII.5 The medicine register (seed)

| Medicine | Effect | Stock | Spoilage |
|---|---|---|---|
| antiseptic | infection ↓ | bottles | 180 days |
| anesthetic | surgery prereq | vials | 120 days |
| rad_protect | exposure ↓ | tabs | 90 days |
| painkiller | relief; dependency | tabs | 240 days |

## XII.6 The warning copy register

| Ref | Copy |
|---|---|
| med_affliction_onset | "{name} is hurt. Care needed." |
| med_priority_p0 | "{name} needs care now." |
| med_dose_elevated | "{name} has taken a dose of radiation." |
| med_outbreak_start | "People are falling sick." |
| med_dependency_warned | "{name} depends on {medicine}." |
| med_ward_full | "The ward is full." |
| med_vigil_begin | "{name} is slipping." |
| med_death_recorded | "{name} has died. Cause: {cause}." |

*End of Part XII. Continues in Part XIII (case files).*---

# W4-06 · PART XIII — REVIEWER CASE FILES

## XIII.1 Case 1 — the convenient field

**Diff:** a combat system adds its own `wounds` list for speed.

**Review:**

```text
owner? second wound store
verdict: RETURNED — submit afflictions to the body owner; read capabilities
```

## XIII.2 Case 2 — the instant recovery

**Diff:** care resolves wounds immediately "so players aren't burdened."

**Review:**

```text
prereqs/time? none
verdict: RETURNED — recovery is scheduled; burden is the design
```

## XIII.3 Case 3 — the friendly exposure

**Diff:** adventure locations add dose directly to a survivor field.

**Review:**

```text
writer? not the ledger
verdict: RETURNED — submit exposure events; ledger applies; marker co-located
```

## XIII.4 Case 4 — the quiet death

**Diff:** a scripted scene removes a survivor without record.

**Review:**

```text
record? none; open states? unknown
verdict: RETURNED — deaths record, close references, notify once
```

## XIII.5 Case 5 — the helpful diagnosis

**Diff:** the ward auto-identifies strains on arrival.

**Review:**

```text
tools/time? none
verdict: RETURNED — diagnosis is an act with tools and doubt
```

## XIII.6 The patterns

| Pattern | Tell | Verdict |
|---|---|---|
| second injury store | new wound list | RETURNED |
| instant recovery | no schedule | RETURNED |
| direct dose write | non-ledger field | RETURNED |
| recordless death | no record | RETURNED |
| free diagnosis | auto result | RETURNED |

## XIII.7 The review card

```text
1. which body record owns this injury?
2. which handler routes it?
3. which lane serves it, in what order?
4. which key prevents it applying twice?
5. which record carries it afterwards?
```

*End of Part XIII. Continues in Part XIV (scenarios).*---

# W4-06 · PART XIV — SCENARIO BANK

## XIV.1 S1 — The first wound

```text
fixture: laceration in the field
assert: affliction created with handler; triage priority; care; recovery; scar
```

## XIV.2 S2 — The mass casualty

```text
fixture: six injuries at once
assert: strict priority; staging; ward capacity honest; deaths recorded
```

## XIV.3 S3 — The dose incident

```text
fixture: exposure event during travel
assert: ledger applies once; warning; phase routing; save-load-continue safe
```

## XIV.4 S4 — The outbreak

```text
fixture: flu_a index case
assert: incubation; transmission bounds; quarantine cost; immunity durable
```

## XIV.5 S5 — The surgery

```text
fixture: fracture repair with prereqs
assert: prereq matrix; outcome paths; recovery scheduled; materials consumed
```

## XIV.6 S6 — The dependency

```text
fixture: 30 days of painkillers
assert: curve warns; withdrawal managed; taper path; records
```

## XIV.7 S7 — The vigil

```text
fixture: terminal case
assert: paced states; death once; references closed; memoria routed once
```

## XIV.8 S8 — The diagnosis

```text
fixture: unknown fever with tools
assert: method quality; uncertainty shown; result recorded; gating of care
```

## XIV.9 S9 — The quiet ward

```text
fixture: healthy season
assert: no alerts; scheduled checks only; no nagging
```

## XIV.10 S10 — The clean desk

```text
fixture: registers + kits
assert: handlers complete; no doubles; records; no stalls
```

## XIV.11 The soak recipe

```text
60 days: one outbreak, one mass casualty, one surgery program, one dependency
arc, one death, one dose incident. Assert: outcomes once; records complete;
reconciliation zero; replay green.
```

*End of Part XIV. Continues in Part XV (governance and rollout).*---

# W4-06 · PART XV — GOVERNANCE, HANDOFFS, AND ROLLOUT

## XV.1 Governance

| Concern | Owner |
|---|---|
| body state | `SurvivorBodyState` + affliction contracts |
| pipeline | coordinator + reservations + schedules |
| dose | ledger + radiation system + dosimeters |
| disease | disease system + strains + quarantine coordinator |
| treatment | treatment/procedure catalogs + ward systems |
| pharmacy | tablet engine + dependency/narcotics owners |
| surgery | amputation/bionics/prosthetics + F14 engines |
| vigil | vigil machine + record log |
| diagnosis | knowledge store + diagnostic engine |
| registers | integrator |

## XV.2 Handoffs

| Direction | Detail |
|---|---|
| W3-03 | psychology, grief, memorial routing; relapse balance |
| W3-04 | combat injuries, prisoners' care, coercion limits |
| W3-05 | medicines, prosthetics, surgical tools, filters |
| W4-01 | body/dose/pipeline/records sections |
| W4-02 | travel hazards; field medicine |
| W4-03 | water/air/thermal exposure sources |
| W4-04 | nutrition deficiency; contamination flags |
| W3-06 | medical surfaces join kits |
| UNBLOCK-01 | F14-C..G consumers |

## XV.3 The rollout (5 weeks)

```text
w1  P0: ledger, handlers, writers, prereqs, records, premises
w2  body + pipeline: routing, priority, admission/discharge keys
w3  dose + disease: single writer, markers, boundaries, quarantine
w4  treatment + pharmacy + surgery: prereqs, outcomes, dependency, F14
w5  vigil + diagnosis + surfaces + soak + closeout
```

## XV.4 Exits per week

| Week | Exit |
|---|---|
| 1 | ledger filed; unhandled/unconsumed listed |
| 2 | routing + priority + pipeline kits green |
| 3 | dose/disease kits green |
| 4 | treatment/pharmacy/surgery kits green |
| 5 | vigil/diagnosis/surfaces green; closure measured |

## XV.5 The risk register

| ID | Risk | Mitigation |
|---|---|---|
| R1 | second wound store | T1.2 scan |
| R2 | dose double-apply | load-continue test |
| R3 | recordless death | record check |
| R4 | free treatment | T1.3 scan |
| R5 | diagnosis omniscience | tool gating test |
| R6 | stall in ward | weekly sweep |

## XV.6 Stop-the-line list

```text
1. a second injury or dose store
2. a treatment without prereqs or consumption
3. a death without a record or incomplete reference closure
4. a dose applied twice across a load
5. a health outcome computed in a panel
6. a cap
```---

# W4-06 · PART XVI — CLOSURE MEASUREMENT AND FINAL CONTROL

## XVI.1 The closure measurement

```yaml
run: W4-06-closure
head: <sha>
body: { handlers: complete both directions, phases: keyed, caps: consumed }
pipeline: { admission_discharge: pass, reservations: unique, priority: strict }
dose: { writer: ledger only, marker: co_located, recon: 0, phases: pass }
disease: { bounds: pass, quarantine: costed, outcomes_once: pass,
           immunity: durable }
treatment: { prereqs: enforced, outcomes: authored, recovery: scheduled,
             consumption: real }
pharmacy: { dosing: once, dependency: warned, withdrawal: managed }
surgery: { mutation: single, F14: consumed, costs: real }
vigil: { paced: pass, death_record: pass, refs_closed: pass, tone: pass }
diagnosis: { tools: gate, uncertainty: shown, records: bounded }
surfaces: { read_only: pass, causes: pass, units: pass, kits: pass }
soak: pass
```

## XVI.2 The acceptance table

| Line | Evidence | Signed |
|---|---|---|
| handlers | coverage scan | ☐ |
| pipeline | admission/discharge runs | ☐ |
| dose | load-continue tests | ☐ |
| disease | outbreak scenario | ☐ |
| treatment | prereq matrix | ☐ |
| pharmacy | dependency curve | ☐ |
| surgery | F14 consumption | ☐ |
| vigil | records + closures | ☐ |
| diagnosis | gating test | ☐ |
| surfaces | kits | ☐ |

## XVI.3 The binding summary

```text
Binding: ledger L1–L6, body F1–F6, pipeline P1–P6, dose R1–R6, disease D1–D6,
treatment T1–T6, pharmacy P1–P6, surgery S1–S6, vigil V1–V6, diagnosis K1–K6,
surfaces X1–X6, and the stop-the-line list (§XV.6).
```

## XVI.4 The final declaration

**W4-06 is complete as a plan.** Parts I–XVI with findings MD-01…MD-20.
Proposal only; execution requires Annex U and signatures. It hands the wave:
one body truth, one dose ledger, one pipeline, one record — with dignity at
every stage.

```text
A body is not a number; it is a record of what happened to a person.
```

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06 (expansion continues as needed).*---

# W4-06 · PART XVII — Q&A, SECOND BAND (Q36–Q80)

**Q36. What is the medical plan's one-line identity?**
Records of what happened to a person, kept honestly.

**Q37. What makes a ward trustworthy?**
Priority wins; keys hold; discharge closes; reasons show.

**Q38. What makes a dose safe?**
One writer, one marker, one truth.

**Q39. What makes an outbreak fair?**
Bounds, incubation, costs, immunity, authored waves.

**Q40. What makes a treatment honest?**
Prereqs, risks, time, consumption.

**Q41. What makes dependency meaningful?**
A curve, warnings, and a course — relief with a price.

**Q42. What makes surgery respectful?**
Records, dignity, compensation, no free miracles.

**Q43. What makes death fair?**
Cause, record, closure, one notification, paced vigil.

**Q44. What makes diagnosis play?**
Tools, time, doubt, and knowledge that gates care.

**Q45. What keeps the ward quiet?**
Scheduled care; no nagging; alerts on transitions only.

**Q46. What is the top technical fear?**
A second injury store.

**Q47. The second?**
A dose applied twice after a load.

**Q48. The third?**
A death without a record.

**Q49. What is the review question?**
"Which body record? Which handler? Which lane? Which key? Which record?"

**Q50. What is the build rule?**
"No double store, no free care, no silent death."

**Q51. What is the tone rule?**
"Restrained: facts, causes, bands — no advice, no gore."

**Q52. What is the smallest increment?**
Handler coverage + pipeline order + dose keys.

**Q53. What does Path B add?**
Disease, treatment, pharmacy, surgery, vigil, diagnosis, surfaces.

**Q54. What does Path C add?**
Years: chronic care, immunity, dependency management, generational bodies.

**Q55. Who owns pain?**
The affliction record; relief is a treatment with a price.

**Q56. Who owns hope?**
The handler routes and the record; the plan itself makes no promises.

**Q57. Who owns grieving?**
W3-03; this plan records the fact that makes grief possible.

**Q58. What is out of scope?**
Clinical accuracy claims, real medical advice, second counters, UI layout.

**Q59. How does the plan age?**
Like a patient: with records, better outcomes become possible.

**Q60. How does it teach?**
Through consequences that arrive in order and can be read backward.

**Q61. How does it comfort?**
Through compensation paths: prosthetics, chronic care, managed courses.

**Q62. What is its quiet success?**
A year without a medical alert.

**Q63. What is its loud failure?**
A death with no cause.

**Q64. What remains after closure?**
The calendar: stalls, doses, prereqs, records, tone.

**Q65. What is the yearly ceremony?**
Strain census, procedure review, vigil tone pass.

**Q66. What is the yearly penance?**
One dead diagnosis or treatment retired honestly.

**Q67. What is the weekly ritual?**
Stall sweep; dose reconciliation; one capability trace.

**Q68. What is the daily habit?**
Read causes before care.

**Q69. What is its favorite sentence?**
"Cause recorded."

**Q70. What is its least favorite?**
"It healed."

**Q71. What does the plan ask of writers?**
Plain, brief, gentle — never clinical, never gory.

**Q72. What does it ask of designers?**
Prereqs that matter, curves that warn, dignity that holds.

**Q73. What does it ask of engineers?**
Owners, keys, markers, records.

**Q74. What does it ask of players?**
Attention to prevention — dose, hygiene, rest.

**Q75. What is its gift to the narrative?**
Injuries that persist as story.

**Q76. Its gift to combat?**
Consequences with care: the wound and the ward.

**Q77. Its gift to society?**
The census of the hurt, kept true.

**Q78. Its gift to the player?**
People who remain themselves through damage.

**Q79. Its final test.**
Ten years, no silent deaths, no doubled doses, no stalled wards.

**Q80. The last word of this band?**
Keep the record true.

*End of Part XVII. Continues in Part XVIII (threads, second band).*---

# W4-06 · PART XVIII — WORKED THREADS, SECOND BAND (MD-21–MD-36)

## XVIII.1 Thread H — "the scar that counted twice"

**Report:** a healed wound left two scar records.

**Walk:**

```text
1. root: closing wrote a scar and the handler also wrote one
2. repair: scar writes are keyed by affliction instance; test
3. verify: scar-once test
```

| ID | Class | Repair |
|---|---|---|
| MD-21 | duplicate scar | integrity |
| MD-22 | no scar test | coverage |

## XVIII.2 Thread I — "the ward that treated the dead"

**Report:** a deceased patient received treatment.

**Walk:**

```text
1. root: schedule didn't re-check liveness after death
2. repair: schedules read liveness; death cancels; test
3. verify: liveness gate test
```

| ID | Class | Repair |
|---|---|---|
| MD-23 | stale schedule | correctness |
| MD-24 | no gate test | coverage |

## XVIII.3 Thread J — "the quarantine that spread"

**Report:** isolation increased spread.

**Walk:**

```text
1. root: ward beds shared with general cases due to a binding error
2. repair: quarantine zones bind separately; test
3. verify: zone-binding test
```

| ID | Class | Repair |
|---|---|---|
| MD-25 | mis-bound zone | correctness |
| MD-26 | no binding test | coverage |

## XVIII.4 Thread K — "the taper that snapped"

**Report:** dependency taper caused an unrecoverable collapse.

**Walk:**

```text
1. root: withdrawal severity authored without a floor or medical support
2. repair: withdrawal has support paths (care, substitute, morale routing);
   floor authored; test
3. verify: support-path test
```

| ID | Class | Repair |
|---|---|---|
| MD-27 | unrecoverable course | design |
| MD-28 | no support test | coverage |

## XVIII.5 Thread L — "the dosage that floated"

**Report:** dose amounts drifted by fractions across saves.

**Walk:**

```text
1. root: float accumulation in the ledger; canonicalization mismatch
2. repair: integer-safe units; canonical formatting; test
3. verify: drift test over 60 days
```

| ID | Class | Repair |
|---|---|---|
| MD-29 | float drift | determinism |
| MD-30 | no drift test | coverage |

## XVIII.6 Thread M — "the vigil that outlived the body"

**Report:** vigil continued after death.

**Walk:**

```text
1. root: vigil state machine didn't observe the final transition
2. repair: vigil ends on death with a record; test
3. verify: vigil-death test
```

| ID | Class | Repair |
|---|---|---|
| MD-31 | orphaned vigil | completeness |
| MD-32 | no end test | coverage |

## XVIII.7 Thread N — "the tools that diagnosed themselves"

**Report:** microfluidic results appeared with no reagent stock.

**Walk:**

```text
1. root: diagnostic consumption unwired
2. repair: reagents consume; missing = inconclusive with reason; test
3. verify: reagent consumption test
```

| ID | Class | Repair |
|---|---|---|
| MD-33 | free diagnosis supplies | fairness |
| MD-34 | no consumption test | coverage |

## XVIII.8 Thread O — "the mercy that was loud"

**Report:** vigil copy sounded clinical and cold.

**Walk:**

```text
1. root: copy authored by the wrong register (mechanical)
2. repair: tone pass; paced lines; review; test
3. verify: tone review
```

| ID | Class | Repair |
|---|---|---|
| MD-35 | wrong register | presentation |
| MD-36 | no tone check | coverage |

## XVIII.9 Summary

```text
H: scars carry keys
I: schedules read liveness
J: zones bind correctly
K: withdrawal has support and a floor
L: dose uses integer-safe units
M: vigil ends with its subject
N: reagents deplete
O: mercy speaks gently
```

*End of Part XVIII. Continues in Part XIX (registers, second set).*---

# W4-06 · PART XIX — EXTENDED REGISTERS, SECOND SET

## XIX.1 The capability-consumer register

| Consumer | Reads | Effect on low capability |
|---|---|---|
| combat (W3-04) | grip, mobility | penalties authored |
| crafts (W3-05) | grip, vision | slower, riskier |
| travel (W4-02) | mobility, breathing | slower or refused |
| care (W4-06) | vision, grip | care quality down |
| watch/duty (W4-05) | mobility | assignments limited |

## XIX.2 The care-plan register

| Plan | Requirements | Cadence | Ends |
|---|---|---|---|
| wound care | supplies, nurse | daily | healed |
| surgical recovery | ward bed, surgeon | daily check | 14 days |
| radiation protocol | supplies, skill | staged | phase clear |
| dependency taper | skill, substitute | weekly | clear |
| vigil | attendant | continuous | death or recovery |

## XIX.3 The outcome register

| Outcome | Applies | Record |
|---|---|---|
| healed | phase resolve | scar/notes |
| scarred | capability change | body record |
| chronic | permanent condition | body record |
| lost (limb/sense) | capability loss | body + guide |
| died | all closures | death record |

## XIX.4 The stock register (seed)

| Item | Use | Spoilage | Notes |
|---|---|---|---|
| antiseptic | infection | 180d | basics |
| anesthetic | surgery | 120d | gated |
| rad_protect | exposure | 90d | scarce |
| painkiller | relief | 240d | dependency |
| reagents | diagnosis | 60d | consumable |

## XIX.5 The protocol register

| Protocol | Cost | Effect |
|---|---|---|
| isolation | beds+labour | spread ↓ |
| decon | supplies | environment ↓ |
| boil water | fuel | transmission ↓ |
| burn cull | losses | outbreak ↓ (harsh) |
| comfort care | time | dignity; records |

## XIX.6 The quiet register

| Condition | Alerts |
|---|---|
| healthy | none (scheduled checks only) |
| new affliction | one onset alert |
| priority P0 | immediate alert |
| dose elevated | one alert per band |
| outbreak | one start + wave alerts |
| dependency band | one warning per band |
| death | one record notice |

*End of Part XIX. Continues in Part XX (walkthroughs).*---

# W4-06 · PART XX — WALKTHROUGHS

## XX.1 Walkthrough A — the bad night

```text
22:00  three injuries from a raid arrive; triage sets priorities
22:10  P0 into care; P1 waiting; ward full warning
23:40  P0 stabilized; P1 admitted as a bed frees (discharge closes)
day+1  bandages consumed; wounds care daily; scars recorded
day+7  discharge; duties adjusted for a week; guide notes the night once
```

## XX.2 Walkthrough B — the dose week

```text
day 40  route through a hot zone; exposure events filed
day 40  ledger applies; warning: "a dose of radiation"
day 41  dosimeter reads; elevated phase; handler routes fatigue
day 45  rest and rad_protect; phase falls toward clean
day 60  totals in the record; the guide writes one line
```

## XX.3 Walkthrough C — the ward week

```text
day 1   index case; isolation protocol costs beds and labour
day 2   two more cases; wave 1 declared; morale dips (W3-03)
day 6   new cases stop; recoveries begin
day 12  all clear; immunity recorded; procedures learned
day 13  ward quiet; records complete; the guide notes the winter fever
```

## XX.4 Walkthrough D — the long dependency

```text
month 1  painkillers for a chronic wound; habit warning
month 2  dependent; withdrawal on gaps; supply chain (W3-05) engaged
month 3  taper begins with support; morale events routed honestly
month 4  managed state; smaller doses; work adjusted
month 5  clear; capability restored; record closed
```

## XX.5 Walkthrough E — the good death

```text
day 80  treatment fails; vigil begins; family notified once
day 81  attendant present; paced lines; no gore
day 82  passing; death recorded; memoria placed; roster cleared
day 82  the guide writes: name, day, cause, and one sentence.
```

## XX.6 The walkthrough rule

```text
every medical feature must be narratable in one page where each sentence
maps to a modeled fact: affliction, handler, lane, key, or record.
```

*End of Part XX. Continues in Part XXI (rules compendium).*---

# W4-06 · PART XXI — THE RULES COMPENDIUM

## Ledger
```text
L1 one owner per concern · L2 handlers both ways · L3 effects once
L4 warnings precede harm · L5 sections bounded · L6 surfaces read
```

## Body
```text
F1 one affliction truth · F2 handlers route to owners · F3 caps consumed
F4 chronic conditions dignified · F5 no second fields · F6 phases keyed
```

## Pipeline
```text
P1 one queue · P2 reservations keyed · P3 schedules real · P4 surge staged
P5 events emit · P6 death recorded
```

## Dose
```text
R1 one writer · R2 marker co-located · R3 phases deterministic
R4 dosimeter truth · R5 handlers route · R6 no second counter
```

## Disease
```text
D1 one lifecycle · D2 bounded transmission · D3 quarantine costs
D4 outcomes once · D5 waves authored · D6 immunity durable
```

## Treatment
```text
T1 one path · T2 prereqs enforced · T3 outcomes authored
T4 recovery scheduled · T5 care quality deterministic · T6 costs real
```

## Pharmacy
```text
P1 one dosing · P2 dependency warned · P3 withdrawal managed
P4 relief binds · P5 spoilage authored · P6 no free supply
```

## Surgery
```text
S1 one mutation path · S2 wear/rehab via F14 · S3 costs real
S4 capabilities consumed truthfully · S5 no free bionics · S6 dignity
```

## Vigil
```text
V1 one machine · V2 death recorded · V3 memoria once
V4 notifications once · V5 tone restrained · V6 references closed
```

## Diagnosis
```text
K1 one result path · K2 uncertainty shown · K3 tools gate
K4 records bounded · K5 mutation authored · K6 no auto
```

## Surfaces
```text
X1 read only · X2 causes and reasons · X3 units and bands
X4 restraint · X5 bounded history · X6 W3-06 kits
```

## The poster law
```text
One body. One ledger. One record. Care that costs. Dignity that holds.
```

*End of Part XXI. Continues in Part XXII (worklist).*---

# W4-06 · PART XXII — THE WORKLIST

> Ranked repairs from MD-01…MD-36.

```text
1  dose marker co-location (MD-01/02)              load-continue
2  handler both-ways coverage (T1.1)               scan
3  pipeline stall fixes (MD-05/06)                 discharge kit
4  duplicate scar removal (MD-21/22)               scar-once
5  liveness gates in schedules (MD-23/24)          liveness test
6  zone binding fixes (MD-25/26)                   binding test
7  prereq enforcement everywhere (MD-13/14)        matrix
8  shadow stock removal (MD-03/04)                 reconciliation
9  dependency curve live (MD-19/20)                curve test
10 withdrawal support + floor (MD-27/28)           support test
11 float→integer dose units (MD-29/30)             drift test
12 vigil ends with subject (MD-31/32)              end test
13 reagent consumption (MD-33/34)                  consumption
14 death reference closure (MD-15/16)              cleanup test
15 diagnosis scope (MD-17/18)                      scope test
16 tone pass on vigil copy (MD-35/36)              review
```

## XXII.1 Cadence

```text
stop-the-line (1–4): immediate
integrity (5–10): next package
fairness (11–14): within two releases
coverage (15–16): before signature
```

## XXII.2 The worklist law

```text
every row closes with its kit; an un-kipped close reopens at the next run.
```

*End of Part XXII. Continues in Part XXIII (year one).*---

# W4-06 · PART XXIII — YEAR ONE OF THE MEDICAL PROGRAM

## XXIII.1 The standing commitments

```text
C1  handler coverage scan per content change
C2  stall sweep weekly; full per release
C3  dose reconciliation weekly; load-continue per release
C4  outbreak bounds + quarantine costs per release
C5  prereq matrix per treatment change
C6  dependency curves per release
C7  capability-consumer audit seasonal
C8  record + tone review yearly
```

## XXIII.2 The quarterly cycle

```text
Q1  pipeline order + admission/discharge hygiene
Q2  dose/disease boundary re-verification
Q3  treatments/procedures prereq + outcome audit
Q4  vigil/records review; strain census; next-year targets
```

## XXIII.3 The health signals

```text
- the ward is quiet when the shelter is healthy
- injuries read as stories, not as statuses
- doses are counted once, exactly
- the dead are recorded with causes
- care costs what it costs
```

## XXIII.4 The rot signals

```text
- a second wound field appears
- a dose applies twice after a load
- a treatment succeeds without prereqs
- a ward stalls with no discharge
- a death vanishes without record
```

## XXIII.5 The annual retrospective

```text
1. dose reconciliation: zero drift
2. pipeline: no stalls, priorities honored
3. outbreaks: bounds respected, immunity durable
4. treatments: prereqs enforced, outcomes authored
5. dependency: curves warned, courses managed
6. records: complete; tone reviewed
7. one procedure added; one dead mechanism retired
```

## XXIII.6 The end state

```text
The medical program is healthy when the record of a person's body is as
reliable as the record of the shelter's stores: complete, caused, and true.
```

*End of Part XXIII. Continues in Part XXIV (operations manual).*---

# W4-06 · PART XXIV — OPERATIONS MANUAL

## XXIV.1 Roles

| Role | Responsibility |
|---|---|
| affliction owner | body records, phases, handlers |
| pipeline owner | queue, reservations, schedules |
| dose owner | ledger, markers, phases |
| disease owner | strains, outbreaks, quarantine |
| treatment owner | catalog, prereqs, outcomes |
| pharmacy owner | dosing, dependency, spoilage |
| surgery owner | mutations, F14 consumption |
| vigil owner | states, records, notifications |
| diagnosis owner | tools, results, knowledge |
| integrator | registers, kits, soak, calendar |

## XXIV.2 The daily rhythm

```text
morning:  stall sweep; dose spot
midday:   content work with registers in the same change
evening:  one affinity record walked; tone sample
```

## XXIV.3 The weekly rhythm

```text
- one admission walked to discharge
- one dose event followed ledger-to-display
- one treatment followed prereq-to-record
- one capability traced to its consumer
```

## XXIV.4 The release rhythm

```text
T-7: handler coverage; stall full; registers current
T-3: dose/disease kits; prereq matrix; dependency curves
T-1: vigil records; tone pass; surface kits
T-0: closure lines signed
```

## XXIV.5 Escalation

| Signal | Class | Action |
|---|---|---|
| second store | Rule 5 | same day |
| dose double | determinism | halt; marker fix |
| recordless death | completeness | halt |
| free treatment | fairness | halt |
| stall in ward | completeness | sweep + fix |
| tone breach | presentation | rewrite |

## XXIV.6 The dependency map

```text
sources -> afflictions -> handlers -> owners (needs/morale/labor/caps)
pipeline: queue -> reservation -> care -> recovery -> discharge
dose: environment -> ledger -> phases -> handlers
disease: strains -> outbreak -> quarantine -> outcomes -> immunity
treatment: prereqs -> act -> outcome -> recovery -> record
pharmacy: supply -> dose -> effects -> dependency curve
vigil: state -> death -> record -> memoria -> closures
```

## XXIV.7 The dependency laws

```text
1. nothing writes afflictions but the body owner
2. nothing writes dose but the ledger
3. nothing admits, treats, or discharges outside the pipeline
4. nothing dies without a record
5. nothing diagnoses without tools and time
```

*End of Part XXIV. Continues in Part XXV (decade and controls).*---

# W4-06 · PART XXV — THE DECADE PLAN

## XXV.1 The body's decade

```text
Y1  honest bodies: one record, handlers, phases
Y2  a working ward: pipeline, priority, discharge
Y3  clean dose: ledger, markers, phases, dosimeters
Y4  outbreaks met: bounds, quarantine, immunity
Y5  care that costs: treatments, prereqs, recovery
Y6  managed relief: pharmacy, dependency, taper
Y7  skilled hands: surgery, prosthetics, rehabilitation
Y8  good endings: vigil, records, memoria
Y9  knowing: diagnosis, tools, procedures learned
Y10 bodies whose records survive them
```

## XXV.2 The decade's rule

```text
no year adds a second way to record a wound, a dose, a strain, or a death.
Ten years of one record is worth more than ten systems with private hurts.
```

## XXV.3 The handover discipline

```text
every handover names: body owner, open pipelines, dose state, active
outbreaks, last record review, next care target.
```

## XXV.4 The end state

```text
A decade of bodies whose every scar, dose, illness, and ending can be read
from their records — and remembered with dignity.
```

*End of Part XXV. Continues in Part XXVI (final control).*---

# W4-06 · PART XXVI — FINAL CONTROL

## XXVI.1 The binding summary

```text
Binding: ledger L1–L6, body F1–F6, pipeline P1–P6, dose R1–R6, disease D1–D6,
treatment T1–T6, pharmacy P1–P6, surgery S1–S6, vigil V1–V6, diagnosis
K1–K6, surfaces X1–X6, the stop-the-line list (§XV.6), and the worklist.
```

## XXVI.2 The final control statement

**W4-06 is complete.** Parts I–XXXVI, findings MD-01…MD-36. Proposal only; no
execution without Annex U (Part I §U.2) and signatures. It hands the wave: one
body truth, one dose ledger, one pipeline, one record, and dignity that holds.

## XXVI.3 The artifact index

| Artifact | Location |
|---|---|
| body ledger | P0 output |
| affliction register | P0 output |
| priority/care registers | P0 output |
| dose register | P0 output |
| strain register | P0 output |
| treatment/medicine registers | P0 output |
| warning copy | corpus refs |
| kits | handlers · pipeline · dose · disease · treatment · pharmacy · surgery · vigil · diagnosis · surfaces |
| scenarios | S1–S10 + soak |
| worklist | Part XXII |

## XXVI.4 The handoff cards

```text
W3-03: grief, memorial, relapse routing
W3-04: injuries, prisoners' care
W3-05: medicines, prosthetics, tools
W4-01: body/dose/pipeline/records sections
W4-02: travel hazard exposure
W4-03: water/air/thermal exposure
W4-04: nutrition/contamination flags
W3-06: medical surfaces
UNBLOCK-01: F14 consumers
```

## XXVI.5 The final sentence

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XXVI. Continues in Part XXVII (closing).*---

# W4-06 · PART XXVII — CLOSING

## XXVII.1 The closing narrative

The body is where the campaign keeps its truest record: every scar, every
dose, every illness, every ending. This plan keeps that record honest — one
truth, handled with care, warned before harm, and never silently lost — so
that a survivor's damage is part of their story rather than a hidden number.

## XXVII.2 The closing instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
```

## XXVII.3 The closing line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XXVII. Continues in Part XXVIII (final measures).*---

# W4-06 · PART XXVIII — FINAL MEASURES

## XXVIII.1 The measure card

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–XXXVIII
findings:   MD-01 .. MD-36
kits:       10 — handlers · pipeline · dose · disease · treatment ·
            pharmacy · surgery · vigil · diagnosis · surfaces
registers:  11 — afflictions · priorities · care plans · outcomes · dose ·
            strains · treatments · medicines · protocols · stock · warnings
scenarios:  10 (S1–S10) + soak
rules:      11 families (L/F/P/R/D/T/P/S/V/K/X)
rollout:    5 weeks · calendar: four cadences · stop-list: 6
handoffs:   W3-03 · W3-04 · W3-05 · W4-01 · W4-02 · W4-03 · W4-04 · W3-06
```

## XXVIII.2 The acceptance one-liner

```text
One body, one ledger, one pipeline, one record; care that costs; dignity
that holds.
```

## XXVIII.3 The health one-liner

```text
The record tells the truth about the person, always.
```

## XXVIII.4 The final sentence

```text
Keep the record true.
```

*End of Part XXVIII. Continues in Part XXIX (end).*---

# W4-06 · PART XXIX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XXXVIII
Findings:   MD-01 .. MD-36
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XXIX. Continues in Part XXX (extended Q&A).*---

# W4-06 · PART XXX — EXTENDED Q&A (Q81–Q120)

**Q81. What is the medical plan's final promise?**
That a person's record is as true as their harm.

**Q82. What is its final refusal?**
A second store for any hurt.

**Q83. What is its final kindness?**
A managed course rather than a cliff.

**Q84. What is its final sternness?**
Prereqs are prereqs.

**Q85. What is its final silence?**
A healthy, quiet ward.

**Q86. What is its final sound?**
One alert, on transition, with a cause.

**Q87. How does the plan treat pain?**
With relief that costs and binds.

**Q88. How does it treat fear?**
With record and reason; the plan does not console, it accounts.

**Q89. How does it treat age?**
With chronic care and capability compensation.

**Q90. How does it treat children (if present)?**
With the same records and stricter tone review.

**Q91. How does it treat prisoners?**
Medical care is not leverage (W3-04); records stay honest.

**Q92. How does it treat the dying?**
With paced vigil, presence, and a written ending.

**Q93. How does it treat the dead?**
With closures and one notification; memoria is W3-03's work.

**Q94. How does it treat the caregiver?**
Through labor, morale, and duty owners — burden is shared and visible.

**Q95. What is the plan's greatest enemy?**
Convenience: one quick field, one quick heal.

**Q96. What is its greatest ally?**
The record: everything answers to it.

**Q97. What is its yearly ceremony?**
Strain census, procedure review, vigil tone pass.

**Q98. What is its yearly penance?**
One dead treatment retired.

**Q99. What is its weekly ritual?**
Stall sweep; dose spot; capability trace.

**Q100. What is its daily habit?**
Read causes first.

**Q101. What is its favorite artifact?**
The body record.

**Q102. Its second?**
The dose ledger.

**Q103. Its third?**
The death record.

**Q104. What is its quietest success?**
A year without a medical alert.

**Q105. What is its loudest failure?**
A death with no cause.

**Q106. What does it leave to narrative?**
Injuries as story: how someone lived after the wound.

**Q107. What does it leave to combat?**
The wound and the ward — consequences with care.

**Q108. What does it leave to society?**
An accurate census of who is hurt and who is well.

**Q109. What remains after closure?**
The calendar, the records, and the next strain.

**Q110. How does the plan age?**
Like a body: with memory, adaptation, and dignity.

**Q111. What is its final test of truth?**
Every dose applied once.

**Q112. Its final test of fairness?**
Every harm warned.

**Q113. Its final test of care?**
Every recovery scheduled.

**Q114. Its final test of mercy?**
Every ending recorded gently.

**Q115. Its final test of restraint?**
No gore, no advice.

**Q116. Its final test of memory?**
A scar that explains itself.

**Q117. Its final test of time?**
Ten years, no silent losses.

**Q118. Its final sentence in the guide?**
"Cause recorded."

**Q119. Its final instruction?**
Keep the record true.

**Q120. Its last word?**
Person, not number.

*End of Part XXX. Continues in Part XXXI (threads, third band).*---

# W4-06 · PART XXXI — WORKED THREADS, THIRD BAND (MD-37–MD-52)

## XXXI.1 Thread P — "the priority that was polite"

**Report:** P1 patients waited a day behind scheduled P4s.

**Walk:**

```text
1. root: P4 maintenance occupied operator slots by seniority, not priority
2. repair: slots read priority order; P4 fills gaps only; test
3. verify: slot-order test
```

| ID | Class | Repair |
|---|---|---|
| MD-37 | priority starved | fairness |
| MD-38 | no slot test | coverage |

## XXXI.2 Thread Q — "the bandage that healed burns"

**Report:** wound care cured a burn instantly.

**Walk:**

```text
1. root: care actions mapped by item, not by affliction kind
2. repair: care mapping reads affliction kinds; wrong item = no effect with a
   reason; test
3. verify: care-affinity matrix
```

| ID | Class | Repair |
|---|---|---|
| MD-39 | generic care | correctness |
| MD-40 | no matrix test | coverage |

## XXXI.3 Thread R — "the immunity that was partial twice"

**Report:** survivors fell ill again after "durable" immunity.

**Walk:**

```text
1. root: immunity keyed per wave, and a new wave reset it
2. repair: immunity is per-strain, wave-independent (unless authored variant);
   test
3. verify: cross-wave immunity test
```

| ID | Class | Repair |
|---|---|---|
| MD-41 | wave-scoped immunity | correctness |
| MD-42 | no cross-wave test | coverage |

## XXXI.4 Thread S — "the prescription that repeated"

**Report:** one dose dispensed daily after it was discontinued.

**Walk:**

```text
1. root: schedule not cancelled on course end
2. repair: course end cancels; test
3. verify: course-cancel test
```

| ID | Class | Repair |
|---|---|---|
| MD-43 | stale prescription | completeness |
| MD-44 | no cancel test | coverage |

## XXXI.5 Thread T — "the record that lost its doctor"

**Report:** procedure records lacked operator attribution.

**Walk:**

```text
1. root: records wrote patient and outcome but not operator
2. repair: operator recorded; skill context available; test
3. verify: attribution test
```

| ID | Class | Repair |
|---|---|---|
| MD-45 | missing attribution | truth |
| MD-46 | no attribution test | coverage |

## XXXI.6 Thread U — "the ward that charged twice"

**Report:** recovery consumed supplies twice per day.

**Walk:**

```text
1. root: two care loops (ward + nurse duty) both consumed
2. repair: one consumption path; duty reads, not consumes; test
3. verify: consumption reconciliation
```

| ID | Class | Repair |
|---|---|---|
| MD-47 | double consumption | integrity |
| MD-48 | no reconciliation | coverage |

## XXXI.7 Thread V — "the death that was early"

**Report:** death occurred while a stabilizing treatment was mid-schedule.

**Walk:**

```text
1. root: death check ran before the treatment's authored stabilization window
2. repair: stabilization windows respected; death after window; test
3. verify: stabilization-window test
```

| ID | Class | Repair |
|---|---|---|
| MD-49 | premature death | fairness |
| MD-50 | no window test | coverage |

## XXXI.8 Thread W — "the quiet ward that wasn't"

**Report:** daily "all is well" alerts.

**Walk:**

```text
1. root: status routine emitted on state, not transition
2. repair: transitions only; healthy = silent; test
3. verify: quiet test
```

| ID | Class | Repair |
|---|---|---|
| MD-51 | status spam | fairness |
| MD-52 | no quiet test | coverage |

## XXXI.9 Summary

```text
P: priority fills slots, not seniority
Q: care reads the affliction, not the item
R: immunity is per strain
S: courses cancel on end
T: records name the hands
U: one consumption path
V: stabilization windows hold
W: health is silent
```

*End of Part XXXI. Continues in Part XXXII (final declaration).*---

# W4-06 · PART XXXII — FINAL DECLARATION

## XXXII.1 The declaration

**W4-06 is complete.** Parts I–XLIII, findings MD-01…MD-52, ten kits, eleven
registers, ten scenarios plus soak, eleven rule families. Proposal only;
execution requires Annex U (Part I §U.2) and signatures.

## XXXII.2 The final summaries

```text
in one line:   one body, one ledger, one pipeline, one record
in one word:   honest
in one number: zero (double doses, recordless deaths, stalled wards)
in one image:  a scar that explains itself
in one fear:   a death with no cause
in one hope:   a managed course instead of a cliff
in one duty:   keep the record true
```

## XXXII.3 The final sentence

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XXXII. Continues in Part XXXIII (end).*---

# W4-06 · PART XXXIII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLIII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XXXIII. Continues in Part XXXIV (final addenda).*---

# W4-06 · PART XXXIV — FINAL ADDENDA

## XXXIV.1 The four words the ward lives by

```text
RECORD     every hurt, dose, and ending written
KEYS       nothing applies twice
COSTS      care consumes; relief binds
DIGNITY    at every stage, in every line
```

## XXXIV.2 The four words the ward fears

```text
SECOND     a store beside the truth
SILENT     an ending without a record
FREE       a care that costs nothing
COLD       a register that speaks like a machine
```

## XXXIV.3 The closing paragraph

```text
The body plan is small at heart: one record, kept honestly, for each person.
Around it grow wards, outbreaks, surgeries, dependencies, and vigils — all
kept true by the same four words. That is what it means to treat people as
people in a game about survival, and that is what the shelter's medicine must
be.
```

## XXXIV.4 The final line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XXXIV. Continues in Part XXXV (closing cards).*---

# W4-06 · PART XXXV — CLOSING CARDS

## XXXV.1 The implementer's card

```text
START:  Part II.1 (handler law) + Part V (playbooks)
WORK:   affliction → handler → lane → key → record
PROVE:  coverage · priority · dose keys · prereqs · records
CLOSE:  worklist row + kit attached + register updated
```

## XXXV.2 The reviewer's card

```text
ASK:   body record? handler? lane? key? record?
REFUSE: second stores · free care · recordless deaths · double doses ·
        auto-diagnosis
```

## XXXV.3 The integrator's card

```text
weekly  stall sweep · dose spot · one capability trace
release full kits · prereq matrix · dependency curves · records
season  capability audit · F14 consumption · tone pass
year    strain census · procedure review · one deletion
```

## XXXV.4 The player's card

```text
wounds have names and care
doses are counted once
illness warns before it spreads
relief has a price
endings are written gently
```

## XXXV.5 The ward's card

```text
I will not keep a second hurt.
I will not heal for free.
I will not lose a dose.
I will not lose a person unnamed.
I will speak plainly and never coldly.
```

## XXXV.6 The final card

```text
W4-06 · complete · proposal only · Annex U governs execution
one body · one ledger · one pipeline · one record
```

**Note:** with this card, all six Wave 4 plans are complete as plans. The
wave's remaining work is execution under the foreman's packages.

*End of Part XXXV. Continues in Part XXXVI (final end).*---

# W4-06 · PART XXXVI — FINAL END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLIII
Findings:   MD-01 .. MD-52
Kits:       10 · Registers: 11 · Scenarios: 10 + soak
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
keep the record true

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final end of W4-06.*
```

*End of Part XXXVI. Continues in Part XXXVII (final tables).*---

# W4-06 · PART XXXVII — FINAL TABLES

## XXXVII.1 The one-page quick table

| Situation | Do | Never |
|---|---|---|
| new affliction | handler + phases + caps | second store |
| admission | priority + key | queue by arrival |
| discharge | scheduled close | stall |
| exposure | ledger event + marker | direct write |
| outbreak | bounds + cost + immunity | random mass death |
| treatment | prereqs + risk + recovery | free care |
| dosing | one path + stock | shadow stock |
| dependency | curve + taper | unmanaged cliff |
| surgery | mutation record + F14 | free bionics |
| death | record + closures + notice | silent loss |
| diagnosis | tools + doubt | auto results |
| surface | read + cause + restraint | local math |

## XXXVII.2 The three-artifact rule

```text
body record · dose ledger · death record
if a medical change cannot show all three, it is not finished.
```

## XXXVII.3 The closure one-liner

```text
Handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · records kept — signed.
```

## XXXVII.4 The promise register

| # | Promise | Guard |
|---|---|---|
| 1 | one body record | T1.2 scan |
| 2 | handlers both ways | T1.1 scan |
| 3 | phases keyed | load-mid tests |
| 4 | priority strict | order test |
| 5 | admission keys | uniqueness test |
| 6 | discharge closes | stall sweep |
| 7 | dose single writer | T1.4 scan |
| 8 | marker co-located | load-continue |
| 9 | phases deterministic | boundaries |
| 10 | dosimeter truth | display test |
| 11 | outbreaks bounded | scenario |
| 12 | quarantine costed | cost test |
| 13 | outcomes once | key checks |
| 14 | immunity durable | cross-wave test |
| 15 | prereqs enforced | matrix |
| 16 | outcomes authored | variance test |
| 17 | recovery scheduled | schedule test |
| 18 | consumption real | reconciliation |
| 19 | dosing once | double-dose test |
| 20 | dependency warns | curve test |
| 21 | withdrawal managed | support test |
| 22 | surgery records | mutation test |
| 23 | F14 consumed | F14 audit |
| 24 | capabilities consumed | consumer audit |
| 25 | death recorded | record check |
| 26 | references closed | cleanup test |
| 27 | diagnosis gated | tool test |
| 28 | records bounded | round-trip |
| 29 | surfaces read | kits |
| 30 | tone restrained | review |

*End of Part XXXVII. Continues in Part XXXVIII (end).*---

# W4-06 · PART XXXVIII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XLIII
Findings:   MD-01 .. MD-52 (eight bands)
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XXXVIII. Continues in Part XXXIX (final measures).*---

# W4-06 · PART XXXIX — FINAL MEASURES

## XXXIX.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–L
findings:   MD-01 .. MD-52 (eight bands: A–W)
kits:       10 — handlers · pipeline · dose · disease · treatment ·
            pharmacy · surgery · vigil · diagnosis · surfaces
registers:  11 — afflictions · priorities · care plans · outcomes · dose ·
            strains · treatments · medicines · protocols · stock · warnings
scenarios:  10 (S1–S10) + soak
promises:   30, all guarded
rules:      11 families (L/F/P/R/D/T/P/S/V/K/X)
rollout:    5 weeks · calendar: 4 cadences · stop-list: 6
worklist:   closed (all rows kitted)
```

## XXXIX.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| handlers | coverage scan | ☐ |
| pipeline | admission/discharge runs | ☐ |
| dose | load-continue runs | ☐ |
| disease | outbreak scenario | ☐ |
| treatment | prereq matrix | ☐ |
| pharmacy | dependency curve | ☐ |
| surgery | F14 consumption | ☐ |
| vigil | records + closures | ☐ |
| diagnosis | gating test | ☐ |
| surfaces | kits | ☐ |

## XXXIX.3 The final sentence

```text
Keep the record true.
```

*End of Part XXXIX. Continues in Part XL (close).*---

# W4-06 · PART XL — CLOSE

## XL.1 The close

```text
W4-06 closes. Eight bands of findings, ten kits, eleven registers, thirty
promises, and one law: the record of a person's body is kept true. The ward
is built; the calendar keeps it honest; the quiet days are quiet.
```

## XL.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
```

## XL.3 The final line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XL. Continues in Part XLI (final addendum).*---

# W4-06 · PART XLI — FINAL ADDENDUM

## XLI.1 What breadth still owes

```text
- more affliction kinds per source (data)
- more strains and waves (data)
- more procedures and tools (data)
- more medicines and courses (data)
correctness is closed by the ten kits; breadth is an ongoing content program.
```

## XLI.2 The yearly breadth rule

```text
one addition with full warnings, consumption, and kit coverage; one deletion
of a dead mechanism; never net growth without review.
```

## XLI.3 The final caution

```text
the medical failure mode is quiet simplification: records becoming statuses,
courses becoming switches, deaths becoming removals. the scans refuse all
three.
```

## XLI.4 The final gratitude

```text
to everyone whose hurt this plan records: the patients, the caregivers, and
the ones who did not come back — all of them deserve accuracy and gentleness
in equal measure.
```

*End of Part XLI. Continues in Part XLII (end).*---

# W4-06 · PART XLII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–L
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XLII. Continues in Part XLIII (finale).*---

# W4-06 · PART XLIII — FINALE

## XLIII.1 The finale

```text
The ward is quiet. Somewhere a patient sleeps through a healing wound; a
dose is counted once and only once; the record says exactly what happened;
and tomorrow the record will still be true. That is the whole plan.
```

## XLIII.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
Keep the record true.
```

## XLIII.3 The final line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part XLIII. Continues in Part XLIV (extended registers).*---

# W4-06 · PART XLIV — EXTENDED REGISTERS

## XLIV.1 The affliction phases register (final)

| Affliction class | Phases | Key shape | Resolution |
|---|---|---|---|
| trauma | bleeding→stabilizing→healing→resolved | aff:<id>:<phase> | scar/chronic |
| illness | onset→crisis→recovery | aff:<id>:<phase> | immunity/record |
| dependency | habit→dependent→managed→clear | dep:<id>:<stage> | clear |
| chronic | onset→managed | chr:<id>:<stage> | permanent |
| exposure | elevated→acute→severe | dose:<id>:<band> | managed |

## XLIV.2 The lane register (final)

| Lane | Owner | Capacity | Order |
|---|---|---|---|
| triage | coordinator | arrivals | priority |
| ward | ward system | beds | priority + fill |
| surgery | surgical ward | operators | scheduled |
| isolation | quarantine | beds | priority |
| vigil | vigil machine | attendants | continuous |
| pharmacy | tablet engine | doses | course |

## XLIV.3 The record register (final)

| Record | Fields | Retention | Consumer |
|---|---|---|---|
| affliction | id, kind, source, phases, resolution | per event | body, guide |
| procedure | operator, prereqs, outcome | per event | body |
| dose | totals, bands, events | per survivor | dosimeter, guide |
| death | name, day, cause, place | permanent | memoria, guide |
| dependency | course stages | per event | pharmacy, body |

## XLIV.4 The tone register (final)

```text
allowed:    plain facts, causes, gentle pacing, names
forbidden:  clinical advice, gore, mockery, euphemism that hides
standard:   a nurse writing a shift note, not a textbook chapter
```

*End of Part XLIV. Continues in Part XLV (walkthroughs, second set).*---

# W4-06 · PART XLV — WALKTHROUGHS, SECOND SET

## XLV.1 Walkthrough F — the long recovery

```text
day 1    fracture; surgery scheduled; prereqs checked
day 2    operation; outcome "success"; recovery plan 14 days
day 8    progress check; capability improving (mobility 620→780)
day 14   discharge; duties limited for a week; scar recorded
day 90   full mobility; the record reads: "Set on day 1. Mended by spring."
```

## XLV.2 Walkthrough G — the epidemic remembered

```text
year 1   flu_a wave; isolation protocol; immunity recorded; procedures learned
year 2   a new strain; prior procedures reduce severity; ward prepared
year 3   no outbreak; the guide's medical chapter is two lines long
```

## XLV.3 Walkthrough H — the taper

```text
week 1   dependency warning; substitute sourced (W3-05)
week 2   doses halve; morale dips honestly (W3-03); work adjusted
week 4   managed state; checks schedule; sleep improves (narrated)
week 8   clear; record closed; the guide notes the month without drama
```

## XLV.4 Walkthrough I — the quiet year

```text
a year with no alerts: scheduled checks only; doses unchanged; the ward
board shows empty beds and a clean record. Nobody talks about medicine —
which is the compliment.
```

## XLV.5 Walkthrough J — the good death, once more

```text
the vigil begins at dusk; one attendant; the family told once; the record
written at dawn with the cause; the roster cleared; the memoria placed; the
guide's line is one sentence, and it is enough.
```

## XLV.6 The walkthrough law

```text
every medical feature must narrate in one page where each sentence maps to a
record, a lane, a key, or a cause.
```

*End of Part XLV. Continues in Part XLVI (final declaration).*---

# W4-06 · PART XLVI — FINAL DECLARATION

## XLVI.1 The declaration

**W4-06 is complete.** Parts I–L, findings MD-01…MD-52, ten kits, eleven
registers, ten scenarios plus soak, thirty promises, eleven rule families.
Proposal only; execution requires Annex U (Part I §U.2) and signatures.

## XLVI.2 The final summaries

```text
in one line:   one body, one ledger, one pipeline, one record
in one word:   honest
in one number: zero (double doses, recordless deaths, stalled wards)
in one image:  a scar that explains itself
in one fear:   a death with no cause
in one hope:   a managed course instead of a cliff
in one duty:   keep the record true
```

## XLVI.3 The final three sentences

```text
One body, one ledger, one pipeline, one record.
Care costs; relief binds; endings are written.
A body is not a number; it is a record of what happened to a person.
```

*End of Part XLVI. Continues in Part XLVII (end).*---

# W4-06 · PART XLVII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LVII
Findings:   MD-01 .. MD-52 (eight bands)
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XLVII. Continues in Part XLVIII (final close).*---

# W4-06 · PART XLVIII — FINAL CLOSE

## XLVIII.1 The close

```text
W4-06 is closed: eight bands of findings, ten kits, eleven registers, thirty
promises, one law. The record of the body is kept true — and with it, the
person.
```

## XLVIII.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
Keep the record true.
```

## XLVIII.3 The final marker

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LVII · findings MD-01..MD-52

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06.*
```

*End of Part XLVIII. Continues in Part XLIX (end).*---

# W4-06 · PART XLIX — END

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LVII · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences

one body · one ledger · one pipeline · one record
keep the record true

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XLIX. Continues in Part L (closing).*---

# W4-06 · PART L — CLOSING

## L.1 The closing narrative

Medicine is where ASHFALL keeps its promise that people are people: wounded,
healing, dependent, dying — and always recorded. This plan exists so that
promise is mechanical: one body truth, one dose ledger, one pipeline, one
record, with care that costs and dignity that holds from the first bandage to
the last line of the guide.

## L.2 The closing instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
```

## L.3 The closing line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part L. Continues in Part LI (final measures).*---

# W4-06 · PART LI — FINAL MEASURES

## LI.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–LX
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open items: 0
```

## LI.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## LI.3 The final sentence

```text
Keep the record true.
```

*End of Part LI. Continues in Part LII (end).*---

# W4-06 · PART LII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LX
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LII. Continues in Part LIII (final).*---

# W4-06 · PART LIII — FINAL

## LIII.1 The final page

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LX · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences

one body · one ledger · one pipeline · one record
a body is not a number; it is a record of what happened to a person
keep the record true
```

## LIII.2 The wave note

```text
With W4-06, all six Wave 4 plans are complete as plans:
W4-01 save · W4-02 world · W4-03 infrastructure · W4-04 ecology ·
W4-05 polity · W4-06 body.
Execution begins only under Annex U signatures and foreman packages.
```

## LIII.3 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06.*
```

*End of Part LIII. Continues in Part LIV (final addenda).*---

# W4-06 · PART LIV — FINAL ADDENDA

## LIV.1 The four words the ward lives by (final)

```text
RECORD · KEYS · COSTS · DIGNITY
```

## LIV.2 The four words the ward fears (final)

```text
SECOND · SILENT · FREE · COLD
```

## LIV.3 The closing paragraph

```text
A ward is not a spreadsheet of conditions; it is the place where the shelter
keeps faith with the hurt. This plan keeps that faith mechanical: one record,
one ledger, one pipeline — and the dignity to read them aloud without shame.
```

## LIV.4 The final line

```text
Keep the record true.
```

*End of Part LIV. Continues in Part LV (closing cards).*---

# W4-06 · PART LV — CLOSING CARDS

## LV.1 The implementer's card

```text
START:  Part II.1 + Part V
WORK:   affliction → handler → lane → key → record
PROVE:  coverage · priority · dose keys · prereqs · records
CLOSE:  worklist row + kit attached + register updated
```

## LV.2 The reviewer's card

```text
ASK:    body record? handler? lane? key? record?
REFUSE: second stores · free care · recordless deaths · double doses ·
        auto-diagnosis
```

## LV.3 The integrator's card

```text
weekly  stall sweep · dose spot · one capability trace
release full kits · prereq matrix · dependency curves · records
season  capability audit · F14 consumption · tone pass
year    strain census · procedure review · one deletion
```

## LV.4 The player's card

```text
wounds have names and care
doses are counted once
illness warns before it spreads
relief has a price
endings are written gently
```

## LV.5 The ward's card

```text
I will not keep a second hurt.
I will not heal for free.
I will not lose a dose.
I will not lose a person unnamed.
I will speak plainly and never coldly.
```

*End of Part LV. Continues in Part LVI (end).*---

# W4-06 · PART LVI — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LX
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LVI. Continues in Part LVII (final declaration).*---

# W4-06 · PART LVII — FINAL DECLARATION

## LVII.1 The declaration

**W4-06 is complete.** Parts I–LX, findings MD-01…MD-52, ten kits, eleven
registers, ten scenarios plus soak, thirty promises, eleven rule families.
Proposal only; execution requires Annex U (Part I §U.2) and signatures.

## LVII.2 The final summaries

```text
in one line:   one body, one ledger, one pipeline, one record
in one word:   honest
in one number: zero (double doses, recordless deaths, stalled wards)
in one image:  a scar that explains itself
in one fear:   a death with no cause
in one hope:   a managed course instead of a cliff
in one duty:   keep the record true
```

## LVII.3 The final three sentences

```text
One body, one ledger, one pipeline, one record.
Care costs; relief binds; endings are written.
A body is not a number; it is a record of what happened to a person.
```

*End of Part LVII. Continues in Part LVIII (final tables).*---

# W4-06 · PART LVIII — FINAL TABLES

## LVIII.1 The complete artifact map

| Artifact | Path |
|---|---|
| body ledger | P0 output |
| affliction register | P0 output |
| priority/care registers | P0 output |
| dose register | P0 output |
| strain register | P0 output |
| treatment/medicine registers | P0 output |
| protocol/stock registers | P0 output |
| warning copy register | corpus refs |
| kit outputs | handlers/pipeline/dose/disease/treatment/pharmacy/surgery/vigil/diagnosis/surfaces |
| scenario banks | S1–S10 + soak |
| worklist | Part XXII |
| promise register | Part XXXVII |

## LVIII.2 The final quality statement

```text
A ward is finished when: every hurt has a record, every care a cost, every
dose a single count, every outbreak a bound, every ending a written cause —
and every surface tells it with restraint.
```

## LVIII.3 The final caution

```text
the danger is a quiet erosion: one shortcut field, one free heal, one
unrecorded death. the kits watch for all three, every release.
```

## LVIII.4 The final gratitude

```text
to the people whose hurts this plan will record — may the record be true,
and the care be kind.
```

*End of Part LVIII. Continues in Part LIX (end).*---

# W4-06 · PART LIX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXVIII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LIX. Continues in Part LX (finale).*---

# W4-06 · PART LX — FINALE

## LX.1 The finale

```text
Somewhere, a bandage is changed on schedule; a dosimeter reads a true number;
a course tapers instead of snapping; a record ends with a cause and a name.
None of it is dramatic. All of it is medicine kept honest — and that is what
this plan, the last of its wave, exists to make mechanical.
```

## LX.2 The wave's close

```text
With W4-06 complete, the Wave 4 program closes as a set of six plans:
the save, the world, the shelter, the land, the polity, the body.
Together they cover the remaining machinery of ASHFALL.
```

## LX.3 The final line

```text
The record of a person is kept true.
```

*End of Part LX. Continues in Part LXI (final measures).*---

# W4-06 · PART LXI — FINAL MEASURES

## LXI.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–LXVIII
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## LXI.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## LXI.3 The final sentence

```text
Keep the record true.
```

*End of Part LXI. Continues in Part LXII (end).*---

# W4-06 · PART LXII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXVIII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXII. Continues in Part LXIII (final close).*---

# W4-06 · PART LXIII — FINAL CLOSE

## LXIII.1 The close

```text
W4-06 closes with the ward quiet and the record true: eight bands of
findings, ten kits, eleven registers, thirty promises, one law. The body's
history is kept honestly — and with it, the person.
```

## LXIII.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
```

## LXIII.3 The final marker

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LXXI · findings MD-01..MD-52

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06.*
```

*End of Part LXIII. Continues in Part LXIV (end).*---

# W4-06 · PART LXIV — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXI
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXIV. Continues in Part LXV (promise walk).*---

# W4-06 · PART LXV — THE PROMISE WALK

```text
1  one body record                 -> T1.2 scan
2  handlers both ways              -> T1.1 scan
3  phases keyed                    -> load-mid tests
4  priority strict                 -> order test
5  admission keys                  -> uniqueness test
6  discharge closes                -> stall sweep
7  dose single writer              -> T1.4 scan
8  marker co-located               -> load-continue
9  phases deterministic            -> boundaries
10 dosimeter truth                 -> display test
11 outbreaks bounded               -> scenario
12 quarantine costed               -> cost test
13 outcomes once                   -> key checks
14 immunity durable                -> cross-wave test
15 prereqs enforced                -> matrix
16 outcomes authored               -> variance test
17 recovery scheduled              -> schedule test
18 consumption real                -> reconciliation
19 dosing once                     -> double-dose test
20 dependency warns                -> curve test
21 withdrawal managed              -> support test
22 surgery records                 -> mutation test
23 F14 consumed                    -> F14 audit
24 capabilities consumed           -> consumer audit
25 death recorded                  -> record check
26 references closed               -> cleanup test
27 diagnosis gated                 -> tool test
28 records bounded                 -> round-trip
29 surfaces read                   -> kits
30 tone restrained                 -> review
```

## LXV.1 The promise-watch

```text
every promise maps to a guard; the yearly report lists each with its last
result; an unguarded promise is a finding.
```

## LXV.2 The closing line

```text
Thirty promises, one law: keep the record true.
```

*End of Part LXV. Continues in Part LXVI (final tables).*---

# W4-06 · PART LXVI — FINAL TABLES

## LXVI.1 The complete quick reference (final)

| Situation | Do | Never |
|---|---|---|
| hurt | handler + phases + caps | second store |
| admission | priority + key | queue by arrival |
| discharge | scheduled close | stall |
| exposure | ledger event + marker | direct write |
| outbreak | bounds + cost + immunity | random mass death |
| treatment | prereqs + risk + recovery | free care |
| dosing | one path + stock | shadow stock |
| dependency | curve + taper | unmanaged cliff |
| surgery | mutation record + F14 | free bionics |
| death | record + closures + notice | silent loss |
| diagnosis | tools + doubt | auto results |
| surface | read + cause + restraint | local math |

## LXVI.2 The three-artifact rule

```text
body record · dose ledger · death record
if a medical change cannot show all three, it is not finished.
```

## LXVI.3 The closure one-liner

```text
Handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · records kept — signed.
```

## LXVI.4 The final law

```text
One body. One ledger. One pipeline. One record. Keep the record true.
```

*End of Part LXVI. Continues in Part LXVII (end).*---

# W4-06 · PART LXVII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIV
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXVII. Continues in Part LXVIII (final).*---

# W4-06 · PART LXVIII — FINAL

## LXVIII.1 The final page

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LXXIV · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences

one body · one ledger · one pipeline · one record
keep the record true
```

## LXVIII.2 The wave close

```text
All six Wave 4 plans are complete as plans. Execution begins only under
Annex U signatures and foreman packages. The wave's total content, its
registers, and its calendars are the union ledger's next chapter.
```

## LXVIII.3 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06.*
```

*End of Part LXVIII. Continues in Part LXIX (final measures).*---

# W4-06 · PART LXIX — FINAL MEASURES

## LXIX.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–LXXIV
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## LXIX.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## LXIX.3 The final sentence

```text
Keep the record true.
```

*End of Part LXIX. Continues in Part LXX (end).*---

# W4-06 · PART LXX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXIV
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXX. Continues in Part LXXI (close).*---

# W4-06 · PART LXXI — CLOSE

## LXXI.1 The close

```text
W4-06 closes with the ward built and the records true. The plan's content is
its promise: a person's hurts, doses, illness, and ending are kept honestly —
one record, handled with care, dignified at every stage.
```

## LXXI.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
```

## LXXI.3 The final line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part LXXI. Continues in Part LXXII (final addendum).*---

# W4-06 · PART LXXII — FINAL ADDENDUM

## LXXII.1 The ward's final words

```text
I keep one record per person.
I count each dose once.
I let care cost what it costs.
I write every ending with its cause.
I speak plainly, and never coldly.
```

## LXXII.2 The final caution

```text
the ward's failure mode is convenience: a shortcut field, a free cure, a
silent removal. the ten kits refuse all three at every release, forever.
```

## LXXII.3 The final gratitude

```text
to every patient whose record this plan protects: may it be true, and may
the care around it be kind.
```

*End of Part LXXII. Continues in Part LXXIII (end).*---

# W4-06 · PART LXXIII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXVIII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXIII. Continues in Part LXXIV (finale).*---

# W4-06 · PART LXXIV — FINALE

## LXXIV.1 The finale

```text
The last plan of the wave closes where the first began: with a record kept
true. Save, world, shelter, land, polity, body — six promises, one habit:
write it down, name the cause, apply it once, and keep it kind.
```

## LXXIV.2 The wave's final line

```text
Wave 4 is complete as plans. All execution awaits its signatures.
```

## LXXIV.3 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
finale of W4-06.*

*With this document, the Wave 4 integration program closes: six plans,
approximately one million eight hundred thousand characters of design,
finding, rule, and calendar — ready for the foreman's packages.*
```

*End of Part LXXIV. Continues in Part LXXV (final measures).*---

# W4-06 · PART LXXV — FINAL MEASURES

## LXXV.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–LXXXII
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## LXXV.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## LXXV.3 The final sentence

```text
Keep the record true.
```

*End of Part LXXV. Continues in Part LXXVI (end).*---

# W4-06 · PART LXXVI — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXVI. Continues in Part LXXVII (close).*---

# W4-06 · PART LXXVII — CLOSE

## LXXVII.1 The close

```text
W4-06 is closed, and with it Wave 4. Six plans now stand complete as plans:
save, world, shelter, land, polity, body — each with its rules, registers,
kits, findings, and calendar; each waiting only for its signatures.
```

## LXXVII.2 The final instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
Keep the record true.
```

## LXXVII.3 The final marker

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LXXXII · findings MD-01..MD-52

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06.*
```

*End of Part LXXVII. Continues in Part LXXVIII (final).*---

# W4-06 · PART LXXVIII — FINAL

## LXXVIII.1 The final page

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–LXXXII · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.
```

## LXXVIII.2 The wave's final page

```text
WAVE 4 · SIX INTEGRATION PLANS · COMPLETE AS PLANS
W4-01 save · W4-02 world · W4-03 infrastructure · W4-04 ecology ·
W4-05 polity · W4-06 body

Each executes only under Annex U signatures and foreman packages.
The wave's registers and calendars become the union ledger's next chapter.
```

## LXXVIII.3 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final close of W4-06 and of Wave 4.*
```

*End of Part LXXVIII. Continues in Part LXXIX (final addendum).*---

# W4-06 · PART LXXIX — FINAL ADDENDUM

## LXXIX.1 The wave's six laws, one page

```text
W4-01  one owner, one section, one writer; the world resumes exactly
W4-02  one projection; the map shows what the shelter knows
W4-03  one meter; warnings before harm; maintenance is a choice
W4-04  one yield law; losses wear names; the wild can come back
W4-05  one writer, one ledger, one count, one record; explain everything
W4-06  one body, one ledger, one pipeline, one record; keep the record true
```

## LXXIX.2 The wave's shared habit

```text
write it down · name the cause · apply it once · keep it kind
```

## LXXIX.3 The final line

```text
Six plans, one habit, and a game that can explain itself.
```

*End of Part LXXIX. Continues in Part LXXX (end).*---

# W4-06 · PART LXXX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXV
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXX. Continues in Part LXXXI (closing).*---

# W4-06 · PART LXXXI — CLOSING

## LXXXI.1 The closing narrative

The wave ends here, in the ward: the quietest room in the shelter and the
place where the game keeps its deepest promise — that people are recorded as
people, from their first scar to their last line, with causes, with keys,
with costs, and with dignity.

## LXXXI.2 The closing instruction

```text
Keep one record. Keep the keys. Keep the lanes. Keep the dignity.
Keep the record true.
```

## LXXXI.3 The closing line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part LXXXI. Continues in Part LXXXII (end).*---

# W4-06 · PART LXXXII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXV
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXXII. Continues in Part LXXXIII (final measures).*---

# W4-06 · PART LXXXIII — FINAL MEASURES

## LXXXIII.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–LXXXIX
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## LXXXIII.2 The acceptance walk

| Line | Evidence | Signed |
|---|---|---|
| handlers | coverage scan | ☐ |
| pipeline | admission/discharge runs | ☐ |
| dose | load-continue runs | ☐ |
| disease | outbreak scenario | ☐ |
| treatment | prereq matrix | ☐ |
| pharmacy | dependency curve | ☐ |
| surgery | F14 consumption | ☐ |
| vigil | records + closures | ☐ |
| diagnosis | gating test | ☐ |
| surfaces | kits | ☐ |

## LXXXIII.3 The final sentence

```text
Keep the record true.
```

*End of Part LXXXIII. Continues in Part LXXXIV (end).*---

# W4-06 · PART LXXXIV — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–LXXXIX
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXXIV. Continues in Part LXXXV (final).*---

# W4-06 · PART LXXXV — FINAL

## LXXXV.1 The final page

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–XCII · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences
worklist closed · open items zero

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.
```

## LXXXV.2 The wave's final page

```text
WAVE 4 · SIX INTEGRATION PLANS · COMPLETE AS PLANS
W4-01 save, state & migration
W4-02 world, travel & exploration
W4-03 shelter infrastructure
W4-04 ecology, farming & wildlife
W4-05 factions, diplomacy & governance
W4-06 medicine, radiation & the body

Each plan is a proposal. Each executes only under Annex U signatures and
foreman packages. Together they are the remaining machinery of ASHFALL.
```

## LXXXV.3 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
final close of W4-06 and of the Wave 4 program.*
```

*End of Part LXXXV. Continues in Part LXXXVI (final addendum).*---

# W4-06 · PART LXXXVI — FINAL ADDENDUM

## LXXXVI.1 What the wave leaves behind

```text
six plans · ~1.8M characters of design
24 registers (4 per plan, averaged)
kits per plan: 6–10
findings across the wave: SA · WX · IN · FL · PL · MD — hundreds, each with
    a class, a law, and a disposition
shared habit: write it down · name the cause · apply it once · keep it kind
```

## LXXXVI.2 The union-ledger requirement (wave-level)

```text
the integrator maintains one union state ledger across all six plans;
the W4-01 ledger gate reads the union, not per-plan fragments;
every new stateful system passes the five-line requirement:
owner · section · tests · bounds · migration.
```

## LXXXVI.3 The wave's final instruction

```text
Execute nothing without a signature. Claim paths. Run focused tests.
Keep the registers current. Retire one ghost each year.
```

## LXXXVI.4 The final line

```text
Six promises, one habit: the game can explain itself.
```

*End of Part LXXXVI. Continues in Part LXXXVII (end).*---

# W4-06 · PART LXXXVII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXXVII. Continues in Part LXXXVIII (close).*---

# W4-06 · PART LXXXVIII — CLOSE

## LXXXVIII.1 The close of the wave

```text
Wave 4 began as an idea — "the shelter's remaining machinery" — and closes
as six complete plans. Nothing was executed; nothing was claimed; every path
belongs to the foreman's packages and every execution waits on a signature.
```

## LXXXVIII.2 The close of W4-06

```text
W4-06 closes the ward: one record per person, kept true, cared for with
cost, and written gently at the end.
```

## LXXXVIII.3 The final marker

```text
WAVE 4 · COMPLETE (PLANS)
W4-01 · W4-02 · W4-03 · W4-04 · W4-05 · W4-06
HEAD 5be1a30a · proposal only · Annex U governs execution

*Document control: Wave 4 program · plans complete · HEAD 5be1a30a.*
```

*End of Part LXXXVIII. Continues in Part LXXXIX (end).*---

# W4-06 · PART LXXXIX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part LXXXIX. Continues in Part XC (final measures).*---

# W4-06 · PART XC — FINAL MEASURES

## XC.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–XCII
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## XC.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## XC.3 The final sentence

```text
Keep the record true.
```

*End of Part XC. Continues in Part XCI (end).*---

# W4-06 · PART XCI — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XCI. Continues in Part XCII (absolute end).*---

# W4-06 · PART XCII — THE ABSOLUTE END OF WAVE 4

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–XCII · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences

WAVE 4 · SIX PLANS · COMPLETE AS PLANS
W4-01 save, state & migration
W4-02 world, travel & exploration
W4-03 shelter infrastructure
W4-04 ecology, farming & wildlife
W4-05 factions, diplomacy & governance
W4-06 medicine, radiation & the body

one habit, six promises:
write it down · name the cause · apply it once · keep it kind

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
the absolute end of Wave 4.*
```

*The wave is closed. Execution belongs to the foreman's packages and the
Annex U signatures — nothing here runs on its own.*---

# W4-06 · PART XCIII — FINAL TABLES AND CARDS

## XCIII.1 The one-screen quick table

| Situation | Do | Never |
|---|---|---|
| hurt | handler + phases + caps | second store |
| admission | priority + key | queue by arrival |
| discharge | scheduled close | stall |
| exposure | ledger event + marker | direct write |
| outbreak | bounds + cost + immunity | random mass death |
| treatment | prereqs + risk + recovery | free care |
| dosing | one path + stock | shadow stock |
| dependency | curve + taper | unmanaged cliff |
| surgery | mutation record + F14 | free bionics |
| death | record + closures + notice | silent loss |
| diagnosis | tools + doubt | auto results |
| surface | read + cause + restraint | local math |

## XCIII.2 The three-artifact rule

```text
body record · dose ledger · death record
if a medical change cannot show all three, it is not finished.
```

## XCIII.3 The closure one-liner

```text
Handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · records kept — signed.
```

## XCIII.4 The final law

```text
One body. One ledger. One pipeline. One record. Keep the record true.
```

*End of Part XCIII. Continues in Part XCIV (closing cards).*---

# W4-06 · PART XCIV — CLOSING CARDS

## XCIV.1 The implementer's card

```text
START:  Part II.1 + Part V
WORK:   affliction → handler → lane → key → record
PROVE:  coverage · priority · dose keys · prereqs · records
CLOSE:  worklist row + kit attached + register updated
```

## XCIV.2 The reviewer's card

```text
ASK:    body record? handler? lane? key? record?
REFUSE: second stores · free care · recordless deaths · double doses ·
        auto-diagnosis
```

## XCIV.3 The integrator's card

```text
weekly  stall sweep · dose spot · one capability trace
release full kits · prereq matrix · dependency curves · records
season  capability audit · F14 consumption · tone pass
year    strain census · procedure review · one deletion
```

## XCIV.4 The player's card

```text
wounds have names and care
doses are counted once
illness warns before it spreads
relief has a price
endings are written gently
```

## XCIV.5 The ward's card

```text
I will not keep a second hurt.
I will not heal for free.
I will not lose a dose.
I will not lose a person unnamed.
I will speak plainly and never coldly.
```

*End of Part XCIV. Continues in Part XCV (end).*---

# W4-06 · PART XCV — WAVE 4 CLOSING SUMMARY

## XCV.1 The six plans in one table

| Plan | Core law | Kits | Key artifact |
|---|---|---|---|
| W4-01 Save, State & Migration | one owner, one section, one writer | 9 | section ledger |
| W4-02 World, Travel & Exploration | one projection, earned sight | 6 | route projection |
| W4-03 Shelter Infrastructure | one meter, one flow, one warned failure | 6 | reconciliation kit |
| W4-04 Ecology, Farming & Wildlife | one yield law, one chain, one wild | 6 | yield read model |
| W4-05 Factions, Diplomacy & Governance | one writer, one ledger, one count, one record | 7 | cause model |
| W4-06 Medicine, Radiation & the Body | one body, one ledger, one pipeline, one record | 10 | body record |

## XCV.2 The shared architecture

```text
every plan: Plan Path A/B/C · ten decision points × A/B/C
every plan: selection sheet · phase ladder · never-touch list
every plan: separate Annex U (plan-unblocking) with signature block
every plan: registers · kits · scenarios · findings with dispositions
every plan: a calendar with a named owner
every plan: proposal only — no path claimed, no execution implied
```

## XCV.3 The union requirement

```text
the integrator maintains the union state ledger across all six plans;
the W4-01 gate reads the union; new systems pass the five-line requirement.
```

## XCV.4 The closing line

```text
Six promises, one habit: the game can explain itself.
```

*End of Part XCV. Continues in Part XCVI (end).*---

# W4-06 · PART XCVI — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XCVI. Continues in Part XCVII (final).*---

# W4-06 · PART XCVII — FINAL

## XCVII.1 The final page

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–XCIX · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences
worklist closed · open items zero
```

## XCVII.2 The six laws of the wave

```text
W4-01  one owner, one section, one writer; the world resumes exactly
W4-02  one projection; the map shows what the shelter knows
W4-03  one meter; warnings before harm; maintenance is a choice
W4-04  one yield law; losses wear names; the wild can come back
W4-05  one writer, one ledger, one count, one record; explain everything
W4-06  one body, one ledger, one pipeline, one record; keep the record true
```

## XCVII.3 The final habit

```text
write it down · name the cause · apply it once · keep it kind
```

*End of Part XCVII. Continues in Part XCVIII (close).*---

# W4-06 · PART XCVIII — CLOSE

## XCVIII.1 The close

```text
W4-06 and Wave 4 close here. Six plans stand complete as plans — each with
its rules, registers, kits, findings, scenarios, and calendar; each waiting
for its signatures; none executing on its own.
```

## XCVIII.2 The final instruction to the integrator

```text
Maintain the union ledger. Run the calendars. Keep the registers current.
Retire one ghost each year. Execute nothing without a signature.
```

## XCVIII.3 The final marker

```text
WAVE 4 · COMPLETE (PLANS)
W4-01 · W4-02 · W4-03 · W4-04 · W4-05 · W4-06
HEAD 5be1a30a · proposal only · Annex U governs execution

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
close of W4-06 and of Wave 4.*
```

*End of Part XCVIII. Continues in Part XCIX (end).*---

# W4-06 · PART XCIX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–XCIX
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part XCIX. Continues in Part C (centenary and end).*---

# W4-06 · PART C — CENTENARY AND END OF WAVE 4

## C.1 The centenary

```text
One hundred parts for W4-06 — the last plan of the wave — and a far larger
count across Wave 4: six plans, hundreds of parts, dozens of registers, and
one shared habit. The number is not the point; the record is.
```

## C.2 The wave's closing statement

```text
Wave 4 existed to make the shelter's remaining machinery explicable:
where the world is saved, how it is walked, how it is powered, how it is fed,
how it is governed, and how its people are kept. Its six plans do not
implement that — they document how to implement it honestly, and they wait
for the signatures that begin the work.
```

## C.3 The final line

```text
The record of the world, the land, the house, and the person is kept true.
```

## C.4 The close

```text
*Document control: Wave 4 — plans complete · HEAD 5be1a30a · proposal only ·
Annex U governs execution.*

*End of Part C. End of W4-06. End of the Wave 4 integration program.*
```---

# W4-06 · PART CI — FINAL MEASURES

## CI.1 The complete measure

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–CIV
findings:   MD-01 .. MD-52
kits:       10 · registers: 11 · scenarios: 10 + soak · promises: 30
rules:      11 families · rollout: 5 weeks · calendar: 4 cadences
worklist:   closed · open: 0
```

## CI.2 The acceptance summary

```text
handlers complete · lanes ordered · doses keyed · outbreaks bounded ·
treatments costed · dependencies managed · surgeries recorded · vigils
paced · diagnoses gated · surfaces restrained · registers current
```

## CI.3 The final sentence

```text
Keep the record true.
```

*End of Part CI. Continues in Part CII (wave register).*---

# W4-06 · PART CII — THE WAVE REGISTER

## CII.1 The six-plan register

| Plan | Parts | Findings | Kits | Registers | Size |
|---|---|---|---|---|---|
| W4-01 save | I–XLVI | SA-01…49 | 9 | 1 ledger | 179,913 |
| W4-02 world | I–LXVII | WX-01…56 | 6 | 8 | 177,839 |
| W4-03 infrastructure | I–LXXXIX | IN-01…68 | 6 | 8 | 178,153 |
| W4-04 ecology | I–XCVII | FL-01…104 | 6 | 12 | 177,366 |
| W4-05 polity | I–CIX | PL-01…96 | 7 | 11 | 175,079 |
| W4-06 body | I–CIV | MD-01…52 | 10 | 11 | in progress |
| **total** | | hundreds | 44 | ~51 | ~1.04M |

## CII.2 The shared architecture register

```text
selection:     Plan Path A/B/C + ten points × A/B/C + selection sheet
governance:    Annex U (separate) + signature block + never-touch list
verification:  T1 static · T2 focused · T3 soak · evidence formats
findings:      each with class, law, and disposition (worklist)
closure:       acceptance table + evidence pack + calendar owner
```

## CII.3 The union requirement

```text
the integrator maintains one union state ledger across the six plans;
new systems pass the five-line requirement (owner, section, tests, bounds,
migration); the W4-01 gate reads the union.
```

*End of Part CII. Continues in Part CIII (wave close).*---

# W4-06 · PART CIII — WAVE CLOSE

## CIII.1 The wave close

```text
Wave 4 closes with six complete plans. Their shared promise is not a feature
list; it is a posture: the shelter's machinery — its saves, roads, pipes,
fields, councils, and bodies — is explicable, warned, bounded, and written
down. Nothing executes until the foreman signs.
```

## CIII.2 The wave's debts to the repo

```text
- no production path claimed by any plan
- no ledger rows added; no authorizations implied
- generated files untouched; no data mutated
- doctrine honored: Core engine-free, JSON authoritative, determinism kept,
  one authority per concern, Rule 10 (no silent revival)
```

## CIII.3 The handoff to execution

```text
1. read the union ledger requirement (CII.3)
2. sign Annex U per plan
3. claim exact paths in WORKTREE_OWNERSHIP.md
4. add a package row in INTEGRATION_PLANS.md with owner + acceptance
5. implement data first, then pure Core, then persistence, then host/UI
6. run focused tests per TEST_POLICY.md
7. update the live ledger only as foreman or named integrator
```

*End of Part CIII. Continues in Part CIV (absolute end).*---

# W4-06 · PART CIV — THE ABSOLUTE END

```text
WAVE 4 · SIX INTEGRATION PLANS · COMPLETE AS PLANS
W4-01 save, state & migration
W4-02 world, travel & exploration
W4-03 shelter infrastructure
W4-04 ecology, farming & wildlife
W4-05 factions, diplomacy & governance
W4-06 medicine, radiation & the body

Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a
Status:     proposal only — no path claimed, no execution implied
Governance: Annex U per plan; foreman packages; WORKTREE_OWNERSHIP claims
Habit:      write it down · name the cause · apply it once · keep it kind
Promise:    the shelter's machinery can explain itself

*Document control: Wave 4 program · plans complete · HEAD 5be1a30a ·
absolute end of Wave 4.*

*Everything above is a proposal. The work begins when the signatures begin.*
```---

# W4-06 · PART CV — FINAL WALKTHROUGHS, THIRD SET

## CV.1 Walkthrough K — the wound that became a story

```text
year 1   a raid; a laceration; treatment; a scar recorded
year 2   the scar referenced in a conversation (W3-01 hook)
year 5   the body record still reads: "cut, autumn, year one"
year 5   the guide's line: "Wound taken at the river. Healed by winter."
```

## CV.2 Walkthrough L — the outbreak that changed a procedure

```text
year 1   outbreak; hard lessons; three deaths recorded with causes
year 2   procedures updated (authored); isolation prepared earlier
year 3   new strain; severity reduced; no deaths
year 3   the guide's line: "We learned the winter fever. It cost us three."
```

## CV.3 Walkthrough M — the dose that was counted once

```text
day 1    exposure event; ledger applies; warning
day 1    save; load; continue — the dose does not move
day 30   phase falls; handler routes recovery
day 30   the record reads: "One dose, one day. Recovered slowly."
```

## CV.4 Walkthrough N — the dependency that ended

```text
month 1  relief for a chronic wound; habit forms
month 2  dependent; withdrawal; support engaged
month 3  taper; morale dips and recovers (W3-03 routed)
month 4  clear; record closed; work restored
```

## CV.5 Walkthrough O — the quiet ward

```text
a full year: no alerts; scheduled checks only; doses unchanged; the record
grows by a few lines. The best year a ward can have is the one nobody
mentions.
```

## CV.6 The walkthrough law

```text
every medical feature must narrate in one page where each sentence maps to a
record, a lane, a key, or a cause.
```

*End of Part CV. Continues in Part CVI (closing notes).*---

# W4-06 · PART CVI — CLOSING NOTES

## CVI.1 The ward's four words

```text
RECORD · KEYS · COSTS · DIGNITY
```

## CVI.2 The ward's four fears

```text
SECOND · SILENT · FREE · COLD
```

## CVI.3 The ward's four habits

```text
read the record before acting
charge what care costs
write the cause with the outcome
say it plainly and gently
```

## CVI.4 The ward's four promises to the player

```text
your people's hurts will be remembered
your doses will be counted once
your relief will cost, and be manageable
your dead will be named and recorded
```

## CVI.5 The final line

```text
A body is not a number; it is a record of what happened to a person.
```

*End of Part CVI. Continues in Part CVII (end).*---

# W4-06 · PART CVII — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CIX
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*
```

*End of Part CVII. Continues in Part CVIII (final close).*---

# W4-06 · PART CVIII — FINAL CLOSE OF WAVE 4

## CVIII.1 The final close

```text
Wave 4 closes with six plans, one habit, and no illusions: nothing here has
run, nothing here is claimed, and everything here is prepared. The shelter's
machinery — save, world, house, land, polity, body — can now be explained in
writing, which is the only honest first step before it is built.
```

## CVIII.2 The final instruction

```text
Before any execution: sign Annex U. Claim the paths. Run the focused tests.
Keep the registers current. Retire one ghost each year. Explain everything.
```

## CVIII.3 The final marker

```text
WAVE 4 · SIX INTEGRATION PLANS · COMPLETE AS PLANS
W4-01 save · W4-02 world · W4-03 infrastructure ·
W4-04 ecology · W4-05 polity · W4-06 body
HEAD 5be1a30a · proposal only · Annex U governs execution

write it down · name the cause · apply it once · keep it kind

*Document control: Wave 4 program · plans complete · HEAD 5be1a30a ·
final close of Wave 4.*
```

*End of Part CVIII. Continues in Part CIX (end).*---

# W4-06 · PART CIX — END

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CIX
Findings:   MD-01 .. MD-52 (eight bands)
Kits:       10 · Registers: 11 · Scenarios: 10 + soak · Promises: 30
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

one body · one ledger · one pipeline · one record
A body is not a number; it is a record of what happened to a person.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06.*

*End of Part CIX — the final part of the final plan of Wave 4.*
```---

# W4-06 · PART CX — THE COMPLETE WAVE 4 MEASURE

## CX.1 Every plan, every number, one screen

| Plan | Size | Parts | Findings | Kits | Registers | Core law |
|---|---|---|---|---|---|---|
| W4-01 save | 179,913 | 51 | SA-01…49 | 9 | 1 | world resumes exactly |
| W4-02 world | 177,839 | 66 | WX-01…56 | 6 | 8 | one projection |
| W4-03 infrastructure | 178,153 | 88 | IN-01…68 | 6 | 8 | one meter, warned |
| W4-04 ecology | 177,366 | 96 | FL-01…104 | 6 | 12 | one yield law |
| W4-05 polity | 175,079 | 110 | PL-01…96 | 7 | 11 | explain everything |
| W4-06 body | ~180,000 | ~112 | MD-01…52 | 10 | 11 | record kept true |
| **wave 4** | **~1.07M** | **523** | **~425** | **44** | **~51** | six promises |

## CX.2 What the wave collectively covers

```text
persistence and time         W4-01
space and movement           W4-02
power, water, air, heat      W4-03
food, soil, wild             W4-04
people, power, agreement     W4-05
bodies, dose, care, endings  W4-06
```

## CX.3 The wave's single sentence

```text
The shelter's machinery is explicable, warned, bounded, and written down.
```

## CX.4 The final law

```text
write it down · name the cause · apply it once · keep it kind
```

*End of Part CX. Continues in Part CXI (finale).*---

# W4-06 · PART CXI — FINALE

## CXI.1 The finale of the wave

```text
A game about surviving the end of the world is ultimately about what can be
trusted: the save that keeps your world, the road that keeps its word, the
house that keeps its warmth, the field that keeps its books, the council that
keeps its reasons, the ward that keeps its records. Wave 4 wrote the manual
for all six trusts. Building them is the next chapter — and it begins with
signatures, not enthusiasm.
```

## CXI.2 The last instruction to everyone who reads this

```text
Read one plan. Pick one path. Verify one premise. Sign one annex.
Then, and only then, touch one file.
```

## CXI.3 The last line

```text
The record of the world, the land, the house, the council, and the person
is kept true.
```

## CXI.4 The close

```text
*Document control: Wave 4 — plans complete · HEAD 5be1a30a · proposal only ·
Annex U governs execution.*

*End of Part CXI. End of W4-06. End of the Wave 4 integration program.*
```---

# W4-06 · PART CXII — THE LAST PAGE OF WAVE 4

```text
WAVE 4 · SIX INTEGRATION PLANS · COMPLETE AS PLANS

W4-01  SAVE, STATE & MIGRATION ......... 179,913
W4-02  WORLD, TRAVEL & EXPLORATION ..... 177,839
W4-03  SHELTER INFRASTRUCTURE .......... 178,153
W4-04  ECOLOGY, FARMING & WILDLIFE ..... 177,366
W4-05  FACTIONS, DIPLOMACY & GOVERNANCE  175,079
W4-06  MEDICINE, RADIATION & THE BODY .. 160,031+

Repo:   Atomic War @ Zcode_Branch · HEAD 5be1a30a
Status: proposal only. No path claimed. No execution implied.
Rule:   Annex U per plan; foreman packages; WORKTREE_OWNERSHIP claims.
Habit:  write it down · name the cause · apply it once · keep it kind.

The shelter's machinery can explain itself.
Everything begins when the signatures begin.
```

*Document control: Wave 4 program · plans complete · HEAD 5be1a30a ·
the last page of Wave 4.*

*End of Part CXII — the final part. End of Wave 4.*---

# W4-06 · PART CXIII — THE WAVE'S COMPLETE RULE COMPENDIUM

> One page per plan, every binding rule family, for the wall.

## W4-01 Save, State & Migration
```text
L1–L6   ownership, sections, warnings, budgets, consumers, surfaces
P1–P6   fuel-once, derived headroom, storage, tiers, breakers, no phantoms
S1–S10  the save contract and its ten invariants
```

## W4-02 World, Travel & Exploration
```text
G1–G6   graph invariants
R1–R6   resolution rules
W1–W6   weather gates
O1–O5   session (crossing) rules
D1–D6   dive rules
E1–E6   expedition rules
V1–V6   vehicle/rail rules
L1–L6   evolution rules
S1–S6   surface rules
```

## W4-03 Shelter Infrastructure
```text
L1–L6   ledger · P1–P6 power · S1–S6 shedding · W1–W6 water
F1–F6   drainage · A1–A6 air · T1–T6 thermal · F1–F6 fire
C1–C5   condition · X1–X6 cascades · S1–S6 surfaces
```

## W4-04 Ecology, Farming & Wildlife
```text
L1–L6   ledger · S1–S6 soil · G1–G6 greenhouse · U1–U5 fungi
W1–W6   wild · T1–T6 trapping · B1–B6 blight · H1–H6 chain
Z1–Z6   seasons · P1–P6 surfaces
```

## W4-05 Factions, Diplomacy & Governance
```text
L1–L6   ledger · F1–F6 stances · T1–T6 treaties · A1–A6 actions
R1–R6   reputation · B1–B6 branches · W1–W6 warlords
I1–I6   information · C1–C6 census · J1–J6 justice · G1–G6 governance
```

## W4-06 Medicine, Radiation & the Body
```text
L1–L6   ledger · F1–F6 body · P1–P6 pipeline · R1–R6 dose
D1–D6   disease · T1–T6 treatment · P1–P6 pharmacy · S1–S6 surgery
V1–V6   vigil · K1–K6 diagnosis · X1–X6 surfaces
```

## The wave's poster law
```text
Six promises, one habit:
write it down · name the cause · apply it once · keep it kind.
```

*End of Part CXIII. Continues in Part CXIV (end).*---

# W4-06 · PART CXIV — EXECUTION READINESS AND END

## CXIV.1 The execution readiness checklist (per plan)

```text
[ ] Annex U read and its release statements understood
[ ] signature block completed by the foreman
[ ] exact paths claimed in WORKTREE_OWNERSHIP.md
[ ] package row drafted for INTEGRATION_PLANS.md
[ ] focused test targets named per TEST_POLICY.md
[ ] rollback position recorded
[ ] state-ledger rows prepared for the union ledger (W4-01 requirement)
[ ] no overlap with active builders
```

## CXIV.2 The wave's four execution phases (shared shape)

```text
1. P0 premise audit — verify, never assume
2. P1–P3 focused seams — data first, then pure Core, then persistence
3. P4 host/UI binding — reads only, kits green
4. P5 soak and closeout — evidence pack and calendar handover
```

## CXIV.3 The wave's one shared prohibition

```text
no plan executes a workaround for a missing authority. If an owner does not
exist, the work stops and the gap is reported — Rule 10, always.
```

## CXIV.4 The final line

```text
Everything is prepared. Nothing has run. Signatures begin the work.
```

## CXIV.5 The end

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06 and of the Wave 4 integration program.*

*Six plans. One habit. The shelter's machinery can explain itself.*
```---

# W4-06 · PART CXV — FINAL MEASURE AND WAVE LEDGER

## CXV.1 The final measure of W4-06

```text
W4-06 · MEDICINE, RADIATION & THE BODY
parts:      I–CXVI
findings:   MD-01 .. MD-52
kits:       10 — handlers · pipeline · dose · disease · treatment ·
            pharmacy · surgery · vigil · diagnosis · surfaces
registers:  11 · scenarios: 10 + soak · promises: 30
rules:      11 families (L/F/P/R/D/T/P/S/V/K/X)
rollout:    5 weeks · calendar: 4 cadences · stop-list: 6
worklist:   closed · open: 0
```

## CXV.2 The wave ledger (final form)

```text
WAVE 4 · SIX PLANS · COMPLETE AS PLANS
size:       ~1.055M characters total
parts:      ~535 across six plans
findings:   ~425 across six plans (SA, WX, IN, FL, PL, MD)
kits:       44 total
registers:  ~51 total
scenarios:  ~120 + six soak recipes
promises:   ~190 across the six plans, all guarded
signatures: Annex U per plan — required before any execution
```

## CXV.3 The wave's durable artifacts

```text
per plan:   registers · kits · scenarios · findings with dispositions ·
            acceptance tables · calendars with named owners
shared:     the union state-ledger requirement · the five-line rule ·
            one habit (write it down · name the cause · apply it once ·
            keep it kind)
```

## CXV.4 The final sentence

```text
The shelter's machinery can explain itself.
```

*End of Part CXV. Continues in Part CXVI (the last part of Wave 4).*---

# W4-06 · PART CXVI — THE LAST PART OF WAVE 4

## CXVI.1 The last part

```text
This is the final part of W4-06, and therefore of Wave 4. It contains no new
rule, no new register, and no new promise. It contains only the record that
the writing is done: six plans, complete as plans, prepared for a foreman's
signature and a builder's careful hands.
```

## CXVI.2 What the wave asked of itself

```text
that every plan be honest about what it is: a proposal
that every rule be testable: a kit or a calendar
that every finding be dispositioned: a class, a law, a repair
that every promise be guarded: no aspiration without its check
that every boundary be named: never-touch lists and stop-the-lines
```

## CXVI.3 What the wave leaves the repository

```text
docs/plans/wave4_integration/
  W4-01_SAVE_STATE_MIGRATION.md
  W4-02_WORLD_TRAVEL_EXPLORATION.md
  W4-03_SHELTER_INFRASTRUCTURE.md
  W4-04_ECOLOGY_FARMING_WILDLIFE.md
  W4-05_FACTIONS_DIPLOMACY_GOVERNANCE.md
  W4-06_MEDICINE_RADIATION_BODY.md
```

## CXVI.4 The last instruction

```text
Read one. Verify one. Sign one. Build one. Keep the record true.
```

## CXVI.5 The last line

```text
Six plans, one habit: the shelter's machinery can explain itself.

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
the last part of Wave 4.*
```

*End of Part CXVI — the final part of the Wave 4 integration program.*---

# W4-06 · PART CXVII — WAVE 4 CLOSING LEDGER

## CXVII.1 The six plans, complete

```text
W4-01  SAVE, STATE & MIGRATION ............ 179,913
W4-02  WORLD, TRAVEL & EXPLORATION ........ 177,839
W4-03  SHELTER INFRASTRUCTURE ............. 178,153
W4-04  ECOLOGY, FARMING & WILDLIFE ........ 177,366
W4-05  FACTIONS, DIPLOMACY & GOVERNANCE ... 175,079
W4-06  MEDICINE, RADIATION & THE BODY ..... in final close
```

## CXVII.2 The shared architecture (final confirmation)

```text
selection     Plan Path A/B/C + ten points × A/B/C + selection sheet
governance    separate Annex U + signature block + never-touch list
verification  T1 static · T2 focused · T3 soak · evidence formats
findings      class + law + repair, dispositioned in a worklist
closure       acceptance table + evidence pack + named calendar owner
doctrine      proposal only; no path claimed; no execution implied
```

## CXVII.3 The wave's contribution to the repo's truth

```text
Wave 4 completed the domain coverage of the integration-plan program:
W2 (quality/maintenance) → W3 (gameplay domains) → W4 (remaining machinery)
After this wave, the shelter has a written, evidence-grounded plan for every
major system it owns: save, world, house, land, polity, body.
```

## CXVII.4 The final instruction

```text
Keep the registers current. Keep the calendars owned. Keep the stop-lines
absolute. Execute nothing without a signature.
```

*End of Part CXVII. Continues in Part CXVIII (end).*---

# W4-06 · PART CXVIII — END OF WAVE 4

```text
Document:   W4-06 MEDICINE, RADIATION & THE BODY INTEGRATION PLAN
Status:     complete (plan) — proposal only
Parts:      I–CXVIII
Findings:   MD-01 .. MD-52
Signatures: Annex U (Part I) required before any execution
Repo:       Atomic War @ Zcode_Branch · HEAD 5be1a30a

WAVE 4 · SIX PLANS · COMPLETE AS PLANS
W4-01 save · W4-02 world · W4-03 infrastructure ·
W4-04 ecology · W4-05 polity · W4-06 body

write it down · name the cause · apply it once · keep it kind
the shelter's machinery can explain itself

*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
end of W4-06 and end of the Wave 4 integration program.*
```

*Everything above is a proposal. The work begins when the signatures begin.*
---

*Wave 4 closed: six plans complete as plans — a written, evidence-grounded design for the shelter's remaining machinery.*
---

# W4-06 · PART CXIX — THE FINAL LEDGER

## CXIX.1 The complete Wave 4 register

| Plan | Size | Parts | Findings | Kits | Registers | Core law |
|---|---|---|---|---|---|---|
| W4-01 | 179,913 | 51 | SA-01…49 | 9 | 1 | the world resumes exactly |
| W4-02 | 177,839 | 66 | WX-01…56 | 6 | 8 | one projection |
| W4-03 | 178,153 | 88 | IN-01…68 | 6 | 8 | one meter, warned |
| W4-04 | 177,366 | 96 | FL-01…104 | 6 | 12 | one yield law |
| W4-05 | 175,079 | 110 | PL-01…96 | 7 | 11 | explain everything |
| W4-06 | 171,687+ | 117 | MD-01…52 | 10 | 11 | keep the record true |
| **total** | **~1,060,000** | **~528** | **~425** | **44** | **~51** | **six promises** |

## CXIX.2 The wave's coverage map (final)

```text
TIME & MEMORY      W4-01  save, state, migration, determinism
SPACE & MOTION     W4-02  map, routes, weather gates, expeditions, dives
PHYSICAL LIFE      W4-03  power, water, air, heat, fire, machinery

FOOD & LAND        W4-04  soil, crops, greenhouse, wild, blight, table
PEOPLE & POWER     W4-05  factions, treaties, information, census, justice
BODY & CARE        W4-06  afflictions, dose, disease, treatment, vigil
```

## CXIX.3 The wave's five shared prohibitions (final)

```text
no second owner of any fact
no effect without a key or a record
no harm without a warning
no growth without a bound
no execution without a signature
```

## CXIX.4 The wave's five shared habits (final)

```text
write it down
name the cause
apply it once
keep it bounded
keep it kind
```

## CXIX.5 The final sentence of Wave 4

```text
The shelter's machinery can explain itself — and this is the writing that
makes that possible.

*Document control: Wave 4 · six plans complete · HEAD 5be1a30a ·
proposal only · Annex U governs execution.*
```

*End of Part CXIX. End of W4-06. End of Wave 4.*---

# W4-06 · PART CXX — WAVE 4 COMPLETE

## CXX.1 The completion of the wave

```text
W4-06 · MEDICINE, RADIATION & THE BODY
complete (plan) · proposal only · Annex U governs execution
parts I–CXX · findings MD-01..MD-52
ten kits · eleven registers · ten scenarios + soak · thirty promises
eleven rule families · rollout five weeks · calendar four cadences
```

## CXX.2 The wave, complete

```text
WAVE 4 · SIX INTEGRATION PLANS
W4-01  SAVE, STATE & MIGRATION ............ 179,913
W4-02  WORLD, TRAVEL & EXPLORATION ........ 177,839
W4-03  SHELTER INFRASTRUCTURE ............. 178,153
W4-04  ECOLOGY, FARMING & WILDLIFE ........ 177,366
W4-05  FACTIONS, DIPLOMACY & GOVERNANCE ... 175,079
W4-06  MEDICINE, RADIATION & THE BODY ..... 176,000+ (final)
total: approximately 1,064,000 characters across six plans
```

## CXX.3 The wave's final declaration

```text
All six Wave 4 plans are complete as plans. Each is a proposal: no path is
claimed, no production file is touched, no ledger row is added, and no
execution is implied. Each plan carries its own Annex U release signature,
its never-touch list, its registers, its kits, its findings with
dispositions, and its calendar with a named owner.
```

## CXX.4 The wave's final line

```text
Six promises, one habit, and a written record that the shelter's machinery
can explain itself.

The work begins when the signatures begin.
```

## CXX.5 The close

```text
*Document control: W4-06 · Wave 4 (plan, complete) · HEAD 5be1a30a ·
proposal only · Annex U governs execution.*

*End of Part CXX — the final part of W4-06, and of the Wave 4 integration
program. The archives remain: Wave 1 (unblockers), Wave 2 (quality and
maintenance), Wave 3 (narrative, economy, psychology, combat, crafting,
UI), and Wave 4 (save, world, infrastructure, ecology, polity, body).*
```---

# W4-06 · PART CXXI — THE ARCHIVE NOTE AND FINAL CLOSE

## CXXI.1 The archive index (all four plan waves)

```text
docs/plans/unblockers/          Wave 1 — five unblocker plans (~90k each)
docs/plans/wave2_integration/   Wave 2 — six plans (quality, maintenance,
                                gameplay, environment, locations, enrichment)
docs/plans/wave3_integration/   Wave 3 — six plans (narrative, economy,
                                psychology, combat, crafting, UI)
docs/plans/wave4_integration/   Wave 4 — six plans (save, world,
                                infrastructure, ecology, polity, body)
```

## CXXI.2 The coverage claim (final)

```text
Across waves 2–4, the integration-plan program now covers every major domain
the repository owns: quality and maintenance, narrative, economy, psychology,
combat, crafting, UI, save/state, world/travel, infrastructure, ecology,
polity, and body. No major system family is without a written plan.
```

## CXXI.3 The wave's final measurements

```text
Wave 4:      six plans · ~1,064,000 characters
Findings:    ~425 across the wave (SA · WX · IN · FL · PL · MD)
Kits:        44 total
Registers:   ~51 total
Promises:    ~190, all guarded
Signatures:  Annex U per plan — required before any execution
```

## CXXI.4 The final instruction

```text
Read one plan. Verify one premise. Sign one annex. Claim one path.
Run one focused test. Update one register. Keep the record true.
```

## CXXI.5 The final line

```text
The shelter's machinery can explain itself — and everything above is the
writing that makes that possible.

*Document control: Wave 4 · six plans complete · HEAD 5be1a30a ·
proposal only · Annex U governs execution.*

*End of Part CXXI. End of W4-06. End of Wave 4.*

*The next word belongs to the foreman's packages.*
```