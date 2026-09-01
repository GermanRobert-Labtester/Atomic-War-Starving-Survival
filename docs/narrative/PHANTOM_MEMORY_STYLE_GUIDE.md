# Phantom Memory & Heirloom Style Guide

> **Author:** Narrative Design & Worldbuilding Group
> **Applicability:** All phantom memory fragments, heirloom descriptions, provenance records, and confession texts in ASHFALL.

---

## 1. Core Narrative Voice

Phantom memories are not ghostly manifestations or psychic transmissions. They are **human associations, tactile recalls, and sensory reconstructions** grounded in the material reality of a world that ended in minutes.

The strength of the phantom memory layer lies in **restraint, precision, and emotional understatement**.

---

## 2. Voice Principles

### 2.1 Perspective
- Use **third-person close** anchored around the survivor (`{name}`), or **direct sensory observation**.
- When addressing `{name}`, describe physical actions and subtle bodily responses (hands steadying, throat tightening, a thumb checking a worn edge) rather than internal dramatic monologues.

### 2.2 Sentence Length & Rhythm
- Keep memory fragments tight: **1 to 4 sentences**.
- Pacing should feel like a sudden inhalation: observation $\rightarrow$ tactile memory $\rightarrow$ quiet resolution or heavy silence.

### 2.3 Tactile Detail Over Abstraction
Avoid generic sadness. Anchor every memory in physical, mundane specifics:
- *Good:* "A brass safety razor, dull on one corner where the plating wore thin twenty years before the sirens. {name} runs a thumb along the comb and remembers a father humming off-key over hot tap water."
- *Avoid:* "You feel a profound sense of despair looking at this sad item from the lost civilization."

### 2.4 No Omniscient Exposition
- A spoon does not explain military treaties or high command casualty estimates.
- The fragment must only reveal what a normal human being saw, felt, touched, or feared in the rhythm of daily life.

### 2.5 Interpretive Ambiguity
- Allow natural ambiguity: is this the survivor's exact memory, a familiar sight reconstructed from childhood stories, or a universal human gesture recognized across the ruins?
- The game engine never declares that an object is magical. The survivor's mind and heart do the work.

### 2.6 Balance of Tone
- Avoid unrelenting despair across 100% of triggers.
- Life before the Exchange included mundane boredom, small acts of kindness, stubborn professional pride, half-finished errands, and bittersweet affection.
- Work objects should reflect **labor dignity and routine**.
- Ordinary objects should reflect **interrupted ordinariness**.
- Keepsakes should reflect **unspoken loyalty and memory**.

---

## 3. Trigger Taxonomy

| Class | Description | Tone Goal | Examples |
|---|---|---|---|
| **Personal Keepsakes** | Mementos of intimacy, family, love, and promises. | Bittersweet, protective, private grief. | Wedding band, photo, child's mitten, medal. |
| **Work Objects** | Tools of labor, craft, trade, and industry. | Pride, exhaustion, muscle memory, duty. | Foreman's whistle, caliper, nurse's watch, miner's tag. |
| **Ordinary Objects** | Mundane ephemera of civilization. | Haunting in their normalcy, interrupted routine. | Bus ticket, receipt, shopping list, enamel mug. |
| **Institutional Remnants** | Badges, forms, charts, and public safety gear. | Lost order, bureaucratic tragedy, triage. | Evacuation pass, patient chart, stamp, badge. |

---

## 4. Text Interpolation Standards

- Always use `{name}` as the survivor placeholder.
- Ensure sentences read naturally with any survivor name:
  - *Format:* `{name} turns the slide rule over in grease-stained fingers...`
- When writing dialogue quotes, use single quotes inside double-quoted JSON strings:
  - *Format:* `"{name} pockets the tags. 'I'll remember them,' they say quietly."`
