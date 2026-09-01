# Task #132 — PRE-2 Trusted Baseline

Captured after PRE-1 (expedition test-oracle repair) and before any domain
migration. This is the reference point every later Task #132 phase is compared
against. Recorded because the previous baseline was untrustworthy: the
expedition gate printed PASS while crashing.

- **Date:** 2026-08-29
- **Branch:** `feat/workspace-optimization-phase5`
- **HEAD:** `a3b9f2c6`
- **Godot:** 4.7.1.stable.mono
- **Runner:** `python3 scripts/ci/run-gates.py --tier fast --no-fail-fast`

---

## Working-tree contamination (read this first)

The working tree carries **uncommitted Task #131 composition-root work** that is
not mine and must not be committed or modified:

| Path | State |
|---|---|
| `src/Host/CampaignServices.cs` | untracked |
| `src/Main.CampaignServices.cs` | untracked |
| `docs/architecture/setup_inventory.json` | untracked |
| `src/Main.cs` | modified |
| `src/Main.GameFlow.cs` | modified |
| `src/Main.ExpandedShelterSystems.cs` | modified |
| `Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | modified (`<Compile Remove="Performance/*" />`) |

That work throws
`InvalidOperationException: Campaign composition invariant failure: SetupSurvivors() was called before ComposeCampaign().`
and breaks one canonical gate. Proven by running the gate in a clean worktree at
`HEAD`, where it passes.

---

## Canonical 42-gate suite

**39 of 42 pass.** Three fail, none introduced by Task #132.

| Gate | Verdict | Provenance |
|---|---|---|
| `player_panels_uitest` | FAIL | **Task #131 uncommitted WIP.** Passes in a clean worktree at `HEAD`; fails only with the WIP present. |
| `compiler_warning_baseline` | FAIL | **Pre-existing.** Identical failure reproduced at `707bdae6`. |
| `forbidden_core_apis` | FAIL | **Pre-existing.** Identical failure reproduced at `707bdae6`. |

The other 39 pass, including `build_core_tests`, `test_core_suite`
(4845/4845), `build_godot_host`, `data_integrity`, `bridge_removal`,
`triad_drift`, `campaign_envelope_fuzz_test`, and every drift gate.

### Fixed during PRE-2

| Gate | Was | Cause |
|---|---|---|
| `save_store_matrix_drift` | FAIL → PASS | Pre-existing stale generated doc (verified stale at `707bdae6`). Regenerated. |
| `docs_index_drift` | FAIL → PASS | Caused by Task #132 P0 adding a markdown file. Regenerated. |

### Detail on the two remaining pre-existing failures

**`compiler_warning_baseline`** — three unique `CS8602` warnings, each reported
twice by MSBuild (hence the gate's "6 warnings"):

- `Assets/Ashfall.Core/Performance/PerfSession.cs:129`
- `src/Host/HostCli.Onboarding.cs:173`
- `src/UI/SaveLoadPanel.cs:194`

This gate is also **incremental-sensitive**: it runs `dotnet build` without
`-t:Rebuild`, so on an already-built tree it reports 0 warnings and passes, and
on a fresh compile it reports the three real ones and fails. AGENTS.md's claim
that this gate is "0 errors, 0 warnings" only holds for the incremental case.
Any warning claim about this repository must state whether the build was
incremental or a full rebuild.

**`forbidden_core_apis`** — the "Zero Wall-Clock Simulation Drift" check flags
`Assets/Ashfall.Core/IWallClock.cs:26-29`, which is the sanctioned wall-clock
abstraction. `Ashfall.Core.Tests/CoreInvariantSourceTests.cs` deliberately skips
`IWallClock.cs` when checking the same invariant, so the two gates disagree
about their own exemption. One of them is wrong; neither was changed here.

---

## Self-tests absent from the manifest

These are **not registered in `docs/ci/CI_GATE_MANIFEST.json`**, so CI never
runs them. That is why the expedition false-green survived: the gate AGENTS.md
describes as pinning nine vehicle gates was not in the suite at all.

| Verb | Verdict | Exit |
|---|---|---|
| `--survivors-selftest` | PASS | 0 |
| `--expedition-selftest` | PASS (19/19: 10 demo + 9 vehicle) | 0 |
| `--save-store-checksum-selftest` | PASS | 0 |
| `--medical-selftest` | PASS | 0 |
| `--world-selftest` | PASS | 0 |

`--survivors-selftest` and `--expedition-selftest` are the two gates that would
detect a Task #132 survivor or expedition regression. Registering them is
recommended before the domain migrations begin, otherwise the migrations proceed
without CI coverage of the exact areas they change.

---

## Oracle trustworthiness (PRE-1 exit evidence)

Proven by fault injection, then reverted:

| Injected fault | Result |
|---|---|
| Unhandled `throw` outside the gate's own try | `FAIL`, exit 1, no hang, action named, stack retained, no PASS line |
| Unhandled `throw` inside the vehicle gates | `FAIL` naming the stage (`"V5 fuel gate blocks dispatch"`), counted, exit 1 |
| False assertion (`Check(false, ...)`) | `[FAIL] V1` counted, summary FAIL, exit 1 |

Clean state after revert: 19/19 PASS, exit 0.

---

## Reproducing this baseline

```bash
python3 scripts/ci/run-gates.py --tier fast --no-fail-fast --report-json /tmp/gates.json
python3 scripts/ci/run-gates.py --list          # 42 registered gates

# Not in the manifest — run explicitly:
godot --headless --path . -- --survivors-selftest
godot --headless --path . -- --expedition-selftest
```

To reproduce without Task #131 contamination:

```bash
git worktree add --detach /tmp/wt HEAD
cd /tmp/wt && dotnet build Ashfall.csproj && godot --headless --path . --import
godot --headless --path . -- --player-panels-uitest    # passes here
git worktree remove --force /tmp/wt
```

Note that a fresh worktree is a poor environment for the full suite:
`test_core_suite` and `godot_import` exceed their manifest timeouts on a
cold build, and the drift gates regenerate against different state.


---

## Addendum — a fourth false-green mechanism (found during P1-A)

**Godot self-tests silently run a stale binary when the build fails.**

`godot --headless --path . -- --<gate>` loads
`.godot/mono/temp/bin/Debug/Ashfall.dll`. If `dotnet build Ashfall.csproj`
fails, that DLL is simply left at its last successful build, and the gate runs
happily against old code and reports PASS.

Observed directly:

```
host compile errors: 6
stale dll timestamp: 22:41:42
now:                 22:57:25
gate says: [HOST_SELFTEST] bridge_selftest PASS
gate says: [HOST_SELFTEST_SUMMARY] ... status=PASS exit_code=0
```

A gate reporting PASS 16 minutes after the code stopped compiling is not
evidence of anything.

### Consequence for every verification claim

A host self-test result is meaningless unless the build that preceded it
succeeded. Always assert the build first:

```bash
dotnet build Ashfall.csproj --nologo || { echo "BUILD FAILED — gate results are stale"; exit 1; }
godot --headless --path . -- --survivors-selftest
```

`scripts/ci/run-gates.py` happens to be safe here because
`build_godot_host` is gate 5 and runs before the host gates — but only when the
suite is run in full. Running a single host gate with `--gate <id>` does **not**
rebuild first, so a single-gate invocation can report PASS off stale bytes.

### Recommended hardening (not implemented — outside Task #132)

Either make each host gate depend on a successful `build_godot_host`, or have
the host print and the runner verify a build fingerprint. Filed as a validation
finding rather than fixed, because it changes gate infrastructure rather than
survivor code.

---

## Addendum — working tree instability during P1-A

At the start of P1-A the working tree carried seven uncommitted Task #131 files.
Partway through, it carried **51 modified `src/` files (780 insertions, 722
deletions)**, four of which do not compile:

| File | Symptom |
|---|---|
| `src/Main.Phase0.cs` | truncated mid-expression, spliced into `ConstructPhase0()` |
| `src/Main.ShelterBatch3.cs` | truncated mid-expression, spliced into `ConstructKitchenNutrition()` |
| `src/Main.ShelterInfrastructure.cs` | syntax errors |
| `src/Main.ShelterSocial.cs` | syntax errors |

Example corruption:

```csharp
if (CaptureSection("phantom_memory", Phantom        private void ConstructPhase0()
```

The change set is consistent with Task #131's composition-root refactor in
progress (`_Ready()` losing its `SetupJournal()`/`SetupIceRoad()`/
`SetupDutyRoster()`/`SetupExpansions()`/`SetupYearOfAsh()`/`SetupMoralChoice()`
calls as they move into `ComposeCampaign()`), so it appears to be another actor
working the same checkout concurrently rather than disk corruption.

**None of those files were touched by Task #132**, and none were staged or
committed by it. Task #132's own committed work is unaffected: `dotnet test`
remains green at 4857/4857 because Core does not depend on `src/`.

Consequence: no host self-test can be executed or trusted until the host
compiles again. Core-only phases remain verifiable.
