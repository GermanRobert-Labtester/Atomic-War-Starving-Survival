# Plan 28 — The Orchestration Spine: Registration You Cannot Forget

> **Wave:** Continuity Wave 3 — *Ship It Intact*
> **Depends on:** Wave 1's 15C (liveness gate) and Wave 2's day-event vocabulary; do 28A before
> touching any large file split.
>
> **Theme:** the host is the composition root for ~128 Core systems, 19 day-advance owners, 67
> save sections, 110 panel routes, and 62 save stores — and each of those is registered by a
> *different mechanism in a different file*, kept in sync by human memory. That is the structural
> cause behind almost every gap in Waves 1–2: a Setup without a Save, a system that ticks but never
> emits, a panel routed but never bound, a store that persists nothing. This plan replaces memory
> with declaration.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The orchestration layer is already partialized — the docs don't know | `ls src/Main*.cs` → **56 files, 14,361 lines**; `src/Main.cs` itself is **80 lines**. `AGENTS.md` **H7** still describes "`Main.cs` (Godot) — one `partial class Main` in a single ~6.5k-line file" and the canon registry §26.4 claims "7,014-line file" |
| 2 | Triad counts disagree across three sources of truth | `grep -c "private void Setup"` over `src/Main*.cs` → **72**; `"private void Save[A-Z]"` → **69**; `"private void Flush"` → **26**. `AGENTS.md` says 31 Setup / 24 Save + SaveAll / 17 Flush; the canon registry says 38 / 30 / 18. All three are wrong or stale |
| 3 | The triad gate is narrower than its name suggests | `scripts/ci/triad-drift-gate.sh` — "validates that every save section defined in `SaveSectionRegistry.cs` has matching `SaveXxx` and `SetupXxx` methods in `src/Main.*.cs`, and that no unregistered Save methods exist". It does **not** cover Flush parity, event-subscription parity, or day-owner participation |
| 4 | Registration is split across four mechanisms in four files | Save sections: `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs` (declarative, 67 entries + `SectionFileNames` + `SchemaVersions` + lifecycle groups). Day owners: string literals `grep -c '_campaignDay.Register("'` → **19** in `src/Main.CampaignOwners.cs`. Panels: **110** `PanelRegistry.ConfigureActions(...)` calls in `src/Main.PlayerSurfaces.cs` + `PanelRegistryBootstrap.cs` descriptors. Stores: 62 `*SaveStore*.cs` façades over `SaveStoreHub` |
| 5 | The declarative pattern already works | `SaveSectionRegistry` is the single authority for names/versions/aliases (Initiative #42) and the envelope builder consumes it — proving one table + generated consumers is viable here |
| 6 | Drift already caused shipped bugs | Wave 1: fire/matrix/skill panels bound to fresh systems; onboarding panel constructed but never routed. Wave 2: `ServeMeal`, `SetHunterSkill`, `SetCellar/SetRefrigeration`, `OnInventoryConsumeClicked` all **constructed-and-never-called** — the identical failure shape as a Setup without a Save |
| 7 | Host ownership is documented by hand | `docs/scenes.ownership.manifest.json`, `docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`, `scripts/ci/generate-architecture-map.sh --check` — generated artifacts exist, so a generated ownership table fits the house style |
| 8 | Largest hot files | `src/Host/HostCli.PanelTests.cs` **3,253**, `src/Host/AssetRegistry.cs` 1,214, `src/Main.UiPanels.cs` 1,131, `src/Host/SaveLoadHostSession.cs` 1,052, `src/Host/Phase0HostSession.cs` 1,009 |
| 9 | The composition root does ordering implicitly | `src/Main.Application.cs` calls `PanelRegistryBootstrap.RegisterAll()` (`:39`) and `_dataDir = CatalogPath.ResolveDataDir()` (`:553`) among ~30 setups; lazy `SetupXxx()` guards are sprinkled inside bind lambdas (`Main.PlayerSurfaces.cs`) so the required order is invisible and enforced only by "did someone remember to call Setup first" |

**Reading:** don't split big files for navigability — that's cosmetic. Split them only as a
side-effect of making registration **declarative**, so the compiler and a gate know what a
subsystem needs to be alive: setup, save, flush, day-owner, panel route, events, cues.

---

## Task 28A — One subsystem manifest: declare a subsystem once, generate the rest

**Goal:** a single declarative table per subsystem (id, owner class, setup, save section, day-owner
+ phase, panel routes, event sources, required bindings), consumed by the host and by gates.

**Files:** new `Assets/Ashfall.Core/Composition/SubsystemManifest.cs`,
new `src/Host/SubsystemRegistration.cs`, `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`
(reference, don't duplicate), `src/Main.CampaignOwners.cs`, `src/Main.PlayerSurfaces.cs`,
`docs/architecture/TRIAD_GATE_AND_SAVE_OWNERSHIP.md`, new
`scripts/ci/generate-subsystem-manifest.py`, `Ashfall.Core.Tests/SubsystemManifestTests.cs`.

### Substeps

1. **Design the record first, on paper**: `SubsystemDescriptor { id, lifecycleGroup, saveSection?,
   dayOwner? { phase }, panelRoutes[], setupMethod, saveMethod, flushMethod?, eventSources[],
   requiredAuthorities[] }` — every field that today lives in someone's memory somewhere.
2. **Reuse `SaveSectionRegistry` as the save half** rather than re-listing sections; the manifest
   *references* section keys so there is still exactly one authority for names and schema versions
   (the same non-duplication rule `SectionFileNames` established in Initiative #42).
3. **Adopt the existing owner-id vocabulary** for the 19 day-advance registrations
   (`survivors_needs`, `power_grid`, `weather_world`, …) instead of minting new ids — those strings
   already appear in `DayStateChangeEvent.SourceOwnerId` and in the briefing; make them constants.
4. **Panel routes reference `PanelRegistry` ids**, and the manifest is where `PanelGroup`/maturity
   (16A) and liveness (15C) meet, so one lookup answers "is this subsystem surfaced to the player,
   and does the surface act on it?"
5. **Generate, never hand-maintain**: `scripts/ci/generate-subsystem-manifest.py` emits
   `docs/architecture/SUBSYSTEM_MANIFEST.md` + JSON, in the exact style of
   `generate-save-store-matrix.sh --check` / `generate-ui-panel-catalog.py` /
   `generate-core-systems-catalog.py` (all of which already run as gates with `--check`).
6. **Register the `--check` gate** in `docs/ci/CI_GATE_MANIFEST.json`; a subsystem added without a
   manifest row fails CI, which is the whole point.
7. **Make the host consume it**: `SubsystemRegistration` drives setup ordering, save capture, and
   dirty-flush from the manifest, replacing the per-domain hand-written call lists. Keep the public
   `SetupXxx`/`SaveXxx` methods as thin, still-callable wrappers so nothing else has to change at
   once.
8. **Order deterministically and document the rule** (lifecycle group → phase → ordinal id). The
   current implicit ordering plus in-lambda `SetupXxx()` guards cannot be reasoned about, and lazy
   setup inside a bind lambda is how a panel can bind a half-initialised session.
9. **Encode "flush is optional but declared"**: 72 setups, 69 saves, **26 flushes** — make each
   subsystem state whether deferred flush applies and why, then have the gate check the declared
   set rather than pretending every subsystem needs one.
10. **Declare required authorities per panel route** so 27C's runtime identity check
    (`ReferenceEquals(panel.Authority, campaign.Authority)`) has something to check against, and
    16B's "fresh system at bind time" becomes a manifest violation rather than a grep pattern.
11. **Wire the events half**: declare each subsystem's Core event sources so `AudioEventBridge`
    (9 domains subscribed today of ~15 emitters), the briefing feed (1 producer of 19 owners), and
    journal writers derive their subscriptions from the manifest instead of hand-maintained lists —
    the mechanical fix for Wave 2's "emits nothing" gaps.
12. **Migrate three subsystems end to end first** (pick one small, one medium, one messy — e.g.
    `vinyl_morale`, `power_grid`, `phase0_psychology`) before batch conversion, and compare
    behaviour byte-for-byte on the golden saves (27A step 7).
13. **Tests**: manifest completeness (every `_campaignDay.Register`, `SaveStore`, panel route, and
    `Setup*` appears exactly once), no orphan sections, no undeclared authority dependency, and an
    intentional-omission test proving the gate fails.
14. **Run the checklist** + triad gate + `verify-fast.sh`.

**DoD:** adding a subsystem without registering every part of it is a CI failure, not a bug report.

---

## Task 28B — Behaviour-preserving decomposition of the remaining mega-files

**Goal:** split by ownership once the manifest defines ownership — so each resulting file is one
subsystem's triad, reviewable and testable, with zero behaviour change.

**Files:** `src/Host/HostCli.PanelTests.cs` (3,253 lines), `src/Main.UiPanels.cs` (1,131),
`src/Host/SaveLoadHostSession.cs` (1,052), `src/Host/Phase0HostSession.cs` (1,009),
`src/Host/AssetRegistry.cs` (1,214), the 56 `src/Main*.cs` partials,
`Ashfall.csproj` (compile glob), `.csproj`/`.bitmap`-free (no build config surprises),
`scripts/ci/generate-architecture-map.sh`.

### Substeps

1. **Freeze the baseline first**: pin the golden-save digests, the 30 snapshots, the full test run,
   and `--runtime-scale-selftest` output, so "behaviour-preserving" is measured rather than argued.
2. **Order by risk, not size**: start with `HostCli.PanelTests.cs` (test-only surface, 3,253 lines,
   already partially split by the missing `.Campaign/.Diagnostics/.Expansion/.Persistence/.UI`
   partials whose `.cs.uid` sidecars are dangling — see Wave 1's 19C sweep: those five files were
   *intended* and never landed).
3. **One file per verb group, named after the subsystem** — not per feature branch of an `if`. The
   intended split already exists in the sidecar names; finish the job it records.
4. **Then `Main.UiPanels.cs`** by domain group (survivors, expeditions, medical, economy, narrative,
   holdfast, maritime, verdict…) — matching the existing `Main.<Domain>.cs` convention so the file
   set stays predictable and the `ashfall-godot-patterns` skill's rules keep applying.
5. **Then the host sessions** (`Phase0HostSession`, `SaveLoadHostSession`) only where a real
   ownership seam exists; a 1,000-line file with one responsibility is fine and must not be split
   for the sake of a metric.
6. **Mechanical moves only**: no renames, no signature changes, no logic edits in a decomposition
   commit. If something looks worth changing, file it as its own task — a split with an embedded
   fix is unreviewable and un-revertable.
7. **Verify per move**: build, full test run, the affected selftests, and a `git log --follow`-able
   diff. Commit per file, so a single bad move is one revert.
8. **Regenerate the architecture map** (`scripts/ci/generate-architecture-map.sh --check`) and the
   code index so the docs stop describing files that no longer exist.
9. **Guard against partial drift**: ensure `Ashfall.csproj` picks up new partials via its existing
   glob; confirm no file is silently excluded (a missing `.cs` with a present `.uid` was already the
   symptom of exactly that class of accident — 19C found 15).
10. **Add a size advisory** (not a hard gate) for `src/**` with a per-file budget, exempting
    generated catalogs, so growth is visible without becoming a formatting war.
11. **Re-run the 46-gate suite** plus the two export smokes at the end; then re-measure day-advance
    cost — decomposition should be perf-neutral, and if it isn't, that is a finding worth its own
    task.
12. **Update `AGENTS.md` H7** to describe what the code actually is now: 56 partials,
    N triads, manifest-driven registration — the entry that currently claims "~6.5k-line file" is
    the reason agents still treat the composition root as one monolith.
13. **Run the checklist.**

**DoD:** files are organised by ownership, every commit in the task is provably behaviour-neutral,
and the docs match.

---

## Task 28C — Lifecycle contracts: construction, wiring, init, tick, save, teardown

**Goal:** make the six lifecycle stages explicit per subsystem so half-initialised and
never-torn-down states stop being reachable — the residual risk behind Wave 1's 16B (session
replacement) and 16C (subscription identity).

**Files:** new `Assets/Ashfall.Core/Composition/ILifecycle.cs`,
`src/Host/SubsystemRegistration.cs`, `src/Main.Lifecycle.cs`, `src/Main.PanelLifecycle.cs`,
`src/UI/*Panel.cs` (bind/unbind), `Ashfall.Core.Tests/LifecycleContractTests.cs`,
`src/Host/PanelBindLifecycleSelfTest.cs`.

### Substeps

1. **Name the stages in code** — `Construct → Wire → Initialise → TickOwner? → Persist → Teardown`
   — as an interface Core can hold (engine-free), with each stage idempotent or explicitly
   documented as single-shot.
2. **Enumerate today's reality** per subsystem: which stages exist, which are folded into a lazy
   `SetupXxx()` guard inside a bind lambda, and which have no teardown at all. Publish the table;
   it will be the argument for the rest of the task.
3. **Separate lazy setup from lazy binding**: a bind lambda should *assert* its authority exists
   (26A/27A style loud failure), not silently construct a parallel one — that single change removes
   the "fresh system instance" class permanently.
4. **Make teardown mandatory and symmetrical**: every subsystem that subscribes must declare its
   unsubscribing counterpart in the manifest; the panel-side twin is 16C's subscription bag.
5. **Session-replacement protocol**: `src/Main.Lifecycle.cs` nulls and rebuilds sessions on
   new-game/load (verified for onboarding at `:290–294`); define one ordered rebind pass over the
   manifest so *every* open surface re-resolves, not just the ones someone remembered.
6. **Order the day owner's phases as data**: the coordinator takes `phase` per owner
   (`CampaignDayCoordinator.Register(ownerId, owner, phase = 3)`); document the intended ordering
   and add a gate that no owner registers at the default phase by accident.
7. **Fail-fast on missing stages**: an incomplete subsystem should be unable to register (throw in
   dev, log-and-refuse in release), instead of shipping a silently non-ticking system — the
   structural version of "compiles but does nothing".
8. **Instrument the contract**: `--panel-bind-lifecycle-selftest` and
   `--save-load-ui-failure-selftest` already exist as gates; extend both to walk the manifest so
   every subsystem is probed, not just those with hand-written cases.
9. **Determinism of lifecycle**: construction order influences RNG fork order
   (`_campaignDay.Rng.Fork(stream, day, index)` — e.g. `Main.ShelterBatch3.cs:106`); the manifest
   ordering must therefore be stable across runs, and a digest test proves it.
10. **Perf**: confirm registration/rebind cost is negligible against the 26C budget; rebind passes
    are where accidental per-frame work hides.
11. **Tests**: contract tests per stage, a teardown-completeness scan (every `+=` has a matching
    `-=` on the same delegate — 16C's rule, now enforced for subsystems too), and a
    new-game→load→new-game soak asserting no stale references survive.
12. **Docs**: `docs/architecture/LIFECYCLE.md` with the stage diagram and the manifest fields that
    drive each; link from `docs/CURRENT_AUTHORITY.md`.
13. **Run the checklist** + `verify-fast.sh` + the exported-build boot smoke (26B).

**DoD:** a subsystem cannot be half-alive, and re-entering a session rebinds everything or fails
loudly.

---

## Cross-Task Dependencies

```
28A (manifest = single declaration) ──► 28B (split along declared ownership) ──► 28C (lifecycle stages)
        │                                   ▲                                      │
        ├──► 15C liveness gate (Wave 1) consumes panel fields                       ├──► 27C step 9 runtime checks
        ├──► 17A/24B: event-source declarations fix "emits nothing"                 └──► 26C diagnostics per subsystem
        └──► 16B/16C become manifest violations, not greps
```

**Execution order:** 28A → 28B → 28C. 28B without 28A is re-shuffling the same implicit knowledge
into new files; 28C without 28A has nothing to declare the stages against.

**Do not** start 28B while Wave 1's 16B or Wave 2's 22A are mid-flight in the same files — the
mechanical-split rule (28B step 6) requires a quiet tree.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj      # 5303+ passing, 0 failed
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-subsystem-manifest.py --check     # (28A)
7. bash scripts/ci/triad-drift-gate.sh                           # parity, unchanged semantics
8. bash scripts/ci/generate-architecture-map.sh --check
9. godot --headless --path . -- --runtime-scale-selftest         # perf-neutral proof
10. golden-save digests + 30 snapshots unchanged                 # behaviour-preserving proof
11. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | New files | Files moved/split | Gates | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 28A | 2 Core + 1 host + 1 script | 4 registration sites | 1 | 10–14 | Medium–High | MEDIUM (ordering change → digest check) |
| 28B | 0 | 5 large files + partials | 1 advisory | 0 new | Medium | **LOW if mechanical**, HIGH if mixed with fixes |
| 28C | 1 interface | all subsystems declared | 1 | 12–18 | High | MEDIUM (lifecycle edits can leak or double-subscribe) |

**Guardrails:** no behaviour change in 28B commits; no new orchestration framework or DI container —
one table, one registration file, one script, matching the patterns `SaveSectionRegistry` and the
`generate-*.py --check` family already use; no split that leaves two files owning one subsystem.
