# ASHFALL Flagship Advanced Industrial & Expedition Systems Master Plan
## Plans 146–149 — EB-PVD Thermal Barrier Coatings × Mine-Clearing Flail × Microfluidic Diagnostics × Rail Grinding Corridors

**Plan ID:** AF-146-149-FLAGSHIP
**Status:** Major implementation-ready flagship master plan
**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Prepared:** 2026-09-02
**Scope:** Plans 146, 147, 148, 149
**Priority:** P1/P2 advanced late-game infrastructure integration
**Risk:** High
**Implementation doctrine:** deterministic Core simulation, data-driven catalogs, thin Godot presentation, single-authority state ownership, atomic inventory transactions, versioned persistence, content utilization proof, headless integration verification

---

# 0. Executive Summary

Plans 146–149 are not four isolated “advanced machine” additions. Together they form a late-game technological progression layer in which the shelter can convert recovered industrial knowledge into:

1. **high-temperature component protection** for power-generation and vehicle machinery;
2. **route-clearing capability** that changes the topology and risk of expedition travel;
3. **rapid field/shelter diagnostic capability** that changes the timing and certainty of medical decisions;
4. **persistent railway rehabilitation** that creates strategic high-speed logistics corridors.

The implementation must therefore close four loops:

```text
INDUSTRIAL INPUTS
    ↓
ADVANCED MACHINE PROCESS
    ↓
PERSISTENT COMPONENT / ROUTE / DIAGNOSTIC STATE
    ↓
REAL DOWNSTREAM AUTHORITY
    ↓
CAMPAIGN CONSEQUENCE
    ↓
SAVE / RESTORE / DETERMINISTIC REPLAY
```

The flagship objective is not “the panel animates and the numbers move.” It is:

> Every advanced technology must consume canonical items, require canonical infrastructure, produce a saveable state mutation, affect an already-live gameplay authority, survive save/reload, remain deterministic, surface through UI without owning rules, and be provably reachable through authored content.

---

# 1. Repository Forensic Audit — Verified Starting State

## 1.1 Plan 146 prerequisite names are not all live under the supplied identities

The task references:

```text
VacuumInductionMeltingEngine.cs
RefractoryCeramicsEngine.cs
FoundryProductionSystem.cs
```

Those exact names did not resolve on the audited default branch.

The live industrial authority is instead centered on:

```text
Assets/Ashfall.Core/Foundry/SilentFoundrySystem.cs
Assets/Ashfall.Core/Foundry/SilentFoundrySystem.Heat.cs
Assets/Ashfall.Core/Foundry/SilentFoundrySystem.TreatyLabor.cs
Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs
```

The implementation must therefore perform a **dependency reconciliation pass** before adding EB-PVD.

Do not create duplicate metallurgy systems simply to preserve an outdated plan filename.

## 1.2 Silent Foundry is the live production authority

`SilentFoundrySystem` is already a production-grade Core system with:

- heat/labor mechanics;
- product data;
- production output;
- treaty/labor extensions;
- journal hooks;
- a host action surface.

Plan 146 should consume this system as a supplier and/or downstream component producer.

## 1.3 Power-grid authority is live and typed

`PowerGridSystem` exists under:

```text
Assets/Ashfall.Core/Shelter/PowerGridSystem.cs
```

It owns typed power-grid state and emits typed events.

The host authority is:

```text
src/Host/PowerGridHostSession.cs
```

Do not create a separate generator-condition ledger inside EB-PVD.

Plan 146 may contribute a bounded modifier/projection to generator capability only through an explicit PowerGrid integration contract.

## 1.4 Expedition travel is already vehicle-aware

The live `ExpeditionSystem` has:

```text
ExpeditionVehicleProfile
vehicleId
vehicleSpeedMultiplier
vehicleBreakdownChancePerTick
```

and retains deterministic travel through host-owned seeded RNG.

This is the natural integration seam for Plans 147 and 149.

## 1.5 Vehicle ownership already has a dedicated system

The repository contains:

```text
Assets/Ashfall.Core/ExpeditionVehicleSystem.cs
Assets/StreamingAssets/Data/vehicles.json
```

Plan 147’s mine flail and Plan 149’s rail-grinding trolley should become **vehicle modules / operational capabilities**, not independent expedition vehicles unless the catalog explicitly defines them as separate vehicles.

## 1.6 Minefield art exists, but a live minefield gameplay authority was not verified

The repository contains `enc_minefield` and `enc_minefield_remnant` visual assets, but the audit did not establish a canonical minefield-state system.

Therefore Plan 147 must first determine whether minefields are:

- authored encounter definitions;
- route hazards;
- location hazards;
- combat hazards;
- or only visual/content remnants.

Do not make `MineClearingFlailEngine` clear an invented shadow list of coordinates.

## 1.7 Rail narrative content exists, but a rail-network mechanics authority was not verified

The repository contains authored railway/draisine flavor such as the Iron Line Express and hand-pumped rail routes, but the task-proposed:

```text
DraisineTransmissionEngine
RailwayInterlockEngine
```

did not resolve under those exact names.

Plan 149 therefore requires a **rail authority bootstrap** unless a newer branch has already implemented one.

## 1.8 The old monolithic `MedicalSystem` is not the production authority

Repository guidance explicitly records the old large `_Game/Medical/MedicalSystem.cs` as deleted/resolved.

The live medical architecture includes:

```text
Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs
src/Host/MedicalHostSession.cs
Assets/Ashfall.Core/Disease/DiseaseSystem.cs
SickListSystem
DiagnosisKnowledgeStore
MedicalWardSystem
```

Plan 148 must integrate with this pipeline.

## 1.9 Diagnosis already has an authority boundary

Medical architecture documentation establishes that diagnosis is not a free reveal of hidden disease state.

Diagnosis is gated through the sickness/diagnostic authority.

The microfluidic system must therefore provide **diagnostic evidence/certainty**, not bypass disease progression by directly reading hidden infection state in UI.

## 1.10 Wasteland map is the canonical persistent world topology authority

Route and node consequences must ultimately integrate with:

```text
Assets/Ashfall.Core/World/WastelandMapSystem.cs
```

but Plan 149 requires a separate edge-quality layer because the base map system does not currently own persistent per-rail-segment roughness and speed limits.

---

# 2. Architectural Doctrine

## 2.1 One authority per fact

| Fact | Canonical owner |
|---|---|
| Foundry products | SilentFoundrySystem |
| EB-PVD machine process | EbPvdCoatingEngine |
| Generator power state | PowerGridSystem |
| Vehicle ownership / modules | ExpeditionVehicleSystem |
| Active expedition travel | ExpeditionSystem |
| Minefield hazard state | existing hazard authority if found; otherwise new MinefieldRouteState |
| Rail route condition | new RailInfrastructureState if none exists |
| Infection progression | DiseaseSystem |
| Diagnosis knowledge | medical diagnostic pipeline / SickList |
| Microfluidic cartridge manufacturing | MicrofluidicDiagnosticEngine |
| Inventory | existing inventory authority |
| Survivor traits/skills | canonical survivor progression authority |
| Map nodes/routes | WastelandMapSystem |
| Save envelope | existing campaign save orchestration |

No system introduced here may persist another authority’s state merely for convenience.

## 2.2 Core owns rules; host owns wiring; Godot owns presentation

Every new system follows:

```text
Assets/Ashfall.Core/
    simulation
    DTOs
    pure validation
    deterministic outcomes

src/Host/
    catalog loading
    inventory transactions
    cross-system wiring
    save-store integration

src/UI/
    panels
    read models
    user commands
    animation
```

Panels must never contain process equations, risk rolls, item mutations, or save logic.

## 2.3 Advanced-tech systems require infrastructure gates

Each technology must depend on real shelter capability.

Example:

```text
EB-PVD
requires:
  high-power electrical allocation
  high vacuum capability
  ceramic target
  bond-coat consumable
  valid substrate
  trained operator or penalty

Microfluidic cartridge press
requires:
  clean manufacturing station
  consumables
  assay definition
  reader
```

Avoid “late-game machine exists because a panel was opened.”

## 2.4 All modifiers are bounded projections

No downstream system receives absolute replacements.

Examples:

```text
PowerGrid:
base generator efficiency
× coating efficiency modifier

Expedition:
base route hazard
× mine-clearing residual risk

Rail:
base vehicle speed
× route condition multiplier

Diagnosis:
base clinical confidence
+ assay evidence confidence
```

Clamp each result at the consumer boundary.

## 2.5 No unsafe real-world fabrication dependency

These are game systems.

Catalogs may contain gameplay parameters inspired by engineering, but implementation documentation must not become a real-world fabrication manual for:

- electron-beam high-voltage hardware;
- explosive breaching devices;
- pathogen culturing/amplification workflows.

Model them at simulation abstraction level.

---

# 3. Program Dependency Graph

```text
                ┌────────────────────────────┐
                │ Silent Foundry / Inventory │
                └─────────────┬──────────────┘
                              │
                 advanced inputs/components
                              │
          ┌───────────────────┴───────────────────┐
          │                                       │
          ▼                                       ▼
 ┌─────────────────┐                    ┌─────────────────────┐
 │ Plan 146 EB-PVD │                    │ Plan 148 Microfluidic│
 └───────┬─────────┘                    └─────────┬───────────┘
         │                                        │
 coated components                       diagnostic evidence
         │                                        │
         ▼                                        ▼
 ┌─────────────────┐                    ┌─────────────────────┐
 │ Power / Vehicle │                    │ Medical / Disease    │
 └─────────────────┘                    └─────────────────────┘

     Vehicle capability / infrastructure progression
                         │
          ┌──────────────┴──────────────┐
          ▼                             ▼
 ┌────────────────────┐        ┌─────────────────────┐
 │ Plan 147 Mine Flail│        │ Plan 149 Rail Grind │
 └─────────┬──────────┘        └─────────┬───────────┘
           │                             │
 route hazard clearance             rail condition
           │                             │
           └──────────────┬──────────────┘
                          ▼
               ┌─────────────────────┐
               │ Expedition / Map    │
               └─────────────────────┘
```

---

# 4. Implementation Order

Implement in this order:

1. **Phase A — Global forensic readiness**
2. **Phase B — shared advanced-machine contracts**
3. **Phase C — Plan 146 EB-PVD**
4. **Phase D — Plan 148 microfluidic diagnostics**
5. **Phase E — route infrastructure authority**
6. **Phase F — Plan 147 mine-clearing flail**
7. **Phase G — Plan 149 rail grinding**
8. **Phase H — UI / accessibility / localization**
9. **Phase I — persistence / migrations**
10. **Phase J — deterministic integration tests**
11. **Phase K — content-utilization and closeout**

This order prevents Plans 147 and 149 from inventing incompatible route-state storage independently.

---

# 5. Phase A — Global Forensic Readiness

## A1. Baseline

Record:

```text
git rev-parse HEAD
git status --short
dotnet --version
godot --version
```

Run:

```bash
dotnet build Ashfall.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
python3 scripts/ci/run-gates.py --tier fast
```

Capture baseline test count and warnings.

## A2. Audit proposed dependencies

Verify existence / replacement for:

```text
VacuumInductionMeltingEngine
RefractoryCeramicsEngine
FoundryProductionSystem
GroundPenetratingRadarEngine
DraisineTransmissionEngine
RailwayInterlockEngine
LyophilizationEngine
MedicalSystem
```

For each absent name, document:

```text
proposed_name
status
live_replacement
reason
migration_required
```

Create:

```text
docs/architecture/PLANS_146_149_AUTHORITY_AUDIT.md
```

## A3. Audit data authority

Search:

```text
items.json
recipes.json
vehicles.json
wasteland_map*.json
locations.json
travel_encounters.json
narrative_encounters*.json
skills.json
traits catalogs
disease catalogs
foundry_production.json
```

Confirm ID schemas before authoring new IDs.

## A4. Audit save architecture

Locate canonical registration path for every `*SaveStore`.

All four new sections must use the same versioned atomic campaign envelope model.

No direct sidecar save file writes outside orchestration.

## A5. Audit player-command pattern

Use the repository’s preview/execute contract where applicable.

New commands should follow:

```text
Preview
→ validate
→ projected costs/risks
→ Execute
→ stale-state check
→ atomic mutation
→ typed event
```

## A6. Create shared ID namespaces

Suggested:

```text
ebpvd_*
mine_flail_*
microfluidic_*
rail_grinding_*
rail_sector_*
minefield_*
```

Item IDs retain existing `item_` conventions.

---

# 6. Phase B — Shared Advanced Machine Contracts

## B1. Introduce a common machine-operation shape only if useful

Do **not** build a giant generic machine framework.

A small reusable contract is acceptable:

```csharp
public interface IAdvancedMachineOperation
{
    bool IsOperational { get; }
    float Condition01 { get; }
    string ActiveJobId { get; }
}
```

Only add it if at least two systems actually use it without semantic distortion.

## B2. Shared operator projection

Define a read-only host projection:

```text
AdvancedMachineOperatorContext
survivorId
relevantSkillProgress01
traitIds
fatigueModifier
injuryModifier
```

The Core engine receives already-resolved numeric capability.

It should not instantiate skill systems.

## B3. Shared power projection

Machine engines receive:

```text
availablePowerKw
powerStable
brownoutSeverity
```

from PowerGrid.

They never own grid state.

## B4. Shared inventory transaction pattern

Job start must atomically:

1. validate all required inputs;
2. validate machine state;
3. validate power/infrastructure;
4. quote consumption;
5. consume inputs;
6. create job state;
7. emit event.

If any validation fails, consume nothing.

## B5. Shared process state invariants

Every advanced process has:

```text
Idle
Queued
Running
Paused
Completed
Failed
MaintenanceRequired
```

Do not overload bools such as:

```text
isRunning
isComplete
isBroken
```

into contradictory combinations.

---

# 7. PLAN 146 — EB-PVD Thermal Barrier Coating System

# 7.1 Goal

Create a late-game coating process that converts:

- validated superalloy substrates;
- ceramic targets;
- bond-coat consumables;
- power;
- machine time;
- operator capability;

into a persistent **coated-component record** that downstream generator/vehicle systems can consume.

The system should feel technologically advanced without becoming a physics research simulator.

---

# 7.2 Catalog

Create:

```text
Assets/StreamingAssets/Data/ebpvd_coating_catalog.json
```

Recommended schema:

```json
{
  "schema_version": 1,
  "machine": {
    "min_voltage_kv": 15.0,
    "max_voltage_kv": 25.0,
    "nominal_power_kw": 10.0,
    "target_vacuum_mbar": 0.00001
  },
  "coatings": [],
  "substrate_classes": [],
  "failure_profiles": [],
  "maintenance": {}
}
```

Each coating definition:

```text
id
display_name_key
ceramic_target_item_id
bond_coat_item_id
valid_substrate_tags[]
target_thickness_um
min_substrate_temperature_c
nominal_rotation_rpm
nominal_raster_hz
base_duration_hours
base_power_kwh
thermal_resistance_bonus
max_temperature_bonus_c
durability_bonus
spallation_base_risk
required_skill
tags[]
```

Do not make 400°C and +45% global hard-coded facts.

Represent them as tunable authored values with balance caps.

---

# 7.3 State DTO

Create:

```text
Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs
```

State:

```text
systemId
machineCondition01
operatingHours
filamentHours
vacuumPumpHours
chamberShieldingCondition01
activeJob
completedComponentIds[]
maintenanceFlags[]
```

Active job:

```text
jobId
substrateInstanceId
coatingId
operatorId
startedDay
progressHours
requiredHours
beamVoltageKv
beamCurrentA
beamPowerKw
vacuumMbar
substrateRotationRpm
rasterFrequencyHz
coatingThicknessUm
bondCoatApplied
status
failureCode
```

## 7.4 Coated component record

Do not represent a coated part as only a new generic inventory item if instance state matters.

Prefer:

```text
ComponentModification
instanceId
modificationId
sourceProcessId
appliedDay
condition01
```

or the existing equipment modifier pattern if present.

For purely fungible components, an authored result item is acceptable.

---

# 7.5 Process model

Use bounded gameplay equations.

### Beam power

```text
P_kW = voltage_kV × current_A
```

Unit tests pin dimensional conversion.

### Deposition progress

```text
depositionRate =
    authoredBaseRate
    × vacuumQuality
    × rasterUniformity
    × substrateRotationUniformity
    × operatorModifier
    × machineCondition
```

### Thickness

```text
thickness += depositionRate × dt
```

### Uniformity

```text
uniformity01 =
    clamp(
      rasterFactor
      × rotationFactor
      × operatorFactor
      × machineCondition,
      0, 1)
```

### Spallation risk

```text
risk =
    baseSpallation
    + poorBondCoatPenalty
    + poorUniformityPenalty
    + contaminationPenalty
```

Clamp 0..1.

Use seeded RNG only when the job resolves.

---

# 7.6 Bond coat / TGO abstraction

Model:

```text
Unprepared
BondCoated
OxideScaleEstablished
Coated
```

Do not simulate chemical kinetics at lab granularity.

Mechanical consequence:

```text
BondCoated
→ lower spallation probability
→ higher thermal-cycle durability
```

---

# 7.7 Machine hazards

Hazards are simulation state, not real-world operating instructions.

Possible authored failure kinds:

```text
filament_blowout
vacuum_loss
substrate_overheat
coating_nonuniform
shielding_fault
pump_maintenance_due
```

### X-ray hazard abstraction

The process requires:

```text
shieldingIntegrity >= minimum
```

If below threshold:

- machine cannot start;
- or host marks operation unsafe.

Do not implement actionable shielding-construction calculations.

---

# 7.8 Trait and skill integration

The supplied traits:

```text
EbPvdSpecialist
SurfaceCoatingsEngineer
```

must first be verified against canonical trait/skill catalogs.

If absent, either:

- author them through the established skill content pipeline;
- or use an existing materials/science/mechanical skill.

Avoid hardcoding trait-name checks inside engine equations.

Preferred host projection:

```text
operatorUniformityBonus
operatorFailureReduction
operatorMaintenanceBonus
```

---

# 7.9 Foundry integration

Silent Foundry should produce or supply:

```text
superalloy component blanks
coating-compatible parts
ceramic-target precursor items
```

EB-PVD should not duplicate foundry recipes.

Add explicit tag contract:

```text
substrate_ebpvd_compatible
```

to eligible outputs.

---

# 7.10 Power-grid integration

EB-PVD is a significant electrical load.

Expose a room/device load to PowerGrid:

```text
ebpvd_coater
power_kw
priority
can_pause
```

Brownout behavior:

```text
stable power → normal progress
minor brownout → slower/quality penalty
severe brownout → auto-pause
power collapse → fail-safe stop
```

No progress while unpowered.

---

# 7.11 Downstream coating effects

## Generator components

Do not mutate global generator efficiency permanently from “a coating exists.”

Require installation:

```text
coated component produced
→ component installed into generator
→ PowerGrid receives installed upgrade projection
```

Possible benefits:

```text
thermal efficiency modifier
overheat tolerance
wear reduction
warp-risk reduction
```

Cap benefits.

## Vehicle engines

Integrate through `ExpeditionVehicleSystem` module/upgrade state.

Possible:

```text
heatToleranceModifier
breakdownChanceModifier
fuelEfficiencyModifier
```

No duplicate vehicle state in EB-PVD save.

---

# 7.12 UI

Create:

```text
src/UI/EbPvdCoatingPanel.cs
```

Presentation:

- machine status;
- vacuum gauge;
- voltage/current readout;
- beam-power readout;
- substrate carousel;
- thickness progress;
- uniformity indicator;
- maintenance counters;
- input requirements;
- output preview;
- risk text.

The “beam raster visualizer” is cosmetic.

It does not drive simulation timestep.

## Accessibility

- numeric labels accompany gauges;
- warning icons + text;
- no color-only hazard states;
- keyboard navigation;
- reduced-motion fallback for raster animation.

---

# 7.13 Items

Verify IDs and author as needed:

```text
item_ebpvd_ceramic_target_ingot
item_electron_gun_tungsten_filament
item_mcraly_bond_coat_powder
```

Also consider:

```text
item_ebpvd_vacuum_pump_seal
item_ebpvd_chamber_shield_panel
```

only if they create meaningful maintenance gameplay.

---

# 7.14 Persistence

Create:

```text
src/Host/EbPvdCoatingSaveStore.cs
```

Persist:

- machine condition;
- operating hours;
- filament hours;
- vacuum-pump hours;
- active job;
- completed machine-owned records;
- maintenance flags.

Do not duplicate installed generator/vehicle upgrade state.

---

# 7.15 Plan 146 tests

### Catalog
- IDs unique;
- item references valid;
- coating thickness > 0;
- process power > 0;
- valid substrate tags.

### Math
- beam power conversion;
- deposition monotonicity;
- vacuum quality bounds;
- thickness convergence;
- uniformity clamp;
- failure probability clamp.

### Workflow
- start consumes all inputs atomically;
- insufficient input consumes nothing;
- no power → cannot run;
- brownout behavior;
- completion produces one output;
- restore does not duplicate output;
- maintenance due blocks appropriately.

### Integration
- foundry substrate accepted;
- invalid substrate rejected;
- installed coated generator component changes only intended PowerGrid projection;
- coated vehicle component changes only intended vehicle projection.

### Determinism
- same state + same seed → same failure/completion result.

---

# 8. PLAN 148 — Microfluidic Diagnostic Cartridge System

# 8.1 Goal

Create a late-game medical manufacturing and diagnosis support system that:

1. manufactures diagnostic cartridges;
2. consumes assay-specific reagents;
3. accepts a patient sample abstractly;
4. produces bounded diagnostic evidence;
5. integrates with canonical diagnosis knowledge;
6. helps detect disease earlier without directly revealing hidden disease state.

---

# 8.2 Medical authority decision

Do not implement against a new monolithic `MedicalSystem`.

Use:

```text
MedicalPipelineCoordinator
DiseaseSystem
SickListSystem
DiagnosisKnowledgeStore
MedicalWardSystem
```

according to existing ownership.

The microfluidic engine is a **diagnostic instrument**, not a new disease engine.

---

# 8.3 Catalog

Create:

```text
Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json
```

Schema:

```text
schema_version

assays[]
  id
  display_name_key
  target_disease_ids[]
  cartridge_item_id
  reagent_item_ids[]
  reader_item_id
  base_duration_minutes
  sensitivity
  specificity
  early_detection_modifier
  invalid_run_base_chance
  minimum_operator_skill
  tags[]
```

The supplied eight assay targets must map to **existing canonical disease IDs**.

If some are not present, do not silently invent diseases as part of this plan.

Either:

- author them through the disease-content pipeline;
- or reduce the initial assay set to existing disease targets and document the dependency.

---

# 8.4 Avoid operational wet-lab simulation

The game may represent:

- channel quality;
- capillary fill;
- reagent status;
- reader intensity;
- assay confidence.

Do not implement real pathogen handling/culture protocols.

The simulation layer should remain abstract:

```text
sample_quality
cartridge_quality
operator_modifier
disease_stage_modifier
assay sensitivity/specificity
```

---

# 8.5 State

Create:

```text
Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs
```

State:

```text
systemId
machineCondition01
masterMoldCondition01
cartridgesManufactured
activeManufacturingJobs[]
activeDiagnosticRuns[]
completedRunIds[]
maintenanceFlags[]
```

Diagnostic run:

```text
runId
patientId
assayId
cartridgeInstanceId
operatorId
startedDay
startedMinute
progressMinutes
sampleQuality01
capillaryFill01
reagentReconstituted
readerIntensity01
status
resultKind
confidence01
invalidReason
```

---

# 8.6 Manufacturing model

Cartridge manufacturing is a shelter production process.

Inputs:

```text
PDMS/silicone kit
assay reagent pack
clean consumables
machine time
power
operator
```

Output:

```text
assay-specific diagnostic cartridge
```

Do not treat one generic cartridge as magically able to identify every disease unless that is explicitly authored as a multiplex cartridge.

---

# 8.7 Failure abstraction

Failure types:

```text
bubble_clog
poor_bond
reagent_failure
reader_alignment_error
sample_insufficient
invalid_control
```

These should result from:

- cartridge quality;
- machine condition;
- operator modifier;
- authored base failure chance.

No real procedural troubleshooting instructions.

---

# 8.8 Diagnostic evidence model

Use standard game-level probabilistic evidence.

Inputs:

```text
actual disease state (Core-internal)
assay sensitivity
assay specificity
disease stage detectability
sample quality
cartridge quality
operator modifier
```

Output:

```text
Positive
Negative
Indeterminate
Invalid
```

with confidence.

The panel receives only the result DTO.

It must never inspect `DiseaseSystem` hidden state directly.

---

# 8.9 Early-incubation capability

The technology’s value should be earlier evidence, not omniscience.

Model:

```text
pre-symptomatic stage
→ lower baseline detectability
→ assay-specific early-detection bonus
```

Even advanced testing can yield:

```text
false negative
indeterminate
invalid
```

within authored rates.

---

# 8.10 Diagnosis handoff

On a valid run:

```text
MicrofluidicDiagnosticResult
        ↓
MedicalPipelineCoordinator
        ↓
diagnostic evidence store
        ↓
SickListSystem diagnosis availability/confidence
```

If current diagnosis contracts cannot accept evidence, add a narrow API such as:

```text
RecordDiagnosticEvidence(patientId, diseaseId, evidence)
```

Do not bypass it with:

```text
SetDiagnosed(true)
```

unless that is already canonical behavior.

---

# 8.11 Disease spread consequence

Earlier diagnosis may enable:

- earlier isolation;
- earlier treatment;
- lower outbreak exposure.

The microfluidic engine itself does not reduce spread.

It supplies evidence; the medical/disease systems enact quarantine/treatment.

---

# 8.12 Traits and skills

Verify:

```text
MicrofluidicsEngineer
Epidemiologist
```

If absent, map to canonical:

- science;
- medicine;
- diagnostics;

through progression APIs.

Benefits:

```text
lower invalid-run chance
better sample interpretation
slightly higher confidence
faster cartridge manufacture
```

Do not let skill turn specificity/sensitivity to 100%.

---

# 8.13 UI

Create:

```text
src/UI/MicrofluidicDiagnosticPanel.cs
```

Surfaces:

- available assays;
- patient selector;
- required cartridge;
- sample status;
- run progress;
- capillary flow visualization;
- control validity;
- reader signal;
- result;
- confidence band;
- recommended next clinical action.

Accessibility:

- result written in text;
- no green/red-only result;
- uncertainty explicitly stated;
- keyboard accessible;
- animation can be reduced.

---

# 8.14 Items

Verify/author:

```text
item_pdms_silicone_elastomer_kit
item_isothermal_lamp_pathogen_primer_set
item_handheld_fluorescence_chip_reader
```

Prefer assay-specific reagent IDs if the disease catalog warrants it.

Do not make an unlimited universal primer item if that undermines scarcity.

---

# 8.15 Persistence

Create:

```text
src/Host/MicrofluidicDiagnosticSaveStore.cs
```

Persist:

- manufacturing state;
- master-mold condition;
- machine condition;
- active runs;
- run history IDs / diagnostic evidence references.

Do not duplicate the patient infection truth from DiseaseSystem.

---

# 8.16 Plan 148 tests

### Catalog
- assay IDs unique;
- target disease IDs valid;
- sensitivity/specificity 0..1;
- durations positive;
- item references valid.

### Process
- atomic manufacturing costs;
- cartridge output exactly once;
- invalid cartridge cannot diagnose;
- reader requirement enforced;
- machine condition influences failure only through defined path.

### Diagnostic
- positive disease state produces authored sensitivity across deterministic seeds;
- disease absent respects specificity;
- early stage has lower detectability;
- indeterminate path works;
- invalid control does not mutate diagnosis knowledge.

### Integration
- valid result records evidence;
- UI cannot access hidden infection state;
- quarantine/treatment remain separate actions;
- save/reload retains evidence without re-running assay.

### Determinism
- paired seed replay matches run result and confidence.

---

# 9. Phase E — Route Infrastructure Authority

Plans 147 and 149 both mutate persistent travel infrastructure.

Implement one shared route-state authority before either specialized engine.

## 9.1 Create `RouteInfrastructureSystem`

Recommended path:

```text
Assets/Ashfall.Core/World/RouteInfrastructureSystem.cs
```

State by route/segment:

```text
routeId
segmentId
mode
hazardFlags
minefieldState
railCondition
clearanceState
speedLimitKph
roughnessIndex
maintenanceFlags
lastModifiedDay
```

## 9.2 Do not duplicate WastelandMap routes

`WastelandMapSystem` remains topology authority.

`RouteInfrastructureSystem` references route IDs.

It adds mutable condition.

## 9.3 Route modes

```text
road
rail
subway
service_tunnel
offroad
```

Only compatible modes accept relevant operations.

Mine flail:

```text
road
offroad
selected fortified approaches
```

Rail grinder:

```text
rail
subway rail
```

## 9.4 Route query

Expose:

```text
GetTravelModifier(routeId, vehicleCapabilities)
GetHazardModifier(routeId, vehicleCapabilities)
GetSpeedLimit(routeId)
CanTraverse(routeId, vehicleCapabilities)
```

Expedition host projects route effects into `ExpeditionSystem`.

## 9.5 Save

One route-infrastructure save section owns:

- cleared minefields;
- remaining mine density;
- rail roughness;
- rail grinding completion;
- upgraded speed limits.

This avoids two unrelated coordinate ledgers.

---

# 10. PLAN 147 — Mine-Clearing Flail Vehicle Module

# 10.1 Goal

Create a heavy expedition vehicle module that converts dangerous minefield route segments into persistent cleared or reduced-hazard corridors at significant cost, wear, time, and vehicle risk.

This is a strategic breaching capability.

It is not an instant “minefields disabled” toggle.

---

# 10.2 Catalog

Create:

```text
Assets/StreamingAssets/Data/mine_flail_catalog.json
```

Schema:

```text
modules[]
  id
  display_name_key
  compatible_vehicle_tags[]
  nominal_rpm
  rpm_min
  rpm_max
  cleared_width_m
  nominal_breach_speed_kph
  chain_link_capacity
  chain_wear_per_km
  hydraulic_pressure_nominal
  blast_shield_integrity
  mine_clearance_efficiency
  tripwire_clearance_efficiency
  obstacle_stall_risk
  fuel_multiplier
  required_items[]
  tags[]
```

Physical numbers are gameplay tuning, not fabrication guidance.

---

# 10.3 State

Create:

```text
Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs
```

State is associated with a vehicle/module instance:

```text
moduleInstanceId
vehicleId
moduleDefinitionId
condition01
drumRpm
hydraulicPressure
chainLinksRemaining
blastShieldIntegrity01
totalClearedDistanceKm
maintenanceState
activeBreach
```

Active breach:

```text
routeId
segmentId
progressMeters
targetMeters
mineDensity01
residualMineRisk01
startedDay
operatorId
status
```

---

# 10.4 Minefield authority

Before implementation, identify minefield source.

Preferred data:

```text
RouteInfrastructureState.minefield
```

Fields:

```text
density01
typeTags[]
detected
clearedFraction01
lastClearedDay
```

Do not use raw world coordinates unless existing map system is coordinate-authoritative.

---

# 10.5 Breach operation

Start requires:

- compatible vehicle;
- installed flail module;
- sufficient chain links;
- hydraulic system operational;
- blast shield above minimum;
- fuel;
- minefield route segment.

Progress:

```text
metersClearedPerTick =
    baseBreachSpeed
    × chainCondition
    × hydraulicCondition
    × operatorModifier
    × terrainModifier
```

Minefield clearance:

```text
clearedFraction += progress / segmentLength
```

Residual risk:

```text
mineRisk =
    baseDensity
    × (1 - clearanceEfficiency × clearedFraction)
```

Clamp to a minimum residual risk unless the authored module allows certified full clearance.

---

# 10.6 Route corridor consequence

Once threshold reached:

```text
route segment:
Impassable / Catastrophic
→ Breached / High residual risk
→ Cleared / Standard residual risk
```

Following expeditions consume this shared route state.

Do not grant immunity merely because the player owns the flail.

---

# 10.7 Encounter integration

If minefields exist as travel encounters:

- route clearance should suppress or reweight those encounters;
- do not delete encounter definitions;
- record that the route no longer satisfies eligibility.

If minefields exist as route hazard multipliers:

- modify the route hazard directly.

Whichever model is live becomes canonical.

---

# 10.8 Barbed wire/tripwire abstraction

Represent as route obstacle tags:

```text
wire_entanglement
tripwire_field
light_barricade
```

Flail operation can clear eligible tags.

Do not model real explosive fuze mechanics.

---

# 10.9 Failure model

Possible failures:

```text
chain_link_snap
drum_stall
hydraulic_overheat
shield_damage
radiator_damage
mobility_kill
```

A failure should cause:

- resource cost;
- time loss;
- degraded clearance rate;
- possible route retreat.

It should not silently destroy expedition state.

---

# 10.10 Vehicle integration

Use `ExpeditionVehicleSystem` to install module.

Add capability tags:

```text
cap_mine_flail
cap_route_breach
```

Expedition profile may receive:

```text
breachCapability
mineHazardModifier
fuelMultiplier
```

through host projection.

---

# 10.11 Traits/skills

Verify:

```text
CombatPioneer
FlailOperator
```

If absent, use existing:

- combat;
- mechanical;
- survival;

skill projection.

Benefits remain bounded.

---

# 10.12 UI

Create:

```text
src/UI/MineFlailPanel.cs
```

Shows:

- installed vehicle;
- route target;
- minefield state;
- RPM;
- hydraulic state;
- chain count;
- shield integrity;
- progress;
- residual risk;
- estimated consumables;
- start/stop/repair.

Animation is cosmetic.

---

# 10.13 Items

Verify/author:

```text
item_hardened_steel_flail_chain_link
item_heavy_hydraulic_flail_drive_motor
item_angled_v_hull_blast_shield
```

Prefer component recipes through foundry/crafting.

---

# 10.14 Persistence

Module state belongs with vehicle-module or dedicated flail save.

Cleared route state belongs **only** in RouteInfrastructure save.

Do not store the same cleared sector in both.

---

# 10.15 Plan 147 tests

### Catalog
- module IDs unique;
- speed/rpm ranges valid;
- item refs valid;
- compatible tags valid.

### Workflow
- cannot breach without compatible vehicle;
- cannot breach non-mine segment;
- consumes fuel/chain wear;
- progress monotonic;
- stall failure deterministic.

### Route
- clearing reduces mine risk;
- other routes unchanged;
- route remains changed after save/reload;
- repeated command does not duplicate completion reward.

### Expedition
- expedition using cleared route gets correct hazard modifier;
- expedition using uncleared route retains baseline risk;
- ownership without active/cleared route grants no global immunity.

### Determinism
- same seed → same failure sequence.

---

# 11. PLAN 149 — Rail Grinding & Strategic Corridor Rehabilitation

# 11.1 Goal

Create a persistent rail-maintenance system in which armored draisine crews can rehabilitate damaged rail segments, reducing roughness, raising safe route speed, and creating strategic express corridors at ongoing abrasive, water, power/fuel, labor, and maintenance cost.

---

# 11.2 Rail-network prerequisite

The task-proposed `DraisineTransmissionEngine` and `RailwayInterlockEngine` were not verified.

Before coding the grinder, establish:

```text
Which map routes are rail?
Which vehicles can use them?
Where is rail speed currently derived?
How are route restrictions persisted?
```

If no rail mechanics authority exists, add the minimum route-mode and condition extension to `RouteInfrastructureSystem`.

---

# 11.3 Catalog

Create:

```text
Assets/StreamingAssets/Data/rail_grinding_catalog.json
```

Schema:

```text
rail_profiles[]
  id
  route_mode
  nominal_profile
  roughness_good_threshold
  roughness_bad_threshold

grinding_heads[]
  id
  compatible_vehicle_tags[]
  nominal_rpm
  min_rpm
  max_rpm
  pass_depth_mm
  base_work_rate_km_per_hour
  stone_wear_per_km
  water_suppression_per_km
  target_roughness
  max_speed_upgrade_kph
  spark_hazard_base
  stone_shatter_base
  required_items[]
```

---

# 11.4 Route rail condition

Extend route infrastructure:

```text
RailSegmentCondition
roughnessIndex
profileError01
surfaceDefect01
rustScale01
safeSpeedLimitKph
groundFraction01
lastGroundDay
```

Use a normalized gameplay roughness index internally.

If authored engineering units are stored, convert at catalog boundary.

---

# 11.5 Grinding engine

Create:

```text
Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs
```

State:

```text
moduleInstanceId
vehicleId
grindingHeadId
condition01
motorRpm
downforceBar
stoneDiameterMm
waterSuppressorCondition01
maintenanceState
activeJob
```

Job:

```text
routeId
segmentId
operatorId
progressKm
targetKm
passesCompleted
currentRoughness
status
failureCode
```

---

# 11.6 Material-removal abstraction

Use a bounded wear model.

Do not require simulation of true rail metallurgy.

Gameplay equation:

```text
removal =
    baseRemoval
    × downforceFactor
    × rpmFactor
    × stoneCondition
    × operatorFactor
```

Route roughness improves toward authored target.

Do not allow negative roughness.

---

# 11.7 Constant-pressure control

Player may choose operating profile:

```text
Conservative
Standard
Aggressive
```

Tradeoffs:

### Conservative
- slower;
- low stone wear;
- low spark/shatter risk.

### Standard
- balanced.

### Aggressive
- faster;
- more stone wear;
- higher hazard;
- potential profile damage.

This creates decision gameplay without exposing real machine-control instructions.

---

# 11.8 Permanent speed upgrade

Do not set:

```text
30 → 80 km/h
```

unconditionally.

Compute:

```text
safeSpeedLimit =
    min(
      routeDesignLimit,
      railConditionLimit,
      vehicleLimit,
      signaling/interlockLimit if modeled
    )
```

Grinding can improve the **rail-condition limit**.

It cannot exceed the route or vehicle design cap.

This prevents impossible route buffs.

---

# 11.9 Expedition integration

At route estimation:

```text
vehicle speed
× route infrastructure speed modifier
× stance
× weather/other travel modifiers
```

Avoid mutating `ExpeditionDefinition.distanceTicks` permanently.

The map catalog stays immutable.

Persistent improvement lives in route state.

---

# 11.10 Strategic map presentation

Map UI may show:

```text
DAMAGED RAIL
SERVICEABLE
REPROFILED
EXPRESS-CERTIFIED
```

This is derived from route condition.

No second UI-only status list.

---

# 11.11 Spark/fire hazard

Use abstract route/environment conditions:

```text
dryVegetation
waterSuppressionAvailable
weatherDryness
operatorProfile
```

Risk:

```text
sparkFireRisk =
    base
    × operatingProfile
    × dryness
    × (1 - suppression)
```

Potential consequence:

- forced stop;
- route hazard event;
- resource loss;
- local fire encounter.

No real ignition engineering detail.

---

# 11.12 Stone shatter

Failure chance depends on:

- stone condition;
- aggressive profile;
- machine condition;
- defect/foreign-object route tags.

On shatter:

- job pauses;
- stone consumed;
- machine condition hit;
- no progress duplication.

---

# 11.13 Traits/skills

Verify:

```text
RailGrinderOperator
TrackMaintenanceChief
```

Fallback to canonical:

- mechanical;
- crafting;
- survival;

as appropriate.

No engine-level string checks unless traits are the project’s established integration pattern.

---

# 11.14 Items

Verify/author:

```text
item_ceramic_rail_grinding_stone_set
item_pneumatic_rail_profiling_cylinder
item_trackside_spark_water_suppressor
```

Use foundry/crafting outputs where possible.

---

# 11.15 UI

Create:

```text
src/UI/RailGrindingPanel.cs
```

Show:

- rail segment;
- current roughness;
- current safe speed;
- projected safe speed;
- grinding profile;
- RPM;
- downforce;
- stone life;
- suppression water;
- route progress;
- hazard estimate.

No color-only status.

---

# 11.16 Persistence

Machine/module state belongs to the rail-grinding engine/module save.

Route roughness and permanent speed changes belong only to RouteInfrastructure state.

---

# 11.17 Plan 149 tests

### Catalog
- rail profiles valid;
- pass depth positive;
- speed upgrade within cap;
- item refs valid.

### Grinding
- roughness improves monotonically under valid operation;
- no negative roughness;
- stone wear increases;
- aggressive profile has greater wear/risk;
- no water increases spark-risk modifier.

### Route
- only target segment changes;
- route speed cap respects design/vehicle limits;
- persisted route condition survives reload.

### Expedition
- route estimate uses improved speed;
- non-rail route unaffected;
- non-draisine vehicle cannot consume rail bonus unless authored.

### Determinism
- same seed → same shatter/fire outcomes.

---

# 12. Cross-Plan Integration — Industrial Supply Chain

The four plans should share a coherent late-game item economy.

## 12.1 Foundry outputs

Potential Foundry products:

```text
superalloy_turbine_component_blank
precision_machine_mount
flail_chain_link
blast_shield_plate
rail_grinder_carriage
reader_housing
```

## 12.2 Crafting / specialized processes

Crafting may assemble:

```text
hydraulic drive motor
grinding trolley assembly
microfluidic reader
vacuum-pump service kit
```

## 12.3 Advanced processes

EB-PVD modifies:

```text
generator hot-section parts
vehicle heat-critical parts
```

Microfluidics consumes:

```text
chemical/medical consumables
precision manufactured cartridges
```

Mine flail consumes:

```text
chain links
fuel
repair parts
```

Rail grinder consumes:

```text
grinding stones
water
maintenance parts
```

## 12.4 No circular free-resource loop

Add a graph test ensuring no recipe cycle yields net-positive:

- mechanical parts;
- rare alloy;
- diagnostic reagents;
- flail chain;
- grinding stones;

without external input.

---

# 13. Cross-Plan Integration — Research & Unlock Progression

If the repository has the shared research unlock bridge, wire each plan through it.

Suggested progression:

```text
materials science
    ↓
advanced ceramics / surface coatings
    ↓
EB-PVD

field engineering
    ↓
heavy vehicle breaching
    ↓
Mine Flail

medical diagnostics
    ↓
microfluidics
    ↓
Diagnostic Cartridge Press

rail logistics
    ↓
track engineering
    ↓
Rail Grinding
```

Do not make panel availability equivalent to research completion.

Use canonical unlock flags/recipe unlocks.

---

# 14. Cross-Plan Integration — Skill Progression

Each operation should award action XP through the canonical `SkillProgressionSystem`.

Examples:

```text
EB-PVD completion → science/crafting
Flail breach → survival/mechanical/combat-engineering mapping
Diagnostic run → medical/science
Rail grinding → crafting/mechanical
```

Use authored mappings.

Avoid inventing four new disciplines unless progression design explicitly calls for them.

---

# 15. Cross-Plan Integration — Power and Infrastructure

## EB-PVD
High continuous electrical load.

## Microfluidic
Low/moderate clean-lab load + reader.

## Mine flail
Vehicle fuel/hydraulic load, not shelter grid during use.

## Rail grinder
Vehicle/mechanical energy + suppression water.

Add resource estimates to command previews.

---

# 16. Cross-Plan Integration — Maintenance

Each advanced machine requires maintenance.

Unify semantics:

```text
operatingHours
condition01
maintenanceDue
maintenanceCost
```

but let each engine own its own condition.

Do not create a universal global machine-health float that erases domain differences.

---

# 17. Player Command Surfaces

Each major irreversible operation should support:

```text
Preview
Execute
```

## EB-PVD preview

Show:

- component;
- coating;
- inputs;
- power;
- hours;
- operator;
- risk;
- projected modifier.

## Diagnostic preview

Show:

- patient;
- assay;
- cartridge;
- duration;
- expected confidence band;
- invalid-run risk.

Do not show hidden true disease state.

## Flail preview

Show:

- route;
- minefield severity;
- chain wear estimate;
- fuel;
- breach time;
- residual hazard estimate.

## Rail preview

Show:

- segment;
- roughness;
- target roughness;
- stones;
- water;
- time;
- expected speed cap.

---

# 18. Event Contracts

Prefer typed event DTOs.

## EB-PVD

```text
OnJobStarted(EbPvdJobEvent)
OnJobPaused(...)
OnJobFailed(...)
OnJobCompleted(...)
OnMaintenanceRequired(...)
```

## Microfluidic

```text
OnCartridgeManufactured(...)
OnDiagnosticStarted(...)
OnDiagnosticInvalid(...)
OnDiagnosticCompleted(...)
```

## Mine flail

```text
OnBreachStarted(...)
OnMinefieldProgress(...)
OnFlailFailure(...)
OnRouteBreached(...)
```

## Rail grinding

```text
OnGrindingStarted(...)
OnGrindingProgress(...)
OnGrindingFailure(...)
OnRailSegmentReprofiled(...)
```

Restore must not replay historical completion events.

---

# 19. Daily Briefing / Journal Integration

These technologies should appear in the campaign narrative layer.

Recommended typed day-event kinds:

```text
ebpvd_component_coated
ebpvd_maintenance_due
microfluidic_diagnosis_completed
microfluidic_invalid_run
minefield_route_breached
mine_flail_disabled
rail_segment_reprofiled
rail_grinder_disabled
```

Briefing categories:

```text
Production & Maintenance
Medical
Expedition Infrastructure
Warnings
```

Journal/codex milestones:

- first successful thermal-barrier coating;
- first pre-symptomatic diagnosis;
- first minefield corridor opened;
- first express rail corridor restored.

Use once-only knowledge keys.

---

# 20. UI Architecture

Each new panel should follow the established panel registry / scene-binding architecture.

Required:

```text
PanelRegistry registration
scene contract if scene-backed
bind action
open action
close action
headless construction test
accessibility test
```

Panels read immutable/read-model snapshots where practical.

## Common panel qualities

- stable labels;
- compact numeric readouts;
- tooltip/accessibility descriptions;
- explicit failure code message;
- no direct file IO;
- no direct RNG;
- no direct inventory mutation.

---

# 21. Persistence Architecture

New save sections:

```text
ebpvd_coating
microfluidic_diagnostics
route_infrastructure
```

Potential machine-module state may live inside vehicle save if architecture prefers:

```text
vehicle.modules.mine_flail
vehicle.modules.rail_grinder
```

Choose one authority and document it.

## Save invariants

- schema version explicit;
- old save missing section loads default;
- no runtime event replay;
- active jobs restore exactly;
- consumed inputs remain consumed;
- completed outputs do not duplicate;
- route upgrades survive reload;
- diagnostic evidence survives through its canonical medical owner.

---

# 22. Deterministic RNG Contract

Use campaign streams.

Suggested new stream IDs only if no suitable stream exists:

```text
AdvancedManufacturing
MedicalDiagnostics
RouteEngineering
```

Prefer domain streams:

```text
Foundry / Crafting
Disease / Medical
Expeditions
```

if current architecture expects those.

RNG calls occur only for true stochastic decisions:

- EB-PVD job failure/spallation;
- diagnostic false positive/negative/invalid;
- flail stall/damage;
- rail spark/shatter hazard.

Deterministic arithmetic consumes no RNG.

---

# 23. Scientific / Gameplay Validation Doctrine

The source plans contain several engineering numbers.

Treat them as **design inputs**, not automatically verified facts.

Create:

```text
docs/technical/PLANS_146_149_GAMEPLAY_ASSUMPTIONS.md
```

For each numeric claim:

```text
claim
source-plan value
gameplay use
physical unit
allowed range
hard-coded? no
catalog field
balance rationale
```

Examples requiring explicit balance review:

```text
EB-PVD +400°C component tolerance
+45% power output
flail 500 RPM / 1500 kPa
rail 30 → 80 km/h
rail friction -40%
15-minute assay
```

Do not encode sensational numbers as unconditional system-wide buffs.

---

# 24. Data Integrity Rules

Extend catalog validator.

## EB-PVD
- coating target item exists;
- bond coat item exists;
- valid substrate tag known;
- target thickness > 0;
- voltage range sane.

## Microfluidic
- assay disease IDs exist;
- cartridge/reagent IDs exist;
- sensitivity/specificity bounded;
- reader exists.

## Mine flail
- module-compatible vehicle tags exist;
- item refs exist;
- route-mode tags valid.

## Rail grinding
- rail profile IDs unique;
- vehicle compatibility valid;
- speed cap nonnegative;
- items valid.

---

# 25. Content Utilization Rules

Every authored catalog must have a live loader and runtime consumer.

Add scanner coverage:

```text
ebpvd_coating_catalog.json
microfluidic_diagnostic_catalog.json
mine_flail_catalog.json
rail_grinding_catalog.json
```

Fail if marked gameplay-consumed but no loader/consumer path exists.

---

# 26. Test Matrix — Plan 146

| ID | Contract |
|---|---|
| P146-001 | catalog loads |
| P146-002 | IDs unique |
| P146-003 | item references valid |
| P146-004 | beam-power math dimensionally correct |
| P146-005 | progress monotonic |
| P146-006 | thickness bounded |
| P146-007 | uniformity bounded |
| P146-008 | insufficient items atomic failure |
| P146-009 | unpowered start blocked |
| P146-010 | brownout pauses/degrades per contract |
| P146-011 | completion produces exactly one component |
| P146-012 | restore does not duplicate component |
| P146-013 | maintenance timer persists |
| P146-014 | generator modifier applied only after install |
| P146-015 | unrelated generator fields unchanged |
| P146-016 | vehicle modifier only installed target |
| P146-017 | same seed replay identical |
| P146-018 | old save missing section loads |
| P146-019 | panel binds production authority |
| P146-020 | data/content selftests pass |

---

# 27. Test Matrix — Plan 147

| ID | Contract |
|---|---|
| P147-001 | catalog loads |
| P147-002 | valid vehicle compatibility |
| P147-003 | cannot operate on invalid route mode |
| P147-004 | minefield required |
| P147-005 | chain wear monotonic |
| P147-006 | hydraulic failure deterministic |
| P147-007 | shield damage bounded |
| P147-008 | breach progress monotonic |
| P147-009 | residual risk decreases |
| P147-010 | other route unchanged |
| P147-011 | route clearance persists |
| P147-012 | no global mine immunity |
| P147-013 | expedition hazard reads route state |
| P147-014 | repeated completion idempotent |
| P147-015 | repair consumes items atomically |
| P147-016 | same seed replay identical |
| P147-017 | old save compatible |
| P147-018 | panel never mutates route directly |
| P147-019 | minefield encounter eligibility responds |
| P147-020 | data/content selftests pass |

---

# 28. Test Matrix — Plan 148

| ID | Contract |
|---|---|
| P148-001 | catalog loads |
| P148-002 | disease refs valid |
| P148-003 | item refs valid |
| P148-004 | sensitivity/specificity bounded |
| P148-005 | cartridge manufacture atomic |
| P148-006 | invalid cartridge blocked |
| P148-007 | reader requirement enforced |
| P148-008 | diagnostic result deterministic |
| P148-009 | false negative path possible |
| P148-010 | false positive path possible |
| P148-011 | invalid control yields no diagnosis mutation |
| P148-012 | early stage lower detectability |
| P148-013 | evidence enters canonical pipeline |
| P148-014 | UI cannot reveal hidden disease truth |
| P148-015 | save/reload retains run state |
| P148-016 | completed run not repeated on restore |
| P148-017 | isolation remains separate authority |
| P148-018 | treatment remains separate authority |
| P148-019 | panel accessibility |
| P148-020 | data/content selftests pass |

---

# 29. Test Matrix — Plan 149

| ID | Contract |
|---|---|
| P149-001 | catalog loads |
| P149-002 | rail route required |
| P149-003 | roughness monotonic improvement |
| P149-004 | roughness never negative |
| P149-005 | stone wear monotonic |
| P149-006 | aggressive profile higher wear |
| P149-007 | suppression reduces fire risk |
| P149-008 | shatter deterministic |
| P149-009 | target route only modified |
| P149-010 | route speed cap bounded |
| P149-011 | vehicle design cap respected |
| P149-012 | route design cap respected |
| P149-013 | state survives reload |
| P149-014 | expedition estimate sees speed improvement |
| P149-015 | non-rail travel unaffected |
| P149-016 | repeated completion idempotent |
| P149-017 | repair atomic |
| P149-018 | panel binds canonical state |
| P149-019 | same seed replay identical |
| P149-020 | data/content selftests pass |

---

# 30. Cross-Plan Integration Tests

## INT-001 — Foundry → EB-PVD → Power

```text
produce compatible component
→ consume coating inputs
→ finish coating
→ install component
→ generator projection changes
→ save
→ reload
→ same installed effect
```

## INT-002 — Foundry → Flail repair → Route

```text
produce chain links
→ repair module
→ breach minefield
→ route state changes
→ later expedition sees reduced hazard
```

## INT-003 — Medical production → diagnosis → quarantine

```text
manufacture cartridge
→ test infected survivor
→ evidence recorded
→ diagnosis becomes available
→ player chooses isolation
→ DiseaseSystem spread outcome changes
```

## INT-004 — Rail grinding → expedition travel

```text
damaged rail route
→ grind segment
→ roughness improved
→ speed limit improved
→ expedition estimate shorter
→ save/reload
→ improvement retained
```

## INT-005 — No cross-route leakage

Modify one rail or minefield segment.

All other segments retain exact checksums.

## INT-006 — Same-seed replay

Run all four systems with a fixed seed.

Compare canonical state hashes.

---

# 31. Balance Model

## EB-PVD

Costs:
- rare consumables;
- high power;
- long machine time;
- maintenance;
- trained labor.

Benefits:
- modest but durable efficiency/reliability;
- late-game component survival.

Avoid:
- +45% global power becoming mandatory dominant strategy.

## Mine flail

Costs:
- vehicle slot/module;
- high fuel;
- chain wear;
- slow breach;
- repair risk.

Benefits:
- persistent route access;
- reduced casualties;
- strategic logistics.

Avoid:
- minefields becoming irrelevant once first module exists.

## Microfluidic diagnostics

Costs:
- assay reagents;
- cartridge manufacturing;
- reader;
- skilled labor.

Benefits:
- earlier certainty;
- better quarantine timing.

Avoid:
- disease system trivialized by universal zero-cost tests.

## Rail grinding

Costs:
- grinding stones;
- water;
- labor;
- machine wear;
- route time.

Benefits:
- recurring travel savings;
- strategic redeployment.

Avoid:
- instant network-wide 80 km/h bonus.

---

# 32. UI / UX Acceptance

Every panel must have:

1. clear current state;
2. prerequisites;
3. cost quote;
4. projected outcome;
5. risk explanation;
6. command confirmation;
7. stable failure message;
8. post-action feedback;
9. save/reload continuity;
10. keyboard path;
11. accessible labels;
12. no color-only critical state.

---

# 33. Localization

Add keys:

```text
ebpvd.*
mine_flail.*
microfluidic.*
rail_grinding.*
route_infrastructure.*
```

Use English fallback.

Technical terms can remain authentic, but player-facing explanations should be concise.

Example:

```text
ebpvd.risk.vacuum_loss
"Vacuum instability may ruin the coating run."
```

not a dense materials-science lecture in every tooltip.

---

# 34. Save Migration Matrix

Create:

```text
docs/saves/PLANS_146_149_SAVE_MIGRATION_MATRIX.md
```

Rows:

```text
section
schema version
introduced plan
missing-section default
null-list normalization
invalid ID behavior
migration test
checksum coverage
```

Mandatory sections:

```text
ebpvd_coating
microfluidic_diagnostics
route_infrastructure
vehicle-module state if separate
```

---

# 35. File Touch Map

## Plan 146

New/likely:

```text
Assets/StreamingAssets/Data/ebpvd_coating_catalog.json
Assets/Ashfall.Core/Shelter/EbPvdCoatingEngine.cs
src/Host/EbPvdCoatingHostSession.cs
src/Host/EbPvdCoatingSaveStore.cs
src/UI/EbPvdCoatingPanel.cs
Ashfall.Core.Tests/Shelter/EbPvdCoatingEngineTests.cs
docs/shelter/PLAN_146_EBPVD_COATINGS_CLOSEOUT.md
```

Existing integration:

```text
Assets/Ashfall.Core/Foundry/SilentFoundrySystem*.cs
Assets/Ashfall.Core/Foundry/FoundryActionSurface.cs
Assets/Ashfall.Core/Shelter/PowerGridSystem.cs
Assets/Ashfall.Core/ExpeditionVehicleSystem.cs
items.json
recipes/foundry data
Main composition
SaveSectionRegistry / Save orchestrator
```

## Plan 147

New/likely:

```text
Assets/StreamingAssets/Data/mine_flail_catalog.json
Assets/Ashfall.Core/Expeditions/MineClearingFlailEngine.cs
src/Host/MineClearingFlailHostSession.cs
src/UI/MineFlailPanel.cs
Ashfall.Core.Tests/Expeditions/MineClearingFlailEngineTests.cs
docs/expeditions/PLAN_147_MINE_FLAIL_CLOSEOUT.md
```

Shared:

```text
RouteInfrastructureSystem.cs
RouteInfrastructureSaveStore.cs
ExpeditionSystem.cs
ExpeditionVehicleSystem.cs
WastelandMapSystem.cs
encounter/hazard data
items.json
```

## Plan 148

New/likely:

```text
Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json
Assets/Ashfall.Core/Medical/MicrofluidicDiagnosticEngine.cs
src/Host/MicrofluidicDiagnosticHostSession.cs
src/Host/MicrofluidicDiagnosticSaveStore.cs
src/UI/MicrofluidicDiagnosticPanel.cs
Ashfall.Core.Tests/Medical/MicrofluidicDiagnosticEngineTests.cs
docs/medical/PLAN_148_MICROFLUIDIC_DIAGNOSTICS_CLOSEOUT.md
```

Existing:

```text
MedicalPipelineCoordinator.cs
MedicalHostSession.cs
DiseaseSystem.cs
SickListSystem
DiagnosisKnowledgeStore
MedicalWardSystem
items.json
```

## Plan 149

New/likely:

```text
Assets/StreamingAssets/Data/rail_grinding_catalog.json
Assets/Ashfall.Core/Expeditions/RailGrindingEngine.cs
src/Host/RailGrindingHostSession.cs
src/UI/RailGrindingPanel.cs
Ashfall.Core.Tests/Expeditions/RailGrindingEngineTests.cs
docs/expeditions/PLAN_149_RAIL_GRINDING_CLOSEOUT.md
```

Shared:

```text
RouteInfrastructureSystem.cs
RouteInfrastructureSaveStore.cs
ExpeditionSystem.cs
ExpeditionVehicleSystem.cs
WastelandMapSystem.cs
items.json
vehicles.json
```

---

# 36. Consolidated Execution Checklist

## Global audit

1. Record baseline SHA.
2. Run build.
3. Run full Core tests.
4. Run fast gates.
5. Audit Plan 146 dependencies.
6. Audit Plan 147 dependencies.
7. Audit Plan 148 dependencies.
8. Audit Plan 149 dependencies.
9. Document missing/renamed systems.
10. Audit item naming conventions.
11. Audit recipe conventions.
12. Audit trait/skill conventions.
13. Audit save registration.
14. Audit panel registration.
15. Audit deterministic RNG stream policy.

## Shared route infrastructure

16. Define route infrastructure DTO.
17. Reference canonical Wasteland route IDs.
18. Add route mode.
19. Add minefield state.
20. Add rail condition state.
21. Add route hazard query.
22. Add route speed query.
23. Add save section.
24. Add migration.
25. Add checksum coverage.
26. Add route-isolation tests.

## EB-PVD catalog/core

27. Author catalog schema.
28. Add loader.
29. Add validator.
30. Add machine state.
31. Add job state.
32. Add bond-coat state.
33. Add beam-power math.
34. Add deposition progress.
35. Add uniformity.
36. Add thickness.
37. Add failure profiles.
38. Add maintenance.
39. Add capture.
40. Add restore.
41. Add typed events.
42. Add command preview.
43. Add command execute.

## EB-PVD integration

44. Identify compatible foundry outputs.
45. Add substrate tag.
46. Add ceramic target item.
47. Add filament item.
48. Add bond-coat item.
49. Add recipes.
50. Add PowerGrid load.
51. Add brownout behavior.
52. Add operator projection.
53. Add component modification output.
54. Add generator install bridge.
55. Add vehicle install bridge.
56. Add save store.
57. Add host session.
58. Add panel.
59. Add accessibility.
60. Add localization.
61. Add journal milestone.
62. Add briefing event.
63. Add unit tests.
64. Add integration tests.

## Microfluidic catalog/core

65. Enumerate canonical disease IDs.
66. Select initial assay targets.
67. Author assay catalog.
68. Add loader.
69. Add validator.
70. Add cartridge manufacture state.
71. Add diagnostic run state.
72. Add sample-quality abstraction.
73. Add assay detectability.
74. Add sensitivity/specificity.
75. Add invalid-run states.
76. Add confidence output.
77. Add typed events.
78. Add capture.
79. Add restore.
80. Add preview/execute.

## Microfluidic integration

81. Add PDMS kit.
82. Add assay reagents.
83. Add reader item.
84. Add manufacturing recipe.
85. Add inventory transactions.
86. Add power/station gate.
87. Add operator projection.
88. Add MedicalPipeline evidence adapter.
89. Add SickList integration.
90. Ensure DiseaseSystem truth remains hidden.
91. Add save store.
92. Add host session.
93. Add panel.
94. Add accessibility.
95. Add localization.
96. Add journal milestone.
97. Add briefing event.
98. Add unit tests.
99. Add deterministic diagnostic tests.
100. Add vertical-slice medical test.

## Mine flail catalog/core

101. Identify minefield authority.
102. Map minefields to route segments.
103. Author flail catalog.
104. Add loader.
105. Add validator.
106. Add module state.
107. Add active breach state.
108. Add chain wear.
109. Add hydraulic condition.
110. Add shield condition.
111. Add breach progress.
112. Add clearance efficiency.
113. Add residual risk.
114. Add failure profiles.
115. Add typed events.
116. Add capture.
117. Add restore.
118. Add preview/execute.

## Mine flail integration

119. Add vehicle compatibility tags.
120. Add installable module.
121. Add chain item.
122. Add hydraulic motor item.
123. Add shield item.
124. Add recipes.
125. Add operator projection.
126. Add expedition hazard projection.
127. Add encounter eligibility adjustment.
128. Add route state mutation.
129. Add host session.
130. Add panel.
131. Add accessibility.
132. Add localization.
133. Add journal milestone.
134. Add briefing event.
135. Add unit tests.
136. Add route integration tests.
137. Add expedition integration tests.

## Rail catalog/core

138. Identify rail route IDs.
139. Identify rail-capable vehicles.
140. Author rail catalog.
141. Add loader.
142. Add validator.
143. Add rail condition DTO.
144. Add grinder module state.
145. Add grinding job.
146. Add roughness model.
147. Add profile-error model.
148. Add stone wear.
149. Add downforce/RPM factors.
150. Add operating profiles.
151. Add water suppression.
152. Add spark hazard.
153. Add stone shatter.
154. Add safe-speed derivation.
155. Add typed events.
156. Add capture.
157. Add restore.
158. Add preview/execute.

## Rail integration

159. Add grinder vehicle module.
160. Add grinding stone item.
161. Add profiling cylinder item.
162. Add suppressor item.
163. Add recipes.
164. Add operator projection.
165. Add route condition mutation.
166. Add Expedition speed projection.
167. Add map route read model.
168. Add host session.
169. Add panel.
170. Add accessibility.
171. Add localization.
172. Add journal milestone.
173. Add briefing event.
174. Add unit tests.
175. Add route speed tests.
176. Add expedition integration tests.

## Cross-system

177. Add research unlocks.
178. Add skill XP hooks.
179. Add Daily Briefing mappings.
180. Add Journal/Codex milestones.
181. Add content-utilization registrations.
182. Add data-integrity rules.
183. Add save migration matrix.
184. Add command preview tests.
185. Add atomic inventory tests.
186. Add no-shadow-state tests.
187. Add same-seed replay tests.
188. Add old-save compatibility tests.
189. Add panel route tests.
190. Add scene/accessibility tests.

## Final verification

191. Run P146 tests.
192. Run P147 tests.
193. Run P148 tests.
194. Run P149 tests.
195. Run expedition suite.
196. Run vehicle suite.
197. Run foundry suite.
198. Run power-grid suite.
199. Run medical/disease suite.
200. Run save suite.
201. Run data-integrity selftest.
202. Run content-utilization selftest.
203. Run bridge/selftests relevant to touched systems.
204. Run full Core test suite.
205. Run full build.
206. Run fast CI gates.
207. Verify zero new warnings.
208. Regenerate architecture/data docs if required.
209. Author four closeout documents.
210. Author unified implementation report.

---

# 37. Cross-System Failure Modes to Prevent

## FM-01 — Building against nonexistent plan-class names
Always resolve live authority first.

## FM-02 — Creating a second foundry stack
EB-PVD consumes Silent Foundry outputs.

## FM-03 — Treating a coating as a global buff
Coating must be applied to/installed on a component.

## FM-04 — Hardcoding +45% power
Use authored bounded modifiers.

## FM-05 — Vehicle owns route state
Route infrastructure owns persistent route changes.

## FM-06 — Flail ownership means mine immunity everywhere
Only cleared/actively breached route segments change.

## FM-07 — Minefield state duplicated in encounter and route saves
Choose one authority.

## FM-08 — Rail speed stored in UI
Route state owns condition; speed is derived.

## FM-09 — Grinding exceeds vehicle/design limit
Use minimum of all limiting authorities.

## FM-10 — Microfluidic UI reads hidden disease state
Only result DTO reaches UI.

## FM-11 — Diagnostic tool replaces DiseaseSystem
It only contributes evidence.

## FM-12 — Failed assay consumes nothing after run
Manufacturing/run costs should follow explicit transaction semantics.

## FM-13 — Save restore repeats completion effects
Restore is silent.

## FM-14 — New systems use unseeded randomness
All stochastic resolution uses campaign RNG.

## FM-15 — Catalog exists but no runtime consumer
Content-utilization gate must catch it.

## FM-16 — New traits hard-coded without catalog
Verify or author through canonical progression.

## FM-17 — Four separate maintenance frameworks
Reuse semantic patterns but maintain domain ownership.

## FM-18 — Real engineering claims used as unquestioned balance facts
Catalog them and validate gameplay effect.

## FM-19 — Item IDs invented without data validation
All references integrity-checked.

## FM-20 — New save sidecars bypass campaign envelope
Register all sections canonically.

---

# 38. Suggested Commit Sequence

1. `audit: reconcile plan 146-149 authorities`
2. `world: add persistent route infrastructure state`
3. `save: register route infrastructure persistence`
4. `shelter: add ebpvd coating catalog and core`
5. `shelter: integrate ebpvd with foundry and power`
6. `shelter: add ebpvd host ui save and tests`
7. `medical: add microfluidic diagnostic catalog and core`
8. `medical: integrate diagnostic evidence pipeline`
9. `medical: add host ui save and vertical tests`
10. `expeditions: add mine flail catalog and core`
11. `expeditions: integrate flail with vehicles and route hazards`
12. `expeditions: add mine flail ui save tests`
13. `expeditions: add rail grinding catalog and core`
14. `world: integrate rail condition with route travel`
15. `expeditions: add rail grinding ui save tests`
16. `campaign: add advanced technology briefing and journal hooks`
17. `qa: add determinism data integrity content utilization gates`
18. `docs: close plans 146-149`

Each commit should compile independently.

---

# 39. Verification Commands

```bash
dotnet build Ashfall.csproj
```

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

Focused:

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter EbPvd
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter MineClearingFlail
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Microfluidic
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter RailGrinding
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Expedition
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Vehicle
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Medical
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Disease
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter PowerGrid
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Foundry
```

Headless:

```bash
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --content-utilization-selftest
```

CI:

```bash
python3 scripts/ci/run-gates.py --tier fast
```

Use the project’s actual accepted invocation if current CLI syntax differs.

---

# 40. Acceptance Gates

## Gate 1 — Authority
No duplicated owner for foundry, power, disease, vehicle, route, or inventory facts.

## Gate 2 — Data
All four new catalogs load and validate.

## Gate 3 — Gameplay reachability
Each technology has a real unlock and player command path.

## Gate 4 — Inventory
Every irreversible job consumes canonical resources atomically.

## Gate 5 — Determinism
Paired runs with identical seed produce identical canonical results.

## Gate 6 — Persistence
Active jobs, machine wear, route improvements, and diagnostic evidence survive save/reload.

## Gate 7 — Downstream consequence
- EB-PVD changes installed component behavior.
- Flail changes route hazard.
- Diagnostics change evidence/diagnosis timing.
- Rail grinding changes route travel speed/condition.

## Gate 8 — UI
Panels are presentation-only and accessibility compliant.

## Gate 9 — Content utilization
No dead catalog or dead item IDs.

## Gate 10 — Regression
Full test suite + full build + fast gates pass.

---

# 41. Definition of Done — Plan 146

A player can:

1. unlock EB-PVD;
2. obtain a compatible component;
3. obtain target/bond-coat consumables;
4. allocate power;
5. assign a qualified operator;
6. preview job;
7. start job atomically;
8. observe bounded process state;
9. encounter maintenance/failure possibility;
10. complete one coated component;
11. install it into generator/vehicle;
12. see a bounded downstream modifier;
13. save;
14. reload;
15. retain the exact effect without duplicated output.

---

# 42. Definition of Done — Plan 147

A player can:

1. acquire/install a mine-flail vehicle module;
2. identify a minefield route segment;
3. inspect chain/shield/hydraulic condition;
4. preview breach costs;
5. begin deterministic breach;
6. consume fuel/chain condition;
7. suffer possible bounded equipment failures;
8. progress clearance;
9. create a persistent breached corridor;
10. send a following expedition;
11. receive the reduced route hazard only there;
12. save/reload;
13. retain clearance;
14. repair the flail through real inventory;
15. never gain global mine immunity.

---

# 43. Definition of Done — Plan 148

A player can:

1. unlock microfluidic diagnostics;
2. manufacture an assay cartridge;
3. select an eligible patient;
4. select an assay;
5. consume the cartridge;
6. run the diagnostic;
7. receive positive/negative/indeterminate/invalid evidence;
8. never see hidden disease truth directly;
9. feed evidence to the canonical diagnosis pipeline;
10. choose quarantine/treatment separately;
11. save/reload;
12. retain diagnosis evidence;
13. avoid duplicated result events;
14. see accessible uncertainty messaging;
15. gain meaningful earlier-detection value without trivializing disease.

---

# 44. Definition of Done — Plan 149

A player can:

1. own a rail-capable vehicle;
2. install/prepare grinding equipment;
3. target a damaged rail segment;
4. see roughness and safe-speed state;
5. quote stone/water/time costs;
6. choose operating profile;
7. grind the segment;
8. incur stone wear and hazard risk;
9. improve route condition;
10. raise the condition-limited speed cap;
11. remain bounded by vehicle/route design limits;
12. dispatch an expedition;
13. observe shorter travel on that rail segment;
14. save/reload;
15. retain route improvement exactly.

---

# 45. Unified Definition of Done

Plans 146–149 are fully complete when this campaign-scale chain is possible without test-only shortcuts:

```text
Foundry produces advanced component stock
        ↓
EB-PVD coats a real component
        ↓
Generator / vehicle receives installed thermal upgrade
        ↓
Shelter gains bounded efficiency/reliability
        ↓
Advanced industrial economy produces route-engineering parts
        ↓
Expedition vehicle installs mine flail
        ↓
Minefield corridor is persistently breached
        ↓
Previously dangerous route becomes strategically viable
        ↓
Rail-grinding trolley rehabilitates a rail segment
        ↓
Safe rail travel speed improves persistently
        ↓
Expeditions reach distant nodes faster
        ↓
Medical expedition/shelter acquires diagnostic supplies
        ↓
Microfluidic cartridge detects disease earlier
        ↓
Medical pipeline receives evidence
        ↓
Player isolates/treats sooner
        ↓
Campaign saves
        ↓
Runtime is destroyed
        ↓
Campaign reloads
        ↓
All component upgrades, route changes, machine wear,
diagnostic evidence, inventories and active jobs remain correct
        ↓
Same-seed replay produces identical canonical outcomes
```

At that point, the four plans form a coherent late-game technological pillar rather than four disconnected panels.

---

# 46. Explicit Non-Goals

This program does not authorize:

- replacing Silent Foundry;
- replacing PowerGridSystem;
- replacing DiseaseSystem;
- replacing ExpeditionSystem;
- replacing ExpeditionVehicleSystem;
- creating a second world map;
- simulating real high-voltage hardware construction;
- simulating real explosive-device construction;
- implementing real pathogen culture protocols;
- adding unrelated diseases solely to fill an arbitrary assay count;
- allowing EB-PVD to globally buff every generator/vehicle;
- allowing mine flails to erase every mine encounter globally;
- allowing rail grinding to exceed route/vehicle design limits;
- turning diagnostics into perfect certainty;
- rewriting the game’s entire research tree;
- introducing multiplayer networking.

Necessary authority closure, content wiring, persistence, UI, testing, balance and documentation required to make Plans 146–149 real and reachable are in scope.

---

# 47. Final Delivery Artifacts

Implementation should finish with:

```text
docs/architecture/PLANS_146_149_AUTHORITY_AUDIT.md
docs/technical/PLANS_146_149_GAMEPLAY_ASSUMPTIONS.md
docs/saves/PLANS_146_149_SAVE_MIGRATION_MATRIX.md
docs/shelter/PLAN_146_EBPVD_COATINGS_CLOSEOUT.md
docs/expeditions/PLAN_147_MINE_FLAIL_CLOSEOUT.md
docs/medical/PLAN_148_MICROFLUIDIC_DIAGNOSTICS_CLOSEOUT.md
docs/expeditions/PLAN_149_RAIL_GRINDING_CLOSEOUT.md
docs/integration/PLANS_146_149_UNIFIED_CLOSEOUT.md
```

The unified closeout must report:

- files created/modified;
- catalog counts;
- save sections;
- exact test counts;
- exact gate results;
- data-integrity result;
- content-utilization result;
- determinism proof;
- known deferred items;
- final commit SHA.

---

**END OF FLAGSHIP MASTER PLAN**
</USER_REQUEST>
<ADDITIONAL_METADATA>
The current local time is: 2026-09-05T21:19:26+03:00.
</ADDITIONAL_METADATA>
<USER_SETTINGS_CHANGE>
The user changed setting `Model Selection` from None to Gemini 3.8 Flash (High). No need to comment on this change if the user doesn't ask about it. If reporting what model you are, please use a human readable name instead of the exact string.
</USER_SETTINGS_CHANGE>