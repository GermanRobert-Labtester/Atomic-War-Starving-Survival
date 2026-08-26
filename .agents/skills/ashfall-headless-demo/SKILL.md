---
name: ashfall-headless-demo
description: Designs and, when explicitly requested, adds a minimal ASHFALL Core or Godot headless smoke path for one system using existing CLI and demo conventions. Use for integration coverage; never invent a new self-test verb by default.
---

# ASHFALL Headless Smoke Scaffolder

## Boundary

Existing `*HeadlessDemo`, `HostCli`, panel tests, and self-test verbs are the
source of truth. This skill fills a verified gap; it does not create a second
runner or duplicate a test already reachable through an existing verb. It is
read-only by default; implementation requires explicit authorization and then
delegation to `ashfall-implement` for production wiring.

## Workflow

1. Identify the named system and search for an existing demo, xUnit test,
   `HostCli` dispatch path, and self-test coverage.
2. If coverage exists, report it and extend the existing path only if the user
   explicitly asks. Do not scaffold duplicate coverage.
3. Define a short deterministic scenario with explicit setup, action, expected
   state transition, and failure message. Use real Core ports and canonical
   data IDs.
4. Prefer a Core `Run()` demo plus xUnit assertion when the system is engine-
   agnostic. Use a Godot CLI path only when host wiring or rendering is the
   behavior under test.
5. If implementation is authorized, hand the smallest existing-path change to
   `ashfall-implement`; this skill does not directly implement Core, host, test,
   or CLI changes. Add a new CLI verb only with explicit approval and after
   checking argument parsing, help output, CI gates, and collision risk.
   Otherwise attach to an existing appropriate self-test.
6. Run focused tests when a Core test is appropriate, the relevant existing
   headless verb, and the canonical
   project checks. Inspect stdout for real PASS/FAIL evidence, not exit code alone.

## Rules

- No Unity tools or `.unity` scene loading.
- No gameplay logic in the host demo; keep it in Core.
- Fixed seed, no wall-clock input, no random GUIDs.
- A smoke demo is not a replacement for behavior, save, or determinism tests.

## Output

Return the existing coverage search with file/line citations, scenario contract,
files changed (if any), CLI wiring decision with dispatch/help evidence, stdout
evidence, and remaining test gaps. If a report is requested, use
`docs/headless/DEMO_<system>.md`.

## Quality gate

- The demo proves at least one meaningful state transition.
- It is reachable through a documented existing or approved self-test path.
- Core demos have a matching xUnit test; host/rendering-only demos have an
  appropriate Godot self-test. Do not invent an xUnit seam for presentation.
