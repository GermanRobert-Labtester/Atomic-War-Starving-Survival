# Plan 25 — One Language of Strings: Make Every Word Reachable

> **Wave:** Continuity Wave 3 — *Ship It Intact* (Plans 25–29)
> **Predecessors:** [Wave 1](Wave1_Continuity_Audit_INDEX.md) (narrative continuity),
> [Wave 2](Wave2_Continuity_Audit_INDEX.md) (physical continuity).
> **Depends on:** nothing to start; 16A's panel verdicts make the extraction surface smaller.
>
> **Theme:** Wave 1 and 2 make the game one connected experience. Wave 3 makes it a **product** —
> and the first product fact is that ASHFALL cannot be localized at all. Not "poorly": there is no
> translation layer in the codebase, no locale configuration, and no path from the 372+ UI string
> literals or the 4,808 authored definitions' display text to anything translatable. For a game
> whose identity is diegetic documents, radio transcripts, and restrained prose, that is not a
> polish item — it is a wall between the writing and the player.

---

## Evidence Inventory (re-verified @ `ccac926e`)

| Fact | Evidence |
|---|---|
| **No translation API usage anywhere in the host** | `grep -rn "TranslationServer\|tr(\"\|Tr(\"" src/` → **0 hits**; `project.godot` has no `internationalization`/`locale`/`translations` entries and no `*.csv`/`*.po`/`*.translations` files exist |
| UI strings are inline literals | `grep -rn 'Text = "' src/UI/*.cs` → **372** assignment sites (564 multi-word literal starts) across **164** files in `src/UI/` |
| Panel code mixes prose with logic | e.g. `AnaerobicBiogasDigesterPanel.cs:88–100` embeds full sentences in `ShowFeedback("Fed 50L organic biomass slurry…")`; `HoldfastRuntimeSession.cs:246` returns `"Starting supplies loaded into Holdfast storage."` |
| Host status/briefing text is formatted inline | `src/Main.Campaign.cs` builds `DailyBriefingEntry(…, $"{name} is hungry ({s.Hunger:F0}% hunger).")` — interpolation fused with wording, so no translator can reposition the number |
| The data authority mixes display text with mechanical keys | `items.json` entries carry `displayName` + a prose `description` alongside `hungerRestore`/`radProtection`; property naming already mixes `camelCase` and `snake_case` (recorded in `AGENTS.md` as the A11 migration debt) |
| Diegetic prose is the product | 272 `narrative/` catalogs + 118 radio broadcasts + 79 world-history articles + echoes/memorials/epilogue text — the writing is the value, and it is currently single-language by construction |
| Voice/radio already separates into cues | `docs/ui/FACTION_VOICE_MATRIX.md` and the 70-cue audio catalog give a per-faction voice register — a localization target for *tone*, not just words |
| Typography is Latin-only by default | `assets/fonts/` = Barlow Condensed (5 faces) + ShareTech Mono — no fallback chain configured; Cyrillic/CJK/accented coverage is untested, and the cast is transliterated-slavic (`survivor_gunner_mikhail`) |
| The gates that would catch this are absent | `docs/ci/CI_GATE_MANIFEST.json` has **46 gates** — none is localization-, string-, or font-related |
| Tooling exists as intent, not implementation | `ashfall-localize` and `ashfall-string-extractor` skills describe the layer; nothing in `src/` or `Assets/` implements it |

**Reading:** every later localization attempt gets harder the more prose is written into code.
Extraction is cheap now and expensive after Wave 1+2's new briefing/guidance/echo text lands — so
this plan should run **alongside** the content waves, not after them.

---

## Task 25A — Build the seam: a translation layer the host can adopt incrementally

**Goal:** one host-side string table + lookup, with a fallback that makes untranslated text
visible rather than broken, and an adoption path that does not require rewriting 164 panels at
once.

**Files:** new `src/L10n/AshfallText.cs`, new `src/L10n/LocaleBootstrap.cs`,
`Assets/Ashfall.Core/L10n/TextKey.cs` (Core, engine-free), `project.godot`
(`[internationalization]`), `src/UI/AshfallUiHelpers.cs`, `assets/localization/` (new),
`docs/ui/DESIGN_SYSTEM_RULES.md`, `scripts/ci/generate-string-catalog.py` (new).

### Substeps

1. **Decide the key convention first** and write it down: `domain.surface.element` in snake_case
   (e.g. `briefing.survivor.hungry`), mirroring the project's existing id discipline so
   `CatalogIntegrityValidator`-style checks can reuse the prefix machinery.
2. **Add a Core-side registry of keys** (`Assets/Ashfall.Core/L10n/`) as plain strings + metadata
   so Core systems (briefing entries, event kinds, message keys like the roster's
   `duty_roster.unknown_role`) can reference text without touching Godot. Core stays engine-free —
   it never resolves a translation, it only carries keys.
3. **Implement `AshfallText.T(key, args…)`** in the host: `TranslationServer` first, then
   English fallback from the generated master table, then a **loud** dev-mode marker (`[KEY]`) when
   a key is unknown — a silent fallback is how untranslated text becomes invisible.
4. **Reuse the existing message-key convention** already present in Core result objects
   (`ActionResult` keys like `"duty_roster.cannot_assign"`, `"unknown_item"`): treat them as
   translation keys where they are user-facing, and separate them from internal codes where they
   aren't. Do not invent a second vocabulary for the same thing.
5. **Convert the shared UI factory first** — `AshfallUiHelpers` label/button/row helpers gain a
   key-aware overload, so the highest-volume string sites route through the layer in one change.
6. **Parameterise, never concatenate**: `{name}`, `{value:F0}`, `{unit}` placeholders and a
   plural/ordinal rule — the current `$"{name} is hungry ({s.Hunger:F0}% hunger)."` cannot be
   translated into a language that puts the number first. This is the substantive rewrite, and it
   is why doing it early is cheap.
7. **Wire locale into settings**: existing settings surface + `project.godot`
   `locale/translations` list; auto-detect, with an explicit override, persisted across sessions in
   the same place audio settings already persist.
8. **Add a dev-only "string highlight" toggle** that overlays keys on-screen (cheap, hugely
   useful for the extraction sweep, and doubles as an accessibility audit aid).
9. **Generate the master string catalog** (`scripts/ci/generate-string-catalog.py` →
   `docs/l10n/STRING_CATALOG.md` + a CSV/POT handoff file) in the same "generated, never
   hand-edited" style as `docs/cli/HOST_CLI_COMMAND_CATALOG.md`.
10. **Register the generator as a gate** in `docs/ci/CI_GATE_MANIFEST.json` with `--check`, so the
    catalog can't drift from source.
11. **Tests**: unknown-key dev behaviour, parameter formatting (invariant culture — determinism
    rule), settings round-trip, and a smoke test that every key in the English table resolves.
12. **Run the five-step verification checklist** + `bash scripts/ci/verify-fast.sh`.

**DoD:** one lookup path, no silent fallbacks, catalog generated and gated, new strings cost one
line.

---

## Task 25B — Extract without breaking prose: UI, host, and briefing

**Goal:** move user-facing text out of code with the writing intact — tone is a
requirement, not a refactor artifact.

**Files:** the 164 files in `src/UI/`, `src/Host/*.cs` status strings, `src/Main*.cs` briefing and
status text, `src/Host/HoldfastTerminalPanel.cs`, `docs/ui/UI_VISUAL_TEXT_SPEC.md`,
`assets/localization/en/*.json` (new), plus `ashfall-write` tone rules as the acceptance standard.

### Substeps

1. **Sweep by volume, not by folder**: start with the 10 highest string-count files
   (`wc`+`grep` report generated in step 25A.9) so effort tracks payoff.
2. **Classify each string** as `UI_LABEL`, `UI_SENTENCE`, `STATUS_LINE`, `DIEGETIC`, `DEV_ONLY`,
   or `INTERNAL_CODE` (never translated — message keys used in logic). Record counts;
   `DEV_ONLY` stays literal so dev output isn't mangled.
3. **Extract UI labels** (`"[FEED ORGANIC WASTE SLURRY]"`, nav button captions like `OVERVIEW`)
   into keys with the label text as the English value, preserving the established uppercase/condensed
   voice.
4. **Extract sentences with parameters** and assert the parameter set matches the placeholder set —
   the single most common localization break.
5. **Never let extraction rewrite the prose**: capture before/after text equality in a test
   (key → value equals the original literal, modulo placeholders). Writers reviewing the diff must
   see only structure changes.
6. **Move host status lines** (`"Starting supplies loaded into Holdfast storage."`,
   `"Day 2 briefing…"`) out of `src/Host/*` into keys — host sessions return results/keys, not
   English.
7. **Split the briefing builder** so wording comes from the table and `DailyBriefingEntry` carries
   `key + args`; keep severity/number semantics untouched (this is also Wave 1's 17A data path).
8. **Deduplicate near-identical strings** found by the sweep ("none held", "no stock", "shelves
   are bare") — collapse to one key with parameters where the meaning is the same, and say so in
   the PR so no writer thinks a voice nuance was deleted by accident.
9. **Gate new hardcoded strings**: add a CI check that a *changed* `src/UI/**` or `src/Host/**`
   diff may not introduce a new user-facing literal without a key, mirroring the existing
   `forbidden-api-gate.sh` / `catch-policy-gate.sh` idiom. Start with warnings-as-errors on the
   changed-files diff only, so the gate can't be defeated by a large exempted commit.
10. **Terminology lock**: build a glossary (`docs/l10n/GLOSSARY.md`) from the domain list in
    `AGENTS.md` (chelation, brine water, dose ledger, sick list, holdfast, muster…) so repeated
    system nouns stay identical across panels, JSON, and radio.
11. **Snapshot review**: 29 golden UI targets will shift if text length changes; regenerate
    deliberately per `docs/ui/SNAPSHOT_FIXTURE_POLICY.md` with an approval note, and check overflow
    for a +30% string length case (the standard "German test").
12. **Accessibility pass**: no text conveyed by colour alone; every extracted label reachable by
    keyboard focus order; run `ashfall-ui-access` on touched panels.
13. **Tests + run the checklist** (string catalog up to date, 0 unkeyed new strings, formatting
    culture-invariant).

**DoD:** no user-facing English lives in `src/`, and the prose reads exactly as before.

---

## Task 25C — Data-authority text and typography: the writing is the product

**Goal:** make the 4,808 authored definitions' display text translatable, and make sure it can be
*rendered* — including scripts the current two fonts don't cover.

**Files:** `Assets/StreamingAssets/Data/**` (items, locations, narrative/, radio, echoes,
memorials, epilogue), loaders in `Assets/Ashfall.Core/*CatalogLoader.cs`,
`Assets/Ashfall.Core/CatalogIntegrityValidator.cs`, `assets/fonts/*.import`, theme font chain
(`docs/ui/DESIGN_SYSTEM_RULES.md`), `docs/data/CATALOG_REGISTRY.md`.

### Substeps

1. **Choose the data representation** and justify it in a short ADR: (a) in-file `_l10n` sibling
   keys, (b) per-locale JSON overlays keyed by definition id, or (c) key-ized text — pick (b) as
   the default because it keeps the mod-safe authority intact (`ashfall-mod-contract`: JSON is
   mod-safe; don't rewrite 411 files' shape) and lets translators ship deltas.
2. **Split mechanical from expressive** in the loader contract: `hungerRestore`, `radProtection`,
   thresholds, ids stay authoritative in the base file; `displayName`, `description`, prose fields
   resolve through an overlay lookup with base-file fallback.
3. **Finish the naming migration while touching the same files**: `AGENTS.md` records mixed
   `camelCase`/`snake_case` with per-file migration notes filed (A11). Convert *text* fields to
   `snake_case` in the same pass so a second sweep isn't needed, and keep
   `schema_version` + migration notes honest per `ashfall-data-schema`.
4. **Gate text coverage**: extend `CatalogIntegrityValidator` so a definition with prose fields in
   an overlay references an id that exists in the base authority (prevents orphan translations —
   the same class as the TIER-1/TIER-2 reference checks it already does).
5. **Diegetic register per locale**: radio transcripts, memorials, and item descriptions must keep
   the faction voice distinction (`FACTION_VOICE_MATRIX.md`) — record the register (terse/clipped,
   bureaucratic, devotional) as metadata so translators preserve it rather than flattening it.
6. **Typography audit**: verify glyph coverage of Barlow Condensed + ShareTech Mono for the target
   scripts; configure a **font fallback chain** in the theme for Cyrillic and any CJK target, and
   confirm hinting/antialiasing import settings per locale (a missing-glyph box is a crash for
   immersion, not a cosmetic bug).
7. **Measure layout tolerance**: run the panel suite at +30% and −25% string length; record
   truncation/overflow offenders (this is also the accessibility and 1080p-vs-windowed check).
8. **Audio/caption parity**: the 70-cue catalog includes VO lines; any locale that gets VO needs
   captions or a text substitute, and radio already has a transcript surface — decide and document
   the rule, don't let it drift silently.
9. **Numbers, dates, culture**: reuse the existing invariant-culture formatting discipline
   (`SaveChecksum` is culture-invariant by design) for *storage*, and format for *display* through
   one helper so a locale change can't corrupt a save or a checksum.
10. **Never translate mechanical identifiers**: add the forbidden-API-style gate for `item_`,
    `loc_`, `quest_`, `flag_`, `radio_`, `echo_` prefixes appearing inside any locale overlay —
    `CatalogIntegrityValidator` already knows those prefixes.
11. **Pseudo-locale test build**: a CI-run pseudolocalizer (accented + expanded strings) to catch
    unkeyed strings, clipping, and font gaps in one shot — cheaper than any real translation pass and
    it finds the same bugs.
12. **Store readiness**: `docs/` gains a localization status page enumerating supported locales,
    string counts, untranslated remainder — the number a store page actually requires.
13. **Tests** (key resolution, overlay fallback, orphan translation rejection, glyph coverage
    probe) + full checklist + `--data-integrity-selftest`.

**DoD:** prose is translatable without touching the authority's shape, and every target script
renders in the project's own type.

---

## Cross-Task Dependencies

```
25A (seam + convention + gate) ──► 25B (UI/host extraction) ──► 25C (data text + typography)
        │                                  │
        ├──► 16A (shelved consoles shrink the sweep)
        └──► 17A/17B supply new briefing & guidance strings — extract them as keys on arrival,
             or Wave 1 recreates the debt this plan removes
```

**Execution order:** 25A → 25B → 25C. Run 25A **before or with** Wave 1's 17A/17B: briefing lines
and guidance text are the newest strings in the project, and they should be born keyed.

---

## Verification Checklist (per task)

```
1. dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
2. dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
3. dotnet build Ashfall.csproj                                   # 0 errors, 0 warnings
4. godot --headless --path . -- --data-integrity-selftest        # 0 errors
5. godot --headless --path . -- --bridge-selftest                # exits 0
6. python3 scripts/ci/generate-string-catalog.py --check         # catalog in sync
7. new-string gate (25B step 9)                                  # no unkeyed literals added
8. pseudo-locale render check + ashfall-ui-access + ashfall-snapshot-diff
9. bash scripts/ci/verify-fast.sh
```

---

## Estimated Effort & Risk

| Task | New | Files touched | Strings | Tests | Difficulty | Regression risk |
|---|---|---|---|---|---|---|
| 25A | 4 files + 1 gate | 2 (`AshfallUiHelpers`, `project.godot`) | — | 5–8 | Medium | LOW |
| 25B | 0 | ~60 of 164 UI + ~15 host | ~370 | 6–10 | Medium (volume) | MEDIUM (snapshot/overflow) |
| 25C | 1 ADR + overlays | loaders, validators, theme | 4,808 defs | 8–12 | Medium–High | MEDIUM (data shape) |

**Guardrails:** do not restructure the JSON authority to be translatable (overlays, not rewrites);
do not translate internal message keys used by logic; do not let extraction alter prose; do not add
a locale to the store listing until pseudo-locale passes.
