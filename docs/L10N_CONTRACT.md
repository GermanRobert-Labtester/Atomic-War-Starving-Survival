# ASHFALL localization contract

## Wave 1 scope

Wave 1 localizes `ResearchPanel`, `OnboardingHintPanel`, and the onboarding
status-bar projection. Other panels remain in the inventory and roadmap.

## Runtime authority

`Ashfall.Core.Localization.LocalizationService` owns stable-key lookup and
English fallback. `src/Localization/AshfallLocalization.cs` is the Godot
adapter and the only pilot-panel lookup path. It loads the deterministic CSV
catalog and mirrors locale changes to Godot's `TranslationServer`.

The source format is UTF-8 CSV:

```text
key,en,de,source
```

Keys are lowercase, dot-separated semantic names. English is the fallback
authority. German is the Wave-1 secondary-locale skeleton. `pseudo` remains a
development-only expansion locale.

## Missing keys and placeholders

Missing keys return the explicit fallback text or visible key marker and emit
a developer diagnostic. Positional placeholders are formatted with invariant
culture. The CI gate checks placeholder-set parity for every translated row.

## Settings

Locale is persisted in the existing `user://settings.json` settings store.
Legacy settings default to English. Invalid locales recover to English.

## Verification

```bash
python3 scripts/ci/extract_l10n_inventory.py
python3 scripts/ci/l10n_drift_gate.py
```

The inventory is deterministic and the drift gate covers pilot references,
duplicate keys, German completeness, placeholder parity, and direct literal
assignments in pilot files.
