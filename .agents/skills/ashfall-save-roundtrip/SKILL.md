---
name: ashfall-save-roundtrip
description: Audits one ASHFALL stateful system or save store for capture/restore fidelity, serializer parity, and checksum behavior. Use when adding save coverage or investigating state loss; complements ashfall-save-fuzz and ashfall-test-gap rather than replacing them.
---

# ASHFALL Save Round-Trip Verifier

## Role

Prove that one stateful system preserves player-relevant state across
`CaptureState`, serialization, deserialization, and `RestoreState`. Work from
current source; `REPO_REVIEW_REPORT.md` and older plans are leads, not proof.

## Boundary

- Use `ashfall-test-gap` to find a broad coverage backlog.
- Use `ashfall-save-fuzz` for a repository-wide adversarial battery.
- Use this skill for one named system/store and its focused tests.
- Do not invent a generic reflection-based test generator. Constructors,
  fixtures, and invariants are domain-specific and must be inspected first.

## Workflow

1. Identify the system, state DTO, codec/store, serializer, existing tests,
   and any existing fixture. Fixture construction belongs to
   `ashfall-test-fixture`; do not duplicate it here.
2. Record the baseline test command and any pre-existing failure.
3. Reuse a meaningful existing fixture. If none exists, describe the missing
   fixture contract and hand setup to `ashfall-test-fixture` rather than
   guessing at constructors or data.
4. Test capture/restore parity after a serialize/deserialize boundary. Assert
   behavior or explicit fields, not object-reference equality.
5. Test snapshot isolation if `CaptureState` exposes mutable collections.
6. For an envelope, test valid checksum, one-field mutation rejection, and
   missing checksum rejection for the new format. Test legacy bare-state load
   only when that fallback is an existing contract.
7. Test version migration or future-version rejection when the codec supports
   versions. Do not create migration cases unsupported by the current codec.
8. By default, report missing coverage without editing tests. Add focused xUnit
   tests only when the user explicitly authorizes test edits. Production changes
   belong in Core and require `ashfall-repair` or `ashfall-implement`.

## Rules

- Core remains engine-agnostic; use `IJsonSerializer`, never `JsonUtility`.
- Use fixed seeds and invariant comparisons where a system is stochastic.
- Never weaken validation to make a round-trip pass.
- Do not edit save data, production code, or unrelated tests merely to remove a
  finding.

## Output

Report the system, state fields exercised with file/line evidence, format/version
cases, baseline, tests added (if authorized), exact failures, and commands run. Use
`docs/saves/ROUNDTRIP_<system>.md` only when a report is requested; do not
overwrite an existing report without checking its ownership.

## Quality gate

- Focused tests pass and the full `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` result is reported.
- No missing persisted field is silently treated as covered.
- Any production defect is a finding with file/line evidence, not a hidden fix.
