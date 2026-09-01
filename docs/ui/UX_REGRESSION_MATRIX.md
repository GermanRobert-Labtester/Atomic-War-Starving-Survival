# ASHFALL — UX, Accessibility & Onboarding Regression Matrix

**Execution Date:** 2026-09-01
**Plan:** Plan 14 — UX, Onboarding & Accessibility
**Target:** Godot 4.7+ .NET Mono Host / .NET 8 Host / .NET 9 Tests

---

## 1. Test Matrix Execution Results

| Suite / Gate | Type | Scope | Result | Evidence / Details |
|---|:---:|---|:---:|---|
| `dotnet test Ashfall.Core.Tests` | Unit & Determinism | Core simulation, localization, settings, save roundtrips | **PASS** | 5,311 passed, 0 failed, 0 skipped (Duration: 16s) |
| `godot --headless --path . -- --ui-accessibility-selftest` | UI Integration | Focus traversal, labels, close actions, modal traps, static lint | **PASS** | 5/5 gates passed (51 controls, 226 labels, 9 close actions, 164 UI files) |
| `godot --headless --path . -- --onboarding-journey-selftest` | Journey Integration | Day 1 opening loop, sigil progression, save/load resume | **PASS** | 20/20 checks passed; full journey with save/load resume |
| `godot --headless --path . -- --data-integrity-selftest` | Data Contract | Authoritative JSON catalogs, foreign key resolution, schema version | **PASS** | 0 errors across 138 catalogs (5,563 authored IDs) |
| `godot --headless --path . -- --content-utilization-selftest` | Asset Gate | Runtime catalog query and consumption | **PASS** | 413 catalogs scanned, 0 orphaned, CI gate PASS |
| `godot --headless --path . -- --scene-binding-selftest` | Presentation Binding | 22 packed UI scene contracts | **PASS** | 22/22 scenes bound and verified |
| `python3 scripts/ci/scene-lint.py` | Linter | Production `.tscn` structural lint | **PASS** | 26 production scenes checked; 0 errors, 0 warnings |

---

## 2. Invariant & Regression Proofs

1. **Deterministic Localization & Pseudo-Locale:**
   - English source strings and pseudo-locale transformations preserve positional formatting (`{0}`, `{1}`) while stress-testing +30-40% length expansion.
   - Pinned by `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs` (7 test cases).
2. **Safe Settings Recovery:**
   - Unrecognized locales, out-of-range UI scale values, and invalid tutorial modes safely sanitize to defaults without throwing.
   - Pinned by `UserSettingsCodec_SanitizesLocaleAndTutorialMode`.
3. **Multi-Channel Warning Communication:**
   - Zero critical states rely on color alone; every alert pair combines high-contrast color (`Theme.Critical`, `Theme.Radiation`), symbol/marker (`[!]`, `[RAD]`, `[▲]`), and text label.
4. **Dynamic Action Prompt Resolution:**
   - All tutorial and help text queries `AshfallInputActions.GetActionPrompt()` rather than hardcoding static keys.
