# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# ASHFALL — 15 Small Repo Follow-ups (2026-10-02)

> **STATUS: APPROVED BY USER** — user directed implementation of all 15 follow-ups.

## Outcome

Close the 15 follow-ups listed in the prior assistant response. Reuse existing
contracts and evidence where a proposed task is already satisfied; implement only
confirmed gaps. Preserve all pre-existing dirty worktree changes.

## Non-goals

- Do not reopen closed Plan 24 production architecture or duplicate `NeedsSystem`.
- Do not create a second content registry, save owner, audio catalog, focus system,
  or panel router.
- Do not run the full test suite or commit changes.
- Do not alter authored gameplay data except the missing localized tooltip row.

## Task list and acceptance

1. Add `ui.expedition.railway_tooltip` to the existing EN/DE localization authority;
   localization gates and accessibility probe pass.
2. Regenerate and check `docs/INDEX.md` after all document edits settle.
3. Harden capability-cluster receipt validation to check the receipt heading and
   the referenced plan ID; its self-test covers valid and invalid receipts.
4. Run `--food-loop-selftest` and record the result in `NEXT-PLAN-215.md`.
5. Recheck the Plan 24 candidate against the current modifier stack, performance
   bridge, and duty consumers; update only its receipt if evidence supports it.
6. Make `RetentionDayOwner` retry-safe for its mutable audit state and prove that
   a failed same-day retry does not double-apply the retention pass count.
7. Verify one dormant content candidate remains correctly dispositioned with
   owner, rationale, and expiry; do not alter it without new consumer evidence.
8. Verify the existing `NeedsSystem` save round-trip coverage.
9. Verify the existing Journal save round-trip coverage.
10. Verify an existing seeded save-resume parity journey.
11. Verify combat audio cue/catalog/bridge parity using the existing audio owner.
12. Extract `[SCENE_BIND_REPORT]` rows into a dedicated CI report artifact using
    the existing fast-verification output and artifact upload.
13. Verify one concrete panel's keyboard focus open/trap/close-return path and pin
    any missing contract in its existing focused test. Initialize the existing
    expedition probe's survivor fixture when the runtime check exposes it as
    absent, so focus and encounter assertions are actually reached. Guard the
    registry route's deferred focus callback against a panel already removed
    during smoke-test teardown.
14. Verify the existing catalog boundary validator and its negative cases.
15. Extend the existing player-panel smoke to prove one registry-routed panel and
    a real action consequence through the current owner.

## Done means

- Every row above is either implemented or shown already satisfied by current
  source plus its focused verification; no stale suggestion is counted as a new
  change.
- Retention retry preserves pass count and owner state after fault rollback.
- Focused checks and host build pass; localization and capability gates pass.
  The docs index was regenerated and its final `--check` passed for 5,624 files.
- Food-loop, retention, audio, and player-panel bounded probes pass at 15 FPS.
  The expedition-panel assertions pass after fixing its missing fresh survivor
  fixture, but Godot exceeded the 180-second wrapper cap during shutdown (exit 124).

## Verification details

- `bin/run-scoped-tests` passed Retention 5/5, Accessibility 8/8 and later 9/9,
  LocalizationRatchet 2/2, Plan24JourneyParity 3/3, and
  ContentReachabilityDisposition 5/5.
- Host build: `dotnet build Ashfall.csproj --no-restore --verbosity:quiet -m:1
  /p:UseSharedCompilation=true /p:DebugType=None /p:DebugSymbols=false
  /p:Optimize=true` — 0 warnings, 0 errors.
- `scripts/ci/l10n_drift_gate.py` passed; capability-cluster `--self-test` and
  `--check` passed; docs index generation and `--check` passed for 5,624 documents.
- Bounded probes: food loop PASS, retention 19/19, audio 649/649, player panels
  PASS. Expedition-panel emitted PASS for all assertions after fixture repair but
  wrapper exit was 124 at the 180-second shutdown cap.
- Existing Needs, Journal, and catalog test sources were inspected and already
  cover save round trips/checksum, journal restore, and malformed/unknown catalog
  references. The scoped runner did not schedule these unchanged test files.
- CI artifact extraction is configured in `.github/workflows/ci.yml`; no remote
  CI job was run locally. No full suite or commit.
