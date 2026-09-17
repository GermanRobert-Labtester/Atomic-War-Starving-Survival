# C2 — Flagship Integration Plan [21]: Version Contracts, Tagged Releases, Artifact Provenance, and Save-Safe Hotfixes

> **Deliverable:** `C2_planintegration[21].md`
> **Source scope:** Plan 48 — *Release Craft: Versions, Tags, Changelog, and the Hotfix Path*
> **Wave:** Continuity Wave 7 — *Content on Rails & the Measurement Layer*
> **Primary objective:** turn ASHFALL’s existing version-report, save-schema, data-schema, CI, and export machinery into an explicit release practice: one compatibility policy, one automated version gate, one scripted tagged release path, one machine-readable release manifest, and one rehearsed hotfix/rollback procedure that preserves campaign saves.
> **Required execution order:** **29A → 39A → 48A → 48B → 48C**
> **Hard gate:** do not cut or publish a release tag until 48A compatibility tests and the 39A release gate are green.
> **Dependencies:** Plan 29A green record/doc truth, Plan 39A release gate, Plan 46A/46B balance and metrics reporting, Plan 47A/47C data/mod compatibility and breaking-change detection.
> **Scope discipline:** no force-push past a release tag, no save-schema migration in a hotfix, no hand-edited generated reports, no version claim without a fixture-backed compatibility test, no release artifact that has not been booted/tested, and no player-facing compatibility promise that is not enforced by a gate.

---

# 0. Executive Intent

ASHFALL already has much of the engineering needed for reliable releases:

- project version metadata,
- version-report composition,
- per-store save schema versions,
- data schema versions,
- save migrations,
- export presets,
- CI gates,
- content-utilization artifacts,
- performance reports,
- balance evidence,
- release-gate work from Plan 39.

What is missing is the product/repository contract that turns these into a release.

Current defects include:

```text
zero git tags
no changelog
no versioning policy
no compatibility matrix
no release process
no hotfix process
no durable release archive
no rule connecting version changes to save/data migrations
```

The desired architecture is:

```text
game version
+ data compatibility policy
+ save compatibility policy
        │
        ▼
version gate
        │
        ▼
release candidate
        │
        ├─ release gate
        ├─ artifact export
        ├─ artifact boot/load smoke
        ├─ generated changelog
        ├─ compatibility report
        └─ balance/metrics delta
        │
        ▼
signed release manifest
        │
        ▼
git tag + archived release record
        │
        ▼
hotfix branch from tag
        │
        ├─ fixture compatibility
        ├─ artifact smoke
        ├─ patch-only version
        └─ back-merge to main
```

The flagship outcome is:

> **A release becomes a verifiable event: one version policy, one tagged commit, one tested artifact set, one generated changelog, one compatibility matrix, one gate report, and one rehearsed recovery path.**

---

# 1. Source Diagnosis

The source establishes:

- there are currently no release tags,
- the game version is a bare `1.0.0` string,
- CLI can report `unknown`,
- no changelog/release process document exists,
- `VersionReport` already reports game/data/save versions,
- CI gates exist but are not tied to a release ritual,
- exported artifacts are not yet the proven shipped object,
- save compatibility windows are unspecified,
- repository instructions still contain an unrelated `bit lane/snap/export` workflow,
- release/hotfix skill guidance exists without repository automation,
- content/balance drift lacks per-release chronology.

The correct repair is:

```text
policy
→ tests
→ automation
→ release artifact
→ tag
→ archive
```

not:

```text
write release notes manually after shipping
```

---

# 2. Program-Level Success Criteria

C2[21] closes only if:

1. Game/data/save versions have explicit policy.
2. `project.godot` and assembly/product version cannot drift.
3. CLI never reports `unknown` in a valid release build.
4. Accepted data schema ranges are encoded and tested.
5. Supported save-version windows are explicit and fixture-backed.
6. Every supported historical save fixture loads on every push.
7. Version changes without matching notes/migration evidence fail CI.
8. Actual Git workflow replaces obsolete `bit` instructions.
9. First real known-good release tag exists.
10. Changelog is generated from repository history plus release-critical diffs.
11. Release artifacts are self-describing and include their report.
12. Release preparation is one command/check path.
13. Exported/uploaded artifacts are booted and save/load tested post-build.
14. Release reports are archived permanently.
15. Hotfix classes are explicit.
16. A hotfix cannot introduce a save migration.
17. Hotfix fixture checks use the target release line’s fixtures.
18. Rollback is rehearsed.
19. Support triage bundle is redaction-safe.
20. Every incident produces a “which gate would have caught this?” postmortem answer.

---

# 3. Architectural Invariants

## 3.1 Three version axes, one policy

ASHFALL versions:

```text
game
data
save
```

They are distinct but governed together.

## 3.2 Game version has one source of truth

`project.godot` and assembly metadata must agree automatically.

## 3.3 Data compatibility is a range

A build states:

```text
accepted schema range
```

not merely “current schema”.

## 3.4 Save compatibility is fixture-backed

Prose support promises are invalid without load/migration tests.

## 3.5 Version changes are reviewed behavior changes

A store/data schema bump requires:

- migration/compatibility evidence,
- changelog/release note.

## 3.6 Tag identifies immutable release intent

No force-push/rewrite past tag.

## 3.7 Artifact is the thing that ships

Post-release verification operates on uploaded/exported artifacts, not source tree.

## 3.8 Hotfix is patch-only

If save migration is required, classify as normal release, not hotfix.

## 3.9 Changelog is evidence-linked

Balance, metrics, schema, and mod-contract changes cite generated sources.

## 3.10 Release process is scripted

No human-memory checklist is authoritative.

---

# 4. Dependency Graph

```text
29A green gates/docs
       │
       ▼
39A release gate
       │
       ▼
48A version/compatibility policy
       │
       ▼
48B release event automation
       │
       ▼
48C hotfix/rollback rehearsal

46A/46B ─────────► balance + funnel report lines
47A/47C ─────────► mod/data breaking-change reporting
26B/39A ─────────► exported artifact validation
```

Required order:

```text
29A → 39A → 48A → 48B → 48C
```

---

# 5. Baseline Capture

Record:

- current `project.godot` version,
- current assembly/product version,
- CLI version fallback path,
- save-store `CurrentVersion` values,
- save envelope manifest version,
- catalog schema versions,
- current CI gate count,
- current export scripts/workflows,
- current git tags,
- current dirty paths/branch policy docs,
- current release/hotfix scripts,
- current archived fixtures.

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/verify-fast.sh
```

Do not start tagging until baseline is green.

---

# 6. Workstream 48A — One Versioning Scheme for Game, Data, and Save

## Goal

Make compatibility a computed, tested answer.

---

# 7. 48A Phase A — `docs/release/VERSIONING.md`

Create the policy document.

Define:

```text
game version
data schema version
save schema version
save envelope manifest version
mod-contract version if applicable
```

For each:

- owner,
- source,
- compatibility rules,
- bump criteria,
- test/gate.

---

# 8. 48A Phase B — Semantic Versioning Policy

Adopt explicit project-specific semver mapping.

Recommended source rule:

## Major

- save compatibility break,
- mod contract break,
- unsupported breaking data contract,
- major engine/product compatibility break.

## Minor

- new systems/content/reachability,
- additive data schemas,
- backward-compatible save sections,
- large player-facing feature changes.

## Patch

- bug fixes,
- tuning corrections,
- content fixes that preserve compatibility,
- hotfixes.

Document exceptions.

---

# 9. 48A Phase C — Single Game-Version Authority

Canonical game version lives in:

```text
project.godot config/version
```

or one generated source feeding both project and assembly metadata.

Whichever architecture is chosen, enforce equality with:

```text
Directory.Build.props / assembly version
```

No manual dual edits.

---

# 10. 48A Phase D — Ban `unknown`

Release/build validation must fail if:

```text
HostCli
→ version = unknown
```

Development fallback may still be useful in isolated contexts, but a release candidate must carry a valid version.

Add test.

---

# 11. 48A Phase E — Data Compatibility Ranges

For every versioned catalog family define:

```text
minimum supported schema
maximum supported schema
current authored schema
```

Loading behavior:

- too old and unsupported → explicit failure,
- supported old → migrate/accept,
- supported current → load,
- newer than build → explicit rejection naming file/schema.

---

# 12. 48A Phase F — Data Compatibility Matrix

Generate:

```text
docs/release/DATA_COMPATIBILITY.md
```

or include in `VERSIONING.md`.

Columns:

| Catalog family | Min | Current | Max accepted | Migration | Notes |
|---|---:|---:|---:|---|---|

Generated from code/data contracts where possible.

---

# 13. 48A Phase G — Save Support Window

For every registered save store define:

```text
CurrentVersion
MinimumReadableVersion
MigratableVersions
RefusedVersions
```

No implicit support.

---

# 14. 48A Phase H — Save Compatibility Matrix

Generate table:

| Save store | Current | Reads | Migrates | Refuses |
|---|---:|---|---|---|

Tie directly to codec registry/tests.

---

# 15. 48A Phase I — Historical Fixture Corpus

Commit one real fixture per supported historical version.

Structure:

```text
tests/fixtures/saves/
  vX/
  vY/
```

Each fixture includes:

- source version,
- expected migrated checksum/state digest,
- minimal provenance note.

---

# 16. 48A Phase J — Migration Fixture Gate

Every push:

```text
for each supported fixture
→ load
→ migrate if needed
→ verify expected state/checksum
→ save current format
```

Fail any unsupported regression.

---

# 17. 48A Phase K — Version Bump Script

Create release helper that:

- bumps product version,
- updates generated assembly version,
- validates schema/version deltas,
- regenerates version-report expectations,
- refuses manual contract-test edits that bypass policy.

---

# 18. 48A Phase L — `version-gate.sh`

Create:

```bash
scripts/ci/version-gate.sh
```

Fail when:

- project/assembly version disagree,
- version changed without changelog entry,
- save store version changed without fixture/migration,
- catalog schema changed without release note,
- CLI reports unknown,
- compatibility matrix stale.

---

# 19. 48A Phase M — Gate Failure Proof

Deliberately create test fixtures for:

- version mismatch,
- missing changelog,
- store version bump without migration,
- catalog schema bump without note.

Assert gate fails.

---

# 20. 48A Phase N — Publish Mod Compatibility

Feed Plan 47 compatibility contract from same source.

No separate hand-maintained mod compatibility chart.

---

# 21. 48A Phase O — Rewrite AGENTS VCS Instructions

Remove obsolete:

```text
bit lane
bit snap
bit export
```

Replace with actual repo policy:

- Git branches,
- commit discipline,
- release tags,
- one system per commit,
- no force-push past tag,
- release/hotfix branch rules.

Do not alter unrelated architecture invariants.

---

# 22. 48A Phase P — Regenerate Rulebook Copies

Use Plan 29A mechanism.

Prove no stale copy retains `bit` instructions.

---

# 23. 48A Tests

- game version equality,
- no unknown release version,
- accepted data range,
- newer-data refusal,
- supported save fixture loads,
- unsupported save version refuses clearly,
- migration writes current version,
- version gate failure fixtures,
- rulebook drift check.

---

# 24. 48A Definition of Done

- [ ] VERSIONING.md,
- [ ] semver rules,
- [ ] one game-version authority,
- [ ] no unknown release version,
- [ ] data accepted ranges,
- [ ] generated data compatibility matrix,
- [ ] save support window,
- [ ] generated save compatibility matrix,
- [ ] historical save fixtures,
- [ ] migration fixture gate,
- [ ] bump automation,
- [ ] `version-gate.sh`,
- [ ] gate failure proof,
- [ ] mod compatibility projection,
- [ ] AGENTS Git workflow corrected,
- [ ] rulebook copies regenerated.

---

# 25. Workstream 48B — Releases as Events

## Goal

A release is one scripted, tagged, evidence-backed event producing tested artifacts and a durable manifest.

---

# 26. 48B Phase A — Release Branch Model

Document:

```text
main
release/x.y
hotfix/*
```

Rules:

- `main` continues development,
- `release/x.y` stabilization only,
- hotfix branches originate from release tag,
- fixes back-merge to main,
- no history rewrite past tag.

---

# 27. 48B Phase B — First Known-Good Tag

Once 48A/39A pass:

- select current policy-compliant pre-1.0 or current semantic version,
- prepare release candidate,
- create annotated tag only after successful check.

Do not invent tag if version policy implies another value.

---

# 28. 48B Phase C — `CHANGELOG.md`

Create generated/hybrid changelog.

Sections derived from conventional commit prefixes:

```text
Added
Changed
Fixed
Data/Schema
Save Compatibility
Mod Contract
Balance
Known Issues
```

Use actual project style.

---

# 29. 48B Phase D — Commit-to-Plan Traceability

Where commits include:

```text
Plan NN
Task NX
```

preserve that reference in generated release notes/report.

This improves archaeology.

---

# 30. 48B Phase E — Release-Critical Diffs

Every release notes:

- save schema changes,
- data schema changes,
- mod contract changes,
- migrations,
- compatibility-window changes.

Generate from 48A/47C sources.

---

# 31. 48B Phase F — Balance and Funnel Delta

Use Plan 46 outputs.

Every release report should include:

```text
balance decision IDs
key measured deltas
synthetic funnel summary
```

Do not handtype numbers that can drift.

---

# 32. 48B Phase G — Artifact Layout

Release bundle:

```text
binary
PCK/resources
deployed data
report/
  version-report
  gate-report
  compatibility-matrix
  content-utilization snapshot
  balance delta
  metrics/funnel summary
  artifact hashes
```

Keep platform-specific layouts explicit.

---

# 33. 48B Phase H — Self-Describing Artifact

Artifact must answer:

```text
game version
commit SHA
build timestamp
data compatibility
save compatibility
platform
gate status
```

without repository access.

---

# 34. 48B Phase I — Signed Gate Report

Every release includes a machine-readable gate report.

“Signed” means integrity-authenticated according to current release infrastructure, not necessarily public-key signing if that is not yet available.

At minimum:

- immutable hash,
- exact commit/tag,
- complete gate statuses.

---

# 35. 48B Phase J — `prepare-release.sh`

Create:

```bash
scripts/release/prepare-release.sh
```

Modes:

```text
--check
--dry-run
--prepare
```

Responsibilities:

- verify clean tree,
- verify correct branch,
- verify version/changelog,
- run version gate,
- run release gate,
- export artifacts,
- boot artifacts,
- collect reports,
- prepare tag metadata.

Avoid auto-pushing unless repository policy explicitly wants it.

---

# 36. 48B Phase K — Candidate Preconditions

`--check` fails on:

- dirty tree,
- invalid branch,
- red gates,
- missing changelog section,
- version mismatch,
- missing compatibility fixtures,
- unbooted artifact,
- stale generated report.

---

# 37. 48B Phase L — Export Workflow Alignment

Update CI build workflow to invoke canonical export scripts.

Do not keep raw CI-only `godot --export-release` path if Plan 26B canonical script exists.

One export path.

---

# 38. 48B Phase M — Artifact Boot/Load Verification

For each release artifact:

```text
boot
→ resolve packaged data
→ load fixture save
→ advance/smoke
→ save
→ reload
→ exit 0
```

Use Plan 39A.

---

# 39. 48B Phase N — Post-Upload Verification

After artifact is copied/uploaded to final distribution staging:

- download/use the staged artifact,
- re-run boot/load/smoke.

This catches packaging/upload corruption.

Do not claim shipped artifact tested if only workspace binary was tested.

---

# 40. 48B Phase O — Player-Facing Patch Notes Template

Create:

```text
docs/release/TEMPLATE.md
```

Player-facing fields:

- highlights,
- fixes,
- compatibility,
- known issues,
- save guidance,
- balance changes.

No internal class names unless useful.

---

# 41. 48B Phase P — Known-Issue Source

Generate known issues from current authority/issue registry.

Do not maintain a separate stale release-known-issues list.

---

# 42. 48B Phase Q — Release Archive

Persist:

```text
docs/archive/releases/<version>/
```

with:

- report,
- compatibility matrix,
- gate summary,
- artifact hashes,
- notes,
- balance delta.

This enables release-level bisection.

---

# 43. 48B Phase R — Dry-Run Release

Execute full dry run before first real release.

Capture:

- command output,
- report path,
- artifact hashes,
- failures encountered.

Fix process until repeatable.

---

# 44. 48B Phase S — Broken-Candidate Test

Test `prepare-release.sh --check` against:

- dirty tree,
- missing changelog,
- invalid version,
- failed gate,
- missing boot smoke.

Assert non-zero.

---

# 45. 48B Definition of Done

- [ ] branch/tag model,
- [ ] first known-good tag plan/cut,
- [ ] CHANGELOG.md,
- [ ] commit→plan traceability,
- [ ] release-critical diffs,
- [ ] balance/funnel delta,
- [ ] complete artifact layout,
- [ ] self-describing artifact,
- [ ] gate report,
- [ ] `prepare-release.sh`,
- [ ] strict preconditions,
- [ ] canonical export path,
- [ ] artifact boot/load verification,
- [ ] post-upload verification,
- [ ] patch-note template,
- [ ] generated known issues,
- [ ] release archive,
- [ ] full dry-run release,
- [ ] broken-candidate tests.

---

# 46. Workstream 48C — Hotfixes, Rollbacks, and Save Safety

## Goal

Rehearse the emergency path before it is needed.

---

# 47. 48C Phase A — `docs/release/HOTFIX.md`

Write the playbook first.

Fix classes:

```text
PATCH
DATA
SAVE_AFFECTING
BREAKING
```

Only valid hotfix classes:

```text
PATCH
DATA (if compatibility-safe)
```

A required save migration means:

```text
not a hotfix
```

---

# 48. 48C Phase B — PR/Branch Classification

Require hotfix PR metadata/title to state classification.

Automation uses classification to select gates.

---

# 49. 48C Phase C — Hotfix Branch from Tag

Flow:

```text
git checkout tag
→ create hotfix/*
→ cherry-pick/minimal fix
→ run release-line gates
→ export
→ artifact smoke
→ publish patch tag
→ back-merge to main
```

Document exact commands in script/docs.

---

# 50. 48C Phase D — `scripts/release/hotfix.sh`

Support:

```text
--check
--dry-run
--prepare
```

Require:

- source tag,
- patch version,
- change class,
- clean tree.

---

# 51. 48C Phase E — Save-Compatibility Rule

Hotfix must load all release-line fixture saves with no migration.

Assert:

```text
input fixture checksum/state digest
→ candidate build
→ identical semantic state
```

Saving may naturally write canonical current format only if current patch version uses same schema.

No schema bump.

---

# 52. 48C Phase F — Save-Schema Diff Gate

Diff candidate against source tag.

Fail hotfix if:

- any `CurrentVersion` changed,
- save envelope manifest changed,
- migration added/removed,
- persisted wire shape changed incompatibly.

---

# 53. 48C Phase G — Data-Only Hotfix Policy

Decide whether supported.

If yes:

- allowed catalog families,
- schema must remain accepted,
- integrity tests,
- content utilization,
- effective content report,
- package/signature expectations.

If no:

- explicitly state binary rebuild required.

---

# 54. 48C Phase H — Emergency Content Quarantine

Provide config/exemption mechanism only if existing architecture safely supports it.

Requirements:

- disable broken content family,
- preserve dependent systems or fail clearly,
- record release note,
- no hidden deletion of player data.

---

# 55. 48C Phase I — Rollback Playbook

Rehearse:

```text
release N+1 problematic
→ restore/redeploy previous artifact
→ verify previous artifact can still read campaign saves produced under compatible patch
```

If previous version cannot read saves written by newer version, rollback policy must state limits.

Do not promise impossible rollback.

---

# 56. 48C Phase J — Backup/Envelope Validation

Use:

- SaveChecksum,
- SaveEnvelopeHelper,
- `.bak` rotation.

Rollback tests should include:

- current primary,
- backup,
- hotfix save/reload.

---

# 57. 48C Phase K — Player Communication

Hotfix notes must state:

- what broke,
- whether saves are affected,
- whether rollback is safe,
- whether backup recovery is needed.

No vague “stability fixes” if save risk existed.

---

# 58. 48C Phase L — Support Triage Bundle

Create command producing sanitized bundle:

```text
version report
effective content report
day record
save header
gate/build identifier
```

No full save unless explicitly user-chosen.
Apply Plan 31C/46B redaction rules.

---

# 59. 48C Phase M — Triage Bundle Redaction Gate

Assert bundle contains no:

- username,
- absolute path,
- email,
- arbitrary prose beyond approved logs,
- local telemetry raw data unless explicitly requested.

---

# 60. 48C Phase N — Release-Line Backport CI

Workflow runs hotfix branch against:

- source release fixtures,
- source release compatibility matrix,
- source release artifact assumptions.

Do not substitute `main` fixtures.

---

# 61. 48C Phase O — Patch-Only Version Discipline

Hotfix version increment:

```text
x.y.z → x.y.(z+1)
```

No minor/major bump through hotfix path.

---

# 62. 48C Phase P — Changelog Integration

Hotfix changelog generated through 48B pipeline.

Required sections:

- fix,
- compatibility,
- save impact,
- known issue/resolution.

---

# 63. 48C Phase Q — Postmortem Template

Create:

```text
docs/release/POSTMORTEM_TEMPLATE.md
```

Required fields:

```text
incident
affected versions
player impact
save impact
root cause
why gates missed it
which gate would have caught it
new/changed gate
rollback result
```

---

# 64. 48C Phase R — Gate-Follows-Incident Rule

If no existing gate would have caught incident:

```text
add one before closing incident
```

This is mandatory process debt.

---

# 65. 48C Phase S — Full Hotfix Rehearsal

Before first real incident:

- cut from first release tag,
- make harmless test patch,
- run hotfix script,
- run fixture compatibility,
- export,
- post-upload smoke,
- rollback rehearsal,
- back-merge.

Record in Wave ledger.

---

# 66. 48C Tests

- invalid hotfix class,
- schema version change rejected,
- fixture save unchanged,
- data-only allowed/rejected policy,
- release-line fixture use,
- support bundle redaction,
- rollback rehearsal,
- patch version enforcement,
- generated changelog.

---

# 67. 48C Definition of Done

- [ ] HOTFIX.md,
- [ ] fix classification,
- [ ] branch-from-tag flow,
- [ ] `hotfix.sh`,
- [ ] no save migration allowed,
- [ ] save-schema diff gate,
- [ ] release fixtures load unchanged,
- [ ] data-only hotfix policy decided,
- [ ] quarantine procedure,
- [ ] rollback playbook,
- [ ] backup/envelope validation,
- [ ] player communication template,
- [ ] support triage bundle,
- [ ] redaction gate,
- [ ] release-line backport CI,
- [ ] patch-only version rule,
- [ ] generated hotfix changelog,
- [ ] postmortem template,
- [ ] incident→gate rule,
- [ ] full rehearsal recorded.

---

# 68. Integrated Release Pipeline

```text
green main
   │
   ▼
version policy
   │
   ▼
compatibility fixtures
   │
   ▼
version gate
   │
   ▼
release branch
   │
   ▼
prepare-release --check
   │
   ├─ release gate
   ├─ export
   ├─ artifact smoke
   ├─ changelog
   ├─ compatibility report
   ├─ balance/funnel delta
   └─ known issues
   │
   ▼
tag
   │
   ▼
upload/stage
   │
   ▼
post-upload verification
   │
   ▼
release archive
```

---

# 69. Compatibility Decision Contract

Question:

```text
Can build X load save Y with data Z?
```

Answer must come from:

- version policy,
- compatibility matrix,
- fixture tests.

Never from tribal knowledge.

---

# 70. Game Version Contract

Release game version is:

- valid semver,
- present in project config,
- present in assembly metadata,
- included in report/artifact,
- never `unknown`.

---

# 71. Data Version Contract

Each catalog family declares:

```text
min accepted
current
max accepted
```

Newer schema rejection must name offending file/version.

---

# 72. Save Version Contract

Each save store declares:

```text
current
supported historical
migration paths
refused historical
```

Every supported version has fixture evidence.

---

# 73. Changelog Contract

Every release note entry that changes:

- save schema,
- data schema,
- mod contract,
- balance target

must cite/generated evidence.

---

# 74. Release Manifest Contract

Machine-readable release manifest should include:

```text
version
tag
commit
platform
artifact hashes
game/data/save compatibility
gate report hash
content-utilization snapshot
balance decision IDs
funnel report summary
known issues hash
```

---

# 75. Tag Integrity Contract

Tag creation happens only after:

```text
prepare-release --check
```

passes.

No tag rewrite.

---

# 76. Artifact Verification Contract

The final staged/downloaded artifact must be the object tested.

Workspace build alone is not sufficient.

---

# 77. Hotfix Compatibility Contract

Hotfix:

```text
same save schema
same migration contract
patch version only
release-line fixtures pass
```

If not, use normal release process.

---

# 78. Rollback Contract

Rollback support must state:

- what prior artifact can read,
- what newer saves it can/cannot read,
- whether backup recovery is required.

No blanket promise.

---

# 79. Support Bundle Contract

Support bundle is:

- minimal,
- sanitized,
- reproducible,
- generated by command.

No manual “zip your user folder.”

---

# 80. Gate Self-Proof Contract

Critical release/version/hotfix gates must each have a deliberate failure fixture.

---

# 81. Documentation Truth Contract

Release docs and AGENTS instructions must reflect actual Git/Godot workflow.

No obsolete VCS vocabulary survives.

---

# 82. Failure Modes

## Game version says 1.0.0 but assembly says another version

Version gate fails.

## CLI prints unknown

Release blocked.

## Save store version bump has no fixture

Version gate fails.

## New data schema accepted silently by old build

Compatibility gate fails/rejects explicitly.

## Release notes omit schema change

Version gate fails.

## Dirty tree enters prepare-release

Fail.

## CI artifact not booted

Release check fails.

## Uploaded artifact differs from tested artifact

Post-upload smoke/hashes fail.

## Hotfix changes save schema

Hotfix gate rejects.

## Backport CI uses main fixtures

Fix workflow to pin release-line fixtures.

## Rollback claim is impossible

Document actual support window.

---

# 83. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| first release exposes red gates | Medium | Medium | complete 29A/39A first |
| version sources drift | Medium | High | equality gate |
| fixtures become stale | Medium | High | per-push migration tests |
| changelog generation misses intent | Medium | Medium | conventional commits + ADR sources |
| release script becomes too magical | Medium | Medium | check/dry-run modes |
| hotfix invented during incident | High | Critical | rehearsal |
| rollback impossible due to newer saves | Medium | High | explicit support policy |
| data-only hotfix violates schema | Medium | High | strict allowed-family gate |
| support bundle leaks paths | Medium | High | redaction test |
| tag created before evidence | Low–Med | High | prepare-release gate |

---

# 84. Commit Strategy

## C2[21].1 — versioning baseline + policy

## C2[21].2 — game-version single source + no-unknown gate

## C2[21].3 — data compatibility ranges/matrix

## C2[21].4 — save support windows + fixtures

## C2[21].5 — version bump automation

## C2[21].6 — `version-gate.sh` + failure proofs

## C2[21].7 — AGENTS Git workflow rewrite + rulebook regeneration

### Gate: 48A complete

## C2[21].8 — branch/tag model + CHANGELOG

## C2[21].9 — release-critical diff generation

## C2[21].10 — release manifest/artifact layout

## C2[21].11 — `prepare-release.sh`

## C2[21].12 — canonical export workflow

## C2[21].13 — post-upload artifact verification

## C2[21].14 — patch-note template + known issues

## C2[21].15 — release archive + dry run

### Gate: 48B complete

## C2[21].16 — HOTFIX policy + script

## C2[21].17 — schema-diff/historical fixture gate

## C2[21].18 — data-only/quarantine policy

## C2[21].19 — rollback + support bundle

## C2[21].20 — release-line backport CI

## C2[21].21 — postmortem/gate feedback loop

## C2[21].22 — full hotfix rehearsal

### Gate: 48C complete

## C2[21].23 — Wave‑7 release-craft closure

---

# 85. Verification Checklist

Run:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
bash scripts/ci/version-gate.sh
bash scripts/ci/release-gate.sh
bash scripts/ci/export-smoke-boot.sh
bash scripts/release/prepare-release.sh --check --dry-run
bash scripts/ci/verify-fast.sh
```

Also verify:

```text
every supported fixture save loads
fixture migration checksum/state matches
intentional version/changelog drift fails
hotfix schema-change fixture fails
post-upload artifact smoke
git tag --points-at HEAD after real cut
```

---

# 86. Flagship Definition of Done

## 48A — Versioning

- [ ] three-axis policy,
- [ ] semver mapping,
- [ ] one game version source,
- [ ] no unknown release version,
- [ ] data ranges,
- [ ] save support window,
- [ ] compatibility matrices,
- [ ] historical fixtures,
- [ ] fixture gate,
- [ ] bump automation,
- [ ] version gate,
- [ ] failure proofs,
- [ ] mod compatibility projection,
- [ ] AGENTS Git policy fixed,
- [ ] rulebooks regenerated.

## 48B — Release Event

- [ ] branch/tag model,
- [ ] first known-good tag,
- [ ] changelog,
- [ ] release-critical diffs,
- [ ] balance/funnel lines,
- [ ] complete self-describing artifacts,
- [ ] gate report,
- [ ] prepare-release script,
- [ ] strict check mode,
- [ ] canonical export path,
- [ ] artifact boot/load,
- [ ] post-upload verification,
- [ ] player patch-note template,
- [ ] generated known issues,
- [ ] archive,
- [ ] dry-run release.

## 48C — Hotfix

- [ ] hotfix classes,
- [ ] branch-from-tag process,
- [ ] hotfix script,
- [ ] save migrations forbidden,
- [ ] schema diff gate,
- [ ] release fixtures stable,
- [ ] data-only policy,
- [ ] quarantine policy,
- [ ] rollback rehearsal,
- [ ] backup/envelope validation,
- [ ] support bundle,
- [ ] redaction,
- [ ] release-line backport CI,
- [ ] patch-only versioning,
- [ ] hotfix changelog,
- [ ] postmortem template,
- [ ] gate-follow-up rule,
- [ ] full rehearsal.

## Global

- [ ] no hand-edited generated report,
- [ ] no tag without compatibility evidence,
- [ ] no force-push past tag,
- [ ] no unsupported player-facing promise,
- [ ] no artifact released without boot/load proof,
- [ ] full verification green.

---

# 87. Closure Report Template

```markdown
## C2[21] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:
- First release tag:

### 48A — Versioning
- Game version:
- Assembly version:
- Unknown-version paths:
- Data ranges:
- Save support window:
- Historical fixtures:
- Migration fixtures:
- Version gate:
- Failure proof:
- AGENTS VCS rewrite:
- Result:

### 48B — Release
- Release branch:
- Changelog:
- Release-critical diffs:
- Balance/funnel delta:
- Artifact platforms:
- Artifact hashes:
- Gate report:
- prepare-release dry run:
- Export smoke:
- Post-upload smoke:
- Release archive:
- Tag:
- Result:

### 48C — Hotfix
- Hotfix classification:
- Source release tag:
- Candidate patch version:
- Schema diff:
- Fixture checksum stability:
- Data-only policy:
- Quarantine:
- Rollback:
- Support bundle:
- Redaction:
- Backport CI:
- Postmortem:
- Rehearsal:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Bridge:
- Version gate:
- Release gate:
- Export smoke:
- Prepare release:
- Fixture compatibility:
- Verify fast:

### Final Metrics
- RELEASE_TAGS:
- VERSION_SOURCE_MISMATCHES:
- UNKNOWN_VERSION_PATHS:
- SUPPORTED_SAVE_FIXTURES:
- UNSUPPORTED_SCHEMA_DRIFT:
- RELEASE_ARTIFACTS_BOOTED:
- POST_UPLOAD_SMOKE_FAILURES:
- HOTFIX_SCHEMA_CHANGES_REJECTED:
- ARCHIVED_RELEASE_REPORTS:

### Remaining Debt
- Versioning:
- Release:
- Artifact:
- Hotfix:
- Support:
```

---

# 88. Final Execution Directive

Execute Plan 48 as a release-practice hardening plan.

The critical sequence is:

```text
make compatibility policy explicit
→ pin it with fixtures
→ automate version drift detection
→ replace obsolete VCS instructions
→ produce one scripted release candidate
→ boot/test the exact shipped artifacts
→ tag only after proof
→ archive the release record
→ rehearse hotfix and rollback from the tag
```

Do not tag before compatibility tests exist.

Do not claim a save-support window without real historical fixtures.

Do not let a hotfix change save schema.

Do not test only the workspace build.

Do not keep release knowledge in skills or human memory when the repository can enforce it.

The strongest versioning rule is:

> **“Can build X load save Y with data Z?” must be answered by the compatibility matrix and fixture tests, not by archaeology.**

The strongest release rule is:

> **A release is a tagged commit plus a tested artifact set, generated notes, compatibility evidence, and a complete gate report.**

The strongest hotfix rule is:

> **A hotfix is patch-only, save-schema-stable, tested against the release line’s fixtures, and rehearsed before an incident makes improvisation expensive.**

The flagship acceptance scenario is:

> **Cut a dry-run release from a green candidate, generate its changelog/report/artifacts, boot and load the staged artifact, then branch from that tag, apply a harmless patch-level hotfix, prove every release fixture loads unchanged, rehearse rollback, and back-merge — with every step driven by scripts and every compatibility claim backed by a gate.**
