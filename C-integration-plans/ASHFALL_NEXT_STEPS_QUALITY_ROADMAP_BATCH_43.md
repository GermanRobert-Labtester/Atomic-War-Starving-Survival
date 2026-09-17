# Ashfall Next Steps Quality Roadmap — Batch 43

**Plan file:** `/home/robertsrff/Desktop/luna)plans/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_43.md`<br>
**Source inventory:** `/home/robertsrff/Desktop/ASHFALL_NEXT_STEPS_QUALITY_ROADMAP_BATCH_43.md`<br>
**Scope:** Steps 673–688<br>
**Sequence:** First of Batches 43–48; every later batch depends on the contracts and gates defined here.

## 1. Purpose and exit condition

Batch 43 is the authority-and-verification spine for the next six batches. It must not be implemented as sixteen unrelated panels. Its exit condition is a set of reusable Core contracts that make later records, craft systems, social systems, observations, and endgame presentation safe to add:

- one authoritative owner for each mutable value;
- deterministic identifiers and seeded outcomes;
- portable, versioned, checksummed save state;
- JSON catalog entries with `schema_version` and canonical `snake_case` IDs;
- typed read models for Godot presentation, including loading, empty, stale, error, and disabled states;
- explicit consent, privacy, uncertainty, correction, and interruption behavior;
- a truthful Godot-only verification baseline.

The batch is complete only when a fresh checkout can prove the contracts without Unity and the next batch can consume them without inventing a second record ledger, medical system, message store, map authority, clock, event bus, or ending writer.

## 2. Review findings that shape this batch

The current project already demonstrates the desired pattern in `Assets/Ashfall.Core/Shelter/PowerGridSystem.cs` (verified: 475 lines, Core-owned rules, injected `ISeededRng`, typed state changes, and capture/restore state). `src/Host/MedicalHostSession.cs` shows the current medical boundary; it wraps `ChemicalDependencySystem` and `VigilStateMachine` (both verified to exist at `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs` and `Assets/Ashfall.Core/Medical/VigilStateMachine.cs`), so a new "sports medicine" or spa feature must not create a historical or generic `MedicalSystem` authority. `src/Main.cs` (verified: currently **7,014 lines**, not a number this batch should quote without re-measuring) has repeated `SetupXxx`, `SaveXxx`, and `FlushXxxIfDirty` orchestration (verified: 38 Setup, 30 Save, 17 Flush methods as of this review), which means every new stateful system needs a registration matrix. `REPO_REVIEW_REPORT.md` is useful historical evidence but contains stale counts and stale bridge findings; source must be measured again — do not copy any specific number (Main.cs length, catalog count, etc.) from that file into a new plan without re-verifying it first.

**Verified naming collision risk:** a class named `RelicProvenanceCatalog` already exists (`Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs`, with tests in `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`). It is a narrow static-data lookup for "The 32 Relic Provenance Master Dossiers" (name/tone/material/curator note per relic) — it is **not** the general-purpose observation/provenance record contract (confidence, uncertainty, status, consent, revision) that §4.1 below describes, and this batch must not confuse or merge the two. However, the name collision is real: an implementer searching the codebase for "provenance" before building §4.1's contract will find this file first. Slice 43.1 must explicitly document that the new provenance contract is a different, more general concept and should be named to avoid confusion (e.g. `Ashfall.Core.Records.ObservationRecord` or similar, not `ProvenanceCatalog`).

**Verified stale CLI text:** `src/Host/HostCli.cs` (line ~306) prints "Cross-reference every id in the 55 StreamingAssets catalogs" in its `--data-integrity-selftest` help text. The actual current catalog count is **98 top-level JSON files** and **296 total JSON files** under `Assets/StreamingAssets/Data/` (including subdirectories such as `narrative/`) as of this review — the "55" figure is stale and should be corrected or removed (made non-numeric, e.g. "every catalog under StreamingAssets/Data") as part of Slice 43.0's baseline-reconciliation work, since presenting a wrong count in a user-facing CLI message is itself a small instance of the same "stale historical report" problem this batch exists to fix.

The current CI workflow files still contain Unity jobs even though Godot is the active host — **verified**: `.github/workflows/ci.yml` and `.github/workflows/build.yml` both reference `UNITY_VERSION: 6000.5.5f1`, `game-ci/unity-builder@v4`, and `game-ci/unity-test-runner@v4` for build/editmode/playmode/Linux/Windows/WebGL jobs. The intended data/asset gate is `scripts/ci/godot-asset-gate.sh` (verified to exist). `src/Host/HostCli.PanelTests.cs` and `src/Host/HostCli.cs` (both verified to exist) retain the bridge self-test as a removal notice (`--bridge-selftest` prints the removal notice and exits 0, verified at `src/Host/HostCli.cs` line ~205/298). Batch 43 must resolve the verification contradiction before later batches use a green result as evidence.

## 3. Non-goals

- Do not add gameplay logic to `Assets/_Game/`; it is the read-only legacy Unity tree.
- Do not add `JsonUtility`, `System.Random`, `Guid.NewGuid()`, a new Unity/Godot bridge, or a new save envelope per feature.
- Do not make a panel the owner of civic, medical, narrative, inventory, or endgame state.
- Do not create a second map/territory authority or a second narrative/ending writer.
- Do not hard-code a correct political, historical, scientific, or moral outcome.
- Do not use real countries, wars, people, surveillance mechanics, or glorified violence.

## 4. Shared contracts to establish first

### 4.1 Provenance and observation record

Create or extend a Core record contract after checking for an existing equivalent. **Verified naming collision:** `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs` already uses "Provenance" in its name for an unrelated, narrower concept (a static per-relic dossier lookup with no confidence/uncertainty/consent/revision semantics). Name the new contract to avoid this collision — for example `Ashfall.Core.Records.ObservationRecord` / `Ashfall.Core.Records.RecordLedger` rather than any `*ProvenanceCatalog`/`*Provenance*` name, and cross-reference `RelicProvenanceCatalog` in the new contract's doc comment so future readers do not conflate the two.

A record should contain a deterministic record ID, record kind, source/reference, observer or creator, day, location ID, confidence, uncertainty, status, consent/privacy scope, custody/attribution information where relevant, and a revision/correction link. A record must distinguish `unobserved`, `pending`, `observed`, `ambiguous`, `rejected`, and `withdrawn`; "no result" must not be serialized as a successful result.

Use explicit units and calibration metadata for science or water results. Keep human-readable text in data catalogs; keep rule state in Core. A corrected record should supersede the previous record without erasing its audit trail. IDs must be deterministic (for example, a monotonic save-owned sequence or an explicit stable key), not generated with `Guid.NewGuid()`.

### 4.2 Consent, privacy, and accessibility read models

Add a reusable capability/read-model contract rather than a UI-only toggle. It should represent requested access, available interpretation or assistance, partial communication, refusal, withdrawal, and degraded communication. Privacy scope must be enforced by the Core query, not merely hidden by a Godot control. A presentation model should expose `Loading`, `Ready`, `Empty`, `Stale`, `Error`, and `Disabled` states and a reason for disabled actions.

### 4.3 Save and event contract

Every stateful system must expose `CaptureState()` and `RestoreState()`, use a versioned DTO, reject future versions, migrate past versions, and participate in the established checksum envelope. Changes should raise typed C# events or use the host’s existing event adapter; do not introduce a third bus. Register construction, tick order, event wiring, save capture, restore order, dirty flush, and teardown in one table tied to `src/Main.cs`.

## 5. Delivery sequence

### Slice 43.0 — Reconcile the quality baseline

1. Measure current source counts for serializer call sites, catalog loaders, narrative files, stale bridge references, and host-owned gameplay instead of copying old report numbers. **Concrete method:** run `grep -rl "JsonUtility" Assets/_Game/ | wc -l` for serializer call sites (AGENTS.md currently cites "21 remaining" catalog loaders out of 28 — re-run and record the actual number, do not carry either figure forward unverified), `find Assets/StreamingAssets/Data -iname "*.json" | wc -l` for catalog count (verified 296 total / 98 top-level as of this review — supersedes the "55" figure in `HostCli.cs`'s help text), and `wc -l src/Main.cs` for host size (verified 7,014 lines as of this review).
2. Replace or quarantine Unity-only jobs in `.github/workflows/ci.yml` and `.github/workflows/build.yml` (verified: both currently define `UNITY_VERSION: 6000.5.5f1` and `game-ci/unity-builder@v4`/`unity-test-runner@v4` jobs) so CI proves the active Godot host. Preserve the stable `--bridge-selftest` verb and verify its removal notice (verified present at `src/Host/HostCli.cs` line ~205, prints the notice and exits 0 per line ~298).
   - **Risk:** This is a **high-risk change** relative to the rest of this batch — CI workflow files gate every PR merge for the whole team. Disabling or quarantining Unity jobs changes what "green" means for every contributor, not just this repo's local state. **Do not merge a CI workflow change without a separate, explicit review/approval step distinct from the code review for the Core contracts in this batch** — treat it as its own PR.
   - **Rollback:** keep the original `ci.yml`/`build.yml` in git history; reverting is a plain `git revert` of that one commit. Prefer quarantining (e.g. `if: false` guard with a comment explaining why, or moving Unity jobs to a manually-triggered `workflow_dispatch`-only job) over deleting the Unity job definitions outright, so they can be restored quickly if Unity work is explicitly requested later per AGENTS.md's own rule ("Unity is NOT a target editor... unless the user explicitly asks in that message").
3. Document a non-mutating canonical gate: Core build/test, Godot host build, data integrity, bridge self-test, and any asset gate prerequisites. Record baseline failures rather than presenting historical reports as current. **Concrete method:** run the 5 commands in Section 7 verbatim, in order, on a clean checkout, and paste the actual PASS/FAIL/error output into the batch's tracking issue — do not summarize as "should pass."
4. Add an ownership matrix for every contract introduced in this batch, including its save field and host setup/save/flush owner. **Concrete format:** a table with columns `Contract | Core type | Host session | Setup method | Save method | Flush method | src/Main.cs field name` — populated with real verified names, following the pattern corrected in Batch 49's review (do not invent field names like `_provenanceHost` without checking `src/Main.cs` first).

**Acceptance:** a CI run cannot be green solely because an inactive Unity builder passed — **testable via:** after Slice 43.0 step 2 lands, a PR that only touches `Assets/Ashfall.Core/` and `Ashfall.Core.Tests/` (no Unity-specific files) must show 0 executed Unity job steps in the CI run log, not merely a "skipped" Unity job that still reports success. Baseline failures are named and reproducible — **testable via:** the tracking issue from step 3 lists each of the 5 canonical commands with a literal PASS/FAIL and, for any FAIL, the exact error text and file/line.

### Slice 43.1 — Establish the shared records and presentation boundary

Implement the provenance/observation DTOs, deterministic ID policy, revision semantics, consent/privacy policy, accessibility capability, and typed read-model states. Add focused Core tests for correction, withdrawal, ambiguous evidence, duplicate IDs, deterministic replay, and round-trip migration. Add a thin Godot host adapter and a headless contract self-test without building feature-specific panels yet.

**Acceptance:** later features can create and query a record through one Core owner, and a withdrawn/private record is not returned to an unauthorized read model — **testable via:** a focused Core test that (a) creates a record, (b) withdraws it, (c) queries it through a read model constructed with a caller lacking the required consent/privacy scope, and (d) asserts the query returns the record's `Empty`/`Disabled` state (per the presentation-state contract in §4.2), not the underlying withdrawn data. A second test asserts a caller *with* the correct scope still cannot see a withdrawn record's live value, only its correction/audit trail — vague terms like "not returned" must be pinned to a specific assertion before this slice is marked done.

### Slice 43.2 — Civic and legal records

- **Step 673 — Legal aid:** model a request, protected case data, eligibility evidence, advocate availability, appointment/session state, refusal, appeal, and interruption. Keep outcomes evidence-based and reversible; the system must not grant an automatic “justice” result.
- **Step 683 — Electoral commission:** model registration, eligibility evidence, consent to participate, ballot/session state, quorum or term timing, absence, challenge/recount, privacy, and closure. Treat a failed quorum or withdrawn consent as a valid state, not a victory failure.

Both features consume the same civic record and consent authority. Their host sessions expose commands and read models; UI must not write directly to DTOs.

**Tests:** eligibility boundaries, duplicate registration, refusal/withdrawal, interrupted sessions, deterministic recount, save/load, checksum mutation, and future-version rejection.

### Slice 43.3 — Evidence, learning, and measured observation

- **Step 676 — Numismatics:** catalog an artifact, provenance, condition, identification confidence, and attribution; support unknown or disputed identity.
- **Step 679 — Academic conference:** represent submissions, review status, attendance/consent, accessible participation, correction, and published proceedings as records rather than a second narrative system.
- **Step 685 — Astronomy:** use calibrated observations with time, location, instrument condition, units, weather/visibility, confidence, and failed observation states.
- **Step 687 — Water sampling:** record sample source, custody, collection day, contamination risk, test method, units, detection limits, uncertain result, and safe-use interpretation. A failed or contaminated sample must not silently become clean water.

These features should share a knowledge/observation registry. Results must be queryable by the later ecology, health, water, and history batches without copying data into feature-local ledgers.

**Tests:** units and bounds, contaminated samples, missing calibration, custody breaks, ambiguous artifact identification, inaccessible attendance, record correction, deterministic observations, and cross-host JSON equality.

### Slice 43.4 — Utility, agriculture, craft, and care adapters

- **Step 674 — Tinsmithing:** reuse inventory/material and recipe authorities; account for sheet material, heat/work time, tool wear, quality, scrap, and interruption.
- **Step 675 — Seed saving:** connect crop/seed state to the existing agriculture/data authority; track variety, viable quantity, contamination, season, storage, and selection without free duplication.
- **Step 678 — Saddlery:** reuse leather/material inventory and equipment maintenance; model quality, repairability, labor, and failed work.
- **Step 682 — Bioluminescent tunnels:** make this a location/observation/ecology capability with visibility, habitat condition, exposure, access, and uncertainty—not a fantasy resource generator.
- **Step 684 — Garden tools:** use a canonical tool/material catalog with durability, repair, labor, and effect on existing agriculture.
- **Step 686 — Broom/brush:** use the same material and maintenance rules, with hygiene effects routed through existing needs rather than a parallel cleanliness system.
- **Step 677 — Grotto spa:** integrate with existing needs, hygiene, chemical dependency, vigil, disease, and medical host surfaces. Model access, water quality, staffing, treatment duration, consent, and adverse/empty states.
- **Step 681 — Sports medicine:** extend existing medical effects and care records with assessment, treatment, rest, referral, and uncertainty. Do not create `MedicalSystem.cs` as a new authority.
- **Step 680 — Printing telegraph:** use one communication/message persistence contract; account for power/materials, delivery delay, legibility, access, and loss. Do not make a scene-local message list.

**Tests:** material conservation, tool wear, contaminated water, treatment refusal, dependency interaction, message loss/delay, interruption, save/restore, and no direct host mutation.

### Slice 43.5 — Official history, last

- **Step 688 — Official history:** implement a read-only synthesis over provenance, civic, artifact, scientific, and communication records. It must show source attribution, uncertainty, corrections, privacy filtering, gaps, and competing interpretations. It may produce an archive view or derived summary, but it must not rewrite source records or own an ending.

Use this feature as the proof that the shared record contract works. Test incomplete evidence, conflicting accounts, withdrawn material, correction history, and deterministic ordering of records.

**Acceptance (tightened):** `Official history` must ship with at least one automated test that (a) feeds it two conflicting records for the same subject and asserts both are surfaced with their respective confidence/source rather than one silently winning, (b) withdraws one of its source records mid-test and asserts the derived view updates on next query without needing an explicit "refresh" call, and (c) asserts calling the read-only query method twice in a row with no intervening writes produces byte-identical output (idempotence) and leaves every source record's own `RestoreState`-observable state unchanged (no silent mutation) — "shows source attribution, uncertainty, corrections, privacy filtering, gaps, and competing interpretations" is not testable as written until pinned to concrete assertions like these.

## 6. Likely implementation surfaces

These are planning targets, not permission to invent duplicate authorities; reconcile names with the current tree before implementation:

- `Assets/Ashfall.Core/` — shared records/provenance, civic, accessibility, observation, communication adapters, craft/care extensions, DTOs, migrations, and events.
- `Assets/StreamingAssets/Data/` — snake_case IDs, catalog definitions, schema versions, accessibility/civic/observation text, and no dual-authority ScriptableObjects.
- `src/Host/` — thin session/read-model adapters and headless self-test commands.
- `src/Main.cs` — explicit Setup/Save/Restore/Flush registration only; no new rule logic.
- `src/Host/HostCli.cs` and `src/Host/HostCli.PanelTests.cs` — stable self-test routing, including bridge removal verification.
- `.github/workflows/ci.yml`, `.github/workflows/build.yml`, and `scripts/ci/godot-asset-gate.sh` — active-host verification alignment.
- `Ashfall.Core.Tests/` — contract, migration, determinism, privacy, conservation, and checksum tests.

## 7. Required verification

Run the canonical project checks after implementation, without Unity:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

Also run focused self-tests for records, civic workflows, observations, communication, and medical integration. If a new CLI verb is added, it must be deterministic, exit nonzero on a contract violation, and not require a graphical editor. Do not use `scripts/ci/godot-asset-gate.sh` as a supposedly read-only check unless its repository setup/import side effects are understood and accepted.

## 8. Batch acceptance checklist

- [ ] Current source baseline is measured fresh (not copied from `REPO_REVIEW_REPORT.md` or this plan's own Section 2) — **testable via:** the tracking issue for Slice 43.0 quotes the literal output of `wc -l src/Main.cs`, `find Assets/StreamingAssets/Data -iname "*.json" | wc -l`, and `grep -rl "JsonUtility" Assets/_Game/ | wc -l`, each timestamped to this batch's work, not inherited from an older report.
- [ ] CI proves Godot/Core rather than inactive Unity — **testable via:** a CI run log for a Core-only PR shows the Unity job either absent or explicitly skipped-by-design (not executed-and-passed), verified by inspecting the actual job status in the CI provider's UI/API, not just reading the workflow YAML.
- [ ] Provenance, observation, consent/privacy, accessibility, and read-model contracts exist exactly once each — **testable via:** `grep -rn "class.*Provenance\|class.*ObservationRecord\|class.*ConsentScope\|class.*AccessibilityCapability" Assets/Ashfall.Core/` returns exactly one authoritative class per concept (plus the pre-existing, deliberately-distinct `RelicProvenanceCatalog`, which must be named and referenced clearly enough that it is not miscounted as a second general-purpose contract).
- [ ] Steps 673–688 are all implemented and each one's host session/Core class is confirmed (by name, via `grep`) not to duplicate an existing civic/medical/observation/communication authority listed in this plan's Section 2 and AGENTS.md's Known Issues.
- [ ] Every stateful system introduced in this batch has versioned capture/restore, checksum coverage, migration tests, deterministic IDs, and typed state-change events — **testable via:** each new system's test file contains at minimum the four-test pattern already established in this codebase (`CaptureRestore_RoundTrip`, `Save_RoundTrip_ChecksumStable`, `Save_TamperedChecksumRejected`, `Save_EmptyChecksumRejected` — see `Ashfall.Core.Tests/Shelter/PowerGridSystemTests.cs` for the reference shape) plus one future-version-rejection migration test.
- [ ] JSON data introduced in this batch is authoritative, snake_case, and schema-versioned, and `godot --headless --path . -- --data-integrity-selftest` reports 0 errors against it.
- [ ] Medical-adjacent work (Steps 677, 681) routes through the existing `ChemicalDependencySystem`/`VigilStateMachine`/`MedicalHostSession` boundary — **testable via:** `grep -rn "class MedicalSystem\b" Assets/Ashfall.Core/` returns no results (no new generic `MedicalSystem.cs` authority was created).
- [ ] Official history (Step 688) is derived/read-only — **testable via:** the Step 688 acceptance tests defined in Section 5, Slice 43.5 above (conflicting-records, withdrawal-propagation, idempotence-with-no-mutation) all pass.
- [ ] Core, host, data, bridge, and focused self-tests pass, or failures are explicitly recorded with exact command + error text (per Slice 43.0 item 3) — no failure may be silently omitted from the batch's closing report.

## 9. Handoff to Batch 44

Batch 44 may start only after it can consume: the provenance/observation registry; accessibility and consent read models; the communication/message contract; the craft/agriculture/material ownership map; the water-sampling interpretation contract; the portable save/checksum pattern; and the corrected Godot verification baseline. Batch 44 must extend these contracts rather than create another infrastructure, ecology, wellbeing, or archive authority.


## Review Notes (Corrected)

This batch plan was adversarially reviewed against the real repository at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Most of this plan's file/class references verified correctly (`Assets/Ashfall.Core/Shelter/PowerGridSystem.cs`, `src/Host/MedicalHostSession.cs`, `Assets/Ashfall.Core/Medical/ChemicalDependencySystem.cs`, `Assets/Ashfall.Core/Medical/VigilStateMachine.cs`, `src/Host/HostCli.cs`, `src/Host/HostCli.PanelTests.cs`, `scripts/ci/godot-asset-gate.sh`, `REPO_REVIEW_REPORT.md`, and both `.github/workflows/*.yml` files with active Unity jobs all exist exactly as described). The following issues were found and fixed in place:

1. **Unverified/stale numeric claims not caught by the original plan.** The plan correctly warned that `REPO_REVIEW_REPORT.md` "contains stale counts" but did not itself supply corrected numbers. Added verified figures: `src/Main.cs` is **7,014 lines** (not to be quoted as any other number without re-measuring); it has **38 Setup, 30 Save, 17 Flush methods** as of this review. `Assets/StreamingAssets/Data/` contains **98 top-level JSON files, 296 total** — this directly contradicts the "55 StreamingAssets catalogs" figure baked into `src/Host/HostCli.cs`'s own `--data-integrity-selftest` help text (line ~306), which is itself stale and should be corrected as part of Slice 43.0's baseline work.
2. **Real naming-collision risk not flagged by the original plan.** `Assets/Ashfall.Core/Narrative/RelicProvenanceCatalog.cs` already exists (with tests in `Ashfall.Core.Tests/RelicProvenanceCatalogTests.cs`) and uses "Provenance" in its name for a narrow, unrelated concept (static relic-dossier lookup, no confidence/consent/revision semantics). The plan's §4.1 instructs implementers to "check for an existing equivalent" but never actually performs that check itself or names the collision. Added an explicit naming disambiguation note in §4.1 and Section 2, recommending a distinct name (`ObservationRecord`/`RecordLedger`, not `*ProvenanceCatalog`) so a future grep for "provenance" does not lead an implementer to conflate the two systems.
3. **High-risk step lacked risk/rollback guidance.** Slice 43.0 step 2 instructs editing `.github/workflows/ci.yml` and `.github/workflows/build.yml` — verified both currently gate CI on Unity 6000.5.5f1 builds via `game-ci/unity-builder@v4`/`unity-test-runner@v4`. This is a repository-wide, hard-to-reverse-quickly change (it changes what "green" means for every contributor's PRs) and the original plan gave it the same treatment as every other bullet point. Added an explicit high-risk callout requiring the CI change to be its own reviewed PR, a preference for quarantining (`workflow_dispatch`-gated or `if: false`-guarded) over deleting Unity job definitions, and a concrete rollback path.
4. **Vague, unfalsifiable acceptance criteria tightened.** Multiple acceptance lines used language that cannot be mechanically checked: "contracts exist once," "represented without duplicate authorities," "cannot become a second ending writer," "shows source attribution, uncertainty, corrections... and competing interpretations." Each was rewritten with a concrete verification method (a specific `grep` pattern, a specific test scenario, or a specific CI-log inspection step) in Section 8 and in Slice 43.5's acceptance line.
5. **Confirmed accurate, not changed:** the plan's caution that `scripts/ci/godot-asset-gate.sh` should not be treated as a read-only check is correct — verified the script runs `setup-repo.sh` and `godot --headless --path . --import`, both of which mutate the working tree / `.godot/` cache. No fix needed here; flagged as a correctly-identified risk.
6. **Confirmed accurate, not changed:** Step 681's non-goal ("Do not create `MedicalSystem.cs` as a new authority") is well-founded — verified no `MedicalSystem.cs` exists anywhere in the repository; the only medical authorities are `MedicalHostSession.cs`, `ChemicalDependencySystem.cs`, and `VigilStateMachine.cs`. Added a `grep`-based acceptance check in Section 8 to keep this true going forward.
7. **Step ordering:** verified as logically sound — Slice 43.0 (baseline) → 43.1 (shared contracts) → 43.2–43.4 (features consuming those contracts) → 43.5 (read-only synthesis over everything else) has no forward dependency on work not yet built. No reordering was needed.
