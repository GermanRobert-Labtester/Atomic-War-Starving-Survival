# ASHFALL — Latest Deep Code Audit & 50-Task Planned Implementation Plan

**Audit date:** 2026-08-26
**Current `main` snapshot inspected:** `236be96e6c05fd7241cd07a60158942912af9c6e`
**Open PR inspected:** PR #29, head `0b0cc2d0cd2cbc531811534d1fcf1a43735cfc09`
**Primary engine:** Godot 4.7.1 .NET/C#
**Core authority:** `Assets/Ashfall.Core/`
**Data authority:** `Assets/StreamingAssets/Data/`
**Active host:** `src/` + `scenes/`
**Legacy rule for this plan:** Unity is retired. Do not invoke Unity, do not add Unity code, and do not create new gameplay ownership under `Assets/_Game/`.

> This is an implementation plan, not an implementation. The current repository was inspected through the connected GitHub source at the commit above. The execution environment used to create this report could not clone GitHub directly, so current build/test commands were **not rerun here**. Historical repository evidence reports green suites at prior checkpoints, but every task below contains its own fresh verification gate.

---

## 1. Executive conclusion

The largest current problem is **not missing gameplay logic**. ASHFALL now contains another substantial tranche of Core implementation, but integration has not kept pace.

The most important new finding is the **Batch 3 integration gap**:

- Twelve new engine-agnostic Core systems were added with xUnit tests: Apprenticeship, Archive Desk, Autopsy, Contractor Roster, Decontamination, Equipment Condition, Kitchen Nutrition, Library Study, Mental Health Crisis, Shelter Schedule, Shelter Thermal, and Sump Flooding.
- Ten of those now have Godot `HostSession` wrappers on `main`.
- Two still appear to have no host wrapper: **Apprenticeship** and **Shelter Thermal**.
- Indexed source search did not find those Batch 3 host wrappers composed from `src/Main.cs` or another obvious active composition root. Treat them as **unwired until a runtime registration proves otherwise**.
- At least four new Batch 3 save stores contain a real integrity regression: they compute a checksum during save but only require a non-empty checksum during load. They do **not** recompute and compare it before trusting state.
- The established `WorldSaveStore` demonstrates the correct pattern and should be the baseline.
- The canonical asset gate still tolerates a failed Godot import and proceeds.
- `TimeSystem` still has an event-boundary bug when a step crosses multiple integer hours, plus a restore boundary that allows an hour accumulator of exactly 24.
- Generated visual manifest tests and runtime context data still encode stale phase assumptions.
- The Godot project still compiles the whole `Assets/_Game/**/*.cs` legacy tree and the repository still has a Unity compatibility build workflow, which is incompatible with the current Godot-only project direction.
- The 2D spatial layer remains only partially state-driven: Holdfast room hotspots are hard-coded; survivor positions are fixed; Wasteland Map directly parses JSON in a Godot view and does not use its bound world host to derive marker state.

### Recommended execution principle

**Do not add another major gameplay expansion yet.** First close the P0 correctness/authority defects, then wire Batch 3 end-to-end, then harden persistence and orchestration, then finish the 2D runtime layer.

---

## 2. Evidence base

### Current source inspected

- `Ashfall.csproj`
- `.github/workflows/ci.yml`
- `.github/workflows/build.yml`
- `docs/ENGINE_SUPPORT_POLICY.md`
- `docs/DEEP_CODE_AUDIT_2026-08-23.md`
- `docs/debug/BATCH_REPAIR_BATCH3_RESOLUTION.md`
- `docs/debug/logs/BATCH_REPAIR_BATCH3_IMPLEMENTATION_LOG.md`
- `Assets/_Game/Core/TimeSystem.cs`
- `scripts/ci/godot-asset-gate.sh`
- `Ashfall.Core.Tests/ProductionArtManifestTests.cs`
- `docs/visual/runtime_context_top_ids.json`
- Batch 3 Core systems and tests
- Batch 3 host-session files
- `src/Host/WorldSaveStore.cs`
- `scenes/HoldfastInterior.tscn`
- `src/World/HoldfastInteriorView.cs`
- `scenes/WastelandMap.tscn`
- `src/World/WastelandMapView.cs`
- Open issues #25 and #26
- Open PR #29

### Historical baseline used carefully

The 2026-08-18 comprehensive audit remains useful for architecture and long-running visual/spatial debt, but any old “next action” was rechecked before carrying it forward. Several old issues are now fixed or superseded and are **not** repeated as current defects.

---

## 3. Current highest-severity defect register

| ID | Severity | Current finding | Why it matters |
|---|---|---|---|
| BUG-L01 | P0 | Batch 3 save loaders accept a stale non-empty checksum | Tampered/corrupt state can be trusted despite a checksum field |
| BUG-L02 | P0 | `TimeSystem` can skip intermediate hourly events | Hour-driven needs/effects can under-tick when chunk size changes |
| BUG-L03 | P0 | `TimeSystem` restore can expose hour 24 | Public state invariant is broken immediately after restore |
| BUG-L04 | P0 | Godot import failure is non-fatal in canonical gate | CI can proceed after a failed required build/import stage |
| BUG-L05 | P0 | Active Godot project still compiles entire legacy `_Game` | Legacy authority remains load-bearing; migration can silently fork |
| BUG-L06 | P0 | Unity compatibility workflow still active | Direct contradiction with Godot-only target |
| BUG-L07 | P1 | 12 Batch 3 Core systems are not proven end-to-end player-wired | Implemented logic is not equivalent to playable integration |
| BUG-L08 | P1 | Apprenticeship and Shelter Thermal have no indexed host wrapper | Core-only systems cannot enter live Godot lifecycle |
| BUG-L09 | P1 | `Main.cs` remains a broad orchestration choke point | New systems are easy to omit from setup/tick/save/teardown |
| BUG-L10 | P1 | Save coordination is many concrete stores without global transaction semantics | Crash can create mixed-generation campaign state |
| BUG-L11 | P1 | Generated art/runtime context artifacts can drift | Tests may fail on stale assumptions or trust stale derived data |
| BUG-L12 | P1 | Required catalog loaders can degrade toward empty features | Feature may “compile” but contain no production content |
| BUG-L13 | P1 | Global nullable suppressions hide new null regressions | Host/Core defects can compile without useful warnings |
| BUG-L14 | P1 | Wasteland view parses authoritative JSON directly in Godot UI | Data authority/validation is bypassed and world binding is unused |
| BUG-L15 | P1 | Holdfast rooms and survivor positions are hard-coded | 2D view does not represent live shelter/duty simulation |
| BUG-L16 | P1 | CLI registry/help/gate coverage is manually duplicated | Commands can become unreachable or untested |
| BUG-L17 | P1 | Canonical CI runs only a small subset of available self-tests | Large verified surfaces can regress outside normal PR gates |
| BUG-L18 | P1 | Multi-resource atomicity was recently broken in multiple systems | Similar partial-consumption bugs may remain elsewhere |
| BUG-L19 | P2 | Host wrappers may emit dirty/state events on no-op ticks | Avoidable writes, UI churn, and event duplication |
| BUG-L20 | P2 | Event subscription ownership is not uniformly explicit | Recreate/rebind cycles can duplicate callbacks |

---

## 4. Batch 3 live-integration matrix

| System | Core + tests | Godot host wrapper | Save wrapper | Live composition evidence | Player UI evidence | Plan task |
|---|---:|---:|---:|---:|---:|---|
| Apprenticeship | Yes | **Not indexed** | **Not indexed** | Not found | Not found | AF-016 |
| Archive Desk | Yes | Yes | Yes; checksum load bug | Not found | Not proven | AF-017 |
| Autopsy | Yes | Yes | Needs sweep | Not found | Not proven | AF-018 |
| Contractor Roster | Yes | Yes | Needs sweep | Not found | Not proven | AF-019 |
| Decontamination | Yes | Yes | Yes; checksum load bug | Not found | Not proven | AF-020 |
| Equipment Condition | Yes | Yes | Yes; checksum load bug | Not found | Not proven | AF-021 |
| Kitchen Nutrition | Yes | Yes | Yes; checksum load bug | Not found | Not proven | AF-022 |
| Library Study | Yes | Yes | Needs sweep | Not found | Not proven | AF-023 |
| Mental Health Crisis | Yes | Yes | Needs sweep | Not found | Not proven | AF-024 |
| Shelter Schedule | Yes | Yes | Needs sweep | Not found | Not proven | AF-025 |
| Shelter Thermal | Yes | **Not indexed** | **Not indexed** | Not found | Not found | AF-026 |
| Sump Flooding | Yes | Yes | Needs sweep | Not found | Not proven | AF-027 |

**Interpretation:** “Not found” means no indexed source call site was found during the current remote audit. Before implementation, AF-015 should mechanically prove the runtime registry rather than relying on search alone.

---

## 5. Planned implementation phases

### Phase A — Authority and correctness blockers
AF-001 through AF-008.
**Goal:** establish a trustworthy Godot-only baseline and close correctness defects that can invalidate tests, time progression, saves, or builds.

### Phase B — Verification architecture
AF-009 through AF-015.
**Goal:** make generated data, nullable diagnostics, CLI tests, branch protection, and system wiring mechanically auditable.

### Phase C — Batch 3 end-to-end integration
AF-016 through AF-027.
**Goal:** turn Core-only/host-only systems into real campaign mechanics with save, tick, UI, data, and self-tests.

### Phase D — Persistence hardening
AF-028 through AF-035.
**Goal:** make every save participant tamper-safe, versioned, crash-safe, transactional, and lifecycle-safe.

### Phase E — Host architecture decomposition
AF-036 through AF-041.
**Goal:** reduce Main/legacy/bridge coupling and finish the migration to one Core authority.

### Phase F — 2D spatial gameplay integration
AF-042 through AF-046.
**Goal:** make the shelter and wasteland views state-driven rather than static/host-hard-coded.

### Phase G — Content correctness and cross-system atomicity
AF-047 through AF-048.
**Goal:** ensure new systems have real content and no partial-consumption failure paths.

### Phase H — Campaign and release gates
AF-049 through AF-050.
**Goal:** prove multi-day deterministic play, reload equivalence, and Godot-only packaging before adding more expansion scope.

---

## 6. The 50 highly actionable implementation tasks

### 01. AF-001 — Reconcile open PR #29 against current main before any implementation

**Priority:** P0
**Effort:** SMALL
**Dependencies:** None

**Evidence / rationale:** PR #29 (`audit/fix-batch3-plus-phases`) is open on 2026-08-26; its history contains older remediation commits and generated/agent-skill work. Base is current main, but the branch predates several merged repairs.

**Implementation steps**

1. Fetch PR #29 file list and classify every changed file as production code, test, generated artifact, documentation, or agent tooling.
2. Diff the PR head against `main` by semantic area; identify commits already superseded by merges on August 22–25.
3. Reject any change that reintroduces Unity-active assumptions, stale manifest counts, or deleted legacy artifacts.
4. Cherry-pick/rebase only still-valid production fixes into a fresh branch; do not merge the historical branch wholesale.
5. Run the canonical Godot/Core gate on the refreshed branch.
6. Close PR #29 if it contains no unique production value after reconciliation; otherwise retitle it to the exact remaining scope.

**Acceptance gate:** There is one reviewable branch with no superseded repairs, no historical reverts, and a clean diff against current main.

**Verification**

- `git diff main...HEAD --stat`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`

---
### 02. AF-002 — Retire the Unity compatibility GitHub Actions workflow

**Priority:** P0
**Effort:** SMALL
**Dependencies:** AF-001

**Evidence / rationale:** `.github/workflows/build.yml` still invokes Unity 6000.5.5f1 for Windows/WebGL compatibility artifacts, while the project direction is now Godot-only.

**Implementation steps**

1. Delete `.github/workflows/build.yml` or replace it with an archived documentation record outside `.github/workflows/`.
2. Remove Unity license/email/password workflow references and any repository documentation that instructs contributors to maintain them.
3. Search `.github/`, `scripts/`, and active docs for `unity`, `UNITY_LICENSE`, `game-ci/unity-builder`, and `6000.5.5f1`.
4. Preserve historical Unity evidence only in clearly archived documentation.
5. Add a CI source-policy assertion that fails if an active workflow invokes Unity again.

**Acceptance gate:** No active GitHub Actions workflow executes Unity or depends on Unity credentials.

**Verification**

- `grep -RniE 'unity-builder|UNITY_LICENSE|UNITY_EMAIL|UNITY_PASSWORD|6000\.5\.5f1' .github scripts || true`
- `git ls-files '.github/workflows/*'`

---
### 03. AF-003 — Stop compiling the entire legacy `Assets/_Game` tree into the Godot host

**Priority:** P0
**Effort:** LARGE
**Dependencies:** AF-001, AF-002

**Evidence / rationale:** `Ashfall.csproj` currently includes `<Compile Include="Assets/_Game/**/*.cs" .../>`, keeping the full legacy semantic surface load-bearing in Godot.

**Implementation steps**

1. Generate a compile-time dependency census from `src/` and `Assets/Ashfall.Core/` to every `AtomicWar._Game.*` type actually referenced.
2. Classify each dependency: already has Core equivalent, needs Core migration, presentation-only compatibility, or genuinely dead.
3. Replace the wildcard include with a temporary explicit allowlist containing only unavoidable compatibility files.
4. Migrate each allowlisted gameplay dependency to `Assets/Ashfall.Core/` or a Godot-native adapter in `src/`.
5. Shrink the allowlist to zero and remove the `_Game` compile item entirely.
6. Add a test/CI assertion that `Ashfall.csproj` never regains a wildcard `_Game` compile include.

**Acceptance gate:** `dotnet build Ashfall.csproj` succeeds without compiling any `Assets/_Game/**/*.cs` file.

**Verification**

- `dotnet build Ashfall.csproj`
- `grep -n 'Assets/_Game' Ashfall.csproj || true`
- `grep -Rni 'AtomicWar\._Game' src Assets/Ashfall.Core || true`

---
### 04. AF-004 — Rewrite engine/source-authority policy as Godot-only

**Priority:** P0
**Effort:** SMALL
**Dependencies:** AF-002, AF-003

**Evidence / rationale:** `docs/ENGINE_SUPPORT_POLICY.md` still describes Unity as a retained compatibility surface. That no longer matches the project mandate.

**Implementation steps**

1. Update `docs/ENGINE_SUPPORT_POLICY.md` so Godot 4.7.1+ and Core are the only supported runtime/build path.
2. Mark `Assets/_Game`, `Packages`, and `ProjectSettings` Unity material as archive/migration history only.
3. Update README, AGENTS/contributor guidance, CI docs, and release docs to the same authority model.
4. State explicitly that new code must never target Unity and that Unity tool invocation is prohibited.
5. Add a short migration-completion checklist covering zero `_Game` compile references and zero active Unity workflows.

**Acceptance gate:** All active contributor and release documentation states one consistent Godot/Core authority model.

**Verification**

- `grep -RniE 'compatibility build|Unity compatibility|Unity 6|unity-builder' README.md AGENTS.md docs .github || true`

---
### 05. AF-005 — Add a Godot-only source-policy CI gate

**Priority:** P0
**Effort:** SMALL
**Dependencies:** AF-003, AF-004

**Evidence / rationale:** Architecture rules currently rely heavily on convention. The repository needs a mechanical guard against regression.

**Implementation steps**

1. Create `scripts/ci/source-authority-gate.sh`.
2. Fail if `Ashfall.csproj` compiles `Assets/_Game`.
3. Fail if active workflows reference Unity tooling/secrets.
4. Fail if `Assets/Ashfall.Core/` contains `UnityEngine`, `UnityEditor`, `Godot`, `GodotSharp`, or `JsonUtility`.
5. Fail if new gameplay IDs are hard-coded in `src/` where JSON authority is required.
6. Wire the script into `.github/workflows/ci.yml` before build/test jobs.

**Acceptance gate:** A deliberate reintroduction of Unity or engine coupling makes CI fail immediately.

**Verification**

- `./scripts/ci/source-authority-gate.sh`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter CoreInvariantSourceTests`

---
### 06. AF-006 — Fix `TimeSystem` so every crossed integer hour emits exactly one hour tick

**Priority:** P0
**Effort:** MEDIUM
**Dependencies:** AF-003 if TimeSystem is migrated first; otherwise independent

**Evidence / rationale:** `Assets/_Game/Core/TimeSystem.cs` documents one `OnHourTick` per crossed integer hour, but `Advance(stepHours)` compares only the starting and final integer hour. If `MaxGameHoursPerStep > 1`, intermediate hours are skipped.

**Implementation steps**

1. Move authoritative time logic into Core if it is still only under `_Game`.
2. Rewrite advancement to iterate hour boundaries, not merely sub-step boundaries.
3. Emit `OnHourTick(day,hour)` once for each integer hour crossed, in chronological order.
4. Emit `OnDayTick` exactly once at each 24-hour boundary and define ordering relative to hour 0.
5. Test spans of 0.2h, 1h, 3.5h, 25h, and multi-day fast-forward.
6. Test with `MaxGameHoursPerStep` values below 1, equal to 1, and above 1 so tuning cannot violate correctness.

**Acceptance gate:** For any positive time delta, event count and ordering depend only on elapsed game time, not on chunk size.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter TimeSystem`
- `godot --headless --path . -- --playable-shell-selftest`

---
### 07. AF-007 — Canonicalize `TimeSystem` restore state and reject non-finite time values

**Priority:** P0
**Effort:** SMALL
**Dependencies:** AF-006

**Evidence / rationale:** `RestoreState` clamps `hourAccumulator` to 0..24 inclusive even though current hour is documented 0..23; exactly 24 can leave an invalid hour until a later tick.

**Implementation steps**

1. Define canonical serialized domain: day >= 1, hourAccumulator in [0,24), elapsed seconds finite and >= 0.
2. Normalize `hourAccumulator == 24` to next day at hour 0.
3. Normalize values >24 using day carry rather than clamping.
4. Handle NaN and positive/negative infinity explicitly for hour accumulator, elapsed seconds, and total elapsed hours setters.
5. Ensure restore never emits historical tick events.
6. Add migration/regression vectors for malformed and boundary states.

**Acceptance gate:** Every restored clock immediately satisfies its public invariants and is deterministic.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter TimeSystem`

---
### 08. AF-008 — Make Godot asset import failure fatal in the canonical gate

**Priority:** P0
**Effort:** SMALL
**Dependencies:** None

**Evidence / rationale:** `scripts/ci/godot-asset-gate.sh` currently logs an import failure and continues to subsequent gates.

**Implementation steps**

1. Change the `godot --headless --path . --import` failure branch to set failure and exit before asset-dependent tests.
2. Print the import log tail on failure for diagnosis.
3. Upload `/tmp/godot-import.log` from CI on all import failures.
4. Add a CI test fixture or script-level test proving a forced import failure returns non-zero.
5. Keep asset orphan sweep before import, but distinguish preflight and import failures clearly.

**Acceptance gate:** A failed Godot import can never result in a green canonical gate.

**Verification**

- `./scripts/ci/godot-asset-gate.sh`
- `bash -n scripts/ci/godot-asset-gate.sh`

---
### 09. AF-009 — Repair stale production-art manifest tests to assert invariants, not historical counts

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** None

**Evidence / rationale:** `ProductionArtManifestTests` still encodes the old 478-actionable/136-skipped phase model and requires `skipped > 0`, although regeneration can legitimately produce zero skipped rows.

**Implementation steps**

1. Remove absolute actionable/skipped count expectations from test comments and assertions.
2. Require only relational invariants: statuses partition total rows; IDs unique; target filenames valid; priority bands valid.
3. Allow zero `SKIP_REFERENCE_ONLY` rows.
4. Keep a separate intentional minimum only where gameplay requires at least one actionable row.
5. Regenerate production manifest and runtime context from the same source snapshot.
6. Add regression fixture for a valid manifest with zero reference-skip rows.

**Acceptance gate:** Regenerated valid manifests pass regardless of historical phase counts; malformed relationships still fail.

**Verification**

- `python3 tools/production_manifest.py`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter ProductionArtManifestTests`

---
### 10. AF-010 — Create one atomic freshness boundary for generated visual/data artifacts

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-009

**Evidence / rationale:** `runtime_context_top_ids.json` still reports `manifest_actionable: 478`; generated outputs can drift independently.

**Implementation steps**

1. Identify every derivative of the production-art source: manifest, wiring matrix, runtime context, prompt queues, summaries.
2. Make one generator command produce all derivatives in a deterministic order.
3. Add `--check` mode that regenerates into a temp directory and diffs without mutating the tree.
4. Fail CI when tracked derived artifacts are stale.
5. Write generator version/source hash into derived metadata where practical.
6. Add idempotence test: two consecutive generations must produce byte-identical outputs.

**Acceptance gate:** One command proves all tracked generated artifacts are fresh and mutually consistent.

**Verification**

- `python3 tools/production_manifest.py --check`
- `git diff --exit-code -- docs/visual`

---
### 11. AF-011 — Ratchet nullable/compiler warning policy for Core and new Godot host code

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-003

**Evidence / rationale:** `Ashfall.csproj` globally suppresses high-signal nullable warnings including CS8602/CS8603/CS8604/CS8618.

**Implementation steps**

1. Move legacy-only suppressions into a legacy props scope while `_Game` remains temporarily compiled.
2. Enable CS8602/CS8603/CS8604/CS8618 for `Assets/Ashfall.Core` and newly touched `src/Host` code.
3. Create a warnings-baseline file for unavoidable existing host warnings rather than global NoWarn.
4. Fix warnings in files modified by each subsequent task.
5. Make CI fail on any new high-signal warning outside the frozen baseline.
6. Remove the baseline incrementally until warning count reaches zero.

**Acceptance gate:** New Core/host code cannot introduce null-dereference warnings hidden by project-wide suppression.

**Verification**

- `dotnet build Ashfall.csproj -warnaserror`
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj -warnaserror`

---
### 12. AF-012 — Replace duplicated `HostCli` parsing/help wiring with a command descriptor registry

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** None

**Evidence / rationale:** The CLI surface is large, command registration/help are manually duplicated, and canonical CI exercises only a subset.

**Implementation steps**

1. Define one descriptor record: canonical flag, aliases, action, category, cadence, description.
2. Generate parse behavior and help text from the same descriptor list.
3. Validate alias uniqueness at startup/test time.
4. Change unknown `--...` flags to an explicit non-zero error instead of silently falling back to interactive mode.
5. Keep no-argument launch behavior interactive.
6. Add unit/self-tests proving every registered action is reachable and help lists every canonical flag.

**Acceptance gate:** There is one source of truth for CLI commands, no duplicate aliases, and typos cannot silently launch the game.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter HostCli`
- `godot --headless --path . -- --host-help`

---
### 13. AF-013 — Split headless verification into PR-smoke and full/nightly matrices

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-012

**Evidence / rationale:** `godot-asset-gate.sh` currently executes only seven headless flags despite a much larger self-test surface.

**Implementation steps**

1. Tag each CLI descriptor as `pr`, `nightly`, `manual`, or `ui-snapshot`.
2. Keep deterministic fast blockers in the PR tier: data, asset registry, bridge/source-authority, save integrity, core loop, expansions.
3. Create a full matrix job covering all non-interactive self-tests on schedule and manual dispatch.
4. Shard long tests by category to keep failure output legible.
5. Upload per-gate logs and a machine-readable result summary.
6. Add a coverage assertion that every non-manual command belongs to at least one CI tier.

**Acceptance gate:** Every supported self-test has a defined execution cadence and no command is accidentally orphaned from verification.

**Verification**

- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --host-help`

---
### 14. AF-014 — Verify and enforce required branch-protection checks

**Priority:** P1
**Effort:** SMALL
**Dependencies:** AF-008, AF-013

**Evidence / rationale:** The deep audit reported that strong CI existed without required-status-check enforcement. Current protection metadata could not be read through the integration, so this must be verified explicitly.

**Implementation steps**

1. Inspect GitHub branch rules/rulesets for `main`.
2. Require Repository/Data Validation, Core Tests, and Godot Headless Gates.
3. Require pull requests and up-to-date branches before merge.
4. Disable force-push and direct bypass except a documented emergency role.
5. If merge queue is enabled, require the same checks in queue context.
6. Create a disposable failing PR to prove a red gate blocks normal merge.

**Acceptance gate:** A PR with a failing canonical gate cannot be merged to `main` through the normal path.

**Verification**

- `GitHub ruleset/branch-protection inspection`
- `failing-check dry-run PR`

---
### 15. AF-015 — Create a machine-readable persistent-system wiring registry and omission test

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-012

**Evidence / rationale:** `Main.cs` uses repeated Setup/Save/Flush/tick patterns; new Batch 3 systems demonstrate how Core code can exist without runtime composition.

**Implementation steps**

1. Define descriptors for each stateful runtime system: ID, factory/owner, tick cadence, save participant/store, dirty source, UI surface, self-test.
2. Populate the registry for every currently live host session.
3. Add a test that fails if a registered persistent system lacks save, restore, tick ownership, or self-test metadata.
4. Add a complementary scan/test for known Core systems that are not explicitly registered or explicitly marked Core-only.
5. Use the registry to generate a diagnostic wiring report at headless startup.
6. Make Batch 3 integration tasks close only when their descriptor is complete.

**Acceptance gate:** A newly created Core system cannot silently remain unwired without being reported by verification.

**Verification**

- `godot --headless --path . -- --wiring-report-selftest`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Wiring`

---
### 16. AF-016 — Wire Apprenticeship end-to-end into the live Godot campaign

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015

**Evidence / rationale:** `ApprenticeshipSystem` and tests exist in Core, but no `ApprenticeshipHostSession` indexed on current main and no live composition evidence was found.

**Implementation steps**

1. Create `src/Host/ApprenticeshipHostSession.cs` as a thin adapter over the existing Core system.
2. Inject the authoritative SkillProgression, DutyRoster, SurvivorRelations, and seeded RNG instances—never create parallel domain authorities.
3. Add checksummed/versioned save capture/restore using the standardized participant/store contract.
4. Register exactly one daily tick owner in the persistent-system registry.
5. Add data definitions for training disciplines/requirements only if not already in authoritative JSON; validate IDs.
6. Add a survivor/training UI panel for mentor, apprentice, skill, progress, cancel/completion state.
7. Add a headless self-test that starts an apprenticeship, advances days, saves, reloads, and verifies completion deterministically.

**Acceptance gate:** A player can start, inspect, persist, resume, and complete an apprenticeship in a normal campaign.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --apprenticeship-selftest`

---
### 17. AF-017 — Complete Archive Desk runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `ArchiveDeskHostSession` and `ArchiveDeskSaveStore` exist, but no indexed `Main`/composition call site was found. Its loader also has checksum-validation regression.

**Implementation steps**

1. Construct the host session from the existing authoritative Journal, KnowledgeBase, Inventory, and DutyRoster instances.
2. Load `archive_inks.json` through `ArchiveInkCatalogLoader` and fail loudly if a required production catalog is empty/missing.
3. Restore saved ArchiveDesk state before player interaction; register dirty/save flush semantics.
4. Tick once per campaign day from the single daily pipeline.
5. Add an Archive Desk UI that selects evidence, archivist, ink, shows queued jobs, and allows valid cancellation.
6. Surface completed transcriptions into the existing Journal/Codex without duplicate unlocks.
7. Add save/tamper/reload and UI lifecycle headless tests.

**Acceptance gate:** Archive Desk actions are player-reachable and survive save/load with validated integrity.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --archive-desk-selftest`

---
### 18. AF-018 — Complete Autopsy runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `AutopsySystem`, tests, and an `AutopsyHostSession` exist, but no live composition call site was found.

**Implementation steps**

1. Resolve and inject authoritative Inventory, Radiation, Ventilation/Shelter, Research, and Medical instances.
2. Define the body/case source: only real deceased/case IDs from runtime state may enter the autopsy queue.
3. Register save/restore and deterministic daily/procedure progression.
4. Wire results to Journal evidence and Research unlocks using stable IDs.
5. Create Medical/Autopsy UI with prerequisites, contamination risk, procedure state, evidence output, and blocked reasons.
6. Add post-load idempotency tests so completed autopsies cannot award evidence twice.
7. Add a full headless case from death/case registration through result persistence.

**Acceptance gate:** Autopsy is a real medical/narrative loop, not a Core-only API.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --autopsy-selftest`

---
### 19. AF-019 — Complete Contractor Roster runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `ContractorRosterSystem` and `ContractorRosterHostSession` exist, but no live composition call site was found.

**Implementation steps**

1. Inject authoritative Inventory, DutyRoster, Expeditions, faction/economy dependencies required by Core behavior.
2. Load contractor definitions/fees/skills from JSON rather than host constants.
3. Wire hire, assignment, payment, expiry, missed-payment, and dismissal actions to UI.
4. Register daily tick exactly once and persist contract dates/payment state.
5. Ensure contractors cannot simultaneously violate survivor/duty assignment exclusivity.
6. Surface payment obligations in economy/dashboard alerts.
7. Add expiry-boundary, insufficient-funds, save/reload, and expedition-assignment integration tests.

**Acceptance gate:** Contractors can be hired, assigned, paid, expire correctly, and persist without duplicate duty ownership.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --contractor-roster-selftest`

---
### 20. AF-020 — Complete Decontamination runtime integration and validate net-contamination semantics

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `DecontaminationHostSession` exists and Core duplicate-queue bugs were repaired, but no live composition call site was found; Batch 3 audit also left net-contamination intent unresolved.

**Implementation steps**

1. Inject the same Radiation, Inventory, Airlock, and StartingLevel instances used by the rest of the campaign.
2. Document and test the formula for surface contamination removed versus survivor dose/airlock contamination transferred.
3. Define safe-release vs unsafe-release consequences in Core; do not bury them in UI callbacks.
4. Wire expedition/airlock returns to enqueue eligible contaminated survivors/gear exactly once.
5. Add decon queue/status UI and explicit resource requirements.
6. Persist active/queued cases and ensure reload does not replay completion effects.
7. Add deterministic contamination balance tests and a return-from-expedition integration self-test.

**Acceptance gate:** Contaminated returns flow into decon automatically and all dose/resource consequences are specified and test-pinned.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --decontamination-selftest`

---
### 21. AF-021 — Complete Equipment Condition runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `EquipmentConditionHostSession` exists, but no live composition call site was found. Core atomic maintenance consumption has been repaired.

**Implementation steps**

1. Map inventory item instances/equipped gear/weapons/tools to stable equipment instance IDs.
2. Register items when acquired/equipped and remove/retire condition state when items are destroyed.
3. Route actual tool/weapon/gear use into `UseItem` rather than UI-only test calls.
4. Load maintenance types/parts/stations from data authority or stable Core definitions.
5. Wire maintenance jobs into crafting/workshop UI and daily tick.
6. Persist condition and pending jobs with integrity validation.
7. Add cross-system tests: degraded gear changes its real gameplay effect; repaired gear restores it.

**Acceptance gate:** Condition changes are driven by real gameplay usage and visibly affect mechanics, maintenance, and persistence.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --equipment-condition-selftest`

---
### 22. AF-022 — Complete Kitchen Nutrition runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028, AF-047

**Evidence / rationale:** `KitchenNutritionHostSession` exists but no live composition call site was found; Core job eviction and resource atomicity were repaired.

**Implementation steps**

1. Resolve recipe authority: map existing food/recipe JSON into kitchen recipes instead of passing arbitrary dictionaries from UI.
2. Inject the live Inventory and Needs instances.
3. Wire prep jobs to cook selection, shelter power/heat prerequisites if required, and daily progression.
4. Wire `ServeMeal` to actual survivor hunger/morale and prevent double serving from repeated UI events.
5. Add pantry/spoilage UI with predicted expiry and portions.
6. Persist pantry/jobs and validate checksum/tamper behavior.
7. Add day-advance test covering prep → pantry → serve → needs change → save/reload.

**Acceptance gate:** Cooking consumes authoritative ingredients and produces meals that change live survivor needs across save/load.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --kitchen-nutrition-selftest`

---
### 23. AF-023 — Complete Library Study runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028, AF-047

**Evidence / rationale:** `LibraryStudyHostSession` exists but no live composition call site was found.

**Implementation steps**

1. Inject authoritative SkillProgression, Research, Journal, and DutyRoster instances.
2. Load study materials/manuals from JSON and validate knowledge/research IDs.
3. Wire study assignment with duty exclusivity and explicit duration/progress.
4. On completion, award the intended skill/research/journal effect exactly once.
5. Add Library UI with material, reader, expected outcome, progress, and blocked reasons.
6. Persist active studies/unlocks and add migration-safe schema.
7. Add save/reload idempotency and concurrent-duty regression tests.

**Acceptance gate:** Study jobs are player-reachable, deterministic, and unlock real research/skills without duplicate awards.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --library-study-selftest`

---
### 24. AF-024 — Complete Mental Health Crisis runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `MentalHealthCrisisHostSession` exists but no live composition call site was found.

**Implementation steps**

1. Inject authoritative Needs, Medical, ChemicalDependency, DutyRoster, Relations/trauma dependencies used by Core.
2. Define real crisis triggers from stress/morale/withdrawal/trauma rather than manual debug calls.
3. Guarantee one unresolved crisis per survivor and deterministic resolution selection.
4. Wire treatment/intervention choices into Medical/Survivors UI and duty availability.
5. Persist active/resolved crises and protect against replay on load.
6. Surface meaningful dashboard alerts and event log entries.
7. Add trigger → intervention → resolution → save/reload headless scenario.

**Acceptance gate:** Mental-health crises emerge from live state, affect availability, can be treated, and persist correctly.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --mental-health-crisis-selftest`

---
### 25. AF-025 — Complete Shelter Schedule runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `ShelterScheduleHostSession` exists but no live composition call site was found.

**Implementation steps**

1. Use the authoritative PowerGridSystem and room/station identities.
2. Define schedule slots/loads in data or stable Core definitions; remove host-only magic times.
3. Integrate schedule execution with the campaign clock/day pipeline without duplicate ticks.
4. Expose schedule editing and current/next load in Shelter UI.
5. Make brownouts and unavailable equipment produce explicit blocked/degraded outcomes.
6. Persist schedule edits and current schedule state.
7. Add tests for midnight rollover, power loss, reload, and conflicting schedules.

**Acceptance gate:** Player-defined shelter schedules actually control live loads and survive day boundaries/save-load.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --shelter-schedule-selftest`

---
### 26. AF-026 — Create and wire the missing Shelter Thermal host/runtime layer

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `ShelterThermalSystem` and Core tests exist, but no `ShelterThermalHostSession` was indexed on current main; Batch 3 notes also flagged unresolved thermal design decisions.

**Implementation steps**

1. Document authoritative thermal units, safe temperature bands, heat sources, losses, and survivor need consequences.
2. Resolve integration with Year-of-Ash deep freeze, StartingLevel shelter state, room model, fuel/power, and Needs.
3. Create a thin `ShelterThermalHostSession` only after Core equations/ownership are pinned.
4. Add checksummed/versioned save participant and daily/hourly tick cadence as appropriate.
5. Add Shelter thermal UI: indoor/outdoor temperature, trend, heater state, risk warnings, actions.
6. Add deterministic heat-loss and cold-injury integration tests across extreme weather.
7. Add save/reload tests at fractional thermal states.

**Acceptance gate:** Thermal state is a single authoritative simulation affecting live survivors and shelter systems, not an isolated test system.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --shelter-thermal-selftest`

---
### 27. AF-027 — Complete Sump Flooding runtime integration

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-028

**Evidence / rationale:** `SumpFloodingHostSession` exists but no live composition call site was found.

**Implementation steps**

1. Inject authoritative Weather, PowerGrid, room/node model, and YearOfAsh deep-freeze state.
2. Map flooded nodes/rooms to real shelter room IDs instead of disconnected synthetic IDs.
3. Wire pump power draw and condition to actual power/equipment systems.
4. Define gameplay consequences for depth thresholds: access, equipment damage, health, contamination.
5. Create Shelter flooding/pump UI and alert thresholds.
6. Persist flood depths/pump state and ensure restoration does not re-emit historical incidents.
7. Add storm → pump loss → flooding → recovery → save/reload headless scenario.

**Acceptance gate:** Flooding is driven by real weather/power and has measurable shelter consequences and recovery paths.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --sump-flooding-selftest`

---
### 28. AF-028 — Fix checksum verification in all new Batch 3 save stores

**Priority:** P0
**Effort:** MEDIUM
**Dependencies:** None

**Evidence / rationale:** At least Archive Desk, Kitchen Nutrition, Equipment Condition, and Decontamination loaders accept any non-empty checksum without recomputing it; older stores such as `WorldSaveStore` correctly recompute and compare.

**Implementation steps**

1. Inventory every new Batch 3 `*SaveStore` and `*HostSave` class.
2. For envelope-format saves, require a non-empty checksum.
3. Recompute `SaveChecksum.Compute(envelope)` after deserialization and compare using `StringComparison.Ordinal`.
4. Reject mismatch before returning any state.
5. Preserve bare-state legacy fallback only when the payload is genuinely not an envelope, not when an envelope is malformed.
6. Add three tests per store: clean round-trip, state mutation with stale checksum rejected, missing checksum rejected.
7. Add all stores to `SaveStoreChecksumSweepTests` or successor participant-registry tests.

**Acceptance gate:** No checksummed Batch 3 save can be tampered with while retaining a stale non-empty checksum and still load.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveStoreChecksum`
- `dotnet build Ashfall.csproj`

---
### 29. AF-029 — Standardize Batch 3 save envelope versions and migration policy

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-028

**Evidence / rationale:** Batch 3 host saves use `SchemaVersion = "1.0"` ad hoc; new systems need the same explicit migration/future-version behavior as mature stores.

**Implementation steps**

1. Choose one schema-version representation for host save envelopes (prefer integer or existing project convention).
2. Define current version constants per participant.
3. Reject future versions explicitly with a diagnostic.
4. Implement migration functions for any shipped prior form, including bare-state legacy fallback where supported.
5. Validate migrated detached state before apply.
6. Pin wire format with serialization tree/golden vector tests.
7. Document retirement criteria for legacy bare-state loading.

**Acceptance gate:** Every new save participant has explicit current version, future rejection, migration tests, and stable wire-format coverage.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveWireContract`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Migration`

---
### 30. AF-030 — Make every save write crash-safe with temp-file + atomic replace

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-028

**Evidence / rationale:** Many stores use direct `File.WriteAllText(path, ...)`; process/power loss can leave a truncated current save.

**Implementation steps**

1. Add a host `AtomicSaveWriter` using the project file abstraction where practical.
2. Write serialized bytes to `path.tmp` in the same filesystem.
3. Flush/close the temp file before replace.
4. Atomically replace/move into the canonical file and optionally retain one `.bak` previous generation.
5. On load, define recovery order for current, backup, and invalid temp files.
6. Add fault-injection tests for failure before write, mid-write, and before replace.
7. Migrate all host stores to the shared writer rather than copy-pasting.

**Acceptance gate:** A simulated interrupted save cannot destroy the last known-good campaign state.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter AtomicSave`
- `godot --headless --path . -- --save-fault-selftest`

---
### 31. AF-031 — Add campaign save-generation IDs to prevent cross-store mixed snapshots

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-030

**Evidence / rationale:** The campaign persists many independent domain files; adding Batch 3 stores increases the risk that a crash yields files from different day/save generations.

**Implementation steps**

1. Generate a monotonic campaign save generation ID for each `SaveAll` transaction.
2. Include generation ID, campaign ID, and campaign day in each domain envelope.
3. Write all participant temp files first.
4. Commit a small manifest only after all participant writes validate.
5. On load, accept only participant files matching the committed manifest generation or recover the prior complete generation.
6. Add a simulated crash after N-of-M participant writes and prove load selects a coherent generation.
7. Expose generation mismatch diagnostics instead of silently mixing state.

**Acceptance gate:** The game never restores a campaign composed of domain files from different save transactions.

**Verification**

- `godot --headless --path . -- --multistore-save-selftest`

---
### 32. AF-032 — Implement the versioned save-participant registry with two-phase transactional restore

**Priority:** P1
**Effort:** VERY LARGE
**Dependencies:** AF-015, AF-028, AF-029, AF-031

**Evidence / rationale:** Tracked issue #26 identifies excessive persistence fan-out and partial-restore risk in the central coordinator.

**Implementation steps**

1. Define an engine-agnostic participant descriptor: stable ID, schema version, capture, decode/migrate/validate, apply.
2. Register a small low-risk participant first and retain old fields for compatibility.
3. Validate participant ID uniqueness and required-participant presence.
4. Phase 1 restore: deserialize/checksum/migrate/validate every participant into detached snapshots only.
5. Phase 2 restore: apply snapshots only if Phase 1 succeeded globally.
6. Add rollback/no-mutation assertions for one deliberately corrupt participant.
7. Migrate remaining participants incrementally; eliminate concrete fields from the coordinator as each moves.
8. Make the wiring registry (AF-015) and save registry cross-check each other.

**Acceptance gate:** A corrupt participant cannot leave a running session partially restored, and adding a participant no longer expands a god coordinator.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveParticipant`
- `godot --headless --path . -- --transactional-save-selftest`

---
### 33. AF-033 — Add property-based/fuzz corruption tests across every save participant

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-028, AF-032

**Evidence / rationale:** Checksums catch mutation only if all stores validate correctly; malformed JSON, future versions, null collections, and numeric edge cases require systematic coverage.

**Implementation steps**

1. Enumerate all save envelopes/DTOs from the participant registry.
2. Generate valid representative states and serialize them.
3. Apply byte/field mutations, truncation, missing checksum, duplicated fields, null collections, invalid enum/string IDs, NaN-like textual cases where parsers allow them.
4. Assert load either returns fully validated state or a controlled failure—never partial mutation or unhandled crash.
5. Include old supported versions in the fuzz corpus.
6. Store deterministic seeds and minimize failing cases.
7. Run a bounded corpus in PR CI and a larger corpus nightly.

**Acceptance gate:** Save corruption handling is uniformly fail-safe across all registered participants.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter SaveFuzz`

---
### 34. AF-034 — Eliminate unconditional host `StateChanged`/dirty churn on no-op daily ticks

**Priority:** P2
**Effort:** MEDIUM
**Dependencies:** AF-015

**Evidence / rationale:** Several Batch 3 wrappers call `System.TickDay(day); StateChanged?.Invoke();` even if Core state did not change, potentially causing avoidable UI refresh/save flush churn.

**Implementation steps**

1. Instrument dirty marks and save writes per simulated day in a representative 30-day run.
2. For each host session, rely on Core change events when they accurately indicate mutations.
3. Where Core lacks a change event, compare a lightweight mutation/version counter instead of full serialization.
4. Remove duplicate wrapper-level `StateChanged` invocations when a Core event already fires for the same action.
5. Ensure real daily changes still mark save dirty exactly once per logical transaction.
6. Add test assertions on event/dirty counts for no-op and changing ticks.

**Acceptance gate:** No-op daily ticks do not trigger unnecessary save writes or duplicate UI refresh events.

**Verification**

- `godot --headless --path . -- --dirty-coalescing-selftest`

---
### 35. AF-035 — Add explicit subscription ownership and disposal for host sessions/panels

**Priority:** P2
**Effort:** MEDIUM
**Dependencies:** AF-036

**Evidence / rationale:** Host wrappers subscribe to Core events with lambdas; repeated new-game/load/UI reconstruction can create duplicate callbacks unless ownership is strictly bounded.

**Implementation steps**

1. Inventory event subscriptions in `src/Host`, `src/UI`, and `src/World`.
2. Introduce `IDisposable`/`Detach` for wrappers that can outlive/rebind their Core source.
3. Store delegate instances when needed so handlers can be unsubscribed deterministically.
4. Call detach during game-session teardown, new game, load replacement, and panel free.
5. Add weak-reference/lifecycle tests that repeatedly create/destroy sessions and assert one callback per event.
6. Add a diagnostic counter for duplicate subscription detection in debug/headless tests.

**Acceptance gate:** Repeated new-game/load/UI open-close cycles never multiply event handlers or retain abandoned session graphs.

**Verification**

- `godot --headless --path . -- --lifecycle-selftest`
- `godot --headless --path . -- --expedition-panel-uitest`

---
### 36. AF-036 — Decompose `src/Main.cs` phase 1: CLI, diagnostics, and save-flush coordination

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-015, AF-012

**Evidence / rationale:** Tracked issue #25 identifies `src/Main.cs` as an orchestration god object with large host/UI/save/lifecycle responsibility.

**Implementation steps**

1. Extract CLI dispatch/execution lifecycle to `HostCliRunner` without changing flags or exit codes.
2. Extract dirty flag/coalesced save behavior to `SaveFlushCoordinator` using the persistent-system registry.
3. Extract diagnostics timers/version caching to `DiagnosticsCoordinator`.
4. Keep gameplay rules in Core; extracted classes are composition/infrastructure only.
5. Add focused tests/self-tests for each extracted coordinator.
6. After each extraction, diff behavior and run full canonical gates before proceeding.
7. Measure Main line/method/field reduction and reject extra abstractions that merely move complexity.

**Acceptance gate:** `Main.cs` no longer owns CLI dispatch, diagnostics cadence, or per-feature dirty/flush mechanics.

**Verification**

- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --host-help`

---
### 37. AF-037 — Decompose `src/Main.cs` phase 2: UI composition/navigation and game-session lifecycle

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-036

**Evidence / rationale:** Even after infrastructure extraction, Main should not directly construct dozens of panels and own every stateful host session transition.

**Implementation steps**

1. Create a `UiCompositionRoot` that constructs/binds panels from already-created host dependencies.
2. Create a `NavigationCoordinator` for screen switching, modal ownership, and Escape/back behavior.
3. Create a `GameSessionCoordinator` for new/load/start/end/teardown transitions.
4. Move only wiring; leave all simulation in Core and persistence mechanics in save coordinators.
5. Define explicit construction order and teardown order.
6. Add new-game → play → save → main menu → continue lifecycle self-test.
7. Remove obsolete Main fields/methods after each ownership transfer.

**Acceptance gate:** Main becomes a small Godot entry/composition shell with obvious owners for session, UI, and infrastructure.

**Verification**

- `godot --headless --path . -- --playable-shell-selftest`
- `godot --headless --path . -- --ui-layout-selftest`

---
### 38. AF-038 — Eliminate duplicated survival behavior from `HoldfastRuntimeSession`

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-003

**Evidence / rationale:** Project architecture notes flag `src/Host/HoldfastRuntimeSession.cs` as duplicating Core survival mechanics, violating thin-host ownership.

**Implementation steps**

1. Diff every state transition/calculation in HoldfastRuntimeSession against existing Core systems.
2. Classify methods as presentation adapter, orchestration, or duplicated gameplay rule.
3. Move missing engine-agnostic rules into Core with deterministic tests.
4. Replace host calculations with calls to the single Core authority.
5. Remove duplicate state fields once save migration/compatibility is covered.
6. Add parity tests using fixed scenarios before deleting the old path.
7. Assert no new gameplay constants remain in the host.

**Acceptance gate:** Holdfast host code only adapts inputs/events/UI; survival calculations have one Core implementation.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Holdfast`
- `grep -nE 'damage|radiation|hunger|thirst|morale' src/Host/HoldfastRuntimeSession.cs || true`

---
### 39. AF-039 — Consolidate duplicate `WornGear` domain models

**Priority:** P2
**Effort:** MEDIUM
**Dependencies:** AF-003

**Evidence / rationale:** Project architecture notes identify `Inventory.WornGear` and `Radiation.WornGear` as duplicated models connected by a sanctioned conversion bridge.

**Implementation steps**

1. Enumerate all fields/semantics and call sites of both models.
2. Choose one Core-owned canonical gear state representation.
3. Add radiation-view calculation methods/adapters without copying state.
4. Migrate consumers one subsystem at a time and preserve serialized compatibility.
5. Add tests for gas mask/hazmat attenuation and equipment-condition interaction.
6. Remove the obsolete duplicate type only after no references remain.
7. Pin one type definition with a source-invariant test.

**Acceptance gate:** One authoritative worn-gear state feeds inventory, equipment condition, and radiation without state conversion drift.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter InventoryGearBridge`
- `grep -Rni 'class WornGear' Assets/Ashfall.Core`

---
### 40. AF-040 — Replace swallowed catalog-loader exceptions with typed diagnostics

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-003

**Evidence / rationale:** Architecture audit notes identify bare `catch { }` blocks in catalog loaders, which can silently turn content/configuration failures into empty systems.

**Implementation steps**

1. Search active Core/src for `catch {}` and catch blocks that discard exceptions.
2. Classify expected optional-file absence separately from parse/schema/reference failure.
3. Return typed result objects or throw controlled configuration exceptions for required production catalogs.
4. Include file path, catalog ID, and JSON path in diagnostics.
5. Allow optional catalogs only through explicit metadata/configuration.
6. Add malformed-catalog tests proving failures are visible and actionable.
7. Make canonical data gate exercise these loaders, not only generic JSON syntax.

**Acceptance gate:** Required catalog failures cannot be silently swallowed into an apparently empty feature.

**Verification**

- `grep -RniE 'catch[[:space:]]*\{[[:space:]]*\}' Assets/Ashfall.Core src || true`
- `godot --headless --path . -- --data-integrity-selftest`

---
### 41. AF-041 — Shrink and then retire `src/Bridge` after legacy compile removal

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-003

**Evidence / rationale:** `src/Bridge` is a migration aid; keeping it after `_Game` is no longer compiled creates dead semantic surface and future confusion.

**Implementation steps**

1. After each `_Game` dependency migration, remove now-unused shim types.
2. Create a bridge usage report mapping each remaining shim member to an active source reference.
3. Fail CI if a new shim type is added without an explicit temporary migration ticket.
4. Remove cosmetic no-op surfaces once no active code references them.
5. Retain bridge self-test only while any bridge code is compiled.
6. Delete the Bridge project/directory and gate when usage reaches zero.
7. Update docs to mark the migration complete.

**Acceptance gate:** The active Godot build contains no Unity compatibility namespace or shim implementation.

**Verification**

- `grep -Rni 'UnityEngine' src Assets/Ashfall.Core || true`
- `dotnet build Ashfall.csproj`

---
### 42. AF-042 — Refactor Wasteland Map data loading onto the project data/port architecture

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-003

**Evidence / rationale:** `WastelandMapView` directly uses Godot `FileAccess` and `System.Text.Json` for `wasteland_map_v1.json`; it also stores `_worldHost` but does not use it to derive marker state.

**Implementation steps**

1. Create/locate a Core/data loader for wasteland map definitions using `IFileIO` + `IJsonSerializer`.
2. Move DTO validation/reference checks to Core/data layer.
3. Have the Godot view receive a validated map model from a host session instead of parsing JSON itself.
4. Remove direct `System.Text.Json`/`FileAccess` content loading from the view.
5. Add loader tests for missing, malformed, duplicate, and invalid-reference nodes.
6. Make world host state decorate the immutable authored map model at runtime.
7. Keep marker rendering Godot-specific.

**Acceptance gate:** WastelandMapView renders validated supplied data and contains no authoritative content parsing logic.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter WastelandMap`
- `dotnet build Ashfall.csproj`

---
### 43. AF-043 — Replace hard-coded Holdfast room definitions with authoritative room/layout data

**Priority:** P1
**Effort:** MEDIUM
**Dependencies:** AF-003

**Evidence / rationale:** `HoldfastInteriorView.PopulateRoomHotspots()` hard-codes three rooms and display coordinates/names.

**Implementation steps**

1. Identify the authoritative Standing Record/location-layout room definitions.
2. Create a view model containing room ID, display label, position/rect, station type, and runtime status.
3. Supply that model to HoldfastInteriorView through a host adapter.
4. Remove the hard-coded anonymous room array from the view.
5. Validate every displayed room ID against data authority.
6. Add layout tests for required rooms and unique hotspot bounds.
7. Use the same room IDs for thermal, flooding, duty, power, and interaction routing.

**Acceptance gate:** Shelter spatial UI uses the same room authority as simulation systems and expands automatically with authored layouts.

**Verification**

- `godot --headless --path . -- --standing-record-selftest`
- `godot --headless --path . -- --ui-layout-selftest`

---
### 44. AF-044 — Bind survivor actors to Duty Roster assignments and real room positions

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-043, AF-025

**Evidence / rationale:** `HoldfastInteriorView` spawns only the first four survivors at fixed horizontal positions and updates vitals, not duty-room positions.

**Implementation steps**

1. Bind the view to DutyRoster/room assignment state in addition to Survivors.
2. Map each assignment role/station to a canonical room anchor.
3. Render all relevant living shelter occupants using crowding/visibility rules rather than an arbitrary first-four cap.
4. Animate movement between prior and new room anchors when assignments change.
5. Keep actor state derived from host sessions—do not mutate roster from animation callbacks.
6. Add off-duty/bunk/medical/quarantine position policies.
7. Add a self-test that changes duty assignment and verifies the actor target room updates.

**Acceptance gate:** The 2D shelter view visually reflects live survivor assignments and state rather than static placeholder positions.

**Verification**

- `godot --headless --path . -- --duty-roster-selftest`
- `godot --headless --path . -- --holdfast-interior-selftest`

---
### 45. AF-045 — Make Wasteland Map markers reflect live access, hazards, factions, and expeditions

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-042

**Evidence / rationale:** `WastelandMapView` currently builds markers from static JSON `Danger`/Faction fields; bound `_worldHost` is unused. The scene background is `bunker_map_root.jpg`, not a confirmed regional production map.

**Implementation steps**

1. Define a runtime map-node view model combining authored location data with world weather/radiation, faction control, discovery, quest access, and expedition status.
2. Update marker state from host events without reloading JSON.
3. Disable/lock destinations according to actual access rules and show the blocking reason.
4. Show expedition en-route/returning states and selected destination.
5. Replace the generic/bunker map background only with an approved regional asset registered through the asset pipeline.
6. Add hazard/faction legend and tooltip details.
7. Add a headless/UI test proving a faction/weather/access change updates the corresponding marker.

**Acceptance gate:** Map markers are live gameplay instruments, not a static catalog browser.

**Verification**

- `godot --headless --path . -- --world-selftest`
- `godot --headless --path . -- --expedition-selftest`
- `godot --headless --path . -- --wasteland-map-uitest`

---
### 46. AF-046 — Add dedicated 2D spatial-view regression/self-tests

**Priority:** P2
**Effort:** MEDIUM
**Dependencies:** AF-043, AF-044, AF-045

**Evidence / rationale:** The 2D spatial layer remains substantially less verified than the dashboard/data systems.

**Implementation steps**

1. Create a headless `--holdfast-interior-selftest` that instantiates the scene and validates required nodes/resources.
2. Create `--wasteland-map-uitest` that loads all markers and simulates a node click.
3. Verify no duplicate markers/actors after repeated initialize/bind cycles.
4. Verify room hotspots and marker positions stay inside supported viewport bounds.
5. Verify missing textures/data fail the test, not silently log and continue.
6. Add screenshot tests only for stable structural regions; keep logic assertions primary.
7. Include both tests in the full CI matrix and the faster one in PR CI.

**Acceptance gate:** Spatial scenes have automated lifecycle, binding, resource, and interaction coverage.

**Verification**

- `godot --headless --path . -- --holdfast-interior-selftest`
- `godot --headless --path . -- --wasteland-map-uitest`

---
### 47. AF-047 — Audit and populate all Batch 3 production catalogs; fail loudly on required empty catalogs

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-040

**Evidence / rationale:** Batch 3 resolution notes explicitly identify four wired systems with empty catalogs, while newer host wrappers can also silently operate with no loaded content.

**Implementation steps**

1. List every Batch 3 system and all expected catalogs/definition sets.
2. For each catalog, record required/optional status, row count, schema version, and loader.
3. Populate missing production definitions using existing canonical IDs; do not create duplicate authorities.
4. Add cross-reference validation for item, skill, room, survivor, evidence, recipe, and station IDs.
5. Change required loaders to fail initialization when row count is zero.
6. Add a data-integrity summary by feature with minimum intentional cardinalities.
7. Add one smoke action per catalog so 'loads' also means 'usable by runtime'.

**Acceptance gate:** Every player-facing Batch 3 feature has non-empty validated production content and cannot silently boot contentless.

**Verification**

- `godot --headless --path . -- --data-integrity-selftest`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter NewCatalogLoaderTests`

---
### 48. AF-048 — Perform a Core-wide validate-before-mutate atomicity sweep

**Priority:** P1
**Effort:** LARGE
**Dependencies:** AF-003

**Evidence / rationale:** Batch 3 repairs established an explicit invariant after Kitchen/Equipment bugs: multi-resource actions must validate all prerequisites before consuming anything.

**Implementation steps**

1. Search Core for methods that call `Remove`, `Consume`, `Spend`, `Deduct`, `Pay`, or mutate state inside loops before later failure returns.
2. Prioritize crafting, barter, medical treatment, expedition launch, tribute, construction, repair, decon, research, and new Batch 3 systems.
3. For each action, write a regression test where an early requirement is available and a later requirement fails.
4. Refactor to two-phase validation/reservation/commit or a small transaction helper.
5. Assert failure leaves inventory, currency, duty assignment, and side-effect ledgers unchanged.
6. Test cancellation/refund paths separately from precondition failure.
7. Document the atomicity invariant in contributor guidance and code-review checklist.

**Acceptance gate:** No multi-step failed action can partially consume resources or partially mutate related systems.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter Atomicity`
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`

---
### 49. AF-049 — Build deterministic 30-day and 100-day end-to-end campaign harnesses with reload checkpoints

**Priority:** P0
**Effort:** LARGE
**Dependencies:** AF-016 through AF-035 as available

**Evidence / rationale:** Unit/self-tests are deep but fragmented; integration omissions and cross-store drift require a campaign-level oracle.

**Implementation steps**

1. Define a deterministic scripted player policy using a fixed seed and stable action sequence.
2. Run Day 1 opening protocol, shelter assignments, inventory/crafting, medical, radio, expedition, and new Batch 3 actions.
3. Save/reload at multiple checkpoints (e.g. days 2, 7, 15, 30; longer run at 50/100).
4. After each reload, compare a normalized campaign snapshot against an uninterrupted control run.
5. Assert no duplicate rewards/events, no negative resources, no invalid IDs, and all invariants remain valid.
6. Record final deterministic hash so regression changes are explicit and reviewable.
7. Run 30-day harness in PR CI; 100-day harness nightly.

**Acceptance gate:** Interrupted/reloaded runs converge exactly with uninterrupted deterministic runs across all integrated systems.

**Verification**

- `godot --headless --path . -- --campaign-30day-selftest`
- `godot --headless --path . -- --campaign-100day-selftest`

---
### 50. AF-050 — Create a Godot-only release-readiness gate and freeze new expansion work until it is green

**Priority:** P1
**Effort:** VERY LARGE
**Dependencies:** AF-002 through AF-049

**Evidence / rationale:** Expansion 11 planning has already appeared while core integration debt remains. Release confidence should be based on the active Godot product, not feature count.

**Implementation steps**

1. Define Alpha exit criteria: zero P0s, Batch 3 fully player-wired, transactional saves, spatial views functional, canonical CI green.
2. Create Godot export presets for supported desktop targets and verify clean-clone exports.
3. Add CI release job that builds the Godot executable only after canonical gates.
4. Smoke-run packaged build with a fresh user directory and a save/continue cycle.
5. Review runtime dependencies such as Sentry for initialization, privacy/consent, offline failure behavior, and necessity; remove unused dependencies.
6. Verify LFS/imported assets and data catalogs are present in export.
7. Generate a release manifest with commit SHA, data schema versions, save schema versions, and test results.
8. Do not begin full Expansion 11 implementation until this gate and the 30-day campaign harness are green.

**Acceptance gate:** A clean clone can produce and smoke-run a Godot-only build with a coherent save/load campaign and no known P0 integration defect.

**Verification**

- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`
- `dotnet build Ashfall.csproj`
- `./scripts/ci/godot-asset-gate.sh`
- `godot --headless --path . -- --campaign-30day-selftest`
- `godot --headless --path . --export-release '<platform preset>'`

---

## 7. Dependency-critical ordering

The tasks are numbered for a reason. The safest critical path is:

1. **AF-001 → AF-008:** remove stale branch/Unity/build ambiguity and fix correctness blockers.
2. **AF-009 → AF-015:** build the verification/wiring machinery before adding 12 systems to Main.
3. **AF-028 + AF-029 first inside persistence:** do not wire new save stores into live campaigns while checksum validation is defective.
4. **AF-016 → AF-027:** integrate Batch 3 in small domain PRs, one or two systems per PR maximum.
5. **AF-030 → AF-035:** harden persistence and lifecycle once all participants are known.
6. **AF-036 → AF-041:** decompose Main and remove the legacy/bridge surface without mixing gameplay changes into the same PR.
7. **AF-042 → AF-048:** finish spatial/data integration and systematic atomicity.
8. **AF-049 → AF-050:** make deterministic campaign and packaged release gates the entry criterion for new expansion implementation.

---

## 8. Per-task PR discipline

For every task:

1. Rebase on latest green `main`.
2. Run the smallest targeted test first.
3. Implement one ownership change only.
4. Add a regression test that fails before the fix whenever the task closes a bug.
5. Run Core tests.
6. Build Godot host.
7. Run targeted Godot self-test.
8. Run canonical gate before merge.
9. Inspect the diff for accidental `_Game`, generated artifact, or save-wire changes.
10. Record any save schema change and migration explicitly in the PR.

Do not combine:
- Main decomposition + gameplay feature addition;
- save-format migration + unrelated UI redesign;
- legacy removal + mechanics rebalance;
- generated-art regeneration + gameplay code unless the generation is the direct purpose of the PR.

---

## 9. Canonical verification stack after this plan is implemented

```bash
# Source authority
./scripts/ci/source-authority-gate.sh

# Core
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# Godot host
dotnet build Ashfall.csproj

# Fast PR gate
./scripts/ci/godot-asset-gate.sh

# Generated artifact freshness
python3 tools/production_manifest.py --check

# Campaign integration
godot --headless --path . -- --campaign-30day-selftest

# Full/nightly verification registry
godot --headless --path . -- --all-registered-selftests
godot --headless --path . -- --campaign-100day-selftest
```

The exact final CLI name for the generated “run all registered tests” command can differ; the requirement is that it comes from the same descriptor registry as parsing/help/CI coverage.

---

## 10. Definition of “wired” for ASHFALL

A system is **not** considered implemented merely because a Core class and unit tests exist.

A player-facing stateful system is only **WIRED** when all of the following are true:

- one authoritative Core instance exists;
- dependencies are the same live campaign instances used by connected systems;
- its authored data loads from `Assets/StreamingAssets/Data/`;
- it is constructed exactly once;
- tick ownership is explicit and exactly once;
- it captures/restores through the standardized save participant contract;
- checksum and schema version are validated;
- its actions are reachable through player UI or a deliberate automated runtime path;
- state changes mark persistence dirty without duplicate/no-op churn;
- teardown/rebind behavior is explicit;
- a headless integration test proves the full action → tick → save → reload → effect path.

This definition should be encoded by AF-015 so “unwired code” becomes mechanically detectable.

---

## 11. Do-not-rebuild list

The audit does **not** recommend rebuilding the mature foundations already present:

- deterministic engine-agnostic Core;
- existing survivor/needs/radiation/medical/economy/combat systems unless a specific bug requires change;
- existing JSON authority and catalog validator;
- established SaveChecksum algorithm unless schema guards require additive hardening;
- mature WorldSaveStore-style checksum validation pattern;
- existing expansion 01–10 logic simply to make it “newer”;
- Batch 3 Core logic that already has regression coverage—wire it rather than rewriting it.

---

## 12. Final milestone

### Milestone: **ASHFALL Godot Integration Alpha**

Exit only when:

- Unity is absent from active workflows and the Godot compile graph.
- All P0 tasks are closed.
- All 12 Batch 3 systems are either fully wired or explicitly deferred with no runtime registration.
- Every stateful participant passes checksum, version, corruption, and transactional-restore tests.
- Main has clear composition boundaries.
- Holdfast and Wasteland views are driven by live simulation state.
- The deterministic 30-day campaign harness passes with reload equivalence.
- A Godot-only packaged build launches, starts a campaign, saves, exits, and continues successfully.

Only after this milestone should Expansion 11 move from planning into substantial implementation.
