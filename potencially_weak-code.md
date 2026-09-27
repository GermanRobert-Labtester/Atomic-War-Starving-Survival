# Potentially Weak Code

## Journey Diagnostics self-test JSON handling (2026-09-26)

- **Finding:** `src/Host/HostCli.JourneyDiagnostics.cs` previously used an
  empty `catch` while parsing `FailureJson`. That discarded parser details and
  made malformed JSON indistinguishable from a missing or incorrect `status`.
- **Fix:** catch only `JsonException`, retain its message, validate `status`
  with `TryGetProperty`, and print a specific failure. The self-test still
  returns nonzero unless all six checks pass.
- **Potential weakness:** this probe validates only that the top-level
  `status` is the string `FAILED`; it does not validate the remaining failure
  payload schema. Other unexpected exceptions still reach the outer probe
  handler, which reports failure but skips later checks.
- **Scope:** comment/reporting and probe failure visibility only. No campaign
  behavior or authored data changes.
- **Verification:** `bin/run-scoped-tests
  Ashfall.Core.Tests/Tooling/CatchPolicyLintGateTests.cs` passed 3/3;
  `godot --headless --path . -- --journey-diagnostics-selftest` passed 6/6.
