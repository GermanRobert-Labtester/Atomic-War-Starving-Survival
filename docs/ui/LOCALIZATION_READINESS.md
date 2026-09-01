# ASHFALL — Localization Readiness & String Extraction Architecture

**Audit Reference:** Plan 14 Task 14C / `ashfall-string-extractor`
**Target Format:** Standard CSV / GNU gettext POT (`assets/l10n/strings.csv`, `assets/l10n/template.pot`)
**Engine Integration:** Godot 4.7 `TranslationServer` + Core `LocalizationService` (engine-agnostic fallback)

---

## 1. Localization Boundary

To ensure maintainability and realistic translation scope:

### Included in Immediate Localization Boundary:
1. **UI Chrome & Navigation:** Titles, tab labels, button text, headers, status messages, empty states.
2. **System Settings:** Display, audio, accessibility, gameplay, and language option labels.
3. **Tutorial & Onboarding:** Stage objectives, tutorial directives, tips, contextual hints.
4. **Warnings & Alerts:** Radiation warnings, critical needs, power deficit, hunger/thirst alerts.
5. **Item & Equipment Metadata:** Names, categories, effect descriptions, units.
6. **Field Manual / Codex Definitions:** Survival rules, glossary, role descriptions.

### Deferred / Later Phase:
- Large narrative prose corpus (world history, radio stories, lore documents) — to be localized during dedicated content translation packs.

---

## 2. Key Naming Convention

Stable, domain-prefixed `snake_case` keys independent of display wording:

- `ui.<domain>.<element>` (e.g. `ui.inventory.title`, `ui.medical.triage`, `ui.common.close`)
- `warning.<system>.<severity>` (e.g. `warning.radiation.critical`, `warning.water.depleted`)
- `tutorial.<stage>.<field>` (e.g. `tutorial.protocol.objective`, `tutorial.radiation.hint`)
- `settings.<category>.<item>` (e.g. `settings.display.ui_scale`, `settings.accessibility.high_contrast`)
- `item.<id>.<name|desc>` (e.g. `item.canned_food.name`, `item.iodine_pills.desc`)
- `codex.<topic>.<title|body>` (e.g. `codex.radiation.title`, `codex.radiation.body`)

---

## 3. Dynamic Formatting & Pluralization

Dynamic values are formatted via positional placeholders (`{0}`, `{1}`) to allow translators to adjust sentence order:
- Localized format: `warning.radiation.dose_rate = "Radiation Dose: {0:F1} mSv/h ({1})"`
- Localized runway: `ui.inventory.runway = "{0} units remaining (~{1:F1} days)"`

---

## 4. Development Pseudo-Locale

A QA pseudo-locale (`pseudo`) expands strings by +30–40% and wraps them in brackets (e.g., `[!!! Ḓāȳ 1 Õḅǰēċṫīṽē !!!]`) to stress-test:
1. Text wrapping and box expansion.
2. Fixed-width label overflow.
3. Hardcoded string misses (any string not wrapped in `Tr()` renders plain English without brackets).
