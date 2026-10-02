# PERF PROGRAM — `compiler_warning_baseline` proposal (frequency tiering)

**Status:** `STATUS: AWAITING USER APPROVAL` — analysis only; no gate changed.
**Author:** perf program session, 2026-10-02.

## Why this gate

`scripts/ci/warning-baseline-gate.sh` proves the three C# assemblies compile with
**0 warnings**. It uses `dotnet build -t:Rebuild -m:1 -p:UseSharedCompilation=false`
**deliberately**: an incremental build skips compilation and then emits no
warnings, so `-t:Rebuild` is required to avoid a false pass. This is the single
largest fast-tier gate: **~190 s / 2,273 MB peak** (2026-10-02, load ~15–24).

## Measured attribution (2026-10-02, `-t:Rebuild -m:1 -p:UseSharedCompilation=false`)

| Rebuild | Wall | Peak RSS |
|---|---:|---:|
| `Ashfall.Core.Tests` (71.6 s) | 1:11.64 | 1,207 MB |
| `Ashfall.Core` (31.8 s) | 0:31.77 | 1,083 MB |
| `Ashfall.csproj` host (84.9 s) | 1:24.91 | 2,322 MB |
| **Total** | **~188 s** | **2,322 MB (host)** |

## Why it is not safely reducible

- **Each rebuild is necessary.** `-t:Rebuild` on a project rebuilds *that* project;
  its project references are only built (incrementally), so a dependency's full
  recompile — and therefore its warnings — would be lost. Dropping the separate
  `Ashfall.Core` build would silently stop catching Core warnings → false pass.
- **Parallelising the three is unsafe on this host.** Peaks are 1.2 + 1.1 + 2.3 GiB;
  the machine has ~2.2 GiB free. Running them concurrently would swap/OOM — and the
  gate comment already keeps `-m:1` for exactly this reason.
- **`-m:1`/`UseSharedCompilation=false` are deliberate** (cold-runner timeout safety).

Conclusion: the gate's *cost* is intrinsic on this hardware. The only honest lever
is **frequency**, not duration.

## Proposal — tier the gate by change, not add it to every fast sweep

1. **Run `compiler_warning_baseline` only when C# or build inputs change**
   (`*.cs`, `*.csproj`, `Directory.Build.props/.targets`), skipped on doc/data/asset-only
   sweeps. Warnings cannot change when no C# compiled.
2. **Move it off the per-PR fast path onto the merge-to-main / nightly tier**
   (same shape the program already prescribes for `test_core_suite`), keeping it
   required on merge to `main` and in nightly.
3. Keep the in-place gate exactly as-is elsewhere — the false-pass property is
   preserved because nothing about the rebuild flags changes; only *when* it runs.

**Expected saving:** up to **~190 s per doc/data-only sweep** (the sweep that does
not touch C#), and off the per-PR critical path entirely.

## False-pass proof obligation (must be stated before any change)

Any implementation must demonstrate, on a synthetic warning, that the gate still
fails — i.e. that the change-based skip cannot hide a real new warning on a run
that did touch C#. The skip predicate must be a **superset** of every input that
can affect compiler output.

## Approval requested

- Implement the change-based skip and/or the tier move in `run-gates.py` /
  the CI workflows, with the false-pass proof above.
- No change to the gate's rebuild flags or its 0-warning assertion.