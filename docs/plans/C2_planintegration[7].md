# C2 — Flagship Integration Plan [7]: One Language of Strings, Localization Seam, Extraction, and Typography

> Deliverable: `C2_planintegration[7].md`
>
> Source scope: Plan 25 — One Language of Strings: Make Every Word Reachable
>
> Wave: Continuity Wave 3 — Ship It Intact (Plans 25–29)
>
> Predecessors: Wave 1 narrative continuity and Wave 2 physical continuity
>
> Execution order: 25A → 25B → 25C
>
> Status: planning only. This document does not authorize a production package,
> edit the live integration ledger, or claim another agent’s paths.

## 0. Executive Intent

ASHFALL already has a partial localization implementation. The historical
Plan 25 diagnosis that there is no translation API, no project configuration,
and no translation asset is stale at the current repository baseline.

The actual repair is therefore an integration and completion task:

    existing Core LocalizationService
        → existing Godot AshfallLocalization bridge
        → one authoritative English key table
        → locale translations and data overlays
        → formatted UI, briefing, radio, journal, and catalog text

The flagship outcome is:

> Every player-facing word has one reachable key or stable data-overlay
> identity, resolves through one host path, has a visible fallback, preserves
> semantic parameters and voice, renders in declared scripts, and is protected
> by focused CI gates.

The work is not complete when a locale dropdown, a CSV, a pseudo-locale, or a
single translated catalog exists. It is complete when the lookup, content,
font, layout, determinism, and regression gates agree.

This plan is intentionally conservative about current shared paths. The
current integration ledger is still marked COMPLETE (presented for
acceptance), and no Plan 25 package is registered. A foreman must accept this
plan and assign disjoint claims before implementation begins.

# 1. Objective

## 1.1 Bounded outcome

Implement Plan 25 in three ordered workstreams:

1. 25A — harden and document the existing localization seam, key contract,
   fallback policy, formatting contract, catalog generation, and CI entry
   point.
2. 25B — extract live host/UI/briefing wording into keys and arguments without
   editorial rewriting.
3. 25C — add stable-ID data overlays, typography readiness, pseudo-locale
   rendering, layout validation, caption policy, and locale readiness status.

The final system must preserve:

- Core engine neutrality;
- JSON mechanical authority;
- existing save ownership;
- deterministic simulation and checksum behavior;
- existing narrative voice and prose;
- keyboard/controller and accessibility behavior;
- current C1 economy ownership and shared-path claims.

## 1.2 Non-goals

This plan does not authorize:

- translation of every string into a real shipping language;
- a rewrite of the gameplay JSON authority shape;
- localization of internal IDs, result codes used only by logic, file paths,
  commands, or developer diagnostics;
- copy editing during extraction;
- a second parallel localization service or per-subsystem resolver;
- a new localization save file;
- locale-dependent save/checksum serialization;
- a store-supported locale declaration before pseudo-locale, glyph, layout,
  fallback, and coverage gates pass;
- Unity restoration or Unity dependency work;
- speculative migration of panels shelved by Plan 16A;
- direct editing of active Campaign/DailyBriefing shared seams without a
  foreman-assigned integrator claim.

## 1.3 Acceptance language

“Keyed” means a stable key is present in the authoritative string table and
the runtime reaches it through the host resolver.

“Overlay-localized” means a stable mechanical definition ID selects an
expressive-field delta through the approved locale overlay loader, with the
base definition remaining the English fallback.

“Supported locale” means a locale that passes the status matrix. A locale
with partial translations or QA pseudo text is not automatically store-ready.

# 2. Current Repository Reality

## 2.1 Premise correction

The historical source plan was written against an earlier baseline at
`ccac926e`. Current evidence shows that a Wave 1 localization pilot already
exists:

- Core authority: `Assets/Ashfall.Core/Localization/LocalizationService.cs`;
- Godot bridge: `src/Localization/AshfallLocalization.cs`;
- English/source table: `assets/l10n/strings.csv`;
- imported translation resources:
  `assets/l10n/strings.en.translation`,
  `assets/l10n/strings.de.translation`, and
  `assets/l10n/strings.source.translation`;
- pilot contract: `docs/L10N_CONTRACT.md`;
- readiness plan: `docs/i18n/LOCALIZATION_PLAN.md`;
- UI readiness notes: `docs/ui/LOCALIZATION_READINESS.md`;
- current drift tooling:
  `scripts/ci/extract_l10n_inventory.py` and
  `scripts/ci/l10n_drift_gate.py`.

The implementation task must extend these seams. It must not create
`src/L10n/AshfallText.cs`, `src/L10n/LocaleBootstrap.cs`, a duplicate Core
service, or a second translation asset root merely because the historical
plan named those paths.

## 2.2 Measured baseline on 2026-09-16

These figures are inventory probes, not completion claims:

| Probe | Current result | Interpretation |
|---|---:|---|
| `assets/l10n/strings.csv` rows | 359 keys | Existing pilot/master table |
| `l10n_drift_gate.py` | PASS — 359 keys, 69 pilot references, German parity | Pilot gate is healthy |
| focused localization tests | 16 passed, 0 failed, 0 skipped | Existing Core localization contracts pass |
| raw `Text`/`TooltipText` matches in `src/UI` | 493 lines | Crude candidate surface; requires classification |
| files with those UI matches | 129 files | Long-tail extraction surface |
| raw `Text`/`TooltipText` matches in `src/Host` | 15 lines | Crude host candidate surface |
| raw string/return matches in `src/Main*.cs` | 113 lines | Crude Main/briefing candidate surface |
| direct `AshfallLocalization` calls | 21 lines | Existing adoption is selective |
| `ActionResult` construction matches in Core | 1,599 lines | Crude result/message inventory; not all are user-facing |
| current CI manifest gate IDs | 50 | No localization gate is registered in the manifest |
| font `.import` fallback arrays | empty | No explicit project fallback chain |

The raw counts include false positives and must not be used as acceptance
numbers. The first implementation package must generate a deterministic
classified inventory with source locations and an explicit exception class.

## 2.3 Existing runtime behavior

`LocalizationService` currently provides:

- an engine-free singleton;
- current-locale state, defaulting to `en`;
- English fallback strings;
- locale translation dictionaries;
- CSV loading;
- positional and named formatting helpers;
- pseudo-locale generation;
- missing-key notification and visible markers.

`AshfallLocalization` currently provides:

- idempotent initialization;
- Godot `TranslationServer` locale application;
- `Tr`, `TrFormat`, and `TrNamed`;
- CSV/resource loading;
- locale-change forwarding;
- visible missing-key logging.

Important gaps remain:

- English fallback text is also embedded in the Core
  `LoadDefaultEnglishStrings()` method, duplicating the CSV authority;
- `Format` is positional and invariant-culture based;
- named formatting does not currently reject missing or extra placeholders;
- locale validation and supported-locale policy are not centralized;
- helper controls accept raw strings but have no key-aware overloads;
- the current inventory/drift scripts intentionally cover only the pilot;
- the pseudo-locale is runtime-generated and is not yet a complete render gate;
- data catalogs have no generic expressive-field overlay contract;
- the project has no explicit authored font fallback chain;
- no localization gate is registered in `docs/ci/CI_GATE_MANIFEST.json`.

## 2.4 Existing content seams

Current keyed or semi-keyed content includes:

- onboarding strings resolved in `src/Main.Onboarding.cs`;
- wildlife/trapping key helpers in
  `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs`;
- micro-location localization and stable IDs;
- radio signal key contracts and focused localization tests;
- multiple UI pilot surfaces;
- `ActionResult.MessageKey` values returned by Core systems.

Current raw or partially structured content includes:

- `DailyBriefingEntry` wording built in `src/Main.Campaign.cs`;
- host/session status wording;
- inventory and expedition feedback;
- journal, cassette, narrative progression, and collectible prose;
- catalog display names and descriptions embedded in base JSON;
- many Main/UI panel labels and status lines.

The plan must classify an existing key as either a user-facing translation key
or an internal code before changing its owner. It must not translate a logic
identifier simply because its value resembles English.

# 3. Source Evidence and Required Reading

The implementation foreman and each builder must re-check current evidence
before claiming paths:

1. `C1_COMPLETION.md`;
2. `INTEGRATION_PLANS.md`;
3. `WORKTREE_OWNERSHIP.md`;
4. `TEST_POLICY.md`;
5. `KNOWN_DEBT.md`;
6. `AI_AGENT_WORKFLOW.md`;
7. `docs/CURRENT_AUTHORITY.md`;
8. the relevant existing localization contracts.

The historical source is:

`Next-steps-plans/shipped_to_chat/Plan_25_Localization_One_Language_Of_Strings.md`

It remains useful for intent and acceptance vocabulary, but its recorded
baseline is not proof of current APIs or missing infrastructure.

The current plan must also cross-check:

- `docs/L10N_CONTRACT.md`;
- `docs/i18n/LOCALIZATION_PLAN.md`;
- `docs/L10N_WAVE2_ROADMAP.md`;
- `docs/ui/LOCALIZATION_READINESS.md`;
- `docs/journal/JOURNAL_VOICE_RUNTIME_CONTRACT.md`;
- `docs/narrative/CASSETTE_SET_RUNTIME_CONTRACT.md`;
- `docs/narrative/NARRATIVE_PROGRESSION_RUNTIME_CONTRACT.md`;
- `docs/collectibles/COLLECTIBLES_CONTENT_INTEGRATION_CLOSEOUT.md`;
- `docs/discovery/MICRO_LOCATION_LOCALIZATION.md`;
- `docs/radio/SIGNAL_LOCALIZATION_AUDIT.md`.

Where those documents disagree, source/data/runtime evidence wins and the
disagreement becomes a migration note or stale-document finding.

# 4. Baseline Capture

## 4.1 Required pre-implementation capture

Before production edits, record:

- current branch and working-tree state;
- current claims and shared paths;
- all existing localization keys and source locations;
- raw player-facing literal candidates by file and category;
- direct resolver calls;
- `ActionResult.MessageKey` producers and renderers;
- briefing/guidance semantic producers;
- data catalogs and expressive fields;
- current locale settings persistence;
- current translation resources and their generation path;
- current fonts, imports, theme assignment, and fallback behavior;
- current CI manifest and script ownership.

The baseline report must distinguish:

- `UI_LABEL`;
- `UI_SENTENCE`;
- `STATUS_LINE`;
- `DIEGETIC`;
- `DEV_ONLY`;
- `INTERNAL_CODE`;
- `DATA_EXPRESSIVE_FIELD`;
- `UNKNOWN_REVIEW`.

## 4.2 Focused evidence commands

The following are bounded baseline checks. They are not a request to run the
full repository test suite:

    python3 scripts/ci/l10n_drift_gate.py
    bash scripts/run_test.sh Ashfall.Core.Tests/Localization/
    python3 scripts/ci/extract_l10n_inventory.py
    rg -n 'Text[[:space:]]*=[[:space:]]*"|TooltipText[[:space:]]*=[[:space:]]*"' src/UI src/Host --glob '*.cs'
    rg -n 'AshfallLocalization\.(Tr|TrFormat|TrNamed)\(' src --glob '*.cs'
    rg -n 'MessageKey|DailyBriefingEntry|briefing|guidance' Assets/Ashfall.Core src --glob '*.cs'
    rg -n '"(display_name|description|title|body|text|transcript|prose)"' Assets/StreamingAssets/Data --glob '*.json'
    find assets/fonts -maxdepth 1 -type f -name '*.ttf' -o -name '*.otf'

The existing baseline result is:

    L10N_DRIFT_GATE PASS — 359 keys, 69 pilot references, German parity verified
    localization focused target — 16 passed, 0 failed, 0 skipped

No full-suite result is implied by this plan.

## 4.3 Baseline artifacts

Create, under an accepted implementation claim:

- `artifacts/l10n/plan25-baseline.json`;
- `artifacts/l10n/plan25-literal-inventory.json`;
- `artifacts/l10n/plan25-data-field-inventory.json`;
- a short baseline section in the closure report.

Artifacts must be reproducible and must not contain secrets, user paths
outside the repository, or generated prose copies that become a second
authority.

# 5. Architectural Invariants

## 5.1 One key namespace

Use one documented syntax:

    domain.surface.element

Components are lowercase and snake_case where a component contains multiple
words. Examples:

    briefing.survivor.hungry
    inventory.action.repair
    guidance.expedition.prepare
    weather.forecast.confidence

Existing valid prefixes such as `ui.*`, `warning.*`, `tutorial.*`, and
`settings.*` are retained. The ADR must define how older valid keys are
classified and how renames are deprecated. Do not mass-rename stable keys for
cosmetic consistency.

## 5.2 Keys are identifiers, not prose

Do not derive keys from full English sentences. A key may survive a wording
change; an editorial change must be separately reviewable.

## 5.3 Core carries, host resolves

Core may carry a stable text key and structured arguments. Core must not call
Godot APIs, inspect `TranslationServer`, read locale files, or format
player-facing text as a side effect of simulation.

## 5.4 English is one authority

The canonical English value must live in the approved localization source
table or the approved base-data expressive field. It must not be duplicated in
runtime fallback code. Existing `LoadDefaultEnglishStrings()` content must be
parity-checked and then migrated or removed in a dedicated step.

## 5.5 Formatting is parameterized

New dynamic strings use named placeholders such as:

    {name}
    {value}
    {unit}
    {count}

Translation determines argument order. Existing positional placeholders may be
supported during migration, but new code must use the named contract unless
the ADR records an exception.

## 5.6 Storage is culture-invariant

Save files, checksums, replay fingerprints, event IDs, RNG state, and
mechanical serialization remain invariant. Locale-aware formatting is confined
to display adapters.

## 5.7 Data overlays are expressive deltas

Base JSON remains authoritative for IDs and mechanics. An overlay may replace
only an approved expressive field. It may not add or alter quantities,
requirements, costs, flags, probabilities, progression, or other mechanical
properties.

## 5.8 Missing content is visible

Unknown keys, missing required fallback text, placeholder mismatch, invalid
overlay IDs, missing glyphs, and layout failures must be visible in
development/CI. No resolver may return an empty string for an unresolved
player-facing key.

## 5.9 Locale support requires render support

A locale is not supported merely because its table exists. The status matrix
must include key coverage, fallback count, glyph coverage, pseudo/real render
results, layout tolerance, and caption policy.

# 6. Required Delta from the Current Baseline

Plan 25 must deliver the following deltas:

1. one authoritative key and argument contract across the existing pilot,
   Core message results, host status, UI helpers, and briefing projections;
2. one English source of truth, with the Core hardcoded catalog treated as a
   migration duplicate rather than a second authority;
3. centralized supported-locale and fallback policy;
4. key-aware helper overloads with migration-only raw-string overloads;
5. named-placeholder validation and a display-formatting seam;
6. a changed-files no-new-user-facing-literal gate;
7. a volume-ranked extraction inventory and batched migration;
8. a stable-ID locale overlay contract for expressive catalog/narrative data;
9. overlay integrity and mechanical-ID protection;
10. explicit font fallback and script/glyph probes;
11. pseudo-locale and layout-tolerance CI;
12. paired-locale determinism verification;
13. a readiness matrix that prevents premature store claims.

# 7. Workstream 25A — Localization Seam

## 7.1 25A objective

Harden the current `LocalizationService` → `AshfallLocalization` path so new
strings are cheap to add and all later extraction can use one contract.

## 7.2 25A.1 Key convention ADR

Update or supersede the current localization contract with a short ADR that
defines:

- namespace and component syntax;
- key ownership by category;
- named argument naming and type rules;
- legacy positional placeholder policy;
- plural/ordinal strategy;
- locale tag normalization and fallback;
- internal-code versus user-facing-key classification;
- key rename/deprecation policy;
- developer notes and register metadata;
- source-table versus generated-resource ownership.

The ADR must explicitly retain established stable keys and document any
translation-key aliases. It must not rename keys in the same change as a
large UI extraction.

## 7.3 25A.2 Core contract

Audit whether the current string `ActionResult.MessageKey` contract is enough
for the next migration. The preferred additive shape is:

    TextKey
        stable value used by new Core-facing contracts

    LocalizedMessage
        TextKey + immutable named arguments + optional semantic metadata

The implementation decision must be evidence-based:

- if multiple Core systems need typed key/argument transport, add engine-free
  value types under `Assets/Ashfall.Core/Localization/` and preserve existing
  string properties for compatibility;
- if existing string keys plus a separate immutable argument record are
  sufficient, do not add a wrapper solely for naming;
- never force Godot types into Core;
- never change save/checksum representation to include rendered text.

`ActionResult.MessageKey` remains compatible with current tests and callers.
Existing values are classified before migration. User-facing values become
resolver inputs; internal codes remain logic values and are never translated.

## 7.4 25A.3 Canonical host resolver

Extend `src/Localization/AshfallLocalization.cs` as the one host-facing
resolver. It must own or delegate:

- requested-locale resolution;
- English master fallback;
- named argument formatting;
- current display-culture formatting;
- missing-key diagnostics;
- placeholder-contract diagnostics;
- locale-change notification;
- optional development key highlighting.

All production UI, host, and presentation adapters must call this seam. Direct
`TranslationServer` calls outside this adapter are prohibited after migration.

The resolver policy is:

    requested locale
        → language-only fallback
        → English master
        → loud unresolved marker and diagnostic

During migration, an explicit `defaultText` argument may remain only at
approved legacy sites and must be reported by the inventory. New call sites
must not embed the English fallback in C#.

## 7.5 25A.4 English master and generated resources

Choose `assets/l10n/strings.csv` as the source table for host/UI keys unless
the ADR finds a repository-canonical generator that supersedes it. Imported
Godot translation resources are generated/packaged outputs, not a second
hand-edited English authority.

The migration must:

- compare `LoadDefaultEnglishStrings()` with the source table;
- report missing, duplicate, and conflicting values;
- move any still-live unique values into the source table;
- retain exact English values during extraction;
- remove or disable the duplicate Core source only after focused parity tests;
- preserve `source` metadata without treating it as a translation locale.

The change must not silently delete a key that a current route still resolves.

## 7.6 25A.5 Locale bootstrap and settings

Use the existing settings persistence architecture. Current locale selection
must remain a user setting, not campaign state.

Bootstrap responsibilities:

- normalize system locale;
- honor a valid user override;
- reject unsupported/invalid tags to English;
- apply the locale before player-facing panels first render;
- persist through existing `user://settings.json` ownership;
- emit one refresh signal when the effective locale changes.

Initial QA locales are `en`, `de`, and `pseudo` according to current
contracts. German remains a skeleton/QA pack until the status matrix and
product approval say otherwise. Latvian, Cyrillic, CJK, or other real locales
must not be declared supported until fonts and render tests exist.

Do not create a localization-specific save file or a second settings store.

## 7.7 25A.6 Project configuration

Keep the existing `[internationalization]` resource configuration in
`project.godot` aligned with the generator/import path. Validate:

- English boot;
- QA German boot;
- pseudo-locale boot;
- invalid-locale fallback;
- exported/resource-pack inclusion.

The project configuration must not become an unreviewed hand-maintained list
that diverges from the source table.

## 7.8 25A.7 Key-aware UI helpers

Extend `src/UI/AshfallUiHelpers.cs` with key-aware overloads for the controls
that already centralize presentation:

- labels;
- buttons;
- section/header rows;
- tooltips;
- modal titles;
- metadata/status rows.

The helper resolves text at assignment/refresh time and does not own gameplay
state. Raw string overloads may remain as migration-only APIs, but:

- they are marked/documented as legacy;
- new code cannot use them for player-facing copy;
- the changed-files gate reports new raw-literal use;
- locale-change refresh does not require panels to duplicate resolver logic.

## 7.9 25A.8 Formatting

Separate:

- invariant serialization/checksum formatting;
- locale-aware display formatting;
- text-template substitution.

Named argument validation must reject:

- a missing required placeholder;
- an unknown supplied argument;
- an accidental duplicate where the contract disallows it;
- malformed placeholder syntax.

Numeric/date/unit display goes through one host display-formatting helper.
Core calculations and save serializers remain invariant.

Pluralization must use the chosen translation format’s rules. Do not encode
English plural grammar in a global helper.

## 7.10 25A.9 Development key highlighting

Add a development-only mode with one or more of:

- key shown beside the rendered value;
- key-only rendering;
- missing-key styling.

The mode must be presentation-only, must not ship as a store feature unless
explicitly approved, and must not affect save state or simulation behavior.

## 7.11 25A.10 Catalog generator and gate

Create or extend the catalog generator so it produces:

- `docs/l10n/STRING_CATALOG.md`;
- a translator handoff format selected by the ADR;
- deterministic source metadata;
- key, English value, argument names, category, developer note, and source
  file list.

The generator must be deterministic: stable path ordering, stable key
ordering, invariant serialization, and no timestamps in generated content.

`--check` must fail on:

- stale generated output;
- duplicate conflicting keys;
- missing English fallback;
- placeholder mismatch;
- source key references absent from the master table;
- generated output differing from a clean run.

Extend the existing drift gate instead of registering a competing gate.

## 7.12 25A acceptance

25A passes only when:

- the ADR is current and cites the existing pilot;
- one host resolver is authoritative;
- Core remains engine-free;
- locale settings use existing persistence;
- English fallback is singular and visible;
- key-aware helpers exist;
- new placeholder validation exists;
- catalog generation is deterministic;
- missing-key behavior is loud;
- focused Core/host tests pass;
- the gate is registered through the repository CI manifest by the named
  integrator.

# 8. Workstream 25B — UI, Host, and Briefing Extraction

## 8.1 25B objective

Remove player-facing English from live production source in volume-ranked,
reviewable batches while preserving prose exactly.

## 8.2 25B.1 Volume-ranked inventory

Replace the current pilot-only inventory with a report ranked by:

1. player-facing literal sites removed;
2. live route coverage;
3. shared helper leverage;
4. number of affected controls;
5. review risk.

Do not process alphabetically. The report must identify shelved Plan 16A
panels and exclude them unless the same text is player-facing through a live
route.

## 8.3 25B.2 Classification

Every candidate becomes exactly one of:

- `UI_LABEL`;
- `UI_SENTENCE`;
- `STATUS_LINE`;
- `DIEGETIC`;
- `DEV_ONLY`;
- `INTERNAL_CODE`.

The scanner must allow documented patterns for logs, tests, commands, paths,
diagnostics, and internal codes. Arbitrary file or line exemptions are not
allowed.

## 8.4 25B.3 UI labels and controls

Extract navigation labels, button captions, state labels, section headers,
tooltips, modal headings, warnings, and empty states. Preserve:

- capitalization and uppercase styling;
- punctuation;
- bracket conventions;
- intentional terse industrial tone;
- accessibility names and hotkey labels.

Style belongs to the control/theme. The translation value must not be
rewritten merely to force visual casing.

## 8.5 25B.4 Sentence extraction

Replace interpolated wording with a key and semantic arguments.

Before:

    “{name} is hungry ({value:F0}% hunger).”

After:

    key: briefing.survivor.hungry
    args: name, value

The translation controls order and grammar. The English rendered result must
match the pre-extraction result modulo the approved placeholder representation
and display-formatting boundary.

Do not concatenate localized fragments. Do not pass already-rendered English
sentences as the canonical event payload.

## 8.6 25B.5 Host status and ActionResult rendering

Audit `src/Host/**`, `src/Main*.cs`, and all direct `MessageKey` renderers.

Convert player-facing status paths to:

    key + named arguments + semantic status

Keep low-level diagnostics and internal result codes out of the localization
table. A host must not display `result.MessageKey` raw merely because the
property exists. It must classify the value and resolve it only when it is a
user-facing key.

The migration must preserve status enum behavior, retry/blocked semantics,
and event ownership.

## 8.7 25B.6 Briefing builder

`DailyBriefingReportBuilder` and its Campaign projections are shared seams.
They are integrator-only until the current claim is accepted and a new claim
is assigned.

The target shape is additive:

    DailyBriefingEntry
        semantic kind/category
        stable key
        named arguments
        entity IDs
        route/severity metadata

The old rendered English field may remain as a migration compatibility field
only while every live consumer is moved and equality tests exist. It must not
become the new authority.

Plan 17 semantic kinds remain separate from wording keys. A semantic kind may
select different keys by context without changing the underlying campaign
fact.

## 8.8 25B.7 Guidance and new Wave-1 strings

Plan 17 briefing/guidance additions must be born keyed after 25A. If a
briefing or guidance package is already claimed, the integrator must merge the
key/argument seam at the shared boundary rather than having two builders
modify the same file.

## 8.9 25B.8 Prose equality guard

For each extraction batch, capture the old rendered English output and compare
it with the new resolver output using a structural placeholder substitution.

The guard must detect:

- changed words;
- changed punctuation;
- changed whitespace that affects presentation;
- lost or reordered parameter values;
- accidental number formatting changes;
- missing status context.

Editorial changes are separate commits with explicit content review. An
extraction PR cannot hide a copy edit inside a localization migration.

## 8.10 25B.9 Near-duplicate review

Generate a semantic duplicate candidate report, but merge keys only after
human review. Similar strings such as “no stock”, “none held”, and “shelves
bare” may carry different gameplay or diegetic meaning. Preserve faction and
narrative voice.

Record approved aliases/merges in migration notes and keep compatibility
aliases until all routes are moved.

## 8.11 25B.10 No-new-literal ratchet

Add a changed-files-first gate for:

- `src/UI/**`;
- `src/Host/**`;
- `src/Main*.cs`;
- other explicitly declared player-facing host/presentation roots.

The gate compares the patch to the baseline and fails new unclassified
player-facing literals. The legacy backlog may remain temporarily, but it
cannot grow.

Exceptions are pattern-based and documented:

- tests;
- logs;
- internal codes;
- file paths;
- commands;
- developer diagnostics.

The gate must emit source locations and category decisions for every
exception.

## 8.12 25B.11 Glossary and register

Create `docs/l10n/GLOSSARY.md` with established terms such as:

- dose ledger;
- brine water;
- sick list;
- holdfast;
- muster;
- chelation;
- fallout;
- decontamination;
- brownout;
- ration.

Each entry contains the canonical English term, definition, translation note,
and a “do not translate” rule where needed. Narrative register metadata is
maintained separately or linked from the existing faction voice authority.

## 8.13 25B.12 Snapshot, layout, and accessibility review

For each touched panel:

- review English baseline;
- render an expanded-text fixture of at least +30%;
- render a compressed fixture of at least −25%;
- render pseudo-locale text;
- check clipping, overlap, wrapping, minimum sizes, scroll behavior,
  tooltip overflow, and icon/text collisions;
- confirm focus, close/back behavior, keyboard/controller navigation, and
  accessible names;
- do not bless snapshots that conceal overflow.

Snapshot changes require deliberate approval notes. The snapshot tool is a
verification aid, not an automatic acceptance of layout regressions.

## 8.14 25B acceptance

25B passes only when:

- every migrated literal has a category and key policy;
- UI labels, sentences, status lines, and briefing text use the seam;
- briefing semantics are not replaced by rendered prose;
- English equality tests pass;
- the no-new-literal ratchet is active;
- the glossary and register notes exist;
- touched panels pass accessibility and expanded-layout review;
- remaining live user-facing literals are enumerated and assigned;
- no production user-facing English was silently left in a migrated scope.

# 9. Workstream 25C — Data Overlays and Typography

## 9.1 25C objective

Make authored item, location, journal, radio, memorial, epilogue, and related
text translatable without duplicating or destabilizing mechanical JSON.

## 9.2 25C.1 Overlay ADR

Use the existing `assets/l10n/` root rather than introduce a competing
`assets/localization/` authority. The recommended overlay layout is:

    assets/l10n/overlays/
        en/
            items.json
            locations.json
            narrative.json
            radio.json
        de/
            ...
        pseudo/
            ...

The exact catalog partition is an ADR decision, but every overlay entry must
be keyed by a stable base definition ID. A conceptual entry is:

    {
      "schema_version": 1,
      "catalog": "items",
      "entries": {
        "item_gas_mask": {
          "display_name": "Gas Mask",
          "description": "..."
        }
      }
    }

This is an overlay schema, not a rewrite of the base gameplay JSON shape.

## 9.3 25C.2 Expressive-field whitelist

Start with an explicit whitelist, for example:

- `display_name`;
- `description`;
- `title`;
- `body`;
- `text`;
- `transcript`;
- `prose`;
- `memorial`;
- `epilogue`.

The actual list must be derived from current catalogs and their consumers.
Fields such as IDs, quantities, costs, prerequisites, tags used by logic,
flags, probabilities, effects, and route keys are forbidden.

An overlay may not create a new gameplay definition. It may not change a
mechanical field by adding a same-named value outside the whitelist.

## 9.4 25C.3 Loader contract

The catalog text resolver follows:

    requested locale overlay
        → language-only locale overlay
        → base definition expressive field
        → visible integrity/development error if required text is absent

Overlay loading is indexed and performed once per locale/catalog load. It must
not reparse JSON on every render. Locale changes invalidate the overlay cache
and trigger presentation refresh through the existing host lifecycle.

The loader must retain raw base text as the English fallback until overlay
coverage is proven.

## 9.5 25C.4 Content migration boundaries

Existing content contracts intentionally use raw strings in some journal,
cassette, progression, collectible, micro-location, and radio paths. Plan 25C
must reconcile those contracts explicitly:

- preserve stable IDs and current fallback behavior;
- avoid a second per-subsystem translation resolver;
- migrate expressive fields only through the approved shared overlay loader;
- preserve register and voice metadata;
- migrate in catalog batches with focused parity tests;
- leave mechanical data and save payloads unchanged.

The first migration batch should use a small representative catalog, including
one item/display pair and one narrative/transcript pair. Do not begin with all
catalogs or a 411-shape rewrite.

## 9.6 25C.5 Overlay integrity

Extend the existing catalog integrity path or its canonical validator to
reject:

- unknown base IDs;
- duplicate conflicting overlay entries;
- invalid locale tags;
- fields outside the expressive whitelist;
- missing required fallback text;
- malformed placeholders;
- overlay files omitted from the exported resource pack;
- mechanical fields present in overlay entries.

An orphan overlay is a hard integrity failure. A missing optional translation
falls back to the base English expressive field and increments the readiness
fallback count.

## 9.7 25C.6 Mechanical ID protection

Locale values and translator metadata must never replace or translate
mechanical IDs. Protect sanctioned prefixes and the complete current ID
grammar, including at least:

- `item_`;
- `loc_`;
- `quest_`;
- `flag_`;
- `radio_`;
- `echo_`.

If an ID is shown to a translator, store it in an isolated metadata position,
never in a translation-value position. The gate must report the exact overlay
file, entry, and field on failure.

## 9.8 25C.7 Register metadata

Each narrative catalog or entry family must identify the register required by
translation:

- clipped military;
- bureaucratic;
- devotional;
- technical;
- exhausted civilian;
- propagandistic.

Reuse existing faction/narrative voice documentation. Do not flatten
diegetic voice into generic UI wording. The overlay format may carry a
translator note, but the note is not player-facing text.

## 9.9 25C.8 Typography audit

Audit the current Barlow Condensed and ShareTech Mono fonts and their Godot
imports. Current evidence shows `allow_system_fallback=true` but empty
explicit fallback arrays; this is not sufficient as a tested product
fallback chain.

For the initially declared QA set, verify:

- Latin glyphs;
- Latvian diacritics if Latvian text is declared for testing;
- German characters;
- UI punctuation and symbols;
- mono-font compatibility for numeric/status surfaces.

Cyrillic and CJK remain candidate script tiers until an approved font family,
fallback order, import settings, and render probe exist.

Configure fallback at the theme/project level. Do not make every panel select
fonts independently. Verify weight, condensed width, hinting, antialiasing,
fallback order, and resource-pack inclusion.

## 9.10 25C.9 Layout tolerance matrix

Run the UI suite under:

- normal English;
- +30% expanded text;
- −25% compressed text;
- pseudo-accented locale;
- each declared target-script smoke sample.

Record per panel:

- clipping;
- overlap;
- wrapping;
- minimum-size break;
- tooltip overflow;
- scroll failure;
- focus/accessible-name regression;
- icon/text collision.

Pseudo expansion must not be defeated by shortening the pseudo text.

## 9.11 25C.10 Audio and caption parity

For radio, voice, cassette, and other spoken content, establish:

    VO present + localized text
        → captions/transcript always available

If localized VO is absent, retain localized text and use source-language VO
only under an explicit product policy. The status matrix must report VO,
caption, and transcript coverage separately. No audio cue ID is translated.

## 9.12 25C.11 Numbers, dates, and units

Create or extend one display-formatting helper for:

- percentages;
- decimal separators;
- dates;
- quantities;
- temperatures;
- dose units;
- time/duration;
- named numeric arguments.

The same action under different locales must serialize the same mechanical
value and checksum. Only the display projection changes.

## 9.13 25C.12 Pseudo-locale

The pseudo-locale must:

- accent supported letters;
- expand text by the declared target range;
- preserve named placeholders;
- preserve markup/brackets where required;
- make untranslated/unkeyed text visible;
- exercise fallback fonts and layout.

Generation must be deterministic and available in CI. Runtime-only pseudo
generation is insufficient unless the render gate can invoke it headlessly.

## 9.14 25C.13 Readiness status

Create a generated localization status page, for example:

    docs/l10n/LOCALIZATION_STATUS.md

Report per locale:

- total host/UI keys;
- translated keys;
- fallback count;
- data overlay coverage;
- orphan/invalid entries;
- placeholder status;
- glyph status;
- pseudo/render status;
- layout status;
- VO/caption policy;
- store-ready yes/no and the blocking gate.

`de` may remain a QA/skeleton status until all required evidence passes.
`pseudo` is a development readiness locale, never a store locale.

## 9.15 25C acceptance

25C passes only when:

- the overlay ADR is approved;
- base JSON remains mechanical authority;
- expressive fields resolve through the overlay loader;
- fallback and orphan behavior are tested;
- IDs are protected;
- voice/register metadata survives;
- explicit font fallback is configured and tested;
- pseudo-locale and layout gates pass;
- caption/transcript policy is documented;
- culture-aware display formatting is separate from serialization;
- readiness status is generated from gates;
- no store claim exceeds the status matrix.

# 10. Integrated Data and Runtime Flow

## 10.1 Host/UI flow

    Core action/event
        → TextKey or classified MessageKey + named arguments
        → host AshfallLocalization
        → requested locale
        → language fallback
        → English master
        → display formatting
        → UI/helper/accessibility name

## 10.2 Briefing flow

    Core/campaign fact
        → semantic briefing kind + entity IDs + typed arguments
        → host briefing projection
        → AshfallLocalization
        → localized briefing row
        → existing route/severity presentation

## 10.3 Authored-data flow

    Base JSON
        → IDs and mechanics
        → expressive English fallback
        → stable-ID locale overlay
        → CatalogTextResolver
        → journal/radio/item/memorial/epilogue UI

The overlay resolver and host key resolver share locale policy and diagnostics;
they are not two competing translation APIs.

## 10.4 Locale-change flow

    settings override/system locale
        → LocaleBootstrap
        → AshfallLocalization effective locale
        → TranslationServer/application resource update
        → overlay cache refresh
        → existing panel refresh/disposal lifecycle

No simulation event, RNG draw, save mutation, or campaign transition is
triggered by a locale change.

# 11. Ownership Matrix

| Concern | Authority | Planned change | Forbidden duplicate |
|---|---|---|---|
| Core key/message contract | `Assets/Ashfall.Core/Localization/` and existing `ActionResult` | Additive typed contract only if audit proves need | Godot types or a second message model |
| Host lookup | `src/Localization/AshfallLocalization.cs` | Extend as canonical facade | `src/L10n/AshfallText.cs` parallel resolver |
| English host/UI values | `assets/l10n/strings.csv` | Make source singular and generated outputs checked | English literals in C# fallback catalogs |
| Imported Godot resources | `assets/l10n/*.translation` | Regenerate/package from source path | Hand-edited competing translation table |
| Locale setting | existing settings owner and `user://settings.json` | Add/validate supported-locale policy | Separate localization save file |
| UI helper presentation | `src/UI/AshfallUiHelpers.cs` | Add key-aware overloads | Gameplay state in helpers |
| Action result semantics | Core result owner | Preserve statuses/codes; resolve only at host | Panels interpreting result codes ad hoc |
| Briefing semantics | Campaign/DailyBriefing owner | Add key/args projection | A second briefing builder |
| Base data mechanics | `Assets/StreamingAssets/Data/` | Leave authority shape unchanged | Overlay mechanics or copied catalogs |
| Expressive data text | locale overlay loader | Add whitelist and stable-ID overlays | Per-catalog translation services |
| Fonts/theme | `project.godot`, theme, `assets/fonts/` | Explicit fallback chain and probes | Per-panel font hacks |
| Catalog/gate generation | `scripts/ci/` and CI manifest | Extend existing drift tooling | Unregistered ad hoc checks |
| Narrative register | existing narrative/faction docs | Link translator metadata | Voice-neutral bulk rewrites |

# 12. API and Contract Design

## 12.1 Core contract

The contract must support stable keys and semantic arguments without coupling
Core to Godot. The implementation package must document:

- key validation rules;
- argument value types allowed in Core;
- immutable argument ownership;
- null/missing argument behavior;
- compatibility with current `ActionResult.MessageKey`;
- whether a `TextKey` wrapper is added;
- whether message metadata is persisted or transient.

Arguments are transient presentation inputs unless a current save owner
already persists the underlying semantic fact. Never persist a rendered
localized sentence.

## 12.2 Host contract

The canonical host API remains conceptually:

    AshfallLocalization.Tr(key, defaultText?)
    AshfallLocalization.TrFormat(key, args...)
    AshfallLocalization.TrNamed(key, namedArgs)

The implementation may rename or overload these only after inventorying
current call sites. Existing calls must remain source-compatible during
incremental migration where practical.

New code should pass a stable key and named arguments. `defaultText` is
migration-only and is reported by CI.

## 12.3 Placeholder contract

For each key:

    declared argument set == placeholders in English value
    declared argument set == placeholders in every checked translation

The validator must report key, locale, source file, and missing/extra names.
Formatting errors must never produce an empty string. Development output must
show the key and the failure.

## 12.4 Message code classification

Before changing `ActionResult` renderers, classify current message values:

- `USER_FACING_KEY`: translated through the host resolver;
- `INTERNAL_CODE`: remains stable and untranslated;
- `DIAGNOSTIC_TEXT`: remains outside player-facing localization;
- `UNKNOWN_REVIEW`: blocks migration until an owner decides.

The classification is a contract artifact, not an inference made by a regex
alone.

# 13. Save, Settings, and Determinism Contract

## 13.1 Locale state

Locale selection is presentation/settings state:

- default `en`;
- valid user override wins over system locale;
- language-only fallback precedes English;
- invalid/unsupported locale resolves to English;
- saved through the existing settings store;
- no new campaign-save section;
- no overlay cache persistence.

## 13.2 Save invariance

Localization must not alter:

- save checksum;
- replay fingerprint;
- event ordering;
- numeric state;
- IDs;
- RNG streams;
- migration version;
- resource/accounting ledgers.

## 13.3 Paired-locale test

Run the same seed and action sequence with `en` and `pseudo`:

    Run A: locale=en
    Run B: locale=pseudo
    same seed/actions
        → identical gameplay state/checksum
        → presentation text may differ

If the current simulation harness cannot change locale without a host
process, add a focused test seam rather than placing locale in Core state.

# 14. Event, Lifecycle, and Integration Wiring

## 14.1 Initialization order

The composition root must:

1. load existing settings;
2. initialize localization;
3. apply effective locale;
4. load/validate overlay indexes;
5. construct player-facing panels;
6. subscribe panels to the existing locale-change refresh lifecycle.

Initialization must be idempotent. A panel must not independently load CSV,
translation resources, or overlay JSON.

## 14.2 Refresh and disposal

Locale change must refresh mounted text-bearing controls without leaking
subscriptions. Every touched panel must preserve its current disposal path.
A locale change must not reconstruct or mutate Core gameplay owners.

## 14.3 Shared seams

The Campaign/DailyBriefing path is shared with completed/current C1 packages.
The integrator owns any change that crosses:

- `DailyBriefingReportBuilder`;
- `src/Main.Campaign.cs`;
- `src/Main.CampaignOwners.cs`;
- shared Main partials;
- CI manifests;
- generated documentation indexes.

Builders may prepare disjoint adapters, tests, inventories, and new
localization assets, but must stop before modifying a claimed shared seam.

# 15. Godot, UI, and Accessibility Integration

## 15.1 Godot boundary

Only host/presentation code may use:

- `Godot`;
- `TranslationServer`;
- `FileAccess`;
- `ResourceLoader`;
- theme/font resources.

Core contracts remain usable in `netstandard2.1` tests without a Godot
runtime.

## 15.2 UI behavior

Key-aware controls must preserve:

- current sizing and layout behavior;
- visible feedback for action results;
- keyboard/controller focus;
- close/back behavior;
- tooltip behavior;
- accessible names;
- non-color-only status meaning;
- snapshot ownership and approval flow.

No panel may recompute a Core outcome to obtain a translation key.

## 15.3 Resource packaging

Headless checks must verify that:

- CSV/master resources;
- imported translations;
- overlay files;
- fallback fonts;
- generated status/catalog outputs where packaged;

are present in the expected project/export path. A source file existing in the
repository is not proof that the exported game can load it.

# 16. Data and Narrative Integration

## 16.1 Catalog batch order

After the overlay contract passes, migrate in small batches:

1. one item/catalog display pair;
2. locations and micro-locations with stable-ID parity;
3. radio/transcript content;
4. journal/cassette/collectible content;
5. memorial/epilogue/narrative progression content;
6. remaining expressive catalogs by measured impact.

Each batch must preserve raw English output and use the same loader.

## 16.2 Existing raw-content contracts

The journal, cassette, narrative progression, and collectible contracts
currently document raw-string authority in places. They are migration inputs,
not permission to create separate `nameKey`/`descriptionKey` systems per
subsystem. The integrator must update those contracts only after the generic
overlay schema and loader are proven.

## 16.3 Plan 17 compatibility

Briefing/guidance semantic kinds remain intact. Text keys are a presentation
projection. Plan 25 must not alter:

- campaign facts;
- warning thresholds;
- route IDs;
- entity IDs;
- severity/category meaning;
- event ordering.

## 16.4 Audio/caption compatibility

Transcript and caption keys remain aligned with stable audio cue IDs. Localized
text availability must not make voice playback a hidden gameplay condition.

# 17. CI and No-Silent-Debt Gates

Register gates through the canonical CI manifest, not by adding an uncalled
script. The final gate set must prevent:

1. stale generated string catalog;
2. new player-facing literals in changed source;
3. unknown or missing English keys;
4. duplicate/conflicting source keys;
5. placeholder mismatch;
6. orphan or invalid locale-overlay IDs;
7. forbidden mechanical-ID mutation;
8. missing pseudo-locale generation;
9. missing glyph coverage for declared locales;
10. layout overflow in the declared tolerance matrix;
11. exported-resource omission;
12. locale-dependent save/checksum changes.

The current pilot drift gate is retained and extended. A new script is allowed
only when it is the canonical owner for a distinct gate and is registered in
the manifest with focused verification.

# 18. Test Strategy

## 18.1 Core tests

Extend the existing localization-focused tests for:

- known key resolution;
- English fallback;
- visible unknown-key marker;
- invalid locale fallback;
- locale override persistence contract;
- named argument set matching;
- missing/extra placeholder rejection;
- duplicate key rejection;
- deterministic catalog generation;
- invariant serialization;
- locale-independent checksum;
- paired `en`/`pseudo` same-state replay.

Reuse current tests in:

- `Ashfall.Core.Tests/Localization/LocalizationPilotTests.cs`;
- `Ashfall.Core.Tests/Localization/LocalizationServiceTests.cs`;
- `Ashfall.Core.Tests/Localization/MicroLocationLocalizationTests.cs`;
- `Ashfall.Core.Tests/RadioSignalLocalizationTests.cs`.

## 18.2 Host/Godot tests

Add focused host/runtime checks only for changed seams:

- idempotent bridge initialization;
- locale setting and invalid fallback;
- `TranslationServer` application through the adapter;
- key-aware helper output;
- locale-change refresh/disposal;
- missing-key diagnostics;
- project/resource-pack loading;
- pseudo-locale boot and render;
- font fallback glyph probe.

Use a 15 FPS Godot session if a runtime session is required. Use headless
checks for data/resource paths where possible.

## 18.3 Data tests

Add focused overlay tests for:

- overlay resolution;
- base expressive-field fallback;
- orphan rejection;
- expressive whitelist;
- invalid locale;
- placeholder preservation;
- mechanical-ID protection;
- duplicate conflict;
- export/resource-pack inclusion.

## 18.4 Static gates

Run:

    python3 scripts/ci/extract_l10n_inventory.py
    python3 scripts/ci/generate-string-catalog.py --check
    python3 scripts/ci/l10n_drift_gate.py
    python3 scripts/ci/new-localization-literal-gate.py

The exact new-gate filename may be chosen during 25A, but the existing
inventory/drift tooling must remain the integration point rather than become
dead parallel scripts.

## 18.5 Focused verification policy

Each builder runs the smallest affected test file or focused directory,
normally below 100 cases, through `bash scripts/run_test.sh`. Full-suite
verification requires a new bounded hypothesis and foreman/user reason.

Do not claim a compile-green result as proof of runtime integration.

# 19. Dependency-Ordered Implementation Phases

## Phase 0 — Reconfirm evidence and claims

Inputs:

- current ledger and ownership;
- current localization contracts;
- current dirty worktree;
- baseline commands and inventories.

Outputs:

- baseline artifacts;
- exact package claims;
- stale-premise notes;
- no production code changes.

Acceptance:

- no claimed path overlaps an active claim;
- shared Campaign, CI, generated-doc, and settings paths are assigned to the
  integrator where required.

## Phase 1 — 25A contract reconciliation

Paths:

- `Assets/Ashfall.Core/Localization/`;
- `src/Localization/AshfallLocalization.cs`;
- `assets/l10n/strings.csv`;
- `docs/L10N_CONTRACT.md`;
- ADR/catalog documentation.

Work:

- approve namespace and placeholder policy;
- classify existing message keys;
- choose source-table/generated-resource ownership;
- decide whether typed Core key/message value objects are needed.

Acceptance:

- no duplicate resolver or source root is introduced;
- current pilot tests remain green;
- the plan for removing Core/CSV English duplication is explicit.

## Phase 2 — 25A runtime seam hardening

Paths:

- existing Core localization service and focused tests;
- existing host bridge and focused host tests;
- existing settings owner;
- `src/UI/AshfallUiHelpers.cs`.

Work:

- centralized locale/fallback validation;
- named placeholder validation;
- display-formatting split;
- key-aware helper overloads;
- locale-change lifecycle;
- development key highlighting.

Acceptance:

- known/unknown/fallback behavior is visible and deterministic;
- settings persistence remains in the current owner;
- Core build/test remains engine-free.

## Phase 3 — 25A catalog and CI gate

Paths:

- existing inventory/drift scripts;
- new catalog generator if needed;
- `docs/l10n/STRING_CATALOG.md`;
- CI manifest through integrator ownership.

Work:

- deterministic source metadata;
- English key/fallback validation;
- generator `--check`;
- pilot-compatible no-new-debt ratchet.

Gate:

25A complete only after focused tests, catalog check, project/resource check,
and named integrator acceptance.

## Phase 4 — 25B volume-ranked extraction

Order:

1. shared UI shells/helpers;
2. dashboard/navigation/common panels;
3. expedition/inventory/radiation/weather/power surfaces;
4. host status and ActionResult projections;
5. briefing/guidance shared seam;
6. long-tail live panels.

Rules:

- one batch per owned path set;
- equality guard for every migrated sentence;
- no copy edits;
- shelved panels excluded;
- shared Campaign/Main paths integrator-only.

## Phase 5 — 25B regression closure

Work:

- glossary/register notes;
- no-new-literal ratchet;
- snapshot approval;
- expanded/compressed/pseudo layout checks;
- accessibility checks;
- remaining-literal ledger.

Gate:

25B complete only when migrated scopes have no unclassified player-facing
literals and all remaining debt is enumerated with owners.

## Phase 6 — 25C overlay implementation

Paths:

- `assets/l10n/overlays/`;
- generic catalog/text loader;
- catalog integrity validator;
- focused overlay tests;
- representative item/narrative batches.

Work:

- overlay ADR;
- expressive whitelist;
- stable-ID lookup;
- base fallback;
- orphan/ID/placeholder gates;
- resource-pack verification.

Gate:

No mechanical JSON shape changes and no per-subsystem translation service.

## Phase 7 — 25C typography and readiness

Paths:

- theme/project configuration;
- font import/fallback settings;
- glyph/layout/pseudo tooling;
- localization status page;
- caption policy docs.

Work:

- explicit fallback chain;
- declared script probes;
- pseudo generation;
- layout matrix;
- caption/transcript parity;
- store readiness derivation.

## Phase 8 — Integrated closure

Run:

- focused Core and host tests;
- data integrity;
- bridge self-test;
- catalog check;
- literal gate;
- pseudo render;
- glyph/layout checks;
- accessibility and snapshot review;
- paired-locale determinism.

Acceptance:

- final closure report is evidence-backed;
- no unsupported locale is called store-ready;
- shared ledgers are updated only by the foreman/integrator.

# 20. File Impact Map

This map describes planned ownership. It does not claim the paths for the
current documentation task.

## 20.1 Create

- `docs/plans/C2_planintegration[7].md` — this plan, created now;
- `docs/l10n/STRING_CATALOG.md` — generated catalog report;
- `docs/l10n/GLOSSARY.md` — terminology and translation notes;
- `docs/l10n/LOCALIZATION_STATUS.md` — generated readiness matrix;
- `assets/l10n/overlays/<locale>/<catalog>.json` — approved data overlays;
- focused generator/gate files only where existing scripts cannot be extended;
- focused tests for confirmed contracts.

## 20.2 Modify through existing owners

- `Assets/Ashfall.Core/Localization/LocalizationService.cs`;
- `Assets/Ashfall.Core/Localization/WildlifeTrappingLocalization.cs` only if
  generic overlay integration requires it;
- `src/Localization/AshfallLocalization.cs`;
- `src/UI/AshfallUiHelpers.cs`;
- existing settings owner and its tests;
- existing inventory/drift scripts;
- `assets/l10n/strings.csv`;
- generated translation resources through their owning generator/import path;
- selected `src/UI/**`, `src/Host/**`, and `src/Main*.cs` extraction batches;
- catalog integrity validator and focused tests;
- theme/project font configuration after typography evidence.

## 20.3 Integrator-only shared paths

The following require a fresh ownership check and normally integrator
ownership:

- `Assets/Ashfall.Core/ActionResult.cs` if the public contract changes;
- `Assets/Ashfall.Core/Campaign/**`;
- `src/Main.Campaign.cs`;
- `src/Main.CampaignOwners.cs`;
- other shared `src/Main*.cs` partials;
- `docs/ci/CI_GATE_MANIFEST.json`;
- generated documentation indexes;
- shared settings/bootstrap composition roots.

## 20.4 Read-only during this plan-writing task

- `INTEGRATION_PLANS.md`;
- `WORKTREE_OWNERSHIP.md`;
- `TEST_POLICY.md`;
- `KNOWN_DEBT.md`;
- `AI_AGENT_WORKFLOW.md`;
- historical Plan 25 source;
- current C1 economy files/data;
- unrelated dirty worktree changes.

## 20.5 Explicitly excluded

- `Assets/_Game/**`;
- Unity projects and Unity dependencies;
- mechanical JSON rewrites in
  `Assets/StreamingAssets/Data/`;
- arbitrary panel redesign;
- generated outputs edited by hand;
- broad mass-formatting of shared code.

# 21. Risk Register

| Risk | Likelihood | Impact | Mitigation |
|---|---:|---:|---|
| Historical baseline causes duplicate architecture | High | High | Reconcile current localization service/bridge first |
| English CSV and Core catalog diverge | High | High | Parity report, one-source decision, focused migration |
| Key grammar fragments | Medium | High | ADR, aliases, generator validation |
| Literal gate is too noisy | Medium | Medium | Changed-files ratchet and pattern-based categories |
| Extraction changes prose | Medium | High | Equality guard and separate editorial review |
| Placeholder mismatch | Medium | High | Named schema validation per locale |
| ActionResult code is mistranslated | Medium | High | Explicit code/key classification |
| Briefing shared seam is raced | Medium | High | Integrator-only Campaign/Main claim |
| Overlay duplicates mechanics | Low–Medium | Critical | Expressive whitelist and validator |
| Orphan/omitted overlay ships | Medium | High | Integrity and export-resource gates |
| Mechanical ID is translated | Low | Critical | Prefix/grammar protection gate |
| Locale changes checksum | Low | Critical | Paired-locale determinism test |
| Font fallback depends on host machine | Medium | High | Authored theme fallback and glyph probe |
| Pseudo-locale is decorative only | Medium | High | Headless render/layout gate |
| Snapshot churn hides overflow | High | Medium | Deliberate approvals plus matrix checks |
| German QA pack is declared as product support | Medium | High | Generated status page and explicit policy |
| Generated files are hand-edited | Medium | Medium | Generator ownership and `--check` |
| Current dirty worktree is overwritten | Medium | Critical | Exact claims, read-only unrelated paths |

# 22. Out of Scope and Stop Conditions

Stop and report to the foreman/user when:

- a planned path is claimed by another package;
- current evidence shows an API or catalog does not exist;
- a change requires a new architecture decision not covered by the ADR;
- an overlay needs mechanical fields;
- a save owner would have to change;
- a translation requires copy editing rather than structural extraction;
- a locale needs unsupported font/script assets;
- a current contract contradicts this plan and no integrator decision exists;
- a proposed fix would restore Unity-era behavior.

Do not work around a stop condition by adding a local cache, duplicate
resolver, parallel save store, blanket literal exemption, or deprecated API.

# 23. Rollback and Compatibility

## 23.1 Incremental commits

Use small, independently reviewable commits:

1. baseline and ADR;
2. Core/host seam hardening;
3. settings/project/bootstrap;
4. helper/formatting support;
5. catalog generator and gate;
6. highest-volume UI extraction;
7. common UI extraction;
8. host status and ActionResult projection;
9. briefing key/argument projection;
10. glossary and no-new-literal ratchet;
11. long-tail extraction;
12. snapshots/accessibility/layout;
13. overlay ADR/loader;
14. overlay integrity/ID gate;
15. representative catalog migration;
16. font fallback/glyph audit;
17. pseudo-locale/layout matrix;
18. audio/caption/status page;
19. determinism closure.

## 23.2 Safe fallback

During migration:

- old base expressive text remains available;
- compatibility aliases may point old keys to new keys;
- raw helper overloads are reported, not silently deleted;
- old settings values normalize to English when invalid;
- overlay absence falls back to base expressive English;
- generated resources are regenerated from source;
- no save migration is required for locale.

Rollback means reverting the smallest accepted commit and regenerating owned
outputs. Do not reset or discard unrelated worktree changes.

## 23.3 Feature exposure

The integrator may keep extraction/overlay families behind an existing
developer or pilot allowlist until their focused gates pass. The allowlist
must not become a permanent silent exemption; its removal is a named
acceptance criterion.

# 24. Definition of Done and Closure Evidence

## 24.1 25A — Seam

- [ ] current key convention ADR is approved;
- [ ] Core remains engine-free;
- [ ] typed key/message contract decision is documented;
- [ ] one host resolver is authoritative;
- [ ] English master is singular;
- [ ] locale settings use the existing settings owner;
- [ ] project internationalization resources are aligned;
- [ ] key-aware UI helpers exist;
- [ ] named parameters and placeholder checks exist;
- [ ] development missing-key/highlight behavior is visible;
- [ ] deterministic catalog generation and `--check` exist;
- [ ] all English master keys resolve;
- [ ] focused Core/host tests pass.

## 24.2 25B — Extraction

- [ ] candidate literals are classified;
- [ ] extraction is volume-ranked;
- [ ] UI labels and sentences are keyed;
- [ ] host status paths resolve keys;
- [ ] briefing carries semantics plus key/arguments;
- [ ] English prose equality passes;
- [ ] near-duplicate decisions are recorded;
- [ ] no-new-literal gate is active;
- [ ] glossary/register notes exist;
- [ ] snapshots are deliberately reviewed;
- [ ] +30% and −25% layout checks are recorded;
- [ ] accessibility behavior remains intact;
- [ ] remaining user-facing literals are enumerated;
- [ ] no unclassified player-facing literal remains in migrated scope.

## 24.3 25C — Data and typography

- [ ] overlay ADR is approved;
- [ ] base JSON mechanics remain authoritative;
- [ ] expressive fields resolve through the generic overlay loader;
- [ ] base fallback works;
- [ ] orphan and invalid overlays fail integrity;
- [ ] expressive whitelist is enforced;
- [ ] mechanical IDs are protected;
- [ ] register/voice metadata is retained;
- [ ] explicit font fallback is configured;
- [ ] declared glyph probes pass;
- [ ] pseudo-locale generation and render pass;
- [ ] layout matrix is recorded;
- [ ] caption/transcript policy is documented;
- [ ] culture-aware display formatting is isolated;
- [ ] localization status is generated;
- [ ] store-ready status is derived from passing gates.

## 24.4 Cross-system closure

- [ ] locale does not alter save checksum;
- [ ] same seed/actions produce identical state in `en` and `pseudo`;
- [ ] Plan 17 briefing/guidance semantics remain intact;
- [ ] Plan 16A shelved panels were not unnecessarily migrated;
- [ ] no new localization debt was introduced;
- [ ] export/resource-pack checks pass;
- [ ] shared ledgers were updated only by the foreman/integrator;
- [ ] closure report includes commands, results, limitations, and remaining
      debt.

## 24.5 Closure report template

    ## C2[7] Closure Report

    ### Repository
    - Start commit:
    - End commit:
    - Branch:
    - Working tree:

    ### Baseline
    - UI literals:
    - Host literals:
    - Main/briefing literals:
    - Existing message keys:
    - Data definitions with expressive text:
    - Fonts/fallbacks:
    - Registered localization gates:

    ### 25A — Seam
    - Key convention:
    - Core contract:
    - AshfallLocalization:
    - Settings/bootstrap:
    - Project resources:
    - UI helper adoption:
    - Formatting:
    - Dev diagnostics:
    - Catalog generator:
    - Result:

    ### 25B — Extraction
    - UI labels migrated:
    - UI sentences migrated:
    - Host status migrated:
    - Briefing migrated:
    - Equality result:
    - New-literal gate:
    - Glossary:
    - Snapshot/layout/accessibility:
    - User-facing literals remaining:
    - Result:

    ### 25C — Data and typography
    - Overlay representation:
    - Loader:
    - Overlay coverage:
    - Orphan/ID checks:
    - Register metadata:
    - Font fallback:
    - Glyph/layout:
    - Pseudo-locale:
    - Caption policy:
    - Store status:
    - Result:

    ### Determinism
    - English checksum:
    - Pseudo checksum:
    - Same-state result:

    ### Verification
    - Core build:
    - Focused Core tests:
    - Host build:
    - Data integrity:
    - Bridge:
    - Catalog check:
    - New-string gate:
    - Pseudo render:
    - Glyph/layout:
    - Accessibility:
    - Snapshot:

    ### Remaining debt
    - UI:
    - Host:
    - Briefing/shared seams:
    - Narrative overlays:
    - Fonts:
    - VO/captions:
    - Real translation:

# 25. Implementation Handoff

## MUST PRESERVE

- Godot is authoritative; Unity is retired.
- Core remains engine-free.
- JSON mechanics and stable IDs remain authoritative.
- Existing `LocalizationService`/`AshfallLocalization` pilot contracts and
  focused tests remain green.
- Existing settings ownership and `user://settings.json` behavior.
- Save/checksum/RNG determinism and invariant serialization.
- `ActionResult` status semantics and internal-code behavior.
- Briefing/guidance semantic kinds, entity IDs, routes, and severity.
- ASHFALL prose, faction voice, register, punctuation, and tone during
  extraction.
- Keyboard/controller focus, close/back behavior, accessibility names, and
  panel disposal lifecycle.
- Current C1 economy authority and active shared-path claims.
- Current user/agent dirty worktree changes outside the assigned package.

## MUST ADD

- A current key/argument ADR that extends the existing pilot.
- One authoritative host resolution path.
- One English source table and a deterministic catalog check.
- Explicit locale fallback and invalid-locale policy.
- Key-aware shared UI helper overloads.
- Named-placeholder validation and display-formatting separation.
- Volume-ranked classified literal inventory.
- Changed-files no-new-player-facing-literal ratchet.
- Briefing/host key+argument projections at integrator-owned seams.
- Generic stable-ID expressive data overlays with a whitelist.
- Overlay orphan/ID/placeholder/resource-pack gates.
- Explicit font fallback and declared glyph probes.
- Pseudo-locale generation and layout-tolerance verification.
- Caption/transcript parity policy.
- Paired-locale determinism verification.
- Generated localization readiness status.

## MUST NOT DO

- Do not create a second `AshfallText`/`LocaleBootstrap` resolver stack without
  an approved evidence-based architecture decision.
- Do not leave English in Core fallback code after the source-table migration
  without a documented compatibility reason.
- Do not translate internal IDs, result codes used only by logic, audio cue
  IDs, or mechanical field values.
- Do not rewrite prose while extracting it.
- Do not concatenate localized fragments or use positional-only placeholders
  for new dynamic strings.
- Do not place Godot APIs in Core.
- Do not put simulation decisions in panels or localization helpers.
- Do not add a localization save file or locale-dependent checksum data.
- Do not restructure the gameplay JSON authority for translation.
- Do not create per-subsystem translation services.
- Do not bless snapshots with clipping or shorten pseudo text to hide overflow.
- Do not declare German, Latvian, Cyrillic, CJK, or any other locale
  store-supported without passing its status matrix.
- Do not edit active Campaign/DailyBriefing or CI shared paths without an
  assigned integrator claim.
- Do not modify Unity-era structures.

## VERIFY WITH

Current focused evidence:

    python3 scripts/ci/l10n_drift_gate.py
    bash scripts/run_test.sh Ashfall.Core.Tests/Localization/

25A and later focused checks:

    python3 scripts/ci/extract_l10n_inventory.py
    python3 scripts/ci/generate-string-catalog.py --check
    python3 scripts/ci/new-localization-literal-gate.py
    bash scripts/run_test.sh Ashfall.Core.Tests/Localization/
    dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
    dotnet build Ashfall.csproj
    godot --headless --path . -- --data-integrity-selftest
    godot --headless --path . -- --bridge-selftest
    bash scripts/ci/verify-fast.sh

Use repository-canonical commands for:

- pseudo-locale render;
- glyph coverage;
- `ashfall-ui-access`;
- `ashfall-snapshot-diff`;
- export/resource-pack verification.

Run only the smallest relevant target per package. Report skipped checks and
why. A passing build alone is not an integration result.

## FIRST SAFE IMPLEMENTATION STEP

After this plan is accepted and a foreman assigns a non-overlapping package:

1. re-read the current ledgers and active claims;
2. rerun the bounded localization drift gate, focused localization tests, and
   current inventory;
3. generate the Plan 25 baseline/classification report without modifying
   production source;
4. reconcile the report with `docs/L10N_CONTRACT.md` and write the key/source
   authority ADR;
5. stop before touching Campaign/DailyBriefing, shared Main partials, CI
   manifests, or another claimed path unless the integrator has assigned that
   seam.

The first code-level change must be the smallest 25A contract hardening that
removes ambiguity from the existing pilot. It must not begin with a
thousands-string extraction or a gameplay-data rewrite.
