# Plan 14 — UX, Onboarding & Accessibility: The First Hour and the Thousandth

> **Theme:** 265 UI files, a fixed 1920×1080 viewport, 69 golden snapshots — and known gaps in
> onboarding, accessibility, and information clarity. This plan makes the game *learnable* and
> *readable* without touching a single survival mechanic.
>
> **Key evidence:** `docs/HoldfastManualPlaytest.md`, `docs/ui/`, 69 snapshot targets;
> skills `ashfall-tutorial-review`, `ashfall-ui-access`, `ashfall-input-map-audit`,
> `ashfall-string-extractor` exist because these are known needs.

---

## Task 14A — First-hour onboarding & tutorial review

**Goal:** Audit and fix the first hour so a new player understands needs, radiation, and the
daily tick before the game punishes them for not knowing.

**Files:** `src/UI/` TutorialPanel + HUD overlays, starting-state data, read-only
`docs/HoldfastManualPlaytest.md`, `NeedsSystem`, `RadiationSystem`.

**Substeps:**
1. Run skill `ashfall-tutorial-review` to produce the evidence report: what the first hour demands vs. what it teaches.
2. Play/trace the first 10 ticks headless (`ashfall-telemetry-playtest`) harvesting need/morale/radiation KPIs to find the first unavoidable failure.
3. Inventory TutorialPanel content vs. the 8 core needs + radiation + power/water triage.
4. Identify the 3 teach-vs-demand gaps (things demanded before taught).
5. Author/fix tutorial steps to close those 3 gaps (teach radiation dosimeter reading, teach ration policy, teach power triage).
6. Add contextual first-time hints (first fallout storm, first sickness, first expedition) as dismissible overlays.
7. Ensure tutorial state persists (dismissed stays dismissed) via existing save pattern.
8. Adjust starting inventory/grace so the first hour is tense but survivable (data-only).
9. Snapshot-diff the tutorial/HUD panels; approve deliberately.
10. Re-run telemetry playtest → first-hour funnel shows no unavoidable early death.

**Next steps:** a "field manual" codex tab (uses JournalCodex); difficulty presets for grace period.

---

## Task 14B — Accessibility & readability sweep

**Goal:** Bring the fixed-1080p UI up to accessibility baseline: contrast, overflow, scaling,
keyboard navigation, and text readability across all 265 UI files.

**Files:** `src/UI/` broadly, `project.godot` (display/input), theme resources; output report
to `docs/ui/ACCESSIBILITY_AUDIT.md`.

**Substeps:**
1. Run `ashfall-ui-access` to generate the evidence audit (contrast, overflow, scaling, keyboard nav).
2. Fix contrast failures in the theme (the two fonts: BarlowCondensed + ShareTechMono at small sizes are the likely offenders).
3. Fix text overflow on the highest-traffic panels (Survivors, Medical, Inventory, Map) at default and scaled sizes.
4. Add UI scale setting (100/125/150%) if absent; verify fixed-viewport handling.
5. Audit keyboard/controller navigation: tab order, focus indicators, panel shortcuts.
6. Add colorblind-safe treatment to danger/radiation indicators (icon + color, never color alone).
7. Add font-size floor for critical readouts (dosimeter, health).
8. Run `ashfall-input-map-audit` for binding conflicts and rebind gaps.
9. Re-render + snapshot-diff all 69 golden panels; review every diff visually.
10. Document baseline + fixes in `ACCESSIBILITY_AUDIT.md`; add a contrast regression check if tooling allows.

**Next steps:** localization prep (14C) benefits directly; screen-reader/one-hand play is out
of scope for 2D management but document the stance.

---

## Task 14C — Localization readiness & string extraction

**Goal:** Prepare the game for translation: inventory hardcoded user-facing strings, scaffold
the Godot translation layer, and gate new hardcoded strings.

**Files:** `src/UI/` + `.tscn` + `Assets/StreamingAssets/Data/*.json` (user-facing text),
`project.godot` (locale), new translation scaffolding; output CSV/POT inventory.

**Substeps:**
1. Run `ashfall-string-extractor` to inventory hardcoded strings across UI, scenes, and data JSON.
2. Categorize: UI labels, tutorial text, event/quest prose, item descriptions, journal/radio text.
3. Decide the localization boundary: UI labels + structured text localizable; the 272 narrative docs are a separate, later translation project (note it).
4. Scaffold Godot `TranslationServer` integration + a `res://` locale resource structure.
5. Extract UI labels + structured strings into CSV/POT with stable keys.
6. Replace hardcoded UI strings with keyed lookups (panel by panel, small batches).
7. Add a locale switcher to settings (English source only to start).
8. Add a CI gate flagging new hardcoded user-facing strings in `src/UI`.
9. Verify data-JSON text fields use a consistent, extractable pattern (they're content, translated separately).
10. `dotnet build` + snapshot-diff (keyed lookups must render identically in source locale).

**Next steps:** actual translation is a content project post-release-candidate; RTL/text-expansion
UI stress test with a pseudo-locale.
