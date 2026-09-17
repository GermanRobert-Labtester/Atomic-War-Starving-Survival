# ASHFALL — Quality Roadmap Batch 59

## Theme: Determinism Sweep — Verify the Fix + Add a Permanent CI Gate

**Priority:** LOW-MEDIUM (Invariant 4 already holds; this batch is now about *proving and pinning* it, not fixing anything)
**Risk:** Low — no production code changes expected; only tests/tooling
**Batch:** 59
**Depends on:** `ISeededRng` interface and `SeededRng` (xorshift64*, in `Assets/Ashfall.Core/HostDefaults.cs`) already implemented and already in use by all four cited systems
**Blocks:** Nothing new — Invariant 4 is not currently blocking anything. Retained as a hygiene batch.

---

## ⚠️ CORRECTED PREMISE — read before doing any work

This batch as originally written assumed four Core files still contained live `System.Random`/`Guid.NewGuid()` offenders, quoting AGENTS.md's "Known offenders" list under Invariant 4. **That list is stale.** Direct inspection of the current tree (see Review Notes at the bottom) shows all four are already fixed:

| File | AGENTS.md claim | Actual current state |
|---|---|---|
| `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs:66` | `public System.Random Rng;` | `public ISeededRng Rng;` (line 66) — already fixed |
| `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs:53` | `public System.Random Rng;` | `public ISeededRng Rng;` (line 53) — already fixed |
| `Assets/Ashfall.Core/World/WeatherSystem.cs:144` | `new Random(unchecked(...))` | No `Random` construction anywhere in the file; class-doc comment (lines 44-49) states it reseeds fresh from seed + rollCount per roll instead of persisting RNG state — already fixed |
| `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs:36` | `Guid.NewGuid()` | `MakeInstanceId` uses FNV-1a hashing over itemId + an `Interlocked.Increment` counter, explicitly documented "No Guid.NewGuid" (lines 46-50) — already fixed |

A repo-wide grep for `System\.Random|new Random\(|Guid\.NewGuid` under `Assets/Ashfall.Core/` returns **zero live call sites** — every hit is a doc-comment stating the *absence* of the pattern (in `DiseaseSystem.cs`, `SilentFoundrySystem.cs`, `HostDefaults.cs`, `ProceduralItemInstance.cs`, `SkillProgressionSystem.cs`, `UtilityAiSystem.cs`).

Original Steps 2–4 (rewrite FinalWishSystem/CombatTraumaSystem/WeatherSystem) are **deleted** — there is nothing left to fix. Step 5 (verify the Guid fix) is **retained but re-scoped as a confirmation, not an investigation** since the fix is confirmed complete. Steps 1, 6, 7 (audit, regression tests, CI gate) are the only steps with real remaining value, and are kept.

Also note: the original plan invented `ISeededRng` API surface that does not exist. The actual interface (`Assets/Ashfall.Core/Ports.cs`) is:
```csharp
public interface ISeededRng
{
    int Seed { get; }
    int Next(int minInclusive, int maxExclusive);
    float NextFloat();
    double NextDouble();
}
```
There is no `NextInt`, no `Fork()`, no `DeriveChild(string)`. Any future code referencing those must not be written — use `Next(min, max)`, `NextFloat()`, `NextDouble()` only.

---

## Motivation

Invariant 4 demands: same seed ⇒ identical simulation in both engines. The four previously-known offenders are fixed. What's missing is **proof that stays true**: a regression test suite asserting determinism, and a CI gate preventing regression. This batch is now purely about locking in what already works.

---

## Step 1 — Audit All Remaining Non-Deterministic Sources in Core

**Goal:** Produce a complete inventory of every `System.Random`, `Guid.NewGuid()`, `DateTime.Now` (used for seed derivation), and `Environment.TickCount` usage within `Assets/Ashfall.Core/`. Confirm no new offenders have been introduced since the last sweep.

**Implementation:**
- Grep `Assets/Ashfall.Core/` recursively for:
  - `System.Random` (including `new Random`)
  - `Guid.NewGuid`
  - `DateTime.Now` / `DateTime.UtcNow` used as seed material
  - `Environment.TickCount`
  - `Random()` constructor calls without explicit seed from `ISeededRng`
- Cross-reference against the four previously-known offenders (now fixed, see Review Notes) — expect zero live hits, only doc-comment mentions.
- Document any NEW offenders not previously catalogued. If the audit finds any, this batch's scope expands to fix them (treat as a stop-and-report condition, not silent scope creep).
- Record the exact line, file, and usage context for each hit.

**Verification:**
```bash
grep -rn "System\.Random\|new Random\|Guid\.NewGuid\|DateTime\.Now\|DateTime\.UtcNow\|Environment\.TickCount" Assets/Ashfall.Core/ --include="*.cs"
```
Expected result as of this writing: matches only inside doc comments (`DiseaseSystem.cs`, `SilentFoundrySystem.cs`, `HostDefaults.cs`, `ProceduralItemInstance.cs`, `SkillProgressionSystem.cs`, `UtilityAiSystem.cs`) — no executable statement.

**Done when:** A markdown table of all non-deterministic sources exists (`docs/determinism-audit.md`), with status (known-fixed/new/comment-only) for every grep hit. If any hit is a live executable offender, do not close this step — open a fix step following the same pattern as the (now-removed) original Steps 2–4 and get explicit sign-off before proceeding to Step 2 below.

---

## Step 2 — Verify ProceduralItemInstance Guid Fix Is Complete (Confirmation Pass)

**Goal:** Confirm the documented fix at `ProceduralItemInstance.cs` (FNV-1a based `MakeInstanceId`, lines 46-63) is complete and that no other Core code path constructs an instance ID via `Guid.NewGuid()` or `string.GetHashCode()`.

**Implementation:**
- Read `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` in full (already done for this review — confirmed: `MakeInstanceId` combines FNV-1a over `itemId` with an `Interlocked.Increment`-based per-process counter, explicitly documents "No Guid.NewGuid, no string.GetHashCode").
- Search for any remaining `Guid.NewGuid()` in the file and in all files that construct `ProceduralItemInstance`.
- Search for `Guid.NewGuid()` across ALL of `Assets/Ashfall.Core/` — any hit is a new offender (Step 1's audit should already surface this).
- Confirm the generated 8-hex-char IDs are:
  - Deterministic for a given seed + call order (the counter is process-local, not RNG-seeded — note this explicitly: `InstanceId` uniqueness depends on construction *order*, not on `ISeededRng`, so cross-host determinism requires both hosts to construct items in the same order for the same seed. This is a real, subtle distinction the original plan's Step 5 did not surface — flag it in the audit doc rather than assume it's a non-issue.)
  - Not relied upon as a cross-save-boundary stable identity beyond what `CaptureState`/`RestoreState` already round-trips (confirm no code re-derives or re-validates `InstanceId` format on load in a way that would break if the counter resets on process restart).

**Verification:**
```bash
grep -rn "Guid\.NewGuid" Assets/Ashfall.Core/ --include="*.cs"  # Must return 0 hits
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

**Done when:** Zero live `Guid.NewGuid()` call sites exist in `Assets/Ashfall.Core/` (confirmed); the counter-reset-on-restart caveat is documented in the audit doc; existing item-related tests pass.

---

## Step 3 — Add Determinism Regression Test (Same Seed = Same Output)

**Goal:** Create a test class that proves all four previously-fixed systems produce identical output when given the same seed, across multiple runs — this is the missing proof, since no regression test currently pins this behavior.

**Implementation:**
- Create `Ashfall.Core.Tests/DeterminismRegressionTests.cs`.
- For `FinalWishSystem` and `CombatTraumaSystem`: both take `ISeededRng Rng` as a plain public field (not a constructor parameter — confirm actual injection pattern by reading the class before writing the test; do not assume constructor injection). Construct with `new SeededRng(42)` (the concrete type in `HostDefaults.cs`; `ISeededRng` itself has no factory), assign to `.Rng`, run N operations that exercise the RNG-dependent method (`FinalWishSystem` uses it inside prognosis-days calculation; `CombatTraumaSystem` inside the false-alarm-chance roll — read both call sites first, they are gated behind `Rng?.NextDouble() ?? 0.5`, i.e. a null `Rng` silently falls back to 0.5, which is itself worth a dedicated test case: "no Rng assigned → deterministic fallback, not a crash, not System.Random").
- For `WeatherSystem`: identify the actual reseed-per-roll constructor/method signature by reading the class (the doc comment says "each roll reseeds fresh from seed + rollCount" — confirm the exact parameter names before writing assertions).
- For `ProceduralItemInstance`: note from Step 2 that `MakeInstanceId` depends on a process-static `Interlocked.Increment` counter, NOT on `ISeededRng`. A "same seed → same output" test for this class must reset or control that static counter (e.g. via reflection or a test-only reset hook) or it will be inherently order-dependent and flaky across parallel test runs. Flag this to the user as a design gap if no reset mechanism exists — do not paper over it with `[Collection]` test-ordering hacks alone.
- Assert outputs are byte-identical / value-identical (not "close enough" float comparisons — use exact equality for hashed/string IDs, and document the tolerance if any float comparison is unavoidable).
- Add a combined integration test wiring all systems with the same master seed, simulating N in-game days, hashing combined output against a golden value — only after the per-system tests above are stable, since a golden hash test that fails gives no actionable signal about which system regressed.
- Test naming convention: `[Fact] public void FinalWishSystem_SameSeed_ProducesIdenticalOutput()`

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~DeterminismRegression"
```

**Done when:** All determinism regression tests pass; running them 10 times in a row (including in parallel, since xUnit parallelizes test classes by default) produces no flakiness; the `ProceduralItemInstance` counter-reset gap from Step 2 is either resolved or explicitly documented as a known limitation; the golden hash is stable.

---

## Step 4 — Add CI Gate for New System.Random/Guid.NewGuid() Introduction

**Goal:** Prevent future regressions by adding an automated check that fails if `System.Random` or `Guid.NewGuid()` appears in `Assets/Ashfall.Core/`.

**Implementation:**
- Create `Ashfall.Core.Tests/NoDeterminismViolationsTests.cs` (or add to existing `DataRuleComplianceTests.cs`).
- The test scans all `.cs` files under `Assets/Ashfall.Core/`:
  - Reject any line containing `new Random(` or `System.Random` (excluding comments that document the fix).
  - Reject any line containing `Guid.NewGuid()` (excluding comments).
  - Reject `DateTime.Now` / `DateTime.UtcNow` used outside of logging.
- Allowlist: lines containing `// DETERMINISM-EXEMPT:` with a justification (for edge cases like build-time tooling).
- Optionally add a shell script `scripts/ci/no-system-random.sh` for pre-commit hook use:
  ```bash
  if grep -rn --include="*.cs" "new Random\|System\.Random\|Guid\.NewGuid" Assets/Ashfall.Core/ | grep -v "// DETERMINISM-EXEMPT" | grep -v "^.*//"; then
    echo "FAIL: Non-deterministic source found in Core"
    exit 1
  fi
  ```
- Wire into the existing CI pipeline (runs as part of `dotnet test`).

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~NoDeterminismViolations"
```

Intentionally introduce a `new Random()` in a test file under Core, confirm the gate catches it, then revert.

**Done when:** The CI gate test exists and passes; intentionally introducing `System.Random` in Core causes a test failure; the gate is documented in AGENTS.md known-issues as resolved.

---

## Summary

| Step | System / Area | Action | Risk | Estimated Effort |
|------|---------------|--------|------|-----------------|
| 1 | All of Core | Audit non-deterministic sources (confirm zero live offenders) | None | 30 min |
| 2 | ProceduralItemInstance | Confirm Guid fix completeness + document counter-reset caveat | Low | 30–45 min |
| 3 | Tests | Determinism regression test suite (the actual missing work) | Low | 3–4 hr (higher than original 2-3hr estimate due to the counter-reset and Rng-nullability edge cases above) |
| 4 | CI / Tests | Gate against future violations | Low | 1 hr |

**Total estimated effort:** 5–6.25 hours (down from the original 7–11 hours — three of the four original "fix" steps were unnecessary because the code is already fixed)
**Invariants enforced:** #4 (Determinism), #1 (Zero engine coupling — ISeededRng is Core-only)
**Rollback:** All work in this batch is additive (new test file, new CI check, new audit doc). If Step 4's CI gate produces false positives against legitimate doc-comment mentions of `System.Random`/`Guid.NewGuid`, relax the gate's exclusion regex rather than reverting — no production code is touched, so there is nothing else to roll back.
**Verification command after all steps:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
grep -rn "System\.Random\|Guid\.NewGuid" Assets/Ashfall.Core/ --include="*.cs" | grep -v "// DETERMINISM-EXEMPT" | grep -v "^.*\/\/"
```

**Next prompt after completion:** "Run the full verification checklist and confirm Invariant 4 holds across all Core systems."

---

## Review Notes (Corrected)

This batch was adversarially reviewed against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. Findings:

1. **The batch's entire "fix" premise (original Steps 2–4) was false.** AGENTS.md's "Known offenders" list under Invariant 4 is stale documentation, not a current defect list. All four cited call sites were checked by direct file read and grep:
   - `FinalWishSystem.cs:66` → `public ISeededRng Rng;` (confirmed fixed).
   - `CombatTraumaSystem.cs:53` → `public ISeededRng Rng;` (confirmed fixed).
   - `WeatherSystem.cs:144` → no `Random` construction; class doc (lines 44-49) documents the seed+rollCount reseed strategy (confirmed fixed).
   - `ProceduralItemInstance.cs` → `MakeInstanceId` (lines 46-63) uses FNV-1a + `Interlocked.Increment` counter, explicitly documented as avoiding `Guid.NewGuid`/`GetHashCode` (confirmed fixed).
   - A full-tree grep for `System\.Random|new Random\(|Guid\.NewGuid` under `Assets/Ashfall.Core/` returns only documentation-comment hits (`DiseaseSystem.cs`, `SilentFoundrySystem.cs`, `HostDefaults.cs`, `ProceduralItemInstance.cs`, `SkillProgressionSystem.cs`, `UtilityAiSystem.cs`), zero executable statements.
   - AGENTS.md should itself be updated to move this off the "Known offenders" list — that is a documentation fix outside this batch's scope but worth flagging to the user.
2. **The original plan invented non-existent `ISeededRng` API members.** `rng.NextInt(max)`, `rng.NextInt(min, max)`, `ISeededRng.Fork()`, and `ISeededRng.DeriveChild(string)` do not exist anywhere in `Assets/Ashfall.Core/Ports.cs`. The real interface only has `Next(int minInclusive, int maxExclusive)`, `NextFloat()`, `NextDouble()`, and `Seed`. Any implementation following the original plan verbatim would not compile.
3. **Renumbered steps.** Original Steps 2, 3, 4 (fix FinalWishSystem/CombatTraumaSystem/WeatherSystem) are deleted entirely — there is no work left to do. Original Step 5 (verify Guid fix) becomes new Step 2, re-scoped as a confirmation pass with an added caveat about the process-static counter used for instance-ID uniqueness (this is an "edge-case" flag, not a defect — the counter is deterministic within a single process run but resets across process restarts, which the original plan did not consider or mention). Original Steps 6, 7 become new Steps 3, 4, tightened to use real method names and to flag concrete flakiness risks (xUnit's default test-class parallelism; the RNG-nullability fallback path in both systems).
4. **Priority and risk downgraded.** Original: Priority HIGH, Risk Medium ("behavioral changes to randomness patterns"). Corrected: Priority LOW-MEDIUM, Risk Low — there is no behavior to change, only tests and a CI gate to add. Estimated effort dropped from 7–11 hours to 5–6.25 hours.
5. **Done-when criteria tightened.** Step 1's original "done when" accepted any audit table with no escalation path if new offenders were found; corrected version requires an explicit stop-and-report/fix-and-get-sign-off path rather than silently expanding scope. Step 3 (regression tests) now requires resolving or explicitly documenting the `ProceduralItemInstance` counter-reset flakiness risk before being considered complete, rather than allowing a green test suite that is secretly order-dependent.
6. **`--next prompt--` and verification commands were already runnable and correct** (grep-based, no missing tool dependencies) and are retained unchanged.


---

## Review Notes (Corrected) — Second Pass

A second independent adversarial pass re-verified every claim in this document against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`, deliberately not trusting the first pass's own "Review Notes" section without re-checking the underlying files directly. Findings:

1. **All four "already fixed" citations re-confirmed by direct file read, exact line matches:**
   - `Assets/Ashfall.Core/Survivors/FinalWishSystem.cs:66` → `public ISeededRng Rng;` — confirmed exact line.
   - `Assets/Ashfall.Core/Survivors/CombatTraumaSystem.cs:53` → `public ISeededRng Rng;` — confirmed exact line.
   - `Assets/Ashfall.Core/World/WeatherSystem.cs:144` → `var rng = new SeededRng(unchecked(_seed * 397 + _state.rollCount));` — confirmed exact line; this constructs the deterministic `SeededRng` (xorshift64*/SplitMix64, verified by reading `HostDefaults.cs:96-131` — internal state is a single `ulong`, no wrapped `System.Random` anywhere in its body), not `System.Random`, despite superficially similar `new X(...)` shape. Also confirmed the class doc-comment at lines 44-49 states the reseed-per-roll strategy exactly as cited. A second, identical reseed call also exists at `WeatherSystem.cs:282` (a forward-simulation preview path) — not previously mentioned by either pass; noted here for completeness, does not change any conclusion since it uses the same deterministic `SeededRng` constructor.
   - `Assets/Ashfall.Core/Inventory/ProceduralItemInstance.cs` → `MakeInstanceId` re-confirmed at exactly lines 46-63 (doc comment 46-50, method signature 51, body 52-62, closing brace 63) — the first pass's citation was precisely correct; this second pass's own initial line-count estimate (43-58) was the inaccurate one and has been discarded rather than applied as a "fix." Uses FNV-1a hashing combined with `System.Threading.Interlocked.Increment(ref _instanceCounter)` — confirmed no `Guid.NewGuid()` call anywhere in the file.
   - **No correction needed** — verified the existing "lines 46-63" citation in Step 2 is exact and left it unchanged.
2. **Repo-wide grep re-run independently, exact match with both prior passes' claimed result.** `grep -rn "System\.Random\|new Random\|Guid\.NewGuid\|DateTime\.Now\|DateTime\.UtcNow\|Environment\.TickCount" Assets/Ashfall.Core/ --include="*.cs"` returns exactly 7 hits, all inside doc comments, across `ProceduralItemInstance.cs`, `SkillProgressionSystem.cs`, `UtilityAI/UtilityAiSystem.cs`, `Foundry/SilentFoundrySystem.cs`, `Disease/DiseaseSystem.cs`, `HostDefaults.cs`, and `Ports.cs` (this last file — the `IClock` "Never DateTime.Now" doc comment — was not individually named by either prior pass's file list but is part of the same true zero-live-offenders result). Zero executable statements. This independently confirms the batch's central "already fixed" premise.
3. **`ISeededRng` interface contents re-confirmed exact.** Direct read of `Assets/Ashfall.Core/Ports.cs` (49 lines total) shows exactly: `Seed { get; }`, `Next(int minInclusive, int maxExclusive)`, `NextFloat()`, `NextDouble()`. No `NextInt`, `Fork()`, or `DeriveChild(string)` exist. The batch's warning against inventing these members is correct and necessary — confirmed by reading the actual interface, not by trusting the file's own prior claim.
4. **`Rng?.NextDouble() ?? 0.5` null-fallback pattern re-confirmed exact in both systems** — `FinalWishSystem.cs:100`, `CombatTraumaSystem.cs:169`. Step 3's guidance to test this fallback path explicitly is well-founded and unchanged.
5. **CLI verbs re-confirmed real and correctly named.** `--bridge-selftest`, `--data-integrity-selftest` both exist in `src/Host/HostCli.cs` exactly as the wider verification checklist (outside this batch) assumes; this batch itself does not invoke either directly in its own verification commands, which was already correct.
6. **No line-range correction needed after re-verification.** Initially suspected the Step 2 citation of `MakeInstanceId` at "lines 46-63" might be off, but a precise re-read (doc comment 46-50, signature 51, body 52-62, closing brace 63) confirmed the existing citation is exact. Left unchanged.
7. **No other factual, ordering, or scope defects found on this second pass.** The renumbered step structure (audit → confirm Guid fix → regression tests → CI gate), the deleted original fix-steps, the effort re-estimate, and the risk/rollback language were all independently judged sound and internally consistent with the verified facts above. No edits to the body of the document were needed — this batch's prior review pass held up under independent re-verification.
