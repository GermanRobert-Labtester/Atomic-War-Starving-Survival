# ASHFALL — UI Accessibility & Readability Baseline Audit

**Audit Reference:** Plan 14 Task 14B / `ashfall-ui-access`
**Canvas Target:** 1920×1080 fixed viewport, supporting 100%, 125%, 150% UI scale
**Typography:** BarlowCondensed (Regular, SemiBold, Bold), ShareTechMono (Regular)
**Palette:** Ashfall Design Theme (Ink, Warm Amber, Pale Bone, Muted Grey, Critical Red, Entropy Orange, Lethe Cyan, Success Green)

---

## 1. Static Accessibility & Contrast Baseline

### 1.1 Contrast Ratio Analysis (WCAG 2.1 AA Target: 4.5:1 for body, 3.0:1 for large text)

| Token Pair | Foreground Hex | Background Hex | Contrast Ratio | WCAG AA Status | Remediation / Usage Notes |
|---|---|---|:---:|:---:|---|
| **Pale on Ink** (Primary Body) | `#E6E0D2` | `#090B0C` | **15.2:1** | **PASS (AAA)** | Primary narrative and data body text. |
| **Warm on Ink** (Primary Accent) | `#D3AA62` | `#090B0C` | **9.8:1** | **PASS (AAA)** | Section titles, highlighted metrics. |
| **Hot on Ink** (Emphasis) | `#F4C875` | `#090B0C` | **13.6:1** | **PASS (AAA)** | Active button borders and crucial stats. |
| **Muted on Ink** (Secondary Text) | `#938F84` | `#090B0C` | **6.4:1** | **PASS (AA)** | Subheaders, descriptions, secondary values. |
| **Dim on Ink** (Metadata / Tertiary) | `#66675F` | `#090B0C` | **3.6:1** | **PASS (Large / Non-Body)** | Used strictly for metadata badges & disabled states. Floor set to 11px. |
| **Critical on Ink** (Urgent Danger) | `#E63333` | `#090B0C` | **4.6:1** | **PASS (AA)** | Danger indicators. Paired with `[!]` symbol and explicit text. |
| **Success on Ink** (Resolved State) | `#5CD670` | `#090B0C` | **11.4:1** | **PASS (AAA)** | Safe conditions, completed directives. |
| **Radiation on Ink** (Hazard Warning) | `#D9A026` | `#090B0C` | **8.8:1** | **PASS (AAA)** | Radiation dose readouts, paired with `[RAD]` icon. |

---

## 2. Typography Hierarchy & Font-Size Floors

To prevent illegible micro-text in condensed fonts:

| Role | Font Face | Target Size | Floor Rule | Usage |
|---|---|:---:|:---:|---|
| **Display Header (H1)** | BarlowCondensed SemiBold | 30px | Min 28px | Main titles, modal headings |
| **Section Header (H2)** | BarlowCondensed SemiBold | 24px | Min 20px | Major panel sections, tab headers |
| **Subsection (H3)** | BarlowCondensed SemiBold | 19px | Min 16px | Card headers, table column headers |
| **Primary Body** | BarlowCondensed Regular | 15px | **Min 13px** | Narrative copy, item descriptions, dialog |
| **Telemetry / Data Mono** | ShareTechMono Regular | 13px | **Min 12px** | Dosimeter readings, inventory counts, coordinates |
| **Small / Helper Text** | BarlowCondensed Regular | 12px | **Min 12px** | Button subtitles, status rail descriptions |
| **Metadata / Micro-Badge** | BarlowCondensed Regular | 11px | **Min 11px** | Category tags, timestamps, secondary labels |

*Zero text elements in the entire UI are allowed below 11px.*

---

## 3. Redundant Semantic Channels (No Color-Only State)

Every critical survival state in ASHFALL communicates across multiple independent visual channels:

| Semantic State | Color Token | Icon / Symbol | Shape / Border Marker | Text Label | Grayscale Legibility |
|---|---|:---:|:---:|---|:---:|
| **Critical Danger / Sickness** | `#E63333` (Critical) | `[!]` / `▲` | Thick Solid Border (2px) | "CRITICAL" / "ACUTE" | **HIGH** (distinct bold shape + label) |
| **Radiation Exposure / Hazard** | `#D9A026` (Radiation) | `[RAD]` / `☢` | Striped Pattern / Badge | "ELEVATED DOSE" / "STORM" | **HIGH** (distinct badge icon) |
| **Resource Depleted / Starvation** | `#E65C2B` (Hot Red) | `[EMPTY]` / `Ø` | Dashed Frame Box | "DEPLETED" / "STARVING" | **HIGH** (dashed box + badge) |
| **Operational / Normal** | `#5CD670` (Success) | `[OK]` / `✔` | Subtle Border (1px) | "STABLE" / "ACTIVE" | **HIGH** (standard box + status text) |
| **Action Disabled / Blocked** | `#66675F` (Dim) | `[X]` / `—` | Muted Flat Background | Reason: "Missing 1 Filter" | **HIGH** (explicit reason text) |

---

## 4. UI Scaling & Viewport Behavior

1. **Supported Scale Factors:** `1.0× (100%)`, `1.25× (125%)`, `1.5× (150%)`.
2. **Fixed Viewport Architecture:** The game renders onto a standard 1920×1080 canvas (`gl_compatibility` / Forward+ with `canvas_items` stretch mode).
3. **Responsive Container Bounds:**
   - All high-traffic panels (Survivors, Medical, Inventory, Map, Settings) use `ScrollContainer` with `SizeFlagsVertical = ExpandFill` and `WordSmart` autowrap.
   - At 150% UI scale, modal dialogs remain centered within safe viewport bounds with minimum 24px margins to screen edges.
   - No interactive button or critical text is clipped or pushed outside the viewport at 150% scale.

---

## 5. Keyboard & Controller Traversal & Focus Invariants

1. **Predictable Focus Entry:** Opening any panel immediately focuses the first actionable control or close button.
2. **Focus Visibility:** Focused controls receive a high-contrast Hot Amber border highlight (`#F4C875`) and distinct background tint (`#282319`).
3. **Zero Focus Traps:** Modal dialogs, tutorial overlays, and briefing windows provide clear Escape/Cancel or Enter key handlers that cleanly dismiss the overlay and restore focus to the originating control.
4. **Directional Traversal:** D-Pad / Arrow keys traverse grids, tab bars, and item lists deterministically.
