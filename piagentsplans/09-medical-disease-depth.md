# Plan 09 — Medical Disease, Detox, Triage, and Palliative-Care Integration

> **Rebuild status:** CORE AUTHORITIES PRESENT — BOUNDED DIAGNOSTIC/CARE INTEGRATION REMAINS
>
> **Package:** `OLDEST-15-PIAGENTS-PLAN-QUALITY-REBASE`
>
> **Claim:** `claim-oldest-15-piagents-quality-rebase-2026-09-25`
>
> **Current-evidence date:** 2026-09-25
>
> **Authority order:** live source and data → `AGENTS.md` → `docs/newest-ashfall-master-expansion-authority-v2-0-complete-compiled-edition-volumes-1-57.md` → plan ledgers → this document.
>
> **Authority checksum:** `911d6bdc292b1d4f525f7948c1c8d98b1cdaf951b9b2c9f00e8ac7965372a63c`
>
> **Length policy:** the 150k–170k band is a completeness checkpoint, never a reason to add filler. This plan is allowed to trim below the band if the verified architecture is exhausted.

## 0. Integrity Statement and Plan Status

This file replaces unclaimed generated sections that mixed current evidence, fictional APIs, and unsupported save claims. It is a planning and architecture artifact only. It authorizes no production, data, test, save, generated-index, or UI edits. Every path labeled current must exist at rebuild time. Any future `CREATE` proposal is explicitly hypothetical and belongs to a later, separately claimed implementation package.

The rebuild follows four passes: content/current-reality first; integration framework second; accuracy and contradiction removal third; independent precision and handoff review fourth. Character count is recorded by external verification, not embedded recursively in the document.

# 1. Objective

- Disease, chemical dependency, taper management, clinical triage and palliative dignity already exist as distinct owners. The original plan is therefore an integration and legibility plan, not a greenfield medical architecture.
- Diagnosis should project authored `medical_texts.json` content against real `DiseaseInfectionState` and catalog definitions. It must distinguish suspected, diagnosed, treated and terminal states and never recalculate infection probability.
- Detox and palliative flows must route through existing item, ward, relationship, final-wish and memorial owners. The plan focuses on truthful host projections, consent, capacity, save and cross-system consequence tests.

**Bounded outcome:** Use `DiseaseSystem`, `ChemicalDependencySystem`, `DependencyTaperLedger`, `ClinicalWardTriageEngine`, `PalliativeCareDignityEngine`, `MedicalWardSystem`, dose/autopsy owners and current host sessions. Do not create `MedicalSystem`, a second disease loader, a parallel dependency ledger, or a new palliative meter.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- `disease_catalog.json` schema 3 contains 20 diseases with vectors, phases and treatments.
- `medical_texts.json` contains 83 condition records with symptoms, treatment steps, consequences and prevention text.
- `DiseaseSystem` persists RNG state, infection stage, diagnosis, treatment, immunity and outbreak state.
- `ChemicalDependencySystem` persists dependency, managed detox and cold-turkey state and exposes previews/results.
- `ClinicalWardTriageEngine` and `PalliativeCareDignityEngine` provide pure permille-based decision support without owning beds or final wishes.

**Master-authority sections applied to this rebase:**

- Volume 7 record contracts
- Volume 8 balance harness H-C3
- Volume 20 medical device
- Volume 27 worker seeds

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Build or verify a read-only diagnostic projection joining medical text to real disease/infection state.
- Expose honest uncertainty and test availability without creating a diagnosis authority.
- Connect taper, ward and palliative records through existing host/save owners.
- Audit consent, visitor capacity, memorial and final-wish boundaries end to end.

Anything beyond this list is a different package. In particular, this plan does not convert a documentation gap into permission to create a second domain owner.

# 4. Current Evidence and Premise Audit

The current evidence set for this plan is enumerated in the appendices with file hashes, declaration digests, catalog schema/counts and focused test inventories. A source declaration proves an API surface exists; it does not prove a fresh test run or live player reachability. Those claims require the verification steps in this document.

**Evidence classes used here:**

- **VERIFIED CURRENT:** the named path exists and its contents were read during this rebuild.
- **HISTORICAL RECORD:** an archived closeout or old plan says a package once landed; it is useful context but is not current pass evidence.
- **PROPOSAL:** a future seam or file shape that requires a new claim and premise recheck.
- **UNKNOWN:** deliberately unresolved because the present plan does not need to invent an answer.

# 5. Existing Extension Seams

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| infection, spread, treatment response and immunity | DiseaseSystem | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | Sole disease simulation owner. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | Sole dependency ledger. |
| clinical taper schedule and care posture | Dependency taper ledger | `Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs` | Extends dependency state; does not replace it. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs` | Pure decision support; MedicalWardSystem owns beds. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs` | Pure engine; final wishes and memorial remain separate owners. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Medical Disease, Detox, Triage, and Palliative-Care Integration
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ DiseaseSystem
│   infection, spread, treatment response and immunity
│ ChemicalDependencySystem
│   dependency, withdrawal and detox command state
│ Dependency taper ledger
│   clinical taper schedule and care posture
│ ClinicalWardTriageEngine
│   triage priority, isolation and surgical readiness
│ PalliativeCareDignityEngine
│   pain, lucidity, dignity and grief progression
                │
                ▼
Host projection → existing command → owner mutation → typed fact
                │
                ├─ UI / briefing / journal / audio presentation
                ├─ existing save envelope and checksum
                └─ focused Core / host / headless verification
```

The architecture is deliberately projection-first where a read model is sufficient, owner-extension-first where new mutable facts are required, and data-first only when an existing catalog can express the content. It does not permit a new subsystem merely to make the plan look larger.

## 6.1 Architectural decisions

1. **Preserve current state ownership.** DiseaseSystem owns infection, spread, treatment response and immunity: Sole disease simulation owner.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| infection, spread, treatment response and immunity | DiseaseSystem | `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` | Sole disease simulation owner. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` | Sole dependency ledger. |
| clinical taper schedule and care posture | Dependency taper ledger | `Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs` | Extends dependency state; does not replace it. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs` | Pure decision support; MedicalWardSystem owns beds. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs` | Pure engine; final wishes and memorial remain separate owners. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. author disease/medical text
2. validate references
3. expose through canonical medical owner
4. record exposure or dependency action
5. project symptoms and uncertainty
6. preview command
7. execute through owner
8. route outcome to ward/memorial/needs

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Diagnosis is stored on DiseaseInfectionState, not in a second diagnostic ledger.
- Treatment effects remain per patient and capped by lethality reduction.
- Detox progress and relapse facts remain in dependency/taper state.
- Palliative patient records are host/session state projected into the existing medical/save family.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- A diagnosis names a real disease and patient; it never infers a disease solely from prose.
- Treatment refusal returns a named reason and consumes nothing.
- Palliative care never rewrites prognosis outside its explicit record.
- Consent and next-of-kin actions are recorded without stealing FinalWishSystem authority.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- Consume `disease_catalog.json` and `medical_texts.json`; do not add rows in the integration tranche.
- Use existing medical items and procedures.
- No second symptom or diagnosis catalog.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- Disease uses its current versioned state.
- Dependency/taper/ward/palliative use their current sections or host session envelopes.
- Old saves without diagnosis default to undiagnosed, not healthy.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Disease persists seed and RNG position.
- Dependency and palliative calculations are deterministic.
- No wall-clock prognosis or treatment timing.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- Disease events expose infection, quarantine, outbreak, recovery and death facts.
- Dependency events request bounded morale/craft/combat effects.
- Palliative outcomes feed memorial/final-wish consumers only through existing owners.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Medical.cs
- src/Host/MedicalHostSession.cs
- src/UI/AfflictionsPanel.cs
- src/UI/ChemicalDependencyPanel.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Casebook and therapy prose must match actual stage and treatment facts.
- Consent and dignity language is restrained and non-judgmental.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | UI treats suspected as confirmed. | DiseaseSystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | A failed treatment consumes inventory twice. | ChemicalDependencySystem | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | Palliative UI grants a final wish. | Dependency taper ledger | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A new diagnosis state bypasses immunity. | ClinicalWardTriageEngine | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Taper and dependency both drain morale in the same tick. | PalliativeCareDignityEngine | Focused negative test or static source gate; no broad-suite dependency. |
| F-06 | A medical read model becomes authoritative. | DiseaseSystem | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DiseaseSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/AutopsyBridgeTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` only if a touched medical catalog changes.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — owner census | Map disease, dependency, ward, palliative, final-wish and memorial owners. | No duplicated state owner. | No production path until the owning implementation package is separately claimed. |
| 1 — diagnostic projection | Join authored text to current infection state read-only. | Suspected/confirmed distinction is tested. | No production path until the owning implementation package is separately claimed. |
| 2 — care commands | Verify preview/execute parity for dependency and treatment commands. | No double consumption or stale preview. | No production path until the owning implementation package is separately claimed. |
| 3 — palliative handoff | Connect care outcomes to existing final-wish and memorial seams. | No duplicated end-of-life authority. | No production path until the owning implementation package is separately claimed. |
| 4 — host UX | Expose uncertainty, refusals and consent in current panels. | Keyboard/controller and a11y gates pass. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| Assets/Ashfall.Core/Disease/DiseaseSystem.cs | READ; MODIFY only for proven contract gap | Disease owner |
| Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs | READ | Dependency owner |
| Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs | READ | Triage support |
| Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs | READ | Palliative support |
| src/UI/AfflictionsPanel.cs | READ; MODIFY only for truthful projection gap | Presentation |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Parallel diagnosis state. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Double-counted care effects. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Stale treatment previews. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Unconsented end-of-life automation. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Overclaiming diagnostic certainty. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No disease catalog expansion.
- No new pathogen or diagnosis system.
- No replacement mental-health authority.
- No clinical advice outside fictional game mechanics.

# 23. Rollback and Recovery

- Read-model/UI changes are reversible.
- Medical state changes require versioned migration and focused save tests.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- All five medical concerns have one owner.
- Current catalog counts and schemas are recorded.
- Uncertainty and consent are explicit.
- No parallel medical meter is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Build or verify a read-only diagnostic projection joining medical text to real disease/infection state.
- Expose honest uncertainty and test availability without creating a diagnosis authority.
- Connect taper, ward and palliative records through existing host/save owners.
- Audit consent, visitor capacity, memorial and final-wish boundaries end to end.

## MUST NOT DO

- No disease catalog expansion.
- No new pathogen or diagnosis system.
- No replacement mental-health authority.
- No clinical advice outside fictional game mechanics.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/DiseaseSystemTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`
4. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`
5. `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/AutopsyBridgeTests.cs`
6. `godot --headless --path . -- --data-integrity-selftest` only if a touched medical catalog changes.

## FIRST SAFE IMPLEMENTATION STEP

0 — owner census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: infection, spread, treatment response and immunity → DiseaseSystem; dependency, withdrawal and detox command state → ChemicalDependencySystem; clinical taper schedule and care posture → Dependency taper ledger; triage priority, isolation and surgical readiness → ClinicalWardTriageEngine; pain, lucidity, dignity and grief progression → PalliativeCareDignityEngine. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 09.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 09 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by DiseaseSystem or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

### `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1706 lines / 79732 bytes.
- SHA-256: `63336084c564afa0084a69c1bee665d9978f012387d6c26bdb58d0d7901bc8e6`.
- Architecture signals: seeded references=8; save/restore symbols=3; typed event declarations=14; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseIds
public const string ExpansionId = "expansion_disease_expansion";
public const string CatalogCollectionId = DiseaseCatalog.CollectionId;
public const string Cholera = "disease_cholera";
public const string ZoonoticFlu = "disease_zoonotic_flu";
public const string BloodFever = "disease_blood_fever";
public const string SporeBlight = "disease_spore_blight";
public const string TyphoidWaterborne = "disease_typhoid_waterborne";
public const string Dysentery = "disease_dysentery";
public const string EventInfection = "disease_infection";
public const string EventQuarantineStarted = "disease_quarantine_started";
public const string EventQuarantineEnded = "disease_quarantine_ended";
public const string EventOutbreakDeclared = "disease_outbreak_declared";
public const string EventOutbreakContained = "disease_outbreak_contained";
public const string EventRecovered = "disease_recovered";
public const string EventDied = "disease_death";
public const string EventProtocolApplied = "disease_protocol_applied";
public const string EventProtocolReset = "disease_protocol_reset";
public const string EventTreatmentApplied = "disease_treatment_applied";
public sealed class DiseaseInfectionState
public string survivor_id = string.Empty;
public int infected_day = 0;
public int days_sick = 0;
public bool quarantined = false;
public string current_stage = DiseaseStageNames.Incubating;
public int stage_entered_day = 0;
public int treatments_applied = 0;
public float lethality_reduction = 0f;
public int last_treatment_day = -1;
public bool is_diagnosed = false;
public sealed class DiseaseImmunityRecord
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public int immunity_until_day = 0;
public float strength = 1.0f;
public sealed class DiseaseExposureContext
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SourceId { get; set; } = string.Empty;
public float ProbabilityModifier { get; set; } = 1.0f;
public bool BypassImmunity { get; set; } = false;
public int Day { get; set; } = 0;
public sealed class DiseaseExposureResult
public bool Infected { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public float EffectiveProbability { get; set; }
public static DiseaseExposureResult CreateInfected(string survivorId, string diseaseId, float prob) =>
public static DiseaseExposureResult CreateBlocked(string reason, string survivorId, string diseaseId, float prob = 0f) =>
public sealed class DiseaseTreatmentResult
public bool Accepted { get; set; }
public string Reason { get; set; } = string.Empty;
public string Role { get; set; } = string.Empty;
public string ItemId { get; set; } = string.Empty;
public string DiseaseId { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public float LethalityReduction { get; set; }
public bool Cured { get; set; }
public static DiseaseTreatmentResult Refuse(string reason, string itemId, string diseaseId, string survivorId) =>
public static class DiseaseTreatmentRefusals
public const string NotPatient = "not_patient";
public const string UnknownDisease = "unknown_disease";
public const string NoTreatmentAuthorised = "no_treatment_authorised";
public const string ItemNotAuthorised = "item_not_authorised";
public const string OutsideWindow = "outside_window";
public const string AlreadyTreatedToday = "already_treated_today";
public const string NoSupplyChannel = "no_supply_channel";
public const string SupplyUnavailable = "supply_unavailable";
public sealed class DiseaseEntryState
public string disease_id = string.Empty;
public string vector_type = DiseaseVectorNames.Water;
public float spread_timer = 0f;
public bool outbreak_active = false;
public int deaths_during_outbreak = 0;
public int outbreaks_total = 0;
public int outbreaks_prevented = 0;
public int recovered_total = 0;
public int deaths_total = 0;
public int infections_total = 0;
public List<DiseaseInfectionState> infected = new List<DiseaseInfectionState>();
public sealed class DiseaseSystemState
public const int CurrentVersion = 2;
public int stateVersion = CurrentVersion;
public string system_id = DiseaseIds.ExpansionId;
public bool water_purified = false;
public bool vents_sealed = false;
public bool tools_sterilized = false;
public bool air_filtration = false;
public int water_purified_until_day = 0;
public int vents_sealed_until_day = 0;
public int tools_sterilized_until_day = 0;
public int air_filtration_until_day = 0;
public int rngSeed = 0;
public long rngPosition = 0;
public List<DiseaseEntryState> diseases = new List<DiseaseEntryState>();
public List<DiseaseImmunityRecord> immunities = new List<DiseaseImmunityRecord>();
public sealed class DiseasePatientSnapshot
public string survivor_id = string.Empty;
public string disease_id = string.Empty;
public string disease_name = string.Empty;
public int days_sick = 0;
public bool quarantined = false;
public bool contagious = false;          // past incubation, not isolated
public int contagion_risk_percent = 0;   // infectivity * 100
public int treatments_applied = 0;
public float effective_lethality = 0f;
public string current_stage = DiseaseStageNames.Incubating;
public string stage_token = "incubating";
public sealed class DiseaseSnapshot
public int total_infected = 0;
public int total_quarantined = 0;
public int total_contagious = 0;
public int total_outbreaks = 0;
public int total_outbreaks_prevented = 0;
public int total_recovered = 0;
public int total_deaths = 0;
public List<DiseasePatientSnapshot> patients = new List<DiseasePatientSnapshot>();
public sealed class DiseaseSystem
public const int DefaultSeed = 1013;
public const int OutbreakThreshold = 3;
public const float MaxLethalityReduction = 0.9f;
public event Action<string, string> OnInfection;                    // survivorId, diseaseId
public event Action<string, string> OnQuarantineStarted;            // survivorId, diseaseId
public event Action<string, string> OnQuarantineEnded;              // survivorId, diseaseId
public event Action<string> OnOutbreakDeclared;                     // diseaseId
public event Action<string, bool> OnOutbreakContained;              // diseaseId, prevented
public event Action<string, string, bool> OnOutcomeResolved;        // survivorId, diseaseId, recovered
public event Action<string, string, string, string, int>? OnTreatmentApplied;
public event Action<DiseaseSystemState> OnStateChanged;
public event Action<string, string> OnEventRaised;                  // eventId, detail
public Func<string, string, float>? EffectiveLethalityModifier;
public event Action<string, string>? OnStrainMutated;
public Func<string, float>? GetIsolationQuality;
public Func<float>? OnsetProbabilityMultiplier { get; set; }
public ContainmentCapability Containment { get; set; } = ContainmentCapability.None;
public DiseaseSystemState State => _state;
public DiseaseCatalog Catalog => _catalog;
public string SystemId => _state.system_id;
public bool HasImmunity(string survivorId, string diseaseId, int currentDay) {
public DiseaseImmunityRecord? GetImmunity(string survivorId, string diseaseId) {
public void SetImmunity(string survivorId, string diseaseId, int untilDay, float strength = 1.0f) {
public DiseaseExposureResult TryExpose(DiseaseExposureContext context) {
public DiseaseExposureResult TryInfect(string survivorId, string diseaseId, int day, string? sourceId = null) {
public void BindCatalog(DiseaseCatalog catalog) {
public DiseaseDefinition? GetDefinition(string diseaseId) {
public bool RegisterStrain(DiseaseDefinition strainDefinition) {
public void Infect(string survivorId, string diseaseId, int day) {
public event Action<string, string, string, int>? OnOutbreakTriggered;
public DiseaseOutbreakResult TriggerOutbreak( IDiseaseOutbreakSource source, string diseaseId, int day, IReadOnlyList<string>? candidates = null) {
public void TickDaily(int day, IReadOnlyList<string>? candidates = null) {
public void Quarantine(string survivorId, string diseaseId) {
public void EndQuarantine(string survivorId, string diseaseId) {
public bool MutateInfection(string survivorId, string fromDiseaseId, string toDiseaseId) {
public Func<string, int, bool>? TryConsumeItem;
public DiseaseTreatmentResult TryTreat( string survivorId, string diseaseId, string itemId, int day) {
public float GetEffectiveLethality(string survivorId, string diseaseId) {
public bool IsContagious(string survivorId, string diseaseId) {
public bool IsInfected(string survivorId, string diseaseId) {
public bool IsQuarantined(string survivorId, string diseaseId) {
public bool TryGetInfection(string survivorId, string diseaseId, out int daysSick, out bool quarantined) {
public bool Diagnose(string survivorId, string diseaseId) {
public bool IsDiagnosed(string survivorId, string diseaseId) {
public DiseaseClinicalPicture GetClinicalPicture(string survivorId, string diseaseId) {
public string GetTransmissionVector(string diseaseId) {
public bool IsVectorBlocked(string vectorType) {
public void PurifyWater(int day = 0) {
public void ResetWaterPurification() {
public void SealVents(int day = 0) {
public void ResetVentSeal() {
public void SterilizeTools(int day = 0) {
public void ResetToolSterilization() {
public void SetAirFiltration(bool active, int day = 0) {
public void TickProtocolExpiry(int day) {
public int ProtocolDaysRemaining(string vectorType, int today) {
public DiseaseSnapshot GetSnapshot() {
public DiseaseEntryState? GetDiseaseState(string diseaseId) {
public DiseaseSystemState CaptureState() {
public void RestoreState(DiseaseSystemState saved) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`

### `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 535 lines / 24991 bytes.
- SHA-256: `f40c6c58b6a183bd64d62ce19ef85dcf8b16ffbc29e335ef8f8e1d3658179249`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=25; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum ChemicalDependencyKind
public class ChemicalDependencyState
public string itemId = string.Empty;
public float dependencyLevel = 0f;      // 0..1
public string kind = "Opioid";
public bool inManagedDetox = false;
public bool inColdTurkey = false;       // Unity abused progress<0; we flag it (see migration notes)
public float detoxProgressHours = 0f;
public class ChemicalDependencyLedgerState
public string systemId = ChemicalDependencySystem.SystemId;
public List<SurvivorDependencyList> survivors = new List<SurvivorDependencyList>();
public class SurvivorDependencyList
public string survivorId = string.Empty;
public List<ChemicalDependencyState> dependencies = new List<ChemicalDependencyState>();
public class ChemicalDependencySystem
public const string SystemId = "chemical_dependency_system";
public const float DependencyThreshold = 0.3f;
public const float DependencyIncreasePerDose = 0.15f;
public const float DependencyDecayPerDayClean = 0.05f;
public const float MaxDependencyLevel = 1f;
public const float ColdTurkeyWithdrawalDurationHours = 72f;
public const float ManagedDetoxDurationHours = 120f;
public const float ColdTurkeyTremorCraftingPenalty = 0.40f;
public const float ColdTurkeyTremorCombatPenalty = 0.30f;
public const float ColdTurkeyMoraleDrainPerHour = 3f;
public const float ManagedDetoxMoraleDrainPerHour = 1f;
public const float DetoxSuccessThresholdHours = 96f;
public static readonly Dictionary<ChemicalDependencyKind, float> KindBaseSeverity = new Dictionary<ChemicalDependencyKind, float> {
public event Action<string, string> OnDependencyFormed;        // survivorId, itemId
public event Action<string, string> OnWithdrawalStarted;       // survivorId, itemId
public event Action<string, string> OnDetoxCompleted;          // survivorId, itemId
public event Action<string, string> OnDetoxFailed;             // survivorId, itemId
public event Action<string, float> OnMoraleDrainRequested;     // survivorId, amount
public event Action<string, float> OnCraftingPenaltyChanged;   // survivorId, factor
public event Action<string, float> OnCombatPenaltyChanged;     // survivorId, factor
public event Action<string, string, float> OnStressReported;   // survivorId, source, magnitude
public event Action<string, string, ChemicalDependencyKind> OnDependencyReFormedByStress;
public event Action OnStateChanged;
public IReadOnlyDictionary<string, List<ChemicalDependencyState>> Ledger => _ledger;
public void OnSubstanceConsumed(string survivorId, string itemId, ChemicalDependencyKind kind) {
public int ReportStress(string survivorId, string source, float magnitude) {
public bool BeginManagedDetox(string survivorId, string itemId) {
public bool BeginColdTurkey(string survivorId, string itemId) {
public CommandPreview PreviewBeginManagedDetox(string survivorId, string itemId, long stateVersion = 0) {
public CommandResult ExecuteBeginManagedDetox(string survivorId, string itemId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public CommandPreview PreviewBeginColdTurkey(string survivorId, string itemId, long stateVersion = 0) {
public CommandResult ExecuteBeginColdTurkey(string survivorId, string itemId, long expectedStateVersion = 0, long currentStateVersion = 0) {
public void TickHours(string survivorId, float gameHours, bool isStaffed = false, float staffSpeedMultiplier = 1.25f) {
public bool HasActiveWithdrawal(string survivorId) {
public float DependencyLevel(string survivorId, string itemId) {
public IReadOnlyList<ChemicalDependencyState> DependenciesFor(string survivorId) {
public ChemicalDependencyLedgerState CaptureState() {
public void RestoreState(ChemicalDependencyLedgerState saved) {
```


# Appendix B.04 — Current Code Architecture: `Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs`

### `Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 267 lines / 10581 bytes.
- SHA-256: `98a1e84e23c49134b17ff7cfb61526e673bc1176581b3a8fe411771ce929de79`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DependencyTaperState
public int SchemaVersion { get; set; } = 1;
public CarePolicyPosture CurrentPosture { get; set; } = CarePolicyPosture.Monitored;
public int SubstituteMedicineStockPermille { get; set; } = 1000;
public Dictionary<string, TaperProgramState> ActivePrograms { get; set; } =
public List<string> CompletedSurvivorIds { get; set; } = new List<string>();
public DependencyTaperState Clone() {
public struct DependencyTaperCensus
public int ActiveProgramsCount { get; }
public int CompletedProgramsCount { get; }
public int MedicallySupervisedCount { get; }
public int CriticalBurdenCount { get; }
public CarePolicyPosture CurrentPosture { get; }
public int SubstituteMedicineStockPermille { get; }
public sealed class DependencyTaperLedger
public IReadOnlyDictionary<string, TaperProgramState> ActivePrograms => _state.ActivePrograms;
public IReadOnlyList<string> CompletedSurvivorIds => _state.CompletedSurvivorIds;
public CarePolicyPosture CurrentPosture => _state.CurrentPosture;
public int SubstituteMedicineStockPermille => _state.SubstituteMedicineStockPermille;
public DependencyTaperState CaptureState() => _state.Clone();
public void RestoreState(DependencyTaperState? saved) {
public void SetPolicyPosture(CarePolicyPosture posture) {
public void SetSubstituteStock(int stockPermille) {
public bool TryGetProgram(string survivorId, out TaperProgramState? program) {
public TaperProgramState EnrollProgram( string survivorId, int dependencyPermille, bool isMedicallySupervised, int? dailyStepDown = null) {
public TaperDayResult AdvanceProgramDay(string survivorId, bool peerSupportRunToday) {
public CarePolicyPosture EvaluateCarePolicy(int totalShelterPopulation) {
public DependencyTaperCensus GetCensus() {
public void Clear() {
```


# Appendix B.05 — Current Code Architecture: `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`

### `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 307 lines / 13247 bytes.
- SHA-256: `400c8f9b33d692cd0fd0907e8cb2509d030534d7d1882894ae69f1575373556b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum TriagePriorityTier
public enum WardCleanlinessGrade
public sealed class ClinicalWardState
public string WardId                     { get; set; } = string.Empty;
public int TotalBeds                     { get; set; } = 10;
public int OccupiedBeds                  { get; set; } = 0;
public int SterileSupplyStockPermille    { get; set; } = 800;
public int StaffingReadinessPermille     { get; set; } = 750;
public WardCleanlinessGrade Cleanliness  { get; set; } = WardCleanlinessGrade.AntisepticStandard;
public int IsolationBedsTotal           { get; set; } = 2;
public int IsolationBedsOccupied         { get; set; } = 0;
public ClinicalWardState Clone() => new ClinicalWardState
public readonly struct TriageEvaluationResult
public TriagePriorityTier AssignedPriority    { get; }
public int EstimatedUrgencyMinutes            { get; }
public bool BedAvailable                      { get; }
public bool RequiresIsolation                 { get; }
public string RecommendationNotice            { get; }
public readonly struct SurgicalReadinessResult
public bool IsApprovedForSurgery               { get; }
public int ShockRiskPermille                   { get; }
public int InfectionRiskPermille               { get; }
public int SterileSuppliesConsumedPermille     { get; }
public string BottleneckReason                 { get; }
public static class ClinicalWardTriageEngine
public const int MinimumSterileStockForSurgeryPermille = 300;
public const int MinimumStaffingForSurgeryPermille = 500;
public static TriageEvaluationResult EvaluatePatientTriage( int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, ClinicalWardState ward) {
public static SurgicalReadinessResult EvaluateSurgicalPreparation( ClinicalWardState ward, int procedureComplexityPermille, int patientConditionPermille, int surgerySeed) {
public static int ComputeBedTurnoverCapacity(ClinicalWardState ward, int averageLengthOfStayDays) {
public static int CalculateNosocomialInfectionRisk( WardCleanlinessGrade cleanliness, int wardOccupancyPermille, int sterileSupplyPermille) {
```


# Appendix B.06 — Current Code Architecture: `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs`

### `Assets/Ashfall.Core/Medical/PalliativeCareDignityEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 255 lines / 10383 bytes.
- SHA-256: `d4091631c458e75e83d296a7f05a3840004870baec946886e1d16e6852244db1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum GriefStage
public enum PalliativeCareProtocol
public sealed class PalliativePatientRecord
public string SurvivorId { get; set; } = string.Empty;
public int DaysRemainingPrognosis { get; set; } = 7;
public int PainLevelPermille { get; set; } = 600; // 0..1000
public int LucidityPermille { get; set; } = 800;  // 0..1000
public int DignityIndexPermille { get; set; } = 750; // 0..1000
public PalliativeCareProtocol ActiveProtocol { get; set; } = PalliativeCareProtocol.BalancedAnalgesia;
public string FinalWishQuestId { get; set; } = string.Empty;
public bool FinalWishFulfilled { get; set; }
public GriefStage CurrentGriefStage { get; set; } = GriefStage.Denial;
public int DaysInCurrentGriefStage { get; set; }
public PalliativePatientRecord Clone() => new PalliativePatientRecord
public readonly struct DailyPalliativeOutcome
public int PainDeltaPermille { get; }
public int LucidityDeltaPermille { get; }
public int DignityDeltaPermille { get; }
public bool PrognosisExpired { get; }
public readonly struct MemorialLegacyEcho
public string SurvivorId { get; }
public int MoraleDelta { get; }
public string MemorialJournalKey { get; }
public bool DiedInDignity { get; }
public static class PalliativeCareDignityEngine
public const int HighDignityThresholdPermille = 700;
public static DailyPalliativeOutcome AdvanceDailyCare( PalliativePatientRecord patient, int medicineAvailabilityPermille, int caregiverSkillPermille) {
public static void EvaluateGriefStageProgression( PalliativePatientRecord patient, long simTick, int worldSeed) {
public static MemorialLegacyEcho CalculateMemorialEcho(PalliativePatientRecord deceased) {
```


# Appendix B.07 — Current Code Architecture: `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

### `Assets/Ashfall.Core/Medical/MedicalWardSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 362 lines / 13288 bytes.
- SHA-256: `03800899c4d6f2d88ea6c4fe7ee3606d480adafa42d898bcfe697a234dedb06d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalWardSystem
public event Action<MedicalWardEvent>? OnWardChanged;
public event Action<string>? OnPatientAdmitted;
public Func<bool>? StaffingPreflight { get; set; }
public IReadOnlyList<MedicalBed> Beds => _beds;
public IReadOnlyList<MedicalProcedureDef> Procedures => _procedures;
public MedicalWardState State => _state;
public MedicalWardAdmissionResult Admit(string patientId, string bedId, int day) {
public MedicalWardAdmissionResult Discharge(string patientId, int day) {
public MedicalWardProcedureResult RunProcedure(string patientId, string procedureId, int day) {
public MedicalWardState CaptureState() => _state.Capture();
public void RestoreState(MedicalWardState state) {
public string? GetBedOccupant(string bedId) {
public MedicalAdmissionRecord? GetActiveAdmission(string patientId) {
public sealed class MedicalBed
public string BedId;
public string DisplayName;
public MedicalBedCategory Category;
public bool Isolation;
public enum MedicalBedCategory
public sealed class MedicalProcedureDef
public string ProcedureId;
public string DisplayName;
public string DelegatedSystemId; // e.g. "MedicalSystem", "DoseLedgerSystem"
public Dictionary<string, int> SupplyCost = new Dictionary<string, int>();
public float DurationHours;
public sealed class MedicalAdmissionRecord
public string PatientId;
public string BedId;
public int AdmittedDay;
public int DischargedDay;
public MedicalAdmissionStatus Status;
public enum MedicalAdmissionStatus
public sealed class MedicalProcedureRecord
public string PatientId;
public string ProcedureId;
public string BedId;
public int Day;
public sealed class MedicalWardState
public List<MedicalAdmissionRecord> Admissions = new List<MedicalAdmissionRecord>();
public List<MedicalProcedureRecord> ProceduresRun = new List<MedicalProcedureRecord>();
public void NormalizeAndValidate(IReadOnlyList<MedicalBed> beds) {
public MedicalWardState Capture() => new MedicalWardState
public void RestoreInto(MedicalWardState state, IReadOnlyList<MedicalBed> beds) {
public enum MedicalWardEventKind
public sealed class MedicalWardEvent
public MedicalWardEventKind Kind;
public string PatientId;
public string BedId;
public int Day;
public string Detail;
public sealed class MedicalWardAdmissionResult
public bool Succeeded;
public string ReasonCode;
public MedicalAdmissionRecord Record;
public static MedicalWardAdmissionResult Ok(MedicalAdmissionRecord r) => new MedicalWardAdmissionResult { Succeeded = true, ReasonCode = "ok", Record = r };
public static MedicalWardAdmissionResult Fail(string reason) => new MedicalWardAdmissionResult { Succeeded = false, ReasonCode = reason ?? "fail", Record = null! };
public sealed class MedicalWardProcedureResult
public bool Succeeded;
public string ReasonCode;
public string ProcedureId;
public Dictionary<string, int> SupplyCost;
public static MedicalWardProcedureResult Ok(string procedureId, Dictionary<string, int> cost) => new MedicalWardProcedureResult
public static MedicalWardProcedureResult Fail(string reason) => new MedicalWardProcedureResult
```


# Appendix B.08 — Current Code Architecture: `src/Main.Medical.cs`

### `src/Main.Medical.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 651 lines / 31882 bytes.
- SHA-256: `56732cc5acbf9c9129f4159be2c23cb6164bf13e1a3b6b2521152b10fa90cf39`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=10; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public IReadOnlyList<string> GetActiveAfflictionIds(string survivorId) {
public bool IsQuestBlockedForSurvivor(string survivorId, string questTag, out List<string> blockingAfflictions) {
public IReadOnlyList<string> GetUnlockedQuestsForSurvivor(string survivorId) {
```


# Appendix B.09 — Current Code Architecture: `src/Host/MedicalHostSession.cs`

### `src/Host/MedicalHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 330 lines / 15025 bytes.
- SHA-256: `13551c202e601feb9bccb5fe5e029686e14857b6775e84527fc13ad66b55eda8`.
- Architecture signals: seeded references=0; save/restore symbols=9; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalHostSession
public ChemicalDependencySystem Engine { get; }
public VigilStateMachine Vigil { get; }
public MedicalPipelineCoordinator? Pipeline { get; private set; }
public AfflictionQuestWorkBridge Bridge { get; } = new AfflictionQuestWorkBridge();
public float TotalMoraleDrain { get; private set; }
public float ActiveCraftingPenalty { get; private set; }
public float ActiveCombatPenalty { get; private set; }
public string LastEvent { get; private set; } = string.Empty;
public void AddCareEntry(string survivorId, string treatmentDetails) {
public bool VigilActive => Vigil != null && Vigil.IsActive;
public float VigilProgress =>
public void BindVigilContext( Func<int> dayProvider, Ashfall.Core.Flags.IFlagLedger? flags, Func<IReadOnlyList<string>>? namesProvider = null) {
public string HoldVigil(string survivorId) {
public void TickVigil(double deltaSeconds) {
public static MedicalHostSession Create(string dataDir) {
public void LoadBridgeRules(string json) => Bridge.LoadCatalog(json);
public void BindPipeline(MedicalPipelineCoordinator pipeline) {
public MedicalPipelineSaveState? CapturePipelineSave() => Pipeline?.CaptureState();
public string BeginDetoxDemo(string survivorId, string itemId, bool managed) {
public void TickHours(float hours) {
public string StatusLine() {
public ChemicalDependencyLedgerState CaptureSave() => Engine.CaptureState();
public void RestoreSave(ChemicalDependencyLedgerState state) => Engine.RestoreState(state);
public string SkipVigilDemo() {
public string VigilStatusLine() {
public VigilSaveState CaptureVigilSave() => Vigil.CaptureState();
public void RestoreVigilSave(VigilSaveState state) => Vigil.RestoreState(state);
```


# Appendix B.10 — Current Code Architecture: `src/UI/AfflictionsPanel.cs`

### `src/UI/AfflictionsPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 488 lines / 23541 bytes.
- SHA-256: `189133c74fdd0eff3dc5e7bc8aa518c8cef986ef5e05c8fa49d900f3219bcccf`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class AfflictionsPanel : Control
public event Action? OnClose;
public bool IsBound { get; private set; }
public int RenderedActiveCount { get; private set; }
public void Bind( MedicalHostSession? medical = null, SurvivorsHostSession? survivors = null, InventoryHostSession? inventory = null, RespiratoryDegenerationSystem? respiratory = null, MedicalTextCatalog? medicalTexts = null)
public override void _Ready() {
public void RefreshView() {
public void Open() {
public override void _UnhandledInput(InputEvent @event) {
```


# Appendix B.11 — Current Code Architecture: `src/UI/ChemicalDependencyPanel.cs`

### `src/UI/ChemicalDependencyPanel.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 412 lines / 21721 bytes.
- SHA-256: `06b97e99a590e6be9a4a4a132cc9dfca51204dd40340fbeec2267ba040732fa4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=3; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class ChemicalDependencyPanel : Control, IBindablePanel
public event Action? OnClose;
public bool IsBound => _host != null;
public void Bind(ChemicalDependencyHostSession session) {
public void Unbind() {
public override void _Ready() {
public void Open() {
public void RefreshView() {
public override void _UnhandledInput(InputEvent @event) {
public override void _ExitTree() {
```


# Appendix C.12 — Catalog Census: `Assets/StreamingAssets/Data/disease_catalog.json`

### `Assets/StreamingAssets/Data/disease_catalog.json`

- Parse: valid strict JSON.
- Schema version: `3`.
- Size: 54811 bytes / 54809 characters.
- SHA-256: `f6c2369fc2adf26f8e05fbd6e3c6e929f0d5023bbeded1f8b5aed5129e472fdd`.
- Root keys: `collection_id`, `diseases`, `exposure_sources`, `schema_version`, `vector_protocols`.

Array-path census (minimum, maximum, observed rows):

```text
diseases: min=20, max=20, observed_paths=1
diseases[].phases: min=4, max=4, observed_paths=2
diseases[].phases[].symptom_tags: min=1, max=3, observed_paths=4
diseases[].treatments: min=2, max=2, observed_paths=2
exposure_sources: min=5, max=5, observed_paths=1
vector_protocols: min=4, max=4, observed_paths=1
```

Representative record fields:

- `countermeasure_item_id`
- `display_name`
- `guidance`
- `id`
- `illness_days`
- `immunity_duration_days`
- `immunity_strength`
- `incubation_days`
- `infectivity`
- `lethality`
- `phases`
- `source_note`
- `spread_interval_days`
- `spread_radius`
- `tell`
- `tell_secondary`
- `timing_clue`
- `treatments`
- `vector`

Representative identifiers (ordered, capped for readability):

```text
disease_cholera
disease_zoonotic_flu
disease_blood_fever
disease_spore_blight
disease_acute_radiation_syndrome
disease_fungal_respiratory
disease_typhoid_waterborne
disease_wellspring_cramps
disease_silt_jaundice
disease_condemned_air_cough
disease_dry_bunker_hiss
disease_septic_rust_wound_fever
disease_reused_needle_fever
disease_deep_excavation_mold_lung
disease_silo_lung
disease_prion_tremor
disease_dysentery
disease_meningococcal_fever
disease_bloodborne_hepatitis
disease_spore_wound_dermatitis
```


# Appendix C.13 — Catalog Census: `Assets/StreamingAssets/Data/medical_texts.json`

### `Assets/StreamingAssets/Data/medical_texts.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 222244 bytes / 222228 characters.
- SHA-256: `867ac96ebabc9c40db200106c84a8f8fd2e0f5c21650a737aa3678312d548347`.
- Root keys: `collection_id`, `conditions`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
conditions: min=83, max=83, observed_paths=1
conditions[].complication_warnings: min=3, max=3, observed_paths=2
conditions[].failure_consequences: min=3, max=3, observed_paths=2
conditions[].long_term_effects: min=3, max=3, observed_paths=2
conditions[].pain_descriptions: min=3, max=3, observed_paths=2
conditions[].prevention_advice: min=3, max=3, observed_paths=2
conditions[].recovery_descriptions: min=3, max=3, observed_paths=2
conditions[].required_items: min=2, max=3, observed_paths=2
conditions[].symptom_descriptions: min=3, max=3, observed_paths=2
conditions[].treatment_steps: min=4, max=4, observed_paths=2
```

Representative record fields:

- `category`
- `complication_warnings`
- `diagnosis_text`
- `display_name`
- `emotional_impact`
- `failure_consequences`
- `id`
- `long_term_effects`
- `mental_state`
- `pain_descriptions`
- `physical_state`
- `prevention_advice`
- `recovery_descriptions`
- `required_items`
- `success_chances`
- `symptom_descriptions`
- `system_integration`
- `treatment_steps`

Representative identifiers (ordered, capped for readability):

```text
medical_laceration
medical_burn
medical_fracture
medical_radiation_exposure
medical_infection
medical_dehydration
medical_starvation
medical_hypothermia
medical_heatstroke
medical_panic_attack
medical_insomnia
medical_wound_infection
medical_frostbite
medical_concussion
medical_dehydration_severe
medical_amputation
medical_suturing
medical_chronic_radiation
medical_chronic_illness
medical_addiction
medical_ptsd
medical_depression
medical_wound_care
medical_hygiene
medical_nutrition
medical_rest
medical_toothache
medical_eye_infection
medical_pregnancy
medical_childbirth
medical_newborn_care
medical_broken_nose
medical_dislocated_shoulder
medical_sprained_wrist
medical_bruised_ribs
medical_cuts_and_scrapes
medical_blister_care
medical_food_poisoning
medical_chemical_poisoning
medical_allergic_reaction
medical_insect_bite
medical_animal_bite
medical_snake_bite
medical_spider_bite
medical_dog_bite
medical_cat_scratch
medical_horse_kick
medical_bone_setting
medical_wound_debridement
medical_physical_therapy
medical_counseling
medical_medication_management
medical_grief_counseling
medical_trauma_therapy
medical_stress_management
medical_diabetes
medical_asthma
medical_arthritis
medical_heart_disease
medical_cancer
medical_epilepsy
medical_chronic_pain
medical_chronic_fatigue
medical_chronic_headache
medical_chronic_back_pain
medical_malaria
medical_tapeworm
medical_scurvy
medical_beriberi
medical_pellagra
medical_rickets
medical_anemia
medical_goiter
medical_deep_wound
medical_puncture_wound
medical_avulsion
medical_incision
medical_laceration_deep
medical_burn_first_degree
medical_burn_second_degree
medical_burn_third_degree
medical_chemical_burn
medical_electrical_burn
```


# Appendix C.14 — Catalog Census: `Assets/StreamingAssets/Data/recipes_cooking.json`

### `Assets/StreamingAssets/Data/recipes_cooking.json`

- Parse: valid strict JSON.
- Schema version: `1`.
- Size: 9228 bytes / 9228 characters.
- SHA-256: `7ccd193c0c2812984e545a45754c3d00f2f4dfe69475078321d7c4e1aaa29100`.
- Root keys: `recipes`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
recipes: min=15, max=15, observed_paths=1
recipes[].inputItems: min=1, max=2, observed_paths=2
```

Representative record fields:

- `cookTimeMinutes`
- `description`
- `displayName`
- `id`
- `inputItems`
- `moraleBonus`
- `nutritionValue`
- `outputItemId`
- `outputQuantity`
- `radiationRemoval`
- `requiredEquipment`
- `shelfLifeDays`

Representative identifiers (ordered, capped for readability):

```text
recipe_roasted_meat
recipe_boiled_meat_broth
recipe_hardwood_smoked_strips
recipe_boiled_frost_tubers
recipe_vegetable_potage
recipe_ash_grain_flatbread
recipe_canned_grain_stew
recipe_pemmican_travel_block
recipe_travel_field_ration
recipe_dry_salted_meat
recipe_brined_iron_pea_paste
recipe_fermented_winter_cress
recipe_fat_sealed_confit
recipe_industrial_bulk_stew
recipe_phosphor_mushroom_infusion
```


# Appendix D.15 — Existing Focused Test Inventory: `Ashfall.Core.Tests/DiseaseSystemTests.cs`

### `Ashfall.Core.Tests/DiseaseSystemTests.cs`

- Current test declarations: Fact=16, Theory=0, InlineData=0.
- File lines: 502; SHA-256: `e04281bcfbedf6e90537be62b62463851fb644dad3902c7fbb3c6ea52c7e4483`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Catalog_LoadsFromDataDirectory_WithCanonicalIds
UnknownVector_DefaultsToWater_AndUnknownDiseaseIsRejected
ThreeActiveInfections_DeclareOutbreak
QuarantineWard_StopsSpread_AndContainmentPreventsTheOutbreak
Quarantine_AppliesPerDisease_PerSurvivor
WaterProtocol_BlocksWaterborneSpread_AndResetRestoresTheThreat
EveryVector_HasALivableProtocol
SameSeed_ProducesIdenticalOutbreaks
OutcomeRolls_ProduceBothRecoveryAndDeath_Deterministically
Save_ExpansionHubEnvelopeRoundTrips_DiseaseState
Save_TamperWithDiseasePayload_FailsChecksum
V3Save_MigratesForward_WithFreshWard
Restore_ResumesSameRngStream_AsATwin
DiseaseCatalog_IntroducesNoDataIntegrityErrors
CaptureState_ReturnsIndependentClone_NotLiveAlias
JsonRoundTripSnapshot_RestoresExactPreDayState_AfterMutation
```


# Appendix D.16 — Existing Focused Test Inventory: `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`

### `Ashfall.Core.Tests/ChemicalDependencySystemTests.cs`

- Current test declarations: Fact=13, Theory=0, InlineData=0.
- File lines: 202; SHA-256: `955e87ae46881175bfb243022955c4975f8e61ab2230c9db3f9eb4217ac10ee7`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Consumption_FormsDependencyAtThreshold
Consumption_FirstDoseCreatesEntry
Consumption_UnknownSurvivorIgnored
ManagedDetox_CompletesAfterThresholdHours
ManagedDetox_BelowThresholdRefused
ColdTurkey_AppliesPenaltiesAndCompletes
CleanDecay_RemovesDependency
Severity_ScalesMoraleDrainByKind
ProgramSwitch_ColdTurkeyToManagedUsesManagedProfile
CaptureState_ReturnsSnapshotNotLiveState
CaptureState_EmitsInOrdinalOrder
SaveLoad_RoundTripsAllState
SaveLoad_ChecksumStable
```


# Appendix D.17 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`

### `Ashfall.Core.Tests/Medical/PalliativeCareDignityEngineTests.cs`

- Current test declarations: Fact=4, Theory=0, InlineData=0.
- File lines: 95; SHA-256: `73f88be9ba46b7c925a579d0e508b38af7c9fc6215d83ae67a30fa20de4c5e75`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AdvanceDailyCare_ReducesPainAndCalculatesDignity
EvaluateGriefStageProgression_AdvancesTowardAcceptance_WhenDignityHigh
CalculateMemorialEcho_HighDignityAndWishFulfilled_GrantsMoraleBuff
CalculateMemorialEcho_AgonizingNeglect_ImposesMoralePenalty
```


# Appendix D.18 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`

### `Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 113; SHA-256: `97d3443e682ed2cba087751cb9cfec0ef70188934ce8109198720237145972b8`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
EvaluatePatientTriage_AssignsImmediate_AndRoutesIsolationWhenContagious
EvaluateSurgicalPreparation_DeniesSurgery_WhenWardContaminated
EvaluateSurgicalPreparation_ApprovesAndConsumesSupplies_WhenSterileReady
ComputeBedTurnoverCapacity_MatchesExpectedMonthlyTurns
CalculateNosocomialInfectionRisk_RisesWithCrowdingAndContamination
```


# Appendix E.19 — Supporting Code Evidence: `src/Main.ClinicalWardTriage.cs`

### `src/Main.ClinicalWardTriage.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 117 lines / 4233 bytes.
- SHA-256: `50dacd08321b5c2e6210c5d390ee9dd4d8c172bcb087e6180b9880505c0f3074`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ClinicalWardTriageHostSession? ClinicalWardTriage => _clinicalWardTriage;
public void SetupClinicalWardTriage() {
public TriageEvaluationResult TriageClinicalPatient( string patientId, int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, int currentDay)
public SurgicalReadinessResult PreflightAndExecuteSurgery( string procedureId, string patientId, int procedureComplexityPermille, int patientConditionPermille, int currentDay,
public bool DischargeClinicalPatient(string patientId) {
public void RestockWardSterileSupplies(int amountPermille) {
public void RunWardAutoclaveCycle() {
public void SetWardCleanlinessGrade(WardCleanlinessGrade grade) {
public void SetWardStaffingReadiness(int staffingPermille) {
public void AdvanceClinicalWardDay(int currentDay, int seed) {
public ClinicalWardCensus GetClinicalWardCensus() =>
public void SaveClinicalWardTriage() {
public void FlushClinicalWardTriageIfDirty() {
public void ResetClinicalWardTriage() {
```


# Appendix E.20 — Supporting Code Evidence: `src/Main.DependencyTaperWithdrawal.cs`

### `src/Main.DependencyTaperWithdrawal.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 98 lines / 3751 bytes.
- SHA-256: `a1f79949e0710fe6ad8b2834bf11f0d58eb646cdc26b1353349c7c2007b5cfd2`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public DependencyTaperWithdrawalHostSession? DependencyTaper => _dependencyTaper;
public void SetupDependencyTaperWithdrawal() {
public TaperProgramState EnrollDependencyTaper( string survivorId, int dependencyPermille, bool isMedicallySupervised, int? dailyStepDown = null) {
public TaperDayResult AdvanceDependencyTaperDay(string survivorId, bool peerSupportRunToday) {
public void SetDependencyCarePosture(CarePolicyPosture posture) {
public void SetDependencySubstituteStock(int stockPermille) {
public CarePolicyPosture EvaluateDependencyCarePolicy(int totalShelterPopulation) {
public DependencyTaperCensus GetDependencyTaperCensus() =>
public void SaveDependencyTaperWithdrawal() {
public void FlushDependencyTaperWithdrawalIfDirty() {
public void ResetDependencyTaperWithdrawal() {
```


# Appendix E.21 — Supporting Code Evidence: `src/Host/HostCli.ClinicalWardTriage.cs`

### `src/Host/HostCli.ClinicalWardTriage.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 252 lines / 13577 bytes.
- SHA-256: `2c30069efbb727aa7e96718be16792566d63abfbd70e4e3d7a569bb30558e136`.
- Architecture signals: seeded references=0; save/restore symbols=1; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HostCliClinicalWardTriage
public static int RunSelfTest(string dataDir) {
```


# Appendix E.22 — Supporting Code Evidence: `Assets/Ashfall.Core/Medical/ClinicalWardLedger.cs`

### `Assets/Ashfall.Core/Medical/ClinicalWardLedger.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 399 lines / 15909 bytes.
- SHA-256: `cd6285e1673a1e8a9a5a3d953eaefcf269be1713dc6fc744cce90017ab283393`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TriageAdmissionRecord
public string PatientSurvivorId { get; set; } = string.Empty;
public int AdmissionDay { get; set; } = 1;
public TriagePriorityTier Priority { get; set; } = TriagePriorityTier.Delayed;
public bool IsContagious { get; set; } = false;
public int EstimatedUrgencyMinutes { get; set; } = 60;
public bool InIsolation { get; set; } = false;
public int DaysInWard { get; set; } = 0;
public bool IsDischarged { get; set; } = false;
public TriageAdmissionRecord Clone() => new TriageAdmissionRecord
public sealed class SurgicalProcedureRecord
public string ProcedureId { get; set; } = string.Empty;
public string PatientSurvivorId { get; set; } = string.Empty;
public int Day { get; set; } = 1;
public bool WasApproved { get; set; } = false;
public int ShockRiskPermille { get; set; } = 0;
public int InfectionRiskPermille { get; set; } = 0;
public int SterileSuppliesConsumedPermille { get; set; } = 0;
public string BottleneckReason { get; set; } = string.Empty;
public SurgicalProcedureRecord Clone() => new SurgicalProcedureRecord
public sealed class ClinicalWardTriageState
public int SchemaVersion { get; set; } = 1;
public ClinicalWardState Ward { get; set; } = new ClinicalWardState();
public Dictionary<string, TriageAdmissionRecord> ActiveAdmissions { get; set; } =
public List<SurgicalProcedureRecord> SurgicalHistory { get; set; } = new List<SurgicalProcedureRecord>();
public int AutoclaveCycleCount { get; set; } = 0;
public int TotalPatientsTriaged { get; set; } = 0;
public int TotalSurgeriesPerformed { get; set; } = 0;
public int NosocomialInfectionsContracted { get; set; } = 0;
public ClinicalWardTriageState Clone() {
public struct ClinicalWardCensus
public int TotalBeds;
public int OccupiedBeds;
public int AvailableBeds;
public int IsolationBedsTotal;
public int IsolationBedsOccupied;
public int AvailableIsolationBeds;
public WardCleanlinessGrade Cleanliness;
public int SterileSupplyStockPermille;
public int StaffingReadinessPermille;
public int ActiveAdmissionsCount;
public int SurgeriesPerformedCount;
public int ProjectedNosocomialRiskPermille;
public sealed class ClinicalWardLedger
public ClinicalWardTriageState State { get; private set; }
public TriageEvaluationResult TriageAndAdmitPatient( string patientId, int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, int currentDay)
public SurgicalReadinessResult PreflightAndExecuteSurgery( string procedureId, string patientId, int procedureComplexityPermille, int patientConditionPermille, int currentDay,
public bool DischargePatient(string patientId) {
public void RestockSterileSupplies(int amountPermille) {
public void RunAutoclaveCycle() {
public void SetWardCleanliness(WardCleanlinessGrade grade) {
public void SetStaffingReadiness(int staffingPermille) {
public void AdvanceDay(int currentDay, int seed) {
public ClinicalWardCensus GetCensus() {
public ClinicalWardTriageState CaptureState() => State.Clone();
public void RestoreState(ClinicalWardTriageState? state) {
```


# Appendix E.23 — Supporting Code Evidence: `src/Host/DependencyTaperWithdrawalHostSession.cs`

### `src/Host/DependencyTaperWithdrawalHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 113 lines / 5009 bytes.
- SHA-256: `3c474c2186fa57494af189d3cc5c1e624c978787e84463b0dce253f13a4dbb69`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DependencyTaperWithdrawalSaveStore
public const string FileName = "dependency_taper_withdrawal_save.json";
public const string SectionName = "dependency_taper_withdrawal";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCapturePersisted(DependencyTaperState state) => s_store.CaptureBare(state);
public static DependencyTaperState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(DependencyTaperState state) => s_store.TrySave(state);
public static DependencyTaperState? TryLoad() => s_store.TryLoad();
public sealed class DependencyTaperWithdrawalHostSession : HostSessionBase
public DependencyTaperLedger Ledger => _ledger;
public DependencyTaperCensus Census => _ledger.GetCensus();
public CarePolicyPosture CurrentPosture => _ledger.CurrentPosture;
public int SubstituteMedicineStockPermille => _ledger.SubstituteMedicineStockPermille;
public string LastEvent { get; private set; } = string.Empty;
public static DependencyTaperWithdrawalHostSession Create(DependencyTaperState? state = null) =>
public TaperProgramState EnrollProgram( string survivorId, int dependencyPermille, bool isMedicallySupervised, int? dailyStepDown = null) {
public TaperDayResult AdvanceProgramDay(string survivorId, bool peerSupportRunToday) {
public void SetPolicyPosture(CarePolicyPosture posture) {
public void SetSubstituteStock(int stockPermille) {
public CarePolicyPosture EvaluateCarePolicy(int totalShelterPopulation) =>
public DependencyTaperState CaptureState() => _ledger.CaptureState();
public void RestoreState(DependencyTaperState? state) => _ledger.RestoreState(state);
public void Clear() {
```


# Appendix G.24 — Supporting Regression Evidence: `Ashfall.Core.Tests/Medical/ChemicalDependencyStressRelapseTests.cs`

### `Ashfall.Core.Tests/Medical/ChemicalDependencyStressRelapseTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 278; SHA-256: `d1d2f58b56bf9871216a87c565b8cd16d82a84ed8cd0e358ef5ac37bb2fcdc2d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ComputeDelta_ThresholdAndKindTable_ReturnsExpectedClampedSeverityScaled
ComputeDelta_IsHostStable_BothSidesSeeSameResult
IsReportable_CheapestFilterForPreCallGating
ReportStress_ReturnsZero_WhenNoLedgerEntry
ReportStress_AlwaysRaisesOnStressReported_OncePerCall
ReportStress_NudgesActiveDependency_PerKindDelta
ReportStress_DoesNotTouch_DependenciesInDetoxPrograms
ReportStress_RespectsSaturationFloor_NeverExceedsMax
ReportStress_BelowThresholdMagnitude_ReportsButDoesNotMutate
ReportStress_OnCrossUp_FiresOnDependencyReFormedByStress
DependencyLedger_Capture_Restore_StableAcrossStressCall
```


# Appendix G.25 — Supporting Regression Evidence: `Ashfall.Core.Tests/Medical/ChemicalDependencyVerticalSliceTests.cs`

### `Ashfall.Core.Tests/Medical/ChemicalDependencyVerticalSliceTests.cs`

- Current test declarations: Fact=15, Theory=0, InlineData=0.
- File lines: 311; SHA-256: `f3c3466f8d7b3b938c3e8064c9c01b5556983f95c4c59edd6dccdbbb23399835`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ManagedDetox_MatchesLegacyDirectCall_Exactly
ColdTurkey_MatchesLegacyDirectCall_Exactly
MissingTargetItem_IsRejected
UnknownSubstance_MissingDependency
BelowThreshold_Blocked
AlreadyInTreatment_Blocked
SwitchFromManagedToColdTurkey_MatchesDomainSemantics
ForeignTreatmentViaTarget_Rejected
TwoSubstances_StartingOneLeavesOtherUntouched
NoSecondClock_ScheduleStaysEmpty
DomainClock_AloneAdvancesDetox
Preview_DoesNotMutateLedger
Episode_SurfacesHighestDependency_OnlyWhenFormed
Episode_ReflectsActiveProgram
DiagnosisStore_PlaysNoRole
```


# Appendix G.26 — Supporting Regression Evidence: `Ashfall.Core.Tests/ChemicalDependencyCommandTests.cs`

### `Ashfall.Core.Tests/ChemicalDependencyCommandTests.cs`

- Current test declarations: Fact=7, Theory=0, InlineData=0.
- File lines: 104; SHA-256: `9650386eb54038bb4c78aa493df0de7ad76d0703f0b5a8f2deecfdc4e66161b3`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
PreviewBeginManagedDetox_Available_WhenDependencyExists
PreviewBeginManagedDetox_Unavailable_WhenNoDependency
ExecuteBeginManagedDetox_StalePreview_RejectsWithoutMutation
ExecuteBeginManagedDetox_MatchingVersions_StartsDetox
PreviewBeginColdTurkey_Available_WhenDependencyExists
ExecuteBeginColdTurkey_StalePreview_RejectsWithoutMutation
ExecuteBeginColdTurkey_MatchingVersions_StartsColdTurkey
```


# Appendix G.27 — Supporting Regression Evidence: `Ashfall.Core.Tests/Medical/DependencyTaperWithdrawalEngineTests.cs`

### `Ashfall.Core.Tests/Medical/DependencyTaperWithdrawalEngineTests.cs`

- Current test declarations: Fact=5, Theory=0, InlineData=0.
- File lines: 111; SHA-256: `dfa3da792a9901a4821b7ef93d064cd83496d5ba04a794d6efc4db8d540ccb11`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ComputeRecommendedStepDown_Slow_ForCriticalDependency
ComputeRecommendedStepDown_Faster_WhenMedicineStockLow
AdvanceTaperDay_CompletesProgram_AfterSufficientStepDowns
AdvanceTaperDay_ReducesSymptoms_WithPeerSupport
RecommendCarePolicy_Emergency_WhenCriticalBurdenHigh
```


# Appendix H.28 — Supporting Authority Document: `docs/MEDICAL_PIPELINE_JOURNEY.md`

### `docs/MEDICAL_PIPELINE_JOURNEY.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 30 lines / 1555 bytes.
- SHA-256: `b046e2b8054f355ae4fba11dfaa35a3b7dfb8d0abc9e25fdc60ab464f81d465e`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.29 — Supporting Authority Document: `docs/MEDICAL_DOSE_TREATMENT_MATRIX.md`

### `docs/MEDICAL_DOSE_TREATMENT_MATRIX.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 17 lines / 1427 bytes.
- SHA-256: `aecd9cb61064b72176dcf9231fe49c048c69669f9cdc0e9f070f7f7d87546961`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.30 — Supporting Authority Document: `docs/medical/AUTOPSY_RUNTIME_CONTRACT.md`

### `docs/medical/AUTOPSY_RUNTIME_CONTRACT.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 56 lines / 3435 bytes.
- SHA-256: `2c1506a80b438f4b715723731882b246441b6e87c714817ff1f6f937dcd37553`.
- Architecture signals: seeded references=1; save/restore symbols=2; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| infection, spread, treatment response and immunity | DiseaseSystem | dependency, withdrawal and detox command state | ChemicalDependencySystem | Owner emits/reads a typed fact; no mirror state. |
| infection, spread, treatment response and immunity | DiseaseSystem | clinical taper schedule and care posture | Dependency taper ledger | Owner emits/reads a typed fact; no mirror state. |
| infection, spread, treatment response and immunity | DiseaseSystem | triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | Owner emits/reads a typed fact; no mirror state. |
| infection, spread, treatment response and immunity | DiseaseSystem | pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | Owner emits/reads a typed fact; no mirror state. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | infection, spread, treatment response and immunity | DiseaseSystem | Owner emits/reads a typed fact; no mirror state. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | clinical taper schedule and care posture | Dependency taper ledger | Owner emits/reads a typed fact; no mirror state. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | Owner emits/reads a typed fact; no mirror state. |
| dependency, withdrawal and detox command state | ChemicalDependencySystem | pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | Owner emits/reads a typed fact; no mirror state. |
| clinical taper schedule and care posture | Dependency taper ledger | infection, spread, treatment response and immunity | DiseaseSystem | Owner emits/reads a typed fact; no mirror state. |
| clinical taper schedule and care posture | Dependency taper ledger | dependency, withdrawal and detox command state | ChemicalDependencySystem | Owner emits/reads a typed fact; no mirror state. |
| clinical taper schedule and care posture | Dependency taper ledger | triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | Owner emits/reads a typed fact; no mirror state. |
| clinical taper schedule and care posture | Dependency taper ledger | pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | Owner emits/reads a typed fact; no mirror state. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | infection, spread, treatment response and immunity | DiseaseSystem | Owner emits/reads a typed fact; no mirror state. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | dependency, withdrawal and detox command state | ChemicalDependencySystem | Owner emits/reads a typed fact; no mirror state. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | clinical taper schedule and care posture | Dependency taper ledger | Owner emits/reads a typed fact; no mirror state. |
| triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | Owner emits/reads a typed fact; no mirror state. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | infection, spread, treatment response and immunity | DiseaseSystem | Owner emits/reads a typed fact; no mirror state. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | dependency, withdrawal and detox command state | ChemicalDependencySystem | Owner emits/reads a typed fact; no mirror state. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | clinical taper schedule and care posture | Dependency taper ledger | Owner emits/reads a typed fact; no mirror state. |
| pain, lucidity, dignity and grief progression | PalliativeCareDignityEngine | triage priority, isolation and surgical readiness | ClinicalWardTriageEngine | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Build or verify a read-only diagnostic projection joining medical text to real disease/infection state. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Expose honest uncertainty and test availability without creating a diagnosis authority. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Connect taper, ward and palliative records through existing host/save owners. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Audit consent, visitor capacity, memorial and final-wish boundaries end to end. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

Every requirement in the objective and delta must resolve to at least one current owner, one negative condition and one future focused verification. A requirement with no owner is removed or returned as `STALE_PLAN`.

# Appendix K — Status and Evidence Labels

| Claim type | Label | Meaning |
| --- | --- | --- |
| Current source | VERIFIED PATH INVENTORY | Appendices hash current files; declarations are read-only. |
| Historical completion | HISTORICAL, NOT FRESH TEST PROOF | Focused tests must be rerun by an implementation/verification package. |
| Proposed types/paths | PROPOSAL ONLY | Never shown as current evidence; requires a new claim. |
| Character count | COMPLETENESS CHECK ONLY | External verifier records it; no padding or repeated boilerplate. |

# Appendix L — Plan Maintenance and Re-Audit Triggers

Re-run the premise sweep when any of the following occurs:

1. A listed Core owner is renamed, split, merged or removed.
2. A listed catalog changes `schema_version`, root shape or consumer.
3. A save section, checksum contract or campaign-day order changes.
4. A listed test is removed, renamed or moved to quarantine.
5. A live ledger marks a surface sealed, retired, accepted or blocked.
6. A generated architecture/save/catalog matrix changes the owner relationship.
7. A new active claim touches any current or proposed path.

The re-audit records only changed evidence. Historical prose is not rewritten merely to appear current, and current evidence is not deleted merely because an old plan disagrees with it.

# Appendix M — Definition of a Safe No-Change Result

A safe no-change result is valid when the current implementation already satisfies the requested behavior. It records: current owner paths, focused tests that exist, any rerun performed by a future verification package, and the precise condition that would justify reopening. A no-change result does not create a placeholder subsystem, a synthetic integration framework, or a test solely to increase counts.

# Appendix N — Final Precision Checklist

- [ ] Every current path in this document exists or is explicitly labeled unavailable.
- [ ] Every proposed path/type is labeled `PROPOSAL` and excluded from current claims.
- [ ] No current owner is duplicated.
- [ ] Core architecture remains engine-free.
- [ ] JSON is described as data authority, not automatic reachability.
- [ ] Save impact names the current owner and migration behavior.
- [ ] Determinism names streams or explicitly states no randomness.
- [ ] UI remains presentation over owner commands.
- [ ] Tests are focused and current commands use `scripts/run_test.sh` policy.
- [ ] Sealed/retired/blocked ledger decisions are respected.
- [ ] No full-suite result is claimed without a dedicated execution window.
- [ ] No Unity dependency or historical architecture is proposed.
- [ ] Character count is not used as evidence of quality.
- [ ] The first implementation step is a premise recheck, not code creation.
- [ ] The implementation handoff can be executed without reinterpreting ownership.

# Appendix O — Handoff Record Template

```text
Package:
Current status rechecked:
Outcome implemented:
Files changed:
Current owner contract used:
Save section/version touched:
Determinism streams touched:
Focused verification commands and results:
Tests reused/added:
Known limitation or debt:
Shared files intentionally untouched:
Ready for independent sweep: yes/no
```


# Appendix Q.562 — Additional Current Architecture Evidence: `src/Host/ClinicalWardTriageHostSession.cs`

### `src/Host/ClinicalWardTriageHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 141 lines / 6015 bytes.
- SHA-256: `c1379f6ca459ecc4b8d16cd17b99aadb0f8a9bc2b6c18dc7daeec61c1d2a0880`.
- Architecture signals: seeded references=0; save/restore symbols=4; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ClinicalWardTriageSaveStore
public const string FileName = "clinical_ward_triage_save.json";
public const string SectionName = "clinical_ward_triage";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCapturePersisted(ClinicalWardTriageState state) => s_store.CaptureBare(state);
public static ClinicalWardTriageState? TryRestorePersisted(string json) => s_store.RestoreBare(json);
public static bool TrySave(ClinicalWardTriageState state) => s_store.TrySave(state);
public static ClinicalWardTriageState? TryLoad() => s_store.TryLoad();
public sealed class ClinicalWardTriageHostSession : HostSessionBase
public ClinicalWardLedger Ledger => _ledger;
public ClinicalWardCensus Census => _ledger.GetCensus();
public string LastEvent { get; private set; } = string.Empty;
public static ClinicalWardTriageHostSession Create(ClinicalWardTriageState? state = null) =>
public TriageEvaluationResult TriageAndAdmitPatient( string patientId, int traumaSeverityPermille, int vitalStabilityPermille, bool isContagious, int currentDay)
public SurgicalReadinessResult PreflightAndExecuteSurgery( string procedureId, string patientId, int procedureComplexityPermille, int patientConditionPermille, int currentDay,
public bool DischargePatient(string patientId) {
public void RestockSterileSupplies(int amountPermille) {
public void RunAutoclaveCycle() {
public void SetWardCleanliness(WardCleanlinessGrade grade) {
public void SetStaffingReadiness(int staffingPermille) {
public void AdvanceDay(int currentDay, int seed) {
public ClinicalWardTriageState CaptureState() => _ledger.CaptureState();
public void RestoreState(ClinicalWardTriageState? state) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/ChemicalDependencyAfflictionHandler.cs`

### `Assets/Ashfall.Core/Medical/ChemicalDependencyAfflictionHandler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 228 lines / 10020 bytes.
- SHA-256: `21111233537540d1d750401ff991c2ff618a58d99fa41da6e0d736b2360e0a1c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ChemicalDependencyAfflictionHandler : IAfflictionHandler
public const string SymptomCraving = "symptom_craving";
public const string SymptomWithdrawalTremor = "symptom_withdrawal_tremor";
public const string SymptomManagedWithdrawal = "symptom_managed_withdrawal";
public AfflictionId DefinitionId =>
public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor) {
public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor) {
public bool CouldHaveCondition(Survivors.SurvivorId survivor) {
public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool HasResolved(Survivors.SurvivorId survivor) {
```


# Appendix Q.564 — Additional Current Architecture Evidence: `src/Host/HostCli.DependencyTaperWithdrawal.cs`

### `src/Host/HostCli.DependencyTaperWithdrawal.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 202 lines / 10797 bytes.
- SHA-256: `c2c3b8e04a2cd00ff340c731edb1378b95ba74043d52d23802d4dbb9d9333d32`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class HostCliDependencyTaperWithdrawal
public static int RunSelfTest(string dataDir) {
```


# Appendix Q.565 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

### `Assets/Ashfall.Core/Disease/DiseaseQuarantineCoordinator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 375 lines / 15683 bytes.
- SHA-256: `3a926de1f7f6e5bb05b4f4bd0a57492ceb92dac2142f13ddd6ca4aa3303529fc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=6; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseQuarantineCoordinator
public event Action<string, string, int>? OnQuarantineAssigned;
public event Action<string, string, int>? OnQuarantineReleased;
public event Action<int, int, float>? OnDailyBurdenProcessed;
public MedicalWardSystem Ward => _medicalWard;
public DiseaseSystem Disease => _diseaseSystem;
public DutyRosterSystem? Roster => _dutyRoster;
public ContainmentCapability Containment =>
public bool IsIsolated(string survivorId) {
public float GetIsolationQuality(string survivorId) {
public MedicalBed? FindAvailableIsolationBed() {
public QuarantineCommandPreview PreviewAssignIsolation(string survivorId) {
public QuarantineCommandResult ExecuteAssignIsolation(string survivorId, int day) {
public QuarantineCommandPreview PreviewReleaseIsolation(string survivorId) {
public QuarantineCommandResult ExecuteReleaseIsolation(string survivorId, int day) {
public void TickDaily(int day) {
public void Rehydrate() {
public sealed class QuarantineCommandPreview
public bool CanExecute { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string TargetBedId { get; set; } = string.Empty;
public string? ConflictingRole { get; set; }
public float ProjectedIsolationQuality { get; set; } = 1.0f;
public Dictionary<string, int> DailySupplyCost { get; set; } = new Dictionary<string, int>();
public static QuarantineCommandPreview Success(string survivorId, string bedId, string? conflictingRole, float projectedQuality, Dictionary<string, int> costs) =>
public static QuarantineCommandPreview Blocked(string reason, string survivorId) =>
public sealed class QuarantineCommandResult
public bool Success { get; set; }
public string Reason { get; set; } = string.Empty;
public string SurvivorId { get; set; } = string.Empty;
public string BedId { get; set; } = string.Empty;
public int Day { get; set; }
public static QuarantineCommandResult Ok(string survivorId, string bedId, int day) =>
public static QuarantineCommandResult Fail(string reason, string survivorId, string bedId = "", int day = 0) =>
```


# Appendix Q.566 — Additional Current Architecture Evidence: `src/Host/ChemicalDependencyHostSession.cs`

### `src/Host/ChemicalDependencyHostSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 123 lines / 4670 bytes.
- SHA-256: `ca5c15cf413101e9509145e5cb6e0ca828d8b8ced14594065743b54ff7a3353b`.
- Architecture signals: seeded references=0; save/restore symbols=3; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ChemicalDependencyHostSession : HostSessionBase
public ChemicalDependencySystem System { get; }
public string LastEvent { get; private set; } = string.Empty;
public MedicalPipelineCoordinator? Pipeline { get; set; }
public override void Save() {
public void RestoreSave(ChemicalDependencyLedgerState? state) {
public CommandResult BeginManagedDetox(string survivorId, string itemId) {
public CommandResult BeginColdTurkey(string survivorId, string itemId) {
protected override void UnsubscribeSystemEvents() {
```


# Appendix Q.567 — Additional Current Architecture Evidence: `src/Host/ChemicalDependencySaveStore.cs`

### `src/Host/ChemicalDependencySaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 42 lines / 2176 bytes.
- SHA-256: `56929e035c0eeeccb5dd67a7113ba1b6934e8d3db217abcedb64776494588ac2`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ChemicalDependencySaveStore
public const string FileName = "chemical_dependency_save.json";
public const string SectionName = "chemical_dependency";
public static string SavePath => s_store.SavePath;
public static bool TrySave(ChemicalDependencyLedgerState state) => s_store.TrySave(state);
public static ChemicalDependencyLedgerState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ChemicalDependencyLedgerState state) => s_store.CapturePersisted(state);
public static string TryCaptureDirect(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);
public static ChemicalDependencyLedgerState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
```


# Appendix Q.568 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/DependencyTaperWithdrawalEngine.cs`

### `Assets/Ashfall.Core/Medical/DependencyTaperWithdrawalEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 267 lines / 12882 bytes.
- SHA-256: `d2b7aa61c10979a7bc670785d893aa72b40d45fa61be70fbf6dcbb83e91a8421`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum DependencySeverityTier
public enum WithdrawalSymptomBand
public enum CarePolicyPosture
public sealed class TaperProgramState
public string SurvivorId             { get; set; } = string.Empty;
public int DependencyPermille        { get; set; } = 0;
public int DailyStepDownPermille     { get; set; } = 50;
public int CurrentSubstituteDosePermille { get; set; } = 1000;
public int PeerSupportSessionsCompleted { get; set; } = 0;
public bool IsMedicallySupervised    { get; set; } = false;
public int TaperDaysElapsed          { get; set; } = 0;
public TaperProgramState Clone() => new TaperProgramState
public readonly struct TaperDayResult
public int NewSubstituteDosePermille  { get; }
public WithdrawalSymptomBand Symptoms { get; }
public bool TaperComplete             { get; }
public bool RequiresMedicalEscalation { get; }
public int ProductivityPenaltyPermille { get; }
public static class DependencyTaperWithdrawalEngine
public const int TaperCompletionThreshold = 30;
public const int FullPeerSupportSessionCount = 5;
public static int ComputeRecommendedStepDown( int dependencyPermille, bool isMedicallySupervised, int substituteMedicineAvailablePermille) {
public static TaperDayResult AdvanceTaperDay(TaperProgramState program, bool peerSupportRunToday) {
public static DependencySeverityTier ClassifyDependencySeverity(int dependencyPermille) =>
public static CarePolicyPosture RecommendCarePolicy( int criticalCaseCount, int totalShelterPopulation, CarePolicyPosture currentPosture) {
```


# Appendix Q.569 — Additional Current Architecture Evidence: `src/Main.CampaignOwners.cs`

### `src/Main.CampaignOwners.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2957 lines / 146936 bytes.
- SHA-256: `6c612e267459f02941f6c11c6536eaba89417aff5cf2a99aa28350aba04b7930`.
- Architecture signals: seeded references=0; save/restore symbols=121; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* retention is idempotent; captured via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* derived read projection */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) => _m._commitments?.System.CapturePreDaySnapshot(day);
public void RestorePreDaySnapshot(int day) => _m._commitments?.System.RestorePreDaySnapshot(day);
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: campaign days are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* jobs are day-local; capture via save section */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { /* no snapshot: hazards are day-local */ }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) { }
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
public void RestorePreDaySnapshot(int day) {
public void TickDay(int day, List<DayStateChangeEvent> events) {
public void CapturePreDaySnapshot(int day) {
```


# Appendix Q.570 — Additional Current Architecture Evidence: `src/Main.ShelterBatch3.cs`

### `src/Main.ShelterBatch3.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 467 lines / 24198 bytes.
- SHA-256: `641492ff2b80e3c6ae9004000c647a0d8401a3a46b16ea5f9300fab2c3d91338`.
- Architecture signals: seeded references=0; save/restore symbols=20; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
```


# Appendix Q.571 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionHubSave.cs`

### `Assets/Ashfall.Core/ExpansionHubSave.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 507 lines / 28559 bytes.
- SHA-256: `6f9efccbfed1c5101889fb1aeff9b33800d110d217b1f669924bbbfdf2d1e282`.
- Architecture signals: seeded references=0; save/restore symbols=31; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class ExpansionHubSave
public const int CurrentSaveVersion = 6;
public int saveVersion = CurrentSaveVersion;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public SaltMineState saltMine = new SaltMineState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV5
public int saveVersion = 5;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public DebtDispatcherState debtDispatcher = new DebtDispatcherState();
public FactionEmbargoLedgerState embargoes = new FactionEmbargoLedgerState();
public DebtConsequenceBridgeState debtBridge = new DebtConsequenceBridgeState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV1
public int saveVersion = 1;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV2
public int saveVersion = 2;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV3
public int saveVersion = 3;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public string Checksum = string.Empty;
public sealed class ExpansionHubSaveV4
public int saveVersion = 4;
public int simDay;
public WaystationSystemState waystation = new WaystationSystemState();
public LocationLayoutState layouts = new LocationLayoutState();
public LocationMemoryState memory = new LocationMemoryState();
public SiteEncounterState siteEncounters = new SiteEncounterState();
public VouchAccessSystemState vouch = new VouchAccessSystemState();
public GreenhouseState greenhouse = new GreenhouseState();
public CrossingArbitrationState arbitration = new CrossingArbitrationState();
public LedgerDebtSystemState ledger = new LedgerDebtSystemState();
public CrossingQuestSystemState crossingQuests = new CrossingQuestSystemState();
public GenerationalSuccessionSaveState generational = new GenerationalSuccessionSaveState();
public SilentFoundryState foundry = new SilentFoundryState();
public SilentFoundryConsequenceState consequences = new SilentFoundryConsequenceState();
public DiseaseSystemState disease = new DiseaseSystemState();
public string Checksum = string.Empty;
public static class ExpansionHubSaveCodec
public static ExpansionHubSave Capture( int simDay, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
public static string Encode(ExpansionHubSave save, IJsonSerializer json) {
public static ExpansionHubSave Decode(string jsonText, IJsonSerializer json) {
public static void Restore( ExpansionHubSave save, WaystationSystem waystation, LocationLayoutSystem layouts, LocationMemorySystem memory, SiteEncounterSystem siteEncounters,
```


# Appendix Q.572 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

### `Assets/Ashfall.Core/Disease/DiseaseHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 489 lines / 28179 bytes.
- SHA-256: `d985f4110e9c912b9818316dad3b3cf219b2d9b5703fed11e1e5531a51a47b1b`.
- Architecture signals: seeded references=8; save/restore symbols=11; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseHeadlessDemo
public const int DemoSeed = 1013;
public static HeadlessReport Run(string? dataDirectory = null, ILog? log = null) {
public string id = string.Empty;
public int schema_version;
public List<DiseaseDemoItemRow> items = new List<DiseaseDemoItemRow>();
```


# Appendix Q.573 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

### `Assets/Ashfall.Core/Content/ContentUtilizationScanner.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 2073 lines / 130366 bytes.
- SHA-256: `7588dbb7ed053936964371ce06c49160f772cb9fffd2e7d519884ab430f044c4`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ContentUtilizationScanner
public static bool IsNarrativeSubdirectoryFile(string relativePath) {
public static bool IsAuthoritativeCatalog(string fileName) {
public ContentUtilizationGraph Scan() {
```


# Appendix Q.574 — Additional Current Architecture Evidence: `src/Host/ChemicalDependencySaveSelfTest.cs`

### `src/Host/ChemicalDependencySaveSelfTest.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 32 lines / 1080 bytes.
- SHA-256: `ddb263308352c17e464746f305291603ee3fb93fbb05ebe149f786e7d9780120`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class ChemicalDependencySaveSelfTest
public static string Run(string dataDirectory) {
```


# Appendix Q.575 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalHeadlessDemo.cs`

### `Assets/Ashfall.Core/Medical/MedicalHeadlessDemo.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 107 lines / 5342 bytes.
- SHA-256: `5f4b0e32becc67a543cce3ae2d29565cf8823616ed830b466378f3df7689f4b7`.
- Architecture signals: seeded references=0; save/restore symbols=5; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MedicalHeadlessDemo
public static HeadlessReport Run(ILog? log = null) {
```


# Appendix Q.576 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Survivors/DesperationSystem.cs`

### `Assets/Ashfall.Core/Survivors/DesperationSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 288 lines / 11666 bytes.
- SHA-256: `ae21338fa8cfaca95506734b57332cfe6f0dc52a0c3403c09d853980b5bf6626`.
- Architecture signals: seeded references=2; save/restore symbols=2; typed event declarations=4; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DesperationEventDef
public string desperation_id { get; set; } = string.Empty;
public string event_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public float required_starvation { get; set; } = 90.0f;
public List<string> required_conditions { get; set; } = new List<string>();
public List<string> forbidden_conditions { get; set; } = new List<string>();
public string resource_yield_item_id { get; set; } = "raw_meat";
public int resource_yield_count { get; set; } = 6;
public string taboo_level { get; set; } = "Broken";
public float morale_shock { get; set; } = 35.0f;
public float prion_risk { get; set; } = 0.15f;
public float mutiny_pressure { get; set; } = 25.0f;
public string trait_granted { get; set; } = "trait_cannibal";
public string trait_awarded { get; set; } = "trait_cannibal";
public string Id => !string.IsNullOrEmpty(desperation_id) ? desperation_id : event_id;
public sealed class DesperationCatalogContainer
public int schema_version { get; set; } = 1;
public List<DesperationEventDef> events { get; set; } = new List<DesperationEventDef>();
public sealed class DesperationActRecord
public string actId { get; set; } = string.Empty;
public string eventId { get; set; } = string.Empty;
public string actorId { get; set; } = string.Empty;
public string corpseId { get; set; } = string.Empty;
public int day { get; set; }
public string tabooLevel { get; set; } = "Broken";
public int meatYield { get; set; }
public bool prionContracted { get; set; }
public sealed class DesperationState
public string systemId = DesperationSystem.SystemId;
public float mutinyPressure;
public List<string> harvestedCorpseIds = new List<string>();
public List<string> cannibalSurvivorIds = new List<string>();
public List<DesperationActRecord> actsHistory = new List<DesperationActRecord>();
public List<string> unburiedCorpseIds = new List<string>();
public List<string> buriedCorpseIds = new List<string>();
public List<string> oneShotShockIds = new List<string>();
public sealed class DesperationSystem
public const string SystemId = "desperation";
public const float CrisisStarvationThreshold = 90.0f;
public DesperationState State => _state;
public float MutinyPressure => _state.mutinyPressure;
public event Action<DesperationActRecord>? OnTabooBroken;
public event Action<float>? OnMutinyPressureChanged;
public void LoadCatalog(string dataPath) {
public void RegisterEvent(DesperationEventDef def) {
public void RegisterCorpse(string corpseId) {
public ActionResult PerformBurial(string corpseId) {
public bool IsActionEligible(string survivorId, string eventId, string corpseId) {
public ActionResult HarvestCorpse(string actorId, string corpseId, string eventId, int currentDay, bool hasCynicalTrait = false) {
public DesperationState CaptureState() {
public void RestoreState(DesperationState state) {
```


# Appendix Q.577 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/StressRelapseRules.cs`

### `Assets/Ashfall.Core/Medical/StressRelapseRules.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 82 lines / 3858 bytes.
- SHA-256: `49b8bc94afc7a27ff5bfcc3047f73bcd312b55c8f28be52f0718703e66eaa6bc`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class StressRelapseRules
public const float MaxDeltaPerCall = 0.15f;
public const float MinMeaningfulMagnitude = 0.05f;
public static float ComputeDelta(float magnitude, ChemicalDependencyKind kind) {
public static bool IsReportable(float magnitude) {
```


# Appendix Q.578 — Additional Current Architecture Evidence: `src/Main.Plans147.cs`

### `src/Main.Plans147.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 322 lines / 14409 bytes.
- SHA-256: `aae10684588c6294c3948d601deafcfdd81a503deedf6f92a4ee5ef1552b6485`.
- Architecture signals: seeded references=1; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ContrabandStashSystem EnsureContrabandStash() {
public ActionResult ClaimContrabandStash(string entryId) {
public UI.ShelterBarterPanel EnsureShelterBarterPanel() {
public void OpenShelterBarterPanel() {
public ShelterBarterSystem EnsureShelterBarter() {
```


# Appendix Q.579 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/HostCliRegistry.cs`

### `Assets/Ashfall.Core/HostCliRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1912 lines / 102959 bytes.
- SHA-256: `827182dd993f0bff556c0f2e1ba84448391b5b2b728a0d2242780da4594d9940`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=1; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum HostCliAction
public sealed class HostCliActionDescriptor
public HostCliAction Action { get; }
public string Category { get; }
public string PrimaryFlag { get; }
public IReadOnlyList<string> Aliases { get; }
public string Description { get; }
public string ValuePlaceholder { get; }
public IReadOnlyList<string> AllFlags { get; }
public bool IsSelfTest { get; }
public bool IsTest { get; }
public bool HeadlessCompatible { get; }
public string TestId { get; }
public string FormatHelpLine() {
public static class HostCliRegistry
public static readonly IReadOnlyList<string> Categories = new ReadOnlyCollection<string>(new[] {
public static IReadOnlyList<HostCliActionDescriptor> AllDescriptors => _descriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> FlagMap => _flagMap;
public static IReadOnlyList<HostCliActionDescriptor> CoreDescriptors => _coreDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ExpansionDescriptors => _expansionDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> HostDomainDescriptors => _hostDomainDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> UiDescriptors => _uiDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> ConfigDescriptors => _configDescriptors;
public static IReadOnlyList<HostCliActionDescriptor> InfoDescriptors => _infoDescriptors;
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateFlagRegistry() {
public static IReadOnlyDictionary<string, HostCliActionDescriptor> ValidateDescriptors(IEnumerable<HostCliActionDescriptor> descriptors) {
public static HostCliAction Resolve(string[]? args) {
public static void PrintHelp(Action<string> print) {
public static void PrintSelfTests(Action<string> print) {
public static HostSelfTestManifest CreateSelfTestManifest() {
public static string GenerateJsonManifest() {
public static string GenerateMarkdownCatalog(string verifiedDate) {
public sealed class HostSelfTestManifest
public string SchemaVersion { get; set; } = "1.0.0";
public string Description { get; set; } = "";
public int TotalTests { get; set; }
public int HeadlessTestCount { get; set; }
public List<HostSelfTestItem> Tests { get; set; } = new List<HostSelfTestItem>();
public sealed class HostSelfTestItem
public string TestId { get; set; } = "";
public string Action { get; set; } = "";
public string Category { get; set; } = "";
public string PrimaryFlag { get; set; } = "";
public string[] Aliases { get; set; } = Array.Empty<string>();
public string Description { get; set; } = "";
public bool HeadlessCompatible { get; set; }
public string ExpectedSummaryId { get; set; } = "";
public int TimeoutSeconds { get; set; } = 30;
```


# Appendix Q.580 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/DiseaseAfflictionHandler.cs`

### `Assets/Ashfall.Core/Medical/DiseaseAfflictionHandler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 176 lines / 8179 bytes.
- SHA-256: `b839868c1ca10cdb00c489920220545281f04590d762a75b4bd5211cfc3e68ad`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class DiseaseAfflictionHandler : IAfflictionHandler
public const string SymptomGastrointestinalDistress = "symptom_gastrointestinal_distress";
public const string SymptomFever = "symptom_fever";
public const string SymptomPersistentCough = "symptom_persistent_cough";
public const string SymptomFatigue = "symptom_fatigue";
public const string SymptomBreathlessness = "symptom_breathlessness";
public AfflictionId DefinitionId => new AfflictionId(_definition.id);
public string DiseaseId => _definition.id;
public string DisplayName => _definition.display_name;
public AfflictionEpisodeSnapshot? GetEpisode(Survivors.SurvivorId survivor) {
public IReadOnlyList<SymptomProjection> ProjectSymptoms(Survivors.SurvivorId survivor) {
public bool CouldHaveCondition(Survivors.SurvivorId survivor) {
public string? ValidateTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool ApplyTreatment(Survivors.SurvivorId survivor, string treatmentId, string? targetItem = null) {
public bool HasResolved(Survivors.SurvivorId survivor) {
public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease, DiseaseCatalog catalog) {
```


# Appendix Q.581 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`

### `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 727 lines / 75985 bytes.
- SHA-256: `f6c7527c4c8d3a554a58ae7690d54f773a331203ddbb26b65b6c05377616ee74`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public record SaveSectionMetadata(
public static class SaveSectionRegistry
public const string ExpandedShelterLifecycleGroup = "expanded_shelter";
public static readonly IReadOnlyDictionary<string, string> LifecycleSectionAliases = new Dictionary<string, string>(StringComparer.Ordinal) {
public static readonly IReadOnlyList<SaveSectionMetadata> All = new List<SaveSectionMetadata> {
public static readonly IReadOnlyDictionary<string, string> SectionFileNames = new Dictionary<string, string>(StringComparer.Ordinal) {
public static readonly IReadOnlyDictionary<string, int> SchemaVersions = new Dictionary<string, int>(StringComparer.Ordinal) {
public static string? CanonicalizeSectionKey(string? sectionKey) {
public static IReadOnlyList<string> SectionKeysForLifecycleGroup(string lifecycleGroup) {
public static bool IsLifecycleGroup(string lifecycleGroup) =>
public static IReadOnlyCollection<string> LifecycleGroupKeys => SectionsByLifecycleGroup.Keys.ToArray();
public static string? FileNameFor(string sectionKey) {
public static int SchemaVersionFor(string sectionKey) {
public static bool TryGetKeyForSectionName(string sectionName, out string? sectionKey) {
public static bool TryGetSection(string sectionKey, out SaveSectionMetadata? metadata) {
public static IReadOnlyList<string> SectionKeys => All.Select(s => s.SectionKey).ToList();
```


# Appendix Q.582 — Additional Current Architecture Evidence: `src/Host/DiseaseSaveStore.cs`

### `src/Host/DiseaseSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 2361 bytes.
- SHA-256: `8002e13be286bd28bbe4f3b93dcf88e9ab097fb20218cf2b856da5b9bfa4c12f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class DiseaseSaveStore
public const string FileName = "disease_save.json";
public const string SectionName = "disease";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(DiseaseSystemState state) => s_store.CaptureBare(state);
public static DiseaseSystemState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(DiseaseSystemState state) => s_store.CaptureBare(state);
public static DiseaseSystemState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(DiseaseSystemState state) => s_store.TrySave(state);
public static DiseaseSystemState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(DiseaseSystemState state) => s_store.CapturePersisted(state);
```


# Appendix Q.583 — Additional Current Architecture Evidence: `src/Host/MedicalSaveStore.cs`

### `src/Host/MedicalSaveStore.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 49 lines / 2493 bytes.
- SHA-256: `be6b19501cb9b44ecccbb5eb90d9bc077f19d7101e9375ab1ff1926e0c00ad4f`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Persistence evidence must distinguish current owner state, derived snapshots and host fallback. A new DTO is not automatically a new save section.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class MedicalSaveStore
public const string FileName = "medical_save.json";
public const string SectionName = "medical";
public static string SavePath => s_store.SavePath;
public static bool Exists => s_store.Exists();
public static string TryCaptureDirect(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);
public static ChemicalDependencyLedgerState? TryRestoreDirect(string json) => s_store.RestoreBare(json);
public static string TryCapture(ChemicalDependencyLedgerState state) => s_store.CaptureBare(state);
public static ChemicalDependencyLedgerState? TryRestore(string json) => s_store.RestoreBare(json);
public static bool TrySave(ChemicalDependencyLedgerState state) => s_store.TrySave(state);
public static ChemicalDependencyLedgerState? TryLoad() => s_store.TryLoad();
public static string TryCapturePersisted(ChemicalDependencyLedgerState state) => s_store.CapturePersisted(state);
```


# Appendix Q.584 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/ExpansionMasterSession.cs`

### `Assets/Ashfall.Core/ExpansionMasterSession.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 212 lines / 10386 bytes.
- SHA-256: `568760f21b08597c7cd360ea5f147c07feebc84edac5a7ca2f20addfd7545f0b`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ExpansionMasterSession
public HoldfastSession Holdfast { get; }
public DutyRosterSystem DutyRoster { get; }
public DutyRosterCatalog DutyRosterData { get; }
public LocationLayoutSystem StandingRecord { get; }
public CrossingSession Crossing { get; }
public SimClock Clock { get; }
public ILog Log { get; }
public SilentFoundrySystem SilentFoundry { get; }
public SilentFoundryCatalog FoundryData { get; }
public DiseaseSystem Disease { get; }
public DiseaseCatalog DiseaseData { get; }
public bool AllExpansionsActive =>
public static ExpansionMasterSession Load(string dataDirectory, int seed =808, ILog? log = null) {
public void TickDaily(WeatherKind weather, float outdoorTemp, List<DutyRosterOccupant>? homeOccupants = null, IReadOnlyList<string>? diseaseCandidates = null) {
public static HeadlessReport RunAllSelfTests(string? dataDirectory = null, ILog? log = null) {
```


# Appendix Q.585 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/DiseaseProtocolHandler.cs`

### `Assets/Ashfall.Core/Medical/DiseaseProtocolHandler.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 140 lines / 6019 bytes.
- SHA-256: `3da3d99ac11d1ffbb386f6a137d050a9bf4037b4e5fd4a6241e81fe87605fa96`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public interface IMedicalProtocolHandler
public sealed class DiseaseProtocolHandler : IMedicalProtocolHandler
public string ProtocolId => _protocolId;
public string DisplayName => _displayName;
public IReadOnlyDictionary<string, int> ItemCosts => _costs;
public string? Validate() {
public bool Apply() {
public static int RegisterAll(MedicalPipelineCoordinator pipeline, DiseaseSystem disease, Func<int>? dayProvider = null) {
```


# Appendix Q.586 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalTextCatalog.cs`

### `Assets/Ashfall.Core/Medical/MedicalTextCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 178 lines / 7033 bytes.
- SHA-256: `78d0ded961ed22231f6178c8d6bbfe11a95170c4b55bedbbbd3e15ccfedf47f1`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalConditionTextEntry
public string id = string.Empty;
public string category = string.Empty;
public string display_name = string.Empty;
public string diagnosis_text = string.Empty;
public List<string> symptom_descriptions = new List<string>();
public List<string> treatment_steps = new List<string>();
public List<string> required_items = new List<string>();
public Dictionary<string, double> success_chances = new Dictionary<string, double>();
public List<string> failure_consequences = new List<string>();
public List<string> recovery_descriptions = new List<string>();
public List<string> complication_warnings = new List<string>();
public List<string> prevention_advice = new List<string>();
public List<string> long_term_effects = new List<string>();
public List<string> pain_descriptions = new List<string>();
public string mental_state = string.Empty;
public string physical_state = string.Empty;
public string emotional_impact = string.Empty;
public string system_integration = string.Empty;
public sealed class MedicalTextsFile
public int schema_version = 1;
public string collection_id = "medical_texts";
public List<MedicalConditionTextEntry> conditions = new List<MedicalConditionTextEntry>();
public sealed class MedicalTextCatalog
public IReadOnlyList<MedicalConditionTextEntry> AllConditions => _allConditions;
public int Count => _allConditions.Count;
public void Load(string json, IJsonSerializer serializer) {
public static MedicalTextCatalog LoadFromDirectory(string dataDir, IFileIO fileIo, IJsonSerializer? serializer = null) {
public MedicalConditionTextEntry? TryGetConditionText(string? conditionId) {
public string GetSafeDiagnosisSummary(string? conditionId) {
public string GetSymptomProse(string? conditionId, int? seed = null) {
public string GetComplicationWarning(string? conditionId) {
public string GetRecoveryProse(string? conditionId) {
```


# Appendix Q.587 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/MedicalTreatmentCatalog.cs`

### `Assets/Ashfall.Core/Medical/MedicalTreatmentCatalog.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 220 lines / 11412 bytes.
- SHA-256: `312fa3e3a92b04ad5d55c3917f8d386f6ec1ae84b675b80617be6c152a157004`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class MedicalTreatmentDef
public string TreatmentId = string.Empty;
public string DisplayName = string.Empty;
public string AfflictionId = string.Empty;
public Dictionary<string, int> ItemCosts = new Dictionary<string, int>();
public bool RequiresConfirmedDiagnosis;
public bool IsScheduled;
public float DurationHours;
public bool ExclusivePerPatient;
public string RequiredCapability = string.Empty;
public bool RequiresWardAdmission;
public MedicalTreatmentDef Clone() {
public static class MedicalTreatmentCatalog
public const string RespiratoryDegenerationId = "affliction_respiratory_degeneration";
public const string RadiationSicknessId = "affliction_radiation_sickness";
public const string ChemicalDependencyId = "affliction_chemical_dependency";
public const string DiseaseId = "affliction_disease";
public const string HealthDeficitId = "affliction_health_deficit";
public const string CombatTraumaId = "affliction_combat_trauma";
public const string SomaticFlashbackId = "affliction_somatic_flashback";
public const string GuiltInsomniaId = "affliction_guilt_insomnia";
public const string ItemInhaler = "inhaler";
public const string ItemHerbalTea = "herbal_tea";
public const string ItemBandage = "bandage";
public const string ItemIodine = "iodine_pills";
public const string ItemAntiRad = "rad_away";
public const string TreatmentInhaler = "treatment_inhaler";
public const string TreatmentHerbalTea = "treatment_herbal_tea";
public const string TreatmentOxygenSupport = "treatment_oxygen_support";
public const string TreatmentBandage = "treatment_bandage";
public const string TreatmentIodine = "treatment_iodine";
public const string TreatmentAntiRad = "treatment_anti_rad";
public const string TreatmentQuarantine = "treatment_quarantine";
public const string TreatmentRelease = "treatment_release";
public const string UnidentifiedIllnessId = "affliction_unidentified_illness";
public const string ProtocolPurifyWater = "protocol_purify_water";
public const string ProtocolSealVents = "protocol_seal_vents";
public const string ProtocolSterilizeTools = "protocol_sterilize_tools";
public const string ProtocolAirFiltration = "protocol_air_filtration";
public const string TreatmentManagedDetox = "treatment_managed_detox";
public const string TreatmentColdTurkey = "treatment_cold_turkey";
public const string ItemCleanWater = "clean_water";
public const string ItemGasMask = "gas_mask";
public const string ItemAntibiotics = "antibiotics";
public const string ItemHazmatSuit = "hazmat_suit";
public const string ItemOxygenSupply = "item_oxygen_supply";
public static MedicalTreatmentDef? Get(string treatmentId) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
