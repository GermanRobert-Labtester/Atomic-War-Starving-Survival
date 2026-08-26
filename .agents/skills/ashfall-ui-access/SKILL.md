---
name: ashfall-ui-access
description: Audits ASHFALL's fixed 1920x1080 UI for accessibility and usability — contrast, overflow, scaling, keyboard navigation, readability — across the 207-file UI tree, with evidence-based findings.
---

# ASHFALL UI Accessibility & Usability Auditor

## ROLE

ASHFALL is a dense survival-management game on a fixed 1920×1080 canvas with a 207-file UI tree (`src/UI/`), heavy text load (ledgers, rosters, dose registers), and fonts BarlowCondensed + ShareTechMono. You audit whether players can actually read, navigate, and operate it.

## WORKFLOW

### PHASE 1 — Surface Map
- Inventory panels/HUDs in `src/UI/` and `Main.UiPanels.cs`/`Main.UiHandlers.cs`; note input handling paths (mouse-only vs keyboard-wired).
- Identify high-density screens: trade ledgers, duty rosters, medical triage, dose ledger, journal.

### PHASE 2 — Static Checks
- Theme inspection: extract color pairs (text on background) from the Godot theme; compute WCAG contrast ratios; flag < 4.5:1 for body text, < 3:1 for large text.
- Font readability: minimum sizes in use vs theme defaults (14px base — flag anything below readable threshold).
- Overflow risk: fixed-size containers + long localized-ready text sources (item names, survivor epithets) — simulate long strings where fixtures exist.

### PHASE 3 — Interaction Checks
- Keyboard navigation: tab order, focus visibility, hotkey collisions across panel stacks (PanelLifecycle).
- Mouse-target sizes for interactive elements; hover/click state feedback presence.
- Panel layering: can critical alerts be obscured by modal stacks?

### PHASE 4 — Evidence Capture
- Where snapshot captures exist (`snapshots/`), annotate findings on the actual renders; otherwise cite theme/code locations with file:line.

## RULES
- Read-only audit: findings and ranked fix proposals, not UI edits (hand to repair/implement skills).
- Headless where possible; no editor dependency for static analysis.
- Prioritize by player impact: unreadable > unnavigable > inconvenient.

## OUTPUT
`docs/ui/ACCESSIBILITY_REPORT.md` — contrast table, overflow risks, navigation findings, severity-ranked fixes with evidence.

## QUALITY GATE
- Every finding has evidence (color values, file:line, or snapshot reference).
- Ranked fix list is actionable without rediscovery.
