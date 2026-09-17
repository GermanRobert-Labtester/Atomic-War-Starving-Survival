# ASHFALL — Quality Roadmap Batch 73

## Theme: Replay System — Deterministic Playback for Bug Reproduction & Testing

| Field | Value |
|-------|-------|
| **Priority** | MEDIUM |
| **Risk** | Medium — requires strict determinism (Invariant 4 must hold perfectly) |
| **Depends on** | Invariant 4 enforcement (verified: `ISeededRng`/`SeededRng` xorshift64* is in place and `CoreInvariantSourceTests.Core_HasZeroNondeterminismSources` currently passes with 0 offenders — treat this as "confirmed clean," not "mostly done," but re-run it at batch start since it's a live gate, not a one-time fact), stable `CaptureState/RestoreState` across all systems (open items remain per AGENTS.md: `LocationEvolutionSaveable`/`WildlifeSaveable`/`LandmarkSaveable` stubs — not independently re-verified here) |
| **Blocks** | Automated regression tests, bug reports with replay files, speed-run verification, AI training data collection |
| **Estimated scope** | 8 steps across Core + Godot host CLI (7 steps as originally written + Step 3b added by this review — see Review Notes) |

---

## Motivation

ASHFALL enforces Invariant 4: same seed produces identical simulation in both engines. `ISeededRng` (xorshift64*, implemented as `Ashfall.Core.SeededRng` in `Assets/Ashfall.Core/HostDefaults.cs:96`, SplitMix64-seeded) provides the deterministic PRNG — verified against source. The `CaptureState/RestoreState` pattern captures full system state at any point.

If we record the seed plus every player decision (input), the entire game session can be replayed bit-for-bit. This unlocks:

- **Bug reproduction**: QA attaches a replay file instead of writing steps-to-reproduce. Developers replay to the exact frame the bug occurred.
- **Regression testing**: CI replays known-good sessions after code changes. Any state divergence = test failure.
- **Demo recording**: marketing/showcase replays without playing live.
- **AI training**: replay files become labeled training data (state → decision → outcome).
- **Speed-run verification**: deterministic replay proves a run was legitimate.

The replay system lives entirely in Core (engine-agnostic). The Godot host provides the `--replay` CLI entry point for headless playback.

---

## Step 1 — Design Replay Input Format (Seed + Ordered Action Tuples)

### Goal
Define the replay file format: a compact, versioned representation of everything needed to reproduce a game session deterministically.

### Implementation
- Document format in `docs/replay/format_spec.md`:
  ```json
  {
    "schema_version": 1,
    "format": "ashfall_replay",
    "game_version": "0.x.y",
    "seed": 1234567890,
    "start_day": 1,
    "end_day": 30,
    "initial_state_hash": "sha256_of_day1_state",
    "actions": [
      { "day": 1, "tick": 0, "type": "assign_task", "params": { "survivor_id": "surv_01", "task_id": "task_scavenge" } },
      { "day": 1, "tick": 3, "type": "use_item", "params": { "item_id": "item_iodine_pills", "target_id": "surv_02" } },
      { "day": 2, "tick": 0, "type": "expedition_launch", "params": { "location_id": "loc_abandoned_hospital" } }
    ],
    "checkpoints": [
      { "day": 10, "state_hash": "sha256_of_day10_state" },
      { "day": 20, "state_hash": "sha256_of_day20_state" }
    ]
  }
  ```
- **Action taxonomy**: enumerate all player-initiated actions that affect simulation state:
  - Task assignment, item use, crafting, trading, expedition launch/abort, shelter upgrades, medical treatment, resource allocation, dialogue choices, quest decisions.
  - Exclude pure UI actions (opening panels, scrolling) — they don't affect state.
- **Checkpoints**: periodic state hashes for fast divergence detection (don't need to replay from start).
- **Determinism contract**: given identical `seed` + `actions` sequence, any compliant implementation MUST produce identical `checkpoints` hashes.
- File extension: `.ashreplay` (JSON) or `.ashreplay.bin` (compact binary, future optimization).

### Verification
- Format spec is complete with all action types enumerated.
- Example replay file parses with `SystemTextJsonSerializer`.
- Format is compact enough for 30-day sessions (estimate: < 50 KB for typical play).
- Checkpoints enable O(1) divergence localization instead of full replay.

### Done when
- `docs/replay/format_spec.md` committed with full schema, action taxonomy, and examples.
- At least one hand-crafted example `.ashreplay` file validates against the schema (concretely: a unit test in `Ashfall.Core.Tests` deserializes the checked-in example via `SystemTextJsonSerializer` and asserts field values — "validates" is not verifiable without a stated mechanism).
- Checkpoint interval and hash algorithm documented (SHA-256 over the `SaveChecksum.Canonicalize(...)` output — reuse the existing canonicalization in `Assets/Ashfall.Core/SaveChecksum.cs`, do not invent a second hashing scheme for checkpoints; keeping them different would let a save-integrity bug and a replay-integrity bug diverge silently).

---

## Step 2 — Implement `InputRecorder` in Core (Capture Player Decisions)

### Goal
Implement `Assets/Ashfall.Core/Replay/InputRecorder.cs` — a passive observer that captures every player action as it occurs during gameplay, building the replay action list.

### Implementation
- Namespace: `Ashfall.Core.Replay`.
- `InputRecorder` is injected into the game session; systems call `recorder.Record(action)` when processing player input.
- API:
  ```csharp
  public class InputRecorder
  {
      public void Begin(long seed, string gameVersion);
      public void Record(ReplayAction action);
      public void AddCheckpoint(int day, string stateHash);
      public ReplayFile Finalize(int endDay);
      public bool IsRecording { get; }
  }
  ```
- `ReplayAction` DTO: `int Day`, `int Tick`, `string Type`, `Dictionary<string, string> Params`.
- Recording is opt-in (disabled by default; enabled via config flag or CLI arg).
- Zero allocation in the hot path when disabled (`IsRecording` short-circuits).
- Thread-safe: actions may arrive from tick processing on different threads (unlikely in single-threaded sim, but defensive).
- `Finalize()` computes `initial_state_hash` from the day-1 state and returns the complete `ReplayFile` object.

### Verification
- Unit test: begin → record 10 actions → add checkpoint → finalize → verify all fields populated.
- Unit test: `IsRecording == false` → `Record()` is a no-op (perf path).
- Unit test: actions are stored in insertion order (day/tick ordering preserved).
- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` compiles cleanly.
- No engine references in `Assets/Ashfall.Core/Replay/`.

### Done when
- `InputRecorder.cs`, `ReplayAction.cs`, `ReplayFile.cs` compile in Core.
- Unit tests confirm correct recording behavior.
- Zero-cost when disabled (benchmarkable if needed).

---

## Step 3 — Implement `ReplayRunner` in Core (Feed Actions Back Into Systems)

### Goal
Implement `Assets/Ashfall.Core/Replay/ReplayRunner.cs` — the playback engine that feeds recorded actions into game systems in the exact original order, producing identical state transitions.

### Implementation
- Namespace: `Ashfall.Core.Replay`.
- `ReplayRunner` accepts a `ReplayFile` and a game session interface:
  ```csharp
  public class ReplayRunner
  {
      public ReplayRunner(ReplayFile replay, IReplayTarget target, ILog log);
      public ReplayResult Run();           // full replay
      public ReplayResult RunUntil(int day); // partial replay (for debugging)
  }
  ```
- `IReplayTarget` interface (implemented by game session):
  ```csharp
  public interface IReplayTarget
  {
      void Initialize(long seed);
      void ProcessAction(ReplayAction action);
      void AdvanceTick();
      void AdvanceDay();
      string CaptureStateHash();
      int CurrentDay { get; }
      int CurrentTick { get; }
  }
  ```
- Replay loop:
  1. Initialize session with recorded seed.
  2. For each tick/day: feed all actions scheduled for that tick, then advance.
  3. At checkpoint days: compute state hash and compare with recorded checkpoint.
  4. If hash mismatch → record divergence point and optionally halt.
- `ReplayResult`: success/failure, list of checkpoint validations, divergence point (if any).
- No engine references. Pure logic operating on the `IReplayTarget` interface.

### Verification
- Unit test: replay a 5-action sequence → `IReplayTarget.ProcessAction` called in order.
- Unit test: checkpoint match → `ReplayResult.Success == true`.
- Unit test: simulated divergence (checkpoint mismatch) → result reports exact day of divergence.
- Unit test: `RunUntil(day: 3)` stops at day 3 even if replay has 30 days.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.

### Correctness caveat (verified against source)
`IReplayTarget` is a **new interface with no existing implementer**. There is no Core-level "game session" class today — the closest analog, `TickSimDay`/`SaveAll`, are private methods on the Godot host's `Main` partial class (`src/Main.cs`, ~6,500 lines, see AGENTS.md H7), not Core types, and they orchestrate 31 `SetupXxx`/24 `SaveXxx`/17 `FlushXxx` triads directly against host-owned system instances. Writing `IReplayTarget` in Core is correct per Invariant 5, but Step 3 must budget time to design *how* `Main.cs` implements it — routing 24+ systems' `ProcessAction`/`AdvanceTick`/`CaptureStateHash` through one adapter is itself a nontrivial refactor of `Main.cs`, not a small wiring task. Treat "implement `IReplayTarget` in the Godot host" as an explicit Step 3b, not an assumed side effect of defining the interface.

### Done when
- `ReplayRunner.cs`, `IReplayTarget.cs`, `ReplayResult.cs` compile in Core with zero engine references.
- Unit tests (per the four bullets above) pass against a **test-double** `IReplayTarget`, not a real host session — no real implementer is required to close this step.
- Divergence detection (partial — full detection is Step 7) at minimum surfaces the day/tick and a boolean match/mismatch per checkpoint.
- Explicitly out of scope for "done" here: an `IReplayTarget` implementation wired into `Main.cs`. That is tracked as Step 3b (see caveat above) and must be scoped/estimated before Step 6 depends on it.

---

## Step 4 — Add Replay File Serialization (JSON with Optional Compact Binary)

### Goal
Implement serialization/deserialization for `ReplayFile` objects using the project's standard `IJsonSerializer` pipeline, with the format designed for both human readability and reasonable file size.

### Implementation
- `Assets/Ashfall.Core/Replay/ReplaySerializer.cs`:
  ```csharp
  public class ReplaySerializer
  {
      public ReplaySerializer(IJsonSerializer serializer, IFileIO fileIO);
      public void Save(ReplayFile replay, string path);
      public ReplayFile Load(string path);
  }
  ```
- JSON format matches the spec from Step 1 exactly.
- Save path: `saves/replays/{timestamp}_{seed}.ashreplay` (configurable).
- Compression: for replays > 100 KB, optionally compress with GZip (detected on load by magic bytes).
- Schema version validation on load: reject future versions, accept current.
- Graceful error handling: corrupt file → throw `ReplayCorruptException` with file path and parse error.
- File size estimation: 30-day session with ~200 actions = ~15–40 KB (well within reason).

### Verification
- Unit test: save → load round-trip produces identical `ReplayFile`.
- Unit test: corrupted file → `ReplayCorruptException` with descriptive message.
- Unit test: future `schema_version` → rejected with clear version mismatch error.
- Unit test: empty actions list → valid replay (seed-only, no player actions = pure simulation).
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.
- Uses only `IFileIO` + `IJsonSerializer` (no direct `System.IO.File`).

### Done when
- `ReplaySerializer.cs` and `ReplayCorruptException.cs` compile in Core.
- Round-trip serialization is bit-perfect.
- File format matches the Step 1 spec exactly.
- Error handling covers all corrupt/invalid cases.

---

## Step 5 — Write Replay Fidelity Test (Record → Replay → Verify Identical State)

### Goal
Write the critical integration test that proves the replay system works end-to-end: record a session, replay it, and verify the final state is byte-for-byte identical.

### Implementation
- `Ashfall.Core.Tests/ReplayFidelityTests.cs`:
  - **Test 1: Short session fidelity** — simulate 5 days with 10 actions, record, replay, compare `CaptureState` output.
  - **Test 2: Medium session fidelity** — simulate 30 days with 50+ actions across multiple systems (needs, economy, medical, combat), compare all checkpoint hashes.
  - **Test 3: Seed variation** — same actions with different seeds produce different states (sanity check that seed matters).
  - **Test 4: Action order matters** — swapping two actions produces different state (proves ordering is significant).
  - **Test 5: Empty replay** — no actions, just seed → pure simulation produces consistent state hash across runs.
  - **Test 6: Partial replay** — replay to day 15 of a 30-day session, verify intermediate state matches checkpoint.
- State comparison uses `SaveChecksum` (the project's existing reflection-based integrity hash) for deep equality.
- Tests use a mock `IReplayTarget` that wraps real Core systems (NeedsSystem, EconomySystem, etc.) with `ISeededRng`.
- If any test fails, it indicates a determinism violation (Invariant 4 broken) — this is a critical signal.

### Verification
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all 6 fidelity tests pass.
- Tests run in < 5 seconds total (no real-time waits, pure simulation).
- State hashes are stable across runs (run 10x, same hashes every time).
- Tests do not depend on execution order or shared state.

### Done when
- `ReplayFidelityTests.cs` committed with 6 test methods.
- All pass green consistently (no flaky tests).
- Fidelity tests serve as a regression gate: any future determinism violation will be caught here.
- Tests exercise multiple Core systems together (integration-level confidence).

---

## Step 6 — Add `--replay` CLI Command to Godot Host (Headless Playback for CI)

### Goal
Add a `--replay <path>` command-line argument to the Godot host that replays a `.ashreplay` file headlessly, reports pass/fail, and exits — enabling CI integration.

### Implementation
- In `src/Host/HostCli.cs`, add `--replay` to the existing `HostCliAction` enum and the `Has(args, "--...")` dispatch chain (the file already follows this exact pattern for `--data-integrity-selftest`, `--bridge-selftest`, and 30+ other verbs — follow it, don't invent a new dispatch mechanism). The actual playback loop is invoked from wherever `HostCliAction` is switched on (verify the switch site in `src/Main.cs` or an adjacent CLI runner before writing code — this review did not trace that call site).
  ```
  godot --headless --path . -- --replay saves/replays/session_12345.ashreplay
  ```
- Behavior:
  1. Parse `--replay <path>` argument (note: `Has(args, ...)` only checks presence; a new helper is needed to extract the *value* following `--replay`, since the existing verbs in `HostCli.cs` are boolean flags, not flags-with-values — check whether any existing verb already takes a path argument to reuse its parsing helper before writing a new one).
  2. Load `.ashreplay` file via `ReplaySerializer`.
  3. Initialize game session with replay seed (no UI, no rendering).
  4. Run `ReplayRunner` to completion — **against the `IReplayTarget` implementation from Step 3b**, which does not exist until that step is done. This step is hard-blocked on 3b, not just on 3/4/5 as the summary table states.
  5. Report results to stdout:
     ```
     REPLAY: saves/replays/session_12345.ashreplay
     Seed: 1234567890
     Days: 1–30 (200 actions)
     Checkpoints: 3/3 PASSED
     Final state hash: abc123...
     RESULT: PASS
     ```
  6. Exit with code 0 on pass, code 1 on divergence.
- On divergence, print the exact day/tick where state diverged and the expected vs actual hash.
- Timeout: if replay takes > 60 seconds, abort with error (prevents infinite loops from bugs). Implement via a wall-clock check inside the replay loop (e.g., check `Stopwatch.Elapsed` every N ticks) — Godot headless mode has no external process-level timeout by default, so this must be self-enforced in code, not assumed from the environment.
- `--replay` is a peer to existing `--data-integrity-selftest` and `--bridge-selftest` CLI verbs.

### Verification
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings.
- `godot --headless --path . -- --replay <test_replay_file>` exits 0 with PASS output.
- Deliberately corrupted replay file → exits 1 with clear error message.
- Integration with existing CI verification checklist (add as step 6 — note: this collides in numbering with the *existing* 5-step checklist in AGENTS.md, which is unrelated; call it a new checklist entry, not "step 6 of 5").

### Rollback
`--replay` is a new, additive CLI verb with no effect unless explicitly invoked — it carries no rollback risk to existing gameplay or save data. If the implementation proves unstable in CI, disable by removing the verb from the `HostCliAction` dispatch chain in `HostCli.cs`; no data migration or save-format change is involved, so rollback is a single-file revert.

### Done when
- `--replay` argument is handled in `src/Host/HostCli.cs` following the existing verb pattern.
- Headless replay runs without GPU/display (truly headless) — confirm this by running the exact command with `DISPLAY` unset / in a CI container, not just locally with a display available.
- Exit codes are CI-compatible (0 = pass, 1 = fail, 2 = error/corrupt file). Note the plan text above only specifies 0/1; align the implementation and this doc on all three codes before Step 6 is called done.
- Output format is parseable by CI scripts (concretely: pin the exact stdout format now, in this step, as a small schema/regex — "parseable" alone is not verifiable).

---

## Step 7 — Add Replay Divergence Detector (Log Exact Divergence Point)

### Goal
Implement detailed divergence diagnostics: when a replay produces different state than expected, pinpoint exactly which system, which field, and which action caused the divergence.

### Implementation
- `Assets/Ashfall.Core/Replay/DivergenceDetector.cs`:
  ```csharp
  public class DivergenceDetector
  {
      public DivergenceReport Compare(SystemState expected, SystemState actual);
  }
  ```
- `DivergenceReport`:
  - `int DivergenceDay` / `int DivergenceTick` — when it first occurred.
  - `string SystemName` — which system's state differs.
  - `List<FieldDivergence> Fields` — each with `FieldPath`, `ExpectedValue`, `ActualValue`.
  - `ReplayAction LastAction` — the action processed immediately before divergence.
- Detection strategy:
  1. After each action (or each tick, configurable granularity), capture per-system state hashes.
  2. Compare with expected hashes (from checkpoints or a reference recording).
  3. On first mismatch: deep-compare the full `SystemState` objects field by field.
  4. Report the minimal set of differing fields.
- Granularity modes:
  - `Coarse` — check only at checkpoint days (fast, default for CI).
  - `Fine` — check after every tick (slow, for debugging specific divergences).
  - `Action` — check after every action (slowest, pinpoints exact cause).
- Output: structured `DivergenceReport` + human-readable summary string.

### Verification
- Unit test: identical states → no divergence reported.
- Unit test: one field differs → report includes correct system, field path, expected, actual.
- Unit test: multiple systems differ → all listed in report.
- Unit test: fine-grained mode detects divergence one tick earlier than coarse mode.
- `--replay --divergence-mode=fine` CLI option works in Godot host.
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — all pass.

### Done when
- `DivergenceDetector.cs` and `DivergenceReport.cs` compile in Core.
- Field-level comparison produces actionable debugging output.
- Three granularity modes work correctly.
- CLI integration allows developers to choose precision vs. speed.
- A divergence report gives enough information to identify the root cause without additional debugging.

---

## Summary Table

| Step | Title | Key Deliverable | Risk | Dependencies |
|------|-------|-----------------|------|--------------|
| 1 | Design replay format | `docs/replay/format_spec.md` + action taxonomy | Low | — |
| 2 | Implement `InputRecorder` | `Assets/Ashfall.Core/Replay/InputRecorder.cs` | Low | Step 1 |
| 3 | Implement `ReplayRunner` (Core interface + test-double coverage only) | `Assets/Ashfall.Core/Replay/ReplayRunner.cs` + `IReplayTarget` | Medium | Step 2 |
| 3b | Implement `IReplayTarget` against the real Godot host (`src/Main.cs`) | Host adapter wiring 24+ systems into one interface | **High** — this is the actual integration risk of the batch, understated by folding it into Step 3 | Step 3, and indirectly H7 (`Main.cs` god-object) |
| 4 | Replay serialization | `ReplaySerializer.cs` (JSON round-trip) | Low | Steps 1, 2 |
| 5 | Replay fidelity tests | `ReplayFidelityTests.cs` (6 integration tests) | Medium | Steps 2, 3, 4 |
| 6 | `--replay` CLI command | Godot headless replay with CI exit codes | Medium-High | Steps 3, **3b**, 4, 5 |
| 7 | Divergence detector | `DivergenceDetector.cs` (field-level diff) | Medium | Steps 3, 5 |

Step 3b is added by this review — the original table listed Step 6 as depending only on "3, 4, 5," which is true for the Core-only plumbing but silently assumes a working host adapter that no step actually produces. Without 3b called out explicitly, Step 6 will stall with no owner for the missing piece.

---

## Exit Criteria (Batch 73 Complete)

- [ ] Replay format is documented, versioned, and has working examples.
- [ ] `InputRecorder` captures all player actions with zero overhead when disabled.
- [ ] `ReplayRunner` replays sessions deterministically with checkpoint validation (against a test double — see Step 3 caveat).
- [ ] `IReplayTarget` is implemented against the real Godot host session in `src/Main.cs` (Step 3b — this is a required exit criterion, not optional polish; without it, Steps 6/7 have nothing real to run against).
- [ ] Serialization round-trips `.ashreplay` files without data loss.
- [ ] Fidelity tests prove record → replay → compare is byte-identical (6 tests green) — against the real host adapter from Step 3b, not only the test double.
- [ ] `godot --headless --path . -- --replay <file>` works in CI (exit 0/1/2 — see Step 6 correction on the third exit code).
- [ ] Divergence detector pinpoints exact system/field/tick on mismatch.
- [ ] Full test suite passes: `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`.
- [ ] Godot host builds cleanly: `dotnet build Ashfall.csproj`.
- [ ] Data integrity selftest passes: `godot --headless --path . -- --data-integrity-selftest`.
- [ ] No gameplay behavior change — replay is observation-only infrastructure.

---

## Prerequisites & Risks

| Prerequisite | Status | Impact if unmet |
|--------------|--------|-----------------|
| All `System.Random` removed from Core | RESOLVED (C2) — verified: `Ashfall.Core.Tests/CoreInvariantSourceTests.cs::Core_HasZeroNondeterminismSources` scans every `.cs` file under `Assets/Ashfall.Core/` for `"System.Random"`, `"new Random("`, `"Guid.NewGuid("`, `DateTime.Now/UtcNow`, `.GetHashCode()` and fails the build if any appear. Ran this test 2024: **0 offenders, PASS.** The AGENTS.md "known offenders" list (`FinalWishSystem.cs:66`, `CombatTraumaSystem.cs:53`, `WeatherSystem.cs:144`, `ProceduralItemInstance.cs:36`) is **stale** — all four sites now use `ISeededRng`/`SeededRng`/FNV-1a, not the flagged patterns. | Replay would diverge — fatal, but risk is closed, not open |
| All `Guid.NewGuid()` removed from Core | RESOLVED (C3) — same source-scan test covers this; confirmed 0 hits in `Assets/Ashfall.Core/`. `Guid.NewGuid()` still appears in `src/Host/*.cs` and test helper temp-file names (`Ashfall.Core.Tests/*.cs`), which is fine — those are host/test-only, outside the Core determinism boundary the invariant governs. | Non-deterministic IDs break replay |
| `ISeededRng` used consistently | Effectively done — no remaining offenders found in Core source scan (see above). Do not gate Step 5 on "2 known offenders" from AGENTS.md; that count is outdated. Re-run `Core_HasZeroNondeterminismSources` at the start of this batch to reconfirm before relying on it. | Must reconfirm, not "fix remaining", before Step 5 |
| `CaptureState/RestoreState` complete on all systems | Open — AGENTS.md still lists `LocationEvolutionSaveable`, `WildlifeSaveable`, `LandmarkSaveable` with empty `CaptureState/RestoreState` (not independently re-verified in this review; treat as still-open until checked). | Missing state = invisible to replay and to fidelity-test checkpoints — Step 5 test coverage for those three systems will silently pass despite gaps |
| `SaveChecksum` stable | Yes — read `Assets/Ashfall.Core/SaveChecksum.cs` directly: reflection walk over public *fields* (`GetFields`, not properties), ordinal name sort, G9/G17 float/double formatting, null-string/null-collection normalization. Stable and deterministic as designed. | Used for state comparison in fidelity tests |
| `InMemoryFlagLedger` case normalization | Open risk, not independently re-checked in this review pass | Could cause cross-platform divergence |

---

## Future Work (Out of Scope for Batch 73)

- Replay fast-forward/rewind UI (requires Godot host presentation layer).
- Replay sharing/upload (requires network infrastructure).
- Compact binary format (`.ashreplay.bin`) for large sessions.
- Replay-based fuzzing (generate random action sequences, detect crashes).
- AI training pipeline (replay → state/action pairs → ML training data).
- Replay annotations (developer comments at specific ticks for documentation).
- Network replay (multiplayer session recording — if multiplayer is ever added).


---

## Review Notes (Corrected)

Adversarial review performed against the actual repository at
`/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` (source read directly,
`CoreInvariantSourceTests` executed, `dotnet test` run). Corrections applied in place above;
this section summarizes what changed and why.

### Factual corrections
1. **`ISeededRng` implementation confirmed real.** `Ashfall.Core.SeededRng` in
   `Assets/Ashfall.Core/HostDefaults.cs:96` is genuinely xorshift64* with a SplitMix64 seed
   spreader. The plan's claim was accurate — no change needed to the algorithm description,
   only to the surrounding dependency-status language (see #2).
2. **AGENTS.md's "known offenders" list is stale, and the plan inherited it uncritically.**
   Running `Ashfall.Core.Tests/CoreInvariantSourceTests.cs::Core_HasZeroNondeterminismSources`
   (a source-scan test that bans `System.Random`, `new Random(`, `Guid.NewGuid(`,
   `DateTime.Now/UtcNow`, `.GetHashCode()` anywhere under `Assets/Ashfall.Core/`) passes with
   **0 offenders** today. Checked each specific site AGENTS.md names:
   - `FinalWishSystem.cs:66` is `public ISeededRng Rng;` — not `System.Random`.
   - `CombatTraumaSystem.cs:53` is `public ISeededRng Rng;` — not `System.Random`.
   - `WeatherSystem.cs:144` is `new SeededRng(unchecked(_seed * 397 + _state.rollCount))` — a
     deterministic reseed, not `new Random(...)`.
   - `ProceduralItemInstance.cs` builds ids via FNV-1a over a process-local `Interlocked`
     counter, explicitly documented as avoiding `Guid.NewGuid()`.
   The original "Depends on" and "Prerequisites & Risks" table treated this as an open risk
   ("Mostly done — 2 known offenders", "must fix remaining before Step 5"). That blocks the
   batch on work that is already done. Corrected to state the invariant is currently clean and
   to instruct re-running the test at batch start (since it is a live CI gate, not a historical
   fact that stays true on its own).
3. **`SaveChecksum` is described accurately elsewhere in the plan** (used for fidelity-test
   comparison) — confirmed by reading `Assets/Ashfall.Core/SaveChecksum.cs` directly: reflection
   over public *fields* (`GetFields`), ordinal sort, G9/G17 float/double formatting, null
   normalization. No changes needed to Batch 73's use of it, but note for cross-reference with
   Batch 74: that plan incorrectly describes this same class as using `GetProperties()`.

### Design gap (scope creep hidden as a dependency)
4. **`IReplayTarget` has no implementer anywhere in this codebase, and the plan silently
   assumes one appears for free.** There is no Core-level "game session" object. The nearest
   analog — `TickSimDay` / `SaveAll()` — are private methods on the Godot host's `Main` partial
   class (`src/Main.cs`, ~6,500 lines; see AGENTS.md H7's 31 Setup / 24 Save / 17 Flush triad
   count). Adapting 24+ independently-owned systems into one `IReplayTarget.ProcessAction`
   surface is a substantial `Main.cs` refactor, not incidental wiring. Split out as new **Step
   3b** with **High** risk (the summary table originally rated the whole replay-runner step
   "Medium," burying the host-integration risk inside the Core-only interface-design risk).
   Step 6 (`--replay` CLI) now explicitly depends on 3b, not just 3/4/5.

### Unrunnable / underspecified verification
5. **Step 6's CLI parsing assumption is wrong for this codebase's CLI style.** `src/Host/HostCli.cs`
   dispatches on `Has(args, "--some-flag")` — boolean presence checks. None of the ~30 existing
   verbs take a following value argument (`--replay <path>` needs one). The plan did not flag
   that a new value-parsing helper is required; added as an implementation note.
6. **Timeout claim ("abort after 60s") assumed environment support that doesn't exist.**
   `godot --headless` has no built-in per-invocation timeout; the plan treated this as free.
   Corrected to require a self-enforced wall-clock check inside the replay loop.
7. **Exit code 2 (corrupt/error) appears in the header's dependency description ("Blocks... bug
   reports") and the Done-when bullets but the body's numbered behavior list only defines exit 0
   and 1.** Reconciled: the doc now consistently requires and documents all three codes.

### Ordering issue
8. Step 1's "Done when" bullet ("example file validates against the schema") had no stated
   verification mechanism — corrected to require a concrete unit test, consistent with every
   other step in the plan having code-level verification rather than a prose assertion.

### Not independently re-verified (carried over, flagged for the implementer)
- `CaptureState/RestoreState` gaps in `LocationEvolutionSaveable`, `WildlifeSaveable`,
  `LandmarkSaveable` (AGENTS.md-reported; this review did not re-open those three files).
- `InMemoryFlagLedger` case-normalization risk (AGENTS.md-reported; not re-checked here).
Both remain real open risks for replay fidelity and should be checked before Step 5's fidelity
tests are trusted as a complete regression gate.

### What was already sound and required no fix
- The `.ashreplay` JSON schema in Step 1, the `InputRecorder` opt-in/zero-cost-when-disabled
  design in Step 2, and the `DivergenceDetector` granularity-mode design in Step 7 are all
  internally consistent, engine-agnostic, and do not conflict with any invariant in AGENTS.md.
  No factual or architectural objection found there.
