# C2 — Flagship Integration Plan [39]: Accessibility Preferences, Inclusive Presentation, Input Adaptation, and Cross-System Accessibility Contract

> **Deliverable:** `C2_planintegration[39].md`
> **Source scope:** Plan 184 — *Accessibility Options System*
> **Primary objective:** create a comprehensive accessibility layer covering visual, auditory, motor, and cognitive access; make accessibility preferences available before campaign start and throughout play; route those preferences into UI, theme, audio, input, motion, subtitle/caption, guidance, pause, and layout systems; and validate that accessibility changes presentation/control without creating parallel gameplay state or granting hidden mechanical advantage.
> **Required execution order:** **184A Foundation / Preference Contract → 184B Visual, Auditory, Motor, Cognitive Implementation → 184C Cross-System Integration, Automated/Manual Validation, and Release Closure**
> **Hard dependencies:** Plan 25 localization/text abstraction; Plan 37 input/focus/controller/rebinding foundation; UI theme/layout system; audio buses and cue system; subtitle/dialogue presentation; render/animation layer; pause/time-control authority; save/settings infrastructure; Plan 39 save durability only for migration compatibility where campaign files currently contain settings; Plan 55 retention only for diagnostics/history, not accessibility preference storage.
> **Critical ownership correction:** accessibility options are **user/device preferences**, not campaign simulation facts. Their primary persistence target should be the application/user settings store, not per-campaign save data. Campaign load may import legacy accessibility fields if any exist, but one campaign must not silently overwrite the player's global accessibility configuration.
> **Scope discipline:** no duplicate input-remapping engine, no second theme system, no second subtitle system, no duplicate audio bus controls, no accessibility setting that alters simulation outcomes unless it is an explicitly permitted timing/interaction accommodation, no global color filter as the sole colorblind solution, no flashing visual-alert default that creates photosensitivity risk, no hidden “accessibility on/off” master switch that can accidentally disable critical accommodations, and no requirement that players disclose a disability or choose a diagnostic label to access options.

---

# 0. Executive Intent

ASHFALL already has or is planned to have many of the systems accessibility must modify:

- UI panels,
- text rendering,
- localization,
- input actions,
- keyboard/controller navigation,
- audio buses,
- subtitles/captions,
- animation and screen shake,
- pausing,
- tutorials and objective guidance.

What it lacks is a single **accessibility preference contract** that tells those systems how to present and accept interaction.

The desired architecture is:

```text
user accessibility preferences
        │
        ▼
AccessibilitySettingsSystem
        │
        ├─ visual preferences
        ├─ auditory preferences
        ├─ motor/input preferences
        ├─ cognitive-load preferences
        └─ accessibility presets
        │
        ▼
existing presentation/control owners
        │
   ┌────┼───────────┬───────────┬────────────┐
   ▼    ▼           ▼           ▼            ▼
 Theme  UI/Layout  Audio      Input      Time/Pause
   │                 │           │             │
   ▼                 ▼           ▼             ▼
contrast/fonts   captions   remapping      motion/time
```

The system should not become a new game-simulation subsystem.

It is a **cross-cutting settings and adaptation layer**.

The strongest product outcome is:

> **A player can configure accessibility before seeing the main campaign UI, combine accommodations freely without selecting a disability label, change them while paused, and trust that the entire game—including menus, alerts, subtitles, input, motion, and dense management screens—respects those choices consistently.**

---

# 1. Source Diagnosis and Architectural Reconciliation

The source claims there are no accessibility options.

Before implementation, verify this against current repository state because prior C2 work already defined adjacent accessibility capabilities.

Relevant prior architecture:

- **Plan 25** established localization/text extraction, pseudo-locale, font/glyph audit, and text-scaling/accessibility concerns.
- **Plan 37** established input dispatcher completeness, focus navigation, controller bindings, rebinding/settings, and text-scale/accessibility work.
- **Plan 52** established audio/visual parity for critical alerts.
- Multiple later plans require keyboard/controller accessibility and non-color-only indicators.

Therefore the implementation must begin with a **capability audit**, not assume all source claims are still true.

The correct outcome may be:

```text
existing partial feature
→ register under AccessibilitySettingsSystem

missing feature
→ implement in owning subsystem

duplicate source task
→ integrate / retire duplicate
```

Do not create parallel versions of:

- input remapping,
- text scale,
- controller navigation,
- audio volume controls,
- subtitle rendering.

---

# 2. Program-Level Success Criteria

C2[39] closes only when all of the following are true.

1. One authoritative accessibility settings service exists.
2. Accessibility settings persist globally per user/device.
3. Accessibility settings are available before campaign creation/load.
4. Existing campaigns do not overwrite global accessibility settings.
5. Legacy accessibility fields, if any, migrate safely.
6. Visual accommodations can be combined independently.
7. Auditory accommodations can be combined independently.
8. Motor accommodations can be combined independently.
9. Cognitive accommodations can be combined independently.
10. Presets are convenience bundles, not mutually exclusive disability modes.
11. Custom overrides can modify preset values.
12. Color-dependent information always has a non-color cue.
13. Font scaling reaches 2.0× without critical clipping on supported layouts.
14. Text wrapping and scrolling remain usable at maximum scale.
15. High-contrast mode applies through the theme layer.
16. Colorblind support does not rely solely on a full-screen transform matrix.
17. Critical audio cues have visual/text equivalents.
18. Dialogue and critical voice content have captions/subtitles.
19. Input actions can be rebound through the canonical input system.
20. Conflict detection prevents unusable bindings.
21. Essential actions always retain a recoverable input path.
22. Hold/toggle preferences work for supported actions.
23. Reduced motion disables/minimizes screen shake, parallax, and unnecessary motion.
24. Visual alerts respect reduced-motion and photosensitivity-safe rules.
25. Simplified/cognitive-load UI mode reduces density without hiding required gameplay truth.
26. Pause accessibility is integrated with the canonical time-control system.
27. Accessibility preferences can be changed while paused.
28. No accessibility setting silently changes core simulation RNG/state.
29. Any time-pressure accommodation has an explicit simulation policy and is tested.
30. Screen-reader/accessibility semantics exist for major interactive UI.
31. Accessibility self-test verifies representative panels and interactions.
32. Headless tests validate settings transformation logic without needing rendered UI.
33. Real-render/manual tests validate what headless tests cannot prove.
34. Old saves remain loadable.
35. Settings application is idempotent.
36. Profile activation followed by custom override behaves predictably.
37. Accessibility options are localized.
38. UI does not require color, sound, hover, precise pointer movement, or rapid repeated input for critical actions.
39. All major release-routed panels pass an accessibility coverage matrix.
40. `--accessibility-selftest` is part of CI/release validation.

---

# 3. Architectural Invariants

## 3.1 Accessibility is preference state, not campaign state

Primary persistence:

```text
user:// accessibility/settings
```

or the repository's canonical user-settings store.

Campaign saves may contain only migration compatibility metadata if unavoidable.

## 3.2 No diagnostic gating

A player never needs to declare:

```text
I am visually impaired
I am hearing impaired
```

to use an option.

Profiles are presets only.

## 3.3 Settings compose

A player can enable:

```text
high contrast
+ large text
+ reduced motion
+ mono audio
+ remapped controls
```

simultaneously.

No profile exclusivity.

## 3.4 Existing owners apply settings

AccessibilitySettingsSystem stores and broadcasts preferences.

It does not render, mix audio, or dispatch input itself.

## 3.5 Presentation should not alter simulation truth

Color, font, captions, high contrast, mono audio, reduced motion, and most input settings affect presentation/control only.

## 3.6 Timing accommodations require explicit policy

“Time pressure removal” is not a cosmetic toggle.

If implemented:

- define which timers are pausable/extensible,
- preserve deterministic behavior,
- avoid competitive/fairness assumptions that do not apply to single-player,
- document exceptions.

## 3.7 Color is never the sole state channel

Every critical status that uses color also uses:

- icon,
- shape,
- label,
- pattern,
- text.

## 3.8 Audio is never the sole critical alert channel

Every critical cue has:

- visual indicator,
- caption/text equivalent.

## 3.9 Motion is never required to understand state

Reduced-motion mode retains information.

## 3.10 Accessibility tests include real rendering/manual review

Headless CI cannot validate every visual or assistive-technology behavior.

---

# 4. Accessibility Settings Ownership Model

Recommended settings service:

```text
AccessibilitySettingsSystem
        │
        ├─ current effective settings
        ├─ selected preset/profile
        ├─ custom overrides
        ├─ validation
        ├─ change notifications
        └─ persistence adapter
```

Downstream adapters:

```text
IVisualAccessibilitySink
IAudioAccessibilitySink
IInputAccessibilitySink
IMotionAccessibilitySink
ICognitiveAccessibilitySink
IPauseAccessibilitySink
IAccessibilitySemanticTreeSink
```

Avoid a monolithic method such as:

```text
ApplyAccessibilityToEverything()
```

with direct references to 100 panels.

---

# 5. Persistence Contract

The source proposes:

```text
AccessibilityState
→ CaptureState / RestoreState
```

Refine this.

Use two distinct concerns:

## User preference persistence

```text
AccessibilityPreferencesStore
```

Owns actual settings.

## Campaign compatibility

If old saves or current architecture serializes settings:

```text
AccessibilityCampaignCompatibilityState
```

may capture:

- schema marker,
- imported legacy preference version.

Do not make campaign save the long-term source of truth.

---

# 6. Recommended DTOs

```text
AccessibilitySettings
AccessibilityPresetDefinition
AccessibilityOverrideSet
AccessibilityEffectiveSettings
AccessibilityValidationResult
InputAccessibilitySettings
VisualAccessibilitySettings
AuditoryAccessibilitySettings
MotorAccessibilitySettings
CognitiveAccessibilitySettings
```

Avoid one flat DTO growing forever if subsystem boundaries are clear.

---

# 7. Visual Settings DTO

Suggested:

```text
color_vision_mode
font_scale
ui_scale
high_contrast
brightness
contrast
saturation
shape_redundancy
focus_outline_strength
cursor_scale
text_spacing_preset optional
```

Source-required colorblind modes:

```text
None
Protanopia
Deuteranopia
Tritanopia
```

---

# 8. Auditory Settings DTO

Suggested:

```text
audio_descriptions
subtitle_size
subtitle_background
speaker_labels
closed_captions
visual_audio_cues
mono_audio
bus_volume_overrides
dynamic_range_preset optional
```

Do not duplicate the actual audio-bus model.

Store user values by canonical bus ID.

---

# 9. Motor Settings DTO

Suggested:

```text
binding_overrides
hold_toggle_overrides
input_buffer_multiplier
auto_walk
aim_assist
one_handed_layout
double_click_speed/accommodation if supported
pointer_sensitivity
controller_deadzone
```

Only expose features the current game interaction model actually uses.

---

# 10. Cognitive Settings DTO

Suggested:

```text
reduced_motion
reduced_ui_density
pause_anywhere
objective_guidance
persistent_tutorial_access
time_pressure_mode
confirmation_assistance
information_chunking
```

Do not hide required status values in reduced-density mode.

---

# 11. Accessibility Presets

Source preset concepts:

```text
visual_impairment
hearing_impairment
motor_impairment
cognitive_impairment
custom
```

Rename carefully in user-facing UI if desired.

Safer product terminology is capability-focused:

```text
Visual Support
Hearing Support
Motor Support
Cognitive Support
Custom
```

Presets should be editable.

---

# 12. Preset Data Authority

Create:

```text
Assets/StreamingAssets/Data/accessibility_profiles.json
```

or canonical settings-data location.

A preset contains only defaults.

It does not lock the player to those values.

---

# 13. Preset Override Semantics

Effective settings:

```text
base defaults
→ selected preset
→ custom overrides
```

Switching preset should prompt or clearly explain whether custom overrides reset.

No silent loss of configuration.

---

# 14. `settings locked` Source Field

The source proposes:

```text
settings locked bool
```

Do not use as a normal player feature unless there is a real ownership reason.

Potential valid uses:

- parental/managed environment,
- automated test fixture.

Baseline accessibility should remain user-editable.

If no valid consumer exists:

```text
remove settings_locked
```

from production DTO.

---

# 15. Deterministic Application

Accessibility settings application should be deterministic:

```text
same settings
+ same UI state
→ same effective presentation/input configuration
```

This is not gameplay RNG determinism.

---

# 16. Change Notification Model

Use:

```text
AccessibilitySettingsChanged
```

with typed changed-domain set:

```text
Visual
Auditory
Motor
Cognitive
InputBindings
```

Downstream systems reapply only relevant changes.

---

# 17. Settings Versioning

Version the user preference file.

Migrations must:

- preserve known settings,
- apply defaults only to new fields,
- never reset existing accessibility choices silently.

---

# 18. Settings Recovery

If preferences corrupt:

- fall back to safe defaults,
- retain recoverable backup if settings service supports it,
- never strand player with unreadable UI.

---

# 19. First-Launch Accessibility Entry Point

Before normal onboarding/campaign:

```text
Accessibility
```

must be visible.

The player should be able to configure:

- text size,
- contrast,
- subtitles,
- motion,
- input basics

before reading dense screens.

---

# 20. First-Launch Preview

Preview changes live:

- sample text,
- status colors/shapes,
- subtitle sample,
- motion sample,
- audio cue + visual cue,
- input test.

No need to launch campaign.

---

# 21. Workstream 184A — Foundation / Preference Contract

## Goal

Create one settings authority, reconcile existing partial accessibility work, define adapters, persistence, profiles, validation, and startup application.

---

# 22. 184A Phase A — Repository Capability Audit

Search:

```text
Accessibility
Colorblind
HighContrast
FontScale
TextScale
Subtitle
Caption
ReducedMotion
Rebind
InputMap
Controller
Focus
ScreenReader
AccessibilityName
```

Classify every hit:

```text
LIVE
PARTIAL
PLAN_ONLY
DEAD
DUPLICATE
```

---

# 23. 184A Phase B — Reconcile Plan 37

Plan 37 already owns:

- action completeness,
- focus/navigation,
- controller bindings,
- rebinding,
- settings,
- text scale/accessibility.

Therefore:

```text
AccessibilitySettingsSystem
→ consumes/coordinates Plan 37 services
```

Do not create a second rebind implementation.

---

# 24. 184A Phase C — Reconcile Plan 25

Plan 25 already owns:

- localized text keys,
- font/glyph audit,
- pseudo-expansion,
- text-scale concerns.

Accessibility uses that infrastructure.

Large text cannot depend on hardcoded inline labels.

---

# 25. 184A Phase D — Create `AccessibilitySettingsSystem`

Path:

```text
Assets/Ashfall.Core/Accessibility/AccessibilitySettingsSystem.cs
```

Responsibilities:

- validate settings,
- calculate effective settings,
- select preset,
- apply custom overrides,
- notify adapters,
- import/export user preferences,
- expose diagnostics.

It should remain UI-framework agnostic where possible.

---

# 26. 184A Phase E — Create Preference Store Adapter

Example:

```text
IAccessibilityPreferencesStore
```

Methods:

```text
Load()
Save(settings)
ResetToDefaults()
```

Implementation lives in host/user settings layer.

---

# 27. 184A Phase F — Defaults

Default configuration should be conservative and readable.

Examples:

```text
font_scale = 1.0
ui_scale = 1.0
high_contrast = false
closed_captions = true if dialogue requires it
reduced_motion = false
```

Do not assume accessibility features must be off if enabling them is universally beneficial.

---

# 28. 184A Phase G — Settings Validation

Validate ranges:

```text
font scale 0.75–2.0
UI scale supported range
brightness/contrast/saturation safe ranges
input buffer safe range
volume 0–1
```

Reject NaN/invalid values.

---

# 29. 184A Phase H — Font Scale Floor

The source allows 0.75×.

Audit whether 0.75× violates the product’s minimum readable text size.

If so:

- retain slider range only if minimum absolute pixel/point size is still readable,
- otherwise raise effective minimum.

Accessibility settings should not allow creating inaccessible text.

---

# 30. 184A Phase I — UI Scale vs Font Scale

Keep separate:

```text
font_scale
ui_scale
```

Font scale changes text.

UI scale changes panels/controls.

Layouts must support both.

---

# 31. 184A Phase J — Color Vision Mode

Do not rely solely on transformation matrices.

Preferred stack:

```text
semantic palette remap
+ shape/icon redundancy
+ optional simulation/filter preview
```

Global matrices can help preview or artwork correction, but cannot fix meaning encoded only in red/green statuses.

---

# 32. 184A Phase K — Shape Redundancy

Create shared semantic indicator rules.

Example:

```text
safe
→ green + check shape + "Safe"

warning
→ amber + triangle + "Warning"

critical
→ red + octagon/exclamation + "Critical"
```

Exact palette/icon set follows theme system.

---

# 33. 184A Phase L — High Contrast

Theme variant must alter:

- text/background contrast,
- outlines,
- focus indicators,
- selected controls,
- disabled controls,
- critical statuses.

Do not merely increase saturation.

---

# 34. 184A Phase M — Brightness/Contrast/Saturation

Determine owner.

If render pipeline has post-processing controls:

- accessibility stores preference,
- render owner applies.

Do not apply a UI-only transform if source goal is whole-screen correction.

---

# 35. 184A Phase N — Focus Visibility

Motor/visual accessibility requires visible keyboard/controller focus.

Plan 37 focus factory must support:

- stronger focus outline,
- focus animation reduced-motion alternative.

---

# 36. 184A Phase O — Screen Reader Contract

A game UI may not integrate identically with desktop web screen readers.

Define actual target:

- semantic accessibility names,
- roles,
- values,
- focus announcements,
- optional built-in narration/TTS adapter if platform supports it.

Do not check a `screenReader bool` without a working semantic output path.

---

# 37. 184A Phase P — Accessibility Semantic Node

Create/extend UI abstraction:

```text
AccessibilityNode
{
    role
    label
    value
    hint
    state
    actions
}
```

Generated from visible UI.

Hidden game information must not leak.

---

# 38. 184A Phase Q — Screen Reader Adapter

Interface:

```text
IAccessibilityNarrator
```

Can:

- announce focused control,
- announce changed critical state,
- narrate accessible descriptions.

Platform implementation may vary.

---

# 39. 184A Phase R — Audio Description Contract

Do not conflate:

```text
screen-reader UI narration
```

with:

```text
audio description of visual gameplay events
```

Separate settings and content channels.

---

# 40. 184A Phase S — Closed Caption Contract

Closed captions include meaningful non-dialogue audio cues.

Examples:

```text
[alarm sounding]
[geiger clicks intensify]
[distant gunfire]
[door banging]
```

Only for information-significant cues.

---

# 41. 184A Phase T — Subtitle Contract

Subtitles cover speech/dialogue.

Settings:

- size,
- background,
- speaker label,
- duration if configurable.

---

# 42. 184A Phase U — Mono Audio

Audio owner combines channels or uses engine-supported output setting.

Test stereo-positioned critical cues.

Directional info must also have visual cue.

---

# 43. 184A Phase V — Per-Bus Volume

Reuse actual audio buses.

Accessibility store keeps user volume by canonical bus IDs.

Do not create duplicate bus hierarchy.

---

# 44. 184A Phase W — Visual Alerts

Replace source's unsafe concept:

```text
flash/screen shake for audio cues
```

with:

```text
icon
edge indicator
text caption
direction marker
optional restrained luminance pulse
```

Screen shake is not an accessibility substitute and conflicts with reduced motion.

---

# 45. 184A Phase X — Photosensitivity-Safe Alert Rule

Avoid rapid high-contrast flashing.

Any pulsing alert should:

- be slow,
- optional,
- have static icon/text equivalent.

---

# 46. 184A Phase Y — Input Remapping

Reuse canonical Plan 37 input mapping.

Accessibility layer configures:

- bindings,
- presets,
- hold/toggle,
- buffer window,
- one-handed preset.

---

# 47. 184A Phase Z — Binding Conflict Validation

Detect:

- exact conflicts,
- inaccessible essential action,
- same-context collision.

Offer:

```text
Swap
Replace
Keep both if allowed
Cancel
```

---

# 48. 184A Phase AA — Essential Recovery Binding

Never permit user to lose all routes to:

```text
Open Settings
Back/Cancel
Confirm
Pause
```

Provide safe recovery/reset.

---

# 49. 184A Phase AB — Hold vs Toggle

Supported actions declare interaction style.

Examples:

```text
hold-to-aim
hold-to-sprint
hold-to-pan
```

For management UI, many actions may be press-based already.

Do not invent hold/toggle semantics for actions that do not use them.

---

# 50. 184A Phase AC — Input Buffer

Define what it means in current game.

For UI navigation:

- longer acceptance window,
- debounced repeat timing.

For combat/action gameplay:

- input queue window.

Do not globally delay all controls.

---

# 51. 184A Phase AD — Auto-Walk

Only if ASHFALL has continuous character walking.

If game is primarily management/point-and-click:

```text
mark not applicable
```

Do not add a fake feature to satisfy a checklist.

---

# 52. 184A Phase AE — Assisted Aiming

Only if combat exposes player aiming.

If combat is command/management driven:

```text
define applicable targeting assistance instead
```

or mark not applicable.

No unnecessary feature invention.

---

# 53. 184A Phase AF — One-Handed Mode

Implement as input preset/layout where interaction model supports it.

Goals:

- reduce simultaneous inputs,
- avoid chorded controls,
- keep all core actions reachable.

---

# 54. 184A Phase AG — Reduced Motion

One setting coordinates:

- screen shake,
- parallax,
- animated backgrounds,
- camera easing,
- nonessential transitions,
- particles.

Each owner consumes the preference.

---

# 55. 184A Phase AH — Motion Levels

Instead of bool only, consider:

```text
Full
Reduced
Minimal
```

Source requires bool, so baseline can map:

```text
false → Full
true → Reduced/Minimal
```

with future extension.

---

# 56. 184A Phase AI — Cognitive Load Reduction

Define concrete UI transformations.

Examples:

- collapse secondary metadata,
- default advanced details closed,
- prioritize top 3 actions,
- progressive disclosure,
- simplified tooltips,
- one-warning-at-a-time grouping.

No hidden mechanics.

---

# 57. 184A Phase AJ — Objective Tracking

Reuse quest/objective system.

Accessibility setting may:

- pin objective,
- increase marker clarity,
- show next action.

Do not create duplicate objective truth.

---

# 58. 184A Phase AK — Tutorial Persistence

Tutorial/help content should remain reopenable.

Plan 37 help routing can host this.

---

# 59. 184A Phase AL — Pause Anywhere

Audit what “all situations” means.

Single-player simulation should allow pause except:

- non-pausable external OS/system actions,
- possibly cutscene loading transitions.

Canonical time system owns actual pause.

---

# 60. 184A Phase AM — Settings While Paused

Accessibility menu must work when simulation time is stopped.

No settings application should advance tick.

---

# 61. 184A Phase AN — Time Pressure Removal

This is high-risk scope.

Define explicit modes:

```text
Default
Extended
NoDeadlineWhereSafe
```

Only affect supported player-facing timers.

Do not erase:

- world chronology,
- resource decay,
- weather duration

unless design explicitly says so.

---

# 62. 184A Phase AO — Timer Classification

Inventory all timers:

```text
UI response timer
dialogue choice timer
combat/action timer
quest deadline
world simulation clock
broadcast schedule
disaster response window
```

Only eligible classes get accessibility extension/removal.

---

# 63. 184A Phase AP — No Gameplay Advantage Claim

The source says no gameplay advantage.

For a single-player game, that framing is too simplistic.

Some accommodations legitimately reduce timing/motor difficulty.

Correct contract:

> Accessibility accommodations may reduce interaction barriers, but they must not silently alter unrelated economy, RNG, rewards, or simulation state.

Document intended exceptions.

---

# 64. 184A Phase AQ — Accessibility Profiles

Create preset definitions:

```text
visual_support
hearing_support
motor_support
cognitive_support
custom
```

Each maps to defaults.

---

# 65. 184A Phase AR — Profile Does Not Choose Colorblind Type Automatically

A “visual support” preset should not assume:

```text
protanopia
```

because visual needs differ.

Instead enable:

- high contrast,
- larger text,
- stronger focus,

and prompt color-vision mode separately or choose neutral shape redundancy.

---

# 66. 184A Phase AS — Hearing Preset

Can default:

- large subtitles,
- closed captions,
- visual alerts,
- mono audio optional.

Do not force mono if stereo is preferred.

---

# 67. 184A Phase AT — Motor Preset

Can default:

- toggle interactions,
- one-handed-friendly layout,
- longer input buffer,
- aim/target assistance if applicable.

Bindings still editable.

---

# 68. 184A Phase AU — Cognitive Preset

Can default:

- reduced motion,
- simplified density,
- persistent guidance,
- pause accessibility.

Do not hide critical resource information.

---

# 69. 184A Phase AV — Custom Preset

Custom simply means no preset authority.

All settings remain individual.

---

# 70. 184A Phase AW — Startup Application Order

Apply settings before:

- main menu final layout,
- audio startup cues,
- animated backgrounds,
- focus navigation.

Avoid one-frame inaccessible defaults.

---

# 71. 184A Phase AX — Bootstrap Integration

Source says:

```text
SetupAccessibility
```

Use current composition-root conventions.

If Plan 28 generated registration exists:

- register descriptor,
- do not reintroduce manual setup drift.

---

# 72. 184A Phase AY — Accessibility Indicator

Settings menu may show:

```text
Accessibility
```

with current preset/summary.

Do not clutter HUD with a permanent accessibility icon unless needed.

---

# 73. 184A Phase AZ — Diagnostics

Expose developer report:

```text
ACCESSIBILITY_EFFECTIVE_PROFILE
ACCESSIBILITY_FONT_SCALE
ACCESSIBILITY_UI_SCALE
ACCESSIBILITY_HIGH_CONTRAST
ACCESSIBILITY_REDUCED_MOTION
ACCESSIBILITY_CAPTIONS
ACCESSIBILITY_REMAP_OVERRIDES
ACCESSIBILITY_UNSUPPORTED_OPTIONS
ACCESSIBILITY_REQUIRED_SINKS_MISSING
```

No sensitive inference.

---

# 74. 184A Tests

- load defaults,
- preset activation,
- preset + override,
- settings migration,
- global persistence,
- campaign does not overwrite user preference,
- invalid range clamping/rejection,
- change-event routing,
- startup apply,
- missing sink detection,
- no duplicate input map.

---

# 75. 184A Definition of Done

- [ ] capability audit,
- [ ] Plan 25 reconciliation,
- [ ] Plan 37 reconciliation,
- [ ] AccessibilitySettingsSystem,
- [ ] user-preference store,
- [ ] visual/auditory/motor/cognitive DTOs,
- [ ] presets,
- [ ] custom overrides,
- [ ] validation,
- [ ] semantic UI node contract,
- [ ] screen-reader/narrator adapter,
- [ ] audio-description distinction,
- [ ] input-remapping adapter,
- [ ] timer classification,
- [ ] startup application,
- [ ] settings UI entry before campaign,
- [ ] diagnostics,
- [ ] migration tests.

---

# 76. Workstream 184B — Visual Accessibility

## Goal

Make every live-routed UI understandable without relying on color, tiny text, low contrast, rapid animation, or hover-only information.

---

# 77. 184B-V Phase A — Semantic Color Inventory

Scan UI for:

```text
red
green
amber
yellow
blue
purple
statusColor
dangerColor
successColor
```

Classify every use:

```text
decorative
semantic
critical
```

Critical/semantic colors require redundant cue.

---

# 78. 184B-V Phase B — Colorblind Modes

Implement:

```text
None
Protanopia
Deuteranopia
Tritanopia
```

Use theme palette variants where possible.

---

# 79. 184B-V Phase C — Do Not Use Matrix-Only Correction

A full-screen matrix cannot make red/green-only states semantically distinct in every case.

Every semantic component must also use:

- shape,
- icon,
- label,
- pattern.

---

# 80. 184B-V Phase D — Colorblind Preview/Test Panel

Provide sample:

- health state,
- radiation warning,
- faction standing,
- weather hazard,
- route state,
- inventory quality,
- power status.

Player can compare modes instantly.

---

# 81. 184B-V Phase E — Font Scaling

All text uses shared typography tokens.

No panel-specific hardcoded font size where avoidable.

---

# 82. 184B-V Phase F — Font Scale Range

Source:

```text
0.75×–2.0×
```

Support full range only if minimum readable floor is maintained.

---

# 83. 184B-V Phase G — Large-Text Layout Envelope

Test at:

```text
1.0×
1.25×
1.5×
1.75×
2.0×
```

At each:

- no critical clipping,
- no hidden buttons,
- scrolling appears where needed,
- dialogs remain navigable.

---

# 84. 184B-V Phase H — Text Reflow

Prefer:

- flexible height,
- wrapping,
- scroll containers,
- adaptive column collapse.

Avoid shrinking text to fit.

---

# 85. 184B-V Phase I — UI Scale

Test:

```text
small supported
100%
125%
150%
175%
200%
```

or repository-supported range.

---

# 86. 184B-V Phase J — Font + UI Scale Combination

Worst-case:

```text
2.0× font
+ high UI scale
+ localized long strings
```

must remain usable.

---

# 87. 184B-V Phase K — High Contrast Theme

Create theme token override.

Audit contrast of:

- normal text,
- secondary text,
- selected row,
- focused button,
- disabled state,
- warning,
- danger,
- positive state.

---

# 88. 184B-V Phase L — Contrast Target

Source requests WCAG 2.1 AA.

Use that as minimum design target for comparable UI text/controls where applicable.

Record automated and manual contrast checks.

---

# 89. 184B-V Phase M — Focus Outline

High-contrast mode strengthens:

- keyboard focus,
- selected list item,
- active tab,
- current control.

---

# 90. 184B-V Phase N — Brightness

Apply via render system.

Keep minimum/maximum safe enough to preserve legibility.

---

# 91. 184B-V Phase O — Contrast Slider

Do not let slider destroy text readability.

UI theme may need compensation independent of scene/post-process contrast.

---

# 92. 184B-V Phase P — Saturation Slider

Useful for visual comfort.

Semantic palette redundancy ensures status survives low saturation.

---

# 93. 184B-V Phase Q — Cursor Scale

Not in source, but strongly related to visual/motor access.

If custom cursor exists, expose scaling.

If OS cursor only:

```text
not applicable
```

Document.

---

# 94. 184B-V Phase R — Dyslexia-Friendly Font

Source mentions grep for Dyslexia but does not explicitly require a font mode.

Do not invent a special “dyslexia font” as medical guarantee.

If adding font alternatives:

- present as optional readable-font choice,
- test metrics/spacing,
- avoid efficacy claims.

---

# 95. 184B-V Phase S — Text Spacing

Optional:

- line spacing,
- letter spacing,
- paragraph spacing.

Only if theme architecture supports without destabilizing layouts.

---

# 96. 184B-V Phase T — Panel Coverage Matrix

For every live-routed panel:

| Panel | 2× font | high contrast | color-independent | keyboard focus | reduced motion | screen-reader semantics |
|---|---:|---:|---:|---:|---:|---:|

No release-routed panel omitted.

---

# 97. 184B-V Phase U — Snapshot Tests

Capture key panels under:

- default,
- high contrast,
- protanopia,
- deuteranopia,
- tritanopia,
- 2× text.

Snapshot proves rendering presence, not human usability.

---

# 98. 184B-V Phase V — Visual Human Review

Review:

- hierarchy,
- contrast,
- clutter,
- discoverability,
- color independence,
- readability.

---

# 99. Workstream 184B — Auditory Accessibility

## Goal

Ensure speech and meaningful audio events are available in text/visual form and that audio output can be configured without losing directional or critical information.

---

# 100. 184B-A Phase A — Subtitle Coverage Audit

Inventory all:

- dialogue,
- voice barks,
- radio voice,
- narration,
- cinematic speech.

Every required spoken line gets subtitle/caption metadata.

---

# 101. 184B-A Phase B — Subtitle Size

Source:

```text
Small
Medium
Large
```

Map to typography tokens.

Large subtitles must not cover essential controls.

---

# 102. 184B-A Phase C — Subtitle Background

Options:

```text
Off
Translucent
Opaque/High Contrast
```

or simpler bool if needed.

---

# 103. 184B-A Phase D — Speaker Identification

Display:

- speaker name,
- directional label if useful,
- icon optional.

Do not rely only on text color for speaker.

---

# 104. 184B-A Phase E — Closed Captions

Caption meaningful non-speech cues.

Create cue vocabulary:

```text
[alarm]
[explosion]
[gunfire]
[door pounding]
[geiger clicks]
[radio static]
[generator stops]
[water alarm]
```

---

# 105. 184B-A Phase F — Caption Direction

If audio direction is gameplay-relevant:

```text
[gunfire — left]
```

or directional icon.

---

# 106. 184B-A Phase G — Visual Alert Bus

Audio cue may emit semantic alert event.

UI visual-alert layer renders:

- icon,
- direction,
- text,
- urgency.

---

# 107. 184B-A Phase H — No Flash/Shake Requirement

Critical visual alerts do not require flash or shake.

Reduced-motion setting suppresses motion-heavy alert variants.

---

# 108. 184B-A Phase I — Audio Descriptions

Define content scope.

Priority:

- scene changes,
- non-obvious critical visual events,
- important visual-only outcomes,
- major cinematic information.

Do not narrate every decorative animation.

---

# 109. 184B-A Phase J — Audio Description Source

Use authored description strings or event descriptions.

Do not automatically infer critical descriptions from arbitrary visuals at runtime.

---

# 110. 184B-A Phase K — Narrator Arbitration

If screen-reader narration and audio description overlap:

- queue/prioritize,
- avoid speaking two channels simultaneously.

---

# 111. 184B-A Phase L — Per-Bus Volume

Expose canonical buses.

At minimum:

- master,
- music,
- ambience,
- SFX,
- voice,
- UI

according to actual project buses.

---

# 112. 184B-A Phase M — Mute Independence

Player can mute music without muting voice/captions.

No critical info disappears when a bus is muted.

---

# 113. 184B-A Phase N — Mono Audio

Test:

- UI pings,
- directional hazards,
- radio static,
- combat cues.

Visual fallback covers lost spatial detail.

---

# 114. 184B-A Phase O — Audio Chaos Reduction

If Plan 52 exposes alert density controls:

- accessibility may request reduced simultaneous nonessential cues.

Do not reimplement audio priority.

---

# 115. 184B-A Phase P — Auditory Coverage Matrix

| Cue family | Subtitle | Closed caption | Visual cue | Direction equivalent | Muted-audio safe |
|---|---:|---:|---:|---:|---:|

---

# 116. Workstream 184B — Motor Accessibility

## Goal

Make every critical action reachable with remappable input, controller/keyboard navigation, reduced holding/chording, forgiving timing, and recoverable control configuration.

---

# 117. 184B-M Phase A — Input Action Inventory

Use Plan 37 authoritative action manifest.

No local duplicate list.

---

# 118. 184B-M Phase B — Full Remapping

Every gameplay-relevant remappable action declares:

```text
action ID
contexts
allowed device types
default bindings
essential?
```

---

# 119. 184B-M Phase C — Keyboard/Mouse/Gamepad

Support devices actually supported by game.

No fake device support merely in settings UI.

---

# 120. 184B-M Phase D — Binding Persistence

Store per-device overrides globally.

Campaign change does not alter bindings.

---

# 121. 184B-M Phase E — Controller Navigation

All settings and gameplay panels navigable without mouse.

Plan 37 focus order is prerequisite.

---

# 122. 184B-M Phase F — Mouse-Only Hover Elimination

Critical information shown on hover must also be reachable by:

- focus,
- click/details,
- keyboard shortcut.

---

# 123. 184B-M Phase G — Hold/Toggle

Inventory all hold actions.

Expose supported toggle alternatives.

---

# 124. 184B-M Phase H — Chord Reduction

Avoid requiring:

```text
Ctrl+Shift+X
```

for essential actions with no alternative.

One-handed preset should prefer single-button sequences.

---

# 125. 184B-M Phase I — Input Repeat

Navigation repeat speed configurable or accessibility-adjusted.

Avoid runaway list scrolling.

---

# 126. 184B-M Phase J — Double-Click Requirements

No essential action should require double-click only.

Provide single-click/confirm alternative.

---

# 127. 184B-M Phase K — Drag Requirements

If drag-and-drop exists:

- provide select + destination alternative.

---

# 128. 184B-M Phase L — Precise Pointer Requirements

Small targets gain:

- larger hitboxes,
- UI scaling,
- keyboard/controller alternative.

---

# 129. 184B-M Phase M — Auto-Walk Applicability Audit

If continuous exploration exists:

- implement.

If not:

- mark non-applicable and substitute relevant auto-pan/continuous-scroll accommodation only if needed.

---

# 130. 184B-M Phase N — Aim Assist Applicability Audit

If direct combat aiming exists:

- implement configurable assistance.

If combat is indirect/tactical:

- implement target selection assistance only if relevant.

---

# 131. 184B-M Phase O — One-Handed Presets

Possible:

```text
keyboard-left
keyboard-right
gamepad-minimal-chord
mouse-primary
```

Only after actual input study.

---

# 132. 184B-M Phase P — Input Self-Test

Panel:

- press requested actions,
- verify remap,
- test confirm/back/pause,
- detect unreachable essential action.

---

# 133. Workstream 184B — Cognitive Accessibility

## Goal

Reduce avoidable information density, motion, time pressure, and memory burden while preserving the full underlying simulation state.

---

# 134. 184B-C Phase A — Reduced Motion

Audit:

- screen shake,
- parallax,
- animated panels,
- camera sweeps,
- particle bursts,
- pulsing warnings,
- auto-scrolling.

---

# 135. 184B-C Phase B — Reduced-Motion Substitution

Replace motion with:

- static highlight,
- opacity change,
- icon,
- text.

Do not merely slow every animation.

---

# 136. 184B-C Phase C — Essential Animation

Some animation may communicate state transition.

Reduced-motion mode should:

- shorten or cross-fade,
- preserve semantic endpoint.

---

# 137. 184B-C Phase D — Cognitive Load Reduction

Create view-density preference:

```text
Standard
Reduced
```

Potential future:

```text
Minimal
```

---

# 138. 184B-C Phase E — Reduced Density Rules

Examples:

- hide secondary flavor stats behind “More details,”
- group related alerts,
- simplify table columns,
- display one recommended next action,
- collapse historical logs.

Never hide:

- urgent survival state,
- costs,
- consequences,
- required decisions.

---

# 139. 184B-C Phase F — Progressive Disclosure

Panels should expose:

```text
essential
→ supporting detail
→ advanced detail
```

not dump everything simultaneously.

---

# 140. 184B-C Phase G — Objective Tracking

Pinned objectives:

- current objective,
- next actionable step,
- location/system target if known.

Uses canonical quest/objective state.

---

# 141. 184B-C Phase H — Guidance

Step-by-step guidance can highlight:

- required control,
- relevant panel,
- next subtask.

Must be dismissible.

---

# 142. 184B-C Phase I — Tutorial Library

All tutorials reopenable through Help.

Searchable if feasible.

---

# 143. 184B-C Phase J — Pause Anywhere

Use canonical pause.

Test:

- normal gameplay,
- modal decisions,
- combat if applicable,
- expeditions,
- disasters,
- radio,
- cutscenes.

Document exceptions.

---

# 144. 184B-C Phase K — Time Pressure Mode

Inventory time-limited player decisions.

Each declares:

```text
timer class
extendable?
removable?
simulation implication
```

---

# 145. 184B-C Phase L — No Hidden Deadline

If accessibility mode removes/extends timer:

- UI clearly indicates it,
- simulation owner receives policy.

No timer visually removed while backend still expires.

---

# 146. 184B-C Phase M — Confirmation Assistance

Optional:

- confirmation for destructive actions,
- clearer consequence summary,
- undo where canonical system supports it.

Do not use confirmation spam.

---

# 147. 184B-C Phase N — Alert Grouping

Multiple related alerts collapse into one summary.

This aligns with prior attention-budget plans.

---

# 148. 184B-C Phase O — Cognitive Accessibility Test Fixture

Use a complex shelter state:

- low power,
- illness,
- expedition,
- faction warning,
- damaged equipment.

Verify reduced mode remains actionable without hiding critical facts.

---

# 149. Workstream 184B — Profiles, Startup, and UI

## Goal

Provide discoverable, reversible, composable accessibility controls before and during gameplay.

---

# 150. 184B-U Phase A — Accessibility Settings Panel

Path:

```text
src/UI/AccessibilitySettingsPanel.cs
```

or repository-canonical panel location.

Tabs:

```text
Visual
Audio
Controls
Motion & Cognitive
Presets
Test
```

---

# 151. 184B-U Phase B — No “Accessibility On/Off” Master Toggle

Source asks for quick toggle on/off.

Reject this as unsafe baseline UX.

A master off switch can accidentally disable:

- captions,
- remaps,
- large text,
- contrast.

Instead provide:

```text
Accessibility Quick Menu
```

with individual toggles and preset switch.

---

# 152. 184B-U Phase C — Quick Menu

Quick access:

- font scale,
- high contrast,
- reduced motion,
- captions,
- visual alerts,
- pause behavior.

Do not hide full settings.

---

# 153. 184B-U Phase D — Profile Selection

Selecting preset:

- previews changes,
- shows affected settings,
- allows confirm,
- retains custom editability.

---

# 154. 184B-U Phase E — Test Panel

Test:

- text scale,
- semantic status cues,
- subtitle sample,
- visual audio cue,
- narration,
- motion,
- input bindings.

No “pass/fail disability certification.”

The test verifies configuration functionality.

---

# 155. 184B-U Phase F — First Launch

Prompt:

```text
Accessibility & Display
```

before campaign.

Player can skip and return later.

---

# 156. 184B-U Phase G — Mid-Campaign Changes

Settings apply immediately or at clearly indicated safe boundary.

No campaign restart required for ordinary presentation settings.

---

# 157. 184B-U Phase H — Settings Search

If settings menu is large:

- searchable categories,
- “accessibility” keyword.

---

# 158. 184B-U Phase I — Tooltips

Explain:

- what setting changes,
- any limitations,
- preview.

Avoid diagnostic claims.

---

# 159. 184B-U Phase J — Accessibility Tutorial

First-launch tutorial should be short.

Prefer:

```text
"You can change text, color, subtitles, motion, and controls at any time."
```

with direct button to panel.

---

# 160. 184B-U Phase K — Accessibility Journal

Source asks for a journal of accessibility changes.

Do **not** store accessibility preference changes in the in-world campaign journal by default.

This is user/device configuration, not survivor history.

If diagnostic history is useful:

```text
developer/settings change log
```

bounded and local.

No diegetic journal entry such as:

```text
Day 40: Player enabled high contrast
```

---

# 161. 184B-U Phase L — Accessibility “Events” and “Quests”

Source proposes:

```text
The Configuration
The Profile
The Adjustment
The Test
```

and quest hooks.

These are inappropriate as normal in-world campaign content because accessibility choices should not be gamified or rewarded.

Recommended replacement:

```text
non-diegetic onboarding milestones / settings analytics if local
```

No achievement or quest pressure to configure accessibility.

If retained for automated testing only:

- keep developer-only event names,
- no gameplay rewards.

---

# 162. 184B-U Phase M — Ethical Guardrail

Do not create incentives that make players feel they should enable/disable accessibility settings for rewards.

Accessibility is preference, not content progression.

---

# 163. 184B Definition of Done

- [ ] color vision modes,
- [ ] semantic palette remap,
- [ ] shape/icon redundancy,
- [ ] font scaling,
- [ ] UI scaling,
- [ ] high-contrast theme,
- [ ] brightness/contrast/saturation,
- [ ] focus visibility,
- [ ] screen-reader semantic contract,
- [ ] audio descriptions,
- [ ] subtitle size/background/speaker labels,
- [ ] closed captions,
- [ ] visual audio cues,
- [ ] per-bus volume,
- [ ] mono audio,
- [ ] full input remapping through Plan 37,
- [ ] binding conflict/recovery,
- [ ] hold/toggle,
- [ ] input-buffer accommodation,
- [ ] one-handed presets where applicable,
- [ ] auto-walk applicability resolved,
- [ ] aim-assist applicability resolved,
- [ ] reduced motion,
- [ ] reduced cognitive density,
- [ ] pause accessibility,
- [ ] timer classification/accommodation,
- [ ] objective guidance,
- [ ] persistent tutorials,
- [ ] accessibility panel,
- [ ] first-launch access,
- [ ] test panel,
- [ ] quick menu rather than unsafe master off,
- [ ] no gamified accessibility quests,
- [ ] localization.

---

# 164. Workstream 184C — Cross-System Integration

## Goal

Wire settings into every relevant owner and prove accessibility state is consistent, global, reversible, and non-destructive.

---

# 165. 184C Phase A — UI Panel Integration

Every live-routed panel must consume:

- text scale,
- UI scale,
- high contrast/theme,
- cognitive density,
- focus semantics,
- screen-reader metadata,
- reduced motion.

---

# 166. 184C Phase B — UI Registry Gate

Use Plan 28/37 panel registry.

Generate accessibility coverage report.

No manually maintained partial list if a canonical route registry exists.

---

# 167. 184C Phase C — Theme Integration

`Theme.cs` or actual theme authority consumes:

- contrast mode,
- color-vision palette,
- focus strength,
- typography scale tokens.

---

# 168. 184C Phase D — Layout Integration

Layout engine consumes:

- font/UI scale,
- reduced density.

Test dynamic resizing.

---

# 169. 184C Phase E — Audio Integration

Audio system consumes:

- bus volume,
- mono,
- audio description enablement,
- caption/visual-cue hooks.

---

# 170. 184C Phase F — Input Integration

Input system consumes:

- binding overrides,
- hold/toggle,
- buffer/repeat,
- one-handed preset,
- aim/target assist if applicable.

---

# 171. 184C Phase G — Render Integration

Render/animation owners consume:

- reduced motion,
- brightness,
- contrast,
- saturation,
- color correction.

---

# 172. 184C Phase H — Pause Integration

Time authority consumes:

- pause-anywhere policy,
- timer accommodation policy.

Accessibility service never directly manipulates day counters.

---

# 173. 184C Phase I — Objective/Tutorial Integration

Quest/help owners expose:

- objective tracking,
- persistent tutorial access,
- guidance.

---

# 174. 184C Phase J — Settings Hot Reload

Changing:

```text
font scale
high contrast
reduced motion
subtitles
```

should update live without duplicating subscriptions/listeners.

---

# 175. 184C Phase K — Session Swap

New campaign/load:

- accessibility preferences remain unchanged.

This is a critical test.

---

# 176. 184C Phase L — Multi-Save Test

Load Save A.

Change accessibility.

Load Save B.

Expected:

```text
same user settings continue
```

unless player changed them globally.

---

# 177. 184C Phase M — Legacy Save Migration

If older campaign saves contain accessibility/text-scale settings:

- import once only if user global setting is absent,
- mark migration complete,
- do not continually override.

---

# 178. 184C Phase N — Settings Backup/Reset

Provide:

```text
Reset category
Reset all accessibility settings
Restore default controls
```

with confirmation.

---

# 179. 184C Phase O — Safe Defaults After Reset

Reset must preserve at least one usable input path.

---

# 180. 184C Phase P — No Simulation RNG Impact

Compare campaign simulation digest with:

```text
default accessibility
high contrast + large text + captions + reduced motion
```

Expected identical gameplay digest.

---

# 181. 184C Phase Q — Timing Accommodation Exception

If time-pressure mode changes player-facing timers, separate that from pure presentation digest test.

Document expected simulation differences.

---

# 182. 184C Phase R — Input Accommodation Exception

Aim assist or targeting assist can alter player command execution.

It must not:

- change loot RNG,
- alter AI stats,
- modify unrelated economy.

---

# 183. 184C Phase S — Full Pause Determinism

While paused:

```text
zero simulation ticks
zero scheduled-event advancement
zero fatigue/hunger progression
zero expedition progression
```

Settings UI still works.

---

# 184. 184C Phase T — Reduced Motion Correctness

Reduced motion must not:

- skip required state transition,
- miss event callbacks,
- change gameplay timing.

Animations become presentation-only shortened/suppressed.

---

# 185. 184C Phase U — Subtitle/Caption Synchronization

Caption timing follows audio event lifetime but does not depend on audio being audible.

Muted voice still displays subtitle.

---

# 186. 184C Phase V — Visual Alert Synchronization

Visual alert originates from semantic cue event, not audio waveform playback.

Muted SFX still yields visual alert.

---

# 187. 184C Phase W — Screen Reader Focus

When focus changes:

- announce control,
- value,
- state,
- available action.

Do not announce hidden content.

---

# 188. 184C Phase X — Dynamic Update Announcements

Critical changes:

- new alert,
- dialog opened,
- validation error,
- timer warning.

Use polite/priority channels.

Avoid narration spam.

---

# 189. 184C Phase Y — Large-Text Navigation

At 2× text:

- every dialog closable,
- confirmation reachable,
- tabs scroll/stack,
- no focus offscreen without auto-scroll.

---

# 190. 184C Phase Z — Colorblind Gameplay Parity

Every semantic status remains distinguishable under each mode and grayscale-style review.

---

# 191. 184C Phase AA — High-Contrast Gameplay Parity

No:

- invisible disabled controls,
- selected-row ambiguity,
- unreadable tooltip,
- washed-out icon.

---

# 192. 184C Phase AB — One-Handed Navigation Test

Complete representative loop:

```text
open map
select expedition
assign survivor
confirm
open shelter panel
change duty
pause
open settings
```

with one-handed preset.

---

# 193. 184C Phase AC — No-Mouse Navigation Test

Complete core UI loop keyboard/controller only.

---

# 194. 184C Phase AD — No-Audio Navigation/Test

Set all audio buses to zero.

Player still receives all critical alerts through visual/caption channels.

---

# 195. 184C Phase AE — No-Color Test

Evaluate critical UI with color information removed/desaturated.

Shapes/text remain sufficient.

---

# 196. 184C Phase AF — Reduced-Motion Test

Enable reduced motion.

Assert:

- screen shake zero/minimized,
- parallax disabled,
- critical alerts still visible,
- state transitions preserved.

---

# 197. 184C Phase AG — Cognitive-Load Test

Enable reduced density.

Assert:

- critical state retained,
- no cost/consequence omitted,
- advanced detail reachable.

---

# 198. 184C Phase AH — Pause-Anywhere Test

Attempt pause in every major gameplay context.

Generate exception list.

Every exception requires rationale.

---

# 199. 184C Phase AI — Timer Accommodation Test

For each timer class:

- default,
- extended,
- no-deadline-if-safe.

Verify backend/UI parity.

---

# 200. Workstream 184C — Automated Validation

## Goal

Build repeatable CI gates for the parts of accessibility that can be validated programmatically.

---

# 201. 184C Phase AJ — Create `--accessibility-selftest`

Required command:

```bash
godot --headless --path . -- --accessibility-selftest
```

---

# 202. 184C Phase AK — Selftest: Settings

Test:

- default load,
- preset,
- custom override,
- migration,
- save/reload,
- reset.

---

# 203. 184C Phase AL — Selftest: Visual

Validate:

- allowed enum values,
- scale ranges,
- theme token coverage,
- semantic indicator redundancy metadata.

Headless cannot prove actual human contrast.

---

# 204. 184C Phase AM — Selftest: Input

Validate:

- action manifest coverage,
- remapping,
- conflicts,
- essential-action recovery,
- hold/toggle compatibility.

---

# 205. 184C Phase AN — Selftest: Audio

Validate:

- subtitle/caption metadata,
- visual cue mapping,
- audio bus references,
- mono setting route.

---

# 206. 184C Phase AO — Selftest: Motion

Validate registered motion effects expose reduced-motion behavior.

---

# 207. 184C Phase AP — Selftest: Cognitive

Validate eligible panels have reduced-density view mapping.

---

# 208. 184C Phase AQ — Selftest: Screen Reader Semantics

Validate live interactive controls have:

- role,
- label,
- state/value where relevant.

---

# 209. 184C Phase AR — Semantic Name Gate

Fail interactive controls with:

```text
empty accessible name
```

unless explicitly exempted.

---

# 210. 184C Phase AS — Focus Order Gate

Use Plan 37 focus tree.

Detect:

- unreachable controls,
- cycles with no escape,
- missing first focus,
- hidden focused control.

---

# 211. 184C Phase AT — Text Overflow Gate

Render/snapshot representative panels at:

```text
2× font
pseudo-locale expansion
```

Fail critical clipping.

---

# 212. 184C Phase AU — Color Redundancy Gate

For semantic status components, require metadata for:

```text
color + non-color cue
```

---

# 213. 184C Phase AV — Audio Redundancy Gate

Critical audio cue requires:

```text
caption or visual alert
```

---

# 214. 184C Phase AW — Motion Registration Gate

Motion-heavy effect must register:

```text
reduced-motion behavior
```

---

# 215. 184C Phase AX — Panel Coverage Gate

Every release-routed panel appears in accessibility coverage matrix.

---

# 216. 184C Phase AY — Settings Schema Integrity

Validate:

- enum values,
- ranges,
- preset references,
- bus IDs,
- input action IDs,
- timer mode IDs.

---

# 217. 184C Phase AZ — Deliberate Failure Proof

Break:

- accessible label,
- input action ref,
- contrast theme token,
- critical audio visual-cue ref,
- 2× layout fixture.

Assert gate fails.

---

# 218. Workstream 184C — Manual / Assistive-Technology Validation

## Goal

Validate what deterministic code tests cannot establish.

---

# 219. 184C Phase BA — Color Vision Simulation Review

For each:

```text
protanopia
deuteranopia
tritanopia
low saturation
```

Review core screens.

---

# 220. 184C Phase BB — Contrast Review

Test major text/control combinations.

Record contrast failures and exemptions.

---

# 221. 184C Phase BC — Screen Reader / Narration Review

Use supported assistive technology or built-in narrator adapter.

Test:

- menus,
- settings,
- inventory,
- shelter status,
- expedition setup,
- modal dialog.

---

# 222. 184C Phase BD — Keyboard-Only Review

No mouse.

Complete representative campaign actions.

---

# 223. 184C Phase BE — Controller-Only Review

No keyboard/mouse if controller is supported.

---

# 224. 184C Phase BF — One-Handed Review

Use preset with one hand/input cluster.

---

# 225. 184C Phase BG — No-Audio Review

Mute audio.

Confirm critical state remains understandable.

---

# 226. 184C Phase BH — Large-Text Review

Use:

```text
2× font
large subtitles
high UI scale
```

---

# 227. 184C Phase BI — Reduced-Motion Review

Review:

- menus,
- combat/alerts,
- disasters,
- map,
- weather,
- transitions.

---

# 228. 184C Phase BJ — Cognitive-Load Review

Evaluate complex management screen.

Ask:

```text
Can player identify highest-priority issue?
Can player find next action?
Can advanced detail still be opened?
```

---

# 229. 184C Phase BK — Accessibility User Testing

Source correctly recommends real-user testing.

Track findings by:

```text
blocking
major
minor
preference
```

Do not claim comprehensive accessibility from automated tests alone.

---

# 230. 184C Phase BL — Feedback Loop

Create issue template:

```text
Accessibility area
Device/input
Setting combination
Panel/context
Expected
Observed
Severity
```

---

# 231. Workstream 184C — Performance and Stability

## Goal

Ensure accessibility modes do not create significant regressions or unstable UI.

---

# 232. 184C Phase BM — Font Scaling Performance

Large text may increase:

- layout passes,
- scrolling,
- node count.

Measure worst-case panels.

---

# 233. 184C Phase BN — Screen Reader Semantics Performance

Accessibility tree should update incrementally.

Do not rebuild whole UI semantic tree every frame.

---

# 234. 184C Phase BO — Visual Alert Performance

Use event-driven alerts.

No polling all audio sources.

---

# 235. 184C Phase BP — Reduced Motion Performance

Should generally reduce visual cost.

Ensure disabling particles does not leak nodes/resources.

---

# 236. 184C Phase BQ — Settings Apply Idempotency

Repeated:

```text
Apply(settings)
```

must not duplicate:

- event subscriptions,
- audio buses,
- input bindings,
- UI wrappers.

---

# 237. 184C Phase BR — Settings Change Stress

Toggle major settings 100 times in test.

Assert:

- no duplicate input mappings,
- no memory growth,
- no stale theme state,
- no focus corruption.

---

# 238. 184C Phase BS — Session Swap Stress

Open/load multiple campaigns while changing accessibility settings.

Preferences remain consistent.

---

# 239. Workstream 184C — Release Documentation

Create:

```text
docs/accessibility/ACCESSIBILITY_SUPPORT.md
```

Include:

- supported options,
- known limitations,
- input devices,
- subtitle/caption coverage,
- motion accommodations,
- screen-reader/narrator support,
- timer accommodation policy.

Do not make unsupported compliance claims.

---

# 240. Player-Facing Accessibility Statement

Prepare release-facing accessibility list based only on shipped/verified features.

Example structure:

```text
Visual
Audio
Controls
Motion
Cognitive
Known limitations
```

No blanket:

```text
fully accessible
```

unless validated to a defined standard.

---

# 241. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| accessibility settings stored per campaign | Medium | High | global user preference authority |
| duplicate Plan 37 rebinding implementation | Medium | High | integrate canonical input map |
| colorblind support relies only on matrix | High | High | semantic palette + shape redundancy |
| high contrast breaks some panels | High | Medium | panel coverage/snapshots |
| 2× font causes clipping | High | High | layout envelope gate |
| visual alerts use screen shake/flashing | Medium | High | static icon/text default |
| screen-reader checkbox has no real semantics | Medium | High | semantic node/narrator contract |
| reduced-density mode hides critical facts | Medium | Critical | invariant tests |
| pause-anywhere breaks timers | Medium | High | timer taxonomy + authority integration |
| time-pressure removal changes world simulation accidentally | Medium | Critical | timer-class policy |
| remapping can strand user | Medium | Critical | essential-action recovery |
| accessibility gets gamified through quests | Medium | Medium | remove diegetic reward hooks |
| settings hot reload duplicates subscriptions | Medium | High | idempotency stress test |
| automated tests overclaim accessibility | High | High | manual/assistive-technology validation |

---

# 242. Commit Strategy

## 184A — Foundation

### C2[39].1 — repository accessibility capability audit

### C2[39].2 — ownership ADR: global preference vs campaign state

### C2[39].3 — settings DTOs / validation / migration

### C2[39].4 — preset/profile data authority

### C2[39].5 — preference-store adapter

### C2[39].6 — Plan 25 / Plan 37 integration adapters

### C2[39].7 — semantic accessibility node / narrator interfaces

### C2[39].8 — startup application / first-launch entry point

### C2[39].9 — diagnostics / coverage registry

### Gate: 184A complete

---

## 184B — Visual

### C2[39].10 — semantic color inventory

### C2[39].11 — color-vision palette modes

### C2[39].12 — shape/icon redundancy

### C2[39].13 — font/UI scaling

### C2[39].14 — high-contrast theme

### C2[39].15 — render color correction

### C2[39].16 — visual snapshot/layout envelope

---

## 184B — Auditory

### C2[39].17 — subtitle coverage / speaker labels

### C2[39].18 — closed-caption cue vocabulary

### C2[39].19 — visual-audio alert layer

### C2[39].20 — audio descriptions / narrator arbitration

### C2[39].21 — mono / per-bus settings integration

---

## 184B — Motor

### C2[39].22 — canonical remapping integration

### C2[39].23 — binding conflict/recovery

### C2[39].24 — hold/toggle / input buffering

### C2[39].25 — one-handed presets

### C2[39].26 — auto-walk / aim-assist applicability closure

---

## 184B — Cognitive / UI

### C2[39].27 — reduced motion

### C2[39].28 — reduced-density UI

### C2[39].29 — objective guidance / tutorial library

### C2[39].30 — pause-anywhere integration

### C2[39].31 — timer accommodation taxonomy

### C2[39].32 — accessibility settings / quick / test panels

### Gate: 184B complete

---

## 184C — Integration / Validation

### C2[39].33 — all-panel settings integration

### C2[39].34 — audio/input/render/pause integration

### C2[39].35 — global persistence / multi-save migration tests

### C2[39].36 — simulation non-interference tests

### C2[39].37 — `--accessibility-selftest`

### C2[39].38 — semantic/focus/color/audio redundancy gates

### C2[39].39 — 2× font + pseudo-locale layout gate

### C2[39].40 — deliberate failure proof

### C2[39].41 — assistive-technology/manual validation

### C2[39].42 — settings stress/performance tests

### C2[39].43 — documentation/release accessibility statement

### Gate: 184C complete

---

# 243. Verification Checklist

Run the source-required commands:

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --accessibility-selftest
```

Also run repository-canonical equivalents of:

```text
panel accessibility coverage gate
input-action/rebinding completeness gate
semantic-name/focus-order gate
critical-audio visual-equivalent gate
semantic-color redundancy gate
2× font + pseudo-locale snapshot gate
reduced-motion registration gate
global-settings multi-save persistence test
simulation non-interference digest test
pause/time-pressure parity test
accessibility settings stress test
```

Manual validation suite:

```text
protanopia simulation review
deuteranopia simulation review
tritanopia simulation review
high-contrast review
screen-reader/narrator review
keyboard-only review
controller-only review
no-audio review
2× text review
reduced-motion review
cognitive-load review
```

---

# 244. `--accessibility-selftest` Required Scenarios

1. default settings load,
2. visual-support preset,
3. hearing-support preset,
4. motor-support preset,
5. cognitive-support preset,
6. custom override after preset,
7. color vision mode validation,
8. 2× font scaling,
9. high-contrast theme application,
10. subtitle size/background,
11. closed-caption cue mapping,
12. visual audio alert,
13. mono audio setting route,
14. input remap,
15. binding conflict,
16. essential binding recovery,
17. hold/toggle,
18. reduced motion,
19. cognitive reduced-density mode,
20. pause-anywhere,
21. time-pressure policy,
22. global preference persistence,
23. campaign A → campaign B preference continuity,
24. legacy settings import,
25. settings reset,
26. semantic accessible labels,
27. no duplicate settings application,
28. old save load.

---

# 245. Deliberate Failure Fixtures

Create fixtures that intentionally:

- omit accessible control name,
- encode critical state color-only,
- omit visual fallback for critical audio cue,
- clip confirm button at 2× text,
- duplicate essential input binding with no recovery,
- register motion effect without reduced-motion path,
- use invalid audio bus ID,
- include unsupported timer class,
- allow campaign save to overwrite global setting.

Each must fail the correct gate.

---

# 246. Accessibility Coverage Matrix

Generate at build/test time:

| Surface | Large text | High contrast | Color redundant | Captions | Visual alerts | Remap/focus | Reduced motion | Semantic labels | Cognitive mode |
|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|

Sources:

- live-routed UI registry,
- audio cue registry,
- input action manifest,
- motion registry.

No hand-maintained spreadsheet as authority.

---

# 247. Release Priority Order

Because the source risk is MEDIUM and scope is broad, implement in impact order.

## Priority 0 — foundational

- global preference storage,
- input/focus integration,
- font scaling,
- subtitles/captions,
- reduced motion,
- high contrast,
- non-color indicators.

## Priority 1

- full remapping,
- visual audio cues,
- mono/per-bus audio,
- cognitive reduced-density,
- pause/time accommodations.

## Priority 2

- screen-reader/narrator depth,
- audio descriptions,
- one-handed advanced presets,
- optional fine-grained visual controls.

Do not ship half-connected checkboxes.

---

# 248. Definition of Done — Foundation

- [ ] AccessibilitySettingsSystem exists,
- [ ] user preference storage exists,
- [ ] campaign save is not accessibility authority,
- [ ] schema versioning/migration exists,
- [ ] effective-settings composition exists,
- [ ] preset profiles exist,
- [ ] custom overrides exist,
- [ ] Plan 25 integration exists,
- [ ] Plan 37 integration exists,
- [ ] semantic UI contract exists,
- [ ] startup application happens before main UI,
- [ ] first-launch accessibility access exists,
- [ ] diagnostics exist.

---

# 249. Definition of Done — Visual

- [ ] protanopia mode,
- [ ] deuteranopia mode,
- [ ] tritanopia mode,
- [ ] shape/icon redundancy,
- [ ] font scaling through 2.0×,
- [ ] UI scaling,
- [ ] high contrast,
- [ ] brightness,
- [ ] contrast,
- [ ] saturation,
- [ ] visible focus,
- [ ] no critical color-only state,
- [ ] representative panel snapshots,
- [ ] manual visual review.

---

# 250. Definition of Done — Auditory

- [ ] all critical speech subtitled,
- [ ] subtitle size,
- [ ] subtitle background,
- [ ] speaker labels,
- [ ] meaningful closed captions,
- [ ] critical audio visual equivalents,
- [ ] audio descriptions for defined scope,
- [ ] per-bus volume,
- [ ] mono audio,
- [ ] no audio-only critical state,
- [ ] muted-audio test.

---

# 251. Definition of Done — Motor

- [ ] full canonical input remapping,
- [ ] keyboard support,
- [ ] mouse support,
- [ ] controller support where game supports it,
- [ ] conflict detection,
- [ ] binding recovery,
- [ ] hold/toggle,
- [ ] input buffering/repeat accommodation,
- [ ] one-handed preset,
- [ ] no essential double-click-only action,
- [ ] no essential drag-only action,
- [ ] keyboard/controller core-flow tests,
- [ ] auto-walk applicability closed,
- [ ] aim-assist applicability closed.

---

# 252. Definition of Done — Cognitive

- [ ] reduced motion,
- [ ] no required information lost,
- [ ] reduced-density UI,
- [ ] objective tracking,
- [ ] persistent tutorial/help,
- [ ] pause-anywhere policy,
- [ ] timer classification,
- [ ] time-pressure accommodations where safe,
- [ ] alert grouping,
- [ ] destructive-action clarification where useful,
- [ ] cognitive-load review.

---

# 253. Definition of Done — Integration / Validation

- [ ] every release panel covered,
- [ ] theme integration,
- [ ] layout integration,
- [ ] audio integration,
- [ ] input integration,
- [ ] render integration,
- [ ] pause/time integration,
- [ ] tutorial/objective integration,
- [ ] global persistence,
- [ ] multi-save continuity,
- [ ] old-save migration,
- [ ] simulation non-interference,
- [ ] expected timing exceptions documented,
- [ ] settings apply idempotently,
- [ ] settings stress test,
- [ ] `--accessibility-selftest`,
- [ ] deliberate failure fixtures,
- [ ] manual assistive-technology review,
- [ ] accessibility support documentation,
- [ ] release-facing verified feature list.

---

# 254. Global Definition of Done

- [ ] no duplicate input-remapping system,
- [ ] no duplicate theme/audio/subtitle authority,
- [ ] no campaign-owned accessibility preference truth,
- [ ] no unsafe master “accessibility off” toggle,
- [ ] no gamified accessibility quests/rewards,
- [ ] no critical color-only state,
- [ ] no critical audio-only state,
- [ ] no required motion-only state,
- [ ] no essential mouse-only interaction,
- [ ] no stranded input configuration,
- [ ] no 2× text critical clipping,
- [ ] no hidden simulation advantage from presentation settings,
- [ ] no unsupported compliance claim,
- [ ] full verification green.

---

# 255. Closure Report Template

```markdown
## C2[39] Closure Report

### Repository
- Start commit:
- End commit:
- Branch:

### Baseline Audit
- Existing text scale:
- Existing input remapping:
- Existing controller navigation:
- Existing subtitle system:
- Existing audio bus controls:
- Existing reduced-motion hooks:
- Existing accessible labels:
- Plan 25 overlap:
- Plan 37 overlap:

### 184A — Foundation
- Settings system:
- Preference store:
- Campaign migration:
- Presets:
- Custom overrides:
- Validation:
- Semantic UI:
- Narrator:
- Startup application:
- Missing sinks:
- Result:

### Visual
- Protanopia:
- Deuteranopia:
- Tritanopia:
- Shape redundancy:
- Font scale max:
- UI scale max:
- High contrast:
- Color correction:
- Focus visibility:
- 2× layout failures:
- Result:

### Auditory
- Subtitle coverage:
- Closed-caption coverage:
- Visual-alert coverage:
- Audio descriptions:
- Mono:
- Bus controls:
- Muted-audio failures:
- Result:

### Motor
- Remappable actions:
- Unremappable justified actions:
- Binding conflicts:
- Recovery binding:
- Hold/toggle:
- One-handed preset:
- Keyboard-only test:
- Controller-only test:
- Result:

### Cognitive
- Reduced motion:
- Motion effects without fallback:
- Reduced-density panels:
- Objective tracking:
- Tutorial persistence:
- Pause contexts:
- Timer classes:
- Time accommodations:
- Result:

### Persistence / Non-Interference
- Global preference save:
- Campaign A → B continuity:
- Legacy import:
- Reset:
- Settings apply duplicate subscriptions:
- Simulation digest differences:
- Expected timing-assist differences:
- Result:

### Manual Validation
- Color-vision review:
- Contrast review:
- Narrator/screen-reader review:
- No-audio review:
- 2× text review:
- Reduced-motion review:
- Cognitive-load review:
- User testing:
- Result:

### Verification
- Core build:
- Core tests:
- Host build:
- Data integrity:
- Accessibility selftest:
- Panel coverage:
- Input/focus gate:
- Audio redundancy gate:
- Color redundancy gate:
- 2× pseudo-locale gate:
- Settings stress:
- Result:

### Final Metrics
- ACCESSIBILITY_PROFILES:
- LIVE_UI_PANELS:
- ACCESSIBILITY_COVERED_PANELS:
- UNNAMED_INTERACTIVE_CONTROLS:
- COLOR_ONLY_CRITICAL_STATES:
- AUDIO_ONLY_CRITICAL_CUES:
- MOTION_EFFECTS_WITHOUT_REDUCED_PATH:
- REMAPPABLE_ACTIONS:
- ESSENTIAL_BINDING_RECOVERY_FAILURES:
- FONT_2X_CLIPPING_FAILURES:
- GLOBAL_SETTINGS_PERSISTENCE_FAILURES:
- SIMULATION_NONINTERFERENCE_FAILURES:
- REQUIRED_ACCESSIBILITY_SINKS_MISSING:

### Remaining Debt
- Screen reader:
- Audio descriptions:
- Specialized input devices:
- Fine-grained typography:
- User testing:
- Store/platform requirements:
```

---

# 256. Final Execution Directive

Execute Plan 184 as a **global accessibility-preference and adaptation layer over existing UI, input, audio, theme, render, help, and time-control authorities**.

The critical sequence is:

```text
audit existing accessibility-adjacent work
→ establish global user preference ownership
→ integrate Plan 25 text/localization infrastructure
→ integrate Plan 37 input/focus/rebinding infrastructure
→ implement visual/auditory/motor/cognitive settings
→ expose settings before campaign start
→ apply them through explicit subsystem adapters
→ validate every live-routed panel/cue/action
→ prove global persistence and simulation non-interference
→ perform real assistive-technology/manual review
→ publish only verified accessibility claims
```

Do not store accessibility preferences as campaign gameplay truth.

Do not create another input-remapping engine.

Do not make a global color matrix the only colorblind solution.

Do not use screen shake as a visual substitute for sound.

Do not create a master accessibility-off switch that disables accommodations.

Do not gamify accessibility configuration with campaign quests or rewards.

The strongest ownership rule is:

> **AccessibilitySettingsSystem owns the player's preferences; UI, audio, input, render, and time systems remain responsible for applying those preferences within their own domains.**

The strongest presentation rule is:

> **No critical information may depend on color alone, sound alone, motion alone, hover alone, or one precise input method.**

The strongest persistence rule is:

> **Accessibility follows the player across campaigns and saves; loading a campaign must never silently reset or override the player's chosen accommodations.**

The flagship acceptance scenario is:

> **Launch ASHFALL with no campaign loaded and configure 2× text, high contrast, deuteranopia support, closed captions, visual audio cues, reduced motion, a one-handed input preset, and reduced cognitive density. Start a campaign, navigate the shelter status, inventory, expedition setup, map, radio, and settings without a mouse, mute all audio, and verify every critical alert remains available visually/textually. Pause during an active high-pressure situation, change font scale and motion settings, and confirm zero simulation ticks advance while paused. Save the campaign, load a different campaign, and verify the same accessibility preferences remain active. Then compare deterministic campaign state against a default-presentation run: presentation-only accessibility changes must not alter simulation state. Finally run the 2× text + pseudo-locale layout gate, color/audio redundancy gates, accessible-name/focus gate, and `--accessibility-selftest`, followed by manual color-vision and assistive-narration review.**
