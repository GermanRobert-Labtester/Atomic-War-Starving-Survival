# Plan 03 — Data-Authority Schema Version and JSON Policy Governance

> **Rebuild status:** COMPLETE FOUNDATIONAL SWEEP — CONTINUOUS ENFORCEMENT PLAN
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

- The original premise that only a small fraction of catalogs carried `schema_version` is stale. Current data policy requires object-root documents and integer schema versions, enforced by a dedicated gate.
- The remaining work is not a blind key rename. Each family must be inventoried against its actual loader; unknown fields, aliases and camelCase compatibility may be deliberate and must not be changed without a paired loader/migration package.
- A strong plan therefore includes a full current census, a per-family schema sheet, staged-mode behavior, and explicit waiver governance for any non-JSON data-authority artifact.

**Bounded outcome:** The one-time bulk edit is obsolete. `json-schema-policy-gate`, `CatalogIntegrityValidator`, `VersionReport`, and the JSON schema-policy tests now provide continuous enforcement. This plan becomes the authority for future catalog onboarding and exception handling.

# 2. Current Decision and Terminal/Residual Status

This distinction prevents historical plans from reopening completed systems. The following statements are the current planning truth:

- The repository has dedicated `json-schema-policy-gate.py` and shell entry points.
- Catalog integrity performs deeper ID, reference, range and uniqueness checks beyond schema presence.
- Version reporting inventories data schemas and participates in release/version contracts.
- Many current catalogs carry schema versions greater than one; adding version 1 blindly would be incorrect.

**Master-authority sections applied to this rebase:**

- Part III Lane H
- Volume 7 catalog contracts
- Volume 29 versioning authority

These sections supply anti-padding, planning, evidence, verification and domain-boundary discipline. Live source and current ledgers still win on every conflict.

# 3. Required Delta

The minimum safe delta is:

- Replace the one-time edit list with a current generated census and family exception process.
- Bind every new catalog kind to a loader, validator rule, consumer and support window.
- Treat staged/diff mode as the pre-commit path and full mode as CI authority.
- Record deliberate legacy key aliases as migration notes, not silent technical debt.

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
| root/schema_version enforcement | JSON schema policy gate | `scripts/ci/json-schema-policy-gate.py` | The gate is the mechanical policy authority. |
| IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | Semantic integrity is deeper than schema presence. |
| schema inventory and version reporting | VersionReport | `Assets/Ashfall.Core/VersionReport.cs` | Version reporting consumes catalog schema headers. |
| actual accepted shape and migration | Family loaders | `Assets/Ashfall.Core/**/*CatalogLoader.cs` | The loader is proof for compatibility, not JSON alone. |

The safe implementation route is to extend these seams. A plan that cannot name the current owner is not ready for implementation.

# 6. Proposed Architecture

```text
Authored JSON / current persisted owner state
                │
                ▼
┌──────────────────────────────────────────────────────────────┐
│ Data-Authority Schema Version and JSON Policy Governance
│ Integration route: extend current seams; no parallel authority │
└──────────────────────────────────────────────────────────────┘
│ JSON schema policy gate
│   root/schema_version enforcement
│ CatalogIntegrityValidator
│   IDs, references, ranges, duplicate definitions
│ VersionReport
│   schema inventory and version reporting
│ Family loaders
│   actual accepted shape and migration
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

1. **Preserve current state ownership.** JSON schema policy gate owns root/schema_version enforcement: The gate is the mechanical policy authority.
2. **Use existing catalogs only for their declared content class.** New content requires a loader, validation, consumer and focused test; editing a file does not establish reachability.
3. **Keep Core engine-free.** New reusable rules belong in `Assets/Ashfall.Core/`; Godot is limited to host composition and presentation.
4. **Project rather than mirror.** Read models are derived from owner state and carry an evidence/confidence label. They are not independently persisted unless a named owner accepts a durable fact.
5. **Persist only player decisions and exactly-once facts.** Derived indexes, previews and labels are recomputed.

# 7. Ownership Matrix

| Concern | Sole owner | Current evidence | Boundary |
| --- | --- | --- | --- |
| root/schema_version enforcement | JSON schema policy gate | `scripts/ci/json-schema-policy-gate.py` | The gate is the mechanical policy authority. |
| IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | `Assets/Ashfall.Core/CatalogIntegrityValidator.cs` | Semantic integrity is deeper than schema presence. |
| schema inventory and version reporting | VersionReport | `Assets/Ashfall.Core/VersionReport.cs` | Version reporting consumes catalog schema headers. |
| actual accepted shape and migration | Family loaders | `Assets/Ashfall.Core/**/*CatalogLoader.cs` | The loader is proof for compatibility, not JSON alone. |

**Single-owner test:** for each row, search for another mutable collection, save field, catalog copy or panel cache that claims the same concern. A duplicate is a blocker, not a harmless convenience.

# 8. Data Flow

1. author JSON
2. parse root and schema_version
3. run family loader validation
4. run semantic integrity references
5. run staged gate
6. run full CI gate
7. register generated catalog documentation

Every arrow is one-way for authority. Presentation can call commands, but data must return through the owner mutation and event, not by writing a shadow field in the view.

# 9. State Model and Invariants

- Schema policy is repository metadata, not save state.
- Catalog definitions are reloaded deterministically at boot.
- A schema-version exception must name a file and reason.

**Invariant pattern:** state transitions are monotonic where appropriate, bounded where repeated, idempotent where events can repeat, and explicit about legacy defaults. Derived values are not duplicated into save state unless the owning contract intentionally caches them.

# 10. API and Contract Design

- Every root JSON object declares integer schema_version >= 1.
- Unknown fields are tolerated only when the loader contract says so.
- A loader migration preserves old field meaning and defaults missing fields explicitly.
- A generated schema sheet expires when the family schema_version changes.

The live declaration digest in Appendix B is the source for current method names. Any proposed method below is a contract shape, not a claim that it already exists:

```text
Preview(command, expectedStateVersion) -> named availability/refusal + exact deltas
Execute(command, expectedStateVersion) -> owner mutation + stable fact
QueryCurrentState(subjectId) -> read-only projection
Capture() -> deep, serializable owner state
Restore(legacyOrCurrentState) -> normalized owner state
```

# 11. Data Plan and Catalog Authority

- No bulk insertion in this package.
- New catalogs follow the current policy.
- Non-JSON files require an explicit whitelist or relocation decision.

**Data admission checklist:** schema/version present; ids resolve; ranges/enums valid; no duplicate ids; every row has a consumer; catalog kind has a loader/integrity/utilization path; migration impact is documented.

# 12. Save, Restore, and Migration

- No direct save impact.
- A catalog schema change can alter persisted meaning and therefore requires a save review.

**Save proof matrix:** current owner state → deep capture → serialize → restore to fresh instance → continue action → compare final state/checksum. A UI snapshot test does not replace this matrix.

# 13. Determinism and Replay

- Catalog ordering and generated reports are deterministic.
- Staged mode input is Git index order, not filesystem enumeration.

**Replay proof:** two runs with the same seed, catalog versions, command sequence and save fixture must produce the same final state and event order. A changed RNG stream is an architecture change even if average outcomes look similar.

# 14. System and Event Wiring

- No gameplay event.
- Gate failure is a CI/boot diagnostic fact.

**Event ordering:** owner mutation commits first, then the typed fact is emitted, then the host projects it, then any dirty save marker is set. A listener must not mutate owner state recursively unless the owner exposes an explicit command and reentrancy contract.

# 15. Godot Host Integration

- src/Main.Application.cs

The host may compose owners, resolve routes, bind providers, own nodes and present data. It must not duplicate gameplay calculations. Shared UI registration files remain integrator-owned and require exact path claims before edits.

# 16. Narrative and Content Integration

- Schema metadata is not player-facing prose.

Content validation includes references, information-flow legality, tone, quantities that reconcile to owner state, and the rule that prose never drives mechanics.

# 17. Failure Modes and Negative Contracts

| ID | Failure | Primary reviewer | Required proof |
| --- | --- | --- | --- |
| F-01 | A root array is added without a family-specific contract. | JSON schema policy gate | Focused negative test or static source gate; no broad-suite dependency. |
| F-02 | schema_version is added as a string. | CatalogIntegrityValidator | Focused negative test or static source gate; no broad-suite dependency. |
| F-03 | A loader rejects a valid legacy alias after the gate passes. | VersionReport | Focused negative test or static source gate; no broad-suite dependency. |
| F-04 | A waiver becomes permanent and unnamed. | Family loaders | Focused negative test or static source gate; no broad-suite dependency. |
| F-05 | Generated indexes are hand-edited. | JSON schema policy gate | Focused negative test or static source gate; no broad-suite dependency. |

Fail-closed means the smallest truthful result: named refusal, no mutation, visible diagnostic, and no fabricated fallback state. Optional content may remain absent only when the current loader contract explicitly says so.

# 18. Test Strategy

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Data/JsonSchemaPolicyGateTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs`
4. `bash scripts/ci/json-schema-policy-gate.sh --diff` for the documentation/data-policy tranche.

The first command is the primary focused target; later commands are selected only for the directly affected owner. **Layer split:** Core unit/edge tests; data integrity and focused loader tests; save/round-trip; determinism/replay; host wiring; headless runtime; UI/a11y only when a player surface changes. Tests stay below `TEST_POLICY.md` caps and never default to a broad suite.

# 19. Dependency-Ordered Implementation Phases

| Phase | Outcome | Completion gate | Path discipline |
| --- | --- | --- | --- |
| 0 — full census | Generate current file/schema/parse status without editing. | Census is reproducible and complete. | No production path until the owning implementation package is separately claimed. |
| 1 — family sheets | Inventory loader, consumer, root shape and migration for each family. | No loader is inferred from filename. | No production path until the owning implementation package is separately claimed. |
| 2 — waiver audit | Review every nonconforming or intentionally exceptional file. | Every exception has an owner and expiry/review condition. | No production path until the owning implementation package is separately claimed. |
| 3 — enforcement | Keep staged and full gate modes in CI. | New schema regressions fail before merge. | No production path until the owning implementation package is separately claimed. |
| 4 — migration governance | Require paired loader/test changes for key or version migrations. | No silent compatibility break. | No production path until the owning implementation package is separately claimed. |

The first safe step is always premise verification. No phase begins by creating the type named in an old generated plan; it begins by proving whether the existing owner already exposes the required seam.

# 20. File Impact Map

| Path / area | Action | Reason |
| --- | --- | --- |
| scripts/ci/json-schema-policy-gate.py | READ ONLY; MODIFY only for proven policy gap | Policy authority |
| Assets/Ashfall.Core/CatalogIntegrityValidator.cs | READ ONLY | Semantic gate |
| Assets/StreamingAssets/Data/**/*.json | NO BULK EDIT | Authoritative content |
| Ashfall.Core.Tests/Data/JsonSchemaPolicyGateTests.cs | EXTEND for discovered policy gap | Gate contract |

Any path not in this table is out of scope. A discovered need becomes a finding with an owner and evidence; it is not silently appended to this plan.

# 21. Risks and Mitigations

| Risk | Control |
| --- | --- |
| Blindly adding schema_version 1 to newer schemas. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Bulk renaming keys without loader updates. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Treating schema presence as full integrity. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |
| Editing generated indexes manually. | Keep the phase gated; stop and report if the current owner cannot support the proposed seam. |

# 22. Explicit Non-Goals

- No camelCase-to-snake_case conversion in this package.
- No invented schema numbers.
- No modification of historical saves.

# 23. Rollback and Recovery

- Revert isolated loader or policy changes.
- Data migrations require forward rollback notes and fixture-backed recovery.

For data or codec changes, recovery includes the prior valid file/save fixture, a documented migration path and a proof that newer state is not silently down-converted. For documentation/read-model changes, revert the isolated file and retain source evidence.

# 24. Definition of Done

- A current census exists.
- Every family has a schema sheet.
- Waivers are explicit.
- Staged and full gates are named.
- No blind key migration is proposed.

**DoD is behavioral:** the current owner is named, the required delta is bounded, save/determinism/host/test contracts are explicit, and every implementation claim has a future focused verification command. A high character count without these properties is not done.

# 25. Implementation Handoff Contract

## MUST PRESERVE

- Godot as the only active engine; Core remains engine-free.
- Current source/data/save owners and their generated evidence matrices.
- Existing deterministic streams, campaign-day semantics, UI accessibility and controller behavior.
- Sealed, retired, accepted and blocked decisions in the live ledgers.

## MUST ADD ONLY AFTER A NEW CLAIM

- Replace the one-time edit list with a current generated census and family exception process.
- Bind every new catalog kind to a loader, validator rule, consumer and support window.
- Treat staged/diff mode as the pre-commit path and full mode as CI authority.
- Record deliberate legacy key aliases as migration notes, not silent technical debt.

## MUST NOT DO

- No camelCase-to-snake_case conversion in this package.
- No invented schema numbers.
- No modification of historical saves.

## VERIFY WITH

1. `bash scripts/run_test.sh Ashfall.Core.Tests/Data/JsonSchemaPolicyGateTests.cs`
2. `bash scripts/run_test.sh Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`
3. `bash scripts/run_test.sh Ashfall.Core.Tests/VersionReportContractTests.cs`
4. `bash scripts/ci/json-schema-policy-gate.sh --diff` for the documentation/data-policy tranche.

## FIRST SAFE IMPLEMENTATION STEP

0 — full census — re-read the current owner APIs, reproduce the focused baseline, and write down any premise that differs from this plan. Stop with `STALE_PLAN` if the owner, save path or data contract has moved.


# Appendix A — Contract Clause Library

**Authority clause — concern-specific ownership.** Every mutable value named in this plan is governed by the owner shown for that concern in the matrix: root/schema_version enforcement → JSON schema policy gate; IDs, references, ranges, duplicate definitions → CatalogIntegrityValidator; schema inventory and version reporting → VersionReport; actual accepted shape and migration → Family loaders. A host object, panel, derived snapshot, generated document or test fixture is not an authority merely because it contains the same field name.

**Data clause.** A row in `Assets/StreamingAssets/Data/` is authored content, not reachability. The row must resolve to a loader, validator rule, runtime consumer, player or host command, and test surface. This applies to every catalog listed for Plan 03.

**Save clause.** Capture must be deep, restore must normalize only documented legacy absence, and a checksum must change when authoritative state changes. Plan 03 does not authorize a new save section when an existing owner can carry the fact.

**Determinism clause.** Randomness is optional. When present, it must use the owning campaign stream or a named stable substream, and restore must preserve the position or the next result must be derivable. Dictionary iteration, wall-clock time and GUIDs are not acceptable tie-breakers.

**Event clause.** Core raises a fact; the host applies presentation and cross-owner effects. Events are emitted after the owning mutation succeeds and carry enough stable identity for exactly-once handling and save-aware deduplication.

**UI clause.** The interface reads the current owner projection, previews a real command and renders named refusals. It must not recompute state owned by JSON schema policy gate or any other authority, hide uncertainty, or introduce a gameplay-only counter.

**Migration clause.** Additive fields default to the truthful legacy meaning. A codec/version bump is release-class work and requires fixture-backed old-save loading; unknown future versions fail closed.

**Verification clause.** Presence tests are insufficient. Each plan requirement maps to a focused behavior, boundary, persistence or determinism test, with current command syntax taken from `TEST_POLICY.md` and the live test tree.

**Accessibility clause.** State is communicated by words and semantic controls, not color alone. Focus order, close/back behavior and controller operation match the current input contract.

**Rollback clause.** Documentation and read-model changes are isolated. Runtime changes are split by owner and save contract so a failed tranche can be reverted without rewriting unrelated systems.

These clauses are normative for any later implementation package. They are not substitutes for the live APIs in Appendix B.


# Appendix B.02 — Current Code Architecture: `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/CatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 3459 lines / 190529 bytes.
- SHA-256: `d79f9fa53bb0e6eed315b2dbb58e4c2a2f32991272ddd44200da26afa1458719`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=2; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CatalogIntegrityReport
public readonly List<string> Errors = new List<string>();
public readonly List<string> Warnings = new List<string>();
public int ErrorCount => Errors.Count;
public bool Clean => Errors.Count == 0;
public int AuthoredIds;
public int ReuseCount;
public void Error(string message) {
public void Warn(string message) {
public static class CatalogIntegrityValidator
public static readonly string[] IdPrefixes = {
public static readonly string[] DefinitionKeys = {
public static readonly string[] ReferenceKeys = {
public static readonly string[] RangeKeys = { "minDay", "maxDay", "MinDay", "min_day" };
public static readonly string[] VocabularyKeys = {
public static readonly string[] KnownRuntimeIds = {
public static readonly string[] PrefixPatternKeys = {
public readonly Dictionary<string, List<string>> Registry = new Dictionary<string, List<string>>(StringComparer.Ordinal);
public readonly List<Ref> PendingRefs = new List<Ref>();
public readonly Dictionary<string, RangeMemoEntry> RangeMemo = new Dictionary<string, RangeMemoEntry>(StringComparer.Ordinal);
public CatalogIntegrityReport Report;
public string File;
public int Authored;
public int Reuse;
public string Value;
public string Path;
public bool Strict;
public string? EntityContext;
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files) => Validate(dataDirectory, files, SearchOption.TopDirectoryOnly);
public static CatalogIntegrityReport Validate(string dataDirectory, IFileIO files, SearchOption searchOption) {
public static void ValidateNightWatchOperationsCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateShelterOperationsCatalogs( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateDifficultyPresetCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateVehicleArmorGradeCatalog( string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public int? Min;
public int? Max;
public static void ValidateDistressSignalStages(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateTradeEmbargoRules(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateRegionalPriceAtlas(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateCommitments(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
public static void ValidateWildlifeTrappingCatalog(string dataDirectory, IFileIO files, CatalogIntegrityReport report) {
```


# Appendix B.03 — Current Code Architecture: `Assets/Ashfall.Core/VersionReport.cs`

### `Assets/Ashfall.Core/VersionReport.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 293 lines / 13041 bytes.
- SHA-256: `06aac4489ba2a9ae939bc2464bde920115ef6b059137fe033ec7ed4a5f598484`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum SavePersistenceKind
public readonly struct PersistenceFormatEntry
public readonly string SectionKey;
public readonly SavePersistenceKind Kind;
public readonly int? Version;
public readonly string FormatDescription;
public static class VersionReport
public readonly struct SaveSchemaEntry
public readonly string Store;
public readonly int CurrentVersion;
public static readonly SaveSchemaEntry[] SaveSchemaVersions = {
public static IReadOnlyList<PersistenceFormatEntry> AllPersistenceFormats => s_allFormats.Value;
public sealed class DataSchemaSummary
public int Catalogs;
public int WithSchemaVersion;
public int WithoutSchemaVersion;
public int MaxVersion;
public static DataSchemaSummary ScanDataSchemas(string dataDir) {
public static string FormatDataSchemas(DataSchemaSummary summary) {
public static string FormatSaveSchemas() {
public static string FormatPersistenceInventory() {
public static string Compose(string gameVersion, string dataDir) {
```


# Appendix B.04 — Current Code Architecture: `src/Main.Application.cs`

### `src/Main.Application.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1144 lines / 55746 bytes.
- SHA-256: `ea35c17ff8c236beead0d68584cde85f606db646aa0072a8ca203c74cb1893ed`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=7; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main : Control
public override void _Ready() {
public override void _Process(double delta) {
public override void _UnhandledKeyInput(InputEvent @event) {
public override void _Notification(int what) {
```


# Appendix C.05 — Catalog Census: `Assets/StreamingAssets/Data/disease_catalog.json`

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


# Appendix C.06 — Catalog Census: `Assets/StreamingAssets/Data/weather_effects.json`

### `Assets/StreamingAssets/Data/weather_effects.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 7615 bytes / 7615 characters.
- SHA-256: `2879d31c294f9f84f274e14b5b266d02d73b4f7bb1abe0a1d8604023af846600`.
- Root keys: `schema_version`, `weather_effects`.

Array-path census (minimum, maximum, observed rows):

```text
weather_effects: min=22, max=22, observed_paths=1
```

Representative record fields:

- `caravan_availability_multiplier`
- `explicitly_neutral`
- `outdoor_rad_modifier`
- `thermal_load_additive_c`
- `trap_yield_multiplier`
- `travel_encounter_multiplier`
- `travel_speed_multiplier`
- `visibility_modifier`
- `weather`


# Appendix C.07 — Catalog Census: `Assets/StreamingAssets/Data/combat_catalog.json`

### `Assets/StreamingAssets/Data/combat_catalog.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 20773 bytes / 20771 characters.
- SHA-256: `e837b53cbab1b7595a12f58c09cd7dbac753ef1736a4fbf69757a4d83f36a1c6`.
- Root keys: `ammo`, `collection_id`, `combatants`, `materials`, `schema_version`, `weapons`.

Array-path census (minimum, maximum, observed rows):

```text
ammo: min=14, max=14, observed_paths=1
combatants: min=12, max=12, observed_paths=1
materials: min=7, max=7, observed_paths=1
weapons: min=20, max=20, observed_paths=1
```

Representative record fields:

- `accuracy`
- `burst`
- `caliber`
- `condition_threshold`
- `damage`
- `degrade_per_shot`
- `display_name`
- `id`
- `is_jury_rigged`
- `is_suppression_capable`
- `jam_base`
- `range`
- `scrap_repair_cost`

Representative identifiers (ordered, capped for readability):

```text
weapon_pipe_rifle
weapon_scrap_shotgun
weapon_bolt_rifle
weapon_assault_rifle
weapon_lmg
weapon_pipe_shotgun
weapon_nail_driver
weapon_rebar_spear
weapon_molotov_thrower
weapon_service_rifle
weapon_marksman_rifle
weapon_smg
weapon_sidearm
weapon_rust_mosin
weapon_farm_carbine
weapon_revolver
weapon_coach_shotgun
weapon_trail_carbine
weapon_battle_rifle
weapon_quiet_carbine
```


# Appendix C.08 — Catalog Census: `Assets/StreamingAssets/Data/dive_sites.json`

### `Assets/StreamingAssets/Data/dive_sites.json`

- Parse: valid strict JSON.
- Schema version: `2`.
- Size: 23434 bytes / 23434 characters.
- SHA-256: `f4f273ab223859ce24fec441d2ba9612cf284d885fd4055d18cab2832832a7e9`.
- Root keys: `dive_sites`, `schema_version`.

Array-path census (minimum, maximum, observed rows):

```text
dive_sites: min=14, max=14, observed_paths=1
dive_sites[].loot_table: min=3, max=3, observed_paths=2
dive_sites[].rooms: min=4, max=4, observed_paths=2
dive_sites[].safes: min=1, max=1, observed_paths=1
dive_sites[].safes[].loot: min=2, max=2, observed_paths=1
```

Representative record fields:

- `base_noise_floor`
- `contamination_key`
- `discovery`
- `keeper_thread_id`
- `location_id`
- `loot_table`
- `name`
- `oxygen_budget_ticks`
- `required_item_count`
- `required_item_id`
- `rooms`
- `safes`
- `site_id`
- `tide_window`


# Appendix D.09 — Existing Focused Test Inventory: `Ashfall.Core.Tests/Data/JsonSchemaPolicyGateTests.cs`

### `Ashfall.Core.Tests/Data/JsonSchemaPolicyGateTests.cs`

- Current test declarations: Fact=1, Theory=1, InlineData=7.
- File lines: 112; SHA-256: `dceeaf690edf7de3fdf7afc60d2a3037465a5f07097fc220a2cbf8753bd86692`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AllAuthoritativeCatalogs_MustDeclareSchemaVersion
ValidateJsonPayload_ConformsToSchemaPolicy
```


# Appendix D.10 — Existing Focused Test Inventory: `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`

### `Ashfall.Core.Tests/CatalogIntegrityValidatorTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 228; SHA-256: `2acc7e8bb1d1d1af8ea72d76d6aa97e425e1a00f06ea31a57b1a428704b21477`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
AllCatalogIdsCrossReferenceCleanly
ValidatorReportsMalformedJsonWithFileAndExceptionContext
ValidatorFlagsAnUnknownPrefixedReference
ValidatorFlagsAnUnknownBareReference
CrossFileReuseOfAnAuthoredIdIsNotReported
SameFileEntityRootDuplicateIdIsAnError
NestedSharedTemplateIdsAreNotConflicts
RootObjectCatalogMissingSchemaVersionIsAnError
RootObjectCatalogWithSchemaVersionPassesTheRule
BareArrayRootIsExemptFromSchemaVersionRule
```


# Appendix D.11 — Existing Focused Test Inventory: `Ashfall.Core.Tests/VersionReportContractTests.cs`

### `Ashfall.Core.Tests/VersionReportContractTests.cs`

- Current test declarations: Fact=11, Theory=0, InlineData=0.
- File lines: 223; SHA-256: `10679cb9d3ce3baf342e14236cf51cae5a7b949969ff606ffa77fd85c0fbe34f`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
Compose_RendersPinnedHeaderAndSectionLines
Compose_ContainsEverySaveStoreVersion
SaveSchemaVersions_ListsEveryVersionedSaveCodec
SaveSchemaVersions_MatchTheActualCodecConstants
ScanDataSchemas_TalliesVersionsAndMissingDeclarations
ScanDataSchemas_MissingDirectory_ReturnsEmptySummary
FormatDataSchemas_RendersDistributionWithMax
ScanDataSchemas_LiveDataAuthority_IsNonTrivialAndCurrent
AllPersistenceFormats_CoversAllSaveSectionRegistrySections
AllPersistenceFormats_DistinguishesVersionedCodecsAndChecksumEnvelopes
FormatPersistenceInventory_RendersSummaryAndEntries
```


# Appendix E.12 — Supporting Code Evidence: `src/Main.Plans190_193.cs`

### `src/Main.Plans190_193.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 590 lines / 25776 bytes.
- SHA-256: `5dbed7ad680ddca7eeea7e542c17531d13f456a4e1d583e47cab527002a2963a`.
- Architecture signals: seeded references=4; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public AmputationSystem EnsureAmputation() {
public RailwaySystem EnsureRailway() {
public FungiCultivationSystem EnsureFungi() {
public JusticeSystem EnsureJustice() {
```


# Appendix E.13 — Supporting Code Evidence: `src/Main.Plans178_181.cs`

### `src/Main.Plans178_181.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 578 lines / 22923 bytes.
- SHA-256: `c4c849b6f193a9ee40417ba0646b2b6f2b26db294f8654de09400dc27cfd1480`.
- Architecture signals: seeded references=4; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public GenerationalSystem EnsureGenerational() {
public ChildProfile? GetCanonicalChildDevelopment(string childId, int currentDay = -1) {
public PrisonerSystem EnsurePrisoners() {
public MutationSystem EnsureMutations() {
public RadiationMutationHostSession EnsureMutationSession() {
public bool ApplyRadiationExposure(string survivorId, float dose, int day) {
public void TickMutations(int day, List<DayStateChangeEvent>? events = null) {
public GeneTherapyResult PerformGeneTherapy(string survivorId, string mutationId) {
public void AdministerRadAway(string survivorId, float detoxAmount) {
public SurvivorMutationProfile? GetSurvivorMutationProfile(string survivorId) {
public List<string> GetSurvivorCapabilities(string survivorId) {
public StealthSystem EnsureStealth() {
public void TickPlans178_181(int currentDay) {
```


# Appendix E.14 — Supporting Code Evidence: `src/Main.Plans186_189.cs`

### `src/Main.Plans186_189.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 333 lines / 13029 bytes.
- SHA-256: `f235d060772f83b8a42f716f892397ec6cbf92b2f4e4b893449facc3608f2cb6`.
- Architecture signals: seeded references=3; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public FalloutSystem EnsureFallout() {
public DesperationSystem EnsureDesperation() {
public MercenarySystem EnsureMercenary() {
public ArchaeologySystem EnsureArchaeology() {
public void TickPlans186_189(int day, float deltaHours) {
```


# Appendix E.15 — Supporting Code Evidence: `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

### `Assets/Ashfall.Core/Content/CollectibleCatalogIntegrityValidator.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 432 lines / 20841 bytes.
- SHA-256: `fad9423c257b5fcc2c8e38b078acf1669cc01e3cd3156e8b2764f3c79703dc43`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This evidence contributes to the current architecture map and must be interpreted through the ownership matrix below.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class CollectibleIntegrityFinding
public string SourceCatalog { get; }
public string SourceId { get; }
public string FieldPath { get; }
public string TargetId { get; }
public string TargetCatalog { get; }
public string ErrorCode { get; }
public string Message { get; }
public override string ToString() =>
public static class CollectibleCatalogIntegrityValidator
public static readonly HashSet<string> ValidCategories = new HashSet<string>(StringComparer.Ordinal) {
public static readonly HashSet<string> ValidRarities = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static readonly HashSet<string> ValidEffectTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase) {
public static List<CollectibleIntegrityFinding> Validate( string dataDir, IFileIO fileIO, IJsonSerializer json, ILog? log = null) {
public sealed class ItemFileRootDto
public int schema_version { get; set; } = 1;
public List<ItemHeaderDto> items { get; set; } = new List<ItemHeaderDto>();
public sealed class ItemHeaderDto
public string id { get; set; } = string.Empty;
public sealed class JournalVoiceProseFileRaw
public int schema_version { get; set; } = 1;
public Dictionary<string, Dictionary<string, string>> prose_variants { get; set; } =
```


# Appendix E.16 — Supporting Code Evidence: `src/Main.Plans182_185.cs`

### `src/Main.Plans182_185.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 241 lines / 8216 bytes.
- SHA-256: `3855af1f8524bb5df277ae6d2229e862fd82f5936ab01b3e2fcf2725fc5b3978`.
- Architecture signals: seeded references=1; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public AviationSystem EnsureAviation() {
public ForcedLaborSystem EnsureForcedLabor() {
public NarcoticsSystem EnsureNarcotics() {
public PoliticsSystem EnsurePolitics() {
public void TickPlans182_185(int currentDay) {
```


# Appendix G.17 — Supporting Regression Evidence: `Ashfall.Core.Tests/WildlifeTrappingCatalogIntegrityTests.cs`

### `Ashfall.Core.Tests/WildlifeTrappingCatalogIntegrityTests.cs`

- Current test declarations: Fact=21, Theory=0, InlineData=0.
- File lines: 421; SHA-256: `cfaa1a2cc60000986b4f9300f411eb4a36eedef3b908ae096a7056bc6aa24509`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ShippedWildlifeTrappingCatalog_PassesIntegrityCheck
BycatchChance_NumericRangeTable_Validation
BycatchChance_NonNumericString_Rejected
ContaminationDose_NonNegativeRangeTable_Validation
ContaminationDose_NonNumericString_Rejected
ContaminationDose_InfinityTable_Rejected
ContaminationDose_ExactAcuteThreshold_PassesWithoutWarning
ContaminationDose_ExceedsAcuteThreshold_PassesWithWarning
DiseaseId_ValidReference_ResolvesCleanly
DiseaseId_EmptyString_IsIgnoredAndPasses
DiseaseId_NonexistentReference_FailsWithErrorContainingPreyAndDiseaseId
BycatchSpecies_NonexistentSpecies_FailsWithErrorContainingTrapAndBadSpecies
BycatchSpecies_ValidSpeciesAndEmptyArray_Passes
PreyDefinition_Validate_ValidatesCorrectly
TrapDefinition_Validate_ValidatesBycatchCandidates
DiseaseId_OmittedFromPrey_PassesCleanly
DiseaseId_NullValue_PassesCleanly
ContaminationDose_NegativeInfinity_NumericString_Rejected
ContaminationDose_MultipleInvalidEntries_StableDeterministicDiagnostics
TrapDefinition_CalculateRepairBill_ImprovisedWireSnare_CeilRounding
TrapDefinition_CalculateRepairBill_BoxTrap_PreservesOrderAndAggregates
```


# Appendix G.18 — Supporting Regression Evidence: `Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs`

### `Ashfall.Core.Tests/Tooling/CatalogPathForbiddenGateTests.cs`

- Current test declarations: Fact=2, Theory=0, InlineData=0.
- File lines: 81; SHA-256: `c05f8dda3304da5bb69e711c98442d5621a4237377210709706a2debcedbfb74`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
NoNewPrivateDataResolvers
AllowlistShrinksAsSitesMigrate
```


# Appendix G.19 — Supporting Regression Evidence: `Ashfall.Core.Tests/Plan17ALoreBaselineTests.cs`

### `Ashfall.Core.Tests/Plan17ALoreBaselineTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 289; SHA-256: `fcb1dc95c8986a44599265350d75a35ad725ed3db65b4e752692e28308b67c1d`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
WorldHistory_HasSchemaVersion
FactionLore_HasSchemaVersion
ArchiveInks_HasSchemaVersion
DeepLoreLocations_HasSchemaVersion
EnvironmentalAtmosphere_HasSchemaVersion
WorldHistory_AllErasAreValid
WorldHistory_AllEntriesHaveRequiredFields
FactionLore_FactionIdsAreUnique
DeepLoreLocations_IdsAreUnique
EnvironmentalAtmosphere_AllEntriesHaveValidStructure
```


# Appendix G.20 — Supporting Regression Evidence: `Ashfall.Core.Tests/World/CatalogIntegrityWeatherGateTests.cs`

### `Ashfall.Core.Tests/World/CatalogIntegrityWeatherGateTests.cs`

- Current test declarations: Fact=10, Theory=0, InlineData=0.
- File lines: 275; SHA-256: `7fee168d9d0d60325b9943b7a33a2990dada6d849dade875f8bfb48fe4707282`.
- The list below is an inventory, not a newly executed pass result. Focused tests must be rerun when the owning implementation changes.

```text
ProductionCatalog_ReportsZeroWeatherGateErrors
InvalidWeatherId_EmitsIntegrityError
NonexistentTarget_EmitsIntegrityError
NonexistentOverrideItem_EmitsIntegrityError
RequiredAndBlockedOverlap_EmitsIntegrityError
DuplicateGateId_EmitsIntegrityError
InvalidGateType_EmitsIntegrityError
NullOrEmptyOptionalOverrideItem_ProducesNoFalsePositiveReferenceError
DeterministicErrorOrdering_AcrossRuns
GenericTargetKey_NoUnrelatedCatalogRegressions
```


# Appendix H.21 — Supporting Authority Document: `docs/releases/VERSIONING.md`

### `docs/releases/VERSIONING.md`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 71 lines / 4988 bytes.
- SHA-256: `4e5d12d4666de5a0c3be87ea32af24db0cf51b92ed158904bffec0bf642d0e6c`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Documentation is a navigation and evidence source only. Current source and generated matrices outrank it on conflict.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix H.22 — Supporting Authority Document: `docs/architecture/CLAIMS.json`

### `docs/architecture/CLAIMS.json`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 713 lines / 25824 bytes.
- SHA-256: `54d91208429166ce4bdd24d045f0c9b414aaf109f9ab63a0fee53950253162df`.
- Architecture signals: seeded references=0; save/restore symbols=0; typed event declarations=0; textual Godot mentions=3; textual Unity/JsonUtility mentions=1; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: This is authored data authority. Presence proves schema/reference validity only when the integrity and utilization gates are run; it does not prove player reachability.

No stable declaration lines were extracted; use the file hash and surrounding ownership matrix as evidence.


# Appendix I — Cross-System Precision Matrix

| Source concern | Source owner | Target concern | Target owner | Allowed contact |
| --- | --- | --- | --- | --- |
| root/schema_version enforcement | JSON schema policy gate | IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | Owner emits/reads a typed fact; no mirror state. |
| root/schema_version enforcement | JSON schema policy gate | schema inventory and version reporting | VersionReport | Owner emits/reads a typed fact; no mirror state. |
| root/schema_version enforcement | JSON schema policy gate | actual accepted shape and migration | Family loaders | Owner emits/reads a typed fact; no mirror state. |
| IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | root/schema_version enforcement | JSON schema policy gate | Owner emits/reads a typed fact; no mirror state. |
| IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | schema inventory and version reporting | VersionReport | Owner emits/reads a typed fact; no mirror state. |
| IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | actual accepted shape and migration | Family loaders | Owner emits/reads a typed fact; no mirror state. |
| schema inventory and version reporting | VersionReport | root/schema_version enforcement | JSON schema policy gate | Owner emits/reads a typed fact; no mirror state. |
| schema inventory and version reporting | VersionReport | IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | Owner emits/reads a typed fact; no mirror state. |
| schema inventory and version reporting | VersionReport | actual accepted shape and migration | Family loaders | Owner emits/reads a typed fact; no mirror state. |
| actual accepted shape and migration | Family loaders | root/schema_version enforcement | JSON schema policy gate | Owner emits/reads a typed fact; no mirror state. |
| actual accepted shape and migration | Family loaders | IDs, references, ranges, duplicate definitions | CatalogIntegrityValidator | Owner emits/reads a typed fact; no mirror state. |
| actual accepted shape and migration | Family loaders | schema inventory and version reporting | VersionReport | Owner emits/reads a typed fact; no mirror state. |

**Precision rule:** every cross-system cell has a typed fact, an explicit command, or a read-only query. A panel-to-panel copy, shared mutable object, unowned callback or duplicated save field fails this matrix.

# Appendix J — Requirement-to-Evidence Traceability

| Requirement | Required delta | Verification obligation | Failure response |
| --- | --- | --- | --- |
| R-01 | Replace the one-time edit list with a current generated census and family exception process. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-02 | Bind every new catalog kind to a loader, validator rule, consumer and support window. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-03 | Treat staged/diff mode as the pre-commit path and full mode as CI authority. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |
| R-04 | Record deliberate legacy key aliases as migration notes, not silent technical debt. | Focused negative/edge test; host/runtime proof where player-visible. | BLOCKED if no named owner can accept the fact. |

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


# Appendix P — Full Data-Authority Schema Census

```text
Assets/StreamingAssets/Data/accessibility_profiles.json	2959	1	valid
Assets/StreamingAssets/Data/achievements.json	4946	1	valid
Assets/StreamingAssets/Data/acoustic_triangulation_catalog.json	3430	1	valid
Assets/StreamingAssets/Data/aeroponics_nutrient_catalog.json	1112	1	valid
Assets/StreamingAssets/Data/affliction_bridge_rules.json	3482	1	valid
Assets/StreamingAssets/Data/agriculture_items.json	710	1	valid
Assets/StreamingAssets/Data/aircraft_parts.json	1967	1	valid
Assets/StreamingAssets/Data/alloys_and_ores.json	2405	1	valid
Assets/StreamingAssets/Data/amphibious_draisine_catalog.json	4001	1	valid
Assets/StreamingAssets/Data/anomalies.json	6387	1	valid
Assets/StreamingAssets/Data/anomalous_expedition_encounters.json	2267	1	valid
Assets/StreamingAssets/Data/antigravity_survivor_fields.json	1198	1	valid
Assets/StreamingAssets/Data/apprenticeship_catalog.json	1749	1	valid
Assets/StreamingAssets/Data/aquaponics_system_catalog.json	3093	1	valid
Assets/StreamingAssets/Data/archive_categories.json	1750	1	valid
Assets/StreamingAssets/Data/archive_inks.json	3191	1	valid
Assets/StreamingAssets/Data/armored_crawler_modules.json	4798	1	valid
Assets/StreamingAssets/Data/art_forms.json	3145	1	valid
Assets/StreamingAssets/Data/asset_registry.json	109342	1	valid
Assets/StreamingAssets/Data/atmosphere_profiles.json	3171	1	valid
Assets/StreamingAssets/Data/atmospheric_sounding_catalog.json	2242	1	valid
Assets/StreamingAssets/Data/audio_accessibility_cues.json	2948	1	valid
Assets/StreamingAssets/Data/audio_cues.json	112400	1	valid
Assets/StreamingAssets/Data/audio_logs_expansion_05.json	17440	1	valid
Assets/StreamingAssets/Data/autonomy_actions.json	9692	1	valid
Assets/StreamingAssets/Data/autopsy_procedures.json	7566	1	valid
Assets/StreamingAssets/Data/backstory_templates.json	8863	1	valid
Assets/StreamingAssets/Data/ballistic_shield_catalog.json	2193	1	valid
Assets/StreamingAssets/Data/ballistics_workbench_catalog.json	1116	1	valid
Assets/StreamingAssets/Data/barter_rules.json	1322	1	valid
Assets/StreamingAssets/Data/belief_movements.json	4270	1	valid
Assets/StreamingAssets/Data/bio_fermentation_catalog.json	3332	1	valid
Assets/StreamingAssets/Data/bionics.json	3920	1	valid
Assets/StreamingAssets/Data/black_flotilla_items.json	9255	1	valid
Assets/StreamingAssets/Data/black_market_inventory.json	4465	1	valid
Assets/StreamingAssets/Data/bounty_board.json	2141	1	valid
Assets/StreamingAssets/Data/breaching_equipment_catalog.json	7409	1	valid
Assets/StreamingAssets/Data/bunker_graffiti_postings.json	13476	1	valid
Assets/StreamingAssets/Data/camouflage_gear.json	1676	1	valid
Assets/StreamingAssets/Data/campaign_epilogues.json	5189	1	valid
Assets/StreamingAssets/Data/captive_interrogations.json	3196	1	valid
Assets/StreamingAssets/Data/caravan_trade_routes.json	6711	1	valid
Assets/StreamingAssets/Data/caravans.json	4154	1	valid
Assets/StreamingAssets/Data/carbon_composite_catalog.json	888	1	valid
Assets/StreamingAssets/Data/cargo_airdrop_catalog.json	4902	1	valid
Assets/StreamingAssets/Data/cascade_rules.json	1695	1	valid
Assets/StreamingAssets/Data/cassette_sets.json	30666	1	valid
Assets/StreamingAssets/Data/cellulosic_ethanol_catalog.json	2108	1	valid
Assets/StreamingAssets/Data/ceremonies.json	4382	1	valid
Assets/StreamingAssets/Data/characters.json	68429	1	valid
Assets/StreamingAssets/Data/chemical_dependency_items.json	2828	1	valid
Assets/StreamingAssets/Data/chemical_syntheses.json	7001	1	valid
Assets/StreamingAssets/Data/chemical_weapons.json	2727	1	valid
Assets/StreamingAssets/Data/chlor_alkali_synthesis_catalog.json	1617	1	valid
Assets/StreamingAssets/Data/chronic_conditions.json	5223	1	valid
Assets/StreamingAssets/Data/climbing_winch_catalog.json	6047	1	valid
Assets/StreamingAssets/Data/codex_entries.json	49021	1	valid
Assets/StreamingAssets/Data/cohort_tuning.json	147	1	valid
Assets/StreamingAssets/Data/collectibles.json	11272	1	valid
Assets/StreamingAssets/Data/colony_blueprints.json	2807	1	valid
Assets/StreamingAssets/Data/combat_arenas.json	1133	1	valid
Assets/StreamingAssets/Data/combat_catalog.json	20773	2	valid
Assets/StreamingAssets/Data/commitments.json	1587	1	valid
Assets/StreamingAssets/Data/commodity_baselines.json	2808	1	valid
Assets/StreamingAssets/Data/comms_targets.json	4027	1	valid
Assets/StreamingAssets/Data/communication_templates.json	2194	1	valid
Assets/StreamingAssets/Data/communications_networks.json	3592	1	valid
Assets/StreamingAssets/Data/companion_animals.json	2974	1	valid
Assets/StreamingAssets/Data/confession_secrets.json	95078	1	valid
Assets/StreamingAssets/Data/conflict_templates.json	8662	1	valid
Assets/StreamingAssets/Data/contagion_events.json	2850	1	valid
Assets/StreamingAssets/Data/crop_strains.json	10520	1	valid
Assets/StreamingAssets/Data/crossing_encounters.json	28037	1	valid
Assets/StreamingAssets/Data/crossing_factions.json	6289	1	valid
Assets/StreamingAssets/Data/crossing_items.json	11588	1	valid
Assets/StreamingAssets/Data/crossing_locations.json	8999	1	valid
Assets/StreamingAssets/Data/crossing_quests.json	34791	1	valid
Assets/StreamingAssets/Data/cryo_cultivars.json	10030	1	valid
Assets/StreamingAssets/Data/cryogenic_air_separation.json	245	1	valid
Assets/StreamingAssets/Data/cultural_archive_tomes.json	9530	1	valid
Assets/StreamingAssets/Data/cupola_foundry_catalog.json	4722	1	valid
Assets/StreamingAssets/Data/currents.json	11386	1	valid
Assets/StreamingAssets/Data/cvd_diamond_catalog.json	5152	1	valid
Assets/StreamingAssets/Data/damaged_map_zones.json	15568	1	valid
Assets/StreamingAssets/Data/death_legacy_templates.json	2260	1	valid
Assets/StreamingAssets/Data/decontamination_protocol_catalog.json	9283	1	valid
Assets/StreamingAssets/Data/deep_lore_locations.json	30686	1	valid
Assets/StreamingAssets/Data/deep_lore_survivor_fields.json	1169	1	valid
Assets/StreamingAssets/Data/defenses.json	1917	1	valid
Assets/StreamingAssets/Data/desperation_events.json	1513	1	valid
Assets/StreamingAssets/Data/development_traits.json	4291	1	valid
Assets/StreamingAssets/Data/difficulty_presets.json	2152	1	valid
Assets/StreamingAssets/Data/diplomatic_treaties.json	6846	1	valid
Assets/StreamingAssets/Data/direction_finding_catalog.json	4064	1	valid
Assets/StreamingAssets/Data/disaster_templates.json	4051	1	valid
Assets/StreamingAssets/Data/discovery_consequences.json	1851	1	valid
Assets/StreamingAssets/Data/disease_catalog.json	54811	3	valid
Assets/StreamingAssets/Data/dive_sites.json	23434	2	valid
Assets/StreamingAssets/Data/documentation_templates.json	1707	1	valid
Assets/StreamingAssets/Data/documents/vel_triage_log_names.json	2498	1	valid
Assets/StreamingAssets/Data/door_encounters.json	165468	1	valid
Assets/StreamingAssets/Data/dose_items.json	5341	1	valid
Assets/StreamingAssets/Data/dose_locations.json	5141	1	valid
Assets/StreamingAssets/Data/dose_quests.json	32196	1	valid
Assets/StreamingAssets/Data/dose_registers.json	6225	1	valid
Assets/StreamingAssets/Data/dream_templates.json	4750	1	valid
Assets/StreamingAssets/Data/duty_roles.json	3437	1	valid
Assets/StreamingAssets/Data/duty_roster_locations.json	15323	1	valid
Assets/StreamingAssets/Data/duty_roster_marks.json	8060	1	valid
Assets/StreamingAssets/Data/duty_roster_quests.json	45967	1	valid
Assets/StreamingAssets/Data/duty_roster_seasons.json	1449	1	valid
Assets/StreamingAssets/Data/dynamic_quest_templates.json	4304	1	valid
Assets/StreamingAssets/Data/dynamic_questlines.json	4153	1	valid
Assets/StreamingAssets/Data/ebpvd_coating_catalog.json	4134	1	valid
Assets/StreamingAssets/Data/echoes.json	55507	1	valid
Assets/StreamingAssets/Data/ecological_infestations.json	16361	1	valid
Assets/StreamingAssets/Data/economy_goods.json	21607	1	valid
Assets/StreamingAssets/Data/education_curriculum.json	6062	1	valid
Assets/StreamingAssets/Data/electrostatic_filtration_catalog.json	1269	1	valid
Assets/StreamingAssets/Data/emergency_alerts.json	2870	1	valid
Assets/StreamingAssets/Data/endings.json	6888	1	valid
Assets/StreamingAssets/Data/environmental_atmosphere_expansion.json	160920	1	valid
Assets/StreamingAssets/Data/environmental_texts_expansion_05.json	14494	1	valid
Assets/StreamingAssets/Data/epilogue_chronicle.json	1951	1	valid
Assets/StreamingAssets/Data/epilogue_personalization.json	5593	1	valid
Assets/StreamingAssets/Data/espionage_missions.json	3462	1	valid
Assets/StreamingAssets/Data/espionage_operations.json	2655	1	valid
Assets/StreamingAssets/Data/events.json	240926	1	valid
Assets/StreamingAssets/Data/excavation_hazard_mitigation.json	4945	1	valid
Assets/StreamingAssets/Data/excavation_sites.json	10152	1	valid
Assets/StreamingAssets/Data/exercise_routines.json	3299	1	valid
Assets/StreamingAssets/Data/expansion_item_tags.json	13659	1	valid
Assets/StreamingAssets/Data/expansion_survivor_fields.json	17080	1	valid
Assets/StreamingAssets/Data/expeditions.json	30255	1	valid
Assets/StreamingAssets/Data/faction_combat_thresholds.json	1560	1	valid
Assets/StreamingAssets/Data/faction_intelligence.json	3884	1	valid
Assets/StreamingAssets/Data/faction_lore.json	70920	1	valid
Assets/StreamingAssets/Data/faction_radio_corpus.json	68834	1	valid
Assets/StreamingAssets/Data/faction_territory.json	19950	1	valid
Assets/StreamingAssets/Data/faction_war_communiques.json	41590	1	valid
Assets/StreamingAssets/Data/faction_war_dialogue.json	21823	1	valid
Assets/StreamingAssets/Data/faction_war_events.json	93855	1	valid
Assets/StreamingAssets/Data/faction_war_journal.json	17412	1	valid
Assets/StreamingAssets/Data/faction_war_location_overrides.json	14411	1	valid
Assets/StreamingAssets/Data/faction_war_radio.json	17501	1	valid
Assets/StreamingAssets/Data/fallout_patterns.json	2048	1	valid
Assets/StreamingAssets/Data/family_name_templates.json	1044	1	valid
Assets/StreamingAssets/Data/feedback_messages.json	45537	1	valid
Assets/StreamingAssets/Data/field_guide.json	44604	1	valid
Assets/StreamingAssets/Data/final_wishes.json	62358	1	valid
Assets/StreamingAssets/Data/fischer_tropsch_catalog.json	1338	1	valid
Assets/StreamingAssets/Data/fluid_infrastructure.json	1321	1	valid
Assets/StreamingAssets/Data/fog_harvesting_catalog.json	1896	1	valid
Assets/StreamingAssets/Data/food_preservation.json	5234	1	valid
Assets/StreamingAssets/Data/food_types.json	2644	1	valid
Assets/StreamingAssets/Data/foundry_accords.json	20524	1	valid
Assets/StreamingAssets/Data/foundry_faction.json	3020	1	valid
Assets/StreamingAssets/Data/foundry_items.json	10788	1	valid
Assets/StreamingAssets/Data/foundry_production.json	32750	1	valid
Assets/StreamingAssets/Data/foundry_treaty_consequences.json	10615	1	valid
Assets/StreamingAssets/Data/geodetic_survey_catalog.json	9995	1	valid
Assets/StreamingAssets/Data/geothermal_drilling_depths.json	2112	1	valid
Assets/StreamingAssets/Data/geothermal_strata_catalog.json	1547	1	valid
Assets/StreamingAssets/Data/glassworks_recipes.json	1392	1	valid
Assets/StreamingAssets/Data/gpr_exploration_catalog.json	1142	1	valid
Assets/StreamingAssets/Data/grain_processing.json	396	1	valid
Assets/StreamingAssets/Data/greenhouse_items.json	11933	1	valid
Assets/StreamingAssets/Data/guilt_sources.json	10440	1	valid
Assets/StreamingAssets/Data/hardcore_economy_tuning.json	7588	1	valid
Assets/StreamingAssets/Data/heliograph.json	290	1	valid
Assets/StreamingAssets/Data/hidden_agendas.json	4153	1	valid
Assets/StreamingAssets/Data/hobby_definitions.json	2972	1	valid
Assets/StreamingAssets/Data/holdfast_factions.json	6557	1	valid
Assets/StreamingAssets/Data/holdfast_flavor.json	8716	1	valid
Assets/StreamingAssets/Data/holdfast_items.json	26102	1	valid
Assets/StreamingAssets/Data/holdfast_locations.json	45781	1	valid
Assets/StreamingAssets/Data/holdfast_npcs.json	14423	1	valid
Assets/StreamingAssets/Data/holdfast_quests.json	42199	1	valid
Assets/StreamingAssets/Data/hydraulic_extrusion_catalog.json	3257	1	valid
Assets/StreamingAssets/Data/hydroponic_crops.json	6997	1	valid
Assets/StreamingAssets/Data/ideological_events.json	4177	1	valid
Assets/StreamingAssets/Data/incidents.json	12879	1	valid
Assets/StreamingAssets/Data/independent_faction_branch.json	12209	1	valid
Assets/StreamingAssets/Data/infiltrator_profiles.json	4714	1	valid
Assets/StreamingAssets/Data/insar_geodesy_catalog.json	2447	1	valid
Assets/StreamingAssets/Data/interrogation_tactics.json	4495	1	valid
Assets/StreamingAssets/Data/item_degradation.json	3029	1	valid
Assets/StreamingAssets/Data/item_description_texts.json	272869	1	valid
Assets/StreamingAssets/Data/items.json	390056	1	valid
Assets/StreamingAssets/Data/journal_entries_expansion_05.json	15443	1	valid
Assets/StreamingAssets/Data/journal_voice_prose.json	44842	1	valid
Assets/StreamingAssets/Data/kinetic_flywheel_catalog.json	5984	1	valid
Assets/StreamingAssets/Data/labor_camps.json	2394	1	valid
Assets/StreamingAssets/Data/leadership_policies.json	2050	1	valid
Assets/StreamingAssets/Data/ledger_debt_templates.json	14240	1	valid
Assets/StreamingAssets/Data/legacy_traits.json	5878	1	valid
Assets/StreamingAssets/Data/library_manuals.json	21889	1	valid
Assets/StreamingAssets/Data/life_stages.json	2577	1	valid
Assets/StreamingAssets/Data/locations.json	112815	1	valid
Assets/StreamingAssets/Data/locations_expansion3.json	13139	1	valid
Assets/StreamingAssets/Data/lore_archives.json	2661	1	valid
Assets/StreamingAssets/Data/low_background_lead_catalog.json	2796	1	valid
Assets/StreamingAssets/Data/lyophilization_catalog.json	1150	1	valid
Assets/StreamingAssets/Data/map_regions.json	2290	1	valid
Assets/StreamingAssets/Data/maritime_zones.json	3736	1	valid
Assets/StreamingAssets/Data/medical_record_templates.json	2406	1	valid
Assets/StreamingAssets/Data/medical_texts.json	222244	1	valid
Assets/StreamingAssets/Data/memorial_rites.json	3787	1	valid
Assets/StreamingAssets/Data/memorials_expansion_05.json	9758	1	valid
Assets/StreamingAssets/Data/memory_decay_rates.json	1596	1	valid
Assets/StreamingAssets/Data/mental_arcs.json	1904	1	valid
Assets/StreamingAssets/Data/merchant_caravans.json	3250	1	valid
Assets/StreamingAssets/Data/meta_unlockables.json	4036	1	valid
Assets/StreamingAssets/Data/metallurgy_recipes.json	7738	1	valid
Assets/StreamingAssets/Data/metrology_standards_catalog.json	3143	1	valid
Assets/StreamingAssets/Data/micro_locations.json	35356	1	valid
Assets/StreamingAssets/Data/microfluidic_diagnostic_catalog.json	5891	1	valid
Assets/StreamingAssets/Data/military_faction_branch.json	12177	1	valid
Assets/StreamingAssets/Data/mine_flail_catalog.json	1999	1	valid
Assets/StreamingAssets/Data/mineral_acid_synthesis_catalog.json	4677	1	valid
Assets/StreamingAssets/Data/mod_manifest_schema.json	1175	1	valid
Assets/StreamingAssets/Data/moral_choice_chains.json	31788	1	valid
Assets/StreamingAssets/Data/moral_choice_faction_reactions.json	14988	1	valid
Assets/StreamingAssets/Data/moral_choice_flags.json	2090	1	valid
Assets/StreamingAssets/Data/moral_choice_gossip.json	27959	1	valid
Assets/StreamingAssets/Data/moral_choice_quests.json	141249	1	valid
Assets/StreamingAssets/Data/moral_choice_quests_branching.json	339862	1	valid
Assets/StreamingAssets/Data/moral_choice_quests_distress.json	14705	1	valid
Assets/StreamingAssets/Data/moral_choice_quests_expansion.json	118103	1	valid
Assets/StreamingAssets/Data/museum_collection_templates.json	2461	1	valid
Assets/StreamingAssets/Data/muster_camp_scenes.json	9924	1	valid
Assets/StreamingAssets/Data/muster_epilogues.json	11862	1	valid
Assets/StreamingAssets/Data/muster_faction_actions.json	50913	1	valid
Assets/StreamingAssets/Data/muster_faction_culture.json	18132	1	valid
Assets/StreamingAssets/Data/muster_witnesses.json	34574	2	valid
Assets/StreamingAssets/Data/mutations.json	6386	1	valid
Assets/StreamingAssets/Data/narcotics.json	6385	1	valid
Assets/StreamingAssets/Data/narrative/activated_carbon_adsorption_records.json	5375	1	valid
Assets/StreamingAssets/Data/narrative/ammo_hoist_jam_reports.json	6108	1	valid
Assets/StreamingAssets/Data/narrative/ammonia_chiller_leak_logs.json	5894	1	valid
Assets/StreamingAssets/Data/narrative/annealing_lehr_birefringence_records.json	5423	1	valid
Assets/StreamingAssets/Data/narrative/antler_horn_sawing_records.json	3650	1	valid
Assets/StreamingAssets/Data/narrative/apiculture_red_light_audits.json	5952	1	valid
Assets/StreamingAssets/Data/narrative/aramid_fiber_rot_reports.json	5916	1	valid
Assets/StreamingAssets/Data/narrative/architect_vault_audits.json	7219	1	valid
Assets/StreamingAssets/Data/narrative/armored_cockroach_hive_logs.json	5801	1	valid
Assets/StreamingAssets/Data/narrative/armored_locomotive_manifests.json	5216	1	valid
Assets/StreamingAssets/Data/narrative/artesian_well_contamination_logs.json	5978	1	valid
Assets/StreamingAssets/Data/narrative/awl_saddle_stitch_journals.json	3160	1	valid
Assets/StreamingAssets/Data/narrative/bark_tanning_vat_logs.json	3186	1	valid
Assets/StreamingAssets/Data/narrative/beeswax_clarification_records.json	3756	1	valid
Assets/StreamingAssets/Data/narrative/beeswax_rendering_dipping_assays.json	5011	1	valid
Assets/StreamingAssets/Data/narrative/biochar_cation_exchange_reports.json	5082	1	valid
Assets/StreamingAssets/Data/narrative/bisque_firing_records.json	3674	1	valid
Assets/StreamingAssets/Data/narrative/blast_gate_mechanical_audits.json	6208	1	valid
Assets/StreamingAssets/Data/narrative/blind_cave_molerat_studies.json	5696	1	valid
Assets/StreamingAssets/Data/narrative/boiler_feedwater_deaerator_audits.json	5002	1	valid
Assets/StreamingAssets/Data/narrative/bolting_silk_mesh_reports.json	5919	1	valid
Assets/StreamingAssets/Data/narrative/bone_degreasing_prep_logs.json	3623	1	valid
Assets/StreamingAssets/Data/narrative/borosilicate_sight_glass_thermal_shock.json	6078	1	valid
Assets/StreamingAssets/Data/narrative/brain_tanning_hide_reports.json	3303	1	valid
Assets/StreamingAssets/Data/narrative/brewers_yeast_krausen_audits.json	6029	1	valid
Assets/StreamingAssets/Data/narrative/brine_pickling_barrel_spoilage.json	5978	1	valid
Assets/StreamingAssets/Data/narrative/bullet_alloy_assay_reports.json	4867	1	valid
Assets/StreamingAssets/Data/narrative/bunker_blueprints_codex.json	22706	1	valid
Assets/StreamingAssets/Data/narrative/bunker_bureaucratic_anomalies.json	5936	1	valid
Assets/StreamingAssets/Data/narrative/bunker_children_folklore.json	12897	1	valid
Assets/StreamingAssets/Data/narrative/bunker_children_folklore_batch_2.json	10207	1	valid
Assets/StreamingAssets/Data/narrative/bunker_contraband_barter.json	18368	1	valid
Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_batch_2.json	11827	1	valid
Assets/StreamingAssets/Data/narrative/bunker_court_verdicts_codex.json	25752	1	valid
Assets/StreamingAssets/Data/narrative/bunker_graffiti_postings.json	22805	1	valid
Assets/StreamingAssets/Data/narrative/bunker_herbalism_pharmacology.json	6056	1	valid
Assets/StreamingAssets/Data/narrative/bunker_maintenance_glitches.json	24674	1	valid
Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_2.json	13370	1	valid
Assets/StreamingAssets/Data/narrative/bunker_maintenance_logs_batch_3.json	34017	1	valid
Assets/StreamingAssets/Data/narrative/bunker_rituals_and_cults.json	3810	1	valid
Assets/StreamingAssets/Data/narrative/bunker_shift_schedules_and_notices.json	12553	1	valid
Assets/StreamingAssets/Data/narrative/bunker_trade_ledger_batch_2.json	11060	1	valid
Assets/StreamingAssets/Data/narrative/bunker_wiretap_transcripts.json	5322	1	valid
Assets/StreamingAssets/Data/narrative/bunker_wiretap_transcripts_batch_2.json	12894	1	valid
Assets/StreamingAssets/Data/narrative/bureaucratic_document_runtime_map.json	5562	1	valid
Assets/StreamingAssets/Data/narrative/bureaucratic_documents_expansion.json	41421	1	valid
Assets/StreamingAssets/Data/narrative/burr_millstone_dressing_logs.json	5882	1	valid
Assets/StreamingAssets/Data/narrative/calcium_hypochlorite_titration_reports.json	5505	1	valid
Assets/StreamingAssets/Data/narrative/candle_dip_mould_assays.json	3099	1	valid
Assets/StreamingAssets/Data/narrative/canyon_mudflow_hazard_reports.json	5117	1	valid
Assets/StreamingAssets/Data/narrative/carbide_tool_wear_audits.json	5806	1	valid
Assets/StreamingAssets/Data/narrative/carrion_vulture_sighting_logs.json	5275	1	valid
Assets/StreamingAssets/Data/narrative/cave_aquatic_biota_logs.json	6231	1	valid
Assets/StreamingAssets/Data/narrative/celluloid_film_decomposition_records.json	5530	1	valid
Assets/StreamingAssets/Data/narrative/charcoal_mound_pyrolysis_logs.json	6063	1	valid
Assets/StreamingAssets/Data/narrative/chef_recipe_development.json	18859	1	valid
Assets/StreamingAssets/Data/narrative/chemist_lab_notes_batch_1.json	4411	1	valid
Assets/StreamingAssets/Data/narrative/childrens_artwork_batch_2.json	13370	1	valid
Assets/StreamingAssets/Data/narrative/childrens_folklore_expansion.json	40946	1	valid
Assets/StreamingAssets/Data/narrative/chrome_alum_tanning_assays.json	5903	1	valid
Assets/StreamingAssets/Data/narrative/clay_wedging_forming_logs.json	3658	1	valid
Assets/StreamingAssets/Data/narrative/cobalt_arming_directives.json	6698	1	valid
Assets/StreamingAssets/Data/narrative/cobalt_liturgies.json	5965	1	valid
Assets/StreamingAssets/Data/narrative/cobalt_liturgies_batch_2.json	8985	1	valid
Assets/StreamingAssets/Data/narrative/cold_process_soap_curing_reports.json	4874	1	valid
Assets/StreamingAssets/Data/narrative/conflict_mediation_records.json	30478	1	valid
Assets/StreamingAssets/Data/narrative/council_meeting_minutes.json	144863	1	valid
Assets/StreamingAssets/Data/narrative/courier_dispatches_master.json	23006	1	valid
Assets/StreamingAssets/Data/narrative/courier_mission_logs.json	13133	1	valid
Assets/StreamingAssets/Data/narrative/courier_mission_logs_batch_2.json	6185	1	valid
Assets/StreamingAssets/Data/narrative/crater_lake_limnology_records.json	5392	1	valid
Assets/StreamingAssets/Data/narrative/crop_experiment_logs.json	13102	1	valid
Assets/StreamingAssets/Data/narrative/crop_genome_degradation_reports.json	5116	1	valid
Assets/StreamingAssets/Data/narrative/crucible_clay_pot_slag_logs.json	5871	1	valid
Assets/StreamingAssets/Data/narrative/cryo_germplasm_viability_audits.json	5470	1	valid
Assets/StreamingAssets/Data/narrative/cryo_seed_ampoule_logs.json	5791	1	valid
Assets/StreamingAssets/Data/narrative/cryopod_failure_logs.json	6593	1	valid
Assets/StreamingAssets/Data/narrative/culinary_ration_batch_2.json	13308	1	valid
Assets/StreamingAssets/Data/narrative/culinary_ration_codex.json	26321	1	valid
Assets/StreamingAssets/Data/narrative/cupola_melting_ratio_audits.json	5847	1	valid
Assets/StreamingAssets/Data/narrative/cupola_slag_leaching_records.json	5710	1	valid
Assets/StreamingAssets/Data/narrative/currents_pamphlets.json	7440	1	valid
Assets/StreamingAssets/Data/narrative/currying_burnishing_assays.json	3060	1	valid
Assets/StreamingAssets/Data/narrative/dead_hand_directives.json	21973	1	valid
Assets/StreamingAssets/Data/narrative/deadbeat_escapement_wear_logs.json	5832	1	valid
Assets/StreamingAssets/Data/narrative/deckle_mould_watermark_audits.json	5467	1	valid
Assets/StreamingAssets/Data/narrative/deep_lore_texts.json	8441	1	valid
Assets/StreamingAssets/Data/narrative/diplomatic_contact_records_batch_1.json	41883	1	valid
Assets/StreamingAssets/Data/narrative/documents_batch_1.json	3488	1	valid
Assets/StreamingAssets/Data/narrative/documents_batch_2.json	7074	1	valid
Assets/StreamingAssets/Data/narrative/documents_batch_3.json	29389	1	valid
Assets/StreamingAssets/Data/narrative/drone_carrier_blackboxes.json	7368	1	valid
Assets/StreamingAssets/Data/narrative/drop_spindle_fibre_drafting_logs.json	3016	1	valid
Assets/StreamingAssets/Data/narrative/dweller_dependency_backstories.json	6918	1	valid
Assets/StreamingAssets/Data/narrative/dweller_heirlooms_master.json	27382	1	valid
Assets/StreamingAssets/Data/narrative/dweller_medical_casebook.json	38098	1	valid
Assets/StreamingAssets/Data/narrative/dweller_psychological_journals.json	6305	1	valid
Assets/StreamingAssets/Data/narrative/education_session_records.json	31990	1	valid
Assets/StreamingAssets/Data/narrative/emp_atmospheric_sniffer_logs.json	5267	1	valid
Assets/StreamingAssets/Data/narrative/engineering_logs_expansion.json	32048	1	valid
Assets/StreamingAssets/Data/narrative/engineering_mod_notes.json	24304	1	valid
Assets/StreamingAssets/Data/narrative/equipment_failure_logs.json	23142	1	valid
Assets/StreamingAssets/Data/narrative/eulogy_corpus_batch_1.json	2277	1	valid
Assets/StreamingAssets/Data/narrative/expedition_briefs_expansion.json	88733	1	valid
Assets/StreamingAssets/Data/narrative/expedition_field_reports.json	19500	1	valid
Assets/StreamingAssets/Data/narrative/expedition_field_reports_batch_2.json	4449	1	valid
Assets/StreamingAssets/Data/narrative/expedition_planning_briefs_batch_1.json	4345	1	valid
Assets/StreamingAssets/Data/narrative/expedition_route_waypoint_notes_batch_2.json	28747	1	valid
Assets/StreamingAssets/Data/narrative/faction_directives_and_notices.json	25002	1	valid
Assets/StreamingAssets/Data/narrative/faction_field_documents.json	16334	1	valid
Assets/StreamingAssets/Data/narrative/faction_texts_expansion.json	41821	1	valid
Assets/StreamingAssets/Data/narrative/fallout_sensory_loss_records.json	5114	1	valid
Assets/StreamingAssets/Data/narrative/fermentation_crock_airlock_assays.json	5055	1	valid
Assets/StreamingAssets/Data/narrative/fibre_heckling_prep_logs.json	3546	1	valid
Assets/StreamingAssets/Data/narrative/field_reports_expansion.json	55835	1	valid
Assets/StreamingAssets/Data/narrative/forge_charcoal_ash_assays.json	4804	1	valid
Assets/StreamingAssets/Data/narrative/found_objects_expansion.json	24171	1	valid
Assets/StreamingAssets/Data/narrative/fulling_trough_nap_assays.json	3164	1	valid
Assets/StreamingAssets/Data/narrative/gear_quenching_fault_logs.json	5240	1	valid
Assets/StreamingAssets/Data/narrative/geological_strata_logs.json	19259	1	valid
Assets/StreamingAssets/Data/narrative/geophone_hymnals.json	4930	1	valid
Assets/StreamingAssets/Data/narrative/geothermal_borehole_logs.json	5358	1	valid
Assets/StreamingAssets/Data/narrative/geothermal_steam_vent_diagnostics.json	5224	1	valid
Assets/StreamingAssets/Data/narrative/geothermal_steam_well_logs.json	5713	1	valid
Assets/StreamingAssets/Data/narrative/ghost_transmissions.json	10687	1	valid
Assets/StreamingAssets/Data/narrative/graffiti_expansion.json	20667	1	valid
Assets/StreamingAssets/Data/narrative/grain_silo_weevil_audits.json	5059	1	valid
Assets/StreamingAssets/Data/narrative/green_sand_bentonite_assays.json	5168	1	valid
Assets/StreamingAssets/Data/narrative/greenhouse_cultivation_logs.json	16261	1	valid
Assets/StreamingAssets/Data/narrative/ground_glass_joint_greasing_audits.json	5249	1	valid
Assets/StreamingAssets/Data/narrative/heirloom_seed_viability_reports.json	5104	1	valid
Assets/StreamingAssets/Data/narrative/hemp_fiber_hackling_logs.json	5771	1	valid
Assets/StreamingAssets/Data/narrative/hollander_beater_pulping_logs.json	5806	1	valid
Assets/StreamingAssets/Data/narrative/honey_extractor_balance_reports.json	4834	1	valid
Assets/StreamingAssets/Data/narrative/hydrophone_acoustic_logs.json	6199	1	valid
Assets/StreamingAssets/Data/narrative/improvised_repair_guides_batch_2.json	26288	1	valid
Assets/StreamingAssets/Data/narrative/inkle_loom_warp_tally_sheets.json	3257	1	valid
Assets/StreamingAssets/Data/narrative/intake_filter_clogging_logs.json	5122	1	valid
Assets/StreamingAssets/Data/narrative/invar_pendulum_thermal_expansion.json	5909	1	valid
Assets/StreamingAssets/Data/narrative/iron_gall_ink_acidity_reports.json	5642	1	valid
Assets/StreamingAssets/Data/narrative/iron_synod_canons.json	5716	1	valid
Assets/StreamingAssets/Data/narrative/journal_entries_batch_1.json	14608	1	valid
Assets/StreamingAssets/Data/narrative/journal_entries_batch_2.json	14132	1	valid
Assets/StreamingAssets/Data/narrative/journal_entries_batch_3.json	165910	1	valid
Assets/StreamingAssets/Data/narrative/journals_expansion.json	33195	1	valid
Assets/StreamingAssets/Data/narrative/jrnl_templates_cycle_c.json	4479	1	valid
Assets/StreamingAssets/Data/narrative/jrnl_templates_cycle_d.json	3264	1	valid
Assets/StreamingAssets/Data/narrative/kiln_draw_trial_assays.json	3338	1	valid
Assets/StreamingAssets/Data/narrative/langstroth_hive_foundation_logs.json	5749	1	valid
Assets/StreamingAssets/Data/narrative/lead_crystal_scintillator_aging_logs.json	5395	1	valid
Assets/StreamingAssets/Data/narrative/lead_wall_degradation_logs.json	5247	1	valid
Assets/StreamingAssets/Data/narrative/leather_harness_conditioning_audits.json	5312	1	valid
Assets/StreamingAssets/Data/narrative/letters_expansion.json	40436	1	valid
Assets/StreamingAssets/Data/narrative/liebig_condenser_fracture_logs.json	6184	1	valid
Assets/StreamingAssets/Data/narrative/lime_kiln_calcination_logs.json	5834	1	valid
Assets/StreamingAssets/Data/narrative/liquid_nitrogen_compressor_failures.json	5923	1	valid
Assets/StreamingAssets/Data/narrative/load_shed_schedule_001.json	6347	1	valid
Assets/StreamingAssets/Data/narrative/lost_tech_manuals.json	23552	1	valid
Assets/StreamingAssets/Data/narrative/mainspring_fatigue_rupture_audits.json	5046	1	valid
Assets/StreamingAssets/Data/narrative/manila_hawser_breakage_reports.json	5153	1	valid
Assets/StreamingAssets/Data/narrative/medical_documents_expansion.json	47793	1	valid
Assets/StreamingAssets/Data/narrative/memorials_expansion.json	14240	1	valid
Assets/StreamingAssets/Data/narrative/mill_dampener_tempering_assays.json	5047	1	valid
Assets/StreamingAssets/Data/narrative/mortise_tenon_failure_reports.json	5246	1	valid
Assets/StreamingAssets/Data/narrative/mudbrick_weathering_assays.json	5212	1	valid
Assets/StreamingAssets/Data/narrative/munitions_leaching_records.json	6215	1	valid
Assets/StreamingAssets/Data/narrative/mutated_botanical_logs.json	6136	1	valid
Assets/StreamingAssets/Data/narrative/needle_awl_hook_assays.json	3228	1	valid
Assets/StreamingAssets/Data/narrative/neoprene_gasket_degradation_logs.json	6059	1	valid
Assets/StreamingAssets/Data/narrative/new_arrival_intake_interviews.json	28655	1	valid
Assets/StreamingAssets/Data/narrative/night_watch_expansion.json	56976	1	valid
Assets/StreamingAssets/Data/narrative/night_watch_logbook.json	20198	1	valid
Assets/StreamingAssets/Data/narrative/numbers_station_ciphers.json	7924	1	valid
Assets/StreamingAssets/Data/narrative/oak_bark_tanning_pit_logs.json	5887	1	valid
Assets/StreamingAssets/Data/narrative/operating_theater_surgical_logs.json	5325	1	valid
Assets/StreamingAssets/Data/narrative/optical_coating_rad_browning_reports.json	5434	1	valid
Assets/StreamingAssets/Data/narrative/oral_lore_batch_2.json	9670	1	valid
Assets/StreamingAssets/Data/narrative/oral_lore_codex.json	11955	1	valid
Assets/StreamingAssets/Data/narrative/orbital_kinetic_telemetry.json	8442	1	valid
Assets/StreamingAssets/Data/narrative/ozone_contact_tower_audits.json	5768	1	valid
Assets/StreamingAssets/Data/narrative/patrol_debriefs.json	61185	1	valid
Assets/StreamingAssets/Data/narrative/pattern_maker_shrinkage_records.json	5392	1	valid
Assets/StreamingAssets/Data/narrative/periscope_prism_delamination_logs.json	5920	1	valid
Assets/StreamingAssets/Data/narrative/permafrost_methane_eruption_logs.json	5365	1	valid
Assets/StreamingAssets/Data/narrative/personal_effects_inventory_batch_2.json	9747	1	valid
Assets/StreamingAssets/Data/narrative/pipeline_sabotage_records.json	5393	1	valid
Assets/StreamingAssets/Data/narrative/plan17_discoverable_documents.json	19613	1	valid
Assets/StreamingAssets/Data/narrative/pneumatic_carrier_capsule_logs.json	5728	1	valid
Assets/StreamingAssets/Data/narrative/pneumatic_cylinder_leather_assays.json	5271	1	valid
Assets/StreamingAssets/Data/narrative/pneumatic_tube_diverter_audits.json	5933	1	valid
Assets/StreamingAssets/Data/narrative/pot_furnace_glass_melts.json	6085	1	valid
Assets/StreamingAssets/Data/narrative/power_grid_management_logs.json	8745	1	valid
Assets/StreamingAssets/Data/narrative/pozzolan_mortar_formulations.json	6012	1	valid
Assets/StreamingAssets/Data/narrative/quest_narrative_documents.json	22741	1	valid
Assets/StreamingAssets/Data/narrative/rad_pathology_autopsy_records.json	5967	1	valid
Assets/StreamingAssets/Data/narrative/radiation_survey_readings_batch_2.json	8609	1	valid
Assets/StreamingAssets/Data/narrative/radio_broadcast_rundowns.json	38352	1	valid
Assets/StreamingAssets/Data/narrative/radio_mysteries_expansion.json	2212	1	valid
Assets/StreamingAssets/Data/narrative/radio_scriptbook.json	11954	1	valid
Assets/StreamingAssets/Data/narrative/radio_scripts_expansion.json	34614	1	valid
Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_2.json	10524	1	valid
Assets/StreamingAssets/Data/narrative/radio_transcripts_batch_3.json	50544	1	valid
Assets/StreamingAssets/Data/narrative/rag_pulp_beater_records.json	5597	1	valid
Assets/StreamingAssets/Data/narrative/ragdoll_germination_assays.json	5886	1	valid
Assets/StreamingAssets/Data/narrative/ration_fraud_records.json	5378	1	valid
Assets/StreamingAssets/Data/narrative/ration_records_expansion.json	70869	1	valid
Assets/StreamingAssets/Data/narrative/rawhide_bating_failure_reports.json	5117	1	valid
Assets/StreamingAssets/Data/narrative/refractory_firebrick_spalling_logs.json	5349	1	valid
Assets/StreamingAssets/Data/narrative/regional_treaty_protocols.json	19842	1	valid
Assets/StreamingAssets/Data/narrative/relic_provenance_dossiers.json	25448	1	valid
Assets/StreamingAssets/Data/narrative/retort_wood_vinegar_audits.json	5792	1	valid
Assets/StreamingAssets/Data/narrative/root_cellar_humidity_rot_reports.json	5274	1	valid
Assets/StreamingAssets/Data/narrative/rootes_blower_vacuum_reports.json	5181	1	valid
Assets/StreamingAssets/Data/narrative/rope_break_load_assays.json	3307	1	valid
Assets/StreamingAssets/Data/narrative/rope_transmission_splicing_audits.json	5263	1	valid
Assets/StreamingAssets/Data/narrative/salt_mine_inscriptions.json	5036	1	valid
Assets/StreamingAssets/Data/narrative/scavenger_expedition_route_notes.json	5633	1	valid
Assets/StreamingAssets/Data/narrative/scraping_polishing_reports.json	3290	1	valid
Assets/StreamingAssets/Data/narrative/screw_press_felt_reports.json	4712	1	valid
Assets/StreamingAssets/Data/narrative/security_incident_reports_batch_2.json	15463	1	valid
Assets/StreamingAssets/Data/narrative/seismic_array_fault_alarms.json	5406	1	valid
Assets/StreamingAssets/Data/narrative/shelter_notices_expansion.json	55591	1	valid
Assets/StreamingAssets/Data/narrative/shelter_songs_expansion.json	45069	1	valid
Assets/StreamingAssets/Data/narrative/silage_lactic_pit_reports.json	5159	1	valid
Assets/StreamingAssets/Data/narrative/silica_gel_seed_desiccation_audits.json	5107	1	valid
Assets/StreamingAssets/Data/narrative/silo_mosquito_vector_records.json	5229	1	valid
Assets/StreamingAssets/Data/narrative/slip_glaze_formulation_notes.json	3248	1	valid
Assets/StreamingAssets/Data/narrative/slow_sand_schmutzdecke_logs.json	5830	1	valid
Assets/StreamingAssets/Data/narrative/smoked_meat_creosote_assays.json	5181	1	valid
Assets/StreamingAssets/Data/narrative/sonar_array_fault_logs.json	5308	1	valid
Assets/StreamingAssets/Data/narrative/sourdough_mother_acidity_logs.json	5956	1	valid
Assets/StreamingAssets/Data/narrative/square_set_shoring_audits.json	5950	1	valid
Assets/StreamingAssets/Data/narrative/stalactite_mineral_assay_reports.json	5260	1	valid
Assets/StreamingAssets/Data/narrative/steam_trap_water_hammer_logs.json	5279	1	valid
Assets/StreamingAssets/Data/narrative/stencil_propaganda_smear_logs.json	5429	1	valid
Assets/StreamingAssets/Data/narrative/strand_twisting_lay_reports.json	3444	1	valid
Assets/StreamingAssets/Data/narrative/substation_transformer_fires.json	5942	1	valid
Assets/StreamingAssets/Data/narrative/sump_drainage_silt_reports.json	5995	1	valid
Assets/StreamingAssets/Data/narrative/supply_audit_records.json	21408	1	valid
Assets/StreamingAssets/Data/narrative/supply_audit_records_batch_2.json	73443	1	valid
Assets/StreamingAssets/Data/narrative/surface_dragline_ruins.json	5828	1	valid
Assets/StreamingAssets/Data/narrative/surface_radiation_topo_sheets.json	5783	1	valid
Assets/StreamingAssets/Data/narrative/surgeons_casebook_batch_2.json	40589	1	valid
Assets/StreamingAssets/Data/narrative/survivor_letters_lost_kin.json	30702	1	valid
Assets/StreamingAssets/Data/narrative/survivor_profiles_expansion.json	50666	1	valid
Assets/StreamingAssets/Data/narrative/sweet_water_glycerin_assays.json	5059	1	valid
Assets/StreamingAssets/Data/narrative/tallow_rendering_vat_logs.json	3516	1	valid
Assets/StreamingAssets/Data/narrative/tallow_saponification_kettle_audits.json	5752	1	valid
Assets/StreamingAssets/Data/narrative/therapist_session_notes.json	28059	1	valid
Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_2.json	52548	1	valid
Assets/StreamingAssets/Data/narrative/therapist_session_notes_batch_3.json	4742	1	valid
Assets/StreamingAssets/Data/narrative/three_strand_rope_closing_logs.json	3249	1	valid
Assets/StreamingAssets/Data/narrative/timber_creosote_treatment_logs.json	5781	1	valid
Assets/StreamingAssets/Data/narrative/timber_dry_rot_fruiting_records.json	5294	1	valid
Assets/StreamingAssets/Data/narrative/tire_retreading_compound_logs.json	5239	1	valid
Assets/StreamingAssets/Data/narrative/trade_ledgers_expansion.json	60258	1	valid
Assets/StreamingAssets/Data/narrative/treadle_loom_heddle_reports.json	2876	1	valid
Assets/StreamingAssets/Data/narrative/tub_sizing_gelatin_assays.json	4903	1	valid
Assets/StreamingAssets/Data/narrative/turbine_blade_erosion_reports.json	5708	1	valid
Assets/StreamingAssets/Data/narrative/typographic_lead_wear_logs.json	5172	1	valid
Assets/StreamingAssets/Data/narrative/underground_fungi_flora.json	20583	1	valid
Assets/StreamingAssets/Data/narrative/undertaker_burial_records.json	37624	1	valid
Assets/StreamingAssets/Data/narrative/unsent_letters_batch_2.json	3084	1	valid
Assets/StreamingAssets/Data/narrative/vault_seal_breach_logs.json	5365	1	valid
Assets/StreamingAssets/Data/narrative/vinyl_record_archive.json	25564	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_expeditions_master.json	42468	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs.json	5357	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_grave_epitaphs_batch_2.json	12333	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_settlement_gazetteer.json	19523	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_trade_caravan_routes.json	15072	1	valid
Assets/StreamingAssets/Data/narrative/wasteland_wildlife_bestiary.json	19491	1	valid
Assets/StreamingAssets/Data/narrative/water_clock_orifice_silt_records.json	5226	1	valid
Assets/StreamingAssets/Data/narrative/water_quality_test_reports_batch_2.json	10691	1	valid
Assets/StreamingAssets/Data/narrative/weather_almanac_expansion.json	62035	1	valid
Assets/StreamingAssets/Data/narrative/wick_braiding_priming_reports.json	3195	1	valid
Assets/StreamingAssets/Data/narrative/wildlife_field_encounter_logs.json	14717	1	valid
Assets/StreamingAssets/Data/narrative/wire_confessions.json	26613	1	valid
Assets/StreamingAssets/Data/narrative/wire_rope_stranding_assays.json	5982	1	valid
Assets/StreamingAssets/Data/narrative/wood_ash_lye_hydrometer_logs.json	5892	1	valid
Assets/StreamingAssets/Data/narrative/world_history_expansion.json	30271	1	valid
Assets/StreamingAssets/Data/narrative_arc_events.json	13415	1	valid
Assets/StreamingAssets/Data/narrative_discovery_manifest.json	117812	1	valid
Assets/StreamingAssets/Data/narrative_encounters.json	23144	1	valid
Assets/StreamingAssets/Data/narrative_encounters_expansion.json	39422	1	valid
Assets/StreamingAssets/Data/narrative_encounters_npc_arcs.json	40979	1	valid
Assets/StreamingAssets/Data/narrative_progression.json	3563	1	valid
Assets/StreamingAssets/Data/narrative_questlines.json	32775	1	valid
Assets/StreamingAssets/Data/naval_vessels.json	4182	1	valid
Assets/StreamingAssets/Data/needs_performance.json	3978	1	valid
Assets/StreamingAssets/Data/night_watch_operations.json	17642	1	valid
Assets/StreamingAssets/Data/noise_sources.json	5481	1	valid
Assets/StreamingAssets/Data/npc_arcs.json	83546	1	valid
Assets/StreamingAssets/Data/npc_memory_dialogue.json	2260	1	valid
Assets/StreamingAssets/Data/nuclear_core_profiles.json	3314	1	valid
Assets/StreamingAssets/Data/nuclear_winter_phases.json	3250	1	valid
Assets/StreamingAssets/Data/nutrition_profiles.json	3194	1	valid
Assets/StreamingAssets/Data/nvis_communications_catalog.json	641	1	valid
Assets/StreamingAssets/Data/orbital_harrow_events.json	9102	1	valid
Assets/StreamingAssets/Data/outposts.json	1497	1	valid
Assets/StreamingAssets/Data/pathogens.json	2707	1	valid
Assets/StreamingAssets/Data/perimeter_defenses.json	5818	1	valid
Assets/StreamingAssets/Data/personal_belongings.json	2724	1	valid
Assets/StreamingAssets/Data/personal_quests.json	32368	1	valid
Assets/StreamingAssets/Data/phantom_heirlooms.json	19948	1	valid
Assets/StreamingAssets/Data/phantom_triggers.json	47040	1	valid
Assets/StreamingAssets/Data/pharma_recipes.json	12898	1	valid
Assets/StreamingAssets/Data/piezometer_network_catalog.json	2637	1	valid
Assets/StreamingAssets/Data/plastic_pyrolysis_catalog.json	3464	1	valid
Assets/StreamingAssets/Data/pneumatic_network_catalog.json	2419	1	valid
Assets/StreamingAssets/Data/policies.json	3178	1	valid
Assets/StreamingAssets/Data/political_policies.json	4449	1	valid
Assets/StreamingAssets/Data/powder_metallurgy_catalog.json	1291	1	valid
Assets/StreamingAssets/Data/power_grid.json	3741	1	valid
Assets/StreamingAssets/Data/power_subgrid_nodes.json	4263	1	valid
Assets/StreamingAssets/Data/precision_broaching_catalog.json	1739	1	valid
Assets/StreamingAssets/Data/precision_optics_catalog.json	3200	1	valid
Assets/StreamingAssets/Data/prewar_archives.json	4072	1	valid
Assets/StreamingAssets/Data/propaganda_campaigns.json	4998	1	valid
Assets/StreamingAssets/Data/propaganda_templates.json	1963	1	valid
Assets/StreamingAssets/Data/psychological_therapies.json	8022	1	valid
Assets/StreamingAssets/Data/psychological_trauma.json	5373	1	valid
Assets/StreamingAssets/Data/psychology_profiles.json	4922	1	valid
Assets/StreamingAssets/Data/quest_templates.json	2307	1	valid
Assets/StreamingAssets/Data/questline_master.json	66388	1	valid
Assets/StreamingAssets/Data/quests_bureaucratic_morality.json	1885	1	valid
Assets/StreamingAssets/Data/quests_expansion_05.json	96587	1	valid
Assets/StreamingAssets/Data/quests_expansion_06.json	55220	1	valid
Assets/StreamingAssets/Data/quests_faction_branching.json	194389	1	valid
Assets/StreamingAssets/Data/quests_massive_expansion_200.json	223249	1	valid
Assets/StreamingAssets/Data/quests_moral_branching_expansion.json	36716	1	valid
Assets/StreamingAssets/Data/quests_npc_arcs.json	52267	1	valid
Assets/StreamingAssets/Data/radar_ecm_catalog.json	1509	1	valid
Assets/StreamingAssets/Data/radiation_economy_social.json	4692	1	valid
Assets/StreamingAssets/Data/radio.json	44197	1	valid
Assets/StreamingAssets/Data/radio_distress_signals.json	41571	1	valid
Assets/StreamingAssets/Data/radio_distress_signals_expansion.json	47132	1	valid
Assets/StreamingAssets/Data/radio_intercepts.json	13716	1	valid
Assets/StreamingAssets/Data/radio_programs.json	1190	1	valid
Assets/StreamingAssets/Data/radio_stations.json	12840	1	valid
Assets/StreamingAssets/Data/rail_grinding_catalog.json	1543	1	valid
Assets/StreamingAssets/Data/rail_logistics_catalog.json	2620	1	valid
Assets/StreamingAssets/Data/rail_network.json	4200	1	valid
Assets/StreamingAssets/Data/railway_interlock_catalog.json	2705	1	valid
Assets/StreamingAssets/Data/rationing_protocols.json	1345	1	valid
Assets/StreamingAssets/Data/rebel_faction_branch.json	12323	1	valid
Assets/StreamingAssets/Data/recipes.json	61479	1	valid
Assets/StreamingAssets/Data/recipes_cooking.json	9228	1	valid
Assets/StreamingAssets/Data/recon_telemetry_probes.json	3835	1	valid
Assets/StreamingAssets/Data/recreation.json	3726	1	valid
Assets/StreamingAssets/Data/recruitment_templates.json	4422	1	valid
Assets/StreamingAssets/Data/regional_prices.json	3789	1	valid
Assets/StreamingAssets/Data/regional_treaties.json	3197	1	valid
Assets/StreamingAssets/Data/relationship_bands.json	2216	1	valid
Assets/StreamingAssets/Data/relationship_decay_profiles.json	2288	1	valid
Assets/StreamingAssets/Data/relic_recipes.json	28485	1	valid
Assets/StreamingAssets/Data/repeatable_quests.json	6627	1	valid
Assets/StreamingAssets/Data/reputation_dimensions.json	3192	1	valid
Assets/StreamingAssets/Data/rerailing_equipment_catalog.json	981	1	valid
Assets/StreamingAssets/Data/research_knowledge.json	21516	1	valid
Assets/StreamingAssets/Data/research_unlocks.json	8728	1	valid
Assets/StreamingAssets/Data/retention_policies.json	1318	1	valid
Assets/StreamingAssets/Data/robotics.json	4431	1	valid
Assets/StreamingAssets/Data/romance_courtship.json	6173	1	valid
Assets/StreamingAssets/Data/routine_templates.json	5421	1	valid
Assets/StreamingAssets/Data/rumor_hubs.json	1418	1	valid
Assets/StreamingAssets/Data/runflat_tire_catalog.json	2246	1	valid
Assets/StreamingAssets/Data/sanitation_facilities.json	3880	1	valid
Assets/StreamingAssets/Data/scavenging_tables.json	146802	1	valid
Assets/StreamingAssets/Data/seasonal_events.json	12281	1	valid
Assets/StreamingAssets/Data/seasonal_human_migration.json	2368	1	valid
Assets/StreamingAssets/Data/seismic_fault_catalog.json	1989	1	valid
Assets/StreamingAssets/Data/settlements.json	24974	1	valid
Assets/StreamingAssets/Data/shelter_audio_cues.json	4738	1	valid
Assets/StreamingAssets/Data/shelter_celebrations.json	3046	1	valid
Assets/StreamingAssets/Data/shelter_components.json	4680	1	valid
Assets/StreamingAssets/Data/shelter_construction.json	5004	1	valid
Assets/StreamingAssets/Data/shelter_governance_blocs.json	2059	1	valid
Assets/StreamingAssets/Data/shelter_insulation_catalog.json	2396	1	valid
Assets/StreamingAssets/Data/shelter_machine_identities.json	24630	1	valid
Assets/StreamingAssets/Data/shelter_origins.json	3822	1	valid
Assets/StreamingAssets/Data/shelter_room_identities.json	54024	1	valid
Assets/StreamingAssets/Data/shelter_rooms.json	20439	1	valid
Assets/StreamingAssets/Data/shelter_schedules.json	7638	1	valid
Assets/StreamingAssets/Data/shelter_security_zones.json	2708	1	valid
Assets/StreamingAssets/Data/shelter_shielding.json	580	1	valid
Assets/StreamingAssets/Data/shelter_social_events.json	7377	1	valid
Assets/StreamingAssets/Data/skill_certifications.json	6511	1	valid
Assets/StreamingAssets/Data/skills.json	55682	1	valid
Assets/StreamingAssets/Data/sky_defense_ordnance.json	3531	1	valid
Assets/StreamingAssets/Data/sky_layer_armor_catalog.json	5475	1	valid
Assets/StreamingAssets/Data/slice_seven_days.json	3248	1	valid
Assets/StreamingAssets/Data/sofc_power_catalog.json	5411	1	valid
Assets/StreamingAssets/Data/solar_concentrator_catalog.json	1345	1	valid
Assets/StreamingAssets/Data/sound_ranging_catalog.json	3346	1	valid
Assets/StreamingAssets/Data/spiritual_rituals.json	11128	1	valid
Assets/StreamingAssets/Data/standing_gates.json	11416	1	valid
Assets/StreamingAssets/Data/standing_record_factions.json	6222	1	valid
Assets/StreamingAssets/Data/standing_record_layouts.json	62607	1	valid
Assets/StreamingAssets/Data/standing_record_memory.json	11295	1	valid
Assets/StreamingAssets/Data/standing_record_quests.json	55098	1	valid
Assets/StreamingAssets/Data/starting_supplies.json	6597	2	valid
Assets/StreamingAssets/Data/starting_survivor_cohorts.json	7136	1	valid
Assets/StreamingAssets/Data/starting_survivors.json	933	1	valid
Assets/StreamingAssets/Data/store_capability_claims.json	4087	1	valid
Assets/StreamingAssets/Data/subterranean_zones.json	6494	1	valid
Assets/StreamingAssets/Data/sump_drainage_catalog.json	1770	1	valid
Assets/StreamingAssets/Data/supply_lines.json	2069	1	valid
Assets/StreamingAssets/Data/surgical_procedures.json	2141	1	valid
Assets/StreamingAssets/Data/survivor_life_stages.json	1765	1	valid
Assets/StreamingAssets/Data/survivor_roles.json	4267	1	valid
Assets/StreamingAssets/Data/survivor_voice_lines.json	3645	1	valid
Assets/StreamingAssets/Data/survivors.json	70319	1	valid
Assets/StreamingAssets/Data/tablet_manufacturing_catalog.json	3537	1	valid
Assets/StreamingAssets/Data/tech_salvage.json	3268	1	valid
Assets/StreamingAssets/Data/thermal_gear.json	2610	1	valid
Assets/StreamingAssets/Data/thirdonary_quests.json	143059	1	valid
Assets/StreamingAssets/Data/time_capsules.json	4294	1	valid
Assets/StreamingAssets/Data/toxic_chemical_catalog.json	9177	1	valid
Assets/StreamingAssets/Data/trade_embargoes.json	4901	1	valid
Assets/StreamingAssets/Data/trade_screen_scenarios.json	23775	1	valid
Assets/StreamingAssets/Data/trade_specialties.json	20427	1	valid
Assets/StreamingAssets/Data/trade_tell_lines.json	8811	1	valid
Assets/StreamingAssets/Data/trade_texts.json	107715	1	valid
Assets/StreamingAssets/Data/travel_encounters.json	97206	1	valid
Assets/StreamingAssets/Data/treaty_templates.json	4331	1	valid
Assets/StreamingAssets/Data/trophies.json	5651	1	valid
Assets/StreamingAssets/Data/underground_flora.json	2670	1	valid
Assets/StreamingAssets/Data/underground_tunnels.json	1571	1	valid
Assets/StreamingAssets/Data/utility_actions.json	10835	1	valid
Assets/StreamingAssets/Data/uv_corona_detector_catalog.json	751	1	valid
Assets/StreamingAssets/Data/vehicle_armor_grades.json	4568	1	valid
Assets/StreamingAssets/Data/vehicle_modifications.json	4882	1	valid
Assets/StreamingAssets/Data/vehicle_modules.json	8238	1	valid
Assets/StreamingAssets/Data/vehicles.json	3044	1	valid
Assets/StreamingAssets/Data/verdict_data.json	9814	1	valid
Assets/StreamingAssets/Data/verdict_items.json	10345	1	valid
Assets/StreamingAssets/Data/verdict_locations.json	14430	1	valid
Assets/StreamingAssets/Data/verdict_npcs.json	9832	1	valid
Assets/StreamingAssets/Data/verdict_questlines.json	85668	1	valid
Assets/StreamingAssets/Data/verdict_radio.json	11606	1	valid
Assets/StreamingAssets/Data/visitor_templates.json	1976	1	valid
Assets/StreamingAssets/Data/wall_carving_templates.json	6148	1	valid
Assets/StreamingAssets/Data/warlord_doctrines.json	40429	1	valid
Assets/StreamingAssets/Data/wasteland_grave_epitaphs.json	4974	1	valid
Assets/StreamingAssets/Data/wasteland_laws.json	2253	1	valid
Assets/StreamingAssets/Data/wasteland_map_v1.json	20796	2	valid
Assets/StreamingAssets/Data/wasteland_religions.json	1873	1	valid
Assets/StreamingAssets/Data/wasteland_settlement_npcs.json	30940	1	valid
Assets/StreamingAssets/Data/water_sources.json	4098	1	valid
Assets/StreamingAssets/Data/waystations.json	10829	1	valid
Assets/StreamingAssets/Data/weather_effects.json	7615	2	valid
Assets/StreamingAssets/Data/weather_gameplay_effects.json	4956	1	valid
Assets/StreamingAssets/Data/weather_hardening_upgrades.json	3717	1	valid
Assets/StreamingAssets/Data/weather_route_gates.json	10986	1	valid
Assets/StreamingAssets/Data/weather_seasons.json	3212	1	valid
Assets/StreamingAssets/Data/whitelists/companion_trust_flags.json	2486	1	valid
Assets/StreamingAssets/Data/whitelists/orphan_knocks.json	403	1	valid
Assets/StreamingAssets/Data/whitelists/plan25_flags.json	19661	1	valid
Assets/StreamingAssets/Data/wildlife_ecosystem.json	3850	1	valid
Assets/StreamingAssets/Data/wildlife_trapping_catalog.json	23204	1	valid
Assets/StreamingAssets/Data/workshop_recipes.json	12281	1	valid
Assets/StreamingAssets/Data/world_evolution_events.json	9076	1	valid
Assets/StreamingAssets/Data/world_evolution_seeds.json	20728	1	valid
Assets/StreamingAssets/Data/world_history.json	45166	1	valid
Assets/StreamingAssets/Data/year_of_ash_events.json	21786	1	valid
Assets/StreamingAssets/Data/year_of_ash_items.json	21660	1	valid
Assets/StreamingAssets/Data/year_of_ash_locations.json	23453	1	valid
Assets/StreamingAssets/Data/year_of_ash_questlines.json	140283	1	valid
Assets/StreamingAssets/Data/year_of_ash_quests.json	27315	1	valid
Assets/StreamingAssets/Data/year_of_ash_radio.json	27921	1	valid
Assets/StreamingAssets/Data/year_of_ash_storm_windows.json	6504	3	valid
Assets/StreamingAssets/Data/year_of_ash_survivors.json	26345	1	valid
```


# Appendix Q.555 — Additional Current Architecture Evidence: `src/Main.Plans198_201.cs`

### `src/Main.Plans198_201.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 553 lines / 22660 bytes.
- SHA-256: `f947c5b2e95e5b403b6c22279d762d93a5d1199f6a031087e6ecb7a1f86974d3`.
- Architecture signals: seeded references=4; save/restore symbols=8; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ChemWarfareSystem EnsureChemWarfare() {
public CommsArraySystem EnsureCommsArray() {
public CeremonySystem EnsureCeremonySystem() {
public RoboticsSystem EnsureRobotics() {
```


# Appendix Q.556 — Additional Current Architecture Evidence: `src/Main.Plans46_49.cs`

### `src/Main.Plans46_49.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 474 lines / 19620 bytes.
- SHA-256: `312d6602edf690061111377cd4798a738c47e6c5c344ed258c88e91167262bb8`.
- Architecture signals: seeded references=4; save/restore symbols=10; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public ShelterWorkshopSystem EnsureShelterWorkshop() {
public ShelterRadioStationSystem EnsureRadioStation() {
public ShelterSocialDynamicsSystem EnsureShelterSocialDynamics() {
public ExcavationHazardSystem EnsureExcavationHazards() {
public DynamicQuestlineSystem EnsureDynamicQuests() {
```


# Appendix Q.557 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`

### `Assets/Ashfall.Core/Shelter/ShelterIdentitySystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 478 lines / 20477 bytes.
- SHA-256: `d2d26ca5a7acf3624400762cf3654800efbe987c40f65c96fc7bd51da406fd63`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=8; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class ShelterOriginDef
public string origin_id = string.Empty;
public string display_name = string.Empty;
public string description = string.Empty;
public List<string> starting_bonuses = new List<string>();
public List<string> starting_drawbacks = new List<string>();
public int radiation_shielding_bp;
public int ventilation_bonus_bp;
public int space_modifier_bp;
public string flavor_text = string.Empty;
public sealed class ShelterOriginsCatalogJson
public int schema_version = 1;
public List<ShelterOriginDef> origins = new List<ShelterOriginDef>();
public sealed class ShelterIdentityState
public int schema_version = 1;
public string shelter_name = "The Shelter";
public string origin_id = string.Empty;
public int founding_day = 1;
public string founder_survivor_id = string.Empty;
public string motto = string.Empty;
public string emblem_color = "amber";
public string emblem_symbol = "shield";
public Dictionary<string, int> reputation_by_faction = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
public int infamy_score; // 0 to 100
public int reputation_points_trade;
public int reputation_points_raid;
public int reputation_points_medical;
public int reputation_points_isolation;
public sealed class ShelterIdentitySystem
public const string SystemId = "shelter_identity";
public const string DefaultShelterName = "The Shelter";
public event Action<string>? OnShelterNamed; // newName
public event Action<string>? OnOriginSelected; // originId
public event Action<string, int>? OnFactionReputationChanged; // factionId, newRep
public event Action<int>? OnInfamyChanged; // newInfamy
public ShelterIdentityState State => _state;
public IReadOnlyDictionary<string, ShelterOriginDef> Origins => _origins;
public string ShelterName => string.IsNullOrWhiteSpace(_state.shelter_name) ? DefaultShelterName : _state.shelter_name;
public string OriginId => _state.origin_id;
public string Motto => _state.motto;
public int Infamy => _state.infamy_score;
public void LoadCatalog(string json, IJsonSerializer serializer) {
public void RegisterOrigin(ShelterOriginDef def) {
public void BindOrigins(ShelterOriginsCatalogJson catalog) {
public ActionResult SetShelterName(string name) {
public ActionResult SetMotto(string motto) {
public ActionResult SetEmblem(string symbol, string color) {
public ActionResult SelectOrigin(string originId, int day = 1, string founderSurvivorId = "") {
public ShelterOriginDef? GetSelectedOrigin() {
public void RecordFactionReputation(string factionId, int delta) {
public int GetFactionReputation(string factionId) {
public void RecordCommunityAction(string actionType, int magnitude = 1) {
public void AdjustInfamy(int delta) {
public List<string> GetKnownForTags() {
public string FormatText(string template) {
public ShelterIdentityCensus GetCensus() {
public ShelterIdentityState CaptureState() {
public void RestoreState(ShelterIdentityState? saved) {
public struct ShelterIdentityCensus
public int OriginCount { get; }
public string OriginId { get; }
public string ShelterName { get; }
public int Infamy { get; }
public int KnownForTagCount { get; }
public int FactionReputationCount { get; }
public int CommunityActionPoints { get; }
```


# Appendix Q.558 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs`

### `Assets/Ashfall.Core/Medical/PharmaceuticalTabletEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 720 lines / 36744 bytes.
- SHA-256: `b80500efd4c5b46be3e86606767322c62bf6ab1a05d5b3466d621b4ef8bf12f0`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=5; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class TabletReleaseClassDef
public string release_class_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string shelf_life_class { get; set; } = "standard";
public float coating_quality_floor { get; set; } = 1f;
public float trade_value_bonus { get; set; } = 0f;
public int extra_process_days { get; set; } = 0;
public sealed class TabletFormulationDef
public string formulation_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string medical_effect_item_id { get; set; } = string.Empty;
public string formulation_class { get; set; } = "supplement";
public Dictionary<string, int> precursor_costs { get; set; } = new Dictionary<string, int>();
public List<string> required_precursor_tags { get; set; } = new List<string>();
public string binder_resource_id { get; set; } = string.Empty;
public int binder_amount { get; set; } = 1;
public string packaging_resource_id { get; set; } = string.Empty;
public string release_class_id { get; set; } = "immediate";
public int base_batch_size { get; set; } = 10;
public float quality_threshold { get; set; } = 0.5f;
public float trade_value_modifier { get; set; } = 1f;
public string controlled_substance_tag { get; set; } = string.Empty;
public List<string> tags { get; set; } = new List<string>();
public sealed class TabletPressDef
public string press_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public Dictionary<string, int> construction_required_items { get; set; } = new Dictionary<string, int>();
public int construction_labor_days { get; set; } = 2;
public float max_condition { get; set; } = 100f;
public int maintenance_interval_days { get; set; } = 8;
public Dictionary<string, int> maintenance_required_items { get; set; } = new Dictionary<string, int>();
public Dictionary<string, int> tooling_replacement_items { get; set; } = new Dictionary<string, int>();
public string room_id { get; set; } = string.Empty;
public float tooling_wear_per_batch { get; set; } = 2.5f;
public sealed class TabletManufacturingCatalog
public int schema_version { get; set; } = 1;
public string network_id { get; set; } = "tablet_manufacturing_primary";
public string display_name { get; set; } = "Shelter Tablet Works";
public TabletPressDef press { get; set; } = new TabletPressDef();
public List<TabletFormulationDef> formulations { get; set; } = new List<TabletFormulationDef>();
public List<TabletReleaseClassDef> release_classes { get; set; } = new List<TabletReleaseClassDef>();
public int output_buffer_max_batches { get; set; } = 3;
public List<string> tags { get; set; } = new List<string>();
public sealed class PharmaceuticalTabletPressState
public int schema_version { get; set; } = 1;
public string press_id { get; set; } = string.Empty;
public bool constructed { get; set; }
public float machine_condition { get; set; } = 100f;
public float tooling_condition { get; set; } = 100f;
public float calibration_state { get; set; } = 1f;
public string current_formulation_id { get; set; } = string.Empty;
public string machine_state { get; set; } = "idle";
public float batch_progress { get; set; } = 0f;
public float uniformity_quality { get; set; } = 0f;
public float tablet_integrity_quality { get; set; } = 0f;
public float coating_quality { get; set; } = 0f;
public float packaging_quality { get; set; } = 0f;
public float reject_fraction { get; set; } = 0f;
public string fault_state { get; set; } = string.Empty;
public int cycle_count { get; set; } = 0;
public int days_since_maintenance { get; set; } = 0;
public int next_batch_number { get; set; } = 1;
public sealed class TabletBatchState
public string batch_id { get; set; } = string.Empty;
public string formulation_id { get; set; } = string.Empty;
public string release_class_id { get; set; } = string.Empty;
public int input_batch_size { get; set; }
public int progress_days { get; set; } = 0;
public int total_process_days { get; set; } = 3;
public string machine_state_at_save { get; set; } = "staging";
public bool quality_resolved { get; set; }
public bool packaging_applied { get; set; }
public sealed class PharmaceuticalBatchResult
public string batch_id { get; set; } = string.Empty;
public string result_item_id { get; set; } = string.Empty;
public int quantity { get; set; }
public string quality_grade { get; set; } = string.Empty;
public string shelf_life_class { get; set; } = string.Empty;
public int reject_quantity { get; set; }
public float trade_value_modifier { get; set; } = 1f;
public List<string> warning_tags { get; set; } = new List<string>();
public int completed_day { get; set; }
public static class PharmaceuticalTabletFailures
public const string NotConstructed = "tab.not_constructed";
public const string AlreadyConstructed = "tab.already_constructed";
public const string MaterialsMissing = "tab.materials_missing";
public const string UnknownFormulation = "tab.unknown_formulation";
public const string MachineBusy = "tab.machine_busy";
public const string MachineFaulted = "tab.machine_faulted";
public const string MaintenanceRequired = "tab.maintenance_required";
public const string NoBatch = "tab.no_batch";
public const string NothingToClaim = "tab.nothing_to_claim";
public const string BufferFull = "tab.buffer_full";
public sealed class PharmaceuticalTabletEngine
public Func<int>? DayProvider { get; set; }
public Func<float>? PharmaceuticalChemistSkillProvider { get; set; }
public Func<float>? FormulationTechnicianSkillProvider { get; set; }
public PharmaceuticalTabletPressState State => _state;
public TabletManufacturingCatalog Catalog => _catalog;
public bool IsConstructed => _state.constructed;
public TabletBatchState? ActiveBatch => _activeBatch;
public IReadOnlyList<PharmaceuticalBatchResult> OutputBuffer => _outputBuffer;
public event Action<PharmaceuticalBatchResult, bool>? OnBatchResolved; // result, accepted
public event Action<string>? OnEventRaised;
public void BindCatalog(TabletManufacturingCatalog catalog) {
public void BindInventory( Func<string, int> getCount, Func<string, int, bool> canAdd, Action<string, int> addItem, Action<string, int> consume) {
public ActionResult ConstructPress() {
public ActionResult MaintainPress() {
public ActionResult ReplaceTooling() {
public ActionResult Recalibrate() {
public ActionResult StageBatch(string formulationId) {
public void TickDay(int day) {
public ClaimedTabletOutputs? ClaimOutputs() {
public PharmaceuticalTabletPressState CaptureState() {
public TabletWorksFullState CaptureFullState() {
public void RestoreFullState(TabletWorksFullState? state) {
public sealed class TabletWorksFullState
public int schema_version { get; set; } = 1;
public PharmaceuticalTabletPressState? press { get; set; }
public TabletBatchState? active_batch { get; set; }
public List<PharmaceuticalBatchResult> output_buffer { get; set; } = new List<PharmaceuticalBatchResult>();
public sealed class ClaimedTabletOutputs
public int batches_claimed { get; set; }
public int total_units { get; set; }
public Dictionary<string, int> item_units { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
```


# Appendix Q.559 — Additional Current Architecture Evidence: `src/Main.Plans202_205.cs`

### `src/Main.Plans202_205.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 304 lines / 14069 bytes.
- SHA-256: `bb5c916bd76d1650dc5fc32be94bfc59e730b4e425049f9d2c22aa0eb337e63f`.
- Architecture signals: seeded references=2; save/restore symbols=4; typed event declarations=0; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Host evidence shows composition and presentation. It may route facts to owners but cannot become the gameplay authority.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public partial class Main
public PlasticPyrolysisHostSession EnsurePlasticPyrolysis() {
public CargoAirdropHostSession EnsureCargoAirdrop() {
```


# Appendix Q.560 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`

### `Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 1161 lines / 49527 bytes.
- SHA-256: `6223ad74f06bae54742b4bce7ac1d52573173884dd8fef9d3de261cca872c54d`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=14; textual Godot mentions=1; textual Unity/JsonUtility mentions=2; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public class DutyRosterRow
public string survivorId;
public string displayName;
public string occupationObserved;
public string status;
public string script;
public int lastSleptDay = -1;
public DutyRosterRow Clone() {
public class DutyRosterAssignmentEntry
public string role;
public string survivorId;
public bool fitnessWarningAcknowledged;
public int fitnessWarningDay = -1;
public List<string> fitnessWarningReasons = new List<string>();
public class DutyRosterWatchShift
public string shift_id = string.Empty;
public string post_id = string.Empty;
public string survivorId = string.Empty;
public int day = -1;
public int start_hour = 0;
public int duration_hours = 1;
public int fatigue_before_permille = 0;
public int fatigue_after_permille = 0;
public string fatigue_tier = "Alert";
public bool completed = false;
public class DutyRosterPneumaticMemo
public string memoId;
public string targetRoomId;
public int deliveredDay;
public int expiresDay;
public float shiftEfficiencyBonus;
public class DutyRosterOccupant
public string survivorId;
public string displayName;
public string occupationObserved;
public bool sleptHere;
public class DutyRosterSystemState
public string systemId = DutyRosterIds.SystemId;
public bool expansionUnlocked;
public bool wallInspected;
public string chartScript = DutyRosterIds.ScriptBlank;
public bool kessPencilAllowed;
public bool waitInk;
public bool blankRowsAccess = true;
public bool mutationRosterInUse;
public bool mutationRosterStillBlank;
public bool mutationRosterBurned;
public bool mutationRationProtocol;
public string endingId;
public bool secondWinterActive;
public int seedSalt = DutyRosterIds.SeedUtilityOffset;
public int lastMorningDay = -1;
public int daysLeftBlank;
public int lastBurnDay = -1;
public bool overflowAccess;
public List<string> overflowVisited = new List<string>();
public List<DutyRosterRow> rows = new List<DutyRosterRow>();
public List<DutyRosterAssignmentEntry> assignments = new List<DutyRosterAssignmentEntry>();
public List<DutyRosterWatchShift> watch_shifts = new List<DutyRosterWatchShift>();
public int watch_schema_version = 1;
public List<DutyRosterPneumaticMemo> pneumaticMemos = new List<DutyRosterPneumaticMemo>();
public List<string> hiddenFromNorth = new List<string>();
public List<string> blankRowsLivingNames = new List<string>();
public class DutyRosterSystem
public const string SystemId = DutyRosterIds.SystemId;
public const string ExpansionId = DutyRosterIds.ExpansionId;
public const string FlagExpUnlocked = DutyRosterIds.FlagExpUnlocked;
public const string LocStackRosterWall = DutyRosterIds.LocStackRosterWall;
public const string LocStackSleeping = DutyRosterIds.LocStackSleeping;
public const string LocStackMess = DutyRosterIds.LocStackMess;
public const string LocStackFiltration = DutyRosterIds.LocStackFiltration;
public const string LocStackAirlock = DutyRosterIds.LocStackAirlock;
public const string LocStackClinicAlcove = DutyRosterIds.LocStackClinicAlcove;
public const string QuestTheChart = DutyRosterIds.QuestTheChart;
public const string QuestWhoEats = DutyRosterIds.QuestWhoEats;
public const string QuestFourteenth = DutyRosterIds.QuestFourteenth;
public const string QuestCaretaker = DutyRosterIds.QuestCaretaker;
public const string QuestTheColumn = DutyRosterIds.QuestTheColumn;
public const string QuestTheTin = DutyRosterIds.QuestTheTin;
public const string QuestQuiet = DutyRosterIds.QuestQuiet;
public const string QuestSole = DutyRosterIds.QuestSole;
public const string QuestWindow = DutyRosterIds.QuestWindow;
public const string QuestInk = DutyRosterIds.QuestInk;
public const string NpcKessAdler = DutyRosterIds.NpcKessAdler;
public const string NpcAnselDuth = DutyRosterIds.NpcAnselDuth;
public const string NpcHadiMorrow = DutyRosterIds.NpcHadiMorrow;
public const string NpcTamsinRook = DutyRosterIds.NpcTamsinRook;
public const string NpcLenQuill = DutyRosterIds.NpcLenQuill;
public const string NpcNilaBrant = DutyRosterIds.NpcNilaBrant;
public const string ChoiceWritePencil = DutyRosterIds.ChoiceWritePencil;
public const string ChoiceLeaveBlank = DutyRosterIds.ChoiceLeaveBlank;
public const string ChoiceWaitInk = DutyRosterIds.ChoiceWaitInk;
public const string ChoiceLadleChild = DutyRosterIds.ChoiceLadleChild;
public const string ChoiceLadleHatch = DutyRosterIds.ChoiceLadleHatch;
public const string ChoiceLadleLeave = DutyRosterIds.ChoiceLadleLeave;
public const string ChoiceLadleProtocol = DutyRosterIds.ChoiceLadleProtocol;
public const string ScriptBlank = DutyRosterIds.ScriptBlank;
public const string ScriptPencil = DutyRosterIds.ScriptPencil;
public const string ScriptInk = DutyRosterIds.ScriptInk;
public const string ScriptBurned = DutyRosterIds.ScriptBurned;
public const string StatusHome = DutyRosterIds.StatusHome;
public const string StatusLevy = DutyRosterIds.StatusLevy;
public const string StatusWaystation = DutyRosterIds.StatusWaystation;
public const string StatusQuiet = DutyRosterIds.StatusQuiet;
public const string StatusMissing = DutyRosterIds.StatusMissing;
public const string StatusDead = DutyRosterIds.StatusDead;
public const string RoleNightWatch = DutyRosterIds.RoleNightWatch;
public const string RoleMess = DutyRosterIds.RoleMess;
public const string RoleHatchOpener = DutyRosterIds.RoleHatchOpener;
public const string RoleIntakeSleeper = DutyRosterIds.RoleIntakeSleeper;
public const string RoleExpedition = DutyRosterIds.RoleExpedition;
public const string MutationRosterInUse = DutyRosterIds.MutationRosterInUse;
public const string MutationRosterStillBlank = DutyRosterIds.MutationRosterStillBlank;
public const string MutationRationProtocol = DutyRosterIds.MutationRationProtocol;
public const string MutationRosterBurned = DutyRosterIds.MutationRosterBurned;
public const string MutationRosterInk = DutyRosterIds.MutationRosterInk;
public const string MutationRosterBlank = DutyRosterIds.MutationRosterBlank;
public const string MutationFactionBlankRowsAccess = DutyRosterIds.MutationFactionBlankRowsAccess;
public const string FlagWaitInk = DutyRosterIds.FlagWaitInk;
public const string EndingInk = DutyRosterIds.EndingInk;
public const string EndingPencil = DutyRosterIds.EndingPencil;
public const string EndingBlank = DutyRosterIds.EndingBlank;
public const string EndingBurned = DutyRosterIds.EndingBurned;
public const string EndingSecondWinter = DutyRosterIds.EndingSecondWinter;
public const string SeasonSecondWinter = DutyRosterIds.SeasonSecondWinter;
public const int SecondWinterWindowMinDays = DutyRosterIds.SecondWinterWindowMinDays;
public const int SecondWinterWindowMaxDays = DutyRosterIds.SecondWinterWindowMaxDays;
public const float SecondWinterEncounterWeight = DutyRosterIds.SecondWinterEncounterWeight;
public const int ManifestCap = DutyRosterIds.ManifestCap;
public const int SoftGateDay = DutyRosterIds.SoftGateDay;
public const int StillBlankDays = DutyRosterIds.StillBlankDays;
public const int SeedUtilityOffset = DutyRosterIds.SeedUtilityOffset;
public static readonly string[] StackWingIds = DutyRosterIds.StackWingIds;
public const string LocOverflowAlloc11 = DutyRosterIds.LocOverflowAlloc11;
public const string LocOverflowAlloc13 = DutyRosterIds.LocOverflowAlloc13;
public const string LocOverflowPumpHatch = DutyRosterIds.LocOverflowPumpHatch;
public const string LocOverflowBlankCellar = DutyRosterIds.LocOverflowBlankCellar;
public static readonly string[] OverflowNodeIds = DutyRosterIds.OverflowNodeIds;
public static readonly string[] AssignmentRoles = DutyRosterIds.AssignmentRoles;
public event Action OnRosterUpdated;
public event Action<string> OnNameWritten;
public event Action<string> OnNameErased;
public event Action OnRosterBurned;
public event Action<string, string> OnAssignmentChanged;
public event Action<string, string> OnDutyVacated;
public event Action<DutyRosterSystemState> OnStateChanged;
public DutyRosterSystemState State => _state;
public bool IsUnlocked => _state.expansionUnlocked;
public string ChartScript => _state.chartScript;
public bool BlankRowsAccess => _state.blankRowsAccess;
public bool MutationInUse => _state.mutationRosterInUse;
public int OccupiedRowCount => _state.rows != null ? _state.rows.Count : 0;
public IReadOnlyList<DutyRosterRow> Rows => _state.rows;
public RoleFitnessVerdict? PreviewRoleFitness(string survivorId, string role) {
public CrewConsentVerdict? PreviewCrewConsent(string survivorId, string role) {
public void Initialise(int seedSalt) {
public void Unlock(int day) {
public void NotifyWallInspected() {
public bool CanBeginChart(int day, bool loreAllocationWrongness, bool holdfastClerkStarted) {
public DutyRosterRow GetRow(string survivorId) {
public bool WriteName( string survivorId, string displayName, string occupationObserved, string script, int day,
public bool EraseName(string survivorId) {
public bool BurnChart(int day) {
public void TickMorning(int day, IReadOnlyList<DutyRosterOccupant> occupants) {
public bool ResolveChartChoice(string choiceId, int day) {
public bool ResolveLadleChoice(string choiceId, int day) {
public bool ResolveInkEnding(int day) {
public bool SetStatus(string survivorId, string status) {
public bool SetRowScript(string survivorId, string script) {
public void SetSecondWinterActive(bool active) {
public bool IsSecondWinterActive => _state.secondWinterActive;
public bool Assign(string role, string survivorId) {
public ActionResult AssignWatchShift( string shiftId, string postId, string survivorId, int day, int startHour,
public ActionResult CompleteWatchShift(string shiftId, int fatigueAfterPermille, int? restQualityPermille = null) {
public IReadOnlyList<DutyRosterWatchShift> GetWatchShifts(int? day = null) {
public int GetWatchShiftCount(string postId, int day, bool completedOnly = false) {
public int GetAverageWatchFatigue(string postId, int day) {
public DutyRosterWatchShift? FindWatchShift(string shiftId) {
public Func<string, DutyHourSnapshot>? DutyHourResolver { get; set; }
public DutyHourSnapshot? PreviewDutyHours(string survivorId) => DutyHourResolver?.Invoke(survivorId);
```


# Appendix Q.561 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`

### `Assets/Ashfall.Core/Ecology/CompanionAnimalSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 730 lines / 37464 bytes.
- SHA-256: `7a4ce72a45ca3b105dab66e96045b873452f24a027e850a97d4408d2d8c5cc39`.
- Architecture signals: seeded references=0; save/restore symbols=2; typed event declarations=15; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public enum CompanionRole
public enum CompanionTrainingLevel
public enum CompanionSicknessState
public sealed class CompanionSpeciesProfile
public string species_id { get; set; } = string.Empty;     // must exist in wildlife_ecosystem.json
public string display_name { get; set; } = string.Empty;
public List<string> role_tags { get; set; } = new List<string>();    // guard | pack | morale
public int base_food_per_day { get; set; }                 // units of canonical food/day
public List<string> preferred_food_tags { get; set; } = new List<string>();
public List<string> fallback_food_item_ids { get; set; } = new List<string>();
public int max_health { get; set; } = 50;
public int trainability { get; set; } = 5;                 // 1..10
public int bond_rate { get; set; } = 5;                    // 1..10
public int guard_rating { get; set; } = 0;                 // 0..100 warning value
public int pack_capacity_kg { get; set; } = 0;             // 0..60
public int morale_support_bp { get; set; } = 0;            // 0..500 bounded
public int disease_resistance { get; set; } = 0;           // 0..10
public List<string> terrain_tags { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class CompanionCatalogRoot
public int schema_version { get; set; } = 1;
public List<CompanionSpeciesProfile> companions { get; set; } = new List<CompanionSpeciesProfile>();
public sealed class CompanionCatalogLoadResult
public List<CompanionSpeciesProfile> Companions { get; } = new List<CompanionSpeciesProfile>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public sealed class CompanionState
public string companion_id { get; set; } = string.Empty;   // = wildlife DomesticAnimalState.animal_id
public string species_id { get; set; } = string.Empty;
public string name { get; set; } = string.Empty;           // presentation/state data, NOT identity
public string assigned_survivor_id { get; set; } = string.Empty;
public int role { get; set; } = (int)CompanionRole.Unassigned;
public int training_progress { get; set; }                 // 0..TrainingProgressPerLevel-1
public int training_level { get; set; }                    // (int) CompanionTrainingLevel
public int bond { get; set; }                              // 0..MaxBond
public int health { get; set; } = 50;
public int hunger { get; set; }                            // 0..100, HIGHER = WORSE (NeedsSystem parity)
public int sickness { get; set; } = 0;                     // (int) CompanionSicknessState
public int last_fed_day { get; set; } = -1;
public int tamed_day { get; set; }
public bool on_expedition { get; set; }
public bool alive { get; set; } = true;
public sealed class CompanionSystemState
public string system_id { get; set; } = "companion_animals";
public int schema_version { get; set; } = 1;
public int last_tick_day { get; set; }
public List<CompanionState> companions { get; set; } = new List<CompanionState>();
public sealed class CompanionAssignResult
public bool Success;
public string ReasonCode = string.Empty;                   // unknown_companion | companion_dead |
public static CompanionAssignResult Fail(string reason) => new CompanionAssignResult { Success = false, ReasonCode = reason };
public sealed class CompanionFeedResult
public bool Fed;
public string FoodItemId = string.Empty;                   // canonical item consumed ("" = none available)
public bool UsedEmergencyFallback;                          // fallback item: reduced benefit
public string ReasonCode = string.Empty;                   // no_food_available | already_fed | companion_dead
public sealed class CompanionAnimalSystem
public const string SystemId = "companion_animals";
public const int MaxBond = 100;
public const int TrainingProgressPerLevel = 10;
public const int HungerDailyGain = 25;             // unfed day
public const int HungerCritical = 80;              // health risk begins (§5.8: never one missed meal → death)
public const int HealthLossPerHungryDay = 6;
public const int HealthLossPerSickDay = 4;
public const int BondDailyCareGain = 2;            // fed + tended day (§5.6)
public const int BondHungerLossPerDay = 3;
public const int BondSickLossPerDay = 2;
public const int BondOnDeathOfHandler = 10;
public const int GriefMoraleShockMaxBp = -1500;    // bounded grief ceiling (§5.15 — morale authority applies)
public const int GriefMoraleFloorBp = -300;        // low-bond companions still register, smaller shock
public const float GuardBenefitHealthFloor = 0.4f; // injured/hungry animals fade (§5.10)
public const float PackBenefitHealthFloor = 0.4f;
public Func<double>? SicknessRoll;
public Func<string, bool>? KnownSpeciesCheck;
public event Action<CompanionState>? OnCompanionRegistered;
public event Action<CompanionState, CompanionRole>? OnRoleChanged;
public event Action<CompanionState, int>? OnHungerChanged;
public event Action<CompanionState, CompanionSicknessState>? OnSicknessChanged;
public event Action<CompanionState>? OnCompanionRecovered;
public event Action<CompanionState>? OnCompanionDied;
public CompanionSystemState State => _state;
public IReadOnlyCollection<CompanionSpeciesProfile> Profiles => _profiles.Values;
public void BindFoodPort(Func<string, int> count, Action<string, int> consume) {
public CompanionSpeciesProfile? Profile(string speciesId) =>
public CompanionAssignResult RegisterCompanion(string companionId, string speciesId, int tamedDay, string? name = null) {
public CompanionState? Companion(string companionId) =>
public float GetGuardModifierTotal() {
public float GetPackCapacityBonusForSurvivor(string survivorId) {
public void SetOnExpedition(string companionId, bool onExpedition) {
public CompanionAssignResult TreatSickness(string companionId, string itemId) {
public CompanionAssignResult Assign(string companionId, string survivorId, CompanionRole role, Func<string, bool>? survivorAlive = null) {
public void TickDay(int day) {
public CompanionFeedResult Feed(CompanionState c, CompanionSpeciesProfile profile, int day, IReadOnlyDictionary<string, string>? preferredTagItemMap = null) {
public float GetGuardModifier(string companionId) {
public float GetPackCapacityBonus(string companionId) {
public int GetMoraleSupportBp(string companionId) {
public int GetGriefMoraleDeltaBp(string companionId) {
public CompanionSystemState CaptureState() {
public void RestoreState(CompanionSystemState? state) {
public static class CompanionAnimalCatalogLoader
public const string FileName = "companion_animals.json";
public const int CurrentSchemaVersion = 1;
public static readonly IReadOnlyList<string> AcceptedRoleTags = new[] { "guard", "pack", "morale" };
public const int MaxFoodPerDay = 10;
public const int MaxHealth = 200;
public const int MaxTrainability = 10;
public const int MaxBondRate = 10;
public const int MaxGuardRating = 100;
public const int MaxPackCapacityKg = 60;
public const int MaxMoraleSupportBp = 500;
public const int MaxDiseaseResistance = 10;
public static CompanionCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<CompanionSpeciesProfile> ToProfiles(CompanionCatalogLoadResult result) {
```


# Appendix Q.562 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Medical/BionicsSystem.cs`

### `Assets/Ashfall.Core/Medical/BionicsSystem.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 820 lines / 42240 bytes.
- SHA-256: `b0fa14dddc57cb6e47d2235b62e77738652257764c717eaf9745e9f43150c9a1`.
- Architecture signals: seeded references=3; save/restore symbols=2; typed event declarations=19; textual Godot mentions=0; textual Unity/JsonUtility mentions=0; `System.Random`=0; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public static class BionicsCaps
public const float CapabilityBonusCapBp = 200f;
public const float ConditionFunctionFloor = 25f;
public const float DestroyedCondition = 0f;
public const float RehabStartFactor = 0.5f;
public const int OverdueMalfunctionGraceDays = 2;
public enum ImplantMalfunction
public enum ImplantIntegrationStatus
public enum ImplantComplication
public enum ImplantElectricalVulnerability
public sealed class ImplantDefinition
public string implant_id { get; set; } = string.Empty;     // implant_*
public string display_name { get; set; } = string.Empty;
public string body_slot { get; set; } = "arm";             // arm | leg (v1 limb slots)
public string implant_class { get; set; } = "mechanical";  // mechanical|powered|rechargeable|neuro_linked
public int functional_restore_bp { get; set; }             // 500..1200 capability restoration
public int skill_modifier_bp { get; set; }                 // 0..500
public string power_profile { get; set; } = "passive";     // passive|rechargeable|high_draw
public int daily_power_draw_watts { get; set; }            // 0..60
public int battery_days { get; set; }                      // 0..14 expedition endurance
public int maintenance_interval_days { get; set; }         // 3..30
public int daily_condition_decay_bp { get; set; }          // 0..200 bp/day
public int condition_max { get; set; } = 100;
public int integration_risk_bp { get; set; }               // 0..3000 complication chance
public int integration_recovery_days { get; set; }         // 3..30 rehab
public int malfunction_risk_bp { get; set; }               // 0..1000 overdue-maintenance roll
public string electrical_vulnerability { get; set; } = "none"; // none|low|high
public string required_surgery_tool_id { get; set; } = "surgical_saw";
public List<string> required_item_ids { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class BionicsCatalogRoot
public int schema_version { get; set; } = 1;
public List<ImplantDefinition> implants { get; set; } = new List<ImplantDefinition>();
public sealed class BionicsCatalogLoadResult
public List<ImplantDefinition> Implants { get; } = new List<ImplantDefinition>();
public List<string> Errors { get; } = new List<string>();
public bool HasErrors => Errors.Count > 0;
public sealed class ImplantInstanceState
public string instance_id { get; set; } = string.Empty;    // implant_inst_<n>
public string implant_id { get; set; } = string.Empty;     // authored definition
public string survivor_id { get; set; } = string.Empty;
public int body_slot_limb { get; set; }                    // (int) LimbId
public float condition { get; set; } = 100f;
public int installed_day { get; set; }
public int integration_status { get; set; } = (int)ImplantIntegrationStatus.Integrating;
public int integration_days_left { get; set; }
public int integration_days_total { get; set; }
public float battery_days_remaining { get; set; }
public bool is_charging { get; set; }
public int last_maintenance_day { get; set; }
public int complication { get; set; } = 0;                 // (int) ImplantComplication
public int complication_days_left { get; set; }            // -1 = chronic (persistent until treated)
public int malfunction { get; set; } = 0;                  // (int) ImplantMalfunction
public int malfunction_days_left { get; set; }
public bool destroyed { get; set; }
public sealed class BionicsSystemState
public string system_id { get; set; } = "bionics";
public int schema_version { get; set; } = 1;
public int instance_counter { get; set; }
public int last_tick_day { get; set; }
public List<ImplantInstanceState> implants { get; set; } = new List<ImplantInstanceState>();
public sealed class ImplantInstallResult
public bool Success;
public string ReasonCode = string.Empty;                   // unknown_implant | body_slot_mismatch |
public ImplantInstanceState? Instance;
public ImplantComplication Complication = ImplantComplication.None;
public static ImplantInstallResult Fail(string reason) => new ImplantInstallResult { Success = false, ReasonCode = reason };
public sealed class ImplantDisruptionOutcome
public string InstanceId = string.Empty;
public string Effect = string.Empty;    // stunned | actuator_lock | sensor_blackout | battery_drain | condition_damage | destroyed
public int Days;
public float BatteryDaysDrained;
public sealed class BionicsSystem
public const string SystemId = "bionics";
public Func<double>? MalfunctionRoll;
public Func<bool>? ChargerAvailable;
public event Action<ImplantInstanceState, ImplantDefinition>? OnImplantInstalled;
public event Action<ImplantInstanceState>? OnImplantConditionChanged;
public event Action<ImplantInstanceState, ImplantMalfunction>? OnImplantMalfunctioned;
public event Action<ImplantInstanceState, ImplantComplication>? OnImplantComplication;
public event Action<ImplantInstanceState>? OnImplantRemoved;
public event Action<ImplantInstanceState>? OnImplantDestroyed;
public event Action<ImplantInstanceState>? OnIntegrationCompleted;
public BionicsSystemState State => _state;
public void BindInventory(Func<string, int> count, Action<string, int> consume) {
public ImplantDefinition? Definition(string implantId) =>
public IReadOnlyList<ImplantInstanceState> ImplantsFor(string survivorId) =>
public ImplantInstanceState? InstanceFor(string survivorId, LimbId limb) =>
public ImplantInstallResult TryInstall(string survivorId, LimbId limb, string implantId, int day, ISeededRng? rng) {
public bool RemoveImplant(string survivorId, LimbId limb) {
public void TickDay(int day) {
public List<ImplantDisruptionOutcome> ApplyElectricalDisruption(string survivorId, float severity) {
public bool PerformMaintenance(string survivorId, LimbId limb, string kitItemId, int day) {
public bool Repair(string survivorId, LimbId limb, string kitItemId) {
public float GetLimbCapabilityBonusBp(string survivorId, LimbId limb) {
public ImplantDisruptionOutcome ApplyCombatDamage(string survivorId, LimbId limb, float severity) {
public BionicsSystemState CaptureState() {
public void RestoreState(BionicsSystemState? state) {
public static class BionicsCatalogLoader
public const string FileName = "bionics.json";
public const int CurrentSchemaVersion = 1;
public static readonly IReadOnlyList<string> AcceptedBodySlots = new[] { "arm", "leg" };
public static readonly IReadOnlyList<string> AcceptedClasses = new[] { "mechanical", "powered", "rechargeable", "neuro_linked" };
public static readonly IReadOnlyList<string> AcceptedPowerProfiles = new[] { "passive", "rechargeable", "high_draw" };
public static readonly IReadOnlyList<string> AcceptedVulnerabilities = new[] { "none", "low", "high" };
public const int MaxFunctionalRestoreBp = 1200;
public const int MaxSkillModifierBp = 500;
public const int MaxDailyDrawWatts = 60;
public const int MaxBatteryDays = 14;
public const int MaxMaintenanceInterval = 30;
public const int MaxDecayBp = 200;
public const int MaxIntegrationRiskBp = 3000;
public const int MaxRecoveryDays = 30;
public const int MaxMalfunctionRiskBp = 1000;
public static BionicsCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json) {
public static List<ImplantDefinition> ToDefinitions(BionicsCatalogLoadResult result) {
```


# Appendix Q.563 — Additional Current Architecture Evidence: `Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs`

### `Assets/Ashfall.Core/Shelter/AquiferPiezometerEngine.cs`

- Evidence status: **CURRENT FILE PRESENT**.
- Size: 828 lines / 43008 bytes.
- SHA-256: `56fb2b37cfee1245ebd2da74d8e81ee2445a11da30a038f724cbafb40af0431b`.
- Architecture signals: seeded references=4; save/restore symbols=2; typed event declarations=4; textual Godot mentions=1; textual Unity/JsonUtility mentions=0; `System.Random`=1; wall-clock DateTime reads=0; dynamic GUIDs=0.
- Integration reading: Core/API evidence establishes the current owner surface. Any extension must preserve engine independence, deterministic ordering and explicit events.

Declared API/declaration digest (implementation bodies intentionally omitted):

```csharp
public sealed class PiezometerStrataDef
public string strata_id { get; set; } = string.Empty;
public string display_name { get; set; } = string.Empty;
public string baseline_recharge_class { get; set; } = "moderate";
public string storage_capacity_class { get; set; } = "moderate";
public float fracture_susceptibility { get; set; } = 0.3f;
public string contamination_transport_class { get; set; } = "moderate";
public Dictionary<string, int> sensor_install_cost { get; set; } = new Dictionary<string, int>();
public float monitoring_accuracy { get; set; } = 0.7f;
public string maintenance_profile_id { get; set; } = string.Empty;
public List<string> monitored_source_ids { get; set; } = new List<string>();
public List<string> tags { get; set; } = new List<string>();
public sealed class PiezometerMaintenanceProfile
public string maintenance_profile_id { get; set; } = string.Empty;
public Dictionary<string, int> required_items { get; set; } = new Dictionary<string, int>();
public float fouling_per_day { get; set; } = 0.01f;
public float calibration_drift_per_day { get; set; } = 0.008f;
public sealed class PiezometerForecastTuning
public float warning_signal_threshold { get; set; } = 0.45f;
public float critical_head_index { get; set; } = 0.25f;
public float stable_head_index { get; set; } = 0.55f;
public float minimum_warning_confidence { get; set; } = 0.3f;
public float heavy_demand_threshold { get; set; } = 0.7f;
public sealed class AquiferPiezometerCatalog
public int schema_version { get; set; } = 1;
public string network_id { get; set; } = "aquifer_network_primary";
public string display_name { get; set; } = "Deep Monitoring Network";
public string sensor_item_id { get; set; } = "item_groundwater_sensor";
public string isolation_module_item_id { get; set; } = "item_aquifer_isolation_module";
public string maintenance_kit_item_id { get; set; } = "item_well_maintenance_kit";
public List<PiezometerStrataDef> strata { get; set; } = new List<PiezometerStrataDef>();
public List<PiezometerMaintenanceProfile> maintenance_profiles { get; set; } = new List<PiezometerMaintenanceProfile>();
public PiezometerForecastTuning forecast { get; set; } = new PiezometerForecastTuning();
public int trend_window { get; set; } = 8;
public List<string> tags { get; set; } = new List<string>();
public sealed class PiezometerNodeState
public string node_id { get; set; } = string.Empty;
public string strata_id { get; set; } = string.Empty;
public float condition { get; set; } = 100f;
public float calibration { get; set; } = 1f;
public float head_index { get; set; } = 0.8f;
public float water_quality_index { get; set; } = 1f;
public float contamination_signal { get; set; } = 0f;
public float clogging_state { get; set; } = 0f;
public bool online { get; set; } = true;
public int last_read_day { get; set; }
public List<float> head_trend { get; set; } = new List<float>();
public sealed class AquiferWarningRecord
public string warning_id { get; set; } = string.Empty;
public string strata_id { get; set; } = string.Empty;
public int issued_day { get; set; }
public int arrival_window_min_days { get; set; }
public int arrival_window_max_days { get; set; }
public float confidence_at_issue { get; set; }
public bool resolved { get; set; }
public sealed class HydrogeologyNetworkState
public int schema_version { get; set; } = 1;
public string network_id { get; set; } = string.Empty;
public bool constructed { get; set; }
public List<PiezometerNodeState> nodes { get; set; } = new List<PiezometerNodeState>();
public float aquifer_health { get; set; } = 80f;
public string drawdown_state { get; set; } = "stable";
public float contamination_risk { get; set; } = 0f;
public float contamination_front_progress { get; set; } = 0f;
public string recharge_state { get; set; } = "moderate";
public float fracture_state { get; set; } = 0f;
public float monitoring_confidence { get; set; } = 0.6f;
public List<string> active_isolation_zone_ids { get; set; } = new List<string>();
public List<AquiferWarningRecord> warnings { get; set; } = new List<AquiferWarningRecord>();
public int last_sample_day { get; set; }
public int days_monitored { get; set; }
public int next_warning_number { get; set; } = 1;
public sealed class AquiferContaminationForecast
public string risk_level { get; set; } = "none";
public int estimated_arrival_window_min { get; set; } = -1;
public int estimated_arrival_window_max { get; set; } = -1;
public List<string> affected_source_ids { get; set; } = new List<string>();
public float confidence { get; set; }
public List<string> recommended_action_tags { get; set; } = new List<string>();
public sealed class HydrogeologyAdvisory
public string advisory_id { get; set; } = string.Empty;
public int day { get; set; }
public string advisory_level { get; set; } = "none";
public string drawdown_state { get; set; } = "stable";
public float monitoring_confidence { get; set; }
public List<string> contaminated_source_ids { get; set; } = new List<string>();
public List<string> isolated_zone_ids { get; set; } = new List<string>();
public List<string> recommended_action_tags { get; set; } = new List<string>();
public static class AquiferPiezometerFailures
public const string NotConstructed = "piez.not_constructed";
public const string AlreadyConstructed = "piez.already_constructed";
public const string MaterialsMissing = "piez.materials_missing";
public const string UnknownNode = "piez.unknown_node";
public const string UnknownZone = "piez.unknown_zone";
public const string ZoneAlreadyIsolated = "piez.zone_already_isolated";
public const string ZoneNotIsolated = "piez.zone_not_isolated";
public const string MaintenanceNotNeeded = "piez.maintenance_not_needed";
public sealed class AquiferPiezometerEngine
public Func<int>? DayProvider { get; set; }
public Func<float>? PumpDemandProvider { get; set; }
public Func<float>? SeasonalRechargeModifierProvider { get; set; }
public Func<float>? HydrogeologistSkillProvider { get; set; }
public Func<float>? WellDrillerSkillProvider { get; set; }
public HydrogeologyNetworkState State => _state;
public AquiferPiezometerCatalog Catalog => _catalog;
public bool IsConstructed => _state.constructed;
public event Action<AquiferWarningRecord>? OnWarningIssued;
public event Action<string>? OnEventRaised;
public void BindCatalog(AquiferPiezometerCatalog catalog) {
public void BindInventory(Func<string, int> getCount, Action<string, int> consume) {
public ActionResult ConstructNetwork() {
public void TickDay(int day) {
public void ReportSurfaceContaminationEvent(float severity01) {
public void ReportFractureEvent(float severity01) {
public AquiferContaminationForecast ForecastContamination(string? strataId = null) {
public HydrogeologyAdvisory BuildAdvisory() {
public List<string> AvailableSourceIds() {
public bool IsSourceIsolated(string sourceId) {
public ActionResult IsolateAquiferZone(string zoneId) {
public ActionResult ReconnectAquiferZone(string zoneId) {
public ActionResult MaintainNode(string nodeId) {
public HydrogeologyNetworkState CaptureState() {
public void RestoreState(HydrogeologyNetworkState? state) {
```


# Appendix R — Rebuild Closeout Note

- Current-evidence snapshot: 2026-09-25.
- Core/host/catalog/test appendices are generated from the working tree and carry file hashes.
- No fresh code test result is asserted by this planning rebuild.
- The external verifier checks content range, required sections, path labeling, repetition and stale generated-path artifacts.
- This document may be shorter than the target if verified material is exhausted; it may not be padded to reach it.
