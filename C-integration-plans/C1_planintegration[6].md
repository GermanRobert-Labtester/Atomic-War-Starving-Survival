# C1 — Flagship Integration Plan [6]: Tests That Mean It — Fidelity, Coverage, Runtime Evidence & Real Campaign Journeys

> **Output:** `C1_planintegration[6].md`
>
> **Source baseline:** Plan 27 — Tests That Mean It: Fidelity, Coverage, and Runtime Evidence
>
> **Wave:** Continuity Wave 3 — *Ship It Intact*
>
> **Dependencies:** Plan 26A/26B for artifact/bootstrap fidelity; pairs with Wave-1 Plan 15C liveness; consumes Wave-2 integration work (Plans 20–24) as journey acceptance targets.
>
> **Mandatory execution order:** 27A → 27B → 27C.
>
> **Primary principle:** test the game the player actually runs, measure the risk-bearing behavior that matters, and prove integration by observed state transitions—not by presence, class names, route registration, or synthetic demo fixtures.
>
> **Guardrails:** no global vanity coverage target, no implicit demo fixture in a production selftest, no "does not throw" as sole acceptance, no unreviewed golden refresh, no retained flaky journey, no runtime-evidence metric derived only from grep/static inference.

---

# 0. Mission

ASHFALL's test volume is not the problem.

The source baseline reports:
- 323 test files;
- 5,303 passing tests;
- every Core `*System.cs` class mentioned by at least one test;
- yet critical continuity failures still survived:
  - hardcoded ending inputs;
  - inert consume callbacks;
  - fake player-routable consoles;
  - wear/condition paths that did not meaningfully affect play.

That means the test suite has quantity without enough fidelity.

Plan 27 changes the test strategy from:

```text
"Does this class exist?"
"Does this constructor work?"
"Does this fixture not throw?"
"Is this route registered?"
```

to:

```text
"Does the shipped authority behave correctly?"
"Does the real campaign wiring reach the next system?"
"Does save/load preserve actual state?"
"Does the same seed replay deterministically?"
"Did a catalog progress from load → query → selection → effect?"
"Did the exact panel bind to the exact campaign authority?"
```

The intended test architecture is:

```text
SHIPPED DATA AUTHORITY
        │
        ▼
AUTHORITY-BACKED CAMPAIGN FIXTURE
        │
        ├──────────────► UNIT / CONTRACT TESTS
        │
        ├──────────────► SAVE ROUND-TRIP TESTS
        │
        ├──────────────► DETERMINISM TESTS
        │
        ├──────────────► COVERAGE GATES
        │
        └──────────────► REAL CAMPAIGN JOURNEYS
                                  │
                                  ▼
                     RUNTIME CONTENT EVIDENCE
                                  │
                                  ▼
                           EFFECT_PRODUCED
```

The plan closes only when a future regression that reintroduces one of the Waves 1–2 integration failures causes CI to fail for a behavioral reason.

---

# 1. Source-Evidence Interpretation

## 1.1 Test volume is high but fidelity is uneven

A large suite can still miss production defects when it:
- constructs systems differently from the game;
- uses synthetic item catalogs;
- tests isolated Core contracts but not host callback wiring;
- treats route metadata as "coverage";
- validates construction rather than consequence.

Therefore 27A is prerequisite to every metric in 27B and every journey in 27C.

## 1.2 Selftests reportedly use different item authority

The source plan identifies a concrete split:

```text
player path:
InventoryHostSession.Create(dataDir)
→ ItemCatalogLoader
→ items.json

test/selftest path:
new InventoryHostSession()
→ SeedCatalog(...)
→ hardcoded demo definitions
```

Coverage over the demo catalog measures the wrong game.

## 1.3 No coverage measurement means no risk ratchet

Without coverage collection:
- nobody knows current line/branch coverage;
- coverage cannot be reviewed;
- no slice can be protected from regression;
- round-trip and determinism requirements remain conventions, not gates.

## 1.4 Static content evidence overstates runtime certainty

A catalog being discovered, queried, or source-referenced does not prove a player path selected it or produced a gameplay effect.

Plan 27C therefore promotes observed runtime transitions, especially `EFFECT_PRODUCED`.

## 1.5 Existing good gate patterns already exist

The repository already has patterns worth copying:
- source-scan gates;
- checksum sweeps;
- save-store coverage gates;
- data-rule compliance tests.

The plan should extend those house styles rather than introduce a completely different test framework.

---

# 2. Non-Negotiable Testing Invariants

## INV-27.1 — Production selftests use shipped authorities

Any selftest intended to certify the game must use the same:
- data directory resolution;
- catalog loaders;
- composition root;
- campaign owner graph;
as the shipped runtime.

## INV-27.2 — Synthetic fixtures are explicit

Synthetic fixtures are allowed only when:
- the test name says they are synthetic/fixture;
- construction API says so explicitly;
- the test is testing a narrow unit property rather than certifying production behavior.

## INV-27.3 — No implicit fallback systems in selftests

A host selftest must not silently instantiate a fresh campaign-owned system.

If production owns a service, the selftest should obtain the campaign-owned instance.

## INV-27.4 — Coverage protects risk slices, not vanity totals

Coverage gates target:
- save fidelity;
- determinism;
- stateful systems;
- continuity-critical domains.

A global percentage may be reported but is not the primary ship gate.

## INV-27.5 — Stateful systems require round-trip coverage

Every production `CaptureState` / `RestoreState` pair must be exercised by a round-trip contract.

## INV-27.6 — Day owners require determinism coverage

Every registered campaign-day owner must be represented in paired-seed replay coverage.

## INV-27.7 — Behavioral assertions beat presence assertions

Tests should assert:
- state transition;
- effect amount;
- reference identity;
- save equality;
- branch reachability;
- effect provenance;
not merely:
- class instantiated;
- value non-null;
- route exists;
- call did not throw.

## INV-27.8 — Runtime evidence means observed execution

`RUNTIME`, `SELECTED`, and `EFFECT_PRODUCED` must be populated by instrumentation running through real campaign paths.

## INV-27.9 — Journey tests prove connections

A journey passes only if each stage changes state consumed by the next stage.

## INV-27.10 — Flaky gates are defects

A journey that produces unexplained CI flakes is repaired or removed.

Do not normalize flakiness with retries/ignore labels unless the cause is understood and explicitly sanctioned.

---

# 3. Definition of Done

Plan 27 is complete only when:

- every production selftest either loads the shipped data authority or is explicitly named synthetic;
- implicit demo catalog seeding is removed from default host construction;
- one shared authority-backed `CampaignFixture` exists;
- test/source gate detects fresh campaign-system construction in host selftests/UI tests;
- golden save fixtures exist for early/mid/late campaign states;
- determinism digests exist for 30/180/360-day authority-backed runs;
- duplicate authored item seeds are consolidated;
- fixture policy is documented;
- coverage collection emits Cobertura artifacts;
- baseline line/branch coverage is published;
- continuity-critical slices cannot decrease without reviewed baseline change;
- every Core state capture/restore pair has a round-trip contract;
- every campaign-day owner has determinism coverage;
- weak assertions on high-risk paths are replaced with state assertions;
- bounded mutation spot-checks measure assertion quality;
- coverage gate is registered in CI manifest;
- runtime content evidence is collected during real boot/journey paths;
- `DESERIALIZED`, `REGISTERED`, `SELECTED`, and `EFFECT_PRODUCED` stages are instrumented meaningfully;
- at least five named journey classes exist and are documented;
- first-hour journey traverses Wave-2 integration seams;
- ending journey proves state-derived epilogue behavior;
- persistence journey compares real bound values/history after load;
- input journey reaches every live player route by keyboard;
- runtime panel liveness asserts reference identity with campaign authorities;
- snapshot coverage tracks live route count rather than registry count;
- all journeys are seed-pinned, culture-invariant, and stable;
- Tier-2/nightly split preserves fast local verification;
- complete Wave-3 verification is green.

---

# 4. Phase P0 — Baseline and Test-System Inventory

## P0.1 Record current baseline

Capture:

```text
commit SHA
branch
dirty-file count
dotnet test total / passed / failed
test file count
Core System.cs count
current selftest verbs
current host/selftest fixture constructors
current hardcoded fixture catalog definitions
current explicit fixture builders
current save-test fixtures
current coverage package references
current CI gate count
current content-utilization stage counts
current snapshot count
current live panel route count
current registered panel route count
current campaign-day owners
```

Store as an artifact or task-log table.

---

## P0.2 Enumerate all test construction seams

Search for:
- direct `new <CampaignOwnedSystem>()`;
- `CreateDefault`;
- `SeedCatalog`;
- `SeedStartingSupplies`;
- hardcoded survivor IDs;
- hardcoded power/schedule defaults;
- fixture-only world builders;
- test-only data arrays;
- test construction that bypasses loaders;
- UI tests that build partial hosts.

Produce:

```text
file
line/symbol
constructed_type
production_owner
test_purpose
uses_real_data?
uses_real_campaign_instance?
verdict
```

Verdicts:
- `AUTHORITY_BACKED`
- `SYNTHETIC_EXPLICIT`
- `IMPLICIT_FIXTURE`
- `FRESH_SYSTEM_DEFECT`
- `DEMO_ONLY`
- `REPLACE`

The list itself is an acceptance artifact.

---

## P0.3 Enumerate all data authorities used by selftests

At minimum:
- items;
- recipes;
- quests;
- roles;
- weather;
- events;
- medical data;
- survivors;
- world topology;
- content catalogs.

For each:
- shipped source path;
- loader;
- test fallback if any;
- selftest authority path;
- fixture policy.

---

## P0.4 Capture runtime evidence baseline

Record current content utilization counts from the actual current artifact:

```text
DISCOVERED
LOADED
DESERIALIZED
REGISTERED
QUERIED
SELECTED
EFFECT_PRODUCED
bestEvidence STATIC
bestEvidence RUNTIME
```

Do not freeze old numbers from the source plan if they changed.

---

# TASK 27A — Fidelity: Tests Must Use the Shipped Authority

# 27A.0 Goal

Make the default testing path construct the same data/model authority the player uses.

Synthetic fixtures remain legal but must be impossible to mistake for production certification.

---

## 27A.1 Inventory every implicit fallback

Search and classify:

- hardcoded item catalogs;
- starting supplies;
- default power grids;
- default shelter schedules;
- demo survivor IDs;
- demo signal IDs;
- default expedition locations;
- default world state;
- fresh skill systems;
- fresh economy systems;
- test-only condition states.

The source plan explicitly names examples such as:
- `surv_01`;
- `sv_cohort_demo`;
- `PowerGridHostSession.CreateDefault`;
- literal 800 W schedule grid.

Do not limit the sweep to those names.

---

## 27A.2 Make fixture creation explicit in APIs

Preferred pattern:

```csharp
InventoryHostSession.Create(dataDir)
```

for production/authority-backed paths.

Synthetic path:

```csharp
InventoryHostSession.CreateForFixture(...)
```

or:

```csharp
SeedCatalogForTest(...)
```

Requirements:
- synthetic method name is unmistakable;
- default constructor must not silently seed production-like demo data;
- production selftests call authority-backed path.

---

## 27A.3 Eliminate ambiguous default constructors

Where a host session default constructor creates fixture state:
- remove it if safe;
- make it internal to test assembly;
- or require explicit fixture builder.

Avoid breaking legitimate pure unit tests unnecessarily.

The target is semantic clarity.

---

## 27A.4 Selftests resolve real data directory

Every `--*-selftest` that claims shipped-runtime behavior should resolve:

```text
CatalogPath.ResolveDataDir()
```

or the current canonical equivalent.

Requirements:
- same path-resolution logic as game;
- no separate CI data path;
- fail clearly if data unavailable;
- exported-build selftest uses packaged data authority.

---

## 27A.5 Authority-backed inventory tests

Migrate current live-behavior tests to use actual `items.json`.

For significant item behavior, assert catalog fields are loaded as authored:
- hunger restore;
- thirst restore;
- radiation protection;
- durability;
- contamination.

This directly protects Wave-2 Plans 21/22.

---

## 27A.6 Fidelity assertion between fixture and authority

For explicitly synthetic fixtures that intentionally mirror shipped items:
- compare selected behaviorally significant fields;
- report divergent item IDs and fields.

Do not require every synthetic fixture to equal authority if its purpose is deliberately artificial.

Policy must distinguish:
- "synthetic minimal unit fixture";
- "mirror fixture expected to track authority".

---

## 27A.7 Create shared `CampaignFixture`

Create under:

`Ashfall.Core.Tests/Fixture/CampaignFixture.cs`

Purpose:
- construct a campaign-shaped test environment;
- use real authority loaders;
- accept explicit deterministic seed;
- provide campaign-owned system references;
- expose composition points needed by tests.

Possible API:

```csharp
var fixture = CampaignFixture.CreateAuthorityBacked(
    dataDir,
    seed: 12345);
```

---

## 27A.8 CampaignFixture responsibilities

It should:
- resolve/load shipped catalogs;
- compose relevant campaign owners;
- use production defaults intentionally;
- provide seed control;
- expose save/load helper;
- expose day advance helper;
- expose current authority references.

It should not:
- duplicate the entire game host;
- hide random test mutations;
- create systems independently from campaign ownership.

---

## 27A.9 Synthetic fixture builder

Provide explicit:

```csharp
CampaignFixture.CreateSynthetic(...)
```

or a separate builder.

Use only for:
- boundary values;
- malformed data tests;
- impossible edge cases;
- micro-unit behavior.

Name the test class accordingly when useful.

---

## 27A.10 Ban fresh campaign-owned systems in host selftests

Add source-scan gate modeled after existing repository gates.

Scope:
- `src/Host/*SelfTest*.cs`;
- `src/Main.UiTests*.cs`;
- real-campaign journey files.

Gate logic:
- identify `new <SystemType>` where type is campaign-owned;
- allow explicit whitelisted synthetic tests only;
- fail with file, line, type, expected owner.

Avoid regex-only false positives if a small parser/source list is practical.

---

## 27A.11 Campaign-owned type registry for gate

Build the source list from:
- composition root;
- campaign service registry;
- `_campaignDay.Register(...)`;
- explicit owner map.

Do not maintain a second arbitrary manual list if current project can derive ownership.

---

## 27A.12 Test the no-fresh-system gate itself

Create an intentionally bad sample under gate test fixture or injected source string.

Assert:
- gate detects construction;
- message identifies type/path;
- whitelisted synthetic construction does not fail.

A gate that has never been observed failing is not trusted.

---

## 27A.13 Golden save fixture strategy

Commit 2–3 checksummed saves:

### Early
- minimal progression;
- day low;
- few systems active.

### Mid
- survivor state changed;
- inventory/kitchen/power/medical/duty state active;
- some quests/events.

### Late
- endgame-relevant flags/state;
- multiple save sections;
- long campaign history.

Use current save version/migration rules.

---

## 27A.14 Golden save fixture generation

Provide reproducible generation script:
- authority-backed campaign;
- pinned seed;
- scripted choices;
- save;
- checksum;
- manifest.

Do not hand-edit JSON save payloads unless the save format explicitly expects handcrafted fixtures.

---

## 27A.15 Golden save fixture manifest

Create metadata:

```text
fixture_name
save_version
seed
day
expected_checksum
scenario_policy
required_sections
generated_by
last_reviewed_commit
```

This makes save changes reviewable.

---

## 27A.16 Exported-build reuse

Use the same golden saves in Plan 26B exported-build smoke tests.

Acceptance:
- exported artifact loads all supported golden saves;
- migration behavior is explicit;
- no separate export-only fixture set.

---

## 27A.17 Determinism fixture policy

Generate pinned:
- 30-day digest;
- 180-day digest;
- 360-day digest.

Digest should summarize stable campaign state:
- key survivor stats;
- inventory;
- world state;
- quests;
- save section checksums;
- event history hashes;
depending on existing harness.

Do not hash nondeterministic timestamps/log IDs.

---

## 27A.18 Digest review rule

A changed digest is not automatically failure forever.

Process:
1. CI shows diff;
2. developer explains intentional behavioral change;
3. tests verify new behavior;
4. baseline updated in reviewed commit.

No silent regeneration.

---

## 27A.19 De-duplicate authored item definitions

Source plan identifies item definitions duplicated in:
- `items.json`;
- `InventoryHostSession`;
- `CraftingHostSession`.

Reconcile current source.

Preferred:
- shipped item facts live in data;
- host reads catalog;
- tests use fixture builder or data;
- synthetic definitions are minimal and explicitly test-only.

---

## 27A.20 De-duplicate starting supplies

Where starting supplies appear in both code and data:
- establish one authority;
- tests reference authority;
- synthetic starting inventory belongs only to explicit fixtures.

---

## 27A.21 Fixture policy documentation

Create:

`docs/testing/FIXTURE_POLICY.md`

Must define two legal categories.

### Authority-backed fixture
Use when testing:
- shipped gameplay behavior;
- integration;
- selftests;
- save/load;
- journeys;
- content/runtime evidence.

### Explicit synthetic fixture
Use when testing:
- edge cases;
- malformed values;
- pure formulas;
- narrow failure modes.

Include worked examples.

---

## 27A.22 Naming policy

Recommended test naming:

```text
AuthorityBacked_...
Synthetic_...
RealCampaign_...
GoldenSave_...
```

Not mandatory for every test, but useful for ambiguous test suites.

---

## 27A.23 Data authority fidelity tests

Add `DataAuthorityFidelityTests.cs`.

Examples:
- authority-backed fixture loads same item count as loader;
- selected known item fields match source;
- campaign fixture references same catalog object/definitions where appropriate;
- no silent fallback activated.

---

## 27A.24 Host selftest migration order

Migrate highest-value first:
1. inventory;
2. save;
3. panel lifecycle;
4. onboarding;
5. day-one;
6. real-campaign journey;
7. remaining CLI panel/selftests.

Each migration should show before/after authority path.

---

## 27A.25 Fixture fallback instrumentation

During transition, optionally log in test:
- fixture mode;
- authority-backed mode.

Do not emit noisy production logs.

Goal: identify accidental synthetic path use.

---

## 27A.26 27A acceptance metrics

Record:

```text
implicit fixture constructors before/after
production selftests using real data before/after
fresh campaign-owned system constructions before/after
duplicate authored item definition sites before/after
golden saves count
determinism digests count
```

### 27A DoD

Every production-certifying selftest uses the same shipped data authority as the player, or the test explicitly names itself as synthetic.

---

# TASK 27B — Coverage, Save Fidelity, Determinism & Assertion Strength

# 27B.0 Goal

Introduce measurable, reviewable, risk-weighted coverage.

The plan does not chase a global percentage.

It protects:
- state persistence;
- deterministic simulation;
- continuity-critical domains;
- high-risk behavior assertions.

---

## 27B.1 Add coverage collector

Use central package management.

Add:
- `coverlet.collector`;
- compatible pinned version;
- test configuration for Cobertura.

Output:
`artifacts/coverage/coverage.cobertura.xml`
or current repository artifact convention.

Ensure artifacts are ignored appropriately.

---

## 27B.2 Local coverage command

Document exact command, e.g.:

```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj \
  --collect:"XPlat Code Coverage" \
  --results-directory artifacts/coverage
```

If repository tooling wraps this, prefer one script.

---

## 27B.3 Publish baseline

Create:

`artifacts/coverage-baseline.json`

and:

`docs/testing/COVERAGE.md`

Record:
- overall line coverage;
- overall branch coverage;
- risk-slice coverage;
- date/commit;
- exclusions;
- collector version.

---

## 27B.4 Define continuity-critical slices

Minimum source-plan set:

- `Save/**`
- `Survivors/**`
- `Radiation/**`
- `Medical/**`
- `Economy/**`
- `Campaign/**`

Consider including:
- `Inventory/**`
- `DutyRoster/**`
- `Endgame/**`
- `World/**`
if current architecture and Waves 1–2 justify them.

Document final list.

---

## 27B.5 No-decrease gate

For each protected slice:
- current line/branch coverage must not drop below baseline beyond documented tolerance;
- intentional reductions require reviewed baseline update.

Prefer exact/no-decrease where collector stability allows.

---

## 27B.6 Avoid line-number fragility

Coverage parser should group by stable path/class.

Normalize:
- path separators;
- generated files;
- partial classes.

Do not compare raw XML ordering.

---

## 27B.7 Capture/Restore source scan

Search Core for:
- `CaptureState`;
- `RestoreState`.

Generate stateful-system registry.

For each pair:
- require at least one round-trip contract test.

---

## 27B.8 Round-trip contract template

At minimum:

### Test A — Clean round trip

```text
state A
→ capture
→ new authority
→ restore
→ capture B
assert semantic equality
```

### Test B — Mutation changes checksum/state

```text
capture A
mutate
capture B
assert A != B
```

### Test C — invalid/null checksum handling

Use current store contract.

Do not force checksum semantics onto systems that do not use checksums; match repository architecture while preserving equivalent integrity tests.

---

## 27B.9 Generator/source-scan enforcement

Follow existing save-store coverage gate style.

Gate:
- every production stateful system is mapped to test class/contract;
- newly added stateful system fails CI until covered.

---

## 27B.10 Test generated round-trips semantically

Avoid generating meaningless tests that only invoke methods.

Require registry metadata:
- constructor/factory;
- meaningful mutation callback;
- equality/digest method.

If a system cannot support generic template, it must provide explicit custom test.

---

## 27B.11 Campaign-day owner inventory

Generate authoritative list of `_campaignDay.Register(...)` owner IDs.

Store artifact:

```text
owner_id
type
registration_site
determinism_test
```

---

## 27B.12 Paired-seed replay contract

For each owner:

```text
fixture A seed S
fixture B seed S
same initial state
same inputs
advance owner/day
compare digest
```

Then:
- seed S vs T may differ where randomness expected;
- same seed must not differ.

---

## 27B.13 Determinism owner gate

A newly registered day owner without determinism mapping fails CI.

This converts deterministic simulation from convention to enforced invariant.

---

## 27B.14 Assertion-strength sweep

Search for weak patterns:

```text
Assert.NotNull
Assert.True(true)
Assert.DoesNotThrow
no-exception-only tests
presence-only route assertions
```

Do not blanket-delete them; classify risk.

Create ranked table:
- high-risk production behavior;
- medium;
- harmless structural.

---

## 27B.15 High-risk behavior list

At minimum include Waves 1–2 defect families:

1. ending context derivation;
2. inventory consume effects;
3. panel campaign authority identity;
4. equipment condition/wear;
5. moral choice consequence caller;
6. duty fitness;
7. needs external modifiers;
8. kitchen serve meal;
9. medicine effects;
10. save/load continuity.

Each requires state-changing assertions.

---

## 27B.16 Replace no-op assertions

Examples:

Bad:

```csharp
Assert.NotNull(result);
```

Better:

```text
item count decreased
survivor hunger changed by authored amount
event emitted once
same authority reference used
```

---

## 27B.17 Branch reachability checks

For conditional engines:
- verify meaningful branches reachable from production state.

Especially:
- epilogue;
- duty fitness;
- medical policy;
- equipment failure/wear;
- ration/grievance.

Do not satisfy branch coverage with direct synthetic boolean construction alone when integration behavior matters.

---

## 27B.18 Mutation spot-check scope

Run bounded mutation testing on a few pure functions named by source:

- radiation exposure calculation;
- needs decay;
- equipment condition wear;
- kitchen spoilage days.

Potential mutation operators:
- invert comparison;
- ± boundary;
- zero constant;
- negate multiplier.

---

## 27B.19 Mutation tooling discipline

Use a tool compatible with current .NET stack.

Scope:
- limited files/functions;
- Tier-2/nightly;
- no full-repo mutation campaign.

Record:
- mutants generated;
- killed;
- survived;
- timeout/error.

---

## 27B.20 Survived-mutant review

A survived mutant triggers:
1. test gap classification;
2. whether behavior matters;
3. add assertion if meaningful;
4. document exclusion if equivalent/non-behavioral.

---

## 27B.21 Coverage gate script

Create:

`scripts/ci/coverage-gate.sh`

Responsibilities:
- locate Cobertura artifact;
- parse metrics;
- compare risk-slice baseline;
- check round-trip registry/gate;
- clear failure summary.

Keep script deterministic and platform-compatible with CI.

---

## 27B.22 Gate manifest registration

Update:

`docs/ci/CI_GATE_MANIFEST.json`

Fields:
- gate name;
- tier;
- command;
- expected summary;
- artifact paths;
- owner/purpose.

---

## 27B.23 Keep coverage out of fast path

Coverage belongs Tier 2.

`verify-fast.sh` remains useful locally.

Do not make every local edit pay coverage instrumentation cost.

---

## 27B.24 Local explicit invocation

Provide:

```bash
scripts/ci/coverage-gate.sh
```

or:

```bash
scripts/ci/run-gate.sh coverage
```

according to current conventions.

---

## 27B.25 PR step summary

Use existing GitHub summary helper.

Report:
- overall coverage;
- protected-slice deltas;
- round-trip coverage count;
- determinism owner count;
- mutation spot-check summary if run.

---

## 27B.26 Baseline ratchet policy

Monthly or deliberate ratchet only.

Rules:
- baseline never silently reduced;
- increase names newly protected code;
- review evidence included.

---

## 27B.27 Test the coverage gate

Inject test coverage artifact or temporary baseline mismatch.

Assert:
- lower protected-slice coverage fails;
- dropped round-trip mapping fails;
- allowed baseline update path is explicit.

---

## 27B.28 27B acceptance metrics

Record:

```text
overall line coverage
overall branch coverage
protected slice metrics
CaptureState declarations
Capture/Restore pairs covered
campaign day owners
day owners with determinism test
high-risk weak assertions found
high-risk weak assertions replaced
mutation kill rate for sampled formulas
```

### 27B DoD

Coverage exists, is published, protects the right code, round-trip omissions fail CI, determinism ownership is enforced, and sampled behavior assertions kill meaningful mutations.

---

# TASK 27C — Runtime Evidence and Real Campaign Journeys

# 27C.0 Goal

Prove actual runtime behavior through real campaign boot paths.

Static discovery remains useful, but runtime evidence becomes the authority for "did the game actually use this content/system?"

---

## 27C.1 Real boot instrumentation

Drive `ContentUtilizationRuntimeCollector` through:
- `--day1-selftest`;
- `--real-campaign-journey-selftest`;
- exported-build smoke where feasible.

Do not create a separate synthetic content collector path.

---

## 27C.2 Define utilization stage semantics

Document exact meaning:

### DISCOVERED
File/catalog exists and scanner sees it.

### LOADED
Bytes/content source opened.

### DESERIALIZED
Typed data object successfully constructed.

### REGISTERED
Definition entered runtime registry/catalog.

### QUERIED
Runtime consumer requested it or category containing it.

### SELECTED
Runtime logic chose a specific definition/content entry for use.

### EFFECT_PRODUCED
Selection caused observable simulation/UI consequence.

No stage may be populated just to improve a metric.

---

## 27C.3 Instrument loader seam

When typed object deserializes:
- emit `DESERIALIZED`.

Avoid:
- per-frame instrumentation;
- duplicate count for one load unless metric semantics say event count rather than unique content.

Prefer unique content ID + stage evidence.

---

## 27C.4 Instrument registry seam

When definition becomes available to runtime:
- emit `REGISTERED`.

Examples:
- item catalog;
- recipe catalog;
- events;
- quest definitions;
- role definitions.

---

## 27C.5 Instrument query seam

When a system requests content:
- emit `QUERIED`.

Do not equate broad collection enumeration with selection.

---

## 27C.6 Instrument selection seam

When runtime picks an actual content ID:
- emit `SELECTED`.

Examples:
- chosen event;
- chosen recipe;
- chosen encounter;
- selected item effect;
- selected journal entry.

---

## 27C.7 Instrument effect seam

`EFFECT_PRODUCED` requires observable consequence.

Examples:
- item consume changed needs;
- event changed world flag;
- recipe created item;
- quest choice changed campaign state;
- weather event changed system state;
- panel action mutated authority.

Document per-domain effect criteria.

---

## 27C.8 Runtime evidence identity

Evidence entry should include where feasible:

```text
content_id
catalog
stage
day
consumer
effect_type
source_journey
```

Do not store huge payloads.

---

## 27C.9 Runtime baseline artifact

Create:

`artifacts/content-utilization-baseline.json`

Track:
- unique IDs per stage;
- runtime evidence count;
- effect-produced count;
- baseline commit.

---

## 27C.10 Monotonic gate strategy

Avoid requiring every metric to rise forever.

Primary:
- protected runtime catalogs do not regress unexpectedly;
- `EFFECT_PRODUCED` for previously exercised journeys does not fall;
- `SELECTED` remains ≥ baseline for journey scope;
- new Wave features should add named expected evidence.

---

## 27C.11 First-hour / early-campaign journey

Create a real campaign journey spanning Wave-2 seams.

Source-plan sequence:

```text
new game
→ read guidance
→ ration cut
→ craft
→ dispatch
→ storm
→ dose rises
→ mask fails
→ treat
→ someone dies
→ shift vacated
→ memorial
→ day-30 briefing
```

Exact day wording may adapt to current systems.

---

## 27C.12 Journey assertion style

At each step assert both:
1. current system changed;
2. next system can observe/use the changed state.

Example:

```text
storm selected
→ RadiationSystem dose increased
→ medical panel/ledger sees dose
```

This is the defining integration assertion.

---

## 27C.13 Guidance step

Assert:
- guidance content selected;
- player-visible/read state changed;
- next action remains reachable.

Do not only assert panel opened.

---

## 27C.14 Ration cut step

Assert:
- ration policy/stock changed;
- survivor needs/social system observes result;
- event history records consequence.

---

## 27C.15 Craft step

Assert:
- real recipe selected;
- bill consumed;
- item produced;
- content utilization reaches `EFFECT_PRODUCED`.

---

## 27C.16 Dispatch step

Assert:
- expedition uses campaign authority;
- survivor/duty/fitness conditions applied;
- expedition state persists.

---

## 27C.17 Storm step

Assert:
- real weather content selected;
- world/system effect occurs;
- dose/exposure next system changes.

---

## 27C.18 Equipment failure step

Where Wave-2 wear logic supports mask failure:
- durability/condition reaches failure threshold;
- protective effect changes;
- exposure consequence observed.

No synthetic direct "mask failed=true" injection in journey.

---

## 27C.19 Treatment step

Use Plan 22 single consume authority.

Assert:
- medicine removed from real inventory;
- treatment effect applied;
- dose ledger records it;
- event/cue path not duplicated.

---

## 27C.20 Death step

Assert:
- survivor state dies through actual system;
- duty vacates;
- ration requirements change;
- memorial created;
- grief/briefing consequence follows.

This validates Plan 24C chain.

---

## 27C.21 Day-30 briefing

Assert briefing includes:
- selected meaningful events;
- correct ordering/attribution;
- no stale duplicate events after save/load if journey includes persistence.

---

## 27C.22 Ending journey

Script a fixed-choice 200-day campaign.

Requirements:
- authority-backed data;
- pinned seed;
- production ending projector;
- no direct context booleans.

Assert:
- derived context matches campaign state;
- outcome branch expected for policy.

---

## 27C.23 Three-policy ending comparison

Run policies A/B/C.

Assert:
- state digests differ;
- derived epilogue contexts differ;
- endings differ on meaningful branch/outcome identity.

No need to assert every prose character if a stable outcome ID exists.

---

## 27C.24 Persistence journey

Flow:

```text
new game
→ mutate multiple systems
→ open representative panels
→ save
→ quit/recreate host
→ load
→ compare
```

Compare:
- campaign authority state;
- panel-bound values;
- event/day history;
- key reference identities;
- selected current survivor/route state where persisted.

---

## 27C.25 Panel-value snapshot model

Create a stable semantic snapshot per live panel.

Avoid raw screenshot-only persistence assertions.

Examples:
- displayed numeric values;
- status IDs;
- selected entity;
- button enabled states;
- authority identity.

---

## 27C.26 Session authority identity after load

For each live panel in journey:
- panel binding authority must be reference-equal to campaign owner;
- old session event must not refresh panel.

This is runtime twin of Plan 15C/16B/16C gates.

---

## 27C.27 Input/accessibility journey

For every player-routable live panel:
- reachable by keyboard only;
- focus order defined;
- actionable controls reachable;
- escape/back returns safely;
- no focus trap.

Use current accessibility harness.

---

## 27C.28 Input journey scope

Use live route count from Plan 16A, not all registered descriptors.

Prototype/shelved panels are excluded from player-navigation journey.

---

## 27C.29 Snapshot expansion policy

For each live route:
- golden image with meaningful populated fixture state;
- fixture uses authority-backed or explicit UI synthetic state per policy;
- manifest maps image to route and fixture.

Do not generate goldens for shelved consoles merely to increase count.

---

## 27C.30 Snapshot approval note

Any golden change requires:
- reason;
- task/commit;
- expected visual difference.

No blind `--update-snapshots`.

---

## 27C.31 Runtime liveness assertion

During panel route traversal:

```text
panel is bound
AND panel authority/session reference == campaign authority/session
AND at least one meaningful state read exists
```

This is stronger than setup metadata.

---

## 27C.32 Live action proof

For mutating panels:
- trigger representative action;
- assert campaign authority changes.

For read-only panels:
- assert displayed values come from campaign read model.

---

## 27C.33 Journey seeding

Each journey:
- pins campaign seed;
- pins explicit policy inputs;
- isolates external clock;
- avoids locale-sensitive ordering.

---

## 27C.34 Culture invariance

Run selected journey under:
- invariant/default CI culture;
- at least one alternate culture if formatting-sensitive code has historically failed.

Assertions should prefer semantic IDs/numbers over localized rendered prose where possible.

---

## 27C.35 Time/randomness isolation

No:
- wall-clock timing;
- `Thread.Sleep` for game progression;
- unseeded randomness;
- hash-order-dependent expected result.

Use campaign clock and seeded RNG.

---

## 27C.36 Flake policy

If a journey flakes:
1. reproduce with same seed;
2. identify nondeterminism/race/external dependency;
3. fix;
4. rerun soak;
5. remove from gate if reliability cannot be achieved.

Do not add infinite retries.

---

## 27C.37 Tiering

### Fast tier
- unit/contracts;
- static gates;
- very short selftests.

### Tier 2
- coverage;
- authority-backed journeys;
- save/load journey;
- runtime evidence.

### Nightly/soak
- 180/360-day replay;
- multi-policy ending;
- mutation spot-check;
- repeated journey stability.

---

## 27C.38 CI time budget

Measure each journey duration.

Publish:
- median;
- p95;
- artifact size.

Keep Tier-2 bounded.

If one journey becomes too large:
- preserve invariant coverage;
- split into deterministic phases;
- do not weaken assertions.

---

## 27C.39 Journey documentation

Create:

`docs/testing/JOURNEYS.md`

For each:
- name;
- purpose;
- systems crossed;
- invariants;
- seed;
- expected runtime;
- continuity gaps retired;
- CI tier.

---

## 27C.40 Journey verbs

Prefer explicit verbs, e.g.:

```text
--day1-selftest
--real-campaign-journey-selftest
--ending-journey-selftest
--persistence-journey-selftest
--input-journey-selftest
```

Use repository naming conventions.

---

## 27C.41 Exported-build smoke integration

After host journeys pass:
- run Plan 26B export smoke;
- verify packaged data authority;
- load golden save;
- run basic real-campaign boot.

The exported artifact is the highest-fidelity environment.

---

## 27C.42 Runtime evidence gate

Add a gate that:
- runs selected real campaign journey;
- writes utilization artifact;
- compares expected minimum stages/effect IDs;
- fails if a required effect disappears.

---

## 27C.43 Runtime collector idempotence

Repeated journey instrumentation should not:
- double count unique evidence unexpectedly;
- alter simulation;
- change selection order.

Collector must be observational.

---

## 27C.44 Runtime artifact reproducibility

Same journey/seed should produce same normalized evidence artifact.

Exclude:
- timestamps;
- machine paths;
- nondeterministic ordering.

---

## 27C.45 27C acceptance metrics

Record:

```text
runtime evidence unique catalogs
DESERIALIZED count
REGISTERED count
QUERIED count
SELECTED count
EFFECT_PRODUCED count
live routes journey-covered
live routes snapshot-covered
runtime authority-identity failures
journey count
journey median/p95 runtime
flake count over soak
```

### 27C DoD

Runtime content evidence comes from actual boot/journey behavior, and named journeys fail when one system stops feeding the next.

---

# 5. Cross-Task Dependency Graph

```text
26A / 26B artifact/bootstrap fidelity
              │
              ▼
        27A FIXTURE FIDELITY
              │
        ┌─────┴────────────┐
        ▼                  ▼
27B COVERAGE /        27C RUNTIME
ROUND-TRIP /          EVIDENCE SEAMS
DETERMINISM                │
        │                  │
        └──────────┬───────┘
                   ▼
             REAL JOURNEYS
                   │
         ┌─────────┼──────────────┐
         ▼         ▼              ▼
     first-hour  persistence     ending
         │         │              │
         └─────────┼──────────────┘
                   ▼
           SHIP-INTEGRITY GATES
```

Plan 15C runtime twin:
- static liveness gate from Wave 1;
- reference-identity journey assertion in 27C.

---

# 6. Verification Matrix

| Concern | 27A | 27B | 27C |
|---|---|---|---|
| shipped data authority | owns | measures | executes |
| synthetic fixtures | classifies | excludes/reports | limited |
| fresh system defects | source gate | coverage consequence | runtime identity |
| save fidelity | golden fixtures | round-trip gate | persistence journey |
| determinism | pinned fixtures | owner gate | long journeys |
| test assertion quality | fidelity assertions | mutation/weak assertion review | state-transition journeys |
| content utilization | authority basis | optional coverage | runtime owner |
| panel liveness | prevents fresh systems | measured paths | reference identity |
| snapshots | fixture policy | not primary | live route expansion |
| export fidelity | reused save/data | optional coverage artifact | exported smoke |

---

# 7. Failure Injection Matrix

## N27.1 Selftest uses synthetic catalog accidentally
Expected: fidelity/source gate fails.

## N27.2 Fixture item diverges from authority
Expected: fidelity test identifies item + field.

## N27.3 Fresh `SkillProgressionSystem` added to a selftest
Expected: no-fresh-system gate fails.

## N27.4 Golden save checksum altered unexpectedly
Expected: fixture manifest/checksum test fails.

## N27.5 Same seed gives different 30-day digest
Expected: determinism fixture fails.

## N27.6 CaptureState pair loses round-trip test
Expected: coverage/save contract gate fails.

## N27.7 New campaign-day owner added without determinism mapping
Expected: owner gate fails.

## N27.8 Protected coverage slice drops
Expected: coverage gate fails.

## N27.9 `DegradeRate`-like zero mutant survives
Expected: mutation review identifies assertion gap.

## N27.10 Runtime content selected but no effect
Expected: `EFFECT_PRODUCED` missing; journey/gate fails.

## N27.11 Panel route registered but bound to fresh authority
Expected: runtime liveness identity assertion fails.

## N27.12 Journey only asserts panel opens
Expected: journey review/test helper requires state assertions.

## N27.13 Golden snapshot updated without approval metadata
Expected: manifest/CI policy gate fails if implemented.

## N27.14 Flaky journey
Expected: soak reveals; gate removed or defect fixed—never ignored.

---

# 8. Artifact Set

Plan 27 should create or update:

```text
docs/testing/FIXTURE_POLICY.md
docs/testing/COVERAGE.md
docs/testing/JOURNEYS.md

Ashfall.Core.Tests/Fixture/CampaignFixture.cs
Ashfall.Core.Tests/DataAuthorityFidelityTests.cs

scripts/ci/coverage-gate.sh

artifacts/coverage-baseline.json
artifacts/content-utilization-baseline.json

golden save fixtures + manifest
determinism digest baseline

src/Host/JourneySelfTests.cs
src/Main.UiTests.RealCampaignJourney.cs

docs/ci/CI_GATE_MANIFEST.json
docs/ui/SNAPSHOT_COVERAGE.md
```

Exact names may adapt to existing repository structure.

---

# 9. Golden Save Fixture Contract

Each golden save must be:
- generated from shipped authority;
- checksummed;
- versioned by save version;
- reproducible from script;
- loadable in current test host;
- used by at least one persistence/export smoke.

Do not treat golden saves as immutable forever.

Migration changes may update them with explicit review.

---

# 10. Determinism Digest Contract

Digest should include stable authoritative state.

Suggested categories:
- campaign day;
- survivor roster + needs;
- inventory;
- world flags;
- economy summary;
- quest progression;
- duty assignments;
- radiation/medical state;
- event history normalized;
- endgame state.

Avoid:
- rendered strings;
- timestamps;
- machine-specific paths;
- random GUIDs not part of simulation authority.

---

# 11. Coverage Baseline Governance

Baseline updates require:
- previous/new metrics;
- explanation;
- protected slice deltas;
- test additions/removals;
- reason if coverage dropped.

A refactor that lowers coverage may still be acceptable, but only through reviewed decision.

---

# 12. Assertion Quality Rubric

Score high-risk tests:

### Level 0 — Presence
- file/class exists;
- route registered.

### Level 1 — Construction
- object non-null;
- no throw.

### Level 2 — Local behavior
- method returns expected value.

### Level 3 — State transition
- authoritative state changes as specified.

### Level 4 — Cross-system consequence
- next system sees/uses state.

### Level 5 — Persistence/replay
- effect survives save/load and deterministic replay.

For continuity-critical behavior, target Level 4–5.

---

# 13. Journey Acceptance Rubric

A journey stage is valid only when it has:

```text
PRECONDITION
ACTION
AUTHORITY CHANGE
DOWNSTREAM OBSERVER CHANGE
ASSERTION
```

Example:

```text
PRE: survivor owns iodine
ACTION: consume iodine
AUTHORITY CHANGE: inventory -1, iodine state active
DOWNSTREAM: radiation exposure calculation observes protection
ASSERTION: protected exposure < unprotected exposure
```

This prevents "journey theater" where steps execute but do not prove connections.

---

# 14. Snapshot Policy Integration

Snapshot tests support visual regression, not behavioral correctness.

Rules:
- only live routes matter for player coverage;
- fixture-populated state must be meaningful;
- behavior test still required for mutating panel;
- snapshot approval note required.

Never substitute screenshot coverage for authority/reference assertions.

---

# 15. Performance / CI Budget Guardrails

Coverage, mutation, and journeys can become expensive.

Track per-gate duration.

Targets:
- fast tier remains developer-friendly;
- Tier-2 remains bounded;
- nightly handles long replay/mutation.

Do not parallelize in ways that break deterministic shared fixtures unless isolation is guaranteed.

---

# 16. Reliability Strategy

For every journey:
- isolated save directory;
- isolated artifact directory;
- pinned seed;
- invariant culture;
- no network dependency;
- no wall-clock sleeps;
- deterministic output normalization.

Clean up artifacts between runs.

---

# 17. Recommended Commit Breakdown

```text
27A-1 fixture-seam inventory + failing fidelity tests
27A-2 explicit fixture constructors
27A-3 authority-backed CampaignFixture
27A-4 migrate host/selftests to real data
27A-5 no-fresh-system gate
27A-6 golden saves + determinism fixtures + fixture policy
27A-7 duplicate authored seed removal

27B-1 coverlet + baseline docs
27B-2 coverage parser/gate
27B-3 Capture/Restore coverage registry
27B-4 campaign-day determinism gate
27B-5 high-risk assertion-strength repairs
27B-6 bounded mutation spot-check
27B-7 CI manifest + PR summary + gate selftest

27C-1 runtime collector stage instrumentation
27C-2 utilization baseline/gate
27C-3 first-hour journey
27C-4 ending journey
27C-5 persistence journey
27C-6 input/runtime-liveness journey
27C-7 snapshot live-route expansion
27C-8 Tier-2/nightly integration + exported smoke + docs
```

---

# 18. Risk Register

## R27.1 Fixture migration breaks many tests

Expected and acceptable if tests depended on demo data.

Mitigation:
- migrate in slices;
- distinguish real behavior vs synthetic unit tests;
- fix assumptions rather than recreating demo authority.

---

## R27.2 Coverage creates vanity pressure

Mitigation:
- risk slices;
- no global ship percentage;
- review deltas.

---

## R27.3 Generated round-trip tests become shallow

Mitigation:
- meaningful mutation registry;
- custom tests where generic contract insufficient.

---

## R27.4 Determinism gate becomes brittle

Mitigation:
- stable digest;
- exclude non-simulation metadata;
- reviewed baseline changes.

---

## R27.5 Runtime instrumentation changes performance/behavior

Mitigation:
- observational collector;
- low allocation;
- optional release compilation if needed;
- determinism comparison with collector on/off.

---

## R27.6 Journey becomes too broad to debug

Mitigation:
- stage assertions;
- diagnostic state snapshots;
- split while preserving cross-system link checks.

---

## R27.7 Snapshot explosion

Mitigation:
- only live routes;
- fixture categories;
- approval notes.

---

## R27.8 Flake normalization

Mitigation:
- explicit no-unexplained-flake policy;
- soak metrics;
- remove unreliable gate.

---

# 19. Acceptance Checklist

## 27A — Fidelity

- [ ] construction seam inventory complete
- [ ] data authority inventory complete
- [ ] implicit fixture constructors classified
- [ ] default host construction no longer silently seeds demo data
- [ ] explicit synthetic fixture APIs exist
- [ ] production selftests resolve real data dir
- [ ] authority-backed item fidelity tests exist
- [ ] CampaignFixture exists
- [ ] CampaignFixture uses campaign-owned system graph
- [ ] synthetic fixture path clearly named
- [ ] no-fresh-system source gate exists
- [ ] campaign-owned type list derived or authoritative
- [ ] gate failure test exists
- [ ] early/mid/late golden saves committed
- [ ] golden save manifest committed
- [ ] export smoke reuses golden saves
- [ ] 30/180/360-day digests pinned
- [ ] digest baseline updates require review
- [ ] duplicate item seeds removed/reconciled
- [ ] duplicate starting-supply authorities reconciled
- [ ] fixture policy documentation complete
- [ ] major host selftests migrated
- [ ] fidelity metrics recorded

## 27B — Coverage and assertion quality

- [ ] coverage collector added centrally
- [ ] Cobertura artifact generated
- [ ] local coverage command documented
- [ ] coverage baseline published
- [ ] protected slices documented
- [ ] no-decrease gate implemented
- [ ] coverage parser path-normalized
- [ ] CaptureState/RestoreState scan exists
- [ ] every stateful pair has round-trip contract
- [ ] generic template has meaningful mutation
- [ ] campaign-day owner inventory generated
- [ ] every owner has determinism mapping
- [ ] newly added owner without test fails CI
- [ ] weak assertion sweep completed
- [ ] high-risk weak tests strengthened
- [ ] bounded mutation spot-check runs
- [ ] survived mutants reviewed
- [ ] coverage gate registered in manifest
- [ ] coverage stays Tier 2
- [ ] PR step summary shows delta
- [ ] baseline ratchet policy documented
- [ ] coverage gate failure test exists
- [ ] metrics recorded

## 27C — Runtime evidence and journeys

- [ ] runtime collector driven by real boot
- [ ] stage semantics documented
- [ ] DESERIALIZED instrumented
- [ ] REGISTERED instrumented
- [ ] QUERIED instrumented correctly
- [ ] SELECTED instrumented
- [ ] EFFECT_PRODUCED instrumented
- [ ] runtime evidence carries stable identity
- [ ] utilization baseline committed
- [ ] monotonic/protected gate policy defined
- [ ] first-hour journey exists
- [ ] every journey stage asserts downstream observation
- [ ] real craft path proven
- [ ] real dispatch path proven
- [ ] storm→dose path proven
- [ ] equipment-failure consequence proven where supported
- [ ] treatment path proven
- [ ] death→duty→memorial path proven
- [ ] briefing outcome proven
- [ ] 200-day ending journey exists
- [ ] three-policy ending comparison exists
- [ ] persistence journey exists
- [ ] semantic panel-value snapshot model exists
- [ ] post-load panel authority identity asserted
- [ ] keyboard-only live route journey exists
- [ ] scope uses live routes
- [ ] snapshot coverage tracks live routes
- [ ] golden changes require approval note
- [ ] runtime liveness identity assertion exists
- [ ] representative mutating panel actions proven
- [ ] all journeys seed-pinned
- [ ] culture-invariant assertions used
- [ ] no wall-clock sleeps/unseeded RNG
- [ ] flake policy enforced
- [ ] CI tiering documented
- [ ] journey runtime budget measured
- [ ] journey docs complete
- [ ] explicit journey verbs added
- [ ] exported-build smoke runs after journey suite
- [ ] runtime evidence gate exists
- [ ] collector is observational/idempotent
- [ ] normalized runtime artifact reproducible
- [ ] metrics recorded

---

# 20. Ship / No-Ship Gate

**SHIP** only if:

```text
production_selftests_using_shipped_authority == true
AND implicit_production_fixture_fallbacks == 0
AND fresh_campaign_systems_in_real_selftests == 0
AND golden_save_fixtures >= 3
AND determinism_digests_cover_30_180_360_day == true
AND coverage_artifact_exists == true
AND protected_coverage_slices_not_regressed == true
AND capture_restore_roundtrip_coverage == 100_percent
AND campaign_day_owner_determinism_coverage == 100_percent
AND high_risk_presence_only_tests == 0
AND runtime_stage_selected_observed == true
AND runtime_effect_produced_observed == true
AND first_hour_journey == pass
AND ending_journey == pass
AND persistence_journey == pass
AND input_journey == pass
AND runtime_panel_authority_identity == pass
AND unexplained_journey_flakes == 0
AND export_smokes == pass
AND verify_fast == pass
```

Otherwise: **NO SHIP**.

---

# 21. Implementer Handoff

1. Do 27A before measuring coverage.
2. Never certify shipped behavior with a silent demo fixture.
3. Make synthetic fixtures explicit and keep them for true unit-edge cases.
4. Use one authority-backed campaign fixture rather than hand-rolling partial game worlds.
5. Reuse real catalog loaders and campaign owners.
6. Protect save round-trips and deterministic day owners with source-derived gates.
7. Replace weak high-risk assertions with cross-system state assertions.
8. Keep mutation testing bounded and purposeful.
9. Instrument runtime stage transitions at the actual loader/registry/query/selection/effect seams.
10. Make journeys assert the next system's state, not just the current action.
11. Use live route count, not registry count, for player UI journeys/snapshots.
12. Assert reference identity for panel authorities.
13. Pin seeds and culture.
14. Treat flakes as defects.
15. Keep coverage/journeys Tier 2 and long replay nightly so local verification stays fast.
16. Close with exported-build smoke, runtime evidence, and all CI gates green.

---

# 22. Final Outcome

When this plan is complete, ASHFALL's test suite stops proving that a demo-shaped collection of classes can execute and starts proving that the shipped campaign stays connected.

Selftests use the same data authority as the player. Synthetic fixtures remain possible, but they are explicit and unmistakable. Golden saves exercise real persisted shapes. Determinism fixtures make long simulation changes reviewable.

Coverage becomes a risk-control instrument instead of a vanity number: stateful systems must round-trip, campaign-day owners must replay deterministically, and continuity-critical slices cannot silently lose protection. High-risk tests assert state and consequence, not presence.

Runtime content evidence is observed during real boot. A catalog entry only earns `SELECTED` when it was chosen, and only earns `EFFECT_PRODUCED` when it changed the game.

Finally, real journeys make the suite fail at the exact seams that previously broke: rationing into needs, weather into dose, equipment into exposure, medicine into treatment, illness into duty, death into memorial, saved state into restored panels, and campaign history into the ending.

The project does not need more tests for the sake of count. It needs tests that mean the same thing the game means.
