# EXPANSION CW126-03 — Counted by Touch

## A prose-first game-content plan grounded in a single local narrative encounter record.

### Prose Wave 126: Small Signals, Unfinished Stories

## Batch brief

**Content type:** original narrative prose proposal with four alternative forms per beat.
**Content bank:** six editorial movements × eight source phrases × four drafts = 192 optional candidates; selection is editorial, not a promise that all text will ship.
**Current local anchor:** `enc_roadside_trader` — The Roadside Trader.
**Source file:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`.
**Thesis:** A roadside barter scene built around a trader’s expertise, terms, and privacy rather than around the observer’s assumptions about blindness.
**Scope:** prose/content planning only; no production code, authoritative JSON, mechanics, route, quest, flags, simulation, or save change.

## 1. Expansion thesis

A roadside barter scene built around a trader’s expertise, terms, and privacy rather than around the observer’s assumptions about blindness. The plan builds an optional scene bank around the exact local description “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” and its existing choice text. It adds no confirmed history. The six movements are a writer’s organization, not a required chronology, quest chain, visit count, or dependency on player completion.

## 2. Story question

How can a trade conversation recognize the woman’s tactile counting as competence while leaving her name and terms under her control?

## 3. Verified source record

The source record contains these exact fields: id: "enc_roadside_trader"; title: "The Roadside Trader"; description: "A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind."; category: "Trade"; baseWeight: 3.0; stealthWeightMultiplier: 1.0; speedWeightMultiplier: 1.0; minDangerLevel: 0.0; requiredLocationId: ""; forceOnArrival: false; choices: [{"choiceId": "fair_trade", "text": "Trade medical supplies for the salt and tins.", "moraleDelta": 2, "guiltDelta": 1}, {"choiceId": "flashlight_only", "text": "Trade only for the flashlight. Keep your medical supplies.", "moraleDelta": 3, "guiltDelta": 0}, {"choiceId": "everything_trade", "text": "Trade everything she asks for everything she has.", "moraleDelta": 1, "guiltDelta": 3}, {"choiceId": "refuse_and_leave", "text": "Refuse the trade. Leave her on the roadside.", "moraleDelta": 0, "guiltDelta": 2}]. Source: `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. Preserve field values and authorship. The description establishes the limited factual floor; every line of new dialogue, reaction, scene staging, and callback below is proposed writing.

| Local source | Anchor | Current record facts |
|---|---|---|
| `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | `enc_roadside_trader` | title=The Roadside Trader; category=Trade; description=A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind. |

### Existing choice text (reference only)

The following choice IDs, texts, and morale/guilt values are unchanged source data. They are transcribed here so a prose author can see the current language; the numerical deltas are resolver inputs, not a narrative judgment or a writing target. Do not add a new choice, reinterpret a delta as ethical truth, or claim these choices already display this expansion text.

- `fair_trade` — “Trade medical supplies for the salt and tins.” (moraleDelta 2, guiltDelta 1)
- `flashlight_only` — “Trade only for the flashlight. Keep your medical supplies.” (moraleDelta 3, guiltDelta 0)
- `everything_trade` — “Trade everything she asks for everything she has.” (moraleDelta 1, guiltDelta 3)
- `refuse_and_leave` — “Refuse the trade. Leave her on the roadside.” (moraleDelta 0, guiltDelta 2)

## 4. Fixed canon and open space

Do not make blindness a trick, weakness, cure plot, supernatural sense, or proof that another person should count for her. Do not invent the medical supply type, prices, stock amounts beyond the stated goods, illness, history, or destination. Avoid describing her as helpless or suspicious because she withholds her name. No trade system, price table, or medical advice is proposed.

Only the source record itself is fixed canon for this plan. New lines, gestures, voices, notebook fragments, and temporal returns are candidate prose. Do not quietly promote them into character biography, location history, faction doctrine, or a guaranteed campaign outcome. This record is present in the expansion JSON, but current NarrativeEncounterCatalogLoader loads narrative_encounters.json, narrative_encounters_npc_arcs.json, and micro_locations.json. ContentUtilizationScanner references are static mapping declarations, not proof that this expansion file is loaded. Treat every passage below as editorial and currently unverified for runtime reachability.

## 5. Human center

She has a cart, a small described set of goods, an unmet request for medical supplies, and no offered name. Her identity is not a question the player is owed an answer to.

The protagonist is not entitled to complete another person’s story. Keep agency visible through the right to offer, refuse, wait, remain unnamed, or end an exchange. Do not use distress as a shortcut to force a response from the player.

## 6. Voice and point of view

- **A traveler negotiating carefully:** States what they have and asks before moving any goods.
- **The trader, in optional proposed dialogue:** Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.
- **A companion who is learning to wait:** Notices the impulse to assist and lets the trader direct the exchange instead.

All voices and dialogue are editorial unless the source explicitly quotes them. The encounter description is not a transcript. Use simple diction and differentiated attention: one person may describe a physical fact, another may qualify an inference, and a third may stop the conversation. Never attribute a proposed sentence to the source character without an authored decision and a clear speaker label.

## 7. Placement and current reachability

The local source is a separate narrative_encounters_expansion.json file. The production NarrativeEncounterCatalogLoader names narrative_encounters.json as its base file and additionally loads narrative_encounters_npc_arcs.json and micro_locations.json; NarrativeHostSession registers that loader’s output. The expansion file is not among those filenames. Static scanner mappings do not establish runtime reachability. These drafts therefore propose content only and do not claim to be playable, registered, or routed.

No generated draft is present in production data. If a future content owner considers a line, verify the present schema and existing consumer first; do not create a parallel catalog, generic narrative panel, new route, registry, or save path as part of this prose plan. A source-file entry and a static utilization mapping are not runtime proof.

## 8. Player agency

The draft bank makes room for reading, asking, listening, declining, leaving, and silence. These are authoring postures, not promised controls or branches. The plan does not attach trust, morality, reputation, inventory, medical state, faction standing, relationship values, rewards, unlocks, or saved outcomes to a player’s interpretation. Existing source choices remain the complete choice list unless a separately authorized content decision changes them.

## 9. Continuity, dignity, and safety

**Boundary review.** Separate direct observation from inference in every line. The plan’s specific exclusions are listed in Section 4; carry those limits into voice, staging, and callback text without restating them as new setting facts.

Keep the distinction visible between a catalog fact, a character’s allegation, a traveler’s inference, and an editor’s optional flourish. Material detail should be quiet and specific. No real places, people, wars, hazardous procedures, medical recommendations, weapon techniques, or copied game text are introduced.

## 10. Existing owner and implementation boundary

The content anchor is `enc_roadside_trader` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`. The existing narrative encounter owner is `NarrativeEncounterSystem`, and `NarrativeEncounterCatalogLoader` is the relevant current loader. This plan proposes prose only. It does not claim a playable route, an active UI presentation, a new resolver behavior, or a data migration. No production file is changed by the plan.

## 11. Narrative sequence

These six movements arrange the writer’s questions from first observation to an unresolved exit. They are not additional encounter instances and do not prescribe a game-day order. Each can stand alone; some can be omitted entirely.

### Movement 1: Cart at the Shoulder

A wooden cart has been brought onto a highway shoulder, making an ordinary trading space in a dangerous world. The movement asks: What does the cart say about work without supplying a biography? Its source handle is “wooden cart”: A working object that marks the encounter as trade. Use the question to shape a passage, not to announce a correct player response.

### Movement 2: Goods Named Plainly

Salt, two rusted tins of beans, and a working flashlight are the described offer. The movement asks: Can the prose preserve exact limits and avoid upgrading the stock? Its source handle is “highway shoulder”: A narrow place for negotiation, not a guaranteed safe market. Use the question to shape a passage, not to announce a correct player response.

### Movement 3: A Request for Medical Supplies

The request is clear, while the object and terms remain open. The movement asks: How can a scene keep a request concrete without inventing a diagnosis? Its source handle is “salt”: A named good without quantity or use invented here. Use the question to shape a passage, not to announce a correct player response.

### Movement 4: No Name Offered

The woman does not offer her name. That boundary needs no justification. The movement asks: Can a conversation continue without treating identity as payment? Its source handle is “two rusted tins of beans”: An exact count and condition; nothing about contents is added. Use the question to shape a passage, not to announce a correct player response.

### Movement 5: Counting by Touch

Tactile counting is her stated practice and a demonstration of agency. The movement asks: Which narrator assumption can be removed so the counting stays hers? Its source handle is “working flashlight”: A functional object, not a promise of batteries or future safety. Use the question to shape a passage, not to announce a correct player response.

### Movement 6: Terms End Where She Says

The trade, refusal, or departure may finish without becoming a rescue story. The movement asks: How can the final line leave the trader’s next move unclaimed? Its source handle is “medical supplies”: A broad request whose intended use remains unknown. Use the question to shape a passage, not to announce a correct player response.

## 12. Beat bank: alternative prose drafts

Each movement meets all eight source handles. The four alternatives are: a present scene, a proposed field-note fragment, an attributed conversation, and a conditional return vignette. They are comparison drafts, not cumulative dialogue or a requirement to write 192 separate runtime events. Where a candidate needs a dialogue or note surface that the current content owner does not support, keep it in planning or discard it; do not invent interface or data architecture here.

### Beat 01: Cart at the Shoulder × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “Cart at the Shoulder” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 001 — wooden cart — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 001.** Leave one full beat of silence after “wooden cart.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 001, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 001 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, scene draft, “Cart at the Shoulder” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “wooden cart” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, scene draft, return to “wooden cart” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 001 — wooden cart — Cart at the Shoulder

This proposed field-note fragment, beat 001 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 001.** Put “wooden cart” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 001, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 001 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, field-note fragment, “Cart at the Shoulder” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “wooden cart” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, field-note fragment, return to “wooden cart” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 001 — wooden cart — Cart at the Shoulder

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “wooden cart” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 001.** Let a practical question about “wooden cart” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 001, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 001 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conversation fragment, “Cart at the Shoulder” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 001, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “wooden cart” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conversation fragment, return to “wooden cart” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 001 — wooden cart — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 001.** End the passage one sentence earlier than instinct suggests. Keep “wooden cart” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 001, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 001 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 001, conditional return vignette, “Cart at the Shoulder” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 001, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “wooden cart” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 001, conditional return vignette, return to “wooden cart” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 02: Cart at the Shoulder × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “Cart at the Shoulder” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 002 — highway shoulder — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 002.** Let a practical question about “highway shoulder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 002, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 002 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, scene draft, “Cart at the Shoulder” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “highway shoulder” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, scene draft, return to “highway shoulder” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 002 — highway shoulder — Cart at the Shoulder

This proposed field-note fragment, beat 002 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 002.** End the passage one sentence earlier than instinct suggests. Keep “highway shoulder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 002, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 002 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, field-note fragment, “Cart at the Shoulder” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “highway shoulder” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, field-note fragment, return to “highway shoulder” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 002 — highway shoulder — Cart at the Shoulder

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “highway shoulder” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 002.** Begin after the first response rather than at arrival. Let the reader encounter “highway shoulder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 002, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 002 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conversation fragment, “Cart at the Shoulder” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 002, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “highway shoulder” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conversation fragment, return to “highway shoulder” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 002 — highway shoulder — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 002.** Leave one full beat of silence after “highway shoulder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 002, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 002 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 002, conditional return vignette, “Cart at the Shoulder” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 002, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “highway shoulder” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 002, conditional return vignette, return to “highway shoulder” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 03: Cart at the Shoulder × salt

**Beat question:** What can the writer say about “salt” during “Cart at the Shoulder” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 003 — salt — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 003.** Begin after the first response rather than at arrival. Let the reader encounter “salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 003, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 003 gives A companion who is learning to wait a distinct perspective on “salt” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, scene draft, “Cart at the Shoulder” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “salt” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, scene draft, return to “salt” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 003 — salt — Cart at the Shoulder

This proposed field-note fragment, beat 003 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 003.** Leave one full beat of silence after “salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 003, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 003 gives A traveler negotiating carefully a distinct perspective on “salt” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, field-note fragment, “Cart at the Shoulder” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “salt” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, field-note fragment, return to “salt” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 003 — salt — Cart at the Shoulder

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “salt” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 003.** Put “salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 003, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 003 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conversation fragment, “Cart at the Shoulder” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 003, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “salt” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conversation fragment, return to “salt” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 003 — salt — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 003.** Let a practical question about “salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 003, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 003 gives A companion who is learning to wait a distinct perspective on “salt” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 003, conditional return vignette, “Cart at the Shoulder” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 003, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “salt” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 003, conditional return vignette, return to “salt” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 04: Cart at the Shoulder × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “Cart at the Shoulder” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 004 — two rusted tins of beans — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 004.** Put “two rusted tins of beans” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 004, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 004 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, scene draft, “Cart at the Shoulder” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “two rusted tins of beans” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, scene draft, return to “two rusted tins of beans” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 004 — two rusted tins of beans — Cart at the Shoulder

This proposed field-note fragment, beat 004 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 004.** Let a practical question about “two rusted tins of beans” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 004, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 004 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, field-note fragment, “Cart at the Shoulder” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “two rusted tins of beans” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, field-note fragment, return to “two rusted tins of beans” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 004 — two rusted tins of beans — Cart at the Shoulder

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “two rusted tins of beans” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 004.** End the passage one sentence earlier than instinct suggests. Keep “two rusted tins of beans” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 004, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 004 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conversation fragment, “Cart at the Shoulder” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 004, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “two rusted tins of beans” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conversation fragment, return to “two rusted tins of beans” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 004 — two rusted tins of beans — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 004.** Begin after the first response rather than at arrival. Let the reader encounter “two rusted tins of beans” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 004, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 004 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 004, conditional return vignette, “Cart at the Shoulder” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 004, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “two rusted tins of beans” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 004, conditional return vignette, return to “two rusted tins of beans” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 05: Cart at the Shoulder × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “Cart at the Shoulder” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 005 — working flashlight — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 005.** End the passage one sentence earlier than instinct suggests. Keep “working flashlight” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 005, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 005 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, scene draft, “Cart at the Shoulder” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “working flashlight” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, scene draft, return to “working flashlight” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 005 — working flashlight — Cart at the Shoulder

This proposed field-note fragment, beat 005 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 005.** Begin after the first response rather than at arrival. Let the reader encounter “working flashlight” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 005, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 005 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, field-note fragment, “Cart at the Shoulder” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “working flashlight” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, field-note fragment, return to “working flashlight” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 005 — working flashlight — Cart at the Shoulder

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “working flashlight” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 005.** Leave one full beat of silence after “working flashlight.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 005, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 005 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conversation fragment, “Cart at the Shoulder” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 005, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “working flashlight” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conversation fragment, return to “working flashlight” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 005 — working flashlight — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 005.** Put “working flashlight” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 005, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 005 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 005, conditional return vignette, “Cart at the Shoulder” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 005, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “working flashlight” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 005, conditional return vignette, return to “working flashlight” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 06: Cart at the Shoulder × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “Cart at the Shoulder” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 006 — medical supplies — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 006.** Leave one full beat of silence after “medical supplies.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 006, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 006 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, scene draft, “Cart at the Shoulder” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “medical supplies” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, scene draft, return to “medical supplies” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 006 — medical supplies — Cart at the Shoulder

This proposed field-note fragment, beat 006 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 006.** Put “medical supplies” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 006, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 006 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, field-note fragment, “Cart at the Shoulder” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “medical supplies” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, field-note fragment, return to “medical supplies” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 006 — medical supplies — Cart at the Shoulder

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “medical supplies” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 006.** Let a practical question about “medical supplies” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 006, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 006 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conversation fragment, “Cart at the Shoulder” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 006, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “medical supplies” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conversation fragment, return to “medical supplies” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 006 — medical supplies — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 006.** End the passage one sentence earlier than instinct suggests. Keep “medical supplies” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 006, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 006 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 006, conditional return vignette, “Cart at the Shoulder” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 006, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “medical supplies” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 006, conditional return vignette, return to “medical supplies” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 07: Cart at the Shoulder × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “Cart at the Shoulder” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 007 — does not offer her name — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 007.** Let a practical question about “does not offer her name” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 007, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 007 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, scene draft, “Cart at the Shoulder” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “does not offer her name” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, scene draft, return to “does not offer her name” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 007 — does not offer her name — Cart at the Shoulder

This proposed field-note fragment, beat 007 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 007.** End the passage one sentence earlier than instinct suggests. Keep “does not offer her name” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 007, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 007 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, field-note fragment, “Cart at the Shoulder” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “does not offer her name” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, field-note fragment, return to “does not offer her name” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 007 — does not offer her name — Cart at the Shoulder

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “does not offer her name” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 007.** Begin after the first response rather than at arrival. Let the reader encounter “does not offer her name” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 007, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 007 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conversation fragment, “Cart at the Shoulder” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 007, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “does not offer her name” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conversation fragment, return to “does not offer her name” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 007 — does not offer her name — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 007.** Leave one full beat of silence after “does not offer her name.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 007, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 007 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 007, conditional return vignette, “Cart at the Shoulder” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 007, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “does not offer her name” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 007, conditional return vignette, return to “does not offer her name” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 08: Cart at the Shoulder × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “Cart at the Shoulder” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: What does the cart say about work without supplying a biography?

#### Scene draft 008 — counts by touch — Cart at the Shoulder

For “Cart at the Shoulder” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 008.** Begin after the first response rather than at arrival. Let the reader encounter “counts by touch” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 008, “Cart at the Shoulder” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 008 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, scene draft, “Cart at the Shoulder” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, scene draft, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “counts by touch” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, scene draft, return to “counts by touch” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 008 — counts by touch — Cart at the Shoulder

This proposed field-note fragment, beat 008 in “Cart at the Shoulder,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 008.** Leave one full beat of silence after “counts by touch.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 008, “Cart at the Shoulder” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 008 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “Cart at the Shoulder.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, field-note fragment, “Cart at the Shoulder” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, field-note fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “counts by touch” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, field-note fragment, return to “counts by touch” during “Cart at the Shoulder” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 008 — counts by touch — Cart at the Shoulder

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “counts by touch” during “Cart at the Shoulder,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 008.** Put “counts by touch” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 008, “Cart at the Shoulder” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 008 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Cart at the Shoulder.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conversation fragment, “Cart at the Shoulder” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 008, conversation fragment, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “counts by touch” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conversation fragment, return to “counts by touch” during “Cart at the Shoulder” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 008 — counts by touch — Cart at the Shoulder

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “Cart at the Shoulder” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 008.** Let a practical question about “counts by touch” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 008, “Cart at the Shoulder” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 008 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “Cart at the Shoulder.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 008, conditional return vignette, “Cart at the Shoulder” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 008, conditional return vignette, use the question—“What does the cart say about work without supplying a biography?”—as a revision test tied to “counts by touch” during “Cart at the Shoulder.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 008, conditional return vignette, return to “counts by touch” during “Cart at the Shoulder” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Cart at the Shoulder” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 09: Goods Named Plainly × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “Goods Named Plainly” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 009 — wooden cart — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 009.** End the passage one sentence earlier than instinct suggests. Keep “wooden cart” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 009, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 009 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, scene draft, “Goods Named Plainly” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “wooden cart” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, scene draft, return to “wooden cart” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 009 — wooden cart — Goods Named Plainly

This proposed field-note fragment, beat 009 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 009.** Begin after the first response rather than at arrival. Let the reader encounter “wooden cart” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 009, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 009 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, field-note fragment, “Goods Named Plainly” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “wooden cart” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, field-note fragment, return to “wooden cart” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 009 — wooden cart — Goods Named Plainly

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “wooden cart” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 009.** Leave one full beat of silence after “wooden cart.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 009, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 009 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conversation fragment, “Goods Named Plainly” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 009, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “wooden cart” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conversation fragment, return to “wooden cart” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 009 — wooden cart — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 009.** Put “wooden cart” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 009, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 009 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 009, conditional return vignette, “Goods Named Plainly” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 009, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “wooden cart” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 009, conditional return vignette, return to “wooden cart” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 10: Goods Named Plainly × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “Goods Named Plainly” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 010 — highway shoulder — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 010.** Leave one full beat of silence after “highway shoulder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 010, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 010 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, scene draft, “Goods Named Plainly” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “highway shoulder” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, scene draft, return to “highway shoulder” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 010 — highway shoulder — Goods Named Plainly

This proposed field-note fragment, beat 010 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 010.** Put “highway shoulder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 010, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 010 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, field-note fragment, “Goods Named Plainly” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “highway shoulder” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, field-note fragment, return to “highway shoulder” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 010 — highway shoulder — Goods Named Plainly

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “highway shoulder” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 010.** Let a practical question about “highway shoulder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 010, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 010 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conversation fragment, “Goods Named Plainly” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 010, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “highway shoulder” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conversation fragment, return to “highway shoulder” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 010 — highway shoulder — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 010.** End the passage one sentence earlier than instinct suggests. Keep “highway shoulder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 010, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 010 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 010, conditional return vignette, “Goods Named Plainly” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 010, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “highway shoulder” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 010, conditional return vignette, return to “highway shoulder” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 11: Goods Named Plainly × salt

**Beat question:** What can the writer say about “salt” during “Goods Named Plainly” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 011 — salt — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 011.** Let a practical question about “salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 011, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 011 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, scene draft, “Goods Named Plainly” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “salt” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, scene draft, return to “salt” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 011 — salt — Goods Named Plainly

This proposed field-note fragment, beat 011 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 011.** End the passage one sentence earlier than instinct suggests. Keep “salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 011, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 011 gives A companion who is learning to wait a distinct perspective on “salt” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, field-note fragment, “Goods Named Plainly” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “salt” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, field-note fragment, return to “salt” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 011 — salt — Goods Named Plainly

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “salt” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 011.** Begin after the first response rather than at arrival. Let the reader encounter “salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 011, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 011 gives A traveler negotiating carefully a distinct perspective on “salt” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conversation fragment, “Goods Named Plainly” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 011, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “salt” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conversation fragment, return to “salt” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 011 — salt — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 011.** Leave one full beat of silence after “salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 011, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 011 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 011, conditional return vignette, “Goods Named Plainly” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 011, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “salt” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 011, conditional return vignette, return to “salt” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 12: Goods Named Plainly × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “Goods Named Plainly” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 012 — two rusted tins of beans — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 012.** Begin after the first response rather than at arrival. Let the reader encounter “two rusted tins of beans” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 012, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 012 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, scene draft, “Goods Named Plainly” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “two rusted tins of beans” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, scene draft, return to “two rusted tins of beans” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 012 — two rusted tins of beans — Goods Named Plainly

This proposed field-note fragment, beat 012 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 012.** Leave one full beat of silence after “two rusted tins of beans.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 012, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 012 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, field-note fragment, “Goods Named Plainly” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “two rusted tins of beans” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, field-note fragment, return to “two rusted tins of beans” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 012 — two rusted tins of beans — Goods Named Plainly

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “two rusted tins of beans” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 012.** Put “two rusted tins of beans” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 012, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 012 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conversation fragment, “Goods Named Plainly” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 012, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “two rusted tins of beans” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conversation fragment, return to “two rusted tins of beans” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 012 — two rusted tins of beans — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 012.** Let a practical question about “two rusted tins of beans” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 012, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 012 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 012, conditional return vignette, “Goods Named Plainly” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 012, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “two rusted tins of beans” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 012, conditional return vignette, return to “two rusted tins of beans” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 13: Goods Named Plainly × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “Goods Named Plainly” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 013 — working flashlight — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 013.** Put “working flashlight” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 013, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 013 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, scene draft, “Goods Named Plainly” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “working flashlight” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, scene draft, return to “working flashlight” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 013 — working flashlight — Goods Named Plainly

This proposed field-note fragment, beat 013 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 013.** Let a practical question about “working flashlight” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 013, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 013 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, field-note fragment, “Goods Named Plainly” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “working flashlight” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, field-note fragment, return to “working flashlight” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 013 — working flashlight — Goods Named Plainly

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “working flashlight” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 013.** End the passage one sentence earlier than instinct suggests. Keep “working flashlight” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 013, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 013 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conversation fragment, “Goods Named Plainly” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 013, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “working flashlight” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conversation fragment, return to “working flashlight” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 013 — working flashlight — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 013.** Begin after the first response rather than at arrival. Let the reader encounter “working flashlight” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 013, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 013 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 013, conditional return vignette, “Goods Named Plainly” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 013, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “working flashlight” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 013, conditional return vignette, return to “working flashlight” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 14: Goods Named Plainly × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “Goods Named Plainly” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 014 — medical supplies — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 014.** End the passage one sentence earlier than instinct suggests. Keep “medical supplies” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 014, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 014 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, scene draft, “Goods Named Plainly” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “medical supplies” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, scene draft, return to “medical supplies” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 014 — medical supplies — Goods Named Plainly

This proposed field-note fragment, beat 014 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 014.** Begin after the first response rather than at arrival. Let the reader encounter “medical supplies” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 014, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 014 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, field-note fragment, “Goods Named Plainly” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “medical supplies” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, field-note fragment, return to “medical supplies” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 014 — medical supplies — Goods Named Plainly

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “medical supplies” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 014.** Leave one full beat of silence after “medical supplies.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 014, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 014 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conversation fragment, “Goods Named Plainly” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 014, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “medical supplies” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conversation fragment, return to “medical supplies” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 014 — medical supplies — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 014.** Put “medical supplies” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 014, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 014 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 014, conditional return vignette, “Goods Named Plainly” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 014, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “medical supplies” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 014, conditional return vignette, return to “medical supplies” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 15: Goods Named Plainly × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “Goods Named Plainly” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 015 — does not offer her name — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 015.** Leave one full beat of silence after “does not offer her name.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 015, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 015 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, scene draft, “Goods Named Plainly” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “does not offer her name” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, scene draft, return to “does not offer her name” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 015 — does not offer her name — Goods Named Plainly

This proposed field-note fragment, beat 015 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 015.** Put “does not offer her name” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 015, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 015 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, field-note fragment, “Goods Named Plainly” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “does not offer her name” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, field-note fragment, return to “does not offer her name” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 015 — does not offer her name — Goods Named Plainly

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “does not offer her name” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 015.** Let a practical question about “does not offer her name” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 015, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 015 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conversation fragment, “Goods Named Plainly” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 015, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “does not offer her name” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conversation fragment, return to “does not offer her name” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 015 — does not offer her name — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 015.** End the passage one sentence earlier than instinct suggests. Keep “does not offer her name” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 015, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 015 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 015, conditional return vignette, “Goods Named Plainly” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 015, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “does not offer her name” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 015, conditional return vignette, return to “does not offer her name” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 16: Goods Named Plainly × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “Goods Named Plainly” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: Can the prose preserve exact limits and avoid upgrading the stock?

#### Scene draft 016 — counts by touch — Goods Named Plainly

For “Goods Named Plainly” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 016.** Let a practical question about “counts by touch” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 016, “Goods Named Plainly” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 016 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, scene draft, “Goods Named Plainly” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, scene draft, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “counts by touch” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, scene draft, return to “counts by touch” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 016 — counts by touch — Goods Named Plainly

This proposed field-note fragment, beat 016 in “Goods Named Plainly,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 016.** End the passage one sentence earlier than instinct suggests. Keep “counts by touch” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 016, “Goods Named Plainly” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 016 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “Goods Named Plainly.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, field-note fragment, “Goods Named Plainly” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, field-note fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “counts by touch” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, field-note fragment, return to “counts by touch” during “Goods Named Plainly” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 016 — counts by touch — Goods Named Plainly

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “counts by touch” during “Goods Named Plainly,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 016.** Begin after the first response rather than at arrival. Let the reader encounter “counts by touch” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 016, “Goods Named Plainly” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 016 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “Goods Named Plainly.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conversation fragment, “Goods Named Plainly” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 016, conversation fragment, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “counts by touch” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conversation fragment, return to “counts by touch” during “Goods Named Plainly” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 016 — counts by touch — Goods Named Plainly

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “Goods Named Plainly” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 016.** Leave one full beat of silence after “counts by touch.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 016, “Goods Named Plainly” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 016 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Goods Named Plainly.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 016, conditional return vignette, “Goods Named Plainly” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 016, conditional return vignette, use the question—“Can the prose preserve exact limits and avoid upgrading the stock?”—as a revision test tied to “counts by touch” during “Goods Named Plainly.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 016, conditional return vignette, return to “counts by touch” during “Goods Named Plainly” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Goods Named Plainly” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 17: A Request for Medical Supplies × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “A Request for Medical Supplies” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 017 — wooden cart — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 017.** Put “wooden cart” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 017, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 017 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, scene draft, “A Request for Medical Supplies” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “wooden cart” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, scene draft, return to “wooden cart” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 017 — wooden cart — A Request for Medical Supplies

This proposed field-note fragment, beat 017 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 017.** Let a practical question about “wooden cart” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 017, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 017 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, field-note fragment, “A Request for Medical Supplies” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “wooden cart” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, field-note fragment, return to “wooden cart” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 017 — wooden cart — A Request for Medical Supplies

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “wooden cart” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 017.** End the passage one sentence earlier than instinct suggests. Keep “wooden cart” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 017, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 017 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conversation fragment, “A Request for Medical Supplies” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 017, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “wooden cart” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conversation fragment, return to “wooden cart” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 017 — wooden cart — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 017.** Begin after the first response rather than at arrival. Let the reader encounter “wooden cart” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 017, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 017 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 017, conditional return vignette, “A Request for Medical Supplies” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 017, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “wooden cart” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 017, conditional return vignette, return to “wooden cart” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 18: A Request for Medical Supplies × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “A Request for Medical Supplies” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 018 — highway shoulder — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 018.** End the passage one sentence earlier than instinct suggests. Keep “highway shoulder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 018, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 018 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, scene draft, “A Request for Medical Supplies” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “highway shoulder” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, scene draft, return to “highway shoulder” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 018 — highway shoulder — A Request for Medical Supplies

This proposed field-note fragment, beat 018 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 018.** Begin after the first response rather than at arrival. Let the reader encounter “highway shoulder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 018, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 018 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, field-note fragment, “A Request for Medical Supplies” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “highway shoulder” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, field-note fragment, return to “highway shoulder” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 018 — highway shoulder — A Request for Medical Supplies

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “highway shoulder” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 018.** Leave one full beat of silence after “highway shoulder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 018, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 018 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conversation fragment, “A Request for Medical Supplies” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 018, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “highway shoulder” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conversation fragment, return to “highway shoulder” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 018 — highway shoulder — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 018.** Put “highway shoulder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 018, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 018 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 018, conditional return vignette, “A Request for Medical Supplies” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 018, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “highway shoulder” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 018, conditional return vignette, return to “highway shoulder” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 19: A Request for Medical Supplies × salt

**Beat question:** What can the writer say about “salt” during “A Request for Medical Supplies” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 019 — salt — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 019.** Leave one full beat of silence after “salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 019, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 019 gives A traveler negotiating carefully a distinct perspective on “salt” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, scene draft, “A Request for Medical Supplies” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “salt” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, scene draft, return to “salt” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 019 — salt — A Request for Medical Supplies

This proposed field-note fragment, beat 019 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 019.** Put “salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 019, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 019 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, field-note fragment, “A Request for Medical Supplies” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “salt” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, field-note fragment, return to “salt” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 019 — salt — A Request for Medical Supplies

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “salt” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 019.** Let a practical question about “salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 019, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 019 gives A companion who is learning to wait a distinct perspective on “salt” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conversation fragment, “A Request for Medical Supplies” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 019, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “salt” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conversation fragment, return to “salt” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 019 — salt — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 019.** End the passage one sentence earlier than instinct suggests. Keep “salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 019, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 019 gives A traveler negotiating carefully a distinct perspective on “salt” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 019, conditional return vignette, “A Request for Medical Supplies” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 019, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “salt” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 019, conditional return vignette, return to “salt” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 20: A Request for Medical Supplies × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “A Request for Medical Supplies” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 020 — two rusted tins of beans — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 020.** Let a practical question about “two rusted tins of beans” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 020, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 020 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, scene draft, “A Request for Medical Supplies” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “two rusted tins of beans” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, scene draft, return to “two rusted tins of beans” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 020 — two rusted tins of beans — A Request for Medical Supplies

This proposed field-note fragment, beat 020 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 020.** End the passage one sentence earlier than instinct suggests. Keep “two rusted tins of beans” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 020, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 020 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, field-note fragment, “A Request for Medical Supplies” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “two rusted tins of beans” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, field-note fragment, return to “two rusted tins of beans” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 020 — two rusted tins of beans — A Request for Medical Supplies

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “two rusted tins of beans” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 020.** Begin after the first response rather than at arrival. Let the reader encounter “two rusted tins of beans” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 020, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 020 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conversation fragment, “A Request for Medical Supplies” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 020, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “two rusted tins of beans” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conversation fragment, return to “two rusted tins of beans” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 020 — two rusted tins of beans — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 020.** Leave one full beat of silence after “two rusted tins of beans.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 020, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 020 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 020, conditional return vignette, “A Request for Medical Supplies” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 020, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “two rusted tins of beans” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 020, conditional return vignette, return to “two rusted tins of beans” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 21: A Request for Medical Supplies × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “A Request for Medical Supplies” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 021 — working flashlight — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 021.** Begin after the first response rather than at arrival. Let the reader encounter “working flashlight” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 021, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 021 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, scene draft, “A Request for Medical Supplies” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “working flashlight” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, scene draft, return to “working flashlight” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 021 — working flashlight — A Request for Medical Supplies

This proposed field-note fragment, beat 021 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 021.** Leave one full beat of silence after “working flashlight.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 021, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 021 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, field-note fragment, “A Request for Medical Supplies” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “working flashlight” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, field-note fragment, return to “working flashlight” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 021 — working flashlight — A Request for Medical Supplies

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “working flashlight” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 021.** Put “working flashlight” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 021, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 021 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conversation fragment, “A Request for Medical Supplies” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 021, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “working flashlight” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conversation fragment, return to “working flashlight” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 021 — working flashlight — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 021.** Let a practical question about “working flashlight” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 021, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 021 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 021, conditional return vignette, “A Request for Medical Supplies” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 021, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “working flashlight” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 021, conditional return vignette, return to “working flashlight” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 22: A Request for Medical Supplies × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “A Request for Medical Supplies” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 022 — medical supplies — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 022.** Put “medical supplies” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 022, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 022 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, scene draft, “A Request for Medical Supplies” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “medical supplies” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, scene draft, return to “medical supplies” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 022 — medical supplies — A Request for Medical Supplies

This proposed field-note fragment, beat 022 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 022.** Let a practical question about “medical supplies” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 022, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 022 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, field-note fragment, “A Request for Medical Supplies” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “medical supplies” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, field-note fragment, return to “medical supplies” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 022 — medical supplies — A Request for Medical Supplies

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “medical supplies” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 022.** End the passage one sentence earlier than instinct suggests. Keep “medical supplies” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 022, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 022 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conversation fragment, “A Request for Medical Supplies” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 022, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “medical supplies” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conversation fragment, return to “medical supplies” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 022 — medical supplies — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 022.** Begin after the first response rather than at arrival. Let the reader encounter “medical supplies” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 022, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 022 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 022, conditional return vignette, “A Request for Medical Supplies” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 022, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “medical supplies” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 022, conditional return vignette, return to “medical supplies” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 23: A Request for Medical Supplies × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “A Request for Medical Supplies” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 023 — does not offer her name — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 023.** End the passage one sentence earlier than instinct suggests. Keep “does not offer her name” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 023, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 023 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, scene draft, “A Request for Medical Supplies” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “does not offer her name” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, scene draft, return to “does not offer her name” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 023 — does not offer her name — A Request for Medical Supplies

This proposed field-note fragment, beat 023 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 023.** Begin after the first response rather than at arrival. Let the reader encounter “does not offer her name” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 023, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 023 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, field-note fragment, “A Request for Medical Supplies” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “does not offer her name” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, field-note fragment, return to “does not offer her name” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 023 — does not offer her name — A Request for Medical Supplies

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “does not offer her name” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 023.** Leave one full beat of silence after “does not offer her name.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 023, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 023 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conversation fragment, “A Request for Medical Supplies” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 023, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “does not offer her name” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conversation fragment, return to “does not offer her name” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 023 — does not offer her name — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 023.** Put “does not offer her name” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 023, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 023 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 023, conditional return vignette, “A Request for Medical Supplies” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 023, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “does not offer her name” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 023, conditional return vignette, return to “does not offer her name” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 24: A Request for Medical Supplies × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “A Request for Medical Supplies” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: How can a scene keep a request concrete without inventing a diagnosis?

#### Scene draft 024 — counts by touch — A Request for Medical Supplies

For “A Request for Medical Supplies” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 024.** Leave one full beat of silence after “counts by touch.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 024, “A Request for Medical Supplies” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 024 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, scene draft, “A Request for Medical Supplies” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, scene draft, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “counts by touch” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, scene draft, return to “counts by touch” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 024 — counts by touch — A Request for Medical Supplies

This proposed field-note fragment, beat 024 in “A Request for Medical Supplies,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 024.** Put “counts by touch” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 024, “A Request for Medical Supplies” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 024 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “A Request for Medical Supplies.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, field-note fragment, “A Request for Medical Supplies” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, field-note fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “counts by touch” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, field-note fragment, return to “counts by touch” during “A Request for Medical Supplies” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 024 — counts by touch — A Request for Medical Supplies

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “counts by touch” during “A Request for Medical Supplies,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 024.** Let a practical question about “counts by touch” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 024, “A Request for Medical Supplies” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 024 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “A Request for Medical Supplies.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conversation fragment, “A Request for Medical Supplies” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 024, conversation fragment, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “counts by touch” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conversation fragment, return to “counts by touch” during “A Request for Medical Supplies” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 024 — counts by touch — A Request for Medical Supplies

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “A Request for Medical Supplies” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 024.** End the passage one sentence earlier than instinct suggests. Keep “counts by touch” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 024, “A Request for Medical Supplies” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 024 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “A Request for Medical Supplies.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 024, conditional return vignette, “A Request for Medical Supplies” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 024, conditional return vignette, use the question—“How can a scene keep a request concrete without inventing a diagnosis?”—as a revision test tied to “counts by touch” during “A Request for Medical Supplies.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 024, conditional return vignette, return to “counts by touch” during “A Request for Medical Supplies” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “A Request for Medical Supplies” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 25: No Name Offered × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “No Name Offered” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 025 — wooden cart — No Name Offered

For “No Name Offered” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 025.** Begin after the first response rather than at arrival. Let the reader encounter “wooden cart” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 025, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 025 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, scene draft, “No Name Offered” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “wooden cart” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, scene draft, return to “wooden cart” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 025 — wooden cart — No Name Offered

This proposed field-note fragment, beat 025 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 025.** Leave one full beat of silence after “wooden cart.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 025, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 025 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, field-note fragment, “No Name Offered” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “wooden cart” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, field-note fragment, return to “wooden cart” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 025 — wooden cart — No Name Offered

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “wooden cart” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 025.** Put “wooden cart” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 025, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 025 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conversation fragment, “No Name Offered” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 025, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “wooden cart” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conversation fragment, return to “wooden cart” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 025 — wooden cart — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 025.** Let a practical question about “wooden cart” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 025, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 025 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 025, conditional return vignette, “No Name Offered” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 025, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “wooden cart” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 025, conditional return vignette, return to “wooden cart” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 26: No Name Offered × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “No Name Offered” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 026 — highway shoulder — No Name Offered

For “No Name Offered” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 026.** Put “highway shoulder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 026, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 026 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, scene draft, “No Name Offered” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “highway shoulder” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, scene draft, return to “highway shoulder” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 026 — highway shoulder — No Name Offered

This proposed field-note fragment, beat 026 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 026.** Let a practical question about “highway shoulder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 026, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 026 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, field-note fragment, “No Name Offered” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “highway shoulder” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, field-note fragment, return to “highway shoulder” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 026 — highway shoulder — No Name Offered

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “highway shoulder” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 026.** End the passage one sentence earlier than instinct suggests. Keep “highway shoulder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 026, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 026 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conversation fragment, “No Name Offered” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 026, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “highway shoulder” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conversation fragment, return to “highway shoulder” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 026 — highway shoulder — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 026.** Begin after the first response rather than at arrival. Let the reader encounter “highway shoulder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 026, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 026 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 026, conditional return vignette, “No Name Offered” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 026, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “highway shoulder” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 026, conditional return vignette, return to “highway shoulder” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 27: No Name Offered × salt

**Beat question:** What can the writer say about “salt” during “No Name Offered” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 027 — salt — No Name Offered

For “No Name Offered” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 027.** End the passage one sentence earlier than instinct suggests. Keep “salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 027, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 027 gives A companion who is learning to wait a distinct perspective on “salt” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, scene draft, “No Name Offered” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “salt” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, scene draft, return to “salt” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 027 — salt — No Name Offered

This proposed field-note fragment, beat 027 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 027.** Begin after the first response rather than at arrival. Let the reader encounter “salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 027, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 027 gives A traveler negotiating carefully a distinct perspective on “salt” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, field-note fragment, “No Name Offered” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “salt” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, field-note fragment, return to “salt” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 027 — salt — No Name Offered

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “salt” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 027.** Leave one full beat of silence after “salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 027, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 027 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conversation fragment, “No Name Offered” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 027, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “salt” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conversation fragment, return to “salt” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 027 — salt — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 027.** Put “salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 027, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 027 gives A companion who is learning to wait a distinct perspective on “salt” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 027, conditional return vignette, “No Name Offered” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 027, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “salt” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 027, conditional return vignette, return to “salt” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 28: No Name Offered × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “No Name Offered” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 028 — two rusted tins of beans — No Name Offered

For “No Name Offered” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 028.** Leave one full beat of silence after “two rusted tins of beans.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 028, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 028 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, scene draft, “No Name Offered” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “two rusted tins of beans” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, scene draft, return to “two rusted tins of beans” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 028 — two rusted tins of beans — No Name Offered

This proposed field-note fragment, beat 028 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 028.** Put “two rusted tins of beans” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 028, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 028 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, field-note fragment, “No Name Offered” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “two rusted tins of beans” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, field-note fragment, return to “two rusted tins of beans” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 028 — two rusted tins of beans — No Name Offered

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “two rusted tins of beans” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 028.** Let a practical question about “two rusted tins of beans” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 028, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 028 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conversation fragment, “No Name Offered” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 028, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “two rusted tins of beans” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conversation fragment, return to “two rusted tins of beans” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 028 — two rusted tins of beans — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 028.** End the passage one sentence earlier than instinct suggests. Keep “two rusted tins of beans” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 028, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 028 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 028, conditional return vignette, “No Name Offered” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 028, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “two rusted tins of beans” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 028, conditional return vignette, return to “two rusted tins of beans” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 29: No Name Offered × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “No Name Offered” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 029 — working flashlight — No Name Offered

For “No Name Offered” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 029.** Let a practical question about “working flashlight” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 029, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 029 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, scene draft, “No Name Offered” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “working flashlight” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, scene draft, return to “working flashlight” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 029 — working flashlight — No Name Offered

This proposed field-note fragment, beat 029 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 029.** End the passage one sentence earlier than instinct suggests. Keep “working flashlight” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 029, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 029 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, field-note fragment, “No Name Offered” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “working flashlight” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, field-note fragment, return to “working flashlight” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 029 — working flashlight — No Name Offered

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “working flashlight” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 029.** Begin after the first response rather than at arrival. Let the reader encounter “working flashlight” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 029, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 029 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conversation fragment, “No Name Offered” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 029, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “working flashlight” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conversation fragment, return to “working flashlight” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 029 — working flashlight — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 029.** Leave one full beat of silence after “working flashlight.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 029, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 029 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 029, conditional return vignette, “No Name Offered” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 029, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “working flashlight” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 029, conditional return vignette, return to “working flashlight” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 30: No Name Offered × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “No Name Offered” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 030 — medical supplies — No Name Offered

For “No Name Offered” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 030.** Begin after the first response rather than at arrival. Let the reader encounter “medical supplies” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 030, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 030 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, scene draft, “No Name Offered” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “medical supplies” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, scene draft, return to “medical supplies” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 030 — medical supplies — No Name Offered

This proposed field-note fragment, beat 030 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 030.** Leave one full beat of silence after “medical supplies.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 030, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 030 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, field-note fragment, “No Name Offered” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “medical supplies” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, field-note fragment, return to “medical supplies” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 030 — medical supplies — No Name Offered

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “medical supplies” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 030.** Put “medical supplies” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 030, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 030 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conversation fragment, “No Name Offered” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 030, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “medical supplies” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conversation fragment, return to “medical supplies” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 030 — medical supplies — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 030.** Let a practical question about “medical supplies” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 030, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 030 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 030, conditional return vignette, “No Name Offered” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 030, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “medical supplies” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 030, conditional return vignette, return to “medical supplies” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 31: No Name Offered × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “No Name Offered” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 031 — does not offer her name — No Name Offered

For “No Name Offered” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 031.** Put “does not offer her name” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 031, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 031 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, scene draft, “No Name Offered” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “does not offer her name” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, scene draft, return to “does not offer her name” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 031 — does not offer her name — No Name Offered

This proposed field-note fragment, beat 031 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 031.** Let a practical question about “does not offer her name” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 031, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 031 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, field-note fragment, “No Name Offered” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “does not offer her name” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, field-note fragment, return to “does not offer her name” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 031 — does not offer her name — No Name Offered

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “does not offer her name” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 031.** End the passage one sentence earlier than instinct suggests. Keep “does not offer her name” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 031, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 031 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conversation fragment, “No Name Offered” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 031, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “does not offer her name” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conversation fragment, return to “does not offer her name” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 031 — does not offer her name — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 031.** Begin after the first response rather than at arrival. Let the reader encounter “does not offer her name” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 031, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 031 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 031, conditional return vignette, “No Name Offered” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 031, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “does not offer her name” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 031, conditional return vignette, return to “does not offer her name” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 32: No Name Offered × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “No Name Offered” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: Can a conversation continue without treating identity as payment?

#### Scene draft 032 — counts by touch — No Name Offered

For “No Name Offered” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 032.** End the passage one sentence earlier than instinct suggests. Keep “counts by touch” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 032, “No Name Offered” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 032 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, scene draft, “No Name Offered” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, scene draft, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “counts by touch” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, scene draft, return to “counts by touch” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 032 — counts by touch — No Name Offered

This proposed field-note fragment, beat 032 in “No Name Offered,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 032.** Begin after the first response rather than at arrival. Let the reader encounter “counts by touch” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 032, “No Name Offered” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 032 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “No Name Offered.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, field-note fragment, “No Name Offered” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, field-note fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “counts by touch” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, field-note fragment, return to “counts by touch” during “No Name Offered” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 032 — counts by touch — No Name Offered

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “counts by touch” during “No Name Offered,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 032.** Leave one full beat of silence after “counts by touch.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 032, “No Name Offered” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 032 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “No Name Offered.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conversation fragment, “No Name Offered” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 032, conversation fragment, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “counts by touch” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conversation fragment, return to “counts by touch” during “No Name Offered” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 032 — counts by touch — No Name Offered

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “No Name Offered” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 032.** Put “counts by touch” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 032, “No Name Offered” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 032 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “No Name Offered.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 032, conditional return vignette, “No Name Offered” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 032, conditional return vignette, use the question—“Can a conversation continue without treating identity as payment?”—as a revision test tied to “counts by touch” during “No Name Offered.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 032, conditional return vignette, return to “counts by touch” during “No Name Offered” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “No Name Offered” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 33: Counting by Touch × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “Counting by Touch” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 033 — wooden cart — Counting by Touch

For “Counting by Touch” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 033.** Let a practical question about “wooden cart” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 033, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 033 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, scene draft, “Counting by Touch” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “wooden cart” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, scene draft, return to “wooden cart” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 033 — wooden cart — Counting by Touch

This proposed field-note fragment, beat 033 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 033.** End the passage one sentence earlier than instinct suggests. Keep “wooden cart” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 033, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 033 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, field-note fragment, “Counting by Touch” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “wooden cart” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, field-note fragment, return to “wooden cart” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 033 — wooden cart — Counting by Touch

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “wooden cart” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 033.** Begin after the first response rather than at arrival. Let the reader encounter “wooden cart” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 033, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 033 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conversation fragment, “Counting by Touch” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 033, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “wooden cart” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conversation fragment, return to “wooden cart” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 033 — wooden cart — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 033.** Leave one full beat of silence after “wooden cart.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 033, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 033 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 033, conditional return vignette, “Counting by Touch” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 033, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “wooden cart” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 033, conditional return vignette, return to “wooden cart” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 34: Counting by Touch × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “Counting by Touch” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 034 — highway shoulder — Counting by Touch

For “Counting by Touch” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 034.** Begin after the first response rather than at arrival. Let the reader encounter “highway shoulder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 034, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 034 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, scene draft, “Counting by Touch” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “highway shoulder” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, scene draft, return to “highway shoulder” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 034 — highway shoulder — Counting by Touch

This proposed field-note fragment, beat 034 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 034.** Leave one full beat of silence after “highway shoulder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 034, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 034 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, field-note fragment, “Counting by Touch” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “highway shoulder” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, field-note fragment, return to “highway shoulder” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 034 — highway shoulder — Counting by Touch

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “highway shoulder” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 034.** Put “highway shoulder” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 034, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 034 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conversation fragment, “Counting by Touch” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 034, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “highway shoulder” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conversation fragment, return to “highway shoulder” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 034 — highway shoulder — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 034.** Let a practical question about “highway shoulder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 034, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 034 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 034, conditional return vignette, “Counting by Touch” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 034, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “highway shoulder” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 034, conditional return vignette, return to “highway shoulder” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 35: Counting by Touch × salt

**Beat question:** What can the writer say about “salt” during “Counting by Touch” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 035 — salt — Counting by Touch

For “Counting by Touch” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 035.** Put “salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 035, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 035 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, scene draft, “Counting by Touch” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “salt” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, scene draft, return to “salt” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 035 — salt — Counting by Touch

This proposed field-note fragment, beat 035 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 035.** Let a practical question about “salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 035, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 035 gives A companion who is learning to wait a distinct perspective on “salt” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, field-note fragment, “Counting by Touch” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “salt” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, field-note fragment, return to “salt” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 035 — salt — Counting by Touch

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “salt” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 035.** End the passage one sentence earlier than instinct suggests. Keep “salt” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 035, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 035 gives A traveler negotiating carefully a distinct perspective on “salt” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conversation fragment, “Counting by Touch” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 035, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “salt” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conversation fragment, return to “salt” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 035 — salt — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 035.** Begin after the first response rather than at arrival. Let the reader encounter “salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 035, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 035 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 035, conditional return vignette, “Counting by Touch” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 035, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “salt” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 035, conditional return vignette, return to “salt” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 36: Counting by Touch × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “Counting by Touch” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 036 — two rusted tins of beans — Counting by Touch

For “Counting by Touch” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 036.** End the passage one sentence earlier than instinct suggests. Keep “two rusted tins of beans” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 036, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 036 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, scene draft, “Counting by Touch” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “two rusted tins of beans” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, scene draft, return to “two rusted tins of beans” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 036 — two rusted tins of beans — Counting by Touch

This proposed field-note fragment, beat 036 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 036.** Begin after the first response rather than at arrival. Let the reader encounter “two rusted tins of beans” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 036, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 036 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, field-note fragment, “Counting by Touch” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “two rusted tins of beans” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, field-note fragment, return to “two rusted tins of beans” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 036 — two rusted tins of beans — Counting by Touch

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “two rusted tins of beans” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 036.** Leave one full beat of silence after “two rusted tins of beans.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 036, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 036 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conversation fragment, “Counting by Touch” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 036, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “two rusted tins of beans” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conversation fragment, return to “two rusted tins of beans” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 036 — two rusted tins of beans — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 036.** Put “two rusted tins of beans” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 036, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 036 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 036, conditional return vignette, “Counting by Touch” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 036, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “two rusted tins of beans” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 036, conditional return vignette, return to “two rusted tins of beans” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 37: Counting by Touch × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “Counting by Touch” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 037 — working flashlight — Counting by Touch

For “Counting by Touch” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 037.** Leave one full beat of silence after “working flashlight.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 037, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 037 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, scene draft, “Counting by Touch” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “working flashlight” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, scene draft, return to “working flashlight” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 037 — working flashlight — Counting by Touch

This proposed field-note fragment, beat 037 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 037.** Put “working flashlight” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 037, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 037 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, field-note fragment, “Counting by Touch” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “working flashlight” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, field-note fragment, return to “working flashlight” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 037 — working flashlight — Counting by Touch

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “working flashlight” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 037.** Let a practical question about “working flashlight” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 037, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 037 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conversation fragment, “Counting by Touch” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 037, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “working flashlight” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conversation fragment, return to “working flashlight” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 037 — working flashlight — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 037.** End the passage one sentence earlier than instinct suggests. Keep “working flashlight” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 037, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 037 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 037, conditional return vignette, “Counting by Touch” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 037, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “working flashlight” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 037, conditional return vignette, return to “working flashlight” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 38: Counting by Touch × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “Counting by Touch” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 038 — medical supplies — Counting by Touch

For “Counting by Touch” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 038.** Let a practical question about “medical supplies” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 038, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 038 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, scene draft, “Counting by Touch” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “medical supplies” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, scene draft, return to “medical supplies” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 038 — medical supplies — Counting by Touch

This proposed field-note fragment, beat 038 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 038.** End the passage one sentence earlier than instinct suggests. Keep “medical supplies” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 038, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 038 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, field-note fragment, “Counting by Touch” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “medical supplies” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, field-note fragment, return to “medical supplies” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 038 — medical supplies — Counting by Touch

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “medical supplies” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 038.** Begin after the first response rather than at arrival. Let the reader encounter “medical supplies” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 038, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 038 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conversation fragment, “Counting by Touch” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 038, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “medical supplies” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conversation fragment, return to “medical supplies” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 038 — medical supplies — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 038.** Leave one full beat of silence after “medical supplies.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 038, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 038 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 038, conditional return vignette, “Counting by Touch” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 038, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “medical supplies” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 038, conditional return vignette, return to “medical supplies” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 39: Counting by Touch × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “Counting by Touch” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 039 — does not offer her name — Counting by Touch

For “Counting by Touch” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 039.** Begin after the first response rather than at arrival. Let the reader encounter “does not offer her name” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 039, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 039 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, scene draft, “Counting by Touch” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “does not offer her name” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, scene draft, return to “does not offer her name” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 039 — does not offer her name — Counting by Touch

This proposed field-note fragment, beat 039 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 039.** Leave one full beat of silence after “does not offer her name.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 039, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 039 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, field-note fragment, “Counting by Touch” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “does not offer her name” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, field-note fragment, return to “does not offer her name” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 039 — does not offer her name — Counting by Touch

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “does not offer her name” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 039.** Put “does not offer her name” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 039, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 039 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conversation fragment, “Counting by Touch” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 039, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “does not offer her name” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conversation fragment, return to “does not offer her name” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 039 — does not offer her name — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 039.** Let a practical question about “does not offer her name” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 039, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 039 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 039, conditional return vignette, “Counting by Touch” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 039, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “does not offer her name” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 039, conditional return vignette, return to “does not offer her name” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 40: Counting by Touch × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “Counting by Touch” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: Which narrator assumption can be removed so the counting stays hers?

#### Scene draft 040 — counts by touch — Counting by Touch

For “Counting by Touch” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 040.** Put “counts by touch” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 040, “Counting by Touch” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 040 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, scene draft, “Counting by Touch” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, scene draft, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “counts by touch” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, scene draft, return to “counts by touch” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 040 — counts by touch — Counting by Touch

This proposed field-note fragment, beat 040 in “Counting by Touch,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 040.** Let a practical question about “counts by touch” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 040, “Counting by Touch” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 040 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “Counting by Touch.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, field-note fragment, “Counting by Touch” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, field-note fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “counts by touch” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, field-note fragment, return to “counts by touch” during “Counting by Touch” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 040 — counts by touch — Counting by Touch

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “counts by touch” during “Counting by Touch,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 040.** End the passage one sentence earlier than instinct suggests. Keep “counts by touch” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 040, “Counting by Touch” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 040 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “Counting by Touch.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conversation fragment, “Counting by Touch” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 040, conversation fragment, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “counts by touch” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conversation fragment, return to “counts by touch” during “Counting by Touch” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 040 — counts by touch — Counting by Touch

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “Counting by Touch” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 040.** Begin after the first response rather than at arrival. Let the reader encounter “counts by touch” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 040, “Counting by Touch” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 040 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Counting by Touch.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 040, conditional return vignette, “Counting by Touch” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 040, conditional return vignette, use the question—“Which narrator assumption can be removed so the counting stays hers?”—as a revision test tied to “counts by touch” during “Counting by Touch.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 040, conditional return vignette, return to “counts by touch” during “Counting by Touch” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Counting by Touch” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 41: Terms End Where She Says × wooden cart

**Beat question:** What can the writer say about “wooden cart” during “Terms End Where She Says” while preserving this limit: a working object that marks the encounter as trade. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 041 — wooden cart — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “wooden cart,” the candidate passage attends to A working object that marks the encounter as trade. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 041.** Leave one full beat of silence after “wooden cart.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 041, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The scene draft for beat 041 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, scene draft, “Terms End Where She Says” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “wooden cart” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, scene draft, return to “wooden cart” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 041 — wooden cart — Terms End Where She Says

This proposed field-note fragment, beat 041 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “wooden cart” is the point of return. A working object that marks the encounter as trade. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 041.** Put “wooden cart” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 041, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 041 gives A traveler negotiating carefully a distinct perspective on “wooden cart” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, field-note fragment, “Terms End Where She Says” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “wooden cart” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, field-note fragment, return to “wooden cart” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 041 — wooden cart — Terms End Where She Says

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “wooden cart” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 041.** Let a practical question about “wooden cart” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 041, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 041 gives The trader, in optional proposed dialogue a distinct perspective on “wooden cart” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conversation fragment, “Terms End Where She Says” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 041, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “wooden cart” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conversation fragment, return to “wooden cart” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 041 — wooden cart — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “wooden cart” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 041.** End the passage one sentence earlier than instinct suggests. Keep “wooden cart” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 041, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “wooden cart” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 041 gives A companion who is learning to wait a distinct perspective on “wooden cart” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 041, conditional return vignette, “Terms End Where She Says” × “wooden cart,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 041, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “wooden cart” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 041, conditional return vignette, return to “wooden cart” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “wooden cart.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 42: Terms End Where She Says × highway shoulder

**Beat question:** What can the writer say about “highway shoulder” during “Terms End Where She Says” while preserving this limit: a narrow place for negotiation, not a guaranteed safe market. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 042 — highway shoulder — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “highway shoulder,” the candidate passage attends to A narrow place for negotiation, not a guaranteed safe market. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 042.** Let a practical question about “highway shoulder” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 042, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The scene draft for beat 042 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, scene draft, “Terms End Where She Says” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “highway shoulder” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, scene draft, return to “highway shoulder” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 042 — highway shoulder — Terms End Where She Says

This proposed field-note fragment, beat 042 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “highway shoulder” is the point of return. A narrow place for negotiation, not a guaranteed safe market. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 042.** End the passage one sentence earlier than instinct suggests. Keep “highway shoulder” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 042, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 042 gives A companion who is learning to wait a distinct perspective on “highway shoulder” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, field-note fragment, “Terms End Where She Says” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “highway shoulder” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, field-note fragment, return to “highway shoulder” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 042 — highway shoulder — Terms End Where She Says

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “highway shoulder” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 042.** Begin after the first response rather than at arrival. Let the reader encounter “highway shoulder” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 042, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 042 gives A traveler negotiating carefully a distinct perspective on “highway shoulder” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conversation fragment, “Terms End Where She Says” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 042, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “highway shoulder” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conversation fragment, return to “highway shoulder” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 042 — highway shoulder — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “highway shoulder” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 042.** Leave one full beat of silence after “highway shoulder.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 042, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “highway shoulder” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 042 gives The trader, in optional proposed dialogue a distinct perspective on “highway shoulder” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 042, conditional return vignette, “Terms End Where She Says” × “highway shoulder,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 042, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “highway shoulder” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 042, conditional return vignette, return to “highway shoulder” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “highway shoulder.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 43: Terms End Where She Says × salt

**Beat question:** What can the writer say about “salt” during “Terms End Where She Says” while preserving this limit: a named good without quantity or use invented here. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 043 — salt — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “salt,” the candidate passage attends to A named good without quantity or use invented here. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 043.** Begin after the first response rather than at arrival. Let the reader encounter “salt” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 043, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The scene draft for beat 043 gives A traveler negotiating carefully a distinct perspective on “salt” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, scene draft, “Terms End Where She Says” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “salt” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, scene draft, return to “salt” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 043 — salt — Terms End Where She Says

This proposed field-note fragment, beat 043 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “salt” is the point of return. A named good without quantity or use invented here. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 043.** Leave one full beat of silence after “salt.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 043, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 043 gives The trader, in optional proposed dialogue a distinct perspective on “salt” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, field-note fragment, “Terms End Where She Says” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “salt” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, field-note fragment, return to “salt” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 043 — salt — Terms End Where She Says

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “salt” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 043.** Put “salt” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 043, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 043 gives A companion who is learning to wait a distinct perspective on “salt” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conversation fragment, “Terms End Where She Says” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 043, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “salt” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conversation fragment, return to “salt” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 043 — salt — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “salt” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 043.** Let a practical question about “salt” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 043, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “salt” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 043 gives A traveler negotiating carefully a distinct perspective on “salt” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 043, conditional return vignette, “Terms End Where She Says” × “salt,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 043, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “salt” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 043, conditional return vignette, return to “salt” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “salt.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 44: Terms End Where She Says × two rusted tins of beans

**Beat question:** What can the writer say about “two rusted tins of beans” during “Terms End Where She Says” while preserving this limit: an exact count and condition; nothing about contents is added. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 044 — two rusted tins of beans — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “two rusted tins of beans,” the candidate passage attends to An exact count and condition; nothing about contents is added. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 044.** Put “two rusted tins of beans” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 044, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The scene draft for beat 044 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, scene draft, “Terms End Where She Says” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “two rusted tins of beans” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, scene draft, return to “two rusted tins of beans” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 044 — two rusted tins of beans — Terms End Where She Says

This proposed field-note fragment, beat 044 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “two rusted tins of beans” is the point of return. An exact count and condition; nothing about contents is added. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 044.** Let a practical question about “two rusted tins of beans” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 044, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 044 gives A traveler negotiating carefully a distinct perspective on “two rusted tins of beans” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, field-note fragment, “Terms End Where She Says” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “two rusted tins of beans” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, field-note fragment, return to “two rusted tins of beans” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 044 — two rusted tins of beans — Terms End Where She Says

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “two rusted tins of beans” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 044.** End the passage one sentence earlier than instinct suggests. Keep “two rusted tins of beans” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 044, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 044 gives The trader, in optional proposed dialogue a distinct perspective on “two rusted tins of beans” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conversation fragment, “Terms End Where She Says” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 044, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “two rusted tins of beans” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conversation fragment, return to “two rusted tins of beans” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 044 — two rusted tins of beans — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “two rusted tins of beans” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 044.** Begin after the first response rather than at arrival. Let the reader encounter “two rusted tins of beans” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 044, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “two rusted tins of beans” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 044 gives A companion who is learning to wait a distinct perspective on “two rusted tins of beans” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 044, conditional return vignette, “Terms End Where She Says” × “two rusted tins of beans,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 044, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “two rusted tins of beans” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 044, conditional return vignette, return to “two rusted tins of beans” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “two rusted tins of beans.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 45: Terms End Where She Says × working flashlight

**Beat question:** What can the writer say about “working flashlight” during “Terms End Where She Says” while preserving this limit: a functional object, not a promise of batteries or future safety. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 045 — working flashlight — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “working flashlight,” the candidate passage attends to A functional object, not a promise of batteries or future safety. The present action begins small: the flashlight left where both parties can identify it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 045.** End the passage one sentence earlier than instinct suggests. Keep “working flashlight” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 045, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The scene draft for beat 045 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, scene draft, “Terms End Where She Says” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “working flashlight” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, scene draft, return to “working flashlight” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 045 — working flashlight — Terms End Where She Says

This proposed field-note fragment, beat 045 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “working flashlight” is the point of return. A functional object, not a promise of batteries or future safety. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 045.** Begin after the first response rather than at arrival. Let the reader encounter “working flashlight” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 045, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 045 gives A companion who is learning to wait a distinct perspective on “working flashlight” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, field-note fragment, “Terms End Where She Says” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “working flashlight” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, field-note fragment, return to “working flashlight” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 045 — working flashlight — Terms End Where She Says

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “working flashlight” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 045.** Leave one full beat of silence after “working flashlight.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 045, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 045 gives A traveler negotiating carefully a distinct perspective on “working flashlight” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conversation fragment, “Terms End Where She Says” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 045, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “working flashlight” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conversation fragment, return to “working flashlight” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 045 — working flashlight — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “working flashlight” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 045.** Put “working flashlight” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 045, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “working flashlight” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 045 gives The trader, in optional proposed dialogue a distinct perspective on “working flashlight” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 045, conditional return vignette, “Terms End Where She Says” × “working flashlight,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 045, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “working flashlight” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 045, conditional return vignette, return to “working flashlight” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “working flashlight.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 46: Terms End Where She Says × medical supplies

**Beat question:** What can the writer say about “medical supplies” during “Terms End Where She Says” while preserving this limit: a broad request whose intended use remains unknown. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 046 — medical supplies — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “medical supplies,” the candidate passage attends to A broad request whose intended use remains unknown. The present action begins small: a count repeated aloud only if the trader asks for it. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 046.** Leave one full beat of silence after “medical supplies.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 046, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The scene draft for beat 046 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, scene draft, “Terms End Where She Says” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “medical supplies” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, scene draft, return to “medical supplies” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 046 — medical supplies — Terms End Where She Says

This proposed field-note fragment, beat 046 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “medical supplies” is the point of return. A broad request whose intended use remains unknown. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 046.** Put “medical supplies” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 046, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 046 gives The trader, in optional proposed dialogue a distinct perspective on “medical supplies” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, field-note fragment, “Terms End Where She Says” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “medical supplies” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, field-note fragment, return to “medical supplies” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 046 — medical supplies — Terms End Where She Says

The proposed exchange gives a companion who is learning to wait a distinct reason to speak. Its authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” The talk concerns “medical supplies” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 046.** Let a practical question about “medical supplies” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 046, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 046 gives A companion who is learning to wait a distinct perspective on “medical supplies” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conversation fragment, “Terms End Where She Says” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “That is my offer. My name is not part of it.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 046, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “medical supplies” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conversation fragment, return to “medical supplies” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 046 — medical supplies — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “medical supplies” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 046.** End the passage one sentence earlier than instinct suggests. Keep “medical supplies” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 046, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “medical supplies” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 046 gives A traveler negotiating carefully a distinct perspective on “medical supplies” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 046, conditional return vignette, “Terms End Where She Says” × “medical supplies,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 046, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “medical supplies” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 046, conditional return vignette, return to “medical supplies” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “medical supplies.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 47: Terms End Where She Says × does not offer her name

**Beat question:** What can the writer say about “does not offer her name” during “Terms End Where She Says” while preserving this limit: a chosen limit on what the encounter makes public. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 047 — does not offer her name — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “does not offer her name,” the candidate passage attends to A chosen limit on what the encounter makes public. The present action begins small: goods set down one at a time only with permission. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 047.** Let a practical question about “does not offer her name” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 047, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The scene draft for beat 047 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, scene draft, “Terms End Where She Says” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “does not offer her name” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, scene draft, return to “does not offer her name” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 047 — does not offer her name — Terms End Where She Says

This proposed field-note fragment, beat 047 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “does not offer her name” is the point of return. A chosen limit on what the encounter makes public. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 047.** End the passage one sentence earlier than instinct suggests. Keep “does not offer her name” unresolved and give the player a natural exit. More explanation would make this beat less honest, not more complete.

The factual floor for beat 047, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 047 gives A traveler negotiating carefully a distinct perspective on “does not offer her name” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, field-note fragment, “Terms End Where She Says” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “does not offer her name” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, field-note fragment, return to “does not offer her name” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 047 — does not offer her name — Terms End Where She Says

The proposed exchange gives the trader, in optional proposed dialogue a distinct reason to speak. Its authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” The talk concerns “does not offer her name” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 047.** Begin after the first response rather than at arrival. Let the reader encounter “does not offer her name” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 047, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 047 gives The trader, in optional proposed dialogue a distinct perspective on “does not offer her name” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conversation fragment, “Terms End Where She Says” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “Tell me which item you want moved. I can count what I brought.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 047, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “does not offer her name” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conversation fragment, return to “does not offer her name” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 047 — does not offer her name — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “does not offer her name” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 047.** Leave one full beat of silence after “does not offer her name.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 047, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “does not offer her name” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 047 gives A companion who is learning to wait a distinct perspective on “does not offer her name” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 047, conditional return vignette, “Terms End Where She Says” × “does not offer her name,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 047, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “does not offer her name” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 047, conditional return vignette, return to “does not offer her name” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “does not offer her name.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

### Beat 48: Terms End Where She Says × counts by touch

**Beat question:** What can the writer say about “counts by touch” during “Terms End Where She Says” while preserving this limit: a capability shown in the source; not a cue for another character to take over. The larger movement question is: How can the final line leave the trader’s next move unclaimed?

#### Scene draft 048 — counts by touch — Terms End Where She Says

For “Terms End Where She Says” and the source phrase “counts by touch,” the candidate passage attends to A capability shown in the source; not a cue for another character to take over. The present action begins small: an empty space beside the cart where a refused offer can remain refused. The source gives the writer enough to set a boundary and not enough to fill every silence. Keep the proposed scene close to what a traveler could notice at this encounter; do not supply an unseen witness, a private history, or a result the record never states.

**Pacing variant for scene draft beat 048.** Begin after the first response rather than at arrival. Let the reader encounter “counts by touch” through another voice’s correction, then return to the source wording. This changes the order of disclosure only; it does not add a witness or reinterpret the catalog as a transcript.

The factual floor for beat 048, “Terms End Where She Says” scene draft, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The scene draft for beat 048 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, scene draft, “Terms End Where She Says” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, scene draft, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “counts by touch” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, scene draft, return to “counts by touch” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This scene draft belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Field-note fragment 048 — counts by touch — Terms End Where She Says

This proposed field-note fragment, beat 048 in “Terms End Where She Says,” is written after the encounter in a traveler’s plain campaign notebook. It is not an existing catalog item or source-authored document. “counts by touch” is the point of return. A capability shown in the source; not a cue for another character to take over. Let the note record an observation, an uncertainty, and the part the writer elects not to claim.

**Pacing variant for field-note fragment beat 048.** Leave one full beat of silence after “counts by touch.” The pause should allow the player to stop reading without being punished by a cliffhanger, a timer, a blocked control, or an implied failure. A quiet scene can end cleanly without explaining the silence.

The factual floor for beat 048, “Terms End Where She Says” field-note fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The field-note fragment for beat 048 gives A companion who is learning to wait a distinct perspective on “counts by touch” during “Terms End Where She Says.” The optional authoring note is: “Notices the impulse to assist and lets the trader direct the exchange instead.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, field-note fragment, “Terms End Where She Says” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “If your supplies do not match the request, say so before we trade.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, field-note fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “counts by touch” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, field-note fragment, return to “counts by touch” during “Terms End Where She Says” in a changed register: “Tell me which item you want moved. I can count what I brought.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This field-note fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conversation fragment 048 — counts by touch — Terms End Where She Says

The proposed exchange gives a traveler negotiating carefully a distinct reason to speak. Its authoring note is: “States what they have and asks before moving any goods.” The talk concerns “counts by touch” during “Terms End Where She Says,” but no line is presented as a recovered quotation from the JSON. Every quoted sentence here is new editorial dialogue and can be discarded if the chosen content form cannot preserve attribution.

**Pacing variant for conversation fragment beat 048.** Put “counts by touch” at the end of the first sentence so the source detail lands before anyone comments on it. Avoid a musical sting, camera instruction, or interface cue unless the actual content owner already provides one.

The factual floor for beat 048, “Terms End Where She Says” conversation fragment, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conversation fragment for beat 048 gives A traveler negotiating carefully a distinct perspective on “counts by touch” during “Terms End Where She Says.” The optional authoring note is: “States what they have and asks before moving any goods.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conversation fragment, “Terms End Where She Says” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “Tell me which item you want moved. I can count what I brought.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence. A second proposed voice answers, “If your supplies do not match the request, say so before we trade.” The exchange may disagree, but it cannot settle what the source leaves open.

For beat 048, conversation fragment, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “counts by touch” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conversation fragment, return to “counts by touch” during “Terms End Where She Says” in a changed register: “That is my offer. My name is not part of it.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conversation fragment belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

#### Conditional return vignette 048 — counts by touch — Terms End Where She Says

This conditional return is a later prose possibility, not a new event flag or guaranteed callback. It remembers “counts by touch” through “Terms End Where She Says” only if an existing narrative owner already has a truthful reason to revisit this text. No new relationship, inventory, faction, route, or save state follows from the vignette.

**Pacing variant for conditional return vignette beat 048.** Let a practical question about “counts by touch” interrupt the reflective line. The interruption grounds the exchange in this specific encounter; it must not smuggle in another system or an unverified task objective.

The factual floor for beat 048, “Terms End Where She Says” conditional return vignette, is narrow. The local description says: “A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind.” Treat that sentence as the source record, not as a complete account of causes, motives, aftermath, or what another person knows. In this beat, the phrase “counts by touch” can receive attention while the rest of the scene remains open.

The conditional return vignette for beat 048 gives The trader, in optional proposed dialogue a distinct perspective on “counts by touch” during “Terms End Where She Says.” The optional authoring note is: “Sets boundaries and can refuse a term; her voice is not supplied by the source and should remain revisable.” A performance can show that through the order of attention: first the observed detail, then a pause, then the decision whether to continue speaking. Do not insert explanatory narration to assure the player that the emotion has been understood.

For beat 048, conditional return vignette, “Terms End Where She Says” × “counts by touch,” a possible line, offered as newly authored dialogue rather than canon, is: “That is my offer. My name is not part of it.” Keep it only if the proposed speaker could know the detail and if the surrounding text identifies that speaker. A shorter silence may be more truthful than the sentence.

For beat 048, conditional return vignette, use the question—“How can the final line leave the trader’s next move unclaimed?”—as a revision test tied to “counts by touch” during “Terms End Where She Says.” The passage should make its answer visible in who gets to decide, who remains unnamed, and what the narrator refuses to know. It should not answer by adding a mechanic, inventing an outcome, or making the player perform an authorial argument.

At the close of beat 048, conditional return vignette, return to “counts by touch” during “Terms End Where She Says” in a changed register: “If your supplies do not match the request, say so before we trade.” The line is a second optional candidate, not a required response. End on a physical limit or an unfinished sentence so the encounter remains human rather than turning into a thesis statement.

**Selection note.** This conditional return vignette belongs in the bank only if it gives “Terms End Where She Says” a different dramatic job from the adjacent beats. Keep the anchor exact, keep interpretation attributable, and cut any clause that assumes a fact beyond “counts by touch.” A brief, honest passage is stronger than a fuller passage that closes the source’s open question.

## 13. Tone and performance

Keep the register restrained and physically grounded. For `enc_roadside_trader`, let the object, sound, gesture, or stated choice carry emotion without narration telling the player what the scene means. The source wording controls factual claims; proposed dialogue remains visibly authored. No draft should turn an uncertain situation into a suspense puzzle whose solution is withheld for engagement.

## 14. Editorial acceptance

- Re-open the current source record before any later use and preserve its source-owned fields.
- Keep the four passage forms optional; choose only text that a verified existing owner can attribute and present honestly.
- Do not introduce a new route, choice, flag, save section, mechanic, catalog authority, or interface through prose planning.
- Mark invented dialogue and staging as editorial until a content owner accepts them.
- Remove any sentence that implies an outcome, identity, motive, location detail, or procedure missing from the source.
- Preserve silence, refusal, and departure as complete dramatic outcomes.

## 15. Passage selection guide

**Scene draft:** use when the encounter owner can support a present-tense observation without false environmental claims. **Field-note fragment:** use only as an explicitly proposed traveler-authored line; the source record has no such note unless stated above. **Conversation fragment:** use only with clear speaker attribution and no invented testimony. **Conditional return vignette:** retain only if a current route already revisits the relevant content; otherwise it remains an editorial exercise. None of these forms changes game state.

## 16. Continuity and reuse

The proposal is local to `enc_roadside_trader` and should not be reused as generic dialogue for other encounters. If another record shares a motif such as radio silence, a locked threshold, trade, empty transport, or uncertainty, write new lines against that record’s own facts. For the pianist, resolve the same-ID description conflict before any integration; do not borrow from the separate expansion variant.

## 17. Limits and open questions

| Concern | Evidence in the source | Limit for this plan |
|---|---|---|
| Record | `enc_roadside_trader` in `Assets/StreamingAssets/Data/narrative_encounters_expansion.json` | Catalog presence does not by itself show where prose is presented. |
| Description | A woman has pulled a wooden cart onto the highway shoulder. She has salt, two rusted tins of beans, and a working flashlight. She asks for medical supplies. She does not offer her name, and counts the barter goods entirely by touch. She is totally blind. | No unstated biography, cause, aftermath, or outcome. |
| Voices | The listed encounter description and choice labels | Candidate dialogue remains editorial and attributable. |
| Runtime path | Current loader filenames and host registration | Static scanner mapping alone is not runtime evidence. |
| Player response | Existing source choice list above | No new state or ideal-morality claim. |

**Boundary review:** Apply the encounter-specific limits in Section 4 to every proposed voice, staging detail, and return. Keep the boundary visible during selection without adding another source claim.

## 18. Local-canon and collision audit

The exact source anchor was searched against previous `docs/expansions/prose_wave*` anchor labels before drafting. The selected IDs are distinct across this batch. The pianist’s ID collision is stated in its source note and remains unresolved; all other plans use their exact distinct expansion records without asserting loadability. This is a documentation-level novelty check, not a claim that related themes do not exist elsewhere in ASHFALL.

## 19. Handoff and acceptance

**Deliverable:** an optional prose bank for `enc_roadside_trader` with a strict source boundary and authoring rationale. **Accepted scope:** content planning only. **Files to revisit if a later prose integration is approved:** `Assets/StreamingAssets/Data/narrative_encounters_expansion.json`, `Assets/Ashfall.Core/Narrative/NarrativeEncounterSystem.cs`, and the current host/content presentation owner identified by fresh inspection. The plan does not claim any runtime change or require a production-code edit.

This document is a game-content prose expansion plan. It is not an implementation plan for new features, and its candidate drafts are not yet canon or confirmed playable text.