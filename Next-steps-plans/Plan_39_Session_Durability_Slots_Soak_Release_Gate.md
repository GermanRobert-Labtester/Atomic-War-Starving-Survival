# Plan 39 — Session Durability: Saves, Slots, Soak, and the Release Gate

> **Wave:** Continuity Wave 5 — *The Human Interface* (closing plan)
> **Depends on:** everything before it — a soak test is only meaningful once the loops the earlier
> waves fixed actually run.
>
> **Theme:** ASHFALL's save architecture is genuinely strong: a generic checksummed `SaveStore<T>`
> service with atomic temp+rename writes and `.bak` rotation (Initiative #41), one versioned
> campaign envelope per slot (#42), a 62-store contract matrix regenerated and checked, a
> seven-gate save/load UI failure selftest — and a **seven-day deterministic smoke test that ships
> as a CLI verb but is not a gate**. Meanwhile the things that break players are the ones nothing
> measures: sessions 200 hours long, quit-mid-panel, disk-full, corrupted-by-hand, two saves of the
> same slot, and the day-advance that takes 0.6–1.3 s per click and grows.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| # | Fact | Evidence |
|---|---|---|
| 1 | The store layer is sound | `Assets/Ashfall.Core/Save/SaveStore.cs` via `src/Host/SaveStoreHub.cs` (injected `FileSystemIO`/`SystemTextJsonSerializer`/`GodotLog` + `SaveSlotRoot` router), atomic writes, optional `.bak` (e.g. `src/Host/HoldfastTradeSaveStore.cs:38–60`), per-store header comments documenting envelope/checksum/slot routing (`EncounterChoiceSaveStore.cs:9`) |
| 2 | Envelope + slots | `src/Host/SaveLoadHostSession.cs` (1,052 lines) owns `SaveSlotService`, `SaveProfileId`, `SaveSlotId?`, `AggregateSaveEnvelope`; `SaveSlotRoot.ResolveBaseDirectory` routes every store (`SaveStoreHub.cs:17,51,76`) |
| 3 | Coverage is gated | 62 stores in `docs/saves/SAVE_STORE_CONTRACT_MATRIX.md` (generated, `generate-save-store-matrix.sh --check` is a manifest gate); `SaveStoreCoverageGateTests` fails a store with neither envelope nor codec delegation |
| 4 | UI failure path is gated | manifest gate `save_load_failure` → `--save-load-ui-failure-selftest` (the seven-gate suite from #42) |
| 5 | **A real soak/release probe exists but is ungated** | `src/Host/HostCli.cs:122,375` exposes `SevenDayDeterministicSmokeSelfTest`, run by `HostCli.SelfTests.cs:752–756` (`7day_smoke_selftest`) — it is **absent from `docs/ci/CI_GATE_MANIFEST.json`** (the manifest's 46 gates include `playable_shell`, `day1_onboarding`, `save_load_failure`… and not this) |
| 6 | Quit handling exists, untested for durability | `src/Main.Application.cs:531–533` handles `NotificationWMCloseRequest`; no gate asserts a save written there is loadable, nor that an interrupted write leaves the `.bak` recoverable |
| 7 | Performance is measured, not bounded | `artifacts/runtime-scale-results.json`: `day_advance_30d` count 5, min 0.548 / median **0.609** / mean 0.739 / p95 **1.145** / max **1.265 s**, 363 KB allocated — and `src/Host/PerformanceSelfTest.cs:47` labels the result `"advisory"` (Wave 3's 26C step 1) |
| 8 | Iteration count is too low to trust a p95 | n = 5 — a "p95" of five samples is a maximum with a decimal point |
| 9 | Long-session hazards are documented but manual | `docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`, Wave 1's 16C (lambda unsubscribe defects), `Main.Lifecycle.cs` session teardown — all real, none gated beyond `--panel-bind-lifecycle-selftest` |
| 10 | Manual QA exists as prose | `docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md` (incl. C4 "corrupt/missing save → Continue disabled or a clean error message… no crash"), `docs/HoldfastManualPlaytest.md`, `docs/qa/AUDIO_AND_SETTINGS_RECOVERY_SMOKE_TEST.md` |
| 11 | Data authority in the artifact | Wave 3's 26B: export staging double-packs or under-packs `Assets/StreamingAssets/Data` (413 JSON / 8.3 MB), and no exported build is ever booted in CI |
| 12 | The tree itself is a durability risk | 95 uncommitted paths on `main`; three Wave-3 doc gates still red |

---

## Task 39A — Turn the existing probes into release gates

**Goal:** every session-integrity check that already exists becomes mandatory, with a boot of the
*exported* artifact in the loop, and the soak gets enough iterations to mean something.

**Files:** `docs/ci/CI_GATE_MANIFEST.json`, `.github/workflows/ci.yml`,
`src/Host/SevenDayDeterministicSmokeTest.cs`, `src/Host/PerformanceSelfTest.cs`,
`artifacts/runtime-scale-results.json`, Wave 3's `scripts/ci/export-smoke-boot.sh`,
new `scripts/ci/release-gate.sh`, `docs/CI.md`, `docs/release/RELEASE_CHECKLIST.md` (or
`ashfall-release-captain`'s checklist).

### Substeps

1. **Register `7day_smoke_selftest`** as a Tier-2 (nightly) gate with an `expected_summary`, and a
   reduced 3-day variant as Tier-1 fast so every push sees some multi-day behaviour.
2. **Raise the iteration count** for perf sampling to a stated n (≥25 warm iterations after
   warmup, or a time budget) — then re-baseline the p95 honestly. Today n=5 cannot support a
   percentile, and a budget built on it would be theatre.
3. **Split tiers by intent**: fast tier = compile, data, determinism, boot; nightly = 7-day soak,
   perf budget, coverage, export boot; release = everything plus the checklist. State the tier
   contract in `docs/CI.md` and make `verify-fast.sh`'s list match it.
4. **Add a durability matrix** to the soak: save at day 1 → advance 40 → save → quit mid-panel
   (simulate `NotificationWMCloseRequest` while a modal is open) → load → assert state, and repeat at
   an odd day count so off-by-one restore bugs can't hide.
5. **Interrupted-write test**: kill the write between temp and rename and assert the previous save +
   `.bak` still load; assert a truncated file yields the documented clean error, not a crash
   (`MANUAL_PLAYTHROUGH_CHECKLIST.md` C4 is the spec — turn that row into a test).
6. **Disk-full and read-only path** behaviour: no silent loss, one clear message (the store layer
   already routes failures through `ILog`; prove it does).
7. **Slot isolation assertions**: two profiles, two slots, interleaved saves — no cross-write, no
   envelope mix-up, checksum mismatch rejected.
8. **Migration direction test**: an old-envelope (V1 filename-keyed) and a pre-envelope bare-state
   save both load, migrate in memory, and never get re-written as the old shape.
9. **Export boot in the gate chain** (Wave 3's 26B): CI builds via the staging script, boots
   headless, ticks one day, saves, reloads, exits 0 — on Linux and Windows artifacts.
10. **Publish a release report**: version, data counts (5563 ids / 138 catalogs), gate list with
    pass/fail, coverage %, unbound-port count (Wave 5's 36), `EFFECT_PRODUCED` count, snapshot set
    hash — one artifact that says what shipped.
11. **Fail loudly on unknowns**: any gate whose command exits 0 without producing its expected
    summary line is treated as failing (a silent pass is indistinguishable from a skipped test).
12. **Tests**: meta-tests that each new gate can fail (introduce a corrupt fixture save, a slow owner,
    a missing JSON in a PCK), because a gate that has never failed is a rumour.
13. **Run the checklist** + `bash scripts/ci/release-gate.sh` locally and paste the report.

**DoD:** a release candidate is produced by one command that can say no.

---

## Task 39B — Sessions that last: leaks, growth, and the 200-hour campaign

**Goal:** prove that a long campaign costs the same as a short one, per day, and that memory/node
counts return to baseline across sessions.

**Files:** `src/Host/PerformanceSelfTest.cs`, `Assets/Ashfall.Core/Performance/*`,
`src/Main.Lifecycle.cs`, `src/Main.PanelLifecycle.cs`, `src/UI/*` (subscription paths),
`docs/ui/UI_NODE_DIAGNOSTICS_AND_LEAK_TRIAGE.md`, new `scripts/ci/soak-gate.sh`,
`artifacts/soak-results.json`.

### Substeps

1. **Define the soak**: 360 in-game days with a scripted policy (dispatch, craft, treat, trade) plus
   20 new-game/load cycles, measuring per-day wall time, allocations, Godot node count, C# event
   handler counts, and `user://` growth.
2. **Assert monotonic stability, not absolute speed**: day 300 must cost like day 10; a per-day slope
   above the stated tolerance fails. This catches collections that only grow (journal, serving logs,
   `MealServingLog`, memorial rows, census claims).
3. **Cap unbounded collections explicitly**: any `List` in a persisted state that grows without a
   retention policy is a defect — add documented caps/rolling windows with tests (and never drop
   data a save checksum depends on without a version bump).
4. **Node-count baseline**: open→close all live panels (Wave 1's 16A set) 20×; assert the tree returns
   to its starting node count using the telemetry guide's instrumentation.
5. **Handler baseline**: after the same loop, assert each Core event's invocation-list length is
   unchanged (16C's rule, now measured continuously instead of once).
6. **Session-swap safety**: new game → load → new game → load, asserting no panel holds a freed
   authority (`ReferenceEquals` checks from 16B plus a freed-object probe).
7. **GC/alloc hot paths**: the day loop's known allocators — the per-tick `CollectWornGear` list
   (Wave 2's 21A step 10), modifier stacks (24B), cascade rules (23C), briefing assembly — reuse
   buffers and add allocation assertions per owner.
8. **Save size discipline**: the campaign envelope must not grow linearly with wall-clock play;
   measure and cap log sections.
9. **Long-idle behaviour**: 60 s of no input must not spin (the day loop is manual-advance, so
   `_Process` work and ambience loops are the suspects).
10. **Determinism under soak**: same seed + same policy ⇒ same final digest — the soak doubles as a
    determinism torture test (`ashfall-seed-replay`).
11. **Low-end target**: run the soak on the minimum spec (old iGPU, `gl_compatibility`) and record
    frame pacing during the heaviest dashboard, since 164 UI classes on one canvas is where a 2D
    game actually dies.
12. **Gate + baseline file**: `scripts/ci/soak-gate.sh` (nightly, Tier-2) writing
    `artifacts/soak-results.json`, trended in `docs/perf/README.md` (Wave 3's 26C).
13. **Tests**: retention policies, allocation assertions, node/handler baselines, digest equality.

**DoD:** the 360th day costs the same as the third, and nothing accumulates quietly.

---

## Task 39C — The player's side of durability: slots, continue, and never losing a run

**Goal:** make save/continue behaviour something the player can reason about — visible, recoverable,
and honest about what just happened.

**Files:** `src/Host/SaveLoadHostSession.cs`, `src/UI/MainMenuPanel.cs` / `MainMenuBuilder.cs`,
`src/Main.SaveOrchestrator.cs`, `src/Main.Holdfast.cs` (`AutoSaveOnDay`),
`docs/qa/MANUAL_PLAYTHROUGH_CHECKLIST.md`, Wave 3's 25A string layer, Wave 4's 31B briefing routes,
new `docs/saves/PLAYER_SAVE_MODEL.md`.

### Substeps

1. **Write the player-facing model first** (one page): what a slot is, what autosave does, what
   "Continue" loads, what `.bak` means to a human, and when a save can *not* be trusted. Everything
   in this task implements that page.
2. **Slot metadata**: each slot shows day, season/chapter (38A), crew alive, last-saved wall time,
   difficulty preset (34B), and ending-in-progress state — all already persisted; the UI just isn't
   showing it.
3. **Continue honesty**: `Continue` disabled with a reason when the newest slot is corrupt or newer
   than the build's schema (the C4 checklist row made machine-checkable, feeding 39A step 5).
4. **Autosave visibility**: when `AutoSaveOnDay` writes, say so in the existing save-success feedback
   channel (cue + status line) so the player knows the last safe point — and add a distinct warning
   when a save *failed*, which currently only appears in `GD.PushWarning` scroll.
5. **Manual save slots + overwrite confirmation**, naming the target ("Day 214 · Late Thaw · 4
   alive") before destroying the old one.
6. **Recovery affordance**: surface the `.bak` explicitly ("try the previous save") when the primary
   fails checksum validation — the mechanism exists, the player can't reach it.
7. **Never leave the player worse off for saving**: prove save→load→save is idempotent
   (byte-stable envelope for unchanged state), which also protects the checksum tests from churn.
8. **Migration messaging**: a load that upgraded a save (V1→V2, pre-envelope → envelope) must say so
   once, in plain language, and record it in the slot metadata.
9. **Failure surfaces use the standard error path**: `UserSettingsStore.HasDiagnosticMessage` already
   models a persisted diagnostic message with a clear method — reuse that pattern for save errors
   instead of a second mechanism.
10. **Quit-during-write**: confirm the close-request handler (row 6) can't strand a partially written
    envelope, and that a quit mid-save is reported at next boot.
11. **Accessibility of the save UI**: keyboard-navigable (37B), text-labelled (not icon-only), and
    localised (25A) — the screen where a lost run is most expensive is the screen least allowed to be
    confusing.
12. **Tests**: slot metadata round-trip, continue-disabled paths, autosave notification, recovery
    prompt, idempotent save→load→save digest equality, overwrite confirmation, and a scripted
    "corrupt the primary, keep the .bak" scenario.
13. **Manual QA**: extend the playthrough checklist with this matrix and attach the run; re-verify
    with `ashfall-save-fuzz` (existing skill) before release.

**DoD:** a player can always answer "what did I save, where, and what happens if I quit now?"

---

## Cross-Task Dependencies

```
36 (port contract) ──► 39A step 10's report pulls the unbound-port count
26B (export + boot) ──► 39A steps 9–12
38A (calendar)      ──► 39C step 2 (slot metadata: season/chapter)
34B (difficulty)    ──► 39C step 2, 39A step 4 (soak policies)
31B (briefing)      ──► 39C steps 4,6 (where failures become visible)
27B/27C (coverage, journeys) ──► 39A step 3's tier contract
        │
39A (gates) ──► 39B (soak) ──► 39C (player-facing save model)
```

**Execution order:** 39A → 39B → 39C. 39C depends on 39A step 5 (recovery behaviour must be tested
before it is exposed) and on Wave 3's localization seam (39C step 11) for its text.

**Wave 5 overall order:** 36A → 35A → 37A → 38A → 39A → 36B → 35B → 37B → 38B → 39B → 36C → 35C →
37C → 38C → 39C. Three tasks only, if forced: **36A, 35A, 38A** — seams proven bound, goods that
arrive, and a clock that means something.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. godot --headless --path . -- --7day-smoke-…                   # (39A verb, now a gate)
7. godot --headless --path . -- --save-load-ui-failure-selftest
8. godot --headless --path . -- --runtime-scale-selftest         # n≥25, budgeted
9. bash scripts/ci/soak-gate.sh                                  # 360-day + 20 session cycles
10. bash scripts/ci/export-smoke-boot.sh                         # boot the artifact
11. bash scripts/ci/release-gate.sh                              # one command, one report
12. ashfall-save-fuzz + ashfall-seed-replay + ashfall-lfs-gate
13. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | Core | Host | Tooling | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 39A | 0 | 1 | 3 scripts + manifest | 6–10 | Low–Med | LOW (CI-side; failures are truthful, not spurious — verify before gating) |
| 39B | 2–3 (retention) | 2 | 1 soak gate | 10–14 | Medium–High | LOW–MED (caps change saved data shape → version bump) |
| 39C | 0 | 3–4 | 0 | 10–14 | Medium | LOW (additive UI on a stable store layer) |

**Guardrails:** no new save format, no cloud/Steam sync, no autosave-policy change without a
player-facing explanation, no gate added that hasn't been proven able to fail, and no percentile
claim from five samples.
