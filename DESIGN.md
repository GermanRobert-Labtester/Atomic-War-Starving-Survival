---
name: ASHFALL Tactical UI
colors:
  surface: '#1a1a1a'
  surface-dim: '#141414'
  surface-bright: '#242424'
  surface-container-lowest: '#0a0a0a'
  surface-container-low: '#121212'
  surface-container: '#1a1a1a'
  surface-container-high: '#2a2a2a'
  surface-container-highest: '#383838'
  on-surface: '#e0e0e0'
  on-surface-variant: '#a0a0a0'
  inverse-surface: '#e0e0e0'
  inverse-on-surface: '#121212'
  outline: '#4d4d4d'
  outline-variant: '#2a2a2a'
  primary: '#c7dcd0'
  on-primary: '#101a14'
  primary-container: '#1f2824'
  on-primary-container: '#c7dcd0'
  inverse-primary: '#1f2824'
  secondary: '#ff6b35'
  on-secondary: '#2e0f02'
  secondary-container: '#541c04'
  on-secondary-container: '#ff6b35'
  error: '#ff3333'
  on-error: '#330000'
  error-container: '#550000'
  on-error-container: '#ff9999'
  background: '#0a0a0a'
  on-background: '#e0e0e0'
typography:
  display-lg:
    fontFamily: BarlowCondensed
    fontSize: 48px
    fontWeight: '700'
    lineHeight: '1.1'
    letterSpacing: 2px
  headline-md:
    fontFamily: BarlowCondensed
    fontSize: 28px
    fontWeight: '600'
    lineHeight: '1.2'
    letterSpacing: 1px
  body-base:
    fontFamily: ShareTechMono
    fontSize: 14px
    fontWeight: '400'
    lineHeight: '1.4'
  label-caps:
    fontFamily: ShareTechMono
    fontSize: 12px
    fontWeight: '700'
    lineHeight: '1'
    letterSpacing: 2px
spacing:
  unit: 4px
  gutter: 16px
  margin-page: 32px
  pane-padding: 16px
---

## Brand & Style
ASHFALL is a 2D atomic-war survival management game. The UI must reflect a cold, exhausted, human, restrained, material, and bureaucratic tone. The visual identity should be utilitarian, slightly analog, and devoid of gloss or modern padding. It is a world of scarcity.
- **Gritty Utility:** Panels resemble clipboard manifests, CRT terminal screens, or metal-stamped ledgers.
- **Scarcity in Design:** Minimalist, data-dense, monochromatic with sparse but striking color accents (warning orange).

## Colors
- **Backgrounds:** Deep charcoals and flat blacks.
- **Primary Text & Accents:** A washed-out, sickly CRT green-white (`#c7dcd0`).
- **Alerts & Warnings:** A rusted, desaturated hazard orange (`#ff6b35`). Avoid bright reds unless for absolute failure states.
- **Borders:** Hard, 1px solid or dashed borders. No soft dropshadows, only sharp high-contrast drop-shadows or block backgrounds.

## Typography
- **BarlowCondensed:** Used strictly for large headers, panel titles, and prominent metrics. Should always be ALL CAPS.
- **ShareTechMono:** The workhorse font for all body text, lists, and button labels. Represents the bureaucratic data entry feel.

## Components & Layout
- **Panels/Cards:** Flat rectangles with strict 1px borders. Use clipped corners (chamfers) occasionally to simulate industrial casing.
- **Buttons:** Solid, blocky, often utilizing an outlined state. Hover states simply invert the colors.
- **Data Tables:** Dense arrays of information separated by thin 1px dashed borders.
- **Progress Bars:** Segmented blocks `[||||   ]` instead of continuous smooth fills.

## Global Constraints
- Target resolution: Fixed 1920x1080 (DESKTOP layout).
- No round corners (border-radius: 0).
- Keep everything aligned to a strict grid.
