# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Expedition follow-up wave 6 — radar phase/rail l10n + 5 Core guards + UI-test header smoke

> **STATUS: APPROVED BY USER**

User-directed ("Continue with these small tasks completely finish all of them … 3
loops … then suggest 15 very small tasks"). Source: the 15 immediately-integrable
tasks listed after the wave-5 report in `.ai/state.md`.

## Bounded outcome

Finish all 15 tasks, then run 3 find→repair→harden loops, then propose the next
15 immediately-integrable tasks. No new authority, no save section, no
gameplay-state change; presentation + pure read-model tests only.

## The 15 tasks

1. Localize radar phase cell values (Outbound/Looting/Inbound/Completed/Failed).
2. Localize radar status-rail card labels (Active/Queued/Blocked/Median/Max Danger/Encounter %).
3. Localize the radar shell title.
4. Core test: `GradeRisk(null, null)` returns Low.
5. Core test: `ChecklistLine` marks advisory rows with `(adv)`.
6. Core test: `BuildCeremony` floors a zero-quantity loot row to 1.
7. Localize the `ExpeditionPanel._statusSummary` description.
8. Localize the "Press [Esc] to return" hint.
9. Localize "No active scavenging sorties currently deployed.".
10. Localize the `[ONE-TIME]` badge.
11. Core test: `GetLatestEventOfTypePrefix` survivor-id case-insensitivity.
12. Core test: `ExpeditionInjuryDigest.Build` skips blank survivor ids.
13. Localize the FITNESS: UNFIT/FIT/IMPAIRED states.
14. Add a German-locale smoke assertion to `--expedition-panel-uitest` for a localized header.
15. Add a `CURRENT_AUTHORITY.md` row for the PanelRegistry expedition routes.

## Exact files

- `src/UI/ExpeditionPanel.cs`
- `src/UI/ExpeditionRadarPanel.cs`
- `src/UI/AshfallUiHelpers.cs` (one shared phase formatter)
- `src/Main.UiTests.Expeditions.cs`
- `assets/l10n/strings.csv`
- `Ashfall.Core.Tests/Expeditions/ExpeditionPrepPlanTests.cs`
- `Ashfall.Core.Tests/Localization/ExpeditionLocaleKeysTests.cs`
- `docs/CURRENT_AUTHORITY.md`
- `.ai/state.md`, `INTEGRATION_PLANS.md`, `WORKTREE_OWNERSHIP.md`, this plan

## Non-goals

- No new save section, no mutable state, no gameplay decision change.
- No new panel, no new authority, no Unity dependency.
- No full test suite. The original no-commit boundary was superseded by the
  user's explicit request to commit today's completed work after verification.

## Acceptance

Host build 0/0; `ExpeditionPrepPlanTests` 50/50 (incl. edge guards);
`ExpeditionLocaleKeysTests` 2/2 with the new keys pinned;
`l10n_drift_gate` PASS (888 keys, German parity);
`--expedition-panel-uitest` PASS including the German header smoke and patrol
modal behavior, exiting in about 60 seconds under the 180-second cap.

## Bounded-runtime closeout (2026-10-02)

The former UI smoke timed out because it embedded a fatal expedition cascade
through fate, journal, health, memorial, and save owners. Removed that unrelated
cascade from this panel-focused smoke; its panel aftermath rendering assertion
remains, and the existing rescue/failure Core bridge test remains the focused
failure contract. The UI smoke still exercises open/close/reopen, encounter
modals, German localization, patrol affordability, and keyboard focusability.
The capped runtime command passed with exit 0. Godot still prints teardown
diagnostics (3 CanvasItem RIDs, one dummy texture RID, 8 ObjectDB instances, and
one resource) after the PASS summary; these do not affect the exit code. No
production gameplay or persistence behavior changed.

Three closeout loops: (1) localization key/source/locale parity via the drift
gate; (2) read-model edge contracts via the 50-test expedition target; (3)
runtime open/close/reopen and cap via the bounded Godot smoke. Claim paths are
released on this closeout.
