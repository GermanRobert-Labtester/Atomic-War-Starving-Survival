# D1 Flagship Integration Plan [18]
## Plan 198 — Health History & Medical Records System

> **Purpose:** Create a persistent, survivor-scoped medical record that gives ASHFALL genuine health continuity:
> diagnoses, injuries, treatment episodes, recoveries, relapses, complications, vaccinations, radiation
> exposures, chronic-condition changes, checkups, contraindications, and long-term trends become queryable
> historical facts rather than isolated transient events.
>
> **Primary source:** Plan 198 — Health History & Medical Records System.
>
> **Core repository problem:** `MedicalPipelineCoordinator`, `DiseaseSystem`, `RadiationSystem`,
> `CombatTraumaSystem`, and related medical systems resolve acute events but do not preserve one unified health
> history. `DoseLedgerSystem` provides radiation-specific longitudinal data, but no general survivor medical
> chart exists. Medics therefore cannot inspect prior diagnoses, treatment response, radiation history,
> vaccinations, or chronic-condition progression through one canonical record.
>
> **Implementation posture:** append-only where practical, event-sourced, clinically conservative, deterministic,
> schema-versioned, read-model oriented, and explicitly subordinate to existing medical authorities. The system
> records what authoritative systems say happened; it does not become a second disease, trauma, radiation,
> chronic-condition, immunity, or treatment engine.
>
> **Critical guardrail:** `HealthHistorySystem` is primarily a medical-history/read-model authority. It must not
> duplicate active health state. "History informs treatment" means the MedicalPipeline may query prior verified
> facts such as previous treatment failures, allergies, contraindications, vaccination status, and recurrence;
> the history system itself should not directly mutate treatment effectiveness.
---

## 1. Source Problem Statement

The source identifies the missing continuity layer:

- no `HealthHistory`, `MedicalRecord`, `MedicalHistory`, or `HealthLog` authority exists;
- illness, injury, treatment, radiation, and trauma systems are episodic;
- no durable treatment history exists;
- no vaccination history exists;
- no general health trend model exists;
- no per-survivor medical chart exists;
- `DoseLedgerSystem` is useful but radiation-specific;
- chronic conditions from Plan 193 would add current persistent state, not retrospective medical history.

The correct architecture is:

```text
Authoritative medical systems
 ├─ DiseaseSystem
 ├─ CombatTraumaSystem
 ├─ MedicalPipelineCoordinator
 ├─ RadiationSystem
 ├─ DoseLedgerSystem
 ├─ ChronicConditionSystem
 ├─ Vaccination/Immunity authority
 └─ checkup/medical assessment
                ↓ typed events
        HealthHistorySystem
                ↓
        normalized medical record
        ├─ episodes
        ├─ interventions
        ├─ immunizations
        ├─ measurements
        ├─ contraindication facts
        └─ historical references
                ↓
       projections / decision support
        ├─ medic patient summary
        ├─ timeline/chart
        ├─ trends
        ├─ alerts
        ├─ archive/export
        └─ quest/achievement hooks
```

The health-history layer records truth already established elsewhere. It should not create diagnoses, invent
treatments, recalculate radiation dose, or decide whether a chronic condition exists.
---

## 2. Flagship Success Criteria

The implementation is complete only when all of the following are true:

1. `HealthHistorySystem.cs` exists and supports schema-versioned capture/restore.
2. Every medical history entry has a stable record/event ID.
3. Every entry references a real survivor ID.
4. Diagnosis records originate from canonical diagnosis events.
5. Treatment records originate from canonical treatment/procedure/medication completion.
6. Recovery records originate from canonical resolution.
7. Relapse records originate from a real recurrence/reinstatement event, not a guessed repeated diagnosis.
8. Complications link to the originating condition/episode when known.
9. Radiation history references `DoseLedgerSystem` rather than copying its full ledger.
10. Chronic-condition history references Plan 193 condition instance IDs rather than duplicating active condition state.
11. Vaccination history records administration events but does not duplicate immunity math if an immunity system exists.
12. Contraindications and allergies are stable medical facts with explicit provenance.
13. Medic decision support can query history without mutating the record.
14. Previous treatment response can influence treatment selection only through explicit MedicalPipeline rules.
15. Health trends are derived/downsampled from authoritative health measurements.
16. Trends do not create a second `overallHealth` authority.
17. The system does not persist one daily record per metric indefinitely.
18. Trend storage is bounded or compressed.
19. Medical records are automatically generated for authoritative events by default.
20. Manual notes cannot overwrite clinical facts.
21. Manual notes are clearly separated from system-verified records.
22. Deceased survivors retain archived records.
23. Old saves load with empty history unless trustworthy reconstruction data exists.
24. No retroactive fake diagnoses/treatments are invented during migration.
25. Save/load does not duplicate events.
26. UI search/filter uses projections, not raw mutation logic.
27. Export has a stable schema and does not mutate campaign state.
28. Headless simulations produce the same record history as UI-driven runs.
29. `--health-history-selftest` validates all record types, idempotency, trend compaction, vaccination links,
    contraindication checks, migration, archive, and save round-trip.
---

## 3. Repository Reconnaissance Before Editing

Create:

`docs/medical_history/HEALTH_HISTORY_INTEGRATION_AUDIT.md`

Inspect at minimum:

- `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs`
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs`
- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs`
- `Assets/Ashfall.Core/Radiation/DoseLedgerSystem.cs`
- combat trauma / wound / fracture systems
- medication administration
- surgery/procedure pipeline
- diagnostics
- vaccination/immunity systems if any
- chronic condition system from Plan 193
- aging/geriatric health systems
- survivor death/archive lifecycle
- inventory consumption for medications
- allergies/contraindications if already represented
- journal/archive systems
- save schema/migrations
- event bus and stable event IDs
- deterministic RNG
- UI medical panel
- quest/achievement/epilogue consumers
- localization
- export/file-writing utilities if the game supports export

Build an authority matrix:

| Fact | Canonical owner | Event/API | History representation |
|---|---|---|---|
| active disease | DiseaseSystem | | episode reference |
| injury/wound | Trauma/Medical | | injury episode |
| treatment | MedicalPipeline | | intervention record |
| radiation dose | DoseLedger | | measurement/reference |
| chronic condition | ChronicConditionSystem | | longitudinal reference |
| vaccination | Immunity/Vaccine authority | | immunization record |
| death | SurvivorLifecycle/Fate | | terminal outcome |
| allergy | medical/survivor fact authority | | verified fact |

Do not implement record creation by scanning current state if authoritative events already exist.
---

## 4. Scope Boundary

### In scope

- survivor medical charts;
- diagnosis/injury/treatment/recovery records;
- vaccination administration history;
- radiation exposure summaries/references;
- chronic-condition history references;
- checkup/assessment records;
- contraindication/allergy facts;
- medic notes;
- health measurements and trend projection;
- alerts;
- search/filter;
- deceased-survivor archive;
- export;
- save/load;
- migration;
- CI/selftests.

### Out of scope

- replacing DiseaseSystem;
- replacing MedicalPipeline;
- replacing DoseLedgerSystem;
- replacing ChronicConditionSystem;
- independent immunity simulation;
- medical-AI diagnosis;
- real-world clinical advice;
- unrestricted free-text notes becoming mechanics;
- infinite daily telemetry retention;
- cross-campaign profile medical history;
- multiplayer privacy/security architecture.

The system is a historical continuity layer.

---
## 5. Core State Model
- Prefer a survivor-keyed sparse state rather than one giant global flat list.
- Keep active medical truth elsewhere; store historical records, references, measurements, notes, and indexes.
- Use append-only records where possible and explicit amendment records where correction is needed.
- Never silently edit a past record's meaning after it has been emitted.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 6. Medical Record DTO
- Replace free-form `description` as authority with structured references plus a localization template.
- Include `recordId`, `survivorId`, `recordTypeId`, `recordedDay`, `sourceEventId`, `severityId`, `outcomeId`, and linked condition/treatment IDs.
- `duration` should be derived from episode start/end where possible rather than manually maintained.
- Use a compact metadata/fact map only for validated fields.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 7. Health Event DTO
- Use health events as immutable timeline entries linked to a parent episode or medical record.
- Event types include diagnosis, treatment, recovery, relapse, complication, vaccination, checkup, and clinically relevant progression.
- Every event carries a source event ID for idempotency.
- Do not duplicate the same intervention as both unrelated record and unrelated event; define parent-child relationships.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 8. Episode Model
- Introduce `MedicalEpisode` for illness/injury/chronic-treatment arcs where several events belong together.
- Episode fields: episodeId, survivorId, episodeType, sourceConditionId, startDay, endDay, outcome, severity snapshots, event IDs.
- This makes one disease case readable without flattening diagnosis, treatment, complication, and recovery into unrelated rows.
- Simple one-off vaccinations/checkups need not create full episodes.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 9. Record Type Taxonomy
- Support at least illness, injury, treatment, vaccination, radiation_exposure, chronic_condition, and checkup.
- Add procedure, medication, complication, and measurement only if they produce meaningful UI/search value.
- Do not create redundant record types that are better represented as event subtypes.
- Record taxonomy lives in data/registered enums, not UI strings.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 10. Event Type Taxonomy
- Support diagnosis, treatment, recovery, relapse, complication, vaccination, and checkup.
- Potential additional types: procedure_started, procedure_completed, adverse_reaction, condition_progressed, condition_stabilized.
- Only add types with an authoritative upstream emitter.
- Event IDs must be stable across save/load.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 11. Medical Record Templates
- Create `medical_record_templates.json` for presentation templates, labels, grouping, and severity/outcome vocabulary.
- Templates define rendering, not medical truth.
- Store IDs and structured facts in save; render localized text at runtime.
- This allows wording/localization changes without rewriting history.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 12. Illness Records
- DiseaseSystem emits episode/diagnosis/resolution facts.
- History stores disease ID, diagnosis day, severity, relevant treatment event IDs, and final outcome.
- Symptoms may be summarized only from canonical disease state.
- Relapse must reference a real recurrence event or a new linked episode.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 13. Injury Records
- CombatTrauma/wound systems emit injury facts.
- History can group related wounds from one combat incident under one episode while retaining significant individual injuries.
- Do not record every transient damage tick.
- Fracture, severe wound, surgery, amputation, and chronic sequela links should be retained when real.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 14. Treatment Records
- MedicalPipelineCoordinator is authoritative for intervention completion.
- Record procedure/medication/treatment IDs, treating survivor, day, outcome, and linked episode.
- Do not record treatment merely because the player opened a treatment panel or queued an action.
- Cancelled/failed attempts need explicit outcomes if clinically relevant.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 15. Medication History
- Repeated medication doses should not flood the record.
- Store major medication course start/end, adverse reactions, and clinically important changes.
- Routine scheduled doses can be summarized by treatment episode or aggregate adherence if another system owns it.
- Never infer medication taken merely because item disappeared from inventory.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 16. Surgery/Procedure History
- Procedures create explicit records with operator, indication, result, and complication links.
- Procedure outcome comes from MedicalPipeline.
- Do not let HealthHistory independently decide surgical success.
- Major surgery is always retained even after record compaction.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 17. Recovery Records
- Recovery is emitted by the authority that resolves the condition.
- Close the parent episode and record outcome.
- Do not infer recovery merely because a condition is absent from a snapshot unless the upstream system guarantees that semantics.
- Resolved records remain historical.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 18. Relapse Records
- Relapse should link to prior episode/condition when the DiseaseSystem identifies recurrence.
- Do not classify any repeated disease ID as relapse automatically.
- If recurrence linkage is unavailable, create a new episode and optionally note prior history through query projection.
- Relapse events should be uncommon enough to remain meaningful.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 19. Complication Records
- Complications link child condition/episode to parent condition/treatment/event.
- Preserve causal provenance only if authoritative system asserts it.
- Do not infer causation from temporal proximity.
- UI may display 'complication of' only when relation exists.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 20. Radiation Exposure Records
- `DoseLedgerSystem` remains authoritative for dose history.
- HealthHistory should store clinically significant exposure events and summary references, not duplicate every dose ledger entry.
- High-dose event, decontamination, radiation sickness diagnosis, and chronic sequela are meaningful record points.
- Trend projection may query cumulative dose directly from DoseLedger.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 21. Dose Ledger Integration
- Expose a read adapter such as `IRadiationDoseHistoryView` rather than copying dose state.
- HealthHistory can cache a sparse measurement series for UI only if needed.
- Selftest compares projected radiation trend against DoseLedger truth.
- No independent cumulative-dose arithmetic.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 22. Chronic Condition Integration
- Plan 193 owns active chronic conditions and progression.
- HealthHistory records onset, diagnosis, severity transitions, management milestones, accommodation/treatment milestones, and resolution where applicable.
- Condition instance ID is the durable link.
- Do not duplicate active condition severity as a separate editable record.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 23. Vaccination Boundary
- The source proposes a vaccination system inside HealthHistory; prefer separating administration history from immunity authority.
- If no immunity system exists, create a small Vaccination/Immunity component under medical systems, not hidden inside record storage.
- HealthHistory records what vaccine was given, when, by whom, and any booster schedule reference.
- Immunity decay/susceptibility should have one owner.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 24. Vaccination Record
- Fields: vaccinationRecordId, survivorId, vaccineDefinitionId, administeredDay, administeredBy, sourceEventId, seriesDoseNumber, nextBoosterDue if authored.
- Do not persist `immunityLevel` here if an ImmunitySystem is authoritative.
- If immunity is intentionally part of HealthHistory in v1, document it as temporary ownership and expose one API.
- Booster due is derived from schedule when possible.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 25. Booster Alerts
- Alert only when a booster is actually due and relevant vaccine/medical capability exists.
- Use one notification per due window, not daily spam.
- Resolved booster administration closes the alert.
- Do not duplicate calendar reminders in another system.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 26. Allergy Facts
- Allergies must be stable verified medical facts with provenance.
- Do not infer allergy from a generic treatment failure.
- Allergy facts can originate from survivor generation, diagnosed adverse reaction, or authored scenario.
- MedicalPipeline queries them through a read-only contraindication interface.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 27. Contraindications
- Contraindications may be condition-dependent, medication-dependent, or procedure-dependent.
- Keep rules in treatment/medical catalogs, not free-text notes.
- History supplies past reaction/allergy/current-history facts.
- MedicalPipeline evaluates whether a proposed treatment is contraindicated.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 28. Adverse Reactions
- Adverse reactions are clinically meaningful events and should be retained.
- Link reaction to treatment/medication source event.
- Only a validated reaction rule may create a lasting allergy/contraindication fact.
- Do not turn every treatment failure into an allergy.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 29. Medic Notes
- Manual notes are optional annotation, never authoritative medical mechanics by themselves.
- Store author survivor ID, day, related record/episode, and localized/free text depending engine policy.
- Manual notes cannot overwrite diagnosis, severity, treatment outcome, allergy, or vaccine state.
- UI visually distinguishes 'Medic note' from system-verified record.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 30. Manual Checkups
- Checkups should create records only when an actual medical examination action occurs.
- Checkup can snapshot selected authoritative findings without duplicating active health state.
- Routine checkups should be batch-schedulable if used heavily.
- Do not require checkups for basic visibility of obvious active conditions.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 31. Checkup Findings
- Store findings as references to conditions/measurements or structured status bands.
- A checkup may reveal previously undiagnosed facts only if diagnosis systems support uncertainty.
- Recommendations are presentation unless backed by treatment/policy actions.
- Do not generate fake medical recommendations from history alone.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 32. Treating Survivor
- Record medic ID only when a named survivor actually performed treatment/checkup.
- Automated/self-care can use null or system provider kind.
- Do not assign nearest medic retrospectively.
- Historical medic names remain resolvable after death/departure through survivor archive.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 33. Health Trend Philosophy
- Do not create a vague new `overallHealth` stat solely for graphs.
- Trend metrics should primarily be projections of existing authorities.
- Useful candidates: active disease burden, injury burden, cumulative radiation dose, chronic-condition count, treatment burden, vaccination coverage.
- Composite health is optional and should be explicitly derived/presentation-only.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 34. Trend Sampling
- Do not store daily values forever.
- Use event-driven measurements plus downsampling.
- Example retention: daily for recent 30 days, weekly for older period, monthly/yearly for long campaigns.
- Exact policy belongs in data/config and must preserve graph usefulness.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 35. Trend Downsampling
- Aggregation method per metric must be explicit: last, mean, max, min, cumulative delta, or count.
- Radiation cumulative dose should never be averaged into nonsense.
- Chronic-condition count can use last or daily max.
- Store enough provenance for deterministic reconstruction.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 36. Trend State
- Use a `HealthMeasurementSeries` keyed by metric ID and survivor ID.
- Each point has day, value, source kind, and optional source record.
- Trend direction is derived from recent window rather than persisted as authority.
- Do not store `improving/stable/declining` as a permanent fact unless tied to a specific alert episode.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 37. Declining Trend Alert
- Alert only on sustained meaningful decline according to metric-specific rules.
- Do not alert on normal noise or one bad day.
- Use cooldown/hysteresis.
- Medical authority defines what metrics warrant action.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 38. Overall Health Composite
- If retained, define it as a read-only UI index with transparent components.
- Never use it as a hidden master stat that bypasses disease/injury/chronic systems.
- Do not feed the composite back into treatment effectiveness.
- Allow disabling the composite if it adds no value.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 39. Immune Strength Guardrail
- The source suggests `immune_strength 0-100`; this risks inventing a new global biology stat.
- Prefer concrete immunities/vaccination protection by disease.
- If a generic immune resilience stat already exists, read it; otherwise do not create one solely for the chart.
- Trend coverage can show vaccination protection/active immunities instead.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 40. Treatment Decision Support
- History may expose prior successful/failed treatments, allergies, contraindications, recurrences, and radiation/chronic context.
- MedicalPipeline chooses whether these facts alter available options or success probability.
- HealthHistory never writes treatment modifiers directly.
- Decision-support UI should explain why an option is preferred/blocked.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 41. Previous Treatment Effectiveness
- Do not implement a generic 'previous treatment = better/worse next time' rule.
- Use treatment-specific rules: resistance, repeated surgery risk, allergy, failed prior therapy, or learned protocol if actual systems support them.
- History provides evidence; medical rules interpret it.
- This avoids nonsensical universal memory bonuses/penalties.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 42. Medic Panel Summary
- Show current problems first, then relevant history.
- Summarize allergies/contraindications prominently.
- Show prior related episodes/treatments only when relevant to selected condition.
- Do not force medics/player to scroll through a lifetime timeline during urgent treatment.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 43. Chronological Chart
- Provide a complete timeline view sorted by day and stable sequence.
- Group events under episodes.
- Allow filters by illness, injury, treatment, vaccination, radiation, chronic, checkup, severity, and outcome.
- Do not hide current active state in a history-only timeline.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 44. Search and Filter
- Search by condition name, treatment, medic, record type, and structured note text where allowed.
- Filter by date range, severity, outcome, and status.
- Use indexes/projections rather than scanning and reparsing rendered strings.
- Raw internal IDs should not be needed by players.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 45. Health Record Export
- Export should be explicit user action.
- Support structured JSON and optionally human-readable text.
- Export uses a stable schema/version and does not alter campaign state.
- Do not expose unrelated account/user information.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 46. Export Scope
- Allow one survivor, selected survivors, or all archived records if UI supports it.
- Include generated timestamp only in exported file metadata, not gameplay state.
- Export can include localized display text plus stable IDs.
- Mark manual notes separately from verified records.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 47. Sharing Between Medics
- In a single-player shelter, medics should access the same shelter medical chart by default.
- Do not simulate manual paper transfer unless that mechanic exists.
- If factions/settlements later exchange records, make that a follow-on data-transfer system.
- The source's 'share records between medics' should not create busywork.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 48. Record Retention
- Major medical records persist for survivor lifetime and archive.
- Routine measurements and low-level events can be compacted.
- Never delete diagnosis, major injury, surgery, chronic onset, vaccination administration, adverse reaction, or terminal outcome merely for optimization.
- Retention rules must be documented and tested.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 49. Deceased Survivor Archive
- On death, close ongoing episodes with terminal/unfinished outcome only if authoritative systems provide it.
- Move or mark chart as archived rather than deleting it.
- Preserve medic history, treatment, radiation, chronic-condition, and vaccination records.
- Archive remains read-only.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 50. Fatal Outcome
- Do not set a medical episode outcome to fatal unless death causation is known.
- If cause of death is unknown/general, record survivor death separately and leave unrelated episodes ongoing/closed appropriately.
- Do not infer causality from active disease.
- Cause-of-death linkage belongs to fate/medical authority.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 51. Old Save Migration
- Default old saves to empty HealthHistoryState.
- Do not reconstruct a lifetime chart from current disease/chronic snapshots unless exact historical events are persisted elsewhere.
- Exact DoseLedger history may remain queryable but should not be copied retroactively into fabricated records.
- Future events begin normal tracking.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 52. Selective Reconstruction
- Where upstream persistent logs already contain exact events, reconstruct them with `migrationProvenance`.
- Examples: radiation dose ledger, surgery history, explicit vaccine administration ledger.
- Suppress all retroactive notifications.
- Never guess dates, medics, treatments, or outcomes.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 53. Migration Current Active Condition
- Do not create a fake historical diagnosis date for a condition merely because it is currently active.
- Optionally create a `legacy_active_at_migration` anchor record with unknown onset if UI continuity requires.
- Mark date as migration/current day and provenance as legacy.
- Do not present it as the true onset date.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 54. Stable Record IDs
- Use source-event-derived IDs where possible.
- Example: `medrec:<survivorId>:<sourceEventId>:<recordKind>`.
- Episode IDs may derive from authoritative condition instance/episode ID.
- Do not use wall-clock timestamps or UI sequence alone.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 55. Idempotency
- Every upstream event is processed at most once.
- Save/load replay cannot duplicate records.
- Multiple systems summarizing one treatment must share/root the same source event where possible.
- Add duplicate-event tests for Disease → MedicalPipeline → Journal fan-out.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 56. Record Amendments
- If a diagnosis is corrected, do not silently overwrite the old record.
- Add an amendment/correction event that references the prior record.
- UI displays current interpretation while preserving audit history.
- Only authoritative diagnosis correction can amend clinical facts.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 57. Data Integrity
- Validate record/event type IDs, template IDs, condition IDs, treatment IDs, vaccine IDs, metric IDs, localization keys, and survivor references.
- Unknown dynamic IDs must have registered namespaces/fallback behavior.
- Fail base-game catalog errors in CI.
- Historical missing references should degrade safely after mods/content removal.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 58. Medical Record Templates
- Template data should define labels, grouping, optional fields, and rendering keys.
- No arbitrary expressions or executable formulas.
- Templates must tolerate missing optional medic/location/condition details.
- Localization should avoid clinical overstatement.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 59. Privacy/Visibility Boundary
- Within a single-player shelter, medical records are usually visible to the player.
- If future secrecy/privacy mechanics exist, layer permissions above HealthHistory.
- Do not add hidden medical-record access penalties in v1.
- Export remains explicit.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 60. Alert Types
- Booster due, sustained declining trend, chronic-condition progression, significant radiation threshold, unresolved severe episode, and treatment follow-up may be alert candidates.
- Do not create 'history completed' as a routine gameplay alert.
- Every alert must be actionable or genuinely important.
- Use one alert state with cooldown/resolution semantics.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 61. Radiation Alert
- Radiation threshold comes from DoseLedger/RadiationSystem, not HealthHistory.
- History may display and alert on emitted threshold event.
- Do not recalculate dose risk independently.
- Long-term radiation trend remains a projection.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 62. Chronic Progression Alert
- Plan 193 emits progression/stabilization.
- History records it and UI can surface it.
- Do not poll chronic state and create duplicate progression records.
- Alert clears only according to chronic-condition status or user acknowledgement policy.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 63. Checkup Alert
- Routine checkup reminders are optional.
- Do not turn the system into compulsory appointment administration.
- Use a policy or low-frequency recommendation if gameplay value exists.
- Skip entirely if checkups add no treatment/diagnostic value.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 64. Quest Hooks
- Export facts for treated-patient count, valid records, vaccinations, checkups, prevented illnesses only when DiseaseSystem can causally support prevention.
- Do not count record creation itself as medical success.
- QuestSystem owns progress/rewards.
- `maintain complete records` must have a precise completeness definition.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 65. Achievement Hooks
- Plan 149 can observe milestones such as first complete chart, broad vaccination coverage, major recovery, or long-term medical stewardship.
- Do not reward documentation spam.
- Unique patient/episode IDs should drive counts.
- Achievement logic remains outside HealthHistory.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 66. Epilogue Integration
- Plan 145 may consume long medical-care arcs, extraordinary recoveries, epidemic survival, long-term radiation burden, or famous medic contributions.
- Export structural facts only.
- Do not turn the medical chart into prose-generation authority.
- Manual notes should not directly drive canonical epilogue claims.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 67. Archive Integration
- ShelterArchive may reference major medical cases, outbreaks, breakthrough treatments, or exceptional care.
- HealthHistory supplies candidate records/episodes.
- Archive owns curation.
- Routine checkups and ordinary medications are not archive-worthy.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 68. Modding Integration
- If Plan 165 exists, expose registered record types/templates/metrics carefully.
- Mods may add templates and known type IDs but should not inject arbitrary code through data.
- Missing mod condition/treatment IDs render historical placeholders rather than disappearing silently.
- Save fingerprinting remains Plan 165's responsibility.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 69. Performance Budget
- History writes are event-driven and low-frequency.
- Indexes should support survivor, record type, condition, date, severity, and outcome.
- UI should page/virtualize very long charts.
- Do not rebuild all trend graphs every frame.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 70. Trend Storage Budget
- Daily trend samples across hundreds of survivors and long campaigns can explode save size.
- Use metric-specific downsampling and bounded recent windows.
- Prefer deriving radiation/chronic counts live when cheap.
- Persist only measurements needed for trend history.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 71. Record Storage Budget
- Most survivors should generate tens to hundreds of meaningful events, not thousands of dose/medication micro-events.
- Summarize medication courses and repeated procedures where appropriate.
- Do not duplicate data already persisted in upstream ledgers.
- Stress-test 100 survivors over long campaigns.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 72. Headless Parity
- All record creation comes from Core events, not UI callbacks.
- Opening/closing medical panel cannot generate history.
- Export cannot affect medical state.
- Selftest should run with no UI nodes.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 73. Medical UI Projection
- Expose read-only DTOs for summary, timeline, vaccination view, trends, and alerts.
- Projection combines current authoritative state with historical records where appropriate.
- Do not let UI calculate contraindications or treatment eligibility.
- Separate current conditions from historical resolved episodes.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 74. Timeline Grouping
- Group events under medical episodes and collapse low-value details by default.
- Allow expand/collapse.
- Show major milestones prominently: diagnosis, intervention, complication, recovery.
- This keeps extensive chronic histories readable.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 75. Trend Visualization
- Use one chart per metric or a selectable chart surface.
- Clearly label units and whether a value is cumulative, count, score, or rate.
- Do not overlay incomparable metrics on one axis.
- Mark measurement gaps honestly.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 76. Vaccination UI
- Show vaccine, administered day, dose/series status, booster due, and current protection status from the immunity authority.
- Do not show historical stored immunity level as if it were current if immunity is derived elsewhere.
- Group vaccine series.
- Expired protection remains historically recorded.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 77. Medic Decision-Support UI
- For a proposed treatment, show relevant prior facts: allergy, contraindication, prior failed/successful course, related surgery, recent radiation/chronic state.
- Use concise explanations.
- History provides evidence; MedicalPipeline provides treatment validity/result.
- Never show unsupported causal claims.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 78. Manual Notes UI
- Allow medic notes only if game design values them.
- Label author and date.
- Free text should not be searchable into mechanics unless explicitly parsed by a safe manual tagging system.
- Deletion/editing policy should preserve audit expectations if notes are used strategically.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 79. Record Export UI
- Export command should select survivor and format.
- Give a deterministic filename plus current real-world export timestamp if desktop environment allows, but this timestamp is outside game state.
- Include schema version.
- Do not claim inter-medic sharing requires export in ordinary single-player play.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 80. Verification Matrix
- Test seven+ record types and seven+ event types.
- Test disease, trauma, radiation, chronic-condition, vaccination, checkup, treatment, recovery, relapse, complication, and death/archive flows.
- Test no-record healthy survivor and extensive-history survivor.
- Test 10-year-equivalent trend downsampling if long modes exist.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 81. Selftest
- `--health-history-selftest` should create a survivor, diagnose disease, treat, recover, vaccinate, add radiation exposure, add chronic condition, perform checkup, save/reload, and archive on death.
- Replay duplicate upstream events and verify no duplicate records.
- Verify contraindication query, trend compaction, and export projection.
- Exit non-zero on any mismatch.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 82. Data Integrity Selftest
- Validate record templates against medical catalogs.
- Validate treatment/condition/vaccine references.
- Validate metric aggregation strategies.
- Validate retention policies.
- Validate localization keys and event-type registrations.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 83. Golden Scenario — Illness Episode
- Day 10 diagnosis, Day 11 medication course, Day 12 complication, Day 15 treatment adjustment, Day 20 recovery.
- Expected: one illness episode with linked events, not five disconnected top-level diseases.
- Search by disease returns the whole episode.
- Save/load preserves order and IDs.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 84. Golden Scenario — Radiation Continuity
- Several DoseLedger entries accumulate, one clinically significant exposure event occurs, later radiation sickness is diagnosed, then chronic radiation condition appears.
- Expected: DoseLedger remains dose truth; HealthHistory shows meaningful exposure/diagnosis/chronic links without duplicating the entire ledger.
- Trend reads cumulative dose from the proper authority.
- No double-counting.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 85. Golden Scenario — Adverse Drug Reaction
- Medication is administered, MedicalPipeline emits adverse reaction, medical authority confirms allergy.
- History records intervention, reaction, and allergy fact with provenance.
- Future treatment query sees the allergy.
- History itself does not block treatment; MedicalPipeline does.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 86. Golden Scenario — Legacy Save
- Old save has current disease and radiation ledger but no medical history.
- Default migration creates no fabricated past chart.
- If a legacy-active anchor is enabled, it is labeled unknown onset.
- Radiation ledger remains viewable without generating fake historical exposure records.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 87. Golden Scenario — Deceased Survivor
- Survivor has long history and dies.
- Active medical state closes according to authoritative death/fate data.
- Chart becomes archived/read-only.
- Historical medic names, chronic conditions, treatments, vaccinations, and major episodes remain readable.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 88. Trend Downsampling Test
- Generate daily measurements for multiple years.
- Verify recent daily samples, older weekly/monthly aggregates, and deterministic graph.
- Ensure cumulative metrics use correct aggregation.
- Save size stays bounded.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 89. Record Idempotency Test
- Replay the same diagnosis/treatment/recovery event stream multiple times.
- Record count and digest remain unchanged after first processing.
- Journal/notification consumers do not cause new records.
- This is a mandatory CI gate.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 90. Archive Retention Test
- Archive a survivor with minor and major records.
- Routine measurement detail may compact, but diagnoses, surgery, vaccination, adverse reaction, chronic onset, and major exposures remain.
- Search still works.
- No live-treatment API accepts archived survivor.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 91. Export Test
- Export one survivor to JSON.
- Verify schema version, stable IDs, record ordering, episode links, manual-note labels, and no mutation of save state.
- Human-readable text export may be separately snapshot-tested.
- Export should succeed headlessly where file APIs allow.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 92. Implementation Phase A — Audit & Contracts
- Audit all medical event emitters and persistent ledgers.
- Define episode/record/event IDs and authority boundaries.
- Create migration strategy and baseline fixtures.
- Exit when every desired record type has a canonical source or is explicitly deferred.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 93. Implementation Phase B — Core History Store
- Implement state DTOs, append APIs, indexes, idempotency, capture/restore, record templates, and survivor-scoped queries.
- Do not add trends or UI first.
- Exit when illness/injury/treatment histories persist correctly.
- Add digest tests.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 94. Implementation Phase C — Disease/Trauma/Medical Wiring
- Wire diagnosis, injury, treatment, procedure, complication, recovery, and relapse events.
- Group related events into episodes.
- Prevent duplicate fan-out.
- Exit with golden illness/injury scenarios.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 95. Implementation Phase D — Radiation/Chronic Integration
- Wire clinically significant radiation events and DoseLedger references.
- Wire Plan 193 condition onset/progression/management/resolution.
- Do not duplicate active state.
- Exit with radiation/chronic golden scenarios.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 96. Implementation Phase E — Vaccination & Contraindications
- Integrate vaccine administration with the canonical immunity owner.
- Add allergy/adverse-reaction provenance.
- Expose treatment-history query adapter to MedicalPipeline.
- Exit when decision support can identify real contraindications without HealthHistory owning treatment rules.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 97. Implementation Phase F — Trends & Alerts
- Define metric sources, aggregation, downsampling, alert thresholds, cooldowns, and graph projections.
- Reject unnecessary composite health stats.
- Exit with bounded multi-year trend storage.
- Add decline/booster/radiation/chronic alert tests.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 98. Implementation Phase G — UI
- Build patient summary, timeline, filters, vaccination status, trends, alert surface, medic decision support, and archive view.
- Use projections only.
- Add accessibility and large-history virtualization.
- Exit when UI contains no medical business logic.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 99. Implementation Phase H — Export/Archive
- Add explicit export path and deceased-survivor archival.
- Preserve major records while allowing trend compaction.
- Add stable JSON schema.
- Exit with export/archive tests.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 100. Implementation Phase I — Migration & CI
- Implement empty-history migration plus exact selective reconstruction only where authoritative logs exist.
- Suppress retroactive notifications.
- Add selftest, data-integrity test, stress tests, trend compaction, and full regression.
- Exit when old and new campaigns are deterministic.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 101. Exact File Plan
- Core: `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs`.
- Core DTOs: `MedicalEpisode.cs`, `MedicalRecord.cs`, `HealthEvent.cs`, `VaccinationRecord.cs`, `HealthMeasurementSeries.cs`, `MedicalContraindicationFact.cs`.
- Data: `Assets/StreamingAssets/Data/medical_record_templates.json`.
- Tests: HealthHistorySystemTests, HealthHistoryPersistenceTests, MedicalEpisodeTests, VaccinationHistoryTests, HealthTrendTests, MedicalDecisionSupportTests, HealthHistoryMigrationTests.
- Docs: audit, retention, trend metrics, migration, coverage, export schema.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 102. Bootstrap / Composition Root
- Load medical record templates.
- Construct canonical medical systems first.
- Construct HealthHistorySystem after event contracts are available.
- Restore active medical state and history before binding medical UI.
- Wire event adapters, trend sources, contraindication query, archive, and export projections.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 103. No Generic Per-Frame Tick
- History writes on events.
- Trend sampling can run on daily/weekly simulation ticks.
- Alerts evaluate on relevant source changes or scheduled checkpoints.
- Never scan all records every frame.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 104. Medical Record Completeness Definition
- If a quest uses 'complete medical record', define completeness structurally.
- Example: all authoritative major medical episodes since HealthHistory activation are recorded and no unresolved ingestion errors exist.
- Do not require manual notes/checkups for completeness unless explicitly part of the quest.
- Migration-era unknown past should not make completion impossible.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 105. Risk Register
- Primary risk: duplicating medical state and letting history mutate treatment outcomes.
- Secondary risk: save bloat from daily trends, repeated medication doses, and verbose event logs.
- Other risks: false causality, fabricated old history, unreadable UI, and contraindication drift.
- Mitigate with event sourcing, sparse retention, authority adapters, episode grouping, and strict provenance.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 106. Definition of Done — Flagship
- `HealthHistorySystem.cs` exists with schema-versioned persistence.
- Seven+ meaningful medical record types and event types are supported.
- Illness/injury/treatment/radiation/chronic/vaccination/checkup integration is wired.
- Vaccination administration is historical and immunity ownership is explicit.
- Contraindication/allergy provenance is queryable.
- Trends are derived and downsampled.
- Medical UI supports summary/timeline/search/filter/vaccination/trends.
- Deceased records archive correctly.
- Export is stable and explicit.
- Old saves do not fabricate history.
- `--health-history-selftest` and data-integrity selftests pass.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 107. Follow-On 198-A — Epidemiological Cohort Analytics
- Aggregate de-identified shelter-level disease incidence and treatment outcomes.
- Do not turn individual medical history into a surveillance penalty.
- Use read-only projections.
- Useful for epidemic planning and medical research.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 108. Follow-On 198-B — Medical Research Feedback
- Allow ResearchSystem to consume anonymized treatment-outcome statistics.
- Research owns unlocks; history supplies evidence.
- Do not directly improve treatment effectiveness from record count.
- Requires a clear research protocol.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 109. Follow-On 198-C — Inter-Settlement Medical Record Exchange
- Support explicit transfer of selected records between allied settlements.
- Requires diplomacy/privacy policy and shared patient identity.
- Not needed for base single-player shelter medics.
- History export/serialization provides the foundation.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 110. Follow-On 198-D — Advanced Preventive Care
- Use history to schedule condition-specific checkups, screenings, and boosters where actual medical content exists.
- Avoid generic daily reminders.
- MedicalPipeline defines actionable prevention.
- History supplies timing/evidence.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 111. Follow-On 198-E — Historic Medical Cases
- Allow Archive/Epilogue to remember famous cases, breakthrough operations, epidemic survival, or extraordinary radiation recovery.
- History provides structural case facts.
- Narrative systems author presentation.
- Do not elevate ordinary records automatically.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---
## 112. Final Guardrails
- No second DiseaseSystem.
- No second TraumaSystem.
- No second Radiation/DoseLedger.
- No second ChronicConditionSystem.
- No hidden immunity model duplicated inside history unless explicitly designated.
- No per-frame record scan.
- No daily forever trend storage.
- No wall-clock RNG for medical history.
- No free-text notes becoming treatment mechanics.
- No fabricated cause-of-death linkage.
- No guessed relapse causality.
- No guessed allergy from generic treatment failure.
- No retroactive old-save chart fabrication.
- No duplicate record fan-out.
- No UI-owned contraindication/treatment logic.
- No generic prior-treatment bonus/penalty.
- No one opaque overall-health stat controlling gameplay.
- No routine medication-dose spam.
- No manual paper-sharing busywork between shelter medics.
- No archive deletion of major medical facts.

Implementation consequence: treat this concern as an integration boundary, not an isolated feature. The save model, source event, query projection, migration path, and headless validation must agree on the same authority before the UI is considered complete.

---

## Annex A — Recommended Core Contracts

### MedicalEpisode

```csharp
public sealed record MedicalEpisode
{
    public string EpisodeId { get; init; }
    public string SurvivorId { get; init; }
    public string EpisodeTypeId { get; init; }
    public string? ConditionDefinitionId { get; init; }
    public string? SourceConditionInstanceId { get; init; }
    public int StartDay { get; init; }
    public int? EndDay { get; init; }
    public string OutcomeId { get; init; }
    public IReadOnlyList<string> EventIds { get; init; }
}
```

### MedicalRecord

```csharp
public sealed record MedicalRecord
{
    public string RecordId { get; init; }
    public string SurvivorId { get; init; }
    public string RecordTypeId { get; init; }
    public int RecordedDay { get; init; }
    public string SourceEventId { get; init; }

    public string? EpisodeId { get; init; }
    public string? SeverityId { get; init; }
    public string? OutcomeId { get; init; }

    public string TemplateId { get; init; }
    public IReadOnlyDictionary<string, string> FactRefs { get; init; }
}
```

### HealthEvent

```csharp
public sealed record HealthEvent
{
    public string EventId { get; init; }
    public string SurvivorId { get; init; }
    public string EventTypeId { get; init; }
    public int EventDay { get; init; }
    public string SourceEventId { get; init; }

    public string? EpisodeId { get; init; }
    public string? RelatedConditionId { get; init; }
    public string? RelatedTreatmentId { get; init; }
    public string? TreatingSurvivorId { get; init; }
    public string? OutcomeId { get; init; }
}
```

### MedicalContraindicationFact

```csharp
public sealed record MedicalContraindicationFact
{
    public string FactId { get; init; }
    public string SurvivorId { get; init; }
    public string FactTypeId { get; init; } // allergy / adverse_reaction / contraindication
    public string SubjectDefinitionId { get; init; }
    public string SourceEventId { get; init; }
    public int RecordedDay { get; init; }
    public bool Active { get; init; }
}
```

The exact DTO shapes should follow repository serialization conventions, but their authority boundaries should
remain intact.

---

## Annex B — Medical History Event Ingestion Matrix

| Upstream System | Event | History Result | Duplicate State Forbidden |
|---|---|---|---|
| DiseaseSystem | diagnosis | illness episode + diagnosis event | active disease |
| DiseaseSystem | recovery | episode close + recovery event | disease resolution |
| MedicalPipeline | treatment complete | treatment event | treatment success logic |
| MedicalPipeline | surgery | procedure record | surgery outcome |
| CombatTrauma | severe injury | injury episode | wound state |
| RadiationSystem | clinically significant exposure | exposure record | cumulative dose |
| DoseLedger | measurement | trend/reference | dose ledger |
| ChronicConditionSystem | onset/progression | chronic history event | active condition |
| Vaccination authority | administered | vaccination record | immunity math |
| Medical assessment | checkup | checkup record | current health state |
| SurvivorLifecycle | death | archive/terminal context | cause of death unless known |

---

## Annex C — Trend Retention Policy Example

Recommended initial strategy:

```text
0–30 days old:
  keep daily points

31–180 days:
  aggregate weekly

181+ days:
  aggregate monthly
```

Metric-specific aggregation:

- cumulative radiation dose → last/max cumulative value for interval;
- active chronic count → last or max;
- active disease burden → mean/max depending UI purpose;
- treatment burden → sum of events;
- vaccination coverage → last state.

Do not use one universal average.

---

## Annex D — Decision-Support Acceptance Scenario

A survivor previously received Drug A for Disease X and experienced a verified adverse reaction. The medical
authority subsequently created an allergy/contraindication fact.

Months later Disease X recurs.

Expected flow:

1. DiseaseSystem diagnoses current Disease X.
2. HealthHistory exposes prior episode and active contraindication fact.
3. MedicalPipeline evaluates available treatments.
4. Drug A is blocked or penalized **by MedicalPipeline rules**, not by HealthHistory.
5. Medic UI displays why Drug A is contraindicated and surfaces prior reaction date.
6. Selecting another treatment creates a new treatment event.
7. The old history remains immutable.

This is the flagship proof that history informs care without becoming a second treatment engine.

---

## Annex E — Old Save Migration Matrix

### Old save, no historical logs
- create empty HealthHistoryState;
- no fabricated diagnoses/treatments;
- current health remains owned by active systems.

### Old save with current chronic condition
- do not invent onset date;
- optional `legacy_active_at_migration` anchor if required for chart continuity.

### Old save with DoseLedger
- retain DoseLedger as source;
- trend can begin from available historical measurements if exact dates exist;
- do not create synthetic radiation illness records.

### Old save with exact vaccination ledger
- reconstruct only if source contains actual administration events.

### Notification behavior
- all reconstructed data is silent;
- only future events produce normal notifications.

---

## Annex F — Extensive Medical History Stress Scenario

Generate one survivor with:

- 12 illness episodes;
- 7 significant injuries;
- 3 surgeries;
- 4 vaccination series;
- 2 adverse reactions;
- 1 chronic condition;
- 20 checkups;
- 10 years of trend-equivalent measurements.

Expected:

- timeline remains queryable;
- episode grouping is readable;
- trend storage is compacted;
- search/filter remains fast;
- save/load digest stable;
- UI does not render thousands of hidden rows;
- archive remains available after death.

---

## Annex G — CI Verification Commands

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --health-history-selftest
```

Also run any existing:
- medical-pipeline selftest;
- disease selftest;
- radiation/dose selftest;
- chronic-condition selftest;
- save migration gates;
- archive/journal regression gates.

---

## Annex H — Final Execution Sequence

1. Audit source medical events and ledgers.
2. Freeze authority boundaries.
3. Implement sparse history state and stable IDs.
4. Wire illness/injury/treatment episodes.
5. Wire radiation/chronic-condition references.
6. Establish vaccination/immunity ownership.
7. Add allergies/contraindications and MedicalPipeline read adapter.
8. Add bounded trends.
9. Add actionable alerts.
10. Implement UI projections.
11. Add archive/export.
12. Migrate old saves conservatively.
13. Run idempotency, stress, downsampling, and headless tests.
14. Run full regression.
15. Only then author optional quest/achievement polish.

When complete, Plan 198 should make every survivor's health feel continuous without making the player maintain
paperwork. The chart should answer the important questions immediately: what happened, what was done, what
worked, what failed, what remains ongoing, what risks are known, what protection is current, and what history
matters to the next treatment decision.
