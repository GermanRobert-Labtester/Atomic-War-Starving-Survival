# ASHFALL Repository Audit & Codex Execution Plan

**Repository:** `GermanRobert-Labtester/Atomic-War-Starving-Survival`
**Audit date:** 2026-09-19 (Europe/Riga)
**Audited default branch:** `main`
**Audited `main` HEAD:** `166fa9aebb75a2310b15cb47dc5cc9b8a270e2b6`
**Primary execution target:** Codex acting as the repository integrator/repair agent
**Document character target:** 120,000–150,000 characters
**Scope:** current repository correctness, CI truth, integration blockers, deterministic host behavior, generated-authority drift, branch/merge safety, and bounded follow-up integration work.
**Non-goal:** inventing new game mechanics, overriding decision-blocked product choices, reviving Unity-era architecture, or opportunistically rewriting unrelated systems.

---

## 0. Executive audit verdict

The repository is **buildable and exportable, but the canonical verification state is red**. At the audited `main` HEAD, the `Build ASHFALL` workflow succeeded for both Windows Desktop and Linux/X11 export paths. The separate `ASHFALL CI` workflow failed in its fast-tier run only after **49 of 50 selected fast gates passed**. The failing gate is `port_contract_gate`, and the failure is deterministic: `docs/architecture/port-contract.json` is stale relative to the current policy/generator.

That immediate failure is easy to repair, but the audit found a more important correctness defect underneath it: the current port-contract caller verifier does not prove class-specific wiring. It searches all `src/**/*.cs` text for the *method name alone*. This produces systematic false positives for generic method names. Current generated evidence contains **22 colliding method names affecting 108 seams, including 103 `HOST_REQUIRED` seams**. For example:

- 27 unrelated `*.Register` seams all report the same 24 caller files.
- 22 unrelated `*.BindCatalog` seams all report the same 19 caller files.
- 10 unrelated `*.BindInventory` seams all report the same 7 caller files.
- 5 unrelated `*.Bind` seams all report the same 261 caller files.
- 5 unrelated `*.RegisterRange` seams all report the same 7 caller files.

Therefore the statement “176 `HOST_REQUIRED` seams have production callers” is currently a **token-level claim, not a symbol-level proof**. The gate is valuable, but until this is fixed it can certify a missing integration because another class happens to expose/call the same method name.

A second repository-state defect is a **stranded merged PR**. PR #55, `refactor(ports): remove dead crafting seam and activate Ice Road registration`, was merged into the feature branch `chore/port-contract-ratchet-wave2` approximately 18 seconds after that base branch had already been merged to `main` as PR #53. Comparing PR #55’s head with current `main` shows divergence; its semantic code changes are not on `main`. The current source still contains `CraftingSystem.BindCraftResultGate`, and `IceRoadSystem.RegisterDefaultHoldfastNodes()` still bypasses `RegisterHoldfastNode`. The correct repair is to reapply the *semantic delta* from PR #55 on top of fresh `main`, not to merge the stale branch history.

A third systemic issue is merge governance. The active repository ruleset enforces pull requests, review, non-fast-forward/deletion protections, code quality, and CodeQL, but it does **not require the ASHFALL CI/build status checks to pass before merge**. The connected repository owner can also bypass the ruleset. This makes a red `main` possible even when CI correctly detects a defect. Repository protection should be changed only after the current gate is green, and should require the canonical checks while preserving an explicitly documented emergency bypass procedure.

The audit also found deterministic-host and test defects:

- `src/World/SurvivorActorView.cs` selects a visual variant using `Math.Abs(_survivorId.GetHashCode())`. .NET string hashing is process-randomized, so the same survivor can map to a different visual variant after restart. `Math.Abs(int.MinValue)` can also throw. The host already has deterministic `StableHash`; use an unsigned modulo over that stable value.
- `Ashfall.Core.Tests/Collectibles/CollectibleBalanceCharacterizationTests.cs` seeds a characterization RNG from `t.id.GetHashCode()`, so a test described as deterministic can vary between processes.
- `Ashfall.Core.Tests/AudioEventIntegrationTests.cs` also contains a `GetHashCode()`-based test helper. Test determinism must use the same stable hash discipline as production characterization.
- Open issue #52 correctly identifies hard-coded host `DemoSeed` values that are independent of the canonical campaign seed. Fresh audit searches found additional production-seed candidates that do not use the `DemoSeed` identifier, including Counter Intelligence, Recon Telemetry, Year of Ash warlord RNG, and several host/default construction paths. These must be classified by call path before conversion; not every hard-coded seed is wrong, because some are isolated fixtures/selftests/default fallbacks.
- `Ashfall.csproj` comments state that `CS8603` is **not suppressed**, but the actual `<NoWarn>` list includes `CS8603`. This is both documentation drift and a nullability-safety blind spot.

Several authority/generator documents are also stale or self-consistent for the wrong reason:

- `KNOWN_DEBT.md` and `docs/plans/UNCLAIMED_CORPUS_CENSUS.md` still describe the earlier Plan 36 state of 17 deferred seams / 34 `LIVE_VIA_CORE`, while the current policy is 6 deferred / 40 `LIVE_VIA_CORE` / 5 `OPTIONAL_HOST`.
- `docs/CURRENT_AUTHORITY.md` still says the fast runner mirrors “all 14 CI gates” and elsewhere records older 47/47 metrics. The current manifest declares **53 total gates and 50 fast-tier gates**.
- `AGENTS.md`/auto-generated `CODEX.md` still contain an “ACTIVE HANDOFF — AGY: C1 UI PANEL WAVE”, while the sole live `INTEGRATION_PLANS.md` current batch is `XP-WAVE1-DIFFICULTY-AUTHORITY`. Since the rules themselves say `INTEGRATION_PLANS.md` owns current package truth, the handoff prose is stale and should be generated or removed.
- `scripts/ci/generate-architecture-map.py` currently classifies `weather_hardening` as `ticked: False / On-Demand`, but source truth in `src/Main.ExpandedShelterSystems.cs` calls `_weatherHardening?.TickDay(day)`. The repository’s older reconnaissance claim that Weather Hardening is never ticked is stale. **Do not add another tick.** Fix the architecture evidence generator instead.
- `scripts/ci/generate-port-contract.py` hard-codes `"generated_at": "2026-09-18"`, so the generated manifest contains a date that is no longer meaningful.
- The generator reads `policy["total_seams"]` into a variable but does not enforce that it equals `len(ports)`.
- The port policy identifies methods by `Class.Method`, so overloaded public integration methods cannot be represented distinctly.
- The docs-regeneration workflow does not invoke the port-contract generator and does not trigger on the port policy, which is why this generated authority can drift outside the automated regeneration path.

The repository nevertheless has a strong verification foundation. The current failing run proves that the first 49 fast gates—including builds, data integrity, host selftests, save/load checks, campaign journey checks, source policy gates, generated-catalog checks, focused UI/audio/fuzz gates, coverage, and content acceptance—were green before the final port-contract mismatch. This plan preserves those controls and strengthens the few verification layers that currently overclaim.

---

## 1. Evidence ledger — verified findings

### F-001 — P0 — Current `main` fails canonical fast CI

**Evidence:** `ASHFALL CI` run #197 (`35397919879`) failed in job `105770879893`. The final selected fast gate, `port_contract_gate`, reported:

`Port contract artifacts out of date: .../docs/architecture/port-contract.json`

The build/export workflow at the same HEAD succeeded. Therefore this is not a compile/export failure; it is a generated-contract drift failure.

**Exact stale entries:** policy ownership changed but the generated `port_id` retained the old `core-architecture` owner prefix for:

1. `EquipmentConditionSystem.RegisterProfile`
   current JSON: `core-architecture.equipmentconditionsystem.registerprofile`
   expected from policy/generator: `equipment.equipmentconditionsystem.registerprofile`
2. `ExpeditionLootReferenceResolver.RegisterCategory`
   current JSON: `core-architecture.expeditionlootreferenceresolver.registercategory`
   expected: `expeditions.expeditionlootreferenceresolver.registercategory`
3. `ExpeditionNavalSystem.RegisterVessel`
   current JSON: `core-architecture.expeditionnavalsystem.registervessel`
   expected: `expeditions.expeditionnavalsystem.registervessel`
4. `ShelterThermalSystem.RegisterThermalGear`
   current JSON: `core-architecture.shelterthermalsystem.registerthermalgear`
   expected: `shelter.shelterthermalsystem.registerthermalgear`
5. `SurvivorDowntimeSystem.RegisterHobby`
   current JSON: `core-architecture.survivordowntimesystem.registerhobby`
   expected: `survivors.survivordowntimesystem.registerhobby`

**Repair principle:** regenerate; never hand-edit those five strings.

### F-002 — P0 — Port caller verification is method-name-only and unsound

`find_callers_in_src(method_name, src_texts)` constructs `\b<method>\s*\(` and scans every source file. It has no receiver type, variable binding, symbol, namespace, overload, or class qualification.

Current collision metrics from the generated manifest:

| Collision family | Seams | HOST_REQUIRED | Reported caller files shared by every seam |
|---|---:|---:|---:|
| `Register` | 27 | 27 | 24 |
| `BindCatalog` | 22 | 22 | 19 |
| `BindInventory` | 10 | 10 | 7 |
| `Bind` | 5 | 5 | 261 |
| `RegisterRange` | 5 | 5 | 7 |
| `RegisterDefinition` | 3 | 3 | 2 |
| `RegisterCatalog` | 3 | 3 | 2 |
| `RegisterSurvivor` | 3 | 3 | 1 |
| `RegisterAll` | 3 | 3 | 3 |
| plus 13 smaller collision families | — | — | — |

Overall: **108 seams** use a colliding method name, **103 of them `HOST_REQUIRED`**.

This is not merely theoretical. Identical caller arrays across unrelated types prove that the current evidence is method-token attribution.

### F-003 — P0 — PR #55 is semantically stranded off `main`

PR #55:
- title: `refactor(ports): remove dead crafting seam and activate Ice Road registration`
- base: `chore/port-contract-ratchet-wave2`
- base SHA: `197b81905e21e59a24898544456c6cfa42c8fc86`
- head SHA: `9ea2635ba5f7ddc803eb8290410cfcec79ea3ad6`
- merged at: 2026-09-18 21:40:22 UTC

PR #53 merged the base branch to `main` at 21:40:04 UTC, before #55 merged into that branch.

Current `main` still contains:
- `_isCraftResultAllowed`
- `CraftingSystem.BindCraftResultGate(...)`
- direct `_holdfastNodes.Add(CutNodeIds[i])` in default Ice Road registration.

Do not merge the stale branch. Reapply its reviewed semantic delta on fresh main and regenerate contracts.

### F-004 — P0/P1 — Ruleset does not require ASHFALL checks

The active ruleset `Managment of repo` protects all branches and includes deletion/non-fast-forward protection, code quality, CodeQL, and PR review. It has no rule requiring `ASHFALL CI` or `Build ASHFALL` statuses. The connected owner can bypass.

**Consequence:** correctness gates are advisory at merge time. Make them required after they are repaired and stable.

### F-005 — P0/P1 — Port contract is omitted from docs auto-regeneration

`.github/workflows/docs-regen.yml` regenerates catalog registry, audio, agent skills, core systems, UI panel catalog, expansions, save-store matrix, and docs index. It does **not** run `generate-port-contract.py`. Its path triggers also do not include the port policy.

The repo rule “do not modify generated outputs by hand” therefore lacks an automated owner for this generated output.

### F-006 — P1 — Active authority documents disagree

- `INTEGRATION_PLANS.md` says the active current batch is `XP-WAVE1-DIFFICULTY-AUTHORITY`.
- `AGENTS.md` and generated `CODEX.md` still advertise `ACTIVE HANDOFF — AGY (Antigravity): C1 UI PANEL WAVE`.
- `KNOWN_DEBT.md` Plan 36 row uses pre-ratchet counts.
- `UNCLAIMED_CORPUS_CENSUS.md` Plan 36 row uses pre-ratchet counts.
- `docs/CURRENT_AUTHORITY.md` uses obsolete gate-count language.

This is operational risk because agents are explicitly instructed to consume these documents as authority.

### F-007 — P1 — `CS8603` is suppressed despite the project comment claiming otherwise

`Ashfall.csproj` states:

`CS8602/CS8603 — NOT suppressed: real null-dereference detection active`

but the actual `<NoWarn>` list contains `CS8603`.

Treat this as a safety-budget defect. First repair the comment/invariant test, then remove the suppression only after measuring/fixing the resulting warnings.

### F-008 — P1 — Survivor visual identity uses randomized process hash

`src/World/SurvivorActorView.cs`:

`int index = Math.Abs(_survivorId.GetHashCode()) % CharacterVariants.Length;`

Effects:
- survivor variant is not stable across process restarts;
- snapshots/visual identity can drift for identical saves;
- `Math.Abs(int.MinValue)` is an overflow edge case.

Use `StableHash.Of` and unsigned modulo.

### F-009 — P1 — “Deterministic” characterization tests use `string.GetHashCode()`

`CollectibleBalanceCharacterizationTests` seeds with `t.id.GetHashCode()`. Replace with `StableHash.Of(t.id)`. Do the same for the audio test helper. Add a test/source gate so deterministic tests cannot regress to process-randomized string hashing.

### F-010 — P1 — Canonical campaign RNG is not consistently propagated through host composition

Open issue #52 is valid. Confirmed hard-coded `DemoSeed` hosts include World, Expedition, Narrative, Radio, Maritime, Dose Ledger, Economy, Deep Coast, and Combat-related defaults. Additional hard-coded seed candidates appear in Counter Intelligence, Recon Telemetry, Year of Ash warlord RNG, Verdict, Airlock/Shelter infrastructure and host default constructors.

Not every occurrence is a defect. Fixtures, selftests, deterministic fallbacks used only when no campaign exists, and pure presentation samples may intentionally use constants. Codex must classify call reachability before replacing a seed.

### F-011 — P1 — Architecture-map cadence truth is stale

Source calls `_weatherHardening?.TickDay(day)` in `Main.ExpandedShelterSystems.cs`, while `generate-architecture-map.py` records Weather Hardening as `ticked: False`, `On-Demand`.

Repair the generator/source mapping. Do not introduce a duplicate weather-hardening tick.

### F-012 — P1 — Port policy metadata can drift silently

`policy.total_seams` is read but not validated. Add exact equality with `len(ports)` or remove the redundant property.

### F-013 — P2 — Generated timestamp is hard-coded

The port generator emits `"generated_at": "2026-09-18"`. This is stale metadata. Prefer a deterministic schema/policy revision, source commit field supplied externally, or no date at all.

### F-014 — P1 — Regex seam discovery cannot safely model overloads/symbols

Current seam identity is `Class.Method`; a dictionary keyed this way collapses overloads. The parser is also regex/brace-stack based. This is acceptable as an inventory bootstrap but not strong enough to be a permanent “unbound effects must fail CI” proof.

The target state is a small Roslyn-based analyzer or another compiler-backed symbol inventory. If introducing a Roslyn tool is too large for the immediate repair, land a bounded receiver-aware intermediate implementation first and keep the old gate explicitly labeled as heuristic until symbol proof lands.

### F-015 — P1 — Expensive fast gates execute before cheap drift failure

The failing port-contract check runs at the end of the 50 fast gates. The repository spent almost the complete fast run before discovering a deterministic text-generation mismatch. Reorder cheap, dependency-free static/drift gates ahead of Godot imports and runtime selftests while preserving dependency semantics.

### F-016 — P1 — Current authority metrics are stale despite generated-doc gates

`docs/CURRENT_AUTHORITY.md` is partly hand-maintained and records obsolete gate counts. This shows that passing generated docs-index drift does not guarantee semantic freshness of authority prose. Move volatile counts to generated snippets or eliminate them from hand-maintained authority text.

### F-017 — P2 — Deferred seam taxonomy still mixes three different states

Current six deferred seams are:
- `CraftingSystem.BindCraftResultGate`
- `IceRoadSystem.RegisterHoldfastNode`
- `ShelterThermalSystem.RegisterExternalBurst`
- `SkillProgressionSystem.RegisterDefaultSkills`
- `SpiritualMeaningCoordinator.RegisterDeath`
- `StealthSystem.RegisterWeaponNoise`

These are not one kind of debt:
- PR #55 already decided the first two.
- `RegisterDefaultSkills` is documented as a legacy/zero-op fallback after JSON skill authority.
- the final three represent real potential integration contracts.

Split “compatibility shim”, “dead seam pending deletion”, and “unwired production behavior” instead of keeping all under generic DEFERRED wording.

### F-018 — P2 — Diagnostic merge history reveals base-branch hygiene weakness

A one-shot diagnostic PR labelled “do not merge” was merged during the repair sequence, although its temporary workflow is not present in current `main`. Treat this as process evidence: diagnostic branches/workflows must be disposable, and PRs must target the protected integration branch directly unless a stacked-PR protocol is explicitly active and enforced.

---

## 2. Non-negotiable execution doctrine for Codex

1. **Start from current source, not this audit’s assumptions.** At the beginning of every work package, re-read the touched files and current `main`/integration head. If evidence changed, record the delta and adapt the package.
2. **Honor worktree claims.** The audit found active claims, including `claim-xp-wave1-difficulty-2026-09-18` and `claim-wave11-part2-execution-2026-09-18`. Port policy/tests are currently within the Wave 11 Part 2 candidate surface. Several campaign composition files are inside XP Wave 1 ownership. Codex must not race those owners.
3. **User authorization is not permission to trample active claims.** Before edits, the foreman/integrator must transfer, close, or explicitly overlap-authorize the exact paths in `WORKTREE_OWNERSHIP.md`.
4. **Godot is authoritative. Unity remains retired.**
5. **Core remains engine-free.**
6. **JSON remains authored-data authority.**
7. **One authority per concern.** Never fix an unwired effect by adding a parallel subsystem.
8. **Generated outputs are generated.** Never patch `PORT_CONTRACT.md`, `port-contract.json`, architecture maps, docs index, catalog registries, or save matrices by hand.
9. **Focused tests first.** Package tests stay narrow; full verification is an integrator closeout action.
10. **No speculative product decisions.** Plan 30 runtime horizon/consequence destination, Plan 32 graph-travel composition, Plan 34 history/chronicle semantics, amputation equipment schema, and other decision-blocked items remain blocked until a signed decision exists.
11. **No “compile green = integrated” claims.** Every integration package must show owner → host composition → persistence if stateful → observable consumer → focused test evidence.
12. **Preserve unrelated dirty state.** Never reset, clean, mass-format, or rewrite history to simplify the task.
13. **Do not merge stale stacked branches.** Reapply semantic commits on a fresh branch based on the actual integration head.
14. **Do not weaken gates to make green.** A failing gate is fixed at its source contract; never downgrade `critical`, blanket-allow paths, or broaden a suppression merely to pass CI.
15. **When a gate itself is unsound, repair the proof before trusting its green result.**

---

## 3. Target end-state

Codex may declare this audit remediation complete only when all of the following are true:

- current integration head has zero known stale generated port artifacts;
- `port_contract_gate` verifies class/symbol-specific caller evidence, not method-name token evidence;
- the stranded PR #55 behavior is either cleanly re-landed or explicitly rejected with source evidence;
- generated port outputs have an automated regeneration/check owner;
- protected-branch policy requires the canonical build/CI checks, unless the user explicitly declines that repository-setting change;
- no production survivor visual selection or deterministic characterization seed uses randomized `string.GetHashCode()`;
- production campaign stochastic systems consume canonical campaign-derived streams or have documented non-campaign fixture/fallback classification;
- the nullability suppression comment and actual compiler settings agree, and `CS8603` has a measured remediation path;
- generated architecture evidence reflects the actual Weather Hardening daily tick;
- `AGENTS.md`/`CODEX.md`, `INTEGRATION_PLANS.md`, `KNOWN_DEBT.md`, `UNCLAIMED_CORPUS_CENSUS.md`, and `CURRENT_AUTHORITY.md` no longer contradict current execution truth in volatile fields;
- decision-blocked product work remains decision-blocked rather than being silently invented;
- focused tests for every changed package pass;
- final integrator verification passes the canonical fast suite, full Core xUnit gate, build/export-relevant checks, and any newly added source/generator gates;
- post-merge `main` itself is re-verified, not just the PR head.

---

# 4. Codex work packages

## W00 — Acquire integrator ownership and freeze the audit baseline

**Priority:** P0
**Dependencies:** none
**Premise/evidence:** `WORKTREE_OWNERSHIP.md` currently has active claims over the port-contract candidate surface and XP Wave 1 campaign composition files.
**Primary paths:** `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`; read-only inspection of all files named by later packages.

### Execution

1. Fetch/rebase onto the actual current integration head; record `git rev-parse HEAD`, branch name, `git status --short`, and any pre-existing modifications.
2. Read `AGENTS.md`, `CODEX.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, `TEST_POLICY.md`, `KNOWN_DEBT.md`, and `AI_AGENT_WORKFLOW.md` in that order.
3. Create one bounded audit-remediation claim owned by the integrator. Do not claim XP Wave 1 paths until that owner closes/transfers them.
4. Partition later packages into disjoint path groups. Shared governance, CI manifests, policy files, generated docs, and composition roots remain integrator-only.
5. Capture the current CI/build URLs or run IDs in the implementation log so later green results can be compared against the audited baseline.

### Verification

No broad tests. This is coordination/evidence only. Validate that the claim itself does not overlap another ACTIVE claim without an explicit transfer.

### Acceptance

Exact-path ownership is unambiguous; no code has changed; baseline HEAD and dirty-state evidence are recorded.

### Stop / rollback boundary

If the required shared paths remain owned by another active builder, do not edit them. Hand off the requested transfer instead.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W01 — Restore `main` port-contract artifact consistency without hand editing

**Priority:** P0
**Dependencies:** W00
**Premise/evidence:** Fast CI currently fails only because generated `port-contract.json` is out of date. Five owner-derived IDs are stale.
**Primary paths:** `scripts/ci/generate-port-contract.py` (read only unless a separately justified generator fix is needed), `docs/ci/port_contract_policy.json` (read only in this package), generated `docs/architecture/port-contract.json`, generated `docs/architecture/PORT_CONTRACT.md`.

### Execution

1. On a fresh branch from the current integration head, run `python3 scripts/ci/generate-port-contract.py` exactly once.
2. Inspect the diff before staging. At the audited state, expect the five stale owner-derived `port_id` values to change and no unrelated semantic policy changes.
3. Run `python3 scripts/ci/generate-port-contract.py --check` and `python3 scripts/ci/run-gates.py --gate port_contract_gate`.
4. Run the focused `PortContractGateTests.cs` test file, but do not treat its green result as symbol-proof yet; W03/W04 repair that limitation.
5. Commit the generated repair as an isolated commit so it can be reviewed/reverted independently of later gate hardening.

### Verification

`python3 scripts/ci/generate-port-contract.py --check`; `python3 scripts/ci/run-gates.py --gate port_contract_gate`; `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PortContractGateTests.cs`.

### Acceptance

Current generator produces byte-identical artifacts on a second run; the dedicated gate passes; diff contains no manual edits.

### Stop / rollback boundary

If regeneration changes more than expected, inspect the policy/source delta first. Do not accept broad generated churn without explaining every class of change.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W02 — Re-land the semantic content of stranded PR #55 on fresh main

**Priority:** P0
**Dependencies:** W01
**Premise/evidence:** PR #55 merged into an already-merged feature branch and is not present on current `main`.
**Primary paths:** `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`, `Assets/Ashfall.Core/IceRoadSystem.cs`, `docs/ci/port_contract_policy.json`, generated port-contract outputs, focused Crafting/Ice Road/port tests.

### Execution

1. Read PR #55 patch as evidence, but implement against current source rather than cherry-picking the stale merge commit.
2. Re-prove `BindCraftResultGate` has no production caller and no current behavior depends on `_isCraftResultAllowed`. If still dead, remove the field, binder, and the associated unused conditional logic exactly as the reviewed PR intended.
3. Re-prove `RegisterHoldfastNode` is the correct validation/deduplication entry point. Change default Holdfast registration to call it rather than directly mutating `_holdfastNodes`.
4. Update policy classification/removal from source truth, then regenerate artifacts. Never pre-write expected seam counts; allow the generator to derive them.
5. Run Crafting, Ice Road/Holdfast, and PortContract focused tests. Add a regression proving default nodes pass through the same public registration invariants as externally registered nodes.
6. Document why the stale branch itself was not merged.

### Verification

Focused Crafting tests; focused IceRoad/Holdfast tests; `PortContractGateTests.cs`; generator `--check`.

### Acceptance

Current source expresses the PR #55 behavior on top of current main; no stale branch history is merged; generated contract is consistent.

### Stop / rollback boundary

If `BindCraftResultGate` acquired a real consumer since the audit, stop deletion and classify/wire it based on current authority instead.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W03 — Replace method-name-only host caller attribution with symbol-aware evidence

**Priority:** P0
**Dependencies:** W01,W02
**Premise/evidence:** 103 HOST_REQUIRED seams share colliding method names. Identical caller arrays across unrelated classes prove false attribution.
**Primary paths:** `scripts/ci/generate-port-contract.py` initially; preferably a new small analyzer under `scripts/ci/` or `tools/`; `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs`; package/dependency files only if a compiler-backed analyzer is approved.

### Execution

1. Write a failing regression fixture first: define two Core classes with the same integration method name; call only one from synthetic host source; assert the other is reported unbound.
2. Choose implementation path A (preferred): compiler-backed C# symbol analysis using Roslyn/MSBuild, resolving invocation target symbols and matching declaring type + method signature. Keep this tool deterministic and runnable on Linux CI.
3. If dependency cost blocks path A, implement path B as an explicitly temporary receiver-aware scanner: discover variable/field declarations and direct constructor/member calls sufficiently to distinguish class ownership. Label output `heuristic` and retain a debt item for Roslyn.
4. Represent observed callers as source path plus line and, when possible, fully qualified target signature. Do not merely count files.
5. Recompute all HOST_REQUIRED seams. Expect some currently-green seams to become unbound; those are real findings, not test failures to suppress.
6. For each newly exposed unbound seam, classify into: real missing host wiring; LIVE_VIA_CORE; TEST_ONLY; OPTIONAL_HOST; compatibility/dead seam. Fix/classify one bounded domain at a time.
7. Do not bulk reclassify to make the count green. Every classification change needs source evidence and an owner.

### Verification

Synthetic same-method/different-class proof; same class overload proof; exact-symbol caller positive test; wrong-class negative test; current repository contract gate.

### Acceptance

A call to `Foo.Register()` can never satisfy `Bar.Register()`. Generated callers identify target class/signature. Any newly red seam has an evidence-backed disposition.

### Stop / rollback boundary

If compiler-backed analysis would require invasive project retargeting, land the failing regression and a narrow intermediate scanner first; do not destabilize Core build targets.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W04 — Make seam identity overload-safe and parser-safe

**Priority:** P1
**Dependencies:** W03
**Premise/evidence:** Current seam key is `Class.Method`; overloads collapse. Core seam discovery is regex/brace-stack based.
**Primary paths:** Port analyzer/generator, policy schema, generated outputs, contract tests, migration notes.

### Execution

1. Inventory public Bind/Wire/Register/Configure overloads in Core using compiler symbols.
2. Define canonical seam signature: fully qualified declaring type + method name + normalized parameter type list. Keep a human display name separate.
3. Add schema versioning to port policy before changing key format. Provide migration logic or regenerate policy deterministically from a reviewed mapping.
4. Assert unique canonical signatures and reject duplicate IDs.
5. Handle partial classes, nested types, generic methods/types, nullable annotations, arrays, tuples, and expression-bodied methods through symbols rather than regex.
6. Preserve stable display ordering for deterministic JSON/Markdown diffs.
7. Add compatibility handling if any other repository tool consumes old `Class.Method` keys.

### Verification

Overloaded method tests; partial class tests; nested type tests; generic signature tests; stable ordering/golden generation test.

### Acceptance

No two distinct methods collapse into one seam. Parser correctness no longer depends on brace heuristics.

### Stop / rollback boundary

Do not change public gameplay APIs merely to simplify the analyzer.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W05 — Repair port policy invariants and generated metadata semantics

**Priority:** P1
**Dependencies:** W03
**Premise/evidence:** `total_seams` is not enforced; `generated_at` is hard-coded; classification status display is not fully taxonomy-aware.
**Primary paths:** `scripts/ci/generate-port-contract.py`, `docs/ci/port_contract_policy.json`, contract tests, generated outputs.

### Execution

1. Enforce `policy.total_seams == len(policy.ports)` or remove `total_seams` from the authored policy. Prefer one source of truth.
2. Remove hard-coded `generated_at`. If a provenance field is required, use a deterministic policy revision or an explicitly supplied commit SHA rather than wall clock.
3. Validate classification-specific invariants: HOST_REQUIRED requires symbol-resolved production caller; DEFERRED forbids production caller; TEST_ONLY should not silently become production-bound without review; LIVE_VIA_CORE must have Core-side evidence; OPTIONAL_HOST must document its optionality boundary.
4. Change DEFERRED error wording from 'must upgrade to HOST_REQUIRED' to 'must be reclassified/wired according to actual call ownership'.
5. Add machine-readable reasons for exemptions and compatibility shims where useful.
6. Keep generation deterministic across machines/timezones.

### Verification

Metadata mismatch proof; deterministic two-run byte comparison; classification transition proofs; no wall-clock dependency test.

### Acceptance

Policy metadata cannot contradict body length; generated content contains no stale hard-coded date; taxonomy failures are precise.

### Stop / rollback boundary

Do not add volatile timestamps that make a clean checkout dirty on every run.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W06 — Expand port-contract tests from proof-of-failure demos to repository proof

**Priority:** P0/P1
**Dependencies:** W03,W04,W05
**Premise/evidence:** Current tests reproduce the same method-name-only regex and therefore validate the same unsound assumption as the generator.
**Primary paths:** `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` and any analyzer unit-test project/files.

### Execution

1. Delete duplicated token-only caller logic from tests; call the same analyzer library used by production generation or compare independent symbol results.
2. Add false-positive regression: wrong class, same method token.
3. Add false-negative regression: aliased namespace/field/constructor target resolves correctly.
4. Add overload regression.
5. Add owner-change regression verifying generated `port_id`/identity behavior.
6. Add `total_seams` mismatch regression.
7. Add stale generated artifact regression that identifies exact changed ports rather than only whole-file byte mismatch.
8. Add classification transition tests for DEFERRED→LIVE, DEFERRED→OPTIONAL, seam deletion, and compatibility shim.
9. Keep test messages actionable: include seam, expected target, observed invocations, source locations.

### Verification

Run the new file alone, then the Tooling directory if under repository policy limits.

### Acceptance

The exact bug class that let 103 HOST_REQUIRED seams share token evidence is impossible to reintroduce unnoticed.

### Stop / rollback boundary

Do not add enormous snapshot blobs to xUnit; keep expected symbol sets small and readable.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W07 — Give port-contract generated outputs an automated owner

**Priority:** P0/P1
**Dependencies:** W01,W05
**Premise/evidence:** `docs-regen.yml` omits the port generator and does not trigger on policy changes.
**Primary paths:** `.github/workflows/docs-regen.yml`, optional dedicated `.github/workflows/port-contract-regen.yml`, generator docs.

### Execution

1. Choose either a dedicated port-contract regeneration workflow or add the generator to the existing docs workflow. Prefer dedicated ownership if Core-source triggers would make the generic docs workflow too broad.
2. Trigger on `docs/ci/port_contract_policy.json`, the analyzer/generator files, and relevant Core seam surface changes. If Core-wide path triggers are too noisy, keep CI `--check` mandatory and use workflow-dispatch regeneration rather than hidden auto-commits.
3. Run generation before docs index, because docs index embeds document headers.
4. Use a bot-created branch/PR rather than pushing directly to protected main.
5. Ensure workflow never fights a human PR by writing to the same branch unexpectedly.
6. Document local parity command in `docs/CI.md`.

### Verification

Workflow YAML syntax check where available; local generator sequence; docs index `--check`; port contract `--check`.

### Acceptance

A policy/analyzer change cannot be merged with stale generated contract artifacts without a required check failing, and maintainers have a one-command/one-workflow regeneration path.

### Stop / rollback boundary

Do not create a bot loop where regeneration commits retrigger themselves indefinitely.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W08 — Reorder CI so cheap deterministic drift gates fail before expensive Godot work

**Priority:** P1
**Dependencies:** W01,W03
**Premise/evidence:** Current port-contract mismatch is discovered at the last fast gate after minutes of successful runtime checks.
**Primary paths:** `docs/ci/CI_GATE_MANIFEST.json`, `scripts/ci/run-gates.py` only if dependency ordering capabilities need improvement, CI docs.

### Execution

1. Classify fast gates by cost and dependencies: pure text/source checks; dotnet compile/tests; Godot import; host runtime selftests.
2. Move dependency-free static/drift gates—including port contract, docs/catalog drift, whitespace, JSON syntax, case collision, package policy—before Godot import/runtime gates.
3. Preserve explicit `depends_on` ordering. Do not move a generated check ahead of prerequisites it truly needs.
4. Keep the full Core xUnit gate outside fast tier as currently designed; the manifest marks `test_core_suite` as `full`, and `ci.yml` intentionally invokes it after fast tier.
5. Update reporting so early failure still uploads complete failure artifacts.
6. Measure run duration before/after with a deliberately stale generated fixture in a test branch.

### Verification

Manifest validation; targeted `run-gates.py --gate ...`; one fast dry run after package completion.

### Acceptance

A deterministic source/doc drift failure exits before Godot import/runtime work while healthy runs preserve the same semantic gate coverage.

### Stop / rollback boundary

Do not lower timeouts or skip critical gates to claim speedup.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W09 — Require canonical CI/build statuses in protected-branch rules

**Priority:** P1
**Dependencies:** W01,W08
**Premise/evidence:** Active ruleset has PR review and CodeQL rules but no required-status-check rule.
**Primary paths:** GitHub repository ruleset settings; repository docs describing emergency bypass. No gameplay source.

### Execution

1. After the repaired workflows are green and their check names are stable, add required status checks for the canonical ASHFALL CI and Build ASHFALL workflows (or their stable job checks).
2. Require branch to be up to date before merge if that behavior fits the repository's stacked-PR workflow; otherwise define a merge-queue/rebase protocol that tests the candidate against latest main.
3. Retain review requirement and stale-review dismissal.
4. Review bypass actors. Keep only necessary automation/owner emergency access and document when bypass is allowed.
5. Document that bypass requires a post-merge verification run and a follow-up issue when used for a failing required check.
6. Do not require transient diagnostic workflow names.

### Verification

Create a harmless test PR and verify GitHub blocks merge until both required checks report success.

### Acceptance

A red canonical CI/build result blocks normal merges to main.

### Stop / rollback boundary

If the repository cannot configure required checks through Codex/GitHub permissions, produce the exact settings delta for the owner rather than pretending it was applied.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W10 — Prevent stale stacked-PR merges from stranding fixes

**Priority:** P1
**Dependencies:** W02,W09
**Premise/evidence:** PR #55 merged into a feature branch after that branch had already merged to main.
**Primary paths:** `CONTRIBUTING.md` or active workflow docs; optional lightweight CI script checking PR base policy; GitHub rules/settings if supported.

### Execution

1. Define allowed PR bases: normally `main`; temporary stacked bases must carry an explicit stack label/record and must be retargeted/rebased after parent merge.
2. Add a pre-merge checklist: base ref still open/active; merge-base includes latest required parent; generated artifacts regenerated after rebase.
3. If GitHub Actions context permits, add a PR-only guard that fails when the base branch matches a known closed/merged stack branch or when an active stack manifest says the parent has landed.
4. Require semantic cherry-pick/reapplication rather than merging a diverged stale stack after parent closure.
5. Document how to close/delete stacked branches after landing.

### Verification

Synthetic stack workflow/documented rehearsal; no runtime tests.

### Acceptance

Future child PRs cannot silently 'merge successfully' without placing their semantic delta on the intended integration branch.

### Stop / rollback boundary

Do not ban all stacked PRs if the foreman relies on them; make the stack state explicit instead.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W11 — Synchronize active agent handoff with the live integration ledger

**Priority:** P1
**Dependencies:** W00
**Premise/evidence:** `AGENTS.md`/`CODEX.md` advertise a stale C1 UI handoff while `INTEGRATION_PLANS.md` names XP Wave 1 as current.
**Primary paths:** Canonical `AGENTS.md`, generator/sync source for client rulebooks, generated `CODEX.md` and other synchronized rulebooks, `INTEGRATION_PLANS.md` reference only.

### Execution

1. Remove volatile package-specific 'ACTIVE HANDOFF' prose from canonical agent rules, or generate it from a single machine-readable/current-ledger source.
2. Replace with a short directive: current package must be read from `INTEGRATION_PLANS.md`; do not duplicate current-batch content in rulebooks.
3. Run `sync-agent-rulebooks.py` through its canonical mode; never edit generated CODEX client rulebooks independently.
4. Add a sync test that rejects hard-coded active package identifiers in generated rulebooks unless they match the ledger.
5. Confirm no historical archived rulebook is modified.

### Verification

`python3 scripts/ci/sync-agent-rulebooks.py --check`; focused agent-rule tests if present.

### Acceptance

Codex can never receive two different 'current task' authorities from active rulebooks and the integration ledger.

### Stop / rollback boundary

Do not rewrite historical archived rules.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W12 — Reconcile current authority and debt/census metrics from current truth

**Priority:** P1
**Dependencies:** W02,W03,W08
**Premise/evidence:** Current authority still references 14/47 gate eras; Plan 36 debt/census rows reference 17 deferred seams even though current policy has 6 before PR55 re-land.
**Primary paths:** `KNOWN_DEBT.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, `docs/CURRENT_AUTHORITY.md`, `INTEGRATION_PLANS.md`, generated `docs/INDEX.md` last.

### Execution

1. After port fixes land, derive final seam counts from generated contract and update only live-authority summaries. Preserve historical implementation logs as historical evidence.
2. Update CI gate counts from `CI_GATE_MANIFEST.json`; do not manually freeze counts that will immediately drift again.
3. Replace exact volatile catalog/save/gate counts in hand-maintained prose with links to generated authorities where possible.
4. Correct Plan 36 closure wording to distinguish symbol-proof remediation from the old token-level gate.
5. Regenerate docs index last.
6. Record that old logs remain truthful snapshots of their dates and must not be retroactively rewritten.

### Verification

docs index `--check`; portable doc link gate; port contract `--check`; any claims/debt validators.

### Acceptance

Active authority docs agree on current package state and current generated metrics; historical records remain untouched.

### Stop / rollback boundary

Do not mass-edit thousands of historical plan references merely because counts changed.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W13 — Repair architecture-map cadence truth for Weather Hardening and similar systems

**Priority:** P1
**Dependencies:** W00
**Premise/evidence:** Source ticks Weather Hardening daily; generator declares `On-Demand`.
**Primary paths:** `scripts/ci/generate-architecture-map.py`, generated `docs/architecture/ARCHITECTURE_TEST_MAP.md`, architecture-map gate tests.

### Execution

1. Trace the architecture generator's Weather Hardening row. Replace the hard-coded false cadence with source-derived or explicitly correct daily owner/cadence evidence.
2. Search for other `ticked: False` manifest rows whose host instances are called from day owners/expanded shelter tick paths.
3. Conversely search for `ticked: True` rows with no production tick call. Treat each mismatch as a separate evidence correction, not an automatic wiring request.
4. Where feasible, generate cadence from a machine-readable subsystem manifest rather than duplicating booleans in Python.
5. Regenerate architecture map and docs index after fixes.

### Verification

`python3 scripts/ci/generate-architecture-map.py --check` after generation; architecture map gate tests; focused source fixture proving Weather Hardening daily call is recognized.

### Acceptance

Weather Hardening is documented as the cadence source actually executes. No duplicate gameplay tick is added.

### Stop / rollback boundary

If source cadence is conditional, encode the condition rather than flattening it to an inaccurate binary label.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W14 — Fix the `CS8603` suppression/comment contradiction and establish a nullability budget

**Priority:** P1
**Dependencies:** W00
**Premise/evidence:** Project comment says CS8603 active; NoWarn suppresses it.
**Primary paths:** `Ashfall.csproj`, warning-baseline scripts/tests; production files only when repairing measured warnings.

### Execution

1. First add a tooling assertion that the explanatory warning policy and `<NoWarn>` cannot contradict each other.
2. Temporarily build with CS8603 unsuppressed in a diagnostic command and capture the exact warning count/files. Do not commit a red project solely to measure.
3. Classify warnings: serializer DTO initialization, legitimate nullable return contract, or potential real bug.
4. Repair real warnings with correct annotations/guards. Use narrow pragma/suppression only for proven serializer-assigned cases; avoid project-wide blanket suppression.
5. Remove CS8603 from global NoWarn when the measured set is zero or explicitly localized.
6. Repeat category-by-category later for other broad nullable suppressions; do not turn this package into a whole-repo nullable rewrite.

### Verification

`dotnet build Ashfall.csproj --nologo`; `warning-baseline-gate.sh`; new warning-policy consistency test.

### Acceptance

Project comment and compiler behavior agree; CS8603 is either active or narrowly justified at specific sites.

### Stop / rollback boundary

Never use null-forgiving operators as a mass mechanical fix without proving object lifecycle.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W15 — Make survivor visual variant selection process-stable and overflow-safe

**Priority:** P1
**Dependencies:** W00
**Premise/evidence:** `SurvivorActorView` uses `Math.Abs(string.GetHashCode()) % variants`.
**Primary paths:** `src/World/SurvivorActorView.cs`; a focused source/UI selftest; no Core authority changes unless required.

### Execution

1. Use the existing deterministic `Ashfall.Core.StableHash` utility rather than adding a second hash.
2. Compute variant index with unsigned arithmetic, e.g. cast the stable 32-bit result to `uint` before modulo, avoiding `Math.Abs(int.MinValue)`.
3. Keep visual selection presentation-only; do not persist the index unless design already requires it.
4. Add a source-level/test assertion that `SurvivorActorView` no longer calls `GetHashCode()` for selection.
5. Exercise multiple known survivor IDs and verify index remains in range and stable for repeated construction/reload.

### Verification

Godot host build; smallest relevant survivor/world UI selftest; host determinism source check from W17 when available.

### Acceptance

Same survivor ID deterministically selects the same variant across process runs and no signed-abs overflow path exists.

### Stop / rollback boundary

Do not change authored character variant ordering in the same commit.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W16 — Remove randomized string hashing from deterministic tests

**Priority:** P1
**Dependencies:** W15
**Premise/evidence:** Collectible characterization and audio integration test helpers use `GetHashCode()`.
**Primary paths:** `Ashfall.Core.Tests/Collectibles/CollectibleBalanceCharacterizationTests.cs`, `Ashfall.Core.Tests/AudioEventIntegrationTests.cs`, potentially a tooling source test.

### Execution

1. Replace `t.id.GetHashCode()` with existing `StableHash.Of(t.id)` using a deterministic conversion that preserves valid seed range.
2. Replace audio helper hash usage the same way.
3. Search all test sources for `GetHashCode()` and distinguish legitimate equality/hash-code contract tests from uses as RNG seed/dedup identity.
4. Do not ban `GetHashCode` overrides/value-object tests globally; ban only stochastic seed/identity use in deterministic tests.
5. Run each changed test file twice in separate test processes and compare relevant counts/results.

### Verification

Changed test files individually; deterministic source-policy test.

### Acceptance

Characterization outcomes are process-invariant; no deterministic test seed is derived from runtime-randomized string hash.

### Stop / rollback boundary

Do not update expected balance thresholds unless stable hashing reveals a genuine authored-distribution discrepancy; investigate first.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W17 — Add a host-side determinism source gate with explicit diagnostic allowlist

**Priority:** P1
**Dependencies:** W15,W16
**Premise/evidence:** Core already bans nondeterministic APIs, but production `src/` has weaker enforcement and issue #52 exposes seed drift.
**Primary paths:** new/extended source gate under `scripts/ci/` or tests; `CI_GATE_MANIFEST.json`; allowlist file if needed.

### Execution

1. Scan production host code for `System.Random`, parameterless/random wall-clock seeds, `Guid.NewGuid` in gameplay identity, `DateTime.Now/UtcNow` in simulation decisions, and `string.GetHashCode()` used for identity/RNG.
2. Allow diagnostic timestamps, save metadata timestamps, corrupt-file quarantine naming, and selftest scratch paths through explicit path+reason entries.
3. Separate 'hard-coded deterministic seed' from outright nondeterministic API: constants require reachability review, not blanket failure.
4. Fail new unallowlisted violations with file/line and remediation message.
5. Register gate early in fast tier because it is cheap.

### Verification

Proof-of-failure fixtures for each banned pattern and allowlisted diagnostic pattern.

### Acceptance

A new randomized hash or wall-clock gameplay seed in `src/` fails CI immediately, while legitimate diagnostics stay permitted.

### Stop / rollback boundary

Do not forbid timestamps in metadata/logging that are intentionally non-simulation.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W18 — Canonical campaign RNG migration — Phase A: World, Expedition, Narrative, Radio

**Priority:** P1
**Dependencies:** W00,W17; wait for conflicting active claims
**Premise/evidence:** Issue #52 and source show independent `DemoSeed` defaults.
**Primary paths:** Relevant host sessions and `Main` composition paths for World, Expedition, Narrative, Radio; `CampaignRngManager`/stream IDs only if extending the canonical stream registry; focused tests.

### Execution

1. For each host, locate the actual production constructor call from Main/composition. Do not modify test-only default constructors first.
2. Pass a campaign-derived `ISeededRng` or stable substream from `_campaignDay.Rng`, using a named `CampaignStreamIds` domain rather than magic integers.
3. Define stream semantics: same campaign seed + same command history must reproduce; different campaign seeds must diverge where stochastic behavior is expected; unrelated subsystem draws must not perturb each other.
4. Preserve save semantics. If RNG state is derived/forked per day/action rather than stored, prove reload parity. If the host keeps mutable RNG state, persist/restore it through its existing owner rather than introducing a parallel save.
5. Keep existing constant seed only as an explicit fixture/fallback path when no campaign context exists, and rename/comment it accordingly.
6. Do one subsystem per commit if the changes touch different save/runtime owners.

### Verification

Paired-seed tests, continuous-vs-mid-reload replay, existing host focused suites, campaign journey slice where appropriate.

### Acceptance

Production composition for these four domains is campaign-seed-derived and replay-stable; fixture paths remain deterministic and explicit.

### Stop / rollback boundary

If a host has no canonical campaign composition owner, report that missing authority rather than creating another global RNG singleton.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W19 — Canonical campaign RNG migration — Phase B: Maritime, Dose Ledger, Economy, Deep Coast

**Priority:** P1
**Dependencies:** W18
**Premise/evidence:** These hosts also advertise independent `DemoSeed` constants or construct their own seeded RNG.
**Primary paths:** Corresponding host sessions, their Main setup paths, canonical stream registry, focused domain tests.

### Execution

1. Repeat the production-call-path classification used in W18.
2. Use distinct stable campaign substreams for maritime route/scavenge decisions, dose stochastic decisions (if truly stochastic), economy variation, and deep-coast decisions.
3. Do not merge logically separate random domains merely because they previously shared a constant like 2026.
4. Audit any shared RNG instance consumed by multiple mechanics; split into named forks when draw-order coupling would make unrelated actions perturb results.
5. Verify saved state contains all non-derivable stochastic state.

### Verification

Domain-specific same-seed/different-seed/reload tests plus existing save tests.

### Acceptance

Changing campaign seed influences intended stochastic outputs while preserving deterministic replay and subsystem independence.

### Stop / rollback boundary

If a system's constant seed intentionally defines authored deterministic content rather than campaign randomness, document and exclude it.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W20 — Canonical campaign RNG migration — Phase C: hard-coded seeds not named `DemoSeed`

**Priority:** P1
**Dependencies:** W18,W19
**Premise/evidence:** Fresh search found production candidates in Counter Intelligence, Recon Telemetry, Year of Ash warlord, Verdict, Airlock/Shelter infrastructure, and default host constructors.
**Primary paths:** Only candidates proven production-reachable after audit.

### Execution

1. Produce a table of every `new SeededRng(<constant/expression>)` under `src/` with file, method, production reachability, owner, current save semantics, and disposition.
2. Classify as CAMPAIGN_STREAM, DAY/ACTION_DERIVED_STREAM, FIXTURE_ONLY, SELFTEST_ONLY, PRESENTATION_ONLY, or AUTHORED_FIXED_SEQUENCE.
3. Convert only CAMPAIGN_STREAM/DAY-ACTION cases to canonical RNG.
4. Where Main already uses `_campaignDay != null ? Fork(...) : new SeededRng(fallback)`, preserve that good pattern and standardize stream identifiers rather than deleting useful deterministic fallback behavior.
5. Add source comments only when they explain a non-obvious fixed-sequence exemption.

### Verification

Source inventory test ensures every constant seed has an explicit classification; focused runtime tests only for converted production paths.

### Acceptance

No unclassified production hard-coded seed remains.

### Stop / rollback boundary

Do not mechanically replace every constant seed; doing so would corrupt fixtures and authored deterministic tools.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W21 — Build RNG replay characterization across save/load boundaries

**Priority:** P1
**Dependencies:** W18-W20
**Premise/evidence:** RNG migration is only safe if replay semantics are proven, not merely if seeds are centralized.
**Primary paths:** Focused integration/replay tests; no new gameplay owner.

### Execution

1. Create a small campaign harness using canonical campaign seed and a fixed action script.
2. Run continuous N-day path and mid-run save/reload path; compare domain fingerprints.
3. Run same action script under two distinct campaign seeds and assert at least one intended stochastic result differs without asserting exact cosmetic randomness everywhere.
4. Assert unrelated-domain action insertion does not perturb another domain when separate streams are intended.
5. Capture enough fields to detect drift: event IDs, selected encounter IDs, market stochastic components, expedition outcomes, radio selections, etc., limited to touched systems.
6. Keep the harness bounded and deterministic; no wall-clock waits.

### Verification

New replay test file alone; then relevant deterministic integration directory.

### Acceptance

Canonical seed propagation has explicit continuous/reload and stream-isolation proof.

### Stop / rollback boundary

Do not freeze huge serialized snapshots if field-level fingerprints give clearer failure diagnostics.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W22 — Retire or reclassify `SkillProgressionSystem.RegisterDefaultSkills` legacy zero-op

**Priority:** P2
**Dependencies:** W02,W03
**Premise/evidence:** Migration docs describe `RegisterDefaultSkills()` as a retained zero-op compatibility/fallback after `skills.json` became authoritative.
**Primary paths:** `SkillProgressionSystem.cs`, callers/tests/docs/policy only if current source reconfirms zero-op status.

### Execution

1. Inspect implementation and all callers with symbol-aware search.
2. If truly zero-op and no supported external API compatibility requires it, remove the method and policy seam; update focused tests to use JSON catalog loader.
3. If compatibility is required, classify it explicitly as `COMPATIBILITY_SHIM` (or equivalent policy metadata) rather than DEFERRED production work.
4. Ensure headless tests still have an authority-backed way to load skills; do not restore hard-coded defaults.

### Verification

Skill progression and skill catalog focused tests; port contract gate.

### Acceptance

The method is either gone or explicitly represented as compatibility, not falsely treated as pending beta integration.

### Stop / rollback boundary

Never reintroduce hard-coded skill definitions.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W23 — Audit and wire `SpiritualMeaningCoordinator.RegisterDeath` through the canonical death fact only if authority exists

**Priority:** P2
**Dependencies:** W03; decision-safe source audit first
**Premise/evidence:** Method is DEFERRED and appears in spiritual tests, but no production caller is currently recorded.
**Primary paths:** Spiritual coordinator, existing canonical death/memorial event owner, thin host adapter, existing spiritual save owner, focused tests.

### Execution

1. Find the canonical survivor-death fact/event already used by memorials, fate, roster, journal, and epilogue continuity.
2. Prove `RegisterDeath` represents a derived spiritual observation, not a second death ledger.
3. If an existing event can feed it exactly once, subscribe in the owning host/integrator path with idempotence keyed by deceased ID/day as current domain semantics require.
4. Verify capture/restore does not replay death registration twice.
5. If no canonical event contract is safe, leave DEFERRED and write the missing-authority blocker; do not call it from arbitrary UI or daily polling.

### Verification

Spiritual Plan30 tests; death/memorial focused integration; save/reload exactly-once test.

### Acceptance

Either a single canonical death event feeds spiritual meaning exactly once, or the blocker is explicitly documented with no fake integration.

### Stop / rollback boundary

Do not add another survivor death authority.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W24 — Audit `StealthSystem.RegisterWeaponNoise` against canonical weapon/item authority

**Priority:** P2
**Dependencies:** W03
**Premise/evidence:** Camouflage gear is production-bound; weapon-noise registration is DEFERRED.
**Primary paths:** `StealthSystem.cs`, weapon/item catalog owner, stealth host composition, policy/tests.

### Execution

1. Determine whether weapon noise profiles already exist in canonical weapon/item data or another combat/noise system.
2. Reject duplicated authored noise tables. If canonical values exist, add an adapter/loader that registers them into Stealth through one owner.
3. Validate item/weapon IDs and range units at data-integrity time.
4. Ensure stealth resolution reads the registered profile and has a safe default for legacy/unknown weapons.
5. If no canonical noise property exists, produce a schema decision packet rather than inventing values.

### Verification

Focused Stealth tests; catalog/reference validation; one production host wiring test.

### Acceptance

Weapon noise is either canonically sourced and reachable, or explicitly blocked on a named schema authority.

### Stop / rollback boundary

No parallel `weapon_noise.json` unless the current data authority explicitly selects it.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W25 — Audit `ShelterThermalSystem.RegisterExternalBurst` as a consequence sink, not a new hazard producer

**Priority:** P2
**Dependencies:** W13,W03
**Premise/evidence:** Thermal external-burst seam is DEFERRED; Weather Hardening already ticks daily and can produce pipe/freeze-related facts.
**Primary paths:** Shelter thermal, Weather Hardening event adapter, existing save owners, focused tests.

### Execution

1. Trace `RegisterExternalBurst` semantics and current Weather Hardening `OnPipeBurst` payload.
2. Check whether another existing producer (fire, combat, structural damage) already routes thermal bursts.
3. If Weather Hardening pipe burst is semantically compatible, wire its event to thermal through a thin host adapter, preserving each system's own state authority.
4. Prevent duplicate effects on reload/resubscription; verify event subscription lifecycle.
5. Do not make ShelterThermal poll WeatherHardening state if an event is already canonical.

### Verification

Weather Hardening focused tests, Shelter Thermal focused tests, one cross-system event/reload test.

### Acceptance

External burst effect has one producer→consumer route or remains explicitly blocked with reason.

### Stop / rollback boundary

Do not add a second weather-hardening tick or structural-health ledger.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W26 — Execute the already-promoted Plan 32 map-orphan hygiene package

**Priority:** P2
**Dependencies:** after P0 closure and claim availability
**Premise/evidence:** KNOWN_DEBT marks 10 routed `loc_*` nodes lacking canonical `locations.json` records as PROMOTED.
**Primary paths:** Canonical locations/map data and existing validators/tests only; exact IDs must be re-derived at execution time.

### Execution

1. Re-run the orphan query against current map nodes/routes and `locations.json`; do not assume the count is still 10.
2. For each orphan decide from existing evidence whether it is a valid location missing a canonical record or a dead node that should be removed. Follow the signed promotion condition.
3. Add minimal canonical records only from existing authored metadata; do not invent lore/rewards to fill fields.
4. Extend integrity validation so a routed `loc_*` identity cannot exist without the selected canonical location authority.
5. Run map route/reference tests and data integrity.

### Verification

WastelandMap focused suite; data-integrity selftest; catalog/reference validator.

### Acceptance

No routed map node violates the selected canonical-location invariant.

### Stop / rollback boundary

Graph-native travel and discovery gating are separate decision-blocked packages; do not smuggle them into data hygiene.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W27 — Prepare Plan 32 graph-native travel decision evidence; do not implement without signature

**Priority:** DECISION-BLOCKED
**Dependencies:** W26
**Premise/evidence:** Debt register says expedition/caravan do not consume `WastelandMapSystem`; migration would affect multiple travel multipliers and saves.
**Primary paths:** Read-only map/expedition/caravan/aviation/naval/save authority inspection plus a decision memo.

### Execution

1. Map current travel distance/time authorities for expedition, caravan, aviation, naval, and special traversal.
2. List multiplier composition order currently used (injury/amputation, vehicle, weather, terrain, difficulty, route hazard, etc.).
3. Define candidate graph-query API and backward-compatible save migration without coding it.
4. Specify discovery/fog behavior separately from route-distance adoption.
5. Present decision options with exact compatibility impact; wait for foreman/user signature.

### Verification

No gameplay tests; static evidence only.

### Acceptance

Decision memo is sufficient for a later implementation package to avoid inventing multiplier order.

### Stop / rollback boundary

No source change before the signed scope/order exists.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W28 — Prepare Plan 30 runtime horizon and consequence-route decision evidence

**Priority:** DECISION-BLOCKED
**Dependencies:** P0 closure
**Premise/evidence:** Authored war-chain days 480–607 conflict with live 180–360 campaign horizon; several emitted events have no selected player-facing consumer.
**Primary paths:** Read-only Year of Ash/faction war/event consumers plus decision memo.

### Execution

1. Reconfirm live campaign horizon and authored event days.
2. Reconfirm subscriber set for territorial clash, decree, stage surfaced/resolved, chain resolved, and standing changed.
3. Quantify what happens today when the campaign ends before late chain content.
4. Offer explicit choices: extend runtime horizon, remap authored days, make chain postgame simulation, or retire unreachable content—without selecting on Codex's own authority.
5. Separately offer first consequence route candidates (economy, expedition, radio, airlock) with existing owner APIs.

### Verification

Static/characterization evidence only.

### Acceptance

The user/foreman can choose horizon and first consequence consumer with concrete compatibility implications.

### Stop / rollback boundary

Do not change event days or wire a consequence route without the decision.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W29 — Prepare Plan 34 completion-history difficulty/chronicle decision evidence

**Priority:** DECISION-BLOCKED
**Dependencies:** respect XP active claim
**Premise/evidence:** Canonical difficulty authority now exists, but completion history intentionally omits it pending decision; chronicle consumer is not selected.
**Primary paths:** Read-only `Difficulty`, completion history/store, Endgame, candidate chronicle/epilogue read surfaces, decision memo.

### Execution

1. Reconfirm difficulty ID is immutable for a campaign and available at completion.
2. Define additive history schema option including migration/checksum implications.
3. Identify existing read surfaces that could display history without becoming a new authority.
4. Separate 'record difficulty ID' from 'grant profile reward / New Game+' because Plan 175 boundaries remain distinct.
5. Present options and stop.

### Verification

No implementation tests until signed.

### Acceptance

Decision packet narrows the remaining question to additive observation and selected consumer.

### Stop / rollback boundary

No reward/profile progression is invented.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W30 — Make volatile authority metrics generated instead of hand-maintained

**Priority:** P2
**Dependencies:** W12
**Premise/evidence:** CURRENT_AUTHORITY gate counts and Plan 36 counts drifted while structural docs-index checks still passed.
**Primary paths:** Authority docs, generator/small metrics script, docs index.

### Execution

1. Inventory volatile counts in active authority docs: gate totals, fast counts, save sections, catalogs, seam classifications, UI panel counts.
2. For each count choose: generated include/snippet; link to generated authority; or stable qualitative wording.
3. Prefer links for metrics not essential to the document's purpose.
4. Add a small consistency test for the few counts intentionally duplicated.
5. Regenerate docs index last.

### Verification

Docs consistency test; docs index; link portability.

### Acceptance

Routine feature work cannot silently make core authority prose numerically false.

### Stop / rollback boundary

Do not generate historical narrative documents.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W31 — Harden generated-artifact provenance and no-hand-edit policy

**Priority:** P2
**Dependencies:** W07,W13,W30
**Premise/evidence:** Multiple generated authorities exist, but ownership/trigger behavior differs and stale branch merges can reintroduce old bytes.
**Primary paths:** Generator scripts, generated-file headers, CI source-policy gate.

### Execution

1. Add a standardized generated-file header where format permits, naming generator command and source inputs.
2. Create a manifest mapping generated outputs → generator → primary inputs → check command.
3. Add CI verification that every registered generated output is reproduced byte-for-byte from current inputs.
4. Ensure JSON generated files carry provenance through adjacent manifest if comments are impossible.
5. Use this registry to order docs regeneration and reduce bespoke workflow lists.

### Verification

Generated-artifact registry consistency; regenerate-all dry run followed by zero diff.

### Acceptance

Every generated artifact has one discoverable owner and reproducible check command.

### Stop / rollback boundary

Do not auto-rewrite outputs during ordinary tests; checks should be read-only.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W32 — Review broad exception swallowing only where it hides integration failures

**Priority:** P2
**Dependencies:** P0 closure
**Premise/evidence:** Source contains broad catches in presentation/helper paths; catch-policy gate passes, so this is a targeted honesty audit, not an allegation of widespread breakage.
**Primary paths:** Only call paths where swallowed exceptions can hide data/integration failure; logger policy/tests.

### Execution

1. Search `src/` for bare/broad `catch` blocks returning empty/default values.
2. Classify presentation-best-effort paths versus authoritative setup/save/data paths.
3. For authoritative paths, log actionable context or propagate through existing failure surface rather than returning silent success.
4. Do not spam logs for optional cosmetic lookups already covered by fallback policy.
5. Add focused failure-path tests only for changed authoritative paths.

### Verification

Existing catch-policy gate plus focused failure-path tests.

### Acceptance

No authoritative integration failure can be converted into a silent nominal state by a changed catch block.

### Stop / rollback boundary

Do not rewrite every catch in the repo.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W33 — Modernize GitHub Actions runtime dependencies that emit deprecation warnings

**Priority:** P2
**Dependencies:** P0 green first
**Premise/evidence:** Current Actions log reports Node 20 deprecation/forced Node 24 and Node API deprecation warnings from action dependencies.
**Primary paths:** `.github/workflows/*.yml`; action versions only.

### Execution

1. Identify which action emits each warning from workflow logs.
2. Check upstream supported release compatible with GitHub's Node 24 transition.
3. Update one action family at a time (`checkout`, `setup-dotnet`, Godot setup, artifact) only when a stable compatible major exists.
4. Pin majors/commits according to repository security policy.
5. Run CI on a workflow-only PR.

### Verification

Hosted workflow run; no gameplay test changes required beyond normal workflow.

### Acceptance

Deprecated action runtime warnings are removed or documented as upstream-blocked.

### Stop / rollback boundary

Do not replace the Godot toolchain version merely to silence Node warnings.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W34 — Clarify fast/full/performance gate semantics and CI reporting

**Priority:** P2
**Dependencies:** W08
**Premise/evidence:** Manifest has 53 gates, fast count 50; `test_core_suite` is `full`, performance gate is `performance`, export parity is `full`. `ci.yml` explicitly runs fast then the full Core gate.
**Primary paths:** `CI_GATE_MANIFEST.json`, `ci.yml`, docs; runner only if needed.

### Execution

1. Document exactly which three gates are excluded from fast and why.
2. Rename CI steps if necessary so 'Fast Tier' followed by 'Full Core xUnit' is unambiguous.
3. Ensure failure summary combines both reports without implying skipped full gate passed.
4. Consider a scheduled/nightly lane for performance/export/full combinations if runtime warrants it, but do not reduce PR coverage without user approval.

### Verification

Runner selection unit test showing expected gate IDs per classification.

### Acceptance

Humans and agents can infer coverage from a green check without reading runner internals.

### Stop / rollback boundary

Do not silently move critical verification from PR to nightly-only.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W35 — Make deferred-expiry governance reproducible and intentional

**Priority:** P2
**Dependencies:** W05
**Premise/evidence:** DEFERRED expiry checks compare policy dates to wall-clock UTC, causing historical commits to become red after time passes.
**Primary paths:** Port policy/generator/tests/governance docs.

### Execution

1. Decide whether expiry is intentionally time-sensitive governance or should be reproducible per commit.
2. If time-sensitive is desired, isolate it as a clearly named governance deadline gate and document why historical checkout CI can fail after expiry.
3. If reproducibility is preferred, compare against an explicit policy evaluation date/version updated by governance, or surface expired debt as a separate report until actively evaluated.
4. Do not hide genuinely expired debt; the goal is explicit semantics, not removing deadlines.

### Verification

Before/on/after deadline fixture with injected evaluation date.

### Acceptance

Expiry behavior is deterministic under its declared inputs and understandable to maintainers.

### Stop / rollback boundary

No automatic deadline extension.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W36 — Split port debt status into compatibility, optional, internal-live, and truly unwired work

**Priority:** P2
**Dependencies:** W02,W22-W25
**Premise/evidence:** Generic DEFERRED currently conflates dead seam cleanup, compatibility fallback, and real integration work.
**Primary paths:** Policy schema/docs/generated contract/KNOWN_DEBT.

### Execution

1. Use existing taxonomy where sufficient (`OPTIONAL_HOST`, `LIVE_VIA_CORE`, `TEST_ONLY`, `PURE_LIBRARY`) before inventing new categories.
2. If compatibility shims need explicit representation, add a narrowly defined `COMPATIBILITY` state with removal/review condition.
3. Require every truly deferred production seam to name activation condition, owner, and concrete blocker.
4. After W22–W25, recalculate deferred count from source and update live debt.
5. Set shrink-only expectations on semantically comparable debt, not on a mixed count that can be gamed by reclassification.

### Verification

Policy schema tests and generator check.

### Acceptance

A reader can tell whether a zero-caller seam is safe optional API, internal Core API, compatibility shim, dead code, or missing production integration.

### Stop / rollback boundary

Do not use taxonomy changes to conceal real missing behavior.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W37 — Final integrator regression, merge preparation, and evidence closeout

**Priority:** P0 closeout
**Dependencies:** all executable packages selected for this remediation wave
**Premise/evidence:** Repository rules require focused verification during building, but a cross-cutting repair needs one integrator-level broad acceptance run.
**Primary paths:** No new feature files unless final verification exposes a package-owned defect.

### Execution

1. Regenerate every touched generated artifact in canonical order and verify zero second-run diff.
2. Run all newly added source/tooling gates individually.
3. Run focused tests for every package from its handoff log.
4. Run `bash scripts/ci/verify-fast.sh` from clean integration state.
5. Run the full Core xUnit gate via the canonical runner, not an ad-hoc command that bypasses manifest behavior.
6. Build Godot host. Run relevant headless journeys/selftests changed by RNG/UI integration.
7. If build workflow semantics changed, run/observe Windows and Linux export workflows.
8. Review `git diff --check`, case-collision, LFS health, and generated docs index.
9. Produce a machine-readable closeout table: package, commit, files, tests, result, known blocker, rollback commit.
10. Do not merge with a known failing required gate.

### Verification

Canonical fast tier; full Core; host build; relevant export/runtime gates; all package-focused tests.

### Acceptance

PR head is clean, reproducible, and green under the same commands protected main will require.

### Stop / rollback boundary

If broad verification exposes an unrelated pre-existing failure, prove baseline relation and file a separate blocker; do not opportunistically patch outside ownership.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---

## W38 — Post-merge `main` verification and branch/claim cleanup

**Priority:** P0 closeout
**Dependencies:** W37 + merge
**Premise/evidence:** The audited regression was caused partly by merge/base ordering, so PR-head green is insufficient.
**Primary paths:** Repository state, CI runs, governance ledgers; no gameplay edits unless a merge-only defect is proven.

### Execution

1. After merge, wait only for the actual GitHub workflows in the current interaction/session; verify `main` SHA and run conclusions.
2. Confirm port generator `--check` on main-equivalent checkout if available.
3. Confirm required checks attach to the merge commit.
4. Close/delete obsolete stacked repair branches after confirming their semantic delta is present.
5. Close or update issue #52 only when all scoped production RNG migrations and replay proofs are actually complete.
6. Update `WORKTREE_OWNERSHIP.md` claim status and `INTEGRATION_PLANS.md` closeout through the foreman/integrator.
7. Record any decision-blocked packages as explicitly not executed.

### Verification

Post-merge hosted CI/build plus no-diff generated checks.

### Acceptance

`main` itself is green; no fix is stranded on a merged feature branch; live ledgers match reality.

### Stop / rollback boundary

Do not mark completion from local/PR evidence if merge commit CI is red.

### Required handoff record

Codex must hand off: baseline commit; exact files changed; semantic contract changed or preserved; commands run; pass/fail counts; generated artifacts regenerated; save/schema impact; deterministic-RNG impact; UI/runtime impact; unresolved blockers; and the commit that cleanly reverts this package if rollback becomes necessary.

---


# Appendix A — Exact first-response procedure for Codex

When this plan is handed to Codex for execution, Codex should begin with the following behavior rather than immediately editing:

1. Read the active authorities in the order required by `AGENTS.md`.
2. Print the current branch, HEAD SHA, worktree status, and active claims that overlap the first requested package.
3. State which package from this document it is claiming first and which packages are explicitly deferred.
4. Re-query/fetch current source for every premise in that package.
5. If current evidence invalidates the premise, stop that package and record `STALE-PREMISE` with exact current evidence.
6. If the premise still holds, list exact paths before editing.
7. Make one coherent package, not several unrelated repairs.
8. Run the focused verification written in the package.
9. Produce a handoff entry before claiming another package.
10. Shared governance/generator files are integrated serially.

A recommended first implementation sequence is:

`W00 → W01 → W02 → W03 → W06 → W05 → W07 → W08 → W12 → W13 → W15 → W16 → W17`

Only after that infrastructure is trustworthy should the campaign RNG phases (`W18–W21`) or remaining port-debt integrations (`W22–W25`) proceed. Decision-bound packages (`W27–W29`) never become implementation merely because earlier packages finished.

# Appendix B — P0 emergency repair command sheet

The commands below are intentionally conservative. Codex must adapt exact test paths if the repository changed.

```bash
git status --short
git branch --show-current
git rev-parse HEAD

# Read authorities before edit.
sed -n '1,220p' AGENTS.md
sed -n '1,180p' INTEGRATION_PLANS.md
sed -n '1,180p' WORKTREE_OWNERSHIP.md
sed -n '1,180p' TEST_POLICY.md
sed -n '1,180p' KNOWN_DEBT.md

# Reproduce the current port-contract failure.
python3 scripts/ci/generate-port-contract.py --check || true

# Generate using the canonical owner.
python3 scripts/ci/generate-port-contract.py
git diff -- docs/architecture/PORT_CONTRACT.md docs/architecture/port-contract.json

# Second run must be zero-diff.
python3 scripts/ci/generate-port-contract.py --check

# Focused proof.
python3 scripts/ci/run-gates.py --gate port_contract_gate
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PortContractGateTests.cs

# Never stage before reviewing generated delta.
git diff --check
git status --short
```

For the stranded PR #55 semantic re-land, inspect the PR patch first and then edit current source. Do not use a blind merge of its historical feature branch. After re-landing:

```bash
python3 scripts/ci/generate-port-contract.py
python3 scripts/ci/generate-port-contract.py --check
bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PortContractGateTests.cs
# plus the smallest current Crafting and IceRoad/Holdfast focused tests
```

At final integration only:

```bash
bash scripts/ci/verify-fast.sh
python3 scripts/ci/run-gates.py --gate test_core_suite
dotnet build Ashfall.csproj --nologo
```

Use the repository’s bounded Godot runner for any runtime selftest rather than unbounded headless invocations.

# Appendix C — Port-contract analyzer acceptance matrix

The replacement analyzer is not accepted merely because the current repository becomes green. It must satisfy this matrix:

| Case | Expected |
|---|---|
| `Foo.Register()` exists, host calls `Foo.Register()` | Foo seam bound |
| `Foo.Register()` + `Bar.Register()`, host calls only Foo | Foo bound; Bar unbound |
| `Foo.BindCatalog()` called through a typed field | Foo bound |
| Same method called through local variable inferred from constructor | correct declaring type resolved |
| Namespace alias used | target still resolved |
| Derived override/virtual dispatch | canonical policy target explicitly defined and tested |
| Two overloads on same class | signatures tracked independently |
| Partial class declaration | one declaring type identity |
| Nested class | fully qualified identity remains unique |
| Generic method | normalized signature stable |
| Extension method named `Bind` | must not satisfy an instance seam on unrelated Core type |
| Invocation appears only in comment/string | no caller |
| Invocation appears only in tests | does not satisfy production `src/` requirement |
| Invocation exists in archived/retired path | does not satisfy current production requirement |
| DEFERRED seam becomes production-called | gate fails with reclassification message |
| Owner field changes | identity behavior follows the chosen stable-ID contract and generated artifacts update deterministically |

The tool should output source path + line number for each observed production invocation. When multiple callers exist, deterministic sort order is required.

# Appendix D — RNG classification ledger template

Codex should create this table during W20 rather than bulk-replacing constants:

| File | Constructor/call | Current seed | Production reachable? | State persisted? | Intended domain | Classification | Action |
|---|---|---:|---|---|---|---|---|
| `src/Host/WorldHostSession.cs` | weather intelligence/default host | 1234 | re-prove | inspect | world/weather | candidate campaign stream | migrate if production |
| `src/Host/ExpeditionHostSession.cs` | expedition RNG | 7071 | re-prove | inspect | expedition | candidate campaign stream | migrate |
| `src/Host/NarrativeHostSession.cs` | encounter demo/select | 4242 | split production/demo paths | inspect | narrative | mixed | production fork; fixture constant |
| `src/Host/RadioHostSession.cs` | radio RNG | 2026 | re-prove | inspect | radio | candidate | migrate |
| `src/Host/MaritimeHostSession.cs` | scavenge/safe defaults | 9909 | re-prove | inspect | maritime | candidate | split streams if draw-order coupled |
| `src/Host/DoseLedgerHostSession.cs` | ledger/calibration RNG | 1401 | re-prove | inspect | dose | candidate | migrate only stochastic gameplay |
| `src/Host/EconomyHostSession.cs` | economy RNG | 2026 | re-prove | inspect | economy | candidate | migrate |
| `src/Host/DeepCoastHostSession.cs` | deep coast | 4048 | re-prove | inspect | world/deep coast | candidate | migrate |
| `src/Main.FactionBranch.cs` | counter intelligence | 2001 | yes candidate | inspect | factions | candidate | campaign fork |
| `src/Main.Expeditions.cs` | recon telemetry | 2005 | yes candidate | inspect | expedition/recon | candidate | campaign fork |
| `src/YearOfAsh/YearOfAshHostSession.cs` | warlord RNG | 2026 | re-prove | inspect | year-of-ash | candidate | campaign fork |
| `src/Host/VerdictHostSession.cs` | machine RNG | fixed expression | re-prove | inspect | verdict | could be authored fixed sequence | classify before change |
| selftest/fixture files | scratch/test RNG | constants | no | n/a | test | fixture | keep |

Required classification vocabulary:

- `CAMPAIGN_STREAM`: production randomness must vary by campaign master seed.
- `DAY_ACTION_FORK`: deterministic per day/action, derived from campaign master stream.
- `AUTHORED_FIXED_SEQUENCE`: intentionally fixed behavior independent of campaign; requires reason.
- `FIXTURE_ONLY`: unit/integration fixture, not production reachable.
- `SELFTEST_ONLY`: host CLI/selftest path.
- `PRESENTATION_ONLY`: cosmetic deterministic sample with no simulation consequence.
- `UNKNOWN`: blocks migration/closure.

No `UNKNOWN` row may be silently converted.

# Appendix E — Generated-authority registry proposal

Create one machine-readable registry rather than relying on memory:

```text
output                                      generator/check                                  primary inputs
docs/architecture/port-contract.json        generate-port-contract.py --check                Core seams + port policy + src callers
docs/architecture/PORT_CONTRACT.md           generate-port-contract.py --check                same
docs/architecture/ARCHITECTURE_TEST_MAP.md   generate-architecture-map.py --check             save registry + host/source evidence
docs/saves/SAVE_STORE_CONTRACT_MATRIX.md     generate-save-store-matrix.sh --check            save stores/registry
docs/cli/HOST_CLI_COMMAND_CATALOG.md         generate-cli-catalog.sh --check                  HostCliRegistry
docs/INDEX.md                                generate-docs-index.py --check                    all docs; MUST RUN LAST
...
```

Rules:

1. Every output has exactly one owner generator.
2. Every generator has read-only `--check`.
3. Check mode never mutates the worktree.
4. Generation order is topologically sorted; docs index is last.
5. CI reports which input class caused drift where feasible.
6. Bot regeneration opens a branch/PR; it never bypass-pushes main.
7. A merge conflict in generated output is resolved by regenerating from reconciled inputs, not by manually choosing conflict sides.

# Appendix F — Merge-safety protocol for stacked work

A child PR based on another feature branch must be treated as a dependency edge, not as an independent mergeable unit.

Before merging a child:
- verify parent PR state;
- if parent is still open, child may remain stacked;
- if parent has merged, immediately retarget/rebase child to current main;
- regenerate all derived artifacts after rebase;
- rerun required checks against the new base;
- never merge child into the obsolete feature branch and assume GitHub “merged” means main contains it.

At closeout, compare the child's intended files/semantic deltas against main. A zero open PR list is not proof that all merged feature work reached main.

# Appendix G — Ruleset target configuration

After CI repair, the protected branch should normally require:

- pull request before merge;
- at least the existing review count;
- dismiss stale reviews on push;
- non-fast-forward protection;
- CodeQL/security requirements already in place;
- canonical `ASHFALL CI` successful;
- canonical `Build ASHFALL` successful if exports/build are intended to block merge;
- branch updated with target before merge or merge-queue equivalent;
- resolution of required review threads if the team uses inline blocking review.

Bypass should be emergency-only. Every bypass should leave an audit trail and trigger immediate post-merge main verification. Automation accounts should not have broad bypass merely because they generate docs unless that permission is actually required.

# Appendix H — Nullability remediation protocol

Do not convert the host to nullable-clean in one giant commit. Use this loop:

1. remove one warning code from a diagnostic build suppression;
2. capture exact warnings;
3. cluster by root cause;
4. repair lifecycle/contract bugs first;
5. annotate serializer-populated DTOs accurately;
6. localize unavoidable suppressions;
7. add a warning-policy regression;
8. remove the global suppression;
9. run build + focused runtime path;
10. commit category separately.

For `CS8603`, focus specifically on methods that may return null despite non-nullable return type. A return-value default inserted merely to silence the compiler is not acceptable if callers need to distinguish missing data.

# Appendix I — Authority conflict resolution rules

When active documents disagree, Codex uses this order for *current execution state*:

1. current user instruction;
2. active foreman/integrator decision in `INTEGRATION_PLANS.md`;
3. exact path ownership in `WORKTREE_OWNERSHIP.md`;
4. current `KNOWN_DEBT.md` blocker/promotion state;
5. source + data evidence;
6. domain authority map;
7. historical plan/closeout documents only as provenance.

`AGENTS.md` defines enduring process rules but should not duplicate a volatile current package. `CODEX.md` is generated from canonical agent rules and must not be edited as an independent current-task ledger.

# Appendix J — Audit facts Codex must revalidate before using

These were true at audit time and may change before execution:

- default branch HEAD was `166fa9aebb75a2310b15cb47dc5cc9b8a270e2b6`;
- build workflow was green;
- fast CI was red only at final selected port-contract gate;
- current policy had 248 seams: 176 HOST_REQUIRED, 5 OPTIONAL_HOST, 40 LIVE_VIA_CORE, 21 TEST_ONLY, 6 DEFERRED;
- five generated `port_id` values were stale after owner changes;
- 22 method-name collision families affected 108 seams / 103 HOST_REQUIRED seams;
- PR #55 semantic changes were absent from main;
- active ruleset did not require ASHFALL CI/build status checks;
- port contract was omitted from docs auto-regeneration;
- survivor view and two deterministic test areas used runtime `GetHashCode`;
- active port debt listed six DEFERRED seams;
- Weather Hardening was actually ticked daily despite architecture map saying On-Demand;
- active integration batch was XP Wave 1 while agent rulebook handoff prose still referenced older C1 UI work.

If any item is no longer true, update the execution log and use current evidence.

# Appendix K — Definition of Done for each code-changing package

A package is not DONE until all applicable rows are true:

- premise revalidated on current integration head;
- ownership acquired;
- non-goals written;
- exact paths listed before edit;
- no unrelated formatting/churn;
- Core remains engine-free;
- JSON authority not duplicated;
- no parallel save/state owner created;
- deterministic RNG route identified;
- capture/restore updated if state changed;
- schema migration added if persisted shape changed;
- loader/integrity validation updated if authored data changed;
- host composition actually calls the new Core behavior;
- observable consumer exists if feature claims player reachability;
- subscription/unsubscription lifecycle safe;
- focused test fails before fix when feasible;
- focused test passes after fix;
- generated outputs regenerated by owner script;
- generator `--check` passes;
- `git diff --check` passes;
- handoff records commands and counts;
- rollback is one coherent commit or clearly documented commit range.

# Appendix L — Items explicitly NOT authorized by this audit plan

Codex must not interpret “clean the repository” as permission to:

- rewrite or delete historical plan archives;
- reintroduce Unity;
- create a new shelter structural-health system;
- create a second RNG manager;
- create another map/place authority;
- invent weapon-noise values without canonical data;
- invent Plan 30 event timing;
- select a Plan 30 consequence destination;
- choose Plan 32 multiplier composition;
- add difficulty rewards/New Game+ semantics;
- implement amputation equipment handedness schema without signed design;
- remove Godot `partial` modifiers from Node/Control scripts;
- mass-enable quarantined historical tests;
- convert all host fixed seeds without reachability classification;
- suppress new warnings to preserve a zero-warning badge;
- hand-edit generated Markdown/JSON;
- force-push or reset user/agent work;
- claim a branch merge reached main without comparing ancestry/content.

# Appendix M — Recommended commit/PR decomposition

Keep reviewable commits:

1. `fix(ci): regenerate current port contract artifacts`
2. `refactor(ports): re-land dead crafting seam removal and Ice Road registration`
3. `test(ports): add same-method wrong-class failure proof`
4. `feat(ci): make port caller analysis symbol-aware`
5. `fix(ci): enforce port metadata invariants and deterministic provenance`
6. `ci: add port contract to generated authority workflow`
7. `ci: move cheap drift gates ahead of runtime gates`
8. `docs(governance): reconcile live Plan 36 and CI authority`
9. `fix(host): use stable hash for survivor visual variant`
10. `test(determinism): remove randomized string hash seeds`
11. `ci(determinism): enforce host nondeterministic API policy`
12+. one campaign RNG domain per commit/package
13+. one remaining deferred integration per domain, only with authority
14. final generated-doc reconciliation/closeout

Do not squash every semantic layer into one mega-commit before review; if the team prefers squash-merge, retain meaningful internal commits during validation and let GitHub squash only at final merge after review.

# Appendix N — Final acceptance report template

```markdown
## ASHFALL audit remediation closeout

Baseline:
- base SHA:
- final PR SHA:
- merged main SHA:

P0:
- current fast CI restored:
- port stale IDs fixed:
- PR55 semantics re-landed:
- symbol-aware caller proof:
- required status checks configured:

Determinism:
- survivor GetHashCode removed:
- deterministic tests stable:
- host RNG inventory complete:
- campaign stream migrations complete/deferred:

Authority:
- AGENTS/CODEX current-handoff conflict removed:
- KNOWN_DEBT current:
- census current:
- CURRENT_AUTHORITY current:
- architecture cadence current:

Verification:
- focused package tests:
- port gate:
- fast suite:
- full Core:
- Godot host build:
- relevant headless journeys:
- build/export workflow:
- post-merge main CI:

Decision-blocked and intentionally untouched:
- Plan 30:
- Plan 32:
- Plan 34:
- amputation equipment:
- other:

Known remaining debt:
- ID / owner / blocker / next condition
```

# Appendix O — Why this sequence unblocks larger plans

The repository's larger integration work depends on trustworthy answers to four questions: “is the seam really wired?”, “does a save/reload replay the same world?”, “which document is current authority?”, and “did the merged code actually reach main?”. The audited defects weaken each of those questions:

- method-token port attribution can say yes when the wrong class is called;
- hard-coded host RNG can make campaign seed ownership incomplete;
- stale active authority can route agents to already-finished work;
- stacked branch merges can report merged while leaving main unchanged.

The P0/P1 sequence repairs those *meta-contracts* before adding more content. This is intentionally more valuable than immediately implementing every remaining DEFERRED seam. Once symbol-level wiring, deterministic host streams, generated authority, and protected merges are reliable, later expansion plans can use the existing automated evidence instead of requiring repeated forensic audits.

The plan therefore treats repository truth as an executable feature: source, data, generated contracts, tests, CI, governance, and merge state must all describe the same game.

# Appendix P — Package execution cards (operator-grade checklist)


## Card W00 — Acquire integrator ownership and freeze the audit baseline

**Before touching files**
- Revalidate the premise: `WORKTREE_OWNERSHIP.md` currently has active claims over the port-contract candidate surface and XP Wave 1 campaign composition files.
- Re-check ownership for: `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`; read-only inspection of all files named by later packages.
- Record current HEAD and dirty paths.
- State why this package is P0 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: No broad tests. This is coordination/evidence only. Validate that the claim itself does not overlap another ACTIVE claim without an explicit transfer.
- Acceptance statement to prove: Exact-path ownership is unambiguous; no code has changed; baseline HEAD and dirty-state evidence are recorded.
- Stop boundary: If the required shared paths remain owned by another active builder, do not edit them. Hand off the requested transfer instead.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W00`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W01 — Restore `main` port-contract artifact consistency without hand editing

**Before touching files**
- Revalidate the premise: Fast CI currently fails only because generated `port-contract.json` is out of date. Five owner-derived IDs are stale.
- Re-check ownership for: `scripts/ci/generate-port-contract.py` (read only unless a separately justified generator fix is needed), `docs/ci/port_contract_policy.json` (read only in this package), generated `docs/architecture/port-contract.json`, generated `docs/architecture/PORT_CONTRACT.md`.
- Record current HEAD and dirty paths.
- State why this package is P0 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: `python3 scripts/ci/generate-port-contract.py --check`; `python3 scripts/ci/run-gates.py --gate port_contract_gate`; `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/PortContractGateTests.cs`.
- Acceptance statement to prove: Current generator produces byte-identical artifacts on a second run; the dedicated gate passes; diff contains no manual edits.
- Stop boundary: If regeneration changes more than expected, inspect the policy/source delta first. Do not accept broad generated churn without explaining every class of change.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W01`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W02 — Re-land the semantic content of stranded PR #55 on fresh main

**Before touching files**
- Revalidate the premise: PR #55 merged into an already-merged feature branch and is not present on current `main`.
- Re-check ownership for: `Assets/Ashfall.Core/Crafting/CraftingSystem.cs`, `Assets/Ashfall.Core/IceRoadSystem.cs`, `docs/ci/port_contract_policy.json`, generated port-contract outputs, focused Crafting/Ice Road/port tests.
- Record current HEAD and dirty paths.
- State why this package is P0 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Focused Crafting tests; focused IceRoad/Holdfast tests; `PortContractGateTests.cs`; generator `--check`.
- Acceptance statement to prove: Current source expresses the PR #55 behavior on top of current main; no stale branch history is merged; generated contract is consistent.
- Stop boundary: If `BindCraftResultGate` acquired a real consumer since the audit, stop deletion and classify/wire it based on current authority instead.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W02`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W03 — Replace method-name-only host caller attribution with symbol-aware evidence

**Before touching files**
- Revalidate the premise: 103 HOST_REQUIRED seams share colliding method names. Identical caller arrays across unrelated classes prove false attribution.
- Re-check ownership for: `scripts/ci/generate-port-contract.py` initially; preferably a new small analyzer under `scripts/ci/` or `tools/`; `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs`; package/dependency files only if a compiler-backed analyzer is approved.
- Record current HEAD and dirty paths.
- State why this package is P0 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Synthetic same-method/different-class proof; same class overload proof; exact-symbol caller positive test; wrong-class negative test; current repository contract gate.
- Acceptance statement to prove: A call to `Foo.Register()` can never satisfy `Bar.Register()`. Generated callers identify target class/signature. Any newly red seam has an evidence-backed disposition.
- Stop boundary: If compiler-backed analysis would require invasive project retargeting, land the failing regression and a narrow intermediate scanner first; do not destabilize Core build targets.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W03`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W04 — Make seam identity overload-safe and parser-safe

**Before touching files**
- Revalidate the premise: Current seam key is `Class.Method`; overloads collapse. Core seam discovery is regex/brace-stack based.
- Re-check ownership for: Port analyzer/generator, policy schema, generated outputs, contract tests, migration notes.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Overloaded method tests; partial class tests; nested type tests; generic signature tests; stable ordering/golden generation test.
- Acceptance statement to prove: No two distinct methods collapse into one seam. Parser correctness no longer depends on brace heuristics.
- Stop boundary: Do not change public gameplay APIs merely to simplify the analyzer.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W04`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W05 — Repair port policy invariants and generated metadata semantics

**Before touching files**
- Revalidate the premise: `total_seams` is not enforced; `generated_at` is hard-coded; classification status display is not fully taxonomy-aware.
- Re-check ownership for: `scripts/ci/generate-port-contract.py`, `docs/ci/port_contract_policy.json`, contract tests, generated outputs.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Metadata mismatch proof; deterministic two-run byte comparison; classification transition proofs; no wall-clock dependency test.
- Acceptance statement to prove: Policy metadata cannot contradict body length; generated content contains no stale hard-coded date; taxonomy failures are precise.
- Stop boundary: Do not add volatile timestamps that make a clean checkout dirty on every run.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W05`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W06 — Expand port-contract tests from proof-of-failure demos to repository proof

**Before touching files**
- Revalidate the premise: Current tests reproduce the same method-name-only regex and therefore validate the same unsound assumption as the generator.
- Re-check ownership for: `Ashfall.Core.Tests/Tooling/PortContractGateTests.cs` and any analyzer unit-test project/files.
- Record current HEAD and dirty paths.
- State why this package is P0/P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Run the new file alone, then the Tooling directory if under repository policy limits.
- Acceptance statement to prove: The exact bug class that let 103 HOST_REQUIRED seams share token evidence is impossible to reintroduce unnoticed.
- Stop boundary: Do not add enormous snapshot blobs to xUnit; keep expected symbol sets small and readable.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W06`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W07 — Give port-contract generated outputs an automated owner

**Before touching files**
- Revalidate the premise: `docs-regen.yml` omits the port generator and does not trigger on policy changes.
- Re-check ownership for: `.github/workflows/docs-regen.yml`, optional dedicated `.github/workflows/port-contract-regen.yml`, generator docs.
- Record current HEAD and dirty paths.
- State why this package is P0/P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Workflow YAML syntax check where available; local generator sequence; docs index `--check`; port contract `--check`.
- Acceptance statement to prove: A policy/analyzer change cannot be merged with stale generated contract artifacts without a required check failing, and maintainers have a one-command/one-workflow regeneration path.
- Stop boundary: Do not create a bot loop where regeneration commits retrigger themselves indefinitely.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W07`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W08 — Reorder CI so cheap deterministic drift gates fail before expensive Godot work

**Before touching files**
- Revalidate the premise: Current port-contract mismatch is discovered at the last fast gate after minutes of successful runtime checks.
- Re-check ownership for: `docs/ci/CI_GATE_MANIFEST.json`, `scripts/ci/run-gates.py` only if dependency ordering capabilities need improvement, CI docs.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Manifest validation; targeted `run-gates.py --gate ...`; one fast dry run after package completion.
- Acceptance statement to prove: A deterministic source/doc drift failure exits before Godot import/runtime work while healthy runs preserve the same semantic gate coverage.
- Stop boundary: Do not lower timeouts or skip critical gates to claim speedup.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W08`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W09 — Require canonical CI/build statuses in protected-branch rules

**Before touching files**
- Revalidate the premise: Active ruleset has PR review and CodeQL rules but no required-status-check rule.
- Re-check ownership for: GitHub repository ruleset settings; repository docs describing emergency bypass. No gameplay source.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Create a harmless test PR and verify GitHub blocks merge until both required checks report success.
- Acceptance statement to prove: A red canonical CI/build result blocks normal merges to main.
- Stop boundary: If the repository cannot configure required checks through Codex/GitHub permissions, produce the exact settings delta for the owner rather than pretending it was applied.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W09`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W10 — Prevent stale stacked-PR merges from stranding fixes

**Before touching files**
- Revalidate the premise: PR #55 merged into a feature branch after that branch had already merged to main.
- Re-check ownership for: `CONTRIBUTING.md` or active workflow docs; optional lightweight CI script checking PR base policy; GitHub rules/settings if supported.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: Synthetic stack workflow/documented rehearsal; no runtime tests.
- Acceptance statement to prove: Future child PRs cannot silently 'merge successfully' without placing their semantic delta on the intended integration branch.
- Stop boundary: Do not ban all stacked PRs if the foreman relies on them; make the stack state explicit instead.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W10`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W11 — Synchronize active agent handoff with the live integration ledger

**Before touching files**
- Revalidate the premise: `AGENTS.md`/`CODEX.md` advertise a stale C1 UI handoff while `INTEGRATION_PLANS.md` names XP Wave 1 as current.
- Re-check ownership for: Canonical `AGENTS.md`, generator/sync source for client rulebooks, generated `CODEX.md` and other synchronized rulebooks, `INTEGRATION_PLANS.md` reference only.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: `python3 scripts/ci/sync-agent-rulebooks.py --check`; focused agent-rule tests if present.
- Acceptance statement to prove: Codex can never receive two different 'current task' authorities from active rulebooks and the integration ledger.
- Stop boundary: Do not rewrite historical archived rules.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W11`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W12 — Reconcile current authority and debt/census metrics from current truth

**Before touching files**
- Revalidate the premise: Current authority still references 14/47 gate eras; Plan 36 debt/census rows reference 17 deferred seams even though current policy has 6 before PR55 re-land.
- Re-check ownership for: `KNOWN_DEBT.md`, `docs/plans/UNCLAIMED_CORPUS_CENSUS.md`, `docs/CURRENT_AUTHORITY.md`, `INTEGRATION_PLANS.md`, generated `docs/INDEX.md` last.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: docs index `--check`; portable doc link gate; port contract `--check`; any claims/debt validators.
- Acceptance statement to prove: Active authority docs agree on current package state and current generated metrics; historical records remain untouched.
- Stop boundary: Do not mass-edit thousands of historical plan references merely because counts changed.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W12`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W13 — Repair architecture-map cadence truth for Weather Hardening and similar systems

**Before touching files**
- Revalidate the premise: Source ticks Weather Hardening daily; generator declares `On-Demand`.
- Re-check ownership for: `scripts/ci/generate-architecture-map.py`, generated `docs/architecture/ARCHITECTURE_TEST_MAP.md`, architecture-map gate tests.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: `python3 scripts/ci/generate-architecture-map.py --check` after generation; architecture map gate tests; focused source fixture proving Weather Hardening daily call is recognized.
- Acceptance statement to prove: Weather Hardening is documented as the cadence source actually executes. No duplicate gameplay tick is added.
- Stop boundary: If source cadence is conditional, encode the condition rather than flattening it to an inaccurate binary label.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W13`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W14 — Fix the `CS8603` suppression/comment contradiction and establish a nullability budget

**Before touching files**
- Revalidate the premise: Project comment says CS8603 active; NoWarn suppresses it.
- Re-check ownership for: `Ashfall.csproj`, warning-baseline scripts/tests; production files only when repairing measured warnings.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

**Implementation discipline**
- Keep the first commit limited to the smallest coherent semantic change.
- When a generated file changes, identify the generator invocation in the commit message or handoff.
- If current source contradicts this plan, source wins; mark the plan premise stale and update the handoff.
- Do not borrow a convenient API from a historical document without proving the API exists at the current HEAD.
- Do not weaken a test or invariant merely because the current implementation fails it.
- Preserve old-save behavior for additive state unless the package explicitly includes a versioned migration.
- Preserve deterministic ordering in dictionaries/sets when their output enters save data, generated docs, hashes, tests, or UI snapshots.

**Verification discipline**
- Required focused verification: `dotnet build Ashfall.csproj --nologo`; `warning-baseline-gate.sh`; new warning-policy consistency test.
- Acceptance statement to prove: Project comment and compiler behavior agree; CS8603 is either active or narrowly justified at specific sites.
- Stop boundary: Never use null-forgiving operators as a mass mechanical fix without proving object lifecycle.
- On a failure, capture the first causal error rather than pasting thousands of downstream failures.
- Re-run the narrow failing command after the fix; broad suites belong at integration closeout.
- `git diff --check` and generated `--check` commands are mandatory whenever applicable.

**Handoff fields**
- package id: `W14`
- baseline SHA:
- final commit(s):
- exact files:
- behavior before:
- behavior after:
- save/schema/RNG impact:
- tests + exact counts:
- generated artifacts:
- known limitations:
- paths intentionally not touched:
- rollback:



## Card W15 — Make survivor visual variant selection process-stable and overflow-safe

**Before touching files**
- Revalidate the premise: `SurvivorActorView` uses `Math.Abs(string.GetHashCode()) % variants`.
- Re-check ownership for: `src/World/SurvivorActorView.cs`; a focused source/UI selftest; no Core authority changes unless required.
- Record current HEAD and dirty paths.
- State why this package is P1 and what is explicitly outside it.
- Identify whether the package can change save shape, data schema, RNG behavior, generated authority, or runtime composition. If none, write `none` rather than omitting the field.

> Remaining execution cards omitted because the primary package specifications above are authoritative.
