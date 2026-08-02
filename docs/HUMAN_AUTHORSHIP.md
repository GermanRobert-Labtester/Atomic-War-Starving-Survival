# Human Authorship Checklist — ASHFALL

> This checklist ensures that no AI-generated asset ships in its raw form.
> Every visual, audio, and text asset must pass through human creative judgment
> before it enters the final build.

---

## Visual Art

### Sprites & Icons
- [ ] **Paint-over**: Every AI-generated sprite has been painted over by hand — brushstrokes, color choices, and details are human-authored
- [ ] **Recolored**: Color palette adjusted to match the game's visual identity (desaturated, cold, amber-grey fallout tones)
- [ ] **Composited**: AI-generated elements composited with original human art, not used as-is
- [ ] **Silhouette check**: Character/object silhouettes are distinct and readable at game zoom
- [ ] **Style consistency**: No sprite looks "out of place" next to hand-authored assets
- [ ] **Resolution/format**: Correct sprite sheet format, pivot points, and compression for the target platform

### UI Elements
- [ ] **Layout**: UI layout is human-designed, not AI-generated
- [ ] **Typography**: Font choices are licensed and human-selected for readability + tone
- [ ] **Icons**: All UI icons are either hand-drawn or heavily modified from AI base
- [ ] **Color scheme**: Matches the game's cold, exhausted, restrained palette

### Environment Art
- [ ] **Tilemaps**: Tile sets are human-authored or paint-over-composited from AI reference
- [ ] **Lighting**: 2D lighting setup is human-tuned for mood (URP 2D lights)
- [ ] **Parallax/backgrounds**: Background layers are human-composited, not raw AI output

---

## Narrative & Text

- [ ] **Tone**: All text passes the "cold, exhausted, human, restrained" tone check
- [ ] **No AI tells**: No "In this post-apocalyptic world..." or "As a survivor, you must..." filler
- [ ] **Consistency**: Character names, locations, and lore are internally consistent
- [ ] **Sensitivity**: No glorification of violence, real-world references, or insensitive content
- [ ] **Proofread**: Human proofread — no AI-hallucinated facts or anachronisms

---

## Audio (if applicable)

- [ ] **Music**: Licensed stock or human-composed. No raw AI music in final build
- [ ] **SFX**: Human-curated and edited. No raw AI sound effects
- [ ] **Voice**: If voice is added, human-performed or properly licensed TTS with human direction

---

## Code

- [ ] **Architecture**: System architecture is human-designed (Utility AI, event bus, save system)
- [ ] **Review**: Every AI-generated function reviewed for correctness, security, and style
- [ ] **Tests**: Test scenarios and acceptance criteria are human-defined
- [ ] **Tuning**: All balance values (need rates, radiation thresholds, recipe costs) are human-tuned from playtesting
- [ ] **No AI runtime dependency**: Game runs fully offline with no LLM/AI calls at runtime

---

## Process

- [ ] **No raw AI output ships**: Every asset passes through this checklist before merging
- [ ] **Version control**: Git history shows human review commits on top of AI-generated drafts
- [ ] **Playtesting**: Human playtested after every art/text/code integration pass

---

> **Goal**: The final product should feel authored — not generated. Every pixel,
> word, and system should reflect human creative intent.
