# ASHFALL — ACCESSIBILITY ARCHITECTURE & STANDARDS (PLAN 80 / TASK B21)

**Classification:** Core UX & Accessibility Authority
**Author:** AI Pair Programmer / Antigravity
**Status:** Implemented & CI-Gated
**Enforcement Test:** `Ashfall.Core.Tests/UI/AccessibilitySourceAuditTests.cs`

---

## 1. Executive Summary

ASHFALL is designed to deliver a rigorous, immersive post-nuclear survival simulation that is fully navigable by keyboard, contrast-safe under WCAG guidelines, and free of photosensitive or disorienting visual effects.

This document records the foundational accessibility guarantees introduced in **Plan 80 (Task B21)**, including the centralized **Focus Policy**, **Typographic Floors**, **WCAG Color Contrast Ratios**, **Keyboard Navigation Model**, and our **honest statement of assistive technology limitations**.

---

## 2. Centralized Focus Policy (`AshfallFocusPolicy`)

All overlay panels, modals, and interactive dialogs conform to a unified focus lifecycle implemented in `src/UI/AshfallFocusPolicy.cs`:

1. **Deterministic Initial Focus (`OpenWithFocus`)**:
   - When an overlay or modal opens, keyboard focus is immediately and deterministically acquired by the primary action or first interactive control.
   - The opening control (e.g., navigation button in `GameDashboardPanel`) is remembered via internal metadata (`_ashfall_focus_opener`).
2. **Modal Focus Trap (`TrapFocus`)**:
   - When an overlay is active, `Tab` and `Shift+Tab` events are intercepted and constrained within the container's interactive elements.
   - Focus can never invisibly leak to background dashboard buttons or obscured HUD elements.
3. **Visible Focus Rings (`MakeFocusVisibleStyleBox`, `ApplyFocusVisibleStyle`)**:
   - Every interactive control (`Button`, `LineEdit`, `AshfallDataGrid` rows, etc.) features a high-contrast 2px amber focus border (`Theme.Hot` / `#F4C875`) with subtle translucent fill and sharp brutalist corners.
   - Focus indicator contrast against the `#090B0C` background exceeds **12:1**.
4. **Focus Restoration on Dismissal (`RestoreFocus`)**:
   - Closing any panel via `Escape`, `ashfall_close`, or the on-screen close button immediately returns focus to the initiating control.
   - The player's spatial context on the dashboard navigation rail is strictly preserved.

---

## 3. Keyboard Navigation Model

All core gameplay operations can be executed entirely without mouse input:

| Input / Verb | Scope | Default Key | Function |
|---|---|---|---|
| `ashfall_close` / `ui_cancel` | Global / Overlay | `Escape` | Dismisses active overlay, closes top modal on stack, restores focus |
| `ui_focus_next` | Active Panel | `Tab` | Cycles focus forward to the next interactive control |
| `ui_focus_prev` | Active Panel | `Shift + Tab` | Cycles focus backward to the previous interactive control |
| `ui_up` / `ui_down` | Grids / Lists | `Up` / `Down` Arrow | Moves selection between table rows or scrollable items |
| `ui_accept` / `ashfall_confirm` | Focused Control | `Enter` / `Space` | Activates focused button or commits highlighted table row selection |

---

## 4. Color Contrast Authority & WCAG Analysis

Color tokens are governed exclusively by `Assets/Ashfall.Core/UI/Theme.cs`. Contrast ratios against our primary dark background token (`Theme.Ink` `#090B0C`, relative luminance ~0.002) are mechanically verified in `ThemeSemanticTokensTests.cs`:

| Token | Hex | Relative Luminance | Contrast Ratio (vs Ink) | WCAG Standard Met | Usage |
|---|---|---|---|---|---|
| **Pale** | `#C7DCD0` | 0.672 | **13.8:1** | **WCAG AAA** (>= 7.0:1) | Primary body text, main readouts |
| **Hot** | `#F4C875` | 0.602 | **12.5:1** | **WCAG AAA** (>= 7.0:1) | Primary highlights, focus rings, urgent telemetry |
| **Warm** | `#D3AA62` | 0.428 | **9.2:1** | **WCAG AAA** (>= 7.0:1) | Secondary highlights, section headers, accents |
| **Muted** | `#938F84` | 0.274 | **6.2:1** | **WCAG AA** (>= 4.5:1) | Secondary body text, timestamps, passive hints |
| **Critical** | `#FF4D4D` | 0.268 | **6.1:1** | **WCAG AA** (>= 4.5:1) | Danger alerts, radiation acute warnings, zero HP |
| **Dim** | `#7E827A` | 0.217 | **5.1:1** | **WCAG AA** (>= 4.5:1) | Disabled controls, tertiary metadata (exceeds 3.0:1 floor) |
| **Warning** | `#FF6B35` | 0.285 | **6.4:1** | **WCAG AA** (>= 4.5:1) | Scarcity warnings, radiation caution, mechanical alerts |
| **Success** | `#7CD3A2` | 0.548 | **11.5:1** | **WCAG AAA** (>= 7.0:1) | Successful crafting, healed conditions, green status |

---

## 5. Typographic Scale & Minimum Font Floors

To prevent illegible micro-text, ASHFALL enforces strict typographic minimums in `Assets/Ashfall.Core/UI/Theme.cs` and checks them via `AccessibilitySourceAuditTests.cs`:

- **Title / H1 (`Theme.FontSizeH1`)**: 22px
- **Section Header / H2 (`Theme.FontSizeH2`)**: 18px
- **Body Text Floor (`Theme.FontSizeBody`)**: **15px** (never below 14px)
- **Monospace Telemetry Floor (`Theme.FontSizeMono`)**: **13px**
- **Small Metadata Floor (`Theme.FontSizeSmall`)**: **12px**
- **Label Floor (`Theme.FontSizeLabel`)**: **11px**
- **Absolute Global Floor**: **No font size < 11px may exist in production code.**

---

## 6. Motion & Visual Disruption Posture

1. **No Strobe or Rapid Flashing**:
   - Zero high-frequency strobing, flashing screen overlays, or pulsating lights exist in any UI panel.
2. **Reduced Motion Compatibility**:
   - UI panel animations are restrained and fast (fade/slide <= 150ms).
   - Screen-shake during catastrophe events is clamped and respects the game's display settings.

---

## 7. Known Assistive Technology Limitations

While ASHFALL achieves keyboard completeness, focus trapping, high contrast, and font minimums, users should note the following current engine-level limitation:

> [!NOTE]
> **Screen Reader Support**: Godot Engine 4.x currently lacks native integration with platform screen-reading accessibility APIs (such as AT-SPI on Linux, Microsoft UI Automation on Windows, or NSAccessibility on macOS) for custom GUI nodes. As a result, software like NVDA, JAWS, or Orca cannot currently read out in-game text automatically. We will re-evaluate native screen-reader integration as Godot's upstream accessibility framework matures.
