# Localization implementation plan

The active Wave-1 implementation uses the existing Core
`LocalizationService`, the Godot `AshfallLocalization` adapter, and
`assets/l10n/strings.csv`. The two pilot surfaces are `ResearchPanel` and
`OnboardingHintPanel`.

The extraction artifact is `artifacts/l10n-inventory.json`; the executable
drift contract is `scripts/ci/l10n_drift_gate.py`. English remains the
fallback source, German is the verified secondary locale, and `pseudo` is
reserved for layout expansion checks.

The inventory deliberately records the broader hardcoded UI backlog without
claiming that those panels are localized. Wave 2 work is ranked in
`docs/L10N_WAVE2_ROADMAP.md`.
