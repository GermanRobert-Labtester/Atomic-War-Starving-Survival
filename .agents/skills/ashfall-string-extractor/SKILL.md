---
name: ashfall-string-extractor
description: Extracts hardcoded user-facing strings from src/UI, .tscn, and StreamingAssets/Data JSON into CSV/POT, scaffolds Godot TranslationServer, and gates new hardcoded strings in CI. Use when adding UI, dialogue, or preparing store release.
---

# ASHFALL String Extractor

## ROLE
`ashfall-localize` bootstraps the translation layer; you do the extraction. ASHFALL has 207-file UI tree + 280 JSON authority files + rich diegetic prose — every user-facing literal must be keyed, not hardcoded, before store release.

## RULES
1. Never hand-edit extracted CSV/POT as source — keys derive from `tr()`/`TranslationServer` lookups, diegetic JSON via `loc_key`.
2. `snake_case` keys with domain prefix (`ui_`, `dialog_`, `item_`, `quest_`, `radio_`, `tutorial_`).
3. CI gate: new hardcoded literals outside `TranslationServer` path fail PR.

## WORKFLOW
### PHASE 1 — Extract
- `src/UI/**/*.cs`: regex `Tr("...")`, `TranslationServer.Translate`, plus bare `Label.Text = "..."` / `Button.Text = "..."` misses.
- `**/*.tscn`: `[node name="Label" ... text="..."]` and `placeholder_text`.
- `Assets/StreamingAssets/Data/**/*.json`: `name`, `description`, `flavor`, `transmission`, `article_body` fields.
- Emit `assets/l10n/strings.csv` (key, en, file:line) and `assets/l10n/template.pot`.

### PHASE 2 — Scaffold / Diff
- If `assets/l10n/` missing, scaffold `TranslationServer` autoload + `ProjectSettings` `locale/translations` entries (as `ashfall-localize` prescribes).
- Diff against previous extraction: new keys, orphan keys (code deleted but key remains), changed en copy.

### PHASE 3 — Gate
- Fail if any `src/**/*.cs` contains `\.Text\s*=\s*"` literal not wrapped in `Tr()` outside test fixtures.
- Warn on duplicate en values with different keys (merge candidate).

### PHASE 4 — Verify
- `godot --headless --path . -- --data-integrity-selftest` 0 errors (JSON keys still resolve)
- `dotnet build Ashfall.csproj` 0 warnings

## OUTPUT
`docs/l10n/STRING_EXTRACTION_REPORT.md` — counts, per-domain table, new/orphan/duplicate keys, CI gate status, next translator handoff.

## QUALITY GATE
- 0 unwrapped literal assignments in `src/UI/`, 0 `.tscn` with inline `text` outside `tr_key`, POT/CSV in sync.
