# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan 37 residual — live keybinding reset

> STATUS: APPROVED BY USER
> Authorization: “continue with next plan 1 integration!” (2026-09-30).

## Scope and evidence

SettingsPanel removes overrides in its draft on keybinding RESET; Apply previously ignored missing entries, leaving old live keys active. The existing KeyBindingApplicator now restores canonical defaults on APPLY for absent/empty overrides through its Reset helper. Joypad events, non-rebindable actions, safe-mode behavior and pending draft cancellation remain preserved. Audio previews use an audio-only method on the existing settings owner so volume callbacks do not apply the pending keybinding draft. No new authority, save schema or controller-dispatch redesign.

## Owned files

`src/Settings/KeyBindingApplicator.cs`, `src/Settings/UserSettings.cs`, `src/UI/SettingsPanel.cs` (three audio preview callbacks only), `src/Host/HostCli.Command.RunSettingsSelfTest.cs`, this plan/archive, `.ai/state.md`, claim and bounded `INTEGRATION_PLANS.md` entry. All Year Two and panel source is read-only.

## Acceptance

Current runtime evidence required a probe-only reconciliation: its pre-existing critical-red assertion expected (0.902, 0.200), while the current `Theme.Critical` authority is #FF5252 (1.000, 0.322). Match that current contract; do not change theme or restore the retired color.

Existing focused UserSettingsRecoveryTests pass via bin/run-scoped-tests. Current host build succeeds; existing settings-selftest at 15 FPS verifies custom binding, remove-and-apply reset, empty-list reset, repeated apply and preserved non-key events. Auditor checks draft/save ownership. Mark repeated FULLY INTEGRATED and immediately archive under `.ai/plans/integrated/ui/`. This certifies the reset residual only; original Plan 37 controller acceptance remains open.

## Final integration handoff — 2026-09-30

Package: complete bounded live-keybinding reset residual. Current source already contained the claimed implementation; this continuation verified all acceptance criteria and sealed it. Missing/empty overrides reset exactly one canonical key on APPLY; draft reset and audio preview retain the current live keys; controller events remain present. Audio preview neither replaces Current nor applies pending keys. Apply/Save and Cancel retain existing ownership and clone semantics. Probe color assertion reconciled to current Theme.Critical; no theme change.

Files: src/Settings/KeyBindingApplicator.cs; src/Settings/UserSettings.cs; src/UI/SettingsPanel.cs (three audio callbacks); src/Host/HostCli.Command.RunSettingsSelfTest.cs; bounded governance/plan archival. Year Two and whole-Plan37 source seams intentionally untouched by this package.

Verification: `bin/run-scoped-tests Ashfall.Core.Tests/Settings/UserSettingsRecoveryTests.cs` PASS 20/20, no failures/skips. `dotnet build Ashfall.csproj --no-restore -v:minimal` PASS zero errors/seven unrelated existing warnings. `godot --headless --path . --max-fps 15 -- --settings-selftest` PASS zero failures, including all five live-map assertions and existing save/reload/corruption/sanitization checks. Scoped whitespace check PASS; independent read-only draft/save/safe-mode/controller audit clear. No duplicate xUnit tests added. No commit or full suite. Low regression risk: bounded existing owner path, no new gameplay/save state.

Unrelated candidate disposition: user declined starting-age changes. This session's P4b production/test implementation was precisely reverted and remains unsealed; P5 received only a premise audit and remains blocked by its Standing C service prerequisite. Starting ages unchanged; P4a preserved.
