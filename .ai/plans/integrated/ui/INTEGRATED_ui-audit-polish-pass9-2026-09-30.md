# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 9 (deeper: harden the verification infrastructure)

> **STATUS: APPROVED BY USER** (user directive: "another deeper pass!")

Ninth pass, deliberately aimed **below the product** at the verification
infrastructure itself. The gate that shipped blind in PASS 7 was a systemic
hazard, not a one-off, so the question was: what else in the audit layer can
silently pass while being wrong?

## Two systemic hazards probed

### A. "any-of" blindness in gates — scoped, and correctly so

Scanned every `Ashfall.Core.Tests` gate for `.Any(` deciding pass/fail. All
observed uses check *existence*, which is the right semantic
(`routes.Any(reachable)`, `treatments.Any(IsCurative)`, `findings.Any(code)`).
The PASS-7 bug was different in kind: it used `Any` over a set of **declared
obligations**, where each obligation must hold. **No further instances.**

### B. Generators whose `--check` validates against a stale build — REAL, systemic

`generate-selftest-manifest.py` and `generate-cli-catalog.sh` both query the
**compiled host** (`godot --headless --path . -- --selftest-manifest` /
`--host-help`). If C# is edited without a rebuild they read stale code, and
their `--check` reports **"OK"** — because both sides are equally stale. This
is exactly the failure that silently skipped four aliases in PASS 8.

## Fix at the single seam

Every headless Godot invocation used by verification goes through
`scripts/ci/run-godot-bounded.sh` (verified — see coverage below). A
build-staleness guard was added there:

* Blocks when staleness is **provable**: `.godot/mono/temp/bin/Debug/Ashfall.dll`
  exists **and** is older than the newest `src/**.cs` / `Assets/Ashfall.Core/**.cs`.
* A missing assembly is *not* proof, so it can never false-positive into
  blocking a valid run.
* Emits the exact assembly, the reason, and the fix
  (`dotnet build Ashfall.csproj`).
* Escape hatch: `ASHFALL_SKIP_BUILD_STALENESS=1`.

## Proof the guard works

```
1) current build        → run proceeds        (settings-selftest PASS)
2) touch src/Host/HostCli.cs  (mtime only, no content change)
                          → BLOCKED, exit 2:
    ERROR: compiled host is STALE — C# sources are newer than
           .../.godot/mono/temp/bin/Debug/Ashfall.dll
           This run would execute, and verify, stale code.
    Fix:   dotnet build Ashfall.csproj   # then re-run
3) ASHFALL_SKIP_BUILD_STALENESS=1 → override honoured
4) dotnet build → run proceeds again
```

`touch` only changes mtime, so `git diff` stayed clean throughout.

## Coverage audit — does anything bypass the seam?

Six scripts *mention* `godot` without referencing the wrapper. Inspected all of
them: `generate-catalog-registry.py`, `generate-core-systems-catalog.py`,
`generate-ui-panel-catalog.py`, `sync-agent-rulebooks.py`, `pre-commit` and
`asset-orphan-sweep.sh` are **documentation strings** emitting markdown/help
that names the commands. **None execute Godot.** `input-map-gate.sh` and
`version-gate.py` have no Godot execution at all. So the wrapper really is a
single seam and the guard covers the whole class.

## Verification

```
bin/run-scoped-tests HostCliActionParityGateTests UserSettingsRecoveryTests   2/2 targets
generate-selftest-manifest.py --check   OK (317 tests)
generate-cli-catalog.sh --check         OK (358 entries / 590 flag tokens)
run-godot-bounded.sh: settings / ui-accessibility / ui-layout /
  player-panels / warlord-ui            all PASS, staleBlocked=0
dotnet build Ashfall.csproj             0 errors
git diff --check                        clean
```

## Owned paths (exact)

`scripts/ci/run-godot-bounded.sh` only (29 added lines, purely additive).
No C#, data, test, or generated-output changes in this pass.
