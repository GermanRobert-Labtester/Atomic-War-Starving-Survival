# ASHFALL — Quality Roadmap Batch 60

## Theme: Localization Infrastructure — i18n Pipeline for All Player-Facing Strings

**Priority:** MEDIUM (enables future translation, not blocking release)
**Risk:** Medium — touches all UI panels and narrative text
**Batch:** 60
**Depends on:** Stable UI panel architecture in `src/UI/`, narrative JSON schema in `Assets/StreamingAssets/Data/narrative/`
**Blocks:** Translation community contributions, non-English release, accessibility (screen reader string keys)

---

## Motivation

The game currently has 83 files named `*Panel.cs` in `src/UI/` (97 total `.cs` files in that directory, including shared widgets/modals/overlays), 196 narrative JSON files (verified exact count via `Assets/StreamingAssets/Data/narrative/*.json`), and multiple Core systems that generate player-facing text directly. There is no localization infrastructure — confirmed by an exhaustive search of `Assets/Ashfall.Core/`, `src/`, and `Ashfall.Core.Tests/` for `ILocalizer`, `TranslationServer`, `assets/locale/`, or any "i18n"/"l10n"/"locale"/"translation" term: none exist except an unrelated `"locale": "en_US"` metadata field in a UI snapshot-test fixture and an unfilled template row in `docs/AI_DISCLOSURE.md`. `project.godot` has no `[internationalization]` section and no locale/translation keys at all — this project has never touched Godot's built-in translation system. Adding i18n now (before content lock) avoids a painful retrofit later and enables community translation workflows. The architecture must respect the Core/Host boundary: string resolution logic belongs in Core (engine-agnostic), while the actual translation file format and loading mechanism belong to the Godot host.

**Important correction to scope:** this codebase does not use the Unity-style `.Text = "..."` / `TooltipText = "..."` direct-assignment idiom that a naive string-extraction tool might assume. Sampled panels (`InventoryPanel.cs`, `MedicalPanel.cs`, `SettingsPanel.cs`) contain zero such assignments. Player-facing strings instead appear as literal arguments to helper calls, e.g. `AshfallUiHelpers.MakeMetadata("Nothing stored. The shelter shelves are bare.")` and `_statusRail.Set("stacks", "0", ...)` (`src/UI/InventoryPanel.cs:58, 79`). Step 2's extraction tool design is corrected below to reflect this.

---

## Step 1 — Design Localization Architecture (String Keys vs Inline, Core vs Host)

**Goal:** Produce a design document defining where localization responsibility lives, what format translation files use, and how Core systems request translated strings without coupling to Godot.

**Implementation:**
- Define the `ILocalizer` port interface in Core:
  ```csharp
  namespace Ashfall.Core
  {
      public interface ILocalizer
      {
          string Get(string key);
          string Get(string key, params (string name, object value)[] args);
          bool HasKey(string key);
          string CurrentLocale { get; }
      }
  }
  ```
- Define string key conventions:
  - Format: `{domain}.{panel_or_system}.{element}` — e.g., `ui.shelter.title`, `narrative.encounter.intro_text`
  - All keys are `snake_case` with dot separators.
  - Parameterized strings use named placeholders: `"survivor.status.hunger" → "{name} is starving ({days} days without food)"`
- Define translation file format:
  - Primary: `.csv` (Godot TranslationServer native support) — one file per locale.
  - Location: `assets/locale/{locale_code}.csv` (e.g., `assets/locale/en.csv`, `assets/locale/de.csv`).
  - Fallback chain: requested locale → `en` (English is always complete).
- Define responsibility split:
  - **Core** owns: `ILocalizer` interface, string key constants, parameterized string formatting logic.
  - **Godot Host** owns: `TranslationServer` adapter, `.csv` file loading, locale switching UI.
  - **Narrative JSON** owns: its own localization strategy (see Step 6).
- Write design doc to `docs/localization-architecture.md`.

**Verification:**
- Design doc reviewed for Core/Host boundary compliance (no `Godot.*` in Core design).
- `ILocalizer` interface compiles in `Assets/Ashfall.Core/`.

**Risk / Rollback:** None — this step produces only a design document and a standalone interface file with no callers yet. If the design is later revised, nothing else in the codebase references it at this point.

**Done when:** `docs/localization-architecture.md` exists on disk, `ILocalizer` compiles standalone in `Assets/Ashfall.Core/Localization/ILocalizer.cs` with zero engine references (verify via `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj`), the string key convention is documented with at least 3 concrete example keys, and the `.csv` format choice is recorded with its rationale (Godot's native `TranslationServer` support).

---

## Step 2 — Create String Extraction Tool (Find All Player-Facing Hardcoded Strings)

**Goal:** Build a tool/script that scans the Godot host (`src/`) and Core systems to identify all hardcoded player-facing strings that need localization keys.

**Implementation:**

**Corrected approach:** the original plan assumed strings are found via `.Text = "..."` / `TooltipText = "..."` assignments. That idiom does not exist in this codebase (verified: zero matches across sampled panels). Real player-facing strings are passed as literal or interpolated string arguments to helper methods (`AshfallUiHelpers.MakeMetadata(...)`, `_statusRail.Set(...)`, and similar calls on the 5 shared `Ashfall*` widgets). The extraction tool must therefore scan for **string-literal call arguments and interpolated strings**, not assignment expressions.

- Create `scripts/tools/extract-hardcoded-strings.sh` (or a C# Roslyn analyzer if precision is needed — recommended here, since assignment-based regex scanning will not work; a Roslyn syntax walk over `InvocationExpressionSyntax` argument lists is required to reliably distinguish "this string argument is player-facing text" from "this string argument is an internal key/id/format-token"):
  - Scan `src/UI/**/*.cs` for string-literal arguments passed into UI helper calls (`AshfallUiHelpers.*`, `.Set(...)` on `AshfallStatusRail`/`AshfallDataGrid`/`AshfallMetricCard`/`AshfallSidebar`/`AshfallDashboardShell`, and any `Label`/`Button`/`RichTextLabel` constructor or property-setter call taking a string).
  - Scan `src/Host/**/*.cs` for strings passed to UI-bound methods.
  - Scan `Assets/Ashfall.Core/` for systems that build player-facing strings:
    - `DailyBriefingReportBuilder` — report lines. **Verified to exist:** `Assets/Ashfall.Core/Campaign/DailyBriefingReportBuilder.cs`.
    - `JournalVoice` — journal entries. **Verified to exist:** `Assets/Ashfall.Core/Journal/JournalVoice.cs`.
    - `ProceduralEulogyEngine` — eulogy text. **Verified to exist:** `Assets/Ashfall.Core/Journal/ProceduralEulogyEngine.cs`.
    - `TradeTellEngine` — trade narration. **Verified to exist:** `Assets/Ashfall.Core/Economy/TradeTellEngine.cs`.
    - Any system returning `string` that reaches UI.
  - Exclude: log messages, debug strings, internal error messages, test strings, and — newly identified in this codebase — non-text string arguments that look like ids/keys/filter tokens (e.g. `"all"`, `"consumable"`, `"stacks"` as seen in the sampled `InventoryPanel.cs` calls). A raw literal count across `src/UI/*.cs` found roughly 6,000-7,000 total string literals depending on exact regex shape (this review's own independent re-count with a slightly different regex got 6,914 total / 1,678 space-containing vs. the previously-cited 6,258/1,181 — the two counts disagree because "string literal" and "human-readable-shaped" are both regex-approximation-dependent, not because either count is wrong), of which roughly 1,000-1,700 look human-readable (space-separated, alphabetic) by either count — the true extractable count depends heavily on this filtering step, so budget real triage time for it rather than assuming the tool's raw output is directly usable, and do not treat either cited number as precise.
- Output format: TSV with columns `file`, `line`, `current_string`, `suggested_key`, `domain`.
- Estimated yield: ~400–600 extractable strings from UI (order-of-magnitude estimate based on the ~1,181 human-readable-shaped literal count above, before id/key filtering — treat this range as a planning estimate, not a verified figure), ~100–200 from Core systems.
- Triage output into priority buckets:
  - P1: Main HUD, shelter panel, inventory — always visible.
  - P2: Secondary panels (medical, trade, journal).
  - P3: Rare/edge-case UI (settings, debug overlays).

**Risk / Rollback:** None — this step only creates a standalone analysis tool and its output file; it does not modify any production code.

**Verification:**
```bash
bash scripts/tools/extract-hardcoded-strings.sh > /tmp/string-audit.tsv
wc -l /tmp/string-audit.tsv  # Compare against the ~400-600 planning estimate; if far outside this range, that is a signal to review the tool's filtering logic, not a hard pass/fail gate
```

**Done when:** The extraction tool runs against the actual call-argument pattern used in this codebase (not an assignment pattern that doesn't exist here), produces a categorized TSV list of hardcoded player-facing strings, and the P1/P2/P3 triage is complete with each of the 10 target panels from Step 5 represented in the P1 bucket.

---

## Step 3 — Implement ILocalizer Port in Core (Engine-Agnostic String Resolution)

**Goal:** Implement the `ILocalizer` interface in Core with a default in-memory adapter suitable for tests and a registration point for host adapters.

**Implementation:**
- Create `Assets/Ashfall.Core/Localization/ILocalizer.cs` (interface from Step 1).
- Create `Assets/Ashfall.Core/Localization/InMemoryLocalizer.cs`:
  ```csharp
  namespace Ashfall.Core.Localization
  {
      public class InMemoryLocalizer : ILocalizer
      {
          private readonly Dictionary<string, string> _strings;
          public string CurrentLocale { get; }

          public InMemoryLocalizer(string locale, Dictionary<string, string> strings) { ... }
          public string Get(string key) => _strings.TryGetValue(key, out var v) ? v : $"[MISSING:{key}]";
          public string Get(string key, params (string name, object value)[] args) { ... }
          public bool HasKey(string key) => _strings.ContainsKey(key);
      }
  }
  ```
- Create `Assets/Ashfall.Core/Localization/StringKeys.cs` — static class with `const string` fields for all P1 keys:
  ```csharp
  public static class StringKeys
  {
      public static class Ui
      {
          public const string ShelterTitle = "ui.shelter.title";
          public const string InventoryTitle = "ui.inventory.title";
          // ...
      }
  }
  ```
- Parameterized formatting: implement simple `{name}` replacement in `Get(key, args)` — no external dependency (not `string.Format` positional, use named params for translator clarity).
- Add `ILocalizer` to `Ports.cs`. Note: `Ports.cs` (`Assets/Ashfall.Core/Ports.cs`, 49 lines — verified by direct line count; a prior draft of this note said 46) currently defines exactly `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng` — there is no separate "documentation table" file to update; the closest thing is the port table in `docs/architecture/dual_engine_plan.md:25-27` (confirmed: `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng` listed on line 27, default adapters on line 29), which should also be updated when `ILocalizer` is added.
- Register the default `InMemoryLocalizer` adapter in `Assets/Ashfall.Core/HostDefaults.cs`. Note: `HostDefaults.cs` is not a single class — it's a file containing several independent default-adapter classes (`FileSystemIO`, `SystemTextJsonSerializer`, `ConsoleLog`/`NullLog`, `SimClock`, `SeededRng`, plus the `CatalogLocator` helper). Add the new `InMemoryLocalizer` registration/loading logic alongside these as another standalone class in the same file, consistent with the existing convention — do not assume a `HostDefaults` class exists to add a method to.

**Risk / Rollback:** Low — new, standalone files with no existing callers. If `InMemoryLocalizer`'s `{name}` parameterized-formatting logic has edge cases (unmatched placeholders, null args), it fails closed by design (`Get` falls back to `[MISSING:{key}]`), so a bug here surfaces as a visible placeholder string in-game rather than a crash.

**Verification:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~Localization"
```

**Done when:** `ILocalizer` and `InMemoryLocalizer` compile in Core with no engine references; unit tests cover `Get`, parameterized `Get`, missing key fallback, and `HasKey`; `Ports.cs` and `docs/architecture/dual_engine_plan.md`'s port table both list `ILocalizer`.

---

## Step 4 — Create Godot TranslationServer Adapter

**Goal:** Implement the Godot-side adapter that bridges `ILocalizer` to Godot's built-in `TranslationServer`, enabling runtime locale switching.

**Implementation:**
- Create `src/Localization/GodotLocalizer.cs`:
  ```csharp
  namespace AtomicWar.GodotApp.Localization
  {
      public class GodotLocalizer : ILocalizer
      {
          public string CurrentLocale => TranslationServer.GetLocale();

          public string Get(string key) => TranslationServer.Translate(key);
          public string Get(string key, params (string name, object value)[] args)
          {
              var template = TranslationServer.Translate(key);
              return FormatNamed(template, args);
          }
          public bool HasKey(string key) => TranslationServer.Translate(key) != key;
      }
  }
  ```
- Create `assets/locale/en.csv` with header row and initial P1 strings:
  ```csv
  keys,en
  ui.shelter.title,Shelter
  ui.inventory.title,Inventory
  ui.hud.day_counter,Day {day}
  ...
  ```
- Register the `.csv` translation in `project.godot` or via `TranslationServer.AddTranslation()` at startup. Note: `project.godot` currently has no `[internationalization]` section at all (confirmed by direct read) — this is a net-new section, not an edit to existing locale config.
- Wire `GodotLocalizer` into `Main.cs` initialization (replace any test/fallback localizer).
- Add locale switching support: `TranslationServer.SetLocale(code)` triggered from settings UI (placeholder — actual settings panel is future work).

**Risk / Rollback:** Low-Medium — `Main.cs` is a single large partial class (7,014 lines as of this review — AGENTS.md's H7 "~6.5k-line" figure is stale/rounds down); adding `GodotLocalizer` construction to its init path is a small, additive change, but should land as its own commit so a `TranslationServer` misconfiguration (e.g. missing `.csv` at the registered path) is easy to isolate and revert without touching unrelated `Main.cs` init logic. Rollback: `GodotLocalizer` is only referenced from the one init call site; removing that line reverts to no host-side localizer being registered (Core's `InMemoryLocalizer` fallback, if wired per Step 3, still functions).

**Verification:**
```bash
dotnet build Ashfall.csproj  # Godot host compiles
godot --headless --path . -- --bridge-selftest  # Stable CI verb still exits 0
```

Manual verification: launch game, confirm P1 strings display from `.csv` rather than hardcoded.

**Done when:** `GodotLocalizer` compiles and is wired into `Main.cs`; the `en.csv` file exists with at least 20 P1 string keys; the Godot host resolves strings through `TranslationServer` at runtime.

---

## Step 5 — Extract UI Panel Strings to Translation Keys (First 10 Critical Panels)

**Goal:** Convert the 10 highest-traffic UI panels from hardcoded strings to `ILocalizer.Get(key)` calls, proving the pipeline end-to-end.

**Implementation:**
- Target panels (P1 priority) — **corrected against the real 83 `*Panel.cs` filenames in `src/UI/` (verified by directory listing during this review; the original list used generic descriptions instead of real class names and two of the ten did not resolve cleanly):**
  1. `GameHudOverlay.cs` (day counter, need bars, alert text — the original "Main HUD" is not a `*Panel.cs` file at all; it's `GameHudOverlay.cs`)
  2. `ShelterPanel.cs` (shelter overview — `ShelterDetailPanel.cs` and `ShelterHudPanel.cs` also exist; this step targets the top-level `ShelterPanel.cs` only, not its Detail/Hud siblings)
  3. `InventoryPanel.cs` (confirmed — matches original)
  4. `SurvivorsPanel.cs` (original said "Survivor list panel" — real class is plural `SurvivorsPanel`, not `SurvivorPanel`; `SurvivorDetailPanel.cs` is a separate file, out of scope here)
  5. `CraftingPanel.cs` (original "Crafting/recipe panel" was ambiguous among `CraftingPanel.cs` / `CraftingDetailPanel.cs` / `CraftingHistoryPanel.cs`; this step targets the top-level list/recipe panel only)
  6. `TradeDetailPanel.cs` (original "Trade session panel" does not exist under that name; closest real match confirmed by reading its class doc-comment — "Shows detailed trade information, trade history, market prices, and trade negotiations" — is `TradeDetailPanel.cs`. `CaravanBarterLedgerPanel.cs` is a related but distinct file covering caravan/barter ledger UI; not in scope for this step)
  7. `MedicalPanel.cs` (original "Medical triage panel" — real class is `MedicalPanel.cs`; `MedicalDetailPanel.cs`/`MedicalHistoryPanel.cs` are separate files, out of scope)
  8. `ExpeditionPanel.cs` (original "Expedition briefing panel" — no "briefing" panel exists; real top-level class is `ExpeditionPanel.cs`. `ExpeditionDetailPanel.cs`/`ExpeditionHistoryPanel.cs`/`ExpeditionRadarPanel.cs` are separate files, out of scope)
  9. `RadioPanel.cs` (confirmed — matches the original's own guess; `RadioDetailPanel.cs` is separate, out of scope)
  10. `DailyBriefingModal.cs` — **not a Panel.** The original "Daily briefing panel" does not exist as a `*Panel.cs` file; the real class is `DailyBriefingModal.cs` (a `Control`-derived modal, not a panel). Either (a) swap this list slot for a real panel not otherwise covered (e.g. `StatusPanel.cs` or `WeatherPanel.cs`), or (b) keep `DailyBriefingModal.cs` in scope but rename this step's title/wording so "10 critical panels" doesn't misdescribe a modal as a panel. Decide explicitly before starting — do not silently substitute.
- For each panel:
  - Identify all player-facing string literals — per the Step 2 correction, these are string arguments passed to helper calls (`AshfallUiHelpers.*`, `_statusRail.Set(...)`, `.Set(...)` on the shared `Ashfall*` widgets) and `Label`/`Button` constructor/property calls, **not** `.Text = "..."` assignments (that pattern does not exist in this codebase).
  - Replace with `_localizer.Get(StringKeys.Ui.XxxYyy)` (or equivalent key constant).
  - Add the English string to `assets/locale/en.csv`.
  - Add the key constant to `StringKeys.cs`.
- Inject `ILocalizer` into each panel's constructor or `_Ready()` via the host session.
- Ensure dynamic strings (e.g., "Day 14", "3 survivors") use parameterized keys: `_localizer.Get("ui.hud.day_counter", ("day", currentDay))`.
- Do NOT change string content — only change where it's sourced from.

**Risk / Rollback:** Medium — this is the first step that touches live, shipping panel code (all prior steps only added new standalone files). A mistranslated key or a missed dynamic-string case (e.g. forgetting to parameterize "Day 14" and instead hardcoding the key) causes a visible in-game regression. Mitigation: convert and manually verify one panel at a time rather than all 10 in a single commit, and keep the "Do NOT change string content" rule strictly — any wording change bundled into this step makes it impossible to tell whether a visual diff is a localization-pipeline bug or an intentional copy edit. Rollback: since `ILocalizer.Get(key)` for an English-locale player returns the exact same string as before (assuming the extraction was faithful), a regression here is isolated to the specific panel touched — revert that one panel file back to its literal-string form; the pipeline itself (`ILocalizer`, `GodotLocalizer`, `en.csv`) is unaffected by reverting a single consumer.

**Verification:**
```bash
dotnet build Ashfall.csproj
godot --headless --path . -- --bridge-selftest
# Confirm no unlocalized player-facing string literals remain in the 10 target panel files.
# Note: src/UI/ is a FLAT directory (no Panels/ subfolder) — panel files live directly at src/UI/*.cs.
# There is no ".Text = " assignment idiom in this codebase, so a literal-assignment grep will find
# nothing whether or not the migration succeeded. Verify manually per-panel instead: diff each
# panel's helper-call string arguments before/after migration and confirm every English literal
# that was extracted now reads from StringKeys.* / _localizer.Get(...).
```

**Done when:** the 10 concrete files listed above (real filenames, with slot 10 explicitly resolved to either a real panel or a corrected step description) source all player-facing strings from `ILocalizer`; `en.csv` contains all their keys; the game displays identically to before (no visual regression) — verified via manual side-by-side screenshot comparison of each of the 10 targets, since no automated grep-based check applies to this codebase's string-argument idiom.

---

## Step 6 — Design Narrative Content Localization Strategy (196 JSON Files)

**Goal:** Define how the 196 narrative JSON files (encounters, radio broadcasts, journal templates) will be localized without breaking the existing JSON-as-authority pattern.

**Implementation:**
- Evaluate two approaches:
  - **Option A — Parallel locale files:** `Assets/StreamingAssets/Data/narrative/en/encounter_001.json`, `de/encounter_001.json`, etc. Loader picks locale subfolder.
  - **Option B — Key indirection:** Narrative JSON stores string keys (`"text_key": "encounter.001.intro"`), resolved via `ILocalizer`. English strings live in `en.csv` alongside UI strings.
- Recommended: **Option A** for narrative (long-form text doesn't belong in flat CSV; translators need full context of encounters). **Option B** for short UI strings (already implemented in Steps 3–5).
- Define schema extension for localized narrative:
  ```json
  {
    "schema_version": 2,
    "locale": "en",
    "encounter_id": "encounter_rad_storm_01",
    "text": "The sky turns a sickly amber..."
  }
  ```
- Define fallback behavior: if a locale file is missing, fall back to `en/` (never show a raw key to the player).
- Define translator workflow:
  - Copy `en/` folder to new locale folder.
  - Translate `"text"` fields in-place.
  - Validate with a schema check (same keys, same `encounter_id`, matching `schema_version`).
- Update `CatalogIntegrityValidator` to validate locale folder consistency (same file count, same IDs). Note: `CatalogIntegrityValidator.cs` (`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`) is currently 657 lines (AGENTS.md's "603 lines" figure is stale) and performs the project's central 5-tier data validation gate — treat any change to it as touching shared, high-blast-radius infrastructure, not an isolated addition.
- Write strategy doc to `docs/narrative-localization-strategy.md`.

**Risk / Rollback:** Low for the design/doc work itself, but flag the `CatalogIntegrityValidator` extension specifically: this file backs the `--data-integrity-selftest` gate that every other batch's verification checklist depends on. Land the locale-folder-consistency check as an additively-gated new validation tier (only runs if a locale subfolder actually exists) rather than modifying existing tier logic, so that projects with zero non-English locales (the current state) see no behavior change. Rollback: since no non-English locale folder exists yet, the new check is dead code until Step 6's own prototype runs — safe to revert in isolation.

**Verification:**
- Strategy doc reviewed for compatibility with existing `StreamingAssets/Data/` authority pattern.
- Prototype: create `Assets/StreamingAssets/Data/narrative/en/` with 3 test encounters, verify loader resolves them.

**Done when:** Strategy document exists; the locale-subfolder approach is validated with a prototype; `CatalogIntegrityValidator` extension is specified; translator workflow is documented.

---

## Step 7 — Add Localization Completeness Test (Detect Untranslated Keys)

**Goal:** Create an automated test that detects missing translations — any key referenced in code but absent from a locale file, or any key in `en.csv` missing from other locale files.

**Implementation:**
- Create `Ashfall.Core.Tests/LocalizationCompletenessTests.cs`:
  - **Test 1 — All StringKeys constants have entries in en.csv:**
    Reflect over `StringKeys` to get all `const string` values. Parse `assets/locale/en.csv`. Assert every key exists in the CSV.
  - **Test 2 — No orphaned keys in en.csv:**
    Parse `en.csv` keys. Scan `Assets/Ashfall.Core/` and `src/` for usage of each key (via `StringKeys.*` or raw string). Warn on keys that appear in CSV but are never referenced (dead keys).
  - **Test 3 — Locale parity (when additional locales exist):**
    For each `assets/locale/{locale}.csv` file, assert it contains every key present in `en.csv`. Report missing keys per locale with line count.
  - **Test 4 — Narrative locale parity (when additional locales exist):**
    For each locale subfolder in `Assets/StreamingAssets/Data/narrative/{locale}/`, assert it contains the same filenames as `en/`. Report missing files.
- Output format: test failure message lists all missing keys/files with their expected location.
- Gate: test fails CI if `en.csv` is incomplete (missing a `StringKeys` constant). Other locales produce warnings (not failures) until translation is declared complete.

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~LocalizationCompleteness"
```

Intentionally remove a key from `en.csv`, confirm the test fails, then restore.

**Done when:** Completeness tests exist and pass; adding a new `StringKeys` constant without a matching `en.csv` entry causes test failure; the test documents the exact gap for translators.

---

## Summary

| Step | Area | Action | Risk | Estimated Effort |
|------|------|--------|------|-----------------|
| 1 | Architecture | Design localization system (Core/Host split, key format, file format) | Low | 2–3 hr |
| 2 | Tooling | String extraction tool for hardcoded player-facing text | Low | 3–4 hr |
| 3 | Core | `ILocalizer` port + `InMemoryLocalizer` + `StringKeys` | Low | 2–3 hr |
| 4 | Godot Host | `GodotLocalizer` adapter + `en.csv` + TranslationServer wiring | Medium | 2–3 hr |
| 5 | UI Panels | Extract strings from 10 critical panels to keys | Medium (UI regression risk) | 4–6 hr |
| 6 | Narrative | Design locale-subfolder strategy for 196 JSON files | Low | 2–3 hr |
| 7 | Tests | Localization completeness test suite | Low | 2–3 hr |

**Total estimated effort:** 17–25 hours
**Invariants enforced:** #1 (Core stays engine-agnostic — `ILocalizer` has no engine dependency), #5 (No gameplay logic in hosts — string resolution logic lives in Core), #6 (Data authority is JSON — locale CSVs and narrative JSONs are the source of truth)
**Verification command after all steps:**
```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --bridge-selftest
```

**Next prompt after completion:** "Extract strings from the remaining 73 UI panels (83 total `*Panel.cs` files minus the 10 covered in this batch; P2 + P3 priority) and create the first non-English locale file (`de.csv`) as a translation proof-of-concept."

---

## Review Notes (Corrected)

This plan was adversarially reviewed against the real codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War`. The following errors were found and fixed:

1. **Confirmed TRUE: zero localization infrastructure exists.** An exhaustive search of `Assets/Ashfall.Core/`, `src/`, and `Ashfall.Core.Tests/` for `ILocalizer`, `TranslationServer`, `assets/locale/`, and "i18n"/"l10n"/"locale"/"translation" found nothing but an unrelated snapshot-test metadata field and an unfilled disclosure-template row. `project.godot` has no `[internationalization]` section. This is genuinely a from-scratch effort — the plan's core premise is correct and required no fixing.
2. **Critical factual error: the plan assumed a `.Text = "..."` / `TooltipText = "..."` direct-assignment string idiom that does not exist in this codebase.** Sampled `InventoryPanel.cs`, `MedicalPanel.cs`, `SettingsPanel.cs` for that pattern and found zero matches in all three. Real player-facing strings are passed as literal/interpolated arguments to helper calls (`AshfallUiHelpers.MakeMetadata(...)`, `_statusRail.Set(...)`, etc. on the shared `Ashfall*` widgets). Fixed: Step 2's extraction-tool design now targets call-argument scanning (recommending a Roslyn syntax walk over invocation arguments rather than an assignment-pattern regex), Step 5's per-panel migration instructions were corrected to describe the real string-sourcing pattern, and Step 5's verification command — which grepped for `\.Text = "` and would silently report "0 matches, therefore done" regardless of whether the migration actually happened — was replaced with a manual per-panel diff check, since no automated grep pattern reliably applies here.
3. **Panel count precision fixed.** "~85 UI panels" is close but the exact figure is 83 files literally named `*Panel.cs` (97 total files in `src/UI/` including shared widgets/modals). The final "Next prompt" line's "remaining 75 UI panels" was arithmetically wrong even under the original ~85 assumption (85 − 10 = 75, which was internally consistent with itself, but wrong against the real 83) — corrected to "remaining 73" (83 − 10).
4. **Path error fixed.** Step 5's verification grep assumed panels live under `src/UI/Panels/{target_panels}.cs`. `src/UI/` is confirmed to be a **flat** directory with no `Panels/` subdirectory — all 83 panel files sit directly at `src/UI/*.cs`. Fixed in the corrected verification note.
5. **196 narrative JSON file count — VERIFIED EXACT**, matches `find Assets/StreamingAssets/Data/narrative/ -name "*.json" | wc -l`. No change needed; now cited with the verification method in the Motivation section.
6. **`Ports.cs` "documentation table" reference corrected.** `Ports.cs` itself (46 lines) has no documentation table — it's a plain interface-definitions file currently listing `IJsonSerializer`, `IFileIO`, `ILog`, `IClock`, `ISeededRng`. The actual port documentation table lives in `docs/architecture/dual_engine_plan.md:25-27`. Fixed Step 3 to reference the correct file for the table update.
7. **`HostDefaults.cs` is not a single class.** It's a file containing several independent standalone adapter classes (`FileSystemIO`, `SystemTextJsonSerializer`, `ConsoleLog`/`NullLog`, `SimClock`, `SeededRng`, `CatalogLocator`). Step 3's "Register in HostDefaults.cs" instruction was corrected to describe adding a new standalone class to that file, consistent with its existing convention, rather than implying a `HostDefaults` class with a method to add to.
8. **Missing risk/rollback notes added.** The original plan had zero risk/rollback language anywhere despite Step 5 (touching 10 live, shipping panels) and Step 6 (proposing to extend `CatalogIntegrityValidator`, the shared 657-line validator backing the `--data-integrity-selftest` gate every other batch's verification depends on) both carrying real regression risk. Added explicit Risk/Rollback subsections to Steps 3, 4, 5, and 6, and corrected `CatalogIntegrityValidator`'s cited line count from the stale "603" (AGENTS.md) to the current 657.
9. **Vague Done-when criteria tightened.** Step 1's "the team agrees on the .csv format" (unfalsifiable — no team consensus mechanism exists in a CI/test context) replaced with concrete file-existence and compile checks. Step 5's "no visual regression" now specifies the actual verification method (manual side-by-side screenshots per panel) since no automated pixel-diff tooling was found to exist in this repo already.


---

## Review Notes (Corrected) — Second Pass

A second independent adversarial pass re-verified every claim in this document against the live codebase at `/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War` rather than trusting the first pass's own "Review Notes" section. Findings:

1. **Re-confirmed independently, no changes needed:** zero localization infrastructure anywhere (`ILocalizer`/`TranslationServer`/`i18n`/`l10n` grep across the repo returns nothing but the `docs/ui/snapshot_manifest.json` `"locale": "en_US"` metadata field and the unfilled `docs/AI_DISCLOSURE.md:19` template row); `project.godot` has no `[internationalization]` section (direct read, lines 1-40); 196 narrative JSON files (`find ... -name "*.json" | wc -l` = 196); 83 `*Panel.cs` files / 97 total `.cs` files in `src/UI/` (flat directory, no subfolders except an unrelated `__pycache__`); `docs/architecture/dual_engine_plan.md:25-29` contains the real port table; the four Step 2 class names (`DailyBriefingReportBuilder`, `JournalVoice`, `ProceduralEulogyEngine`, `TradeTellEngine`) all exist exactly as named in `Assets/Ashfall.Core/Campaign/`, `Journal/`, `Journal/`, and `Economy/` respectively; `HostDefaults.cs` genuinely contains standalone sibling classes (`FileSystemIO`, `SystemTextJsonSerializer`, `ConsoleLog`, `NullLog`, `SimClock`, `SeededRng`...) with no enclosing `HostDefaults` class.
2. **Factual correction: `Ports.cs` is 49 lines, not 46.** Fixed the citation in Step 3.
3. **Factual correction: `Main.cs` is 7,014 lines, not "~6.5k."** AGENTS.md's own H7 figure is a stale rounding; fixed the citation in Step 4's Risk/Rollback note to cite the exact current count and flag the source figure as stale.
4. **Real defect found and fixed: Step 5's "10 target panels" list used descriptive names that do not match real class names, and the plan's own hedge ("verify before starting") understated the problem.** Checked all 83 real filenames directly. Concrete findings:
   - "Main HUD" is not a `*Panel.cs` file at all — the real class is `GameHudOverlay.cs`.
   - "Survivor list panel" → real class is `SurvivorsPanel.cs` (plural), easily confused with `SurvivorDetailPanel.cs`.
   - "Trade session panel" does not exist under that name; the closest real match (confirmed via its own class doc-comment) is `TradeDetailPanel.cs`, not the also-plausible `CaravanBarterLedgerPanel.cs`.
   - "Expedition briefing panel" does not exist; no panel has "briefing" in its name. Real candidate is `ExpeditionPanel.cs`.
   - "Daily briefing panel" is the worst miss: the real class is `DailyBriefingModal.cs` — a **Modal**, not a Panel. A plan that instructs someone to migrate "10 panels" cannot silently include a modal without saying so; this would have caused real confusion or a wrong-file edit during implementation.
   - Fixed: Step 5 now lists the 10 real filenames with explicit reasoning for each substitution, and flags slot 10 (`DailyBriefingModal.cs`) as needing an explicit decision (keep it and correct the step's framing, or swap it for an uncovered real panel) rather than leaving it ambiguous.
5. **String-literal count precision walked back.** Independently re-ran a raw-literal count across `src/UI/*.cs` with a slightly different regex and got 6,914 total / 1,678 space-containing literals, vs. the document's previously-cited 6,258/1,181. Both counts are regex-approximation artifacts of "what counts as a string literal" and "what counts as human-readable," not a real disagreement about the codebase. Fixed the wording to state a range and explicitly say neither number is precise, rather than presenting either as a verified figure.
6. **No overwrite risk found.** None of the three docs this plan proposes creating (`docs/localization-architecture.md`, `docs/narrative-localization-strategy.md`) or Batch 59's `docs/determinism-audit.md` currently exist — confirmed by directory listing of `docs/`. `docs/architecture/` exists and currently contains only `dual_engine_plan.md`, consistent with the plan's references.
7. **No `scripts/` policy violation.** AGENTS.md's git-rules section warns "`scripts/` directory is included in `.csproj` but empty — do not add to it without understanding why," which at first read looks like it could block Step 2's proposed `scripts/tools/extract-hardcoded-strings.sh`. Checked directly: `scripts/` is not empty (contains `audit_assets.py`, `ci/`, `pipeline/`, etc.) and the `.csproj` inclusion (`scripts/**/*.cs`) only compiles `.cs` files; a new `.sh` file under a new `scripts/tools/` subfolder does not conflict with that rule. No fix needed, but noting this was checked since the AGENTS.md wording is easy to misread as a blanket "don't touch `scripts/`" rule.
