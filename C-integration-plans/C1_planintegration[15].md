# C1 — Flagship Integration Plan [15]: The Mod & Content-Pack Contract — Stable Boundaries, Deterministic Overlays & Compatibility Governance

> **Output:** `C1_planintegration[15].md`
>
> **Source baseline:** Plan 47 — The Mod & Content-Pack Contract: Write Down the Boundary
>
> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
>
> **Depends on:** Plan 45A acceptance ladder; Plan 26A canonical data-path resolver; Plan 40B tags; Plan 25C locale overlay semantics; Plan 27A authority-backed fixtures; Plan 46A deterministic sweeps; Plan 48A changelog/release notes; Plan 29B generated canon claims.
>
> **Mandatory execution order:** 45A → 47A → 47B → 47C.
>
> **Public-communication constraint:** 47A must land before ASHFALL makes any public claim that content packs or modding are supported. A support promise is part of the API surface.
>
> **Primary architectural rule:** ASHFALL supports data/content packs only. No code execution, assemblies, scene replacement, arbitrary shaders, input-map hijacking, per-pack save sections, or executable scripting.
>
> **Primary compatibility rule:** the supported pack surface is generated from the same validators, registries, version sources, and save rules that enforce the base game. Documentation must never become a second handwritten truth.
>
> **Primary determinism rule:** pack merging, definition iteration, validation, save fallback, and runtime selection must remain stable under identical base data, pack set, load order, seed, and player inputs.
>
> **Guardrails:** no Steam Workshop coupling in this plan; no encrypted pack format; no unversioned manifests; no filesystem-order precedence; no arbitrary code hooks; no save-schema extension by packs; no weakening base-game integrity checks for modded runs; no “best effort” silent partial merge after validation failure.

---

# 0. Mission

ASHFALL is already moddable by accident.

The repository already contains:
- an `ASHFALL_DATA` override that can point the game at a different content directory;
- a resolver that can choose PCK-backed or filesystem-backed data;
- temp data directories used by selftests;
- `schema_version` on the project's catalogs;
- a multi-tier `CatalogIntegrityValidator`;
- canonical ID prefixes and reference checks;
- tags as behavior-extension vocabulary;
- locale overlays keyed by definition ID;
- a save section registry and strict envelope contract;
- deterministic RNG and culture-invariant simulation requirements;
- content-utilization runtime evidence.

But none of these pieces currently form a documented support contract.

The current implicit model is:

```text
"replace data directory"
        │
        ▼
some catalogs probably load
        │
        ▼
maybe references work
        │
        ▼
maybe saves still work
        │
        ▼
no public compatibility promise
```

Plan 47 turns that into a supported, bounded, testable contract:

```text
BASE CONTENT
    │
    ▼
OFFICIAL PACKS
    │
    ▼
USER PACKS
    │
    ▼
DETERMINISTIC OVERLAY MERGE
    │
    ▼
INTEGRITY VALIDATION
    │
    ▼
ACCEPTANCE VALIDATION
    │
    ▼
EFFECTIVE CONTENT SET
    │
    ▼
GAME BOOT
```

The user-facing promise becomes explicit:

```text
A content pack MAY:
- add or override supported data definitions;
- retire supported definitions where contract permits;
- add tags;
- add locale overlays;
- add supported world/content definitions;
- rely on documented stable identifiers and schemas.

A content pack MAY NOT:
- run code;
- load assemblies;
- add arbitrary Godot scenes;
- replace input maps;
- inject shaders;
- add new save-section keys;
- mutate deterministic RNG contracts;
- rely on undocumented internal catalogs.
```

The release promise becomes equally explicit:

```text
change to stable pack surface
        │
        ▼
generated contract diff
        │
        ▼
BREAKING change classification
        │
        ▼
deprecation/changelog requirement
        │
        ▼
CI gate
```

The result is a mod surface ASHFALL can actually support.

---

# 1. Source-Evidence Interpretation

## 1.1 External data loading already exists

The source baseline identifies `ASHFALL_DATA` as precedence #1 when a valid directory exists, and notes that the host can swap between packaged and filesystem IO.

This is not a hypothetical future loader.

47B should generalize it rather than create another data-root mechanism.

## 1.2 Alternate data roots already exist in tests

Several selftests create temporary data directories.

That proves:
- the loader stack already supports non-default content roots;
- fixture-pack testing can reuse existing test mechanics.

## 1.3 Catalog versioning already exists

The baseline states all 411 JSON catalogs declare `schema_version`.

The plan should convert that broad version presence into a pack compatibility rule.

## 1.4 The validator already defines much of the public surface

The validator knows:
- registry rules;
- ID prefixes;
- references;
- ranges;
- uniqueness.

Those rules should generate the public contract.

Do not rewrite them manually in a doc.

## 1.5 Tags and locale overlays already demonstrate extensibility

Wave 6 tags and Wave 3 locale overlays are the clearest supported extension primitives.

They should be explicitly named in the public contract.

## 1.6 Saves are the dangerous boundary

Packs can safely change content much more easily than persisted structure.

Therefore:
- save-section keys remain forbidden;
- stable IDs referenced by saves become part of the compatibility promise;
- missing pack content must degrade gracefully when a save is loaded without the pack.

## 1.7 Determinism is a compatibility boundary

A pack that changes load order nondeterministically or introduces unseeded behavior can break:
- replay;
- long deterministic sweeps;
- regression hashes.

Therefore deterministic merge order is non-negotiable.

## 1.8 Handwritten mod documentation would drift

The project has already experienced hand-maintained documentation diverging from source.

The mod contract must therefore be generated and gated.

---

# 2. Non-Negotiable Modding Invariants

## INV-47.1 — Data-only packs

Packs never execute arbitrary code.

## INV-47.2 — One canonical content resolver

Pack discovery extends `CatalogPath` / canonical resolver.

No second data-loading root.

## INV-47.3 — Deterministic precedence

Effective content order is fully specified.

Never filesystem enumeration order.

## INV-47.4 — Per-definition overlay semantics

Supported overrides operate by stable definition ID.

Whole-file replacement is not the default public model.

## INV-47.5 — Validation before activation

A pack's merged result is validated before gameplay systems consume it.

## INV-47.6 — Invalid packs never partially apply

A rejected pack cannot leave half of its definitions merged.

## INV-47.7 — Stable identity contract is generated

Prefixes, uniqueness, and references come from authoritative validation rules.

## INV-47.8 — Pack manifests are versioned

Every pack declares:
- pack ID;
- pack version;
- target game/data contract range;
- dependencies if supported;
- overlay roots.

## INV-47.9 — Packs cannot add save sections

Only `SaveSectionRegistry` owns save-section keys.

## INV-47.10 — Saves remain loadable without packs

Where a save references pack IDs that are now missing:
- loading should degrade gracefully;
- no crash;
- no corrupt save rewrite.

## INV-47.11 — Missing definitions are explicit

Graceful fallback does not mean silent substitution.

Missing IDs are surfaced in diagnostics.

## INV-47.12 — Disabled packs do not perturb base determinism

Base campaign digest with no active packs remains unchanged.

## INV-47.13 — Enabled pack determinism is stable

Same pack set/order + same seed => same merged content order and simulation result.

## INV-47.14 — Contract changes are release-governed

Breaking stable-surface changes require:
- generated diff;
- changelog marker;
- version-policy compliance.

## INV-47.15 — Deprecated stable fields cannot disappear early

Deprecation windows are machine-enforced.

## INV-47.16 — Public docs are generated

`docs/modding/CONTRACT.md` is never hand-maintained.

---

# 3. Definition of Done

Plan 47 closes only when:

- a generated `docs/modding/CONTRACT.md` exists;
- the contract classifies supported layers as STABLE / INTERNAL / FORBIDDEN;
- the supported pack mechanism is explicit;
- load/override semantics are specified;
- ID rules are generated from `CatalogIntegrityRules`;
- save compatibility boundaries are documented;
- determinism rules are documented and test-backed;
- only actually existing supported schemas are listed;
- forbidden surfaces are explicit;
- contract version is generated;
- game/data/save compatibility matrix is generated from authoritative version sources;
- author workflow is documented;
- contract drift gate exists;
- legal/illegal fixture pack tests exist;
- pack discovery exists as a supported code path;
- precedence is deterministic;
- per-definition add/modify/retire is supported where allowed;
- schema compatibility is checked before merge;
- merged content is fully validated before activation;
- invalid packs are disabled/rejected atomically;
- effective content report is emitted;
- reserved namespace collisions fail;
- path traversal fails;
- pack size/shape limits exist;
- disabled-pack save load degrades gracefully;
- pack manifests support version ranges;
- duplicate loader boilerplate is consolidated where safe;
- fixture example pack exists;
- fixture-pack matrix runs in CI;
- 30-day deterministic soak runs for packs;
- pack-enabled save reloads with pack and without pack;
- stable-surface breaking changes are diffed automatically;
- breaking changes require `BREAKING:` changelog entry;
- deprecation windows are enforced;
- mod-relevant version information appears in the version report;
- determinism/save/reference gates run in the same release tier;
- base-vs-pack content usage can be measured locally;
- pack-support issue template/workflow is documented;
- non-goals are explicit;
- release gate passes with fixture packs enabled.

---

# 4. Phase P0 — Re-verify the Current Mod Surface

## P0.1 Capture baseline

Record:

```text
commit SHA
branch
dirty paths
CatalogPath precedence
ASHFALL_DATA behavior
FileSystemIO/GodotFileIO seam
catalog count
schema version set
CatalogIntegrityRules prefix set
SaveSectionRegistry keys
SaveWireContract version
VersionReport fields
current whitelist directories
locale overlay implementation
tag overlay implementation
content utilization stages
existing data-root selftests
current boot time baseline
```

---

## P0.2 Build mod-surface inventory

Create:

`docs/modding/MOD_SURFACE_INVENTORY.md`

Columns:

```text
surface
authority
current use
pack safe?
stability class
versioned?
validated?
save impact
determinism impact
notes
```

Rows:
- JSON catalogs;
- tags;
- locale overlays;
- location/world data;
- encounter data;
- memorial/epitaph data;
- policy catalogs;
- voice data;
- save sections;
- code;
- scenes;
- shaders;
- input maps;
- RNG streams;
- whitelists;
- internal generated artifacts.

---

## P0.3 Enumerate stable candidate schemas

Only schemas with:
- real loaders;
- real acceptance path;
- current tests;
may be considered for STABLE pack support.

Do not list aspirational schemas.

---

## P0.4 Reproduce current alternate-data behavior

Use temp data dir and `ASHFALL_DATA`.

Verify:
- resolver chooses it;
- packaged content is replaced/overridden according to current behavior;
- record deficiencies.

---

## P0.5 Establish base boot benchmark

Record:
- median;
- p95;
- catalog parse count;
- validation time.

Used for 47B performance acceptance.

---

# TASK 47A — Define the Public Contract

# 47A.0 Goal

Create a generated, versioned statement of exactly what a content pack may rely on.

---

## 47A.1 Create contract generator

File:

`scripts/ci/generate-mod-contract.py`

Inputs should come from:
- `CatalogIntegrityRules`;
- `CatalogIntegrityValidator`;
- catalog registry;
- `SaveSectionRegistry`;
- `VersionReport`;
- supported overlay registries;
- stable schema metadata.

---

## 47A.2 Generated contract path

Output:

`docs/modding/CONTRACT.md`

Start with:

```text
GENERATED FILE — DO NOT EDIT
source authorities
generator command
contract version
```

---

## 47A.3 Stability classes

Define:

```text
STABLE
INTERNAL
FORBIDDEN
```

Exact meaning:

### STABLE
Public pack contract; compatibility promises apply.

### INTERNAL
May change without pack compatibility guarantees.

### FORBIDDEN
Pack loader rejects direct use/modification.

---

## 47A.4 Layer table

Generate rows for:
- catalogs by family;
- tags;
- locale overlays;
- content pack manifest;
- save schema;
- save-section keys;
- RNG streams;
- code;
- scenes;
- shaders;
- input maps;
- internal whitelists.

---

## 47A.5 Supported installation mechanism

Declare one supported mechanism.

Recommended:

```text
base content
→ official overlays
→ user pack overlays
→ explicit ASHFALL_DATA developer override
```

If `ASHFALL_DATA` remains a full replacement/developer mechanism, document it separately from user-pack install path.

---

## 47A.6 Distinguish developer override from user packs

`ASHFALL_DATA` may be:
- developer/test full-root override;
- not necessarily end-user pack folder.

47B can introduce a pack folder while preserving env override.

---

## 47A.7 Public overlay semantics

Default public behavior:
- per-definition merge by `id`;
- add;
- modify;
- retire.

Do not promise arbitrary full-file replacement as stable.

---

## 47A.8 Overlay operation contract

### Add
New ID in supported namespace.

### Modify
Existing supported definition ID, only allowed fields.

### Retire
Explicit tombstone/retired marker where family permits.

Do not use file deletion as retirement semantics.

---

## 47A.9 Field-level stability

For each STABLE family, identify:
- required fields;
- optional stable fields;
- internal fields if any.

Generate from schemas/validator metadata where possible.

---

## 47A.10 Identity contract

Generate:
- snake_case rule;
- allowed prefixes;
- uniqueness;
- reference resolution.

No manual prefix list.

---

## 47A.11 Reserved namespaces

Mark:
- internal flags;
- internal whitelists;
- save section IDs;
- other restricted prefixes.

Pack loader rejects use.

---

## 47A.12 Save compatibility promise

Document:

```text
packs may reference existing save-stable IDs
packs may add content IDs
packs may not add save-section keys
packs may not change persisted wire shape
renaming a save-referenced ID is breaking/unsupported unless migration exists
```

---

## 47A.13 Missing-ID save fallback policy

Document expected behavior:
- placeholder/missing-content marker;
- skip invalid optional content;
- preserve raw ID if possible;
- warning.

Never silently map to unrelated base item.

---

## 47A.14 Determinism contract

Generate/reference tests for:
- `ISeededRng`;
- stable iteration;
- invariant culture;
- no `Guid.NewGuid()` in deterministic paths;
- deterministic pack ordering.

---

## 47A.15 Supported pack families

List only verified families such as current:
- items with tags;
- locations/map nodes if Plan 32 support is complete;
- encounters/echoes;
- voice lines if schema live;
- policies if 43B live;
- memorial/epitaph definitions;
- locale overlays.

Recompute actual list at execution time.

---

## 47A.16 Unsupported families

Anything without 45A acceptance rails remains:
- INTERNAL;
or:
- FORBIDDEN.

Do not overpromise.

---

## 47A.17 Explicit forbidden surfaces

Document rejection for:
- assemblies/DLLs;
- C# source;
- GDScript;
- scene replacement;
- shader injection;
- input remapping;
- executable scripts;
- arbitrary native files;
- save-section keys outside registry.

---

## 47A.18 Contract version

Create:

```text
mod_contract_version
data_contract_version
save_contract_version
```

Version changes follow release policy.

---

## 47A.19 Compatibility matrix

Generate:

```text
game version
mod contract version
data schema range
save schema/wire version
```

---

## 47A.20 Source of version truth

Use `VersionReport`.

Do not duplicate version strings in generator.

---

## 47A.21 Pack author loop

Document:

```text
scaffold
→ validate manifest
→ enable pack
→ data integrity
→ family acceptance
→ content utilization
→ deterministic smoke
```

---

## 47A.22 Error-reading guide

Explain rejection categories:
- manifest;
- schema;
- IDs;
- refs;
- version;
- forbidden operation;
- determinism;
- save compatibility.

---

## 47A.23 Pack manifest schema

Define public fields:

```text
id
name
version
author/display only if desired
supported_game_range
supported_mod_contract_range
load_order_class
dependencies
content_roots
```

Keep small.

---

## 47A.24 Pack ID rules

Pack IDs:
- snake_case;
- globally unique within installed set;
- deterministic ordering key.

---

## 47A.25 Dependency policy

If dependencies are supported:
- pack ID + version range;
- acyclic;
- validated.

If not needed initially:
- explicitly unsupported.

Do not half-support.

---

## 47A.26 Conflict policy

When two packs modify same ID:
- deterministic last-wins by declared precedence;
or:
- reject conflict.

Pick one public policy.

Recommended:
- deterministic precedence + explicit conflict report.

---

## 47A.27 Conflict report

Effective-content report lists:
- original source;
- override chain;
- final source.

---

## 47A.28 Contract drift gate

Run:

```bash
python3 scripts/ci/generate-mod-contract.py --check
```

Tier 2 or appropriate tier.

---

## 47A.29 Contract-vs-validator test

Change a validator rule fixture.

Expected:
- generated contract diff.

---

## 47A.30 Legal pack fixture

Fixture:
- one supported item;
- valid tag;
- valid locale overlay.

Expected:
- loads/validates.

---

## 47A.31 Illegal pack fixture

Examples:
- unknown prefix;
- invalid reference;
- forbidden save section.

Expected:
- rejected with actionable message.

---

## 47A.32 AGENTS linkage

Add one canonical rule:

```text
Content-pack boundary: docs/modding/CONTRACT.md
```

No copied contract prose.

---

## 47A.33 47A metrics

Record:

```text
stable surfaces
internal surfaces
forbidden surfaces
stable families
contract version
validator rules represented
manual contract claims
```

Target:
- zero manually duplicated rules.

### 47A DoD

ASHFALL has one generated public content-pack contract backed by the same validators and version authorities used by the game.

---

# TASK 47B — Pack Discovery, Merge, Validation & Safe Loading

# 47B.0 Goal

Turn "install a content pack" into a deterministic, validated loader path.

---

## 47B.1 Create `ContentPackService`

Host file:

`src/Host/ContentPackService.cs`

Responsibilities:
- discover pack manifests;
- resolve order;
- validate manifest/version;
- create merged content view;
- report active/rejected packs.

Do not own domain-specific catalog parsing.

---

## 47B.2 Pack root location

Use existing user-data root conventions.

Example:

```text
<user data>/content_packs/
```

Do not write into installed PCK.

---

## 47B.3 Resolver precedence

Canonical order:

```text
base PCK/base dir
→ official expansion packs by declared order
→ user packs by deterministic pack ID / declared priority policy
→ explicit ASHFALL_DATA developer override
```

Finalize after reconciling current resolver semantics.

---

## 47B.4 Never filesystem order

Directory enumeration must be sorted ordinally.

---

## 47B.5 Manifest discovery

Only directories with valid pack manifest are candidates.

Ignore unrelated user files safely.

---

## 47B.6 Manifest validation

Validate:
- ID;
- version;
- compatibility range;
- content paths;
- dependencies if supported;
- path safety.

---

## 47B.7 Path traversal rejection

Reject:
- `../`;
- absolute paths;
- symlink escape where relevant;
- encoded traversal forms.

---

## 47B.8 Size limits

Set bounded:
- manifest size;
- file count;
- per-file size;
- total unpacked size.

Data packs should not become a trivial memory exhaustion vector.

---

## 47B.9 Extension allowlist

Accept only:
- supported JSON/text/audio assets if public contract explicitly permits them.

If audio binary packs are not supported, forbid them.

Do not accidentally load executable formats.

---

## 47B.10 Engine-free merge layer

Implement per-definition overlay merge in Core.

No Godot types.

---

## 47B.11 Merge input

For each family:

```text
base definitions
+ ordered overlays
→ effective definitions
```

---

## 47B.12 Add operation

New ID:
- validate prefix;
- validate uniqueness;
- validate references after full merge.

---

## 47B.13 Modify operation

Existing ID:
- merge permitted fields;
- required immutable identity retained.

---

## 47B.14 Retire operation

Explicit tombstone:

```text
id
retired: true
```

or family-specific canonical form.

---

## 47B.15 Retirement safety

Reject retirement when:
- required base invariant depends on ID;
- save-critical mandatory definition;
- internal reserved entity.

---

## 47B.16 Schema compatibility before merge

A pack definition schema version outside supported range:
- reject that pack before mutation.

---

## 47B.17 Atomic pack activation

Each pack is:
- wholly accepted;
or:
- wholly rejected.

No half-applied pack.

If dependency chain requires disabling dependents:
- report it clearly.

---

## 47B.18 Full effective-set validation

After all accepted overlays:
- registry;
- prefixes;
- references;
- ranges;
- uniqueness;
- family integrity.

Boot must not proceed with invalid merged set unless user explicitly chooses safe-mode disable of bad packs.

---

## 47B.19 Safe boot strategy

Preferred:
1. discover packs;
2. validate;
3. disable invalid packs;
4. revalidate remaining set;
5. boot menu;
6. show pack warning/report.

For strictly required dependencies:
- disable dependents too.

---

## 47B.20 No silent skip

Rejected pack visible in:
- log;
- version/effective-content report;
- optional UI notification.

---

## 47B.21 Effective content report

Record:

```text
base definition counts
active pack IDs/versions
rejected pack IDs/reasons
added IDs
modified IDs
retired IDs
conflicts
effective counts
contract/data/save versions
```

---

## 47B.22 VersionReport integration

Expose mod-relevant:
- active packs;
- contract version;
- data version;
- save version.

---

## 47B.23 Content-utilization integration

Runtime evidence identifies:
- source pack per definition;
- base vs pack.

---

## 47B.24 Provenance

Each effective definition should be able to report:
- base;
- official pack;
- user pack ID.

This is diagnostic metadata, not gameplay state.

---

## 47B.25 Reserved namespace collision

Reject pack definitions using:
- forbidden internal prefixes;
- whitelists;
- save sections;
- other reserved IDs.

---

## 47B.26 Unknown family

If pack contains unsupported catalog family:
- reject with contract link/reason.

---

## 47B.27 Unsupported field

If stable schema disallows field:
- reject or ignore only if schema explicitly says unknown fields tolerated.

Do not silently accept accidental typo fields.

---

## 47B.28 Loader consolidation

Audit the 10 `*CatalogLoader.cs` families.

Extract common:
- read;
- parse;
- schema check;
- warnings;
- provenance;
- pack overlay input.

Keep domain-specific validation separate.

---

## 47B.29 Fixture fidelity

All tests use the same merged content loader path.

No special pack-test loader.

---

## 47B.30 Deterministic effective order

Effective definitions sorted by:
- declared canonical ID order;
or:
- stable source order defined by contract.

No hash/dictionary iteration.

---

## 47B.31 Disabled-pack base digest

Paired-seed:
- no packs enabled;
- pack loader code active but empty.

Expected:
- identical digest to pre-pack baseline.

---

## 47B.32 Enabled-pack digest

Same active pack set:
- identical digest across runs/platforms where supported.

---

## 47B.33 Save with pack enabled

Create save containing references to pack-added:
- item;
- location;
- content ID
where supported.

---

## 47B.34 Load save with pack disabled

Expected:
- save loads;
- missing references become explicit placeholders/omissions;
- no crash;
- original IDs preserved where possible;
- diagnostics list missing pack content.

---

## 47B.35 Re-enable pack

Reload same save with pack restored.

Expected:
- pack IDs resolve again where data preserved.

---

## 47B.36 Save rewrite safety

Do not rewrite missing pack IDs to unrelated base IDs on load.

---

## 47B.37 Missing mandatory content

If pack retired a mandatory base entity and save needs it:
- pack should have been rejected during validation.

---

## 47B.38 Version pinning

Manifest range fields:
- min/max game;
- min/max mod contract;
- data schema range.

Out-of-range:
- disabled by default;
- actionable warning.

---

## 47B.39 Override compatibility

A modify operation against an ID whose schema changed:
- version check catches;
- pack does not partially adapt.

---

## 47B.40 Pack dependency ordering

If supported:
- topological sort;
- deterministic tie-break;
- cycle fails.

---

## 47B.41 Conflict determinism test

Two packs override same ID.

Expected:
- documented winner;
- conflict report;
- same result every run.

---

## 47B.42 Boot cost measurement

Compare:
- pack loader disabled/no packs;
- one small pack;
- large fixture pack.

Measure median/p95.

---

## 47B.43 Base overhead budget

No-pack path should remain within Plan-26 boot budget.

If additional scan cost exists:
- bound and report it.

---

## 47B.44 Cache strategy

If needed:
- cache manifest/effective merge by file hashes.

Do not introduce cache before profiling proves need.

---

## 47B.45 Install documentation

Create:

`docs/modding/INSTALL.md`

Include:
- folder;
- manifest;
- enabling/disabling;
- validation;
- version report;
- troubleshooting.

---

## 47B.46 Working example

Ship fixture example in test project.

Do not promise production shipping of sample pack if not desired.

---

## 47B.47 Example pack content

Minimal:
- one additive item;
- one tag;
- one locale line;
- one supported overlay.

---

## 47B.48 Example pack acceptance

Must pass:
- data integrity;
- contract validation;
- acceptance ladder appropriate to family.

---

## 47B.49 Illegal input security tests

Include:
- traversal;
- absurd size;
- malformed JSON;
- unsupported extension;
- duplicate pack ID;
- reserved namespace.

---

## 47B.50 47B metrics

Record:

```text
pack discovery time
merge time
validation time
base boot delta
active packs
rejected packs
definitions added/modified/retired
conflicts
```

### 47B DoD

A user can install, disable, and validate a content pack without touching game code, without nondeterministic precedence, and without risking an unreadable save.

---

# TASK 47C — Compatibility as CI and Release Policy

# 47C.0 Goal

Turn the public content-pack contract into a regression suite and release obligation.

---

## 47C.1 Fixture-pack suite

Create fixtures:

```text
minimal_legal
additive
override
retire
bad_reference
reserved_namespace
out_of_range
tags_only
locale_only
save_reference
dependency_chain if supported
conflict_pair
```

---

## 47C.2 Fixture manifest quality

Each fixture is intentionally tiny.

One behavior per pack.

---

## 47C.3 CI pack gate

Create/register named gate:

```text
pack_contract_regression
```

Runs:
- load;
- validate;
- merge;
- 30-day deterministic soak;
- save/reload;
- disabled-pack reload.

---

## 47C.4 Fast-tier split

Pure:
- manifest/schema/security;
- merge;
- contract diff
may run fast.

Longer 30-day soak:
- Tier 2 if necessary.

The source plan requests a named fast-tier gate; preserve fast usability by splitting if runtime requires it.

---

## 47C.5 Base-without-packs regression

Every pack CI lane includes control:
- no active packs;
- base digest unchanged.

---

## 47C.6 Stable-surface snapshot

Generate machine-readable contract surface:

```text
mod contract version
stable families
stable fields
ID prefixes
tag vocabulary
save section keys
schema versions
```

---

## 47C.7 Previous-release comparison

Diff current surface against previous release tag.

---

## 47C.8 Breaking-change classifier

Breaking examples:
- remove stable field;
- rename stable prefix;
- remove stable tag;
- change required field semantics;
- remove supported family;
- change save-referenced stable ID contract.

---

## 47C.9 Non-breaking examples

- add optional field;
- add stable family;
- add tag;
- add catalog entry.

Still subject to acceptance.

---

## 47C.10 `BREAKING:` requirement

If classifier detects breaking stable-surface change:
- changelog/release decision must include `BREAKING:` line;
- CI fails otherwise.

---

## 47C.11 Version bump requirement

Breaking stable contract may require:
- mod contract major;
- game major/minor according to policy.

Encode in tests.

---

## 47C.12 Deprecation metadata

Stable field/prefix may be marked:

```text
deprecated_since
remove_not_before
replacement
```

---

## 47C.13 Deprecation warnings

Pack validation warns when deprecated surface used.

---

## 47C.14 Early-removal gate

If current date/version is before allowed removal:
- CI fails on deletion.

---

## 47C.15 Removal gate

When window expires:
- removal allowed only with breaking classification/changelog.

---

## 47C.16 Supported-changes release artifact

Generate:

```text
Added
Deprecated
Removed
Breaking
```

for pack authors.

---

## 47C.17 Changelog integration

Plan 48A consumes generated compatibility diff.

No manual recollection.

---

## 47C.18 VersionReport UI/CLI

Expose:

```text
Game
Mod contract
Data contract
Save contract
Active packs
```

---

## 47C.19 Support artifact

Effective-content report should be exportable/copyable for bug reports.

---

## 47C.20 Issue template

Request:
- game SHA/version;
- pack manifest;
- active pack list;
- effective-content report;
- validator output;
- save version;
- reproduction steps.

---

## 47C.21 No private data

Report excludes:
- username;
- home directory;
- personal paths;
- account IDs.

---

## 47C.22 Determinism invariant gate

Run:
- same seed + pack;
- compare digest.

---

## 47C.23 Save invariant gate

Run:
- save with pack;
- reload with pack;
- reload without pack.

---

## 47C.24 Reference invariant gate

Every effective merged reference resolves before boot.

---

## 47C.25 Pack reachability metrics

Local-only metrics can compare:
- base definitions selected/effect-producing;
- pack definitions selected/effect-producing.

---

## 47C.26 Privacy stance

No remote telemetry required.

Local diagnostic report is enough.

---

## 47C.27 Adoption question

If post-release telemetry/diagnostics are voluntarily used later, keep this plan neutral.

Do not build analytics backend.

---

## 47C.28 Non-goals documentation

Explicit:
- no code mods;
- no DLL loading;
- no Steam Workshop;
- no encrypted packs;
- no executable scripts;
- no arbitrary scenes;
- no per-pack saves.

---

## 47C.29 Capability claim update

Plan 29B canon should say only:
- content packs supported within generated contract.

No broad "mod support" claim if code mods are forbidden.

---

## 47C.30 Release gate integration

Plan 39A/release pipeline runs:
- fixture packs enabled;
- no-pack control;
- export smoke.

---

## 47C.31 Pack-enabled exported build smoke

Verify:
- external user pack folder found where supported;
- base PCK still authoritative first layer;
- report lists active pack.

---

## 47C.32 Pack-disabled exported smoke

No pack:
- identical base behavior.

---

## 47C.33 Meta-test breaking fixture

Deliberately:
- remove stable prefix/field.

Assert:
- diff tool marks breaking;
- CI requires changelog marker.

---

## 47C.34 Contract-version meta-test

If stable surface changes incompatibly but version stays same:
- fail.

---

## 47C.35 Deprecation meta-test

Remove a still-supported deprecated field early.

Fail.

---

## 47C.36 Release notes pack section

Generated section:

```text
Content-pack compatibility
- contract version
- new supported surfaces
- deprecated
- breaking
```

---

## 47C.37 Long-term support policy

Document:
- how many minor versions stable surface is supported;
- deprecation window;
- major-version reset rules.

Keep promise conservative.

---

## 47C.38 47C metrics

Publish:

```text
fixture packs passing
stable surface changes
breaking changes
deprecations
compatibility warnings
save-without-pack failures
determinism failures
```

### 47C DoD

A breaking content-pack compatibility change cannot reach release without CI identifying it, a version/deprecation decision, and a changelog declaration.

---

# 5. Cross-Task Dependency Graph

```text
45A acceptance contract
        │
        ▼
47A public mod contract
        │
        ▼
47B deterministic pack loader
        │
        ▼
47C release compatibility gate
```

Supporting:

```text
26A CatalogPath ─────────► discovery/resolution
40B tags ────────────────► pack-safe behavior vocabulary
25C locale overlays ────► merge semantics
27A fixtures ────────────► fixture packs
46A deterministic sweeps ► 30-day soak
29B generated canon ─────► support claim
48A changelog ───────────► BREAKING release notes
```

---

# 6. Public Surface Classification

Recommended generated categories:

| Surface | Class |
|---|---|
| supported JSON family | STABLE |
| tags | STABLE |
| locale overlay | STABLE |
| pack manifest | STABLE |
| save-section registry | FORBIDDEN to extend |
| internal whitelist | FORBIDDEN |
| runtime code | FORBIDDEN |
| assemblies | FORBIDDEN |
| scenes | INTERNAL/FORBIDDEN |
| shaders | FORBIDDEN |
| input maps | FORBIDDEN |
| internal generated docs | INTERNAL |
| RNG implementation | INTERNAL, behavior contract stable |

---

# 7. Pack Manifest Contract

Example:

```json
{
  "schema_version": 1,
  "id": "example_pack",
  "version": "1.0.0",
  "game_range": ">=1.2 <2.0",
  "mod_contract_range": ">=1 <2",
  "content_roots": ["Data"],
  "dependencies": []
}
```

Exact version syntax should use existing version library/convention.

---

# 8. Overlay Merge Contract

For one definition ID:

```text
base
→ official override(s)
→ user override(s)
→ effective definition
```

Every step records provenance.

---

# 9. Conflict Policy

If last-wins:

```text
priority class
→ dependency order
→ pack ID ordinal
```

Do not let user filesystem install time decide.

If explicit priority is supported:
- range bounded;
- tie-break ordinal.

---

# 10. Retire/Tombstone Contract

Retire is allowed only on pack-safe definitions.

A tombstone must:
- preserve ID identity in provenance;
- fail if target not found unless `add_or_retire_unknown` explicitly supported (not recommended).

---

# 11. Validation Pipeline

```text
manifest validation
→ file/path safety
→ schema compatibility
→ overlay parse
→ deterministic merge
→ registry/prefix validation
→ reference validation
→ range/uniqueness
→ acceptance metadata
→ effective set activation
```

No gameplay system sees content earlier.

---

# 12. Save Compatibility Contract

Packs may add IDs that saves can reference.

Therefore loader must support:

```text
pack enabled save
→ pack disabled load
→ missing content marker / safe omission
→ no crash
→ preserve unknown ID when possible
```

---

# 13. Missing-Content Fallback Rules

By domain:

### Inventory item
- retain placeholder/missing item record if feasible;
- never convert to unrelated item.

### Location
- mark unavailable/missing content;
- remove from active route safely.

### Narrative/codex
- omit unavailable entry;
- preserve history ID.

### Policy/quest
- if active state depends on missing pack content, fail that state gracefully with explicit diagnostic.

These rules need per-domain tests.

---

# 14. Deterministic Pack Order

Pack order digest should include:

```text
pack id
pack version
dependency order
priority
```

Store in diagnostic report.

---

# 15. Content Provenance

Every effective definition carries diagnostic provenance:

```text
base source
last modifier
override chain
```

Not necessarily persisted into save if derivable from active pack set.

---

# 16. Security Boundaries

Reject:
- traversal;
- executables;
- symbolic escape where relevant;
- huge inputs;
- malformed nesting;
- unsupported extensions;
- reserved namespaces.

This is defensive input handling, not an anti-cheat system.

---

# 17. Performance Budget

Measure:

```text
base boot
base + pack discovery no packs
small pack
large fixture pack
```

Targets:
- no material base regression;
- merge/validation bounded.

---

# 18. Fixture-Pack Matrix

| Fixture | Purpose |
|---|---|
| minimal_legal | contract happy path |
| additive | new IDs |
| override | modify |
| retire | tombstone |
| bad_reference | validator rejection |
| reserved_namespace | security/contract |
| out_of_range | version rejection |
| tags_only | extension vocabulary |
| locale_only | overlay |
| save_reference | save fallback |
| conflict_a/b | deterministic conflict |
| dependency_cycle | if dependencies supported |

---

# 19. Pack Regression Journey

Example 30-day run:

```text
boot base + pack
→ add item selected
→ locale overlay displayed
→ tagged item behavior consumed
→ save
→ reload with pack
→ continue
→ disable pack
→ load
→ graceful missing-content handling
```

---

# 20. Breaking-Change Detection Surface

Generate JSON snapshot:

```text
contract_version
families
stable fields
prefixes
tags
schema versions
save section keys
```

Diff between release tags.

---

# 21. Breaking Classification Rules

Breaking:
- delete stable family;
- delete required field;
- rename prefix;
- rename stable ID semantics;
- shrink allowed enum incompatibly;
- change overlay operation semantics;
- remove tag used as public behavior key.

Potentially non-breaking:
- add optional field;
- add tag;
- add family;
- broaden allowed range.

---

# 22. Deprecation Policy

Recommended:

```text
announce deprecation
→ warn for N minor releases
→ allow removal at next major
```

Choose N conservatively.

Machine-enforce dates/versions.

---

# 23. Release Compatibility Matrix

Generated table:

```text
Game 1.4.x
Mod contract 2
Data schema 3–4
Save wire 7
```

Support docs and version report use same values.

---

# 24. Documentation Set

Create:

```text
docs/modding/CONTRACT.md          generated
docs/modding/INSTALL.md           authored
docs/modding/TROUBLESHOOTING.md   authored/generated hybrid
docs/modding/MOD_SURFACE_INVENTORY.md
fixtures/content_pack_example/
```

Generated files clearly marked.

---

# 25. CI / Gate Set

Recommended:

```text
mod_contract_drift
content_pack_manifest
content_pack_security
content_pack_merge
content_pack_reference_integrity
content_pack_determinism
content_pack_save_fallback
content_pack_breaking_change
content_pack_deprecation
```

---

# 26. Failure Injection Matrix

## N47.1 Pack uses `../`
Expected: rejected.

## N47.2 Pack uses reserved `flag_` namespace
Expected: rejected.

## N47.3 Pack schema version too new
Expected: disabled with actionable warning.

## N47.4 Two packs override same ID
Expected: documented deterministic winner/conflict report.

## N47.5 Bad pack partially merges
Expected: impossible; atomic rejection.

## N47.6 Pack removed after save
Expected: save loads gracefully.

## N47.7 Missing pack item remapped to base item
Expected: test fails.

## N47.8 No packs enabled changes base digest
Expected: regression failure.

## N47.9 Same pack set changes digest run-to-run
Expected: determinism failure.

## N47.10 Stable field removed without BREAKING line
Expected: CI fails.

## N47.11 Deprecated field removed early
Expected: CI fails.

## N47.12 Contract doc hand-edited
Expected: generator check fails.

---

# 27. Persistence Matrix

| Data | Authority | Pack may alter? |
|---|---|---:|
| content definitions | supported catalogs | yes |
| tags | tag catalog | yes |
| locale overlay | locale overlay | yes |
| save section keys | SaveSectionRegistry | no |
| save wire shape | SaveWireContract | no |
| stable referenced IDs | catalog identity | add; rename unsupported |
| active pack list | runtime/version report | optionally recorded |
| provenance | derived | no need to persist |
| missing IDs | save domain fallback | preserved where possible |

---

# 28. Determinism Acceptance

Same:

```text
base version
+ active pack IDs/versions
+ deterministic order
+ seed
+ player inputs
```

must yield same:
- effective content digest;
- selected IDs;
- simulation digest.

---

# 29. UI / Version Report Acceptance

Version report shows:

```text
Game version
Data contract
Save contract
Mod contract
Active packs
Rejected packs
```

No need for a full mod manager UI in this plan unless a minimal enable/disable surface already exists.

---

# 30. Non-Goals

Explicitly out of scope:

- Steam Workshop;
- Nexus integration;
- code mods;
- scripting runtime;
- assembly loading;
- scene replacement;
- shader injection;
- input-map extension;
- per-pack save stores;
- encrypted packs;
- dependency resolver beyond simple declared pack dependencies if supported;
- remote pack telemetry.

---

# 31. Recommended Commit Breakdown

```text
47A-1 mod surface inventory
47A-2 contract generator
47A-3 stability classes + identity rules
47A-4 save/determinism contract
47A-5 supported family generation
47A-6 manifest/compat matrix
47A-7 legal/illegal fixture tests
47A-8 AGENTS link + drift gate

47B-1 ContentPackService + discovery
47B-2 deterministic precedence
47B-3 Core overlay merge
47B-4 validation/security/atomic activation
47B-5 effective-content/provenance report
47B-6 loader consolidation
47B-7 save fallback
47B-8 determinism/performance/example docs

47C-1 fixture pack suite
47C-2 CI pack regression gate
47C-3 stable-surface snapshot/diff
47C-4 BREAKING classifier/changelog enforcement
47C-5 deprecation policy/tests
47C-6 version report/support artifacts
47C-7 pack reachability metrics
47C-8 release gate integration
```

---

# 32. Risk Register

## R47.1 Pack loader changes base-game boot

Mitigation:
- no-pack digest control;
- base boot budget;
- shared loader tests.

## R47.2 Save fallback loses pack IDs

Mitigation:
- preserve missing IDs;
- no remap to unrelated base content.

## R47.3 Public contract overpromises

Mitigation:
- generated supported-family list;
- stable only where loader/tests exist.

## R47.4 Conflicts confuse authors

Mitigation:
- deterministic precedence;
- override-chain report.

## R47.5 Deprecated surface lingers forever

Mitigation:
- machine-enforced window.

## R47.6 Path loader becomes security hazard

Mitigation:
- canonicalized paths;
- traversal checks;
- extension/size limits.

## R47.7 Fixture packs drift from real loader

Mitigation:
- same production merge/validation path.

## R47.8 Contract documentation drifts

Mitigation:
- generated doc;
- `--check`.

---

# 33. Acceptance Checklist

## 47A — Contract

- [ ] mod-surface inventory generated
- [ ] stable candidate schemas reverified
- [ ] alternate data path behavior reproduced
- [ ] base boot benchmark captured
- [ ] `generate-mod-contract.py` exists
- [ ] `CONTRACT.md` generated
- [ ] generated header present
- [ ] STABLE/INTERNAL/FORBIDDEN defined
- [ ] layer table generated
- [ ] installation mechanism declared
- [ ] developer override distinguished
- [ ] per-definition overlay semantics declared
- [ ] add semantics
- [ ] modify semantics
- [ ] retire semantics
- [ ] field-level stability generated
- [ ] ID rules generated
- [ ] reserved namespaces generated
- [ ] save compatibility promise
- [ ] missing-ID fallback documented
- [ ] determinism promise
- [ ] supported families list only real schemas
- [ ] unsupported families explicit
- [ ] forbidden surfaces explicit
- [ ] contract version generated
- [ ] compatibility matrix generated
- [ ] VersionReport is source
- [ ] author loop documented
- [ ] rejection guide
- [ ] manifest schema
- [ ] pack ID rules
- [ ] dependency policy explicit
- [ ] conflict policy explicit
- [ ] conflict report
- [ ] contract drift gate
- [ ] validator consistency test
- [ ] legal pack fixture
- [ ] illegal pack fixture
- [ ] AGENTS link
- [ ] metrics recorded

## 47B — Loader

- [ ] ContentPackService created
- [ ] pack root uses canonical user path
- [ ] resolver precedence documented
- [ ] no filesystem-order dependence
- [ ] manifest discovery
- [ ] manifest validation
- [ ] path traversal rejection
- [ ] size limits
- [ ] extension allowlist
- [ ] engine-free merge layer
- [ ] add operation
- [ ] modify operation
- [ ] retire operation
- [ ] retirement safety
- [ ] schema compatibility before merge
- [ ] atomic pack activation
- [ ] full effective-set validation
- [ ] safe boot strategy
- [ ] no silent rejected pack
- [ ] effective-content report
- [ ] VersionReport integration
- [ ] content-utilization integration
- [ ] provenance
- [ ] reserved namespace rejection
- [ ] unsupported family rejection
- [ ] unknown-field behavior explicit
- [ ] loader consolidation
- [ ] fixture fidelity
- [ ] stable merged iteration
- [ ] disabled-pack base digest unchanged
- [ ] enabled-pack deterministic digest
- [ ] pack-enabled save created
- [ ] pack-disabled save loads
- [ ] re-enable pack resolves IDs again
- [ ] save rewrite safe
- [ ] mandatory-content retirement blocked
- [ ] version pinning
- [ ] schema-change override rejection
- [ ] dependency ordering if supported
- [ ] conflict determinism
- [ ] boot cost measured
- [ ] base overhead budget met
- [ ] cache only if needed
- [ ] INSTALL.md
- [ ] working fixture example
- [ ] example content minimal
- [ ] example acceptance passes
- [ ] security tests
- [ ] metrics recorded

## 47C — Compatibility

- [ ] fixture pack matrix created
- [ ] fixtures tiny/single-purpose
- [ ] CI pack gate registered
- [ ] tier split preserves fast loop
- [ ] no-pack control lane
- [ ] stable-surface snapshot generated
- [ ] previous-release comparison
- [ ] breaking classifier
- [ ] non-breaking classifier
- [ ] BREAKING changelog requirement
- [ ] version bump requirement
- [ ] deprecation metadata
- [ ] deprecation warnings
- [ ] early-removal gate
- [ ] removal gate
- [ ] supported-changes release artifact
- [ ] changelog integration
- [ ] version report includes mod fields
- [ ] support artifact export/copy
- [ ] issue template
- [ ] no private data
- [ ] determinism invariant gate
- [ ] save invariant gate
- [ ] reference invariant gate
- [ ] pack reachability metrics
- [ ] privacy stance
- [ ] no analytics backend
- [ ] non-goals documented
- [ ] canon capability claim updated
- [ ] release gate integration
- [ ] pack-enabled export smoke
- [ ] pack-disabled export smoke
- [ ] breaking meta-test
- [ ] contract-version meta-test
- [ ] deprecation meta-test
- [ ] release notes pack section
- [ ] long-term support policy
- [ ] metrics recorded

---

# 34. Ship / No-Ship Gate

**SHIP** only if:

```text
public_mod_contracts == 1
AND contract_generated_from_authorities == true
AND manual_rule_duplication == 0
AND supported_pack_surface_is_data_only == true
AND forbidden_code_execution == true
AND pack_precedence_deterministic == true
AND filesystem_order_dependencies == 0
AND invalid_pack_partial_application == false
AND path_traversal_rejected == true
AND reserved_namespace_collisions_rejected == true
AND effective_content_report_available == true
AND no_pack_base_digest_regression == 0
AND pack_enabled_replay_deterministic == true
AND pack_enabled_save_reload == pass
AND pack_disabled_save_reload == pass
AND missing_pack_ids_remapped_to_unrelated_base == 0
AND stable_surface_diff_tool == pass
AND breaking_change_without_changelog_allowed == false
AND deprecated_surface_removed_early == false
AND contract_drift_gate == pass
AND fixture_pack_suite == pass
AND data_integrity_selftest_with_pack == pass
AND content_utilization_selftest_with_pack == pass
AND release_gate_with_packs == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 35. Implementer Handoff

1. Start from `CatalogPath`; do not invent a second resolver.
2. Generate the public contract from validator/registry/version authorities.
3. Distinguish user content packs from `ASHFALL_DATA` developer override.
4. Keep the supported surface data-only.
5. Support only schemas that are genuinely loaded and accepted by Plan 45.
6. Use per-definition overlay semantics keyed by stable ID.
7. Define deterministic precedence before implementing merge.
8. Reject malformed packs atomically.
9. Record provenance and effective content at boot.
10. Make save-without-pack recovery a first-class requirement.
11. Never remap missing IDs to unrelated base content.
12. Keep merged iteration deterministic.
13. Consolidate loader boilerplate only where it improves one-door validation.
14. Measure no-pack boot regression.
15. Ship fixture packs as regression inputs.
16. Diff the stable public surface every release.
17. Require `BREAKING:` and version-policy updates for incompatible changes.
18. Enforce deprecation windows mechanically.
19. Expose mod-relevant versions in VersionReport.
20. Keep non-goals explicit so “mod support” cannot be misread as code-mod support.
21. Close with release smoke using packs enabled and disabled.

---

# 36. Final Outcome

When this plan is complete, ASHFALL stops being accidentally moddable and becomes deliberately extensible.

A content-pack author can read one generated contract and know which schemas are stable, which identifiers are public, which tags and locale overlays are supported, which operations are legal, and which boundaries are forbidden. That document is not a promise maintained by memory; it is generated from the validator and version authorities that actually enforce the game.

A pack then enters through one deterministic loader path. Base data loads first. Official and user overlays merge by stable definition ID. Every pack is schema-checked, reference-checked, namespace-checked, range-checked, and validated as an atomic unit before the game uses it. The effective content report shows exactly which pack added or replaced each definition.

Saves remain protected. A campaign created with pack content can still be opened when that pack is absent without crashing or silently remapping content to unrelated base definitions. Re-enabling the pack restores resolvable IDs where the save retained them.

Determinism remains protected too. Installing the pack loader does not change the base game when no packs are active. The same active pack set yields the same merged order and the same seeded campaign behavior.

Finally, compatibility becomes a release property. Fixture packs run in CI. Stable-surface changes are diffed automatically. Deprecated fields cannot disappear early. Breaking changes require explicit release notes and version-policy action.

The result is not a code-mod ecosystem or workshop integration.

It is a narrow, reliable, test-backed content-pack contract that ASHFALL can honestly support without sacrificing the save, determinism, integrity, and continuity guarantees built during the previous waves.
