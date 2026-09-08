# Plan 184 — Accessibility Options System

## Goal

Create a comprehensive accessibility options system that makes ASHFALL playable by people with diverse abilities — colorblind modes, font scaling, screen reader support, high contrast, input remapping, reduced motion, audio descriptions, and cognitive load reduction. Currently the game has zero accessibility options. Plan 34 mentions "accessibility parity" as a goal and Plan 25 (localization) mentions accessibility audit, but no actual accessibility system exists. This plan ensures ASHFALL can be enjoyed by the widest possible audience.

## Why

**Repository evidence:** Grep for `Accessibility`, `Colorblind`, `Dyslexia`, `ScreenReader`, `HighContrast` in Core and src/ returns ZERO system matches — only references in existing plans and documentation. No accessibility settings, no colorblind modes, no font scaling, no input remapping, no reduced motion, no cognitive load options. The game assumes all players have full vision, hearing, motor control, and cognitive ability.

**What is missing:** No accessibility options at all. No colorblind mode. No font scaling. No screen reader support. No high contrast. No input remapping. No reduced motion. No audio descriptions. No cognitive load reduction. The game is inaccessible to players with disabilities.

**Why existing plans don't solve it:** Plan 34 (long arc) mentions "accessibility parity" as a goal but doesn't implement it. Plan 25 (localization) mentions accessibility audit but focuses on text extraction. No plan addresses accessibility options as a system.

**Player value:** Makes game playable by people with visual, auditory, motor, and cognitive disabilities. Expands potential player base. Fulfills ethical obligation. May be legally required for store release. Makes game better for all players (accessibility improvements help everyone).

## Files / Systems to Inspect

- `src/UI/` — all UI panels
- `src/Audio/` — audio system
- `project.godot` — Godot project settings
- `Assets/Ashfall.Core/UI/Theme.cs` — UI theme
- NEW: `Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs`
- NEW: `src/UI/AccessibilitySettingsPanel.cs`

## Main Task 1 — Foundation / System Contract

1. Create `AccessibilitySettingsSystem.cs` in `Assets/Ashfall.Core/Accessibility/`
2. Define `AccessibilitySettings` DTO: `colorblindMode` (none/protanopia/deuteranopia/tritanopia), `fontSize` (0.75-2.0 multiplier), `highContrast` bool, `screenReader` bool, `reducedMotion` bool, `audioDescriptions` bool, `cognitiveLoadReduction` bool, `inputRemapping` (dict of action → key/button), `subtitleSize` (small/medium/large), `colorCorrection` (brightness/contrast/saturation sliders)
3. Define `AccessibilityProfile` DTO: `profileId`, `profileName` (visual_impairment/hearing_impairment/motor_impairment/cognitive_impairment/custom), `presetSettings` (pre-configured settings for specific needs)
4. Define `AccessibilityState` DTO: active settings, active profile, custom overrides, settings locked bool
5. Implement `CaptureState/RestoreState` with schema versioning
6. Define visual accessibility:
   - **Colorblind modes**: protanopia (red-blind), deuteranopia (green-blind), tritanopia (blue-blind)
   - **Font scaling**: 0.75x to 2.0x text size
   - **High contrast**: increased color contrast for all UI
   - **Color correction**: brightness, contrast, saturation sliders
   - **UI scaling**: entire UI scales up/down
   - **Shape indicators**: add shapes alongside colors for colorblind players
7. Define auditory accessibility:
   - **Audio descriptions**: narrate visual events
   - **Subtitle size**: small/medium/large subtitles
   - **Visual alerts**: flash/screen shake for audio cues
   - **Volume per bus**: individual volume control per audio bus
   - **Mono audio**: combine stereo to mono
   - **Closed captions**: all dialogue captioned
8. Define motor accessibility:
   - **Input remapping**: remap any action to any key/button
   - **Hold vs toggle**: actions can be hold or toggle
   - **Input buffering**: longer input buffer window
   - **Auto-walk**: toggle walk instead of hold
   - **Assisted aiming**: aim assist for combat
   - **One-handed mode**: reduced input requirements
9. Define cognitive accessibility:
   - **Reduced motion**: minimize animations and screen shake
   - **Cognitive load reduction**: simplify UI, reduce information density
   - **Pause anywhere**: pause in all situations
   - **Objective tracking**: clear objective markers
   - **Tutorial persistence**: tutorials remain accessible
   - **Time pressure removal**: optional removal of time limits
10. Define accessibility profiles:
    - **Visual impairment**: high contrast, font scaling, colorblind mode, audio descriptions
    - **Hearing impairment**: visual alerts, large subtitles, mono audio
    - **Motor impairment**: input remapping, hold/toggle, auto-walk, assisted aiming
    - **Cognitive impairment**: reduced motion, cognitive load reduction, pause anywhere
    - **Custom**: player-configured combination
11. Add deterministic settings: accessibility settings are applied deterministically
12. Wire into `GameBootstrap`: `SetupAccessibility`, apply settings to all systems
13. Create accessibility settings UI: comprehensive settings panel
14. Implement accessibility at game start: accessibility selection before campaign
15. Add accessibility indicator to settings menu

## Main Task 2 — Implementation / Visual / Auditory / Motor / Cognitive

1. Implement colorblind modes:
   - Color transformation matrices for each mode
   - Applied to all UI rendering
   - Shape indicators alongside color indicators
   - Tested with colorblind simulation tools
2. Implement font scaling:
   - All text respects font size multiplier
   - UI layouts adjust for larger text
   - Text wrapping handles overflow
   - Minimum readable size enforced
3. Implement high contrast:
   - Increased contrast ratios for all UI elements
   - Bold outlines on interactive elements
   - High contrast color palette
   - WCAG 2.1 AA compliance target
4. Implement audio descriptions:
   - Narrate significant visual events
   - Describe scene changes
   - Narrate UI state changes
   - Optional audio description track
5. Implement input remapping:
   - Full input remapping for all actions
   - Keyboard, mouse, gamepad support
   - Preset configurations
   - Custom binding save/load
6. Implement reduced motion:
   - Minimize screen shake
   - Reduce animation speed
   - Remove parallax effects
   - Simplify particle effects
7. Implement cognitive load reduction:
   - Simplified UI mode
   - Reduced information density
   - Clear objective markers
   - Step-by-step guidance
8. Implement subtitle system:
   - All dialogue subtitled
   - Adjustable subtitle size
   - Subtitle background for readability
   - Speaker identification
9. Implement visual alerts:
   - Visual indicators for audio cues
   - Screen edge flash for alerts
   - Directional indicators
   - Icon-based alerts
10. Implement pause system:
    - Pause available in all situations
    - Game fully pauses (no tick advancement)
    - Pause menu accessible
    - Settings adjustable while paused
11. Create accessibility events:
    - "The Configuration" — accessibility settings configured
    - "The Profile" — accessibility profile activated
    - "The Adjustment" — settings adjusted mid-campaign
    - "The Test" — accessibility test passed
12. Add accessibility quest hooks:
    - "The Access" — configure accessibility settings
    - "The Profile" — activate accessibility profile
    - "The Test" — complete accessibility self-test
13. Implement accessibility UI:
    - Settings panel: all accessibility options
    - Profile selection: preset profiles
    - Test panel: accessibility self-test
    - Quick toggle: accessibility on/off
    - Per-category tabs (visual/auditory/motor/cognitive)
14. Add accessibility journal: log of accessibility changes
15. Implement accessibility tutorial: first launch explains options

## Main Task 3 — Integration / Consequences / Validation

1. Wire into all UI panels: respect accessibility settings
2. Connect to audio system: respect auditory settings
3. Integrate with input system: respect remapping
4. Connect to theme system: respect visual settings
5. Wire into render system: respect motion settings
6. Connect to UI layout: respect scaling settings
7. Implement old-save compatibility: settings persist across saves
8. Add deterministic application: settings applied deterministically
9. Create exploit prevention: settings are player preference, no gameplay advantage
10. Add tests: colorblind modes, font scaling, input remapping, reduced motion, save round-trip
11. Verify all accessibility options work
12. Test with accessibility tools (screen readers, colorblind simulators)
13. Verify headless behavior: settings apply correctly without UI
14. Add data-integrity-selftest: accessibility settings validate
15. Create `--accessibility-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --accessibility-selftest
```

## Risk

**MEDIUM** — Accessibility implementation is extensive and touches every system. Risk of incomplete implementation or settings that don't fully work. Mitigation: prioritize most impactful options (colorblind, font scaling, input remapping), test with real users, iterate based on feedback, and ensure settings are saved and persist.

## Definition of Done

- `AccessibilitySettingsSystem.cs` exists with full `CaptureState/RestoreState`
- 3 colorblind modes (protanopia, deuteranopia, tritanopia)
- Font scaling (0.75x-2.0x)
- High contrast mode
- Audio descriptions
- Full input remapping
- Reduced motion mode
- Cognitive load reduction
- Subtitle system with size options
- Visual alerts for audio cues
- Pause anywhere
- 4 accessibility profiles (visual, hearing, motor, cognitive)
- Accessibility settings UI panel
- Settings persist across saves
- All accessibility options tested
- Old saves maintain settings
- Accessibility profiles in data authority
- Cross-system integration (UI, audio, input, theme, render, layout)

## Follow-On Opportunities

- Accessibility testing suite (automated accessibility tests)
- Accessibility community feedback (player reports)
- Accessibility certification (meet accessibility standards)
- Accessibility documentation (player guide)
- Accessibility legacy (accessibility improvements remembered)
