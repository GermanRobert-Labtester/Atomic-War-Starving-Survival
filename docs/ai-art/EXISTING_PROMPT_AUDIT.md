# EXISTING_PROMPT_AUDIT.md

Two prompt-adjacent files already exist. Audited against: GOOD / COMPRESS / CONTRADICTORY / GENERATOR-MISMATCHED / TOO GENERIC / ART-DIRECTION DRIFT / REDUNDANT.

## `prompts_for_ai.md` (63 lines, hand-written)

**Verdict: salvage the style guide, discard the model routing.**

- **GOOD** — the "General Art Style Guidelines" section (palette, lighting, texture, perspective) is accurate and matches the GDD independently. Promoted into `GAME_VISUAL_DNA.md`.
- **GENERATOR-MISMATCHED / stale** — none of the three model headers match a generator that exists today:
  - "Adobe Firefly Prompts (Flux 2 Pro)" conflates two unrelated models under one header.
  - "ChatGPT Image 2 (DALL-E 3)" conflates GPT Image 2 with the older, unrelated DALL-E 3.
  - "Nano Banana Oro" is explicitly flagged in the file itself as hypothetical — no such model exists or ever did. (Real models: Nano Banana Pro = Gemini 3 Pro Image; Nano Banana 2 = Google's Feb-2026 general model. See `IMAGE_MODEL_PROFILES.md`.)
  - → Superseded by `IMAGE_MODEL_PROFILES.md`. Don't route new work through this file's model sections.

## `game_assets_prompts.md` (284 lines, machine-templated from code symbol names)

**Verdict: do not reuse as-is. Regenerate from the taxonomy, category by category, as each scales up.**

- **COMPRESS** — nearly every entry (~230+) wraps the same ~60-word boilerplate around a swapped-in name: *"A [mood] [category] asset of **[Name]**... The object looks heavily used, with scratches, rust, and dirt... inspired by This War of Mine and Darkest Dungeon... On a transparent background."* This is the exact prompt-bloat anti-pattern this system exists to avoid — the same idea restated per entry, not new visual information per word.
- **TOO GENERIC / mis-scoped category** — a large fraction of the "UI & Icons" and "Props & Items" entries are not renderable single assets at all; they're C# enum/class names pulled straight from the codebase with no human curation: `Affliction Phase`, `Death Screen Kind`, `Expedition Phase`, `Game Phase`, `World Phase`, `Room Unlock State`, `Blood Type`, `Weather Kind`, `Workbench Action Kind`, `Pet Traits`, `Trait Agoraphobic`. These are data types (some with 5–8 enum values each), not pictures — a prompt for "Blood Type" or "Game Phase" has no single correct image.
- **CONTRADICTORY** — every "Environments & Locations" entry is framed as *both* "2.5D isometric perspective, emphasizing verticality" *and* "A 2D prop asset... On a transparent background" in the same entry. An isometric environment backdrop and a transparent-background prop are structurally different asset types; the template merged both boilerplate blocks. The isometric claim also contradicts the one real reference image — see `GAME_VISUAL_DNA.md`.
- **Salvageable pattern** — roughly 1 in 10 entries carries a hand-written trailing sentence with real visual information (Ammo Box Rifle: "stock wrapped in dirty rags, scope is cracked"; Hazmat Suit: "visor cracked, sealed with yellowing tape"; Bottled Water Irradiated: "faint eerie green light... cracked plastic bottle"). This is the right *level* of asset-specific delta — kept as the model for `PROMPT_RULES.md`'s "GLOBAL STYLE + delta" pattern; the boilerplate carrier around it is what gets dropped.
- **Incomplete** — 284 lines covers a fraction of the current 419-item catalog; the file predates several catalog-expansion commits ("The Ash Gets Deeper," "Into the Ash"). Not a coverage source of truth — `ASSET_MANIFEST.md` is.
- **REDUNDANT with itself** — no structural distinction between an inventory icon, a full-body character sprite, and a VFX sprite sheet beyond one trailing sentence fragment; three different asset types get near-identical prompt shapes.

## `INTEGRATION_PLAN_FOR_CURSOR.md` §III "Canva Business — Asset Generation List" (not a prompt file, but the best-scoped existing artifact)

**GOOD** — 12 assets, each with an exact size, format, and consuming widget, tied to the actively uncommitted Phase 11 HUD work. This is asset *specification*, not prompts. No compression needed — it just needed prompts written against it, which is what `prompts/pilot_batch.md` does for 10 of the 12 (the remaining 2 are same-recipe color/symbol swaps of drafted entries, noted in `ASSET_MANIFEST.md`).
