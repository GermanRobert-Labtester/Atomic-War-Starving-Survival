# IMAGE_MODEL_PROFILES.md

> Checked against each vendor's current docs/announcements via web search on 2026-08-12 (sources at the bottom) — this project's own knowledge cutoff lags the game's in-repo dates enough that model names/tiers needed a live check rather than a guess, per this system's own rule against inventing parameters. Entries marked "general knowledge" weren't individually re-verified this pass; confirm before a large batch if they seem to have drifted.

## FLUX.2 [pro] — verified
- **Best for:** photoreal/painterly textures, materials, environments — this project's default for gritty painterly assets.
- **Structure:** `Subject → Action/state → Style → Context`, most important words first (FLUX.2 weights earlier tokens more heavily).
- **Length:** 10–30 words for quick exploration, 30–80 for most production prompts.
- **Negative prompts:** not supported — translate every exclusion into a positive description (see `PROMPT_RULES.md`).
- **Reference images:** supported; state what each one contributes.

## FLUX.2 [max] — verified (same official doc family as Pro)
- **Best for:** this project's hardest single shots — multi-object interiors, anything that must closely match `UI_StyleReference_01.jpg`, reference-heavy generation.
- Same structure/length rules as Pro; step up to Max only when Pro's control isn't precise enough.

## Nano Banana Pro (= Gemini 3 Pro Image, API `gemini-3-pro-image`) — verified
- **Best for:** professional/consistency-critical assets — recurring survivor portraits, anything leaning on reference images to hold a likeness across generations.
- **Structure:** a structured creative brief — purpose, subject, setting, composition, lighting, style, exact text (if any), constraints, output format. A focused short prompt beats a long one with competing styles/viewpoints.
- **Thinking mode:** adds latency but earns it for multi-element/precision compositions.

## Nano Banana 2 — verified
- **Best for:** the fast/cheap general-purpose workhorse — this project's default for the 419-item inventory catalog and one-off faction/fauna archetypes.
- Same brief-style prompting as Pro, lighter weight; use for volume, not hero assets.

## GPT Image 2 — verified
- **Best for:** anything that needs to behave like a *designed* asset — clean layouts, product-style compositions, high pixel-stability across repeated edits. Good hero-item alternate to Nano Banana 2.
- **Structure:** natural language, 1–3 information-dense sentences; can carry semantic intent ("should look inhabited, not abandoned"), not just keyword tags.
- **Notable:** 95%+ text-rendering accuracy, up to 16 reference images, native 4K. That text strength is a reason to route *away* from it for most of this project — almost nothing here needs baked-in text (UI Toolkit renders all text live); reserve it for the rare exception.
- **Length:** 40–120 words typical.

## Seedream 5.0 Lite — verified
- Launched 2026-02-13. ~$0.035/image, up to 14 reference images, built-in reasoning. A direct alternative to Nano Banana 2 for catalog-volume work — worth A/B testing once real generation starts.

## Seedream 5.0 Pro — verified (exists; not currently load-bearing here)
- Launched 2026-07-08. ~$0.075/image up to 2.36MP. Adds precision layer-style editing and reference-grade output tuned as a Seedance video-anchor frame. Nothing in this static-2D-UI project currently needs layer-precision editing or a video anchor — hold in reserve rather than route to by default; revisit if a trailer or animated key art gets commissioned.

## Kling Image (Kling Image 03 / Kling 3.0) — verified
- Visual Chain-of-Thought reasoning, up to 10 reference images. Structure: Subject + Action + Context (3–5 elements) + Style.
- **Not recommended as a default here** — this is a static 2D UI Toolkit game with no motion pipeline, and Kling's differentiators lean image-to-video. Keep it in the matrix for the day a trailer is commissioned (pair a still prompt with a separate Kling Motion Brief, per this system's own rule about not contaminating a still prompt with motion instructions).

## Recraft V4 / V4.1 — verified
- **Best for:** the small vector UI icon family (eye, shield, heart, pill, hourglass, checkmark) — native scalable SVG, no tracing/cleanup, stays clean at 16–24px.
- Prompt in plain graphic-design language: shape, stroke weight, fill/no-fill, color, background. Skip lighting/material language — it's a flat icon, not an illustration.

## Adobe Firefly — general knowledge, not re-verified this pass
- Strong photoreal/painterly concept-art register; the project's own earlier prompt file already used it for environment concept art (sound in spirit, if not in model-naming accuracy — see `EXISTING_PROMPT_AUDIT.md`). Good secondary for key art and establishing shots.
- Structure: subject + descriptors + environment + art treatment, 20–70 words — usually the shortest prompts in the set. Configure Content Type / Style Reference / Aspect Ratio / Lighting in Firefly's own UI rather than folding all of it into text.

## Midjourney — general knowledge
- Secondary painterly alternative for characters/environments when variety across many one-off assets (e.g. the 47 scavenging locations) matters more than exact control. Keep `--` parameter syntax out of any prompt meant to double as input to another model.

## Ideogram — general knowledge
- **Not recommended** for almost everything here today: almost nothing needs baked-in typography (UI Toolkit renders text live). Hold in reserve solely for a future logo/title-card key-art piece — and only once the ASHFALL/LAST STATIC naming question in `GAME_VISUAL_DNA.md` is resolved.

## Runway, Grok Imagine, Qwen Image — general knowledge, secondary/exploratory
- **Runway:** relevant only if a cinematic trailer or storyboard is commissioned; not for gameplay assets.
- **Grok Imagine:** viable general-purpose secondary for environments/VFX; no differentiated strength identified for this project specifically.
- **Qwen Image:** fallback/exploratory option; no current primary use case here.

## Sources (WebSearch, 2026-08-12)
- [Prompting Guide - FLUX.2 pro & max - Black Forest Labs](https://docs.bfl.ml/guides/prompting_guide_flux2)
- [Google Nano Banana Pro: The Complete Guide for 2026](https://wavespeed.ai/blog/posts/google-nano-banana-pro-complete-guide-2026/)
- [Nano Banana 2 guide: Prompts, features, and examples](https://www.magnific.com/blog/nano-banana-2/)
- [GPT Image 2 Prompt Guide (2026)](https://pixverse.ai/en/blog/gpt-image-2-review-and-prompt-guide)
- [Seedream 5.0 Pro Review: ByteDance's Multimodal Image Model (2026)](https://www.buildfastwithai.com/blogs/seedream-5-0-pro-review-bytedance-multimodal-image-model-2026)
- [Seedream 5.0 Complete Guide 2026: Lite vs Pro](https://createvision.ai/guides/seedream-5-complete-guide)
- [Introducing Recraft V4](https://www.recraft.ai/blog/introducing-recraft-v4-design-taste-meets-image-generation)
- [Recraft V4.1 for Brand Design](https://www.mindstudio.ai/blog/recraft-v4-1-brand-design-logos-svg-assets)
- [Kling AI Image Generation User Guide](https://kling.ai/quickstart/ai-image-generation-guide)
- [Kling IMAGE 3.0 Model User Guide](https://kling.ai/quickstart/klingai-image-3-model-user-guide)
