# Plan 27 — Tests That Mean It: Fidelity, Coverage, and Runtime Evidence

> **Wave:** Continuity Wave 3 — *Ship It Intact*
> **Depends on:** 26A/26B (a boot of the artifact is the highest-fidelity test there is); pairs
> with 15C (liveness gate) from Wave 1.
>
> **Theme:** the test suite is genuinely large — 323 test files, **5,303 passing tests**, and
> every one of the 128 Core `*System.cs` classes is named in at least one test. It shipped anyway
> with a hardcoded ending, an inert consume call, 30 fake consoles, and gear that never wears out.
> The suite is not thin; it is **unfaithful**. It tests a demo version of the game, measures
> nothing about how much it tests, and asserts presence rather than behaviour.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | Volume is not the problem | `dotnet test` → **5303 passed, 0 failed**; `ls Ashfall.Core.Tests/*.cs` → **323** files; all 128 `Assets/Ashfall.Core/**/*System.cs` names appear in ≥1 test file |
| 2 | **…and the game still shipped dead ends** | hardcoded epilogue inputs (`src/Main.GameFlow.cs:444`), null consume callbacks (`src/Host/InventoryHostSession.cs:303`), `DegradeRate = 0f` (`Inventory.cs:951`), `TryResolveMoralChoice` with 0 callers, 30 consoles with `IsBound = true` |
| 3 | **Selftests exercise a different item set than the game** | live path: `src/Main.Inventory.cs:38` → `InventoryHostSession.Create(_dataDir)` → `ItemCatalogLoader.LoadCatalog` (items.json). Test path: `new InventoryHostSession()` → `SeedCatalog(Catalog)` (`src/Host/InventoryHostSession.cs:30`) — the hardcoded demo defs. Sites: `src/Host/InventorySaveSelfTest.cs:12,21`, `src/Host/PanelBindLifecycleSelfTest.cs:211`, `src/Host/HostCli.PanelTests.cs:627,663,673,2077`, `src/Host/HostCli.Onboarding.cs:53`, `src/Main.UiTests.Inventory.cs:90` |
| 4 | **No coverage measurement exists** | `grep -rn "coverlet\|CollectCoverage" Ashfall.Core.Tests/*.csproj Directory.Packages.props` → **0 hits**; no coverage gate among the 46 CI gates, despite the `ashfall-coverage-gate` skill's stated purpose (save round-trip + determinism coverage for H10/H11) |
| 5 | Content evidence is static, not observed | `artifacts/content-utilization.json` — `bestEvidence`: **STATIC 402 / RUNTIME 9**; stages: `DISCOVERED 271 · LOADED 3 · DESERIALIZED 0 · REGISTERED 0 · QUERIED 133 · SELECTED 0 · EFFECT_PRODUCED 4` |
| 6 | Gates assert presence | `PanelRouteGateTests` (routes registered), `PlayerSurfaceCoverageGateTests` (setup metadata treated as binding coverage) — per `docs/debug/10LOOP_player_ui_ux_BUG_AUDIT.md` §14; Wave 1's 15C is the corrective |
| 7 | UI visual coverage is 30 / 135 | `snapshots/*.png` → **30** golden images against 135 registered panel routes (`PanelRegistryBootstrap.cs`), and 30 of those routes are the unbacked consoles |
| 8 | The good pattern already exists in-repo | `SaveStoreChecksumSweepTests` (3 per store), `BareSaveStoreSealTests`, `SaveStoreCoverageGateTests` (**source-scans** `src/**/*SaveStore*.cs` and fails on non-delegation), `DataRuleComplianceTests` (policy over the authority). These are the templates to copy, not new inventions |
| 9 | Stale test claims mislead agents | `AGENTS.md` **H11** says `JournalSystem` behaviour is untested — `Ashfall.Core.Tests/JournalSystemTests.cs` **and** `JournalSystemCoreBehaviorTests.cs` exist. **H5** (Utility AI forked) is likewise resolved (`src/UtilityAI/` = one panel). Reconciled in 24C step 13; 27C adds the machine check that keeps them honest |

**Reading:** the fix is not "more tests". It is (a) make tests run against the same authority the
player runs against, (b) measure what's covered, (c) assert behaviour and liveness instead of
presence, and (d) let a real boot produce runtime evidence instead of grep evidence.

---

## Task 27A — Fidelity: the suite must run against the shipped data authority

**Goal:** one rule — no test or selftest may validate a fixture version of the game the player
cannot possibly run.

**Files:** `src/Host/InventoryHostSession.cs` (`Create` vs ctor seeding), the six test-path sites
listed above, `Ashfall.Core.Tests/Fixture/*` (new), `assets/Ashfall.Core/…CatalogLoader` (read),
`docs/testing/FIXTURE_POLICY.md` (new), `Ashfall.Core.Tests/DataAuthorityFidelityTests.cs` (new).

### Substeps

1. **Inventory the seams**: list every host/Core construction that silently substitutes a
   hardcoded fallback (seed catalog, `SeedStartingSupplies`, demo survivor ids like
   `"surv_01"`/`"sv_cohort_demo"`, `PowerGridHostSession.CreateDefault`,
   `ShelterScheduleHostSession`'s literal 800 W grid at `:21–23`). The list *is* the task.
2. **Make the fallback explicit**: convert implicit seeding into an opt-in
   `InventoryHostSession.CreateForFixture()` / `SeedCatalogForTest()` with a name that says it is a
   fixture, and make `Create(dataDir)` the only path used by the game **and** by default in
   selftests.
3. **Point CI selftests at the real authority**: host selftests (`--*-selftest`) should load
   `CatalogPath.ResolveDataDir()` — they are meant to certify the shipped configuration, so they
   must not run against demo defs.
4. **Add a fidelity assertion**: a test proving the fixture catalog equals the JSON authority for a
   sample of behaviourally significant fields (`hungerRestore`, `thirstRestore`, `radProtection`,
   `durability`, `contamination`) — the fields Wave 2's plans 21/22 depend on. When they diverge,
   the test names the item ids.
5. **Promote one shared fixture builder** (`Ashfall.Core.Tests/Fixture/CampaignFixture.cs`) that
   constructs a campaign-shaped world from the authority with an explicit seed, so new tests stop
   hand-rolling partial systems — the drift that let the null-callback consume path survive 5,303
   tests.
6. **Ban "fresh system" in tests of the game**: a source-scan gate (pattern:
   `SaveStoreCoverageGateTests`) failing any `src/Host/*SelfTest*.cs` or `src/Main.UiTests*.cs` file
   that constructs a system the campaign already owns — closes the same defect class as 16B and
   24B step 10 from the test side.
7. **Golden save fixtures**: commit 2–3 checksummed save files (early, mid, late campaign) as test
   inputs so load behaviour is tested against real shapes, not round-trips of freshly built state —
   and reuse them for 26B's exported-build load smoke.
8. **Determinism fixtures**: pin a seed → expected digest for a 30/180/360-day run, generated from
   the existing harness, so a behavioural change is a *reviewed* digest diff instead of a surprise.
9. **De-duplicate the two seeds**: the item defs exist in `items.json`,
   `InventoryHostSession.cs:97–176`, and `CraftingHostSession.cs:144`; consolidate so one authored
   fact has one home (mirrors Wave 2's single-authority principle).
10. **Document the policy** in `docs/testing/FIXTURE_POLICY.md` with the two legal fixture kinds
    (authority-backed, or explicitly-named synthetic) and a worked example of each.
11. **Tests for the tests**: assert the new fidelity gate catches an intentionally divergent
    fixture (a gate that has never failed is a rumour).
12. **Run the checklist** + `verify-fast.sh`.

**DoD:** every selftest and unit test reads the same 413-file authority the game reads, or says in
its name that it doesn't.

---

## Task 27B — Measure it: coverage + assertion quality for the classes that matter

**Goal:** a number that exists, is tracked, and protects the two properties the project's
invariants are actually about: **save fidelity** and **determinism**.

**Files:** `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`, `Directory.Packages.props`,
new `scripts/ci/coverage-gate.sh`, new `docs/testing/COVERAGE.md`,
`docs/ci/CI_GATE_MANIFEST.json`, `artifacts/coverage-baseline.json` (new).

### Substeps

1. **Add coverage collection with the smallest possible footprint**: `coverlet.collector` in
   `Directory.Packages.props` (central package management is already in use) producing Cobertura
   XML under `artifacts/` — a `.gdignore`d, gitignored output.
2. **Baseline before gating**: record current line/branch coverage for `Ashfall.Core` and for the
   subset the invariants care about; publish the number in `docs/testing/COVERAGE.md` so the
   baseline is reviewable rather than negotiated.
3. **Gate the right slices, not the whole cake**: enforce (a) **no decrease** on
   `Assets/Ashfall.Core/Save/**`, `Survivors/**`, `Radiation/**`, `Medical/**`, `Economy/**`,
   `Campaign/**` and (b) 100 % of every `CaptureState`/`RestoreState` pair being exercised by a
   round-trip test. A global percentage target would be met by testing the easiest code.
4. **Add the round-trip generator**: for every stateful system, the same 3-test template the
   existing store sweeps use — clean round-trip, mutated state changes the checksum, null checksum
   rejected. That contract already exists for 12 stores (`SaveStoreChecksumSweepTests`) and
   3 bare stores (`BareSaveStoreSealTests`); extend the sweep to every
   `CaptureState()` declaration in Core via the source-scan approach.
5. **Determinism coverage**: paired-seed replay per day-advance owner (the 19
   `_campaignDay.Register(...)` ids) so an owner can't be added without a determinism test — the
   missing half of Invariant 4 as enforced practice.
6. **Assertion-strength review on the hot path**: for the ten behaviours Wave 1/2 exposed, replace
   "does not throw" with "state changed as specified". Grep for tests asserting only
   `Assert.NotNull` / `Assert.True(true)` / no-exception, and rank them by the risk of the code
   they cover.
7. **Mutation spot-check** (bounded, not a campaign): run a mutation tool on a handful of pure Core
   formulas (`RadiationSystem.ComputeExposurePerHour`, `NeedsSystem` decay,
   `EquipmentConditionSystem` wear, `KitchenNutritionSystem.GetSpoilageDays`) and record whether
   the suite actually catches inverted bounds. This measures assertion quality objectively — and it
   is the one step that would have caught `DegradeRate = 0f`-class bugs.
8. **Register the gate** in `docs/ci/CI_GATE_MANIFEST.json` with `expected_summary`, tier-2, and a
   `--gate coverage` local invocation path.
9. **Keep CI fast**: coverage in the Tier-2 job, not in `verify-fast.sh`'s critical path; document
   the split in `docs/CI.md`.
10. **Report in the PR body**: `scripts/ci/github-step-summary.py` already exists — add the coverage
    delta line so reviewers see it without opening artifacts.
11. **Ratchet monthly** with a recorded decision, never silently — each bump names the code it now
    protects.
12. **Tests**: assert the gate fails on an intentionally dropped round-trip test (same "prove the
    gate can fail" discipline as 26B step 5 and 27A step 11).

**DoD:** a coverage number exists, is published, is gated where it matters, and a dropped
round-trip test fails CI.

---

## Task 27C — Runtime evidence and journeys: from grep to observed behaviour

**Goal:** replace static inference with observed runtime facts, and add end-to-end journeys that
fail when a *connection* breaks — the failure mode every finding in Waves 1–2 shared.

**Files:** `src/Host/ContentUtilizationRuntimeCollector.cs`, `Assets/Ashfall.Core/Content/*`,
`artifacts/content-utilization-baseline.json`, `src/Main.UiTests.RealCampaignJourney.cs`,
`src/Host/HostCli.SelfTests.cs`, new `src/Host/JourneySelfTests.cs`,
`docs/testing/JOURNEYS.md` (new), `scripts/ci/generate-ui-panel-catalog.py`,
`docs/ui/SNAPSHOT_COVERAGE.md`.

### Substeps

1. **Collect during a real boot**, not a synthetic harness: drive
   `ContentUtilizationRuntimeCollector` from the same code path `--day1-selftest` /
   `--real-campaign-journey-selftest` uses, so `RUNTIME` evidence climbs from 9 catalogs toward the
   number of catalogs a normal campaign actually touches.
2. **Fill the empty stage columns**: today `DESERIALIZED 0 / REGISTERED 0 / SELECTED 0` while
   `QUERIED 133` — instrument the loader/registry/query seams so the pipeline stages are real
   rather than terminological, and gate `SELECTED > baseline`.
3. **Add `EFFECT_PRODUCED` as the headline metric** (it is currently 4 of 411) and make the
   baseline file monotonic; every Wave 1/2 plan should move it.
4. **Journey: the first hour.** new game → read guidance (17B) → ration cut → craft → dispatch →
   storm → dose rises → mask fails (21A) → treat (22C) → someone dies → shift vacated (24A) →
   memorial → day 30 briefing legible. Assert each link changes the *next* system's state — that is
   the test that would have caught gaps 1–10 of both earlier waves simultaneously.
5. **Journey: the ending.** scripted 200-day campaign with a fixed choice policy → game over →
   assert epilogue derives from state (Wave 1's 19A) and differs across three policies.
6. **Journey: persistence.** play → save → quit → load → compare a snapshot of every panel's bound
   values and the day-event history; assert equality (covers 16B/16C and Wave 3's fixture work).
7. **Journey: input.** every player-routable panel reachable by keyboard only, with focus order
   intact — the accessibility claim becomes a test instead of an intention.
8. **Expand snapshot coverage** from 30 toward the *live* route count defined by 16A (not 135):
   each newly-live console gets a golden image with a fixture-populated state, per
   `docs/ui/SNAPSHOT_FIXTURE_POLICY.md`, and the manifest consistency audit stays green.
9. **Panel liveness at runtime** (15C's static gate, runtime twin): assert each routed panel's
   `Bind` received a reference identical to the campaign's — turning "twenty have no `Bind`
   method" into a permanent impossibility.
10. **Flake control**: journeys must be seed-pinned and culture-invariant; a journey that fails
    only in CI gets fixed or removed, never marked flaky and kept — an unreliable gate is worse
    than none because it trains people to ignore it.
11. **Timebox CI**: journeys in Tier 2 + nightly soak; the fast tier keeps its runtime contract so
    `verify-fast.sh` stays usable locally (it mirrors all fast gates today).
12. **Docs**: `docs/testing/JOURNEYS.md` lists each journey, its invariants, and which continuity
    gap it retires — so a future agent can see what a test is *for*.
13. **Run the checklist**, both export smokes (26B), and `verify-fast.sh`.

**DoD:** content utilization is proven by observation, and five named journeys fail when any
system stops talking to the next.

---

## Cross-Task Dependencies

```
26A/26B ──► 27A (selftests point at the resolved authority) ──► 27B (coverage of the real thing)
                                   │                                    │
                                   └──► 27C steps 1–3 (runtime evidence)┘
15C liveness gate (Wave 1) ◄────────► 27C step 9 (runtime twin)
Wave 2 plans 20–24 ─────────────────► 27C step 4 (the first-hour journey is their acceptance test)
```

**Execution order:** 27A → 27B → 27C. 27A must precede 27B: coverage measured against demo
fixtures is a number that describes the wrong game.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --content-utilization-selftest   # RUNTIME + EFFECT_PRODUCED up
7. bash scripts/ci/coverage-gate.sh                              # (27B)
8. bash scripts/ci/verify-fast.sh
9. journeys: --day1-selftest --real-campaign-journey-selftest --<journey verbs>
```

---

## Estimated Effort & Risk

| Task | Test files | New gates | Net new tests | Difficulty | Regression risk |
|---|---|---|---|---|---|
| 27A | ~8 sites + 2 new | 2 (fidelity + no-fresh-system) | 6–10 | Medium | LOW–MED (fixtures fail loudly when corrected) |
| 27B | tooling + ~30 round-trips | 1 (coverage) | 20–40 (generated) | Medium | LOW |
| 27C | 4–5 journeys | 1 (runtime evidence) | 15–25 | **High** | MEDIUM (flakes — see step 10) |

**Guardrails:** no global coverage percentage target; no test asserting only "does not throw"; no
journey kept after two unexplained flakes; no snapshot regenerated without an approval note; and no
fixture that silently differs from `Assets/StreamingAssets/Data` — which is the entire lesson of a
5,303-test suite that shipped an unreachable ending.
