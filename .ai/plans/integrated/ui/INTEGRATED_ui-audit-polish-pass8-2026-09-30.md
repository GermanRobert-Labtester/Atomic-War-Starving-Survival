# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
>
> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# UI AUDIT POLISH — PASS 8 (close the parity-gate coverage hole)

> **STATUS: APPROVED BY USER** (user directive: "another pass!")

Eighth bounded pass. Targets the **one item left open at the end of PASS 7**:
`HostCliActionParityGateTests` was green both before and after the alias drift
was fixed, so its alias↔Parse coverage had a demonstrable hole.

## Root cause of the gate's blindness

`EveryManifestFlag_IsParsedByHostCli` used **`Any`**:

```csharp
if (!flags.Any(f => parseBody.Contains("\"" + f + "\"")))
    unparsed.Add(...);
```

An entry passed as long as **one** flag in its set parsed. For
`ui_accessibility_selftest` the primary `--ui-accessibility-selftest` parsed, so
the registered-but-dead `--ui-a11y-selftest` was never reported. The gate could
not fail by construction.

## Fix — both directions

1. **`Any` → per-flag.** Every declared flag must parse, and the failure names
   the exact `test_id -> flag` pair.
2. **New reverse gate `EveryParsedProbeFlag_IsDeclaredInTheManifest`.** Every
   probe-shaped flag literal (`--*selftest|uitest|selfcheck|ui-test`) that
   `HostCli.Parse` accepts must be declared in the manifest. Scoped to
   probe-shaped flags so runtime flags like `--headless` stay out of scope.

## The tightened gate immediately found 4 more real instances

```
--outposts-selftest        (paired with --outpost-settlement-selftest)
--port-contracts-selftest  (paired with --port-contract-selftest)
--the-network-selftest     (paired with --informant-network-selftest)
--the-underneath-selftest  (paired with --subsidence-selftest)
```

All four were **parsed and advertised in `--host-help`, but absent from the
registry alias arrays** — so the generated `SELFTEST_MANIFEST.json` omitted
them. Same drift class as `--ui-a11y-selftest`. Fixed by declaring the aliases
in their existing `HostCliActionDescriptor`s and regenerating.

## Proof the gate works (TDD verification)

The gate was validated against the defect it is meant to catch, not merely
observed to be green. Temporarily reverting the PASS-7 parse fix:

```
[FAIL] EveryManifestFlag_IsParsedByHostCli
  Error Message:
  ui_accessibility_selftest -> --ui-a11y-selftest
```

It names exactly the flag that had silently shipped dead. Fix restored, gate
green again.

## Generator pitfall recorded (would have silently skipped the fix)

The first regeneration appeared to succeed and reported "OK", but the four new
aliases were **absent** from the manifest. Cause: `generate-selftest-manifest.py`
shells out to the **built** host, and I regenerated before rebuilding after
editing `HostCliRegistry.cs`. Rebuilding first made them land. Worth knowing:
`--check` cannot detect this, because both sides were equally stale.

## Verification

```
Ashfall.Core.Tests/Tooling/HostCliActionParityGateTests   5/5 PASS (was 4)
Ashfall.Core.Tests/HostCliHelpContractTests               2/2 PASS
generate-selftest-manifest.py --check   OK (317 tests)
generate-cli-catalog.sh --check         OK (358 entries / 590 flag tokens)
--outposts-selftest / --port-contracts-selftest /
  --the-network-selftest / --the-underneath-selftest       all run (was: 3 undocumented)
--ui-a11y-selftest                                          runs (PASS 7)
dotnet build Ashfall.csproj          0 errors
git diff --check                     clean
```

## Owned paths (exact)

`Ashfall.Core.Tests/Tooling/HostCliActionParityGateTests.cs` (gate logic only),
`Assets/Ashfall.Core/HostCliRegistry.cs` (4 alias arrays), regenerated
`docs/ci/SELFTEST_MANIFEST.json` + `docs/cli/HOST_CLI_COMMAND_CATALOG.md`.
`src/Host/HostCli.cs` touched only transiently for the TDD proof and restored
byte-identically.
