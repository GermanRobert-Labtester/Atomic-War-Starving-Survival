# PROMPT_RULES.md

> **Supersession note (2026-08-12):** two pre-existing, already-in-production Desktop prompt libraries were found after this file was first written — `ASHFALL_Firefly_Item_Icon_Prompts.md` (321 items) and `ASHFALL_Firefly_Flux_200_Asset_Prompts.pdf` (200 key art/env/character/VFX entries), both built against Flux 2 Pro via Adobe Firefly. Where their established GLOBAL SUFFIX / visual bible differs from the `GLOBAL_STYLE` below, **treat the Desktop files as canonical** — they represent ~300+ already-produced real assets, and new work should stay consistent with what's already made rather than drift toward this file's independently-derived version. Concretely: items use a photorealistic product-shot register (pure black background, rim-lit), not the graphic-novel-illustration register below; everything else (environments/survivors/factions/weather) uses a hand-painted gouache-and-charcoal register, closer to but not identical to the illustration language below. See `docs/ai-art/prompts/FULL_CATALOG_EXPANSION.md` for the two real GLOBAL STYLE blocks quoted verbatim, and use those for any new item/location/survivor/faction/weather prompt.

## GLOBAL_STYLE

*(This project's own independently-derived version — still the right reference for asset families the Desktop libraries don't cover, e.g. props, UI icons, VFX. See the supersession note above before applying it to items, locations, survivors, factions, or weather.)*

Every prompt = `GLOBAL_STYLE + asset-specific delta`. Never paste the full art bible into one prompt — compress to whichever form fits the model's length band (see `IMAGE_MODEL_PROFILES.md`).

**Full form (~40 words):**
> 2D graphic-novel illustration: inked linework, cross-hatch shading, painted color. Chiaroscuro lighting — single warm practical light source against cool ambient light. Desaturated palette: ash blues, charcoal grey, rust orange, mud brown. Worn, scavenged, hand-repaired. Cold, exhausted, restrained mood.

**Compressed form (~15 words, for tight budgets):**
> Gritty 2D graphic-novel illustration, inked linework, chiaroscuro lighting, desaturated ash-blue/rust/mud palette, worn and scavenged.

**UI semantic-icon variant** (icons/badges use flat color, not the illustrative palette):
> Flat minimalist vector icon, single stroke weight, no gradient, semantic color per `design-tokens.json`.

## Asset ID convention

The project already enforces snake_case ids everywhere (`README.md`: "Ids: snake_case everywhere"). Follow it:
- If an id already exists in a JSON catalog (`items.json`, `survivors.json`, `locations.json`) or an existing plan (`INTEGRATION_PLAN_FOR_CURSOR.md` §III), **reuse it exactly** — don't invent a parallel naming scheme for the same thing.
- New ids (no existing catalog entry) follow `<family>_<descriptive_name>`, e.g. `env_shelter_medical_bay`.

## Per-model compilation quick-reference

| Model | Structure | Length |
|---|---|---|
| FLUX.2 [pro] / [max] | Subject → Action/state → Style → Context, most important first | 10–30 (exploration) / 30–80 (production) words |
| Nano Banana Pro / 2 | Structured brief: purpose, subject, setting, composition, lighting, style, constraints, format | as short as covers the brief |
| GPT Image 2 | 1–3 natural-language sentences, can carry semantic intent | 40–120 words |
| Recraft | Plain graphic-design terms: shape, stroke weight, fill, color, background — no lighting/material language | 15–45 words |
| Adobe Firefly | subject + descriptors + environment + art treatment | 20–70 words |
| Seedream 5.0 Lite | Semantic description of intent and relationships, not a tag list | as short as covers the brief |
| Kling | Subject + Action + Context (3–5 elements) + Style | short, concrete |

Full detail and verification status per model: `IMAGE_MODEL_PROFILES.md`.

## Negative-constraint translation (game-specific)

Translate every exclusion into what to draw instead — most of these models don't support negative prompts, and the ones that do still do better with positive language.

| Unwanted | Say instead |
|---|---|
| Fantasy/magic elements | Realistic post-nuclear physics and materials only |
| Sci-fi mutants, lasers, aliens | Ordinary human survivors, conventional damaged technology |
| Real-world flags/logos/countries | Generic, unbranded fictional-faction insignia |
| Heroic/glamorous poses | Exhausted, defensive, functional posture |
| Bright saturated environmental colors | Desaturated ash-blue/charcoal/rust/mud palette |
| Clean/new-looking objects | Worn, scratched, corroded, patched |
| Isometric/top-down environment framing | Eye-level to slightly elevated single-scene framing |
| Baked-in UI text/logotype | Empty text-free surface — UI Toolkit renders text at runtime |
| Gore/shock violence | Implied consequence, not graphic detail |

## Asset variations

State what changes and what's invariant — don't regenerate the whole brief for a palette/symbol swap. Example from this batch: `vignette_sepia` → `vignette_blue` changes only the edge color (`#b98a5e` → `#6ea3a8`); radius, falloff, transparent center, and format stay identical. See `prompts/pilot_batch.md` for the worked example.

## Compression / QC checklist

Run every prompt against the failure modes actually found in this repo's existing files (`EXISTING_PROMPT_AUDIT.md`) before accepting it:
- [ ] Would this read as boilerplate if you swapped the subject name and changed nothing else? (The #1 failure in `game_assets_prompts.md`.)
- [ ] Is the subject an actual renderable picture, not a code enum/data type? (`Blood Type`, `Game Phase` are not images.)
- [ ] Does the framing match `GAME_VISUAL_DNA.md`'s camera language (no invented isometric claim)?
- [ ] Is every phrase doing visual work — could 20% of the words be cut without losing control?
- [ ] Is this the model this asset type actually routes to per `ASSET_TAXONOMY.md`, not just the default?
- [ ] Are recurring subjects (survivors, the shelter, icon family) checked against `CONSISTENCY_ANCHORS.md` before generating a new one?
- [ ] Does it respect the human-authorship framing — is this meant as a paint-over base, not a shipped final?
