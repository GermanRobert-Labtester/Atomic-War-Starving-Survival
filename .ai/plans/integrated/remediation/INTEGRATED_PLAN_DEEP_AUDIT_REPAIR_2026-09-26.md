# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

> **FULLY INTEGRATED 2026-09-26** — user-authorized ("find 4 plans to fully integrate, don't leave as partials"). Archival performed this session. No commit.

> **Final acceptance evidence (this session):** host build `Ashfall.csproj` 0 warnings / 0 errors; `Ashfall.Core.Tests/Tooling/MainTriadDriftGateTests.cs` 7/7; `Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs` 6/6; `Ashfall.Core.Tests/Tooling/UiWave4SourceContractTests.cs` 7/7; `scripts/ci/generate-architecture-map.py` regenerated (309 subsystems) and `--check` OK. Waves 1–5 complete; no residual blockers.
>
> **Real-campaign regression (the plan's named Wave 1 acceptance check) PASS:** `godot --headless -- --real-campaign-journey-selftest` → `SELFTEST PASS: real_campaign_journey_selftest` — New Game → `ComposeCampaign()` → real gameplay actions → day advance → `SaveAll()` → full in-memory reset → Continue → restored composed state (inventory trade, radiation dose round-trip, session rebuild all PASS).

# Deep Audit Repair — Save, Lifecycle, Core, Host, and UI

**STATUS: APPROVED BY USER**
**Date:** 2026-09-26
**Claim:** `claim-deep-audit-repair-2026-09-26`

## Goal

Repair the currently verified save-path, session-reset, weather-gate, Core
diagnostic/determinism, host-safety, and UI lifecycle/style defects described
in the user-approved repair plan. Preserve existing architecture and integrate
through current owners.

## Non-goals

- Do not split god files, merge the two `HostCli` enums, re-enable nullable
  warnings, or change save-checksum canonicalization.
- Do not commit, rewrite history, reset the worktree, or discard staged,
  unstaged, or untracked work.
- Do not run the full test suite. Use scoped tests and the named headless
  probes only, with no more than 15 test steps.
- Do not rebaseline UI snapshots without the documented renderer procedure.

## Execution constraints

- Re-check each target immediately before editing. Use narrow exact-string
  changes and preserve existing staged work.
- Claim each additional target in `WORKTREE_OWNERSHIP.md` before editing it.
- Treat another active edit/build as a coordination boundary; do not overwrite
  its work or infer its result.
- Generated artifacts are changed only by their owning generators.
- Stop and record any golden replay change, ownership conflict, or new
  architecture decision instead of improvising.

## Phases

1. **Wave 0:** record the approved plan, exact ownership, current staged state,
   and build baseline.
2. **Wave 1:** reconcile every registered save filename and method name; reset
   the 11 stale session owners through the existing lifecycle chain; replace
   the modal-travel reflection lookup with typed `WeatherCascadeSeverity`;
   repair architecture-map coverage/gates; remove `AllSaveSections`; add a
   real-campaign fresh-session regression check.
3. **Wave 2:** route catalog parse failures through `CatalogDiagnostics.Warn`;
   preserve one final decode exception in `SaveEnvelopeHelper`; enforce the
   Core catch policy; use invariant culture; fix signed modulo and
   dictionary-order determinism; deduplicate safe clamp helpers and remove
   the dead performance marker.
4. **Wave 3:** centralize save-store contract checks; harden numeric/string
   parsing, null guards, exception reporting, self-test failure exit behavior,
   and event rebind cleanup.
5. **Wave 4:** close panel event leaks; auto-unbind on predelete; fix content
   replacement, rounded corners, backdrop/color token usage, duplicate child
   clearing/action helpers, and long-label wrapping.
6. **Wave 5:** reconcile suppression comments and agent docs, run the owning
   agent-sync command, measure the suppressed-warning backlog, and record
   exact verification and limitations.

## Acceptance

- Every live registered save section has one filename entry matching its
  store constant, and the corrected save-method names pass the triad gate.
- Loading another slot rebuilds the 11 affected sessions; modal flight
  feasibility reads the typed weather owner.
- All changed Core/host/test projects build without errors; the targeted
  gates, scoped tests, required generators, and applicable headless probes
  pass within the 15-step test budget.
- No unintended staged-work changes, checksum changes, or snapshot
  rebaselines are introduced. The final plan is marked fully integrated and
  archived only when its acceptance is met.

## Verification

Use `bin/run-scoped-tests` for focused xUnit targets. Run the specifically
listed host/test builds and the named `godot --headless` probes from the
approved repair request. Record commands, counts, pre-existing failures,
generator `--check` results, and any skipped targets in `.ai/state.md`.
