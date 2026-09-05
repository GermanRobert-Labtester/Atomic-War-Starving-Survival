# Localization — Store Locale Packs Ticket

**Audit:** local post-PR #36 issue **#42**
**Date:** 2026-09-05
**Status:** Dispositioned — English + QA pseudo only until store pack lane

## Current surface

- `SettingsPanel` language options: `English (US)` and `[QA] Pseudo-Locale (Expanded)`
- `UserSettingsData.Locale` allows `"en"` and `"pseudo"`
- No real second-language packs shipped under `assets/` / TranslationServer

## Activation criteria (LOC-01)

1. Inventory user-facing strings (UI + data prose) via `ashfall-string-extractor`.
2. Choose first real locale (store requirement), not a fictional nation name.
3. Add Godot TranslationServer wiring + CSV/PO pipeline.
4. Extend Settings options beyond en/pseudo with fallback to English.
5. Gate missing keys for the new locale in CI.
6. Close this ticket.

**Owner lane:** Release / localization
**Expiry review:** pre-store release checklist
