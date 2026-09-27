# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> **Closed:** 2026-09-26 · **Claim:** `claim-quad-g-cassettes-guiltsources-flotilla-recordintegrity-2026-09-26`
> **Not committed** (per user direction). See §6 for the closeout evidence.

---

# PLAN-PATIENT-RECORD-INTEGRITY — Clinical Record Integrity Validation Host Integration

> **STATUS: APPROVED BY USER**
> **Package:** `PATIENT-RECORD-INTEGRITY`
> **Category:** medical / data integrity
> **Plan type:** run an unhosted Core validator through the live integrity seam.
> **Date of evidence:** 2026-09-26, branch `integration/all-latest-2026-09-24`

---

## 1. Objective

Bind `Assets/Ashfall.Core/Medical/PatientRecordIntegrityValidator.cs` to the
**already-live** `MedicalPipelineCoordinator` / `MedicalPipelineSaveState`, so
clinical records are checked for dangling survivor, treatment, and item
references at load and at save — instead of the authored pipeline silently
carrying broken references.

**Bounded outcome:** one validation call with a real `Context` (roster, ward
eligibility, item catalog), findings surfaced through the medical host, and a
probe that proves each rule actually fires on a corrupted record.

**Non-goals:** no new record store, no repair-by-invention (a dangling reference
is reported, never guessed), no new save section, no change to clinical
behaviour.

## 2. Current Reality (re-verified 2026-09-26)

| Fact | Evidence |
| --- | --- |
| Orphan | `PatientRecordIntegrityValidator` has **zero** consumers in `src/` and zero runtime consumers in Core |
| Live state under test | `src/Host/MedicalPipelineSaveStore.cs`, `MedicalHostSession.cs:28` `Pipeline`, `:202` loads the pipeline save, `:247` `BindPipeline(...)` |
| Validator contract | `Validate(MedicalPipelineSaveState, Context)` → `List<MedicalPipelineIntegrityFinding>`, with an injectable `Context { IsKnownSurvivor, IsTreatmentEligible, IsKnownItem }` |
| Keyed to real owners | survivor ids resolve through the roster, items through `ItemCatalog`, eligibility through the ward owner |
| Integrity pipeline exists | AGENTS.md: "Validate catalog IDs, references, ranges, and consumers through the current integrity pipeline" |

## 3. Files

### New — Host
- `src/Host/PatientRecordIntegrityHostSession.cs`
- `src/Host/HostCli.PatientRecordIntegrity.cs`

### Modified — Core
- `Assets/Ashfall.Core/HostCliRegistry.cs` — `--patient-record-integrity-selftest`

### Modified — Host
- `src/Host/MedicalHostSession.cs` — validate after pipeline bind/load, expose findings
- `src/Main.Medical*.cs`, `src/Host/HostCli.cs`, `src/Main.Application.cs`
- `scripts/ci/generate-architecture-map.py`

### Modified — Tests
- `Ashfall.Core.Tests/Medical/PlanPatientRecordIntegrityHostIntegrationTests.cs` (new)

## 4. Acceptance

1. `--patient-record-integrity-selftest` ≥ 9/9: clean pipeline reports zero
   findings; each injected defect (unknown survivor, ineligible treatment,
   unknown item, duplicate episode) produces exactly its authored finding code;
   validation is read-only; repeat validation is idempotent.
2. Focused xUnit suite green.
3. The validator reports only — no record is mutated, created, or deleted.
4. Adjacent gates green (no section-count change).

## 5. Deferred with named reasons

- No auto-repair of dangling references: silently inventing a survivor or item
  id is exactly the "presence in JSON is not reachability" failure the rulebook
  warns about; repair needs a signed policy.
- No CI data-integrity gate wiring: that generator is integrator-owned.

## 6. Closeout evidence (2026-09-26)

**Verified fully integrated 2026-09-26** (no-commit
seal session): **not a partial.** Full Core↔host↔route↔persistence↔observable chain is
live and proven.

| Item | Evidence |
| --- | --- |
| Headless probe | `--patient-record-integrity-selftest` **10/10** |
| Focused xUnit | `PlanPatientRecordIntegrityTests` **11/11** |
| Save section | none — the pipeline save already persists via MedicalPipelineSaveStore |
| Host build | `dotnet build Ashfall.csproj` — 0 errors |
| Core test build | `dotnet build Ashfall.Core.Tests` — 0 errors |
| Registry pin | `ComprehensiveSaveStoreCorruptionAndMigrationTests` — 309 sections, 1932 assertions green with all adjacent gates |
| Adjacent gates | `SaveSectionRegistryTests`, `PersistentFilenameRegistry`, `DayEventVocabulary`, `DayEventParitySourceGate`, `HostCliActionParityGate`, `MainTriadDriftGate` (both), `SaveStoreMatrixGate`, `PortContractGate` — all green |
| Regenerated | selftest manifest **277** (275 headless) · CLI catalog **337 / 547** · save-store matrix **311** · port contract **307** |

**Authority boundary held (Rule 5):** no parallel ledger, no new morale/needs/item/
trust authority, no invented content — every value comes from the existing owners or
the authored JSON.

**No commit** (user directive). `INTEGRATION_PLANS.md` / `WORKTREE_OWNERSHIP.md`
intentionally unwritten (foreman / named-integrator only).
