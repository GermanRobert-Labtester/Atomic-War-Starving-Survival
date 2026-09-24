# UNBLOCK — Plan 184: AccessibilitySettingsSystem / Accessibility Options System

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-184-accessibility-settings-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Accessibility Options system is fully reachable
from user preference composition, profile application, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `AccessibilitySettingsSystem` (DEC-339) the live profile and options authority
for visual assistance, hearing assistance, motor assistance, cognitive clarity, and custom overrides
while preserving existing single owners (Rule 5):

- `UserSettingsStore` owns user device and configuration persistence;
  `AudioAccessibilityCoordinator` owns directional captioning and sound visualization;
  `AccessibilitySettingsSystem` owns accessibility profiles (`accessibility_profiles.json`),
  sensory accommodation parameters, font scaling bounds [0.75, 2.0], colorblind modes,
  high contrast toggles, motion reduction flags, and census statistics (`AccessibilityCensus`).
- `AccessibilitySettingsHostSession` and `AccessibilitySettingsSaveStore` provide host-level
  lifecycle, persistence (`accessibility_settings` section, `accessibility_settings_save.json`, section #258),
  and synchronize active preferences with `UserSettingsStore.Current`.
- `--accessibility-settings-selftest` (alias `--accessibility-options-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs`: Pure domain system, state model, profiles, and `AccessibilityCensus`.
  - `Assets/StreamingAssets/Data/accessibility_profiles.json`: Authoritative accessibility profiles catalog.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `accessibility_settings` registered under `settings` domain with `ExpandedShelterLifecycleGroup` (file: `accessibility_settings_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/AccessibilitySettingsHostSession.cs`: Host session and checksummed save store, bridging with `UserSettingsStore`.
  - `src/Main.AccessibilitySettings.cs`: Main partial with setup, save, reset, profile selection, setting mutators, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetEnrolledFlagshipSessions()`.
  - `src/Host/HostCli.AccessibilitySettings.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Accessibility/Plan184AccessibilitySettingsIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: 5/5 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --accessibility-settings-selftest`: 12/12 PASS
- `godot --headless --path . -- --accessibility-options-selftest`: 12/12 PASS

## Non-Goals

No duplicate rendering or settings authorities.
No Unity dependencies or invocation.
