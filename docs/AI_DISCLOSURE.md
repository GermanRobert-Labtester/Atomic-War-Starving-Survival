# AI Content Disclosure — ASHFALL

> Draft for Steam's AI-content questionnaire. Fill in the bracketed placeholders
> before submission. Delete sections that don't apply.

---

## 1. Which assets were generated or assisted by AI?

| Category | AI-Generated? | Details |
|----------|--------------|---------|
| **Code (C#, shaders)** | [YES / NO] | [e.g. "Core game systems were written by the human developer. AI coding assistants were used for boilerplate, data importers, and test scaffolding."] |
| **2D Art (sprites, icons, UI)** | [YES / NO] | [e.g. "Placeholder art generated with Midjourney v6. Final art will be hand-painted or composited by the developer."] |
| **3D Art / Models** | [YES / N/A] | [N/A if no 3D assets] |
| **Music** | [YES / NO / N/A] | [e.g. "None currently. If added, will be licensed stock or human-composed."] |
| **Sound Effects** | [YES / NO / N/A] | [e.g. "Freesound.org licensed samples. No AI generation."] |
| **Narrative / Dialogue Text** | [YES / NO] | [e.g. "Event text and survivor bios were drafted by the developer with AI editing assistance."] |
| **Voice Acting** | [YES / NO / N/A] | [N/A if no voice] |
| **Translations** | [YES / NO / N/A] | [N/A if English-only] |

---

## 2. Which AI models were used?

| Model | Version | Provider | Used For |
|-------|---------|----------|----------|
| [e.g. Claude] | [e.g. Sonnet 4] | [e.g. Anthropic] | [e.g. "Code generation, test writing, system design"] |
| [e.g. Midjourney] | [e.g. v6] | [e.g. Midjourney Inc.] | [e.g. "Placeholder 2D art, concept sketches"] |
| [e.g. ChatGPT] | [e.g. GPT-4o] | [e.g. OpenAI] | [e.g. "Narrative text drafts, event descriptions"] |

---

## 3. What human editing was applied?

> Describe the human review, modification, and creative direction applied to
> every AI-generated output before it ships in the final product.

- **Code**: [e.g. "Every AI-generated function was reviewed, tested, and often rewritten. Architecture decisions, game design, and tuning are human-authored."]
- **Art**: [e.g. "AI-generated images are used as reference/placeholder only. Final sprites will be hand-painted over AI base, recolored, and composited by the developer. See HUMAN_AUTHORSHIP.md for the full checklist."]
- **Text**: [e.g. "All narrative text was reviewed and edited for tone, consistency, and thematic fit. The game's voice (cold, exhausted, human, restrained) is a deliberate human creative choice."]
- **Audio**: [e.g. "N/A — no AI-generated audio in current build."]

---

## 4. Rights and licensing

- **AI-generated code**: The developer holds full rights to all code in the repository. AI-generated code was produced under the developer's direction and is treated as work-for-hire.
- **AI-generated art**: [e.g. "Placeholder art was generated under a commercial license (Midjourney Pro plan). Final art will be original human work or properly licensed stock."]
- **AI-generated text**: All narrative content is original to this project. AI tools were used as drafting aids; the final text is human-approved.
- **Third-party assets**: [e.g. "All third-party assets (fonts, sound effects, plugins) are properly licensed. See CREDITS.md."]

---

## 5. Does the game use AI at runtime?

**No.** ASHFALL does not use any LLM, generative AI, or cloud-based AI service
at runtime. All NPC decision-making uses a deterministic Utility AI system
(authored in C#, no machine learning). The game runs fully offline.

---

## 6. Additional notes

> Add any context that helps Steam reviewers understand the role of AI in
> development.

[e.g. "AI was used as a development tool (like an IDE or compiler), not as a content delivery mechanism. The game itself is entirely deterministic and runs offline. All AI-assisted outputs go through human review before inclusion."]
