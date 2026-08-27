# Actionable Execution Plan: ASHFALL Forensic Remediation

**Status:** REVISED — forensic reconciliation required before broad mutation
**Date:** 2026-08-23
**Scope:** The six subsystem forensic surveys, the consolidated forensic report, `DEEP_ANALYSIS_254_SUBSYSTEMS.md`, `DEEP_INTEGRATION_PLAN.md`, and the previous `ACTIONABLE_EXECUTION_PLAN.md`
**Explicitly out of scope:** the separate deep code audit and its remediation roadmap
**Mode:** Evidence-first, dependency-ordered, reversible changes with explicit exit criteria

---

## 1. Why this plan is being revised

The forensic work is useful, but the reports are not internally consistent enough to treat every count and classification as execution truth.

The current plan assumes that:

- the subsystem inventory is already canonical;
- every `PORTED_NOT_WIRED` entry is a true runtime orphan;
- all reported zero-test statuses are current;
- `schema_version` can safely be added to hundreds of JSON files in one broad migration;
- new `HostSession` classes are the default remedy for every unhosted Core type;
- host integration tests belong in `Ashfall.Core.Tests`;
- the large `Main` partial should be decomposed by making each HostSession own Core state, save state, UI panels, dirty state, and ticking.

Those assumptions are too aggressive.

The revised plan separates **forensic reconciliation**, **correctness repair**, **orchestration isolation**, **orphan disposition**, and **catalog-schema migration**. No broad refactor or data sweep begins until the underlying forensic evidence is normalized.

---

## 2. Forensic inconsistencies that must be treated as blockers

### 2.1 Inventory/count drift

The reports variously describe:

- 243 unique subsystems;
- 254 unique subsystems;
- 267 batch entries;
- numbering that extends beyond 254;
- 15 orphan Core systems;
- 16 orphan/missing systems;
- 224 LOW / 2 MEDIUM / 0 HIGH in one summary while the tables contain a HIGH-risk `SurvivorsHostSession` and many MEDIUM entries.

**Plan rule:** ordinal numbers and summary totals are not authoritative. Canonical identity must be based on type/concept name plus source path and runtime ownership.

### 2.2 “Orphan” classification drift

At least two entries are self-contradictory inside the forensic set:

- `PhantomMemorySystem` is reported as having no Godot host, but a later report records `PhantomMemoryHostSession`.
- `MaritimeDiveSystem` is reported as having no Godot host, while the same survey family records `MaritimeHostSession` as wiring `MaritimeDiveSystem`.

Other candidates such as `BallisticsSystem`, `SkillAtrophySystem`, and `WeaponConditionSystem` may be **Core-internal collaborators**, not features that require their own host session.

**Plan rule:** never create a HostSession solely because a table says `PORTED_NOT_WIRED`.

### 2.3 Test-count/status drift

The reports cite different global test totals. The previous plan also says that three new catalog files contain four tests each while reporting only `9/9` tests executed.

**Plan rule:** “complete” means verified against the current tree, not copied from an earlier report.

### 2.4 Schema-version policy contradiction

The deep analysis reports roughly 317/318 JSON files without `schema_version`, while the previous plan says `CatalogIntegrityValidator` already validates schema-version presence and simultaneously reports a green data-integrity gate.

Those statements cannot all describe the same enforcement policy.

**Plan rule:** define and verify the schema-version contract before mutating catalog roots.

---

# PHASE F0 — Reconcile the forensic evidence first

This is the new prerequisite phase. It is intentionally read-only except for generated forensic documentation.

## F0-1 — Build one canonical subsystem registry

Create a generated registry such as:

```text
docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md
docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.csv
```

Each unique entry should contain:

| Field | Meaning |
|---|---|
| `canonical_name` | Actual class/concept name |
| `source_path` | Primary Core/host/catalog path |
| `kind` | gameplay system / catalog / host session / save store / distributed concept |
| `core_authority` | yes/no/path |
| `host_entrypoint` | direct / indirect / none |
| `runtime_reachable` | proven / not proven |
| `save_owner` | system/store/envelope/none |
| `test_surface` | Core tests / Godot selftest / aggregate integrity / none |
| `classification` | normalized classification |
| `risk` | normalized LOW/MEDIUM/HIGH |
| `evidence_source` | forensic batch/report |
| `status_confidence` | verified / contradictory / stale candidate |

### Deduplication rules

1. Deduplicate by semantic type/concept + source path, not report number.
2. A HostSession is a separate architectural component from the Core system it hosts.
3. A distributed concept such as “WorldSystem” or “VerdictSystem” is not automatically a missing class.
4. Do not count the same class twice because it appears in two batch scopes.
5. Do not infer “orphan” from `0 Host files`; inspect runtime references.

### Exit criteria

- One row per unique component.
- No duplicate canonical identities.
- Every conflicting forensic entry is marked resolved or explicitly unresolved.
- Final counts are regenerated from the registry, not hand-maintained.

---

## F0-2 — Reclassify all orphan candidates by runtime reachability

Use five dispositions:

| Disposition | Definition | Default action |
|---|---|---|
| `DIRECT_HOSTED` | Dedicated host/session owns or exposes it | No new host |
| `INDIRECT_HOSTED` | Composed by another runtime system/session | No new host unless UI/lifecycle requires one |
| `CORE_INTERNAL` | Collaborator/value service used by another Core authority | Keep Core-only |
| `FEATURE_DEFERRED` | Valid implemented feature intentionally not active | Track feature decision |
| `TRUE_ORPHAN` | No runtime owner/reference and intended to ship | Wire or retire |

Start with the reported candidate set:

```text
BallisticsSystem
CaregivingSystem
ExpeditionVehicleSystem
IdeologicalFrictionSystem
LeadershipSystem
PhantomMemorySystem
RationConflictSystem
MaritimeDiveSystem
OrbitalHarrowTelemetrySystem
PharmaLabSystem
SkillAtrophySystem
TraumaBondSystem
WeaponConditionSystem
WeatherStationSystem
WorkshopReverseEngineeringSystem
```

### Mandatory reconciliation examples

- Re-check `PhantomMemorySystem` against `PhantomMemoryHostSession`.
- Re-check `MaritimeDiveSystem` against `MaritimeHostSession`.
- Re-check `BallisticsSystem` as a TacticalCombat collaborator before considering a new host.
- Re-check `SkillAtrophySystem` against skill-progression ownership.
- Re-check `WeaponConditionSystem` against combat/equipment ownership.

### Exit criteria

No candidate is labeled “wire a HostSession” until it has:

- a proven runtime feature requirement;
- an intended tick/lifecycle owner;
- save semantics;
- an integration surface;
- an explicit reason a separate HostSession is preferable to indirect composition.

---

## F0-3 — Re-establish the executable baseline

Do not carry forward historical counts as current truth.

Capture:

```text
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Record actual:

- build result;
- test count;
- failing/skipped tests;
- data-integrity result;
- bridge result;
- relevant Godot selftests available for survivor/save/world/inventory flows.

### Reconcile the reported P0-3 catalog tests

The prior plan states:

- 3 files;
- 4 tests per file;
- `9/9` executed.

Verify the real number and only then mark the work complete.

### Exit criteria

`BASELINE_FORENSIC_VERIFICATION.md` contains one current, reproducible set of results and replaces copied historical counts in this plan.

---

# PHASE P0 — Correctness and state authority

## P0-1 — Lock survivor/needs save behavior before refactoring

The first survey flags:

- `NeedsSystem` save/load round-trip coverage gap (H11);
- `SurvivorsHostSession` host/Core duplication (H1).

These are coupled and should be addressed in that order.

### Step A — Characterization tests

Before changing ownership, prove current behavior for:

- survivor registration;
- hunger/thirst/fatigue/warmth/morale/health/hygiene values;
- radiation state;
- death/alive state;
- hourly tick propagation;
- capture → serialize → restore → capture equivalence;
- load of an existing representative survivor save if fixtures exist.

### Step B — Define authority explicitly

Target rule:

```text
Core systems own simulation truth.
Host owns orchestration/adaptation only.
UI receives read-only projections.
Save code captures canonical Core state plus explicitly host-owned presentation/session state only.
```

### Exit criteria

There is a failing test demonstrating H11 before the fix, then a passing regression after the fix.

---

## P0-2 — Remove semantic duplication from `SurvivorsHostSession`

Do **not** mechanically replace the host state class with a mutable Core DTO without checking API and save compatibility.

### Required end state

`SurvivorsHostSession` may keep an adapter/view model when useful, but it must not own a second independently mutable copy of needs/radiation truth.

Preferred shape:

```text
NeedsSystem / RadiationSystem
        ↓ canonical state
SurvivorsHostSession
        ↓ read-only projection / commands
Survivors UI
```

### Migration sequence

1. Identify fields that are true simulation state vs UI/session metadata.
2. Move simulation mutations behind Core methods.
3. Replace host-owned mutable mirrors with projections/adapters.
4. Preserve save compatibility or add explicit migration logic.
5. Remove duplicate tick/state mutations.
6. Run survivor, radiation, save, bridge, and relevant headless tests.

### Guardrails

- No second survivor-needs authority.
- No direct UI mutation of canonical state.
- No save format change without compatibility coverage.
- No reliance on shared mutable DTO lists as the public UI contract.

---

## P0-3 — Close correctness-grade test gaps, not every zero-test wrapper

Do not treat every thin HostSession with zero dedicated unit tests as a defect.

Prioritize:

1. `NeedsSystem` save round-trip.
2. `SurvivorsHostSession` authority/save integration.
3. `PhantomMemorySystem` only if F0 confirms it still lacks meaningful behavior coverage.
4. Any catalog with a unique parser/schema not covered by aggregate catalog validation.
5. Central host hubs through host-aware tests (see P1-2).

For simple narrative catalogs, prefer one strong parameterized/aggregate catalog-contract suite over dozens of near-identical one-file tests unless the loader has special behavior.

---

# PHASE P1 — Isolate host orchestration safely

## P1-1 — Decompose `Main.ExpandedShelterSystems.cs` without creating duplicate HostSessions

The forensic analysis identifies `Main`/its partials as the major blast-radius concentration. At the same time, later batches already list many dedicated HostSession types for expanded-shelter features.

Therefore the goal is **not** “create 20 new HostSessions.”

The goal is:

- use the existing HostSession types where they already exist;
- move lifecycle/dirty/save/tick ownership out of the giant partial in small slices;
- keep UI composition separate from simulation/session logic;
- avoid turning each HostSession into a mini god object.

### Responsibility boundaries

**HostSession should own:**
- Core-facing commands/adaptation;
- host-specific runtime events;
- minimal host lifecycle required by that subsystem.

**SaveStore/SaveCoordinator should own:**
- persistence envelope;
- file IO;
- dirty flushing;
- migration/checksum policy.

**UI composition should own:**
- panel creation;
- view binding;
- navigation/open/close.

**Feature coordinator should own:**
- cross-session dependency ordering;
- ticking/lifecycle fan-out where needed.

### Do not use the previous pattern

Avoid a HostSession that simultaneously owns:

```text
Core System + Panel + SaveStore + dirty flag + navigation + global ticking
```

That merely moves the god object into 20 smaller coupled objects.

### Slice order

Work in 3–5 component slices rather than a 33-file mega-refactor.

Suggested first slice: low-dependency components already reported with dedicated sessions, for example:

```text
WaterTreatment
AirlockSecurity
RegionalTreaty
VinylMorale
WildlifeTrapping
```

Then proceed to dependency-heavy slices:

```text
ShelterThermal / ShelterSchedule / SumpFlooding
Decontamination / KitchenNutrition / EquipmentCondition
LibraryStudy / ArchiveDesk / ContractorRoster
MentalHealthCrisis / Autopsy
```

The exact order must be regenerated from F0 runtime dependency evidence.

### Per-slice exit criteria

- `Main.ExpandedShelterSystems.cs` loses concrete responsibility.
- Existing behavior and save state remain equivalent.
- No duplicate host session is introduced.
- Each moved dependency has one clear owner.
- Relevant Godot headless tests pass before the next slice.

---

## P1-2 — Test host hubs in a host-aware surface

The previous plan proposes:

```text
Ashfall.Core.Tests/InventoryHostSessionTests.cs
Ashfall.Core.Tests/WorldHostSessionTests.cs
Ashfall.Core.Tests/Phase0HostSessionTests.cs
```

That is only valid if those classes are intentionally compilable without the Godot host assembly.

Preferred options:

1. add/extend Godot headless selftests through the existing host CLI;
2. create a dedicated host test project if the repository supports it;
3. extract pure coordinator logic into engine-neutral classes and unit-test those separately.

### Required integration scenarios

**Inventory hub**
- add/remove/transfer/equip flow;
- failed transaction does not partially mutate inventory;
- save/load retains inventory/equipment state.

**World hub**
- weather/map/radiation/landmark propagation;
- deterministic tick ordering;
- save/load world-state round-trip.

**Phase0 hub**
- survivor/medical/mental-health integration;
- expansion/feature gating;
- save/load and rehydration;
- no duplicate tick ownership.

---

## P1-3 — Resolve `ExpansionHubSave` Phase 11 stubs

The first forensic batch identifies Phase 11 wiring stubs as a MEDIUM gap, but the old actionable plan omits them.

Disposition must be explicit:

- complete the missing wiring;
- remove obsolete stubs;
- or feature-flag/document them as intentionally deferred.

### Exit criteria

No production save path contains ambiguous placeholder wiring without an owner/issue/feature flag.

---

# PHASE P2 — Orphan disposition and data-schema evolution

## P2-1 — Dispose true orphan systems one by one

After F0-2, create one decision record per remaining `TRUE_ORPHAN`.

Required decision fields:

| Field | Requirement |
|---|---|
| Intended player-facing feature | yes/no |
| Runtime trigger | event/tick/UI/command |
| Owning feature/session | named |
| Save state | none / existing / new |
| Determinism | RNG/ordering requirement |
| Test surface | named |
| Decision | wire / indirect-compose / defer / retire |

### Rule

**Do not automatically add a HostSession.**

If a system is a collaborator of another Core authority, integrate it there.

If a feature has no product/runtime requirement, retire or explicitly defer it rather than making dead code “live” solely to satisfy a forensic table.

---

## P2-2 — Define the catalog schema-version contract before bulk migration

The reported `317/318` gap is important, but it is not safe to convert directly into a 317-file write.

### Step A — Establish policy

Document:

- which JSON files are versioned schemas vs static content blobs;
- whether version metadata must live inside every file;
- how list-root files are represented;
- compatibility expectations for old shapes;
- generated vs authored files;
- loader fallback duration.

### Step B — Add loader-level version support first

The loader must be able to:

- read legacy unversioned form;
- read versioned form;
- reject unsupported future versions clearly;
- preserve data semantics.

### Step C — Pilot representative files

Select a small matrix:

1. one object-root catalog;
2. one bare-list catalog;
3. one wrapper-list catalog;
4. one expansion catalog;
5. one narrative catalog if structurally different.

Do not proceed until all current catalog tests and data-integrity checks remain green.

### Step D — Build an idempotent migration tool

Required modes:

```text
--check   # report what would change; no writes
--write   # mutate only validated eligible files
```

The tool should parse JSON. Do not detect `schema_version` by searching the first 200 bytes.

It must:

- classify root shape;
- preserve semantic content;
- avoid double wrapping;
- avoid generated/non-authoritative files unless explicitly included;
- output a migration manifest.

### Step E — Migrate by loader family

Use small reviewable batches, not one 317-file commit.

Suggested batch size:

```text
10–30 files or one loader family per change
```

### Exit criteria

- Every migrated file is readable by its canonical loader.
- Legacy fixtures remain readable for the documented compatibility window.
- `--check` is idempotent after migration.
- Aggregate data-integrity remains green.
- No root-shape mutation occurs without a corresponding loader contract test.

---

## P2-3 — Add direct `CatalogFileSystem` tests

Cover:

- deterministic file discovery;
- nested path resolution;
- missing root/file behavior;
- platform path normalization;
- malformed JSON handling boundary, if owned here;
- schema metadata discovery only if F2 policy assigns it to this class.

Do not invent `GetSchemaVersion()` as a required API unless the class actually owns that concern.

---

# PHASE P3 — Complete the host-ownership cleanup

## P3-1 — Re-audit the reported 41 `Main.cs`-only systems

The “41” is provisional until F0 produces a canonical registry.

For each confirmed Main-owned system, score:

| Criterion | Weight |
|---|---:|
| Persistent mutable state | 3 |
| UI lifecycle | 2 |
| Cross-system dependencies | 2 |
| Independent tick lifecycle | 2 |
| External events/subscriptions | 2 |
| Pure one-time composition only | -2 |

Suggested disposition:

- high score → extract to explicit coordinator/session;
- medium score → keep composed but isolate save/tick ownership;
- low score → leave as composition-root wiring.

**Goal:** reduce hidden ownership, not maximize the number of classes.

---

## P3-2 — Consolidate repetitive catalog coverage

The forensic reports list multiple simple content catalogs with no dedicated tests.

Create a data-driven contract suite that can cover many catalogs consistently:

- file exists;
- parses through canonical loader;
- IDs are non-empty/unique where applicable;
- expected wrapper/root shape is accepted;
- references resolve through global integrity validator.

Keep bespoke tests only for catalogs with custom loader logic or gameplay semantics.

---

# 3. Revised execution order

| Order | Work item | Risk | Why now |
|---:|---|---|---|
| 1 | F0-1 canonical subsystem registry | LOW | Removes conflicting counts/classifications |
| 2 | F0-2 runtime reachability/orphan reclassification | LOW | Prevents unnecessary host code |
| 3 | F0-3 executable baseline/status reconciliation | LOW | Establishes current truth |
| 4 | P0-1 survivor/needs save characterization | LOW | Locks behavior before refactor |
| 5 | P0-2 SurvivorsHostSession authority fix | MEDIUM | Highest confirmed semantic risk |
| 6 | P0-3 correctness-grade test gaps | LOW | Protects subsequent structural work |
| 7 | P1-1 Main expanded-shelter decomposition in slices | HIGH | Largest confirmed coupling cluster |
| 8 | P1-2 host-hub integration tests | MEDIUM | Protects runtime wiring |
| 9 | P1-3 ExpansionHubSave stub disposition | MEDIUM | Closes reported save ambiguity |
| 10 | P2-1 true-orphan disposition | MEDIUM | Uses reconciled reachability evidence |
| 11 | P2-2 schema-version policy + pilot + staged migration | HIGH | Large blast radius; now protected |
| 12 | P2-3 CatalogFileSystem direct tests | LOW | Infrastructure hardening |
| 13 | P3-1 remaining Main ownership audit | MEDIUM | Broader architectural convergence |
| 14 | P3-2 aggregate catalog coverage | LOW | Reduces repetitive test debt |

### Critical path

```text
F0 reconciliation
    ↓
Needs/Survivor characterization
    ↓
SurvivorsHostSession authority fix
    ↓
Main orchestration isolation
    ↓
true-orphan disposition
    ↓
schema migration
```

The schema sweep is deliberately **not** on the initial critical path.

---

# 4. Verification matrix

Run the common gates after every behavior-changing slice:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Add change-specific gates:

| Change type | Additional verification |
|---|---|
| survivor/needs | save round-trip + survivor/radiation headless scenario |
| save wiring | old-save fixture + capture/restore equality + checksum/tamper test |
| Main refactor | affected panels/selftests + tick ownership assertion |
| host hub | Godot headless integration scenario |
| orphan wiring | explicit feature-trigger scenario proving runtime reachability |
| schema migration | loader-family contract tests + legacy fixture + migration `--check` |
| catalog test consolidation | full data-integrity + aggregate catalog suite |

### Non-negotiable invariants

- No `UnityEngine.*` or `Godot.*` references enter Core.
- No new `System.Random` use in deterministic simulation.
- One semantic state authority per subsystem.
- Stateful systems retain capture/restore coverage.
- Host wrappers do not re-implement simulation rules.
- Failed resource transactions do not partially mutate state.
- Schema migration does not silently change catalog semantics.
- A refactor does not count as successful merely because the aggregate build passes.

---

# 5. Change packaging and rollback

Do not use broad destructive rollback commands against an active workspace.

Preferred approach:

1. one branch/worktree per phase or high-risk slice;
2. one logical concern per commit;
3. record baseline verification before the first commit;
4. revert commits, not wildcard paths;
5. never combine:
   - hundreds of JSON shape changes;
   - Main orchestration refactors;
   - save format changes;
   - orphan feature wiring
   in the same change set.

### Suggested PR/change-set sizes

- F0 registry: docs/generated evidence only.
- Survivor authority: one focused behavior PR.
- Main decomposition: 3–5 sessions/components per PR.
- Orphans: one feature family per PR.
- Schema migration: one loader family or 10–30 files per PR.

---

# 6. Definition of done

The forensic remediation track is complete when:

- [ ] the canonical registry has one normalized classification per component;
- [ ] conflicting orphan classifications are resolved;
- [ ] current baseline test/build counts are recorded and reproducible;
- [ ] H11 save-round-trip coverage exists;
- [ ] `SurvivorsHostSession` no longer owns duplicate simulation truth;
- [ ] `Main` no longer concentrates expanded-shelter lifecycle/save/UI policy in one partial;
- [ ] central host hubs have host-aware integration coverage;
- [ ] `ExpansionHubSave` Phase 11 stubs are completed, removed, or explicitly deferred;
- [ ] every true orphan has a documented wire/defer/retire decision;
- [ ] schema-version policy is explicit before broad data migration;
- [ ] schema migration, if approved, is staged, reversible, loader-tested, and idempotent;
- [ ] `CatalogFileSystem` has direct infrastructure coverage;
- [ ] remaining Main-only ownership is intentional and documented;
- [ ] all common and change-specific verification gates pass.

---

# 7. Immediate next action

Do **not** begin with the 317-file schema sweep.

Begin with:

```text
F0-1 — generate the canonical subsystem registry from all six forensic batches,
deduplicate by type/path, and reconcile the contradictory classifications.
```

That registry becomes the execution authority for the rest of this forensic-remediation plan.

---

## Final planning principle

The forensic reports are an excellent discovery corpus, but they should not themselves become a second source of truth.

The improved plan therefore follows one rule:

> **Prove ownership and reachability first; then change code or data.**

This keeps the forensic-remediation effort separate from the deep code audit while making the forensic findings safe to execute.
