# Plan 3 — Campaign backup recovery and resumption

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

## Outcome

Recover a damaged primary save from up to three validated backup generations,
with player confirmation, and resume the saved campaign day and valid panel.

## Contract and sequence

1. Keep `SaveSlotService` as the sole rotation/recovery authority. Validate source
   generations before rotation, preserve atomic writes, and propagate failures
   through `SaveLoadHostSession` and the existing save orchestrator.
2. Offer recovery only when a backup validates. Show its campaign day and warn
   that later progress will be lost. Cancel preserves the current session;
   confirmation restores through the normal load path. Report recovery failures.
3. Use manifest v3 for `lastPanelId`; retain the existing `currentDay` authority.
   Preserve v1/v2 checksum compatibility and a safe route fallback when saved
   panel metadata is absent or no longer usable.
4. Reuse the existing Continue entry point and capture/restore lifecycle.
   Record autosave triggers and the remaining crash window: backup rotation
   protects committed generations, not progress that has never been saved.

## Ownership

- Core: `Assets/Ashfall.Core/Save/{SaveSlotService,CampaignSaveEnvelope,SaveSlotTypes}.cs`.
- Host/UI: `src/Host/SaveLoadHostSession.cs`,
  `src/Main.{SaveOrchestrator,GameFlow,UiPanels}.cs`, `src/UI/SaveLoadPanel.cs`.
- Verification: `Ashfall.Core.Tests/SaveSlotServiceTests.cs`,
  `Ashfall.Core.Tests/Save/ActiveSaveSlotPersistenceTests.cs`, and
  `src/Main.UiTests.StartingCohortLifecycle.cs`.

## Acceptance

- Round-trip restores payload, day and panel; legacy manifests still load.
- Corrupt primary and backup cases select a validated candidate or report that
  recovery is unavailable. Failed saves cannot report success.
- The existing lifecycle probe covers navigation, dialog focus, cancel and
  confirmation using isolated temporary saves.
- A bounded interrupted-write check preserves previously committed saves and
  demonstrates explicit recovery. It does not prove protection from every
  filesystem or power-loss failure.

Reuse existing tests through `bin/run-scoped-tests`; serialize runtime checks
with the integrator. Repository cleanup remains in the parent QoL package.
