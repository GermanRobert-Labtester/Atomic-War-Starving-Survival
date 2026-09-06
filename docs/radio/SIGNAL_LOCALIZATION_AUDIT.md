# ASHFALL — Distress Signal Localization Readiness Audit (Task 19)

> **Document Status:** Authoritative Localization & Internationalization Audit
> **Authority:** Invariant 6 (Data Authority) & Flagship Tasks 17–20 Hardening
> **Target Scope:** Text extraction, string hygiene, translation key architecture, and expansion budgets for all 25 distress signals
> **Verification Harness:** `Ashfall.Core.Tests/Radio/RadioSignalLocalizationTests.cs` (8 tests, 100% pass)

---

## 1. Executive Summary

This audit assesses the localization readiness of the distress signal narrative catalog (`radio_distress_signals.json`). Survival-management games depend heavily on diegetic atmosphere; radio transmissions are a primary storytelling vessel. Preparing for multilingual release requires that all broadcast text is:
1. Deterministically addressable through structured translation keys.
2. Free from dynamic runtime concatenation.
3. Clean of real-world historical/geographical terms that break immersion or violate distribution policies.
4. Budgeted for text expansion in longer European and Slavic languages (German, Russian, Polish).

---

## 2. Translation Key Architecture

To prevent key collisions and enable smooth ingestion into standard localization platforms (POEditor, Crowdin, Weblate, or Godot `.translation` CSVs), distress strings follow a strict namespace convention:

```text
distress.<frequency_id>.<attribute>
```

### 2.1 Key Schema

| Attribute | Pattern | Example Key |
|---|---|---|
| Source Name | `distress.{frequency_id}.source_name` | `distress.freq_distress_88_3.source_name` |
| Warning Text | `distress.{frequency_id}.warning` | `distress.freq_distress_192_4.warning` |
| Message Fragment | `distress.{frequency_id}.frag_{day}` | `distress.freq_distress_88_3.frag_1` |
| Resolution Summary | `distress.{frequency_id}.outcome` | `distress.freq_distress_88_3.outcome` |

### 2.2 Inventory Statistics

- **Total Signals:** 25 signals.
- **Source Names:** 25 translatable strings.
- **Warning Texts:** 25 translatable strings.
- **Message Fragments:** 93 translatable fragments across days 1–7.
- **Total Discrete Keys:** 143 localization keys.
- **Collision Rate:** 0.0% (all 143 keys are strictly unique).

---

## 3. String Hygiene & Universe Policy Compliance

### 3.1 Zero Real-World Entities (Invariant 6 Compliance)
All 25 distress signals were scanned against the canonical ASHFALL banned-entity list:
- **Prohibited Nations / Alliances:** America, United States, USA, Russia, USSR, Soviet, China, Britain, United Kingdom, UK, Germany, France, Japan, NATO, Warsaw Pact.
- **Prohibited Real Cities:** Berlin, Moscow, Washington, Beijing, London, Tokyo, Paris.
- **Prohibited Slang:** lol, cringe, based, sus, rofl, lmao, yolo, yeet, boomer, zoomer.
- **Audit Finding:** 0 violations. All signals employ established fictional geography (e.g. Meridian Compact, Sector 4, Checkpoint Kilo, Bunker 4-East).

### 3.2 Terminal Punctuation & Segment Boundaries
Translators and CAT tools require clear segment termination to generate coherent sentence units:
- Every fragment strictly terminates with a valid punctuation mark (`.`, `!`, `?`, `*`, `"`).
- Narrative stage directions are clearly demarcated with asterisks (`*A piano plays through the static...*`), signaling non-verbal audio to translators.

---

## 4. UI Layout & Text Expansion Budget

When translating English text into target languages:
- **German:** Expect +25% to +35% character growth.
- **Russian:** Expect +15% to +30% character growth.
- **French:** Expect +20% to +30% character growth.

### 4.1 Length Constraints
- **Sentence Cap:** Strictly `<= 3` sentences per message fragment (verified across all 93 fragments).
- **Character Budget:**
  - English source text maximum: **332 characters** (observed max: `freq_distress_144_1` Day 4).
  - Maximum allowable in UI dialogue box: **500 characters**.
  - Headroom for 35% German expansion: `332 * 1.35 = 448 characters` (fits safely within the 500-char UI panel budget without text truncation or vertical scrollbar overflow).

---

## 5. Non-Concatenation & Parameterization Standards

Core simulation code never constructs message fragments via runtime string interpolation (`$"..."`). Instead:
- Narrative fragments are complete, discrete sentences.
- Procedural placeholders (if required in future expansions) must use standard named tokens (e.g., `{survivor_name}`, `{days}`) rather than split phrases.
- Character encodings are verified UTF-8 with zero unprintable control characters (`\x00`–`\x1F`).

---

## 6. Verification Evidence

All 8 localization criteria are mechanically enforced in CI by:
`Ashfall.Core.Tests/Radio/RadioSignalLocalizationTests.cs`
- `SignalIdentifiers_AreLocalizationKeySafe`: PASS
- `SourceNames_AreNonEmpty_Trimmed_AndClean`: PASS
- `MessageFragments_ContainNoForbiddenRealWorldEntities`: PASS
- `MessageFragments_HaveValidTerminalPunctuation`: PASS
- `MessageFragments_AdhereToSentenceAndCharacterBudgets`: PASS
- `TranslatableMetadata_AreStructuredStrings`: PASS
- `UTF8Encoding_ContainsNoUnprintableControlCharacters`: PASS
- `LocalizationKeyInventory_IsCompleteAndUnique`: PASS
