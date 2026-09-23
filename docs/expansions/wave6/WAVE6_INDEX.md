# ASHFALL — Expansion Wave 6 Index (Expansions 37–41)

**Status:** Design plans (pre-integration). Not claims, not authorizations.
**Date:** 2026-09-23
**Purpose:** Index and evidence summary for the five Wave 6 expansion bibles.

These documents investigate the live JSON data authority and Core systems, find
domains where content is thin or absent around a working seam, and propose
expansions that attach to existing owners. They are design bibles: they do not
claim paths, change code, or authorize implementation. Any implementation must
later pass through `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, and
`TEST_POLICY.md`.

---

## The wave theme: care at every scale

Waves 1–5 built the shelter outward: survival and culture, hazards and
infrastructure, crafts and food, the frontier and its cost. Wave 6 turns to the
smallest and most precise registers of care. It begins a life, heals a body,
transforms matter, makes a machine worth trusting, and gives tired people a
room to sleep in.

| Expansion | Live seam | Current content | Gap |
|---|---|---|---|
| 37 · The Quickening | `ChildDevelopmentSystem` (`ChildProfile`, `DevelopmentStage`, `RegisterChild`, `TickDay`, `AssignCaregiver`, `GetChoreWorkCapacity`), `CohortSystem` (`CohortChild` with `guessBand`/`trueBand` and `moralityMemory`), `GenerationalLineageExtension` (`LineageRecord`, `FamilyUnit`), `CaregivingSystem` (bond constants), `SurvivorRelationsSystem` | no maternity data at all | pregnancy care, birth, nursery, feeding, postpartum recovery, early years, sealed records |
| 38 · The Ward | `MedicalWardSystem` (beds, admissions, procedures), `AdvancedSurgicalWardSystem` (shock, anesthesia, milestones, complications), `MedicalProcedureSchedule`, `NarcoticsSystem`, `DiagnosisKnowledgeStore`, `MedicalReservationLedger` | `surgical_procedures.json` = **2,141 B** | triage, rooms, rosters, teams, trays, recovery, isolation, sterilization, charts |
| 39 · The Reagent | `ChlorAlkaliSynthesisEngine`, `PlasticPyrolysisSystem`, `FischerTropschSynthesisEngine`, `BioFermentationEngine`, `PharmaLabSystem` | 1.6–7 KB catalogs (`chlor_alkali` **1,617 B**) | feedstocks, catalysts, grades, storage, safety, spills, waste, notebook |
| 40 · The Wheel | `KineticStorageSystem`, `ShelterWorkshopSystem`, `PrecisionMetrologySystem`, `PrecisionBroachingCatalog`, `MachineIdentity` | `precision_broaching_catalog.json` = **1,739 B**, `metrology_standards_catalog.json` = 3,143 B | wheels, drivelines, gearing, belts, machine tools, lubrication, maintenance, mill work |
| 41 · The Quiet | `ShelterNoiseSystem` (sources, acoustic profiles, quiet hours 22–06, `OnNoiseSpike`, `OnThreatDetectionRiskIncreased`), `NeedsSystem` (Fatigue), `ShelterAssignmentSystem`, `ShelterSocialDynamicsSystem`, `CaregivingSystem` (`CaregiverFatigueDrain`) | no sleep or rest data at all | sleep schedules, rest rooms, soundproofing, quiet hours, crowding, sensory relief, night culture |

The strongest authority splits in the wave: children stay with the child
development owner and the dose band is read but never ranked; the ward adds no
second medical model and never replaces the bed, surgery, or schedule owners;
every chemical output has a mass balance and a waste stream; every turn of a
machine comes from water, wind, or muscle; and sleep writes only through the
live needs owner, with fatigue met by rosters rather than shame.

---

## The five plans

1. `expansion_37_the_quickening_plan.md` — 70,202 chars.
   Antenatal care, birth attendance, nurseries, infant feeding, postpartum
   recovery, early years, family records, and loss routed to live grief owners.
   Extends the child, lineage, caregiving, and relations systems.
2. `expansion_38_the_ward_plan.md` — 70,255 chars.
   Triage, ward rooms and beds, shifts, surgery teams and trays, recovery, iso-
   lation, sterilization, supplies, and honest charts. Extends the ward, surgery,
   and schedule owners.
3. `expansion_39_the_reagent_plan.md` — 70,419 chars.
   Feedstock sorting, catalysts, campaigns, grades, storage segregation, spills,
   waste neutralization, and the lab notebook. Extends the live synthesis engines
   and the pharma lab.
4. `expansion_40_the_wheel_plan.md` — 70,841 chars.
   Water wheels, windmills, drivelines, gearing, belts, machine tools, calibra-
   tion, lubrication, maintenance, and mill work. Extends the flywheel, workshop,
   and metrology owners.
5. `expansion_41_the_quiet_plan.md` — 70,335 chars.
   Sleep windows, rest rooms, soundproofing, quiet hours, privacy, crowding,
   sensory relief, and night culture. Extends the noise, needs, assignment, and
   social owners.

---

## Shared design constraints (all five)

- **Godot authoritative; Core engine-free.** No Godot or Unity reference in Core
  logic.
- **JSON data authoritative.** New content lives in snake_case catalogs with
  integer `schema_version`, validated by `CatalogIntegrityValidator` and
  registered with `ContentUtilizationScanner`.
- **One authority per concern.** Every plan contains a non-duplication statement
  and an integration-seam table. No second child, ward, chemistry, power,
  noise, needs, or save system is introduced.
- **Deterministic.** Live seeded paths only. Birth timing, surgery phases,
  reactions, power output, wear, and sleep recovery are deterministic functions
  of authored inputs and live state. Paired replay hashes must match.
- **Persistence.** New state is additive inside existing owners
  (`ChildDevelopmentState`, `CaregivingSaveState`, `MedicalWardSave`,
  `AdvancedSurgicalWardSave`, synthesis engine states, flywheel/workshop/
  metrology state, `ShelterNoiseState`, assignment state). Legacy saves load
  neutral; the Triad drift gate must pass.
- **Tone.** Restrained, human, fictional. No real institutions or events copied.
- **Verification.** Focused tests per `TEST_POLICY.md`, plus
  `--data-integrity-selftest` and `--content-utilization-selftest`. A
  compile-green result is not acceptance.

---

## Ethical and content contracts specific to Wave 6

| Plan | Hard contract |
|---|---|
| 37 Quickening | Birth is elided, never explicit; no dose ranking, no selection, no eugenics; family planning is private and unpressured; loss routes to grief owners with no reward; records are sealed by default with an access log |
| 38 Ward | No graphic surgery; no torture or experimentation; prisoners and outsiders receive identical care; charts are complete and privacy-sealed; errors are reviewed blamelessly and change procedures |
| 39 Reagent | No weapons, poisons, or explosives; hazard agents remain with 19's owners; every output has a real input and a waste stream; apprentices are 16+ and supervised |
| 40 Wheel | Every turn comes from water, wind, or muscle; no perpetual motion; every driveline is guarded; machines have mechanical quirks, never haunted ones; workers are 16+ and supervised |
| 41 Quiet | No shaming of sleep or needing quiet; records are clinical and private; isolated rooms can always be opened from inside; quiet hours are negotiated and reminded, never enforced by punishment; calm rooms are open space, not a diagnosis |

---

## Cross-wave hooks (summary)

- **Wave 6 × Wave 6:** the nursery and the ward share quiet and rest standards;
  the reagent supplies soap, antiseptic, grease, and salve to the ward, wheel,
  and clinic; the wheel machined the autoclave, the cradle, and the gauges; the
  quiet gives the watch, ward, and nursery a night roster that respects sleep.
- **Wave 6 × earlier waves:** the kiln fires warm stones and door fit; the
  thread supplies wraps, aprons, rugs, and belts; the press prints charts,
  labels, and records; the glass blows lamps, gauges, and carboys; the clean
  flow supplies sterile water and takes treated waste; the wild, farm, and
  kitchen supply milk, fat, herbs, and grain; the grid and the watch share
  nights; the long goodbye receives every loss with dignity.

Each plan is self-contained; none requires another to ship.

---

## How to promote a Wave 6 plan

1. Pick exactly one plan and one phase. Do not start two.
2. Re-audit the premise against current source and data; a plan is not proof an
   API or catalog still exists (`AGENTS.md` Rule 7).
3. Claim exact paths in `WORKTREE_OWNERSHIP.md`; confirm no overlap.
4. Add the package row to `INTEGRATION_PLANS.md` with owner, acceptance, and
   focused verification.
5. Implement data first, then pure Core, then persistence, then host/UI, then
   content.
6. Verify with focused tests and the data/content selftests; record limitations.
7. Update the live ledger only if you are the foreman or named integrator.

Until a foreman signature exists, these plans remain proposals. The safe
pre-signature work is Phase 1 (schemas and validators), which is additive and
reversible.

---

## Open decisions common to the wave

- Content volume budgets (each plan lists an authoring estimate; the full wave is
  roughly 300,000–330,000 words of new prose if all five are authored).
- Save placement (additive sub-objects versus sibling sections) per domain. Each
  plan recommends additive sub-objects.
- Whether any Wave 6 expansion adds a new headless selftest verb or extends an
  existing one. Each plan recommends extending existing verbs.
- Priority order. Recommended: 38 (Ward) first, because every other plan can
  send it a patient; 39 (Reagent) second, because soap, antiseptic, grease, and
  salve feed the ward, wheel, and clinic; 40 (Wheel) third, because four other
  plans already order machined parts; 37 (Quickening) fourth, because it depends
  on the ward and the calm room; 41 (Quiet) fifth and always, because a shelter
  that cannot rest cannot run any of the rest.

---

## Evidence anchors (file references used across the plans)

- `Assets/Ashfall.Core/Survivors/ChildDevelopmentSystem.cs` — `ChildProfile`
  (`BirthDay`, `Stage` = Infant, `AssignedCaregiverId`, `EducationScore`,
  `ChoreEfficiency`, `ParentIds`, `Milestones`), `ResolveStage`,
  `ResolveCanonicalAgeDays`, `ResolveCanonicalStage`, `ProjectCanonicalChild`,
  `RegisterChild`, `TickDay`, `RecordEducation`, `AssignCaregiver`,
  `GetChoreWorkCapacity`.
- `Assets/Ashfall.Core/Survivors/CohortSystem.cs` — `CohortChild` with
  `parentIds`, `guessBand`/`trueBand`, `baselineCorrected`, `moralityMemory`,
  `isMatured`, `maturationDay`, `isDeceased`.
- `Assets/Ashfall.Core/GenerationalLineageExtension.cs` — `LineageRecord`,
  `FamilyUnit`.
- `Assets/Ashfall.Core/Survivors/CaregivingSystem.cs` — `CaregivingSaveState`,
  `CaregivingAssignmentState`, `RecoverySpeedBonus` = 0.30f, `AffinityGainPerDay`
  = 5f, `CaregiverFatigueDrain` = 0.15f, `BondGrowthPerDay` = 0.02f.
- `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs` — `MedicalBed`,
  `Admit`, `Discharge`, `RunProcedure`, `GetBedOccupant`,
  `GetActiveAdmission`, `CaptureState`, `RestoreState`.
- `Assets/Ashfall.Core/Medical/AdvancedSurgicalWardSystem.cs` —
  `SurgicalOperationState` (`shock_percent`, `anesthesia_level`, milestone
  flags, `patient_survived`, `recovery_days_remaining`, `complications`).
- `Assets/Ashfall.Core/Shelter/ChlorAlkaliSynthesisEngine.cs`,
  `PlasticPyrolysisSystem.cs`, `FischerTropschSynthesisEngine.cs`,
  `BioFermentationEngine.cs`, `PharmaLabSystem.cs`.
- `Assets/Ashfall.Core/Shelter/KineticStorageSystem.cs`,
  `ShelterWorkshopSystem.cs`, `PrecisionMetrologySystem.cs`,
  `PrecisionBroachingCatalog.cs`, `MachineIdentity`.
- `Assets/Ashfall.Core/Shelter/ShelterNoiseSystem.cs` —
  `NoiseSourceType`, `NoiseFrequency`, `NoiseSource`, `RoomAcousticProfile`,
  `NoiseEvent`, `ShelterNoiseState` (`OverallNoiseLevel` = 20f,
  `QuietHoursStart` = 22, `QuietHoursEnd` = 6), `OnNoiseSpike`,
  `OnThreatDetectionRiskIncreased`, `OnQuietHoursChanged`.
- Data: `surgical_procedures.json` (2,141 B), `chlor_alkali_synthesis_catalog.json`
  (1,617 B), `precision_broaching_catalog.json` (1,739 B),
  `metrology_standards_catalog.json` (3,143 B), `bio_fermentation_catalog.json`
  (3,332 B), `plastic_pyrolysis_catalog.json` (3,464 B),
  `kinetic_flywheel_catalog.json` (5,984 B), `mineral_acid_synthesis_catalog.json`
  (4,677 B), `chemical_syntheses.json` (7,001 B), `workshop_recipes.json`
  (12,281 B), `shelter_machine_identities.json` (24,630 B),
  `disease_catalog.json` (54,638 B), `medical_texts.json` (222,244 B).
- `docs/expansions/wave1/WAVE1_INDEX.md` through
  `docs/expansions/wave5/WAVE5_INDEX.md`.
- `AGENTS.md`, `TEST_POLICY.md`, `WORKTREE_OWNERSHIP.md`,
  `INTEGRATION_PLANS.md`.