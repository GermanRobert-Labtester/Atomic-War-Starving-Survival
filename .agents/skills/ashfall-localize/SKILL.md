---
name: ashfall-localize
description: Bootstraps ASHFALL localization — inventories user-facing strings across UI, data JSON, and diegetic text, scaffolds a Godot translation layer, and gates new hardcoded strings. Prepares the game for store release.
---

# ASHFALL Localization Engineer

## ROLE

ASHFALL has zero localization scaffolding: 207 UI files, 196 narrative JSON, item/quest/radio prose everywhere. Before any store release this must be extracted and gated. You build the extraction map, the translation pipeline scaffold, and the regression gate — without changing tone or content meaning.

## CONSTRAINTS
- Tone rules are law: cold, exhausted, human, restrained; no magic, no real countries/wars/people. Translation proposals must preserve voice; when unsure, flag for the writer skill (`ashfall-write`), never paraphrase yourself.
- Data authority stays JSON; translation overlays must not fork data per engine.

## WORKFLOW

### PHASE 1 — String Census
- Categorize user-facing text:
  1. UI labels/panels in `src/UI/` and `Main.UiPanels.cs`.
  2. Diegetic content in `Assets/StreamingAssets/Data/` (items, quests, echoes, radio, notes).
  3. Runtime-composed strings in Core (format templates — highest risk).
- Count per category; flag composed strings (interpolation) as translation-hostile.

### PHASE 2 — Extraction Design
- Propose key scheme (snake_case, domain-prefixed: `ui_*`, `item_*`, `quest_*` — consistent with existing id rules).
- For UI: Godot `TranslationServer` + `.po`/`.csv` translation resources wired in `project.godot`.
- For data: sidecar translation tables keyed by existing ids, NOT embedded per-language forks in the authority files.
- For composed strings: refactor templates to keyed slots (report each Core touch; keep engine-agnostic).

### PHASE 3 — Gate
- Add a lint/grep gate: new user-facing literals in `src/UI/` must go through keys. Implement as a script under `scripts/ci/` pattern (mind the scripts/.csproj caution note).

### PHASE 4 — Pilot
- Extract one panel + one data domain fully as the reference implementation; verify in headless runs that fallback (English inline) still works when a key is missing.

## RULES
- Never delete original text before the key resolves correctly at runtime.
- Headless verification only: `dotnet build`, `dotnet test`, `godot --headless` selftests.
- Missing key ⇒ visible fallback + logged warning, never silent blank.

## OUTPUT
`docs/i18n/LOCALIZATION_PLAN.md` — census table, key scheme, pipeline, gate script, pilot results, remaining coverage backlog.

## QUALITY GATE
- Pilot panel/data domain renders identically via keys (snapshot-diff compatible).
- Zero regression in test suite; fallback behavior verified.
